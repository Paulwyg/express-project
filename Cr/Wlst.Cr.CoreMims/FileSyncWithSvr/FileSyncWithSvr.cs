using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Wlst.Cr.CoreMims.Services;
using Wlst.client;

namespace Wlst.Cr.CoreMims.FileSyncWithSvr
{
    public class FileSyncWithSvr
    {
        private static bool _init = false;

        public void InitAciotn()
        {
            if (_init) return;
            _init = true;
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSys.wst_sys_file_lst,
                OnReqFileLst,
                typeof (FileSyncWithSvr), this);


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSys.wst_sys_file_brief_info,
                OnReqFileBriefInfo,
                typeof (FileSyncWithSvr), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSys.wst_sys_request_file,
                OnReqFileInfo
                ,
                typeof (FileSyncWithSvr), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSys.wst_sys_request_file_back,
                OnReqFileInfoBack
                ,
                typeof (FileSyncWithSvr), this);

            //注册调度函数
            Wlst.Cr.Coreb.Servers.QtzLp.AddQtz("nt", 0, DateTime.Now.AddMinutes(5).Ticks, 24 * 60 * 60,
                                               OnInit);
            Wlst.Cr.Coreb.Servers.QtzLp.AddQtz("nt", 0, DateTime.Now.AddMinutes(5).Ticks, 5,
                                               OnOnReqFileInfoBackWrInThrd);

