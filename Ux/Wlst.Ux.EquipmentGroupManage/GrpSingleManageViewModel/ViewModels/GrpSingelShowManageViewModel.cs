using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using DragDropExtend.DragAndDrop;
using DragDropExtend.ExtensionsHelper;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.EquipmentGroupManage.GrpSingleManageViewModel.Services;
using Wlst.Ux.EquipmentGroupManage.Models;
using Wlst.Ux.EquipmentGroupManage.Services;
using Wlst.client;

namespace Wlst.Ux.EquipmentGroupManage.GrpSingleManageViewModel.ViewModels
{
    [Export(typeof (IIGrpSingelShowManage))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class GrpSingelShowManageViewModel : VmEventActionProperyChangedBase, IIGrpSingelShowManage
    {
        public GrpSingelShowManageViewModel()
        {
            Visi = Visibility.Collapsed;
            Title = "全局分组";
            InitEvent();
        }


        #region IEventAggregator Subscription

        public override void  ExPublishedEvent(PublishEventArgs args)
{
 	 
            if (args.EventId == PublishEventTypeLocal.GrpSingleReloadTmlGroup)
            {
                this.ReLoadTmlsGrpsBelong();
            }

            if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleNeedRefresh )
            {
                this.LoadItems();
            }

            if (args.EventId == Wlst.Sr.EquipmentInfoHolding .Services.EventIdAssign.SingleInfoGroupAllNeedUpdate)
            {
                if (DateTime.Now.Ticks - dtSnd.Ticks < 600000000)
                {

                    Msg = DateTime.Now + " 更新成功!";
                }
                else
                {
                    Msg = DateTime.Now + " 收到更新数据!";
                }
                LoadItems();
            }
        }



        public void InitEvent() 
        {
           
            this.AddEventFilterInfo(PublishEventTypeLocal.GrpSingleReloadTmlGroup, PublishEventTypeLocal.Name);
            this.AddEventFilterInfo(
                Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.SingleInfoGroupAllNeedUpdate, "Core.CoreServices.PublishEventArgs.Core", true);
        }


        private DateTime dtSnd;
        #endregion

        /// <summary>
        /// add maxid 
        /// </summary>
        private int AddMaxId = 1;
        public override void NavOnLoadr(params object[] parsObjects)
        {

            Msg = "";
            this.ItemsArea.Clear();
            if (UserInfo.UserLoginInfo.D == true)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                    this.ItemsArea.Add(new NameValueInt()
                    {
                        Name = f.Key.ToString("d2") + "-" + f.Value.AreaName,
                        Value = f.Key
                    });
            }
            else
            {
                List<int> areaLst = new List<int>();
                areaLst.AddRange(UserInfo.UserLoginInfo.AreaX);
                foreach (var t in UserInfo.UserLoginInfo.AreaW)
                {
                    if (!areaLst.Contains(t))
                    {
                        areaLst.Add(t);
                    }
                }
                foreach (var f in UserInfo.UserLoginInfo.AreaR)
                {
                    if (!areaLst.Contains(f))
                    {
                        areaLst.Add(f);
                    }
                }
                foreach (var f in areaLst)
                {
                    var areaInfo = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(f);
                    if (areaInfo == null) continue;
                    this.ItemsArea.Add(new NameValueInt()
                    {
                        Name = areaInfo.AreaId.ToString("d2") + "-" + areaInfo.AreaName,
                        Value = areaInfo.AreaId
                    });
                }
            }

            if (ItemsArea.Count > 0) CurrentSelectArea = ItemsArea[0];
            Visi = ItemsArea.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
            LoadItems();

        }


        public override void OnUserHideOrClosingr()
        {
            this.ChildTreeItems.Clear();
            this.ItemTmls.Clear();
            //base.OnUserHideOrClosingr();
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _fItemsArea;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> ItemsArea
        {
            get
            {
                if (_fItemsArea == null)
                {
                    _fItemsArea = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                }
                return _fItemsArea;
            }
            set
            {
                if (value == _fItemsArea) return;
                _fItemsArea = value;
                this.RaisePropertyChanged(() => ItemsArea);
            }
        }

        private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }


        private bool _isDelete;

        /// <summary>
        /// 删除分组按钮是否可用
        /// </summary>
        public bool IsDelete
        {
            get { return _isDelete; }
            set
            {
                if (value != _isDelete)
                {
                    _isDelete = value;
                    this.RaisePropertyChanged(() => this.IsDelete);
                }
            }
        }



