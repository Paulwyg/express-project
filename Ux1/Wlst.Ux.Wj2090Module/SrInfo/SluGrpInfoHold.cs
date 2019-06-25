//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//
//
//using Wlst.Cr.Core.EventHandlerHelper;
//using Wlst.Cr.Core.ModuleServices;
//using Wlst.Cr.CoreMims.Services;
//using Wlst.client;


//namespace Wlst.Ux.Wj2090Module.SrInfo
//{


//    public partial class SluGrpInfoHold : EventHandlerHelper
//    {
//        #region single instances

//        private static SluGrpInfoHold _mySlef = null;
//        private static object obj = 1;

//        public static SluGrpInfoHold MySelf
//        {
//            get
//            {
//                if (_mySlef == null)
//                {
//                    lock (obj)
//                    {
//                        if (_mySlef == null)
//                            new SluGrpInfoHold();
//                    }

//                }
//                return _mySlef;
//            }
//        }

//        #endregion


//        /// <summary>
//        /// 存放单灯时间方案
//        /// </summary>
//        public ConcurrentDictionary<int, GroupInfo> Info = new ConcurrentDictionary<int, SluGrpInfo.GroupInfo>();

//        private SluGrpInfoHold()
//        {
//            if (_mySlef != null) return;

//            _mySlef = this;
//            this.InitAciton();
//            this.InitEvent();
//            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestGrpInfofromSvr, 5,
//                                                                      DelayEventHappen.EventSvAc);
//        }

//        public void Init()
//        {

//        }

//        private void RequestGrpInfofromSvr()
//        {
//            var info = Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_grp ;// .wlst_cnt_wj2090_request_slu_grps ;//.ServerPart.wlst_Wj2090_clinet_request_slu_grp_info;
//            info.WstSluGrp.Op = 1;
//            SndOrderServer.OrderSnd(info, 10, 3);
//        }

//        /// <summary>
//        /// 更新所有的分组信息
//        /// </summary>
//        /// <param name="timeInfo"></param>
//        public void UpdateGroupInfo(List<SluGrpInfo.GroupInfo> timeInfo)
//        {
//            var info = Wlst.Sr.ProtocolPhone.LxSlu.wst_slu_grp;
//                // .wlst_cnt_wj2090_update_slu_grps ;//.ServerPart.wlst_Wj2090_clinet_update_slu_grp_info;
//            info.WstSluGrp.Op = 2;

//            info.WstSluGrp.GrpItems.AddRange(timeInfo);
//            SndOrderServer.OrderSnd(info, 10, 3);
//        }


//        public SluGrpInfo.GroupInfo GetGrpInfoByGrpId(int grpId)
//        {
//            if (Info.ContainsKey(grpId)) return Info[grpId];
//            return null;
//        }
//    }


//    public partial class SluGrpInfoHold
//    {
//        private void InitAciton()
//        {

//            ProtocolServer.RegistProtocol(
//                Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_grp ,// .wlst_svr_ans_cnt_wj2090_request_slu_grps ,//.ClientPart.wlst_Wj2090_svr_ans_clinet_request_slu_grp_info,
//                OnGrpRequestOrUpdate,
//                typeof(SluGrpInfoHold), this, true);

//        }

//        private void InitEvent()
//        {
//            this.AddEventFilterInfo(-1, PublishEventType.ReCn);

//        }

//        /// <summary>
//        /// 事件数据处理
//        /// </summary>
//        /// <param name="args"></param>
//        public override void ExPublishedEvent(PublishEventArgs args)
//        {
//            if (args.EventType == PublishEventType.ReCn)
//            {
//                this.RequestGrpInfofromSvr();
//                return;
//            }
//        }

//        private void OnGrpRequestOrUpdate(string sessionid,Wlst .mobile .MsgWithMobile  info)
//        {
//            if (info == null||info .WstSluGrp ==null ) return;
//            Info.Clear();
//            foreach (var g in info.WstSluGrp  .GrpItems )
//            {

//                if (!Info.ContainsKey(g.GroupId))
//                    Info.TryAdd(g.GroupId, g);
//                Info[g.GroupId] = g;
//            }


//            var ar = new PublishEventArgs()
//                         {
//                             EventId = info.WstSluGrp.Op == 1 ? Wj2090Module.Services.EventIdAssign.GrpSluInfoRequestId : Wj2090Module.Services.EventIdAssign.GrpSluInfoUpdateId,
//                             EventType = PublishEventType.Core
//                         };
//            EventPublish.PublishEvent(ar);
//        }

      

//    }
//}
