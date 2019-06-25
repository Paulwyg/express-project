using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
 
using System.Collections.ObjectModel;

using System.Reflection;
using System.Windows;

using Wlst.Cr.Core.EventHandlerHelper;
using System.Windows.Media;
using System.Windows.Controls;
 
using Wlst.Cr.CoreMims.Commands;
 
using Wlst.Ux.Wj2096Module.Services;
using System.Windows.Input;
using Wlst.client;

namespace Wlst.Ux.Wj2096Module.FieldInfoSet.ViewModel
{
    /// <summary>
    /// 提供终端树节点基本结构
    /// </summary>
    public class TreeItemGrplViewModel : ObservableObject
    {
        public TreeItemGrplViewModel()
        {
        
        }

        private GrpFieldSluSglCtrl.GrpFieldSluSglItem GrpInfo = null;

       // private int SluId;
        public TreeItemGrplViewModel(object targetInfomation,int sluId, bool isGrp)
        {
            SluId = sluId;
            _isGroup = isGrp;
            if (!isGrp)
            {
                var terminalInfomation = targetInfomation as EquSluSgl.ParaSluCtrl;
                if (terminalInfomation == null)
                {
                    this.NodeName = "加载出错";
                }
                else
                {
                    this.NodeName = terminalInfomation.CtrlName ;
                    this.NodeType = TreeNodeType.SluNode ;
                    this.NodeId = terminalInfomation.CtrlId;
                }
            }
            else
            {
                var groupInfomatioin = targetInfomation as GrpFieldSluSglCtrl.GrpFieldSluSglItem;
                if (groupInfomatioin == null)
                {
                    this.NodeName = "加载出错";
                }
                else
                {
                    GrpInfo = groupInfomatioin;
                    this.NodeName = groupInfomatioin.GrpName;
                    this.NodeType = TreeNodeType.IsGrp;
                    this.NodeId = groupInfomatioin.GrpId;
                }
            }
            LabNameVisi = Visibility.Visible;
            TxbNameVisi = Visibility.Hidden;
            BackUpName = "";

        }


        #region 定义数据

        private ObservableCollection<TreeItemGrplViewModel> _ChildTreeItems;

        /// <summary>
        /// 保存本节点下的子节点结构
        /// </summary>
        public ObservableCollection<TreeItemGrplViewModel> ChildTreeItems
        {
            get
            {
                if (_ChildTreeItems == null) _ChildTreeItems = new ObservableCollection<TreeItemGrplViewModel>();
                return _ChildTreeItems;
            }
        }

        /// <summary>
        /// 节点类型 是组节点 还是终端节点
        /// </summary>
        public TreeNodeType NodeType { get; set; }


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

        /// <summary>
        /// 当前节点是否为系统当前选中节点
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

        private bool _isTreeChecked;

        /// <summary>
        /// 是否选中该条数据
        /// </summary>
        public bool IsTreeChecked
        {
            get { return _isTreeChecked; }
            set
            {
                if (value != _isTreeChecked)
                {
                    _isTreeChecked = value;
                    this.RaisePropertyChanged(() => this.IsTreeChecked);
                }
                if (NodeType == TreeNodeType.IsGrp)
                {
                    foreach (var t in ChildTreeItems)
                    {
                        t.IsTreeChecked = _isTreeChecked;
                    }
                }
            }
        }

        private bool _isGroup;

        /// <summary>
        /// 终端地址或分组地址4为地址化
        /// </summary>
        public string FormatNodeId
        {
            get
            {
                if (!_isGroup)
                {
                    var res = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( NodeId);
                    return string.Format("{0:D4}", res==null ?NodeId :res .RtuPhyId );
                }
                return string.Format("{0:D4}", NodeId);
            }
        }

        private int _sluId;

