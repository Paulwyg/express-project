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
using Wlst.Ux.Wj6005Module.Jd601ManageViewModel.Services;

namespace Wlst.Ux.Wj6005Module.Jd601ManageViewModel.Views
{
    /// <summary>
    /// Jd601ManageView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(  Ux.Wj6005Module.Services.ViewIdAssign.Jd601ManageViewId,
        AttachRegion = Ux.Wj6005Module.Services.ViewIdAssign.Jd601ManageViewAttachRegion
       )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601ManageView : UserControl
    {
        public Jd601ManageView()
        {
            InitializeComponent();
        }

        [Import]
        public IIJd601ManageView Model
        {
            get { return DataContext as IIJd601ManageView; }
            set { DataContext = value; }
        }
    

    public void tvProperties_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 如果使用附加属性来开启右键选中功能，
            // 那么在这里面的代码发生在TreeViewHelper中的代码之后，逻辑正确
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

        private T GetTemplatedAncestor<T>(FrameworkElement element) where T : FrameworkElement
        {
            if (element is T)
            {
                return element as T;
            }

            var templatedParent = element.TemplatedParent as FrameworkElement;
            return templatedParent != null ? GetTemplatedAncestor<T>(templatedParent) : null;
        }


        private bool first = true;
        private void tvProperties_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (first == false) return;
            first = false;

            this.Width = this.ActualWidth - 1;
            this.Width = this.ActualWidth + 1;

            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            var item = GetTemplatedAncestor<Telerik.Windows.Controls.RadTreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
        }
    }
}
