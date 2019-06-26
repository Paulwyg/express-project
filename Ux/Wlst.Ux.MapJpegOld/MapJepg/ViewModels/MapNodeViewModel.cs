using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Telerik.Windows.Controls.Map;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
 
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.RadMapJpeg.MapJepg.ViewModels
{
    /// <summary>
    /// 继承此类 需要实现：OnNodeSelect;ResetCm
    /// </summary>
    public class MapNodeViewModel : ObservableObject
    {
        public MapNodeViewModel()
        {
            this.ImageWidth = RadMapJpeg.MapJepg.ViewModels.RadMapJpegViewModel.IconWidths;
            this.ImageHeight = RadMapJpeg.MapJepg.ViewModels.RadMapJpegViewModel.IconWidths;
          VisiTooltips=Visibility.Collapsed;
            ImgVis = Visibility.Visible;
        }

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

        private Visibility _toolvisi;

        /// <summary>
        /// 设置节点是否可见
        /// </summary>
        public Visibility VisiTooltips
        {
            get { return _toolvisi; }
            set
            {
                if (value == _toolvisi) return;
                _toolvisi = value;
                this.RaisePropertyChanged(() => this.VisiTooltips);
            }
        }
        /// <summary>
        /// 当节点被选中的时候调用，实现了刷新右键菜单；
        /// 是否需要发送事件需要在此实现;以及其他的需要处理的事件;
        /// 动态加载子节点
        /// </summary>
        public void OnEquipmentSelected()
        {
            IsCanRefreshMenu = true;
            this.RaisePropertyChanged(() => this.Cm);
            IsCanRefreshMenu = false;
            //发布事件  选中当前节点
            var args = new PublishEventArgs
                           {
                               EventId =Sr .EquipmentInfoHolding .Services .EventIdAssign   .EquipmentSelected,
                               EventType = PublishEventType.Core,
                           };
            args.AddParams(EquipmentRtuId);
            EventPublisher.EventPublish(args);
        }

        private Location _equipmentLocation;

        public Location EquipmentLocation
        {
            get { return _equipmentLocation; }
            set
            {
                if (_equipmentLocation != value)
                {
                    _equipmentLocation = value;
                    this.RaisePropertyChanged(() => this.EquipmentLocation);
                    this.RaisePropertyChanged(() => this.TooltiopsLocation);
                }
            }
        }



        public Location TooltiopsLocation
        {
            get
            {
                return
                    new Location(_equipmentLocation.Latitude - Wlst.Ux.RadMapJpeg.Views.MapJpegView.ImageIconWidth*1.5,
                                 _equipmentLocation.Longitude + Wlst.Ux.RadMapJpeg.Views.MapJpegView.ImageIconWidth*1.5);
            }
        }


        public void UpdateEquipmentLocation()
        {
            Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.OnEquipmentMentMapLocationChangeByMap(
                EquipmentRtuId, EquipmentLocation.Latitude, EquipmentLocation.Longitude);

            //Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.OnEquipmentMentMapLocationChangeByMap(
            //    EquipmentRtuId, EquipmentLocation.Latitude, EquipmentLocation.Longitude);
        }


        public void UpdateTmlStateInfomation()
        {
            //if (EquipmentRunningInfoHolding.TmlRunningInfoDictionary.ContainsKey(this.NodeId))
            //{
            //    this.UpdateTerminalStateInfo(EquipmentRunningInfoHolding.TmlRunningInfoDictionary[this.NodeId]);
            //}
            if (
                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
                    EquipmentRtuId))
            {
                var equ =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
                        EquipmentRtuId];
                if (equ.RtuModel == 3005 || equ.RtuModel == 3090)
                {
                    var s = equ.RtuState;
                    if (s == 0)
                    {
                        EquipmentImageState = 3011;
                        return;
                    }
                    if (s == 1)
                    {
                        EquipmentImageState = 3012;
                        return;
                    }
                    var haserror =
                       Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.IsRtuHasError(this.EquipmentRtuId);
                    var lighton = RtuNewDataService.IsRtuHasElectric(this.EquipmentRtuId);
                    int errorindex = 0;
                    if (haserror && lighton) errorindex = 3;
                    if (haserror && !lighton) errorindex = 1;
                    if (!haserror && lighton) errorindex = 2;

                    EquipmentImageState = 3015 + errorindex;

 
                }
                else
                {
                    var haserror =
                       Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.IsRtuHasError(this.EquipmentRtuId);
                //    var lighton = Wlst.Sr.EquipmentNewData.Services.RtuNewDataService.IsRtuHasElectric(this.EquipmentRtuId);
                    EquipmentImageState = equ.RtuModel + (haserror?1:0);
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

        private int _imageWidth;

        public int ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                if (_imageWidth != value)
                {
                    _imageWidth = value;
                    this.RaisePropertyChanged(() => this.ImageWidth);
                }
            }
        }

        private int _imageHeight;

        public int ImageHeight
        {
            get { return _imageHeight; }
            set
            {
                if (_imageHeight != value)
                {
                    _imageHeight = value;
                    this.RaisePropertyChanged(() => this.ImageHeight);
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
                    ImagesIcon = Resources.ImageResource.GetEquipmentIcon(_equipmentImageState);
                    //this.RaisePropertyChanged(() => this.ImagesIcon);
                }
            }
        }

        private int _phyId;
        private string _fixInfo;
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
                str += "安装信息: "+_fixInfo;
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
                    this.RaisePropertyChanged(() => this.Tooltips);
                    var tt = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(value);
                    if(tt==null) return;
                    _fixInfo = tt.Remark;
                    _phyId = Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(value) ? tt.PhyId : value;
                    _anzhuangxinxi = new DateTime(tt.DataCreate).ToString("yyyy-MM-dd");
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
                if (!IsCanRefreshMenu) return null;
                if (_cm == null)
                {
                    _cm = new ContextMenu();
                    _cm.BorderThickness=new Thickness(0);
                }

                if (
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .
                        ContainsKey(EquipmentRtuId))
                {
                    var s =
                        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary [
                            EquipmentRtuId];
                   
                        var t =MenuBuilding.BulidCm(s.RtuModel.ToString(CultureInfo.InvariantCulture), false, s);
                        HelpCmm(t);
                    
                }

                var mi = new MenuItem();
                mi.Header = this.EquipmentName;
                mi.IsEnabled = false;
                if (_cm.Items.Count > 0)
                    _cm.Items.Insert(0, mi);
                else _cm.Items.Add(mi);
                return _cm;
            }
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
            foreach (var f in t)
            {
                if (f.Items != null && f.Items.Count > 0)
                {
                    var ggg = helpCmmm(f);
                    ggg.Header = f.Text;
                    _cm.Items.Add(ggg);
                }
                else
                {
                    MenuItem mii = new MenuItem();
                    mii.Header = f.Text;
                    mii.IsCheckable = f.IsCheckable;
                    mii.IsEnabled = f.IsEnabled;
                    mii.ToolTip = f.Tooltips;
                    mii.Command = f.Command;

                    _cm.Items.Add(mii);
                }
            }
        }

        private MenuItem helpCmmm(IIMenuItem g)
        {
            MenuItem mi = new MenuItem();
            foreach (var f in g.Items)
            {
                if (f.Items != null && f.Items.Count > 0)
                {
                    var ggg = helpCmmm(f);
                    ggg.Header = f.Text;
                    mi.Items.Add(ggg);
                }
                else
                {
                    var mii = new MenuItem();
                    mii.Header = f.Text;
                    mii.IsCheckable = f.IsCheckable;
                    mii.IsEnabled = f.IsEnabled;
                    mii.ToolTip = f.Tooltips;
                    mii.Command = f.Command;

                    mi.Items.Add(mii);
                }
            }
            return mi;
        }

        #endregion
    }
}