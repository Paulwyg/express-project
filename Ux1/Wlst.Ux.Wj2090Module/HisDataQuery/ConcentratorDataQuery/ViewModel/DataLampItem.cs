using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.ViewModel
{
    public class DataLampItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public DataLampItem(int sluId, int ctrId, int lampId, int index)
       {
            Index = index;

            //lvf 2018年5月31日16:48:35   物联网单灯 信息方式不同于往常控制器
            if (ctrId > 8000000)
            {
                ControlId = ctrId;
                var t = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(ctrId);
                if (t == null) return;
                ControlName = t.CtrlName;
                SluId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(ctrId);
            }
            else
            {
                ControlId = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(sluId, ctrId);
                ControlName = Wj2090Module.Services.CommonSlu.GetNameByCtrl(sluId, ctrId);
                SluId = sluId;
            }
            //SluId = sluId;
            //ControlId = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(sluId , ctrId );
            //ControlName = Wj2090Module.Services.CommonSlu.GetNameByCtrl(SluId, ctrId);
            LightNum = lampId;
       }
        public DataLampItem(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlLamp tt, int index)
        {
            Index = index;

            //lvf 2018年5月31日16:48:35   物联网单灯 信息方式不同于往常控制器
            if (SluId>1700000 && SluId<1800000)
            {
                ControlId = tt.CtrlId;
                var t = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(tt.CtrlId);
                if (t == null) return;
                ControlName = t.CtrlName;
                SluId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(tt.CtrlId);
            }
            else
            {
                ControlId = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(tt.SluId, tt.CtrlId);
                ControlName = Wj2090Module.Services.CommonSlu.GetNameByCtrl(tt.SluId, tt.CtrlId);
                SluId = tt.SluId;
            }

            LightNum = tt.LampId;
            PowerLever = tt.PowerLevel + "%";
            if (tt.DateCtrlCreate > -1)
            {
                SampleTime = new DateTime(tt.DateCtrlCreate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            V = tt.Voltage.ToString("f2");
            A = tt.Current.ToString("f2");


            //lvf 2019年5月7日09:19:52 有功功率乘以0.98
            var bs = 1.0;
            var bss = Wlst.Cr.CoreMims.SystemOption.GetOption(1002);
            if (string.IsNullOrEmpty(bss) == false) bs = Convert.ToDouble(bss.Trim());
            //ActivePower = (tt.ActivePower).ToString("f2");
            ActivePower = (tt.ActivePower * bs).ToString("f2");
            //lvf 2018年11月13日16:25:56  计算无功功率   （ui2-p2）^-2
            if ((tt.Voltage * tt.Current) < (tt.ActivePower * bs))
            {
                ReactivePower = "0.00";
            }else
            {
                ReactivePower = Math.Sqrt(Math.Pow(tt.Voltage * tt.Current, 2) - Math.Pow((tt.ActivePower * bs), 2)).ToString("f2");

            }
            
            Electricity = tt.Electricity.ToString("f2");
            IntControlStatus = tt.StateWorkingOn;
            IntLightStatus = tt.Fault;
            LeakageStatus = tt.IsLeakage ? "漏电" : "正常";
            IntPowerStatus = tt.PowerStatus;
            ElectricityTotal = tt.ElectricityTotal.ToString("f2");
            ActiveTime = (tt.ActiveTime/60).ToString("f2");
            ActiveTimeTotal = (tt.ActiveTimeTotal/60).ToString("f2");

            var states = tt.State;
            if (states == 0)
            {
                States = "正常";
            }
            if (states == 1)
            {
                States = "电压越上限";
            }
            if (states == 2)
            {
                States = "电压越下限";
            }
            if (states == 3)
            {
                States = "通信故障";
            }

            if (tt.Voltage > 0 && tt.ActivePower > 0 && tt.Current > 0)
            {
                var x = (tt.ActivePower*bs) / (tt.Voltage * tt.Current);
                //var x = (tt.ActivePower) / (tt.Voltage * tt.Current);
                if (x > 1 && x < 1.2) x = 1;
                //var y =(Math.Truncate(x * 10000) / 10000.0f).ToString("0.0000");
                PwFactor = x.ToString("f2") + "";
            }
            else
            {
                PwFactor = "--";
            }


            if (states == 3)
            {
                V = tt.Voltage.ToString("f2");
                A = tt.Current.ToString("f2");


                //ActivePower = (tt.ActivePower).ToString("f2");
                ActivePower = (tt.ActivePower * bs).ToString("f2");
                //lvf 2018年11月13日16:25:56  计算无功功率   （ui2-p2）^-2
                if ((tt.Voltage * tt.Current) < (tt.ActivePower * bs))
                {
                    ReactivePower = "0.00";
                }
                else
                {
                    ReactivePower = Math.Sqrt(Math.Pow(tt.Voltage * tt.Current, 2) - Math.Pow((tt.ActivePower * bs), 2)).ToString("f2");

                }
                Electricity = tt.Electricity.ToString("f2");
                IntControlStatus = tt.StateWorkingOn;
                IntLightStatus = tt.Fault;
                LeakageStatus = "--";
                IntPowerStatus = 0;
                ElectricityTotal = "--";
                ActiveTime = "--";
                ActiveTimeTotal = "--";
                IsCtrlStop = "--";
                PwFactor = "--";
               // ControlStatus = "通信故障";
            }


            if (tt.DateCtrlCreate < 0)
            {
                SampleTime = "终端报警数据";
                V = "--";
                A = "--";
                PwFactor = "--";
                ActiveTime = "--";
                ActiveTimeTotal = "--";
                ElectricityTotal = "--";
                Electricity = "--";
                ActivePower = "--";
                ReactivePower = "--";
            }
        }


        public DataLampItem(Wlst .client .SluOrCtrlData.DataSluCtrlLampEx tt, int index)
        {
            Index = index;
            //lvf 2018年5月31日16:48:35   物联网单灯 信息方式不同于往常控制器
            if (tt.SluId>1700000 && tt.SluId<1800000)
            {
                ControlId = tt.CtrlId;
                var t = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(tt.CtrlId);
                if (t == null) return;
                ControlName = t.CtrlName;
                SluId = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetCtrlField(tt.CtrlId);
            }
            else
            {
                ControlId = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(tt.SluId, tt.CtrlId);
                ControlName = Wj2090Module.Services.CommonSlu.GetNameByCtrl(tt.SluId, tt.CtrlId);
                SluId = (tt.SluId);
            }

            // ControlId = tt.CtrlId;
            ;
            LightNum = tt.LampId;
            if (tt.DateCtrlCreate < 0)
            {
                //SampleTime = "终端报警数据";
            }
            else
            {
                SampleTime = new DateTime(tt.DateCtrlCreate).ToString("yyyy-MM-dd HH:mm:ss");
            }
            PowerLever = tt.PowerLevel + "%";


            V = tt.Voltage.ToString("f2");
            A = tt.Current.ToString("f2");

            //lvf 2019年5月7日09:19:52 有功功率乘以0.98
            var bs = 1.0;
            var bss = Wlst.Cr.CoreMims.SystemOption.GetOption(1002);
            if (string.IsNullOrEmpty(bss) == false) bs = Convert.ToDouble(bss.Trim());
            //ActivePower = (tt.ActivePower).ToString("f2");
            ActivePower = (tt.ActivePower * bs).ToString("f2");
            //lvf 2018年11月13日16:25:56  计算无功功率   （ui2-p2）^-2
            if ((tt.Voltage * tt.Current) < (tt.ActivePower * bs))
            {
                ReactivePower = "0.00";
            }
            else
            {
                ReactivePower = Math.Sqrt(Math.Pow(tt.Voltage * tt.Current, 2) - Math.Pow((tt.ActivePower *bs), 2)).ToString("f2");

            }
            Electricity =  tt.Electricity.ToString("f2");
            IntControlStatus = tt.StateWorkingOn;
            IntLightStatus = tt.Fault;
            LeakageStatus = tt.IsLeakage ? "漏电" : "正常";
            IntPowerStatus = tt.PowerStatus;
            ElectricityTotal = tt.ElectricityTotal.ToString("f2");
            ActiveTime = (tt.ActiveTime/60).ToString("f2");
            ActiveTimeTotal = (tt.ActiveTimeTotal/60).ToString("f2");

            var states = tt.State;
            if (states == 0)
            {
                States = "正常";
            }
            if (states == 1)
            {
                States = "电压越上限";
            }
            if (states == 2)
            {
                States = "电压越下限";
            }
            if (states == 3)
            {
                States = "通信故障";
            }
            DateCreate = new DateTime(tt.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
            IsCtrlStop = tt.IsCtrlStop ? "停运" : "正常";

            if (tt.Voltage > 0 && tt.ActivePower > 0 && tt.Current > 0)
            {
                var x = (tt.ActivePower * bs) / (tt.Voltage * tt.Current);
                //var x = (tt.ActivePower)/(tt.Voltage*tt.Current);
                if (x > 1 && x < 1.2) x = 1;
                //var y = (Math.Truncate(x * 10000) / 10000.0f).ToString("0.0000");
                PwFactor =  x.ToString("f2") + "";
            }
            else
            {
                PwFactor = "--";
            }

            if (states == 3)
            {
                SampleTime = "--";
                V = tt.Voltage.ToString("f2");
                A = tt.Current.ToString("f2");
                //ActivePower = (tt.ActivePower ).ToString("f2");
                ActivePower = (tt.ActivePower *bs).ToString("f2");
                //lvf 2018年11月13日16:25:56  计算无功功率   （ui2-p2）^-2
                if ((tt.Voltage * tt.Current) < (tt.ActivePower * bs))
                {
                    ReactivePower = "0.00";
                }
                else
                {
                    ReactivePower = Math.Sqrt(Math.Pow(tt.Voltage * tt.Current, 2) - Math.Pow((tt.ActivePower * bs), 2)).ToString("f2");

                }
                Electricity = tt.Electricity.ToString("f2");
                IntControlStatus = tt.StateWorkingOn;
                IntLightStatus = tt.Fault;
                LeakageStatus = "--";
                IntPowerStatus = 0;
                ElectricityTotal = "--";
                ActiveTime = "--";
                ActiveTimeTotal = "--";
                IsCtrlStop = "--";
                PwFactor = "--";
            }
            if (tt.DateCtrlCreate < 0)
            {
                SampleTime = "终端报警数据";
                V = "--";
                A = "--";
                PwFactor = "--";
                ActiveTime = "--";
                ActiveTimeTotal = "--";
                ElectricityTotal = "--";
                Electricity = "--";
                ActivePower = "--";
                ReactivePower = "--";
            }
        }

        private string _isdfStatessdfndexsdf;

        public string States
        {
            get { return _isdfStatessdfndexsdf; }
            set
            {
                if (_isdfStatessdfndexsdf == value) return;
                _isdfStatessdfndexsdf = value;
                RaisePropertyChanged(() => States);
            }
        }
        #region SluId

        private int   _indeSluIdx;
        public int   SluId
        {
            get { return _indeSluIdx; }
            set
            {
                if (_indeSluIdx.Equals(value)) return;
                _indeSluIdx = value;
                RaisePropertyChanged(() => SluId);
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(value))
                {
                    SluShowId =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value).RtuPhyId.ToString("D4");
                    Is2090 = true;
                }
                //var infos = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(value);
                //if (infos != null)
                //{
                //    if (infos.AttachRtuId == 0)
                //    {
                //        SluShowId = string.Format("{0:D7}", infos.PhyId);
                //    }
                //    else SluShowId = infos.RtuId + "";
                //}
                if (Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(value) != null)
                {
                    var para = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(value);
                    SluShowId = para.PhyId.ToString("D4");
                    Is2090 = para.OtherAttri==1 ?false :true ;//是2290设备
                }
                    
            }
        }     private string _ssdfSluId;


        private bool _is2090;
        /// <summary>
        /// 是否为2090型设备   lvf 2018年10月17日13:15:28
        /// </summary>
        public bool Is2090
        {
            get { return _is2090; }
            set
            {
                if (_is2090 == value) return;
                _is2090 = value;
                RaisePropertyChanged(() => Is2090);

            }
        }


        public string SluShowId
        {
            get { return _ssdfSluId; }
            set
            {
                if (value != _ssdfSluId)
                {
                    _ssdfSluId = value;
                    this.RaisePropertyChanged(() => this.SluShowId);
                }
            }
        }

        #endregion

        #region Index

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index.Equals(value)) return;
                _index = value;
                RaisePropertyChanged(() => Index);
            }
        }

        #endregion

        #region ControlId
        private int _controlId;
        public int ControlId
        {
            get { return _controlId; }
            set
            {
                if (_controlId == value) return;
                _controlId = value;
                RaisePropertyChanged(() => ControlId);
            }
        }
        #endregion

        //lvf 2018年4月23日16:16:40 列改成控制器名称
        #region ControlName
        private string _controlName;
        public string ControlName
        {
            get { return _controlName; }
            set
            {
                if (_controlName == value) return;
                _controlName = value;
                RaisePropertyChanged(() => ControlName);
            }
        }
        #endregion

        #region LightNum

        private int _lightNum;
        public int LightNum
        {
            get { return _lightNum; }
            set
            {
                if (_lightNum.Equals(value)) return;
                _lightNum = value;
                RaisePropertyChanged(() => LightNum);
            }
        }

        #endregion

        private string _sdfsdex;
        public string PowerLever
        {
            get { return _sdfsdex; }
            set
            {
                if (_sdfsdex == value) return;
                _sdfsdex = value;
                RaisePropertyChanged(() => PowerLever);
            }
        }
     
     
        public string SlIsCtrlStop;

        /// <summary>
        /// 停运 0-正常，1-停运
        /// </summary>

        public string IsCtrlStop
        {
            get { return SlIsCtrlStop; }
            set
            {
                if (SlIsCtrlStop == value) return;
                SlIsCtrlStop = value;
                RaisePropertyChanged(() => IsCtrlStop);
            }
        }

        private string _saDateCreatempleTime;
        public string DateCreate
        {
            get { return _saDateCreatempleTime; }
            set
            {
                if (_saDateCreatempleTime == value) return;
                _saDateCreatempleTime = value;
                RaisePropertyChanged(() => DateCreate);
            }
        }

        #region SampleTime
        private string  _sampleTime;
        public string  SampleTime
        {
            get { return _sampleTime; }
            set
            {
                if (_sampleTime == value) return;
                _sampleTime = value;
                RaisePropertyChanged(() => SampleTime);
            }
        }
        #endregion

        #region V
        private string  _v;
        public string  V
        {
            get { return _v; }
            set
            {
                if (_v==value) return;
                _v = value;
                RaisePropertyChanged(() => V);
            }
        }
        #endregion

        #region A
        private string  _a;
        public string  A
        {
            get { return _a; }
            set
            {
                if (_a==value ) return;
                _a = value;
                RaisePropertyChanged(() => A);
            }
        }
        #endregion

        #region ActivePower
        private string  _activePower;
        public string  ActivePower
        {
            get { return _activePower; }
            set
            {
                if (_activePower==value ) return;
                _activePower = value;
                RaisePropertyChanged(() => ActivePower);
            }
        }
        #endregion

        #region ReactivePower
        private string _reactivePower;
        public string ReactivePower
        {
            get { return _reactivePower; }
            set
            {
                if (_reactivePower == value) return;
                _reactivePower = value;
                RaisePropertyChanged(() => ReactivePower);
            }
        }
        #endregion


        #region Electricity
        private string _electricity;
        public string Electricity
        {
            get { return _electricity; }
            set
            {
                if (_electricity==value ) return;
                _electricity = value;
                RaisePropertyChanged(() => Electricity);
            }
        }
        #endregion

        private string pwfactor;
        public string PwFactor
        {
            get { return pwfactor; }
            set
            {
                if (pwfactor == value) return;
                pwfactor = value;
                RaisePropertyChanged(() => PwFactor);
            }
        }

        #region IntControlStatus
        private int _intControlStatus;
        public int IntControlStatus
        {
            get { return _intControlStatus; }
            set
            {
                //if (_intControlStatus == value) return;
                _intControlStatus = value;
                //     工作状态 0-正常亮灯，1-一档节能，2-二档节能，3-关灯
                switch (_intControlStatus)
                {
                    case 0:         
                        ControlStatus = "正常亮灯"; 
                        break; 
                    case 1:
                        ControlStatus = "调档节能";
                        break;
                    case 2:
                        ControlStatus = "调光节能";
                        break;
                    case 3:
                        ControlStatus = "关灯"; 
                        break;
                    default:
                        ControlStatus = "";
                        break;
                }
                ////如果是2290设备 显示---
                //if (Is2090 == false) ControlStatus = "---";
            }
        }
        #endregion

        #region IntLightStatus
        private int _intLightStatus;
        public int IntLightStatus
        {
            get { return _intLightStatus; }
            set
            {
                //if (_intLightStatus == value) return;
                _intLightStatus = value;
                //故障 0-正常，1-光源故障，2-补偿电容故障，3-意外灭灯，4-意外亮灯，5-自熄灯
                switch (_intLightStatus)
                {
                    case 0:
                        LightStatus = "正常";
                        break;
                    case 1:
                        LightStatus = "光源故障";
                        break;
                    case 2:
                        LightStatus = "补偿电容故障";
                        break;
                    case 3:
                        LightStatus = "意外灭灯";
                        break;
                    case 4:
                        LightStatus = "意外亮灯";
                        break;
                    case 5:
                        LightStatus = "自熄灯";
                        break;
                    default:
                        LightStatus = "";
                        break;
                }
                ////如果是2290设备 显示---
                //if (Is2090 == false) ControlStatus = "---";
            }
        }
        #endregion

        #region IntPowerStatus
        private int _intPowerStatus;
        public int IntPowerStatus
        {
            get { return _intPowerStatus; }
            set
            {
                //if (_intPowerStatus == value) return;
                _intPowerStatus = value;
                //     功率状态 0-正常，1-功率越上限，2-功率越下限
                switch (_intPowerStatus)
                {
                    case 0:
                        PowerStatus = "正常";
                        break;
                    case 1:
                        PowerStatus = "功率越上限";
                        break;
                    case 2:
                        PowerStatus = "功率越下限";
                        break;
                    default:
                        PowerStatus = "";
                        break;
                }
            }
        }
        #endregion

        #region ControlStatus
        private string _controlStatus;
        public string ControlStatus
        {
            get { return _controlStatus; }
            set
            {
                if (_controlStatus == value) return;
                _controlStatus = value;
                RaisePropertyChanged(() => ControlStatus);
            }
        }
        #endregion

        #region LightStatus
        private string _lightStatus;
        public string LightStatus
        {
            get { return _lightStatus; }
            set
            {
                if (_lightStatus == value) return;
                _lightStatus = value;
                RaisePropertyChanged(() => LightStatus);
            }
        }
        #endregion

        #region LeakageStatus
        private string _leakageStatus;
        public string LeakageStatus
        {
            get { return _leakageStatus; }
            set
            {
                if (_leakageStatus == value) return;
                _leakageStatus = value;
                RaisePropertyChanged(() => LeakageStatus);
            }
        }
        #endregion

        #region PowerStatus
        private string _powerStatus;
        public string PowerStatus
        {
            get { return _powerStatus; }
            set
            {
                if (_powerStatus == value) return;
                _powerStatus = value;
                RaisePropertyChanged(() => PowerStatus);
            }
        }
        #endregion


        private string _indexx;

        public string ElectricityTotal
        {
            get { return _indexx; }
            set
            {
                if (_indexx == value) return;
                _indexx = value;
                RaisePropertyChanged(() => ElectricityTotal);
            }
        }


        private string _indesdfx;

        public string ActiveTime
        {
            get { return _indesdfx; }
            set
            {
                if (_indesdfx == value) return;
                _indesdfx = value;
                RaisePropertyChanged(() => ActiveTime);
            }
        }



        private string _indexdfsdfx;

        public string ActiveTimeTotal
        {
            get { return _indexdfsdfx; }
            set
            {
                if (_indexdfsdfx == value) return;
                _indexdfsdfx = value;
                RaisePropertyChanged(() => ActiveTimeTotal);
            }
        }
    }
}
