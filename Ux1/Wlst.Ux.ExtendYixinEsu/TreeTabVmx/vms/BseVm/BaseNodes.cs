using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.vms;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.BseVm
{
    public class BaseNodes
    {
        // private static  TreeNodeBaseNode  tnb = new TreeNodeBaseNode();
        public static Dictionary<int, TreeNodeBaseNode> Nodess = new Dictionary<int, TreeNodeBaseNode>();

        public static void AddNode(TreeNodeBaseNode node)
        {
            if (Nodess.ContainsKey(node.NodeId)) return;
            Nodess.Add(node.NodeId, node);
            // tnb.ChildTreeItems.Add(node);
        }




        public static Dictionary<int, TreeNodeBaseNode> AllTmpNodess = new Dictionary<int, TreeNodeBaseNode>();

        public static void AddAllTmpNodess(TreeNodeBaseNode node)
        {
            if (AllTmpNodess.ContainsKey(node.NodeId)) return;
            AllTmpNodess.Add(node.NodeId, node);
            // tnb.ChildTreeItems.Add(node);
        }


        public static int CurrentSelectedGrpId;

        public static void OnRtuGroupSelectdWantedMapUpEventRvd()
        {
            if (DateTime.Now.Ticks - lastsndeventtime < 30000000) return;
            CurrentSelectedGrpId = 0;
        }

        private static long lastsndeventtime = 0;
        public static void CurrentSelectedGrpIdChanged(TreeNodeBaseNode info)
        {
            int xg = xyrdsfd(info);
            if (xg == 0) return;
            if (CurrentSelectedGrpId == xg) return;
            CurrentSelectedGrpId = xg;

            TreeTabRtuSet.TabRtuHolding.GetRtuLstByIdx(CurrentSelectedGrpId);

            var ins = new PublishEventArgs()
                          {
                              EventType = PublishEventType.Core,
                              EventId =
                                  Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
                          };
            ins.AddParams(TreeTabRtuSet.TabRtuHolding.GetRtuLstByIdx(CurrentSelectedGrpId));

            lastsndeventtime = DateTime.Now.Ticks;
            EventPublisher.EventPublish(ins);

        }

        private static int xyrdsfd(TreeNodeBaseNode info)
        {

            var tmp = info;
            while (tmp.Father != null)
            {
                if (info.Father != null) tmp = info.Father;
            }

            int x1 = tmp.NodeId;
            int x2 = 0;
            if (tmp.ChildTreeItems.Count > 0) x2 = tmp.ChildTreeItems[0].NodeId;

            foreach (var f in TreeTabRtuSet.TabRtuHolding.Info)
            {
                if (f.Value.GrpOrRtus.Contains(x1)) return f.Key;
            }
            if (x2 > 0)
            {
                foreach (var f in TreeTabRtuSet.TabRtuHolding.Info)
                {
                    if (f.Value.GrpOrRtus.Contains(x2)) return f.Key;
                }
            }
            return 0;
        }


    }
}
