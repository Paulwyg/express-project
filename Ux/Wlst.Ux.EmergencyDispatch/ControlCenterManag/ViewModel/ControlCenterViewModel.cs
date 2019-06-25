using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.ProtocolCnt.Fault;
using Wlst.Sr.ProtocolCnt.GrpShowSingeInfo;
using Wlst.Sr.ProtocolCnt.OpenCloseLight;
using Wlst.Ux.EmergencyDispatch.ControlCenterManag.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag.ViewModel
{
    [Export(typeof(IIControlCenterManag))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ControlCenterViewModel : EventHandlerHelperExtendNotifyProperyChanged,IIControlCenterManag
    {
        #region IITab
        public string Title
        {
            get { return "控制中心"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }
        #endregion

        public ControlCenterViewModel()
        {
            InitEvent();
            InitAction();
     

        }

        public void NavOnLoad(params object[] parsObjects)
        {
            if (_isReloadView)
            {
                LeftWidth = 400;
                _isReloadView = false;
                NormalShow = true;
                SaveShow = false;
                NextStep = "下一步";
                BeginDate = DateTime.Now;
                EndDate = DateTime.Now.AddHours(2);
                IsEnabled = true;
                LoadLeftTreeNode();
                LoadFaultType();
                ShieldName = string.Format("{0}{1}", "控制中心", DateTime.Now.Ticks);
                _id = DateTime.Now.Ticks;
            }


        }

    }

    /// <summary>
    /// Attri
    /// </summary>
    public partial class ControlCenterViewModel
    {
        #region Field

        private long _id;
        private int _lightState;
        private bool _lightOpen=true;
        private bool _lightClose = true;
        private bool _lightOpenAgain;
        private bool _lightCloseAgain;
        #endregion
        #region Attri
        #region NormalShow

        private bool _normalShow;
        public bool NormalShow
        {
            get { return _normalShow; }
            set
            {
                if (_normalShow == value) return;
                _normalShow = value;
                RaisePropertyChanged(() => NormalShow);
            }
        }
        #endregion

        #region SaveShow
        private bool _saveShow;
        public bool SaveShow
        {
            get { return _saveShow; }
            set
            {
                if (_saveShow == value) return;
                _saveShow = value;
                RaisePropertyChanged(() => SaveShow);
            }
        }
        #endregion

        #region Remind

        private string _remind;
        public string Remind
        {
            get { return _remind; }
            set
            {
                if(_remind==value) return;
                _remind = value;
                RaisePropertyChanged(()=>Remind);
            }
        }
        #endregion

        #region ChildTreeLeftItems

        private ObservableCollection<LeftTreeNodeBase> _childtreeLeftItems;
        public ObservableCollection<LeftTreeNodeBase> ChildTreeLeftItems
        {
            get { return _childtreeLeftItems ?? (_childtreeLeftItems = new ObservableCollection<LeftTreeNodeBase>()); }
        }
        #endregion

        #region ChildTreeRightItems

        private ObservableCollection<RightTreeNodeBase> _childTreeRightItems;
        public ObservableCollection<RightTreeNodeBase>  ChildTreeRightItems
        {
            get { return _childTreeRightItems ?? (_childTreeRightItems = new ObservableCollection<RightTreeNodeBase>()); }
            set
            {
                if(_childTreeRightItems==value)return;
                _childTreeRightItems = value;
                RaisePropertyChanged(()=>ChildTreeRightItems);
            }
        }
        #endregion

        #region FaultType

        private ObservableCollection<NameIntBool> _faultType;
        public ObservableCollection<NameIntBool>  FaultType
        {
            get { return _faultType ?? (_faultType = new ObservableCollection<NameIntBool>()); }
        }
        #endregion

        #region IsEnabled

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if(_isEnabled==value) return;
                _isEnabled = value;
                RaisePropertyChanged(()=>IsEnabled);
            }
        }

        #endregion

        #region BeginDate

        private DateTime _beginDate;
        public DateTime BeginDate
        {
            get { return _beginDate;}
            set
            {
                if(_beginDate==value) return;
                _beginDate = value;
                RaisePropertyChanged(()=>BeginDate);
            }
        }
        #endregion

        #region EndDate

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (_endDate == value) return;
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
            }
        }
        #endregion

        #region  IsShowTmlFault

        private bool _isShowTmlFault;
        public bool IsShowTmlFault
        {
            get { return _isShowTmlFault; }
            set
            {
                if(_isShowTmlFault==value) return;
                _isShowTmlFault = value;
                RaisePropertyChanged(()=>IsShowTmlFault);
            }
        }
        #endregion

        #region  FaultVisi

        private Visibility  _faultVisi=Visibility.Collapsed;
        public Visibility FaultVisi
        {
            get { return _faultVisi; }
            set
            {
                if(_faultVisi==value) return;
                _faultVisi = value;
                RaisePropertyChanged(()=>FaultVisi);
            }
        }

        #endregion

        #region ResetVisi
        private Visibility _resetVisi=Visibility.Collapsed;
        public Visibility ResetVisi
        {
            get { return _resetVisi; }
            set
            {
                if (_resetVisi == value) return;
                _resetVisi = value;
                RaisePropertyChanged(() => ResetVisi);
            }
        }
        #endregion

        #region LeftWidth

        private int _leftWidth;
        public int LeftWidth
        {
            get { return _leftWidth; }
            set
            {
                if(_leftWidth==value) return;
                _leftWidth = value;
                RaisePropertyChanged(()=>LeftWidth);
            }
        }
        #endregion

        #region NextStepIsEnabled

        private bool _nextStepIsEnabled=true;
        public bool NextStepIsEnabled
        {
            get { return _nextStepIsEnabled; }
            set
            {
                if (_nextStepIsEnabled == value) return;
                _nextStepIsEnabled = value;
                RaisePropertyChanged(() => NextStepIsEnabled);
            }
        }
        #endregion

        #region ShieldName
        private string _shieldName;
        public string ShieldName
        {
            get { return _shieldName; }
            set
            {
                if (_shieldName == value) return;
                _shieldName = value;
                RaisePropertyChanged(() => ShieldName);
            }
        }
        #endregion

        #region AllSelected

        private bool _allSelected;
        public bool AllSelected
        {
            get { return _allSelected; }
            set
            {
                if(_allSelected==value) return;
                _allSelected = value;
                foreach (var item in FaultType)
                {
                    item.IsSelected = _allSelected;
                }
                RaisePropertyChanged(()=>AllSelected);
            }
        }
        #endregion

        #region IsShowFilterPanel

        private bool _isShowFilterPanel;
        public bool IsShowFilterPanel
        {
            get { return _isShowFilterPanel; }
            set
            {
                if(value==_isShowFilterPanel) return;
                _isShowFilterPanel = value;
                RaisePropertyChanged(()=>IsShowFilterPanel);
            }
        }
        #endregion

        #region FilterType
        private EnumFilterType  _filterType;
        public EnumFilterType FilterType
        {
            get
            {
                return _filterType;
            }
            set
            {
                if(_filterType==value) return;
                _filterType = value;
                RaisePropertyChanged(() => FilterType);
            }
        }
        #endregion 

        #endregion

        #region ICommand

        #region CmdReset

        private DateTime _dtReset;
        private ICommand _cmdReset;
        public ICommand CmdReset
        {
            get { return _cmdReset ?? (_cmdReset = new RelayCommand(ExReset, CanReset, true)); }
        }
        private void ExReset()
        {
            _dtReset = DateTime.Now;
            var items = RightTreeTmlNode.GetRightTreeTmlNodes();
            foreach (var item in items)
            {
                item.Value.SndWeekSet = "--";
                item.Value.SyncTime = "--";
                item.Value.AnsPatrol6 = "--";
                item.Value.AnsOpenSwitch6 = "--";
                item.Value.AnsPatrol5 = "--";
                item.Value.AnsOpenSwitch5 = "--";
                item.Value.AnsPatrol4 = "--";
                item.Value.AnsOpenSwitch4 = "--";
                item.Value.AnsPatrol3 = "--";
                item.Value.AnsOpenSwitch3 = "--";
                item.Value.AnsPatrol2 = "--";
                item.Value.AnsOpenSwitch2 = "--";
                item.Value.AnsPatrol1 = "--";
                item.Value.AnsOpenSwitch1 = "--";
                item.Value.State = "--";
                item.Value.Switch0 = false;
                item.Value.Switch1 = false;
                item.Value.Switch2 = false;
                item.Value.Switch3 = false;
                item.Value.Switch4 = false;
                item.Value.Switch5 = false;
                item.Value.Switch6 = false;

                item.Value.OpenSwitch1Visi = Visibility.Collapsed;
                item.Value.OpenSwitch2Visi = Visibility.Collapsed;
                item.Value.OpenSwitch3Visi = Visibility.Collapsed;
                item.Value.OpenSwitch4Visi = Visibility.Collapsed;
                item.Value.OpenSwitch5Visi = Visibility.Collapsed;
                item.Value.OpenSwitch6Visi = Visibility.Collapsed;
                item.Value.Patrol1Visi=Visibility.Collapsed;
                item.Value.Patrol2Visi = Visibility.Collapsed;
                item.Value.Patrol3Visi = Visibility.Collapsed;
                item.Value.Patrol4Visi = Visibility.Collapsed;
                item.Value.Patrol5Visi = Visibility.Collapsed;
                item.Value.Patrol6Visi = Visibility.Collapsed;

                item.Value.IsChecked = false;
                NextStepIsEnabled = true;

                ShieldName = string.Format("{0}{1}", "控制中心", DateTime.Now.Ticks);

                _lightClose = true;
                _lightOpen = true;
                _lightCloseAgain = false;
                _lightOpenAgain = false;

                _id = DateTime.Now.Ticks;
            }
        }
        
        private bool CanReset()
        {
            return DateTime.Now.Ticks-_dtReset.Ticks>30000000;
        }
        #endregion

        #region CmdNextStep

        private string _nextStep;
        public string NextStep
        {
            get { return _nextStep; }
            set
            {
                if(value==_nextStep) return;
                _nextStep = value;
                RaisePropertyChanged(()=>NextStep);
            }
        }

        private DateTime _dtNextStep;
        private ICommand _cmdNextStep;
        public ICommand CmdNextStep
        {
            get { return _cmdNextStep ?? (_cmdNextStep = new RelayCommand(ExNextStep, CanNextStep, true)); }
        }
        private bool CanNextStep()
        {
            return DateTime.Now.Ticks - _dtNextStep.Ticks > 30000000;
        }
        private void ExNextStep()
        {
            _dtNextStep = DateTime.Now;
            if(NextStep=="下一步")
            {
                if (!RightTreeTmlNode.GetRightTreeTmlNodes().Keys.Any())
                {
                    Remind = "右侧列表中无操作终端，请在左侧列表树中选择需要操作的终端！！！";
                    return;
                }
                FaultVisi=Visibility.Visible;
                NormalShow = false;
                SaveShow = true;
                LeftWidth = 0;
                IsShowTmlFault = false;
                NextStep = "上一步";
            }
            else
            {
                FaultVisi=Visibility.Collapsed;
                NextStep = "下一步";
                LeftWidth = 400;
                NormalShow = true;
                SaveShow = false;
            }


        }
        #endregion

        #region CmdOpenLight

        private DateTime _dtOpenLight;
        private ICommand _cmdOpenLight;
        public ICommand CmdOpenLight
        {
            get { return _cmdOpenLight ?? (_cmdOpenLight = new RelayCommand(ExOpenLight, CanOpenLight, true)); }
        }
        private void ExOpenLight()
        {
            if (_lightState == 2) //表示当前是关灯状态
            {
                if (Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show("开灯提示", "是否确定要进行开灯操作？", WlstMessageBoxType.OkCancel) == WlstMessageBoxResults.Cancel)
                {
                    return;
                }
            }
            _lightState = 1;
            _lightOpen = false;
            _lightCloseAgain = false;
            _lightOpenAgain = true;
            _lightClose = true;

            _dtOpenLight = DateTime.Now;
            var nodes = RightTreeTmlNode.GetRightTreeTmlNodes();
            foreach (var item in nodes)
            {
                item.Value.ChangeState = true;
            }
            var data = new OpenCloseOperatorCenter
                           {
                               K1Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch1
                                    select item.Value.NodeId).ToList(),
                               K2Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch2
                                    select item.Value.NodeId).ToList(),
                               K3Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch3
                                    select item.Value.NodeId).ToList(),
                               K4Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch4
                                    select item.Value.NodeId).ToList(),
                               K5Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch5
                                    select item.Value.NodeId).ToList(),
                               K6Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch6
                                    select item.Value.NodeId).ToList(),
                               Open = 1
                           };
            var info = Sr.ProtocolCnt.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
            info.Data = data;
            SndOrderServer.OrderSnd(info, 10, 6);
            foreach (var item in RightTreeTmlNode.GetRightTreeTmlNodes())
            {


                item.Value.OpenSwitch1Visi =item.Value.Switch1? Visibility.Visible:Visibility.Collapsed;
                item.Value.OpenSwitch2Visi = item.Value.Switch2 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch3Visi = item.Value.Switch3 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch4Visi = item.Value.Switch4 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch5Visi = item.Value.Switch5 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch6Visi = item.Value.Switch6 ? Visibility.Visible : Visibility.Collapsed;
            }
            Remind = "开灯命令已发出....";
        }
        private bool CanOpenLight()
        {
            return _lightOpen && DateTime.Now.Ticks-_dtOpenLight.Ticks>30000000;
        }
        #endregion

        #region CmdCloseLight

        private DateTime _dtCloseLight;
        private ICommand _cmdCloseLight;
        public ICommand CmdCloseLight
        {
            get { return _cmdCloseLight ?? (_cmdCloseLight = new RelayCommand(ExCloseLight, CanCloseLight, true)); }
        }
        private void ExCloseLight()
        {
            if(_lightState==1) //表示当前是开灯状态
            {
              if( Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show("关灯提示","是否确定要进行关灯操作？",WlstMessageBoxType.OkCancel)==WlstMessageBoxResults.Cancel)
              {
                  return;
              }
            }
            _lightState = 2;
            _lightClose = false;
            _lightOpen = true;
            _lightCloseAgain = true;
            _lightOpenAgain = false;

            _dtCloseLight = DateTime.Now;
            var nodes = RightTreeTmlNode.GetRightTreeTmlNodes();
            foreach (var item in nodes)
            {
                item.Value.ChangeState = false;
            }
            var data = new OpenCloseOperatorCenter
                           {
                               Open = 2,
                               K1Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch1
                                    select item.Value.NodeId).ToList(),
                               K2Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch2
                                    select item.Value.NodeId).ToList(),
                               K3Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch3
                                    select item.Value.NodeId).ToList(),
                               K4Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch4
                                    select item.Value.NodeId).ToList(),
                               K5Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch5
                                    select item.Value.NodeId).ToList(),
                               K6Rtus =
                                   (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                                    where item.Value.Switch6
                                    select item.Value.NodeId).ToList()
                           };
            var info = Sr.ProtocolCnt.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
            info.Data = data;
            SndOrderServer.OrderSnd(info, 10, 6);

            foreach (var item in RightTreeTmlNode.GetRightTreeTmlNodes())
            {

                item.Value.OpenSwitch1Visi = item.Value.Switch1 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch2Visi = item.Value.Switch2 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch3Visi = item.Value.Switch3 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch4Visi = item.Value.Switch4 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch5Visi = item.Value.Switch5 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch6Visi = item.Value.Switch6 ? Visibility.Visible : Visibility.Collapsed;
            }
            Remind = "关灯命令已发出....";
        }
        private bool CanCloseLight()
        {
            return _lightClose && DateTime.Now.Ticks-_dtCloseLight.Ticks>30000000;
        }
        #endregion

        #region CmdOpenLightAgain

        private DateTime _dtOpenLightAgain;
        private ICommand _cmdOpenLightAgain;
        public ICommand CmdOpenLightAgain
        {
            get { return _cmdOpenLightAgain ?? (_cmdOpenLightAgain = new RelayCommand(ExOpenLightAgain, CanOpenLightAgain, true)); }
        }
        private void ExOpenLightAgain()
        {
            if (_lightState == 2) //表示当前是关灯状态
            {
                if (Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show("开灯提示", "是否确定要进行开灯操作？", WlstMessageBoxType.OkCancel) == WlstMessageBoxResults.Cancel)
                {
                    return;
                }
            }
            _lightState = 1;
            _lightCloseAgain = false;
            _lightOpenAgain = true;
            _lightOpen = false;
            _lightClose = true;
            _dtOpenLightAgain = DateTime.Now;
            var nodes = RightTreeTmlNode.GetRightTreeTmlNodes();
            foreach (var item in nodes)
            {
                item.Value.ChangeState = true;
            }
            var data = new OpenCloseOperatorCenter
            {
                Open = 1,
                K1Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch1 == "--" && item.Value.Switch1
                     select item.Value.NodeId).ToList(),
                K2Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch2 == "--" && item.Value.Switch2
                     select item.Value.NodeId).ToList(),
                K3Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch3 == "--" && item.Value.Switch3
                     select item.Value.NodeId).ToList(),
                K4Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch4 == "--" && item.Value.Switch4
                     select item.Value.NodeId).ToList(),
                K5Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch5 == "--" && item.Value.Switch5
                     select item.Value.NodeId).ToList(),
                K6Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch6 == "--" && item.Value.Switch6
                     select item.Value.NodeId).ToList()
            };
            var info = Sr.ProtocolCnt.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
            info.Data = data;
            SndOrderServer.OrderSnd(info, 10, 6);
            foreach (var item in RightTreeTmlNode.GetRightTreeTmlNodes())
            {

                item.Value.OpenSwitch1Visi = item.Value.Switch1 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch2Visi = item.Value.Switch2 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch3Visi = item.Value.Switch3 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch4Visi = item.Value.Switch4 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch5Visi = item.Value.Switch5 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch6Visi = item.Value.Switch6 ? Visibility.Visible : Visibility.Collapsed;
            }
            Remind = "补开命令已发出....";
        }
        private bool CanOpenLightAgain()
        {
            return _lightOpenAgain && DateTime.Now.Ticks-_dtOpenLightAgain.Ticks>30000000;
        }
        #endregion

        #region CmdCloseLightAgain

        private DateTime _dtCloseLightAgain;
        private ICommand _cmdCloseLightAgain;
        public ICommand CmdCloseLightAgain
        {
            get { return _cmdCloseLightAgain ?? (_cmdCloseLightAgain = new RelayCommand(ExCloseLightAgain, CanCloseLightAgain, true)); }
        }
        private void ExCloseLightAgain()
        {
            if (_lightState == 1) //表示当前是开灯状态
            {
                if (Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show("关灯提示", "是否确定要进行关灯操作？", WlstMessageBoxType.OkCancel) == WlstMessageBoxResults.Cancel)
                {
                    return;
                }
            }
            _lightState = 2;
            _lightOpenAgain = false;
            _lightOpen = true;
            _lightCloseAgain = true;
            _dtCloseLightAgain = DateTime.Now;
            var nodes = RightTreeTmlNode.GetRightTreeTmlNodes();
            foreach (var item in nodes)
            {
                item.Value.ChangeState = false;
            }
            var data = new OpenCloseOperatorCenter
            {
                Open = 2,
                K1Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch1 == "--" && item.Value.Switch1
                     select item.Value.NodeId).ToList(),
                K2Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch2 == "--" && item.Value.Switch2
                     select item.Value.NodeId).ToList(),
                K3Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch3 == "--" && item.Value.Switch3
                     select item.Value.NodeId).ToList(),
                K4Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch4 == "--" && item.Value.Switch4
                     select item.Value.NodeId).ToList(),
                K5Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch5 == "--" && item.Value.Switch5
                     select item.Value.NodeId).ToList(),
                K6Rtus =
                    (from item in RightTreeTmlNode.GetRightTreeTmlNodes()
                     where item.Value.AnsOpenSwitch6 == "--" && item.Value.Switch6
                     select item.Value.NodeId).ToList()
            };
            var info = Sr.ProtocolCnt.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
            info.Data = data;
            SndOrderServer.OrderSnd(info, 10, 6);
            foreach (var item in RightTreeTmlNode.GetRightTreeTmlNodes())
            {

                item.Value.OpenSwitch1Visi = item.Value.Switch1 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch2Visi = item.Value.Switch2 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch3Visi = item.Value.Switch3 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch4Visi = item.Value.Switch4 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch5Visi = item.Value.Switch5 ? Visibility.Visible : Visibility.Collapsed;
                item.Value.OpenSwitch6Visi = item.Value.Switch6 ? Visibility.Visible : Visibility.Collapsed;
            }
            Remind = "补关命令已发出....";
        }
        private bool CanCloseLightAgain()
        {
            return _lightCloseAgain && DateTime.Now.Ticks-_dtCloseLightAgain.Ticks>30000000;
        }
        #endregion

        #region CmdPart

        private DateTime _dtPart;
        private ICommand _cmdPart;
        public ICommand CmdPart
        {
            get { return _cmdPart ?? (_cmdPart = new RelayCommand(ExPart, CanPart, true)); }
        }
        private void ExPart()
        {
            _dtPart = DateTime.Now;
            var info = Sr.ProtocolCnt.ServerPart.wlst_Measures_clinet_order_RtuMeasure;
            info.AddrLst.AddRange(RightTreeTmlNode.GetRightTreeTmlNodes().Keys.ToList());
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "巡测命令已经发送...";
            foreach (var item in RightTreeTmlNode.GetRightTreeTmlNodes())
            {

                item.Value.Patrol1Visi = Visibility.Visible;
                item.Value.Patrol2Visi = Visibility.Visible;
                item.Value.Patrol3Visi = Visibility.Visible;
                item.Value.Patrol4Visi = Visibility.Visible;
                item.Value.Patrol5Visi = Visibility.Visible;
                item.Value.Patrol6Visi = Visibility.Visible;

                item.Value.AnsPatrol1 = "--";
                item.Value.AnsPatrol2 = "--";
                item.Value.AnsPatrol3 = "--";
                item.Value.AnsPatrol4 = "--";
                item.Value.AnsPatrol5 = "--";
                item.Value.AnsPatrol6 = "--";
            }
        }
        private bool CanPart()
        {
            return DateTime.Now.Ticks-_dtPart.Ticks>30000000;
        }
        #endregion

        #region CmdAsynTime

        private DateTime _dtAsynTime;
        private ICommand _cmdAsynTime;
        public ICommand CmdAsynTime
        {
            get { return _cmdAsynTime ?? (_cmdAsynTime = new RelayCommand(ExAsynTime, CanAsynTime, true)); }
        }
        private void ExAsynTime()
        {
            _dtAsynTime = DateTime.Now;
            foreach (var item in RightTreeTmlNode.GetRightTreeTmlNodes())
            {
                item.Value.SyncTime = "--";
            }
            var lstRtu= RightTreeTmlNode.GetRightTreeTmlNodes().Select(node => node.Value.NodeId).ToList();
            var info = Sr.ProtocolCnt.ServerPart.wlst_asyntime_clinet_order_asynrtutime;
            info.AddrLst.AddRange(lstRtu);
            info.Data = DateTime.Now;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "对时命令已发出！！！";
        }
        private bool CanAsynTime()
        {
            return DateTime.Now.Ticks-_dtAsynTime.Ticks>30000000;
        }
        #endregion

        #region CmdSndWeekSet

        private DateTime _dtWeekSet;
        private ICommand _cmdSndWeekSet;
        public ICommand CmdSndWeekSet
        {
            get { return _cmdSndWeekSet ?? (_cmdSndWeekSet = new RelayCommand(ExSndWeekSet, CanSndWeekSet, true)); }
        }
        private void ExSndWeekSet()
        {
            _dtWeekSet = DateTime.Now;
            foreach (var item in RightTreeTmlNode.GetRightTreeTmlNodes())
            {
                item.Value.SndWeekSet = "--";
            }
            var lstRtu = RightTreeTmlNode.GetRightTreeTmlNodes().Select(node => node.Value.NodeId).ToList();
            var info = Sr.ProtocolCnt.ServerPart.wlst_asyntime_clinet_order_sendweekset;
            info.AddrLst.AddRange(lstRtu);
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "发送周设置命令已发出...";
        }
        private bool CanSndWeekSet()
        {
            return DateTime.Now.Ticks- _dtWeekSet.Ticks>30000000;
        }
        #endregion

        #region AddTml

        private ICommand _addTml;
        public ICommand AddTml
        {
            get { return _addTml ?? (_addTml = new RelayCommand(ExAddTml, CanAddTml, true)); }
        }
        private void ExAddTml()
        {
            foreach (var item in ChildTreeLeftItems)
            {
                GetAllCheckedTreeNode(item);
            }
            LoadRightTreeNode();

        }
        private bool CanAddTml()
        {
            return true;
        }
        #endregion

        #region SubTml

        private ICommand _subTml;
        public ICommand SubTml
        {
            get { return _subTml ?? (_subTml = new RelayCommand(ExSubTml, CanSubTml, true)); }
        }
        private void ExSubTml()
        {
            _rightTreeDeleteTml.Clear();
            //删除树勾选点并得出要删除的终端信息，用于更新左侧树
            for (var i = 0; i < ChildTreeRightItems.Count; i++)
            {
                var item = ChildTreeRightItems[i];
                GetAllNeedDeleteNode(item);
                if (item.ChildTreeItems.Count != 0) continue;
                ChildTreeRightItems.Remove(item);
                i--;
            }
            //更新CheckGroupInfo数据
            UpdateCheckGroupInfo();
            if(_rightTreeDeleteTml.Count>0)
            {
                BackToTheNormalState(_rightTreeDeleteTml);   //将不需要加入的勾选去除 
            }
            foreach (var id in _rightTreeDeleteTml)
            {
                if(!RightTreeTmlNode.GetRightTreeTmlNodes().Keys.Contains(id))continue;
                RightTreeTmlNode.DeleteTmlNode(id);
            }
        }
        private bool CanSubTml()
        {
            return true;
        }
        #endregion

        #region CmdSaveShieldFault

        private DateTime _dtSaveShieldFault;
        private ICommand _cmdSaveShieldFault;
        public ICommand CmdSaveShieldFault
        {
            get
            {
                return _cmdSaveShieldFault ??
                       (_cmdSaveShieldFault = new RelayCommand(ExSaveShieldFault, CanSaveShieldFault, true));
            }
        }
        private void ExSaveShieldFault()
        {
            _dtSaveShieldFault = DateTime.Now;
            NextStepIsEnabled = false;
            SetShieldFaultAlarm();
        }
        private bool CanSaveShieldFault()
        {
            return DateTime.Now.Ticks - _dtSaveShieldFault.Ticks > 30000000;
        }
        #endregion



        #endregion

        #region Field
        /// <summary>
        /// 保存右侧树组的信息用来生成右侧树
        /// </summary>
        private static readonly Dictionary<int, GroupInformation> CheckGroupInfo = new Dictionary<int, GroupInformation>();
        public static Dictionary<int,GroupInformation> GetChickGroupInfo()
        {
            return CheckGroupInfo;
        }
        /// <summary>
        /// 保存右侧树中需要删除的终端节点，在右侧树删除后，将左侧树中对应的勾选去除
        /// </summary>
        private readonly List<int> _rightTreeDeleteTml=new List<int>();

        private bool _isReloadView = true;

        #endregion
    }
    /// <summary>
    /// Methods
    /// </summary>
    public partial class ControlCenterViewModel
    {
        private void LoadFaultType()
        {
            //加载故障列表
            FaultType.Clear();
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                if (!t.Value.IsEnable) continue;
                FaultType.Add(!string.IsNullOrEmpty(t.Value.FaultNameByDefine)
                                  ? new NameIntBool { Name = t.Value.FaultNameByDefine, Value = t.Value.FaultId, IsSelected = false }
                                  : new NameIntBool { Name = t.Value.FaultName, Value = t.Value.FaultId, IsSelected = false });
            }
        }
        private void SetShieldFaultAlarm()
        {
            if (!(from type in FaultType where type.IsSelected select type.Value).Any())
            {
                if (Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show("提示", "故障屏蔽信息未设置，是否设置？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.Yes)
                {
                    return;
                }
            }
            ResetVisi=Visibility.Visible;
            var info = Sr.ProtocolCnt.ServerPart.wlst_EquipemntLightFault_clinet_add_OneRtuFaultShield;
            var item = new FaultShieldItemInfo
            {
                Id = _id,
                Name = ShieldName,
                RtusShield=RightTreeTmlNode.GetRightTreeTmlNodes().Keys.ToList(),
                FaultsShield=(from tt in FaultType where tt.IsSelected select  tt.Value).ToList(),
                TimeStart=BeginDate.Ticks,
                TimeEnd =EndDate.Ticks,

            };

            var data = new ExchangedRtuFaultShield();
            data.Info.Add(item);
            info.Data = data;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "故障屏蔽信息已经发送...";
        }

        //初始化时加载左侧树终端节点
        private void LoadLeftTreeNode()
        {
            ChildTreeLeftItems.Clear();

            if (Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(0))
                foreach (var t in Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[0].LstGrp)
                {
                    if (
                        !Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(t))
                        continue;
                    ChildTreeLeftItems.Add(new LeftTreeGroupNode(null,t));
                }

            var lst = LeftTreeTmlNode.GetRegisterTmlNode().Keys.ToList();
            var lstAllSpecial = (from t in ServicesEquipemntInfoHold.EquipmentInfoDictionary let equipmentInfo = t.Value as IIRtuParaWork where equipmentInfo != null where !lst.Contains(t.Key) select t.Key).ToList();

            if (lstAllSpecial.Count <= 0) return;
            var f = new LeftTreeGroupNode {NodeId =-1,NodeName = "未分组"};
            foreach (var t in lstAllSpecial)
            {
                f.ChildTreeItems.Add(new LeftTreeTmlNode(f,t));
            }
            ChildTreeLeftItems.Add(f);
        }

        //将在右侧树中删除的终端在左侧树中去除勾选
        #region
        private void BackToTheNormalState(List<int> tmls )
        {
            foreach (var item in ChildTreeLeftItems)
            {
                ChangedTheState(item,tmls);
            }
        }
        private void ChangedTheState(LeftTreeNodeBase tree,List<int> tmls )
        {
            foreach (var child in tree.ChildTreeItems)
            {
                if(child.IsGroup)
                {
                    ChangedTheState(child,tmls);
                }
                else
                {
                    if(child.IsChecked&&tmls.Contains(child.NodeId))
                    {
                        child.IsChecked = false;
                    }
                }
            }
        }
        #endregion

        //获取左侧树中勾选的树信息
        private bool GetAllCheckedTreeNode(LeftTreeNodeBase node)
        {
            bool res = false;
            if (node.IsGroup)
            {
                var grpInfo = CheckGroupInfo.ContainsKey(node.NodeId) ? CheckGroupInfo[node.NodeId] : new GroupInformation();
               
                foreach (var item in node.ChildTreeItems)
                {
                    if (item.IsGroup)
                    {
                        if (GetAllCheckedTreeNode(item))
                        {
                            if(!grpInfo.LstGrp.Contains(item.NodeId))
                            {
                                grpInfo.LstGrp.Add(item.NodeId);
                            }
                            res = true;
                        }
                    }
                    else
                    {
                        if (item.IsChecked)
                        {
                            if(!grpInfo.LstTml.Contains(item.NodeId))
                            {
                                grpInfo.LstTml.Add(item.NodeId);
                            }
                            res = true;
                        }
                    }
                }
                if (res)
                {
                    if (!CheckGroupInfo.ContainsKey(node.NodeId))
                    CheckGroupInfo.Add(node.NodeId, grpInfo);
                }

            }
            return res;
        }

        #region Create RightChildTree
        private void LoadRightTreeNode()
        {
            ChildTreeRightItems.Clear();

            if (Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(0))
                foreach (var t in Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[0].LstGrp)
                {
                    if (
                        !Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(t))
                        continue;
                    if (CheckGroupInfo.ContainsKey(t))
                    {

                        ChildTreeRightItems.Add(new RightTreeGroupNode(null, t));
                    }

                }

            if (CheckGroupInfo.ContainsKey(-1))
            {
                var f = new RightTreeGroupNode{ NodeId = -1, NodeName = "未分组终端" };
                foreach (var information in CheckGroupInfo[-1].LstTml)
                {
                    f.ChildTreeItems.Add(new RightTreeTmlNode(f, information));
                }
                ChildTreeRightItems.Add(f);
            }

        }
        #endregion

        #region 获取右侧树分组信息
        private void UpdateCheckGroupInfo()
        {
            CheckGroupInfo.Clear();
            foreach (var item in ChildTreeRightItems)
            {
                GetRightTreeNodeInfo(item);
            }
        }
        private void GetRightTreeNodeInfo(RightTreeNodeBase node)
        {
            if (node.IsGroup)
            {
                var grpInfo = CheckGroupInfo.ContainsKey(node.NodeId) ? CheckGroupInfo[node.NodeId] : new GroupInformation();
                
                foreach (var item in node.ChildTreeItems)
                {
                    if (item.IsGroup)
                    {
                        GetRightTreeNodeInfo(item);
                        if(!grpInfo.LstGrp.Contains(item.NodeId))
                        {
                            grpInfo.LstGrp.Add(item.NodeId);
                        }
                       
                    }
                    else
                    {
                        if(!grpInfo.LstTml.Contains(item.NodeId))
                        {
                            grpInfo.LstTml.Add(item.NodeId);
                        }
                    }
                }
                if (!CheckGroupInfo.ContainsKey(node.NodeId))
                {
                    CheckGroupInfo.Add(node.NodeId, grpInfo);
                }
            }
        }
        #endregion

        //右侧树删除，并获取需要删除的终端信息
        private bool GetAllNeedDeleteNode(RightTreeNodeBase node)
        {
            bool res = false;
            if (node.IsGroup)
            {
                for (int i = 0; i < node.ChildTreeItems.Count; i++)
                {
                    var item = node.ChildTreeItems[i];
                    if (item.IsGroup)
                    {
                        if (GetAllNeedDeleteNode(item))
                        {
                            if (item.ChildTreeItems.Count == 0)
                            {
                                item.Father.ChildTreeItems.Remove(item);
                                i--;
                            }

                            res = true;
                        }

                    }
                    else
                    {
                        if (item.IsChecked)
                        {
                            _rightTreeDeleteTml.Add(item.NodeId);
                            item.Father.ChildTreeItems.Remove(item);
                            i--;
                            res = true;
                        }
                    }
                }

            }
            return res;
        }

    }

    public partial class ControlCenterViewModel
    {
         private void InitEvent()
         {
             AddEventFilterInfo(Cr.CoreOne.CoreIdAssign.EventIdAssign.AsyncTimeEventId,PublishEventType.Core);
             AddEventFilterInfo(Cr.CoreOne.CoreIdAssign.EventIdAssign.OpenOrCloseLightReceiveEventId, PublishEventType.Core);
             AddEventFilterInfo(EventIdAssign.EquipmentNewDataArrive,PublishEventType.Core);
             AddEventFilterInfo(EmergencyDispatch.Services.EventIdAssign.AddOneFaultShield,PublishEventType.Core);
         }
         public override void ExPublishedEvent(PublishEventArgs args)
         {
             #region 时间同步
             if (args.EventId == Cr.CoreOne.CoreIdAssign.EventIdAssign.AsyncTimeEventId)  //事件在OpenCloseLightDataDispatch文件中监听，后发布该事件
             {
                 var lst = args.GetParams()[0] as List<int>;
                 if(lst==null)return;

                 foreach (var key in RightTreeTmlNode.GetRightTreeTmlNodes().Keys.ToList())
                 {
                     if (lst.Contains(key))
                     {
                         RightTreeTmlNode.GetRightTreeTmlNodes()[key].SyncTime = "成功";
                     }
                 }

                 Remind = "时钟同步数据已返回！！！";
             }
             #endregion

             #region 开关灯
             else if (args.EventId == Cr.CoreOne.CoreIdAssign.EventIdAssign.OpenOrCloseLightReceiveEventId)
             {
                 var rtuid = Convert.ToInt32(args.GetParams()[0]);
                 var loopid = Convert.ToInt32(args.GetParams()[1]);
                 var isOpen = Convert.ToBoolean(args.GetParams()[2]);
                 if(!RightTreeTmlNode.GetRightTreeTmlNodes().Keys.ToList().Contains(rtuid)) return;
                 bool state = RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].ChangeState;
                 switch (loopid)
                 {
                     case 1:
                         if (isOpen && state ||  !isOpen && !state)
                         {
                             RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsOpenSwitch1 = "应答";
                         }
                         break;
                     case 2:
                         if (isOpen && state || !isOpen && !state)
                         {
                             RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsOpenSwitch2 = "应答";
                         }
                         break;
                     case 3:
                         if (isOpen && state || !isOpen && !state)
                         {
                             RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsOpenSwitch3 = "应答";
                         }
                         break;
                     case 4:
                         if (isOpen && state || !isOpen && !state)
                         {
                             RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsOpenSwitch4 = "应答";
                         }
                         break;
                     case 5:
                         if (isOpen && state || !isOpen && !state)
                         {
                             RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsOpenSwitch5 = "应答";
                         }
                         break;
                     case 6:
                         if (isOpen && state || !isOpen && !state)
                         {
                             RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsOpenSwitch6 = "应答";
                         }
                         break;
                 }
             }
             #endregion

             #region 关灯
             //else if(Cr.CoreOne.CoreIdAssign.EventIdAssign.CloseLightReceiveEventId==args.EventId)
             //{
             //    var rtuid = Convert.ToInt32(args.GetParams()[0]);
             //    var loopid = Convert.ToInt32(args.GetParams()[1]);
             //    if (!RightTreeTmlNode.GetRightTreeTmlNodes().Keys.ToList().Contains(rtuid)) return;

             //    switch (loopid)
             //    {
             //        case 1:
             //            RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsCloseSwitch1 = "答";
             //            break;
             //        case 2:
             //            RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsCloseSwitch2 = "答";
             //            break;
             //        case 3:
             //            RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsCloseSwitch3 = "答";
             //            break;
             //        case 4:
             //            RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsCloseSwitch4 = "答";
             //            break;
             //        case 5:
             //            RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsCloseSwitch5 = "答";
             //            break;
             //        case 6:
             //            RightTreeTmlNode.GetRightTreeTmlNodes()[rtuid].AnsCloseSwitch6 = "答";
             //            break;
             //        default:
             //            break;
             //    }
             //}
             #endregion

             #region 巡测
             else if(EventIdAssign.EquipmentNewDataArrive==args.EventId)
             {
                 var lst = args.GetParams()[0] as List<int>;
                 if(lst==null ||lst.Count<1) return;
                 foreach (var item in RightTreeTmlNode.GetRightTreeTmlNodes())
                 {
                     if(lst.Contains(item.Key))
                     {
                         var tmp = RtuNewDataService.GetInfoById(item.Key);
                         for (int i = 0; i < tmp.IsSwitchOutAttraction.Count; i++)
                         {
                             switch (i+1)
                             {
                                 case 1: item.Value.AnsPatrol1 = tmp.IsSwitchOutAttraction[0] ? "开" : "关";
                                     item.Value.K1Foreground = tmp.IsSwitchOutAttraction[0]
                                                                   ? Colors.Green.ToString()
                                                                   : Colors.Red.ToString();
                                     break;
                                 case 2: item.Value.AnsPatrol2 = tmp.IsSwitchOutAttraction[1] ? "开" : "关";
                                     item.Value.K2Foreground = tmp.IsSwitchOutAttraction[1]
                                                        ? Colors.Green.ToString()
                                                        : Colors.Red.ToString();
                                     break;
                                 case 3: item.Value.AnsPatrol3 = tmp.IsSwitchOutAttraction[2] ? "开" : "关";
                                     item.Value.K3Foreground = tmp.IsSwitchOutAttraction[2]
                                                        ? Colors.Green.ToString()
                                                        : Colors.Red.ToString();
                                     break;
                                 case 4: item.Value.AnsPatrol4 = tmp.IsSwitchOutAttraction[3] ? "开" : "关";
                                     item.Value.K4Foreground = tmp.IsSwitchOutAttraction[3]
                                                        ? Colors.Green.ToString()
                                                        : Colors.Red.ToString();
                                     break;
                                 case 5: item.Value.AnsPatrol5 = tmp.IsSwitchOutAttraction[4] ? "开" : "关";
                                     item.Value.K5Foreground = tmp.IsSwitchOutAttraction[4]
                                                        ? Colors.Green.ToString()
                                                        : Colors.Red.ToString();
                                     break;
                                 case 6: item.Value.AnsPatrol6 = tmp.IsSwitchOutAttraction[5] ? "开" : "关";
                                     item.Value.K6Foreground = tmp.IsSwitchOutAttraction[5]
                                                        ? Colors.Green.ToString()
                                                        : Colors.Red.ToString();
                                     break;
                                 default:
                                     Remind = "开关数不在1-6之间";
                                     break;
                             }
                         }
                     }
                 }
                 
             }
             #endregion

             #region 保存屏蔽终端故障信息
             else if(args.EventId==EmergencyDispatch.Services.EventIdAssign.AddOneFaultShield)
             {
                 var myId = (long)args.GetParams()[0];
                 if(myId==_id)
                 {
                     Remind = "屏蔽终端故障信息发送成功！！！";
                 }

             }
             #endregion
         }
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_asyntime_server_ans_clinet_order_sendweeksetk1k3,
             ResponseSndWeekSetK1K3,typeof(ControlCenterViewModel), this);
            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_asyntime_server_ans_clinet_order_sendweeksetk4k6,
             ResponseSndWeekSetK4K6,typeof(ControlCenterViewModel), this);
        }
        private void ResponseSndWeekSetK1K3(string session,ProtocolEncodingCnt<int> infos)
        {
            var lst = infos.AddrLst;
            if(lst==null) return;
            foreach (var nodeid in RightTreeTmlNode.GetRightTreeTmlNodes().Keys.ToList())
            {
                if(lst.Contains(nodeid))
                {
                    RightTreeTmlNode.GetRightTreeTmlNodes()[nodeid].SndWeekSet =
                        RightTreeTmlNode.GetRightTreeTmlNodes()[nodeid].SndWeekSet == "K4K6成功" ? "全部成功" : "K1K3成功";
                }
            }
            Remind = "发送周设置数据已返回！！！";
        }
        private void ResponseSndWeekSetK4K6(string session, ProtocolEncodingCnt<int> infos)
        {
            var lst = infos.AddrLst;
            if (lst == null) return;
            foreach (var nodeid in RightTreeTmlNode.GetRightTreeTmlNodes().Keys.ToList())
            {
                if (lst.Contains(nodeid))
                {
                    RightTreeTmlNode.GetRightTreeTmlNodes()[nodeid].SndWeekSet =
                        RightTreeTmlNode.GetRightTreeTmlNodes()[nodeid].SndWeekSet == "K1K3成功" ? "全部成功" : "K4K6成功";
                }
            }
            Remind = "发送周设置数据已返回！！！";
        }

    }

    public partial class ControlCenterViewModel
    {

        #region  FilterContent

        private string _filterContent="数据过滤";
        public string FilterContent
        {
            get { return _filterContent; }
            set
            {
                if(_filterContent==value) return;
                _filterContent = value;
                RaisePropertyChanged(()=>FilterContent);
            }
        }
        #endregion

        #region IsShowAllDate

        private bool _isShowAllDate=true;
        public bool IsShowAllDate
        {
            get { return _isShowAllDate; }
            set
            {
                if(value==_isShowAllDate)return;
                _isShowAllDate = value;
                RaisePropertyChanged(()=>IsShowAllDate);
            }
        }
        #endregion

        #region CmdFilter

        private ObservableCollection<RightTreeNodeBase> ChildTreeRightItemsCotpy;  //指向原始数据，保存对原ChildTreeRightItems的引用
        private DateTime _dtFilter;
        private ICommand _cmdFilter;
        public ICommand CmdFilter
        {
            get { return _cmdFilter ?? (_cmdFilter = new RelayCommand(ExFilter, CanFilter, true)); }
        }
        private bool CanFilter()
        {
            return DateTime.Now.Ticks - _dtFilter.Ticks > 30000000;
        }
        private void ExFilter()
        {
            _dtFilter = DateTime.Now;
            if (FilterContent.Equals("数据过滤"))
            {
                FilterContent = "全部数据";
                IsShowAllDate = false;
                ChildTreeRightItemsCotpy = ChildTreeRightItems;
                ChildTreeRightItems = new ObservableCollection<RightTreeNodeBase>();
                foreach (var nodeBase in ChildTreeRightItemsCotpy)
                {
                    var tt = new RightTreeNodeBase();
                    CopyRightChildTree(tt, nodeBase);
                    ChildTreeRightItems.Add(tt);
                }
                switch (FilterType)
                {
                    case EnumFilterType.OpenCloseData:
                        RemoveReceivedLightOpenCloseDataRightChild();
                        break;
                    case EnumFilterType.PartData:
                        RemoveReceivedPartDataRightChild();
                        break;
                    case EnumFilterType.AsynTimeData:
                        RemoveReceivedAsynTimeDataRightChild();
                        break;
                    case EnumFilterType.SndWeekSetData:
                        RemoveReceivedSndWeekSetDataRightChild();
                        break;
                    default:
                        ChildTreeRightItems = ChildTreeRightItemsCotpy;
                         System.GC.Collect(); //回收内存
                        break;
                }
                
            }
            else
            {
                FilterContent = "数据过滤";
                IsShowAllDate = true;
                ChildTreeRightItems = ChildTreeRightItemsCotpy;
                System.GC.Collect(); //回收内存
            }

        }
        private void CopyRightChildTree(RightTreeNodeBase copynode, RightTreeNodeBase node)  //将node中的信息复制到copynode中
        {
            copynode.AnsCloseSwitch1 = node.AnsCloseSwitch1;
            copynode.AnsCloseSwitch2 = node.AnsCloseSwitch2;
            copynode.AnsCloseSwitch3 = node.AnsCloseSwitch3;
            copynode.AnsCloseSwitch4 = node.AnsCloseSwitch4;
            copynode.AnsCloseSwitch5 = node.AnsCloseSwitch5;
            copynode.AnsCloseSwitch6 = node.AnsCloseSwitch6;
            copynode.AnsOpenSwitch1 = node.AnsOpenSwitch1;
            copynode.AnsOpenSwitch2 = node.AnsOpenSwitch2;
            copynode.AnsOpenSwitch3 = node.AnsOpenSwitch3;
            copynode.AnsOpenSwitch4 = node.AnsOpenSwitch4;
            copynode.AnsOpenSwitch5 = node.AnsOpenSwitch5;
            copynode.AnsOpenSwitch6 = node.AnsOpenSwitch6;
            copynode.AnsPatrol1 = node.AnsPatrol1;
            copynode.AnsPatrol2 = node.AnsPatrol2;
            copynode.AnsPatrol3 = node.AnsPatrol3;
            copynode.AnsPatrol4 = node.AnsPatrol4;
            copynode.AnsPatrol5 = node.AnsPatrol5;
            copynode.AnsPatrol6 = node.AnsPatrol6;
            // copynode.ChangeState = node.ChangeState;
            copynode.CloseSwitch1Visi = node.CloseSwitch1Visi;
            copynode.CloseSwitch2Visi = node.CloseSwitch2Visi;
            copynode.CloseSwitch3Visi = node.CloseSwitch3Visi;
            copynode.CloseSwitch4Visi = node.CloseSwitch4Visi;
            copynode.CloseSwitch5Visi = node.CloseSwitch5Visi;
            copynode.CloseSwitch6Visi = node.CloseSwitch6Visi;
            copynode.IsChecked = node.IsChecked;
            copynode.IsGroup = node.IsGroup;
            copynode.IsSelected = node.IsChecked;
            copynode.K1Foreground = node.K1Foreground;
            copynode.K2Foreground = node.K2Foreground;
            copynode.K3Foreground = node.K3Foreground;
            copynode.K4Foreground = node.K4Foreground;
            copynode.K5Foreground = node.K5Foreground;
            copynode.K6Foreground = node.K6Foreground;
            copynode.NodeId = node.NodeId;
            copynode.NodeName = node.NodeName;
            copynode.OpenSwitch1Visi = node.OpenSwitch1Visi;
            copynode.OpenSwitch2Visi = node.OpenSwitch2Visi;
            copynode.OpenSwitch3Visi = node.OpenSwitch3Visi;
            copynode.OpenSwitch4Visi = node.OpenSwitch4Visi;
            copynode.OpenSwitch5Visi = node.OpenSwitch5Visi;
            copynode.OpenSwitch6Visi = node.OpenSwitch6Visi;
            copynode.Patrol1Visi = node.Patrol1Visi;
            copynode.Patrol2Visi = node.Patrol2Visi;
            copynode.Patrol3Visi = node.Patrol3Visi;
            copynode.Patrol4Visi = node.Patrol4Visi;
            copynode.Patrol5Visi = node.Patrol5Visi;
            copynode.Patrol6Visi = node.Patrol6Visi;
            copynode.SndWeekSet = node.SndWeekSet;
            copynode.State = node.State;
            copynode.Switch0 = node.Switch0;
            copynode.Switch1 = node.Switch1;
            copynode.Switch2 = node.Switch2;
            copynode.Switch3 = node.Switch3;
            copynode.Switch4 = node.Switch4;
            copynode.Switch5 = node.Switch5;
            copynode.Switch6 = node.Switch6;
            copynode.SyncTime = node.SyncTime;

            if (node.ChildTreeItems.Count > 0)
            {
                foreach (var childnode in node.ChildTreeItems)
                {
                    var t = new RightTreeNodeBase();
                    copynode.ChildTreeItems.Add(t);
                    t.Father = copynode;
                    CopyRightChildTree(t, childnode);

                }
            }

        }

        private void RemoveReceivedLightOpenCloseDataRightChild()
        {
            for (var i = 0; i < ChildTreeRightItems.Count; i++)
            {
                RemoveLightOpenCloseDataRightChild(ChildTreeRightItems[i]);
                if (ChildTreeRightItems[i].ChildTreeItems.Count == 0)
                {
                    ChildTreeRightItems.RemoveAt(i);
                }
            }
        }
        private bool RemoveLightOpenCloseDataRightChild(RightTreeNodeBase node)
        {
            try
            {
                if (!node.IsGroup && node.Father != null)
                {
                    var condition = true;
                    if(node.Switch1)
                    {
                        condition =  (!node.AnsOpenSwitch1.Equals("--"));
                    }
                    if(node.Switch2)
                    {
                        condition = condition && (!node.AnsOpenSwitch2.Equals("--"));
                    }
                    if (node.Switch3)
                    {
                        condition = condition && (!node.AnsOpenSwitch3.Equals("--"));
                    }
                    if (node.Switch4)
                    {
                        condition = condition && (!node.AnsOpenSwitch4.Equals("--"));
                    }
                    if (node.Switch5)
                    {
                        condition = condition && (!node.AnsOpenSwitch5.Equals("--"));
                    }
                    if (node.Switch6)
                    {
                        condition = condition && (!node.AnsOpenSwitch6.Equals("--"));
                    }
                    if (condition)
                    {
                        node.Father.ChildTreeItems.Remove(node);
                        return true;
                    }

                }
                else
                {
                    for (var i = 0; i < node.ChildTreeItems.Count; i++)
                    {
                        if (RemoveLightOpenCloseDataRightChild(node.ChildTreeItems[i]))
                        {
                            i--;
                        }
                    }
                    if (node.ChildTreeItems.Count == 0 && node.Father != null)
                    {
                        node.Father.ChildTreeItems.Remove(node);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            return false;
        }

        private void RemoveReceivedPartDataRightChild()
        {
            for (var i = 0; i < ChildTreeRightItems.Count; i++)
            {
                RemovePartDataRightChild(ChildTreeRightItems[i]);
                if (ChildTreeRightItems[i].ChildTreeItems.Count == 0)
                {
                    ChildTreeRightItems.RemoveAt(i);
                }
            }
        }
        private bool RemovePartDataRightChild(RightTreeNodeBase node)
        {
            try
            {
                if (!node.IsGroup && node.Father != null)
                {
                    if ((!node.AnsPatrol1.Equals("--")) && (!node.AnsPatrol2.Equals("--")) && (!node.AnsPatrol3.Equals("--")) && (!node.AnsPatrol4.Equals("--")) && (!node.AnsPatrol5.Equals("--")) && (!node.AnsPatrol6.Equals("--")))
                    {
                        node.Father.ChildTreeItems.Remove(node);
                        return true;
                    }

                }
                else
                {
                    for (var i = 0; i < node.ChildTreeItems.Count; i++)
                    {
                        if (RemovePartDataRightChild(node.ChildTreeItems[i]))
                        {
                            i--;
                        }
                    }
                    if (node.ChildTreeItems.Count == 0 && node.Father != null)
                    {
                        node.Father.ChildTreeItems.Remove(node);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            return false;
        }

        private void RemoveReceivedAsynTimeDataRightChild()
        {
            for (var i = 0; i < ChildTreeRightItems.Count; i++)
            {
                RemoveAsynTimeDataRightChild(ChildTreeRightItems[i]);
                if (ChildTreeRightItems[i].ChildTreeItems.Count == 0)
                {
                    ChildTreeRightItems.RemoveAt(i);
                }
            }
        }
        private bool RemoveAsynTimeDataRightChild(RightTreeNodeBase node)
        {
            try
            {
                if (!node.IsGroup && node.Father != null)
                {
                    if (!node.SyncTime.Equals("--"))
                    {
                        node.Father.ChildTreeItems.Remove(node);
                        return true;
                    }

                }
                else
                {
                    for (var i = 0; i < node.ChildTreeItems.Count; i++)
                    {
                        if (RemoveAsynTimeDataRightChild(node.ChildTreeItems[i]))
                        {
                            i--;
                        }
                    }
                    if (node.ChildTreeItems.Count == 0 && node.Father != null)
                    {
                        node.Father.ChildTreeItems.Remove(node);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            return false;
        }

        private void RemoveReceivedSndWeekSetDataRightChild()
        {
            for (var i = 0; i < ChildTreeRightItems.Count; i++)
            {
                RemoveSndWeekSetDataRightChild(ChildTreeRightItems[i]);
                if (ChildTreeRightItems[i].ChildTreeItems.Count == 0)
                {
                    ChildTreeRightItems.RemoveAt(i);
                }
            }
        }
        private bool RemoveSndWeekSetDataRightChild(RightTreeNodeBase node)
        {
            try
            {
                if (!node.IsGroup && node.Father != null)
                {
                    if (node.SndWeekSet.Equals("全部成功"))
                    {
                        node.Father.ChildTreeItems.Remove(node);
                        return true;
                    }

                }
                else
                {
                    for (var i = 0; i < node.ChildTreeItems.Count; i++)
                    {
                        if (RemoveSndWeekSetDataRightChild(node.ChildTreeItems[i]))
                        {
                            i--;
                        }
                    }
                    if (node.ChildTreeItems.Count == 0 && node.Father != null)
                    {
                        node.Father.ChildTreeItems.Remove(node);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            return false;
        }

        #endregion
    }

    public enum EnumFilterType
    {
        OpenCloseData,
        PartData,
        AsynTimeData,
        SndWeekSetData
    }
}
