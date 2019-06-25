using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Services;
using Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Views;
using Wlst.client;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.ViewModel
{
    [Export(typeof(IIControlCenterManagDemo2))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ControlCenterViewModel2 : EventHandlerHelperExtendNotifyProperyChanged, IIControlCenterManagDemo2
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

        #region radiobuttion & visual

        private int rdIndex;

        /// <summary>
        /// 1、全局，2、本地、3、临时
        /// </summary>
        public int RdIndex
        {
            get { return rdIndex; }
            set
            {
                if (value == rdIndex) return;
                rdIndex = value;
                this.RaisePropertyChanged(() => this.RdIndex);

                if (value == 1) LoadTreeNodeGlobal();
                else if (value == 2) LoadTreeNodeLocal();
            }
        }

        private bool  rdisEnable;
        /// <summary>
        /// 全局 本地是否可用  
        /// </summary>
        public bool RdGbIsEnable
        {
            get { return rdisEnable; }
            set
            {
                if (value == rdisEnable) return;
                rdisEnable = value;
                this.RaisePropertyChanged(() => this.RdGbIsEnable);
            }
        }

        #endregion
        public ControlCenterViewModel2()
        {
            InitEvent();
            InitAction();
            Wlst.Cr.Coreb.AsyncTask.Qtz.AddQtz("s",0, DateTime.Now.Ticks, 1, Qtz1);
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            if (_isViewShow) return;
            _isViewShow = true;

            _id = DateTime.Now.Ticks;
            _currentSelectAllState = false;

            if (parsObjects.Count() == 0)
            {
                RdGbIsEnable = true;
                RdIndex = 1;
                //主菜单

                //  LoadTreeNodeGlobal();



            }
            else
            {
                RdGbIsEnable = false;
                RdIndex = 3;

                var rtus = parsObjects[0] as List<int>;
                if (rtus == null || rtus.Count == 0)
                {
                    RdGbIsEnable = true;
                    RdIndex = 1;
                }
                else
                {
                    LoadTreeNodeTemp(rtus);
                }
            }


        }



        public void OnUserHideOrClosing()
        {
            _isViewShow = false;
            Items.Clear();

            //throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Attri
    /// </summary>
    public partial class ControlCenterViewModel2
    {
        #region Field

        private bool _isViewShow;
        private int _isOnOpenLight; //2 表示当前正在进行关灯，1 表示当前正在进行开灯 ,0表示初始状态
        private long _id;
        #endregion

        #region Attri

        #region SelectReportType

        private bool _selectReportType;
        public bool SelectReportType
        {
            get { return _selectReportType; }
            set
            {
                if(_selectReportType==value) return;
                _selectReportType = value;
                RaisePropertyChanged(()=>SelectReportType);
            }
        }
        #endregion

        #region ReportType //报表类型

        private EnumReportTypes _reportTypes;
        public EnumReportTypes ReportType
        {
            get { return _reportTypes; }
            set
            {
                if(_reportTypes==value) return;
                _reportTypes = value;
                RaisePropertyChanged(()=>ReportType);
            }
        }
        #endregion

        #region CurrtReportType  //记录系统操作的状态，在查看报表时有应用
        private EnumReportTypes _currtReportType;
        public EnumReportTypes CurrtReportType
        {
            get { return _currtReportType; }
            set
            {
                if (_currtReportType == value) return;
                _currtReportType = value;
                RaisePropertyChanged(() => CurrtReportType);
            }
        }
        #endregion

        #region IsShowSyncTime

        private bool _isShowSyncTime;
        public bool IsShowSyncTime
        {
            get { return _isShowSyncTime; }
            set
            {
                if (IsShowSyncTime == value) return;
                _isShowSyncTime = value;
                RaisePropertyChanged(() => IsShowSyncTime);
            }
        }
        #endregion

        #region IsShowWeekSnd

        private bool _isShowWeekSnd;
        public bool IsShowWeekSnd
        {
            get { return _isShowWeekSnd; }
            set
            {
                if (_isShowWeekSnd == value) return;
                _isShowWeekSnd = value;
                RaisePropertyChanged(() => IsShowWeekSnd);
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
                if (_remind == value) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }

        #endregion

        #region OcCount

        
        private int _remindOcCount;
        public int OcCount
        {
            get { return _remindOcCount; }
            set
            {
                if (_remindOcCount == value) return;
                _remindOcCount = value;
                RaisePropertyChanged(() => OcCount);
            }
        }
        #endregion

        #region OcCountAns
        private int _remindOcCountOcCountAns;
        public int OcCountAns
        {
            get { return _remindOcCountOcCountAns; }
            set
            {
                if (_remindOcCountOcCountAns == value) return;
                _remindOcCountOcCountAns = value;
                RaisePropertyChanged(() => OcCountAns);
            }
        }
        #endregion

        #region ExLoopCount


        private List<Tuple<int,int>> _remindExLoopCount;
        public List<Tuple<int, int>> ExLoopCount
        {
            get { return _remindExLoopCount; }
            set
            {
                if (_remindExLoopCount == value) return;
                _remindExLoopCount = value;
                RaisePropertyChanged(() => ExLoopCount);
            }
        }
        #endregion

        //#region ExTmlCount


        //private List<int> _remindExTmlCount;
        //public List<int> ExTmlCount
        //{
        //    get { return _remindExTmlCount; }
        //    set
        //    {
        //        if (_remindExTmlCount == value) return;
        //        _remindExTmlCount = value;
        //        RaisePropertyChanged(() => ExTmlCount);
        //    }
        //}
        //#endregion

        #region OcTmlCount


        private int _remindOcTmlCount;
        public int OcTmlCount
        {
            get { return _remindOcTmlCount; }
            set
            {
                if (_remindOcTmlCount == value) return;
                _remindOcTmlCount = value;
                RaisePropertyChanged(() => OcTmlCount);
            }
        }
        #endregion

        #region OcTmlCountAns


        private int _remindOcTmlCountAns;
        public int OcTmlCountAns
        {
            get { return _remindOcTmlCountAns; }
            set
            {
                if (_remindOcTmlCountAns == value) return;
                _remindOcTmlCountAns = value;
                RaisePropertyChanged(() => OcTmlCountAns);
            }
        }
        #endregion

        private long _opeTime = 0;
        #region TimeAns
        private double  _remindOcCountOcCoIsShowOcCountAnsuntAns;
        public double TimeAns
        {
            get { return _remindOcCountOcCoIsShowOcCountAnsuntAns; }
            set
            {
                if (_remindOcCountOcCoIsShowOcCountAnsuntAns == value) return;
                _remindOcCountOcCoIsShowOcCountAnsuntAns = value;
                RaisePropertyChanged(() => TimeAns);
            }
        }
        #endregion

        #region Items

        private ObservableCollection<TreeNodeBase> _items;
        public ObservableCollection<TreeNodeBase> Items
        {
            get { return _items ?? (_items = new ObservableCollection<TreeNodeBase>()); }
        }

        #endregion

        #region 开灯报表数据

        private ObservableCollection<TreeNodeBase> _openLightReport;
        public ObservableCollection<TreeNodeBase> OpenLightReport
        {
            get { return _openLightReport ?? (_openLightReport = new ObservableCollection<TreeNodeBase>()); }
        }
        #endregion

        #region 关灯报表数据

        private ObservableCollection<TreeNodeBase> _closeLightReport;
        public ObservableCollection<TreeNodeBase> CloseLightReport
        {
            get { return _closeLightReport ?? (_closeLightReport = new ObservableCollection<TreeNodeBase>()); }
        }
        #endregion

        #region 批量选测报表数据

        private ObservableCollection<TreeNodeBase> _selectionTestReport;
        public ObservableCollection<TreeNodeBase> SelectionTestReport
        {
            get { return _selectionTestReport ?? (_selectionTestReport = new ObservableCollection<TreeNodeBase>()); }
        }
        #endregion

        #region 对时报表数据

        private ObservableCollection<TreeNodeBase> _syncTimeReport;
        public ObservableCollection<TreeNodeBase> SyncTimeReport
        {
            get { return _syncTimeReport ?? (_syncTimeReport = new ObservableCollection<TreeNodeBase>()); }
        }
        #endregion

        #region 发送周设置报表数据

        private ObservableCollection<TreeNodeBase> _weekSndReport;
        public ObservableCollection<TreeNodeBase> WeekSndReport
        {
            get { return _weekSndReport ?? (_weekSndReport = new ObservableCollection<TreeNodeBase>()); }
        }
        #endregion

        #region 查看全部数据按钮显示控制

        #region ShowOpenLightAllData

        private bool _showOpenLightAllData;
        public bool ShowOpenLightAllData
        {
            get { return _showOpenLightAllData; }
            set
            {
                if(_showOpenLightAllData==value) return;
                _showOpenLightAllData = value;
                RaisePropertyChanged(()=>ShowOpenLightAllData);
            }
        }
        #endregion

        #region ShowCloseLightAllData
        private bool _showCloseLightAllData;
        public bool ShowCloseLightAllData
        {
            get { return _showCloseLightAllData; }
            set
            {
                if (_showCloseLightAllData == value) return;
                _showCloseLightAllData = value;
                RaisePropertyChanged(() => ShowCloseLightAllData);
            }
        }
        #endregion

        #region ShowSelectionAllData

        private bool _showSelectionAllData;
        public bool ShowSelectionAllData
        {
            get { return _showSelectionAllData; }
            set
            {
                if (_showSelectionAllData == value) return;
                _showSelectionAllData = value;
                RaisePropertyChanged(() => ShowSelectionAllData);
            }
        }

        #endregion

        #region ShowSynTimeAllData

        private bool _showSynTimeAllData;
        public bool ShowSynTimeAllData
        {
            get { return _showSynTimeAllData; }
            set
            {
                if (_showSynTimeAllData == value) return;
                _showSynTimeAllData = value;
                RaisePropertyChanged(() => ShowSynTimeAllData);
            }
        }

        #endregion

        #region ShowWeekSndAllData

        private bool _showWeekSndAllData;
        public bool ShowWeekSndAllData
        {
            get { return _showWeekSndAllData; }
            set
            {
                if (_showWeekSndAllData == value) return;
                _showWeekSndAllData = value;
                RaisePropertyChanged(() => ShowWeekSndAllData);
            }
        }

        #endregion
        #endregion


        #endregion

        #region Command

    

        #region 全选
 private ICommand _cmdselectall;
 public ICommand CmdSelectAll
        {
            get { return _cmdselectall ?? (_cmdselectall = new RelayCommand(ExSelectAll, CanSelectAll, false )); }
        }

        private bool  _currentSelectAllState = false;
        private void ExSelectAll()
        {
            _currentSelectAllState = !_currentSelectAllState;
            foreach (var t in this.Items)
            {
                t.IsChecked = _currentSelectAllState;
            }
        }

        private bool CanSelectAll()
        {
            return true;
        }
        #endregion

        #region 全选K1-K6
        private ICommand _cmdselectallK;
        public ICommand CmdSelectAllk1k6
        {
            get { return _cmdselectallK ?? (_cmdselectallK = new RelayCommand(ExSelectAllk1k6, CanSelectAllk1k6, false)); }
        }

        private bool _currentSelectAllStatek1k6 = false;
        private void ExSelectAllk1k6()
        {
            _currentSelectAllStatek1k6 = !_currentSelectAllStatek1k6;
            foreach (var t in this.Items) t.IsSwitch0 = _currentSelectAllStatek1k6;
        }

        private bool CanSelectAllk1k6()
        {
            return true;
        }
        #endregion

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

            foreach (var f in Items)
            {
                f.IsChecked = false;
                f.IsSwitch0 = false;
                f.IsSwitch1Checked = false;
                f.IsSwitch2Checked = false;
                f.IsSwitch3Checked = false;
                f.IsSwitch4Checked = false;
                f.IsSwitch5Checked = false;
                f.IsSwitch6Checked = false;
                f.IsSwitch7Checked = false;
                f.IsSwitch8Checked = false;
                foreach (var g in f.ChildTreeItems) 
                {
                    g.IsChecked = false;
                    g.IsSwitch0 = false;
                    g.IsSwitch1Checked = false;
                    g.IsSwitch2Checked = false;
                    g.IsSwitch3Checked = false;
                    g.IsSwitch4Checked = false;
                    g.IsSwitch5Checked = false;
                    g.IsSwitch6Checked = false;
                    g.IsSwitch7Checked = false;
                    g.IsSwitch8Checked = false;
                    g.IsK1ShowOpenOrColseAns = false;
                    g.IsK2ShowOpenOrColseAns = false;
                    g.IsK3ShowOpenOrColseAns = false;
                    g.IsK4ShowOpenOrColseAns = false;
                    g.IsK5ShowOpenOrColseAns = false;
                    g.IsK6ShowOpenOrColseAns = false;
                    g.IsK7ShowOpenOrColseAns = false;
                    g.IsK8ShowOpenOrColseAns = false;
                    g.SyncTimeAns = false;
                    g.WeekSndAns =EnumWeekSndAns.Ready;
                    g.K0SelectionTestAns = EnumSelectionTestAns.Ready;
                }
            }


            _id = DateTime.Now.Ticks;
 
            OpenLightReport.Clear();
            CloseLightReport.Clear();
            SelectionTestReport.Clear();
            SyncTimeReport.Clear();
            WeekSndReport.Clear();

        }
        private bool CanReset()
        {
            return DateTime.Now.Ticks - _dtReset.Ticks > 30000000;
        }
        #endregion

        #region CmdOpenLight

        private DateTime _dtOpenLight;
        private ICommand _cmdOpenLight;
        public ICommand CmdOpenLight
        {
            get { return _cmdOpenLight ?? (_cmdOpenLight = new RelayCommand<string >(ExOpenLight,CanOpenLight,true)); }
        }
        private void ExOpenLight(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            if (x == 1)
            {
                _dtOpenLight = DateTime.Now;
                //var nodes = TreeTmlNode.RegisterTmlNode;
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm)
                {
                    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行开关灯操作，\r\n若确定请输入验证码:1234", "");
                    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                    {
                        return;
                    }

                    if (sss != "1234")
                    {
                        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                        return;
                    }
                }
                if (_isOnOpenLight == 2)
                {
                    if (
                        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                            "当前正在进行开灯操作，确定现在进行开灯操作吗？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.Yes)
                    {

                        //清除关灯应答数据
                        foreach (var items in TreeTmlNode.RegisterTmlNode)
                        {
                            foreach (var item in items.Value)
                            {
                                item.K1OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K2OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K3OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K4OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K5OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K6OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K7OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K8OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                _isOnOpenLight = 1;

                var data = new Wlst.client.OpenCloseOperatorCenter
                               {
                                   Open = 1
                               };
                var k1Rtus = TreeTmlNode.GetNodeKxChecked(1);
                var k2Rtus = TreeTmlNode.GetNodeKxChecked(2);
                var k3Rtus = TreeTmlNode.GetNodeKxChecked(3);
                var k4Rtus = TreeTmlNode.GetNodeKxChecked(4);
                var k5Rtus = TreeTmlNode.GetNodeKxChecked(5);
                var k6Rtus = TreeTmlNode.GetNodeKxChecked(6);
                var k7Rtus = TreeTmlNode.GetNodeKxChecked(7);
                var k8Rtus = TreeTmlNode.GetNodeKxChecked(8);
                if (k1Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 1, Rtus = k1Rtus});
                if (k2Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 2, Rtus = k2Rtus});
                if (k3Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 3, Rtus = k3Rtus});
                if (k4Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 4, Rtus = k4Rtus});
                if (k5Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 5, Rtus = k5Rtus});
                if (k6Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 6, Rtus = k6Rtus});
                if (k7Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 7, Rtus = k7Rtus});
                if (k8Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 8, Rtus = k8Rtus});

                OcCount = 0;
                _opeTime = DateTime.Now.Ticks;
                TimeAns = 0;
                OcCountAns = 0;
                OcTmlCount = 0;
                int xtmp = 0;
                
                ExLoopCount = new List<Tuple<int, int>>();
                var nodes = TreeTmlNode.GetNodeChecked();
                OcTmlCount = nodes.Count;
                foreach (var f in data.Items)
                {
                    foreach (var g in f.Rtus)
                    {
                        ExLoopCount.Add(new Tuple<int, int>(g, f.LoopId)); //todo
                    }

                    xtmp += f.Rtus.Count;
                }
                OcCount = xtmp;

                var info = Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center;
                // .wlst_cnt_wj3090_order_open_close_light_center ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
                // info.WstCntOrderWj3090OpenClsoeCenter  = data;
                info.WstRtuCntOrderOpenCloseCenter = data;
                SndOrderServer.OrderSnd(info, 10, 6);


                CurrtReportType = EnumReportTypes.OpenLightReport;
                Remind = "开灯命令已发出....";

                ExSearchReport();
            }
            else if (x ==2)
            {
                if (ExLoopCount==null ||ExLoopCount.Count ==0)
                {
                    UMessageBox.Show("操作失败", "所有回路已经应答，不需要补开灯", UMessageBoxButton.Yes);
                    return;
                }
                var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行开关灯操作，\r\n若确定请输入验证码:1234", "");
                if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                {
                    return;
                }

                if (sss != "1234")
                {
                    UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                    return;
                }
                var lstRtu1 = new List<int>();
                var lstRtu2 = new List<int>();
                var lstRtu3 = new List<int>();
                var lstRtu4 = new List<int>();
                var lstRtu5 = new List<int>();
                var lstRtu6 = new List<int>();
                var lstRtu7 = new List<int>();
                var lstRtu8 = new List<int>();
                foreach (var g in ExLoopCount)
                {
                    if (g.Item2 == 1 && !lstRtu1.Contains(g.Item1)) lstRtu1.Add(g.Item1);
                    if (g.Item2 == 2 && !lstRtu2.Contains(g.Item1)) lstRtu2.Add(g.Item1);
                    if (g.Item2 == 3 && !lstRtu3.Contains(g.Item1)) lstRtu3.Add(g.Item1);
                    if (g.Item2 == 4 && !lstRtu4.Contains(g.Item1)) lstRtu4.Add(g.Item1);
                    if (g.Item2 == 5 && !lstRtu5.Contains(g.Item1)) lstRtu5.Add(g.Item1);
                    if (g.Item2 == 6 && !lstRtu6.Contains(g.Item1)) lstRtu6.Add(g.Item1);
                    if (g.Item2 == 7 && !lstRtu7.Contains(g.Item1)) lstRtu7.Add(g.Item1);
                    if (g.Item2 == 8 && !lstRtu8.Contains(g.Item1)) lstRtu8.Add(g.Item1);
                }
                var data = new Wlst.client.OpenCloseOperatorCenter
                {
                    Open = 1
                };
                if (lstRtu1.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 1, Rtus = lstRtu1 });
                if (lstRtu2.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 2, Rtus = lstRtu2 });
                if (lstRtu3.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 3, Rtus = lstRtu3 });
                if (lstRtu4.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 4, Rtus = lstRtu4 });
                if (lstRtu5.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 5, Rtus = lstRtu5 });
                if (lstRtu6.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 6, Rtus = lstRtu6 });
                if (lstRtu7.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 7, Rtus = lstRtu7 });
                if (lstRtu8.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 8, Rtus = lstRtu8 });

                var info = Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center;
                // .wlst_cnt_wj3090_order_open_close_light_center ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
                // info.WstCntOrderWj3090OpenClsoeCenter  = data;
                info.WstRtuCntOrderOpenCloseCenter = data;
                SndOrderServer.OrderSnd(info, 10, 6);


                CurrtReportType = EnumReportTypes.OpenLightReport;
                Remind = "补开灯命令已发出....";


            }
        }

        private bool CanOpenLight(string str)
        {
            return  DateTime.Now.Ticks - _dtOpenLight.Ticks > 30000000;
        }
        #endregion

        #region CmdCloseLight
        private DateTime _dtCloseLight;
        private ICommand _cmdCloseLight;
        public ICommand CmdCloseLight
        {
            get { return _cmdCloseLight ?? (_cmdCloseLight = new RelayCommand<string>(ExCloseLight,CanCloseLight,true)); }
        }
        private void ExCloseLight(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            if (x == 1)
            {
                _dtCloseLight = DateTime.Now;
                //var nodes = TreeTmlNode.RegisterTmlNode;
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm)
                {
                    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行开关灯操作，若确定请输入验证码:1234", "");
                    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                    {
                        return;
                    }
                    if (sss != "1234")
                    {
                        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                        return;
                    }
                }
                if (_isOnOpenLight == 1)
                {
                    if (
                        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                            "当前正在进行开灯操作，确定现在进行关灯操作吗？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.Yes)
                    {

                        foreach (var items in TreeTmlNode.RegisterTmlNode)
                        {
                            foreach (var item in items.Value)
                            {
                                item.K1OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K2OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K3OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K4OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K5OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K6OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K7OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                                item.K8OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                _isOnOpenLight = 2;



                var data = new Wlst.client.OpenCloseOperatorCenter
                               {
                                   Open = 2
                               };
                var k1Rtus = TreeTmlNode.GetNodeKxChecked(1);
                var k2Rtus = TreeTmlNode.GetNodeKxChecked(2);
                var k3Rtus = TreeTmlNode.GetNodeKxChecked(3);
                var k4Rtus = TreeTmlNode.GetNodeKxChecked(4);
                var k5Rtus = TreeTmlNode.GetNodeKxChecked(5);
                var k6Rtus = TreeTmlNode.GetNodeKxChecked(6);
                var k7Rtus = TreeTmlNode.GetNodeKxChecked(7);
                var k8Rtus = TreeTmlNode.GetNodeKxChecked(8);
                if (k1Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 1, Rtus = k1Rtus});
                if (k2Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 2, Rtus = k2Rtus});
                if (k3Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 3, Rtus = k3Rtus});
                if (k4Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 4, Rtus = k4Rtus});
                if (k5Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 5, Rtus = k5Rtus});
                if (k6Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 6, Rtus = k6Rtus});
                if (k7Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 7, Rtus = k7Rtus});
                if (k8Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 8, Rtus = k8Rtus});


                OcCount = 0;
                _opeTime = DateTime.Now.Ticks;
                TimeAns = 0;
                OcCountAns = 0;
                OcTmlCount = 0;
                var nodes = TreeTmlNode.GetNodeChecked();
                OcTmlCount = nodes.Count;
                ExLoopCount =new List<Tuple<int, int>>();
                int xtmp = 0;
                foreach (var f in data.Items)
                {
                    foreach (var g in f.Rtus)
                    {
                        ExLoopCount.Add(new Tuple<int, int>(g, f.LoopId)); 
                    }
                    xtmp += f.Rtus.Count;
                }
                OcCount = xtmp;

                var info = Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center;
                    // .wlst_cnt_wj3090_order_open_close_light_center ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
                info.WstRtuCntOrderOpenCloseCenter = data;
                SndOrderServer.OrderSnd(info, 10, 6);
                CurrtReportType = EnumReportTypes.CloseLightReport;
                Remind = "关灯命令已发出，请等待数据反馈...";

                ExSearchReport();
            }
            else if (x==2)
            {

                if (ExLoopCount == null || ExLoopCount.Count == 0)
                {
                    UMessageBox.Show("操作失败", "所有回路已经应答，不需要补关灯", UMessageBoxButton.Yes);
                    return;
                }
                var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行开关灯操作，若确定请输入验证码:1234", "");
                if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                {
                    return;
                }

                if (sss != "1234")
                {
                    UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                    return;
                }
                var lstRtu1 = new List<int>();
                var lstRtu2 = new List<int>();
                var lstRtu3 = new List<int>();
                var lstRtu4 = new List<int>();
                var lstRtu5 = new List<int>();
                var lstRtu6 = new List<int>();
                var lstRtu7 = new List<int>();
                var lstRtu8 = new List<int>();
                foreach (var g in ExLoopCount)
                {
                    if (g.Item2 == 1 && !lstRtu1.Contains(g.Item1)) lstRtu1.Add(g.Item1);
                    if (g.Item2 == 2 && !lstRtu2.Contains(g.Item1)) lstRtu2.Add(g.Item1);
                    if (g.Item2 == 3 && !lstRtu3.Contains(g.Item1)) lstRtu3.Add(g.Item1);
                    if (g.Item2 == 4 && !lstRtu4.Contains(g.Item1)) lstRtu4.Add(g.Item1);
                    if (g.Item2 == 5 && !lstRtu5.Contains(g.Item1)) lstRtu5.Add(g.Item1);
                    if (g.Item2 == 6 && !lstRtu6.Contains(g.Item1)) lstRtu6.Add(g.Item1);
                    if (g.Item2 == 7 && !lstRtu7.Contains(g.Item1)) lstRtu7.Add(g.Item1);
                    if (g.Item2 == 8 && !lstRtu8.Contains(g.Item1)) lstRtu8.Add(g.Item1);
                }
                var data = new Wlst.client.OpenCloseOperatorCenter
                {
                    Open = 2
                };
                if (lstRtu1.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 1, Rtus = lstRtu1 });
                if (lstRtu2.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 2, Rtus = lstRtu2 });
                if (lstRtu3.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 3, Rtus = lstRtu3 });
                if (lstRtu4.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 4, Rtus = lstRtu4 });
                if (lstRtu5.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 5, Rtus = lstRtu5 });
                if (lstRtu6.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 6, Rtus = lstRtu6 });
                if (lstRtu7.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 7, Rtus = lstRtu7 });
                if (lstRtu8.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 8, Rtus = lstRtu8 });

                var info = Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center;
                // .wlst_cnt_wj3090_order_open_close_light_center ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
                // info.WstCntOrderWj3090OpenClsoeCenter  = data;
                info.WstRtuCntOrderOpenCloseCenter = data;
                SndOrderServer.OrderSnd(info, 10, 6);


                CurrtReportType = EnumReportTypes.OpenLightReport;
                Remind = "补关灯命令已发出....";

            }
        }
        private bool CanCloseLight(string str)
        {
            return DateTime.Now.Ticks - _dtCloseLight.Ticks > 30000000;
        }
        #endregion

        #region CmdSelectTest
        private DateTime _dtSelectTest;
        private ICommand _cmdSelectTest;
        public ICommand CmdSelectTest
        {
            get { return _cmdSelectTest ?? (_cmdSelectTest = new RelayCommand(ExSelectTest, CanSelectTest, true)); }
        }
        private void ExSelectTest()
        {
            _dtSelectTest = DateTime.Now;
            OcTmlCount = 0;
            OcTmlCountAns = 0;
            var nodes = TreeTmlNode.GetNodeChecked();
                // (from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value).ToList();
            foreach (var tts in TreeTmlNode.RegisterTmlNode)
            {
                foreach (var tt in tts.Value)
                {
                    tt.K0SelectionTestAns = EnumSelectionTestAns.Ready;
                    tt.K1SelectionTestAns = EnumSelectionTestAns.Ready;
                    tt.K2SelectionTestAns = EnumSelectionTestAns.Ready;
                    tt.K3SelectionTestAns = EnumSelectionTestAns.Ready;
                    tt.K4SelectionTestAns = EnumSelectionTestAns.Ready;
                    tt.K5SelectionTestAns = EnumSelectionTestAns.Ready;
                    tt.K6SelectionTestAns = EnumSelectionTestAns.Ready;
                    tt.K7SelectionTestAns = EnumSelectionTestAns.Ready;
                    tt.K8SelectionTestAns = EnumSelectionTestAns.Ready;
                    tt.SelectVisi = true;
                }
            }
            if (nodes.Count < 1) return;
            OcTmlCount   = nodes.Count;
            var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.AddRange(nodes);
            info.WstRtuOrders.RtuIds.AddRange(nodes);
            info.WstRtuOrders.Op = 31;
            SndOrderServer.OrderSnd(info, 10, 6);
            CurrtReportType = EnumReportTypes.SelectionTestReport;
            Remind = "批量选测命令已经发送...";

            _opeTime = DateTime.Now.Ticks;
            TimeAns = 0;
            ExSearchReport();
        
        }
        private bool CanSelectTest()
        {
            return  DateTime.Now.Ticks - _dtSelectTest.Ticks > 30000000;
        }
        #endregion

        #region CmdSelectTestAgain
        private DateTime _dtSelectTestAgain;
        private ICommand _cmdSelectTestAgain;
        public ICommand CmdSelectTestAgain
        {
            get { return _cmdSelectTestAgain ?? (_cmdSelectTestAgain = new RelayCommand(ExSelectTestAgain, CanSelectTestAgain, true)); }
        }
        private void ExSelectTestAgain()
        {
            _dtSelectTestAgain = DateTime.Now;
            var lst = SelectionTestReport.Select(t => t.NodeId).ToList();
            if(lst.Count<1) return;
            //var info = Sr.ProtocolPhone .ServerListen .wlst_cnt_request_wj3090_measure ;//.ProtocolCnt.ServerPart.wlst_Measures_clinet_order_RtuMeasure;
            //info.Args .Addr .AddRange(lst);


            var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.AddRange(lst );
            info.WstRtuOrders.RtuIds.AddRange(lst );
            info.WstRtuOrders.Op = 31;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "批量选测命令已经发送...";
        }
        private bool CanSelectTestAgain()
        {
            return DateTime.Now.Ticks - _dtSelectTestAgain.Ticks > 30000000;
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
            OcTmlCount = 0;
            OcTmlCountAns = 0;
            IsShowSyncTime = true;
            var lstRtu = TreeTmlNode.GetNodeChecked();// from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value;
            OcTmlCount = lstRtu.Count;
            foreach (var t in TreeTmlNode.RegisterTmlNode .Values )
            {
                foreach (var g in t)
                g.SyncTimeAns = false;
            }
            //var lstRtu =nodes.Select(t=>t.NodeId);
            //var info = Sr.ProtocolPhone .ServerListen .wlst_cnt_request_asyn_rtu_time ;//.ServerPart.wlst_asyntime_clinet_order_asynrtutime;
            //info.Args .Addr .AddRange(lstRtu);
            //info.WstCntRequestAsynRtuTime .DateSnd  = DateTime.Now.Ticks ;
            //SndOrderServer.OrderSnd(info, 10, 6);

           
            

            var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.AddRange(lstRtu);
            info.WstRtuOrders.RtuIds.AddRange(lstRtu);
            info.WstRtuOrders.Op = 21;
            info.WstRtuOrders.DtTime = DateTime.Now.Ticks;
            SndOrderServer.OrderSnd(info, 10, 6);
            CurrtReportType = EnumReportTypes.AsyncTimeReport;
            Remind = "对时命令已发出！！！";
            _opeTime = DateTime.Now.Ticks;
            TimeAns = 0;
            ExSearchReport();
        }
        private bool CanAsynTime()
        {
            return  DateTime.Now.Ticks - _dtAsynTime.Ticks > 30000000;
        }
        #endregion


        #region CmdAsynTimeAgain
        private DateTime _dtAsynTimeAgain;
        private ICommand _cmdAsynTimeAgain;
        public ICommand CmdAsynTimeAgain
        {
            get { return _cmdAsynTimeAgain ?? (_cmdAsynTimeAgain = new RelayCommand(ExAsynTimeAgain, CanAsynTimeAgain, true)); }
        }
        private void ExAsynTimeAgain()
        {
            _dtAsynTimeAgain = DateTime.Now;
            var lstRtu = SyncTimeReport.Select(t => t.NodeId);
            //var info = Sr.ProtocolPhone .ServerListen .wlst_cnt_request_asyn_rtu_time ;//.ServerPart.wlst_asyntime_clinet_order_asynrtutime;
            //info.Args .Addr .AddRange(lstRtu);
            //info.WstCntRequestAsynRtuTime.DateSnd  = DateTime.Now.Ticks ;


            var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.AddRange(lstRtu );
            info.WstRtuOrders.RtuIds.AddRange(lstRtu );
            info.WstRtuOrders.DtTime = DateTime.Now.Ticks;
            info.WstRtuOrders.Op = 21;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "对时命令已发出！！！";
        }
        private bool CanAsynTimeAgain()
        {
            return DateTime.Now.Ticks - _dtAsynTimeAgain.Ticks > 30000000;
        }
        #endregion

        #region CmdSndWeekSet
        private DateTime _dtSndWeekSet;
        private ICommand _cmdSndWeekSet;
        public ICommand CmdSndWeekSet
        {
            get { return _cmdSndWeekSet ?? (_cmdSndWeekSet = new RelayCommand(ExSndWeekSet, CanSndWeekSet, true)); }
        }
        private void ExSndWeekSet()
        {
            _dtSndWeekSet = DateTime.Now;
            IsShowWeekSnd = true;
            OcTmlCount = 0;
            OcTmlCountAns = 0;
            var lstRtu = TreeTmlNode.GetNodeChecked();//( from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value).ToList();
            if (lstRtu.Count < 1) return;

            foreach (var ts in TreeTmlNode .RegisterTmlNode .Values )
            {
                foreach (var t in ts )
                t.WeekSndAns=EnumWeekSndAns.Ready;
            }
          //  var lstRtu = nodes.Select(t => t.NodeId);
            //var info = Sr.ProtocolPhone .ServerListen .wlst_cnt_request_snd_rtu_time  ;//.ServerPart.wlst_asyntime_clinet_order_sendweekset;
            //info.Args .Addr .AddRange(lstRtu);

            
            var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
            OcTmlCount = lstRtu.Count;
            info.Args.Addr.AddRange(lstRtu);
            info.WstRtuOrders.RtuIds.AddRange(lstRtu);
            info.WstRtuOrders.DtTime = DateTime.Now.Ticks;
            info.WstRtuOrders.Op = 11;
            SndOrderServer.OrderSnd(info, 10, 6);
            CurrtReportType = EnumReportTypes.WeekSndReport;
            Remind = "发送周设置命令已发出...";

            _opeTime = DateTime.Now.Ticks;
            TimeAns = 0;
            ExSearchReport();
        }
        private bool CanSndWeekSet()
        {
            return   DateTime.Now.Ticks - _dtSndWeekSet.Ticks > 30000000;
        }
        #endregion

        #region CmdSndWeekSetAgain
        private DateTime _dtSndWeekSetAgain;
        private ICommand _cmdSndWeekSetAgain;
        public ICommand CmdSndWeekSetAgain
        {
            get { return _cmdSndWeekSetAgain ?? (_cmdSndWeekSetAgain = new RelayCommand(ExSndWeekSetAgain, CanSndWeekSetAgain, true)); }
        }
        private void ExSndWeekSetAgain()
        {
            _dtSndWeekSetAgain = DateTime.Now;
            var lstRtu = WeekSndReport.Select(t => t.NodeId);
            //var info = Sr.ProtocolPhone .ServerListen .wlst_cnt_request_snd_rtu_time ;//.ServerPart.wlst_asyntime_clinet_order_sendweekset;
            //info.Args .Addr .AddRange(lstRtu);


            var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders;//.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.AddRange(lstRtu);
            info.WstRtuOrders.RtuIds.AddRange(lstRtu);
            info.WstRtuOrders.Op = 21;
            info.WstRtuOrders.DtTime = DateTime.Now.Ticks;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "发送周设置命令已发出...";
        }
        private bool CanSndWeekSetAgain()
        {
            return DateTime.Now.Ticks - _dtSndWeekSetAgain.Ticks > 30000000;
        }
        #endregion

        #region CmdStopRun
        private DateTime _dtStopRun;
        private ICommand _cmdStopRun;
        public ICommand CmdStopRun
        {
            get { return _cmdStopRun ?? (_cmdStopRun = new RelayCommand(ExStopRun, CanStopRun, true)); }
        }
        private void ExStopRun()
        {
            _dtStopRun = DateTime.Now;
            var shut=new ShutDownOrReRunWindow(true);
            shut.ShowDialog();

        }
        private bool CanStopRun()
        {
            return DateTime.Now.Ticks - _dtStopRun.Ticks > 30000000;
        }
        #endregion

        #region CmdReRun
        private DateTime _dtReRun;
        private ICommand _cmdReRun;
        public ICommand CmdReRun
        {
            get { return _cmdReRun ?? (_cmdReRun = new RelayCommand(ExReRun, CanReRun, true)); }
        }
        private void ExReRun()
        {
            _dtReRun = DateTime.Now;
            var shut = new ShutDownOrReRunWindow(false);
            shut.ShowDialog();
        }
        private bool CanReRun()
        {
            return DateTime.Now.Ticks - _dtReRun.Ticks > 30000000;
        }
        #endregion

        #region CmdSearchReport

        private DateTime _dtSearchReport;
        private ICommand _cmdSearchReport;
        public ICommand CmdSearchReport
        {
            get { return _cmdSearchReport ?? (_cmdSearchReport = new RelayCommand(ExSearchReport,CanSearchReport,true)); }
        }
        private void ExSearchReport()
        {
            _dtSearchReport = DateTime.Now;
            ReportType = CurrtReportType;
    
            switch (CurrtReportType)
            {
                    case EnumReportTypes.OpenLightReport:
                    LoadOpenLightReport();
                    break;
                    case EnumReportTypes.CloseLightReport:
                    LoadCloseLightReport();
                    break;
                    case EnumReportTypes.SelectionTestReport:
                    LoadSelectionTestReport();
                    break;
                    case EnumReportTypes.AsyncTimeReport:
                    LoadAsyncTimeReport();
                    break;
                    case EnumReportTypes.WeekSndReport:
                    LoadWeekSndReport();
                    break;

            }
        }
        private bool CanSearchReport()
        {
            return DateTime.Now.Ticks - _dtSearchReport.Ticks > 30000000;
        }
        #endregion

        #region CmdTurnBack

        private DateTime _dtTurnBack;
        private ICommand _cmdTurnBack;
        public ICommand CmdTurnBack
        {
            get { return _cmdTurnBack ?? (_cmdTurnBack = new RelayCommand(ExTurnBack, CanTurnBack, true)); }
        }
        private void ExTurnBack()
        {
            _dtTurnBack = DateTime.Now;
            ReportType=EnumReportTypes.NoReport;

        }
        private bool CanTurnBack()
        {
            return DateTime.Now.Ticks - _dtTurnBack.Ticks > 30000000;
        }
        #endregion

        #region 报表数据过滤

        #region CmdWatchOpenLightForNoResponseData

        private DateTime _dtWatchOpenLightForNoResponseData;
        private ICommand _cmdWatchOpenLightForNoResponseData;
        public ICommand CmdWatchOpenLightForNoResponseData
        {
            get
            {
                return _cmdWatchOpenLightForNoResponseData ??
                       (_cmdWatchOpenLightForNoResponseData =
                        new RelayCommand(ExWatchOpenLightForNoResponseData, CanWatchOpenLightForNoResponseData, true));
            }
        }
        private void ExWatchOpenLightForNoResponseData()
        {
            _dtWatchOpenLightForNoResponseData=DateTime.Now;
            ShowOpenLightAllData = true;
            for (var i = 0; i < OpenLightReport.Count; i++)
            {
                var item = OpenLightReport[i];
                var condition = true;
                if (item.IsSwitch1Checked)
                    condition =item.K1OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch2Checked)
                    condition= condition && item.K2OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch3Checked)
                    condition = condition && item.K3OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch4Checked)
                    condition = condition && item.K4OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch5Checked)
                    condition = condition && item.K5OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch6Checked)
                    condition = condition && item.K6OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch7Checked)
                    condition = condition && item.K7OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch8Checked)
                    condition = condition && item.K8OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (!condition) continue;
                OpenLightReport.RemoveAt(i);
                i--;
            }
        }
        private bool  CanWatchOpenLightForNoResponseData()
        {
            return DateTime.Now.Ticks-_dtWatchOpenLightForNoResponseData.Ticks>30000000;
        }
        #endregion

        #region CmdWatchCloseLightForNoResponseData

        private DateTime _dtWatchCloseLightForNoReponseData;
        private ICommand _cmdWatchCloseLightForNoResponseData;
        public ICommand CmdWatchCloseLightForNoResponseData
        {
            get
            {
                return _cmdWatchCloseLightForNoResponseData ??
                       (_cmdWatchCloseLightForNoResponseData =
                        new RelayCommand(ExWatchCloseLightForNoResponseData, CanWatchCloseLightForNoResponseData, true));
            }
        }
        private void ExWatchCloseLightForNoResponseData()
        {
            _dtWatchCloseLightForNoReponseData = DateTime.Now;
            ShowCloseLightAllData = true;
            for (var i = 0; i < CloseLightReport.Count; i++)
            {
                var item = CloseLightReport[i];
                var condition = true;
                if (item.IsSwitch1Checked)
                    condition = item.K1OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch2Checked)
                    condition = condition && item.K2OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch3Checked)
                    condition = condition && item.K3OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch4Checked)
                    condition = condition && item.K4OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch5Checked)
                    condition = condition && item.K5OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch6Checked)
                    condition = condition && item.K6OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch7Checked)
                    condition = condition && item.K7OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (item.IsSwitch8Checked)
                    condition = condition && item.K8OpenOrCloseAns == EnumOpenOrCloseAns.YesAnswer;
                if (!condition) continue;
                CloseLightReport.RemoveAt(i);
                i--;
            }

        }
        private bool CanWatchCloseLightForNoResponseData()
        {
            return DateTime.Now.Ticks - _dtWatchCloseLightForNoReponseData.Ticks > 30000000;
        }
        #endregion

        #region CmdWatchSelectionTestForNoResponseData
        private DateTime _dtWatchSelectionTestForNoResponseData;
        private ICommand _cmdWatchSelectionTestForNoResponseData;
        public ICommand CmdWatchSelectionTestForNoResponseData
        {
            get
            {
                return _cmdWatchSelectionTestForNoResponseData ??
                       (_cmdWatchSelectionTestForNoResponseData =
                        new RelayCommand(ExWatchSelectionTestForNoResponseData, CanWatchSelectionTestForNoResponseData, true));
            }
        }
        private void ExWatchSelectionTestForNoResponseData()
        {
            _dtWatchSelectionTestForNoResponseData = DateTime.Now;
            ShowSelectionAllData = true;
            for (var i = 0; i < SelectionTestReport.Count; i++)
            {
                var item = SelectionTestReport[i];
                if (item.K1SelectionTestAns ==EnumSelectionTestAns.Ready||
                    item.K2SelectionTestAns == EnumSelectionTestAns.Ready ||
                    item.K3SelectionTestAns == EnumSelectionTestAns.Ready ||
                    item.K4SelectionTestAns == EnumSelectionTestAns.Ready ||
                    item.K5SelectionTestAns == EnumSelectionTestAns.Ready ||
                    item.K6SelectionTestAns == EnumSelectionTestAns.Ready ||
                    item.K7SelectionTestAns == EnumSelectionTestAns.Ready ||
                    item.K8SelectionTestAns == EnumSelectionTestAns.Ready) continue;
                SelectionTestReport.RemoveAt(i);
                i--;
            }

        }
        private bool CanWatchSelectionTestForNoResponseData()
        {
            return DateTime.Now.Ticks - _dtWatchSelectionTestForNoResponseData.Ticks > 30000000;
        }
        #endregion

        #region CmdWatchSyncTimeForNoResponseData
        private DateTime _dtWatchSyncTimeForNoResponseData;
        private ICommand _cmdWatchSyncTimeForNoResponseData;
        public ICommand CmdWatchSyncTimeForNoResponseData
        {
            get
            {
                return _cmdWatchSyncTimeForNoResponseData ??
                       (_cmdWatchSyncTimeForNoResponseData =
                        new RelayCommand(ExWatchSyncTimeForNoResponseData, CanWatchSyncTimeForNoResponseData, true));
            }
        }
        private void ExWatchSyncTimeForNoResponseData()
        {
            _dtWatchSyncTimeForNoResponseData = DateTime.Now;
            ShowSynTimeAllData = true;
            for (var i = 0; i < SyncTimeReport.Count; i++)
            {
                var item = SyncTimeReport[i];
                if (!item.SyncTimeAns) continue;
                SyncTimeReport.RemoveAt(i);
                i--;
            }

        }
        private bool CanWatchSyncTimeForNoResponseData()
        {
            return DateTime.Now.Ticks - _dtWatchSyncTimeForNoResponseData.Ticks > 30000000;
        }
        #endregion

        #region CmdWatchWeekSndForNoResponseData
        private DateTime _dtWatchWeekSndForNoResponseData;
        private ICommand _cmdWatchWeekSndForNoResponseData;
        public ICommand CmdWatchWeekSndForNoResponseData
        {
            get
            {
                return _cmdWatchWeekSndForNoResponseData ??
                       (_cmdWatchWeekSndForNoResponseData =
                        new RelayCommand(ExWatchWeekSndForNoResponseData, CanWatchWeekSndForNoResponseData, true));
            }
        }
        private void ExWatchWeekSndForNoResponseData()
        {
            _dtWatchWeekSndForNoResponseData = DateTime.Now;
            ShowWeekSndAllData = true;
            for (var i = 0; i < WeekSndReport.Count; i++)
            {
                var item = WeekSndReport[i];
                if (item.WeekSndAns != EnumWeekSndAns.AllAns) continue;
                WeekSndReport.RemoveAt(i);
                i--;
            }

        }
        private bool CanWatchWeekSndForNoResponseData()
        {
            return DateTime.Now.Ticks - _dtWatchWeekSndForNoResponseData.Ticks > 30000000;
        }
        #endregion

        #endregion

        #region 查看报表全部数据

        #region   CmdWatchOpenLightAllData

        private DateTime _dtWatchOpenLightAllData;
        private ICommand _cmdWatchOpenLightAllData;
        public ICommand CmdWatchOpenLightAllData
        {
            get
            {
                return _cmdWatchOpenLightAllData ??
                       (_cmdWatchOpenLightAllData =
                        new RelayCommand(ExWatchOpenLightAllData, CanWatchOpenLigthAllData, true));
            }
        }
        private void ExWatchOpenLightAllData()
        {
            _dtWatchOpenLightAllData = DateTime.Now;
            ShowOpenLightAllData = false;
            LoadOpenLightReport();
        }
        private bool CanWatchOpenLigthAllData()
        {
            return DateTime.Now.Ticks - _dtWatchOpenLightAllData.Ticks > 30000000;
        }
        #endregion

        #region CmdWatchCloseLightAllData

        private DateTime _dtWatchCloseLightAllData;
        private ICommand _cmdWatchCloseLightAllData;
        public ICommand CmdWatchCloseLightAllData
        {
            get
            {
                return _cmdWatchCloseLightAllData ??
                       (_cmdWatchCloseLightAllData =
                        new RelayCommand(ExWatchCloseLightAllData, CanWatchCloseLightAllData, true));
            }
        }
        private void ExWatchCloseLightAllData()
        {
            _dtWatchCloseLightAllData = DateTime.Now;
            ShowCloseLightAllData = false;
           LoadCloseLightReport();
        }
        private bool CanWatchCloseLightAllData()
        {
            return DateTime.Now.Ticks - _dtWatchCloseLightAllData.Ticks > 30000000;
        }

        #endregion

        #region CmdWatchSelectionTestAllData

        private DateTime _dtWatchSelectionTestAllData;
        private ICommand _cmdWatchSelectionTestAllData;
        public ICommand CmdWatchSelectionTestAllData
        {
            get
            {
                return _cmdWatchSelectionTestAllData ??
                       (_cmdWatchSelectionTestAllData =
                        new RelayCommand(ExWatchSelectionTestAllData, CanWatchSelectionTestAllData, true));
            }
        }
        private void ExWatchSelectionTestAllData()
        {
            _dtWatchSelectionTestAllData = DateTime.Now;
            ShowSelectionAllData = false;
            LoadSelectionTestReport();
        }
        private bool CanWatchSelectionTestAllData()
        {
            return DateTime.Now.Ticks - _dtWatchSelectionTestAllData.Ticks > 30000000;
        }

        #endregion

        #region CmdWatchSynTimeAllData
        private DateTime _dtWatchSynTimeAllData;
        private ICommand _cmdWatchSynTimeAllData;
        public ICommand CmdWatchSynTimeAllData
        {
            get
            {
                return _cmdWatchSynTimeAllData ??
                       (_cmdWatchSynTimeAllData =
                        new RelayCommand(ExWatchSynTimeAllData, CanWatchSynTimeAllData, true));
            }
        }
        private void ExWatchSynTimeAllData()
        {
            _dtWatchSynTimeAllData = DateTime.Now;
            ShowSynTimeAllData = false;
            LoadAsyncTimeReport();
        }
        private bool CanWatchSynTimeAllData()
        {
            return DateTime.Now.Ticks - _dtWatchSynTimeAllData.Ticks > 30000000;
        }
        #endregion

        #region CmdWatchWeekSndAllData
        private DateTime _dtWatchWeekSndAllData;
        private ICommand _cmdWatchWeekSndAllData;
        public ICommand CmdWatchWeekSndAllData
        {
            get
            {
                return _cmdWatchWeekSndAllData ??
                       (_cmdWatchWeekSndAllData =
                        new RelayCommand(ExWatchWeekSndAllData, CanWatchWeekSndAllData, true));
            }
        }
        private void ExWatchWeekSndAllData()
        {
            _dtWatchWeekSndAllData = DateTime.Now;
            ShowWeekSndAllData = false;
            LoadWeekSndReport();
        }
        private bool CanWatchWeekSndAllData()
        {
            return DateTime.Now.Ticks - _dtWatchWeekSndAllData.Ticks > 30000000;
        }
        #endregion

        #endregion

        #endregion

        #region 区域显示列宽度

 


 



        private bool  _areaCount;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public bool  AreaCount
        {
            get { return _areaCount; }
            set
            {
                if (_areaCount != value)
                {
                    _areaCount = value;
                    this.RaisePropertyChanged(() => this.AreaCount);
                }
            }
        }

        #endregion
    }
    /// <summary>
    /// Methods
    /// </summary>
    public partial class ControlCenterViewModel2
    {
       

        //初始化时加载左侧树终端节点
        private void LoadTreeNodeGlobal()
        {
            Items.Clear();
            if(!TreeTmlNode.ClearRegisterTmlNodes()) return;
            var areas = new List<int>();
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo .AreaX )
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW )
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
            //foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR )
            //{
            //    if (areas.Contains(f) == false) areas.Add(f);
            //}
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    if (areas.Contains(f) == false) areas.Add(f);
                }
            }
            AreaCount = areas.Count >1 ;
            foreach (var f in areas )
            {
                var grps = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(f);
                foreach (var g in grps )
                {
                    this.Items.Add(new TreeGroupNode(f, g.GroupId));
                }
                this.Items.Add(new TreeGroupNode(f, 0));
            }

            for (int i = Items.Count - 1; i >= 0;i-- )
            {
                if (Items[i].ChildTreeItems.Count == 0) Items.RemoveAt(i);
            }
        }

        //初始化时加载左侧树终端节点
        private void LoadTreeNodeLocal()
        {
            Items.Clear();
            if (!TreeTmlNode.ClearRegisterTmlNodes()) return;
            var areas = new List<int>();
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
             
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    if (areas.Contains(f) == false) areas.Add(f);
                }
            }
            AreaCount = areas.Count >1;// ? 0 : 150;

            var ntg = (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.ItemsMultiGrp
                       orderby t.Key.Item1 , t.Key.Item2
                       select t.Value ).ToList();
            foreach (var f in ntg )
            {
                if (f.LstTml.Count > 0) this.Items.Add(new TreeGroupNodeLoacl( f));
            }
        }

        //初始化时加载左侧树终端节点
        private void LoadTreeNodeTemp(List<int> rtus)
        {
            Items.Clear();

            this.Items.Add(new TreeGroupNodeTemp(rtus));

        }


        private void LoadOpenLightReport()
        {
         

            OpenLightReport.Clear();
            var nodes = TreeTmlNode.GetAnykxChecked( );
            foreach (var node in nodes)
            {
                OpenLightReport.Add(node);
            }
        

        }

        private void LoadCloseLightReport()
        {
        
            CloseLightReport.Clear();
            var nodes = TreeTmlNode.GetAnykxChecked();
            foreach (var node in nodes)
            {
                CloseLightReport.Add(node);
            }

        }
        private void LoadSelectionTestReport()
        {
            SelectionTestReport.Clear();
            var nodes = TreeTmlNode.GetAnyChecked();
               // (from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value).ToList();
            foreach (var node in nodes)
            {
                SelectionTestReport.Add(node);
            }
        }
        private void LoadAsyncTimeReport()
        {
            SyncTimeReport.Clear();
            var nodes = TreeTmlNode.GetAnyChecked();
          //    (from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value).ToList();
            foreach (var node in nodes)
            {
                SyncTimeReport.Add(node);
            }
        }

        private void LoadWeekSndReport()
        {
            WeekSndReport.Clear();
            var nodes = TreeTmlNode.GetAnyChecked();
            foreach (var node in nodes)
            {
                WeekSndReport.Add(node);
            }
        }

  
    }

    public partial class ControlCenterViewModel2
    {
        private void InitEvent()
        {
            AddEventFilterInfo(Cr.CoreOne.CoreIdAssign.EventIdAssign.AsyncTimeEventId, PublishEventType.Core);
            //AddEventFilterInfo(Cr.CoreOne.CoreIdAssign.EventIdAssign.OpenOrCloseLightReceiveEventId, PublishEventType.Core);
            AddEventFilterInfo(EventIdAssign.RunningInfoUpdate2 , PublishEventType.Core);


        }
        public override void ExPublishedEvent(PublishEventArgs args)
        {
            #region 时间同步
            if (args.EventId == Cr.CoreOne.CoreIdAssign.EventIdAssign.AsyncTimeEventId)  //事件在OpenCloseLightDataDispatch文件中监听，后发布该事件
            {
                var lst = args.GetParams()[0] as List<int>;
                if (lst == null) return;

                foreach (var key in TreeTmlNode.RegisterTmlNode.Keys.ToList().Where(lst.Contains))
                {
                    foreach (var f in TreeTmlNode.RegisterTmlNode[key])
                f.SyncTimeAns =true;
                }

                Remind = "时钟同步数据已返回！！！";
            }
            #endregion

            #region 开关灯
            else if(args.EventId==Cr.CoreOne.CoreIdAssign.EventIdAssign.OpenOrCloseLightReceiveEventId)
            {
                var rtuid = Convert.ToInt32(args.GetParams()[0]);
                var loopid = Convert.ToInt32(args.GetParams()[1]);
                var isOpen = Convert.ToBoolean(args.GetParams()[2]);
                if (!TreeTmlNode.RegisterTmlNode.Keys.ToList().Contains(rtuid)) return;
                switch (loopid)
                {
                    case 1:
                        if((isOpen && _isOnOpenLight==1) || (!isOpen && _isOnOpenLight==2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                            f.K1OpenOrCloseAns=EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 2:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                                f.K2OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 3:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid]) f.K3OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 4:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid]) f.K4OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 5:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid]) f.K5OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 6:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid]) f.K6OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 7:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid]) f.K7OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 8:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid]) f.K8OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                }
                Remind = "开关灯数据正在反馈...";
            }
            #endregion

            #region 批量选测
             else if(EventIdAssign.RunningInfoUpdate2 ==args.EventId)
             {
                 var lst = args.GetParams()[0] as List<int>;
                 if (lst == null || lst.Count < 1) return;
                 foreach (var items in TreeTmlNode.RegisterTmlNode)
                 {
                     if (lst.Contains(items.Key))
                     {
                         var tmp = RunningInfoHold .GetRunInfo(items.Key);
                         if (tmp == null || tmp .RtuNewData ==null ) continue;
                         foreach (var item in items.Value )
                         {
                             item.K0SelectionTestAns = EnumSelectionTestAns.Reply;
                             for (int i = 0; i < tmp.RtuNewData.IsSwitchOutAttraction.Count; i++)
                             {
                                 switch (i + 1)
                                 {
                                     case 1:
                                         item.K1SelectionTestAns = tmp.RtuNewData.IsSwitchOutAttraction[0]
                                                                             ? EnumSelectionTestAns.Open
                                                                             : EnumSelectionTestAns.Close;
                                         break;
                                     case 2:
                                         item.K2SelectionTestAns = tmp.RtuNewData.IsSwitchOutAttraction[1]
                                                                             ? EnumSelectionTestAns.Open
                                                                             : EnumSelectionTestAns.Close;
                                         break;
                                     case 3:
                                         item.K3SelectionTestAns = tmp.RtuNewData.IsSwitchOutAttraction[2]
                                                                             ? EnumSelectionTestAns.Open
                                                                             : EnumSelectionTestAns.Close;
                                         break;
                                     case 4:
                                         item.K4SelectionTestAns = tmp.RtuNewData.IsSwitchOutAttraction[3]
                                                                             ? EnumSelectionTestAns.Open
                                                                             : EnumSelectionTestAns.Close;
                                         break;
                                     case 5:
                                         item.K5SelectionTestAns = tmp.RtuNewData.IsSwitchOutAttraction[4]
                                                                             ? EnumSelectionTestAns.Open
                                                                             : EnumSelectionTestAns.Close;
                                         break;
                                     case 6:
                                         item.K6SelectionTestAns = tmp.RtuNewData.IsSwitchOutAttraction[5]
                                                                             ? EnumSelectionTestAns.Open
                                                                             : EnumSelectionTestAns.Close;
                                         break;
                                     case 7:
                                         item.K7SelectionTestAns = tmp.RtuNewData.IsSwitchOutAttraction[6]
                                                                             ? EnumSelectionTestAns.Open
                                                                             : EnumSelectionTestAns.Close;
                                         break;
                                     case 8:
                                         item.K8SelectionTestAns = tmp.RtuNewData.IsSwitchOutAttraction[7]
                                                                             ? EnumSelectionTestAns.Open
                                                                             : EnumSelectionTestAns.Close;
                                         break;
                                     default:
                                         Remind = "开关数不在1-8之间";
                                         break;
                                 }
                             }
                         }
                     }
                 }
             }
            #endregion

        }
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu .wst_rtu_orders ,//.wlst_svr_ans_cnt_request_snd_rtu_time,
                                          //.ClientPart.wlst_asyntime_server_ans_clinet_order_sendweeksetk1k3,
                                          ResponseSndWeekSetK1K3, typeof (ControlCenterViewModel2), this);

            ProtocolServer.RegistProtocol(
              Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders,// .wlst_cnt_wj3090_order_snd_paras ,//.ClientPart.wlst_rtuargsupdate_server_ans_clinet_order_paras4000,
              RtuParaUpdate40000,
              typeof(ControlCenterViewModel2), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_svr_ans_cnt_order_rtu_open_close_light,// .wlst_svr_ans_cnt_wj3090_order_open_close_light ,//.ClientPart.wlst_OpenCloseLight_server_ans_clinet_order_opencloseLight ,
                ExExecuteOpenLight,
                typeof(ControlCenterViewModel2), this,true );
        }
        private void ExExecuteOpenLight(string session, Wlst.mobile.MsgWithMobile args)
        {
            var datax = args.WstRtuSvrAnsCntOrderOpenCloseLight;
            var lst = args.Args.Addr;
            if (lst == null) return;
            var tu = new Tuple<int, int>(datax.RtuId, datax.LoopId);


            lock (this)
            {
                if (ExLoopCount == null) return;
                if (ExLoopCount.Contains(tu))
                {
                    ExLoopCount.Remove(tu);
                }
                else
                {
                    return;
                }

                data.Enqueue(new Tuple<int, int, bool>(datax.RtuId, datax.LoopId, datax.IsOpen));
                OcCountAns++;
            }
            var tmp = DateTime.Now.Ticks - _opeTime;
            if (tmp > 0)
                TimeAns = (tmp/100000)*0.01;

            //    Remind = "开关灯数据正在反馈...";

        }

        private ConcurrentQueue<Tuple<int, int, bool>> data = new ConcurrentQueue<Tuple<int, int, bool>>();
        private delegate void CmbdelegateMbl();

        void Qtz1(object obj)
        {
            if (data.Count == 0) return;

            Application.Current.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal,
                new CmbdelegateMbl(Qtz123));
        }


        void Qtz123()
        {
            while (data.Count > 0)
            {
                Tuple<int, int, bool> datax;
                if (data.TryDequeue(out datax) == false) continue;
                //var info = data as RfData;
                //if (info == null) return;
                var rtuid = datax.Item1 ;
                var loopid = datax.Item2 ;
                var isOpen = datax.Item3 ;
                if (!TreeTmlNode.RegisterTmlNode.Keys.ToList().Contains(rtuid)) return;

                switch (loopid)
                {
                    case 1:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                                f.K1OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 2:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                                f.K2OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 3:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                                f.K3OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 4:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                                f.K4OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 5:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                                f.K5OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 6:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                                f.K6OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 7:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                                f.K7OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                    case 8:
                        if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                        {
                            foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                                f.K8OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                        }
                        break;
                }
            }
        }

        private void ResponseSndWeekSetK1K3(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isViewShow == false) return;
            var datax = infos.WstRtuOrders ;
            if (datax == null) return;
            if (datax.Op < 11 || datax.Op > 14) return;
            var lst = infos.Args.Addr;
            if (lst == null) return;
            foreach (var nodeid in TreeTmlNode.RegisterTmlNode .Keys.ToList())
            {
                if (lst.Contains(nodeid))
                {
                    foreach (var f in TreeTmlNode.RegisterTmlNode[nodeid])
                    {
                        if (datax.Op == 11)
                            f.WeekSndAns =
                                f.WeekSndAns == EnumWeekSndAns.K4K6Ans
                                    ? EnumWeekSndAns.AllAns
                                    : EnumWeekSndAns.K1K3Ans;
                        if (datax.Op == 12)
                            f.WeekSndAns =
                                f.WeekSndAns == EnumWeekSndAns.K1K3Ans
                                    ? EnumWeekSndAns.AllAns
                                    : EnumWeekSndAns.K4K6Ans;
                    }
                }
            }
            Remind = "发送周设置数据已返回！！！";
            var tmp = DateTime.Now.Ticks - _opeTime;
            if (tmp > 0)
                TimeAns = (tmp / 100000) * 0.01;
        }



       

        public void RtuParaUpdate40000(string session, Wlst.mobile.MsgWithMobile args)
        {
            if (_isViewShow == false) return;
            var datax = args.WstRtuOrders;
            if (datax.RtuIds.Count == 0) return;

            foreach (var item in TreeTmlNode.RegisterTmlNode.Where(item => item.Key == datax.RtuIds[0]))
            {
                foreach (var f in item.Value)
                {
                    if (datax.Op == 7)
                    {
                        f.State = EnumTmlState.Use;
                    }
                    else if (datax.Op == 6)
                    {
                        f.State = EnumTmlState.Disable;
                    }
                }
            }

            var tmp = DateTime.Now.Ticks - _opeTime;
            if (tmp > 0)
                TimeAns = (tmp / 100000) * 0.01;

        }

     
    }
}
