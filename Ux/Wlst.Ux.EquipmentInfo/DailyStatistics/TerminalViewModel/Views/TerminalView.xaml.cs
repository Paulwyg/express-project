using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Wlst.Ux.EquipmentInfo.DailyStatistics.TerminalViewModel.ViewModel;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.TerminalViewModel.Views
{
    /// <summary>
    /// TerminalView.xaml 的交互逻辑
    /// </summary>
    public partial class TerminalView : UserControl
    {
        public TerminalView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\开关灯信息.png";
            var picturename = new Grid();
            switch (ViewModel.TerminalViewModel.PassName)
            {

                case "全部":
                    picturename = this.terminal;
                    break;
                case "开关灯信息":
                    picturename = openorclose;
                    break;
                case "操作信息":
                    picturename = RadChart1;
                    break;
                case "故障统计":
                    picturename = Fault;
                    break;
                case "能耗统计":
                    picturename = RadChart3;
                    break;
            }
            string extension = "png";
            SaveFileDialog dialog = new SaveFileDialog()
                                        {
                                            DefaultExt = extension,
                                            Filter =
                                                String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                                              extension,
                                                              "Picture"),
                                            FilterIndex = 1,
                                            FileName = ViewModel.TerminalViewModel.PassName + " " + ViewModel.TerminalViewModel.PassDate.ToString("yyyy-MM-dd") + " " + ViewModel.TerminalViewModel.PassRtuName 
                                        };
            if (dialog.ShowDialog() == true)
            {
                using (Stream fileStream = dialog.OpenFile())
                {
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(picturename, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
            //CheckBox chk = (CheckBox) sender;
            //if (chk.IsChecked == true)
            //    chk.Content = 0;
            //chart = new RadCartesianChart();
            //chart.HorizontalAxis = new DateTimeCategoricalAxis() { PlotMode = AxisPlotMode.OnTicksPadded, LabelFormat = "MM-dd" };
            //chart.VerticalAxis = new LinearAxis();
            //var line = new LineSeries();
            //line.Stroke = new SolidColorBrush(Colors.Red);
            //line.StrokeThickness = 2;
            //foreach (var f in ViewModel.TerminalViewModel.opitem)
            //{
            //    line.DataPoints.Add(new CategoricalDataPoint() { Value = f.Count, Category = f.DtCreateTime });
            //}
            //chart.Series.Add(line);
            //chart.SetValue(Grid.RowProperty, 1);
            //this.root.Children.Add(chart);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\操作信息.png";
            string extension = "png";
            SaveFileDialog dialog = new SaveFileDialog()
                                        {
                                            DefaultExt = extension,
                                            Filter =
                                                String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                                              extension,
                                                              "Picture"),
                                            FilterIndex = 1
                                        };
            if (dialog.ShowDialog() == true)
            {
                using (Stream fileStream = dialog.OpenFile())
                {
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart1, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\故障统计.png";
            string extension = "png";
            SaveFileDialog dialog = new SaveFileDialog()
                                        {
                                            DefaultExt = extension,
                                            Filter =
                                                String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                                              extension,
                                                              "Picture"),
                                            FilterIndex = 1
                                        };
            if (dialog.ShowDialog() == true)
            {
                using (Stream fileStream = dialog.OpenFile())
                {
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart2, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\能耗统计.png";
            string extension = "png";
            SaveFileDialog dialog = new SaveFileDialog()
                                        {
                                            DefaultExt = extension,
                                            Filter =
                                                String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                                              extension,
                                                              "Picture"),
                                            FilterIndex = 1
                                        };
            if (dialog.ShowDialog() == true)
            {
                using (Stream fileStream = dialog.OpenFile())
                {
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart3, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart1, "Print Test");
            }
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart2, "Print Test");
            }
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart3, "Print Test");
            }
        }

        private void Button_Click_(object sender, RoutedEventArgs e)
        {
            var picturename = new Grid();
            switch (ViewModel.TerminalViewModel.PassName)
            {

                case "全部":
                    picturename = this.terminal;
                    break;
                case "开关灯信息":
                    picturename = openorclose;
                    break;
                case "操作信息":
                    picturename = RadChart1;
                    break;
                case "故障统计":
                    picturename = Fault;
                    break;
                case "能耗统计":
                    picturename = RadChart3;
                    break;
            }
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(picturename, ViewModel.TerminalViewModel.PassName);
            }
        }

        private DataPointInfo tmp = null;
        private void ChartTrackBallBehavior_TrackInfoUpdated(object sender, TrackBallInfoEventArgs e)
        {
            foreach (DataPointInfo info in e.Context.DataPointInfos)
            {
                var x = info.DisplayContent.ToString().Replace("Value", "数量");
                info.DisplayContent = x.Split('\r')[0];
                tmp = info;
                info.DisplayHeader = "Custom data point header";
            }
            if (e.Context.DataPointInfos.Count == 0) tmp = null;
        }

        private void ChartTrackBallBehavior_TrackInfoUpdated_1(object sender, TrackBallInfoEventArgs e)
        {
            foreach (DataPointInfo info in e.Context.DataPointInfos)
            {
                var x = info.DisplayContent.ToString().Replace("Value", "操作");
                info.DisplayContent = x.Replace("Category","时间");
                tmp = info;
                info.DisplayHeader = "Custom data point header";
            }
            if (e.Context.DataPointInfos.Count == 0) tmp = null;
        }

        private void ChartTrackBallBehavior_TrackInfoUpdated_2(object sender, TrackBallInfoEventArgs e)
        {
            var nameitems = new List<Run>();
            if (pow.IsChecked == true) nameitems.Add(this.power);
            if (ope.IsChecked == true) nameitems.Add(this.open);
            if (clo.IsChecked == true) nameitems.Add(this.close);
            if (loopoth.IsChecked == true) nameitems.Add(this.loopother);
            if (terminaloth.IsChecked == true) nameitems.Add(this.terminalother);
            if (sum.IsChecked == true) nameitems.Add(this.sumnumber);
            //foreach (DataPointInfo info in e.Context.DataPointInfos)
            //{
            //    foreach (var f in nameitems)
            //    {
            //        f.Text = info.DisplayContent.ToString().Split('\r')[0];
            //    }
            //}
            if (nameitems.Count != e.Context.DataPointInfos.Count) return;
            for (int i = 0; i < nameitems.Count; i++)
            {
                nameitems[i].Text = e.Context.DataPointInfos[i].DisplayContent.ToString().Split('\r')[0].Split(':')[1].Trim();
            }
        }

    }
}
