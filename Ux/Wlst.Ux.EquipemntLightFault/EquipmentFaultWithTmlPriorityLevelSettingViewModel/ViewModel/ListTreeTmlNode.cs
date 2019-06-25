using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlPriorityLevelSettingViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlPriorityLevelSettingViewModel.ViewModel
{
    public class ListTreeTmlNode : ListTreeNodeBase
    {
        public static Dictionary<int, ListTreeTmlNode> Info = new Dictionary<int, ListTreeTmlNode>();

        public ListTreeTmlNode()
        {
            this.IsGroup = false;
        }

        public ListTreeTmlNode(int tmlId)
        {
            this.IsGroup = false;
            this.NodeId = tmlId;
            PhyId = tmlId;

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
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                    this.NodeId];
            this.NodeName = f.RtuName;
            PhyId = f.RtuPhyId;
        }

    }
}
