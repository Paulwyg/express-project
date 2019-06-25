using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;

using Wlst.Ux.Wj1090Module.LduTreeSettingViewModel.ViewModel;
using Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.Wj1090Module.Wj1090TreeManageViewModel.ViewModel
{
    [Export(typeof (IIWj1090ManageViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090TreeManageViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IIWj1090ManageViewModel
    {
        private int _hxxx = 0;
        /// <summary>
        /// 前台界面绑定的图标大小
        /// </summary>
        public int Hightxx
        {
            get
            {
                if (_hxxx < 0.1)
                {
                    _hxxx = (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeightTree;
                    if (_hxxx > 24) _hxxx = 24;
                    if (_hxxx < 12) _hxxx = 12;
                }
                return _hxxx;
            }
        }
        public Wj1090TreeManageViewModel()
        {
            EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(LoadNode, 1, DelayEventHappen.EventOne);
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            //// Load();
            //if (_equipdir != null)
            //    foreach (var g in _equipdir)
            //        g.Value.UpdateTmlStateInfomation(g.Key);


        }

        #region IITab
        public int Index
        {
            get { return 5; }
        }
        public string Title
        {
            get { return "线路检测"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion
    }

    public partial class Wj1090TreeManageViewModel
    {
        //private ObservableCollection<TreeNodeTmlViewModel> _collectionWj1090;

        //public ObservableCollection<TreeNodeTmlViewModel> ChildTreeItems
        //{
        //    get { return _collectionWj1090 ?? (_collectionWj1090 = new ObservableCollection<TreeNodeTmlViewModel>()); }
        //    set
        //    {
        //        if (value == _collectionWj1090) return;
        //        _collectionWj1090 = value;
        //        RaisePropertyChanged(() => ChildTreeItems);
        //    }
        //}

        #region Reflesh

        private DateTime _dtReflesh;
        private ICommand _reflesh;

        public ICommand Reflesh
        {
            get { return _reflesh ?? (_reflesh = new RelayCommand(ExReflesh, CanExReflesh, true)); }
        }

        private bool CanExReflesh()
        {
            return DateTime.Now.Ticks - _dtReflesh.Ticks > 30000000;
        }

        private void ExReflesh()
        {
            _dtReflesh = DateTime.Now;
            this.LoadNode();
        }

        #endregion

        private string _allLine;//lvf

        public string AllLine
        {
            get { return _allLine; }
            set
            {
                if (_allLine == value) return;
                _allLine = value;
                RaisePropertyChanged(() => this.AllLine);
            }
        }
        private int _lineNum;//lvf

        public int LineNum
        {
            get { return _lineNum; }
            set
            {
                if (_lineNum == value) return;
                _lineNum = value;
                RaisePropertyChanged(() => this.LineNum);
            }
        }

        private int _noUseLineNum;//lvf

        public int NoUseLineNum
        {
            get { return _noUseLineNum; }
            set
            {
                if (_noUseLineNum == value) return;
                _noUseLineNum = value;
                RaisePropertyChanged(() => this.NoUseLineNum);
            }
        }

        #region LoadNode by Lvf

        private ObservableCollection<TreeNodeBaseNode> _childTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value != _childTreeItemsInfo)
                {
                    _childTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItems);
                }
            }
        }

        protected bool IsLoadOnlyOneArea = false;
        private void LoadNode()
        {

            if (ServicesGrpSingleInfoHold.InfoGroups.Count == 0 &&
                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                return;
            ChildTreeItems.Clear();
            List<int> areaLst = new List<int>();
            var userProperty = UserInfo.UserLoginInfo;
            areaLst.AddRange(userProperty.AreaX);
            foreach (var t in userProperty.AreaW)
            {
                if (!areaLst.Contains(t))
                {
                    areaLst.Add(t);
                }
            }
            foreach (var f in userProperty.AreaR)
            {
                if (!areaLst.Contains(f))
                {
                    areaLst.Add(f);
                }
            }
            IsLoadOnlyOneArea = areaLst.Count < 2;
            if (userProperty.D == true)
            {
                if (Wj1090TreeSetLoad.Myself.IsShowArea)
                {
                    foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                    {
                     
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var a in lstInArea)
                        {
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                            if (pb == null) continue;
                            if (pb.EquipmentType == WjParaBase.EquType.Ldu && pb.RtuFid == 0) //线路为主设备
                            {
                                rtuLst.Add(pb.RtuId);
                                //if (IsLoadOnlyOneArea)
                                //{
                                //    int AreaId = areaLst[0];
                                //    ShowGrpInArea(AreaId);
                                //}
                                //else
                                //{
                                //    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0,
                                //                                                      TypeOfTabTreeNode.IsArea));
                                //}
                                //break; 
                            }
                            else if (pb.EquipmentType == WjParaBase.EquType.Rtu &&
                                     pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有线路
                            {
                                foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                                {
                                    var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                    if (pa == null) continue;
                                    if (pa.EquipmentType == WjParaBase.EquType.Ldu && pa.RtuFid > 0)
                                    {
                                        rtuLst.Add(g);
                                    }
                                }    
                            }
                        }
                        if (rtuLst.Count > 0)
                        {
                            if (IsLoadOnlyOneArea)
                            {
                                int AreaId = areaLst[0];
                                ShowGrpInArea(AreaId);

                            }
                            else
                            {
                                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0,
                                                                                  TypeOfTabTreeNode.IsArea));
                            }
                        }
                    }
                }
                else
                {
                    foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                    {
                        ShowGrpInArea(f);
                    }
                }
            }
            else
            {
                
               
                if (IsLoadOnlyOneArea)
                {
                    int AreaId = areaLst[0];
                    ShowGrpInArea(AreaId);
                }
                else
                {
                    foreach (var f in areaLst)
                    {
                        //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var a in lstInArea)
                        {
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                            if (pb == null) continue;
                            if (pb.EquipmentType == WjParaBase.EquType.Ldu && pb.RtuFid == 0) //线路为主设备
                            {
                                rtuLst.Add(pb.RtuId);
                                //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                //break; ;
                            }
                            else if (pb.EquipmentType == WjParaBase.EquType.Rtu &&
                                     pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有线路
                            {
                                foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                                {
                                    var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                    if (pa == null) continue;
                                    if (pa.EquipmentType == WjParaBase.EquType.Ldu && pa.RtuFid > 0)
                                    {
                                        rtuLst.Add(g);
                                        //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                        //break;

                                    }
                                }
                            }
                        }
                        if (rtuLst.Count > 0)
                        {
             
                                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0,
                                                                                  TypeOfTabTreeNode.IsArea));
                
                        }
                    }
                }
            }
            for (int i = this.ChildTreeItems.Count - 1; i > 0; i--)
            {
                var t = this.ChildTreeItems[i];
                if (t.NodeType == TypeOfTabTreeNode.IsTml) continue;
                t.GetChildRtuCount();
                if (t.RtuCount == 0)
                {
                    this.ChildTreeItems.Remove(t);
                }
            }
            foreach (var t in this.ChildTreeItems) t.GetChildRtuCount();
            //CleanGrpNotHaveDic(this.ChildTreeItems);
           

            LineNum = 0;
            NoUseLineNum = 0;
            
            if(userProperty.D == true)
            {
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    // var info = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(t.Value.AttachRtuId);
                    // if (info == null) continue;
                    var lines = t.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj1090Ldu;
                        // Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
                    if (lines == null) continue;
                    if (lines.WjLduLines == null) continue;

                    foreach (var at in lines.WjLduLines)
                    {
                        if (at.Value.IsUsed)
                        {
                            LineNum++;
                        }
                        else
                        {
                            NoUseLineNum++;
                        }
                    }
                }
            }else
            {
                List<int> areaTmls = new List<int>();
                foreach (var f in areaLst)
                {
                    var lst = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(f);
                    if (lst == null) continue;
                    if (lst.LstTml.Count >0)
                    {
                        areaTmls.AddRange(lst.LstTml);
                    }
                }
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    // var info = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(t.Value.AttachRtuId);
                    // if (info == null) continue;
                    var lines = t.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj1090Ldu;
                    // Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IILduConcentrator;
                    if (lines == null) continue;
                    if (lines.WjLduLines == null) continue;
                    if(!areaTmls.Contains(lines.RtuId)) continue;
                    foreach (var at in lines.WjLduLines)
                    {
                        if (at.Value.IsUsed )
                        {
                            LineNum++;
                        }
                        else
                        {
                            NoUseLineNum++;
                        }
                    }
                }
            }

            var allnum = LineNum + NoUseLineNum;
            AllLine = "回路总数: " + LineNum + " / " + allnum;
        }

        private void ShowGrpInArea(int AreaId)
        {

            if (Wj1090TreeSetLoad.Myself.IsShowGrp)
            {
                var grp =
                            (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                             where t.Key.Item1 == AreaId
                             orderby t.Value.Index
                             select t.Value).ToList();
                foreach (var f in grp)
                {
                    var rtuList = new List<int>();
                    foreach (var fff in f.LstTml)
                    {
                        var rtu = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(fff);
                        if (rtu == null) continue;
                        if (rtu.EquipmentType == WjParaBase.EquType.Rtu &&
                            rtu.EquipmentsThatAttachToThisRtu.Count > 0)
                        {
                            foreach (var g in rtu.EquipmentsThatAttachToThisRtu)
                            {
                                var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                if (pa == null) continue;
                                if (pa.EquipmentType == WjParaBase.EquType.Ldu && pa.RtuFid > 0)
                                {
                                    rtuList.Add(g);
                                }
                            }
                        }
                        else if (rtu.EquipmentType == WjParaBase.EquType.Ldu && rtu.RtuFid == 0)
                        {
                            rtuList.Add(rtu.RtuId);
                        }

                    }
                    if (rtuList.Count < 1) continue;
                    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f.AreaId, f.GroupId,
                                                                      TypeOfTabTreeNode.IsGrp));
                }
                var sp =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                var rtuLst = new List<int>();
                foreach (var v in sp)
                {
                    var rtu = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(v);
                    if (rtu == null) continue;
                    if (rtu.EquipmentType == WjParaBase.EquType.Rtu &&
                        rtu.EquipmentsThatAttachToThisRtu.Count > 0)
                    {
                        foreach (var g in rtu.EquipmentsThatAttachToThisRtu)
                        {
                            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                            if (pa == null) continue;
                            if (pa.EquipmentType == WjParaBase.EquType.Ldu && pa.RtuFid > 0)
                            {
                                rtuLst.Add(g);
                            }
                        }
                    }
                    else if (rtu.EquipmentType == WjParaBase.EquType.Ldu && rtu.RtuFid == 0)
                    {
                        rtuLst.Add(rtu.RtuId);
                    }
                }
                if (rtuLst.Count>0)
                    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, AreaId, 0,
                                                                      TypeOfTabTreeNode.IsGrpSpecial));
            }
            else
            {
                var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                foreach (var f in lstInArea)
                {
                    var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                    if (pb == null) continue;
                    if (pb.EquipmentType == WjParaBase.EquType.Ldu && pb.RtuFid == 0) //线路为主设备
                    {
                        this.ChildTreeItems.Add(new TreeNodeWj1090ViewModel(pb.RtuId, pb.RtuName, pb.RtuFid));
                    }
                    else if (pb.EquipmentType == WjParaBase.EquType.Rtu && pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有线路
                    {

                        foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                        {
                            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                            if (pa == null) continue;
                            if (pa.EquipmentType == WjParaBase.EquType.Ldu && pa.RtuFid > 0)
                            {
                                this.ChildTreeItems.Add(new TreeNodeWj1090ViewModel(pa.RtuId, pa.RtuName, pa.RtuFid));
                            }
                        }
                    }
                }
            }
        }

        #endregion


        private Dictionary<int, TreeNodeTmlViewModel> _equipdir = new Dictionary<int, TreeNodeTmlViewModel>();

        //private void Load()
        //{
        //    ChildTreeItems.Clear();
        //    _equipdir.Clear();
        //    var tmpssss = new List<TreeNodeTmlViewModel>();
        //    foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)//.EquipmentInfoDictionary)
        //    {
        //        if (t.Value.RtuModel != EnumRtuModel.Wj1090 && t.Value.RtuModel != EnumRtuModel.Wj30920 && t.Value.RtuModel != EnumRtuModel.Wj30910) continue;
        //        if (t.Value.RtuFid == 0) continue;
        //        var tvalue = t.Value;
        //        if (
        //            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
        //                tvalue.RtuFid))
        //        {
        //            var attachInfo =
        //                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
        //                    tvalue.RtuFid];
        //            AddNode(tvalue.RtuFid, attachInfo.RtuName, t.Value.RtuId, t.Value.RtuName, ref tmpssss);

        //        }
        //    }
        //    var dir = new Dictionary<int, TreeNodeTmlViewModel>();
        //    foreach (var g in tmpssss) if (!dir.ContainsKey(g.NodeId)) dir.Add(g.NodeId, g);


        //    var ntsss = GetRtuSortBy(dir.Keys.ToList());
        //       // Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(dir.Keys.ToList());

        //    // var tmpggg = (from t in tmpssss orderby t.NodeId select t).ToList();
        //    var ggsssg = new ObservableCollection<TreeNodeTmlViewModel>();
        //    foreach (var t in ntsss) if (dir.ContainsKey(t)) ggsssg.Add(dir[t]);
        //    ChildTreeItems = ggsssg;
        //    foreach (var g in ChildTreeItems)
        //    {
        //        foreach (var tm in g.ChildTreeItems)
        //        {
        //            if (!_equipdir.ContainsKey(tm.NodeId)) _equipdir.Add(tm.NodeId, g);
        //        }
        //        if (!_equipdir.ContainsKey(g.NodeId)) _equipdir.Add(g.NodeId, g);
        //        g.UpdateTmlStateInfomation(g.NodeId);
        //    }

        //}



        private List<int> GetRtuSortBy(List<int> lstneedadd)
        {
            if (Sr.EquipmentInfoHolding .Services.UxTreeSetting.TreeSortBy == 1)//物理地址
            {
                Dictionary<int, int> sr = new Dictionary<int, int>();
                foreach (var g in lstneedadd)
                {
                    var hold = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g].RtuPhyId;//.GetPhysicalIdByLogicalId(g);
                    if (!sr.ContainsKey(g)) sr.Add(g, hold);
                }
                return (from t in sr orderby t.Value ascending select t.Key).ToList();
            }
            if (Sr.EquipmentInfoHolding .Services.UxTreeSetting.TreeSortBy == 2)//拼音排序
            {
                Dictionary<int, string> sr = new Dictionary<int, string>();
                foreach (var g in lstneedadd)
                {

                    var hold = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);//.GetEquipmentInfo(g);
                    if (hold == null)
                    {
                        if (!sr.ContainsKey(g)) sr.Add(g, g + "");
                    }
                    else
                    {
                        if (!sr.ContainsKey(g)) sr.Add(g, hold.RtuName);//this.chinesecap(hold.RtuName));
                    }
                }
                //var nt = (from t in sr orderby t.Value ascending select t).ToList();
                //if (nt == null) return new List<int>();
                return (from t in sr orderby t.Value ascending select t.Key).ToList();
            }
            if (Sr.EquipmentInfoHolding .Services.UxTreeSetting.TreeSortBy == 3) //组地址
            {
                return Sr.EquipmentInfoHolding .Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(lstneedadd);
                return lstneedadd;
            }
            if (Sr.EquipmentInfoHolding .Services.UxTreeSetting.TreeSortBy == 4)  //逻辑地址
            {
                return (from t in lstneedadd orderby t ascending select t).ToList();
            }
            return lstneedadd;
        }

        private void AddNode(int rtuId, string rtuName, int murId, string mruName, ref List<TreeNodeTmlViewModel> infos)
        {
            foreach (var t in infos)
            {
                if (t.NodeId == rtuId)
                {
                    foreach (var f in t.ChildTreeItems)
                    {
                        if (f.NodeId == murId)
                        {
                            return;
                        }
                    }
                    t.ChildTreeItems.Add(new TreeNodeWj1090ViewModel(murId, mruName, rtuId));
                    return;
                }

            }

            var tml = new TreeNodeTmlViewModel(rtuId, rtuName);
            tml.ChildTreeItems.Add(new TreeNodeWj1090ViewModel(murId, mruName, rtuId));
            infos.Add(tml);
        }


        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (!Wj1090Module.LduTreeSettingViewModel.ViewModel.Wj1090TreeSetLoad.Myself.IsShowTreeOnTab)
                    return false;
                if (args.EventType == PublishEventType.ReCn) return true;
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                        return true;
                    if (args.EventId ==
                       global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                        return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                    {
                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null) return false ;
                        if (lst.Count > 0)
                        {
                            foreach (var g in lst)
                            {
                                if (_equipdir.ContainsKey(g))
                                {
                                    return true;
                                  //  _equipdir[g].UpdateTmlStateInfomation(g);
                                }
                            }
                        }
                        return false ;
                    }
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        private void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.ReCn) LoadNode();
                if (args.EventType == PublishEventType.Core)
                {
                    //update name

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                    {
                        var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                        if (lst == null) return;
                        foreach (var t in lst)
                        {
                            if (_equipdir.ContainsKey(t.Item1))
                            {
                                _equipdir[t.Item1].EquipmentUpdate(t.Item1);
                            }
                        }
                        //UpdateNoUsedShow(lst);
                    }else if (args.EventId ==
                         global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                    {
                        LoadNode();
                    }
               
                    else if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                    {
                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null) return;
                        if (lst.Count > 0)
                        {
                            foreach (var g in lst)
                            {
                                if (_equipdir.ContainsKey(g))
                                {
                                    _equipdir[g].UpdateTmlStateInfomation(g);
                                }
                            }
                        }
                        ;
                    }
                    else if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                    {
                        var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        if (lst == null) return;
                        if (lst.Count > 0)
                        {
                            foreach (var g in lst)
                            {
                                if (_equipdir.ContainsKey(g.Item1))
                                {
                                    foreach (var l in _equipdir[g.Item1].ChildTreeItems)
                                    {
                                        var mtp = _equipdir[g.Item1];
                                        if (l.NodeId == g.Item1)
                                        {
                                            _equipdir[g.Item1].ChildTreeItems.Remove(l);

                                            _equipdir.Remove(g.Item1);

                                            if (mtp != null)
                                            {
                                                mtp.UpdateTmlStateInfomation(g.Item1);

                                                if (mtp.ChildTreeItems.Count < 1)
                                                {
                                                    //if (this.ChildTreeItems.Contains(mtp))    lvf
                                                    //    this.ChildTreeItems.Remove(mtp);
                                                    //if (_equipdir.ContainsKey(mtp.NodeId)) _equipdir.Remove(mtp.NodeId);

                                                }
                                            }

                                            break;
                                        }
                                    }
                                    //_equipdir[g.Item1].UpdateTmlStateInfomation(g.Item1);
                                }
                            }
                        }
                        ;
                    }
                    else if (args.EventId == EventIdAssign.EquipmentAddEventId)
                    {
                        //todo 
                        var lst = args.GetParams()[0] as List<Tuple<int, int>>;
                        if (lst == null) return;
                        if (lst.Count > 0)
                        {
                            foreach (var g in lst)
                            {
                                if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLine(g.Item1))
                                {
                                    var tmp =
                                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g.Item1);// GetEquipmentInfo
                                             //(g.Item1);
                                    if (tmp == null) continue;
                                    var father = tmp.RtuFid;
                                    if (_equipdir.ContainsKey(father))
                                    {
                                        _equipdir[father].ChildTreeItems.Add(new TreeNodeWj1090ViewModel(g.Item1,
                                                                                                           tmp.RtuName,
                                                                                                           father));
                                        if (!_equipdir.ContainsKey(g.Item1)) _equipdir.Add(g.Item1, _equipdir[g.Item1]);
                                    }
                                    else
                                    {
                                        if (father == 0) father = g.Item1;
                                        var equ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(father);//.
                                            //GetEquipmentInfo
                                            //(father);
                                        if (equ == null) continue;

                                        var tml = new TreeNodeTmlViewModel(father, equ.RtuName);
                                        tml.ChildTreeItems.Add(new TreeNodeWj1090ViewModel(g.Item1,
                                                                                             tmp.RtuName,
                                                                                             father));
                                        //this.ChildTreeItems.Add(tml);   lvf
                                        //if (!_equipdir.ContainsKey(g.Item1)) _equipdir.Add(g.Item1, tml);
                                        //if (!_equipdir.ContainsKey(father)) _equipdir.Add(father, tml);
                                    }
                                }
                            }
                        }
                    }
                    //else if (args.EventId == EventIdAssign.RunningInfoUpdate1)
                    //{
                    //    var lst = args.GetParams()[0] as IEnumerable<int>;
                    //    if (lst == null) return;
                    //    foreach (var t in lst)
                    //    {
                    //        if (TreeNodeItemSluViewModel.RtuItems.ContainsKey(t))
                    //        {
                    //            foreach (var f in TreeNodeItemSluViewModel.RtuItems[t])
                    //            {
                    //                if (f.Target != null)
                    //                {
                    //                    var xg = f.Target as SluTreeNodeBase;
                    //                    if (xg != null) xg.ReUpdate(2);
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

    public partial class Wj1090TreeManageViewModel
    {
        private ObservableCollection<TreeNodeTmlViewModel> _collectionWj1090Sr;

        public ObservableCollection<TreeNodeTmlViewModel> ChildTreeItemsSearch
        {
            get { return _collectionWj1090Sr ?? (_collectionWj1090Sr = new ObservableCollection<TreeNodeTmlViewModel>()); }
            set
            {
                if (value == _collectionWj1090Sr) return;
                _collectionWj1090Sr = value;
                RaisePropertyChanged(() => ChildTreeItemsSearch);
            }
        }

        //#region Search Node
        ////lvf
        //private string _searchText;

        //public string SearchText
        //{
        //    get { return _searchText; }
        //    set
        //    {
        //        if (_searchText != value)
        //        {
        //            _searchText = value;
        //            this.RaisePropertyChanged(() => this.SearchText);
        //            SearchNode(_searchText);
        //        }
        //    }
        //}

        //CmdClearUpSearchText
      //  #region CmdClearUpSearchText

      //  private ICommand _cmdCmdClearUpSearchText;

      //  public ICommand CmdClearUpSearchText
      //  {
      //      get
      //      {
      //          if (_cmdCmdClearUpSearchText == null)
      //              _cmdCmdClearUpSearchText = new RelayCommand(ExCmdClearUpSearchText, CanCmdClearUpSearchText, false);
      //          return _cmdCmdClearUpSearchText;
      //      }
      //  }

      //  private void ExCmdClearUpSearchText()
      //  {
      //      SearchText = "";
      //  }


      //  private bool CanCmdClearUpSearchText()
      //  {
      //      return !string.IsNullOrEmpty(SearchText);
      //      ;
      //  }



      //  #endregion

        //  private Dictionary<int, string> _ipaddrs = new Dictionary<int, string>();

        //查询终端  lvf
        //private void SearchNode(string keyWord)
        //{
        //    ChildTreeItemsSearch.Clear();
        //    if (keyWord == "")
        //    {
        //        IsSearchTreeVisi = Visibility.Collapsed;
        //        ChildTreeItemsSearch.Clear();
        //        return;
        //    }


        //    foreach (var node in ChildTreeItems)
        //    {

        //        //var tmlName = string.Format("{0:D4}", nodeId) + "-" + BaseNodes.Nodess[nodeId].NodeName;



        //        if (node.NodeIds.Contains(keyWord))
        //        {
        //            node.ExtendSerachConten = "物理地址";
        //            ChildTreeItemsSearch.Add(node);
        //            continue;
        //        }
        //        if (node.NodeId.ToString().Contains(keyWord))
        //        {
        //            node.ExtendSerachConten = "逻辑地址";
        //            ChildTreeItemsSearch.Add(node);
        //            continue;
        //        }


        //        if (StringContainKeyword(node.NodeName, keyWord))
        //        {
        //            node.ExtendSerachConten = "终端名称";
        //            ChildTreeItemsSearch.Add(node);
        //            continue;
        //        }

        //        foreach (var g in node.ChildTreeItems)
        //        {

        //            if (g.NodeId.ToString().Contains(keyWord))
        //            {
        //                node.ExtendSerachConten = "集中器地址";
        //                ChildTreeItemsSearch.Add(node);
        //                continue;
        //            }


        //            if (StringContainKeyword(g.NodeName, keyWord))
        //            {
        //                node.ExtendSerachConten = "集中器名称";
        //                ChildTreeItemsSearch.Add(node);
        //                continue;
        //            }
        //        }

        //    }
        //    IsSearchTreeVisi = Visibility.Visible;

        //}


      //  private Visibility _isSearchTreeVisi;

      //  public Visibility IsSearchTreeVisi
      //  {
      //      get { return _isSearchTreeVisi; }
      //      set
      //      {
      //          if (value == _isSearchTreeVisi) return;
      //          _isSearchTreeVisi = value;
      //          this.RaisePropertyChanged(() => this.IsSearchTreeVisi);
      //      }
      //  }


      //  //private void SearchChildrenNodeVisi(TreeNodeBaseNode viewModel)
      //  //{
      //  //    foreach (var node in viewModel.ChildTreeItems)
      //  //    {
      //  //        node.Visi = Visibility.Visible;
      //  //        if (node.NodeType == TypeOfTabTreeNode.IsGrp ||node .NodeType ==TypeOfTabTreeNode.IsGrpSpecial )
      //  //        {
      //  //            SearchChildrenNodeVisi(node);
      //  //        }
      //  //    }
      //  //}


      //  ///// <summary>
      //  ///// 提供递归查询终端树节点名称是否包含某特定关键字
      //  ///// </summary>
      //  ///// <param name="viewModel"></param>
      //  ///// <param name="keyWord"></param>
      //  ///// <returns></returns>
      //  //private int SearchChildrenNode(TreeNodeBaseNode viewModel, string keyWord)
      //  //{
      //  //    if (viewModel.NodeType == TypeOfTabTreeNode.IsGrp || viewModel.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
      //  //    {
      //  //    }
      //  //    else return 0;

      //  //    int nodeCount = 0;
      //  //    foreach (var node in viewModel.ChildTreeItems)
      //  //    {
      //  //        var tmlName = node.NodeName;
      //  //        tmlName = string.Format("{0:D4}", node.NodeId) + "-" + node.NodeName;

      //  //        if (node.NodeType == TypeOfTabTreeNode.IsGrp)
      //  //        {
      //  //            int pares = SearchChildrenNode(node, keyWord);
      //  //            nodeCount += pares;

      //  //            if (pares > 0) node.Visi = Visibility.Visible;
      //  //            else
      //  //            {
      //  //                node.Visi = Visibility.Collapsed;
      //  //            }
      //  //        }
      //  //        else
      //  //        {
      //  //            if (StringContainKeyword(tmlName, keyWord))
      //  //            {
      //  //                node.Visi = Visibility.Visible;
      //  //                nodeCount += 1;
      //  //            }
      //  //            else
      //  //            {
      //  //                node.Visi = Visibility.Collapsed;
      //  //            }
      //  //        }
      //  //    }
      //  //    return nodeCount;
      //  //}

      //  /// <summary>
      //  /// 前者是否包含后者数据 
      //  /// </summary>
      //  /// <param name="containerStinng"></param>
      //  /// <param name="keyword"></param>
      //  /// <returns></returns>
      //  private bool StringContainKeyword(string containerStinng, string keyword)
      //  {
      //      if (containerStinng.Contains(keyword)) return true;
      //      string conv = chinesecap(containerStinng);
      //      if (conv.Contains(keyword)) return true;
      //      if (containerStinng.ToUpper().Contains(keyword.ToUpper())) return true;
      //      return false;
      //  }


      //  /// <summary>
      //  /// 返回汉字字符串的拼音的首字母
      //  /// </summary>
      //  /// <param name="chinesestr">要转换的字符串</param>
      //  /// <returns></returns>
      //  public string chinesecap(string chinesestr)
      //  {
      //      byte[] zw = new byte[2];
      //      string charstr = "";
      //      string capstr = "";
      //      for (int i = 0; i <= chinesestr.Length - 1; i++)
      //      {
      //          charstr = chinesestr.Substring(i, 1).ToString(CultureInfo.InvariantCulture);
      //          zw = System.Text.Encoding.Default.GetBytes(charstr);
      //          // 得到汉字符的字节数组
      //          if (zw.Length == 2)
      //          {
      //              int i1 = (short)(zw[0]);
      //              int i2 = (short)(zw[1]);
      //              long chinesestrInt = i1 * 256 + i2;
      //              //table of the constant list
      //              // a; //45217..45252
      //              // z; //54481..55289
      //              capstr += GetChinesefirst(chinesestrInt);
      //          }
      //          else
      //          {
      //              capstr += charstr;
      //          }

      //          //capstr = capstr + chinastr;
      //      }

      //      return capstr;
      //  }

      //  private string GetChinesefirst(long chinesestrInt)
      //  {
      //      string chinastr = "";
      //      //table of the constant list
      //      // a; //45217..45252
      //      // b; //45253..45760
      //      // c; //45761..46317
      //      // d; //46318..46825
      //      // e; //46826..47009
      //      // f; //47010..47296
      //      // g; //47297..47613

      //      // h; //47614..48118
      //      // j; //48119..49061
      //      // k; //49062..49323
      //      // l; //49324..49895
      //      // m; //49896..50370
      //      // n; //50371..50613
      //      // o; //50614..50621
      //      // p; //50622..50905
      //      // q; //50906..51386

      //      // r; //51387..51445
      //      // s; //51446..52217
      //      // t; //52218..52697
      //      //没有u,v
      //      // w; //52698..52979
      //      // x; //52980..53640
      //      // y; //53689..54480
      //      // z; //54481..55289

      //      if ((chinesestrInt >= 45217) && (chinesestrInt <= 45252))
      //      {
      //          chinastr = "a";
      //      }
      //      else if ((chinesestrInt >= 45253) && (chinesestrInt <= 45760))
      //      {
      //          chinastr = "b";
      //      }
      //      else if ((chinesestrInt >= 45761) && (chinesestrInt <= 46317))
      //      {
      //          chinastr = "c";
      //      }
      //      else if ((chinesestrInt >= 46318) && (chinesestrInt <= 46825))
      //      {
      //          chinastr = "d";
      //      }
      //      else if ((chinesestrInt >= 46826) && (chinesestrInt <= 47009))
      //      {
      //          chinastr = "e";
      //      }
      //      else if ((chinesestrInt >= 47010) && (chinesestrInt <= 47296))
      //      {
      //          chinastr = "f";
      //      }
      //      else if ((chinesestrInt >= 47297) && (chinesestrInt <= 47613))
      //      {
      //          chinastr = "g";
      //      }
      //      else if ((chinesestrInt >= 47614) && (chinesestrInt <= 48118))
      //      {
      //          chinastr = "h";
      //      }

      //      else if ((chinesestrInt >= 48119) && (chinesestrInt <= 49061))
      //      {
      //          chinastr = "j";
      //      }
      //      else if ((chinesestrInt >= 49062) && (chinesestrInt <= 49323))
      //      {
      //          chinastr = "k";
      //      }
      //      else if ((chinesestrInt >= 49324) && (chinesestrInt <= 49895))
      //      {
      //          chinastr = "l";
      //      }
      //      else if ((chinesestrInt >= 49896) && (chinesestrInt <= 50370))
      //      {
      //          chinastr = "m";
      //      }

      //      else if ((chinesestrInt >= 50371) && (chinesestrInt <= 50613))
      //      {
      //          chinastr = "n";
      //      }
      //      else if ((chinesestrInt >= 50614) && (chinesestrInt <= 50621))
      //      {
      //          chinastr = "o";
      //      }
      //      else if ((chinesestrInt >= 50622) && (chinesestrInt <= 50905))
      //      {
      //          chinastr = "p";
      //      }
      //      else if ((chinesestrInt >= 50906) && (chinesestrInt <= 51386))
      //      {
      //          chinastr = "q";
      //      }

      //      else if ((chinesestrInt >= 51387) && (chinesestrInt <= 51445))
      //      {
      //          chinastr = "r";
      //      }
      //      else if ((chinesestrInt >= 51446) && (chinesestrInt <= 52217))
      //      {
      //          chinastr = "s";
      //      }
      //      else if ((chinesestrInt >= 52218) && (chinesestrInt <= 52697))
      //      {
      //          chinastr = "t";
      //      }
      //      else if ((chinesestrInt >= 52698) && (chinesestrInt <= 52979))
      //      {
      //          chinastr = "w";
      //      }
      //      else if ((chinesestrInt >= 52980) && (chinesestrInt <= 53640))
      //      {
      //          chinastr = "x";
      //      }
      //      else if ((chinesestrInt >= 53689) && (chinesestrInt <= 54480))
      //      {
      //          chinastr = "y";
      //      }
      //      else if ((chinesestrInt >= 54481) && (chinesestrInt <= 55289))
      //      {
      //          chinastr = "z";
      //      }
      //      return chinastr;
      //  }

      //  #endregion
    }

}
