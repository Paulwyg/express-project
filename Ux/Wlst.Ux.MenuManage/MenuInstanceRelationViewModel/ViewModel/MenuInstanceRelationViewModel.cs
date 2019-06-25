using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using DragDropExtend.DragAndDrop;


using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.ComponentHold;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Models;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.MenuManage.MenuInstanceRelationViewModel.Services;
using EventIdAssign = Wlst.Cr.CoreOne.CoreIdAssign.EventIdAssign;

namespace Wlst.Ux.MenuManage.MenuInstanceRelationViewModel.ViewModel
{
    [Export(typeof (IIInstancesRelationManagement))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MenuInstanceRelationViewModel : ObservableObject,
                                                                    IIInstancesRelationManagement
    {


        #region IITab
        public int Index
        {
            get { return 1; }
        }
        /// <summary>
        /// 当显示在主界面的tab页面时 显示的title
        /// </summary>
        public string Title
        {
            get { return "菜单实例管理"; }
        }

        /// <summary>
        /// 当显示在主界面tab时是否允许用户关闭  地图不运行关闭
        /// </summary>
        public bool CanClose
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        public MenuInstanceRelationViewModel()
        {
            OnInit();
            this.NavOnLoad();
        }

        public void OnUserHideOrClosing()
        {
            ClassicItems.Clear();
            InstancesItems.Clear();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            LoadItems();
        }

        private void LoadItems()
        {
            LoadClassicItems();
            LoadInatancesItems();
        }

        /// <summary>
        /// 加载模板数量
        /// </summary>
        private void LoadClassicItems()
        {
            //try
            //{
            //    ClassicItems.Clear();
            //}
            //catch (Exception)
            //{
            //}
            var tmp = new ObservableCollection<NameValueInt>();
            foreach (var t in ServerClassic.GetClassicDic)
            {
                tmp.Add(new NameValueInt()
                                     {
                                         Value = t.Value.Id,
                                         Name = t.Value.Name
                                     });
            }
            this.ClassicItems = tmp;

        }

        /// <summary>
        /// 加载菜单实例
        /// </summary>
        private void LoadInatancesItems()
        {
            //try
            //{
            //    InstancesItems.Clear();
            //}
            //catch (Exception)
            //{
            //}
            var tmp = new ObservableCollection<MenuInstancesViewModel>();
            foreach (var t in ServerInstanceRoot.GetInstancesDic)
            {
                var menu = new MenuInstancesViewModel()
                               {
                                   Id = t.Value.Id,
                                   IdClassic = t.Value.IdClassic,
                                   Key = t.Value.Key,
                                   Name = t.Value.Name
                               };
                tmp.Add(menu);
            }
            InstancesItems = tmp;

            if (InstancesItems.Count > 0) CurrentSelectInstancesItem = InstancesItems[0];
        }

        #region iMenuInstanceManage 成员

        private ObservableCollection<MenuInstancesViewModel> _instancesItems;

        /// <summary>
        /// 菜单实例
        /// </summary>
        public ObservableCollection<MenuInstancesViewModel> InstancesItems
        {
            get
            {
                if (_instancesItems == null) _instancesItems = new ObservableCollection<MenuInstancesViewModel>();
                return _instancesItems;
            }
            set
            {
                if (value == _instancesItems) return;
                _instancesItems = value;
                this.RaisePropertyChanged(() => this.InstancesItems);
            }
        }

        private MenuInstancesViewModel _currentSelectInstancesItem;

        /// <summary>
        /// 当前选中的设备类型
        /// </summary>
        public MenuInstancesViewModel CurrentSelectInstancesItem
        {
            get
            {
                if (_currentSelectInstancesItem == null) _currentSelectInstancesItem = new MenuInstancesViewModel();
                return _currentSelectInstancesItem;
            }
            set
            {
                if (_currentSelectInstancesItem != value)
                {
                    _currentSelectInstancesItem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectInstancesItem);
                    OnSelectInstancesChange();
                }
            }
        }

        private void OnSelectInstancesChange()
        {
            OnSelectInstanceChangeforCurrentInstanceClassic();
            OnSelectInstancesChangeforList();
            OnSelectInstancesChangeforThee();
        }


        private void OnSelectInstanceChangeforCurrentInstanceClassic()
        {
            if (CurrentSelectInstancesItem == null)
            {
                return;
            }
            foreach (var t in ClassicItems)
            {
                if (t.Value == CurrentSelectInstancesItem.IdClassic)
                {
                    CurrenSelectClassicItem = t;
                    break;
                }
            }
        }

        private ObservableCollection<NameValueInt> _classicItems;

        /// <summary>
        /// 菜单母版
        /// </summary>
        public ObservableCollection<NameValueInt> ClassicItems
        {
            get
            {
                if (_classicItems == null) _classicItems = new ObservableCollection<NameValueInt>();
                return _classicItems;
            }
            set
            {
                if (value == _classicItems) return;
                _classicItems = value;
                this.RaisePropertyChanged(() => this.ClassicItems);
            }
        }

        private NameValueInt _currenSelectClassicItem;

        /// <summary>
        /// 当前选中的目标菜单
        /// </summary>
        public NameValueInt CurrenSelectClassicItem
        {
            get
            {
                if (_currenSelectClassicItem == null) _currenSelectClassicItem = new NameValueInt();
                return _currenSelectClassicItem;
            }
            set
            {
                if (_currenSelectClassicItem != value)
                {
                    _currenSelectClassicItem = value;
                    this.RaisePropertyChanged(() => this.CurrenSelectClassicItem);
                }
            }
        }

        #endregion

        private DateTime _dtaddEx;
        private DateTime _dtUpdateEx;
        private DateTime _dtDeleteEx;
        private DateTime _dtSaveEx;

        #region add

        private RelayCommand _relayAddCommand;

        private void AddEx()
        {
            _dtaddEx = DateTime.Now;
            var intValId = ServerInstanceRoot.GetMaxAviableInstancesId();
            var instances = new MenuInstancesViewModel()
                                {
                                    Id = intValId,
                                    IdClassic = -1,
                                    Key = DateTime.Now.ToString(CultureInfo.InvariantCulture).Trim(),
                                    Name = "NewMenu"
                                };
            //var fff = new DataHoldingExtend.MenuInstancesHoldingExtend();
            //fff.AddMenuInstances(instances.Value, instances.Name, instances.Key, instances.IdClassic);
            //fff.WriteUpdateDb(instances.Value);
            ServerInstanceRoot.UpdateMenuInstances(instances.Id, instances.Name, instances.Key,
                                                                             instances.IdClassic);

            //Base_MenuControl.Services.ServerInstanceRelation.AddMenuInstanceRelation(0, 0, 1, "NewMenu", instances.Value);  


            foreach (var t in InstancesItems)
            {
                if (t.Id == intValId)
                {
                    CurrentSelectInstancesItem = t;
                    return;
                }
            }
            InstancesItems.Add(instances);
            CurrentSelectInstancesItem = instances;
        }
        private bool CanAddEx()
        {
            return DateTime.Now.Ticks - _dtaddEx.Ticks > 30000000;
        }
        public ICommand CmdAdd
        {
            get { return _relayAddCommand ?? (_relayAddCommand = new RelayCommand(AddEx,CanAddEx,true )); }
        }

        #endregion

        #region Update

        private RelayCommand _relayUpdateCommand;

        private void UpdateEx()
        {
            _dtUpdateEx = DateTime.Now;
            if (CurrentSelectInstancesItem == null) return;
            if (CurrentSelectInstancesItem.Id < MenuIdControlAssign.MenuInstanceKeyIdMin)
                return;
            //var fff = new DataHoldingExtend.MenuInstancesHoldingExtend();
            //fff.DeleteMenuInstances(CurrentSelectInstancesItem.Value);

            //Base_MenuControl.Services.ServerInstanceRoot.DeleteMenuInstances(CurrentSelectInstancesItem.Value);
            CurrentSelectInstancesItem.IdClassic = CurrenSelectClassicItem.Value;
            //fff.AddMenuInstances(CurrentSelectInstancesItem.Value, CurrentSelectInstancesItem.Name,
            //                     CurrentSelectInstancesItem.Key,
            //                     CurrentSelectInstancesItem.IdClassic);

            //fff.WriteUpdateDb();
            ServerInstanceRoot.UpdateMenuInstances(CurrentSelectInstancesItem.Id,
                                                                             CurrentSelectInstancesItem.Name,
                                                                             CurrentSelectInstancesItem.Key,
                                                                             CurrentSelectInstancesItem.IdClassic);
            OnSelectInstancesChangeforList();
        }
        private bool CanUpdateEx()
        {
            return DateTime.Now.Ticks - _dtUpdateEx.Ticks > 30000000;
        }
        public ICommand CmdUpdage
        {
            get { return _relayUpdateCommand ?? (_relayUpdateCommand = new RelayCommand(UpdateEx,CanUpdateEx,true )); }
        }

        #endregion

        #region Delete

        private RelayCommand _relayDeleteCommand;

        private void DeleteEx()
        {
            _dtDeleteEx = DateTime.Now;
            if (CurrentSelectInstancesItem == null) return;
            if (CurrentSelectInstancesItem.Id < MenuIdControlAssign.MenuInstanceKeyIdMin)
                return;
            //var fff = new DataHoldingExtend.MenuInstancesHoldingExtend();
            int intId = CurrentSelectInstancesItem.Id;
            //fff.DeleteMenuInstances(intId);
            //fff.WriteUpdateDb();

            ServerInstanceRoot.DeleteMenuInstances(intId);
            if (InstancesItems.Contains(CurrentSelectInstancesItem))
            {
                InstancesItems.Remove(CurrentSelectInstancesItem);
                if (InstancesItems.Count > 0) CurrentSelectInstancesItem = InstancesItems[0];
            }
        }
        private bool CanDeleteEx()
        {
            return DateTime.Now.Ticks - _dtDeleteEx.Ticks > 30000000;
        }
        public ICommand CmdDelete
        {
            get { return _relayDeleteCommand ?? (_relayDeleteCommand = new RelayCommand(DeleteEx ,CanDeleteEx,true)); }
        }

        #endregion

        #region CmdUpDown  Down

        private RelayCommand _relayUpDownCommand;

        private void UpDownEx()
        {
            _dtDeleteEx = DateTime.Now;

            try
            {
                var info = GetSelectMvvm();
                if (info.FatherMenu == null) return;
                var father = info.FatherMenu;
                int currentindex = 0;
                bool find = false;
                foreach (var t in father.ChildTreeItems)
                {
                    if (t.MenuId == info.MenuId)
                    {
                        find = true;
                        break;
                    }
                    else
                    {
                        currentindex++;
                    }
                }
                if (find == false) return;
                if (father.ChildTreeItems.Count <= currentindex + 1) return;
                var tmp = father.ChildTreeItems[currentindex + 1];
                father.ChildTreeItems.RemoveAt(currentindex);
                father.ChildTreeItems.RemoveAt(currentindex);
                father.ChildTreeItems.Insert(currentindex, info);
                father.ChildTreeItems.Insert(currentindex, tmp);
            }
            catch (Exception ex)
            {
                
            }

        }

        private bool CanUpDownEx()
        {
            return DateTime.Now.Ticks - _dtDeleteEx.Ticks > 10000000 && GetSelectMvvm() !=null ;
        }
        public ICommand CmdUpDown
        {
            get { return _relayUpDownCommand ?? (_relayUpDownCommand = new RelayCommand(UpDownEx ,CanUpDownEx,true)); }
        }

        #endregion


       
        #region save

        private RelayCommand _relaySaveCommand;

        private void SaveEx()
        {
            _dtSaveEx = DateTime.Now;
            if (CurrentSelectInstancesItem == null) return;
            if (ChildTreeItems == null) return;
            int instancesId = CurrentSelectInstancesItem.Id;
            //var fff = new DataHoldingExtend.MenuInstanceRelationHoldingExtend();
            //fff.DeleteMenuInstanceRelation(instancesId);

            //Base_MenuControl.Services.ServerInstanceRelation. 

            var lst = new List<MenuInstancesRelation>();
            int index = 0;
            foreach (var t in ChildTreeItems)
            {
                if (t.MenuId != 0)
                {
                    lst.Add(new MenuInstancesRelation()
                                {
                                    FatherId = 0,
                                    Id = t.MenuId,
                                    InstancesId = t.InstanceId,
                                    Name = t.Name,
                                    SortIndex = index
                                });

                    //fff.AddMenuInstanceRelation(0, t.MenuId, index, t.Name, t.InstanceId);
                    index++;
                }
                if (t.MenuId == 0 ||
                    (t.MenuId >= MenuIdControlAssign.MenuFileGroupIdMin &&
                     t.MenuId <= MenuIdControlAssign.MenuFileGroupIdMax))
                {
                    var ff = this.Ex(t);
                    foreach (var gg in ff)
                    {
                        lst.Add(gg);
                    }
                }
            }
            ServerInstanceRelation.UpdateMenuInstanceRelation(instancesId,
                                                                                        CurrentSelectInstancesItem.Key,
                                                                                        lst);

        }

        private List<MenuInstancesRelation> Ex(MenuTreeItemViewModel mvvm)
        {
            var lst = new List<MenuInstancesRelation>();
            int index = 0;
            if (mvvm.ChildTreeItems == null) return lst;
            foreach (var t in mvvm.ChildTreeItems)
            {
                lst.Add(new MenuInstancesRelation()
                            {
                                FatherId = mvvm.MenuId,
                                Id = t.MenuId,
                                InstancesId = t.InstanceId,
                                Name = t.Name,
                                SortIndex = index
                            });
                //fff.AddMenuInstanceRelation(mvvm.MenuId, t.MenuId, index, t.Name, t.InstanceId);
                index++;

                if (t.MenuId == 0 ||
                    (t.MenuId >= MenuIdControlAssign.MenuFileGroupIdMin &&
                     t.MenuId <= MenuIdControlAssign.MenuFileGroupIdMax))
                {
                    var ff = this.Ex(t);
                    foreach (var ggg in ff)
                    {
                        lst.Add(ggg);
                    }
                }
            }
            return lst;
        }
        private bool CanSaveEx()
        {
            return DateTime.Now.Ticks - _dtSaveEx.Ticks > 30000000;
        }
        public ICommand CmdSave
        {
            get { return _relaySaveCommand ?? (_relaySaveCommand = new RelayCommand(SaveEx,CanSaveEx,true )); }
        }

        #endregion
    };

