using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Wj9001Module.Resources;
using Wlst.Ux.Wj9001Module.Wj9001TreeView.Sercives;
using Wlst.client;

namespace Wlst.Ux.Wj9001Module.Wj9001TreeView.ViewModel
{
    [Export(typeof(IIWj9001TreeView))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj9001TreeViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, Sercives.IIWj9001TreeView
    {
        public static Wj9001TreeViewModel MySelf;
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

        public Wj9001TreeViewModel()
        {
            if (MySelf == null) MySelf = this;
            //Load();
           EventPublish.AddEventTokener( 
                    Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(LoadNode, 1, DelayEventHappen.EventOne);
            LoadXml();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            // Load();
        }
        private void LoadXml()
        {
            try
            {
                var infos = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("TabTreeSetConfg");
                if (infos.ContainsKey("SearchLimit"))
                {
                    SearchLimit = Convert.ToInt32(infos["SearchLimit"]);
                }
                else SearchLimit = 0;


            }
            catch (Exception ex)
            {

            }

        }

       //public static Wj1050ManageViewModel MySelf;
        private static TreeNodeWj9001ViewModel _currentSelectedTreeNode;

        public TreeNodeWj9001ViewModel CurrentSelectedTreeNode
        {
            get { return _currentSelectedTreeNode; }
            set
            {
                if (_currentSelectedTreeNode != value)
                {
                    _currentSelectedTreeNode = value;
                    if (_currentSelectedTreeNode != null)
                    {
                        // UpdateViewById(_currentSelectedTreeNode.NodeId);
                        //var args = new PublishEventArgs
                        //{
                        //    EventType = PublishEventType.Core,
                        //    EventId =Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                        //};
                        //args.AddParams(_currentSelectedTreeNode.AttachRtuId);
                        //args.AddParams(_currentSelectedTreeNode.NodeId);
                        //EventPublish.PublishEvent(args);
                    }
                }
            }
        }

        public static bool OnSelectNodeChangeNavToParsSet = true;

        public void UpdateViewById(int leakId)
        {
            if (OnSelectNodeChangeNavToParsSet == false) return;
            Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(
                Wj9001Module.Services.ViewIdAssign.Wj9001ParaSetViewId, leakId);

        }

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






        //private EventHandler<NodeSelectedArgs> OnSelectedNodeByCodeIns;
        //event EventHandler<NodeSelectedArgs> IISingleTree.OnSelectedNodeByCode
        //{
        //    add { OnSelectedNodeByCodeIns += value; }
        //    remove { if (OnSelectedNodeByCodeIns != null) OnSelectedNodeByCodeIns -= value; }
        //}



        private ObservableCollection<TreeNodeTmlViewModel> _collectionWj9001;

        /// <summary>
        /// 开关量输入参数
        /// </summary>

        public ObservableCollection<TreeNodeTmlViewModel> CollectionWj9001
        {
            get { return _collectionWj9001 ?? (_collectionWj9001 = new ObservableCollection<TreeNodeTmlViewModel>()); }
            set
            {
                if (value == _collectionWj9001) return;
                _collectionWj9001 = value;
                this.RaisePropertyChanged(() => this.CollectionWj9001);
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

        private ObservableCollection<TreeNodeBaseNode> _searchchildTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItemsSearch
        {
            get
            {
                if (_searchchildTreeItemsInfo == null)
                    _searchchildTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                return _searchchildTreeItemsInfo;
            }
            set
            {
                if (value != _searchchildTreeItemsInfo)
                {
                    _searchchildTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItemsSearch);
                }
            }
        }


        protected bool IsLoadOnlyOneArea = false;
        public  void LoadNode()
        {
            if (ServicesGrpSingleInfoHold.InfoGroups.Count == 0 &&
                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                return;
            ChildTreeItems.Clear();
            var userProperty = UserInfo.UserLoginInfo;
            List<int> areaLst = new List<int>();
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
                if (Wj9001ManageSetting .ViewModel .Wj9001LoadSet .Myself .IsShowArea)
                {
                    foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                    {
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var a in lstInArea)
                        {
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                            if (pb == null) continue;
                            if (pb.EquipmentType == WjParaBase.EquType.Leak  && pb.RtuFid == 0) //线路为主设备
                            {
                                rtuLst.Add(pb.RtuId);
                                //if (IsLoadOnlyOneArea)
                                //{
                                //    int AreaId = areaLst[0];
                                //    ShowGrpInArea(AreaId);
                                //}
                                //else
                                //{
                                //    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
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
                                    if (pa.EquipmentType == WjParaBase.EquType.Leak  && pa.RtuFid > 0)
                                    {
                                        rtuLst.Add(g);
                                        //if (IsLoadOnlyOneArea)
                                        //{
                                        //    int AreaId = areaLst[0];
                                        //    ShowGrpInArea(AreaId);
                                        //}
                                        //else
                                        //{
                                        //     this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                        //}

                                        //break;

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
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var a in lstInArea)
                        {
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                            if (pb == null) continue;
                            if (pb.EquipmentType == WjParaBase.EquType.Leak  && pb.RtuFid == 0) //线路为主设备
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
                                    if (pa.EquipmentType == WjParaBase.EquType.Leak && pa.RtuFid > 0)
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
                


            }
            for (int i = this.ChildTreeItems.Count - 1; i > 0; i--)
            {
                var t = this.ChildTreeItems[i];
                if (t.NodeType == TypeOfTabTreeNode.IsTml) continue;
                t.GetChildRtuCount();
                if (t.RtuCount == 0)
                {
                    ChildTreeItems.Remove(t);
                }
            }
            foreach (var t in this.ChildTreeItems) t.GetChildRtuCount();
        }
        public  void ShowGrpInArea(int AreaId)
        {

            if (Wj9001ManageSetting .ViewModel .Wj9001LoadSet .Myself.IsShowGrp)
            {


                var grp =
                            (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups
                             where t.Key.Item1 == AreaId
                             orderby t.Value.Index
                             select t.Value).ToList();

                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, AreaId, 0, TypeOfTabTreeNode.IsAll));

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
                                if (pa.EquipmentType == WjParaBase.EquType.Leak && pa.RtuFid > 0)
                                {
                                    rtuList.Add(g);
                                }
                            }
                        }
                        else if (rtu.EquipmentType == WjParaBase.EquType.Leak && rtu.RtuFid == 0)
                        {
                            rtuList.Add(rtu.RtuId);
                        }
                    }
                    if (rtuList.Count < 1) continue;
                    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f.AreaId, f.GroupId,
                                                                      TypeOfTabTreeNode.IsGrp));
                }
                var sp =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);//返回列表只有主设备
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
                            if (pa.EquipmentType == WjParaBase.EquType.Leak && pa.RtuFid > 0)
                            {
                                rtuLst.Add(g);
                            }
                        }
                    }
                    else if (rtu.EquipmentType == WjParaBase.EquType.Leak && rtu.RtuFid == 0)
                    {
                        rtuLst.Add(rtu.RtuId);
                    }
                }
                if (rtuLst.Count > 0)
                    this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, AreaId, 0,
                                                                      TypeOfTabTreeNode.IsGrpSpecial));
            }
            else
            {

                this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, AreaId, 0, TypeOfTabTreeNode.IsAll));
                //var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                //foreach (var f in lstInArea)
                //{
                //    var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                //    if (pb == null) continue;
                //    if (pb.EquipmentType == WjParaBase.EquType.Leak && pb.RtuFid == 0) //电表为主设备
                //    {
                //        this.ChildTreeItems.Add(new TreeNodeWj9001ViewModel(pb.RtuId, pb.RtuName, pb.RtuFid));
                //    }
                //    else if (pb.EquipmentType == WjParaBase.EquType.Rtu && pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有电表
                //    {

                //        foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                //        {
                //            var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                //            if(pa==null) continue;
                //            if (pa.EquipmentType == WjParaBase.EquType.Leak && pa.RtuFid > 0)
                //            {
                //                this.ChildTreeItems.Add(new TreeNodeWj9001ViewModel(pa.RtuId, pa.RtuName, pa.RtuFid));
                //            }
                //        }
                //    }
                //}
            }
        }


        #endregion

        private void Load()
        {
            CollectionWj9001 .Clear();


            var tmpssss = new List<TreeNodeTmlViewModel>();//new List<TreeNodeTmlViewModel>();

            foreach (
                var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                )
            {
                if (t.Value.RtuModel != EnumRtuModel.Wj9001 ) continue;
                if (t.Value.RtuFid == 0) continue;


                var ggg = t.Value;
                if (
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                        ContainsKey(ggg.RtuFid))
                {
                    var ggggg =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [
                                ggg.RtuFid];
                    this.AddNode(ggg.RtuFid, ggggg.RtuName, t.Value.RtuId, t.Value.RtuName, ref tmpssss);

                }
                // CollectionWj1050.Add(new TreeNodeWj1050ViewModel(fff.RtuId, fff.RtuName, ggg.AttachRtuId));
            }
            var tmpggg = (from t in tmpssss orderby t.NodeId select t).ToList();
            //foreach (var t in tmpggg) this.CollectionWj1050.Add(t);
            var ggsssg = new ObservableCollection<TreeNodeTmlViewModel>();//new ObservableCollection<TreeNodeTmlViewModel>();
            foreach (var t in tmpggg) ggsssg.Add(t);
            CollectionWj9001  = ggsssg;
        }

        private void AddNode(int rtuId, string rtuName, int murId, string mruName, ref List<TreeNodeTmlViewModel> infos)
        {
            foreach (var t in this.CollectionWj9001)
            {
                if (t.NodeId == rtuId)
                {
                    foreach (var f in t.CollectionWj9001)
                    {
                        if (f.NodeId == murId)
                        {
                            return;
                        }
                    }
                    t.CollectionWj9001.Add(new TreeNodeWj9001ViewModel(murId, mruName, rtuId));
                    return;
                }

            }

            var tml = new TreeNodeTmlViewModel(rtuId, rtuName);
            //  this.CollectionWj1050.Add(tml);
            var wj1050lst = new TreeNodeWj9001ViewModel(murId, mruName, rtuId);
            tml.CollectionWj9001.Add(new TreeNodeWj9001ViewModel(murId, mruName, rtuId));
            infos.Add(tml);
        }


        #region tab iinterface

        public int Index
        {
            get { return 7; }
        }
        public string Title
        {
            get { return "漏电保护"; }
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






    }

    //event
    public partial class Wj9001TreeViewModel
    {
        #region IEventAggregator Subscription

        /// <summary>
        /// 事件过滤
        /// 目前只处理
        /// 1、系统当前选中的终端或分组变更，提供联动
        /// 2、终端参数发生变化的时候，即使更新显示数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.SvAv) return true;
                if (args.EventType == PublishEventType.ReCn) return true;
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId ==
                        global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                        return true;
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                        return true;

                    if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                        return true;
                    //if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                    //    return true;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                        return true;
                    //lvf 2018年3月30日08:32:11  增加变图标机制,根据终端状态
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate1)
                        return true ;
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2)
                        return true ;
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
                if (args.EventType == PublishEventType.SvAv)
                {
                    LoadNode();
                    return;
                }
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId ==
                        global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                    {
                        LoadNode();
                    }



                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                    {
                        LoadNode();
                    }
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                    {

                        LoadNode();
                    }
                    //if (args.EventId == EventIdAssign.RunningInfoUpdate1)  todo
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
                    //                    if (xg != null) UpdateTmlStateInfomation();
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                     if (args.EventId == EventIdAssign.EquipmentUpdateEventId)
                     {

                         var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
                         if (lst == null) return;
                         //    this.ReUpdateLoadChild();
                         foreach (var t in lst)
                         {
                             if (t.Item2 != 0)
                             {

                                LoadNode();
                             }
                         }
                     }

                     if (args.EventId == EventIdAssign.RunningInfoUpdate1 || args.EventId == EventIdAssign.RunningInfoUpdate2)//todo  那终端id 变电表状态
                     {
                         var lst = args.GetParams()[0] as IEnumerable<int>;
                         if (lst == null) return;
                         foreach (var t in lst)
                         {

                             if (!TreeNodeWj9001ViewModel.RtuLeakIds.ContainsKey(t)) continue;


                             //var id = GetImageIconByState(t);
                             var leakId = TreeNodeWj9001ViewModel.RtuLeakIds[t];
                             foreach (var i in leakId)
                             {
                                 if (TreeNodeWj9001ViewModel.RtuItems.ContainsKey(i))
                                 {
                                     foreach (var f in TreeNodeWj9001ViewModel.RtuItems[i])
                                     {
                                         if (f.Target != null)
                                         {

                                             var xg = f.Target as TreeNodeBaseNode;
                                             if (xg != null) xg.ReUpdate(1);


                                         }
                                     }

                                 }
                             }

                         }
                     }
                 






                }
            }
            catch (Exception ex)
            {
            }
        }





        #endregion
        public event EventHandler<NodeSelectedArgs> OnClearSerchTest;

        private EventHandler<NodeSelectedArgs> OnSelectedNodeByCodeIns;
        event EventHandler<NodeSelectedArgs> IIWj9001TreeView.OnSelectedNodeByCode
        {
            add { OnSelectedNodeByCodeIns += value; }
            remove { if (OnSelectedNodeByCodeIns != null) OnSelectedNodeByCodeIns -= value; }
        }



        #region Search Node

        private bool StartSearch = false;
        private int SearchLimit = 0;
        /// <summary>
        /// 区域呈现排序
        /// </summary>
        private List<int> areaIndex = new List<int>();

        private string _searchText;
        public delegate void SearchNodeInvoke(string text);

        public string SearchText
        {
            get { return _searchText; }
            set
            {

                if (_searchText != value)
                {

                    _searchText = value;
                    this.RaisePropertyChanged(() => this.SearchText);

                    if (string.IsNullOrEmpty(value) || value == "")
                    {
                        IsSearchTreeVisi = Visibility.Collapsed;
                    }
                    if (SearchLimit == 1) return;
                    SearchNodeold(_searchText);


                    //////StartSearch = true;
                    //////timer_count = 0;
                    //if (UxTreeSetting.IsShowRapidOp == 1)
                    //{

                    //SearchNode(_searchText);
                    //}
                    //else
                    //{
                    //    SearchNodeold(_searchText);
                    //}


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
                    _cmdCmdClearUpSearchText = new RelayCommand(ExCmdClearUpSearchText, CanCmdClearUpSearchText, false);

                return _cmdCmdClearUpSearchText;
            }
        }

        private void ExCmdClearUpSearchText()
        {
            SearchText = "";

        }


        private bool CanCmdClearUpSearchText()
        {
            return ChildTreeItemsSearch.Count > 0;
            return !string.IsNullOrEmpty(SearchText);

        }



        #endregion




        ObservableCollection<TreeNodeBaseNode> Getallnode(TreeNodeBaseNode node)
        {
            //lvf 2019年2月20日16:00:07
            //if (node.NodeType == TypeOfTabTreeNode.IsArea) if (node.NodeType == TypeOfTabTreeNode.IsAll) return node.ChildTreeItems;//return node.ChildTreeItems; 
            foreach (var f in node.ChildTreeItems) if (f.NodeType == TypeOfTabTreeNode.IsAll) return f.ChildTreeItems;
            return new ObservableCollection<TreeNodeBaseNode>();
        }
        //查询终端





        private List<TreeNodeBaseNode> tmpList2 = new List<TreeNodeBaseNode>();
        private List<TreeNodeBaseNode> tmpListChk = new List<TreeNodeBaseNode>();
        public void SearchNodeold(string keyWord)
        {

            tmpList2.Clear();
            ChildTreeItemsSearch.Clear();
            if (keyWord == "")
            {
                IsSearchTreeVisi = Visibility.Collapsed;
                ChildTreeItemsSearch.Clear();
                return;
            }

            //var kesss =
            //    (from t in GrpComSingleMuliViewModel.BaseNodes.Nodess.Keys orderby t ascending select t).ToList();

            var lst = new List<TreeNodeBaseNode>();
            foreach (var f in ChildTreeItems)
            {
                if (f.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    lst.AddRange(f.ChildTreeItems);
                }
                else if (f.NodeType == TypeOfTabTreeNode.IsArea)
                {
                    lst.AddRange(Getallnode(f));
                }

            }
            #region edit

            List<TreeNodeBaseNode> tmpList = new List<TreeNodeBaseNode>();
            if (keyWord.Length > 0)
            {

                    #region foreach

                    foreach (var nodeId in lst)
                    {
                        nodeId.ExtendSerachConten = null;
                        if (nodeId.PhyId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten = "物理地址-" + nodeId.PhyId;

                        }
                        if (nodeId.NodeId.ToString().Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 逻辑地址-" + nodeId.NodeId;


                        }
                        if (nodeId.PhoneNumber.Contains(keyWord))
                        {
                            nodeId.ExtendSerachConten += " 手机号码-" + nodeId.PhoneNumber;


                        }

                        if (StringContainKeyword(nodeId.NodeName, keyWord))
                        {
                            nodeId.ExtendSerachConten += " 终端名称";

                        }


                        if (StringContainKeyword(nodeId.IpAddr, keyWord))
                        {
                            nodeId.ExtendSerachConten += " Ip-" + nodeId.IpAddr.Trim();

                        }

                        if (nodeId.ExtendSerachConten != null) tmpList.Add(nodeId);
                        // ChildTreeItemsSearch.Add(nodeId);

                    }

                    #endregion
              

            }
            #endregion

            tmpList2 = (from t in tmpList orderby t.PhyId ascending select t).ToList();
            int index = 0;
            foreach (var t in tmpList2)
            {

                index++;
                if (SearchLimit != 0 && SearchLimit != 1 && index > SearchLimit) break;//todo lvf test
                ChildTreeItemsSearch.Add(t);
                //if (index % 20 == 1)
                //{
                //    Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();
                //}
            }


            IsSearchTreeVisi = Visibility.Visible;

            if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;
            var ins = new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
            };


            var info = (from t in ChildTreeItemsSearch select t.NodeId).ToList();
            ins.AddParams(info);
            if (info.Count > 0)
            {
                EventPublish.PublishEvent(ins);
            }

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
            if (containerStinng == null) return false;
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
                    int i1 = (short)(zw[0]);
                    int i2 = (short)(zw[1]);
                    long chinesestrInt = i1 * 256 + i2;
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
