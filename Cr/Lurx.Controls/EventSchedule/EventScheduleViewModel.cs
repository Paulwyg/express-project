using System;
using System.ComponentModel;
using System.Windows;


namespace Lurx.Controls.EventSchedule
{
    public class EventScheduleViewModel : INotifyPropertyChanged 
    {
        public EventScheduleViewModel()
        {
            this.IsCheckEveryDay = true;
            this.StartDate = DateTime.Now;
            this.StartHour = DateTime.Now.Hour;
            this.StartMinutes = DateTime.Now.Minute;
            this.RepeatTimes = 1;
            this.IsCheckNoEndDate = true;

            this.EveryHours = 1;
            this.EveryDays = 1;
            this.EveryMinuters = 15;
            this.EveryMonths = 1;
            this.EveryWeeks = 1;
            this.EveryYearMonth = 1;
            this.EveryYearMonthDate = 1;
            this.EveryMonthDate = 1;
        }

        public EventScheduleViewModel(Wlst.client.EventSchedule info)
        {
            this.StartDate = new DateTime( info.StartDate);
            this.StartHour = info.StartHour;
            this.StartMinutes = info.StartMinutes;
            if (info.IsCheckNoEndDate)
            {
                this.IsCheckNoEndDate = true;
                this.RepeatTimes = 1;
            }
            if (info.IsCheckRepeatTime)
            {
                this.IsCheckRepeatTime = true;
                this.RepeatTimes = info.RepeatTimes;
            }

            this.EveryHours = 1;
            this.EveryDays = 1;
            this.EveryMinuters = 15;
            this.EveryMonths = 1;
            this.EveryWeeks = 1;
            this.EveryYearMonth = 1;
            this.EveryYearMonthDate = 1;
            this.EveryMonthDate = 1;

            if (info.IsCheckEveryMinute)
            {
                this.IsCheckEveryMinute = true;
                this.EveryMinuters = info.EveryMinuters;
            }
            else if (info.IsCheckEveryHour)
            {
                this.IsCheckEveryHour = true;
                this.EveryHours = info.EveryHours;
            }
            else if (info.IsCheckEveryDay)
            {
                this.IsCheckEveryDay = true;
                this.EveryDays = info.EveryDays;
            }
            else if (info.IsCheckEveryWeek )
            {
                this.IsCheckEveryWeek = true;
                this.EveryWeeks = info.EveryWeeks;
                this.IsCheckedMon = info.IsCheckedMon;
                this.IsCheckedTue = info.IsCheckedTue;
                this.IsCheckedWen = info.IsCheckedWen;
                this.IsCheckedThr = info.IsCheckedThr;
                this.IsCheckedFri = info.IsCheckedFri;
                this.IsCheckedSta = info.IsCheckedSta;
                this.IsCheckedSun = info.IsCheckedSun;
            }
            else if (info.IsCheckEveryMonth)
            {
                this.IsCheckEveryMonth = true;
                this.EveryMonths = info.EveryMonths;
                this.EveryMonthDate = info.EveryMonthDate;
            }
            else if (info.IsCheckEveryYear )
            {
                this.IsCheckEveryYear = true ;
                this.EveryYearMonth = info.EveryYearMonth;
                this.EveryYearMonthDate = info.EveryYearMonthDate;
            }
        }

        public Wlst .client .EventSchedule GetEventSchedule()
        {
            return new Wlst.client.EventSchedule()
                       {
                           EveryDays = this.EveryDays,
                           EveryHours = this.EveryHours,
                           EveryMinuters = this.EveryMinuters,
                           EveryMonthDate = this.EveryMonthDate,
                           EveryMonths = this.EveryMonths,
                           EveryWeeks = this.EveryWeeks,
                           EveryYearMonth = this.EveryYearMonth,
                           EveryYearMonthDate = this.EveryYearMonthDate,
                           IsCheckEveryDay = this.IsCheckEveryDay,
                           IsCheckEveryHour = this.IsCheckEveryHour,
                           IsCheckEveryMinute = this.IsCheckEveryMinute,
                           IsCheckEveryMonth = this.IsCheckEveryMonth,
                           IsCheckEveryWeek = this.IsCheckEveryWeek,
                           IsCheckEveryYear = this.IsCheckEveryYear,
                           IsCheckNoEndDate = this.IsCheckNoEndDate,
                           IsCheckRepeatTime = this.IsCheckRepeatTime,
                           IsCheckedFri = this.IsCheckedFri,
                           IsCheckedMon = this.IsCheckedMon,
                           IsCheckedSta = this.IsCheckedSta,
                           IsCheckedSun = this.IsCheckedSun,
                           IsCheckedThr = this.IsCheckedThr,
                           IsCheckedTue = this.IsCheckedTue,
                           IsCheckedWen = this.IsCheckedWen,
                           RepeatTimes = this.RepeatTimes,
                           StartDate = this.StartDate.Ticks,
                           StartHour = this.StartHour,
                           StartMinutes = this.StartMinutes,

                       };
        }

