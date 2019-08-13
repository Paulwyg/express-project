using System.Collections.ObjectModel;
using System.Windows;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Sr.EquipmentInfoHolding.Services;

namespace Wlst.Ux.Wj9001Module.Wj9001TreeView.ViewModel
{
    public class TreeNodeBaseNode : TreeNodeBaseViewModel
    {
        public string RightMenuKey = "NoKey";
        public TypeOfTabTreeNode NodeType;

        public long Md5 = 0;

        /// <summary>
        /// 父节点
        /// </summary>
        protected TreeNodeBaseNode _father;

        public TreeNodeBaseNode Father
        {
            get { return _father; }
        }

        public string PhoneNumber = "";
        public string IpAddr = "";

        private ObservableCollection<TreeNodeBaseNode> _childTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                {
                    _childTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
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

        public virtual void OnDoubleClick()
        {

        }

        //#region 在树上显示统计的未使用线路检测线路数

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

        public int PhyId;

        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public override string NodeIdFormat
        {
            get
            {
                if (UxTreeSetting.IsShowGrpInTreeModelShowId)
                {
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(NodeId))
                    {
                        var phyId =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[NodeId].RtuPhyId;
                        return phyId.ToString("d4") + "-";
                    }
                    else
                    {
                        return string.Format("{0:D4}", NodeId) + "-";
                    }
                }
                return "";
            }
        }


        private string extendSerach;

        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public string ExtendSerachConten
        {
            get { return extendSerach; }
            set
            {
                if (value == extendSerach) return;
                extendSerach = value;
                this.RaisePropertyChanged(() => this.ExtendSerachConten);
            }
        }


        private string _extendCount;

        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public string ExtendCount
        {
            get
            {
                if (this.NodeType == TypeOfTabTreeNode.IsTml) return null;
                if (this.NodeType == TypeOfTabTreeNode.IsArea)
                {
                    _extendCount = " [" + RtuCount + " 个]";
                }
                return _extendCount;
            }
            set
            {
                if (value == _extendCount) return;
                _extendCount = value;
                this.RaisePropertyChanged(() => this.ExtendCount);
            }
        }



        public int RtuCount;
        public void GetChildRtuCount()
        {

            if (this.NodeType == TypeOfTabTreeNode.IsTml) return;

            int count = 0;
            foreach (var t in this.ChildTreeItems)
            {
                //if (t.NodeType == TypeOfTabTreeNode.IsGrp || t.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
                //{
                //    t.GetChildRtuCount();
                //    count += t.RtuCount;
                //}
                //if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsRtuLight(t.NodeId)) count += 1;
                if (t.NodeType == TypeOfTabTreeNode.IsTml) count += 1;
                else if (t.NodeType == TypeOfTabTreeNode.IsAll)
                {
                    t.GetChildRtuCount();
                    count += t.RtuCount;

                }


            }
            RtuCount = count;
            //if(this.NodeType == TypeOfTabTreeNode.IsGrp || this.NodeType==TypeOfTabTreeNode.IsGrpSpecial ||this.NodeType ==TypeOfTabTreeNode.IsArea)
            //{
            //    if(RtuCount<1)
            //    {
                    
            //    }
            //    //this.DeleteChild(this.NodeId);
            //}

            this.RaisePropertyChanged(() => this.ExtendCount);
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

        public void DeleteChild(int i)
        {
            this.ChildTreeItems.RemoveAt(i);
        }

    }

    public enum TypeOfTabTreeNode
    {
        IsGrp = 1,
        IsArea,
        IsGrpSpecial,
        IsAll,
        IsTml,
    }
}
