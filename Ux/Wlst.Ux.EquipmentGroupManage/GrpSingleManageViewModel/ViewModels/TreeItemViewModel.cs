﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
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
using Wlst.Ux.EquipmentGroupManage.Models;
using Wlst.Ux.EquipmentGroupManage.Resources;
using Wlst.Ux.EquipmentGroupManage.Services;
using Wlst.client;

namespace Wlst.Ux.EquipmentGroupManage.GrpSingleManageViewModel.ViewModels
{

    /// <summary>
    /// 提供终端树节点基本结构
    /// </summary>
    public class TreeItemViewModel : ObservableObject
    {
        public TreeItemViewModel()
        {
        }

        private int AreaId = 0;
        public TreeItemViewModel(TreeItemViewModel mvvmFather,int areaId, object targetInfomation, bool IsGrp)
        {
            AreaId = areaId;
            IsGroup = IsGrp;
            if (!IsGrp)
            {
                var terminalInfomation = targetInfomation as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
                if (terminalInfomation == null)
                {
                    this.NodeName = "加载出错";
                }
                else
                {
                    this.NodeName = terminalInfomation.RtuName;                    
                    this.NodeType = TreeNodeType.IsTml;
                    this.NodeId = terminalInfomation.RtuId;
                    if(this.NodeId <1499999 && this.NodeId >=1399999)
                    {
                        this.ImagesSource = ImageResources.LuxIcon;
                    }
                    else if(this.NodeId >=1499999 && this.NodeId < 1599999)
                    {
                        this.ImagesSource = ImageResources.SluIcon;
                    }
                    else
                    {
                        this.ImagesSource = ImageResources.RtuIcon;
                    }
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
                    ImagesSource = ImageResources.GroupIcon;
                    this.NodeType = TreeNodeType.IsGrp;
                    this.NodeId = groupInfomatioin.GroupId;
                    //MenuRightTargetKey = groupInfomatioin.MenuRightTargetKey;
                    //Status = groupInfomatioin.Status;
                }
            }
            LabNameVisi = Visibility.Visible;
            TxbNameVisi = Visibility.Hidden;
            //BackUpName = "";

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
                    var t = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems  [NodeId];

                        NodeName = t.RtuName;
                    

                }

            }
        }


        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.Core && args.EventId == 52303 && //终端信息修改
                    NodeType == TreeNodeType.IsTml && NodeId == (int) args.GetParams()[0])
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }


        #endregion
        
        
        #region 定义数据
        
        private ObservableCollection<TreeItemViewModel> _ChildTreeItems;
        /// <summary>
        /// 保存本节点下的子节点结构
        /// </summary>
        public ObservableCollection<TreeItemViewModel> ChildTreeItems
        {
            get { return _ChildTreeItems; }
        }

        /// <summary>
        /// 仅提供保存分组信息的右键菜单关键字信息
        /// </summary>
        public string MenuRightTargetKey;
        /// <summary>
        /// 仅提供保存分组信息状态
        /// </summary>
        public int Status;

        private readonly TreeItemViewModel _father;

        /// <summary>
        /// 本节点父节点
        /// </summary>
        public TreeItemViewModel Father
        {
            get { return _father; }
        }

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
                    var res = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( NodeId);
                    if (res != null) return string.Format("{0:D4}", res.RtuPhyId );
                    else return string.Format("{0:D4}", NodeId); 
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
        [StringLength(10, ErrorMessage = "输入需小于10个字符")]  
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
            _ChildTreeItems.Clear();
            if (NodeType != TreeNodeType.IsGrp) return;

            var grp = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(AreaId, NodeId);
            if (grp == null) return;

            //加载终端节点
            var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
            var ntsss = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);

            foreach (
                var t in
                    ntsss)
            {
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                    continue;
                if (tmlLstOfArea.Contains(t) == false) continue;
                var f = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                _ChildTreeItems.Add(new TreeItemViewModel(this, AreaId, f, false));
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



        #region EditFolder

        private MenuItem _EditFolder;

        private MenuItem EditFolder
        {
            get
            {

                _EditFolder = new MenuItem();
                _EditFolder.ToolTip = "编辑";//"I36N .Services .I36N .ConvertByCoding("11020011");// "Edit a folder";
                _EditFolder.Header = "编辑";//"I36N .Services .I36N .ConvertByCoding("11020012");
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
                if (NodeType == TreeNodeType.IsGrp)
                {
                    //需要对所有标记本分组的终端宣布新分组信息
                    var args = new PublishEventArgs()
                                   {
                                       EventType = PublishEventTypeLocal.Name,
                                       EventId =
                                           PublishEventTypeLocal.GrpSingleReloadTmlGroup
                                   };
                    EventPublish.PublishEvent(args);

                }
                else
                {
                    //不修改
                    NodeName = BackUpName;
                }
                if (NodeName.Length > 30) NodeName = NodeName.Substring(0, 29);
                if (NodeName.Length < 1) NodeName = BackUpName;
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
        //        _AddFolder.ToolTip = "添加";//"I36N .Services .I36N .ConvertByCoding("11020013");
        //        _AddFolder.Header = "添加";//I36N .Services .I36N .ConvertByCoding("11020014");
        //        _AddFolder.Command = new RelayCommand(ExAddFolderFolder);
        //        return _AddFolder;
        //    }
        //}

        //private void ExAddFolderFolder()
        //{
        //    if (TreeNodeType.IsGrp == NodeType)
        //    {
        //        var motherMv = this;
        //        while (motherMv != null && motherMv.NodeId != 0)
        //        {
        //            motherMv = motherMv.Father;
        //        }
        //        if (motherMv == null) return;
        //        if (motherMv.NodeId != 0) return;

        //        int childId = motherMv.GetMaxGrpId();
        //        if (childId < 10) return;

        //        if (childId == -1) return;
        //        var gi = new GroupItemsInfo.GroupItem()
        //                     {
        //                         GroupId = childId,
        //                         GroupName = "New Group",
        //                         //MenuRightTargetKey = "WJ3005",
        //                         //Status = 2,
        //                     };


        //        this.ChildTreeItems.Add(new TreeItemViewModel(this, gi, true));
        //    }
        //}

        //public int GetMaxGrpId()
        //{
        //    if (NodeType != TreeNodeType.IsGrp || NodeId != 0) return 0;
        //    int max = Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GroupStartId;
        //    foreach (var t in this.ChildTreeItems)
        //    {
        //        if (t.NodeType == TreeNodeType.IsGrp)
        //        {
        //            if (t.NodeId > max) max = t.NodeId;
        //            int gid = t.GetMaxGrpId(max);
        //            if (gid > max) max = gid;
        //        }
        //    }


        //    return max + 1;
        //}

        public int GetMaxGrpId(int id)
        {
            int max = id;
            if (NodeType != TreeNodeType.IsGrp) return 0;
            foreach (var t in this.ChildTreeItems)
            {
                if (t.NodeType == TreeNodeType.IsGrp)
                {
                    if (t.NodeId > max) max = t.NodeId;
                    int gid = t.GetMaxGrpId(max);
                    if (gid > max) max = gid;
                }
            }
            if (max > id) return max;
            return 0;
        }

        //#endregion

        #region DeleteFolder

        private MenuItem _DeleteFolder;

        private MenuItem DeleteFolder
        {
            get
            {

                _DeleteFolder = new MenuItem();
                _DeleteFolder.ToolTip = "删除";//I36N .Services .I36N .ConvertByCoding("11020015");
                _DeleteFolder.Header = "删除";//I36N .Services .I36N .ConvertByCoding("11020015");
                _DeleteFolder.Command = new RelayCommand(ExDeleteFolderFolder);

                return _DeleteFolder;
            }
        }

        private void ExDeleteFolderFolder()
        {
            int nodid = this.NodeId;
            if (NodeId != 0 && _father != null)//todo  youhua
            {
                if (NodeType == TreeNodeType.IsGrp)
                {
                    //int fatherId = _father.NodeId;
                    //getGrpsInfoForUse.DeleteGrpInfo(NodeId, fatherId);
                    if (_father != null)
                    {
                        _father._ChildTreeItems.Remove(this);

                        var args = new PublishEventArgs()
                                       {
                                           EventType = PublishEventTypeLocal.Name,
                                           EventId =
                                               PublishEventTypeLocal.GrpSingleReloadTmlGroup
                                       };
                        args.AddParams(nodid);
                        args.AddParams(0);
                        EventPublish.PublishEvent(args);
                    }
                }
                else if (NodeType == TreeNodeType.IsTml)
                {
                    //if (getGrpsInfoForUse.GrpInfoDictionary.ContainsKey(this.Father.NodeId))
                    //{
                    //    if (getGrpsInfoForUse.GrpInfoDictionary[this.Father.NodeId].LstTml.Contains(this.NodeId))
                    //    {
                    //        getGrpsInfoForUse.GrpInfoDictionary[this.Father.NodeId].LstTml.Remove(this.NodeId);
                    //    }
                    //}
                    this._father.ChildTreeItems.Remove(this);

                    var args = new PublishEventArgs()
                                   {
                                       EventType = PublishEventTypeLocal.Name,
                                       EventId =
                                           PublishEventTypeLocal.GrpSingleReloadTmlGroup
                                   };
                    args.AddParams(nodid);
                    args.AddParams(1);
                    EventPublish.PublishEvent(args);
                }
            }

            // if (_father != null) _father.AddChild();
        }

        #endregion

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

        /// <summary>
        /// 递归删除标记为终端的节点且其地址为TmlId
        /// </summary>
        /// <param name="tmlId"></param>
        public void DeleteTreeNode(int tmlId)
        {
            for (int i = 0; i < this.ChildTreeItems.Count; i++)
            {
                if (this.ChildTreeItems[i].NodeType == TreeNodeType.IsGrp)
                {
                    this.ChildTreeItems[i].DeleteTreeNode(tmlId);
                }
                else if (this.ChildTreeItems[i].NodeType == TreeNodeType.IsTml)
                {
                    if (this.ChildTreeItems[i].NodeId == tmlId)
                    {
                        this.ChildTreeItems.RemoveAt(i);
                        break;
                    }
                }
            }
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

        



        public int  RtuCount;
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