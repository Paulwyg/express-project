using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Sr.TimeTableSystem.Services.IdServices;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView;
using Wlst.client;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;


namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel
{

    /// <summary>一张时间表中一个规则的开关灯的信息</summary>
    public partial class TimeTableOneDayInfomationItem : EventHandlerHelperExtendNotifyProperyChanged
    {
        public TimeTableOneDayInfomationItem()
        {
        }

        public DateTime datetime;
        public TimeTableOneDayInfomationItem(Wlst.client.TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule itemTable, int dayorweek,int lightonoffset ,int lightoffoffset,int timeid,int areaid,int max,bool isedit)
        {
            int week = (int)DateTime.Now.DayOfWeek;
            this.Date = DateTime.Now.AddDays(dayorweek - week).ToString("MM.dd");
            this.IsEdit = isedit;

            DateMonth = DateTime.Now.AddDays(dayorweek - week).Month;
            DateDay = DateTime.Now.AddDays(dayorweek - week).Day;

            this.TimeOff = itemTable.TimeOff;
            this.TimeOn = itemTable.TimeOn;
            this.TimetableSectionId = itemTable.TimetableSectionId;
            this.RuleSectionId = itemTable.RuleSectionId;

            this.IsUsedLuxOn = itemTable.TypeOn == 1;
            this.IsUsedLuxOff = itemTable.TypeOff == 1;
            this.IsUsedOffSet = itemTable.TypeOff == 2 || this.IsUsedLuxOff;
            this.IsUsedOnSet = itemTable.TypeOn == 2 || this.IsUsedLuxOn;

            this.IsTimeOffEnable = this.IsUsedOffSet == false;
            this.IsTimeOnEnable = this.IsUsedOnSet == false;

            this.DayOfWeekUsed = dayorweek;

            if (this.IsUsedOnSet || this.IsUsedLuxOn) this.LightOnOffSet = lightonoffset;
            else
            {
                this.LightOnOffSet = 0;
                Lightonoffset = lightonoffset;
            }

            if (this.IsUsedOffSet || this.IsUsedLuxOff) this.LightOffOffSet = lightoffoffset;
            else 
            {
                this.LightOffOffSet = 0;
                Lightoffoffset = lightoffoffset;
            }
            
            this.TimetableId = timeid;
            this.TimeAreaId = areaid;
            this.Maxsection = max;
        }

        #region
        private int _timetablesectionid;
        /// <summary>
        /// 时间段序号 1-4
        /// </summary>
        public int TimetableSectionId
        {
            get { return _timetablesectionid; }
            set
            {
                if (_timetablesectionid != value)
                {
                    _timetablesectionid = value;
                    this.RaisePropertyChanged(() => this.TimetableSectionId);
                }
            }
        }

        public int Lightonoffset;
        public int Lightoffoffset;
        public int Maxsection;
        public bool IsEdit;
        public int DateMonth;

        public int DateDay;


        private int _rulesectionid;
        /// <summary>
        /// 规则列表  如第1个规则中的第一选项 可能包含7个，一周每天不一样
        /// </summary>
        public int RuleSectionId
        {
            get { return _rulesectionid; }
            set
            {
                if (_rulesectionid != value)
                {
                    _rulesectionid = value;
                    this.RaisePropertyChanged(() => this.RuleSectionId);
                }
            }
        }

        private bool _isusedluxon;
        /// <summary>
        /// 开灯是否使用光控
        /// </summary>
        public bool IsUsedLuxOn
        {
            get { return _isusedluxon; }
            set
            {
                if (_isusedluxon != value)
                {
                    _isusedluxon = value;

                    this.RaisePropertyChanged(() => this.IsUsedLuxOn);
                    if (IsEdit) InitEvent(1);
                    this.CalculateTimeOn();
                }
            }
        }

        private bool _isusedonset;
        /// <summary>
        /// 开灯是否使用偏移
        /// </summary>
        public bool IsUsedOnSet
        {
            get { return _isusedonset; }
            set
            {
                if (_isusedonset != value)
                {
                    _isusedonset = value;
                    if (IsEdit) InitEvent(2);
                    this.RaisePropertyChanged(() => this.IsUsedOnSet);
                    this.CalculateTimeOn();
                    
                }
            }
        }

        private bool _isusedluxoff;
        /// <summary>
        /// 关灯是否使用光控
        /// </summary>
        public bool IsUsedLuxOff
        {
            get { return _isusedluxoff; }
            set
            {
                if (_isusedluxoff != value)
                {
                    _isusedluxoff = value;
                    if (IsEdit) InitEvent(4);
                    this.RaisePropertyChanged(() => this.IsUsedLuxOff);
                    this.CalculateTimeOff();
                    
                }
            }
        }

        private bool _isusedoffset;
        /// <summary>
        /// 关灯是否使用偏移
        /// </summary>
        public bool IsUsedOffSet
        {
            get { return _isusedoffset; }
            set
            {
                if (_isusedoffset != value)
                {
                    _isusedoffset = value;
                    if (IsEdit) InitEvent(5);
                    this.RaisePropertyChanged(() => this.IsUsedOffSet);
                    this.CalculateTimeOff();
                    
                }
            }
        }

        private int _timeon;
        /// <summary>
        /// 开灯最后时限
        /// </summary>
        public int TimeOn
        {
            get { return _timeon; }
            set
            {
                if (_timeon != value)
                {
                    _timeon = value;

                    if (value > 1500) _timeon = 1500;

                    if (IsEdit) InitEvent(3);
                    this.CalculateLuxTimeOn();
                    
                    this.RaisePropertyChanged(() => this.TimeOn);
                    
                }
            }
        }

        private int _timeoff;
        /// <summary>
        /// 关灯最后时限
        /// </summary>
        public int TimeOff
        {
            get { return _timeoff; }
            set
            {
                if (_timeoff != value)
                {
                    _timeoff = value;

                    if (value > 1500) _timeoff = 1500;

                    if (IsEdit) InitEvent(6);
                    this.CalculateLuxTimeOff();

                    this.RaisePropertyChanged(() => this.TimeOff);
                    
                }
            }
        }

        private int _dayofweekused;
        /// <summary>
        /// 星期 0为周日  1-6 表示星期几  
        /// </summary>
        public int DayOfWeekUsed
        {
            get { return _dayofweekused; }
            set
            {
                if (_dayofweekused != value)
                {
                    _dayofweekused = value;
                    this.RaisePropertyChanged(() => this.DayOfWeekUsed);
                }
            }
        }

        private string _week;
        /// <summary>
        /// 根据dayofweekused转换星期
        /// </summary>
        public string Week
        {
            get { return _week; }
            set
            {
                if (_week != value)
                {
                    _week = value;
                    this.RaisePropertyChanged(() => this.Week);
                }
            }
        }

        private string _date;
        /// <summary>
        /// 日期
        /// </summary>
        public string Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    this.RaisePropertyChanged(() => this.Date);
                }
            }
        }
        #endregion



        #region 辅助显示
        private int _timetableid;
        /// <summary>
        /// 时间表序号
        /// </summary>
        public int TimetableId
        {
            get { return _timetableid; }
            set
            {
                if (_timetableid != value)
                {
                    _timetableid = value;
                    this.RaisePropertyChanged(() => this.TimetableId);
                }
            }
        }

        private int _timeareaid;
        public int TimeAreaId
        {
            get { return _timeareaid; }
            set
            {
                if (_timeareaid != value)
                {
                    _timeareaid = value;
                    this.RaisePropertyChanged(() => this.TimeAreaId);
                }
            }
        }

        private bool _isTimeOnEnable;

        /// <summary>
        /// 界面自定义设定开灯时间是否可修改
        /// </summary>
        public bool IsTimeOnEnable
        {
            get
            {
                return _isTimeOnEnable;
            }
            set
            {
                if (_isTimeOnEnable != value)
                {
                    _isTimeOnEnable = value;
                    this.RaisePropertyChanged(() => this.IsTimeOnEnable);
                }
            }
        }

        private bool _isTimeOffEnable;

        /// <summary>
        /// 界面自定义设定关灯时间是否可修改
        /// </summary>
        public bool IsTimeOffEnable
        {
            get
            {
                return _isTimeOffEnable;
            }
            set
            {
                if (_isTimeOffEnable != value)
                {
                    _isTimeOffEnable = value;
                    this.RaisePropertyChanged(() => this.IsTimeOffEnable);
                }
            }
        }

        private string _msgOnHelp;

        public string MsgOnHelp
        {
            get
            {
                return _msgOnHelp;
            }
            set
            {
                if (_msgOnHelp != value)
                {
                    _msgOnHelp = value;
                    this.RaisePropertyChanged(() => this.MsgOnHelp);
                }
            }
        }

        private string _msgOffHelp;

        public string MsgOffHelp
        {
            get
            {
                return _msgOffHelp;
            }
            set
            {
                if (_msgOffHelp != value)
                {
                    _msgOffHelp = value;
                    this.RaisePropertyChanged(() => this.MsgOffHelp);
                }
            }
        }

        private int _lightOffOffSet;

        /// <summary>
        /// 关灯偏移量
        /// </summary>
        public int LightOffOffSet
        {
            get
            {
                return _lightOffOffSet;
            }
            set
            {
                if (_lightOffOffSet != value)
                {
                    _lightOffOffSet = value;
                    this.RaisePropertyChanged(() => this.LightOffOffSet);
                    this.CalculateTimeOff();
                }
            }
        }

        private int _lightOnOffSet;

        /// <summary>
        /// 开灯偏移量
        /// </summary>
        public int LightOnOffSet
        {
            get
            {
                return _lightOnOffSet;
            }
            set
            {
                if (_lightOnOffSet != value)
                {
                    _lightOnOffSet = value;
                    this.RaisePropertyChanged(() => this.LightOnOffSet);
                    this.CalculateTimeOn();
                }
            }
        }
        #endregion

        #region 计算开关灯时间
        /// <summary>
        /// 计算开灯时间
        /// </summary>
        public void CalculateTimeOn()
        {
            if (IsUsedLuxOn || IsUsedOnSet)
            {
                if (IsUsedLuxOn)
                    IsUsedOnSet  = true;
                IsTimeOnEnable = false;

                var oneDaysOnOffTime = Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                    this.DateMonth, this.DateDay);

                if (oneDaysOnOffTime == null)
                {
                    this._timeon = 25 * 60;
                    this.RaisePropertyChanged(() => this.TimeOn);
                    MsgOnHelp = "无法查阅日出日落时间，自动设为不开灯";
                }
                else 
               {
                   if (LightOnOffSet != 0) this.TimeOn = oneDaysOnOffTime.time_sunset + LightOnOffSet;
                   else this.TimeOn = oneDaysOnOffTime.time_sunset + Lightonoffset;
                }
                    
            }
            else
            {
                IsTimeOnEnable = true;
            }
        }

        public void CalculateLuxTimeOn()
        {
            if (this._timeon ==1500)
            {
                IsUsedLuxOn = false;
                IsUsedOnSet = false;
            }
        }

        /// <summary>
        /// 计算关灯时间
        /// </summary>
        public void CalculateTimeOff()
        {
            //如果光控则 时间为日出日落时间 + 偏移量，并且时间不可修改；
            //如果不是光控 如果偏移量不为0 则使用偏移
            if (IsUsedLuxOff || IsUsedOffSet)
            {
                if (IsUsedLuxOff)
                    IsUsedOffSet = true;

                IsTimeOffEnable = false;
                var oneDaysOnOffTime = Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(this.DateMonth, this.DateDay);
                if (oneDaysOnOffTime == null)
                {
                    this._timeoff = 25 * 60;
                    this.RaisePropertyChanged(() => this.TimeOff);
                    MsgOffHelp = "无法查阅日出日落时间，自动设置为不开灯";
                }
                else
                {
                    if (LightOffOffSet != 0)this.TimeOff = oneDaysOnOffTime.time_sunrise + LightOffOffSet;
                    else this.TimeOff = oneDaysOnOffTime.time_sunrise + Lightoffoffset;
                }
            }
            else
            {
                IsTimeOffEnable = true;
            }
        }

        public void CalculateLuxTimeOff()
        {
            if (this._timeoff == 1500)
            {
                IsUsedLuxOff = false;
                IsUsedOffSet = false;
            }
        }

        #endregion

        #region 批量操作
        private void InitEvent(int i)
        {
            //   this.AddEventFilterInfo(i,"Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel.TimeTableOneDayInfomationItem");
            var args = new PublishEventArgs()
            {
                EventType = "Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel.TimeTableOneDayInfomationItem",
                EventId = 20160704
            };
            args.AddParams(i, this);
            EventPublish.PublishEvent(args);
        }
        #endregion







    }

    public class WeekSndReport : EventHandlerHelperExtendNotifyProperyChanged
    {
        public WeekSndReport()
        {

        }

        public WeekSndReport(int _rtuId, int _phyId, string _name, string _status, long _weekAck)
        {
            RtuId = _rtuId;
            PhysicalId = _phyId;
            NodeName = _name;
            State = _status;
            WeekAck = _weekAck;

            var infox = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(RtuId);

            int m = 0;

            if(infox.RtuModel == EnumRtuModel.Wj3006)
            {
                for (int i = 0; i < 3; i++)
                {
                    long x = ((_weekAck >> i) & 0x00000001);

                    if (x != 0)
                    {
                        m++;
                    }
                }

                if (m == 3)
                {
                    WeekSndAns = "√";
                }
                else
                {
                    WeekSndAns = m + " / 3";
                }

                if (WeekAck == 0)
                {
                    WeekSndAns = "0 / 3";
                }
                else if (WeekAck == 2)
                {
                    WeekSndAns = "2 / 3";
                }
                else if (WeekAck == 1)
                {
                    WeekSndAns = "1 / 3";
                }
                else if (WeekAck == 3)
                {
                    WeekSndAns = "√";
                }
            }
            else if (infox.RtuModel == EnumRtuModel.Wj3005)
            {
                for (int i = 0; i < 2; i++)
                {
                    long x = ((_weekAck >> i) & 0x00000001);

                    if (x != 0)
                    {
                        m++;
                    }
                }

                if (m == 2)
                {
                    WeekSndAns = "√";
                }
                else
                {
                    WeekSndAns = m + " / 2";
                }

                if (WeekAck == 0)
                {
                    WeekSndAns = "0 / 2";
                }
                else if (WeekAck == 3)
                {
                    WeekSndAns = "√";
                }
                else if (WeekAck == 1)
                {
                    WeekSndAns = "1 / 2";
                }
            }

          
        }

        private int _rtuId;
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value == _rtuId) return;
                _rtuId = value;
                RaisePropertyChanged(() => RtuId);
            }
        }

        private int _physicalId;
        public int PhysicalId
        {
            get { return _physicalId; }
            set
            {
                if (value == _physicalId) return;
                _physicalId = value;
                RaisePropertyChanged(() => PhysicalId);
            }
        }


        private string _nodeName;
        public string NodeName
        {
            get { return _nodeName; }
            set
            {
                if (_nodeName != value)
                {
                    _nodeName = value;
                    this.RaisePropertyChanged(() => this.NodeName);
                }
            }
        }

        private string _state;
        public string State
        {
            get { return _state; }
            set
            {
                if (value == _state) return;
                _state = value;
                RaisePropertyChanged(() => State);
            }
        }

        private long _weekAck;

        public long WeekAck
        {
            get { return _weekAck; }
            set
            {
                if (_weekAck == value) return;
                _weekAck = value;
                RaisePropertyChanged(() => WeekAck);
            }
        }

        private string _weekSndAns;
        public string WeekSndAns
        {
            get { return _weekSndAns; }
            set
            {
                if (_weekSndAns == value) return;
                _weekSndAns = value;
                RaisePropertyChanged(() => WeekSndAns);
            }
        }

    }

    public partial class WeekReport : EventHandlerHelperExtendNotifyProperyChanged
    {
        private ObservableCollection<WeekSndReport> _WeekSndReport = null;

        public ObservableCollection<WeekSndReport> xWeekSndReport
        {
            get
            {
                if (_WeekSndReport == null) _WeekSndReport = new ObservableCollection<WeekSndReport>();
                return _WeekSndReport;
            }
            set
            {
                if (_WeekSndReport == value) return;
                _WeekSndReport = value;
                RaisePropertyChanged(() => xWeekSndReport);
            }
        }


        private int _remindOcTmlCount;
        public int OcTmlCount
        {
            get { return _remindOcTmlCount; }
            set
            {
                if (_remindOcTmlCount == value) return;
                _remindOcTmlCount = value;
                RaisePropertyChanged(() => OcTmlCount);
            }
        }

        private int _remindOcCount;
        public int OcCount
        {
            get { return _remindOcCount; }
            set
            {
                if (_remindOcCount == value) return;
                _remindOcCount = value;
                RaisePropertyChanged(() => OcCount);
            }
        }
    }

    /// <summary>一张时间表的信息</summary>
    public partial class TimeTableInfomationItem : EventHandlerHelperExtendNotifyProperyChanged
    {
        public TimeTableInfomationItem()
        {

        }

        private int AreaId;
        public bool LuxChanged = false;

        public TimeTableInfomationItem(Wlst.client.TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem itemTable, int areaId)
        {
            InitEvent();
            AreaId = areaId;
            IsEdit = false;
            this.LuxEffective = itemTable.LuxEffective;
            this.LightOffOffset = itemTable.LightOffOffset;
            this.LightOnOffset = itemTable.LightOnOffset;
            this.LuxId = itemTable.LuxId;
            LuxChanged = true;
            this.LuxOffValue = itemTable.LuxOffValue;
            this.LuxOnValue = itemTable.LuxOnValue;
            var plans = (from tt in TimeTabletemporaryHold.Myself.Info
                         where tt.Key.Item1 == TimeInfoMnVm.areaId
                         orderby tt.Key.Item2 ascending
                         select tt.Value).ToList();
            foreach (var t in plans)
            {
                foreach (var tt in t.TimetablesUseThisPlan)
                {
                    if (tt == itemTable.TimeId)
                         this.TimeDesc1= "(" + t.TimePlanName + ")";
                }
            }
            this.TimeDesc = itemTable.TimeDesc;
            this.TimeId = itemTable.TimeId;
            this.TimeName = itemTable.TimeName;
            
            this.LuxId2 = itemTable.LuxIdBackup;

            if (LuxId2 == 0)
                ShowCurrentSelectLux2 = 0;
            else
                ShowCurrentSelectLux2 = 22;

            int max = 0;
            
            //if (this.LuxId != 0)
            //    this.LuxName = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[this.LuxId].RtuName;

            RuleItems.Clear();
            foreach (var t in itemTable.RuleItems)
            {
                foreach (var tt in t.DayOfWeekUsed)
                {
                    RuleItems.Add(new TimeTableOneDayInfomationItem(t, tt, LightOnOffset, LightOffOffset, TimeId, AreaId,
                                                                    max, IsEdit));
                }
            }

        

            lstSections.Clear();
            MainRuleItems.Clear();


            var dic = new Dictionary<int, List<int>>();

            //try
            //{
            //    max = RuleItems.Count/7;
            //}
            //catch 
            //{
            foreach (var f in RuleItems)
            {
                if (!lstSections.Contains(f.TimetableSectionId))
                    lstSections.Add(f.TimetableSectionId);
                if (f.TimetableSectionId > max)
                    max = f.TimetableSectionId ;
            }
            //}


            foreach (var t in RuleItems)
            {
                t.Maxsection = max;
                //for (int i = 0; i < 7; i++)
                //{
                    if (dic.ContainsKey(t.TimetableSectionId)) dic[t.TimetableSectionId].Add(t.DayOfWeekUsed);
                    else dic.Add(t.TimetableSectionId, new List<int>() { t.DayOfWeekUsed });
                //}
            }

            for (int i = 1; i < 5; i++)
            {
                if (dic.ContainsKey(i) == false) continue;
                var list = new List<int> {0, 1, 2, 3, 4, 5, 6};
                var expectedList = list.Except(dic[i]);

                for (int j = 0; j < 7; j++)
                {
                    int week = (int) DateTime.Now.DayOfWeek;
                    if (expectedList.Contains(j))
                    {
                        var tmp = new TimeTableOneDayInfomationItem()
                                      {
                                          DayOfWeekUsed = j,
                                          RuleSectionId = 1,
                                          TimeOff = 1500,
                                          TimeOn = 1500,
                                          TimetableSectionId = i,
                                          IsUsedLuxOff = false,
                                          IsUsedLuxOn = false,
                                          IsUsedOffSet = false,
                                          IsUsedOnSet = false,
                                          Date = DateTime.Now.AddDays(j - week).ToString("MM.dd"),
                                          DateMonth = DateTime.Now.AddDays(j - week).Month,
                                          DateDay = DateTime.Now.AddDays(j - week).Day,
                                          Maxsection = max
                                      };
                        RuleItems.Add(tmp);
                    }
                }
            }

            var ruleitemslist = (from t in RuleItems select t).ToList();
            var ruleitemslistorder = (from t in ruleitemslist orderby t.DayOfWeekUsed, t.TimetableSectionId select t).ToList();

            RuleItems.Clear();
            foreach (var t in ruleitemslistorder)
            {
                RuleItems.Add(t);
            }

            MainRuleTermporaryItemsCalulate(max, TimeId);

            MainIsOverOne = new ObservableCollection<bool>() { false, false, false };
            for (int i = 0; i < max - 1; i++)
            {
                MainIsOverOne[i] = true;
            }
        }

        public Wlst.client.TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem BackToWeekTimeTableInforemation()
        {
            var tmp = new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem()
            {
                RuleItems = new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule>(),
                LightOffOffset = this.LightOffOffset,
                LightOnOffset = this.LightOnOffset,


                LuxId = this.LuxId,
                LuxIdBackup = this.LuxId2,
                
                LuxEffective = this.LuxEffective,
                TimeDesc = this.TimeDesc,
                TimeId = this.TimeId,
                TimeName = this.TimeName,
                LuxOffValue = this.LuxOffValue,
                LuxOnValue = this.LuxOnValue
            };
            
            //key:TypeOn,TypeOff,TimeOn,TimeOff;value:DayOfWeekUsed,TimetableSectionId,RuleSectionId,TimeON,TimeOff
            Dictionary<Tuple<int,int, int, int, int>, Tuple<List<int>, int, int, int, int>> dic =
                new Dictionary<Tuple<int,int, int, int, int>, Tuple<List<int>, int, int, int, int>>();
            foreach (var f in RuleItems)
            {
                if (f.TimeOn == 1500 && f.TimeOff == 1500) continue;
                int on, off = 0;
                if (f.IsUsedLuxOn) on = 1;
                else if (f.IsUsedOnSet) on = 2;
                else on = 3;
                if (f.IsUsedLuxOff) off = 1;
                else if (f.IsUsedOffSet) off = 2;
                else off = 3;

                var key = new Tuple<int, int, int, int,int>(f.TimetableSectionId,on, off, on == 3 ? f.TimeOn : 0, off == 3 ? f.TimeOff : 0);


                if (dic.ContainsKey(key))
                {
                    dic[key].Item1.Add(f.DayOfWeekUsed);
                }
                else
                {
                    var dayofweekused = new List<int>();
                    dayofweekused.Add(f.DayOfWeekUsed);
                    var value = new Tuple<List<int>, int, int, int, int>(dayofweekused, f.TimetableSectionId, f.RuleSectionId, f.TimeOn, f.TimeOff);
                    dic.Add(key, value);
                }
            }

            foreach (var f in dic)
            {
                if (f.Value.Item4 == 1500 && f.Value.Item5 == 1500) continue;

                tmp.RuleItems.Add(new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem.TimeTableSectionRule()
                {
                    TimeOn = f.Value.Item4,
                    TimeOff = f.Value.Item5,
                    TimetableSectionId = f.Value.Item2,
                    TypeOff = f.Key.Item3,
                    TypeOn = f.Key.Item2,
                    RuleSectionId = f.Value.Item3,
                    RuleId = 1,
                    DateEnd = 1231,
                    DateStart = 101,
                    DayOfWeekUsed = f.Value.Item1
                });
            }


            return tmp;



        }

        #region CmdAdd

        private ICommand _cmdaddtimesection;

        public ICommand CmdAdd
        {
            get
            {
                return _cmdaddtimesection ??
                       (_cmdaddtimesection = new RelayCommand(ExCmdAddTimeSection, CanExCmdAddTimeSection, true));
            }
        }

        private DateTime _dtAdd;

        private void ExCmdAddTimeSection()
        {
            lstSections.Clear();
            foreach (var f in RuleItems)
                if (!lstSections.Contains(f.TimetableSectionId))
                    lstSections.Add(f.TimetableSectionId);

            int max = 1;
            foreach (var f in lstSections) if (f >= max) max = f + 1;
            this.MainIsOverOne = new ObservableCollection<bool>() { false, false, false };
            for (int i = 0; i < max - 1; i++)
            {
                this.MainIsOverOne[i] = true;
            }

            //if (max == 1)
            //{
            //    MainType = new ObservableCollection<int>() { 55, 75, 160, 30, 50, 160 };
            //}
            //else if (max == 2)
            //{
            //    MainType = new ObservableCollection<int>() { 30, 50, 105, 30, 50, 80 };
            //}
            //else
            //{
            //    MainType = new ObservableCollection<int>() { 25, 45, 100, 30, 50, 75 };
            //}
            if (max == 1)
            {
                MainType = new ObservableCollection<int>() { 100, 150, 250, 30, 50, 160 };
                //MainScrollBar = "Hidden";
            }
            else if (max == 2)
            {
                MainType = new ObservableCollection<int>() { 64, 128, 150, 30, 50, 80 };
                //MainScrollBar = "Hidden";
            }
            else
            {
                MainType = new ObservableCollection<int>() { 65, 75, 120, 30, 50, 75 };
                //MainScrollBar = "Visible";
            }

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
                    TimetableSectionId = max,
                    TypeOff = 3,
                    TypeOn = 3,
                };
                tmp.DayOfWeekUsed.Add(i);
                
                this.RuleItems.Add(new TimeTableOneDayInfomationItem(tmp, i, LightOnOffset, LightOffOffset,TimeId,AreaId,max,true));
                
            }


        }

        

        private bool CanExCmdAddTimeSection()
        {
            int max = 1;
            lstSections.Clear();
            foreach (var f in RuleItems)
                if (!lstSections.Contains(f.TimetableSectionId))
                    lstSections.Add(f.TimetableSectionId);
            foreach (var f in lstSections) if (f >= max) max = f + 1;
            bool timeoff = true;
            int timeoffint = 0;
            foreach (var t in RuleItems)
            {
                //if (t.TimetableSectionId ==max - 1  && (t.TimeOff==1500 || t.TimeOff<t.TimeOn))
                //{
                //    timeoff = false;
                //}
                if (t.TimetableSectionId == max - 1 && t.TimeOff > t.TimeOn && t.TimeOff!=1500 &&t.TimeOn!=1500) timeoffint = timeoffint + 1;

                if (t.TimetableSectionId == max - 1 && t.TimeOn != 1500 && t.TimeOff < t.TimeOn) timeoff = false;

            }
            if (timeoffint == 0) timeoff = false;

            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(AreaId) && lstSections.Count < 4 && timeoff
                 &&  DateTime.Now.Ticks - _dtAdd.Ticks > 30000000;
        }

        private List<int> lstSections = new List<int>();
        #endregion

        #region CmdDelete

        private ICommand _cmddeletetimesection;

        public ICommand CmdDelete
        {
            get
            {
                return _cmddeletetimesection ??
                       (_cmddeletetimesection = new RelayCommand(ExCmdDeleteTimeSection, CanExCmdDeleteTimeSection, true));
            }
        }

        private DateTime _dtdelete;

        private void ExCmdDeleteTimeSection()
        {
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
            for (int i = RuleItems.Count - 1; i >= 0; i--)
            {
                if (RuleItems[i].TimetableSectionId == tt)
                {
                    RuleItems.RemoveAt(i);
                }
            }

            foreach (var t in RuleItems)
            {
                if (t.TimetableSectionId > tt)
                {
                    t.TimetableSectionId = t.TimetableSectionId - 1;
                }
            }

            lstSections.Clear();
            foreach (var f in RuleItems)
                if (!lstSections.Contains(f.TimetableSectionId))
                    lstSections.Add(f.TimetableSectionId);
            int max = 1;
            foreach (var f in lstSections) if (f >= max) max = f + 1;

            this.MainIsOverOne = new ObservableCollection<bool>() { false, false, false };
            for (int i = 0; i < max - 2; i++)
            {
                this.MainIsOverOne[i] = true;
            }

            if (max == 1)
            {
                MainType = new ObservableCollection<int>() { 55, 75, 160, 30, 50, 160 };
            }
            else if (max == 2)
            {
                MainType = new ObservableCollection<int>() { 30, 50, 105, 30, 50, 80 };
            }
            else
            {
                MainType = new ObservableCollection<int>() { 25, 45, 100, 30, 50, 75 };
            }


        }

        private bool CanExCmdDeleteTimeSection()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(AreaId) && lstSections.Count > 1 &&
                   DateTime.Now.Ticks - _dtdelete.Ticks > 30000000;
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

        #region

        private bool _isedit;

        /// <summary>
        /// 是否编辑
        /// </summary>
        public bool IsEdit
        {
            get { return _isedit; }
            set
            {
                if (_isedit != value)
                {
                    _isedit = value;
                    this.RaisePropertyChanged(() => this.IsEdit);
                    foreach (var t in RuleItems)
                    {
                        t.IsEdit = _isedit;
                    }
                }
            }
        }

        private int _timeid;

        /// <summary>
        /// 时间表ID
        /// </summary>
        public int TimeId
        {
            get { return _timeid; }
            set
            {
                if (_timeid != value)
                {
                    _timeid = value;
                    this.RaisePropertyChanged(() => this.TimeId);
                }
            }
        }

        private string _luxname;

        /// <summary>
        /// 光控名称
        /// </summary>
        public string LuxName
        {
            get { return _luxname; }
            set
            {
                if (_luxname != value)
                {
                    _luxname = value;
                    this.RaisePropertyChanged(() => this.LuxName);
                }
            }
        }

        private string _timename;

        /// <summary>
        /// 时间表名称
        /// </summary>
        public string TimeName
        {
            get { return _timename; }
            set
            {
                if (_timename != value)
                {
                    _timename = value;
                    this.RaisePropertyChanged(() => this.TimeName);
                }
            }
        }

        private string _timedesc;

        /// <summary>
        /// 时间表描述
        /// </summary>
        public string TimeDesc
        {
            get { return _timedesc; }
            set
            {
                if (_timedesc != value)
                {
                    _timedesc = value;
                    this.RaisePropertyChanged(() => this.TimeDesc);
                }
            }
        }

        private string _timedesc1;

        /// <summary>
        /// 时间表描述1
        /// </summary>
        public string TimeDesc1
        {
            get { return _timedesc1; }
            set
            {
                if (_timedesc1 != value)
                {
                    _timedesc1 = value;
                    this.RaisePropertyChanged(() => this.TimeDesc1);
                }
            }
        }

        #region LuxCollection

        private ObservableCollection<IdNameDesc> _luxCollection;
        public ObservableCollection<IdNameDesc> LuxCollection
        {
            get
            {
                if (_luxCollection == null)
                {
                    _luxCollection = new ObservableCollection<IdNameDesc>();
                    _luxCollection.Add(new IdNameDesc { Id = 0, Name = " " });
                    foreach (var t in LuxGetServer.GetAllLuxEquipment)
                    {
                        _luxCollection.Add(new IdNameDesc { Id = t.Value, Name = t.Name,NameDesc = t.Value2.ToString("d4") +"-"+t.Name});
                    }
                    if (LuxId > 0)
                    {
                        foreach (var t in _luxCollection.Where(t => t.Id == LuxId))
                        {
                            CurrentSelectLux = t;
                            break;
                        }
                    }
                }
                return _luxCollection;
            }
        }
        #endregion

        #region CurrentSelectLux
        private IdNameDesc _currentSelectLux;

        /// <summary>
        /// 当前选中的光控
        /// </summary>
        public IdNameDesc CurrentSelectLux
        {
            get { return _currentSelectLux ?? (_currentSelectLux = new IdNameDesc()); }
            set
            {
                if (_currentSelectLux == value) return;
                _currentSelectLux = value;
                RaisePropertyChanged(() => CurrentSelectLux);
                if (_currentSelectLux != null)
                    LuxId = _currentSelectLux.Id;


                //if (_currentSelectLux.Id>0)
                //    ShowCurrentSelectLux2 = Visibility.Visible;
                //else
                //    ShowCurrentSelectLux2 = Visibility.Hidden;

            }
        }

        #endregion

        #region LuxCollection2

        private ObservableCollection<IdNameDesc> _luxCollection2;
        public ObservableCollection<IdNameDesc> LuxCollection2
        {
            get
            {
                if (_luxCollection2 == null)
                {
                    _luxCollection2 = new ObservableCollection<IdNameDesc>();
                    _luxCollection2.Add(new IdNameDesc { Id = 0, Name = " " });
                    foreach (var t in LuxGetServer.GetAllLuxEquipment)
                    {
                        if (t.Value != CurrentSelectLux.Id)
                        {
                            _luxCollection2.Add(new IdNameDesc { Id = t.Value, Name = t.Name, NameDesc = t.Value2.ToString("d4") + "-" + t.Name });
                        }

                    }


                    if (LuxId2 != 0)
                    {
                        foreach (var t in _luxCollection2.Where(t => t.Id == LuxId2))
                        {
                            CurrentSelectLux2 = t;
                            break;
                        }
                    }
                }



                return _luxCollection2;
            }
        }
        #endregion

        #region CurrentSelectLux2
        private IdNameDesc _currentSelectLux2;

        /// <summary>
        /// 备用光控
        /// </summary>
        public IdNameDesc CurrentSelectLux2
        {
            get { return _currentSelectLux2 ?? (_currentSelectLux = new IdNameDesc()); }
            set
            {
                if (_currentSelectLux2 == value) return;
                _currentSelectLux2 = value;
                RaisePropertyChanged(() => CurrentSelectLux2);
                if (_currentSelectLux2 != null)
                    LuxId2 = _currentSelectLux2.Id;
            }
        }

        #endregion

        #region ShowCurrentSelectLux2
        private int _showcurrentSelectLux2;

        /// <summary>
        /// 备用光控
        /// </summary>
        public int ShowCurrentSelectLux2
        {
            get { return _showcurrentSelectLux2; }
            set
            {
                if (_showcurrentSelectLux2 != value)
                {
                    _showcurrentSelectLux2 = value;
                    this.RaisePropertyChanged(() => this.ShowCurrentSelectLux2);
                }
            }
        }

        #endregion

        private int _luxid;

        /// <summary>
        /// 该时间表使用的光控探头ID
        /// </summary>
        public int LuxId
        {
            get
            {
                return _luxid;
            }
            set
            {
                if (_luxid == value) return;
                _luxid = value;
                foreach (var t in LuxCollection.Where(t => t.Id == value))
                {
                    CurrentSelectLux = t;
                    LuxName = t.Name;
                    LuxChanged = false;
                    break;
                }
                RaisePropertyChanged(() => LuxId);

                if (_luxid > 0)
                    ShowCurrentSelectLux2 = 22;
                else
                    ShowCurrentSelectLux2 = 0;
            }
        }

        private int _luxid2;

        /// <summary>
        /// 该时间表使用的光控探头ID
        /// </summary>
        public int LuxId2
        {
            get
            {
                return _luxid2;
            }
            set
            {
                if (_luxid2 == value) return;
                _luxid2 = value;
                foreach (var t in LuxCollection2.Where(t => t.Id == value))
                {
                    CurrentSelectLux2 = t;
                    LuxName2 = t.Name;
                    break;
                }
                RaisePropertyChanged(() => LuxId2);
            }
        }

        private string _luxname2;

        /// <summary>
        /// 光控名称
        /// </summary>
        public string LuxName2
        {
            get { return _luxname2; }
            set
            {
                if (_luxname2 != value)
                {
                    _luxname2 = value;
                    this.RaisePropertyChanged(() => this.LuxName2);
                }
            }
        }

        private int _luxonvalue;

        /// <summary>
        /// 该时间表若是使用光控则开灯光照度值
        /// </summary>
        public int LuxOnValue
        {
            get { return _luxonvalue; }
            set
            {
                if (_luxonvalue != value)
                {
                    if (value < 0)
                    {
                        value = 15;
                    }
                    _luxonvalue = value;
                    this.RaisePropertyChanged(() => this.LuxOnValue);
                }
            }
        }

        private int _luxoffvalue;

        /// <summary>
        /// 该时间表若是使用光照度关灯 则关灯光照度值
        /// </summary>
        public int LuxOffValue
        {
            get { return _luxoffvalue; }
            set
            {
                if (_luxoffvalue != value)
                {
                    if (value < 0)
                    {
                        value = 15;
                    }
                    _luxoffvalue = value;
                    this.RaisePropertyChanged(() => this.LuxOffValue);
                }
            }
        }

        private int _lighronoffset;

        /// <summary>
        /// 如果该时间表使用偏移  则开灯偏移值 0不启用偏移 定时； 不为0则启用偏移  根据日出日落计算
        /// </summary>
        public int LightOnOffset
        {
            get { return _lighronoffset; }
            set
            {
                if (_lighronoffset != value)
                {
                    if (value < -60 || value > 60)
                    {
                        value = 15;
                    }
                    _lighronoffset = value;
                    this.RaisePropertyChanged(() => this.LightOnOffset);
                    foreach (var t in RuleItems)
                    {
                        t.LightOnOffSet = this.LightOnOffset;
                    }
                }
            }
        }

        private int _lighroffoffset;

        /// <summary>
        /// 如果该时间表使用偏移 则关灯偏移值为；0不启用偏移 定时；不为0则启用偏移  根据日出日落计算
        /// </summary>
        public int LightOffOffset
        {
            get { return _lighroffoffset; }
            set
            {
                if (_lighroffoffset != value)
                {
                    if (value < -60 || value > 60)
                    {
                        value = -15;
                    }
                    _lighroffoffset = value;
                    this.RaisePropertyChanged(() => this.LightOffOffset);
                    foreach(var t in RuleItems)
                    {
                        t.LightOffOffSet = this.LightOffOffset;
                    }
                }
            }
        }

        private int _luxeffective;

        /// <summary>
        /// 光控有效值
        /// </summary>
        public int LuxEffective
        {
            get { return _luxeffective; }
            set
            {
                if (_luxeffective != value)
                {
                    if (value > 60)
                    {
                        value = 30;
                    }
                    _luxeffective = value;

                    this.RaisePropertyChanged(() => this.LuxEffective);
                }
            }
        }


        private ObservableCollection<TimeTableOneDayInfomationItem> _ruleitems;

        /// <summary>
        /// 一周开关灯规则
        /// </summary>
        public ObservableCollection<TimeTableOneDayInfomationItem> RuleItems
        {
            get
            {
                if (_ruleitems == null) _ruleitems = new ObservableCollection<TimeTableOneDayInfomationItem>();
                return _ruleitems;

            }
            set
            {
                if (_ruleitems != value)
                {
                    _ruleitems = value;
                    this.RaisePropertyChanged(() => this.RuleItems);
                    //int max = 0;
                    //foreach (var t in RuleItems)
                    //{
                    //    if (t.TimetableSectionId > max)
                    //        max = t.TimetableSectionId;
                    //}
                    //MainRuleItemsCalulate(max);
                }
            }
        }
        #endregion

        #region 表格排序
        private ICommand _cmdorder;
        public ICommand CmdOrder
        {
            get { return _cmdorder ?? (_cmdorder = new RelayCommand<string>(ExCmdOrder, CanCmdOrder, true)); }
        }


        private long lastexute = 0;
        private int lastexutetpara = 0;

        private void ExCmdOrder(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            lastexute = DateTime.Now.Ticks;
            lastexutetpara = x;

            CmdOrderBy(x);
        }

        private bool CanCmdOrder(string str)
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(AreaId) &&
                   DateTime.Now.Ticks - lastexute > 30000;
        }

        private void CmdOrderBy(int x)
        {
            if (x < 1 || x > 2) return;

            var ruleitemslist = (from t in RuleItems select t).ToList();

            RuleItems.Clear();

            if (x == 1)
            {
                var tmp1 = (from t in ruleitemslist orderby t.DayOfWeekUsed, t.TimetableSectionId select t).ToList();
                foreach (var f in tmp1) RuleItems.Add(f);
            }
            else if (x == 2)
            {
                var tmp1 = (from t in ruleitemslist orderby t.TimetableSectionId, t.DayOfWeekUsed select t).ToList();
                foreach (var f in tmp1) RuleItems.Add(f);
            }

        }






        #endregion

        #region 批量操作
        private bool _isChecked;

        /// <summary>
        /// 是否选中该条数据
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                }
            }
        }

        void InitEvent()
        {
            this.AddEventFilterInfo(20160704,
                                     "Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel.TimeTableOneDayInfomationItem");
        }

        //private int LightOpenCloseProtect = 5;

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            //base.ExPublishedEvent(args);

            if (args.GetParams().Count < 2) return;
            int obj1 = 0;
            try
            {
                obj1 = Convert.ToInt32(args.GetParams()[0]);
            }
            catch (Exception)
            {
            }
            var listtime = new List<int> { 0, 0, 0, 0 };


            var obj2 = args.GetParams()[1] as TimeTableOneDayInfomationItem;
            if (obj2 == null) return;

            switch (obj1)
            {
                case 1:
                    if (!_isChecked) break;
                    if (obj2.TimetableId == TimeId && obj2.TimeAreaId == AreaId)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.IsUsedLuxOn = obj2.IsUsedLuxOn;
                        }
                    }
                    break;
                case 2:
                    if (!_isChecked) break;
                    if (obj2.TimetableId == TimeId && obj2.TimeAreaId == AreaId)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.IsUsedOnSet = obj2.IsUsedOnSet;
                        }
                    }
                    break;
                case 3:
                    #region
                    //if (obj2.TimetableId == TimeId)
                    //{
                    //    int max = 1;
                    //    for (int i = 0; i < 3; i++)
                    //    {
                    //        if (MainIsOverOne[i]) max = max + 1;
                    //    }
                    //    if (RuleItems.Count == 7*max)
                    //    {


                    //        var dic = new Dictionary<int, List<int>>();
                    //        var lst = new List<int>(); //光控开，开，光控关，关

                    //        var ruleitemslistorder =
                    //            (from t in RuleItems orderby t.DayOfWeekUsed, t.TimetableSectionId select t).ToList();
                    //        var RuleItemsOnce = new ObservableCollection<TimeTableOneDayInfomationItem>();
                    //        foreach (var f in ruleitemslistorder) RuleItemsOnce.Add(f);


                    //        foreach (var t in RuleItemsOnce)
                    //        {

                    //            lst = new List<int>();

                    //            if (t.IsUsedLuxOn) lst.Add(t.TimeOn - this.LuxEffective);
                    //            else lst.Add(1500);

                    //            if (t.TimeOn == 1500) lst.Add(1500);
                    //            else if (t.TimeOn > 1440)
                    //                lst.Add(t.TimeOn - 1440);
                    //            else if (t.TimeOn == 1440)
                    //                lst.Add(t.TimeOn  - 1440 + 1);
                    //            else lst.Add(t.TimeOn );

                    //            if (t.IsUsedLuxOff) lst.Add(t.TimeOff - this.LuxEffective);
                    //            else lst.Add(1500);

                    //            if (t.TimeOff == 1500) lst.Add(1500);
                    //            else if (t.TimeOff > 1440 )
                    //                lst.Add(t.TimeOff  - 1440);
                    //            else if (t.TimeOff == 1440)
                    //                lst.Add(t.TimeOff  - 1440 + 1);
                    //            else lst.Add(t.TimeOff );

                    //            if (dic.ContainsKey(t.DayOfWeekUsed))
                    //            {
                    //                var lit = new List<int>(dic[t.DayOfWeekUsed]);
                    //                for (int i = 0; i < lst.Count; i++)
                    //                {
                    //                    lit.Add(lst[i]);
                    //                }
                    //                dic[t.DayOfWeekUsed] = new List<int>(lit);
                    //            }
                    //            else
                    //            {
                    //                dic.Add(t.DayOfWeekUsed, lst);
                    //            }
                    //        }
                    //        var listnow = new List<int>(dic[obj2.DayOfWeekUsed]);
                    //        int weekafter = new int();
                    //        if (obj2.DayOfWeekUsed == 0) weekafter = 6;
                    //        else weekafter = obj2.DayOfWeekUsed - 1;
                    //        int weekbefore = new int();
                    //        if (obj2.DayOfWeekUsed == 0) weekbefore = 6;
                    //        else weekbefore = obj2.DayOfWeekUsed - 1;
                    //        var listbefore = new List<int>(dic[weekbefore]);
                    //        var listafter = new List<int>(dic[weekafter]);

                    //        for (int i = 0; i < listnow.Count; i = i + 4)
                    //        {
                    //             //第一段
                                
                    //                if (i == 0 && listbefore[listbefore.Count - 1] < listbefore[listbefore.Count - 3] &&
                    //                    listnow[0] == 1500 && obj2.TimetableSectionId * 4 == i + 4) //before跨天,开无光控
                    //                {
                    //                    if (listbefore[listbefore.Count - 2] == 1500 &&
                    //                        listnow[1] > listbefore[listbefore.Count - 1] ) obj2.TimeOn = 1500;
                    //                    else if (listbefore[listbefore.Count - 2] != 1500 &&
                    //                             (listnow[1] > listbefore[listbefore.Count - 3] ||
                    //                              listnow[1] > listbefore[listbefore.Count - 2])) obj2.TimeOn = 1500;
                    //                }
                    //                else if (i == 0 && listbefore[listbefore.Count - 1] < listbefore[listbefore.Count - 3] &&
                    //                         listnow[0] == 1500 && obj2.TimetableSectionId * 4 == i + 4) //before跨天,开有光控
                    //                {
                    //                    if (listbefore[listbefore.Count - 2] == 1500 &&
                    //                        (listnow[1] > listbefore[listbefore.Count - 3] ||
                    //                         listnow[0] > listbefore[listbefore.Count - 3])) obj2.TimeOn = 1500;
                    //                    else if (listbefore[listbefore.Count - 2] != 1500 &&
                    //                             (listnow[1] > listbefore[listbefore.Count - 3] ||
                    //                              listnow[1] > listbefore[listbefore.Count - 2] ||
                    //                              listnow[0] > listbefore[listbefore.Count - 3] ||
                    //                              listnow[0] > listbefore[listbefore.Count - 2]))
                    //                        obj2.TimeOn = 1500;
                    //                }
                                
                    //            else if (i == listnow.Count - 4 && listnow[i + 3] < listnow[i + 1] && obj2.TimetableSectionId * 4 == i + 4) //最后一段，关跨天
                    //            {
                    //                if (listnow[i] == 1500 && listnow[i - 2] == 1500 && listnow[i - 1] > listnow[i + 1])
                    //                    obj2.TimeOn = 1500;
                    //                else if (listnow[i] == 1500 && listnow[i - 2] != 1500 &&
                    //                         (listnow[i - 1] > listnow[i + 1] || listnow[i - 2] > listnow[i + 1]))
                    //                    obj2.TimeOn = 1500;
                    //                else if (listnow[i] != 1500 && listnow[i - 2] == 1500 &&
                    //                         (listnow[i - 1] > listnow[i + 1] || listnow[i - 1] > listnow[i]))
                    //                    obj2.TimeOn = 1500;
                    //                else if (listnow[i] != 1500 && listnow[i - 2] == 1500 &&
                    //                         (listnow[i - 1] > listnow[i + 1] || listnow[i - 2] > listnow[i + 1] ||
                    //                          listnow[i - 1] > listnow[i] || listnow[i - 2] > listnow[i]))
                    //                    obj2.TimeOn = 1500;
                    //            }
                    //            else if (listnow[i + 3] > listnow[i + 1] && listnow[i] == 1500 && obj2.TimetableSectionId * 4 == i + 4) //关未跨天,开无光控
                    //            {
                    //                if (listnow[i + 2] == 1500 && listnow[i + 1] > listnow[i + 3]) obj2.TimeOn = 1500;
                    //                else if (listnow[i + 2] != 1500 &&
                    //                         (listnow[i + 1] > listnow[i + 3] || listnow[i + 1] > listnow[i + 2]))
                    //                    obj2.TimeOn = 1500;
                    //            }
                    //            else if (listnow[i + 3] > listnow[i + 1] && listnow[i] != 1500 && obj2.TimetableSectionId * 4 == i + 4) //关未跨天,开有光控
                    //            {
                    //                if (listnow[i + 2] == 1500 &&
                    //                    (listnow[i + 1] > listnow[i + 3] || listnow[i + 0] > listnow[i + 3]))
                    //                    obj2.TimeOn = 1500;
                    //                else if (listnow[i + 2] != 1500 &&
                    //                         (listnow[i + 1] > listnow[i + 3] || listnow[i + 1] > listnow[i + 2] ||
                    //                          listnow[i + 0] > listnow[i + 3] || listnow[i + 0] > listnow[i + 2]))
                    //                    obj2.TimeOn = 1500;
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion
                    #region
                    //if (obj2.TimetableId == TimeId)
                    //{
                    //    var dic3 = new Dictionary<int, List<int>>();
                    //    int max = 1;
                    //    //int LightOpenCloseProtect = 5;
                    //    for (int i = 0; i < 3; i++)
                    //    {
                    //        if (MainIsOverOne[i]) max = max + 1;
                    //    }
                    //    if (RuleItems.Count == 7 * max && obj2.TimeAreaId == AreaId)
                    //    {

                    //        var lst = new List<int>(); //光控开，开，光控关，关

                    //        var ruleitemslistorder =
                    //            (from t in RuleItems orderby t.DayOfWeekUsed, t.TimetableSectionId select t).ToList();
                    //        var RuleItemsOnce = new ObservableCollection<TimeTableOneDayInfomationItem>();
                    //        foreach (var f in ruleitemslistorder) RuleItemsOnce.Add(f);


                    //        foreach (var t in RuleItemsOnce)
                    //        {

                    //            lst = new List<int>();

                    //            if (t.IsUsedLuxOn) lst.Add(t.TimeOn - LuxEffective);
                    //            else
                    //            {
                    //                if (t.TimeOn == 1500) lst.Add(1500);
                    //                else if (t.TimeOn > 1440)
                    //                    lst.Add(t.TimeOn - 1440);
                    //                else if (t.TimeOn == 1440)
                    //                    lst.Add(t.TimeOn - 1440 + 1);
                    //                else lst.Add(t.TimeOn);
                    //            }

                    //            if (t.TimeOn == 1500) lst.Add(1500);
                    //            else if (t.TimeOn > 1440)
                    //                lst.Add(t.TimeOn - 1440);
                    //            else if (t.TimeOn == 1440)
                    //                lst.Add(t.TimeOn - 1440 + 1);
                    //            else lst.Add(t.TimeOn);

                    //            if (t.IsUsedLuxOff) lst.Add(t.TimeOff - LuxEffective);
                    //            else
                    //            {
                    //                if (t.TimeOff == 1500) lst.Add(1500);
                    //                else if (t.TimeOff > 1440)
                    //                    lst.Add(t.TimeOff - 1440);
                    //                else if (t.TimeOff == 1440)
                    //                    lst.Add(t.TimeOff - 1440 + 1);
                    //                else lst.Add(t.TimeOff);
                    //            }

                    //            if (t.TimeOff == 1500) lst.Add(1500);
                    //            else if (t.TimeOff > 1440)
                    //                lst.Add(t.TimeOff - 1440);
                    //            else if (t.TimeOff == 1440)
                    //                lst.Add(t.TimeOff - 1440 + 1);
                    //            else lst.Add(t.TimeOff);

                    //            if (dic3.ContainsKey(t.DayOfWeekUsed))
                    //            {
                    //                var lit = new List<int>(dic3[t.DayOfWeekUsed]);
                    //                for (int i = 0; i < lst.Count; i++)
                    //                {
                    //                    lit.Add(lst[i]);
                    //                }
                    //                dic3[t.DayOfWeekUsed] = new List<int>(lit);
                    //            }
                    //            else
                    //            {
                    //                dic3.Add(t.DayOfWeekUsed, lst);
                    //            }
                    //        }

                    //        var listnow = new List<int>(dic3[obj2.DayOfWeekUsed]);
                    //        //int weekafter = new int();
                    //        //if (obj2.DayOfWeekUsed == 0) weekafter = 6;
                    //        //else weekafter = obj2.DayOfWeekUsed - 1;
                    //        //int weekbefore = new int();
                    //        //if (obj2.DayOfWeekUsed == 0) weekbefore = 6;
                    //        //else weekbefore = obj2.DayOfWeekUsed - 1;
                    //        //var listbefore = new List<int>(dic3[weekbefore]);
                    //        //var listafter = new List<int>(dic3[weekafter]);

                    //        //if (listnow[listnow.Count - 1] == 1500 && listnow[listnow.Count - 2] == 1500 &&
                    //        //listnow[listnow.Count - 3] == 1500 && listnow[listnow.Count - 4] == 1500 && listnow.Count > 5)
                    //        //{
                    //        //    listnow.RemoveAt(listnow.Count - 1);
                    //        //    listnow.RemoveAt(listnow.Count - 2);
                    //        //    listnow.RemoveAt(listnow.Count - 3);
                    //        //    listnow.RemoveAt(listnow.Count - 4);
                    //        //}

                    //        for (int j = 2; j < obj2.TimetableSectionId * 4 - 2; j = j + 2)
                    //        {
                    //            if (j > 2)
                    //            {
                    //                if (listnow[j - 1] < listnow[j + 1] && listnow[j - 2] < listnow[j + 1] &&
                    //                    listnow[j - 1] < listnow[j] && listnow[j - 2] < listnow[j])
                    //                {
                    //                }
                    //                else
                    //                {
                    //                    obj2.TimeOn = 1500;
                    //                }
                    //            }
                    //            //else
                    //            //{
                    //            //    if (listnow[1] != 1500 && listnow[1] < listnow[3] && listnow[0] < listnow[3] &&
                    //            //        listnow[1] < listnow[2] && listnow[0] < listnow[2])
                    //            //    {
                    //            //    }
                    //            //    else if (listnow[1] == 1500 && listbefore[listbefore.Count - 1] < listnow[3] &&
                    //            //             listbefore[listbefore.Count - 2] < listnow[3]
                    //            //             && listbefore[listbefore.Count - 2] < listnow[2] &&
                    //            //             listbefore[listbefore.Count - 1] < listnow[2])
                    //            //    {
                    //            //    }
                    //            //    else if (listnow[2] == 1500 && listnow[3] == 1500 &&
                    //            //        listnow[0] > listbefore[listbefore.Count - 1] && listnow[0] > listbefore[listbefore.Count - 2]
                    //            //        && listnow[1] > listbefore[listbefore.Count - 1] && listnow[1] > listbefore[listbefore.Count - 2])
                    //            //    {
                    //            //    }
                    //            //    else
                    //            //    {
                    //            //        obj2.TimeOn = 1500;
                    //            //    }
                    //            //}

                    //            //if (j == listnow.Count - 4)
                    //            //{
                    //            //    if ((listnow[j + 3] < listnow[j + 1] && listnow[j + 3] < listafter[1] &&
                    //            //         listnow[j + 3] < listafter[2] && listnow[j + 2] < listafter[1] &&
                    //            //         listnow[j + 2] < listafter[2]) ||
                    //            //        (listnow[j + 3] > listnow[j + 1] && listnow[j + 3] > listnow[j] &&
                    //            //         listnow[j + 2] > listnow[j + 1] && listnow[j + 2] > listnow[j]))
                    //            //    {
                    //            //    }
                    //            //    else
                    //            //    {
                    //            //        obj2.TimeOn = 1500;
                    //            //    }
                    //            //}
                    //        }
                    //    }


                    //}
                    #endregion

                    #region
                    if (obj2.TimetableId == TimeId && obj2.TimeAreaId == AreaId && IsEdit == true)
                    {
                        if (obj2.TimetableSectionId > 1)
                        {
                            foreach (var t in RuleItems)
                            {
                                if (obj2.TimetableSectionId == t.TimetableSectionId + 1 && obj2.DayOfWeekUsed == t.DayOfWeekUsed && t.Maxsection == obj2.Maxsection)
                                {
                                    if (obj2.TimeOn <= t.TimeOff) obj2.TimeOn = 1500;
                                }
                            }
                        }
                    }
                    #endregion


                    if (!_isChecked) break;
                    if (obj2.TimetableId == TimeId && obj2.TimeAreaId == AreaId)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (obj2.IsUsedOnSet == true) continue;
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.TimeOn = obj2.TimeOn;
                        }
                    }
                    break;
                case 4:
                    if (!_isChecked) break;
                    if (obj2.TimetableId == TimeId && obj2.TimeAreaId == AreaId)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.IsUsedLuxOff = obj2.IsUsedLuxOff;
                        }
                    }
                    break;
                case 5:
                    if (!_isChecked) break;
                    if (obj2.TimetableId == TimeId && obj2.TimeAreaId == AreaId)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.IsUsedOffSet = obj2.IsUsedOffSet;
                        }
                    }
                    break;
                case 6:
                    #region
                    //if (obj2.TimetableId==TimeId)
                    //{
                    //    int max1 = 1;
                    //    for (int i = 0; i < 3; i++)
                    //    {
                    //        if (MainIsOverOne[i]) max1 = max1 + 1;
                    //    }
                    //    if (RuleItems.Count == 7 * max1)
                    //    {
                    //        var dic = new Dictionary<int, List<int>>();
                    //        var lst = new List<int>(); //光控开，开，光控关，关
                           
                    //        var ruleitemslistorder =
                    //            (from t in RuleItems orderby t.DayOfWeekUsed, t.TimeOn select t).ToList();
                    //        var RuleItemsOnce = new ObservableCollection<TimeTableOneDayInfomationItem>();
                    //        foreach (var f in ruleitemslistorder) RuleItemsOnce.Add(f);


                    //        foreach (var t in RuleItemsOnce)
                    //        {

                    //            lst = new List<int>();

                    //            if (t.IsUsedLuxOn) lst.Add(t.TimeOn - this.LuxEffective);
                    //            else lst.Add(1500);

                    //            if (t.TimeOn == 1500) lst.Add(1500);
                    //            else if (t.TimeOn > 1440 )
                    //                lst.Add(t.TimeOn  - 1440);
                    //            else if (t.TimeOn == 1440 )
                    //                lst.Add(t.TimeOn  - 1440 + 1);
                    //            else lst.Add(t.TimeOn );

                    //            if (t.IsUsedLuxOff) lst.Add(t.TimeOff - this.LuxEffective );
                    //            else lst.Add(1500);

                    //            if (t.TimeOff == 1500) lst.Add(1500);
                    //            else if (t.TimeOff > 1440 )
                    //                lst.Add(t.TimeOff  - 1440);
                    //            else if (t.TimeOff == 1440 )
                    //                lst.Add(t.TimeOff  - 1440 + 1);
                    //            else lst.Add(t.TimeOff );

                    //            if (dic.ContainsKey(t.DayOfWeekUsed))
                    //            {
                    //                var lit = new List<int>(dic[t.DayOfWeekUsed]);
                    //                for (int i = 0; i < lst.Count; i++)
                    //                {
                    //                    lit.Add(lst[i]);
                    //                }
                    //                dic[t.DayOfWeekUsed] = new List<int>(lit);
                    //            }
                    //            else
                    //            {
                    //                dic.Add(t.DayOfWeekUsed, lst);
                    //            }
                    //        }
                    //        var listnow = new List<int>(dic[obj2.DayOfWeekUsed]);
                    //        int weekafter = new int();
                    //        if (obj2.DayOfWeekUsed == 0) weekafter = 6;
                    //        else weekafter = obj2.DayOfWeekUsed - 1;
                    //        int weekbefore = new int();
                    //        if (obj2.DayOfWeekUsed == 0) weekbefore = 6;
                    //        else weekbefore = obj2.DayOfWeekUsed - 1;
                    //        var listbefore = new List<int>(dic[weekbefore]);
                    //        var listafter = new List<int>(dic[weekafter]);

                    //        for (int i = 0; i < listnow.Count; i = i + 4)
                    //        {
                    //            if (i == 0 && listnow[1] == 1500 && listnow[2] == 1500 &&
                    //                (listbefore[listbefore.Count - 1] < listbefore[listbefore.Count - 3] && listbefore[listbefore.Count - 3] != 1500) && obj2.TimetableSectionId*4==i+4)
                    //            //一段开未设置，关无光控，before跨天
                    //            {
                    //                if (listbefore[listbefore.Count - 2] == 1500 &&
                    //                    listnow[3] <= listbefore[listbefore.Count - 1]) obj2.TimeOff = 1500;
                    //                else if (listbefore[listbefore.Count - 2] != 1500 &&
                    //                         (listnow[3] <= listbefore[listbefore.Count - 1] ||
                    //                          listnow[3] <= listbefore[listbefore.Count - 2])) obj2.TimeOff = 1500;
                    //            }
                    //            else if (i == 0 && listnow[1] == 1500 && listnow[2] != 1500 &&
                    //                     (listbefore[listbefore.Count - 1] < listbefore[listbefore.Count - 3] && listbefore[listbefore.Count - 3] != 1500) && obj2.TimetableSectionId * 4 == i + 4)
                    //            //一段开未设置，关有光控，before跨天
                    //            {
                    //                if (listbefore[listbefore.Count - 2] == 1500 &&
                    //                    (listnow[3] <= listbefore[listbefore.Count - 1] ||
                    //                     listnow[2] <= listbefore[listbefore.Count - 1])) obj2.TimeOff = 1500;
                    //                else if (listbefore[listbefore.Count - 2] != 1500 &&
                    //                         (listnow[3] <= listbefore[listbefore.Count - 1] ||
                    //                          listnow[3] <= listbefore[listbefore.Count - 2] ||
                    //                          listnow[2] <= listbefore[listbefore.Count - 1] ||
                    //                          listnow[2] <= listbefore[listbefore.Count - 2])) obj2.TimeOff = 1500;
                    //            }

                    //            else if (i == listnow.Count - 4 && listnow[i + 3] < listnow[i + 1] && obj2.TimetableSectionId * 4 == i + 4) //最后段跨天
                    //            {
                    //                if (listafter[0] == 1500 && listnow[i + 3] > listafter[1]) obj2.TimeOff = 1500;
                    //                else if (listafter[0] != 1500 &&
                    //                         (listnow[i + 3] > listafter[1] || listnow[i + 3] > listafter[0]))
                    //                    obj2.TimeOff = 1500;
                    //            }
                    //            else if (listnow[i + 2] == 1500 && obj2.TimetableSectionId * 4 == i + 4) //关无光控
                    //            {
                    //                if (listnow[i] != 1500 &&
                    //                    (listnow[i + 3] <= listnow[i + 1] || listnow[i + 3] <= listnow[i + 0]))
                    //                    obj2.TimeOff = 1500;
                    //                else if (listnow[i] == 1500 && listnow[i + 3] <= listnow[i + 1])
                    //                    obj2.TimeOff = 1500;
                    //            }
                    //            else if (listnow[i + 2] != 1500 && obj2.TimetableSectionId * 4 == i + 4) //关有光控
                    //            {
                    //                if (listnow[i] != 1500 &&
                    //                    (listnow[i + 3] <= listnow[i + 1] || listnow[i + 3] <= listnow[i + 0]
                    //                     || listnow[i + 2] <= listnow[i + 1] || listnow[i + 2] <= listnow[i + 0]))
                    //                    obj2.TimeOff = 1500;
                    //                else if (listnow[i] == 1500 &&
                    //                         (listnow[i + 3] <= listnow[i + 1] || listnow[i + 2] <= listnow[i + 1]))
                    //                    obj2.TimeOff = 1500;
                    //            }
                    //        }


                    //    }
                    //}
                    #endregion

                    #region
                    //if (obj2.TimetableId == TimeId)
                    //{
                    //    var dic3 = new Dictionary<int, List<int>>();
                    //    int max = 1;
                    //    //int LightOpenCloseProtect = 5;
                    //    for (int i = 0; i < 3; i++)
                    //    {
                    //        if (MainIsOverOne[i]) max = max + 1;
                    //    }
                    //    if (RuleItems.Count == 7 * max && obj2.TimeAreaId==AreaId)
                    //    {

                    //        var lst = new List<int>(); //光控开，开，光控关，关

                    //        var ruleitemslistorder =
                    //            (from t in RuleItems orderby t.DayOfWeekUsed, t.TimetableSectionId select t).ToList();
                    //        var RuleItemsOnce = new ObservableCollection<TimeTableOneDayInfomationItem>();
                    //        foreach (var f in ruleitemslistorder) RuleItemsOnce.Add(f);


                    //        foreach (var t in RuleItemsOnce)
                    //        {

                    //            lst = new List<int>();

                    //            if (t.IsUsedLuxOn) lst.Add(t.TimeOn - LuxEffective);
                    //            else
                    //            {
                    //                if (t.TimeOn == 1500) lst.Add(1500);
                    //                else if (t.TimeOn > 1440)
                    //                    lst.Add(t.TimeOn - 1440);
                    //                else if (t.TimeOn == 1440)
                    //                    lst.Add(t.TimeOn - 1440 + 1);
                    //                else lst.Add(t.TimeOn);
                    //            }

                    //            if (t.TimeOn == 1500) lst.Add(1500);
                    //            else if (t.TimeOn > 1440)
                    //                lst.Add(t.TimeOn - 1440);
                    //            else if (t.TimeOn == 1440)
                    //                lst.Add(t.TimeOn - 1440 + 1);
                    //            else lst.Add(t.TimeOn);

                    //            if (t.IsUsedLuxOff) lst.Add(t.TimeOff - LuxEffective);
                    //            else
                    //            {
                    //                if (t.TimeOff == 1500) lst.Add(1500);
                    //                else if (t.TimeOff > 1440)
                    //                    lst.Add(t.TimeOff - 1440);
                    //                else if (t.TimeOff == 1440)
                    //                    lst.Add(t.TimeOff - 1440 + 1);
                    //                else lst.Add(t.TimeOff);
                    //            }

                    //            if (t.TimeOff == 1500) lst.Add(1500);
                    //            else if (t.TimeOff > 1440)
                    //                lst.Add(t.TimeOff - 1440);
                    //            else if (t.TimeOff == 1440)
                    //                lst.Add(t.TimeOff - 1440 + 1);
                    //            else lst.Add(t.TimeOff);

                    //            if (dic3.ContainsKey(t.DayOfWeekUsed))
                    //            {
                    //                var lit = new List<int>(dic3[t.DayOfWeekUsed]);
                    //                for (int i = 0; i < lst.Count; i++)
                    //                {
                    //                    lit.Add(lst[i]);
                    //                }
                    //                dic3[t.DayOfWeekUsed] = new List<int>(lit);
                    //            }
                    //            else
                    //            {
                    //                dic3.Add(t.DayOfWeekUsed, lst);
                    //            }
                    //        }


                    //        var listnow = new List<int>(dic3[obj2.DayOfWeekUsed]);
                    //        //int weekafter = new int();
                    //        //if (obj2.DayOfWeekUsed == 0) weekafter = 6;
                    //        //else weekafter = obj2.DayOfWeekUsed - 1;
                    //        //int weekbefore = new int();
                    //        //if (obj2.DayOfWeekUsed == 0) weekbefore = 6;
                    //        //else weekbefore = obj2.DayOfWeekUsed - 1;
                    //        //var listbefore = new List<int>(dic3[weekbefore]);
                    //        //var listafter = new List<int>(dic3[weekafter]);

                    //        //if (listnow[listnow.Count - 1] == 1500 && listnow[listnow.Count - 2] == 1500 &&
                    //        //    listnow[listnow.Count - 3] == 1500 && listnow[listnow.Count - 4] == 1500 && listnow.Count > 5)
                    //        //{
                    //        //    listnow.RemoveAt(listnow.Count - 1);
                    //        //    listnow.RemoveAt(listnow.Count - 2);
                    //        //    listnow.RemoveAt(listnow.Count - 3);
                    //        //    listnow.RemoveAt(listnow.Count - 4);
                    //        //}


                    //        for (int j = 2; j < obj2.TimetableSectionId * 4 - 2; j = j + 2)
                    //        {
                    //            if (j > 2)
                    //            {
                    //                if (listnow[j - 1] < listnow[j + 1] && listnow[j - 2] < listnow[j + 1] &&
                    //                    listnow[j - 1] < listnow[j] && listnow[j - 2] < listnow[j])
                    //                {
                    //                }
                    //                else
                    //                {
                    //                    obj2.TimeOff = 1500;
                    //                }
                    //            }
                    //            //else
                    //            //{
                    //            //    if (listnow[1] != 1500 && listnow[1] < listnow[3] && listnow[0] < listnow[3] &&
                    //            //        listnow[1] < listnow[2] && listnow[0] < listnow[2])
                    //            //    {
                    //            //    }
                    //            //    else if (listnow[1] == 1500 && listbefore[listbefore.Count - 1] < listnow[3] &&
                    //            //             listbefore[listbefore.Count - 2] < listnow[3]
                    //            //             && listbefore[listbefore.Count - 2] < listnow[2] &&
                    //            //             listbefore[listbefore.Count - 1] < listnow[2])
                    //            //    {
                    //            //    }
                    //            //    else
                    //            //    {
                    //            //        obj2.TimeOff = 1500;
                    //            //    }
                    //            //}

                    //            //if (j == listnow.Count - 4)
                    //            //{
                    //            //    if ((listnow[j + 3] < listnow[j + 1] && listnow[j + 3] < listafter[1] &&
                    //            //         listnow[j + 3] < listafter[0] && listnow[j + 2] < listafter[1] &&
                    //            //         listnow[j + 2] < listafter[0] && listafter[1] != 1500) ||
                    //            //        (listnow[j + 3] > listnow[j + 1] && listnow[j + 3] > listnow[j] &&
                    //            //         listnow[j + 2] > listnow[j + 1] && listnow[j + 2] > listnow[j]))
                    //            //    {
                    //            //    }
                    //            //    else
                    //            //    {
                    //            //        obj2.TimeOff = 1500;
                    //            //    }
                    //            //}
                    //        }
                    //    }
                    //}
