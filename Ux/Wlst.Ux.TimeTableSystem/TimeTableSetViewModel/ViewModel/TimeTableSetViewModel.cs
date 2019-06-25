using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.ProtocolCnt.TimeTable;
using Wlst.Sr.TimeTableSystem.Models;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.Ux.TimeTableSystem.TimeTableSetViewModel.Services;

namespace Wlst.Ux.TimeTableSystem.TimeTableSetViewModel.ViewModel
{
    /// <summary>
    /// 主设置界面
    /// </summary>
    [Export(typeof (IITimeTableSetViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class TimeTableSetViewModel : ObservableObject, IITimeTableSetViewModel
    {
        public TimeTableSetViewModel()
        {
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);
        }

        #region NavOnLoad

        /// <summary>
        /// 页面加载或是导航显示的时候 需要执行的初始化操作
        /// </summary>
        /// <param name="parsObjects"></param>
        public void NavOnLoad(params object[] parsObjects)
        {
            GetTimeTableViewModelData();
        }

        private void GetTimeTableViewModelData()
        {
            this.TimeTables.Clear();
            foreach (var t in WeekTimeTableInfoService.GeteekTimeTableInfoList)
            {
                bool bolfind = false;
                foreach (var tt in this.TimeTables)
                {
                    if (tt.TimeTableId == t.time_id)
                    {
                        tt.ReloadLux();

                        bolfind = true;
                        break;
                    }
                }
                if (!bolfind)
                {
                    this.TimeTables.Add(new TimeTableViewModel(t));
                }
            }
            if (this.TimeTables.Count > 0) CurrentSelectItem = TimeTables[0];
        }

        #endregion

        #region logic with tml CurrentSelectTmlId为触发点，然后更新选中终端名称，回路信息 选中回路 以及时间表



        /// <summary>
        /// 根据数据区域中的数据查找时间表
        /// </summary>
        /// <param name="timeTableId"></param>
        /// <param name="timeTableName"></param>
        /// <param name="onTime"></param>
        /// <param name="offTime"></param>
        /// <param name="onLight"> </param>
        /// <param name="offLight"> </param>
        private void GetTimeTableTodayOnOffTimeByDataHolding(int timeTableId, out string timeTableName, out string onTime, out string offTime,
            out bool onLight, out bool offLight)
        {
            timeTableName = "Reserve"; //Reserve 建议别修改，如果在界面显示UnKnown字样则表示数据为保存 建议用户保存
            onTime = "No";
            offTime = "No";
            onLight = false;
            offLight = false;
            var timeTable = WeekTimeTableInfoService.GeteekTimeTableInfo(timeTableId);
            if (timeTable != null)
            {
                timeTableName = timeTable.time_name;
                if (timeTable.LstOneWeekOpenCloseControl != null)
                    foreach (var ttt in timeTable.LstOneWeekOpenCloseControl)
                    {
                        if (ttt.date_day == DateTime.Now.Day && ttt.date_month == DateTime.Now.Month)
                        {
                            int hourOnOff = ttt.time_off / 60;
                            int minuteOnOff = ttt.time_off % 60;
                            string textOnOff = hourOnOff + ":" + minuteOnOff;
                            onTime = textOnOff;

                            hourOnOff = ttt.time_on / 60;
                            minuteOnOff = ttt.time_on % 60;
                            textOnOff = hourOnOff + ":" + minuteOnOff;
                            offTime = textOnOff;
                            onLight = ttt.is_lux_on;
                            offLight = ttt.is_lux_off;
                        }
                    }
            }
        }

        /// <summary>
        /// 根据显示区域的及时数据来查找时间表  即使用户立即修改了时间表数据也能正常显示
        /// </summary>
        /// <param name="timeTableId"></param>
        /// <param name="timeTableName"></param>
        /// <param name="onTime"></param>
        /// <param name="offTime"></param>
        private void GetTimeTableTodayOnOffTimeByMvvm(int timeTableId, out string timeTableName, out string onTime, out string offTime)
        {
            timeTableName = "Reserve";
            onTime = "No";
            offTime = "No";
            foreach (var t in TimeTables)
            {
                if (t.TimeTableId == timeTableId)
                {
                    timeTableName = t.TimeTableName;
                    if (t.OneWeekTimeTable != null)
                        foreach (var ttt in t.OneWeekTimeTable)
                        {
                            if (ttt.DateDay == DateTime.Now.Day && ttt.DateMonth == DateTime.Now.Month)
                            {
                                int hourOnOff = ttt.TimeOn / 60;
                                int minuteOnOff = ttt.TimeOn % 60;
                                string textOnOff = hourOnOff + ":" + minuteOnOff;
                                onTime = textOnOff;

                                hourOnOff = ttt.TimeOff / 60;
                                minuteOnOff = ttt.TimeOff % 60;
                                textOnOff = hourOnOff + ":" + minuteOnOff;
                                offTime = textOnOff;
                            }
                        }
                    break;
                }
            }
        }



        #endregion

        #region iinterface

        #region tabTitle

        public string Title
        {
            get
            {
                return "时间表设置";
            }
        }

        public bool CanClose
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region data

        private ObservableCollection<TimeTableViewModel> _timeTableViewModels;

        /// <summary>
        /// 所有时间表集合
        /// </summary>
        public ObservableCollection<TimeTableViewModel> TimeTables
        {
            get
            {
                if (_timeTableViewModels == null)
                    _timeTableViewModels = new ObservableCollection<TimeTableViewModel>();
                return _timeTableViewModels;
            }
        }

        private TimeTableViewModel _currenSelectItem;

        /// <summary>
        /// 当前选中的时间表
        /// </summary>
        public TimeTableViewModel CurrentSelectItem
        {
            get
            {
                if (_currenSelectItem == null)
                    _currenSelectItem = new TimeTableViewModel();
                return _currenSelectItem;
            }
            set
            {
                if (_currenSelectItem != value)
                {
                    _currenSelectItem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectItem);
                    TimeTableOneWeeksViewModels.Clear();
                    if (_currenSelectItem == null)
                        return;
                    if (_currenSelectItem.OneWeekTimeTable == null)
                        return;
                    foreach (var t in _currenSelectItem.OneWeekTimeTable)
                    {
                        TimeTableOneWeeksViewModels.Add(t);
                    }
                }
            }
        }

        private ObservableCollection<TimeTableOneDayViewModel> _timeTableOneDaysViewModels;

        /// <summary>
        /// 当前选中的时间表的一周七天的时间详细
        /// </summary>
        public ObservableCollection<TimeTableOneDayViewModel> TimeTableOneWeeksViewModels
        {
            get
            {
                if (_timeTableOneDaysViewModels == null)
                    _timeTableOneDaysViewModels = new ObservableCollection<TimeTableOneDayViewModel>();
                return _timeTableOneDaysViewModels;
            }
        }

        #endregion

        #region  add new

        private ICommand _addNewCmd;

        public System.Windows.Input.ICommand AddNewCmd
        {
            get
            {
                if (_addNewCmd == null)
                    _addNewCmd = new RelayCommand(ExAddNew,CanExAddNew ,true );
                return _addNewCmd;
            }
        }

        bool CanExAddNew()
        {
            return true;
        }

        private void ExAddNew()
        {
            WeekTimeTableInforemation weekTimeTable = new WeekTimeTableInforemation();
            int id = 1001;
            foreach (var t in this.TimeTables)
            {
                if (t.TimeTableId >= id)
                {
                    id = t.TimeTableId + 1;
                }
            }
            weekTimeTable.time_id = id;
            weekTimeTable.time_name = "NewCreate";
            var dateOffirstSunDay = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))));

            for (int i = 0; i < 7; i++)
            {
                var tt = new WeekTimeTableItemInfomation();
                var dayOftoday = dateOffirstSunDay.AddDays(i);
                tt.date_day = dayOftoday.Day;
                tt.date_month = dayOftoday.Month;
                weekTimeTable.LstOneWeekOpenCloseControl.Add(tt);
            }
            TimeTableViewModel tv = new TimeTableViewModel(weekTimeTable);
            this.TimeTables.Insert(0, tv);
        }

        #endregion

        #region DeleteTimeTable

        private ContextMenu _cm;

        /// <summary>
        /// 菜单
        /// </summary>
        public ContextMenu Cm
        {
            get
            {
                if (_cm == null)
                    _cm = new ContextMenu();
                _cm.Items.Clear();
                MenuItem mi = new MenuItem();
                mi.Header = "Delete";
                mi.ToolTip = "Delete this Time Table";
                mi.Command = new RelayCommand(ExCmdDeleteTimeTable, CanExCmdDeleteTimeTable,true);
                ;
                _cm.Items.Add(mi);
                return _cm;
            }
        }
        bool CanExCmdDeleteTimeTable()
        {
            return true;
        }
        //private ICommand _cmdDeleteTimeTable;

        //public ICommand CmdDeleteTimeTable
        //{
        //    get
        //    {
        //        if (_cmdDeleteTimeTable == null) _cmdDeleteTimeTable = new RelayCommand(ExCmdDeleteTimeTable);
        //        return _cmdDeleteTimeTable;
        //    }
        //}

        private void ExCmdDeleteTimeTable()
        {
            if (this.TimeTables.Contains(CurrentSelectItem))
            {
                TimeTables.Remove(CurrentSelectItem);
            }
        }

        #endregion

        #region SaveTimeTable

        private ICommand _saveTimeTable;

        public ICommand SaveTimeTable
        {
            get
            {
                if (_saveTimeTable == null)
                    _saveTimeTable = new RelayCommand(ExSaveTimeTable, CanSaveTimeTable,true);
                return _saveTimeTable;
            }
        }
        bool CanSaveTimeTable()
        {
            return true;
        }

        private void ExSaveTimeTable()
        {
            var lst = new List<WeekTimeTableInforemation>();
            foreach (var t in this.TimeTables)
            {
                if (t.TimeTableId > 0)
                {
                    lst.Add(t.BackToWeekTimeTableInforemation());
                }
            }
            if (lst.Count > 0)
            {
                WeekTimeTableInfoService.UpdateTimeTable(lst);
            }
        }

        #endregion


        #endregion

        #region IEventAggregator Subscription

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private void FundEventHandler(PublishEventArgs args)
        {
            try
            {
                switch (args.EventId)
                {
                    case EventIdAssign.TimeTimeRequest :
                    case EventIdAssign.TimeTimeDelete :
                    case EventIdAssign.TimeTimeAdd :
                    case EventIdAssign.TimeTimeUpdate: //update
                        OnTimeTableUpdateEvent();
                        return;
                       
                    case EventIdAssign.TmlBelongTimeTableUpdate: //终端归属时间表发生变化
                    case EventIdAssign.TmlBelongTimeTableRequest:
                    case EventIdAssign.TmlBelongTimeTableDelete:
                    case EventIdAssign.TmlBelongTimeTableAdd:
                        return;
                       
                    case EventIdAssign.SunSetRiseRequest: //日出日落发送变化
                        foreach (var t in TimeTables)
                        {
                            if (t.OneWeekTimeTable != null)
                            {
                                foreach (var tt in t.OneWeekTimeTable)
                                {
                                    tt.CalculateTimeOff();
                                    tt.CalculateTimeOn();
                                }
                            }
                        }
                        break;

                //else if (args.EventId == 52103) //lux
                //{
                //    foreach (var t in TimeTables)
                //    {
                //        t.ReloadLux();
                //    }
                //}
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }

        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilter(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.Core)
            {
                switch (args.EventId)
                {
                    case EventIdAssign.TimeTimeRequest: //update
                    case EventIdAssign.TimeTimeAdd:
                    case EventIdAssign.TimeTimeDelete:
                    case EventIdAssign.TimeTimeUpdate:
                        return true;
                    case EventIdAssign.SunSetRiseRequest:
                        return true;
                    case EventIdAssign.TmlBelongTimeTableUpdate:
                        return true;
                    case EventIdAssign.TmlBelongTimeTableRequest:
                        return true;
                    case EventIdAssign.TmlBelongTimeTableDelete:
                        return true;
                    case EventIdAssign.TmlBelongTimeTableAdd:
                        return true;
                }
            }

            //if (args.EventType == PublishEventType.Core && args.EventId == 52103) //lux update
            //{
            //    return true;
            //}
            return false;
        }

        private void OnTimeTableUpdateEvent() //52503事件
        {
            for (int i = 0; i < TimeTables.Count - 1; i++)
            {
                if (
                   Sr .TimeTableSystem .Services .WeekTimeTableInfoService  .WeekTimeTableInfoDictionary.ContainsKey(
                        TimeTables[i].TimeTableId))
                {
                    if (
                        !TimeTables[i].Compar(
                            WeekTimeTableInfoService.WeekTimeTableInfoDictionary[TimeTables[i].TimeTableId]))
                    {
                        TimeTables[i].Update(
                            WeekTimeTableInfoService.WeekTimeTableInfoDictionary[TimeTables[i].TimeTableId]);
                    }
                }
                else
                {
                    TimeTables.RemoveAt(i);
                }
            }
            foreach (var t in WeekTimeTableInfoService.WeekTimeTableInfoDictionary)
            {
                bool bolfind = false;
                foreach (var tt in this.TimeTables)
                {
                    if (t.Value.time_id == tt.TimeTableId)
                    {
                        bolfind = true;
                        break;
                    }
                }
                if (!bolfind)
                {
                    this.TimeTables.Add(new TimeTableViewModel(t.Value));
                }
            }
           
        }


        #endregion


    
    };
        

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    