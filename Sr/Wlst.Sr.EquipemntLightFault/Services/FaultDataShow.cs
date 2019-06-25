using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.PPProtocolSvrCnt.Common;


namespace Wlst.Sr.EquipemntLightFault.Services
{
    public class FaultDataShow
    {
        public FaultDataShow ()
        {
            this.InitAction();
        }

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxFault  .wlst_fault_occu_data  ,
                OnRcvSvrFaultHappenData,
                typeof (FaultDataShow), this);
        }

        private void OnRcvSvrFaultHappenData(string session, Wlst .mobile .MsgWithMobile info)
        {
            if (info == null || info.WstFaultOccuData   == null) return;
            if (info.WstFaultOccuData.RtuNewData != null && info.WstFaultOccuData.RtuNewData.RtuId > 0)
            {
                var args = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId =
                        Sr.EquipmentInfoHolding.Services.EventIdAssign.
                        RtuDataQueryDataInfoNeedShowInTab,
                };
                args.AddParams(info.WstFaultOccuData.RtuNewData);
                EventPublish.PublishEvent(args);
                EventPublish.PublishEvent(new PublishEventArgs() { EventType = "MainWindow.Measure.show" });
                Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(1102809);
                return;
            }
            if (info.WstFaultOccuData.LineData != null && info.WstFaultOccuData.LineData.LduId  > 0)
            {
                var args = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId =
                        Sr.EquipmentInfoHolding.Services.EventIdAssign.
                        RtuDataQueryDataInfoNeedShowInTab,
                };
                args.AddParams(info.WstFaultOccuData.LineData);
                EventPublish.PublishEvent(args);
                //EventPublish.PublishEvent(new PublishEventArgs() { EventType = "MainWindow.Measure.show" });
                Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(1102709);
                return;
            }

            if (info.WstFaultOccuData.RtuId > 1100000 && info.WstFaultOccuData.RtuId < 1200000)
            {
                var args = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId =
                        Sr.EquipmentInfoHolding.Services.EventIdAssign.
                        RtuDataQueryDataInfoNeedShowInTab,
                };
                if (info.WstFaultOccuData.LineData == null) return;
                if (info.WstFaultOccuData.LineData.LduId == 0)
                {
                    info.WstFaultOccuData.LineData.LduId = info.WstFaultOccuData.RtuId;
                }
                args.AddParams(info.WstFaultOccuData.LineData);
                EventPublish.PublishEvent(args);
                Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(1102709);
                return;
            }

            if (info.WstFaultOccuData.SluCtrlData != null && info.WstFaultOccuData.SluCtrlData.Info != null && info.WstFaultOccuData.SluCtrlData.Info.SluId > 0)
            {
                var args = new PublishEventArgs
                               {
                                   EventType = PublishEventType.Core,
                                   EventId =
                                       Sr.EquipmentInfoHolding.Services.EventIdAssign.
                                       RtuDataQueryDataInfoNeedShowInTab,
                               };
                args.AddParams(info.WstFaultOccuData.SluCtrlData);
                EventPublish.PublishEvent(args);
                EventPublish.PublishEvent(new PublishEventArgs() { EventType = "MainWindow.Measure.show" });
                //   Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(1102709);
                return;
            }

            if (info.WstFaultOccuData.RtuId > 1000000 && info.WstFaultOccuData.RtuId < 1100000)
            {
                var args = new PublishEventArgs
                {
                    EventType = PublishEventType.Core,
                    EventId =
                        Sr.EquipmentInfoHolding.Services.EventIdAssign.
                        RtuDataQueryDataInfoNeedShowInTab,
                };
                args.AddParams(null);
                EventPublish.PublishEvent(args);
                EventPublish.PublishEvent(new PublishEventArgs() { EventType = "MainWindow.Measure.show" });
                Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(1102809);
                return;
            }

        }
    }
}
