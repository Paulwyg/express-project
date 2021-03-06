﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipemntLightFault.Services;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.ViewModel
{
    public class FaultRecordViewModel:ObservableObject
    {
        public int RtuLgcId;
        public int LoopId;
        public long DateCreate;
        public long DateFirst;
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        private int _rtuId;

        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId != value)
                {
                    _rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);
                }
            }
        }

        private int _faultId;

        public int FaultId
        {
            get { return _faultId; }
            set
            {
                if (_faultId != value)
                {
                    _faultId = value;
                    this.RaisePropertyChanged(() => this.FaultId);
                }
            }
        }

        private string _faultName;

        public string FaultName
        {
            get { return _faultName; }
            set
            {
                if (_faultName != value)
                {
                    _faultName = value;
                    this.RaisePropertyChanged(() => this.FaultName);
                }
            }
        }

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (_rtuName != value)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private string _loopName;

        public string LoopName
        {
            get { return _loopName; }
            set
            {
                if (_loopName != value)
                {
                    _loopName = value;
                    this.RaisePropertyChanged(() => this.LoopName);
                }
            }
        }

        private string _dataCreateTime;

        public string DataCreateTime
        {
            get { return _dataCreateTime; }
            set
            {
                if (_dataCreateTime != value)
                {
                    _dataCreateTime = value;
                    this.RaisePropertyChanged(() => this.DataCreateTime);
                }
            }
        }

        private int _priorityLevel;

        public int PriorityLevel
        {
            get { return _priorityLevel; }
            set
            {
                if (_priorityLevel != value)
                {
                    _priorityLevel = value;
                    this.RaisePropertyChanged(() => this.PriorityLevel);
                }
            }
        }

        private string _paidan;

        public string Paidan
        {
            get { return _paidan; }
            set
            {
                if (_paidan != value)
                {
                    _paidan = value;
                    this.RaisePropertyChanged(() => this.Paidan);
                }
            }
        }

        private string _orderId;

        public string OrderId
        {
            get { return _orderId; }
            set
            {
                if (_orderId != value)
                {
                    _orderId = value;
                    this.RaisePropertyChanged(() => this.OrderId);
                }
            }
        }

        private string _color;
        /// <summary>
        /// 新故障标红
        /// </summary>
        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    this.RaisePropertyChanged(() => this.Color);
                }
            }
        }


        private string _bkcolor;
        /// <summary>
        /// 同一终端背景色相同
        /// </summary>
        public string BkColor
        {
            get { return _bkcolor; }
            set
            {
                if (_bkcolor != value)
                {
                    _bkcolor = value;
                    this.RaisePropertyChanged(() => this.BkColor);
                }
            }
        }


        private int _counts;
        /// <summary>
        /// 次数
        /// </summary>
        public int AlarmCount
        {
            get { return _counts; }
            set
            {
                if (_counts != value)
                {
                    _counts = value;
                    this.RaisePropertyChanged(() => this.AlarmCount);
                }
            }
        }

        private string _dataFirstTime;
        /// <summary>
        /// 次数
        /// </summary>
        public string DataFirstTime
        {
            get { return _dataFirstTime; }
            set
            {
                if (_dataFirstTime != value)
                {
                    _dataFirstTime = value;
                    this.RaisePropertyChanged(() => this.DataFirstTime);
                }
            }
        }

        private string _cqj;
        /// <summary>
        /// 城区局
        /// </summary>
        public string CQJ
        {
            get { return _cqj; }
            set
            {
                if (_cqj != value)
                {
                    _cqj = value;
                    this.RaisePropertyChanged(() => this.CQJ);
                }
            }
        }

        private string _dygh;
        /// <summary>
        /// 电源杆号
        /// </summary>
        public string DYGH
        {
            get { return _dygh; }
            set
            {
                if (_dygh != value)
                {
                    _dygh = value;
                    this.RaisePropertyChanged(() => this.DYGH);
                }
            }
        }

        private string _voltage;
        /// <summary>
        /// 电压
        /// </summary>
        public string Voltage
        {
            get { return _voltage; }
            set
            {
                if (_voltage != value)
                {
                    _voltage = value;
                    this.RaisePropertyChanged(() => this.Voltage);
                }
            }
        }

        private string _current;
        /// <summary>
        /// 电流
        /// </summary>
        public string Current 
        {
            get { return _current; }
            set
            {
                if (_current != value)
                {
                    _current = value;
                    this.RaisePropertyChanged(() => this.Current);
                }
            }
        }

        private string _power;
        /// <summary>
        /// 功率
        /// </summary>
        public string Power
        {
            get { return _power; }
            set
            {
                if (_power != value)
                {
                    _power = value;
                    this.RaisePropertyChanged(() => this.Power);
                }
            }
        }

        private string _lowLimit;
        /// <summary>
        ///   下限
        /// </summary>
        public string LowLimit
        {
            get { return _lowLimit; }
            set
            {
                if (_lowLimit != value)
                {
                    _lowLimit = value;
                    this.RaisePropertyChanged(() => this.LowLimit);
                }
            }
        }

        private string _highLimit;
        /// <summary>
        ///   上限
        /// </summary>
        public string HighLimit
        {
            get { return _highLimit; }
            set
            {
                if (_highLimit != value)
                {
                    _highLimit = value;
                    this.RaisePropertyChanged(() => this.HighLimit);
                }
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                }
            }
        }
    }

    public class AllFaultRecordsViewModel:ObservableObject
    {
      //  public List<int> FautsContas = new List<int>(); 
        private string _faultName;

        public string FaultName
        {
            get { return _faultName; }
            set
            {
                if (_faultName != value)
                {
                    _faultName = value;
                    this.RaisePropertyChanged(() => this.FaultName);
                }
            }
        }


        private int  _faultNamesfsd;

        public int FaultCountNew
        {
            get { return _faultNamesfsd; }
            set
            {
                if (_faultNamesfsd != value)
                {
                    _faultNamesfsd = value;
                    this.RaisePropertyChanged(() => this.FaultCountNew);
                    FaultCountNewShow = value > 0?Visibility.Visible :Visibility.Collapsed ;
                }
            }
        }

        private Visibility _faultNamesfasdasd;

        public Visibility FaultCountNewShow
        {
            get { return _faultNamesfasdasd; }
            set
            {
                if (_faultNamesfasdasd != value)
                {
                    _faultNamesfasdasd = value;
                    this.RaisePropertyChanged(() => this.FaultCountNewShow);
                }
            }
        }

        //private int FaultCountOldX;
        private int _faultNasdfsdfme;

        public int FaultCountOld
        {
            get { return _faultNasdfsdfme; }
            set
            {
                if (_faultNasdfsdfme != value)
                {
                    _faultNasdfsdfme = value;
                    this.RaisePropertyChanged(() => this.FaultCountOld);
                }
            }
        }

        public bool IsOther = false;
        public List<int> FaultId = new List<int>();
       

        private ObservableCollection<FaultRecordViewModel> _recordItems;

        /// <summary>
        /// 故障表格
        /// </summary>
        public ObservableCollection<FaultRecordViewModel> RecordItems
        {
            get
            {
                if (_recordItems == null)
                {
                    _recordItems = new ObservableCollection<FaultRecordViewModel>();
                }
                return _recordItems;
            }
            set
            {
                if (value == _recordItems) return;
                _recordItems = value;
                this.RaisePropertyChanged(() => RecordItems);
            }
        }
    }

    public class CurrentItemViewModel : ObservableObject
    {
        public CurrentItemViewModel()
        {
            

        }

        private bool _isenable;

        public bool IsEnable
        {
            get { return _isenable; }
            set
            {
                if (_isenable != value)
                {
                    _isenable = value;
                    this.RaisePropertyChanged(() => this.IsEnable);
                }
            }
        }

        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }

        private int _count;

        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    this.RaisePropertyChanged(() => this.Count);
                }
            }
        }

        private int _stTime;

        public int StTime
        {
            get { return _stTime; }
            set
            {
                if (_stTime != value)
                {
                    _stTime = value;
                    this.RaisePropertyChanged(() => this.StTime);
                }
            }
        }


        private int _endTime;

        public int EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime != value)
                {
                    _endTime = value;
                    this.RaisePropertyChanged(() => this.EndTime);
                }
            }
        }

        private bool _showmoren;

        public bool ShowMoRen
        {
            get { return _showmoren; }
            set
            {
                if (_showmoren != value)
                {
                    _showmoren = value;
                    this.RaisePropertyChanged(() => this.EndTime);
                }
            }
        }

        #region 选中故障

        //private FaultItemViewModel _selectedFault1;

        //public FaultItemViewModel SelectedFault1
        //{
        //    get { return _selectedFault1; }
        //    set
        //    {
        //        if (_selectedFault1 != value)
        //        {
        //            _selectedFault1 = value;
        //            this.RaisePropertyChanged(() => this.SelectedFault1);
        //        }
        //    }
        //}

        //private FaultItemViewModel _selectedFault2;

        //public FaultItemViewModel SelectedFault2
        //{
        //    get { return _selectedFault2; }
        //    set
        //    {
        //        if (_selectedFault2 != value)
        //        {
        //            _selectedFault2 = value;
        //            this.RaisePropertyChanged(() => this.SelectedFault2);
        //        }
        //    }
        //}

        //private FaultItemViewModel _selectedFault3;

        //public FaultItemViewModel SelectedFault3
        //{
        //    get { return _selectedFault3; }
        //    set
        //    {
        //        if (_selectedFault3 != value)
        //        {
        //            _selectedFault3 = value;
        //            this.RaisePropertyChanged(() => this.SelectedFault3);
        //        }
        //    }
        //}

        //private FaultItemViewModel _selectedFault4;

        //public FaultItemViewModel SelectedFault4
        //{
        //    get { return _selectedFault4; }
        //    set
        //    {
        //        if (_selectedFault4 != value)
        //        {
        //            _selectedFault4 = value;
        //            this.RaisePropertyChanged(() => this.SelectedFault4);
        //        }
        //    }
        //}

        //private FaultItemViewModel _selectedFault5;

        //public FaultItemViewModel SelectedFault5
        //{
        //    get { return _selectedFault5; }
        //    set
        //    {
        //        if (_selectedFault5 != value)
        //        {
        //            _selectedFault5 = value;
        //            this.RaisePropertyChanged(() => this.SelectedFault5);
        //        }
        //    }
        //}

        //private int _fault1;

        //public int Fault1
        //{
        //    get { return _fault1; }
        //    set
        //    {
        //        if (_fault1 != value)
        //        {
        //            _fault1 = value;
        //            this.RaisePropertyChanged(() => this.Fault1);
        //            foreach (var ff in FaultComboBox)
        //            {
        //                if (ff.Id == _fault1)
        //                {
        //                    SelectedFault1 = ff;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        //private int _fault2;

        //public int Fault2
        //{
        //    get { return _fault2; }
        //    set
        //    {
        //        if (_fault2 != value)
        //        {
        //            _fault2 = value;
        //            this.RaisePropertyChanged(() => this.Fault2);
        //            foreach (var ff in FaultComboBox)
        //            {
        //                if (ff.Id == _fault2)
        //                {
        //                    SelectedFault2 = ff;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        //private int _fault3;

        //public int Fault3
        //{
        //    get { return _fault3; }
        //    set
        //    {
        //        if (_fault3 != value)
        //        {
        //            _fault3 = value;
        //            this.RaisePropertyChanged(() => this.Fault3);
        //            foreach (var ff in FaultComboBox)
        //            {
        //                if (ff.Id == _fault3)
        //                {
        //                    SelectedFault3 = ff;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        //private int _fault4;

        //public int Fault4
        //{
        //    get { return _fault4; }
        //    set
        //    {
        //        if (_fault4 != value)
        //        {
        //            _fault4 = value;
        //            this.RaisePropertyChanged(() => this.Fault4);
        //            foreach (var ff in FaultComboBox)
        //            {
        //                if (ff.Id == _fault4)
        //                {
        //                    SelectedFault4 = ff;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}
        //private int _fault5;

        //public int Fault5
        //{
        //    get { return _fault5; }
        //    set
        //    {
        //        if (_fault5 != value)
        //        {
        //            _fault5 = value;
        //            this.RaisePropertyChanged(() => this.Fault5);
        //            foreach (var ff in FaultComboBox)
        //            {
        //                if (ff.Id == _fault5)
        //                {
        //                    SelectedFault5 = ff;
        //                    break;
        //                }
        //            }
        //        }

        //    }
        //}

        #endregion

        private ObservableCollection<NameIntBool> names = null;

        public ObservableCollection<NameIntBool> Names
        {
            get
            {
                if (names == null)
                {
                    names = new ObservableCollection<NameIntBool>();
                    names.Add(new NameIntBool() {Name = "无"});
                    names.Add(new NameIntBool() {Name = "无"});
                    names.Add(new NameIntBool() {Name = "无"});
                    names.Add(new NameIntBool() {Name = "无"});
                    names.Add(new NameIntBool() {Name = "无"});
                    names.Add(new NameIntBool() {Name = "无"});
                }
                return names;
            }
            set
            {
                names = value;
                this.RaisePropertyChanged(() => this.Names);
            }
        }

        public Dictionary<int, List<int>> SelectedFaults = new Dictionary<int, List<int>>();

        //private List<int> _chosenFaults;

        ///// <summary>
        ///// 已选故障
        ///// </summary>
        //public List<int> ChosenFaults
        //{
        //    get
        //    {
        //        if (_chosenFaults == null)
        //        {
        //            _chosenFaults = new List<int>();
        //        }
        //        return _chosenFaults;
        //    }
        //    set
        //    {
        //        if (value == _chosenFaults) return;
        //        _chosenFaults = value;
        //        this.RaisePropertyChanged(() => ChosenFaults);
        //    }
        //}

        //private string _selectedFault;

        //public string SelectedFault
        //{
        //    get { return _selectedFault; }
        //    set
        //    {
        //        if (_selectedFault != value)
        //        {
        //            _selectedFault = value;
        //            this.RaisePropertyChanged(() => this.SelectedFault);                                      
        //        }
        //    }
        //}

        //private ObservableCollection<FaultItemViewModel > _faultComboBox;

        //public ObservableCollection<FaultItemViewModel> FaultComboBox
        //{
        //    get { return _faultComboBox; }
        //    set
        //    {
        //        if (_faultComboBox != value)
        //        {
        //            _faultComboBox = value;
        //            this.RaisePropertyChanged(() => this.FaultComboBox);

        //        }
        //    }
        //}
    }

    //public class FaultItemViewModel:ObservableObject
    //{
    //    private int _Id;

    //    public int Id
    //    {
    //        get { return _Id; }
    //        set
    //        {
    //            if (_Id != value)
    //            {
    //                _Id = value;
    //                this.RaisePropertyChanged(() => this.Id);
    //            }
    //        }
    //    }

    //    private int _index;

    //    public int Index
    //    {
    //        get { return _index; }
    //        set
    //        {
    //            if (_index != value)
    //            {
    //                _index = value;
    //                this.RaisePropertyChanged(() => this.Index);
    //            }
    //        }
    //    }

    //    private int _ruleId;

    //    public int RuleId
    //    {
    //        get { return _ruleId; }
    //        set
    //        {
    //            if (_ruleId != value)
    //            {
    //                _ruleId = value;
    //                this.RaisePropertyChanged(() => this.RuleId);
    //            }
    //        }
    //    }

       

    //    private string _faultName;

    //    public string FaultName
    //    {
    //        get { return _faultName; }
    //        set
    //        {
    //            if (_faultName != value)
    //            {
    //                _faultName = value;
    //                this.RaisePropertyChanged(() => this.FaultName);
    //            }
    //        }
    //    }

    //    private bool _isChecked;

    //    public bool IsChecked
    //    {
    //        get { return _isChecked; }
    //        set
    //        {
    //            if (_isChecked != value)
    //            {
    //                _isChecked = value;
    //                this.RaisePropertyChanged(() => this.IsChecked);
    //                //if (IsChecked == true)
    //                //{
    //                //    //var args = new PublishEventArgs()
    //                //    //{
    //                //    //    EventId = EventIdAssign.EquipmentFaultIsChecked,
    //                //    //    EventType = PublishEventType.Core
    //                //    //};
    //                //    //args.AddParams(Id,this);
    //                //    //EventPublish.PublishEvent(args);

    //                //    var args2 = new PublishEventArgs()
    //                //    {
    //                //        EventId = EventIdAssign.EquipmentFaultIsCheckedCount,
    //                //        EventType = PublishEventType.Core
    //                //    };
    //                //    args2.AddParams(this );
    //                //    EventPublish.PublishEvent(args2);
    //                //}
    //            }
    //        }
    //    }
    //}
}
