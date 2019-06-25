using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Documents;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipemntLightFault.Model;



namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.ViewModel
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
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        private int  _priority;

        public int Priority
        {
            get { return _priority; }
            set
            {
                if (_priority != value)
                {
                    _priority = value;
                    this.RaisePropertyChanged(() => this.Priority);
                }
            }
        }
    }

    [Serializable]
    public class TmlFaultTypeViewModel : ObservableObject//, IIEquipmentFaultType
    {

        #region

        public class NameColor : ObservableObject
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
                        this.RaisePropertyChanged(() => this.Name);
                    }
                }
            }



            private string _color;

            public string Color
            {
                get { return _color; }
                set
                {
                    if (_color != value)
                    {
                        _color = value;
                        this.RaisePropertyChanged(() => this.Color);
                    }
                }
            }

            private int _index;

            public int Index
            {
                get { return _index; }
                set
                {
                    if (_index != value)
                    {
                        _index = value;
                        this.RaisePropertyChanged(() => this.Index);
                    }
                }
            }
        }

        private int _faultId;

        public int FaultId
        {
            get { return _faultId; }
            set
            {
                if (_faultId != value)
                {
                    _faultId = value;
                    this.RaisePropertyChanged(() => this.FaultId);

                    if(_faultId ==0 ||_faultId >80)
                    {
                        IsSelfDefineFault = Visibility.Visible;
                    }
                    else
                    {
                        IsSelfDefineFault = Visibility.Collapsed;
                    }
                }
            }
        }
        
        private bool  _CheckBoxIsEnable;

        public bool CheckBoxIsEnable
        {
            get { return _CheckBoxIsEnable; }
            set
            {
                if (_CheckBoxIsEnable != value)
                {
                    _CheckBoxIsEnable = value;
                    this.RaisePropertyChanged(() => this.CheckBoxIsEnable);
                }
            }
        }

        private int  _priorityLevel;

        /// <summary>
        /// 报警等级
        /// </summary>
        public int PriorityLevel
        {
            get { return _priorityLevel; }
            set
            {
                if (_priorityLevel != value)
                {
                    _priorityLevel = value;
                    this.RaisePropertyChanged(() => this.PriorityLevel);
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
       
        public ObservableCollection<PriorityLevelHelper> CollectionProprity
        {
            get
            {
                if (_ollectionProprity == null)
                {
                    _ollectionProprity = new ObservableCollection<PriorityLevelHelper>();
                    _ollectionProprity.Add(new PriorityLevelHelper() { Name = "仅记录",Priority = 1});
                    _ollectionProprity.Add(new PriorityLevelHelper() { Name = "普通报警", Priority = 2 });
                    _ollectionProprity.Add(new PriorityLevelHelper() { Name = "置顶显示", Priority = 3 });
                }

                return _ollectionProprity;
            }
        }

        private PriorityLevelHelper _currentSelectProprity;

        public PriorityLevelHelper CurrentSelectProprity
        {
            get { return _currentSelectProprity; }
            set
            {
                if (_currentSelectProprity != value)
                {
                    _currentSelectProprity = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectProprity);
                    PriorityLevel = _currentSelectProprity.Priority;
                }
            }
        }

        #endregion

  
        private Visibility _isSelfDefineFault;

        public Visibility IsSelfDefineFault
        {
            get { return _isSelfDefineFault; }
            set
            {
                if (_isSelfDefineFault != value)
                {
                    _isSelfDefineFault = value;
                    this.RaisePropertyChanged(() => this.IsSelfDefineFault);
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
                    this.RaisePropertyChanged(() => this.FaultTypeName);
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
                    this.RaisePropertyChanged(() => this.SwitchStateAlwaysKeepOn);
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
                    this.RaisePropertyChanged(() => this.FaultCheckKey);
                }
            }
        }


        private string faultName;

        /// <summary>
        /// 故障原始名称
        /// </summary>
        public string FaultName
        {
            get { return faultName; }
            set
            {
                if (faultName != value)
                {
                    faultName = value;
                    this.RaisePropertyChanged(() => this.FaultName);
                }
            }
        }

        private string faultNameByDefine;

        /// <summary>
        /// 故障自定名称
        /// </summary>

        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        public string FaultNameByDefine
        {
            get { return faultNameByDefine; }
            set
            {
                if (faultNameByDefine != value)
                {
                    faultNameByDefine = value;
                    this.RaisePropertyChanged(() => this.FaultNameByDefine);
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
                    this.RaisePropertyChanged(() => this.IsEnable);
                    if (!_isEnable )
                    {
                        IsTimeEnable = false;
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
                this.RaisePropertyChanged(()=>this.IsEnableEnable);
                if (IsEnableEnable) return;
                //IsTimeEnable = false;
               // CheckBoxIsEnable = false;
                
            }
        }

        private string faultRemak;

        /// <summary>
        /// 故障备注信息
        /// </summary>
        public string FaultRemak
        {
            get { return faultRemak; }
            set
            {
                if (faultRemak != value)
                {
                    faultRemak = value;
                    this.RaisePropertyChanged(() => this.FaultRemak);
                }
            }
        }

        private string color;

       public string Color
        {
            get { return color; }
            set
            {
                if (color != value)
                {
                    color = value;
                    this.RaisePropertyChanged(() => this.Color);

                    foreach (var ff in CollectionColor)
                    {
                        if (ff.Color == color)
                        {
                            SelectColor = ff;
                            break;
                        }
                    }
                }
            }
        }


        private NameColor selectcolor;


        /// <summary>
        /// Color
        /// </summary>
        public NameColor SelectColor
        {
            get { return selectcolor; }
            set
            {
                if (selectcolor != value)
                {

                    selectcolor = value;
                    this.RaisePropertyChanged(() => this.SelectColor);
                    Color = value.Color;

                }
            }
        }

        private ObservableCollection<NameColor> _collectionColor;

        public ObservableCollection<NameColor> CollectionColor
        {
            get
            {
                if (_collectionColor == null)
                {
                    _collectionColor = new ObservableCollection<NameColor>();
                    _collectionColor.Add(new NameColor() { Name = "黑色", Color = "#FF000000",Index = 0 });
                    _collectionColor.Add(new NameColor() { Name = "白色", Color = "#FFFFFFFF",Index = 1 });
                    _collectionColor.Add(new NameColor() { Name = "黄色", Color = "#FFFFFF00",Index = 2 });
                    _collectionColor.Add(new NameColor() { Name = "红色", Color = "#FFFF0000",Index = 3 });
                    _collectionColor.Add(new NameColor() { Name = "淡蓝", Color = "#FF00BFFF",Index = 4 });
                    _collectionColor.Add(new NameColor() { Name = "橙色", Color = "#FFFF4500",Index = 5 });
                }

                return _collectionColor;
            }
        }

        private int alarmTimeType;

        /// <summary>
        /// 报警时间段选中，0 全天；1  开灯时间段，2 关灯时间段，3 自定义时间
        /// </summary>
        public int AlarmTimeType
        {
            get { return alarmTimeType; }
            set
            {
                if (alarmTimeType != value)
                {
                    alarmTimeType = value;
                    this.RaisePropertyChanged(() => this.AlarmTimeType);
                    if (alarmTimeType == 3) IsTimeEnable = true;
                    else IsTimeEnable = false;

                    //for (int t = 0; t < CollectionAlarmTimeType.Count; t++)
                    //{
                    //    if (CollectionAlarmTimeType[t].Value != alarmTimeType)
                    //        continue;
                    //    SelectAlarmTimeTypeIndex = CollectionAlarmTimeType[t];
                    //    break;
                    //}

                    foreach (var ff in CollectionAlarmTimeType)
                    {
                        if (ff.Value == alarmTimeType)
                        {
                            SelectAlarmTimeTypeIndex = ff;
                            break;
                        }
                    }
                }
            }
        }
        #region assist OnState show

        private ObservableCollection<NameValueInt> _collectionAlarmTimeType;
        
        public ObservableCollection<NameValueInt> CollectionAlarmTimeType
        {
            get
            {
                if (_collectionAlarmTimeType == null)
                {
                    _collectionAlarmTimeType = new ObservableCollection<NameValueInt>();
                    _collectionAlarmTimeType.Add(new NameValueInt() {Name = "全天", Value = 0});
                    _collectionAlarmTimeType.Add(new NameValueInt() {Name = "开灯期间", Value = 1});
                    _collectionAlarmTimeType.Add(new NameValueInt() {Name = "关灯期间", Value = 2});
                    _collectionAlarmTimeType.Add(new NameValueInt() {Name = "自定义时间", Value = 3});
                }

                return _collectionAlarmTimeType;
            }
        }

        private NameValueInt _SelectAlarmTimeTypeIndex;

        public NameValueInt SelectAlarmTimeTypeIndex
        {
            get
            {
                return _SelectAlarmTimeTypeIndex;
            }
            set
            {
                if (_SelectAlarmTimeTypeIndex != value)
                {
                    _SelectAlarmTimeTypeIndex = value;

                    this.RaisePropertyChanged(() => this.SelectAlarmTimeTypeIndex);
                    //AlarmTimeType = CollectionAlarmTimeType[value.Value].Value;
                    AlarmTimeType = value.Value;
                }
            }
        }

        #endregion

        private bool  isTimeEnable;

        /// <summary>
        /// 时间设置是否有效
        /// </summary>
        public bool  IsTimeEnable
        {
            get { return isTimeEnable; }
            set
            {
                if (isTimeEnable != value)
                {
                    isTimeEnable = value;
                    //if (!_isEnable)
                    //{
                    //    isTimeEnable = false;
                    //}
                    this.RaisePropertyChanged(() => this.IsTimeEnable);

                    //if(isTimeEnable )
                    //{
                    //    this.HourStartAlarm = 1;
                    //    this.HourEndAlarm = 1;
                    //    this.MinuteEndAlarm = 1;
                    //    this.MinuteStartAlarm = 1;
                    //}
                    //else
                    //{
                    //    this.HourStartAlarm = 0;
                    //    this.HourEndAlarm = 0;
                    //    this.MinuteEndAlarm = 0;
                    //    this.MinuteStartAlarm = 0;
                    //}
                }
            }
        }

        private int hourStartAlarm;

        /// <summary>
        /// 如果为自定义时间段则开始报警时
        /// </summary>
        public int HourStartAlarm
        {
            get { return hourStartAlarm; }
            set
            {
                if (hourStartAlarm != value)
                {
                    if (value > 23 || value < 0) value = 0;
                    hourStartAlarm = value;
                    this.RaisePropertyChanged(() => this.HourStartAlarm);
                }
            }
        }

        private int hourEndAlarm;

        public int HourEndAlarm
        {
            get { return hourEndAlarm; }
            set
            {
                if (hourEndAlarm != value)
                {
                    if (value > 23 || value < 0) value = 0;
                    hourEndAlarm = value;
                    this.RaisePropertyChanged(() => this.HourEndAlarm);
                }
            }
        }

        private int minuteStartAlarm;

        public int MinuteStartAlarm
        {
            get { return minuteStartAlarm; }
            set
            {
                if (minuteStartAlarm != value)
                {
                    if (value > 59 || value < 0) value = 0;
                    minuteStartAlarm = value;
                    this.RaisePropertyChanged(() => this.MinuteStartAlarm);
                }
            }
        }

        private int minuteEndAlarm;

        public int MinuteEndAlarm
        {
            get { return minuteEndAlarm; }
            set
            {
                if (minuteEndAlarm != value)
                {
                    if (value > 59 || value < 0) value = 0;
                    minuteEndAlarm = value;
                    this.RaisePropertyChanged(() => this.MinuteEndAlarm);
                }
            }
        }

        #endregion




        public TmlFaultTypeViewModel(Wlst .client.FaultTypes  .FaultTypeItem   tmlFaultType)
        {
            this.AlarmTimeType = tmlFaultType.AlarmTimeSet;
            if (AlarmTimeType == 3)
            {
                this.IsTimeEnable = true;
                this.HourStartAlarm = tmlFaultType.AlarmTimeStart / 60;
                this.MinuteStartAlarm = tmlFaultType.AlarmTimeStart - HourStartAlarm * 60;
                this.HourEndAlarm = tmlFaultType.AlarmTimeEnd / 60;
                this.MinuteEndAlarm = tmlFaultType.AlarmTimeEnd - HourEndAlarm * 60;
            }
            else this.IsTimeEnable = false;
            foreach (var ff in CollectionAlarmTimeType)
            {
                if (ff.Value == this.AlarmTimeType)
                {
                    SelectAlarmTimeTypeIndex = ff;
                    break;
                }
            }

            this.IsSelfDefineFault = Visibility.Collapsed;

            this.FaultId = tmlFaultType.FaultId;
            if(FaultId ==0)
            {
                IsSelfDefineFault = Visibility.Visible;
            }

            this.FaultName = tmlFaultType.FaultName;
            this.FaultNameByDefine = tmlFaultType.FaultNameByDefine;
            this.FaultRemak = tmlFaultType.FaultRemak;

            this.IsEnable = tmlFaultType.IsEnable;
            this.PriorityLevel = tmlFaultType.PriorityLevel;

            if (tmlFaultType.Color != "#00FFFFFF") this.Color = tmlFaultType.Color;
            else this.Color = "#FFFFFFFF";
 
            this.FaultCheckKey = tmlFaultType.FaultCheckKey;

            //if (this.FaultId < 6)
            //{
            //   // CheckBoxIsEnable = false;
            //    IsEnable = true;
            //}
           // else CheckBoxIsEnable = true;
            CheckBoxIsEnable = true;
        //    IsEnableEnable = tmlFaultType.IsEnable;

            if (this.FaultId == 9 || this.FaultId == 10 || this.FaultId == 11 || this.FaultId == 15 || this.FaultId == 16 || this.FaultId == 17 || this.FaultId == 20) 
                CollectionAlarmTimeType.RemoveAt(2);
            if (this.FaultId == 12 || this.FaultId == 13 || this.FaultId == 14 || this.FaultId == 21) 
                CollectionAlarmTimeType.RemoveAt(1);
        }

        public Wlst.client.FaultTypes.FaultTypeItem  GetTmlFaultType()
        {
            var tmlfaule = new Wlst.client.FaultTypes.FaultTypeItem()
                               {
                                   Color = this.color,
                                   FaultId = this.FaultId,
                                   FaultCheckKey = this.FaultCheckKey,
                                   FaultName = this.FaultName,
                                   FaultNameByDefine = this.FaultNameByDefine,
                                   FaultRemak = this.FaultRemak,
                                   IsEnable = this.IsEnable,
                                   PriorityLevel = (int) this.PriorityLevel,
                                   AlarmTimeSet = AlarmTimeType,
                                   AlarmTimeEnd = HourEndAlarm*60+MinuteEndAlarm,
                                   AlarmTimeStart = HourStartAlarm*60+MinuteStartAlarm,
                               };
            return tmlfaule;
        }

        //public Wlst.client.WlstFaultType GetIiTmlFaultType()
        //{
        //    var tmlfaule = new Wlst.client.WlstFaultType(this);
        //    return tmlfaule;
        //}


    }
}
