using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Ux.EmergencyDispatch.ControlCenterManag.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag.ViewModel
{
    public class RightTreeTmlNode:RightTreeNodeBase
    {
        private static Dictionary<int, RightTreeNodeBase> _registerRightNode = new Dictionary<int, RightTreeNodeBase>();
        public static Dictionary<int, RightTreeNodeBase> GetRightTreeTmlNodes()
        {
            return _registerRightNode;
        }
        public static void DeleteTmlNode(int node)
        {
            _registerRightNode.Remove(node);
        }
        public static void ClearRigisterNode()
        {
            _registerRightNode.Clear();
            
        }
        public RightTreeTmlNode()
        {
            IsGroup = false;
            Visi = Visibility.Visible;
            if (!_registerRightNode.ContainsKey(NodeId) && !IsGroup)
            {
                _registerRightNode.Add(NodeId, this);
            }
        }
        public RightTreeTmlNode(RightTreeNodeBase father,int nodeid)
        {
            Visi = Visibility.Visible;
            _father = father;
            IsGroup = false;
            NodeId = nodeid;
            GetNodeInfomation();
            if (_registerRightNode.ContainsKey(NodeId))
            {
                _registerRightNode.Remove(NodeId);
                _registerRightNode.Add(NodeId, this);
            }
            else if(!_registerRightNode.ContainsKey(NodeId))
            {
                _registerRightNode.Add(NodeId, this);
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
