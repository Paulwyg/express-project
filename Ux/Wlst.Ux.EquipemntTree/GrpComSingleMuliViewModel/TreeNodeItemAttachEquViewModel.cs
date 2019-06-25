using System.Collections.ObjectModel;
using System.Windows;


using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.EquipemntTree.Models;
using Wlst.Ux.EquipemntTree.Resources;

namespace Wlst.Ux.EquipemntTree.GrpComSingleMuliViewModel
{

    public class TreeNodeItemAttachEquViewModel : TreeNodeBaseNode
    {
        public TreeNodeItemAttachEquViewModel()
        {
            this.NodeType = TypeOfTabTreeNode.IsTmlParts;
            //Visi = Visibility.Visible;
        }

        protected int RtuModel;
        protected Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase AttInfo;
        //private int _rtuModes=0;
        public TreeNodeItemAttachEquViewModel(TreeNodeBaseNode mvvmFather,
                                              Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase attachInfomation)
        {
            AttInfo = attachInfomation;
            this.NodeType = TypeOfTabTreeNode.IsTmlParts;
            //Visi = Visibility.Visible;
            this._father = mvvmFather;

            this.IsShowChkTree =Visibility.Collapsed;
                // Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 2, false)
                         //? Visibility.Visible
                         //: Visibility.Collapsed;

            if (attachInfomation == null)
            {
                this.NodeName = "加载出错";
            }
            else
            {
                this.NodeName = attachInfomation.RtuName;
                var running = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(attachInfomation.RtuId);
                //if (running != null && running.ErrorCount > 0)
                //{
                    //this.ImagesIcon = ImageResources.GetEquipmentIcon((int) attachInfomation.RtuModel + 1);
                //}
                //else
                //{
                    this.ImagesIcon = ImageResources.GetEquipmentIcon((int) attachInfomation.RtuModel);
                //}

                this.NodeId = attachInfomation.RtuId;
                this.Md5 = attachInfomation.DateUpdate;
                RtuModel = (int) attachInfomation.RtuModel;
            }
        }



        private ObservableCollection<TreeNodeBaseViewModel> _childTreeItemsInfo;

        public new ObservableCollection<TreeNodeBaseViewModel> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                {
                    _childTreeItemsInfo = new ObservableCollection<TreeNodeBaseViewModel>();
                }
                return _childTreeItemsInfo;
            }
            set
            {
                if (value == _childTreeItemsInfo) return;
                _childTreeItemsInfo = value;
                this.RaisePropertyChanged(() => this.ChildTreeItems);
            }
        }


        public override void OnNodeSelectDisActive()
        {
            // ChildTreeItems.Clear();
            //base.OnNodeSelectDisActive();
        }


        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public override void AddChild()
        {
            ChildTreeItems.Clear();
        }

        private void LoadNodes()
        {
            ChildTreeItems.Clear();
            if (!Wlst.Cr.CoreMims.ComponentHold.TreeNodeExportHolding.DicEquipmentModels.ContainsKey(RtuModel)) return;
            var rtux =
                Wlst.Cr.CoreMims.ComponentHold.TreeNodeExportHolding.DicEquipmentModels[RtuModel].GetTreeNodeInfo(
                    NodeId);
            if (rtux != null && rtux.Count > 0)
            {
                foreach (var f in rtux)
                {
                    f.IsShowChkTree = Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(4001, 2, false)
                         ? Visibility.Visible
                         : Visibility.Collapsed;
                    ChildTreeItems.Add(f);
                }
            }
        }

        /// <summary>
        /// 当选择的终端发送变化时，如果 
        /// </summary>
        public override void OnNodeSelectActive()
        {
            //base.OnNodeSelect();
            //发布事件  选中当前节点
            var args = new PublishEventArgs
                           {
                               EventType = PublishEventType.Core,
                               EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                               EventAttachInfo = "Tree"
                           };
            args.AddParams(NodeId);
            EventPublish.PublishEvent(args);
            //lvf  2018年5月22日14:40:48  记录当前点击终端
            Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId = NodeId;
            LoadNodes();
        }

        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public override string NodeIdFormat
        {
            get { return string.Format("{0:D4}", NodeId) + "-"; }
        }

        #region  Reset ContextMenu

        public override void ResetContextMenu()
        {
            ResetCm();
        }

        public void ResetCm()
        {
            this.CmItems = MenuBuilding.BulidCm(RtuModel.ToString(), false, AttInfo);
        }



        #endregion



    }
}
