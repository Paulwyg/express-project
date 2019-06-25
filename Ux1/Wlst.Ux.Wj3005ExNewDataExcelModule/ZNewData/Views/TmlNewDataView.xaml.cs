using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj3005ExNewDataExcelModule.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.NewDataSetting;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataViewModel.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataViewModel.ViewModel;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.Services;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.Views
{
    /// <summary>
    /// TmlNewDataView.xaml 的交互逻辑
    /// </summary>

    //[ViewExport(
    //    AttachNow = true,
    //    AttachRegion = Services .ViewIdAssign .TmlNewDataViewAttachRegion  ,
    //    ID = Services .ViewIdAssign .TmlNewDataViewId )]
    //[PartCreationPolicy(CreationPolicy.Shared)]

    [ViewExport( ViewIdAssign.TmlNewDataViewId ,
        AttachRegion = ViewIdAssign.TmlNewDataViewAttachRegion
        )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlNewDataView : UserControl
    {
        public TmlNewDataView()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(WindowsLoaded);
        }
        private void WindowsLoaded(object sender, RoutedEventArgs e)
        {
            //LoadDisplayIndex();
            //if (_newDataColumnsDisplayIndex == null || _newDataColumnsDisplayIndex.Count == 0) return;
            //foreach (var g in loopinfo.Columns)
            //{
            //    foreach (var j in _newDataColumnsDisplayIndex)
            //    {
            //        if (g.Header.ToString() == j.Key)
            //        {
            //            g.DisplayIndex = int.Parse(j.Value);
            //            break;
            //        }
            //    }
            //}

            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.LoadDisplayIndex(loopinfo.Columns, XmlConfigName);
        }

        public const string XmlConfigName = "DisplayIndex\\Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.Views.TmlNewDataView";

        [Import]
        public IINewDataViewModel Model
        {

            get { return DataContext as IINewDataViewModel; }
            set
            {
                NewDataSettingViewModel.LoadNewDataLenghtSetConfgX();
                DataContext = value;

                //loopinfo .Margin = new Thickness(value.Left , 35 , 0, 0);
                //value.VisiChanged += (sender, args) => { dcx.Visibility = (Visibility)sender; };
                value.LoopCountChanged += (sender, args)
                                          =>
                                              {
                                                  Application.Current.Dispatcher.Invoke(
                                                      new Action(() =>
                                                                     {
                                                                         LoopCount_Changed(sender, args);
                                                                     }


                                                          ));
                                              };

                //value.CompareVisiChanged += (sender, args) =>
                //{
                //    yesterA.Visibility = (Visibility)sender;
                //    yesterV.Visibility = (Visibility)sender;
                //};
                //value.DetailVisiChanged += (sender, args) =>
                //{
                //    RefA.Visibility = (Visibility)sender;
                //    adLower.Visibility = (Visibility)sender;
                //    adRatio.Visibility = (Visibility)sender;
                //    adUpper.Visibility = (Visibility)sender;

                //};
                //value.OnlineVisiChanged += (sender, args) =>
                //{
                //    adRate.Visibility = (Visibility)sender;
                //};
                value.MarginChanged += (sender, args) =>
                                           {
                                               loopinfo.Margin = new Thickness((double) sender, 0, 0, 0);
                                           };
            }
        }

        private void LoopCount_Changed(object sender,EventArsgLoopCount args)
        {
                                                          // 0 、序号
                                                  // 1 、回路名称
                                                  // 2 、参考电流
                                                  // 3、 亮灯率
                                                  // 4、 功率因数


                                                  // 5、 互感器比
                                                  // 6、 回路上限
                                                  // 7 、回路下限
                                                  // 8、 线路状态


                                                  // 9 、昨日数据
                                                  // 10、状态
                                                  // 11、电压
                                                  // 12、电流
                                                  // 13、功率

                                                  // 14、手动选测自动显示数据
                                                  // 15、显示回路数据电压电流等单位
                                                  // 16、历史数据查询显示高级选项
                                                  var tmp = args.IsShowPro;
                                                  while (tmp.Count < 21) tmp.Add(false);
                                                  cx0.IsVisible  = tmp[0]  ;
                                                  cx1.IsVisible = tmp[1];
                                                  cx2.IsVisible = tmp[2] ;
                                                  cx3.IsVisible = tmp[3] ;
                                                  cx4.IsVisible = tmp[4] ;
                                                  cx5.IsVisible = tmp[5] ;
                                                  cx6.IsVisible = tmp[6] ;
                                                  cx7.IsVisible = tmp[7] ;
                                                  cx08.IsVisible = tmp[8] ;
                                                  cx12.IsVisible = tmp[9] && tmp[12] ;
                                                  cx13.IsVisible = tmp[9] && tmp[13] ;
                                                  cx17.IsVisible = tmp[18] ;
                                                  cx18.IsVisible =
                                                      Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 5) == true;
                                                  cx19.IsVisible = tmp[20];
        }

        //[Import]
        //public IINewDataViewModel Model
        //{
        //    get { return DataContext as IINewDataViewModel; }
        //    set
        //    {
        //        DataContext = value;
        //        //loopinfo .Margin = new Thickness(value.Left , 35 , 0, 0);
        //        value.VisiChanged += (sender, args) => { dcx.IsVisible = (Visibility)sender == Visibility.Visible; };
        //        value.CompareVisiChanged += (sender, args) =>
        //        {
        //            yesterA.IsVisible = (Visibility)sender == Visibility.Visible;
        //            yesterV.IsVisible = (Visibility)sender == Visibility.Visible;
        //        };
        //        value.DetailVisiChanged += (sender, args) =>
        //        {
        //            RefA.IsVisible = (Visibility)sender == Visibility.Visible;
        //            adLower.IsVisible = (Visibility)sender == Visibility.Visible;
        //            adRatio.IsVisible = (Visibility)sender == Visibility.Visible;
        //            adUpper.IsVisible = (Visibility)sender == Visibility.Visible;

        //        };
        //        value.OnlineVisiChanged += (sender, args) =>
        //        {
        //            adRate.IsVisible = (Visibility)sender == Visibility.Visible;
        //        };
        //        value.MarginChanged += (sender, args) =>
        //        {
        //            loopinfo.Margin = new Thickness((double)sender, 0, 0, 0);
        //        };
        //    }
        //}

        private void TextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.LeftButton == MouseButtonState.Released)
            {
                Model.MeasureRtu();
            }
        }

        //private Dictionary<string, string> _newDataColumnsDisplayIndex = new Dictionary<string, string>();
       
        //private void LoadDisplayIndex()
        //{
        //    _newDataColumnsDisplayIndex.Clear();
           
        //    var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
        //    foreach (var g in info)
        //    {
        //        _newDataColumnsDisplayIndex.Add(g.Key, g.Value);
        //    }
        //}


        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.LeftButton == MouseButtonState.Released)
            {
                Model.RequestNearData();
            }
        }


        private void loopinfo_ColumnReordered(object sender, GridViewColumnEventArgs e)
        {
            //_newDataColumnsDisplayIndex.Clear();
            //foreach (var g in loopinfo.Columns)
            //{
            //    _newDataColumnsDisplayIndex.Add(g.Header.ToString(),g.DisplayIndex.ToString());
            //}


            //Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(_newDataColumnsDisplayIndex, XmlConfigName);
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.SaveDisplayIndex(loopinfo.Columns, XmlConfigName);
        }

        private void loopinfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
          //  RapidSetRtuAmp.NavToRapidSetRtuAmp.InitWin(loopinfo.SelectedIndex, Model.RtuId, Model.LoopxInfo);


            var listView = sender as Telerik.Windows.Controls.RadGridView;
            if (listView == null) return;
            var ggg = listView.CurrentCellInfo;
            var mvvm = ggg.Item as LoopInfox;
            if (mvvm == null) return;
            int index = mvvm.LoopId - 1;
            RapidSetRtuAmp.NavToRapidSetRtuAmp.InitWin(index, Model.RtuId, Model.LoopxInfo);

        }

        private void loopinfo_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            try
            {
                if (e.Row.DataContext is LoopInfox)
                {
                    var sp = e.Row.DataContext as LoopInfox;
                    if (sp == null) return;
                    var cc = (Color)ColorConverter.ConvertFromString(sp.Backgroundx);
                    if (cc == null) return;

                    e.Row.Background = new SolidColorBrush(cc);
                    //  ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Gray);
                }
            }
            catch (Exception ex)
            {

            }
        }
        //public Color ToColor(string colorName)
        //{
        //    if (colorName.StartsWith("#"))
        //        colorName = colorName.Replace("#", string.Empty);
        //    int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
        //    return new Color()
        //    {
        //        A = Convert.ToByte((v >> 24) & 255),
        //        R = Convert.ToByte((v >> 16) & 255),
        //        G = Convert.ToByte((v >> 8) & 255),
        //        B = Convert.ToByte((v >> 0) & 255)






























        //    };
        //}
    }

}
