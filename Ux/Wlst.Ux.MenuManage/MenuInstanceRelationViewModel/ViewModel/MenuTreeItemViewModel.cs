using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.ComponentHold;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.MenuManage.MenuInstanceRelationViewModel.ViewModel
{
    public class MenuTreeItemViewModel : ObservableObject
    {

        public MenuTreeItemViewModel(MenuTreeItemViewModel mvvmFather, int instanceId, int menuId)
        {
            this._fatherMenu = mvvmFather;
            LabNameVisi = Visibility.Visible;
            TxbNameVisi = Visibility.Hidden;
            InstanceId = instanceId;
            this.MenuId = menuId;
            BackUpName = "";
        }


        private ObservableCollection<MenuTreeItemViewModel> _childTreeItems;

        public ObservableCollection<MenuTreeItemViewModel> ChildTreeItems
        {
            get
            {
                if (_childTreeItems == null) _childTreeItems = new ObservableCollection<MenuTreeItemViewModel>();
                return _childTreeItems;
            }
        }

        /// <summary>
        /// 标记界面上Lable可视化状态
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
        /// 标记界面text可视化状态 平时显示时不需要显示text
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
            if (MenuId >= MenuIdControlAssign.MenuFileGroupIdMin)
            {
                BackUpName = Name;
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
            if (Name != BackUpName && BackUpName != "")
            {
                ////////////////////////////////////////////////// 汉化资源
                if (MenuId > MenuIdControlAssign.MenuFileGroupIdMin)
                {
                    //(new MenuFunciontDeleteAddUpdate()).updateDictionaryResources(TargetModel, _MenuId, DictionaryResourcesSetion.Name, Name, true);
                }
                else
                {
                    //非自定义资源 不允许修改名称
                    //需要修改再放开
                    Name = BackUpName;
                }

                BackUpName = Name;
            }
        }

        #region Observable propory

        /// <summary>
        /// 当前节点是否为系统当前选中节点
        /// </summary>
        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                }
            }
        }

        /// <summary>
        /// 当前节点是否展开
        /// </summary>
        private bool _isExpanded;

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    if (_fatherMenu != null) _fatherMenu.IsExpanded = true;

                    this.RaisePropertyChanged(() => this.IsExpanded);
                }
            }
        }

        /// <summary>
        /// 菜单Id 与数据库同步
        /// 菜单Id修改  是引发所有数据更新的导引
        /// id修改后需要同步修改显示的菜单名称
        /// 需要同步加载本菜单下的子菜单
        /// </summary>
        private int _menuId;

        public int MenuId
        {
            get { return _menuId; }
            set
            {
                if (_menuId != value)
                {
                    _menuId = value;
                    this.RaisePropertyChanged(() => this.MenuId);

                    //if (_menuId <= DataHolding.MenuIdAssign.MenuIdMax && _menuId >= DataHolding.MenuIdAssign.MenuIdMin)
                    //    //menu id
                    //{
                    //    var t = MenuComponentHolding.GetMenuItemById(_menuId);
                    //    if (t != null)
                    //    {
                    //        Name = t.Text;
                    //    }

                    //    //var eng = DataHolding.EngHolding.GetEngValue(_menuId);
                    //    var eng = CETC50_Core.I36N.I36N.ConvertByCoding(_menuId.ToString());//
                    //    if (!string .IsNullOrEmpty( eng ))
                    //    {
                    //        Name = eng;
                    //    }
                    //}
                    if (_menuId <= MenuIdControlAssign.MenuFileGroupIdMax &&
                        _menuId >= MenuIdControlAssign.MenuFileGroupIdMin) //file
                    {

                        this.LoadChildren();
                    }
                }
            }
        }

        ///// <summary>
        ///// 本菜单在菜单中的排序位置
        ///// 当第一次修改时 通过拖动来并未进行排序处理，所有同级菜单的排序位置均为0
        ///// 需要手动移动一下菜单位置方可实现排序
        ///// </summary>
        //private int _intSortIndex;

        //public int SortIndex
        //{
        //    get { return _intSortIndex; }
        //    set
        //    {
        //        if (_intSortIndex != value)
        //        {
        //            _intSortIndex = value;
        //            this.RaisePropertyChanged(() => this.SortIndex);
        //        }
        //    }
        //}

        /// <summary>
        /// 菜单名称
        /// </summary>
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        /// <summary>
        /// 本菜单属于
        /// </summary>
        private int _instanceId;

        public int InstanceId
        {
            get { return _instanceId; }
            set
            {
                if (_instanceId != value)
                {
                    _instanceId = value;
                    this.RaisePropertyChanged(() => this.InstanceId);
                }
            }
        }

        /// <summary>
        /// 父菜单  跟节点父菜单为null
        /// </summary>
        private MenuTreeItemViewModel _fatherMenu;

        public MenuTreeItemViewModel FatherMenu
        {
            get { return _fatherMenu; }
            set { _fatherMenu = value; }
        }



        private ContextMenu _cm;

        public ContextMenu Cm
        {
            get
            {
                _cm = new ContextMenu();
                foreach (var t in Ccm)
                {
                    var fff = new MenuItem();
                    fff.Header = t.Text;
                    fff.ToolTip = t.Tooltips;
                    fff.Command = t.Command;

                    _cm.Items.Add(fff);
                }

                return _cm;
            }
        }


        private ObservableCollection<MenuItemBase> _ccm;

        /// <summary>
        /// 右键菜单
        /// </summary>
        private ObservableCollection<MenuItemBase> Ccm
        {
            get
            {
                if (_ccm == null)
                {
                    _ccm = new ObservableCollection<MenuItemBase>();
                }

                _ccm.Clear();
                if (MenuId == 0)
                {
                    _ccm.Add(AddFolder);
                }
                else if (MenuId >= MenuIdControlAssign.MenuFileGroupIdMin &&
                         MenuId <= MenuIdControlAssign.MenuFileGroupIdMax)
                {
                    _ccm.Add(AddFolder);
                    _ccm.Add(DeleteFolder);
                    _ccm.Add(EditFolder);
                }
                else if (MenuId >= MenuIdControlAssign.MenuIdMin && MenuId < MenuIdControlAssign.MenuIdMax)
                {
                    _ccm.Add(DeleteFolder);
                }

                return _ccm;
            }
        }


        #endregion

        #region EditFolder

        private MenuItemBase _editFolder;

        private MenuItemBase EditFolder
        {
            get
            {

                _editFolder = new MenuItemBase();
                _editFolder.Tooltips = "Edit a folder";
                _editFolder.Text = "Edit";
                _editFolder.Command = new RelayCommand(ExEditFolderFolder);
                _editFolder.IsCheckable = false;
                _editFolder.IsEnabled = true;
                return _editFolder;
            }
        }

        private void ExEditFolderFolder()
        {
            if (MenuId >= MenuIdControlAssign.MenuFileGroupIdMin &&
                MenuId <= MenuIdControlAssign.MenuFileGroupIdMax)
            {
                StartEditName();
            }
        }

        #endregion

        #region AddFolder

        private MenuItemBase _addFolder;

        private MenuItemBase AddFolder
        {
            get
            {

                _addFolder = new MenuItemBase();
                _addFolder.Tooltips = "Add a folder";
                _addFolder.Text = "Add";
                _addFolder.Command = new RelayCommand(ExAddFolderFolder);
                _addFolder.IsCheckable = false;
                _addFolder.IsEnabled = true;

                return _addFolder;
            }
        }

        private void ExAddFolderFolder()
        {
            if (MenuId == 0 ||
                (MenuId >= MenuIdControlAssign.MenuFileGroupIdMin &&
                 MenuId <= MenuIdControlAssign.MenuFileGroupIdMax))
            {
                int intAvlId = ServerInstanceRelation.GetMaxAviableMenuFileId();
                ServerInstanceRelation.AddMenuInstanceRelation(MenuId, intAvlId, 0, "New File", this.InstanceId);
                ChildTreeItems.Add(new MenuTreeItemViewModel(this, this.InstanceId, intAvlId) {Name = "New File"});
                this.SetSelectNode(MenuId, intAvlId);
            }
        }

        #endregion

        #region DeleteFolder

        private MenuItemBase _deleteFolder;

        private MenuItemBase DeleteFolder
        {
            get
            {

                _deleteFolder = new MenuItemBase();
                _deleteFolder.Tooltips = "Delete a folder";
                _deleteFolder.Text = "Delete";
                _deleteFolder.Command = new RelayCommand(ExDeleteFolderFolder);
                _deleteFolder.IsCheckable = false;
                _deleteFolder.IsEnabled = true;

                return _deleteFolder;
            }
        }

        private void ExDeleteFolderFolder()
        {
            if (MenuId == 0) return;
            //var fff = new DataHoldingExtend.MenuInstanceRelationHoldingExtend();

            //try
            //{
            //    if (MenuId >= DataHolding.MenuIdAssign.MenuFileGroupIdMin &&
            //        MenuId <= DataHolding.MenuIdAssign.MenuFileGroupIdMax)
            //    {
            //        fff.DeleteMenuInstanceRelation(this.InstanceId, MenuId);
            //    }
            //    else
            //    {
            //        if (this.FatherMenu != null)
            //        {
            //            int fatherId = FatherMenu.MenuId;
            //            fff.DeleteMenuInstanceRelation(this.InstanceId, fatherId, MenuId);
            //        }
            //    }
            //}
            //catch (Exception e){}
            if (this.FatherMenu != null)
                if (this.FatherMenu.ChildTreeItems.Contains(this))
                {
                    this.FatherMenu.ChildTreeItems.Remove(this);
                }
        }

        #endregion

        public void LoadChildren()
        {
            ChildTreeItems.Clear();
            if (MenuId == 0 ||
                (MenuId >= MenuIdControlAssign.MenuFileGroupIdMin &&
                 MenuId <= MenuIdControlAssign.MenuFileGroupIdMax))
            {
                var f = ServerInstanceRelation.GetInstanceRelationsByfatherId(this.InstanceId, MenuId);
                foreach (var t in f)
                {
                    if (t.Id >= MenuIdControlAssign.MenuIdMin && t.Id <= MenuIdControlAssign.MenuIdMax)
                    {
                        // var eng = I36N.Services.I36N.ConvertByCoding(t.Id.ToString());//
                        string name = t.Name;
                        //if (!eng.Equals("Missing")) name = eng;
                        var tt = MenuComponentHolding.GetMenuItemById(t.Id);
                        if (tt == null) continue;
                        ChildTreeItems.Add(new MenuTreeItemViewModel(this, this.InstanceId, t.Id) {Name = name});
                    }
                    else
                    {
                        ChildTreeItems.Add(new MenuTreeItemViewModel(this, this.InstanceId, t.Id) {Name = t.Name});
                    }
                }
            }
        }

        public MenuTreeItemViewModel GetSelectMvvm()
        {
            if (_isSelected)
            {
                return this;
            }
            else
            {
                if ((MenuId >= MenuIdControlAssign.MenuFileGroupIdMin &&
                     MenuId <= MenuIdControlAssign.MenuFileGroupIdMax) || MenuId == 0)
                {
                    for (int i = 0; i < ChildTreeItems.Count; i++)
                    {
                        if (ChildTreeItems[i] != null)
                        {
                            if (ChildTreeItems[i].GetSelectMvvm() != null)
                            {
                                return ChildTreeItems[i].GetSelectMvvm();
                            }
                        }
                    }
                }
            }

            return null;
        }

        public void SetSelectNode(int intfatherId, int intMenuid)
        {
            if (FatherMenu != null)
            {
                if (FatherMenu.MenuId == intfatherId && MenuId == intMenuid)
                {
                    this.IsSelected = true;
                    this.IsExpanded = true;
                }
                else
                {
                    if ((MenuId >= MenuIdControlAssign.MenuFileGroupIdMin &&
                         MenuId <= MenuIdControlAssign.MenuFileGroupIdMax) || MenuId == 0)
                    {
                        for (int i = 0; i < ChildTreeItems.Count; i++)
                        {
                            if (ChildTreeItems[i] != null)
                            {
                                ChildTreeItems[i].SetSelectNode(intfatherId, intMenuid);
                            }
                        }
                    }
                }
            }
        }
    }
}