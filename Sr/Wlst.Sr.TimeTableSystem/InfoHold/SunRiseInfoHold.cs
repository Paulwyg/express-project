using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using SunRiseItemInfomation = Wlst.Sr.TimeTableSystem.Models.SunRiseItemInfomation;

namespace Wlst.Sr.TimeTableSystem.InfoHold
{
    /// <summary>
    /// 基础数据
    /// </summary>
    public class SunRaiseInfoHoldBase :Wlst .Cr .Core .EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged 
    {
        /// <summary>
        /// 构造函数；
        /// </summary>
        public SunRaiseInfoHoldBase()
        {
            Info = new Dictionary<string, SunRiseItemInfomation>();

             
        }

     

        /// <summary>
        /// 提供数据持有的数据结构
        /// </summary>
        protected Dictionary<string, SunRiseItemInfomation> Info;


        #region 提供外部对数据的操作Get Set

        /// <summary>
        /// <para>任何使用此数据务必注意 此数据为原始数据，___只允许读不允许修改___ </para> 
        /// <para>任何修改会使原始数据被修改形成脏数据 </para>
        /// <para>修改请用SupperEquipmentInstanceContains 中具体数据的clone方法进行克隆副本使用</para>
        /// </summary>
        public Dictionary<string, SunRiseItemInfomation> InfoDictionary
        {
            get { return Info; } //将原始数据返回  数据安全性无法保证
        }


        /// <summary>
        /// 获取日出日落信息
        /// </summary>
        /// <param name="month">月 </param>
        /// <param name="day"> 日</param>
        /// <returns>日出日落信息 无则null</returns>
        public SunRiseItemInfomation GetSunRiseItemInfo(int month, int day)
        {
            string key = month + "_" + day;
            if (Info.ContainsKey(key))
            {
                return Info[key];
            }
            return null;
        }

        #endregion


    }

    public partial class SunRiseInfoHold : SunRaiseInfoHoldBase
    {
        public void InitStart ()
        {
            InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestSunRaiseSetInfomation, 0);
           // this.RequestSunRaiseSetInfomation();
        }
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class SunRiseInfoHold 
    {
        //private  void InitEvnet()
        //{
        //    this.AddEventFilterInfo(EventIdAssign.SunSetRiseRequest, PublishEventType.Sevr);
        //}

        //public override void ExPublishedEvent(PublishEventArgs args)
        //{
        //    if (args.EventId == EventIdAssign.SunSetRiseRequest)
        //    {
        //        var infoExchangefromServer = args.GetParams()[1] as SunRiseSetInfoExchangeforServer;
        //        if (infoExchangefromServer == null || infoExchangefromServer.LstInfoNeedUpdate.Count == 0)
        //            return;
        //        UpdateSunRiseInfo(infoExchangefromServer);
        //    }
        //}


        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxRtuTime .wst_request_sunrise_set_info ,// .wlst_svr_ans_cnt_request_sun_rise_set ,//.ClientPart.wlst_SunRiseSet_server_ans_clinet_request_sunriseset ,
                SunSetRiseRequest,
                typeof(SunRiseInfoHold), this);
        }

        public void SunSetRiseRequest(string session, Wlst .mobile .MsgWithMobile  infos)
        {
            var infoExchangefromServer = infos.WstRtutimeRequestSunriseSetInfo  ;
          //  var infoExchangefromServer = args.GetParams()[1] as SunRiseSetInfoExchangeforServer;
            if (infoExchangefromServer == null || infoExchangefromServer.Items .Count == 0)
                return;
            UpdateSunRiseInfo(infoExchangefromServer);

        }


        void UpdateSunRiseInfo(client .SunRiseSetInfo  infoExchangefromServer)
        {
            if (infoExchangefromServer == null || infoExchangefromServer.Items .Count == 0)
                return;
            LogInfo.Log("与服务器同步信息...");


            Info.Clear();
            foreach (var t in infoExchangefromServer.Items )
            {
                if (t == null)continue;
                this.AddOrUpdateSunRiseTime(t.DateMonth , t.DateDay , t.TimeSunrise, t.TimeSunset);
            }


            var arg = new PublishEventArgs()
                          {
                              EventId = EventIdAssign.SunSetRiseRequest,
                              EventType = PublishEventType.Core
                          };

            EventPublish.PublishEvent(arg);
        }

        void AddOrUpdateSunRiseTime(int month, int day, int sunrise, int sunset)
        {
            var sunRise = new SunRiseItemInfomation()
                              {
                                  date_day = day,
                                  date_month = month,
                                  time_sunrise = sunrise,
                                  time_sunset = sunset
                              };
            string key = month + "_" + day;

            if (!Info.ContainsKey(key)) Info.Add(key, sunRise);
            else Info[key] = sunRise;
        }

    };

    /// <summary>
    /// Socket to Server
    /// </summary>
    public partial class SunRiseInfoHold
    {
        /// <summary>
        /// 请求日出日落时间;
        /// <para>qz  qiangzhi</para>
        /// </summary>
        public void  RequestSunRaiseSetInfomation()
        {


            var info = ProtocolPhone .LxRtuTime .wst_request_sunrise_set_info ;// .wlst_cnt_request_sun_rise_set ;//.ServerPart.wlst_SunRiseSet_clinet_request_sunriseset;
          
            SndOrderServer.OrderSnd(info, 10, 6);
            return ;
        }

        public SunRiseInfoHold ()
        {
            this.AddEventFilterInfo(100, PublishEventType.ReCn);
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.ReCn)
            {
                this.RequestSunRaiseSetInfomation();
            }
            //base.ExPublishedEvent(args);
        }

    }
}
