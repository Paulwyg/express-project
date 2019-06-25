//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.ComponentModel.Composition;
//using System.Linq;
//using System.Reflection;
//using System.Windows.Input;
//
//
//using Wlst.Cr.Core.CoreServices;
//using Wlst.Cr.Core.EventHandlerHelper;
//using Wlst.Cr.CoreMims.Commands;
//using Wlst.Sr.EquipmentInfoHolding.Services;
//using Wlst.Ux.EquipemntTree.GrpComSingleMuliViewModel;
//using Wlst.Ux.EquipemntTree.GrpMulitTabShowViewModel.Services;
//using Wlst.Ux.EquipemntTree.Models;

//namespace Wlst.Ux.EquipemntTree.GrpMulitTabShowViewModel.ViewModel
//{
//    //[Export(typeof (IIMultiTree))]
//    //[PartCreationPolicy(CreationPolicy.Shared)]
//    public partial class TreeMulitViewModel : ObservableObject, IIMultiTree
//    {

//        public TreeMulitViewModel()
//        {
//           EventPublish.AddEventTokener( 
//                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);
//            LoadNode();
//        }

//        private void ReUpdateLoadChild()
//        {
//            if (ChildTreeItems.Count == 0)
//            {
//                LoadNode();
//                return;
//            }
//            var lst = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp.Keys.ToList();

//            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
//            {
//                if (lst.Contains(ChildTreeItems[i].NodeId) == false)
//                {
//                    this.ChildTreeItems.RemoveAt(i);
//                }
//                else
//                {
//                    this.ChildTreeItems[i].ReUpdate(0);
//                }
//            }

//            var existnode = (from t in ChildTreeItems select t.NodeId).ToList();
//            var allnode = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp.Values.ToList();
//            foreach (var f in allnode)
//            {
//                if (existnode.Contains(f.GroupId)) continue;
//                this.ChildTreeItems.Add(new TreeNodeItemMulitGroupViewModel(null, f));
//            }

//            foreach (var t in this.ChildTreeItems) t.GetChildRtuCount();

//        }

//        #region load node

//        //加载终端节点
//        private void LoadNode()
//        {

//            if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Count == 0 || Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp.Count == 0)
//                return;

//            ChildTreeItems.Clear();

//            var lst = (from t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp
//                       orderby t.Value.Index
//                       select t.Value).ToList();
//            foreach (var f in lst)
//            {
//                this.ChildTreeItems.Add(new TreeNodeItemMulitGroupViewModel(null, f));
//            }

//            foreach (var t in this.ChildTreeItems) t.GetChildRtuCount();

//        }



//        #endregion

//        #region tab iinterface

//        public string Title
//        {
//            get
//            {
//                return "本地分组";
//                return "本地分组";
//            }
//        }


//        public bool CanClose
//        {
//            get { return false; }
//        }

//        /// <summary>
//        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
//        /// 是否可锁定
//        /// </summary>
//        public bool CanUserPin
//        {
//            get { return true; }
//        }

//        /// <summary>
//        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
//        /// 是否可悬浮
//        /// </summary>
//        public bool CanFloat
//        {
//            get { return true; }
//        }

//        /// <summary>
//        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
//        /// 是否可移动至document host
//        /// </summary>
//        public bool CanDockInDocumentHost
//        {
//            get { return true; }
//        }

//        #endregion

//        private ObservableCollection<TreeNodeBaseNode> _childTreeItemsInfo;

//        public ObservableCollection<TreeNodeBaseNode> ChildTreeItems
//        {
//            get
//            {
//                if (_childTreeItemsInfo == null)
//                    _childTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
//                return _childTreeItemsInfo;
//            }
//        }
//    };


//    //event
//    public partial class TreeMulitViewModel
//    {
//        #region IEventAggregator Subscription

//        //10201：分组增加 参数一个: 地址
//        //10202：分组删除 参数一个：地址
//        //10203：分组更新 参数一个：地址
//        public void FundEventHandler(PublishEventArgs args)
//        {
//            if (args.EventType == PublishEventType.SvAv)
//            {
//                ReUpdateLoadChild();
//                return;
//            }
//            if (args.EventType == PublishEventType.Core)
//            {

//                if (args.EventId ==
//                    EventIdAssign.EquipmentAddEventId)
//                {
//                    //int x = Convert.ToInt32(args.GetParams()[0]);
//                    ReUpdateLoadChild();
//                    return;
//                }
//                if (args.EventId ==
//                    EventIdAssign.EquipmentDeleteEventId)
//                {
//                    ReUpdateLoadChild();
//                    ////int x = Convert.ToInt32(args.GetParams()[0]);
//                    ////ReUpdateLoadChild();
//                    //var lst = args.GetParams()[0] as IEnumerable<Tuple<int, int>>;
//                    //if (lst == null) return;
//                    ////    this.ReUpdateLoadChild();
//                    //foreach (var t in lst)
//                    //{
//                    //    if (t.Item2 == 0)
//                    //    {
//                    //        //  deltenode

//                    //        if (BaseNodes.Nodess.ContainsKey(t.Item1))
//                    //        {
//                    //            BaseNodes.Nodess.Remove(t.Item1);
//                    //        }
//                    //        if (BaseNodes.AllTmpNodess.ContainsKey(t.Item1))
//                    //        {
//                    //            BaseNodes.AllTmpNodess.Remove(t.Item1);
//                    //        }
//                    //        DeleteNode(t.Item1, ChildTreeItems);

//                    //    }
//                    //}

//                    return;
//                }
//                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.MulityInfoGroupAllNeedUpdate
//                    )
//                {
//                    //int x = Convert.ToInt32(args.GetParams()[0]);
//                    ReUpdateLoadChild();
//                    return;
//                }


//            }
//        }

//        #region CmdReflesh
//        private ICommand _CmdReflesh;

//        /// <summary>
//        /// 左侧树 添加根节点
//        /// </summary>
//        public ICommand CmdReflesh
//        {
//            get { return _CmdReflesh ?? (_CmdReflesh = new RelayCommand(ExAddTree, CanExAddTree, false)); }
//        }

//        private bool CanExAddTree()
//        {
//            return true;
//        }


//        private void ExAddTree()
//        {

//            ReUpdateLoadChild();

//        }


//        #endregion

//        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
//        {
//            if (args.EventType == PublishEventType.SvAv) return true;
//            if (args.EventType == PublishEventType.Core)
//                switch (args.EventId)
//                {
//                    //case EventIdAssign.EquipmentUpdateEventId:
//                    //    return true;
//                    //    break;
//                    case EventIdAssign.EquipmentDeleteEventId:
//                        //52304：终端已经重新加载需要更新其他显示数据
//                        return true;
//                        break;
//                    case EventIdAssign.EquipmentAddEventId: //10202：分组删除
//                        return true;
//                        break;
//                    //case EventIdAssign.EquipmentStateChanged:
//                    //    return true;
//                    //    break;
//                    case Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.MulityInfoGroupAllNeedUpdate:
//                        return true;
//                        break;
//                    default:
//                        return false;
//                }
//            return false;
//        }





//        #endregion
//    }
//}