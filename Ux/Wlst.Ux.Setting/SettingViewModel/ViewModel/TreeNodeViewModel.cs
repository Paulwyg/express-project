using System.Collections.ObjectModel;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;

namespace Wlst.Ux.Setting.SettingViewModel.ViewModel //InfrastructureSettingView
{
    public class TreeNodeViewModel : ObservableObject,Services.IITreeNode
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewId">如果为导航节点 必须赋值，如果为母节点 则赋值0</param>
        /// <param name="name">显示名称</param>
        public TreeNodeViewModel(int viewId, string name)
        {
            this.ViewId = viewId;
            this.NodeName = name;
        }

        private ObservableCollection<TreeNodeViewModel> _childTreeItems;

        public ObservableCollection<TreeNodeViewModel> ChildTreeItems
        {
            get
            {
                if (_childTreeItems == null) _childTreeItems = new ObservableCollection<TreeNodeViewModel>();
                return _childTreeItems;
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
                if (value)
                {
                    OnNodeSelect();
                }
            }
        }

        /// <summary>
        /// 当节点被选中的时候调用，实现了刷新右键菜单；
        /// 是否需要发送事件需要在此实现;以及其他的需要处理的事件;
        /// 动态加载子节点
        /// </summary>
        public void OnNodeSelect()
        {
            if (ViewId > 1000)
            {
                //发布事件  选中当前节点
                var args = new PublishEventArgs
                               {
                                   EventType = SettingViewModel.EventType,
                                   EventId = SettingViewModel.EventId,
                               };
                args.AddParams(ViewId);
                EventPublish.PublishEvent(args);
            }
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


        private int _nodeId;

        /// <summary>
        /// 
        /// </summary>
        public int ViewId
        {
            get { return _nodeId; }
            set
            {
                if (_nodeId != value)
                {
                    _nodeId = value;
                    this.RaisePropertyChanged(() => this.ViewId);
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
    }
}