using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.TreeNodeBase;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.LdEquipemntTree.Models;

namespace Wlst.Ux.LdEquipemntTree.GrpComSingleMuliViewModel
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
        public string IpAddr = "";
        public int TmlState = 0;
        public string RtuInstallAddr = "";
        public string RtuOnly = "";        

        private ObservableCollection<TreeNodeBaseNode> _ldChildTreeItemsInfo;

        public ObservableCollection<TreeNodeBaseNode> ChildTreeItems
        {
            get
            {
                if (_ldChildTreeItemsInfo == null)
                {
                    _ldChildTreeItemsInfo = new ObservableCollection<TreeNodeBaseNode>();
                }
                return _ldChildTreeItemsInfo;
            }
            set
            {
                if (value == _ldChildTreeItemsInfo) return;
                _ldChildTreeItemsInfo = value;
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
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(NodeId))
                    {
                        var phyId =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[NodeId].RtuPhyId;
                        return phyId.ToString("d4") + "-";
                    }
                    else if (NodeId == -1)
                    {
                        return string.Format("{0:D1}", NodeId) + "-";
                    }
                    else
                    {
                        return string.Format("{0:D2}", NodeId) + "-";
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

        private string mark;

        /// <summary>
        /// 标记 是否满足关键字
        /// </summary>
        public string Mark
        {
            get { return mark; }
            set
            {
                if (value == mark) return;
                mark = value;
                this.RaisePropertyChanged(() => this.Mark);
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

                if (this.NodeType == TypeOfTabTreeNode.IsArea) return null;

                else if (this.NodeType == TypeOfTabTreeNode.IsGrp)
                {
                    _extendRtuCount = " [" + RtuCount + " 个]";
                }

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
                //if (t.NodeType == TypeOfTabTreeNode.IsGrp || t.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
                //{
                //    t.GetChildRtuCount();
                //    count += t.RtuCount;
                //}
                // if (Wlst .Sr .EquipmentInfoHolding .Services .EquipmentIdAssignRang .IsRtuIsRtuLight( t.NodeId ))  count += 1;
                if (t.NodeType == TypeOfTabTreeNode.IsTml) count += 1;
                else
                {
                    t.GetChildRtuCount();
                    count += t.RtuCount;

                }
                
                
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

        public void DeleteChild(int i)
        {
            this.ChildTreeItems.RemoveAt(i);
        }

    }
}

