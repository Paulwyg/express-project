using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Lurx.Controls.EventSchedule;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Sr.ProtocolCnt.EventInstances;
using Wlst.Sr.ProtocolCnt.EventInstancesMru;
using Wlst.Sr.ProtocolCnt.EventInstancesPartol;


namespace Wlst.Ux.Wj1090Module.Wj1090LduEventScheduleViewModel.ViewModel
{
    public class LduEventTaskInstanceInfoViewModel : ObservableObject, IIEventSchduleTaskInstance
    {
        public LduEventTaskInstanceInfoViewModel()
        {
            this.EventSchduleId = Wj1090Module.Services.MenuIdAssgin.EventSchduleNavTaskWj1090LduEventScheduleViewId;
            this.EventSchduleInstanceDescription = "No Description";
            this.EventSchduleInstanceId = -1;
            this.EventSchduleInstanceName = "Not Set";
            this.EventSchduleViewId = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewId;
            this.EventSchduleInstanceDetail = "No Detail";
            EventSchedule = new EventSchedule();
            this.CreateTime = DateTime.Now.Ticks ;
            this.CreateUserName = UserInfo.UserLoginInfo.UserName;
            this.NextExcuteTime = 0;
            this.AlreadyExcutedTimes = 0;
            
        }

        public LduEventTaskInstanceInfoViewModel(PartolEventTaskInstanceInfo info)
        {
            this.EventSchduleId = Wj1090Module.Services.MenuIdAssgin.EventSchduleNavTaskWj1090LduEventScheduleViewId ;
            this.EventSchduleInstanceDescription = info.EventSchduleInstanceDescription;
            this.EventSchduleInstanceId = info.EventSchduleInstanceId;
            this.EventSchduleInstanceName = info.EventSchduleInstanceName;
            this.EventSchduleViewId = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewId ;
            this.EventSchduleInstanceDetail = info.EventSchduleInstanceDetail;
            this.EventSchedule = info.EventSchedule;
            this.CreateTime = info.CreateTime;
            this.CreateUserName = info.CreateUserName;

            this.NextExcuteTime = info .NextExcuteTime ;
            this.AlreadyExcutedTimes = info .AlreadyExcutedTimes ;
     
        }

        public void UpdateEventInstanceViewModel(PartolEventTaskInstanceInfo info)
        {
            this.EventSchduleId = Wj1090Module.Services.MenuIdAssgin.EventSchduleNavTaskWj1090LduEventScheduleViewId ;
            this.EventSchduleInstanceDescription = info.EventSchduleInstanceDescription;
            this.EventSchduleInstanceId = info.EventSchduleInstanceId;
            this.EventSchduleInstanceName = info.EventSchduleInstanceName;
            this.EventSchduleViewId = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewId ;
            this.EventSchduleInstanceDetail = info.EventSchduleInstanceDetail;
            this.EventSchedule = info.EventSchedule;
            this.CreateTime = info.CreateTime;
            this.CreateUserName = info.CreateUserName;
            this.NextExcuteTime = info.NextExcuteTime;
            this.AlreadyExcutedTimes = info.AlreadyExcutedTimes;
            //this.ReMruTimes = info.ReMruTimes;
            
        }

        public PartolEventTaskInstanceInfo GetMruEventTaskInstanceInfo()
        {
            PartolEventTaskInstanceInfo info = new PartolEventTaskInstanceInfo();
            info.EventSchduleId = Wj1090Module.Services.MenuIdAssgin.EventSchduleNavTaskWj1090LduEventScheduleViewId ;
            info.EventSchduleInstanceDescription = this.EventSchduleInstanceDescription;
            info.EventSchduleInstanceId = this.EventSchduleInstanceId;
            info.EventSchduleInstanceName = this.EventSchduleInstanceName;
            info.EventSchduleViewId = Wj1090Module.Services.ViewIdAssign.Wj1090LduEventScheduleViewId ;
            info.EventSchduleInstanceDetail = this.EventSchduleInstanceDetail;
            info.EventSchedule = this.EventSchedule;
            info.CreateTime = this.CreateTime;
            info.CreateUserName = this.CreateUserName;
            info.NextExcuteTime = this.NextExcuteTime;
            info.AlreadyExcutedTimes = this.AlreadyExcutedTimes;

            return info;
        }