#endregion

                    #region
                    //if (obj2.TimetableId == TimeId && obj2.TimeAreaId == AreaId)
                    //{
                    //    foreach (var t in RuleItems)
                    //    {
                    //        if (obj2.TimetableSectionId == t.TimetableSectionId && obj2.DayOfWeekUsed == t.DayOfWeekUsed && t.Maxsection == obj2.Maxsection )
                    //        {
                    //            if (obj2.TimeOff <= t.TimeOn && t.TimeOn!=1500) obj2.TimeOff = 1500;
                    //        }
                    //    }
                    //}
                    #endregion




                    if (!_isChecked) break;
                    if (obj2.TimetableId == TimeId && obj2.TimeAreaId == AreaId && IsEdit == true)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (obj2.IsUsedOffSet == true) continue;
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.TimeOff = obj2.TimeOff;
                        }
                    }
                    break;
                default:
                    break;

            }
        }
        #endregion

        #region 主界面显示

        private ObservableCollection<MainRuleItemsStyle> _mainruleitems;

        public ObservableCollection<MainRuleItemsStyle> MainRuleItems
        {
            get
            {
                if (_mainruleitems == null)
                {
                    _mainruleitems = new ObservableCollection<MainRuleItemsStyle>();
                }
                return _mainruleitems;
            }
            set
            {
                if (_mainruleitems != value)
                {
                    _mainruleitems = value;
                    this.RaisePropertyChanged(() => this.MainRuleItems);
                }
            }

        }

        private ObservableCollection<bool> _mainisoverone;
        public ObservableCollection<bool> MainIsOverOne
        {
            get
            {
                return _mainisoverone;
            }
            set
            {
                if (_mainisoverone != value)
                {
                    _mainisoverone = value;
                    this.RaisePropertyChanged(() => this.MainIsOverOne);
                }
            }
        }

        //private ObservableCollection<Visibility> _mainisovertwo;
        //public ObservableCollection<Visibility> MainIsOverTwo
        //{
        //    get
        //    {
        //        return _mainisovertwo;
        //    }
        //    set
        //    {
        //        if (_mainisovertwo != value)
        //        {
        //            _mainisovertwo = value;
        //            this.RaisePropertyChanged(() => this.MainIsOverTwo);
        //        }
        //    }
        //}


        private ObservableCollection<int> _maintype;
        public ObservableCollection<int> MainType
        {
            get
            {
                return _maintype;
            }
            set
            {
                if (_maintype != value)
                {
                    _maintype = value;
                    this.RaisePropertyChanged(() => this.MainType);
                }
            }
        }

        //private string _mainscrollbar;
        //public string MainScrollBar
        //{
        //    get
        //    {
        //        return _mainscrollbar;
        //    }
        //    set
        //    {
        //        if (_mainscrollbar != value)
        //        {
        //            _mainscrollbar = value;
        //            this.RaisePropertyChanged(() => this.MainScrollBar);
        //        }
        //    }
        //}

        public void MainRuleItemsCalulate(int max)
        {
            var ruleitemslist = (from t in RuleItems select t).ToList();
            var ruleitemslistorder =
                (from t in ruleitemslist orderby t.DayOfWeekUsed , t.TimetableSectionId select t).ToList();

            MainIsOverOne = new ObservableCollection<bool>() {false, false, false}; //第二段，第三段，第四段，没有第三段
            for (int i = 0; i < max - 1; i++)
            {
                MainIsOverOne[i] = true;
            }


            if (max == 1)
            {
                MainType = new ObservableCollection<int>() { 100, 150, 250, 30, 50, 160};
                //MainScrollBar = "Hidden";
            }
            else if (max == 2)
            {
                MainType = new ObservableCollection<int>() { 64, 128, 150, 30, 50, 80};
                //MainScrollBar = "Hidden";
            }
            else
            {
                MainType = new ObservableCollection<int>() { 65, 75, 120, 30, 50, 75};
                //MainScrollBar = "Visible";
            }


            if (max == 1 || max == 2 || max == 3 || max == 4)
            {
                if (ruleitemslistorder.Count == 7*max)
                {
                    for (int i = 0; i < ruleitemslistorder.Count; i = i + max)
                    {
                        string date = ruleitemslistorder[i].Date;
                        int week = ruleitemslistorder[i].DayOfWeekUsed;

                        if (Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                            ruleitemslistorder[i].DateMonth, ruleitemslistorder[i].DateDay) == null)
                            return;
                        string sunrise =
                            MainCalculateTime(
                                Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                    ruleitemslistorder[i].DateMonth, ruleitemslistorder[i].DateDay).time_sunrise);
                        string sunset =
                            MainCalculateTime(
                                Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                    ruleitemslistorder[i].DateMonth, ruleitemslistorder[i].DateDay).time_sunset);

                        var tu = MainCalculateTimeOnOff(ruleitemslistorder[i].IsUsedLuxOn,
                                                        ruleitemslistorder[i].IsUsedLuxOff,
                                                        ruleitemslistorder[i].IsUsedOnSet,
                                                        ruleitemslistorder[i].IsUsedOffSet,
                                                        ruleitemslistorder[i].TimeOn,
                                                        ruleitemslistorder[i].TimeOff);

                        string timeonone = tu.Item1;
                        string timeoffone = tu.Item2;
                        string timeontwo = null;
                        string timeofftwo = null;
                        string timeonthree = null;
                        string timeoffthree = null;
                        string timeonfour = null;
                        string timeofffour = null;
                        if (max == 1)
                        {
                            timeontwo = null;
                            timeofftwo = null;
                            timeonthree = null;
                            timeoffthree = null;
                            timeonfour = null;
                            timeofffour = null;
                        }
                        else
                        {
                            tu = MainCalculateTimeOnOff(ruleitemslistorder[i + 1].IsUsedLuxOn,
                                                        ruleitemslistorder[i + 1].IsUsedLuxOff,
                                                        ruleitemslistorder[i + 1].IsUsedOnSet,
                                                        ruleitemslistorder[i + 1].IsUsedOffSet,
                                                        ruleitemslistorder[i + 1].TimeOn,
                                                        ruleitemslistorder[i + 1].TimeOff);
                            timeontwo = tu.Item1;
                            timeofftwo = tu.Item2;
                            timeonthree = null;
                            timeoffthree = null;
                            timeonfour = null;
                            timeofffour = null;

                            if (max == 3 || max == 4)
                            {
                                tu = MainCalculateTimeOnOff(ruleitemslistorder[i + 2].IsUsedLuxOn,
                                                            ruleitemslistorder[i + 2].IsUsedLuxOff,
                                                            ruleitemslistorder[i + 2].IsUsedOnSet,
                                                            ruleitemslistorder[i + 2].IsUsedOffSet,
                                                            ruleitemslistorder[i + 2].TimeOn,
                                                            ruleitemslistorder[i + 2].TimeOff);
                                timeonthree = tu.Item1;
                                timeoffthree = tu.Item2;
                                timeonfour = null;
                                timeofffour = null;
                                if (max == 4)
                                {
                                    tu = MainCalculateTimeOnOff(ruleitemslistorder[i + 3].IsUsedLuxOn,
                                                                ruleitemslistorder[i + 3].IsUsedLuxOff,
                                                                ruleitemslistorder[i + 3].IsUsedOnSet,
                                                                ruleitemslistorder[i + 3].IsUsedOffSet,
                                                                ruleitemslistorder[i + 3].TimeOn,
                                                                ruleitemslistorder[i + 3].TimeOff);
                                    timeonfour = tu.Item1;
                                    timeofffour = tu.Item2;
                                }
                            }
                        }


                        MainRuleItems.Add(new MainRuleItemsStyle()
                                              {
                                                  MainWeek = week,
                                                  MainDate = date,
                                                  MainTimeOnOne = timeonone,
                                                  MainTimeOffOne = timeoffone,
                                                  MainTimeOnTwo = timeontwo,
                                                  MainTimeOffTwo = timeofftwo,
                                                  MainTimeOnThree = timeonthree,
                                                  MainTimeOffThree = timeoffthree,
                                                  MainTimeOnFour = timeonfour,
                                                  MainTimeOffFour = timeofffour,
                                                  MainSunRise = sunrise,
                                                  MainSunSet = sunset
                                              });
                    }

                }
            }
        }

        //加了临时方案  2018/4/23
        public void MainRuleTermporaryItemsCalulate(int max, int timeId)
        {
            var ruleitemslist = (from t in RuleItems select t).ToList();
            var ruleitemslistorder =
                (from t in ruleitemslist orderby t.DayOfWeekUsed , t.TimetableSectionId select t).ToList();

            MainIsOverOne = new ObservableCollection<bool>() {false, false, false}; //第二段，第三段，第四段，没有第三段
            for (int i = 0; i < max - 1; i++)
            {
                MainIsOverOne[i] = true;
            }


            //if (max == 1)
            //{
            //    MainType = new ObservableCollection<int>() {55, 75, 160, 30, 50, 160};
            //    //MainScrollBar = "Hidden";
            //}
            //else if (max == 2)
            //{
            //    MainType = new ObservableCollection<int>() {30, 50, 105, 30, 50, 80};
            //    //MainScrollBar = "Hidden";
            //}
            //else
            //{
            //    MainType = new ObservableCollection<int>() {25, 45, 100, 30, 50, 75};
            //    //MainScrollBar = "Visible";
            //}
            if (max == 1)
            {
                MainType = new ObservableCollection<int>() { 100, 150, 250, 30, 50, 160 };
                //MainScrollBar = "Hidden";
            }
            else if (max == 2)
            {
                MainType = new ObservableCollection<int>() { 64, 128, 150, 30, 50, 80 };
                //MainScrollBar = "Hidden";
            }
            else
            {
                MainType = new ObservableCollection<int>() { 65, 75, 120, 30, 50, 75 };
                //MainScrollBar = "Visible";
            }
            var plans = (from tt in TimeTabletemporaryHold.Myself.Info
                         where tt.Key.Item1 == TimeInfoMnVm.areaId
                         orderby tt.Key.Item2 ascending
                         select tt.Value).ToList();

            if (max == 1 || max == 2 || max == 3 || max == 4)
            {
                if (ruleitemslistorder.Count == 7*max)
                {
                    //int j = 0;
                    for (int i = 0; i < ruleitemslistorder.Count; i = i + max)
                    {
                        string date = ruleitemslistorder[i].Date;
                        int week = ruleitemslistorder[i].DayOfWeekUsed;

                        if (Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                            ruleitemslistorder[i].DateMonth, ruleitemslistorder[i].DateDay) == null)
                            return;
                        string sunrise =
                            MainCalculateTime(
                                Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                    ruleitemslistorder[i].DateMonth, ruleitemslistorder[i].DateDay).time_sunrise);
                        string sunset =
                            MainCalculateTime(
                                Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                    ruleitemslistorder[i].DateMonth, ruleitemslistorder[i].DateDay).time_sunset);

                        //判断是否有临时方案  2018/4/19

                        var tu = new Tuple<string, string>("", "");
                        string timeonone = null;
                        string timeoffone = null;
                        string timeonone1 = null;
                        string timeoffone1 = null;
                        string timeontwo = null;
                        string timeofftwo = null;
                        string timeontwo1 = null;
                        string timeofftwo1 = null;
                        string timeonthree = null;
                        string timeoffthree = null;
                        string timeonthree1 = null;
                        string timeoffthree1 = null;
                        string timeonfour = null;
                        string timeofffour = null;
                        string timeonfour1 = null;
                        string timeofffour1 = null;
                        foreach (var t in plans)
                        {
                            foreach (var tt in t.TimetablesUseThisPlan)
                            {

                                //lvf  2019年2月7日14:35:24  如果不包含该时间表 pass
                                if (tt != timeId) continue;


                                int j = 0;
                                foreach (var ttt in t.ItemsPlan)
                                {
                                    var itemslist = t.ItemsPlan.OrderBy(u => u.Date).ThenBy(u=>u.SectionId).ToList();
                                    if ( ttt.Date.ToString().Substring(4, 4) == date.Replace(".", "")) //tt == timeId &&
                                    {

                                        tu = MainCalculateTimeOnOff(itemslist[j].TypeOn == 1, itemslist[j].TypeOff == 1,
                                                                    itemslist[j].TypeOn == 2 || itemslist[j].TypeOn == 1,
                                                                    itemslist[j].TypeOff == 2 ||
                                                                    itemslist[j].TypeOff == 1,
                                                                    itemslist[j].TimeOn, itemslist[j].TimeOff);
                                        timeonone1 = tu.Item1;
                                        timeoffone1 = tu.Item2;
                                        if (max == 1)
                                        {
                                            timeontwo1 = null;
                                            timeofftwo1 = null;
                                            timeonthree1 = null;
                                            timeoffthree1 = null;
                                            timeonfour1 = null;
                                            timeofffour1 = null;
                                        }
                                        else
                                        {
                                            tu = MainCalculateTimeOnOff(itemslist[j + 1].TypeOn == 1,
                                                                        itemslist[j + 1].TypeOff == 1,
                                                                        itemslist[j + 1].TypeOn == 2 ||
                                                                        itemslist[j + 1].TypeOn == 1,
                                                                        itemslist[j + 1].TypeOff == 2 ||
                                                                        itemslist[j + 1].TypeOff == 1,
                                                                        itemslist[j + 1].TimeOn,
                                                                        itemslist[j + 1].TimeOff);
                                            timeontwo1 = tu.Item1;
                                            timeofftwo1 = tu.Item2;
                                            timeonthree1 = null;
                                            timeoffthree1 = null;
                                            timeonfour1 = null;
                                            timeofffour1 = null;

                                            if (max == 3 || max == 4)
                                            {
                                                tu = MainCalculateTimeOnOff(itemslist[j + 2].TypeOn == 1,
                                                                            itemslist[j + 2].TypeOff == 1,
                                                                            itemslist[j + 2].TypeOn == 2 ||
                                                                            itemslist[j + 2].TypeOn == 1,
                                                                            itemslist[j + 2].TypeOff == 2 ||
                                                                            itemslist[j + 2].TypeOff == 1,
                                                                            itemslist[j + 2].TimeOn,
                                                                            itemslist[j + 2].TimeOff);
                                                timeonthree1 = tu.Item1;
                                                timeoffthree1 = tu.Item2;
                                                timeonfour1 = null;
                                                timeofffour1 = null;
                                                if (max == 4)
                                                {
                                                    tu = MainCalculateTimeOnOff(itemslist[j + 3].TypeOn == 1,
                                                                                itemslist[j + 3].TypeOff == 1,
                                                                                itemslist[j + 3].TypeOn == 2 ||
                                                                                itemslist[j + 3].TypeOn == 1,
                                                                                itemslist[j + 3].TypeOff == 2 ||
                                                                                itemslist[j + 3].TypeOff == 1,
                                                                                itemslist[j + 3].TimeOn,
                                                                                itemslist[j + 3].TimeOff);
                                                    timeonfour1 = tu.Item1;
                                                    timeofffour1 = tu.Item2;
                                                }
                                            }
                                        }
                                        j = j + max;
                                        //break;
                                    }

                                }
                            }
                        }
                        if (tu.Item1 == "" && tu.Item2 == "")
                        {
                            tu = MainCalculateTimeOnOff(ruleitemslistorder[i].IsUsedLuxOn,
                                                        ruleitemslistorder[i].IsUsedLuxOff,
                                                        ruleitemslistorder[i].IsUsedOnSet,
                                                        ruleitemslistorder[i].IsUsedOffSet,
                                                        ruleitemslistorder[i].TimeOn,
                                                        ruleitemslistorder[i].TimeOff);

                            timeonone = tu.Item1;
                            timeoffone = tu.Item2;

                            if (max == 1)
                            {
                                timeontwo = null;
                                timeofftwo = null;
                                timeonthree = null;
                                timeoffthree = null;
                                timeonfour = null;
                                timeofffour = null;
                            }
                            else
                            {
                                tu = MainCalculateTimeOnOff(ruleitemslistorder[i + 1].IsUsedLuxOn,
                                                            ruleitemslistorder[i + 1].IsUsedLuxOff,
                                                            ruleitemslistorder[i + 1].IsUsedOnSet,
                                                            ruleitemslistorder[i + 1].IsUsedOffSet,
                                                            ruleitemslistorder[i + 1].TimeOn,
                                                            ruleitemslistorder[i + 1].TimeOff);
                                timeontwo = tu.Item1;
                                timeofftwo = tu.Item2;
                                timeonthree = null;
                                timeoffthree = null;
                                timeonfour = null;
                                timeofffour = null;

                                if (max == 3 || max == 4)
                                {
                                    tu = MainCalculateTimeOnOff(ruleitemslistorder[i + 2].IsUsedLuxOn,
                                                                ruleitemslistorder[i + 2].IsUsedLuxOff,
                                                                ruleitemslistorder[i + 2].IsUsedOnSet,
                                                                ruleitemslistorder[i + 2].IsUsedOffSet,
                                                                ruleitemslistorder[i + 2].TimeOn,
                                                                ruleitemslistorder[i + 2].TimeOff);
                                    timeonthree = tu.Item1;
                                    timeoffthree = tu.Item2;
                                    timeonfour = null;
                                    timeofffour = null;
                                    if (max == 4)
                                    {
                                        tu = MainCalculateTimeOnOff(ruleitemslistorder[i + 3].IsUsedLuxOn,
                                                                    ruleitemslistorder[i + 3].IsUsedLuxOff,
                                                                    ruleitemslistorder[i + 3].IsUsedOnSet,
                                                                    ruleitemslistorder[i + 3].IsUsedOffSet,
                                                                    ruleitemslistorder[i + 3].TimeOn,
                                                                    ruleitemslistorder[i + 3].TimeOff);
                                        timeonfour = tu.Item1;
                                        timeofffour = tu.Item2;
                                    }
                                }
                            }
                        }

                        MainRuleItems.Add(new MainRuleItemsStyle()
                                              {
                                                  MainWeek = week,
                                                  MainDate = date,
                                                  MainTimeOnOne = timeonone,
                                                  MainTimeOnOne1 = timeonone1,
                                                  MainTimeOffOne = timeoffone,
                                                  MainTimeOffOne1 = timeoffone1,
                                                  MainTimeOnTwo = timeontwo,
                                                  MainTimeOnTwo1 = timeontwo1,
                                                  MainTimeOffTwo = timeofftwo,
                                                  MainTimeOffTwo1 = timeofftwo1,
                                                  MainTimeOnThree = timeonthree,
                                                  MainTimeOnThree1 = timeonthree1,
                                                  MainTimeOffThree = timeoffthree,
                                                  MainTimeOffThree1 = timeoffthree1,
                                                  MainTimeOnFour = timeonfour,
                                                  MainTimeOnFour1 = timeonfour1,
                                                  MainTimeOffFour = timeofffour,
                                                  MainTimeOffFour1 = timeofffour1,
                                                  MainSunRise = sunrise,
                                                  MainSunSet = sunset
                                              });
                    }



                }
            }
        }

        public string MainCalculateTime(int time)
        {
            try
            {
                var inttime = time;
                int hour = inttime / 60;
                int minute = inttime % 60;

                if (hour == 25)
                    return "-";
                return hour.ToString("D2") + ":" + minute.ToString("D2");
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return "25:00";
        }

        public Tuple<string, string> MainCalculateTimeOnOff(bool isusedluxon, bool isusedluxoff, bool isusedtimeonset, bool isusedtimeoffset, int timeon, int timeoff)
        {
            try
            {


                string str1 = "", str2 = "";
                if (isusedluxon) str1 = str1 + " 光 ";
                else if (isusedtimeonset) str1 = str1 + " 偏 ";
                else str1 = str1 + "      ";


                str1 = str1 + MainCalculateTime(timeon);

                if (isusedluxoff) str2 = str2 + " 光 ";
                else if (isusedtimeoffset) str2 = str2 + " 偏 ";
                else str2 = str2 + "      ";

                str2 = str2 + MainCalculateTime(timeoff);

                if (timeon != 1500)
                {
                    if (timeon > timeoff) str2 = str2 + "(明)";
                }

                var tu = new Tuple<string, string>(str1, str2);
                return tu;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return new Tuple<string, string>(" ", " ");
        }


        public class MainRuleItemsStyle : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private string _maindate;
            public string MainDate
            {
                get
                {
                    return _maindate;
                }
                set
                {
                    if (_maindate != value)
                    {
                        _maindate = value;
                        this.RaisePropertyChanged(() => this.MainDate);
                    }
                }
            }

            private int _mainweek;
            public int MainWeek
            {
                get
                {
                    return _mainweek;
                }
                set
                {
                    if (_mainweek != value)
                    {
                        _mainweek = value;
                        this.RaisePropertyChanged(() => this.MainWeek);
                    }
                }
            }

            private string _maintimeonone;
            public string MainTimeOnOne
            {
                get
                {
                    return _maintimeonone;
                }
                set
                {
                    if (_maintimeonone != value)
                    {
                        _maintimeonone = value;
                        this.RaisePropertyChanged(() => this.MainTimeOnOne);
                    }
                }
            }

            //新增
            private string _maintimeonone1;
            public string MainTimeOnOne1
            {
                get
                {
                    return _maintimeonone1;
                }
                set
                {
                    if (_maintimeonone1 != value)
                    {
                        _maintimeonone1 = value;
                        this.RaisePropertyChanged(() => this.MainTimeOnOne1);
                    }
                }
            }

            private string _maintimeoffone;
            public string MainTimeOffOne
            {
                get
                {
                    return _maintimeoffone;
                }
                set
                {
                    if (_maintimeoffone != value)
                    {
                        _maintimeoffone = value;
                        this.RaisePropertyChanged(() => this.MainTimeOffOne);
                    }
                }
            }

            //新增
            private string _maintimeoffone1;
            public string MainTimeOffOne1
            {
                get
                {
                    return _maintimeoffone1;
                }
                set
                {
                    if (_maintimeoffone1 != value)
                    {
                        _maintimeoffone1 = value;
                        this.RaisePropertyChanged(() => this.MainTimeOffOne1);
                    }
                }
            }

            private string _maintimeontwo;
            public string MainTimeOnTwo
            {
                get
                {
                    return _maintimeontwo;
                }
                set
                {
                    if (_maintimeontwo != value)
                    {
                        _maintimeontwo = value;
                        this.RaisePropertyChanged(() => this.MainTimeOnTwo);
                    }
                }
            }

            //新增
            private string _maintimeontwo1;
            public string MainTimeOnTwo1
            {
                get
                {
                    return _maintimeontwo1;
                }
                set
                {
                    if (_maintimeontwo1 != value)
                    {
                        _maintimeontwo1 = value;
                        this.RaisePropertyChanged(() => this.MainTimeOnTwo1);
                    }
                }
            }

            private string _maintimeofftwo;
            public string MainTimeOffTwo
            {
                get
                {
                    return _maintimeofftwo;
                }
                set
                {
                    if (_maintimeofftwo != value)
                    {
                        _maintimeofftwo = value;
                        this.RaisePropertyChanged(() => this.MainTimeOffTwo);
                    }
                }
            }

            //新增
            private string _maintimeofftwo1;
            public string MainTimeOffTwo1
            {
                get
                {
                    return _maintimeofftwo1;
                }
                set
                {
                    if (_maintimeofftwo1 != value)
                    {
                        _maintimeofftwo1 = value;
                        this.RaisePropertyChanged(() => this.MainTimeOffTwo1);
                    }
                }
            }

            private string _maintimeonthree;
            public string MainTimeOnThree
            {
                get
                {
                    return _maintimeonthree;
                }
                set
                {
                    if (_maintimeonthree != value)
                    {
                        _maintimeonthree = value;
                        this.RaisePropertyChanged(() => this.MainTimeOnThree);
                    }
                }
            }

            //新增
            private string _maintimeonthree1;
            public string MainTimeOnThree1
            {
                get
                {
                    return _maintimeonthree1;
                }
                set
                {
                    if (_maintimeonthree1 != value)
                    {
                        _maintimeonthree1 = value;
                        this.RaisePropertyChanged(() => this.MainTimeOnThree1);
                    }
                }
            }

            private string _maintimeoffthree;
            public string MainTimeOffThree
            {
                get
                {
                    return _maintimeoffthree;
                }
                set
                {
                    if (_maintimeoffthree != value)
                    {
                        _maintimeoffthree = value;
                        this.RaisePropertyChanged(() => this.MainTimeOffThree);
                    }
                }
            }

            //新增
            private string _maintimeoffthree1;
            public string MainTimeOffThree1
            {
                get
                {
                    return _maintimeoffthree1;
                }
                set
                {
                    if (_maintimeoffthree1 != value)
                    {
                        _maintimeoffthree1 = value;
                        this.RaisePropertyChanged(() => this.MainTimeOffThree1);
                    }
                }
            }

            private string _maintimeonfour;
            public string MainTimeOnFour
            {
                get
                {
                    return _maintimeonfour;
                }
                set
                {
                    if (_maintimeonfour != value)
                    {
                        _maintimeonfour = value;
                        this.RaisePropertyChanged(() => this.MainTimeOnFour);
                    }
                }
            }

            //新增
            private string _maintimeonfour1;
            public string MainTimeOnFour1
            {
                get
                {
                    return _maintimeonfour1;
                }
                set
                {
                    if (_maintimeonfour1 != value)
                    {
                        _maintimeonfour1 = value;
                        this.RaisePropertyChanged(() => this.MainTimeOnFour1);
                    }
                }
            }

            private string _maintimeofffour;
            public string MainTimeOffFour
            {
                get
                {
                    return _maintimeofffour;
                }
                set
                {
                    if (_maintimeofffour != value)
                    {
                        _maintimeofffour = value;
                        this.RaisePropertyChanged(() => this.MainTimeOffFour);
                    }
                }
            }

            //新增
            private string _maintimeofffour1;
            public string MainTimeOffFour1
            {
                get
                {
                    return _maintimeofffour1;
                }
                set
                {
                    if (_maintimeofffour1 != value)
                    {
                        _maintimeofffour1 = value;
                        this.RaisePropertyChanged(() => this.MainTimeOffFour1);
                    }
                }
            }

            private string _mainsunrise;
            public string MainSunRise
            {
                get
                {
                    return _mainsunrise;
                }
                set
                {
                    if (_mainsunrise != value)
                    {
                        _mainsunrise = value;
                        this.RaisePropertyChanged(() => this.MainSunRise);
                    }
                }
            }

            private string _mainsunset;
            public string MainSunSet
            {
                get
                {
                    return _mainsunset;
                }
                set
                {
                    if (_mainsunset != value)
                    {
                        _mainsunset = value;
                        this.RaisePropertyChanged(() => this.MainSunSet);
                    }
                }
            }

            private ObservableCollection<int> _maintype;
            public ObservableCollection<int> MainType
            {
                get
                {
                    return _maintype;
                }
                set
                {
                    if (_maintype != value)
                    {
                        _maintype = value;
                        this.RaisePropertyChanged(() => this.MainType);
                    }
                }
            }
        }

        #endregion
    }
}





