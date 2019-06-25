using System.Collections.Concurrent;
using System.Windows;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.Services;

namespace Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.ViewModels
{
    public class ListTreeGroupNode : ListTreeNodeBase
    {
      public static ConcurrentDictionary<int, ListTreeGroupNode> Info = new ConcurrentDictionary<int, ListTreeGroupNode>();
        


        public ListTreeGroupNode()
        {
            Visi = Visibility.Visible;
            IsGroup  = true;
        }

        public ListTreeGroupNode(ListTreeGroupNode mvvmFather, int groupId)
        {
            Visi = Visibility.Visible;
            _father = mvvmFather;
            IsGroup = true;
            NodeId = groupId;
            GetNodeInfomation();
            AddChild();
            if (!Info.ContainsKey(this.NodeId)) Info.TryAdd(this.NodeId, this);
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
                ChildTreeItems.Add(new ListTreeGroupNode(this, t));
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

                ChildTreeItems.Add(new ListTreeTmlNode(this, f.RtuId));
            }
        }

        private void GetNodeInfomation()
        {
            if (!Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary.ContainsKey(NodeId)) return;
            NodeName = Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoDictionary[NodeId].GroupName;
        }


    }
}
