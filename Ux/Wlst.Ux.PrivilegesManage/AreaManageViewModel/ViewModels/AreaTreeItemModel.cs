using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.PrivilegesManage.AreaManageViewModel.Models;
using Wlst.Ux.PrivilegesManage.AreaManageViewModel.Resources;
using Wlst.Ux.PrivilegesManage.AreaManageViewModel.Services;
using Wlst.Ux.PrivilegesManage.Services;
using Wlst.client;

namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel.ViewModels
{
    public class AreaTreeItemModel:ObservableObject
    {
        
            public AreaTreeItemModel(){}

       

            public AreaTreeItemModel(AreaTreeItemModel mvvmFather, object targetInfomation, bool IsPartition)
            {
                _isPartition = IsPartition;
                if (!IsPartition)
                {
                    var terminalInfomation = targetInfomation as EquipmentParameter;
                    if (terminalInfomation == null)
                    {
                        this.NodeName = "加载出错";
                    }
                    else
                    {
                        this.NodeName = terminalInfomation.RtuName;
                        this.NodeType = TreeNodeType.IsTml;
                        this.NodeId = terminalInfomation.RtuId;
                        if (this.NodeId < 1499999 && this.NodeId >= 1399999)
                        {
                            this.ImagesSource = ImageResources.LuxIcon;
                        }
                        else if (this.NodeId >= 1499999 && this.NodeId < 1599999)
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
                    var groupInfomatioin = targetInfomation as AreaInfo.AreaItem;
                    if (groupInfomatioin == null)
                    {
                        this.NodeName = "加载出错";
                    }
                    else
                    {
                        this.NodeName = groupInfomatioin.AreaName;
                        ImagesSource = ImageResources.GroupIcon;
                        this.NodeType = TreeNodeType.IsPartition;
                        this.NodeId = groupInfomatioin.AreaId;
                   

                    }
                }

                LabNameVisi = Visibility.Visible;
                TxbNameVisi = Visibility.Hidden;
                BackUpName = "";

                this._father = mvvmFather;
                _areaTreeItem = new ObservableCollection<AreaTreeItemModel>();

               EventPublish.AddEventTokener( 
                    Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);

            }

            #region IEventAggregator Subscription

            public void FundEventHandler(PublishEventArgs args)
            {
                if (args.EventType == PublishEventType.Core && args.EventId == 52309) //EventId？
                {
                    if (
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                            NodeId))
                    {
                        var t = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[NodeId];
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
                    if (args.EventType == PublishEventType.Core && args.EventId == 52309 && //终端信息修改
                        NodeType == TreeNodeType.IsTml && NodeId == (int)args.GetParams()[0])
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

            #region 数据绑定

            private ObservableCollection<AreaTreeItemModel> _areaTreeItem;

            /// <summary>
            /// 保存本节点下的子节点结构
            /// </summary>
            public ObservableCollection<AreaTreeItemModel> AreaTreeItem
            {
                get { return _areaTreeItem; }
            }

            /// <summary>
            /// 仅提供保存分组信息状态
            /// </summary>
            public int Status;

            private readonly AreaTreeItemModel _father;

            /// <summary>
            /// 本节点父节点
            /// </summary>
            public AreaTreeItemModel Father
            {
                get { return _father; }
            }

            /// <summary>
            /// 节点类型 是组节点 还是终端节点
            /// 或是其他节点
            /// </summary>
            public TreeNodeType NodeType { get; set; }

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

            private bool _isPartition;
            /// <summary>
            /// 终端地址或分组地址4为地址化
            /// </summary>
            public string FormatNodeId
            {
                get
                {
                    if (!_isPartition)
                    {
                        var res = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(NodeId);
                        if (res != null) return string.Format("{0:D4}", res.RtuPhyId);

                    }
                    return string.Format("{0:D2}", NodeId);
                }
            }



            /// <summary>
            /// 当修改树节点名称前需要备份前节点名称
            /// 若用户输入不合法则恢复原名称
            /// </summary>
            public string BackUpName;

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
                _areaTreeItem.Clear();
                if (NodeType != TreeNodeType.IsPartition) return;

                var grp = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(NodeId );
                
                if (grp == null  ) return;
                //if(grp.LstTml.Count==0) return;

                //加载终端节点
                var tmpLst = new List<AreaTmlModel>();


                foreach (var t in grp.LstTml)
                {
                    if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                        continue;
                    var f = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                    var ttt = new AreaTmlModel();
                    ttt.TmlId = f.RtuId;
                    ttt.PhysicalId = f.RtuPhyId;
                    if (f.RtuModel == EnumRtuModel.Wj3006 || f.RtuModel == EnumRtuModel.Wj3005)
                    {
                        ttt.TmlType = "aTml";
                    }
                    else if (f.RtuModel == EnumRtuModel.Wj1080)
                    {
                        ttt.TmlType = "zLux";
                    }
                    else
                    {
                        ttt.TmlType = f.RtuModel.ToString();
                    }

                    tmpLst.Add(ttt);
                }
                var tmpLstTml = (from t in tmpLst orderby t.TmlType ,t.PhysicalId ascending select t.TmlId).ToList();

                foreach (var t in tmpLstTml)//grp.LstTml
                {
 
                    
                    var f = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];

                    if(f.RtuFid == 0)
                    _areaTreeItem.Add(new AreaTreeItemModel(this, f, false));
                }
            }

            /// <summary>
            /// 为外部提供递归函数；
            /// 递归查询当前树节点选中的是哪一个节点
            /// </summary>
            /// <returns></returns>
            public AreaTreeItemModel GetSelectMenu()
            {
                if (_isSelected)
                {
                    return this;
                }

                if (NodeType == TreeNodeType.IsPartition)
                {
                    for (int i = 0; i < _areaTreeItem.Count; i++)
                    {
                        if (_areaTreeItem[i] != null)
                        {
                            var menuSelected = _areaTreeItem[i].GetSelectMenu();

                            if (menuSelected != null)
                            {
                                return menuSelected;
                            }
                        }
                    }
                }
                return null;
            }

            //public int GetMaxGrpId()
            //{
            //    if (NodeType != TreeNodeType.IsPartition || NodeId != 0) return 0;
            //    int max = ServicesAreaInfoHold.AreaStartId;
            //    foreach (var t in this.AreaTreeItem)
            //    {
            //        if (t.NodeType == TreeNodeType.IsPartition)
            //        {
            //            if (t.NodeId > max) max = t.NodeId;
            //            int gid = t.GetMaxGrpId(max);
            //            if (gid > max) max = gid;
            //        }
            //    }


            //    return max + 1;
            //}

            //public int GetMaxGrpId(int id)
            //{
            //    int max = id;
            //    if (NodeType != TreeNodeType.IsPartition) return 0;
            //    foreach (var t in this.AreaTreeItem)
            //    {
            //        if (t.NodeType == TreeNodeType.IsPartition)
            //        {
            //            if (t.NodeId > max) max = t.NodeId;
            //            int gid = t.GetMaxGrpId(max);
            //            if (gid > max) max = gid;
            //        }
            //    }
            //    if (max > id) return max;
            //    return 0;
            //}

            /// <summary>
            /// 递归删除标记为终端的节点且其地址为TmlId
            /// </summary>
            /// <param name="tmlId"></param>
            public void DeleteTreeNode(int tmlId)
            {
                for (int i = 0; i < this.AreaTreeItem.Count; i++)
                {
                    if (this.AreaTreeItem[i].NodeType == TreeNodeType.IsPartition)
                    {
                        this.AreaTreeItem[i].DeleteTreeNode(tmlId);
                    }
                    else if (this.AreaTreeItem[i].NodeType == TreeNodeType.IsTml)
                    {
                        if (this.AreaTreeItem[i].NodeId == tmlId)
                        {
                            this.AreaTreeItem.RemoveAt(i);
                            break;
                        }
                    }
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
                    if (NodeType == TreeNodeType.IsPartition)
                    {
                        //需要对所有标记本分组的终端宣布新分组信息
                        var args = new PublishEventArgs()
                        {
                            EventType = PublishEventTypeLocal.Name,
                            EventId =
                                PublishEventTypeLocal.GrpAreaReloadTmlGroup
                        };
                        EventPublish.PublishEvent(args);

                    }
                    else
                    {
                        //不修改
                        NodeName = BackUpName;
                    }
                    if (NodeName.Length > 15) NodeName = NodeName.Substring(0, 14);
                    if (NodeName.Length < 1) NodeName = BackUpName;
                    BackUpName = NodeName;
                }
            }

           
            #endregion



        
    }
}
