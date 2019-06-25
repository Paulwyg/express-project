using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;

namespace Wlst.Ux.EquipemntLightFault.SendOrderViewModel.ViewModel
{
    public class SendOrderItems : EventHandlerHelperExtendNotifyProperyChanged
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        private int _rtuid;

        public int RtuId
        {
            get { return _rtuid; }
            set
            {
                if (_rtuid != value)
                {
                    _rtuid = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                }
            }
        }

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (_rtuName != value)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private int _loopid;

        public int LoopId
        {
            get { return _loopid; }
            set
            {
                if (_loopid != value)
                {
                    _loopid = value;
                    this.RaisePropertyChanged(() => this.LoopId);
                }
            }
        }

        private ulong _orderid;

        public ulong OrderId
        {
            get { return _orderid; }
            set
            {
                if (_orderid != value)
                {
                    _orderid = value;
                    this.RaisePropertyChanged(() => this.OrderId);
                }
            }
        }

        private string _faultName;

        public string FaultName
        {
            get { return _faultName; }
            set
            {
                if (_faultName != value)
                {
                    _faultName = value;
                    this.RaisePropertyChanged(() => this.FaultName);
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
                }
            }
        }

        private string _priorityLevel;

        public string PriorityLevel
        {
            get { return _priorityLevel; }
            set
            {
                if (_priorityLevel != value)
                {
                    _priorityLevel = value;
                    this.RaisePropertyChanged(() => this.PriorityLevel);
                }
            }
        }



        private string _rtuGroup;

        public string RtuGroup
        {
            get { return _rtuGroup; }
            set
            {
                if (_rtuGroup != value)
                {
                    _rtuGroup = value;
                    this.RaisePropertyChanged(() => this.RtuGroup);
                }
            }
        }

        private string _adminName;

        public string AdminName
        {
            get { return _adminName; }
            set
            {
                if (_adminName != value)
                {
                    _adminName = value;
                    this.RaisePropertyChanged(() => this.AdminName);
                }
            }
        }

        private string _orderTime;

        public string OrderTime
        {
            get { return _orderTime; }
            set
            {
                if (_orderTime != value)
                {
                    _orderTime = value;
                    this.RaisePropertyChanged(() => this.OrderTime);
                }
            }
        }

        private string _mergencyLocation;

        public string MergencyLocation
        {
            get { return _mergencyLocation; }
            set
            {
                if (_mergencyLocation != value)
                {
                    _mergencyLocation = value;
                    this.RaisePropertyChanged(() => this.MergencyLocation);
                }
            }
        }

        private bool _mergencyLocationEnable;

        public bool MergencyLocationEnable
        {
            get { return _mergencyLocationEnable; }
            set
            {
                if (_mergencyLocationEnable != value)
                {
                    _mergencyLocationEnable = value;
                    this.RaisePropertyChanged(() => this.MergencyLocationEnable);
                }
            }
        }

        private string _repairContent;

        public string RepairContent
        {
            get { return _repairContent; }
            set
            {
                if (_repairContent != value)
                {
                    _repairContent = value;
                    this.RaisePropertyChanged(() => this.RepairContent);
                }
            }
        }

        private string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    this.RaisePropertyChanged(() => this.Status);
                }
            }
        }
    }
}
