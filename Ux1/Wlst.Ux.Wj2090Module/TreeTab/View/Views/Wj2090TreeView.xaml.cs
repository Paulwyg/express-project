using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj2090Module.TreeTab.View.Serivices;

namespace Wlst.Ux.Wj2090Module.TreeTab.View.Views
{
    /// <summary>
    /// Wj2090TreeView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj2090Module.Services.ViewIdAssign.Wj2090TreeViewId,
        AttachRegion = Wj2090Module.Services.ViewIdAssign.Wj2090TreeViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj2090TreeView : UserControl
    {
        public Wj2090TreeView()
        {
            InitializeComponent();
        }

        [Import]
        public IIAreaTree Model
        {
            get { return DataContext as IIAreaTree; }
            set { DataContext = value; }
        }

        //[Import]
        //public IIWj2090Tree Model
        //{
        //    get { return DataContext as IIWj2090Tree; }
        //    set { DataContext = value; }
        //}
        
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
