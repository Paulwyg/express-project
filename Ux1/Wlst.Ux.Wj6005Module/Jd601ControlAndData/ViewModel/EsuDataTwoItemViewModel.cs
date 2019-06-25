using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.ProtocolCnt.Jd601;
using Wlst.Ux.Wj6005Module.Models;

namespace Wlst.Ux.Wj6005Module.Jd601ControlAndData.ViewModel
{
    public class EsuDataTwoItemViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        #region private attri

        private int _value1;
        private int _value2;
        private int _value3;
        private int _value4;
        private string _value5;
        private string _value6;
        private string _value7;
        private string _value8;
        private string _value9;
        private string _value10;
        private string _value11;
        private string _value12;
        private int _value13;
        private string _value14;
        private string _value15;
        private string _value16;

        #endregion

        #region attri

        /// <summary>
        /// #今天复位次数(1字节)
        /// </summary>
        public int RebootTimesToday
        {
            get { return _value1; }
            set
            {
                if (value != _value1)
                {
                    _value1 = value;
                    this.RaisePropertyChanged(() => this.RebootTimesToday);
                }

            }
        }

        /// <summary>
        /// #昨天复位次数(1字节)
        /// </summary>
        public int RebootTimesYesterday
        {
            get { return _value2; }
            set
            {
                if (value != _value2)
                {
                    _value2 = value;
                    this.RaisePropertyChanged(() => this.RebootTimesYesterday);
                }

            }
        }

        /// <summary>
        /// #前天复位次数(1字节)
        /// </summary>
        public int RebootTimesLastYesterDay
        {
            get { return _value3; }
            set
            {
                if (value != _value3)
                {
                    _value3 = value;
                    this.RaisePropertyChanged(() => this.RebootTimesLastYesterDay);
                }

            }
        }

        /// <summary>
        /// #大前天复位次数(1字节)
        /// </summary>
        public int RebootTimesLastLastYesterDay
        {
            get { return _value4; }
            set
            {
                if (value != _value4)
                {
                    _value4 = value;
                    this.RaisePropertyChanged(() => this.RebootTimesLastLastYesterDay);
                }

            }
        }

        ///// <summary>
        ///// #参数状态(4字节=>%d)
        ///// </summary>
        //public int Args;

        private ObservableCollection<NameIntBool> _args;

        public ObservableCollection<NameIntBool> Args
        {
            get
            {
                if (_args == null)
                {
                    _args = new ObservableCollection<NameIntBool>();
                    for (int i = 0; i < 30; i++)
                    {
                        _args.Add(new NameIntBool()
                                      {
                                          Value = i,
                                          Name = "--",
                                          IsSelected = false
                                      });
                    }
                }
                return _args;
            }
        }


        /// <summary>
        /// #节能模式（1字节）接触器=1，IGBT=0
        /// </summary>
        public string EsuMode
        {
            get { return _value5; }
            set
            {
                if (value != _value5)
                {
                    _value5 = value;
                    this.RaisePropertyChanged(() => this.EsuMode);
                }

            }
        }

        /// <summary>
        /// #当前调压档位值（1字节）
        /// </summary>
        public string CurrentVolLevel
        {
            get { return _value6; }
            set
            {
                if (value != _value6)
                {
                    _value6 = value;
                    this.RaisePropertyChanged(() => this.CurrentVolLevel);
                }

            }
        }

        /// <summary>
        /// #调压等待时间（1字节）
        /// </summary>
        public string CurrentWaiteTimes
        {
            get { return _value7; }
            set
            {
                if (value != _value7)
                {
                    _value7 = value;
                    this.RaisePropertyChanged(() => this.CurrentWaiteTimes);
                }

            }
        }

        /// <summary>
        /// #A相电压调节位置（1字节）
        /// </summary>
        public string CurrentaLocation
        {
            get { return _value8; }
            set
            {
                if (value != _value8)
                {
                    _value8 = value;
                    this.RaisePropertyChanged(() => this.CurrentaLocation);
                }

            }
        }

        /// <summary>
        /// #B相电压调节位置（1字节）
        /// </summary>
        public string CurrentbLocation
        {
            get { return _value9; }
            set
            {
                if (value != _value9)
                {
                    _value9 = value;
                    this.RaisePropertyChanged(() => this.CurrentbLocation);
                }

            }
        }

        /// <summary>
        /// #C相电压调节位置（1字节）
        /// </summary>
        public string CurrentcLocation
        {
            get { return _value10; }
            set
            {
                if (value != _value10)
                {
                    _value10 = value;
                    this.RaisePropertyChanged(() => this.CurrentcLocation);
                }

            }
        }

        /// <summary>
        /// #IGBT状态（1字节）
        /// </summary>
        public string IsIgbt
        {
            get { return _value11; }
            set
            {
                if (value != _value11)
                {
                    _value11 = value;
                    this.RaisePropertyChanged(() => this.IsIgbt);
                }

            }
        }

        /// <summary>
        /// 	#IGBT温度（1字节）
        /// </summary>
        public string IgbtTemperture
        {
            get { return _value12; }
            set
            {
                if (value != _value12)
                {
                    _value12 = value;
                    this.RaisePropertyChanged(() => this.IgbtTemperture);
                }

            }
        }

        /// <summary>
        /// #事件记录序号（1字节）
        /// </summary>
        public int EventLogId
        {
            get { return _value13; }
            set
            {
                if (value != _value13)
                {
                    _value13 = value;
                    this.RaisePropertyChanged(() => this.EventLogId);
                }

            }
        }

