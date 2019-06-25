using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Ux.Wj1090Module.Resources;


namespace Wlst.Ux.Wj1090Module.LduTreeInfo.Base
{
    public partial class NodeBaseNode : TreeNodeBaseViewModel
    {
        public string RightMenuKey = "NoKey";
        public TypeOfTreeNode NodeType = TypeOfTreeNode.IsLine;
        public long Md5 = 0;

        /// <summary>
        /// 父节点
        /// </summary>
        protected NodeBaseNode _father;

        public NodeBaseNode Father
        {
            get { return _father; }
        }

        public NodeBaseNode ()
        {
            this.ForeGround = "Black";
        }

        private ObservableCollection<NodeBaseNode> _childTreeItemsInfo;

        public ObservableCollection<NodeBaseNode> ChildTreeItems
        {
            get { return _childTreeItemsInfo ?? (_childTreeItemsInfo = new ObservableCollection<NodeBaseNode>()); }
        }


        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public override string NodeIdFormat
        {
            get
            {
                if (NodeType == TypeOfTreeNode.IsTml)
                {
                    return string.Format("{0:D4}",
                                         Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
                                             EquipmentInfoDictionary[NodeId].PhyId);
                }
                return string.Format("{0:D2}", NodeId) + "-";
            }
        }


        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public virtual void AddChild()
        {

        }

        public virtual void ReUpdate(int updateArgu)
        {

        }

        /// <summary>
        /// 提供删除所有子节点功能，当选中的终端变化时，前选择的终端子节点全部删除
        /// </summary>
        public void CleanChildren()
        {
            for (int i = this.ChildTreeItems.Count - 1; i > -1; i--)
            {
                this.ChildTreeItems.RemoveAt(i);
            }
        }


        protected void HelpCmm(ObservableCollection<IIMenuItem> t)
        {
            var ggggg = Wlst.Sr.Menu.Services.MenuBuilding.HelpCmm(t);
            Cm.Items.Clear();
            foreach (var gggggggg in ggggg)
            {
                Cm.Items.Add(gggggggg);
            }
        }



    }

    public partial class NodeBaseNode
    {
        #region  IsUsed

        private Visibility _isUsed = Visibility.Collapsed;

        public Visibility IsUsed
        {
            get { return _isUsed; }
            set
            {
                if (_isUsed != value)
                {
                    _isUsed = value;
                    RaisePropertyChanged(() => this.IsUsed);
                }

            }
        }

        #endregion

        #region NoUsed

        private Visibility _noUsed = Visibility.Collapsed;

        public Visibility NoUsed
        {
            get { return _noUsed; }
            set
            {
                if (_noUsed != value)
                {
                    _noUsed = value;
                    RaisePropertyChanged(() => this.NoUsed);
                }

            }
        }

        #endregion

        #region 在树上显示统计的未使用防盗线路数

        //统计显示控制

        #region ConcentratorCountVisi

        private Visibility _concentratorCountVisi = Visibility.Collapsed;

        public Visibility ConcentratorCountVisi
        {
            get { return _concentratorCountVisi; }
            set
            {
                if (_concentratorCountVisi == value) return;
                _concentratorCountVisi = value;
                RaisePropertyChanged(() => this.ConcentratorCountVisi);
            }
        }

        #endregion

        //统计数

        #region  ConcentratorCount

        private int _concentratorCount;

        public int ConcentratorCount
        {
            get { return _concentratorCount; }
            set
            {
                if (_concentratorCount == value) return;
                _concentratorCount = value;
                RaisePropertyChanged(() => this.ConcentratorCount);
            }
        }

        #endregion

        #endregion


        private object _imagesIcon;

        /// <summary>
        /// 前台界面绑定的图标
        /// </summary>
        public object ImagesIcon
        {
            get { return _imagesIcon; }
            set
            {
                if (_imagesIcon != value)
                {
                    _imagesIcon = value;
                    this.RaisePropertyChanged(() => this.ImagesIcon);
                }
            }
        }

        /// <summary>
        /// 图片图标  停用图标 地址 1010199  不用图标1010198  
        /// 其他状态使用 1010100+状态值
        /// </summary>
        private int _picIndex = 0;

        protected int PicIndex
        {
            get { return _picIndex; }
            set
            {
                if (value != _picIndex)
                {
                    _picIndex = value;
                    ImagesIcon = ImageResources.GetEquipmentIcon(value);
                }
            }
        }

        public virtual void UpdateTmlStateInfomation()
        {

            //if (
            //    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
            //        NodeId))
            //{
            //    var equ =
            //        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
            //            NodeId];

            //    var tmp =
            //        Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipmentStateInfoHold.GetEquipmentState(
            //            this.NodeId);
            //    PicIndex = equ.RtuModel + tmp;


            //}

            //if (
            //    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
            //        NodeId))
            //{
            //    var equ =
            //        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
            //            NodeId];
            //    if (equ.RtuModel == 3005 || equ.RtuModel == 3090)
            //    {
            //        var s = equ.RtuState;
            //        if (s == 0)
            //        {
            //            PicIndex = 3001;
            //            return;
            //        }
            //        if (s == 1)
            //        {
            //            PicIndex = 3002;
            //            return;
            //        }
            //        var tmp =
            //            Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipmentStateInfoHold.GetEquipmentState(
            //                this.NodeId);
            //        PicIndex = 3005 + tmp;
            //    }
            //    else
            //    {
            //        var tmp =
            //            Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipmentStateInfoHold.GetEquipmentState(
            //                this.NodeId);
            //        PicIndex = equ.RtuModel + tmp;
            //    }

            //}

        }
    }
}

