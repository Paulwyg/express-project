using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Input;


using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.ComponentHold;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.MenuManage.MenuClassicViewModel.Services;
using EventIdAssign = Wlst.Cr.CoreOne.CoreIdAssign.EventIdAssign;

namespace Wlst.Ux.MenuManage.MenuClassicViewModel.ViewModel
{

    [Export(typeof (IIMenuClassicViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MenuClassicViewModel : ObservableObject, IIMenuClassicViewModel
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
            get { return "菜单类型管理"; }
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

        public MenuClassicViewModel()
        {
            CurrentClassicItem = new NameValueInt();
            this.OnInit();
            LoadItems();
        }

        public void OnUserHideOrClosing()
        {
            MenuClassicItems.Clear();
            MenusItems.Clear();
        }

        #region IIClassicManagement 成员

        private ObservableCollection<MenuItemforClassis> _menusItems;

        /// <summary>
        /// 所有菜单信息栏
        /// </summary>
        public ObservableCollection<MenuItemforClassis> MenusItems
        {
            get
            {
                if (_menusItems == null) _menusItems = new ObservableCollection<MenuItemforClassis>();
                return _menusItems;
            }
            set
            {
                if (value == _menusItems) return;
                _menusItems = value;
                this.RaisePropertyChanged(() => this.MenusItems);
            }
        }

        private ObservableCollection<NameValueInt> _menuClassicItems;

        public ObservableCollection<NameValueInt> MenuClassicItems
        {

            get
            {
                if (_menuClassicItems == null) _menuClassicItems = new ObservableCollection<NameValueInt>();
                return _menuClassicItems;
            }
            set
            {
                if (value == _menuClassicItems) return;
                _menuClassicItems = value;
                this.RaisePropertyChanged(() => this.MenuClassicItems);
            }
        }


        private NameValueInt _currentClassicItem;

        public NameValueInt CurrentClassicItem
        {
            get
            {
                if (_currentClassicItem == null) _currentClassicItem = new NameValueInt();
                return _currentClassicItem;
            }
            set
            {
                if (_currentClassicItem != value)
                {
                    _currentClassicItem = value;
                    this.RaisePropertyChanged(() => this.CurrentClassicItem);
                    OnSelectClassicItemChange();
                }
            }
        }

        private void OnSelectClassicItemChange()
        {
            foreach (var t in MenusItems)
            {
                t.Selected = false;
            }
            if (CurrentClassicItem == null) return;
            if (!ServerClassic.GetClassicDic.ContainsKey(CurrentClassicItem.Value)) return;
            //检测该母版包含的菜单
            foreach (var t in MenusItems)
            {
                if (ServerClassic.GetClassicDic[CurrentClassicItem.Value].Items.Contains(t.Id))
                {
                    t.Selected = true;
                }
            }
        }

        #endregion

        public void NavOnLoad(params object[] parsObjects)
        {
            LoadItems();
        }

        #region init load

        private void LoadItems()
        {
            ReLoadMenuItems();
            ReLoadMenuTypesItems();
        }

        /// <summary>
        /// 加载所有菜单部件信息
        /// </summary>
        private void ReLoadMenuItems()
        {
            //try
            //{
            //    MenusItems.Clear();
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}

            var tmp = new ObservableCollection<MenuItemforClassis>();
            foreach (var menuId in MenuComponentHolding.GetAllComponentIDs)
            {
                //var menuTarget = new MenuTargetTypeMenusListModeItem();
                var menuItem = MenuComponentHolding.GetMenuItemById(menuId);
                if (menuItem == null) continue;
                //var eng = DataHolding.EngHolding.GetEngValue(menuId);
              //  var eng = I36N.Services.I36N.ConvertByCoding(menuId.ToString()); //
                var menuItemIns = new MenuItemforClassis()
                                      {
                                          Text = menuItem.Text,
                                          Id = menuItem.Id,
                                          Description = menuItem.Description,
                                          Tooltips = menuItem.Tooltips,
                                          Selected = false,
                                          Classic =menuItem .Classic ,
                                      };


               // if (!string.IsNullOrEmpty(eng) && !eng.Contains( "issing")) menuItemIns.Text = eng;
                // if (!string.IsNullOrEmpty(eng.Item2)) menuItemIns.Tooltips = eng.Item2;


                tmp.Add(menuItemIns);
            }
            MenusItems = tmp;
        }

        /// <summary>
        /// 重新加载模板菜单类型
        /// </summary>
        private void ReLoadMenuTypesItems()
        {
            //try
            //{
            //    MenuClassicItems.Clear();
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}
            var tmp = new ObservableCollection<NameValueInt>();
            foreach (var t in ServerClassic.GetClassicDic)
            {
                tmp.Add(
                    new NameValueInt()
                        {
                            Value = t.Value.Id,
                            Name = t.Value.Name
                        });
            }
            MenuClassicItems = tmp;
            if (MenuClassicItems.Count > 0) CurrentClassicItem = MenuClassicItems[0];
        }

        #endregion

        private DateTime _dtAddEx;
        private DateTime _dtUpdateEx;
        private DateTime _dtDeleteEx;
        private DateTime _dtRefleshEx;
        #region add

        private RelayCommand _relayAddCommand;

        private void AddEx()
        {
            _dtAddEx = DateTime.Now;
            int intValueId = ServerClassic.GetMaxAviableClassicId();
            var menusTarget = new NameValueInt()
                                  {
                                      Name = "新模板",
                                      Value = intValueId
                                  };
            foreach (var t in MenuClassicItems)
            {
                if (t.Value == intValueId)
                {
                    CurrentClassicItem = t;
                    return;
                }
            }
            MenuClassicItems.Add(menusTarget);
            CurrentClassicItem = menusTarget;
        }
        private bool CanAddEx()
        {
            return DateTime.Now.Ticks - _dtAddEx.Ticks > 30000000;
        }
        public ICommand CmdAdd
        {
            get { return _relayAddCommand ?? (_relayAddCommand = new RelayCommand(AddEx,CanAddEx,true )); }
        }

        #endregion

        #region update

        private RelayCommand _relayUpdateCommand;

        private void UpdateEx()
        {
            _dtUpdateEx = DateTime.Now;
            if (CurrentClassicItem == null) return;
            var lst = new List<int>();
            foreach (var t in MenusItems)
            {
                if (t.Selected)
                {
                    lst.Add(t.Id);
                }
            }
            ServerClassic.UpdateMneu(CurrentClassicItem.Value, CurrentClassicItem.Name, lst);
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

        #region delete

        private RelayCommand _relayDeleteCommand;

        private bool CanDeleteEx()
        {
            return DateTime.Now.Ticks - _dtDeleteEx.Ticks > 30000000;
        }
        private void DeleteEx()
        {
            _dtDeleteEx = DateTime.Now;
            if (CurrentClassicItem == null) return;
            var result = MessageBox.Show("确认删除菜单类别: " + CurrentClassicItem.Name + " ?", "确认删除", MessageBoxButton.YesNoCancel);
            if(result ==MessageBoxResult.No ||result ==MessageBoxResult.Cancel )
            {
                return;
            }
            int id = CurrentClassicItem.Value;
            if (MenuClassicItems.Contains(CurrentClassicItem))
            {
                MenuClassicItems.Remove(CurrentClassicItem);
                if (MenuClassicItems.Count > 0) CurrentClassicItem = MenuClassicItems[0];
            }
            ServerClassic.DeleteMneu(id);
        }

        public ICommand CmdDelete
        {
            get { return _relayDeleteCommand ?? (_relayDeleteCommand = new RelayCommand(DeleteEx,CanDeleteEx,true )); }
        }

        #endregion


        #region CmdReflesh

        private RelayCommand _relayRefleshCommand;
        private bool CanRefleshEx()
        {
            return DateTime.Now.Ticks - _dtRefleshEx.Ticks > 30000000;
        }
        private void RefleshEx()
        {
            _dtRefleshEx = DateTime.Now;
            this.NavOnLoad();

            Services.ExportMenuToXml.WriteMenuToXml();
        }

        public ICommand CmdReflesh
        {
            get { return _relayRefleshCommand ?? (_relayRefleshCommand = new RelayCommand(RefleshEx ,CanRefleshEx,true)); }
        }

        #endregion


    }

    //events
    public partial class MenuClassicViewModel
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
            int currentselectitemid = -1;
            if(CurrentClassicItem !=null )
            {
                currentselectitemid = CurrentClassicItem.Value;
            }
            this.NavOnLoad();
            foreach (var t in MenuClassicItems )
            {
                if (t.Value == currentselectitemid)
                {
                    CurrentClassicItem = t;
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
                if (args.EventId == EventIdAssign.MenuComponentAdd)
                {
                    return true;
                }
                if (args.EventId == EventIdAssign.MenuComponentDelete)
                {
                    return true;
                }
                if (args.EventId == Wlst.Sr.Menu.Services.EventIdAssign.ClassicMenuLoadUpdate)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}
