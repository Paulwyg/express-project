using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Ux.EquipemntLightFault.UserFaultSettingByAdmin.Services;
using Wlst.client;
using Wlst.mobile;

namespace Wlst.Ux.EquipemntLightFault.UserFaultSettingByAdmin.ViewModel
{
    [Export(typeof (IIUserFaultSettingByAdminVm))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class UserFaultSettingByAdminVm : Wlst.Cr.Core.CoreServices.ObservableObject,
                                                     IIUserFaultSettingByAdminVm
    {
        public UserFaultSettingByAdminVm()
        {
            this.InitAction();
        }

        private bool _isViewShow = false;
        private bool _isloaduseralready = false;
        public void NavOnLoad(params object[] parsObjects)
        {
            _isloaduseralready = false;
            IsUserCanEnable = true;
            _isViewShow = true;
            Records.Clear();
            RequestUserName();
            ShowMsg =  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 正在请求用户信息...";


        }

        public void OnUserHideOrClosing()
        {
            _isloaduseralready = false;
            _isViewShow = false;
            this.Records.Clear();
            this.Items.Clear();
        }

        private ObservableCollection<OneFaultTypeInfo> _record;

        public ObservableCollection<OneFaultTypeInfo> Records
        {
            get
            {
                if (_record == null)
                    _record = new ObservableCollection<OneFaultTypeInfo>();
                return _record;
            }
            set
            {
                if (value == _record) return;
                _record = value;
                this.RaisePropertyChanged(() => Records);
            }
        }

        private bool _isShieldAlarmsThatUserOcLightCause;

        public bool IsShieldAlarmsThatUserOcLightCause
        {
            get { return _isShieldAlarmsThatUserOcLightCause; }
            set
            {
                if (_isShieldAlarmsThatUserOcLightCause != value)
                {
                    _isShieldAlarmsThatUserOcLightCause = value;
                    this.RaisePropertyChanged(() => this.IsShieldAlarmsThatUserOcLightCause);
                }
            }
        }

        #region CmdSave
        private DateTime _dtSaveAll;

        public ICommand CmdSaveAll
        {
            get { return new RelayCommand(Ex, CanEx, true); }
        }

        private void Ex()
        {
            _dtSaveAll = DateTime.Now;
            var lst = new List<Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem>();


            foreach (var t in this.Records)
            {
                lst.Add(t.GetTmlFaultType());
            }
            var info = Wlst.Sr.ProtocolPhone.LxFault.wlst_user_define_fault_alarms;
            info.WstFaultUserDefineFaultAlarms.Op = 2;
            info.WstFaultUserDefineFaultAlarms.RequestOrSetUserName = _currentSetUser;
            foreach (var t in lst)
            {
                info.WstFaultUserDefineFaultAlarms.Items.Add(t);
            }
            info.WstFaultUserDefineFaultAlarms.IsShieldAlarmsThatUserOcLightCause = IsShieldAlarmsThatUserOcLightCause;

            bool allselected = true;
            foreach (var f in this.Items)
            {

                var tmplst = (from t in f.ChildTreeItems where t.IsSelected select t.NodeId).ToList();
                if (f.NodeId == 0) //special rtu
                {
                    foreach (var fx in tmplst)
                    {
                        info.WstFaultUserDefineFaultAlarms.ItemsAlarmGroup.Add(
                            new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmGroupItem()
                                {AreaId = f.AreaId, GroupId = fx});
                    }
                    if (tmplst.Count != f.ChildTreeItems.Count) allselected = false;
                }
                else
                {
                    if (tmplst.Count == f.ChildTreeItems.Count)
                    {
                        info.WstFaultUserDefineFaultAlarms.ItemsAlarmGroup.Add(
                            new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmGroupItem()
                                {AreaId = f.AreaId, GroupId = f.NodeId});
                    }
                    else
                    {
                        allselected = false;
                        foreach (var fx in tmplst)
                        {
                            info.WstFaultUserDefineFaultAlarms.ItemsAlarmGroup.Add(
                                new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmGroupItem()
                                    {AreaId = f.AreaId, GroupId = fx});
                        }
                    }
                }

            }
            if (allselected)
                info.WstFaultUserDefineFaultAlarms.ItemsAlarmGroup =
                    new List<Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmGroupItem>();

            SndOrderServer.OrderSnd(info, 10, 6);
            this.ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在更新用户:"+_currentSetUser +" 的报警设置...";
        }

        private bool CanEx()
        {
            if (Items.Count == 0) return false;
            if (Records.Count == 0) return false;
            return DateTime.Now.Ticks - _dtSaveAll.Ticks > 10000000;
        }
        #endregion

        #region CmdBselected 反选

        public ICommand CmdBselected
        {
            get { return new RelayCommand(ExCmdBselected, CanExCmdBselected, true); }
        }

        private void ExCmdBselected()
        {
            var flg = true;
            if (Items.Count > 0)
            {
                flg = Items[0].IsSelected;
            }

            foreach (var f in Items)
            {
                //if (f.NodeId == 0) continue;
                f.IsSelected = !flg;
                if (f.NodeId == 0)
                {
                    foreach (var t in f.ChildTreeItems)
                    {
                        t.IsSelected = !flg;
                    }
                }
            }
        }


        private bool CanExCmdBselected()
        {
            if (Items.Count > 0) return true;
            else return false;
        }
        #endregion

        private string _showMsg;

        public string ShowMsg
        {
            get { return _showMsg; }
            set
            {
                if (_showMsg != value)
                {
                    _showMsg = value;
                    this.RaisePropertyChanged(() => this.ShowMsg);
                }
            }
        }

        #region tab

        public int Index
        {
            get { return 1; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "用户报警设置"; }
        }

        #endregion
    }



    /// <summary>
    /// Action
    /// </summary>
    public partial class UserFaultSettingByAdminVm
    {

        private void InitAction()
        {

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxFault.wlst_user_define_fault_alarms,
                RequestOrUpdateUserFaultSet,
                typeof (UserFaultSettingByAdminVm), this);

            ProtocolServer.RegistProtocol(Wlst.Sr.ProtocolPhone.LxLogin.wst_request_user_info,
                                          OnRequestUser1, typeof (UserFaultSettingByAdminVm), this ,true );
        }


        private string _currentSetUser = "";

        private int Get_Real_Name_Index(string User)
        {
            int i = 0;

            foreach (var f in userRealName)
            {
                if(f.Substring(0, f.IndexOf(' ')) == User)
                {
                    return i;
                }
                i++;
            }

            return 0;
        }

        public void RequestOrUpdateUserFaultSet(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isViewShow == false) return;
            Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(RequestOrUpdateUserFaultSet1,infos );
        }

