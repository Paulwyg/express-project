using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.Views;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.ViewModels
{
    
    public partial class TimeTabletemporaryViewModel : EventHandlerHelperExtendNotifyProperyChanged 
    {
        
        #region IITab

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "开关灯特设方案"; }
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

        public TimeTabletemporaryViewModel()
        {
            this.AddEventFilterInfo(EventIdAssign.TimeTemporaryInfoUpdateId, PublishEventType.Core, true);
            this.AddEventFilterInfo(EventIdAssign.TimeTemporaryInfoRequestId, PublishEventType.Core, true);
            this.AddEventFilterInfo(EventIdAssign.TimeTemporaryInfoDeleteId, PublishEventType.Core, true);
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);
            if (args.EventId == EventIdAssign.TimeTemporaryInfoUpdateId)
            {
                if (DateTime.Now.Ticks - _dtCmdSave.Ticks < 600000000)
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "保存成功.");
                else
                {
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "数据更新.");

                }
            }
            if (args.EventId == EventIdAssign.TimeTemporaryInfoDeleteId)
            {
                if (DateTime.Now.Ticks - _dtCmdSave.Ticks < 600000000)
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "删除成功.");
                else
                {
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss" + "  " + "数据更新.");

                }
            }
        }

    }

    public partial class TimeTabletemporaryViewModel
    {
        private ObservableCollection<OneItemTemporary> _sTemporaryItems;
        /// <summary>
        /// 临时时间方案集合
        /// </summary>
        public ObservableCollection<OneItemTemporary> TemporaryItems
        {
            get
            {
                if (_sTemporaryItems == null)
                {
                    if (TimeInfoMnVm.PassTemporaryItem != null)
                        _sTemporaryItems = TimeInfoMnVm.PassTemporaryItem;
                    else
                        _sTemporaryItems = new ObservableCollection<OneItemTemporary>();
                }
                return _sTemporaryItems;
            }
            set
            {
                if (_sTemporaryItems != value)
                {
                    _sTemporaryItems = value;
                    this.RaisePropertyChanged(() => this.TemporaryItems);
                }
            }
        }

        private OneItemTemporary _currentSelectedTemporaryItems;
        /// <summary>
        /// 当前选中方案
        /// </summary>
        public OneItemTemporary CurrentSelectedTemporaryItems
        {
            get
            {
                if (_currentSelectedTemporaryItems == null && TimeInfoMnVm.PassTemporaryItem.Count != 0)
                    _currentSelectedTemporaryItems = TimeInfoMnVm.PassTemporaryItem[0];
                return _currentSelectedTemporaryItems;
            }
            set
            {
                if (_currentSelectedTemporaryItems == value || value == null) return;
                _currentSelectedTemporaryItems = value;
                RaisePropertyChanged(() => CurrentSelectedTemporaryItems);
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

    }

    /// <summary>
    /// ICommand
    /// </summary>
    public partial class TimeTabletemporaryViewModel
    {
        /// <summary>
        /// 增加方案
        /// </summary>

        #region CmdAdd
        private DateTime _dtCmdAdd;

        private ICommand _cmdAdd;

        public ICommand CmdAdd
        {
            get
            {
                return _cmdAdd ??
                       (_cmdAdd = new RelayCommand(ExCmdAdd, CanCmdAdd, true));
            }
        }

        private bool CanCmdAdd()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 &&
                   DateTime.Now.Ticks - _dtCmdAdd.Ticks > 30000000;
        }

        private void ExCmdAdd()
        {
            _dtCmdAdd = DateTime.Now;
            var txv = new OneItemTemporary();
            //txv.AreaId = TimeInfoMnVm.areaId;
            var id = 0;
            foreach (var t in TemporaryItems)
            {
                if (t.SchemeId >= id)
                {
                    id = t.SchemeId;
                }
            }
            txv.SchemeId = id + 1;
            txv.SchemeName = "新建方案" + txv.SchemeId;
            TemporaryItems.Add(txv);
            CurrentSelectedTemporaryItems = txv;
            //var itemtable = new TempTimePlanWithTimeTableBandingInfo.TimeTablePlan();
            //itemtable.TimePlanId = id + 1;
            //itemtable.TimePlanName = "新建方案"+itemtable.TimePlanId;
            //itemtable.DateStart = DateTime.Now.Ticks;
            //itemtable.DateEnd = DateTime.Now.AddDays(1).Ticks;
            //itemtable.LightOffOffset = -15;
            //itemtable.LightOnOffset = 15;
            //itemtable.LuxOffValue = 15;
            //itemtable.LuxOnValue = 15;
            //itemtable.LuxEffective = 30;
            //var txv = new OneItemTemporary(itemtable, TimeInfoMnVm.areaId);
            //TemporaryItems.Add(txv);
            //CurrentSelectedTemporaryItems = txv;
        }

        #endregion

        /// <summary>
        /// 删除方案
        /// </summary>

        #region CmdDelete
        private DateTime _dtCmdDelete;

        private ICommand _cmdDelete;

        public ICommand CmdDelete
        {
            get
            {
                return _cmdDelete ??
                       (_cmdDelete = new RelayCommand(ExCmdDelete, CanCmdDelete, true));
            }
        }

        private bool CanCmdDelete()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 &&
                   DateTime.Now.Ticks - _dtCmdDelete.Ticks > 10000000 && CurrentSelectedTemporaryItems != null;
        }

        private void ExCmdDelete()
        {
            _dtCmdDelete = DateTime.Now;
            if (TemporaryItems.Contains(CurrentSelectedTemporaryItems))
            {
                var infoss = WlstMessageBox.Show("确认删除", "即将删除当前选中方案，是 继续，否 退出.", WlstMessageBoxType.YesNo);
                if (infoss != WlstMessageBoxResults.Yes) return;
                TemporaryItems.Remove(CurrentSelectedTemporaryItems);
                //if (TemporaryItems.Count > 0)
                //    CurrentSelectedTemporaryItems = TemporaryItems[0];
            }

            var temp = new TempTimePlanWithTimeTableBandingInfo.TimeTablePlan()
            {
                AreaId = TimeInfoMnVm.areaId,               
                TimePlanId =CurrentSelectedTemporaryItems.SchemeId
            };
            var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_temp_time_plan;
            info.WstRtutimeTempTimePlan.Op = 3;
            info.WstRtutimeTempTimePlan.TempTimePlanItems.Add(temp);
            SndOrderServer.OrderSnd(info, 10, 2);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在删除 ...";
        }

        #endregion

        /// <summary>
        /// 确定时间
        /// </summary>

        #region CmdDeterminData
        private DateTime _dtCmdDeterminData;

        private ICommand _cmdDeterminData;

        public ICommand CmdDeterminData
        {
            get
            {
                return _cmdDeterminData ??
                       (_cmdDeterminData = new RelayCommand(ExCmdDeterminData, CanCmdDeterminData, true));
            }
        }

        private bool CanCmdDeterminData()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 &&
                   CurrentSelectedTemporaryItems != null && DateTime.Now.Ticks - _dtCmdDeterminData.Ticks > 30000000;
        }

        private void ExCmdDeterminData()
        {
            _dtCmdDeterminData = DateTime.Now;
            CurrentSelectedTemporaryItems.RuleItems.Clear();
            if (CurrentSelectedTemporaryItems.DtEndTime < CurrentSelectedTemporaryItems.DtStartTime)
            {
                var infoss = WlstMessageBox.Show("警告",
                                                 "结束时间应大于开始时间", WlstMessageBoxType.Ok);
                return;
            }
            TimeSpan diff = CurrentSelectedTemporaryItems.DtEndTime - CurrentSelectedTemporaryItems.DtStartTime;
            if (diff.TotalDays > 30)
            {
                var infoss = WlstMessageBox.Show("警告",
                                                 "时间跨度应小于30天", WlstMessageBoxType.Ok);
                return;
            }
            for (int i = 0; i < diff.TotalDays+1; i++)
            {
                var tmp = new TimeTableOneDayInfomationItem()
                              {
                                  TimeAreaId = TimeInfoMnVm.areaId,
                                  DateMonth = CurrentSelectedTemporaryItems.DtStartTime.AddDays(i).Month,
                                  DateDay = CurrentSelectedTemporaryItems.DtStartTime.AddDays(i).Day,
                                  Date = CurrentSelectedTemporaryItems.DtStartTime.AddDays(i).ToString("yyyy.MM.dd"),
                                  TimeOff = 1500,
                                  TimeOn = 1500,
                                  TimetableSectionId = 1,
                                  RuleSectionId = 1,
                                  IsUsedLuxOff = false,
                                  IsUsedLuxOn = false,
                                  IsUsedOffSet = false,
                                  IsUsedOnSet = false,
                                  LightOffOffSet = CurrentSelectedTemporaryItems.LightOffOffset,
                                  LightOnOffSet = CurrentSelectedTemporaryItems.LightOnOffset,
                                  IsEdit = true,
                                  TimetableId = CurrentSelectedTemporaryItems.SchemeId,
                                  IsTimeOffEnable = true,
                                  IsTimeOnEnable = true

                              };
                tmp.DayOfWeekUsed = (int) CurrentSelectedTemporaryItems.DtStartTime.AddDays(i).DayOfWeek;
                CurrentSelectedTemporaryItems.RuleItems.Add(tmp);
            }
        }

        #endregion


        /// <summary>
        /// 选择时间表
        /// </summary>

        #region CmdSelectTimeTable

        private SelectTimeTable _selectTimeTable = null;

        private DateTime _dtCmdSelectTimeTable;
        private ICommand _cmdSelectTimeTable;

        public ICommand CmdSelectTimeTable
        {
            get
            {
                return _cmdSelectTimeTable ??
                       (_cmdSelectTimeTable = new RelayCommand(ExCmdSelectTimeTable, CanCmdSelectTimeTable, true));
            }
        }

        private bool CanCmdSelectTimeTable()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 &&
                   CurrentSelectedTemporaryItems != null && DateTime.Now.Ticks - _dtCmdSelectTimeTable.Ticks > 30000000;
        }

        private void ExCmdSelectTimeTable()
        {
            _dtCmdSelectTimeTable = DateTime.Now;
            _selectTimeTable = new SelectTimeTable();
            var select = new OneItemTemporary();
            if (TimeInfoMnVm.PassItems.Count == 0)
            {
                var infoss = WlstMessageBox.Show("警告",
                                                 "当前区域无时间表", WlstMessageBoxType.Ok);
                return;
            }
            var plans = (from tt in TimeTabletemporaryHold.Myself.Info
                         where tt.Key.Item1 == TimeInfoMnVm.areaId
                         orderby tt.Key.Item2 ascending
                         select tt.Value).ToList();
            foreach (var t in TimeInfoMnVm.PassItems)
            {               
                string owner = "";
                var isenbled = true;
                foreach (var x in plans)
                {
                    foreach (var xx in x.TimetablesUseThisPlan)
                    {
                        if (t.TimeId == xx)
                        {
                            owner = x.TimePlanName;
                            if (CurrentSelectedTemporaryItems.SchemeId != x.TimePlanId)
                                isenbled = false;
                        }
                        //owner = owner + " " + x.TimePlanName;
                    }

                }
                var isselect=false;
                foreach (var f in CurrentSelectedTemporaryItems.SelectedItems)
                {
                    if(f.TimeId==t.TimeId)
                    {
                         isselect = true;
                    }
                }

                //判断当前方案时间段与时间表时间段是否相同
                var sectionid = 1;

                foreach (var f in CurrentSelectedTemporaryItems.RuleItems)
                {
                    if (f.TimetableSectionId > sectionid)
                        sectionid = f.TimetableSectionId;
                }
                if (sectionid != t.RuleItems[0].Maxsection)
                    isenbled = false;
                

                var tml = new OneItemTimeTable()
                              {
                                  TimeId = t.TimeId,
                                  TimeName = t.TimeName,
                                  TimeDesc = t.TimeDesc,
                                  OwnerScheme = owner,
                                  SectionNumber = t.RuleItems[0].Maxsection,
                                  IsSelected = isselect,
                                  IsEnableUsed = isenbled
                              };
                select.SelectedItems.Add(tml);
            }
            _selectTimeTable.OnFormBtnOkClick += _selectTimeTable_OnFormBtnOkClick;
            _selectTimeTable.SetContext(select);
            _selectTimeTable.ShowDialog();
        }

        private void _selectTimeTable_OnFormBtnOkClick(object sender, SelectTimeTable.EventArgsSelectTimeTable e)
        {
            var updateinfo = e.SelectTimeTable;
            if (updateinfo == null) return;
            CurrentSelectedTemporaryItems.SelectedItems.Clear();
            foreach (var t in updateinfo.SelectedItems)
            {
                if (t.IsSelected)
                    CurrentSelectedTemporaryItems.SelectedItems.Add(t);
            }
            try
            {
                _selectTimeTable.OnFormBtnOkClick -=
                    new EventHandler<SelectTimeTable.EventArgsSelectTimeTable>(_selectTimeTable_OnFormBtnOkClick);
            }
            catch (Exception ex)
            {

            }
            _selectTimeTable = null;

        }

        #endregion

        /// <summary>
        /// 保存
        /// </summary>
        #region CmdSave

        private DateTime _dtCmdSave;
        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new RelayCommand(ExCmdSave, CanCmdSave, true)); }
        }

        private bool CanCmdSave()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 &&
                   CurrentSelectedTemporaryItems != null && DateTime.Now.Ticks - _dtCmdSave.Ticks > 30000000;
        }


        
        private void ExCmdSave()
        {
            _dtCmdSave = DateTime.Now;
            if(CurrentSelectedTemporaryItems.SchemeName.Trim()==string.Empty)
            {
                var infos = WlstMessageBox.Show("警告",
                                                     "方案名称不能为空", WlstMessageBoxType.Ok);
                return;
            }
            foreach (var t in CurrentSelectedTemporaryItems.RuleItems)
            {
                if (t.IsUsedLuxOff || t.IsUsedLuxOn)
                {
                    if (CurrentSelectedTemporaryItems.CurrentSelectLux.Id <= 0 &&
                        CurrentSelectedTemporaryItems.CurrentSelectLux2.Id > 0)
                    {
                        var infos = WlstMessageBox.Show("警告",
                                                        "未设置主光控时设置了备用光控！", WlstMessageBoxType.Ok);
                        return;
                    }
                    if (CurrentSelectedTemporaryItems.CurrentSelectLux.Id <= 0)
                    {
                        var infos = WlstMessageBox.Show("警告",
                                                        "未选择主光控！", WlstMessageBoxType.Ok);
                        return;

                    }

                }

            }

            int time = 0;
            foreach (var t in CurrentSelectedTemporaryItems.RuleItems)
            {
                if (t.TimeOff >= 1500 && t.TimeOn >= 1500)
                    time++;
            }
            if (time == CurrentSelectedTemporaryItems.RuleItems.Count)
            {
                var infos = WlstMessageBox.Show("警告",
                                                        "当前方案没有设置时间！",
                                                        WlstMessageBoxType.Ok);
                return;
            }

            if(CurrentSelectedTemporaryItems.SelectedItems.Count==0)
            {
                var infos = WlstMessageBox.Show("警告",
                                                        "当前方案未绑定时间表！",
                                                        WlstMessageBoxType.Ok);
                return;
            }


            var plans = (from tt in TimeTabletemporaryHold.Myself.Info
                         where tt.Key.Item1 == TimeInfoMnVm.areaId
                         orderby tt.Key.Item2 ascending
                         select tt.Value).ToList();
            //判断当前绑定的方案是否与当前方案时间上有重合
            //foreach (var t in plans)
            //{
            //    foreach (var tt in t.TimetablesUseThisPlan)
            //    {
            //        foreach (var x in CurrentSelectedTemporaryItems.SelectedItems)
            //        {
            //            if (x.TimeId == tt && CurrentSelectedTemporaryItems.SchemeId != t.TimePlanId)
            //            {
            //                if (CurrentSelectedTemporaryItems.DtEndTime < new DateTime(t.DateStart) ||
            //                    CurrentSelectedTemporaryItems.DtStartTime > new DateTime(t.DateEnd))
            //                {
            //                }
            //                else
            //                {
            //                    var infos = WlstMessageBox.Show("警告",
            //                                                    "当前方案选中的时间表已有绑定方案，且该方案起始时间与当前方案有重合！",
            //                                                    WlstMessageBoxType.Ok);
            //                    return;
            //                }
            //            }
            //        }

            //    }
                
            //}
            foreach (var t in plans)
            {
                foreach (var tt in t.TimetablesUseThisPlan)
                {
                    foreach (var x in CurrentSelectedTemporaryItems.SelectedItems)
                    {
                        if(x.TimeId==tt&& CurrentSelectedTemporaryItems.SchemeId != t.TimePlanId)
                        {
                            var infos = WlstMessageBox.Show("警告",
                                                               "当前方案选中的时间表已绑定方案！",
                                                                WlstMessageBoxType.Ok);
                                return;
                        }

                    }
                }
            }

            var sectionid = 1;
            var sectionid1 = 1;
            foreach (var t in CurrentSelectedTemporaryItems.RuleItems)
            {
                if(t.TimetableSectionId>sectionid)
                    sectionid = t.TimetableSectionId;
            }
            foreach (var f in CurrentSelectedTemporaryItems.SelectedItems)
            {
                foreach (var ff in TimeInfoMnVm.PassItems)
                {
                    if (ff.TimeId == f.TimeId)
                        foreach (var fff in ff.RuleItems)
                        {
                            if (fff.TimetableSectionId > sectionid1)
                                sectionid1 = fff.TimetableSectionId;
                        }
                }

            }
            if(sectionid!=sectionid1)
            {
                var infos = WlstMessageBox.Show("警告",
                                                "当前方案时间段与选中时间表中时间段不一致！",
                                                WlstMessageBoxType.Ok);
                return;
            }
            var listItems =
                CurrentSelectedTemporaryItems.RuleItems.OrderBy(u => u.TimetableSectionId).ThenBy(u => u.DateMonth).
                    ThenBy(u => u.DateDay).
                    ToList();
            TimeSpan diff = CurrentSelectedTemporaryItems.DtEndTime - CurrentSelectedTemporaryItems.DtStartTime;
            for (int i = 0; i < diff.TotalDays+1; i++)
            {
                foreach (var t in listItems)
                {
                    if (t.TimetableSectionId < sectionid && sectionid > 1 && //有多段时间、时间段不是最后一段、同一天、不同段、关灯时间小于第一段开灯时间
                        t.TimetableSectionId != listItems[i].TimetableSectionId &&
                        t.Date == listItems[i].Date &&
                        t.TimeOff < listItems[i].TimeOn)
                    {
                        var infos = WlstMessageBox.Show("警告",
                                                        "当前方案存在非最后时间段跨天！",
                                                        WlstMessageBoxType.Ok);
                        return;
                    }

                    if (t.TimetableSectionId == sectionid && t.Date == listItems[i].Date &&    //有多段时间、最后时间段、同一天
                       sectionid > 1)
                    {
                        if (t.TimeOff > listItems[i].TimeOn && t.TimeOff < t.TimeOn) //关灯时间大于第一段开灯时间、关灯时间小于同一段时间
                        {
                            var infos = WlstMessageBox.Show("警告",
                                                            "当前方案最后时间段关灯时间大于开灯时间！",
                                                            WlstMessageBoxType.Ok);
                            return;
                        }
                        if (t.TimeOn < listItems[i].TimeOn) //最后一段开灯时间小于第一段开灯时间
                        {
                            var infos = WlstMessageBox.Show("警告",
                                                            "当前方案最后时间段开灯时间跨天！",
                                                            WlstMessageBoxType.Ok);
                            return;
                        }
                    }
                }

                if (sectionid > 1)
                {
                    for (int j = 0; j < listItems.Count/(diff.TotalDays + 1) - 1; j++)
                    {
                        if (listItems[(int) (i + (diff.TotalDays + 1)*(j + 1))].TimeOn >=
                            listItems[(int) (i + (diff.TotalDays + 1)*j)].TimeOn
                            &&
                            listItems[(int) (i + (diff.TotalDays + 1)*(j + 1))].TimeOn <=
                            listItems[(int) (i + (diff.TotalDays + 1)*j)].TimeOff)
                        {
                            var infos = WlstMessageBox.Show("警告",
                                                            "当前方案存在同一时间段内关灯时间大于开灯时间！",
                                                            WlstMessageBoxType.Ok);
                            return;
                        }
                    }
                }

            }
            



            var ruleitems = new List<TempTimePlanWithTimeTableBandingInfo.TimeTablePlan.TimeTableOnedayPlan>();
            var belongtimetable = new List<int>();
          
            foreach (var t in CurrentSelectedTemporaryItems.RuleItems)
            {
                if (t.TimeOn == 1500 && t.TimeOff == 1500) continue;
                int on, off = 0;
                if (t.IsUsedLuxOn) on = 1;
                else if (t.IsUsedOnSet) on = 2;
                else on = 3;
                if (t.IsUsedLuxOff) off = 1;
                else if (t.IsUsedOffSet) off = 2;
                else off = 3;
                ruleitems.Add(new TempTimePlanWithTimeTableBandingInfo.TimeTablePlan.TimeTableOnedayPlan()
                                  {
                                      Date = long.Parse(t.Date.Replace(".","")),
                                      SectionId = t.TimetableSectionId,
                                      TimeOff = t.TimeOff,
                                      TimeOn = t.TimeOn,
                                      TypeOff = off,
                                      TypeOn = on
                                  });
            }
            foreach (var f in CurrentSelectedTemporaryItems.SelectedItems)
            {
                belongtimetable.Add(f.TimeId);
            }
            var temp = new TempTimePlanWithTimeTableBandingInfo.TimeTablePlan()
                           {
                               AreaId = TimeInfoMnVm.areaId,
                               DateEnd = CurrentSelectedTemporaryItems.DtEndTime.Ticks,
                               DateStart = CurrentSelectedTemporaryItems.DtStartTime.Ticks,
                               ItemsPlan = ruleitems,
                               LightOffOffset = CurrentSelectedTemporaryItems.LightOffOffset,
                               LightOnOffset = CurrentSelectedTemporaryItems.LightOnOffset,
                               LuxEffective = CurrentSelectedTemporaryItems.LuxEffective,
                               LuxId = CurrentSelectedTemporaryItems.LuxId,
                               LuxIdBackup = CurrentSelectedTemporaryItems.LuxId2,
                               LuxOffValue = CurrentSelectedTemporaryItems.LuxOffValue,
                               LuxOnValue = CurrentSelectedTemporaryItems.LuxOnValue,
                               TimePlanId =
                                   TimeInfoMnVm.tt.Contains(CurrentSelectedTemporaryItems.SchemeId)
                                       ? CurrentSelectedTemporaryItems.SchemeId
                                       : 0,
                               TimePlanName = CurrentSelectedTemporaryItems.SchemeName,
                               TimetablesUseThisPlan = belongtimetable

                           };
            var info = Sr.ProtocolPhone.LxRtuTime.wst_rtutime_temp_time_plan;
            info.WstRtutimeTempTimePlan.Op = 2;
            info.WstRtutimeTempTimePlan.TempTimePlanItems.Add(temp);
            var infoss = WlstMessageBox.Show("确认保存", "即将保存信息，是 继续，否 退出.", WlstMessageBoxType.YesNo);
            if (infoss != WlstMessageBoxResults.Yes) return;
            SndOrderServer.OrderSnd(info, 10, 2);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存 ...";
        }

        #endregion


        private List<int> lstSections = new List<int>();
        /// <summary>
        /// 增加时间段
        /// </summary>
        #region CmdAddTimeSection
        private DateTime _dtAddTimeSection;
        private ICommand _cmdaddtimesection;

        public ICommand CmdAddTimeSection
        {
            get
            {
                return _cmdaddtimesection ??
                       (_cmdaddtimesection = new RelayCommand(ExCmdAddTimeSection, CanExCmdAddTimeSection, true));
            }
        }
        private void ExCmdAddTimeSection()
        {
            _dtAddTimeSection = DateTime.Now;
            if(CurrentSelectedTemporaryItems.SelectedItems.Count!=0)
            {
                WlstMessageBox.Show("无法执行增加 ",
                                    "请先解除时间表绑定! ", WlstMessageBoxType.Ok);
                return;
            }
            lstSections.Clear();
            foreach (var f in CurrentSelectedTemporaryItems.RuleItems)
                if (!lstSections.Contains(f.TimetableSectionId))
                    lstSections.Add(f.TimetableSectionId);
            int max = 1;
            foreach (var f in lstSections) if (f >= max) max = f + 1;
            TimeSpan diff = CurrentSelectedTemporaryItems.DtEndTime - CurrentSelectedTemporaryItems.DtStartTime;
            for (int i = 0; i < diff.TotalDays + 1; i++)
            {
                var tmp = new TimeTableOneDayInfomationItem()
                {
                    TimeAreaId = TimeInfoMnVm.areaId,
                    DateMonth = CurrentSelectedTemporaryItems.DtStartTime.AddDays(i).Month,
                    DateDay = CurrentSelectedTemporaryItems.DtStartTime.AddDays(i).Day,
                    Date = CurrentSelectedTemporaryItems.DtStartTime.AddDays(i).ToString("yyyy.MM.dd"),
                    TimeOff = 1500,
                    TimeOn = 1500,
                    TimetableSectionId = max,
                    RuleSectionId = 1,
                    IsUsedLuxOff = false,
                    IsUsedLuxOn = false,
                    IsUsedOffSet = false,
                    IsUsedOnSet = false,
                    LightOffOffSet = CurrentSelectedTemporaryItems.LightOffOffset,
                    LightOnOffSet = CurrentSelectedTemporaryItems.LightOnOffset,
                    IsEdit = true,
                    TimetableId = CurrentSelectedTemporaryItems.SchemeId,
                    IsTimeOnEnable = true,
                    IsTimeOffEnable = true

                };
                tmp.DayOfWeekUsed = (int)CurrentSelectedTemporaryItems.DtStartTime.AddDays(i).DayOfWeek;
                CurrentSelectedTemporaryItems.RuleItems.Add(tmp);
            }
        }
        private bool CanExCmdAddTimeSection()
        {
            int max = 1;
            lstSections.Clear();
            if (CurrentSelectedTemporaryItems == null)
                return false;
            foreach (var f in CurrentSelectedTemporaryItems.RuleItems)
                if (!lstSections.Contains(f.TimetableSectionId))
                    lstSections.Add(f.TimetableSectionId);
            foreach (var f in lstSections) if (f >= max) max = f + 1;
            bool timeoff = true;
            int timeoffint = 0;
            foreach (var t in CurrentSelectedTemporaryItems.RuleItems)
            {
                //if (t.TimetableSectionId ==max - 1  && (t.TimeOff==1500 || t.TimeOff<t.TimeOn))
                //{
                //    timeoff = false;
                //}
                if (t.TimetableSectionId == max - 1 && t.TimeOff > t.TimeOn && t.TimeOff != 1500 && t.TimeOn != 1500) timeoffint = timeoffint + 1;

                if (t.TimetableSectionId == max - 1 && t.TimeOn != 1500 && t.TimeOff < t.TimeOn) timeoff = false;

            }
            if (timeoffint == 0) timeoff = false;

            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && lstSections.Count < 4 && timeoff
                 && DateTime.Now.Ticks - _dtAddTimeSection.Ticks > 30000000;
        }
        #endregion

        /// <summary>
        /// 删除时间段
        /// </summary>
        #region CmdDeleteTimeSection
        private DateTime _dtDeleteTimeSection;
        private ICommand _cmddeletetimesection;

        public ICommand CmdDeleteTimeSection
        {
            get
            {
                return _cmddeletetimesection ??
                       (_cmddeletetimesection = new RelayCommand(ExCmdDeleteTimeSection, CanExCmdDeleteTimeSection, true));
            }
        }

        private void ExCmdDeleteTimeSection()
        {
            _dtDeleteTimeSection = DateTime.Now;
            if (CurrentSelectedTemporaryItems.SelectedItems.Count != 0)
            {
                WlstMessageBox.Show("无法执行删除 ",
                                    "请先解除时间表绑定! ", WlstMessageBoxType.Ok);
                return;
            }
            if (AddTimeTableSelectItem == null)
            {
                WlstMessageBox.Show("无法执行删除 ",
                                    "请选中需要删除的段，否则无法执行删除! ", WlstMessageBoxType.Ok);
                return;
            }
            else
            {
                var x = WlstMessageBox.Show("确认删除 ",
                                            "是否确认删除段" + AddTimeTableSelectItem.TimetableSectionId + "! ",
                                            WlstMessageBoxType.YesNo);
                if (x == WlstMessageBoxResults.No)
                    return;
            }

            int tt = AddTimeTableSelectItem.TimetableSectionId;
            for (int i = CurrentSelectedTemporaryItems.RuleItems.Count - 1; i >= 0; i--)
            {
                if (CurrentSelectedTemporaryItems.RuleItems[i].TimetableSectionId == tt)
                {
                    CurrentSelectedTemporaryItems.RuleItems.RemoveAt(i);
                }
            }

            foreach (var t in CurrentSelectedTemporaryItems.RuleItems)
            {
                if (t.TimetableSectionId > tt)
                {
                    t.TimetableSectionId = t.TimetableSectionId - 1;
                }
            }

            lstSections.Clear();
            foreach (var f in CurrentSelectedTemporaryItems.RuleItems)
                if (!lstSections.Contains(f.TimetableSectionId))
                    lstSections.Add(f.TimetableSectionId);
            int max = 1;
            foreach (var f in lstSections) if (f >= max) max = f + 1;
        }

        private bool CanExCmdDeleteTimeSection()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 && lstSections.Count > 1 &&
                   DateTime.Now.Ticks - _dtDeleteTimeSection.Ticks > 30000000;
        }

        #endregion

        #region AddTimeTableSelectItem
        private TimeTableOneDayInfomationItem _addtimetableSelectItem = null;

        public TimeTableOneDayInfomationItem AddTimeTableSelectItem
        {
            get { return _addtimetableSelectItem; }
            set
            {
                if (_addtimetableSelectItem == value) return;
                _addtimetableSelectItem = value;
                RaisePropertyChanged(() => AddTimeTableSelectItem);
            }
        }
        #endregion
    }
}
