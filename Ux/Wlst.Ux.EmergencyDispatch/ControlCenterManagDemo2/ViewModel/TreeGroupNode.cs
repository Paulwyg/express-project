using System.Collections.Generic;
using System.Windows;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.ViewModel
{
    public class TreeGroupNode:TreeNodeBase
    {
        public TreeGroupNode()
        {
            IsGroup = true;
            IsShowSelectedCheckBox = false;
        }
 
        public TreeGroupNode(int areaId,int groupId)
        {
            AreaId = areaId;
            if(Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(areaId))
            {
                AreaName = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
            }else
            {
                AreaName = "未知";
            }
            
            Visi = Visibility.Visible;
            //_father = mvvmfather;
            IsGroup = true;
            IsShowSelectedCheckBox = false;
            NodeId = groupId;
            PhysicalId = groupId;
            AddChild();
        }

        public override void AddChild()  //将nodes中属于该分组下的节点增加上
        {
            ChildTreeItems.Clear();

            var rtuLst = new List<int>();
            if (NodeId == 0)
            {
                rtuLst = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuNotInAnyGroup(AreaId);
                NodeName =AreaId + "-特殊终端";
            }
            else
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                             NodeId);
                if (grp == null) return;
                NodeName = AreaId + "-"+grp.GroupName;
                rtuLst = grp.LstTml;
            }
            if (rtuLst.Count == 0) return;

            //加载终端节点
            var ntssss = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(rtuLst);

            foreach (
                var t in ntssss)
            {
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                    continue;
                var f = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                if (f == null || f.EquipmentType != WjParaBase.EquType.Rtu) continue;

                //if (TreeTmlNode.RegisterTmlNode.ContainsKey(f.RtuId))
                //    ChildTreeItems.Add(TreeTmlNode.RegisterTmlNode[f.RtuId]);
                //else
                    ChildTreeItems.Add(new TreeTmlNode(this, f.RtuId));
            }

        }



    }
}