        /// <summary>
        /// #开关量输出状态（1字节）
        /// </summary>
        public string SwitchOutState
        {
            get { return _value14; }
            set
            {
                if (value != _value14)
                {
                    _value14 = value;
                    this.RaisePropertyChanged(() => this.SwitchOutState);
                }

            }
        }

        /// <summary>
        /// #开关量输入状态（1字节）
        /// </summary>
        public string SwitchInState
        {
            get { return _value15; }
            set
            {
                if (value != _value15)
                {
                    _value15 = value;
                    this.RaisePropertyChanged(() => this.SwitchInState);
                }

            }
        }

        /// <summary>
        /// #投运/停运状态(1字节)
        /// </summary>
        public string RunStopState
        {
            get { return _value16; }
            set
            {
                if (value != _value16)
                {
                    _value16 = value;
                    this.RaisePropertyChanged(() => this.RunStopState);
                }

            }
        }

        #endregion

        public EsuDataTwoItemViewModel()
        {
            RebootTimesLastLastYesterDay = 0;
            RebootTimesLastYesterDay = 0;
            RebootTimesToday = 0;
            RebootTimesYesterday = 0;
            EsuMode = "--";
            CurrentVolLevel = "--";
            CurrentWaiteTimes = "--";
            CurrentaLocation = "--";
            CurrentbLocation = "--";
            CurrentcLocation = "--";
            IsIgbt = "--";
            IgbtTemperture = "--";
            EventLogId = 0;
            SwitchInState = "--";
            SwitchOutState = "--";
            RunStopState = "--";


            for (int i = 0; i < Args.Count; i++)
            {
                Args[i].IsSelected = false;
            }
        }

        public void ResetAllArgs()
        {
            RebootTimesLastLastYesterDay = 0;
            RebootTimesLastYesterDay = 0;
            RebootTimesToday = 0;
            RebootTimesYesterday = 0;
            EsuMode = "--";
            CurrentVolLevel = "--";
            CurrentWaiteTimes = "--";
            CurrentaLocation = "--";
            CurrentbLocation = "--";
            CurrentcLocation = "--";
            IsIgbt = "--";
            IgbtTemperture = "--";
            EventLogId = 0;
            SwitchInState = "--";
            SwitchOutState = "--";
            RunStopState = "--";


            for (int i = 0; i < Args.Count; i++)
            {
                Args[i].IsSelected = false;
            }
        }

        public EsuDataTwoItemViewModel(Jd601DataTwo info)
        {
            RebootTimesLastLastYesterDay = info.RebootTimesLastLastYesterDay;
            RebootTimesLastYesterDay = info.RebootTimesLastYesterDay;
            RebootTimesToday = info.RebootTimesToday;
            RebootTimesYesterday = info.RebootTimesYesterday;
            EsuMode = info.EsuMode == 1 ? "IGBT" : "接触器模式";
            CurrentVolLevel = "未知";
            if (Jd601DataTwoAdjustVollevel.AdjustVolLevel.ContainsKey(info.CurrentVolLevel))
                CurrentVolLevel = Jd601DataTwoAdjustVollevel.AdjustVolLevel[info.CurrentVolLevel];
            CurrentWaiteTimes = info.CurrentWaiteTimes + " 分";
            CurrentaLocation = info.CurrentaLocation + "";
            CurrentbLocation = info.CurrentbLocation + "";
            CurrentcLocation = info.CurrentcLocation + "";
            IsIgbt = info.IsIgbt ? "开" : "关";
            IgbtTemperture = info.IgbtTemperture + " 度";
            EventLogId = info.EventLogId;
            SwitchInState = info.SwitchInState == 1 ? "吸合" : "断开";
            SwitchOutState = info.SwitchOutState == 1 ? "开" : "关";
            RunStopState = info.RunStopState == 1 ? "投运" : "停运";


            for (int i = 0; i < 29; i++)
            {
                if (Args.Count < i) return;
                if (((info.Args >> i) & 1) == 1) Args[i].IsSelected = true;
                else Args[i].IsSelected = false;
            }
        }



        public void UpdateAllArgs(Jd601DataTwo info)
        {
            RebootTimesLastLastYesterDay = info.RebootTimesLastLastYesterDay;
            RebootTimesLastYesterDay = info.RebootTimesLastYesterDay;
            RebootTimesToday = info.RebootTimesToday;
            RebootTimesYesterday = info.RebootTimesYesterday;
            EsuMode = info.EsuMode == 1 ? "IGBT" : "接触器模式";
            CurrentVolLevel = "未知";
            if (Jd601DataTwoAdjustVollevel.AdjustVolLevel.ContainsKey(info.CurrentVolLevel))
                CurrentVolLevel = Jd601DataTwoAdjustVollevel.AdjustVolLevel[info.CurrentVolLevel];
            CurrentWaiteTimes = info.CurrentWaiteTimes + " 分";
            CurrentaLocation = info.CurrentaLocation + "";
            CurrentbLocation = info.CurrentbLocation + "";
            CurrentcLocation = info.CurrentcLocation + "";
            IsIgbt = info.IsIgbt ? "开" : "关";
            IgbtTemperture = info.IgbtTemperture + " 度";
            EventLogId = info.EventLogId;
            SwitchInState = info.SwitchInState == 1 ? "吸合" : "断开";
            SwitchOutState = info.SwitchOutState == 1 ? "开" : "关";
            RunStopState = info.RunStopState == 1 ? "投运" : "停运";


            for (int i = 0; i < 29; i++)
            {
                if (Args.Count < i) return;
                if (((info.Args >> i) & 1) == 1) Args[i].IsSelected = true;
                else Args[i].IsSelected = false;
            }
        }
    }
}
