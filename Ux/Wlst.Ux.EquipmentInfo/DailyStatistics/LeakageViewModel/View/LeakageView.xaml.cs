using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using Telerik.Windows.Controls.ChartView;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.LeakageViewModel.View
{
    /// <summary>
    /// LeakageView.xaml 的交互逻辑
    /// </summary>
    public partial class LeakageView : UserControl
    {
        public LeakageView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string extension = "png";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = extension,
                Filter =
                    String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                  extension,
                                  "Picture"),
                FilterIndex = 1,
                FileName = "漏电信息"
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream fileStream = dialog.OpenFile())
                {
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(Leak, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(Leak, "漏电信息");
            }
        }
        private DataPointInfo tmp = null;
        private void ChartTrackBallBehavior_TrackInfoUpdated(object sender, Telerik.Windows.Controls.ChartView.TrackBallInfoEventArgs e)
        {
            foreach (DataPointInfo info in e.Context.DataPointInfos)
            {
                var x = info.DisplayContent.ToString().Replace("Value", "漏电值");
                info.DisplayContent = x.Split('\r')[0] + "mA";
                //info.DisplayContent = "无数据"; ;
                tmp = info;
                info.DisplayHeader = "Custom data point header";
            }
            if (e.Context.DataPointInfos.Count == 0) tmp = null;
        }
    }
}