        private Wlst.Cr.CoreOne.Models.NameValueInt _cur;
        public Wlst.Cr.CoreOne.Models.NameValueInt CurrentSelectArea
        {
            get { return _cur; }
            set
            {
                if (value == _cur) return;
                _cur = value;
                this.RaisePropertyChanged(() => this.CurrentSelectArea);
                LoadItems();
                
            }
        }
        public void LoadItems()
        {
            dtSnd = DateTime.Now.AddDays(-1);
            //_getGrpsInfoForUse = new GetGrpsSingelInfoForUse();

            int maxidx = 1;
            ChildTreeItems.Clear();
            if (CurrentSelectArea == null) return;
            var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(CurrentSelectArea.Value);
            if (area == null) return;           
            var grps =
                Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(CurrentSelectArea.Value);
            
            if (grps.Count > 0)
            {
                var grpsTmp = (from t in grps orderby t.GroupId select t).ToList() ;
                foreach (var f in grpsTmp)
                {
                    
                    this.AddChild(f);

                }
                //对分组子节点 进行数据加载
                foreach (var t in ChildTreeItems)
                {
                    if (t.NodeType != TreeNodeType.IsGrp) continue;
                    t.AddChild();
                }
            }
            
            LoadTmlsInfo();
            ReLoadTmlsGrpsBelong();

            foreach (var f in this .ChildTreeItems )
            {
                if (f.NodeId >= maxidx) maxidx = f.NodeId + 1;
                f.GetChildRtuCount();
            }
            AddMaxId = maxidx;
            IsDelete = ChildTreeItems.Count > 1;
        }

        
        #region tree

        public string HeaderInfo
        {
            get { return "Sgp"; }
        }


        #region TreeView & 事件

        public void AddChild(GroupItemsInfo.GroupItem gi)
        {
            if (CurrentSelectArea == null) return;
            var t = new TreeItemViewModel(null, CurrentSelectArea .Value ,gi, true);
            ChildTreeItems.Add(t);
        }

        private ObservableCollection<TreeItemViewModel> _childTreeItemsInfo;

        /// <summary>
        /// 终端树  根节点
        /// </summary>
        public ObservableCollection<TreeItemViewModel> ChildTreeItems
        {
            get
            {
                if (_childTreeItemsInfo == null)
                    _childTreeItemsInfo = new ObservableCollection<TreeItemViewModel>();
                return _childTreeItemsInfo;
            }
        }

        private TreeItemViewModel _selectedgrp;
        /// <summary>
        /// 选中终端树 根节点
        /// </summary>
        public TreeItemViewModel SelectedGrp
        {
            get
            {
                if (_selectedgrp == null) _selectedgrp = new TreeItemViewModel();
                return _selectedgrp;
            }
            set
            {
                if (value != _selectedgrp)
                {
                    _selectedgrp = value;
                    this.RaisePropertyChanged(() => this.SelectedGrp);
                }
            }
        }


