using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlSettingViewModel.ViewModel
{
   public  class TmlIdNameSelectViewModel:ObservableObject 
    {
        private int rtuId;

        public int RtuId
        {
            get { return rtuId; }
            set
            {
                if (rtuId != value)
                {
                    rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                }
            }
        }

        //private int _iphyd;

        //public int PhyId
        //{
        //    get { return _iphyd; }
        //    set
        //    {
        //        if (_iphyd != value)
        //        {
        //            _iphyd = value;
        //            this.RaisePropertyChanged(() => this.PhyId);
        //        }
        //    }
        //}

        private string rtuName;

        /// <summary>
        /// 名称
        /// </summary>
        public string RtuName
        {
            get { return rtuName; }
            set
            {
                if (rtuName != value)
                {
                    rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                }
            }
        }
    }
}
