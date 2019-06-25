using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Sr.ProtocolCnt.TimeTable;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Ux.TimeTableSystem.ExecuteRecordQuery.Services;

namespace Wlst.Ux.TimeTableSystem.ExecuteRecordQuery.ViewModel
{
    [Export(typeof (IIExecuteRecordView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ExecuteRecordViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIExecuteRecordView
    {
        #region tab

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
            get { return "开关灯报表"; }
        }

        #endregion

        public ExecuteRecordViewModel ()
        {
            InitAction();
            DtStartTime = DateTime.Now.AddDays(-7);
            DtEndTime = DateTime.Now;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            InitTimeTable();
            ShowOrderInfo = "";

        }

        public void OnUserHideOrClosing()
        {
            this.Records.Clear();
        }

        private ObservableCollection<ExecuteItem> _records;

        public ObservableCollection<ExecuteItem> Records
        {
            get
            {
                if (_records == null) _records = new ObservableCollection<ExecuteItem>();
                return _records;
            }
        }
    }

    public partial class ExecuteRecordViewModel
    {
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolCnt.ClientPart.wlst_TimeTable_server_ans_clinet_request_timetable_execute_record,
                MyAction, typeof (ExecuteRecordViewModel), this);
        }



        private void MyAction(string session, ProtocolEncodingCnt<TimeTableExecuteInfo> infos)
        {
            Records.Clear();

            var tmps = (from t in infos.Data.Items orderby t.TimeTableId , t.DateCreate select t).ToList();
            TimeTableExecuteInfoItem lst = null;
            int index = 0;

            double sum = 0;
            double onesum = 0;
            foreach (var t in tmps)
            {
                index++;

                if (lst == null) lst = t;
                else
                {
                    if (t.TimeTableId == lst.TimeTableId)
                    {
                        if (t.OpenOrClose == 1)
                        {
                            onesum = 0;
                        }
                        else
                        {

                            if (t.OpenOrClose == 2 && lst.OpenOrClose == 1)
                            {
                                onesum = (t.DateCreate - lst.DateCreate)/(60.0*60*10000000);
                                sum += onesum;
                            }
                        }
                    }
                    else
                    {
                        onesum = 0;
                        sum = 0;
                    }
                    lst = t;
                }

                Records.Add(new ExecuteItem(t) { Id = index, OneSum = onesum.ToString("f2"), AllSum = sum.ToString("f2") });
            }
            ShowOrderInfo = "数据已反馈请查看数据！！！";
        }
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class ExecuteRecordViewModel
    {
        #region attri

        private DateTime _dtStartTime;

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


        private void InitTimeTable()
        {
            TimeTableItems.Add(new NameIntBool() {IsSelected = false, Name = "全部", Value = 0});
            foreach (var t in WeekTimeTableInfoService.GeteekTimeTableInfoList)
            {
                TimeTableItems.Add(new NameIntBool() {IsSelected = false, Name = t.time_name, Value = t.time_id});
            }
            if (TimeTableItems.Count > 0) CurrentSelectTimeTableItem = TimeTableItems[0];
        }

        private ObservableCollection<NameIntBool> _timetableitems;

        public ObservableCollection<NameIntBool> TimeTableItems
        {
            get
            {
                if (_timetableitems == null)
                {
                    _timetableitems = new ObservableCollection<NameIntBool>();
                }
                return _timetableitems;
            }
        }

        private NameIntBool _currentselectopentimetableitem;

        public NameIntBool CurrentSelectTimeTableItem
        {
            get { return _currentselectopentimetableitem; }
            set
            {
                if (value != _currentselectopentimetableitem)
                {
                    _currentselectopentimetableitem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectTimeTableItem);
                }
            }
        }


        private string _showOrderInfo;

        public string ShowOrderInfo
        {
            get { return _showOrderInfo; }
            set
            {
                if (_showOrderInfo == value) return;
                _showOrderInfo = value;
                RaisePropertyChanged(() => ShowOrderInfo);
            }
        }

        #endregion

        #region ICommand CmdQuery



        private ICommand _cmdAddTimeTable;

        public ICommand CmdQuery
        {
            get
            {
                return _cmdAddTimeTable ??
                       (_cmdAddTimeTable = new RelayCommand(ExCmdAddTimeTable, CanCmdAddTimeTable, true));
            }

        }

        private DateTime _dtLastQuery;

        private bool CanCmdAddTimeTable()
        {
            if (DtStartTime.Ticks > DtEndTime.Ticks) return false;


            return DateTime.Now.Ticks - _dtLastQuery.Ticks > 30000000;
        }

        private void ExCmdAddTimeTable()
        {

            var timetableid = 0;
            if (CurrentSelectTimeTableItem != null) timetableid = CurrentSelectTimeTableItem.Value;

            var info = Wlst.Sr.ProtocolCnt.ServerPart.wlst_TimeTable_clinet_request_timetable_execute_record;
            info.Data.DtEndTime = DtEndTime.Ticks;
            info.Data.DtStartTime = DtStartTime.Ticks;
            info.Data.TimeTableId = timetableid;
            info.Data.TimeTableId = timetableid;
            SndOrderServer.OrderSnd(info, 10, 6);
            ShowOrderInfo = "查询命令已发送...请等待数据反馈！！！";

        }



        #endregion
    }
}
