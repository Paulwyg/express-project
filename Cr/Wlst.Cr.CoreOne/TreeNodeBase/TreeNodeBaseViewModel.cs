using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Cr.CoreOne.TreeNodeBase
{
    /// <summary>
    /// 继承此类 需要实现：OnNodeSelect;ResetCm
    /// </summary>
    [Serializable]
    public class TreeNodeBaseViewModel : ObservableObject, IITreeNodeBaseViewModel
    {
        private string _foreGround;

        /// <summary>
        /// 设置节点前景颜色
        /// </summary>
        public string ForeGround
        {
            get { return _foreGround; }
            set
            {
                if (value == _foreGround) return;
                _foreGround = value;
                this.RaisePropertyChanged(() => this.ForeGround);
            }
        }


        private string _backGround;

        /// <summary>
        /// 节点背景颜色
        /// </summary>
        public string BackGround
        {
            get { return _backGround; }
            set
            {
                if (value == _backGround) return;
                _backGround = value;
                this.RaisePropertyChanged(() => this.BackGround);
            }
        }



        private Visibility _isShowChkTree;

        public Visibility IsShowChkTree
        {
            get { return _isShowChkTree; }
            set
            {
                if (_isShowChkTree != value)
                {
                    _isShowChkTree = value;
                    this.RaisePropertyChanged(() => this.IsShowChkTree);
                }
            }
        }

        private Visibility _visi;

        public Visibility Visi
        {
            get { return _visi; }
            set
            {
                if (_visi != value)
                {
                    _visi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }

        private bool _isNoAlarm;

        public bool IsNoAlarm
        {
            get { return _isNoAlarm; }
            set
            {
                if (_isNoAlarm != value)
                {
                    _isNoAlarm = value;
                    this.RaisePropertyChanged(() => this.IsNoAlarm);
                }
            }
        }

        /// <summary>
        /// 当前节点是否为系统当前选中节点
        /// 1、需要向外发布终端树当前选中的节点 
        /// 2、如果是则用户可能需要右击终端弹出菜单 
        ///    此时需要刷新菜单
        /// </summary>
        private bool _isSelected;

        /// <summary>
        /// 当前终端被选中
        /// </summary>
        public virtual bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value && _isSelected)
                {
                    if (CmItems != null && CmItems.Count > 0)
                    {
                        ResetContextMenu();
                    }
                }
                if (value == _isSelected) return;
                _isSelected = value;
                this.RaisePropertyChanged(() => this.IsSelected);

                if (!_isSelected)
                {
                    OnNodeSelectDisActive();
                    return;
                }

                ResetContextMenu();
                OnNodeSelectActive();
            }
        }

        /// <summary>
        /// 当节点被选中的时候调用，实现了刷新右键菜单；
        /// 是否需要发送事件需要在此实现;以及其他的需要处理的事件;
        /// 动态加载子节点
        /// </summary>
        public virtual void OnNodeSelectActive()
        {
        }

        /// <summary>
        /// 当节点取消选中状态时;
        /// 如果节点有子节点则删除所有子节点可在此操作
        /// </summary>
        public virtual void OnNodeSelectDisActive()
        {
        }


        public virtual void ReloadChild(){}

        private bool _isExpanded;

        /// <summary>
        /// 当前节点是否展开
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.RaisePropertyChanged(() => this.IsExpanded);
                }
            }
        }

        private bool _isChecked;

        /// <summary>
        /// 是否被勾选
        /// </summary>
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (value != _isChecked)
                {
                    _isChecked = value;
                    this.RaisePropertyChanged(() => this.IsChecked);
                }
                OnNodeChecked();
            }
        }
        public virtual void OnNodeChecked()
        {
        }


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

                }
            }
        }

        private int _ctrlId;

        /// <summary>
        /// 节点ID  逻辑地址  
        /// </summary>
        public int CtrlId
        {
            get { return _ctrlId; }
            set
            {
                if (_ctrlId != value)
                {
                    _ctrlId = value;
                    this.RaisePropertyChanged(() => this.CtrlId);
                }
            }
        }

        private string _wuliId;

        /// <summary>
        /// 节点ID  物理地址  
        /// </summary>
        public string WuLiId
        {
            get { return _wuliId; }
            set
            {
                if (_wuliId != value)
                {
                    _wuliId = value;
                    this.RaisePropertyChanged(() => this.WuLiId);
                }
            }
        }

        private int _areaId;
        /// <summary>
        /// 区域ID    
        /// </summary>
        public int AreaId
        {
            get { return _areaId; }
            set
            {
                if (_areaId != value)
                {
                    _areaId = value;
                    this.RaisePropertyChanged(() => this.AreaId);
                }
            }
        }

        public virtual string NodeIdFormat
        {
            get { return string.Format("{0:D4}"+"-", NodeId); }
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


        private int _lightCount;

        /// <summary>
        /// 灯头数量
        /// </summary>
        public int LightCount
        {
            get { return _lightCount; }
            set
            {
                if (_lightCount != value)
                {
                    _lightCount = value;
                    this.RaisePropertyChanged(() => this.LightCount);
                }
            }
        }


        private string _nodeColor;

        /// <summary>
        /// 节点颜色
        /// </summary>
        public string NodeColor
        {
            get { return _nodeColor; }
            set
            {
                if (_nodeColor != value)
                {
                    _nodeColor = value;
                    this.RaisePropertyChanged(() => this.NodeName);
                }
            }
        }

        //private ObservableCollection<IIMenuItem> _nodeContextMenu;

        ///// <summary>
        ///// 菜单
        ///// </summary>
        //public ObservableCollection<IIMenuItem> NodeContextMenu
        //{
        //    get
        //    {
        //        if (_nodeContextMenu == null) _nodeContextMenu = new ObservableCollection<IIMenuItem>();
        //        return _nodeContextMenu;
        //    }
        //}

        private ObservableCollection<IIMenuItem> items;
        public ObservableCollection<IIMenuItem> CmItems
        {
            get
            {
                if (items == null) items = new ObservableCollection<IIMenuItem>();
                return items;
            }
            set
            {
                if (value == items) return;
                items = value;
                this.RaisePropertyChanged(() => this.CmItems);
            }
        }


        /// <summary>
        /// 更新菜单函数
        /// </summary>
        public virtual void ResetContextMenu()
        {
           // this.RaisePropertyChanged(() => this.Items);
        }
    }
}