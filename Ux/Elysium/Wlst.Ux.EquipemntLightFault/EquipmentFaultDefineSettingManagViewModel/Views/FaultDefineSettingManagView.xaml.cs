using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingManagViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingManagViewModel.Views
{
    /// <summary>
    /// FaultDefineSettingManagView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(AttachNow = false, ID = EquipemntLightFault.Services.ViewIdAssign.FaultDefineSettingManagViewId,
   AttachRegion = EquipemntLightFault.Services.ViewIdAssign.FaultDefineSettingManagViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class FaultDefineSettingManagView
    {
        public FaultDefineSettingManagView()
        {
            InitializeComponent();
            EventPublisher.AddEventSubScriptionTokener(Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }

        [Import]
        public IIFaultDefineSettingManagViewModel Model
        {
            get { return DataContext as IIFaultDefineSettingManagViewModel; }
            set { DataContext = value; }
        }
    }

    public partial class FaultDefineSettingManagView
    {
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.None && args.EventId ==EquipemntLightFault.Services.EventIdAssign.EventAddFaultDefineSetttingAnimationId)
                {
                    return true;
                }
                if (args.EventType == PublishEventType.None && args.EventId == EquipemntLightFault.Services.EventIdAssign.EventCancelDefineSettingAnimationId)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.None)
                {
                    switch (args.EventId)
                    {
                        case EquipemntLightFault.Services.EventIdAssign.EventAddFaultDefineSetttingAnimationId :
                            Animations.AddAnimation.AddorModifyAn(TableManage, TableAdd);
                            break;
                        case EquipemntLightFault.Services.EventIdAssign.EventCancelDefineSettingAnimationId:
                            Animations.CancelAnimation.CancelAn(TableManage,TableAdd);
                            break;
                        default:
                            WriteLog.WriteLogError(args.EventId.ToString(CultureInfo.InvariantCulture));
                            break;
                    }

                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("FaultDefineSettingManagView Animation error in FundEventHandlers:ex:" + xe);
            }
        }
    }
}
