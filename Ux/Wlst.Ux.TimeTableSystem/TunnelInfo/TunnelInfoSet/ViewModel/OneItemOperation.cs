using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.Services;
using Wlst.client;

namespace Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet.ViewModel
{
    public partial class OneItemOperation : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public OneItemOperation(ObservableCollection<OneItemTerminal> items )
        {
            OperationName = "新操作";
            OperationDesc = "";
            MaxLux = 100;
            LastTimeHour = 9;
            LastTimeMinute = 0;
            SelectedItems = items;
           
        }


        ////光控改变时的操作判断
        //public bool LuxChanged;
    }

    /// <summary>
    /// 操作信息
    /// </summary>
    public partial class OneItemOperation
    {


        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        private string _operationName;
        /// <summary>
        /// 操作名称  
        /// </summary>
        public string OperationName
        {
            get { return _operationName; }
            set
            {
                if (value != _operationName)
                {
                    _operationName = value;
                    this.RaisePropertyChanged(() => this.OperationName);
                }
            }
        }



        [StringLength(30, ErrorMessage = "方案描述长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        private string _operationDesc;
        /// <summary>
        /// 操作描述
        /// </summary>
        public string OperationDesc
        {
            get { return _operationDesc; }
            set
            {
                if (value != _operationDesc)
                {
                    _operationDesc = value;
                    this.RaisePropertyChanged(() => this.OperationDesc);
                }
            }
        }



        private int _maxLux;
        /// <summary>
        ///最大光控值
        /// </summary>
        public int MaxLux
        {
            get { return _maxLux; }
            set
            {
                if (value != _maxLux)
                {
                    _maxLux = value;
                    this.RaisePropertyChanged(() => this.MaxLux);
                }
            }
        }



        private int _lastTimeHour;
        /// <summary>
        ///时控操作最后时间
        /// </summary>
        public int LastTimeHour
        {
            get { return _lastTimeHour; }
            set
            {
                if (value != _lastTimeHour)
                {
                    if (value < 0) value = 0;
                    if (value > 23) value = 23;
                    _lastTimeHour = value;
                    this.RaisePropertyChanged(() => this.LastTimeHour);

                    
                }
                StrLastTimeHour = LastTimeHour < 10 ? "0" + LastTimeHour : LastTimeHour + "";
            }
        }

        private string _strlastTimeHour;
        /// <summary>
        ///时控操作最后时间
        /// </summary>
        public string StrLastTimeHour
        {
            get { return _strlastTimeHour; }
            set
            {
                if (value != _strlastTimeHour)
                {
                    _strlastTimeHour = value;
                    this.RaisePropertyChanged(() => this.StrLastTimeHour);
                }
            }
        }


        private int _lastTimeMinute;
        /// <summary>
        ///时控操作最后时间
        /// </summary>
        public int LastTimeMinute
        {
            get { return _lastTimeMinute; }
            set
            {
                if (value != _lastTimeMinute)
                {
                    if (value < 0) value = 0;
                    if (value > 59) value = 59;
                    _lastTimeMinute = value;
                    this.RaisePropertyChanged(() => this.LastTimeMinute);

                   
                    
                }
                if (LastTimeMinute == 0)
                {
                    StrLastTimeMinute = "00";
                }
                else
                {
                    StrLastTimeMinute = LastTimeMinute < 10 ? "0" + LastTimeMinute : LastTimeMinute + "";
                }
            }
        }

        private string _strlastTimeMinute;
        /// <summary>
        ///时控操作最后时间
        /// </summary>
        public string StrLastTimeMinute
        {
            get { return _strlastTimeMinute; }
            set
            {
                if (value != _strlastTimeMinute)
                {
                    _strlastTimeMinute = value;
                    this.RaisePropertyChanged(() => this.StrLastTimeMinute);
                }
            }
        }

        //#region LuxCollection

        //private ObservableCollection<IdNameDesc> _luxCollection;
        //public ObservableCollection<IdNameDesc> LuxCollection
        //{
        //    get
        //    {
        //        if (_luxCollection == null)
        //        {
        //            _luxCollection = new ObservableCollection<IdNameDesc>();
        //            _luxCollection.Add(new IdNameDesc { Id = 0, Name = " " });
        //            foreach (var t in LuxGetServer.GetAllLuxEquipment)
        //            {
        //                _luxCollection.Add(new IdNameDesc { Id = t.Value, Name = t.Name, NameDesc = t.Value2.ToString("d4") + "-" + t.Name });
        //            }
        //            if (LuxId > 0)
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
        //    set
        //    {
        //        if (_luxCollection == value) return;
        //        _luxCollection = value;
        //        RaisePropertyChanged(() => LuxCollection);
        //    }
        //}
        //#endregion
        
        //#region CurrentSelectLux
        //private IdNameDesc _currentSelectLux;

        ///// <summary>
        ///// 当前选中的光控
        ///// </summary>
        //public IdNameDesc CurrentSelectLux
        //{
        //    get
        //    {
        //        return _currentSelectLux ?? (_currentSelectLux = new IdNameDesc());
        //    }
        //    set
        //    {
        //        if (_currentSelectLux == value) return;
        //        _currentSelectLux = value;
        //        RaisePropertyChanged(() => CurrentSelectLux);
        //        if (_currentSelectLux != null)
        //            LuxId = _currentSelectLux.Id;


        //        //if (_currentSelectLux.Id>0)
        //        //    ShowCurrentSelectLux2 = Visibility.Visible;
        //        //else
        //        //    ShowCurrentSelectLux2 = Visibility.Hidden;

        //    }
        //}

        //#endregion

        //#region LuxCollection2

        //private ObservableCollection<IdNameDesc> _luxCollection2;
        //public ObservableCollection<IdNameDesc> LuxCollection2
        //{
        //    get
        //    {
        //        if (_luxCollection2 == null)
        //        {
        //            _luxCollection2 = new ObservableCollection<IdNameDesc>();
        //            _luxCollection2.Add(new IdNameDesc { Id = 0, Name = " " });
        //            foreach (var t in LuxGetServer.GetAllLuxEquipment)
        //            {
        //                if (t.Value != CurrentSelectLux.Id)
        //                {
        //                    _luxCollection2.Add(new IdNameDesc { Id = t.Value, Name = t.Name, NameDesc = t.Value2.ToString("d4") + "-" + t.Name });
        //                }

        //            }


        //            if (LuxId2 != 0)
        //            {
        //                foreach (var t in _luxCollection2.Where(t => t.Id == LuxId2))
        //                {
        //                    CurrentSelectLux2 = t;
        //                    break;
        //                }
        //            }
        //        }



        //        return _luxCollection2;
        //    }
        //    set
        //    {
        //        if (_luxCollection2 == value) return;
        //        _luxCollection2 = value;
        //        RaisePropertyChanged(() => LuxCollection2);
        //    }
        //}
        //#endregion

        //#region CurrentSelectLux2
        //private IdNameDesc _currentSelectLux2;

        ///// <summary>
        ///// 备用光控
        ///// </summary>
        //public IdNameDesc CurrentSelectLux2
        //{
        //    get { return _currentSelectLux2 ?? (_currentSelectLux = new IdNameDesc()); }
        //    set
        //    {
        //        if (_currentSelectLux2 == value) return;
        //        _currentSelectLux2 = value;
        //        RaisePropertyChanged(() => CurrentSelectLux2);
        //        if (_currentSelectLux2 != null)
        //            LuxId2 = _currentSelectLux2.Id;
        //    }
        //}

        //#endregion


        //private int _luxid;

        ///// <summary>
        ///// 该时间表使用的光控探头ID
        ///// </summary>
        //public int LuxId
        //{
        //    get
        //    {
        //        return _luxid;
        //    }
        //    set
        //    {
        //        if (_luxid == value) return;
        //        _luxid = value;
        //        foreach (var t in LuxCollection.Where(t => t.Id == value))
        //        {
        //            CurrentSelectLux = t;
        //            LuxName = t.Name;
        //            LuxChanged = false;
        //            break;
        //        }
        //        RaisePropertyChanged(() => LuxId);

        //    }
        //}

        //private int _luxid2;

        ///// <summary>
        ///// 该时间表使用的光控探头ID
        ///// </summary>
        //public int LuxId2
        //{
        //    get
        //    {
        //        return _luxid2;
        //    }
        //    set
        //    {
        //        if (_luxid2 == value) return;
        //        _luxid2 = value;
        //        foreach (var t in LuxCollection2.Where(t => t.Id == value))
        //        {
        //            CurrentSelectLux2 = t;
        //            LuxName2 = t.Name;
        //            break;
        //        }
        //        RaisePropertyChanged(() => LuxId2);
        //    }
        //}

        //private string _luxname;
        ///// <summary>
        ///// 光控名称
        ///// </summary>
        //public string LuxName
        //{
        //    get { return _luxname; }
        //    set
        //    {
        //        if (_luxname != value)
        //        {
        //            _luxname = value;
        //            this.RaisePropertyChanged(() => this.LuxName);
        //        }
        //    }
        //}

        //private string _luxname2;

        ///// <summary>
        ///// 光控名称
        ///// </summary>
        //public string LuxName2
        //{
        //    get { return _luxname2; }
        //    set
        //    {
        //        if (_luxname2 != value)
        //        {
        //            _luxname2 = value;
        //            this.RaisePropertyChanged(() => this.LuxName2);
        //        }
        //    }
        //}

        private ObservableCollection<OneItemTerminal> _selectedItems;
        /// <summary>
        /// 所选终端
        /// </summary>
        public ObservableCollection<OneItemTerminal> SelectedItems
        {
            get
            {
                if (_selectedItems == null)
                    _selectedItems = new ObservableCollection<OneItemTerminal>();
                return _selectedItems;
            }
            set
            {
                if (_selectedItems != value)
                {
                    _selectedItems = value;
                    this.RaisePropertyChanged(() => this.SelectedItems);
                }
            }
        }

        //public class IdNameDesc : Wlst.Cr.Core.CoreServices.ObservableObject
        //{
        //    private int _id;

        //    /// <summary>
        //    /// 光控Id
        //    /// </summary>
        //    public int Id
        //    {
        //        get { return _id; }
        //        set
        //        {
        //            if (_id != value)
        //            {
        //                _id = value;
        //                RaisePropertyChanged(() => Id);
        //            }
        //        }
        //    }

        //    private string _name;

        //    public string Name
        //    {
        //        get { return _name; }
        //        set
        //        {
        //            if (_name != value)
        //            {
        //                _name = value;
        //                RaisePropertyChanged(() => Name);
        //            }
        //        }
        //    }


        //    private string _luxNsdfame;

        //    public string NameDesc
        //    {
        //        get { return _luxNsdfame; }
        //        set
        //        {
        //            if (_luxNsdfame != value)
        //            {
        //                _luxNsdfame = value;
        //                RaisePropertyChanged(() => NameDesc);
        //            }
        //        }
        //    }

           
        //}
    }
}
