using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Elysium.ThemesSet.RadGridViewSet;
using Microsoft.Practices.Prism.Commands;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.Sr.TimeTableSystem.Models;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.client;
using Color = System.Drawing.Color;
using Wlst.Ux.TimeTableSystem.TimeInfoNew.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoNew.Services;

namespace Wlst.Ux.TimeTableSystem.TimeInfoNew
{
    [Export(typeof(IITimeInfoNew))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeInfoNewViewModel : VmEventActionProperyChangedBase, IITimeInfoNew
    {
        public TimeInfoNewViewModel()
        {
            InitAction();
            InitEvent();
        }

        private bool _isViewActive = false;
        private bool isUserSelectGrp = false;
        private int aareaId = 0;
        private int ggrpId = 0;
        private List<int> rrrtulst = new List<int>();
        public void NavOnLoad(params object[] parsObjects)
        {
            _isViewActive = true;
            isUserSelectGrp = false;
            Msg = "";
            AreaName.Clear();
            getAreaRId();
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
            if (AreaName.Count > 1) Visi = Visibility.Visible;
            else Visi = Visibility.Collapsed;

            //grpid //areaid
            if (parsObjects.Count() > 1)
            {
                this.TreeItems.Clear();
                isUserSelectGrp = true;
                Visi = Visibility.Collapsed;
                aareaId = (int)parsObjects[0];
                ggrpId = (int)parsObjects[1];

                var tu = new Tuple<int, int>(aareaId, ggrpId);

                if (Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu) == false) return;

                var tn = new TreeGrpNodes(aareaId, ggrpId, true);

                this.TreeItems.Add(tn);


            }
            else if (parsObjects.Count() == 1)  //交叉分组
            {
                var rtulst = parsObjects[0] as List<int>;
                if (rtulst == null || rtulst.Count == 0) return;

                this.TreeItems.Clear();
                rrrtulst.Clear();

                isUserSelectGrp = true;

                rrrtulst.AddRange(rtulst);
                var tn = new TreeGrpNodes("地域分组",rtulst);

                this.TreeItems.Add(tn);
            }
            if (OnNavOnLoadSelectdRtus != null)
            {
                OnNavOnLoadSelectdRtus(isUserSelectGrp ? null : this, EventArgs.Empty);
            }

        }

        public event EventHandler<EventArgsEx> OnUserWantSetGroupWeekSet;


        public void OnUserHideOrClosing()
        {
            _isViewActive = false;
        }

        #region tab iinterface

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "应急操作查询"; }
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


