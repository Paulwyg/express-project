using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.Services;
using Wlst.client;

namespace Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter.ViewModel
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

        public static List< int > GetNodeChecked(bool isMeasure)
        {
            var rtn = new List<int>();
            foreach (var f in RegisterTmlNode )
            {
                foreach (var l in f.Value )
                {
                    if (isMeasure)
                    {
                        if (l.State == EnumTmlState.NotUse ) continue;
                    }
                    else
                    {
                        if (l.State != EnumTmlState.Use) continue;
                    }
                    if (l.IsChecked && rtn.Contains(l.NodeId) == false) rtn.Add(l.NodeId);
                }
            }
            return rtn;

        }

        public static List<int> GetNodeKxChecked(int kx,bool isMeasure)
        {
            var rtn = new List<int>();
            foreach (var f in RegisterTmlNode)
            {
                
                foreach (var l in f.Value)
                {
                    //if (l.IsRtuUsed == false) continue;
                    if (isMeasure)
                    {
                        if (l.State == EnumTmlState.NotUse) continue;
                    }
                    else
                    {
                        if (l.State != EnumTmlState.Use) continue;
                    }
                    if (l.IsChecked == false) continue;
                    if (rtn.Contains(l.NodeId)) continue;

                    if (kx == 1)
                    {
                        if (l.IsSwitch1Checked) rtn.Add(l.NodeId);
                    }
                    else if (kx == 2)
                    {
                        if (l.IsSwitch2Checked) rtn.Add(l.NodeId);
                    }
                    else if (kx == 3)
                    {
                        if (l.IsSwitch3Checked ) rtn.Add(l.NodeId);
                    }
                    else if (kx == 4)
                    {
                        if (l.IsSwitch4Checked ) rtn.Add(l.NodeId);
                    }
                    else if (kx == 5)
                    {
                        if (l.IsSwitch5Checked ) rtn.Add(l.NodeId);
                    }
                    else if (kx == 6)
                    {
                        if (l.IsSwitch6Checked ) rtn.Add(l.NodeId);
                    }
                    else if (kx == 7)
                    {
                        if (l.IsSwitch7Checked) rtn.Add(l.NodeId);
                    }
                    else if (kx == 8)
                    {
                        if (l.IsSwitch8Checked ) rtn.Add(l.NodeId);
                    }


                }
            }
            return rtn;
        }

        public static List<int> GetNodeKxNoAnswer(int kx)
        {
            var rtn = new List<int>();
            foreach (var f in RegisterTmlNode)
            {

                foreach (var l in f.Value)
                {
                    //if (l.IsRtuUsed == false) continue;
                    if (rtn.Contains(l.NodeId)) continue;

                    if (kx == 1)
                    {
                        if (l.K1SelectionTestAns==EnumSelectionTestAns.Ready) rtn.Add(l.NodeId);
                    }
                    else if (kx == 2)
                    {
                        if (l.K2SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(l.NodeId);
                    }
                    else if (kx == 3)
                    {
                        if (l.K3SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(l.NodeId);
                    }
                    else if (kx == 4)
                    {
                        if (l.K4SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(l.NodeId);
                    }
                    else if (kx == 5)
                    {
                        if (l.K5SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(l.NodeId);
                    }
                    else if (kx == 6)
                    {
                        if (l.K6SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(l.NodeId);
                    }
                    else if (kx == 7)
                    {
                        if (l.K7SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(l.NodeId);
                    }
                    else if (kx == 8)
                    {
                        if (l.K8SelectionTestAns == EnumSelectionTestAns.Ready) rtn.Add(l.NodeId);
                    }


                }
            }
            return rtn;
        }


        public static List<TreeNodeBase > GetAnykxChecked(bool isMeasure )
        {
            
            var dic = new Dictionary< int ,TreeNodeBase>();
            var rtn = new List<TreeNodeBase>();
            foreach (var f in RegisterTmlNode)
            {
                foreach (var l in f.Value)
                {
                    //if (l.IsRtuUsed == false) continue;
                    if(isMeasure )
                    {
                        if (l.State == EnumTmlState.NotUse) continue;
                    }
                    else
                    {
                        if (l.State != EnumTmlState.Use) continue;
                    }
                    if (l.IsChecked == false) continue;
                    if (dic.ContainsKey(l.NodeId)) continue;

                    if (l.IsSwitch1Checked || l.IsSwitch2Checked || l.IsSwitch3Checked || l.IsSwitch4Checked ||
                        l.IsSwitch5Checked || l.IsSwitch6Checked || l.IsSwitch7Checked || l.IsSwitch8Checked)
                    {
                        rtn.Add(l);
                        //dic.Add(l.NodeId, l);
                    }

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
            return (from t in rtn  orderby t.PhysicalId ascending select t).ToList();
          //  return  (from t in dic orderby t.Value .PhysicalId ascending  select t.Value ).ToList() ;
        }

        public static List<TreeNodeBase> GetAnyChecked(bool isMeasure)
        {
            var dic = new Dictionary<int, TreeNodeBase>();
            foreach (var f in RegisterTmlNode)
            {
                foreach (var l in f.Value)
                {
                    //if (l.IsRtuUsed == false) continue;
                    if (isMeasure)
                    {
                        if (l.State == EnumTmlState.NotUse) continue;
                    }
                    else
                    {
                        if (l.State != EnumTmlState.Use) continue;
                    }
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

            if (!_registerTmlNode.ContainsKey(NodeId))
            {
                _registerTmlNode.Add(NodeId, new List<TreeNodeBase>());
            }

            _registerTmlNode[NodeId].Add(this);
        }

       
        public TreeTmlNode(TreeNodeBase father, int nodeid)
        {
            //IsRtuUsed = false ;

          
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
                //IsRtuUsed = infox.RtuStateCode == 2;
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


            //if (!_registerTmlNode.ContainsKey(NodeId))
            //{
            //    _registerTmlNode.Add(NodeId, new List<TreeNodeBase>());
            //}

            //_registerTmlNode[NodeId].Add(this);
        }

        public TreeTmlNode( int nodeid,string remarks = "",int index= 0)
        {
            //IsRtuUsed = false ;

            
            Visi = Visibility.Visible;
            IsGroup = false;
            NodeId = nodeid;
            Remarks = remarks;
            Index = index;

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
                //IsRtuUsed = infox.RtuStateCode == 2;
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


            //if (!_registerTmlNode.ContainsKey(NodeId))
            //{
            //    _registerTmlNode.Add(NodeId, new List<TreeNodeBase>());
            //}

            //_registerTmlNode[NodeId].Add(this);
        }
    }
}
