using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.ViewModel
{
    public class LuxViewModel : ObservableObject
    {
        private int _luxId;

        /// <summary>
        /// 光控Id
        /// </summary>
        public int LuxId
        {
            get
            {
                return _luxId;
            }
            set
            {
                if (_luxId != value)
                {
                    _luxId = value;
                    RaisePropertyChanged(() => LuxId);
                }
            }
        }

        private string _luxName;

        /// <summary>
        /// 光照度Name
        /// </summary>
        public string LuxName
        {
            get
            {
                return _luxName;
            }
            set
            {
                if (_luxName != value)
                {
                    _luxName = value;
                    RaisePropertyChanged(() => LuxName);
                }
            }
        }
    }
}
