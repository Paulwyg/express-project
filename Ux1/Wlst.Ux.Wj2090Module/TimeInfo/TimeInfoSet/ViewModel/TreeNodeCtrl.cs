using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.TimeInfo.ViewModel
{
    public class TreeNodeCtrl : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        private bool _isSelected;

        /// <summary>
        /// 当前终端被选中
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                this.RaisePropertyChanged(() => this.IsSelected);
                OnIsSelectedChanged();
            }
        }

        public virtual void OnIsSelectedChanged()
        {

        }


        private int _nodeId;

        /// <summary>
        /// 节点ID  终端地址或分组地址  
        /// </summary>
        public int NodeId
        {
            get { return _nodeId; }
            set
            {
                if (_nodeId != value)
                {
                    _nodeId = value;
                    this.RaisePropertyChanged(() => this.NodeId);
                    //this.RaisePropertyChanged(() => this.NodeIdFormat);
                }
            }
        }

        //private int _ctrlPhyId;
        // private int CtrlPhyId
        //{
        //    get { return _ctrlPhyId; }
        //    set
        //    {
        //        if (_ctrlPhyId != value)
        //        {
        //            _ctrlPhyId = value;
        //            this.RaisePropertyChanged(() => this.CtrlPhyId);
        //            this.RaisePropertyChanged(() => this.NodeIdFormat);
        //        }
        //    }
        //}

        public virtual string NodeIdFormat
        {
            get { return string.Format("{0:D3}" + "-", NodeId); }
        }

        private string _nodeName;

        /// <summary>
        /// 节点名称  终端名称或是分组名称
        /// </summary>
        public string NodeName
        {
            get { return _nodeName; }
            set
            {
                if (_nodeName != value)
                {
                    _nodeName = value;
                    this.RaisePropertyChanged(() => this.NodeName);
                }
            }
        }


        private string _isSelsfdected;

        /// <summary>
        /// 当前终端被选中
        /// </summary>
        public string Count
        {
            get { return _isSelsfdected; }
            set
            {
                if (value == _isSelsfdected) return;
                _isSelsfdected = value;
                this.RaisePropertyChanged(() => this.Count);
            }
        }
    }


    public class TreeNodeGrp : TreeNodeCtrl
    {
        private ObservableCollection<TreeNodeCtrl> _childTreeItemsInfo;

        public ObservableCollection<TreeNodeCtrl> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<TreeNodeCtrl>();
                return _childTreeItemsInfo;
            }
            set
            {
                if (value != _childTreeItemsInfo)
                {
                    _childTreeItemsInfo = value;
                    this.RaisePropertyChanged(() => this.ChildTreeItems);
                    if (_childTreeItemsInfo != null)
                        this.Count = " - " + _childTreeItemsInfo.Count;
                    else
                        this.Count = " - 0";
                }
            }
        }

        public void SetCount()
        {
            if (_childTreeItemsInfo != null)
            {
                int xcount = 0;
                foreach (var g in this._childTreeItemsInfo)
                {
                    if (g.IsSelected) xcount++;
                }
                this.Count = " - " + xcount + "/" + _childTreeItemsInfo.Count;
            }
            else
                this.Count = " - 0";
        }


        public bool IsSelectedByView = true;

        public override void OnIsSelectedChanged()
        {
            //base.OnIsSelectedChanged();
            if (IsSelectedByView)
            {
                foreach (var g in ChildTreeItems) g.IsSelected = this.IsSelected;
            }
        }

        public void GetThisCheckByChild()
        {
            bool allchecked = true;
            foreach (var g in this.ChildTreeItems)
            {
                if (g.IsSelected == false)
                {
                    allchecked = false;
                }
            }
            if (allchecked) this.IsSelected = true ;
        }

    }
}
