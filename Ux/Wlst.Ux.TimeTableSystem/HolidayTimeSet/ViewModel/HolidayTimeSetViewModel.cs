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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
//using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.TimeTableSystem.HolidayTimeSet.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.client;


namespace Wlst.Ux.TimeTableSystem.HolidayTimeSet.ViewModel
{

    [Export(typeof(IIHolidayTimeSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class HolidayTimeSetViewModel : VmEventActionProperyChangedBase, IIHolidayTimeSet
    {

        public HolidayTimeSetViewModel()
        {
            Title = "节假日时间设置";
            this.AddEventFilterInfo(
                Sr.TimeTableSystem.Services.IdServices.EventIdAssign.TimeHolidayTimeSchduleAndRtuBandingChanged,
                PublishEventType.Core);
            this.AddEventFilterInfo(
                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.AreaInfoChanged,
                PublishEventType.Core, true);    
            this.AddEventFilterInfo(
                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate,
                PublishEventType.Core, true);    
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
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
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
                    this.Load();
                }
            }
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
            if(args .EventId ==Sr .TimeTableSystem .Services .IdServices .EventIdAssign .TimeHolidayTimeSchduleAndRtuBandingChanged
                && args.EventType == PublishEventType.Core)
            {
                this.Load();
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 节假日数据更新.";
            }
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
        }


        protected Dictionary<int, int> RtuBandingTimeSchdule = new Dictionary<int,  int>();

        public static int AreaId = new int();

        private ObservableCollection<ListTreeNodeBase> _childTreeItemsInfo;

        /// <summary>
        /// 终端树
        /// </summary>
        public ObservableCollection<ListTreeNodeBase> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                {
                    _childTreeItemsInfo = new ObservableCollection<ListTreeNodeBase>();
                }
                return _childTreeItemsInfo;
            }
            set
            {
                if (value == _childTreeItemsInfo) return;
                _childTreeItemsInfo = value;
                this.RaisePropertyChanged(() => ChildTreeItems);
            }
        }



        private ObservableCollection<TimeSchduleItem> _timeSchdules;

        /// <summary>
        /// 节假日时间调度方案集合  
        /// </summary>
        public ObservableCollection<TimeSchduleItem> TimeSchdules
        {
            get
            {
                if (_timeSchdules == null)
                {
                    _timeSchdules = new ObservableCollection<TimeSchduleItem>();
                }
                return _timeSchdules;
            }
            set
            {
                if (value == _timeSchdules) return;
                _timeSchdules = value;
                this.RaisePropertyChanged(() => TimeSchdules);
            }
        }


        private bool _istreeeenable;
        public bool IsTreeEnable
        {
            get { return _istreeeenable; }
            set
            {
                if (value == _istreeeenable) return;
                _istreeeenable = value;
                this.RaisePropertyChanged(() => this.IsTreeEnable);
            }
        }
        private bool _istextable;
        public bool IsTextEnable
        {
            get { return _istextable; }
            set
            {
                if (value == _istextable) return;
                _istextable = value;
                this.RaisePropertyChanged(() => this.IsTextEnable);
            }
        }

          private string  _remind;

        public string Remind
        {
            get { return _remind; }
            set
            {
                if (value == _remind) return;
                _remind = value;
                this.RaisePropertyChanged(() => this.Remind);
            }
        }


        private TimeSchduleItem _currentSelectTimeSchduleItem;

