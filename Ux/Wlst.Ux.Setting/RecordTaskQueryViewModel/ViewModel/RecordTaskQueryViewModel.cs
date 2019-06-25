using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.Setting.RecordTaskQueryViewModel.Services;


namespace Wlst.Ux.Setting.RecordTaskQueryViewModel.ViewModel
{
    [Export(typeof (IIRecordTaskQueryViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RecordTaskQueryViewModel :
        Wlst.Cr.Core.CoreServices .ObservableObject , IIRecordTaskQueryViewModel
    {
        public RecordTaskQueryViewModel()
        {
            this.InitAction();
        }

        private ObservableCollection<OneRecordTaskViewModel> _record;

        public ObservableCollection<OneRecordTaskViewModel> Record
        {
            get
            {
                if (_record == null)
                    _record = new ObservableCollection<OneRecordTaskViewModel>();
                return _record;
            }
            set
            {
                if (value == _record) return;
                _record = value;
                this.RaisePropertyChanged(() => Record);
            }
        }


        private ObservableCollection<Wlst .Cr .CoreOne .Models .NameValueInt> _class;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> Class
        {
            get
            {
                if (_class == null)
                {
                    _class = new ObservableCollection<NameValueInt>();
                    _class.Add(new NameValueInt() {Name = "全部", Value = 0,});
                    _class.Add(new NameValueInt() {Name = "时间表开关灯任务", Value = 11,});
                    _class.Add(new NameValueInt() {Name = "周设置自动调整任务", Value = 12,});
                    _class.Add(new NameValueInt() {Name = "巡测任务", Value = 21,});
                    _class.Add(new NameValueInt() { Name = "单灯开关灯任务", Value = 31, });
                }
                return _class;
            }
            set
            {
                if (value == _class) return;
                _class = value;
                this.RaisePropertyChanged(() => Record);
            }
        }

        private NameValueInt _currentslelec;

        public NameValueInt CurrentSelectItem
        {
            get
            {
                if (_currentslelec == null && Class != null && Class.Count > 0) _currentslelec = Class[0];
                return _currentslelec;
            }
            set
            {
                if (value == _currentslelec) return;
                _currentslelec = value;
                this.RaisePropertyChanged(() => this.CurrentSelectItem);
            }
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            Record.Clear();
            DtStartTime = DateTime.Now.AddDays(-1);
            DtEndTime = DateTime.Now;
            Remind = "请设置好查询起始结束时间进行查询，查询时间不得超过一个月...";

            if (Class != null && Class.Count > 0) CurrentSelectItem = Class[0];

        }

        public void OnUserHideOrClosing()
        {
            //throw new NotImplementedException();
            Record = new ObservableCollection<OneRecordTaskViewModel>();
        }

        #region Field

        private bool _isOnExport = true;
        #endregion

        #region iitab
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
            get { return "任务记录查询"; }
        }

        #endregion

        #region arrti

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
                    _dtStartTime = value;
                    this.RaisePropertyChanged(() => this.DtStartTime);
                }
            }
        }

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
                    _dtEndTime = value;
                    this.RaisePropertyChanged(() => this.DtEndTime);
                }
            }
        }

        #region Remind

        private string _remind;
        public string  Remind
        {
            get { return _remind; }
            set
            {
                if (value == _remind) return;
                _remind = value;
                RaisePropertyChanged(()=>Remind);
            }
        }
        #endregion

        #region ExportVisi

        private Visibility _exportVisi=Visibility.Collapsed;
        public Visibility ExportVisi
        {
            get { return _exportVisi; }
            set
            {
                if(value==_exportVisi) return;
                _exportVisi = value;
                RaisePropertyChanged(()=>ExportVisi);
            }
        }
        #endregion
        #endregion

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
            int currentselect = 0;
            if (CurrentSelectItem != null) currentselect = CurrentSelectItem.Value;
            Query(DtStartTime, DtEndTime,currentselect );
           // Remind = "查询命令已发送...请等待数据反馈！";
            _isOnExport = false;
            ExportVisi=Visibility.Visible;
        }

        private bool CanExQuery()
        {
            if (DtStartTime.Ticks <= DtEndTime.Ticks)
                return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
            if (DtStartTime.Year == DtEndTime.Year && DtStartTime.Month == DtEndTime.Month &&
                DtEndTime.Day == DtStartTime.Day)
                return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
            return false;
        }

        #endregion


    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class RecordTaskQueryViewModel
    {
  


        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSys .wst_task_record ,// .wlst_svr_ans_cnt_request_record_task  ,
                RecordTaskRequest,
                typeof(RecordTaskQueryViewModel), this, true);
        }

        public void RecordTaskRequest(string session, Wlst .mobile .MsgWithMobile  infos)
        {
            var info = infos.WstSysTaskRecord;
            if (info == null) return;
            this.Record.Clear();
            int index = 0;
            foreach (var t in info.Items )
            {
                
                var lst = OneRecordTaskViewModel.GetOneRecordVm(t);
                if (lst != null)
                {
                    index++;
                    lst.RecordIndex = index;
                    this.Record.Add(lst);
                }
            }
           // Remind = "数据已反馈，查询命令结束，请查看数据！";
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  任务记录查询成功，共计" + info.Items .Count + " 条数据.";
        }

  
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class RecordTaskQueryViewModel
    {
        private void Query(DateTime dtstarttime, DateTime dtendtime,int clas)
        {
            //int waitId = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new Model.ExchangeRequestRecordTask()
            //               {
            //                   RequestEndTime = dtendtime,
            //                   RequestStartTime = dtstarttime,
            //               };
            //SndOrderServer.OrderSnd(Wlst.Ux.EquipmentDataQuery.Services.EventIdAssign.RecordTaskRequest , null, info,
            //                        waitId);


            var tStartTime = new DateTime(dtstarttime.Year, dtstarttime.Month, dtstarttime.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59);

            if (!GetCheckedInformation()) return;
            this.Record.Clear();
            var nt = Wlst.Sr.ProtocolPhone .LxSys .wst_task_record  ;
            nt.WstSysTaskRecord .DtEndTime = tEndTime.Ticks ;
            nt.WstSysTaskRecord.DtStartTime = tStartTime.Ticks;
            nt.WstSysTaskRecord.TaskId = clas;
            SndOrderServer.OrderSnd(nt);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";

        }

        private bool GetCheckedInformation()
        {
            if (DtStartTime.AddDays(63) < DtEndTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在62天以内", UMessageBoxButton.Ok);
                
                return false;
            }
            return true;
        }
    }
}
