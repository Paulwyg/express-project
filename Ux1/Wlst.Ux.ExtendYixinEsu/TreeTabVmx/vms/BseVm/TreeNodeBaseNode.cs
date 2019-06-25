using System.Collections.ObjectModel;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Sr.EquipmentInfoHolding.Services;


namespace Wlst.Ux.ExtendYixinEsu.TreeTabVmx.vms.BseVm
{
    public class TreeNodeBaseNode : TreeNodeBaseViewModel
    {
        public string RightMenuKey = "NoKey";
        public TypeOfTabTreeNode NodeType;

        public long  Md5 = 0;

        /// <summary>
        /// 父节点
        /// </summary>
        protected TreeNodeBaseNode _father;

        public TreeNodeBaseNode Father
        {
            get { return _father; }
        }

        public string PhoneNumber="";

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
                    var res =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( NodeId);
                    if(res!=null )
                    {
                        return string.Format("{0:D4}", res.RtuPhyId )+"-";
                    }
                    return string.Format("{0:D4}", NodeId) + "-";
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


        private string _extendRtuCount;

        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public string ExtendRtuCount
        {
            get
            {
                if (this.NodeType == TypeOfTabTreeNode.IsTmlParts) return null;
                if (this.NodeType == TypeOfTabTreeNode.IsTml) return null;
                //if (this.NodeType == TypeOfTabTreeNode.IsAll) return null;
                _extendRtuCount = " [" + RtuCount + " 个]";

                return _extendRtuCount;
            }
            set
            {
                if (value == _extendRtuCount) return;
                _extendRtuCount = value;
                this.RaisePropertyChanged(() => this.ExtendRtuCount);
            }
        }



        public int  RtuCount;
        public void GetChildRtuCount()
        {
            if (this.NodeType == TypeOfTabTreeNode.IsTmlParts) return;
            if (this.NodeType == TypeOfTabTreeNode.IsTml) return;
          //  if (this.NodeType == TypeOfTabTreeNode.IsAll) return;

            int count = 0;
            foreach (var t in this.ChildTreeItems)
            {
                if (t.NodeType == TypeOfTabTreeNode.IsGrp || t.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
                {
                    t.GetChildRtuCount();
                    count += t.RtuCount;
                }
               // if (Wlst .Sr .EquipmentInfoHolding .Services .EquipmentIdAssignRang .IsRtuIsRtuLight( t.NodeId ))  count += 1;
                if (t.NodeType ==TypeOfTabTreeNode.IsTml ) count += 1;
            }
            RtuCount = count;
            this.RaisePropertyChanged(() => this.ExtendRtuCount);
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

      
    }


    public enum TypeOfTabTreeNode
    {
        /// <summary>
        /// Is  group or  not
        /// </summary>
        IsGrp,
        /// <summary>
        /// Is  terminal or not
        /// </summary>
        IsTml,
        /// <summary>
        /// Is the part of  one  terminal
        /// </summary>
        IsTmlParts,
        /// <summary>
        /// Is SPECIAL  group
        /// </summary>
        IsGrpSpecial,
        /// <summary>
        /// 所有终端
        /// </summary>
        IsAll,
        /// <summary>
        /// other  unknown
        /// </summary>
        OtherElse,
    }
}

