using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Service;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Views
{
    /// <summary>
    /// TimeTableManage.xaml 的交互逻辑
    /// </summary>
    [ViewExport(AttachNow = false, ID = TimeTableSystem.Services.ViewIdAssign.TimeTableManageViewId,
AttachRegion = TimeTableSystem.Services.ViewIdAssign.TimeTableManageViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeTableManage 
    {
        public TimeTableManage()
        {
            InitializeComponent();
              EventPublisher.AddEventSubScriptionTokener(Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }

        [Import]
        public IITimeTableManageViewModel Model
        {
            get { return DataContext as IITimeTableManageViewModel; }
            set { DataContext = value; }
        }
    }


    public partial class TimeTableManage
    {
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.None && args.EventId == TimeTableSystem.Services.EventIdAssign.EventAddOrUpdateTimeTableAnimationId)
                {
                    return true;
                }
                if (args.EventType == PublishEventType.None && args.EventId == TimeTableSystem.Services.EventIdAssign.EventSaveOrCancelTimeTableAnimationId)
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
                        case Services.EventIdAssign.EventAddOrUpdateTimeTableAnimationId:
                            Animations.AddAnimation.AddorModifyAn(TableManage, TableAdd);
                            break;
                        case Services.EventIdAssign.EventSaveOrCancelTimeTableAnimationId:
                            Animations.CancelAnimation.CancelAn(TableManage,TableAdd);
                            break;
                        default :
                            WriteLog.WriteLogError(args.EventId.ToString(CultureInfo.InvariantCulture));
                            break;
                    }
                    
                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("AddOrModifyAnimation error in FundEventHandlers:ex:" + xe);
            }
        }
    }
}
