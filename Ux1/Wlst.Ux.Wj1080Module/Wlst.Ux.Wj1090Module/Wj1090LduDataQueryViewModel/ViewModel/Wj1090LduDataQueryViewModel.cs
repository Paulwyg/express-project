using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Sr.ProtocolCnt.Wj1090;
using Wlst.Ux.Wj1090Module.Wj1090LduDataQueryViewModel.Services;

namespace Wlst.Ux.Wj1090Module.Wj1090LduDataQueryViewModel.ViewModel
{
    [Export(typeof(IIWj1090LduDataQueryView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090LduDataQueryViewModel : EventHandlerHelperExtendNotifyProperyChanged,
                                       Cr.Core.CoreInterface.IINavOnLoad, Cr.Core.CoreInterface.IITab, IIWj1090LduDataQueryView
    {

        private bool _isThisViewActive = false;
        public void NavOnLoad(params object[] parsObjects)
        {
            FlagDetectionVisi = true;
            FlagAlarmVisi = true;
            _isThisViewActive = true;
            DtStart = DateTime.Now.AddDays(-1);
            DtEnd = DateTime.Now;
            try
            {
                int rtuid = Convert.ToInt32(parsObjects[0]);
                if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLine(rtuid)) return;

                var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(rtuid);
                if (info == null) return;
                AttachId = info.AttachRtuId == 0 ? rtuid : info.AttachRtuId;
                RtuId = rtuid;
                LduLineId = 0;

                AttachVisi = AttachId.Equals(0) ? Visibility.Collapsed : Visibility.Visible;
                RtuVisi = RtuId.Equals(0) ? Visibility.Collapsed : Visibility.Visible;
                LduLineVisi = LduLineId.Equals(0) ? Visibility.Collapsed : Visibility.Visible;
            }
            catch (Exception ec)
            {

            }

        }

        public void OnUserHideOrClosing()
        {
            _isThisViewActive = false;
            Items = new ObservableCollection<LduDataQueryoneItemViewModel>();

            //throw new NotImplementedException();
        }

        #region Tab
        public string Title
        {
            get { return "防盗数据查询"; }
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

        public Wj1090LduDataQueryViewModel()
        {
            InitAction();
            InitEvent();

        }
    }

    /// <summary>
    /// 属性，Field
    /// </summary>
    public partial class Wj1090LduDataQueryViewModel
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
                    !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                         EquipmentInfoDictionary.ContainsKey
                         (_attachId))
                    return;
                var tml =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary
                        [_attachId];
                AttachName = tml.RtuName;
                AttachPhyId = tml.PhyId;
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

        #region 集中器名称
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
                        !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                             EquipmentInfoDictionary.ContainsKey
                             (_rtuId))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary
                            [_rtuId];
                    RtuName = tml.RtuName;
                }
            }
        }
        #endregion

        #region 线路名称
        private string _lduLineName;
        public string LduLineName
        {
            get { return _lduLineName; }
            set
            {
                if (_lduLineName != value)
                {
                    _lduLineName = value;
                    RaisePropertyChanged(() => LduLineName);
                }
            }
        }
        #endregion

        #region 线路ID
        private int _lduLineId;
        public int LduLineId
        {
            get { return _lduLineId; }
            set
            {
                if (_lduLineId == value) return;
                _lduLineId = value;
                RaisePropertyChanged(() => LduLineId);


                if(_lduLineId>0) 
                {
                    LduLineName = "Reserve";
                    if (
                        !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                             EquipmentInfoDictionary.ContainsKey
                             (_rtuId))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary
                            [_rtuId] as Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;
                    if (tml == null) return;
                    LduLineName = tml.LduLines[_lduLineId - 1].LduLineName;
                }
            }
        }
        #endregion

        #region IsLockTml

        private bool _isLockTml;
        public bool IsLockTml
        {
            get { return _isLockTml; }
            set
            {
                if(_isLockTml==value) return;
                _isLockTml = value;
                RaisePropertyChanged(()=>IsLockTml);
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
            return DateTime.Now.Ticks - _dtBtnQuery.Ticks>30000000;
        }
        private void ExBtnQuery()
        {
            _dtBtnQuery = DateTime.Now;
            RequestData();
        }
        #endregion

        #region Items

        private ObservableCollection<LduDataQueryoneItemViewModel> _items;

        public ObservableCollection<LduDataQueryoneItemViewModel> Items
        {
            get { return _items ?? (_items = new ObservableCollection<LduDataQueryoneItemViewModel>()); }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => Items);
            }
        }

        #endregion

        #region 主设备显示

        private Visibility _attachVisi;
        public Visibility AttachVisi
        {
            get { return _attachVisi; }
            set
            {
                if (_attachVisi == value) return;
                _attachVisi = value;
                RaisePropertyChanged(()=>AttachVisi);
            }
        }

        #endregion

        #region 集中器设备显示
        private Visibility _rtuVisi;
        public Visibility RtuVisi
        {
            get { return _rtuVisi; }
            set
            {
                if (_rtuVisi != value)
                {
                    _rtuVisi = value;
                    RaisePropertyChanged(() => RtuVisi);
                }
            }
        }
        #endregion

        #region 线路名称显示
        private Visibility _lduLineVisi;
        public Visibility LduLineVisi
        {
            get { return _lduLineVisi; }
            set
            {
                if (_lduLineVisi == value) return;
                _lduLineVisi = value;
                RaisePropertyChanged(() => LduLineVisi);
            }
        }
        #endregion

        #region IsAdvacnceChecked

        private bool _isAdvacnceChecked;
        public bool IsAdvacnceChecked
        {
            get { return _isAdvacnceChecked; }
            set
            {
                if(_isAdvacnceChecked==value) return;
                _isAdvacnceChecked = value;
                AdvacnceVisi = _isAdvacnceChecked ? Visibility.Visible : Visibility.Collapsed;
                RaisePropertyChanged(()=>IsAdvacnceChecked);
            }
        }
        #endregion

        #region AdvacnceVisi

        private Visibility _advacnceVisi;
        public Visibility AdvacnceVisi
        {
            get { return _advacnceVisi; }
            set
            {
                if (_advacnceVisi == value) return;
                _advacnceVisi = value;
                RaisePropertyChanged(()=>AdvacnceVisi);
            }
        }
        #endregion

        #region gridview Columns visible

        #region BrightRateVisi

        private bool _brightRateVisi;
        public bool BrightRateVisi
        {
            get { return _brightRateVisi; }
            set
            {
                if(_brightRateVisi==value) return;
                _brightRateVisi = value;
                RaisePropertyChanged(()=>BrightRateVisi);
            }
        }

        #endregion

        #region SingleVisi
        private bool _singleVisi;
        public bool SingleVisi
        {
            get { return _singleVisi; }
            set
            {
                if (_singleVisi == value) return;
                _singleVisi = value;
                RaisePropertyChanged(() => SingleVisi);
            }
        }
        #endregion

        #region ImpedanceVisi
        private bool _impedanceVisi;
        public bool ImpedanceVisi
        {
            get { return _impedanceVisi; }
            set
            {
                if (_impedanceVisi == value) return;
                _impedanceVisi = value;
                RaisePropertyChanged(() => ImpedanceVisi);
            }
        }
        #endregion

        #region NumOfUseFullSingleIn12SecVisi
        private bool _numOfUseFullSingleIn12SecVisi;
        public bool NumOfUseFullSingleIn12SecVisi
        {
            get { return _numOfUseFullSingleIn12SecVisi; }
            set
            {
                if (_numOfUseFullSingleIn12SecVisi == value) return;
                _numOfUseFullSingleIn12SecVisi = value;
                RaisePropertyChanged(() => NumOfUseFullSingleIn12SecVisi);
            }
        }
        #endregion

        #region NumOfSingleIn12SecVisi
        private bool _numOfSingleIn12SecVisi;
        public bool NumOfSingleIn12SecVisi
        {
            get { return _numOfSingleIn12SecVisi; }
            set
            {
                if (_numOfSingleIn12SecVisi == value) return;
                _numOfSingleIn12SecVisi = value;
                RaisePropertyChanged(() => NumOfSingleIn12SecVisi);
            }
        }
        #endregion

        #region FlagDetectionVisi
        private bool _flagDetectionVisi;
        public bool FlagDetectionVisi
        {
            get { return _flagDetectionVisi; }
            set
            {
                if (_flagDetectionVisi == value) return;
                _flagDetectionVisi = value;
                RaisePropertyChanged(() => FlagDetectionVisi);
            }
        }
        #endregion

        #region FlagAlarmVisi
        private bool _flagAlarmVisi;
        public bool FlagAlarmVisi
        {
            get { return _flagAlarmVisi; }
            set
            {
                if (_flagAlarmVisi == value) return;
                _flagAlarmVisi = value;
                RaisePropertyChanged(() => FlagAlarmVisi);
            }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class Wj1090LduDataQueryViewModel
    {
        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_request_Data,
                ResolveRequestData,
                typeof (Wj1090LduDataQueryViewModel), this);
        }

        private void ResolveRequestData(string session,
                                        Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<LduMeasureData> infos)
        {
            var info = infos.Data;
            if (info == null) return;
            Items.Clear();
            int i = 0;
            foreach (LduLineData item in info.Items)
            {
                Items.Add(new LduDataQueryoneItemViewModel(item, i));
                i++;
            }
            Remind = "数据已反馈...";
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
            Microsoft.Practices.Prism.MefExtensions.Event.EventHelper.PublishEventArgs args)
        {
            if (!_isThisViewActive) return;
            ////if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
            ////{
            ////    AttachId = Convert.ToInt32(args.GetParams()[0]);
            ////    RtuId = 0;
            ////    LduLineId = 0;
            ////    //RtuId = Convert.ToInt32(args.GetParams()[1]);
            ////    //LduLineId = Convert.ToInt32(args.GetParams()[2]);
            ////}
            ////else 
            //if (args.EventId == Wlst .Sr .EquipmentInfoHolding .Services .EventIdAssign .EquipmentSelected)
            //{

            //    AttachId = Convert.ToInt32(args.GetParams()[0]);
            //    RtuId = Convert.ToInt32(args.GetParams()[1]);
            //    LduLineId = Convert.ToInt32(args.GetParams()[2]);

            //}
            //else if (args.EventId == Wj1090Module.Services.EventIdAssign.EventTreeNodeLineSelected)
            //{

            //    AttachId = Convert.ToInt32(args.GetParams()[0]);
            //    RtuId = Convert.ToInt32(args.GetParams()[1]);
            //    LduLineId = Convert.ToInt32(args.GetParams()[2]);

            //}
            if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
            {
                int rtuid = Convert.ToInt32(args.GetParams()[0]);
                if (!Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLine(rtuid)) return;

                var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(rtuid);
                if (info == null) return;
                AttachId = info.AttachRtuId == 0 ? rtuid : info.AttachRtuId;
                RtuId = rtuid;
                if (args.GetParams().Count > 1)
                {
                    try
                    {
                        if (args.GetParams().Count > 1)
                        {
                            LduLineId = Convert.ToInt32(args.GetParams()[1]);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                    LduLineId = 0;
            }

            AttachVisi = AttachId.Equals(0) ? Visibility.Collapsed : Visibility.Visible;
            RtuVisi = RtuId.Equals(0) ? Visibility.Collapsed : Visibility.Visible;
            LduLineVisi = LduLineId.Equals(0) ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    /// <summary>
    /// socket
    /// </summary>
    public partial class Wj1090LduDataQueryViewModel
    {
        private void RequestData()
        {

            LogInfo.Log("正在请求所有类型信息!!!");
            Remind = "查询命令已发送...";
            var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_request_Data;
            if (IsLockTml)
            {
                info.Data.AttachRtuId = AttachId;
                info.Data.RtuId = RtuId;
                info.Data.LoopId = LduLineId;
            }
            else
            {
                info.Data.AttachRtuId = AttachId;
                info.Data.RtuId = 0;
                info.Data.LoopId = 0;
            }
            info.Data.DtStart = DtStart;
            info.Data.DtEnd = DtEnd;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
    }
}
