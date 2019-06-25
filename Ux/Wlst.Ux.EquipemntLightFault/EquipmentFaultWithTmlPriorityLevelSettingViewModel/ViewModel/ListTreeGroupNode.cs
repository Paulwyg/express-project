using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlPriorityLevelSettingViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlPriorityLevelSettingViewModel.ViewModel
{
    public class ListTreeGroupNode : ListTreeNodeBase
    {
        public static Dictionary<int, ListTreeGroupNode> Info = new Dictionary<int, ListTreeGroupNode>();

        public ListTreeGroupNode()
        {
            this.IsGroup = true;

        }

        /// <summary>
        /// grp 为0 表示特殊终端
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="groupId"></param>
        public ListTreeGroupNode(int areaId, int groupId)
        {
            this.IsGroup = true;
            this.NodeId = groupId;
            PhyId = groupId;
            AreaId = areaId;
            if (Info.ContainsKey(groupId)) Info.Remove(groupId);
            Info.Add(groupId, this);
            this.AddChild();
        }

        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public void AddChild()
        {
            ChildTreeItems.Clear();
            var  ordtml = new List<int>();
            if (NodeId ==0)
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                if (grp.Count == 0) return;
                this.NodeName = "特殊终端";
                ordtml = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp);
            }else
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId, NodeId);
                if (grp == null) return;
                this.NodeName = grp.GroupName;
                ordtml = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);
            }
            

            //加载终端节点

           

            foreach (var t in ordtml)
            {
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                    continue;
                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t].EquipmentType !=
                    WjParaBase.EquType.Rtu)
                    continue;
                ChildTreeItems.Add(new ListTreeTmlNode(t));
            }
        }


        public override void UpdateNodeSelected()
        {
            //base.UpdateNodeSelected();
            foreach (var t in ChildTreeItems )
            {
                if (t.IsGroup) t.UpdateNodeSelected();
            }

            bool findtrue = false;
            foreach (var t in ChildTreeItems)
            {
                if (t.IsSelected)
                {
                    findtrue = true;
                    break;
                }
            }
            this.IsSelected = findtrue;
        }

        public override void UpdateNoAlarmSelected()
        {
            //base.UpdateNodeSelected();
            foreach (var t in ChildTreeItems)
            {
                if (t.IsGroup) t.UpdateNoAlarmSelected();
            }

            bool findtrue = false;
            foreach (var t in ChildTreeItems)
            {
                if (t.IsNoAlarm)
                {
                    findtrue = true;
                    break;
                }
            }
            this.IsNoAlarm = findtrue;
        }

    }
}
