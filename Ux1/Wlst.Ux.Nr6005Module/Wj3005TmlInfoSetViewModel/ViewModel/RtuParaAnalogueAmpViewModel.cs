//using System;
//using System.Collections.ObjectModel;
//using System.Windows;
//using System.Windows.Controls;
//using Wlst.Cr.CoreMims.Commands;
//using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
//using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
//using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;
//using Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.Services;

//namespace Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.ViewModel
//{
//    /// <summary>
//    /// 回路数据
//    /// </summary>
//    public sealed class RtuParaAnalogueAmpViewModel :
//        Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationPartsViewModel.RtuParaAnalogueAmpViewModel
//    {
//        public RtuParaAnalogueAmpViewModel(RtuParaAnalogueAmps.RtuParaAnalogueAmp rtuParaAnalogueAmp) : base()
//        {
//            //this.RtuParaSwitchInViewModels = rtuParaSwitchInViewModels;
//            //this.RtuParaSwitchOutViewModels = rtuParaSwitchOutViewModels;
//            this.RtuId = rtuParaAnalogueAmp.RtuId;
//            this.LoopId = rtuParaAnalogueAmp.LoopId;
//            this.LoopName = rtuParaAnalogueAmp.LoopName;
//            this.Range = rtuParaAnalogueAmp.Range;

//            this.UpperLimit = rtuParaAnalogueAmp.UpperLimit;
//            this.LowerLimit = rtuParaAnalogueAmp.LowerLimit;
//            this.Vector = rtuParaAnalogueAmp.Vector;
//            this.Phase = rtuParaAnalogueAmp.Phase;
//            this.LightRate = rtuParaAnalogueAmp.LightRate;
//            this.LightRateLowerLimit = rtuParaAnalogueAmp.LightRateLowerLimit;
//            this.SwitchInId = rtuParaAnalogueAmp.SwitchInId;
//            this.SwitchOutId = rtuParaAnalogueAmp.SwitchOutId;
//            this.AmRange = rtuParaAnalogueAmp.AmRange;
//        }

//        public RtuParaAnalogueAmpViewModel()
//        {
//        }

//        public override int Vector
//        {
//            get
//            {
//                return base.Vector;
//            }
//            set
//            {
//                base.Vector = value;
//                this.RaisePropertyChanged(() => this.Vector);
//                for (int t = 0; t < CollectionVector.Count; t++)
//                {
//                    if (CollectionVector[t].Value != _vector)
//                        continue;
//                    SelectVectorIndex = t;
//                    break;
//                }
//            }
//        }

//        #region assist Vector show

//        //辅助显示
//        public ObservableCollection<StateBase> CollectionVector
//        {
//            get
//            {
//                return Comm.CollectionVector36;
//            }
//        }

//        private int _SelectVectorIndex;

//        //辅助显示
//        public int SelectVectorIndex
//        {
//            get
//            {
//                return _SelectVectorIndex;
//            }
//            set
//            {
//                if (_SelectVectorIndex != value)
//                {
//                    _SelectVectorIndex = value;

//                    this.RaisePropertyChanged(() => this.SelectVectorIndex);
//                    if (Vector != CollectionVector[value].Value)//这句话非常重要 不允许删除 删除则逻辑错误
//                    {
//                        Vector = CollectionVector[value].Value;
//                        OnSwitchInOutChangeEx(1);
//                    }
//                }
//            }
//        }

//        #endregion

//        public override int SwitchInId
//        {
//            get
//            {
//                return base.SwitchInId;
//            }
//            set
//            {
//                base.SwitchInId = value;
//                this.RaisePropertyChanged(() => this.SwitchInId);
                
//                for (int t = 0; t < CollectionSwitchInId.Count; t++)
//                {
//                    if (CollectionSwitchInId[t].Value != _switchInId)
//                        continue;
//                    SelectSwitchInIdIndex = t;
//                    break;
//                }
//            }
//        }

//        #region assist SwitchInId show

//        //辅助显示
//        public ObservableCollection<StateBase> CollectionSwitchInId
//        {
//            get
//            {
//                return Comm.CollectionSwitchIn16 ;
//            }
//        }

//        private int _SelectSwitchInIdIndex;

//        //辅助显示
//        public int SelectSwitchInIdIndex
//        {
//            get
//            {
//                return _SelectSwitchInIdIndex;
//            }
//            set
//            {
//                if (_SelectSwitchInIdIndex != value)
//                {
//                    _SelectSwitchInIdIndex = value;

//                    this.RaisePropertyChanged(() => this.SelectSwitchInIdIndex);
//                    if (SwitchInId != CollectionSwitchInId[value].Value)
//                    {
//                        SwitchInId = CollectionSwitchInId[value].Value;
//                        OnSwitchInOutChangeEx(0);
//                    }
//                }
//            }
//        }

//        #endregion

//        public override int SwitchOutId
//        {
//            get
//            {
//                return base.SwitchOutId;
//            }
//            set
//            {
//                base.SwitchOutId = value;
//                this.RaisePropertyChanged(() => this.SwitchOutId);

//                for (int t = 0; t < CollectionSwitchOutId.Count; t++)
//                {
//                    if (CollectionSwitchOutId[t].Value != _switchOutId)
//                        continue;
//                    SelectSwitchOutIdIndex = t;
//                    break;
//                }
//            }
//        }

//        #region assist SwitchOutId show