        public TimeSchduleItem CurrentSelectTimeSchduleItem
        {
            get { return _currentSelectTimeSchduleItem; }
            set
            {
                if (value == _currentSelectTimeSchduleItem) return;
               
                if (_currentSelectTimeSchduleItem != null)
                {
                    //勾选的终端
                    var lst = (from t in TreeNodeRtu.Info where t.Value.IsSelected select t.Key).ToList();

                    var kes =
                        (from t in RtuBandingTimeSchdule where t.Value == _currentSelectTimeSchduleItem.Id select t.Key)
                            .ToList();
                    //清楚先前勾选了的 但这此取消了
                    foreach (var t in kes)
                    {
                        if (!lst.Contains(t))
                        {
                            RtuBandingTimeSchdule.Remove(t);
                            if (TreeNodeRtu.Info.ContainsKey(t)) TreeNodeRtu.Info[t].SchemeName = "--";

                        }
                    }


                    //勾选
                    foreach (var t in lst)
                    {
                        if (RtuBandingTimeSchdule.ContainsKey(t))
                        {

                            RtuBandingTimeSchdule[t] = _currentSelectTimeSchduleItem.Id;

                            if (TreeNodeRtu.Info.ContainsKey(t)) TreeNodeRtu.Info[t].SchemeName = _currentSelectTimeSchduleItem.Name;
                        }
                        else
                        {
                            RtuBandingTimeSchdule.Add(t, _currentSelectTimeSchduleItem.Id);
                            if (TreeNodeRtu.Info.ContainsKey(t)) TreeNodeRtu.Info[t].SchemeName = _currentSelectTimeSchduleItem.Name;
                        }
                    }


                }

                _currentSelectTimeSchduleItem = value;
                this.RaisePropertyChanged(() => this.CurrentSelectTimeSchduleItem);

                if (value == null) CurrentSelectTimeSchduleItemItem = null;
                else if (value.Schdules.Count > 0) CurrentSelectTimeSchduleItemItem = value.Schdules[0];
                else CurrentSelectTimeSchduleItemItem = null;

                if (value == null) SetRtuSelectedBanding(new List<int>());
                else
                {
                    var lst =
                        (from t in RtuBandingTimeSchdule where t.Value > 0 && t.Value == value.Id select t.Key).ToList();
                    SetRtuSelectedBanding(lst);
                }

                foreach (var t in TimeSchdules)
                {
                    t.Count = (from g in RtuBandingTimeSchdule where g.Value == t.Id select t).Count();
                }


                IsTreeEnable = _currentSelectTimeSchduleItem != null;
            }
        }

        private TimeSchduleItemItme _currentSelectTimeSchduleItemItem;

        public TimeSchduleItemItme CurrentSelectTimeSchduleItemItem
        {
            get { return _currentSelectTimeSchduleItemItem; }
            set
            {
                if (value == _currentSelectTimeSchduleItemItem) return;
                _currentSelectTimeSchduleItemItem = value;
                this.RaisePropertyChanged(() => this.CurrentSelectTimeSchduleItemItem);
                IsTextEnable = _currentSelectTimeSchduleItemItem != null;
            }
        }

        #region 对方案 进行删除 增加

        #region CmdExDeleteSchdule

        private ICommand _cCmdExDeleteSchdule;

        public ICommand CmdExDeleteSchdule
        {
            get
            {
                if (_cCmdExDeleteSchdule == null)
                    _cCmdExDeleteSchdule = new RelayCommand(ExCmdExDeleteSchdule, CanCmdExDeleteSchdule, true);
                return _cCmdExDeleteSchdule;
            }
        }

        private void ExCmdExDeleteSchdule()
        {
            int schid = CurrentSelectTimeSchduleItem.Id;

            var keys = (from t in RtuBandingTimeSchdule where t.Value == schid select t.Key).ToList();
            foreach (var t in keys)
            {
                RtuBandingTimeSchdule.Remove(t);
                if (TreeNodeRtu.Info.ContainsKey(t))
                {
                    TreeNodeRtu.Info[t].SchemeName = "--";
                    TreeNodeRtu.Info[t].IsSelected = false;

                }
            }


            foreach (var t in TimeSchdules)
                if (t.Id == CurrentSelectTimeSchduleItem.Id)
                {
                    {
                        TimeSchdules.Remove(t);
                        _currentSelectTimeSchduleItem = null;
                        CurrentSelectTimeSchduleItem = TimeSchdules.Count > 0 ? TimeSchdules[0] : null;
                        break;
                    }
                }

            _dtExDeleteSchdule = DateTime.Now;
        }

        private DateTime _dtExDeleteSchdule;
        private bool CanCmdExDeleteSchdule()
        {
            return CurrentSelectTimeSchduleItem != null && DateTime.Now.Ticks - _dtExDeleteSchdule.Ticks > 10000000; ;
        }
       

     
        #endregion

        #region CmdExAddSchdule

        private ICommand _cmdExAddSchdule;

        public ICommand CmdExAddSchdule
        {
            get
            {
                if (_cmdExAddSchdule == null)
                    _cmdExAddSchdule = new RelayCommand(ExCmdExAddSchdule, CanCmdExAddSchdule, true);
                return _cmdExAddSchdule;
            }
        }

        private void ExCmdExAddSchdule()
        {
            int max = 0;
            foreach (var t in TimeSchdules) if (t.Id > max) max = t.Id;
            max += 1;

            var tmp = new TimeSchduleItem() {Id = max};
            TimeSchdules.Add(tmp);
            CurrentSelectTimeSchduleItem = tmp;
            _dtExAddSchdule = DateTime.Now;
        }

        private DateTime _dtExAddSchdule;

        private bool CanCmdExAddSchdule()
        {
            return  DateTime.Now.Ticks - _dtExAddSchdule.Ticks > 10000000; ;
        }
        

       
        #endregion

        
        #endregion

