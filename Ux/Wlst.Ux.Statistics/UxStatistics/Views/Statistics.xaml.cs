using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Microsoft.Win32;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Telerik.Windows.Data;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Statistics.UxStatistics.Services;
using Wlst.Ux.Statistics.UxStatistics.ViewModel;

namespace Wlst.Ux.Statistics.UxStatistics.Views
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>

    [ViewExport(Wlst.Ux.Statistics.Services.ViewIdAssign.UxStatisticsViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Statistics
    {
       
        public Statistics()
        {
            InitializeComponent();
        }


        [Import]
        public IIUxStatisticsModule Model
        {
            get { return DataContext as IIUxStatisticsModule; }
            set { DataContext = value; }

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






        private void RadChartLine_MouseDoubleClick1(object sender, System.Windows.Input.MouseButtonEventArgs e) //终端故障
        {
            if (tmp == null) return;
            var piontInfo =tmp.DataPoint.DataItem as Data;
            //tb1.Text = tmp.DisplayContent.ToString();

            Model.ShowDetailView(2, 1, piontInfo.Category);
        }
        private void RadChartLine_MouseDoubleClick2(object sender, System.Windows.Input.MouseButtonEventArgs e) //单灯故障
        {
            if (tmp == null) return;
            var piontInfo = tmp.DataPoint.DataItem as Data;
            //tb1.Text = tmp.DisplayContent.ToString();

            Model.ShowDetailView(2, 2, piontInfo.Category);
        }
        private void RadChartLine_MouseDoubleClick3(object sender, System.Windows.Input.MouseButtonEventArgs e) //上线率
        {
            if (tmp == null) return;
            var piontInfo = tmp.DataPoint.DataItem as Data;
            //tb1.Text = tmp.DisplayContent.ToString();

            Model.ShowDetailView(1, 3, piontInfo.Category);
        }
        private void RadChartLine_MouseDoubleClick4(object sender, System.Windows.Input.MouseButtonEventArgs e) //耗电量
        {
            if (tmp == null) return;
            var piontInfo = tmp.DataPoint.DataItem as Data;
            //tb1.Text = tmp.DisplayContent.ToString();

            Model.ShowDetailView(2, 4, piontInfo.Category);
        }
        private void RadChartLine_MouseDoubleClick5(object sender, System.Windows.Input.MouseButtonEventArgs e)//亮灯率
        {
            if (tmp == null) return;
            var piontInfo = tmp.DataPoint.DataItem as Data;
            //tb1.Text = tmp.DisplayContent.ToString();

            Model.ShowDetailView(1, 5, piontInfo.Category);  
        }
        private void RadChartLine_MouseDoubleClick6(object sender, System.Windows.Input.MouseButtonEventArgs e)//节能率
        {
            if (tmp == null) return;
            var piontInfo = tmp.DataPoint.DataItem as Data;
            //tb1.Text = tmp.DisplayContent.ToString();

            Model.ShowDetailView(1, 6, piontInfo.Category);


        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string extension = "xls";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter =
                    String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                  extension,
                                  "Excel"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    //  if (File.Exists(stream)) File.Delete(stream);
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToExcelMLImage(this.RadChartLine1, stream);
                   
                }

            }
     
            
        }

     
    }
}