        public virtual void OnEventScheduleChanged()
        {

            if (EventSchedule == null) return;

            string txtShow = "";
            txtShow += "任务名称:" + this.EventSchduleInstanceName;
            txtShow += "\r\n";
            txtShow += "执行描述:" + this.EventSchduleInstanceDescription;
            txtShow += "\r\n";
            txtShow += "\r\n";


            txtShow += "执行起始时间:" + EventSchedule.StartDate.Year + "-" + EventSchedule.StartDate.Month + "-" +
                       EventSchedule.StartDate.Day + " " +
                       EventSchedule.StartHour + ":" + EventSchedule.StartMinutes;
            txtShow += "\r\n";
            txtShow += "执行次数:" +
                       (EventSchedule.IsCheckNoEndDate
                            ? "永久周期执行"
                            : EventSchedule.RepeatTimes.ToString(CultureInfo.InvariantCulture));
            txtShow += "\r\n";
            txtShow += "执行周期:";

            string str = "";
            if (EventSchedule.IsCheckEveryMinute)
            {
                str += "每 " + EventSchedule.EveryMinuters + "分钟执行一次";
                if (EventSchedule.EveryMinuters < 30 )
                    str += " [建议巡测周期保持至少间隔30分钟执行]";
            }
            else if (EventSchedule.IsCheckEveryHour)
            {
                str += "每 " + EventSchedule.EveryHours + "小时执行一次";
            }
            else if (EventSchedule.IsCheckEveryDay)
            {
                str += "每 " + EventSchedule.EveryDays + "天执行一次";
            }
            else if (EventSchedule.IsCheckEveryWeek)
            {
                str += "每周 ";
                if (EventSchedule.IsCheckedMon) str += "星期一、 ";
                if (EventSchedule.IsCheckedTue) str += "星期二、 ";
                if (EventSchedule.IsCheckedWen) str += "星期三、 ";
                if (EventSchedule.IsCheckedThr) str += "星期四、 ";
                if (EventSchedule.IsCheckedFri) str += "星期五、 ";
                if (EventSchedule.IsCheckedSta) str += "星期六、 ";
                if (EventSchedule.IsCheckedSun) str += "星期日、 ";
                if (str.Length > 4) str = str.Substring(0, str.Length - 1);
                str += " 执行";
            }
            else if (EventSchedule.IsCheckEveryMonth)
            {
                str += "每间隔 " + EventSchedule.EveryMonths + "月并在该月的" + EventSchedule.EveryMonthDate + "日执行一次";
            }
            else if (EventSchedule.IsCheckEveryYear)
            {
                str += "每年 " + EventSchedule.EveryYearMonth + "月" + EventSchedule.EveryYearMonthDate + "日执行一次";
            }
            txtShow += str;
            txtShow += "\r\n";
            this.EventSchduleInstanceDetail = txtShow;
        }

        #region attri

        private EventSchedule _eventSchedule;

        public EventSchedule EventSchedule
        {
            get { return _eventSchedule; }
            set
            {
                if (_eventSchedule != value)
                {
                    _eventSchedule = value;
                    this.OnEventScheduleChanged();
                }
            }
        }

        /// <summary>
        /// 任务大类的唯一标示码  用于本任务实例标示自己的归属类 目前无用
        /// </summary>
        public int EventSchduleId { get; set; }


        /// <summary>
        /// 本任务的唯一识别编码
        /// </summary>
        public int EventSchduleInstanceId { get; set; }

