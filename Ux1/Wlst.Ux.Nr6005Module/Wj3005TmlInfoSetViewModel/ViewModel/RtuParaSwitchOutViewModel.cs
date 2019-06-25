using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreServices;
using Wlst.client;


namespace Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.ViewModel
{
    public class RtuParaSwitchOutViewModelBase : ObservableObject
    {


        public RtuParaSwitchOutViewModelBase(Wlst .client .RtuSwitchOutputParameter  rtuParaSwitchOut)
        {
            this.RtuId = rtuParaSwitchOut.RtuId;
            this.SwitchOutId = rtuParaSwitchOut.SwitchId ;
            this.SwichtOutName = rtuParaSwitchOut.SwitchName;
           // this.ControlGroup = rtuParaSwitchOut.ControlGroup;
            this.Vector = rtuParaSwitchOut.SwitchVecotr ;
           // this.LoopSum = rtuParaSwitchOut.LoopSum;

        }
        /// <summary>
        /// 初始化的时候参数均未赋值；等待继承类实现
        /// </summary>
        public RtuParaSwitchOutViewModelBase()
        {

        }

        /// <summary>
        /// 还原数据为 RtuParaSwitchOut
        /// </summary>
        /// <returns></returns>
        public Wlst .client .RtuSwitchOutputParameter  BackRtuParaSwitchOut()
        {

            return new RtuSwitchOutputParameter()
                       {
                           RtuId = RtuId,
                           SwitchId = this.SwitchOutId,
                           SwitchName = this.SwichtOutName,
                           SwitchVecotr = this.Vector
                       };
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

        //protected int _controlGroup;

        ///// <summary>
        ///// 归属于哪个分组控制
        ///// </summary>
        //public virtual int ControlGroup
        //{
        //    get { return _controlGroup; }
        //    set
        //    {
        //        if (_controlGroup == value) return;
        //        _controlGroup = value;

        //        this.RaisePropertyChanged(() => this.ControlGroup);
        //    }
        //}

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
    /// <summary>
    /// 开关量输出
    /// </summary>
    public class RtuParaSwitchOutViewModel : RtuParaSwitchOutViewModelBase
    {
        public RtuParaSwitchOutViewModel(Wlst .client .RtuSwitchOutputParameter  rtuParaSwitchOut)
        {
            this.RtuId = rtuParaSwitchOut.RtuId;
            this.SwitchOutId = rtuParaSwitchOut.SwitchId ;
            if (rtuParaSwitchOut.SwitchName.Contains("开关量输出"))
            {
                var tmp = rtuParaSwitchOut.SwitchName;
                int a = tmp.IndexOf("K");
                rtuParaSwitchOut.SwitchName =tmp.Substring(a, 2);
            }
            this.SwichtOutName = rtuParaSwitchOut.SwitchName ;
           // this.SwichtOutName = "rtus";
          //  this.ControlGroup = rtuParaSwitchOut.ControlGroup;
            this.Vector = rtuParaSwitchOut.SwitchVecotr ;
            if (this.Vector == 0)
            {
                this.Vector = this.SwitchOutId;
            }
       //     this.LoopSum = rtuParaSwitchOut.LoopSum;
        }

        /// <summary>
        /// 开关量输出序号
        /// </summary>
        public override int SwitchOutId
        {
            get
            {
                return _switchOutId;
            }
            set
            {
                if (_switchOutId == value)
                    return;
                _switchOutId = value;
                
                this.RaisePropertyChanged(() => this.SwitchOutId);
                CalocatePoint();
            }
        }

        public override int Vector
        {
            get
            {
                return base.Vector;
            }
            set
            {
                base.Vector = value;
                this.RaisePropertyChanged(() => this.Vector);
                for (int t = 0; t < CollectionVector.Count; t++)
                {
                    if (CollectionVector[t] != _vector)
                        continue;
                    SelectVectorIndex = t;
                    break;
                }
            }
        }

        #region assist Vector show

        public ObservableCollection<int> _collectionVector;

        //辅助显示
        public ObservableCollection<int> CollectionVector
        {
            get
            {
                if (_collectionVector == null)
                {
                    _collectionVector = new ObservableCollection<int>();
                    for (int i = 1; i < 7; i++)
                    {
                        _collectionVector.Add(i);
                    }
                }
                return _collectionVector;
            }
        }

        private int _SelectVectorIndex;

        //辅助显示
        public int SelectVectorIndex
        {
            get
            {
                return _SelectVectorIndex;
            }
            set
            {
                if (_SelectVectorIndex != value)
                {
                    _SelectVectorIndex = value;

                    this.RaisePropertyChanged(() => this.SelectVectorIndex);
                    Vector = CollectionVector[value];
                }
            }
        }

        #endregion

        #region point

        private void CalocatePoint()
        {
            X1onMap = 20;
            Y1onMap = this.SwitchOutId * 50 + 80;
        }

        int _x1OnMap;

        public int X1onMap
        {
            get
            {
                return _x1OnMap;
            }
            set
            {
                if (_x1OnMap != value)
                {
                    _x1OnMap = value;
                    this.RaisePropertyChanged("X1onMap");
                }
            }
        }

        int _y1OnMap;

        public int Y1onMap
        {
            get
            {
                return _y1OnMap;
            }
            set
            {
                if (_y1OnMap != value)
                {
                    _y1OnMap = value;
                    this.RaisePropertyChanged("Y1onMap");
                }
            }
        }

        public string BackgroundColor
        {
            get
            {
                return "#FFFAEBD7";
            }
        }

        //int _widthControl;
        public int WidthControl
        {
            get
            {
                return 120;
            }
        }

        //int _heightControl;
        public int HeightControl
        {
            get
            {
                return 30;
            }
        }

        public int HeightDes
        {
            get
            {
                return 50;
            }
        }
        
        #endregion

    };
}