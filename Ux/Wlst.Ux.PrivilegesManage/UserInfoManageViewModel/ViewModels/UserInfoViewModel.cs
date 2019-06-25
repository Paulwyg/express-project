using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.client;

namespace Wlst.Ux.PrivilegesManage.UserInfoManageViewModel.ViewModels
{
    public class UserInfoViewModel:Cr.Core.CoreServices.ObservableObject
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
                if (value != _userPassword)
                {
                    _userPassword = value;
                    RaisePropertyChanged(() => UserPassword);
                }
            }
        }


        private bool  r;

        public bool R
        {
            get { return r; }
            set
            {
                if (value != r)
                {
                    r = value;
                    RaisePropertyChanged(() => R);
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
                if (value == _userPositon) return;
                _userPositon = value;
                RaisePropertyChanged(() => UserPositon);
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

        private string _areaManage;
        /// <summary>
        /// 拥有管理权限的区域
        /// </summary>
        public string AreaManage
        {
            get { return _areaManage; }
            set
            {
                if (value == _areaManage) return;
                _areaManage = value;
                RaisePropertyChanged(() => AreaManage);
            }
        }

        private List<int> _areaXLst;
        /// <summary>
        /// 拥有X权限的区域ID
        /// </summary>
        public List<int> AreaXLst
        {
            get { return _areaXLst; }
            set
            {
                if (value == _areaXLst) return;
                _areaXLst = value;
                RaisePropertyChanged(() => AreaXLst);
            }
        }

        private List<int> _areaWLst;
        /// <summary>
        /// 拥有W权限的区域ID
        /// </summary>
        public List<int> AreaWLst
        {
            get { return _areaWLst; }
            set
            {
                if (value == _areaWLst) return;
                _areaWLst = value;
                RaisePropertyChanged(() => AreaWLst);
            }
        }

        private List<int> _areaRLst;
        /// <summary>
        /// 拥有R权限的区域ID
        /// </summary>
        public List<int> AreaRLst
        {
            get { return _areaRLst; }
            set
            {
                if (value == _areaRLst) return;
                _areaRLst = value;
                RaisePropertyChanged(() => AreaRLst);
            }
        }

        #region UserMobileRight

        private int _userMobileRight;
        public int UserMobileRight
        {
            get { return _userMobileRight; }
            set
            {
                if(_userMobileRight==value) return;
                _userMobileRight = value;
                RaisePropertyChanged(()=>UserMobileRight);
            }
        }
        #endregion

      



        #endregion

        //public void ReSetRwx(int rwx)
        //{
        //    int tmpr = rwx;

        //    if (tmpr > 3)
        //    {
        //        X = true;
        //        tmpr -= 4;
        //    }
        //    else x = false;
        //    if (tmpr > 1)
        //    {
        //        W = true;
        //        tmpr -= 2;
        //    }
        //    else W = false;
        //    if (tmpr > 0) R = true;
        //    else R = false;
        //}

        //public void ReSetR(List<int> AddAreaR )
        //{           
        //}

        //public void ReSetW(List<int> AddAreaW)
        //{
        //}

        //public void ReSetX(List<int> AddAreaX)
        //{
        //}

        #region controuction
        public UserInfoViewModel(UserInfomation userinfo)
        {
            UserName = userinfo.UserName;
            UserPassword = userinfo.UserPasswrod;
            UserDepartment = userinfo.Department;
            UserPhoneNumber = userinfo.PhoneNumber;
            UserPositon = userinfo.Position;
            UserRealName = userinfo.UserRealName;
            UserMobileRight = userinfo.UserMobileRight;
          
            IsUserUserOperatorCode = userinfo.IsUserUserOperatorCode;
            UserOperatorCode = userinfo.UserOperatorCode;
            if (userinfo.Position.Contains("管理者")&&userinfo.AreaR.Count==0)
            {
                AreaManage = "管理员";
            }
            else
            {
                AreaManage = GetAreaR(userinfo) + GetAreaW(userinfo) + GetAreaX(userinfo);
            }           
            AreaRLst = GetAreaRLst(userinfo);
            AreaWLst = GetAreaWLst(userinfo);
            AreaXLst = GetAreaXLst(userinfo);


        }

        public void UpdaetRwx(UserInfomation userinfo)
        {
            AreaRLst = GetAreaRLst(userinfo);
            AreaWLst = GetAreaWLst(userinfo);
            AreaXLst = GetAreaXLst(userinfo);

        }
        public UserInfoViewModel()
        {
            UserName = "";
            UserPassword = "";
            UserDepartment = "";
            UserPhoneNumber = "";
            UserPositon = "";
            UserRealName = "";
            UserMobileRight = 0;
 
        }
        #endregion

        #region cmd

        private ICommand _cmdModflyUser;
        public ICommand CmdModflyUser
        {
            get { return _cmdModflyUser ?? (_cmdModflyUser = new RelayCommand(ExModflyUser, ExCanModflyUser, true)); }
        }
        private void ExModflyUser()
        {
            if (CmdModflyUser == null) return;
            OnCmdModflyUser(this, EventArgs.Empty);
        }
        public event EventHandler OnCmdModflyUser;
        private bool ExCanModflyUser()
        {
            return true;
        }

        private ICommand _cmdDeleteUser;
        public  ICommand CmdDeleteUser
        {
            get { return _cmdDeleteUser ?? (_cmdDeleteUser = new RelayCommand(ExDeleteUser, ExCanDeleteUser, true)); }
        }

        public event EventHandler OnCmdDeleteUser;
        private void ExDeleteUser()
        {
            if (CmdDeleteUser == null) return;
            OnCmdDeleteUser(this, EventArgs.Empty);
        }
        private bool ExCanDeleteUser()
        {
            return this.AreaManage != "管理员";
        }

        #endregion

        public string GetAreaR(UserInfomation userinfo)
        {
            string areaR = "";
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                if (userinfo.AreaR.Count != 0)
                {
                    areaR = "可查看 ";
                }
            }
            else
            {
                if (userinfo.AreaR.Count == 0)
                {
                    areaR = "";
                }

                else
                {
                    areaR = "查看：";
                    foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                    {
                        if (userinfo.AreaR.Contains(t.Value.AreaId))
                        {
                            areaR += t.Value.AreaId + ",";
                        }
                    }
                    areaR = areaR.Substring(0, areaR.Length - 1);
                }

            }
            return areaR;
        }





        public string GetAreaX(UserInfomation userinfo)
        {
            string areaX = "";
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                if (userinfo.AreaX.Count != 0)
                {
                    areaX = "可操作 ";
                }
            }
            else
            {
                if (userinfo.AreaX.Count == 0)
                {
                    areaX = "";
                }
                else
                {
                    areaX = " 操作：";
                    foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                    {
                        if (userinfo.AreaX.Contains(t.Value.AreaId))
                        {
                            areaX += t.Value.AreaId + ",";
                        }
                    }
                    areaX = areaX.Substring(0, areaX.Length - 1);
                }


            }
            return areaX;
        }

        public string GetAreaW(UserInfomation userinfo)
        {
            string areaW = "";
            if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
            {
                if (userinfo.AreaW.Count != 0)
                {
                    areaW = "可设置 ";
                }
            }
            else
            {
                if (userinfo.AreaW.Count == 0)
                {
                    areaW = "";
                }
                else
                {
                    areaW = " 设置：";
                    foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                    {
                        if (userinfo.AreaW.Contains(t.Value.AreaId))
                        {
                            areaW += t.Value.AreaId + ",";
                        }
                    }
                    areaW = areaW.Substring(0, areaW.Length - 1);
                }
            }
            return areaW;
        }

        public List<int> GetAreaRLst(UserInfomation userinfo)
        {
            List<int> areaR = new List<int>( );
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
            {
                if (userinfo.AreaR.Contains(t.Value.AreaId))
                {
                    areaR.Add(t.Value.AreaId);
                }
            } return areaR;
        }

        public List<int> GetAreaXLst(UserInfomation userinfo)
        {
            List<int> areaX = new List<int>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
            {
                if (userinfo.AreaX.Contains(t.Value.AreaId))
                {
                    areaX.Add(t.Value.AreaId);
                }
            } return areaX;
        }

        public List<int> GetAreaWLst(UserInfomation userinfo)
        {
            List<int> areaW = new List<int>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
            {
                if (userinfo.AreaW.Contains(t.Value.AreaId))
                {
                    areaW.Add(t.Value.AreaId);
                }
            } return areaW;
        } 


    }

    public class AreaInfoViewModel:ObservableObject
    {
        #region 区域权限

        private int _areaId;
        /// <summary>
        /// 区域id
        /// </summary>
        public int AreaId
        {
            get { return _areaId; }
            set
            {
                if (value == _areaId) return;
                _areaId = value;
                RaisePropertyChanged(() => AreaId);
            }
        }

        private string _areaName;
        /// <summary>
        /// 区域id
        /// </summary>
        public string AreaName
        {
            get { return _areaName; }
            set
            {
                if (value == _areaName) return;
                _areaName = value;
                RaisePropertyChanged(() => AreaName);
            }
        }

        private bool _x;
        /// <summary>
        /// 可执行命令
        /// </summary>
        public bool IsAreaX
        {
            get { return _x; }
            set
            {
                if (value == _x) return;
                _x = value;
                RaisePropertyChanged(() => IsAreaX);
                if (IsAreaX == true) IsAreaR = true;
            }
        }

        private bool _r;
        /// <summary>查询读写数据库
        /// </summary>
        public bool IsAreaR
        {
            get { return _r; }
            set
            {
                if (value == _r) return;               
                _r = value;
                RaisePropertyChanged(() => IsAreaR);
                if (IsAreaW == true || IsAreaX == true) IsAreaR = true;
                
            }
        }

        private bool _w;
        /// <summary>
        /// 可读
        /// </summary>
        public bool IsAreaW
        {
            get { return _w; }
            set
            {
                if (value == _w) return;
                _w = value;
                RaisePropertyChanged(() => IsAreaW);
                if (IsAreaW == true) IsAreaR = true;
            }
        }
        #endregion
    }
}
