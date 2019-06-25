using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.Services;
using Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.View;
using Wlst.Ux.TimeTableSystem.OpenCloseReportTabVm.Services;
using Wlst.Ux.TimeTableSystem.OpenCloseReportTabVm.ViewModel;


namespace Wlst.Ux.TimeTableSystem.OpenCloseReportQuery.ViewModel
{
    [Export(typeof (IIOpenCloseReportQuery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class OpenCloseReportQueryViewModel : VmEventActionProperyChangedBase,
                                                         IIOpenCloseReportQuery
    {
        public OpenCloseReportQueryViewModel()
        {
            Title = "时间表报表查询";
            InitAction();
        }

        private void InitAction()
        {
            this.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtuTime.wst_request_timetable_x_report_record,
                //.wlst_svr_ans_cnt_wj3090_request_open_close_light_report_reocrd ,//.ClientPart.wlst_OpenCloseLight_server_ans_clinet_request_openCloseLightReportRecord,
                MyAction,true);
        }

        public static ObservableCollection<OpenCloseReportItem> ItemsOut =
            new ObservableCollection<OpenCloseReportItem>();
        public static OpenCloseReportQueryForWin OpenCloseReportQueryWindow = null;
        private void MyAction(string session, Wlst.mobile.MsgWithMobile infos)
        {
            // Records.Clear();
            if (infos == null) return;
            if (infos.WstRtutimeRequestTimetableXReportRecord == null) return;
            if (infos.WstRtutimeRequestTimetableXReportRecord.Op == 1) return;

            var tmpitems = new ObservableCollection<OpenCloseReportItem>();
            var tmpitemsout = new ObservableCollection<OpenCloseReportItem>();

            var tmps = (from t in infos.WstRtutimeRequestTimetableXReportRecord.Items orderby t.DateCreate select t).ToList();
            foreach (var t in tmps)
            {
                if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Contains(t.AreaId))
                {
                    if (t.TheXTimes > 100) 
                        tmpitemsout.Add(new OpenCloseReportItem(t.AreaId, t));
                    else 
                        tmpitems.Add(new OpenCloseReportItem(t.AreaId, t));
                }
            }

            if (tmpitems.Count != 0)
            {
                this.Records = tmpitems;
                this.CurrentSelectItem = this.Records.First();
                ShowOrderInfo = "数据已反馈请查看数据，点击左边列表时将在右边列表中显示详细信息！！！";
            }
            else if (Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowTimeTableOnTime)
            {
                if (OpenCloseReportQueryWindow == null)
                {
                    OpenCloseReportQueryWindow = new OpenCloseReportQueryForWin();
                }

                var failedopen = new ObservableCollection<OpenCloseReportRtuItem>();
                var successopen = new ObservableCollection<OpenCloseReportRtuItem>();
                var flg = new bool();

                foreach (var t in tmpitemsout)
                {
                    failedopen.Clear();
                    successopen.Clear();
                    foreach (var tt in t.Records)
                    {
                        flg = true;
                        foreach (var ttt in tt.Records)
                        { 
                            if (ttt.Value == "失败" && flg)
                            {
                                failedopen.Add(tt);
                                flg = false;
                            }
                        }

                        if (flg)
                        {
                            successopen.Add(tt);
                        }
                    }

                    t.Records.Clear();
                    foreach (var tt in failedopen)
                    {
                        t.Records.Add(tt);
                    }
                    foreach (var tt in successopen)
                    {
                        t.Records.Add(tt);
                    }
                }



                if (OpenCloseReportQueryWindow.Visibility == Visibility.Visible)
                {
                    foreach (var t in tmpitemsout)
                    {
                        ItemsOut.Add(t);
                    }
                }
                else
                {
                    ItemsOut = tmpitemsout;
                }

                OpenCloseReportQueryWindow.SetContext(ItemsOut);

                OpenCloseReportQueryWindow.Visibility = Visibility.Visible;
                
                OpenCloseReportQueryWindow.Show();
                OpenCloseReportQueryWindow.Focus();
            }

        }


        private ObservableCollection<OpenCloseReportItem> _records;

        public ObservableCollection<OpenCloseReportItem> Records
        {
            get
            {
                if (_records == null) _records = new ObservableCollection<OpenCloseReportItem>();
                return _records;
            }
            set
            {
                if (_records == value) return;
                _records = value;
                this.RaisePropertyChanged(() => this.Records);
            }
        }

        private OpenCloseReportItem _rCurrentSelectItem;

        public OpenCloseReportItem CurrentSelectItem
        {
            get { return _rCurrentSelectItem; }
            set
            {
                if (value != _rCurrentSelectItem)
                {
                    _rCurrentSelectItem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectItem);
                    //RtuOpenCloseItems = CurrentSelectItem.Records;
                    if (_rCurrentSelectItem == null) return;

                    RtuOpenCloseItems.Clear();
                    foreach (var t in _rCurrentSelectItem.Records) RtuOpenCloseItems.Add(t);
                }
                if (value == null) btnExEnable = false;
                else if (value.Records.Count == 0) btnExEnable = false;
                else btnExEnable = true;
            }
        }

        private bool _btnExEnable;

        public bool btnExEnable
        {
            get { return _btnExEnable; }
            set
            {
                if (_btnExEnable == value) return;
                _btnExEnable = value;
                this.RaisePropertyChanged(() => this.btnExEnable);
            }
        }


        private ObservableCollection<OpenCloseReportRtuItem> _recordsss;


        public ObservableCollection<OpenCloseReportRtuItem> RtuOpenCloseItems
        {
            get
            {
                if (_recordsss == null)
                {
                    _recordsss = new ObservableCollection<OpenCloseReportRtuItem>();
                }
                return _recordsss;
            }
            set
            {
                if (value != _recordsss)
                {
                    _recordsss = value;
                    this.RaisePropertyChanged(() => this.RtuOpenCloseItems);
                }
            }
        }




        public override void NavOnLoadr(params object[] parsObjects)
        {
            this.Records.Clear();
            TimeTableItems.Clear();
            _dtLastQuery = DateTime.Now.AddHours(-1);
            DtStartTime = DateTime.Now.AddDays(-1);
            DtEndTime = DateTime.Now.AddHours(1);


            InitTimeTable();
            if (OpenOrCloseItems.Count > 0) CurrentSelectOpenCloseItem = OpenOrCloseItems[0];
            if (TimeTableItems.Count > 0) CurrentSelectTimeTableItem = TimeTableItems[0];
        }

        public override void OnUserHideOrClosingr()
        {
            Records = new ObservableCollection<OpenCloseReportItem>();
            TimeTableItems = new ObservableCollection<NameIntBool>();
            RtuOpenCloseItems = new ObservableCollection<OpenCloseReportRtuItem>();
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

        #region

        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class OpenCloseReportQueryViewModel
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


        private ObservableCollection<NameIntBool> _openOrclose;

        public ObservableCollection<NameIntBool> OpenOrCloseItems
        {
            get
            {
                if (_openOrclose == null)
                {
                    _openOrclose = new ObservableCollection<NameIntBool>();
                    _openOrclose.Add(new NameIntBool() {IsSelected = false, Name = "全部", Value = 0});
                    _openOrclose.Add(new NameIntBool() {IsSelected = false, Name = "开灯", Value = 1});
                    _openOrclose.Add(new NameIntBool() {IsSelected = false, Name = "关灯", Value = 2});
                }
                return _openOrclose;
            }
        }

        private NameIntBool _currentselectopencloseitem;

        public NameIntBool CurrentSelectOpenCloseItem
        {
            get { return _currentselectopencloseitem; }
            set
            {
                if (value != _currentselectopencloseitem)
                {
                    _currentselectopencloseitem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectOpenCloseItem);
                }
            }
        }


        //private void InitTimeTable()
        //{
        //    TimeTableItems.Add(new NameIntBool() {IsSelected = false, Name = "全部", Value = 0});
        //    foreach (var tt in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
        //    {
        //        foreach (var t in WeekTimeTableInfoService.GeteekTimeTableInfoList(tt))
        //        {
        //            TimeTableItems.Add(new NameIntBool() {IsSelected = false, Name = t.TimeName, Value = t.TimeId,AreaId = tt});
        //        }
        //    }


        //}

        private void InitTimeTable()
        {
            TimeTableItems.Clear();
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 1)
                {
                    TimeTableItems.Add(new NameIntBool()
                    {
                        IsSelected = false,
                        Name = "全部",
                        Value = 0,
                        AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.First().Value.AreaId
                    });
                }
                else
                {
                    foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                    {
                        string area = t.Value.AreaName;
                        TimeTableItems.Add(new NameIntBool() { IsSelected = false, Name = area + "全部", Value = 0, AreaId = t.Value.AreaId });
                    }
                }
            }
            else
            {
                if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count == 1)
                {
                    TimeTableItems.Add(new NameIntBool()
                    {
                        IsSelected = false,
                        Name = "全部",
                        Value = 0,
                        AreaId = Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.First()
                    });
                }
                else
                {
                    foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                    {
                        if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                        {
                            string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                            TimeTableItems.Add(new NameIntBool() { IsSelected = false, Name = area + "全部", Value = 0, AreaId = t });
                        }
                    }
                }
            }


            var tmp =
                (from t in WeekTimeTableInfoService.WeekTimeTableInfoDictionary
                 orderby t.Key.Item1, t.Key.Item2
                 select t);
            foreach (var t in tmp)
            {
                if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Contains(t.Key.Item1) ||
                    UserInfo.UserLoginInfo.D)
                {
                    TimeTableItems.Add(new NameIntBool()
                    {
                        IsSelected = false,
                        Name = t.Value.TimeName,
                        Value = t.Value.TimeId,
                        AreaId = t.Key.Item1
                    });
                }

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
            set
            {
                if (value == _timetableitems) return;
                _timetableitems = value;
                this.RaisePropertyChanged(() => TimeTableItems);
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

            var isopen = 0;
            if (CurrentSelectOpenCloseItem != null) isopen = CurrentSelectOpenCloseItem.Value;
            var timetableid = 0;
            if (CurrentSelectTimeTableItem != null) timetableid = CurrentSelectTimeTableItem.Value;

            var tStartTime = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, 23, 59, 59);

            if (!GetCheckedInformation()) return;
            this.Records.Clear();

            var info = Wlst.Sr.ProtocolPhone.LxRtuTime.wst_request_timetable_x_report_record;
                // .wlst_cnt_wj3090_request_open_close_light_report_reocrd ;//.ServerPart.wlst_OpenCloseLight_clinet_request_openCloseLightReportRecord;
            info.WstRtutimeRequestTimetableXReportRecord.Op = 2;
            info.WstRtutimeRequestTimetableXReportRecord.DtEndTime = tEndTime.Ticks;
            info.WstRtutimeRequestTimetableXReportRecord.DtStartTime = tStartTime.Ticks;
            info.WstRtutimeRequestTimetableXReportRecord.IsOpenLight = isopen;
            info.WstRtutimeRequestTimetableXReportRecord.TimeTableId = timetableid;

            var areaid = 0;
            if (CurrentSelectTimeTableItem != null) areaid = CurrentSelectTimeTableItem.AreaId;
            info.WstRtutimeRequestTimetableXReportRecord.AreaId = areaid;

            SndOrderServer.OrderSnd(info, 10, 6);
            ShowOrderInfo = "查询命令已发送...请等待数据反馈！！！";

        }



        #endregion
    }

    public class NameIntBool : ObservableObject
    {
        public int AreaId;

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.RaisePropertyChanged(() => this.Value);
                }
            }
        }

        /// <summary>
        /// 当IsSelected状态发生变化的时候出发  如果本函数被注册了
        /// </summary>
        public event EventHandler OnIsSelectedChanged;

        private bool _check;

        public bool IsSelected
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    if (OnIsSelectedChanged != null) OnIsSelectedChanged(this, EventArgs.Empty);
                }
            }
        }



        //~NameIntBool()
        //{

        //}
    }
}