//        //辅助显示
//        public ObservableCollection<StateBase> CollectionSwitchOutId
//        {
//            get
//            {
//                return Comm.CollectionSwitchOut6;
//            }
//        }

//        private int _SelectSwitchOutIdIndex;

//        //辅助显示
//        public int SelectSwitchOutIdIndex
//        {
//            get
//            {
//                return _SelectSwitchOutIdIndex;
//            }
//            set
//            {
//                if (_SelectSwitchOutIdIndex != value)
//                {
//                    _SelectSwitchOutIdIndex = value;

//                    this.RaisePropertyChanged(() => this.SelectSwitchOutIdIndex);
//                    if (SwitchOutId != CollectionSwitchOutId[value].Value)
//                    {
//                        SwitchOutId = CollectionSwitchOutId[value].Value;
//                        OnSwitchInOutChangeEx(0);
//                    }
//                }
//            }
//        }

//        #endregion

//        /// <summary>
//        /// 电压相位
//        /// </summary>
//        public override int Phase
//        {
//            get
//            {
//                return _phase;
//            }
//            set
//            {
//                //if (_Phase == value) return;
//                _phase = value;
//                //this.RaisePropertyChanged(() => this.Phase);
//                for (int t = 0; t < CollectionPhase.Count; t++)
//                {
//                    if (CollectionPhase[t].Value != _phase)
//                        continue;
//                    SelectPhaseIndex = t;
//                    break;
//                }
//            }
//        }

//        #region assist Phase show

//        //辅助显示接触器状态所有选择
//        public ObservableCollection<StateBase> CollectionPhase
//        {
//            get
//            {
//                return Comm.CollectionPhase;
//            }
//        }

//        private int _SelectPhaseIndex;

//        //辅助显示接触器状态当前选择
//        public int SelectPhaseIndex
//        {
//            get
//            {
//                return _SelectPhaseIndex;
//            }
//            set
//            {
//                if (_SelectPhaseIndex != value)
//                {
//                    _SelectPhaseIndex = value;

//                    this.RaisePropertyChanged(() => this.SelectPhaseIndex);
//                    Phase = CollectionPhase[value].Value;
//                }
//            }
//        }

//        #endregion

//        private int _index;

//        /// <summary>
//        /// index 1开始
//        /// </summary>
//        public int Index
//        {
//            get
//            {
//                return _index;
//            }
//            set
//            {
//                if (_index != value)
//                {
//                    _index = value;
//                    CalocatePoint();
//                }
//            }
//        }

//        public string BackgroundColor
//        {
//            get
//            {
//                return "#FFFFFF";
//            }
//        }

//        #region point

//        private void CalocatePoint()
//        {
//            X1onMap = 240;
//            if (this.Index < 17)
//            {
//                Y1onMap = (this.Index - 1) * 31 + 40;
//            }
//            else if (this.Index < 33)
//            {
//                Y1onMap = (this.Index - 17) * 31 + 40;
//            }
//            else if (this.Index < 49)
//            {
//                Y1onMap = (this.Index - 33) * 31 + 40;
//            }
//        }

//        private int _x1OnMap;

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

//        private int _y1OnMap;

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
//                return 150;
//            }
//        }

//        private int _heightControl;

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
//                return 31;
//            }
//        }

//        #endregion

//        #region Delete 右键菜单
        
//        /// <summary>
//        /// 右键菜单
//        /// </summary>
//        public ContextMenu Cm
//        {
//            get
//            {
//                var cm = new ContextMenu();
//                try
//                {
//                    cm.Items.Add(DeleteThisLoop);
//                }
//                catch (Exception ex)
//                {
//                    ex.ToString();
//                }
//                return cm;
//            }
//        }
        
//        private MenuItem DeleteThisLoop
//        {
//            get
//            {
//                MenuItem _EditFolder;
//                _EditFolder = new MenuItem();
//                _EditFolder.ToolTip = "删除本回路;回路地址:" + this.LoopId + ",回路名称:" + this.LoopName + "。";
//                _EditFolder.Header = "删除";
//                _EditFolder.Command = new RelayCommand(DeleteAnalogueAmpViewModelEx  );
//                return _EditFolder;
//            }
//        }
        
//        public event EventHandler OnDeleteAnalogueAmpViewModel = delegate { };
        
//        private void DeleteAnalogueAmpViewModelEx()
//        {
//            if (OnDeleteAnalogueAmpViewModel == null)
//                return;
//            //var result = MessageBox.Show("Sure to Delete Loop " + LoopId + " ?", "Warm", MessageBoxButton.YesNo);
//           // var result = WlstMessageBox.Show("Sure to Delete Loop " + LoopId + " ?", WlstMessageBoxType.YesNo);
//            if (WlstMessageBox.Show("Sure to Delete Loop " + LoopId + " ?", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.Yes)
//            {
//                OnDeleteAnalogueAmpViewModel(this, EventArgs.Empty);
//            }
//        }
        
//        #endregion
        
//        #region  refresh draw line

//        public event EventHandler OnSwitchInOutChange = delegate { };
        
//        private void OnSwitchInOutChangeEx(int xtype)
//        {
//            if (OnSwitchInOutChange == null)
//                return;
//            if (xtype == 0)
//                OnSwitchInOutChange(this, EventArgs.Empty);
//            else if (xtype == 1)
//                OnSwitchInOutChange(this, new EventArgs());
//        }

//        #endregion

//    };
//}