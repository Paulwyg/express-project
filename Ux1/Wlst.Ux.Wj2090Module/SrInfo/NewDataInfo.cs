//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//
//
//using Wlst.Cr.Core.EventHandlerHelper;
//using Wlst.Cr.CoreMims.Services;


//namespace Wlst.Ux.Wj2090Module.SrInfo
//{
//    public partial class NewDataInfo
//    {
//        #region single instances

//        private static NewDataInfo _mySlef = null;
//        private static object obj = 1;

//        public static NewDataInfo MySelf
//        {
//            get
//            {
//                if (_mySlef == null)
//                {
//                    lock (obj)
//                    {
//                        if (_mySlef == null)
//                            new NewDataInfo();
//                    }

//                }
//                return _mySlef;
//            }
//        }

//        #endregion


//        /// <summary>
//        /// 存放单灯集中器数据
//        /// </summary>
//        public ConcurrentDictionary<int, SluMeasureInfo> InfoSlu = new ConcurrentDictionary<int, SluMeasureInfo>();

//        public ConcurrentDictionary<Tuple<int, int>, CtrlMeasureInfo> InfoCtrl =
//            new ConcurrentDictionary<Tuple<int, int>, CtrlMeasureInfo>();

//        private NewDataInfo()
//        {
//            if (_mySlef != null) return;

//            _mySlef = this;
//            this.InitAciton();

//        }

//        public void Init()
//        {

//        }
//    }

//    public partial class NewDataInfo
//    {
//        private void InitAciton()
//        {
//            ProtocolServer.RegistProtocol(
//                Wlst.Sr.ProtocolPhone .LxSlu .wst_svr_ans_slu_ctrl_measure   , OnSluMeasure,
//                typeof(NewDataInfo), this);


//            //ProtocolServer.RegistProtocol(
//            //   Wlst.Sr.ProtocolCnt.ClientPart.wlst_Wj2090_svr_to_clinet_xc_conn_args, OnSluMeasurex,
//            //   typeof(NewDataInfo), this);
//        }

//        private void OnSluMeasure(string sessionid,Wlst .mobile .MsgWithMobile  info)
//        {
//            if (info == null||info .WstSvrAnsSluCtrlMeasure ==null ) return;

//            int sluId = info.WstSvrAnsSluCtrlMeasure  .SluId;
//            if (sluId < 1) return;

//            var tu5 = new List<Tuple<int, int>>();
//            var lst = new List<int>();
//            int xtype = 0;
//            foreach  (var f in info.WstSvrAnsSluCtrlMeasure.InfoSlu0 )
//            {
//                this.AddSluInfo(sluId, f);
//                lst.Add(1);
//            }
//            if (info.WstSvrAnsSluCtrlMeasure.UnknowCtrlField != null && info.WstSvrAnsSluCtrlMeasure.UnknowCtrlField.Count  > 1)
//            {
//                this.AddSluInfo(sluId, info.WstSvrAnsSluCtrlMeasure.UnknowCtrlField);
//                lst.Add(2);
//            }

//            //foreach (var g in info.Data.InfoPhy4)
//            //{
//            //    this.AddSluCtrlInfo(sluId, g.CtrlId, g);
//            //    if (!lst.Contains(4)) lst.Add(4);
//            //}
//            //foreach (var g in info.Data.InfoAssis6)
//            //{
//            //    this.AddSluCtrlInfo(sluId, g.CtrlId, g);
//            //    if (!lst.Contains(6)) lst.Add(6);
//            //}
//            foreach (var g in info.WstSvrAnsSluCtrlMeasure.InfoBaseic5)
//            {
//                this.AddSluCtrlInfo(sluId, g.Info.CtrlId, g);
//                if (!lst.Contains(5)) lst.Add(5);
//                tu5.Add(new Tuple<int, int>(sluId, g.Info.CtrlId));
//            }


//            var ar = new PublishEventArgs()
//                         {
//                             EventId = Wlst .Sr .EquipmentInfoHolding .Services.EventIdAssign.RunningInfo1Update ,
//                             EventType = PublishEventType.Core
//                         };
//            ar.AddParams(sluId);
//            ar.AddParams(lst);
//            ar.AddParams(tu5);
//            //if (info.Data.InfoBaseic5.Count > 0 && info.Data.InfoBasic5IsAll)
//            //{
//            //    ar.AddParams(5);
//            //}
//            EventPublish.PublishEvent(ar);
//        }


//        //public void sdfsdf()
//        //{
//        //    PublishEventArgs ar=new PublishEventArgs() ;
//        //    if (ar.EventId == Wj2090Module.Services.EventIdAssign.OnSluNewDataArrive && ar.EventType == PublishEventType.Core) //单灯最新数据
//        //    {
//        //        int sluId = Convert.ToInt32(ar.GetParams()[0]);
//        //        List<int> ctrls = ar.GetParams()[1] as List<int>;
//        //        if (ctrls == null) return;

//        //        foreach (var f in ctrls )
//        //        {
//        //            var tu = new Tuple<int, int>(sluId, f);
//        //            if (Wlst.Ux.Wj2090Module.SrInfo.NewDataInfo.MySelf.InfoCtrl.ContainsKey(tu))
//        //            {
//        //                var data = Wlst.Ux.Wj2090Module.SrInfo.NewDataInfo.MySelf.InfoCtrl[tu].Data5;
//        //                if (data == null) continue;

