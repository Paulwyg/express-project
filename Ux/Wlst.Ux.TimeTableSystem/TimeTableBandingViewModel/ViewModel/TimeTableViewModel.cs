using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreServices;
using Wlst.Sr.ProtocolCnt.TimeTable;
using Wlst.Sr.TimeTableSystem.Models;
using Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Service;
using Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.ViewModel
{
    public class TimeTableViewModel : ObservableObject
    {
        /// <summary>
        /// 根据已有的timetable生产viewmodel
        /// </summary>
        /// <param name="weekTimeTable"></param>
        public TimeTableViewModel(TimeTableInfomationItem weekTimeTable)
        {
            foreach (var t in weekTimeTable.LstOneWeekOpenCloseControl)
            {
                this.OneWeekTimeTable.Add(new TimeTableOneDayViewModel(t));
            }

            this.TimeDesc = weekTimeTable.TimeDesc;
            this.TimeTableId = weekTimeTable.TimeId;
            this.TimeTableName = weekTimeTable.TimeName;
            this.LuxId = weekTimeTable.LuxId;
            this.LuxOnValue = weekTimeTable.LuxOnValue;
            this.LuxOffValue = weekTimeTable.LuxOffValue;
            this.LightOffOffset = weekTimeTable.LightOffOffset;
            this.LightOnOffset = weekTimeTable.LightOnOffset;
            this.LuxEffective = weekTimeTable.LuxEffective;
        }

        public TimeTableInfomationItem BackToWeekTimeTableInforemation()
        {
            var weekTimeTable = new TimeTableInfomationItem();
            weekTimeTable.TimeDesc = this.TimeDesc;
            weekTimeTable.TimeId = this.TimeTableId;
            weekTimeTable.TimeName = this.TimeTableName;
            weekTimeTable.LuxId = this.LuxId;
            weekTimeTable.LuxOnValue = this.LuxOnValue;
            weekTimeTable.LuxOffValue = this.LuxOffValue;
            weekTimeTable.LightOffOffset = this.LightOffOffset;
            weekTimeTable.LightOnOffset = this.LightOnOffset;
            weekTimeTable.LuxEffective = this.LuxEffective;
            foreach (var t in this.OneWeekTimeTable)
            {
                weekTimeTable.LstOneWeekOpenCloseControl.Add(t.BackToWeekTimeTableItemInfomation());
            }
            return weekTimeTable;
        }

        /// <summary>
        /// 比较两条数据是否相同
        /// </summary>
        /// <param name="timeTableView"></param>
        /// <returns></returns>
        public bool Compar(TimeTableInfomationItem timeTableView)
        {
            if (this.TimeDesc != timeTableView.TimeDesc)
            {
                return false;
            }
            if (this.TimeTableId != timeTableView.TimeId)
            {
                return false;
            }
            if (this.TimeTableName != timeTableView.TimeName)
            {
                return false;
            }
            if (this.LuxId != timeTableView.LuxId)
            {
                return false;
            }
            if (this.LuxOnValue != timeTableView.LuxOnValue)
            {
                return false;
            }
            if (this.LuxOffValue != timeTableView.LuxOffValue)
            {
                return false;
            }
            if (this.LightOffOffset != timeTableView.LightOffOffset)
            {
                return false;
            }
            if (this.LightOnOffset != timeTableView.LightOnOffset)
            {
                return false;
            }
            if (this.OneWeekTimeTable.Count != timeTableView.LstOneWeekOpenCloseControl.Count)
            {
                return false;
            }
            foreach (var t in this.OneWeekTimeTable)
            {
                bool bolfind = false;
                foreach (var tt in timeTableView.LstOneWeekOpenCloseControl)
                {
                    if (t.DateDay == tt.DateDay && t.DateMonth == tt.DateMonth)
                    {
                        bolfind = t.Compare(tt);
                        if (!bolfind)
                            return false;
                    }
                }
                if (!bolfind)
                    return false;
            }
            return true;
        }

        public void Update(TimeTableInfomationItem weekTimeTable)
        {
            this.TimeDesc = weekTimeTable.TimeDesc;
            this.TimeTableId = weekTimeTable.TimeId;
            this.TimeTableName = weekTimeTable.TimeName;
            this.LuxId = weekTimeTable.LuxId;
            this.LuxOnValue = weekTimeTable.LuxOnValue;
            this.LuxOffValue = weekTimeTable.LuxOffValue;
            this.LightOffOffset = weekTimeTable.LightOffOffset;
            this.LightOnOffset = weekTimeTable.LightOnOffset;
            this.LuxEffective = weekTimeTable.LuxEffective;

            this.OneWeekTimeTable.Clear();
            foreach (var t in weekTimeTable.LstOneWeekOpenCloseControl)
            {
                this.OneWeekTimeTable.Add(new TimeTableOneDayViewModel(t));
            }
        }

        /// <summary>
        /// 新建timetable
        /// </summary>
        public TimeTableViewModel()
        {
            if (_oneWeekTimeTable == null)
                _oneWeekTimeTable = new ObservableCollection<TimeTableOneDayViewModel>();
            this.TimeDesc = "New Table";
            this.TimeTableId = -1;
            this.TimeTableName = "New Table";
            this.LuxEffective = 35;
        }

        #region weektimetable中的参数

        private int _timeId;

        /// <summary>
        /// 时间表ID
        /// </summary>
        public int TimeTableId
        {
            get
            {
                return _timeId;
            }
            set
            {
                if (_timeId != value)
                {
                    _timeId = value;
                    this.RaisePropertyChanged(() => this.TimeTableId);
                }
            }
        }

        private string _timeName;

        /// <summary>
        /// 时间表名称
        /// </summary>
        public string TimeTableName
        {
            get
            {
                return _timeName;
            }
            set
            {
                if (_timeName != value)
                {
                    _timeName = value;
                    this.RaisePropertyChanged(() => this.TimeTableName);
                }
            }
        }

        private string _timeDesc;

        /// <summary>
        /// 时间表描述
        /// </summary>
        public string TimeDesc
        {
            get
            {
                return _timeDesc;
            }
            set
            {
                if (_timeDesc != value)
                {
                    _timeDesc = value;
                    this.RaisePropertyChanged(() => this.TimeDesc);
                }
            }
        }

        private int _luxId;

        /// <summary>
        /// 该时间表使用的光控探头ID
        /// </summary>
        public int LuxId
        {
            get
            {
                return _luxId;
            }
            set
            {
                if (_luxId != value)
                {
                    _luxId = value;
                    foreach (var t in LuxCollection)
                    {
                        if (t.LuxId == value)
                        {
                            CurrentSelectLux = t;
                            break;
                        }
                    }
                    this.RaisePropertyChanged(() => this.LuxId);
                }
            }
        }

        private ObservableCollection<LuxViewModel> _luxCollection;

        /// <summary>
        /// 光控
        /// </summary>
        public ObservableCollection<LuxViewModel> LuxCollection
        {
            get
            {
                if (_luxCollection == null)
                {
                    _luxCollection = new ObservableCollection<LuxViewModel>();
                    foreach (var t in LuxGetServer.GetAllLuxEquipment)
                    {
                        _luxCollection.Add(new LuxViewModel() { LuxId = t.Value, LuxName = t.Name });
                    }
                    if (LuxId != 0)
                    {
                        foreach (var t in _luxCollection)
                        {
                            if (t.LuxId == LuxId)
                            {
                                CurrentSelectLux = t;
                                break;
                            }
                        }
                    }
                }
                return _luxCollection;
            }
        }

        public void ReloadLux()
        {
            if (_luxCollection == null)
                _luxCollection = new ObservableCollection<LuxViewModel>();
            _luxCollection.Clear();
            foreach (var t in LuxGetServer.GetAllLuxEquipment)
            {
                _luxCollection.Add(new LuxViewModel() { LuxId = t.Value, LuxName = t.Name });
            }
            foreach (var t in LuxCollection)
            {
                if (t.LuxId == LuxId)
                {
                    CurrentSelectLux = t;
                    break;
                }
            }
        }

        private LuxViewModel _currentSelectLux;

        /// <summary>
        /// 当前选中的光控
        /// </summary>
        public LuxViewModel CurrentSelectLux
        {
            get
            {
                if (_currentSelectLux == null)
                    _currentSelectLux = new LuxViewModel();
                return _currentSelectLux;
            }
            set
            {
                if (_currentSelectLux != value)
                {
                    _currentSelectLux = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectLux);
                    if (_currentSelectLux != null)
                        LuxId = _currentSelectLux.LuxId;
                }
            }
        }

        private int _luxOnValue;

        /// <summary>
        /// 该时间表若是使用光控则开灯光照度值
        /// </summary>
        public int LuxOnValue
        {
            get
            {
                return _luxOnValue;
            }
            set
            {
                if (_luxOnValue != value)
                {
                    _luxOnValue = value;
                    this.RaisePropertyChanged(() => this.LuxOnValue);
                }
            }
        }

        private int _luxEffective;

        /// <summary>
        /// 光控有效值 20-90
        /// </summary>
        public int LuxEffective
        {
            get { return _luxEffective; }
            set
            {
                if (_luxEffective != value)
                {
                    if (value < 20) value = 20;
                    if (value > 90) value = 90;
                    _luxEffective = value;
                    this.RaisePropertyChanged(() => this.LuxEffective);
                }
            }
        }

        private int _luxOffValue;

        /// <summary>
        /// 该时间表若是使用光照度关灯 则关灯光照度值
        /// </summary>
        public int LuxOffValue
        {
            get
            {
                return _luxOffValue;
            }
            set
            {
                if (_luxOffValue != value)
                {
                    _luxOffValue = value;
                    this.RaisePropertyChanged(() => this.LuxOffValue);
                }
            }
        }

        private int _lightOnOffset;

        /// <summary>
        /// 如果该时间表使用偏移  则开灯偏移值
        /// 0不启用偏移 定时；
        /// 不为0则启用偏移  根据日出日落计算
        /// </summary>
        public int LightOnOffset
        {
            get
            {
                return _lightOnOffset;
            }
            set
            {
                if (_lightOnOffset != value)
                {
                    if (value > 60)
                        value = 60;
                    if (value < -60)
                        value = -60;
                    _lightOnOffset = value;
                    UpdateChildOnOffSet();
                    this.RaisePropertyChanged(() => this.LightOnOffset);
                }
            }
        }

        void UpdateChildOnOffSet()
        {
            for (int i = 0; i < this.OneWeekTimeTable.Count; i++)
            {
                OneWeekTimeTable[i].LightOnOffSet = this.LightOnOffset;
            }
        }

        private int _lightOffOffset;

        /// <summary>
        /// 如果该时间表使用偏移 
        /// 则关灯偏移值为；
        /// 0不启用偏移 定时；
        /// 不为0则启用偏移  根据日出日落计算
        /// </summary>
        public int LightOffOffset
        {
            get
            {
                return _lightOffOffset;
            }
            set
            {
                if (_lightOffOffset != value)
                {
                    if (value > 60)
                        value = 60;
                    if (value < -60)
                        value = -60;
                    _lightOffOffset = value;
                    UpdateChildOffOffSet();
                    this.RaisePropertyChanged(() => this.LightOffOffset);
                }
            }
        }

        void UpdateChildOffOffSet()
        {
            for (int i = 0; i < this.OneWeekTimeTable.Count; i++)
            {
                OneWeekTimeTable[i].LightOffOffSet = this.LightOffOffset;
            }
        }

        private ObservableCollection<TimeTableOneDayViewModel> _oneWeekTimeTable;

        /// <summary>
        /// 一周时间表具体数据
        /// </summary>
        public ObservableCollection<TimeTableOneDayViewModel> OneWeekTimeTable
        {
            get
            {
                if (_oneWeekTimeTable == null)
                    _oneWeekTimeTable = new ObservableCollection<TimeTableOneDayViewModel>();
                return _oneWeekTimeTable;
            }
        }

        #endregion

        #region 辅助显示信息

        #endregion

        #region 输出

        /// <summary>
        /// 将viewmodel中的数据转换为 model数据
        /// </summary>
        public WeekTimeTableInforemation WeekTimeTable
        {
            get
            {
                WeekTimeTableInforemation weekTimeTable = new WeekTimeTableInforemation();
                weekTimeTable.light_off_offset = this.LightOffOffset;
                weekTimeTable.time_id = this.TimeTableId;
                weekTimeTable.time_name = this.TimeTableName;
                weekTimeTable.light_on_offset = this.LightOnOffset;
                weekTimeTable.lux_off_value = this.LuxOffValue;
                weekTimeTable.lux_on_value = this.LuxOnValue;
                weekTimeTable.time_desc = this.TimeDesc;
                weekTimeTable.lux_id = this.LuxId;
                foreach (var t in this.OneWeekTimeTable)
                {
                    weekTimeTable.LstOneWeekOpenCloseControl.Add(t.WeekTimeTableItem);
                }
                return weekTimeTable;
            }
        }

        #endregion

    }
}