using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Services;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Views;
using Wlst.client;

namespace Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.ViewModel
{
    [Export(typeof(IIControlCenterManagDemo2))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ControlCenterViewModel2 : EventHandlerHelperExtendNotifyProperyChanged, IIControlCenterManagDemo2
    {
        private int _hxxx = 0;
        /// <summary>
        /// 前台界面绑定的图标大小
        /// </summary>
        public int Hightxx
        {
            get
            {
                if (_hxxx < 0.1)
                {
                    _hxxx = (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeadHeightt - 2;
                    if (_hxxx > 25) _hxxx = 25;
                    if (_hxxx < 12) _hxxx = 12;
                }
                return _hxxx;
            }
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get
            {
                if (File.Exists("Config\\suzhou.txt"))
                {
                    return "应急调度";
                }
                return "控制中心";
            }
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
        private string _btText;

        public string BtText
        {
            get { return _btText; }
            set
            {
                if (value == _btText) return;
                _btText = value;
                this.RaisePropertyChanged(() => this.BtText);
            }
        }

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

                if (value == 1)
                {
                    BtText = "按时间表控制";
                    LoadTreeNodeGlobal();
                }
                else if (value == 2)
                {
                    LoadTreeNodeLocal();
                }
                else if (value == 3)
                {
                    BtText = "全部终端筛选";
                }
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

        #region
        private Visibility _linshiVisi;
        /// <summary>
        /// 临时操作 终端数 显示
        /// </summary>
        public Visibility LinshiVisi
        {
            get { return _linshiVisi; }
            set
            {
                if (value == _linshiVisi) return;
                _linshiVisi = value;
                this.RaisePropertyChanged(() => this.LinshiVisi);
            }
        }


        #endregion

        public ControlCenterViewModel2()
        {
            //InitEvent();
            InitAction();
            Wlst.Cr.Coreb.AsyncTask .Qtz .AddQtz("ColNoAlarmTime",0,DateTime.Now.Ticks,1,ColNoAlarmTime);
          
        }

        public event EventHandler OnNavOnLoadSelectdRtus;

        public void NavInitBeforShow(params object[] parsObjects)
        {
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.IsControlCenterNoErr)
            {
                IsShowAlarmTime = Visibility.Hidden;
                IsShowAlarmTimeSet = Visibility.Visible;
            }
            else
            {
                IsShowAlarmTime = Visibility.Hidden;
                IsShowAlarmTimeSet = Visibility.Hidden;
            }
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            if (_isViewShow) return;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.ControlCenterIsShow = true;
            _isViewShow = true;
            _isSyncTime = false;
            _isZcTime = false;
            _isZcVer = false;

            NowSelected = new Tuple<List<Tuple<int, int>>, List<string>>(new List<Tuple<int, int>>(), new List<string>());

            GetNoLoadAlarmTime();
            _id = DateTime.Now.Ticks;
            _currentSelectAllState1 = false;
            _currentSelectAllStatek1k62 = false;
            _currentSelectAllStateTmp = false;

            OcCount = 0;
            OcCountAns = 0;
            OcTmlCount = 0;
            TimeAns = 0;
            TimeAnss = "";
            Remind = "";

            //citynum==1  为武汉特殊功能
            IsLnErr = Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum ==1;
            if (IsLnErr) GetEmergencyGroups();

            bool isUserSelectRtu = false;
            if (parsObjects.Count() == 0)
            {
                RdGbIsEnable = true;
                LinshiVisi = Visibility.Collapsed;
                RtuSumTemp = "";
                RdIndex = 1;
                //主菜单

                //  LoadTreeNodeGlobal();



            }
            else
            {
                RdGbIsEnable = false;
                LinshiVisi = Visibility.Visible;
                RtuSumTemp = "";
                RdIndex = 3;

                var rtus = parsObjects[0] as List<int>;
                if (rtus == null || rtus.Count == 0)
                {

                    var rtusLn = parsObjects[0] as Dictionary<int, List<int>>;
                    if (rtusLn == null || rtusLn.Count == 0)
                    {

                        RdGbIsEnable = true;
                        LinshiVisi = Visibility.Collapsed;
                        RtuSumTemp = "";
                        RdIndex = 1;
                    }
                    else
                    {
                        ////火零不平衡 lvf 2018年6月13日09:13:46
                        //IsLnErr = true;
                        RtuSumTemp = rtusLn.Count + "";
                        LoadTreeNodeTempByLn(rtusLn);
                        isUserSelectRtu = true;
                    }







                }
                else
                {
                    if (rtus.First() == -1)
                    {
                        ExFastControlSelect();
                    }
                    else
                    {
                        RtuSumTemp = rtus.Count + "";
                        LoadTreeNodeTemp(rtus);
                        isUserSelectRtu = true;


                    }
                }
            }
            if (OnNavOnLoadSelectdRtus != null)
            {
                OnNavOnLoadSelectdRtus(  isUserSelectRtu ? null : this ,EventArgs .Empty );
            }

        }



        public void OnUserHideOrClosing()
        {
          //  ZOrders.OpenCloseLight.OpenCloseLightDataDispatch.IsControlCenterManagDemo2TakeOverOcOrderShow = false;
            _isViewShow = false;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.ControlCenterIsShow = false;
            Items.Clear();
            TreeTmlNode.RegisterTmlNode.Clear();
            _dtTurnBack = DateTime.Now;
            ReportType = EnumReportTypes.NoReport;

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

        private bool _isSyncTime; //按下 对时按钮
        private bool _isZcTime; //按下  召测对时按钮
        private bool _isZcVer; //按下   召测版本按钮

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

        #region RtuSumTemp
        private string _rtuSumTemp;
        public string RtuSumTemp
        {
            get { return _rtuSumTemp; }
            set
            {
                if (_rtuSumTemp == value) return;
                _rtuSumTemp = value;
                RaisePropertyChanged(() => RtuSumTemp);
            }
        }

        #endregion

        #region SyncTime
        
        private string _syncTime;
        /// <summary>
        /// 对时按钮的文本
        /// </summary>
        public string SyncTime
        {
            get { return _syncTime; }
            set
            {
                if (_syncTime == value) return;
                _syncTime = value;
                RaisePropertyChanged(() => SyncTime);
            }
        }

        #endregion
        

        #region ZcTime
   
        private string _zcTime;
        /// <summary>
        /// 召测时间按钮文本
        /// </summary>
        public string ZcTime
        {
            get { return _zcTime; }
            set
            {
                if (_zcTime == value) return;
                _zcTime = value;
                RaisePropertyChanged(() => ZcTime);
            }
        }

        #endregion

        
        #region ZcTime
        private string _zcVer;
        /// <summary>
        /// 召测版本按钮文本
        /// </summary>
        public string ZcVer
        {
            get { return _zcVer; }
            set
            {
                if (_zcVer == value) return;
                _zcVer = value;
                RaisePropertyChanged(() => ZcVer);
            }
        }

        #endregion


        #region  Group




        private Visibility _txtgrpVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility GrpVisi
        {
            get { return _txtgrpVisi; }
            set
            {
                if (value != _txtgrpVisi)
                {
                    _txtgrpVisi = value;
                    this.RaisePropertyChanged(() => this.GrpVisi);
                }
            }
        }

        private static ObservableCollection<GroupInt> _grpdevices;

        public static ObservableCollection<GroupInt> GroupName
        {
            get
            {
                if (_grpdevices == null)
                {
                    _grpdevices = new ObservableCollection<GroupInt>();
                }
                return _grpdevices;
            }

        }

        public class GroupInt : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private GroupInt _grpcomboboxselected;
        private int GrpId;

        public GroupInt GroupComboBoxSelected
        {
            get { return _grpcomboboxselected; }
            set
            {
                if (_grpcomboboxselected != value)
                {
                    _grpcomboboxselected = value;
                    this.RaisePropertyChanged(() => this.GroupComboBoxSelected);
                    if (value == null) return;
                    GrpId = value.Key;

                    
                }
            }
        }


        public void GetEmergencyGroups()
        {
            GroupName.Clear();
            
            //if (AreaId == -1) //全部区域
            //{
            //    GrpVisi = Visibility.Collapsed;

            //}
            //else
            //{
            //    GrpVisi = Visibility.Visible;
            //    var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
            //    if (area == null) return;
            //    var grps =
            //        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(AreaId);
            //    GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
            //    if (grps.Count > 0)
            //    {
            //        var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
            //        foreach (var f in grpsTmp)
            //        {

            //            GroupName.Add(new GroupInt() { Value = f.GroupName, Key = f.GroupId });
            //        }
            //    }
            //    GroupComboBoxSelected = GroupName[0];
            //}
            var grpInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.AreaInfo;
            if (grpInfo.Count == 0) return;
            //GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
            foreach (var g in grpInfo)
            {
                string name = "应急" + g.Key + "级";
                GroupName.Add(new GroupInt() { Value = name, Key = g.Key });
            }
            //GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
            //GroupName.Add(new GroupInt() { Value = "积水1", Key = 1 });
            //GroupName.Add(new GroupInt() { Value = "积水2", Key = 2 });
            //GroupName.Add(new GroupInt() { Value = "积水3", Key = 3 });
            //GroupName.Add(new GroupInt() { Value = "积水4", Key = 4 });

            GroupComboBoxSelected = GroupName[0];
        }

        //public   GetEmergencyRtusByGrpId(int grpId)
        //{

        //    //if (grpId == -1) //全部区域
        //    //{
        //    //    GrpVisi = Visibility.Collapsed;

        //    //}
        //    //else
        //    //{
        //    //    GrpVisi = Visibility.Visible;
        //    //    var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
        //    //    if (area == null) return;
        //    //    var grps =
        //    //        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(AreaId);
        //    //    GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
        //    //    if (grps.Count > 0)
        //    //    {
        //    //        var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
        //    //        foreach (var f in grpsTmp)
        //    //        {

        //    //            GroupName.Add(new GroupInt() { Value = f.GroupName, Key = f.GroupId });
        //    //        }
        //    //    }
        //    //    GroupComboBoxSelected = GroupName[0];
        //    //}
        //    return  Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.GetEmeInfo(grpId);
        
        //}

        #endregion


        #region IsLnErr
        /// <summary>
        /// 如果是 火零不平衡导航过来的，呈现 “应急关灯”和“恢复开灯”
        /// </summary>
        private bool _isLnErr;
        public bool IsLnErr
        {
            get { return _isLnErr; }
            set
            {
                if (_isLnErr == value) return;
                _isLnErr = value;
                RaisePropertyChanged(() => IsLnErr);
            }
        }
        #endregion



        //#region OpLst


        //private List<int> _opLst;
        //public List<int> OpLst
        //{
        //    get { return _opLst; }
        //    set
        //    {
        //        if (_opLst == value) return;
        //        _opLst = value;
        //        RaisePropertyChanged(() => OpLst);
        //    }
        //}
        //#endregion

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

        //#region OcTmlCountAns


        //private int _remindOcTmlCountAns;
        //public int OcTmlCountAns
        //{
        //    get { return _remindOcTmlCountAns; }
        //    set
        //    {
        //        if (_remindOcTmlCountAns == value) return;
        //        _remindOcTmlCountAns = value;
        //        RaisePropertyChanged(() => OcTmlCountAns);
        //    }
        //}
        //#endregion

        private long _opeTime = 0;

        private List<int> _opLst = new List<int>();
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

        private string _remindOcCountOcCoIsShowOcCountAnsuntAnss;
        public string TimeAnss
        {
            get { return _remindOcCountOcCoIsShowOcCountAnsuntAnss; }
            set
            {
                if (_remindOcCountOcCoIsShowOcCountAnsuntAnss == value) return;
                _remindOcCountOcCoIsShowOcCountAnsuntAnss = value;
                RaisePropertyChanged(() => TimeAnss);
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


        #region 应急开灯报表数据

        private ObservableCollection<TreeNodeBase> _yjopenLightReport;
        public ObservableCollection<TreeNodeBase> YjOpenLightReport
        {
            get { return _yjopenLightReport ?? (_yjopenLightReport = new ObservableCollection<TreeNodeBase>()); }
        }
        #endregion

        #region 应急关灯报表数据

        private ObservableCollection<TreeNodeBase> _yjcloseLightReport;
        public ObservableCollection<TreeNodeBase> YjCloseLightReport
        {
            get { return _yjcloseLightReport ?? (_yjcloseLightReport = new ObservableCollection<TreeNodeBase>()); }
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

    


        #region 命令
        private ICommand _cmdCmdInMainWindow;
        public ICommand CmdInMainWindow
        {
            get { return _cmdCmdInMainWindow ?? (_cmdCmdInMainWindow = new RelayCommand<string>(ExCmdInMainWindow, CanCmdInMainWindow, true)); }
        }

        private bool  _currentSelectAllState1 = false;
        private bool _currentSelectAllStatek1k62 = false;
        private bool _currentSelectAllStateTmp = false;
        private int _lastOcOr = 0;


        private ConcurrentDictionary<Tuple<int, int>, bool> ExLoopCount =
    new ConcurrentDictionary<Tuple<int, int>, bool>();

        private ConcurrentDictionary<int, int> ExWeekSetCount =
            new ConcurrentDictionary<int, int>();

        private List<int> _rtusThatOpe = new List<int>();


        private void ExCmdInMainWindow(string datax)
        {
            //1、全选或全清
            //2、K1-K8全选或全清
            //3、重置
            //4、开灯
            //5、关灯
            //6、选测
            //7、对时     
            //8、发送周设置
            //20、查看操作结果
            //21、报表界面返回

            //9、 停运
            //10、解除停运

            //11.召测版本

            //12. 召测时间
            //13.  其他操作   lvf 在原对时界面增加 对时 召测时间 召测版本;  2018年3月27日15:26:12

            //14. 应急关灯   lvf 2018年6月13日09:03:32
            //15. 恢复关灯  lvf 2018年6月13日09:03:48
            //16. 取消应急  lvf 2018年7月5日16:27:02


            _opLst.Clear();
            int x = 0;
            if (Int32.TryParse(datax, out x) == false) return;
            if (x == 1)
            {
                _currentSelectAllState1 = !_currentSelectAllState1;
                foreach (var t in this.Items) t.IsChecked = _currentSelectAllState1;
                _lastOcOr = x;
                return;
            }
            if (x == 2)
            {
                _currentSelectAllStatek1k62 = !_currentSelectAllStatek1k62;
                foreach (var t in this.Items) t.IsSwitch0 = _currentSelectAllStatek1k62;
                _lastOcOr = x;
                return;
            }
            if (x == 3)
            {
                #region

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
                        g.WeekSndAns = "0/2";
                        g.K0SelectionTestAns = EnumSelectionTestAns.Ready;
                    }
                }


                _id = DateTime.Now.Ticks;
                _lastOcOr = x;
                OpenLightReport.Clear();
                CloseLightReport.Clear();
                SelectionTestReport.Clear();
                SyncTimeReport.Clear();
                WeekSndReport.Clear();

                YjCloseLightReport.Clear();
                YjOpenLightReport.Clear();
                
                return;

                #endregion
            }
            if (x == 4 || x == 5)
            {
                #region

                var data = new Wlst.client.OpenCloseOperatorCenter
                               {
                                   Open = x == 4 ? 1 : 2
                               };
                var k1Rtus = TreeTmlNode.GetNodeKxChecked(1, false);
                var k2Rtus = TreeTmlNode.GetNodeKxChecked(2, false);
                var k3Rtus = TreeTmlNode.GetNodeKxChecked(3, false);
                var k4Rtus = TreeTmlNode.GetNodeKxChecked(4, false);
                var k5Rtus = TreeTmlNode.GetNodeKxChecked(5, false);
                var k6Rtus = TreeTmlNode.GetNodeKxChecked(6, false);
                var k7Rtus = TreeTmlNode.GetNodeKxChecked(7, false);
                var k8Rtus = TreeTmlNode.GetNodeKxChecked(8, false);
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


                int xcount = 0;
                foreach (var f in data.Items) xcount += f.Rtus.Count;
                if (xcount == 0)
                {
                    UMessageBox.Show("请选择输出回路", "请选择需要操作的输出回路......", UMessageBoxButton.Ok);
                    return;
                }


                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                {
                    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                    if (sss == UMessageBoxWantPassWord.CancelReturn)
                    {
                        return;
                    }
                    if (sss != UserInfo.UserLoginInfo.UserPassword)
                    {
                        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                         UMessageBoxButton.Yes);
                        return;
                    }
                }
                else
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

                if (_lastOcOr != x)
                {
                    var str = "开灯";
                    if (x == 5) str = "关灯";
                    if (
                        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                            "当前正在进行" + str + "操作，确定现在进行" + str + "操作吗？", WlstMessageBoxType.YesNo) ==
                        WlstMessageBoxResults.Yes)
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
                _lastOcOr = x;


                //通知 开关灯显示区域 取消显示开关灯命令 由控制中心接管显示
                //  ZOrders.OpenCloseLight.OpenCloseLightDataDispatch.IsControlCenterManagDemo2TakeOverOcOrderShow = true;


                //OcCount = 0;
                _opeTime = DateTime.Now.Ticks;
                TimeAns = 0;
                TimeAnss = "";
                OcCountAns = 0;
                OcTmlCount = 0;
                int xtmp = 0;

                ExLoopCount.Clear();
                var nodes = TreeTmlNode.GetNodeChecked(false);
                OcTmlCount = nodes.Count;
                foreach (var f in data.Items)
                {
                    foreach (var g in f.Rtus)
                    {
                        ExLoopCount.TryAdd(new Tuple<int, int>(g, f.LoopId), false); //todo
                    }

                    xtmp += f.Rtus.Count;
                }
                OcCount = xtmp;

                data.Op = 2;
                data.ItemsShield.Add(new OpenCloseOperatorCenter.ShieldTimeItem()
                                         {
                                             MinutesforShield = NoAlarmTime,
                                             RtusShield = new List<int>()
                                         });
                foreach (var t in data.Items)
                {
                    foreach (var tt in t.Rtus)
                    {
                        if (!data.ItemsShield[0].RtusShield.Contains(tt))
                        {
                            data.ItemsShield[0].RtusShield.Add(tt);
                        }
                    }
                }

                var info = Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center;
                info.WstRtuCntOrderOpenCloseCenter = data;
                SndOrderServer.OrderSnd(info, 10, 6);


                if (x == 4)
                {
                    _dtOpenLight = DateTime.Now;
                    CurrtReportType = EnumReportTypes.OpenLightReport;
                    Remind = "开灯命令已发出....";
                    ExSearchReport();
                }
                else
                {
                    _dtCloseLight = DateTime.Now;
                    CurrtReportType = EnumReportTypes.CloseLightReport;
                    Remind = "关灯命令已发出....";
                    ExSearchReport();
                }

                var tmp = new Dictionary<string, string>();
                tmp.Add("database", NoAlarmTime.ToString());
                Elysium.ThemesSet.Common.ReadSave.Save(tmp, "NoAlarmTime");

                #endregion

                return;
            }
            if (x == 6)
            {
                #region

                _dtSelectTest6 = DateTime.Now;
                OcTmlCount = 0;
                //  OcTmlCountAns = 0;
                var nodes = TreeTmlNode.GetNodeChecked(true);

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
                if (nodes.Count < 1)
                {
                    UMessageBox.Show("请选择终端", "请选择需要操作的终端......", UMessageBoxButton.Ok);
                    return;
                }
                _rtusThatOpe.Clear();
                _rtusThatOpe.AddRange(nodes);

                OcTmlCount = nodes.Count;
                _opLst.AddRange(nodes);

                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(nodes);
                info.WstRtuOrders.RtuIds.AddRange(nodes);
                info.WstRtuOrders.Op = 31;
                SndOrderServer.OrderSnd(info, 10, 6);
                CurrtReportType = EnumReportTypes.SelectionTestReport;
                Remind = "批量选测命令已经发送...";

                _lastOcOr = x;
                _opeTime = DateTime.Now.Ticks;
                TimeAns = 0;
                TimeAnss = "";
                OcCountAns = 0;

                OcCount = nodes.Count;

                ExSearchReport();

                #endregion

                return;

            }
            if (x == 7)
            {
                #region

                // 改为 跳转界面   呈现  对时/召测时间/召测版本 界面 lvf 2018年3月28日10:58:41

                //_dtAsynTime7 = DateTime.Now;
                OcTmlCount = 0;
                //OcTmlCountAns = 0;
                IsShowSyncTime = true;
                var lstRtu = TreeTmlNode.GetNodeChecked(false);
                if (lstRtu.Count < 1)
                {
                    UMessageBox.Show("请选择终端", "请选择需要操作的终端......", UMessageBoxButton.Ok);
                    return;
                }
                // from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value;
                OcTmlCount = lstRtu.Count;
                foreach (var t in TreeTmlNode.RegisterTmlNode.Values)
                {
                    foreach (var g in t)
                        g.SyncTimeAns = false;
                }
                _lastOcOr = x;
                _rtusThatOpe.Clear();
                _rtusThatOpe.AddRange(lstRtu);

                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(lstRtu);
                info.WstRtuOrders.RtuIds.AddRange(lstRtu);
                info.WstRtuOrders.Op = 21;
                info.WstRtuOrders.DtTime = DateTime.Now.Ticks;
                SndOrderServer.OrderSnd(info, 10, 6);
                CurrtReportType = EnumReportTypes.AsyncTimeReport;
                Remind = "对时命令已发出！！！";

                _opeTime = DateTime.Now.Ticks;
                TimeAns = 0;
                TimeAnss = "";
                OcCountAns = 0;
                OcTmlCount = 0;
                OcCount = lstRtu.Count;

                ExSearchReport();

                #endregion

                return;
            }
            if (x == 8)
            {
                #region

                _dtSndWeekSet8 = DateTime.Now;
                IsShowWeekSnd = true;
                OcTmlCount = 0;
                //OcTmlCountAns = 0;
                var lstRtu = TreeTmlNode.GetNodeChecked(false);
                //( from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value).ToList();
                if (lstRtu.Count < 1)
                {
                    UMessageBox.Show("请选择终端", "请选择需要操作的终端......", UMessageBoxButton.Ok);
                    return;
                }

                foreach (var ts in TreeTmlNode.RegisterTmlNode.Values)
                {
                    foreach (var t in ts)
                        t.WeekSndAns = "0/2";
                }
                ExWeekSetCount.Clear();
                foreach (var f in lstRtu) if (ExWeekSetCount.ContainsKey(f) == false) ExWeekSetCount.TryAdd(f, 0);

                _lastOcOr = x;
                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
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
                TimeAnss = "";
                OcCountAns = 0;
                OcTmlCount = 0;
                OcCount = lstRtu.Count;

                ExSearchReport();

                #endregion

                return;
            }
            if (x == 20)
            {
                #region

                _dtSearchReport20 = DateTime.Now;
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
                    case EnumReportTypes.YjOpenLightReport:
                        LoadYjOpenLightReport();
                        break;
                    case EnumReportTypes.YjCloseLightReport:
                        LoadYjCloseLightReport();
                        break;

                }

                #endregion

                return;

            }
            if (x == 21)
            {
                Remind = "";
                _dtTurnBack = DateTime.Now;
                ReportType = EnumReportTypes.NoReport;

                if (DateTime.Now.Ticks < NoAlarmTimeEnd) IsShowAlarmTime = Visibility.Visible;
                else IsShowAlarmTime = Visibility.Hidden;
                IsShowAlarmTimeSet = Visibility.Visible;


                SyncTime = "对时";
                ZcTime = "召测时间";
                ZcVer = "召测版本";
                _isSyncTime = false;
                _isZcTime = false;
                _isZcVer = false;
                OcCountAns = 0;
                SyncTimeReport.Clear();
                return;
            }
            if (x == 9)
            {
                var lstRtu = TreeTmlNode.GetNodeChecked(false);
                if (lstRtu.Count < 1)
                {
                    UMessageBox.Show("请选择终端", "请选择需要操作的终端......", UMessageBoxButton.Ok);
                    return;
                }

                RegionManage.ShowViewByIdAttachRegionWithArgu(
                    Wlst.Ux.WJ3005Module.Services.ViewIdAssign.NavToBatchStopView, lstRtu,0);
                return;
            }
            if (x == 10)
            {
                var lstRtu = TreeTmlNode.GetNodeChecked(false);
                if (lstRtu.Count < 1)
                {
                    UMessageBox.Show("请选择终端", "请选择需要操作的终端......", UMessageBoxButton.Ok);
                    return;
                }
                RegionManage.ShowViewByIdAttachRegionWithArgu(
                    Wlst.Ux.WJ3005Module.Services.ViewIdAssign.NavToBatchStopView, lstRtu,1);
                return;
            }
            if (x == 13) //跳转其他界面 ,原对时界面  lvf 2018年3月27日15:28:49
            {
                #region

                SyncTime = "对时";
                ZcTime = "召测时间";
                ZcVer = "召测版本";
                OcTmlCount = 0;
                //OcTmlCountAns = 0;
                IsShowSyncTime = true;
                var lstRtu = TreeTmlNode.GetNodeChecked(false);
                if (lstRtu.Count < 1)
                {
                    UMessageBox.Show("请选择终端", "请选择需要操作的终端......", UMessageBoxButton.Ok);
                    return;
                }
                // from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value;
                OcTmlCount = lstRtu.Count;
                OcCount = lstRtu.Count;
                foreach (var t in TreeTmlNode.RegisterTmlNode.Values)
                {
                    foreach (var g in t)
                    {
                        g.SyncTimeAns = false;
                        g.ZcTimeAns = "---";
                        g.ZcVerAns = "---";
                    }


                }


                _lastOcOr = x;
                _rtusThatOpe.Clear();
                _rtusThatOpe.AddRange(lstRtu);


                CurrtReportType = EnumReportTypes.AsyncTimeReport;


                ExSearchReport();

                #endregion

                return;
            }
            if (x == 14 || x == 15 || x==16) // 火零不平衡  14 关灯  15 开灯  16取消应急
            {
                //#region

                //var data = new Wlst.client.TimeTableEmergenceOper
                //               {
                //                   Op  = x == 14 ? 1 : 2
                //               };
                //if (x == 16) data.Op = 5;

                //var k1Rtus = TreeTmlNode.GetNodeKxChecked(1, false);
                //var k2Rtus = TreeTmlNode.GetNodeKxChecked(2, false);
                //var k3Rtus = TreeTmlNode.GetNodeKxChecked(3, false);
                //var k4Rtus = TreeTmlNode.GetNodeKxChecked(4, false);
                //var k5Rtus = TreeTmlNode.GetNodeKxChecked(5, false);
                //var k6Rtus = TreeTmlNode.GetNodeKxChecked(6, false);
                //var k7Rtus = TreeTmlNode.GetNodeKxChecked(7, false);
                //var k8Rtus = TreeTmlNode.GetNodeKxChecked(8, false);
                //if (k1Rtus.Count > 0)
                //    data.RtuInfoItems.Add(new TimeTableEmergenceOper.RtuList() { LoopId = 1, RtuId = k1Rtus });
                //if (k2Rtus.Count > 0)
                //    data.RtuInfoItems.Add(new TimeTableEmergenceOper.RtuList() { LoopId = 2, RtuId = k2Rtus });
                //if (k3Rtus.Count > 0)
                //    data.RtuInfoItems.Add(new TimeTableEmergenceOper.RtuList() { LoopId = 3, RtuId = k3Rtus });
                //if (k4Rtus.Count > 0)
                //    data.RtuInfoItems.Add(new TimeTableEmergenceOper.RtuList() { LoopId = 4, RtuId = k4Rtus });
                //if (k5Rtus.Count > 0)
                //    data.RtuInfoItems.Add(new TimeTableEmergenceOper.RtuList() { LoopId = 5, RtuId = k5Rtus });
                //if (k6Rtus.Count > 0)
                //    data.RtuInfoItems.Add(new TimeTableEmergenceOper.RtuList() { LoopId = 6, RtuId = k6Rtus });
                //if (k7Rtus.Count > 0)
                //    data.RtuInfoItems.Add(new TimeTableEmergenceOper.RtuList() { LoopId = 7, RtuId = k7Rtus });
                //if (k8Rtus.Count > 0)
                //    data.RtuInfoItems.Add(new TimeTableEmergenceOper.RtuList() { LoopId = 8, RtuId = k8Rtus });


                //int xcount = 0;
                //foreach (var f in data.RtuInfoItems) xcount += f.RtuId.Count;
                //if (xcount == 0)
                //{
                //    UMessageBox.Show("请选择输出回路", "请选择需要操作的输出回路......", UMessageBoxButton.Ok);
                //    return;
                //}


                //if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                //{
                //    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                //    if (sss == UMessageBoxWantPassWord.CancelReturn)
                //    {
                //        return;
                //    }
                //    if (sss != UserInfo.UserLoginInfo.UserPassword)
                //    {
                //        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                //                         UMessageBoxButton.Yes);
                //        return;
                //    }
                //}
                //else
                //{
                //    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行应急开关灯操作，\r\n若确定请输入验证码:1234", "");
                //    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                //    {
                //        return;
                //    }

                //    if (sss != "1234")
                //    {
                //        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                //        return;
                //    }
                //}

                //if (_lastOcOr != x)
                //{
                //    var str = "恢复开灯";
                //    if (x == 14) str = "应急关灯";
                //    if (x == 16) str = "取消应急关灯";
                //    if (
                //        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                //            "当前正在进行" + str + "操作，确定现在进行" + str + "操作吗？", WlstMessageBoxType.YesNo) ==
                //        WlstMessageBoxResults.Yes)
                //    {
                //        //清除关灯应答数据
                //        foreach (var items in TreeTmlNode.RegisterTmlNode)
                //        {
                //            foreach (var item in items.Value)
                //            {
                //                item.K1OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                //                item.K2OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                //                item.K3OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                //                item.K4OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                //                item.K5OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                //                item.K6OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                //                item.K7OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                //                item.K8OpenOrCloseAns = EnumOpenOrCloseAns.NoAnswer;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        return;
                //    }

                //}
                //_lastOcOr = x;


                ////通知 开关灯显示区域 取消显示开关灯命令 由控制中心接管显示
                ////  ZOrders.OpenCloseLight.OpenCloseLightDataDispatch.IsControlCenterManagDemo2TakeOverOcOrderShow = true;


                ////OcCount = 0;
                //_opeTime = DateTime.Now.Ticks;
                //TimeAns = 0;
                //TimeAnss = "";
                //OcCountAns = 0;
                //OcTmlCount = 0;
                //int xtmp = 0;

                //ExLoopCount.Clear();
                //ExWeekSetCount.Clear();
                //var nodes = TreeTmlNode.GetNodeChecked(false);

                
                //OcTmlCount = nodes.Count;
                //foreach (var f in data.RtuInfoItems)
                //{
                //    foreach (var g in f.RtuId)
                //    {
                //        ExLoopCount.TryAdd(new Tuple<int, int>(g, f.LoopId), false); //todo
                //    }

                //    xtmp += f.RtuId.Count;
                //}
                //foreach (var f in nodes) if (ExWeekSetCount.ContainsKey(f) == false) ExWeekSetCount.TryAdd(f, 0);
                //OcCount = xtmp;

                //var shieldTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3302, 3, 24); //生效时间 默认为24小时
                //var suninfo = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month,
                //                                                                                DateTime.Now.Day);
           
                //int sunriseHour =Convert.ToInt16(suninfo.time_sunrise/60) ;
                //int sunriseMin = Convert.ToInt16(suninfo.time_sunrise%60);
                //var sunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunriseHour,
                //                               sunriseMin,0);
                ////判断点击时间 是否大于今天日出时间
                //if (DateTime.Now.CompareTo(sunriseTime) < 0)
                //{
                //    var dtyesterday = DateTime.Now.AddDays(-1);
                //    var dtstart = new DateTime(dtyesterday.Year, dtyesterday.Month, dtyesterday.Day, 12, 0, 1);
                //    data.DtStartTime = dtstart.Ticks;
                //    data.DtEndTime = dtstart.AddHours(shieldTime).Ticks;
                //}
                //else
                //{
                   
                //    var dts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 1);
                //    data.DtStartTime = dts.Ticks;
                //    data.DtEndTime = dts.AddHours(shieldTime).Ticks;
                //}
       
                //var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;
                //info.WstRtutimeTimeTableEmerg= data ;      
                //SndOrderServer.OrderSnd(info, 10, 6);



                //if (x == 14)
                //{
                //    _dtCloseLight = DateTime.Now;
                //    CurrtReportType = EnumReportTypes.YjCloseLightReport;
                //    var dt = new DateTime(data.DtStartTime).ToString("yyyy-MM-dd HH:mm:ss");
                //    var dtt = new DateTime(data.DtEndTime).ToString("yyyy-MM-dd HH:mm:ss");
                //    Remind = "应急关灯命令已发出....屏蔽时间为："+ dt+" 至 "+dtt;
                //    ExSearchReport();
                //}
                //else if(x ==15)
                //{
                //    _dtOpenLight = DateTime.Now;
                //    CurrtReportType = EnumReportTypes.YjOpenLightReport;
                //    Remind = "恢复关灯命令已发出....";
                //    ExSearchReport();
                //}
                //else if(x ==16)
                //{
                //    _dtOpenLight = DateTime.Now;
                //    CurrtReportType = EnumReportTypes.YjOpenLightReport;
                //    Remind = "取消应急命令已发出....";
                //    ExSearchReport();
                //}

                //var tmp = new Dictionary<string, string>();
                //tmp.Add("database", NoAlarmTime.ToString());
                //Elysium.ThemesSet.Common.ReadSave.Save(tmp, "NoAlarmTime");


                //return;

                //#endregion




            }
        }

        private DateTime _dtReset3;
        private DateTime _dtOpenLight4;
        private DateTime _dtCloseLight5;
        private DateTime _dtSelectTest6;
        private DateTime _dtAsynTime7;
        private DateTime _dtSndWeekSet8;
        private DateTime _dtSearchReport20;
        private DateTime _dtStopRunning;
        private DateTime _dtSetRunning;
        private DateTime _dtZcTime;
        private DateTime _dtZcVer;

        private bool CanCmdInMainWindow(string data)
        {
            //1、全选或全清
            //2、K1-K8全选或全清
            //3、重置
            //4、开灯
            //5、关灯
            //6、选测
            //7、对时
            //8、发送周设置
            //20、查看操作结果
            //13. 跳转其他操作界面  对时 召测时间 召测版本
            int x = 0;
            if (Int32.TryParse(data, out x) == false) return false ;
            if (x < 3) return true;
            if (x == 3) return DateTime.Now.Ticks - _dtReset3.Ticks > 30000000;
            if (x == 4) return DateTime.Now.Ticks - _dtOpenLight4.Ticks > 30000000;
            if (x == 5) return DateTime.Now.Ticks - _dtCloseLight5.Ticks > 30000000;
            if (x == 6) return DateTime.Now.Ticks - _dtSelectTest6.Ticks > 30000000;
            if (x == 7) return DateTime.Now.Ticks - _dtAsynTime7.Ticks > 30000000;
            if (x == 8) return DateTime.Now.Ticks - _dtSndWeekSet8.Ticks > 30000000;
            if (x == 20) return DateTime.Now.Ticks - _dtSearchReport20.Ticks > 30000000;
            if (x == 21) return true;
            if (x == 9) return DateTime.Now.Ticks - _dtStopRunning.Ticks > 30000000;
            if (x == 10) return DateTime.Now.Ticks - _dtSetRunning.Ticks > 30000000;
            //if (x == 11) return DateTime.Now.Ticks - _dtSetRunning.Ticks > 30000000;
            //if (x == 12) return DateTime.Now.Ticks - _dtSetRunning.Ticks > 30000000;
            if (x == 13) return DateTime.Now.Ticks - _dtSetRunning.Ticks > 30000000;
            if (x == 14) return DateTime.Now.Ticks - _dtCloseLight.Ticks > 30000000;
            if (x == 15) return DateTime.Now.Ticks - _dtOpenLight.Ticks > 30000000;
            if (x == 16) return DateTime.Now.Ticks - _dtOpenLight.Ticks > 30000000;
            return false ;
        }




        #region CmdExport
        private DateTime _dtCmdExport;
        private ICommand _cmdCmdExport;

        public ICommand CmdExport
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, false);
                return _cmdCmdExport;
            }
        }

