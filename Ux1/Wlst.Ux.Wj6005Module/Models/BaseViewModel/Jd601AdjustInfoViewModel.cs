using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Wlst.client;


namespace Wlst.Ux.Wj6005Module.Models.BaseViewModel
{
    public partial class Jd601AdjustInfoViewModel :
        Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged
        
    {
        #region

        public void UpdateVmInfo(Wlst.Sr.EquipmentInfoHolding.Model.Wj601Esu  info)
        {
            PhyId = info.RtuPhyId;
            RtuId = info.RtuId;
            RtuName = info.RtuName;
            AlarmDelay = info.WjEsu.EsuAlarmDelay;
            CloseTime = info.WjEsu.EsuCloseTime;
            CommTypeCode = info.WjEsu.EsuCommTypeCode;//.CommTypeCode;
            CtRadioA = info.WjEsu.EsuCtRadioA;//.CtRadioA;
            CtRadioB = info.WjEsu.EsuCtRadioB;// CtRadioB;
            CtRadioC = info.WjEsu.EsuCtRadioC;// CtRadioC;
            EnerySaveTemp = info.WjEsu.EsuEnerySaveTemp;// EnerySaveTemp;
            FanClosedTemp = info.WjEsu.EsuFanClosedTemp;// .EsuFanClosedTemp.FanClosedTemp;
            FanSatrtTemp = info.WjEsu.EsuFanSatrtTemp;//FanSatrtTemp;
            InputOvervoltageLimit = info.WjEsu.EsuInputOvervoltageLimit;//InputOvervoltageLimit;
            InputUndervoltageLimit = info.WjEsu.EsuInputUndervoltageLimit;//InputUndervoltageLimit;
            MandatoryProtectTemp = info.WjEsu.EsuMandatoryProtectTemp;//MandatoryProtectTemp;
            Mode = (int)info.WjEsu.EsuMode;
            OutputOverloadLimit = info.WjEsu.EsuOutputOverloadLimit;//OutputOverloadLimit;
            OutputUndervoltageLimit = info.WjEsu.EsuOutputUndervoltageLimit;//OutputUndervoltageLimit;
            PowerSupplyPhases = info.WjEsu.EsuPowerSupplyPhases;// PowerSupplyPhases;
            PreheatingTime = info.WjEsu.EsuPreheatingTime;//.PreheatingTime;
            RecoverEnergySaveTemp = info.WjEsu.EsuRecoverEnergySaveTemp;//RecoverEnergySaveTemp;
            RegulatingSpeed = info.WjEsu.EsuRegulatingSpeed;//.RegulatingSpeed;
            RunMode = info.WjEsu.EsuRunMode;// RunMode;
            TimeMode = info.WjEsu.EsuTimeMode;
            WorkMode = info.WjEsu.EsuWorkMode;//.WorkMode;
            EsyValidIdentify = info.WjEsu.EsyValidIdentify;// EsyValidIdentify;
            IsActiveAlarm = info.WjEsu.IsActiveAlarm;// IsActiveAlarm;
        }

        #endregion

        #region PrivateInfo

        /// <summary>
        /// 有效标示  指示是否使用
        /// </summary>

        private bool _EsyValidIdentify;

        /// <summary>
        /// 预热时间 默认2分钟
        /// </summary>

        private int _PreheatingTime;

        /// <summary>
        /// 开机时间 不设置
        /// </summary>

        private int _EsyOpentTime;

        /// <summary>
        /// 关机时间 不设置
        /// </summary>

        private int _CloseTime;

        /// <summary>
        /// A相接触器变比 50~500 默认150
        /// </summary>

        private int _CtRadioA;

        /// <summary>
        /// B相接触器变比 50~500 默认150
        /// </summary>

        private int _CtRadioB;

        /// <summary>
        /// C相接触器变比 50~500 默认150
        /// </summary>

        private int _CtRadioC;

        /// <summary>
        /// 时间模式：0 为定时模式 ；1为延时模式；默认1
        /// </summary>

        private int _TimeMode;

        /// <summary>
        /// 运行模式：0 自动，1 手动； 默认 0
        /// </summary>

        private int _RunMode;

        /// <summary>
        /// 风机启动温度 默认45
        /// </summary>

        private int _FanSatrtTemp;

        /// <summary>
        /// 风机关闭温度 默认35
        /// </summary>

        private int _FanClosedTemp;

        /// <summary>
        /// 退出节能温度 默认 70 界面设置
        /// </summary>

        private int _EnerySaveTemp;

        /// <summary>
        /// 强制保护温度 默认85
        /// </summary>

        private int _MandatoryProtectTemp;

        /// <summary>
        /// 恢复节能温度 默认50
        /// </summary>

        private int _RecoverEnergySaveTemp;

        /// <summary>
        /// 输入过压门限值 默认270
        /// </summary>

        private int _InputOvervoltageLimit;

        /// <summary>
        /// 输入欠压门限值 默认170
        /// </summary>

        private int _InputUndervoltageLimit;

        /// <summary>
        /// 输出欠压门限值 默认160
        /// </summary>

        private int _OutputUndervoltageLimit;

        /// <summary>
        /// 输入过载门限值 默认 144 电流
        /// </summary>

        private int _OutputOverloadLimit;

        /// <summary>
        /// 调压速度 仅模式为延时模式时有效 默认10 6~60
        /// </summary>

        private int _RegulatingSpeed;

        /// <summary>
        /// 供电相数  默认3 不提供界面设置
        /// </summary>

        private int _PowerSupplyPhases;

        /// <summary>
        /// 通信模式 ：0 通过终端；1 通过通信模块 默认0
        /// </summary>

        private int _CommTypeCode;

        /// <summary>
        /// 工作模式：0 通用模式；1 特殊模式；不提供界面设置 默认0
        /// </summary>

        private int _WorkMode;

        /// <summary>
        /// 是否主动报警 默认false
        /// </summary>

        private bool _IsActiveAlarm;

        /// <summary>
        /// 报警延时时间  默认10秒钟
        /// </summary>

        private int _AlarmDelay;

        /// <summary>
        /// 节能模式：0 接触器模式；1 IGBT模式 默认1
        /// </summary>

        private int _Mode;

        private int _RtuId
                    ;

        private int _PhyId
                    ;

        private string _RtuName;

        #endregion

        #region Attri

        /// <summary>
        /// 有效标示  指示是否使用
        /// </summary>

        public bool EsyValidIdentify
        {
            get { return _EsyValidIdentify; }
            set
            {
                if (_EsyValidIdentify == value) return;
                _EsyValidIdentify = value;
                this.RaisePropertyChanged(() => this.EsyValidIdentify);
            }
        }

        /// <summary>
        /// 预热时间 默认2分钟 2~30
        /// </summary>

        public int PreheatingTime
        {
            get { return _PreheatingTime; }
            set
            {
                if (_PreheatingTime == value) return;
                if (value < 2) return;
                if (value > 30) return;
                _PreheatingTime = value;
                this.RaisePropertyChanged(() => this.PreheatingTime);
            }
        }

        /// <summary>
        /// 开机时间 不设置
        /// </summary>

        public int EsyOpentTime
        {
            get { return _EsyOpentTime; }
            set
            {
                if (_EsyOpentTime == value) return;
                _EsyOpentTime = value;
                this.RaisePropertyChanged(() => this.EsyOpentTime);
            }
        }

        /// <summary>
        /// 关机时间 不设置
        /// </summary>

        public int CloseTime
        {
            get { return _CloseTime; }
            set
            {
                if (_CloseTime == value) return;
                _CloseTime = value;
                this.RaisePropertyChanged(() => this.CloseTime);
            }
        }

        /// <summary>
        /// A相接触器变比 50~500 默认150
        /// </summary>

        public int CtRadioA
        {
            get { return _CtRadioA; }
            set
            {
                if (_CtRadioA == value) return;
                if (value < 50) return;
                if (value > 500) return;
                if (value%5 != 0) return;
                _CtRadioA = value;
                this.RaisePropertyChanged(() => this.CtRadioA);
            }
        }

        /// <summary>
        /// B相接触器变比 50~500 默认150
        /// </summary>

        public int CtRadioB
        {
            get { return _CtRadioB; }
            set
            {
                if (_CtRadioB == value) return;
                if (value < 50) return;
                if (value > 500) return;
                if (value%5 != 0) return;
                _CtRadioB = value;
                this.RaisePropertyChanged(() => this.CtRadioB);
            }
        }

        /// <summary>
        /// C相接触器变比 50~500 默认150
        /// </summary>

        public int CtRadioC
        {
            get { return _CtRadioC; }
            set
            {
                if (_CtRadioC == value) return;
                if (value < 50) return;
                if (value > 500) return;
                if (value%5 != 0) return;
                _CtRadioC = value;
                this.RaisePropertyChanged(() => this.CtRadioC);
            }
        }

        /// <summary>
        /// 时间模式：0 为定时模式 ；1为延时模式；默认1
        /// </summary>

        public int TimeMode
        {
            get { return _TimeMode; }
            set
            {
                _TimeMode = value;
                if (value == 1)
                    IsTimeModeDelaySelected = true;
                else IsTimeModeDelaySelected = false;
            }
        }

        /// <summary>
        /// 运行模式：0 自动，1 手动； 默认 0
        /// </summary>

        public int RunMode
        {
            get { return _RunMode; }
            set
            {
                _RunMode = value;
                if (value == 0) IsRUnModeAutoSelected = true;
                else IsRUnModeAutoSelected = false;
            }
        }

        /// <summary>
        /// 风机启动温度 默认45 40~90
        /// </summary>

        public int FanSatrtTemp
        {
            get { return _FanSatrtTemp; }
            set
            {
                if (_FanSatrtTemp == value) return;
                if (value < 40) return;
                if (value > 90) return;
                _FanSatrtTemp = value;
                this.RaisePropertyChanged(() => this.FanSatrtTemp);
            }
        }

        /// <summary>
        /// 风机关闭温度 默认35  0~35
        /// </summary>

        public int FanClosedTemp
        {
            get { return _FanClosedTemp; }
            set
            {
                if (_FanClosedTemp == value) return;
                if (value > 35) return;
                _FanClosedTemp = value;
                this.RaisePropertyChanged(() => this.FanClosedTemp);
            }
        }

        /// <summary>
        /// 退出节能温度 默认 70 界面设置
        /// </summary>

        public int EnerySaveTemp
        {
            get { return 70; }
            set { _EnerySaveTemp = 70; }
        }

        /// <summary>
        /// 强制保护温度 默认85 40--85
        /// </summary>

        public int MandatoryProtectTemp
        {
            get { return _MandatoryProtectTemp; }
            set
            {
                if (_MandatoryProtectTemp == value) return;
                if (value > 85) return;
                if (value < 40) return;
                _MandatoryProtectTemp = value;
                this.RaisePropertyChanged(() => this.MandatoryProtectTemp);
            }
        }

        /// <summary>
        /// 恢复节能温度 默认50
        /// </summary>

        public int RecoverEnergySaveTemp
        {
            get { return _RecoverEnergySaveTemp; }
            set
            {
                if (_RecoverEnergySaveTemp == value) return;
                if (value > 99) return;
                _RecoverEnergySaveTemp = value;
                this.RaisePropertyChanged(() => this.RecoverEnergySaveTemp);
            }
        }

        /// <summary>
        /// 输入过压门限值 默认270
        /// </summary>

        public int InputOvervoltageLimit
        {
            get { return _InputOvervoltageLimit; }
            set
            {
                if (_InputOvervoltageLimit == value) return;
                if (value > 300) return;
                _InputOvervoltageLimit = value;
                this.RaisePropertyChanged(() => this.InputOvervoltageLimit);
            }
        }

        /// <summary>
        /// 输入欠压门限值 默认160
        /// </summary>

        public int InputUndervoltageLimit
        {
            get { return _InputUndervoltageLimit; }
            set
            {
                if (_InputUndervoltageLimit == value) return;
                if (value > 300) return;
                _InputUndervoltageLimit = value;
                this.RaisePropertyChanged(() => this.InputUndervoltageLimit);
            }
        }

        /// <summary>
        /// 输出欠压门限值 默认160
        /// </summary>

        public int OutputUndervoltageLimit
        {
            get { return _OutputUndervoltageLimit; }
            set
            {
                if (_OutputUndervoltageLimit == value) return;
                _OutputUndervoltageLimit = value;
                this.RaisePropertyChanged(() => this.OutputUndervoltageLimit);
            }
        }

        /// <summary>
        /// 输入过载门限值 默认 144 电流
        /// </summary>

        public int OutputOverloadLimit
        {
            get { return _OutputOverloadLimit; }
            set
            {
                if (_OutputOverloadLimit == value) return;
                if (value > 600) return;
                if (value < 0) return;
                _OutputOverloadLimit = value;
                this.RaisePropertyChanged(() => this.OutputOverloadLimit);
            }
        }

        /// <summary>
        /// 调压速度 仅模式为延时模式时有效 默认10 6~60
        /// </summary>

        public int RegulatingSpeed
        {
            get { return _RegulatingSpeed; }
            set
            {
                if (_RegulatingSpeed == value) return;
                if (value > 60) return;
                if (value < 6) return;
                _RegulatingSpeed = value;
                this.RaisePropertyChanged(() => this.RegulatingSpeed);
            }
        }

        /// <summary>
        /// 供电相数  默认3 不提供界面设置
        /// </summary>

        public int PowerSupplyPhases
        {
            get { return _PowerSupplyPhases; }
            set
            {
                if (_PowerSupplyPhases == value) return;
                _PowerSupplyPhases = value;
                this.RaisePropertyChanged(() => this.PowerSupplyPhases);
            }
        }

        /// <summary>
        /// 通信模式 ：0 通过终端；1 通过通信模块 默认0
        /// </summary>

        public int  CommTypeCode
        {
            get { return _CommTypeCode; }
            set
            {
                _CommTypeCode = value;
                if (value == 0) IsCommuModeThrouRtuSelected = true;
                else IsCommuModeThrouRtuSelected = false;
            }
        }

        /// <summary>
        /// 工作模式：0 通用模式；1 特殊模式；不提供界面设置 默认0
        /// </summary>

        public int WorkMode
        {
            get { return _WorkMode; }
            set
            {
                _WorkMode = value;
                if (value == 0) IsWorkModeNoramlSelected = true;
                else IsWorkModeNoramlSelected = false;
            }
        }

        /// <summary>
        /// 是否主动报警 默认false
        /// </summary>

        public bool IsActiveAlarm
        {
            get { return _IsActiveAlarm; }
            set
            {
                if (_IsActiveAlarm == value) return;
                _IsActiveAlarm = value;
                this.RaisePropertyChanged(() => this.IsActiveAlarm);
            }
        }

        /// <summary>
        /// 报警延时时间  默认10秒钟
        /// </summary>

        public int AlarmDelay
        {
            get { return _AlarmDelay; }
            set
            {
                if (_AlarmDelay == value) return;
                if (value > 199) return;
                if (value < 1) return;
                _AlarmDelay = value;
                this.RaisePropertyChanged(() => this.AlarmDelay);
            }
        }

        /// <summary>
        /// 节能模式：0 接触器模式；1 IGBT模式 默认1
        /// </summary>

        public int Mode
        {
            get { return _Mode; }
            set
            {
                _Mode = value;
                if (value == 1) IsEsuModeIgbtSelected = true;
                else IsEsuModeIgbtSelected = false;
            }
        }

        public int RtuId
        {
            get { return _RtuId; }
            set
            {
                if (_RtuId == value)
                    return;
                _RtuId = value;
                this.RaisePropertyChanged(() => this.RtuId);
            }
        }

        public int PhyId
        {
            get { return _PhyId; }
            set
            {
                if (_PhyId == value)
                    return;
                _PhyId = value;
                this.RaisePropertyChanged(() => this.PhyId);
            }
        }

        public string RtuName
        {
            get { return _RtuName; }
            set
            {
                if (_RtuName == value) return;
                _RtuName = value;
                this.RaisePropertyChanged(() => this.RtuName);
            }
        }

        #endregion

        #region conv

        private bool _IsTimeModeDelaySelected;

        public bool IsTimeModeDelaySelected
        {

            get { return _IsTimeModeDelaySelected; }
            set
            {
                if (_IsTimeModeDelaySelected == value) return;
                _IsTimeModeDelaySelected = value;
                this.RaisePropertyChanged(() => this.IsTimeModeDelaySelected);
            }
        }






        private bool _IsRUnModeAutoSelected;

        public bool IsRUnModeAutoSelected
        {

            get { return _IsRUnModeAutoSelected; }
            set
            {
                if (_IsRUnModeAutoSelected == value) return;
                _IsRUnModeAutoSelected = value;
                this.RaisePropertyChanged(() => this.IsRUnModeAutoSelected);
            }
        }



        private bool _IsCommuModeThrouRtuSelected;

        public bool IsCommuModeThrouRtuSelected
        {

            get { return _IsCommuModeThrouRtuSelected; }
            set
            {
                if (_IsCommuModeThrouRtuSelected == value) return;
                _IsCommuModeThrouRtuSelected = value;
                this.RaisePropertyChanged(() => this.IsCommuModeThrouRtuSelected);
            }
        }





        private bool _IsWorkModeNoramlSelected;

        public bool IsWorkModeNoramlSelected
        {

            get { return _IsWorkModeNoramlSelected; }
            set
            {
                if (_IsWorkModeNoramlSelected == value) return;
                _IsWorkModeNoramlSelected = value;
                this.RaisePropertyChanged(() => this.IsWorkModeNoramlSelected);
            }
        }



        private bool _IsEsuModeIgbtSelected;

        public bool IsEsuModeIgbtSelected
        {

            get { return _IsEsuModeIgbtSelected; }
            set
            {
                IsRegulatingSpeedEnable = value;


                if (_IsEsuModeIgbtSelected == value) return;
                _IsEsuModeIgbtSelected = value;
                this.RaisePropertyChanged(() => this.IsEsuModeIgbtSelected);


            }
        }

        #endregion

        #region IsEnable

        private bool _IsRegulatingSpeedEnable;

        public bool IsRegulatingSpeedEnable
        {

            get { return _IsRegulatingSpeedEnable; }
            set
            {
                if (_IsRegulatingSpeedEnable == value) return;
                _IsRegulatingSpeedEnable = value;
                this.RaisePropertyChanged(() => this.IsRegulatingSpeedEnable);
            }
        }


        #endregion

        public EsuParameter GetSetIIJd601()
        {
            return new EsuParameter()
                       {
                            EsuAlarmDelay = AlarmDelay   ,
                            EsuCloseTime  = CloseTime,
                            EsuCtRadioA = CtRadioA,
                            EsuCtRadioB = CtRadioB,
                            EsuCtRadioC = CtRadioC,
                            EsuEnerySaveTemp =EnerySaveTemp,
                            EsuFanClosedTemp = FanClosedTemp,
                            EsuFanSatrtTemp = FanSatrtTemp,
                            EsuInputOvervoltageLimit =InputOvervoltageLimit,
                            EsuInputUndervoltageLimit = InputUndervoltageLimit,
                            EsuMandatoryProtectTemp = MandatoryProtectTemp,
                            EsuOutputOverloadLimit = OutputOverloadLimit,
                            EsuOutputUndervoltageLimit = OutputUndervoltageLimit,
                            EsuPowerSupplyPhases = PowerSupplyPhases,
                            EsuPreheatingTime = PreheatingTime,
                            EsuRecoverEnergySaveTemp =RecoverEnergySaveTemp,
                            EsuRegulatingSpeed = RegulatingSpeed,
                            EsyValidIdentify = EsyValidIdentify,
                            IsActiveAlarm = IsActiveAlarm,
                            RtuId = RtuId,
                            EsuCommTypeCode = IsCommuModeThrouRtuSelected?0:1,
                            EsuMode = IsEsuModeIgbtSelected?1:0,
                            EsuRunMode=IsRUnModeAutoSelected?0:1,
                            EsuTimeMode = IsTimeModeDelaySelected?1:0,
                            EsuWorkMode = IsWorkModeNoramlSelected?0:1,
                       };
           
            
            //if (IsCommuModeThrouRtuSelected) CommTypeCode = 0 ;
            //else CommTypeCode = 1;
            //if (IsEsuModeIgbtSelected) Mode = 1;
            //else Mode = 0;
            //if (IsRUnModeAutoSelected) RunMode = 0;
            //else RunMode = 1;
            //if (IsTimeModeDelaySelected) TimeMode = 1;
            //else TimeMode = 0;
            //if (IsWorkModeNoramlSelected) WorkMode = 0;
            //else WorkMode = 1;
            //return this;
        }
    }