            //if (DirFileNeed.Contains(@"D:\CETC50\FileSync\SingleLamp") == false)
            //    DirFileNeed.Add(@"D:\CETC50\FileSync\SingleLamp");
            SetNeedShareFileDir(@"D:\CETC50\FileSync\SingleLamp");
        }

        private static List<string> DirFileNeed = new List<string>();

        public static void SetNeedShareFileDir(string path)
        {
            if (DirFileNeed.Contains(path)) return;
            DirFileNeed.Add(path);
        }

        private void OnInit(object obj)
        {

            if (DirFileNeed.Count == 0) return;

            foreach (var f in DirFileNeed)
            {
                var nt = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_file_lst;
                nt.WstSysFileInfoLst.Op = 1;
                nt.WstSysFileInfoLst.FileDir = f;


                SndOrderServer.OrderSnd(nt);
            }
        }

        /// 请求文件列表
        private void OnReqFileLst(string session, Wlst.mobile.MsgWithMobile obj)
        {
            // var pe = obj as ProtocolEncoding;
            if (obj == null || obj.WstSysFileInfoLst == null)
                return;
            if (obj.WstSysFileInfoLst.Op == 1) //客户端请求
            {
                var nt = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_file_lst;
                nt.WstSysFileInfoLst.Op = 2;

                if (System.IO.Directory.Exists(obj.WstSysFileInfoLst.FileDir) == false)
                    System.IO.Directory.CreateDirectory(obj.WstSysFileInfoLst.FileDir);


                //在指定目录及子目录下查找文件,在list中列出子目录及文件
                DirectoryInfo Dir = new DirectoryInfo(obj.WstSysFileInfoLst.FileDir);
                nt.WstSysFileInfoLst.FileDir = obj.WstSysFileInfoLst.FileDir; //20180123
                foreach (FileInfo f in Dir.GetFiles()) //查找文件
                {
                    if (f.Name.Contains("EnsureThisFileHasContent_xx"))
                    {
                        if (f.Length == 0) return;
                    }
                    nt.WstSysFileInfoLst.FileFullNameInSrcPc.Add(f.FullName);
                }
                SndOrderServer.OrderSnd(nt);
            }
            else if (obj.WstSysFileInfoLst.Op == 2) //服务器端请求 应答
            {
                if (System.IO.Directory.Exists(obj.WstSysFileInfoLst.FileDir) == false)
                    System.IO.Directory.CreateDirectory(obj.WstSysFileInfoLst.FileDir);

                long time = 0;
                //在指定目录及子目录下查找文件,在list中列出子目录及文件
                DirectoryInfo Dir = new DirectoryInfo(obj.WstSysFileInfoLst.FileDir);
                foreach (FileInfo f in Dir.GetFiles()) //查找文件
                {
                    if (f.Name.Contains("EnsureThisFileHasContent_xx"))
                    {
                        var spr = f.Name.Replace(".txt", "").Replace("EnsureThisFileHasContent_xx", "");
                        Int64.TryParse(spr, out time);

                        
                    }
                }
                long time1 = 0;
                foreach (var f in obj.WstSysFileInfoLst.FileFullNameInSrcPc)
                {
                    if (f.Contains("EnsureThisFileHasContent_xx"))
                    {
                        var sp = f.Substring(f.IndexOf("EnsureThisFileHasContent_xx"));
                        var spr = sp.Replace(".txt", "").Replace("EnsureThisFileHasContent_xx", "");
                        Int64.TryParse(spr, out time1);
                    }
                }
                if (time1 > time)
                {
                    //load
                    var files = (from t in Dir.GetFiles() select t.FullName).ToList();
                    foreach (var f in files)
                    {
                        if (File.Exists(f)) File.Delete(f);
                    }

                    foreach (var f in obj.WstSysFileInfoLst.FileFullNameInSrcPc)
                    {

                        if (_data.ContainsKey(f)) continue;
                        _data.TryAdd(f, new List<Tuple<int, int>>());

                        //注册时间  每个5分钟检测一下 是否接受完毕 接受完毕则清理
                        Wlst.Cr.Coreb.Servers.QtzLp.AddQtz("nt", 0, DateTime.Now.AddMinutes(5).Ticks,
                                                           OnLoadOver,
                                                           180, f, 6);
                        //间隔35分钟执行清理操作 即清除队列
                        Wlst.Cr.Coreb.Servers.QtzLp.AddQtz("nt", 0, DateTime.Now.AddMinutes(24).Ticks,
                                                           OnLoadOver1,
                                                           10, f, 1);


                        var nt = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_file_brief_info;
                        nt.WstSysFileBiefInfo.Op = 1;
                        nt.WstSysFileBiefInfo.FileFullNameInSrcPc = f;
                        nt.WstSysFileBiefInfo.Length = 0;
                        SndOrderServer.OrderSnd(nt);
                    }

                }
            }
        }


        ///  请求文件数据
        private void OnReqFileInfo(string session, Wlst.mobile.MsgWithMobile obj)
        {
            // var pe = obj as ProtocolEncoding;
            if (obj == null || obj.WstSysRequestFile == null || obj.WstSysRequestFile.RquPackages.Count == 0)
                return;
            if (System.IO.File.Exists(obj.WstSysRequestFile.FileFullNameInSrcPc) == false) return;
            try
            {
                using (var fs = new FileStream(obj.WstSysRequestFile.FileFullNameInSrcPc, System.IO.FileMode.Open))
                {
                    //var fs = new FileStream(obj.WstSysRequestFile.FileFullNameInSrcPc, System.IO.FileMode.Open);
                    // var br = new BinaryReader((Stream)fs);
                    int length = (int) fs.Length;
                    int len1 = obj.WstSysRequestFile.PackageLength;

                    foreach (var g in obj.WstSysRequestFile.RquPackages)
                    {
                        var nt = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_request_file_back;
                        nt.WstSysAnsRequestFile.FileFullNameInSrcPc = obj.WstSysRequestFile.FileFullNameInSrcPc;
                        nt.WstSysAnsRequestFile.Op = 1;
                        nt.WstSysAnsRequestFile.PackageLength = len1;
                        nt.WstSysAnsRequestFile.PackageSum = obj.WstSysRequestFile.PackageSum;
                        nt.WstSysAnsRequestFile.CurrentPackageIndex = g;



                        if (g*len1 >= length) nt.WstSysAnsRequestFile.Data = "";
                        else
                        {
                            var intLength = len1;

                            if (g*len1 + len1 > length)
                            {
                                intLength = length - g*len1;
                            }

                            var bytContent = new byte[intLength];
                            fs.Seek(g*len1, SeekOrigin.Begin);
                            fs.Read(bytContent, 0, bytContent.Length);
                            var snd = System.Convert.ToBase64String(bytContent);
                            nt.WstSysAnsRequestFile.Data = snd;
                        }

                        SndOrderServer.OrderSnd(nt);
                        Thread.Sleep(3);
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("发送数据 OnReqFileInfo 出错");
            }


        }


        /// 请求文件数据 返回
        private void OnReqFileInfoBack(string session, Wlst.mobile.MsgWithMobile obj)
        {

            if (obj == null || obj.WstSysAnsRequestFile == null) return;

            if (_data.ContainsKey(obj.WstSysAnsRequestFile.FileFullNameInSrcPc) == false) return;

            var ntg = (from t in _data[obj.WstSysAnsRequestFile.FileFullNameInSrcPc] select t.Item2).ToList();
            bool find = ntg.Contains(obj.WstSysAnsRequestFile.CurrentPackageIndex);



            if (find) return;
            _data[obj.WstSysAnsRequestFile.FileFullNameInSrcPc].Add(
                new Tuple<int, int>(obj.WstSysAnsRequestFile.PackageSum, obj.WstSysAnsRequestFile.CurrentPackageIndex));

            queue.Enqueue(new Tuple<string, RquFileInfoBack>(obj.WstSysAnsRequestFile.FileFullNameInSrcPc,
                                                             obj.WstSysAnsRequestFile));

            //using (System.IO.FileStream stream = System.IO.File.OpenWrite(obj.WstSysAnsRequestFile.FileFullNameInSrcPc))
            //{
            //    stream.Seek((long)obj.WstSysAnsRequestFile.CurrentPackageIndex * (long)obj.WstSysAnsRequestFile.PackageLength, System.IO.SeekOrigin.Begin);
            //    var buf = System.Convert.FromBase64String(obj.WstSysAnsRequestFile.Data);
            //    stream.Write(buf, 0, buf.Length);
            //    stream.Flush();
            //}
            //if (_data[obj.WstSysAnsRequestFile.FileFullNameInSrcPc][0].Item1 == _data[obj.WstSysAnsRequestFile.FileFullNameInSrcPc].Count)
            //{
            //    _data[obj.WstSysAnsRequestFile.FileFullNameInSrcPc].Clear();
            //    List<Tuple<int, int>> tmp = null;
            //    _data.TryRemove(obj.WstSysAnsRequestFile.FileFullNameInSrcPc, out tmp);
            //}

        }

        private ConcurrentQueue<Tuple<string, RquFileInfoBack>> queue =
            new ConcurrentQueue<Tuple<string, RquFileInfoBack>>();

        private void OnOnReqFileInfoBackWrInThrd(object obj)
        {
            Tuple<string, RquFileInfoBack> tmp = null;
            while (queue.TryDequeue(out tmp))
            {
                if (tmp == null) continue;
                if (_data.ContainsKey(tmp.Item1) == false) continue;
                using (System.IO.FileStream stream = System.IO.File.OpenWrite(tmp.Item1))
                {
                    stream.Seek(tmp.Item2.CurrentPackageIndex*tmp.Item2.PackageLength, System.IO.SeekOrigin.Begin);
                    var buf = System.Convert.FromBase64String(tmp.Item2.Data);
                    stream.Write(buf, 0, buf.Length);
                    stream.Flush();
                }
            }

            if (queue.Count == 0 && _data.Count > 0)
            {

                var ntr =
                    (from t in _data where t.Value.Count > 0 && t.Value[0].Item1 == t.Value.Count select t.Key).ToList();
                foreach (var f in ntr)
                {
                    _data[f].Clear();
                    List<Tuple<int, int>> tmpr = null;
                    _data.TryRemove(f, out tmpr);
                }
            }


        }

        private const int Packagelength = 8096; //一个包 8096字节  

        /// <summary>
        /// 请求文件信息 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private void OnReqFileBriefInfo(string session, Wlst.mobile.MsgWithMobile obj)
        {
            // var pe = obj as ProtocolEncoding;
            if (obj == null || obj.WstSysFileBiefInfo == null) return;
            if (obj.WstSysFileBiefInfo.Op == 1) //客户端下载
            {
                var nt = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_file_brief_info;
                nt.WstSysFileBiefInfo.FileFullNameInSrcPc = obj.WstSysFileBiefInfo.FileFullNameInSrcPc;
                nt.WstSysFileBiefInfo.Op = 2;
                nt.WstSysFileBiefInfo.Length = 0;
                if (System.IO.File.Exists(obj.WstSysFileBiefInfo.FileFullNameInSrcPc))
                {
                    using (
                        var fs = new FileStream(obj.WstSysFileBiefInfo.FileFullNameInSrcPc, System.IO.FileMode.Open))
                    {
                        nt.WstSysFileBiefInfo.Name = fs.Name;
                        nt.WstSysFileBiefInfo.Length = (int) fs.Length;
                    }

                }
                SndOrderServer.OrderSnd(nt);
                return;
            }
            if (obj.WstSysFileBiefInfo.Op == 2) //服务向客户端请求的  需要下载的本地的 
            {
                if (obj.WstSysFileBiefInfo.Length == 0) return; //该文件无法查阅

                int addOnePack = obj.WstSysFileBiefInfo.Length%8096 == 0 ? 0 : 1; //最后一个包 是否存在
                var nt = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_request_file; //发送给客户端请求协议
                nt.WstSysRequestFile.Op = 2; //标记 来自服务器下载事件
                nt.WstSysRequestFile.PackageLength = Packagelength;
                nt.WstSysRequestFile.PackageSum = (obj.WstSysFileBiefInfo.Length/Packagelength) + addOnePack;
                for (int i = 0; i < nt.WstSysRequestFile.PackageSum; i++) nt.WstSysRequestFile.RquPackages.Add(i);
                nt.WstSysRequestFile.FileFullNameInSrcPc = obj.WstSysFileBiefInfo.FileFullNameInSrcPc;

                SndOrderServer.OrderSnd(nt); //请求文件
            }


        }

        /// <summary>
        /// portinf - sessionid - fullName
        /// </summary>
        private ConcurrentDictionary<string, List<Tuple<int, int>>> _data =
            new ConcurrentDictionary<string, List<Tuple<int, int>>>();

        //请求数据包 每隔5分钟进行检测一次是否请求完毕   
        private void OnLoadOver(object obj)
        {
            if (obj == null) return;

            var idf = obj.ToString();


            if (_data.ContainsKey(idf) == false) return;

            if (_data.Count == 0) return; //也可能是请求还没开始


            //数据包未接收完毕 需要继续接收
            {
                var nt = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_request_file; //发送给客户端请求协议
                nt.WstSysRequestFile.Op = 2; //标记 来自服务器下载事件
                nt.WstSysRequestFile.PackageLength = Packagelength;
                nt.WstSysRequestFile.PackageSum = _data[idf][0].Item1;
                nt.WstSysRequestFile.FileFullNameInSrcPc = idf;

                var lst = (from t in _data[idf] select t.Item2).ToList();
                for (int i = 0; i < _data[idf][0].Item1; i++)
                {
                    if (lst.Contains(i)) continue;
                    nt.WstSysRequestFile.RquPackages.Add(i);
                }
                SndOrderServer.OrderSnd(nt); //请求文件
            }
        }


        //清理 
        private void OnLoadOver1(object obj)
        {
            if (obj == null) return;
            var idf = obj.ToString();


            if (_data.ContainsKey(idf) == false) return;
            List<Tuple<int, int>> tmp = null;
            _data.TryRemove(idf, out tmp);

            if (tmp == null || tmp.Count == 0) return;
            if (tmp.Count != tmp[0].Item1)
            {
                if (File.Exists(idf)) File.Delete(idf);
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("File down Faild :" + idf);
            }
        }


    }
}
