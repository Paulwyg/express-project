using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows;
using System.Windows.Input;

using Microsoft.Win32;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.WJ3005Module.ZDataQuery.SndWeekTimeQuery.Services;

namespace Wlst.Ux.WJ3005Module.ZDataQuery.SndWeekTimeQuery.ViewModel
{
    [Export(typeof (IISndWeekTimeQueryViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SndWeekTimeQueryViewModel :EventHandlerHelperExtendNotifyProperyChanged,
        Services.IISndWeekTimeQueryViewModel
    {
        public SndWeekTimeQueryViewModel()
        {
            InitAction();
            InitEvent();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            _isThisViewActive = true;
            Record.Clear();
            DtEndTime = DateTime.Now;
            DtStartTime = DateTime.Now.AddDays(-7);
            try
            {
                if (parsObjects.Length > 0)
                {
                    RtuId = Convert.ToInt32(parsObjects[0]);
                    if (RtuId > 0)
                    {
                       // this.Query(DtStartTime, DtEndTime, RtuId);
                        IsLock = true;
                        //ExQuery();
                        RequestHttpData(DtStartTime, DtEndTime, RtuId, 1, 0);
                    }
                    else
                    {
                        PhyId = 0;
                        IsLock = false;
                        RtuInfo = "0 - 所有终端";
                    }
                }
                else
                {
                    PhyId = 0;
                    IsLock = false;
                    RtuInfo = "0 - 所有终端";
                }
            }
            catch (Exception ex)
            {

            }
            Remind = "请选择终端...";
        }

        public void OnUserHideOrClosing()
        {
            _isThisViewActive = false;
            Record = new ObservableCollection<SndWeekTimeOneRecordViewModel>();
            ExportVisi=Visibility.Collapsed;
            ItemCount = 0;
            PageTotal = "";
            RtuInfo = "";
            RtuName = "";
        }

        #region iitabl
        public int Index
        {
            get { return 1; }
        }
        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "周设置记录查询"; }
        }

        #endregion
    }
    public partial class SndWeekTimeQueryViewModel
    {
        #region Field
        private bool _isThisViewActive = false;
        private bool _isOnExport = true;
        #endregion

        #region arrti

        #region IsLock

        private bool _isLock;
        public bool IsLock
        {
            get { return _isLock; }
            set
            {
                if(_isLock==value) return;
                _isLock = value;
                if(value==false)
                {
                    //PhyId = 0;
                    RtuInfo = "0 - 所有终端";
                }
                else
                {
                    RtuInfo = PhyId + " - " + RtuName;
                }
                RaisePropertyChanged(()=>IsLock);
                
            }
        }
        #endregion

        #region Record
        private ObservableCollection<SndWeekTimeOneRecordViewModel> _record;

        public ObservableCollection<SndWeekTimeOneRecordViewModel> Record
        {
            get
            {
                if (_record == null)
                    _record = new ObservableCollection<SndWeekTimeOneRecordViewModel>();
                return _record;
            }
            set
            {
                if (_record == value) return;
                _record = value;
                this.RaisePropertyChanged(() => this.Record);
            }
        }
        #endregion

        #region ExportVisi

        private Visibility _exportVisi = Visibility.Collapsed;
        public Visibility ExportVisi
        {
            get { return _exportVisi; }
            set
            {
                if (value == _exportVisi) return;
                _exportVisi = value;
                RaisePropertyChanged(() => ExportVisi);
            }
        }
        #endregion

        #region PagerVisi

        private Visibility _pagerVisi = Visibility.Collapsed;
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

        #region  DtStartTime
        private DateTime _dtStartTime;

        /// <summary>
        /// 
        /// </summary>
        public DateTime DtStartTime
        {
            get { return _dtStartTime; }
            set
            {
                if (value != _dtStartTime)
                {
                    if (value.Ticks >= DateTime.Now.Ticks) value = DateTime.Now;
                    _dtStartTime = value;
                    this.RaisePropertyChanged(() => this.DtStartTime);
                }
            }
        }
        #endregion

        #region DtEndTime
        private DateTime _dtEndTime;

        /// <summary>
        /// 
        /// </summary>
        public DateTime DtEndTime
        {
            get { return _dtEndTime; }
            set
            {
                if (value != _dtEndTime)
                {
                    if (value.Ticks >= DateTime.Now.Ticks) value = DateTime.Now;
                    _dtEndTime = value;
                    this.RaisePropertyChanged(() => this.DtEndTime);
                }
            }
        }
        #endregion

        #region RtuId
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
                    if (_rtuId == 0)
                    {
                        //  IsLock = false;
                        RtuInfo = "0 - 所有终端设备.";
                        return;
                    }
                    //RtuName = "" + _rtuId;
                    if (
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                            InfoItems .ContainsKey(_rtuId))
                    {
                        var info =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                                InfoItems [_rtuId];
                        this.RtuName = info.RtuName;
                        PhyId = info.RtuPhyId ;
                        if (IsLock)
                        {
                            RtuInfo = PhyId + " - " + RtuName;
                        }
                    }
                    else
                    {
                        PhyId = value ;
                    }
                }
            }
        }

         private int _phyid;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int PhyId
        {
            get { return _phyid; }
            set
            {
                if (value != _phyid)
                {
                    _phyid = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        #endregion

        #region RtuName
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
        #endregion

        #region RtuInfo
        private string _rtuInfo;

        /// <summary>
        /// 选中终端信息
        /// </summary>
        public string RtuInfo
        {
            get { return _rtuInfo; }
            set
            {
                if (value != _rtuInfo)
                {
                    _rtuInfo = value;
                    this.RaisePropertyChanged(() => this.RtuInfo);
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

        private int _pageIndex ;

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
                    RequestHttpData(DtStartTime, DtEndTime, RtuId, PageIndex,1);

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

        #region PagerRecords
        /// <summary>
        /// 分页数据
        /// </summary>
        private ObservableCollection<SndWeekTimeOneRecordViewModel> _pagerRecords;

        public ObservableCollection<SndWeekTimeOneRecordViewModel> PagerRecords
        {
            get
            {

                if (_pagerRecords == null)
                    _pagerRecords = new ObservableCollection<SndWeekTimeOneRecordViewModel>();
                return _pagerRecords;
            }
            set
            {
                if (_pagerRecords == value) return;
                _pagerRecords = value;
                this.RaisePropertyChanged(() => this.PagerRecords);
            }
        }
        #endregion
        #endregion


        #endregion

        #region ICommand

        #region CmdQuery

        private DateTime _dtQuery;
        private ICommand _cmdQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdQuery == null) _cmdQuery = new RelayCommand(ExQuery, CanExQuery, true);
                return _cmdQuery;
            }
        }

        private void ExQuery()
        {
            _dtQuery = DateTime.Now;
            //Query(DtStartTime, DtEndTime, RtuId);
            ExportVisi = Visibility.Visible;
            PageIndex = 0;
            RequestHttpData(DtStartTime, DtEndTime, RtuId, PageIndex, 0);
            _isOnExport = false;
        }

        private bool CanExQuery()
        {
            if (DtStartTime.Ticks < DtEndTime.Ticks) return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
            return false;
        }

        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// 选中终端变化  提取数据
        /// </summary>
        /// <param name="rtuId"></param>
        private void SelectRtuIdChange(int rtuId)
        {
            if (rtuId < 1) return;
            RtuId = rtuId;
            // Record.Clear();
            //Query(DtStartTime, DtEndTime, RtuId);
            if (IsLock)
                this.Record.Clear();
        }

        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class SndWeekTimeQueryViewModel 
    {
      //  private Thread _thread;
        public void InitAction()
        {
            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr .ProtocolPhone .LxRtu .wst_weekset_snd_record ,// .wlst_svr_ans_cnt_request_weekset_record  ,
            //    RecordWeekTimeRequest,
            //    typeof(SndWeekTimeQueryViewModel), this);
        }

        public void RecordWeekTimeRequest( Wlst .mobile .MsgWithMobile  infos , int pagingFlag)
        {
            var info = infos.WstRtuWeeksetSndRecord;
            if (info == null) return;
            //this.RtuId = info.RtuId ;

            var tmpitems = new ObservableCollection<SndWeekTimeOneRecordViewModel>();
            int index = 0;

            var phyid = info.RtuId;
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(info.RtuId))
                phyid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[info.RtuId].RtuPhyId;

            if (pagingFlag == 0)
            {
                PageSize = infos.Head.PagingNum;
                ItemCount = infos.Head.PagingRecordTotal;
                var count = ItemCount / PageSize + (ItemCount % PageSize > 0 ? 1 : 0);
                PagerVisi = count < 2 ? Visibility.Collapsed : Visibility.Visible;
                PageTotal = "页 " + ItemCount + " 条";
            }

            if(info.Info.Count==0)
            {
                //Remind = "没有所需要查询记录！";
                Record.Clear();
                Remind = "查询成功.";
     
                return;
            }

            foreach (var t in info.Info)
            {
                index++;
                tmpitems.Add(new SndWeekTimeOneRecordViewModel(t, PageIndex*PageSize+index));
            }
            this.Record = tmpitems;
            //Remind = "数据已反馈！";

            Remind = "查询成功.";
        }
        //private void ResoloveRequestAnsData(object data)
        //{
        //    var list = data as List<RecordWeekTime>;
        //    if(list==null) return;
        //    foreach (var item in list)
        //    {
        //     //   Wlst.Cr.Core.CoreServices.AsynObservableCollectionAdd.Insert(Record, new SndWeekTimeOneRecordViewModel(item));
        //        Record.Add(new SndWeekTimeOneRecordViewModel(item));
        //    }
        //  //  Remind = "数据反馈完毕，请查看数据！";
         
        //}


        private void InitEvent()
        {
            AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected, PublishEventType.Core);
        }

        public override void ExPublishedEvent( PublishEventArgs args)
        {
            if (!_isThisViewActive) return;
            switch (args.EventId)
            {

                case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:
                    {

                        var id = Convert.ToInt32(args.GetParams()[0]);
                        if ( id > 1100000)
                        {
                            var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( id);
                            if (tmps == null) return;
                            id = tmps.RtuFid ;
                        }
                        if (id < 1000000 || id > 1100000) return;
                        //if(IsLock)
                        //{
                            SelectRtuIdChange(id); 
                        //}
                    }
                    break;
            }
        }

    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class SndWeekTimeQueryViewModel
    {
        private void Query(DateTime dtstarttime, DateTime dtendtime, int tml)
        {
            var tStartTime = new DateTime(dtstarttime.Year, dtstarttime.Month, dtstarttime.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59);

            if (!GetCheckedInformation()) return;
            this.Record.Clear();
            //Remind = "查询命令已发送...请等待数据反馈！";
            var info = Wlst.Sr.ProtocolPhone.LxRtu .wst_weekset_snd_record ;//.wlst_cnt_request_weekset_record;
            info.WstRtuWeeksetSndRecord .DtEndTime  = tEndTime.Ticks;
            info.WstRtuWeeksetSndRecord.DtStartTime = tStartTime.Ticks;

            info.WstRtuWeeksetSndRecord.RtuId = IsLock ? tml : 0;
            SndOrderServer.OrderSnd(info, 10, 6);


            var phyid = tml ;
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(tml ))
                phyid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tml ].RtuPhyId;
            Remind = "正在查询...";
        }

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

        //http请求
        private void RequestHttpData(DateTime dtstarttime, DateTime dtendtime, int tml, int pageIndex, int pagingFlag)
        {
            var tStartTime = new DateTime(dtstarttime.Year, dtstarttime.Month, dtstarttime.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59);

            if (!GetCheckedInformation()) return;
            this.Record.Clear();
            if (IsLock && tml == 0) return;
            //Remind = "查询命令已发送...请等待数据反馈！";
            var info = Wlst.Sr.ProtocolPhone.LxRtuHttp.wst_weekset_snd_record_http;//.wlst_cnt_request_weekset_record;
            info.WstRtuWeeksetSndRecord.DtEndTime = tEndTime.Ticks;
            info.WstRtuWeeksetSndRecord.DtStartTime = tStartTime.Ticks;

            info.WstRtuWeeksetSndRecord.RtuId = IsLock ? tml : 0;

            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (data == null) return;
            //SndOrderServer.OrderSnd(info, 10, 6);
            RecordWeekTimeRequest(data,pagingFlag);
        }
    }
}
