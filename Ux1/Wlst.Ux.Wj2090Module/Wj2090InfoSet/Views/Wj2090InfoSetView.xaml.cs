using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using DragDropExtend.ExtensionsHelper;
using Wlst.Cr.Core.Behavior;
using System.ComponentModel.Composition;
using Wlst.Ux.Wj2090Module.Wj2090InfoSet.Services;
using Telerik.Windows.Controls;
using Wlst.Ux.Wj2090Module.Wj2090InfoSet.ViewModel;

namespace Wlst.Ux.Wj2090Module.Wj2090InfoSet.Views
{
    /// <summary>
    /// Wj2090InfoSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj2090Module.Services.ViewIdAssign.Wj2090SluInfoSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj2090InfoSetView : UserControl
    {
        public Wj2090InfoSetView()
        {
            InitializeComponent();
        }

        [Import]
        public IIConcentratorParaInformationViewModel Model
        {
            get { return DataContext as IIConcentratorParaInformationViewModel; }
            set { DataContext = value; }
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
          
            if (Model != null)
                Model.ShowSndInfo = "";
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

        private int count = 0;
        private void TextBox_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            count++;
            if (count >= 5)
            {
                if (Model != null) Model.IsEnableCore = true;
                count = 0;
            }
        }

        private void txtBarCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            if (tb == null) return;
            var tmp = tb.Text.Replace(" ","");
            if(tmp.Length == 13)
            {
               var a= Convert.ToInt64(tmp );
               if (a > 4294967295)
               {
                   MessageBox.Show(" 条形码不得超过：4294967295 ", "条形码出错", MessageBoxButton.OK);
                   tb.Text = "";
               }
               else
               {
                   if (ScanMode.IsChecked == true)
                   {
                       FrameworkElement fsource = e.Source as FrameworkElement;
                       fsource.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                   }
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

        private void txtBarCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                TextBox tb = e.Source as TextBox;
                if (tb == null) return;
                var tmp = tb.Text.Replace(" ", "");
                if (tmp.Length == 13)
                {
                    var a = Convert.ToInt64(tmp);
                    if (a > 4294967295)
                    {
                        MessageBox.Show(" 条形码不得超过：4294967295 ", "条形码出错", MessageBoxButton.OK);
                        tb.Text = "";
                    }
                    else
                    {

                        FrameworkElement fsource = e.Source as FrameworkElement;
                        fsource.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                    }

                }else if (tmp.Length<13)
                {
                    var tmpsss = tmp.PadLeft(13, '0');
                    tb.Text = tmpsss;
                    FrameworkElement fsource = e.Source as FrameworkElement;
                    fsource.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                }
                else
                {
                    MessageBox.Show(" 条形码超过13位 ", "条形码出错", MessageBoxButton.OK);
                    tb.Text = "";
                }


            }
        }


        private void ScrollToEndlvf()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            //lblHello.Content = "欢迎你光临WPF的世界,Dispatcher";

            this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate()
            {

                txtShowInfo.ScrollToEnd();

            });
            
        }

    }
}
