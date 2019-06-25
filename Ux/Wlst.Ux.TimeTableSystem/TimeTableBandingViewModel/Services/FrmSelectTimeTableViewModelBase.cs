using System.Collections.ObjectModel;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel.Services
{
    public class FrmSelectTimeTableViewModelBase : ObservableObject
    {
        public FrmSelectTimeTableViewModelBase()
        {
            this.TimeTables.Clear();
            TimeTables.Add(new TimeTableViewModel()
                               {
                                   TimeTableId = -1,
                                   TimeTableName = "不操作",
                                   TimeDesc = "不执行开灯与关灯操作"

                               });
            foreach (var t in Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfoList)
            {
                bool bolfind = false;
                foreach (var tt in this.TimeTables)
                {
                    if (tt.TimeTableId == t.TimeId)
                    {
                        tt.ReloadLux();
                        bolfind = true;
                        break;
                    }
                }
                if (!bolfind)
                {
                    this.TimeTables.Add(new TimeTableViewModel(t));
                }
            }
        }

        #region data

        private ObservableCollection<TimeTableViewModel> _timeTableViewModels;

        /// <summary>
        /// 所有时间表集合
        /// </summary>
        public ObservableCollection<TimeTableViewModel> TimeTables
        {
            get
            {
                if (_timeTableViewModels == null)
                    _timeTableViewModels = new ObservableCollection<TimeTableViewModel>();
                return _timeTableViewModels;
            }
        }

        private TimeTableViewModel _currenSelectItem;

        /// <summary>
        /// 当前选中的时间表
        /// </summary>
        public TimeTableViewModel CurrentSelectItem
        {
            get
            {
                if (_currenSelectItem == null) _currenSelectItem = new TimeTableViewModel();
                return _currenSelectItem;
            }
            set
            {
                if (_currenSelectItem != value)
                {
                    _currenSelectItem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectItem);
                }
            }
        }

        #endregion

        #region

        private int _oldSelectTimeTableId;

        public int OldSelectTimeTableId
        {
            get { return _oldSelectTimeTableId; }
            set
            {
                if (value != _oldSelectTimeTableId)
                {
                    _oldSelectTimeTableId = value;
                    this.RaisePropertyChanged(() => this.OldSelectTimeTableId);

                    bool find = false;
                    foreach (var t in this.TimeTables)
                    {
                        if (t.TimeTableId == _oldSelectTimeTableId)
                        {
                            CurrentSelectItem = t;
                            OldSelectTimeTableName = t.TimeTableName;
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        OldSelectTimeTableName = "该地址的时间表已经不存在，不执行开关灯操作...";
                    }
                }
            }
        }

        private string _oldSelectTimeTableName;

        public string OldSelectTimeTableName
        {
            get { return _oldSelectTimeTableName; }
            set
            {
                if (value != _oldSelectTimeTableName)
                {
                    _oldSelectTimeTableName = value;
                    this.RaisePropertyChanged(() => this.OldSelectTimeTableName);
                }
            }
        }


        private int _selectRtuOrGroupId;

        public int SelectRtuOrGroupId
        {
            get { return _selectRtuOrGroupId; }
            set
            {
                if (value != _selectRtuOrGroupId)
                {
                    _selectRtuOrGroupId = value;
                    this.RaisePropertyChanged(() => this.SelectRtuOrGroupId);

                }
            }
        }

        private int  _selectRtuOrGroupPhyId;

        public int SelectRtuOrGroupPhyId
        {
            get { return _selectRtuOrGroupPhyId; }
            set
            {
                if (value != _selectRtuOrGroupPhyId)
                {
                    _selectRtuOrGroupPhyId = value;
                    this.RaisePropertyChanged(() => this.SelectRtuOrGroupPhyId);

                }
            }
        }

        private int _selectKloop;

        public int SelectKloop
        {
            get { return _selectKloop; }
            set
            {
                if (value != _selectKloop)
                {
                    _selectKloop = value;
                    this.RaisePropertyChanged(() => this.SelectKloop);
                   // SelectRtuLoop = "K" + value;
                }
            }
        }

        private string _selectRtuOrGroupName;

        public string SelectRtuOrGroupName
        {
            get { return _selectRtuOrGroupName; }
            set
            {
                if (value != _selectRtuOrGroupName)
                {
                    _selectRtuOrGroupName = value;
                    this.RaisePropertyChanged(() => this.SelectRtuOrGroupName);
                }
            }
        }


        //private string _sSelectRtuLoop;

        //public string SelectRtuLoop
        //{
        //    get { return _sSelectRtuLoop; }
        //    set
        //    {
        //        if (value != _sSelectRtuLoop)
        //        {
        //            _sSelectRtuLoop = value;
        //            this.RaisePropertyChanged(() => this.SelectRtuLoop);
        //        }
        //    }
        //}



        #endregion
    }
}
