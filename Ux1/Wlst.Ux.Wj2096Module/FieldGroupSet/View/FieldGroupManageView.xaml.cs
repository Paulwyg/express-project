using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DragDropExtend.ExtensionsHelper;
using Telerik.Windows.Controls;
using Wlst.Cr.Core.Behavior;

using Wlst.Ux.Wj2096Module.Services;

namespace Wlst.Ux.Wj2096Module.FieldGroupSet.View
{
    /// <summary>
    /// GrpSingleManageView.xaml 的交互逻辑
    /// </summary>

    [ViewExport(ViewIdAssign.FieldGroupSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class FieldGroupManageView : UserControl
    {
        public FieldGroupManageView()
        {
            InitializeComponent();
        }

        [Import]
        public IIFieldGrpManage Model
        {
            get { return DataContext as IIFieldGrpManage; }
            set { DataContext = value; }
        }


        //public void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    // 那么在这里面的代码发生在PreviewMouseRightButtonDown中的代码之后，逻辑正确
        //    Model.TreeView_MouseRightButtonDown(sender, e);
        //}


        //private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    Model.TreeView_PreviewMouseRightButtonDown(sender, e);
        //}

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

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            rtuid.IsVisible=true;
        }
    }
}
