using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using Wlst.Cr.Core.CoreServices;
using Wlst.Sr.TimeTableSystem.Models;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel
{
  
public class OneDayTimeTableViewModel : ObservableObject
    {
        public OneDayTimeTableViewModel()
        {
            this.TimeOff = 25 * 60;
            this.TimeOn = 25 * 60;
        }

        public OneDayTimeTableViewModel(TimeTableOneDayInfomationItem weekTimeTable)
        {
            this.RuleSectionId = weekTimeTable.RuleSectionId;
            this.TimetableSectionId = weekTimeTable.TimetableSectionId;
            this.DayOfWeekUsed = weekTimeTable.DayOfWeekUsed;
            this.Date = weekTimeTable.Date;
            this.DateDay = weekTimeTable.DateDay;
            this.DateMonth = weekTimeTable.DateMonth;
            this.IsLightOffOffSetOn = weekTimeTable.IsUsedOffSet;
            this.IsLightOnOffSetOn = weekTimeTable.IsUsedOnSet;
            this.IsLuxOff = weekTimeTable.IsUsedLuxOff;
            this.IsLuxOn = weekTimeTable.IsUsedLuxOn;
            this.TimeOff = weekTimeTable.TimeOff;
            this.TimeOn = weekTimeTable.TimeOn;
        }


        public TimeTableOneDayInfomationItem BackToWeekTimeTableItemInfomation()
        {
            var weekTimeTable = new TimeTableOneDayInfomationItem();
            weekTimeTable.RuleSectionId = this.RuleSectionId;
            weekTimeTable.TimetableSectionId = this.TimetableSectionId;
            weekTimeTable.DayOfWeekUsed = this.DayOfWeekUsed;
            weekTimeTable.Date = this.Date;
            weekTimeTable.DateDay = this.DateDay;
            weekTimeTable.DateMonth = this.DateMonth;
            weekTimeTable.IsUsedOffSet = this.IsLightOffOffSetOn;
            weekTimeTable.IsUsedOnSet = this.IsLightOnOffSetOn;
            weekTimeTable.IsUsedLuxOff = this.IsLuxOff;
            weekTimeTable.IsUsedLuxOn = this.IsLuxOn;
            weekTimeTable.TimeOff = this.TimeOff;
            weekTimeTable.TimeOn = this.TimeOn;
            return weekTimeTable;
        }

        public bool Compare(TimeTableOneDayInfomationItem weekTimeTable)
        {
            if (this.RuleSectionId != weekTimeTable.RuleSectionId)
            {
                return false;
            }
            if (this.TimetableSectionId != weekTimeTable.TimetableSectionId)
            {
                return false;
            }
            if (this.DayOfWeekUsed != weekTimeTable.DayOfWeekUsed)
            {
                return false;
            }
            if (this.Date != weekTimeTable.Date)
            {
                return false;
            }
            if (this.IsLightOffOffSetOn != weekTimeTable.IsUsedOffSet)
            {
                return false;
            }
            if (this.IsLightOnOffSetOn != weekTimeTable.IsUsedOnSet)
            {
                return false;
            }
            if (this.IsLuxOff != weekTimeTable.IsUsedLuxOff)
            {
                return false;
            }
            if (this.IsLuxOn != weekTimeTable.IsUsedLuxOn)
            {
                return false;
            }
            if (this.TimeOff != weekTimeTable.TimeOff)
            {
                return false;
            }
            if (this.TimeOn != weekTimeTable.TimeOn)
            {
                return false;
            }
            return true;
        }

        #region WeekTimeTableItemInfomation

        private string _date;

        /// <summary>
        /// 日期
        /// </summary>
        public string Date
        {
            get
            {
                return _date;
            }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    this.RaisePropertyChanged(() => this.Date);
                }
            }
        }
        private int _dateMonth;

        /// <summary>
        /// 月
        /// </summary>
        public int DateMonth
        {
            get
            {
                return _dateMonth;
            }
            set
            {
                if (_dateMonth != value)
                {
                    _dateMonth = value;
                    this.RaisePropertyChanged(() => this.DateMonth);
                }
            }
        }

        private int _dateDay;

        /// <summary>
        /// 日
        /// </summary>
        public int DateDay
        {
            get
            {
                return _dateDay;
            }
            set
            {
                if (_dateDay != value)
                {
                    _dateDay = value;
                    this.RaisePropertyChanged(() => this.DateDay);
                }
            }
        }


        private bool _isLuxOn;

        /// <summary>
        /// 是否为光照度控制开灯
        /// </summary>
        public bool IsLuxOn
        {
            get
            {
                return _isLuxOn;
            }
            set
            {
                if (_isLuxOn != value)
                {
                    _isLuxOn = value;
                    this.RaisePropertyChanged(() => this.IsLuxOn);
                    this.CalculateTimeOn();
                }
            }
        }

        private bool _isLuxOff;

        /// <summary>
        /// 是否为光照度控制关灯
        /// </summary>
        public bool IsLuxOff
        {
            get
            {
                return _isLuxOff;
            }
            set
            {
                if (_isLuxOff != value)
                {
                    _isLuxOff = value;
                    this.RaisePropertyChanged(() => this.IsLuxOff);
                    this.CalculateTimeOff();
                }
            }
        }

        private int _timeOn;

        /// <summary>
        /// 开灯最后时限
        /// </summary>
        public int TimeOn
        {
            get
            {
                return _timeOn;
            }
            set
            {
                if (_timeOn != value)
                {
                    this.TimeOnCheck(ref value);
                    if (value == 0) value = 1; 
                    _timeOn = value;                                       
                    this.CalculateTimeOn();
                    this.RaisePropertyChanged(() => this.TimeOn);
                }
            }
        }

        private int _timeOff;

        /// <summary>
        /// 关灯最后时限
        /// </summary>
        public int TimeOff
        {
            get
            {
                return _timeOff;
            }
            set
            {
                if (_timeOff != value)
                {
                    this.TimeOffCheck(ref value);
                    if (value == 0) value = 1439;
                    _timeOff = value;  
                    this.CalculateTimeOff();
                    this.RaisePropertyChanged(() => this.TimeOff);
                }
            }
        }

        private bool _isLightOnOffSetOn;

        /// <summary>
        /// 是否为使用开灯时间偏移
        /// </summary>
        public bool IsLightOnOffSetOn
        {
            get
            {
                return _isLightOnOffSetOn;
            }
            set
            {
                if (_isLightOnOffSetOn != value)
                {
                    _isLightOnOffSetOn = value;
                    this.RaisePropertyChanged(() => this.IsLightOnOffSetOn);
                    this.CalculateTimeOn();
                }
            }
        }

        private bool _isLightOffOffSetOn;

        /// <summary>
        /// 是否使用关灯时间偏移
        /// </summary>
        public bool IsLightOffOffSetOn
        {
            get
            {
                return _isLightOffOffSetOn;
            }
            set
            {
                if (_isLightOffOffSetOn != value)
                {
                    _isLightOffOffSetOn = value;
                    this.RaisePropertyChanged(() => this.IsLightOffOffSetOn);
                    this.CalculateTimeOff();
                }
            }
        }

        private int _rulesectionid;

        /// <summary>
        /// 规则列表  如第1个规则中的第一选项 可能包含7个，一周每天不一样
        /// </summary>
        public int RuleSectionId
        {
            get { return _rulesectionid; }
            set
            {
                if (_rulesectionid != value)
                {
                    _rulesectionid = value;
                    this.RaisePropertyChanged(() => this.RuleSectionId);
                }
            }
        }

        private int _timetablesectionid;

        /// <summary>
        /// 时间段序号 1-4
        /// </summary>
        public int TimetableSectionId
        {
            get { return _timetablesectionid; }
            set
            {
                if (_timetablesectionid != value)
                {
                    _timetablesectionid = value;
                    this.RaisePropertyChanged(() => this.TimetableSectionId);
                }
            }
        }

        private int _dayofweekused;

        /// <summary>
        /// 本规则那几天使用 0为周日  1-6 表示星期几  可多选，但同一分段规则中的多段不能包含同一天
        /// </summary>
        public int DayOfWeekUsed
        {
            get { return _dayofweekused; }
            set
            {
                if (_dayofweekused != value)
                {
                    _dayofweekused = value;
                    this.RaisePropertyChanged(() => this.DayOfWeekUsed);
                }
            }
        }
        #endregion

        #region 辅助显示数据

        private int _lightOffOffSet;

        /// <summary>
        /// 关灯偏移量
        /// </summary>
        public int LightOffOffSet
        {
            get
            {
                return _lightOffOffSet;
            }
            set
            {
                if (_lightOffOffSet != value)
                {
                    _lightOffOffSet = value;
                    this.RaisePropertyChanged(() => this.LightOffOffSet);
                    this.CalculateTimeOff();
                }
            }
        }

        private int _lightOnOffSet;

        /// <summary>
        /// 开灯偏移量
        /// </summary>
        public int LightOnOffSet
        {
            get
            {
                return _lightOnOffSet;
            }
            set
            {
                if (_lightOnOffSet != value)
                {
                    _lightOnOffSet = value;
                    this.RaisePropertyChanged(() => this.LightOnOffSet);
                    this.CalculateTimeOn();
                }
            }
        }

        private bool _isTimeOnEnable;

        /// <summary>
        /// 界面自定义设定开灯时间是否可修改
        /// </summary>
        public bool IsTimeOnEnable
        {
            get
            {
                return _isTimeOnEnable;
            }
            set
            {
                if (_isTimeOnEnable != value)
                {
                    _isTimeOnEnable = value;
                    this.RaisePropertyChanged(() => this.IsTimeOnEnable);
                }
            }
        }

        private bool _isTimeOffEnable;

        /// <summary>
        /// 界面自定义设定关灯时间是否可修改
        /// </summary>
        public bool IsTimeOffEnable
        {
            get
            {
                return _isTimeOffEnable;
            }
            set
            {
                if (_isTimeOffEnable != value)
                {
                    _isTimeOffEnable = value;
                    this.RaisePropertyChanged(() => this.IsTimeOffEnable);
                }
            }
        }

        private string _msgOffHelp;

        public string MsgOffHelp
        {
            get
            {
                return _msgOffHelp;
            }
            set
            {
                if (_msgOffHelp != value)
                {
                    _msgOffHelp = value;
                    this.RaisePropertyChanged(() => this.MsgOffHelp);
                }
            }
        }

        private string _msgOnHelp;

        public string MsgOnHelp
        {
            get
            {
                return _msgOnHelp;
            }
            set
            {
                if (_msgOnHelp != value)
                {
                    _msgOnHelp = value;
                    this.RaisePropertyChanged(() => this.MsgOnHelp);
                }
            }
        }

        #endregion

        #region 输出

        public TimeTableOneDayInfomationItem WeekTimeTableItem
        {
            get
            {
                TimeTableOneDayInfomationItem weekTimeTable = new TimeTableOneDayInfomationItem();
                weekTimeTable.RuleSectionId = this.RuleSectionId;
                weekTimeTable.TimetableSectionId = this.TimetableSectionId;
                weekTimeTable.DayOfWeekUsed = this.DayOfWeekUsed;
                weekTimeTable.Date = this.Date;
                weekTimeTable.IsUsedOffSet = this.IsLightOffOffSetOn;
                weekTimeTable.IsUsedOnSet = this.IsLightOnOffSetOn;
                weekTimeTable.IsUsedLuxOff = this.IsLuxOff;
                weekTimeTable.IsUsedLuxOn = this.IsLuxOn;
                weekTimeTable.TimeOff = this.TimeOff;
                weekTimeTable.TimeOn = this.TimeOn;
                return weekTimeTable;
            }
        }

        #endregion

        /// <summary>
        /// 赋值前对时间进行检测  不符合条件则设置为不开灯时间
        /// </summary>
        /// <param name="value"></param>
        private void TimeOnCheck(ref int value)
        {
            int hour = value / 60;
            if (hour > -1 && hour < 24)
            {
                MsgOnHelp = "";
                return;
            }
            else
                value = 25 * 60;
            MsgOnHelp = "软件、硬件均将不执行开灯操作";
        }

        /// <summary>
        /// 赋值前对时间进行检测  不符合条件则设置为不开灯时间
        /// </summary>
        /// <param name="value"></param>
        private void TimeOffCheck(ref int value)
        {
            int hour = value / 60;
            if (hour > -1 && hour < 24)
            {
                MsgOffHelp = "";
                return;
            }
            else
                value = 25 * 60;
            MsgOffHelp = "软件、硬件均将不执行关灯操作";
        }

        /// <summary>
        /// 计算开灯时间
        /// </summary>
        public void CalculateTimeOn()
        {
            if (IsLuxOn || IsLightOnOffSetOn)
            {
                if (IsLuxOn)
                    IsLightOnOffSetOn = true;
                IsTimeOnEnable = false;
                var oneDaysOnOffTime = Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(this.DateMonth,this.DateDay);

                if (oneDaysOnOffTime == null)
                {
                    this._timeOn = 25 * 60;
                    this.RaisePropertyChanged(() => this.TimeOn);
                    MsgOnHelp = "无法查阅日出日落时间，自动设为不开灯";
                }
                else
                    this.TimeOn = oneDaysOnOffTime.time_sunset + LightOnOffSet;
            }
            else
            {
                IsTimeOnEnable = true;
            }
        }

        /// <summary>
        /// 计算关灯时间
        /// </summary>
        public void CalculateTimeOff()
        {
            //如果光控则 时间为日出日落时间 + 偏移量，并且时间不可修改；
            //如果不是光控 如果偏移量不为0 则使用偏移
            if (IsLuxOff || IsLightOffOffSetOn)
            {
                if (IsLuxOff)
                    IsLightOffOffSetOn = true;

                IsTimeOffEnable = false;
                var oneDaysOnOffTime = Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(this.DateMonth, this.DateDay);
                if (oneDaysOnOffTime == null)
                {
                    this._timeOff = 25 * 60;
                    this.RaisePropertyChanged(() => this.TimeOn);
                    MsgOffHelp = "无法查阅日出日落时间，自动设置为不开灯";
                }
                else
                    this.TimeOff = oneDaysOnOffTime.time_sunrise + LightOffOffSet;
            }
            else
            {
                IsTimeOffEnable = true;
            }
        }
    }




}
