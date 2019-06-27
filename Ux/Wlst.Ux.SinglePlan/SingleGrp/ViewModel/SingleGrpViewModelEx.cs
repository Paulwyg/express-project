using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.CoreMims.NodeServices;
using Wlst.iifx;

namespace Wlst.Ux.SinglePlan.SingleGrp.ViewModel
{

    public class TreeViewBaseNodeEx:TreeViewBaseNode
    {
        public TreeViewBaseNodeEx(TreeViewBaseNode fatherNode,
            Action<long, TreeViewBaseNode> onNodeSelected,
             
            Action<long, TreeViewBaseNode> onExpanded, int key1TypeN, int key2, int key3, int key4, int key5,
            int key6, int zindexN, bool isCollapsedWhenChildItemsEmptyN,Action<long, TreeViewBaseNode> onNodeSelected2b) :
            base(fatherNode ,onNodeSelected ,onExpanded ,key1TypeN ,key2 ,key3 ,key4 ,key5 ,key6 ,zindexN ,isCollapsedWhenChildItemsEmptyN  )
        {
            OnNodeSelected2B = onNodeSelected2b;
        }


        /// <summary>
        /// 当选中的时候触发该事件
        /// </summary>
        private Action<long, TreeViewBaseNode> OnNodeSelected2B = null;


        /// <summary>
        /// 当前节点是否为系统当前选中节点  选中触发选中事件  
        /// 
        /// </summary>
        public bool IsSelected2B
        {
            get { return GetZbool(() => IsSelected2B); }
            set
            {
                SetZ(() => IsSelected2B, value);
                if (OnNodeSelected2B != null) OnNodeSelected2B(IdfN, this);
            }
        }

    }

    public class TreeViewControlEx : TreeViewControl
    {   /// <summary>
        /// 当选中的时候触发该事件
        /// </summary>
        private Action<long, TreeViewBaseNode> OnNodeSelected2B = null;

        public TreeViewControlEx(Action<long, TreeViewBaseNode> onNodeSelected,
                                 Action<long, TreeViewBaseNode> onExpanded = null,Action<long, TreeViewBaseNode> onNodeSelected2b=null )
            : base(onNodeSelected, onExpanded)
        {
            OnNodeSelected2B = onNodeSelected2b;
        }

        public override TreeViewBaseNode GetTreeViewBaseNodeExtend(TreeViewBaseNode fatherNode, InputInfo data)
        {


            bool isCollapsedWhenChildItemsEmptyN = _dicIsCollapsedWhenChildItemsEmptyN.ContainsKey(data.Key1TypeN) &&
                                                   _dicIsCollapsedWhenChildItemsEmptyN[data.Key1TypeN];

            var rtn = new TreeViewBaseNodeEx(fatherNode, OnNodeSelected, OnNodeExpanded, data.Key1TypeN, data.Key2,
                                             data.Key3, data.Key4, data.Key5, data.Key6, data.ZindexN,
                                             isCollapsedWhenChildItemsEmptyN, OnNodeSelected2B);


            return GetTreeViewBaseNode(rtn, data);
        }



    }

    public partial class SingleGrpViewModel
    {

        public TreeViewControl Tvc = new TreeViewControlEx(null, null, OnNodeSelected2B);
        public void InitTvc()
        {
            //var lst = RequestList();
            if (SluBref.Count == 0) return;
            var rtn = new List<InputInfo>();
            foreach (var f in SluBref)
            {
                var rootinfo = LoadNodeGetSluInfo(f);
                if (rootinfo == null) continue;
                var lst2 = LoadNode2GetAreaGrpInput(f, rootinfo);
                rtn.Add(rootinfo);
                rtn.AddRange(lst2);
            }
            TvcInitNode(rtn);
        }

        private void TvcInitNode(List<InputInfo> data)
        {

            Tvc.InitNode(data);
        }


        private void De(List<long> idfs)
        {
            Tvc.DeleteNode(idfs);
        }

