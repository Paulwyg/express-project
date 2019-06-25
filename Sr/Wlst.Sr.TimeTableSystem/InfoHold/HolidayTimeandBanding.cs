using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.TimeTableSystem.Models;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.client;

namespace Wlst.Sr.TimeTableSystem.InfoHold
{
    /// <summary>
    /// 节假日方案以及 终端与节假日的绑定关系
    /// </summary>
    public partial class HolidayTimeandBanding : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged
    {
        /// <summary>
        /// 节假日调度方案 全部 areaid-holidayid-  detail 
        /// </summary>
        protected Dictionary<Tuple< int ,int >, HolidayWeekSetInfo.HolidaySchduleTime> InfoHolidaySchduleTime;

        /// <summary>
        /// 终端绑定调度方案 全部 areaid-rtuid- holidayid
        /// </summary>
        protected Dictionary<Tuple<int, int>, int> InfoRtuBandingSchdule;

        /// <summary>
        /// 
        /// </summary>
        public HolidayTimeandBanding()
        {
            InfoHolidaySchduleTime = new Dictionary<Tuple<int, int>, HolidayWeekSetInfo.HolidaySchduleTime>();
            InfoRtuBandingSchdule = new Dictionary<Tuple<int, int>, int>();
            this.AddEventFilterInfo(100, PublishEventType.ReCn);
        }

        

        /// <summary>
        /// 节假日调度方案  获取只能读 不允许改
        /// </summary>
        public Dictionary<Tuple<int, int>, HolidayWeekSetInfo.HolidaySchduleTime> InfoHolidaySchduleTimeGet
        {
            get { return InfoHolidaySchduleTime; }
        }

        /// <summary>
        /// 终端绑定调度方案  获取只能读 不允许改
        /// </summary>
        public Dictionary<Tuple<int, int>, int> InfoRtuBandingSchduleGet
        {
            get { return InfoRtuBandingSchdule; }
        }

    }

    public partial class HolidayTimeandBanding
    {
        /// <summary>
        /// 
        /// </summary>
        public void InitStart()
        {
            InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestTimeSchduleInfo, 1);

        }



        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                this.RequestTimeSchduleInfo();
            }
        }

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtuTime .wst_holiday_week_set ,// .wlst_svr_ans_cnt_request_holiday_info ,//.ClientPart.wlst_TimeTable_server_ans_clinet_request_time_holiday ,
                UpdateScduleTimeAndRtuBandingInfo,
                typeof(HolidayTimeandBanding), this);

            //ProtocolServer.RegistProtocol(
            //   Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_update_holiday_info ,//.ClientPart.wlst_TimeTable_server_ans_clinet_update_time_holiday,
            //   UpdateScduleTimeAndRtuBandingInfo,
            //   typeof(HolidayTimeandBanding), this);
        }

        private void UpdateScduleTimeAndRtuBandingInfo(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            if (infos == null) return;
            if (infos.WstRtutimeHolidayWeekSet.Op == 1)
            {
                this.InfoHolidaySchduleTime.Clear();
                this.InfoRtuBandingSchdule.Clear();
            }
            if (infos.WstRtutimeHolidayWeekSet.Op == 2)
            {
                if (infos.WstRtutimeHolidayWeekSet.HolidaySchduleTimeAndBandingItems.Count == 0) return;
                int areaid = infos.WstRtutimeHolidayWeekSet.HolidaySchduleTimeAndBandingItems[0].AreaId;
                var keys = (from t in InfoHolidaySchduleTime where t.Key.Item1 == areaid select t.Key).ToList();
                foreach (var f in keys) if (InfoHolidaySchduleTime.ContainsKey(f)) InfoHolidaySchduleTime.Remove(f);

                var keys1 = (from t in InfoRtuBandingSchdule where t.Key.Item1 == areaid select t.Key).ToList();
                foreach (var f in keys1) if (InfoRtuBandingSchdule.ContainsKey(f)) InfoRtuBandingSchdule.Remove(f);
            }

            foreach (var f in infos.WstRtutimeHolidayWeekSet.HolidaySchduleTimeAndBandingItems)
            {
                //todo delete
                
                foreach (var t in f.Schdules)
                {
                    var tu = new Tuple<int, int>(f.AreaId, t.Id);
                    if (!InfoHolidaySchduleTime.ContainsKey(tu ))
                        InfoHolidaySchduleTime.Add(tu , t);
                }
                foreach (var t in f.RtuBandings)
                {
                    var tu = new Tuple<int, int>(f.AreaId, t.RtuId);
                    if (!InfoRtuBandingSchdule.ContainsKey(tu))
                        InfoRtuBandingSchdule.Add(tu, t.HolidaySchduleId);
                }
            }

            var arg = new PublishEventArgs()
                          {
                              EventId = EventIdAssign.TimeHolidayTimeSchduleAndRtuBandingChanged,
                              EventType = PublishEventType.Core
                          };

            EventPublish.PublishEvent(arg);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "" + "终端 ", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "服务器反馈节假日信息变更.");
        }
    }

    /// <summary>
    /// Socket to Server
    /// </summary>
    public partial class HolidayTimeandBanding
    {
        /// <summary>
        /// 请求数据;
        /// </summary>
        public void RequestTimeSchduleInfo()
        {
            var info = Wlst.Sr.ProtocolPhone .LxRtuTime .wst_holiday_week_set ;// .wlst_cnt_request_holiday_info ;//.ServerPart.wlst_TimeTable_clinet_request_time_holiday ;
            info.WstRtutimeHolidayWeekSet.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
        }


    }


}
