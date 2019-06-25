using System;
using System.Globalization;
using Wlst.client;


namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.Jd601OperatorControl.ViewModel
{
    public class EsuDataOneItemViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        #region private attri

        private string _value1;
        private string _value2;
        private string _value3;
        private string _value4;
        private string _value5;
        private string _value6;
        private string _value7;
        private string _value8;
        private string _value9;
        private string _value10;
        private string _value11;
        private string _value12;
        private string _value13;
        private string _value14;
        private string _value15;
        private string _value16;
        private string _value17;
        private string _value18;
        private string _value19;
        private string _value20;
        private string _value21;
        private string _value22;
        private string _value23;
        private string _value24;
        private string _value25;

        #endregion

        #region attri


        private string id;

        /// <summary>
        /// 辅助值 自增属性
        /// </summary>

        public string Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }

            }
        }

        private int _rtuId;

        /// <summary>
        /// 节能设备地址
        /// </summary>

        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);

                    var mmm = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                   _rtuId);
                    if (mmm != null)
                    {
                        RtuName = mmm.RtuName;
                        return;
                    }
                    RtuName = "未知";

                }

            }
        }

        private string _rtuName;

        /// <summary>
        /// 节能设备
        /// </summary>

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value == _rtuName) return;
                _rtuName = value;
                RaisePropertyChanged(() => RtuName);
            }
        }

        private string _attachName;

        public string AttachName
        {
            get { return _attachName; }
            set
            {
                if (_attachName != value)
                {
                    _attachName = value;
                    this.RaisePropertyChanged(() => this.AttachName);
                }
            }
        }


        private int _attachId;

        public int AttachId
        {
            get { return _attachId; }
            set
            {
                if (_attachId != value)
                {
                    _attachId = value;
                    this.RaisePropertyChanged(() => this.AttachId);


                    var mmm = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                   _attachId);
                    if (mmm != null)
                    {
                        AttachName = mmm.RtuName;
                        return;
                    }
                    AttachName = "未知";

                }
            }
        }


        private int _index;

        /// <summary>
        /// 数据序号
        /// </summary>

        public int Index
        {
            get { return _index; }
            set
            {
                if (value != _index)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }

            }
        }

        

        /// <summary>
        /// 接收时间
        /// </summary>

        public string EsuReceiptTime
        {
            get { return _value1; }
            set
            {
                if (value != _value1)
                {
                    _value1 = value;
                    this.RaisePropertyChanged(() => this.EsuReceiptTime);
                }
            }
        }

        /// <summary>
        /// 温度
        /// </summary>

        public string EsuTemperature
        {
            get { return _value2; }
            set
            {
                if (value != _value2)
                {
                    _value2 = value;
                    this.RaisePropertyChanged(() => this.EsuTemperature);
                }
            }
        }

        /// <summary>
        /// A输入电压
        /// </summary>

        public string EsuInputVoltageA
        {
            get { return _value3; }
            set
            {
                if (value != _value3)
                {
                    _value3 = value;
                    this.RaisePropertyChanged(() => this.EsuInputVoltageA);
                }
            }
        }

        /// <summary>
        /// B输入电压
        /// </summary>

        public string EsuInputVoltageB
        {
            get { return _value4; }
            set
            {
                if (value != _value4)
                {
                    _value4 = value;
                    this.RaisePropertyChanged(() => this.EsuInputVoltageB);
                }
            }
        }

        /// <summary>
        /// C输入电压
        /// </summary>

        public string EsuInputVoltageC
        {
            get { return _value5; }
            set
            {
                if (value != _value5)
                {
                    _value5 = value;
                    this.RaisePropertyChanged(() => this.EsuInputVoltageC);
                }
            }
        }

        /// <summary>
        /// A输出电压
        /// </summary>

        public string EsuOutputVoltageA
        {
            get { return _value6; }
            set
            {
                if (value != _value6)
                {
                    _value6 = value;
                    this.RaisePropertyChanged(() => this.EsuOutputVoltageA);
                }
            }
        }

        /// <summary>
        /// B输出电压
        /// </summary>

        public string EsuOutputVoltageB
        {
            get { return _value7; }
            set
            {
                if (value != _value7)
                {
                    _value7 = value;
                    this.RaisePropertyChanged(() => this.EsuOutputVoltageB);
                }
            }
        }

        /// <summary>
        /// C输出电压
        /// </summary>

        public string EsuOutputVoltageC
        {
            get { return _value8; }
            set
            {
                if (value != _value8)
                {
                    _value8 = value;
                    this.RaisePropertyChanged(() => this.EsuOutputVoltageC);
                }
            }
        }

        /// <summary>
        /// A相输出功率
        /// </summary>

        public string EsuOutputCurrentpA
        {
            get { return _value9; }
            set
            {
                if (value != _value9)
                {
                    _value9 = value;
                    this.RaisePropertyChanged(() => this.EsuOutputCurrentpA);
                }
            }
        }

        /// <summary>
        /// B相输出功率
        /// </summary>

        public string EsuOutputCurrentpB
        {
            get { return _value10; }
            set
            {
                if (value != _value10)
                {
                    _value10 = value;
                    this.RaisePropertyChanged(() => this.EsuOutputCurrentpB);
                }
            }
        }

        /// <summary>
        /// C相输出功率
        /// </summary>

        public string EsuOutputCurrentpC
        {
            get { return _value11; }
            set
            {
                if (value != _value11)
                {
                    _value11 = value;
                    this.RaisePropertyChanged(() => this.EsuOutputCurrentpC);
                }
            }
        }

        /// <summary>
        /// A相输出电流
        /// </summary>

        public string EsuOutputCurrentA
        {
            get { return _value12; }
            set
            {
                if (value != _value12)
                {
                    _value12 = value;
                    this.RaisePropertyChanged(() => this.EsuOutputCurrentA);
                }
            }
        }

        /// <summary>
        /// B相输出电流
        /// </summary>

        public string EsuOutputCurrentB
        {
            get { return _value13; }
            set
            {
                if (value != _value13)
                {
                    _value13 = value;
                    this.RaisePropertyChanged(() => this.EsuOutputCurrentB);
                }
            }
        }

        /// <summary>
        /// C相输出电流
        /// </summary>

        public string EsuOutputCurrentC
        {
            get { return _value14; }
            set
            {
                if (value != _value14)
                {
                    _value14 = value;
                    this.RaisePropertyChanged(() => this.EsuOutputCurrentC);
                }
            }
        }


        /// <summary>
        /// 风机状态 0关闭 ；1 开启
        /// </summary>

        public string EsuFanState
        {
            get { return _value15; }
            set
            {
                if (value != _value15)
                {
                    _value15 = value;
                    this.RaisePropertyChanged(() => this.EsuFanState);
                }
            }
        }

        /// <summary>
        /// 节电控制状态 EsuControlState
        /// </summary>

        public string EsuControlState
        {
            get { return _value16; }
            set
            {
                if (value != _value16)
                {
                    _value16 = value;
                    this.RaisePropertyChanged(() => this.EsuControlState);
                }
            }
        }

        /// <summary>
        /// 开机时间
        /// </summary>

        public string EsuBootTime
        {
            get { return _value17; }
            set
            {
                if (value != _value17)
                {
                    _value17 = value;
                    this.RaisePropertyChanged(() => this.EsuBootTime);
                }
            }
        }

        /// <summary>
        /// 节电运行时间
        /// </summary>

        public string EsuRunTime
        {
            get { return _value18; }
            set
            {
                if (value != _value18)
                {
                    _value18 = value;
                    this.RaisePropertyChanged(() => this.EsuRunTime);
                }
            }
        }

        /// <summary>
        /// 目标调压值
        /// </summary>

        public string EsuTargetValue
        {
            get { return _value19; }
            set
            {
                if (value != _value19)
                {
                    _value19 = value;
                    this.RaisePropertyChanged(() => this.EsuTargetValue);
                }
            }
        }

        /// <summary>
        /// 故障状态26
        /// </summary>

        public string EsuFaultState
        {
            get { return _value20; }
            set
            {
                if (value != _value20)
                {
                    _value20 = value;
                    this.RaisePropertyChanged(() => this.EsuFaultState);
                }
            }
        }

        /// <summary>
        /// 节电量
        /// </summary>

        public string EsuQuantity
        {
            get { return _value21; }
            set
            {
                if (value != _value21)
                {
                    _value21 = value;
                    this.RaisePropertyChanged(() => this.EsuQuantity);
                }
            }
        }

        /// <summary>
        /// 节电率
        /// </summary>

        public string EsuRatio
        {
            get { return _value22; }
            set
            {
                if (value != _value22)
                {
                    _value22 = value;
                    this.RaisePropertyChanged(() => this.EsuRatio);
                }
            }
        }


        /// <summary>
        /// 节电率
        /// </summary>

        public string EsuRatioA
        {
            get { return _value23; }
            set
            {
                if (value != _value23)
                {
                    _value23 = value;
                    this.RaisePropertyChanged(() => this.EsuRatioA);
                }
            }
        }

        /// <summary>
        /// 节电率
        /// </summary>

        public string EsuRatioB
        {
            get { return _value24; }
            set
            {
                if (value != _value24)
                {
                    _value24 = value;
                    this.RaisePropertyChanged(() => this.EsuRatioB);
                }
            }
        }

        /// <summary>
        /// 节电率
        /// </summary>

        public string EsuRatioC
        {
            get { return _value25; }
            set
            {
                if (value != _value25)
                {
                    _value25 = value;
                    this.RaisePropertyChanged(() => this.EsuRatioC);
                }
            }
        }

        #endregion

        public EsuDataOneItemViewModel()
        {
            Index = 0;
            RtuId = 0;
            EsuBootTime = "--";
            EsuControlState = "--";
            EsuControlState = "--";
            EsuFanState = "--";
            EsuInputVoltageA = "--";
            EsuInputVoltageB = "--";
            EsuInputVoltageC = "--";
            EsuOutputCurrentA = "--";
            EsuOutputCurrentB = "--";
            EsuOutputCurrentC = "--";
            EsuOutputCurrentpA = "--";
            EsuOutputCurrentpB = "--";
            EsuOutputCurrentpC = "--";
            EsuOutputVoltageA = "--";
            EsuOutputVoltageB = "--";
            EsuOutputVoltageC = "--";
            EsuQuantity = "--";
            EsuRatio = "--";
            EsuReceiptTime = "--";
            EsuRunTime = "--";
            EsuTargetValue = "--";
            EsuTemperature = "--";
            EsuFaultState = "--";
        }


        public EsuDataOneItemViewModel(ReplyEsuData.Jd601Data info,int rtuid,int index=0)
        {
            Index = index;
            Id = new DateTime(info.EsuReceiptTime).ToString("yyyy-MM-dd HH:mm:ss");
            AttachId = info.RtuId;
            EsuBootTime = "" + (info.EsuBootTime/60).ToString("d2") + ":" + (info.EsuBootTime%60).ToString( "d2")+ "  ["+info .EsuBootTime +"分钟]";
            EsuControlState = "未知";
            if (Models.EsuControlState.EsuControlStates.ContainsKey(info.EsuControlState))
                EsuControlState = Models.EsuControlState.EsuControlStates[info.EsuControlState];
            EsuFanState = info.EsuFanState == 1 ? "开启" : "关闭";
            EsuInputVoltageA = info.EsuInputVoltageA + " V";
            EsuInputVoltageB = info.EsuInputVoltageB + " V";
            EsuInputVoltageC = info.EsuInputVoltageC + " V";
            EsuOutputCurrentA = info.EsuOutputCurrentA + " A";
            EsuOutputCurrentB = info.EsuOutputCurrentB + " A";
            EsuOutputCurrentC = info.EsuOutputCurrentC + " A";
            EsuOutputCurrentpA = info.EsuOutputCurrentpA + " Kw";
            EsuOutputCurrentpB = info.EsuOutputCurrentpB + " Kw";
            EsuOutputCurrentpC = info.EsuOutputCurrentpC + " Kw";
            EsuOutputVoltageA = info.EsuOutputVoltageA + " V";
            EsuOutputVoltageB = info.EsuOutputVoltageB + " V";
            EsuOutputVoltageC = info.EsuOutputVoltageC + " V";
            EsuQuantity = info.EsuQuantity + "";
            EsuRatio = (info.EsuRatio*100) + " %";
            EsuReceiptTime = new DateTime(info.EsuReceiptTime).ToString(CultureInfo.InvariantCulture);
            EsuRunTime = "" + (info.EsuRunTime / 60).ToString("d2") + ":" + (info.EsuRunTime % 60).ToString("d2") + "  [" + info.EsuRunTime + "分钟]";
            EsuTargetValue = info.EsuTarValue + " V";
            EsuTemperature = info.EsuTemperature + " 度";
            RtuId = rtuid;
            

            string str = "";
            for (int i = 0; i < 16; i++)
            {
                if (((info.EsuFaultState >> i) & 1) == 1)
                {
                    if (Models.EsuControlState.EsuErrorStates.ContainsKey(i))
                    {
                        //lst.Add(Models.Exchange.EsuControlState.EsuErrorStates[i]);
                        str += Models.EsuControlState.EsuErrorStates[i] + ";";
                    }
                }
            }
            EsuFaultState = str;

            if (info.EsuOutputVoltageA > 0 && info.EsuInputVoltageA > 0 && info.EsuOutputCurrentA > 0)
                EsuRatioA = ((1 -
                              (info.EsuOutputVoltageA * info.EsuOutputVoltageA * 1.0) /
                              (info.EsuInputVoltageA * info.EsuInputVoltageA)) * 100) + "%";
            else EsuRatioA = "--";

            if (info.EsuOutputVoltageB > 0 && info.EsuInputVoltageB > 0 && info.EsuOutputCurrentB > 0)
                EsuRatioB = ((1 -
                              (info.EsuOutputVoltageB * info.EsuOutputVoltageB * 1.0) /
                              (info.EsuInputVoltageB * info.EsuInputVoltageB)) * 100) + "%";
            else EsuRatioB = "--";

            if (info.EsuOutputVoltageC > 0 && info.EsuInputVoltageC > 0 && info.EsuOutputCurrentC > 0)
                EsuRatioC = ((1 -
                              (info.EsuOutputVoltageC * info.EsuOutputVoltageC * 1.0) /
                              (info.EsuInputVoltageC * info.EsuInputVoltageC)) * 100) + "%";
            else EsuRatioC = "--";

        }
    };

}
