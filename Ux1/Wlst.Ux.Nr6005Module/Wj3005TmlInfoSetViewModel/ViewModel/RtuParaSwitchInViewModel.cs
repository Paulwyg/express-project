//using System.Collections.ObjectModel;
//using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;
//using Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.Services;

//namespace Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.ViewModel
//{
//    /// <summary>
//    /// 开关量输入
//    /// </summary>
//    public class RtuParaSwitchInViewModel : Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationPartsViewModel.RtuParaSwitchInViewModel 
//    {
//        public RtuParaSwitchInViewModel() : base()
//        {
//        }

//        public RtuParaSwitchInViewModel(SwitchIn.RtuParaSwitchIn rtuParaSwitchIn) : base()
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
//        /// 开关量输入序号
//        /// </summary>
//        public override int SwitchInId
//        {
//            get
//            {
//                return _switchInId;
//            }
//            set
//            {
//                if (_switchInId == value)
//                    return;
//                _switchInId = value;
                
//                this.RaisePropertyChanged(() => this.SwitchInId);
//                CalocatePoint();
//            }
//        }

//        /// <summary>
//        /// 接触器状态:常开/常闭/无
//        /// </summary>
//        public override byte ContactorState
//        {
//            get
//            {
//                return _contactorState;
//            }
//            set
//            {
//                //if (_ContactorState == value) return;
//                _contactorState = value;
//                for (int t = 0; t < CollectionContractorState.Count; t++)
//                {
//                    if (CollectionContractorState[t].Value != _contactorState)
//                        continue;
//                    SelectContractorStateIndex = t;
//                    break;
//                }
//            }
//        }

//        #region assist ContactorState show
        
//        //辅助显示接触器状态所有选择
//        public ObservableCollection<StateBase> CollectionContractorState
//        {
//            get 
//            { 
//                return Comm.CollectionContactorState;
//            }
//        }
        
//        private int _SelectContractorStateIndex;
        
//        //辅助显示接触器状态当前选择
//        public int SelectContractorStateIndex
//        {
//            get
//            {
//                return _SelectContractorStateIndex;
//            }
//            set
//            {
//                if (_SelectContractorStateIndex != value)
//                {
//                    _SelectContractorStateIndex = value;
                    
//                    this.RaisePropertyChanged(() => this.SelectContractorStateIndex);
//                    ContactorState = (byte)CollectionContractorState[value].Value;
//                }
//            }
//        }
        
//        #endregion
        
//        /// <summary>
//        /// 跳变报警
//        /// </summary>
//        public override byte Alarm
//        {
//            get
//            {
//                return _alarm;
//            }
//            set
//            {
//                //if (_Alarm == value) return;
//                _alarm = value;
//                //this.RaisePropertyChanged(() => this.Alarm);
//                for (int t = 0; t < CollectionJumpAlarm.Count; t++)
//                {
//                    if (CollectionJumpAlarm[t].Value != _alarm)
//                        continue;
//                    SelectJumpAlarmIndex = t;
//                    break;
//                }
//            }
//        }
        
//        #region assist Alarm show
        
//        //辅助显示接触器状态所有选择
//        public ObservableCollection<StateBase> CollectionJumpAlarm
//        {
//            get
//            {
//                return Comm.CollectionJumpAlarm;
//            }
//        }

//        private int _SelectJumpAlarmIndex;
        
//        //辅助显示接触器状态当前选择
//        public int SelectJumpAlarmIndex
//        {
//            get
//            {
//                return _SelectJumpAlarmIndex;
//            }
//            set
//            {
//                if (_SelectJumpAlarmIndex != value)
//                {
//                    _SelectJumpAlarmIndex = value;
                    
//                    this.RaisePropertyChanged(() => this.SelectJumpAlarmIndex);
//                    Alarm = (byte)CollectionJumpAlarm[value].Value;
//                }
//            }
//        }

//        #endregion
        
//        /// <summary>
//        /// 状态 通时的显示名称
//        /// </summary>
//        public override int NoramlState
//        {
//            get
//            {
//                return _onState;
//            }
//            set
//            {
//                //if (_OnState == value) return;
//                _onState = value;
//                //this.RaisePropertyChanged(() => this.OnState);
//                for (int t = 0; t < CollectionContactorStateShow.Count; t++)
//                {
//                    if (CollectionContactorStateShow[t].Value != _onState)
//                        continue;
//                    SelectContactorOnStateShowIndex = t;
//                    break;
//                }
//            }
//        }
        
//        #region assist OnState show
        
