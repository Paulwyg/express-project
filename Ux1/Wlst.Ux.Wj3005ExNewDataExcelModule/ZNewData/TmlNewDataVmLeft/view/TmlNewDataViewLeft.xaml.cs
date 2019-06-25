using System;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Wj3005ExNewDataExcelModule.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel;
using System.Collections.Generic;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.view
{
    /// <summary>
    /// TmlNewDataViewLeft.xaml 的交互逻辑
    /// </summary>
   [ViewExport(ViewIdAssign.TmlNewDataViewLeftId,
       AttachRegion = ViewIdAssign.TmlNewDataViewAttachRegion 
       )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlNewDataViewLeft : UserControl
    {

   

        public TmlNewDataViewLeft()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WindowsLoaded);

            lp1234lploopinfo.EnableRowVirtualization = false;
        }

        [Import]
        public IINewDataVmLeft Model
        {
            get { return DataContext as IINewDataVmLeft; }
            set
            {
                DataContext = value;
                value.LoopCountChanged += new EventHandler<EventArsgLoopCount>(value_LoopCountChanged);
                //loopinfo .Margin = new Thickness(value.Left , 35 , 0, 0);
                //value.VisiChanged += (sender, args) => { dcx.IsVisible = (Visibility)sender == Visibility.Visible; };
                //value.CompareVisiChanged += (sender, args) =>
                //{
                //    yesterA.IsVisible = (Visibility)sender == Visibility.Visible;
                //    yesterV.IsVisible = (Visibility)sender == Visibility.Visible;
                //};
                //value.DetailVisiChanged += (sender, args) =>
                //{
                //    RefA.IsVisible = (Visibility)sender == Visibility.Visible;
                //    adLower.IsVisible = (Visibility)sender == Visibility.Visible;
                //    adRatio.IsVisible = (Visibility)sender == Visibility.Visible;
                //    adUpper.IsVisible = (Visibility)sender == Visibility.Visible;

                //};
                //value.OnlineVisiChanged += (sender, args) =>
                //{
                //    adRate.IsVisible = (Visibility)sender == Visibility.Visible;
                //};

                //value.MarginChanged += (sender, args) =>
                //{
                //    lp1234lploopinfo.Margin = new Thickness((double)sender, 0, 0, 0);
                //};
            }
        }

        public const string XmlConfigName = "DisplayIndex\\Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft";
        private int _loopcount = 0;
        private bool isShowHis = false;

        private void WindowsLoaded(object sender, RoutedEventArgs e)
        {
            //LoadDisplayIndex();
            //if (_newDataColumnsDisplayIndex == null || _newDataColumnsDisplayIndex.Count == 0) return;
            //foreach (var g in lp1234lploopinfo.Columns)
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

            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.LoadDisplayIndex(lp1234lploopinfo.Columns, XmlConfigName);
        }
        void value_LoopCountChanged(object sender, EventArsgLoopCount e)
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
            var tmp = e.IsShowPro;

            while (tmp.Count < 19) tmp.Add(false);
            xh0.IsVisible = tmp[0];
            xh1.IsVisible = tmp[1];
            xh2.IsVisible = tmp[2];
            xh3.IsVisible = tmp[3];
            xh4.IsVisible = tmp[4];
            xh5.IsVisible = tmp[5];
            xh6.IsVisible = tmp[6];
            xh7.IsVisible = tmp[7];
            xh8.IsVisible = tmp[8];
            xh17.IsVisible = tmp[18];
            xh10.IsVisible = tmp[9] != false && tmp[10];
            xh11.IsVisible = tmp[9] != false && tmp[11];
            xh12.IsVisible = tmp[9] != false && tmp[12];
            xh13.IsVisible = tmp[9] != false && tmp[13];
            xh18.IsVisible = Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 5) == true;
            xh19.IsVisible = tmp[20];
            //rrrr.Visibility = Visibility.Collapsed;
            //rrbb.Visibility = Visibility.Collapsed;
            //rrcc.Visibility = Visibility.Collapsed;
            //gggg.Visibility = tmp[9] ? Visibility.Visible : Visibility.Collapsed;
        
            isShowHis = tmp[9];
            if(tmp[9])
            {
                if( Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 7, true ))
                {
                    tm.Visibility = Visibility.Collapsed;

                    tm1.Visibility = Visibility.Visible;
                  
                   
                }else
                {
                    tm.Visibility = Visibility.Visible;
                    tm1.Visibility = Visibility.Collapsed;
                   
                }

                rrrr.Visibility = Visibility.Visible;

            }else
            {
                tm.Visibility = Visibility.Collapsed;
                tm1.Visibility = Visibility.Collapsed;
                rrrr.Visibility = Visibility.Collapsed;
            }

            _loopcount = e.LoopCount;
          
            foreach (var f in lp1234lploopinfo.Items)
            {
                GridViewRowItem row = lp1234lploopinfo.ItemContainerGenerator.ContainerFromItem(f) as GridViewRowItem;
                if (row != null)
                {
                    if (row.DataContext is LoopInfoLeft)
                    {
                        var sp = row.DataContext as LoopInfoLeft;
                        if (sp.Indexr  <= e.LoopCount)
                        {
                            var cc = (Color)ColorConverter.ConvertFromString(sp.Backgroundx);
                            row.Background = new SolidColorBrush(cc);
                            //  row.Height = _rouheight;
                        }
                        //else
                        //{
                        //    row.Height = 0;
                        //}

                        //  ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Gray);
                    }
                }
            }
           
            if (sc != null)
                sc.ScrollToTop();

            lp1234lploopinfo.SelectedItem = null;
        }


        private double _rowheight = 0;

        private ScrollViewer sc = null;
        private void lp1234lploopinfo_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sc == null)
            {
                sc = grd.GetChildObject<ScrollViewer>(lp1234lploopinfo, typeof(ScrollViewer));
                if (sc == null) return;
                _rowheight = Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightt;
                //_dataheight = _rowheight*56;
            }
            if (sc == null) return;

            bool isUpMove = e.Delta > 0;

            if (isUpMove == false)
            {
                if (sc.VerticalOffset + sc.ViewportHeight + _rowheight - 0.1 < _loopcount * _rowheight)
                {
                    sc.ScrollToVerticalOffset(sc.VerticalOffset + _rowheight);
                    e.Handled = true;
                    return;
                }

                if (sc.VerticalOffset + sc.ViewportHeight + 0.01 < _loopcount * _rowheight)
                {
                    sc.ScrollToVerticalOffset(_loopcount * _rowheight - sc.ViewportHeight + 5);
                    e.Handled = true;
                    return;
                }
                e.Handled = true;
            }
            //e.Handled = true;
        }


        private void lp1234lploopinfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (sc == null)
                {
                    sc = grd.GetChildObject<ScrollViewer>(lp1234lploopinfo, typeof(ScrollViewer));
                    if (sc == null) return;
                    _rowheight = Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightt;
                    //_dataheight = _rowheight*56;
                }
                if (sc == null) return;




                if (sc.VerticalOffset + sc.ViewportHeight - 0.1 < _loopcount * _rowheight)
                {
                    //sc.ScrollToVerticalOffset(sc.VerticalOffset + _rowheight);
                    //e.Handled = true;
                    return;
                }

                //if (sc.VerticalOffset + sc.ViewportHeight + 0.01 < _loopcount*_rowheight)
                //{
                //    sc.ScrollToVerticalOffset(_loopcount*_rowheight - sc.ViewportHeight + 3);
                //    e.Handled = true;
                //    return;
                //}
                e.Handled = true;

            }
        }


        private void TextBlock_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.LeftButton == MouseButtonState.Released)
            {
                Model.MeasureRtu();
            }
        }



        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed || e.LeftButton == MouseButtonState.Released)
            {
                Model.RequestNearData();
            }
        }

        //private void loopinfo_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Row.DataContext is LoopInfoLeft)
        //        {
        //            var sp = e.Row.DataContext as LoopInfoLeft;
        //            if (sp == null) return;
        //            var cc = (Color)ColorConverter.ConvertFromString(sp.Backgroundx);
        //            if (cc == null) return;

        //            e.Row.Background = new SolidColorBrush(cc);
        //            //  ? new SolidColorBrush(Colors.White) : new SolidColorBrush(Colors.Gray);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        public Color ToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", string.Empty);
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new Color()
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }

        private void lp1234lploopinfo_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            int x = 0;
            x += 9;
            x += 12;
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void rrrr_MouseLeave(object sender, MouseEventArgs e)
        {
            //if (isShowHis == false) return;
            //gggg.Visibility = Visibility.Visible ;
            //rrrr.Visibility = Visibility.Collapsed;
            //rrbb.Visibility = Visibility.Collapsed;
            //rrcc.Visibility = Visibility.Collapsed;
        }

        private void gggg_MouseEnter(object sender, MouseEventArgs e)
        {
            //if (isShowHis == false) return;
            //gggg.Visibility = Visibility.Collapsed;
            //rrrr .Visibility =Visibility.Visible;
            //rrbb.Visibility = Visibility.Visible;
            //rrcc.Visibility = Visibility.Visible;
        }
        //private void LoadDisplayIndex()
        //{
        //    _newDataColumnsDisplayIndex.Clear();

        //    var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
        //    foreach (var g in info)
        //    {
        //        _newDataColumnsDisplayIndex.Add(g.Key, g.Value);
        //    }
        //}
        //private Dictionary<string, string> _newDataColumnsDisplayIndex = new Dictionary<string, string>();
        private void lp1234lploopinfo_ColumnReordered(object sender, GridViewColumnEventArgs e)
        {
            //_newDataColumnsDisplayIndex.Clear();
            //foreach (var g in lp1234lploopinfo.Columns)
            //{
            //    _newDataColumnsDisplayIndex.Add(g.Header.ToString(), g.DisplayIndex.ToString());
            //}


            //Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(_newDataColumnsDisplayIndex, XmlConfigName);
            Wlst.Cr.CoreOne.Services.LoadSaveDisplayIndex.SaveDisplayIndex(lp1234lploopinfo.Columns, XmlConfigName);
        }

        private void lp1234lploopinfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
              var listView = sender as Telerik.Windows.Controls.RadGridView;
            if (listView == null) return;
            var ggg = listView.CurrentCellInfo;
            var mvvm = ggg.Item as LoopInfoLeft;

            int index = mvvm.LoopId -1 ;
       
            RapidSetRtuAmp.NavToRapidSetRtuAmp.InitWin(index , Model.RtuId, Model.LoopxInfo);
        }




    }

}