    public partial class Jd601AdjustInfoViewModel
    {
        private ObservableCollection<Jd601ParViewModel> _jd601ParItems;

        public ObservableCollection<Jd601ParViewModel> Jd601ParItems
        {
            get
            {
                if (_jd601ParItems == null)
                {
                    _jd601ParItems = new ObservableCollection<Jd601ParViewModel>();
                    for (int i = 1; i < 9; i++)
                    {
                        _jd601ParItems.Add(new Jd601ParViewModel()
                                               {
                                                   EsuOperateId = i,
                                                   EsuOperateTimeHour = 0,
                                                   EsuOperateTimeMinute = 0,
                                                   EsuOperatoeValue = 220
                                               });
                    }
                }
                return _jd601ParItems;
            }
        }

        public void OnLoadJd601ParVm()
        {
            Jd601ParItems.Clear();
            for (int i = 1; i < 9; i++)
            {
                Jd601ParItems.Add(new Jd601ParViewModel()
                                      {
                                          EsuOperateId = i,
                                          EsuOperateTimeHour = 0,
                                          EsuOperateTimeMinute = 0,
                                          EsuOperatoeValue = 220
                                      });
            }
        }

        public void UpdateJd601Vm(List<ReplyEsyPar.Jd601Par> lst)
        {
            foreach (var t in lst)
            {
                foreach (var g in this.Jd601ParItems)
                {
                    if (t.EsuOperateId == g.EsuOperateId)
                    {
                        g.UpdateInfo(t);
                        break;
                    }
                }
            }
        }

        public List<ReplyEsyPar.Jd601Par> BackJd601Par()
        {
            var lst = new List<ReplyEsyPar.Jd601Par>();
            foreach (var t in this.Jd601ParItems)
            {
                if (t.EsuOperateTimeHour > 0 || t.EsuOperateTimeMinute > 0)
                {
                    var infff = t.BackToPar();
                    infff.RtuId = this.RtuId;
                    lst.Add(infff );
                }
            }
            return lst;
        }
    }
}
