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
using Wlst.client;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxx
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
            TwoVisi = Visibility.Hidden;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            isViewActive = true;
            AreaName.Clear();
            getAreaRId();
            if (AreaName.Count > 0) AreaComboBoxSelected = AreaName.First();
            if (AreaName.Count > 1) Visi = Visibility.Visible;
            else Visi = Visibility.Collapsed;

            //Items.Clear();
            //var ntsg = (from t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
            //            where t.Value.EquipmentType == WjParaBase.EquType.Rtu
            //            orderby t.Key ascending
            //            select t).ToList();

            //foreach (var f in ntsg)
            //{
            //    var amps = f.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
            //    if (amps == null) continue;
            //    var ordntgs =
            //        (from g in amps.WjLoops orderby g.Value.LoopId select g.Value).ToList();
            //    foreach (var g in ordntgs)
            //    {
            //        this.Items.Add(new ObservableObjectCollection(4, 4)
            //                           {
            //                               ValueString0 = amps.RtuPhyId.ToString("D4"),
            //                               ValueString1 = f.Value.RtuName,
            //                               ValueString2 = g.CurrentRange == 0 ? "屏蔽" : "使用",
            //                               //回路使用状态

            //                               ValueString3 = "--",
            //                               //回路电量平均值


            //                               ValueInt0 = f.Key,
            //                               //终端地址
            //                               ValueInt1 = g.LoopId,
            //                               //回路地址
            //                               ValueInt2 = g.CurrentAlarmLowerlimit,
            //                               //下限
            //                               ValueInt3 = g.CurrentAlarmUpperlimit,
            //                               //上限
            //                           });
            //    }
            //}
            //ReqRulesInfo();
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
        }

        private bool CanCmdReqSxx()
        {
            return DateTime.Now.Ticks - _dtCmdReqSxx.Ticks > 90000000;
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

            if ((TwoStart != 1500 && TwoEnd == 1500) || (TwoEnd != 1500 && TwoStart == 1500))
            {
                WlstMessageBox.Show("无法保存", "第二段生效时间有误！", WlstMessageBoxType.Ok);
                return;
            }
            if ((TwoStart2 != 1500 && TwoEnd2 == 1500) || (TwoEnd2 != 1500 && TwoStart2 == 1500))
            {
                WlstMessageBox.Show("无法保存", "第二段生效时间有误！", WlstMessageBoxType.Ok);
                return;
            }

            this.UpdateRules();
            this.ReqLowMaxInfo();
        }

        private bool CanCmdUpdate()
        {
            return DateTime.Now.Ticks - _dtUpdate.Ticks > 200000000;
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

        #region CmdUpdateCurrent

        private ICommand _cCmdCmdUpdateCurrent;

        public ICommand CmdUpdateCurrent
        {
            get
            {
                return _cCmdCmdUpdateCurrent ??
                       (_cCmdCmdUpdateCurrent = new RelayCommand(ExCmdUpdateCurrent, CanCmdUpdateCurrent, true));
            }
        }

        public bool cancmdupdatecurrent;
        private void ExCmdUpdateCurrent()
        {
            _dtUpdate = DateTime.Now;

            var ntgs = (from t in ItemsRules orderby t.Index ascending select t).ToList();
            foreach (var t in Items)
            {
                foreach (var a in t.ChildTreeItems)
                {
                    double avg = 0;
                    double avg2 = 0;
                    if (Double.TryParse(a.ValueString3, out avg))
                    {
                        foreach (var f in ntgs)
                        {
                            if (f.Alow < avg && avg < f.Amax && a.IsChecked && avg > 0)
                            {
                                a.ValueInt2 = (int) (avg*f.LowTimes);

                                if (avg*f.MaxTimes - (int) (avg*f.MaxTimes) >= 0.5)
                                    a.ValueInt3 = (int) (avg*f.MaxTimes) + 1;
                                else
                                    a.ValueInt3 = (int) (avg*f.MaxTimes);

                                //break;
                            }
                        }
                    }

                    if (Double.TryParse(a.ValueString4, out avg2))
                    {
                        foreach (var f in ntgs)
                        {
                            if (a.ValueString4 != null && a.IsChecked && f.Alow < avg2 && avg2 < f.Amax && avg2 > 0)
                            {
                                a.ValueInt4 = (int)(avg2 * f.LowTimes);

                               if (avg2 * f.MaxTimes - (int)(avg2 * f.MaxTimes) >= 0.5)
                                    a.ValueInt5 = (int)(avg2 * f.MaxTimes) + 1;
                                else
                                    a.ValueInt5 = (int)(avg2 * f.MaxTimes);

                            }
                        }
                    }

                    
                }
            }  
        }

        private bool CanCmdUpdateCurrent()
        {
            return  DateTime.Now.Ticks - _dtUpdate.Ticks > 25000000;
            //cancmdupdatecurrent &&
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

        #region CmdAllSelected

        private ICommand _CmdAllSelected;

        public ICommand CmdAllSelected
        {
            get { return _CmdAllSelected ?? (_CmdAllSelected = new RelayCommand(ExCmdAllSelected, CanCmdAllSelected, true)); }
        }


        private void ExCmdAllSelected()
        {
            if (Items.Count>0)
            {
                if (Items[0].IsChecked == false)
                {
                    foreach (var t in Items)
                    {
                        t.IsChecked = true;
                    }
                }
                else
                {
                    foreach (var t in Items)
                    {
                        t.IsChecked = false;
                    }
                }
            }
           
        }


        private bool CanCmdAllSelected()
        {
            return true;
        }

        #endregion


        #region CmdReqSxxXx

        private ICommand _cmdReqSxxXx;

        public ICommand CmdReqSxxXx
        {
            get { return _cmdReqSxxXx ?? (_CmdAllSelected = new RelayCommand(ExCmdReqSxxXx, CanCmdReqSxxXx, true)); }
        }


        private void ExCmdReqSxxXx()
        {
            ReqSxxInfo(true);

        }


        private bool CanCmdReqSxxXx()
        {
            return DateTime.Now.Ticks - dtde.Ticks > 30000000;
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
                    AreaName.Add(new AreaInt() { Value = area, Key = t.Value.AreaId });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = area, Key = t });
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
                    //if (isViewActive)
                    //{
                    //    var flg = WlstMessageBox.Show("确认", "请确认是否已保存当前区域上下限信息！", WlstMessageBoxType.YesNo);
                    //    if (flg == WlstMessageBoxResults.No)return;
                    //}
                    _areacomboboxselected = value;
                    this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
                    if (value == null) return;
                    AreaId = value.Key;
                    Items.Clear();

                    var ntsg = (from t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                                where t.Value.EquipmentType == WjParaBase.EquType.Rtu 
                                orderby t.Key ascending
                                select t).ToList();

                    var areatml = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[AreaId].LstTml;

                    foreach (var f in ntsg)
                    {
                        if (!areatml.Contains(f.Key)) continue;

                        var amps = f.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                        if (amps == null) continue;

                        var ordntgs =
                           (from g in amps.WjLoops orderby g.Value.LoopId select g.Value).ToList();
                        var child = new ObservableCollection<ObservableObjectCollection>();
                        foreach (var g in ordntgs)
                        {
                            if (g.CurrentRange == 0) continue;
                            child.Add(new ObservableObjectCollection(6, 5)
                            {
                                ValueString0 = amps.RtuPhyId.ToString("D4"),
                                ValueString1 = f.Value.RtuName,
                                ValueString2 = g.CurrentRange == 0 ? "屏蔽" : "使用",
                                //回路使用状态

                                ValueString3 = "--",
                                //回路电量平均值
                                ValueString4 = "--",
                                //回路电量平均值


                                ValueInt0 = f.Key,
                                //终端地址
                                ValueInt1 = g.LoopId,
                                //回路地址
                                ValueInt2 = g.CurrentAlarmLowerlimit,
                                //下限
                                ValueInt3 = g.CurrentAlarmUpperlimit,
                                //上限


                                ValueInt4 = g.CurrentAlarmLowerlimit,
                                //下限
                                ValueInt5 = g.CurrentAlarmUpperlimit,
                                //上限

                                IsFather = Visibility.Visible,
                                Rtuid = amps.RtuId
                            });
                        }

                        this.Items.Add(new ObservableObjectCollection(6, 5)
                        {
                            ValueString0 = amps.RtuPhyId.ToString("D4"),
                            ValueString1 = f.Value.RtuName,

                            ValueInt0 = f.Key,
                            //终端地址
                            IsFather = Visibility.Hidden,
                            Rtuid = amps.RtuId
                        }); 
                        foreach (var a in child)
                        {
                            Items.Last().ChildTreeItems.Add(a);
                        }

                        
                        for (int i = Items.Count - 1; i >= 0; i--)
                        {
                            if (Items[i].ChildTreeItems.Count == 0) Items.RemoveAt(i);
                        }


                    }

                    cancmdupdatecurrent = false;
                    ReqRulesInfo();
                    ReqSxxInfo(false);
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

        private int _twostart;
        public int TwoStart
        {
            get { return _twostart; }
            set
            {
                if (value == _twostart) return;
                if (value > 1440 || value <= 0) value = 1500;
                _twostart = value;
                this.RaisePropertyChanged(() => this.TwoStart);
            }
        }

        private int _twoend;
        public int TwoEnd
        {
            get { return _twoend; }
            set
            {
                if (value == _twoend) return;
                if (value > 1440 || value <= 0) value = 1500;
                _twoend = value;
                this.RaisePropertyChanged(() => this.TwoEnd);
            }
        }

        private int _twostart2;
        public int TwoStart2
        {
            get { return _twostart2; }
            set
            {
                if (value == _twostart2) return;
                if (value > 1440 || value <= 0) value = 1500;
                _twostart2 = value;
                this.RaisePropertyChanged(() => this.TwoStart2);
            }
        }

        private int _twoend2;
        public int TwoEnd2
        {
            get { return _twoend2; }
            set
            {
                if (value == _twoend2) return;
                if (value > 1440 || value <= 0) value = 1500;
                _twoend2 = value;
                this.RaisePropertyChanged(() => this.TwoEnd2);
            }
        }

        private Visibility _twovisi;
        public Visibility TwoVisi
        {
            get { return _twovisi; }
            set
            {
                if (value == _twovisi) return;
                _twovisi = value;
                this.RaisePropertyChanged(() => this.TwoVisi);
            }
        }
        private int _twovisiint1;
        public int TwoVisiInt1
        {
            get { return _twovisiint1; }
            set
            {
                if (value == _twovisiint1) return;
                _twovisiint1 = value;
                this.RaisePropertyChanged(() => this.TwoVisiInt1);
            }
        }
        private int _twovisiint2;
        public int TwoVisiInt2
        {
            get { return _twovisiint2; }
            set
            {
                if (value == _twovisiint2) return;
                _twovisiint2 = value;
                this.RaisePropertyChanged(() => this.TwoVisiInt2);
            }
        }


        private ObservableCollection<Wlst.Cr.CoreOne.Models.ObservableObjectCollection> _items;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.ObservableObjectCollection> Items
        {
            get
            {
                if (_items == null) _items = new ObservableCollection<ObservableObjectCollection>();
                return _items;
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
                Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set,// .wlst_svr_ans_cnt_wj3090_request_ldl_loop_current_avg,//.ClientPart.wlst_Measures_server_ans_clinet_rqup_loop_sxx,
                RequestOrUpdateFaultTypeInfo,
                typeof(RtuAmpSxxViewModel), this);
        }


        private void RequestOrUpdateFaultTypeInfo(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (isViewActive == false) return;
            cancmdupdatecurrent = false;
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
                            TwoStart = 1500;
                            TwoEnd = 1500;
                            TwoStart2 = 1500;
                            TwoEnd2 = 1500;
                            TwoVisi = Visibility.Hidden;
                            TwoVisiInt1 = 150;
                            TwoVisiInt2 = 0;
                        }

                        if (f.OpArguStart.Count == 0 || (f.OpArguStart.Count == 1 && f.OpArguEnd[0] == f.OpArguStart[0])
                            || (f.OpArguStart.Count == 2 && f.OpArguEnd[0] == f.OpArguStart[0] && f.OpArguEnd[1] == f.OpArguStart[1]))
                        {
                            TwoStart = 1500;
                            TwoEnd = 1500;
                            TwoStart2 = 1500;
                            TwoEnd2 = 1500;
                            TwoVisi = Visibility.Hidden;
                            TwoVisiInt1 = 150;
                            TwoVisiInt2 = 0;
                        }
                        else if (f.OpArguStart.Count == 1)
                        {
                            TwoStart = f.OpArguStart[0];
                            TwoEnd = f.OpArguEnd[0];
                            TwoStart2 = 1500;
                            TwoEnd2 = 1500;
                            TwoVisi = Visibility.Visible;
                            TwoVisiInt1 = 75;
                            TwoVisiInt2 = 75;
                        }
                        else if (f.OpArguStart.Count > 1)
                        {
                            TwoStart = f.OpArguStart[0];
                            TwoEnd = f.OpArguEnd[0];
                            TwoStart2 = f.OpArguStart[1];
                            TwoEnd2 = f.OpArguEnd[1];
                            TwoVisi = Visibility.Visible;
                            TwoVisiInt1 = 75;
                            TwoVisiInt2 = 75;
                        }

                    }
                }

                if (data.Op == 1) Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 服务器返回平均电流计算规则.";
                else Remark = Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  平均电流计算规则更新，更新后的上下限信息将在客户端下次登录后更新...";

                CurrentSelectRule = ItemsRules.First();
            }
            else if (data.Op == 3)
            {
                var dirc = new Dictionary<Tuple<int, int>, double>();
                var dirc2 = new Dictionary<Tuple<int, int>, double>();
                foreach (var f in data.LoopAvgItems)
                {
                    var tu = new Tuple<int, int>(f.RtuId, f.LoopId);
                    if (dirc.ContainsKey(tu)) continue;
                    dirc.Add(tu, f.Avg);

                    if (f.Avg2 > 0)
                    {
                        if (dirc2.ContainsKey(tu)) continue;
                        dirc2.Add(tu, f.Avg2);
                    }
                }
                //ValueString2 = g.Range == 0 ? "屏蔽" : "使用", //回路使用状态
                //                           ValueString3 = "--",//回路电量平均值
                //                           ValueInt0 = f.Key, //终端地址
                //                           ValueInt1 = g.LoopId,//回路地址


                foreach (var f in this.Items)
                {
                    //if (f.ValueString2.Contains("屏蔽")) continue;
                    var flg = false;
                    var flg2 = false;
                    foreach (var t in f.ChildTreeItems)
                    {
                        if (t.IsChecked)
                        {
                            var tu = new Tuple<int, int>(t.Rtuid, t.ValueInt1);
                            if (dirc.ContainsKey(tu))
                            {
                                t.ValueString3 = dirc[tu].ToString("f2");
                                flg = true;
                                cancmdupdatecurrent = true;
                            }
                            else
                            {
                                t.ValueString3 = "0.0";
                                cancmdupdatecurrent = true;
                            }
                            if (dirc2.ContainsKey(tu))
                            {
                                t.ValueString4 = dirc2[tu].ToString("f2");
                                flg2 = true;
                            }
                            else
                            {
                                t.ValueString4 = "0.0";
                            }
                            
                        }
                    }
                    if (flg) f.ValueString3 = "√";
                    if (flg2) f.ValueString4 = "√";
                }
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 请求回路平均电流成功.";
            }
            else if (data.Op == 14 )
            {
                var ntg = data.SxxItems;
                //foreach (var t in ntg)
                //{
                    
                //}

                var dirc = new Dictionary<Tuple<int, int>, Tuple<double,int, int>>();
                var dirc2 = new Dictionary<Tuple<int, int>, Tuple<double, int, int>>();
                foreach (var f in data.SxxItems)
                {
                    var tu = new Tuple<int, int>(f.RtuId, f.LoopId);
                    if (dirc.ContainsKey(tu)) continue;
                    var tu2 = new Tuple<double, int, int>(f.Avg, f.SxDefault, f.XxDefault);
                    dirc.Add(tu,tu2 );

                    if (f.Avg2 > 0)
                    {
                        if (dirc2.ContainsKey(tu)) continue;
                        var tu3 = new Tuple<double, int, int>(f.Avg2, f.Sx1, f.Xx1);
                        dirc2.Add(tu, tu3);
                    }
                }

                foreach (var f in this.Items)
                {
                    //if (f.ValueString2.Contains("屏蔽")) continue;
                    var flg = false;
                    var flg2 = false;
                    foreach (var t in f.ChildTreeItems)
                    {
                        var tu = new Tuple<int, int>(t.Rtuid, t.ValueInt1);
                        if (dirc.ContainsKey(tu))
                        {
                            t.ValueString3 = dirc[tu].Item1.ToString("f2");
                            t.ValueInt3 = dirc[tu].Item2;
                            t.ValueInt2 = dirc[tu].Item3;
                            flg = true;
                            cancmdupdatecurrent = true;
                        }
                        if (dirc2.ContainsKey(tu))
                        {
                            t.ValueString4 = dirc2[tu].Item1.ToString("f2");
                            t.ValueInt5 = dirc2[tu].Item2;
                            t.ValueInt4 = dirc2[tu].Item3;
                            flg2 = true;
                        }
                    }
                    if (flg) f.ValueString3 = "√";
                    if (flg2) f.ValueString4 = "√";
                }
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 终端平均电流、上下限信息保存成功.";
            }
            else if (data.Op == 4)
            {
                var dirc = new Dictionary<Tuple<int, int>, Tuple<double,int, int>>();
                var dirc2 = new Dictionary<Tuple<int, int>, Tuple<double, int, int>>();
                foreach (var f in data.SxxItems)
                {
                    var tu = new Tuple<int, int>(f.RtuId, f.LoopId);
                    if (dirc.ContainsKey(tu)) continue;
                    var tu2 = new Tuple<double, int, int>(f.Avg, f.SxDefault, f.XxDefault);
                    dirc.Add(tu,tu2 );

                    if (f.Avg2 > 0)
                    {
                        if (dirc2.ContainsKey(tu)) continue;
                        var tu3 = new Tuple<double, int, int>(f.Avg2, f.Sx1, f.Xx1);
                        dirc2.Add(tu, tu3);
                    }
                }

                foreach (var f in this.Items)
                {
                    foreach (var t in f.ChildTreeItems)
                    {
                        var tu = new Tuple<int, int>(t.Rtuid, t.ValueInt1);
                        if (dirc.ContainsKey(tu))
                        {
                            //t.ValueString3 = dirc[tu].Item1.ToString("f2");
                            t.ValueInt3 = dirc[tu].Item2;
                            t.ValueInt2 = dirc[tu].Item3;
                            cancmdupdatecurrent = true;
                        }
                        if (dirc2.ContainsKey(tu))
                        {
                            //t.ValueString4 = dirc2[tu].Item1.ToString("f2");
                            t.ValueInt5 = dirc2[tu].Item2;
                            t.ValueInt4 = dirc2[tu].Item3;
                        }
                    }
                }
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 终端上下限信息请求成功.";
            }


        }
    }

    public partial class RtuAmpSxxViewModel
    {
        private void ReqAvgInfo()
        {
            var info = Wlst.Sr.ProtocolPhone .LxRtu .wst_ldl_sxx_avg_set ;// .wlst_cnt_wj3090_request_ldl_loop_current_avg ;//.ServerPart.wlst_Measures_clinet_rqup_loop_sxx;
            //info.WstCntRequestLdlLoopSxx .CmdInfo = 3;

            foreach (var t in Items)
            {
                foreach (var f in t.ChildTreeItems)
                {
                    if (f.IsChecked)
                    {
                        if (!info.WstRtuLdlSxxAvgSet.RtuIds.Contains(f.Rtuid))
                        {
                            info.WstRtuLdlSxxAvgSet.RtuIds.Add(f.Rtuid);
                        }
                    }
                }
            }

            if (TwoStart!=1500 && TwoEnd!=1500)
            {
                info.WstRtuLdlSxxAvgSet.OpArguStart.Add(TwoStart);
                info.WstRtuLdlSxxAvgSet.OpArguEnd.Add(TwoEnd);
            }
            if (TwoStart2 != 1500 && TwoEnd2 != 1500)
            {
                info.WstRtuLdlSxxAvgSet.OpArguStart.Add(TwoStart2);
                info.WstRtuLdlSxxAvgSet.OpArguEnd.Add(TwoEnd2);
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
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set;//.ServerListen.wlst_cnt_wj3090_request_ldl_loop_current_avg;
            info.WstRtuLdlSxxAvgSet.Op = 14;
            foreach (var t in Items)
            {
                foreach (var f in t.ChildTreeItems)
                {
                    if (f.IsChecked)
                    {
                        var sx1 = f.ValueInt3;
                        var xx1 = f.ValueInt2;
                        var sx2 = f.ValueInt5;
                        var xx2 = f.ValueInt4;
                        double avg = 0;
                        double avg2 = 0;

                        if (sx1 <= xx1 || (sx2 <= xx2 && ((TwoStart != 1500 && TwoEnd != 1500 )||( TwoStart2 != 1500 && TwoEnd2 != 1500))))
                        {
                            WlstMessageBox.Show("上下限保存失败","终端 " + f.Rtuid + " 上下限设置有误！", WlstMessageBoxType.Ok);
                            return;
                        }

                        if (double.TryParse(f.ValueString3, out avg)) avg = double.Parse(f.ValueString3);
                        else avg = 0.0;
                        if (double.TryParse(f.ValueString4, out avg2)) avg2 = double.Parse(f.ValueString4);
                        else avg2 = 0.0;


                        if (f.ValueString3 != "--" || f.ValueString4 != "--")
                        {


                            info.WstRtuLdlSxxAvgSet.SxxItems.Add(new RtuSets.RtuLoopSxx()
                                                                  {
                                                                      RtuId = f.Rtuid,
                                                                      LoopId = f.ValueInt1,
                                                                      SxDefault = sx1,
                                                                      XxDefault = xx1,
                                                                      Sx1 = sx2,
                                                                      Xx1 = xx2,
                                                                      Avg = avg,
                                                                      Avg2 = avg2
                                                                  });
                        }
                    }
                }
            }




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

            if (TwoStart != 1500 && TwoEnd != 1500)
            {
                start.Add(TwoStart);
                end.Add(TwoEnd);
            }
            if (TwoStart2 != 1500 && TwoEnd2 != 1500)
            {
                start.Add(TwoStart2);
                end.Add(TwoEnd2);
            }
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

        private void ReqSxxInfo(bool ischeck)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set;//.ServerListen.wlst_cnt_wj3090_request_ldl_loop_current_avg;

            if (ischeck)
            {
                foreach (var t in Items)
                {
                    foreach (var f in t.ChildTreeItems)
                    {
                        if (f.IsChecked)
                        {
                            if (!info.WstRtuLdlSxxAvgSet.RtuIds.Contains(f.Rtuid))
                            {
                                info.WstRtuLdlSxxAvgSet.RtuIds.Add(f.Rtuid);
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (var t in Items)
                {
                    foreach (var f in t.ChildTreeItems)
                    {
                        if (!info.WstRtuLdlSxxAvgSet.RtuIds.Contains(f.Rtuid))
                        {
                            info.WstRtuLdlSxxAvgSet.RtuIds.Add(f.Rtuid);
                        }
                    }
                }
            }

            if (info.WstRtuLdlSxxAvgSet.RtuIds.Count>0)
            {
                info.WstRtuLdlSxxAvgSet.Op = 4;
                SndOrderServer.OrderSnd(info, 10, 2);
            }
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
