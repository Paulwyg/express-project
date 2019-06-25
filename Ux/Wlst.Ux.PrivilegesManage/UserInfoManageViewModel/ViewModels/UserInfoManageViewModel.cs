using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Input;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.PrivilegesManage.UserInfoManageViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.PrivilegesManage.UserInfoManageViewModel.ViewModels
{
    [Export(typeof (IIUserInfoManageViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class UserInfoManageViewModel :
        VmEventActionProperyChangedBase,
        IIUserInfoManageViewModel
    {

        public UserInfoManageViewModel()
        {
            Title = "用户管理";
            InitAction();
        }


       
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
                _currentSelectUser = value;
                RaisePropertyChanged(() => CurrentSelectUser);
            }
        }

        /// <summary>
        /// 当该用户只有一个区域的时候，显示checkboxs
        /// </summary>
        private Visibility _areaOne;

        public Visibility AreaOne
        {
            get { return _areaOne; }
            set
            {
                if (value != _areaOne)
                {
                    _areaOne = value;
                    this.RaisePropertyChanged(() => this.AreaOne);
                }
            }
        }

        /// <summary>
        /// 当该用户可看多个区域的时候，显示GridView
        /// </summary>
        private Visibility _areaMulti;

        public Visibility AreaMulti
        {
            get { return _areaMulti; }
            set
            {
                if (value != _areaMulti)
                {
                    _areaMulti = value;
                    this.RaisePropertyChanged(() => this.AreaMulti);
                }
            }
        }

        public override void NavOnLoadr(params object[] parsObjects)
        {

            RequestAllUserInfo();
            RequestAreaInfo();
            AddEnable = true;
        }


    }

    public partial class UserInfoManageViewModel
    {
        private long _addrecguid;
        private long _addsndguid;

        #region property

        private ObservableCollection<AreaInfoViewModel> _addareaPrivilege;

        public ObservableCollection<AreaInfoViewModel> AddAreaPrivilege
        {
            get { return _addareaPrivilege ?? (_addareaPrivilege = new ObservableCollection<AreaInfoViewModel>()); }
            set
            {
                _addareaPrivilege = value;
                
                RaisePropertyChanged(() => AddAreaPrivilege);
                
            }
        }

        

        private bool _isBatch;
        /// <summary>
        /// 批量操作
        /// </summary>
        public bool IsBatch
        {
            get { return _isBatch; }
            set
            {
                if (value != _isBatch)
                {
                    _isBatch = value;                  
                    this.RaisePropertyChanged(() => this.IsBatch);
                }
            }
        }

        private List<int> _areaR;
        
        public List<int> AddAreaR
        {
            get
            {
                if (_areaR == null)
                    _areaR = new List<int>();
                return _areaR;
            }
            
        }

        private List<int> _areaW;

        public List<int> AddAreaW
        {
            get
            {
                if (_areaW == null)
                    _areaW = new List<int>();
                return _areaW;
            }
        }

        private List<int> _areaX;

        public List<int> AddAreaX
        {
            get
            {
                if (_areaX == null)
                    _areaX = new List<int>();
                return _areaX;
            }
        } 


        private string _adduserName;
        /// <summary>
        /// 添加用户名
        /// </summary>
        [StringLength(10, ErrorMessage = "用户名不得超过10个字符")]       
        public string AddUserName
        {
            get { return _adduserName; }
            set
            {
                if (_adduserName != value)
                {
                    _adduserName = value;
                    RaisePropertyChanged(() => AddUserName);
                }
            }
        }

        private string _addUserPasswordOne;

        public string AddUserPasswrodOne
        {
            get { return _addUserPasswordOne; }
            set
            {
                if (value != _addUserPasswordOne)
                {
                    _addUserPasswordOne = value;
                    RaisePropertyChanged(() => AddUserPasswrodOne);
                }
            }
        }

        private string _addUserPasswrodTwo;

        public string AddUserPasswrodTwo
        {
            get { return _addUserPasswrodTwo; }
            set
            {
                if (value == _addUserPasswrodTwo) return;
                _addUserPasswrodTwo = value;
                RaisePropertyChanged(() => AddUserPasswrodTwo);
            }
        }


        private string _addUserRealName;

        public string AddUserRealName
        {
            get { return _addUserRealName; }
            set
            {
                if (value == _addUserRealName) return;
                _addUserRealName = value;
                RaisePropertyChanged(() => AddUserRealName);
            }
        }

        private string _addUserPhoneNumber;

        public string AddUserPhoneNumber
        {
            get { return _addUserPhoneNumber; }
            set
            {
                if (value == _addUserPhoneNumber) return;
                _addUserPhoneNumber = value;
                RaisePropertyChanged(() => AddUserPhoneNumber);
            }
        }

        private string _addUserDepartment;

        public string AddUserDepartment
        {
            get { return _addUserDepartment; }
            set
            {
                if (value == _addUserDepartment) return;
                _addUserDepartment = value;
                RaisePropertyChanged(() => AddUserDepartment);
            }
        }

        private string _addUserPositon;

        public string AddUserPositon
        {
            get { return _addUserPositon; }
            set
            {
                if (value == _addUserPositon) return;
                _addUserPositon = value;
                RaisePropertyChanged(() => AddUserPositon);
            }
        }

        private string _inquiryKeyWord;

    [StringLength(5,ErrorMessage = "just for test")]
        public string InquiryKeyWord
        {
            get { return _inquiryKeyWord; }
            set
            {
                if (value == _inquiryKeyWord) return;
                _inquiryKeyWord = value;
                RaisePropertyChanged(() => InquiryKeyWord);
            }
        }

        private bool _addEnable;

        public bool AddEnable
        {
            get { return _addEnable; }
            set
            {
                if (_addEnable != value)
                {
                    _addEnable = value;
                    RaisePropertyChanged(() => AddEnable);
                }
            }
        }

        #endregion

        #region UserMobileRight

        private int _userMobileRight;

        public int UserMobileRight
        {
            get { return _userMobileRight; }
            set
            {
                if (_userMobileRight == value) return;
                _userMobileRight = value;
                RaisePropertyChanged(() => UserMobileRight);
            }
        }

        #endregion

        #region Command

        private DateTime[] _dateTimes = new DateTime[5];

        private ICommand _btnInquiry;

        public ICommand BtnInquiry
        {
            get { return _btnInquiry ?? (_btnInquiry = new RelayCommand(ExBtnInquiry, CanBtnInquiry, true)); }
        }

        private void ExBtnInquiry()
        {
            _dateTimes[0] = DateTime.Now;
            RequestAllUserInfo();
        }

        private bool CanBtnInquiry()
        {
            return DateTime.Now.Ticks - _dateTimes[0].Ticks > 30000000;
        }

        private ICommand _btnReSetInquiryKeyWord;

        public ICommand BtnReSetInquiryKeyWord
        {
            get
            {
                return _btnReSetInquiryKeyWord ??
                       (_btnReSetInquiryKeyWord =
                        new RelayCommand(ExBtnReSetInquiryKeyWord, CanBtnReSetInquiryKeyWord, true));
            }
        }

        private void ExBtnReSetInquiryKeyWord()
        {
            _dateTimes[1] = DateTime.Now;
            InquiryKeyWord = string.Empty;

            var ar = new PublishEventArgs
                         {
                             EventId = PrivilegesManage.Services.EventIdAssign.ResetAnimationEventId,
                             EventType = PublishEventType.None
                         };
            EventPublish.PublishEvent(ar);
            AddEnable = true;
            ExBtnReSetUser();
        }

        private bool CanBtnReSetInquiryKeyWord()
        {
            return DateTime.Now.Ticks - _dateTimes[1].Ticks > 30000000;
        }

        private ICommand _btnAddUser;

        public ICommand BtnAddUser
        {
            get { return _btnAddUser ?? (_btnAddUser = new RelayCommand(ExBtnAddUser, CanBtnAddUser, true)); }
        }

        private void ExBtnAddUser()
        {
            _dateTimes[2] = DateTime.Now;
            AddEnable = false;


            var ar = new PublishEventArgs
                         {
                             EventId = PrivilegesManage.Services.EventIdAssign.AddAnimationEventId,
                             EventType = PublishEventType.None
                         };
            EventPublish.PublishEvent(ar);
        }

        private bool CanBtnAddUser()
        {
            return DateTime.Now.Ticks - _dateTimes[2].Ticks > 30000000;
        }


        private ICommand _btnSaveUser;

        public ICommand BtnSaveUser
        {
            get { return _btnSaveUser ?? (_btnSaveUser = new RelayCommand(ExBtnSaveUser, CanBtnSaveUser, true)); }
        }

        private void ExBtnSaveUser()
        {
            _dateTimes[3] = DateTime.Now;
            //数据验证
            if (string.IsNullOrEmpty(AddUserName) || AddUserName.Trim() == "")
            {
                UMessageBox.Show("错误", "用户名不能为空", UMessageBoxButton.Ok);
                return;
            }
            if (UserItems.Any(t => t.UserName == AddUserName))
            {
                UMessageBox.Show("错误", "您输入的用户名已经存在，请重新输入用户名...",
                                 UMessageBoxButton.Ok);
                return;
            }
            if (AddUserPasswrodOne != AddUserPasswrodTwo)
            {

                UMessageBox.Show("错误", "两次输入密码不一致，请重新输入!", UMessageBoxButton.Ok);
                AddUserPasswrodOne = AddUserPasswrodTwo = "";
                return;
            }
            //int rwx = X ? 4 : 0;
            //rwx += W ? 2 : 0;
            //rwx += R ? 1 : 0;
            GetAddAreaR();
            GetAddAreaW();
            GetAddAreaX();
            
            var user = new UserInfomation
                           {
                               UserName = AddUserName,
                               Department = AddUserDepartment,
                               PhoneNumber = AddUserPhoneNumber,
                               Position = AddUserPositon,
                               UserPasswrod = AddUserPasswrodOne,
                               UserRealName = AddUserRealName,
                               UserMobileRight = UserMobileRight,
                               IsUserUserOperatorCode = this.IsUserUserOperatorCode,
                               UserOperatorCode = this.UserOperatorCode,
                               AreaR = AddAreaR,
                               AreaW = AddAreaW,
                               AreaX = AddAreaX
                           };

            _addsndguid = AddUserInfo(user);
        }

        private bool CanBtnSaveUser()
        {
            return DateTime.Now.Ticks - _dateTimes[3].Ticks > 30000000;
        }

        private void AddNewUserInfo(UserInfomation info)
        {
            if (_addrecguid == _addsndguid)
            {
                UMessageBox.Show("用户增加成功", "是否立即显示?", UMessageBoxButton.OkCancel);
            }

            foreach (var t in UserItems)
            {
                if (t.UserName == info.UserName)
                {
                    UserItems.Remove(t);
                    UserItems.Add(new UserInfoViewModel(info));

                    UserItems[UserItems.Count - 1].OnCmdDeleteUser += OnItemCmdDelete;
                    UserItems[UserItems.Count - 1].OnCmdModflyUser += OnItemCmdModifly;
                    return;
                }
            }
            UserItems.Add(new UserInfoViewModel(info));
            UserItems[UserItems.Count - 1].OnCmdDeleteUser += OnItemCmdDelete;
            UserItems[UserItems.Count - 1].OnCmdModflyUser += OnItemCmdModifly;

            ExBtnReSetUser();
            AddEnable = true;

            var ar = new PublishEventArgs
                         {
                             EventId = PrivilegesManage.Services.EventIdAssign.ResetAnimationEventId,
                             EventType = PublishEventType.None
                         };
            EventPublish.PublishEvent(ar);
        }


        private ICommand _btnReSetUser;

        public ICommand BtnReSetUser
        {
            get { return _btnReSetUser ?? (_btnReSetUser = new RelayCommand(ExBtnReSetUser, CanBtnReSetUser, true)); }
        }

        private void ExBtnReSetUser()
        {
            _dateTimes[4] = DateTime.Now;
            AddUserName = "";
            AddUserPasswrodOne = "";
            AddUserPasswrodTwo = "";
            AddUserPhoneNumber = "";
            AddUserPositon = "";
            AddUserRealName = "";
            AddUserDepartment = "";
            R = true;
            W = false;
            X = false;
            this.UserOperatorCode = "";
            this.IsUserUserOperatorCode = false;
            foreach (var f in this.AddAreaPrivilege )
            {
                f.IsAreaR =f.IsAreaW=f.IsAreaX= false;
                
            }

        }

        private bool CanBtnReSetUser()
        {
            return DateTime.Now.Ticks - _dateTimes[4].Ticks > 30000000;
        }

        private void AddCmdToUserItems()
        {
            foreach (var item in UserItems)
            {
                item.OnCmdModflyUser += OnItemCmdModifly;
                item.OnCmdDeleteUser += OnItemCmdDelete;
            }
        }

        private void OnItemCmdModifly(object sender, EventArgs args)
        {
            var info = sender as UserInfoViewModel;
            if (info == null) return;
            
            //初始化数据
            UpdateUserName = info.UserName;
            UpdateUserPasswrodOne = info.UserPassword;
            UpdateUserPasswrodTwo = info.UserPassword;
            UpdateUserDepartment = info.UserDepartment;
            UpdateUserPhoneNumber = info.UserPhoneNumber;
            UpdateUserPositon = info.UserPositon;
            UpdateUserRealName = info.UserRealName;
            UpdateUserMobileRight = info.UserMobileRight;
            IsUserUserOperatorCode = info.IsUserUserOperatorCode;
            UserOperatorCode = info.UserOperatorCode;
            if (AreaMulti == Visibility.Visible)
            {
                foreach (var f in UpdateAreaPrivilege)
                {
                    if (info.AreaManage.Contains( "管理"))
                    {
                        f.IsAreaR = f.IsAreaW = f.IsAreaX = true;
                    }
                    else
                    {
                        f.IsAreaR = f.IsAreaW = f.IsAreaX = false;
                        if (info.AreaRLst.Contains(f.AreaId))
                        {
                            f.IsAreaR = true;
                        }
                        if (info.AreaWLst.Contains(f.AreaId))
                        {
                            f.IsAreaW = true;
                        }
                        if (info.AreaXLst.Contains(f.AreaId))
                        {
                            f.IsAreaX = true;
                        }
                    }

                }
            }
            else
            {
                if (info.AreaManage == "管理员")
                {
                    RUpdate = WUpdate = XUpdate = true;
                }
                else
                {
                    RUpdate = WUpdate = XUpdate = false;
                    if (info.AreaManage.Contains("可查看"))
                    {
                        RUpdate = true;
                    }
                    if (info.AreaManage.Contains("可操作"))
                    {
                        XUpdate = true;
                    }
                    if (info.AreaManage.Contains("可设置"))
                    {
                        WUpdate = true;
                    }
                }
            }

            //R = info.R;
            //W = info.W;
            //X = info.X;


            //发布事件，调入更新界面
            AddEnable = true;
            var ar = new PublishEventArgs
                         {
                             EventId = PrivilegesManage.Services.EventIdAssign.FleshAnimationEventId,
                             EventType = PublishEventType.None
                         };
            EventPublish.PublishEvent(ar);

        }

        private void OnItemCmdDelete(object sender, EventArgs args)
        {
            var info = sender as UserInfoViewModel;
            if (info == null) return;
            if(info.AreaManage == "管理员")
            {
                WlstMessageBox.Show("该用户为管理员，无法删除", WlstMessageBoxType.YesNo);
                return;
            }
           
            var infoss = WlstMessageBox.Show("确认删除", "确认删除用户 : " + info.UserName, WlstMessageBoxType.YesNo);
            if (infoss == WlstMessageBoxResults.No) return;

            DeleteUserInfo(info.UserName);

        }

        #endregion

    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    public partial class UserInfoManageViewModel
    {
        private long _updatesndguid;
        private long _updaterecguid;

        #region property

        private ObservableCollection<AreaInfoViewModel> _updateareaPrivilege;

        public ObservableCollection<AreaInfoViewModel> UpdateAreaPrivilege
        {
            get { return _updateareaPrivilege ?? (_updateareaPrivilege = new ObservableCollection<AreaInfoViewModel>()); }
            set
            {
                _updateareaPrivilege = value;
                RaisePropertyChanged(() => UpdateAreaPrivilege);
            }
        }

        private string _updateuserName;

        
        public string UpdateUserName
        {
            get { return _updateuserName; }
            set
            {
                if (_updateuserName != value)
                {
                    _updateuserName = value;
                    RaisePropertyChanged(() => UpdateUserName);
                }
            }
        }

        private string _updateUserPasswordOne;

        public string UpdateUserPasswrodOne
        {
            get { return _updateUserPasswordOne; }
            set
            {
                if (value != _updateUserPasswordOne)
                {
                    _updateUserPasswordOne = value;
                    RaisePropertyChanged(() => UpdateUserPasswrodOne);
                }
            }
        }

        private string _updateUserPasswrodTwo;

        public string UpdateUserPasswrodTwo
        {
            get { return _updateUserPasswrodTwo; }
            set
            {
                if (value != _updateUserPasswrodTwo)
                {
                    _updateUserPasswrodTwo = value;
                    RaisePropertyChanged(() => UpdateUserPasswrodTwo);
                }
            }
        }


        private string _updateUserRealName;

        public string UpdateUserRealName
        {
            get { return _updateUserRealName; }
            set
            {
                if (value != _updateUserRealName)
                {
                    _updateUserRealName = value;
                    RaisePropertyChanged(() => UpdateUserRealName);
                }
            }
        }

        private string _updateUserPhoneNumber;

        public string UpdateUserPhoneNumber
        {
            get { return _updateUserPhoneNumber; }
            set
            {
                if (value != _updateUserPhoneNumber)
                {
                    _updateUserPhoneNumber = value;
                    RaisePropertyChanged(() => UpdateUserPhoneNumber);
                }
            }
        }

        private string _updateUserDepartment;

        public string UpdateUserDepartment
        {
            get { return _updateUserDepartment; }
            set
            {
                if (value != _updateUserDepartment)
                {
                    _updateUserDepartment = value;
                    RaisePropertyChanged(() => UpdateUserDepartment);
                }
            }
        }

        private string _updateUserPositon;

        public string UpdateUserPositon
        {
            get { return _updateUserPositon; }
            set
            {
                if (value != _updateUserPositon)
                {
                    _updateUserPositon = value;
                    RaisePropertyChanged(() => UpdateUserPositon);
                }
            }
        }

        private string _uUserOperatorCode;

        /// <summary>
        /// 用户手机登陆操作输入的操作码
        /// </summary>
        public string UserOperatorCode
        {
            get { return _uUserOperatorCode; }
            set
            {
                if (value == _uUserOperatorCode) return;
                _uUserOperatorCode = value;
                RaisePropertyChanged(() => UserOperatorCode);
            }
        }

        private bool _uIsUserUserOperatorCode;

        /// <summary>
        /// 是否用户手机登陆需要输入操作码
        /// </summary>
        public bool IsUserUserOperatorCode
        {
            get { return _uIsUserUserOperatorCode; }
            set
            {
                if (value == _uIsUserUserOperatorCode) return;
                _uIsUserUserOperatorCode = value;
                RaisePropertyChanged(() => IsUserUserOperatorCode);
            }
        }

        #region UpdateUserMobileRight

        private int _updateUserMobileRight;

        public int UpdateUserMobileRight
        {
            get { return _updateUserMobileRight; }
            set
            {
                if (_updateUserMobileRight == value) return;
                _updateUserMobileRight = value;
                RaisePropertyChanged(() => UpdateUserMobileRight);
            }
        }

        private List<int> _updateareaR;

        public List<int> UpdateAreaR
        {
            get
            {
                if (_updateareaR == null)
                    _updateareaR = new List<int>();
                return _updateareaR;
            }
            
        }

        private List<int> _updateareaW;

        public List<int> UpdateAreaW
        {
            get
            {
                if (_updateareaW == null)
                    _updateareaW = new List<int>();
                return _updateareaW;
            }
        }

        private List<int> _updateareaX;

        public List<int> UpdateAreaX
        {
            get
            {
                if (_updateareaX == null)
                    _updateareaX = new List<int>();
                return _updateareaX;
            }
        } 

        private bool r;

        public bool R
        {
            get { return r; }
            set
            {
                if (value != r)
                {
                    r = value;
                    RaisePropertyChanged(() => R);
                    if (W == true || X == true) R = true;
                }
            }
        }

        private bool w;

        public bool W
        {
            get { return w; }
            set
            {
                if (value != w)
                {
                    w = value;
                    RaisePropertyChanged(() => W);
                    if (W == true) R = true;
                }
            }
        }

        private bool x;

        public bool X
        {
            get { return x; }
            set
            {
                if (value != x)
                {
                    x = value;
                    RaisePropertyChanged(() => X);
                    if (X == true) R = true;
                }
            }
        }

        private bool rUpdate;

        public bool RUpdate
        {
            get { return rUpdate; }
            set
            {
                if (value != rUpdate)
                {
                    rUpdate = value;
                    RaisePropertyChanged(() => RUpdate);
                    if (WUpdate == true || XUpdate == true) RUpdate = true;
                }
            }
        }

        private bool wUpdate;

        public bool WUpdate
        {
            get { return wUpdate; }
            set
            {
                if (value != wUpdate)
                {
                    wUpdate = value;
                    RaisePropertyChanged(() => WUpdate);
                    if (WUpdate == true) RUpdate = true;
                }
            }
        }

        private bool xUpdate;

        public bool XUpdate
        {
            get { return xUpdate; }
            set
            {
                if (value != xUpdate)
                {
                    xUpdate = value;
                    RaisePropertyChanged(() => XUpdate);
                    if (XUpdate == true) RUpdate = true;
                }
            }
        }

        #endregion

        #endregion

        #region ICommand

        private ICommand _btnFreshUser;

        public ICommand BtnFreshUser
        {
            get { return _btnFreshUser ?? (_btnFreshUser = new RelayCommand(ExBtnFresh, CanBtnFresh, true)); }
        }

        private void ExBtnFresh()
        {
            if (UpdateUserPasswrodOne != UpdateUserPasswrodTwo)
            {
                UMessageBox.Show("提醒", "两次输入密码不一致，请从新输入！", UMessageBoxButton.Ok);
                UpdateUserPasswrodOne = UpdateUserPasswrodTwo = "";
                return;
            }
           
            GetUpdateAreaX();
            GetUpdateAreaW();
            GetUpdateAreaR();

            //int rwx = X ? 4 : 0;
            //rwx += W ? 2 : 0;
            //rwx += R ? 1 : 0;
            var info = new UserInfomation
                           {
                               UserName = UpdateUserName,
                               UserPasswrod = UpdateUserPasswrodOne,
                               Department = UpdateUserDepartment,
                               PhoneNumber = UpdateUserPhoneNumber,
                               UserRealName = UpdateUserRealName,
                               Position = UpdateUserPositon,
                               UserMobileRight = UpdateUserMobileRight,
                               IsUserUserOperatorCode = IsUserUserOperatorCode,
                               UserOperatorCode = UserOperatorCode,
                               AreaR = UpdateAreaR,
                               AreaW = UpdateAreaW,
                               AreaX = UpdateAreaX
                               //IsUserUserOperatorC
                           };

            _updatesndguid = UpdateUserInfo(info);
        }

        private bool CanBtnFresh()
        {
            return true;
        }

        private ICommand _btnCancel;

        public ICommand BtnCancel
        {
            get { return _btnCancel ?? (_btnCancel = new RelayCommand(ExBtnCancel, CanBtnCancel, true)); }
        }

        private void ExBtnCancel()
        {
            UpdateUserDepartment = "";
            UpdateUserName = "";
            UpdateUserPasswrodOne = "";
            UpdateUserPasswrodTwo = "";
            UpdateUserPhoneNumber = "";
            UpdateUserPositon = "";
            UpdateUserRealName = "";
            IsUserUserOperatorCode = true;

            //发布事件，返回原界面
            var ar = new PublishEventArgs
                         {
                             EventId = PrivilegesManage.Services.EventIdAssign.CancelFleshAnimationEventId,
                             EventType = PublishEventType.None
                         };
            EventPublish.PublishEvent(ar);

        }

        private bool CanBtnCancel()
        {
            return true;
        }

        #endregion



        private void GetFilterUsers()
        {
            if (InquiryKeyWord == null || InquiryKeyWord.Trim().Length < 1) return;

            var list = UserItems.Select(item => item.UserName).ToList();
            var res = SearchFunc.GetSearchResult(InquiryKeyWord, list);
            for (int i = 0; i < UserItems.Count; i++)
            {
                if (!res.Contains(UserItems[i].UserName))
                {
                    UserItems.RemoveAt(i);
                    i--;
                }
            }
        }

    }


    /// <summary>
    /// Event
    /// </summary>
    public partial class UserInfoManageViewModel
    {
        private void InitAction()
        {

            ProtocolServer.RegistProtocol(Wlst.Sr.ProtocolPhone.LxLogin.wst_request_user_info,
                                          OnRequestUser, typeof (UserInfoManageViewModel), this);
            ProtocolServer.RegistProtocol(Wlst.Sr.ProtocolPhone.LxLogin.wst_add_or_update_user,
                                          OnAddOrUpdateUser, typeof (UserInfoManageViewModel), this);
            ProtocolServer.RegistProtocol(Wlst.Sr.ProtocolPhone.LxLogin.wst_delete_user,
                                          OnDeleteUser, typeof (UserInfoManageViewModel), this);


        }

        #region AddArea UpdateArea

        private void GetAddAreaR()
        {
            this.AddAreaR.Clear();
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                if (R == true)
                {
                    this.AddAreaR.Add(Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[0].AreaId);
                }
            }
            else
            {
                foreach (var f in this.AddAreaPrivilege)
                {
                    if (f.IsAreaR == true)
                    {
                        this.AddAreaR.Add(f.AreaId);
                    }
                }
            }

        }

        private void GetAddAreaW()
        {
            this.AddAreaW.Clear();
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                if (W == true)
                {
                    this.AddAreaW.Add(Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[0].AreaId);
                }
            }
            else
            {
                foreach (var f in this.AddAreaPrivilege)
                {
                    if (f.IsAreaW == true)
                    {
                        this.AddAreaW.Add(f.AreaId);
                    }
                }
            }
        }

        private void GetAddAreaX()
        {
            this.AddAreaX.Clear();
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                if (X == true)
                {
                    this.AddAreaX.Add(Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[0].AreaId);
                }
            }
            else
            {
                foreach (var f in this.AddAreaPrivilege)
                {
                    if (f.IsAreaX == true)
                    {
                        this.AddAreaX.Add(f.AreaId);
                    }
                }
            }
        }

        private void GetUpdateAreaR()
        {
            this.UpdateAreaR.Clear();
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                if (RUpdate == true)
                {
                    this.UpdateAreaR.Add(Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[0].AreaId);
                }
            }
            else
            {
                foreach (var f in this.UpdateAreaPrivilege)
                {
                    if (f.IsAreaR == true)
                    {
                        this.UpdateAreaR.Add(f.AreaId);
                    }
                }
            }
        }

        private void GetUpdateAreaW()
        {
            this.UpdateAreaW.Clear();
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                if (WUpdate == true)
                {
                    this.UpdateAreaW.Add(Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[0].AreaId);
                }
            }
            else
            {
                foreach (var f in this.UpdateAreaPrivilege)
                {
                    if (f.IsAreaW == true)
                    {
                        this.UpdateAreaW.Add(f.AreaId);
                    }
                }
            }               
        }

        private void GetUpdateAreaX()
        {
            this.UpdateAreaX.Clear();
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                if (XUpdate == true)
                {
                    this.UpdateAreaX.Add(Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[0].AreaId);
                }
            }
            else
            {
                foreach (var f in this.UpdateAreaPrivilege)
                {
                    if (f.IsAreaX == true)
                    {
                        this.UpdateAreaX.Add(f.AreaId);
                    }
                }
            }
        }

        #endregion

        public void OnRequestUser(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (IsViewShowd == false) return;

            var info = infos.WstLoginRequestUserInfo;
            if (info == null) return;
            if (!string.IsNullOrEmpty(info.TargetUserName)) return;

            UserItems.Clear();
            foreach (var t in info.UserInfo)
            {
                UserItems.Add(new UserInfoViewModel(t));
                
            }
           
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D ==true)
            //{
                //foreach (var f in UserItems)
                //{
                //    //if(f.UserName == Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName)
                //    //{
                //    //    f.AreaManage = "管理员";             
                //    //}
                //    if(f.UserPositon.Contains("管理者"))
                //    {
                //        f.AreaManage = "管理员";
                //    }
                //}
            //}
            if (UserItems.Count > 0)
            {
                CurrentSelectUser = UserItems[0];
            }
            AddCmdToUserItems();

            GetFilterUsers();
            PublishActionEvent();
        }

        public void OnAddOrUpdateUser(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (IsViewShowd == false) return;

            var info = infos.WstLoginAddOrUpdateUserInfo ;
            if (info == null) return;


            var guid = infos.Head.Gid;
            if (info.Op == 1)
            {

                _addrecguid = long.Parse(guid.ToString());


                if (!info.SvrRtnModified)
                {
                    LogInfo.Log("服务器拒绝增加用户信息:" + info.SvrRtnMsg);
                    return;
                }
                AddNewUserInfo(info.UserInfo);
            }
            else
            {
                _updaterecguid = long.Parse(guid.ToString());

                if (!info.SvrRtnModified) //ju jue xiugai
                {
                    LogInfo.Log("服务器拒绝修改用户信息:" + info.SvrRtnMsg);
                    return;
                }
                if (_updatesndguid == _updaterecguid)
                {
                    UMessageBox.Show("用户更新", "更新成功！", UMessageBoxButton.Ok);
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
                        t.UserMobileRight = info.UserInfo.UserMobileRight;
                        t.IsUserUserOperatorCode = info.UserInfo.IsUserUserOperatorCode;
                        t.UserOperatorCode = info.UserInfo.UserOperatorCode;
                        if (info.UserInfo.Position.Contains("管理者"))
                        {
                            t.AreaManage = "管理员";
                        }
                        else
                        {
                            t.AreaManage = t.GetAreaR(info.UserInfo) + t.GetAreaW(info.UserInfo) +
                                           t.GetAreaX(info.UserInfo);
                        }

                       t.UpdaetRwx(info .UserInfo );

                        //t.ReSetR(info.UserInfo.AreaR);
                        //t.ReSetW(info.UserInfo.AreaW);
                        //t.ReSetX(info.UserInfo.AreaX);
                    }
                }
            }
            PublishActionEvent();
        }

        public void OnDeleteUser(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (IsViewShowd == false) return;


            var info = infos.WstLoginDeleteUserInfo;
            if (info == null) return;
            if (string.IsNullOrEmpty(info.TargetUserName)) return;

            var bolfind = false;
            foreach (var t in UserItems)
            {
                if (t.UserName == info.TargetUserName) bolfind = true;
            }
            if (!bolfind) return;

            if (CurrentSelectUser.UserName == info.TargetUserName)
            {
                foreach (var t in UserItems.Where(t => t.UserName != info.TargetUserName))
                {
                    CurrentSelectUser = t;
                    break;
                }
            }
            foreach (var t in UserItems.Where(t => t.UserName == info.TargetUserName))
            {
                UserItems.Remove(t);
                break;
            }
            PublishActionEvent();
        }


        private void PublishActionEvent()
        {
            //发布事件，返回原界面
            var ar = new PublishEventArgs
                         {
                             EventId = PrivilegesManage.Services.EventIdAssign.CancelFleshAnimationEventId,
                             EventType = PublishEventType.None
                         };
            EventPublish.PublishEvent(ar);
        }

    }


    /// <summary>
    /// Socket
    /// </summary>
    public partial class UserInfoManageViewModel
    {
        private void RequestAllUserInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxLogin.wst_request_user_info;
            info.WstLoginRequestUserInfo.TargetUserName = string.Empty;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void RequestAreaInfo()
        {
            this.AddAreaPrivilege.Clear();
            this.UpdateAreaPrivilege.Clear();
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                AreaMulti = Visibility.Collapsed;
                AreaOne = Visibility.Visible;
            }
            else
            {
                AreaMulti = Visibility.Visible;
                AreaOne = Visibility.Collapsed;
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    this.AddAreaPrivilege.Add(new AreaInfoViewModel()
                                                  {AreaId = f.Value.AreaId, AreaName = f.Value.AreaName});
                    this.UpdateAreaPrivilege.Add(new AreaInfoViewModel()
                                                     {AreaId = f.Value.AreaId, AreaName = f.Value.AreaName});
                }
            }
        }

        private long AddUserInfo(UserInfomation user)
        {
            // Sr.PrivilegesCrl.Services.PrivilegeInfoServer.AddUserInfo(info);

            var info = Wlst.Sr.ProtocolPhone.LxLogin.wst_add_or_update_user;
            info.WstLoginAddOrUpdateUserInfo.Op = 1;
            info.WstLoginAddOrUpdateUserInfo.UserInfo = user;
            SndOrderServer.OrderSnd(info, 10, 2);
            return info.Head.Gid;
        }

        private void DeleteUserInfo(string username)
        {
            var info = Wlst.Sr.ProtocolPhone.LxLogin.wst_delete_user;
            info.WstLoginDeleteUserInfo.TargetUserName = username;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private long UpdateUserInfo(UserInfomation user)
        {

            var info = Wlst.Sr.ProtocolPhone.LxLogin.wst_add_or_update_user;
            info.WstLoginAddOrUpdateUserInfo .Op = 2;
            info.WstLoginAddOrUpdateUserInfo.UserInfo = user;
            SndOrderServer.OrderSnd(info, 10, 2);
            return info.Head.Gid;
        }

    }
}
