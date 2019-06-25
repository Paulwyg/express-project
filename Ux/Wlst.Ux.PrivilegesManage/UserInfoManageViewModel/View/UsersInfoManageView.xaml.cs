using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;


using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.PrivilegesManage.Services;
using Wlst.Ux.PrivilegesManage.UserInfoManageViewModel.Services;
using Wlst.Ux.PrivilegesManage.UserInfoManageViewModel.ViewModels;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.PrivilegesManage.UserInfoManageViewModel.View
{
    /// <summary>
    /// UsersInfoManageView.xaml 的交互逻辑
    /// </summary>
        [ViewExport(ViewIdAssign.UserInfoManageViewId)]
    public partial class UsersInfoManageView
    {
        public UsersInfoManageView()
        {
            InitializeComponent();
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            
      
        }

        [Import]
        public IIUserInfoManageViewModel Model
        {
            get { return DataContext as IIUserInfoManageViewModel; }
            set { DataContext = value; }
        }

        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.None && args.EventId == EventIdAssign.ResetAnimationEventId)
                {
                        return true;
                }
                if (args.EventType == PublishEventType.None && args.EventId == EventIdAssign.AddAnimationEventId)
                {
                    return true;
                }
                if (args.EventType == PublishEventType.None && args.EventId == EventIdAssign.FleshAnimationEventId)
                {
                    return true;
                }
                if (args.EventType == PublishEventType.None && args.EventId == EventIdAssign.CancelFleshAnimationEventId)
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

        private bool _isAddFormVisible;
        private bool _isUpdateFormVisible;
        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if(args.EventType==PublishEventType.None && args.EventId==EventIdAssign.ResetAnimationEventId)
                {
                    if(_isAddFormVisible)
                    {
                        Animations.Animation.LeaveToBottom(add, 1);
                        _isAddFormVisible = false;
                    }
                    if(_isUpdateFormVisible)
                    {
                        Animations.Animation.LeaveToBottom(update,1);
                        _isUpdateFormVisible = false;
                    }
                   
                }
                else if (args.EventType == PublishEventType.None && args.EventId == EventIdAssign.AddAnimationEventId)
                {
                    if(_isUpdateFormVisible)
                    {
                        Animations.Animation.EnterFromBottomWithUpdateVisible(update,add,1);
                        _isUpdateFormVisible = false;
                    }
                    else
                    {
                        Animations.Animation.EnterFromBottom(add,1);
                    }
                    _isAddFormVisible = true;
                }
                else if(args.EventType==PublishEventType.None && args.EventId==EventIdAssign.FleshAnimationEventId)
                {
                    if(_isAddFormVisible)
                    {
                        Animations.Animation.UpdateEnterFromLeftWhenAddVisible(add,update,1);
                        _isAddFormVisible = false;
                    }
                    else if(!_isUpdateFormVisible)
                    {
                        Animations.Animation.UpdateEnterFromLeftWhenAddHidden(update,1);
                    }
                    _isUpdateFormVisible = true;
                }
                else if (args.EventType == PublishEventType.None && args.EventId == EventIdAssign.CancelFleshAnimationEventId)
                {
                    Animations.Animation.LeaveToLeft(update, 0.5);
                    _isUpdateFormVisible = false;
                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("ReSetAnimation error in FundEventHandlers:ex:" + xe);
            }
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

           


    }
}
