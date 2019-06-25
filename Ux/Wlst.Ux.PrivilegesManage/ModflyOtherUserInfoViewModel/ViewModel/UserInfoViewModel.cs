using System.Collections.Generic;
using Wlst.Sr.ProtocolCnt.PrivilegeInfo.FrClient;

namespace Wlst.Ux.PrivilegesManage.ModflyOtherUserInfoViewModel.ViewModel
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


        private string _userPassword;

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                if (value != _userPassword)
                {
                    _userPassword = value;
                    RaisePropertyChanged(() => UserPassword);
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

        private string _userDepartment;

        /// <summary>
        /// 工作部门
        /// </summary>
        public string UserDepartment
        {
            get { return _userDepartment; }
            set
            {
                if (value != _userDepartment)
                {
                    _userDepartment = value;
                    RaisePropertyChanged(() => UserDepartment);
                }
            }
        }

        private string _userPositon;

        /// <summary>
        /// 工作职位
        /// </summary>
        public string UserPositon
        {
            get { return _userPositon; }
            set
            {
                if (value != _userPositon)
                {
                    _userPositon = value;
                    RaisePropertyChanged(() => UserPositon);
                }
            }
        }


        public List<int> UserPrivilegeList; 

        #endregion

        public UserInfoViewModel(UserInfomation userinfo)
        {
            UserName = userinfo.UserName;
            UserPassword = userinfo.UserPasswrod;
            UserDepartment = userinfo.Department;
            UserPhoneNumber = userinfo.PhoneNumber;
            UserPositon = userinfo.Position;
            UserRealName = userinfo.UserRealName;
            UserPrivilegeList = new List<int>();
            foreach (var t in userinfo.UserPirvilegeGroupBelong) UserPrivilegeList.Add(t);
        }

        public UserInfoViewModel ()
        {
            UserName = "";
            UserPassword = "";
            UserDepartment = "";
            UserPhoneNumber = "";
            UserPositon = "";
            UserRealName = "";
            UserPrivilegeList = new List<int>();
        }


    }
}
