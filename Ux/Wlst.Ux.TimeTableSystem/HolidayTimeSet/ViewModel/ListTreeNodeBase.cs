using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.TreeNodeBase;

namespace Wlst.Ux.TimeTableSystem.HolidayTimeSet.ViewModel
{

    public class ListTreeNodeBase : TreeNodeBaseViewModel
    {

        public TreeNodeType NodeType;


        private bool _isChecked;

        public new bool IsSelected
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked == value) return;
                _isChecked = value;
                this.RaisePropertyChanged(() => this.IsSelected);
                this.UpdateChildNodeSelected(value);
            }
        }

        private string _schemeName;

        public string SchemeName
        {
            get { return _schemeName; }
            set
            {
                if (_schemeName == value) return;
                _schemeName = value;
                this.RaisePropertyChanged(() => this.SchemeName);
            }
        }


        //private string _curschemeName;

        //public string CurrentSchemeName
        //{
        //    get { return _curschemeName; }
        //    set
        //    {
        //        if (_curschemeName == value) return;
        //        _curschemeName = value;
        //        this.RaisePropertyChanged(() => this.CurrentSchemeName);
        //    }
        //}

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


        public virtual void UpdateChildNodeSelected(bool isSelect)
        {
            IsSelected = isSelect;
            foreach (var t in ChildTreeItems)
            {
                t.UpdateChildNodeSelected(isSelect);
            }
        }

        public virtual void UpdateNodeSelectByChildNodeSelected()
        {
            if (NodeType == TreeNodeType.Rtu) return;
            if (ChildTreeItems.Count == 0)
            {
                IsSelected = false;
                return;
            }
            foreach (var t in ChildTreeItems)
            {
                if (t.NodeType != TreeNodeType.Rtu) t.UpdateNodeSelectByChildNodeSelected();
            }

            var allsame = true;
            var first = "";
            if (ChildTreeItems.Count > 0) first = ChildTreeItems[0]._schemeName;
            foreach (var t in ChildTreeItems)
            {
                if (t._schemeName != first)
                {
                    allsame = false;
                    break;
                }
            }

            if (allsame)
            {
                SchemeName = first;
            }
            else
            {
                SchemeName = "-混合-";
            }
            allsame = true;
            var ifirst = ChildTreeItems[0].IsSelected;
            foreach (var t in ChildTreeItems)
            {
                if (t.IsSelected != ifirst)
                {
                    allsame = false;
                    break;
                }
            }
            IsSelected = allsame && ifirst;
            //if(IsSelected )
            //{
            //    if (ChildTreeItems.Count > 0) SchemeName = ChildTreeItems[0].SchemeName;
            //}
        }
    };


    public enum TreeNodeType
    {
        UnKnown = 0,
        Rtu = 1,
        Group,
        Special
    };
}
