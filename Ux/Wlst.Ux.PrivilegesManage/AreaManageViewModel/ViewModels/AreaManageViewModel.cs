using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using DragDropExtend.DragAndDrop;


using Telerik.Windows.Controls;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.PrivilegesManage.AreaManageViewModel.Models;
using Wlst.Ux.PrivilegesManage.AreaManageViewModel.Services;
using Wlst.Ux.PrivilegesManage.Services;
using Wlst.client;

namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel.ViewModels
{
    [Export(typeof(IIAreaManage))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class GrpAreaShowManageViewModel : VmEventActionProperyChangedBase, IIAreaManage
    {
        private int AddIdMax = 1;
        public GrpAreaShowManageViewModel()
        {
            Title = "区域分组";
            InitEvent();
        }

        protected string AreaName0 = "";

        #region IEventAggregator Subscription

        public override void ExPublishedEvent(PublishEventArgs args)
        {

            if (args.EventId == PublishEventTypeLocal.GrpAreaReloadTmlGroup)
            {
                this.ReLoadTmlsAreasBelong();
            }

            if (args.EventId == Wlst.Ux.PrivilegesManage.Services.EventIdAssign.SingleInfoAreaAllNeedUpdate)
            {
                if (DateTime.Now.Ticks - datetime.Ticks < 600000000)
                {
                    Msg = DateTime.Now + " 更新成功!";
                }
                else
                {
                    Msg = DateTime.Now + " 收到数据并执行页面刷新!";
                }
                LoadItems();
            }


        }



        public void InitEvent()
        {

            this.AddEventFilterInfo(PublishEventTypeLocal.GrpAreaReloadTmlGroup, PublishEventTypeLocal.Name);
            this.AddEventFilterInfo(
                Wlst.Ux.PrivilegesManage.Services.EventIdAssign.SingleInfoAreaAllNeedUpdate);
        }


        private DateTime datetime;

        #endregion

        public override void NavOnLoadr(params object[] parsObjects)
        {
            Msg = "";
            LoadItems();
            LabNameVisi = Visibility.Visible;
            TxbNameVisi = Visibility.Collapsed;
            
        }

        public override void OnUserHideOrClosingr()
        {
            this.AreaTreeItem.Clear();
            this.TmlData.Clear();
        }

        /// <summary>
        /// 加载根节点、子节点数据到树中
        /// </summary>
        public void LoadItems()
        {
            datetime = DateTime.Now.AddDays(-1);
            AreaTreeItem.Clear();
            //添加根节点到树中
            foreach (var f in Wlst .Sr .EquipmentInfoHolding .Services .AreaInfoHold .MySlef .AreaInfo )
            {
                this.AreaTreeItem.Add(new AreaTreeItemModel(null, f.Value, true));
            }           
            foreach (var t in AreaTreeItem)
            {
                if (t.NodeType != TreeNodeType.IsPartition) continue;
                t.AddChild();
            }
            LoadTmlsData();
            ReLoadTmlsAreasBelong();
            foreach (var f in AreaTreeItem) if (f.NodeId >= AddIdMax) AddIdMax = f.NodeId + 1;
        }


        /// <summary>
        /// 获取整棵树的区域分组信息
        /// </summary>
        /// <returns></returns>
        private List<AreaInformation> GetAreasBelong()
        {
            var list = new List<AreaInformation>();

            int index = 1;
            foreach (var t in this.AreaTreeItem)
            {
                if (t.NodeType != TreeNodeType.IsPartition) continue;
                var tmp =
                    new AreaInformation(new AreaInfo.AreaItem()
                    {
                        AreaId = t.NodeId,
                        AreaName=t.NodeName,
                        LstTml = new List<int>()
                    }
                        )
                    {
                        Index = index++
                    };
                foreach (var g in t.AreaTreeItem)
                {
                    if (g.NodeType == TreeNodeType.IsPartition) continue;
                    tmp.LstTml.Add(g.NodeId);
                }

                list.Add(tmp);
            }
            return list;
        }
       

        #region TreeView

        public string HeaderInfo
        {
            get { return "Sgp"; }
        }



        public void AddArea(AreaInfo.AreaItem gi)
        {
            var t = new AreaTreeItemModel(null, gi, true);
            AreaTreeItem.Add(t);
        }

        private ObservableCollection<AreaTreeItemModel> _areaTreeItem;

        /// <summary>
        /// 左侧区域分组列表
        /// </summary>
        public ObservableCollection<AreaTreeItemModel> AreaTreeItem
        {
            get
            {
                if (_areaTreeItem == null)
                    _areaTreeItem = new ObservableCollection<AreaTreeItemModel>();
                return _areaTreeItem;
            }
        }

        private AreaTreeItemModel _selectedTreeItem;

        /// <summary>
        /// 左侧区域分组列表当前选中
        /// </summary>
        public AreaTreeItemModel SelectedTreeItem
        {
            get
            {
                if (_selectedTreeItem == null) _selectedTreeItem = new AreaTreeItemModel();
                return _selectedTreeItem;
            }
            set
            {
                if (value != _selectedTreeItem)
                {
                    _selectedTreeItem = value;
                    this.RaisePropertyChanged(() => this.SelectedTreeItem);
                }
            }
        }

        /// <summary>
        /// 获取菜单树当前选择的菜单
        /// </summary>
        public AreaTreeItemModel GetSelectMenu()
        {
            for (int i = 0; i < AreaTreeItem.Count; i++)
            {
                var menuSelected = AreaTreeItem[i].GetSelectMenu();
                if (menuSelected != null)
                {
                    return menuSelected;
                }
            }
            return null;
        }

        private List<Tuple<int, int>> GetIndexBelong(AreaTreeItemModel father, ref  int index)
        {
            var lst = new List<Tuple<int, int>>();

            lst.Add(new Tuple<int, int>(father.NodeId, index));
            index++;
            foreach (var t in father.AreaTreeItem)
            {
                var ff = GetIndexBelong(t, ref index);
                lst.AddRange(ff);
            }
            return lst;
        }

        private List<int> GetIndexBelong()
        {
            int index = 1;
            var lst = new List<Tuple<int, int>>();
            foreach (var t in this.AreaTreeItem)
            {
                var ff = GetIndexBelong(t, ref index);
                lst.AddRange(ff);
            }
            return (from t in lst orderby t.Item2 ascending select t.Item1).ToList();
        }

        #endregion

        #region 数据绑定

        private string _msg;

        /// <summary>
        /// 界面备注
        /// </summary>
        public string Msg
        {
            get { return _msg; }
            set
            {
                if (value != _msg)
                {
                    _msg = value;
                    this.RaisePropertyChanged(() => this.Msg);
                }
            }
        }

        /// <summary>
        /// 当修改树节点名称前需要备份前节点名称
        /// 若用户输入不合法则恢复原名称
        /// </summary>
      //  private string BackUpName;

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

        private string _searchText;
        [StringLength(10, ErrorMessage = "查询名称字符长度小于10")]
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (value != _searchText)
                {
                    _searchText = value;
                    this.RaisePropertyChanged(() => this.SearchText);
                }
            } 
        }
        #endregion 
  
        #region GridView

        private ObservableCollection<AreaTmlModel> _tmldata;

        /// <summary>
        /// 右侧终端列表
        /// </summary>
        public ObservableCollection<AreaTmlModel> TmlData
        {
            get
            {
                if (_tmldata == null)
                    _tmldata = new ObservableCollection<AreaTmlModel>();
                return _tmldata;
            }
        }

        private AreaTmlModel _selectedTmlData;

        /// <summary>
        /// 右侧终端列表被选中
        /// </summary>
        public AreaTmlModel SelectedTmlData
        {
            get
            {
                if (_selectedTmlData == null) _selectedTmlData = new AreaTmlModel();
                return _selectedTmlData;
            }
            set
            {
                if (value != _selectedTmlData)
                {
                    _selectedTmlData = value;
                    this.RaisePropertyChanged(() => this.SelectedTmlData);
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _selectArea;
        /// <summary>
        /// 右侧区域选择下拉框
        /// </summary>
        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> SelectArea
        {
            get
            {
                if (_selectArea == null)
                {
                    _selectArea = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                }
                return _selectArea;
            }
            set
            {
                if (value == _selectArea) return;
                _selectArea = value;
                this.RaisePropertyChanged(() => SelectArea);
            }
        }

        private Wlst.Cr.CoreOne.Models.NameValueInt _currentSelectArea;

        /// <summary>
        /// 右侧区域选择下拉框被选中
        /// </summary>
        public Wlst.Cr.CoreOne.Models.NameValueInt CurrentSelectArea
        {
            get
            {
                if (_currentSelectArea == null) _currentSelectArea = new Wlst.Cr.CoreOne.Models.NameValueInt();
                return _currentSelectArea;
            }
            set
            {
                if (value != _currentSelectArea)
                {
                    _currentSelectArea = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectArea);
                }
            }
        }

        

        /// <summary>
        /// 保存终端地址到终端信息的关系映射
        /// </summary>
        private Dictionary<int, int> dictionaryC;

        /// <summary>
        /// 全部重新计算所有终端所属归属组名称
        /// </summary>
        private void  ReLoadTmlsAreaBelong()
        {
            foreach (var t in TmlData)
            {
                t.AreaName = AreaTreeItem[0].NodeName;
                t.AreaId = -1;
            }
            foreach (var t in this.AreaTreeItem)
            {
                if (t.NodeType == TreeNodeType.IsPartition)
                {
                    ReLoadTmlsAreaBelong(t);
                }
                else if (t.NodeType == TreeNodeType.IsTml)
                {
                    if (dictionaryC.ContainsKey(t.NodeId))
                    {
                        TmlData[dictionaryC[t.NodeId]].AreaName = t.NodeName;
                        TmlData[dictionaryC[t.NodeId]].AreaId = t.NodeId;
                    }
                }
            }
        }

        /// <summary>
        /// 提供递归查询终端树中的终端节点直属父节点组名称
        /// </summary>
        /// <param name="father"></param>
        private void ReLoadTmlsAreaBelong(AreaTreeItemModel father)
        {
            if (father.NodeType != TreeNodeType.IsPartition) return;
            foreach (var t in father.AreaTreeItem)
            {
                if (t.NodeType == TreeNodeType.IsPartition)
                {
                    ReLoadTmlsAreaBelong(t);
                }
                else if (t.NodeType == TreeNodeType.IsTml)
                {
                    if (dictionaryC.ContainsKey(t.NodeId))
                    {
                        TmlData[dictionaryC[t.NodeId]].AreaName = father.NodeName;
                        TmlData[dictionaryC[t.NodeId]].AreaId = father.NodeId;
                    }
                }
            }
        }

        /// <summary>
        /// 从终端数据区域加载终端信息到ItemTmls中
        /// </summary>
        private void LoadTmlsData()
        {
            dictionaryC = new Dictionary<int, int>();
            TmlData.Clear();
            string type = " ";
            int i = 0;
            var tmpLst = new List<AreaTmlModel>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.RtuFid != 0) continue;
                int id = t.Value.RtuId;
                string name = t.Value.RtuName;
                if (t.Value.RtuModel == EnumRtuModel.Wj3005 ) type = "3005终端";
                else if (t.Value.RtuModel == EnumRtuModel.Wj3090) type = "3090终端";  
                else if (t.Value.RtuModel == EnumRtuModel.Wj3006) type = "3006终端";
                else if (t.Value.RtuModel == EnumRtuModel.Wj2090) type = "单灯设备";
                else if (t.Value.RtuModel == EnumRtuModel.Wj1080) type = "光控设备";
                else if (t.Value.RtuModel == EnumRtuModel.Wj1050) type = "电表设备";
                else if (t.Value.RtuModel == EnumRtuModel.Jd601) type = "节电设备";
                else if (t.Value.RtuModel == EnumRtuModel.Wj1090) type = "线路检测";
                else if (t.Value.RtuModel == EnumRtuModel.Wj4005) type = "4005终端";
                var ttt = new AreaTmlModel();
                ttt.PhysicalId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id).RtuPhyId;
                ttt.TmlId = id;
                ttt.AreaName = AreaTreeItem[0].NodeName;
                ttt.IsChecked = false;
                ttt.TmlName = name;
                ttt.TmlType = type;
                tmpLst.Add(ttt);
                //TmlData.Add(ttt);
                //dictionaryC.Add(ttt.TmlId, i);
                //i++;

            }
            var tmpLst2 = (from t in tmpLst orderby t.TmlType, t.PhysicalId  ascending select t).ToList();
            foreach (var t in tmpLst2)
            {
                TmlData.Add(t);
                dictionaryC.Add(t.TmlId, i);
                i++;
            }
        }

        /// <summary>
        /// 全部重新计算所有终端所属归属区域名称
        /// </summary>
        private void ReLoadTmlsAreasBelong()
        {
            foreach (var t in TmlData)
            {
                t.AreaName = AreaTreeItem[0].NodeName;
                t.AreaId = -1;
            }
            foreach (var t in this.AreaTreeItem)
            {
                if (t.NodeType == TreeNodeType.IsPartition)
                {
                    ReLoadTmlsAreasBelong(t);
                }
                else if (t.NodeType == TreeNodeType.IsTml)
                {
                    if (dictionaryC.ContainsKey(t.NodeId))
                    {
                        TmlData[dictionaryC[t.NodeId]].AreaName = t.NodeName;
                        TmlData[dictionaryC[t.NodeId]].AreaId = t.NodeId;
                    }
                }
            }
        }

        /// <summary>
        /// 提供递归查询终端树中的终端节点直属父节点组名称
        /// </summary>
        /// <param name="father"></param>
        private void ReLoadTmlsAreasBelong(AreaTreeItemModel father)
        {
            if (father.NodeType != TreeNodeType.IsPartition) return;
            foreach (var t in father.AreaTreeItem)
            {
                if (t.NodeType == TreeNodeType.IsPartition)
                {
                    ReLoadTmlsAreasBelong(t);
                }
                else if (t.NodeType == TreeNodeType.IsTml)
                {
                    if (dictionaryC.ContainsKey(t.NodeId))
                    {
                        TmlData[dictionaryC[t.NodeId]].AreaName = father.NodeName;
                        TmlData[dictionaryC[t.NodeId]].AreaId = father.NodeId;
                    }
                }
            }
        }

        

        #endregion

        #region Drap & Drop

        #region DragSourceTree 成员  从树中拖出数据 树为源

        private DragSourceClassIDragSource DragSourceClassTree;

        public IDragSource DragSourceTree
        {
            get
            {
                if (DragSourceClassTree == null)
                {
                    DragSourceClassTree = new DragSourceClassIDragSource(DragSourceTreeStartDrag,
                                                                          DragSourceTreeDragData);
                }
                return DragSourceClassTree;
            }
        }

        public DragDropEffects DragSourceTreeStartDrag(object Data)
        {
            try
            {
                var dataAsPath = Data as HelpDragAndDrop;
                if (dataAsPath == null) return DragDropEffects.None;
                if (dataAsPath.areaTreeData == null) return DragDropEffects.None;
                return DragDropEffects.Copy | DragDropEffects.Move;

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return DragDropEffects.None;
        }

        public object DragSourceTreeDragData()
        {
            var strGetSeletcTreeNodePath = GetSelectMenu();
            if (strGetSeletcTreeNodePath == null) return null;

            return new HelpDragAndDrop(strGetSeletcTreeNodePath);
        }

        #endregion


        #region DropTargetTree 树中加入数据 树为目的地

        private DropTargetClassIDropTarget DropTargetClassTree;

        public IDropTarget DropTargetTree
        {
            get
            {
                if (DropTargetClassTree == null)
                {
                    DropTargetClassTree = new DropTargetClassIDropTarget(DropTargetTreeDragOver, DropTargetTreeDrop);
                }
                return DropTargetClassTree;
            }
        }

        public DragDropEffects DropTargetTreeDragOver(object Data)
        {
            var t = Data as HelpDragAndDrop;
            if (t == null) return DragDropEffects.None;
            if (t.listData.Count > 0) return DragDropEffects.Copy | DragDropEffects.Move;
            else if (t.areaTreeData != null)
            {
                return DragDropEffects.Copy | DragDropEffects.Move;
            }
            return DragDropEffects.None;

        }

        public void DropTargetTreeDrop(object sender, DragEventArgs e, object Data)
        {
            var t = Data as HelpDragAndDrop;
            if (t == null) return;
            if (t.listData.Count > 0)
            {

                //区域增加终端
                //todo
                var dropInfomation = new DropTargetInfomation(sender, e);
                var mvvm = dropInfomation.TargetItem as AreaTreeItemModel;
                if (mvvm == null) return;
                int fatherMvvmId = 0;
                var fatherMvvm = mvvm;
                if (mvvm.NodeType == TreeNodeType.IsPartition)
                {
                   // if (mvvm.NodeId == 0) return;
                    fatherMvvmId = mvvm.NodeId;
                }
                else if (mvvm.NodeType == TreeNodeType.IsTml)
                {
                    if (mvvm.Father == null) return;
                    fatherMvvmId = mvvm.Father.NodeId;
                    fatherMvvm = mvvm.Father;
                }


                foreach (var tmlNeedAdd in t.listData)
                {

                    foreach (var tft in this.AreaTreeItem)
                    {
                        tft.DeleteTreeNode(tmlNeedAdd);
                    }
                }

                foreach (var tmlNeedAdd in t.listData)
                {
                    if (
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                            tmlNeedAdd))
                    {
                        
                        fatherMvvm.AreaTreeItem.Add(new AreaTreeItemModel(fatherMvvm,
                                                                             Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tmlNeedAdd],
                                                                              false));
                    }

                }
                
                ReLoadTmlsAreasBelong();
                //GetAreasInfos(fatherMvvm);
                this.ExClearAllSelected();
            }

            else
            {
                if (t.areaTreeData == null) return;

                try
                {
                    var dropInfomation = new DropTargetInfomation(sender, e);
                    var mvvm = dropInfomation.TargetItem as AreaTreeItemModel;
                    if (mvvm == null) return;
                    if (mvvm.NodeType != t.areaTreeData.NodeType) return;
                    if (mvvm.Father != t.areaTreeData.Father) return;
                    if (mvvm.Father == null) return;
                    if (mvvm.NodeId == t.areaTreeData.NodeId) return;

                    if (!mvvm.Father.AreaTreeItem.Contains(t.areaTreeData)) return;
                    mvvm.Father.AreaTreeItem.Remove(t.areaTreeData);
                    int index = mvvm.Father.AreaTreeItem.IndexOf(mvvm);
                    mvvm.Father.AreaTreeItem.Insert(index, t.areaTreeData);
                }
                catch (Exception ex)
                {

                }

            }
        }

        public class HelpDragAndDrop
        {
            public HelpDragAndDrop(AreaTreeItemModel tv)
            {
                areaTreeData = tv;
                listData = new List<int>();
            }

            public HelpDragAndDrop(List<int> lst)
            {
                areaTreeData = null;
                listData = new List<int>();
                foreach (var t in lst)
                {
                    listData.Add(t);
                }
            }

            public AreaTreeItemModel areaTreeData;

            /// <summary>
            /// 拖转携带的数据
            /// 树到终端大致为  id-grp-name/id
            /// 终端到树大致为  id/id/id/id
            /// </summary>
            public List<int> listData;
        }

        #endregion

        #region DragSourceListView 成员  从LV中拖出数据 LV为源

        private DragSourceClassIDragSource DragSourceClassListView;

        public IDragSource DragSourceListView
        {
            get
            {
                if (DragSourceClassListView == null)
                {
                    DragSourceClassListView = new DragSourceClassIDragSource(DragSourceListViewStartDrag,
                                                                             DragSourceListViewDragData);
                }
                return DragSourceClassListView;
            }
        }

        public DragDropEffects DragSourceListViewStartDrag(object Data)
        {
            try
            {
                var dataAsPath = Data as HelpDragAndDrop;
                if (dataAsPath == null) return DragDropEffects.None;
                if (dataAsPath.listData == null) return DragDropEffects.None;
                if (dataAsPath.listData.Count < 1) return DragDropEffects.None;
                return DragDropEffects.Copy | DragDropEffects.Move;

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return DragDropEffects.None;
        }

        public object DragSourceListViewDragData()
        {
            List<int> lst = new List<int>();
            foreach (var t in TmlData)
            {
                if (t.IsChecked)
                {
                    lst.Add(t.TmlId);
                }
            }
            if (lst.Count > 0) return new HelpDragAndDrop(lst);
            return null;
        }

        #endregion


        #region DropTargetListView 从树中拖出的数据丢弃到LV中 树删除终端数据  源树目的LV

        private DropTargetClassIDropTarget DropTargetClassListView;

        public IDropTarget DropTargetListView
        {
            get
            {
                if (DropTargetClassListView == null)
                {
                    DropTargetClassListView = new DropTargetClassIDropTarget(DropTargetListViewDragOver,
                                                                             DropTargetListViewDrop);
                }
                return DropTargetClassListView;
            }
        }

        public DragDropEffects DropTargetListViewDragOver(object Data)
        {
            var t = Data as HelpDragAndDrop;
            if (t == null) return DragDropEffects.None;
            return DragDropEffects.Copy | DragDropEffects.Move;
        }

        /// <summary>
        /// 仅终端树 拖动到lv中使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="Data"></param>
        public void DropTargetListViewDrop(object sender, DragEventArgs e, object Data)
        {
            var t = Data as HelpDragAndDrop;
            if (t == null) return;
            if (t.areaTreeData == null) return;

            try
            {
                if (t.areaTreeData.Father != null && t.areaTreeData.NodeType == TreeNodeType.IsTml)
                {
                    int id = t.areaTreeData.NodeId;
                    t.areaTreeData.Father.AreaTreeItem.Remove(t.areaTreeData);
                    foreach (var tm in TmlData)
                    {
                        if (tm.TmlId == id)
                        {
                            tm.AreaName = AreaTreeItem[0].NodeName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("iGrpsHelpShowManageViewModel DropTargetListViewDrop  err path is :" +
                                       ",Err is " + ex.ToString());
                return;
            }

        }

        #endregion

        #endregion

        #region Command

        private ICommand _cmdFastSearch;

        /// <summary>
        /// 选中所有终端
        /// </summary>
        public ICommand CmdFastSearch
        {
            get
            {
                return _cmdFastSearch ??
                       (_cmdFastSearch = new RelayCommand(ExFastSearch, CanExFastSearch, true));
            }
        }

        private bool CanExFastSearch()
        {
            return true;
        }

        private void ExFastSearch()
        {
            if(string.IsNullOrEmpty(SearchText))
            {
                LoadTmlsData();
                ReLoadTmlsAreasBelong();
                
            }
            else
            {
                TmlData.Clear();
                dictionaryC = new Dictionary<int, int>();
                int i = 0;
                string  type = "";
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    
                    if(t.Value.RtuName.Contains(this.SearchText))
                    {
                        if (t.Value.RtuModel == EnumRtuModel.Wj3005) type = "3005终端";
                        else if (t.Value.RtuModel == EnumRtuModel.Wj3090) type = "3090终端";
                        else if (t.Value.RtuModel == EnumRtuModel.Wj3006) type = "3006终端";
                        else if (t.Value.RtuModel == EnumRtuModel.Wj2090) type = "单灯设备";
                        else if (t.Value.RtuModel == EnumRtuModel.Wj4005) type = "4005终端";
                        else if (t.Value.RtuModel == EnumRtuModel.Wj1080) 
                        {
                            type = "光控设备";
                            if (t.Value.RtuFid != 0) continue;
                        }
                        else if (t.Value.RtuModel == EnumRtuModel.Wj1050)
                        {
                            type = "电表设备"; 
                            if (t.Value.RtuFid != 0) continue;
                        }
                        else if (t.Value.RtuModel == EnumRtuModel.Jd601)
                        {
                            if (t.Value.RtuFid != 0) continue;
                            type = "节电设备";
                        }
                        else if (t.Value.RtuModel == EnumRtuModel.Wj1090)
                        {
                            if (t.Value.RtuFid != 0) continue;
                            type = "线路检测";
                        }
                        var SearchResult = new AreaTmlModel();
                        var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(t.Value.RtuId);
                        SearchResult.AreaId = areaId ;
                        SearchResult.PhysicalId = t.Value.RtuPhyId;
                        SearchResult.TmlId = t.Value.RtuId;
                        SearchResult.TmlName = t.Value.RtuName;
                        SearchResult.TmlType = type;


                        this.TmlData.Add(SearchResult);                       
                        dictionaryC.Add(SearchResult.TmlId, i);
                        i++;
                    }
                }
                ReLoadTmlsAreasBelong();

            }
        }

        private DateTime _dtSelectAll;
        private ICommand _cmdSelectAll;

        /// <summary>
        /// 选中所有终端
        /// </summary>
        public ICommand CmdSelectAll
        {
            get
            {
                return _cmdSelectAll ??
                       (_cmdSelectAll = new RelayCommand(ExSelectAllSelected, CanExSelectAllSelected, true));
            }
        }

        private bool CanExSelectAllSelected()
        {
            return DateTime.Now.Ticks - _dtSelectAll.Ticks > 30000000;
        }

        private void ExSelectAllSelected()
        {
            _dtSelectAll = DateTime.Now;
            foreach (var t in TmlData)
            {
                if (t.TmlType == SelectedTmlData.TmlType)
                {
                     t.IsChecked = true;
                }
               
            }
        }

        private DateTime _dtClearSelectAll;
        private ICommand _cmdClearSelectAll;

        /// <summary>
        /// 取消全选所有终端
        /// </summary>
        public ICommand CmdClearSelectAll
        {
            get
            {
                return _cmdClearSelectAll ??
                       (_cmdClearSelectAll = new RelayCommand(ExClearAllSelected, CanExClearAllSelected, true));
            }
        }

        private bool CanExClearAllSelected()
        {
            return DateTime.Now.Ticks - _dtClearSelectAll.Ticks > 30000000;
        }

        private void ExClearAllSelected()
        {
            _dtClearSelectAll = DateTime.Now;
            foreach (var t in TmlData)
            {
                t.IsChecked = false;
            }
        }



        private ICommand _cmdEditTree;

        /// <summary>
        /// 左侧树 编辑根节点
        /// </summary>
        public ICommand CmdEditTree
        {
            get { return _cmdEditTree ?? (_cmdEditTree = new RelayCommand(ExEditTree, CanExEditTree, true)); }
        }

        private bool CanExEditTree()
        {
            return true;
        }

        private void ExEditTree()
        {                     
            foreach (var t in this.AreaTreeItem)
            {
                if(t.IsSelected==true)
                {
                   t. BackUpName = t.NodeName;
                    t.LabNameVisi = Visibility.Collapsed;
                    t.TxbNameVisi = Visibility.Visible;                    
                }
                else
                {
                    t.BackUpName = t.NodeName;
                    t.LabNameVisi = Visibility.Visible;
                    t.TxbNameVisi = Visibility.Collapsed;
                   
                }
            }
            ReLoadTmlsAreasBelong();
            
        }

        private ICommand _cmdAddTree;

        /// <summary>
        /// 左侧树 添加根节点
        /// </summary>
        public ICommand CmdAddTree
        {
            get { return _cmdAddTree ?? (_cmdAddTree = new RelayCommand(ExAddTree, CanExAddTree, true)); }
        }

        private bool CanExAddTree()
        {
            return true;
        }
       

        private void ExAddTree()
        {             
            //int maxIndx = 1;
            //foreach (var f in this.AreaTreeItem) if (f.NodeId >= maxIndx) maxIndx = f.NodeId + 1;
            var tmp = new AreaInformation(new AreaInfo.AreaItem()
            {
                AreaId = AddIdMax ,
                AreaName = "新区域"+ AddIdMax ,
                LstTml = new List<int>()
            });
            AddIdMax += 1;
            this.AreaTreeItem.Add(new AreaTreeItemModel(null, tmp, true));
        }

        private ICommand _cmdDeleteTree;

        /// <summary>
        /// 左侧树 删除根节点
        /// </summary>
        public ICommand CmdDeleteTree
        {
            get { return _cmdDeleteTree ?? (_cmdDeleteTree = new RelayCommand(ExDeleteTree, CanExDeleteTree, true)); }
        }

        private bool CanExDeleteTree()
        {
            return true;
        }

        private void ExDeleteTree()
        {
            for (int i = this.AreaTreeItem.Count - 1; i >= 0; i--)
            {
                if (AreaTreeItem[i].IsSelected == true)
                {
                    if (AreaTreeItem[i].NodeId == 0)
                    {
                        UMessageBox.Show("禁止删除", "此区域为所有终端初始状态时的默认区域，不能删除!", UMessageBoxButton.Yes);
                    }
                    else if (AreaTreeItem[i].AreaTreeItem.Count != 0)
                    {
                        UMessageBox.Show("禁止删除", "此区域下仍有未移出的设备", UMessageBoxButton.Yes);
                    }
                    else
                    {
                        this.AreaTreeItem.RemoveAt(i);
                    }
                }
            }
            ReLoadTmlsAreasBelong();
        }


        private DateTime _dtSave;

        /// <summary>
        /// 保存区域设置
        /// </summary>
        private ICommand _cmdSave;
        private DateTime _dtCmdSave;
        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new RelayCommand(ExSave, CanSave, true)); }
        }
        private void ExSave()
        {
            _dtCmdSave = DateTime.Now;
            var rtn = GetAreasBelong();
            var tmp = (from t in rtn orderby t.Index ascending select t).ToList();
            var snd = new List<AreaInfo.AreaItem>();
            foreach (var f in tmp)
                snd.Add(new AreaInfo.AreaItem() {AreaId = f.AreaId, AreaName = f.AreaName, LstTml = f.LstTml});
            Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.UpdateAreaInfo(snd);
            
                
            Msg = DateTime.Now + " 已提交信息到服务器！";
            datetime = DateTime.Now;
          
        }

        private bool CanSave()
        {
            if (DateTime.Now.Ticks - _dtCmdSave.Ticks < 60000000) return false;
            return true;

        }

        #endregion

        

    }
}