        //this.IsCheckEveryMinute = true;
        //           this.IsCheckEveryHour = true;
        //           this.IsCheckEveryDay = true;
        //           this.IsCheckEveryWeek = true;
        //           this.IsCheckEveryMonth = true;
        //           this.IsCheckEveryYear = true;

        #region  is check

        private bool _isCheckEveryMinute;

        public bool IsCheckEveryMinute
        {
            get { return _isCheckEveryMinute; }
            set
            {
                if (value != _isCheckEveryMinute)
                {
                    _isCheckEveryMinute = value;
                    this.RaisePropertyChanged("IsCheckEveryMinute");
                    if (value) SetOtherFlaseAndNotVisi("IsCheckEveryMinute", true);
                }
            }
        }

        private bool _isCheckEveryHour;

        public bool IsCheckEveryHour
        {
            get { return _isCheckEveryHour; }
            set
            {
                if (value != _isCheckEveryHour)
                {
                    _isCheckEveryHour = value;
                    this.RaisePropertyChanged("IsCheckEveryHour");
                    if (value) SetOtherFlaseAndNotVisi("IsCheckEveryHour", true);
                }
            }
        }

        private bool _isCheckEveryDay;

        public bool IsCheckEveryDay
        {
            get { return _isCheckEveryDay; }
            set
            {
                if (value != _isCheckEveryDay)
                {
                    _isCheckEveryDay = value;
                    this.RaisePropertyChanged("IsCheckEveryDay");
                    if (value) SetOtherFlaseAndNotVisi("IsCheckEveryDay", true);
                }
            }
        }

        private bool _isCheckEveryWeek;

        public bool IsCheckEveryWeek
        {
            get { return _isCheckEveryWeek; }
            set
            {
                if (value != _isCheckEveryWeek)
                {
                    _isCheckEveryWeek = value;
                    this.RaisePropertyChanged("IsCheckEveryWeek");
                    if (value) SetOtherFlaseAndNotVisi("IsCheckEveryWeek", true);
                }
            }
        }

        private bool _isCheckEveryMonth;

        public bool IsCheckEveryMonth
        {
            get { return _isCheckEveryMonth; }
            set
            {
                if (value != _isCheckEveryMonth)
                {
                    _isCheckEveryMonth = value;
                    this.RaisePropertyChanged("IsCheckEveryMonth");
                    if (value) SetOtherFlaseAndNotVisi("IsCheckEveryMonth", true);
                }
            }
        }

        private bool _isCheckEveryYear;

        public bool IsCheckEveryYear
        {
            get { return _isCheckEveryYear; }
            set
            {
                if (value != _isCheckEveryYear)
                {
                    _isCheckEveryYear = value;
                    this.RaisePropertyChanged("IsCheckEveryYear");
                    if (value) SetOtherFlaseAndNotVisi("IsCheckEveryYear", true);
                }
            }
        }


