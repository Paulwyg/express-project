using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.ProtocolCnt.AexchangeModels.ModelParts;

namespace Wlst.Ux.Wj1090Module.LduInfoSet.ViewModel
{
    public class LduLineViewModel : Cr.Core.CoreServices.ObservableObject
    {

        #region 参数

        private int _value1;
        private bool _value2;
        private int _value3;
        private string _value4;
        private int _value6;
        private int _value7;
        private int _value8;
        private int _value9;
        private int _value10;
        private int _value11;
        private int _value12;
        private bool _value13;
        private bool _value14;
        private bool _value15;
        private bool _value16;
        private bool _value17;
        private bool _value18;
        private bool _value19;
        private string _value20;
        private string _value21;
        private bool _value22;



        /// <summary>
        /// 线路序号
        /// </summary>
        public int LduLineID
        {
            get { return _value1; }
            set
            {
                if (_value1 == value) return;
                _value1 = value;
                RaisePropertyChanged(() => LduLineID);
            }
        }

        private bool _isEdit;
        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                if (value == _isEdit) return;
                _isEdit = value;
                RaisePropertyChanged(()=>IsEdit);
            }
        }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUsed
        {
            get { return _value2; }
            set
            {
                if (_value2 == value) return;
                _value2 = value;
                RaisePropertyChanged(() => IsUsed);
            }
        }


        /// <summary>
        /// 控制类型 0保留，1 1控1,2 1控2，3 1控3 。。。。。。
        /// </summary>
        public int LduControlTypeCode
        {
            get { return _value3; }
            set
            {
                if (_value3 == value) return;
                _value3 = value;
                RaisePropertyChanged(() => LduControlTypeCode);
            }
        }

        /// <summary>
        /// 线路名称
        /// </summary>
        public string LduLineName
        {
            get { return _value4; }
            set
            {
                if (_value4 == value) return;
                _value4 = value;
                RaisePropertyChanged(() => LduLineName);
            }
        }

        ///// <summary>
        ///// 通信方式 0 保留，1 电台，2 串口232，3 串口485，4 Zigbee，5 电力载波，6 Socket  一般为3或6
        ///// </summary>
        //public string LduCommType
        //{
        //    get { return _value5; }
        //    set
        //    {
        //        if (_value5 != value)
        //        {
        //            _value5 = value;
        //            this.RaisePropertyChanged(() => LduCommType);
        //        }
        //    }
        //}

        /// <summary>
        /// 互感器比值
        /// </summary>
        public int MutualInductorRadio
        {
            get { return _value6; }
            set
            {
                if (_value6 != value)
                {
                    _value6 = value;
                    RaisePropertyChanged(() => MutualInductorRadio);
                }
            }
        }

        /// <summary>
        /// 相位 1A  ,2B ,3C
        /// </summary>
        public int LduPhase
        {
            get { return _value7; }
            set
            {
                if (_value7 != value)
                {
                    _value7 = value;
                    RaisePropertyChanged(() => LduPhase);

                    for (int t = 0; t < CollectionLduPhase.Count; t++)
                    {
                        if (CollectionLduPhase[t].Value != _value7)
                            continue;
                        SelectLduPhaseIndex = t;
                        break;
                    }
                }
            }
        }

        private ObservableCollection<NameValueInt> _collectionLduPhase;
        //辅助显示接触器通时的显示名称所有选择
        public ObservableCollection<NameValueInt> CollectionLduPhase
        {
            get
            {
                return _collectionLduPhase ?? (_collectionLduPhase = new ObservableCollection<NameValueInt>
                                                                         {
                                                                             new NameValueInt {Name = "A相", Value = 1},
                                                                             new NameValueInt {Name = "B相", Value = 2},
                                                                             new NameValueInt {Name = "C相", Value = 3}
                                                                         });
            }
        }

        private int _selectLduPhaseIndex;

        //辅助显示接触器通时的显示名称当前选择
        public int SelectLduPhaseIndex
        {
            get
            {
                return _selectLduPhaseIndex;
            }
            set
            {
                if (_selectLduPhaseIndex != value)
                {
                    _selectLduPhaseIndex = value;

                    RaisePropertyChanged(() => SelectLduPhaseIndex);
                    LduPhase = CollectionLduPhase[value].Value;
                }
            }
        }

        /// <summary>
        /// 开灯信号强度门限
        /// </summary>
        public int LduLightonSingleLimit
        {
            get { return _value8; }
            set
            {
                if (_value8 != value)
                {
                    _value8 = value;
                    RaisePropertyChanged(() => LduLightonSingleLimit);
                }
            }
        }

        /// <summary>
        /// 开灯阻抗报警门限
        /// </summary>
        public int LduLightonImpedanceLimit
        {
            get { return _value9; }
            set
            {
                if (_value9 != value)
                {
                    _value9 = value;
                    RaisePropertyChanged(() => LduLightonImpedanceLimit);
                }
            }
        }

        /// <summary>
        /// 亮灯率报警门限
        /// </summary>
        public int LduBrightRateAlarmLimit
        {
            get { return _value10; }
            set
            {
                if (_value10 != value)
                {
                    _value10 = value;
                    RaisePropertyChanged(() => LduBrightRateAlarmLimit);
                }
            }
        }

        /// <summary>
        /// 关灯信号强度门限
        /// </summary>
        public int LduLightoffSingleLimit
        {
            get { return _value11; }
            set
            {
                if (_value11 != value)
                {
                    _value11 = value;
                    RaisePropertyChanged(() => LduLightoffSingleLimit);
                }
            }
        }

        /// <summary>
        /// 关灯阻抗报警门限
        /// </summary>
        public int LduLightoffImpedanceLimit
        {
            get { return _value12; }
            set
            {
                if (_value12 != value)
                {
                    _value12 = value;
                    RaisePropertyChanged(() => LduLightoffImpedanceLimit);
                }
            }
        }

        ///// <summary>
        ///// 故障参数
        ///// </summary>
        //public int LduFaultParam { get; set; }

        public bool AlarmAutoReport
        {
            get { return _value22; }
            set
            {
                if(_value22 !=value)
                {
                    _value22 = value;
                    RaisePropertyChanged(() => AlarmAutoReport);
                }
            }
        }

        /// <summary>
        /// 线路短路主动告警
        /// </summary>
        public bool AlarmLineShortCircuit
        {
            get { return _value13; }
            set
            {
                if (_value13 != value)
                {
                    _value13 = value;
                    RaisePropertyChanged(() => AlarmLineShortCircuit);
                }
            }
        }

        /// <summary>
        /// 关灯阻抗主动报警
        /// </summary>
        public bool AlarmLineLightOffImpedance
        {
            get { return _value14; }
            set
            {
                if (_value14 != value)
                {
                    _value14 = value;
                    RaisePropertyChanged(() => AlarmLineLightOffImpedance);
                }
            }
        }

        /// <summary>
        /// 关灯信号强度主动告警
        /// </summary>
        public bool AlarmLineLightOffSingle
        {
            get { return _value15; }
            set
            {
                if (_value15 != value)
                {
                    _value15 = value;
                    RaisePropertyChanged(() => AlarmLineLightOffSingle);
                }
            }
        }

        /// <summary>
        /// 线路失电主动告警
        /// </summary>
        public bool AlarmLineLosePower
        {
            get { return _value16; }
            set
            {
                if (_value16 != value)
                {
                    _value16 = value;
                    RaisePropertyChanged(() => AlarmLineLosePower);
                }
            }
        }

        /// <summary>
        /// 亮灯率变化主动告警
        /// </summary>
        public bool AlarmLineBrightRate
        {
            get { return _value17; }
            set
            {
                if (_value17 != value)
                {
                    _value17 = value;
                    RaisePropertyChanged(() => AlarmLineBrightRate);
                }
            }
        }

        /// <summary>
        /// 开灯阻抗主动报警
        /// </summary>
        public bool AlarmLineLightOpenImpedance
        {
            get { return _value18; }
            set
            {
                if (_value18 != value)
                {
                    _value18 = value;
                    RaisePropertyChanged(() => AlarmLineLightOpenImpedance);
                }
            }
        }

        /// <summary>
        /// 开灯信号强度主动告警
        /// </summary>
        public bool AlarmLineLightOpenSingel
        {
            get { return _value19; }
            set
            {
                if (_value19 != value)
                {
                    _value19 = value;
                    RaisePropertyChanged(() => AlarmLineLightOpenSingel);
                }
            }
        }




        /// <summary>
        /// 本防盗设备的末端 安装的灯杆序号
        /// </summary>
        public string LduEndLampportSn
        {
            get { return _value20; }
            set
            {
                if (_value20 != value)
                {
                    _value20 = value;
                    RaisePropertyChanged(() => LduEndLampportSn);
                }
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return _value21; }
            set
            {
                if (_value21 != value)
                {
                    _value21 = value;
                    RaisePropertyChanged(() => Remark);
                }
            }
        }

        #endregion

        private readonly LduLineParameter _inmodel;
        public LduLineViewModel(LduLineParameter cnt,List<NameValueInt> list )
        {
            foreach (var item in list)
            {
                LoopCollection.Add(new NameValueInt { Name = item.Name, Value = item.Value });
            }
            LoopCollection.Add(new NameValueInt{Name = "请选择回路",Value = 0});
            _inmodel = cnt;
            Remark = cnt.Remark;
            IsUsed = cnt.IsUsed;
            LduBrightRateAlarmLimit = cnt.LduBrightRateAlarmLimit;
            //LduCommType = cnt.LduCommTypeCode == EnumCommunicationMode.Socket ? "无线" : "有线";
            LduControlTypeCode = cnt.LduControlTypeCode;
            LduEndLampportSn = cnt.LduEndLampportSn;
            LduLightoffImpedanceLimit = cnt.LduLightoffImpedanceLimit;
            LduLightoffSingleLimit = cnt.LduLightoffSingleLimit;
            LduLightonImpedanceLimit = cnt.LduLightonImpedanceLimit;
            LduLightonSingleLimit = cnt.LduLightonSingleLimit;
            LduLineID = cnt.LduLineID;
            LduLineName = cnt.LduLineName;
            LduLoopID = cnt.LduLoopID;
            LduPhase = cnt.LduPhase;
            MutualInductorRadio = cnt.MutualInductorRadio;
            AlarmLineBrightRate = cnt.AlarmLineBrightRate;
            AlarmLineLightOffImpedance = cnt.AlarmLineLightOffImpedance;
            AlarmLineLightOffSingle = cnt.AlarmLineLightOffSingle;
            AlarmLineLightOpenImpedance = cnt.AlarmLineLightOpenImpedance;
            AlarmLineLightOpenSingel = cnt.AlarmLineLightOpenSingel;
            AlarmLineLosePower = cnt.AlarmLineLosePower;
            AlarmLineShortCircuit = cnt.AlarmLineShortCircuit;
            //默认情况下不编辑
            IsEdit = false;
        }

        public LduLineParameter BackToLduLineParameter()
        {
            if (_inmodel == null) return null;
            var info = new LduLineParameter
                           {
                               IsUsed = IsUsed,
                               LduBrightRateAlarmLimit = LduBrightRateAlarmLimit,
                               LduCommTypeCode = _inmodel.LduCommTypeCode,
                               LduConcentratorId = _inmodel.LduConcentratorId,
                               LduControlTypeCode = LduControlTypeCode,
                               LduEndLampportSn = LduEndLampportSn,
                               AlarmLineBrightRate = AlarmLineBrightRate,
                               AlarmLineLightOffSingle = AlarmLineLightOffSingle,
                               AlarmLineLightOffImpedance = AlarmLineLightOffImpedance,
                               AlarmLineLightOpenImpedance = AlarmLineLightOpenImpedance,
                               AlarmLineLightOpenSingel = AlarmLineLightOpenSingel,
                               AlarmLineLosePower = AlarmLineLosePower,
                               AlarmLineShortCircuit = AlarmLineShortCircuit,
                               LduLightoffImpedanceLimit = LduLightoffImpedanceLimit,
                               LduLightoffSingleLimit = LduLightoffSingleLimit,
                               LduLightonImpedanceLimit = LduLightonImpedanceLimit,
                               LduLightonSingleLimit = LduLightonSingleLimit,
                               LduLineID = LduLineID,
                               LduLineName = LduLineName,
                               LduLoopID = LduLoopID,
                               LduPhase = LduPhase,
                               MutualInductorRadio = MutualInductorRadio,
                               Remark = Remark
                           };
            return info;
        }

        /// <summary>
        /// 回路序号  本防盗检测设备检测的终端回路的回路序号 ,,,,,,,
        /// </summary>
        private int _lduLoopID;
        public int LduLoopID
        {
            get { return _lduLoopID; }
            set
            {
                if (_lduLoopID != value)
                {
                    _lduLoopID = value;
                    RaisePropertyChanged("LduLoopID");
                }
                foreach (var item in LoopCollection)
                {
                    if (value != item.Value)
                        continue;
                    SelectedLoopVlue = item;
                }
            }
        }

        private NameValueInt _selectLoopValue;
        public NameValueInt SelectedLoopVlue
        {
            get { return _selectLoopValue ?? (_selectLoopValue = new NameValueInt()); }
            set
            {
                if(value  !=SelectedLoopVlue)
                {
                    _selectLoopValue = value;
                    RaisePropertyChanged("SelectedLoopVlue");
                    LduLoopID = _selectLoopValue.Value;
                    
                }
            }
        }

        private ObservableCollection<NameValueInt> _loopCollection;
        public ObservableCollection<NameValueInt> LoopCollection
        {
            get { return _loopCollection ?? (_loopCollection = new ObservableCollection<NameValueInt>()); }
        }
    }
}
