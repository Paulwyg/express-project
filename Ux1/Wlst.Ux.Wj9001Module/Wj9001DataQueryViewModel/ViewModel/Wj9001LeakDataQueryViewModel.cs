using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Ux.Wj9001Module.Wj9001DataQueryViewModel.Services;
using Wlst.client;
//.Wj1090LduDataQueryViewModel.Services;

namespace Wlst.Ux.Wj9001Module.Wj9001DataQueryViewModel.ViewModel
{
    [Export(typeof(IIWj9001LeakDataQueryView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj9001LeakDataQueryViewModel : EventHandlerHelperExtendNotifyProperyChanged,
                                       Cr.Core.CoreInterface.IINavOnLoad, Cr.Core.CoreInterface.IITab, IIWj9001LeakDataQueryView
    {

        private bool _isThisViewActive = false;
        public void NavOnLoad(params object[] parsObjects)
        {

            _isThisViewActive = true;
            DtStart = DateTime.Now.AddDays(-1);
            DtEnd = DateTime.Now;
            IsOldFaultQuery = false;
            IsAUpper = false;
            IsLdValue = false;
            AUpperValue = 100;
            LdValue = 0;
            try
            {
                int rtuid = Convert.ToInt32(parsObjects[0]);
                if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLeak(rtuid)) return;

                var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);//.GetEquipmentInfo(rtuid);
                if (info == null) return;
                AttachId = info.RtuFid == 0 ? rtuid : info.RtuFid;
                RtuId = rtuid;

            }
            catch (Exception ec)
            {

            }
            Remind = "请选择设备...";
        }

        public void OnUserHideOrClosing()
        {
            _isThisViewActive = false;
            Items = new ObservableCollection<Wj9001DataQueryoneItemViewModel>();//<LduDataQueryoneItemViewModel>();

            //throw new NotImplementedException();
            ItemCount = 0;
            PageTotal = "";
        }

        #region Tab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "漏电数据查询"; }
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

        public Wj9001LeakDataQueryViewModel()
        {
            InitAction();
            InitEvent();

        }
    }

    /// <summary>
    /// 属性，Field
    /// </summary>
    public partial class Wj9001LeakDataQueryViewModel
    {
        #region Remind

        private string _remind;
        public string Remind
        {
            get { return _remind; }
            set
            {
                if(_remind==value) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }
        #endregion

        #region 起始时间

        private DateTime _dtStart;
        public DateTime DtStart
        {
            get { return _dtStart; }
            set
            {
                if (_dtStart == value) return;
                _dtStart = value;
                RaisePropertyChanged(()=>DtStart);
            }
        }

        #endregion

        #region 结束时间
        private DateTime _dtEnd;
        public DateTime DtEnd
        {
            get { return _dtEnd; }
            set
            {
                if (_dtEnd == value) return;
                _dtEnd = value;
                RaisePropertyChanged(() => DtEnd);
            }
        }
        #endregion

        #region 主设备名称

        private string _attachName;
        public string AttachName
        {
            get { return _attachName; }
            set
            {
                if(_attachName !=value)
                {
                    _attachName = value;
                    RaisePropertyChanged(()=>AttachName);
                }
            }
        }

        #endregion

        #region 主设备ID
        private int _attachId;
        public int AttachId
        {
            get { return _attachId; }
            set
            {
                if (_attachId == value) return;
                _attachId = value;
                RaisePropertyChanged(() => AttachId);

                AttachName = "Reserve";
                if (
                    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey
                         (_attachId))
                    return;
                var tml =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                        [_attachId];
                AttachName = tml.RtuName;
                AttachPhyId = tml.RtuPhyId;
            }
        }

        private int _attachPhyId;

        public int AttachPhyId
        {
            get { return _attachPhyId; }
            set
            {
                if (_attachPhyId == value) return;
                _attachPhyId = value;
                RaisePropertyChanged(() => AttachPhyId);
            }
        }

        #endregion

        #region 漏电名称
        private string _rtuName;
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (_rtuName == value) return;
                _rtuName = value;
                RaisePropertyChanged(() => RtuName);
            }
        }
        #endregion

        #region 集中器ID
        private int _rtuId;
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId != value)
                {
                    _rtuId = value;
                    RaisePropertyChanged(() => RtuId);

                    RtuName = "Reserve";
                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                             .ContainsKey
                             (_rtuId))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [_rtuId];
                    RtuName = tml.RtuName;
                }
            }
        }
        #endregion



        #region IsOldFaultQuery

        private bool _isOldFaultQuery;
        public bool IsOldFaultQuery
        {
            get { return _isOldFaultQuery; }
            set
            {
                if (_isOldFaultQuery == value) return;
                _isOldFaultQuery = value;
                RaisePropertyChanged(() => IsOldFaultQuery);
            }
        }
        #endregion

        #region IsAUpper

        private bool _isAUpper;
        public bool IsAUpper
        {
            get { return _isAUpper; }
            set
            {
                if (_isAUpper == value) return;
                _isAUpper = value;
                RaisePropertyChanged(() => IsAUpper);
            }
        }
        #endregion

        #region IsLdValue

        private bool _isLdValue;
        public bool IsLdValue
        {
            get { return _isLdValue; }
            set
            {
                if (_isLdValue == value) return;
                _isLdValue = value;
                RaisePropertyChanged(() => IsLdValue);
            }
        }
        #endregion

        #region IsShowErr

        private bool _isShowErr;
        public bool IsShowErr
        {
            get { return _isShowErr; }
            set
            {
                if (_isShowErr == value) return;
                _isShowErr = value;
                RaisePropertyChanged(() => IsShowErr);
            }
        }
        #endregion

        #region AUpperValue

        private int _aUpperValue;

        public int AUpperValue
        {
            get { return _aUpperValue; }
            set
            {
                if (value == _aUpperValue) return;
                _aUpperValue = value;
                this.RaisePropertyChanged(() => this.AUpperValue);
            }
        }

        #endregion

        #region LdValue

        private int _ldValue;

        public int LdValue
        {
            get { return _ldValue; }
            set
            {
                if (value == _ldValue) return;
                _ldValue = value;
                this.RaisePropertyChanged(() => this.LdValue);
            }
        }

        #endregion

        #region 分页

        #region PageTotal

        private string _pageTotal;
        public string PageTotal
        {
            get { return _pageTotal; }
            set
            {
                if (value == _pageTotal) return;
                _pageTotal = value;
                RaisePropertyChanged(() => PageTotal);
            }
        }
        #endregion

        #region   PageIndex

        private int _pageIndex;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (value != _pageIndex)
                {
                    _pageIndex = value;
                    this.RaisePropertyChanged(() => this.PageIndex);
                    RequestHttpData(value, 1);

                }
            }
        }
        #endregion

        #region   ItemCount
        private int _itemCount;

        /// <summary>
        /// 数据总数
        /// </summary>
        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                if (value != _itemCount)
                {
                    _itemCount = value;
                    this.RaisePropertyChanged(() => this.ItemCount);
                }
            }
        }
        #endregion

        #region   PageSize
        private int _pageSize;

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value != _pageSize)
                {
                    _pageSize = value;
                    this.RaisePropertyChanged(() => this.PageSize);
                }
            }
        }
        #endregion

        #region PagerVisi

        private Visibility _pagerVisi = Visibility.Visible;
        public Visibility PagerVisi
        {
            get { return _pagerVisi; }
            set
            {
                if (value == _pagerVisi) return;
                _pagerVisi = value;
                RaisePropertyChanged(() => PagerVisi);
            }
        }
        #endregion

        #endregion
        //#region 线路名称
        //private string _lduLineName;
        //public string LduLineName
        //{
        //    get { return _lduLineName; }
        //    set
        //    {
        //        if (_lduLineName != value)
        //        {
        //            _lduLineName = value;
        //            RaisePropertyChanged(() => LduLineName);
        //        }
        //    }
        //}
        //#endregion

        //#region 线路ID
        //private int _lduLineId;
        //public int LduLineId
        //{
        //    get { return _lduLineId; }
        //    set
        //    {
        //        if (_lduLineId == value) return;
        //        _lduLineId = value;
        //        RaisePropertyChanged(() => LduLineId);


        //        if(_lduLineId>0) 
        //        {
        //            LduLineName = "Reserve";
        //            if (
        //                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
        //                     InfoItems.ContainsKey
        //                     (_rtuId))
        //                return;
        //            var tml =
        //                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
        //                    [_rtuId] as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;// Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;
        //            if (tml == null) return;
        //            LduLineName = tml.WjLduLines[_lduLineId - 1].LduLineName;
        //        }
        //    }
        //}
        //#endregion

        #region IsLockTml

        private bool _isLockTml;
        public bool IsLockTml
        {
            get { return _isLockTml; }
            set
            {
                if (_isLockTml == value) return;
                _isLockTml = value;
                RaisePropertyChanged(() => IsLockTml);
            }
        }
        #endregion

        #region ICommand

        private DateTime _dtBtnQuery;
        private ICommand _cmdBtnQuery;
        public ICommand CmdBtnQuery
        {
            get { return _cmdBtnQuery ?? (_cmdBtnQuery = new RelayCommand(ExBtnQuery, CanBtnQuery, true)); }
        }
        private bool CanBtnQuery()
        {
            return DateTime.Now.Ticks - _dtBtnQuery.Ticks > 30000000;
        }
        private void ExBtnQuery()
        {
            _dtBtnQuery = DateTime.Now;
            //RequestData();
            PageIndex = 0;
            RequestHttpData(PageIndex, 0);
        }
        #endregion

        #region Items

        private ObservableCollection<Wj9001DataQueryoneItemViewModel> _items;

        public ObservableCollection<Wj9001DataQueryoneItemViewModel> Items
        {
            get { return _items ?? (_items = new ObservableCollection<Wj9001DataQueryoneItemViewModel>()); }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => Items);
            }
        }

        #endregion

        //#region 主设备显示

        //private Visibility _attachVisi;
        //public Visibility AttachVisi
        //{
        //    get { return _attachVisi; }
        //    set
        //    {
        //        if (_attachVisi == value) return;
        //        _attachVisi = value;
        //        RaisePropertyChanged(()=>AttachVisi);
        //    }
        //}

        //#endregion

        //#region 集中器设备显示
        //private Visibility _rtuVisi;
        //public Visibility RtuVisi
        //{
        //    get { return _rtuVisi; }
        //    set
        //    {
        //        if (_rtuVisi != value)
        //        {
        //            _rtuVisi = value;
        //            RaisePropertyChanged(() => RtuVisi);
        //        }
        //    }
        //}
        //#endregion

        //#region 线路名称显示
        //private Visibility _lduLineVisi;
        //public Visibility LduLineVisi
        //{
        //    get { return _lduLineVisi; }
        //    set
        //    {
        //        if (_lduLineVisi == value) return;
        //        _lduLineVisi = value;
        //        RaisePropertyChanged(() => LduLineVisi);
        //    }
        //}
        //#endregion

        //#region IsAdvacnceChecked

        //private bool _isAdvacnceChecked;
        //public bool IsAdvacnceChecked
        //{
        //    get { return _isAdvacnceChecked; }
        //    set
        //    {
        //        if(_isAdvacnceChecked==value) return;
        //        _isAdvacnceChecked = value;
        //        AdvacnceVisi = _isAdvacnceChecked ? Visibility.Visible : Visibility.Collapsed;
        //        RaisePropertyChanged(()=>IsAdvacnceChecked);
        //    }
        //}
        //#endregion

        //#region AdvacnceVisi

        //private Visibility _advacnceVisi;
        //public Visibility AdvacnceVisi
        //{
        //    get { return _advacnceVisi; }
        //    set
        //    {
        //        if (_advacnceVisi == value) return;
        //        _advacnceVisi = value;
        //        RaisePropertyChanged(()=>AdvacnceVisi);
        //    }
        //}
        //#endregion

        //#region gridview Columns visible

        //#region BrightRateVisi

        //private bool _brightRateVisi;
        //public bool BrightRateVisi
        //{
        //    get { return _brightRateVisi; }
        //    set
        //    {
        //        if(_brightRateVisi==value) return;
        //        _brightRateVisi = value;
        //        RaisePropertyChanged(()=>BrightRateVisi);
        //    }
        //}

        //#endregion

        //#region SingleVisi
        //private bool _singleVisi;
        //public bool SingleVisi
        //{
        //    get { return _singleVisi; }
        //    set
        //    {
        //        if (_singleVisi == value) return;
        //        _singleVisi = value;
        //        RaisePropertyChanged(() => SingleVisi);
        //    }
        //}
        //#endregion

        //#region ImpedanceVisi
        //private bool _impedanceVisi;
        //public bool ImpedanceVisi
        //{
        //    get { return _impedanceVisi; }
        //    set
        //    {
        //        if (_impedanceVisi == value) return;
        //        _impedanceVisi = value;
        //        RaisePropertyChanged(() => ImpedanceVisi);
        //    }
        //}
        //#endregion

        //#region NumOfUseFullSingleIn12SecVisi
        //private bool _numOfUseFullSingleIn12SecVisi;
        //public bool NumOfUseFullSingleIn12SecVisi
        //{
        //    get { return _numOfUseFullSingleIn12SecVisi; }
        //    set
        //    {
        //        if (_numOfUseFullSingleIn12SecVisi == value) return;
        //        _numOfUseFullSingleIn12SecVisi = value;
        //        RaisePropertyChanged(() => NumOfUseFullSingleIn12SecVisi);
        //    }
        //}
        //#endregion

        //#region NumOfSingleIn12SecVisi
        //private bool _numOfSingleIn12SecVisi;
        //public bool NumOfSingleIn12SecVisi
        //{
        //    get { return _numOfSingleIn12SecVisi; }
        //    set
        //    {
        //        if (_numOfSingleIn12SecVisi == value) return;
        //        _numOfSingleIn12SecVisi = value;
        //        RaisePropertyChanged(() => NumOfSingleIn12SecVisi);
        //    }
        //}
        //#endregion

        //#region FlagDetectionVisi
        //private bool _flagDetectionVisi;
        //public bool FlagDetectionVisi
        //{
        //    get { return _flagDetectionVisi; }
        //    set
        //    {
        //        if (_flagDetectionVisi == value) return;
        //        _flagDetectionVisi = value;
        //        RaisePropertyChanged(() => FlagDetectionVisi);
        //    }
        //}
        //#endregion

        //#region FlagAlarmVisi
        //private bool _flagAlarmVisi;
        //public bool FlagAlarmVisi
        //{
        //    get { return _flagAlarmVisi; }
        //    set
        //    {
        //        if (_flagAlarmVisi == value) return;
        //        _flagAlarmVisi = value;
        //        RaisePropertyChanged(() => FlagAlarmVisi);
        //    }
        //}
        //#endregion

        //#endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class Wj9001LeakDataQueryViewModel
    {
        public void InitAction()
        {
            //ProtocolServer.RegistProtocol(
            //    Sr.ProtocolPhone .LxLeak.wst_leak_datas,// .wlst_svr_ans_cnt_wj1090_request_data ,//.ClientPart.wlst_Wj1090_server_ans_clinet_request_Data,
            //    ResolveRequestData,
            //    typeof (Wj9001LeakDataQueryViewModel), this);
        }

        private void ResolveRequestData(Wlst .mobile .MsgWithMobile  infos,int pagingFlag)
        {
            var info = infos.WstLeakDatas;//WstLduHisData  ;
            if (info == null) return;
            if (pagingFlag == 0)
            {
                PageSize = infos.Head.PagingNum;
                ItemCount = infos.Head.PagingRecordTotal;
                var count = ItemCount / PageSize + (ItemCount % PageSize > 0 ? 1 : 0);
                PagerVisi = count < 2 ? Visibility.Collapsed : Visibility.Visible;
                PageTotal = "页     " + ItemCount + " 条";
            }
            Items.Clear();
            int i = PageIndex*PageSize;
            var obs = new ObservableCollection<Wj9001DataQueryoneItemViewModel>();
            foreach (LeakNewData item in info.Items)
            {
                if(Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(item.LeakId)==false )
                    continue;
                var leakinfo =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[item.LeakId]
                as Sr.EquipmentInfoHolding.Model.Wj9001Leak;
                if (leakinfo == null) continue;

                foreach (var f in item.Items)
                {
                    
                    //if (leakinfo.WjLeakLines.ContainsKey(f.LeakLineId)==false  || leakinfo.WjLeakLines[f.LeakLineId].IsUsed==false ) continue;
                    obs.Add(new Wj9001DataQueryoneItemViewModel(f, i));
                    //Items.Add(new Wj9001DataQueryoneItemViewModel(f, i));
                    i++;
                }
            }
            FilterErrs(obs);
            Remind = "查询成功.";
            Remind =  "查询成功，共" + ItemCount + " 条数据.";
        }
        private void FilterErrs(ObservableCollection<Wj9001DataQueryoneItemViewModel> records)
        {
            Remind = "查询成功.";
            //var obs = new ObservableCollection<Wj9001DataQueryoneItemViewModel>();
            //foreach (var g in records)
            //{
            //    obs.Add(g);
            //}
          
            if (IsAUpper)
            {
                for (int i = records.Count -1; i >=0; i--)
                {
                    if (records[i].UpperAlarmOrBreakforLeakOrTemperature !=AUpperValue) records.RemoveAt(i);
                }
            }
            if (IsLdValue)
            {
                for (int i = records.Count - 1; i >= 0; i--)
                {
                    if (records[i].CurrentLeakOrTemperature < LdValue) records.RemoveAt(i);
                }
            }
            if (IsShowErr)
            {

                for (int i = records.Count - 1; i >= 0; i--)
                {
                    if (records[i].StateofAlarm != 1) records.RemoveAt(i);
                }
            }
            //var index = 1;
            foreach (var g in records)
            {
                //g.RecordIndex = index++;
                Items.Add(g);
            }


            Remind = " 查询成功，共" + ItemCount + " 条数据.";
        }
        private void InitEvent()
        {

            AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                               PublishEventType.Core);
            //AddEventFilterInfo(Wj1090Module.Services.EventIdAssign.EventTreeNodeConcentratorSelected,
            //                   PublishEventType.Core);
            //AddEventFilterInfo(Wj1090Module.Services.EventIdAssign.EventTreeNodeLineSelected,
            //                   PublishEventType.Core);
            //AddEventFilterInfo(Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
            //                   PublishEventType.Core);
        }

        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            if (!_isThisViewActive) return;

            if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
            {
                int rtuid = Convert.ToInt32(args.GetParams()[0]);
                if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLeak(rtuid)) return;

                var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);//.GetEquipmentInfo(rtuid);
                if (info == null) return;
                AttachId = info.RtuFid == 0 ? rtuid : info.RtuFid;
                RtuId = rtuid;
                //if (args.GetParams().Count > 1)  todo
                //{
                //    try
                //    {
                //        if (args.GetParams().Count > 1)
                //        {
                //            LeakLineId = Convert.ToInt32(args.GetParams()[1]);
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
                //else
                //    LduLineId = 0;
            }
        }
    }

    /// <summary>
    /// socket
    /// </summary>
    public partial class Wj9001LeakDataQueryViewModel
    {
        private void RequestData()
        {
            var tStartTime = new DateTime(DtStart.Year, DtStart.Month, DtStart.Day, 0, 0, 1);
            var tEndTime = new DateTime(DtEnd.Year, DtEnd.Month, DtEnd.Day, 23, 59, 59);

           // LogInfo.Log("正在请求所有类型信息!!!");
           // Remind = "查询命令已发送...";
            var info = Sr.ProtocolPhone.LxLeak.wst_leak_datas;//.wst_ldu_data;// .wlst_cnt_wj1090_request_data ;//.ServerPart.wlst_Wj1090_clinet_request_Data;
            if (IsLockTml)
            {
               // info.WstLduHisData  .AttachRtuId = AttachId;
                info.WstLeakDatas.RtuId = RtuId;
                //info.WstLeakDatas.LineIds .Add( LduLineId );// = LduLineId;
            }
            else
            {
                //info.WstLduHisData.AttachRtuId = AttachId;
                info.WstLeakDatas.RtuId = 0;
                //info.WstLduHisData.LoopId = 0;
            }
            if (IsOldFaultQuery ==false )
            {
                info.WstLeakDatas.DtStartTime = 0;
                info.WstLeakDatas.DtEndTime = 0;
            }else
            {
                info.WstLeakDatas.DtStartTime = tStartTime.Ticks;
                info.WstLeakDatas.DtEndTime = tEndTime.Ticks;
            }

            SndOrderServer.OrderSnd(info, 10, 6);
            Remind =  "正在查询 ...";
        }

        //http请求
        private void RequestHttpData(int pageIndex, int pagingFlag)
        {
            var tStartTime = new DateTime(DtStart.Year, DtStart.Month, DtStart.Day, 0, 0, 1);
            var tEndTime = new DateTime(DtEnd.Year, DtEnd.Month, DtEnd.Day, 23, 59, 59);

            // LogInfo.Log("正在请求所有类型信息!!!");
            // Remind = "查询命令已发送...";
            var info = Sr.ProtocolPhone.LxLeakHttp.wst_leak_data_record_http;
                //.wst_ldu_data;// .wlst_cnt_wj1090_request_data ;//.ServerPart.wlst_Wj1090_clinet_request_Data;
            if (IsLockTml)
            {
                // info.WstLduHisData  .AttachRtuId = AttachId;
                info.WstLeakDatas.RtuId = RtuId;
                //info.WstLeakDatas.LineIds .Add( LduLineId );// = LduLineId;
            }
            else
            {
                //info.WstLduHisData.AttachRtuId = AttachId;
                info.WstLeakDatas.RtuId = 0;
                //info.WstLduHisData.LoopId = 0;
            }
            if (IsOldFaultQuery == false)
            {
                info.WstLeakDatas.DtStartTime = 0;
                info.WstLeakDatas.DtEndTime = 0;
            }
            else
            {
                info.WstLeakDatas.DtStartTime = tStartTime.Ticks;
                info.WstLeakDatas.DtEndTime = tEndTime.Ticks;
            }
            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (data == null) return;
            Remind = "正在查询 ...";
            ResolveRequestData(data, pagingFlag);
        }
    }
}
