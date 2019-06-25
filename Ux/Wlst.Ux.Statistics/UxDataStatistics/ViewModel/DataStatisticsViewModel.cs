using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Telerik.Windows.Controls.ChartView;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.Statistics.UxDataStatistics.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.client;
using OptionXmlSvr = Wlst.Cr.CoreOne.Services.OptionXmlSvr;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;

namespace Wlst.Ux.Statistics.UxDataStatistics.ViewModel
{
    [Export(typeof (IIUxDataStatisticsModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class DataStatisticsViewModel : EventHandlerHelperExtendNotifyProperyChanged, IITab,
                                                   IIUxDataStatisticsModule
    {

        private ChartPalette _palette;
        private List<ChartPalette> _palettes;

        public DataStatisticsViewModel()
        {
            InitEvent();
            InitAction();
            DtEndTime = DateTime.Now;
            DtStartTime = DateTime.Now.AddDays(-1);
            IsAllRtus = false;
            IsSystemLightRate = false;
            IsNotShowOffline = false;
            IsShowRtuName = false;
            this.InitializePalettePresets();

        }

        private void InitializePalettePresets()
        {
            List<ChartPalette> palettes = new List<ChartPalette>();
            palettes.Add(ChartPalettes.Arctic);
            palettes.Add(ChartPalettes.Autumn);
            palettes.Add(ChartPalettes.Cold);
            palettes.Add(ChartPalettes.Flower);
            palettes.Add(ChartPalettes.Forest);
            palettes.Add(ChartPalettes.Grayscale);

            palettes.Add(ChartPalettes.Ground);
            palettes.Add(ChartPalettes.Lilac);
            palettes.Add(ChartPalettes.Natural);
            palettes.Add(ChartPalettes.Office2013);
            palettes.Add(ChartPalettes.Pastel);
            palettes.Add(ChartPalettes.Rainbow);
            palettes.Add(ChartPalettes.Spring);
            palettes.Add(ChartPalettes.Summer);
            palettes.Add(ChartPalettes.Warm);
            palettes.Add(ChartPalettes.Windows8);
            palettes.Add(ChartPalettes.VisualStudio2013);

            this.Palettes = palettes;
            this.Palette = ChartPalettes.Windows8;
        }

        public ChartPalette Palette
        {
            get { return this._palette; }
            set
            {
                if (this._palette != value)
                {
                    this._palette = value;
                    RaisePropertyChanged(() => Palette);
                }
            }
        }

        public List<ChartPalette> Palettes
        {
            get { return this._palettes; }
            set
            {
                if (this._palettes != value)
                {
                    this._palettes = value;
                    RaisePropertyChanged(() => Palettes);
                }
            }
        }

        #region NavOnLoad

        private bool _isViewShow = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            _isViewShow = true;
            //this.LineData = this.CreateLineData();
            //初始化是读取上一次点击设备的CurrentSelectRtuId，若程序刚开，CurrentSelectRtuId为0，则提示   lvf 2018年5月10日14:06:14

            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId == 0)
            {
                RtuName = "请在右侧设备树中选择设备";
                return;
            }

            //RtuId = Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId;
        }



        private object _lineData;

        public object LineData
        {
            get { return this._lineData; }
            set
            {
                if (this._lineData != value)
                {
                    this._lineData = value;
                    RaisePropertyChanged(() => LineData);
                }
            }
        }

        public void OnUserHideOrClosing()
        {
            _isViewShow = false;
            LightRateStatisticsData = new ObservableCollection<LightRateStatisticsViewModel>();
        }

        //

        #endregion

        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "数据统计"; }
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
    }

    /// <summary>
    /// Attri define
    /// </summary>
    public partial class DataStatisticsViewModel
    {

        #region DtEndTime

        private DateTime _dtEndTime;

        /// <summary>
        /// 故障查询结束时间
        /// </summary>
        public DateTime DtEndTime
        {
            get { return _dtEndTime; }
            set
            {
                if (_dtEndTime != value)
                {
                    _dtEndTime = value;

                    RaisePropertyChanged(() => DtEndTime);
                }
            }
        }

        #endregion

        #region DtStartTime

        private DateTime _dtStartTime;