        public void getAreaRId()
        {
            AreaName.Clear();
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    string area = t.Value.AreaName;
                    AreaName.Add(new AreaInt() { Value = t.Value.AreaId.ToString("d2") + "-" + area, Key = t.Value.AreaId });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                    }
                }
            }
            Visi = Visibility.Collapsed;
            if (AreaName.Count > 1) Visi = Visibility.Visible;


        }
        private static ObservableCollection<AreaInt> _devices;

        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<AreaInt>();
                }
                return _devices;
            }

        }

        private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }


        private string _msg;

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

        private AreaInt _areacomboboxselected;
        private int AreaId;
        public static int areaId;
        public AreaInt AreaComboBoxSelected
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
                    areaId = value.Key;
                    LoadTimeTableInfoFromSr();
                    this.LoadRtuOrGrpBandingInfo();

                }
            }
        }

    }

    /// <summary>
    /// Attri
    /// </summary>
    public partial class TimeInfoNewViewModel
    {
        #region Attribute


        private TimeTableInfomationItem _currentSelectItem = null;

        public TimeTableInfomationItem CurrentSelectItem
        {
            get { return _currentSelectItem; }
            set
            {
                if (_currentSelectItem == value || value == null) return;
                _currentSelectItem = value;
                if (_currentSelectItem.LuxId2 == 0) _currentSelectItem.ShowCurrentSelectLux2 = 0;
                RaisePropertyChanged(() => CurrentSelectItem);
            }
        }

        public static ObservableCollection<TimeTableInfomationItem> PassItems;
        private ObservableCollection<TimeTableInfomationItem> _items;

        public ObservableCollection<TimeTableInfomationItem> Items
        {
            get
            {
                PassItems = _items;
                return _items ?? (_items = new ObservableCollection<TimeTableInfomationItem>());
            }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => Items);
            }
        }


        #endregion

        //初始化数据池，从服务层中读取数据
        private void LoadTimeTableInfoFromSr()
        {
            Items.Clear();
            //addid = 1;
            foreach (var itemTable in WeekTimeTableInfoService.GeteekTimeTableInfoList(AreaId))
            {
                Items.Add(new TimeTableInfomationItem(itemTable, AreaId));
            }
            if (Items.Count > 0) CurrentSelectItem = Items[0];


            //foreach (var t in Items)
            //{
            //    if (t.TimeId >= addid)
            //    {
            //        addid = t.TimeId + 1;
            //    }
            //}
        }

    }


    /// <summary>
    /// Method
    /// </summary>
    public partial class TimeInfoNewViewModel
    {
        #region load  RtuOrGrp

        //加载终端节点
        private void LoadRtuOrGrpBandingInfo()
        {
            TreeItems.Clear();

            var ctig = new ObservableCollection<TreeGrpNodes>();

            foreach (var t in Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.Keys)
            {
                if (t.Item1 == AreaId && t.Item2 > 0) ctig.Add(new TreeGrpNodes(AreaId, t.Item2, true));
            }

            var lstg = (from a in ctig orderby a.PhyId select a).ToList();

            //加载无分组终端节点
            //var tmp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            //foreach (var f in tmp)
            //{
            //    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(f))
            //    {
            //        var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[f];
            //        if (para.EquipmentType == WjParaBase.EquType.Rtu)
            //        {
            //            ctit.Add(new OneGrpOrRtuLoopsSet(AreaId, f, false));
            //        }
            //    }
            //}

            var lstt = (from a in ctig orderby a.PhyId select a).ToList();

            foreach (var t in lstg) this.TreeItems.Add(t);

            //加载无分组终端节点
            var tmp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            var rtuLstTmp = new List<int>();
            //获取特殊设备
            foreach (var f in tmp)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                {
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[f];
                    if (para.EquipmentType == WjParaBase.EquType.Rtu)
                    {
                        rtuLstTmp.Add(f);
                    }
                }

            }
            //如果有特殊终端 则添加特殊分组
            if (rtuLstTmp.Count > 0) this.TreeItems.Add(new TreeGrpNodes(AreaId, -1, true));

        }



        #endregion

        private ObservableCollection<TreeGrpNodes> _treeItemsInfo;

        public ObservableCollection<TreeGrpNodes> TreeItems
        {
            get
            {
                if (_treeItemsInfo == null)
                    _treeItemsInfo = new ObservableCollection<TreeGrpNodes>();
                return _treeItemsInfo;
            }
            set
            {
                if (value == _treeItemsInfo) return;
                _treeItemsInfo = value;
                this.RaisePropertyChanged(() => TreeItems);
            }
        }

        #region CmdExport
        private DateTime _dtCmdExport;
        private ICommand _cmdCmdExport;

        public ICommand CmdExport
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, false);
                return _cmdCmdExport;
            }
        }

        private void ExCmdExport()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("地址及类型");
                lsttitle.Add("组、终端名称");
                lsttitle.Add("K1时间表");
                lsttitle.Add("K2时间表");
                lsttitle.Add("K3时间表");
                lsttitle.Add("K4时间表");
                lsttitle.Add("K5时间表");
                lsttitle.Add("K6时间表");
                lsttitle.Add("K7时间表");
                lsttitle.Add("K8时间表");


                var lstobj = new List<List<object>>();

                foreach (var g in TreeItems)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.PhyIdMsg);
                    tmp.Add(g.RtuOrGrpName);
                    tmp.Add(g.Items[0].TimeTableName);
                    tmp.Add(g.Items[1].TimeTableName);
                    tmp.Add(g.Items[2].TimeTableName);
                    tmp.Add(g.Items[3].TimeTableName);
                    tmp.Add(g.Items[4].TimeTableName);
                    tmp.Add(g.Items[5].TimeTableName);
                    tmp.Add(g.Items[6].TimeTableName);
                    tmp.Add(g.Items[7].TimeTableName);


                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            if (TreeItems.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion


        private DateTime dtSnd;

        #region CmdSave

        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new RelayCommand(Ex, CanExSave, true)); }
        }

        private bool CanExSave()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId) || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) && DateTime.Now.Ticks - dtSnd.Ticks > 30000000;
        }

        private void Ex()
        {
            dtSnd = DateTime.Now;
            var bandingRelation = new List<TimeTableBandingRtuInfoNew.RtuOrGrpBandingTimeTableItem>();
            var lst = new List<Tuple<int, int, int>>();
            foreach (var t in TreeItems)
            {
                var grpBandingInfo = new List<TimeTableBandingRtuInfoNew.RtuOrGrpBandingTimeTableItem.RtuOrGrpBandingTimeTableSwitchOutItem>();
                bool haveBanding = false;
                //添加分组信息
                for (int i = 0; i < t.Items.Count; i++)
                {
                    if (t.Items[i].TimeTalbe > 0)
                    {
                        lst.Add(new Tuple<int, int, int>(t.RtuOrGrpId, i + 1, t.Items[i].TimeTalbe));
                        grpBandingInfo.Add(new TimeTableBandingRtuInfoNew.RtuOrGrpBandingTimeTableItem.RtuOrGrpBandingTimeTableSwitchOutItem()
                        {
                            LoopId = i + 1,
                            TimeTableId = t.Items[i].TimeTalbe
                        });
                        haveBanding = true;
                    };

                }
                if (haveBanding && t.RtuOrGrpId!=0)
                {
                    
                    bandingRelation.Add(new TimeTableBandingRtuInfoNew.RtuOrGrpBandingTimeTableItem()
                    {
                        RtuOrGrpId = t.RtuOrGrpId,
                        SwitchOutBandingTimeTableInfo = grpBandingInfo,
                    });
                }


                //添加 子节点绑定信息
                foreach (var g in t.ChildTreeItems)
                {
                    grpBandingInfo = new List<TimeTableBandingRtuInfoNew.RtuOrGrpBandingTimeTableItem.RtuOrGrpBandingTimeTableSwitchOutItem>();
                    haveBanding = false;
                    for (int i = 0; i < g.Items.Count; i++)
                    {
                        if (g.Items[i].TimeTalbe > 0)
                        {
                            lst.Add(new Tuple<int, int, int>(g.RtuOrGrpId, i + 1, g.Items[i].TimeTalbe));
                            grpBandingInfo.Add(new TimeTableBandingRtuInfoNew.RtuOrGrpBandingTimeTableItem.RtuOrGrpBandingTimeTableSwitchOutItem()
                            {
                                LoopId = i + 1,
                                TimeTableId = g.Items[i].TimeTalbe
                            });
                            haveBanding = true;
                        };

                    }
                    if (haveBanding)
                    {
                        bandingRelation.Add(new TimeTableBandingRtuInfoNew.RtuOrGrpBandingTimeTableItem()
                        {
                            RtuOrGrpId = g.RtuOrGrpId,
                            SwitchOutBandingTimeTableInfo = grpBandingInfo,
                        });
                    }
                    

                }





            }
            if (lst.Count == 0)
            {
                Msg = "无任何终端的绑定信息，无法执行保存...";
                return;
            }

            var rtn = new Dictionary<int, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
            var rtn1 = new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
            foreach (var f in Items)
            {
                var lst1 = f.BackToWeekTimeTableInforemation();
                rtn.Add(f.TimeId, lst1);
                rtn1.Add(lst1);
            }


            //if (rtn.Count == 0)
            //{
            //    Msg = "无任何时间表信息，无法执行保存...";
            //    return;
            //}

            foreach (var t in lst)
            {
                var isgrp = new bool();
                if (t.Item1 > 999999)
                {
                    isgrp = false;
                }
                else isgrp = true;
                var rtuitem = new OneGrpOrRtuLoopsSet(AreaId, t.Item1, isgrp);
                var timetableitem = new TimeTableInfomationItem();

                if (rtn.ContainsKey(t.Item3))
                {
                    timetableitem = new TimeTableInfomationItem(rtn[t.Item3], AreaId);
                }
                else
                {
                    WlstMessageBox.Show("警告", "无时间表信息，无法保存！", WlstMessageBoxType.Ok);
                    return;
                }


                if (rtuitem.Has3005 && timetableitem.MainIsOverOne[0] && Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection == false)
                {
                    WlstMessageBox.Show("警告", "有非3006设备绑定了多段时间表，无法保存！", WlstMessageBoxType.Ok);
                    return;
                }
            }

            var infoss = WlstMessageBox.Show("确认保存", "即将保存信息，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;

            dtSnd = DateTime.Now;
            //保存时间表
            //UpdateBandingRelation(AreaId, rtn1);

            var info = Wlst.Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_bandingnew;
            info.WstRtutimeTimetableSetbandingnew.Op = 2;
            info.WstRtutimeTimetableSetbandingnew.AreaId = areaId;
            info.WstRtutimeTimetableSetbandingnew.RtuOrGrpTimeTableAndBandingItems = bandingRelation;
            SndOrderServer.OrderSnd(info, 10, 6);

            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "周设置修改", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新周设置");


            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存 ...";
        }

        /// <summary>
        /// 更新时间表
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="lstTimeTables"></param>
        /// <param name=""></param>
        //public void UpdateTimeTableInfos(int areaId,List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> lstTimeTables)
        //{
        //    if (lstTimeTables.Count == 0 ) return;
        //    var info = Wlst.Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_new;


        //    foreach (var f in lstTimeTables)
        //    {
        //        info.WstRtutimeTimetableSetnew.TimeTableItems.Add(f);
        //    }
        //    info.WstRtutimeTimetableSetnew.Op = 2;
        //    info.WstRtutimeTimetableSetnew.AreaId = areaId;
        //    SndOrderServer.OrderSnd(info, 10, 6);



        //    Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
        //        0, "周设置修改", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新周设置");
        //}


        public void UpdateBandingRelation(int areaId, List<Tuple<int, int, int>> lst)
        {
            if (lst.Count == 0) return;
            var info = Wlst.Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_bandingnew;
            var tmp = new List<TimeTableBandingRtuInfoNew.RtuOrGrpBandingTimeTableItem>();
            
            info.WstRtutimeTimetableSetbandingnew.Op = 2;
            info.WstRtutimeTimetableSetbandingnew.AreaId = areaId;
            info.WstRtutimeTimetableSetbandingnew.RtuOrGrpTimeTableAndBandingItems = tmp;
            SndOrderServer.OrderSnd(info, 10, 6);



            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                0, "周设置修改", Wlst.Cr.CoreMims.ShowMsgInfo.OperatrType.UserOperator, "更新周设置");
        }


        public static Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView.SetWeekAck SetWeekAck = null;

        public static ObservableCollection<WeekSndReport> AllWeekSndReport = new ObservableCollection<WeekSndReport>();
        public static ObservableCollection<WeekSndReport> CurrentWeekSndReport = new ObservableCollection<WeekSndReport>();

        public static void SendWeekSet()
        {
            while (flgModify == true)
            {

            }

            ObservableCollection<WeekSndReport> ansWeekSndReport = GetAnsWeekReport(AllWeekSndReport);

            if (ansWeekSndReport.Count == 0)
            {
                return;
            }

            List<int> _lstRtu = new List<int>();

            for (int i = 0; i < ansWeekSndReport.Count; i++)
            {
                _lstRtu.Add(ansWeekSndReport[i].RtuId);

                int j = -1;
                int index = 0;
                var m = new WeekSndReport();

                foreach (WeekSndReport t in AllWeekSndReport)
                {
                    j++;

                    if (ansWeekSndReport[i].RtuId == t.RtuId)
                    {
                        index = j;

                        m = new WeekSndReport(t.RtuId, t.PhysicalId, t.NodeName, t.State, 0);

                        break;
                    }

                }

                AllWeekSndReport.RemoveAt(index);
                AllWeekSndReport.Insert(index, m);


                i = -1;

                foreach (WeekSndReport t in CurrentWeekSndReport)
                {
                    i++;

                    if (m.RtuId == t.RtuId)
                    {
                        index = i;

                        break;
                    }

                }

                CurrentWeekSndReport.RemoveAt(index);
                CurrentWeekSndReport.Insert(index, m);

            }

            //新协议
 
            var infos = Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_bandingnew;
            infos.Args.Addr.AddRange(_lstRtu);
            infos.WstRtutimeTimetableSetnew.AreaId = areaId;
            infos.WstRtutimeTimetableSetnew.Op=2;
            infos.WstRtutimeTimetableSetnew.TimeTableItems = new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
            SndOrderServer.OrderSnd(infos, 10, 6);
        }

        public static void ShowWeekReport(int arg)
        {
            if (arg == 1)
            {
                var xxxx = new WeekReport { xWeekSndReport = CurrentWeekSndReport = AllWeekSndReport, OcCount = AllWeekSndReport.Count(), OcTmlCount = AllWeekSndReport.Count() - (GetAnsWeekReport(AllWeekSndReport)).Count };
                SetWeekAck.SetContext(xxxx);
            }
            else if (arg == 2)
            {
                ObservableCollection<WeekSndReport> ansWeekSndReport = GetAnsWeekReport(AllWeekSndReport);
                var xxxx = new WeekReport { xWeekSndReport = CurrentWeekSndReport = ansWeekSndReport, OcCount = AllWeekSndReport.Count(), OcTmlCount = AllWeekSndReport.Count() - ansWeekSndReport.Count };
                SetWeekAck.SetContext(xxxx);
            }


        }

        private static ObservableCollection<WeekSndReport> GetAnsWeekReport(ObservableCollection<WeekSndReport> WeekSndReport)
        {
            var _WeekSndReport = new ObservableCollection<WeekSndReport>();

            foreach (WeekSndReport t in WeekSndReport)
            {
                if (t.WeekAck != 3)
                {
                    _WeekSndReport.Add(t);
                }
            }

            return _WeekSndReport;
        }

        private void GetWeekReport(List<int> updatedRtu)
        {
            int PhysicalId = 0;
            string NodeName = string.Empty;
            string status = string.Empty;


            AllWeekSndReport.Clear();

            foreach (int t in updatedRtu)
            {
                var infox = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t);

                if (infox != null)
                {
                    PhysicalId = infox.RtuPhyId;
                    NodeName = infox.RtuName;
                    status = GetEquipmentStatus(infox.RtuStateCode);

                    AllWeekSndReport.Add(new WeekSndReport(t, PhysicalId, NodeName, status, 0));

                }
            }
        }

        private string GetEquipmentStatus(int rtuStateCode)
        {
            string status = string.Empty;

            switch (rtuStateCode)
            {
                case 0:
                    status = "不用";
                    break;
                case 1:
                    status = "停用";
                    break;
                default:
                    status = "使用";
                    break;
            }

            return status;
        }

        private List<int> ReturnNeedUpdatedEquipment(int areaId, List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> timeTable, List<Tuple<int, int, int>> Equipmentlist)
        {
            List<int> updatedRtu = new List<int>();

            var xxx = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(areaId);

            foreach (var t in xxx)
            {
                if (t > 1000000 && t < (1000000 + 99999))
                {
                    var xx = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t);

                    if (xx.RtuModel == EnumRtuModel.Wj3005)
                    {
                        AddRtuId(t, ref updatedRtu);
                    }
                }
            }

            var updatedTimeTable = new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();

            var tmp =
                (from t in WeekTimeTableInfoService.WeekTimeTableInfoDictionary
                 orderby t.Key.Item1, t.Key.Item2
                 select t);

            for (int i = 0; i < timeTable.Count; i++)
            {
                bool flag = false;

                foreach (var t in tmp)
                {
                    if ((t.Key.Item1 == areaId) && (t.Key.Item2 == timeTable[i].TimeId))
                    {
                        flag = true;

                        if (CompareTimeTable(t.Value, timeTable[i]) == false)
                        {
                            updatedTimeTable.Add(timeTable[i]);
                        }

                        break;
                    }
                }

                if (flag == false)
                {
                    updatedTimeTable.Add(timeTable[i]);
                }
            }

            List<Tuple<int, int, int>> SourceEquipmentlist = new List<Tuple<int, int, int>>();

            foreach (var t in RtuOrGprBandingTimeTableInfoService.BandingInfoDictionary)
            {
                if (areaId == t.Key.Item1)
                {
                    foreach (var m in t.Value)
                    {
                        SourceEquipmentlist.Add(new Tuple<int, int, int>(t.Key.Item2, m.Key, m.Value));
                    }
                }
            }

            bool flgExist = false;

            foreach (Tuple<int, int, int> t in Equipmentlist)
            {
                flgExist = false;

                if (IfUpdatedTimeTable(t.Item3, updatedTimeTable) == false)
                {
                    foreach (Tuple<int, int, int> t1 in SourceEquipmentlist)
                    {
                        if ((t.Item1 == t1.Item1) && (t.Item2 == t1.Item2))
                        {
                            flgExist = true;

                            if (t.Item2 != t1.Item2)
                            {
                                AddRtuId(areaId, t.Item1, ref updatedRtu);
                            }

                            break;
                        }
                    }

                    if (flgExist == false)
                    {
                        AddRtuId(areaId, t.Item1, ref updatedRtu);
                    }
                }
                else
                {
                    AddRtuId(areaId, t.Item1, ref updatedRtu);
                }
            }


            foreach (Tuple<int, int, int> t in SourceEquipmentlist)
            {
                flgExist = false;

                if (IfUpdatedTimeTable(t.Item3, updatedTimeTable) == false)
                {
                    foreach (Tuple<int, int, int> t1 in Equipmentlist)
                    {
                        if ((t.Item1 == t1.Item1) && (t.Item2 == t1.Item2))
                        {
                            flgExist = true;

                            if (t.Item2 != t1.Item2)
                            {
                                AddRtuId(areaId, t.Item1, ref updatedRtu);
                            }

                            break;
                        }
                    }

                    if (flgExist == false)
                    {
                        AddRtuId(areaId, t.Item1, ref updatedRtu);
                    }
                }
                else
                {
                    AddRtuId(areaId, t.Item1, ref updatedRtu);
                }
            }

            return updatedRtu;
        }

        private void AddRtuId(int _areaId, int id, ref List<int> updatedRtu)
        {
            if (id > 1000000 && id < (1000000 + 99999))
            {
                var xx =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);

                if (xx.RtuModel == EnumRtuModel.Wj3005)
                {
                    AddRtuId(id, ref updatedRtu);
                }
            }
            else if (id >
                     Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GroupStartId
                     &&
                     id <
                     (Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GroupStartId + 999))
            {
                var info =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.
                        GetGroupInfomation(_areaId,
                                          id);

                foreach (var tml in info.LstTml)
                {
                    if (tml > 1000000 && tml < (1000000 + 99999))
                    {
                        var xx =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(tml);

                        if (xx.RtuModel == EnumRtuModel.Wj3005)
                        {
                            AddRtuId(tml, ref updatedRtu);
                        }
                    }
                }
            }
        }

        private void AddRtuId(int id, ref List<int> updatedRtu)
        {
            bool add = true;

            foreach (int t in updatedRtu)
            {
                if (t == id)
                {
                    add = false;
                    break;
                }
            }

            if (add)
            {
                updatedRtu.Add(id);
            }
        }

        private bool IfUpdatedTimeTable(int timeId, List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem> rtn)
        {
            for (int i = 0; i < rtn.Count; i++)
            {
                if (rtn[i].TimeId == timeId)
                {
                    return true;
                }
            }

            return false;
        }

        private bool CompareTimeTable(TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem item1, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem item2)
        {
            if (item1.LightOffOffset != item2.LightOffOffset)
            {
                return false;
            }

            if (item1.LightOnOffset != item2.LightOnOffset)
            {
                return false;
            }

            if (item1.LuxEffective != item2.LuxEffective)
            {
                return false;
            }

            if (item1.LuxId != item2.LuxId)
            {
                return false;
            }

            if (item1.LuxIdBackup != item2.LuxIdBackup)
            {
                return false;
            }

            if (item1.LuxOffValue != item2.LuxOffValue)
            {
                return false;
            }

            if (item1.LuxOnValue != item2.LuxOnValue)
            {
                return false;
            }

            if (item1.TimeDesc != item2.TimeDesc)
            {
                return false;
            }

            if (item1.TimeId != item2.TimeId)
            {
                return false;
            }

            if (item1.TimeName != item2.TimeName)
            {
                return false;
            }

            if (item1.RuleItems.Count != item2.RuleItems.Count)
            {
                return false;
            }

            for (int i = 0; i < item1.RuleItems.Count; i++)
            {
                if (item1.RuleItems[i].DateEnd != item2.RuleItems[i].DateEnd)
                {
                    return false;
                }

                if (item1.RuleItems[i].DateStart != item2.RuleItems[i].DateStart)
                {
                    return false;
                }

                if (item1.RuleItems[i].RuleId != item2.RuleItems[i].RuleId)
                {
                    return false;
                }

                if (item1.RuleItems[i].RuleSectionId != item2.RuleItems[i].RuleSectionId)
                {
                    return false;
                }

                if (item1.RuleItems[i].TimeOff != item2.RuleItems[i].TimeOff)
                {
                    return false;
                }

                if (item1.RuleItems[i].TimeOn != item2.RuleItems[i].TimeOn)
                {
                    return false;
                }

                if (item1.RuleItems[i].TimetableSectionId != item2.RuleItems[i].TimetableSectionId)
                {
                    return false;
                }

                if (item1.RuleItems[i].TypeOff != item2.RuleItems[i].TypeOff)
                {
                    return false;
                }

                if (item1.RuleItems[i].TypeOn != item2.RuleItems[i].TypeOn)
                {
                    return false;
                }

                if (item1.RuleItems[i].DayOfWeekUsed.Count != item2.RuleItems[i].DayOfWeekUsed.Count)
                {
                    return false;
                }

                for (int j = 0; j < item1.RuleItems[i].DayOfWeekUsed.Count; j++)
                {
                    if (item1.RuleItems[i].DayOfWeekUsed[j] != item2.RuleItems[i].DayOfWeekUsed[j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion


        /// <summary>
        /// 用户选择时间表后，更新界面
        /// </summary>
        /// <param name="rtuOrGrpId"></param>
        /// <param name="loop"></param>
        /// <param name="newTimeTableId"></param>
        /// <param name="newTimeTableName"></param>
        /// <param name="newTimeTimeDesc"></param>
        public void UpdateNodeTimeTable(int rtuOrGrpId, int loop, int newTimeTableId, string newTimeTableName, string newTimeTimeDesc)
        {
            //分组
            if (rtuOrGrpId < 1000000)
            {
                foreach (var f in TreeItems)
                {
                    if (f.RtuOrGrpId != rtuOrGrpId) continue;
                    f.UpdateTimeInfo(loop, newTimeTableId, newTimeTableName, newTimeTimeDesc);
                    if (f.IsChecked == false) break;
                    //子节点同步修改
                    foreach (var g in f.ChildTreeItems)
                    {
                        if (g.IsChecked == false) continue;
                        g.UpdateTimeInfo(loop, newTimeTableId, newTimeTableName, newTimeTimeDesc);
                    }
  
                    break;
                }
            }
            else  //终端
            {
                var groupidx =Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(
                                            rtuOrGrpId);
                if (groupidx != null)
                {
                    foreach (var f in TreeItems)
                    {
                        if (f.IsGroup && f.RtuOrGrpId == 0)
                        {
                            foreach (var g in f.ChildTreeItems)
                            {
                                if (g.RtuOrGrpId != rtuOrGrpId) continue;
                                g.UpdateTimeInfo(loop, newTimeTableId, newTimeTableName, newTimeTimeDesc);
                                break;
                            }
                        }
                        else
                        {
                            //判断是否是该分组下终端
                            if (f.RtuOrGrpId != groupidx.Item2) continue;
                            //子节点同步修改
                            foreach (var g in f.ChildTreeItems)
                            {
                                if (g.RtuOrGrpId != rtuOrGrpId) continue;
                                g.UpdateTimeInfo(loop, newTimeTableId, newTimeTableName, newTimeTimeDesc);
                                break;
                            }

                        }

                     


                    }

                }
                else  //特殊终端
                {
                    foreach (var f in TreeItems)
                    {
                        if (f.IsGroup && f.RtuOrGrpId ==-1)
                        {
                            foreach (var g in f.ChildTreeItems)
                            {
                                if (g.RtuOrGrpId != rtuOrGrpId) continue;
                                g.UpdateTimeInfo(loop, newTimeTableId, newTimeTableName, newTimeTimeDesc);
                                break;
                            }
                        }
                    

                    }
                }

              
            }
           
        }


        private int GetBandingTimeTableInfo(int timetableid)
        {
            int xcount = 0;
            foreach (var t in TreeItems)
            {
                for (int i = 0; i < t.Items.Count; i++)
                {
                    if (timetableid == t.Items[i].TimeTalbe)
                    {
                        xcount++;
                    }
                }
            }
            return xcount;
        }


        private string GetBandingTimeTableInfofirst(int timetableid)
        {
            int xcount = 0;
            foreach (var t in TreeItems)
            {
                for (int i = 0; i < t.Items.Count; i++)
                {
                    if (timetableid == t.Items[i].TimeTalbe)
                    {
                        return t.PhyId + "-" + t.RtuOrGrpName;
                    }
                }
            }
            return "无";
        }

    }


    /// <summary>
    /// Event
    /// </summary>
    public partial class TimeInfoNewViewModel
    {
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_rtu_orders,
                              ResponseSndWeek, typeof(TimeInfoNewViewModel), this, true);



        }

        private bool IsTimeTableSaveShowReport = false;


        private static bool flgModify = false;
        private void ResponseSndWeek(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isViewActive == false) return;

            long ack = 0;
            int i = -1;
            int index = 0;
            bool flag = false;


            var datax = infos.WstRtuOrders;
            if (datax == null) return;
            if (datax.Op == 12 || datax.Op == 11 || datax.Op == 13)
            {
                IsTimeTableSaveShowReport = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3302, 1, false);
                if (IsTimeTableSaveShowReport == false) return;
            }
        }


        private void InitEvent()
        {
            this.AddEventFilterInfo(
                       Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.AreaInfoChanged,
                       PublishEventType.Core, true);
            this.AddEventFilterInfo(EventIdAssign.TimeTimeRequest,
                                   PublishEventType.Core, true);
            this.AddEventFilterInfo(EventIdAssign.TimeTimeUpdate,
                                   PublishEventType.Core, true);
            this.AddEventFilterInfo(Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate,
                                   PublishEventType.Core, true);
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);

            if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.AreaInfoChanged
            && args.EventType == PublishEventType.Core)
            {
                getAreaRId();
                if (AreaName.Count > 0) AreaComboBoxSelected = AreaName[0];
                if (AreaName.Count > 1) Visi = Visibility.Visible;
                else Visi = Visibility.Collapsed;
            }


            if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate
            && args.EventType == PublishEventType.Core)
            {
                getAreaRId();
                if (AreaName.Count > 0) AreaComboBoxSelected = AreaName[0];
                if (AreaName.Count > 1) Visi = Visibility.Visible;
                else Visi = Visibility.Collapsed;
            }

            if (!_isViewActive) return;



            if (args.EventId == Wlst.Sr.TimeTableSystem.Services.IdServices.EventIdAssign.TimeTimeUpdate)
            {
                if (DateTime.Now.Ticks - dtSnd.Ticks < 100000000)
                {
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 保存成功.";
                }
                if (DateTime.Now.Ticks - dtSnd.Ticks < 600000000)
                {
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 保存成功.";
                }
                else
                {
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  数据更新. ";
                }


                if (isUserSelectGrp)
                {

                    this.TreeItems.Clear();

                    if (aareaId ==0 && ggrpId == 0)  //交叉分组
                    {
                        var tn = new TreeGrpNodes("地域分组", rrrtulst);

                        this.TreeItems.Add(tn);
                        return;
                    }
                    else
                    {
                        var tu = new Tuple<int, int>(aareaId, ggrpId);

                        if (Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu) == false) return;

                        var tn = new TreeGrpNodes(aareaId, ggrpId, true);

                        this.TreeItems.Add(tn);

                        return;

                    }

                }

                var areaold = AreaId;
                AreaName.Clear();
                getAreaRId();
                //if (AreaName.Count > 0) AreaComboBoxSelected = AreaName.First();
                foreach (var t in AreaName)
                {
                    if (t.Key == areaold)
                    {
                        AreaComboBoxSelected = t;
                        return;
                    }
                }

            }
            else
            {
                Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  数据更新. ";

                var areaold = AreaId;
                AreaName.Clear();
                getAreaRId();
                //if (AreaName.Count > 0) AreaComboBoxSelected = AreaName.First();
                foreach (var t in AreaName)
                {
                    if (t.Key == areaold)
                    {
                        AreaComboBoxSelected = t;
                        return;
                    }
                }
            }



        }

        public event EventHandler OnNavOnLoadSelectdRtus;

    }


}



