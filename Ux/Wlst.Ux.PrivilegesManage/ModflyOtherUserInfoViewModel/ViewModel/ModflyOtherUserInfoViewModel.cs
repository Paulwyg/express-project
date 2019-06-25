using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.MessageBoxOverride;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Sr.ProtocolCnt.PrivilegeInfo.FrClient;
using Wlst.Sr.ProtocolCnt.PrivilegeInfo.ToClient;
using Wlst.Ux.PrivilegesManage.ModflyOtherUserInfoViewModel.Services;

namespace Wlst.Ux.PrivilegesManage.ModflyOtherUserInfoViewModel.ViewModel
{
    [Export(typeof (IIModflyOtherUserInfoViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ModflyOtherUserInfoViewModel :
        EventHandlerHelperExtendNotifyProperyChanged, IIModflyOtherUserInfoViewModel
    {
        #region tabTitle



        public string Title
        {
            get { return "信息管理"; }
        }


        public bool CanClose
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        private ObservableCollection<UserInfoViewModel> _userItems;

        public ObservableCollection<UserInfoViewModel> UserItems
        {
            get { return _userItems ?? (_userItems = new ObservableCollection<UserInfoViewModel>()); }
        }

        private UserInfoViewModel _currentSelectUser;

        public UserInfoViewModel CurrentSelectUser
        {
            get { return _currentSelectUser ?? (_currentSelectUser = new UserInfoViewModel()); }
            set
            {
                //if (_currentSelectUser != value)
                //{
                    _currentSelectUser = value;
                    RaisePropertyChanged(() =>CurrentSelectUser);
                    OnCurrenSelectUserChangeInUserInfo();
                //}
            }
        }


        public ModflyOtherUserInfoViewModel()
        {
           InitEvet();
           AddVisi = Visibility.Collapsed;
        }



        /// <summary>
        /// 页面加载或是导航显示的时候 需要执行的初始化操作
        /// </summary>
        /// <param name="parsObjects"></param>
        public void NavOnLoad(params object[] parsObjects)
        {
           UserItems.Clear();
            CurrentSelectUser = new UserInfoViewModel();
           NavOnLoadUserInfo();
           NavOnLoadPrivilege();
           RequestAllUserInfomation();
        }
    };


    /// <summary>
    /// UserInfo manage
    /// </summary>
    public partial class ModflyOtherUserInfoViewModel
    {
        private bool _stayAtModifyView;

        protected bool StayAtModifyView
        {
            get { return _stayAtModifyView; }
            set
            {
                _stayAtModifyView = value;
                if (_stayAtModifyView)
                {
                   UserName = CurrentSelectUser.UserName;
                   ModifyUserDepartment = CurrentSelectUser.UserDepartment;
                   ModifyUserPasswrodOne = CurrentSelectUser.UserPassword;
                   ModifyUserPasswrodTwo = CurrentSelectUser.UserPassword;
                   ModifyUserPhoneNumber = CurrentSelectUser.UserPhoneNumber;
                   ModifyUserPositon = CurrentSelectUser.UserPositon;
                   ModifyUserRealName = CurrentSelectUser.UserRealName;
                    CmdName = "保存";
                    NormalVisi = Visibility.Collapsed;
                    ModifyVisi = Visibility.Visible;
                }
                else
                {
                    //this.ManagerUserName = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.ManagerUserName;
                    //this.ManagerUserPassword = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserPasswrod;
                    //this.UserDepartment = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.Department;
                    //this.UserPhoneNumber = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.PhoneNumber;
                    //this.UserPositon = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.Position;
                    //this.UserRealName = Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserRealName;
                    CmdName = "修改";
                    NormalVisi = Visibility.Visible;
                    ModifyVisi = Visibility.Collapsed;
                }
            }
        }

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
                if (value != _normalVisi)
                {
                    _normalVisi = value;
                   RaisePropertyChanged(() =>NormalVisi);
                }
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
                if (value != _modifyVisi)
                {
                    _modifyVisi = value;
                   RaisePropertyChanged(() =>ModifyVisi);
                }
            }
        }

        #endregion

        #region Visi add

        private Visibility _addVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility AddVisi
        {
            get { return _addVisi; }
            set
            {
                if (value != _addVisi)
                {
                    _addVisi = value;
                   RaisePropertyChanged(() =>AddVisi);
                }
            }
        }

        #endregion

        #region Attri Modify UserInfo

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
                   RaisePropertyChanged(() =>UserName);
                }
            }
        }

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
                   RaisePropertyChanged(() =>ModifyUserPasswrodOne);
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
                if (value != _modifyUserPasswrodTwo)
                {
                    _modifyUserPasswrodTwo = value;
                   RaisePropertyChanged(() =>ModifyUserPasswrodTwo);
                }
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
                   RaisePropertyChanged(() =>ModifyUserRealName);
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
                if (value != _modifyUserPhoneNumber)
                {
                    _modifyUserPhoneNumber = value;
                   RaisePropertyChanged(() =>ModifyUserPhoneNumber);
                }
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
                if (value != _modifyUserDepartment)
                {
                    _modifyUserDepartment = value;
                   RaisePropertyChanged(() =>ModifyUserDepartment);
                }
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
                   RaisePropertyChanged(() =>ModifyUserPositon);
                }
            }
        }


        #region 权限信息
        private ObservableCollection<NameIntBool> _privilegeInfo;

        public ObservableCollection<NameIntBool> PrivilegeInfo
        {
            get { return _privilegeInfo ?? (_privilegeInfo = new ObservableCollection<NameIntBool>()); }
        }


        private string _userPrivilegeInfo;

        /// <summary>
        /// Pri
        /// </summary>
        public string UserPrivilegeInfo
        {
            get { return _userPrivilegeInfo; }
            set
            {
                if (value != _userPrivilegeInfo)
                {
                    _userPrivilegeInfo = value;
                   RaisePropertyChanged(() =>UserPrivilegeInfo);
                }
            }
        }
        #endregion

        #endregion

        #region button Save modify userinfo

        private string _cmdName;

        /// <summary>
        /// btn
        /// </summary>
        public string CmdName
        {
            get { return _cmdName; }
            set
            {
                if (value != _cmdName)
                {
                    _cmdName = value;
                   RaisePropertyChanged(() =>CmdName);
                }
            }
        }

        private ICommand _cmdButton;

        public ICommand CmdButton
        {
            get { return _cmdButton ?? (_cmdButton = new RelayCommand(ExCmdButton, CanExButton, true)); }
        }

        private void ExCmdButton()
        {
            if (StayAtModifyView)
            {
                if (SaveModfiyInfo())
                    StayAtModifyView = false;
            }
            else
            {
                //var sss = Infrastructure.MessageBoxOverride.UMessageBoxWantSomefromUser.Show("密码验证", "请输入您的用户密码", "");
                //if (sss == Infrastructure.MessageBoxOverride.UMessageBoxWantSomefromUser.CancelReturn)
                //{
                //    return;
                //}
                //if (sss != Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserPasswrod)
                //{
                //    Infrastructure.MessageBoxOverride.UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                //                                                       UMessageBoxButton.Yes);
                //    return;
                //}
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
            if (ModifyUserPasswrodOne !=ModifyUserPasswrodTwo)
            {
                UMessageBox.Show("两次输入的密码不匹配...", "密码为空", UMessageBoxButton.Ok);
                return false;
            }

            var user = new UserInfomation
                           {
                               Department =ModifyUserDepartment,
                               UserPasswrod =ModifyUserPasswrodOne,
                               UserName =UserName,
                               PhoneNumber =ModifyUserPhoneNumber,
                               Position =ModifyUserPositon,
                               UserRealName =ModifyUserRealName
                           };user.UserPirvilegeGroupBelong.Clear();
            foreach (var t in PrivilegeInfo)
            {

                if (t.IsSelected) user.UserPirvilegeGroupBelong.Add(t.Value);
            }
           UpdateUserInfo(user );
            return true;
        }

        private bool CanExButton()
        {
            return true;
        }

        #endregion

        #region CmdDeleteCurrentUser

        private DateTime _dtDeleteCurrentUser;
        private ICommand _cmdDeleteCurrentUser;

        public ICommand CmdDeleteCurrentUser
        {
            get {
                return _cmdDeleteCurrentUser ??
                       (_cmdDeleteCurrentUser = new RelayCommand(ExCmdDeleteCurrentUser, CanExDeleteCurrentUser, true));
            }
        }

        private void ExCmdDeleteCurrentUser()
        {
            _dtDeleteCurrentUser = DateTime.Now;
            var rt = UMessageBox.Show("确认删除?", "确认删除用户:" + CurrentSelectUser.UserName + "？",
                                                                        UMessageBoxButton.YesNo);
            if (rt == true)
            {
               DeleteCurrentUser(CurrentSelectUser.UserName);
            }
        }

        private bool CanExDeleteCurrentUser()
        {
            return !string.IsNullOrEmpty(CurrentSelectUser.UserName) && DateTime.Now.Ticks-_dtDeleteCurrentUser.Ticks>30000000;
        }

        #endregion

        #region CmdAdd

        private DateTime _dtAdd;
        private ICommand _cmdAdd;

        public ICommand CmdAdd
        {
            get { return _cmdAdd ?? (_cmdAdd = new RelayCommand(ExCmdAdd, ExCanAdd, true)); }
        }

        private void ExCmdAdd()
        {
            //var sss = Infrastructure.MessageBoxOverride.UMessageBoxWantSomefromUser.Show("密码验证", "请输入您的用户密码", "");
            //if (sss == Infrastructure.MessageBoxOverride.UMessageBoxWantSomefromUser.CancelReturn)
            //{
            //    return;
            //}
            //if (sss != Wlst.Cr.Core.Services.UserInfo.UserLoginInfo.UserPasswrod)
            //{
            //    Infrastructure.MessageBoxOverride.UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
            //                                                       UMessageBoxButton.Yes);
            //    return;
            //}
            _dtAdd = DateTime.Now;
           AddVisi = Visibility.Visible;


           UserName = "";
           ModifyUserDepartment = "";
           ModifyUserPasswrodOne = "";
           ModifyUserPasswrodTwo = "";
           ModifyUserPhoneNumber = "";
           ModifyUserPositon = "";
           ModifyUserRealName = "";

            foreach (var t in PrivilegeInfo) t.IsSelected = false;
            //foreach (var t in this .UserItems )
            //{
            //    if(t.ManagerUserName ==this .ManagerUserName )
            //    {
            //        Infrastructure.MessageBoxOverride.UMessageBox.Show("错误", "您输入的用户名已经存在，请重新输入用户名...",
            //                                                           UMessageBoxButton.Ok);
            //        return;
            //    }
            //}
            //UserInfomation user = new UserInfomation()
            //                          {
            //                              ManagerUserName =this .ManagerUserName ,
            //                              Department =this .ModifyUserDepartment ,
            //                              PhoneNumber =this .ModifyUserPhoneNumber ,
            //                              Position =this .ModifyUserPositon ,
            //                              UserPasswrod =this .ModifyUserPasswrodOne ,
            //                              UserRealName =this .ModifyUserRealName 
            //                          };
            //this.AddUserInfo(user);
        }

        private bool ExCanAdd()
        {
            return AddVisi != Visibility.Visible && DateTime.Now.Ticks - _dtAdd.Ticks > 30000000;
        }

        #endregion

        #region CmdAddUser

        private DateTime _dtAddUser;
        private ICommand _cmdAddUser;

        public ICommand CmdAddUser
        {
            get { return _cmdAddUser ?? (_cmdAddUser = new RelayCommand(ExCmdAddUser, CanExAddUser, true)); }
        }

        private void ExCmdAddUser()
        {
            _dtAddUser = DateTime.Now;
            //数据验证
            if (UserItems.Any(t => t.UserName ==UserName))
            {
                UMessageBox.Show("错误", "您输入的用户名已经存在，请重新输入用户名...",
                                                                   UMessageBoxButton.Ok);
                return;
            }

            var user = new UserInfomation
                           {
                                          UserName = UserName,
                                          Department = ModifyUserDepartment,
                                          PhoneNumber = ModifyUserPhoneNumber,
                                          Position = ModifyUserPositon,
                                          UserPasswrod = ModifyUserPasswrodOne,
                                          UserRealName = ModifyUserRealName
                                      };
            foreach (var t in PrivilegeInfo )
            {
                if (t.IsSelected) user.UserPirvilegeGroupBelong.Add(t.Value);
            }
           AddUserInfo(user);
        }

        private bool CanExAddUser()
        {
            return DateTime.Now.Ticks-_dtAddUser.Ticks>30000000;
        }

        #endregion

        /// <summary>
        /// 页面加载或是导航显示的时候 需要执行的初始化操作
        /// </summary>
        private void NavOnLoadUserInfo()
        {
            //this.UserItems.Clear();
            //CurrentSelectUser = new UserInfoViewModel();
            //this.RequestAllUserInfomation();
        }

        private void OnCurrenSelectUserChangeInUserInfo()
        {
            StayAtModifyView = false;

           AddVisi = Visibility.Collapsed;

            UpdatePrivilegeUserSelectInfo();
        }

        private void AddNewUserInfo(UserInfomation info)
        {
            foreach (var t in UserItems)
            {
                if (t.UserName == info.UserName)
                {
                    UserItems.Remove(t);
                    UserItems.Add(new UserInfoViewModel(info));
                    return;
                }
            }
            UserItems.Add(new UserInfoViewModel(info));
            if (AddVisi == Visibility.Visible)
            {
                foreach (var t in UserItems)
                {
                    if (t.UserName == info.UserName)
                    {
                        CurrentSelectUser = t;
                    }
                }
            }
        }

        private void UpdatePrivilegeInfo()
        {
            PrivilegeInfo.Clear();
            foreach (var t in PrivilegeInfoNames)
            {
                PrivilegeInfo.Add(new NameIntBool {IsSelected = false, Name = t.Name, Value = t.Value});
            }
        }

        private void UpdatePrivilegeUserSelectInfo()
        {

            UserPrivilegeInfo = "";
            foreach (var f in PrivilegeInfo)
            {
                f.IsSelected = false;
            }
            if (CurrentSelectUser == null) return;
            foreach (var t in CurrentSelectUser.UserPrivilegeList)
            {
                foreach (var f in PrivilegeInfo)
                {
                    if (f.Value == t)
                    {
                        f.IsSelected = true;
                        UserPrivilegeInfo += f.Name + ";";
                    }
                }
            }
        }




    }

    /// <summary>
    /// user privilege
    /// </summary>
    public partial class ModflyOtherUserInfoViewModel
    {

        private ObservableCollection<NameValueInt> _privilegeInfoNames;

        /// <summary>
        /// 权限名称
        /// </summary>
        public ObservableCollection<NameValueInt> PrivilegeInfoNames
        {
            get { return _privilegeInfoNames ?? (_privilegeInfoNames = new ObservableCollection<NameValueInt>()); }
        }

        private NameValueInt _currentSelectPrivilegeInfo;

        public NameValueInt CurrSelectPrivilegeInfoName
        {
            get { return _currentSelectPrivilegeInfo; }
            set
            {
                if (_currentSelectPrivilegeInfo != value)
                {
                    _currentSelectPrivilegeInfo = value;
                    RaisePropertyChanged(() =>CurrSelectPrivilegeInfoName);
                   OnCurrSelectPrivilegeInfoNameChanged();
                }
            }
        }


        private ObservableCollection<PrivilegeInfoViewModel> _privilegeItems;

        /// <summary>
        /// 系统查阅的所有权限信息
        /// </summary>
        public ObservableCollection<PrivilegeInfoViewModel> PrivilegeItems
        {
            get { return _privilegeItems ?? (_privilegeItems = new ObservableCollection<PrivilegeInfoViewModel>()); }
        }


        /// <summary>
        /// 提供初始化请求
        /// </summary>
        private void OnCurrSelectPrivilegeInfoNameChanged()
        {
            if (CurrSelectPrivilegeInfoName == null) return;

            UnCheckAllItem();
           RequestPrivilege(CurrSelectPrivilegeInfoName.Value);

        }


        private void UnCheckAllItem()
        {
            foreach (var t in PrivilegeItems)
            {
                t.IsCanReadOrShow = false;
                t.IsCanWriteOrOperator = false;
            }
        }

        /// <summary>
        /// 当页面导航到此时
        /// </summary>
        private void NavOnLoadPrivilege()
        {
           PrivilegeItems.Clear();
            foreach (var t in PrivilegsInfo.GetAllRegisterControlMoniter)
            {
                if (t.Info == null) continue;
                foreach (var f in t.Info)
                {
                   PrivilegeItems.Add(new PrivilegeInfoViewModel(f.Value.Id, f.Value.Description));
                }
            }
           RequestAllPriGroupInfomation();
        }

        /// <summary>
        /// 更新权限信息
        /// </summary>
        /// <param name="controlId"></param>
        /// <param name="privilegeRight"></param>
        private void SetPrivilegeToItem(int controlId, int privilegeRight)
        {
            foreach (var t in PrivilegeItems)
            {
                if (t.Id == controlId)
                {
                    bool excute = (privilegeRight & 1) == 1;
                    bool wirte = ((privilegeRight >> 1) & 1) == 1;
                    bool read = ((privilegeRight >> 2) & 1) == 1;

                    t.IsCanReadOrShow = read;
                    t.IsCanWriteOrOperator = (excute || wirte);
                    return;
                }
            }
        }

        #region button Save

        private DateTime _dtSavePrivilegeButton;
        private ICommand _cmdSavePrivilegeButton;

        public ICommand CmdSavePrivilegeButton
        {
            get {
                return _cmdSavePrivilegeButton ??
                       (_cmdSavePrivilegeButton =
                        new RelayCommand(ExSavePrivilegeButton, CanExSavePrivilegeButton, true));
            }
        }

        private void ExSavePrivilegeButton()
        {
            _dtSavePrivilegeButton = DateTime.Now;
            var info = new List<Tuple<int, int>>();
            foreach (var t in PrivilegeItems)
            {
                int x = 0;
                if (t.IsCanReadOrShow)
                {
                    x += 4;
                }
                if (t.IsCanWriteOrOperator)
                {
                    x += 3;
                }
                if (x > 0)
                {
                    info.Add(new Tuple<int, int>(t.Id, x));
                }
            }
           UpdatePrivilege(CurrSelectPrivilegeInfoName.Value, info);
        }


        private bool CanExSavePrivilegeButton()
        {
            return CurrSelectPrivilegeInfoName != null && DateTime.Now.Ticks - _dtSavePrivilegeButton.Ticks>30000000;
        }

        #endregion

        #region CmdAddPriGorup

        private DateTime _dtAddPriGorup;
        private ICommand _cCmdAddPriGorup;

        public ICommand CmdAddPriGorup
        {
            get {
                return _cCmdAddPriGorup ??
                       (_cCmdAddPriGorup = new RelayCommand(ExCmdAddPriGorup, ExCanAddPriGorup, true));
            }
        }

        private void ExCmdAddPriGorup()
        {
            _dtAddPriGorup = DateTime.Now;
            var rt = UMessageBoxWantSomefromUser.Show("增加", "请输入权限名称", "");
            if (rt == UMessageBoxWantSomefromUser.CancelReturn)
            {
                return;
            }
            if (PrivilegeInfoNames.Any(t => t.Name == rt))
            {
                UMessageBox.Show("已存在", "您输入的名称已经存在...", UMessageBoxButton.Ok);
                return;
            }
           AddPriGroupInfomation(rt);
        }

        private bool ExCanAddPriGorup()
        {
            return DateTime.Now.Ticks-_dtAddPriGorup.Ticks>30000000;
        }

        #endregion

        #region CmdDeletePriGroup

        private DateTime _dtDelegePriGroup;
        private ICommand _cCmdDeletePriGroup;

        public ICommand CmdDeletePriGroup
        {
            get {
                return _cCmdDeletePriGroup ??
                       (_cCmdDeletePriGroup = new RelayCommand(ExCmdDeletePriGroup, CanExDeletePriGroup, true));
            }
        }

        private void ExCmdDeletePriGroup()
        {
            //数据验证
            _dtDelegePriGroup = DateTime.Now;
            if (CurrSelectPrivilegeInfoName == null) return;
           DeletePriGroupInfomation(CurrSelectPrivilegeInfoName.Value);
        }

        private bool CanExDeletePriGroup()
        {
            return CurrSelectPrivilegeInfoName != null && DateTime.Now.Ticks-_dtDelegePriGroup.Ticks>30000000;
        }

        #endregion

        #region CmdUpdatePriGroup

        private DateTime _dtUpdatePriGroup;
        private ICommand _cCmdUpdatePriGroup;

        public ICommand CmdUpdatePriGroup
        {
            get {
                return _cCmdUpdatePriGroup ??
                       (_cCmdUpdatePriGroup = new RelayCommand(ExCmdUpdatePriGroup, CanExUpdatePriGroup, true));
            }
        }

        private void ExCmdUpdatePriGroup()
        {
            _dtUpdatePriGroup = DateTime.Now;
            //数据验证
            if (CurrSelectPrivilegeInfoName == null) return;
            var rt = UMessageBoxWantSomefromUser.Show("修改该", "请输入权限名称",
                                                                                        CurrSelectPrivilegeInfoName.Name);
            if (rt == UMessageBoxWantSomefromUser.CancelReturn ||
                rt == CurrSelectPrivilegeInfoName.Name)
            {
                return;
            }

           UpdatePriGroupInfomation(CurrSelectPrivilegeInfoName.Value, rt);
        }

        private bool CanExUpdatePriGroup()
        {
            return CurrSelectPrivilegeInfoName != null && DateTime.Now.Ticks - _dtUpdatePriGroup.Ticks>30000000;
        }

        #endregion

        #region CmdCleanAllEnableSelect

        private DateTime _dtCleanAllEnableSelect;
        private ICommand _cCmdCleanAllEnableSelect;

        public ICommand CmdCleanAllEnableSelect
        {
            get
            {
                return _cCmdCleanAllEnableSelect ??
                       (_cCmdCleanAllEnableSelect =
                        new RelayCommand(ExCmdCleanAllEnableSelect, CanCmdCleanAllEnableSelect,
                                         true));
            }
        }

        private void ExCmdCleanAllEnableSelect()
        {
            _dtCleanAllEnableSelect = DateTime.Now;
            foreach (var t in PrivilegeItems) t.IsCanReadOrShow = false;
        }

        private bool CanCmdCleanAllEnableSelect()
        {
            return CurrSelectPrivilegeInfoName != null && DateTime.Now.Ticks - _dtCleanAllEnableSelect.Ticks > 30000000;
        }

        #endregion

        #region CmdSelectAllEnableSelect

        private DateTime _dtSelectAllEnableSelect;
        private ICommand _cCmdSelectAllEnableSelect;

        public ICommand CmdCmdSelectAllEnableSelect
        {
            get
            {
                return _cCmdSelectAllEnableSelect ??
                       (_cCmdSelectAllEnableSelect = new RelayCommand(ExCmdSelectAllEnableSelect,
                                                                      CanCmdSelectAllEnableSelect, true));
            }
        }

        private void ExCmdSelectAllEnableSelect()
        {
            _dtSelectAllEnableSelect = DateTime.Now;
            foreach (var t in PrivilegeItems) t.IsCanReadOrShow = true;
        }

        private bool CanCmdSelectAllEnableSelect()
        {
            return CurrSelectPrivilegeInfoName != null && DateTime.Now.Ticks - _dtSelectAllEnableSelect.Ticks>30000000;
        }

        #endregion

        #region CmdCleanAllOperatorSelect

        private DateTime _dtcleanAllOperatorSelect;
        private ICommand _cCmdCleanAllOperatorSelect;

        public ICommand CmdCleanAllOperatorSelect
        {
            get
            {
                return _cCmdCleanAllOperatorSelect ??
                       (_cCmdCleanAllOperatorSelect = new RelayCommand(ExCmdCleanAllOperatorSelect,
                                                                       CanCmdCleanAllOperatorSelect, true));
            }
        }

        private void ExCmdCleanAllOperatorSelect()
        {
            _dtcleanAllOperatorSelect = DateTime.Now;
            foreach (var t in PrivilegeItems) t.IsCanWriteOrOperator = false;
        }

        private bool CanCmdCleanAllOperatorSelect()
        {
            return CurrSelectPrivilegeInfoName != null && DateTime.Now.Ticks - _dtcleanAllOperatorSelect.Ticks>30000000;
        }

        #endregion

        #region CmdSelectAllOperatorSelect

        private DateTime _dtSelectAllOperatorSelect;
        private ICommand _cCmdSelectAllOperatorSelect;

        public ICommand CmdSelectAllOperatorSelect
        {
            get
            {
                return _cCmdSelectAllOperatorSelect ??
                       (_cCmdSelectAllOperatorSelect = new RelayCommand(ExCmdSelectAllOperatorSelect,
                                                                        CanCmdSelectAllOperatorSelect, true));
            }
        }

        private void ExCmdSelectAllOperatorSelect()
        {
            _dtSelectAllOperatorSelect = DateTime.Now;
            foreach (var t in PrivilegeItems) t.IsCanWriteOrOperator = true;
        }

        private bool CanCmdSelectAllOperatorSelect()
        {
            return CurrSelectPrivilegeInfoName != null && DateTime.Now.Ticks - _dtSelectAllOperatorSelect.Ticks>30000000;
        }

        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class ModflyOtherUserInfoViewModel
    {
        private void InitEvet()
        {
           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.RequestAllUserInfomationId,
                                    PublishEventType.Core);
           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.ModflyUserInfomationId,
                                    PublishEventType.Core);
           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.DeleteUserInfoId,
                                    PublishEventType.Core);
           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.AddNewUserInfoId,
                                    PublishEventType.Core);


           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.RequestAllPriGroupInfomation,
                                    PublishEventType.Core);
           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.ModifyPriGroupInfomation,
                                    PublishEventType.Core);
           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.DeletePriGroupInfomation,
                                    PublishEventType.Core);
           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.AddPriGroupInfomation,
                                    PublishEventType.Core);

           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.RequestGourpInfoPrivilegeById,
                                    PublishEventType.Core);
           AddEventFilterInfo(Sr.PrivilegesCrl.Services.EventIdAssign.ModifyGourpInfoPrivilege,
                                    PublishEventType.Core);
        }

        public override void ExPublishedEvent(
            Microsoft.Practices.Prism.MefExtensions.Event.EventHelper.PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
            if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.RequestAllUserInfomationId)
            {
                var info = args.GetParams()[0] as AllUserInfo;
                if (info == null) return;
                if (!info.Right)
                {
                    UMessageBox.Show("权限不够", "服务器拒绝应答客户端请求的所有用户信息，权限不够...",
                                                                       UMessageBoxButton.Ok);
                    return;
                }

               UserItems.Clear();
                foreach (var t in info.UserInfo)
                {
                   UserItems.Add(new UserInfoViewModel(t));
                }
                if (UserItems.Count > 0)
                {
                    CurrentSelectUser =UserItems[0];
                }
            }

            else if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.ModflyUserInfomationId)
            {
                var info = args.GetParams()[0] as ModfliedUserInfo;
                if (info == null) return;
                if (!info.Modified) //ju jue xiugai
                {
                    if (info.ManagerUserName == UserInfo.UserLoginInfo.UserName)
                    {
                        LogInfo.Log("服务器拒绝修改用户信息:" + info.MsgfromServer);
                    }
                    return;
                }

                foreach (var t in UserItems)
                {
                    if (t.UserName == info.UserInfo.UserName)
                    {
                        t.UserDepartment = info.UserInfo.Department;
                        t.UserPassword = info.UserInfo.UserPasswrod;
                        t.UserPhoneNumber = info.UserInfo.PhoneNumber;
                        t.UserPositon = info.UserInfo.Position;
                        t.UserRealName = info.UserInfo.UserRealName;
                        t.UserPrivilegeList.Clear();
                        foreach (var g in info.UserInfo.UserPirvilegeGroupBelong) t.UserPrivilegeList.Add(g);
                       UpdatePrivilegeUserSelectInfo();
                    }
                }
            }
            else if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.DeleteUserInfoId)
            {
                var info = args.GetParams()[0] as string;
                if (string.IsNullOrEmpty(info)) return;

                bool bolfind = false;
                foreach (var t in UserItems)
                {
                    if (t.UserName == info) bolfind = true;
                }
                if (!bolfind) return;

                if (CurrentSelectUser.UserName == info)
                {
                    foreach (var t in UserItems)
                    {
                        if (t.UserName != info)
                        {
                            CurrentSelectUser = t;
                            break;
                        }
                    }
                }
                foreach (var t in UserItems)
                {
                    if (t.UserName == info)
                    {
                       UserItems.Remove(t);
                        break;
                    }
                }

            }
            else if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.AddNewUserInfoId)
            {
                var info = args.GetParams()[0] as ModfliedUserInfo;
                if (info == null) return;
                if (!info.Modified)
                {
                    if (info.ManagerUserName == UserInfo.UserLoginInfo.UserName)
                    {
                        LogInfo.Log("服务器拒绝增加用户信息:" + info.MsgfromServer);
                    }
                    return;
                }
               AddNewUserInfo(info.UserInfo);
            }





            else if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.AddPriGroupInfomation)
            {
                var info = args.GetParams()[0] as ModifiedPriGroupInfomation;
                if (info == null) return;
                if (!info.Modified)
                {
                    if (info.ManagerUserName == UserInfo.UserLoginInfo.UserName)
                    {
                        LogInfo.Log("服务器拒绝增加权限信息:" + info.MsgfromServer);
                    }
                    return;
                }
               PrivilegeInfoNames.Add(new NameValueInt {Name = info.GroupPriInfoName, Value = info.GourpPriInfoId});

                UpdatePrivilegeInfo();
                UpdatePrivilegeUserSelectInfo();
            }
            else if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.DeletePriGroupInfomation)
            {
                try
                {
                    var info = Convert.ToInt32(args.GetParams()[0].ToString());
                    foreach (var f in PrivilegeInfoNames.Where(f => f.Value == info))
                    {
                        PrivilegeInfoNames.Remove(f);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    LogInfo.Log(ex.ToString());
                }

                UpdatePrivilegeInfo();
                UpdatePrivilegeUserSelectInfo();
            }
            else if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.ModifyPriGroupInfomation)
            {
                var info = args.GetParams()[0] as ModifiedPriGroupInfomation;
                if (info == null) return;
                if (!info.Modified)
                {
                    if (info.ManagerUserName == UserInfo.UserLoginInfo.UserName)
                    {
                        LogInfo.Log("服务器拒绝增修改权限信息:" + info.MsgfromServer);
                    }
                    return;
                }
                foreach (var f in PrivilegeInfoNames)
                {
                    if (f.Value == info.GourpPriInfoId)
                    {
                        f.Name = info.GroupPriInfoName;
                        break;
                    }
                }

                UpdatePrivilegeInfo();
                UpdatePrivilegeUserSelectInfo();
            }
            else if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.RequestAllPriGroupInfomation)
            {
                var info = args.GetParams()[0] as AllPriGroupInfomation;
                if (info == null) return;
               PrivilegeInfoNames.Clear();
                foreach (var t in info.Info)
                {
                   PrivilegeInfoNames.Add(new NameValueInt {Name = t.Item2, Value = t.Item1});
                }
                //this.UpdatePriGroupInfomation();
                if (PrivilegeInfoNames.Count > 0) CurrSelectPrivilegeInfoName = PrivilegeInfoNames[0];

                UpdatePrivilegeInfo();
                UpdatePrivilegeUserSelectInfo();
            }
            else if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.RequestGourpInfoPrivilegeById)
            {
                var info = args.GetParams()[0] as GroupPrivilege;
                if (info == null) return;
                if (CurrSelectPrivilegeInfoName == null) return;
                if (info.TargetPriGroupId != CurrSelectPrivilegeInfoName.Value) return;

                foreach (var t in info.PrivilegeInfo)
                {
                   SetPrivilegeToItem(t.Item1, t.Item2);
                }
            }
            else if (args.EventId == Sr.PrivilegesCrl.Services.EventIdAssign.ModifyGourpInfoPrivilege)
            {
                var info = args.GetParams()[0] as GroupPrivilege;
                if (info == null) return;
                if (CurrSelectPrivilegeInfoName == null) return;
                if (!info.RightOrModified )
                {
                    if (info.ManagerUserName == UserInfo.UserLoginInfo.UserName)
                    {
                        LogInfo.Log("服务器拒绝增修改权限信息:" + info.MsgfromServer);
                    }
                    return;
                }
                if (info.TargetPriGroupId != CurrSelectPrivilegeInfoName.Value) return;

                foreach (var t in info.PrivilegeInfo)
                {
                   SetPrivilegeToItem(t.Item1, t.Item2);
                }
            }


        }
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class ModflyOtherUserInfoViewModel
    {
        private void RequestAllUserInfomation()
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.RequestAllUserInfomation();

        }

        private void UpdateUserInfo(UserInfomation info)
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.ModflyUserInfo(info);
        }

        private void DeleteCurrentUser(string userName)
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.DeleteUserInfomaton(userName);
        }

        private void AddUserInfo(UserInfomation info)
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.AddUserInfo(info);
        }




        private void RequestAllPriGroupInfomation()
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.RequestAllGroupPriInfomation();
        }

        private void AddPriGroupInfomation(string groupName)
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.AddGroupPriInfomation(groupName);
        }

        private void DeletePriGroupInfomation(int groupId)
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.DeleteGroupPriInfomaiton(groupId);
        }

        private void UpdatePriGroupInfomation(int groupId, string groupName)
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.ModflyGroupPriInfomation(groupId, groupName);
        }

        private void UpdatePrivilege(int groupId, List<Tuple<int, int>> privilegeInfo)
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.ModflyGroupPrivilege(groupId, privilegeInfo);
        }

        private void RequestPrivilege(int groupId)
        {
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.RequestGroupPrivilege(groupId);
        }


    }

}
