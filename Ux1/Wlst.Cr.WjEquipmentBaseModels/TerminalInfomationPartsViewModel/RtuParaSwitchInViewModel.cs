//using Wlst.Cr.Core.CoreServices;
//using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
//using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;

//namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationPartsViewModel
//{
//    /// <summary>
//    /// 开关量输入
//    /// </summary>
//    public class RtuParaSwitchInViewModel : ObservableObject, IISwitchInLoop
//    {
//        /// <summary>
//        /// 初始化的时候参数均未赋值；等待继承类实现
//        /// </summary>
//        public RtuParaSwitchInViewModel()
//        {

//        }

//        public RtuParaSwitchInViewModel(SwitchIn.RtuParaSwitchIn rtuParaSwitchIn)
//        {
//            this.Alarm = rtuParaSwitchIn.Alarm;
//            this.ContactorState = rtuParaSwitchIn.ContactorState;
//            this.UnNormalState = rtuParaSwitchIn.UnNormalState;
//            this.NoramlState = rtuParaSwitchIn.NoramlState;
//            this.RtuId = rtuParaSwitchIn.RtuId;
//            this.SwitchInId = rtuParaSwitchIn.SwitchInId;
//            this.SwichtInName = rtuParaSwitchIn.SwichtInName;
//            this.Vector = rtuParaSwitchIn.Vector;

//        }
//        /// <summary>
//        /// 还原数据为 RtuParaSwitchIn
//        /// </summary>
//        /// <returns></returns>
//        public SwitchIn.RtuParaSwitchIn BackRtuParaSwitchIn()
//        {
//            var rtuParaSwitchIn = new SwitchIn.RtuParaSwitchIn(this);
//            return rtuParaSwitchIn;
//        }

//        protected int _rtuId;

//        /// <summary>
//        /// 终端序号
//        /// </summary>
//        public virtual int RtuId
//        {
//            get { return _rtuId; }
//            set
//            {
//                if (_rtuId == value) return;
//                _rtuId = value;

//                this.RaisePropertyChanged(() => this.RtuId);
//            }
//        }

//        protected int _switchInId;

//        /// <summary>
//        /// 开关量输入序号
//        /// </summary>
//        public virtual int SwitchInId
//        {
//            get { return _switchInId; }
//            set
//            {
//                if (_switchInId == value) return;
//                _switchInId = value;

//                this.RaisePropertyChanged(() => this.SwitchInId);
//            }
//        }

//        protected string _swichtInName;

//        /// <summary>
//        /// 名称
//        /// </summary>
//        public virtual string SwichtInName
//        {
//            get { return _swichtInName; }
//            set
//            {
//                if (_swichtInName == value) return;
//                _swichtInName = value;

//                this.RaisePropertyChanged(() => this.SwichtInName);
//            }
//        }

//        protected byte _contactorState;

//        /// <summary>
//        /// 接触器状态:常开/常闭/无
//        /// </summary>
//        public virtual byte ContactorState
//        {
//            get { return _contactorState; }
//            set
//            {
//                if (_contactorState == value) return;
//                _contactorState = value;
//                this.RaisePropertyChanged(() => ContactorState);
//            }
//        }


//        protected byte _alarm;

//        /// <summary>
//        /// 跳变报警
//        /// </summary>
//        public virtual byte Alarm
//        {
//            get { return _alarm; }
//            set
//            {
//                if (_alarm == value) return;
//                _alarm = value;
//                this.RaisePropertyChanged(() => this.Alarm);

//            }
//        }





//        protected byte _vector;

//        /// <summary>
//        /// 口矢参数开关量输入在版子上第几个  不允许修改
//        /// </summary>
//        public virtual byte Vector
//        {
//            get { return _vector; }
//            set
//            {
//                if (_vector == value) return;
//                _vector = value;
//                this.RaisePropertyChanged(() => this.Vector);
//            }
//        }

//        protected int _onState;

//        /// <summary>
//        /// 状态 通时的显示名称
//        /// </summary>
//        public virtual int NoramlState
//        {
//            get { return _onState; }
//            set
//            {
//                if (_onState == value) return;
//                _onState = value;
//                this.RaisePropertyChanged(() => this.NoramlState);

//            }
//        }




//        protected int _offState;

//        /// <summary>
//        /// 状态 断时的显示名称
//        /// </summary>
//        public virtual int UnNormalState //CollectionContactorStateShow
//        {
//            get { return _offState; }
//            set
//            {
//                if (_offState == value) return;
//                _offState = value;
//                this.RaisePropertyChanged(() => this.UnNormalState);

//            }
//        }

//    };
//}
