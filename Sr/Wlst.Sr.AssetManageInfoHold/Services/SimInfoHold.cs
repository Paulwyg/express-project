using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.AssetManageInfoHold.Model;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Sr.AssetManageInfoHold.Services
{
    public class SimInfoHold : EventHandlerHelperExtendNotifyProperyChanged
    {
        private static readonly SimInfoHoldExtend Myself = new SimInfoHoldExtend();

        public static void RequesSimInfo()
        {
            Myself.RequestSimInfo();
        }

        public static void UpdateSimInfo(List<SimInfo > siminfo )
        {
            Myself.UpdateSimInfo(siminfo);
        }

        public static void InitLoad()
        {
            Myself.InitLoad();
        }

        public static Dictionary<int, SimInfo> GetData()
        {
            return Myself.Info;
        }
    }

    internal partial class SimInfoHoldExtend
    {
        /// <summary>
        /// 所有电源资产信息
        /// </summary>
        public Dictionary<int, SimInfo> Info = new Dictionary<int, SimInfo>();

        /// <summary>
        /// 是否与服务器同步了数据
        /// </summary>
        protected bool BolGetServerReturn = false;
    }

    /// <summary>
    /// 实现对分组信息的事件捕获与更新
    /// </summary>
    internal partial class SimInfoHoldExtend : EventHandlerHelperExtendNotifyProperyChanged
    {
        internal void InitEvent()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);

        }

        /// <summary>
        /// 事件数据处理
        /// </summary>
        /// <param name="args"></param>
        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                RequestSimInfo();
                return;
            }
        }


        private bool _bolinitload = false;
        /// <summary>
        /// 程序初始化时必须执行一次
        /// </summary>
        internal void InitLoad()
        {
            if (_bolinitload) return;
            _bolinitload = true;

            InitEvent();
            this.InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestSimInfo, 1);

        }


        private void InitAction()
        {

            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxSpe.wlst_spe_zc_sim ,
                                       OnSvrSimInfoArrive,
                                       typeof(SimInfoHoldExtend), this);

        }

        protected void OnSvrSimInfoArrive(string session, MsgWithMobile infos)
        {
            if (infos.WstSpeZcSim == null) return;

            var simInfoExchangefromServer = infos.WstSpeZcSim;

            var lstfromServer = simInfoExchangefromServer.Items;
            if (lstfromServer == null) return;


            if ((simInfoExchangefromServer.Op == 1) || (simInfoExchangefromServer.Op == 5))
            {
                //分组信息更新
                Info.Clear();
                foreach (var t in simInfoExchangefromServer.Items)
                {
                    var item = new SimInfo(t);
                    if (Info.ContainsKey(t.Id)) Info[t.Id] = item;
                    else Info.Add(t.Id, item);
                }
                var args = new PublishEventArgs()
                {
                    EventId = EventIdAssign.SimNeedUpdate,
                    EventType = PublishEventType.Core
                };
                EventPublish.PublishEvent(args);
            }
        }





        /// <summary>
        ///   请求服务器更新数据
        /// </summary>
        public void UpdateSimInfo(List<SimInfo> siminfo)
        {
            var ntg = (from t in siminfo orderby t.Id ascending select t).ToList();
            var info = Wlst.Sr.ProtocolPhone.LxSpe.wlst_spe_zc_sim ;

            foreach (var t in ntg)
            {
                info.WstSpeZcSim.Items.Add(new ZcSim.ZcSimItem()
                                               {
                                                   DtKt = t.DtKt,
                                                   DtXf = t.DtXf,
                                                   Id = t.Id,
                                                   RtuId = t.RtuId,
                                                   IpAddr = t.IpAddr,
                                                   State = t.State,
                                                   SimNum = t.SimNum

                                               });

            }

            info.WstSpeZcSim.Op = 5;

            SndOrderServer.OrderSnd(info, 10, 6);

            //Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
            //    0, "所有分组",

        }


        /// <summary>
        ///  请求比对数据 触发点
        /// </summary>
        public void RequestSimInfo()
        {
            BolGetServerReturn = false;
            var info = Wlst.Sr.ProtocolPhone.LxSpe.wlst_spe_zc_sim ;
            info.WstSpeZcSim .Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
    }
}