        public void TreeView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 如果使用附加属性来开启右键选中功能，
            // 那么在这里面的代码发生在TreeViewHelper中的代码之后，逻辑正确
        }

        public void TreeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 注意，这里的sender是TreeView
            // 我们需要从e.OriginalSource拿到TreeViewItem
            var item = VisualTreeExtensions.GetTemplatedAncestor<TreeViewItem>(e.OriginalSource as FrameworkElement);
            if (item != null)
            {
                item.IsSelected = true;
            }
        }

        /// <summary>
        /// 获取菜单树当前选择的菜单
        /// </summary>
        /// <returns></returns>
        public TreeItemViewModel GetSelectMvvm()
        {
            for (int i = 0; i < ChildTreeItems.Count; i++)
            {
                var strReturn = ChildTreeItems[i].GetSelectMvvm();
                if (strReturn != null)
                {
                    return strReturn;
                }
            }
            return null;
        }

        #endregion

        #endregion

        #region  终端信息 LoadTmlsInfo & ReLoadTmlsGrpsBelong

        private ObservableCollection<ItemModel> _itemTmlsInfo;

        public ObservableCollection<ItemModel> ItemTmls
        {
            get
            {
                if (_itemTmlsInfo == null)
                    _itemTmlsInfo = new ObservableCollection<ItemModel>();
                return _itemTmlsInfo;
            }
            //set { _itemTmlsInfo = value; }
        }

        private ItemModel _currentListViewItem;
        
        public ItemModel CurrentListViewItem
        {
            get
            {
                if (_currentListViewItem == null) _currentListViewItem = new ItemModel();
                return _currentListViewItem;
            }
            set
            {
                if (value != _currentListViewItem)
                {
                    _currentListViewItem = value;
                    this.RaisePropertyChanged(() => this.CurrentListViewItem);
                }
            } 
        }


        /// <summary>
        /// 从终端数据区域加载终端信息到ItemTmls中
        /// </summary>
        private void LoadTmlsInfo()
        {
            dictionaryC = new Dictionary<int, int>();
            ItemTmls.Clear();
            //GetAreaLstTml();
            string type = "";
            int i = 0;
            var tmpLst = new List<ItemModel>();
            foreach  (var t in  Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems  )
            {
                if (t.Value.RtuFid  != 0) continue;
                int id = t.Value .RtuId ;
                string name = t.Value .RtuName;
                if (t.Value.RtuModel == EnumRtuModel.Wj3090) type = "3090终端";
                else if (t.Value.RtuModel == EnumRtuModel.Wj3005) type = "3005终端";
                else if (t.Value.RtuModel == EnumRtuModel.Wj3006) type = "3006终端";
                else if (t.Value.RtuModel == EnumRtuModel.Wj2090) type = "单灯设备";
                else if (t.Value.RtuModel == EnumRtuModel.Wj1080) type = "光控设备";
                else if (t.Value.RtuModel == EnumRtuModel.Wj1050) type = "电表设备";
                else if (t.Value.RtuModel == EnumRtuModel.Jd601) type = "节电设备";
                else if (t.Value.RtuModel == EnumRtuModel.Wj1090) type = "线路检测";
                else if (t.Value.RtuModel == EnumRtuModel.Wj4005) type = "4005终端";
                var ttt = new ItemModel();
                ttt.PhysicalId = t.Value .RtuPhyId ;
                ttt.ID = id;
                ttt.GroupName = "--";
                var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(id);
                ttt.AreaId = areaId;
                ttt.IsChecked = false;
                ttt.Name = name;
                ttt.Type = type;
                if (ttt.AreaId == this.CurrentSelectArea.Value)
                {
                   tmpLst.Add(ttt);
                   // ItemTmls.Add(ttt);
                //dictionaryC.Add(ttt.ID, i);
                //i++;
                }
               

            }
            var tmpLst2 = (from t in tmpLst orderby t.Type, t.PhysicalId  ascending select t).ToList();
            foreach (var t in tmpLst2)
            {
                ItemTmls.Add(t);
                dictionaryC.Add(t.ID, i);
                i++;
            }

            //for (int i = 0; i < Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentsInfoList.Count; i++)
            //{
            //    var t = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentsInfoList[i];
                
            //        int id = t.RtuId;
            //        string name = t.RtuName;
            //        var ttt = new ItemModel();
            //        ttt.ID = id;
            //        ttt.GroupName = "UnSet";
            //        ttt.IsChecked = false;
            //        ttt.Name = name;
            //        ItemTmls.Add(ttt);
            //        dictionaryC.Add(ttt.ID, i);
                
            //}
        }

        /// <summary>
        /// 保存终端地址到终端信息的关系映射
        /// </summary>
        private Dictionary<int, int> dictionaryC;

        /// <summary>
        /// 全部重新计算所有终端所属归属组名称
        /// </summary>
        private void ReLoadTmlsGrpsBelong()
        {
            foreach (var t in ItemTmls)
            {
                t.GroupName = "--";
                t.GroupId = -1;
            }
            foreach (var t in this.ChildTreeItems)
            {
                if (t.NodeType == TreeNodeType.IsGrp)
                {
                    ReLoadTmlsGrpsBelong(t);
                }
                else if (t.NodeType == TreeNodeType.IsTml)
                {
                    if (dictionaryC.ContainsKey(t.NodeId))
                    {
                        ItemTmls[dictionaryC[t.NodeId]].GroupName = t.NodeName;
                        ItemTmls[dictionaryC[t.NodeId]].GroupId = t.NodeId;
                    }
                }

                t.GetChildRtuCount();
            }
        }

        /// <summary>
        /// 提供递归查询终端树中的终端节点直属父节点组名称
        /// </summary>
        /// <param name="father"></param>
        private void ReLoadTmlsGrpsBelong(TreeItemViewModel father)
        {
            if (father.NodeType != TreeNodeType.IsGrp) return;
            foreach (var t in father.ChildTreeItems)
            {
                if (t.NodeType == TreeNodeType.IsGrp)
                {
                    ReLoadTmlsGrpsBelong(t);
                }
                else if (t.NodeType == TreeNodeType.IsTml)
                {
                    if (dictionaryC.ContainsKey(t.NodeId))
                    {
                        ItemTmls[dictionaryC[t.NodeId]].GroupName = father.NodeName;
                        ItemTmls[dictionaryC[t.NodeId]].GroupId = father.NodeId;
                    }
                }
            }
        }

        #endregion

        #region top command

        #region 强制重新请求服务器段数据 CMD

        private ICommand _cmdGetServerData;

        /// <summary>
        /// 强制重新请求服务器段数据
        /// </summary>
        public ICommand CmdGetServerData
        {
            get { return _cmdGetServerData ?? (_cmdGetServerData = new RelayCommand(ExCmdGetServerData)); }
        }

        private void ExCmdGetServerData()
        {

            Sr.EquipmentInfoHolding .Services.ServicesGrpSingleInfoHold.RequestGroupInfo( );
        }

        #endregion

        #region 清除选中的终端 CMD

        private DateTime _dtCleanSelected;
        private ICommand _cmdCleanSelected;

        /// <summary>
        /// 清除选中的终端
        /// </summary>
        public ICommand CmdCleanSelected
        {
            get { return _cmdCleanSelected ?? (_cmdCleanSelected = new RelayCommand(ExCleanAllSelected,CanExCleanAllSelected,true)); }
        }

        private bool CanExCleanAllSelected()
        {
            return DateTime.Now.Ticks - _dtCleanSelected.Ticks > 30000000;
        }
        private void ExCleanAllSelected()
        {
            _dtCleanSelected = DateTime.Now;
            foreach (var t in ItemTmls)
            {
                t.IsChecked = false;
            }
        }

        #endregion

        #region cmdadd


        private ICommand _CmdSCmdAddGrpave;

        public ICommand CmdAddGrp
        {
            get
            {
                if (_CmdSCmdAddGrpave == null)
                {
                    _CmdSCmdAddGrpave = new RelayCommand(AddCmdEx, CanAdd, false);
                }
                return _CmdSCmdAddGrpave;
            }
        }




        bool CanAdd()
        {
            return CurrentSelectArea !=null ;
        }

        void AddCmdEx()
        {

            //int maxIndx = 1;
            //foreach (var f in this.ChildTreeItems) if (f.NodeId >= maxIndx) maxIndx = f.NodeId + 1;

            var tmp = new Wlst.Sr.EquipmentInfoHolding.Model.GroupInformation(new GroupItemsInfo.GroupItem()
            {
                AreaId = CurrentSelectArea .Value ,
                GroupName = "新分组"+AddMaxId ,
                GroupId = AddMaxId ,
                LstTml = new List<int>()
            });
            AddMaxId += 1;
            this.ChildTreeItems.Add(new TreeItemViewModel( null, CurrentSelectArea .Value ,tmp  , true));
        }

        #endregion

        #region cmdedit

        /// <summary>
        /// 当修改树节点名称前需要备份前节点名称
        /// 若用户输入不合法则恢复原名称
        /// </summary>
        private string BackUpName;

        private ICommand _cmdEdit;

        /// <summary>
        /// 左侧树 编辑根节点
        /// </summary>
        public ICommand CmdEditGrp
        {
            get { return _cmdEdit ?? (_cmdEdit = new RelayCommand(ExEdit, CanExEdit, true)); }
        }

        private bool CanExEdit()
        {
            return true;
        }

        private void ExEdit()
        {
            foreach (var t in this.ChildTreeItems)
            {
                if (t.IsSelected == true)
                {
                    t.BackUpName = t.NodeName;
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
        }

        #endregion

        #region cmddel

        private ICommand _cmddel;

        /// <summary>
        /// 左侧树 删除根节点
        /// </summary>
        public ICommand CmdDelGrp
        {
            get { return _cmddel ?? (_cmddel = new RelayCommand(ExCmdDel, CanExCmdDel, true)); }
        }

        private bool CanExCmdDel()
        {
            return true;
        }

        private void ExCmdDel()
        {
            for (int i = this.ChildTreeItems.Count - 1; i >= 0;i-- )
                {
                    if (this.ChildTreeItems[i].IsSelected == true && this.ChildTreeItems[i].NodeType == TreeNodeType.IsGrp)
                    {
                        //if (this.ChildTreeItems[i].ChildTreeItems.Count != 0)
                        //{
                            var infoss = WlstMessageBox.Show("确认删除", "是否删除该分组，是 继续删除，否 退出.", WlstMessageBoxType.YesNo);
                            if (infoss != WlstMessageBoxResults.Yes) return;
                            this.ChildTreeItems.RemoveAt(i);
                    //}
                    //else
                    //{
                    //    this.ChildTreeItems.RemoveAt( i);
                    //}
                    IsDelete = ChildTreeItems.Count > 1;
                }

                }
            ReLoadTmlsGrpsBelong();
        }

        #endregion

        #region Selectall CMD

        private DateTime _dtSelectAll;
        private ICommand _cmdSelectAll;

        /// <summary>
        /// 清除选中的终端
        /// </summary>
        public ICommand CmdSelectAll
        {
            get { return _cmdSelectAll ?? (_cmdSelectAll = new RelayCommand(ExSelectAllSelected,CanExSelectAllSelected,true)); }
        }
        private bool CanExSelectAllSelected()
        {
            return DateTime.Now.Ticks - _dtSelectAll.Ticks > 30000000;
        }
        private void ExSelectAllSelected()
        {
            _dtSelectAll = DateTime.Now;
            foreach (var t in ItemTmls)
            {
                if (t.Type == CurrentListViewItem.Type)
                {
                    t.IsChecked = true;
                }
                
            }
        }

        #endregion

        #region  保存所有分组信息 CMD  提交服务器

        private DateTime _dtSave;
        private ICommand _cmdSave;

        public ICommand CmdSave
        {
            get { return _cmdSave ?? (_cmdSave = new RelayCommand(ExSave, CanSave, true)); }
        }

        private void ExSave()
        {
            _dtSave = DateTime.Now;
            Sr.EquipmentInfoHolding .Services.ServicesGrpSingleInfoHold.UpdateGroupsInfo( GetGrpsBelong() );
            Msg = DateTime.Now + " 已提交信息到服务器！";
            dtSnd = DateTime.Now;
           // Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.RequestGroupInfo();
          // Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InitLoad();
        }

        private bool CanSave()
        {
           // CETC50_Core .CoreServices .SysRunInfo
            return CurrentSelectArea!=null && Wlst.Cr.CoreMims.Services.UserInfo.CanW(CurrentSelectArea .Value ) && DateTime.Now.Ticks - _dtSave.Ticks > 30000000;
        }

        /// <summary>
        /// 获取整棵树的分组信息
        /// </summary>
        /// <returns></returns>
        private List<GroupInformation> GetGrpsBelong()
        {
            var lis = new List<Wlst.Sr.EquipmentInfoHolding.Model.GroupInformation>();

            int index = 1;
            foreach (var t in this.ChildTreeItems)
            {
                if (t.IsGroup == false) continue;
                var tmp =
                    new GroupInformation(new GroupItemsInfo.GroupItem()
                                             {
                                                 AreaId = CurrentSelectArea.Value,
                                                 GroupName = t.NodeName,
                                                 GroupId = t.NodeId,
                                                 LstTml =new List<int>( )
                                             }
                        )
                        {
                            Index = index++
                        };
                foreach (var g in t.ChildTreeItems)
                {
                    if (g.IsGroup) continue;
                    tmp.LstTml.Add(g.NodeId);
                }

                lis.Add(tmp);
            }
            return lis;
        }


  
        #endregion


        #region  界面筛选按钮 CMD & Option 显示所有 或 显示未分组

        private string _showOptionCmdContent;

        /// <summary>
        /// 筛选选项显示名称，默认  所有
        /// </summary>
        public string ShowOptionCmdContent
        {
            get
            {
                if (string.IsNullOrEmpty(_showOptionCmdContent)) _showOptionCmdContent = "显示所有";
                return _showOptionCmdContent;
            }
            set
            {
                if (_showOptionCmdContent != value)
                {
                    _showOptionCmdContent = value;
                    this.RaisePropertyChanged(() => ShowOptionCmdContent);
                }
            }
        }

        private ICommand _showOption;

        /// <summary>
        /// 筛选选项 执行命令  显示所有终端或显示未分组终端
        /// </summary>
        public ICommand ShowOption
        {
            get
            {
                if (_showOption == null)
                {
                    _showOption = new RelayCommand(ExShowOption );
                }
                return _showOption;
            }
        }

        private void ExShowOption()
        {
            if (ShowOptionCmdContent == "显示所有")
            {
                ShowOptionCmdContent = "显示未分组";
                for (int i = 0; i < ItemTmls.Count; i++)
                {
                    ItemTmls[i].ItemVisi = Visibility.Visible;
                }
            }
            else
            {
                ShowOptionCmdContent = "显示所有";
                for (int i = 0; i < ItemTmls.Count; i++)
                {
                    if (ItemTmls[i].GroupId < 1)
                        ItemTmls[i].ItemVisi = Visibility.Visible;
                    else ItemTmls[i].ItemVisi = Visibility.Collapsed;
                }
            }
        }

        #endregion

        #endregion

        #region  Drag & Drop

        #region DragSourceTree 成员  从树中拖出数据 树为源

        private DragSourceClassIDragSource DragSourceClassTree;

        public IDragSource DragSourceTree
        {
            get
            {
                if (DragSourceClassTree == null)
                {
                    DragSourceClassTree = new DragSourceClassIDragSource(DragSourceTreeStartDrag, DragSourceTreeDragData);
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
                if (dataAsPath.treeData == null) return DragDropEffects.None;
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
            var strGetSeletcTreeNodePath = GetSelectMvvm();
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
            else if (t.treeData != null)
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

                //组增加终端
                //todo
                var dropInfomation = new DropTargetInfomation(sender, e);
                var mvvm = dropInfomation.TargetItem as TreeItemViewModel;
                if (mvvm == null) return;
                int fatherMvvmId = 0;
                var fatherMvvm = mvvm;
                int count = 0;
                int wj3005 = 0;
                List<int> wj3006Lst = new List<int>();
                if (mvvm.NodeType == TreeNodeType.IsGrp)
                {
                    if (mvvm.NodeId == 0) return;
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
                    foreach (var tft in this.ChildTreeItems)
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
                        if (Wlst.Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(
                                CurrentSelectArea.Value, fatherMvvm.NodeId) != null)
                        {
                            var dic1 =
                                Wlst.Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.GetBandingInfo(
                                    CurrentSelectArea.Value, fatherMvvm.NodeId);
                            var timetable = new List<TimeTableInfoWithRtuOrGrpBandingInfo.TimeTableItem>();
                            for (int i = 1; i < 9; i++)
                            {
                                if (dic1.ContainsKey(i))
                                {
                                    var ttt =
                                        Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.GeteekTimeTableInfo(
                                            CurrentSelectArea.Value,
                                            dic1[i]);
                                    timetable.Add(ttt);
                                }
                            }

                            
                            foreach (var f in timetable)
                            {                               
                                foreach(var tt in f.RuleItems)
                                {
                                    if (tt.TimetableSectionId > 1) count += 1;
                                }
                                

                            }

                            //if (count > 0 && Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tmlNeedAdd].RtuModel == EnumRtuModel.Wj3005)
                            //{
                            //    UMessageBox.Show("禁止添加", "勾选值包含6路终端，无法加入使用多段时间表的分组", UMessageBoxButton.Yes);


                            //    break;
                            //}
                            if (count > 0 && Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection == false)
                            {
                                if(Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tmlNeedAdd].RtuModel != EnumRtuModel.Wj3005)
                                {
                                    fatherMvvm.ChildTreeItems.Add(new TreeItemViewModel(fatherMvvm,
                                                                                        CurrentSelectArea.Value,
                                                                                        Wlst.Sr.EquipmentInfoHolding
                                                                                            .Services.
                                                                                            EquipmentDataInfoHold.
                                                                                            InfoItems[tmlNeedAdd],
                                                                                        false));
                                    wj3006Lst.Add(tmlNeedAdd);
                                }
                                
                                else
                                {
                                    wj3005 += 1;
                                                                                                  
                                }
                                                                                            
                            }
                            else
                            {
                                fatherMvvm.ChildTreeItems.Add(new TreeItemViewModel(fatherMvvm,
                                                                                        CurrentSelectArea.Value,
                                                                                        Wlst.Sr.EquipmentInfoHolding
                                                                                            .Services.
                                                                                            EquipmentDataInfoHold.
                                                                                            InfoItems[tmlNeedAdd],
                                                                                        false));
                                this.ExCleanAllSelected();
                            }
                            
                        }
                        else
                        {
                            fatherMvvm.ChildTreeItems.Add(new TreeItemViewModel(fatherMvvm,
                                                                                            CurrentSelectArea.Value,
                                                                                            Wlst.Sr.EquipmentInfoHolding
                                                                                                .Services.
                                                                                                EquipmentDataInfoHold.
                                                                                                InfoItems[tmlNeedAdd],
                                                                                            false));
                            this.ExCleanAllSelected();
                        }
                                               
                    }
                }
                foreach (var sss in wj3006Lst)
                {
                    foreach (var x in ItemTmls)
                    {
                        if (x.ID == sss)
                        {
                            x.IsChecked = false;
                        }
                    }
                }
                if (wj3005 > 0 && Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection == false)
                {
                    
                    wj3006Lst.Clear();
                    UMessageBox.Show("禁止添加", "勾选值包含6路终端，若想实现多次开关灯请允许“二次开关灯”", UMessageBoxButton.Yes);
                }

                ReLoadTmlsGrpsBelong();

                
            }

            else
            {
                if (t.treeData == null) return;

                try
                {
                    var dropInfomation = new DropTargetInfomation(sender, e);
                    var mvvm = dropInfomation.TargetItem as TreeItemViewModel;
                    if (mvvm == null) return;
                    if (mvvm.NodeType != t.treeData.NodeType) return;
                    if (mvvm.Father != t.treeData.Father) return;
                    if (mvvm.Father == null) return;
                    if (mvvm.NodeId == t.treeData.NodeId) return;

                    if (!mvvm.Father.ChildTreeItems.Contains(t.treeData)) return;
                    mvvm.Father.ChildTreeItems.Remove(t.treeData);
                    int index = mvvm.Father.ChildTreeItems.IndexOf(mvvm);
                    mvvm.Father.ChildTreeItems.Insert(index, t.treeData);
                }
                catch (Exception ex)
                {
                    
                }

            }
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
            foreach (var t in ItemTmls)
            {
                if (t.IsChecked)
                {
                    lst.Add(t.ID);
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
            if (t.treeData == null) return;

            try
            {
                if (t.treeData.Father != null && t.treeData.NodeType == TreeNodeType.IsTml)
                {
                    int id = t.treeData.NodeId;
                    t.treeData.Father.ChildTreeItems.Remove(t.treeData);
                    foreach (var tm in ItemTmls)
                    {
                        if (tm.ID == id)
                        {
                            tm.GroupName = "--";
                        }
                    }
                    foreach (var g in ChildTreeItems)
                    {
                        g.GetChildRtuCount();
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

     
        private string _msg;

        /// <summary>
        /// 备注信息
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





        private ICommand _cmdFastSearch;

        /// <summary>
        /// 模糊查询
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
            if (string.IsNullOrEmpty(SearchText))
            {
                LoadTmlsInfo();
                ReLoadTmlsGrpsBelong();

            }
            else
            {
                ItemTmls.Clear();
                dictionaryC = new Dictionary<int, int>();
                int i = 0;
                string type = "";
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                 //判断区域

                    var areaRtuLst = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(CurrentSelectArea.Value);
                    if (areaRtuLst == null || areaRtuLst.LstTml.Count < 1) return;
                    if (areaRtuLst.LstTml.Contains(t.Key) == false) continue;

                    if (t.Value.RtuName.Contains(this.SearchText) || StringContainKeyword(t.Value.RtuName, SearchText) || t.Value.RtuPhyId.ToString().Contains(SearchText))
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
                        var searchResult = new ItemModel();
                        var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(t.Value.RtuId);
                        searchResult.AreaId = areaId;
                        searchResult.PhysicalId = t.Value.RtuPhyId;
                        searchResult.Type= type;
                        searchResult.Name = t.Value.RtuName;
                        //searchResult.GroupName =;
                        searchResult.ID = t.Value.RtuId;

                        this.ItemTmls.Add(searchResult);
                        dictionaryC.Add(searchResult.ID, i);
                        i++;
                    }
                }
                ReLoadTmlsGrpsBelong();

            }
        }


        /// <summary>
        /// 前者是否包含后者数据 
        /// </summary>
        /// <param name="containerStinng"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        private bool StringContainKeyword(string containerStinng, string keyword)
        {
            if (containerStinng == null) return false;
            if (containerStinng.Contains(keyword)) return true;
            string conv = chinesecap(containerStinng);
            if (conv.Contains(keyword)) return true;
            if (containerStinng.ToUpper().Contains(keyword.ToUpper())) return true;
            return false;
        }


        /// <summary>
        /// 返回汉字字符串的拼音的首字母
        /// </summary>
        /// <param name="chinesestr">要转换的字符串</param>
        /// <returns></returns>
        public string chinesecap(string chinesestr)
        {
            byte[] zw = new byte[2];
            string charstr = "";
            string capstr = "";
            for (int i = 0; i <= chinesestr.Length - 1; i++)
            {
                charstr = chinesestr.Substring(i, 1).ToString(CultureInfo.InvariantCulture);
                zw = System.Text.Encoding.Default.GetBytes(charstr);
                // 得到汉字符的字节数组
                if (zw.Length == 2)
                {
                    int i1 = (short)(zw[0]);
                    int i2 = (short)(zw[1]);
                    long chinesestrInt = i1 * 256 + i2;
                    //table of the constant list
                    // a; //45217..45252
                    // z; //54481..55289
                    capstr += GetChinesefirst(chinesestrInt);
                }
                else
                {
                    capstr += charstr;
                }

                //capstr = capstr + chinastr;
            }

            return capstr;
        }

        private string GetChinesefirst(long chinesestrInt)
        {
            string chinastr = "";
            //table of the constant list
            // a; //45217..45252
            // b; //45253..45760
            // c; //45761..46317
            // d; //46318..46825
            // e; //46826..47009
            // f; //47010..47296
            // g; //47297..47613

            // h; //47614..48118
            // j; //48119..49061
            // k; //49062..49323
            // l; //49324..49895
            // m; //49896..50370
            // n; //50371..50613
            // o; //50614..50621
            // p; //50622..50905
            // q; //50906..51386

            // r; //51387..51445
            // s; //51446..52217
            // t; //52218..52697
            //没有u,v
            // w; //52698..52979
            // x; //52980..53640
            // y; //53689..54480
            // z; //54481..55289

            if ((chinesestrInt >= 45217) && (chinesestrInt <= 45252))
            {
                chinastr = "a";
            }
            else if ((chinesestrInt >= 45253) && (chinesestrInt <= 45760))
            {
                chinastr = "b";
            }
            else if ((chinesestrInt >= 45761) && (chinesestrInt <= 46317))
            {
                chinastr = "c";
            }
            else if ((chinesestrInt >= 46318) && (chinesestrInt <= 46825))
            {
                chinastr = "d";
            }
            else if ((chinesestrInt >= 46826) && (chinesestrInt <= 47009))
            {
                chinastr = "e";
            }
            else if ((chinesestrInt >= 47010) && (chinesestrInt <= 47296))
            {
                chinastr = "f";
            }
            else if ((chinesestrInt >= 47297) && (chinesestrInt <= 47613))
            {
                chinastr = "g";
            }
            else if ((chinesestrInt >= 47614) && (chinesestrInt <= 48118))
            {
                chinastr = "h";
            }

            else if ((chinesestrInt >= 48119) && (chinesestrInt <= 49061))
            {
                chinastr = "j";
            }
            else if ((chinesestrInt >= 49062) && (chinesestrInt <= 49323))
            {
                chinastr = "k";
            }
            else if ((chinesestrInt >= 49324) && (chinesestrInt <= 49895))
            {
                chinastr = "l";
            }
            else if ((chinesestrInt >= 49896) && (chinesestrInt <= 50370))
            {
                chinastr = "m";
            }

            else if ((chinesestrInt >= 50371) && (chinesestrInt <= 50613))
            {
                chinastr = "n";
            }
            else if ((chinesestrInt >= 50614) && (chinesestrInt <= 50621))
            {
                chinastr = "o";
            }
            else if ((chinesestrInt >= 50622) && (chinesestrInt <= 50905))
            {
                chinastr = "p";
            }
            else if ((chinesestrInt >= 50906) && (chinesestrInt <= 51386))
            {
                chinastr = "q";
            }

            else if ((chinesestrInt >= 51387) && (chinesestrInt <= 51445))
            {
                chinastr = "r";
            }
            else if ((chinesestrInt >= 51446) && (chinesestrInt <= 52217))
            {
                chinastr = "s";
            }
            else if ((chinesestrInt >= 52218) && (chinesestrInt <= 52697))
            {
                chinastr = "t";
            }
            else if ((chinesestrInt >= 52698) && (chinesestrInt <= 52979))
            {
                chinastr = "w";
            }
            else if ((chinesestrInt >= 52980) && (chinesestrInt <= 53640))
            {
                chinastr = "x";
            }
            else if ((chinesestrInt >= 53689) && (chinesestrInt <= 54480))
            {
                chinastr = "y";
            }
            else if ((chinesestrInt >= 54481) && (chinesestrInt <= 55289))
            {
                chinastr = "z";
            }
            return chinastr;
        }


    };

    public class HelpDragAndDrop
    {
        public HelpDragAndDrop(TreeItemViewModel tv)
        {
            treeData = tv;
            listData = new List<int>();
        }

        public HelpDragAndDrop(List<int> lst)
        {
            treeData = null;
            listData = new List<int>();
            foreach (var t in lst)
            {
                listData.Add(t);
            }
        }

        public TreeItemViewModel treeData;

        /// <summary>
        /// 拖转携带的数据
        /// 树到终端大致为  id-grp-name/id
        /// 终端到树大致为  id/id/id/id
        /// </summary>
        public List<int> listData;



    }


}