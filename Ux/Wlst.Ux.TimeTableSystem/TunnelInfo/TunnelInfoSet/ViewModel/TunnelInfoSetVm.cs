using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.Services;
using Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.Views;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.ViewModel
{
    [Export(typeof(IITunnelInfoSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TunnelInfoSetVm : EventHandlerHelperExtendNotifyProperyChanged,IITunnelInfoSet
    {
        public TunnelInfoSetVm()
        {
            this.AddEventFilterInfo(EventIdAssign.TunnelInfoSetUpdateId, PublishEventType.Core, true);
            this.AddEventFilterInfo(EventIdAssign.TunnelInfoSetRequestId, PublishEventType.Core, true);

        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (args.EventId == EventIdAssign.TunnelInfoSetRequestId)
            {
                LoadTunnelScheme(AreaId);
            }
            //base.ExPublishedEvent(args);
            if (args.EventId == EventIdAssign.TunnelInfoSetUpdateId)
            {
                if (DateTime.Now.Ticks - _dtCmdSaveTunnelInfo.Ticks < 600000000)
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "更新成功.");
                else
                {
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "数据更新.");

                }
            }
        }

        //获取区域名和ID
        public void GetAreaRId()
        {
            AreaName.Clear();
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    string area = t.Value.AreaName;

                    var areainfo = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(t.Value.AreaId);
                    if (areainfo== null) continue;
                    if ( areainfo.Count==0) continue;
                    bool haveRtu = false;
                    foreach (var g in areainfo)
                    {
                        if (g > 1000000 && g < 1100000)
                        {
                            haveRtu = true;
                            break;
                        }
                    }
                    if ( haveRtu == false) continue; 

                    AreaName.Add(new AreaInt() { Value = t.Value.AreaId.ToString("d2") + "-" + area, Key = t.Value.AreaId });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {

                        var areainfo = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(t);
                        if (areainfo == null) continue;
                        if (areainfo.Count == 0) continue;
                        bool haveRtu = false;
                        foreach (var g in areainfo)
                        {
                            if (g > 1000000 && g < 1100000)
                            {
                                haveRtu = true;
                                break;
                            }
                        }
                        if (haveRtu == false) continue; 
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                    }
                }
            }
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            deleteing = true;
            RequestInfo();
            GetAreaRId();
            if (AreaName.Count > 0)
            {
                if (parsObjects.Count() > 2)
                {
                    var areaid = (int)parsObjects[0];
                    foreach (var t in AreaName)
                    {
                        if (t.Key == areaid)
                        {
                            AreaComboBoxSelected = t;
                        }
                    }
                }
                if (AreaComboBoxSelected == null) AreaComboBoxSelected = AreaName.First();
            }
        }


        private void RequestInfo()
        {
            var info = Sr.ProtocolPhone.LxRtuTime.wst_timetable_tunnel_control_plan;
            // .wlst_cnt_request_sun_rise_set ;//.ServerPart.wlst_SunRiseSet_clinet_request_sunriseset;
            info.WstRtutimeTunnelControlPlan.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 2);
            return;
        }


        public void OnUserHideOrClosing()
        {
            AreaName.Clear();
            TunnelItems.Clear();
        }

        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "隧道方案"; }
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
    }
    /// <summary>
    /// ICommand
    /// </summary>
    public partial class TunnelInfoSetVm
    {
        /// <summary>
        /// 增加光控方案
        /// </summary>
        #region CmdAddLightTunnelInfo

        private AddOrModifyTunnelInfo _addTunnelInfo = null;
        private DateTime _dtCmdAddLightTunnelInfo;
        private ICommand _cmdAddLightTunnelInfo;

        public ICommand CmdAddLightTunnelInfo
        {
            get
            {
                return _cmdAddLightTunnelInfo ??
                       (_cmdAddLightTunnelInfo = new RelayCommand(ExCmdAddLightTunnelInfo, CanCmdAddLightTunnelInfo, true));
            }

        }

        private bool CanCmdAddLightTunnelInfo()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdAddLightTunnelInfo.Ticks > 10000000;
        }

        private void ExCmdAddLightTunnelInfo()
        {
            //var id = 0;
            //foreach (var t in TunnelItems)
            //{
            //    if (t.SchemeId >= id)
            //    {
            //        id = t.SchemeId;
            //    }
            //}
            _dtCmdAddLightTunnelInfo = DateTime.Now;
            _addTunnelInfo=new AddOrModifyTunnelInfo();
            var tvx = new TunnelInformation(AreaId);
            tvx.SubOperationCount = tvx.OperationItems.Count;
            //tvx.SchemeId = id + 1;
            tvx.IsLuxOrTime = 1;
            tvx.IsSelectlightEnable = true;
            tvx.IsSelectTimeEnable = false;
            tvx.ControlMode = "光控";
            //CurrentSelectedTunnelItem = tvx;
            _addTunnelInfo.OnFormBtnOkClick +=
                    new EventHandler<AddOrModifyTunnelInfo.EventArgsAddTunnel>(_addTunnelInfo_OnFormBtnOkClick);
            _addTunnelInfo.SetContext(tvx,AreaId);
            _addTunnelInfo.ShowDialog();
        }

        #endregion


        /// <summary>
        /// 增加时控方案
        /// </summary>
        #region CmdAddTimeTunnelInfo

        private DateTime _dtCmdAddTimeTunnelInfo;
        private ICommand _cmdAddTimeTunnelInfo;

        public ICommand CmdAddTimeTunnelInfo
        {
            get
            {
                return _cmdAddTimeTunnelInfo ??
                       (_cmdAddTimeTunnelInfo = new RelayCommand(ExCmdAddTimeTunnelInfo, CanCmdAddTimeTunnelInfo, true));
            }

        }

        private bool CanCmdAddTimeTunnelInfo()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdAddTimeTunnelInfo.Ticks > 10000000;
        }

        private void ExCmdAddTimeTunnelInfo()
        {
            //var id = 0;
            //foreach (var t in TunnelItems)
            //{
            //    if (t.SchemeId >= id)
            //    {
            //        id = t.SchemeId;
            //    }
            //}
            _dtCmdAddTimeTunnelInfo = DateTime.Now;
            _addTunnelInfo = new AddOrModifyTunnelInfo();
            var tvx = new TunnelInformation(AreaId);
            tvx.SubOperationCount = tvx.OperationItems.Count;
            //tvx.SchemeId = id + 1;
            tvx.IsLuxOrTime = 2;
            tvx.IsSelectTimeEnable = true;
            tvx.IsSelectlightEnable = false;
            tvx.ControlMode = "时控";
            //CurrentSelectedTunnelItem = tvx;
            _addTunnelInfo.OnFormBtnOkClick +=
                    new EventHandler<AddOrModifyTunnelInfo.EventArgsAddTunnel>(_addTunnelInfo_OnFormBtnOkClick);
            _addTunnelInfo.SetContext(tvx, AreaId);
            _addTunnelInfo.ShowDialog();
        }

        #endregion


        //方案确定 返回隧道方案主界面
        private void _addTunnelInfo_OnFormBtnOkClick(object sender, AddOrModifyTunnelInfo.EventArgsAddTunnel e) //todo 暂存
        {

            var updateinfo = e.AddTunnelInfo;
            if (updateinfo == null) return;

            var isAdd = true;

            //增加TunnelItems加一行，修改TunnelItems不变
            foreach (
                var item in TunnelItems.Where(item => item.SchemeId == updateinfo.SchemeId))
            {
                isAdd = false;
                item.SchemeName = updateinfo.SchemeName;
                item.IsLuxOrTime = updateinfo.IsLuxOrTime;
                item.IsLuxOrTimeStr = updateinfo.IsLuxOrTimeStr;
                item.OperationItems = updateinfo.OperationItems;
                item.ProtectTime = updateinfo.ProtectTime;
                item.SubOperationCount = updateinfo.SubOperationCount;
                item.TunnelName = updateinfo.TunnelName;
                item.CurrentSelectedTerminalItems = updateinfo.CurrentSelectedTerminalItems;
                item.CurrentSelectLux = updateinfo.CurrentSelectLux;
                item.CurrentSelectLux2 = updateinfo.CurrentSelectLux2;
                item.LuxId = updateinfo.LuxId;
                item.LuxId2 = updateinfo.LuxId2;
                item.LuxName = updateinfo.LuxName;
                item.LuxName2 = updateinfo.LuxName2;

            }

            if (isAdd)
            {
                TunnelItems.Add(updateinfo);
            }

            try
            {
                _addTunnelInfo.OnFormBtnOkClick -=
                    new EventHandler<AddOrModifyTunnelInfo.EventArgsAddTunnel>(_addTunnelInfo_OnFormBtnOkClick);
            }
            catch (Exception ex)
            {

            }
            _addTunnelInfo = null;
        }

        /// <summary>
        /// 修改方案
        /// </summary>
        #region CmdModifyTunnelInfo

        private DateTime _dtCmdModifyTunnelInfo;
        private ICommand _cmdModifyTunnelInfo;

        public ICommand CmdModifyTunnelInfo
        {
            get
            {
                return _cmdModifyTunnelInfo ??
                       (_cmdModifyTunnelInfo = new RelayCommand(ExCmdModifyTunnelInfo, CanCmdModifyTunnelInfo, true));
            }

        }

        private bool CanCmdModifyTunnelInfo()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 &&
                   DateTime.Now.Ticks - _dtCmdModifyTunnelInfo.Ticks > 10000000 && CurrentSelectedTunnelItem != null;
        }

        private void ExCmdModifyTunnelInfo()
        {
            _dtCmdModifyTunnelInfo = DateTime.Now;
            _addTunnelInfo = new AddOrModifyTunnelInfo();

            //用于修改的当前方案
            var tvx = new TunnelInformation(AreaId)
                          {
                              ControlMode = CurrentSelectedTunnelItem.ControlMode,
                              IsLuxOrTime = CurrentSelectedTunnelItem.IsLuxOrTime,
                              IsSelectTimeEnable = CurrentSelectedTunnelItem.IsSelectTimeEnable,
                              IsSelectlightEnable = CurrentSelectedTunnelItem.IsSelectlightEnable,
                              ProtectTime = CurrentSelectedTunnelItem.ProtectTime,
                              SchemeId = CurrentSelectedTunnelItem.SchemeId,
                              SchemeName = CurrentSelectedTunnelItem.SchemeName,
                              TunnelName = CurrentSelectedTunnelItem.TunnelName  ,

                              LuxChanged = CurrentSelectedTunnelItem.LuxChanged,
                              LuxId = CurrentSelectedTunnelItem.LuxId,
                              LuxId2 = CurrentSelectedTunnelItem.LuxId2,
                              LuxName = CurrentSelectedTunnelItem.LuxName,
                              LuxName2 = CurrentSelectedTunnelItem.LuxName2,

                              CurrentSelectLux = CurrentSelectedTunnelItem.CurrentSelectLux,
                              CurrentSelectLux2 = CurrentSelectedTunnelItem.CurrentSelectLux2,
                              LuxCollection = CurrentSelectedTunnelItem.LuxCollection,
                              LuxCollection2 = CurrentSelectedTunnelItem.LuxCollection2
                          };

            foreach (var t in CurrentSelectedTunnelItem.OperationItems)
            {
                var tmp = new ObservableCollection<OneItemTerminal>();
                foreach (var x in t.SelectedItems)
                {
                    var middleItems = new ObservableCollection<TimeInfoName>();
                    foreach (var f in x.Items)
                    {
                        middleItems.Add(new TimeInfoName
                        {
                            IsCheckSwitch = f.IsCheckSwitch,
                            TimeTableName = f.TimeTableName,
                            IsEnabledOn = f.IsEnabledOn
                        });
                    }

                    tmp.Add(new OneItemTerminal
                    {
                        Index = x.Index,
                        IsSelected = x.IsSelected,
                        IsEnable = x.IsEnable,
                        RtuId = x.RtuId,
                        RtuName = x.RtuName,
                        Items = middleItems
                    });
                }

                tvx.OperationItems.Add(new OneItemOperation(tmp)
                                           {
                                               OperationName = t.OperationName,
                                               OperationDesc = t.OperationDesc,
                                               LastTimeHour = t.LastTimeHour,
                                               LastTimeMinute = t.LastTimeMinute,
                                               //LuxChanged = t.LuxChanged,
                                               //LuxId = t.LuxId,
                                               //LuxId2 = t.LuxId2,
                                               //LuxName = t.LuxName,
                                               //LuxName2 = t.LuxName2,
                                               MaxLux = t.MaxLux,
                                               SelectedItems = tmp,
                                               //CurrentSelectLux = t.CurrentSelectLux,
                                               //CurrentSelectLux2 = t.CurrentSelectLux2,
                                               //LuxCollection = t.LuxCollection,
                                               //LuxCollection2 = t.LuxCollection2
                                           }
                    );

            }

            foreach (var t in CurrentSelectedTunnelItem.SelectedTerminalItems)
            {

                var middleItems1 = new ObservableCollection<TimeInfoName>();
                foreach (var f in t.Items)
                {
                    middleItems1.Add(new TimeInfoName
                    {
                        IsCheckSwitch = f.IsCheckSwitch,
                        TimeTableName = f.TimeTableName,
                        IsEnabledOn = f.IsEnabledOn
                    });
                }

                var tmp1 = new OneItemTerminal
                               {
                                   Index = t.Index,
                                   IsSelected = t.IsSelected,
                                   RtuId = t.RtuId,
                                   RtuName = t.RtuName,
                                   Items = middleItems1
                               };
                tvx.SelectedTerminalItems.Add(tmp1);
            }

            foreach (var t in CurrentSelectedTunnelItem.CurrentSelectedTerminalItems)
            {
                var middleItems3 = new ObservableCollection<TimeInfoName>();
                foreach (var f in t.Items)
                {
                    middleItems3.Add(new TimeInfoName
                    {
                        IsCheckSwitch = f.IsCheckSwitch,
                        TimeTableName = f.TimeTableName,
                        IsEnabledOn = f.IsEnabledOn
                    });
                }

                var tmp3 = new OneItemTerminal
                {
                    Index = t.Index,
                    IsSelected = t.IsSelected,
                    RtuId = t.RtuId,
                    RtuName = t.RtuName,
                    Items = middleItems3
                };
                tvx.CurrentSelectedTerminalItems.Add(tmp3);
            }

            if(tvx.OperationItems.Count>0)
            {
                tvx.CurrentSelectedOperationItem = tvx.OperationItems[0];
            }
            _addTunnelInfo.OnFormBtnOkClick +=
                    new EventHandler<AddOrModifyTunnelInfo.EventArgsAddTunnel>(_addTunnelInfo_OnFormBtnOkClick);
            _addTunnelInfo.SetContext(tvx,AreaId);
            _addTunnelInfo.ShowDialog();
        }

        #endregion

        /// <summary>
        /// 删除方案
        /// </summary>
        #region CmdDeleteTunnelInfo
        private DateTime _dtCmdDeleteTunnelInfo;
        private ICommand _cmdCmdDeleteTunnelInfo;

        public ICommand CmdDeleteTunnelInfo
        {
            get
            {
                return _cmdCmdDeleteTunnelInfo ??
                       (_cmdCmdDeleteTunnelInfo = new RelayCommand(ExCmdDeleteTunnelInfo, CanCmdDeleteTunnelInfo, true));
            }
        }

        private bool CanCmdDeleteTunnelInfo()
        {
            //if (TunnelItems.Count < 2) return false;  //当方案少于两条时不可删除
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 &&
                   DateTime.Now.Ticks - _dtCmdDeleteTunnelInfo.Ticks > 10000000 && CurrentSelectedTunnelItem != null;
        }


        private bool deleteing = false;

        private void ExCmdDeleteTunnelInfo()
        {
            _dtCmdDeleteTunnelInfo = DateTime.Now;

            if (TunnelItems.Contains(CurrentSelectedTunnelItem))
            {
                deleteing = true;
                var infoss = WlstMessageBox.Show("确认删除", "即将删除当前选中方案，是 继续，否 退出.", WlstMessageBoxType.YesNo);
                if (infoss != WlstMessageBoxResults.Yes) return;


                var info = Sr.ProtocolPhone.LxRtuTime.wst_timetable_tunnel_control_plan;
                info.WstRtutimeTunnelControlPlan.Op = 3;
                info.WstRtutimeTunnelControlPlan.AreaId = CurrentSelectedTunnelItem.SchemeId;

              
                info.Head.Gid += 1;
                SndOrderServer.OrderSnd(info, 10, 2);
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存 ...";


                TunnelItems.Remove(CurrentSelectedTunnelItem);
                if (TunnelItems.Count > 0) CurrentSelectedTunnelItem = TunnelItems[0];
                else CurrentSelectedTunnelItem = null;
                deleteing = false;
            }

        }
        #endregion

        /// <summary>
        /// 保存隧道方案
        /// </summary>
        #region CmdSaveTunnelInfo
        private DateTime _dtCmdSaveTunnelInfo;
        private ICommand _cmdSaveTunnelInfo;

        public ICommand CmdSaveTunnelInfo
        {
            get
            {
                return _cmdSaveTunnelInfo ??
                       (_cmdSaveTunnelInfo = new RelayCommand(ExCmdSaveTunnelInfo, CanCmdSaveTunnelInfo, true));
            }
        }

        private bool CanCmdSaveTunnelInfo()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && DateTime.Now.Ticks - _dtCmdSaveTunnelInfo.Ticks > 10000000;
        }

        private void ExCmdSaveTunnelInfo()
        {
            _dtCmdSaveTunnelInfo = DateTime.Now;
            //var items = new List<TunnelInformation>();
            var temp = new List<TunnelControlPlan.TunnelControlOnePlan>();

            foreach (var t in TunnelItems)
            {
                var itemplan = new List<TunnelControlPlan.TunnelControlOnePlan.TunnelControlOnePlanOpeItem>();
                var rtubelong = new List<int>();
                foreach (var n in t.CurrentSelectedTerminalItems)
                {
                    rtubelong.Add(n.RtuId);
                }
                int index = 0;
                foreach (var f in t.OperationItems)
                {
                    index++;
                    var itemrtu = new List<TunnelControlPlan.TunnelControlOnePlan.TunnelControlOnePlanOpeItem.RtuOpe>();
                    foreach (var l in f.SelectedItems)
                    {
                        if ( l.IsEnable ==false ) continue;

                        var switchout = new List<int>();
                        var noswitchout = new List<int>();
                        for (int i = 0; i < 8; i++)
                        {
                            if (l.Items[i].IsCheckSwitch)
                                switchout.Add(i + 1);
                            else
                            {
                                if (l.Items[i].IsEnabledOn)
                                    noswitchout.Add(i + 1);
                            }
                        }
                        itemrtu.Add(new TunnelControlPlan.TunnelControlOnePlan.TunnelControlOnePlanOpeItem.RtuOpe()
                                        {
                                            RtuId = l.RtuId,
                                            SwitchOutNeedOpen = switchout,
                                            SwitchOutNeedClose = noswitchout,

                                        });
                    }
                    itemplan.Add(new TunnelControlPlan.TunnelControlOnePlan.TunnelControlOnePlanOpeItem()
                                     {
                                         Id = index,
                                         //LuxId = f.LuxId,   改用到方案中
                                         //LuxIdBackup = f.LuxId2,
                                         MaxValue = t.IsLuxOrTime == 1 ? f.MaxLux : f.LastTimeHour*60 + f.LastTimeMinute,
                                         OpeDesc = f.OperationDesc,
                                         OpeName = f.OperationName,
                                         ItemRtuOpe = itemrtu
                                     });
                }
                temp.Add(new TunnelControlPlan.TunnelControlOnePlan()
                             {
                                 AreaId = t.AreaId,
                                 IsLightControl = t.IsLuxOrTime,
                                 PlanId = t.SchemeId == 0 ? -1 : t.SchemeId,
                                 PlanName = t.SchemeName,
                                 TimeProtect = t.ProtectTime,
                                 TunnelName = t.TunnelName,
                                 ItemPlan = itemplan,
                                 RtusBelongThisTunnel = rtubelong,
                                 LuxId = t.LuxId,
                                 LuxIdBackup = t.LuxId2,
                             });
            }
            var info = Sr.ProtocolPhone.LxRtuTime.wst_timetable_tunnel_control_plan;
            info.WstRtutimeTunnelControlPlan.Op = 2;
            info.WstRtutimeTunnelControlPlan.AreaId = this.AreaId;
            info.WstRtutimeTunnelControlPlan.ItemsTunnelControlPlan.AddRange(temp);
            var infoss = WlstMessageBox.Show("确认保存", "即将保存信息，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;
            info.Head.Gid += 1;  
            SndOrderServer.OrderSnd(info, 10, 2);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存 ...";



        }


        #endregion


    }

    /// <summary>
    /// 加载隧道方案
    /// </summary>
    public partial class TunnelInfoSetVm
    {
        private void LoadTunnelScheme(int areaId)
        {
            TunnelItems.Clear();
            var plans = (from t in Wlst.Sr.TimeTableSystem.Services.TunnelSetHold.Myself.Info
                         where t.Key.Item1 == areaId
                         orderby t.Key.Item2 ascending
                         select t.Value).ToList();
            foreach (var t in plans)
            {
                TunnelItems.Add(new TunnelInformation(AreaId,t));
            }
            if (TunnelItems.Count > 0)
            {
                CurrentSelectedTunnelItem = TunnelItems[0];
                //if(CurrentSelectedTunnelItem.OperationItems.Count>0)
                //{
                //    CurrentSelectedTunnelItem.CurrentSelectedOperationItem = CurrentSelectedTunnelItem.OperationItems[0];
                //}
            }
                

            


        }

    }

    /// <summary>
    /// 集合或变量
    /// </summary>
    public partial class TunnelInfoSetVm
    {

        private bool _currIsSelectTmlEnable;
        /// <summary>
        /// 终端是否能显示
        /// </summary>
        public bool IsSelectTmlEnable
        {
            get { return _currIsSelectTmlEnable; }
            set
            {
                if (value != _currIsSelectTmlEnable)
                {
                    _currIsSelectTmlEnable = value;
                    this.RaisePropertyChanged(() => this.IsSelectTmlEnable);
                }

            }
        }



        private string _msg;
        /// <summary>
        /// 通知
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value == _msg) return;
                _msg = value;
                RaisePropertyChanged(() => Msg);
            }
        }



        private static ObservableCollection<AreaInt> _areaName;
        /// <summary>
        /// 区域名
        /// </summary>
        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_areaName == null)
                {
                    _areaName = new ObservableCollection<AreaInt>();
                }
                return _areaName;
            }

        }

       


        public class AreaInt : Wlst.Cr.Core.CoreServices.ObservableObject
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

        private  AreaInt _areacomboboxselected;
        private  int AreaId;
        public static int AreaId1;  //用于传递的区域ID


        public  AreaInt AreaComboBoxSelected
        {
            get { return _areacomboboxselected; }
            set
            {
                if (_areacomboboxselected != value)
                {
                    _areacomboboxselected = value;
                    this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
                    if (value == null) return;
                    AreaId = value.Key;
                    AreaId1 = value.Key;
                    LoadTunnelScheme(AreaId);
                }
            }
        }

        public static ObservableCollection<TunnelInformation> PassTunnelItems;
        private ObservableCollection<TunnelInformation> _tunnelItems;
        /// <summary>
        /// 隧道方案集合
        /// </summary>
        public ObservableCollection<TunnelInformation> TunnelItems
        {
            get
            {
                PassTunnelItems = _tunnelItems;
                if (_tunnelItems == null) _tunnelItems = new ObservableCollection<TunnelInformation>();
                return _tunnelItems;
            }
            set
            {
                if (value == _tunnelItems) return;
                _tunnelItems = value;
                this.RaisePropertyChanged(() => TunnelItems);
            }

        }


        private TunnelInformation _currentselectTunnelItems;
        /// <summary>
        /// 当前所选方案
        /// </summary>
        public TunnelInformation CurrentSelectedTunnelItem
        {
            get { return _currentselectTunnelItems; }
            set
            {
                IsSelectTmlEnable = value != null;

                if (value != _currentselectTunnelItems)
                {

                    if (_currentselectTunnelItems != null && deleteing == false)
                    {

                        _currentselectTunnelItems.Marked = "";
                    }

                    _currentselectTunnelItems = value;
                    if (_currentselectTunnelItems != null) _currentselectTunnelItems.Marked = "设置中...";
                    this.RaisePropertyChanged(() => this.CurrentSelectedTunnelItem);

                }
                else
                {

                    //todo
                }

            }
        }


    }
}