        private string _eventSchduleInstanceName;

        /// <summary>
        /// 任务类名称
        /// </summary>
        public string EventSchduleInstanceName
        {
            get { return _eventSchduleInstanceName; }
            set
            {
                if (value != _eventSchduleInstanceName)
                {
                    _eventSchduleInstanceName = value;
                    this.RaisePropertyChanged(() => this.EventSchduleInstanceName);
                    OnEventScheduleChanged();
                }
            }
        }

        private string _eventSchduleInstanceDescription;

        /// <summary>
        /// 任务描述  包括执行时间 执行操作 对象 等等 
        /// </summary>
        public string EventSchduleInstanceDescription
        {
            get { return _eventSchduleInstanceDescription; }
            set
            {
                if (value != _eventSchduleInstanceDescription)
                {
                    _eventSchduleInstanceDescription = value;
                    this.RaisePropertyChanged(() => this.EventSchduleInstanceDescription);
                    OnEventScheduleChanged();
                }
            }
        }

        public long CreateTime { get; set; }
        public string CreateUserName { get; set; }


   

        /// <summary>
        /// 当前描述的任务 所在的设置界面Id
        /// </summary>
        public int EventSchduleViewId { get; set; }

        private string _eventSchduleInstanceDetail;

        /// <summary>
        /// 本任务的具体执行细节 包括执行时间 执行操作 对象 等等 
        /// </summary>
        public string EventSchduleInstanceDetail
        {
            get { return _eventSchduleInstanceDetail; }
            set
            {
                if (value != _eventSchduleInstanceDetail)
                {
                    _eventSchduleInstanceDetail = value;
                    this.RaisePropertyChanged(() => this.EventSchduleInstanceDetail);
                }
            }
        }

        /// <summary>
        /// 已经执行次数
        /// </summary>
        public int AlreadyExcutedTimes
        {
            get { return _lreadyExcutedTimes; }
            set
            {
                if (value != _lreadyExcutedTimes)
                {
                    _lreadyExcutedTimes = value;
                    this.RaisePropertyChanged(() => this.AlreadyExcutedTimes);
                }
            }
        }


        private int _lreadyExcutedTimes;





        private long _extExcuteTime;

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public long NextExcuteTime
        {
            get { return _extExcuteTime; }
            set
            {
                if (value != _extExcuteTime)
                {
                    _extExcuteTime = value;
                    this.RaisePropertyChanged(() => this.NextExcuteTime);
                }
            }
        }


        private bool _reMruOne;

        /// <summary>
        /// 
        /// </summary>
        public bool ReMruOne
        {
            get { return _reMruOne; }
            set
            {
                if (value != _reMruOne)
                {
                    _reMruOne = value;
                    this.RaisePropertyChanged(() => this.ReMruOne);
                }
            }
        }

        private bool _reMruTwo;

        /// <summary>
        /// 
        /// </summary>
        public bool ReMruTwo
        {
            get { return _reMruTwo; }
            set
            {
                if (value != _reMruTwo)
                {
                    _reMruTwo = value;
                    this.RaisePropertyChanged(() => this.ReMruTwo);
                }
            }
        }

        private bool _reMruThree;

        /// <summary>
        /// 
        /// </summary>
        public bool ReMruThree
        {
            get { return _reMruThree; }
            set
            {
                if (value != _reMruThree)
                {
                    _reMruThree = value;
                    this.RaisePropertyChanged(() => this.ReMruThree);
                }
            }
        }

        private void SetReMru(int times)
        {
            if (times == 2)
            {
                ReMruOne = false;
                ReMruThree = false;
                ReMruTwo = true;
            }
            else if (times == 3)
            {
                ReMruOne = false;
                ReMruThree = true;
                ReMruTwo = false;
            }
            else
            {

                ReMruOne = true;
                ReMruThree = false;
                ReMruTwo = false;
            }
        }
        #endregion
    }
}
