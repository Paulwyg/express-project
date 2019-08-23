using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel
{
    /// <summary>
    /// Title  回路背景色 
    /// </summary>
    [Export(typeof (IINewDataVmLeft))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataVmLeft : IINewDataVmLeft, Wlst.Cr.CoreMims.CoreInterface.IIShowData
    {
        private static NewDataVmLeft _myself;

        public static NewDataVmLeft Myself
        {
            get { return _myself; }
        }


        private bool _isload = false;

        private void Acload()
        {
            _isload = true;
        }

        public NewDataVmLeft()
        {
            _myself = this;

            LoadOpXml();
            LoadFromHeightInfo();
            //if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 2, 1) == 2)
            //{
            //    CurrentUserThisView = true;
            //    //if (EventSelectRtuidWhileNotUserThisView != 0)
            //    //    OnSelectRtuIdChange(EventSelectRtuidWhileNotUserThisView, true);
            //}
            //else
            //{
            //    CurrentUserThisView = false;
            //}



            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Acload, 2, DelayEventHappen.EventOne);

            InitAction();
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);
        }


        #region tabTitle

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "最新数据"; }
        }


        public bool CanClose
        {
            get { return false; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        /// <summary>
        /// 页面加载或是导航显示的时候 需要执行的初始化操作
        /// </summary>
        /// <param name="parsObjects"></param>
        public void NavOnLoad(params object[] parsObjects)
        {
            //if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 2, 1) == 2)
            //{
            //    CurrentUserThisView = true;
            //    //if (EventSelectRtuidWhileNotUserThisView != 0)
            //    //    OnSelectRtuIdChange(EventSelectRtuidWhileNotUserThisView, true);
            //}
            //else
            //{
            //    CurrentUserThisView = false;
            //}
            IsShowFailVis = Wlst.Sr.EquipmentInfoHolding.Services.Others.IsMeasureFail?Visibility.Visible : Visibility.Collapsed;
        }

        #endregion


        #region CanvansHeight && DataHeight && first load from xml && update

        private const string XmlConfigName = "CETC50_DemoAreaSet.xml";

        private int _maxheightforset = 500;
        private void LoadFromHeightInfo()
        {
            try
            {
                var info =
                    Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(Directory.GetCurrentDirectory() +
                                                                  "\\SystemColorAndFont\\" + XmlConfigName);
                if (info.ContainsKey("Area45Height")) _maxheightforset = Int32.Parse(info["Area45Height"]);
                else _maxheightforset = 500;

                //if (info.ContainsKey("DataArea"))
                //{
                //    var tr = Int32.Parse(info["DataArea"]);
                //    if (tr == 5)
                //    {
                //        CurrentUserThisView = true;
                //    }
                //}

            }
            catch (Exception ex)
            {
                _maxheightforset = 500;
            }
        }

        //private void UpdateMaxHeightWithCanvasHeight(int switchOutCount)
        //{
        //    if (switchOutCount > 6)
        //    {
        //        CanvasHeight = 148;
        //    }
        //    else
        //    {
        //        CanvasHeight = 126;
        //    }
        //    GridViewDataHeiht = MaxHeiht - CanvasHeight;
        //}


        private int _dataMaxHeiht;

        /// <summary>
        /// 最新数据界面最大高度
        /// </summary>
        public int MaxHeiht
        {
            get { return _dataMaxHeiht; }
            set
            {
                if (value == _dataMaxHeiht) return;
                _dataMaxHeiht = value;
                this.RaisePropertyChanged(() => this.MaxHeiht);
            }
        }


        private int _dataCanvasHeight;

        /// <summary>
        /// 表格上方绘图 最大高度
        /// </summary>
        public int CanvasHeight
        {
            get { return _dataCanvasHeight; }
            set
            {
                if (value == _dataCanvasHeight) return;
                _dataCanvasHeight = value;
                this.RaisePropertyChanged(() => this.CanvasHeight);

               // GridViewDataHeiht = MaxHeiht - value - 3; // 45;
            }
        }


        private int _dataGridViewDataHeiht;

        /// <summary>
        /// 列表显示最大高度
        /// </summary>
        public int GridViewDataHeiht
        {
            get { return _dataGridViewDataHeiht; }
            set
            {
                if (value == _dataGridViewDataHeiht) return;
                _dataGridViewDataHeiht = value;
                this.RaisePropertyChanged(() => this.GridViewDataHeiht);
            }
        }

        #endregion

        #region 回路背景色&& 选项里面其他选项 &&  frist load from xml && get

        /// <summary>
        /// 当选侧数据到达的时候  主动显示当前最新数据  弹出最新数据界面
        /// </summary>
        public static bool OnMeasureShowData = false;

        /// <summary>
        /// 最新数据屏蔽回路显示电流电压
        /// </summary>
        public static bool ShieldLoopShA = true ;
        public static bool ShieldLoopShV = true;

        //public static bool HsdataQueryShGjOp = false;
        /// <summary>
        /// 显示数据的单位 A&V
        /// </summary>
        public static bool ShowDw = true;


        public static string BackgroundColor = "Transparent";
        public static string K1BackgroundColor = "Transparent";
        public static string K2BackgroundColor = "Transparent";
        public static string K3BackgroundColor = "Transparent";
        public static string K4BackgroundColor = "Transparent";
        public static string K5BackgroundColor = "Transparent";
        public static string K6BackgroundColor = "Transparent";
        public static string K7BackgroundColor = "Transparent";
        public static string K8BackgroundColor = "Transparent";


        private void LoadOpXml()
        {
            //this.UpdateInfo(ii, jj, kk);
            var info = ZNewData.NewDataSetting.NewDataSettingViewModel.LoadNewDataLenghtSetConfgX();

            IsShowLoopId = ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(0); ;
            //IsCompare = info.Item6.Item3;
            //IsDetailed = info.Item6.Item4;
            //IsOnlineRate = info.Item6.Item5;

            BackgroundColor = info.Item7.Background;
            K1BackgroundColor = info.Item7.K1Background;
            K2BackgroundColor = info.Item7.K2Background;
            K3BackgroundColor = info.Item7.K3Background;
            K4BackgroundColor = info.Item7.K4Background;
            K5BackgroundColor = info.Item7.K5Background;
            K6BackgroundColor = info.Item7.K6Background;
            K7BackgroundColor = info.Item7.K7Background;
            K8BackgroundColor = info.Item7.K8Background;
            OnMeasureShowData = ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(14);// info.Item7.OnMeasureShowData;
            ShieldLoopShA = ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(17);
            ShieldLoopShV = ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(19);
            //HsdataQueryShGjOp = info.Item7.HsdataQueryShGjOp;
            //  Wlst.Sr.EquipmentInfoHolding.Services.Others.HsdataQueryShGjOp = info.Item7.HsdataQueryShGjOp;
            //历史数据查询 显示高级选项  此处仅加载赋值 供使用


        }

        private string GetColorBySwitchOutId(int swid)
        {
            var color = "Transparent";
            if (swid == 1)
            {
                color = K1BackgroundColor;
            }
            if (swid == 2)
            {
                color = K2BackgroundColor;
            }
            if (swid == 3)
            {
                color = K3BackgroundColor;
            }
            if (swid == 4)
            {
                color = K4BackgroundColor;
            }
            if (swid == 5)
            {
                color = K5BackgroundColor;
            }
            if (swid == 6)
            {
                color = K6BackgroundColor;
            }
            if (swid == 7)
            {
                color = K7BackgroundColor;
            }
            if (swid == 8)
            {
                color = K8BackgroundColor;
            }
            return color;
        }

        #endregion

        #region 昨日电流、参数设置、上线率等选项信息

        /// <summary>
        /// 是否显示回路序号  
        /// </summary>
        public static bool IsShowLoopId = false;

        ///// <summary>
        ///// 是否显示昨日对比数据
        ///// </summary>
        //public static bool IsCompare = false;

        ///// <summary>
        ///// 是否显示设置的参数信息
        ///// </summary>
        //public static bool IsDetailed = false;

        ///// <summary>
        ///// 是否显示 在线信息
        ///// </summary>
        //public static bool IsOnlineRate = false;




        //private Visibility _isCompare;

        ///// <summary>
        ///// 昨日电流 是否显示
        ///// </summary>
        //public Visibility IsCompareCheck
        //{
        //    get { return _isCompare; }
        //    set
        //    {
        //        if (_isCompare != value)
        //        {
        //            _isCompare = value;
        //            this.RaisePropertyChanged(() => this.IsCompareCheck);
        //            if (CompareVisiChanged != null) CompareVisiChanged(value, new EventArgs());
        //        }
        //    }
        //}

        //private Visibility _isDetail;

        ///// <summary>
        ///// 电流上下限等设置信息 是否显示
        ///// </summary>
        //public Visibility IsDetailCheck
        //{
        //    get { return _isDetail; }
        //    set
        //    {
        //        if (_isDetail != value)
        //        {
        //            _isDetail = value;
        //            this.RaisePropertyChanged(() => this.IsDetailCheck);
        //            SettingChangedx();

        //        }
        //    }
        //}

        //private Visibility _isOnlineRate;

        ///// <summary>
        ///// 上线率 是否显示
        ///// </summary>
        //public Visibility IsOnlineRateCheck
        //{
        //    get { return _isOnlineRate; }
        //    set
        //    {
        //        if (_isOnlineRate != value)
        //        {
        //            _isOnlineRate = value;
        //            this.RaisePropertyChanged(() => this.IsOnlineRateCheck);
        //            SettingChangedx();

        //        }
        //    }
        //}


        //void SettingChangedx()
        //{
        //    // //100、显示互感比 参考电流，10、显示上下限制，1、请求昨日数据
        //    int x = 0;
        //    if (IsOnlineRate) x += 100;
        //    if (IsDetailed) x += 10;
        //    if (IsCompare) x += 1;

        //    if (SettingChanged != null) SettingChanged(this, new EventArsgLoopCount() { LoopCount =x });
        //}
        public event EventHandler<EventArsgLoopCount> SettingChanged;
        //public event EventHandler DetailVisiChanged;
        //public event EventHandler OnlineVisiChanged;


        #endregion

        #region 显示的信息：终端地址、终端名称、数据时间、终端下的《历史》 是否显示、历史名字、三相电流值、物理地址

        private Visibility _assetNameVisi;

        /// <summary>
        /// 终端名称下的历史 2字是否显示
        /// </summary>
        public Visibility AssetNameVisi
        {
            get { return _assetNameVisi; }
            set
            {
                if (value != _assetNameVisi)
                {
                    _assetNameVisi = value;
                    this.RaisePropertyChanged(() => this.AssetNameVisi);
                }
            }
        }

        private string _timeRtuNameAttInfo;

        /// <summary>
        /// 终端名称 下面的提示信息 如 历史
        /// </summary>
        public string RtuNameAtt
        {
            get { return _timeRtuNameAttInfo; }
            set
            {
                if (value != _timeRtuNameAttInfo)
                {
                    _timeRtuNameAttInfo = value;
                    this.RaisePropertyChanged(() => this.RtuNameAtt);
                }
            }
        }

        private string _menuItemxx;

        /// <summary>
        /// 终端物理地址
        /// </summary>
        public string RtuIdPhy
        {
            get { return _menuItemxx; }
            set
            {
                if (value == _menuItemxx) return;
                _menuItemxx = value;
                this.RaisePropertyChanged(() => this.RtuIdPhy);
            }
        }






        private string _dateTimeGetRtuTime;

        /// <summary>
        /// 获取到终端时间 
        /// </summary>
        public string DateTimeGetRtuTime
        {
            get { return _dateTimeGetRtuTime; }
            set
            {
                if (value != _dateTimeGetRtuTime)
                {
                    _dateTimeGetRtuTime = value;
                    this.RaisePropertyChanged(() => this.DateTimeGetRtuTime);
                }
            }
        }




        private int _rtuId;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;

                    this.RaisePropertyChanged(() => this.RtuId);
                    if (
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                            ContainsKey(_rtuId))
                    {

                        this.RtuName = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            _rtuId].RtuName;

                    }
                }
            }
        }

        private string _rtuName;

        /// <summary>
        /// 终端名称
        /// </summary>
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }



        private string _timeInfo;

        /// <summary>
        /// 终端数据的时间
        /// </summary>
        public string TimeInfo
        {
            get { return _timeInfo; }
            set
            {
                if (value != _timeInfo)
                {
                    _timeInfo = value;
                    this.RaisePropertyChanged(() => this.TimeInfo);
                }
            }
        }


        private Visibility _isShowFail;
        //xiansh
        public Visibility IsShowFailVis
        {
            get
            {
                return _isShowFail;
            }
            set
            {
                if (_isShowFail != value)
                {
                    _isShowFail = value;
                    this.RaisePropertyChanged(() => this.IsShowFailVis);
                }
            }
        }


        private string _timTimeYesterdayeInfo;

        /// <summary>
        /// 终端数据的时间
        /// </summary>
        public string TimeYesterday
        {
            get { return _timTimeYesterdayeInfo; }
            set
            {
                if (value != _timTimeYesterdayeInfo)
                {
                    _timTimeYesterdayeInfo = value;
                    this.RaisePropertyChanged(() => this.TimeYesterday);
                }
            }
        }
         

        private string _timeAx; // todo

        /// <summary>
        /// A相
        /// </summary>
        public string Ax
        {
            get { return _timeAx; }
            set
            {
                if (value != _timeAx)
                {
                    _timeAx = value;
                    this.RaisePropertyChanged(() => this.Ax);
                }
            }
        }

        private int _newDatawidth;
        /// <summary>
        /// 最新数据的 宽度
        /// </summary>
        public int NewDataWidth
        {
            get { return _newDatawidth; }
            set
            {
                if (value != _newDatawidth)
                {
                    _newDatawidth = value;
                    this.RaisePropertyChanged(() => this.NewDataWidth);
                }
            }
        }
        private string _timeABxx; // todo

        /// <summary>
        /// B
        /// </summary>
        public string Bx
        {
            get { return _timeABxx; }
            set
            {
                if (value != _timeABxx)
                {
                    _timeABxx = value;
                    this.RaisePropertyChanged(() => this.Bx);
                }
            }
        }


        private string _timeACxx; // todo

        public string Cx
        {
            get { return _timeACxx; }
            set
            {
                if (value != _timeACxx)
                {
                    _timeACxx = value;
                    this.RaisePropertyChanged(() => this.Cx);
                }
            }
        }


        private string _timeAaxx; // todo

        /// <summary>
        /// 总功
        /// </summary>
        public string AA
        {
            get { return _timeAaxx; }
            set
            {
                if (value != _timeAaxx)
                {
                    _timeAaxx = value;
                    this.RaisePropertyChanged(() => this.AA);
                }
            }
        }





        private string _timeAbxx; // todo

        /// <summary>
        /// 总因
        /// </summary>
        public string AB
        {
            get { return _timeAbxx; }
            set
            {
                if (value != _timeAbxx)
                {
                    _timeAbxx = value;
                    this.RaisePropertyChanged(() => this.AB);
                }
            }
        }

        #endregion

        #region SelectedDateForYes 昨日时间选择

       
        private DateTime _isOnlinesdfsdRate;

        
        public DateTime SelectedDateForYes
        {
            get { return _isOnlinesdfsdRate; }
            set
            {
                //if (_isOnlinesdfsdRate != value)
                {
                    _isOnlinesdfsdRate = value;
                    this.RaisePropertyChanged(() => this.SelectedDateForYes);
                    if (CanQueryData == true)
                    {
                        SelectOldData = true;
                        RequestDailyData(value);
                    }
                    //Changeddata();
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _selectedDateTime = null;
        public ObservableCollection<NameValueInt> SelectedDateTime
        {
            get
            {
                if (_selectedDateTime == null)
                {
                    _selectedDateTime = new ObservableCollection<NameValueInt>();
                }

                return _selectedDateTime;
            }
            set
            {
                if (value == _selectedDateTime) return;
                _selectedDateTime = value;
                this.RaisePropertyChanged(() => SelectedDateTime);
            }
        }

        private NameValueInt _curSelectedDateTime;
        public NameValueInt CurSelectedDateTime
        {
            get { return _curSelectedDateTime; }
            set
            {
                if (value == _curSelectedDateTime) return;
                _curSelectedDateTime = value;
                this.RaisePropertyChanged(() => this.CurSelectedDateTime);

                Req();
            }
        }










        private bool _changebycode = false;
        void Changeddata()
        {
            _changebycode = true;
            int hour = SelectedDateForYes.Hour;
            foreach (var f in SelectedDateForYes1)
                if (f.Value == hour)
                {
                    CurSelectedDateForYes1 = f;
                    break;
                }

            int minue = SelectedDateForYes.Minute;
            foreach (var f in SelectedDateForYes2)
                if (f.Value == minue)
                {
                    CurSelectedDateForYes2 = f;
                    break;
                }
            _changebycode = false;
        }

        void Req()
        {
            //if (_changebycode) return;

            if (CurSelectedDateTime == null) return;

            DateTime dt = Convert.ToDateTime(CurSelectedDateTime.DtName);

            RequestNearData(dt.AddDays(1).Ticks);
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> yes1 = null;
        public ObservableCollection< NameValueInt >  SelectedDateForYes1
        {
            get
            {
                if(yes1 ==null )
                {
                    yes1 = new ObservableCollection<NameValueInt>();
                    for (int i = 0; i < 24; i++) yes1.Add(new NameValueInt() {Value = i, Name = i.ToString("d2")});
                }
                return yes1;
            }
        }

        private NameValueInt cyrs;
        public NameValueInt CurSelectedDateForYes1
        {
            get { return cyrs; }
            set
            {
                if (value == cyrs) return;
                cyrs = value;
                this.RaisePropertyChanged(() => this.CurSelectedDateForYes1);
                Req();
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> yes12 = null;
        public ObservableCollection<NameValueInt> SelectedDateForYes2
        {
            get
            {
                if (yes12 == null)
                {
                    yes12 = new ObservableCollection<NameValueInt>();
                    for (int i = 0; i < 60; i++) yes12.Add(new NameValueInt() { Value = i, Name = i.ToString("d2") });
                }
                return yes12;
            }
        }

        private NameValueInt cyrs1;
        public NameValueInt CurSelectedDateForYes2
        {
            get { return cyrs1; }
            set
            {
                if (value == cyrs1) return;
                cyrs1 = value;
                this.RaisePropertyChanged(() => this.CurSelectedDateForYes2);
                Req();
            }
        }
        #endregion

        #region menu  菜单

        private ContextMenu _cm;

        public ContextMenu Cm
        {
            get
            {
                if (_cm == null)
                {
                    _cm = new ContextMenu();
                    _cm.BorderThickness = new Thickness(0);
                }
                return _cm;
            }
        }

        private ObservableCollection<IIMenuItem> _items;

        public ObservableCollection<IIMenuItem> CmItems
        {
            get { return _items; }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => this.CmItems);
            }
        }



        private void BulidMenus(int rtuId)
        {
            ResetContextMenu(rtuId);

        }


        public void ResetContextMenu(int nodeId)
        {
            UpdateCm(nodeId);
        }


        public void UpdateCm(int rtuId)
        {
            if (
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                    rtuId))
            {
                var tt =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];

                var tmt = MenuBuilding.BulidCm(((int) tt.RtuModel).ToString(), false, tt);
                if (tmt != null)
                {
                    tmt.Insert(0, new MenuItemBase()
                                      {
                                          Text = tt.RtuPhyId.ToString("d3") + "-" + tt.RtuName,
                                          IsEnabled = false,
                                          TextTmp = tt.RtuPhyId.ToString("d3") + "-" + tt.RtuName,
                                      });
                }
                CmItems = tmt;
            }
        }



        #endregion

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_data_1, // .wlst_svr_ans_cnt_request_wj3090_measure_data ,
                RecordDataRequest,
                typeof(NewDataVmLeft), this,true );

            ProtocolServer.RegistProtocol(
               Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_near_data,// .wlst_svr_ans_cnt_request_wj3090_near_measure_data ,//.ProtocolCnt.ClientPart.wlst_Measures_server_ans_clinet_request_Near_data,
               OnNearDataArrive,
               typeof(NewDataVmLeft), this);


            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_rtu_orders,
                                          //.wlst_svr_ans_cnt_request_snd_rtu_time,
                                          //.ClientPart.wlst_asyntime_server_ans_clinet_order_sendweeksetk1k3,
                                          OnRtuTimeArrive, typeof (NewDataVmLeft), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_ana_data, // 请求给定时间段的指定时间的终端最新数据  智能分析  WstRtuAnaData
                HistoryDataArrive,
                typeof (NewDataVmLeft), this);

        }

        #region Event


        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool FundOrderFilter(PublishEventArgs args)
        {
            if (_isload == false) return false;

            if (args.EventType == PublishEventType.Core)
            {
                switch (args.EventId)
                {
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2:
                        if (CurrentUserThisView == false) return false ;
                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null || lst.Count == 0) return false ;
                        //lvf 2018年4月16日13:23:13    判断是否是用户最后一次点击的设备
                        //if (!lst.Contains(Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId)) return false;
                        return true;
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:
                        if (CurrentUserThisView == false) return false;
                        return true;

                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab:
                        if (CurrentUserThisView == false) return false;
                        return true;

                }
            }

            if (args.EventType == "MainWindow.update.windowsset")
            {
                return true;
            }

            return false;
        }

        


        private static  bool currentUserThisView = false;
        /// <summary>
        /// 当前是否启用本页面
        /// </summary>
        public static bool CurrentUserThisView
        {
            get { currentUserThisView =Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 2, 1) == 2;
                return currentUserThisView;
            }
        }
        /// <summary>
        /// 事件 选中的终端地址 如果不显示本界面的话 当需要显示的时候  需要更新设备地址
        /// </summary>
        public static  int EventSelectRtuidWhileNotUserThisView = 0;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private void FundEventHandler(PublishEventArgs args)
        {

            try
            {
                ExExecuteEventIns(args);

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("FundEventHandler No Dispatcher Error:" + ex);
            }
        }



        /// <summary>
        /// 线程执行 具体执行
        /// </summary>
        private void ExExecuteEventIns(PublishEventArgs args)
        {
            try
            {

                if (args.EventId == -1 && args.EventType == "MainWindow.update.windowsset")
                {
                   
                      
                            //if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 8, 4) == 5)
                            //{
                            //    CurrentUserThisView = true;
                            //    //if (EventSelectRtuidWhileNotUserThisView != 0)
                            //    //    OnSelectRtuIdChange(EventSelectRtuidWhileNotUserThisView, true);
                            //}
                            //else
                            //{
                            //    CurrentUserThisView = false;
                            //}
                    _maxheightforset = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 250, "\\SystemColorAndFont");
                    return;

                }
                switch (args.EventId)
                {

                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2:
                       
                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null || lst.Count == 0) return;
                        //if (Infrastructure.DataHolding.Setting.IsOnlyTreeEventCanUpdateNewDataTab)
                        //    //OnlyTreeNodeChangeCanActiveNewData) //仅树选中能激活
                        //{
                        //    if (lst.Contains(RtuIdNeedUpdate))
                        //    {
                        //        OnSelectRtuIdChange(RtuIdNeedUpdate);
                        //    }
                        //    return;
                        //}

                        if (lst.Contains(RtuIdNeedUpdate))
                        {
                            var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuIdNeedUpdate);
                            if (run == null || run.RtuNewData == null) return;
                            CanQueryData = false;
                            OnSelectRtuIdChange(RtuIdNeedUpdate, true);
                        }
                        //else
                        //{
                        //    OnSelectRtuIdChange(lst[0]);
                        //}
                        break;
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:

                        var rtuId = Convert.ToInt32(args.GetParams()[0]);
                        if (rtuId > 1600000 && rtuId < 1700000) return;
                        if (rtuId > 1100000)
                        {
                            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(rtuId)) break;
                            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLine(rtuId)) break;
                            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLeak(rtuId)) break;
                            var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuId);
                            if (tmps == null) return;
                            rtuId = tmps.RtuFid;
                        }
                        if (rtuId > 1000000 && rtuId < 1100000)
                        {
                            if (RtuId == rtuId) return;

                            EventSelectRtuidWhileNotUserThisView = rtuId;
                            
                            if (CurrentUserThisView == false) return  ;
                            CanQueryData = false;
                            this.OnSelectRtuIdChange(rtuId, true);
                            
                        }
                        break;

                        //case Sr.EquipmentInfoHolding.Services.EventIdAssign.HistoryDataUpdate:

                        //    var RtuId = Convert.ToInt32(args.GetParams()[0]);
                        //    var Run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuId);
                        //    if(Run!=null && Run.RtuNewData !=null && RtuId == RtuIdNeedUpdate )
                        //        OnDataChange(RtuId, Run.RtuNewData, "", false);
                        //    else OnDataChange(RtuId,null,"",false);

                        //    HistoryDataResponse = 0;

                        //    break;

                        //case Sr.EquipmentInfoHolding.Services.EventIdAssign.MainEquipmentSelectedByOtherWay:

                        //    var rtuIds = Convert.ToInt32(args.GetParams()[0]);
                        //    if (rtuIds > 0)
                        //    {
                        //        this.OnSelectRtuIdChange(rtuIds);
                        //    }
                        //    break;

                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab:
                        try
                        {
                            var info = args.GetParams()[0] as Wlst.client.TmlNewData;
                            if (info == null) return;
                            OnOtherViewShowData(info);
                        }
                        catch (Exception ex)
                        {

                        }

                        break;

                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }


        #endregion
    }



    public partial class NewDataVmLeft
    {

      public   event EventHandler CompareVisiChanged;
      public event EventHandler DetailVisiChanged;
      public event EventHandler OnlineVisiChanged;

        public int RtuIdNeedUpdate;
        public int HistoryDataResponse;
        public Dictionary<int, Tuple<int, bool, double>> AnaPara;

        // OnlyTreeNodeChangeCanActiveNewData

        private bool _onlyTreeNodeChangeCanActiveNewData;

        /// <summary>
        /// 更新最新数据时 使用书节点焦点转移
        /// </summary>
        public bool OnlyTreeNodeChangeCanActiveNewData
        {
            get { return _onlyTreeNodeChangeCanActiveNewData; }
            set
            {
                if (value != _onlyTreeNodeChangeCanActiveNewData)
                {
                    _onlyTreeNodeChangeCanActiveNewData = value;
                    this.RaisePropertyChanged(() => this.OnlyTreeNodeChangeCanActiveNewData);
                }
            }
        }



        private Dictionary<int, Tuple<int, TmlNewData.TmlNewDataforOneLoop>> _historydata;

        public Dictionary<int, Tuple<int, TmlNewData.TmlNewDataforOneLoop>> HistoryData
        {
            get
            {
                if (_historydata == null)
                    _historydata = new Dictionary<int, Tuple<int, TmlNewData.TmlNewDataforOneLoop>>();
                return _historydata;
            }
        }



        private List<TmlNewData> _onlineRate;

        public List<TmlNewData> OnlineRate
        {
            get
            {
                if (_onlineRate == null)
                    _onlineRate = new List<TmlNewData>();
                return _onlineRate;
            }
        }



        ///// <summary>
        ///// 判断是否勾选“查看比对数据”和“查看详细数据”
        ///// </summary>
        //private void VisiAdvancedData(bool isHistory)
        //{
        //    //if (IsCompare && isHistory == false)
        //    //{
        //    //    IsCompareCheck = Visibility.Visible;
        //    //}
        //    //else IsCompareCheck = Visibility.Collapsed;

        //    //if (IsDetailed) IsDetailCheck = Visibility.Visible;
        //    //else IsDetailCheck = Visibility.Collapsed;
        //    //if (IsOnlineRate) IsOnlineRateCheck = Visibility.Visible;
        //    //else IsOnlineRateCheck = Visibility.Collapsed;

        //    SettingChangedx();
        //}

        private bool CanQueryData = false;
        private bool SelectOldData = false;

        public void ChangeVis()
        {
            IsShowFailVis = Visibility.Visible;
            
        }

        public  void OnSelectRtuIdChange(int rtuId, bool selected, bool showFail = false)
        {
            IsShowFailVis = showFail ? Visibility.Visible : Visibility.Collapsed;
            NewDataWidth = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 13, 600);//最新数据 宽度
            if (NewDataWidth < 100) NewDataWidth = 100;

            this.RtuIdNeedUpdate = rtuId;
            var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuIdNeedUpdate);
            if (run == null || run.RtuNewData == null)
            {
                OnDataChange(rtuId, null, "", false);
                return;
            }
            OnDataChange(rtuId, run.RtuNewData, "", false);
            if (TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(9))
            {

                if (_selectedDateTime == null)
                {
                    _selectedDateTime = new ObservableCollection<NameValueInt>();
                }

                _selectedDateTime.Clear();

                CanQueryData = true;
                SelectedDateForYes = DateTime.Now.AddDays(-1);

                //RequestDailyData(run.RtuNewData.DateCreate);
                //RequestNearData(run.RtuNewData.DateCreate.AddMinutes(-10).Ticks );
                //RequestHistoryData(rtuId, run.RtuNewData.DateCreate.AddDays(-1));
            }
            if (selected)
                Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
                    ViewIdAssign.TmlNewDataViewLeftId);
        }


        private RtuNewDataInfo CurrentShowTmlNewData = null;

        private bool blOtherViewShowData = false;
        private void OnOtherViewShowData(Wlst.client.TmlNewData data)
        {
            try
            {
                if (data.LstNewLoopsData.Count == 0) return;

                //CurrentShowTmlNewData = data;

                this.RtuIdNeedUpdate = data.RtuId;
                var fff = new RtuNewDataInfo(data);
                //if (fff.LstNewLoopsData == null) return;

                blOtherViewShowData = true;
                OnDataChange(data.RtuId, fff, "历史数据", true);
                Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
                    ViewIdAssign.TmlNewDataViewLeftId);
            }
            catch (Exception ex)
            {

            }
        }

        private int Get_TransferState(int rtuId)
        {
            var lst = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();

            if (lst.Count != 0)
            {
                foreach (var t in lst)
                {
                    if (t.Value.RtuId == rtuId)
                    {
                        return t.Value.IsYj;
                    }
                }
            }

            return 0;
        }

        //当前显示的设备信息   终端地址-呈现数据的时间-数据本身生成的时间
        private Tuple<int, long, long> _lastOnDataChangeRtuwithtime = new Tuple<int, long, long>(0, 0, 0);

        private void OnDataChange(int rtuId, RtuNewDataInfo fff, string attachinfo, bool isHistory)
        {
            int swOutCount = 6;
            OnDataChange1(rtuId, fff, isHistory,out swOutCount );
            OnDataChange2(rtuId, fff, isHistory,swOutCount );
        }

        //void OnDataChange(int rtuId, RtuNewDataInfo fff, string attachinfo, bool isHistory)
        //{

        //    Ax = "";
        //    Bx = "";
        //    Cx = "";
        //    SwitchOutInfo.Clear();



        //    VisiAdvancedData(isHistory);             

        //    this.TimeInfo = "";

        //    //与上次显示的终端地址相同
        //    if (rtuId == _lastOnDataChangeRtuwithtime.Item1)
        //    {
        //        if (fff != null && fff.DateCreate.Ticks == _lastOnDataChangeRtuwithtime.Item3)
        //            return;
        //    }
        //    _lastOnDataChangeRtuwithtime = new Tuple<int, long, long>(rtuId, DateTime.Now.Ticks, fff == null ? 0 : fff.DateCreate.Ticks);



        //    var anaPara = new Dictionary<int, Tuple<int,bool,double>>();             
        //    var looxInfo = new ObservableCollection<LoopInfoLeft>();
        //    Visifd = Visibility.Collapsed ;

        //    try
        //    {

        //        CurrentShowTmlNewData = fff;

        //        var rtuState = "";

        //        int phyId = 0;
        //        var tmpequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( rtuId);
        //        var tmpequ2 = tmpequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

        //        if (tmpequ != null)
        //        {
        //            rtuState = tmpequ.RtuStateCode  == 2 ? "使用" : tmpequ.RtuStateCode  == 1 ? "停运" : "不用";
        //            //this.RtuName = tmpequ.RtuName;
        //            phyId = tmpequ.RtuPhyId ;
        //        }
        //        if(tmpequ2 !=null && tmpequ2.WjLoops !=null)
        //        {
        //            foreach(var t in tmpequ2.WjLoops)
        //            {
        //                anaPara.Add(t.Value.LoopId, new Tuple<int, bool,double>( t.Value.MutualInductorRatio,t.Value.IsShieldLoop,t.Value.ShieldLittleA ));
        //                //GetAnaPara(anaPara);
        //            }
        //        }


        //        //终端名称下的  历史 是否显示
        //        string nameAtt = string.Empty;
        //        if (isHistory) nameAtt = "历史";
        //        else
        //        {
        //            if (Get_TransferState(rtuId) == 2)
        //                nameAtt = "未移交";
        //        }
        //        AssetNameVisi = string.IsNullOrEmpty(nameAtt) ? Visibility.Collapsed : Visibility.Visible;




        //        this.RtuIdNeedUpdate = rtuId;
        //        this.RtuId = rtuId;

        //        var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuIdNeedUpdate);
        //        int rtuTemp = 0;
        //        Tuple<int, int> onlineRate = new Tuple<int, int>(-1,1);


        //        if (run != null && run.RtuNewData != null)
        //        {
        //            rtuTemp = run.RtuNewData.RtuTemperature;
        //            onlineRate = new Tuple<int, int>(run.RtuNewData.TimesBackPartolIn24Hour,
        //                                             run.RtuNewData.TimesPartolIn24Hour);


        //            foreach (var t in run.RtuNewData.LstNewLoopsData)
        //            {
        //                foreach (var f in fff.LstNewLoopsData)
        //                {
        //                    if (t.LoopId == f.LoopId )
        //                    {
        //                        f.AvgOf7daysA = t.AvgOf7daysA;
        //                        break;
        //                    }
        //                }
        //            }
        //        }




        //        var rtuName = tmpequ != null ? tmpequ.RtuName : "";
        //        string GroupName = "";
        //        string AreaName = "";
        //        string Error = "";
        //        string color2 = "";
        //        var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuId);
        //        AreaName = (UserInfo.UserLoginInfo.D == false &&UserInfo.UserLoginInfo.AreaR.Count <2) ? "": Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
        //        var lst = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values;
        //        foreach(var x in lst)
        //        {
        //            if (x.RtuId == rtuId)
        //            {
        //                Error =  x.FaultName;

        //            }

        //        }


        //        var groupidx =
        //            Wlst.Sr.EquipmentInfoHolding .Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(RtuId);//.Item2;
        //        //Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(RtuId);
        //        if (groupidx != null )
        //        {
        //            var infosss =
        //                Wlst.Sr.EquipmentInfoHolding .Services.ServicesGrpSingleInfoHold.GetGroupInfomation(groupidx .Item1 ,groupidx .Item2 );
        //            if (infosss != null) GroupName = infosss.GroupName+"  "+ Error;
        //        }


        //        //  var fff = Sr.EquipmentNewData.Services.RtuNewDataService.GetInfoById(rtuId);
        //        if (fff == null)
        //        {

        //            //IsDataVisi = Visibility.Collapsed;
        //            //this.LineItemss.Clear();
        //            //this.TextBlockInfoItemss.Clear();
        //            this.LoopxInfo.Clear();
        //          //  this.SumInfo = "";
        //            this.TimeInfo = "";
        //            //CanWidth = 355 + LoopNameLength + TimeNameLength + 4*VaNameLength;
        //            //CanHeight = 8*RowHeight + 65;
        //            this.RtuId = rtuId;
        //            //this.RtuName = this.RtuId + " - " + rtuName ;
        //            this.AddBasicRtuInfo(phyId, rtuName + "  " + AreaName + "  " + GroupName,
        //                                 " " + "  " + rtuState, rtuTemp, nameAtt, "Red");


        //            //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
        //            var swuout = new List<Tuple<int, bool, int, string,string>>();
        //            //添加输出
        //            //lvf
        //            ConstColor = GetColor();
        //            if (tmpequ2.RtuModel == EnumRtuModel.Wj3006)
        //            {
        //                for (int i = 1; i < 9; i++)
        //                {
        //                    swuout.Add(new Tuple<int, bool, int, string,string>(i, false, 0, ConstColor[i],""));
        //                }
        //            }
        //            else
        //            {
        //                for (int i = 1; i < 7; i++)
        //                {
        //                    swuout.Add(new Tuple<int, bool, int, string,string>(i, false, 0, ConstColor[i],""));
        //                }
        //            }

        //            AddSitchOutInfo( rtuId, swuout);

        //           this.BulidMenus(rtuId);
        //            //RunDispatch(new Tuple<int, Action<Tuple<ObservableCollection<MenuItem>, string>>>(rtuId, UpdateMenu));
        //            return;
        //        }

        //        //var rtuState = "";
        //        //this.RtuName = this.RtuId+""
        //        var title = "";
        //        if (fff.Alarms.ContainsKey(1) && fff.Alarms[1]) title += "停电";
        //        else title += "供电";
        //        if (fff.Alarms.ContainsKey(3) && fff.Alarms[3]) title += "停运中";
        //        else title += "使用中 ";
        //        if (tmpequ != null)
        //        {
        //            rtuState = tmpequ.RtuStateCode  == 0 ? "不用" : title;
        //            //this.RtuName = tmpequ.RtuName;
        //        }

        //        if (fff.LstNewLoopsData == null)
        //        {

        //            this.LoopxInfo.Clear();
        //            this.TimeInfo = "";

        //            //CanWidth = 365 + LoopNameLength + TimeNameLength + 4*VaNameLength;
        //            //CanHeight = 8*RowHeight + 65;
        //            this.RtuId = fff.RtuId;
        //            this.DateTimeGetRtuTime = fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
        //            //this.RtuName = this.RtuId + " - " + fff.RtuName;
        //            this.AddBasicRtuInfo(phyId, rtuName + "  " + AreaName + "  " + GroupName,
        //                                 fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "  " + rtuState, rtuTemp, nameAtt ,"Red");


        //            //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
        //            var swuout = new List<Tuple<int, bool, int, string,string>>();
        //            //添加输出
        //            ConstColor = GetColor(); //lvf
        //            for (int i = 1; i < 9; i++)
        //            {
        //                swuout.Add(new Tuple<int, bool, int, string,string>(i, false, 0, ConstColor[i],""));
        //            }
        //            AddSitchOutInfo(rtuId, swuout);

        //            //、、IsDataVisi = Visibility.Collapsed;
        //            //this.LineItemss = lineItems;
        //            //this.TextBlockInfoItemss = textBlockInfoItems;
        //            //this.TextBlock1InfoItemss = textBlock1InfoItems;
        //            //RunDispatch(new Tuple<int, Action<Tuple<ObservableCollection<MenuItem>, string>>>(rtuId, UpdateMenu));
        //             this.BulidMenus(rtuId);
        //            return;
        //        }

        //        //RequestHistoryData(rtuId, fff.DateCreate.AddDays(-1));
        //        //InitEventH();



        //        this.DateTimeGetRtuTime = fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");

        //        var dic = new Dictionary<int,Tuple<string, List<RtuNewDataLoopItem>>>();

        //        for (int i = 1; i < fff.IsSwitchOutAttraction.Count + 1; i++)
        //        {
        //            if (dic.ContainsKey(i)) continue;
        //            dic.Add(i, new Tuple<string, List<RtuNewDataLoopItem>>("",new List<RtuNewDataLoopItem>( )));

        //        }
        //        foreach (var t in fff.LstNewLoopsData)
        //        {
        //            if (!dic.ContainsKey(t.SwitchOutId)) dic.Add(t.SwitchOutId, new Tuple<string, List<RtuNewDataLoopItem>>("", new List<RtuNewDataLoopItem>()));
        //            dic[t.SwitchOutId].Item2.Add( t);
        //        }




        //        this.AddBasicRtuInfo(phyId,rtuName + "  " + AreaName + "  " + GroupName,
        //                             fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "  " + rtuState, rtuTemp, nameAtt ,"Red");


        //        //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
        //        List<Tuple<int, bool, int, string,string>> swout = new List<Tuple<int, bool, int, string,string>>();


        //        //添加输出
        //        var fffff = (from t in dic orderby t.Key select t).ToList();
        //        ConstColor = GetColor(); 
        //        foreach (var t in fffff)
        //        {
        //            if (t.Key < 1) continue;
        //            bool isclose = true;
        //            if (fff.IsSwitchOutAttraction.Count >= t.Key)
        //            {
        //                isclose = fff.IsSwitchOutAttraction[t.Key - 1];
        //            }
        //            //lvf
        //            string a = "";
        //            foreach (var x in tmpequ2.WjSwitchOuts)
        //            {
        //                if(x.Key == t.Key )
        //                {
        //                    a = x.Value.SwitchName;
        //                }


        //            }
        //            swout.Add(new Tuple<int, bool, int, string,string>(t.Key, isclose, t.Value.Item2 .Count, ConstColor[t.Key],a));

        //        }
        //        AddSitchOutInfo(rtuId, swout);


        //        //添加回路


        //        var dicattach = new Dictionary<int, bool >();
        //        var strinfoxfds = ShowDw ? "[A]" : "";

        //        Ax = "A相:" + string.Format("{0:0.00}", fff.RtuCurrentSumA) + strinfoxfds;
        //        Bx = "B相:" + string.Format("{0:0.00}", fff.RtuCurrentSumB) + strinfoxfds;
        //        Cx = "C相:" + string.Format("{0:0.00}", fff.RtuCurrentSumC) + strinfoxfds;


        //        if (tmpequ != null)
        //        {
        //            foreach (var g in tmpequ.EquipmentsThatAttachToThisRtu)
        //            {
        //                var attrtuInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
        //                if (attrtuInfo == null || attrtuInfo.EquipmentType != WjParaBase.EquType.Ldu) continue;
        //                var wjldu = attrtuInfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj1090Ldu;
        //                if (wjldu == null) continue;
        //                foreach (var f in wjldu.WjLduLines)
        //                {
        //                    if (f.Value.IsUsed == false) continue;
        //                    var errors =
        //                        (from t in
        //                             Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
        //                         where
        //                             t.Value.RtuId == g && t.Value.LoopId == f.Value.LduLineId &&
        //                             t.Value.IsThisUserShow
        //                         select t).ToList().Count;


        //                        if (dicattach.ContainsKey(f.Value.LduLoopId) == false)
        //                            dicattach.Add(f.Value.LduLoopId, errors > 0);

        //                }
        //            }
        //        }
        //        ConstColor = GetColor();



        //        foreach (var t in fffff)
        //        {
        //            if (t.Key < 1) continue;                   
        //            string color = ConstColor[t.Key];
        //            foreach (var g in t.Value.Item2)
        //            {

        //                double v = 0;
        //                double a = 0;
        //                double power = 0;
        //                double rate = 0;
        //                string upper = "";
        //                string lower = "";
        //                double refA = 0;

        //                    rate = g.BrightRate;
        //                try
        //                {
        //                    v = Convert.ToDouble(g.V);
        //                    a = Convert.ToDouble(g.A);
        //                    power = Convert.ToDouble(g.Power);
        //                    refA = Convert.ToDouble(g.AvgOf7daysA );
        //                    upper = g.Upper.ToString("f2");
        //                    lower = g.Lower.ToString("f2");                           


        //                }
        //                catch (Exception ex)
        //                {
        //                }
        //                ;

        //                if (dicattach.ContainsKey(g.LoopId))
        //                {

        //                        AddLoopInfo(ref looxInfo,
        //                                    g.LoopId, g.LoopName, g.BolSwitchInState, v, a,
        //                                    power,
        //                                    rate, upper, lower, anaPara, refA, onlineRate, color, g.Range > 0,
        //                                    isHistory, dicattach[g.LoopId] ? "被盗" : "正常",
        //                                    dicattach[g.LoopId] ? "Red" : color);


        //                }
        //                if (!dicattach.ContainsKey(g.LoopId))
        //                {
        //                    AddLoopInfo(ref looxInfo,
        //                               g.LoopId, g.LoopName, g.BolSwitchInState, v, a,
        //                               power,
        //                               rate, upper, lower, anaPara, refA, onlineRate, color, g.Range > 0, isHistory, " ", color);

        //                }

        //            }

        //        }



        //        //添加输入 入门
        //        foreach (var t in fffff)
        //        {
        //            if (t.Key != 0) continue;
        //            var loopDetect = new List<Tuple<string,string>>();
        //            var loopDetect2 = new List<string>();
        //            foreach (var g in t.Value.Item2)
        //            {
        //                string tmpName = "";
        //                if (g.LoopName.Contains("门")) continue;
        //                if (g.LoopName.Contains("A"))
        //                {
        //                    loopDetect.Add(new Tuple<string, string>( g.LoopName.Split('A')[0],g.LoopName.Split('A')[1]));
        //                    tmpName = g.LoopName.Split('A')[1];
        //                }
        //                if (g.LoopName.Contains("防盗"))
        //                {
        //                    loopDetect2.Add(g.LoopName.Substring(0,g.LoopName.IndexOf("防盗")));
        //                    tmpName = g.LoopName.Substring(0, g.LoopName.IndexOf("防盗"));
        //                }
        //                if (g.LoopName.Contains("B") && loopDetect.Contains(new Tuple<string, string>( g.LoopName.Split('B')[0],g.LoopName.Split('B')[1]))) continue;
        //                if(g.LoopName.Contains("检测器") &&loopDetect2.Contains(g.LoopName.Substring(0,g.LoopName.IndexOf("检测器")))) continue;
        //                double v = 0;
        //                double a = 0;
        //                double power = 0;
        //                double rate = 0;
        //                string upper = "";
        //                string lower = "";
        //                double refA = 0;
        //                string color3 = "Grey";
        //                string status = "正常";
        //                foreach(var x in lst)
        //                {                             
        //                    if (x.RtuId == rtuId && x.RtuLoopName.Contains(tmpName))
        //                    {
        //                        color3 = "Red";
        //                        status = x.FaultName.Length < 3 ? x.FaultName : "报警";
        //                    }                             
        //                }
        //                try
        //                {
        //                    v = Convert.ToDouble(g.V);
        //                    a = Convert.ToDouble(g.A);
        //                    power = Convert.ToDouble(g.Power);
        //                    refA = Convert.ToDouble(g.AvgOf7daysA);
        //                    upper = g.Upper.ToString("f2");
        //                    lower = g.Lower.ToString("f2");
        //                    rate = g.BrightRate;
        //                }
        //                catch (Exception ex)
        //                {
        //                }

        //                    AddSwitchInInfo(ref looxInfo, g.LoopId, g.LoopName, status,
        //                                    color3);



        //            }

        //            var tmps = (from g in t.Value.Item2 orderby g.LoopId select g).ToList();
        //            for (int i = 0; i < tmps.Count; i++)
        //            {

        //                if (tmps[i].LoopName.Contains("门"))
        //                {
        //                    var tmpssss = "正常";
        //                    if (tmps[i].BolSwitchInState == false) tmpssss = "打开";
        //                    AddSwitchInInfo(ref looxInfo, tmps[i].LoopId,

        //                                    tmps[i].LoopName, tmpssss,
        //                                    tmps[i].BolSwitchInState ? "Gray" : "Red");
        //                }


        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog.WriteLogError("On NewDataView change rtu error:" + ex);
        //    }

        //   // this.LoopxInfo = looxInfo;
        //    UpdateLoopxInfo(looxInfo);
        //    this.BulidMenus(rtuId);

        //}


        private void OnDataChange1(int rtuId, RtuNewDataInfo fff, bool isHistory,out int swoutichoutcount)
        {
            swoutichoutcount = 6;
            var tmpequx = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuId);
            if (tmpequx == null) return;
            var equinfo = tmpequx as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
            if (equinfo == null) return;

            this.RtuId = rtuId;


            Ax = "";
            Bx = "";
            Cx = "";
            SwitchOutInfo.Clear();
            this.TimeInfo = "";
            
            if(fff !=null    )
            {
                if (blOtherViewShowData ==false  )
                {
                    SelectedDateForYes = fff.DateCreate.AddDays(-1);    
                }else
                {
                    blOtherViewShowData = false;
                }

            }


            try
            {

                //终端名称下的  历史 是否显示
                string nameAtt = string.Empty;
                if (isHistory) nameAtt = "历史";
                else
                {
                    if (Get_TransferState(rtuId) == 2)
                        nameAtt = "未移交";
                }
                AssetNameVisi = string.IsNullOrEmpty(nameAtt) ? Visibility.Collapsed : Visibility.Visible;


                var rtuState = equinfo.RtuStateCode == 2 ? "使用" : equinfo.RtuStateCode == 1 ? "停运" : "不用";
                var phyId = equinfo.RtuPhyId;





                //获取终端名称、分组名称
                var rtuName = equinfo.RtuName;
                string groupName = string.Empty;

                var groupidx =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(rtuId);
                if (groupidx != null)
                {
                    var infosss =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
                            groupidx.Item1, groupidx.Item2);
                    if (infosss != null) groupName = infosss.GroupName;
                }

                var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuId);
                var areaName = (UserInfo.UserLoginInfo.D == false && UserInfo.UserLoginInfo.AreaR.Count < 2)
                                   ? ""
                                   : Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;



                if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 15, 1) == 2)
                {
                    //将查询出来的第一个故障 显示在 第一列最后
                    var ntgx = (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
                                where t.Value.RtuId == rtuId
                                select t.Value).FirstOrDefault();
                    if (ntgx != null)
                    {
                        groupName = groupName + "  " + ntgx.FaultName;
                    }
                }
                else if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 15, 1) == 3)
                {
                    groupName = groupName + "  " + tmpequx.RtuRemark;
                }



                if (fff == null)
                {
                    this.AddBasicRtuInfo(phyId, rtuName + "  " + areaName + "  " + groupName,
                                         "--  " + rtuState, 0, nameAtt, "Red");

                }
                else
                {

                    var title = "";
                    bool powerOff = true ;
                    if (fff.LstNewLoopsData == null ) title += "正常";
                    else
                    {
                        int xcoutn = 0;
                        foreach (var g in fff.LstNewLoopsData)
                        {

                            xcoutn++;
                            if (g.V > 1)
                            {
                                powerOff = false;
                                break;
                            }
                        }
                        if (xcoutn == 0)
                        {
                            if (fff.Alarms.ContainsKey(1) && fff.Alarms[1]) title += "断电";
                            else title += "供电";
                        }
                        else
                        {
                            //if (fff.Alarms.ContainsKey(1) && fff.Alarms[1]) title += "停电";
                            if (powerOff) title += "停电";

                            else
                            {
                                if (fff.Alarms.ContainsKey(1) && fff.Alarms[1]) title += "断电";
                                else title += "供电";
                            }
                        }
                    }
                    if (fff.Alarms.ContainsKey(3) && fff.Alarms[3]) title += "停运中";
                    else title += "使用中 ";
                    rtuState = equinfo.RtuStateCode == 0 ? "不用" : title;


                    this.DateTimeGetRtuTime = fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                    this.AddBasicRtuInfo(phyId, rtuName + "  " + areaName + "  " + groupName,
                                         fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "  " + rtuState,
                                         fff.RtuTemperature,
                                         nameAtt, "Red");
                }


                //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
                var swuout = new List<Tuple<int, bool, int, string, string>>();
                //添加输出
                //lvf
                ConstColor = GetColor();

                for (int i = 1; i < 9; i++)
                {
                    if (i > 6)
                    {
                        if (equinfo.RtuModel == EnumRtuModel.Wj3005 || equinfo.RtuModel == EnumRtuModel.Wj3090)
                            continue;
                    }
                    bool isclose = false;
                    //lvf
                    string a = "";

                    if (fff != null && fff.IsSwitchOutAttraction != null)
                    {
                        if (fff.IsSwitchOutAttraction.Count >= i)
                        {
                            isclose = fff.IsSwitchOutAttraction[i - 1];
                        }

                        foreach (var x in equinfo.WjSwitchOuts)
                        {
                            if (x.Key == i)
                            {
                                a = x.Value.SwitchName;
                            }


                        }
                    }

                    swuout.Add(new Tuple<int, bool, int, string, string>(i, isclose, 0, ConstColor[i], a));
                }
                AddSitchOutInfo(rtuId, swuout);
                swoutichoutcount = swuout.Count;

                if (fff != null && Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 14, true))
                {

                    var isABC = true;
                    foreach (var f in fff.LstNewLoopsData)
                    {
                        if (f.LoopName.Contains("火线"))
                        {
                            isABC = false;
                            break;
                        }

                    }
                    


                    //      NewDataWidth = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 13, 600);//最新数据 宽度
                    //IsShowAbc=Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 14, true);//是否显示 ABC 电流
                    //是否显示单位
                    var strinfoxfds = ShowDw ? "[A]" : "";

                    //Ax = "A相:" + string.Format("{0:0.00}", fff.RtuCurrentSumA) + strinfoxfds;
                    //Bx = "B相:" + string.Format("{0:0.00}", fff.RtuCurrentSumB) + strinfoxfds;
                    //Cx = "C相:" + string.Format("{0:0.00}", fff.RtuCurrentSumC) + strinfoxfds;
                    if (isABC) //三相
                    {

                        double PA = 0.00, PB = 0.00, PC = 0.00;
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(RtuId))
                        {
                            var info =
                               Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId] as
                               Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                            foreach (var t in fff.LstNewLoopsData)
                            {
                                if (info != null && (t.IsLoop && info.WjLoops.ContainsKey(t.LoopId)))
                                {
                                    //屏蔽回路不参与累计   lvf 2018年9月11日08:51:25 老孟提议
                                    if (info.WjLoops[t.LoopId].IsShieldLoop == 1 || info.WjLoops[t.LoopId].IsShieldLoop == 2) continue;

                                    switch (info.WjLoops[t.LoopId].VoltagePhaseCode)
                                    {
                                        case Wlst.client.EnumVoltagePhase.Aphase:
                                            PA += t.Power;
                                            break;
                                        case Wlst.client.EnumVoltagePhase.Bphase:
                                            PB += t.Power;
                                            break;
                                        case Wlst.client.EnumVoltagePhase.Cphase:
                                            PC += t.Power;
                                            break;
                                    }
                                }
                            }
                        }
                        double totalPower = (fff.RtuVoltageA * fff.RtuCurrentSumA + fff.RtuVoltageB * fff.RtuCurrentSumB +
                                             fff.RtuVoltageC * fff.RtuCurrentSumC) / 1000;

                        var pn = (PA + PB + PC)/totalPower;
                        if (pn > 1) pn = 1;
                        Ax =  "A相:"  + string.Format("{0:0.00}", fff.RtuCurrentSumA) + strinfoxfds;
                        Bx =  "B相:" + string.Format("{0:0.00}", fff.RtuCurrentSumB) + strinfoxfds;
                        Cx =  "C相:" + string.Format("{0:0.00}", fff.RtuCurrentSumC) + strinfoxfds;

                        AA = " 总功" + string.Format("{0:0.00}", PA + PB + PC);

                        AB = " 总因" + (totalPower != 0 ? string.Format("{0:0.00}", pn) : "--");//(PA + PB + PC) / totalPower
                    }
                    else //火零
                    {
                        double FireCur = 0.00, ZeroCur = 0.00,AllPower =0.00,AllUI=0.00;
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(RtuId))
                        {
                            var info =
                               Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId] as
                               Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                            foreach (var t in fff.LstNewLoopsData)
                            {
                                if (info != null && (t.IsLoop && info.WjLoops.ContainsKey(t.LoopId)))
                                {

                                    //屏蔽回路不参与累计   lvf 2018年9月11日08:51:25 老孟提议
                                    if (info.WjLoops[t.LoopId].IsShieldLoop == 1 || info.WjLoops[t.LoopId].IsShieldLoop == 2) continue;

                                    if(t.LoopName.Contains("零线") ==false)
                                    {
                                        FireCur += t.A;
                                        AllPower += t.Power;
                                        AllUI += t.A * t.V;
                                        
                                    }
                                    else
                                    {
                                        ZeroCur += t.A;
                                    }
                                    
                                }
                            }
                        }
                        var powerNum = AllPower * 1000 / AllUI;
                        if (powerNum > 1) powerNum = 1;
                        Ax = "火总" + string.Format("{0:0.00}", FireCur) + strinfoxfds;
                        Bx = "零总" + string.Format("{0:0.00}", ZeroCur) + strinfoxfds;
                        Cx = "";

                        AA = " 总功" + string.Format("{0:0.00}", AllPower);

                        AB = " 总因" + (AllUI != 0 ? string.Format("{0:0.00}", powerNum) : "--");//AllPower * 1000 / AllUI
                    }
                    //Ax = (isABC ? "A相:" : "火总") + string.Format("{0:0.00}", fff.RtuCurrentSumA) + strinfoxfds;
                    //Bx = (isABC ? "B相:" : "零总") + string.Format("{0:0.00}", fff.RtuCurrentSumB) + strinfoxfds;
                    //Cx = isABC ? ("C相:" + string.Format("{0:0.00}", fff.RtuCurrentSumC) + strinfoxfds) : "";

                    //AA = " 总功" + string.Format("{0:0.00}", PA + PB + PC);

                    //AB = " 总因" + (totalPower != 0 ? string.Format("{0:0.00}", (PA + PB + PC) / totalPower) : "--");
                    //string.Format("{0:0.00}", v)

                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("On NewDataView change rtu error:" + ex);
            }
            BulidMenus(rtuId);
        }

        private void OnDataChange2(int rtuId, RtuNewDataInfo fff, bool isHistory,int swOutCount)
        {
            if (fff == null)
            {
                UpdateLoopxInfo(new ObservableCollection<LoopInfoLeft>(),swOutCount );
                return;
            }
            var looxInfo = new ObservableCollection<LoopInfoLeft>();

            //VisiAdvancedData(isHistory);
            var anaPara = new Dictionary<int, Tuple<int, int, double>>();
            Visifd = Visibility.Collapsed;

            try
            {

                CurrentShowTmlNewData = fff;

                var tmpequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuId);
                var tmpequ2 = tmpequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                if (tmpequ2 != null && tmpequ2.WjLoops != null)
                {
                    foreach (var t in tmpequ2.WjLoops)
                    {
                        anaPara.Add(t.Value.LoopId,
                                    new Tuple<int, int, double>(t.Value.MutualInductorRatio, t.Value.IsShieldLoop,
                                                                 t.Value.ShieldLittleA));
                    }
                }


                var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuIdNeedUpdate);
                Tuple<int, int> onlineRate = new Tuple<int, int>(-1, 1);
                if (run != null && run.RtuNewData != null)
                {
                    onlineRate = new Tuple<int, int>(run.RtuNewData.TimesBackPartolIn24Hour,
                                                     run.RtuNewData.TimesPartolIn24Hour);


                    foreach (var t in run.RtuNewData.LstNewLoopsData)
                    {
                        foreach (var f in fff.LstNewLoopsData)
                        {
                            if (t.LoopId == f.LoopId)
                            {
                                f.AvgOf7daysA = t.AvgOf7daysA;
                                break;
                            }
                        }
                    }
                }


                var dic = new Dictionary<int, Tuple<string, List<RtuNewDataLoopItem>>>();
                for (int i = 1; i < fff.IsSwitchOutAttraction.Count + 1; i++)
                {
                    if (dic.ContainsKey(i)) continue;
                    dic.Add(i, new Tuple<string, List<RtuNewDataLoopItem>>("", new List<RtuNewDataLoopItem>()));

                }
                foreach (var t in fff.LstNewLoopsData)
                {
                    if (!dic.ContainsKey(t.SwitchOutId))
                        dic.Add(t.SwitchOutId,
                                new Tuple<string, List<RtuNewDataLoopItem>>("", new List<RtuNewDataLoopItem>()));
                    dic[t.SwitchOutId].Item2.Add(t);
                }

                //添加回路
                var dicattach = new Dictionary<int, bool>();
                var dicattachName = new Dictionary<int, string>();

                //添加输出
                var fffff = (from t in dic orderby t.Key ascending select t).ToList();
                if (tmpequ != null)
                {
                    foreach (var g in tmpequ.EquipmentsThatAttachToThisRtu)
                    {
                        var attrtuInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                        if (attrtuInfo == null || attrtuInfo.EquipmentType != WjParaBase.EquType.Ldu) continue;
                        var wjldu = attrtuInfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj1090Ldu;
                        if (wjldu == null) continue;
                        foreach (var f in wjldu.WjLduLines)
                        {
                            if (f.Value.IsUsed == false) continue;
                            var errors =
                                (from t in
                                     Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
                                 where
                                     t.Value.RtuId == g && t.Value.LoopId == f.Value.LduLineId &&
                                     t.Value.IsThisUserShow
                                 select t).ToList().Count;


                            if (dicattach.ContainsKey(f.Value.LduLoopId) == false)
                            {
                                dicattach.Add(f.Value.LduLoopId, errors > 0);
                                dicattachName.Add(f.Value.LduLoopId, f.Value.LduLineName);
                            }
                        }
                    }
                }
                ConstColor = GetColor();


                foreach (var t in fffff)
                {
                    if (t.Key < 1) continue;
                    string color = ConstColor[t.Key];
                    var ntgs = (from f in t.Value.Item2 orderby f.LoopId ascending select f).ToList();
                    foreach (var g in ntgs)
                    {

                        double v = g.V;
                        double a = g.A;
                        double power = g.Power;
                        double rate = 0;
                        string upper = g.Upper + "";
                        string lower = g.Lower + "";
                        double refA = g.AvgOf7daysA;
                        rate = g.BrightRate;


                        if (dicattach.ContainsKey(g.LoopId))
                        {

                            AddLoopInfox(ref looxInfo, g.LoopId, g.LoopName, g.BolSwitchInState, v, a, power,
                                         rate, upper, lower, anaPara, refA, onlineRate, color, g.Range > 0,
                                         isHistory, dicattach[g.LoopId] ? "被盗" : "正常", dicattachName[g.LoopId],
                                         dicattach[g.LoopId] ? "Red" : color);


                        }
                        else
                        {
                            AddLoopInfox(ref looxInfo, g.LoopId, g.LoopName, g.BolSwitchInState, v, a, power,
                                        rate, upper, lower, anaPara, refA, onlineRate, color, g.Range > 0,
                                        isHistory, " ", " ", color);

                        }
                    }

                }



                //添加输入 入门
                foreach (var t in fffff)
                {
                    if (t.Key != 0) continue;
                    var loopDetect = new List<Tuple<string, string>>();
                    var loopDetect2 = new List<string>();
                    foreach (var g in t.Value.Item2)
                    {
                        string tmpName = "";
                        if (g.LoopName.Contains("门")) continue;
                        if (g.LoopName.Contains("A"))
                        {
                            loopDetect.Add(new Tuple<string, string>(g.LoopName.Split('A')[0], g.LoopName.Split('A')[1]));
                            tmpName = g.LoopName.Split('A')[1];
                        }
                        if (g.LoopName.Contains("防盗"))
                        {
                            loopDetect2.Add(g.LoopName.Substring(0, g.LoopName.IndexOf("防盗")));
                            tmpName = g.LoopName.Substring(0, g.LoopName.IndexOf("防盗"));
                        }
                        if (g.LoopName.Contains("B") &&
                            loopDetect.Contains(new Tuple<string, string>(g.LoopName.Split('B')[0],
                                                                          g.LoopName.Split('B')[1]))) continue;
                        if (g.LoopName.Contains("检测器") &&
                            loopDetect2.Contains(g.LoopName.Substring(0, g.LoopName.IndexOf("检测器")))) continue;


                        string color3 = "Gray";
                        string status = "正常";


                        var spsss =
                            (from trx in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
                             where trx.Value.RtuId == rtuId && trx.Value.RtuLoopName.Contains(tmpName)
                             select trx.Value.FaultName).FirstOrDefault();
                        if (string.IsNullOrEmpty(spsss) == false)
                        {
                            color3 = "Red";
                            status = spsss.Length < 3 ? spsss : "报警";
                        }
                        AddSwitchInInfo(ref looxInfo, g.LoopId, g.LoopName, status,
                                        color3);

                    }

                    var tmps = (from g in t.Value.Item2 orderby g.LoopId select g).ToList();
                    for (int i = 0; i < tmps.Count; i++)
                    {

                        if (tmps[i].LoopName.Contains("门"))
                        {
                            var tmpssss = "正常";
                            if (tmps[i].BolSwitchInState == false) tmpssss = "打开";
                            AddSwitchInInfo(ref looxInfo, tmps[i].LoopId,

                                            tmps[i].LoopName, tmpssss,
                                            tmps[i].BolSwitchInState ? "Gray" : "Red");
                        }


                    }
                }
              

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("On NewDataView change rtu error:" + ex);
            }

            // this.LoopxInfo = looxInfo;
            UpdateLoopxInfo(looxInfo,swOutCount );
            this.BulidMenus(rtuId);

        }


        public event EventHandler<EventArsgLoopCount> LoopCountChanged;
        private void UpdateLoopxInfo(ObservableCollection<LoopInfoLeft> data, int souCount)
        {

            TimeYesterday = "--";

            var canheihg = 126;

            if (souCount > 6)
            {
                canheihg = 148;
            }

            // LoopxInfo.Clear();


            var dataheig = (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeadHeightt +
                           (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightt * data.Count;
            //if (data.Count < 14) dataheig += 7 - (data.Count/2)*1;

            //MaxHeiht = CanvasHeight + (int) Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeadHeightt +
            //           (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightt * data.Count + 10;
            UpdateShowHight(canheihg, dataheig);

            //  Wlst.Cr.CoreOne.OtherHelper.delay.Dealyx();
            //LoopxInfo = data;

            //int x = 0;
            //foreach (var f in data)
            //{
            //    LoopxInfo.Add(f);
            //    x++;
            //    if (x % 6 == 0) Wlst.Cr.CoreOne.OtherHelper.delay.Dealyx();// .ImageSourceHelper .
            //}


            if (_datar.Count == 0)
            {
                for (int i = 1; i < 57; i++)
                {
                    var tmp = new LoopInfoLeft(i) {Backgroundx = "Transparent"};
                    LoopxInfo.Add(tmp);
                    _datar.Add(i, tmp);
                }
            }




            foreach (var f in data)
            {
                if (_datar.ContainsKey(f.Indexr ))
                {
                    _datar[f.Indexr].A = f.A;
                    _datar[f.Indexr].AttachInfo = f.AttachInfo;
                    _datar[f.Indexr].BackgroundAttach = f.BackgroundAttach;
                    _datar[f.Indexr].RefA = f.RefA;
                    _datar[f.Indexr].YesterdayA = f.YesterdayA;
                    _datar[f.Indexr].Ratio = f.Ratio;
                    _datar[f.Indexr].BrightRate = f.BrightRate;
                    _datar[f.Indexr].OnlineRate = f.OnlineRate;
                    _datar[f.Indexr].V = f.V;
                    _datar[f.Indexr].Power = f.Power;
                    _datar[f.Indexr].PowerFactor = f.PowerFactor;
                    _datar[f.Indexr].YesterdayP = f.YesterdayP;

                    _datar[f.Indexr].SwitchInState = f.SwitchInState;
                    _datar[f.Indexr].isShieldLoop = f.isShieldLoop;
                    _datar[f.Indexr].ShieldLoopMarkx = f.ShieldLoopMarkx;
                    _datar[f.Indexr].BackgroundShield = f.BackgroundShield;
                    _datar[f.Indexr].Upper = f.Upper;
                    _datar[f.Indexr].LoopName = f.LoopName;
                    _datar[f.Indexr].Lower = f.Lower;
                    _datar[f.Indexr].Backgroundx = f.Backgroundx;

                    _datar[f.Indexr].IsRed = f.IsRed;
                    _datar[f.Indexr].LoopId  = f.LoopId ;

                    _datar[f.Indexr].YesterdayA = "--";
                    _datar[f.Indexr].YesterdayP = "--";
                    _datar[f.Indexr].YesterdaySwitchin = "--";
                    _datar[f.Indexr].YesterdaySwitchinColor = "--";
                    _datar[f.Indexr].YesterdayV = "--";

                    _datar[f.Indexr].AttachInfoName = f.AttachInfoName;

                }
                else
                {
                    _datar[f.Indexr].A = "--";
                    _datar[f.Indexr].AttachInfo = "--";
                    _datar[f.Indexr].BackgroundAttach = "--";
                    _datar[f.Indexr].RefA = "--";
                    _datar[f.Indexr].YesterdayA = "--";
                    _datar[f.Indexr].Ratio = "--";
                    _datar[f.Indexr].BrightRate = "--";
                    _datar[f.Indexr].OnlineRate = "--";
                    _datar[f.Indexr].V = "--";
                    _datar[f.Indexr].Power = "--";
                    _datar[f.Indexr].PowerFactor = "--";
                    _datar[f.Indexr].YesterdayP = "--";

                    _datar[f.Indexr].SwitchInState = "--";
                    // _datar[f.LoopId].isShieldLoop = "--";
                    _datar[f.Indexr].Upper = "--";
                    _datar[f.Indexr].LoopName = "--";
                    _datar[f.Indexr].Lower = "--";
                    _datar[f.Indexr].Backgroundx = "Transparent";

                    _datar[f.Indexr].ShieldLoopMarkx = "--";
                    _datar[f.Indexr].BackgroundShield = "Transparent";

                    _datar[f.Indexr].LoopId  = f.LoopId ;
                    //_datar[f.LoopId].IsRed = f.IsRed;

                    

                    _datar[f.Indexr].YesterdayA = "--";
                    _datar[f.Indexr].YesterdayP = "--";
                    _datar[f.Indexr].YesterdaySwitchin = "--";
                    _datar[f.Indexr].YesterdaySwitchinColor = "Transparent";
                    _datar[f.Indexr].YesterdayV = "--";

                    _datar[f.Indexr].AttachInfoName = f.AttachInfoName;
                }
            }




            //dicDesc.Add(201, "[[1]]最新数据显示电压相位");
            //dicDesc.Add(202, "[[1]]最新数据模式1下不显示无回路的开关量输出");
            //dicDesc.Add(203, "[[1]]最新数据模式1下不显示未绑定时间表的输出");
            IsPhaseVisible = false;
            if (Wlst.Cr.CoreMims.Services.SystemOptionSvr.GetBoolean(Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.NewDataSetting.NewDataSettingViewModel.Moduleid, 201, false))
            {
                int rtuid = this.RtuId;
                var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);
                if (para != null)
                {

                    var rtupara = para as Wj3005Rtu;
                    if (rtupara != null)
                    {
                        var looppara = rtupara.WjLoops;
                        foreach (var l in _datar)
                        {
                            IsPhaseVisible = true;
                            if (looppara.ContainsKey(l.Value .LoopId) && looppara[l.Value.LoopId].SwitchOutputId > 0)
                            {
                                var p = "未知";
                                var tmp = looppara[l.Value.LoopId].VoltagePhaseCode;
                                if (tmp == EnumVoltagePhase.Aphase) p = "A相";
                                if (tmp == EnumVoltagePhase.Bphase) p = "B相";
                                if (tmp == EnumVoltagePhase.Cphase) p = "C相";
                                l.Value.Phase = p;

                            }
                            else
                            {
                                l.Value.Phase = "";
                            }
                        }
                    }
                }
            }


            if (LoopCountChanged != null)
            {
                LoopCountChanged(this, new EventArsgLoopCount()
                                           {
                                               LoopCount = data.Count,
                                               IsShowPro =
                                                   ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.
                                                   IsShowPropoery
                                           });
            }
        }



        private Dictionary<int, LoopInfoLeft> _datar = new Dictionary<int, LoopInfoLeft>();



        void UpdateShowHight(int canHeight, int dataHeight)
        {
            if (canHeight + dataHeight + 6 < _maxheightforset)
            {
                MaxHeiht = canHeight + dataHeight + 6;
                CanvasHeight = canHeight;
                GridViewDataHeiht = dataHeight;
                CanDataSc =ScrollBarVisibility.Disabled;
            }
            else
            {
                MaxHeiht = _maxheightforset;
                CanvasHeight = canHeight;
                GridViewDataHeiht =  _maxheightforset - canHeight - 30;
                CanDataSc = ScrollBarVisibility.Visible;
            }
            //GridViewDataHeiht = value - CanvasHeight - 3;
        }

        private ScrollBarVisibility _candataSc;
        public ScrollBarVisibility CanDataSc
        {
            get { return _candataSc; }
            set
            {
                if (value == _candataSc) return;
                _candataSc = value;
                this.RaisePropertyChanged(() => this.CanDataSc);
            }
        }

    }

    //扩展显示 指令发送 接收数据解析
    public partial class NewDataVmLeft
    {
        /// <summary>
        /// 发送命令  选测终端数据
        /// </summary>
        public void MeasureRtu()
        {
            try
            {
                if (RtuId < 1000000 || RtuId > 1100000) return;

                var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders;
                info.Args.Addr.Add(this.RtuId);
                info.WstRtuOrders.Op = 31;
                info.WstRtuOrders.RtuIds.Add(this.RtuId);
                SndOrderServer.OrderSnd(info);

                Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                    this.RtuId, RtuName, OperatrType.UserOperator, "选测终端");
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 发送命令 请求给定时间 昨天此刻的附近数据
        /// </summary>
        public void RequestNearData()
        {
            if (CurrentShowTmlNewData == null) return;
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_near_data;
            info.WstRtuData.DtEndTime = CurrentShowTmlNewData.DateCreate.Ticks;
            info.WstRtuData.DtStartTime = CurrentShowTmlNewData.DateCreate.Ticks;
            info.WstRtuData.RtuId = CurrentShowTmlNewData.RtuId;
            info.WstRtuData.Op = 2;
            SndOrderServer.OrderSnd(info, 10, 2);
        }


        /// <summary>
        /// 发送命令 请求给定时间 昨天此刻的附近数据
        /// </summary>
        public void RequestNearData(long dttime)
        {
            if (CurrentShowTmlNewData == null) return;
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_near_data;
            info.WstRtuData.DtEndTime = dttime;
            info.WstRtuData.DtStartTime = dttime;
            info.WstRtuData.RtuId = CurrentShowTmlNewData.RtuId;
            info.WstRtuData.Op = 2;
            SndOrderServer.OrderSnd(info, 10, 2);
        }

        public void RequestDailyData(DateTime dateTime)
        {
            if (CurrentShowTmlNewData == null) return;

            if (_selectedDateTime == null)
            {
                _selectedDateTime = new ObservableCollection<NameValueInt>();
            }

            _selectedDateTime.Clear();

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_data_1;
            info.WstRtuData.DtEndTime = (new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 0)).Ticks;
            info.WstRtuData.DtStartTime = (new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0)).Ticks;
            info.WstRtuData.RtuId = CurrentShowTmlNewData.RtuId;
            info.WstRtuData.Op = 2;
            SndOrderServer.OrderSnd(info);
        }

        /// <summary>
        /// 终端时钟达到
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        private void OnRtuTimeArrive(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var datax = infos.WstRtuOrders;
            if (datax == null) return;
            if (datax.Op != 22) return;
            if (datax.RtuIds[0] == this.RtuId)
            {
                var tmp = datax.Date.Substring(0, datax.Date.Length - 1);
                TimeInfo = _getInfoTimex + "时钟:" + tmp;
            }
        }


        /// <summary>
        /// 请求指定时刻的 终端历史数据
        /// </summary>
        /// <param name="rtuId"></param>
        /// <param name="datetime"></param>
        private void RequestHistoryData(int rtuId, DateTime datetime)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_ana_data;
            info.WstRtuAnaData.RtuId = rtuId;
            info.WstRtuAnaData.LoopId = 0;
            info.WstRtuAnaData.DtMiddleTime = datetime.Ticks;
            info.WstRtuAnaData.PreMinutes = 30;
            info.WstRtuAnaData.AfeMinutes = 30;
            info.WstRtuAnaData.Days = 0;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        //private Dictionary<int, Tuple<int,bool,double>> GetAnaPara(Dictionary<int, Tuple<int,bool,double>> a)
        //{
        //    return a;
        //}

        private void HistoryDataArrive(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos == null || infos.WstRtuAnaData == null) return;

            HistoryData.Clear();
            HistoryDataResponse = infos.WstRtuAnaData.RtuId;
            if (RtuId != HistoryDataResponse) return;

            foreach (var t in infos.WstRtuAnaData.Info)
            {

                foreach (var ttt in t.LstNewLoopsData)
                {
                    if (!HistoryData.ContainsKey(ttt.LoopId))
                        HistoryData.Add(ttt.LoopId, new Tuple<int, TmlNewData.TmlNewDataforOneLoop>(t.RtuId, ttt));
                }

            }

            if (LoopxInfo != null)
                foreach (var f in LoopxInfo)
                {

                    if (HistoryData.ContainsKey(f.LoopId))
                    {
                        f.YesterdayA = f.isShieldLoop == 0
                                           ? HistoryData[f.LoopId].Item2.A.ToString("f2")
                                           : "----";
                        f.YesterdayP = f.isShieldLoop == 0
                                           ? HistoryData[f.LoopId].Item2.Power.ToString("f2")
                                           : "----";
                    }






                }


        }

        private void RecordDataRequest(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (SelectOldData == true)
            {
                SelectOldData = false;
                if (infos == null || infos.WstRtuData == null || infos.WstRtuData.Items == null ||
                    infos.WstRtuData.Items.Count == 0) return;

                for (int i = 0; i < infos.WstRtuData.Items.Count; i++)
                {
                    if (infos.WstRtuData.Items[i].RtuTemperature != -100)
                    {
                        _selectedDateTime.Add(new NameValueInt()
                                                  {
                                                      Value = i,
                                                      Name =
                                                          new DateTime(infos.WstRtuData.Items[i].DateCreate).ToString(
                                                              "HH:mm:ss"),
                                                      DtName =
                                                          new DateTime(infos.WstRtuData.Items[i].DateCreate).ToString(
                                                              "yyyy-MM-dd HH:mm:ss")
                                                  });
                    }

                }

                if (_selectedDateTime.Count != 0)
                {
                    DateTime Xnow = DateTime.Now;
                    long nowTime = new DateTime(SelectedDateForYes.Year, SelectedDateForYes.Month, SelectedDateForYes.Day, Xnow.Hour, Xnow.Minute, Xnow.Second).Ticks;
                    long com = long.MaxValue;
                    int x = 0;
                    for (int i = 0; i < _selectedDateTime.Count; i++)
                    {
                        long time = (Convert.ToDateTime(_selectedDateTime[i].DtName)).Ticks;

                        if (Math.Abs(time - nowTime) < com)
                        {
                            com = Math.Abs(time - nowTime);
                            x = i;
                        }
                    }

                    CurSelectedDateTime = _selectedDateTime[x];
                }
                else
                {
                    foreach (var l in this.LoopxInfo)
                    {

                            l.YesterdayA = "--";
                            l.YesterdayP = "--";
                            l.YesterdaySwitchin = "--";
                            l.YesterdaySwitchinColor = "--";
                            l.YesterdayV = "--";

                    }
                }
            }
        }

        private void OnNearDataArrive(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (infos == null || infos.WstRtuData == null || infos.WstRtuData.Items == null ||
                infos.WstRtuData.Items.Count == 0) return;

            OnNearTwoDataArrive(infos.WstRtuData.Items[0]);
        }


        private void OnNearTwoDataArrive(Wlst.client.TmlNewData fff)
        {
            if (fff == null || fff.RtuId != RtuId) return;


            try
            {

                //loopInfox.Add(new LoopInfoLeft(loopId, name, vx, ax, powerx,
                //           used ? isShieldLoop == false ? string.Format("{0:0.00}", rate * 100) + " %" : "----" : "----",
                //           used ? isShieldLoop == false ? pws : "----" : "----",
                //           switchIsClose ? "吸合" : "断开", isHistory ? "" : attachInfo, color, attachcolor, referenceAx,
                //           upperx, lowerx, ratiox, onlineRate, switchIsClose ? "#CD0000" : "#000000", isShieldLoop));

                if (fff.RtuTemperature < 0)
                {
                    TimeYesterday ="  历史:"+ new DateTime(fff.DateCreate).ToString("yyyy-MM-dd HH:mm:ss") + " 无数据";

                    foreach (var l in this.LoopxInfo)
                    {

                            l.YesterdayA = "--";
                            l.YesterdayP = "--";
                            l.YesterdaySwitchin = "--";
                            l.YesterdaySwitchinColor = "--";
                            l.YesterdayV = "--";
                    }

                    return; //异常
                }


                TimeYesterday = "  历史:" + new DateTime(fff.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
                var meninfo = GetSwitchInInfoYeaterday();


                foreach (var l in this.LoopxInfo)
                {

                    foreach (var f in fff.LstNewLoopsData)
                    {

                        if (l.LoopId == f.LoopId)
                        {
                            if (meninfo.ContainsKey(l.LoopId))
                            {
                                l.YesterdayA = "--";
                                l.YesterdayP = "--";
                                l.YesterdaySwitchin = f.SwitchInState ? "正常" : "打开";
                                l.YesterdaySwitchinColor = f.SwitchInState == false ? "#CD0000" : "#000000";
                                l.YesterdayV = "--";
                            }
                            else
                            {


            // string vx = ShowDw ? string.Format("{0:0.00}", v) + "" : string.Format("{0:0.00}", v);
            //if (isShieldLoop == 1)
            //{
            //    if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 4) == false) vx = "----";
            //}
                           
            //if (isShieldLoop == 1||used ==false )
            //{
            //    if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 3) == false) ax = "----";
            //}

                                 
                                l.YesterdayA = l.isShieldLoop==1 &&  Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 3) == false ? "--" : f.A.ToString("f2");
                                l.YesterdayP = l.isShieldLoop==1 ? "--" : f.Power.ToString("f2");
                                l.YesterdaySwitchin = f.SwitchInState ? "吸合" : "断开";
                                l.YesterdaySwitchinColor = f.SwitchInState ? "#CD0000" : "#000000";
                                l.YesterdayV = l.isShieldLoop == 1 && Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 4) == false ? "--" : f.V.ToString("f2");

                            }
                        }
                    }
                }

                //RunDispatch(new Tuple<int, Action<Tuple<ObservableCollection<MenuItem>, string>>>(CurrentShowTmlNewData .RtuId , UpdateMenu));
            }
            catch (Exception ex)
            {

            }
        }

        Dictionary<  int  ,string  > GetSwitchInInfoYeaterday( )
        {

            
            var tmpequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById( RtuId  );
            var tmpequ2 = tmpequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

            if (tmpequ2 == null)
            {
                return new Dictionary<int, string>();
            }

  
            //添加输出
              var fffff = (from t in tmpequ2.WjLoops.Values where t.SwitchOutputId == 0 select t).ToList();



              var rtn = new Dictionary<int, string>();
              for (int i = 0; i < fffff.Count; i++)
              {
                  if (fffff[i].LoopName.Contains("门") || fffff[i].SwitchOutputId ==0)
                  {
                      rtn.Add(fffff[i].LoopId, fffff[i].LoopName);
                  }
                  //    var tmpssss = "正常";
                  //    if (tmps[i].BolSwitchInState == false) tmpssss = "打开";
                  //    AddSwitchInInfo(ref looxInfo, tmps[i].LoopId,

                  //                    tmps[i].LoopName, tmpssss,
                  //                    tmps[i].BolSwitchInState ? "Gray" : "Red");
                  //}


              }
              return rtn;
        }

    }

    //显示数据 回路 背景色  添加回路  输出
    public partial class NewDataVmLeft : Wlst.Cr.Core.CoreServices.ObservableObject
    {

    
        public static string[] ConstColor = GetColor();
        //K1-K8 回路背景色
        private static string[] GetColor()
        {

            var info = ZNewData.NewDataSetting.NewDataSettingViewModel.LoadNewDataLenghtSetConfgX();
            string[] myColor = new string[9]{
                                              info.Item7.Background,
                                              info.Item7.K1Background, 
                                              info.Item7.K2Background, 
                                              info.Item7.K3Background, 
                                              info.Item7.K4Background,
                                              info.Item7.K5Background ,
                                              info.Item7.K6Background,
                                              info.Item7.K7Background,
                                              info.Item7.K8Background,
                                          };
            return myColor;
        }
       
        #region def



     // public  int RtuId { get; set; }
        
        private ObservableCollection<LoopInfoLeft  > _lineInfox;
        //回路数据
        public ObservableCollection<LoopInfoLeft> LoopxInfo
        {
            get
            {
                if (_lineInfox == null) _lineInfox = new ObservableCollection<LoopInfoLeft>();
                return _lineInfox;
            }
            set
            {
                if (_lineInfox != value)
                {
                    _lineInfox = value;

                    this.RaisePropertyChanged(() => this.LoopxInfo);
                
                    if (value == null) return;

                    //IsDataVisi = value.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                    foreach (var f in value)
                    {
                        if (string.IsNullOrEmpty(f.AttachInfo)) continue;
                        Visifd = Visibility.Visible;
                        return;
                    }
                }
            }
        }


      

        
         private Visibility _linIsDataVVisifdisieInfox;
        //最新数据显示的 最后一列 防盗是否需要显示
        public Visibility Visifd
        {
            get { return _linIsDataVVisifdisieInfox; }
            set
            {
                if (_linIsDataVVisifdisieInfox != value)
                {
                    _linIsDataVVisifdisieInfox = value;
                    this.RaisePropertyChanged(() => this.Visifd);
                    if (VisiChanged != null) VisiChanged(value, new EventArgs());
                }
            }
        }

        public event EventHandler VisiChanged;
  
        
        
        #endregion



        private string _getInfoTimex = string.Empty;
        private void AddBasicRtuInfo(int phyid, string rtuName, string getInfoTime, int rtutemperature, string rtuNamebelowinfo, string color)
        {
            RtuIdPhy = phyid.ToString("d4");
            while (rtuName.StartsWith(" ")) rtuName = rtuName.Substring(1, rtuName.Length - 1);

          
            RtuName = rtuName;
            _getInfoTimex = getInfoTime;
            TimeInfo = getInfoTime;
            RtuNameAtt = rtuNamebelowinfo;




        }


        private ObservableCollection<SwitchOutLoopLeft> _switchOutInfo = null;
        public ObservableCollection<SwitchOutLoopLeft> SwitchOutInfo
        {
            get
            {
                if (_switchOutInfo == null) _switchOutInfo = new ObservableCollection<SwitchOutLoopLeft>();
                return _switchOutInfo;
            }
            set
            {
                if (value == _switchOutInfo) return;
                _switchOutInfo = value;
                this.RaisePropertyChanged(() => this.SwitchOutInfo);
            }
        }

        private bool _iIsPhaseVisiblesCompare;

        public bool IsPhaseVisible
        {
            get { return _iIsPhaseVisiblesCompare; }
            set
            {
                if (_iIsPhaseVisiblesCompare != value)
                {
                    _iIsPhaseVisiblesCompare = value;
                    this.RaisePropertyChanged(() => this.IsPhaseVisible);

                }
            }
        }


        /// <summary>
        /// 添加开关量输出信息
        /// </summary>
        /// <param name="swout">开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色</param>
        /// <param name="color">绘图前面部分绘图颜色 默认blue</param>
        private void AddSitchOutInfo(int rtuId, List<Tuple<int, bool, int, string, string>> swout, string color = "Blue")
        {
            ObservableCollection<SwitchOutLoopLeft> switchOutInrfo = new ObservableCollection<SwitchOutLoopLeft>();

            //UpdateMaxHeightWithCanvasHeight(swout.Count);
 

            //int loopsCount = 0;
            var areaid =
                Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuId);

            var isrtuinholidayinfo =
                Wlst.Sr.TimeTableSystem.Services.HolidayTimeandBandingServices.Myself.IsRtuInHoliday(areaid, rtuId);
            var holidayInfo = new List<string>();
            if (isrtuinholidayinfo)
                holidayInfo =
                    Wlst.Sr.TimeTableSystem.Services.HolidayTimeandBandingServices.Myself.
                        GetRtuSwitchOutOpenCloseTimeInholiday(areaid, rtuId);




            for (int i = 0; i < swout.Count; i++)
            {
                color = GetColorBySwitchOutId(swout[i].Item1);
  

                #region

                {
                    var timeInfo = "";
                    var toolinfo = "";
                    if (isrtuinholidayinfo)
                    {
                        if (holidayInfo != null && holidayInfo.Count > i) timeInfo = "假 " + holidayInfo[i];
                        else timeInfo = "假日时间未知";
                        toolinfo = "节假日开关灯.";
                    }
                    else
                    {
                        var name = "";
                        Wlst.Sr.TimeTableSystem.Models.TodayOpenCloseTime yesterday;

                        var rtuIdOrGrpId = rtuId;

                        //if (xtmp != null) rtuIdOrGrpId = xtmp.Item2;

                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId))
                        {
                            //var areaId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].AreaId;
                            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuId);
                            var tmp =
                                Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.
                                    GetTmlLoopBandTimeTableTodayOpenCloseTimex(areaId,
                                                                               rtuIdOrGrpId, swout[i].Item1);

                            if (tmp != null)
                            {

                                //var name = "";

                                if (tmp.TimeOnOff.Count > 0)
                                {

                                    if (tmp.TimeOnOff.Count == 1)
                                    {
                                        if (tmp.TimeOnOff[0].Item1 == 1500)
                                            name += " -- ";
                                        else
                                            name += string.Format("{0:D2}", tmp.TimeOnOff[0].Item1/60) + ":" +
                                                    string.Format("{0:D2}", tmp.TimeOnOff[0].Item1%60);
                                        name += " - ";

                                        if (tmp.TimeOnOff[0].Item2 == 1500)
                                            name += " -- ";
                                        else
                                            name += string.Format("{0:D2}", tmp.TimeOnOff[0].Item2/60) + ":" +
                                                    string.Format("{0:D2}", tmp.TimeOnOff[0].Item2%60);
                                    }
                                    else
                                    {
                                        //if (tmp.TimeTableName.Length >= 3)
                                        //    name += tmp.TimeTableName.Substring(0, 3);
                                        //else
                                        //    name += tmp.TimeTableName;
                                        //name += ": ";
                                        name += "多段开关灯";
                                    }
                                    var str = "";
                                    foreach (var OnOffTime in tmp.TimeOnOff)
                                    {
                                        if (OnOffTime.Item1 == 1500)
                                            str += " -- ";
                                        else
                                            str += string.Format("{0:D2}", OnOffTime.Item1/60) + ":" +
                                                   string.Format("{0:D2}", OnOffTime.Item1%60);
                                        str += " - ";

                                        if (OnOffTime.Item2 == 1500)
                                            str += " -- ";
                                        else
                                            str += string.Format("{0:D2}", OnOffTime.Item2/60) + ":" +
                                                   string.Format("{0:D2}", OnOffTime.Item2%60);

                                        str += " | ";
                                    }

                                    toolinfo = "今日操作:";
                                    if (str.Length == 0) toolinfo += "无;";

                                    toolinfo += str;
                                }
                                else
                                {
                                    name = " --  -  -- ";
                                    toolinfo = "今日操作:无;";

                                }

                                timeInfo = name;
                            }
                            else
                            {
                                timeInfo = " --  -  -- ";
                                toolinfo = "今日操作:无;";
                            }

                        }
                    }

                    #endregion

                    switchOutInrfo.Add(new SwitchOutLoopLeft(RtuId, swout[i].Item1, swout[i].Item5, color, swout[i].Item2,
                                                        timeInfo, toolinfo));


                }

                SwitchOutInfo = switchOutInrfo;
            }
        }



        /// <summary>
        /// 添加回路
        /// </summary>
        /// <param name="loopId">回路地址</param>
        /// <param name="name">回路名称 </param>
        /// <param name="switchIsClose">本回路开关量输入是否闭合</param>
        /// <param name="v">电压</param>
        /// <param name="a">电流</param>
        /// <param name="power">功率</param>
        /// <param name="rate">亮灯率</param>
        /// <param name="color">本回路颜色</param>
        /// <param name="used">回路是否使用中 量程是否为不为0 </param>
        /// <param name="attachInfo">附加显示信息 </param>
        private void AddLoopInfox(ref ObservableCollection<LoopInfoLeft > loopInfox, int loopId, string name,  bool switchIsClose, double v,
            double a, double power, double rate, string upper,string lower,Dictionary <int,Tuple<int,int,double>> anapara,
            double referencedata, Tuple<int, int> calonline, string color, bool used, bool isHistory, string attachInfo = null, string attachInfoName = null, string attachcolor = "000000")
        {
            
            var pws = "0.0";
            if(used && a >0 && v >0 && power >0)
            {
                var exr = power*1000/(v*a);
                if (exr > 1 && exr < 1.2) exr = 1;
                pws = string.Format("{0:0.00}", exr );
            }

            string yesterdayax= "";
            string yesterdaypx = "";
            string referenceAx = "";
            string ratiox = "";
            string onlineRate = "";
            int isShieldLoop = 0;
            double ShieldLittleA = 0.0;
            
           

            foreach(var t in anapara)
            {
                if(t.Key ==loopId )
                {
                    ratiox = t.Value.Item1 .ToString("f0")+"/5";
                    isShieldLoop = t.Value.Item2;
                    ShieldLittleA = t.Value.Item3;
                }
                
            }

            if (isShieldLoop == 2) return;//屏蔽并不显示 lvf


            referenceAx = referencedata.ToString("f2"); //isShieldLoop == false ? : "----"
            if (isShieldLoop == 1 && Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 3) == false) referenceAx = "----"; 

      

            
                if(calonline .Item2  !=0)
                {
                    onlineRate = isShieldLoop == 0 ? (calonline.Item1 * 100 / calonline.Item2).ToString("f2") + "%" : "----";// isShieldLoop == false ?: "----"; 
                }
                else
                {
                    onlineRate = "----";
                }
                
            string upperx =  upper;//isShieldLoop == false ?: "----";
            string lowerx = lower;//isShieldLoop == false ? : "----";




            //isShieldLoop = (isShieldLoop==1 || used)?1:0;
           
            //string vx = ShowDw ? string.Format("{0:0.00}", v) + "" : string.Format("{0:0.00}", v);
            //if(isShieldLoop ==1)
            //{
            //    if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 4) == false ) vx = "----";   //屏蔽回路是否显示电压
            //}
            ////string ax = ShieldLoopShAV? used ? (ShieldLittleA == 0.0 || a > ShieldLittleA) ? ShowDw ? string.Format("{0:0.00}", a) + "" : string.Format("{0:0.00}", a) : "<" + ShieldLittleA : "----":"----"; //isShieldLoop == false ?: "----";
            //string ax = (ShieldLittleA == 0.0 || a > ShieldLittleA)
            //                ? ShowDw ? string.Format("{0:0.00}", a) + "" : string.Format("{0:0.00}", a)
            //                : "<" + ShieldLittleA;
                          
            //if (isShieldLoop ==1 )
            //{
            //    if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 3) == false) ax = "----"; //屏蔽回路是否显示电流
            //}

            //string powerx = isShieldLoop == 0 ? ShowDw ? string.Format("{0:0.00}", power) + "" : string.Format("{0:0.00}", power)
            //                    : "----" ;





            string vx = ShowDw ? string.Format("{0:0.00}", v) + "" : string.Format("{0:0.00}", v);
            if (isShieldLoop == 1)
            {
                if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 4) == false) vx = "----";
            }
            //string ax = ShieldLoopShAV? used ? (ShieldLittleA == 0.0 || a > ShieldLittleA) ? ShowDw ? string.Format("{0:0.00}", a) + "" : string.Format("{0:0.00}", a) : "<" + ShieldLittleA : "----":"----"; //isShieldLoop == false ?: "----";
            //string ax = (ShieldLittleA == 0.0 || a > ShieldLittleA)
            //                      ? ShowDw ? string.Format("{0:0.00}", a) + "" : string.Format("{0:0.00}", a)
            //                      : "<" + ShieldLittleA;
            string ax = "";

            if ((ShieldLittleA == 0.0 || a > ShieldLittleA))
            {
                ax = ShowDw ? string.Format("{0:0.00}", a) + "A" : string.Format("{0:0.00}", a);
            }
            else
            {

                ax = a == 0 ? "0.00" : "<" + ShieldLittleA;
            }


                           
            if (isShieldLoop == 1||used ==false )
            {
                if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 3) == false) ax = "----";
            }

            string powerx = isShieldLoop == 0 ? used
                                ? ShowDw ? string.Format("{0:0.00}", power) + "" : string.Format("{0:0.00}", power)
                                : "----" : "----";
            loopInfox.Add(new LoopInfoLeft(loopInfox.Count ,loopId, name, vx, ax, powerx,
                                        used ? isShieldLoop == 0 ? string.Format("{0:0.00}", rate * 100) + " %" : "----" : "----", used ? isShieldLoop == 0 ? pws : "----" : "----",
                                        switchIsClose ? "吸合" : "断开", isHistory ? "" : attachInfo,attachInfoName, color, attachcolor, referenceAx, upperx, lowerx, ratiox, onlineRate, switchIsClose ? "#CD0000" : "#000000", isShieldLoop));

        }


        private void AddSwitchInInfo(ref ObservableCollection<LoopInfoLeft > loopInfox,  int loopId,  string loopName,string  switchShowInfo, string color)
        {
            loopInfox.Add(new LoopInfoLeft(loopInfox .Count , loopId, loopName, "", "", "", "", "", switchShowInfo, "","", color, color,  "", "", "", "","","",0));
        }




 
    }


   
}
