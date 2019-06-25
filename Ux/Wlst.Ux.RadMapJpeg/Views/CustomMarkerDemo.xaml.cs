using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;
//using Microsoft.Practices.Prism.MefExtensions.Event;
//using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Microsoft.Practices.Prism.ViewModel;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.RadMapJpeg.MapJepg.Services;
using Wlst.Ux.RadMapJpeg.Resources;

namespace Wlst.Ux.RadMapJpeg.Views
{
    /// <summary>
    /// CustomMarkerDemo.xaml 的交互逻辑
    /// </summary>
    public partial class CustomMarkerDemo : UserControl, INotifyPropertyChanged
    {
  
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            this.RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged(String propertyName)
        {
            VerifyPropertyName(propertyName);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Warns the developer if this Object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(String propertyName)
        {
            // verify that the property name matches a real,  
            // public, instance property on this Object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }




    }


    public partial class CustomMarkerDemo
    {
        public const int ImageWidth = 18;
        public const int ImageHeiht = 24;



        public  static CustomMarkerDemo CurrentRtu;

        private Popup Popup;
        private TextBlock Label;
        public GMapMarker Marker;
        private GMapControl gmap;
        private int RtuId;

        public CustomMarkerDemo(GMapControl map, GMapMarker marker, int rtuId)
        {
            this.InitializeComponent();

            ImgVis = Visibility.Visible;
            EquipmentRtuId = rtuId;
            this.gmap = map;
            this.Marker = marker;
            Marker.Tag = rtuId;

            Marker.Offset = new System.Windows.Point(-ImageWidth/2 , -ImageHeiht/2 );

            this.DataContext = this;
            Popup = new Popup();
            Label = new TextBlock();

            this.Unloaded += new RoutedEventHandler(CustomMarkerDemo_Unloaded);
            this.SizeChanged += new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);

            this.MouseEnter += new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave += new MouseEventHandler(MarkerControl_MouseLeave);
            this.MouseMove += new MouseEventHandler(CustomMarkerDemo_MouseMove);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);
            this.MouseRightButtonDown += new MouseButtonEventHandler(CustomMarkerDemo_MouseRightButtonDown);

            Popup.Placement = PlacementMode.Mouse;
            {
                Label.Background = Brushes.White  ;
                Label.Foreground = Brushes.Black;
                Label.Padding = new Thickness(0);
               // Label.FontSize = 22;
                //Binding bindChinese = new Binding("Tooltips");
                //Label.SetBinding(TextBlock.TextProperty, bindChinese);

               // Label.Text = "fsdfds";
            }
            Popup.Child = Label;
        }

        void CustomMarkerDemo_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
 
            OnEquipmentSelected();
            //throw new NotImplementedException();
        }


        private void CustomMarkerDemo_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= new RoutedEventHandler(CustomMarkerDemo_Unloaded);

