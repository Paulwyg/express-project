using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DragDropExtend.ExtensionsHelper;


using Telerik.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Models;
using Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Services;
using Wlst.Ux.EquipmentGroupManage.Services;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.EquipmentGroupManage.Views
{
    /// <summary>
    /// GrpMulitManageView.xaml 的交互逻辑
    /// </summary>

    [ViewExport( ViewIdAssign.TreeModuleGrpMulitManageViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class GrpMulitManageView : UserControl
    {
        public GrpMulitManageView()
        {
            InitializeComponent();
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            //CETC50_Core.ComponentHold.ViewComponentHolding.GetViewById(ViewIdNameAssign.TreeModuleGrpMulitManageViewId);
        }

        [Import]
        public IIGrpMulitManageView Model
        {
            get { return DataContext as IIGrpMulitManageView; }
            set { DataContext = value;
             }
        }


        public void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 那么在这里面的代码发生在PreviewMouseRightButtonDown中的代码之后，逻辑正确
            Model.TreeView_MouseRightButtonDown(sender, e);
        }


        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Model.TreeView_PreviewMouseRightButtonDown(sender, e);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Type itemType = typeof(RadTreeViewItem);

            UIElement child = sender as UIElement;
            UIElement father = null;
            if (itemType != null)
            {
                father = (UIElement)child.GetVisualAncestor(itemType);
            }

            if (father != null)
            {
                TreeItemViewModel item = ((RadTreeViewItem)father).DataContext as TreeItemViewModel;
                if (item.TxbNameVisi == Visibility.Visible)
                {
                    item.StopEditName();
                }
            }
        }
    }

    /// <summary>
    /// 动画
    /// </summary>
    public partial class GrpMulitManageView
    {
        private bool _isdetailin = false;
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.None && args.EventId == Services.EventIdAssign.AnimationGrpMulitManange)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.None && args.EventId == Services.EventIdAssign.AnimationGrpMulitManange && !_isdetailin)
                {

                    GrpMulitManageViewModel.Animations.Animation.EnterFromLeftAndTop(detail,0.5);
                    _isdetailin = true;
                }

            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("GrpMutiManageView ReSetAnimation error in FundEventHandlers:ex:" + xe);
            }
        }

        protected void Animation_Leave_Click(object sender, EventArgs e)
        {
           GrpMulitManageViewModel.Animations.Animation.LeaveFromLeftAndBottom(detail, 0.2);
            _isdetailin = false;
        }
    }
}
