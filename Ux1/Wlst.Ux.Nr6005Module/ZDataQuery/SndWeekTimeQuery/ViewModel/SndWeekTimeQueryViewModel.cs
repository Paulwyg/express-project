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
using Wlst.Ux.Nr6005Module.ZDataQuery.SndWeekTimeQuery.Services;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.SndWeekTimeQuery.ViewModel
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
                    if (RtuId >= 0)
                    {
                       // this.Query(DtStartTime, DtEndTime, RtuId);
                        IsLock = true;
                        ExQuery();
                    }
                    else
                    {
                        PhyId = 0;
                        IsLock = false;
                        RtuName = "所有终端";
                    }
                }
                else
                {
                    PhyId = 0;
                    IsLock = false;
                    RtuName = "所有终端";
                }
            }
            catch (Exception ex)
            {

            }
            Remind = "请通过点击左侧终端树来选择终端进行终端记录查询...";
        }

        public void OnUserHideOrClosing()
        {
            _isThisViewActive = false;
            Record = new ObservableCollection<SndWeekTimeOneRecordViewModel>();
            ExportVisi=Visibility.Collapsed;
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
                    PhyId = 0;
                    RtuName = "所有终端";
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
                        RtuName = "所有终端设备.";
                        return;
                    }
                    RtuName = "" + _rtuId;
                    if (
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                            InfoItems .ContainsKey(_rtuId))
                    {
                        var info =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .
                                InfoItems [_rtuId];
                        this.RtuName = info.RtuName;
                        PhyId = info.RtuPhyId ;

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
            Query(DtStartTime, DtEndTime, RtuId);
        
            _isOnExport = false;
            ExportVisi = Visibility.Visible;
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
            ProtocolServer.RegistProtocol(
                Wlst.Sr .ProtocolPhone .LxRtu .wst_weekset_snd_record ,// .wlst_svr_ans_cnt_request_weekset_record  ,
                RecordWeekTimeRequest,
                typeof(SndWeekTimeQueryViewModel), this);
        }

        public void RecordWeekTimeRequest(string session, Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstRtuWeeksetSndRecord;
            if (info == null) return;
            this.RtuId = info.RtuId ;

            var tmpitems = new ObservableCollection<SndWeekTimeOneRecordViewModel>();
            int index = 0;

            var phyid = info.RtuId;
            if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(info.RtuId))
                phyid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[info.RtuId].RtuPhyId;
            if(info.Info.Count==0)
            {
                //Remind = "没有所需要查询记录！";
                Record.Clear();
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + phyid  + "-- 终端记录查询成功，共计" + info.Info.Count + " 条数据.";
     
                return;
            }
            foreach (var t in info.Info)
            {
                index++;
                tmpitems.Add(new SndWeekTimeOneRecordViewModel(t, index));
            }
            this.Record = tmpitems;
            //Remind = "数据已反馈！";

            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" +phyid  + "-- 终端记录查询成功，共计" + info.Info.Count + " 条数据.";
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
                        if(IsLock)
                        {
                            SelectRtuIdChange(id); 
                        }
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
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询：" +phyid  +
                     " ...";
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
    }
}
