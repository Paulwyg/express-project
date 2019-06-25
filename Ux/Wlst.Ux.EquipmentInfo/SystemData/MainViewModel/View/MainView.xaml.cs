using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
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
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.EquipmentInfo.SystemData.MainViewModel.Services;
using Wlst.Ux.EquipmentInfo.SystemData.MainViewModel.ViewModel;

namespace Wlst.Ux.EquipmentInfo.SystemData.MainViewModel.View
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wlst.Ux.EquipmentInfo.Services.ViewIdAssign.SystemDataViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.MainViewModel(HistoryFault);
        }

        //[Import]
        //public IIMainViewModel Model
        //{
        //    get { return DataContext as IIMainViewModel; }
        //    set { DataContext = value; }
        //}

        private DataPointInfo tmp = null;
        private string type = "";
        private void ChartTrackBallBehavior_TrackInfoUpdated(object sender, TrackBallInfoEventArgs e)
        {
            foreach (DataPointInfo info in e.Context.DataPointInfos)
            {
                var x = info.DisplayContent.ToString().Replace("Value", "数量");
                type = x.Split(new char[]{':'})[2];
                info.DisplayContent = x.Split('\r')[0];

                tmp = info;
                info.DisplayHeader = "Custom data point header";
            }
            if (e.Context.DataPointInfos.Count == 0) tmp = null;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\时间表操作信息.png";
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
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.Canvas, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\单灯控制成功率走势图.png";
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
            //string filename = Environment.CurrentDirectory + "\\Config\\单灯亮灯率走势图.png";
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

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\历史故障走势图.png";
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
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart6, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\系统能耗统计图.png";
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
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart7, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\系统电流走势图.png";
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
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart8, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\单灯能耗统计图.png";
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
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart9, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\终端在线率走势图.png";
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
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart10, fileStream,
                                                                                 new PngBitmapEncoder());
                }
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            //string filename = Environment.CurrentDirectory + "\\Config\\系统电流走势图.png";
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
                    Telerik.Windows.Media.Imaging.ExportExtensions.ExportToImage(this.RadChart11, fileStream,
                                                                                 new PngBitmapEncoder());
                }
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

        private void Button_Click6(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart6, "Print Test");
            }
        }

        private void Button_Click7(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart7, "Print Test");
            }
        }

        private void Button_Click8(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart8, "Print Test");
            }
        }

        private void Button_Click9(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart9, "Print Test");
            }
        }

        private void Button_Click10(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart10, "Print Test");
            }
        }

        private void Button_Click11(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(RadChart11, "Print Test");
            }
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(Canvas, "Print Test");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var picturename = new Grid();
            switch (ViewModel.MainViewModel.PassName)
            {

                case "全部":
                    picturename = openorclose.IsSelected
                               ? OpenOrClose
                               : fault.IsSelected
                                     ? Fault
                                     : energy.IsSelected ? Energy : online.IsSelected ? Online : OpenOrClose;;
                    break;
                case "全年日出日落信息":
                    picturename = RadChart1;
                    break;
                case "时间表操作信息":
                    picturename = timetable;
                    break;
                case "集中器1周在线率曲线图":
                    picturename = RadChart2;
                    break;
                case "单灯亮灯率曲线图":
                    picturename = RadChart3;
                    break;
                case "现存故障分布图":
                    picturename = RadChart4;
                    break;
                case "今日报警故障分布图":
                    picturename = RadChart5;
                    break;
                case "历史故障曲线图":
                    picturename = RadChart6;
                    break;
                case "系统能耗统计图":
                    picturename = RadChart7;
                    break;
                case "系统功率曲线图":
                    picturename = RadChart8;
                    break;
                case "单灯能耗统计图":
                    picturename = RadChart9;
                    break;
                case "终端24小时在线率曲线图":
                    picturename = RadChart10;
                    break;
                case "集中器24小时在线率曲线图":
                    picturename = RadChart11;
                    break;
                case "终端1周在线率曲线图":
                    picturename = RadChart12;
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
                FileName = ViewModel.MainViewModel.PassName + " " + ViewModel.MainViewModel.PassDate.ToString("yyyy-MM-dd")
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

        private void Button_Click_(object sender, RoutedEventArgs e)
        {
            var picturename = new Grid();
            switch (ViewModel.MainViewModel.PassName)
            {

                case "全部":
                    picturename = openorclose.IsSelected
                               ? OpenOrClose
                               : fault.IsSelected
                                     ? Fault
                                     : energy.IsSelected ? Energy : online.IsSelected ? Online : OpenOrClose; ;
                    break;
                case "全年日出日落信息":
                    picturename = RadChart1;
                    break;
                case "时间表操作信息":
                    picturename = timetable;
                    break;
                case "集中器1周在线率曲线图":
                    picturename = RadChart2;
                    break;
                case "单灯亮灯率曲线图":
                    picturename = RadChart3;
                    break;
                case "现存故障分布图":
                    picturename = RadChart4;
                    break;
                case "今日报警故障分布图":
                    picturename = RadChart5;
                    break;
                case "历史故障曲线图":
                    picturename = RadChart6;
                    break;
                case "系统能耗统计图":
                    picturename = RadChart7;
                    break;
                case "系统功率曲线图":
                    picturename = RadChart8;
                    break;
                case "单灯能耗统计图":
                    picturename = RadChart9;
                    break;
                case "终端24小时在线率曲线图":
                    picturename = RadChart10;
                    break;
                case "集中器24小时在线率曲线图":
                    picturename = RadChart11;
                    break;
                case "终端1周在线率曲线图":
                    picturename = RadChart12;
                    break;
            }

            var dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(picturename, ViewModel.MainViewModel.PassName);
            }
        }

        private void ChartTrackBallBehavior_TrackInfoUpdated_1(object sender, TrackBallInfoEventArgs e)
        {
            foreach (DataPointInfo info in e.Context.DataPointInfos)
            {
                //var x = info.DisplayContent.ToString().Replace("Value", "无数据");
                info.DisplayContent = "无数据"; ;
                tmp = info;
                info.DisplayHeader = "Custom data point header";
            }
            if (e.Context.DataPointInfos.Count == 0) tmp = null;
        }

        private void RadChart4_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var name = "";
            foreach (var f in ViewModel.MainViewModel.PassTypeItems)
            {
                if (f.Name.Split('-')[0] == type.Trim())
                {
                    name = f.Name.Split('-')[1];
                    break;
                }
            }
            RegionManage.ShowViewByIdAttachRegionWithArgu(1103603, name);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var args = new PublishEventArgs()
            {
                EventType = "TabControlChanged",
                EventId = 3333
            };
            args.AddParams(openorclose.IsSelected, this);
            args.AddParams(fault.IsSelected, this);
            args.AddParams(energy.IsSelected, this);
            args.AddParams(online.IsSelected, this);
            EventPublish.PublishEvent(args);
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            var x = sender as TextBlock;
            if (x == null) return;
            x.Width = double.NaN;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            var x = sender as TextBlock;
            if (x == null) return;
            x.Width = 80;
        }

        private void ChartTrackBallBehavior_TrackInfoUpdated_2(object sender, TrackBallInfoEventArgs e)
        {
            DataPointInfo closestDataPoint = e.Context.ClosestDataPoint;
            if (closestDataPoint != null)
            {
                var data = closestDataPoint.DataPoint.DataItem as SunItem;
                if(data==null) return;
                this.date.Text = data.Time.ToString("MM-dd");
                this.sunrise.Text =(data.Value / 60).ToString(CultureInfo.InvariantCulture).PadLeft(2,'0') + ":" + (data.Value % 60).ToString(CultureInfo.InvariantCulture).PadLeft(2,'0');
                this.sunset.Text = (data.Value2 / 60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ":" + (data.Value2 % 60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            }
        }

        private void ChartTrackBallBehavior_TrackInfoUpdated_3(object sender, TrackBallInfoEventArgs e)
        {
            var nameitems = new List<Run>();
            var ischeck = false;
            foreach (var t in one.ItemsSource)
            {
                var xx = t as OperatorTypeItem;
                if (xx == null) return;
                if (xx.IsSelected) ischeck = true;
            }
            if (ter.IsChecked == true) nameitems.Add(this.terminal);
            else this.terminal.Text = "";
            if (ope.IsChecked == true) nameitems.Add(this.openclose);
            else this.openclose.Text = "";
            if (sin.IsChecked == true) nameitems.Add(this.single);
            else this.single.Text = "";
            if (oth.IsChecked == true) nameitems.Add(this.others);
            else this.others.Text = "";
            if (sum.IsChecked == true) nameitems.Add(this.sumnumber);
            else this.sumnumber.Text = "";
            if (ischeck == true) nameitems.Add(this.onefault);
            else this.onefault.Text = "";
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
