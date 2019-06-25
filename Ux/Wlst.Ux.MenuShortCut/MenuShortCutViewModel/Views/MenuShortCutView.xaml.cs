using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DragDropExtend.ExtensionsHelper;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.MenuShortCut.MenuShortCutViewModel.Services;
using Wlst.Ux.MenuShortCut.Services;


namespace Wlst.Ux.MenuShortCut.MenuShortCutViewModel.Views
{
    /// <summary>
    /// MenuShortCutView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( ViewIdAssign.MenuShortCutViewId,
    AttachRegion = DocumentRegionName.DocumentRegion)]
    public partial class MenuShortCutView : UserControl
    {
        public MenuShortCutView()
        {
            InitializeComponent();
        }

        [Import]
        public IIMenuShortCutViewModel Model
        {
            get
            {
                return DataContext as IIMenuShortCutViewModel;
            }
            set
            {
                DataContext = value;
            }
        }

        public void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            TreeViewItem item =
                VisualTreeExtensions.GetTemplatedAncestor<TreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);


            //public enum ModifierKeys
            //{
            //    None = 0,
            //    Alt = 1,
            //    Control = 2,
            //    Shift = 4,
            //    Windows = 8,
            //}

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key != Key.None)
            {
                Model.ShortCuts = "Ctrl + " + e.Key.ToString();
            }
            else if (Keyboard.Modifiers == ModifierKeys.None && e.Key != Key.None)
            {
                Model.ShortCuts = e.Key.ToString();
            }
            else if (Keyboard.Modifiers == ModifierKeys.Shift && e.Key != Key.None)
            {
                Model.ShortCuts = "Shift + " + e.Key.ToString();
            }
            else if (Keyboard.Modifiers == ModifierKeys.Alt && e.Key != Key.None)
            {
                Model.ShortCuts = "Alt + " + e.Key.ToString();
            }
        }


    }
}
