using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.WJ3005Module.TmlInfoValidQuery.ViewModel
{
    public class InvalidTmlModel : ObservableObject 
    {
        public InvalidTmlModel()
        {

        }

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }

        private int _rtuId;
        public int RtuId
        {
            get
            {
                return _rtuId;
            }
            set
            {
                if (_rtuId != value)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                }
            }
        }

        private int _phyId;
        public int PhyId
        {
            get
            {
                return _phyId;
            }
            set
            {
                if (_phyId != value)
                {
                    _phyId = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private string _rtuName;
        public string RtuName
        {
            get
            {
                return _rtuName; 
            }
            set
            {
                if (_rtuName != value)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private string _grpName;
        public string GrpName
        {
            get
            {
                return _grpName;
            }
            set
            {
                if (_grpName != value)
                {
                    _grpName = value;
                    this.RaisePropertyChanged(() => this.GrpName);
                }
            }
        }

        private string _faultType;
        public string FaultType
        {
            get
            {
                return _faultType;
            }
            set
            {
                if (_faultType != value)
                {
                    _faultType = value;
                    this.RaisePropertyChanged(() => this.FaultType);
                }
            }
        }
    }
}
