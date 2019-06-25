using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj1080Module.Wj1080ManageViewModel.Sercives;

namespace Wlst.Ux.Wj1080Module.Wj1080ManageViewModel.Views
{
    /// <summary>
    /// Wj1080ManageView.xaml 的交互逻辑 Wj1080ModuleWj1080ManageView
    /// </summary>
    [ViewExport( Services .ViewIdAssign .Wj1080ManageViewId,
        AttachRegion = Wj1080Module .Services .ViewIdAssign .Wj1080ManageViewAttachRegion 
         )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1080ManageView : UserControl
    {
        public Wj1080ManageView()
        {
            InitializeComponent();

          //  this.Width = this.ActualWidth - 1;
        }


        [Import]
        public IIWj1080ManageViewModel Model
        {
            get { return DataContext as IIWj1080ManageViewModel; }
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
        private void tvProperties_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (first == false) return;
            first = false;

            this.Width = this.ActualWidth-1;
            this.Width = this.ActualWidth +1;

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
