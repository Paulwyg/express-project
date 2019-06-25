using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.TimeTableSystem.TimeTableSetViewModel.ViewModel
{
    public class TmlLoopInfoViewModel : ObservableObject
    {
        private int _loopId;

        /// <summary>
        /// 回路序号
        /// </summary>
        public int LoopId
        {
            get
            {
                return _loopId;
            }
            set
            {
                if (_loopId != value)
                {
                    _loopId = value;
                    this.RaisePropertyChanged(() => this.LoopId);
                }
            }
        }

        private string _loopName;

        /// <summary>
        /// 回路名称
        /// </summary>
        public string LoopName
        {
            get
            {
                return _loopName;
            }
            set
            {
                if (_loopName != value)
                {
                    _loopName = value;
                    this.RaisePropertyChanged(() => this.LoopName);
                }
            }
        }

        private int _timeTableId;

        /// <summary>
        /// 回路归属时间表Id
        /// </summary>
        public int TimeTableId
        {
            get
            {
                return _timeTableId;
            }
            set
            {
                if (_timeTableId != value)
                {
                    _timeTableId = value;
                    this.RaisePropertyChanged(() => this.TimeTableId);
                }
            }
        }

        private string _timeTableName;

        /// <summary>
        /// 回路归属时间表名称
        /// </summary>
        public string TimeTableName
        {
            get
            {
                return _timeTableName;
            }
            set
            {
                if (_timeTableName != value)
                {
                    _timeTableName = value;
                    this.RaisePropertyChanged(() => this.TimeTableName);
                }
            }
        }

        private string _todayOnTime;

        /// <summary>
        /// 今天的开灯时间
        /// </summary>
        public string TodayOnTime
        {
            get
            {
                return _todayOnTime;
            }
            set
            {
                if (_todayOnTime != value)
                {
                    _todayOnTime = value;
                    this.RaisePropertyChanged(() => this.TodayOnTime);
                }
            }
        }

        private string _todayOffTime;

        /// <summary>
        /// 今天关灯时间
        /// </summary>
        public string TodayOffTime
        {
            get
            {
                return _todayOffTime;
            }
            set
            {
                if (_todayOffTime != value)
                {
                    _todayOffTime = value;
                    this.RaisePropertyChanged(() => this.TodayOffTime);
                }
            }
        }

        private bool _isLightOn;

        /// <summary>
        /// 是否开灯使用光控
        /// </summary>
        public bool IsLightOn
        {
            get
            {
                return _isLightOn;
            }
            set
            {
                if (value != _isLightOn)
                {
                    _isLightOn = value;
                    this.RaisePropertyChanged(() => this.IsLightOn);
                }
            }
        }

        private bool _isLightOff;

        /// <summary>
        /// 是否关灯使用光控
        /// </summary>
        public bool IsLightOff
        {
            get
            {
                return _isLightOff;
            }
            set
            {
                if (value != _isLightOff)
                {
                    _isLightOff = value;
                    this.RaisePropertyChanged(() => this.IsLightOff);
                }
            }
        }
    }
}