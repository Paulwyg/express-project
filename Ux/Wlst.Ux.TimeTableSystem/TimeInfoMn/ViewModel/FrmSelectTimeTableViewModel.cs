using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.Core.CoreServices;
using Wlst.Sr.TimeTableSystem.Services;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel
{

    public class FrmSelectTimeTableViewModel : ObservableObject
    {
        public FrmSelectTimeTableViewModel()
        {

        }

        public FrmSelectTimeTableViewModel(ObservableCollection<TimeTableInfomationItem> items, bool has3005)
        {
            this.TimeTables.Clear();
            TimeTables.Add(new IdNameDesc()
            {
                Id = -1,
                Name = "不操作",
                NameDesc = "不执行开灯与关灯操作"
            });
            foreach (var t in items)
            {
                
                bool bolfind = false;
                foreach (var tt in this.TimeTables)
                {
                    if (tt.Id == t.TimeId)
                    {
                        bolfind = true;
                        break;
                    }
                    else if (t.MainIsOverOne[0] && has3005 && Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection == false)
                    {
                        bolfind = true;
                        break;
                    }
                    
                }
                if (!bolfind)
                {
                    this.TimeTables.Add(new IdNameDesc()
                                            {
                                                Id = t.TimeId,
                                                Name = t.TimeName,
                                                NameDesc = t.TimeDesc
                                            });
                }
            }

        }


        public FrmSelectTimeTableViewModel(ObservableCollection<TimeTableInfomationItem> items, List<int> lsttimetable, bool has3005)
        {
            this.TimeTables.Clear();
            string idname = "";

            Mris = new ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle>();
            if (items.Count > 0)
            {
                for (int i = 0; i < 7; i++)
                {
                    Mris.Add(new TimeTableInfomationItem.MainRuleItemsStyle()
                    {
                        MainWeek = items[0].MainRuleItems[i].MainWeek,
                        MainDate = items[0].MainRuleItems[i].MainDate,
                        MainTimeOnOne = "      -",
                        MainTimeOnTwo = "      -",
                        MainTimeOffTwo = "      -",
                        MainTimeOffOne = "      -",
                        MainSunRise = items[0].MainRuleItems[i].MainSunRise,
                        MainSunSet = items[0].MainRuleItems[i].MainSunSet
                    });
                }

            }
            
            TimeTables.Add(new IdNameDesc()
            {
                Id = -1,
                Name = "不操作",
                NameDesc = "不执行开灯与关灯操作",
                MainRuleItems = Mris,
                IdName = "不操作",
                MainIsOverOne = new ObservableCollection<bool>() { false, false, false },
                MainType = new ObservableCollection<int>() { 55, 75, 160, 30, 50, 160 }
            });
            foreach (var t in items)
            {
                bool bolfind = false;
                foreach (var tt in this.TimeTables)
                {
                    if (tt.Id == t.TimeId)
                    {
                        bolfind = true;
                        break;
                    }
                    else if (t.MainIsOverOne[0] && has3005 && Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection == false)
                    {
                        bolfind = true;
                        break;
                    }
                }
                if (!bolfind)
                {
                    idname = t.TimeId.ToString("d4") + " " + t.TimeName;
                    //var miot = new ObservableCollection<Visibility>() { Visibility.Collapsed, Visibility.Collapsed };
                    //if (t.MainIsOverOne[1]==true)
                    //{
                    //    miot[0] = Visibility.Visible;
                    //}
                    //else
                    //{
                    //    miot[1] = Visibility.Visible;
                    //}

                    this.TimeTables.Add(new IdNameDesc()
                    {
                        Id = t.TimeId,
                        Name = t.TimeName,
                        NameDesc = t.TimeDesc,
                        MainRuleItems = t.MainRuleItems,
                        IdName = idname,
                        MainIsOverOne = t.MainIsOverOne,
                        MainType = t.MainType
                    });
                }


            }


            TimeTableComboBoxSelected = new ObservableCollection<IdNameDesc>();
            for (int i = 0; i < 8; i++)
            {
                TimeTableComboBoxSelected.Add(new IdNameDesc()
                {
                    Id = -1,
                    Name = "不操作",
                    NameDesc = "不执行开灯与关灯操作",
                    MainRuleItems = Mris
                });

                for (int t = 0; t < TimeTables.Count; t++)
                {
                    if (TimeTables[t].Id == lsttimetable[i])
                    {
                        TimeTableComboBoxSelected[i] = TimeTables[t];
                    }
                }
            }
        }


        private ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle> _mris;

        public ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle> Mris
        {
            get { return _mris; }
            set
            {
                if (value != _mris)
                {
                    _mris = value;
                    this.RaisePropertyChanged(() => this.Mris);
                }
            }
        }

        #region data

        private ObservableCollection<IdNameDesc> _timeTableViewModels;

        /// <summary>
        /// 所有时间表集合
        /// </summary>
        public ObservableCollection<IdNameDesc> TimeTables
        {
            get
            {
                if (_timeTableViewModels == null)
                    _timeTableViewModels = new ObservableCollection<IdNameDesc>();
                return _timeTableViewModels;
            }
        }

        private IdNameDesc _currenSelectItem;

        /// <summary>
        /// 当前选中的时间表
        /// </summary>
        public IdNameDesc CurrentSelectItem
        {
            get { return _currenSelectItem; }
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
                if (value == _oldSelectTimeTableId)
                {
                    if (value == 0) OldSelectTimeTableName = "不操作";
                    return;
                }

                _oldSelectTimeTableId = value;
                this.RaisePropertyChanged(() => this.OldSelectTimeTableId);

                bool find = false;
                foreach (var t in this.TimeTables)
                {
                    if (t.Id == _oldSelectTimeTableId)
                    {
                        CurrentSelectItem = t;
                        OldSelectTimeTableName = t.Name;
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

        private string _showRtuOrGroupId;

        public string ShowRtuOrGroupId
        {
            get { return _showRtuOrGroupId; }
            set
            {
                if (value != _showRtuOrGroupId)
                {
                    _showRtuOrGroupId = value;
                    this.RaisePropertyChanged(() => this.ShowRtuOrGroupId);

                }
            }
        }

        private string _msg;

        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value != _msg)
                {
                    _msg = value;
                    this.RaisePropertyChanged(() => this.Msg);

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

        #endregion

        #region

        private ObservableCollection<int> _lsttimetable;

        public ObservableCollection<int> Lsttimetable
        {
            get { return _lsttimetable; }
            set
            {
                if (value != _lsttimetable)
                {
                    _lsttimetable = value;
                    this.RaisePropertyChanged(() => this.Lsttimetable);
                }
            }
        }

        private ObservableCollection<IdNameDesc> _timetablecomboboxselected;

        public ObservableCollection<IdNameDesc> TimeTableComboBoxSelected
        {
            get
            {
                return _timetablecomboboxselected;
            }
            set
            {
                if (_timetablecomboboxselected != value)
                {
                    _timetablecomboboxselected = value;
                    this.RaisePropertyChanged(() => this.TimeTableComboBoxSelected);
                    if (value == null || value.Count == 0) return;

                    for (int i = 0; i < 8; i++)
                    {
                        TimeTableComboBoxSelected[i] = value[i];
                    }

                }
            }
        }

        private ObservableCollection<ObservableCollection<TimeTableComboBox>> _timecombobox;

        public ObservableCollection<ObservableCollection<TimeTableComboBox>> TimeComboBox
        {
            get
            {
                if (_timecombobox == null)
                {
                    _timecombobox = new ObservableCollection<ObservableCollection<TimeTableComboBox>>();
                }
                return _timecombobox;
            }
        }

        public class TimeTableComboBox : Wlst.Cr.Core.CoreServices.ObservableObject
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

            private ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle> _show;

            public ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle> Show
            {
                get { return _show; }
                set
                {
                    if (value != _show)
                    {
                        _show = value;
                        this.RaisePropertyChanged(() => this.Show);
                    }
                }
            }
        }






        #endregion
    }
}

//public class GroupWatchTimeTableViewModel : ObservableObject
//{
//    public GroupWatchTimeTableViewModel()
//    {

//    }






//    #region

//    private List<int> _lsttimetable;

//    public List<int> Lsttimetable
//    {
//        get { return _lsttimetable; }
//        set
//        {
//            if (value != _lsttimetable)
//            {
//                _lsttimetable = value;
//                this.RaisePropertyChanged(() => this.Lsttimetable);
//            }
//        }
//    }

//    private List<TimeTableComboBox> _timetablecomboboxselected;
//    public List<int> selecttimetableid = new List<int>();
//    public List<TimeTableComboBox> TimeTableComboBoxSelected
//    {
//        get
//        {
//            return _timetablecomboboxselected;
//        }
//        set
//        {
//            if (_timetablecomboboxselected != value)
//            {
//                _timetablecomboboxselected = value;
//                this.RaisePropertyChanged(() => this.TimeTableComboBoxSelected);
//                if (value == null) return;
//                for (int i = 0; i < 8; i++)
//                {
//                    TimeTableComboBoxSelected[i] = value[i];
//                }
//            }
//        }
//    }

//    private List<TimeTableComboBox> _timecombobox;
//    public List<TimeTableComboBox> TimeComboBox
//    {
//        get
//        {
//            return _timecombobox;
//        }
//        set
//        {
//            if (_timecombobox != value)
//            {
//                _timecombobox = value;
//                this.RaisePropertyChanged(() => this.TimeComboBox);
//            }
//        }
//    }


//    public class TimeTableComboBox : Wlst.Cr.Core.CoreServices.ObservableObject
//    {
//        private int _key;

//        public int Key
//        {
//            get { return _key; }
//            set
//            {
//                if (_key != value)
//                {
//                    _key = value;
//                    this.RaisePropertyChanged(() => this.Key);
//                }
//            }
//        }

//        private string _value;

//        public string Value
//        {
//            get { return _value; }
//            set
//            {
//                if (value != _value)
//                {
//                    _value = value;
//                    this.RaisePropertyChanged(() => this.Value);
//                }
//            }
//        }

//        private ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle> _show;

//        public ObservableCollection<TimeTableInfomationItem.MainRuleItemsStyle> Show
//        {
//            get { return _show; }
//            set
//            {
//                if (value != _show)
//                {
//                    _show = value;
//                    this.RaisePropertyChanged(() => this.Show);
//                }
//            }
//        }
//    }
//    #endregion

//}
