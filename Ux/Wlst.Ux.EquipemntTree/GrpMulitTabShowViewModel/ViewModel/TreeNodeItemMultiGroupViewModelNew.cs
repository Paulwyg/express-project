using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.EquipemntTree.GrpComSingleMuliViewModel;
using Wlst.Ux.EquipemntTree.Models;
using Wlst.Ux.EquipemntTree.Resources;

namespace Wlst.Ux.EquipemntTree.GrpMulitTabShowViewModel.ViewModel
{
    public class TreeNodeItemMultiGroupViewModelNew : TreeNodeBaseNode
    {
        private static TreeNodeBaseNode _currentSelectGroupNode;

        public static TreeNodeBaseNode CurrentSelectGroupNode
        {
            get { return _currentSelectGroupNode; }
            set
            {
                if (_currentSelectGroupNode == value) return;
                if (value == null) return;
                if (value.NodeType == TypeOfTabTreeNode.IsTml) return;
                if (value.NodeType == TypeOfTabTreeNode.IsGrp ||
                    value.NodeType == TypeOfTabTreeNode.IsArea )
                {
                    if (_currentSelectGroupNode != null)
                        _currentSelectGroupNode.IsSelected = false;
                    _currentSelectGroupNode = value;


                    if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;
                    var ins = new PublishEventArgs()
                    {
                        EventType = PublishEventType.Core,
                        EventId =
                            Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
                    };

                    var info = new List<int>();
                    if (value.NodeType == TypeOfTabTreeNode.IsArea)
                    {

                        info.Add(-1);
                        ins.AddParams(info);
                    }
                    else
                    {
                        info = (from t in value.ChildTreeItems select t.NodeId).ToList();
                        ins.AddParams(info);
                    }

                    if (info.Count == 0) return;
                    if (info.Count == 1 && info[0] == -1) info.Clear();
                    EventPublish.PublishEvent(ins);
                }

            }
        }