            this.SizeChanged -= new SizeChangedEventHandler(CustomMarkerDemo_SizeChanged);
            this.MouseEnter -= new MouseEventHandler(MarkerControl_MouseEnter);
            this.MouseLeave -= new MouseEventHandler(MarkerControl_MouseLeave);
            this.MouseMove -= new MouseEventHandler(CustomMarkerDemo_MouseMove);
            this.MouseLeftButtonUp -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonUp);
            this.MouseLeftButtonDown -= new MouseButtonEventHandler(CustomMarkerDemo_MouseLeftButtonDown);

            Marker.Shape = null;
            icon.Source = null;
            icon = null;
            Popup = null;
            Label = null;
        }

        private void CustomMarkerDemo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Marker.Offset = new Point(-e.NewSize.Width/2, -e.NewSize.Height/2);
        }


        private bool isMoved = false;
        private void CustomMarkerDemo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && IsMouseCaptured &&
                (Keyboard.Modifiers == ModifierKeys.Control))
            {
                Point p = e.GetPosition(gmap);
                Marker.Position = gmap.FromLocalToLatLng((int) (p.X), (int) (p.Y));
                isMoved = true;
            }
        }

        private void CustomMarkerDemo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (!IsMouseCaptured)
            {
                Mouse.Capture(this);
            }
            if (Keyboard.Modifiers == ModifierKeys.Control)
                gmap.CanDragMap = false;

            OnEquipmentSelected();
        }

        private void CustomMarkerDemo_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
            }
            gmap.CanDragMap = true;
            if(isMoved )
            {
                UpdateEquipmentLocationToSvr();
                isMoved = false;
            }
        }

        private void MarkerControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Marker.ZIndex -= 100000;
            Popup.IsOpen = false;

            icon.Width = icon.Width - 10;
            icon.Height = icon.Height - 10;
        }

        private void MarkerControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Marker.ZIndex += 100000;
            var nf = Popup.Child as TextBlock;
            if (nf != null) nf.Text = Tooltips;
            Popup.IsOpen = true;

            icon.Width = icon.Width + 10;
            icon.Height = icon.Height + 10;
        }


        private bool issel;

        public bool IsSelected
        {
            get { return issel; }
            set
            {
                if (value == issel) return;
                issel = value;
                var nf = Popup.Child as TextBlock;
                if (nf != null) nf.Text = Tooltips;
                Popup.IsOpen = issel;
            }
        }
    }




    public partial class CustomMarkerDemo 
    {
     

        /// <summary>
        /// 为了节约程序运行资源  当用户选中终端时  刷新菜单
        /// 否则即使终端参数进行更新亦不刷新菜单
        /// 标志菜单是否允许刷新
        /// </summary>
        private bool IsCanRefreshMenu { get; set; }

        #region

        private Visibility _visi;

        /// <summary>
        /// 设置节点是否可见
        /// </summary>
        public Visibility Visi
        {
            get { return _visi; }
            set
            {
                if (value == _visi) return;
                _visi = value;
                this.RaisePropertyChanged(() => this.Visi);
            }
        }


        private bool _selectedaddten = false;
        /// <summary>
        /// 当节点被选中的时候调用，实现了刷新右键菜单；
        /// 是否需要发送事件需要在此实现;以及其他的需要处理的事件;
        /// 动态加载子节点
        /// </summary>
        public void OnEquipmentSelected(bool selectbytree=false )
        {
            if (CurrentRtu != null && CurrentRtu != this && CurrentRtu.Cm != null)
            {
                CurrentRtu.Cm.Items.Clear();
            }
            if (CurrentRtu == this) return;
            
                if (CurrentRtu != null)
                {
                    CurrentRtu.Popup.IsOpen = false;
                    if (CurrentRtu._selectedaddten)
                    {
                        CurrentRtu.icon.Width -= 10;
                        CurrentRtu.icon.Height -= 10;
                        CurrentRtu._selectedaddten = false;
                    }
                }
                CurrentRtu = this;
            
            if (selectbytree && gmap != null)
            {
                gmap.Position = Marker.Position;
              
                //var nf = Popup.Child as TextBlock;
                //if (nf != null) nf.Text = Tooltips;
                //Popup.IsOpen = true;
            }
  
            if (_selectedaddten == false)
                {
                    icon.Width += 10;
                    icon.Height += 10;
                    _selectedaddten = true;
                }
            IsCanRefreshMenu = true;
            ResetCm();
            IsCanRefreshMenu = false;

            if (selectbytree == false)
            {
                //发布事件  选中当前节点
                var args = new PublishEventArgs
                               {
                                   EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                   EventType = PublishEventType.Core,
                                   EventAttachInfo = "gis"
                               };
                args.AddParams(EquipmentRtuId);
                EventPublish.PublishEvent(args );;
            }
        }


        public void UpdateEquipmentLocationToSvr()
        {
            //var nfx =
            //    WGSGCJLatLonHelper.GCJ02ToWGS84(new LatLngPoint(this.Marker.Position.Lat, this.Marker.Position.Lng));
            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .OnEquipmentMentMapLocationChangeByMap(
                EquipmentRtuId, this.Marker.Position.Lat, this.Marker.Position.Lng);
        }

        public void UpdateEquipmentLocationLo(double xmap, double ymap)
        {
         //   var tmx = WGSGCJLatLonHelper.WGS84ToGCJ02(new LatLngPoint(ymap, xmap));

            this.Marker.Position = new  PointLatLng(xmap > ymap ? ymap : xmap, xmap > ymap ? xmap : ymap);

           // this.Marker.Position = new PointLatLng(xmap,ymap);
            Marker.Offset = new System.Windows.Point(-ImageWidth/2, -ImageHeiht/2);

        }


        public bool CanUpdateImage()
        {
            var tu = GetImageIcon();
            if (tu == null) return false ;
            if (tu.Item1 != EquipmentImageState) return true;
            if (tu.Item2 != _fixInfo) return true;
            if (tu.Item3 != _tmlRemark) return true;
            return false;
        }

        /// <summary>
        /// 图标 安装位置  备注  如果图标为0保持原图标
        /// </summary>
        /// <param name="equipmentRtuId"></param>
        /// <returns></returns>
            Tuple<int, string, string> GetImageIcon()
        {

            if (
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                    EquipmentRtuId) == false) return null;
            {

                var equ =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                        EquipmentRtuId];
                var addr = equ.RtuInstallAddr;
                var remark = equ.RtuRemark;

                // this.RaisePropertyChanged(() => this.Tooltips);

                if (equ.EquipmentType == WjParaBase.EquType.Rtu)
                {

                    var s = equ.RtuStateCode;
                    if (s == 0)
                    {
                        return new Tuple<int, string, string>(3011, addr, remark);
                    }
                    if (s == 1)
                    {
                        return new Tuple<int, string, string>(3012, addr, remark);
                    }
                    if (RunningInfoHold.Info.ContainsKey(EquipmentRtuId) == false ||
                        RunningInfoHold.Info[EquipmentRtuId].IsOnLine == false)
                    {
                        return new Tuple<int, string, string>(3019, addr, remark);
                    }
                    if (Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.ContainsKey(EquipmentRtuId) == false)
                        return new Tuple<int, string, string>(0, addr, remark); ;


                    var tmp = Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[EquipmentRtuId];

                    var haserror = false;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                        haserror = tmp.ErrorCount > 0;

                    var lighton = tmp.IsLightHasElectric; // RtuNewDataService.IsRtuHasElectric(this.EquipmentRtuId);
                    int errorindex = 0;
                    if (haserror && lighton) errorindex = 3;
                    if (haserror && !lighton) errorindex = 1;
                    if (!haserror && lighton) errorindex = 2;

                    return new Tuple<int, string, string>(3015 + errorindex, addr, remark);

                }
                else
                {
                    if (Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.ContainsKey(EquipmentRtuId) == false)
                        return new Tuple<int, string, string>(0, addr, remark); ;
                    var tmp = Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[EquipmentRtuId];
                    var haserror = false;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                        haserror = tmp.ErrorCount > 0;
                    // Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.IsRtuHasError(this.EquipmentRtuId);
                    //    var lighton = Wlst.Sr.EquipmentNewData.Services.RtuNewDataService.IsRtuHasElectric(this.EquipmentRtuId);
                    int st = (int) equ.RtuModel + (haserror ? 1 : 0);

                    return new Tuple<int, string, string>(st, addr, remark);
                }

            }
        }

        public void UpdateTmlStateInfomation()
        {

            if (
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .ContainsKey(
                    EquipmentRtuId))
            {
                
                var equ =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                        EquipmentRtuId];
                _fixInfo=equ.RtuInstallAddr;
                _tmlRemark = equ.RtuRemark;
                this.RaisePropertyChanged(() => this.Tooltips);

                if (equ.EquipmentType ==WjParaBase.EquType.Rtu )
                {

                    var s = equ.RtuStateCode ;

                    //lvf 2018年11月19日14:10:04 区别亮化
                    var tpye = equ as Wj3005Rtu;
                    if (tpye == null) return;
                    int rtuType = 3100;// 3015;

                    var rtuUseType =tpye.WjVoltage.RtuUsedType;
                    if ( tpye.WjVoltage != null)
                    {
                        if (rtuUseType == 1) rtuType = 3100;
                        if (rtuUseType == 2) rtuType = 3200;

                    }
                    int errorindex = 0;


                    if (s == 0)
                    {
                        EquipmentImageState = rtuType+8;//3011;  不用 深咖啡
                        return;
                    }
                    if (s == 1)
                    {
                        EquipmentImageState = rtuType+6;// 3012; 停运  红色
                        return;
                    }
                    if (RunningInfoHold.Info.ContainsKey(EquipmentRtuId)==false ||
                        RunningInfoHold.Info[EquipmentRtuId].IsOnLine ==false)
                    {
                        EquipmentImageState = rtuType+7;// 3019; 离线  灰色
                        return;
                    }
                    if (Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.ContainsKey(EquipmentRtuId) == false)
                        return;


                    var tmp = Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[EquipmentRtuId];

                    var haserror = false;
                    var isonline = tmp.IsOnLine;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                        haserror = tmp.ErrorCount >0;

                    var lighton = tmp.IsLightHasElectric;// RtuNewDataService.IsRtuHasElectric(this.EquipmentRtuId);
                    //int errorindex = 0;
                    if (haserror && lighton) errorindex = 3;
                    if (haserror && !lighton) errorindex = 1;
                    if (!haserror && lighton) errorindex = 2;

                    EquipmentImageState = rtuType+errorindex;//3015 + errorindex;
                }
                else if(equ.EquipmentType ==WjParaBase.EquType.Slu)
                {
                    if (Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.ContainsKey(EquipmentRtuId) == false)
                        return;
                    var tmp = Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[EquipmentRtuId];
                    var haserror = false;
                    var isonline = tmp.IsOnLine;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                        haserror = tmp.ErrorCount > 0;
                    // Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.IsRtuHasError(this.EquipmentRtuId);
                    //    var lighton = Wlst.Sr.EquipmentNewData.Services.RtuNewDataService.IsRtuHasElectric(this.EquipmentRtuId);
                    EquipmentImageState = (int)equ.RtuModel + (isonline ? 0 : 3);
                }

                else
                {
                    if (Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info.ContainsKey(EquipmentRtuId) == false)
                        return;
                    var tmp = Sr.EquipmentInfoHolding.Services.RunningInfoHold.Info[EquipmentRtuId];
                    var haserror = false;
                    if (UxTreeSetting.IsRutsNotShowError == false)
                        haserror = tmp.ErrorCount >0;
                      // Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.IsRtuHasError(this.EquipmentRtuId);
                    //    var lighton = Wlst.Sr.EquipmentNewData.Services.RtuNewDataService.IsRtuHasElectric(this.EquipmentRtuId);
                    EquipmentImageState = (int)equ.RtuModel + (haserror ? 1 : 0);
                }

            }
        }

        private object _imagesIcon;

        /// <summary>
        /// 前台界面绑定的图标
        /// </summary>
        public object ImagesIcon
        {
            get { return _imagesIcon; }
            set
            {
                if (_imagesIcon != value)
                {
                    _imagesIcon = value;
                    this.RaisePropertyChanged(() => this.ImagesIcon);
                }
            }
        }

        private Visibility _imgVis;
        public Visibility ImgVis
        {
            get { return _imgVis; }
            set
            {
                if (_imgVis != value)
                {
                    _imgVis = value;
                    this.RaisePropertyChanged(() => this.ImgVis);
                }
            }
        }


 
        private int _equipmentImageState;

        public int EquipmentImageState
        {
            get { return _equipmentImageState; }
            set
            {
                if (_equipmentImageState != value)
                {
                    _equipmentImageState = value;
                    ImagesIcon = ImageResource.GetEquipmentIcon(_equipmentImageState);
                    //this.RaisePropertyChanged(() => this.ImagesIcon);
                }
            }
        }

        public  int _phyId;
        private string _fixInfo;
        private string _tmlRemark;
        private string _anzhuangxinxi;

        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public string Tooltips
        {
            get
            {
                string str = @"";
                str += "终端地址：" + _phyId;
                str += Environment.NewLine;
                str += "终端名称：" + EquipmentName;
                str += "\r\n";
                str += "安装信息: " + _fixInfo;
                str += "\r\n";
                str += "终端备注: " + _tmlRemark;
                str += "\r\n";
                str += "开通时间: " + _anzhuangxinxi;
                //if (DataHolding.Setting.IsShowGrpInTreeModelShowId)
                //    return string.Format("{0:D4}", EquipmentRtuId) + "-" + EquipmentName;
                //else
                return str;
            }
        }

        private int _equipmentRtuId;

        /// <summary>
        /// 节点ID  终端地址或分组地址  
        /// </summary>
        public int EquipmentRtuId
        {
            get { return _equipmentRtuId; }
            set
            {
                if (_equipmentRtuId != value)
                {
                    _equipmentRtuId = value;
                    this.RaisePropertyChanged(() => this.EquipmentRtuId);
                  
                    var tt = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById(value);
                    if (tt == null) return;
                    _fixInfo = tt.RtuRemark ;
                    _tmlRemark = tt.RtuRemark;
                    _phyId = Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(value) ? tt.RtuPhyId  : value;
                    _anzhuangxinxi = new DateTime(tt.DateCreate ).ToString("yyyy-MM-dd");  
                    this.RaisePropertyChanged(() => this.Tooltips);
                }
            }
        }


        private string _equipmentName;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public string EquipmentName
        {
            get { return _equipmentName; }
            set
            {
                if (_equipmentName != value)
                {
                    _equipmentName = value;
                    this.RaisePropertyChanged(() => this.EquipmentName);
                    this.RaisePropertyChanged(() => this.Tooltips);
                }
            }
        }


        private string _text;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    this.RaisePropertyChanged(() => this.Text);
                }
            }
        }

        #endregion

        #region 右键菜单  在节点被选中的时候显示刷新右键菜单

        /// <summary>
        /// 菜单
        /// </summary>
        public ContextMenu Cm
        {
            get
            {

                return _cm;
            }
            set
            {
                if (_cm == value) return;
                _cm = value;
                this.RaisePropertyChanged(() => this.Cm);
            }
        }

        void ResetCm()
        {
            if (!IsCanRefreshMenu) return;
            ResetContextMenu(EquipmentRtuId);
            return;
            //ContextMenu cm = null;
            //cm = new ContextMenu() {BorderThickness = new Thickness(0)};

            //if (
            //    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.
            //        ContainsKey(EquipmentRtuId))
            //{
            //    var s =
            //        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
            //            EquipmentRtuId];

            //    var t = MenuBuilding.BulidCm(s.RtuModel.ToString(CultureInfo.InvariantCulture), false, s);
            //    var tmp = MenuBuilding.HelpCmm(t);
            //    //  HelpCmm(t);
            //    foreach (var f in tmp) cm.Items.Add(f);
            //}

            //var mi = new MenuItem();
            //mi.Header = this.EquipmentName;
            //mi.IsEnabled = false;
            //if (cm.Items.Count > 0)
            //    cm.Items.Insert(0, mi);
            //else cm.Items.Add(mi);
            //Cm = cm;
        }

        private ContextMenu _cm = null;

        private void HelpCmm(ObservableCollection<IIMenuItem> t)
        {
            _cm.Items.Clear();
            var gg = MenuBuilding.HelpCmm(t);
            foreach (var f in gg)
            {
                _cm.Items.Add(f);
            }
            return;
        
        }

      
        #endregion



        #region  Reset ContextMenu

        public void ResetContextMenu(int nodeId)
        {
            UpdateCm(nodeId);
        }


        private ObservableCollection<IIMenuItem> items;
        public ObservableCollection<IIMenuItem> CmItems
        {
            get { return items; }
            set
            {
                if (value == items) return;
                items = value;
                this.RaisePropertyChanged(() => this.CmItems);
            }
        }


        public void UpdateCm(int rtuId)
        {
            if (
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .ContainsKey(
                   rtuId))
            {
                var tt =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [rtuId];

                var tmt = MenuBuilding.BulidCm(((int ) tt.RtuModel).ToString(), false, tt);
                if(tmt !=null )
                {
                    tmt.Insert(0, new MenuItemBase()
                                      {
                                          Text = tt.RtuPhyId .ToString("d3") + "-" + tt.RtuName,
                                          IsEnabled = false,
                                          TextTmp = tt.RtuPhyId.ToString("d3") + "-" + tt.RtuName,
                                      });
                }
                CmItems = tmt;
            }
        }

        #endregion

    }
}