        #region 对当前选中的方案 的具体数据进行删除 增加

        #region CmdExDeleteSchduleItem

        private ICommand _cmdExDeleteSchduleItem;

        public ICommand CmdExDeleteSchduleItem
        {
            get
            {
                if (_cmdExDeleteSchduleItem == null) _cmdExDeleteSchduleItem = new RelayCommand(ExCmdExDeleteSchduleItem, CanExDeleteSchduleItem, true);
                return _cmdExDeleteSchduleItem;
            }
        }

        private void ExCmdExDeleteSchduleItem()
        {
            if (CurrentSelectTimeSchduleItem.Schdules.Contains(CurrentSelectTimeSchduleItemItem))
            {
                CurrentSelectTimeSchduleItem.Schdules.Remove(CurrentSelectTimeSchduleItemItem);
                if (CurrentSelectTimeSchduleItem.Schdules.Count > 0)
                    CurrentSelectTimeSchduleItemItem = CurrentSelectTimeSchduleItem.Schdules[0];
                else CurrentSelectTimeSchduleItemItem = null;
            }
            _dtExDeleteSchduleItem = DateTime.Now;
        }
        private DateTime _dtExDeleteSchduleItem;

        private bool CanExDeleteSchduleItem()
        {
            return CurrentSelectTimeSchduleItem != null && CurrentSelectTimeSchduleItemItem != null && DateTime.Now.Ticks - _dtExDeleteSchduleItem.Ticks > 5000000; ;
        }


       
 
        #endregion

        #region CmdExAddSchduleItem

        private ICommand _cmdExAddSchduleItem;

        public ICommand CmdExAddSchduleItem
        {
            get
            {
                if (_cmdExAddSchduleItem == null) _cmdExAddSchduleItem = new RelayCommand(ExCmdExAddSchduleItem, CanExAddSchduleItem, true);
                return _cmdExAddSchduleItem;
            }
        }

        private void ExCmdExAddSchduleItem()
        {
            var tmp = new TimeSchduleItemItme();
            int max = 0;
            if (CurrentSelectTimeSchduleItem.Schdules .Count >7)
            {
                Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("不允许再增加", "目前设备只支持8档时间设置", UMessageBoxButton.Ok);
                return;
            }
            foreach (var t in CurrentSelectTimeSchduleItem.Schdules)
                if (t.Id > max) max = t.Id;
            max += 1;
            tmp.Id = max;
            CurrentSelectTimeSchduleItem.Schdules.Add(tmp);
            CurrentSelectTimeSchduleItemItem = tmp;
            _dtcmdExAddSchduleItem = DateTime.Now;
        }


        private DateTime _dtcmdExAddSchduleItem;
        private bool CanExAddSchduleItem()
        {
            return CurrentSelectTimeSchduleItem != null && DateTime.Now.Ticks - _dtcmdExAddSchduleItem.Ticks > 5000000; ;
        }

        #endregion

        #endregion

        /// <summary>
        /// 当选中的方案发生变化的时候 要求终端列表选中的也同步变化 
        /// </summary>
        /// <param name="rtulst"></param>
        private void SetRtuSelectedBanding(List<int> rtulst)
        {
            foreach (var t in TreeNodeRtu.Info)
            {
                if (rtulst.Contains(t.Key)) t.Value.IsSelected = true;
                else t.Value.IsSelected = false;
            }

            foreach (var t in this.ChildTreeItems) t.UpdateNodeSelectByChildNodeSelected();
        }

