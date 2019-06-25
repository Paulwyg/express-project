using System;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Ux.StateBarModule.NewSvrMsgView.Services;

namespace Wlst.Ux.StateBarModule.NewSvrMsgView.ViewModel
{
    public class OperatorRecordItem : Wlst.Cr.Core.CoreServices.ObservableObject
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

        private OperatrType _loop;

        public OperatrType Operatr
        {
            get { return _loop; }
            set
            {
                _loop = value;
                if (value == OperatrType.UserOperator) OperatrName = "用户操作";
                else if (value == OperatrType.SystemInfo) OperatrName = "系统信息";
                else OperatrName = "服务反馈";
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
    };


}
