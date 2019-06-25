using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.ShowMsgInfo;

namespace Wlst.Ux.Nr6005Module.BatchStopRunning.ViewModels
{
   public class BatchStopRunningRecords : ObservableObject
    {
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

        private string _ansTime;

        public string AnsTime
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


                    var tmp =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                    if (tmp != null) PhyId = tmp.RtuPhyId;

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


        private int _rtuStateCode;
        public int RtuStateCode
        {
            get { return _rtuStateCode; }
            set
            {
                if (value != _rtuStateCode)
                {
                    _rtuStateCode = value;
                    switch (_rtuStateCode)
                    {
                        default:
                            RtuState = "使用";
                            break;
                        case 0:
                            RtuState = "不用";
                            break;
                        case 1:
                            RtuState = "停运";
                            break;
                    }

                    this.RaisePropertyChanged(() => this.RtuStateCode);
                }
            }
        }
        private string  _rtuState;
        public string RtuState
        {
            get { return _rtuState; }
            set
            {
                if (value != _rtuState)
                {
                    _rtuState = value;
                    this.RaisePropertyChanged(() => this.RtuState);
                }
            }
        }


        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set
            {
                if (value != _answer)
                {
                    _answer = value;
                    this.RaisePropertyChanged(() => this.Answer);
                }
            }
        }
   
   
   }
}
