//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Wlst.Cr.Core.EventHandlerHelper;
//using Wlst.Cr.CoreMims.Services;
//using Wlst.Cr.Coreb.EventHelper;
//using Wlst.Cr.Coreb.Servers;
//using Wlst.client;

//namespace Wlst.Sr.SlusglInfoHold.Services
//{
   
//    public partial class SluSglFieldHold
//    {
//        #region single instance

//        private static SluSglFieldHold _mySlef = null;
//        private static object _obj = 1;

//        public static SluSglFieldHold MySlef
//        {
//            get
//            {
//                if (_mySlef == null)
//                {
//                    lock (_obj)
//                    {
//                        if (_mySlef == null) _mySlef = new SluSglFieldHold();
//                    }
//                }
//                return _mySlef;
//            }
//        }

//        protected SluSglFieldHold()
//        {
//            //this.InitAciotn();
//        }

//        #endregion


//        //public void OnInit()
//        //{
//        //    //long xxx = DateTime.Now.Ticks;
//        //    //var tmp = this.LoadRtufromXml();
//        //    //foreach (var t in tmp)
//        //    //{
//        //    //    if (Info.ContainsKey(t.Key) == false) Info[t.Key] = t.Value;
//        //    //}
//        //    //var longx = DateTime.Now.Ticks - xxx;
//        //    //Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("Load slusgl local data waste:" + longx);
//        //    Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(ReqEquLst, 0);
//        //}



//        /// <summary>
//        /// 单灯控制器参数列表  独立单灯设备
//        /// </summary>
//        public ConcurrentDictionary<int, Wlst.client.FieldSluSgl.FieldSluSglItem > Info =
//            new ConcurrentDictionary<int, FieldSluSgl.FieldSluSglItem>();

//        #region Get

//        /// <summary>
//        /// 存在返回  否则返回null  
//        /// </summary>
//        /// <param name="fieldId"></param>
//        /// <returns></returns>
//        public FieldSluSgl.FieldSluSglItem Get(int fieldId)
//        {
//            if (Info.ContainsKey(fieldId)) return Info[fieldId];
//            return null;
//        }

    

//        /// <summary>
//        /// 存在返回    
//        /// </summary>
//        /// <param name="fieldIds"></param>
//        /// <returns></returns>
//        public List<FieldSluSgl.FieldSluSglItem> Get(List<int> fieldIds)
//        {
//            return (from t in Info where fieldIds.Contains(t.Key) select t.Value).ToList();
//        }
 

//        #endregion

//        //#region to from xml

//        //protected string Xmlfilepath = Environment.CurrentDirectory + "\\slusgldata";

//        //protected void SaveRtuToXml(EquSluSgl.ParaSluCtrl cnt)
//        //{
//        //    try
//        //    {
//        //        if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
//        //        if (cnt == null) return;
//        //        string path = Xmlfilepath + "\\" + cnt.CtrlId;
//        //        Wlst.Cr.CoreOne.Seridata.ClassToFileStream.SaveAsXml(path, cnt);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("终端数据写入本地文本异常:" + ex);
//        //    }
//        //}

//        //protected void DelteRtuFrXml(int id)
//        //{
//        //    try
//        //    {
//        //        if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
//        //        string path = Xmlfilepath + "\\" + id;

//        //        Wlst.Cr.CoreOne.Seridata.ClassToFileStream.DeleteXml(path);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("删除终端数据异常:" + ex);
//        //    }
//        //}

//        //protected Dictionary<int, EquSluSgl.ParaSluCtrl> LoadRtufromXml()
//        //{
//        //    try
//        //    {
//        //        if (!Directory.Exists(Xmlfilepath)) Directory.CreateDirectory(Xmlfilepath);
//        //        DirectoryInfo theFolder = new DirectoryInfo(Xmlfilepath);
//        //        var fileInfo = theFolder.GetFiles();
//        //        //遍历文件夹
//        //        var lst = new Dictionary<int, EquSluSgl.ParaSluCtrl>();
//        //        foreach (FileInfo nextFile in fileInfo) //遍历文件
//        //        {
//        //            var tmp =
//        //                Wlst.Cr.CoreOne.Seridata.ClassToFileStream.LoadFromXml<EquSluSgl.ParaSluCtrl>(nextFile.FullName);
//        //            // this.listBox2.Items.Add(nextFile.FullName );
//        //            if (tmp != null)
//        //            {
//        //                if (!lst.ContainsKey(tmp.CtrlId))
//        //                    lst.Add(tmp.CtrlId, tmp);
//        //            }
//        //        }
//        //        return lst;
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("读取本地终端数据异常:" + ex);
//        //    }
//        //    return new Dictionary<int, EquSluSgl.ParaSluCtrl>();
//        //}

