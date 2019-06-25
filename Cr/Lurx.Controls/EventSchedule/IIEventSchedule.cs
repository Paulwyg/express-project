//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Lurx.Controls.EventSchedule
//{
//    public interface IIEventSchedule
//    {
//        /// <summary>
//        /// 是否选择的是每 分钟
//        /// </summary>
//        bool IsCheckEveryMinute { get; set; }

//        /// <summary>
//        /// 是否选择的是每 小时
//        /// </summary>
//        bool IsCheckEveryHour { get; set; }

//        /// <summary>
//        /// 是否选择的是每 天
//        /// </summary>
//        bool IsCheckEveryDay { get; set; }

//        /// <summary>
//        /// 是否选择的是每 周
//        /// </summary>
//        bool IsCheckEveryWeek { get; set; }

//        /// <summary>
//        /// 是否选择的是每 月
//        /// </summary>
//        bool IsCheckEveryMonth { get; set; }

//        /// <summary>
//        /// 是否选择的是每 年
//        /// </summary>
//        bool IsCheckEveryYear { get; set; }

//        /// <summary>
//        /// 如果选择的是每分钟 则设置的间隔多少分钟 执行一次
//        /// </summary>
//        int EveryMinuters { get; set; }

//        /// <summary>
//        /// 如果选择的是每小时 则设置的间隔多少小时 执行一次
//        /// </summary>
//        int EveryHours { get; set; }

//        /// <summary>
//        /// 如果选择的是每天 则设置的间隔多少天 执行一次
//        /// </summary>
//        int EveryDays { get; set; }

//        /// <summary>
//        /// 如果选择的是每周 则设置的间隔多少周 执行一次
//        /// </summary>
//        int EveryWeeks { get; set; }

//        /// <summary>
//        /// 如果选择的是每月 则设置的间隔多少月 执行一次
//        /// </summary>
//        int EveryMonths { get; set; }

//        /// <summary>
//        /// 如果选择的是按周计划执行 则在执行任务的这周内是周几执行任务 周一是否选择执行任务
//        /// </summary>
//        bool IsCheckedMon { get; set; }

//        /// <summary>
//        /// 如果选择的是按周计划执行 则在执行任务的这周内是周几执行任务 周二是否选择执行任务
//        /// </summary>
//        bool IsCheckedTue { get; set; }

//        /// <summary>
//        /// 如果选择的是按周计划执行 则在执行任务的这周内是周几执行任务 周三是否选择执行任务
//        /// </summary>
//        bool IsCheckedWen { get; set; }

//        /// <summary>
//        /// 如果选择的是按周计划执行 则在执行任务的这周内是周几执行任务 周四是否选择执行任务
//        /// </summary>
//        bool IsCheckedThr { get; set; }

//        /// <summary>
//        /// 如果选择的是按周计划执行 则在执行任务的这周内是周几执行任务 周五是否选择执行任务
//        /// </summary>
//        bool IsCheckedFri { get; set; }

//        /// <summary>
//        /// 如果选择的是按周计划执行 则在执行任务的这周内是周几执行任务 周六是否选择执行任务
//        /// </summary>
//        bool IsCheckedSta { get; set; }

//        /// <summary>
//        /// 如果选择的是按周计划执行 则在执行任务的这周内是周几执行任务 周日是否选择执行任务
//        /// </summary>
//        bool IsCheckedSun { get; set; }

//        /// <summary>
//        /// 如果选择的是按月执行任务 则执行月的第几日执行任务
//        /// </summary>
//        int EveryMonthDate { get; set; }

//        /// <summary>
//        /// 如果选择的是安年执行任务 则每年的第几月执行
//        /// </summary>
//        int EveryYearMonth { get; set; }

//        /// <summary>
//        /// 如果选择的是安年执行任务 则每年执行任务的月份的第几日执行
//        /// </summary>
//        int EveryYearMonthDate { get; set; }

//        /// <summary>
//        /// 执行任务的起始日期
//        /// </summary>
//        DateTime StartDate { get; set; }

//        /// <summary>
//        /// 执行任务的起始时间 时
//        /// </summary>
//        int StartHour { get; set; }

//        /// <summary>
//        /// 执行任务的起始时间 分
//        /// </summary>
//        int StartMinutes { get; set; }

//        /// <summary>
//        /// 是否设置任务永不过时
//        /// </summary>
//        bool IsCheckNoEndDate { get; set; }

//        /// <summary>
//        /// 是否设置任务次数为有限次数
//        /// </summary>
//        bool IsCheckRepeatTime { get; set; }

//        /// <summary>
//        /// 如果设置任务为有效次数 则具体需要执行任务的次数
//        /// </summary>
//        int RepeatTimes { get; set; }
//    }
//}
