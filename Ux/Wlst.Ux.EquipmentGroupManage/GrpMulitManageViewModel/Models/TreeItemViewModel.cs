using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Services;
using Wlst.Ux.EquipmentGroupManage.Models;
using Wlst.Ux.EquipmentGroupManage.Resources;
using Wlst.Ux.EquipmentGroupManage.Services;
using Wlst.client;

namespace Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel.Models
{

    /// <summary>
    /// 提供终端树节点基本结构
    /// 右键菜单从MenuBuliding中动态获取，为节约程序资源，仅当点击该终端时该终端的右键菜单立即刷新
    /// IsMarked为联动标记，初始化时必须在外部初始化，即某一个分组具有联动则其子分组具有该联动属性
    /// </summary>
    public class TreeItemViewModel : ObservableObject
    {


        public TreeItemViewModel()
        {
        }

        private int AreaId = 0;
        private ObservableCollection<TreeItemViewModel> RootChilds;
        public TreeItemViewModel(TreeItemViewModel  mvvmFather,int areaId, object targetInfomation, bool IsGrp,ObservableCollection<TreeItemViewModel > rootChild )
        {
            RootChilds = rootChild;
            IsGroup = IsGrp;
            AreaId = areaId; 
            if (!IsGrp)
            {
                var terminalInfomation = targetInfomation as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
                if (terminalInfomation == null)
                {
                    this.NodeName = "加载出错";
                }
                else
                {
                    this.NodeName = terminalInfomation.RtuName;
                    //this.IconSource = ImageSource.TmlUnConnected;
                    ImagesSource = ImageResources .GetTmlTreeIcon(3) ;
                    this.NodeType = TreeNodeType.IsTml;
                    this.NodeId =terminalInfomation.RtuId;
                    this.TargetModel = (int )terminalInfomation.RtuModel;
                }
            }

            else
            {
                var groupInfomatioin = targetInfomation as GroupItemsInfo.GroupItem;
                if (groupInfomatioin == null)
                {
                    this.NodeName = "加载出错";
                }
                else
                {
                    this.NodeName = groupInfomatioin.GroupName;
                    //this.IconSource = ImageSource.GrpImage;
                    ImagesSource = ImageResources.GroupIcon;
                    this.NodeType = TreeNodeType.IsGrp;
                    this.NodeId = groupInfomatioin.GroupId;
                    this.TargetModel = -1;
                }
            }
            LabNameVisi = Visibility.Visible;
            TxbNameVisi = Visibility.Hidden;
            BackUpName = "";

            this._father = mvvmFather;
            _ChildTreeItems = new ObservableCollection<TreeItemViewModel>();

           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);
        }

        #region IEventAggregator Subscription