        /// <summary>
        /// 集中器的地址
        /// </summary>
        public int SluId
        {
            get { return _sluId; }
            set
            {
                if (_sluId != value)
                {
                    _sluId = value;
                    this.RaisePropertyChanged(() => this.SluId);
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
                    this.RaisePropertyChanged(() => this.FormatNodeId);
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


        /// <summary>
        /// 标记界面上Lable可视化状态；辅助显示使用
        /// 当需要修改树节点名称事需要隐藏名称Lable
        /// </summary>
        private Visibility _labNameVisi;

        public Visibility LabNameVisi
        {
            get { return _labNameVisi; }
            set
            {
                if (value != _labNameVisi)
                {
                    _labNameVisi = value;
                    this.RaisePropertyChanged(() => this.LabNameVisi);
                }
            }
        }

        /// <summary>
        /// 标记界面text可视化状态 平时显示时不需要显示text；辅助显示使用
        /// 当需要修改树节点名称时需要隐藏Lable并显示此text供用户修改名称
        /// </summary>
        private Visibility _txbNameVisi;

        public Visibility TxbNameVisi
        {
            get { return _txbNameVisi; }
            set
            {
                if (value != _txbNameVisi)
                {
                    _txbNameVisi = value;
                    this.RaisePropertyChanged(() => this.TxbNameVisi);
                }
            }
        }

        #endregion


        /// <summary>
        /// 第一次加载时 使用;
        /// 此函数是为节点加载子节点，从dataholding中读取数；
        /// 仅第一次加载节点时使用
        /// </summary>
        public void AddChild()
        {
            if (GrpInfo == null) return;
            //_ChildTreeItems.Clear();
            //var infos = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(SluId);
            //if (infos == null) return;
            foreach (var g in GrpInfo.CtrlLst)
            {
                var infos = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(g);
                if (infos == null) continue;

                this.ChildTreeItems.Add(new TreeItemGrplViewModel(infos, g, false));
            }
        }

        /// <summary>
        /// 右键菜单
        /// </summary>
        public ContextMenu Cm
        {
            get
            {
                ContextMenu CM = new ContextMenu();
                try
                {
                    if (TreeNodeType.IsGrp == NodeType)
                    {
                        //if (_father != null)
                        //{
                        CM.Items.Add(EditFolder);
                        //}
                    }
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
                return CM;
            }
        }

        #region EditFolder

        private MenuItem _EditFolder;

        private MenuItem EditFolder
        {
            get
            {

                _EditFolder = new MenuItem();
                _EditFolder.ToolTip = "编辑分组"; // "Edit a folder";
                _EditFolder.Header = "编辑";
                _EditFolder.Command = new RelayCommand(ExEditFolderFolder);
                return _EditFolder;
            }
        }

        private void ExEditFolderFolder()
        {
            if (NodeType == TreeNodeType.IsGrp)
            {
                StartEditName();
            }
        }

        /// <summary>
        /// 当修改树节点名称前需要备份前节点名称
        /// 若用户输入不合法则恢复原名称
        /// </summary>
        private string BackUpName;

        /// <summary>
        /// 开始编辑名称，此时需要备份名称并控制前台显示状态
        /// </summary>
        public void StartEditName()
        {
            if (NodeType == TreeNodeType.IsGrp)
            {
                BackUpName = NodeName;
                LabNameVisi = Visibility.Collapsed;
                TxbNameVisi = Visibility.Visible;
            }
        }

        /// <summary>
        /// 停止编辑名称，此时需要进行前台空间状态变换并回写数据
        /// </summary>
        public void StopEditName()
        {
            LabNameVisi = Visibility.Visible;
            TxbNameVisi = Visibility.Collapsed;
            if (NodeName != BackUpName && NodeName != "")
            {
                if (NodeType == TreeNodeType.IsGrp)
                {
                    //需要对所有标记本分组的终端宣布新分组信息
                    //var args = new PublishEventArgs()
                    //               {
                    //                   EventType = PublishEventTypeSlu.Name,
                    //                   EventId = PublishEventTypeSlu.GrpReloadTmlGroup
                    //               };
                    //EventPublish.PublishEvent(args);
                }

                BackUpName = NodeName;
            }
            else
            {
                //不修改
                NodeName = BackUpName;
            }
        }

        #endregion
    }

    public enum TreeNodeType
    {
        IsGrp = 1,
        SluNode,
    }
}
