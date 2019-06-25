using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Services;

namespace Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.ViewModel
{
    public class TreeGroupNode : TreeNodeBase
    {
        public TreeGroupNode()
        {
            IsGroup = true;
            IsShowSelectedCheckBox = false;
        }

        public List<int> areas;
        public TreeGroupNode(int areaId, int groupId)
        {
            areas = new List<int>();
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW)
            {
                if (areas.Contains(f) == false) areas.Add(f);
            }

            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    if (areas.Contains(f) == false) areas.Add(f);
                }
            }

            if (areaId != -1)
            {
                AreaId = areaId;
                if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(areaId))
                {
                    AreaName = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
                }
                else
                {
                    AreaName = "未知";
                }
            }
            else
            {
                AreaId = 0;
                AreaName = "全部区域";
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
                if (areas.Count > 1)
                {
                    // NodeName =AreaId + "-特殊终端"; 
                    NodeName = "特殊终端";
                }
                else
                {
                    NodeName = "特殊终端";
                }

            }
            else if (NodeId == -1)
            {                
                foreach (var tt in areas)
                {
                    rtuLst.AddRange( Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(tt)); 
                }
                if (areas.Count > 1)
                {
                    // NodeName =AreaId + "-特殊终端"; 
                    NodeName = "全部终端";
                }
                else
                {
                    NodeName = "全部终端";
                }
            }
            else
            {
                var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId,
                                                                                                             NodeId);
                if (grp == null) return;
                if (areas.Count > 1)
                {
                    // NodeName = AreaId + "-" + grp.GroupName;
                    NodeName = grp.GroupName;
                }
                else
                {
                    NodeName = grp.GroupName;
                }
                rtuLst = grp.LstTml;
            }
            if (rtuLst.Count == 0) return;

            rtuLst = (from t in rtuLst orderby t select t).ToList();

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