        //this.IsCheckEveryMinute = false;
        //           this.IsCheckEveryHour = false;
        //           this.IsCheckEveryDay = false;
        //           this.IsCheckEveryWeek = false;
        //           this.IsCheckEveryMonth = false;
        //           this.IsCheckEveryYear = false;
        //           this.EveryMinutersVisibility = Visibility.Collapsed;
        //           this.EveryHoursVisibility = Visibility.Collapsed;
        //           this.EveryDaysVisibility = Visibility.Collapsed;
        //           this.EveryWeeksVisibility = Visibility.Collapsed;
        //           this.EveryMonthsVisibility = Visibility.Collapsed;
        //           this.EveryYearVisibility = Visibility.Collapsed;
        private void SetOtherFlaseAndNotVisi(string name, bool value)
        {
            if (!value) return;
            switch (name)
            {
                case "IsCheckEveryMinute":
                    this.IsCheckEveryHour = false;
                    this.IsCheckEveryDay = false;
                    this.IsCheckEveryWeek = false;
                    this.IsCheckEveryMonth = false;
                    this.IsCheckEveryYear = false;
                    this.EveryMinutersVisibility = Visibility.Visible;
                    this.EveryHoursVisibility = Visibility.Collapsed;
                    this.EveryDaysVisibility = Visibility.Collapsed;
                    this.EveryWeeksVisibility = Visibility.Collapsed;
                    this.EveryMonthsVisibility = Visibility.Collapsed;
                    this.EveryYearVisibility = Visibility.Collapsed;
                    break;

                case "IsCheckEveryHour":
                    this.IsCheckEveryMinute = false;
                    this.IsCheckEveryDay = false;
                    this.IsCheckEveryWeek = false;
                    this.IsCheckEveryMonth = false;
                    this.IsCheckEveryYear = false;
                    this.EveryMinutersVisibility = Visibility.Collapsed;
                    this.EveryHoursVisibility = Visibility.Visible;
                    this.EveryDaysVisibility = Visibility.Collapsed;
                    this.EveryWeeksVisibility = Visibility.Collapsed;
                    this.EveryMonthsVisibility = Visibility.Collapsed;
                    this.EveryYearVisibility = Visibility.Collapsed;
                    break;

                case "IsCheckEveryDay":
                    this.IsCheckEveryMinute = false;
                    this.IsCheckEveryHour = false;
                    this.IsCheckEveryWeek = false;
                    this.IsCheckEveryMonth = false;
                    this.IsCheckEveryYear = false;
                    this.EveryMinutersVisibility = Visibility.Collapsed;
                    this.EveryHoursVisibility = Visibility.Collapsed;
                    this.EveryDaysVisibility = Visibility.Visible;
                    this.EveryWeeksVisibility = Visibility.Collapsed;
                    this.EveryMonthsVisibility = Visibility.Collapsed;
                    this.EveryYearVisibility = Visibility.Collapsed;
                    break;

                case "IsCheckEveryWeek":
                    this.IsCheckEveryMinute = false;
                    this.IsCheckEveryHour = false;
                    this.IsCheckEveryDay = false;
                    this.IsCheckEveryMonth = false;
                    this.IsCheckEveryYear = false;
                    this.EveryMinutersVisibility = Visibility.Collapsed;
                    this.EveryHoursVisibility = Visibility.Collapsed;
                    this.EveryDaysVisibility = Visibility.Collapsed;
                    this.EveryWeeksVisibility = Visibility.Visible;
                    this.EveryMonthsVisibility = Visibility.Collapsed;
                    this.EveryYearVisibility = Visibility.Collapsed;
                    break;

                case "IsCheckEveryMonth":
                    this.IsCheckEveryMinute = false;
                    this.IsCheckEveryHour = false;
                    this.IsCheckEveryDay = false;
                    this.IsCheckEveryWeek = false;
                    this.IsCheckEveryYear = false;
                    this.EveryMinutersVisibility = Visibility.Collapsed;
                    this.EveryHoursVisibility = Visibility.Collapsed;
                    this.EveryDaysVisibility = Visibility.Collapsed;
                    this.EveryWeeksVisibility = Visibility.Collapsed;
                    this.EveryMonthsVisibility = Visibility.Visible;
                    this.EveryYearVisibility = Visibility.Collapsed;
                    break;

                case "IsCheckEveryYear":
                    this.IsCheckEveryMinute = false;
                    this.IsCheckEveryHour = false;
                    this.IsCheckEveryDay = false;
                    this.IsCheckEveryWeek = false;
                    this.IsCheckEveryMonth = false;
                    this.EveryMinutersVisibility = Visibility.Collapsed;
                    this.EveryHoursVisibility = Visibility.Collapsed;
                    this.EveryDaysVisibility = Visibility.Collapsed;
                    this.EveryWeeksVisibility = Visibility.Collapsed;
                    this.EveryMonthsVisibility = Visibility.Collapsed;
                    this.EveryYearVisibility = Visibility.Visible;
                    break;

            }
        }

        #endregion

        #region visibility

        private Visibility _everyMinutersVisibility;

