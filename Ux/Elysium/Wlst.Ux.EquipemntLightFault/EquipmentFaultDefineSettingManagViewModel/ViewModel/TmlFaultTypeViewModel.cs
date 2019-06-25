using System;
using System.Collections.ObjectModel;
using System.Windows;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.ProtocolCnt.Fault;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingManagViewModel.ViewModel
{
    [Serializable]
    public class PriorityLevelHelper : ObservableObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private PriorityLevelType _priority;

        public PriorityLevelType Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    RaisePropertyChanged(() => Priority);
                }
            }
        }
    }

    [Serializable]
    public class TmlFaultTypeViewModel : ObservableObject, IIEquipmentFaultType
    {

        #region

        private int _faultId;

        public int FaultId
        {
            get { return _faultId; }
            set
            {
                if (_faultId != value)
                {
                    _faultId = value;
                    RaisePropertyChanged(() => FaultId);
                }
            }
        }
        
             private bool  _checkBoxIsEnable;

             public bool CheckBoxIsEnable
        {
            get { return _checkBoxIsEnable; }
            set
            {
                if (_checkBoxIsEnable != value)
                {
                    _checkBoxIsEnable = value;
                    RaisePropertyChanged(() => CheckBoxIsEnable);
                }
            }
        }

        private PriorityLevelType _priorityLevel;

        /// <summary>
        /// 报警时间段选中，1 全天；2  开灯时间段，3 关灯时间段，4 自定义时间
        /// </summary>
        public PriorityLevelType PriorityLevel
        {
            get { return _priorityLevel; }
            set
            {
                if (_priorityLevel != value)
                {
                    _priorityLevel = value;
                    RaisePropertyChanged(() => PriorityLevel);
                    foreach (var ff in CollectionProprity)
                    {
                        if (ff.Priority == _priorityLevel)
                        {
                            CurrentSelectProprity = ff;
                            break;
                        }
                    }
                }
            }
        }

        #region assist OnState show

        private ObservableCollection<PriorityLevelHelper> _ollectionProprity;
        //辅助显示接触器通时的显示名称所有选择
        public ObservableCollection<PriorityLevelHelper> CollectionProprity
        {
            get
            {
                return _ollectionProprity ?? (_ollectionProprity = new ObservableCollection<PriorityLevelHelper>
                                                                       {
                                                                           new PriorityLevelHelper
                                                                               {
                                                                                   Name = "低",
                                                                                   Priority = PriorityLevelType.Low
                                                                               },
                                                                           new PriorityLevelHelper
                                                                               {
                                                                                   Name = "中",
                                                                                   Priority = PriorityLevelType.Normal
                                                                               },
                                                                           new PriorityLevelHelper
                                                                               {
                                                                                   Name = "高",
                                                                                   Priority = PriorityLevelType.High
                                                                               }
                                                                       });
            }
        }

        private PriorityLevelHelper _currentSelectProprity;

        //辅助显示接触器通时的显示名称当前选择
        public PriorityLevelHelper CurrentSelectProprity
        {
            get { return _currentSelectProprity; }
            set
            {
                if (_currentSelectProprity != value)
                {
                    _currentSelectProprity = value;
                    RaisePropertyChanged(() => CurrentSelectProprity);
                    PriorityLevel = _currentSelectProprity.Priority;
                }
            }
        }

        #endregion

        private FaultWarmType _faultType;

        public FaultWarmType FaultType
        {
            get { return _faultType; }
            set
            {
                
                _faultType = value;
                if (_faultType == FaultWarmType.Slu)
                {
                    FaultTypeName = "单灯设备故障";
                }
                else if (_faultType == FaultWarmType.Als)
                {
                    FaultTypeName = "光控设备故障";
                }
                else if (_faultType == FaultWarmType.Esu)
                {
                    FaultTypeName = "节能设备故障";
                }
                else if (_faultType == FaultWarmType.Ldu)
                {
                    FaultTypeName = "线路检查故障";
                }
                else if (_faultType == FaultWarmType.Mru)
                {
                    FaultTypeName = "抄表设备故障";
                }
                else if (_faultType == FaultWarmType.Rtu)
                {
                    FaultTypeName = "终端设备故障";
                }
                else if (_faultType == FaultWarmType.Sse)
                {
                    FaultTypeName = "自定义故障";
                    IsSelfDefineFault = Visibility.Visible;
                }
            }
        }

        private Visibility _isSelfDefineFault;

        public Visibility IsSelfDefineFault
        {
            get { return _isSelfDefineFault; }
            set
            {
                if (_isSelfDefineFault != value)
                {
                    _isSelfDefineFault = value;
                    RaisePropertyChanged(() => IsSelfDefineFault);
                }
            }
        }

        private string _faultTypeName;

        public string FaultTypeName
        {
            get { return _faultTypeName; }
            set
            {
                if (_faultTypeName != value)
                {
                    _faultTypeName = value;
                    RaisePropertyChanged(() => FaultTypeName);
                }
            }
        }



        private bool _switchStateAlwaysKeepOn;
        public bool SwitchStateAlwaysKeepOn
        {
            get { return _switchStateAlwaysKeepOn; }
            set
            {
                if (_switchStateAlwaysKeepOn != value)
                {
                    _switchStateAlwaysKeepOn = value;
                    RaisePropertyChanged(() => SwitchStateAlwaysKeepOn);
                }
            }
        }


        private string _faultCheckKey;
        public string FaultCheckKey
        {
            get { return _faultCheckKey; }
            set
            {
                if (_faultCheckKey != value)
                {
                    _faultCheckKey = value;
                    RaisePropertyChanged(() => FaultCheckKey);
                }
            }
        }


        private string _faultName;

        /// <summary>
        /// 故障原始名称
        /// </summary>
        public string FaultName
        {
            get { return _faultName; }
            set
            {
                if (_faultName != value)
                {
                    _faultName = value;
                    RaisePropertyChanged(() => FaultName);
                }
            }
        }

        private string _faultNameByDefine;

        /// <summary>
        /// 故障自定名称
        /// </summary>
        public string FaultNameByDefine
        {
            get { return _faultNameByDefine; }
            set
            {
                if (_faultNameByDefine != value)
                {
                    _faultNameByDefine = value;
                    RaisePropertyChanged(() => FaultNameByDefine);
                }
            }
        }

        private bool _isEnable;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (_isEnable != value)
                {
                    _isEnable = value;
                    RaisePropertyChanged(() => IsEnable);
                    if(!_isEnable )
                    {
                        IsTimeEnable = false;
                        AlarmTimeType = 1;
                    }
                }
            }
        }

        private bool _isEnableEnable=true;
        public bool IsEnableEnable
        {
            get { return _isEnableEnable; }
            set
            {
                if (value == _isEnableEnable) return;
                _isEnableEnable = value;
                RaisePropertyChanged(()=>IsEnableEnable);
                if (IsEnableEnable) return;
                IsTimeEnable = false;
                CheckBoxIsEnable = false;
                
            }
        }

        private string _faultRemak;

        /// <summary>
        /// 故障备注信息
        /// </summary>
        public string FaultRemak
        {
            get { return _faultRemak; }
            set
            {
                if (_faultRemak != value)
                {
                    _faultRemak = value;
                    RaisePropertyChanged(() => FaultRemak);
                }
            }
        }

        private string  _color;


        /// <summary>
        /// Color
        /// </summary>
        public string  Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {

                    _color = value;
                    RaisePropertyChanged(() => Color);
                }
            }
        }


        private int _alarmTimeType;

        /// <summary>
        /// 报警时间段选中，0 全天；1  开灯时间段，2 关灯时间段，3 自定义时间
        /// </summary>
        public int AlarmTimeType
        {
            get { return _alarmTimeType; }
            set
            {
                if (_alarmTimeType != value)
                {
                    _alarmTimeType = value;
                    RaisePropertyChanged(() => AlarmTimeType);
                    IsTimeEnable = _alarmTimeType == 3;

                    for (int t = 0; t < CollectionAlarmTimeType.Count; t++)
                    {
                        if (CollectionAlarmTimeType[t].Value != _alarmTimeType)
                            continue;
                        SelectAlarmTimeTypeIndex = t;
                        break;
                    }
                }
            }
        }
        #region assist OnState show

        private ObservableCollection<NameValueInt> _collectionAlarmTimeType;
        //辅助显示接触器通时的显示名称所有选择
        public ObservableCollection<NameValueInt> CollectionAlarmTimeType
        {
            get
            {
                return _collectionAlarmTimeType ?? (_collectionAlarmTimeType = new ObservableCollection<NameValueInt>
                                                                                   {
                                                                                       new NameValueInt {Name = "全天", Value = 0},
                                                                                       new NameValueInt {Name = "开灯期间", Value = 1},
                                                                                       new NameValueInt {Name = "关灯期间", Value = 2},
                                                                                       new NameValueInt {Name = "自定义时间", Value = 3}
                                                                                   });
            }
        }

        private int _selectAlarmTimeTypeIndex;

        //辅助显示接触器通时的显示名称当前选择
        public int SelectAlarmTimeTypeIndex
        {
            get
            {
                return _selectAlarmTimeTypeIndex;
            }
            set
            {
                if (_selectAlarmTimeTypeIndex != value)
                {
                    _selectAlarmTimeTypeIndex = value;

                    RaisePropertyChanged(() => SelectAlarmTimeTypeIndex);
                    AlarmTimeType = CollectionAlarmTimeType[value].Value;
                    AlarmTimeTypeName = CollectionAlarmTimeType[value].Name;
                }
            }
        }

        private string _alarmTimeTypeName;
        public string AlarmTimeTypeName
        {
            get { return _alarmTimeTypeName; }
            set
            {
                if (_alarmTimeTypeName == value) return;
                _alarmTimeTypeName = value;
                RaisePropertyChanged(()=>AlarmTimeTypeName);
            }
        }

        #endregion

        private bool  _isTimeEnable;

        /// <summary>
        /// 时间设置是否有效
        /// </summary>
        public bool  IsTimeEnable
        {
            get { return _isTimeEnable; }
            set
            {
                if (_isTimeEnable != value)
                {
                    _isTimeEnable = value;
                    if(!_isEnable )
                    {
                        _isTimeEnable = false;
                    }
                    RaisePropertyChanged(() => IsTimeEnable);

                    if(_isTimeEnable )
                    {
                        HourStartAlarm = 1;
                        HourEndAlarm = 1;
                        MinuteEndAlarm = 1;
                        MinuteStartAlarm = 1;
                    }
                    else
                    {
                        HourStartAlarm = 0;
                        HourEndAlarm = 0;
                        MinuteEndAlarm = 0;
                        MinuteStartAlarm = 0;
                    }
                }
            }
        }

        private int _hourStartAlarm;

        /// <summary>
        /// 如果为自定义时间段则开始报警时
        /// </summary>
        public int HourStartAlarm
        {
            get { return _hourStartAlarm; }
            set
            {
                if (_hourStartAlarm != value)
                {
                    _hourStartAlarm = value;
                    RaisePropertyChanged(() => HourStartAlarm);
                }
            }
        }

        private int _hourEndAlarm;

        public int HourEndAlarm
        {
            get { return _hourEndAlarm; }
            set
            {
                if (_hourEndAlarm != value)
                {
                    _hourEndAlarm = value;
                    RaisePropertyChanged(() => HourEndAlarm);
                }
            }
        }

        private int _minuteStartAlarm;

        public int MinuteStartAlarm
        {
            get { return _minuteStartAlarm; }
            set
            {
                if (_minuteStartAlarm != value)
                {
                    _minuteStartAlarm = value;
                    RaisePropertyChanged(() => MinuteStartAlarm);
                }
            }
        }

        private int _minuteEndAlarm;

        public int MinuteEndAlarm
        {
            get { return _minuteEndAlarm; }
            set
            {
                if (_minuteEndAlarm != value)
                {
                    _minuteEndAlarm = value;
                    RaisePropertyChanged(() => MinuteEndAlarm);
                }
            }
        }

        #endregion




        public TmlFaultTypeViewModel(IIEquipmentFaultType tmlFaultType)
        {
            IsTimeEnable = false;
            AlarmTimeType = 1;
            IsSelfDefineFault = Visibility.Collapsed;
            AlarmTimeType = tmlFaultType.AlarmTimeType;
            FaultId = tmlFaultType.FaultId;
            FaultName = tmlFaultType.FaultName;
            FaultNameByDefine = tmlFaultType.FaultNameByDefine;
            FaultRemak = tmlFaultType.FaultRemak;
            HourEndAlarm = tmlFaultType.HourEndAlarm;
            HourStartAlarm = tmlFaultType.HourStartAlarm;
            MinuteEndAlarm = tmlFaultType.MinuteEndAlarm;
            MinuteStartAlarm = tmlFaultType.MinuteStartAlarm;
            IsEnable = tmlFaultType.IsEnable;
            PriorityLevel = tmlFaultType.PriorityLevel;
            Color = tmlFaultType.Color;
            FaultType = tmlFaultType.FaultType;
            SwitchStateAlwaysKeepOn = tmlFaultType.SwitchStateAlwaysKeepOn;
            FaultCheckKey = tmlFaultType.FaultCheckKey;

            if (FaultId < 6)
            {
                CheckBoxIsEnable = false;
                IsEnable = true;
            }
            else CheckBoxIsEnable = true;

            IsEnableEnable = tmlFaultType.IsEnableEnable;
        }

        public EquipmentFaultType GetTmlFaultType()
        {
            var tmlfaule = new EquipmentFaultType(this);
            return tmlfaule;
        }

        public IIEquipmentFaultType GetIiTmlFaultType()
        {
            var tmlfaule = new EquipmentFaultType(this);
            return tmlfaule;
        }


    }
}
