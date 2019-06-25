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

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.SingleLampViewModel.View
{
    /// <summary>
    /// SingleLampView.xaml 的交互逻辑
    /// </summary>
    public partial class SingleLampView : UserControl
    {
        public SingleLampView()
        {
            InitializeComponent();          
        }

       
        private void Button_Click_(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\集中器通信成功走势图.png";
             var picturename = new Grid();
            switch (ViewModel.SingleLampViewModel.PassName)
            {

                case "全部":
                    picturename = this.single;
                    break;
                case "集中器时间表":
                    picturename = RadChart1;
                    break;
                case "单灯通信成功率(亮灯率)":
                    picturename = RadChart2;
                    break;
                case "故障统计":
                    picturename = RadChart3;
                    break;
                case "电量统计":
                    picturename = RadChart4;
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
                                            FileName = ViewModel.SingleLampViewModel.PassName + " " + ViewModel.SingleLampViewModel.PassDate.ToString("yyyy-MM-dd") + " " + ViewModel.SingleLampViewModel.PassSluName
                                        };
            if (dialog.ShowDialog() == true)
            {
                using (Stream fileStream = dialog.OpenFile())
                {
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(picturename, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string filename = Environment.CurrentDirectory + "\\Config\\集中器亮灯率走势图.png";
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
            string filename = Environment.CurrentDirectory + "\\Config\\集中器故障统计.png";
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string filename = Environment.CurrentDirectory + "\\Config\\集中器电量统计.png";
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
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart4, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var picturename = new Grid();
            switch (ViewModel.SingleLampViewModel.PassName)
            {

                case "全部":
                    picturename = this.single;
                    break;
                case "集中器时间表":
                    picturename = RadChart1;
                    break;
                case "单灯通信成功率(亮灯率)":
                    picturename = RadChart2;
                    break;
                case "故障统计":
                    picturename = RadChart3;
                    break;
                case "电量统计":
                    picturename = RadChart4;
                    break;
            }
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(picturename, ViewModel.SingleLampViewModel.PassName);
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

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart4, "Print Test");
            }
        }

        private DataPointInfo tmp = null;
        private void ChartTrackBallBehavior_TrackInfoUpdated(object sender, Telerik.Windows.Controls.ChartView.TrackBallInfoEventArgs e)
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
            var nameitems = new List<Run>();
            if (con.IsChecked == true) nameitems.Add(this.concentrator);
            if (clo.IsChecked == true) nameitems.Add(this.close);
            if (ope.IsChecked == true) nameitems.Add(this.open);
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