        //加载终端节点
        private void LoadNode()
        {
            ChildTreeItems.Clear();
            TreeNodeRtu.Info.Clear();
            //正常分组

            foreach (var t in Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.Keys)
            {
                if (t.Item1 == AreaId && t.Item2 > 0) ChildTreeItems.Add(new TreeNodeGroup(t.Item2,t.Item1));

            }

            //if (Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(0))
            //{
            //    var tmp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups[0].LstGrp;
            //  //  var atttmp = (from t in tmp orderby t ascending select t).ToList();
            //    var atttmp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(tmp);

            //    foreach (var t in atttmp)
            //    {
            //        if (
            //            !Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(t))
            //            continue;
            //        this.ChildTreeItems.Add(new TreeNodeGroup(t));
            //    }
            //}


            //无分组终端 特殊终端
            var tmp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                        
            //var lstNormal = (from t in TreeNodeRtu.Info select t.Key).ToList();
            //var lstSpecial = new List<int>();
            //foreach (
            //    var t in
            //        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary)
            //{
            //    var equipmentInfo = t.Value as IIRtuParaWork;
            //    if (equipmentInfo == null) continue;
            //    if (!lstNormal.Contains(equipmentInfo.RtuId)) lstSpecial.Add(equipmentInfo.RtuId);
            //} //通过与所有终端比对 查阅为分组终端
            if (tmp.Count > 0)
            {
                var f = new TreeNodeGroup() { NodeId = -1, NodeType = TreeNodeType.Special, NodeName = "未分组终端" };
                var atttmp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(tmp);


                foreach (var t in atttmp)
                {
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                    {
                        var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                        if (para.EquipmentType == WjParaBase.EquType.Rtu)
                        {
                            f.ChildTreeItems.Add(new TreeNodeRtu(t));
                            
                        }
                        
                    }
                }
                this.ChildTreeItems.Add(f);

                if (TreeNodeGroup.Info.ContainsKey(-1)) TreeNodeGroup.Info.Remove(-1);
                TreeNodeGroup.Info.Add(-1, f);
            }

            var lst = (from g in this.ChildTreeItems orderby g.NodeId select g).ToList();
            this.ChildTreeItems.Clear();
            foreach (var t in lst)
            {
                this.ChildTreeItems.Add(t);
            }
            
        }

