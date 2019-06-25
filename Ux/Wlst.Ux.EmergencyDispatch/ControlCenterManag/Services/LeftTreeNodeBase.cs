using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Wlst.Cr.CoreOne.TreeNodeBase;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag.Services
{
    public class LeftTreeNodeBase:TreeNodeBaseViewModel
    {
        public LeftTreeNodeBase()
        {
        }

        #region IsGroup
        private bool _isGroup;

        public bool IsGroup
        {
            get { return _isGroup; }
            set
            {
                _isGroup = value;
                this.RaisePropertyChanged(() => this.IsGroup);
            }
        }
        #endregion

        #region CheckContent

        private string _checkContent="选中";
        public string CheckContent
        {
            get { return _checkContent; }
            set
            {
                if(value==_checkContent) return;
                _checkContent = value;
                RaisePropertyChanged(()=>CheckContent);
            }
        }
        #endregion
        #region IsChecked
        private bool _isChecked;
        public  bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value == _isChecked) return;
                _isChecked = value;
                if(IsGroup)
                {
                    AllGroupIsSelected(IsChecked);
                }
                else
                {
                    if (Father.ChildTreeItems.Count == (from item in Father.ChildTreeItems where item.IsChecked select item.NodeId).Count())
                    {
                        Father.CheckContent = "全选";
                    }
                    else if (!(from item in Father.ChildTreeItems where item.IsChecked select item.NodeId).Any())
                    {
                        Father.IsChecked = false;
                        Father.CheckContent = "选中";
                    }
                    else
                    {
                        Father.CheckContent = (from item in Father.ChildTreeItems where item.IsChecked select item.NodeId).Count().ToString(CultureInfo.InvariantCulture);
                    }
                }
                RaisePropertyChanged(() => IsChecked);
            }
        }

        #endregion

        private void AllGroupIsSelected(bool isSelected)
        {
            foreach (var t in ChildTreeItems)
            {
                t.IsChecked = isSelected;
            }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        protected LeftTreeNodeBase _father;

        public LeftTreeNodeBase Father
        {
            get { return _father; }
        }


        private ObservableCollection<LeftTreeNodeBase> _childTreeItemsInfo;

        public ObservableCollection<LeftTreeNodeBase> ChildTreeItems
        {
            get { return _childTreeItemsInfo ?? (_childTreeItemsInfo = new ObservableCollection<LeftTreeNodeBase>()); }
        }


        /// <summary>
        /// 加载节点，第一次使用
        /// </summary>
        public virtual void AddChild()
        {

        }


        public virtual void UpdateFaultFaultInformation(string info, bool ditui)
        {
            if (IsGroup && ditui) foreach (var t in ChildTreeItems) t.UpdateFaultFaultInformation(info, true);
        }

        public virtual void UpdateGroupSelected(bool selected, bool ditui)
        {
            IsSelected = selected;
            if (IsGroup && ditui) foreach (var t in ChildTreeItems) t.UpdateGroupSelected(selected, true);
        }

        public virtual void UpdateGroupSelectedAuto()
        {

        }

        /// <summary>
        /// 提供删除所有子节点功能，当选中的终端变化时，前选择的终端子节点全部删除
        /// </summary>
        public void CleanChildren()
        {
            ChildTreeItems.Clear();
        }
    }
}