        private void ExCmdExport()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("终端地址");
                lsttitle.Add("物理地址");
                lsttitle.Add("终端名称");
                lsttitle.Add("终端状态");
                lsttitle.Add("对时成功");
                lsttitle.Add("召测时间");
                lsttitle.Add("召测版本");
                
                var lstobj = new List<List<object>>();
                int index =1;
                foreach (var g in SyncTimeReport)
                {

                    var tmp = new List<object>();
                    tmp.Add(index);
                    tmp.Add(g.NodeId);
                    tmp.Add(g.PhysicalId);
                    tmp.Add(g.NodeName);
                    tmp.Add(g.State);
                    tmp.Add(g.SyncTimeAns?"成功":"失败");
                    tmp.Add(g.ZcTimeAns);
                    tmp.Add(g.ZcVerAns);
                    

                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            if (SyncTimeReport.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion


        #region CmdAddEmergencyRtus
        private DateTime _dtCmdAddEmergencyRtus;
        private ICommand _cmdAddEmergencyRtus;

        public ICommand CmdAddEmergencyRtus
        {
            get
            {
                if (_cmdAddEmergencyRtus == null)
                    _cmdAddEmergencyRtus = new RelayCommand(ExCmdAddEmergencyRtus, CanExCmdAddEmergencyRtus, false);
                return _cmdAddEmergencyRtus;
            }
        }

        private void ExCmdAddEmergencyRtus()
        {
            _dtCmdAddEmergencyRtus = DateTime.Now;
            try
            {
                var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.GetEmeInfo(GrpId);
           
                AddTreeNodeTempByLn(rtulst);





            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("添加终端出错:" + ex);
            }

        }

        private bool CanExCmdAddEmergencyRtus()
        {
            if (GroupComboBoxSelected == null) return false;
            return DateTime.Now.Ticks - _dtCmdAddEmergencyRtus.Ticks > 30000000;
            return false;
        }

        #endregion

        #endregion



        #region 命令 补
        private ICommand _cmdCmdInChildWindow;
        public ICommand CmdInChildWindow
        {
            get { return _cmdCmdInChildWindow ?? (_cmdCmdInChildWindow = new RelayCommand<string>(ExCmdInChildWindow, CanExCmdInChildWindow, true)); }
        }


        private void ExCmdInChildWindow(string datax)
        {

            //4、开灯
            //5、关灯
            //6、选测
            //7、对时   lvf
            //8、发送周设置
            //9. 召测时间  lvf
            //10. 召测版本  lvf

            //14. 应急关灯
            //15. 恢复开灯
            //16. 取消应急关灯


            //OcCountAns = 0;
            int x = 0;
            if (Int32.TryParse(datax, out x) == false) return;


            if (x == 4 || x == 5)
            {
                #region

                if (_lastOcOr != x)
                {
                    var str = "开灯";
                    var opstr = "关灯";
                    if (x == 5)
                    {
                        str = "关灯";
                        opstr = "开灯";
                    }


                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "当前正在进行" + opstr + "操作，无法进行" + str + "操作..", WlstMessageBoxType.Ok);
                    return;

                }


                var data = new Wlst.client.OpenCloseOperatorCenter
                               {
                                   Open = x == 4 ? 1 : 2
                               };
                int allcount = 0;
                for (int i = 1; i < 17; i++)
                {
                    var rtus = (from t in ExLoopCount.Keys where t.Item2 == i select t.Item1).ToList();
                    allcount += rtus.Count;
                    if (rtus.Count > 0)
                    {
                        data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem()
                                           {LoopId = i, Rtus = rtus});
                    }
                }
                if (allcount == 0)
                {
                    Remind = "所有操作都已经成功，无须执行补操作...";
                    return;
                }

                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                {
                    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                    if (sss == UMessageBoxWantPassWord.CancelReturn)
                    {
                        return;
                    }
                    if (sss != UserInfo.UserLoginInfo.UserPassword)
                    {
                        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                         UMessageBoxButton.Yes);
                        return;
                    }
                }
                else
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



                var info = Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center;
                info.WstRtuCntOrderOpenCloseCenter = data;
                SndOrderServer.OrderSnd(info, 10, 6);


                if (x == 4)
                {
                    _dt1OpenLight4 = DateTime.Now;
                    Remind = "补开开命令已发出....";
                    ExSearchReport();
                }
                else
                {
                    _dt1CloseLight5 = DateTime.Now;
                    Remind = "补关灯命令已发出....";
                }

                #endregion

                return;
            }
            if (x == 6)
            {
                #region

                
              
                if (_rtusThatOpe.Count == 0)
                {
                    Remind = "选测终端全部应答,无须再补测发...";
                    return;
                }
 
                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(_rtusThatOpe);
                info.WstRtuOrders.RtuIds.AddRange(_rtusThatOpe);
                info.WstRtuOrders.Op = 31;
                SndOrderServer.OrderSnd(info, 10, 6);
               
                Remind = "批量选测命令已经发送...";
                _dt1SelectTest6 = DateTime.Now;

                #endregion

                return;

            }
            if (x == 7)
            {
                #region

                _isSyncTime = true;// 激活对时,接受对时应答事件
                //if (_rtusThatOpe.Count == 0)
                //{
                //    Remind = "终端对时全部应答,无须再补发...";
                //    return;
                //}

                List<int> rtulst = new List<int>();
                foreach (var g in SyncTimeReport)
                {
                    if (g.SyncTimeAns == false)
                    {
                        rtulst.Add(g.NodeId);
                    }
                }
                if (rtulst.Count == 0)
                {
                    Remind = "终端对时全部应答,无须再补发...";
                    return;
                }
                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(rtulst);
                info.WstRtuOrders.RtuIds.AddRange(rtulst);
                info.WstRtuOrders.Op = 21;
                info.WstRtuOrders.DtTime = DateTime.Now.Ticks;
                SndOrderServer.OrderSnd(info, 10, 6);

    
                Remind = "对时命令已发出！！！";
                _dtAsynTime7 = DateTime.Now;
                #endregion

                return;
            }
            if (x == 8)
            {
                #region



                if (ExWeekSetCount.Count == 0)
                {
                    Remind = "终端周设置全部应答,无须再补发...";
                    return;
                }

                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(ExWeekSetCount.Keys .ToList() );
                info.WstRtuOrders.RtuIds.AddRange(ExWeekSetCount.Keys.ToList());
                info.WstRtuOrders.DtTime = DateTime.Now.Ticks;
                info.WstRtuOrders.Op = 11;
                SndOrderServer.OrderSnd(info, 10, 6);
    
                Remind = "补发周设置命令已发出...";
                _dt1SndWeekSet8 = DateTime.Now;
                #endregion

                return;
            }
            if (x == 9) //召测时间
            {
                #region
                _isZcTime = true;// 激活召测时间,接受召测时间应答事件

                //if (_rtusThatOpe.Count == 0)
                //{
                //    Remind = "终端召测版本全部应答,无须再补发...";
                //    return;
                //}

                List<int> rtulst = new List<int>();
                foreach (var g in SyncTimeReport)
                {
                    if (g.ZcTimeAns == "---")
                    {
                        rtulst.Add(g.NodeId);
                    }
                }

                if (rtulst.Count == 0)
                {
                    Remind = "终端召测版本全部应答,无须再补发...";
                    return;
                }
                var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
                info.Args.Addr.AddRange(rtulst);
                info.WstRtuOrders.RtuIds.AddRange(rtulst);
                info.WstRtuOrders.Op = 22;
                info.WstRtuOrders.DtTime = DateTime.Now.Ticks;
                SndOrderServer.OrderSnd(info, 10, 6);


                Remind = "召测时间命令已发出！！！";
                _dtZcTime = DateTime.Now;
                #endregion

                return;
            }
            if (x == 10) //召测版本
            {
                #region
                _isZcVer = true;// 激活召测版本,接受召测版本应答事件



                List<int> rtulst = new List<int>();
                foreach (var g in SyncTimeReport)
                {
                    if (g.ZcVerAns == "---")
                    {
                        rtulst.Add(g.NodeId);
                    }
                }


                if (rtulst.Count == 0)
                {
                    Remind = "召测版本全部应答,无须再补发...";
                    return;
                }

                var nt = Wlst.Sr.ProtocolPhone.LxRtu.wst_zc_rtu_info;
                nt.Args.Addr.AddRange(rtulst);
                nt.WstRtuZcInfo.Op = 33;
                nt.WstRtuZcInfo.RtuId = 0;
                SndOrderServer.OrderSnd(nt, 10, 6);



                Remind = "召测版本命令已发出！！！";
                _dtZcVer = DateTime.Now;
                #endregion

                return;
            }
            if (x == 14 || x == 15 || x==16) // 14应急关灯  15恢复开灯 16 取消应急
            {
                //#region


                //if (_lastOcOr != x)
                //{
                //    var str = "";
                //    var opstr = "";
                //    if (x == 14)
                //    {
                //        str = "应急关灯";
                //        opstr = "恢复开灯";
                //    }
                //    if (x == 15)
                //    {
                //        str = "恢复开灯";
                //        opstr = "应急关灯";
                //    }
                //    if (x == 16)
                //    {
                //        str = "取消应急";
                //        opstr = "其他操作";
                //    }

                //    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                //        "当前正在进行" + opstr + "操作，无法进行" + str + "操作..", WlstMessageBoxType.Ok);
                //    return;

                //}


                //var data = new Wlst.client.TimeTableEmergenceOper
                //               {

                //                   Op = x == 14 ? 1 : 2
                //               };
                ////添加取消应急
                //if (x == 16) data.Op = 5;
                //int allcount = 0;
                //for (int i = 1; i < 17; i++)
                //{
                //    var rtus = (from t in ExLoopCount.Keys where t.Item2 == i select t.Item1).ToList();
                //    allcount += rtus.Count;
                //    if (rtus.Count > 0)
                //    {
                //        data.RtuInfoItems.Add(new TimeTableEmergenceOper.RtuList() {LoopId = i, RtuId = rtus});
                //    }
                //}
                //if (allcount == 0)
                //{
                //    Remind = "所有操作都已经成功，无须执行补操作...";
                //    return;
                //}

                //if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                //{
                //    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                //    if (sss == UMessageBoxWantPassWord.CancelReturn)
                //    {
                //        return;
                //    }
                //    if (sss != UserInfo.UserLoginInfo.UserPassword)
                //    {
                //        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                //                         UMessageBoxButton.Yes);
                //        return;
                //    }
                //}
                //else
                //{
                //    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行开关灯操作，\r\n若确定请输入验证码:1234", "");
                //    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                //    {
                //        return;
                //    }

                //    if (sss != "1234")
                //    {
                //        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                //        return;
                //    }
                //}


                //var shieldTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3302, 3, 24);
                //var suninfo =
                //    Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month,
                //                                                                               DateTime.Now.Day);

                //int sunriseHour = Convert.ToInt16(suninfo.time_sunrise/60);
                //int sunriseMin = Convert.ToInt16(suninfo.time_sunrise%60);
                //var sunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunriseHour,
                //                               sunriseMin, 0);
                ////判断点击时间 是否大于今天日出时间
                //if (DateTime.Now.CompareTo(sunriseTime) < 0)
                //{
                //    var dtyesterday = DateTime.Now.AddDays(-1);
                //    var dtstart = new DateTime(dtyesterday.Year, dtyesterday.Month, dtyesterday.Day, 12, 0, 1);
                //    data.DtStartTime = dtstart.Ticks;
                //    data.DtEndTime = dtstart.AddHours(shieldTime).Ticks;
                //}
                //else
                //{

                //    var dts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 1);
                //    data.DtStartTime = dts.Ticks;
                //    data.DtEndTime = dts.AddHours(shieldTime).Ticks;
                //}

                //var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;
                //info.WstRtutimeTimeTableEmerg = data;
                //SndOrderServer.OrderSnd(info, 10, 6);


                //if (x == 14)
                //{
                //    _dt1OpenLight4 = DateTime.Now;
                //    var dt = new DateTime(data.DtStartTime).ToString("yyyy-MM-dd HH:mm:ss");
                //    var dtt = new DateTime(data.DtEndTime).ToString("yyyy-MM-dd HH:mm:ss");
                //    Remind = "补应急关灯命令已发出....屏蔽时间为：" + dt + " 至 " + dtt;
                //    ExSearchReport();
                //}
                //else if (x == 15)
                //{
                //    _dt1CloseLight5 = DateTime.Now;
                //    Remind = "补恢复开灯命令已发出....";
                //}
                //else if (x == 16)
                //{
                //    Remind = "补取消应急命令已发出....";
                //}

                //#endregion

                return;
            }
        }


        private DateTime _dt1OpenLight4;
        private DateTime _dt1CloseLight5;
        private DateTime _dt1SelectTest6;
        private DateTime _dt1AsynTime7;
        private DateTime _dt1SndWeekSet8;

        private bool CanExCmdInChildWindow(string data)
        {

            //4、开灯
            //5、关灯
            //6、选测
            //7、对时   lvf
            //8、发送周设置
            //9. 召测时间  lvf
            //10. 召测版本  lvf

            //14 15应急关灯 恢复开灯

            return true;
            int x = 0;
            if (Int32.TryParse(data, out x) == false) return false;//return false;
            if (x == 4 || x == 5)
            {
                if (ExLoopCount.Count == 0) return false;
            }
            else if (x == 8)
            {
                if (ExWeekSetCount.Count == 0) return false;
            }
            else
            {
                if (_rtusThatOpe.Count == 0) return false;
            }


            if (x == 4)
                return DateTime.Now.Ticks - _dtOpenLight4.Ticks > 100000000 &&
                       DateTime.Now.Ticks - _dt1OpenLight4.Ticks > 100000000;
            if (x == 5)
                return DateTime.Now.Ticks - _dtCloseLight5.Ticks > 100000000 &&
                       DateTime.Now.Ticks - _dt1CloseLight5.Ticks > 100000000;
            if (x == 6)
                return DateTime.Now.Ticks - _dtSelectTest6.Ticks > 100000000 &&
                       DateTime.Now.Ticks - _dt1SelectTest6.Ticks > 100000000;
            if (x == 7)
                return DateTime.Now.Ticks - _dtAsynTime7.Ticks > 100000000 ;
                    //&&
                    //   DateTime.Now.Ticks - _dt1AsynTime7.Ticks > 100000000;
            if (x == 8)
                return DateTime.Now.Ticks - _dtSndWeekSet8.Ticks > 100000000 &&
                       DateTime.Now.Ticks - _dt1SndWeekSet8.Ticks > 100000000;
            if (x == 9)
                return DateTime.Now.Ticks - _dtZcTime.Ticks > 100000000; 
                    //&&
                    //   DateTime.Now.Ticks - _dt1SndWeekSet8.Ticks > 100000000;
            if (x == 10)
                return DateTime.Now.Ticks - _dtZcVer.Ticks > 100000000;
                    //&&
                    //   DateTime.Now.Ticks - _dt1SndWeekSet8.Ticks > 100000000;


            return false;
        }

        #endregion

        #region 过滤命令

        #region CmdWatch 

        private DateTime _dtWatch;
        private ICommand _cmdWatch ;
        public ICommand CmdWatch
        {
            get
            {
                return _cmdWatch ??
                       (_cmdWatch =
                        new RelayCommand<string >(ExWatch, CanWatchOpen, false ));
            }
        }
        private void ExWatch(string datax)
        {
            //1、开灯过滤
            //2、关灯过滤
            //3、选测过滤
            //4、对时过滤
            //5、周设置过滤
            //11、开灯全部
            //12、关灯全部
            //13、选测全部
            //14、对时全部
            //15、周设置全部

            _dtWatch = DateTime.Now;
            int x = 0;
            if (Int32.TryParse(datax, out x) == false) return;

            if (x == 1)
            {
                #region
                ShowOpenLightAllData = true;
                for (var i = 0; i < OpenLightReport.Count; i++)
                {
                    var item = OpenLightReport[i];
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
                    OpenLightReport.RemoveAt(i);
                    i--;
                }
                #endregion
            }

            if (x == 2)
            {
                #region
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
                #endregion
            }

            if (x == 3)
            {
                #region
                ShowSelectionAllData = true;
                for (var i = 0; i < SelectionTestReport.Count; i++)
                {
                    var item = SelectionTestReport[i];
                    if (item.K0SelectionTestAns != EnumSelectionTestAns.Ready)
                    {
                        SelectionTestReport.RemoveAt(i);
                        i--;
                    }
                }
                #endregion
            }

            if (x == 4)
            {
                #region  
                ShowSynTimeAllData = true;
                for (var i = 0; i < SyncTimeReport.Count; i++)
                {
                    var item = SyncTimeReport[i];
                    if (!item.SyncTimeAns) continue;
                    SyncTimeReport.RemoveAt(i);
                    i--;
                }
                #endregion
            }

            if (x == 5)
            {
                #region
                ShowWeekSndAllData = true;
                for (var i = 0; i < WeekSndReport.Count; i++)
                {
                    var item = WeekSndReport[i];
                    if (item.WeekSndAns != "√") continue;
                    WeekSndReport.RemoveAt(i);
                    i--;
                }
                #endregion
            }

            if (x == 11)
            {
                ShowOpenLightAllData = false;
                LoadOpenLightReport();
            }

            if (x == 12)
            {
                ShowCloseLightAllData = false;
                LoadCloseLightReport();
            }
            if (x == 13)
            {
                ShowSelectionAllData = false;
                LoadSelectionTestReport();
            }
            if (x == 14)
            {
                ShowSynTimeAllData = false;
                LoadAsyncTimeReport();
            }
            if (x == 15)
            {
                ShowWeekSndAllData = false;
                LoadWeekSndReport();
            }
        }

        private bool CanWatchOpen(string datax)
        {
            //1、开灯过滤
            //2、关灯过滤
            //3、选测过滤
            //4、对时过滤
            //5、周设置过滤
            //11、开灯全部
            //12、关灯全部
            //13、选测全部
            //14、对时全部
            //15、周设置全部
            int x = 0;
            if (Int32.TryParse(datax, out x) == false) return false;
            if (x == 1 || x == 2)
            {
                if (ExLoopCount.Count == 0) return false;
            }
            if (x >= 3 && x < 5)
            {
                if (_rtusThatOpe.Count == 0) return false;
            }
            if (x == 5)
            {
                if (ExWeekSetCount.Count == 0) return false;
            }
            return DateTime.Now.Ticks - _dtWatch.Ticks > 30000000;
        }

        #endregion

        #endregion

        #region 老的命令

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
                    g.WeekSndAns ="0/2";
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
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                {
                    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                    if (sss == UMessageBoxWantPassWord.CancelReturn)
                    {
                        return;
                    }
                    if (sss != UserInfo.UserLoginInfo.UserPassword)
                    {
                        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                                                           UMessageBoxButton.Yes);
                        return;
                    }
                }else
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
                var k1Rtus = TreeTmlNode.GetNodeKxChecked(1, false);
                var k2Rtus = TreeTmlNode.GetNodeKxChecked(2, false);
                var k3Rtus = TreeTmlNode.GetNodeKxChecked(3, false);
                var k4Rtus = TreeTmlNode.GetNodeKxChecked(4, false);
                var k5Rtus = TreeTmlNode.GetNodeKxChecked(5, false);
                var k6Rtus = TreeTmlNode.GetNodeKxChecked(6, false);
                var k7Rtus = TreeTmlNode.GetNodeKxChecked(7, false);
                var k8Rtus = TreeTmlNode.GetNodeKxChecked(8, false);
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

                //通知 开关灯显示区域 取消显示开关灯命令 由控制中心接管显示
              //  ZOrders.OpenCloseLight.OpenCloseLightDataDispatch.IsControlCenterManagDemo2TakeOverOcOrderShow = true;


                OcCount = 0;
                _opeTime = DateTime.Now.Ticks;
                TimeAns = 0;
                TimeAnss = "";
                OcCountAns = 0;
                OcTmlCount = 0;
                int xtmp = 0;

                ExLoopCount.Clear();
                var nodes = TreeTmlNode.GetNodeChecked(false );
                OcTmlCount = nodes.Count;
                foreach (var f in data.Items)
                {
                    foreach (var g in f.Rtus)
                    {
                        ExLoopCount.TryAdd(new Tuple<int, int>(g, f.LoopId),false ); //todo
                    }

                    xtmp += f.Rtus.Count;
                }
                OcCount = xtmp;

                data.Op = 2;
                data.ItemsShield.Add(new OpenCloseOperatorCenter.ShieldTimeItem()
                                         {
                                             MinutesforShield = NoAlarmTime,
                                             RtusShield = new List<int>()
                                         });
                foreach (var t in data.Items)
                {
                    foreach (var tt in t.Rtus)
                    {
                        if (!data.ItemsShield[0].RtusShield.Contains(tt))
                        {
                            data.ItemsShield[0].RtusShield.Add(tt);
                        }
                    }
                }

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
                foreach (var g in ExLoopCount.Keys)
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

                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                {
                    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                    if (sss == UMessageBoxWantPassWord.CancelReturn)
                    {
                        return;
                    }
                    if (sss != UserInfo.UserLoginInfo.UserPassword)
                    {
                        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                                                           UMessageBoxButton.Yes);
                        return;
                    }
                }
                else
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
                var k1Rtus = TreeTmlNode.GetNodeKxChecked(1, false);
                var k2Rtus = TreeTmlNode.GetNodeKxChecked(2, false);
                var k3Rtus = TreeTmlNode.GetNodeKxChecked(3, false);
                var k4Rtus = TreeTmlNode.GetNodeKxChecked(4, false);
                var k5Rtus = TreeTmlNode.GetNodeKxChecked(5, false);
                var k6Rtus = TreeTmlNode.GetNodeKxChecked(6, false);
                var k7Rtus = TreeTmlNode.GetNodeKxChecked(7, false);
                var k8Rtus = TreeTmlNode.GetNodeKxChecked(8, false);
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
                TimeAnss = "";
                OcCountAns = 0;
                OcTmlCount = 0;
                var nodes = TreeTmlNode.GetNodeChecked(false );
                OcTmlCount = nodes.Count;
                ExLoopCount.Clear();
                int xtmp = 0;
                foreach (var f in data.Items)
                {
                    foreach (var g in f.Rtus)
                    {
                        ExLoopCount.TryAdd( new Tuple<int, int>(g, f.LoopId),false ); 
                    }
                    xtmp += f.Rtus.Count;
                }
                OcCount = xtmp;

                data.Op = 2;
                data.ItemsShield.Add(new OpenCloseOperatorCenter.ShieldTimeItem()
                {
                    MinutesforShield = NoAlarmTime,
                    RtusShield = new List<int>()
                });
                foreach (var t in data.Items)
                {
                    foreach (var tt in t.Rtus)
                    {
                        if (!data.ItemsShield[0].RtusShield.Contains(tt))
                        {
                            data.ItemsShield[0].RtusShield.Add(tt);
                        }
                    }
                }

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
                foreach (var g in ExLoopCount.Keys )
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
            //OcTmlCountAns = 0;
            var nodes = TreeTmlNode.GetNodeChecked(true );
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

            _rtusThatOpe.Clear();
            _rtusThatOpe.AddRange(nodes);

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
            TimeAnss = "";
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
            //OcTmlCountAns = 0;
            IsShowSyncTime = true;
            var lstRtu = TreeTmlNode.GetNodeChecked(false );// from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value;
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
            TimeAnss = "";
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
            //OcTmlCountAns = 0;
            var lstRtu = TreeTmlNode.GetNodeChecked(false );//( from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value).ToList();
            if (lstRtu.Count < 1) return;

            foreach (var ts in TreeTmlNode .RegisterTmlNode .Values )
            {
                foreach (var t in ts )
                t.WeekSndAns="0/2";
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
            TimeAnss = "";
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
    
            IsShowAlarmTime = Visibility.Hidden;
            IsShowAlarmTimeSet = Visibility.Hidden;
 
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
                    case EnumReportTypes.YjOpenLightReport:
                    LoadYjOpenLightReport();
                    break;
                    case EnumReportTypes.YjCloseLightReport:
                    LoadYjCloseLightReport();
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
                if (item.WeekSndAns != "√") continue;
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

        #region  CmdFastControlSelect
        private ICommand _cmdFastControlSelect;
        public ICommand CmdFastControlSelect
        {
            get { return _cmdFastControlSelect ?? (_cmdFastControlSelect = new RelayCommand(ExFastControlSelect, CanFastControlSelect, true)); }
        }
        private void ExFastControlSelect()
        {
            FastControlSelect _fastControlSelect = new FastControlSelect(NowSelected);
            _fastControlSelect.OnFormBtnOkClick +=
                     new EventHandler<FastControlSelect.EventArgsFrmFastSelectTmlList>(FastControlSelect_OnFormBtnOkClick);

            _fastControlSelect.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _fastControlSelect.ShowDialog();
        }
        private bool CanFastControlSelect()
        {
            return true;
        }

        private Tuple<List<Tuple<int, int>>, List<string>> NowSelected;
        void FastControlSelect_OnFormBtnOkClick(object sender, FastControlSelect.EventArgsFrmFastSelectTmlList args)
        {
            var tmp = args.Info;
            if (tmp == null) return;


            LoadTreeNodeTemp(tmp.Item1);
            NowSelected = new Tuple<List<Tuple<int, int>>, List<string>>(tmp.Item2,tmp.Item3);

        }
        #endregion

        #region CmdTimeTable
        private ICommand _cmdTimeTable;
        public ICommand CmdTimeTable
        {
            get { return _cmdTimeTable ?? (_cmdTimeTable = new RelayCommand(ExTimeTable, CanTimeTable, true)); }
        }
        private void ExTimeTable()
        {
            if (RdIndex == 1)
            {
                TimeTableSelectWindow _timeTableSelectWindow = new TimeTableSelectWindow();
                _timeTableSelectWindow.OnFormBtnOkClick +=
                         new EventHandler<TimeTableSelectWindow.EventArgsFrmSelectTimeTableList>(TimeTableSelectWindow_OnFormBtnOkClick);

                _timeTableSelectWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _timeTableSelectWindow.ShowDialog();
            }
            else if (RdIndex == 3)
                ExFastControlSelect();
           
        }
        private bool CanTimeTable()
        {
            if (RdIndex == 1 || RdIndex == 3)
            return true;
            else return false;
        }
        #endregion

        void TimeTableSelectWindow_OnFormBtnOkClick(object sender, TimeTableSelectWindow.EventArgsFrmSelectTimeTableList args)
        {
            var tmp = args.Info;
            if (tmp == null) return;

            foreach (var t in Items)
            {
                if (t.PhysicalId > 0)
                {
                    foreach (var tu in tmp)
                    {
                        if (tu.Item1 == t.AreaId && tu.Item2 == t.PhysicalId )
                        {
                            t.IsChecked = true;
                            switch (tu.Item4)
                            {
                                case 1:
                                    t.IsSwitch1Checked = true; break;
                                case 2:
                                    t.IsSwitch2Checked = true; break;
                                case 3:
                                    t.IsSwitch3Checked = true; break;
                                case 4:
                                    t.IsSwitch4Checked = true; break;
                                case 5:
                                    t.IsSwitch5Checked = true; break;
                                case 6:
                                    t.IsSwitch6Checked = true; break;
                                case 7:
                                    t.IsSwitch7Checked = true; break;
                                case 8:
                                    t.IsSwitch8Checked = true; break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var tt in t.ChildTreeItems)
                    {
                        foreach (var tu in tmp)
                        {
                            if (tu.Item1 == t.AreaId && tu.Item3 == tt.NodeId)
                            {
                                tt.IsChecked = true;
                                switch (tu.Item4)
                                {
                                    case 1:
                                        tt.IsSwitch1Checked = true; break;
                                    case 2:
                                        tt.IsSwitch2Checked = true; break;
                                    case 3:
                                        tt.IsSwitch3Checked = true; break;
                                    case 4:
                                        tt.IsSwitch4Checked = true; break;
                                    case 5:
                                        tt.IsSwitch5Checked = true; break;
                                    case 6:
                                        tt.IsSwitch6Checked = true; break;
                                    case 7:
                                        tt.IsSwitch7Checked = true; break;
                                    case 8:
                                        tt.IsSwitch8Checked = true; break;
                                }
                            }
                        }

                    }
                }

            }
            

        }

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
            this.Items.Add(new TreeGroupNode(-1, -1));
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


        //初始化时加载左侧树终端节点
        private void LoadTreeNodeTempByLn(Dictionary<int,List<int>> rtusLn)
        {
            Items.Clear();
            var rtus = new List<int>();

            foreach (var g in rtusLn)
            {
                if (rtus.Contains(g.Key)==false ) rtus.Add(g.Key);
            } 
            this.Items.Add(new TreeGroupNodeTemp(rtus));

            foreach (var g in Items)
            {
                g.IsChecked = true;
                foreach (var k in g.ChildTreeItems)
                {
                    if (!rtusLn.ContainsKey(k.NodeId)) continue;
                    var switchoutIDs = rtusLn[k.NodeId];
                    if (switchoutIDs.Contains(1)) k.IsSwitch1Checked = true;
                    if (switchoutIDs.Contains(2)) k.IsSwitch2Checked = true;
                    if (switchoutIDs.Contains(3)) k.IsSwitch3Checked = true;
                    if (switchoutIDs.Contains(4)) k.IsSwitch4Checked = true;
                    if (switchoutIDs.Contains(5)) k.IsSwitch5Checked = true;
                    if (switchoutIDs.Contains(6)) k.IsSwitch6Checked = true;
                    if (switchoutIDs.Contains(7)) k.IsSwitch7Checked = true;
                    if (switchoutIDs.Contains(8)) k.IsSwitch8Checked = true;
                   



                }
                
            }

        }

        //初始化时加载左侧树终端节点
        private void AddTreeNodeTempByLn( ConcurrentDictionary<int, List<int>> rtusLn)
        {

            if (RdIndex == 3)//已经是火零不平衡界面
            {
                ////列表里已存在
                //var lst = (from t in Items[0].ChildTreeItems where rtusLn.ContainsKey(t.NodeId) select t).ToList();

                //foreach (var g in lst)
                //{
                //    var loops = rtusLn[g.NodeId];
                //    if (loops.Contains(1)) g.IsSwitch1Checked = true;
                //    if (loops.Contains(2)) g.IsSwitch2Checked = true;
                //    if (loops.Contains(3)) g.IsSwitch3Checked = true;
                //    if (loops.Contains(4)) g.IsSwitch4Checked = true;
                //    if (loops.Contains(5)) g.IsSwitch5Checked = true;
                //    if (loops.Contains(6)) g.IsSwitch6Checked = true;
                //    if (loops.Contains(7)) g.IsSwitch7Checked = true;
                //    if (loops.Contains(8)) g.IsSwitch8Checked = true;
                //    rtusLn.Remove(g.NodeId);
                //}
                if (rtusLn.Count == 0) return;

                var rtus = new List<int>();
                foreach (var g in rtusLn)
                {
                    if (rtus.Contains(g.Key) == false) rtus.Add(g.Key);
                }

                foreach (var t in Items)
                {
                    if(t.AreaName==GroupComboBoxSelected.Value)
                        return;
                }
                this.Items.Add(new TreeGroupNodeTemp(rtus, GroupComboBoxSelected.Value));
                int index = Items.Count;
                //foreach (var g in Items)
                //{

                Items[index - 1].IsChecked = true;
                foreach (var k in Items[index - 1].ChildTreeItems)
                {
                    if (!rtusLn.ContainsKey(k.NodeId)) continue;
                    var switchoutIDs = rtusLn[k.NodeId];
                    if (switchoutIDs.Contains(1)) k.IsSwitch1Checked = true;
                    if (switchoutIDs.Contains(2)) k.IsSwitch2Checked = true;
                    if (switchoutIDs.Contains(3)) k.IsSwitch3Checked = true;
                    if (switchoutIDs.Contains(4)) k.IsSwitch4Checked = true;
                    if (switchoutIDs.Contains(5)) k.IsSwitch5Checked = true;
                    if (switchoutIDs.Contains(6)) k.IsSwitch6Checked = true;
                    if (switchoutIDs.Contains(7)) k.IsSwitch7Checked = true;
                    if (switchoutIDs.Contains(8)) k.IsSwitch8Checked = true;




                }


            }
            else
            {
                RdGbIsEnable = false;
                LinshiVisi = Visibility.Visible;
                RtuSumTemp = "";
                RdIndex = 3;
                Items.Clear();
                ////火零不平衡 lvf 2018年6月13日09:13:46
                //IsLnErr = true;
                RtuSumTemp = rtusLn.Count + "";
                AddTreeNodeTempByLn(rtusLn);


            }
        }

        private void LoadOpenLightReport()
        {
         

            OpenLightReport.Clear();
            //if(RdIndex == 2 )
            //{
            //    foreach (var f in Items)
            //    {
            //        foreach (var g in f.ChildTreeItems)
            //        {
            //            if (g.IsChecked == true )
            //            {
            //                if (g.IsSwitch1Checked || g.IsSwitch2Checked || g.IsSwitch3Checked || g.IsSwitch4Checked ||
            //          g.IsSwitch5Checked || g.IsSwitch6Checked || g.IsSwitch7Checked || g.IsSwitch8Checked)
            //                    OpenLightReport.Add(g);

            //                //if (OpenLightReport.Contains(g))
            //                //{
                                
            //                //}
            //                //else
            //                //{
                                
            //                //}
                            
            //            }
            //        }
            //    }//todo
            //}
            //else
            {
                var nodes = TreeTmlNode.GetAnykxChecked(false  );
                foreach (var node in nodes)
                {
                    OpenLightReport.Add(node);
                }
            }
            

        

        }

        private void LoadCloseLightReport()
        {
        
            CloseLightReport.Clear();
            //if (RdIndex == 2)
            //{

            //}
            //else
            {
                var nodes = TreeTmlNode.GetAnykxChecked(false);
                foreach (var node in nodes)
                {
                    CloseLightReport.Add(node);
                }
            }
        }

        private void LoadYjOpenLightReport()
        {


            YjOpenLightReport.Clear();
           
     
                var nodes = TreeTmlNode.GetAnykxChecked(false);
                foreach (var node in nodes)
                {
                    YjOpenLightReport.Add(node);
                }
          




        }

        private void LoadYjCloseLightReport()
        {

            YjCloseLightReport.Clear();
            //if (RdIndex == 2)
            //{

            //}
            //else
            {
                var nodes = TreeTmlNode.GetAnykxChecked(false);
                foreach (var node in nodes)
                {
                    YjCloseLightReport.Add(node);
                }
            }
        }

        private void LoadSelectionTestReport()
        {
            SelectionTestReport.Clear();
            var nodes = TreeTmlNode.GetAnyChecked(true );
               // (from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value).ToList();
            foreach (var node in nodes)
            {
                SelectionTestReport.Add(node);
            }
        }
        private void LoadAsyncTimeReport()
        {
            SyncTimeReport.Clear();
            var nodes = TreeTmlNode.GetAnyChecked(false );
          //    (from item in TreeTmlNode.RegisterTmlNode where item.Value.IsChecked select item.Value).ToList();
            foreach (var node in nodes)
            {
                SyncTimeReport.Add(node);
            }
        }

        private void LoadWeekSndReport()
        {
            WeekSndReport.Clear();
            var nodes = TreeTmlNode.GetAnyChecked(false );
            foreach (var node in nodes)
            {
                WeekSndReport.Add(node);
            }
        }

  
    }

    public partial class ControlCenterViewModel2
    {
        //private void InitEvent()
        //{
        //    AddEventFilterInfo(Cr.CoreOne.CoreIdAssign.EventIdAssign.AsyncTimeEventId, PublishEventType.Core);


        //}
        //public override void ExPublishedEvent(PublishEventArgs args)
        //{
        //    #region 时间同步
        //    if (args.EventId == Cr.CoreOne.CoreIdAssign.EventIdAssign.AsyncTimeEventId)  //事件在OpenCloseLightDataDispatch文件中监听，后发布该事件
        //    {
        //        var lst = args.GetParams()[0] as List<int>;
        //        if (lst == null) return;

        //        foreach (var key in TreeTmlNode.RegisterTmlNode.Keys.ToList().Where(lst.Contains))
        //        {
        //            foreach (var f in TreeTmlNode.RegisterTmlNode[key])
        //        f.SyncTimeAns =true;
        //        }

        //        Remind = "时钟同步数据已返回！！！";
        //    }
        //    #endregion



        //}
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_rtu_orders,
                                          //.wlst_svr_ans_cnt_request_snd_rtu_time,
                                          //.ClientPart.wlst_asyntime_server_ans_clinet_order_sendweeksetk1k3,
                                          ResponseSndWeekSetK1K3, typeof (ControlCenterViewModel2), this);


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_svr_ans_cnt_order_rtu_open_close_light,
                // .wlst_svr_ans_cnt_wj3090_order_open_close_light ,//.ClientPart.wlst_OpenCloseLight_server_ans_clinet_order_opencloseLight ,
                ExExecuteOpenLight,
                typeof (ControlCenterViewModel2), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center,
                // .wlst_svr_ans_cnt_wj3090_order_open_close_light ,//.ClientPart.wlst_OpenCloseLight_server_ans_clinet_order_opencloseLight ,
                ExAlarm,
                typeof (ControlCenterViewModel2), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_zc_rtu_info,
                // .wlst_svr_ans_cnt_wj3090_order_open_close_light ,//.ClientPart.wlst_OpenCloseLight_server_ans_clinet_order_opencloseLight ,
                ResponseZc,
                typeof (ControlCenterViewModel2), this);
        }




        private void ExExecuteOpenLight(string session, Wlst.mobile.MsgWithMobile args)
        {
            var datax = args.WstRtuSvrAnsCntOrderOpenCloseLight;
            var lst = args.Args.Addr;
            if (lst == null) return;
            var tu = new Tuple<int, int>(datax.RtuId, datax.LoopId);
            if (_lastOcOr != 4 && _lastOcOr != 5 && _lastOcOr != 14 && _lastOcOr != 15)
                return;


            if (ExLoopCount == null) return;
            if (ExLoopCount.ContainsKey(tu) == false) //非控制中心发出的开关灯指令  解析并在信息区域显示
            {

                //string strisk = datax.IsSingle == false ? "组合开关灯命令" : datax.IsOpen ? "开灯" : "关灯";
                //foreach (var f in lst)
                //{
                //    string tmlName = "";
                //    // str += f;
                //    var g = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                //    if (g != null)
                //    {
                //        tmlName = g.RtuName;
                //    }
                //    Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                //       f, tmlName, Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.ServerReply, strisk + "成功");
                //}
                return;
            }
            //   data.Enqueue(new Tuple<int, int, bool>(datax.RtuId, datax.LoopId, datax.IsOpen));
            bool outbol = false;
            ExLoopCount.TryRemove(tu, out outbol);



            //   Interlocked.Increment(ref OcCountAns);
            lock (this)
            {
                OcCountAns++;
            }

            var rtuid = datax.RtuId;
            var loopid = datax.LoopId;
            var isOpen = datax.IsOpen;

            if (!TreeTmlNode.RegisterTmlNode.Keys.ToList().Contains(rtuid)) return;

            var lastIsOpen = _lastOcOr == 4 || _lastOcOr == 15;
            if (isOpen != lastIsOpen) return;
            switch (loopid)
            {
                case 1:
                    //   if (isOpen == lastIsOpen )
                    {
                        foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                            f.K1OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                    }
                    break;
                case 2:
                    // if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                    {
                        foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                            f.K2OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                    }
                    break;
                case 3:
                    // if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                    {
                        foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                            f.K3OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                    }
                    break;
                case 4:
                    //if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                    {
                        foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                            f.K4OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                    }
                    break;
                case 5:
                    // if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                    {
                        foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                            f.K5OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                    }
                    break;
                case 6:
                    // if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                    {
                        foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                            f.K6OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                    }
                    break;
                case 7:
                    // if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                    {
                        foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                            f.K7OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                    }
                    break;
                case 8:
                    // if ((isOpen && _isOnOpenLight == 1) || (!isOpen && _isOnOpenLight == 2))
                    {
                        foreach (var f in TreeTmlNode.RegisterTmlNode[rtuid])
                            f.K8OpenOrCloseAns = EnumOpenOrCloseAns.YesAnswer;
                    }
                    break;
            }

            var tmp = DateTime.Now.Ticks - _opeTime;
            if (tmp > 0)
            {
                TimeAns = (tmp/100000)*0.01;
                TimeAnss = TimeAns + " s";
            }


        }






        private void ResponseSndWeekSetK1K3(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isViewShow == false) return;
            var datax = infos.WstRtuOrders;
            if (datax == null) return;

            if (datax.Op >= 11 && datax.Op < 16)
            {
                if (_lastOcOr != 8 && _lastOcOr != 14 && _lastOcOr != 15) return;

                #region

                var lst = infos.Args.Addr;
                if (lst == null) return;
                foreach (var fg in lst)
                {
                    if (TreeTmlNode.RegisterTmlNode.ContainsKey(fg) == false) continue;
                    if (ExWeekSetCount.ContainsKey(fg) == false) continue;


                    ExWeekSetCount[fg] += 1;
                    foreach (var f in TreeTmlNode.RegisterTmlNode[fg])
                    {
                        if (ExWeekSetCount[fg] == 1) f.WeekSndAns = "1/2";
                        else f.WeekSndAns = "√";

                    }

                    if (ExWeekSetCount[fg] >= 2)
                    {
                        int rtuid = 0;
                        ExWeekSetCount.TryRemove(fg, out rtuid);
                        lock (this)
                        {
                            OcTmlCount++;
                        }
                    }

                    lock (this)
                    {
                        OcCountAns++;
                    }
                    Remind = "发送周设置数据已返回！！！";
                    var tmp = DateTime.Now.Ticks - _opeTime;
                    if (tmp > 0)
                    {
                        TimeAns = (tmp/100000)*0.01;
                        TimeAnss = TimeAns + " s";
                    }
                }




                #endregion
            }
            if (datax.Op == 14)
            {
                if (_lastOcOr != 8 || _lastOcOr != 14 || _lastOcOr != 15) return;

                #region

                var lst = infos.Args.Addr;
                if (lst == null) return;
                foreach (var fg in lst)
                {
                    if (TreeTmlNode.RegisterTmlNode.ContainsKey(fg) == false) continue;
                    if (ExWeekSetCount.ContainsKey(fg) == false) continue;


                    ExWeekSetCount[fg] += 1;
                    foreach (var f in TreeTmlNode.RegisterTmlNode[fg])
                    {
                        if (ExWeekSetCount[fg] < 36) f.WeekSndAns = ExWeekSetCount[fg] + "/36";
                        else f.WeekSndAns = "√";
                    }

                    if (ExWeekSetCount[fg] >= 36)
                    {
                        int rtuid = 0;
                        ExWeekSetCount.TryRemove(fg, out rtuid);
                        lock (this)
                        {
                            OcTmlCount++;
                        }
                    }

                    lock (this)
                    {
                        OcCountAns++;
                    }
                    Remind = "发送周设置数据已返回!!!";
                    var tmp = DateTime.Now.Ticks - _opeTime;
                    if (tmp > 0)
                    {
                        TimeAns = (tmp/100000)*0.01;
                        TimeAnss = TimeAns + " s";
                    }
                }



                #endregion
            }
            else if (datax.Op == 7 || datax.Op == 8)
            {

                #region

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
                {
                    TimeAns = (tmp/100000)*0.01;
                    TimeAnss = TimeAns + " s";
                }
                Remind = "参数设置反馈数据已返回！！！";

                #endregion
            }
            else if (datax.Op == 31)
            {
                if (_lastOcOr != 6) return;

                #region

                foreach (var data in datax.Items)
                {
                    if (TreeTmlNode.RegisterTmlNode.ContainsKey(data.RtuId) == false) continue;
                    if (_opLst.Contains(data.RtuId) == false) continue;
                    _opLst.Remove(data.RtuId);
                    foreach (var item in TreeTmlNode.RegisterTmlNode[data.RtuId])
                    {
                        item.K0SelectionTestAns = EnumSelectionTestAns.Reply;
                        for (int i = 0; i < data.IsSwitchOutAttraction.Count; i++)
                        {
                            switch (i + 1)
                            {
                                case 1:
                                    item.K1SelectionTestAns = data.IsSwitchOutAttraction[0]
                                                                  ? EnumSelectionTestAns.Open
                                                                  : EnumSelectionTestAns.Close;
                                    break;
                                case 2:
                                    item.K2SelectionTestAns = data.IsSwitchOutAttraction[1]
                                                                  ? EnumSelectionTestAns.Open
                                                                  : EnumSelectionTestAns.Close;
                                    break;
                                case 3:
                                    item.K3SelectionTestAns = data.IsSwitchOutAttraction[2]
                                                                  ? EnumSelectionTestAns.Open
                                                                  : EnumSelectionTestAns.Close;
                                    break;
                                case 4:
                                    item.K4SelectionTestAns = data.IsSwitchOutAttraction[3]
                                                                  ? EnumSelectionTestAns.Open
                                                                  : EnumSelectionTestAns.Close;
                                    break;
                                case 5:
                                    item.K5SelectionTestAns = data.IsSwitchOutAttraction[4]
                                                                  ? EnumSelectionTestAns.Open
                                                                  : EnumSelectionTestAns.Close;
                                    break;
                                case 6:
                                    item.K6SelectionTestAns = data.IsSwitchOutAttraction[5]
                                                                  ? EnumSelectionTestAns.Open
                                                                  : EnumSelectionTestAns.Close;
                                    break;
                                case 7:
                                    item.K7SelectionTestAns = data.IsSwitchOutAttraction[6]
                                                                  ? EnumSelectionTestAns.Open
                                                                  : EnumSelectionTestAns.Close;
                                    break;
                                case 8:
                                    item.K8SelectionTestAns = data.IsSwitchOutAttraction[7]
                                                                  ? EnumSelectionTestAns.Open
                                                                  : EnumSelectionTestAns.Close;
                                    break;
                                default:
                                    Remind = "开关数不在1-8之间";
                                    break;
                            }
                        }
                    }
                    lock (this)
                    {
                        if (_rtusThatOpe.Contains(data.RtuId)) _rtusThatOpe.Remove(data.RtuId);
                        OcCountAns ++;
                    }
                    var tmp = DateTime.Now.Ticks - _opeTime;
                    if (tmp > 0)
                    {
                        TimeAns = (tmp/100000)*0.01;
                        TimeAnss = TimeAns + " s";
                    }

                }

                #endregion
            }
            else if (datax.Op == 21)
            {
                //if (_lastOcOr != 7) return;

                if (_isSyncTime == false) return; //是否点击了对时

                #region

                foreach (var f in datax.RtuIds)
                {
                    if (TreeTmlNode.RegisterTmlNode.ContainsKey(f) == false) continue;
                    if (_rtusThatOpe.Contains(f) == false) continue;

                    foreach (var fg in TreeTmlNode.RegisterTmlNode[f])
                        fg.SyncTimeAns = true;

                    lock (this)      //lvf 2018年3月29日09:11:03 遍历表中  有应答的 计数 呈现到按钮text中
                    {
                        int ansNum = 0;
                        foreach (var g in SyncTimeReport)
                        {
                            if (g.SyncTimeAns ==true ) ansNum++;
                        }
                        SyncTime = ansNum + " 补测";


                        //if (_rtusThatOpe.Contains(f)) _rtusThatOpe.Remove(f);
                        //OcCountAns++;
                        //SyncTime = OcCountAns + " 补测";
                    }
                    var tmp = DateTime.Now.Ticks - _opeTime;
                    if (tmp > 0)
                    {
                        TimeAns = (tmp/100000)*0.01;
                        TimeAnss = TimeAns + " s";
                    }

                    Remind = "时钟同步数据返回!!!";
                }

                #endregion
            }
            else if (datax.Op == 22) //召测时间  lvf 2018年3月29日09:05:42
            {
                //if (_lastOcOr != 7) return;

                if (_isZcTime == false) return; //是否点击了对时

                #region

                foreach (var f in datax.RtuIds)
                {
                    if (TreeTmlNode.RegisterTmlNode.ContainsKey(f) == false) continue;
                    //if (_rtusThatOpe.Contains(f) == false) continue;

                    foreach (var fg in TreeTmlNode.RegisterTmlNode[f])
                    {
                        fg.ZcTimeAns = datax.Date;
                    }




                    lock (this) //lvf 2018年3月29日09:11:03 遍历表中  有应答的 计数 呈现到按钮text中
                    {
                        //if (_rtusThatOpe.Contains(f)) _rtusThatOpe.Remove(f);
                        //OcCountAns++;
                        int ansNum = 0;
                        foreach (var g in SyncTimeReport)
                        {
                            if (g.ZcTimeAns != "---") ansNum++;
                        }
                        ZcTime = ansNum + " 补测";
                    }
                    var tmp = DateTime.Now.Ticks - _opeTime;
                    if (tmp > 0)
                    {
                        TimeAns = (tmp/100000)*0.01;
                        TimeAnss = TimeAns + " s";
                    }

                    Remind = "时钟同步数据返回!!!";
                }

                #endregion
            }

        }
        private void ResponseZc(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isViewShow == false) return;
            if (_isZcVer == false) return; //是否点击了召测版本
            var datax = infos.WstRtuZcInfo;
            if (datax == null) return;
            if (datax.RtuId == 0) return;

            #region

            if (TreeTmlNode.RegisterTmlNode.ContainsKey(datax.RtuId) == false) return;
            //if (_rtusThatOpe.Contains(datax.RtuId) == false) return;

            foreach (var fg in TreeTmlNode.RegisterTmlNode[datax.RtuId])
                fg.ZcVerAns = datax.Version;

            lock (this)  //lvf 2018年3月29日09:11:03 遍历表中  有应答的 计数 呈现到按钮text中
            {
                //if (_rtusThatOpe.Contains(f)) _rtusThatOpe.Remove(f);


                int ansNum = 0;
                foreach (var g in SyncTimeReport)
                {
                    if (g.ZcVerAns != "---") ansNum++;
                }
                ZcVer = ansNum + " 补测";
            }
            var tmp = DateTime.Now.Ticks - _opeTime;
            if (tmp > 0)
            {
                TimeAns = (tmp / 100000) * 0.01;
                TimeAnss = TimeAns + " s";
            }

            Remind = "召测版本数据返回!!!";

            #endregion
        }
    




    #region 屏蔽报警

        #region 定义

        private int _noalarmtime;
        public int NoAlarmTime
        {
            get { return _noalarmtime; }
            set
            {
                if (_noalarmtime == value) return;
                _noalarmtime = value;
                RaisePropertyChanged(() => NoAlarmTime);
            }
        }

        private int _colnoalarmtimehour;
        public int ColNoAlarmTimeHour
        {
            get { return _colnoalarmtimehour; }
            set
            {
                if (_colnoalarmtimehour == value) return;
                _colnoalarmtimehour = value;
                RaisePropertyChanged(() => ColNoAlarmTimeHour);
            }
        }

        private int _colnoalarmtimeminute;
        public int ColNoAlarmTimeMinute
        {
            get { return _colnoalarmtimeminute; }
            set
            {
                if (_colnoalarmtimeminute == value) return;
                _colnoalarmtimeminute = value;
                RaisePropertyChanged(() => ColNoAlarmTimeMinute);
            }
        }

        private long _noalarmtimeend;
        public long NoAlarmTimeEnd
        {
            get { return _noalarmtimeend; }
            set
            {
                if (_noalarmtimeend == value) return;
                _noalarmtimeend = value;
                RaisePropertyChanged(() => NoAlarmTimeEnd);
            }
        }

        private Visibility _isshowalarmtime;
        public Visibility IsShowAlarmTime
        {
            get { return _isshowalarmtime; }
            set
            {
                if (_isshowalarmtime == value) return;
                _isshowalarmtime = value;
                RaisePropertyChanged(() => IsShowAlarmTime);
            }
        }

        private Visibility _isshowalarmtimeset;
        public Visibility IsShowAlarmTimeSet
        {
            get { return _isshowalarmtimeset; }
            set
            {
                if (_isshowalarmtimeset == value) return;
                _isshowalarmtimeset = value;
                RaisePropertyChanged(() => IsShowAlarmTimeSet);
            }
        }
        #endregion

        #region 初始化请求报警时间

        private void GetNoLoadAlarmTime()
        {
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.IsControlCenterNoErr)
            {
                var infos = Elysium.ThemesSet.Common.ReadSave.Read("NoAlarmTime");
                if (infos.ContainsKey("database")) NoAlarmTime = Convert.ToInt32(infos["database"]);
                else NoAlarmTime = 0;

                ColNoAlarmTimeHour = 0;
                ColNoAlarmTimeMinute = 0;
            }
            else
            {
                NoAlarmTime = 0;
                ColNoAlarmTimeHour = 0;
                ColNoAlarmTimeMinute = 0;
            }

            var info = Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center;
            info.WstRtuCntOrderOpenCloseCenter.Op = 3;
            SndOrderServer.OrderSnd(info, 10, 6);

        }

        #endregion

        private void ExAlarm(string session, Wlst.mobile.MsgWithMobile args)
        {

            var datax = args.WstRtuCntOrderOpenCloseCenter;
            if (datax.Op == 3)
            {
                if (datax.ItemsShield.Count > 0)
                {
                    NoAlarmTime = Convert.ToInt32((datax.ItemsShield[0].DtShieldTimeEnd - datax.ItemsShield[0].DtShieldTimeStart) / 600000000);
                    NoAlarmTimeEnd = datax.ItemsShield[0].DtShieldTimeStart;
                }
            }
            else if (datax.Op == 2)
            {
                if (datax.ItemsShield.Count > 0)
                {
                    NoAlarmTime = datax.ItemsShield[0].MinutesforShield;
                    NoAlarmTimeEnd = DateTime.Now.Ticks + ((long)NoAlarmTime * 600000000);
                }
            }
            else if (datax.Op == 4)
            {
                NoAlarmTime = 0;
                NoAlarmTimeEnd = 0;
                ColNoAlarmTimeHour = 0;
                ColNoAlarmTimeMinute = 0;
            }

            if (!Others.IsControlCenterNoErr) NoAlarmTime = 0;
        }

        public void SndDeleteAlarmTime()
        {
            var info = Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center;
            info.WstRtuCntOrderOpenCloseCenter.Op = 4;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
        public void SelectAllSwitchOut( int kx)
        {
            _currentSelectAllStateTmp = !_currentSelectAllStateTmp;
            switch (kx)
            {
                case 1:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch1Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 2:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch2Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 3:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch3Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 4:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch4Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 5:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch5Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 6:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch6Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 7:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch7Checked = _currentSelectAllStateTmp;
                    }
                    break;
                case 8:
                    foreach (var t in this.Items)
                    {
                        t.IsSwitch8Checked = _currentSelectAllStateTmp;
                    }
                    break;

            }


        }

       private void ColNoAlarmTime(object obj)
       {
           if (_isViewShow == false )return;
           if (DateTime.Now.Ticks < NoAlarmTimeEnd)
           {
               var time = Convert.ToInt32((NoAlarmTimeEnd - DateTime.Now.Ticks) / 10000000);
               ColNoAlarmTimeHour = time/60;
               ColNoAlarmTimeMinute = time % 60;
               IsShowAlarmTime = Visibility.Visible;
           }
           //else if (DateTime.Now.Ticks - NoAlarmTimeEnd > 100000000)
           //{
           //    ColNoAlarmTimeHour = 0;
           //    ColNoAlarmTimeMinute = 0;
           //    if (IsShowAlarmTime == Visibility.Hidden) IsShowAlarmTime = Visibility.Visible;
           //}
           else
           {
               IsShowAlarmTime = Visibility.Collapsed;
               ColNoAlarmTimeHour = 0;
               ColNoAlarmTimeMinute = 0;
           }
       }

        
        #endregion





    }
}