    //list
    public partial class MenuInstanceRelationViewModel
    {
        private ObservableCollection<MenuItemBase> _menuItems;

        public ObservableCollection<MenuItemBase> MenuItems
        {
            get
            {
                if (_menuItems == null) _menuItems = new ObservableCollection<MenuItemBase>();
                return _menuItems;
            }
        }

        private MenuItemBase _currentSelectMenuItem;

        public MenuItemBase CurrentSelectMenuItem
        {
            get
            {
                if (_currentSelectMenuItem == null) _currentSelectMenuItem = new MenuItemBase() {Id = -1};
                return _currentSelectMenuItem;
            }
            set
            {
                if (_currentSelectMenuItem != value)
                {
                    _currentSelectMenuItem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectMenuItem);
                }
            }
        }

        private void OnSelectInstancesChangeforList()
        {
            MenuItems.Clear();

            if (CurrentSelectInstancesItem == null) return;
            int classicId = CurrentSelectInstancesItem.IdClassic;
            if (!ServerClassic.GetClassicDic.ContainsKey(classicId)) return;
            var lstMenu = new List<IIMenuItem>();
            foreach (var f in ServerClassic.GetClassicDic[classicId].Items)
            {
                var t = MenuComponentHolding.GetMenuItemById(f);
                if (t != null)
                {
                    lstMenu.Add(t);
                }
            }

            if (lstMenu.Count == 0) return;
            foreach (IIMenuItem t in lstMenu)
            {
                if (t.Id < MenuIdControlAssign.MenuIdMin ||
                    t.Id > MenuIdControlAssign.MenuIdMax) continue;
                //var eng = DataHolding.EngHolding.GetEngValue(t.Value);
              //  var eng = I36N.Services.I36N.ConvertByCoding(t.Id.ToString()); //
                var menuItem = new MenuItemBase()
                                   {
                                       Description = t.Description,
                                       Id = t.Id,
                                       Tooltips = t.Tooltips,
                                       Text = t.Text
                                   };


                //if (!string.IsNullOrEmpty(eng) && !eng.Contains( "issing")) menuItem.Text = eng;
                //if (!string.IsNullOrEmpty(eng.Item2)) menuItem.Tooltips = eng.Item2;

                MenuItems.Add(menuItem);
            }
        }

    };

    //tree
    public partial class MenuInstanceRelationViewModel
    {

        private ObservableCollection<MenuTreeItemViewModel> _childTreeItems;

        public ObservableCollection<MenuTreeItemViewModel> ChildTreeItems
        {
            get
            {
                if (_childTreeItems == null) _childTreeItems = new ObservableCollection<MenuTreeItemViewModel>();
                return _childTreeItems;
            }
        }

        private void OnSelectInstancesChangeforThee()
        {
            LoadChildren();
        }

        private void LoadChildren()
        {
            ChildTreeItems.Clear();
            if (CurrentSelectInstancesItem == null) return;
            if (
                !ServerInstanceRelation.GetInstanceRelatioinDic.ContainsKey(
                    CurrentSelectInstancesItem.Id))
            {
                ChildTreeItems.Add(new MenuTreeItemViewModel(null, CurrentSelectInstancesItem.Id, 0)
                                       {
                                           Name ="NewMenu"
                                       });
                return;
            }

            ChildTreeItems.Add(new MenuTreeItemViewModel(null, CurrentSelectInstancesItem.Id, 0)
                                   {
                                       Name =
                                           ServerInstanceRoot.GetInstancesDic[
                                               CurrentSelectInstancesItem.Id]
                                           .Name
                                   });
            foreach (var t in ChildTreeItems) t.LoadChildren();

            foreach (var t in ChildTreeItems)
            {
                t.IsExpanded = true;
                ExpandedChild(t);
            }
        }

        private void ExpandedChild(MenuTreeItemViewModel fff)
        {
            if (fff.ChildTreeItems != null && fff.ChildTreeItems.Count > 0)
            {
                foreach (var t in fff.ChildTreeItems)
                {
                    t.IsExpanded = true;
                    ExpandedChild(t);
                }
            }
        }

        /// <summary>
        /// 获取菜单树当前选择的菜单
        /// </summary>
        /// <returns></returns>
        public MenuTreeItemViewModel GetSelectMvvm()
        {

            for (int i = 0; i < ChildTreeItems.Count; i++)
            {
                if (ChildTreeItems[i].GetSelectMvvm() != null)
                {
                    return ChildTreeItems[i].GetSelectMvvm();
                }
            }

            return null;
        }
    };

    //drag and drop
    public partial class MenuInstanceRelationViewModel
    {
        #region DragSourceList 成员

        private IDragSource _dragSourceList;

        public IDragSource DragSourceList
        {
            get
            {
                return _dragSourceList ??
                       (_dragSourceList = new DragSourceClassIDragSource(StartDragList, DragDataList));
            }
        }

        public DragDropEffects StartDragList(object data)
        {
            try
            {
                if (((MenuItemBase) data).Id >= MenuIdControlAssign.MenuIdMin &&
                    ((MenuItemBase) data).Id <= MenuIdControlAssign.MenuIdMax)
                {
                    return DragDropEffects.Copy | DragDropEffects.Move;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return DragDropEffects.None;
        }

        public object DragDataList()
        {
            if (CurrentSelectMenuItem == null) return null;
            if (CurrentSelectMenuItem.Id >= MenuIdControlAssign.MenuIdMin &&
                CurrentSelectMenuItem.Id <= MenuIdControlAssign.MenuIdMax)
            {
                return CurrentSelectMenuItem;
            }
            return null;
        }

        #endregion

        #region IDragSourceTree 成员

        private IDragSource _dragSourceTree;

        public IDragSource DragSourceTree
        {
            get { return _dragSourceTree ?? (_dragSourceTree = new DragSourceClassIDragSource(StartDragTree, DragDataTree)); }
        }


        public DragDropEffects StartDragTree(object data)
        {
            try
            {
                var mvvm = data as MenuTreeItemViewModel;
                if (mvvm == null) return DragDropEffects.None;
                if (mvvm.MenuId >= MenuIdControlAssign.MenuIdMin &&
                    mvvm.MenuId <= MenuIdControlAssign.MenuIdMax)
                {
                    return DragDropEffects.Move;
                }
                return DragDropEffects.None;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return DragDropEffects.None;
        }

        public object DragDataTree()
        {
            MenuTreeItemViewModel mvvm = GetSelectMvvm();
            if (mvvm == null) return null;
            if (mvvm.MenuId >= MenuIdControlAssign.MenuIdMin &&
                mvvm.MenuId <= MenuIdControlAssign.MenuIdMax)
            {
                return mvvm;
            }
            return false;
        }



        #endregion

        #region IDropTarget 成员

        private IDropTarget _dropTargetTree;

        public IDropTarget DropTargetTree
        {
            get
            {
                return _dropTargetTree ??
                       (_dropTargetTree = new DropTargetClassIDropTarget(DragDropEffectsTree, DropTree));
            }
        }

        public DragDropEffects DragDropEffectsTree(object data)
        {
            try
            {
                var menuInstance = data as MenuItemBase;
                if (menuInstance != null)
                {
                    if (menuInstance.Id >= MenuIdControlAssign.MenuIdMin &&
                        menuInstance.Id <= MenuIdControlAssign.MenuIdMax)
                    {
                        return System.Windows.DragDropEffects.Copy | System.Windows.DragDropEffects.Move;
                    }
                    return DragDropEffects.None;
                }

                var mvvmTree = data as MenuTreeItemViewModel;
                if (mvvmTree != null)
                {
                    if (mvvmTree.MenuId >= MenuIdControlAssign.MenuIdMin &&
                        mvvmTree.MenuId <= MenuIdControlAssign.MenuIdMax)
                    {
                        return System.Windows.DragDropEffects.Copy | System.Windows.DragDropEffects.Move;
                    }
                    return DragDropEffects.None;
                }
                return DragDropEffects.None;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return DragDropEffects.None;
        }

        public void DropTree(object sender, DragEventArgs e, object data)
        {
            var dropInfomation = DragDropExtend.DragAndDrop.DropTargetInfomation.GetSelectItemByUiElement(sender, e);
            var mvvm = dropInfomation as MenuTreeItemViewModel;
            if (mvvm == null) return;
            var mvvmFather = mvvm;
            int insertIndex = 0;
            if (mvvm.MenuId >= MenuIdControlAssign.MenuIdMin &&
                mvvm.MenuId <= MenuIdControlAssign.MenuIdMax)
            {
                mvvmFather = mvvm.FatherMenu;
                if (mvvmFather == null) return;
                foreach (var t in mvvmFather.ChildTreeItems)
                {
                    if (t.MenuId == mvvm.MenuId)
                    {
                        break;
                    }
                    insertIndex++;
                }
            }

            var menuItem = data as MenuItemBase;
            if (menuItem != null)
            {
                try
                {
                    foreach (var t in mvvmFather.ChildTreeItems)
                    {
                        if (t.MenuId == menuItem.Id)
                        {
                            return;
                        }
                    }
                    mvvmFather.ChildTreeItems.Insert(insertIndex,
                                                     new MenuTreeItemViewModel(mvvmFather, mvvmFather.InstanceId,
                                                                               menuItem.Id) {Name = menuItem.Text});
                    //new DataHoldingExtend.MenuInstanceRelationHoldingExtend().AddMenuInstanceRelation(  todo
                    //    mvvmFather.MenuId, menuItem.Value, insertIndex, menuItem.Text, mvvmFather.InstanceId);

                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            var mvvmTree = data as MenuTreeItemViewModel; //还原 原始移动项数据
            if (mvvmTree != null)
            {
                if (mvvmTree.MenuId >= MenuIdControlAssign.MenuIdMin &&
                    mvvmTree.MenuId <= MenuIdControlAssign.MenuIdMax)
                {
                    if (mvvmTree.MenuId == mvvm.MenuId)
                    {
                        if (mvvmTree.FatherMenu != null && mvvm.FatherMenu != null)
                        {
                            if (mvvm.FatherMenu.MenuId == mvvmTree.FatherMenu.MenuId)
                            {
                                return; //判断是否由于不小心操作 移动源与目的地相同 则不操作
                            }

                            //new DataHoldingExtend.MenuInstanceRelationHoldingExtend().DeleteMenuInstanceRelation( todo
                            //    mvvmTree.FatherMenu.InstanceId, mvvmTree.FatherMenu.MenuId, mvvmTree.MenuId);
                            mvvmTree.FatherMenu.ChildTreeItems.Remove(mvvmTree);
                            return;

                        }
                    }
                    int menuId = mvvmTree.MenuId;
                    string name = mvvmTree.Name;

                    if (mvvmTree.FatherMenu != null)
                    {
                        //new DataHoldingExtend.MenuInstanceRelationHoldingExtend().DeleteMenuInstanceRelation( todo
                        //    mvvmTree.FatherMenu.InstanceId, mvvmTree.FatherMenu.MenuId, mvvmTree.MenuId);
                        mvvmTree.FatherMenu.ChildTreeItems.Remove(mvvmTree);
                    }

                    foreach (var t in mvvmFather.ChildTreeItems)
                    {
                        if (t.MenuId == menuId) return;
                    }


                    //var lst = new List<MenuTreeItemViewModel>();
                    //foreach (var t in mvvmFather.ChildTreeItems)
                    //{
                    //    lst.Add(t);
                    //}
                    //mvvmFather.ChildTreeItems.Clear();
                    //var newAdd = new MenuTreeItemViewModel(mvvmFather, mvvmFather.InstanceId, menuId) {Name = name};
                    //bool boladd = false;
                    //for (int i = 0; i < lst.Count;i++ )
                    //{
                    //    if(i==insertIndex )
                    //    {
                    //        mvvmFather.ChildTreeItems.Add(newAdd);
                    //        boladd = true;
                    //    }
                    //    mvvmFather.ChildTreeItems.Add(lst[i]);
                    //}
                    //if (!boladd) mvvmFather.ChildTreeItems.Add(newAdd);
                    mvvmFather.ChildTreeItems.Insert(insertIndex,
                                                     new MenuTreeItemViewModel(mvvmFather, mvvmFather.InstanceId, menuId)
                                                         {Name = name});
                    //new DataHoldingExtend.MenuInstanceRelationHoldingExtend().AddMenuInstanceRelation( //todo
                    //    mvvmFather.MenuId, menuId, insertIndex, name, mvvmFather.InstanceId);
                }
            }

        }


        #endregion
    }

    //events
    public partial class MenuInstanceRelationViewModel
    {
        private void OnInit()
        {
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);
        }

        #region IEventAggregator Subscription

        public void FundEventHandler(PublishEventArgs args)
        {
            int currentselectinstanceid = -1;
            if(CurrentSelectInstancesItem!=null  )
            {
                currentselectinstanceid = CurrentSelectInstancesItem.Id;
            }
            this.NavOnLoad();
            foreach (var t in InstancesItems )
            {
                if(t.Id ==currentselectinstanceid )
                {
                    CurrentSelectInstancesItem = t;
                    break;
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
            if (args.EventType == PublishEventType.Core)
            {
                if (args.EventId == Wlst.Sr.Menu.Services.EventIdAssign.ClassicMenuUpdate)
                {
                    return true;
                }
                else if (args.EventId == Wlst.Sr.Menu.Services.EventIdAssign.ClassicMenuLoadUpdate)
                {
                    return true;
                }
                else if (args.EventId ==EventIdAssign.MenuComponentAdd)
                {
                    return true;
                }
                else if (args.EventId == EventIdAssign.MenuComponentDelete)
                {
                    return true;
                }
                else if (args.EventId == Wlst.Sr.Menu.Services.EventIdAssign.MenuInstanceLoadUpdate)
                {
                    return true;
                }
                else if (args.EventId == Wlst.Sr.Menu.Services.EventIdAssign.MenuInstanceRelationLoadUpdate)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       