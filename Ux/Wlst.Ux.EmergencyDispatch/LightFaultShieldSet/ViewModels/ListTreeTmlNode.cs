using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;
using Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.Services;

namespace Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.ViewModels
{
    public class ListTreeTmlNode : ListTreeNodeBase
    {
        private static readonly Dictionary<int, ListTreeTmlNode> RegisterNodes = new Dictionary<int, ListTreeTmlNode>();

        public static Dictionary<int ,ListTreeTmlNode> GetRegisterNodes()
        {
            return RegisterNodes;
        }

        public static void ClearRegisterNodes()
        {
            RegisterNodes.Clear();
        }

        public ListTreeTmlNode()
        {
            Visi = Visibility.Visible;
            IsGroup = false;
        }

        public ListTreeTmlNode(ListTreeNodeBase mvvmFather, int tmlId)
        {
            Visi = Visibility.Visible;
            _father = mvvmFather;
            IsGroup = false;
            NodeId = tmlId;
            GetNodeInfomation();
            if (!RegisterNodes.ContainsKey(NodeId))
            {
                RegisterNodes.Add(NodeId, this);
            }
            else
            {
                RegisterNodes.Remove(NodeId);
                RegisterNodes.Add(NodeId, this);
            }
        }

        private void GetNodeInfomation()
        {
            if (
                !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.
                     ContainsKey
                     (NodeId)) return;
            var f = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[NodeId];
            NodeName = f.RtuName;
        }

    }
}
