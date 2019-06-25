using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationPartsViewModel
{
    /// <summary>
    /// 回路数据
    /// </summary>
    public class RtuParaAnalogueAmpViewModel : ObservableObject, IIRtuParaAnalogueAmpLoop
    {


        public RtuParaAnalogueAmpViewModel(RtuParaAnalogueAmps.RtuParaAnalogueAmp rtuParaAnalogueAmp)
        {
            this.RtuId = rtuParaAnalogueAmp.RtuId;
            this.LoopId = rtuParaAnalogueAmp.LoopId;
            this.LoopName = rtuParaAnalogueAmp.LoopName;
            this.Range = rtuParaAnalogueAmp.Range;
            this.UpperLimit = rtuParaAnalogueAmp.UpperLimit;
            this.LowerLimit = rtuParaAnalogueAmp.LowerLimit;
            this.VectorMoniliang = rtuParaAnalogueAmp.VectorMoniliang ;
            this.Phase = rtuParaAnalogueAmp.Phase;
            this.LightRate = rtuParaAnalogueAmp.LightRate;
            this.LightRateLowerLimit = rtuParaAnalogueAmp.LightRateLowerLimit;
            this.VectorSwitchIn = rtuParaAnalogueAmp.VectorSwitchIn;
            this.SwitchOutId = rtuParaAnalogueAmp.SwitchOutId;
            this.IsAlarmSwitch = rtuParaAnalogueAmp.IsAlarmSwitch;


        }

        /// <summary>
        /// 初始化的时候参数均未赋值；等待继承类实现
        /// </summary>
        public RtuParaAnalogueAmpViewModel()
        {

        }


        /// <summary>
        /// 还原数据为 RtuParaAnalogueAmp
        /// </summary>
        /// <returns></returns>
        public RtuParaAnalogueAmps.RtuParaAnalogueAmp BackRtuParaAnalogueAmp()
        {
            var rtuParaAnalogueAmp = new RtuParaAnalogueAmps.RtuParaAnalogueAmp(this);
            return rtuParaAnalogueAmp;
        }



        protected  int _rtuId;

        /// <summary>
        /// 终端序号
        /// </summary>
        public virtual int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId == value) return;
                _rtuId = value;

                this.RaisePropertyChanged(() => this.RtuId);
            }
        }

        protected int _loopId;

        /// <summary>
        /// 回路序号
        /// </summary>
        public virtual int LoopId
        {
            get { return _loopId; }
            set
            {
                if (_loopId == value) return;
                _loopId = value;

                this.RaisePropertyChanged(() => this.LoopId);
            }
        }

        protected string _loopName;

        /// <summary>
        /// 回路名称
        /// </summary>
        public virtual string LoopName
        {
            get { return _loopName; }
            set
            {
                if (_loopName == value) return;
                _loopName = value;

                this.RaisePropertyChanged(() => this.LoopName);
            }
        }

        protected int _range;

        /// <summary>
        /// 量程/互感器比值
        /// </summary>
        public virtual int Range
        {
            get { return _range; }
            set
            {
                if (_range == value) return;
                _range = value;

                this.RaisePropertyChanged(() => this.Range);
            }
        }

        protected int _AmRange;

        /// <summary>
        /// 电流量程
        /// </summary>
        public virtual int AmRange
        {
            get { return _AmRange; }
            set
            {
                if (_AmRange == value) return;
                _AmRange = value;

                this.RaisePropertyChanged(() => this.AmRange);
            }
        }


        protected int _upperLimit;

        /// <summary>
        /// 报警上限
        /// </summary>
        public virtual int UpperLimit
        {
            get { return _upperLimit; }
            set
            {
                if (_upperLimit == value) return;
                _upperLimit = value;

                this.RaisePropertyChanged(() => this.UpperLimit);
            }
        }

        protected int _lowerLimit;

        /// <summary>
        /// 报警下限
        /// </summary>
        public virtual int LowerLimit
        {
            get { return _lowerLimit; }
            set
            {
                if (_lowerLimit == value) return;
                _lowerLimit = value;

                this.RaisePropertyChanged(() => this.LowerLimit);
            }
        }

        protected int _vector;

        /// <summary>
        /// 口矢参数   采样口矢参数
        /// </summary>
        public virtual int VectorMoniliang
        {
            get { return _vector; }
            set
            {
                if (_vector == value) return;
                _vector = value;

                this.RaisePropertyChanged(() => this.VectorMoniliang);
            }
        }

        protected int _phase;

        /// <summary>
        /// 电压相位
        /// </summary>
        public virtual int Phase
        {
            get { return _phase; }
            set
            {
                if (_phase == value) return;
                _phase = value;
                this.RaisePropertyChanged(() => this.Phase);
            }
        }



        protected double  _lightRate;

        /// <summary>
        /// 亮灯率计算
        /// </summary>
        public virtual double  LightRate
        {
            get { return _lightRate; }
            set
            {
                if (_lightRate == value) return;
                _lightRate = value;

                this.RaisePropertyChanged(() => this.LightRate);
            }
        }

        protected int _lightRateLowerLimit;

        /// <summary>
        /// 亮灯率报警下限
        /// </summary>
        public virtual int LightRateLowerLimit
        {
            get { return _lightRateLowerLimit; }
            set
            {
                if (_lightRateLowerLimit == value) return;
                _lightRateLowerLimit = value;

                this.RaisePropertyChanged(() => this.LightRateLowerLimit);
            }
        }

        protected int _switchInId;

        /// <summary>
        /// 关联开关量输入信号
        /// </summary>
        public virtual int VectorSwitchIn
        {
            get { return _switchInId; }
            set
            {
                if (_switchInId == value) return;
                _switchInId = value;
                this.RaisePropertyChanged(() => this.VectorSwitchIn);
            }
        }






        protected int _switchOutId;

        /// <summary>
        /// 关联开关量输出信号
        /// </summary>
        public virtual int SwitchOutId
        {
            get { return _switchOutId; }
            set
            {
                if (_switchOutId == value) return;
                _switchOutId = value;
                this.RaisePropertyChanged(() => this.SwitchOutId);

            }
        }

        protected bool _AlarmSwitch;

        /// <summary>
        /// 跳变报警 开关量跳变报警
        /// </summary>
        public virtual bool IsAlarmSwitch
        {
            get { return _AlarmSwitch; }
            set
            {
                if (_AlarmSwitch == value) return;
                _AlarmSwitch = value;
                this.RaisePropertyChanged(() => this.IsAlarmSwitch);

            }
        }
        
             protected bool _IsSwitchStateClose;

        /// <summary>
        /// 跳变报警 开关量跳变报警
        /// </summary>
        public virtual bool IsSwitchStateClose
        {
            get { return _IsSwitchStateClose; }
            set
            {
                if (_IsSwitchStateClose == value) return;
                _IsSwitchStateClose = value;
                this.RaisePropertyChanged(() => this.IsSwitchStateClose);

            }
        }
        ///// <summary>
        ///// 口矢参数  开关量口失参数
        ///// </summary>
        //public int  VectorSwitchIn { get; set; }
    };
}
