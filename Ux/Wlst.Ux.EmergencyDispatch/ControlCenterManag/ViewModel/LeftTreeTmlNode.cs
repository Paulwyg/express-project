using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Ux.EmergencyDispatch.ControlCenterManag.Services;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag.ViewModel
{
    public class LeftTreeTmlNode:LeftTreeNodeBase
    {
        private static readonly Dictionary<int,LeftTreeTmlNode> RegisterTmlNode=new Dictionary<int, LeftTreeTmlNode>(); 
        public static Dictionary<int,LeftTreeTmlNode> GetRegisterTmlNode()
        {
            return RegisterTmlNode;
        }


        public LeftTreeTmlNode()
        {
            Visi = Visibility.Visible;
            IsGroup = false;
        }

        public LeftTreeTmlNode(LeftTreeNodeBase mvvmFather, int tmlId)
        {
            Visi = Visibility.Visible;
            _father = mvvmFather;
            IsGroup = false;
            NodeId = tmlId;
            GetNodeInfomation();
            if(RegisterTmlNode.ContainsKey(tmlId))
            {
                RegisterTmlNode.Remove(tmlId);
                RegisterTmlNode.Add(tmlId,this);
            }
            else
            {
                RegisterTmlNode.Add(tmlId, this);
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
