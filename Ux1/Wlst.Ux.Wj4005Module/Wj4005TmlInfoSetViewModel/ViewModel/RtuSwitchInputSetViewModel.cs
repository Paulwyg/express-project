using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Ux.WJ4005Module.Wj4005TmlInfoSetViewModel.Services;

namespace Wlst.Ux.WJ4005Module.Wj4005TmlInfoSetViewModel.ViewModel
{
   public  class RtuSwitchInputSetViewModel:Wlst .Cr .Core .CoreServices .ObservableObject 
    {   
        public RtuSwitchInputSetViewModel()
        {
            this.SetAllswitchinputstate = false;
            this.SetLoopswitchinputstate = false;
            this.Setswitchinputstate = false;
            IsCommandClosed = false;
        }

       public bool IsCommandClosed;

       public int _switchInId;
        /// <summary>
        /// 开关量输入序号
        /// </summary>
        public  int SwitchInId
        {
            get
            {
                return _switchInId;
            }
            set
            {
                if (_switchInId == value)  return;
                _switchInId = value;
                
                this.RaisePropertyChanged(() => this.SwitchInId);

            }
        }

       private byte _contactorState;
        /// <summary>
        /// 接触器状态:常开/常闭/无
        /// </summary>
        public  byte ContactorState
        {
            get
            {
                return _contactorState;
            }
            set
            {
                //if (_ContactorState == value) return;
                _contactorState = value;
                for (int t = 0; t < CollectionContractorState.Count; t++)
                {
                    if (CollectionContractorState[t].Value != _contactorState)
                        continue;
                    SelectContractorStateIndex = t;
                    break;
                }
            }
        }

        #region assist ContactorState show
        
        //辅助显示接触器状态所有选择
        public ObservableCollection<StateBase> CollectionContractorState
        {
            get 
            { 
                return Comm.CollectionContactorState;
            }
        }
        
        private int _SelectContractorStateIndex;
        
        //辅助显示接触器状态当前选择
        public int SelectContractorStateIndex
        {
            get
            {
                return _SelectContractorStateIndex;
            }
            set
            {
                if (_SelectContractorStateIndex != value)
                {
                    _SelectContractorStateIndex = value;
                    
                    this.RaisePropertyChanged(() => this.SelectContractorStateIndex);
                    ContactorState = (byte)CollectionContractorState[value].Value;
                }
            }
        }
        
        #endregion

       private byte _alarm;
        /// <summary>
        /// 跳变报警
        /// </summary>
        public  byte Alarm
        {
            get
            {
                return _alarm;
            }
            set
            {
                //if (_Alarm == value) return;
                _alarm = value;
                //this.RaisePropertyChanged(() => this.Alarm);
                for (int t = 0; t < CollectionJumpAlarm.Count; t++)
                {
                    if (CollectionJumpAlarm[t].Value != _alarm)
                        continue;
                    SelectJumpAlarmIndex = t;
                    break;
                }
            }
        }
        
        #region assist Alarm show
        
        //辅助显示接触器状态所有选择
        public ObservableCollection<StateBase> CollectionJumpAlarm
        {
            get
            {
                return Comm.CollectionJumpAlarm;
            }
        }

        private int _SelectJumpAlarmIndex;
        
        //辅助显示接触器状态当前选择
        public int SelectJumpAlarmIndex
        {
            get
            {
                return _SelectJumpAlarmIndex;
            }
            set
            {
                if (_SelectJumpAlarmIndex != value)
                {
                    _SelectJumpAlarmIndex = value;
                    
                    this.RaisePropertyChanged(() => this.SelectJumpAlarmIndex);
                    Alarm = (byte)CollectionJumpAlarm[value].Value;
                }
            }
        }

        #endregion

       private int _onState;
        /// <summary>
        /// 状态 通时的显示名称
        /// </summary>
        public  int OnState
        {
            get
            {
                return _onState;
            }
            set
            {
                //if (_OnState == value) return;
                _onState = value;
                //this.RaisePropertyChanged(() => this.OnState);
                for (int t = 0; t < CollectionContactorStateShow.Count; t++)
                {
                    if (CollectionContactorStateShow[t].Value != _onState)
                        continue;
                    SelectContactorOnStateShowIndex = t;
                    break;
                }
            }
        }
        
        #region assist OnState show
        
        //辅助显示接触器通时的显示名称所有选择
        public ObservableCollection<StateBase> CollectionContactorStateShow
        {
            get
            {
                return Comm.CollectionContactorStateShow;
            }
        }
        
        private int _SelectContactorOnStateShowIndex;
        
        //辅助显示接触器通时的显示名称当前选择
        public int SelectContactorOnStateShowIndex
        {
            get
            {
                return _SelectContactorOnStateShowIndex;
            }
            set
            {
                if (_SelectContactorOnStateShowIndex != value)
                {
                    _SelectContactorOnStateShowIndex = value;
                
                    this.RaisePropertyChanged(() => this.SelectContactorOnStateShowIndex);
                    OnState = (byte)CollectionContactorStateShow[value].Value;
                }
            }
        }
        
        #endregion

       private int _offState;  
        /// <summary>
        /// 状态 断时的显示名称
        /// </summary>
        public  int OffState   //CollectionContactorStateShow
        {
            get
            {
                return _offState;
            }
            set
            {
                //if (_OffState == value) return;
                _offState = value;
                //this.RaisePropertyChanged(() => this.OffState);
                for (int t = 0; t < CollectionContactorStateShow.Count; t++)
                {
                    if (CollectionContactorStateShow[t].Value != _offState)
                        continue;
                    SelectContactorOffStateShowIndex = t;
                    break;
                }
            }
        }

        #region assist OffState show
        
        private int _SelectContactorOffStateShowIndex;
            
        //辅助显示接触器断时的显示名称当前选择
        public int SelectContactorOffStateShowIndex
        {
            get
            {
                return _SelectContactorOffStateShowIndex;
            }
            set
            {
                if (_SelectContactorOffStateShowIndex != value)
                {
                    _SelectContactorOffStateShowIndex = value;
                    this.RaisePropertyChanged(() => this.SelectContactorOffStateShowIndex);
                    OffState = (byte)CollectionContactorStateShow[value].Value;
                }
            }
        }

        #endregion
        


        protected bool _setswitchinputstate;

      
        public  bool Setswitchinputstate
        {
            get { return _setswitchinputstate; }
            set
            {
                if (_setswitchinputstate == value) return;
                _setswitchinputstate = value;
                this.RaisePropertyChanged(() => this.Setswitchinputstate);
            }
        }
        protected bool _setloopswitchinputstate;

       
        public bool SetLoopswitchinputstate
        {
            get { return _setloopswitchinputstate; }
            set
            {
                if (_setloopswitchinputstate == value) return;
                _setloopswitchinputstate = value;
                this.RaisePropertyChanged(() => this.SetLoopswitchinputstate);
            }
        }
        protected bool _setallswitchinputstate;

        
        public bool SetAllswitchinputstate
        {
            get { return _setallswitchinputstate; }
            set
            {
                if (_setallswitchinputstate == value) return;
                _setallswitchinputstate = value;
                this.RaisePropertyChanged(() => this.SetAllswitchinputstate);
            }
        }
    }
}
