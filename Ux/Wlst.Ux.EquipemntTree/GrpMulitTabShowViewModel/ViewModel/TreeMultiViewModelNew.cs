using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.EquipemntTree.GrpComSingleMuliViewModel;
using Wlst.Ux.EquipemntTree.GrpMulitTabShowViewModel.Services;
using Wlst.Ux.EquipemntTree.GrpSingleTabShowViewModel.Services;
using Wlst.Ux.EquipemntTree.Models;

namespace Wlst.Ux.EquipemntTree.GrpMulitTabShowViewModel.ViewModel
{
    [Export(typeof (IIMultiTree))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TreeMultiViewModelNew : ObservableObject, IIMultiTree
    {
        public static TreeMultiViewModelNew MySelf;

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

        public TreeMultiViewModelNew()
        {

            if (MySelf == null) MySelf = this;
            IsLoadOnlyOneArea = true;
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
            LoadNode();

            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(Update, 1, DelayEventHappen.EventOne);

            IsVir = true;
        }

        #region load node

        protected bool IsLoadOnlyOneArea = false;
        //加载终端节点
        private void LoadNode()
        {
            if (ServicesGrpMultiInfoHoldNew.ItemsMultiGrp.Count == 0 &&
                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                return;
            ChildTreeItems.Clear();

            var userProperty = UserInfo.UserLoginInfo;
            if (userProperty.D == true)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0) return;
                IsLoadOnlyOneArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2;
                if (IsLoadOnlyOneArea)
                {
                    int AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList()[0];
                    var grp =
                        (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.ItemsMultiGrp
                         where t.Key.Item1 == AreaId
                         orderby t.Value.Index
                         select t.Value).ToList();

                    foreach (var f in grp)
                    {
                        this.ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(null, f.AreaId, f.GroupId,
                                                                                       TypeOfTabTreeNode.IsGrp));
                    }
                }
                else
                {
                    this.ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(null, -1, 0,
                                                                                       TypeOfTabTreeNode.IsArea));
                    foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                    {
                        var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var fff in tmlLstOfArea)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                continue;
                            rtuLst.Add(fff);
                        }
                        if (rtuLst.Count == 0) continue;
                        
                        this.ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(null, f, 0,
                                                                                       TypeOfTabTreeNode.IsArea));
                    }
                }
            }
            else
            {
                List<int> areaLst = new List<int>();
                areaLst.AddRange(userProperty.AreaR);
                foreach (var t in userProperty.AreaW)
                {
                    if (!areaLst.Contains(t))
                    {
                        areaLst.Add(t);
                    }
                }
                foreach (var t in userProperty.AreaX)
                {
                    if (!areaLst.Contains(t))
                    {
                        areaLst.Add(t);
                    }
                }
                IsLoadOnlyOneArea = areaLst.Count < 2;
                if (IsLoadOnlyOneArea)
                {
                    
                        int AreaId = areaLst[0];
                        var grp =
                            (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.ItemsMultiGrp
                             where t.Key.Item1 == AreaId
                             orderby t.Value.Index
                             select t.Value).ToList();
                        foreach (var f in grp)
                        {
                            this.ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(null, f.AreaId, f.GroupId,
                                                                                           TypeOfTabTreeNode.IsGrp));
                        }
                    

                }
                else
                {
                    this.ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(null, -1, 0,
                                                                                       TypeOfTabTreeNode.IsArea));
                    foreach (var f in areaLst)
                    {
                        var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        var rtuLst = new List<int>();
                        foreach (var fff in tmlLstOfArea)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                continue;
                            rtuLst.Add(fff);
                        }
                        if (rtuLst.Count == 0) continue;
                        //if (tmlLstOfArea.Count == 0) continue;
                        
                        this.ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(null, f, 0,
                                                                                       TypeOfTabTreeNode.IsArea));
                    }
                }
            }

            foreach (var f in ChildTreeItems)
                f.GetChildRtuCount();

            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.ChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
            }
        }
        #endregion

        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点  
        /// </summary>
        public void UpdateArea(int areaId)
        {

            var info = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(areaId);
            if (info == null)
            {
                this.ChildTreeItems.Clear();
                return;
            }

            var gprlst = (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.ItemsMultiGrp
                          where t.Key.Item1 == areaId
                          select t.Key.Item2).ToList();

            //node delete
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeId == 0) continue;
                if (gprlst.Contains(ChildTreeItems[i].NodeId) == false) ChildTreeItems.RemoveAt(i);
                if (ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsGrp)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
            }

            //tml  add and update
            var exist = (from t in ChildTreeItems select t.NodeId).ToList();
            var lstUp = new List<int>();
            foreach (var t in gprlst)
            {
                if (exist.Contains(t))
                {
                    lstUp.Add(t);
                    continue;
                }

                var para = Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.GetGroupInfomation(areaId, t);
                if (para == null) continue;

                if (para.LstTml.Count == 0) continue;
                ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(null, areaId, t, TypeOfTabTreeNode.IsGrp));
            }

            foreach (var f in this.ChildTreeItems)
            {
                if (!lstUp.Contains(f.NodeId)) continue;
                f.ReUpdate(0);
            }

            //for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            //{
            //    if (this.ChildTreeItems[i].ChildTreeItems.Count == 0) this.ChildTreeItems.RemoveAt(i);
            //}
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.ChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
            }
        }


        public void Update()
        {
            if (this.ChildTreeItems.Count == 0)
            {
                this.LoadNode();
                return;
            }
            bool onlyone = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2;
            if (IsLoadOnlyOneArea != onlyone)
            {
                this.ChildTreeItems.Clear();
                this.LoadNode();
                return;
            }
            if (IsLoadOnlyOneArea)
            {
                int areaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0
                                 ? 0
                                 : Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList()[0];
                UpdateArea(areaId);
            }
            else
            {
                var areas = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList();
                for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
                {
                    if (areas.Contains(ChildTreeItems[i].NodeId) ||
                        ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsArea)
                    {
                        this.ChildTreeItems.RemoveAt(i);
                    }
                    else
                    {
                        ChildTreeItems[i].ReUpdate(0);
                    }
                }

                var ars = (from t in ChildTreeItems select t.NodeId).ToList();
                this.ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(null, -1, 0, TypeOfTabTreeNode.IsArea));
                foreach (var f in areas)
                {
                    if (ars.Contains(f)) continue;
                    this.ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(null, f, 0, TypeOfTabTreeNode.IsArea));
                }

            }

            foreach (var f in ChildTreeItems)
            {
                f.GetChildRtuCount();

            }
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (this.ChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0)
                {
                    this.ChildTreeItems.RemoveAt(i);
                }


            }
        }




        #region UpdateAreaGrp

        //public void UpdateAreaGrp()
        //{

        //    var areas = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo;
        //    var areaKeys = areas.Keys.ToList();
        //    var AreaList = new List<int>();
        //    var GrpList = new List<int>();
        //    var LstGrp = new List<int>();
        //    var areagrp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups;
        //    var grpKeys = new List<int>();

        //    //更新分组名称
        //    foreach (var v in areagrp)
        //    {
        //        grpKeys.Add(v.Key.Item2);
        //        foreach (var vvv in ChildTreeItems)
        //        {
        //            foreach (var x in vvv.ChildTreeItems)
        //            {
        //                if (x.NodeId == v.Key.Item2 && x.NodeName != v.Value.GroupName)
        //                {
        //                    x.NodeName = v.Value.GroupName;
        //                }
        //                if (x.NodeType == TypeOfSluTreeNode.SluGrp)
        //                {
        //                    if (GrpList.Contains(x.NodeId)) continue;
        //                    GrpList.Add(x.NodeId);
        //                }

        //            }
        //        }
        //    }
        //    for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
        //    {
        //        if (ChildTreeItems[i].NodeType == TypeOfSluTreeNode.SluArea)
        //        {
        //            AreaList.Add(ChildTreeItems[i].NodeId);
        //        }
        //        //更新区域名称
        //        foreach (var f in areas)
        //        {

        //            if (ChildTreeItems[i].NodeId == f.Value.AreaId &&
        //                ChildTreeItems[i].NodeName != f.Value.AreaName &&
        //                ChildTreeItems[i].NodeType == TypeOfSluTreeNode.SluArea)
        //            {
        //                ChildTreeItems[i].NodeName = f.Value.AreaName;
        //            }
        //        }
        //        //删除已不存在的区域
        //        if (!areaKeys.Contains(ChildTreeItems[i].NodeId) &&
        //            ChildTreeItems[i].NodeType == TypeOfSluTreeNode.SluArea)
        //        {
        //            this.ChildTreeItems.RemoveAt(i);
        //        }

        //        //删除已不存在的分组               
        //        for (int j = ChildTreeItems[i].ChildTreeItems.Count - 1; j >= 0; j--)
        //        {
        //            if (!grpKeys.Contains(ChildTreeItems[i].ChildTreeItems[j].NodeId) &&
        //                ChildTreeItems[i].ChildTreeItems[j].NodeType == TypeOfSluTreeNode.SluGrp)
        //            {
        //                ChildTreeItems[i].DeleteChild(j);
        //                GrpList.Remove(ChildTreeItems[i].ChildTreeItems[j].NodeId);
        //            }
        //        }
        //    }

        //#endregion
        //    //增加新建的区域
        //    if (!IsLoadOnlyOneArea && Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count > 1)
        //    {
        //        foreach (var v in areas)
        //        {
        //            if (!AreaList.Contains(v.Key))
        //            {
        //                this.ChildTreeItems.Add(new TreeNodeItemViewModel(null, v.Key, 0, TypeOfSluTreeNode.SluArea));
        //                AreaList.Add(v.Key);
        //            }
        //        }
        //    }

        //    //增加新建的分组

        //    foreach (var v in grpKeys)
        //    {
        //        if (!GrpList.Contains(v))
        //        {
        //            ChildTreeItems[v / 1000].ChildTreeItems.Add(new TreeNodeItemViewModel(ChildTreeItems[v / 1000], v / 1000, v,
        //                                                                         TypeOfSluTreeNode.SluGrp));
        //            GrpList.Add(v);
        //        }
        //    }

        //    foreach (var f in ChildTreeItems) f.AddChild();


        //    //for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
        //    //{
        //    //    var t = this.ChildTreeItems[i];
        //    //    t.GetChildRtuCount();
        //    //    if (t.RtuCount == 0)
        //    //    {
        //    //        ChildTreeItems.Remove(t);
        //    //    }
        //    //}

        //    foreach (var f in ChildTreeItems)
        //    {

        //        f.GetChildRtuCount();
        //    }
        //    for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
        //    {
        //        if (this.ChildTreeItems[i].RtuCount == 0 || this.ChildTreeItems[i].ChildTreeItems.Count == 0)
        //        {
        //            this.ChildTreeItems.RemoveAt(i);
        //        }


        //    }
        //}

        #endregion

        #region tab iinterface

        public int Index
        {
            get { return 2; }
        }

        public string Title
        {
            get
            {
                return "本地分组";
            }
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

        private ObservableCollection<TreeNodeBaseNode> _childTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                return _childTreeItemsInfo;
            }
        }

        #region CmdReflesh
        private ICommand _CmdReflesh;

        /// <summary>
        /// 左侧树 添加根节点
        /// </summary>
        public ICommand CmdReflesh
        {
            get { return _CmdReflesh ?? (_CmdReflesh = new RelayCommand(ExAddTree, CanExAddTree, false)); }
        }

        private bool CanExAddTree()
        {
            return true;
        }


        private void ExAddTree()
        {

            LoadNode();

        }

       

        #endregion

    };


    //event
    public partial class TreeMultiViewModelNew
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
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.MulityInfoGroupAllNeedUpdate)
                        return true;
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
                if (args.EventType == PublishEventType.SvAv)
                {
                    Update();
                    return;
                }
                if (args.EventType == PublishEventType.Core)
                {
                    if (args.EventId ==
                        global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
                    {
                        Update();
                    }
                    if (args.EventId ==
                        global::Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.MulityInfoGroupAllNeedUpdate)
                    {
                        Update();
                    }
                    if (args.EventId == EventIdAssign.EquipmentAddEventId)
                    {
                        Update();
                    }
                    if (args.EventId == EventIdAssign.EquipmentDeleteEventId)
                    {
                        Update();
                    }


                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        private bool isvier;

        public bool IsVir
        {
            get { return isvier; }
            set
            {
                if (isvier != value)
                {
                    isvier = value;
                    this.RaisePropertyChanged(() => this.IsVir);
                }
            }
        }
    }



}

