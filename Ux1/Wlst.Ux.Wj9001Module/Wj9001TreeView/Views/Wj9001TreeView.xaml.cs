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
using Telerik.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj9001Module.Wj9001TreeView.Sercives;

namespace Wlst.Ux.Wj9001Module.Wj9001TreeView.Views
{
    /// <summary>
    /// Wj9001TreeView.xaml 的交互逻辑
    /// </summary>

    [ViewExport(Wj9001Module.Services.ViewIdAssign.Wj9001TreeViewId,
     AttachRegion = Wj9001Module.Services.ViewIdAssign.Wj9001ParaSetViewAttachRegion
      )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj9001TreeView : UserControl
    {
        public Wj9001TreeView()
        {
            InitializeComponent();
        }
        [Import]
        public IIWj9001TreeView Model
        {
            get { return DataContext as IIWj9001TreeView; }
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

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Model.SearchNodeold(Model.SearchText);
            }
        }
    }
}
