using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Ux.EmergencyDispatch.ControlCenterManag.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag.ViewModel
{
    public class LeftTreeGroupNode : LeftTreeNodeBase
    {
        public LeftTreeGroupNode()
        {
            Visi = Visibility.Visible;
            IsGroup  = true;
        }

        public LeftTreeGroupNode(LeftTreeNodeBase mvvmFather, int groupId)
        {
            Visi = Visibility.Visible;
            _father = mvvmFather;
            IsGroup = true;
            NodeId = groupId;
            GetNodeInfomation();
            AddChild();
        }
        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public override void AddChild()
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
                ChildTreeItems.Add(new LeftTreeGroupNode(this, t));
            }
            //加载终端节点
            foreach (
                var t in
                    Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].LstTml)
            {
                if (!Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(t))
                    continue;
                var f =
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[t] as
                    IIRtuParaWork;
                if (f == null) continue;

                ChildTreeItems.Add(new LeftTreeTmlNode(this, f.RtuId));
            }
        }

        private void GetNodeInfomation()
        {
            if (!Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(NodeId)) return;
            NodeName = Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].GroupName;
        }
    }
}