        public void RequestOrUpdateUserFaultSet1(object  infosss)
        {
            var infos = infosss as MsgWithMobile;
            var info = infos.WstFaultUserDefineFaultAlarms;
            if (info == null) return;
            if (info.Items == null || info.ItemsAlarmGroup == null) return;
            if (string.IsNullOrEmpty(info.RequestOrSetUserName)) return;

            OnUserDataBack(info);
            _currentSetUser = info.RequestOrSetUserName;

            string userName = userRealName[Get_Real_Name_Index(_currentSetUser)];

            ShowUserMsg = "当前正在设置用户:" + userName + "  的报警信息";
            foreach (var f in ItemsUser)
            {
                if (f.Name.Equals(userName))
                {
                    IsUserCanEnable = true;
                    if (f.Value > 10000000000 && f.Value < 110000000000)
                    {

                        ShowUserMsg = "正在设置用户:" + userName + "  的报警信息,电话号码:" + f.Value;
                    }
                    else
                    {
                        if (f.Value == 1)
                            ShowUserMsg = "正在设置用户:" + userName + "  的报警信息,电话号码 空，无法发短消息";

                        else
                            ShowUserMsg = "正在设置用户:" + userName + "  的报警信息,电话号码:" + f.Value +
                                          " ,电话号码存在错误，无法发短消息。";
                    }
                    break;
                }
            }


            if (info.Op == 2)
            {
                ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + userName + " 用户报警信息更新成功.";

            }
            else
            {
                ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + userName + " 用户报警信息请求成功.";
            }
        }

        private List<string> userRealName = new List<string>();


        //public void OnRequestUser(string session, Wlst.mobile.MsgWithMobile infos)
        //{
        //    if (_isViewShow == false) return;
        //    Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(OnRequestUser1, infos);
        //}

