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
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.IIRtuAmpSxxNewOne;
using Wlst.client;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNewOne
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
            isViewActive = true; 
            SaveFlg = false;

            AreaName.Clear();
            getAreaRId();
            if (AreaName.Count > 0) AreaComboBoxSelected = AreaName.First();
            if (AreaName.Count > 1) Visi = Visibility.Visible;
            else Visi = Visibility.Collapsed;
        }

        public void OnUserHideOrClosing()
        {
            isViewActive = false;
            this.TmlItem.Clear();
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
            get { return "回路电流上下限设置"; }
        }

        #endregion



        #region CmdReqSxx

        private ICommand _cmdCmdReqSxx;

        public ICommand CmdReqSxx
        {
            get { return _cmdCmdReqSxx ?? (_cmdCmdReqSxx = new RelayCommand(ExCmdReqSxx, CanCmdReqSxx, true)); }
        }


        private DateTime _dtCmdReqSxx = DateTime.Now.AddDays(-1);

        private void ExCmdReqSxx()
        {
            _dtCmdReqSxx = DateTime.Now;

            this.ReqAvgInfo();

            var sum = 0;
            var use = 0;
            var nouse = 0;
            foreach (var t in TmlItem)
            {
                foreach (var tt in t.Child)
                {
                    if (tt.IsSelect)
                    {
                        sum++;
                        use = tt.LoopUsed + use;
                        nouse = tt.LoopNoUsed + nouse;
                    }
                }
            }

            SelectTmlSum = sum;
            SelectTmlNoUsed = nouse;
            SelectTmlUsed = use;

        }

        private bool CanCmdReqSxx()
        {
            return DateTime.Now.Ticks - _dtCmdReqSxx.Ticks > 10000000;
        }

        #endregion

        #region CmdUpdate

        private ICommand _cCmdUpdate;

        public ICommand CmdUpdate
        {
            get { return _cCmdUpdate ?? (_cCmdUpdate = new RelayCommand(ExCmdUpdate, CanCmdUpdate, true)); }
        }

        private DateTime _dtUpdate = DateTime.Now.AddDays(-1);

        private void ExCmdUpdate()
        {
            _dtUpdate = DateTime.Now;


            var lstbefore =
                (from t in ItemsRules orderby t.Alow select t).ToList();

            var lst = new List<int>();
            foreach (var t in lstbefore)
            {
                lst.Add(t.Alow);
                lst.Add(t.Amax);
            }

            if (lst.First()!=0 || lst.Last()!=1000)
            {
                WlstMessageBox.Show("无法保存", "规则未完全包含0-1000A！", WlstMessageBoxType.Ok);
                return;
            }
            for (int i = 1; i < lst.Count - 1; i=i+2)
            {
                if (lst[i]!=lst[i+1])
                {
                    WlstMessageBox.Show("无法保存", "规则未完全包含0-1000A！", WlstMessageBoxType.Ok);
                    return;
                }
            }


            this.UpdateRules();
            this.ReqLowMaxInfo();

            SaveFlg = false;
        }

        private bool CanCmdUpdate()
        {
            if (SaveFlg && DateTime.Now.Ticks - _dtUpdate.Ticks > 100000000 && SelectTmlSum > 0)return true;
            else return false;
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
                if (t.Alow == AlowBak && t.Amax == AmaxBak && t.Index!=Index)
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
            else if (max1 != 1000 )
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
                        additem =new RultItem() { Index = ItemsRules.Count + 1, Alow = AmaxBak, Amax = max, LowTimes = t.LowTimes, MaxTimes = t.MaxTimes };
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
                    if (t.Amax == 0 && t.Alow==0){}
                    else
                    {
                        var tu = new Tuple<int, int, double, double>(t.Alow, t.Amax, t.LowTimes, t.MaxTimes);
                        lst.Add(tu);
                    }
                }

                var lstafter =
                        (from t in lst orderby t.Item2  select t).ToList();
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

        #region CmdAddRule

        private ICommand _cCmdCmdAddRule;

        public ICommand CmdAddRule
        {
            get { return _cCmdCmdAddRule ?? (_cCmdCmdAddRule = new RelayCommand(ExCmdAddRule, CanCmdAddRule, true)); }
        }


        private void ExCmdAddRule()
        {
            dt = DateTime.Now;
            if (this.ItemsRules.Count>1)
            {
                this.ItemsRules.Add(new RultItem() { Index = ItemsRules.Count + 1, Alow = 1000, Amax = 1000, LowTimes = 0.8, MaxTimes = 1.2 });
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


        #region CmdAllSelected

        private ICommand _CmdAllSelected;

        public ICommand CmdAllSelected
        {
            get { return _CmdAllSelected ?? (_CmdAllSelected = new RelayCommand(ExCmdAllSelected, CanCmdAllSelected, true)); }
        }


        private void ExCmdAllSelected()
        {
            if (TmlItem.Count>0)
            {
                if (TmlItem[0].IsSelect == false)
                {
                    foreach (var t in TmlItem)
                    {
                        t.IsSelect = true;
                    }
                }
                else
                {
                    foreach (var t in TmlItem)
                    {
                        t.IsSelect = false;
                    }
                }
            }
        }


        private bool CanCmdAllSelected()
        {
            return true;
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
                    AreaName.Add(new AreaInt() { Value = t.Value.AreaId.ToString("d2") + "-"+area, Key = t.Value.AreaId });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") +"-"+ area, Key = t });
                    }
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

                    TmlItem.Clear();
                    ReqRulesInfo();

                    var loopusedd = 0;
                    var loopnousedd = 0;
                    var tmlsum = 0;
                    var tmllist = new List<int>();

                    foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
                    {
                        if (t.Key.Item1 == AreaId)
                        {
                            var child = new ObservableCollection<TmlItems>();

                            //排序 lvf 2019年5月24日13:26:27
                            var sortLst1 =
                   Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(t.Value.LstTml);

                            foreach (var tt in sortLst1)
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
                                            if (g.Value.SwitchOutputId == 0) continue;
                                            if (g.Value.IsShieldLoop != 1 )
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
                                TmlItem.Add(new TmlItems()
                                                {
                                                    Id = t.Value.GroupId,
                                                    Name = t.Value.GroupName,
                                                    IsFather = true,
                                                    IsSelect = false,
                                                    Child = child
                                                });
                            }
                        }
                    }
                    var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);

                    // 增加排序 lvf 2019年5月24日13:29:22
                    var sortLst =
                   Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(rtulst);

                    if (rtulst.Count > 0)
                    {
                        var child1 = new ObservableCollection<TmlItems>();
                        foreach (var tt in sortLst)
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
                                        if (g.Value.SwitchOutputId == 0) continue;
                                        if (g.Value.IsShieldLoop != 1 )
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
                                                       IsSelect = false,
                                                       LoopNoUsed = loopnoused,
                                                       LoopUsed = loopused
                                                   });
                                }
                            }
                        }
                        TmlItem.Add(new TmlItems()
                                        {
                                            Id = -1,
                                            Name = "未分组终端",
                                            IsFather = true,
                                            IsSelect = false,
                                            Child = child1
                                        });


                    }

                    //SelectTmlNoUsed = loopnousedd;
                    //SelectTmlUsed = loopusedd;
                    //SelectTmlSum = tmlsum;

                    SelectTmlNoUsed = 0;
                    SelectTmlUsed = 0;
                    SelectTmlSum = 0;

                }
            }
        }

        private ObservableCollection<TmlItems> _devicesitem;
        public ObservableCollection<TmlItems> TmlItem
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
                    this.RaisePropertyChanged(() => this.TmlItem);
                }
            }
        }

        public class TmlItems : Wlst.Cr.Core.CoreServices.ObservableObject
        {


            private string _iAttachInfod;
            public string  AttachInfo
            {
                get { return _iAttachInfod; }
                set
                {
                    if (_iAttachInfod != value)
                    {
                        _iAttachInfod = value;
                        this.RaisePropertyChanged(() => this.AttachInfo);
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

        private ObservableCollection<TmlLoopItems> _devicesloopitem;
        public ObservableCollection<TmlLoopItems> TmlLoopItem
        {
            get
            {
                if (_devicesloopitem == null)
                {
                    _devicesloopitem = new ObservableCollection<TmlLoopItems>();
                }
                return _devicesloopitem;
            }
            set
            {
                if (value != _devicesloopitem)
                {
                    _devicesloopitem = value;
                    this.RaisePropertyChanged(() => this.TmlLoopItem);
                }
            }
        }
        public class TmlLoopItems : Wlst.Cr.Core.CoreServices.ObservableObject
        {

            private string  _isdfadfd;
            public string  AttachInfo
            {
                get { return _isdfadfd; }
                set
                {
                    if (_isdfadfd != value)
                    {
                        _isdfadfd = value;
                        this.RaisePropertyChanged(() => this.AttachInfo);
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

            private double _averagea;
            public double AverageA
            {
                get { return _averagea; }
                set
                {
                    if (_averagea != value)
                    {
                        _averagea = value;
                        this.RaisePropertyChanged(() => this.AverageA);
                    }
                }
            }

            private double _amax;
            public double AMax
            {
                get { return _amax; }
                set
                {
                    if (_amax != value)
                    {
                        _amax = value;
                        this.RaisePropertyChanged(() => this.AMax);
                    }
                }
            }

            private double _amin;
            public double AMin
            {
                get { return _amin; }
                set
                {
                    if (_amin != value)
                    {
                        _amin = value;
                        this.RaisePropertyChanged(() => this.AMin);
                    }
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

        private bool _saveflg;
        public bool SaveFlg
        {
            get { return _saveflg; }
            set
            {
                if (value == _saveflg) return;
                _saveflg = value;
                this.RaisePropertyChanged(() => this.SaveFlg);
            }
        }

        private Visibility _visi;
        public Visibility Visi
        {
            get { return _visi; }
            set
            {
                if (value == _visi) return;
                _visi = value;
                this.RaisePropertyChanged(() => this.Visi);
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
                Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set,
                RequestOrUpdateFaultTypeInfo,
                typeof(RtuAmpSxxViewModel), this);
        }


        private void RequestOrUpdateFaultTypeInfo(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (isViewActive == false)
            {
                var datax = infos.WstRtuLdlSxxAvgSet;
                if (datax != null && datax.Op == 14)
                {
                    var dic = new Dictionary<int, List<RtuSets.RtuLoopSxx>>();
                    foreach (var f in datax.SxxItems)
                    {
                        if (dic.ContainsKey(f.RtuId) == false) dic.Add(f.RtuId, new List<RtuSets.RtuLoopSxx>());
                        dic[f.RtuId].Add(f);
                    }

                    foreach (var f in dic)
                    {
                        var tmpequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.Key);
                        var tmpequ2 = tmpequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                        if (tmpequ2 == null) continue;
                        foreach (var g in f.Value)
                        {
                            if (tmpequ2.WjLoops.ContainsKey(g.LoopId))
                            {
                                tmpequ2.WjLoops[g.LoopId].CurrentAlarmUpperlimit = g.SxDefault;
                                tmpequ2.WjLoops[g.LoopId].CurrentAlarmLowerlimit = g.XxDefault;

                            }
                        }
                    }
                }
                return;
            }

            var data = infos.WstRtuLdlSxxAvgSet;
            if (data == null) return;
            if (data.Op == 1 || data.Op == 11)
            {
                this.ItemsRules.Clear();
                foreach (var f in data.SxxRuleItems)
                {
                    if (f.AreaId == AreaId)
                    {
                        foreach (var t in f.SxxRuleItem)
                        {
                            ItemsRules.Add(new RultItem()
                            {
                                Index = t.Index,
                                Alow = t.CurrentALow,
                                Amax = t.CurrentAMax,
                                LowTimes = Math.Round(t.LowerTimes, 2),
                                MaxTimes = Math.Round(t.MaxTimes, 2)
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

                    }
                }

                if (data.Op == 1) Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 服务器返回平均电流计算规则.";
                else Remark = Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  平均电流计算规则更新，更新后的上下限信息将在客户端下次登录后更新...";

                CurrentSelectRule = ItemsRules.First();
            }
            else if (data.Op == 3)
            {
                TmlLoopItem.Clear();
                foreach (var f in data.LoopAvgItems)
                {
                    var id = 0;
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(f.RtuId))
                    {
                        id = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[f.RtuId].RtuPhyId;
                    }
                    TmlLoopItem.Add(new TmlLoopItems()
                                        {
                                            RtuId = f.RtuId,
                                            LoopId = f.LoopId,
                                            Id = id,
                                            AverageA = f.Avg
                                        });
                }

                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 请求回路平均电流成功.";
                SaveFlg = true;
            }
            else if (data.Op == 14 )
            {
                var ntg = data.SxxItems;
               
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 终端平均电流、上下限信息保存成功.";
            }



        }
    }

    public partial class RtuAmpSxxViewModel
    {
        private void ReqAvgInfo()
        {
            var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_ldl_sxx_avg_set ;

            foreach (var t in TmlItem)
            {
                foreach (var f in t.Child)
                {
                    if (f.IsSelect)
                    {
                        if (!info.WstRtuLdlSxxAvgSet.RtuIds.Contains(f.RtuId))
                        {
                            info.WstRtuLdlSxxAvgSet.RtuIds.Add(f.RtuId);
                        }
                    }
                }
            }

            info.WstRtuLdlSxxAvgSet.Op = 3;
            SndOrderServer.OrderSnd(info, 10, 2);
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在请求平均电流...";

        }

        private void ReqRulesInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu .wst_ldl_sxx_avg_set ;//.ServerListen.wlst_cnt_wj3090_request_ldl_loop_current_avg;
            info.WstRtuLdlSxxAvgSet.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 2);
        }

        private void ReqLowMaxInfo()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set;
            info.WstRtuLdlSxxAvgSet.Op = 14;

            if (TmlLoopItem != null && TmlLoopItem.Count > 0)
            {
                var ntgs = (from t in ItemsRules orderby t.Index ascending select t).ToList();
                foreach (var t in TmlLoopItem)
                {
                    foreach (var f in ntgs)
                    {
                        if (f.Alow <= t.AverageA && t.AverageA <= f.Amax && t.AverageA >= 0)
                        {
                            t.AMin = (int)(t.AverageA * f.LowTimes);

                            if (t.AverageA*f.MaxTimes - (int) (t.AverageA*f.MaxTimes) >= 0.5)
                                t.AMax = (int) (t.AverageA*f.MaxTimes) + 1;
                            else
                                t.AMax = (int) (t.AverageA*f.MaxTimes);
                        }
                    }
                }
            }

            var sxxitem = new List<RtuSets.RtuLoopSxx>();
            foreach (var t in TmlLoopItem)
            {
                sxxitem.Add(new RtuSets.RtuLoopSxx()
                                {
                                    RtuId = t.RtuId,
                                    Avg = t.AverageA,
                                    LoopId = t.LoopId,
                                    SxDefault = (int)t.AMax,
                                    XxDefault = (int)t.AMin
                                });
            }


            info.WstRtuLdlSxxAvgSet.SxxItems = sxxitem;
            SndOrderServer.OrderSnd(info, 10, 4);
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 更新已经提交，修改后的上下限信息将在客户端下次登录后更新...";

        }

        private void UpdateRules()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set;
            info.WstRtuLdlSxxAvgSet.Op = 11;

            var sxxruleitems = new List<RtuSets.LoopSxxRuleItem>();
            foreach (var t in ItemsRules)
            {
                sxxruleitems.Add(new RtuSets.LoopSxxRuleItem()
                                     {
                                         CurrentALow = t.Alow,
                                         CurrentAMax = t.Amax,
                                         Index = t.Index,
                                         LowerTimes = t.LowTimes,
                                         MaxTimes = t.MaxTimes
                                     });
            }

            var end = new List<int>();
            var start = new List<int>();

            info.WstRtuLdlSxxAvgSet.SxxRuleItems.Add(new RtuSets.LoopSxxRule()
            {
                AreaId = AreaId,
                OpArguEnd = end,
                OpArguStart = start,
                SxxRuleItem = sxxruleitems
            });

            SndOrderServer.OrderSnd(info, 10, 5);
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 更新已经提交，修改后的上下限信息将在客户端下次登录后更新...";
        }

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
