using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.EquipemntLightFault.UserFaultSettingByAdmin.ViewModel
{
    public class OneFaultTypeInfo : ObservableObject 
    {

        #region

        private int _faultCode;

        public int FaultCode
        {
            get { return _faultCode; }
            set
            {
                if (_faultCode != value)
                {
                    _faultCode = value;
                    this.RaisePropertyChanged(() => this.FaultCode);
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
                    this.RaisePropertyChanged(() => this.FaultNameByDefine);
                }
            }
        }

        private bool _isAlarm;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsAlarm
        {
            get { return _isAlarm; }
            set
            {
                if (_isAlarm != value)
                {
                    _isAlarm = value;
                    this.RaisePropertyChanged(() => this.IsAlarm);
                }
            }
        }


        private bool _isen;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsEnable
        {
            get { return _isen; }
            set
            {
                if (_isen != value)
                {
                    _isen = value;
                    this.RaisePropertyChanged(() => this.IsEnable);
                }
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
                    this.RaisePropertyChanged(() => this.FaultRemak);
                }
            }
        }


        private string _color;

        /// <summary>
        /// 颜色
        /// </summary>
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


        private int _alarmTimesVoiceAlarmTimes;

        public int AlarmTimes
        {
            get { return _alarmTimesVoiceAlarmTimes; }
            set
            {
                if (_alarmTimesVoiceAlarmTimes != value)
                {
                    _alarmTimesVoiceAlarmTimes = value;
                    this.RaisePropertyChanged(() => this.AlarmTimes);

                }
            }
        }

        #endregion

        public OneFaultTypeInfo(Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem tmlFaultType)
        {

            this.FaultCode = tmlFaultType.FaultCode;
            var info = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoById(FaultCode);
            if (info != null)
            {
                this.FaultNameByDefine = info.FaultNameByDefine;
                this.FaultRemak = info.FaultRemak;
                this.Color = info.Color;
            }

            this.IsAlarm = tmlFaultType.IsDisplay ;
            this.AlarmTimes = tmlFaultType.AlarmTimes;
        }

        public Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem  GetTmlFaultType()
        {
            return new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem() 
                       {
                           AlarmTimes = this.AlarmTimes,
                           FaultCode = this.FaultCode,
                           IsDisplay  = this.IsAlarm
                       };
        }

        //public Wlst.client.WlstFaultUserSelfDefineAlarm.WlstFaultUserSelfDefineAlarmItem GetIiTmlFaultType()
        //{
        //    return this ;
        //}




  
    }
}
