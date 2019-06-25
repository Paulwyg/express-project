
using Wlst.client;

namespace Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuInfoViewModel.ViewModel
{
    public class SwitchOutInfoViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        public SwitchOutInfoViewModel (Wlst .client .ZhaoCeInfo .RtuZhaoRtuPara .RtuZhaoCeSwitchOutInfo    switchOutInfo )
        {
            this.SwitchOutId = switchOutInfo.SwitchOutId;
            this.SwitchOutVerctor = switchOutInfo.SwitchOutVerctor;
            this.KOpenCloseTime = switchOutInfo.KOpenCloseTime;
            this.KCount = switchOutInfo.KCount;
        }

        private int _SwitchOutId;

        /// <summary>
        /// 开关量输出地址序号 1~6
        /// </summary>
        public int SwitchOutId
        {
            get { return _SwitchOutId; }
            set
            {
                if (_SwitchOutId != value)
                {
                    _SwitchOutId = value;
                    this.RaisePropertyChanged(() => this.SwitchOutId);
                }
            }
        }

        private string _KOpenCloseTime;

        /// <summary>
        /// K在召测时间天的开关灯时间;时分，hhmm-hhmm
        /// </summary>
        public string KOpenCloseTime
        {
            get { return _KOpenCloseTime; }
            set
            {
                if (_KOpenCloseTime != value)
                {
                    if(value .Length >8)
                    {
                       value = value.Insert(2, ":");
                        value = value.Insert(8, ":");
                    }
                    _KOpenCloseTime = value;
                    this.RaisePropertyChanged(() => this.KOpenCloseTime);
                }
            }
        }

        private int _KCount;

        /// <summary>
        /// K路数
        /// </summary>
        public int KCount
        {
            get { return _KCount; }
            set
            {
                if (_KCount != value)
                {
                    _KCount = value;
                    this.RaisePropertyChanged(() => this.KCount);
                }
            }
        }

        private int _SwitchOutVerctor;

        /// <summary>
        /// 开关量输出矢量
        /// </summary>
        public int SwitchOutVerctor
        {
            get { return _SwitchOutVerctor; }
            set
            {
                if (_SwitchOutVerctor != value)
                {
                    _SwitchOutVerctor = value;
                    this.RaisePropertyChanged(() => this.SwitchOutVerctor);
                }
            }
        }
    }
}
