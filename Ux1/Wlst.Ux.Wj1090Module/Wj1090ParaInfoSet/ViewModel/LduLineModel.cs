using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.Models;
using Wlst.client;


namespace Wlst.Ux.Wj1090Module.Wj1090ParaInfoSet.ViewModel
{
    public class LduLineModel : Cr.Core.CoreServices.ObservableObject
    {
        #region ZcItems

        private ObservableCollection<LduLineModel> _zcItems;
        public ObservableCollection<LduLineModel> ZcItems
        {
            get { return _zcItems ?? (_zcItems = new ObservableCollection<LduLineModel>()); }
        }
        #endregion

        #region DataType

        private string _dataType;
        public string DataType
        {
            get { return _dataType; }
            set
            {
                if(_dataType==value) return;
                _dataType = value;
                RaisePropertyChanged(()=>DataType);
            }
        }
        #endregion

        #region LduLineID 线路序号
        private int _lduLineId;
        public int LduLineID
        {
            get { return _lduLineId; }
            set
            {
                if (_lduLineId == value) return;
                _lduLineId = value;
                RaisePropertyChanged(() => LduLineID);
            }
        }

        #endregion

        #region IsEdit 编辑
        private bool _isEdit;
        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                if (value == _isEdit) return;
                _isEdit = value;
                RaisePropertyChanged(() => IsEdit);
            }
        }
        #endregion

        #region 使用 IsUsed
        private bool _isUsed;
        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                if (_isUsed == value) return;
                _isUsed = value;
                RaisePropertyChanged(() => IsUsed);
            }
        }
        #endregion

        
        #region 控制类型 0保留，1 1控1,2 1控2，3 1控3 。。。。。。
        private int _lduControlTypeCode;
        public int LduControlTypeCode
        {
            get { return _lduControlTypeCode; }
            set
            {
                if (_lduControlTypeCode == value) return;
                _lduControlTypeCode = value;
                RaisePropertyChanged(() => LduControlTypeCode);
            }
        }
        #endregion
        
        #region 线路名称 LduLineName
        private string _lduLineName;
        public string LduLineName
        {
            get { return _lduLineName; }
            set
            {
                if (_lduLineName == value) return;
                _lduLineName = value;
                RaisePropertyChanged(() => LduLineName);
            }
        }
        #endregion 
        
        #region MutualInductorRadio 互感器比值
        private int _mutualInductorRadio;
        public int MutualInductorRadio
        {
            get { return _mutualInductorRadio; }
            set
            {
                if (_mutualInductorRadio != value)
                {
                    _mutualInductorRadio = value;
                    RaisePropertyChanged(() => MutualInductorRadio);
                }
            }
        }
        #endregion
        
        #region 相位 1A  ,2B ,3C
        private int _lduPhase;
        public int LduPhase
        {
            get { return _lduPhase; }
            set
            {
                if (_lduPhase != value)
                {
                    _lduPhase = value;
                    RaisePropertyChanged(() => LduPhase);

                    for (int t = 0; t < CollectionLduPhase.Count; t++)
                    {
                        if (CollectionLduPhase[t].Value != _lduPhase)
                            continue;
                        SelectLduPhaseIndex = t;
                        break;
                    }
                }
            }
        }
        #endregion 

        #region 辅助显示接触器通时的显示名称所有选择
        private ObservableCollection<NameValueInt> _collectionLduPhase;
        
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
        #endregion

        #region 辅助显示接触器通时的显示名称当前选择
        private int _selectLduPhaseIndex;
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
        #endregion
        
        #region 开灯信号强度门限
        private int _lduLightOnSingleLimit;
        public int LduLightonSingleLimit
        {
            get { return _lduLightOnSingleLimit; }
            set
            {
                if (_lduLightOnSingleLimit != value)
                {
                    _lduLightOnSingleLimit = value;
                    RaisePropertyChanged(() => LduLightonSingleLimit);
                }
            }
        }
        #endregion 

        
        #region 开灯阻抗报警门限
        private int _lduLightonImpedanceLimit;
        public int LduLightonImpedanceLimit
        {
            get { return _lduLightonImpedanceLimit; }
            set
            {
                if (_lduLightonImpedanceLimit != value)
                {
                    _lduLightonImpedanceLimit = value;
                    RaisePropertyChanged(() => LduLightonImpedanceLimit);
                }
            }
        }
        #endregion
        
        #region 亮灯率报警门限
        private int _lduBrightRateAlarmLimit;
        public int LduBrightRateAlarmLimit
        {
            get { return _lduBrightRateAlarmLimit; }
            set
            {
                if (_lduBrightRateAlarmLimit != value)
                {
                    _lduBrightRateAlarmLimit = value;
                    RaisePropertyChanged(() => LduBrightRateAlarmLimit);
                }
            }
        }
        #endregion
        
        #region 关灯信号强度门限
        private int _lduLightoffSingleLimit;
        public int LduLightoffSingleLimit
        {
            get { return _lduLightoffSingleLimit; }
            set
            {
                if (_lduLightoffSingleLimit != value)
                {
                    _lduLightoffSingleLimit = value;
                    RaisePropertyChanged(() => LduLightoffSingleLimit);
                }
            }
        }
        #endregion 
        
        #region 关灯阻抗报警门限
        private int _lduLightoffImpedanceLimit;
        public int LduLightoffImpedanceLimit
        {
            get { return _lduLightoffImpedanceLimit; }
            set
            {
                if (_lduLightoffImpedanceLimit != value)
                {
                    _lduLightoffImpedanceLimit = value;
                    RaisePropertyChanged(() => LduLightoffImpedanceLimit);
                }
            }
        }
        #endregion
        
        #region 故障参数
        private bool _alarmAutoReport;
        public bool AlarmAutoReport
        {
            get { return _alarmAutoReport; }
            set
            {
                if (_alarmAutoReport != value)
                {
                    _alarmAutoReport = value;
                    RaisePropertyChanged(() => AlarmAutoReport);
                }
            }
        }
        #endregion
        
        #region 线路短路主动告警
        private bool _alarmLineShortCircuit;
        public bool AlarmLineShortCircuit
        {
            get { return _alarmLineShortCircuit; }
            set
            {
                if (_alarmLineShortCircuit != value)
                {
                    _alarmLineShortCircuit = value;
                    RaisePropertyChanged(() => AlarmLineShortCircuit);
                }
            }
        }
        #endregion
        
        #region 关灯阻抗主动报警
        private bool _alarmLineLightOffImpedance;
        public bool AlarmLineLightOffImpedance
        {
            get { return _alarmLineLightOffImpedance; }
            set
            {
                if (_alarmLineLightOffImpedance != value)
                {
                    _alarmLineLightOffImpedance = value;
                    RaisePropertyChanged(() => AlarmLineLightOffImpedance);
                }
            }
        }
        #endregion
        
        #region 关灯信号强度主动告警
        private bool _alarmLineLightOffSingle;
        public bool AlarmLineLightOffSingle
        {
            get { return _alarmLineLightOffSingle; }
            set
            {
                if (_alarmLineLightOffSingle != value)
                {
                    _alarmLineLightOffSingle = value;
                    RaisePropertyChanged(() => AlarmLineLightOffSingle);
                }
            }
        }
        #endregion
        
        #region 线路失电主动告警
        private bool _alarmLineLosePower;
        public bool AlarmLineLosePower
        {
            get { return _alarmLineLosePower; }
            set
            {
                if (_alarmLineLosePower != value)
                {
                    _alarmLineLosePower = value;
                    RaisePropertyChanged(() => AlarmLineLosePower);
                }
            }
        }
        #endregion
        
        #region 亮灯率变化主动告警
        private bool _alarmLineBrightRate;       
        public bool AlarmLineBrightRate
        {
            get { return _alarmLineBrightRate; }
            set
            {
                if (_alarmLineBrightRate != value)
                {
                    _alarmLineBrightRate = value;
                    RaisePropertyChanged(() => AlarmLineBrightRate);
                }
            }
        }
        #endregion
        
        #region 开灯阻抗主动报警
        private bool _alarmLineLightOpenImpedance;
        public bool AlarmLineLightOpenImpedance
        {
            get { return _alarmLineLightOpenImpedance; }
            set
            {
                if (_alarmLineLightOpenImpedance != value)
                {
                    _alarmLineLightOpenImpedance = value;
                    RaisePropertyChanged(() => AlarmLineLightOpenImpedance);
                }
            }
        }
        #endregion
        
        #region 开灯信号强度主动告警
        private bool _alarmLineLightOpenSingel;
        public bool AlarmLineLightOpenSingel
        {
            get { return _alarmLineLightOpenSingel; }
            set
            {
                if (_alarmLineLightOpenSingel != value)
                {
                    _alarmLineLightOpenSingel = value;
                    RaisePropertyChanged(() => AlarmLineLightOpenSingel);
                }
            }
        }
        #endregion


        #region  本线路检测设备的末端 安装的灯杆序号
        private string _lduEndLampportSn;
        public string LduEndLampportSn
        {
            get { return _lduEndLampportSn; }
            set
            {
                if (_lduEndLampportSn != value)
                {
                    _lduEndLampportSn = value;
                    RaisePropertyChanged(() => LduEndLampportSn);
                }
            }
        }
        #endregion
        
        #region 备注
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (_remark != value)
                {
                    _remark = value;
                    RaisePropertyChanged(() => Remark);
                }
            }
        }
        #endregion 

        private readonly LduLineParameter _inmodel;
        public LduLineModel(LduLineParameter cnt, List<NameValueInt> list)
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
            LduLineID = cnt.LduLineId;
            LduLineName = cnt.LduLineName;
            LduLoopID = cnt.LduLoopId;
            LduPhase = cnt.LduPhase;
            MutualInductorRadio = cnt.MutualInductorRadio;
            AlarmLineBrightRate = cnt.AlarmLineBrightRate;
            AlarmLineLightOffImpedance = cnt.AlarmLineLightOffImpedance;
            AlarmLineLightOffSingle = cnt.AlarmLineLightOffSingle;
            AlarmLineLightOpenImpedance = cnt.AlarmLineLightOpenImpedance;
            AlarmLineLightOpenSingel = cnt.AlarmLineLightOpenSingel;
            AlarmLineLosePower = cnt.AlarmLineLosePower;
            AlarmLineShortCircuit = cnt.AlarmLineShortCircuit;
            AlarmAutoReport = cnt.AlarmAutoReport;
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
                               LduLineId = LduLineID,
                               LduLineName = LduLineName,
                               LduLoopId = LduLoopID,
                               LduPhase = LduPhase,
                               MutualInductorRadio = MutualInductorRadio,
                               Remark = Remark,
                               AlarmAutoReport = AlarmAutoReport
                           };
            return info;
        }

        #region 回路序号  本线路检测设备检测的终端回路的回路序号 ,,,,,,,
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
        #endregion 

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
