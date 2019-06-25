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


    public partial class SluSglFieldGrpHold
    {
        #region single instance

        private static SluSglFieldGrpHold _mySlef = null;
        private static object _obj = 1;

        public static SluSglFieldGrpHold MySlef
        {
            get
            {
                if (_mySlef == null)
                {
                    lock (_obj)
                    {
                        if (_mySlef == null) _mySlef = new SluSglFieldGrpHold();
                    }
                }
                return _mySlef;
            }
        }

        protected SluSglFieldGrpHold()
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
            var longx = DateTime.Now.Ticks - xxx;
            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("Load slusgl local data waste:" + longx);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(ReqEquLst, 0);
        }



        /// <summary>
        /// 单灯控制器参数列表  独立单灯设备
        /// </summary>
        public ConcurrentDictionary<Tuple<int, int>, Wlst.client.GrpFieldSluSglCtrl.GrpFieldSluSglItem> Info =
            new ConcurrentDictionary<Tuple<int, int>, GrpFieldSluSglCtrl.GrpFieldSluSglItem>();

        #region Get

        /// <summary>
        /// 存在返回   
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public  List<GrpFieldSluSglCtrl.GrpFieldSluSglItem> Get(int fieldId)
        {
            return
                (from t in Info where t.Value.FieldId == fieldId orderby t.Value.GrpId ascending select t.Value).ToList();
        }


        /// <summary>
        /// 存在返回   
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public GrpFieldSluSglCtrl.GrpFieldSluSglItem Get(int fieldId, int grpid)
        {
            return
                (from t in Info
                 where t.Value.FieldId == fieldId && t.Value.GrpId == grpid
                 orderby t.Value.Order  ascending
                 select t.Value).FirstOrDefault();
        }


        /// <summary>
        /// 存在返回   
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public  List<int> GetSpecial(int fieldId)
        {
            var all = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(fieldId).CtrlLst;
            var lst = new List<int>();
            foreach (var t in all)
            {
                lst.Add(t.CtrlId);
            }

            foreach (var t in Info.Values)
            {
                foreach (var i in t.CtrlLst)
                {
                    if (lst.Contains(i))
                    {
                        lst.Remove(i);
                    }
                }
            }

            return lst;
        }

        #endregion

        #region to from xml

        protected string Xmlfilepath = Environment.CurrentDirectory + "\\slusglfldgrpdata";

        protected void SaveRtuToXml(GrpFieldSluSglCtrl.GrpFieldSluSglItem cnt)
        {
            try
            {
                if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
                if (cnt == null) return;
                string path = Xmlfilepath + "\\" + cnt.FieldId + "-" + cnt.GrpId;
                Wlst.Cr.CoreOne.Seridata.ClassToFileStream.SaveAsXml(path, cnt);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("终端数据写入本地文本异常:" + ex);
            }
        }

        protected void DelteRtuFrXml(Tuple<int, int> id)
        {
            try
            {
                if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
                string path = Xmlfilepath + "\\" + id.Item1 + "-" + id.Item2;

                Wlst.Cr.CoreOne.Seridata.ClassToFileStream.DeleteXml(path);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("删除终端数据异常:" + ex);
            }
        }

        protected Dictionary<Tuple<int, int>, GrpFieldSluSglCtrl.GrpFieldSluSglItem> LoadRtufromXml()
        {
            try
            {
                if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
                DirectoryInfo theFolder = new DirectoryInfo(Xmlfilepath);
                var fileInfo = theFolder.GetFiles();
                //遍历文件夹
                var lst = new Dictionary<Tuple<int, int>, GrpFieldSluSglCtrl.GrpFieldSluSglItem>();
                foreach (FileInfo nextFile in fileInfo) //遍历文件
                {
                    var tmp =
                        Wlst.Cr.CoreOne.Seridata.ClassToFileStream.LoadFromXml<GrpFieldSluSglCtrl.GrpFieldSluSglItem>(
                            nextFile.FullName);
                    // this.listBox2.Items.Add(nextFile.FullName );
                    if (tmp != null)
                    {
                        var tu = new Tuple<int, int>(tmp.FieldId, tmp.GrpId);
                        if (!lst.ContainsKey(tu))
                            lst.Add(tu, tmp);
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("读取本地终端数据异常:" + ex);
            }
            return new Dictionary<Tuple<int, int>, GrpFieldSluSglCtrl.GrpFieldSluSglItem>();
        }

        #endregion
    }

    /// <summary>
    /// 与服务器交互数据
    /// </summary>
    public partial class SluSglFieldGrpHold
    {


        private void InitAciotn()
        {

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_field_grp_info,
                OnAddOrUpdateOrDelete,
                typeof (SluSglFieldGrpHold), this);
        }

        public void OnAddOrUpdateOrDelete(string session, Wlst.mobile.MsgWithMobile sdata)
        {
            if (sdata == null || sdata.WstSlusglFieldGrpInfo == null) return;
            var data = sdata.WstSlusglFieldGrpInfo;
            if (data.Op == 1)
            {
                //根据获取的设备 列表 清除本地的  已经不存在的设备
                var dlt = (from t in Info where data.FieldLst.Contains(t.Value.FieldId) == false select t.Key).ToList();
                GrpFieldSluSglCtrl.GrpFieldSluSglItem paras = null;
                foreach (var f in dlt)
                    if (Info.ContainsKey(f))
                    {
                        Info.TryRemove(f, out paras);
                        DelteRtuFrXml(f);
                    }

                //根据获取的列表  比对本地的md5 是否是最新的 是否请求最新的
                int xcount = data.FieldLst.Count;
                if (data.DtFieldUpdate.Count < xcount) xcount = data.DtFieldUpdate .Count;
                var lst = new List<int>();
                for (int i = 0; i < xcount; i++)
                {
                    var ntg =
                        (from t in Info where t.Value.FieldId == data.FieldLst[i] select t.Value.DtUpdate).ToList();
                    if (ntg.Count > 0 && ntg[0] == data.DtFieldUpdate [i]) continue;
                    lst.Add(data.FieldLst[i]);
                }

                if (lst.Count > 0)
                {
                    this.ReqEqu(lst);
                }

                return;
            }

            if (data.Op == 2 || data.Op == 3)
            {
                if (data.Items.Count() == 0) return;

                GrpFieldSluSglCtrl.GrpFieldSluSglItem paras = null;
                foreach (var g in Info)
                {
                    if (g.Key.Item1 == data.FieldId) Info.TryRemove(g.Key, out paras);
                    DelteRtuFrXml(g.Key);
                }


                foreach (var f in data.Items)
                {

                    var tu = new Tuple<int, int>(f.FieldId, f.GrpId);
                    if (Info.ContainsKey(tu)) Info[tu] = f;
                    else Info.TryAdd(tu, f);
                    this.SaveRtuToXml(f);
                }


                var ar = new PublishEventArgs()
                             {
                                 EventId = EventIdAssign.SluSglFieldGrpUpdate,
                                 EventType = PublishEventType.Core
                             };
                if (data.Items.Count>0)
                {
                    ar.AddParams((from t in data.Items select t.FieldId).Distinct().ToList().First());
                }
                EventPublish.PublishEvent(ar);
                return;
            }

        }

        /// <summary>
        /// 当域信息变化的时候 或删除的时候  同步更新域下的组信息
        /// </summary>
        /// <param name="allfileid"></param>
        public void OnDeleteFieldThenDltGrp(List<int> allfileid)
        {
            var ntg = (from t in Info where allfileid.Contains(t.Value.FieldId) == false select t.Key).ToList();
            GrpFieldSluSglCtrl.GrpFieldSluSglItem paras = null;
            foreach (var f in ntg)
                if (Info.ContainsKey(f))
                {
                    Info.TryRemove(f, out paras);
                    DelteRtuFrXml(f);
                }
        }



        /// <summary>
        /// 与服务器交互数据  请求md5
        /// </summary>
        public void ReqEquLst()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_field_grp_info;
            info.WstSlusglFieldGrpInfo.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 120);
        }

        /// <summary>
        /// 请求设备参数
        /// </summary>
        /// <param name="lst"></param>
        public void ReqEqu(List<int> lst)
        {
            var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_field_grp_info;
            info.WstSlusglFieldGrpInfo.Op = 2;
            SndOrderServer.OrderSnd(info, 10, 120);
        }
    }
}
