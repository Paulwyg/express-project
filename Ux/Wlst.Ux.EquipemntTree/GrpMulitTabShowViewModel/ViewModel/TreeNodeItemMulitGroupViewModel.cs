using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;


using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.EquipemntTree.GrpComSingleMuliViewModel;
using Wlst.Ux.EquipemntTree.Models;
using Wlst.Ux.EquipemntTree.Resources;
using Wlst.client;

namespace Wlst.Ux.EquipemntTree.GrpMulitTabShowViewModel.ViewModel 
{
    /// <summary>
    /// 提供终端树节点基本结构
    /// 右键菜单从MenuBuliding中动态获取，为节约程序资源，仅当点击该终端时该终端的右键菜单立即刷新
    /// IsMarked为联动标记，初始化时必须在外部初始化，即某一个分组具有联动则其子分组具有该联动属性
    /// </summary>
    public class TreeNodeItemMulitGroupViewModel : TreeNodeBaseNode 
    {

        private static TreeNodeBaseNode _currentSelectGroupNode;

        public static TreeNodeBaseNode CurrentSelectGroupNode
        {
            get { return _currentSelectGroupNode; }
            set
            {
                if (_currentSelectGroupNode == value) return;
                if (value == null) return;
                if (value.NodeType == TypeOfTabTreeNode.IsTml) return;
                if (value.NodeType == TypeOfTabTreeNode.IsGrp ||
                    value.NodeType == TypeOfTabTreeNode.IsGrpSpecial ||
                    value.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    if (_currentSelectGroupNode != null)
                        _currentSelectGroupNode.IsSelected = false;
                    _currentSelectGroupNode = value;


                    if (UxTreeSetting.IsSelectGrpMapOnlyShow == false) return;
                    var info =
                        Wlst.Sr.EquipmentInfoHolding .Services.ServicesGrpMulitInfoHold .GetRtusInGroup( value.NodeId);
                    if (info.Count == 0) return;

                    var ins = new PublishEventArgs()
                                  {
                                      EventType = PublishEventType.Core,
                                      EventId =
                                          Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuGroupSelectdWantedMapUp
                                  };
                    ins.AddParams(info);

                    EventPublish.PublishEvent(ins);
                }

            }
        }

        public TreeNodeItemMulitGroupViewModel()
        {
            this.NodeType = TypeOfTabTreeNode.IsGrp;
            //Visi = Visibility.Visible;
        }

        public TreeNodeItemMulitGroupViewModel(TreeNodeBaseNode mvvmFather, GroupItemsInfo.GroupItem groupInfomatioin)
        {
            this.NodeType = TypeOfTabTreeNode.IsGrp;
            //Visi = Visibility.Visible;
            this._father = mvvmFather;
            //TreeSingleViewModel.RegisterNodeToControl(this);


            if (groupInfomatioin == null)
            {
                this.NodeName = "GroupInfo Error";
                return;
            }
            this.NodeName = groupInfomatioin.GroupName;
            this.ImagesIcon = ImageResources.GroupIcon; // ImageSource.GrpBitmapImage;
            this.NodeId = groupInfomatioin.GroupId;
            this.AddChild();

        }

        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public override void AddChild()
        {
            ChildTreeItems.Clear();
            if (NodeType != TypeOfTabTreeNode.IsGrp) return;

            if (!Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp.ContainsKey(NodeId))
                return;

            var ntsss =
                ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(
                    Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp[NodeId].LstTml);

            foreach (var t in ntsss)
            {
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t)) continue;
                var ff = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                if (ff == null || ff.EquipmentType != WjParaBase.EquType.Rtu) continue;
                ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, ff));
            }
            foreach (var f in ChildTreeItems) f.GetChildRtuCount();
        }



        /// <summary>
        /// 当分组信息发生变化的时候  增量式重新加载节点  updateArgu wuyong
        /// </summary>
        public override void ReUpdate(int updateArgu)
        {
            //添加分组到子节点中
            if (!Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp.ContainsKey(NodeId))
            {
                this.ChildTreeItems.Clear();
                if (_father != null) _father.ChildTreeItems.Remove(this);
                return;
            }
            var gprInfo = Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp[NodeId];
            this.NodeName = gprInfo.GroupName;

            //node delete
            for (int i = this.ChildTreeItems.Count - 1; i >= 0; i--)
            {
                if (ChildTreeItems[i].NodeType == TypeOfTabTreeNode.IsTml)
                {
                    if (!gprInfo.LstTml.Contains(ChildTreeItems[i].NodeId) ||
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                            ChildTreeItems[i].NodeId))
                    {
                        this.ChildTreeItems.RemoveAt(i);
                    }
                }
                else
                {
                    this.ChildTreeItems.RemoveAt(i);
                    
                }
            }


            var existnode = (from t in ChildTreeItems select t.NodeId).ToList();
            //tml  add and update
            foreach (var t in gprInfo.LstTml)
            {
                if (existnode.Contains(t)) continue;


                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                    continue;
                var f =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                if (f.RtuFid != 0 || f.EquipmentType != WjParaBase.EquType.Rtu) continue;
                ChildTreeItems.Add(new TreeNodeItemTmlViewModel(this, f));

            }
            foreach (var t in this.ChildTreeItems) t.GetChildRtuCount();
        }

        #region Node Select

        ///// <summary>
        ///// 当选择的终端发送变化时，如果 
        ///// </summary>
        //public override void OnNodeSelectActive()
        //{
        //    //base.OnNodeSelect();
        //    //发布事件  选中当前节点
        //    //var args = new PublishEventArgs
        //    //               {
        //    //                   EventType = PublishEventType.Core,
        //    //                   EventId = CommonBase .Services .EventIdAssign .MainSingleTreeNodeActive,
        //    //               };
        //    //args.AddParams(NodeId);
        //    //args.AddParams(1);

        //    //PrismEventExtend.EventHelper.EventPublish.PublishEvent(args);

        //    ResetContextMenu();
        //}

        public override void OnNodeSelectActive()
        {
            base.OnNodeSelectActive();
            CurrentSelectGroupNode = this;
        }


        #region  Reset ContextMenu
        public override void ResetContextMenu()
        {
            ResetCm();
        }

        public void ResetCm()
        {
            ObservableCollection<IIMenuItem> t = null;
            if (Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp.ContainsKey(NodeId))
                t = MenuBuilding.BulidCm("RightMenuMulit",
                                         false,
                                         Sr.EquipmentInfoHolding.Services.ServicesGrpMulitInfoHold.ItemsMultGrp[NodeId]);

            this.CmItems = t;
        }

        #endregion

        #endregion

    }
}
