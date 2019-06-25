using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.BseVm;
using Wlst.client;
using EventIdAssign = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.vms
{
    /// <summary>
    /// 提供终端树节点基本结构
    /// 右键菜单从MenuBuliding中动态获取，为节约程序资源，仅当点击该终端时该终端的右键菜单立即刷新
    /// IsMarked为联动标记，初始化时必须在外部初始化，即某一个分组具有联动则其子分组具有该联动属性
    /// </summary>
    public class TreeNodeItemSingleGroupViewModel : TreeNodeBaseNode 
    {
        private static TreeNodeBaseNode _currentSelectGroupNode;

        public static TreeNodeBaseNode CurrentSelectGroupNode
        {
            get { return _currentSelectGroupNode; }
            set
            {
                if (_currentSelectGroupNode == value) return;
                if (value == null) return;
                if (value.NodeType == TypeOfTabTreeNode.IsGrp ||
                    value.NodeType == TypeOfTabTreeNode.IsGrpSpecial ||
                    value.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    if (_currentSelectGroupNode != null)
                        _currentSelectGroupNode.IsSelected = false;
                    _currentSelectGroupNode = value;
                    BaseNodes.CurrentSelectedGrpIdChanged(value );
                    //return;
                    //if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;
                    //var ins = new PublishEventArgs()
                    //              {
                    //                  EventType = PublishEventType.Core,
                    //                  EventId =
                    //                      Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
                    //              };

                    //var info = new List<int>();
                    //if (value.NodeType == TypeOfTabTreeNode.IsAll)
                    //{

                    //    info.Add(-1);
                    //    ins.AddParams(info);
                    //}
                    //else if (value.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
                    //{
                    //    info = (from t in value.ChildTreeItems select t.NodeId).ToList();
                    //    ins.AddParams(info);
                    //}
                    //else
                    //{
                    //    info =
                    //        Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(
                    //            value.NodeId);
                    //    ins.AddParams(info);
                    //}
                    //if (info.Count == 0) return;
                    //if (info.Count == 1 && info[0] == -1) info.Clear();
                    //EventPublisher.EventPublish(ins);
                }

            }
        }

        public TreeNodeItemSingleGroupViewModel()
        {
            this.NodeType = TypeOfTabTreeNode.IsGrp;
            Visi = Visibility.Visible;
        }

        public TreeNodeItemSingleGroupViewModel(TreeNodeBaseNode mvvmFather, GroupItemsInfo .GroupItem  groupInfomatioin)
        {
            this.NodeType = TypeOfTabTreeNode.IsGrp;
            Visi = Visibility.Visible;
            this._father = mvvmFather;
            //TreeSingleViewModel.RegisterNodeToControl(this);


            if (groupInfomatioin == null)
            {
                this.NodeName = "GroupInfo Error";
                return;
            }
            this.NodeName = groupInfomatioin.GroupName;
            this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
            this.NodeId = groupInfomatioin.GroupId;
            this.AddChild();

        }

        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public override void AddChild()
        {
            ChildTreeItems.Clear();
            if (NodeType != TypeOfTabTreeNode.IsGrp) return;

            //lvf 2018年4月17日14:37:13 默认区域id为0
            var tu = new Tuple<int, int>(0, NodeId);

            //添加分组到子节点中
            //if (!ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
            //    return;


            //var tmps =
            //    (from gt in
            //         ServicesGrpSingleInfoHold.InfoGroups[tu].LstGrp
            //     orderby gt ascending
            //     select gt).ToList();
          
            //var ntss = ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(tmps);

            //foreach (var t in ntss)
            //{
            //    if (!ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(t))
            //        continue;
                //ChildTreeItems.Add(new TreeNodeItemSingleGroupViewModel(this,
            //                                                            ServicesGrpSingleInfoHold.InfoGroups[
            //                                                                    tu]));
            ////}
            ////对分组子节点 进行数据加载
            //foreach (var t1 in ChildTreeItems)
            //{
            //    t1.AddChild();
            //}
            ///加载终端节点
            var tmpssssss =
                (from gt in
                     ServicesGrpSingleInfoHold.InfoGroups[tu].LstTml
                 orderby gt ascending
                 select gt).ToList();

            var ntsss = ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(tmpssssss);

            var LST = new List<TreeNodeBaseNode>();
            foreach (var t in ntsss)
            {
                if (!BaseNodes.Nodess.ContainsKey(t))
                {
                    if (
                        ! Wlst .Sr .EquipmentInfoHolding .Services .EquipmentDataInfoHold .InfoItems  .ContainsKey(
                            t))
                        continue;
                    var f =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems [t];
                    new TreeNodeItemTmlViewModel(this, f);

                }
                if (!BaseNodes.Nodess.ContainsKey(t)) continue;
                //  ChildTreeItems.Add(GrpComSingleMuliViewModel.BaseNodes.Nodess[t]);
                LST.Add(BaseNodes.Nodess[t]);
                continue;
            }
          //  var lstord = (from t in LST orderby t.PhyId ascending select t).ToList();
            foreach (var t in LST) ChildTreeItems.Add(t);
        }


        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点  updateArgu wuyong
        /// </summary>
        public override void ReUpdate(int updateArgu)
        {
            //lvf 2018年4月17日14:37:13 默认区域id为0
            var tu = new Tuple<int, int>(0, NodeId);

            //添加分组到子节点中
            if (!ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
            {
                if (_father != null) _father.ChildTreeItems.Remove(this);
                return;
            }
            this.NodeName =
                ServicesGrpSingleInfoHold.InfoGroups[tu].GroupName;

            ////////////////node delete
            //////////////for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            //////////////{
            //////////////    if (ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsGrp)
            //////////////    {
            //////////////        if (
            //////////////            !ServicesGrpSingleInfoHold.InfoGroups[tu].
            //////////////                 LstGrp.Contains(
            //////////////                     ChildTreeItems[i].NodeId))

            //////////////            if (!Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
            //////////////        {
            //////////////            this.ChildTreeItems.RemoveAt(i);
            //////////////        }
            //////////////    }
            //////////////    else if (ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsTml)
            //////////////    {
            //////////////        if (
            //////////////            !ServicesGrpSingleInfoHold.InfoGroups[tu].
            //////////////                 LstTml.Contains(
            //////////////                     ChildTreeItems[i].NodeId) || 
            //////////////                     !Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(ChildTreeItems[i].NodeId))
            //////////////            //!ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
            //////////////            //    ChildTreeItems[i].NodeId))
            //////////////        {
            //////////////            this.ChildTreeItems.RemoveAt(i);
            //////////////        }
            //////////////        else
            //////////////        {
            //////////////            //if()
            //////////////        }
            //////////////    }
            //////////////}
            //////////////grp  add and update
            ////////////foreach (
            ////////////    var t in
            ////////////        ServicesGrpSingleInfoHold.InfoGroups[NodeId].LstGrp)
            ////////////{
            ////////////    bool bolfind = false;
            ////////////    foreach (var ff in this.ChildTreeItems)
            ////////////    {
            ////////////        if (ff.NodeType == TypeOfTabTreeNode.IsGrp && ff.NodeId == t)
            ////////////        {
            ////////////            bolfind = true;
            ////////////            ff.ReUpdate(0);
            ////////////            break;
            ////////////        }
            ////////////    }
            ////////////    if (!bolfind)
            ////////////    {
            ////////////        if (
            ////////////            !ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(t))
            ////////////            continue;
            ////////////        var addgrp = new TreeNodeItemSingleGroupViewModel(this,
            ////////////                                                          ServicesGrpSingleInfoHold.
            ////////////                                                              InfoGroups[t]);
            ////////////        this.ChildTreeItems.Insert(0, addgrp);
            ////////////        //addgrp.AddChild();
            ////////////    }
            ////////////}

            //tml  add and update


   

            var tmpssssss =
                (from gt in
                     ServicesGrpSingleInfoHold.InfoGroups[tu].LstTml
                 orderby gt ascending
                 select gt).ToList();
            foreach (var t in tmpssssss)
            {
                bool bolfind = false;
                foreach (var ff in this.ChildTreeItems)
                {
                    if (ff.NodeType == TypeOfTabTreeNode.IsTml && ff.NodeId == t)
                    {
                        bolfind = true;
                        break;
                    }
                }
                if (!bolfind)
                {
                    //if (!Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .ContainsKey(t)) continue;
                    //var ff =
                    //    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary 
                    //        [t];
                    //ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, ff));

                    if (!BaseNodes.Nodess.ContainsKey(t))
                    {
                        if (
                            !Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems .
                                 ContainsKey(
                                     t))
                            continue;
                        var f =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems [t];
                        if (f.RtuFid  != 0) continue;
                        new TreeNodeItemTmlViewModel(this, f);
                    }
                    if (!BaseNodes.Nodess.ContainsKey(t)) continue;
                    ChildTreeItems.Add(BaseNodes.Nodess[t]);
                    continue;

                }
            }

            var lst = new List<int>();
            //var tu = new Tuple<int, int>(0, NodeId);

            lst.AddRange(
               ServicesGrpSingleInfoHold.InfoGroups[tu].LstTml);
            //lst.AddRange(
            //    ServicesGrpSingleInfoHold.InfoGroups[tu].LstGrp);


            //var ntss = Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(lst);
            var ntss = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(lst);
            var indexed = new List<TreeNodeBaseNode>();
            var dir = new Dictionary<int, TreeNodeBaseNode>();
            
            foreach (var g in this.ChildTreeItems)
            {
                if (!dir.ContainsKey(g.NodeId)) dir.Add(g.NodeId, g);
            }
            foreach (var t in ntss)
            {
                if (dir.ContainsKey(t)) indexed.Add(dir[t]);
            }
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
        }




        #region Node Select

        /// <summary>
        /// 当选择的终端发送变化时，如果 
        /// </summary>
        public override void OnNodeSelectActive()
        {
            //base.OnNodeSelect();
            //发布事件  选中当前节点
            var args = new PublishEventArgs
                           {
                               EventType = PublishEventType.Core,
                               EventId = EventIdAssign.GroupSelected,
                           };
            args.AddParams(NodeId);

            EventPublisher.EventPublish(args);

            //  base.OnNodeSelectActive();
            TreeNodeItemSingleGroupViewModel.CurrentSelectGroupNode = this;

            // ResetContextMenu();
        }


        #region  Reset ContextMenu
        public override  void ResetContextMenu()
        {
            ResetCm();
        }

        public void  ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            //if (Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(NodeId))
            //    t = MenuBuilding.BulidCm(Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].MenuRightTargetKey,
            //                             false,
            //                             Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId]);

            var tu = new Tuple<int, int>(0, NodeId);
            if (ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
                t = MenuBuilding.BulidCm("RightMenuSingle",
                                         false,
                                         ServicesGrpSingleInfoHold.InfoGroups[tu]);
            CmItems = t;
        }

        #endregion

        #endregion

    }
}