        public Visibility EveryMinutersVisibility
        {
            get { return _everyMinutersVisibility; }
            set
            {
                if (value != _everyMinutersVisibility)
                {
                    _everyMinutersVisibility = value;
                    this.RaisePropertyChanged("EveryMinutersVisibility");
                }
            }
        }

        private Visibility _everyHoursVisibility;

        public Visibility EveryHoursVisibility
        {
            get { return _everyHoursVisibility; }
            set
            {
                if (value != _everyHoursVisibility)
                {
                    _everyHoursVisibility = value;
                    this.RaisePropertyChanged("EveryHoursVisibility");
                }
            }
        }

        private Visibility _everyDaysVisibility;

        public Visibility EveryDaysVisibility
        {
            get { return _everyDaysVisibility; }
            set
            {
                if (value != _everyDaysVisibility)
                {
                    _everyDaysVisibility = value;
                    this.RaisePropertyChanged("EveryDaysVisibility");
                }
            }
        }

        private Visibility _everyWeeksVisibility;

        public Visibility EveryWeeksVisibility
        {
            get { return _everyWeeksVisibility; }
            set
            {
                if (value != _everyWeeksVisibility)
                {
                    _everyWeeksVisibility = value;
                    this.RaisePropertyChanged("EveryWeeksVisibility");
                }
            }
        }

        private Visibility _everyMonthsVisibility;

        public Visibility EveryMonthsVisibility
        {
            get { return _everyMonthsVisibility; }
            set
            {
                if (value != _everyMonthsVisibility)
                {
                    _everyMonthsVisibility = value;
                    this.RaisePropertyChanged("EveryMonthsVisibility");
                }
            }
        }

        private Visibility _everyYearVisibility;

        public Visibility EveryYearVisibility
        {
            get { return _everyYearVisibility; }
            set
            {
                if (value != _everyYearVisibility)
                {
                    _everyYearVisibility = value;
                    this.RaisePropertyChanged("EveryYearVisibility");
                }
            }
        }

        #endregion

        #region  the space between two event

        private int _everyMinuters;

        public int EveryMinuters
        {
            get { return _everyMinuters; }
            set
            {
                if (value != _everyMinuters)
                {
                    if (value < 0) return;
                    _everyMinuters = value;
                    this.RaisePropertyChanged("EveryMinuters");
                }
            }
        }

        private int _everyHours;

        public int EveryHours
        {
            get { return _everyHours; }
            set
            {
                if (value != _everyHours)
                {
                    if (value < 0) return;
                    _everyHours = value;
                    this.RaisePropertyChanged("EveryHours");
                }
            }
        }

        private int _everyDays;

        public int EveryDays
        {
            get { return _everyDays; }
            set
            {
                if (value != _everyDays)
                {
                    if (value < 0) return;
                    _everyDays = value;
                    this.RaisePropertyChanged("EveryDays");
                }
            }
        }

        private int _everyWeeks;

        public int EveryWeeks
        {
            get { return _everyWeeks; }
            set
            {
                if (value != _everyWeeks)
                {
                    if (value < 0) return;
                    _everyWeeks = value;
                    this.RaisePropertyChanged("EveryWeeks");
                }
            }
        }

        private int _everyMonths;

        public int EveryMonths
        {
            get { return _everyMonths; }
            set
            {
                if (value < 0) return;
                if (value != _everyMonths)
                {
                    _everyMonths = value;
                    this.RaisePropertyChanged("EveryMonths");
                }
            }
        }

        #endregion

        #region if select Week then

        private bool _isCheckedMon;

        public bool IsCheckedMon
        {
            get { return _isCheckedMon; }
            set
            {
                if (value != _isCheckedMon)
                {
                    _isCheckedMon = value;
                    this.RaisePropertyChanged("IsCheckedMon");
                }
            }
        }

        private bool _IsCheckedTue;

        public bool IsCheckedTue
        {
            get { return _IsCheckedTue; }
            set
            {
                if (value != _IsCheckedTue)
                {
                    _IsCheckedTue = value;
                    this.RaisePropertyChanged("IsCheckedTue");
                }
            }
        }

        private bool _IsCheckedWen;

        public bool IsCheckedWen
        {
            get { return _IsCheckedWen; }
            set
            {
                if (value != _IsCheckedWen)
                {
                    _IsCheckedWen = value;
                    this.RaisePropertyChanged("IsCheckedWen");
                }
            }
        }