//        //                int lid = sluId*1000 + f;

//        //                bool isLightOn = false;
//        //                bool hasError = false;
//        //                foreach (var g in data.Items)
//        //                {
//        //                    if (g.Fault != 0) hasError = true;
//        //                    if (g.StateWorkingOn != 3) isLightOn = true;

//        //                    UpdateSluCtrlState(lid, g.LampId, isLightOn, hasError);
//        //                }


//        //                //UpdateSluCtrlState(lid, 1,isLightOn, hasError);
//        //            }
//        //        }
//        //    }
//        //}

//        //private void UpdateSluCtrlState(int lid,int lampId,bool isLightOn,bool hasError)
//        //{
            
//        //}


//        private void AddSluInfo(int sluId, Wlst.client.SluCtrlDataMeasureReply.DataSluCon info)
//        {
//            if (!InfoSlu.ContainsKey(sluId)) InfoSlu.TryAdd(sluId, new SluMeasureInfo(sluId));
//            InfoSlu[sluId].SluData = info;
//            InfoSlu[sluId].LastUpdate = 1; // = info;
//            InfoSlu[sluId].LastUpdateTime = DateTime.Now.Ticks;
//        }

//        private void AddSluInfo(int sluId, List<Wlst.client.SluCtrlDataMeasureReply.UnknowCtrl> info)
//        {
//            if (!InfoSlu.ContainsKey(sluId)) InfoSlu.TryAdd(sluId, new SluMeasureInfo(sluId));
//            InfoSlu[sluId].DataUnknown = info;
//            InfoSlu[sluId].LastUpdate = 2; // = info;InfoCtrl[tukey].LastUpdateTime  = DateTime .Now .Ticks ;
//            InfoSlu[sluId].LastUpdateTime = DateTime.Now.Ticks;
//        }


//        private void AddSluCtrlInfo(int sluId, int ctrlId, Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData info)
//        {
//            var tukey = new Tuple<int, int>(sluId, ctrlId);
//            if (!InfoCtrl.ContainsKey(tukey)) InfoCtrl.TryAdd(tukey, new CtrlMeasureInfo(sluId, ctrlId));
//            InfoCtrl[tukey].Data5 = info;
//            InfoCtrl[tukey].LastUpdate = 5;
//            InfoCtrl[tukey].LastUpdateTime = DateTime.Now.Ticks;
//        }

//        private void AddSluCtrlInfo(int sluId, int ctrlId, Wlst.client.SluCtrlDataMeasureReply.CtrlPhyinfo info)
//        {
//            var tukey = new Tuple<int, int>(sluId, ctrlId);
//            if (!InfoCtrl.ContainsKey(tukey)) InfoCtrl.TryAdd(tukey, new CtrlMeasureInfo(sluId, ctrlId));
//            InfoCtrl[tukey].DataPhy4 = info;
//            InfoCtrl[tukey].LastUpdate = 4;
//            InfoCtrl[tukey].LastUpdateTime = DateTime.Now.Ticks;
//        }

//        private void AddSluCtrlInfo(int sluId, int ctrlId, Wlst.client.SluCtrlDataMeasureReply.AssistCtrlData info)
//        {
//            var tukey = new Tuple<int, int>(sluId, ctrlId);
//            if (!InfoCtrl.ContainsKey(tukey)) InfoCtrl.TryAdd(tukey, new CtrlMeasureInfo(sluId, ctrlId));
//            InfoCtrl[tukey].DataAss6 = info;
//            InfoCtrl[tukey].LastUpdate = 6;
//            InfoCtrl[tukey].LastUpdateTime = DateTime.Now.Ticks;
//        }
//    }



//    public class CtrlMeasureInfo
//    {
//        public Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData Data5;
//        public Wlst.client.SluCtrlDataMeasureReply.CtrlPhyinfo DataPhy4;
//        public Wlst.client.SluCtrlDataMeasureReply.AssistCtrlData DataAss6;
//        public int SluId;
//        public int CtrlId;
//        public long LastUpdateTime;

//        /// <summary>
//        /// 最后更新的数据是 4 物理信息，5 控制器数据，6 控制器辅助数据
//        /// </summary>
//        public int LastUpdate;

//        public CtrlMeasureInfo(int sluId, int ctrlId)
//        {
//            SluId = sluId;
//            CtrlId = ctrlId;
//            Data5 = null;
//            DataPhy4 = null;
//            DataAss6 = null;
//        }
//    }

//    public class SluMeasureInfo
//    {
//        public int SluId;
//        public List<Wlst.client.SluCtrlDataMeasureReply.UnknowCtrl> DataUnknown;
//        public Wlst.client.SluCtrlDataMeasureReply.DataSluCon SluData;
//        public long LastUpdateTime;

//        /// <summary>
//        /// 最后更新的数据是 2 未知控制器，1 集中器数据
//        /// </summary>
//        public int LastUpdate;

//        public SluMeasureInfo(int sluId)
//        {
//            SluId = sluId;
//            DataUnknown = new List<Wlst.client.SluCtrlDataMeasureReply.UnknowCtrl>();
//            SluData = null;
//        }
//    }
//}