        private void LoadRtuBanding()
        {
            RtuBandingTimeSchdule.Clear();
            this.TimeSchdules.Clear(); 
            
            var dirsss = new Dictionary<int, string>();
            foreach (var t in Wlst.Sr.TimeTableSystem.Services.HolidayTimeandBandingServices.Myself.InfoHolidaySchduleTimeGet)
            {
                if (t.Key.Item1 == AreaId)
                {
                    this.TimeSchdules.Add(new TimeSchduleItem(t.Value));
                    if (!dirsss.ContainsKey(t.Value.Id)) dirsss.Add(t.Value.Id, t.Value.Name);
                }
            }
            
           
            foreach (var t in Wlst.Sr.TimeTableSystem.Services.HolidayTimeandBandingServices.Myself.InfoRtuBandingSchduleGet)
            {
                if (t.Key.Item1 == AreaId)
                {
                    if (!this.RtuBandingTimeSchdule.ContainsKey(t.Key.Item2))
                    {
                        this.RtuBandingTimeSchdule.Add(t.Key.Item2, t.Value);
                    }
                    if (TreeNodeRtu.Info.ContainsKey(t.Key.Item2) && dirsss.ContainsKey(t.Value))
                        TreeNodeRtu.Info[t.Key.Item2].SchemeName = dirsss[t.Value];
                }
            }
           
        }


    }


    public partial class HolidayTimeSetViewModel
    {
        private bool _isViewShowed = false;

        public override  void NavOnLoadr(params object[] parsObjects)
        {
                _isViewShowed = true;
            Remind = "请设置方案并在右侧选择使用该方案的终端或分组即可.";
            AreaName.Clear();
            getAreaRId();
            if (AreaName.Count > 0) AreaComboBoxSelected = AreaName[0];
            if (AreaName.Count > 1) Visi = Visibility.Visible;
            else Visi = Visibility.Collapsed;

            //this.Load();
        
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


        private void Load()
        {
            if (_isViewShowed == false) return;
            this.LoadNode();
            this.LoadRtuBanding();
            _isViewShowed = true;
            //IsTreeEnable = false;
            IsTextEnable = false;
         

            if (TimeSchdules.Count > 0) CurrentSelectTimeSchduleItem = TimeSchdules[0];

            _dtExAddSchdule = DateTime.Now.AddMinutes(-1);
            _dtExDeleteSchdule = DateTime.Now.AddMinutes(-1);
            _dtExDeleteSchduleItem = DateTime.Now.AddMinutes(-1);
            _dtcmdExAddSchduleItem = DateTime.Now.AddMinutes(-1);
        }

        public override  void OnUserHideOrClosingr()
        {
            ChildTreeItems = new ObservableCollection<ListTreeNodeBase>();
            RtuBandingTimeSchdule.Clear() ;// = new Dictionary<int, int>();
            this.TimeSchdules = new ObservableCollection<TimeSchduleItem>();
            _isViewShowed = false;
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

      

    }

    public partial class HolidayTimeSetViewModel
    {



        #region CmdSave

        private ICommand _cCmdSave;

        public ICommand CmdSave
        {
            get
            {
                if (_cCmdSave == null)
                    _cCmdSave = new RelayCommand(ECmdSave, CanCmdSave, true);
                return _cCmdSave;
            }
        }

        private void ECmdSave()
        {
            CurrentSelectTimeSchduleItem = null;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在保存 ...";

            foreach (var t in TimeSchdules)
            {
                var lstday = new List<int>();
                var lstd = (from a in t.Schdules orderby a.MonthStart,a.DayStart select a).ToList();
                foreach (var tt in lstd)
                {
                    if (tt.MonthStart * 100 + tt.DayStart == tt.MonthEnd * 100 + tt.DayEnd)
                    {
                        lstday.Add(tt.MonthStart * 100 + tt.DayStart);
                    }
                    else
                    {
                        lstday.Add(tt.MonthStart * 100 + tt.DayStart);
                        lstday.Add(tt.MonthEnd * 100 + tt.DayEnd);
                    }

                }

                for (int i = 1; i < lstday.Count; i++)
                {
                    if (lstday[i] <= lstday[i - 1])
                    {
                        WlstMessageBox.Show("无法保存", "方案 " + t.Id + " " + t.Name + " 日期段有冲突，无法保存！", WlstMessageBoxType.Ok);
                        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  保存失败！";
                        return;
                    }

                    if (lstday[i - 1] < 100 || lstday[i] < 100)
                    {
                        WlstMessageBox.Show("无法保存", "方案 " + t.Id + " " + t.Name + " 日期段有0月，无法保存！", WlstMessageBoxType.Ok);
                        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  保存失败！";
                        return;
                    }
                }
            }



            var lst = (from t in RtuBandingTimeSchdule where t.Value == 0 select t.Key).ToList();
            foreach (var t in lst) RtuBandingTimeSchdule.Remove(t);
            
            lst.Clear();
            lst = (from t in TimeSchdules select t.Id).ToList();
            var lsttt = (from t in RtuBandingTimeSchdule where !lst.Contains(t.Value) select t.Key).ToList();
            foreach (var t in lsttt) RtuBandingTimeSchdule.Remove(t);


            var info = Wlst.Sr.ProtocolPhone .LxRtuTime  .wst_holiday_week_set  ;//.ServerPart.wlst_TimeTable_clinet_update_time_holiday;
            info.WstRtutimeHolidayWeekSet.Op = 2;
            var tmp = new HolidayWeekSetInfo.HolidaySchduleTimeAndBandingItem()
                          {
                             RtuBandings =new List<HolidayWeekSetInfo.RtuHolidaySchduleBandings>(),
                             Schdules = new List<HolidayWeekSetInfo.HolidaySchduleTime>(),
                             AreaId = AreaId
                          };
            foreach (var t in RtuBandingTimeSchdule) tmp.RtuBandings.Add(new HolidayWeekSetInfo.RtuHolidaySchduleBandings() { HolidaySchduleId = t.Value, RtuId = t.Key });
            foreach (var t in TimeSchdules) tmp.Schdules.Add(t.BackToSchdule());
            info.WstRtutimeHolidayWeekSet.HolidaySchduleTimeAndBandingItems.Add(tmp);


            #region todo
            //var has3005 = false;
            //foreach (var t in RtuBandingTimeSchdule)
            //{
            //    var type = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.Key];
            //    if (type.RtuModel == EnumRtuModel.Wj3005)
            //    {
            //        has3005 = true;
            //        continue;
            //    }
            //}

            //var err = false;
            //foreach (var t in TimeSchdules)
            //{
            //    foreach (var tt in t.Schdules)
            //    {
            //        if((tt.K7HourStart!=25||tt.K7HourEnd!=25||tt.K8HourEnd!=25||tt.K8HourStart!=25) && has3005 == true)
            //        {
            //            err = true;
            //            continue;
            //        }
            //    }
            //}

            //if (err)
            //{
            //    var infoss = WlstMessageBox.Show("警告",
            //                                        "有设备不支持K7K8，是否继续操作？ 是 继续，否 退出.", WlstMessageBoxType.YesNo);
            //    if (infoss == WlstMessageBoxResults.Yes)
            //    {
            //        SndOrderServer.OrderSnd(info, 10, 6);
            //        _dtExDeleteSchdule = DateTime.Now;
            //    }
            //}
            #endregion

            SndOrderServer.OrderSnd(info, 10, 6);
            _dtExDeleteSchdule = DateTime.Now;
        }

        private DateTime _dtCmdSave;

        private bool CanCmdSave()
        {
            //if (CurrentSelectTimeSchduleItem == null) return false;

            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(AreaId) && DateTime.Now.Ticks - _dtCmdSave.Ticks > 20000000; 
            //20160616 return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.IsInFullSetMod && DateTime.Now.Ticks - _dtCmdSave.Ticks > 20000000;
            
        }



        #endregion


    }

}
