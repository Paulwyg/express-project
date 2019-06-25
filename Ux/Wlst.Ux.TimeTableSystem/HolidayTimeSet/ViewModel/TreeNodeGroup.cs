using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



//using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;

namespace Wlst.Ux.TimeTableSystem.HolidayTimeSet.ViewModel
{
   public  class TreeNodeGroup:ListTreeNodeBase 
    {
        public static Dictionary<int, TreeNodeGroup> Info = new Dictionary<int, TreeNodeGroup>();

        private int AreaId ;


        public TreeNodeGroup()
        {
            this.NodeType  = TreeNodeType.Group;

        }

        public TreeNodeGroup(int groupId,int areaid)
        {
            AreaId = areaid;

            this.NodeType = TreeNodeType.Group;
            this.NodeId = groupId;
            this.WuLiId = groupId.ToString("d2");

            if (Info.ContainsKey(groupId)) Info.Remove(groupId);
            Info.Add(groupId, this);
            GetNodeInfomation();
            this.AddChild();
        }

        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public void AddChild()
        {
            ChildTreeItems.Clear();

            ////添加分组到子节点中
            var tu = new Tuple<int, int>(AreaId, NodeId);
            //if (!Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))//EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(NodeId))
            //    return;

            //foreach (var t in Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.Keys)
            //{
            //    if (t.Item1 != AreaId) continue;
            //        ChildTreeItems.Add(new TreeNodeGroup(t.Item2,t.Item1));
            //}

            //var atttmp = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups[tu].LstGrp);
            //foreach (
            //    var t in
            //       atttmp)
            //{
            //    var tu1 = new Tuple<int, int>(AreaId, NodeId); 
            //    if (!Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu1))
            //        continue;
            //    ChildTreeItems.Add(new TreeNodeGroup(t));
            //}

            //加载终端节点

            //var ordtml =
            //  (from t in
            //       Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].LstTml
            //   orderby t ascending
            //   select t).ToList();
            var ordtml = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups[tu].LstTml);

            foreach (
                var t in ordtml)
            {
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                    continue;
                var f =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t] as Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (f == null) continue;

                ChildTreeItems.Add(new TreeNodeRtu(f.RtuId));
            }
        }

        private void GetNodeInfomation()
        {
            var tu = new Tuple<int, int>(AreaId, NodeId);
            if (!Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups.ContainsKey(tu))
                return;
            this.NodeName =
                Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups[tu].GroupName;
        }


    }
}
