using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.BseVm;
using EventIdAssign = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.vms
{

    public partial class TreeSingleViewModel
    {
        private static Dictionary<int, TreeSingleViewModel> Infox = new Dictionary<int, TreeSingleViewModel>();

        public static void ReloadAllOnAreaDeArri()
        {
            AddEventSubScriptionTokenerx();


            foreach (var f in Infox)
            {
                if (!TreeTabRtuSet.TabRtuHolding.Info.ContainsKey(f.Key))
                {
                    RegionManage.ShowViewByIdAttachRegion(
                        f.Value.ViewId,
                       
                        false
                        );
                    var viewx = Wlst.Cr.Core.CoreServices.RegionManage.GetViewById(f.Value.ViewId);
                    var xgr = viewx as UserControl;
                    if (xgr != null)
                    {
                        xgr.DataContext = null;
                    }
                }
            }

            var kexf = (from t in Infox select t.Key).ToList();
            foreach (var f in kexf)
            {
                if (!TreeTabRtuSet.TabRtuHolding.Info.ContainsKey(f))
                {
                    if (Infox.ContainsKey(f)) Infox.Remove(f);
                }
            }

            foreach (var f in Infox)
            {
                f.Value.Regi();
            }


            foreach (var f in TreeTabRtuSet.TabRtuHolding.Info)
            {
                if (Infox.ContainsKey(f.Key)) continue;
                var usedkeys = (from t in Infox.Values select t.ViewId).ToList();
                int canuserid = 0;
                for (int i = Services.ViewIdAssign.TabTreeViewAttachIdStart;
                     i < Services.ViewIdAssign.TabTreeViewAttachIdEnd;
                     i++)
                {
                    if (usedkeys.Contains(i)) continue;
                    canuserid = i;
                    break;
                }
                if (canuserid == 0) break;


                var viewx = Wlst.Cr.Core.CoreServices.RegionManage.GetViewById(canuserid);
                var xgr = viewx as UserControl;
                if (xgr != null)
                {
                    var tmpr = new TreeSingleViewModel(f.Key, canuserid);
                    tmpr.titlex = f.Value.Name;
                    xgr.DataContext = tmpr;

                    RegionManage.ShowViewByIdAttachRegion(
                        canuserid,
                       
                        true
                        );
                }
            }
        }

        public int Indexx;
        public int ViewId;

        public TreeSingleViewModel(int index,int viewId)
        {
            ViewId = viewId;
            Indexx = index;
            if (Infox.ContainsKey(index))
            {
                Infox.Remove(index);
            }
            Infox.Add(index, this);
           
            LoadNode();
            IsSearchTreeVisi = Visibility.Collapsed;
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Regi, 1, DelayEventHappen.EventOne);
        }



        private void Regi()
        {
            ReUpdateLoadChild();
        }

        #region tab iinterface


        private string titlex = "分组";
        public string Title
        {
            get { return titlex; }
        }


        public bool CanClose
        {
            get { return false; }
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

    }


    //event
    public partial class TreeSingleViewModel
    {
        private static bool IsRegisterx = false;

        private static void AddEventSubScriptionTokenerx()
        {
            if (IsRegisterx) return;
            IsRegisterx = true;
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlersx, FundOrderFilters);
        }

        //event

        #region IEventAggregator Subscription

        /// <summary>
        /// 事件过滤
        /// 目前只处理
        /// 1、系统当前选中的终端或分组变更，提供联动
        /// 2、终端参数发生变化的时候，即使更新显示数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv) return true;
                if (args.EventType != PublishEventType.Core) return false;


                if (args.EventId ==
                    Wlst.Sr.EquipmentInfoHolding .Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                    return true;

                if (args.EventId == EventIdAssign.EquipmentAddEventId)
                    return true;
                if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                    return true;
                if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                    return true;
                if (args.EventId == EventIdAssign.RunningInfoUpdate1 ) //Wlst.Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged  2018年4月17日14:21:21
                    return true;
                if (args.EventId == EventIdAssign.RunningInfoUpdate2) //RunningInfoUpdate   lvf  2018年4月17日14:21:16 
                    return true;
                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp)
                    return true;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        private static void FundEventHandlersx(PublishEventArgs args)
        {
            FundEventHandlersAll(args);
            foreach (var f in Infox) f.Value.FundEventHandlersSelf(args);
        }

        private void FundEventHandlersSelf(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv)
                {
                    ReUpdateLoadChild();
                    return;
                }
                if (args.EventType != PublishEventType.Core) return;

                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp)
                {
                    BaseNodes .OnRtuGroupSelectdWantedMapUpEventRvd();
                }

                if (args.EventId ==
                    Wlst.Sr.EquipmentInfoHolding .Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                {
                    ReUpdateLoadChild();
                }

                if (args.EventId == EventIdAssign.EquipmentAddEventId)
                {
                    ReUpdateLoadChild();
                }

            }
            catch (Exception ex)
            {
            }
        }

        private static void FundEventHandlersAll(PublishEventArgs args)
        {
            try
            {

                if (args.EventType != PublishEventType.Core) return;


                if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                {
                    var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                    if (lst == null) return;

                    foreach (var t in lst)
                    {
                        if (t.Item2 == 0)
                        {

                            if (BaseNodes.Nodess.ContainsKey(t.Item1))
                            {
                                BaseNodes.Nodess.Remove(t.Item1);
                            }
                            if (BaseNodes.AllTmpNodess.ContainsKey(t.Item1))
                            {
                                BaseNodes.AllTmpNodess.Remove(t.Item1);
                            }
                            foreach (var f in Infox)
                                f.Value.DeleteNode(t.Item1, f.Value.ChildTreeItems);
                        }
                    }

                }


                if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                {
                    var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                    if (lst == null) return;
                    foreach (var t in lst)
                    {
                        if (t.Item2 == 0)
                        {
                            if (BaseNodes.Nodess.ContainsKey(t.Item1))
                            {
                                BaseNodes.Nodess[t.Item1].ReUpdate(1);
                            }
                            if (BaseNodes.AllTmpNodess.ContainsKey(t.Item1))
                            {
                                BaseNodes.AllTmpNodess[t.Item1].ReUpdate(1);
                            }
                        }
                    }
                }
                if (args.EventId == EventIdAssign.RunningInfoUpdate1)//RtuOnLineInfoChanged)  lvf 2018年4月17日14:21:54
                {
                    var lst = args.GetParams()[0] as IEnumerable<int>;
                    if (lst == null) return;

                    foreach (var t in lst)
                    {
                        if (BaseNodes.Nodess.ContainsKey(t))
                        {
                            BaseNodes.Nodess[t].ReUpdate(2);
                        }
                        if (BaseNodes.AllTmpNodess.ContainsKey(t))
                        {
                            BaseNodes.AllTmpNodess[t].ReUpdate(2);
                        }
                    }
                }
                //if (args.EventId == EventIdAssign.RunningInfoUpdate1) // Wlst.Sr.EquipemntLightFault.Services.EventIdAssign.RtuErrorStateChanged  lvf 2018年4月17日14:23:25
                //{
                //    try
                //    {

                //        var lst = args.GetParams()[0] as List<Tuple<int, bool>>;
                //        if (lst == null) return;
                //        foreach (var x in lst)
                //        {
                //            if (BaseNodes.Nodess.ContainsKey(x.Item1))
                //            {
                //                BaseNodes.Nodess[x.Item1].ReUpdate(2);
                //            }
                //            if (BaseNodes.AllTmpNodess.ContainsKey(x.Item1))
                //            {
                //                BaseNodes.AllTmpNodess[x.Item1].ReUpdate(2);
                //            }
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //}

                //if (args.EventId == EventIdAssign.RunningInfoUpdate1)RtuLightHasElectricStatesChanged
                //    try
                //    {

                //        int x = Convert.ToInt32(args.GetParams()[0]);
                //        if (x > 0)
                //        {
                //            if (BaseNodes.Nodess.ContainsKey(x))
                //            {
                //                BaseNodes.Nodess[x].ReUpdate(2);
                //            }
                //            if (BaseNodes.AllTmpNodess.ContainsKey(x))
                //            {
                //                BaseNodes.AllTmpNodess[x].ReUpdate(2);
                //            }
                //        }

                //    }
                //    catch (Exception ex)
                //    {
                //    }



            }
            catch (Exception ex)
            {
            }
        }


        private void DeleteNode(int nodeId, ObservableCollection<TreeNodeBaseNode> item)
        {
            for (int i = item.Count - 1; i >= 0; i--)
            {
                if (item[i].NodeId == nodeId)
                {
                    item.RemoveAt(i);
                    continue;
                }
                DeleteNode(nodeId, item[i].ChildTreeItems);
            }
        }

        #endregion


    }

    //load  reload
    public partial class TreeSingleViewModel : ObservableObject
    {

        /// <summary>
        /// 动态更新树
        /// </summary>
        private void ReUpdateLoadChild()
        {
            if (ChildTreeItems.Count == 0)
            {
                LoadNode();
                return;
            }
            var AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(Indexx);
            var grplst = TreeTabRtuSet.TabRtuHolding.GetGrpLstByIdx(Indexx);
                //判断已经存在的分组是否已经删除   确认删除
            for (int i = ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsGrp)
                {
                    if (!grplst.Contains(ChildTreeItems[i].NodeId))
                    {
                        ChildTreeItems.RemoveAt(i);
                    }
                }
                else if (ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsGrpSpecial ||
                         ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsAll)
                {
                }
                else
                {
                    ChildTreeItems.RemoveAt(i);
                }
            }
            //存在该分组则更新 不存在则增加并加载
            foreach (var t in grplst)
            {
                bool bolfind = false;
                foreach (var f in ChildTreeItems)
                {
                    if (f.NodeType == TypeOfTabTreeNode.IsGrp && f.NodeId == t)
                    {
                        bolfind = true;
                        f.ReUpdate(0);
                        break;
                    }
                }
                if (!bolfind)
                {
                    var tu = new Tuple<int, int>(AreaId, t);
                    if (!ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu)) continue;
                    var newAdd = new TreeNodeItemSingleGroupViewModel(null,
                                                                      ServicesGrpSingleInfoHold.
                                                                          InfoGroups[tu]);
                    this.ChildTreeItems.Add(newAdd);
                    // newAdd.AddChild();
                }
            }
            var ntss = ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grplst);


                var indexed = new List<TreeNodeBaseNode>();
                var dir = new Dictionary<int, TreeNodeBaseNode>();
                TreeNodeBaseNode all = null;
                TreeNodeBaseNode sep = null;
                foreach (var g in this.ChildTreeItems)
                {
                    if (g.NodeType == TypeOfTabTreeNode.IsAll)
                    {
                        all = g;
                        continue;
                    }
                    if (g.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
                    {
                        sep = g;
                        continue;
                    }
                    if (!dir.ContainsKey(g.NodeId)) dir.Add(g.NodeId, g);
                }
                if (all != null) indexed.Add(all);
                foreach (var t in ntss )
                {
                    if (dir.ContainsKey(t)) indexed.Add(dir[t]);
                }
                if (sep != null) indexed.Add(sep);
                for (int i = 0; i < indexed.Count; i++)
                {
                    if (this.ChildTreeItems.Count > i)
                    {
                        if (this.ChildTreeItems[i].NodeId == indexed[i].NodeId &&
                            this.ChildTreeItems[i].NodeType == indexed[i].NodeType)
                            continue;
                        this.ChildTreeItems.RemoveAt(i);
                        this.ChildTreeItems.Insert(i, indexed[i]);
                    }
                    else
                    {
                        this.ChildTreeItems.Add(indexed[i]);
                    }
                }
              

            

            AddSepcialTmltoTree();
            AddAllTmltoTree();
            foreach (var t in this.ChildTreeItems) t.GetChildRtuCount();
        }

        #region load node

        //加载终端节点
        private void LoadNode()
        {
            if (!TreeTabRtuSet.TabRtuHolding.Info.ContainsKey(Indexx) ||Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Count ==0)
                //ServicesEquipemntInfoHold.EquipmentInfoDictionary.Count == 0)
                return;
            ChildTreeItems.Clear();

            var tmps = (from gt in  TreeTabRtuSet.TabRtuHolding.GetGrpLstByIdx(Indexx) orderby gt ascending select gt).ToList();
            var ntss = ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(tmps);
            foreach (var t in ntss)
            {
                //lvf 2018年4月17日14:37:13 默认区域id为0
                var tu = new Tuple<int, int>(0, t);
                if (!ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
                    continue;

                ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(null,ServicesGrpSingleInfoHold.InfoGroups[tu]));
            }

            AddSepcialTmltoTree();

            AddAllTmltoTree();
            foreach (var t in this.ChildTreeItems) t.GetChildRtuCount();
        }

        //加载无分组终端节点
        private void AddSepcialTmltoTree()
        {
            TreeNodeBaseNode node = null;
            foreach (var t in ChildTreeItems)
            {
                if (t.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
                {
                    node = t;
                    break;
                }
            }

            var _tmlList = TreeTabRtuSet.TabRtuHolding.GetRtuLstSpecialByIdx(Indexx);

            List<int> lstAllSpecial = new List<int>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)//ServicesEquipemntInfoHold.MainEquipmentInfoDictionary
            {
                if (t.Value.RtuFid != 0) continue;   
                var equipmentInfo = t.Value;
                if (_tmlList.Contains(equipmentInfo.RtuId)) lstAllSpecial.Add(equipmentInfo.RtuId);
            }
            if (node == null)
            {
                var f = new TreeNodeItemSingleSpecialViewModel("未分组设备");
                UpdateSpecialTreeNode(f, lstAllSpecial);
                if (f.ChildTreeItems.Count > 0)
                {
                    this.ChildTreeItems.Add(f);
                }
            }
            else
            {
                UpdateSpecialTreeNode(node, lstAllSpecial);
                //node.GetChildRtuCount();
                if (node.ChildTreeItems.Count == 0)
                {
                    this.ChildTreeItems.Remove(node);
                }
            }
        }

        //加载无分组终端节点
        private void AddAllTmltoTree()
        {
            TreeNodeBaseNode node = null;
            foreach (var t in ChildTreeItems)
            {
                if (t.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    node = t;
                    break;
                }
            }
            if (node == null)
            {
                node = new TreeNodeItemSingleSpecialViewModel("全部设备") {NodeType = TypeOfTabTreeNode.IsAll};
                this.ChildTreeItems.Insert(0, node);
            }
            var lstNoew = TreeTabRtuSet.TabRtuHolding.GetRtuLstByIdx(Indexx);
            var existnode = (from t in node.ChildTreeItems select t.NodeId).ToList();
            List<int> lstneedadd = new List<int>();

            foreach (var t in  Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)//ServicesEquipemntInfoHold.MainEquipmentInfoDictionary
            {
                if (t.Value.RtuFid != 0) continue;//AttachRtuId
                if (existnode.Contains(t.Value.RtuId)) continue;
                var equipmentInfo = t.Value;
                if (lstNoew.Contains(equipmentInfo.RtuId)) lstneedadd.Add(equipmentInfo.RtuId);
            }

            
           
            var ntss = GetRtuSortBy(lstneedadd);

            foreach (var t in ntss)
            {
                if (!BaseNodes.AllTmpNodess.ContainsKey(t))
                {
                    //if (
                    //    !ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
                    //        t))
                    //    continue;
                    //var f =
                    //    ServicesEquipemntInfoHold.EquipmentInfoDictionary[t];
                    //if (f.AttachRtuId != 0) continue;

                    if(Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t)==false )
                        continue;
                    var f = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                    if (f.RtuFid != 0) continue;

                    BaseNodes.AddAllTmpNodess(new TreeNodeItemTmlViewModel(node, f));
                }
                if (!BaseNodes.AllTmpNodess.ContainsKey(t)) continue;
                node.ChildTreeItems.Add(BaseNodes.AllTmpNodess[t]);
            }
        }
        private List< int >  GetRtuSortBy(List< int > lstneedadd)
        {
            if (UxTreeSetting.TreeSortBy == 1)//物理地址
            {
                Dictionary<int, int> sr = new Dictionary<int, int>();
                foreach (var g in lstneedadd)
                {
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(g)==false)
                        continue;
                    var hold = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[g].RtuPhyId;// ServicesEquipemntInfoHold.GetPhysicalIdByLogicalId(g);
                    if (!sr.ContainsKey(g)) sr.Add(g, hold);
                }
                return (from t in sr orderby t.Value ascending select t.Key).ToList();
            }
            if (UxTreeSetting.TreeSortBy == 2)//拼音排序
            {
                Dictionary<int, string > sr = new Dictionary<int, string >();
                foreach (var g in lstneedadd)
                {

                    var hold = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);//ServicesEquipemntInfoHold.GetEquipmentInfo( g);
                    if(hold ==null )
                    {
                        if (!sr.ContainsKey(g)) sr.Add(g, g+"");
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
            if (UxTreeSetting.TreeSortBy == 3) //组地址
            {
                return ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(lstneedadd);
            }
            if (UxTreeSetting.TreeSortBy == 4)  //逻辑地址
            {
                return (from t in lstneedadd orderby t ascending select t).ToList();
            }
            return lstneedadd;
        }


        /// <summary>
        /// 添加特殊终端到特殊终端节点  
        /// </summary>
        /// <param name="specialNode">特殊终端主节点</param>
        /// <param name="lstAllSpecial">特殊终端列表集合</param>
        private void UpdateSpecialTreeNode(TreeNodeBaseNode specialNode, List<int> lstAllSpecial)
        {
            if (specialNode == null) return;

            int count = specialNode.ChildTreeItems.Count;
            for (int i = count - 1; i >= 0; i--) //delete
            {
                int nodeId = specialNode.ChildTreeItems[i].NodeId;
                if (!lstAllSpecial.Contains(nodeId))
                {
                    specialNode.ChildTreeItems.RemoveAt(i);
                }
                else
                {
                    lstAllSpecial.Remove(nodeId);
                }
            }

            var ntss = ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(lstAllSpecial);
            //add
            foreach (var t in ntss)
            {
                if (!BaseNodes.Nodess.ContainsKey(t))
                {
                    //if (
                    //    !ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
                    //        t))
                    //    continue;
                    //var f =
                    //    ServicesEquipemntInfoHold.EquipmentInfoDictionary[t];
                    //if (f.AttachRtuId != 0) continue;
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t) == false)
                        continue;
                    var f = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                    if (f.RtuFid != 0) continue;
                    new TreeNodeItemTmlViewModel(specialNode, f);
                }
                if (!BaseNodes.Nodess.ContainsKey(t)) continue;
                specialNode.ChildTreeItems.Add(BaseNodes.Nodess[t]);
                continue;

            }

        }

        #endregion


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
                if(value !=_childTreeItemsInfo )
                {
                    _childTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItems);
                }
            }
        }


    };

    //search
    public partial class TreeSingleViewModel
    {

        private ObservableCollection<TreeNodeBaseNode> _searchchildTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItemsSearch
        {
            get
            {
                if (_searchchildTreeItemsInfo == null)
                    _searchchildTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                return _searchchildTreeItemsInfo;
            }
        }

        #region Search Node

        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    this.RaisePropertyChanged(() => this.SearchText);
                    SearchNode(_searchText);
                }
            }
        }

        //CmdClearUpSearchText
        #region CmdClearUpSearchText

        private ICommand _cmdCmdClearUpSearchText;

        public ICommand CmdClearUpSearchText
        {
            get
            {
                if (_cmdCmdClearUpSearchText == null)
                    _cmdCmdClearUpSearchText = new RelayCommand(ExCmdClearUpSearchText, CanCmdClearUpSearchText, false );
                return _cmdCmdClearUpSearchText;
            }
        }

        private void ExCmdClearUpSearchText()
        {
            SearchText = "";
        }


        private bool CanCmdClearUpSearchText()
        {
            return !string.IsNullOrEmpty(SearchText);
            ;
        }



        #endregion

        private Dictionary<int, string> _ipaddrs = new Dictionary<int, string>();

        //查询终端
        private void SearchNode(string keyWord)
        {
            ChildTreeItemsSearch.Clear();
            if (keyWord == "")
            {
                IsSearchTreeVisi = Visibility.Collapsed;
                ChildTreeItemsSearch.Clear();
                return;
            }

            var rtnx = TreeTabRtuSet.TabRtuHolding.GetRtuLstByIdx(Indexx);

            var kesss = (from t in BaseNodes.Nodess.Keys where rtnx .Contains(t) orderby t ascending select t).ToList();

            foreach (var nodeId in kesss)
            {
                if(BaseNodes .Nodess [nodeId ].PhyId .ToString().Contains( keyWord ))
                {
                    BaseNodes.Nodess[nodeId].ExtendSerachConten = "物理地址-" + BaseNodes.Nodess[nodeId].PhyId;
                    ChildTreeItemsSearch.Add(BaseNodes.Nodess[nodeId]);
                    continue;
                }
                if (BaseNodes.Nodess[nodeId].NodeId.ToString().Contains(keyWord))
                {
                    BaseNodes.Nodess[nodeId].ExtendSerachConten = "逻辑地址-" + BaseNodes.Nodess[nodeId].NodeId;
                    ChildTreeItemsSearch.Add(BaseNodes.Nodess[nodeId]);
                    continue;
                }
                if (BaseNodes.Nodess[nodeId].PhoneNumber.Contains(keyWord))
                {
                    BaseNodes.Nodess[nodeId].ExtendSerachConten = "手机号码-" + BaseNodes.Nodess[nodeId].PhoneNumber;
                    ChildTreeItemsSearch.Add(BaseNodes.Nodess[nodeId]);
                    continue;
                }

                if (StringContainKeyword(BaseNodes.Nodess[nodeId].NodeName, keyWord))
                {
                    BaseNodes.Nodess[nodeId].ExtendSerachConten = "终端名称";
                    ChildTreeItemsSearch.Add(BaseNodes.Nodess[nodeId]);
                    continue;
                }
                if (!_ipaddrs.ContainsKey(nodeId))
                {
                    //var tmp =
                    //    Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentInfo(nodeId);
                    //if (tmp != null)
                    //{
                    //    var ggg = tmp as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IIRtuParaGprs;
                    //    if (ggg != null && !string.IsNullOrEmpty(ggg.Ip))
                    //    {
                    //        _ipaddrs.Add(nodeId, ggg.Ip);
                    //    }
                    //}

                    var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(nodeId);
                    if (tmp != null)
                    {
                        var ggg = tmp as Wj3005Rtu;
                        if (ggg == null) continue;
                        var ipsdd =
                            new System.Net.IPAddress(BitConverter.GetBytes(ggg.WjGprs.StaticIp)).ToString();
                        _ipaddrs.Add(nodeId, ipsdd);
                    }
                }
                if (_ipaddrs.ContainsKey(nodeId))
                {
                    if (StringContainKeyword(_ipaddrs[nodeId], keyWord))
                    {
                        BaseNodes.Nodess[nodeId].ExtendSerachConten = "Ip-" + _ipaddrs[nodeId].Trim();
                        ChildTreeItemsSearch.Add(BaseNodes.Nodess[nodeId]);
                    }
                }


            }
            IsSearchTreeVisi = Visibility.Visible;

        }


        private Visibility _isSearchTreeVisi;

        public Visibility IsSearchTreeVisi
        {
            get { return _isSearchTreeVisi; }
            set
            {
                if (value == _isSearchTreeVisi) return;
                _isSearchTreeVisi = value;
                this.RaisePropertyChanged(() => this.IsSearchTreeVisi);
            }
        }


      

        /// <summary>
        /// 前者是否包含后者数据 
        /// </summary>
        /// <param name="containerStinng"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private bool StringContainKeyword(string containerStinng, string keyword)
        {
            if (containerStinng.Contains(keyword)) return true;
            string conv = chinesecap(containerStinng);
            if (conv.Contains(keyword)) return true;
            if (containerStinng.ToUpper().Contains(keyword.ToUpper())) return true;
            return false;
        }


        /// <summary>
        /// 返回汉字字符串的拼音的首字母
        /// </summary>
        /// <param name="chinesestr">要转换的字符串</param>
        /// <returns></returns>
        public string chinesecap(string chinesestr)
        {
            byte[] zw = new byte[2];
            string charstr = "";
            string capstr = "";
            for (int i = 0; i <= chinesestr.Length - 1; i++)
            {
                charstr = chinesestr.Substring(i, 1).ToString(CultureInfo.InvariantCulture);
                zw = System.Text.Encoding.Default.GetBytes(charstr);
                // 得到汉字符的字节数组
                if (zw.Length == 2)
                {
                    int i1 = (short) (zw[0]);
                    int i2 = (short) (zw[1]);
                    long chinesestrInt = i1*256 + i2;
                    //table of the constant list
                    // a; //45217..45252
                    // z; //54481..55289
                    capstr += GetChinesefirst(chinesestrInt);
                }
                else
                {
                    capstr += charstr;
                }

                //capstr = capstr + chinastr;
            }

            return capstr;
        }

        private string GetChinesefirst(long chinesestrInt)
        {
            string chinastr = "";
            //table of the constant list
            // a; //45217..45252
            // b; //45253..45760
            // c; //45761..46317
            // d; //46318..46825
            // e; //46826..47009
            // f; //47010..47296
            // g; //47297..47613

            // h; //47614..48118
            // j; //48119..49061
            // k; //49062..49323
            // l; //49324..49895
            // m; //49896..50370
            // n; //50371..50613
            // o; //50614..50621
            // p; //50622..50905
            // q; //50906..51386

            // r; //51387..51445
            // s; //51446..52217
            // t; //52218..52697
            //没有u,v
            // w; //52698..52979
            // x; //52980..53640
            // y; //53689..54480
            // z; //54481..55289

            if ((chinesestrInt >= 45217) && (chinesestrInt <= 45252))
            {
                chinastr = "a";
            }
            else if ((chinesestrInt >= 45253) && (chinesestrInt <= 45760))
            {
                chinastr = "b";
            }
            else if ((chinesestrInt >= 45761) && (chinesestrInt <= 46317))
            {
                chinastr = "c";
            }
            else if ((chinesestrInt >= 46318) && (chinesestrInt <= 46825))
            {
                chinastr = "d";
            }
            else if ((chinesestrInt >= 46826) && (chinesestrInt <= 47009))
            {
                chinastr = "e";
            }
            else if ((chinesestrInt >= 47010) && (chinesestrInt <= 47296))
            {
                chinastr = "f";
            }
            else if ((chinesestrInt >= 47297) && (chinesestrInt <= 47613))
            {
                chinastr = "g";
            }
            else if ((chinesestrInt >= 47614) && (chinesestrInt <= 48118))
            {
                chinastr = "h";
            }

            else if ((chinesestrInt >= 48119) && (chinesestrInt <= 49061))
            {
                chinastr = "j";
            }
            else if ((chinesestrInt >= 49062) && (chinesestrInt <= 49323))
            {
                chinastr = "k";
            }
            else if ((chinesestrInt >= 49324) && (chinesestrInt <= 49895))
            {
                chinastr = "l";
            }
            else if ((chinesestrInt >= 49896) && (chinesestrInt <= 50370))
            {
                chinastr = "m";
            }

            else if ((chinesestrInt >= 50371) && (chinesestrInt <= 50613))
            {
                chinastr = "n";
            }
            else if ((chinesestrInt >= 50614) && (chinesestrInt <= 50621))
            {
                chinastr = "o";
            }
            else if ((chinesestrInt >= 50622) && (chinesestrInt <= 50905))
            {
                chinastr = "p";
            }
            else if ((chinesestrInt >= 50906) && (chinesestrInt <= 51386))
            {
                chinastr = "q";
            }

            else if ((chinesestrInt >= 51387) && (chinesestrInt <= 51445))
            {
                chinastr = "r";
            }
            else if ((chinesestrInt >= 51446) && (chinesestrInt <= 52217))
            {
                chinastr = "s";
            }
            else if ((chinesestrInt >= 52218) && (chinesestrInt <= 52697))
            {
                chinastr = "t";
            }
            else if ((chinesestrInt >= 52698) && (chinesestrInt <= 52979))
            {
                chinastr = "w";
            }
            else if ((chinesestrInt >= 52980) && (chinesestrInt <= 53640))
            {
                chinastr = "x";
            }
            else if ((chinesestrInt >= 53689) && (chinesestrInt <= 54480))
            {
                chinastr = "y";
            }
            else if ((chinesestrInt >= 54481) && (chinesestrInt <= 55289))
            {
                chinastr = "z";
            }
            return chinastr;
        }

        #endregion
    }

    
}