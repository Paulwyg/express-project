using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.TopDataInfo;
using Wlst.Ux.StateBarModule.Services;
using Wlst.Ux.StateBarModule.StateBarInBottom.Services;
using Wlst.Ux.StateBarModule.TopData.Views;


namespace Wlst.Ux.StateBarModule.StateBarInBottom.Views
{
    /// <summary>
    /// StateBarView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( ViewIdAssign.StateBarViewId,AttachNow = true, 
        AttachRegion = ViewIdAssign.StateBarViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class StateBarView : UserControl
    {
        public StateBarView()
        {
            InitializeComponent();
            InitMainWindowInfo();
        }

        public void InitMainWindowInfo()
        {
            var raise = Wlst.Cr.CoreOne.Services.ImageSourceHelper.MySelf.GetImageSourceById(10000102);
            if (raise != null) sunraise.Source = raise;


            var down = Wlst.Cr.CoreOne.Services.ImageSourceHelper.MySelf.GetImageSourceById(10000101);
            if (down != null) sunset.Source = down;

            var sunlux = Wlst.Cr.CoreOne.Services.ImageSourceHelper.MySelf.GetImageSourceById(10000104);
            if (sunlux != null) sunvalue.Source = sunlux;
        }

        private Button _button;


        [Import]
        public IIStateBarViewModule Model
        {
            get { return DataContext as IIStateBarViewModule; }
            set { DataContext = value; }
        }

        private void TextBlock_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //双击查看即将执行的任务安排

            var infox = TopDataInfoServers.MySelf.GetDataInfo(2);
            if(infox !=null )
            {
                  MessageShowInfo.Show(infox.Item1, infox.Item2);
            }
          
        }

        private void TextBlock_MouseUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //双击查看即将执行的时间表安排
            var infox = TopDataInfoServers.MySelf.GetDataInfo(3);

            if (infox != null)
            {
                MessageShowInfo.Show(infox.Item1, infox.Item2);
            }
        }

        private void TextBlock_MouseUp_2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var xg = sender as TextBlock;
            if (xg == null) return;
            MessageShowInfo.Show("光控详情", xg.ToolTip.ToString());  //xg.text
        }

        private void TextBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.LeftButton == MouseButtonState.Released)
            {
                Model .ClearErrNum();
            }
        }



        private void TextBox1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.LeftButton == MouseButtonState.Released)
            {
                Model.ClearEmergencyNum();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            if (tb == null) return;
            var a= Convert.ToInt64(tb.Text.Replace(" ",""));
            if (a>0)
            {
                var colorBrush = new SolidColorBrush(Color.FromArgb(255, 255, 117, 117));
                ErrNumsBackColor.Background = colorBrush;
            }
            if(a==0)
            {
                var colorBrush = new SolidColorBrush(Color.FromArgb(255, 147, 196,125));
                ErrNumsBackColor.Background = colorBrush;
            }
        }

       

        private void TextBox_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
 RegionManage.ShowViewByIdAttachRegionWithArgu(1102805, 0);
        }
    };

  
}