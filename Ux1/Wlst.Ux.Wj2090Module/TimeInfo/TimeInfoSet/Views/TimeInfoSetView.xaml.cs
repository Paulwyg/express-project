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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.Wj2090Module.TimeInfo.Services;

namespace Wlst.Ux.Wj2090Module.TimeInfo.Views
{
    /// <summary>
    /// TimeInfoSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj2090Module.Services.ViewIdAssign.TimeInfoSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeInfoSetView : UserControl
    {
        public TimeInfoSetView()
        {
            InitializeComponent();
        }


        [Import]
        public IITimeInfoSet Model
        {
            get { return DataContext as IITimeInfoSet; }
            set
            {
                DataContext = value;
                value.OnBackNeedShowCtrlView += new EventHandler(value_OnBackNeedShowCtrlView);
            }
        }

        #region  布局控制


 
        /// <summary>
        /// 显示控制器列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void value_OnBackNeedShowCtrlView(object sender, EventArgs e)
        {

            var fawi = father.ActualWidth;
            var TwoWidth = Two.ActualWidth > 980 ? Two.ActualWidth : 980;
         //   TwoWidth = Two.DesiredSize.Width > TwoWidth ? Two.DesiredSize.Width : TwoWidth;

            var OneWidth = One.ActualWidth > 910 ? One.ActualWidth : 910;
          //  OneWidth = One.DesiredSize.Width > OneWidth ? One.DesiredSize.Width : OneWidth;
           var  ThreeWidth = Three.ActualWidth > 550 ? Three.ActualWidth : 550;
           //ThreeWidth = Three.DesiredSize.Width > ThreeWidth ? Three.DesiredSize.Width : ThreeWidth;

            if (fawi > OneWidth + TwoWidth + 30+ThreeWidth )
            {
                One.Visibility = Visibility.Visible;
                Two.Visibility = Visibility.Visible;
                Three.Visibility = Visibility.Visible ;
                One.Margin = new Thickness(10, 0, 0, 0);
                Two.Margin = new Thickness(OneWidth + 20, 0, 0, 0);
                Three.Margin = new Thickness(OneWidth+TwoWidth  + 30, 0, 0, 0);
                return;
            }

            if (fawi >  TwoWidth + 20 + ThreeWidth)
            {
                One.Visibility = Visibility.Collapsed;
                Two.Visibility = Visibility.Visible;
                Three.Visibility = Visibility.Visible;
              //  One.Margin = new Thickness(10, 0, 0, 0);
                Two.Margin = new Thickness(10, 0, 0, 0);
                Three.Margin = new Thickness(TwoWidth + 20, 0, 0, 0);
                return;
            }

            One.Visibility = Visibility.Collapsed;
            Two.Visibility = Visibility.Collapsed;
            Three.Visibility = Visibility.Visible;
            Three.Margin = new Thickness(10, 0, 0, 0);
        }

        /// <summary>
        /// 显示集中器列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Model != null) 
            {
                if ( Model.HaveSlu() == false)
                {
                    WlstMessageBox.Show("方案设置有遗漏,请完善该方案", "该区域没有可用的集中器与控制器...", WlstMessageBoxType.Ok);
                    return;
                }
            }
            var fawi = father.ActualWidth;
            var TwoWidth = Two.ActualWidth > 980 ? Two.ActualWidth : 980;
         //   TwoWidth = Two.DesiredSize.Width > TwoWidth ? Two.DesiredSize.Width : TwoWidth;

            var OneWidth = One.ActualWidth > 910 ? One.ActualWidth : 910;
          //  OneWidth = One.DesiredSize.Width > OneWidth ? One.DesiredSize.Width : OneWidth;

            if (fawi > OneWidth + TwoWidth + 20)
            {
                One.Visibility = Visibility.Visible;
                Two.Visibility = Visibility.Visible;
                Two.Margin = new Thickness(OneWidth + 20, 0, 0, 0);
                Three.Visibility = Visibility.Collapsed;
            }
            else
            {
                One.Visibility = Visibility.Collapsed;
                Two.Visibility = Visibility.Visible;
                Two.Margin = new Thickness(10, 0, 0, 0);
                Three.Visibility = Visibility.Collapsed;
            }
           
        }

        /// <summary>
        /// 完成集中器设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Model != null)
            {
                Model.OnUserSetOverSelectedSefDef();
              var nts=  Model.OnUserSetOverSlus();
                if(nts ==false)
                {
                    WlstMessageBox.Show("方案设置有遗漏,请完善该方案", "未设置任何使用该方案的集中器与控制器...", WlstMessageBoxType.Ok);
                    return;
                }
            }

            One.Visibility = Visibility.Visible ;
            One.Margin = new Thickness(10, 0, 0, 0);
            Two.Visibility = Visibility.Collapsed;
            Three.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// 完成控制器选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Model != null)
                Model.OnUserSetOverSelectedSefDef();

            this.Button_Click(null, null);
        }
        #endregion

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {


        }

        private void TextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            if (tb == null) return;
            var a = Convert.ToInt64(tb.Text.Replace(" ", ""));
            if (a > 0 && a < 10)
            {
                tb.Text = "0" + a;
            }
        }
    }
}
