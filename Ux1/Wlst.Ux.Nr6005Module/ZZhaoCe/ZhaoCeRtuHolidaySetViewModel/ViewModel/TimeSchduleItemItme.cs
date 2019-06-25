using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuHolidaySetViewModel.ViewModel
{
    public class TimeSchduleItemItme : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public TimeSchduleItemItme()
        {
            Id = 0;
            DateTimeRecevie = DateTime.Now;

        }

        public TimeSchduleItemItme(HolidaySchduleTimeItem info)
        {
            Id = info.Id;
            DateTimeRecevie = DateTime.Now;
            this.DayStart = ToStringTupe(info,0,true );

            this.K1HourStart = ToStringTupe(info, 1,false );

            this.K2HourStart = ToStringTupe(info, 2,false );

            this.K3HourStart = ToStringTupe(info, 3,false );

            this.K4HourStart = ToStringTupe(info, 4,false );

            this.K5HourStart = ToStringTupe(info, 5,false );

            this.K6HourStart = ToStringTupe(info, 6,false );

            this.K7HourStart = ToStringTupe(info, 7, false);

            this.K8HourStart = ToStringTupe(info, 8, false);

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


        private DateTime _datatime;

        /// <summary>
        /// 接收时间  
        /// </summary>
        public DateTime DateTimeRecevie
        {
            get { return _datatime; }
            set
            {
                if (_datatime != value)
                {
                    _datatime = value;
                    this.RaisePropertyChanged(() => this.DateTimeRecevie);
                }
            }
        }

        private string dayStart;
        /// <summary>
        /// 调度其实日期
        /// </summary>
        public string  DayStart
        {
            get { return dayStart; }
            set
            {
                if (value == dayStart) return;
                dayStart = value;
                this.RaisePropertyChanged(() => this.DayStart);
            }
        }



        private string k1HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public string K1HourStart
        {
            get { return k1HourStart; }
            set
            {

                if (value == k1HourStart) return;
                k1HourStart = value;
                this.RaisePropertyChanged(() => this.K1HourStart);
            }
        }




        private string k2HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public string K2HourStart
        {
            get { return k2HourStart; }
            set
            {
               
                if (value == k2HourStart) return;
                k2HourStart = value;
                this.RaisePropertyChanged(() => this.K2HourStart);
            }
        }




        private string k3HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public string K3HourStart
        {
            get { return k3HourStart; }
            set
            {
           
                if (value == k3HourStart) return;
                k3HourStart = value;
                this.RaisePropertyChanged(() => this.K3HourStart);
            }
        }





        private string k4HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public string K4HourStart
        {
            get { return k4HourStart; }
            set
            {
              
                if (value == k4HourStart) return;
                k4HourStart = value;
                this.RaisePropertyChanged(() => this.K4HourStart);
            }
        }




        private string k5HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public string K5HourStart
        {
            get { return k5HourStart; }
            set
            {
                if (value == k5HourStart) return;
                k5HourStart = value;
                this.RaisePropertyChanged(() => this.K5HourStart);
            }
        }




        private string k6HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public string K6HourStart
        {
            get { return k6HourStart; }
            set
            {
                if (value == k6HourStart) return;
                k6HourStart = value;
                this.RaisePropertyChanged(() => this.K6HourStart);
            }
        }

        private string k7HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public string K7HourStart
        {
            get { return k7HourStart; }
            set
            {
                if (value == k7HourStart) return;
                k7HourStart = value;
                this.RaisePropertyChanged(() => this.K7HourStart);
            }
        }
        private string k8HourStart;
        /// <summary>
        /// 调度时间段内每天开灯的开始开灯小时
        /// </summary>
        public string K8HourStart
        {
            get { return k8HourStart; }
            set
            {
                if (value == k8HourStart) return;
                k8HourStart = value;
                this.RaisePropertyChanged(() => this.K8HourStart);
            }
        }

        private static string ToStringTupe(client .HolidaySchduleTimeItem info,int kx, bool dates)
        {
            if(dates )
                return string.Format("{0:D2}", info .MonthStart  ) + "-" + string.Format("{0:D2}", info .DayStart ) + "～" +
                   string.Format("{0:D2}", info .MonthEnd ) + "-" + string.Format("{0:D2}", info .DayEnd );
            else
            {
                var fx = (from t in info.SwitchOutTimeItem where t.Kx == kx select t).ToList();
                if (fx.Count == 0) return "----";
                int x1 = fx[0].OpenTime / 60;
                int x2 = fx[0].OpenTime % 60;

                int x3 = fx[0].CloseTime / 60;
                int x4 = fx[0].CloseTime % 60;
                return string.Format("{0:D2}", x1) + ":" + string.Format("{0:D2}", x2) + "-" +
                  string.Format("{0:D2}", x3) + ":" + string.Format("{0:D2}", x4);
            }
                
        }
    }
}
