using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.ViewModel;
using Wlst.Ux.WJ3005Module.LnEmergencyOperationCenter.Services;
using Wlst.client;

namespace Wlst.Ux.WJ3005Module.LnEmergencyOperationCenter.ViewModel
{
    [Export(typeof (IILnEmergencyOperationCenter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LnEmergencyOperationCenterViewModel : EventHandlerHelperExtendNotifyProperyChanged,
                                                               IILnEmergencyOperationCenter
    {
        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "应急中心"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion


        public LnEmergencyOperationCenterViewModel()
        {
            InitAction();
        }

        public void NavInitBeforShow(params object[] parsObjects)
        {

        }


        public void NavOnLoad(params object[] parsObjects)
        {
            //if (_isViewShow) return;
            //todo
            GetEmergencyGroups();
            LnErrValue = 5;
            LdErrValue = 5000;
            _currentSelectAllState = false; 
            AnsRemind = "";
            OperaterRemind = "";
            IsShowRunOne = Visibility.Visible;
            IsShowRunTwo= Visibility.Visible;
            _isViewShow = true;
            RequestEmergencyInfo();
            //Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(RequestEmergencyInfo, 1);


            var rtusLn = parsObjects[0] as ConcurrentDictionary<int, List<int>>;
            if (rtusLn == null || rtusLn.Count ==0) return;
            AddTreeNodeTempByList(rtusLn,"手动操作");
                ////火零不平衡 lvf 2018年6月13日09:13:46
                //IsLnErr = true;



        }

        private  void RequestEmergencyInfo()
        {
            var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;
            info.WstRtutimeTimeTableEmerg.Op = 4;
            SndOrderServer.OrderSnd(info, 10, 6);
        
        
        }


        public void OnUserHideOrClosing()
        {
            //  ZOrders.OpenCloseLight.OpenCloseLightDataDispatch.IsControlCenterManagDemo2TakeOverOcOrderShow = false;
            _isViewShow = false;

            TreeTmlNode.RegisterTmlNode.Clear();


            //throw new NotImplementedException();
        }


    }

    /// <summary>
    /// Attribute
    /// </summary>
    public partial class LnEmergencyOperationCenterViewModel
    {
        //界面是否开启
        private bool _isViewShow;

        #region  Group

        private Visibility _txtgrpVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility GrpVisi
        {
            get { return _txtgrpVisi; }
            set
            {
                if (value != _txtgrpVisi)
                {
                    _txtgrpVisi = value;
                    this.RaisePropertyChanged(() => this.GrpVisi);
                }
            }
        }

        private static ObservableCollection<GroupInt> _grpdevices;

        public static ObservableCollection<GroupInt> GroupName
        {
            get
            {
                if (_grpdevices == null)
                {
                    _grpdevices = new ObservableCollection<GroupInt>();
                }
                return _grpdevices;
            }

        }

        public class GroupInt : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private GroupInt _grpcomboboxselected;
        private int GrpId;

        public GroupInt GroupComboBoxSelected
        {
            get { return _grpcomboboxselected; }
            set
            {
                if (_grpcomboboxselected != value)
                {
                    _grpcomboboxselected = value;
                    this.RaisePropertyChanged(() => this.GroupComboBoxSelected);
                    if (value == null) return;
                    GrpId = value.Key;


                }
            }
        }

        private Dictionary<int, List<int>> NoEmRtus = new Dictionary<int, List<int>>();
        public void GetEmergencyGroups()
        {
            GroupName.Clear();

            //if (AreaId == -1) //全部区域
            //{
            //    GrpVisi = Visibility.Collapsed;

            //}
            //else
            //{
            //    GrpVisi = Visibility.Visible;
            //    var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
            //    if (area == null) return;
            //    var grps =
            //        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(AreaId);
            //    GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
            //    if (grps.Count > 0)
            //    {
            //        var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
            //        foreach (var f in grpsTmp)
            //        {

            //            GroupName.Add(new GroupInt() { Value = f.GroupName, Key = f.GroupId });
            //        }
            //    }
            //    GroupComboBoxSelected = GroupName[0];
            //}
            var grpInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.AreaInfo;
            if (grpInfo.Count == 0) return;
            //GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
            foreach (var g in grpInfo)
            {
                //屏蔽终端记录
                if (g.Key == 9999)
                {
                    foreach (var j in g.Value)
                    {
                         if (NoEmRtus.ContainsKey(j.Key)==false )
                         {
                             NoEmRtus.Add(j.Key,j.Value);
                         }
                         else
                         {
                             NoEmRtus[j.Key] = j.Value;
                         }
                    }
                   
                    continue;
                }
                string name = "应急" + g.Key + "级渍水路段";
                GroupName.Add(new GroupInt() {Value = name, Key = g.Key});
            }
            //GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
            //GroupName.Add(new GroupInt() { Value = "积水1", Key = 1 });
            //GroupName.Add(new GroupInt() { Value = "积水2", Key = 2 });
            //GroupName.Add(new GroupInt() { Value = "积水3", Key = 3 });
            //GroupName.Add(new GroupInt() { Value = "积水4", Key = 4 });

            GroupComboBoxSelected = GroupName[0];
        }

        //public   GetEmergencyRtusByGrpId(int grpId)
        //{

        //    //if (grpId == -1) //全部区域
        //    //{
        //    //    GrpVisi = Visibility.Collapsed;

        //    //}
        //    //else
        //    //{
        //    //    GrpVisi = Visibility.Visible;
        //    //    var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
        //    //    if (area == null) return;
        //    //    var grps =
        //    //        Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(AreaId);
        //    //    GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
        //    //    if (grps.Count > 0)
        //    //    {
        //    //        var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
        //    //        foreach (var f in grpsTmp)
        //    //        {

        //    //            GroupName.Add(new GroupInt() { Value = f.GroupName, Key = f.GroupId });
        //    //        }
        //    //    }
        //    //    GroupComboBoxSelected = GroupName[0];
        //    //}
        //    return  Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.GetEmeInfo(grpId);

        //}

        #endregion

        #region OperateItems

        private ObservableCollection<TreeNodeBase> _operateItems;

        /// <summary>
        /// 操作列表
        /// </summary>
        public ObservableCollection<TreeNodeBase> OperateItems
        {
            get { return _operateItems ?? (_operateItems = new ObservableCollection<TreeNodeBase>()); }
        }

        #endregion

        #region EmergencyItems

        private ObservableCollection<TreeNodeBase> _emergencyItems;

        /// <summary>
        /// 处于应急关灯状态列表
        /// </summary>
        public ObservableCollection<TreeNodeBase> EmergencyItems
        {
            get { return _emergencyItems ?? (_emergencyItems = new ObservableCollection<TreeNodeBase>()); }
        }

        #endregion


        #region LnErrVaule

        private int _lnErrValue;

        public int LnErrValue
        {
            get { return _lnErrValue; }
            set
            {
                if (value == _lnErrValue) return;
                _lnErrValue = value;
                this.RaisePropertyChanged(() => this.LnErrValue);
            }
        }

        #endregion

        #region LdErrVaule

        private int _ldErrValue;

        public int LdErrValue
        {
            get { return _ldErrValue; }
            set
            {
                if (value == _ldErrValue) return;
                _ldErrValue = value;
                this.RaisePropertyChanged(() => this.LdErrValue);
            }
        }

        #endregion

        #region OperaterRemind

        private string _remindop;

        public string OperaterRemind
        {
            get { return _remindop; }
            set
            {
                if (value == _remindop) return;
                _remindop = value;
                this.RaisePropertyChanged(() => this.OperaterRemind);
            }
        }

        #endregion

        #region AnsRemind

        private string _remindans;

        public string AnsRemind
        {
            get { return _remindans; }
            set
            {
                if (value == _remindans) return;
                _remindans = value;
                this.RaisePropertyChanged(() => this.AnsRemind);
            }
        }

        #endregion

        #region BtStopEmergency

        private string _btStopEmergency;
        //关闭智能模式 按钮文本
        public string BtStopEmergency
        {
            get { return _btStopEmergency; }
            set
            {
                if (value == _btStopEmergency) return;
                _btStopEmergency = value;
                this.RaisePropertyChanged(() => this.BtStopEmergency);
            }
        }

        #endregion

        #region IsShowRunOne

        private Visibility _isShowRunOne;
        //是否显示 开启1级按钮
        public Visibility IsShowRunOne
        {
            get { return _isShowRunOne; }
            set
            {
                if (value == _isShowRunOne) return;
                _isShowRunOne = value;
                this.RaisePropertyChanged(() => this.IsShowRunOne);
            }
        }

        #endregion

        #region IsShowRunTwo

        private Visibility _isShowRunTwo;
        //是否显示 开启2级按钮
        public Visibility IsShowRunTwo
        {
            get { return _isShowRunTwo; }
            set
            {
                if (value == _isShowRunTwo) return;
                _isShowRunTwo = value;
                this.RaisePropertyChanged(() => this.IsShowRunTwo);
            }
        }

        #endregion 

    }


    /// <summary>
    /// Methods
    /// </summary>
    public partial class LnEmergencyOperationCenterViewModel
    {
        #region CmdAddEmergencyRtus

        private DateTime _dtCmdAddEmergencyRtus;
        private ICommand _cmdAddEmergencyRtus;

        public ICommand CmdAddEmergencyRtus
        {
            get
            {
                if (_cmdAddEmergencyRtus == null)
                    _cmdAddEmergencyRtus = new RelayCommand(ExCmdAddEmergencyRtus, CanExCmdAddEmergencyRtus, false);
                return _cmdAddEmergencyRtus;
            }
        }

        private void ExCmdAddEmergencyRtus()
        {
            _dtCmdAddEmergencyRtus = DateTime.Now;
            try
            {
                var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.AreaEmeHold.MySlef.GetEmeInfo(GrpId);

                AddTreeNodeTempByList(rtulst, GroupComboBoxSelected.Value);


            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("添加终端出错:" + ex);
            }

        }

        private bool CanExCmdAddEmergencyRtus()
        {
            if (GroupComboBoxSelected == null) return false;
            return DateTime.Now.Ticks - _dtCmdAddEmergencyRtus.Ticks > 30000000;
            return false;
        }




        #endregion

        #region CmdAddLnRtus

        private DateTime _dtCmdAddLnRtus;
        private ICommand _cmdCmdAddLnRtus;

        /// <summary>
        /// 添加 火零不平衡终端
        /// </summary>
        public ICommand CmdAddLnRtus
        {
            get
            {
                if (_cmdCmdAddLnRtus == null)
                    _cmdCmdAddLnRtus = new RelayCommand(ExCmdAddLnRtus, CanExCmdAddLnRtus, false);
                return _cmdCmdAddLnRtus;
            }
        }

        private void ExCmdAddLnRtus()
        {
            _dtCmdAddLnRtus = DateTime.Now;
            try
            {
                var rtulst = GetLnErrRtus(LnErrValue);

                AddTreeNodeTempByList(rtulst,"火零不平衡>"+LnErrValue);


            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("添加终端出错:" + ex);
            }

        }

        private bool CanExCmdAddLnRtus()
        {
            return DateTime.Now.Ticks - _dtCmdAddLnRtus.Ticks > 30000000;
            return false;
        }

        private ConcurrentDictionary<int, List<int>> GetLnErrRtus(int errVaule)
        {
            var rtulst = new ConcurrentDictionary<int, List<int>>();
            var errlst =
                (from t in Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                 where t.FaultId == 25 && Math.Abs(t.AUpper) > errVaule
                 select t).ToList();
            if (errlst.Count == 0) return new ConcurrentDictionary<int, List<int>>();
            foreach (var t in errlst)
            {

                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;

                var tmps =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                        t.RtuId]
                    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmps.WjLoops.ContainsKey(t.LoopId) == false) continue;
                var switchId = tmps.WjLoops[t.LoopId].SwitchOutputId;
                if (rtulst.ContainsKey(t.RtuId) == false)
                {
                    var lst = new List<int>();
                    lst.Add(switchId);
                    rtulst.TryAdd(t.RtuId, lst);
                }
                else
                {
                    if (rtulst[t.RtuId].Contains(switchId) == false)
                    {
                        rtulst[t.RtuId].Add(switchId);
                    }
                }

            }
            return rtulst;
        }


        #endregion

        #region CmdAddLdRtus

        private DateTime _dtCmdAddLdRtus;
        private ICommand _cmdCmdAddLdRtus;

        /// <summary>
        /// 添加 火零不平衡终端
        /// </summary>
        public ICommand CmdAddLdRtus
        {
            get
            {
                if (_cmdCmdAddLdRtus == null)
                    _cmdCmdAddLdRtus = new RelayCommand(ExCmdAddLdRtus, CanExCmdAddLdRtus, false);
                return _cmdCmdAddLdRtus;
            }
        }

        private void ExCmdAddLdRtus()
        {
            _dtCmdAddLdRtus = DateTime.Now;
            try
            {
                var rtulst = GetLdErrRtus(LdErrValue);

                AddTreeNodeTempByList(rtulst, "漏电值>" + LnErrValue);


            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("添加终端出错:" + ex);
            }

        }

        private bool CanExCmdAddLdRtus()
        {
            return DateTime.Now.Ticks - _dtCmdAddLdRtus.Ticks > 30000000;
            return false;
        }


        private ConcurrentDictionary<int, List<int>> GetLdErrRtus(int errVaule)
        {
            var rtulst = new ConcurrentDictionary<int, List<int>>();
            var errlst =
                (from t in Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                 where t.FaultId == 45 && Math.Abs(t.V) > errVaule
                 select t).ToList();
            if (errlst.Count == 0) return new ConcurrentDictionary<int, List<int>>();
            foreach (var t in errlst)
            {

                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;

                var tmps =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                        t.RtuId];
                    //
                if (tmps == null) continue;
                var rtuid = tmps.RtuFid;
                if( Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuid)==false )
                    continue;
                var tmpss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid] as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmpss == null) continue;
                if (tmpss.WjLoops.ContainsKey(t.LoopId) == false) continue;
                var switchId = tmpss.WjLoops[t.LoopId].SwitchOutputId;
                if (rtulst.ContainsKey(t.RtuId) == false)
                {
                    var lst = new List<int>();
                    lst.Add(switchId);
                    rtulst.TryAdd(t.RtuId, lst);
                }
                else
                {
                    if (rtulst[t.RtuId].Contains(switchId) == false)
                    {
                        rtulst[t.RtuId].Add(switchId);
                    }
                }

            }
            return rtulst;
        }
        #endregion

        #region CmdSelectAllOp
        /// <summary>
        /// 全选状态 默认为false
        /// </summary>
        private bool _currentSelectAllState = false;
        private DateTime _dtCmdSelectAllOp;
        private ICommand _cmdSelectAllOp;

        /// <summary>
        /// 添加 火零不平衡终端
        /// </summary>
        public ICommand CmdSelectAllOp
        {
            get
            {
                if (_cmdSelectAllOp == null)
                    _cmdSelectAllOp = new RelayCommand(ExCmdSelectAllOp, CanExCmdSelectAllOp, false);
                return _cmdSelectAllOp;
            }
        }

        private void ExCmdSelectAllOp()
        {
            _dtCmdSelectAllOp = DateTime.Now;
            try
            {
                _currentSelectAllState = !_currentSelectAllState;
                foreach (var g in OperateItems)
                {
                    g.IsChecked = _currentSelectAllState;
                }

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }

        }

        private bool CanExCmdSelectAllOp()
        {
            return true;
            //return DateTime.Now.Ticks - _dtCmdSelectAllOp.Ticks > 30000000;
            return false;
        }



        #endregion

        #region CmdDelSelectionOp

        private DateTime _dtCmdDelSelectionOp;
        private ICommand _cmdDelSelectionOp;

        /// <summary>
        /// 删除选中终端
        /// </summary>
        public ICommand CmdDelSelectionOp
        {
            get
            {
                if (_cmdDelSelectionOp == null)
                    _cmdDelSelectionOp = new RelayCommand(ExCmdDelSelectionOp, CanExCmdDelSelectionOp, false);
                return _cmdDelSelectionOp;
            }
        }

        private void ExCmdDelSelectionOp()
        {
            _dtCmdDelSelectionOp = DateTime.Now;
            try
            {
                var selectLst = (from t in OperateItems where t.IsChecked select t).ToList();
                if ( selectLst.Count ==0)
                {

                    UMessageBox.Show("请选择终端", "请选择需要操作的终端......", UMessageBoxButton.Ok);
                    return;
                
                }

                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "是否删除选中设备？", WlstMessageBoxType.YesNo) ==
                    WlstMessageBoxResults.Yes)
                {
                        for ( int i = OperateItems.Count -1 ;i>=0;i--)
                        {
                            if ( OperateItems[i].IsChecked) OperateItems.Remove(OperateItems[i]);
                        
                        
                        }


                }


            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }

        }

        private bool CanExCmdDelSelectionOp()
        {
            return true;
           // return DateTime.Now.Ticks - _dtCmdDelSelectionOp.Ticks > 30000000;
            return false;
        }

        #endregion

        #region CmdCloseAgain

        private DateTime _dtCmdCloseAgain;
        private ICommand _cmdCloseAgain;

        /// <summary>
        /// 删除选中终端
        /// </summary>
        public ICommand CmdCloseAgain
        {
            get
            {
                if (_cmdCloseAgain == null)
                    _cmdCloseAgain = new RelayCommand(ExCmdCloseAgain, CanExCmdCloseAgain, false);
                return _cmdCloseAgain;
            }
        }

        private void ExCmdCloseAgain()
        {
            _dtCmdCloseAgain = DateTime.Now;
            try
            {


                var data = new Wlst.client.OpenCloseOperatorCenter
                               {
                                   Open = 2  //关灯
                               };

                int allcount = 0;






                var k1Rtus = GetNodeKxNoAnswer(1);
                var k2Rtus = GetNodeKxNoAnswer(2);
                var k3Rtus = GetNodeKxNoAnswer(3);
                var k4Rtus =GetNodeKxNoAnswer(4);
                var k5Rtus = GetNodeKxNoAnswer(5);
                var k6Rtus = GetNodeKxNoAnswer(6);
                var k7Rtus = GetNodeKxNoAnswer(7);
                var k8Rtus = GetNodeKxNoAnswer(8);
                if (k1Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 1, Rtus = k1Rtus});
                if (k2Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 2, Rtus = k2Rtus});
                if (k3Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 3, Rtus = k3Rtus});
                if (k4Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 4, Rtus = k4Rtus});
                if (k5Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 5, Rtus = k5Rtus});
                if (k6Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 6, Rtus = k6Rtus});
                if (k7Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 7, Rtus = k7Rtus});
                if (k8Rtus.Count > 0)
                    data.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() {LoopId = 8, Rtus = k8Rtus});

                allcount = k1Rtus.Count + k2Rtus.Count + k3Rtus.Count + k4Rtus.Count + k5Rtus.Count + k6Rtus.Count +
                           k7Rtus.Count + k8Rtus.Count;

                if (allcount == 0)
                {
                    AnsRemind = "所有操作都已经成功，无须执行补操作...";
                    return;
                }

                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                {
                    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                    if (sss == UMessageBoxWantPassWord.CancelReturn)
                    {
                        return;
                    }
                    if (sss != UserInfo.UserLoginInfo.UserPassword)
                    {
                        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                         UMessageBoxButton.Yes);
                        return;
                    }
                }
                else
                {
                    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行开关灯操作，\r\n若确定请输入验证码:1234", "");
                    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                    {
                        return;
                    }

                    if (sss != "1234")
                    {
                        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                        return;
                    }
                }



                var info = Sr.ProtocolPhone.LxRtu.wst_cnt_order_rtu_open_close_center;
                info.WstRtuCntOrderOpenCloseCenter = data;
                SndOrderServer.OrderSnd(info, 10, 6);

                AnsRemind =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在进行补关操作,共"+allcount+"条回路...";




            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }

        }

        private bool CanExCmdCloseAgain()
        {
            return DateTime.Now.Ticks - _dtCmdCloseAgain.Ticks > 30000000;
            return false;
        }



        public  List<int> GetNodeKxNoAnswer(int kx)
        {
            var rtn = new List<int>();
            foreach (var f in EmergencyItems)
            {

                    //if (l.IsRtuUsed == false) continue;
                    if (rtn.Contains(f.NodeId)) continue;

                    if (kx == 1)
                    {
                        if (f.K1SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(f.NodeId);
                    }
                    else if (kx == 2)
                    {
                        if (f.K2SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(f.NodeId);
                    }
                    else if (kx == 3)
                    {
                        if (f.K3SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(f.NodeId);
                    }
                    else if (kx == 4)
                    {
                        if (f.K4SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(f.NodeId);
                    }
                    else if (kx == 5)
                    {
                        if (f.K5SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(f.NodeId);
                    }
                    else if (kx == 6)
                    {
                        if (f.K6SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(f.NodeId);
                    }
                    else if (kx == 7)
                    {
                        if (f.K7SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(f.NodeId);
                    }
                    else if (kx == 8)
                    {
                        if (f.K8SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(f.NodeId);
                    }


                }
   
            return rtn;
        }
        #endregion

        




        #region CmdEmergencyClose

        private DateTime _dtCmdEmergencyClose1;
        private DateTime _dtCmdEmergencyClose2;
        private DateTime _dtCmdEmergencyClose3;
        private ICommand _cmdEmergencyClose;


        /// <summary>
        /// 删除选中终端
        /// </summary>
        public ICommand CmdEmergencyClose
        {
            get
            {
                if (_cmdEmergencyClose == null)
                    _cmdEmergencyClose = new RelayCommand<string>(ExCmdEmergencyClose, CanExCmdEmergencyClose, false);
                return _cmdEmergencyClose;
            }
        }

        private void ExCmdEmergencyClose(string datax)
        {
            //1、应急关灯
            //2、恢复开灯
            //3、恢复周设置 不开灯
            int x = 0;
            if (Int32.TryParse(datax, out x) == false) return;
            if (x == 1)
            {
                #region

                _dtCmdEmergencyClose1 = DateTime.Now;
                try
                {
                    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                    {
                        var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                        if (sss == UMessageBoxWantPassWord.CancelReturn)
                        {
                            return;
                        }
                        if (sss != UserInfo.UserLoginInfo.UserPassword)
                        {
                            UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                             UMessageBoxButton.Yes);
                            return;
                        }
                    }
                    else
                    {
                        var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行应急关灯操作，\r\n若确定请输入验证码:1234", "");
                        if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                        {
                            return;
                        }

                        if (sss != "1234")
                        {
                            UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                            return;
                        }
                    }

                    //todo
                    var data = new Wlst.client.TimeTableEmergenceOper
                                   {
                                       Op = 1
                                   };
                    var rtulst = new List<TimeTableEmergenceOper.RtuList>(); //new List<Tuple<int, int>>();
                   
                    
                    foreach (var g in OperateItems)
                    {
                        if (g.IsChecked == false) continue;
                        var loops = new List<int>();
                        if (g.IsSwitch1Checked)
                        {
                            loops.Add(1);
                        }
                        if (g.IsSwitch2Checked)
                        {
                            loops.Add(2);
                        }
                        if (g.IsSwitch3Checked)
                        {
                            loops.Add(3);
                        }
                        if (g.IsSwitch4Checked)
                        {
                            loops.Add(4);
                        }
                        if (g.IsSwitch5Checked)
                        {
                            loops.Add(5);
                        }
                        if (g.IsSwitch6Checked)
                        {
                            loops.Add(6);
                        }
                        if (g.IsSwitch7Checked)
                        {
                            loops.Add(7);
                        }
                        if (g.IsSwitch8Checked)
                        {
                            loops.Add(8);
                        }
                        //var noEmLoops = new List<int>();

                        //var emloopid = "";

                        ////记录终端  屏蔽应急关灯回路
                        //if (NoEmRtus.ContainsKey(g.NodeId)) noEmLoops = NoEmRtus[g.NodeId];

                        //if( Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g.NodeId)== false )
                        //    continue;
                        //var rtuInfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g.NodeId];
                        //if (rtuInfo == null) continue;
                        //if (g.IsChecked == false) continue;
                        //if (g.IsSwitch1Checked)
                        //{
                        //    if (noEmLoops.Contains(1))
                        //    {
                        //        emloopid += "1,";
                        //    }
                        //    loops.Add(1);
                        //}
                        //if (g.IsSwitch2Checked)
                        //{
                        //    if (noEmLoops.Contains(2))
                        //    {
                        //        emloopid += "2,";
                        //    }
                        //    loops.Add(2);
                        //}
                        //if (g.IsSwitch3Checked)
                        //{
                        //    if (noEmLoops.Contains(3))
                        //    {
                        //        emloopid += "3,";
                        //    }
                        //    loops.Add(3);
                        //}
                        //if (g.IsSwitch4Checked)
                        //{
                        //    if (noEmLoops.Contains(4))
                        //    {
                        //        emloopid += "4,";
                        //    }
                        //    loops.Add(4);
                        //}
                        //if (g.IsSwitch5Checked)
                        //{
                        //    if (noEmLoops.Contains(5))
                        //    {
                        //        emloopid += "5,";
                        //    }
                        //    loops.Add(5);
                        //}
                        //if (g.IsSwitch6Checked)
                        //{
                        //    if (noEmLoops.Contains(6))
                        //    {
                        //        emloopid += "6,";
                        //    }
                        //    loops.Add(6);
                        //}
                        //if (g.IsSwitch7Checked)
                        //{
                        //    if (noEmLoops.Contains(7))
                        //    {
                        //        emloopid += "7,";
                        //    }
                        //    loops.Add(7);
                        //}
                        //if (g.IsSwitch8Checked)
                        //{
                        //    if (noEmLoops.Contains(8))
                        //    {
                        //        emloopid += "8,";
                        //    }
                        //    loops.Add(8);
                        //}
                        //if (emloopid != "")
                        //{
                        //    if (
                        //        Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        //            rtuInfo.RtuPhyId + "-" + rtuInfo.RtuName + ": 开关量" + emloopid + "是特殊开关量,是否需要应急关灯",
                        //            WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                        //    {
                        //        //如果点否  自动去除特殊回路  lvf 2018年9月19日14:59:14 武汉需求
                        //        foreach (var jk in emloopid)
                        //        {
                        //            if (loops.Contains(jk)) loops.Remove(jk); 
                        //        }

                        //    }
                        //}

                        rtulst.Add(new TimeTableEmergenceOper.RtuList()
                                       {
                                           RtuId = g.NodeId,
                                           LoopId = loops,
                                           Remark = g.Remarks,
                                       });
                    }

                    if (rtulst.Count == 0)
                    {
                        WlstMessageBox.Show("提醒", "请勾选终端...", WlstMessageBoxType.Ok);

                        return;
                    }
                    //todo
                    data.RtuInfoItems.AddRange(rtulst);


                    var shieldTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3601, 3, 24); //生效时间 默认为24小时
                    var suninfo =
                        Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month,
                                                                                                   DateTime.Now.Day);

                    int sunriseHour = Convert.ToInt16(suninfo.time_sunrise/60);
                    int sunriseMin = Convert.ToInt16(suninfo.time_sunrise%60);
                    var sunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunriseHour,
                                                   sunriseMin, 0);
                    //判断点击时间 是否大于今天日出时间
                    if (DateTime.Now.CompareTo(sunriseTime) < 0)
                    {
                        var dtyesterday = DateTime.Now.AddDays(-1);
                        var dtstart = new DateTime(dtyesterday.Year, dtyesterday.Month, dtyesterday.Day, 12, 0, 1);
                        data.DtStartTime = dtstart.Ticks;
                        data.DtEndTime = dtstart.AddHours(shieldTime).Ticks;

                    }
                    else
                    {

                        var dts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 1);
                        data.DtStartTime = dts.Ticks;
                        data.DtEndTime = dts.AddHours(shieldTime).Ticks;
                    }

                    var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;
                    info.WstRtutimeTimeTableEmerg = data;
                    SndOrderServer.OrderSnd(info, 10, 6);

                }
                catch (Exception ex)
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
                }

                #endregion


                OperaterRemind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在应急关灯...";
                return;
            }
            if ( x==2|| x==3 )
            {
                var tr = "";
                #region
                if(x ==2)
                {
                    _dtCmdEmergencyClose2 = DateTime.Now;
                    tr = "取消应急关灯(仅发周设置，不开灯)";
                }else
                {
                    _dtCmdEmergencyClose3 = DateTime.Now;
                    tr = "恢复开灯(恢复周设置，并开灯)";
                }
                
                try
                {
                    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                    {
                        var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                        if (sss == UMessageBoxWantPassWord.CancelReturn)
                        {
                            return;
                        }
                        if (sss != UserInfo.UserLoginInfo.UserPassword)
                        {
                            UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                             UMessageBoxButton.Yes);
                            return;
                        }
                    }
                    else
                    {
                        var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行"+tr+"\r\n若确定请输入验证码:1234", "");
                        if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                        {
                            return;
                        }

                        if (sss != "1234")
                        {
                            UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                            return;
                        }
                    }


                    //todo
                    var data = new Wlst.client.TimeTableEmergenceOper
                    {
                        Op = x 
                    };
                    var rtulst = new List<TimeTableEmergenceOper.RtuList>(); //new List<Tuple<int, int>>();
                    foreach (var g in EmergencyItems)
                    {
                        var loops = new List<int>(){1,2,3,4,5,6,7,8};
                        if (g.IsChecked == false) continue;
                        //if (g.IsSwitch1Checked) loops.Add(1);
                        //if (g.IsSwitch2Checked) loops.Add(2);
                        //if (g.IsSwitch3Checked) loops.Add(3);
                        //if (g.IsSwitch4Checked) loops.Add(4);
                        //if (g.IsSwitch5Checked) loops.Add(5);
                        //if (g.IsSwitch6Checked) loops.Add(6);
                        //if (g.IsSwitch7Checked) loops.Add(7);
                        //if (g.IsSwitch8Checked) loops.Add(8);
                        rtulst.Add(new TimeTableEmergenceOper.RtuList()
                        {
                            RtuId = g.NodeId,
                            LoopId = loops,
                            Remark = g.Remarks,
                        });
                    }
                    if (rtulst.Count ==0)
                    {
                        WlstMessageBox.Show("提醒", "请勾选终端...", WlstMessageBoxType.Ok);
                        
                        return;
                    }
                    //todo
                    data.RtuInfoItems.AddRange(rtulst);
                    var shieldTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3601, 3, 24); //生效时间 默认为24小时
                    var suninfo =
                        Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month,
                                                                                                   DateTime.Now.Day);

                    int sunriseHour = Convert.ToInt16(suninfo.time_sunrise / 60);
                    int sunriseMin = Convert.ToInt16(suninfo.time_sunrise % 60);
                    var sunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunriseHour,
                                                   sunriseMin, 0);
                    //判断点击时间 是否大于今天日出时间
                    if (DateTime.Now.CompareTo(sunriseTime) < 0)
                    {
                        var dtyesterday = DateTime.Now.AddDays(-1);
                        var dtstart = new DateTime(dtyesterday.Year, dtyesterday.Month, dtyesterday.Day, 12, 0, 1);
                        data.DtStartTime = dtstart.Ticks;
                        data.DtEndTime = dtstart.AddHours(shieldTime).Ticks;

                    }
                    else
                    {

                        var dts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 1);
                        data.DtStartTime = dts.Ticks;
                        data.DtEndTime = dts.AddHours(shieldTime).Ticks;
                    }

                    var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;
                    info.WstRtutimeTimeTableEmerg = data;
                    SndOrderServer.OrderSnd(info, 10, 6);

                }
                catch (Exception ex)
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
                }

                #endregion
                if(x==3)
                {
                    AnsRemind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在恢复开灯...";
                }
                else
                {
                    AnsRemind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在取消应急关灯...";
                }
                
            }
        }


        private bool CanExCmdEmergencyClose(string datax)
        {
            //1、应急关灯
            //2、恢复开灯
            //3、恢复周设置 不开灯

            int x = 0;
            if (Int32.TryParse(datax, out x) == false) return false;
            if (x == 1) return DateTime.Now.Ticks - _dtCmdEmergencyClose1.Ticks > 30000000;
            if (x == 2) return DateTime.Now.Ticks - _dtCmdEmergencyClose2.Ticks > 30000000;
            if (x == 3) return DateTime.Now.Ticks - _dtCmdEmergencyClose3.Ticks > 30000000;
            return false;
        }

        #endregion

        #region CmdStopEmergency

        private DateTime _dtCmdStopEmergency;
        private ICommand _cmdStopEmergency;

        /// <summary>
        /// 关闭一级方案
        /// </summary>
        public ICommand CmdStopEmergency
        {
            get
            {
                if (_cmdStopEmergency == null)
                    _cmdStopEmergency = new RelayCommand(ExCmdStopEmergency, CanCmdStopEmergency, false);
                return _cmdStopEmergency;
            }
        }

        private void ExCmdStopEmergency()
        {

            #region
            _dtCmdStopEmergency = DateTime.Now;
            try
            {
                if (
                    Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                        "您将要关闭智能模式，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    return;
                }
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                {
                    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                    if (sss == UMessageBoxWantPassWord.CancelReturn)
                    {
                        return;
                    }
                    if (sss != UserInfo.UserLoginInfo.UserPassword)
                    {
                        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                         UMessageBoxButton.Yes);
                        return;
                    }
                }
                else
                {
                    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行应急关灯操作，\r\n若确定请输入验证码:1234", "");
                    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                    {
                        return;
                    }

                    if (sss != "1234")
                    {
                        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                        return;
                    }
                }

                var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;
                info.WstRtutimeTimeTableEmerg.Op = 6;
                info.WstRtutimeTimeTableEmerg.Moniter = 0;
                SndOrderServer.OrderSnd(info, 10, 6);

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }
            #endregion
        }

        private bool CanCmdStopEmergency()
        {
            //return true;
            return DateTime.Now.Ticks - _dtCmdStopEmergency.Ticks > 30000000;
            return false;
        }

        #endregion

        #region CmdRunEmergencyLevelOne

        private DateTime _dtCmdRunEmergencyLevelOne;
        private ICommand _cmdRunEmergencyLevelOne;

        /// <summary>
        /// 关闭一级方案
        /// </summary>
        public ICommand CmdRunEmergencyLevelOne
        {
            get
            {
                if (_cmdRunEmergencyLevelOne == null)
                    _cmdRunEmergencyLevelOne = new RelayCommand(ExCmdRunEmergencyLevelOne, CanCmdRunEmergencyLevelOne, false);
                return _cmdRunEmergencyLevelOne;
            }
        }

        private void ExCmdRunEmergencyLevelOne()
        {

            #region
            _dtCmdRunEmergencyLevelOne = DateTime.Now;
            try
            {
                if (
                                  Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                      "您将要开启一级智能模式，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    return;
                }
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                {
                    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                    if (sss == UMessageBoxWantPassWord.CancelReturn)
                    {
                        return;
                    }
                    if (sss != UserInfo.UserLoginInfo.UserPassword)
                    {
                        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                         UMessageBoxButton.Yes);
                        return;
                    }
                }
                else
                {
                    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行应急关灯操作，\r\n若确定请输入验证码:1234", "");
                    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                    {
                        return;
                    }

                    if (sss != "1234")
                    {
                        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                        return;
                    }
                }



                var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;


                var shieldTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3601, 3, 24); //生效时间 默认为24小时
                var suninfo =
                    Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month,
                                                                                               DateTime.Now.Day);

                int sunriseHour = Convert.ToInt16(suninfo.time_sunrise / 60);
                int sunriseMin = Convert.ToInt16(suninfo.time_sunrise % 60);
                var sunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunriseHour,
                                               sunriseMin, 0);
                //判断点击时间 是否大于今天日出时间
                if (DateTime.Now.CompareTo(sunriseTime) < 0)
                {
                    var dtyesterday = DateTime.Now.AddDays(-1);
                    var dtstart = new DateTime(dtyesterday.Year, dtyesterday.Month, dtyesterday.Day, 12, 0, 1);
                    info.WstRtutimeTimeTableEmerg.DtStartTime = dtstart.Ticks;
                    info.WstRtutimeTimeTableEmerg.DtEndTime = dtstart.AddHours(shieldTime).Ticks;

                }
                else
                {

                    var dts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 1);
                    info.WstRtutimeTimeTableEmerg.DtStartTime = dts.Ticks;
                    info.WstRtutimeTimeTableEmerg.DtEndTime = dts.AddHours(shieldTime).Ticks;
                }




                info.WstRtutimeTimeTableEmerg.Op = 6;
                info.WstRtutimeTimeTableEmerg.Moniter = 1;
                SndOrderServer.OrderSnd(info, 10, 6);

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }
            #endregion
        }

        private bool CanCmdRunEmergencyLevelOne()
        {
            return DateTime.Now.Ticks - _dtCmdRunEmergencyLevelOne.Ticks > 30000000;
            return false;
        }

        #endregion

        #region CmdRunEmergencyLevelTwo

        private DateTime _dtCmdRunEmergencyLevelTwo;
        private ICommand _cmdRunEmergencyLevelTwo;

        /// <summary>
        /// 关闭一级方案
        /// </summary>
        public ICommand CmdRunEmergencyLevelTwo
        {
            get
            {
                if (_cmdRunEmergencyLevelTwo == null)
                    _cmdRunEmergencyLevelTwo = new RelayCommand(ExCmdRunEmergencyLevelTwo, CanCmdRunEmergencyLevelTwo, false);
                return _cmdRunEmergencyLevelTwo;
            }
        }

        private void ExCmdRunEmergencyLevelTwo()
        {

            #region
            _dtCmdRunEmergencyLevelTwo = DateTime.Now;
            try
            {
                if (
                                  Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                      "您将要开启二级智能模式，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    return;
                }
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                {
                    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                    if (sss == UMessageBoxWantPassWord.CancelReturn)
                    {
                        return;
                    }
                    if (sss != UserInfo.UserLoginInfo.UserPassword)
                    {
                        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                         UMessageBoxButton.Yes);
                        return;
                    }
                }
                else
                {
                    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行应急关灯操作，\r\n若确定请输入验证码:1234", "");
                    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                    {
                        return;
                    }

                    if (sss != "1234")
                    {
                        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                        return;
                    }
                }

                var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;

                var shieldTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3601, 3, 24); //生效时间 默认为24小时
                var suninfo =
                    Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month,
                                                                                               DateTime.Now.Day);

                int sunriseHour = Convert.ToInt16(suninfo.time_sunrise / 60);
                int sunriseMin = Convert.ToInt16(suninfo.time_sunrise % 60);
                var sunriseTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, sunriseHour,
                                               sunriseMin, 0);
                //判断点击时间 是否大于今天日出时间
                if (DateTime.Now.CompareTo(sunriseTime) < 0)
                {
                    var dtyesterday = DateTime.Now.AddDays(-1);
                    var dtstart = new DateTime(dtyesterday.Year, dtyesterday.Month, dtyesterday.Day, 12, 0, 1);
                    info.WstRtutimeTimeTableEmerg.DtStartTime = dtstart.Ticks;
                    info.WstRtutimeTimeTableEmerg.DtEndTime = dtstart.AddHours(shieldTime).Ticks;

                }
                else
                {

                    var dts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 1);
                    info.WstRtutimeTimeTableEmerg.DtStartTime = dts.Ticks;
                    info.WstRtutimeTimeTableEmerg.DtEndTime = dts.AddHours(shieldTime).Ticks;
                }


                info.WstRtutimeTimeTableEmerg.Op = 6;
                info.WstRtutimeTimeTableEmerg.Moniter = 2;
                SndOrderServer.OrderSnd(info, 10, 6);

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }
            #endregion
        }

        private bool CanCmdRunEmergencyLevelTwo()
        {
            return DateTime.Now.Ticks - _dtCmdRunEmergencyLevelTwo.Ticks > 30000000;
            return false;
        }

        #endregion

        #region CmdStopEmergencyLevelTwo

        private DateTime _dtCmdStopEmergencyLevelTwo;
        private ICommand _cmdStopEmergencyLevelTwo;

        /// <summary>
        /// 关闭一级方案
        /// </summary>
        public ICommand CmdStopEmergencyLevelTwo
        {
            get
            {
                if (_cmdStopEmergencyLevelTwo == null)
                    _cmdStopEmergencyLevelTwo = new RelayCommand(ExCmdStopEmergencyLevelTwo, CanCmdStopEmergencyLevelTwo, false);
                return _cmdStopEmergencyLevelTwo;
            }
        }

        private void ExCmdStopEmergencyLevelTwo()
        {

            #region
            _dtCmdStopEmergencyLevelTwo = DateTime.Now;
            try
            {
                if (
                                  Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                      "您将要开启二级智能模式，是否继续？", WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                {
                    return;
                }
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm == 2)
                {
                    var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
                    if (sss == UMessageBoxWantPassWord.CancelReturn)
                    {
                        return;
                    }
                    if (sss != UserInfo.UserLoginInfo.UserPassword)
                    {
                        UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                         UMessageBoxButton.Yes);
                        return;
                    }
                }
                else
                {
                    var sss = UMessageBoxWantSomefromUser.Show("上海五零盛同信息科技有限公司", "您将要进行应急关灯操作，\r\n若确定请输入验证码:1234", "");
                    if (sss == UMessageBoxWantSomefromUser.CancelReturn)
                    {
                        return;
                    }

                    if (sss != "1234")
                    {
                        UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
                        return;
                    }
                }

                var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg;
                info.WstRtutimeTimeTableEmerg.Op = 6;
                info.WstRtutimeTimeTableEmerg.Moniter = 0;
                SndOrderServer.OrderSnd(info, 10, 6);

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }
            #endregion
        }

        private bool CanCmdStopEmergencyLevelTwo()
        {
            return DateTime.Now.Ticks - _dtCmdStopEmergencyLevelTwo.Ticks > 30000000;
            return false;
        }

        #endregion

        #region CmdExportOp

        private DateTime _dtCmdExportOp;
        private ICommand _cmdExportOp;

        /// <summary>
        /// 删除选中终端
        /// </summary>
        public ICommand CmdExportOp
        {
            get
            {
                if (_cmdExportOp == null)
                    _cmdExportOp = new RelayCommand(ExCmdExportOp, CanCmdExportOp, false);
                return _cmdExportOp;
            }
        }

        private void ExCmdExportOp()
        {
            _dtCmdExportOp = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("区域名称");
                lsttitle.Add("地址");
                lsttitle.Add("终端名称");
                lsttitle.Add("终端状态");
                lsttitle.Add("K1");
                lsttitle.Add("K2");
                lsttitle.Add("K3");
                lsttitle.Add("K4");
                lsttitle.Add("K5");
                lsttitle.Add("K6");
                lsttitle.Add("K7"); 
                lsttitle.Add("K8");
                lsttitle.Add("来源");
                //if (IsOldFaultQuery) 
                lsttitle.Add("应急关灯操作表");
                

                var lstobj = new List<List<object>>();
                var index = 1;
                foreach (var g in OperateItems)
                {
                    var tmp = new List<object>();
                    tmp.Add(index);
                    tmp.Add(g.AreaName);
                    tmp.Add(g.PhysicalId);
                    tmp.Add(g.NodeName);
                    tmp.Add(g.State);
                    tmp.Add(g.IsSwitch1Checked?"√":"--");
                    tmp.Add(g.IsSwitch2Checked ? "√" : "--");
                    tmp.Add(g.IsSwitch3Checked ? "√" : "--");
                    tmp.Add(g.IsSwitch4Checked ? "√" : "--");
                    tmp.Add(g.IsSwitch5Checked ? "√" : "--");
                    tmp.Add(g.IsSwitch6Checked ? "√" : "--");
                    tmp.Add(g.IsSwitch7Checked ? "√" : "--");
                    tmp.Add(g.IsSwitch8Checked ? "√" : "--");
                    tmp.Add(g.Remarks);


                    lstobj.Add(tmp);
                    index++;
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }

        }

        private bool CanCmdExportOp()
        {
            return DateTime.Now.Ticks - _dtCmdExportOp.Ticks > 30000000;
            return false;
        }

        #endregion

        #region CmdExportEmergency

        private DateTime _dtCmdExportEmergency;
        private ICommand _cmdCmdExportEmergency;

        /// <summary>
        /// 删除选中终端
        /// </summary>
        public ICommand CmdExportEmergency
        {
            get
            {
                if (_cmdCmdExportEmergency == null)
                    _cmdCmdExportEmergency = new RelayCommand(ExCmdExportEmergency, CanCmdExportEmergency, false);
                return _cmdCmdExportEmergency;
            }
        }

        private void ExCmdExportEmergency()
        {
            _dtCmdExportEmergency = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("地址");
                lsttitle.Add("终端名称");
                //lsttitle.Add("终端状态");
                lsttitle.Add("终端分组");
                lsttitle.Add("时间");
                lsttitle.Add("周设置");
                lsttitle.Add("K1");
                lsttitle.Add("K2");
                lsttitle.Add("K3");
                lsttitle.Add("K4");
                lsttitle.Add("K5");
                lsttitle.Add("K6");
                lsttitle.Add("K7");
                lsttitle.Add("K8");
                lsttitle.Add("来源");
                //if (IsOldFaultQuery) 


                var lstobj = new List<List<object>>();
                var index = 1;
                foreach (var g in EmergencyItems)
                {
                    var tmp = new List<object>();
                    tmp.Add(index);
                    tmp.Add(g.PhysicalId);
                    tmp.Add(g.NodeName);
                    var groupInfo = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(g.NodeId);
                    if (groupInfo != null)
                    {
                        var infosss =
                            Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.
                                GetGroupInfomation(
                                    groupInfo.Item1, groupInfo.Item2);
                        if (infosss != null)
                            tmp.Add(infosss.GroupName); // +" - " + infosss.GroupId;

                        //  if (infosss != null) DtRtuMsg += infosss.GroupName + " - ";

                    }
                    else
                    {
                        tmp.Add( "特殊终端");
                    }
                    tmp.Add(g.OpTime);
                    tmp.Add(g.WeekSndAns);
                    tmp.Add(g.K1SelectionTestAns == EnumSelectionTestAns.NoNeed ?"--": (g.K1SelectionTestAns ==EnumSelectionTestAns.Reply?"√":"未应答"));
                    tmp.Add(g.K2SelectionTestAns == EnumSelectionTestAns.NoNeed ? "--" : (g.K2SelectionTestAns == EnumSelectionTestAns.Reply ? "√" : "未应答"));
                    tmp.Add(g.K3SelectionTestAns == EnumSelectionTestAns.NoNeed ? "--" : (g.K3SelectionTestAns == EnumSelectionTestAns.Reply ? "√" : "未应答"));
                    tmp.Add(g.K4SelectionTestAns == EnumSelectionTestAns.NoNeed ? "--" : (g.K4SelectionTestAns == EnumSelectionTestAns.Reply ? "√" : "未应答"));
                    tmp.Add(g.K5SelectionTestAns == EnumSelectionTestAns.NoNeed ? "--" : (g.K5SelectionTestAns == EnumSelectionTestAns.Reply ? "√" : "未应答"));
                    tmp.Add(g.K6SelectionTestAns == EnumSelectionTestAns.NoNeed ? "--" : (g.K6SelectionTestAns == EnumSelectionTestAns.Reply ? "√" : "未应答"));
                    tmp.Add(g.K7SelectionTestAns == EnumSelectionTestAns.NoNeed ? "--" : (g.K7SelectionTestAns == EnumSelectionTestAns.Reply ? "√" : "未应答"));
                    tmp.Add(g.K8SelectionTestAns == EnumSelectionTestAns.NoNeed ? "--" : (g.K8SelectionTestAns == EnumSelectionTestAns.Reply ? "√" : "未应答"));
                    tmp.Add(g.Remarks);


                    lstobj.Add(tmp);
                    index++;
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }

        }

        private bool CanCmdExportEmergency()
        {
            return DateTime.Now.Ticks - _dtCmdExportEmergency.Ticks > 30000000;
            return false;
        }

        #endregion


        #region CmdSelectEmergencyRtus
        /// <summary>
        /// 全选状态 默认为false
        /// </summary>

        //private DateTime _dtCmdSelectAllOp;
        private ICommand _cmdSelectEmergencyRtus;

        /// <summary>
        /// 添加 火零不平衡终端
        /// </summary>
        public ICommand CmdSelectEmergencyRtus
        {
            get
            {
                if (_cmdSelectEmergencyRtus == null)
                    _cmdSelectEmergencyRtus = new RelayCommand(ExCmdSelectEmergencyRtus, CanExCmdSelectEmergencyRtus, false);
                return _cmdSelectEmergencyRtus;
            }
        }

        private void ExCmdSelectEmergencyRtus()
        {
            //_dtCmdSelectAllOp = DateTime.Now;
            try
            {
                _currentSelectAllState = !_currentSelectAllState;
                foreach (var g in EmergencyItems)
                {
                    g.IsChecked = !g.IsChecked;
                }

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("出错:" + ex);
            }

        }

        private bool CanExCmdSelectEmergencyRtus()
        {
            return true;
            //return DateTime.Now.Ticks - _dtCmdSelectAllOp.Ticks > 30000000;
            return false;
        }



        #endregion


        #region CmdShowNewData

        private DateTime _dtCmdShowNewData;
        private ICommand _cmdShowNewData;

        /// <summary>
        /// 最新数据
        /// </summary>
        public ICommand CmdShowNewData
        {
            get
            {
                if (_cmdShowNewData == null)
                    _cmdShowNewData = new RelayCommand(ExCmdShowNewData, CanCmdShowNewData, false);
                return _cmdShowNewData;
            }
        }

        private void ExCmdShowNewData()
        {
            _dtCmdShowNewData = DateTime.Now;
            RegionManage.ShowViewByIdAttachRegionWithArgu(Wlst.Ux.WJ3005Module.Services.ViewIdAssign.NavToEmergencyOperationNewDataViewId, EmergencyItems);

        }

        private bool CanCmdShowNewData()
        {
            return DateTime.Now.Ticks - _dtCmdShowNewData.Ticks > 30000000;
            return false;
        }

        #endregion

        

        /// <summary>
        /// 添加终端节点
        /// </summary>
        /// <param name="rtusLn"> int rtuid，list-loops</param>
        private void AddTreeNodeTempByList(ConcurrentDictionary<int, List<int>> rtusLn,string remarks = "")
        {


            if (rtusLn.Count == 0) return;

            int index = OperateItems.Count+1;
            var alreadyHave = new List<int>();
            //var needAdd = new List<int>();
            foreach (var g in OperateItems)
            {
                if (alreadyHave.Contains(g.NodeId) == false) alreadyHave.Add(g.NodeId);
            }
            var needAdd = (from t in rtusLn where alreadyHave.Contains(t.Key) == false orderby t.Key select t.Key).ToList();
            
            foreach (var g in needAdd)
            {
                this.OperateItems.Add(new TreeTmlNode(g,remarks,index));
                index++;
            }

            //自动打勾   lvf 2018年7月13日11:29:50
            foreach (var k in OperateItems)
            {



                k.IsChecked = true;
                if (!rtusLn.ContainsKey(k.NodeId)) continue;
                var switchoutIDs = rtusLn[k.NodeId];


                var noEmLoops = new List<int>();
                var emloopid = "";
                //记录终端  屏蔽应急关灯回路
                if (NoEmRtus.ContainsKey(k.NodeId)) noEmLoops = NoEmRtus[k.NodeId];

                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(k.NodeId) == false)
                    continue;
                var rtuInfo = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[k.NodeId];
                if (rtuInfo == null) continue;

                for (int i = switchoutIDs.Count - 1; i >= 0; i--)
                {
                    if (noEmLoops.Contains(switchoutIDs[i]))
                    {
                        if (
                            Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View.WlstMessageBox.Show(
                                rtuInfo.RtuPhyId + "-" + rtuInfo.RtuName + ": 开关量" + switchoutIDs[i] + "是特殊开关量,是否需要应急关灯",
                                WlstMessageBoxType.YesNo) == WlstMessageBoxResults.No)
                        {
                            //如果点否  自动去除特殊回路  lvf 2018年9月19日14:59:14 武汉需求
                            switchoutIDs.Remove(switchoutIDs[i]);

                        }

                    }
                }
                if (switchoutIDs.Count ==0) return;

                if (switchoutIDs.Contains(1)) k.IsSwitch1Checked = true;
                if (switchoutIDs.Contains(2)) k.IsSwitch2Checked = true;
                if (switchoutIDs.Contains(3)) k.IsSwitch3Checked = true;
                if (switchoutIDs.Contains(4)) k.IsSwitch4Checked = true;
                if (switchoutIDs.Contains(5)) k.IsSwitch5Checked = true;
                if (switchoutIDs.Contains(6)) k.IsSwitch6Checked = true;
                if (switchoutIDs.Contains(7)) k.IsSwitch7Checked = true;
                if (switchoutIDs.Contains(8)) k.IsSwitch8Checked = true;




            }


        }

    }





    /// <summary>
    /// Events
    /// </summary>
    public partial class LnEmergencyOperationCenterViewModel
    {

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtuTime.wst_rtutime_time_table_emerg,
                                          ResponseRtusInEmergency, typeof (LnEmergencyOperationCenterViewModel), this,true);

        }




        private void ResponseRtusInEmergency(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isViewShow == false) return;
            var datax = infos.WstRtutimeTimeTableEmerg;
            if (datax == null) return;
            // 1.应急关灯   2.取消应急关灯  3:取消应急关灯 并开灯  4.查询当前所有   5.服务器推送
            if (datax.Op == 1 )
            {
                #region

                //记录已存在的终端
                var alreadyHave = new List<int>();
                foreach (var g in EmergencyItems)
                {
                    if (alreadyHave.Contains(g.NodeId) == false) alreadyHave.Add(g.NodeId);
                }

                var rtuName = "";
                int phyid = 0;
                foreach (var g in datax.RtuInfoItems)
                {
                    var loops = g.LoopId;
                    //var sucLst = g.LoopsOcSucc;
                    //var noAnslst = (from t in loops where sucLst.Contains(t) == false select t).ToList();
                    //列表中存在则更新
                    if (alreadyHave.Contains(g.RtuId))
                    {
                        foreach (var k in EmergencyItems)
                        {
                            if (k.NodeId == g.RtuId)
                            {
                                k.Remarks = g.Remark;
                                k.WeekSndAns = g.WeeksetSucc ? "成功" : "等待";
                                k.OpTime = new DateTime(g.DateUpdate).ToString("yyyy-MM-dd HH:mm:ss");

                                k.K1SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K2SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K3SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K4SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K5SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K6SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K7SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K8SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                if (loops.Contains(1)) k.K1SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (loops.Contains(2)) k.K2SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (loops.Contains(3)) k.K3SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (loops.Contains(4)) k.K4SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (loops.Contains(5)) k.K5SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (loops.Contains(6)) k.K6SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (loops.Contains(7)) k.K7SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (loops.Contains(8)) k.K8SelectionTestAns = EnumSelectionTestAns.Ready;

                                break;
                            }

                        }
                    }
                    else //将列表中没有的终端 添加到列表中
                    {
                        int index = EmergencyItems.Count + 1;
                        var infox = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g.RtuId);

                        if (infox != null)
                        {
                            ////IsRtuUsed = infox.RtuStateCode == 2;
                            //if (infox.RtuModel == EnumRtuModel.Wj3006)
                            //{
                            //    Is3006 = true;
                            //}
                            phyid = infox.RtuPhyId;
                            rtuName = infox.RtuName;

                        }
                        this.EmergencyItems.Add(new TreeTmlNode()
                                                    {
                                                        Index = index,
                                                        NodeId = g.RtuId,
                                                        NodeName = rtuName,
                                                        PhysicalId = phyid,
                                                        Remarks = g.Remark,
                                                        K1SelectionTestAns =
                                                            loops.Contains(1)
                                                                ? EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K2SelectionTestAns =
                                                            loops.Contains(2)
                                                                ? EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K3SelectionTestAns =
                                                            loops.Contains(3)
                                                                ? EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K4SelectionTestAns =
                                                            loops.Contains(4)
                                                                ? EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K5SelectionTestAns =
                                                            loops.Contains(5)
                                                                ? EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K6SelectionTestAns =
                                                            loops.Contains(6)
                                                                ? EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K7SelectionTestAns =
                                                            loops.Contains(7)
                                                                ? EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K8SelectionTestAns =
                                                            loops.Contains(8)
                                                                ? EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        WeekSndAns = g.WeeksetSucc ? "成功" : "等待",
                                                        OpTime =
                                                            new DateTime(g.DateUpdate).ToString("yyyy-MM-dd HH:mm:ss"),
                                                    });
                        alreadyHave.Add(g.RtuId);

                    }
                }
                OperaterRemind = "";
                AnsRemind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  操作成功,共计" + EmergencyItems.Count + "个处于应急关灯的终端..";
                #endregion
            }
            else if (datax.Op == 5)  //应急关灯推送
            {
                #region

                //记录已存在的终端
                var alreadyHave = new List<int>();
                foreach (var g in EmergencyItems)
                {
                    if (alreadyHave.Contains(g.NodeId) == false) alreadyHave.Add(g.NodeId);
                }

                var rtuName = "";
                int phyid = 0;
                foreach (var g in datax.RtuInfoItems)
                {
                    var loops = g.LoopId;
                    var sucLst = g.MeasureSucc; //g.LoopsOcSucc;
                    var noAnslst = (from t in loops where sucLst.Contains(t) == false select t).ToList();
                    //列表中存在则更新
                    if (alreadyHave.Contains(g.RtuId))
                    {
                        foreach (var k in EmergencyItems)
                        {
                            if (k.NodeId == g.RtuId)
                            {
                                k.Remarks = g.Remark;
                                k.WeekSndAns = g.WeeksetSucc ? "成功" : "等待";
                                k.OpTime = new DateTime(g.DateUpdate).ToString("yyyy-MM-dd HH:mm:ss");

                                k.K1SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K2SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K3SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K4SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K5SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K6SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K7SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                k.K8SelectionTestAns = EnumSelectionTestAns.NoNeed;
                                if (sucLst.Contains(1)) k.K1SelectionTestAns = EnumSelectionTestAns.Reply;
                                if (sucLst.Contains(2)) k.K2SelectionTestAns = EnumSelectionTestAns.Reply;
                                if (sucLst.Contains(3)) k.K3SelectionTestAns = EnumSelectionTestAns.Reply;
                                if (sucLst.Contains(4)) k.K4SelectionTestAns = EnumSelectionTestAns.Reply;
                                if (sucLst.Contains(5)) k.K5SelectionTestAns = EnumSelectionTestAns.Reply;
                                if (sucLst.Contains(6)) k.K6SelectionTestAns = EnumSelectionTestAns.Reply;
                                if (sucLst.Contains(7)) k.K7SelectionTestAns = EnumSelectionTestAns.Reply;
                                if (sucLst.Contains(8)) k.K8SelectionTestAns = EnumSelectionTestAns.Reply;

                                if (noAnslst.Contains(1)) k.K1SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (noAnslst.Contains(2)) k.K2SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (noAnslst.Contains(3)) k.K3SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (noAnslst.Contains(4)) k.K4SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (noAnslst.Contains(5)) k.K5SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (noAnslst.Contains(6)) k.K6SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (noAnslst.Contains(7)) k.K7SelectionTestAns = EnumSelectionTestAns.Ready;
                                if (noAnslst.Contains(8)) k.K8SelectionTestAns = EnumSelectionTestAns.Ready;
                                break;
                            }

                        }
                    }
                    else //将列表中没有的终端 添加到列表中
                    {
                        int index = EmergencyItems.Count + 1;
                        var infox = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g.RtuId);

                        if (infox != null)
                        {
                            ////IsRtuUsed = infox.RtuStateCode == 2;
                            //if (infox.RtuModel == EnumRtuModel.Wj3006)
                            //{
                            //    Is3006 = true;
                            //}
                            phyid = infox.RtuPhyId;
                            rtuName = infox.RtuName;

                        }
                        this.EmergencyItems.Add(new TreeTmlNode()
                                                    {
                                                        Index = index,
                                                        NodeId = g.RtuId,
                                                        NodeName = rtuName,
                                                        PhysicalId = phyid,
                                                        Remarks = g.Remark,
                                                        K1SelectionTestAns =
                                                            loops.Contains(1)
                                                                ? sucLst.Contains(1)
                                                                      ? EnumSelectionTestAns.Reply
                                                                      : EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K2SelectionTestAns =
                                                            loops.Contains(2)
                                                                ? sucLst.Contains(2)
                                                                      ? EnumSelectionTestAns.Reply
                                                                      : EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K3SelectionTestAns =
                                                            loops.Contains(3)
                                                                ? sucLst.Contains(3)
                                                                      ? EnumSelectionTestAns.Reply
                                                                      : EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K4SelectionTestAns =
                                                            loops.Contains(4)
                                                                ? sucLst.Contains(4)
                                                                      ? EnumSelectionTestAns.Reply
                                                                      : EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K5SelectionTestAns =
                                                            loops.Contains(5)
                                                                ? sucLst.Contains(5)
                                                                      ? EnumSelectionTestAns.Reply
                                                                      : EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K6SelectionTestAns =
                                                            loops.Contains(6)
                                                                ? sucLst.Contains(6)
                                                                      ? EnumSelectionTestAns.Reply
                                                                      : EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K7SelectionTestAns =
                                                            loops.Contains(7)
                                                                ? sucLst.Contains(7)
                                                                      ? EnumSelectionTestAns.Reply
                                                                      : EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        K8SelectionTestAns =
                                                            loops.Contains(8)
                                                                ? sucLst.Contains(8)
                                                                      ? EnumSelectionTestAns.Reply
                                                                      : EnumSelectionTestAns.Ready
                                                                : EnumSelectionTestAns.NoNeed,
                                                        WeekSndAns = g.WeeksetSucc ? "成功" : "等待",
                                                        OpTime =
                                                            new DateTime(g.DateUpdate).ToString("yyyy-MM-dd HH:mm:ss"),
                                                    });
                        alreadyHave.Add(g.RtuId);

                    }

                }
                AnsRemind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  操作成功,共计" + EmergencyItems.Count + "个处于应急关灯的终端.";
                #endregion

            
            }
            else if (datax.Op == 2 || datax.Op == 3)//2.取消应急关灯  3:取消应急关灯 并开灯   应答
            {
                if (datax.RtuInfoItems == null ||datax.RtuInfoItems.Count== 0) return;
                foreach (var g in datax.RtuInfoItems)
                {
                    //取消应急的回路
                    var loops = g.LoopId;
                    for (int i = EmergencyItems.Count - 1; i >= 0; i--)
                    {

                        if (EmergencyItems[i].NodeId == g.RtuId)
                        {
                            EmergencyItems.RemoveAt(i);
                            //bool needDel = false;
                            ////根据事件 更改回路状态
                            //if (loops.Contains(1)) OperateItems[i].K1SelectionTestAns = EnumSelectionTestAns.NoNeed;
                            //if (loops.Contains(2)) OperateItems[i].K2SelectionTestAns = EnumSelectionTestAns.NoNeed;
                            //if (loops.Contains(3)) OperateItems[i].K3SelectionTestAns = EnumSelectionTestAns.NoNeed;
                            //if (loops.Contains(4)) OperateItems[i].K4SelectionTestAns = EnumSelectionTestAns.NoNeed;
                            //if (loops.Contains(5)) OperateItems[i].K5SelectionTestAns = EnumSelectionTestAns.NoNeed;
                            //if (loops.Contains(6)) OperateItems[i].K6SelectionTestAns = EnumSelectionTestAns.NoNeed;
                            //if (loops.Contains(7)) OperateItems[i].K7SelectionTestAns = EnumSelectionTestAns.NoNeed;
                            //if (loops.Contains(8)) OperateItems[i].K8SelectionTestAns = EnumSelectionTestAns.NoNeed;

                            //if (OperateItems[i].K1SelectionTestAns != EnumSelectionTestAns.NoNeed) needDel = true;
                            //if (OperateItems[i].K2SelectionTestAns != EnumSelectionTestAns.NoNeed) needDel = true;
                            //if (OperateItems[i].K3SelectionTestAns != EnumSelectionTestAns.NoNeed) needDel = true;
                            //if (OperateItems[i].K4SelectionTestAns != EnumSelectionTestAns.NoNeed) needDel = true;
                            //if (OperateItems[i].K5SelectionTestAns != EnumSelectionTestAns.NoNeed) needDel = true;
                            //if (OperateItems[i].K6SelectionTestAns != EnumSelectionTestAns.NoNeed) needDel = true;
                            //if (OperateItems[i].K7SelectionTestAns != EnumSelectionTestAns.NoNeed) needDel = true;
                            //if (OperateItems[i].K8SelectionTestAns != EnumSelectionTestAns.NoNeed) needDel = true;

                            //if (needDel) OperateItems.Remove(OperateItems[i]);
                        }

                    }

                }
                AnsRemind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  取消应急成功,还剩余" + EmergencyItems.Count + "个处于应急关灯的终端.";
            }
            else if (datax.Op == 4) //全部刷新
            {
                if (datax.Moniter == 1) //处于一级智能
                {
                    BtStopEmergency = "关闭一级智能模式";
                    IsShowRunOne = Visibility.Collapsed;
                    IsShowRunTwo = Visibility.Visible;
                }
                else if (datax.Moniter == 2)//处于二级智能模式
                {
                    BtStopEmergency = "关闭二级智能模式";
                    IsShowRunOne = Visibility.Collapsed;
                    IsShowRunTwo = Visibility.Collapsed;
                }
                else if (datax.Moniter == 0)//解除智能模式
                {
                    BtStopEmergency = "关闭智能模式";
                    IsShowRunOne = Visibility.Visible;
                    IsShowRunTwo = Visibility.Visible;
                }
                EmergencyItems.Clear();
                var info = (from t in datax.RtuInfoItems orderby t.RtuId select t).ToList();
                foreach (var g in info)
                {
                    var sucLst = g.MeasureSucc;//g.LoopsOcSucc;
                    var loops = g.LoopId;
                    var noAnslst = (from t in loops where sucLst.Contains(t) == false select t).ToList();
                    var ttn = new TreeTmlNode();

                    //初始化

                    ttn.K1SelectionTestAns = EnumSelectionTestAns.NoNeed;
                    ttn.K2SelectionTestAns = EnumSelectionTestAns.NoNeed;
                    ttn.K3SelectionTestAns = EnumSelectionTestAns.NoNeed;
                    ttn.K4SelectionTestAns = EnumSelectionTestAns.NoNeed;
                    ttn.K5SelectionTestAns = EnumSelectionTestAns.NoNeed;
                    ttn.K6SelectionTestAns = EnumSelectionTestAns.NoNeed;
                    ttn.K7SelectionTestAns = EnumSelectionTestAns.NoNeed;
                    ttn.K8SelectionTestAns = EnumSelectionTestAns.NoNeed;


                    //未应答
                    if (noAnslst.Contains(1)) ttn.K1SelectionTestAns = EnumSelectionTestAns.Ready;
                    if (noAnslst.Contains(2)) ttn.K2SelectionTestAns = EnumSelectionTestAns.Ready;
                    if (noAnslst.Contains(3)) ttn.K3SelectionTestAns = EnumSelectionTestAns.Ready;
                    if (noAnslst.Contains(4)) ttn.K4SelectionTestAns = EnumSelectionTestAns.Ready;
                    if (noAnslst.Contains(5)) ttn.K5SelectionTestAns = EnumSelectionTestAns.Ready;
                    if (noAnslst.Contains(6)) ttn.K6SelectionTestAns = EnumSelectionTestAns.Ready;
                    if (noAnslst.Contains(7)) ttn.K7SelectionTestAns = EnumSelectionTestAns.Ready;
                    if (noAnslst.Contains(8)) ttn.K8SelectionTestAns = EnumSelectionTestAns.Ready;
                    //已经应答
                    if (sucLst.Contains(1)) ttn.K1SelectionTestAns = EnumSelectionTestAns.Reply;
                    if (sucLst.Contains(2)) ttn.K2SelectionTestAns = EnumSelectionTestAns.Reply;
                    if (sucLst.Contains(3)) ttn.K3SelectionTestAns = EnumSelectionTestAns.Reply;
                    if (sucLst.Contains(4)) ttn.K4SelectionTestAns = EnumSelectionTestAns.Reply;
                    if (sucLst.Contains(5)) ttn.K5SelectionTestAns = EnumSelectionTestAns.Reply;
                    if (sucLst.Contains(6)) ttn.K6SelectionTestAns = EnumSelectionTestAns.Reply;
                    if (sucLst.Contains(7)) ttn.K7SelectionTestAns = EnumSelectionTestAns.Reply;
                    if (sucLst.Contains(8)) ttn.K8SelectionTestAns = EnumSelectionTestAns.Reply;

                    var rtuName = "";
                    int phyid = 0;
                    var infox = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g.RtuId);

                    if (infox != null)
                    {
                        ////IsRtuUsed = infox.RtuStateCode == 2;
                        //if (infox.RtuModel == EnumRtuModel.Wj3006)
                        //{
                        //    Is3006 = true;
                        //}
                        phyid = infox.RtuPhyId;
                        rtuName = infox.RtuName;

                    }

                    ttn.NodeId = g.RtuId;
                    ttn.Remarks = g.Remark;
                    ttn.NodeName = rtuName;
                    ttn.PhysicalId = phyid;
                    ttn.OpTime = new DateTime(g.DateUpdate).ToString("yyyy-MM-dd HH:mm:ss");
                    ttn.WeekSndAns = g.WeeksetSucc ? "成功" : "等待";
                    ttn.Index = EmergencyItems.Count + 1;
                    EmergencyItems.Add(ttn);
                }
                AnsRemind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  共计" + EmergencyItems.Count + "个处于应急关灯的终端..";

            }
            else if (datax.Op == 6) //开启 关闭 智能监控
            {
                if (datax.Moniter == 1) //处于一级智能
                {
                    BtStopEmergency = "关闭一级智能模式";
                    IsShowRunOne=Visibility.Collapsed;
                    IsShowRunTwo = Visibility.Visible;
                }
                else if (datax.Moniter == 2)//处于二级智能模式
                {
                    BtStopEmergency = "关闭二级智能模式";
                    IsShowRunOne=Visibility.Collapsed;
                    IsShowRunTwo=Visibility.Collapsed;
                }
                else if (datax.Moniter == 0)//解除智能模式
                {
                    BtStopEmergency = "关闭智能模式";
                    IsShowRunOne = Visibility.Visible;
                    IsShowRunTwo = Visibility.Visible;
                }


            }
            else if(datax.Op ==7)//恢复开灯  推送
            {
                var a = 7;
                IsShowRunTwo = Visibility.Visible;
                // todo,目前当op==2,3时,就直接清除表格了,后续操作应答不做处理.

            }


        }

       
    }
}
