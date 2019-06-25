using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipemntLightFault.InfoHold;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNew.Views;
using Wlst.client;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNew.Services;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNew.ViewModel
{
    [Export(typeof (IIRtuAmpSxx))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RtuAmpSxxViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIRtuAmpSxx
    {
        public RtuAmpSxxViewModel()
        {
            this.InitAction();
        }

        private bool isViewActive = false;

        public void NavInitBeforShow(params object[] parsObjects)
        {
            Visi = Visibility.Hidden;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            IsSelectAllTml = false;
            isViewActive = true;
            AreaName.Clear();
            getAreaRId();
            if (AreaName.Count > 0) AreaComboBoxSelected = AreaName.First();
            if (AreaName.Count > 1) Visi = Visibility.Visible;
            else Visi = Visibility.Collapsed;
            SectionId = 1;


        }

        public void OnUserHideOrClosing()
        {
            isViewActive = false;
            this.Items.Clear();
        }

        #region tab

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "上下限设置向导"; }
        }

        #endregion

        #region Grid1

        #region CmdTmlList


        private bool _isSelectAllTml;
        public bool IsSelectAllTml
        {
            get { return _isSelectAllTml; }
            set
            {
                if (value != _isSelectAllTml)
                {
                    _isSelectAllTml = value;
                    this.RaisePropertyChanged(() => this.IsSelectAllTml);
                    if (!value)
                    {
                        SelectTmlSum = TuLoop.Item1;
                        SelectTmlUsed = TuLoop.Item2;
                        SelectTmlNoUsed = TuLoop.Item3;
                        OutSelectTmlList = TuLoop.Item4;
                        SelectTmlType = "全部终端";
                    }
                    else
                    {
                        SelectTmlType = "自定义终端";
                        OutSelectTmlList = new List<int>();
                        SelectTmlSum = 0;
                        SelectTmlUsed = 0;
                        SelectTmlNoUsed = 0;
                    }
                }
            }
        }

        private SelectTmlList _selectTmlList = null;

        private ICommand _cmdTmlList;
        public ICommand CmdTmlList
        {
            get
            {
                return _cmdTmlList ??
                       (_cmdTmlList = new RelayCommand(ExCmdTmlList, CanCmdTmlList, true));
            }

        }

        private bool CanCmdTmlList()
        {
            if (IsSelectAllTml == false)
            {
                return false;
            }
            return true;
        }


        private void ExCmdTmlList()
        {
            _selectTmlList = new SelectTmlList();
            _selectTmlList.OnFormBtnOkClick +=
                new EventHandler<SelectTmlList.EventArgsSelectTmlList>(_selectTmlList_OnFormBtnOkClick);
            _selectTmlList.SetContext(Items);
            _selectTmlList.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _selectTmlList.ShowDialog();
        }

        private void _selectTmlList_OnFormBtnOkClick(object sender, SelectTmlList.EventArgsSelectTmlList args)
        {
            var tmp = args.SelectTmlListInfo;
            if (tmp == null) return;

            SelectTmlSum = 0;
            SelectTmlUsed = 0;
            SelectTmlNoUsed = 0;
            OutSelectTmlList = new List<int>();

            foreach (var t in tmp)
            {
                foreach (var tt in t.Child)
                {
                    if (tt.IsFather == false && tt.IsSelect == true)
                    {
                        SelectTmlSum++;
                        SelectTmlUsed = tt.LoopUsed + SelectTmlUsed;
                        SelectTmlNoUsed = tt.LoopNoUsed + SelectTmlNoUsed;
                        OutSelectTmlList.Add(tt.RtuId);
                    }
                }
            }
        }

        private int _setcionid;
        public int SectionId
        {
            get { return _setcionid; }
            set
            {
                if (value != _setcionid)
                {
                    _setcionid = value;
                    this.RaisePropertyChanged(() => this.SectionId);
                }
            }
        }


        private List<int> _selecttmllist;
        public List<int> OutSelectTmlList
        {
            get { return _selecttmllist; }
            set
            {
                if (value != _selecttmllist)
                {
                    _selecttmllist = value;
                    this.RaisePropertyChanged(() => this.OutSelectTmlList);
                }
            }
        }

        private int _selectTmlSum;
        public int SelectTmlSum
        {
            get { return _selectTmlSum; }
            set
            {
                if (value != _selectTmlSum)
                {
                    _selectTmlSum = value;
                    this.RaisePropertyChanged(() => this.SelectTmlSum);
                }
            }
        }

        private int _selectTmlUsed;
        public int SelectTmlUsed
        {
            get { return _selectTmlUsed; }
            set
            {
                if (value != _selectTmlUsed)
                {
                    _selectTmlUsed = value;
                    this.RaisePropertyChanged(() => this.SelectTmlUsed);
                }
            }
        }

        private int _selectTmlNoUsed;
        public int SelectTmlNoUsed
        {
            get { return _selectTmlNoUsed; }
            set
            {
                if (value != _selectTmlNoUsed)
                {
                    _selectTmlNoUsed = value;
                    this.RaisePropertyChanged(() => this.SelectTmlNoUsed);
                }
            }
        }

        #endregion

        #region 区域
        public void getAreaRId()
        {
            AreaName.Clear();
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    string area = t.Value.AreaName;
                    AreaName.Add(new AreaInt() { Value = t.Value.AreaId.ToString("d2") + "-" + area, Key = t.Value.AreaId });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                    }
                }
            }


        }

        private string _selectarea;
        public string SelectArea
        {
            get { return _selectarea; }
            set
            {
                if (value != _selectarea)
                {
                    _selectarea = value;
                    this.RaisePropertyChanged(() => this.SelectArea);
                }
            }
        }

        private string _selectTmlType;
        public string SelectTmlType
        {
            get { return _selectTmlType; }
            set
            {
                if (value != _selectTmlType)
                {
                    _selectTmlType = value;
                    this.RaisePropertyChanged(() => this.SelectTmlType);
                }
            }
        }


        private static ObservableCollection<AreaInt> _devices;

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

        private AreaInt _areacomboboxselected;
        private int AreaId;

        private Tuple<int, int, int,List<int>> TuLoop = new Tuple<int, int, int,List<int>>(0, 0, 0,new List<int>());

        public AreaInt AreaComboBoxSelected
        {
            get { return _areacomboboxselected; }
            set
            {
                if (_areacomboboxselected != value)
                {
                    _areacomboboxselected = value;
                    this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
                    if (value == null) return;

                    AreaId = value.Key;
                    SelectArea = value.Value;

                    Items.Clear();
                    var loopusedd = 0;
                    var loopnousedd = 0;
                    var tmlsum = 0;
                    var tmllist = new List<int>();

                    foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
                    {
                        if (t.Key.Item1 == AreaId)
                        {
                            var child = new ObservableCollection<TmlItems>();
                            foreach (var tt in t.Value.LstTml)
                            {
                                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(tt))
                                {
                                    var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tt];
                                    if (tml.EquipmentType == WjParaBase.EquType.Rtu)
                                    {
                                        tmlsum++;
                                        var loops = tml as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                                        if (loops == null) continue;

                                        int loopused = 0, loopnoused = 0;
                                        foreach (var g in loops.WjLoops)
                                        {
                                            if (g.Value.IsShieldLoop != 1)
                                            {
                                                loopused++;
                                                loopusedd++;
                                            }
                                            else
                                            {
                                                loopnoused++;
                                                loopnousedd++;
                                            }
                                        }

                                        tmllist.Add(tml.RtuId);
                                        child.Add(new TmlItems()
                                        {
                                            Id = tml.RtuPhyId,
                                            RtuId = tml.RtuId,
                                            Name = tml.RtuName,
                                            IsFather = false,
                                            IsSelect = false,
                                            LoopNoUsed = loopnoused,
                                            LoopUsed = loopused
                                        });
                                    }
                                }
                            }
                            if (t.Value.LstTml.Count > 0)
                            {
                                Items.Add(new TmlItems()
                                {
                                    Id = t.Value.GroupId,
                                    Name = t.Value.GroupName,
                                    IsFather = true,
                                    IsSelect = true,
                                    Child = child
                                });
                            }
                        }
                    }
                    var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);

                    if (rtulst.Count > 0)
                    {
                        var child1 = new ObservableCollection<TmlItems>();
                        foreach (var tt in rtulst)
                        {
                            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(tt))
                            {
                                var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tt];
                                if (tml.EquipmentType == WjParaBase.EquType.Rtu)
                                {
                                    tmlsum++;
                                    var loops = tml as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                                    if (loops == null) continue;

                                    int loopused = 0, loopnoused = 0;
                                    foreach (var g in loops.WjLoops)
                                    {
                                        if (g.Value.IsShieldLoop != 1)
                                        {
                                            loopused++;
                                            loopusedd++;
                                        }
                                        else
                                        {
                                            loopnoused++;
                                            loopnousedd++;
                                        }
                                    }

                                    tmllist.Add(tml.RtuId);
                                    child1.Add(new TmlItems()
                                    {
                                        Id = tml.RtuPhyId,
                                        RtuId = tml.RtuId,
                                        Name = tml.RtuName,
                                        IsFather = false,
                                        IsSelect = true,
                                        LoopNoUsed = loopnoused,
                                        LoopUsed = loopused
                                    });
                                }
                            }
                        }
                        Items.Add(new TmlItems()
                        {
                            Id = -1,
                            Name = "未分组终端",
                            IsFather = true,
                            IsSelect = true,
                            Child = child1
                        });


                    }

                    SelectTmlNoUsed = loopnousedd;
                    SelectTmlUsed = loopusedd;
                    SelectTmlSum = tmlsum;

                    TuLoop = new Tuple<int, int, int,List<int>>(tmlsum, loopusedd, loopnousedd,tmllist);
                }
            }
        }

        private ObservableCollection<TmlItems> _devicesitem;
        public ObservableCollection<TmlItems> Items
        {
            get
            {
                if (_devicesitem == null)
                {
                    _devicesitem = new ObservableCollection<TmlItems>();
                }
                return _devicesitem;
            }
            set
            {
                if (value != _devicesitem)
                {
                    _devicesitem = value;
                    this.RaisePropertyChanged(() => this.Items);
                }
            }
        }

        public class TmlItems : Wlst.Cr.Core.CoreServices.ObservableObject
        {
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

            private int _rtuid;
            public int RtuId
            {
                get { return _rtuid; }
                set
                {
                    if (_rtuid != value)
                    {
                        _rtuid = value;
                        this.RaisePropertyChanged(() => this.RtuId);
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

            private bool _isSelect;
            public bool IsSelect
            {
                get { return _isSelect; }
                set
                {
                    if (value != _isSelect)
                    {
                        _isSelect = value;
                        this.RaisePropertyChanged(() => this.IsSelect);
                        if (IsFather && Child != null)
                        {
                            foreach (var t in Child)
                            {
                                t.IsSelect = value;
                            }
                        }
                    }
                }
            }

            private bool _isfather;
            public bool IsFather
            {
                get { return _isfather; }
                set
                {
                    _isfather = value;
                    this.RaisePropertyChanged(() => this.IsFather);
                }
            }

            private ObservableCollection<TmlItems> _child;
            public ObservableCollection<TmlItems> Child
            {
                get { return _child; }
                set
                {
                    _child = value;
                    this.RaisePropertyChanged(() => this.Child);
                }
            }

            private int _loopused;
            public int LoopUsed
            {
                get { return _loopused; }
                set
                {
                    if (_loopused != value)
                    {
                        _loopused = value;
                        this.RaisePropertyChanged(() => this.LoopUsed);
                    }
                }
            }

            private int _loopnoused;
            public int LoopNoUsed
            {
                get { return _loopnoused; }
                set
                {
                    if (_loopnoused != value)
                    {
                        _loopnoused = value;
                        this.RaisePropertyChanged(() => this.LoopNoUsed);
                    }
                }
            }
        }






        private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }
        #endregion

        #endregion

        #region Cmd1To2

        private ICommand _cmdCmd1To2;
        public ICommand Cmd1To2
        {
            get { return _cmdCmd1To2 ?? (_cmdCmd1To2 = new RelayCommand(ExCmd1To2, CanCmd1To2, true)); }
        }

        private void ExCmd1To2()
        {
            IsSelectBaseOld = false;
            VisiGrid2 = Visibility.Hidden;
            DtSelectTime = DateTime.Now.AddDays(-1);
            SelectStartTime = 1230;
            TimeComboBoxSelected = new NameValueInt() { Name = "3小时", Value = 6 * 30 };
            if (!IsSelectAllTml)
            {
                SelectTmlType = "全部终端";
            }
            else
            {
                SelectTmlType = "自定义终端";
            }
        }

        private bool CanCmd1To2()
        {
            return true;
        }


        #endregion

        #region Grid2

        private bool _isSelectBaseOld;
        public bool IsSelectBaseOld
        {
            get { return _isSelectBaseOld; }
            set
            {
                if (value != _isSelectBaseOld)
                {
                    _isSelectBaseOld = value;
                    this.RaisePropertyChanged(() => this.IsSelectBaseOld);
                    if (value)
                    {
                        VisiGrid2 = Visibility.Visible;
                        DtSelectTime = DateTime.Now.AddDays(-1);
                        SelectStartTime = 1230;
                        foreach (var t in TimeItems)
                        {
                            if (t.Value == 180)
                            {
                                TimeComboBoxSelected = t;
                            }
                        }
                    }
                    else
                    {
                        VisiGrid2 = Visibility.Hidden;
                    }
                }
            }
        }

        private DateTime _dtselectTime;
        public DateTime DtSelectTime
        {
            get { return _dtselectTime; }
            set
            {
                if (value != _dtselectTime)
                {
                    _dtselectTime = value;
                    this.RaisePropertyChanged(() => this.DtSelectTime);
                }
            }
        }

        private int _selectStartTime;
        public int SelectStartTime
        {
            get { return _selectStartTime; }
            set
            {
                if (value != _selectStartTime)
                {
                    _selectStartTime = value;
                    this.RaisePropertyChanged(() => this.SelectStartTime);
                }
            }
        }

        private Visibility _visigrid2;
        public Visibility VisiGrid2
        {
            get { return _visigrid2; }
            set
            {
                if (value != _visigrid2)
                {
                    _visigrid2 = value;
                    this.RaisePropertyChanged(() => this.VisiGrid2);
                }
            }
        }

        private int OutSelectTimeLong = 0;
        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _timeItems;
        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> TimeItems
        {
            get
            {
                if (_timeItems == null)
                {
                    _timeItems = new ObservableCollection<NameValueInt>();
                    _timeItems.Add(new NameValueInt() { Name = "1小时", Value = 2 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "1.5小时", Value = 3 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "2小时", Value = 4 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "2.5小时", Value = 5 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "3小时", Value = 6 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "3.5小时", Value = 7 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "4小时", Value = 8 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "4.5小时", Value = 9 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "5小时", Value = 10 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "5.5小时", Value = 11 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "6小时", Value = 12 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "7小时", Value = 14 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "8小时", Value = 16 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "9小时", Value = 18 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "10小时", Value = 20 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "11小时", Value = 22 * 30 });
                    _timeItems.Add(new NameValueInt() { Name = "12小时", Value = 24 * 30 });
                }
                return _timeItems;
            }
        }

        private NameValueInt _currenttime;
        public NameValueInt TimeComboBoxSelected
        {
            get { return _currenttime; }
            set
            {
                if (_currenttime == value) return;
                _currenttime = value;
                this.RaisePropertyChanged(() => this.TimeComboBoxSelected);
                if (value != null)
                {
                    OutSelectTimeLong = value.Value;
                }
            }
        }

        #endregion

        #region Cmd2To3

        private ICommand _cmdCmd2To3;
        public ICommand Cmd2To3
        {
            get { return _cmdCmd2To3 ?? (_cmdCmd1To2 = new RelayCommand(ExCmd2To3, CanCmd2To3, true)); }
        }

        private void ExCmd2To3()
        {
            IsGrid3per5 = true;
            IsGrid3per5to10 = false;
            IsGrid3per10to20 = false;
            IsGrid3per20 = false;

            WatchItem.Clear();
            NoDataTmlList.Clear();
            EditItem5to10.Clear();
            EditItem10to20.Clear();
            EditItem20.Clear();

            if (IsSelectBaseOld)
            {
                long sst = Convert.ToInt64(SelectStartTime)*60*10000000;
                long edt = Convert.ToInt64(TimeComboBoxSelected.Value) * 60 * 10000000;
                var st = new DateTime(DtSelectTime.Date.Ticks + sst);
                var ed = new DateTime(DtSelectTime.Date.Ticks + sst + edt);
                Grid2DataTime = st.ToString("yyyy-MM-dd HH:mm") + " - " + ed.ToString("yyyy-MM-dd HH:mm");

                var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new;
                info.WstRtuLdlSxxAvgSetNew.Op = 3;
                info.WstRtuLdlSxxAvgSetNew.RtuIds = OutSelectTmlList;
                info.WstRtuLdlSxxAvgSetNew.LoopdataItems.DtStart = st.Ticks;
                info.WstRtuLdlSxxAvgSetNew.LoopdataItems.DtEnd = ed.Ticks;
                SndOrderServer.OrderSnd(info, 10, 5);
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在向服务器端请求回路数据，请稍后...";
            }
            else
            {
                Grid2DataTime = "当前数据";

                foreach (var t in OutSelectTmlList)
                {
                    if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                    {
                        var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                        if (tml.EquipmentType == WjParaBase.EquType.Rtu)
                        {
                            var info = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t);
                            if (info != null && info.RtuNewData != null && info.RtuNewData.LstNewLoopsData!=null)
                            {
                                foreach (var tt in info.RtuNewData.LstNewLoopsData)
                                {
                                    WatchItem.Add(new WatchItems()
                                                      {
                                                          Id = tml.RtuPhyId,
                                                          RtuId = t,
                                                          Name = tml.RtuName,
                                                          LoopId = tt.LoopId,
                                                          LoopName = tt.LoopName,
                                                          AverageA = tt.A
                                                      });
                                }
                            }
                            else
                            {
                                NoDataTmlList.Add(new WatchItems()
                                {
                                    RtuId = t,
                                    Id=tml.RtuPhyId,
                                    Name=tml.RtuName,
                                });
                            }
                        }
                    }
                }
                Grid2DataSum = WatchItem.Count;
                Grid2NoDataTmlSum = NoDataTmlList.Count;
            }
        }

        private bool CanCmd2To3()
        {
            return true;
        }

        private ICommand _cmdCmdGrid2NoDataTml;
        public ICommand CmdGrid2NoDataTml
        {
            get { return _cmdCmdGrid2NoDataTml ?? (_cmdCmd1To2 = new RelayCommand(ExCmdGrid2NoDataTml, CanCmdGrid2NoDataTml, true)); }
        }

        private void ExCmdGrid2NoDataTml()
        {
            _watchNoDataTmlList = new NoDataTmlList();
            _watchNoDataTmlList.OnFormBtnOkClick +=
                new EventHandler<NoDataTmlList.EventArgsSelectTmlList>(_watchNoDataTmlList_OnFormBtnOkClick);
            _watchNoDataTmlList.SetContext(NoDataTmlList);
            _watchNoDataTmlList.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _watchNoDataTmlList.ShowDialog();
        }

        private bool CanCmdGrid2NoDataTml()
        {
            if (NoDataTmlList!=null && NoDataTmlList.Count>0)return true;
            else return false;
        }

        private NoDataTmlList _watchNoDataTmlList = null;
        private void _watchNoDataTmlList_OnFormBtnOkClick(object sender, NoDataTmlList.EventArgsSelectTmlList args)
        {
            var tmp = args.WatchNoDataTmlListInfo;
            if (tmp == null) return;
        }

        private ObservableCollection<WatchItems> _noDataTmlList;
        public ObservableCollection<WatchItems> NoDataTmlList
        {
            get { return _noDataTmlList; }
            set
            {
                if (value != _noDataTmlList)
                {
                    _noDataTmlList = value;
                    this.RaisePropertyChanged(() => this.NoDataTmlList);
                }
            }
        }


        private string _grid2DataTime;
        public string Grid2DataTime
        {
            get { return _grid2DataTime; }
            set
            {
                if (value != _grid2DataTime)
                {
                    _grid2DataTime = value;
                    this.RaisePropertyChanged(() => this.Grid2DataTime);
                }
            }
        }

        private int _grid2DataSum;
        public int Grid2DataSum
        {
            get { return _grid2DataSum; }
            set
            {
                if (value != _grid2DataSum)
                {
                    _grid2DataSum = value;
                    this.RaisePropertyChanged(() => this.Grid2DataSum);
                }
            }
        }

        private int _grid2NoDataTmlSum;
        public int Grid2NoDataTmlSum
        {
            get { return _grid2NoDataTmlSum; }
            set
            {
                if (value != _grid2NoDataTmlSum)
                {
                    _grid2NoDataTmlSum = value;
                    this.RaisePropertyChanged(() => this.Grid2NoDataTmlSum);
                }
            }
        }
        #endregion

        #region Grid3

        #region 5-10
        private ICommand _cmdCmdGrid3per5to10;
        public ICommand CmdGrid3per5to10
        {
            get { return _cmdCmdGrid3per5to10 ?? (_cmdCmd1To2 = new RelayCommand(ExCmdGrid3per5to10, CanCmdGrid3per5to10, true)); }
        }

        private void ExCmdGrid3per5to10()
        {
            _editTmlList5to10 = new EditTmlList();
            _editTmlList5to10.OnFormBtnOkClick +=
                new EventHandler<EditTmlList.EventArgsSelectTmlList>(_editTmlList5to10_OnFormBtnOkClick);
            _editTmlList5to10.SetContext(EditItem5to10);
            _editTmlList5to10.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _editTmlList5to10.ShowDialog();

        }

        private bool CanCmdGrid3per5to10()
        {
            if (EditItem5to10.Count > 0) return true;
            else return true;
        }

        private EditTmlList _editTmlList5to10 = null;
        private void _editTmlList5to10_OnFormBtnOkClick(object sender, EditTmlList.EventArgsSelectTmlList args)
        {
            var tmp = args.EditTmlListInfo;
            if (tmp == null) return;
            EditItem5to10 = args.EditTmlListInfo;
        }

        #endregion

        #region 10-20
        private ICommand _cmdCmdGrid3per10o20;
        public ICommand CmdGrid3per10to20
        {
            get { return _cmdCmdGrid3per10o20 ?? (_cmdCmd1To2 = new RelayCommand(ExCmdGrid3per10to20, CanCmdGrid3per10to20, true)); }
        }

        private void ExCmdGrid3per10to20()
        {
            _editTmlList10to20 = new EditTmlList();
            _editTmlList10to20.OnFormBtnOkClick +=
                new EventHandler<EditTmlList.EventArgsSelectTmlList>(_editTmlList10to20_OnFormBtnOkClick);
            _editTmlList10to20.SetContext(EditItem10to20);
            _editTmlList10to20.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _editTmlList10to20.ShowDialog();

        }

        private bool CanCmdGrid3per10to20()
        {
            if (EditItem10to20.Count > 0) return true;
            else return true;
        }

        private EditTmlList _editTmlList10to20 = null;
        private void _editTmlList10to20_OnFormBtnOkClick(object sender, EditTmlList.EventArgsSelectTmlList args)
        {
            var tmp = args.EditTmlListInfo;
            if (tmp == null) return;
            EditItem10to20 = args.EditTmlListInfo;
        }


        #endregion

        #region 20

        private ICommand _cmdCmdGrid3per20;
        public ICommand CmdGrid3per20
        {
            get { return _cmdCmdGrid3per20 ?? (_cmdCmd1To2 = new RelayCommand(ExCmdGrid3per20, CanCmdGrid3per20, true)); }
        }

        private void ExCmdGrid3per20()
        {
            _editTmlList20 = new EditTmlList();
            _editTmlList20.OnFormBtnOkClick +=
                new EventHandler<EditTmlList.EventArgsSelectTmlList>(_editTmlList20_OnFormBtnOkClick);
            _editTmlList20.SetContext(EditItem20);
            _editTmlList20.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _editTmlList20.ShowDialog();

        }

        private bool CanCmdGrid3per20()
        {
            if (EditItem20.Count > 0) return true;
            else return true;
        }

        private EditTmlList _editTmlList20 = null;
        private void _editTmlList20_OnFormBtnOkClick(object sender, EditTmlList.EventArgsSelectTmlList args)
        {
            var tmp = args.EditTmlListInfo;
            if (tmp == null) return;
            EditItem20 = args.EditTmlListInfo;
        }

        #endregion

        #region 5

        private ICommand _cmdCmdGrid3per5;
        public ICommand CmdGrid3per5
        {
            get { return _cmdCmdGrid3per5 ?? (_cmdCmd1To2 = new RelayCommand(ExCmdGrid3per5, CanCmdGrid3per5, true)); }
        }

        private void ExCmdGrid3per5()
        {
            _watchTmlList = new WatchTmlList();
            _watchTmlList.OnFormBtnOkClick +=
                new EventHandler<WatchTmlList.EventArgsSelectTmlList>(_watchTmlList_OnFormBtnOkClick);
            _watchTmlList.SetContext(WatchItem);
            _watchTmlList.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _watchTmlList.ShowDialog();

        }

        private bool CanCmdGrid3per5()
        {
            if (WatchItem.Count > 0) return true;
            else return true;
        }

        private WatchTmlList _watchTmlList = null;
        private void _watchTmlList_OnFormBtnOkClick(object sender, WatchTmlList.EventArgsSelectTmlList args)
        {
            var tmp = args.WatchTmlListInfo;
            if (tmp == null) return;
        }

        #endregion

        #region 
        private ObservableCollection<WatchItems> _watchitem;
        public ObservableCollection<WatchItems> WatchItem
        {
            get
            {
                if (_watchitem == null)
                {
                    _watchitem = new ObservableCollection<WatchItems>();
                }
                return _watchitem;
            }
            set
            {
                if (value != _watchitem)
                {
                    _watchitem = value;
                    this.RaisePropertyChanged(() => this.WatchItem);
                }
            }
        }

        public class WatchItems : Wlst.Cr.Core.CoreServices.ObservableObject
        {
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

            private int _rtuid;
            public int RtuId
            {
                get { return _rtuid; }
                set
                {
                    if (_rtuid != value)
                    {
                        _rtuid = value;
                        this.RaisePropertyChanged(() => this.RtuId);
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

            private int _loopid;
            public int LoopId
            {
                get { return _loopid; }
                set
                {
                    if (_loopid != value)
                    {
                        _loopid = value;
                        this.RaisePropertyChanged(() => this.LoopId);
                    }
                }
            }

            private string _loopname;
            public string LoopName
            {
                get { return _loopname; }
                set
                {
                    if (_loopname != value)
                    {
                        _loopname = value;
                        this.RaisePropertyChanged(() => this.LoopName);
                    }
                }
            }

            private double _averageA;
            public double AverageA
            {
                get { return _averageA; }
                set
                {
                    if (_averageA != value)
                    {
                        _averageA = value;
                        this.RaisePropertyChanged(() => this.AverageA);
                    }
                }
            }
        }

        private Dictionary<int,EditItems>EditItemAll;
       
        private ObservableCollection<EditItems> _edititem5to10;
        public ObservableCollection<EditItems> EditItem5to10
        {
            get
            {
                if (_edititem5to10 == null)
                {
                    _edititem5to10 = new ObservableCollection<EditItems>();
                }
                return _edititem5to10;
            }
            set
            {
                if (value != _edititem5to10)
                {
                    _edititem5to10 = value;
                    this.RaisePropertyChanged(() => this.EditItem5to10);
                }
            }
        }

        private ObservableCollection<EditItems> _edititem10to20;
        public ObservableCollection<EditItems> EditItem10to20
        {
            get
            {
                if (_edititem10to20 == null)
                {
                    _edititem10to20 = new ObservableCollection<EditItems>();
                }
                return _edititem10to20;
            }
            set
            {
                if (value != _edititem10to20)
                {
                    _edititem10to20 = value;
                    this.RaisePropertyChanged(() => this.EditItem10to20);
                }
            }
        }

        private ObservableCollection<EditItems> _edititem20;
        public ObservableCollection<EditItems> EditItem20
        {
            get
            {
                if (_edititem20 == null)
                {
                    _edititem20 = new ObservableCollection<EditItems>();
                }
                return _edititem20;
            }
            set
            {
                if (value != _edititem20)
                {
                    _edititem20 = value;
                    this.RaisePropertyChanged(() => this.EditItem20);
                }
            }
        }

        public class EditItems : Wlst.Cr.Core.CoreServices.ObservableObject
        {
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

            private int _rtuid;
            public int RtuId
            {
                get { return _rtuid; }
                set
                {
                    if (_rtuid != value)
                    {
                        _rtuid = value;
                        this.RaisePropertyChanged(() => this.RtuId);
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

            private int _loopid;
            public int LoopId
            {
                get { return _loopid; }
                set
                {
                    if (_loopid != value)
                    {
                        _loopid = value;
                        this.RaisePropertyChanged(() => this.LoopId);
                    }
                }
            }

            private string _loopname;
            public string LoopName
            {
                get { return _loopname; }
                set
                {
                    if (_loopname != value)
                    {
                        _loopname = value;
                        this.RaisePropertyChanged(() => this.LoopName);
                    }
                }
            }

            private double _averageA;
            public double AverageA
            {
                get { return _averageA; }
                set
                {
                    if (_averageA != value)
                    {
                        _averageA = value;
                        this.RaisePropertyChanged(() => this.AverageA);
                    }
                }
            }

            private List<string> _a;
            public List<string> A
            {
                get { return _a; }
                set
                {
                    if (_a != value)
                    {
                        _a = value;
                        this.RaisePropertyChanged(() => this.A);
                    }
                }
            }

            private List<Tuple<bool,double>> _abase;
            public List<Tuple<bool, double>> ABase
            {
                get { return _abase; }
                set
                {
                    if (_abase != value)
                    {
                        _abase = value;
                        this.RaisePropertyChanged(() => this.ABase);

                        foreach (var t in value)
                        {
                            if (t.Item1 == false)
                            {
                                AverageA = 0.00;
                                var sum = 0.00;
                                var index = 0.00;

                                foreach (var tt in value)
                                {
                                    if (tt.Item1 == true)
                                    {
                                        sum = sum + tt.Item2;
                                        index++;
                                    }
                                }
                                AverageA = sum/index;
                            }
                        }

                    }
                }
            }

            private List<double> _aper;
            public List<double> APer
            {
                get { return _aper; }
                set
                {
                    if (_aper != value)
                    {
                        _aper = value;
                        this.RaisePropertyChanged(() => this.APer);
                    }
                }
            }
        }


        private bool _isGrid3per5;
        public bool IsGrid3per5
        {
            get { return _isGrid3per5; }
            set
            {
                if (value != _isGrid3per5)
                {
                    _isGrid3per5 = value;
                    this.RaisePropertyChanged(() => this.IsGrid3per5);
                }
            }
        }

        private int _grid3per5;
        public int Grid3per5
        {
            get { return _grid3per5; }
            set
            {
                if (value != _grid3per5)
                {
                    _grid3per5 = value;
                    this.RaisePropertyChanged(() => this.Grid3per5);
                }
            }
        }

        private bool _isGrid3per5to10;
        public bool IsGrid3per5to10
        {
            get { return _isGrid3per5to10; }
            set
            {
                if (value != _isGrid3per5to10)
                {
                    _isGrid3per5to10 = value;
                    this.RaisePropertyChanged(() => this.IsGrid3per5to10);
                }
            }
        }

        private int _grid3per5to10;
        public int Grid3per5to10
        {
            get { return _grid3per5to10; }
            set
            {
                if (value != _grid3per5to10)
                {
                    _grid3per5to10 = value;
                    this.RaisePropertyChanged(() => this.Grid3per5to10);
                }
            }
        }

        private bool _isGrid3per10to20;
        public bool IsGrid3per10to20
        {
            get { return _isGrid3per10to20; }
            set
            {
                if (value != _isGrid3per10to20)
                {
                    _isGrid3per10to20 = value;
                    this.RaisePropertyChanged(() => this.IsGrid3per10to20);
                }
            }
        }
        
        private int _grid3per10to20;
        public int Grid3per10to20
        {
            get { return _grid3per10to20; }
            set
            {
                if (value != _grid3per10to20)
                {
                    _grid3per10to20 = value;
                    this.RaisePropertyChanged(() => this.Grid3per10to20);
                }
            }
        }

        private bool _isGrid3per20;
        public bool IsGrid3per20
        {
            get { return _isGrid3per20; }
            set
            {
                if (value != _isGrid3per20)
                {
                    _isGrid3per20 = value;
                    this.RaisePropertyChanged(() => this.IsGrid3per20);
                }
            }
        }

        private int _grid3per20;
        public int Grid3per20
        {
            get { return _grid3per20; }
            set
            {
                if (value != _grid3per20)
                {
                    _grid3per20 = value;
                    this.RaisePropertyChanged(() => this.Grid3per20);
                }
            }
        }

        #endregion

        #endregion

        #region Cmd3To4

        private ICommand _cmdCmd3To4;
        public ICommand Cmd3To4
        {
            get { return _cmdCmd3To4 ?? (_cmdCmd3To4 = new RelayCommand(ExCmd3To4, CanCmd3To4, true)); }
        }

        private void ExCmd3To4()
        {
            SetAll.Clear();
            NoSetAll.Clear();
            Grid3noSet = OutSelectTmlList.Count;
            if (IsGrid3per5)
            {
                Grid3noSet = Grid3noSet - Grid3per5;
                foreach (var t in WatchItem)
                {
                    SetAll.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                    {
                        Avg=t.AverageA,
                        LoopId = t.LoopId,
                        RtuId = t.RtuId
                    });
                }
            }
            else
            {
                foreach (var t in WatchItem)
                {
                    NoSetAll.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                    {
                        Avg = t.AverageA,
                        LoopId = t.LoopId,
                        RtuId = t.RtuId
                    });
                }
            }
            if (IsGrid3per5to10)
            {
                Grid3noSet = Grid3noSet - Grid3per5to10;
                foreach (var t in EditItem5to10)
                {
                    SetAll.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                    {
                        Avg = t.AverageA,
                        LoopId = t.LoopId,
                        RtuId = t.RtuId
                    });
                }
            }
            else
            {
                foreach (var t in EditItem5to10)
                {
                    NoSetAll.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                    {
                        Avg = t.AverageA,
                        LoopId = t.LoopId,
                        RtuId = t.RtuId
                    });
                }
            }
            if (IsGrid3per10to20)
            {
                Grid3noSet = Grid3noSet - Grid3per10to20;
                foreach (var t in EditItem10to20)
                {
                    SetAll.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                    {
                        Avg = t.AverageA,
                        LoopId = t.LoopId,
                        RtuId = t.RtuId
                    });
                }
            }
            else
            {
                foreach (var t in EditItem10to20)
                {
                    NoSetAll.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                    {
                        Avg = t.AverageA,
                        LoopId = t.LoopId,
                        RtuId = t.RtuId
                    });
                }
            }
            if (IsGrid3per20)
            {
                Grid3noSet = Grid3noSet - Grid3per20; 
                foreach (var t in EditItem20)
                {
                    SetAll.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                    {
                        Avg = t.AverageA,
                        LoopId = t.LoopId,
                        RtuId = t.RtuId
                    });
                }
            }
            else
            {
                foreach (var t in EditItem20)
                {
                    NoSetAll.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                    {
                        Avg = t.AverageA,
                        LoopId = t.LoopId,
                        RtuId = t.RtuId
                    });
                }
            }



            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new;
            info.WstRtuLdlSxxAvgSetNew.Op = 4;
            info.WstRtuLdlSxxAvgSetNew.RtuIds = OutSelectTmlList;
            SndOrderServer.OrderSnd(info, 10, 5);
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在向服务器端请求上下限规则，请稍后...";
        }

        private bool CanCmd3To4()
        {
            if (!IsGrid3per5 && !IsGrid3per10to20 && !IsGrid3per5to10 && !IsGrid3per20) return false;
            else return true;
        }


        private int _grid3noSet;
        public int Grid3noSet
        {
            get { return _grid3noSet; }
            set
            {
                if (_grid3noSet != value)
                {
                    _grid3noSet = value;
                    this.RaisePropertyChanged(() => this.Grid3noSet);
                }
            }
        }


        private ObservableCollection<RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx> _noSetAll;
        public ObservableCollection<RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx> NoSetAll
        {
            get { return _noSetAll; }
            set
            {
                if (_noSetAll != value)
                {
                    _noSetAll = value;
                    this.RaisePropertyChanged(() => this.NoSetAll);
                }
            }
        }

        private ObservableCollection<RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx> _setAll;
        public ObservableCollection<RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx> SetAll
        {
            get { return _setAll; }
            set
            {
                if (_setAll != value)
                {
                    _setAll = value;
                    this.RaisePropertyChanged(() => this.SetAll);
                }
            }
        }

        #endregion

        #region Grid4

        #region CmdAddRule

        private ICommand _cCmdCmdAddRule;

        public ICommand CmdAddRule
        {
            get { return _cCmdCmdAddRule ?? (_cCmdCmdAddRule = new RelayCommand(ExCmdAddRule, CanCmdAddRule, true)); }
        }


        private void ExCmdAddRule()
        {
            dt = DateTime.Now;
            if (this.ItemsRules.Count > 1)
            {
                this.ItemsRules.Add(new RultItem() { Index = ItemsRules.Count + 1, Alow = 1000, Amax = 2000, LowTimes = 0.8, MaxTimes = 1.2 });
            }
            else
            {
                var tu = new Tuple<double, double>(ItemsRules[0].LowTimes, ItemsRules[0].MaxTimes);
                this.ItemsRules.Clear();
                this.ItemsRules.Add(new RultItem() { Index = 1, Alow = 0, Amax = 500, LowTimes = tu.Item1, MaxTimes = tu.Item2 });
                this.ItemsRules.Add(new RultItem() { Index = 2, Alow = 500, Amax = 1000, LowTimes = tu.Item1, MaxTimes = tu.Item2 });
            }
            CurrentSelectRule = this.ItemsRules.Last();
        }

        private DateTime dt = DateTime.Now.AddDays(-1);

        private bool CanCmdAddRule()
        {
            return DateTime.Now.Ticks - dt.Ticks > 30000000;
        }

        #endregion

        #region CmdDeleteRule

        private ICommand _cCmdDeleteRules;

        public ICommand CmdDeleteRule
        {
            get
            {
                return _cCmdDeleteRules ??
                       (_cCmdDeleteRules = new RelayCommand(ExCmdDeleteRules, CanCmdDeleteRules, true));
            }
        }


        private void ExCmdDeleteRules()
        {

            dtde = DateTime.Now;
            if (ItemsRules.Count < 2) return;

            if (CurrentSelectRule.Alow == 0 && CurrentSelectRule.Amax == 1000)
            {
                if (this.ItemsRules.Contains(CurrentSelectRule))
                {
                    this.ItemsRules.Remove(CurrentSelectRule);
                }
            }
            else
            {
                if (this.ItemsRules.Contains(CurrentSelectRule))
                {
                    this.ItemsRules.Remove(CurrentSelectRule);
                }


                var lst = new List<Tuple<int, int, double, double>>();
                foreach (var t in ItemsRules)
                {
                    var tu = new Tuple<int, int, double, double>(t.Alow, t.Amax, t.LowTimes, t.MaxTimes);
                    lst.Add(tu);
                }

                var lstafter =
                        (from t in lst orderby t.Item2 select t).ToList();
                var lstlast = new List<Tuple<int, int, double, double>>();

                for (int i = 1; i < lstafter.Count; i++)
                {
                    var tu = new Tuple<int, int, double, double>(0, 0, 0, 0);

                    var first = lstafter[i - 1].Item1;
                    if (i == 1) first = 0;

                    if (lstafter[i].Item1 != lstafter[i - 1].Item2)
                    {
                        tu = new Tuple<int, int, double, double>(first, lstafter[i].Item1,
                                                                    lstafter[i - 1].Item3, lstafter[i - 1].Item4);
                    }
                    else
                    {
                        tu = new Tuple<int, int, double, double>(first, lstafter[i - 1].Item2,
                                                                    lstafter[i - 1].Item3, lstafter[i - 1].Item4);
                    }

                    lstlast.Add(tu);


                    tu = new Tuple<int, int, double, double>(0, lstafter[i].Item2,
                                                                    lstafter[i].Item3, lstafter[i].Item4);





                    if (i == lstafter.Count - 1)
                    {
                        tu = new Tuple<int, int, double, double>(lstafter[i].Item1, 1000,
                                                                    lstafter[i].Item3, lstafter[i].Item4);
                        lstlast.Add(tu);
                    }
                }

                ItemsRules.Clear();
                var index = 0;
                if (lstlast.Count > 0)
                {
                    foreach (var t in lstlast)
                    {
                        index++;
                        ItemsRules.Add(new RultItem()
                        {
                            Index = index,
                            Alow = t.Item1,
                            Amax = t.Item2,
                            LowTimes = t.Item3,
                            MaxTimes = t.Item4
                        });
                    }
                }
                else
                {
                    ItemsRules.Add(new RultItem()
                    {
                        Index = 1,
                        Alow = 0,
                        Amax = 1000,
                        LowTimes = lstafter[0].Item3,
                        MaxTimes = lstafter[0].Item4
                    });
                }

            }



        }

        private DateTime dtde = DateTime.Now.AddDays(-1);

        private bool CanCmdDeleteRules()
        {
            return DateTime.Now.Ticks - dtde.Ticks > 30000000 && ItemsRules.Count > 1;
        }

        #endregion

        #region CmdCurrentRule

        private ICommand _cCmdCurrentRule;

        public ICommand CmdCurrentRule
        {
            get { return _cCmdCurrentRule ?? (_cCmdCurrentRule = new RelayCommand(ExCmdCurrentRule, CanCmdCurrentRule, true)); }
        }


        private void ExCmdCurrentRule()
        {
            var max1 = 0;
            foreach (var t in ItemsRules)
            {
                if (t.Alow == AlowBak && t.Amax == AmaxBak && t.Index != Index)
                {
                    WlstMessageBox.Show("无法保存", "规则重复！", WlstMessageBoxType.Ok);
                    return;
                }
                if (max1 < t.Amax && Index != t.Index) max1 = t.Amax;
            }
            if (ItemsRules.Count > 0 && ItemsRules.Last().Amax != 2000 && max1 < ItemsRules.Last().Amax) max1 = ItemsRules.Last().Amax;

            if (CurrentSelectRule == ItemsRules[0] && AlowBak != 0)
            {
                WlstMessageBox.Show("无法保存", "第一段规则电流下限必须等于0！", WlstMessageBoxType.Ok);
                return;
            }
            else if (AlowBak < 0)
            {
                WlstMessageBox.Show("无法保存", "电流下限小于0！", WlstMessageBoxType.Ok);
                return;

            }
            else if (AlowBak >= AmaxBak)
            {
                WlstMessageBox.Show("无法保存", "电流下限大于等于电流上限！", WlstMessageBoxType.Ok);
                return;

            }
            else if (LowTimesBak > 1)
            {
                WlstMessageBox.Show("无法保存", "下限系数大于1", WlstMessageBoxType.Ok);
                return;

            }
            else if (MaxTimesBak < 1)
            {
                WlstMessageBox.Show("无法保存", "上限系数小于1", WlstMessageBoxType.Ok);
                return;

            }
            else if (AmaxBak > 1000)
            {
                WlstMessageBox.Show("无法保存", "电流上限大于1000", WlstMessageBoxType.Ok);
                return;
            }
            else if (max1 != 1000)
            {
                WlstMessageBox.Show("无法保存", "最后段规则电流最大上限必须等于1000！", WlstMessageBoxType.Ok);
                return;
            }
            else if (CurrentSelectRule.Amax == 1000 && AmaxBak != 1000)
            {
                WlstMessageBox.Show("无法保存", "最后段规则电流最大上限必须等于1000！", WlstMessageBoxType.Ok);
                return;
            }
            else
            {
                CurrentSelectRule.Alow = AlowBak;
                CurrentSelectRule.Amax = AmaxBak;
                CurrentSelectRule.LowTimes = LowTimesBak;
                CurrentSelectRule.MaxTimes = MaxTimesBak;

                var additem = new RultItem();
                foreach (var t in ItemsRules)
                {
                    if (t.Amax <= AlowBak || t.Alow >= AmaxBak || (t.Alow == AlowBak && t.Amax == AmaxBak))
                    {
                        continue;
                    }
                    else if (t.Alow <= AlowBak && t.Amax < AmaxBak && t.Amax >= AlowBak)
                    {
                        t.Amax = AlowBak;
                    }
                    else if (t.Alow <= AlowBak && t.Amax > AmaxBak)
                    {
                        var max = t.Amax;
                        t.Amax = AlowBak;
                        additem = new RultItem() { Index = ItemsRules.Count + 1, Alow = AmaxBak, Amax = max, LowTimes = t.LowTimes, MaxTimes = t.MaxTimes };
                    }
                    else if (t.Alow >= AlowBak && t.Alow < AmaxBak && t.Amax > AmaxBak)
                    {
                        t.Alow = AmaxBak;
                    }
                    else if ((t.Alow == AlowBak && t.Amax <= AmaxBak) || (t.Amax == AmaxBak && t.Alow >= AlowBak))
                    {
                        t.Amax = 0;
                        t.Alow = 0;
                    }
                }
                if (additem != new RultItem()) ItemsRules.Add(additem);


                var lst = new List<Tuple<int, int, double, double>>();
                foreach (var t in ItemsRules)
                {
                    if (t.Amax == 0 && t.Alow == 0) { }
                    else
                    {
                        var tu = new Tuple<int, int, double, double>(t.Alow, t.Amax, t.LowTimes, t.MaxTimes);
                        lst.Add(tu);
                    }
                }

                var lstafter =
                        (from t in lst orderby t.Item2 select t).ToList();
                var lstlast = new List<Tuple<int, int, double, double>>();

                if (lstafter.Count == 1)
                {
                    lstlast.Add(lst[0]);
                }
                else
                {
                    for (int i = 1; i < lstafter.Count; i++)
                    {
                        var tu = new Tuple<int, int, double, double>(0, 0, 0, 0);
                        if (lstafter[i].Item1 != lstafter[i - 1].Item2)
                        {
                            tu = new Tuple<int, int, double, double>(lstafter[i - 1].Item1, lstafter[i].Item1,
                                                                     lstafter[i - 1].Item3, lstafter[i - 1].Item4);
                        }
                        else
                        {
                            tu = new Tuple<int, int, double, double>(lstafter[i - 1].Item1, lstafter[i - 1].Item2,
                                                                     lstafter[i - 1].Item3, lstafter[i - 1].Item4);
                        }
                        lstlast.Add(tu);

                        if (i == lstafter.Count - 1)
                        {
                            tu = new Tuple<int, int, double, double>(lstafter[i].Item1, lstafter[i].Item2,
                                                                     lstafter[i].Item3, lstafter[i].Item4);
                            lstlast.Add(tu);
                        }
                    }
                }

                lstlast = (from t in lstlast orderby t.Item1 select t).ToList();

                ItemsRules.Clear();
                var index = 0;
                foreach (var t in lstlast)
                {
                    if (t.Item1 != t.Item2)
                    {
                        index++;
                        ItemsRules.Add(new RultItem()
                        {
                            Index = index,
                            Alow = t.Item1,
                            Amax = t.Item2,
                            LowTimes = t.Item3,
                            MaxTimes = t.Item4
                        });
                    }
                }

                foreach (var t in ItemsRules)
                {
                    if (t.Amax <= t.Alow)
                    {
                        CurrentSelectRule = t;
                        ExCmdDeleteRules();
                    }
                }

            }
        }

        private bool CanCmdCurrentRule()
        {
            return true;
        }

        #endregion

        #endregion

        #region Cmd4To5

        private ICommand _cmdCmd4To5;
        public ICommand Cmd4To5
        {
            get { return _cmdCmd4To5 ?? (_cmdCmd4To5 = new RelayCommand(ExCmd4To5, CanCmd4To5, true)); }
        }

        private void ExCmd4To5()
        {
            RbGrid5Old = false;

            var itemrule = new List<RtuSetsNew.LoopSxxRuleItem>();
            foreach (var t in ItemsRules)
            {
                itemrule.Add(new RtuSetsNew.LoopSxxRuleItem()
                {
                    Index = t.Index,
                    CurrentALow = t.Alow,
                    CurrentAMax = t.Amax,
                    LowerTimes = t.LowTimes,
                    MaxTimes = t.MaxTimes
                });
            }

            var sxxitem = new List<RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx>();
            var ntgs = (from t in ItemsRules orderby t.Index ascending select t).ToList();
            foreach (var t in SetAll)
            {
                foreach (var f in ntgs)
                {
                    if (f.Alow <= t.Avg && t.Avg <= f.Amax && t.Avg >= 0)
                    {
                        t.Xx = (int) (t.Avg*f.LowTimes);

                        if (t.Avg*f.MaxTimes - (int) (t.Avg*f.MaxTimes) >= 0.5)
                            t.Sx = (int) (t.Avg*f.MaxTimes) + 1;
                        else
                            t.Sx  = (int) (t.Avg*f.MaxTimes);
                    }
                }
                sxxitem.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                                {
                                    Avg = t.Avg,
                                    LoopId = t.LoopId,
                                    RtuId = t.RtuId,
                                    Sx =t.Sx,
                                    Xx = t.Xx
                                });
            }
            var sxx = new RtuSetsNew.RtuLoopSxxSetGet()
                          {
                              SectionId = SectionId,
                              SxxItems = sxxitem
                          };


            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new;
            info.WstRtuLdlSxxAvgSetNew.Op = 14;
            info.WstRtuLdlSxxAvgSetNew.SxxRuleItems = itemrule;
            info.WstRtuLdlSxxAvgSetNew.SxxItems.Clear();
            info.WstRtuLdlSxxAvgSetNew.SxxItems.Add(sxx);
            SndOrderServer.OrderSnd(info, 10, 5);
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存上下限设置，请稍后...";
        }

        private bool CanCmd4To5()
        {
            return true;
        }

        #endregion

        #region Grid5

        private ICommand _cmdCmd5;
        public ICommand Cmd5
        {
            get { return _cmdCmd5 ?? (_cmdCmd3To4 = new RelayCommand(ExCmd5, CanCmd5, true)); }
        }

        private void ExCmd5()
        {
            if (RbGrid5Old)
            {
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 操作完成。";
            }
            else
            {
                var sxxitem = new List<RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx>();
                foreach (var t in NoSetAll)
                {
                    var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.RtuId];
                    var amps = tml as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                    sxxitem.Add(new RtuSetsNew.RtuLoopSxxSetGet.RtuLoopSxx()
                                    {
                                        Avg = t.Avg,
                                        LoopId = t.LoopId,
                                        RtuId = t.RtuId,
                                        Sx = amps.WjLoops[t.LoopId].CurrentRange,
                                        Xx = 0
                                    });
                }

                var sxx = new RtuSetsNew.RtuLoopSxxSetGet()
                {
                    SectionId = SectionId,
                    SxxItems = sxxitem
                };

                var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new;
                info.WstRtuLdlSxxAvgSetNew.Op = 14;
                info.WstRtuLdlSxxAvgSetNew.SxxItems.Clear();
                info.WstRtuLdlSxxAvgSetNew.SxxItems.Add(sxx);
                SndOrderServer.OrderSnd(info, 10, 5);
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存放弃回路，请稍后...";
            }

        }

        private bool CanCmd5()
        {
            return true;
        }





        private bool _rbGrid5Old;
        public bool RbGrid5Old
        {
            get { return _rbGrid5Old; }
            set
            {
                if (_rbGrid5Old != value)
                {
                    _rbGrid5Old = value;
                    this.RaisePropertyChanged(() => this.RbGrid5Old);
                }
            }
        }



        private int _grid5SuccessLoop;
        public int Grid5SuccessLoop
        {
            get { return _grid5SuccessLoop; }
            set
            {
                if (_grid5SuccessLoop != value)
                {
                    _grid5SuccessLoop = value;
                    this.RaisePropertyChanged(() => this.Grid5SuccessLoop);
                }
            }
        }

        private int _grid5FailLoop;
        public int Grid5FailLoop
        {
            get { return _grid5FailLoop; }
            set
            {
                if (_grid5FailLoop != value)
                {
                    _grid5FailLoop = value;
                    this.RaisePropertyChanged(() => this.Grid5FailLoop);
                }
            }
        }
        #endregion

    }

    public partial class RtuAmpSxxViewModel
    {
        private string remak;
        public string Remark
        {
            get { return  remak; }
            set
            {
                if (value == remak) return;
                remak = value;
                this.RaisePropertyChanged(() => this.Remark);
            }
        }

        private ObservableCollection<RultItem> _itemItemsRuless;

        public ObservableCollection<RultItem> ItemsRules
        {
            get
            {
                if (_itemItemsRuless == null) _itemItemsRuless = new ObservableCollection<RultItem>();
                return _itemItemsRuless;
            }
        }
        private RultItem _rCurrentSelectRule;
        public RultItem CurrentSelectRule
        {
            get { return _rCurrentSelectRule; }
            set
            {
                if (value == _rCurrentSelectRule) return;
                _rCurrentSelectRule = value;
                this.RaisePropertyChanged(() => this.CurrentSelectRule);
                if (CurrentSelectRule != null)
                {
                    AlowBak = CurrentSelectRule.Alow;
                    AmaxBak = CurrentSelectRule.Amax;
                    LowTimesBak = CurrentSelectRule.LowTimes;
                    MaxTimesBak = CurrentSelectRule.MaxTimes;
                    Index = CurrentSelectRule.Index;
                }
                if (ItemsRules.Count <= 1)
                {
                    IndexEnable = false;
                    IndexEnable2 = true;
                }
                else
                {
                    IndexEnable = true;
                    IndexEnable2 = true;
                }
            }
        }

        #region Bak


        private bool _indexEnable;
        public bool IndexEnable
        {
            get { return _indexEnable; }
            set
            {
                if (value == _indexEnable) return;
                _indexEnable = value;
                this.RaisePropertyChanged(() => this.IndexEnable);
            }
        }
        private bool _indexEnable2;
        public bool IndexEnable2
        {
            get { return _indexEnable2; }
            set
            {
                if (value == _indexEnable2) return;
                _indexEnable2 = value;
                this.RaisePropertyChanged(() => this.IndexEnable2);
            }
        }

        int xxAlowBak;
        int xxAmaxBak;
        double xxLowTimesBak;
        double xxMaxTimesBak;
        int xxIndex;

        public int AlowBak
        {
            get { return xxAlowBak; }
            set
            {
                if (value == xxAlowBak) return;
                xxAlowBak = value;
                this.RaisePropertyChanged(() => this.AlowBak);
            }
        }
        public int AmaxBak
        {
            get { return xxAmaxBak; }
            set
            {
                if (value == xxAmaxBak) return;
                xxAmaxBak = value;
                this.RaisePropertyChanged(() => this.AmaxBak);
            }
        }
        public double LowTimesBak
        {
            get { return xxLowTimesBak; }
            set
            {
                if (value == xxLowTimesBak) return;
                if (value > 1) value = 0.99;
                xxLowTimesBak = value;
                this.RaisePropertyChanged(() => this.LowTimesBak);
            }
        }
        public double MaxTimesBak
        {
            get { return xxMaxTimesBak; }
            set
            {
                if (value == xxMaxTimesBak) return;
                if (value < 1) value = 1.01;
                xxMaxTimesBak = value;
                this.RaisePropertyChanged(() => this.MaxTimesBak);
            }
        }
        public int Index
        {
            get { return xxIndex; }
            set
            {
                if (value == xxIndex) return;
                xxIndex = value;
                this.RaisePropertyChanged(() => this.Index);
            }
        }
        #endregion

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new,
                RequestOrUpdateFaultTypeInfoNew,
                typeof(RtuAmpSxxViewModel), this);
        }


        private void RequestOrUpdateFaultTypeInfoNew(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var data = infos.WstRtuLdlSxxAvgSetNew;
            if (data.Op == 3)
            {
                #region 
                var tmllist = OutSelectTmlList;
                if (data.AreaId == AreaId)
                {
                    if (data.LoopdataItems.ItemLoopdata != null && data.LoopdataItems != null)
                    {
                        EditItemAll.Clear();

                        foreach (var t in data.LoopdataItems.ItemLoopdata)
                        {
                            var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.RtuId];
                            if (tmllist.Contains(t.RtuId)) tmllist.Remove(t.RtuId);

                            if (!EditItemAll.ContainsKey(t.RtuId))
                            {
                                var ab = new List<Tuple<bool, double>>();
                                foreach (var i in t.A)
                                {
                                    ab.Add(new Tuple<bool, double>(true,i));
                                }

                                var sss = new EditItems()
                                              {
                                                  Id = tml.RtuPhyId,
                                                  RtuId = tml.RtuId,
                                                  LoopId = t.LoopId,
                                                  LoopName = tml.GetLoopName(t.LoopId),
                                                  ABase = ab
                                              };
                                EditItemAll.Add(t.RtuId, sss);
                            }
                            else
                            {
                                var ab = new List<Tuple<bool, double>>();
                                foreach (var i in t.A)
                                {
                                    ab.Add(new Tuple<bool, double>(true,i));
                                }

                                EditItemAll[t.RtuId].ABase = ab;
                            }
                        }

                        Grid2DataSum = data.LoopdataItems.ItemLoopdata.Count;
                    }

                    foreach (var i in tmllist)
                    {
                        var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[i];
                        NoDataTmlList.Add(new WatchItems()
                                              {
                                                  Id = tml.RtuPhyId,
                                                  Name = tml.RtuName,
                                                  RtuId = i
                                              });
                    }
                    Grid2NoDataTmlSum = NoDataTmlList.Count;


                    foreach (var i in EditItemAll)
                    {
                        var sum = 0.00;
                        var count = 0.00;
                        var average = 0.00;
                        var per = new List<Tuple<double, double>>();
                        foreach (var t in i.Value.ABase)
                        {
                            sum = sum + t.Item2;
                            count++;
                        }
                        average = sum/count;
                        var max = 1.00;
                        var min = 1.00;

                        foreach (var t in i.Value.ABase)
                        {
                            var perb = t.Item2 / average;
                            var tu = new Tuple<double, double>(t.Item2, perb);
                            per.Add(tu);

                            if (perb > max) max = perb;
                            if (perb < min) min = perb;
                        }
                        per.Sort();

                        if (max > 1.2 || min < 0.8)
                        {
                            var aaa = new List<string>();
                            foreach (var t in per)
                            {
                                aaa.Add(t.Item1.ToString("f2") + "(" + t.Item2.ToString("f2") + ")");
                            }
                            if (aaa.Count < 20)
                            {
                                for (int a = aaa.Count; a < 21; a++)
                                {
                                    aaa.Add("--");
                                }
                            }

                            EditItem20.Add(new EditItems()
                                               {
                                                   AverageA = average,
                                                   Id = i.Value.Id,
                                                   RtuId = i.Value.RtuId,
                                                   LoopId = i.Value.LoopId,
                                                   LoopName = i.Value.LoopName,
                                                   A = aaa,
                                               });


                        }
                        else if ((max <= 1.2 && max > 1.1) || (min >= 0.8 && min < 0.9))
                        {
                            var aaa = new List<string>();
                            foreach (var t in per)
                            {
                                aaa.Add(t.Item1.ToString("f2") + "(" + t.Item2.ToString("f2") + ")");
                            }
                            if (aaa.Count < 20)
                            {
                                for (int a = aaa.Count; a < 21; a++)
                                {
                                    aaa.Add("--");
                                }
                            }

                            EditItem10to20.Add(new EditItems()
                                                   {
                                                       AverageA = average,
                                                       Id = i.Value.Id,
                                                       RtuId = i.Value.RtuId,
                                                       LoopId = i.Value.LoopId,
                                                       LoopName = i.Value.LoopName,
                                                       A = aaa,
                                                   });

                        }
                        else if ((max <= 1.1 && max > 1.05) || (min >= 0.9 && min < 0.95))
                        {
                            var aaa = new List<string>();
                            foreach (var t in per)
                            {
                                aaa.Add(t.Item1.ToString("f2") + "(" + t.Item2.ToString("f2") + ")");
                            }
                            if (aaa.Count < 20)
                            {
                                for (int a = aaa.Count; a < 21; a++)
                                {
                                    aaa.Add("--");
                                }
                            }

                            EditItem5to10.Add(new EditItems()
                                                  {
                                                      AverageA = average,
                                                      Id = i.Value.Id,
                                                      RtuId = i.Value.RtuId,
                                                      LoopId = i.Value.LoopId,
                                                      LoopName = i.Value.LoopName,
                                                      A = aaa,
                                                  });

                        }
                        else if (max <= 1.05 || min >= 0.95)
                        {
                            WatchItem.Add(new WatchItems()
                                              {
                                                  AverageA = average,
                                                  Id = i.Value.Id,
                                                  RtuId = i.Value.RtuId,
                                                  LoopId = i.Value.LoopId,
                                                  LoopName = i.Value.LoopName
                                              });
                        }

                    }

                    Grid3per5 = WatchItem.Count;
                    Grid3per5to10 = EditItem5to10.Count;
                    Grid3per10to20 = EditItem10to20.Count;
                    Grid3per20 = EditItem20.Count;

                    Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 回路数据已返回。";
                }
                else
                {
                    Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 回路数据请求失败。";
                }
                #endregion
            }
            else if (data.Op == 4)
            {
                #region
                ItemsRules.Clear();
                if (data.SxxRuleItems !=null)
                {
                    if (data.AreaId == AreaId)
                    {
                        var sr = data.SxxRuleItems;
                        foreach (var t in sr)
                        {
                            ItemsRules.Add(new RultItem()
                            {
                                Alow = t.CurrentALow,
                                Amax = t.CurrentAMax,
                                Index = t.Index,
                                LowTimes = t.LowerTimes,
                                MaxTimes = t.MaxTimes
                            });
                        }

                        if (ItemsRules.Count < 1)
                        {
                            ItemsRules.Add(new RultItem()
                            {
                                Index = 1,
                                Alow = 0,
                                Amax = 1000,
                                LowTimes = 0.8,
                                MaxTimes = 1.2
                            });
                        }
                        CurrentSelectRule = ItemsRules.First();

                    }
                    Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 上下限规则已返回。";
                }
                else
                {
                    Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 回路数据请求失败。";
                }
               
                #endregion
            }
            else if (data.Op == 14)
            {
                foreach (var t in data.SxxItems)
                {
                    if (t.SectionId == SectionId)
                    {
                        Grid5SuccessLoop=t.SxxItems.Count;
                    }
                }
                Grid5FailLoop = NoSetAll.Count;

                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 上下限设置成功。";
            }
        }
    }

    public partial class RtuAmpSxxViewModel
    {
        
    }

    public class RultItem:Wlst .Cr .Core .CoreServices .ObservableObject 
    {
         int xxIndex;
         int xxAlow;
         int xxAmax;
         double xxLowTimes;
         double xxMaxTimes;


         public int Index
         {
             get { return xxIndex; }
             set
             {
                 if (value == xxIndex) return;
                 xxIndex = value;
                 this.RaisePropertyChanged(() => this.Index);
             }
         }
        public int Alow
        {
            get { return xxAlow; }
            set
            {
                if (value == xxAlow) return;
                xxAlow = value;
                this.RaisePropertyChanged(() => this.Alow);
            }
        }
        public int Amax
        {
            get { return xxAmax; }
            set
            {
                if (value == xxAmax) return;
                xxAmax = value;
                this.RaisePropertyChanged(() => this.Amax);
            }
        }
        public double LowTimes
        {
            get { return xxLowTimes; }
            set
            {
                if (value == xxLowTimes) return;
                if (value >= 1) value = 0.99;
                if (value <= 0) value = 0;
                xxLowTimes = value;
                this.RaisePropertyChanged(() => this.LowTimes);
            }
        }
        public double MaxTimes
        {
            get { return xxMaxTimes; }
            set
            {
                if (value == xxMaxTimes) return;
                if (value <= 1) value = 1.01;
                xxMaxTimes = value;
                this.RaisePropertyChanged(() => this.MaxTimes);
            }
        }




    }




}
