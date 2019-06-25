using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.Services;

namespace Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.Views
{
    /// <summary>
    /// Wj1050ManageView.xaml 的交互逻辑  Wj1050ModuleWj1050ManageViewModel
    /// </summary>
    [ViewExport(
      Wj1090Module.Services.ViewIdAssign.Wj1090ManageViewId,
       Wj1090Module .Services .ViewIdAssign .Wj1090ManageViewAttachRegion  )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090ManageView : UserControl
    {
        public Wj1090ManageView()
        {
            InitializeComponent();
        }
        [Import]
        public IIWj1090ManageViewModel Model
        {
            get { return DataContext as IIWj1090ManageViewModel; }
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
      
    }
}
