using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeTabletemporaryView.ViewModels
{
    public class OneItemTemporary : EventHandlerHelperExtendNotifyProperyChanged
    {
        public OneItemTemporary()
        {         
            InitEvent();
            SchemeName = "新建方案";
            LightOffOffset = -15;
            LightOnOffset = 15;
            LuxEffective = 30;
            LuxOffValue = 15;
            LuxOnValue = 15;
            CurrentSelectLux = new IdNameDesc
            {
                Id = LuxCollection.First().Id,
                Name = LuxCollection.First().Name,
                NameDesc = LuxCollection.First().NameDesc
            };
            CurrentSelectLux2 = new IdNameDesc
            {
                Id = LuxCollection2.First().Id,
                Name = LuxCollection2.First().Name,
                NameDesc = LuxCollection2.First().NameDesc
            };
            DtEndTime = DateTime.Now.Date.AddDays(1);
            DtStartTime = DateTime.Now.Date;
        }


        public OneItemTemporary(Wlst.client.TempTimePlanWithTimeTableBandingInfo.TimeTablePlan itemTable, int areaId)
        {
            InitEvent();
            AreaId = areaId;
            foreach (var t in itemTable.ItemsPlan)
            {
                this.RuleItems.Add(new TimeTableOneDayInfomationItem()
                                       {
                                           TimeAreaId = areaId ,
                                           Date = t.Date.ToString().Insert(4, ".").Insert(7, "."),
                                           TimeOff = t.TimeOff,
                                           TimeOn = t.TimeOn,
                                           TimetableSectionId = t.SectionId,
                                           IsUsedLuxOff = t.TypeOff == 1,
                                           IsUsedOffSet = t.TypeOff == 2 || t.TypeOff == 1,
                                           IsUsedLuxOn = t.TypeOn == 1,
                                           IsUsedOnSet = t.TypeOn == 2 || t.TypeOn == 1,
                                           DateDay = Convert.ToInt32(t.Date.ToString().Substring(6, 2)),
                                           DateMonth = Convert.ToInt32(t.Date.ToString().Substring(4, 2)),
                                           IsEdit = true,
                                           TimetableId = itemTable.TimePlanId,
                                           DayOfWeekUsed =
                                               (int) DateTime.ParseExact(t.Date.ToString(), "yyyyMMdd", null).DayOfWeek
                                               

                                       });
            }
            this.CurrentSelectLux = new IdNameDesc
            {
                Id = LuxCollection.First().Id,
                Name = LuxCollection.First().Name,
                NameDesc = LuxCollection.First().NameDesc
            };
            this.CurrentSelectLux2 = new IdNameDesc
            {
                Id = LuxCollection2.First().Id,
                Name = LuxCollection2.First().Name,
                NameDesc = LuxCollection2.First().NameDesc
            };
            this.LuxEffective = itemTable.LuxEffective;
            this.LightOffOffset = itemTable.LightOffOffset;
            this.LightOnOffset = itemTable.LightOnOffset;
            this.LuxId = itemTable.LuxId;
            this.LuxOffValue = itemTable.LuxOffValue;
            this.LuxOnValue = itemTable.LuxOnValue;
            this.SchemeId = itemTable.TimePlanId;
            this.SchemeName = itemTable.TimePlanName;
            this.LuxId2 = itemTable.LuxIdBackup;
            //var date = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            this.DtEndTime = new DateTime(itemTable.DateEnd);
            this.DtStartTime = new DateTime(itemTable.DateStart);



            foreach (var t in itemTable.TimetablesUseThisPlan)
            {
                var lst = Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfoList(areaId);
                foreach (var f in lst)
                {
                    if(f.TimeId==t)
                      this.SelectedItems.Add(new OneItemTimeTable()
                                           {
                                               TimeId = t,
                                               TimeName = f.TimeName,
                                               TimeDesc = f.TimeDesc
                                           });  
                }
                
            }
        }

        private int AreaId;

        /// <summary>
        ///当前区域ID
        /// </summary>
        //public int AreaId
        //{
        //    get { return _areaId; }
        //    set
        //    {
        //        if (value != _areaId)
        //        {
        //            _areaId = value;
        //            this.RaisePropertyChanged(() => this.AreaId);
        //        }
        //    }
        //}

        private int _schemeid;

        /// <summary>
        /// 临时方案ID
        /// </summary>
        public int SchemeId
        {
            get { return _schemeid; }
            set
            {
                if (_schemeid != value)
                {
                    _schemeid = value;
                    this.RaisePropertyChanged(() => this.SchemeId);
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

        private string _schemename;

        /// <summary>
        /// 方案名称
        /// </summary>
        public string SchemeName
        {
            get { return _schemename; }
            set
            {
                if (_schemename != value)
                {
                    _schemename = value;
                    this.RaisePropertyChanged(() => this.SchemeName);
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
                        _luxCollection.Add(new IdNameDesc { Id = t.Value, Name = t.Name, NameDesc = t.Value2.ToString("d4") + "-" + t.Name });
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
                   // LuxChanged = false;
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
                    foreach (var t in RuleItems)
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

        #region DtEndTime

        private DateTime _dtEndTime;

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime DtEndTime
        {
            get { return _dtEndTime; }
            set
            {
                if (_dtEndTime != value)
                {
                    _dtEndTime = value;

                    RaisePropertyChanged(() => DtEndTime);
                }
            }
        }

        #endregion

        #region DtStartTime

        private DateTime _dtStartTime;

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime DtStartTime
        {
            get { return _dtStartTime; }
            set
            {
                if (_dtStartTime != value)
                {
                    _dtStartTime = value;
                    RaisePropertyChanged(() => DtStartTime);
                }
            }
        }

        #endregion

        private ObservableCollection<TimeTableOneDayInfomationItem> _ruleitems;

        /// <summary>
        /// 开关灯规则
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


        private ObservableCollection<OneItemTimeTable> _selecteditems;

        /// <summary>
        /// 所选时间表
        /// </summary>
        public ObservableCollection<OneItemTimeTable> SelectedItems
        {
            get
            {
                if (_selecteditems == null) _selecteditems = new ObservableCollection<OneItemTimeTable>();
                return _selecteditems;

            }
            set
            {
                if (_selecteditems != value)
                {
                    _selecteditems = value;
                    this.RaisePropertyChanged(() => this.SelectedItems);
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

            var obj2 = args.GetParams()[1] as TimeTableOneDayInfomationItem;
            if (obj2 == null) return;

            switch (obj1)
            {
                case 1:
                    if (!_isChecked) break;
                    if (obj2.TimetableId == SchemeId && obj2.TimeAreaId == TimeInfoMnVm.areaId)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.IsUsedLuxOn = obj2.IsUsedLuxOn;
                        }
                    }
                    break;
                case 2:
                    if (!_isChecked) break;
                    if (obj2.TimetableId == SchemeId && obj2.TimeAreaId == TimeInfoMnVm.areaId)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.IsUsedOnSet = obj2.IsUsedOnSet;
                        }
                    }
                    break;
                case 3:

                    #region

                    //if (obj2.TimetableId == SchemeId && obj2.TimeAreaId == TimeInfoMnVm.areaId)
                    //{
                    //    if (obj2.TimetableSectionId > 1)
                    //    {
                    //        foreach (var t in RuleItems)
                    //        {
                    //            if (obj2.TimetableSectionId == t.TimetableSectionId + 1 &&
                    //                obj2.DayOfWeekUsed == t.DayOfWeekUsed && t.Maxsection == obj2.Maxsection)
                    //            {
                    //                if (obj2.TimeOn <= t.TimeOff) obj2.TimeOn = 1500;
                    //            }
                    //        }
                    //    }
                    //}

                    #endregion


                    if (!_isChecked) break;
                    if (obj2.TimetableId == SchemeId && obj2.TimeAreaId == TimeInfoMnVm.areaId)
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
                    if (obj2.TimetableId == SchemeId && obj2.TimeAreaId == TimeInfoMnVm.areaId)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.IsUsedLuxOff = obj2.IsUsedLuxOff;
                        }
                    }
                    break;
                case 5:
                    if (!_isChecked) break;
                    if (obj2.TimetableId == SchemeId && obj2.TimeAreaId == TimeInfoMnVm.areaId)
                    {
                        foreach (var t in RuleItems)
                        {
                            if (t.TimetableSectionId == obj2.TimetableSectionId) t.IsUsedOffSet = obj2.IsUsedOffSet;
                        }
                    }
                    break;
                case 6:

                    if (!_isChecked) break;
                    if (obj2.TimetableId == SchemeId && obj2.TimeAreaId == TimeInfoMnVm.areaId)
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


    }
}
