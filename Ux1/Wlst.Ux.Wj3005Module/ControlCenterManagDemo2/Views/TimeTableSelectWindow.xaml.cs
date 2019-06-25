using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowForWlst;
using Wlst.Cr.Core.CoreServices;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;

namespace Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Views
{
    /// <summary>
    /// TimeTableSelectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TimeTableSelectWindow : CustomChromeWindow
    {
        public TimeTableSelectWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            AreaName.Clear();
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    string area = t.Value.AreaName;
                    AreaName.Add(new AreaInt() {Value = area, Key = t.Value.AreaId});
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() {Value = area, Key = t});
                    }
                }
            }

            var av = false;
            if (AreaName.Count>1) av=true;

            TimeTables.Clear();
            foreach (var t in AreaName)
            {
                foreach (var itemTable in WeekTimeTableInfoService.GeteekTimeTableInfoList(t.Key))
                {
                    TimeTables.Add(new TimeTable()
                                        {
                                            IsChecked = false,
                                            Area = t.Key + "-" + t.Value ,
                                            Id = itemTable.TimeId,
                                            Name = itemTable.TimeName,
                                            NameDesc = itemTable.TimeDesc,
                                            AreaId = t.Key,
                                            AreaView = av
                                        });
                }
            }

            time.ItemsSource = TimeTables;

        }

        public event EventHandler<EventArgsFrmSelectTimeTableList> OnFormBtnOkClick;
        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<AreaInt>();
                }
                return _devices;
            }
        }
        private static ObservableCollection<AreaInt> _devices;
        public class AreaInt : Wlst.Cr.Core.CoreServices.ObservableObject
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
        }





        public static ObservableCollection<TimeTable> TimeTables
        {
            get
            {
                if (_devices2 == null)
                {
                    _devices2 = new ObservableCollection<TimeTable>();
                }
                return _devices2;
            }

        }
        private static ObservableCollection<TimeTable> _devices2;
        public class TimeTable : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private bool _ischecked;

            public bool IsChecked
            {
                get { return _ischecked; }
                set
                {
                    if (_ischecked != value)
                    {
                        _ischecked = value;
                        this.RaisePropertyChanged(() => this.IsChecked);
                    }
                }
            }

            private int _id;

            public int Id
            {
                get { return _id; }
                set
                {
                    if (value != _id)
                    {
                        _id = value;
                        this.RaisePropertyChanged(() => this.Id);
                    }
                }
            }

            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    if (value != _name)
                    {
                        _name = value;
                        this.RaisePropertyChanged(() => this.Name);
                    }
                }
            }

            private string _area;

            public string Area
            {
                get { return _area; }
                set
                {
                    if (value != _area)
                    {
                        _area = value;
                        this.RaisePropertyChanged(() => this.Area);
                    }
                }
            }

            private int _areaid;

            public int AreaId
            {
                get { return _areaid; }
                set
                {
                    if (value != _areaid)
                    {
                        _areaid = value;
                        this.RaisePropertyChanged(() => this.AreaId);
                    }
                }
            }

            private string _namedesc;

            public string NameDesc
            {
                get { return _namedesc; }
                set
                {
                    if (value != _namedesc)
                    {
                        _namedesc = value;
                        this.RaisePropertyChanged(() => this.NameDesc);
                    }
                }
            }

            //private ObservableCollection<TimeTable> _childTreeItems;

            //public ObservableCollection<TimeTable> ChildTreeItems
            //{
            //    get { return _childTreeItems; }
            //    set
            //    {
            //        if (value != _childTreeItems)
            //        {
            //            _childTreeItems = value;
            //            this.RaisePropertyChanged(() => this.ChildTreeItems);
            //        }
            //    }
            //}
            private bool _areaview;

            public bool AreaView
            {
                get { return _areaview; }
                set
                {
                    if (value != _areaview)
                    {
                        _areaview = value;
                        this.RaisePropertyChanged(() => this.AreaView);
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var list = new List<Tuple<int, int, int, int>>();//areaid,grpid,rtuid,loopid

            var lsttime = new List<Tuple<int,int>>();
            foreach (var t in TimeTables)
            {
                if (t.IsChecked)
                {
                    lsttime.Add(new Tuple<int, int>(t.AreaId,t.Id));
                }
            }

            var dic = new Dictionary<Tuple<bool, int>, List<int>>();

            foreach (var area in AreaName)
            {
                foreach (var t in Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.Keys)
                {
                    if (t.Item1 == area.Key)
                    {
                         var tu = new Tuple<int, int>(t.Item1, t.Item2);
                         if (Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
                         {
                             for (int k = 1; k < 9; k++)
                             {
                                 int timetableid = Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(t.Item1, t.Item2, k);
                                 if (lsttime.Contains(new Tuple<int, int>(t.Item1,timetableid)))
                                 {
                                     var tu1 = new Tuple<int, int, int, int>(t.Item1,t.Item2,-1,k);
                                     list.Add(tu1);
                                 }
                             }
                         }
                    }
                }

                var tmp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(area.Key);
                foreach (var f in tmp)
                {
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                    {
                        var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[f];
                        if (para.EquipmentType == WjParaBase.EquType.Rtu)
                        {
                            for (int k = 1; k < 9; k++)
                            {
                                int timetableid = Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(area.Key, f, k);
                                if (lsttime.Contains(new Tuple<int, int>(area.Key, timetableid)))
                                {
                                    var tu1 = new Tuple<int, int, int, int>(area.Key, -1, f, k);
                                    list.Add(tu1);
                                }
                            }
                        }
                    }


                }
            }

            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsFrmSelectTimeTableList(list));
            }
            this.Close();
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (OnFormBtnOkClick != null)
            {
                OnFormBtnOkClick(this, new EventArgsFrmSelectTimeTableList(null));
            }
            this.Close();
        }

        public class EventArgsFrmSelectTimeTableList : EventArgs
        {
            public List<Tuple<int, int, int, int>> Info;
            public EventArgsFrmSelectTimeTableList(List<Tuple<int, int, int, int>> tmp)
            {
                Info = tmp;
            }
        }

       
    }
}
