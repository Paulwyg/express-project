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
    public partial class TimeTabletemporaryHold : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged
    {
        private static TimeTabletemporaryHold _myself = null;
        public static TimeTabletemporaryHold Myself
        {
            get
            {
                if (_myself == null) _myself = new TimeTabletemporaryHold();
                return _myself;
            }
        }
        private TimeTabletemporaryHold()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);
            this.InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestInfo, 0);
            simulation();
        }

        //模拟
        void simulation()
        {
            var tmp = new TempTimePlanWithTimeTableBandingInfo.TimeTablePlan()
                          {
                              AreaId = 0,
                              DateStart = 636251039236829725,
                              DateEnd = 636251039236985725,
                              TimePlanId = 1,
                              TimePlanName = "测试方案",
                              LuxEffective = 30,
                              LuxId = 1400001,
                              LuxIdBackup = 1400002,
                              ItemsPlan =
                                  new List<TempTimePlanWithTimeTableBandingInfo.TimeTablePlan.TimeTableOnedayPlan>(),
                              TimetablesUseThisPlan = new List<int>()
                          };
            tmp.ItemsPlan.Add(new TempTimePlanWithTimeTableBandingInfo.TimeTablePlan.TimeTableOnedayPlan()
                                  {
                                      Date = 20180412,
                                      SectionId = 1,
                                      TimeOff = 1500,
                                      TimeOn = 1500,
                                      TypeOff = 1,
                                      TypeOn = 1
                                  });
            tmp.TimetablesUseThisPlan.Add(1);
            var tu = new Tuple<int, int>(0, 1);
            this.Info.TryAdd(tu, tmp);
        }

        public void Init()
        {

        }

        /// <summary>
        /// 提供数据持有的数据结构  araid-planid
        /// </summary>
        public ConcurrentDictionary<Tuple<int, int>, Wlst.client.TempTimePlanWithTimeTableBandingInfo.TimeTablePlan> Info = new ConcurrentDictionary<Tuple<int, int>, Wlst.client.TempTimePlanWithTimeTableBandingInfo.TimeTablePlan>();

    }

    public partial class TimeTabletemporaryHold
    {
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtuTime.wst_rtutime_temp_time_plan,
                // .wlst_svr_ans_cnt_request_sun_rise_set ,//.ClientPart.wlst_SunRiseSet_server_ans_clinet_request_sunriseset ,
                Request,
                typeof (TimeTabletemporaryHold), this);
        }
        public void Request(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var data = infos.WstRtutimeTempTimePlan;
            //if ((data.Op == 1) || (data.Op == 3))
            //{
                this.Info.Clear();

            //}
            //if (data.Op == 2)
            //{
            //    int areaId = data.TempTimePlanItems[0].AreaId;
            //    TempTimePlanWithTimeTableBandingInfo.TimeTablePlan tmp = null;
            //    var keys = (from t in Info where t.Key.Item1 == areaId select t.Key).ToList();
            //    foreach (var f in keys)
            //    {
            //        if (Info.ContainsKey(f))
            //            Info.TryRemove(f, out tmp);
            //    }

            //}
            foreach (var f in data.TempTimePlanItems)
            {

                var tu = new Tuple<int, int>(f.AreaId, f.TimePlanId);
                Info.TryAdd(tu, f);

                if (Info.ContainsKey(tu))
                    Info[tu] = f;
                else
                    Info.TryAdd(tu, f);

            }

            if(data.Op==1)
            {
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TimeTemporaryInfoRequestId
                };
                EventPublish.PublishEvent(args);
            }

            if (data.Op == 2)
            {
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TimeTemporaryInfoUpdateId
                };
                EventPublish.PublishEvent(args);
            }

            if (data.Op == 3)
            {
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.TimeTemporaryInfoDeleteId
                };
                EventPublish.PublishEvent(args);
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
            var info = ProtocolPhone.LxRtuTime.wst_rtutime_temp_time_plan;
            // .wlst_cnt_request_sun_rise_set ;//.ServerPart.wlst_SunRiseSet_clinet_request_sunriseset;
            info.WstRtutimeTempTimePlan.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 2);
            return;
        }

    }
}
