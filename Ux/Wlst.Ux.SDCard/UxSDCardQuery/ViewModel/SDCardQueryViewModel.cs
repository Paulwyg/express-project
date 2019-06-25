using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.SDCard.UxSDCardQuery.Services;
using Wlst.Ux.WJ3005Module.ZDataQuery.DailyDataQuery.ViewModel;
using Wlst.client;

namespace Wlst.Ux.SDCard.UxSDCardQuery.ViewModel
{
    [Export(typeof(IIUxSDCardQueryModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class SDCardQueryViewModel : EventHandlerHelperExtendNotifyProperyChanged, Cr.Core.CoreInterface.IITab, IIUxSDCardQueryModule
    {
        public void NavOnLoad(params object[] parsObjects)
        {
            _thisViewActive = true;

            DateTime time = DateTime.Now.AddDays(-1);

            DtStartTime = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            DtEndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0); ;

            CurrentSelectedRecordType = RecordTypeItems[0];
        }

        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "SD卡查询"; }
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

        #region Attri
        private Visibility _dataVisi;
        public Visibility DataVisi
        {
            get { return _dataVisi; }
            set
            {
                if (_dataVisi != value)
                {
                    _dataVisi = value;
                    this.RaisePropertyChanged(() => this.DataVisi);
                }
            }
        }

        private Visibility _maxsdataVisi;
        public Visibility MaxDataVisi
        {
            get { return _maxsdataVisi; }
            set
            {
                if (_maxsdataVisi != value)
                {
                    _maxsdataVisi = value;
                    this.RaisePropertyChanged(() => this.MaxDataVisi);
                }
            }
        }


        private bool _thisViewActive = false;

        private Visibility _recordCountVisi = Visibility.Hidden;
        public Visibility RecordCountVisi
        {
            get { return _recordCountVisi; }
            set
            {
                if (value == _recordCountVisi) return;
                _recordCountVisi = value;
                RaisePropertyChanged(() => RecordCountVisi);
            }
        }

        private Visibility _selectAllTmlVisi = Visibility.Hidden;
        public Visibility SelectAllTmlVisi
        {
            get { return _selectAllTmlVisi; }
            set
            {
                if (value == _selectAllTmlVisi) return;
                _selectAllTmlVisi = value;
                RaisePropertyChanged(() => SelectAllTmlVisi);
            }
        }

        private bool _isAllTmlChecked;

        public bool IsAllTmlChecked
        {
            get { return _isAllTmlChecked; }
            set
            {
                if (_isAllTmlChecked != value)
                {
                    _isAllTmlChecked = value;

                    RaisePropertyChanged(() => IsAllTmlChecked);
                }
            }
        }

        private int _recordCount = 100;
        public int RecordCount
        {
            get { return _recordCount; }
            set
            {
                if (_recordCount != value)
                {
                    _recordCount = value;
                    this.RaisePropertyChanged(() => this.RecordCount);
                }
            }
        }

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

        private int _phyId;
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (_phyId != value)
                {
                    _phyId = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private int _rtuId;
        public int RtuId
        {
            get { return _rtuId; }
            set
            {

                if (value != _rtuId)
                {
                    _rtuId = value;
                    RaisePropertyChanged(() => RtuId);

                    if (_rtuId == 0)
                    {
                        PhyId = 0;
                        this.RtuName = "--";
                    }

                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (_rtuId))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [_rtuId];
                    this.RtuName = tml.RtuName;
                    PhyId = tml.RtuPhyId;
                }
            }
        }

        private DateTime _dtEndTime;
        /// <summary>
        /// 结束时间
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

        private DateTime _dtStartTime;
        /// <summary>
        /// 起始时间
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

        private NameValueInt _currentSelectedRecordType;
        public NameValueInt CurrentSelectedRecordType
        {
            get { return _currentSelectedRecordType; }
            set
            {
                if (value != _currentSelectedRecordType)
                {
                    _currentSelectedRecordType = value;
                    RaisePropertyChanged(() => CurrentSelectedRecordType);

                    if (_currentSelectedRecordType == _recordTypeItems[1])
                    {
                        RecordCountVisi = Visibility.Hidden;
                        SelectAllTmlVisi = Visibility.Visible;
                        DataVisi = Visibility.Hidden;
                        MaxDataVisi = Visibility.Visible;
                    }
                    else
                    {
                        RecordCountVisi = Visibility.Visible;
                        SelectAllTmlVisi = Visibility.Hidden;
                        MaxDataVisi = Visibility.Hidden;
                        DataVisi = Visibility.Visible;
                    }

                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _recordTypeItems;
        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> RecordTypeItems
        {
            get
            {
                if (_recordTypeItems == null)
                {
                    _recordTypeItems = new ObservableCollection<NameValueInt>();

                    _recordTypeItems.Add(new NameValueInt
                    {
                        Name = "普通数据",
                        Value = 0
                    });

                    _recordTypeItems.Add(new NameValueInt
                    {
                        Name = "最大电流数",
                        Value = 1
                    });
                }
                return _recordTypeItems;
            }
        }


        private ObservableCollection<TmlNewDataViewModelExtend> _record;
        public ObservableCollection<TmlNewDataViewModelExtend> Records
        {
            get
            {

                if (_record == null)
                    _record = new ObservableCollection<TmlNewDataViewModelExtend>();
                return _record;
            }
            set
            {
                if (_record == value) return;
                _record = value;
                this.RaisePropertyChanged(() => this.Records);
            }
        }

        private TmlNewDataViewModelExtend _currentSelectRecord;

        public TmlNewDataViewModelExtend CurrentSelectRecord
        {
            get { return _currentSelectRecord ?? (_currentSelectRecord = new TmlNewDataViewModelExtend()); }
            set
            {
                if (_currentSelectRecord != value)
                {
                    _currentSelectRecord = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectRecord);

                    if (_currentSelectRecord == null || _currentSelectRecord.DataInfo == null || _currentSelectRecord.DataInfo.RtuTemperature < -50)
                    { return; }

                    var tmps =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            _currentSelectRecord.DataInfo.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                    var args = new PublishEventArgs
                    {
                        EventType = PublishEventType.Core,
                        EventId =
                            Sr.EquipmentInfoHolding.Services.EventIdAssign.
                            RtuDataQueryDataInfoNeedShowInTab,
                    };
                    args.AddParams(_currentSelectRecord.DataInfo);
                    EventPublish.PublishEvent(args);

                    EventPublish.PublishEvent(new PublishEventArgs() { EventType = "MainWindow.Measure.show" });
                }
            }
        }

        private ObservableCollection<MaxCurrentRecords> _maxRecord;
        public ObservableCollection<MaxCurrentRecords> MaxRecords
        {
            get
            {

                if (_maxRecord == null)
                    _maxRecord = new ObservableCollection<MaxCurrentRecords>();
                return _maxRecord;
            }
            set
            {
                if (_maxRecord == value) return;
                _maxRecord = value;
                this.RaisePropertyChanged(() => this.MaxRecords);
            }
        }
        #endregion

        public SDCardQueryViewModel()
        {
            InitAction();
            InitEvent();
        }

        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {

            if (_thisViewActive == false) return;
            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {

                    int id = Convert.ToInt32(args.GetParams()[0]);
                    if (id > 1100000)
                    {
                        var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                        if (tmps == null) return;
                        id = tmps.RtuFid;
                    }
                    if (id < 1000000 || id > 1100000) return;

                    SelectRtuIdChange(id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 选中终端
        /// </summary>
        /// <param name="rtuId"></param>
        private void SelectRtuIdChange(int rtuId)
        {
            if (rtuId < 1) return;
            //todo  request data
            if (rtuId != this.RtuId)
            {
                this.RtuId = rtuId;
                //Query();
            }
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxRtu.wst_rtu_sd_reply,
                OnRtuSDReplay,
                typeof(SDCardQueryViewModel), this);
        }

        private bool IfIndexExist(int _inputIndex, ref int _outputIndex)
        {
            int m = 0;

            foreach (var t in Records)
            {
                if (t.Index > _inputIndex)
                {
                    _outputIndex = m;

                    return false;
                }
                else if (t.Index == _inputIndex)
                {
                    _outputIndex = m;

                    return true;
                }

                m++;
            }

            _outputIndex = Records.Count;

            return false;
        }

        private int RecordTotalCount = 0;
        private int RecordIndex = 0;

        public void OnRtuSDReplay(string session, Wlst.mobile.MsgWithMobile infos)
        {
            bool add = false;

            RecordTotalCount++;

            if (infos.WstRtuSdReply.RecordStatus == 0)
            {
                //Console.WriteLine("RecordIdx:" + infos.WstRtuSdReply.RecordIdx);
                //Console.WriteLine("RecordTotal:" + infos.WstRtuSdReply.RecordTotal);
                //Console.WriteLine("RecordCount:" + infos.WstRtuSdReply.DataInfo.Count);

                if (infos.WstRtuSdReply.RecordType == 1)
                {
                    int insertIndex = 0;
                    bool res = IfIndexExist(infos.WstRtuSdReply.RecordIdx, ref insertIndex);

                    if (res)
                    {
                        Records.RemoveAt(insertIndex);
                    }

                    foreach (var t in infos.WstRtuSdReply.DataInfo)
                    {
                        var f = new TmlNewDataViewModelExtend(t);
                        f.Index = infos.WstRtuSdReply.RecordIdx;
                        Records.Insert(insertIndex, f);
                    }

                    Remind = "共传输" + RecordTotalCount + "条数据, 其中" + Records.Count + "正确, " + (RecordTotalCount - Records.Count) + "接收错误！";

                }
                else if (infos.WstRtuSdReply.RecordType == 2)
                {
                    int loopCount = infos.WstRtuSdReply.DataMax[0].LoopCount;

                    var terInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(infos.WstRtuSdReply.RtuId);

                    if (terInfo != null)
                    {
                        var tmps = terInfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                        var loopName = new string[loopCount];

                        if (tmps != null)
                        {
                            int m = 0;

                            foreach (var tt in tmps.WjLoops)
                            {
                                if (m < loopCount)
                                {
                                    loopName[m++] = tt.Value.LoopName;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        for (int i = 0; i < loopCount; i++)
                        {

                            MaxRecords.Add(new MaxCurrentRecords
                            {
                                Index = i + 1,
                                RtuId = infos.WstRtuSdReply.RtuId,
                                LoopId = i + 1,
                                LoopName = loopName[i],
                                DtGetDataTime = (new DateTime(infos.WstRtuSdReply.DataMax[0].MaxDataField[i].DtRecord)).ToString("yyyy-MM-dd HH:mm:ss"),
                                MaxCurrent = infos.WstRtuSdReply.DataMax[0].MaxDataField[i].CurrentMax.ToString("f2")

                            });
                        }
                    }



                    Remind = "数据传输完成，共" + MaxRecords.Count + "条数据！";
                }
            }
            else
            {
                if ((infos.WstRtuSdReply.RecordStatus >= 1) && (infos.WstRtuSdReply.RecordStatus <= 6))
                {
                    Remind = "当前第" + RecordTotalCount + "条数据错误！SD卡功能错误！";
                }
                else if (infos.WstRtuSdReply.RecordStatus == 7)
                {
                    Remind = "当前第" + RecordTotalCount + "条数据错误！无SD卡！";
                }
                else if (infos.WstRtuSdReply.RecordStatus == 8)
                {
                    Remind = "当前第" + RecordTotalCount + "条数据错误！记录时间错误！";
                    //if (infos.WstRtuSdReply.RecordType == 1)
                    //{
                    //    Remind = "共传输" + RecordTotalCount + "条数据, 其中" + Records.Count + "正确, " +
                    //             (RecordTotalCount - Records.Count) + "接收错误！";
                    //}
                    //else
                    //{
                    //    Remind = "共传输" + RecordTotalCount + "条数据, 其中" + MaxRecords.Count + "正确, " +
                    //             (RecordTotalCount - MaxRecords.Count) + "接收错误！";
                    //}
                }
                else if (infos.WstRtuSdReply.RecordStatus == 9)
                {
                    Remind = "当前第" + RecordTotalCount + "条数据错误！查询开始时间错误！";
                }
                else if (infos.WstRtuSdReply.RecordStatus == 10)
                {
                    Remind = "当前第" + RecordTotalCount + "条数据错误！查询截止时间错误！";
                }
                else if (infos.WstRtuSdReply.RecordStatus == 11)
                {
                    Remind = "当前第" + RecordTotalCount + "条数据错误！时间间隔错误！";
                }
                else if (infos.WstRtuSdReply.RecordStatus == 12)
                {
                    Remind = "当前第" + RecordTotalCount + "条数据错误！记录类型错误！";
                }
                else if (infos.WstRtuSdReply.RecordStatus == 13)
                {
                    Remind = "当前第" + RecordTotalCount + "条数据错误！记录数据错误！";
                    //if (infos.WstRtuSdReply.RecordType == 1)
                    //{
                    //    Remind = "共传输" + RecordTotalCount + "条数据, 其中" + Records.Count + "正确, " +
                    //             (RecordTotalCount - Records.Count) + "接收错误！";
                    //}
                    //else
                    //{
                    //    Remind = "共传输" + RecordTotalCount + "条数据, 其中" + MaxRecords.Count + "正确, " +
                    //             (RecordTotalCount - MaxRecords.Count) + "接收错误！";
                    //}
                }
                else
                {
                    Remind = "当前第" + RecordTotalCount + "条数据错误！未知错误！";
                }
            }
        }

        #region CmdQuery

        private DateTime _dtQuery = DateTime.Now;

        public ICommand CmdQuery
        {
            get { return new RelayCommand(Ex, CanEx, true); }
        }

        private void Ex()
        {

            int _rtuid = RtuId;

            if (DtStartTime.Ticks >= DtEndTime.Ticks)
            {
                Remind = "时间设置错误，请检查！";
                return;
            }

            if (CurrentSelectedRecordType.Value == 0)
            {
                if (RtuId == 0)
                {
                    Remind = "请先选择终端！";
                    return;
                }

                if (RecordCount == 0)
                {
                    Remind = "记录数设置错误，请检查！";
                    return;
                }
            }
            else if (CurrentSelectedRecordType.Value == 1)
            {
                if (IsAllTmlChecked == false)
                {
                    if (RtuId == 0)
                    {
                        Remind = "请先选择终端！";
                        return;
                    }
                }
                else
                {
                    _rtuid = 0;
                }

                if (DtStartTime.Ticks >= (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)).Ticks)
                {
                    Remind = "时间设置错误，请检查！";
                    return;
                }
            }

            _dtQuery = DateTime.Now;

            var ntsnd = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_sd_request;

            ntsnd.WstRtuSdRequest.CmdIdx = 1;

            ntsnd.WstRtuSdRequest.RtuId = _rtuid;
            ntsnd.WstRtuSdRequest.RecordType = CurrentSelectedRecordType.Value + 1;

            long endTime = DtEndTime.Ticks;

            if (DtEndTime.Ticks > DateTime.Now.Ticks)
            {
                endTime = DateTime.Now.Ticks;
            }

            if (ntsnd.WstRtuSdRequest.RecordType == 1)
            {
                ntsnd.WstRtuSdRequest.DtStart = DtStartTime.Ticks;
                ntsnd.WstRtuSdRequest.RecordCount = RecordCount;
                ntsnd.WstRtuSdRequest.RecordDistance = (((endTime - DtStartTime.Ticks) / RecordCount / 10000000) / 2) * 2;
            }
            else if (ntsnd.WstRtuSdRequest.RecordType == 2)
            {
                var DTendTime = new DateTime(endTime);


                if (endTime < (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)).Ticks)
                {
                    endTime = (new DateTime(DTendTime.Year, DTendTime.Month, DTendTime.Day, 0, 0, 0).AddDays(1)).Ticks;
                }
                else
                {
                    endTime = (new DateTime(DTendTime.Year, DTendTime.Month, DTendTime.Day, 0, 0, 0)).Ticks;
                }


                ntsnd.WstRtuSdRequest.DtStart =
                    (new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 0)).Ticks;
                ntsnd.WstRtuSdRequest.RecordCount =
                    Math.Max(1, Convert.ToInt32((endTime - ntsnd.WstRtuSdRequest.DtStart) / 864000000000));
                ntsnd.WstRtuSdRequest.RecordDistance = 86400;
            }

            if (CurrentSelectedRecordType.Value == 0)
            {
                Records.Clear();
            }
            else
            {
                MaxRecords.Clear();
            }

            RecordTotalCount = 0;

            SndOrderServer.OrderSnd(ntsnd);

            Remind = "查询命令已发送...请等待数据反馈！";
        }

        private bool CanEx()
        {
            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }

        #endregion

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
            if (DataVisi == Visibility.Visible)
            {

                try
                {
                    var lsttitle = new List<Object>();
                    lsttitle.Add("序号");
                    lsttitle.Add("终端ID");
                    lsttitle.Add("终端名称");
                    lsttitle.Add("采集时间");
                    lsttitle.Add("A相电压");
                    lsttitle.Add("B相电压");
                    lsttitle.Add("C相电压");
                    lsttitle.Add("A相电流");
                    lsttitle.Add("B相电流");
                    lsttitle.Add("C相电流");
                    lsttitle.Add("总电流");
                    lsttitle.Add("K1状态");
                    lsttitle.Add("K2状态");
                    lsttitle.Add("K3状态");
                    lsttitle.Add("K4状态");
                    lsttitle.Add("K5状态");
                    lsttitle.Add("K6状态");
                    lsttitle.Add("K7状态");
                    lsttitle.Add("K8状态");
                    lsttitle.Add("A相功率");
                    lsttitle.Add("B相功率");
                    lsttitle.Add("C相功率");
                    lsttitle.Add("总功率");
                    lsttitle.Add("总功率因数");

                    int loopcount = 0;
                    if (Records.Count > 0)
                    {
                        loopcount = Records[0].LstNewLoopsData.Count;
                        foreach (var g in Records[0].LstNewLoopsData)
                        {
                            lsttitle.Add("回路" + g.LoopId + "电压");
                            lsttitle.Add("回路" + g.LoopId + "电流");
                            lsttitle.Add("回路" + g.LoopId + "功率");
                        }
                    }
                    var lstobj = new List<List<object>>();

                    foreach (var g in Records)
                    {
                        var tmp = new List<object>();
                        tmp.Add(g.Index);
                        tmp.Add(g.RtuId);
                        tmp.Add(g.RtuName);
                        tmp.Add(g.DtGetDataTime);
                        tmp.Add(g.RtuVoltageA);
                        tmp.Add(g.RtuVoltageB);
                        tmp.Add(g.RtuVoltageC);
                        tmp.Add(g.RtuCurrentSumA);
                        tmp.Add(g.RtuCurrentSumB);
                        tmp.Add(g.RtuCurrentSumC);
                        tmp.Add(g.RtuCurrentTotal);

                        tmp.Add(g.LstIsSwitchOutAttraction.Count > 0
                                    ? (g.LstIsSwitchOutAttraction[0].IsSelected ? "开灯" : "关灯")
                                    : "--");
                        tmp.Add(g.LstIsSwitchOutAttraction.Count > 1
                                    ? (g.LstIsSwitchOutAttraction[1].IsSelected ? "开灯" : "关灯")
                                    : "--");
                        tmp.Add(g.LstIsSwitchOutAttraction.Count > 2
                                    ? (g.LstIsSwitchOutAttraction[2].IsSelected ? "开灯" : "关灯")
                                    : "--");
                        tmp.Add(g.LstIsSwitchOutAttraction.Count > 3
                                    ? (g.LstIsSwitchOutAttraction[3].IsSelected ? "开灯" : "关灯")
                                    : "--");
                        tmp.Add(g.LstIsSwitchOutAttraction.Count > 4
                                    ? (g.LstIsSwitchOutAttraction[4].IsSelected ? "开灯" : "关灯")
                                    : "--");
                        tmp.Add(g.LstIsSwitchOutAttraction.Count > 5
                                    ? (g.LstIsSwitchOutAttraction[5].IsSelected ? "开灯" : "关灯")
                                    : "--");
                        tmp.Add(g.LstIsSwitchOutAttraction.Count > 6
                                    ? (g.LstIsSwitchOutAttraction[6].IsSelected ? "开灯" : "关灯")
                                    : "--");
                        tmp.Add(g.LstIsSwitchOutAttraction.Count > 7
                                    ? (g.LstIsSwitchOutAttraction[7].IsSelected ? "开灯" : "关灯")
                                    : "--");

                        tmp.Add(g.RtuPowerSumA);
                        tmp.Add(g.RtuPowerSumB);
                        tmp.Add(g.RtuPowerSumC);
                        tmp.Add(g.RtuPowerSum);
                        tmp.Add(g.RtuTotalPowerFactor);

                        int indexx = 0;
                        foreach (var m in g.LstNewLoopsData)
                        {
                            if (loopcount <= indexx) continue;
                            indexx++;

                            tmp.Add(m.V);
                            tmp.Add(m.A);
                            tmp.Add(m.Power);
                        }

                        lstobj.Add(tmp);
                    }
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                    lstobj = null;
                    lsttitle = null;

                }
                catch (Exception e)
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出巡测报表时报错:" + e);
                }
            }
            else if (MaxDataVisi == Visibility.Visible)
            {

                try
                {
                    var lsttitle = new List<Object>();
                    lsttitle.Add("序号");
                    lsttitle.Add("终端地址");
                    lsttitle.Add("终端名称");
                    lsttitle.Add("采集时间");
                    lsttitle.Add("回路序号");
                    lsttitle.Add("回路名称");
                    lsttitle.Add("最大电流");


                    var lstobj = new List<List<object>>();

                    foreach (var g in MaxRecords)
                    {
                        var tmp = new List<object>();
                        tmp.Add(g.Index);
                        tmp.Add(g.RtuId);
                        tmp.Add(g.RtuName);
                        tmp.Add(g.DtGetDataTime);
                        tmp.Add(g.LoopId);
                        tmp.Add(g.LoopName);
                        tmp.Add(g.MaxCurrent);

                        lstobj.Add(tmp);
                    }
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                    lstobj = null;
                    lsttitle = null;

                }
                catch (Exception e)
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出巡测报表时报错:" + e);
                }
            }

        }

        private bool CanExCmdExport()
        {
            if (DataVisi == Visibility.Visible)
            {
                if (Records.Count < 1) return false;
            }
            else if (MaxDataVisi == Visibility.Visible)
            {
                if (MaxRecords.Count < 1) return false;
            }
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }

        #endregion

    }
}
