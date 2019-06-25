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
    public class LampInfoHold : EventHandlerHelperExtendNotifyProperyChanged 
    {
        private static readonly LampInfoHoldExtend Myself = new LampInfoHoldExtend();

        public static void RequestLampInfo()
        {
            Myself.RequestLampInfo();
        }

        public static void InitLoad()
        {
            Myself.InitLoad();
        }

        public static Dictionary<int, LampInfo> GetData()
        {
            return Myself.Info;
        }

        public static void UpdateData(List<LampInfo > lampinfo)
        {
            Myself.UpdateLampInfo(lampinfo );
        }
    }

    internal partial class LampInfoHoldExtend
    {
        /// <summary>
        /// 所有电源资产信息
        /// </summary>
        public Dictionary<int, LampInfo> Info = new Dictionary<int, LampInfo>();

        /// <summary>
        /// 是否与服务器同步了数据
        /// </summary>
        protected bool BolGetServerReturn = false;
    }

    /// <summary>
    /// 实现对分组信息的事件捕获与更新
    /// </summary>
    internal partial class LampInfoHoldExtend : EventHandlerHelperExtendNotifyProperyChanged 
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
                RequestLampInfo();
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
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestLampInfo, 1);

        }


        private void InitAction()
        {

            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxSpe.wlst_spe_zc_dygh,
                                       OnSvrLampInfoArrive,
                                       typeof(LampInfoHoldExtend), this);

        }

        protected void OnSvrLampInfoArrive(string session, MsgWithMobile infos)
        {
             if (infos.WstSpeZcDygh  == null) return;

             var lampInfoExchangefromServer = infos.WstSpeZcDygh;

             var lstfromServer = lampInfoExchangefromServer.Items;
            if (lstfromServer == null) return;


            if (lampInfoExchangefromServer.Op == 1)
            {
                //分组信息更新
                Info.Clear();
                foreach (var t in lampInfoExchangefromServer.Items)
                {
                    var item = new LampInfo(t);
                    if (Info.ContainsKey(t.Id)) Info[t.Id] = item;
                    else Info.Add(t.Id, item);
                }
                var args = new PublishEventArgs()
                {
                    EventId = EventIdAssign.LampNeedUpdate,
                    EventType = PublishEventType.Core,
                    EventAttachInfo = "已经更新信息！"
                };
                EventPublish.PublishEvent(args);
            }
        }





        /// <summary>
        ///   请求服务器更新数据
        /// </summary>
        public void UpdateLampInfo(List<LampInfo > lampinfo)
        {
             var ntg = (from t in lampinfo orderby t.Id ascending select t).ToList();
            var info = Wlst.Sr.ProtocolPhone.LxSpe.wlst_spe_zc_dygh ;

            foreach (var t in ntg)
            {
                info.WstSpeZcDygh.Items .Add(new ZcDyxx.ZcDyxxItem() 
                {
                    Id = t.Id,
                    Cqj = t.Cqj,
                    Dbbh = t.Dbbh,
                    Dygh = t.Dygh ,
                    IsYj = t.IsYj ,
                    RtuId = t.RtuId 
                });
            }

            info.WstSpeZcDygh.Op = 5;

            SndOrderServer.OrderSnd(info, 10, 6);

            //Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
            //    0, "所有分组",

        }


        /// <summary>
        ///  请求比对数据 触发点
        /// </summary>
        public void RequestLampInfo()
        {
            BolGetServerReturn = false;
            var info = Wlst.Sr.ProtocolPhone.LxSpe.wlst_spe_zc_dygh ;
            info.WstSpeZcDygh .Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
    }
}