        private bool _IsCheckedThr;

        public bool IsCheckedThr
        {
            get { return _IsCheckedThr; }
            set
            {
                if (value != _IsCheckedThr)
                {
                    _IsCheckedThr = value;
                    this.RaisePropertyChanged("IsCheckedThr");
                }
            }
        }

        private bool _IsCheckedFri;

        public bool IsCheckedFri
        {
            get { return _IsCheckedFri; }
            set
            {
                if (value != _IsCheckedFri)
                {
                    _IsCheckedFri = value;
                    this.RaisePropertyChanged("IsCheckedFri");
                }
            }
        }

        private bool _IsCheckedSta;

        public bool IsCheckedSta
        {
            get { return _IsCheckedSta; }
            set
            {
                if (value != _IsCheckedSta)
                {
                    _IsCheckedSta = value;
                    this.RaisePropertyChanged("IsCheckedSta");
                }
            }
        }

        private bool _IsCheckedSun;

        public bool IsCheckedSun
        {
            get { return _IsCheckedSun; }
            set
            {
                if (value != _IsCheckedSun)
                {
                    _IsCheckedSun = value;
                    this.RaisePropertyChanged("IsCheckedSun");
                }
            }
        }

        #endregion

        #region if select month  or  year

        private int _EveryMonthDate;

        public int EveryMonthDate
        {
            get { return _EveryMonthDate; }
            set
            {
                if (value != _EveryMonthDate)
                {
                    if (value < 1) return;
                    if (value > 31) return;
                    _EveryMonthDate = value;
                    this.RaisePropertyChanged("EveryMonthDate");
                }
            }
        }

        private int _EveryYearMonth;

        public int EveryYearMonth
        {
            get { return _EveryYearMonth; }
            set
            {
                if (value != _EveryYearMonth)
                {
                    if (value < 1) return;
                    if (value > 12) return;
                    _EveryYearMonth = value;
                    this.RaisePropertyChanged("EveryYearMonth");
                }
            }
        }

        private int _EveryYearMonthDate;

        public int EveryYearMonthDate
        {
            get { return _EveryYearMonthDate; }
            set
            {
                if (value != _EveryYearMonthDate)
                {
                    if (value < 1) return;
                    if (value > 31) return;
                    _EveryYearMonthDate = value;
                    this.RaisePropertyChanged("EveryYearMonthDate");
                }
            }
        }

        #endregion

        #region  Time and  Repeat timers set

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    this.RaisePropertyChanged("StartDate");
                }
            }
        }

        private int _startHour;

        public int StartHour
        {
            get { return _startHour; }
            set
            {
                if (value != _startHour)
                {
                    if (value < 0) return;
                    if (value > 23) return;
                    _startHour = value;
                    this.RaisePropertyChanged("StartHour");
                }
            }
        }

        private int _startMinutes;

        public int StartMinutes
        {
            get { return _startMinutes; }
            set
            {
                if (value != _startMinutes)
                {
                    if (value < 0) return;
                    if (value > 59) return;
                    _startMinutes = value;
                    this.RaisePropertyChanged("StartMinutes");
                }
            }
        }

        private bool _isCheckNoEndDate;

        public bool IsCheckNoEndDate
        {
            get { return _isCheckNoEndDate; }
            set
            {
                if (value != _isCheckNoEndDate)
                {
                    _isCheckNoEndDate = value;
                    this.RaisePropertyChanged("IsCheckNoEndDate");
                    if (value)
                    {
                        IsCheckRepeatTime = false;
                    }
                }
            }
        }

        private bool _isCheckRepeatTime;

        public bool IsCheckRepeatTime
        {
            get { return _isCheckRepeatTime; }
            set
            {
                if (value != _isCheckRepeatTime)
                {
                    _isCheckRepeatTime = value;
                    this.RaisePropertyChanged("IsCheckRepeatTime");
                    if (value)
                    {
                        IsCheckNoEndDate = false;
                    }
                }
            }
        }

        private int _repeatTimes;

        public int RepeatTimes
        {
            get { return _repeatTimes; }
            set
            {
                if (value != _repeatTimes)
                {
                    if (value < 1) value = 1;
                    _repeatTimes = value;
                    this.RaisePropertyChanged("RepeatTimes");
                }
            }
        }

        #endregion

        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion


 
    }
}
