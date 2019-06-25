using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.Services
{
    public class ShieldFaultModel : ObservableObject
    {
        #region Id

        private long _id;
        public long Id
        {
            get { return _id; }
            set
            {
                if(_id.Equals(value)) return;
                _id = value;
                RaisePropertyChanged(()=>Id);
            }
        }
        #endregion

        #region TimeStart
        private DateTime _timeStart;
        public DateTime TimeStart
        {
            get { return _timeStart; }
            set
            {
                if (_timeStart == value) return;
                _timeStart = value;
                RaisePropertyChanged(() => TimeStart);
            }
        }
        #endregion

        #region TimeEnd

        private DateTime _timeEnd;
        public DateTime TimeEnd
        {
            get { return _timeEnd; }
            set
            {
                if (_timeEnd == value) return;
                _timeEnd = value;
                RaisePropertyChanged(() => TimeEnd);
            }
        }

        #endregion

        #region Name

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        #endregion

        #region IsChecked

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if(_isChecked==value) return;
                _isChecked = value;
                RaisePropertyChanged(()=>IsChecked);
            }
        }
        #endregion

        #region 故障列表

        public List<int> FaultsShield;

        #endregion

        #region 终端列表

        public List<int> RtusShield;

        #endregion

        #region CreateUser

        private string _createUser;
        public string CreateUser
        {
            get { return _createUser; }
            set
            {
                if(_createUser==value)return;
                _createUser = value;
                RaisePropertyChanged(()=>CreateUser);
            }
        }
        #endregion

        public ShieldFaultModel()
        {
            FaultsShield = new List<int>();
            RtusShield = new List<int>();
        }


    }
}
