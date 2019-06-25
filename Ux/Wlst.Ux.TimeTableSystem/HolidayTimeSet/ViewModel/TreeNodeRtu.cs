using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.TimeTableSystem.HolidayTimeSet.ViewModel
{
   public class TreeNodeRtu:ListTreeNodeBase
    {
        public static Dictionary<int, TreeNodeRtu> Info = new Dictionary<int, TreeNodeRtu>(); 
        public TreeNodeRtu()
        {
            this.NodeType   = TreeNodeType.Rtu ;
        }

        public TreeNodeRtu(int tmlId)
        {
            this.NodeType = TreeNodeType.Rtu;
            this.NodeId = tmlId;
            this.WuLiId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tmlId].RtuPhyId.ToString("d4");

            if (Info.ContainsKey(tmlId)) Info.Remove(tmlId);
            Info.Add(tmlId, this);

            GetNodeInfomation();
        }

        private void GetNodeInfomation()
        {
            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                     ContainsKey
                     (this.NodeId)) return;
            var f =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems [
                    this.NodeId];
            this.NodeName = f.RtuName;
        }



    }


}
