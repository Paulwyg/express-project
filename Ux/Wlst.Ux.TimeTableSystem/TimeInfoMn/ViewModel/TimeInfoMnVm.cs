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
//using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.TimeTableSystem.Models;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.Ux.TimeTableSystem.HolidayTimeSet.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Views;
using Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.ViewModels;
using Wlst.client;
using Color = System.Drawing.Color;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel
{
    [Export(typeof (IITimeInfoMn))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeInfoMnVm : VmEventActionProperyChangedBase,
                                        IITimeInfoMn
    {
        public TimeInfoMnVm()
        {
            Title = "时间表设置";
            InitAction();
            InitEvent();
            IsShowTimeTable = true;

        }

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
                        AreaName.Add(new AreaInt() {Value = t.ToString("d2") + "-" + area, Key = t});
                    }
                }
            }


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
                    if (value==null) return;
                    AreaId = value.Key;
                    areaId = value.Key;
                    LoadTimeTableInfoFromSr();
                    this.LoadRtuOrGrpBandingInfo();

                }
            }
        }

        private bool _isActive = false;

        public override void NavOnLoadr(params object[] parsObjects)
        {
            VisiSetup = File.Exists("Config\\西安.txt") ? Visibility.Visible : Visibility.Collapsed;

            _isActive = true;
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
                var areaid = (int)parsObjects[0];
                var grpid = (int)parsObjects[1];


                if (grpid > 1000000 && grpid < 1100000)
                {
                    var info = new OneGrpOrRtuLoopsSet(areaid, grpid, false);

                    if (AreaComboBoxSelected.Key != areaid)
                    {
                        foreach (var t in AreaName)
                        {
                            if (t.Key == areaid)
                            {
                                AreaComboBoxSelected = t;
                            }
                        }
                    }
                    if (OnUserWantSetGroupWeekSet != null)
                        OnUserWantSetGroupWeekSet(this, new EventArgsEx() { Info = info });
                }
                else if (grpid < 1000000 && grpid > 0)
                {

                    var info = new OneGrpOrRtuLoopsSet(areaid, grpid, true);

                    if (AreaComboBoxSelected.Key != areaid)
                    {
                        foreach (var t in AreaName)
                        {
                            if (t.Key == areaid)
                            {
                                AreaComboBoxSelected = t;
                            }
                        }
                    }

                    if (OnUserWantSetGroupWeekSet != null)
                        OnUserWantSetGroupWeekSet(this, new EventArgsEx() { Info = info });
                }
            }
        }



        public  event EventHandler<EventArgsEx> OnUserWantSetGroupWeekSet;


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

        private Visibility _visiSetup;

        public Visibility VisiSetup
        {
            get { return _visiSetup; }
            set
            {
                if (value != _visiSetup)
                {
                    _visiSetup = value;
                    this.RaisePropertyChanged(() => this.VisiSetup);
                }
            }
        }

        public override void OnUserHideOrClosingr()
        {

            _isActive = false;
            Items = new ObservableCollection<TimeTableInfomationItem>();
            ChildTreeItems = new ObservableCollection<OneGrpOrRtuLoopsSet>();
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

        private bool _isHideTimeTable;

        public bool IsShowTimeTable
        {
            get { return _isHideTimeTable; }
            set
            {
                if (value == _isHideTimeTable) return;
                _isHideTimeTable = value;
                RaisePropertyChanged(() => IsShowTimeTable);
            }
        }

        #region RecordSun

        public static ObservableCollection<SunItem> PassrecordSun1;
        private ObservableCollection<SunItem> _recordSun1;
        /// <summary>
        /// 段一开关灯时间
        /// </summary>
        public ObservableCollection<SunItem> RecordSun1
        {
            get
            {

                if (_recordSun1 == null)
                {
                    _recordSun1 = new ObservableCollection<SunItem>();
                    PassrecordSun1 = _recordSun1;
                }
                return _recordSun1;
            }
        }

        public static ObservableCollection<SunItem> PassrecordSun2;
        private ObservableCollection<SunItem> _recordSun2;
        /// <summary>
        /// 段二开关灯时间
        /// </summary>
        public ObservableCollection<SunItem> RecordSun2
        {
            get
            {

                if (_recordSun2 == null)
                {
                    _recordSun2 = new ObservableCollection<SunItem>();
                    PassrecordSun2 = _recordSun2;
                }
                return _recordSun2;
            }
        }

        public static ObservableCollection<SunItem> PassrecordSun3;
        private ObservableCollection<SunItem> _recordSun3;
        /// <summary>
        /// 段三开关灯时间
        /// </summary>
        public ObservableCollection<SunItem> RecordSun3
        {
            get
            {

                if (_recordSun3 == null)
                {
                    _recordSun3 = new ObservableCollection<SunItem>();
                    PassrecordSun3 = _recordSun3;
                }
                return _recordSun3;
            }
        }

        public static ObservableCollection<SunItem> PassrecordSun4;
        private ObservableCollection<SunItem> _recordSun4;
        /// <summary>
        /// 段四开关灯时间
        /// </summary>
        public ObservableCollection<SunItem> RecordSun4
        {
            get
            {

                if (_recordSun4 == null)
                {
                    _recordSun4 = new ObservableCollection<SunItem>();
                    PassrecordSun4 = _recordSun4;
                }
                return _recordSun4;
            }
        }
        #endregion
    }

    public partial class TimeInfoMnVm
    {

        

        #region load  RtuOrGrp

        //加载终端节点
        private void LoadRtuOrGrpBandingInfo()
        {
            ChildTreeItems.Clear();

            var ctig = new ObservableCollection<OneGrpOrRtuLoopsSet>();
            var ctit = new ObservableCollection<OneGrpOrRtuLoopsSet>();

            foreach (var t in Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.Keys)
            {
                if (t.Item1 == AreaId && t.Item2 > 0 ) ctig.Add(new OneGrpOrRtuLoopsSet(AreaId,t.Item2,true));
            }

            var lstg = (from a in ctig orderby a.PhyId select a).ToList();

            //加载无分组终端节点
            var tmp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            foreach (var f in tmp)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                {
                    var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[f];
                    if (para.EquipmentType == WjParaBase.EquType.Rtu)
                    {
                        ctit.Add(new OneGrpOrRtuLoopsSet(AreaId, f, false));
                    }
                }
            }

            var lstt = (from a in ctit orderby a.PhyId select a).ToList();

            foreach (var t in lstg) this.ChildTreeItems.Add(t);
            foreach (var t in lstt) this.ChildTreeItems.Add(t);
        }


      
        #endregion


        private ObservableCollection<OneGrpOrRtuLoopsSet> _childTreeItemsInfo;

        public ObservableCollection<OneGrpOrRtuLoopsSet> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<OneGrpOrRtuLoopsSet>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value == _childTreeItemsInfo) return;
                _childTreeItemsInfo = value;
                this.RaisePropertyChanged(() => ChildTreeItems);
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

                foreach (var g in ChildTreeItems)
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
            if (ChildTreeItems.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion


        public void UpdateNodeTimeTable(int rtuOrGrpId, int loop, int newTimeTableId, string newTimeTableName, string newTimeTimeDesc)
        {
            foreach (var f in ChildTreeItems)
            {
                if (f.RtuOrGrpId != rtuOrGrpId) continue;
                f.UpdateTimeInfo(loop, newTimeTableId, newTimeTableName, newTimeTimeDesc);
                break;
            }
        }


        private int GetBandingTimeTableInfo(int timetableid)
        {
            int xcount = 0;
            foreach (var t in ChildTreeItems)
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
            foreach (var t in ChildTreeItems)
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

    };

    /// <summary>
    /// 属性及其命令
    /// </summary>
    public partial class TimeInfoMnVm
    {


        private DateTime[] _dateTimes = new DateTime[4];


        private Views.BaseView.AddTimeTable _addTimeTable = null;

        #region CmdAddTimeTable

        //private int addid = new int();

        private ICommand _cmdAddTimeTable;

        public ICommand CmdAddTimeTable
        {
            get
            {
                return _cmdAddTimeTable ??
                       (_cmdAddTimeTable = new RelayCommand(ExCmdAddTimeTable, CanCmdAddTimeTable, true));
            }

        }

        private bool CanCmdAddTimeTable()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId)||Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) && DateTime.Now.Ticks - _dateTimes[0].Ticks > 30000000;
        }


        private void ExCmdAddTimeTable()
        {
            if (_addTimeTable != null)
            {
                try
                {
                    _addTimeTable.OnFormBtnOkClick -=
                        new EventHandler<Views.BaseView.EventArgsAddTimeTable>(_addTimeTable_OnFormBtnOkClick);
                }
                catch (Exception ex)
                {

                }
                _addTimeTable = null;
            }

            _dateTimes[0] = DateTime.Now;




            var weekTimeTable = new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem();

            var id = 0;
            foreach (var t in Items)
            {
                if (t.TimeId >= id)
                {
                    id = t.TimeId;
                }
            }
            weekTimeTable.TimeId = id+1;
            weekTimeTable.TimeName = "新建时间表" + weekTimeTable.TimeId;
            var dateOffirstSunDay = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))));

            weekTimeTable.LightOffOffset = this.Items.Count > 0 ? Items[0].LightOffOffset : -15;
            weekTimeTable.LightOnOffset = this.Items.Count > 0 ? Items[0].LightOnOffset : 15;
            weekTimeTable.LuxEffective = this.Items.Count > 0 ? Items[0].LuxEffective : 30;
            weekTimeTable.LuxOffValue = this.Items.Count > 0 ? Items[0].LuxOffValue : 15;
            weekTimeTable.LuxOnValue = this.Items.Count > 0 ? Items[0].LuxOnValue : 15;
            weekTimeTable.RuleItems =
                new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule>();
 
            for (int i = 0; i < 7; i++)
            {
                var tmp = new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule()
                              {
                                  DayOfWeekUsed = new List<int>(),
                                  DateEnd = 1231,
                                  DateStart = 101,
                                  RuleId = 1,
                                  RuleSectionId = 1,
                                  TimeOff = 1500,
                                  TimeOn = 1500,
                                  TimetableSectionId = 1,
                                  TypeOff = 3,
                                  TypeOn = 3
                              };
                tmp.DayOfWeekUsed.Add(i);
                weekTimeTable.RuleItems.Add(tmp);
            }

            var tvx = new TimeTableInfomationItem(weekTimeTable, AreaId);

       

            _addTimeTable = new AddTimeTable();
            _addTimeTable.OnFormBtnOkClick +=
                new EventHandler<Views.BaseView.EventArgsAddTimeTable>(_addTimeTable_OnFormBtnOkClick);
            _addTimeTable.SetContext(tvx, AreaId, weekTimeTable.TimeId);
            _addTimeTable.ShowDialog();

            //addid = addid + 1;
        }

        private void _addTimeTable_OnFormBtnOkClick(object sender, Views.BaseView.EventArgsAddTimeTable e) //todo 暂存
        {

            var updateinfo = e.AddTimeTableInfo;
            if (updateinfo == null) return;

            int max = 0;
            foreach (var t in updateinfo.RuleItems)
            {
                if (t.TimetableSectionId > max) max = t.TimetableSectionId;
            }
            updateinfo.MainRuleItems.Clear();
            //updateinfo.MainRuleItemsCalulate(max);
            updateinfo.MainRuleTermporaryItemsCalulate(max, updateinfo.TimeId);
            var isAdd = true;

            foreach (
                var item in Items.Where(item => item.TimeId  == updateinfo.TimeId))
            {
                isAdd = false;
                item.TimeName = updateinfo.TimeName;
                item.TimeDesc = updateinfo.TimeDesc;
                item.LuxOnValue = updateinfo.LuxOnValue;
                item.LuxOffValue = updateinfo.LuxOffValue;
                item.LuxName = updateinfo.LuxName;
                item.LuxId = updateinfo.LuxId;
                item.LuxEffective = updateinfo.LuxEffective;
                item.LightOnOffset = updateinfo.LightOnOffset;
                item.LightOffOffset = updateinfo.LightOffOffset;
                item.RuleItems = updateinfo.RuleItems;
                item.MainRuleItems = updateinfo.MainRuleItems;
                item.MainIsOverOne = updateinfo.MainIsOverOne;
                item.MainType = updateinfo.MainType;

                item.LuxId2 = updateinfo.LuxId2;
                item.LuxName2 = updateinfo.LuxName2;
                item.ShowCurrentSelectLux2 = updateinfo.ShowCurrentSelectLux2;
            }


            
            if (isAdd)
            {
                Items.Add(updateinfo);
            }

            try
            {
                _addTimeTable.OnFormBtnOkClick -=
                    new EventHandler<Views.BaseView.EventArgsAddTimeTable>(_addTimeTable_OnFormBtnOkClick);
            }
            catch (Exception ex)
            {

            }
            _addTimeTable = null;
        }

        #endregion

        #region CmdModifyTimeTable

        private ICommand _cmdModifyTimeTable;

        public ICommand CmdModifyTimeTable
        {
            get
            {
                return _cmdModifyTimeTable ??
                       (_cmdModifyTimeTable = new RelayCommand(ExCmdModifyTimeTable, CanCmdModifyTimeTable, true));
            }

        }

        private bool CanCmdModifyTimeTable()
        {

            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId) || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) && CurrentSelectItem != null && DateTime.Now.Ticks - _dateTimes[1].Ticks > 10000000;
        }

        private void ExCmdModifyTimeTable()
        {
            _dateTimes[1] = DateTime.Now;

            if (_addTimeTable != null)
            {
                try
                {
                    _addTimeTable.OnFormBtnOkClick -=
                        new EventHandler<Views.BaseView.EventArgsAddTimeTable>(_addTimeTable_OnFormBtnOkClick);
                }
                catch (Exception ex)
                {

                }
                _addTimeTable = null;
            }
            if (CurrentSelectItem == null) return;
            var ntg = CurrentSelectItem.BackToWeekTimeTableInforemation();

            //var ntgbak = ntg.RuleItems;
            //ntgbak.Clear();
            //foreach (var t in ntg.RuleItems)
            //{
            //    foreach (var tt in t.DayOfWeekUsed)
            //    {
            //        var additem = t;
            //        additem.DayOfWeekUsed = new List<int>(tt);
            //        ntgbak.Add(additem);
            //    }
            //}
            //ntg.RuleItems = ntgbak;

            var dic = new Dictionary<int, List<int>>();
            
            foreach (var t in ntg.RuleItems)
            {
                for (int i = 0; i < t.DayOfWeekUsed.Count; i++)
                {
                    if (dic.ContainsKey(t.TimetableSectionId)) dic[t.TimetableSectionId].Add(t.DayOfWeekUsed[i]);
                    else dic.Add(t.TimetableSectionId, new List<int>() { t.DayOfWeekUsed[i] });
                }
            }

            for (int i = 1; i < 5; i++)
            {
                if (dic.ContainsKey(i) == false) continue;
                var list = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
                var expectedList = list.Except(dic[i]);

                for (int j = 0; j < 7; j++)
                {
                    if (expectedList.Contains(j))
                    {
                        var tmp = new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule()
                        {
                            DayOfWeekUsed = new List<int>(),
                            DateEnd = 1231,
                            DateStart = 101,
                            RuleId = 1,
                            RuleSectionId = 1,
                            TimeOff = 1500,
                            TimeOn = 1500,
                            TimetableSectionId = i,
                            TypeOff = 3,
                            TypeOn = 3
                        };
                        tmp.DayOfWeekUsed.Add(j);
                        ntg.RuleItems.Add(tmp);
                    }
                }
            }


            var ruleitemslist = (from t in ntg.RuleItems select t).ToList();
            var ruleitemslistorder = (from t in ruleitemslist orderby t.DayOfWeekUsed.First(), t.TimetableSectionId select t).ToList();

            ntg.RuleItems.Clear();
            foreach (var t in ruleitemslistorder)
            {
                ntg.RuleItems.Add(t);
            }


            var tv = new TimeTableInfomationItem(ntg,AreaId );
             
            
            _addTimeTable = new AddTimeTable();
            _addTimeTable.OnFormBtnOkClick +=
                new EventHandler<Views.BaseView.EventArgsAddTimeTable>(_addTimeTable_OnFormBtnOkClick);
            _addTimeTable.SetContext(tv,AreaId,CurrentSelectItem.TimeId);
            _addTimeTable.ShowDialog();

            //todo
        }

        #endregion



        #region CmdCopyAddTimeTable

        private ICommand _cmdCopyAddTimeTable;

        public ICommand CmdCopyAddTimeTable
        {
            get
            {
                return _cmdCopyAddTimeTable ??
                       (_cmdCopyAddTimeTable = new RelayCommand(ExCopyAddTimeTable, CanCopyAddTimeTable, true));
            }

        }

        private bool CanCopyAddTimeTable()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId)||Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) && CurrentSelectItem != null && DateTime.Now.Ticks - _dateTimes[3].Ticks > 10000000;
        }

        private void ExCopyAddTimeTable()
        {
            _dateTimes[3] = DateTime.Now;

            if (_addTimeTable != null)
            {
                try
                {
                    _addTimeTable.OnFormBtnOkClick -=
                        new EventHandler<Views.BaseView.EventArgsAddTimeTable>(_addTimeTable_OnFormBtnOkClick);
                }
                catch (Exception ex)
                {

                }
                _addTimeTable = null;
            }
            if (CurrentSelectItem == null) return;
            var ntg = CurrentSelectItem.BackToWeekTimeTableInforemation();
            var tv = new TimeTableInfomationItem( ntg,AreaId );

            var id = 1;
            foreach (var t in Items)
            {
                if (t.TimeId  >= id)
                {
                    id = t.TimeId  + 1;
                }
            }
            tv.TimeId  = id;
            tv.TimeName  += " 复制增加时间表";
            foreach (var t in tv.RuleItems)
            {
                t.TimetableId = tv.TimeId;
            }

            _addTimeTable = new AddTimeTable();
            _addTimeTable.OnFormBtnOkClick +=
                new EventHandler<Views.BaseView.EventArgsAddTimeTable>(_addTimeTable_OnFormBtnOkClick);
            _addTimeTable.SetContext(tv,AreaId,CurrentSelectItem.TimeId);
            _addTimeTable.ShowDialog();
        }

        #endregion

        #region CmdDeleteTimeTable

        private ICommand _cmddeleteTimeTable;

        public ICommand CmdDeleteTimeTable
        {
            get
            {
                return _cmddeleteTimeTable ??
                       (_cmddeleteTimeTable = new RelayCommand(ExCmdDeleteTimeTable, CanCmdDeleteTimeTable, true));
            }

        }

        private bool CanCmdDeleteTimeTable()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId) || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) && DateTime.Now.Ticks - _dateTimes[2].Ticks > 10000000;
        }

        private void ExCmdDeleteTimeTable()
        {
            _dateTimes[2] = DateTime.Now;
            if (CurrentSelectItem == null) return;
            var xcount = GetBandingTimeTableInfo(CurrentSelectItem.TimeId );
            if (xcount > 0)
            {
          
                var xstromg = GetBandingTimeTableInfofirst(CurrentSelectItem.TimeId );
             

                // MessageBox.Show("无法执行删除", "至少存在" + lst.Count + "个终端正在使用该时间信息，请确认!!!", MessageBoxButton.OK);
                WlstMessageBox.Show("无法执行删除 " + xstromg,
                                    "存在至少" + xcount + " 设备输出正在使用该时间信息，无法执行删除! ", WlstMessageBoxType.Ok);
                return;
            }
            var info = WlstMessageBox.Show("确认删除",
                                           "确认删除。", WlstMessageBoxType.YesNo);
            if (info == WlstMessageBoxResults.Yes)
            {
                if (Items.Contains(CurrentSelectItem))
                {
                    this.Items.Remove(CurrentSelectItem);
                    if (this.Items.Count > 0) CurrentSelectItem = Items[0];
                }
            }
        }




        #endregion

        #region CmdSetup
        private DateTime _xdateTimes = DateTime.Now;
        private ICommand _cmdSetup;

        public ICommand CmdSetup
        {
            get
            {
                return _cmdSetup ??
                       (_cmdSetup = new RelayCommand(ExCmdSetup, CanCmdSetup, true));
            }

        }

        private bool CanCmdSetup()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId) || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) && CurrentSelectItem != null && DateTime.Now.Ticks - _xdateTimes.Ticks > 10000000;
        }

        private Views.BaseView.SetupPara _setupParaTable = null;

        private void ExCmdSetup()
        {
            //_xdateTimes = DateTime.Now;

            //if (CurrentSelectItem == null) return;
            //var ntg = CurrentSelectItem.BackToWeekTimeTableInforemation();

            ////var ntgbak = ntg.RuleItems;
            ////ntgbak.Clear();
            ////foreach (var t in ntg.RuleItems)
            ////{
            ////    foreach (var tt in t.DayOfWeekUsed)
            ////    {
            ////        var additem = t;
            ////        additem.DayOfWeekUsed = new List<int>(tt);
            ////        ntgbak.Add(additem);
            ////    }
            ////}
            ////ntg.RuleItems = ntgbak;

            //var dic = new Dictionary<int, List<int>>();

            //foreach (var t in ntg.RuleItems)
            //{
            //    for (int i = 0; i < t.DayOfWeekUsed.Count; i++)
            //    {
            //        if (dic.ContainsKey(t.TimetableSectionId)) dic[t.TimetableSectionId].Add(t.DayOfWeekUsed[i]);
            //        else dic.Add(t.TimetableSectionId, new List<int>() { t.DayOfWeekUsed[i] });
            //    }
            //}

            //for (int i = 1; i < 5; i++)
            //{
            //    if (dic.ContainsKey(i) == false) continue;
            //    var list = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
            //    var expectedList = list.Except(dic[i]);

            //    for (int j = 0; j < 7; j++)
            //    {
            //        if (expectedList.Contains(j))
            //        {
            //            var tmp = new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule()
            //            {
            //                DayOfWeekUsed = new List<int>(),
            //                DateEnd = 1231,
            //                DateStart = 101,
            //                RuleId = 1,
            //                RuleSectionId = 1,
            //                TimeOff = 1500,
            //                TimeOn = 1500,
            //                TimetableSectionId = i,
            //                TypeOff = 3,
            //                TypeOn = 3
            //            };
            //            tmp.DayOfWeekUsed.Add(j);
            //            ntg.RuleItems.Add(tmp);
            //        }
            //    }
            //}


            //var ruleitemslist = (from t in ntg.RuleItems select t).ToList();
            //var ruleitemslistorder = (from t in ruleitemslist orderby t.DayOfWeekUsed.First(), t.TimetableSectionId select t).ToList();

            //ntg.RuleItems.Clear();
            //foreach (var t in ruleitemslistorder)
            //{
            //    ntg.RuleItems.Add(t);
            //}


            //var tv = new TimeTableInfomationItem(ntg, AreaId);

            _setupParaTable = new SetupPara();
            _setupParaTable.OnFormBtnOkClick +=
                new EventHandler<Views.BaseView.EventArgsAddTimeTable>(_addTimeTable_OnFormBtnOkClick);
            _setupParaTable.SetContext(Items, AreaId, CurrentSelectItem.TimeId);
            _setupParaTable.ShowDialog();
        }




        #endregion

        public static ObservableCollection<OneItemTemporary> PassTemporaryItem=new ObservableCollection<OneItemTemporary>();
        public static List<int> tt = new List<int>();
        private TimeTabletemporaryView.Views.TimeTabletemporaryView _temporaryTimeTable = null;

        /// <summary>
        /// 临时时间方案
        /// </summary>
        #region CmdTemporaryTimeTable
        private DateTime _dtCmdTemporaryTimeTable;
        private ICommand _cmdTemporaryTimeTable;
        public ICommand CmdTemporaryTimeTable
        {
            get
            {
                return _cmdTemporaryTimeTable ??
                       (_cmdTemporaryTimeTable = new RelayCommand(ExCmdTemporaryTimeTable, CanCmdTemporaryTimeTable, true));
            }
        }

        private bool CanCmdTemporaryTimeTable()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId) || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) && DateTime.Now.Ticks - _dtCmdTemporaryTimeTable.Ticks > 30000000;
        }

        private void ExCmdTemporaryTimeTable()
        {
            LoadTemporaryScheme(AreaId);
            _dtCmdTemporaryTimeTable = DateTime.Now;
            _temporaryTimeTable = new TimeTabletemporaryView.Views.TimeTabletemporaryView();
            //_temporaryTimeTable.SetContext();
            _temporaryTimeTable.ShowDialog();
        }

        /// <summary>
        /// 加载临时时间表方案
        /// </summary>
        /// <param name="areaid"></param>
        private void LoadTemporaryScheme(int areaid)
        {
            var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_temp_time_plan;
            info.WstRtutimeTempTimePlan.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 2);
            tt.Clear();
            PassTemporaryItem.Clear();
            var xxx = new ObservableCollection<OneItemTemporary>();
            var plans = (from t in TimeTabletemporaryHold.Myself.Info
                         where t.Key.Item1 == areaid
                         orderby t.Key.Item2 ascending
                         select t.Value).ToList();
            foreach (var t in plans)
            {
                xxx.Add(new OneItemTemporary(t, AreaId));
            }
            if (xxx.Count != 0)
            {
                PassTemporaryItem = xxx;
                foreach (var t in PassTemporaryItem)
                {
                    tt.Add(t.SchemeId);
                }
            }

        }
        #endregion

        /// <summary>
        /// 全年时间查看
        /// </summary>
        private RecordSunView _recordSunView = null;
        #region CmdQueryRecordSun
        private DateTime _dtCmdQueryRecordSun;
        private ICommand _cmdQueryRecordSun;
        public ICommand CmdQueryRecordSun
        {
            get
            {
                return _cmdQueryRecordSun ??
                       (_cmdQueryRecordSun = new RelayCommand(ExCmdQueryRecordSun, CanCmdQueryRecordSun, true));
            }
        }

        private bool CanCmdQueryRecordSun()
        {
            return (UserInfo.UserLoginInfo.AreaW.Contains(AreaId) || UserInfo.UserLoginInfo.D) && DateTime.Now.Ticks - _dtCmdQueryRecordSun.Ticks > 30000000;
        }

        private void ExCmdQueryRecordSun()
        {
            _dtCmdQueryRecordSun = DateTime.Now;
            _recordSunView=new RecordSunView();
            //InitSun();
            RecordSun1.Clear();
            RecordSun2.Clear();
            RecordSun3.Clear();
            RecordSun4.Clear();
            var plans = (from tt in TimeTabletemporaryHold.Myself.Info
                         where tt.Key.Item1 == AreaId
                         orderby tt.Key.Item2 ascending
                         select tt.Value).ToList();
            var dic = new Dictionary<Tuple<int,long>, NameValueInt>();
            foreach (var t in plans)
            {
                foreach (var f in t.TimetablesUseThisPlan)
                {
                    if (f == CurrentSelectItem.TimeId)
                    {
                        foreach (var ttt in t.ItemsPlan)
                        {
                            var records = new NameValueInt();
                            string on = "--:--";
                            string off = "--:--";
 
                            if(ttt.TimeOff!=1500)
                            {
                                off = (ttt.TimeOff/60).ToString("D2") + ":" + (ttt.TimeOff%60).ToString("D2");
                            }
                            if(ttt.TypeOn!=1500)
                            {
                                on = (ttt.TimeOn / 60).ToString("D2") + ":" + (ttt.TimeOn % 60).ToString("D2");
                            }
                            records = new NameValueInt()
                                          {
                                              Name = on + " - " + off,
                                              Color = new SolidColorBrush(Colors.Red)
                                          };
                            dic.Add(new Tuple<int, long>(ttt.SectionId, ttt.Date), records);
                        }
                        break;
                    }
                }
            }
            for (int j = 1; j < 32; j++)
            {
                var tmp1 = new SunItem();
                var tmp2 = new SunItem();
                var tmp3 = new SunItem();
                var tmp4 = new SunItem();
                tmp1.Records.Add(new NameValueInt() { Name = j + " " });
                tmp2.Records.Add(new NameValueInt() { Name = j + " " });
                tmp3.Records.Add(new NameValueInt() { Name = j + " " });
                tmp4.Records.Add(new NameValueInt() { Name = j + " " });
                for (int i = 1; i < 13; i++)
                {

                    if ((DateTime.Now.Year % 4 == 0 && DateTime.Now.Year % 100 != 0) || DateTime.Now.Year % 400 == 0)
                    {
                    }
                    else
                    {
                        if (j == 29 && i == 2)
                        {
                            tmp1.Records.Add(new NameValueInt()
                                                 {
                                                     Name = "--:-- - --:--"
                                                 });
                            tmp2.Records.Add(new NameValueInt()
                                                 {
                                                     Name = "--:-- - --:--"
                                                 });
                            tmp3.Records.Add(new NameValueInt()
                                                 {
                                                     Name = "--:-- - --:--"
                                                 });
                            tmp4.Records.Add(new NameValueInt()
                                                 {
                                                     Name = "--:-- - --:--"
                                                 });
                            continue;
                        }
                    }
                    var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                        i, j);
                    if (info != null)
                    {
                        var week = new DateTime(DateTime.Now.Year, i, j).DayOfWeek.ToString();
                        long time = DateTime.Now.Year*10000 + i*100 + j;
                        foreach (var t in CurrentSelectItem.RuleItems)
                        {
                            if (t.TimetableSectionId == 1)
                            {
                                if (dic.ContainsKey(new Tuple<int, long>(1, time)))
                                {
                                    tmp1.Records.Add(dic[new Tuple<int, long>(1, time)]);
                                    break;
                                }
                                if (t.DayOfWeekUsed == Week(week))
                                {
                                    CountTime(info, tmp1, t, week);
                                    break;
                                }
                            }
                        }
                        foreach (var t in CurrentSelectItem.RuleItems)
                        {
                            if (t.TimetableSectionId == 2)
                            {
                                if (dic.ContainsKey(new Tuple<int, long>(2, time)))
                                {
                                    tmp2.Records.Add(dic[new Tuple<int, long>(2, time)]);
                                    break;
                                }
                                if (t.DayOfWeekUsed == Week(week))
                                {
                                    CountTime(info, tmp2, t, week);
                                    break;
                                }
                            }
                        }
                        foreach (var t in CurrentSelectItem.RuleItems)
                        {
                            if (t.TimetableSectionId == 3)
                            {
                                if (dic.ContainsKey(new Tuple<int, long>(3, time)))
                                {
                                    tmp3.Records.Add(dic[new Tuple<int, long>(3, time)]);
                                    break;
                                }
                                if (t.DayOfWeekUsed == Week(week))
                                {
                                    CountTime(info, tmp3, t, week);
                                    break;
                                }
                            }
                        }
                        foreach (var t in CurrentSelectItem.RuleItems)
                        {
                            if (t.TimetableSectionId == 4)
                            {
                                if (dic.ContainsKey(new Tuple<int, long>(1, time)))
                                    tmp4.Records.Add(dic[new Tuple<int, long>(4, time)]);
                                if (t.DayOfWeekUsed == Week(week))
                                {
                                    CountTime(info, tmp4, t, week);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        tmp1.Records.Add(new NameValueInt()
                                             {
                                                 Name = "--:-- - --:--"
                                             });
                        tmp2.Records.Add(new NameValueInt()
                                             {
                                                 Name = "--:-- - --:--"
                                             });
                        tmp3.Records.Add(new NameValueInt()
                                             {
                                                 Name = "--:-- - --:--"
                                             });
                        tmp4.Records.Add(new NameValueInt()
                                             {
                                                 Name = "--:-- - --:--"
                                             });
                    }
                }
                RecordSun1.Add(tmp1);
                RecordSun2.Add(tmp2);
                RecordSun3.Add(tmp3);
                RecordSun4.Add(tmp4);
            }
            var tvx = new OpenOrClose(RecordSun1, RecordSun2, RecordSun3, RecordSun4, CurrentSelectItem.TimeName);
            _recordSunView.SetContext(tvx);
            _recordSunView.ShowDialog();
        }

        public int Week(string weekName)
        {
            int week = 0;
            switch (weekName)
            {
                case "Sunday":
                    week = 0;
                    break;
                case "Monday":
                    week = 1;
                    break;
                case "Tuesday":
                    week = 2;
                    break;
                case "Wednesday":
                    week = 3;
                    break;
                case "Thursday":
                    week = 4;
                    break;
                case "Friday":
                    week = 5;
                    break;
                case "Saturday":
                    week = 6;
                    break;
            }
            return week;
        }

        public void CountTime(SunRiseItemInfomation info, SunItem tmp, TimeTableOneDayInfomationItem t, string week)
        {
                string on = "--:--";
                if (t.TimeOn != 1500)
                {
                    if (t.IsTimeOnEnable)
                    {
                        on = (t.TimeOn / 60).ToString("D2") + ":" + (t.TimeOn % 60).ToString("D2");
                    }
                    else
                    {
                        on = ((info.time_sunset + CurrentSelectItem.LightOnOffset) / 60).ToString("D2") +
                             ":" +
                             ((info.time_sunset + CurrentSelectItem.LightOnOffset) % 60).ToString("D2");
                    }
                }

                string off = "--:--";
                if (t.TimeOff != 1500)
                {
                    if (t.IsTimeOffEnable)
                    {
                        off = (t.TimeOff / 60).ToString("D2") + ":" + (t.TimeOff % 60).ToString("D2");
                    }
                    else
                    {
                        off =
                            ((info.time_sunrise + CurrentSelectItem.LightOffOffset) / 60).ToString("D2") +
                            ":" +
                            ((info.time_sunrise + CurrentSelectItem.LightOffOffset) % 60).ToString("D2");
                    }
                }
                tmp.Records.Add(new NameValueInt()
                {
                    Name =
                        on + " - " + off

                });
        }
        #endregion
        #region CmdSetupPara

        private ICommand _cmdSetupPara;

        public ICommand CmdSetupPara
        {
            get
            {
                return _cmdSetupPara ??
                       (_cmdSetupPara = new RelayCommand(ExCmdSetupPara, CanCmdSetupPara, true));
            }

        }

        private bool CanCmdSetupPara()
        {
            return true;
        }



        private void ExCmdSetupPara()
        {
            foreach (var m in Items)
            {
                var ntg = m.BackToWeekTimeTableInforemation();

                var dic = new Dictionary<int, List<int>>();

                foreach (var t in ntg.RuleItems)
                {
                    for (int i = 0; i < t.DayOfWeekUsed.Count; i++)
                    {
                        if (dic.ContainsKey(t.TimetableSectionId)) dic[t.TimetableSectionId].Add(t.DayOfWeekUsed[i]);
                        else dic.Add(t.TimetableSectionId, new List<int>() { t.DayOfWeekUsed[i] });
                    }
                }

                for (int i = 1; i < 5; i++)
                {
                    if (dic.ContainsKey(i) == false) continue;
                    var list = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
                    var expectedList = list.Except(dic[i]);

                    for (int j = 0; j < 7; j++)
                    {
                        if (expectedList.Contains(j))
                        {
                            var tmp = new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule()
                                          {
                                              DayOfWeekUsed = new List<int>(),
                                              DateEnd = 1231,
                                              DateStart = 101,
                                              RuleId = 1,
                                              RuleSectionId = 1,
                                              TimeOff = 1500,
                                              TimeOn = 1500,
                                              TimetableSectionId = i,
                                              TypeOff = 3,
                                              TypeOn = 3
                                          };
                            tmp.DayOfWeekUsed.Add(j);
                            ntg.RuleItems.Add(tmp);
                        }
                    }
                }


                var ruleitemslist = (from t in ntg.RuleItems select t).ToList();
                var ruleitemslistorder =
                    (from t in ruleitemslist orderby t.DayOfWeekUsed.First(), t.TimetableSectionId select t).ToList();

                ntg.RuleItems.Clear();
                foreach (var t in ruleitemslistorder)
                {
                    ntg.RuleItems.Add(t);
                }


                var tv = new TimeTableInfomationItem(ntg, AreaId);
            }
        }




        #endregion

        #region Attribute


        private TimeTableInfomationItem _currentSelectItem = null;

        public TimeTableInfomationItem CurrentSelectItem
        {
            get { return _currentSelectItem; }
            set
            {
                if (_currentSelectItem == value || value == null) return;
                _currentSelectItem = value;
                if (_currentSelectItem.LuxId2 == 0 ) _currentSelectItem.ShowCurrentSelectLux2 = 0;
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
                Items.Add(new TimeTableInfomationItem(itemTable,AreaId ));
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

    public class SunItem : ObservableObject
    {
        private ObservableCollection<NameValueInt> _records;

        public ObservableCollection<NameValueInt> Records
        {
            get
            {
                if (_records == null)
                    _records = new ObservableCollection<NameValueInt>();
                return _records;
            }
        }

    }

    public class NameValueInt : ObservableObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }



        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.RaisePropertyChanged(() => this.Value);
                }
            }
        }

        private int _value2;

        public int Value2
        {
            get { return _value2; }
            set
            {
                if (_value2 != value)
                {
                    _value2 = value;
                    this.RaisePropertyChanged(() => this.Value2);
                }
            }
        }

        private SolidColorBrush _color = new SolidColorBrush(Colors.Black);

        public SolidColorBrush Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    this.RaisePropertyChanged(() => this.Color);
                }
            }
        }
    }

    public class OpenOrClose:Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public OpenOrClose(ObservableCollection<SunItem> dt1, ObservableCollection<SunItem> dt2, ObservableCollection<SunItem> dt3, ObservableCollection<SunItem> dt4, string name)
        {
            Item1 = dt1;
            Item2 = dt2;
            Item3 = dt3;
            Item4 = dt4;
            IsVisible2 = dt2[0].Records.Count <= 1 ? Visibility.Collapsed : Visibility.Visible;
            IsVisible3 = dt3[0].Records.Count <= 1 ? Visibility.Collapsed : Visibility.Visible;
            IsVisible4 = dt4[0].Records.Count <= 1 ? Visibility.Collapsed : Visibility.Visible;
            TableName = dt2[0].Records.Count <= 1 ? "操作时间" : "段1操作时间";
            WindowName = name + " - 全年开关灯最后时间";
        }

        #region Item

        private ObservableCollection<SunItem> _item1;

        public ObservableCollection<SunItem> Item1
        {
            get { return _item1 ?? (_item1 = new ObservableCollection<SunItem>()); }
            set
            {
                if (value != _item1)
                {
                    _item1 = value;
                    this.RaisePropertyChanged(() => this.Item1);
                }
            }
        }

        private ObservableCollection<SunItem> _item2;

        public ObservableCollection<SunItem> Item2
        {
            get { return _item2 ?? (_item2 = new ObservableCollection<SunItem>()); }
            set
            {
                if (value != _item2)
                {
                    _item2 = value;
                    this.RaisePropertyChanged(() => this.Item2);
                }
            }
        }

        private ObservableCollection<SunItem> _item3;

        public ObservableCollection<SunItem> Item3
        {
            get { return _item3 ?? (_item3 = new ObservableCollection<SunItem>()); }
            set
            {
                if (value != _item3)
                {
                    _item3 = value;
                    this.RaisePropertyChanged(() => this.Item3);
                }
            }
        }

        private ObservableCollection<SunItem> _item4;

        public ObservableCollection<SunItem> Item4
        {
            get { return _item4 ?? (_item4 = new ObservableCollection<SunItem>()); }
            set
            {
                if (value != _item4)
                {
                    _item4 = value;
                    this.RaisePropertyChanged(() => this.Item4);
                }
            }
        }
        #endregion

        #region IsVisible
        private Visibility _isVisible2 = Visibility.Hidden;
        /// <summary>
        /// 
        /// </summary>
        public Visibility IsVisible2
        {
            get
            {
                return _isVisible2;
            }
            set
            {
                if (value == _isVisible2) return;
                _isVisible2 = value;
                RaisePropertyChanged(() => IsVisible2);
            }
        }

        private Visibility _isVisible3 = Visibility.Hidden;
        /// <summary>
        /// 
        /// </summary>
        public Visibility IsVisible3
        {
            get
            {
                return _isVisible3;
            }
            set
            {
                if (value == _isVisible3) return;
                _isVisible3 = value;
                RaisePropertyChanged(() => IsVisible3);
            }
        }

        private Visibility _isVisible4 = Visibility.Hidden;
        /// <summary>
        /// 
        /// </summary>
        public Visibility IsVisible4
        {
            get
            {
                return _isVisible4;
            }
            set
            {
                if (value == _isVisible4) return;
                _isVisible4 = value;
                RaisePropertyChanged(() => IsVisible4);
            }
        }
        #endregion

        #region TableName
        private string _tableName;

        public string TableName
        {
            get { return _tableName; }
            set
            {
                if (_tableName != value)
                {
                    _tableName = value;
                    this.RaisePropertyChanged(() => this.TableName);
                }
            }
        }
        #endregion

        #region WindowName
        private string _windowName;

        public string WindowName
        {
            get { return _windowName; }
            set
            {
                if (_windowName != value)
                {
                    _windowName = value;
                    this.RaisePropertyChanged(() => this.WindowName);
                }
            }
        }
        #endregion
    }

    //save
    public partial class TimeInfoMnVm
    {

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

            var lst = new List<Tuple<int, int, int>>();
            foreach (var t in ChildTreeItems)
            {
                for (int i = 0; i < t.Items.Count; i++)
                {
                    if (t.Items[i].TimeTalbe > 0)
                    {
                        lst.Add(new Tuple<int, int, int>(t.RtuOrGrpId, i + 1, t.Items[i].TimeTalbe));
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
            

            if (rtn.Count == 0)
            {
                Msg = "无任何时间表信息，无法执行保存...";
                return;
            }

            foreach (var t in lst)
            {
                var isgrp = new bool();
                if (t.Item1 > 999999)
                {
                    isgrp = false;
                }
                else isgrp = true;
                var rtuitem = new OneGrpOrRtuLoopsSet(AreaId, t.Item1,isgrp);
                var timetableitem = new TimeTableInfomationItem();

                if (rtn.ContainsKey(t.Item3))
                {
                    timetableitem = new TimeTableInfomationItem(rtn[t.Item3],AreaId);
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
            Sr.TimeTableSystem.Services.WeekTimeTableInfoService.UpdateTimeTable(AreaId,rtn1, lst);

            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存 ...";
        }

        public static Views.BaseView.SetWeekAck SetWeekAck = null;

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



            var info = Sr.ProtocolPhone.LxRtu.wst_rtu_orders; //.wlst_cnt_request_wj3090_measure;
            info.Args.Addr.AddRange(_lstRtu);
            info.WstRtuOrders.RtuIds.AddRange(_lstRtu);
            info.WstRtuOrders.DtTime = DateTime.Now.Ticks;
            info.WstRtuOrders.Op = 11;
            SndOrderServer.OrderSnd(info, 10, 6);
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
                if(t.WeekAck != 3)
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

                    if(flgExist == false)
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

        #region CmdSaveNew

        private ICommand _cmdSaveNew;

        public ICommand CmdSaveNew
        {
            get { return _cmdSave ?? (_cmdSave = new RelayCommand(ExNew, CanExNewSave, true)); }
        }

        private bool CanExNewSave()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId) || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) && DateTime.Now.Ticks - dtSnd.Ticks > 30000000;
        }

        private void ExNew()
        {
            dtSnd = DateTime.Now;


            var rtn = new Dictionary<int, TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
            var rtn1 = new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
            foreach (var f in Items)
            {
                var lst1 = f.BackToWeekTimeTableInforemation();
                rtn.Add(f.TimeId, lst1);
                rtn1.Add(lst1);
            }


            if (rtn.Count == 0)
            {
                Msg = "无任何时间表信息，无法执行保存...";
                return;
            }



            var infoss = WlstMessageBox.Show("确认保存", "即将保存信息，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;

            dtSnd = DateTime.Now;
            Sr.TimeTableSystem.Services.WeekTimeTableInfoService.UpdateTimeTableNew(AreaId, rtn1);

            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存 ...";
        }




        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class TimeInfoMnVm
    {
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_rtu_orders,
                              ResponseSndWeek, typeof(TimeInfoMnVm), this, true);
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtuTime.wst_timetable_set,
                              ExSaveWeek, typeof(TimeInfoMnVm), this,true);
            //ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtuTime.wst_timetable_set_new,
            //                  ExSaveWeekNew, typeof(TimeInfoMnVm), this, true);


        }

        private bool IsTimeTableSaveShowReport = false;
        private void ExSaveWeek(string session, Wlst.mobile.MsgWithMobile infos)
        {
            IsTimeTableSaveShowReport = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3302, 1, false);
            if (IsTimeTableSaveShowReport == false) return;
            var data = infos.Args.Addr;

            AllWeekSndReport.Clear();
            //var index = -1;
            foreach (var i in data)
            {
                //index++;
                var xx = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(i);
                if (xx != null)
                {
                    if ((xx.RtuModel == EnumRtuModel.Wj3005 || xx.RtuModel == EnumRtuModel.Wj3006 ||
                         xx.RtuModel == EnumRtuModel.Wj4005 ||
                         xx.RtuModel == EnumRtuModel.Gz6005) && xx.RtuStateCode != 0)
                    {
                        var state = "不用";
                        if (xx.RtuStateCode == 1)
                        {
                            state = "停运";
                        }
                        else if (xx.RtuStateCode == 2)
                        {
                            state = "使用";
                        }
                        var m = new WeekSndReport(i, xx.RtuPhyId, xx.RtuName, state, 0);


                        //AllWeekSndReport.Insert(index, m);

                        AllWeekSndReport.Add(m);
                    }
                }
            }
            var xxxx = new WeekReport { xWeekSndReport = CurrentWeekSndReport = AllWeekSndReport, 
                OcCount = AllWeekSndReport.Count(), OcTmlCount = AllWeekSndReport.Count() - (GetAnsWeekReport(AllWeekSndReport)).Count };

            if (AllWeekSndReport.Count != 0 )
            {
                if (SetWeekAck != null)
                {
                    SetWeekAck.Close();
                }

                SetWeekAck = new SetWeekAck();
                SetWeekAck.SetContext(xxxx);
                SetWeekAck.Show();
            }


        }

        private static bool flgModify = false;
        private void ResponseSndWeek(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_isActive == false) return;

            long ack = 0;
            int i = -1;
            int index = 0;
            bool flag = false;


            var datax = infos.WstRtuOrders;
            if (datax == null) return;
            if (datax.Op == 12 ||datax.Op ==11 ||datax.Op ==13)
            {
                IsTimeTableSaveShowReport = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3302, 1, false);
                if (IsTimeTableSaveShowReport == false) return;

                var m = new WeekSndReport();

                foreach (WeekSndReport t in AllWeekSndReport)
                {
                    i++;

                    foreach (var x in infos.Args.Addr)
                    {
                        if (x == t.RtuId)
                        {
                            ack = t.WeekAck;

                            if (datax.Op == 11)
                            {
                                if ((ack & 0x00000001) == 0)
                                {
                                    ack += 1;
                                }
                            }
                            else if (datax.Op == 12)
                            {
                                if ((ack & 0x00000002) == 0)
                                {
                                    ack += 2;
                                }
                            }
                            else if (datax.Op == 13)
                            {
                                if ((ack & 0x00000003) == 0)
                                {
                                    ack += 3;
                                }
                            }

                            flag = true;
                            index = i;

                            m = new WeekSndReport(t.RtuId, t.PhysicalId, t.NodeName, t.State, ack);
                        }
                    }




                }

                if (flag == true)
                {
                    flgModify = true;

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

                    var xxxx = new WeekReport { xWeekSndReport = CurrentWeekSndReport, OcCount = AllWeekSndReport.Count(), OcTmlCount = AllWeekSndReport.Count() - (GetAnsWeekReport(AllWeekSndReport)).Count };
                    SetWeekAck.SetContext(xxxx);

                    flgModify = false;


                }
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

        public override void ExPublishedEvent(
             PublishEventArgs args)
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

            if (!_isActive) return;



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


    
        }
}
