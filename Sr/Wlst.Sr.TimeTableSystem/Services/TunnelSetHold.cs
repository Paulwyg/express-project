using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.client;
namespace Wlst.Sr.TimeTableSystem.Services
{
    public partial class TunnelSetHold : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged
    {
        private static TunnelSetHold _myself = null;
        public static TunnelSetHold Myself
        {
            get
            {
                if (_myself == null) _myself = new TunnelSetHold();
                return _myself;
            }
        }
        private  TunnelSetHold()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);
            this.InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestInfo, 0);


            //moni();
        }

        //模拟
        //void moni()
        //{

        //    var tmp = new TunnelControlPlan.TunnelControlOnePlan()
        //                  {
        //                      AreaId = 0,
        //                      IsLightControl = 1,
        //                      ItemPlan = new List<TunnelControlPlan.TunnelControlOnePlan.TunnelControlOnePlanOpeItem>(),
        //                      PlanId = -1,
        //                      PlanName = "测试方案",
        //                      RtusBelongThisTunnel = new List<int>(),
        //                      TimeProtect = 300,
        //                      TunnelName = "隧道方案"
        //                  };
        //    tmp.ItemPlan.Add(new TunnelControlPlan.TunnelControlOnePlan.TunnelControlOnePlanOpeItem()
        //                         {
        //                             Id = 1,
        //                             ItemRtuOpe =
        //                                 new List
        //                                 <TunnelControlPlan.TunnelControlOnePlan.TunnelControlOnePlanOpeItem.RtuOpe>(),
        //                             LuxId = 1400001,
        //                             LuxIdBackup = 1400002,
        //                             MaxValue = 370,
        //                             OpeDesc = "重阴天",
        //                             OpeName = "基本"

        //                         });
        //    foreach (var t in tmp.ItemPlan)
        //    {
        //        t.ItemRtuOpe.Add(new TunnelControlPlan.TunnelControlOnePlan.TunnelControlOnePlanOpeItem.RtuOpe()
        //                             {
        //                                 RtuId = 1000002,
        //                                 SwitchOutNeedOpen = new List<int>()
        //                             });
        //        foreach (var f in t.ItemRtuOpe)
        //        {
        //            f.SwitchOutNeedOpen.Add(1);
        //            f.SwitchOutNeedOpen.Add(2);
        //        }
        //    }

        //    tmp.RtusBelongThisTunnel.Add(1000002);
           
            


        //    var tu = new Tuple<int, int>(0, 1);
        //    this.Info.TryAdd(tu, tmp);
        //}

        public void Init()
        {
            
        }

        /// <summary>
        /// 提供数据持有的数据结构  araid-planid
        /// </summary>
        public  ConcurrentDictionary<Tuple<int, int>, Wlst.client.TunnelControlPlan.TunnelControlOnePlan> Info = new ConcurrentDictionary<Tuple<int, int>, Wlst.client.TunnelControlPlan.TunnelControlOnePlan>();


    }

    public partial class TunnelSetHold
    {
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtuTime.wst_timetable_tunnel_control_plan,
                // .wlst_svr_ans_cnt_request_sun_rise_set ,//.ClientPart.wlst_SunRiseSet_server_ans_clinet_request_sunriseset ,
                Request,
                typeof (TunnelSetHold), this);

        }

        public void Request(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var data = infos.WstRtutimeTunnelControlPlan;
            this.Info.Clear();
            if (data.Op == 1)
            {
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TunnelInfoSetRequestId
                };
                EventPublish.PublishEvent(args);
            }
            if (data.Op == 2)
            {
                //TunnelControlPlan.TunnelControlOnePlan tmp = null;
                //var keys = (from t in Info where t.Key.Item1 == data.AreaId select t.Key).ToList();
                //foreach (var f in keys) if (Info.ContainsKey(f)) Info.TryRemove(f, out tmp);
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TunnelInfoSetUpdateId
                };
                EventPublish.PublishEvent(args);
            }


            foreach (var f in data.ItemsTunnelControlPlan)
            {
                var tu = new Tuple<int, int>(f.AreaId, f.PlanId);
                if (Info.ContainsKey(tu)) Info[tu] = f;
                else Info.TryAdd(tu, f);
            }
         
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                this.RequestInfo();
            }
            //base.ExPublishedEvent(args);
        }

        private void RequestInfo()
        {
            var info = ProtocolPhone.LxRtuTime.wst_timetable_tunnel_control_plan;
                // .wlst_cnt_request_sun_rise_set ;//.ServerPart.wlst_SunRiseSet_clinet_request_sunriseset;
            info.WstRtutimeTunnelControlPlan.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 2);
            return;
        }


    }

}
