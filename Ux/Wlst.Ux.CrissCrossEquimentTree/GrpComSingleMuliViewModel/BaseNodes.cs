using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.CrissCrossEquipemntTree.GrpComSingleMuliViewModel
{
    public class BaseNodes
    {

        public static Dictionary<int, List<TreeNodeBaseNode>> Nodess = new Dictionary<int, List<TreeNodeBaseNode>>();

        public static void AddNode(TreeNodeBaseNode node)
        {
            if (!Nodess.ContainsKey(node.NodeId)) Nodess.Add(node.NodeId, new List<TreeNodeBaseNode>());
            Nodess[node.NodeId].Add(node);
        }




        //public static Dictionary<int, TreeNodeBaseNode> AllTmpNodess = new Dictionary<int, TreeNodeBaseNode>();

        //public static void AddAllTmpNodess(TreeNodeBaseNode node)
        //{
        //    if (AllTmpNodess.ContainsKey(node.NodeId)) return;
        //    AllTmpNodess.Add(node.NodeId, node);
        //    // tnb.ChildTreeItems.Add(node);
        //}
    }
}
