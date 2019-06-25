using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Ux.EquipemntLightFault.Models;
using Wlst.Ux.EquipemntLightFault.UserIndividuationFaultSettingViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.EquipemntLightFault.UserIndividuationFaultSettingViewModel.ViewModel
{
    [Export(typeof(IIUserIndividuationFaultSettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class UserIndividuationFaultSettingViewModel :Wlst .Cr .Core .CoreServices .ObservableObject ,
        IIUserIndividuationFaultSettingViewModel
    {
        public UserIndividuationFaultSettingViewModel()
        {
            this.InitAction();
        }

        private bool _isViewShow = false;
        public void NavOnLoad(params object[] parsObjects)
        {
            _isViewShow = true;
            Records.Clear();

            IsShieldAlarmsThatUserOcLightCause = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetIsShieldAlarmsThatUserOcLightCause;
            var noselected = true;
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary.Values)
            {
                if (t.IsEnable)
                {
                    noselected = false;
                    break;
                }
            }

            var tmpssss = new List<UserIndividuationFaultViewModel>();
            foreach (var f in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                //存在故障选中 同时 本故障未设置为使用则跳过 否则显示
                if (!noselected && !f.Value.IsEnable)
                {
                    continue;
                }

                var ff = Sr.EquipemntLightFault.Services.UserDisplayErrorSelfSetInfoHold.GetInfoById(f.Value.FaultId);
                if (ff != null)
                {
                    var ffff = new UserIndividuationFaultViewModel(ff);
                    tmpssss.Add(ffff);
                }
                else
                {
                    var ffff = new UserIndividuationFaultViewModel(new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem() 
                                                                       {
                                                                           AlarmTimes = 3,
                                                                           FaultCode = f.Value.FaultId,
                                                                           IsDisplay  = false,
                                                                       });
                    tmpssss.Add(ffff);
                }
            }
            var tmpggg = (from t in tmpssss orderby t.FaultCode select t).ToList();
            var ggsssg = new ObservableCollection<UserIndividuationFaultViewModel>();
            foreach (var t in tmpggg) ggsssg.Add(t);
            Records = ggsssg;
            this.LoadTreeNodeGlobal();
        }

       public  void OnUserHideOrClosing()
       {
           _isViewShow = false;
           this .Records .Clear();
           this.Items.Clear();
       }

        private ObservableCollection<UserIndividuationFaultViewModel> _record;

        public ObservableCollection<UserIndividuationFaultViewModel> Records
        {
            get
            {
                if (_record == null)
                    _record = new ObservableCollection<UserIndividuationFaultViewModel>();
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
            foreach (var t in lst)
            {
                info.WstFaultUserDefineFaultAlarms.Items.Add(t);
            }
            info.WstFaultUserDefineFaultAlarms.IsShieldAlarmsThatUserOcLightCause = IsShieldAlarmsThatUserOcLightCause;

            bool allselected = true;
            foreach (var f in this .Items )
            {
                if(f.IsSelected )
                {
                    info.WstFaultUserDefineFaultAlarms.ItemsAlarmGroup.Add(
                        new Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmGroupItem()
                            {AreaId = f.AreaId, GroupId = f.NodeId});

                }
                else
                {
                    allselected = false;
                }
            }
            if (allselected)
                info.WstFaultUserDefineFaultAlarms.ItemsAlarmGroup =
                    new List<Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmGroupItem>(); 

            SndOrderServer.OrderSnd(info, 10, 6);
            this.ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在更新 ...";
        }

        private bool CanEx()
        {
            return DateTime.Now.Ticks - _dtSaveAll.Ticks > 10000000;
        }

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
            get { return "自定义故障报警"; }
        }

        #endregion
    }

    /// <summary>
    /// Action
    /// </summary>
    public partial class UserIndividuationFaultSettingViewModel
    {

        private void InitAction()
        {

              ProtocolServer.RegistProtocol(
                  Wlst.Sr.ProtocolPhone .LxFault  .wlst_user_define_fault_alarms  ,
                  requestOrUpdateUserIndividuationFault,
                  typeof(UserIndividuationFaultSettingViewModel), this);
        }
     

 
        public void requestOrUpdateUserIndividuationFault(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isViewShow == false) return;
            var tmlInfoExchangeforServer = infos.WstFaultUserDefineFaultAlarms;
            IsShieldAlarmsThatUserOcLightCause = infos.WstFaultUserDefineFaultAlarms.IsShieldAlarmsThatUserOcLightCause;
            if (tmlInfoExchangeforServer == null) return;
            if (tmlInfoExchangeforServer.Items == null) return;


            var tmps = new Dictionary<int, Wlst.client.UserSelfDefineFalutAlarm.UserselfDefineFalutAlarmItem>();
            foreach (var t in tmlInfoExchangeforServer.Items)
                if (!tmps.ContainsKey(t.FaultCode)) tmps.Add(t.FaultCode, t);

            foreach (var t in this.Records)
            {
                if (tmps.ContainsKey(t.FaultCode))
                {
                    t.AlarmTimes = tmps[t.FaultCode].AlarmTimes;
                    t.IsAlarm = tmps[t.FaultCode].IsDisplay;

                }
                else
                {
                    if (tmps.Count == 0)
                    {
                        t.AlarmTimes = 3;
                        t.IsAlarm = true;
                    }
                    else
                    {
                        t.AlarmTimes = 3;
                        t.IsAlarm = false;
                    }

                }
                t.IsEnable = true;
            }
            if (tmlInfoExchangeforServer.Op == 2)
                ShowMsg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  信息更新.";
        }





    }

    /// <summary>
    /// Attri
    /// </summary>
    public partial class UserIndividuationFaultSettingViewModel
    {
      

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


    public partial class UserIndividuationFaultSettingViewModel
    {
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
                }
            }
        }
        private ObservableCollection<TreeNodeBaseViewModel> _items;
        public ObservableCollection<TreeNodeBaseViewModel> Items
        {
            get { return _items ?? (_items = new ObservableCollection<TreeNodeBaseViewModel>()); }
        }

        //初始化时加载左侧树终端节点
        private void LoadTreeNodeGlobal()
        {
            Items.Clear();
           
            var areas = new List<int>();
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
          
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    if (areas.Contains(f) == false) areas.Add(f);
                }
            }


            
            AreaCount = areas.Count > 1;
            List<int> areax = new List<int>();
            bool noselected = true;
            foreach (var f in areas)
            {
                var grps = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(f);
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(f) == false)
                    continue;
                var name = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[f].AreaName + "-" + f;


                foreach (var g in grps)
                {
                    if (g.LstTml.Count == 0) continue;
                    var selsect = false;
                    foreach (var fg in Wlst.Sr.EquipemntLightFault.Services.UserDisplayErrorSelfSetInfoHold.MySelf.InfoAlarmGroups)
                    {
                        if(fg .AreaId ==f && fg.GroupId ==g.GroupId  )
                        {
                            selsect = true;
                            break;
                        }
                    }
                    if (areax.Contains(g.AreaId) == false) areax.Add(g.AreaId);
                    this.Items.Add(new TreeNodeBaseViewModel()
                                       {AreaId = g.AreaId, NodeId = g.GroupId, WuLiId =name , NodeName = g.GroupName,IsSelected =selsect });
                    if (selsect) noselected = false;
                }
                //this.Items.Add(new TreeGroupNode(f, 0));
            }
            if(noselected )
            {
                foreach (var f in Items) f.IsSelected = true;
            }
            AreaCount = areax.Count > 1;



        }

    }
}
