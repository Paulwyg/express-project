using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Wlst.Cr.CoreOne.TreeNodeBase;

namespace Wlst.Ux.EmergencyDispatch.LightFaultShieldSet.Services
{
    public class ListTreeNodeBase : TreeNodeBaseViewModel
    {

        public ListTreeNodeBase()
        {

        }


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
        private string _checkNum="选中";
        public string CheckNum
        {
            get { return _checkNum; }
            set
            {
                if (_checkNum.Equals(value)) return;
                _checkNum = value;
                RaisePropertyChanged(() => CheckNum);
            }
        }
        private bool _isGrpHasTmlChecked;
        public bool IsGrpHasTmlChecked
        {
            get { return _isGrpHasTmlChecked; }
            set { _isGrpHasTmlChecked = value;
            RaisePropertyChanged(()=>IsGrpHasTmlChecked);}
        }

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

                    CheckNum =IsChecked? "全选":"选中";
                }
                else
                {
                   if(Father.ChildTreeItems.Count==(from item in Father.ChildTreeItems where item.IsChecked select item.NodeId).Count())
                   {
                       Father.CheckNum = "全选";
                       Father.IsChecked = true;
                   }
                   else if (!(from item in Father.ChildTreeItems where item.IsChecked select item.NodeId).Any())
                   {
                       Father.CheckNum = "选中";
                       Father.IsChecked = false;
                   }
                   else
                   {
                       Father.CheckNum =
                           (from item in Father.ChildTreeItems where item.IsChecked select item.NodeId).Count().ToString(CultureInfo.InvariantCulture);

                   }
                }
                //GetGrpHasTmlCheckedValue();
                RaisePropertyChanged(() => IsChecked);
            }
        }

        #region IsEnabled

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                if(ChildTreeItems.Count>0)
                {
                    foreach (var item in ChildTreeItems)
                    {
                        item.IsEnabled = IsEnabled;
                    }
                }
                RaisePropertyChanged(() => IsEnabled);
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
        protected ListTreeNodeBase _father;

        public ListTreeNodeBase Father
        {
            get { return _father; }
        }


        private ObservableCollection<ListTreeNodeBase> _childTreeItemsInfo;

        public ObservableCollection<ListTreeNodeBase> ChildTreeItems
        {
            get { return _childTreeItemsInfo ?? (_childTreeItemsInfo = new ObservableCollection<ListTreeNodeBase>()); }
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