//        //辅助显示接触器通时的显示名称所有选择
//        public ObservableCollection<StateBase> CollectionContactorStateShow
//        {
//            get
//            {
//                return Comm.CollectionContactorStateShow;
//            }
//        }
        
//        private int _SelectContactorOnStateShowIndex;
        
//        //辅助显示接触器通时的显示名称当前选择
//        public int SelectContactorOnStateShowIndex
//        {
//            get
//            {
//                return _SelectContactorOnStateShowIndex;
//            }
//            set
//            {
//                if (_SelectContactorOnStateShowIndex != value)
//                {
//                    _SelectContactorOnStateShowIndex = value;
                
//                    this.RaisePropertyChanged(() => this.SelectContactorOnStateShowIndex);
//                    NoramlState = (byte)CollectionContactorStateShow[value].Value;
//                }
//            }
//        }
        
//        #endregion
        
//        /// <summary>
//        /// 状态 断时的显示名称
//        /// </summary>
//        public override int UnNormalState   //CollectionContactorStateShow
//        {
//            get
//            {
//                return _offState;
//            }
//            set
//            {
//                //if (_OffState == value) return;
//                _offState = value;
//                //this.RaisePropertyChanged(() => this.OffState);
//                for (int t = 0; t < CollectionContactorStateShow.Count; t++)
//                {
//                    if (CollectionContactorStateShow[t].Value != _offState)
//                        continue;
//                    SelectContactorOffStateShowIndex = t;
//                    break;
//                }
//            }
//        }

//        #region assist OffState show
        
//        private int _SelectContactorOffStateShowIndex;
            
//        //辅助显示接触器断时的显示名称当前选择
//        public int SelectContactorOffStateShowIndex
//        {
//            get
//            {
//                return _SelectContactorOffStateShowIndex;
//            }
//            set
//            {
//                if (_SelectContactorOffStateShowIndex != value)
//                {
//                    _SelectContactorOffStateShowIndex = value;
//                    this.RaisePropertyChanged(() => this.SelectContactorOffStateShowIndex);
//                    UnNormalState = (byte)CollectionContactorStateShow[value].Value;
//                }
//            }
//        }

//        #endregion
        
//        #region point
            
//        public string BackgroundColor
//        {
//            get
//            {
//                return "#FFB0E0E6";
//            }
//        }
            
//        private void CalocatePoint()
//        {
//            X1onMap = 490;
//            Y1onMap = (this.SwitchInId - 1) * 35 + 10;
//        }
        
//        int _x1OnMap;
            
//        public int X1onMap
//        {
//            get
//            {
//                return _x1OnMap;
//            }
//            set
//            {
//                if (_x1OnMap != value)
//                {
//                    _x1OnMap = value;
//                    this.RaisePropertyChanged("X1onMap");
//                }
//            }
//        }
        
//        int _y1OnMap;
            
//        public int Y1onMap
//        {
//            get
//            {
//                return _y1OnMap;
//            }
//            set
//            {
//                if (_y1OnMap != value)
//                {
//                    _y1OnMap = value;
//                    this.RaisePropertyChanged("Y1onMap");
//                }
//            }
//        }
            
//        //int _widthControl;
//        public int WidthControl
//        {
//            get
//            {
//                return 120;
//            }
//        }
            
//        //int _heightControl;
//        public int HeightControl
//        {
//            get
//            {
//                return 25;
//            }
//        }
            
//        public int HeightDes
//        {
//            get
//            {
//                return 35;
//            }
//        }

//        #endregion











//        protected bool _setswitchinputstate;

      
//        public  bool Setswitchinputstate
//        {
//            get { return _setswitchinputstate; }
//            set
//            {
//                if (_setswitchinputstate == value) return;
//                _setswitchinputstate = value;
//                this.RaisePropertyChanged(() => this.Setswitchinputstate);
//            }
//        }
//        protected bool _setloopswitchinputstate;

       
//        public bool SetLoopswitchinputstate
//        {
//            get { return _setloopswitchinputstate; }
//            set
//            {
//                if (_setloopswitchinputstate == value) return;
//                _setloopswitchinputstate = value;
//                this.RaisePropertyChanged(() => this.SetLoopswitchinputstate);
//            }
//        }
//        protected bool _setallswitchinputstate;

        
//        public bool SetAllswitchinputstate
//        {
//            get { return _setallswitchinputstate; }
//            set
//            {
//                if (_setallswitchinputstate == value) return;
//                _setallswitchinputstate = value;
//                this.RaisePropertyChanged(() => this.SetAllswitchinputstate);
//            }
//        }

//    };
//}