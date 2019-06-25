using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.Services;
using Wlst.client;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2.ViewModel
{
    public class TreeTmlNode : TreeNodeBase
    {
        private static Dictionary<int, List<TreeNodeBase>> _registerTmlNode = new Dictionary<int, List<TreeNodeBase>>();

        public static Dictionary<int, List<TreeNodeBase>> RegisterTmlNode
        {
            get { return _registerTmlNode; }
        }

        public static bool ClearRegisterTmlNodes()
        {
            try
            {
                _registerTmlNode.Clear();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static List< int > GetNodeChecked()
        {
            var rtn = new List<int>();
            foreach (var f in RegisterTmlNode )
            {
                foreach (var l in f.Value )
                {
                    if (l.IsChecked && rtn.Contains(l.NodeId) == false) rtn.Add(l.NodeId);
                }
            }
            return rtn;
        }

        public static List<int> GetNodeKxChecked(int kx)
        {
            var rtn = new List<int>();
            foreach (var f in RegisterTmlNode)
            {
                
                foreach (var l in f.Value)
                {
                    if (l.IsRtuUsed == false) continue;
                    if (l.IsChecked == false) continue;
                    if (rtn.Contains(l.NodeId)) continue;

                    if (kx == 1)
                    {
                        if (l.IsSwitch1Checked && l.K1OpenOrCloseAns == EnumOpenOrCloseAns.NoAnswer) rtn.Add(l.NodeId);
                    }
                    else if (kx == 2)
                    {
                        if (l.IsSwitch2Checked && l.K2OpenOrCloseAns == EnumOpenOrCloseAns.NoAnswer) rtn.Add(l.NodeId);
                    }
                    else if (kx == 3)
                    {
                        if (l.IsSwitch3Checked && l.K3OpenOrCloseAns == EnumOpenOrCloseAns.NoAnswer) rtn.Add(l.NodeId);
                    }
                    else if (kx == 4)
                    {
                        if (l.IsSwitch4Checked && l.K4OpenOrCloseAns == EnumOpenOrCloseAns.NoAnswer) rtn.Add(l.NodeId);
                    }
                    else if (kx == 5)
                    {
                        if (l.IsSwitch5Checked && l.K5OpenOrCloseAns == EnumOpenOrCloseAns.NoAnswer) rtn.Add(l.NodeId);
                    }
                    else if (kx == 6)
                    {
                        if (l.IsSwitch6Checked && l.K6OpenOrCloseAns == EnumOpenOrCloseAns.NoAnswer) rtn.Add(l.NodeId);
                    }
                    else if (kx == 7)
                    {
                        if (l.IsSwitch7Checked && l.K7OpenOrCloseAns == EnumOpenOrCloseAns.NoAnswer) rtn.Add(l.NodeId);
                    }
                    else if (kx == 8)
                    {
                        if (l.IsSwitch8Checked && l.K8OpenOrCloseAns == EnumOpenOrCloseAns.NoAnswer) rtn.Add(l.NodeId);
                    }


                }
            }
            return rtn;
        }

        public static List<TreeNodeBase > GetAnykxChecked( )
        {
            
            var dic = new Dictionary< int ,TreeNodeBase>();
            foreach (var f in RegisterTmlNode)
            {
                foreach (var l in f.Value)
                {
                    if (l.IsRtuUsed == false) continue;
                    if (l.IsChecked == false) continue;
                    if (dic.ContainsKey(l.NodeId)) continue;

                    if (l.IsSwitch1Checked || l.IsSwitch2Checked || l.IsSwitch3Checked || l.IsSwitch4Checked ||
                        l.IsSwitch5Checked || l.IsSwitch6Checked || l.IsSwitch7Checked || l.IsSwitch8Checked)
                        dic.Add(l.NodeId, l);

                    //if (l.IsSwitch1Checked) selectSwitchOutCount++;
                    //if (l.IsSwitch2Checked) selectSwitchOutCount++;
                    //if (l.IsSwitch3Checked) selectSwitchOutCount++;
                    //if (l.IsSwitch4Checked) selectSwitchOutCount++;
                    //if (l.IsSwitch5Checked) selectSwitchOutCount++;
                    //if (l.IsSwitch6Checked) selectSwitchOutCount++;
                    //if (l.IsSwitch7Checked) selectSwitchOutCount++;
                    //if (l.IsSwitch8Checked) selectSwitchOutCount++;

                }
            }
            return  (from t in dic select t.Value ).ToList() ;
        }

        public static List<TreeNodeBase> GetAnyChecked()
        {
            var dic = new Dictionary<int, TreeNodeBase>();
            foreach (var f in RegisterTmlNode)
            {
                foreach (var l in f.Value)
                {
                    if (l.IsRtuUsed == false) continue;
                    if (l.IsChecked == false) continue;
                    if (dic.ContainsKey(l.NodeId)) continue;
                    dic.Add(l.NodeId, l);

                }
            }
            return (from t in dic select t.Value).ToList();
        }

        public TreeTmlNode()
        {
            IsGroup = false;
            Visi = Visibility.Visible;
        }

       
        public TreeTmlNode(TreeNodeBase father, int nodeid)
        {
            IsRtuUsed = false ;

          
            Visi = Visibility.Visible;
            _father = father;
            IsGroup = false;
            NodeId = nodeid;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(nodeid);
            //if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(areaId))
            //{
            //    AreaName = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
            //}
            //else
            //{
            //    AreaName = "未知";
            //}
            var infox = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(nodeid);

            if (infox != null)
            {
                IsRtuUsed = infox.RtuStateCode == 2;
                if (infox.RtuModel == EnumRtuModel.Wj3006)
                {
                    Is3006 = true;
                }

                PhysicalId = infox.RtuPhyId;
                NodeName = infox.RtuName;
                switch (infox.RtuStateCode)
                {
                    case 0:
                        State = EnumTmlState.NotUse;
                        break;
                    case 1:
                        State = EnumTmlState.Disable;
                        break;
                    default:
                        State = EnumTmlState.Use;
                        break;
                }

            }


            if (!_registerTmlNode.ContainsKey(NodeId))
            {
                _registerTmlNode.Add(NodeId, new List<TreeNodeBase>());
            }

            _registerTmlNode[NodeId].Add(this);
        }

    }
}
