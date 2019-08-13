using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.HolidayTimeSet.ViewModel
{
    public class TimeSchduleItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int _id;

        /// <summary>
        /// 调度方案 编号  终端绑定此地址
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                this.RaisePropertyChanged(() => this.Id);
            }
        }

        private string name;

        /// <summary>
        /// 调度方案名称
        /// </summary>
        [StringLength(30, ErrorMessage = "调度方案名称字符长度小于30")]
        [Required(ErrorMessage = "输入不能为空")]
        public string Name
        {
            get { return name; }
            set
            {
                if (value == name) return;
                name = value;
                this.RaisePropertyChanged(() => this.Name);
            }
        }

        private string description;

        /// <summary>
        /// 调度方案描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set
            {
                if (value == description) return;
                description = value;
                this.RaisePropertyChanged(() => this.Description);
            }
        }

        private int count;

        public int Count
        {
            get { return count; }
            set
            {
                if (value == count) return;
                count = value;
                this.RaisePropertyChanged(() => this.Count);
            }
        }

        /// <summary>
        /// 详细的调度列表
        /// </summary>
        private ObservableCollection<TimeSchduleItemItme> _Schdules;

        public ObservableCollection<TimeSchduleItemItme> Schdules
        {
            get
            {
                if (_Schdules == null)
                {
                    _Schdules = new ObservableCollection<TimeSchduleItemItme>();
                }
                return _Schdules;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public TimeSchduleItem()
        {
            Id = 0;
            Name = "方案名称";
            Description = "方案描述";
        }

        public TimeSchduleItem( Wlst.client .HolidayWeekSetInfo.HolidaySchduleTime   timeInfo)
        {
            this.Id = timeInfo.Id;
            this.Name = timeInfo.Name;
            this.Description = timeInfo.Description;
            foreach (var t in timeInfo.Schdules)
            {
                this.Schdules.Add(new TimeSchduleItemItme(t));
            }
        }

        public Wlst.client.HolidayWeekSetInfo.HolidaySchduleTime BackToSchdule()
        {
            var info = new Wlst.client.HolidayWeekSetInfo.HolidaySchduleTime()
                           {
                               Description = this.Description,
                               Id = this.Id,
                               Name = this.Name,
                               Schdules = new List<HolidaySchduleTimeItem>()
                           };
            foreach (var t in this.Schdules) info.Schdules.Add(t.BackToSchdule());
            return info;

        }

    }


    public class TimeSchduleItemItme : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public TimeSchduleItemItme()
        {
            Id = 0;
            MonthStart = 1;
            MonthEnd = 1;
            DayEnd = 1;
            DayStart = 1;
            
            K1HourEnd = 25;
            K1HourStart   = 25;
            K2HourEnd = 25;
            K2HourStart = 25;
            K3HourEnd = 25;
            K3HourStart = 25;
            K4HourEnd = 25;
            K4HourStart = 25;
            K5HourEnd = 25;
            K5HourStart = 25;
            K6HourEnd = 25;
            K6HourStart = 25;
            K7HourEnd = 25;
            K7HourStart = 25;
            K8HourEnd = 25;
            K8HourStart = 25;
        }

        public TimeSchduleItemItme(Wlst.client .HolidaySchduleTimeItem  info)
        {
            Id = info.Id;
            MonthEnd = info.MonthEnd;
            MonthStart = info.MonthStart;
            this.DayEnd = info.DayEnd;
            this.DayStart = info.DayStart;

            var idc = new Dictionary<int,client .HolidaySchduleTimeItem .HolidaySchduleTimeItemOneOutTime >();
            foreach (var f in info .SwitchOutTimeItem )
                if(!idc .ContainsKey( f.Kx )) idc.Add(f.Kx, f);

            foreach (var f in info.SwitchOutTimeItem)
            {
                int hourstart = f.OpenTime/60;
                int minutestart = f.OpenTime%60;

                int hourend = f.CloseTime/60;
                int minuteend = f.CloseTime%60;

                if (f.Kx == 1)
                {
                    this.K1HourEnd = hourend;
                    this.K1HourStart = hourstart;
                    this.K1MinutEnd = minuteend;
                    this.K1MinutStart = minutestart;
                }
                if (f.Kx == 2)
                {
                    this.K2HourEnd = hourend;
                    this.K2HourStart = hourstart;
                    this.K2MinutEnd = minuteend;
                    this.K2MinutStart = minutestart;
                }
                if (f.Kx == 3)
                {
                    this.K3HourEnd = hourend;
                    this.K3HourStart = hourstart;
                    this.K3MinutEnd = minuteend;
                    this.K3MinutStart = minutestart;
                }
                if (f.Kx == 4)
                {
                    this.K4HourEnd = hourend;
                    this.K4HourStart = hourstart;
                    this.K4MinutEnd = minuteend;
                    this.K4MinutStart = minutestart;
                }
                if (f.Kx == 5)
                {
                    this.K5HourEnd = hourend;
                    this.K5HourStart = hourstart;
                    this.K5MinutEnd = minuteend;
                    this.K5MinutStart = minutestart;
                }
                if (f.Kx == 6)
                {
                    this.K6HourEnd = hourend;
                    this.K6HourStart = hourstart;
                    this.K6MinutEnd = minuteend;
                    this.K6MinutStart = minutestart;
                }
                if (f.Kx == 7)
                {
                    this.K7HourEnd = hourend;
                    this.K7HourStart = hourstart;
                    this.K7MinutEnd = minuteend;
                    this.K7MinutStart = minutestart;
                }
                if (f.Kx == 8)
                {
                    this.K8HourEnd = hourend;
                    this.K8HourStart = hourstart;
                    this.K8MinutEnd = minuteend;
                    this.K8MinutStart = minutestart;
                }
            }

        }

        public Wlst.client .HolidaySchduleTimeItem   BackToSchdule()
        {
            var rtn= new Wlst.client.HolidaySchduleTimeItem()
                       {
                           DayEnd = this.DayEnd,
                           DayStart = this.DayStart,
                           Id = this.Id,
                           MonthEnd = this.MonthEnd,
                           MonthStart = this.MonthStart,

                           //K1HourEnd = this.K1HourEnd,
                           //K1HourStart = this.K1HourStart,
                           //K1MinuteEnd = this.K1MinutEnd,
                           //K1MinuteStart = this.K1MinutStart,

                           //K2HourEnd = this.K2HourEnd,
                           //K2HourStart = this.K2HourStart,
                           //K2MinuteEnd = this.K2MinutEnd,
                           //K2MinuteStart = this.K2MinutStart,

                           //K3HourEnd = this.K3HourEnd,
                           //K3HourStart = this.K3HourStart,
                           //K3MinuteEnd = this.K3MinutEnd,
                           //K3MinuteStart = this.K3MinutStart,

                           //K4HourEnd = this.K4HourEnd,
                           //K4HourStart = this.K4HourStart,
                           //K4MinuteEnd = this.K4MinutEnd,
                           //K4MinuteStart = this.K4MinutStart,

                           //K5HourEnd = this.K5HourEnd,
                           //K5HourStart = this.K5HourStart,
                           //K5MinuteEnd = this.K5MinutEnd,
                           //K5MinuteStart = this.K5MinutStart,

                           //K6HourEnd = this.K6HourEnd,
                           //K6HourStart = this.K6HourStart,
                           //K6MinuteEnd = this.K6MinutEnd,
                           //K6MinuteStart = this.K6MinutStart,

                       };

            rtn.SwitchOutTimeItem.Add(new HolidaySchduleTimeItem.HolidaySchduleTimeItemOneOutTime()
                                          {
                                              CloseTime = K1HourEnd*60 + K1MinutEnd,
                                              OpenTime = K1HourStart*60 + K1MinutStart,
                                              Kx = 1
                                          });
            rtn.SwitchOutTimeItem.Add(new HolidaySchduleTimeItem.HolidaySchduleTimeItemOneOutTime()
            {
                CloseTime = K3HourEnd * 60 + K3MinutEnd,
                OpenTime = K3HourStart * 60 + K3MinutStart,
                Kx = 3
            });
            rtn.SwitchOutTimeItem.Add(new HolidaySchduleTimeItem.HolidaySchduleTimeItemOneOutTime()
            {
                CloseTime = K2HourEnd * 60 + K2MinutEnd,
                OpenTime = K2HourStart * 60 + K2MinutStart,
                Kx = 2
            });
            rtn.SwitchOutTimeItem.Add(new HolidaySchduleTimeItem.HolidaySchduleTimeItemOneOutTime()
            {
                CloseTime = K4HourEnd * 60 + K4MinutEnd,
                OpenTime = K4HourStart * 60 + K4MinutStart,
                Kx = 4
            });
            rtn.SwitchOutTimeItem.Add(new HolidaySchduleTimeItem.HolidaySchduleTimeItemOneOutTime()
            {
                CloseTime = K5HourEnd * 60 + K5MinutEnd,
                OpenTime = K5HourStart * 60 + K5MinutStart,
                Kx =5
            });
            rtn.SwitchOutTimeItem.Add(new HolidaySchduleTimeItem.HolidaySchduleTimeItemOneOutTime()
            {
                CloseTime = K6HourEnd * 60 + K6MinutEnd,
                OpenTime = K6HourStart * 60 + K6MinutStart,
                Kx = 6
            });
            rtn.SwitchOutTimeItem.Add(new HolidaySchduleTimeItem.HolidaySchduleTimeItemOneOutTime()
            {
                CloseTime = K7HourEnd * 60 + K7MinutEnd,
                OpenTime = K7HourStart * 60 + K7MinutStart,
                Kx = 7
            });
            rtn.SwitchOutTimeItem.Add(new HolidaySchduleTimeItem.HolidaySchduleTimeItemOneOutTime()
            {
                CloseTime = K8HourEnd * 60 + K8MinutEnd,
                OpenTime = K8HourStart * 60 + K8MinutStart,
                Kx = 8
            });
            return rtn;
        }

        private int _id;

        /// <summary>
        /// 序号
        /// </summary>
        public int Id
        {
            get { return _id; }
            set
            {
                if (value == _id) return;
                _id = value;
                this.RaisePropertyChanged(() => this.Id);
            }
        }

        private int _monthStart;

        /// <summary>
        /// 调度起始月份
        /// </summary>
        public int MonthStart
        {
            get { return _monthStart; }
            set
            {
                if (value < 0) value = 0;
                if (value > 12) value = 12;
                if (value == _monthStart) return;
                _monthStart = value;
                this.RaisePropertyChanged(() => this.MonthStart);
            }
        }

        private int monthEnd;

        /// <summary>
        /// 调度结束月份
        /// </summary>
        public int MonthEnd
        {
            get { return monthEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value > 12) value = 12;
                if (value == monthEnd) return;
                monthEnd = value;
                this.RaisePropertyChanged(() => this.MonthEnd);
            }
        }

        private int dayStart;

        /// <summary>
        /// 调度起始日期
        /// </summary>
        public int DayStart
        {
            get { return dayStart; }
            set
            {
                if (value < 1) value = 1;
                if (MonthStart == 1 || MonthStart == 3 || MonthStart == 5 || MonthStart == 7 || MonthStart == 8 || MonthStart == 10 || MonthStart == 12)
                {
                    if (value > 31)
                        value = 31;
                }
                else if (MonthStart == 2)
                {
                    if (value > 29)
                        value = 29;
                }
                else
                {
                    if (value > 30)
                        value = 30;
                }
                if (value == dayStart) return;
                dayStart = value;
                this.RaisePropertyChanged(() => this.DayStart);
            }
        }

        private int dayEnd;

        /// <summary>
        /// 调度结束日期
        /// </summary>
        public int DayEnd
        {
            get { return dayEnd; }
            set
            {
                if (value < 1) value = 1;
                if (MonthEnd == 1 || MonthEnd == 3 || MonthEnd == 5 || MonthEnd == 7 || MonthEnd == 8 || MonthEnd == 10 || MonthEnd == 12)
                {
                    if (value > 31)
                        value = 31;
                }
                else if (MonthEnd == 2)
                {
                    if (value > 29)
                        value = 29;
                }
                else
                {
                    if (value > 30)
                        value = 30;
                }

                if (value == dayEnd) return;
                dayEnd = value;
                this.RaisePropertyChanged(() => this.DayEnd);
            }
        }

        private int _areaid;

        /// <summary>
        /// 区域
        /// </summary>
        public int AreaId
        {
            get { return _areaid; }
            set
            {
                if (value == _areaid) return;
                _id = value;
                this.RaisePropertyChanged(() => this.AreaId);
            }
        }


        private int k1HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public int K1HourStart
        {
            get { return k1HourStart; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k1HourStart) return;
                k1HourStart = value;
                this.RaisePropertyChanged(() => this.K1HourStart);
            }
        }

        private int k1HourEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束小时
        /// </summary>
        public int K1HourEnd
        {
            get { return k1HourEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k1HourEnd) return;
                k1HourEnd = value;
                this.RaisePropertyChanged(() => this.K1HourEnd);
            }
        }

        private int k1MinutStart;

        /// <summary>
        /// 调度时间段内每天开灯的开始开灯分钟
        /// </summary>
        public int K1MinutStart
        {
            get { return k1MinutStart; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K1HourStart == 24 || K1HourStart == 25) value = 0;
                if (value == k1MinutStart) return;
                k1MinutStart = value;
                this.RaisePropertyChanged(() => this.K1MinutStart);
            }
        }

        private int k1MinutEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束分钟
        /// </summary>
        public int K1MinutEnd
        {
            get { return k1MinutEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K1HourEnd == 24 || K1HourEnd == 25) value = 0;
                if (value == k1MinutEnd) return;

                k1MinutEnd = value;
                this.RaisePropertyChanged(() => this.K1MinutEnd);
            }
        }



        private int k2HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public int K2HourStart
        {
            get { return k2HourStart; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k2HourStart) return;
                k2HourStart = value;
                this.RaisePropertyChanged(() => this.K2HourStart);
            }
        }

        private int k2HourEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束小时
        /// </summary>
        public int K2HourEnd
        {
            get { return k2HourEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k2HourEnd) return;
                k2HourEnd = value;
                this.RaisePropertyChanged(() => this.K2HourEnd);
            }
        }

        private int k2MinutStart;

        /// <summary>
        /// 调度时间段内每天开灯的开始开灯分钟
        /// </summary>
        public int K2MinutStart
        {
            get { return k2MinutStart; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K2HourStart == 24 || K2HourStart == 25) value = 0;
                if (value == k2MinutStart) return;
                
                k2MinutStart = value;
                this.RaisePropertyChanged(() => this.K2MinutStart);
            }
        }

        private int k2MinutEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束分钟
        /// </summary>
        public int K2MinutEnd
        {
            get { return k2MinutEnd; }
            set
            {
                if (value < 0) value =0;
                if (value > 59) value = 59;
                if (K2HourEnd == 24 || K2HourEnd == 25) value = 0;
                if (value == k2MinutEnd) return;

                k2MinutEnd = value;
                this.RaisePropertyChanged(() => this.K2MinutEnd);
            }
        }


        private int k3HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public int K3HourStart
        {
            get { return k3HourStart; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k3HourStart) return;
                k3HourStart = value;
                this.RaisePropertyChanged(() => this.K3HourStart);
            }
        }

        private int k3HourEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束小时
        /// </summary>
        public int K3HourEnd
        {
            get { return k3HourEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k3HourEnd) return;
                k3HourEnd = value;
                this.RaisePropertyChanged(() => this.K3HourEnd);
            }
        }

        private int k3MinutStart;

        /// <summary>
        /// 调度时间段内每天开灯的开始开灯分钟
        /// </summary>
        public int K3MinutStart
        {
            get { return k3MinutStart; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K3HourStart == 24 || K3HourStart == 25) value = 0;
                if (value == k3MinutStart) return;
                k3MinutStart = value;
                this.RaisePropertyChanged(() => this.K3MinutStart);
            }
        }

        private int k3MinutEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束分钟
        /// </summary>
        public int K3MinutEnd
        {
            get { return k3MinutEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K3HourEnd == 24 || K3HourEnd == 25) value = 0;
                if (value == k3MinutEnd) return;

                k3MinutEnd = value;
                this.RaisePropertyChanged(() => this.K3MinutEnd);
            }
        }



        private int k4HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public int K4HourStart
        {
            get { return k4HourStart; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k4HourStart) return;
                k4HourStart = value;
                this.RaisePropertyChanged(() => this.K4HourStart);
            }
        }

        private int k4HourEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束小时
        /// </summary>
        public int K4HourEnd
        {
            get { return k4HourEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k4HourEnd) return;
                k4HourEnd = value;
                this.RaisePropertyChanged(() => this.K4HourEnd);
            }
        }

        private int k4MinutStart;

        /// <summary>
        /// 调度时间段内每天开灯的开始开灯分钟
        /// </summary>
        public int K4MinutStart
        {
            get { return k4MinutStart; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K4HourStart == 24 || K4HourStart == 25) value = 0;
                if (value == k4MinutStart) return;
                k4MinutStart = value;
                this.RaisePropertyChanged(() => this.K4MinutStart);
            }
        }

        private int k4MinutEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束分钟
        /// </summary>
        public int K4MinutEnd
        {
            get { return k4MinutEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K4HourEnd == 24 || K4HourEnd == 25) value = 0;
                if (value == k4MinutEnd) return;

                k4MinutEnd = value;
                this.RaisePropertyChanged(() => this.K4MinutEnd);
            }
        }


        private int k5HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public int K5HourStart
        {
            get { return k5HourStart; }
            set
            {
                if (value < 0) value =0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k5HourStart) return;
                k5HourStart = value;
                this.RaisePropertyChanged(() => this.K5HourStart);
            }
        }

        private int k5HourEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束小时
        /// </summary>
        public int K5HourEnd
        {
            get { return k5HourEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k5HourEnd) return;
                k5HourEnd = value;
                this.RaisePropertyChanged(() => this.K5HourEnd);
            }
        }

        private int k5MinutStart;

        /// <summary>
        /// 调度时间段内每天开灯的开始开灯分钟
        /// </summary>
        public int K5MinutStart
        {
            get { return k5MinutStart; }
            set
            {
                if (value < 0) value =0;
                if (value > 59) value = 59;
                if (K5HourStart == 24 || K5HourStart == 25) value = 0;
                if (value == k5MinutStart) return;
                k5MinutStart = value;
                this.RaisePropertyChanged(() => this.K5MinutStart);
            }
        }

        private int k5MinutEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束分钟
        /// </summary>
        public int K5MinutEnd
        {
            get { return k5MinutEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K5HourEnd == 24 || K5HourEnd == 25) value = 0;
                if (value == k5MinutEnd) return;

                k5MinutEnd = value;
                this.RaisePropertyChanged(() => this.K5MinutEnd);
            }
        }


        private int k6HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public int K6HourStart
        {
            get { return k6HourStart; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k6HourStart) return;
                k6HourStart = value;
                this.RaisePropertyChanged(() => this.K6HourStart);
            }
        }

        private int k6HourEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束小时
        /// </summary>
        public int K6HourEnd
        {
            get { return k6HourEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k6HourEnd) return;
                k6HourEnd = value;
                this.RaisePropertyChanged(() => this.K6HourEnd);
            }
        }

        private int k6MinutStart;

        /// <summary>
        /// 调度时间段内每天开灯的开始开灯分钟
        /// </summary>
        public int K6MinutStart
        {
            get { return k6MinutStart; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K6HourStart == 24 || K6HourStart == 25) value = 0;
                if (value == k6MinutStart) return;
                k6MinutStart = value;
                this.RaisePropertyChanged(() => this.K6MinutStart);
            }
        }

        private int k6MinutEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束分钟
        /// </summary>
        public int K6MinutEnd
        {
            get { return k6MinutEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K6HourEnd == 24 || K6HourEnd == 25) value = 0;
                if (value == k6MinutEnd) return;

                k6MinutEnd = value;
                this.RaisePropertyChanged(() => this.K6MinutEnd);
            }
        }

        private int k7HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public int K7HourStart
        {
            get { return k7HourStart; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k7HourStart) return;
                k7HourStart = value;
                this.RaisePropertyChanged(() => this.K7HourStart);
            }
        }

        private int k7HourEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束小时
        /// </summary>
        public int K7HourEnd
        {
            get { return k7HourEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k7HourEnd) return;
                k7HourEnd = value;
                this.RaisePropertyChanged(() => this.K7HourEnd);
            }
        }

        private int k7MinutStart;

        /// <summary>
        /// 调度时间段内每天开灯的开始开灯分钟
        /// </summary>
        public int K7MinutStart
        {
            get { return k7MinutStart; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K7HourStart == 24 || K7HourStart == 25) value = 0;
                if (value == k7MinutStart) return;
                k7MinutStart = value;
                this.RaisePropertyChanged(() => this.K7MinutStart);
            }
        }

        private int k7MinutEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束分钟
        /// </summary>
        public int K7MinutEnd
        {
            get { return k7MinutEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value > 59) value = 59;
                if (K7HourEnd == 24 || K7HourEnd == 25) value = 0;
                if (value == k7MinutEnd) return;

                k7MinutEnd = value;
                this.RaisePropertyChanged(() => this.K7MinutEnd);
            }
        }

        private int k8HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public int K8HourStart
        {
            get { return k8HourStart; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k8HourStart) return;
                k8HourStart = value;
                this.RaisePropertyChanged(() => this.K8HourStart);
            }
        }

        private int k8HourEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束小时
        /// </summary>
        public int K8HourEnd
        {
            get { return k8HourEnd; }
            set
            {
                if (value < 0) value = 0;
                if (value == 24) value = 25;
                if (value > 25) value = 25;
                if (value == k8HourEnd) return;
                k8HourEnd = value;
                this.RaisePropertyChanged(() => this.K8HourEnd);
            }
        }

        private int k8MinutStart;

        /// <summary>
        /// 调度时间段内每天开灯的开始开灯分钟
        /// </summary>
        public int K8MinutStart
        {
            get { return k8MinutStart; }
            set
            {
                if (value < 0) value =0;
                if (value > 59) value = 59;
                if (K8HourStart == 24 || K8HourStart == 25) value = 0;
                if (value == k8MinutStart) return;
                k8MinutStart = value;
                this.RaisePropertyChanged(() => this.K8MinutStart);
            }
        }

        private int k8MinutEnd;

        /// <summary>
        /// 调度时间段内每天开灯的开始结束分钟
        /// </summary>
        public int K8MinutEnd
        {
            get { return k8MinutEnd; }
            set
            {
                if (value < 0) value =0;
                if (value > 59) value = 59;
                if (K8HourEnd == 24 || K8HourEnd == 25) value = 0;
                if (value == k8MinutEnd) return;

                k8MinutEnd = value;
                this.RaisePropertyChanged(() => this.K8MinutEnd);
            }
        }
    }
}