        private static void OnNodeSelected2B(long idf, TreeViewBaseNode node)
        {
            if (node.Key1TypeN == 2) return;

            var fxfather = node as TreeViewBaseNodeEx;
            if (fxfather == null) return;

            foreach (var f in node.ChildItems)
            {
                var fx = f as TreeViewBaseNodeEx;
                if (fx == null) continue;

                fx.IsSelected2B = fxfather.IsSelected2B;
            }

        }


    }
    public partial class SingleGrpViewModel
    {
        #region 右侧终端树
        /// <summary>
        /// 获取集中器的InputInfo
        /// </summary>
        /// <returns></returns>
        private InputInfo LoadNodeGetSluInfo(int sluId)
        {
            foreach (var list in SluItem)
            {
                if (list.SluId == sluId)
                {
                    var inputinfo = new InputInfo(null, null, list.SluId, 1, list.SluId);
                    inputinfo.NodeName1B = list.SluPhyId + "-" + list.SluName;
                    inputinfo.NodeName2B = list.SluName;
                    inputinfo.Id1StoreN = list.SluPhyId;
                    inputinfo.IsVisi1 = Visibility.Collapsed;
                    inputinfo.IsVisi2 = Visibility.Collapsed;
                    inputinfo.IsVisi3 = Visibility.Collapsed;
                    inputinfo.IsVisi4 = Visibility.Collapsed;
                    return inputinfo;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取集中器下控制器节点
        /// </summary>
        /// <param name="sluId"></param>
        /// <param name="rootinfo"></param>
        /// <param name="rooTreeViewBaseNode"></param>
        /// <returns></returns>
        private List<InputInfo> LoadNode2GetAreaGrpInput(int sluId, InputInfo rootinfo, TreeViewBaseNode rooTreeViewBaseNode = null)
        {
            var lstInput = new List<InputInfo>();

            var ctrlItem = new List<SluGrpSet.SluInfoItem.SluCtrlInfo>();
            foreach (var list in SluItem)
            {
                if (list.SluId == sluId)
                {
                    ctrlItem = list.CtrlsInfo;
                }
            }

            foreach (var item in ctrlItem)
            {
                var inputinfo = new InputInfo(rootinfo, rooTreeViewBaseNode, item.CtrlPhyid, 2, item.CtrlId, sluId);
                inputinfo.NodeName1B = item.CtrlPhyid + "-" + item.CtrlName;
                inputinfo.Str1StoreN = item.CtrlName;
                inputinfo.Id1StoreN = item.CtrlPhyid;
                if (item.LampCount > 0)
                {
                    inputinfo.IsVisi1 = Visibility.Visible;
                    inputinfo.IsEnable1 = item.LampBelongGrp[0] == 0;
                    inputinfo.Id2StoreN = item.LampBelongGrp[0];
                }
                else
                {
                    inputinfo.IsVisi1 = Visibility.Collapsed;
                    inputinfo.IsEnable1 = true;
                    inputinfo.Id2StoreN = 0;
                }
                if (item.LampCount > 1)
                {
                    inputinfo.IsVisi2 = Visibility.Visible;
                    inputinfo.IsEnable2 = item.LampBelongGrp[1] == 0;
                    inputinfo.Id3StoreN = item.LampBelongGrp[1];
                }
                else
                {
                    inputinfo.IsVisi2 = Visibility.Collapsed;
                    inputinfo.IsEnable2 = true;
                    inputinfo.Id3StoreN = 0;
                }
                if (item.LampCount > 2)
                {
                    inputinfo.IsVisi3 = Visibility.Visible;
                    inputinfo.IsEnable3 = item.LampBelongGrp[2] == 0;
                    inputinfo.Id4StoreN = item.LampBelongGrp[2];
                }
                else
                {
                    inputinfo.IsVisi3 = Visibility.Collapsed;
                    inputinfo.IsEnable3 = true;
                    inputinfo.Id4StoreN = 0;
                }
                if (item.LampCount > 3)
                {
                    inputinfo.IsVisi4 = Visibility.Visible;
                    inputinfo.IsEnable4 = item.LampBelongGrp[3] == 0;
                    inputinfo.Id5StoreN = item.LampBelongGrp[3];
                }
                else
                {
                    inputinfo.IsVisi4 = Visibility.Collapsed;
                    inputinfo.IsEnable4 = true;
                    inputinfo.Id5StoreN = 0;
                }
                lstInput.Add(inputinfo);
            }
            return lstInput;
        }
        #endregion
    }
}
