using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows;
using Wlst.Cr.CoreOne.TreeNodeBase;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlPriorityLevelSettingViewModel.Services
{
    public class ListTreeNodeBase : TreeNodeBaseViewModel
    {
        public static bool IsChildItemsSelected = true;

        public override bool IsSelected
        {
            get { return base.IsSelected; }
            set
            {
                base.IsSelected = value;

                if (IsChildItemsSelected)
                {
                    foreach (var f in ChildTreeItems) f.IsSelected = value;
                }
            }
        }

        private bool _isGroup;

        public bool IsGroup
        {
            get { return _isGroup; }
            set { _isGroup = value; }
        }

        ///// <summary>
        ///// 父节点
        ///// </summary>
        //protected ListTreeNodeBase _father;

        //public ListTreeNodeBase Father
        //{
        //    get { return _father; }
        //}

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private ObservableCollection<ListTreeNodeBase> _childTreeItemsInfo;

        public ObservableCollection<ListTreeNodeBase> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                {
                    _childTreeItemsInfo = new ObservableCollection<ListTreeNodeBase>();
                }
                return _childTreeItemsInfo;
            }
        }

        /// <summary>
        /// 提供删除所有子节点功能，当选中的终端变化时，前选择的终端子节点全部删除
        /// </summary>
        public void CleanChildren()
        {
            this.ChildTreeItems.Clear();
        }


        public virtual void UpdateNodeSelected()
        {
            
        }

        public virtual void UpdateNoAlarmSelected()
        {

        }
    }
}
