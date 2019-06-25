using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj1050Module.Wj1050ManageViewModel.Sercives;

namespace Wlst.Ux.Wj1050Module.Wj1050ManageViewModel.Views
{
    /// <summary>
    /// Wj1050ManageView.xaml 的交互逻辑  Wj1050ModuleWj1050ManageViewModel
    /// </summary>
    [ViewExport( Wj1050Module .Services .ViewIdAssign .Wj1050ManageViewId ,
       AttachRegion = Wj1050Module .Services .ViewIdAssign .Wj1050ManageViewAttachRegion 
        )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1050ManageView : UserControl
    {
        public Wj1050ManageView()
        {
            InitializeComponent();
            LoadXml();
        }
        [Import]
        public IIWj1050ManageViewModel Model
        {
            get { return DataContext as IIWj1050ManageViewModel; }
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
        private int SearchLimit = 0;

        private void LoadXml()
        {
            var infos = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("TabTreeSetConfg");
            if (infos.ContainsKey("SearchLimit"))
            {
                SearchLimit = Convert.ToInt32(infos["SearchLimit"]);
            }
            else SearchLimit = 0;

            

        }
        private void tvProperties_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
           
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

        public void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 如果使用附加属性来开启右键选中功能，
            // 那么在这里面的代码发生在TreeViewHelper中的代码之后，逻辑正确
        }
        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchLimit != 1) return;
                Model.SearchNodeold(Model.SearchText);
            }
        }
    }
}
