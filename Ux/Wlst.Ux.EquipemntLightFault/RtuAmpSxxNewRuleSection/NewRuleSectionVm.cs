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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNewOne;
using Wlst.client;

namespace Wlst.Ux.EquipemntLightFault.RtuAmpSxxNewRuleSection
{
    [Export(typeof (IINewRuleSectionvm))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewRuleSectionVm : Wlst.Cr.Core.CoreServices.ObservableObject, IINewRuleSectionvm
    {

        internal static NewRuleSectionVm MySelf; 
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



        public NewRuleSectionVm()
        {
            this.InitAction();
            MySelf = this;
        }


        private bool isViewActive = false;

        public void NavInitBeforShow(params object[] parsObjects)
        {
            Visi = Visibility.Hidden;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            isViewActive = true;


            AreaName.Clear();
            getAreaRId();
            if (AreaName.Count > 0) AreaComboBoxSelected = AreaName.First();
            if (AreaName.Count > 1) Visi = Visibility.Visible;
            else Visi = Visibility.Collapsed;

            IsEnableFirst = CurRuleItem != null;

            Remark = "操作步骤:1、规则调整,2、增加或选中方案,3、方案基本信息,4、选择终端,5、设定请求数据的时间,6、请求并计算上下限，7、保存";
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
            get { return "高级终端回路上下限设置"; }
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
                    AreaName.Add(new RtuAmpSxxViewModel.AreaInt()
                                     {Value = t.Value.AreaId.ToString("d2") + "-" + area, Key = t.Value.AreaId});
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new RtuAmpSxxViewModel.AreaInt() {Value = t.ToString("d2") + "-" + area, Key = t});
                    }
                }
            }


        }

        private static ObservableCollection<RtuAmpSxxViewModel.AreaInt> _devices;

        public static ObservableCollection<RtuAmpSxxViewModel.AreaInt> AreaName
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<RtuAmpSxxViewModel.AreaInt>();
                }
                return _devices;
            }

        }

        private RtuAmpSxxViewModel.AreaInt _areacomboboxselected;
        private int AreaId;

        public RtuAmpSxxViewModel.AreaInt AreaComboBoxSelected
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
                    OnAreaSelectedChanged();

                }
            }
        }

        #endregion

        private void OnAreaSelectedChanged()
        {
            LoadRtu();
            UpdateRules();
            InitLoadRuleItem(AreaId);
        }

        private void LoadRtu()
        {

            TmlItem.Clear();

            //var loopusedd = 0;
            //var loopnousedd = 0;
            //var tmlsum = 0;
            var tmllist = new List<int>();

            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
            {
                if (t.Key.Item1 == AreaId)
                {
                    var child = new ObservableCollection<RtuAmpSxxViewModel.TmlItems>();
                    foreach (var tt in t.Value.LstTml)
                    {
                        if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(tt))
                        {
                            var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tt];
                            if (tml.EquipmentType == WjParaBase.EquType.Rtu)
                            {
                               // tmlsum++;
                               // var loops = tml as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                               // if (loops == null) continue;

                               //// int loopused = 0, loopnoused = 0;
                               // foreach (var g in loops.WjLoops)
                               // {
                               //     if (g.Value.IsShieldLoop != 1)
                               //     {
                               //       //  loopused++;
                               //         //loopusedd++;
                               //     }
                               //     else
                               //     {
                               //        // loopnoused++;
                               //        // loopnousedd++;
                               //     }
                               // }

                                tmllist.Add(tml.RtuId);
                                child.Add(new RtuAmpSxxViewModel.TmlItems()
                                              {
                                                  Id = tml.RtuPhyId,
                                                  RtuId = tml.RtuId,
                                                  Name = tml.RtuName,
                                                  IsFather = false,
                                                  IsSelect = false,
                                                  //LoopNoUsed = loopnoused,
                                                  //LoopUsed = loopused
                                              });
                            }
                        }
                    }
                    if (t.Value.LstTml.Count > 0)
                    {
                        TmlItem.Add(new RtuAmpSxxViewModel.TmlItems()
                                        {
                                            Id = t.Value.GroupId,
                                            Name = t.Value.GroupName,
                                            IsFather = true,
                                            IsSelect = false ,
                                            Child = child
                                        });
                    }
                }
            }
            var rtulst = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);

            if (rtulst.Count > 0)
            {
                var child1 = new ObservableCollection<RtuAmpSxxViewModel.TmlItems>();
                foreach (var tt in rtulst)
                {
                    if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(tt))
                    {
                        var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tt];
                        if (tml.EquipmentType == WjParaBase.EquType.Rtu)
                        {
                            //tmlsum++;
                            //var loops = tml as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                            //if (loops == null) continue;

                            //int loopused = 0, loopnoused = 0;
                            //foreach (var g in loops.WjLoops)
                            //{
                            //    if (g.Value.IsShieldLoop != 1)
                            //    {
                            //       // loopused++;
                            //       // loopusedd++;
                            //    }
                            //    else
                            //    {
                            //       // loopnoused++;
                            //        //loopnousedd++;
                            //    }
                            //}

                            tmllist.Add(tml.RtuId);
                            child1.Add(new RtuAmpSxxViewModel.TmlItems()
                                           {
                                               Id = tml.RtuPhyId,
                                               RtuId = tml.RtuId,
                                               Name = tml.RtuName,
                                               IsFather = false,
                                               IsSelect = false ,
                                               //LoopNoUsed = loopnoused,
                                               //LoopUsed = loopused
                                           });
                        }
                    }
                }
                TmlItem.Add(new RtuAmpSxxViewModel.TmlItems()
                                {
                                    Id = -1,
                                    Name = "未分组终端",
                                    IsFather = true,
                                    IsSelect = false ,
                                    Child = child1
                                });


            }

            //SelectTmlNoUsed = loopnousedd;
            //SelectTmlUsed = loopusedd;
            //SelectTmlSum = tmlsum;
        }

        #region  rule

        private ObservableCollection<RultItem> _itemItemsRuless;

        public ObservableCollection<RultItem> ItemsRules
        {
            get
            {
                if (_itemItemsRuless == null) _itemItemsRuless = new ObservableCollection<RultItem>();
                return _itemItemsRuless;
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

        private int xxAlowBak;
        private int xxAmaxBak;
        private double xxLowTimesBak;
        private double xxMaxTimesBak;
        private int xxIndex;

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

        private void UpdateRules()
        {
            this.ItemsRules.Clear();
            if (Wlst.Sr.EquipmentInfoHolding.Services.RtuSxxRuleInstancesHold.Myself.Rules.ContainsKey(AreaId))
            {
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.RtuSxxRuleInstancesHold.Myself.Rules[AreaId])
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

        #endregion



    }

    public partial class NewRuleSectionVm
    {
        #region CmdAddRule1

        private ICommand _cCmdCmdAddRule1;

        public ICommand CmdAddRule1
        {
            get { return _cCmdCmdAddRule1 ?? (_cCmdCmdAddRule1 = new RelayCommand(ExCmdAddRule1, CanCmdAddRule1, true)); }
        }


        private void ExCmdAddRule1()
        {
             
            if (this.ItemsRules.Count > 1)
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

       
        private bool CanCmdAddRule1()
        {
            return true;
        }

        #endregion

        #region CmdDeleteRule1

        private ICommand _cCmdDeleteRules1;

        public ICommand CmdDeleteRule1
        {
            get
            {
                return _cCmdDeleteRules1 ??
                       (_cCmdDeleteRules1 = new RelayCommand(ExCmdDeleteRules1, CanCmdDeleteRules1, true));
            }
        }


        private void ExCmdDeleteRules1()
        {

            
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

        

        private bool CanCmdDeleteRules1()
        {
            return   ItemsRules.Count > 1;
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
                    WlstMessageBox.Show("非法设置", "规则重复！", WlstMessageBoxType.Ok);
                    return;
                }
                if (max1 < t.Amax && Index != t.Index) max1 = t.Amax;
            }
            if (ItemsRules.Count > 0 && ItemsRules.Last().Amax != 2000 && max1 < ItemsRules.Last().Amax) max1 = ItemsRules.Last().Amax;

            if (CurrentSelectRule == ItemsRules[0] && AlowBak != 0)
            {
                WlstMessageBox.Show("非法设置", "第一段规则电流下限必须等于0！", WlstMessageBoxType.Ok);
                return;
            }
            else if (AlowBak < 0)
            {
                WlstMessageBox.Show("非法设置", "电流下限小于0！", WlstMessageBoxType.Ok);
                return;

            }
            else if (AlowBak >= AmaxBak)
            {
                WlstMessageBox.Show("非法设置", "电流下限大于等于电流上限！", WlstMessageBoxType.Ok);
                return;

            }
            else if (LowTimesBak > 1)
            {
                WlstMessageBox.Show("非法设置", "下限系数大于1", WlstMessageBoxType.Ok);
                return;

            }
            else if (MaxTimesBak < 1)
            {
                WlstMessageBox.Show("非法设置", "上限系数小于1", WlstMessageBoxType.Ok);
                return;

            }
            else if (AmaxBak > 1000)
            {
                WlstMessageBox.Show("非法设置", "电流上限大于1000", WlstMessageBoxType.Ok);
                return;
            }
            else if (max1 != 1000)
            {
                WlstMessageBox.Show("非法设置", "最后段规则电流最大上限必须等于1000！", WlstMessageBoxType.Ok);
                return;
            }
            else if (CurrentSelectRule.Amax == 1000 && AmaxBak != 1000)
            {
                WlstMessageBox.Show("非法设置", "最后段规则电流最大上限必须等于1000！", WlstMessageBoxType.Ok);
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

    }

    //rules
    public partial class NewRuleSectionVm
    {
        private bool  _vissdfsdfi;

        public bool IsEnableFirst
        {
            get { return _vissdfsdfi; }
            set
            {
                if (value == _vissdfsdfi) return;
                _vissdfsdfi = value;
                this.RaisePropertyChanged(() => this.IsEnableFirst);
            }
        }


        private void InitLoadRuleItem(int areaid)
        {
            this.RuleItems.Clear();
            var ntg = (from t in Wlst.Sr.EquipmentInfoHolding.Services.RtuSxxRuleInstancesHold.Myself.InfoItems
                       where t.Key.Item1 == areaid
                       orderby t.Key.Item2 ascending
                       select t.Value).ToList();
            foreach (var f in ntg)
            {
                this.RuleItems.Add(new OneRuleItem(f));
            }
            if (RuleItems.Count > 0)
            {
                CurRuleItem = RuleItems[0];
            }else
            {
                SelectRtu(new List<int>());
            }
        }



        private ObservableCollection<OneRuleItem> _devicsdfdsesitem;

        public ObservableCollection<OneRuleItem> RuleItems
        {
            get
            {
                if (_devicsdfdsesitem == null)
                {
                    _devicsdfdsesitem = new ObservableCollection<OneRuleItem>();
                }
                return _devicsdfdsesitem;
            }
            set
            {
                if (value != _devicsdfdsesitem)
                {
                    _devicsdfdsesitem = value;
                    this.RaisePropertyChanged(() => this.RuleItems);
                }
            }
        }

        private OneRuleItem remCurRuleItemak;

        public OneRuleItem CurRuleItem
        {
            get { return remCurRuleItemak; }
            set
            {
                if (value == remCurRuleItemak) return;
                //if(remCurRuleItemak !=null )
                //{
                //    OnSelectRuleItemChangingSave(remCurRuleItemak);
                //}

                remCurRuleItemak = value;
                this.RaisePropertyChanged(() => this.CurRuleItem);
                OnSelectRuleItemChanged(value);
                IsEnableFirst = value != null;
            }
        }

        private void OnSelectRuleItemChanged(OneRuleItem date)
        {
            foreach (var f in TmlItem) AllSelected(f, false);
            if (date == null) return;
            var ntgs = (from t in date.SelectedRtus select t.RtuId).Distinct().ToList();
            SelectRtu(ntgs);
        }

        private void AllSelected(RtuAmpSxxViewModel.TmlItems data, bool isAllsel)
        {
            foreach (var f in data.Child)
            {
                f.IsSelect = isAllsel;
            }
            data.IsSelect = isAllsel;
        }

        private void SelectRtu(List<int> rtus)
        {
            //foreach (var f in TmlItem) AllSelected(f, false);
            foreach (var f in TmlItem)
            {
                f.IsSelect = true;

                int allcount = 0;
                int selcount = 0;
                foreach (var g in f.Child)
                {
                    allcount++;
                    if (rtus.Contains(g.RtuId))
                    {
                        selcount++;
                        g.IsSelect = true;
                    }
                    else
                    {
                        g.IsSelect = false;
                    }
                }
                if (selcount == 0)
                {
                    f.IsSelect = false;
                    f.AttachInfo = "--";
                }
                else if (selcount == allcount) f.AttachInfo = "全部";
                else f.AttachInfo = selcount + "/" + allcount;
            }
            //foreach (var f in TmlItem)
            //{

            //    bool someNotSelc = (from t in f.Child where t.IsSelect == false select t).Count() > 0;
            //    bool allcount = f.Child.Count > 0;
            //    if (someNotSelc == false && allcount) f.IsSelect = true;
            //}
        }


        #region CmdAddRule

        private ICommand _cCmdCmdAddRule;

        public ICommand CmdAddRule
        {
            get { return _cCmdCmdAddRule ?? (_cCmdCmdAddRule = new RelayCommand(ExCmdAddRule, CanCmdAddRule, true)); }
        }


        private void ExCmdAddRule()
        {
            dt = DateTime.Now;
            int max = 1;
            foreach (var f in RuleItems)
            {
                if (f.Id >= max) max += 1;
            }

            this.RuleItems.Add(new OneRuleItem(AreaId, max));
            CurRuleItem = this.RuleItems.Last();
        }

        private DateTime dt = DateTime.Now.AddDays(-1);

        private bool CanCmdAddRule()
        {
            return DateTime.Now.Ticks - dt.Ticks > 10000000;
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
            if (CurRuleItem != null && RuleItems.Contains(CurRuleItem))
            {
                RuleItems.Remove(CurRuleItem);
            }
            if (RuleItems.Count > 0)
            {
                CurRuleItem = RuleItems[0];
            }
            else
            {
                CurRuleItem = null;
            }
        }

        private DateTime dtde = DateTime.Now.AddDays(-1);

        private bool CanCmdDeleteRules()
        {
            return DateTime.Now.Ticks - dtde.Ticks > 10000000 && CurRuleItem !=null ;
        }

        #endregion


        #region CmdSave

        private ICommand _cCmdCmdAddRuleSave;

        public ICommand CmdSave
        {
            get { return _cCmdCmdAddRuleSave ?? (_cCmdCmdAddRuleSave = new RelayCommand(Save, CanCmdSave, true)); }
        }


        private void Save()
        {
            dtCanCmdSave = DateTime.Now;
            var ntg = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new;
            ntg.WstRtuLdlSxxAvgSetNew.Op = 14;
            ntg.WstRtuLdlSxxAvgSetNew.AreaId = AreaId;
            foreach (var f in RuleItems)
            {

                var tmp = f.BackTo();
                if(tmp .OpTimeStart ==1 && tmp .OpTimeEnd ==1 && tmp .TimeStart > tmp .TimeEnd )
                {
                    WlstMessageBox.Show("非法设置", "方案"+tmp .SectionId +"-"+tmp .SectionName +" 报警结束时间小于报警开始时间!!!", WlstMessageBoxType.Ok);
                    return;
                }
                //ntg.WstRtuLdlSxxAvgSetNew.SxxItems.AddRange(f.SelectedRtus);

                ntg.WstRtuLdlSxxAvgSetNew.SxxItems.Add(f.BackTo());
                //foreach(var vx in f.SelectedRtus )
                //{
                //          ntg.WstRtuLdlSxxAvgSetNew.SxxItems .Add( new RtuSetsNew.RtuLoopSxxSectionInfo()
                //                                           {

                //                                           })
                //}
            }
            // info.WstRtuLdlSxxAvgSet.SxxItems = sxxitem;
            SndOrderServer.OrderSnd(ntg, 10, 4);
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 更新已经提交，修改后的上下限信息将在客户端下次登录后更新...";
        }

        private DateTime dtCanCmdSave = DateTime.Now.AddDays(-1);

        private bool CanCmdSave()
        {
            return DateTime.Now.Ticks - dtCanCmdSave.Ticks > 10000000;
        }

        #endregion

        #region CmdReqAvga

        private ICommand _cCmdCmdReqAvga;

        public ICommand CmdReqAvga
        {
            get { return _cCmdCmdReqAvga ?? (_cCmdCmdReqAvga = new RelayCommand(CmdExCmdReqAvga, CanCmdReqAvga, true)); }
        }


        private void CmdExCmdReqAvga()
        {
            if (CurRuleItem == null) return;
            long dif = CurRuleItem.DtReq2.Ticks - CurRuleItem.DtReq1.Ticks;
            if(dif <0)
            {
                WlstMessageBox.Show("非法设置", "请求数据起始时间大于结束时间！", WlstMessageBoxType.Ok);
                return;
            }
            if (dif < 30*60*10000000L)
            {
                WlstMessageBox.Show("错误", "请求数据的起始时间至结束时间小于半小时，可能无终端数据！", WlstMessageBoxType.Ok);
                return;
            }
            if (dif > 360 * 60 * 10000000L)
            {
                WlstMessageBox.Show("错误", "请求数据的起始时间至结束时间大于6小时，单段时间太长！", WlstMessageBoxType.Ok);
                return;
            }

            dtCanCmdReqAvga = DateTime.Now;
            var ntg = Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new;
            ntg.WstRtuLdlSxxAvgSetNew.Op = 3;
            ntg.WstRtuLdlSxxAvgSetNew.AreaId = AreaId;
            ntg.WstRtuLdlSxxAvgSetNew.LoopAvgdata.RtuIds = new List<int>();
            foreach (var f in TmlItem )
            {
                ntg.WstRtuLdlSxxAvgSetNew.LoopAvgdata .RtuIds.AddRange(
                    (from t in f.Child where t.IsSelect select t.RtuId).ToList());
            }
            ntg.WstRtuLdlSxxAvgSetNew.LoopAvgdata .DtStart = CurRuleItem.DtReq1.Ticks;
            ntg.WstRtuLdlSxxAvgSetNew.LoopAvgdata.DtEnd = CurRuleItem.DtReq2.Ticks;
            SndOrderServer.OrderSnd(ntg, 10, 4);
            StrQingqiu = "正在请求平均电流...";
            Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在请求平均电流...";
        }

        private DateTime dtCanCmdReqAvga = DateTime.Now.AddDays(-1);

        private bool CanCmdReqAvga()
        {
            return DateTime.Now.Ticks - dtCanCmdReqAvga.Ticks > 10000000 && CurRuleItem != null;
        }

           private string IdNDtEndame;

        /// <summary>
        /// 报警结束时间
        /// </summary>
        public string StrQingqiu
        {
            get { return IdNDtEndame; }
            set
            {
                if (value == IdNDtEndame) return;
                 IdNDtEndame = value;

                 
                this.RaisePropertyChanged(() => this.StrQingqiu);  
            }
        }
         
        #endregion



    }


    public partial class NewRuleSectionVm
    {
        private ObservableCollection<RtuAmpSxxViewModel.TmlItems> _devicesitem;

        public ObservableCollection<RtuAmpSxxViewModel.TmlItems> TmlItem
        {
            get
            {
                if (_devicesitem == null)
                {
                    _devicesitem = new ObservableCollection<RtuAmpSxxViewModel.TmlItems>();
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
    }

    /// <summary>
    /// action
    /// </summary>
    public partial class NewRuleSectionVm
    {
        private string remak;

        public string Remark
        {
            get { return remak; }
            set
            {
                if (value == remak) return;
                remak = value;
                this.RaisePropertyChanged(() => this.Remark);
            }
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_ldl_sxx_avg_set_new,
                OnInitAction,
                typeof (NewRuleSectionVm), this);
        }


        private void OnInitAction(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (isViewActive == false) return;
            if (infos == null || infos.WstRtuLdlSxxAvgSetNew == null) return;

            var data = infos.WstRtuLdlSxxAvgSetNew;

            if (data.Op == 3)
            {
                StrQingqiu = "平均电流请求成功.";
                if (CurRuleItem == null) return;
                CurRuleItem.SelectedRtus.Clear();
                var ntgs = (from t in ItemsRules orderby t.Index ascending select t).ToList();

                foreach (var t in data.LoopAvgdata.ItemLoopdata)
                {

                    foreach (var f in ntgs)
                    {

                        if (f.Alow <= t.A && t.A <= f.Amax && t.A >= 0)
                        {


                            var tAMin = (int) (t.A*f.LowTimes);
                            var tamax = 0;

                            if (t.A*f.MaxTimes - (int) (t.A*f.MaxTimes) >= 0.5)
                                tamax = (int) (t.A*f.MaxTimes) + 1;
                            else
                                tamax = (int) (t.A*f.MaxTimes);

                            var tmp = new Wlst.client.RtuSetsNew.RtuLoopSxxSectionInfo.RtuLoopSxx()
                                          {
                                              Avg = t.A,
                                              LoopId = t.LoopId,
                                              RtuId = t.RtuId,
                                              SectionId = CurRuleItem.Id,
                                              Sx = tamax,
                                              Xx = tAMin,
                                          };
                            CurRuleItem.SelectedRtus.Add(tmp);
                        }
                    }
                }
                if(CurRuleItem !=null )
                {
                    var rtus = (from t in CurRuleItem.SelectedRtus select t.RtuId).Distinct().ToList();
                    CurRuleItem.RtuCount = rtus.Count();
                    SelectRtu(rtus);
                }

                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 请求回路平均电流并计算上下限成功";

            }
            else if (data.Op == 14)
            {
                Remark = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 终端平均电流、上下限信息保存（更新）成功.请重新打开界面";
            }
        }
    

 
    }
}
