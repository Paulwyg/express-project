using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationPartsViewModel
{

    /// <summary>
    /// 开关量输出
    /// </summary>
    public class RtuParaSwitchOutViewModel : ObservableObject, IISwitchOutLoop
    {


        public RtuParaSwitchOutViewModel(SwitchOut.RtuParaSwitchOut rtuParaSwitchOut)
        {
            this.RtuId = rtuParaSwitchOut.RtuId;
            this.SwitchOutId = rtuParaSwitchOut.SwitchOutId;
            this.SwichtOutName = rtuParaSwitchOut.SwichtOutName;
            this.ControlGroup = rtuParaSwitchOut.ControlGroup;
            this.Vector = rtuParaSwitchOut.Vector;
            this.LoopSum = rtuParaSwitchOut.LoopSum;

        }
        /// <summary>
        /// 初始化的时候参数均未赋值；等待继承类实现
        /// </summary>
        public RtuParaSwitchOutViewModel()
        {

        }

        /// <summary>
        /// 还原数据为 RtuParaSwitchOut
        /// </summary>
        /// <returns></returns>
        public SwitchOut.RtuParaSwitchOut BackRtuParaSwitchOut()
        {
            var rtuParaSwitchOut = new SwitchOut.RtuParaSwitchOut(this);
            return rtuParaSwitchOut;
        }

        //public event EventHandler SwitchOutLoopSumChanged = delegate { };

        protected int _rtuId;

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

        protected int _switchOutId;

        /// <summary>
        /// 开关量输出序号
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

        protected string _swichtOutName;

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string SwichtOutName
        {
            get { return _swichtOutName; }
            set
            {
                if (_swichtOutName == value) return;
                _swichtOutName = value;

                this.RaisePropertyChanged(() => this.SwichtOutName);
            }
        }

        protected int _controlGroup;

        /// <summary>
        /// 归属于哪个分组控制
        /// </summary>
        public virtual int ControlGroup
        {
            get { return _controlGroup; }
            set
            {
                if (_controlGroup == value) return;
                _controlGroup = value;

                this.RaisePropertyChanged(() => this.ControlGroup);
            }
        }

        protected int _vector;

        /// <summary>
        /// 口矢参数 不允许修改
        /// </summary>
        public virtual int Vector
        {
            get { return _vector; }
            set
            {
                if (_vector == value) return;
                _vector = value;

                this.RaisePropertyChanged(() => this.Vector);
            }
        }

        protected int _loopSum;

        /// <summary>
        /// 回路路数
        /// </summary>
        public virtual int LoopSum
        {
            get { return _loopSum; }
            set
            {
                if (_loopSum == value) return;
                _loopSum = value;
                this.RaisePropertyChanged(() => this.LoopSum);
            }
        }

        protected ObservableCollection<int> _loopsCollection;

        /// <summary>
        /// 绑定到此输出的回路集合
        /// </summary>
        public virtual ObservableCollection<int> LoopsCollection
        {
            get
            {
                if (_loopsCollection == null) _loopsCollection = new ObservableCollection<int>();
                return _loopsCollection;
            }

        }

    };
}
