using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Ux.StateBarModule.UserOperateMsgView.Services;

namespace Wlst.Ux.StateBarModule.UserOperateMsgView.ViewModel
{
    public class OperateItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public int Addr;
        public int AddrType;
        public long Gid;

        /// <summary>
        /// 是否已经完成本次应答操作
        /// </summary>
        public bool Finish = false;


     private DateTime _sndTime;

        public DateTime OpTime
        {
            get { return _sndTime; }
            set
            {
                if (value != _sndTime)
                {
                    _sndTime = value;
                    this.RaisePropertyChanged(() => this.OpTime);
                }
            }
        }

        private string  _ansTime;

        public string  AnsTime
        {
            get { return _ansTime; }
            set
            {
                if (value != _ansTime)
                {
                    _ansTime = value;
                    this.RaisePropertyChanged(() => this.AnsTime);


                }
            }
        }

        private string _timeDifference;

        public string TimeDifference
        {
            get { return _timeDifference; }
            set
            {
                if (value != _timeDifference)
                {
                    _timeDifference = value;
                    this.RaisePropertyChanged(() => this.TimeDifference);


                }
            }
        }

        private int _rtuId;

        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);

          
                    var tmp= 
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( value);
                    if(tmp !=null ) PhyId = tmp.RtuPhyId;

                }
            }
        }

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private int _oppara;

        public int OpPara
        {
            get { return _oppara; }
            set
            {
                if (_oppara != value)
                {
                    _oppara = value;
                    this.RaisePropertyChanged(() => this.OpPara);
                }
            }
        }

        public int Nindex = -1;
        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private OpType _op;

        public OpType Operatr
        {
            get { return _op; }
            set
            {
                _op = value;
                switch (_op)
                {
                    default:
                        OperatrName = "用户操作";
                        break;
                    case OpType.Snd13Week:
                        OperatrName = "发送k1~k3周设置";
                        break;
                    case OpType.Snd46Week:
                        OperatrName = "发送k4~k6周设置";
                        break;
                    case OpType.Snd78Week:
                        OperatrName = "发送k7~k8周设置";
                        break;
                    case OpType.SndAllYearWeek:
                        OperatrName = "发送全年周设置";
                        break;
                    case OpType.AsynTime:
                        OperatrName = "对时";
                        break;
                    case OpType.RtuClose :
                        OperatrName = "关灯";
                        break;
                    case OpType.RtuMeasure:
                        OperatrName = "选测";
                        break;
                    case OpType.RtuOpen:
                        OperatrName = "开灯";
                        break;
                    case OpType.SndHoliday:
                        OperatrName = "发送节假日";
                        break;
                    case OpType.ZcHoliday:
                        OperatrName = "召测节假日";
                        break;
                    case OpType.ZcRtuPara:
                        OperatrName = "召测终端参数";
                        break;
                    case OpType.ZcTime:
                        OperatrName = "召测时钟";
                        break;
                    case OpType.ZcWeek:
                        OperatrName = "召测周设置";
                        break;
                    case OpType.SluAdjust:
                        OperatrName = "调光";
                        break;
                    case OpType.SluClose:
                        OperatrName = "关灯";
                        break;
                    case OpType.SluMeasure:
                        OperatrName = "集中器选测";
                        break;
                    case OpType.SluOpen:
                        OperatrName = "开灯";
                        break;
                    case OpType.CtrlMeasure:
                        OperatrName = "控制器选测";
                        break;

                } 
            }
        }

        private int _loopId;

        public int LoopId
        {
            get { return _loopId; }
            set
            {
                _loopId = value;
                switch (_loopId)
                {
                    default:
                        SwitchOutName = "---";
                        break;
                    case 0:
                        SwitchOutName = "全部";
                        break;
                    case 1:
                        SwitchOutName = "K1";
                        break;
                    case 2:
                        SwitchOutName = "K2";
                        break;
                    case 3:
                        SwitchOutName = "K3";
                        break;
                    case 4:
                        SwitchOutName = "K4";
                        break;
                    case 5:
                        SwitchOutName = "K5";
                        break;
                    case 6:
                        SwitchOutName = "K6";
                        break;
                    case 7:
                        SwitchOutName = "K7";
                        break;
                    case 8:
                        SwitchOutName = "K8";
                        break;

     
                }
            }
        }


        private string _operatrName;

        public string OperatrName
        {
            get { return _operatrName; }
            set
            {
                if (value != _operatrName)
                {
                    _operatrName = value;
                    this.RaisePropertyChanged(() => this.OperatrName);
                }
            }
        }


        private string _result;

        public string OperatorContent
        {
            get { return _result; }
            set
            {
                if (value != _result)
                {
                    _result = value;
                    this.RaisePropertyChanged(() => this.OperatorContent);
                }
            }
        }

        private string _switchOut;

        public string SwitchOutName
        {
            get { return _switchOut; }
            set
            {
                if (value != _switchOut)
                {
                    _switchOut = value;
                    this.RaisePropertyChanged(() => this.SwitchOutName);
                }
            }
        }
    };
    

}
