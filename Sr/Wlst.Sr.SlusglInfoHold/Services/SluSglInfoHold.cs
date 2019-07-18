using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.client;

namespace Wlst.Sr.SlusglInfoHold.Services
{
    public partial class SluSglInfoHold
    {
        #region single instance

        private static SluSglInfoHold _mySlef = null;
        private static object _obj = 1;

        public static SluSglInfoHold MySlef
        {
            get
            {
                if (_mySlef == null)
                {
                    lock (_obj)
                    {
                        if (_mySlef == null) _mySlef = new SluSglInfoHold();
                    }
                }
                return _mySlef;
            }
        }

        protected SluSglInfoHold()
        {
            this.InitAciotn();
        }

        #endregion


        public void OnInit()
        {
            long xxx = DateTime.Now.Ticks;
            var tmp = this.LoadRtufromXml();
            foreach (var t in tmp)
            {
                if (Info.ContainsKey(t.Key) == false) Info[t.Key] = t.Value;
                
            }

            ////lvf 记录条形码
            //foreach (var g in Info)
            //{

            //    var barCodelst = new List<long>();
            //    foreach (var t in g.Value.CtrlLst)
            //    {
            //        if (barCodelst.Contains(t.BarCodeId)) continue;
            //        barCodelst.Add(t.BarCodeId);
            //    }

            //    if (CtrlInfo.ContainsKey(g.Key))
            //    {
            //        CtrlInfo[g.Key] = barCodelst;
            //    }
            //    else
            //    {
            //        CtrlInfo.TryAdd(g.Key, barCodelst);
            //    }
            //}



            var longx = DateTime.Now.Ticks - xxx;
            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("Load slusgl local data waste:" + longx);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(ReqEquLst, 0);
        }



        /// <summary>
        /// 单灯控制器参数列表  独立单灯设备
        /// </summary>
        public ConcurrentDictionary<int, Wlst.client.EquSluSgl.ParaFieldSluSgl> Info =
            new ConcurrentDictionary<int, EquSluSgl.ParaFieldSluSgl>();


        #region Get
/// <summary>
        /// 通过集中器地址获取集中器数据
/// </summary>
/// <param name="fildid"></param>
/// <returns></returns>
        public EquSluSgl.ParaFieldSluSgl GetField(int fildid)
        {
            if (Info.ContainsKey(fildid)) return Info[fildid];
            return null;
        }

