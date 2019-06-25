using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Ux.Wj2096Module.TreeTab.vm
{
    public class NodeItemBase : ObservableObject
    {

        public void Sort(List<int> sortlst)
        {
            var slsdfs = (from t in ChildItems where sortlst.Contains(t.NodeId) select t.NodeId).ToList();
            var st = (from t in sortlst where slsdfs.Contains(t) select t).Distinct().ToList();
            var notex = (from t in ChildItems where st.Contains(t.NodeId) == false select t).ToList();
            foreach (var f in notex)
            {
                if (ChildItems.Contains(f)) ChildItems.Remove(f);
            }


            for (int i = 0; i < ChildItems.Count; i++)
            {
                if (ChildItems[i].NodeId == st[i]) continue;
                foreach (var l in ChildItems)
                    if (l.NodeId == st[i])
                    {
                        var tmp = l;
                        ChildItems.Remove(l);
                        ChildItems.Insert(i, tmp);
                        break;
                    }
            }
            foreach (var f in notex) ChildItems.Add(f);
        }

        public int SortIndex = 9999999;
        public int AreaId = -1;

        /// <summary>
        /// 1、区域，2、终端分组，3、单灯域，4、单灯分组，5、单灯控制器
        /// </summary>
        public int NodeTypeLevel = -1;

        ///// <summary>
        ///// 为了节约程序运行资源  当用户选中终端时  刷新菜单
        ///// 否则即使终端参数进行更新亦不刷新菜单
        ///// 标志菜单是否允许刷新
        ///// </summary>
        //private bool IsCanRefreshMenu { get; set; }

        #region


        private ObservableCollection<NodeItemBase> _childTreeItemsInfo = null;

        public ObservableCollection<NodeItemBase> ChildItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<NodeItemBase>();

                return _childTreeItemsInfo;
            }
            //set
            //{
            //    if(value != _childTreeItemsInfo )
            //    {
            //        _childTreeItemsInfo = value;
            //        this.RaisePropertyChanged(() => this.ChildItems);
            //    }
            //}
        }


        //private Color _foreGround;

        ///// <summary>
        ///// 设置节点前景颜色
        ///// </summary>
        //public Color ForeGround
        //{
        //    get { return _foreGround; }
        //    set
        //    {
        //        if (value == _foreGround) return;
        //        _foreGround = value;
        //        this.RaisePropertyChanged(() => this.ForeGround);
        //    }
        //}


        //private Color _backGround;
        ///// <summary>
        ///// 节点背景颜色
        ///// </summary>
        //public Color BackGround
        //{
        //    get { return _backGround; }
        //    set
        //    {
        //        if (value == _backGround) return;
        //        _backGround = value;
        //        this.RaisePropertyChanged(() => this.BackGround);
        //    }
        //}




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
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                this.RaisePropertyChanged(() => this.IsSelected);

                if (!_isSelected)
                {
                    OnNodeSelectDisActive();
                    return;
                }
                //刷新右键菜单  用户可能会需要右键菜单 刷新
                //IsCanRefreshMenu = true;
                this.ResetCm();
                //IsCanRefreshMenu = false;
                OnNodeSelect();
            }
        }

        /// <summary>
        /// 当节点被选中的时候调用，实现了刷新右键菜单；
        /// 是否需要发送事件需要在此实现;以及其他的需要处理的事件;
        /// 动态加载子节点
        /// </summary>
        public virtual void OnNodeSelect()
        {

        }

        /// <summary>
        /// 当节点取消选中状态时;
        /// 如果节点有子节点则删除所有子节点可在此操作
        /// </summary>
        public virtual void OnNodeSelectDisActive()
        {

        }


        public virtual void UpdateChildImage(List<int> ctrlids)
        {

        }
        public virtual void UpdateChildPara(List<int> ctrlids)
        {
            
        }
        public virtual void UpdateShowInfo()
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


        /// <summary>
        /// id
        /// </summary>
        public int NodeId;
        private string _nodeId;

        /// <summary>
        /// 节点ID  终端地址或分组地址  
        /// </summary>
        public string  NodeShowId
        {
            get { return _nodeId; }
            set
            {
                if (_nodeId != value)
                {
                    _nodeId = value;
                    this.RaisePropertyChanged(() => this.NodeShowId);
                }
            }
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

        #endregion



        /// <summary>
        /// 具体实现如何生成菜单  ；
        /// 此为一级菜单
        /// </summary>
        /// <returns></returns>
        public virtual void ResetCm()
        {
        }





        private ObservableCollection<IIMenuItem> _items = null;

        /// <summary>
        /// 本菜单下的子菜单 ，不允许操作;系统执行操作
        /// </summary>
        public ObservableCollection<IIMenuItem> CmItems
        {
            get { return _items ?? (_items = new ObservableCollection<IIMenuItem>()); }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => this.CmItems);
            }
        }
    }
}
