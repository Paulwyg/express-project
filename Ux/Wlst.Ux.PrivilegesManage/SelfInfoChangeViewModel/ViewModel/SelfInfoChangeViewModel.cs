using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;

using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Ux.PrivilegesManage.SelfInfoChangeViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.PrivilegesManage.SelfInfoChangeViewModel.ViewModel
{
    [Export(typeof (IISelfInfoChangeViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SelfInfoChangeViewModel :
        VmEventActionProperyChangedBase,
        IISelfInfoChangeViewModel
    {

        public SelfInfoChangeViewModel()
        {
            Title = "当前用户修改";
            InitAction();

        }

        private bool _stayAtModifyView;

        protected bool StayAtModifyView
        {
            get { return _stayAtModifyView; }
            set
            {
                _stayAtModifyView = value;
                if (_stayAtModifyView)
                {
                    UserName = UserInfo.UserLoginInfo.UserName;
                    ModifyUserDepartment = UserDepartment ;
                    ModifyUserPhoneNumber = UserPhoneNumber;
                    ModifyUserPositon = UserPositon;
                    ModifyUserRealName = UserRealName;
                    

                    CmdName = "保存";
                    NormalVisi = Visibility.Collapsed;
                    ModifyVisi = Visibility.Visible;
                }
                else
                {
                    UserName = UserInfo.UserLoginInfo.UserName;
                    CmdName = "修改";
                    NormalVisi = Visibility.Visible;
                    ModifyVisi = Visibility.Collapsed;
                }

            }
        }

        #region PrivilegeInfo

        private ObservableCollection<NameIntBool> _privilegeInfo;

        public ObservableCollection<NameIntBool> PrivilegeInfo
        {
            get { return _privilegeInfo ?? (_privilegeInfo = new ObservableCollection<NameIntBool>()); }
        }

        #endregion


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
                if (value == _userName) return;
                _userName = value;
                RaisePropertyChanged(() => UserName);
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
                if (value == _userPassword) return;
                _userPassword = value;
                RaisePropertyChanged(() => UserPassword);
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
                if (value == _userRealName) return;
                _userRealName = value;
                RaisePropertyChanged(() => UserRealName);
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
                if (value == _userPhoneNumber) return;
                _userPhoneNumber = value;
                RaisePropertyChanged(() => UserPhoneNumber);
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
                if (value == _userDepartment) return;
                _userDepartment = value;
                RaisePropertyChanged(() => UserDepartment);
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
                if (value == _userPositon) return;
                _userPositon = value;
                RaisePropertyChanged(() => UserPositon);
            }
        }

        


        #endregion

        #region Visi

        private Visibility _normalVisi;

        /// <summary>
        /// 基本信息是否可见
        /// </summary>
        public Visibility NormalVisi
        {
            get { return _normalVisi; }
            set
            {
                if (value == _normalVisi) return;
                _normalVisi = value;
                RaisePropertyChanged(() => NormalVisi);
            }
        }

        private Visibility _modifyVisi;

        /// <summary>
        /// 修改账号休息是否可见
        /// </summary>
        public Visibility ModifyVisi
        {
            get { return _modifyVisi; }
            set
            {
                if (value == _modifyVisi) return;
                _modifyVisi = value;
                RaisePropertyChanged(() => ModifyVisi);
            }
        }

        #endregion

        #region Attri Modify UserInfo

        private string _modifyUserPasswrodOne;

        /// <summary>
        /// 登陆账号
        /// </summary>
        public string ModifyUserPasswrodOne
        {
            get { return _modifyUserPasswrodOne; }
            set
            {
                if (value != _modifyUserPasswrodOne)
                {
                    _modifyUserPasswrodOne = value;
                    RaisePropertyChanged(() => ModifyUserPasswrodOne);
                }
            }
        }


        private string _modifyUserPasswrodTwo;

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string ModifyUserPasswrodTwo
        {
            get { return _modifyUserPasswrodTwo; }
            set
            {
                if (value == _modifyUserPasswrodTwo) return;
                _modifyUserPasswrodTwo = value;
                RaisePropertyChanged(() => ModifyUserPasswrodTwo);
            }
        }

        private string _modifyUserRealName;

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string ModifyUserRealName
        {
            get { return _modifyUserRealName; }
            set
            {
                if (value != _modifyUserRealName)
                {
                    _modifyUserRealName = value;
                    RaisePropertyChanged(() => ModifyUserRealName);
                }
            }
        }

        private string _modifyUserPhoneNumber;

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ModifyUserPhoneNumber
        {
            get { return _modifyUserPhoneNumber; }
            set
            {
                if (value == _modifyUserPhoneNumber) return;
                _modifyUserPhoneNumber = value;
                RaisePropertyChanged(() => ModifyUserPhoneNumber);
            }
        }

        private string _modifyUserDepartment;

        /// <summary>
        /// 工作部门
        /// </summary>
        public string ModifyUserDepartment
        {
            get { return _modifyUserDepartment; }
            set
            {
                if (value == _modifyUserDepartment) return;
                _modifyUserDepartment = value;
                RaisePropertyChanged(() => ModifyUserDepartment);
            }
        }

        private string _modifyUserPositon;

        /// <summary>
        /// 工作职位
        /// </summary>
        public string ModifyUserPositon
        {
            get { return _modifyUserPositon; }
            set
            {
                if (value != _modifyUserPositon)
                {
                    _modifyUserPositon = value;
                    RaisePropertyChanged(() => ModifyUserPositon);
                }
            }
        }

        #endregion

        #region button

        private string _cmdName;

        /// <summary>
        /// btn
        /// </summary>
        public string CmdName
        {
            get { return _cmdName; }
            set
            {
                if (value == _cmdName) return;
                _cmdName = value;
                RaisePropertyChanged(() => CmdName);
            }
        }

        private DateTime _dtButton;
        private ICommand _cmdButton;

        public ICommand CmdButton
        {
            get { return _cmdButton ?? (_cmdButton = new RelayCommand(ExCmdButton, CanExButton, true)); }
        }

        private void ExCmdButton()
        {
            _dtButton = DateTime.Now;
            if (StayAtModifyView)
            {
                if (SaveModfiyInfo())
                    StayAtModifyView = false;
            }
            else
            {
                var sss = UMessageBoxWantSomefromUser.Show("密码验证", "请输入密码", "");
                if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                {
                    return;
                }
                if (sss != UserInfo .UserLoginInfo.UserPassword )
                {
                    UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......", UMessageBoxButton.Yes);
                    return;
                }
                StayAtModifyView = true;
            }
        }

        private bool SaveModfiyInfo()
        {
            if (string.IsNullOrEmpty(ModifyUserPasswrodOne))
            {
                UMessageBox.Show("密码不允许有空...", "密码为空", UMessageBoxButton.Ok);
                return false;
            }
            if (ModifyUserPasswrodOne != ModifyUserPasswrodTwo)
            {
                UMessageBox.Show("两次输入的密码不匹配...", "密码为空", UMessageBoxButton.Ok);
                return false;
            }

            UpdateUserInfo();
            return true;
        }

        private bool CanExButton()
        {
            return DateTime.Now.Ticks - _dtButton.Ticks > 30000000;
        }

        #endregion

        /// <summary>
        /// 页面加载或是导航显示的时候 需要执行的初始化操作
        /// </summary>
        /// <param name="parsObjects"></param>
        public override void NavOnLoadr(params object[] parsObjects)
        {
            StayAtModifyView = false;

            var nt = Wlst.Sr.ProtocolPhone.LxLogin.wst_request_user_info;
            nt.WstLoginRequestUserInfo.TargetUserName = UserInfo.UserLoginInfo.UserName;
            SndOrderServer.OrderSnd(nt, 10, 2);
        }

    }

    /// <summary>
    /// event
    /// </summary>
    public partial class SelfInfoChangeViewModel
    {

        protected void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxLogin.wst_add_or_update_user,
                //.ClientPart.wlst_PrivilegeInfo_server_ans_clinet_update_UserInfomation,
                UserInfoUpdated,
                typeof (SelfInfoChangeViewModel), this);

            ProtocolServer.RegistProtocol(
               Wlst.Sr.ProtocolPhone.LxLogin.wst_request_user_info ,
                //.ClientPart.wlst_PrivilegeInfo_server_ans_clinet_update_UserInfomation,
               UserRequest,
               typeof(SelfInfoChangeViewModel), this);
        }

        public void UserInfoUpdated(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (IsViewShowd == false) return;
            var userInfo = infos.WstLoginAddOrUpdateUserInfo ;
            if (userInfo == null) return;
            if (userInfo.UserInfo.UserName == UserInfo.UserLoginInfo.UserName)
            {
                if (!userInfo.SvrRtnModified)
                {
                    LogInfo.Log("修改用户信息失败:" + userInfo.SvrRtnMsg);
                }
                NavOnLoad();
            }
        }

        UserInfomation _curruser=null ;
        public void UserRequest(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (IsViewShowd == false) return;
            var userInfo = infos.WstLoginRequestUserInfo;
            if (userInfo == null) return;
            if (userInfo.TargetUserName.Equals(UserInfo.UserLoginInfo.UserName) && userInfo.UserInfo.Count > 0 &&
                userInfo.UserInfo[0].UserName.Equals(UserInfo.UserLoginInfo.UserName))
            {
                _curruser = userInfo.UserInfo[0];
                UserName = userInfo.UserInfo[0].UserName;
                UserPassword = userInfo.UserInfo[0].UserPasswrod;
                UserDepartment = userInfo.UserInfo[0].Department;
                UserPhoneNumber = userInfo.UserInfo[0].PhoneNumber;
                UserPositon = userInfo.UserInfo[0].Position;
                UserRealName = userInfo.UserInfo[0].UserRealName;
            }
        }

        void UpdateUserInfo()
        {
            if (_curruser == null) return;
            var nt = Wlst.Sr.ProtocolPhone.LxLogin.wst_add_or_update_user;
            nt.WstLoginAddOrUpdateUserInfo .Op = 2;
            nt.WstLoginAddOrUpdateUserInfo.UserInfo = _curruser;
            nt.WstLoginAddOrUpdateUserInfo.UserInfo.UserPasswrod = ModifyUserPasswrodOne;
            nt.WstLoginAddOrUpdateUserInfo.UserInfo.Department = ModifyUserDepartment;
            nt.WstLoginAddOrUpdateUserInfo.UserInfo.PhoneNumber = ModifyUserPhoneNumber;
            nt.WstLoginAddOrUpdateUserInfo.UserInfo.Position = ModifyUserPositon;
            nt.WstLoginAddOrUpdateUserInfo.UserInfo.UserRealName = ModifyUserRealName;
            SndOrderServer.OrderSnd(nt, 10, 2);
        }
    }

}