        public void OnRequestUser1(string session, Wlst.mobile.MsgWithMobile infos)
        {

            if (_isViewShow == false) return;
            //var infos = infosss as MsgWithMobile;
            var info = infos.WstLoginRequestUserInfo;
            if (info == null) return;
            if (!string.IsNullOrEmpty(info.TargetUserName)) return;
            if (_isloaduseralready) return;
            _isloaduseralready = true;

            ItemsUser.Clear();
            userRealName.Clear();
            //ItemsUser.Add(new UserInfoItem()
            //                  {
            //                      Name = "请选择用户",
            //                      Value = 0
            //                  });
            foreach (var t in info.UserInfo)
            {
                long phnum = 1;
                Int64.TryParse(t.PhoneNumber, out phnum);
                if (phnum < 1) phnum = 1;

                var tmp = new UserInfoItem()
                              {
                                  Areas = new List<int>(),
                                  Name = t.UserName + " - " + t.UserRealName,
                                  Value = phnum 
                              };


                foreach (var f in t.AreaR) if (tmp.Areas.Contains(f) == false) tmp.Areas.Add(f);
                foreach (var f in t.AreaW) if (tmp.Areas.Contains(f) == false) tmp.Areas.Add(f);
                foreach (var f in t.AreaX) if (tmp.Areas.Contains(f) == false) tmp.Areas.Add(f);

                if (t.UserName.Equals(Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName))
                {
                    foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                    {
                        if (tmp.Areas.Contains(f) == false) tmp.Areas.Add(f);
                    }
                }
                if (tmp.Areas.Count == 0 && Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2)
                {

                    tmp.Areas.Add(Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[0].AreaId);

                }

                ItemsUser.Add(tmp);
                userRealName.Add(tmp.Name);

            }

            //if (ItemsUser.Count > 1) CurrentSelectUser = ItemsUser[1];
            //else 
            

            var flg = false;
            foreach (var t in ItemsUser)
            {
                if (t.Name == UserInfo.UserLoginInfo.UserName)
                {
                    CurrentSelectUser = t;
                    flg = true;
                    continue;
                }
            }

            if (!flg && ItemsUser.Count > 0)
            {
                CurrentSelectUser = ItemsUser[0];
            }


            ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + " 用户信息请求成功。";

        }

        public void RequestUserName()
        {

            var info = Wlst.Sr.ProtocolPhone.LxLogin.wst_request_user_info;
            info.WstLoginRequestUserInfo.TargetUserName = string.Empty;
            SndOrderServer.OrderSnd(info, 10, 6);

        }


