using System;
using System.Collections.Generic;
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
using Telerik.Windows.Controls.ChartView;
using WindowForWlst;
using Wlst.Ux.Statistics.UxStatistics.ViewModel;

namespace Wlst.Ux.Statistics.UxStatistics.Views
{
    /// <summary>
    /// BarView.xaml 的交互逻辑
    /// </summary>
    public partial class SingleBarView : CustomChromeWindow
    {
        public SingleBarView()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "数据统计";
        }

        public void SetContext(StatisticsViewModel x, string name)
        {
            DataContext = x;
            //this.PieData = x;
        }

        private DataPointInfo tmp = null;
        private void ChartTrackBallBehavior_TrackInfoUpdated(object sender, TrackBallInfoEventArgs e)
        {

            foreach (DataPointInfo info in e.Context.DataPointInfos)
            {
                tmp = info;
                info.DisplayHeader = "Custom data point header";
            }
            if (e.Context.DataPointInfos.Count == 0) tmp = null;




        }


        private void RadChartBar_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (tmp == null) return;
            //tb2.Text = tmp.DisplayContent.ToString();
            var piontInfo = tmp.DataPoint.DataItem as Data;

        }
    }
}
