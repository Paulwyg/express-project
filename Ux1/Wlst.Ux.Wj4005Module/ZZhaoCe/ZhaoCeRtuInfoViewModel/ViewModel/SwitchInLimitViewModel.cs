using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj4005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel
{
    public class SwitchInLimitViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public SwitchInLimitViewModel(Wlst.client.ZhaoCeInfo.RtuZhaoRtuPara1.SwitchInLimit switchInLimit, int index)
        {
            this.VoltageLowlimit = Convert.ToString(switchInLimit.VoltageLowlimit[index]);
            this.VoltageUplimit = Convert.ToString(switchInLimit.VoltageUplimit[index]);
            this.CurrentLowlimit = Convert.ToString(switchInLimit.CurrentLowlimit[index]);
            this.CurrentUplimit = Convert.ToString(switchInLimit.CurrentUplimit[index]);
        }


        private string _currentLowlimit;

        /// <summary>
        /// 电流下限
        /// </summary>
        public string CurrentLowlimit
        {
            get { return _currentLowlimit; }
            set
            {
                if (_currentLowlimit != value)
                {

                    _currentLowlimit = value;
                    this.RaisePropertyChanged(() => this.CurrentLowlimit);
                }
            }
        }

        private string _currentUplimit;
        /// <summary>
        /// 电流上限
        /// </summary>
        public string CurrentUplimit
        {
            get { return _currentUplimit; }
            set
            {
               if (_currentUplimit != value)
               {
                   _currentUplimit = value;
                   this.RaisePropertyChanged(() => this.CurrentUplimit);
               }
            }
        }

        private string _voltageLowlimit;
        /// <summary>
        /// 电压下限
        /// </summary>
        public string VoltageLowlimit
        {
            get { return _voltageLowlimit; }
            set
            {
                if (_voltageLowlimit != value)
                {
                    _voltageLowlimit = value;
                    this.RaisePropertyChanged(() => this.VoltageLowlimit);
                }
            }
        }

        private string _voltageUplimit;
        /// <summary>
        /// 电压上限
        /// </summary>
        public string VoltageUplimit
        {
            get { return _voltageUplimit; }
            set
            {
                if (_voltageUplimit != value)
                {
                    _voltageUplimit = value;
                    this.RaisePropertyChanged(() => this.VoltageUplimit);
                }
            }
        }
    }
}