        /// <summary>
        /// 故障查询起始时间
        /// </summary>
        public DateTime DtStartTime
        {
            get { return _dtStartTime; }
            set
            {
                if (_dtStartTime != value)
                {
                    _dtStartTime = value;
                    RaisePropertyChanged(() => DtStartTime);
                }
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
                if (value == _remind) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }

        #endregion

        #region LdRemind

        private string _ldremind;

        public string LdRemind
        {
            get { return _ldremind; }
            set
            {
                if (value == _ldremind) return;
                _ldremind = value;
                RaisePropertyChanged(() => LdRemind);
            }
        }

        #endregion

        #region RtuName

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    RaisePropertyChanged(() => RtuName);
                }
            }
        }

        #endregion

        #region RtuId

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private int _rtuid;

        public int RtuId
        {
            get { return _rtuid; }
            set
            {

                if (value != _rtuid)
                {
                    _rtuid = value;
                    PhyId = value;
                    RaisePropertyChanged(() => RtuId);
                    RtuName = "请选择终端";
                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (_rtuid))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [_rtuid];
                    RtuName = tml.RtuName;

                    if (tml.RtuFid == 0)
                        PhyId = tml.RtuPhyId;
                    else PhyId = value;
                }
            }
        }

        #endregion

        #region IsAllRtus

        private bool _isAllRtus;

        /// <summary>
        /// 是否勾选全部终端
        /// </summary>
        public bool IsAllRtus
        {
            get { return _isAllRtus; }
            set
            {
                if (_isAllRtus != value)
                {
                    _isAllRtus = value;
                    this.RaisePropertyChanged(() => this.IsAllRtus);
                    if (IsAllRtus == true) IsSystemLightRate = false;
                    if (IsSystemLightRate == false && IsAllRtus == false)
                    {
                        IsShowRtuName = false;
                    }
                    else
                    {
                        IsShowRtuName = true;
                    }
                }
            }
        }

        #endregion

        #region IsSystemLightRate

        private bool _isSystemLightRate;

        /// <summary>
        /// 是否勾选全部终端
        /// </summary>
        public bool IsSystemLightRate
        {
            get { return _isSystemLightRate; }
            set
            {
                if (_isSystemLightRate != value)
                {
                    _isSystemLightRate = value;
                    this.RaisePropertyChanged(() => this.IsSystemLightRate);
                    if (IsSystemLightRate == true) IsAllRtus = false;
                    if (IsSystemLightRate == false && IsAllRtus == false)
                    {
                        IsShowRtuName = false;
                    }
                    else
                    {
                        IsShowRtuName = true;
                    }
                }
            }
        }

        #endregion



        #region IsShowRtuName

        private bool _isShowRtuName;

        /// <summary>
        /// 是否显示终端名称
        /// </summary>
        public bool IsShowRtuName
        {
            get { return _isShowRtuName; }
            set
            {
                if (_isShowRtuName != value)
                {
                    _isShowRtuName = value;
                    this.RaisePropertyChanged(() => this.IsShowRtuName);
                }
            }
        }

        #endregion


        #region IsNotShowOffline

        private bool _isNotShowOffline;

        /// <summary>
        /// 是否勾选不显示离线数据
        /// </summary>
        public bool IsNotShowOffline
        {
            get { return _isNotShowOffline; }
            set
            {
                if (_isNotShowOffline != value)
                {
                    _isNotShowOffline = value;
                    this.RaisePropertyChanged(() => this.IsNotShowOffline);
                }
            }
        }

        #endregion


        #region LightRateStatisticsData

        private ObservableCollection<LightRateStatisticsViewModel> _lightRateStatisticsData;

        /// <summary>
        /// 亮灯率数据
        /// </summary>
        public ObservableCollection<LightRateStatisticsViewModel> LightRateStatisticsData
        {
            get
            {
                if (_lightRateStatisticsData == null)
                    _lightRateStatisticsData = new ObservableCollection<LightRateStatisticsViewModel>();
                return _lightRateStatisticsData;
            }
            set
            {
                if (_lightRateStatisticsData == value) return;
                _lightRateStatisticsData = value;
                this.RaisePropertyChanged(() => this.LightRateStatisticsData);
            }
        }

        #endregion

        #region LdData

        private ObservableCollection<ObservableCollection<LdDataViewModel>> _ldData;

        /// <summary>
        /// 漏电数据
        /// </summary>
        public ObservableCollection<ObservableCollection<LdDataViewModel>> LdData
        {
            get
            {
                if (_ldData == null)
                    _ldData = new ObservableCollection<ObservableCollection<LdDataViewModel>>();
                return _ldData;
            }
            set
            {
                if (_ldData == value) return;
                _ldData = value;
                this.RaisePropertyChanged(() => this.LdData);
            }
        }

        #endregion
    }


    /// <summary>
    /// command
    /// </summary>
    public partial class DataStatisticsViewModel
    {


        #region CmdQuery

        private DateTime _dtQuery;

        public ICommand CmdQuery
        {
            get { return new RelayCommand(Ex, CanEx, true); }
        }


        private void Ex()
        {
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮

            _dtQuery = DateTime.Now;

            if (!GetCheckedInformation()) return;
            LightRateStatisticsData.Clear();
            var rtulst = new List<int>();
            rtulst.Add(0);
            //如果勾选全部设备，则传0，其余为设备逻辑地址
            //if(!IsAllRtus)
            //{
            //    rtulst .Add(RtuId);
            //}else
            //{
            //    rtulst.Add(0);
            //}
            if (IsAllRtus)
            {
                rtulst = new List<int>();
            }
            else if (!IsSystemLightRate)
            {
                if (RtuId == 0)
                {
                    UMessageBox.Show("提醒", "请选择设备", UMessageBoxButton.Ok);
                    return;
                }
                rtulst = new List<int>();
                rtulst.Add(RtuId);
            }
            ReqeustLightRateData(rtulst, DtStartTime, DtEndTime);
            Remind = "查询命令已发送...请等待数据反馈！";
        }

        /// <summary>
        /// 请求亮灯率数据
        /// </summary>
        /// <param name="rtuId">设备地址</param>
        /// <param name="dtStartTime">起始时间</param>
        /// <param name="dtEndtime">结束时间</param>
        public static void ReqeustLightRateData(List<int> rtuId, DateTime dtStartTime, DateTime dtEndtime)
        {
            //if (rtuId.Count == 0) return; 
            var dts = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var dte = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);
            var info = Wlst.Sr.ProtocolPhone.LightRate.wst_lightrate_query_light_rate;
            info.WstQueryLightRate.Op = 1;
            info.WstQueryLightRate.RtuId = rtuId;
            info.WstQueryLightRate.EndDate = dte.Ticks;
            info.WstQueryLightRate.StartDate = dts.Ticks;

            SndOrderServer.OrderSnd(info, 10, 6);
        }


        private bool CanEx()
        {

            if (DtStartTime > DtEndTime) return false;



            return DateTime.Now.Ticks - _dtQuery.Ticks > 3000000;
        }

        /// <summary>
        /// 是否可以点击查询按钮
        /// </summary>
        /// <returns></returns>
        private bool GetCheckedInformation()
        {
            if (DtStartTime.AddDays(63) < DtEndTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在62天以内", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            return true;
        }

        #endregion

        #region CmdQueryLd

        private DateTime _dtQueryLd;

        public ICommand CmdQueryLd
        {
            get { return new RelayCommand(ExLd, CanExLd, true); }
        }


        private void ExLd()
        {
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮

            _dtQueryLd = DateTime.Now;

            var rtulst = new List<int>();
            rtulst.Add(0);
            if (!IsSystemLightRate)
            {
                if (RtuId < 1600000 || RtuId > 1699999)
                {
                    UMessageBox.Show("提醒", "请选择漏电设备", UMessageBoxButton.Ok);
                    return;
                }
                rtulst = new List<int>();
                rtulst.Add(RtuId);
            }
            ReqeustLdData(rtulst, DtStartTime, DtEndTime);
            Remind = "查询命令已发送...请等待数据反馈！";
        }

        /// <summary>
        /// 请求漏电数据
        /// </summary>
        /// <param name="rtuId">设备地址</param>
        /// <param name="dtStartTime">起始时间</param>
        /// <param name="dtEndtime">结束时间</param>
        public static void ReqeustLdData(List<int> rtuId, DateTime dtStartTime, DateTime dtEndtime)
        {
            //if (rtuId.Count == 0) return; 
            var dts = new DateTime(dtStartTime.Year, dtStartTime.Month, dtStartTime.Day, 0, 0, 1);
            var dte = new DateTime(dtEndtime.Year, dtEndtime.Month, dtEndtime.Day, 23, 59, 59);
            var info = Sr.ProtocolPhone.LxLeak.wst_leak_datas;
            //.wst_ldu_data;// .wlst_cnt_wj1090_request_data ;//.ServerPart.wlst_Wj1090_clinet_request_Data;
            info.WstLeakDatas.RtuId = rtuId[0];
            info.WstLeakDatas.DtStartTime = dts.Ticks;
            info.WstLeakDatas.DtEndTime = dte.Ticks;

            SndOrderServer.OrderSnd(info, 10, 6);


        }


        private bool CanExLd()
        {

            if (DtStartTime > DtEndTime) return false;



            return DateTime.Now.Ticks - _dtQueryLd.Ticks > 3000000;
        }



        #endregion

        // 导出excel

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
                lsttitle.Add("终端名称");
                lsttitle.Add("采集时间");
                lsttitle.Add("终端状态");
                lsttitle.Add("亮灯率");
                lsttitle.Add("备注");

                var lstobj = new List<List<object>>();

                foreach (var g in LightRateStatisticsData)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.PhyId);
                    tmp.Add(g.RtuName);
                    tmp.Add(g.Date);
                    tmp.Add(g.StrIsOnline);
                    tmp.Add(g.StrBrightRate);
                    tmp.Add(g.Remark);
                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            if (LightRateStatisticsData.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion

        //wyg  打印预览

        #region CmdPrintPriview

        private ICommand _cmdPrintPriview;

        public ICommand CmdPrintPriview
        {
            get
            {
                if (_cmdPrintPriview == null)
                    _cmdPrintPriview = new RelayCommand(ExCmdPrintPriview, CanExPrintPriview, false);
                return _cmdPrintPriview;
            }
        }

        private void ExCmdPrintPriview()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("序号");
                tabletitle.Add("终端地址");
                tabletitle.Add("终端名称");
                tabletitle.Add("采集时间");
                tabletitle.Add("终端状态");
                tabletitle.Add("亮灯率");
                tabletitle.Add("备注");
                var table = new List<List<string>>();
                DateTime createtime;
                DateTime removetime;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                foreach (var g in LightRateStatisticsData)
                {
                    var tem = new List<string>();
                    tem.Add(g.Index.ToString());
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.Date);
                    tem.Add(g.StrIsOnline);
                    tem.Add(g.StrBrightRate);
                    tem.Add(g.Remark);

                    table.Add(tem);
                }
                print.Prints.PrintPriview(tabletitle, table, false, "亮灯率统计表",
                                          Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrintPriview()
        {
            if (LightRateStatisticsData.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }


        #endregion

        //打印

        #region CmdPrint

        private ICommand _cmdPrint;

        public ICommand CmdPrint
        {
            get
            {
                if (_cmdPrint == null)
                    _cmdPrint = new RelayCommand(ExCmdPrint, CanExPrint, false);
                return _cmdPrint;
            }
        }

        private void ExCmdPrint()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("序号");
                tabletitle.Add("终端地址");
                tabletitle.Add("终端名称");
                tabletitle.Add("采集时间");
                tabletitle.Add("终端状态");
                tabletitle.Add("亮灯率");
                tabletitle.Add("备注");
                var table = new List<List<string>>();
                DateTime createtime;
                DateTime removetime;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                foreach (var g in LightRateStatisticsData)
                {
                    //createtime = Convert.ToDateTime(g.DtCreateTime, dtFormat);
                    var tem = new List<string>();
                    tem.Add(g.Index.ToString());
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.Date);
                    tem.Add(g.StrIsOnline);
                    tem.Add(g.StrBrightRate);
                    tem.Add(g.Remark);

                    table.Add(tem);
                }
                print.Prints.Print(tabletitle, table, false, "亮灯率统计表",
                                   Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrint()
        {
            if (LightRateStatisticsData.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }

        #endregion
    }



    /// <summary>
    /// Action
    /// </summary>
    public partial class DataStatisticsViewModel
    {

        private void InitEvent()
        {
            EventPublish.AddEventTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

        }

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {


            try
            {
                if (args.EventType == PublishEventType.Core)
                {


                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        if (_isViewShow == false) return;
                        //if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return;
                        //if (!_isViewShow) return;
                        if (args.EventAttachInfo == "RequestDataWhenErrorHappenEqu") return;
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        //if (id < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart) return;
                        if (RtuId == id) return;
                        RtuId = id;


                        //if (!IsOldFaultQuery)
                        //{
                        //    Ex();
                        //}else
                        //{
                        //     if (IsSingleEquipmentQuery) Ex();
                        //}
                    }

                    //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentMulSelected)
                    //{
                    //    if (!_isViewShow) return;
                    //    if (OptionXmlSvr.GetOptionInt(4001, 2) != 1) return;
                    //    // if (args.EventAttachInfo == "RequestDataWhenErrorHappenEqu") return;
                    //    var ids = args.GetParams()[0] as List<int>;

                    //    var rtus = (from t in ids
                    //                where
                    //                    t < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuEnd &&
                    //                    t > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart
                    //                select t).ToList();
                    //    SelectedRtus.Clear();
                    //    SelectedRtus = rtus;
                    //    if (rtus.Count == 0)
                    //    {
                    //        this.SelectedRtus.Clear();
                    //        RtuId = 0;
                    //        RtuName = "通过终端树勾选终端进行故障查询.";
                    //    }
                    //    else
                    //    {
                    //        RtuId = SelectedRtus[0];
                    //        if (
                    //    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                    //         InfoItems.ContainsKey
                    //         (RtuId))
                    //            return;
                    //        var tml =
                    //            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                    //                [RtuId];
                    //        RtuName = tml.RtuName;
                    //        if (SelectedRtus.Count > 1)
                    //            RtuName += " [等" + SelectedRtus.Count + "个终端]";

                    //    }


                    //}

                }
                //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                //{
                //    var infos = args.GetParams()[1] as EquipmentPreFaultExChange;
                //    if (infos == null) return;
                //    OnPreDataBack(infos);
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "EquipmentDataQuery.EquipmentFaultRecordQueryViewModel FundEventHandler occer an error:" +
                    ex);
            }

        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            if (!_isViewShow) return false;
            // if (!IsSingleEquipmentQuery) return false;
            try
            {
                if (args.EventType == PublishEventType.Core)
                {
                    //if (!IsSingleEquipmentQuery) return false;  lvf
                    //if (args.EventId == Sr.EquipmentGroupInfoHolding.Services.EventIdAssign.MainSingleTreeNodeActive)
                    //{
                    //    if (Convert.ToInt32(args.GetParams()[1]) == 2)
                    //    {
                    //        return true;
                    //    }
                    //}
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        //无视多选框
                        //if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return false;

                        return true;
                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentMulSelected)
                    {
                        //if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return true;
                        return false;

                    }


                }

            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LightRate.wst_lightrate_query_light_rate,
                // .wlst_cnt_wj3090_order_snd_paras ,//.ClientPart.wlst_rtuargsupdate_server_ans_clinet_order_paras4000,
                OnRequestLightRateData,
                typeof (DataStatisticsViewModel), this, true);

            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxLeak.wst_leak_datas,
                // .wlst_svr_ans_cnt_wj1090_request_data ,//.ClientPart.wlst_Wj1090_server_ans_clinet_request_Data,
                OnRequestLdData,
                typeof (DataStatisticsViewModel), this, true);


            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolPhone.LxRtu.wst_zc_rtu_info,// .wlst_cnt_wj3090_order_snd_paras ,//.ClientPart.wlst_rtuargsupdate_server_ans_clinet_order_paras4000,
            //    RtuParaUpdate40001,
            //    typeof(DataStatisticsViewModel), this,true);
        }


        /// <summary>
        /// 亮灯率数据返回处理
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        public void OnRequestLightRateData(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstQueryLightRate;
            if (info == null) return;
            if (_isViewShow == false) return;

            var obs = new ObservableCollection<LightRateStatisticsViewModel>();
            var list = info.Items;
            if (list.Count == 0) return;

            var lightRateItems = (from t in list orderby t.Date ascending select t).ToList();
            int indexx = 0;
            foreach (var item in lightRateItems)
            {
                //不显示离线
                if (IsNotShowOffline && item.IsOnline == false) continue;
                indexx++;
                var tmps = new LightRateStatisticsViewModel
                               {
                                   Date = new DateTime(item.Date).ToString("yyyy-MM-dd HH:mm:ss"),
                                   RtuId = item.RtuOrSluId,
                                   IsOnline = item.IsOnline,
                                   BrightRate = item.BrightRate,
                                   Remark = item.Remark,
                                   Index = indexx,
                               };

                if (tmps.RtuId == 0)
                {
                    tmps.RtuName = "全部终端（整个系统）";
                    tmps.StrIsOnline = " -- ";
                }
                obs.Add(tmps);

            }

            var tmp = (from t in obs orderby t.Date ascending select t).ToList();
            this.LightRateStatisticsData.Clear();
            foreach (var f in tmp)
            {
                LightRateStatisticsData.Add(f);
                if (indexx%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            }

            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + LightRateStatisticsData.Count + " 条数据.";
        }



        private void OnRequestLdData(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstLeakDatas; //WstLduHisData  ;
            if (info == null) return;
            //LdData.Clear();
            this.LineData = CreateLineData(info.Items);


        }

        private IEnumerable<IEnumerable<LdDataViewModel>> CreateLineData(List<LeakNewData> items)
        {


            DateTime today = DateTime.Today;
            List<IEnumerable<LdDataViewModel>> LineData = new List<IEnumerable<LdDataViewModel>>();


            if (items.Count == 0) return LineData;

            var ld1 = new ObservableCollection<LdDataViewModel>();
            var ld2 = new ObservableCollection<LdDataViewModel>();
            var ld3 = new ObservableCollection<LdDataViewModel>();
            var ld4 = new ObservableCollection<LdDataViewModel>();
            var sss = (from t in items[0].Items orderby t.DateCreate select t).ToList();
            foreach (var g in sss)//items[0].Items
            {

                if (g.LeakLineId == 1)
                {
                    ld1.Add(new LdDataViewModel(new DateTime(g.DateCreate), g.CurrentLeakOrTemperature));
                }
                if (g.LeakLineId == 2)
                {
                    ld2.Add(new LdDataViewModel(new DateTime(g.DateCreate), g.CurrentLeakOrTemperature));
                }
                if (g.LeakLineId == 3)
                {
                    ld3.Add(new LdDataViewModel(new DateTime(g.DateCreate), g.CurrentLeakOrTemperature));
                }
                if (g.LeakLineId == 4)
                {
                    ld4.Add(new LdDataViewModel(new DateTime(g.DateCreate), g.CurrentLeakOrTemperature));
                }

            }
            LineData.Add(ld1);
            LineData.Add(ld2);
            LineData.Add(ld3);
            LineData.Add(ld4);


            return LineData;

            //List<LdDataViewModel> WinXPData = new List<LdDataViewModel>();
            //WinXPData.Add(new LdDataViewModel("Q2,2010", 0.5809));
            //WinXPData.Add(new LdDataViewModel("Q3,2010", 0.5179));
            //WinXPData.Add(new LdDataViewModel("Q4,2010", 0.5532));
            //WinXPData.Add(new LdDataViewModel("Q1,2011", 0.4803));

            //List<LdDataViewModel> Win7LdDataViewModel = new List<LdDataViewModel>();
            //Win7LdDataViewModel.Add(new LdDataViewModel("Q2,2010", 0.1481));
            //Win7LdDataViewModel.Add(new LdDataViewModel("Q3,2010", 0.1947));
            //Win7LdDataViewModel.Add(new LdDataViewModel("Q4,2010", 0.2423));
            //Win7LdDataViewModel.Add(new LdDataViewModel("Q1,2011", 0.2913));

            //List<LdDataViewModel> VistaLdDataViewModel = new List<LdDataViewModel>();
            //VistaLdDataViewModel.Add(new LdDataViewModel("Q2,2010", 0.1946));
            //VistaLdDataViewModel.Add(new LdDataViewModel("Q3,2010", 0.1767));
            //VistaLdDataViewModel.Add(new LdDataViewModel("Q4,2010", 0.1602));
            //VistaLdDataViewModel.Add(new LdDataViewModel("Q1,2011", 0.1439));

            //List<LdDataViewModel> MacOSXLdDataViewModel = new List<LdDataViewModel>();
            //MacOSXLdDataViewModel.Add(new LdDataViewModel("Q2,2010", 0.575));
            //MacOSXLdDataViewModel.Add(new LdDataViewModel("Q3,2010", 0.57));
            //MacOSXLdDataViewModel.Add(new LdDataViewModel("Q4,2010", 0.617));
            //MacOSXLdDataViewModel.Add(new LdDataViewModel("Q1,2011", 0.656));

            //List<LdDataViewModel> LinuxLdDataViewModel = new List<LdDataViewModel>();
            //LinuxLdDataViewModel.Add(new LdDataViewModel("Q2,2010", 0.08));
            //LinuxLdDataViewModel.Add(new LdDataViewModel("Q3,2010", 0.078));
            //LinuxLdDataViewModel.Add(new LdDataViewModel("Q4,2010", 0.077));
            //LinuxLdDataViewModel.Add(new LdDataViewModel("Q1,2011", 0.075));

            //LineData.Add(WinXPData);
            //LineData.Add(Win7LdDataViewModel);
            //LineData.Add(VistaLdDataViewModel);
            //LineData.Add(MacOSXLdDataViewModel);
            //LineData.Add(LinuxLdDataViewModel);

            //return LineData;



        }

    }
}