        public TreeNodeItemMultiGroupViewModelNew(TreeNodeBaseNode mvvmFather, int areaId, int groupId, TypeOfTabTreeNode type)
        {
            
            this.AreaId = areaId;
            this.NodeType = type;
            this._father = mvvmFather;

            string nodename = "--";
           // NodeName = "--";

            if (type == TypeOfTabTreeNode.IsArea)
            {
                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = areaId;
                var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef;
                if (areaId == -1)
                {
                    nodename = "全部区域";
                }
                else
                {
                    foreach (var f in areaInfo.AreaInfo)
                    {
                        if (f.Value.AreaId == areaId)
                        {
                            nodename = f.Value.AreaName;
                        }
                    }
                }
            }
            if (type == TypeOfTabTreeNode.IsGrp)
            {
                var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.GetGroupInfomation(AreaId,
                                                                                                                groupId);
                if (info != null)
                {
                    nodename = info.GroupName;

                }

                this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
                this.NodeId = groupId;
            }

            this.NodeName = nodename;
            this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
            this.AddChild();

        }

        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public override void AddChild()
        {
            ChildTreeItems.Clear();

            if (NodeType == TypeOfTabTreeNode.IsGrp)
            {
                if (AreaId != -1)
                {
                    var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                    var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.GetGroupInfomation(AreaId,
                                                                                                                 NodeId);
                    if (grp == null) return;
                    var gprs = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);
                    foreach (var f in gprs)
                    {
                        if (tmlLstOfArea.Contains(f) == false) continue;
                        var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                        if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;
                        this.ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));
                    }
                }
                else
                {
                    var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.GetGroupInfomation(AreaId,
                                                                                                                 NodeId);
                    if (grp == null) return;
                    var gprs = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);
                    foreach (var f in gprs)
                    {
                        var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
                        if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;
                        this.ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));
                    }
                }
            }
            
            if (NodeType == TypeOfTabTreeNode.IsArea)
            {
                var grp =
                    (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.ItemsMultiGrp
                     where t.Key.Item1 == AreaId
                     orderby t.Value.Index
                     select t.Value).ToList();

                foreach (var f in grp)
                {
                    var rtuList = new List<int>();
                    foreach (var fff in f.LstTml)
                    {
                        if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                            continue;
                        rtuList.Add(fff);
                    }
                    if (rtuList.Count < 1) continue;
                    //if (f.LstTml.Count < 1) continue;
                    this.ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(this, f.AreaId, f.GroupId,
                                                                                 TypeOfTabTreeNode.IsGrp));
                }
               
            }

        }

        public override void ReUpdate(int updateArgu)
        {
            if (NodeType == TypeOfTabTreeNode.IsGrp)
                this.UpdateGroup();            
            if (NodeType == TypeOfTabTreeNode.IsArea)
                this.UpdateArea();
            foreach (var f in ChildTreeItems) f.GetChildRtuCount();
        }

        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点  
        /// </summary>
        public void UpdateGroup()
        {
            var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.GetGroupInfomation(AreaId, NodeId);
            if (info == null)
            {
                this.ChildTreeItems.Clear();
                if (_father != null) _father.ChildTreeItems.Remove(this);
                return;
            }

            this.NodeName = info.GroupName;


            //node delete
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeType != TypeOfTabTreeNode.IsTml)
                {
                    this.ChildTreeItems.RemoveAt(i);
                    continue;
                }

                if (info.LstTml.Contains(ChildTreeItems[i].NodeId) == false ||
                    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                        ChildTreeItems[i].NodeId))
                {
                    this.ChildTreeItems.RemoveAt(i);
                }
            }

            //tml  add and update
            var exist = (from t in ChildTreeItems select t.NodeId).ToList();
            foreach (var t in info.LstTml)
            {
                if (exist.Contains(t)) continue;

                var para = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t);
                if (para == null || para.EquipmentType != WjParaBase.EquType.Rtu) continue;

                if (para.RtuFid != 0) continue;
                ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, para));
            }          
        }

        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点  
        /// </summary>
        public void UpdateArea()
        {
            var info = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(this.AreaId);
            //.Values.ToList();
            if (info == null)
            {
                this.ChildTreeItems.Clear();
                if (_father != null) _father.ChildTreeItems.Remove(this);
                return;
            }

            var nodename = "";
            nodename = info.AreaName;

            if(this.AreaId == -1)
            {
                nodename = "全部区域";
            }

            var gprlst = (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.ItemsMultiGrp
                          where t.Key.Item1 == AreaId
                          select t.Key.Item2).ToList();
            //var spe = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
            //if(spe .Count >0) 
            //   gprlst.Add(0);

            
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
            foreach (var t in info.LstTml)
            {
                if (exist.Contains(t))
                {
                    lstUp.Add(t);
                    continue;
                }

                var para = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId, t);
                if (para == null) continue;

                if (para.LstTml.Count == 0) continue;
                ChildTreeItems.Add(new TreeNodeItemMultiGroupViewModelNew(this, AreaId, t, TypeOfTabTreeNode.IsGrp));
            }

            foreach (var f in this.ChildTreeItems)
            {
                if (!lstUp.Contains(f.NodeId)) continue;
                f.ReUpdate(0);
            }

            this.NodeName = nodename;
        }

        #region Node Select
        public override void OnNodeSelectActive()
        {
            base.OnNodeSelectActive();
            CurrentSelectGroupNode = this;
        }


        #region  Reset ContextMenu
        public override void ResetContextMenu()
        {
            ResetCm();
        }

        public void ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (NodeType != TypeOfTabTreeNode.IsGrp) return;
            var info = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.GetGroupInfomation(AreaId, NodeId);
            if (info == null) return;
               this.CmItems = MenuBuilding.BulidCm("RightMenuMulit",false, info);

           
        }

        #endregion
        #endregion
    }
}
