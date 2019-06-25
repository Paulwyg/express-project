using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Ux.EmergencyDispatch.ControlCenterManag.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag.ViewModel
{
    public class RightTreeGroupNode:RightTreeNodeBase
    {
        public RightTreeGroupNode()
        {
            IsGroup = true;
        }
        public RightTreeGroupNode(RightTreeNodeBase mvvmfather,int groupId)
        {
            Visi = Visibility.Visible;
            _father = mvvmfather;
            IsGroup = true;
            NodeId = groupId;
            GetNodeInfomation();
            AddChild();
        }

        public override void AddChild()  //将nodes中属于该分组下的节点增加上
        {
            ChildTreeItems.Clear();

            //添加分组到子节点中
            if (!Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(NodeId))
                return;
            foreach (
                var t in
                    Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].LstGrp)
            {
                if (!Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(t))
                    continue;
                if (ControlCenterViewModel.GetChickGroupInfo().ContainsKey(t))
                {
                    ChildTreeItems.Add(new RightTreeGroupNode(this, t));
                }

            }
            //加载终端节点
            foreach (var t in ControlCenterViewModel.GetChickGroupInfo()[NodeId].LstTml)
            {
                if (!Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(t))
                    continue;
                var f = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[t] as IIRtuParaWork;
                if (f == null) continue;

                ChildTreeItems.Add(new RightTreeTmlNode(this, f.RtuId));
            }

        }



        private void GetNodeInfomation()
        {
            if (!Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(NodeId)) return;
            NodeName = Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].GroupName;
        }



    }
}
