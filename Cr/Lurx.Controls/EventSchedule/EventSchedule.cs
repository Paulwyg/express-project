//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Lurx.Controls.EventSchedule
//{
//    public class EventSchedule : IIEventSchedule
//    {
//        public EventSchedule()
//        {
//            this.IsCheckEveryDay = true;
//            this.StartDate = DateTime.Now;
//            this.StartHour = DateTime.Now.Hour;
//            this.StartMinutes = DateTime.Now.Minute;
//            this.RepeatTimes = 1;
//            this.IsCheckNoEndDate = true;

//            this.EveryHours = 1;
//            this.EveryDays = 1;
//            this.EveryMinuters = 15;
//            this.EveryMonths = 1;
//            this.EveryWeeks = 1;
//            this.EveryYearMonth = 1;
//            this.EveryYearMonthDate = 1;
//            this.EveryMonthDate = 1;
//        }

//        public EventSchedule(IIEventSchedule info)
//        {
//            this.IsCheckEveryMinute = info.IsCheckEveryMinute;

//            this.IsCheckEveryHour = info.IsCheckEveryHour;

//            this.IsCheckEveryDay = info.IsCheckEveryDay;

//            this.IsCheckEveryWeek = info.IsCheckEveryWeek;

//            this.IsCheckEveryMonth = info.IsCheckEveryMonth;

//            this.IsCheckEveryYear = info.IsCheckEveryYear;

//            this.EveryMinuters = info.EveryMinuters;

//            this.EveryHours = info.EveryHours;

//            this.EveryDays = info.EveryDays;

//            this.EveryWeeks = info.EveryWeeks;

//            this.EveryMonths = info.EveryMonths;

//            this.IsCheckedMon = info.IsCheckedMon;

//            this.IsCheckedTue = info.IsCheckedTue;

//            this.IsCheckedWen = info.IsCheckedWen;

//            this.IsCheckedThr = info.IsCheckedThr;

//            this.IsCheckedFri = info.IsCheckedFri;

//            this.IsCheckedSta = info.IsCheckedSta;

//            this.IsCheckedSun = info.IsCheckedSun;

//            this.EveryMonthDate = info.EveryMonthDate;

//            this.EveryYearMonth = info.EveryYearMonth;

//            this.EveryYearMonthDate = info.EveryYearMonthDate;

//            this.StartDate = info.StartDate;

//            this.StartHour = info.StartHour;

//            this.StartMinutes = info.StartMinutes;

//            this.IsCheckNoEndDate = info.IsCheckNoEndDate;

//            this.IsCheckRepeatTime = info.IsCheckRepeatTime;

//            this.RepeatTimes = info.RepeatTimes;


//        }


//        public bool IsCheckEveryMinute { get; set; }

//        public bool IsCheckEveryHour { get; set; }

//        public bool IsCheckEveryDay { get; set; }

//        public bool IsCheckEveryWeek { get; set; }

//        public bool IsCheckEveryMonth { get; set; }

//        public bool IsCheckEveryYear { get; set; }

//        public int EveryMinuters { get; set; }

//        public int EveryHours { get; set; }

//        public int EveryDays { get; set; }

//        public int EveryWeeks { get; set; }

//        public int EveryMonths { get; set; }

//        public bool IsCheckedMon { get; set; }

//        public bool IsCheckedTue { get; set; }

//        public bool IsCheckedWen { get; set; }

//        public bool IsCheckedThr { get; set; }

//        public bool IsCheckedFri { get; set; }

//        public bool IsCheckedSta { get; set; }

//        public bool IsCheckedSun { get; set; }

//        public int EveryMonthDate { get; set; }

//        public int EveryYearMonth { get; set; }

//        public int EveryYearMonthDate { get; set; }

//        public DateTime StartDate { get; set; }

//        public int StartHour { get; set; }

//        public int StartMinutes { get; set; }

//        public bool IsCheckNoEndDate { get; set; }

//        public bool IsCheckRepeatTime { get; set; }

//        public int RepeatTimes { get; set; }
//    }
//}