//        //#endregion
//    }

//    ///// <summary>
//    ///// 与服务器交互数据
//    ///// </summary>
//    //public partial class SluSglFieldHold
//    //{


//    //    private void InitAciotn()
//    //    {

//    //        ProtocolServer.RegistProtocol(
//    //            Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_field_info ,
//    //            OnAddOrUpdateOrDelete,
//    //            typeof(SluSglFieldHold), this);
//    //    }

//    //    public void OnAddOrUpdateOrDelete(string session, Wlst.mobile.MsgWithMobile sdata)
//    //    {
//    //        if (sdata == null || sdata.WstSlusglFieldInfo == null) return;
//    //        var data = sdata.WstSlusglFieldInfo;
//    //        if (data.Op == 1)
//    //        {
//    //            this.Info.Clear();

//    //            foreach (var f in data.Items)
//    //            {
//    //                if (Info.ContainsKey(f.FieldId)) Info[f.FieldId] = f;
//    //                else Info.TryAdd(f.FieldId, f);
//    //            }

//    //            //同步更新域下的 分组信息 如果存在删除域则删除 区域信息
//    //            SluSglFieldGrpHold.MySlef.OnDeleteFieldThenDltGrp(Info.Keys.ToList());
//    //            var ar = new PublishEventArgs()
//    //                         {
//    //                             EventId = EventIdAssign.SluSglFieldReqOver,
//    //                             EventType = PublishEventType.Core
//    //                         };
//    //            EventPublish.PublishEvent(ar);
//    //            return;
//    //        }

//    //        if (data.Op == 2)
//    //        {
//    //            int aradid = data.AreaId;
//    //            var dlt = (from t in Info where t.Value.AreaId == aradid select t.Key).ToList();
//    //            FieldSluSgl.FieldSluSglItem dt = null;
//    //            foreach (var f in dlt) if (Info.ContainsKey(f)) Info.TryRemove(f, out dt);



//    //            foreach (var f in data.Items)
//    //            {
//    //                if (Info.ContainsKey(f.FieldId)) Info[f.FieldId] = f;
//    //                else Info.TryAdd(f.FieldId, f);
//    //            }

//    //            //同步更新域下的 分组信息 如果存在删除域则删除 区域信息
//    //            SluSglFieldGrpHold.MySlef.OnDeleteFieldThenDltGrp(Info.Keys.ToList());
//    //            var ar = new PublishEventArgs()
//    //                         {
//    //                             EventId = EventIdAssign.SluSglFieldUpdate,
//    //                             EventType = PublishEventType.Core
//    //                         };
//    //            ar.AddParams(aradid);
//    //            EventPublish.PublishEvent(ar);
//    //            return;
//    //        }

//    //    }


//    //    /// <summary>
//    //    /// 新增加控制器列表的时候 自动增加域信息  此处不发布事件 由增加终端那 发布事件
//    //    /// </summary>
//    //    /// <param name="areaid"></param>
//    //    /// <param name="fieldid"></param>
//    //    public void OnAddCtrlThenAddField(int areaid, int fieldid, List<int> ctrls)
//    //    {
//    //        if (Info.ContainsKey(fieldid)) return;
//    //        var tmp = new FieldSluSgl.FieldSluSglItem()
//    //                      {
//    //                          AreaId = areaid,
//    //                          CtrlLst = new List<int>(),
//    //                          FieldId = fieldid,
//    //                          FieldName = "新增域" + ctrls.Count,
//    //                          Order = 0,
//    //                          OtherAttri = 0
//    //                      };
//    //        tmp.CtrlLst.AddRange(ctrls);
//    //        Info.TryAdd(fieldid, tmp);
//    //    }

//    //    /// <summary>
//    //    /// 与服务器交互数据  请求md5
//    //    /// </summary>
//    //    public void ReqEquLst()
//    //    {
//    //        var info = Wlst.Sr.ProtocolPhone.LxSluSgl.wst_slusgl_field_info ;
//    //        info.WstSlusglFieldInfo .Op  = 1;
//    //        SndOrderServer.OrderSnd(info, 10, 120);
//    //    }
 
//    //}
//}
