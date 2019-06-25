using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlSettingViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlSettingViewModel.ViewModel
{

    [Export(typeof(IIEquipmentFaultWithTmlSettingViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultWithTmlSettingViewModel :
        Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, IIEquipmentFaultWithTmlSettingViewModel
    {

        public EquipmentFaultWithTmlSettingViewModel()
        {
            Visi = Visibility.Collapsed;
            InitEvent();
            FauleCodeNameSelectViewModel.OnSelectChanged += new EventHandler(FauleCodeNameSelectViewModel_OnSelectChanged);
        }


        private bool _isFauleCodeNameSelectViewModelSystemSelectd = false;
        private int _preUpdateId = -1;
        void FauleCodeNameSelectViewModel_OnSelectChanged(object sender, EventArgs e)
        {
            if (_isFauleCodeNameSelectViewModelSystemSelectd) return;
            //throw new NotImplementedException();
            //var vm = sender as FauleCodeNameSelectViewModel;
            //if (vm == null) return;

            int nodeid = -1;
            var lst = (from t in FaultCollection where t.IsSelected select t.FaultCode).ToList();

            if (IsOneKeySet)
            {
                nodeid = 0;
                _preUpdateId = 0;
                if (!ApplyFault.ContainsKey(0)) ApplyFault.Add(0, new List<int>());
                ApplyFault[0].Clear();
                ApplyFault[0].AddRange(lst);
                return;

            }
            else
            {
                if (CurrentSelectItem != null && CurrentSelectItem.IsGroup == false)
                    nodeid = CurrentSelectItem.NodeId;
                if (_preUpdateId == 0)
                {
                    FaultChecks();
                    UpdateRtuSelected();
                }
                _preUpdateId = nodeid;
                if (ApplyFault.ContainsKey(0))
                {

                    if (ApplyFault[0].Count == lst.Count)
                    {
                        bool same = true;
                        foreach (var g in ApplyFault[0])
                        {
                            if (!lst.Contains(g))
                            {
                                same = false;
                                break;
                            }
                        }
                        if (same)
                        {
                            if (ApplyFault.ContainsKey(nodeid))
                                ApplyFault.Remove(nodeid);
                            if (ListTreeTmlNode.Info.ContainsKey(nodeid))
                                ListTreeTmlNode.Info[nodeid].IsSelected = false;
                            return;
                        }
                    }
                }
                if (!ApplyFault.ContainsKey(nodeid)) ApplyFault.Add(nodeid, new List<int>());
                ApplyFault[nodeid].Clear();
                ApplyFault[nodeid].AddRange(lst);
                if (ListTreeTmlNode.Info.ContainsKey(nodeid))
                    ListTreeTmlNode.Info[nodeid].IsSelected = true;
            }
        }

        /// <summary>
        /// 应用的故障
        /// </summary>
        protected Dictionary<int, List<int>> ApplyFault = new Dictionary<int, List<int>>();

        public void NavOnLoad(params object[] parsObjects)
        {

            this.ItemsArea.Clear();
            foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
            {
                if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Contains(f.Key) == false) continue;
                this.ItemsArea.Add(new NameValueInt()
                                       {
                                           Name = f.Key.ToString("d2") + "-" + f.Value.AreaName,
                                           Value = f.Key
                                       });
            }
            if (ItemsArea.Count > 0) CurrentSlecteArea = ItemsArea[0];
            Visi = ItemsArea.Count > 1 ? Visibility.Visible : Visibility.Collapsed;

            this.InitFaultCollection();

            ApplyFault.Clear();
            var lstfaultshow = (from t in FaultCollection select t.FaultCode).ToList();
            foreach (var t in Wlst.Sr.EquipemntLightFault.Services.FaultBandtoTmlInfoSerices.InfoDictionary)
            {
                if (t.Key == 0) continue;
                ApplyFault.Add(t.Key, new List<int>());
                var lstssss = (from g in t.Value where lstfaultshow.Contains(g) select g).ToList();
                ApplyFault[t.Key].AddRange(lstssss);
            }
            if (!ApplyFault.ContainsKey(0))
            {
                ApplyFault.Add(0, new List<int>());
                foreach (var t in this.FaultCollection) ApplyFault[0].Add(t.FaultCode);
            }
            else
            {
                if (ApplyFault[0].Count == 0) foreach (var t in this.FaultCollection) ApplyFault[0].Add(t.FaultCode);
            }

            ListTreeTmlNode.Info.Clear();
            ListTreeGroupNode.Info.Clear();
            this.LoadNode();

            dtSnd = DateTime.Now.AddDays(-1);
            _dtOneKey = DateTime.Now.AddHours(-1);

            IsTreeEnable = true;
            IsOneKeySet = false;

            Msg = "通过选中左边树中需要设置报警故障的终端，在右侧选择报警故障即可.";
        }

        private void UpdateFaultsWhenUpdate()
        {
            ApplyFault.Clear();


            var lstfaultshow = (from t in FaultCollection select t.FaultCode).ToList();
            foreach (var t in Wlst.Sr.EquipemntLightFault.Services.FaultBandtoTmlInfoSerices.InfoDictionary)
            {
                ApplyFault.Add(t.Key, new List<int>());
                var lstssss = (from g in t.Value where lstfaultshow.Contains(g) select g).ToList();
                ApplyFault[t.Key].AddRange(lstssss);
            }
            if (!ApplyFault.ContainsKey(0))
            {
                ApplyFault.Add(0, new List<int>());
                foreach (var t in this.FaultCollection) ApplyFault[0].Add(t.FaultCode);
            }
            else
            {
                if (ApplyFault[0].Count == 0) foreach (var t in this.FaultCollection) ApplyFault[0].Add(t.FaultCode);
            }
            FaultChecks();
            UpdateRtuSelected();
            OnCurrentSelectItemChange();
        }


        public void OnUserHideOrClosing()
        {
            //throw new NotImplementedException();
            ApplyFault.Clear();
            ChildTreeItems = new ObservableCollection<ListTreeNodeBase>();
            FaultCollection = new ObservableCollection<FauleCodeNameSelectViewModel>();
            ListTreeTmlNode.Info.Clear();
            ListTreeGroupNode.Info.Clear();
        }

        private DateTime dtSnd;
        private string _msg;

        /// <summary>
        /// 显示信息
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value != _msg)
                {
                    _msg = value;
                    this.RaisePropertyChanged(() => Msg);
                }
            }
        }

        //加载终端节点
        private void LoadNode()
        {
            ChildTreeItems.Clear();
            if (CurrentSlecteArea == null) return;
            var groups = (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                          where t.Key.Item1 == CurrentSlecteArea.Value orderby t.Value .Index ascending 
                          select t.Value).ToList();
            if (groups.Count == 0)
            {
                Msg = CurrentSlecteArea.Name + "  未查询到分组信息...";
                return;
            }

    
            //正常分组

                foreach ( var t in groups )
                {
                    var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(CurrentSlecteArea.Value, t.GroupId);
                    if (grp == null) return;
                    var ordtml = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);
                    if(ordtml.Count >0)
                    {
                        foreach (var g in ordtml)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g))
                                continue;
                            if(Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g].EquipmentType ==
                    WjParaBase.EquType.Rtu)
                            {
                                this.ChildTreeItems.Add(new ListTreeGroupNode(CurrentSlecteArea.Value, t.GroupId));
                            }
                            break;
                        }
                    }
                        
                }
            var spe =
                Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(
                    CurrentSlecteArea.Value);
            if(spe .Count >0)
            {
                foreach (var g in spe)
                {
                    if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g))
                        continue;
                    if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g].EquipmentType ==
            WjParaBase.EquType.Rtu)
                    {
                        this.ChildTreeItems.Add(new ListTreeGroupNode(CurrentSlecteArea.Value, 0));
                    }
                    break;
                }
            } 
            
            

            FaultChecks();
            //更新选项 
            UpdateRtuSelected();
        }

        /// <summary>
        /// 对缓存的终端故障进行检测  如果终端故障与通用故障相同则删除 否则保留
        /// </summary>
        private void FaultChecks()
        {
            if (!ApplyFault.ContainsKey(0)) return;
            var lst0 = ApplyFault[0];
            var lstNods = (from t in ApplyFault where t.Key != 0 select t.Key).ToList();
            foreach (var t in lstNods)
            {
                bool same = true;
                if (ApplyFault[t].Count != lst0.Count) continue;
                foreach (var g in ApplyFault[t])
                    if (!lst0.Contains(g))
                    {
                        same = false;
                        break;
                    }
                if (same) ApplyFault.Remove(t);
            }
        }
        /// <summary>
        /// 根据缓存的故障信息 对终端列表Select进行设置
        /// </summary>
        private void UpdateRtuSelected()
        {
            var lst = ApplyFault.Keys.ToList();
            foreach (var t in ListTreeTmlNode.Info)
            {
                if (lst.Contains(t.Key)) t.Value.IsSelected = true;
                else t.Value.IsSelected = false;
            }

            foreach (var t in ChildTreeItems) if (t.IsGroup) t.UpdateNodeSelected();

        }

        private DateTime _dtSaveThisSet;

        #region CmdSaveThisSet 应用到终端

        //private ICommand _cmdSaveThisSet;

        //public ICommand CmdSaveThisSet
        //{
        //    get
        //    {
        //        if (_cmdSaveThisSet == null) _cmdSaveThisSet = new RelayCommand(ExSaveThisSet, CanSaveThisSet, true);
        //        return _cmdSaveThisSet;
        //    }
        //}

        //private bool CanSaveThisSet()
        //{
        //    if (IsOneKeySet) return true;
        //    if (CurrentSelectItem == null) return false;
        //    if (CurrentSelectItem.IsGroup) return false;
        //    return DateTime.Now.Ticks - _dtSaveThisSet.Ticks > 10000000;
        //}

        //private void ExSaveThisSet()
        //{
        //    _dtSaveThisSet = DateTime.Now;
        //    int nodeid = -1;
        //    var lst = (from t in FaultCollection where t.IsSelected select t.FaultCode).ToList();

        //    if (IsOneKeySet)
        //    {
        //        nodeid = 0;
        //    }
        //    else
        //    {
        //        if (CurrentSelectItem != null && CurrentSelectItem.IsGroup == false)
        //            nodeid = CurrentSelectItem.NodeId;
        //    }
        //    if (nodeid == -1) return;


        //    if (!ApplyFault.ContainsKey(nodeid)) ApplyFault.Add(nodeid, new List<int>());
        //    ApplyFault[nodeid].Clear();
        //    ApplyFault[nodeid].AddRange(lst);

        //    SpecialCheck(nodeid);
        //}


        //private void SpecialCheck(int nodid)
        //{
        //    if (nodid == 0)
        //    {
        //        if (!ApplyFault.ContainsKey(0)) return;
        //        var lst0 = ApplyFault[0];
        //        var lstsame = new List<int>();
        //        foreach (var t in ApplyFault)
        //        {
        //            if (t.Key == 0) continue;
        //            bool same = true;
        //            if (t.Value.Count != lst0.Count) continue;
        //            foreach (var g in t.Value)
        //                if (!lst0.Contains(g))
        //                {
        //                    same = false;
        //                    break;
        //                }
        //            if (same) lstsame.Add(t.Key);
        //        }
        //        foreach (var t in lstsame)
        //        {
        //            if (ApplyFault.ContainsKey(t)) ApplyFault.Remove(t);
        //            if (ListTreeTmlNode.Info.ContainsKey(t)) ListTreeTmlNode.Info[t].IsSelected = false;
        //        }
        //        return;
        //    }
        //    else
        //    {
        //        var lst0 = new List<int>();
        //        if (ApplyFault.ContainsKey(0))
        //            lst0 = ApplyFault[0];
        //        if (!ApplyFault.ContainsKey(nodid)) return;
        //        bool same = true;
        //        foreach (var t in ApplyFault[nodid])
        //        {
        //            if (!lst0.Contains(t))
        //            {
        //                same = false;
        //                break;
        //            }
        //        }
        //        if (same)
        //        {
        //            if (ListTreeTmlNode.Info.ContainsKey(nodid))
        //                ListTreeTmlNode.Info[nodid].IsSelected = false;
        //            if (ApplyFault.ContainsKey(nodid)) ApplyFault.Remove(nodid);

        //        }
        //        else
        //        {
        //            if (ListTreeTmlNode.Info.ContainsKey(nodid))
        //                ListTreeTmlNode.Info[nodid].IsSelected = true;
        //        }
        //    }
        //}


        #endregion

        #region CmdSave 提交到服务器


        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get
            {
                if (_cmdSave == null) _cmdSave = new RelayCommand(Ex, CanExSave, true);
                return _cmdSave;
            }
        }



        private void Ex()
        {
            FaultChecks();
            //UpdateRtuSelected();

            var update = new List<Tuple<int, int>>();
            if (ApplyFault.ContainsKey(0) && ApplyFault[0].Count == this.FaultCollection.Count)
            {
                ApplyFault.Remove(0);
            }
            foreach (var t in ApplyFault)
            {
                foreach (var f in t.Value)
                {
                    var tmp = new Tuple<int, int>(t.Key, f);
                    if (!update.Contains(tmp)) update.Add(tmp);
                }
            }

            Sr.EquipemntLightFault.Services.FaultBandtoTmlInfoSerices.UpdateFaultBandTmlInfo(update, CurrentSlecteArea.Value);

            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在保存...";
            dtSnd = DateTime.Now;

        }


        private bool CanExSave()
        {
            return CurrentSlecteArea != null && Wlst.Cr.CoreMims.Services.UserInfo.CanW(CurrentSlecteArea.Value) && DateTime.Now.Ticks - dtSnd.Ticks > 60000000;

        }


        #endregion

        #region export

        private ICommand _cmdexport;

        public ICommand Cmdexport
        {
            get
            {
                if (_cmdexport == null) _cmdexport = new RelayCommand(ExportSet, CanExport, false);
                return _cmdexport;
            }
        }

        bool CanExport()
        {
            return true;
        }

        private void ExportSet()
        {
            //if (ApplyFault.ContainsKey(0) && ApplyFault[0].Count == this.FaultCollection.Count)
            //{
            //    ApplyFault.Remove(0);
            //}

            try
            {
                Dictionary<int, string> tmpss = new Dictionary<int, string>();
                foreach (var g in FaultCollection)
                {
                    if (!tmpss.ContainsKey(g.FaultCode)) tmpss.Add(g.FaultCode, g.FaultName);
                }

                List<object> swritetitle = new List<object>();
                List<List<object>> writeinfo = new List<List<object>>();
                swritetitle.Add("设备地址");
                swritetitle.Add("设备名称");
                swritetitle.Add("报警故障列表");
                string comm = "";
                if (ApplyFault.ContainsKey(0))
                {
                    var tmpssss = (from t in ApplyFault[0] orderby t select t).ToList();
                    foreach (var g in tmpssss)
                    {
                        if (tmpss.ContainsKey(g))
                        {
                            comm += tmpss[g] + ", ";
                        }
                    }
                }


                var rtuinfo = new List<Tuple<int, string, string>>();
                foreach (
                    var t in
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Values)
                {
                    if (t.EquipmentType != WjParaBase.EquType.Rtu) continue;

                    if (!ApplyFault.ContainsKey(t.RtuId))
                    {

                        rtuinfo.Add(new Tuple<int, string, string>(t.RtuPhyId, t.RtuName, comm));
                    }
                    else
                    {
                        string tttt = "";
                        if (ApplyFault.ContainsKey(t.RtuId))
                        {
                            var tmpssss =
                                (from ssss in ApplyFault[t.RtuId] orderby ssss select ssss).ToList();
                            foreach (var g in tmpssss)
                            {
                                if (tmpss.ContainsKey(g))
                                {
                                    tttt += tmpss[g] + ", ";
                                }
                            }
                        }
                        rtuinfo.Add(new Tuple<int, string, string>(t.RtuPhyId, t.RtuName, tttt));
                    }
                }
                var tmpsfsdfsd = (from g in rtuinfo orderby g.Item1 ascending select g).ToList();
                foreach (var g in tmpsfsdfsd) writeinfo.Add(new List<object>() { g.Item1, g.Item2, g.Item3 });



                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(swritetitle, writeinfo);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表出错:" + ex);
            }
        }


        #endregion

        #region CmdReSetAllRtuFault

        private bool _isOneKeySetsss;

        /// <summary>
        /// 显示信息
        /// </summary>
        public bool IsTreeEnable
        {
            get { return _isOneKeySetsss; }
            set
            {
                if (value != _isOneKeySetsss)
                {
                    _isOneKeySetsss = value;
                    this.RaisePropertyChanged(() => IsTreeEnable);
                }
            }
        }

        private bool _isOneKeySet;

        /// <summary>
        /// 显示信息
        /// </summary>
        public bool IsOneKeySet
        {
            get { return _isOneKeySet; }
            set
            {
                if (value != _isOneKeySet)
                {
                    _isOneKeySet = value;
                    this.RaisePropertyChanged(() => IsOneKeySet);
                    if (value)
                    {
                        // CurrentSelectItem = null;
                        if (ApplyFault.ContainsKey(0))
                        {
                            SelectFauleByCode(
                                ApplyFault[0], true);
                        }
                        else
                        {
                            SelectFauleByCode(new List<int>(), true);
                        }
                        ShowMsg = "当前正在设置全局通用故障报警。";
                    }
                    IsTreeEnable = !value;
                    if (!value)
                    {
                        CurrentSelectItem = null;
                    }
                }

            }
        }

        private ICommand _cmdOneKeySet;

        public ICommand CmdReSetAllRtuFault
        {
            get
            {
                if (_cmdOneKeySet == null) _cmdOneKeySet = new RelayCommand(ExOneKey, CanOneKeySet, true);
                return _cmdOneKeySet;
            }
        }


        private DateTime _dtOneKey;

        private void ExOneKey()
        {
            _dtOneKey = DateTime.Now;
            var faults = (from t in FaultCollection where t.IsSelected select t.FaultCode).ToList();
            ApplyFault.Clear();
            ApplyFault.Add(0, faults);

            foreach (var t in ListTreeGroupNode.Info) t.Value.IsSelected = false;
            foreach (var t in ListTreeTmlNode.Info) t.Value.IsSelected = false;

        }


        private bool CanOneKeySet()
        {

            return IsOneKeySet && DateTime.Now.Ticks - _dtOneKey.Ticks > 30000000;
        }

        #endregion

    };

    /// <summary>
    /// Attri
    /// </summary>
    public partial class EquipmentFaultWithTmlSettingViewModel
    {
        private string _showMsg;

        public string ShowMsg
        {
            get { return _showMsg; }
            set
            {
                if (_showMsg != value)
                {
                    _showMsg = value;
                    this.RaisePropertyChanged(() => this.ShowMsg);
                }
            }
        }

        #region tab iinterface

        public string Title
        {
            get { return "终端故障设置"; }
        }


        public bool CanClose
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        #region define

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _fItemsArea;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> ItemsArea
        {
            get
            {
                if (_fItemsArea == null)
                {
                    _fItemsArea = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                }
                return _fItemsArea;
            }
            set
            {
                if (value == _fItemsArea) return;
                _fItemsArea = value;
                this.RaisePropertyChanged(() => ItemsArea);
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

        private Wlst.Cr.CoreOne.Models.NameValueInt _cur;
        public Wlst.Cr.CoreOne.Models.NameValueInt CurrentSlecteArea
        {
            get { return _cur; }
            set
            {
                if (value == _cur) return;
                _cur = value;
                this.RaisePropertyChanged(() => this.CurrentSlecteArea);
                LoadNode();
            }
        }


        private ObservableCollection<FauleCodeNameSelectViewModel> _fauleCodes;

        public ObservableCollection<FauleCodeNameSelectViewModel> FaultCollection
        {
            get
            {
                if (_fauleCodes == null)
                {
                    _fauleCodes = new ObservableCollection<FauleCodeNameSelectViewModel>();
                }
                return _fauleCodes;
            }
            set
            {
                if (value == _fauleCodes) return;
                _fauleCodes = value;
                this.RaisePropertyChanged(() => FaultCollection);
            }
        }


        private ObservableCollection<ListTreeNodeBase> _childTreeItemsInfo;

        public ObservableCollection<ListTreeNodeBase> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<ListTreeNodeBase>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value == _childTreeItemsInfo) return;
                _childTreeItemsInfo = value;
                this.RaisePropertyChanged(() => ChildTreeItems);
            }
        }



        private ListTreeNodeBase _currentSelectItem;

        public ListTreeNodeBase CurrentSelectItem
        {
            get { return _currentSelectItem; }
            set
            {
                if (_currentSelectItem != value)
                {
                    _currentSelectItem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectItem);

                }
                OnCurrentSelectItemChange();
            }
        }

        #endregion

        private List<int> commalarm = new List<int>();
        /// <summary>
        /// 是否自定义故障的时候全部为未选中终端
        /// </summary>
        private bool _isNoSelected = true;
        private void InitFaultCollection()
        {
            _isFauleCodeNameSelectViewModelSystemSelectd = true;
            FaultCollection.Clear();
            _isNoSelected = true;
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary.Values)
            {
                if (t.IsEnable)
                {
                    _isNoSelected = false;
                    break;
                }
            }
            var tmpssss = new List<FauleCodeNameSelectViewModel>();
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                //存在故障选中 同时 本故障未设置为使用则跳过 否则显示
                if (!_isNoSelected && !t.Value.IsEnable)
                {
                    continue;
                }

                tmpssss.Add(new FauleCodeNameSelectViewModel()
                                             {
                                                 FaultCode = t.Value.FaultId,
                                                 FaultName = t.Value.FaultNameByDefine,
                                                 IsSelected = false,
                                                 IsEnabel = false,
                                                 Color = t.Value.Color,
                                             });

            }
            //var fcsort=FaultCollection.Select()
            var tmpggg = (from t in tmpssss orderby t.FaultCode select t).ToList();
            var ggsssg = new ObservableCollection<FauleCodeNameSelectViewModel>();
            foreach (var t in tmpggg) ggsssg.Add(t);
            FaultCollection = ggsssg;
            _isFauleCodeNameSelectViewModelSystemSelectd = false;
        }


        private void OnCurrentSelectItemChange()
        {
            CleanAllSelectedFault();
            if (CurrentSelectItem == null)
            {
                CleanAllSelectedFault();
                SelectFauleByCode(new List<int>(), false);
                return;
            }
            if (CurrentSelectItem.IsGroup)
            {
                ShowMsg = "当前选中为分组，分组不允许进行设置。";
                CleanAllSelectedFault();
                SelectFauleByCode(new List<int>(), false);
                return;
            }

            ShowMsg = "当前选中终端为:" + CurrentSelectItem.PhyId + " ,名称为:" + CurrentSelectItem.NodeName;
            //if (!CurrentSelectItem.IsThisTmlSpecialTerminal && ApplyFault.ContainsKey(CurrentSelectItem.NodeId))
            //    ApplyFault.Remove(CurrentSelectItem.NodeId);


            //if (CurrentSelectItem.IsGroup)
            //{
            //    SelectFauleByCode(ApplyFault[0],false);
            //    return;
            //}
            if (ApplyFault.ContainsKey(CurrentSelectItem.NodeId))
            {
                //this.SelectFauleByCode(ApplyFault[CurrentSelectItem.NodeId], CurrentSelectItem.IsThisTmlSpecialTerminal);
                this.SelectFauleByCode(ApplyFault[CurrentSelectItem.NodeId], true);
                return;
            }
            if (ApplyFault.ContainsKey(0))
            {
                this.SelectFauleByCode(ApplyFault[0], true);
                //this.SelectFauleByCode(ApplyFault[0], CurrentSelectItem.IsThisTmlSpecialTerminal);
                return;
            }
            SelectFauleByCode(new List<int>(), true);

        }

        /// <summary>
        /// Clearn Up all FaultItem
        /// </summary>
        public void CleanAllSelectedFault()
        {
            _isFauleCodeNameSelectViewModelSystemSelectd = true;
            foreach (var t in FaultCollection)
            {
                t.IsSelected = false;
            }
            _isFauleCodeNameSelectViewModelSystemSelectd = false;
        }

        /// <summary>
        /// 设置给定的故障信息为选择 并设置该故障信息Enable为给定值
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isEnable"></param>
        public void SelectFauleByCode(List<int> code, bool isEnable)
        {
            _isFauleCodeNameSelectViewModelSystemSelectd = true;
            foreach (var t in FaultCollection)
            {
                t.IsEnabel = isEnable;
            }

            foreach (var t in FaultCollection)
            {
                if (code.Contains(t.FaultCode))
                {
                    t.IsSelected = true;
                }
                else
                {
                    t.IsSelected = false;
                }
            }
            _isFauleCodeNameSelectViewModelSystemSelectd = false;
        }


    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class EquipmentFaultWithTmlSettingViewModel
    {
        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.FaultBandtoEquipmentUpdate,
                                    PublishEventType.Core);
            this.AddEventFilterInfo(Sr.EquipemntLightFault.Services.EventIdAssign.FaultBandtoEquipmentRequest,
                                    PublishEventType.Core);
        }

        public override void ExPublishedEvent(
            Microsoft.Practices.Prism.MefExtensions.Event.EventHelper.PublishEventArgs args)
        {
            this.OnCurrentSelectItemChange();

            if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.FaultBandtoEquipmentUpdate)
            {
                if (DateTime.Now.Ticks - dtSnd.Ticks < 100000000)
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  保存成功.";
                else
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  接受到服务器更新信息，执行更新.";
                UpdateFaultsWhenUpdate();
            }
        }
    }
}
