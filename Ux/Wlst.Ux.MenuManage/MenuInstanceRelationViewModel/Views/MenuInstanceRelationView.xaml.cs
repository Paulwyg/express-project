using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DragDropExtend.ExtensionsHelper;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.MenuManage.MenuInstanceRelationViewModel.Services;
using Wlst.Ux.MenuManage.MenuInstanceRelationViewModel.ViewModel;
using Wlst.Ux.MenuManage.Services;

namespace Wlst.Ux.MenuManage.MenuInstanceRelationViewModel.Views
{
    /// <summary>
    /// MenuInstanceRelationView.xaml 的交互逻辑
    /// </summary>
        [ViewExport(ViewIdAssign .MenuInstanceRelationViewId,AttachNow = true  )]
    public partial class MenuInstanceRelationView : UserControl
    {
        public MenuInstanceRelationView()
        {
            InitializeComponent();
        }

        [Import]
        public IIInstancesRelationManagement Model
        {
            get
            {
                return DataContext as IIInstancesRelationManagement;
            }
            set
            {
                DataContext = value;
            }
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            TreeViewItem item = VisualTreeExtensions.GetTemplatedAncestor<TreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            Type itemType = typeof(TreeViewItem);

            UIElement child = sender as UIElement;
            UIElement father = null;
            if (itemType != null)
            {
                father = (UIElement)child.GetVisualAncestor(itemType);
            }

            if (father != null)
            {
                MenuTreeItemViewModel item = ((TreeViewItem)father).DataContext as MenuTreeItemViewModel;
                if (item != null)
                    if (item.TxbNameVisi == Visibility.Visible)
                    {
                        item.StopEditName();
                    }
            }
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Type itemType = typeof(TreeViewItem);

            UIElement child = sender as UIElement;
            UIElement father = null;
            if (itemType != null)
            {
                father = (UIElement)child.GetVisualAncestor(itemType);
            }

            if (father != null)
            {

                MenuTreeItemViewModel item = ((TreeViewItem)father).DataContext as MenuTreeItemViewModel;
                if (item != null)
                    if (item.TxbNameVisi == Visibility.Collapsed)
                    {
                        item.StartEditName();
                    }
            }
        }
    }
}
