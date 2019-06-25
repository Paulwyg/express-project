
namespace Wlst.Ux.EquipemntLightFault.Models
{
    public class UserInfoViewModel : Cr.Core.CoreServices.ObservableObject
    {
        #region Attri UserInfo

        private string _userName;

        /// <summary>
        /// 登陆账号
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value != _userName)
                {
                    _userName = value;
                    RaisePropertyChanged(() => UserName);
                }
            }
        }




        private string _userRealName;

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string UserRealName
        {
            get { return _userRealName; }
            set
            {
                if (value != _userRealName)
                {
                    _userRealName = value;
                    RaisePropertyChanged(() => UserRealName);
                }
            }
        }

        private string _userPhoneNumber;

        /// <summary>
        /// 联系电话
        /// </summary>
        public string UserPhoneNumber
        {
            get { return _userPhoneNumber; }
            set
            {
                if (value != _userPhoneNumber)
                {
                    _userPhoneNumber = value;
                    RaisePropertyChanged(() => UserPhoneNumber);
                }
            }
        }





        #endregion

        public UserInfoViewModel(Wlst .client . UserInfomation userinfo)
        {
            UserName = userinfo.UserName;

            UserPhoneNumber = userinfo.PhoneNumber;
            UserRealName = userinfo.UserRealName;
        }

        public UserInfoViewModel()
        {
            UserName = "";
            UserPhoneNumber = "";
            UserRealName = "";
        }


    }
}
