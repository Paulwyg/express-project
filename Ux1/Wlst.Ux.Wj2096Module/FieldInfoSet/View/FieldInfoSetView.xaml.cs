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
using Wlst.Ux.Wj2096Module.FieldInfoSet.Services;
using Wlst.Ux.Wj2096Module.FieldInfoSet.ViewModel;

namespace Wlst.Ux.Wj2096Module.FieldInfoSet.View
{
    /// <summary>
    /// FieldInfoSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Ux.Wj2096Module.Services.ViewIdAssign.Wj2096SluInfoSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class FieldInfoSetView : UserControl
    {
        public FieldInfoSetView()
        {
            InitializeComponent();
        }

        [Import]
        public IIConcentratorParaInformationViewModel Model
        {
            get { return DataContext as IIConcentratorParaInformationViewModel; }
            set { DataContext = value; }
        }

        private void txtBarCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            if (tb == null) return;
            var tmp = tb.Text.Replace(" ", "");

            if (Model.Is2096 == true)
            {
                if (tmp.Length == 13)
                {
                    var a = Convert.ToInt64(tmp);
                    //if (a > 4294967295)
                    //{
                    //    MessageBox.Show(" 条形码不得超过：4294967295 ", "条形码出错", MessageBoxButton.OK);
                    //    tb.Text = "";
                    //}
                    //else
                    //{
                        if (ScanMode.IsChecked == true)
                        {
                            FrameworkElement fsource = e.Source as FrameworkElement;
                            fsource.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        }
                    //}

                }
            }else
            {
                //是2290设备
                if (tmp.Length == 15)
                {
                    var a = Convert.ToInt64(tmp);
                    //if (a > 4294967295)
                    //{
                    //    MessageBox.Show(" 条形码不得超过：4294967295 ", "条形码出错", MessageBoxButton.OK);
                    //    tb.Text = "";
                    //}
                    //else
                    //{
                        if (ScanMode.IsChecked == true)
                        {
                            FrameworkElement fsource = e.Source as FrameworkElement;
                            fsource.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                        }
                    //}

                }
            }

        }

        void OnLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.PreviewMouseDown += new MouseButtonEventHandler(OnPreviewMouseDown);
        }

        void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.Focus();
            e.Handled = true;
        }

        void OnGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.SelectAll();
            tb.PreviewMouseDown -= new MouseButtonEventHandler(OnPreviewMouseDown);
        }


        private void txtName_LostFocus(object sender, RoutedEventArgs e)
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
                TreeItemGrplViewModel item = ((RadTreeViewItem)father).DataContext as TreeItemGrplViewModel;
                if (item.TxbNameVisi == Visibility.Visible)
                {
                    item.StopEditName();
                }
            }
        }

        private void TreeViewItem_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Model.TreeView_PreviewMouseRightButtonDown(sender, e);
        }

        private void TreeViewItem_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 那么在这里面的代码发生在PreviewMouseRightButtonDown中的代码之后，逻辑正确
            Model.TreeView_MouseRightButtonDown(sender, e);
        }

    
    }
}
