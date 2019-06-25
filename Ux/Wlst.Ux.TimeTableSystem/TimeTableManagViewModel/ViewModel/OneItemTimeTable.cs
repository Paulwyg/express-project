
using System.Collections.ObjectModel;
using System.Linq;
using Wlst.Cr.Core.CoreServices;
using Wlst.Sr.ProtocolCnt.TimeTable;
using Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Service;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.ViewModel
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
                if(_timeTableId==value) return;
                _timeTableId = value;
                RaisePropertyChanged(()=>TimeTableId);
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
            get
            {
                return _timeName;
            }
            set
            {
                if (_timeName == value) return;
                _timeName = value;
                RaisePropertyChanged(() => TimeTableName);
            }
        }
        #endregion

        #region TimeDesc
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
            get
            {
                return _lightOnOffset;
            }
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
            get
            {
                return _lightOffOffset;
            }
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

        private ObservableCollection<LuxViewModel> _luxCollection;
        public ObservableCollection<LuxViewModel>  LuxCollection
        {
           get
            {
                if (_luxCollection == null)
                {
                    _luxCollection = new ObservableCollection<LuxViewModel>();
                    foreach (var t in LuxGetServer.GetAllLuxEquipment)
                    {
                        _luxCollection.Add(new LuxViewModel { LuxId = t.Value , LuxName = t.Name  });
                    }
                    if (LuxId != 0)
                    {
                        foreach (var t in _luxCollection.Where(t => t.LuxId == LuxId))
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

        #region LuxId
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
                if (_luxId == value) return;
                _luxId = value;
                foreach (var t in LuxCollection.Where(t => t.LuxId == value))
                {
                    CurrentSelectLux = t;
                    LuxName = t.LuxName;
                    break;
                }
                RaisePropertyChanged(() => LuxId);
            }
        }
        #endregion

        #region LuxName

        private string _luxName;
        public string LuxName
        {
            get { return _luxName; }
            set
            {
                if(_luxName==value) return;
                _luxName = value;
                RaisePropertyChanged(()=>LuxName);
            }
        }
        #endregion

        #region CurrentSelectLux
        private LuxViewModel _currentSelectLux;

        /// <summary>
        /// 当前选中的光控
        /// </summary>
        public LuxViewModel CurrentSelectLux
        {
            get { return _currentSelectLux ?? (_currentSelectLux = new LuxViewModel()); }
            set
            {
                if (_currentSelectLux == value) return;
                _currentSelectLux = value;
                RaisePropertyChanged(() => CurrentSelectLux);
                if (_currentSelectLux != null)
                    LuxId = _currentSelectLux.LuxId;
            }
        }
        #endregion

        #region LuxOnValue
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
            get
            {
                return _luxOffValue;
            }
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
        private ObservableCollection<TimeTableOneDayViewModel> _oneWeekTimeTable;

        /// <summary>
        /// 一周时间表具体数据
        /// </summary>
        public ObservableCollection<TimeTableOneDayViewModel> OneWeekTimeTable
        {
            get { return _oneWeekTimeTable ?? (_oneWeekTimeTable = new ObservableCollection<TimeTableOneDayViewModel>()); }
            set
            {
                if(_oneWeekTimeTable==value) return;
                _oneWeekTimeTable = value;
                RaisePropertyChanged(()=>OneWeekTimeTable);
            }
        }
        #endregion

        #region Constracor
        public OneItemTimeTable(WeekTimeTableInforemation weektable)
        {
            LuxName = "无";

            foreach (var t in weektable.LstOneWeekOpenCloseControl)
            {
                OneWeekTimeTable.Add(new TimeTableOneDayViewModel(t));
            }

            TimeDesc = weektable.time_desc;
            TimeTableId = weektable.time_id;
            TimeTableName = weektable.time_name;
            LuxId = weektable.lux_id;
            LuxOnValue = weektable.lux_on_value;
            LuxOffValue = weektable.lux_off_value;
            LightOffOffset = weektable.light_off_offset;
            LightOnOffset = weektable.light_on_offset;
            LuxEffective = weektable.LuxEffective;
        }
        public WeekTimeTableInforemation BackToWeekTimeTableInforemation()
        {
            var weekTimeTable = new WeekTimeTableInforemation();
            weekTimeTable.time_desc = this.TimeDesc;
            weekTimeTable.time_id = this.TimeTableId;
            weekTimeTable.time_name = this.TimeTableName;
            weekTimeTable.lux_id = this.LuxId;
            weekTimeTable.lux_on_value = this.LuxOnValue;
            weekTimeTable.lux_off_value = this.LuxOffValue;
            weekTimeTable.light_off_offset = this.LightOffOffset;
            weekTimeTable.light_on_offset = this.LightOnOffset;
            weekTimeTable.LuxEffective = this.LuxEffective;
            foreach (var t in this.OneWeekTimeTable)
            {
                weekTimeTable.LstOneWeekOpenCloseControl.Add(t.BackToWeekTimeTableItemInfomation());
            }
            return weekTimeTable;
        }
        #endregion

        public void ReloadLux()
        {
            if (_luxCollection == null)
                _luxCollection = new ObservableCollection<LuxViewModel>();
            _luxCollection.Clear();
            foreach (var t in LuxGetServer.GetAllLuxEquipment)
            {
                _luxCollection.Add(new LuxViewModel { LuxId = t.Value, LuxName = t.Name });
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

        void UpdateChildOnOffSet()
        {
            foreach (var t in OneWeekTimeTable)
            {
                t.LightOnOffSet = LightOnOffset;
            }
        }

        void UpdateChildOffOffSet()
        {
            foreach (var t in OneWeekTimeTable)
            {
                t.LightOffOffSet = LightOffOffset;
            }
        }
    }
}
