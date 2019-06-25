using System;
using System.Collections.ObjectModel;
using System.Windows;


using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.ComponentHold;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.MenuShortCut.MenuShortCutViewModel.Services;

namespace Wlst.Ux.MenuShortCut.MenuShortCutViewModel.ViewModel
{
    public class ShortCutTreeItemViewModel : ObservableObject
    {

        public ShortCutTreeItemViewModel(ShortCutTreeItemViewModel mvvmFather, int instanceId,int menuId)
        {
            this.FatherMenu = mvvmFather;
            this.InstanceId = instanceId;
            this.MenuId = menuId;
        }


        private ObservableCollection<ShortCutTreeItemViewModel> _childTreeItems;

        public ObservableCollection<ShortCutTreeItemViewModel> ChildTreeItems
        {
            get
            {
                if (_childTreeItems == null)
                    _childTreeItems = new ObservableCollection<ShortCutTreeItemViewModel>();
                return _childTreeItems;
            }
        }




        #region Observable propory


        public int InstanceId;
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
                    if (_isSelected)
                    {
                        //发布事件 
                        var args = new PublishEventArgs()
                                       {
                                           EventType = PublishEventLocal.Name,
                                           EventId = PublishEventLocal.ShortCutSelectChange

                                       };
                        args.AddParams(MenuId);
                        EventPublish.PublishEvent(args);
                    }
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


                    if (_menuId <= MenuIdControlAssign.MenuIdMax && _menuId >= MenuIdControlAssign.MenuIdMin)
                        //menu id
                    {
                        var t = MenuComponentHolding.GetMenuItemById(_menuId);
                        if (t != null)
                        {
                            Name = t.Text;
                        }
                        else
                        {
                            if (FatherMenu != null)
                            {
                                try
                                {
                                    this.FatherMenu.ChildTreeItems.Remove(this);

                                }
                                catch (Exception ex)
                                {
                                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                                }
                                return;
                            }
                        }
                       // var eng = I36N.Services.I36N  .ConvertByCoding(_menuId.ToString());//
                       // var eng = DataHolding.EngHolding.GetEngValue(_menuId);
                        //if (!string.IsNullOrEmpty(eng) && !eng.Equals("Missing"))
                        //{
                        //    Name = eng;
                        //}
                    }
                    else if (_menuId <= MenuIdControlAssign.MenuFileGroupIdMax &&
                             _menuId >= MenuIdControlAssign.MenuFileGroupIdMin) //file
                    {

                        this.LoadChildren();
                    }
                }
            }
        }


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


        private ShortCutTreeItemViewModel _fatherMenu;
        public ShortCutTreeItemViewModel FatherMenu
        {
            get { return _fatherMenu; }
            set { _fatherMenu = value; }
        }



        #endregion


        public void LoadChildren()
        {
            ChildTreeItems.Clear();

            if (_menuId <= MenuIdControlAssign.MenuIdMax && _menuId >= MenuIdControlAssign.MenuIdMin)
            {
                return;
            }
            else if (MenuId == 0 || (_menuId <= MenuIdControlAssign.MenuFileGroupIdMax &&
                     _menuId >= MenuIdControlAssign.MenuFileGroupIdMin)) //file
            {

                var f = ServerInstanceRelation.GetInstanceRelationsByfatherId(this.InstanceId, MenuId);
                foreach (var t in f)
                {

                    if (t.Id <= MenuIdControlAssign.MenuIdMax && t.Id >= MenuIdControlAssign.MenuIdMin)
                    //menu id
                    {
                        var fff = MenuComponentHolding.GetMenuItemById(t.Id);
                        if (fff != null)
                        {
                            ChildTreeItems.Add(new ShortCutTreeItemViewModel(this, this.InstanceId, t.Id) { Name = t.Name });
                        }
                    }
                    else
                    {
                        ChildTreeItems.Add(new ShortCutTreeItemViewModel(this, this.InstanceId, t.Id) { Name = t.Name });
                    }
                }
            }
        }

        public void LoadChildren(ObservableCollection<IIMenuItem> lst, int _menuId)
        {
            ChildTreeItems.Clear();

            if (_menuId <= MenuIdControlAssign.MenuFileGroupIdMin) //File)
            {
                return;
            }

            foreach (var t in lst)
            {

                if ( t.Id >= MenuIdControlAssign.MenuFileGroupIdMin) //File)
                //menu id
                {
                    if (t.Visibility != Visibility.Visible) continue;
                    var tmp = new ShortCutTreeItemViewModel(this, t.Id, 0)
                                  {
                                      Name = t.Text
                                  };
                    tmp.LoadChildren(t.CmItems , t.Id);
                    if (tmp.ChildTreeItems.Count > 0)
                        ChildTreeItems.Add(tmp);
                }
                else
                {
                    if (t.Visibility != Visibility.Visible) continue;
                    ChildTreeItems.Add(new ShortCutTreeItemViewModel(this, this.InstanceId, t.Id) { Name = t.Text });
                }
            }
            

        }
    }
}
