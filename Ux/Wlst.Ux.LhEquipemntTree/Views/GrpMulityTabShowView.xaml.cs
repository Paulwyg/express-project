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
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.LhEquipemntTree.GrpMulitTabShowViewModel.Services;

namespace Wlst.Ux.LhEquipemntTree.Views
{
    /// <summary>
    /// GrpMulityTabShowView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( LhEquipemntTree .Services .ViewIdAssign .GrpMulityTabShowViewId ,
    AttachRegion = Services .ViewIdAssign .GrpMulityTabShowViewAttachRegion )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class GrpMulityTabShowView : UserControl
    {
        public GrpMulityTabShowView()
        {
            InitializeComponent();
        }

        [Import]
        public IIMultiTree Model
        {
            get
            {
                return DataContext as IIMultiTree;
            }
            set
            {
                DataContext = value;
            }
        }




        public void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 如果使用附加属性来开启右键选中功能，
            // 那么在这里面的代码发生在TreeViewHelper中的代码之后，逻辑正确
        }


        private void MenuItem_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }


        private void TreeViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            var item = GetTemplatedAncestor<Telerik.Windows.Controls.RadTreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            var item = GetTemplatedAncestor<Telerik.Windows.Controls.RadTreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        private T GetTemplatedAncestor<T>(FrameworkElement element) where T : FrameworkElement
        {
            if (element is T)
            {
                return element as T;
            }

            var templatedParent = element.TemplatedParent as FrameworkElement;
            return templatedParent != null ? GetTemplatedAncestor<T>(templatedParent) : null;
        }


        private void tvProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = GetTemplatedAncestor<Telerik.Windows.Controls.RadTreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                var tmp = item.DataContext as GrpComSingleMuliViewModel.TreeNodeBaseNode;
                if (tmp != null) tmp.OnDoubleClick();
            }
        }
    }
}