        /// <summary>
        /// 存在返回  否则返回null 控制器地址查询
        /// </summary>
        /// <param name="fildid"></param>
        /// <param name="ctrlid"> </param>
        /// <returns></returns>
        public EquSluSgl.ParaSluCtrl Get(int fildid,int ctrlid)
        {
            if (Info.ContainsKey(fildid))
            {
                foreach (var g in Info [fildid ].CtrlLst  )
                {
                    if(g.CtrlId ==ctrlid )
                    {
                        return g;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取该控制器的信息
        /// </summary>
        /// <param name="ctrlid"></param>
        /// <returns></returns>
        public EquSluSgl.ParaSluCtrl Get(int ctrlid)
        {
            foreach (var f in Info)
            {
                foreach (var g in f.Value.CtrlLst)
                    if (g.CtrlId == ctrlid)
                    {
                        return g;
                    }
            }
            return null;
        }

        /// <summary>
        /// 获取该控制器的ctrlid
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public int GetCtrlIdByBarcode(long barcode)
        {
            foreach (var f in Info)
            {
                foreach (var g in f.Value.CtrlLst)
                    if (g.BarCodeId == barcode)
                    {
                        return g.CtrlId;
                    }
            }
            return 0;
        }



        /// <summary>
        /// 获取该控制器的集中器地址
        /// </summary>
        /// <param name="ctrlid"></param>
        /// <returns></returns>
        public int GetCtrlField(int ctrlid)
        {
            foreach (var f in Info)
            {
                foreach (var g in f.Value.CtrlLst)
                    if (g.CtrlId == ctrlid)
                    {
                        return f.Value.FieldId;
                    }
            }
            return -1;
        }
        /// <summary>
        /// 获取该控制器的区域id
        /// </summary>
        /// <param name="ctrlid"></param>
        /// <returns></returns>
        public int GetCtrlAreaId(int ctrlid)
        {
            foreach (var f in Info)
            {
                foreach (var g in f.Value.CtrlLst)
                    if (g.CtrlId == ctrlid)
                    {
                        return f.Value.AreaId;
                    }
            }
            return 0;
        }

        ///// <summary>
        ///// 存在返回  否则返回null 条形码查询
        ///// </summary>
        ///// <returns></returns>
        //public EquSluSgl.ParaFieldSluSgl Get(long barcode)
        //{
        //    var ntg = (from t in Info where t.Value.BarCodeId == barcode select t.Value).ToList();
        //    if (ntg.Count > 0) return ntg[0];
        //    return null;
        //}

        ///// <summary>
        ///// 存在返回   控制器地址查询
        ///// </summary>
        ///// <param name="ctrlids"></param>
        ///// <returns></returns>
        //public List<EquSluSgl.ParaSluCtrl> Get(List<int> ctrlids)
        //{
        //    return (from t in Info where ctrlids.Contains(t.Key) select t.Value).ToList();
        //}


        ///// <summary>
        ///// 存在返回   条形码查询
        ///// </summary>
        ///// <returns></returns>
        //public List<EquSluSgl.ParaSluCtrl> Get(List<long> barcodes)
        //{
        //    return (from t in Info where barcodes.Contains(t.Value.BarCodeId) select t.Value).ToList();
        //}


        /// <summary>
        /// 查询是否存在条形码  lvf 2018年5月17日16:25:14
        /// </summary>
        /// <param name="fieldId"></param>
        /// <param name="barcode"></param>
        /// <returns></returns>
        //public bool HaveSameBarCode(int fieldId,long barcode)
        //{
        //    //if (CtrlInfo.ContainsKey(fieldId)==false ) return false;
            
        //    //是否存在相同条形码 
        //    foreach (var g in CtrlInfo)
        //    {
        //        ////自己存在2个return true
        //        //if (g.Key == fieldId)
        //        //{
        //        //    int selfHasTwo = 0;
        //        //    foreach (var f in CtrlInfo[fieldId])
        //        //    {
        //        //        if (f == barcode) return true ;
        //        //    }
        //        //}
        //        //else
        //        //{
        //            foreach (var f in g.Value)
        //            {
        //                if (f == barcode) return true;
        //            }
                
        //        //}

        //    }

        //    return false ;
        //}


        #endregion

        #region to from xml

        protected string Xmlfilepath = Environment.CurrentDirectory + "\\slusgldata";

        protected void SaveRtuToXml(EquSluSgl.ParaFieldSluSgl cnt)
        {
            try
            {
                if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
                if (cnt == null) return;
                string path = Xmlfilepath + "\\" + cnt.FieldId ;
                Wlst.Cr.CoreOne.Seridata.ClassToFileStream.SaveAsXml(path, cnt);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("终端数据写入本地文本异常:" + ex);
            }
        }

        protected void DelteRtuFrXml(int id)
        {
            try
            {
                if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
                string path = Xmlfilepath + "\\" + id;

                Wlst.Cr.CoreOne.Seridata.ClassToFileStream.DeleteXml(path);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("删除终端数据异常:" + ex);
            }
        }

        protected Dictionary<int, EquSluSgl.ParaFieldSluSgl> LoadRtufromXml()
        {
            try
            {
                if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
                DirectoryInfo theFolder = new DirectoryInfo(Xmlfilepath);
                var fileInfo = theFolder.GetFiles();
                //遍历文件夹
                var lst = new Dictionary<int, EquSluSgl.ParaFieldSluSgl>();
                foreach (FileInfo nextFile in fileInfo) //遍历文件
                {
                    var tmp =
                        Wlst.Cr.CoreOne.Seridata.ClassToFileStream.LoadFromXml<EquSluSgl.ParaFieldSluSgl>(nextFile.FullName);
                    // this.listBox2.Items.Add(nextFile.FullName );
                    if (tmp != null)
                    {
                        if (!lst.ContainsKey(tmp.FieldId ))
                            lst.Add(tmp.FieldId, tmp);
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("读取本地终端数据异常:" + ex);
            }
            return new Dictionary<int, EquSluSgl.ParaFieldSluSgl>();
        }

        #endregion
    }

    /// <summary>
    /// 与服务器交互数据
    /// </summary>
    public partial class SluSglInfoHold
    {


        private void InitAciotn()
        {

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_equ ,
                OnEquipmentAddOrUpdateOrDelete,
                typeof(SluSglInfoHold), this);
        }

        public void OnEquipmentAddOrUpdateOrDelete(string session, Wlst.mobile.MsgWithMobile sdata)
        {
            if (sdata == null || sdata.WstSlusglEqu == null) return;
            if (sdata.Head.Scc == 1) return;
            var data = sdata.WstSlusglEqu;
            
            if (data.Op == 1)
            {
                //根据获取的设备 列表 清除本地的  已经不存在的设备
                var dlt = (from t in Info where data.FieldOrCtrlLst .Contains(t.Key) == false select t.Key).ToList();
                EquSluSgl.ParaFieldSluSgl  paras = null;
                foreach (var f in dlt) if (Info.ContainsKey(f))
                {
                    Info.TryRemove(f, out paras);
                    DelteRtuFrXml(f);
                }

                //根据获取的列表  比对本地的md5 是否是最新的 是否请求最新的
                int xcount = data.FieldOrCtrlLst .Count;
                if (data.DtFieldUpdate.Count < xcount) xcount = data.DtFieldUpdate .Count;
                var lst = new List<int>();
                for (int i = 0; i < xcount; i++)
                {
                    if (Info.ContainsKey(data.FieldOrCtrlLst [i]) == false ||
                        Info[data.FieldOrCtrlLst [i]].DtUpdate != data.DtFieldUpdate [i])
                        lst.Add(data.FieldOrCtrlLst [i]);
                }

                if (lst.Count > 0)
                {
                    this.ReqEqu(lst);
                }


                //读取 控制器类型（厂商） 配置文件在中间层 lvf 2018年12月14日13:27:40
                if (data.Items.Count == 0) return;
                var infotmp = data.Items[0];
                if ((infotmp.ProductName.Count == infotmp.ProductType.Count) && infotmp.ProductName.Count > 0)
                {
                    CtrlTpyes.Clear();
                    for (int i = 0; i < infotmp.ProductName.Count; i++)
                    {
                        CtrlTpyes.Add(infotmp.ProductType[i], infotmp.ProductName[i]);
                    }

                }
                


                return;
            }

            if (data.Op == 2 || data.Op == 3 || data.Op == 4)
            {
                if (data.Items.Count == 0) return;
                foreach (var f in data.Items)
                {
                    if (Info.ContainsKey(f.FieldId )) Info[f.FieldId ] = f;
                    else Info.TryAdd(f.FieldId , f);
                    this.SaveRtuToXml(f);



                    //更新条形码 记录
                    //foreach (var g in Info)
                    //{

                    //    var barCodelst = new List<long>();
                    //    foreach (var t in g.Value.CtrlLst)
                    //    {
                    //        if (barCodelst.Contains(t.BarCodeId)) continue;
                    //        barCodelst.Add(t.BarCodeId);
                    //    }

                        //if (CtrlInfo.ContainsKey(g.Key))
                        //{
                        //    CtrlInfo[g.Key] = barCodelst;
                        //}
                        //else
                        //{
                        //    CtrlInfo.TryAdd(g.Key, barCodelst);
                        //}
                    //}



                }

                int id = 0;
                if (data.Op == 2) id = EventIdAssign.SluSglEquReqOver;
                if (data.Op == 3) id = EventIdAssign.SluSglEquUpdate;
                if (data.Op == 4) id = EventIdAssign.SluSglEquAdd;

                var ar = new PublishEventArgs()
                             {
                                 EventId = id,
                                 EventType = PublishEventType.Core
                             };
                ar.AddParams((from t in data.Items select t.FieldId).ToList().First());
                EventPublish.PublishEvent(ar);


                if(data .Op ==4)
                {
                    var ar1 = new PublishEventArgs()
                    {
                        EventId = 1000,
                        EventType = "onuseraddslusgleequipment"
                    };
                    ar1.AddParams(sdata .Head .Gid );
                    EventPublish.PublishEvent(ar1);
                }
                return;
            }

            if (data.Op == 5)
            {

                EquSluSgl.ParaFieldSluSgl  paras = null;
                foreach (var f in data.FieldOrCtrlLst ) if (Info.ContainsKey(f))
                {
                    Info.TryRemove(f, out paras);
                    DelteRtuFrXml(f);
                }

                var ar = new PublishEventArgs()
                             {
                                 EventId = EventIdAssign.SluSglEquDelete,
                                 EventType = PublishEventType.Core
                             };
                ar.AddParams(data.FieldOrCtrlLst );
                EventPublish.PublishEvent(ar);
                return;
            }

        }

        /// <summary>
        /// 与服务器交互数据  请求md5
        /// </summary>
        public void ReqEquLst()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_equ;
            info.WstSlusglEqu.Op = 1;
            SndOrderServer.OrderSnd(info, 10,6);//120
        }

        /// <summary>
        /// 请求域设备参数
        /// </summary>
        /// <param name="lst"></param>
        public void ReqEqu(List< int > lst)
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_equ;
            info.WstSlusglEqu.Op = 2;
            info.WstSlusglEqu.FieldOrCtrlLst.AddRange(lst);
            //info.WstSlusglEqu.DtFieldUpdate.Add(DateTime.Now.Ticks);
            //info.WstSlusglEqu.AreaIdThatNewsAddto = 1;
            //info.WstSlusglEqu.Items.Add(new EquSluSgl.ParaFieldSluSgl()
            //                                {
            //                                    AreaId = 0,
            //                                    FieldId = 222
            //                                });


            SndOrderServer.OrderSnd(info, 10, 120);
        }

        /// <summary>
        /// 记录设备型号  lvf 2018年12月14日09:58:46
        /// </summary>
        public static Dictionary<int, string> CtrlTpyes = new Dictionary<int, string>();


    }
}
