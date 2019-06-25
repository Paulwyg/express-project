using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.ProtocolCnt.TimeTable;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Service;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.ViewModel
{
    [Export(typeof(IITimeTableManageViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeTableManageViewModel : ObservableObject, IITimeTableManageViewModel 
    {
        public void NavOnLoad(params object[] parsObjects)
        {
            InitDataPool();
        }

        public void OnUserHideOrClosing()
        {
            //throw new NotImplementedException();
            _dataPool.Clear();
            Items = new ObservableCollection<OneItemTimeTable>();
        }

        #region IITab
        public string Title
        {
            get { return "时间表管理"; }
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

        public TimeTableManageViewModel()
        {
            InitEvent();
        }
    }
    /// <summary>
    /// 属性及其命令
    /// </summary>
    public partial class TimeTableManageViewModel
    {
        #region Field
        private static List<WeekTimeTableInforemation> _dataPool = new List<WeekTimeTableInforemation>(); 
        #endregion
        #region ICommand
        private DateTime [] _dateTimes=new DateTime[3];
      
        
        #region CmdAddTimeTable

        private ICommand _cmdAddTimeTable;
        public ICommand CmdAddTimeTable
        {
            get { return _cmdAddTimeTable ?? (_cmdAddTimeTable = new RelayCommand(ExCmdAddTimeTable, CanCmdAddTimeTable, true)); }

        }
        private  bool CanCmdAddTimeTable()
        {
            return DateTime.Now.Ticks - _dateTimes[0].Ticks > 30000000;
        }
        private void ExCmdAddTimeTable()
        {
            _dateTimes[0] = DateTime.Now;
            ExPublishAnimationEvent();
            var weekTimeTable = new WeekTimeTableInforemation();
            var id = 1001;
            foreach (var t in Items)
            {
                if (t.TimeTableId >= id)
                {
                    id = t.TimeTableId + 1;
                }
            }
            weekTimeTable.time_id = id;
            weekTimeTable.time_name = "NewCreate";
            var dateOffirstSunDay = DateTime.Now.AddDays(Convert.ToDouble((0 - Convert.ToInt16(DateTime.Now.DayOfWeek))));


            for (var i = 0; i < 7; i++)
            {
                var tt = new WeekTimeTableItemInfomation();
                var dayOftoday = dateOffirstSunDay.AddDays(i);
                tt.date_day = dayOftoday.Day;
                tt.date_month = dayOftoday.Month;
                
                weekTimeTable.LstOneWeekOpenCloseControl.Add(tt);
            }
            weekTimeTable.light_off_offset = this.Items.Count > 0 ? Items[0].LightOffOffset : -15;
            weekTimeTable.light_on_offset = this.Items.Count > 0 ? Items[0].LightOnOffset : 15;
            weekTimeTable.LuxEffective = this.Items.Count > 0 ? Items[0].LuxEffective  : 30;
            weekTimeTable.lux_off_value = this.Items.Count > 0 ? Items[0].LuxOffValue  : 15;
            weekTimeTable.lux_on_value = this.Items.Count > 0 ? Items[0].LuxOnValue : 15;
          

            var tv = new OneItemTimeTable(weekTimeTable);

            AddTimeTableViewModel.OneItemTimeTable = tv;
        }




        #endregion

        #region CmdModifyTimeTable

        private ICommand _cmdModifyTimeTable;
        public ICommand CmdModifyTimeTable
        {
            get { return _cmdModifyTimeTable ?? (_cmdModifyTimeTable = new RelayCommand(ExCmdModifyTimeTable, CanCmdModifyTimeTable, true)); }

        }
        private  bool CanCmdModifyTimeTable()
        {
            return CurrentSelectItem!=null && DateTime.Now.Ticks - _dateTimes[1].Ticks > 10000000;
        }
        private void ExCmdModifyTimeTable()
        {
            _dateTimes[1] = DateTime.Now;
            AddTimeTableViewModel.OneItemTimeTable = CurrentSelectItem;
            ExPublishAnimationEvent();
        }

        #endregion


        #region CmdSaveAndSnd
        private ICommand _cmdSaveAndSnd;
        public ICommand CmdSaveAndSnd
        {
            get { return _cmdSaveAndSnd ?? (_cmdSaveAndSnd = new RelayCommand(ExCmdSaveAndSnd, CanCmdSaveAndSnd, true)); }
        }
        private  bool CanCmdSaveAndSnd()
        {
            return DateTime.Now.Ticks-_dateTimes[2].Ticks>10000000;
        }

        private void ExCmdSaveAndSnd()
        {
            var infoss = WlstMessageBox.Show("确认保存", "即将保存信息，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;

            _dateTimes[2] = DateTime.Now;
            if (_dataPool.Count <= 0) return;
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存...";
            WeekTimeTableInfoService.UpdateTimeTable(_dataPool);
        }

        #endregion


        #region CmdDeleteTimeTable

        private ICommand _cmddeleteTimeTable;
        public ICommand CmdDeleteTimeTable
        {
            get { return _cmddeleteTimeTable ?? (_cmddeleteTimeTable = new RelayCommand(ExCmdDeleteTimeTable, CanCmdDeleteTimeTable, true)); }

        }
        private bool CanCmdDeleteTimeTable()
        {
            return DateTime.Now.Ticks - _dateTimes[0].Ticks > 10000000;
        }
        private void ExCmdDeleteTimeTable()
        {
            _dateTimes[0] = DateTime.Now;
            if (CurrentSelectItem == null) return;
            var lst =
                Sr.TimeTableSystem.Services.TmlLoopBngTimeTableInfoService.GetBangdingToThisTimeTablesTmls(
                    CurrentSelectItem.TimeTableId);
            if (lst.Count > 0)
            {
                // MessageBox.Show("无法执行删除", "至少存在" + lst.Count + "个终端正在使用该时间信息，请确认!!!", MessageBoxButton.OK);
                WlstMessageBox.Show("无法执行删除",
                                    "存在" + lst.Count + "路输出正在使用该时间信息，无法执行删除!", WlstMessageBoxType.Ok);
                return;
            }
            var info = WlstMessageBox.Show("确认删除",
                                           "确认删除。", WlstMessageBoxType.YesNoCancel);
            if (info == WlstMessageBoxResults.Yes)
            {
                int id = CurrentSelectItem.TimeTableId;
                this.Items.Remove(CurrentSelectItem);
                if (this.Items.Count > 0) CurrentSelectItem = Items[0];
                foreach (var g in _dataPool )if(g.time_id ==id )
                {
                    _dataPool.Remove(g);
                    break;
                }
            }
            BtnSaveAndSndIsEnable = true;
        }




        #endregion


        private void ExPublishAnimationEvent()
        {
            //发布事件，返回原界面
            var ar = new PublishEventArgs
            {
                EventId = TimeTableSystem.Services.EventIdAssign.EventAddOrUpdateTimeTableAnimationId,
                EventType = PublishEventType.None
            };
            EventPublisher.EventPublish(ar);
        }

        #endregion

        #region Attribute
        private AddTimeTableViewModel _addTimeTableViewModel;
        public AddTimeTableViewModel AddTimeTableViewModel
        {
            get { return _addTimeTableViewModel ?? (_addTimeTableViewModel = new AddTimeTableViewModel()); }
            set
            {
                if (_addTimeTableViewModel == value) return;
                _addTimeTableViewModel = value;
                RaisePropertyChanged(()=>AddTimeTableViewModel);
            }
        }

        private OneItemTimeTable _currentSelectItem=null;
        public OneItemTimeTable CurrentSelectItem
        {
            get { return _currentSelectItem; }
            set
            {
                if(_currentSelectItem==value) return;
                _currentSelectItem = value;
                RaisePropertyChanged(()=>CurrentSelectItem);
            }
        }

        private ObservableCollection<OneItemTimeTable> _items;
        public ObservableCollection<OneItemTimeTable> Items
        {
            get { return _items ?? (_items = new ObservableCollection<OneItemTimeTable>()); }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => Items);
            }
        }

        private Visibility _msgVisi;
        public Visibility MsgVisi
        {
            get { return _msgVisi; }
            set
            {
                if(value==_msgVisi) return;
                _msgVisi = value;
                RaisePropertyChanged(()=>MsgVisi);
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
                RaisePropertyChanged(()=>Msg);
            }
        }

        private bool _btnSaveAndSndIsEnable;
        public bool BtnSaveAndSndIsEnable
        {
            get { return _btnSaveAndSndIsEnable; }
            set
            {
                if(_btnSaveAndSndIsEnable==value) return;
                _btnSaveAndSndIsEnable = value;
                RaisePropertyChanged(()=>BtnSaveAndSndIsEnable);
            }
        }

        #endregion
    }

    /// <summary>
    /// 自定义方法及其函数
    /// </summary>
    public partial class TimeTableManageViewModel
    {
        //初始化数据池，从服务层中读取数据
        private void InitDataPool()
        {
            _dataPool.Clear();
            foreach (var itemTable in WeekTimeTableInfoService.GeteekTimeTableInfoList)
            {
                var onetable = new WeekTimeTableInforemation();
                foreach (var t in itemTable.LstOneWeekOpenCloseControl)
                {
                    onetable.LstOneWeekOpenCloseControl.Add(new WeekTimeTableItemInfomation
                                                                {
                                                                    date_day = t.date_day,
                                                                    date_month =t.date_month,
                                                                    dayOfWeek = t.dayOfWeek,
                                                                    is_light_OffOffSet_on = t.is_light_OffOffSet_on,
                                                                    is_light_OnOffSet_on = t.is_light_OnOffSet_on,
                                                                    is_lux_off = t.is_lux_off,
                                                                    is_lux_on =t.is_lux_on,
                                                                    time_off =t.time_off,
                                                                    time_on =t.time_on
                                                                });
                }
                onetable.LuxEffective = itemTable.LuxEffective;
                onetable.light_off_offset = itemTable.light_off_offset;
                onetable.light_on_offset = itemTable.light_on_offset;
                onetable.lux_id = itemTable.lux_id;
                onetable.lux_off_value = itemTable.lux_off_value;
                onetable.lux_on_value = itemTable.lux_on_value;
                onetable.time_desc = itemTable.time_desc;
                onetable.time_id = itemTable.time_id;
                onetable.time_name = itemTable.time_name;
                _dataPool.Add(onetable);
            }
            LoadItems();
        }
        private void LoadItems()
        {
          
            //var lstnn = (from t in Items select t.TimeTableId).ToList();
            // var lst = (from t in _dataPool  select t.time_id).ToList();
            //var delete = (from t in lstnn where !lst.Contains(t) select t).ToList();
            //foreach (var t in delete )
            //{
            //    foreach (var g in Items )
            //    {
            //        if(g.TimeTableId ==t)
            //        {
            //            Items.Remove(g);
            //            break;
            //        }
            //    }
            //}

            Items.Clear();
            foreach (var item in _dataPool)
            {
                var bolfind = false;
                foreach (var tt in Items)
                {
                    if(item==null ||tt.TimeTableId!=item.time_id) continue;
                    tt.ReloadLux();
                    bolfind = true;
                    break;
                }
                if(!bolfind)
                {
                    Items.Add(new OneItemTimeTable(item));
                }
            }
            if (Items.Count > 0) CurrentSelectItem = Items[0];
        }
        //private void GetTimeTableViewModelData()
        //{
        //    Items.Clear();
        //    foreach (var t in WeekTimeTableInfoService.GeteekTimeTableInfoList)
        //    {
        //        var bolfind = false;
        //        foreach (var tt in Items)
        //        {
        //            if (t == null || tt.TimeTableId != t.time_id) continue;
        //            tt.ReloadLux();

        //            bolfind = true;
        //            break;
        //        }
        //        if (!bolfind)
        //        {
        //            Items.Add(new OneItemTimeTable(t));
        //        }
        //    }
        //    if (Items.Count > 0) CurrentSelectItem = Items[0];
        //}
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class TimeTableManageViewModel
    {

        private void InitEvent()
        {
            EventPublisher.AddEventSubScriptionTokener(Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }

        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.Core )
                {
                    switch (args.EventId)
                    {
                        case TimeTableSystem.Services.EventIdAssign.EventSaveTimeTableData:
                            return true;
                        case TimeTableSystem.Services.EventIdAssign.EventCancelTimeTableData:
                            return true;
                        case EventIdAssign.TimeTimeAdd:
                            return true;
                        case  EventIdAssign.TimeTimeUpdate:
                            return true;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.Core && args.EventId==TimeTableSystem.Services.EventIdAssign.EventSaveTimeTableData)
                {
                    //保存后可以点击保存并下发按钮，
                    BtnSaveAndSndIsEnable = true;
                    var isAdd =true ;
                    foreach (var item in Items.Where(item => item.TimeTableId == AddTimeTableViewModel.OneItemTimeTable.TimeTableId))
                    {
                        isAdd = false;
                        item.TimeTableName = AddTimeTableViewModel.OneItemTimeTable.TimeTableName;
                        item.TimeDesc = AddTimeTableViewModel.OneItemTimeTable.TimeDesc;
                        item.OneWeekTimeTable = AddTimeTableViewModel.OneItemTimeTable.OneWeekTimeTable;
                        item.LuxOnValue = AddTimeTableViewModel.OneItemTimeTable.LuxOnValue;
                        item.LuxOffValue = AddTimeTableViewModel.OneItemTimeTable.LuxOffValue;
                        item.LuxName = AddTimeTableViewModel.OneItemTimeTable.LuxName;
                        item.LuxId = AddTimeTableViewModel.OneItemTimeTable.LuxId;
                        item.LuxEffective = AddTimeTableViewModel.OneItemTimeTable.LuxEffective;
                        item.LightOnOffset = AddTimeTableViewModel.OneItemTimeTable.LightOnOffset;
                        item.LightOffOffset = AddTimeTableViewModel.OneItemTimeTable.LightOffOffset;
                        item.CurrentSelectLux = AddTimeTableViewModel.OneItemTimeTable.CurrentSelectLux;
                    }
                    //更新数据池中的数据。将数据清除，然后将Items数据写入数据池中。
                    _dataPool.Clear();
                    foreach (var item in Items)
                    {
                        _dataPool.Add(item.BackToWeekTimeTableInforemation());
                    }
                    if(isAdd)
                    {
                        _dataPool.Add(AddTimeTableViewModel.OneItemTimeTable.BackToWeekTimeTableInforemation());
                        Items.Add(AddTimeTableViewModel.OneItemTimeTable);
                    }
                }
                if(args.EventType == PublishEventType.Core && args.EventId==TimeTableSystem.Services.EventIdAssign.EventCancelTimeTableData)
                {
                    //将已经更改过的数据还原，方法是用数据池中的数据覆盖Items中的数据。
                    LoadItems();
                    Msg = "";
                    MsgVisi=Visibility.Collapsed;
                }
                if(args.EventType== PublishEventType.Core &&args.EventId==EventIdAssign.TimeTimeAdd)
                {
                    BtnSaveAndSndIsEnable = false;
                    InitDataPool();
                    // 增加成功
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"  更新成功.";
                    MsgVisi=Visibility.Visible;
                }
                if (args.EventType == PublishEventType.Core && args.EventId == EventIdAssign.TimeTimeUpdate)
                {
                    BtnSaveAndSndIsEnable = false;
                    InitDataPool();
                    // 更新成功
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  更新成功.";
                    MsgVisi = Visibility.Visible;
                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("SaveTimeTableData error in FundEventHandlers:ex:" + xe);
            }
        }
    }
}
