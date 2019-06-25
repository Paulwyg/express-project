using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.CoreInterface;
 
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.LhEquipemntTree.Models;

namespace Wlst.Ux.LhEquipemntTree.Services
{
    /// <summary>
    /// 继承此类 需要实现：OnNodeSelect;ResetCm
    /// </summary>
    public class BasicTreeNodeViewModel : ObservableObject, IITreeNode
    {
        /// <summary>
        /// 为了节约程序运行资源  当用户选中终端时  刷新菜单
        /// 否则即使终端参数进行更新亦不刷新菜单
        /// 标志菜单是否允许刷新
        /// </summary>
        private bool IsCanRefreshMenu { get; set; }

        #region


        /// <summary>
        /// 节点类型 是组节点 还是终端节点
        /// 或是其他节点
        /// </summary>
        public TreeNodeType NodeType { get; set; }

        /// <summary>
        /// 如果节点为终端下的外部设备则设备类型
        /// </summary>
        public  TreeNodeModel NodeModel { get; set; }


        private Color _foreGround;

        /// <summary>
        /// 设置节点前景颜色
        /// </summary>
        public Color ForeGround
        {
            get { return _foreGround; }
            set
            {
                if (value == _foreGround) return;
                _foreGround = value;
                this.RaisePropertyChanged(() => this.ForeGround);
            }
        }


        private Color _backGround;
        /// <summary>
        /// 节点背景颜色
        /// </summary>
        public Color BackGround
        {
            get { return _backGround; }
            set
            { 
                if (value == _backGround) return;
                _backGround = value;
                this.RaisePropertyChanged(() => this.BackGround);
            }
        }

        

        private Visibility _visi;

        /// <summary>
        /// 设置节点是否可见
        /// </summary>
        public Visibility Visi
        {
            get { return _visi; }
            set
            {
                if (value == _visi) return;
                _visi = value;
                this.RaisePropertyChanged(() => this.Visi);
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
                IsCanRefreshMenu = true;
                this.ResetCm();
                IsCanRefreshMenu = false;
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
                ////发布事件  选中当前节点
                //var eventId = 52305;
                //if (this.NodeType == TreeNodeType.IsGrp)
                //    eventId = 10405;
                //var args = new PublishEventArgs
                //               {
                //                   EventType = Infrastructure.Services.PublishEventArgsType.TreePublish,
                //                   EventId = eventId,
                //               };
                //args.AddParams(NodeId);
                //PrismEventExtend.EventHelper.EventPublish.PublishEvent(args);
        }

        /// <summary>
        /// 当节点取消选中状态时;
        /// 如果节点有子节点则删除所有子节点可在此操作
        /// </summary>
        public virtual void OnNodeSelectDisActive()
        {
            
        }


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



        private object  _imagesIcon;

        /// <summary>
        /// 前台界面绑定的图标
        /// </summary>
        public object  ImagesIcon
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
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public string FormatNodeName
        {
            get
            {
                if (NodeType == TreeNodeType.IsTml)//终端的话  则检阅是否需要加地址
                {
                    if (UxTreeSetting .IsShowGrpInTreeModelShowId )
                        return string.Format("{0:D4}", NodeId) + "-" + NodeName;
                    else
                        return NodeName;
                }
                return NodeName;//非终端  则直接显示名称
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
                    this.RaisePropertyChanged(() => this.FormatNodeName);
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
                    this.RaisePropertyChanged(() => this.FormatNodeName);
                }
            }
        }

        #endregion


        #region 右键菜单  在节点被选中的时候显示刷新右键菜单


    
       /// <summary>
       /// 具体实现如何生成菜单  ；
       /// 此为一级菜单
       /// </summary>
       /// <returns></returns>
        public virtual void  ResetCm()
       {
           return ;
       }

        ///// <summary>
        ///// 重新生产右键菜单
        ///// </summary>
        //private void ResetM()
        //{
            //if (_cm == null) _cm = new ContextMenu();
            //if (_menuBuilding == null) _menuBuilding = new MenuBuilding();

            //_cm.Items.Clear();
            //List<MenuItem> t = null;
            //switch (NodeType)
            //{
            //    case TreeNodeType.IsTml:
            //        if (SupperTmlsInfoHolding.TmlBasicInfoDictionary.ContainsKey(NodeId))
            //        {
            //            var tt = SupperTmlsInfoHolding.TmlBasicInfoDictionary[NodeId];
            //            var s = tt as IIBasicTmlInfomation;
            //            if (s != null)
            //            {
            //                t = _menuBuilding.BulidCm(s.Model.ToString(), false,
            //                                          SupperTmlsInfoHolding.TmlBasicInfoDictionary[NodeId]);
            //            }

            //        }
            //        break;
            //    case TreeNodeType.IsGrp:
            //        if (GrpMultiInfoHolding.GrpInfoDictionary.ContainsKey(NodeId))
            //            t = _menuBuilding.BulidCm(GrpSingleInfoHolding.GrpInfoDictionary[NodeId].MenuRightTargetKey,
            //                                      false,
            //                                      GrpSingleInfoHolding.GrpInfoDictionary[NodeId]);
            //        break;
            //}
            //if (t == null) return;
            //foreach (var tt in t)
            //{
            //    _cm.Items.Add(tt);
            //}
        //}

        #endregion




        private ObservableCollection<IIMenuItem> _items;

        /// <summary>
        /// 本菜单下的子菜单 ，不允许操作;系统执行操作
        /// </summary>
        public ObservableCollection<IIMenuItem> CmItems
        {
            get { return _items ?? (_items = new ObservableCollection<IIMenuItem>()); }
            // set { _items = value; }
        }
    }
}
