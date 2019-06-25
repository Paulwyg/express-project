using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
using Wlst.Sr.TimeTableSystem.Models;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.client;


namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel
{
    public class OneItemTimeTable : ObservableObject
    {
        #region 属性

        #region TimeTableId

        private int _timeTableId;

        public int TimeTableId
        {
            get { return _timeTableId; }
            set
            {
                if (_timeTableId == value) return;
                _timeTableId = value;
                RaisePropertyChanged(() => TimeTableId);
                //if(value >10000)
                //{
                //    ExTimeTableName = " -该时间表保存后有效";
                //}
            }
        }

        #endregion

        #region TimeTableName

        private string _timeName;

        /// <summary>
        /// 时间表名称
        /// </summary>
        public string TimeTableName
        {
            get { return _timeName; }
            set
            {
                if (_timeName == value) return;
                _timeName = value;
                RaisePropertyChanged(() => TimeTableName);
            }
        }

        #endregion

        //#region ExTimeTableName
        //private string _timExTimeTableNameeName;

        ///// <summary>
        ///// 时间表名称
        ///// </summary>
        //public string ExTimeTableName
        //{
        //    get { return _timExTimeTableNameeName; }
        //    set
        //    {
        //        if (_timExTimeTableNameeName == value) return;
        //        _timExTimeTableNameeName = value;
        //        RaisePropertyChanged(() => ExTimeTableName);
        //    }
        //}

        //#endregion

        #region TimeDesc

        private string _timeDesc;

        /// <summary>
        /// 时间表描述
        /// </summary>
        public string TimeDesc
        {
            get { return _timeDesc; }
            set
            {
                if (_timeDesc == value) return;
                _timeDesc = value;
                RaisePropertyChanged(() => TimeDesc);
            }
        }

        #endregion

        #region LightOnOffset

        private int _lightOnOffset;

        /// <summary>
        /// 如果该时间表使用偏移  则开灯偏移值
        /// 0不启用偏移 定时；
        /// 不为0则启用偏移  根据日出日落计算
        /// </summary>
        public int LightOnOffset
        {
            get { return _lightOnOffset; }
            set
            {
                if (_lightOnOffset == value) return;
                if (value > 60)
                    value = 60;
                if (value < -60)
                    value = -60;
                _lightOnOffset = value;
                UpdateChildOnOffSet();
                RaisePropertyChanged(() => LightOnOffset);
            }
        }

        #endregion

        #region LightOffOffset

        private int _lightOffOffset;

        /// <summary>
        /// 如果该时间表使用偏移 
        /// 则关灯偏移值为；
        /// 0不启用偏移 定时；
        /// 不为0则启用偏移  根据日出日落计算
        /// </summary>
        public int LightOffOffset
        {
            get { return _lightOffOffset; }
            set
            {
                if (_lightOffOffset == value) return;
                if (value > 60)
                    value = 60;
                if (value < -60)
                    value = -60;
                _lightOffOffset = value;
                UpdateChildOffOffSet();
                RaisePropertyChanged(() => LightOffOffset);
            }
        }

        #endregion

        #region LuxCollection

        //private ObservableCollection<IdNameDesc> _luxCollection;

        //public ObservableCollection<IdNameDesc> LuxCollection
        //{
        //    get
        //    {
        //        if (_luxCollection == null)
        //        {
        //            _luxCollection = new ObservableCollection<IdNameDesc>();
        //            foreach (var t in LuxGetServer.GetAllLuxEquipment)
        //            {
        //                _luxCollection.Add(new IdNameDesc {Id = t.Value, Name = t.Name});
        //            }
        //            if (LuxId != 0)
        //            {
        //                foreach (var t in _luxCollection.Where(t => t.Id == LuxId))
        //                {
        //                    CurrentSelectLux = t;
        //                    break;
        //                }
        //            }
        //        }
        //        return _luxCollection;
        //    }
        //}

        #endregion

        #region LuxId

        //private int _luxId;

        ///// <summary>
        ///// 该时间表使用的光控探头ID
        ///// </summary>
        //public int LuxId
        //{
        //    get { return _luxId; }
        //    set
        //    {
        //        if (_luxId == value) return;
        //        _luxId = value;
        //        foreach (var t in LuxCollection.Where(t => t.Id == value))
        //        {
        //            CurrentSelectLux = t;
        //            LuxName = t.Name;
        //            break;
        //        }
        //        RaisePropertyChanged(() => LuxId);
        //    }
        //}

        #endregion

        #region LuxName

        private string _luxName;

        public string LuxName
        {
            get { return _luxName; }
            set
            {
                if (_luxName == value) return;
                _luxName = value;
                RaisePropertyChanged(() => LuxName);
            }
        }

        #endregion

        #region CurrentSelectLux

        //private IdNameDesc _currentSelectLux;

        ///// <summary>
        ///// 当前选中的光控
        ///// </summary>
        //public IdNameDesc CurrentSelectLux
        //{
        //    get { return _currentSelectLux ?? (_currentSelectLux = new IdNameDesc()); }
        //    set
        //    {
        //        if (_currentSelectLux == value) return;
        //        _currentSelectLux = value;
        //        RaisePropertyChanged(() => CurrentSelectLux);
        //        if (_currentSelectLux != null)
        //            LuxId = _currentSelectLux.Id;
        //    }
        //}

        #endregion

        #region LuxOnValue

        private int _luxOnValue;

        /// <summary>
        /// 该时间表若是使用光控则开灯光照度值
        /// </summary>
        public int LuxOnValue
        {
            get { return _luxOnValue; }
            set
            {
                if (_luxOnValue == value) return;
                _luxOnValue = value;
                RaisePropertyChanged(() => LuxOnValue);
            }
        }

        #endregion

        #region LuxOffValue

        private int _luxOffValue;

        /// <summary>
        /// 该时间表若是使用光照度关灯 则关灯光照度值
        /// </summary>
        public int LuxOffValue
        {
            get { return _luxOffValue; }
            set
            {
                if (_luxOffValue == value) return;
                _luxOffValue = value;
                RaisePropertyChanged(() => LuxOffValue);
            }
        }

        #endregion

        #region LuxEffective

        private int _luxEffective;

        /// <summary>
        /// 光控有效值 20-90
        /// </summary>
        public int LuxEffective
        {
            get { return _luxEffective; }
            set
            {
                if (_luxEffective == value) return;
                if (value < 20) value = 20;
                if (value > 90) value = 90;
                _luxEffective = value;
                RaisePropertyChanged(() => LuxEffective);
            }
        }

        #endregion

        #region TimeTableOneWeeksViewModels

        #endregion

        private ObservableCollection<OneDayTimeTableViewModel> _oneWeekTimeTable;

        /// <summary>
        /// 一周时间表具体数据
        /// </summary>
        public ObservableCollection<OneDayTimeTableViewModel> OneWeekTimeTable
        {
            get { return _oneWeekTimeTable ?? (_oneWeekTimeTable = new ObservableCollection<OneDayTimeTableViewModel>()); }
            set
            {
                if (_oneWeekTimeTable == value) return;
                _oneWeekTimeTable = value;
                RaisePropertyChanged(() => OneWeekTimeTable);
            }
        }

        #endregion

        #region Constracor

        public OneItemTimeTable(TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem weektable)
        {
            LuxName = "无";

            TimeDesc = weektable.TimeDesc;
            TimeTableId = weektable.TimeId;
            TimeTableName = weektable.TimeName;
            //LuxId = weektable.LuxId;
            LuxOnValue = weektable.LuxOnValue;
            LuxOffValue = weektable.LuxOffValue;
            LightOffOffset = weektable.LightOffOffset;
            LightOnOffset = weektable.LightOnOffset;
            LuxEffective = weektable.LuxEffective;

        }

        public TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem BackToWeekTimeTableInforemation()
        {
            var weekTimeTable = new TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem();
            weekTimeTable.TimeDesc = this.TimeDesc;
            weekTimeTable.TimeId = this.TimeTableId;
            weekTimeTable.TimeName = this.TimeTableName;
            //weekTimeTable.LuxId = this.LuxId;
            weekTimeTable.LuxOnValue = this.LuxOnValue;
            weekTimeTable.LuxOffValue = this.LuxOffValue;
            weekTimeTable.LightOffOffset = this.LightOffOffset;
            weekTimeTable.LightOnOffset = this.LightOnOffset;
            weekTimeTable.LuxEffective = this.LuxEffective;

            //foreach (var t in this.OneWeekTimeTable)
            //{
            //    weekTimeTable.LstOneWeekOpenCloseControl.Add(t.BackToWeekTimeTableItemInfomation());
            //}



            return weekTimeTable;
        }

        #endregion

        //public void ReloadLux()
        //{
        //    if (_luxCollection == null)
        //        _luxCollection = new ObservableCollection<IdNameDesc>();
        //    _luxCollection.Clear();
        //    foreach (var t in LuxGetServer.GetAllLuxEquipment)
        //    {
        //        _luxCollection.Add(new IdNameDesc {Id = t.Value, Name = t.Name});
        //    }
        //    foreach (var t in LuxCollection)
        //    {
        //        if (t.Id == LuxId)
        //        {
        //            CurrentSelectLux = t;
        //            break;
        //        }
        //    }
        //}

        private void UpdateChildOnOffSet()
        {
            foreach (var t in OneWeekTimeTable)
            {
                t.LightOnOffSet = LightOnOffset;
            }
        }

        private void UpdateChildOffOffSet()
        {
            foreach (var t in OneWeekTimeTable)
            {
                t.LightOffOffSet = LightOffOffset;
            }
        }

        #region AddTimeTableGrid

        //private ObservableCollection<AddTimeTableGridItem> _addtimetablegrid;

        //public ObservableCollection<AddTimeTableGridItem> AddTimeTableGrid
        //{
        //    get
        //    {
        //        if (_addtimetablegrid == null)
        //            _addtimetablegrid = new ObservableCollection<AddTimeTableGridItem>();
        //        return _addtimetablegrid;
        //    }
        //    set
        //    {
        //        if (value == _addtimetablegrid) return;
        //        _addtimetablegrid = value;
        //        this.RaisePropertyChanged(() => AddTimeTableGrid);
        //    }
        //}



        //public class AddTimeTableGridItem : Wlst.Cr.Core.CoreServices.ObservableObject
        //{
        //    public AddTimeTableGridItem(TimeTableInfomationItem weektable)
        //    {

        //    }

        //    #region
        //    private string _date;
        //    public string Date
        //    {
        //        get { return _date; }
        //        set
        //        {
        //            if (_date != value)
        //            {
        //                _date = value;
        //                this.RaisePropertyChanged(() => this.Date);
        //            }
        //        }
        //    }

        //    private int _week;
        //    public int Week
        //    {
        //        get { return _week; }
        //        set
        //        {
        //            if (value != _week)
        //            {
        //                _week = value;
        //                this.RaisePropertyChanged(() => this.Week);
        //            }
        //        }
        //    }

        //    private int _timetablesectionid;
        //    public int TimetableSectionId
        //    {
        //        get { return _timetablesectionid; }
        //        set
        //        {
        //            if (value != _timetablesectionid)
        //            {
        //                _timetablesectionid = value;
        //                this.RaisePropertyChanged(() => this.TimetableSectionId);
        //            }
        //        }
        //    }

        //    private int _timeon;
        //    public int TimeOn
        //    {
        //        get { return _timeon; }
        //        set
        //        {
        //            if (value != _timeon)
        //            {
        //                _timeon = value;
        //                this.RaisePropertyChanged(() => this.TimeOn);
        //            }
        //        }
        //    }

        //    private int _timeoff;
        //    public int TimeOff
        //    {
        //        get { return _timeoff; }
        //        set
        //        {
        //            if (value != _timeoff)
        //            {
        //                _timeoff = value;
        //                this.RaisePropertyChanged(() => this.TimeOff);
        //            }
        //        }
        //    }

        //    private int _rulesectionid;
        //    public int RuleSectionId
        //    {
        //        get { return _rulesectionid; }
        //        set
        //        {
        //            if (value != _rulesectionid)
        //            {
        //                _rulesectionid = value;
        //                this.RaisePropertyChanged(() => this.RuleSectionId);
        //            }
        //        }
        //    }

        //    private bool _isusedluxon;
        //    public bool IsUsedLuxOn
        //    {
        //        get { return _isusedluxon; }
        //        set
        //        {
        //            if (value != _isusedluxon)
        //            {
        //                _isusedluxon = value;
        //                this.RaisePropertyChanged(() => this.IsUsedLuxOn);
        //            }
        //        }
        //    }

        //    private bool _isusedluxoff;
        //    public bool IsUsedLuxOff
        //    {
        //        get { return _isusedluxoff; }
        //        set
        //        {
        //            if (value != _isusedluxoff)
        //            {
        //                _isusedluxoff = value;
        //                this.RaisePropertyChanged(() => this.IsUsedLuxOff);
        //            }
        //        }
        //    }

        //    private bool _isusedoffset;
        //    public bool IsUsedOffSet
        //    {
        //        get { return _isusedoffset; }
        //        set
        //        {
        //            if (value != _isusedoffset)
        //            {
        //                _isusedoffset = value;
        //                this.RaisePropertyChanged(() => this.IsUsedOffSet);
        //            }
        //        }
        //    }

        //    private bool _isusedonSet;
        //    public bool IsUsedOnSet
        //    {
        //        get { return _isusedonSet; }
        //        set
        //        {
        //            if (value != _isusedonSet)
        //            {
        //                _isusedonSet = value;
        //                this.RaisePropertyChanged(() => this.IsUsedOnSet);
        //            }
        //        }
        //    }


            //#endregion
        //}

        #endregion
    }
}