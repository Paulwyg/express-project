using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Wlst.Ux.Wj2096Module.NewData.CtrlData.ViewModel
{
  public  class DataSluCtrlLampVm:Wlst .Cr .Core .CoreServices .ObservableObject 
    {
        #region attri



        private int _isdfsdfndexsdf;

        public int LampId
        {
            get { return _isdfsdfndexsdf; }
            set
            {
                if (_isdfsdfndexsdf == value) return;
                _isdfsdfndexsdf = value;
                RaisePropertyChanged(() => LampId);
            }
        }

        private string  _isdfStatessdfndexsdf;

        public string  States
        {
            get { return _isdfStatessdfndexsdf; }
            set
            {
                if (_isdfStatessdfndexsdf == value) return;
                _isdfStatessdfndexsdf = value;
                RaisePropertyChanged(() => States);
            }
        }

        private string  _indexsdf;

        public string  PowerStatus
        {
            get { return _indexsdf; }
            set
            {
                if (_indexsdf == value) return;
                _indexsdf = value;
                RaisePropertyChanged(() => PowerStatus);
            }
        }

        private string _indsdfsdfdf;

        public string IsLeakage
        {
            get { return _indsdfsdfdf; }
            set
            {
                if (_indsdfsdfdf == value) return;
                _indsdfsdfdf = value;
                RaisePropertyChanged(() => IsLeakage);
            }
        }

        private string _index;

        public string Fault
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                RaisePropertyChanged(() => Fault);
            }
        }

  


        private string _lDateCreate;

        public string StateWorkingOn
        {
            get { return _lDateCreate; }
            set
            {
                if (_lDateCreate == value) return;
                _lDateCreate = value;
                RaisePropertyChanged(() => StateWorkingOn);
            }
        }

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

        private string _lDateReply;

        public string Voltage
        {
            get { return _lDateReply; }
            set
            {
                if (_lDateReply == value) return;
                _lDateReply = value;
                RaisePropertyChanged(() => Voltage);
            }
        }


        private string _liUserName;

        public string Current
        {
            get { return _liUserName; }
            set
            {
                if (_liUserName == value) return;
                _liUserName = value;
                RaisePropertyChanged(() => Current);
            }
        }


        private string _lRemark;

        public string ActivePower
        {
            get { return _lRemark; }
            set
            {
                if (_lRemark == value) return;
                _lRemark = value;
                RaisePropertyChanged(() => ActivePower);
            }
        }


        private string _indDateCtrlCreateesdfx;

        public string DateCtrlCreate
        {
            get { return _indDateCtrlCreateesdfx; }
            set
            {
                if (_indDateCtrlCreateesdfx == value) return;
                _indDateCtrlCreateesdfx = value;
                RaisePropertyChanged(() => DateCtrlCreate);
            }
        }


        private string _indexgg;

        public string Electricity
        {
            get { return _indexgg; }
            set
            {
                if (_indexgg == value) return;
                _indexgg = value;
                RaisePropertyChanged(() => Electricity);
            }
        }



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


        private string _dbcode;

        public string DBCode
        {
            get { return _dbcode; }
            set
            {
                if (_dbcode == value) return;
                _dbcode = value;
                RaisePropertyChanged(() => DBCode);
            }
        }


        private string _dblamp;

        public string DBLamp
        {
            get { return _dblamp; }
            set
            {
                if (_dblamp == value) return;
                _dblamp = value;
                RaisePropertyChanged(() => DBLamp);
            }
        }
        #endregion

        public DataSluCtrlLampVm(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlLamp info, int states)
        {
            if (info.Electricity< 0)
            {
                Electricity = "0";
                ElectricityTotal = "0";
            }
            else
            {
                Electricity = info.Electricity.ToString("f2") + "";
                ElectricityTotal = info.ElectricityTotal.ToString("f2");
            }
            if (info.ActiveTime < 0)  //ActiveTimeTotal
            {
                ActiveTimeTotal = "0";
                ActiveTime = "0";
            }
            else
            {
                ActiveTime = (info.ActiveTime / 60).ToString("f2");
                ActiveTimeTotal = (info.ActiveTimeTotal / 60).ToString("f2");

            }

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

            if (states == 3)
            {
                this.ActivePower = "--";
                this.Current = "--";
                this.Voltage = "--";
                LampId = info.LampId;
                PowerStatus = "--";

                IsLeakage = "--";
                StateWorkingOn = "--";
                Fault = "--";
                PwFactor = "--";
            }
            else
            {
                this.ActivePower = info.ActivePower.ToString("f2");
                this.Current = info.Current.ToString("f2");
                this.Voltage = info.Voltage.ToString("f2");
                LampId = info.LampId;
                PowerStatus = info.PowerStatus == 0 ? "正常" : info.PowerStatus == 1 ? "功率越上限" : "功率越下限";

                IsLeakage = info.IsLeakage
                                ? "漏电"
                                : "正常";
                StateWorkingOn = info.StateWorkingOn == 0
                                     ? "正常亮灯"
                                     : info.StateWorkingOn == 1
                                           ? "调档节能"
                                           : info.StateWorkingOn == 2
                                                 ? "调光" + info.PowerLevel + "%"
                                                 : "关灯";



                if (info.Voltage > 0 && info.ActivePower > 0 && info.Current > 0)
                {
                    var x = info.ActivePower/(info.Voltage*info.Current);
                    if (x > 1 && x < 1.2) x = 1;

                    PwFactor = (x.ToString("f2"));
                }
                else
                {
                    PwFactor = "--";
                }

                var str = "正常";
                if (info.Fault == 1)
                {
                    str = "光源故障";
                }
                else if (info.Fault == 2)
                {
                    str = "补偿电容故障";
                }
                else if (info.Fault == 3)
                {
                    str = "意外灭灯";
                }
                else if (info.Fault == 4)
                {
                    str = "意外亮灯";
                }
                else if (info.Fault == 5)
                {
                    str = "自熄灯";
                }
                else if (info.Fault == 6)
                {
                    str = "控制器断电告警";
                }
                else if (info.Fault == 7)
                {
                    str = "继电器故障";
                }
                Fault = str;

                if (info.DateCtrlCreate < 0)
                {
                    Voltage = "无";
                    ActivePower = "无";
                    ActiveTime = "无";
                    ActiveTimeTotal = "无";
                    Electricity = "无";
                    ElectricityTotal = "无";
                    PowerStatus = "无";
                    Current = "无";
                }
            }

            DBCode = "--";
            DBLamp = "--";

            if (Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info.
                ContainsKey(info.SluId))
            {
                var t =
                    Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Info[info.SluId];
                foreach (var tt in t.CtrlLst)
                {
                    if (tt.CtrlId == info.CtrlId)
                    {
                        DBCode = tt.BarCodeId + "";
                        DBLamp = tt.LampCode;
                        break;
                    }
                }
            }

        }
    }
}
