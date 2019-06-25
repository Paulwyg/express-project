using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DragDropExtend.ExtensionsHelper;
using Telerik.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.PrivilegesManage.AreaManageViewModel.Services;
using Wlst.Ux.PrivilegesManage.AreaManageViewModel.ViewModels;
using Wlst.Ux.PrivilegesManage.Services;

namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel.Views
{
    /// <summary>
    /// AreaManageViews.xaml 的交互逻辑
    /// </summary>
    [ViewExport(ViewIdAssign.AreaManageViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class AreaManageView : UserControl
    {
        public AreaManageView()
        {
            InitializeComponent();
        }
        [Import]
        public IIAreaManage Model
        {
            get { return DataContext as IIAreaManage; }
            set { DataContext = value; }
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
                AreaTreeItemModel item = ((RadTreeViewItem)father).DataContext as AreaTreeItemModel;
                

                if (item.TxbNameVisi == Visibility.Visible)
                {
                    item.StopEditName();
                    
                }
            }
        }
    }
}