        public void FundEventHandler(PublishEventArgs args)
        {
            if (args.EventType == PublishEventType.Core && args.EventId == 52303)
            {
                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems  .ContainsKey(NodeId))
                {
                    var s = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems  [NodeId];
                        NodeName = s.RtuName;
                    

                }

            }
        }


        //ToDo 1、分组参数变更时 数据更新；2、分组、终端增删改变动时时数据更新 
        /// <summary>
        /// 事件过滤
        /// 目前只处理
        /// 1、系统当前选中的终端或分组变更，提供联动
        /// 2、终端参数发生变化的时候，即使更新显示数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            if (args.EventType == PublishEventType.Core && args.EventId == 52303 && NodeType == TreeNodeType.IsTml
                && NodeId == (int) args.GetParams()[0])
            {
                //switch (args.EventSection)
                //{
                //    case PublishEventSection.Update:
                //        return true;
                //}
                return false;
            }
            return false;
        }


        #endregion

        private ObservableCollection<TreeItemViewModel> _ChildTreeItems;

        public ObservableCollection<TreeItemViewModel> ChildTreeItems
        {
            get { return _ChildTreeItems; }
        }



        #region

        /// <summary>
        /// 父节点
        /// </summary>
        private readonly TreeItemViewModel _father;

        public TreeItemViewModel Father
        {
            get { return _father; }
        }

        //ToDo NodeModel 目前未使用
        /// <summary>
        /// 如果该节点为终端节点 则标记该终端类型
        /// 目前未使用
        /// </summary>
        public int TargetModel { get; set; }


        /// <summary>
        /// 设置本节点是否应该有右键菜单
        /// 为了节约程序运行资源  当用户选中终端时  刷新菜单
        /// 否则即使终端参数进行更新亦不刷新菜单
        /// </summary>
        public bool RefreshMenu { get; set; }

        /// <summary>
        /// 节点类型 是组节点 还是终端节点
        /// 或是其他节点
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

                //if (!_isSelected) return;
                //刷新右键菜单  用户可能会需要右键菜单 刷新
                //RefreshMenu = true;
                //this.RaisePropertyChanged(() => this.Cm);
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
                    if (_isExpanded && _father != null)
                    {
                        _father.IsExpanded = true;
                    }
                }
            }
        }

 


        private BitmapSource _imagesSource;

        /// <summary>
        /// 前台界面绑定的图标
        /// </summary>
        public BitmapSource ImagesSource
        {
            get { return _imagesSource; }
            set
            {
                if (_imagesSource != value)
                {
                    _imagesSource = value;
                    this.RaisePropertyChanged(() => this.ImagesSource);
                }
            }
        }

        public bool IsGroup;

        /// <summary>
        /// 终端地址或分组地址4为地址化
        /// </summary>
        public string FormatNodeId
        {
            get
            {
                if(!IsGroup)
                {
                    var tjmp = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                        GetInfoById(NodeId);
                    if(tjmp !=null )
                    return string.Format("{0:D4}",tjmp .RtuPhyId);
                    return string.Format("{0:D4}", NodeId);
                }
                return string.Format("{0:D4}", NodeId);
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


       

        #endregion




        public void AddChild()
        {
 
            _ChildTreeItems.Clear();
            if (NodeType != TreeNodeType.IsGrp) return;

            var tmltmp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpMultiInfoHoldNew.GetRtuInGroup(AreaId,this.NodeId);
            var _area = new AreaInfo.AreaItem();
            if(AreaId!= -1)
            {
                _area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
            }            
            foreach (var t in tmltmp)
            {
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems  .ContainsKey(t)) continue;
                if(AreaId !=-1 && !_area.LstTml.Contains(t)) continue;
                _ChildTreeItems.Add(new TreeItemViewModel(this,AreaId,
                                                          Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems[t],
                                                          false,null));
            }
        }



        /// <summary>
        /// 右键菜单
        /// </summary>
        //public ContextMenu Cm
        //{
        //    get
        //    {
        //        ContextMenu CM = new ContextMenu();
        //        try
        //        {
        //            if (TreeNodeType.IsTml == NodeType)
        //            {
        //                CM.Items.Add(DeleteFolder);
        //            }
        //            else if (TreeNodeType.IsGrp == NodeType)
        //            {
        //                // CM.Items.Add(AddFolder);
        //                CM.Items.Add(EditFolder);
        //                CM.Items.Add(DeleteFolder);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ex.ToString();
        //        }
        //        return CM;
        //    }
        //}

        /// <summary>
        /// 通过父节点事件删除终端 响应终端界面中的删除路径事件
        /// </summary>
        /// <param name="intNeedDeleteTmlId"></param>
        /// <param name="grpPath"></param>
        public void DeleteTmlNodeByFather(int intNeedDeleteTmlId, int grpId)
        {
            if (NodeType == TreeNodeType.IsGrp && grpId == NodeId)
            {

                foreach (var t in _ChildTreeItems)
                {
                    if (t.NodeType == TreeNodeType.IsTml && t.NodeId == intNeedDeleteTmlId)
                    {
                        _ChildTreeItems.Remove(t);
                        break;
                    }
                }

            }
        }

        #region EditFolder

        private MenuItem _EditFolder;

        private MenuItem EditFolder
        {
            get
            {

                _EditFolder = new MenuItem();
                _EditFolder.ToolTip = "编辑分组";
                _EditFolder.Header = "编辑";
                _EditFolder.Command = new RelayCommand(ExEditFolderFolder );
                // _EditFolder.CommandParameter = _MenuId;

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
        /// 标记界面上Lable可视化状态
        /// 当需要修改树节点名称事需要隐藏名称Lable
        /// </summary>
        private Visibility _LabNameVisi;

        public Visibility LabNameVisi
        {
            get { return _LabNameVisi; }
            set
            {
                if (value != _LabNameVisi)
                {
                    _LabNameVisi = value;
                    this.RaisePropertyChanged(() => this.LabNameVisi);
                }
            }
        }

        /// <summary>
        /// 标记界面text可视化状态 平时显示时不需要显示text
        /// 当需要修改树节点名称时需要隐藏Lable并显示此text供用户修改名称
        /// </summary>
        private Visibility _TxbNameVisi;

        public Visibility TxbNameVisi
        {
            get { return _TxbNameVisi; }
            set
            {
                if (value != _TxbNameVisi)
                {
                    _TxbNameVisi = value;
                    this.RaisePropertyChanged(() => this.TxbNameVisi);
                }
            }
        }

        /// <summary>
        /// 当修改树节点名称前需要备份前节点名称
        /// 若用户输入不合法则恢复原名称
        /// </summary>
        public string BackUpName;

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
            if (NodeName != BackUpName && BackUpName != "")
            {
                ////////////////////////////////////////////////// 汉化资源
                if (NodeType == TreeNodeType.IsGrp)
                {
       

                }
                else
                {
                    //非自定义资源 不允许修改名称
                    //需要修改再放开
                    NodeName = BackUpName;
                }
                if (NodeName.Length > 30) NodeName = NodeName.Substring(0, 29);
                if (NodeName.Length <1) NodeName = BackUpName ;
                BackUpName = NodeName;
            }
        }

        #endregion

        //#region AddFolder

        //private MenuItem _AddFolder;

        //private MenuItem AddFolder
        //{
        //    get
        //    {

        //        _AddFolder = new MenuItem();
        //        _AddFolder.ToolTip = "增加分组";
        //        _AddFolder.Header = "增加";
        //        _AddFolder.Command = new RelayCommand(ExAddFolderFolder);
        //        return _AddFolder;
        //    }
        //}

        //private void ExAddFolderFolder()
        //{
        //    if (TreeNodeType.IsGrp == NodeType)
        //    {
        //        int childId = getGrpsInfoForUse.GetAviableId();
        //        if (childId == -1) return;
        //        var gi = new GroupItemsInfo.GroupItem()
        //                     {
        //                         GroupId = childId,
        //                         GroupName = "新分组",
        //                         //MenuRightTargetKey = "RightMenuMulit"
        //                     };
        //        getGrpsInfoForUse.GrpInfoDictionary.Add(childId, gi);
        //        //if (getGrpsInfoForUse.GrpInfoDictionary.ContainsKey(NodeId))
        //        //{
        //        //    getGrpsInfoForUse.GrpInfoDictionary[NodeId].LstGrp.Add(childId);
        //        //}
        //        //本地分组待定
        //    }
        //    this.AddChild();
        //}

        //#endregion

        #region DeleteFolder

        private MenuItem _DeleteFolder;

        private MenuItem DeleteFolder
        {
            get
            {

                _DeleteFolder = new MenuItem();
                _DeleteFolder.ToolTip = "删除分组";
                _DeleteFolder.Header = "删除";
                _DeleteFolder.Command = new RelayCommand(ExDeleteFolderFolder );
                //_DeleteFolder.CommandParameter = _MenuId;

                return _DeleteFolder;
            }
        }

        private void ExDeleteFolderFolder()
        {
           
                if (NodeType == TreeNodeType.IsGrp)
                {
                    if (RootChilds != null && RootChilds.Contains(this)) RootChilds.Remove(this);
                }
                else if (NodeType == TreeNodeType.IsTml)
                {
                    if (NodeId != 0 && _father != null)
                    {
                        var father = this._father;
                        string pathArg = "" + NodeId;
                        while (father != null)
                        {
                            pathArg = father.NodeId + "-" + father.NodeName + "/" + pathArg;
                            father = father._father;
                        }

                        var args = new PublishEventArgs()
                                       {
                                           EventType = PublishEventTypeLocal.Name,
                                           EventId =
                                               PublishEventTypeLocal.DeleteGrpPathByCommandBasicShowGroupManage

                                       };
                        args.AddParams(NodeId, pathArg);
                        EventPublish.PublishEvent(args);
                    }
                }

            // if (_father != null) _father.AddChild();
        }

        #endregion


        public string GetSelectMVVMPath(string fatherPath)
        {
            if (_isSelected)
            {
                if (NodeType == TreeNodeType.IsTml)
                {
                    return fatherPath + NodeId;
                }
                else return "";
            }

            if (NodeType == TreeNodeType.IsGrp)
            {
                string fathPath = fatherPath + NodeId + "-" + NodeName + "/";
                for (int i = 0; i < _ChildTreeItems.Count; i++)
                {
                    if (_ChildTreeItems[i] != null)
                    {
                        string strReturn = _ChildTreeItems[i].GetSelectMVVMPath(fathPath);

                        if (!strReturn.Equals(""))
                        {
                            return strReturn;
                        }
                    }
                }

            }
            return "";
        }

        /// <summary>
        /// 为外部提供递归函数；
        /// 递归查询当前树节点选中的是哪一个节点
        /// </summary>
        /// <returns></returns>
        public TreeItemViewModel GetSelectMvvm()
        {
            if (_isSelected)
            {

                return this;

            }

            if (NodeType == TreeNodeType.IsGrp)
            {
                for (int i = 0; i < _ChildTreeItems.Count; i++)
                {
                    if (_ChildTreeItems[i] != null)
                    {
                        var strReturn = _ChildTreeItems[i].GetSelectMvvm();

                        if (strReturn != null)
                        {
                            return strReturn;
                        }
                    }
                }

            }
            return null;
        }



        private string _extendRtuCount;

        /// <summary>
        /// 终端地址或分组地址4为地址化+name
        /// </summary>
        public string ExtendRtuCount
        {
            get
            {
                if (this.NodeType == TreeNodeType.IsTml) return null;
                else if (this.NodeType == TreeNodeType.IsGrp || this.NodeType == TreeNodeType.IsGrpSpecial)
                {
                    _extendRtuCount = " [" + RtuCount + " 个]";
                }

                return _extendRtuCount;
            }
            set
            {
                if (value == _extendRtuCount) return;
                _extendRtuCount = value;
                this.RaisePropertyChanged(() => this.ExtendRtuCount);
            }
        }





        public int RtuCount;
        public void GetChildRtuCount()
        {
            if (this.NodeType == TreeNodeType.IsTml) return;
            //  if (this.NodeType == TypeOfTabTreeNode.IsAll) return;

            int count = 0;
            foreach (var t in this.ChildTreeItems)
            {
                //if (t.NodeType == TypeOfTabTreeNode.IsGrp || t.NodeType == TypeOfTabTreeNode.IsGrpSpecial)
                //{
                //    t.GetChildRtuCount();
                //    count += t.RtuCount;
                //}
                // if (Wlst .Sr .EquipmentInfoHolding .Services .EquipmentIdAssignRang .IsRtuIsRtuLight( t.NodeId ))  count += 1;
                if (t.NodeType == TreeNodeType.IsTml) count += 1;
                else
                {
                    t.GetChildRtuCount();
                    count += t.RtuCount;

                }


            }
            RtuCount = count;
            this.RaisePropertyChanged(() => this.ExtendRtuCount);
        }
    }
}