        public void RequestUserFaultSet(string userName)
        {

            var info = Wlst.Sr.ProtocolPhone.LxFault.wlst_user_define_fault_alarms;
            info.WstFaultUserDefineFaultAlarms.Op = 1; //.TargetUserName = string.Empty;
            info.WstFaultUserDefineFaultAlarms.RequestOrSetUserName = userName;
            SndOrderServer.OrderSnd(info, 10, 6);

        }
    }

    /// <summary>
    /// Attri
    /// </summary>
    public partial class UserFaultSettingByAdminVm
    {

        private bool _txtVissssi;

        /// <summary>
        /// 
        /// </summary>
        public bool IsUserCanEnable
        {
            get { return _txtVissssi; }
            set
            {
                if (value != _txtVissssi)
                {
                    _txtVissssi = value;
                    this.RaisePropertyChanged(() => this.IsUserCanEnable);
                }
            }
        }



        private string _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public string ShowUserMsg
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.ShowUserMsg);
                }
            }
        }


        private UserInfoItem _cur;

        public UserInfoItem CurrentSelectUser
        {
            get { return _cur; }
            set
            {
                if (value == _cur) return;
                _cur = value;
                this.RaisePropertyChanged(() => this.CurrentSelectUser);

                if (value != null && value.Value != 0)
                {
                    ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + "当前选中:" + value.Name + " ,正在请求该用户的报警信息";
                    RequestUserFaultSet(value.Name.Substring(0, value.Name.IndexOf(" ", System.StringComparison.Ordinal)));
                }
                else ShowMsg = "";
            }
        }


        private ObservableCollection<UserInfoItem> _fItemsArea;

        public ObservableCollection<UserInfoItem> ItemsUser
        {
            get
            {
                if (_fItemsArea == null)
                {
                    _fItemsArea = new ObservableCollection<UserInfoItem>();
                }
                return _fItemsArea;
            }
            set
            {
                if (value == _fItemsArea) return;
                _fItemsArea = value;
                this.RaisePropertyChanged(() => ItemsUser);
            }
        }

        private string _msg;

        /// <summary>
        /// 显示信息
        /// </summary>
        public string CurrentServerBackUserName
        {
            get { return _msg; }
            set
            {
                if (value != _msg)
                {
                    _msg = value;
                    this.RaisePropertyChanged(() => CurrentServerBackUserName);
                }
            }
        }


    }

    /// <summary>
    /// load tree
    /// </summary>
    public partial class UserFaultSettingByAdminVm
    {


        private bool _bselec;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public bool Bselected
        {
            get { return _bselec; }
            set
            {
                if (_bselec != value)
                {
                    _bselec = value;
                    this.RaisePropertyChanged(() => this.Bselected);

                    foreach (var f in Items)
                    {
                        if (f.NodeId == 0) continue;
                        f.IsSelected = !f.IsSelected;
                        f.IsSelected = !f.IsSelected;
                        f.IsSelected = !f.IsSelected;

                    }
                }
            }
        }

        public event EventHandler OnChanged;
        private bool _areaCount;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public bool AreaCount
        {
            get { return _areaCount; }
            set
            {
                if (_areaCount != value)
                {
                    _areaCount = value;
                    this.RaisePropertyChanged(() => this.AreaCount);
                    if (OnChanged != null) OnChanged(value ? 2 : 0, null);
                }
            }
        }

        private ObservableCollection<TreeNodeBaseVm> _items;

        public ObservableCollection<TreeNodeBaseVm> Items
        {
            get { return _items ?? (_items = new ObservableCollection<TreeNodeBaseVm>()); }
        }

        private void OnUserDataBack(client.UserSelfDefineFalutAlarm info)
        {
            UserInfoItem user = null;
            foreach (var f in ItemsUser)
            {
                if (f.Name.Contains(info.RequestOrSetUserName ))
                {
                    user = f;
                    break;
                }
            }
            if (user == null) return;

            LoadFaultType(info, user);
            this.LoadTreeNode(info, user);
        }

        private void LoadFaultType(client.UserSelfDefineFalutAlarm info, UserInfoItem user)
        {

            IsShieldAlarmsThatUserOcLightCause = info.IsShieldAlarmsThatUserOcLightCause;

            var noselected = true;
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary.Values)
            {
                if (t.IsEnable)
                {
                    noselected = false;
                    break;
                }
            }

            var tmpssss = new List<OneFaultTypeInfo>();
            foreach (var f in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                //存在故障选中 同时 本故障未设置为使用则跳过 否则显示
                if (!noselected && !f.Value.IsEnable)
                {
                    continue;
                }

                UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem ff = null;
                // Sr.EquipemntLightFault.Services.UserDisplayErrorSelfSetInfoHold.GetInfoById(f.Value.FaultId);



                bool someSelected = info.Items.Any(t => t.IsDisplay);

                if (someSelected == false || info.Items.Count == 0)
                {
                    ff = new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem()
                             {
                                 AlarmTimes = 1,
                                 FaultCode = f.Value.FaultId,
                                 IsDisplay = true
                             };
                }
                else
                {
                    foreach (var fg in info.Items)
                    {
                        if (fg.FaultCode == f.Value.FaultId)
                        {
                            ff = fg;
                            break;
                        }
                    }
                }


                if (ff != null)
                {
                    var ffff = new OneFaultTypeInfo(ff);
                    tmpssss.Add(ffff);
                }
                else
                {
                    var ffff =
                        new OneFaultTypeInfo(new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem()
                                                 {
                                                     AlarmTimes = 1,
                                                     FaultCode = f.Value.FaultId,
                                                     IsDisplay = false,
                                                 });
                    tmpssss.Add(ffff);
                }
            }
            var tmpggg = (from t in tmpssss orderby t.FaultCode select t).ToList();
            var ggsssg = new ObservableCollection<OneFaultTypeInfo>();
            foreach (var t in tmpggg) ggsssg.Add(t);
            Records = ggsssg;
        }

        //初始化时加载左侧树终端节点
        private void LoadTreeNode(client.UserSelfDefineFalutAlarm info, UserInfoItem user)
        {
            Items.Clear();

            var areas = user.Areas;

            var tmpArea = (from t in areas orderby t ascending select t).ToList();
            AreaCount = areas.Count > 1;
            List<int> areax = new List<int>();
            bool noselected = true;

            foreach (var f in tmpArea)
            {
                //服务器端选中的  设备列表
                var selecteRtu = (from t in info .ItemsAlarmGroup  where t.AreaId == f select t.GroupId).ToList();

                var grps = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(f);
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(f) == false)
                    continue;

                var areaTml = new List<int>();
                areaTml.AddRange(Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[f].LstTml);

                var name = f + "-" + Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[f].AreaName;

                if (selecteRtu.Count > 0) noselected = false;

                foreach (var g in grps)
                {
                    if (g.LstTml.Count == 0) continue;
                    var selsect = false;
                    foreach (
                        var fg in info .ItemsAlarmGroup  )
                    {
                        if (fg.AreaId == f && fg.GroupId == g.GroupId)
                        {
                            selsect = true;
                            noselected = false;
                            break;
                        }
                    }
                    if (areax.Contains(g.AreaId) == false) areax.Add(g.AreaId);
                    var tmp = GetNode(g.AreaId, name, g.GroupId, g.GroupId, g.GroupName, selsect, g.LstTml, ref areaTml,
                                      selecteRtu);
                    this.Items.Add(tmp);

                }
                if (areaTml.Count > 0)
                {
                    var trsfsf = new List<int>();
                    trsfsf.AddRange(areaTml);
                    var tmp = GetNode(f, name, 0, 0, "未分组设备", selecteRtu.Count > 0, trsfsf, ref areaTml, selecteRtu);

                    tmp.IsExpanded = false;

                    this.Items.Add(tmp);
                }

            }
            if (noselected)
            {
                foreach (var f in Items) f.IsSelected = true;
            }

            AreaCount = areas.Count > 1;

        }

        private TreeNodeBaseVm GetNode(int areaId, string areaName, int nodeid, int wulidi, string nodename,
                                       bool isSelected, List<int> tmls, ref List<int> areaRtu, List<int> selectedRtu)
        {
            var tmp = new TreeNodeBaseVm()
                          {
                              AreaId = areaId,
                              NodeId = nodeid,
                              WuLiId = wulidi + "",
                              NodeName = nodename,
                              IsSelected = isSelected,
                              IsExpanded = true,
                              BackGround = areaName,
                          };
            bool nodeselected = false;
            int scount = 0;
            foreach (var tml in tmls)
            {
                if (areaRtu.Contains(tml) == false) continue;
                areaRtu.Remove(tml);

                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(tml) ==
                    false)
                    continue;
                int phyId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tml].RtuPhyId;
                string namer =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tml].RtuName;
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tml].RtuFid > 0)
                {
                    int fid = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tml].RtuFid;
                    if (
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fid) ==
                        false)
                        continue;
                    phyId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[fid].RtuPhyId;
                    namer = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[fid].RtuName +
                            "-" + namer;
                }

                var sel = isSelected || selectedRtu.Contains(tml);

                if (sel)
                {
                    scount++;
                    nodeselected = true;
                }
                tmp.ChildTreeItems.Add(new TreeNodeBaseViewModel()
                                           {
                                               AreaId = areaId,
                                               NodeId = tml,
                                               WuLiId = phyId.ToString("d4") + "",
                                               NodeName = namer,
                                               IsSelected = sel,
                                               IsExpanded = true,
                                               BackGround = areaName,
                                           });
            }
            if (isSelected == false && nodeselected)
            {
                tmp.SetIsselectWithOutChiledSelected(true);
            }


            if (nodeid == 0)
            {
                scount = 0;
                foreach (var fx in tmp.ChildTreeItems)
                {
                    if (selectedRtu.Contains(fx.NodeId))
                    {
                        fx.IsSelected = true;
                        scount++;
                    }
                    else fx.IsSelected = false;
                    //fx.IsSelected = false;
                    //  fx.IsSelected = selectedRtu.Contains(fx.NodeId);

                }
            }
            if (scount == 0 || scount == tmp.ChildTreeItems.Count)
            {

            }
            else
            {
                tmp.WuLiId = wulidi + " - " + scount + "/" + tmp.ChildTreeItems.Count;
            }
            if (nodeid == 0) tmp.WuLiId = wulidi + " - " + scount + "/" + tmp.ChildTreeItems.Count;
            return tmp;
        }

    }
}
