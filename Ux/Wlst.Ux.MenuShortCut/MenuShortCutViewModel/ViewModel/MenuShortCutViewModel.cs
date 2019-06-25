using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Input;


using Wlst.Cr.Core.ComponentHold;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.ComponentHold;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.MenuShortCut.MenuShortCutViewModel.Services;

namespace Wlst.Ux.MenuShortCut.MenuShortCutViewModel.ViewModel
{


    [Export(typeof (IIMenuShortCutViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MenuShortCutViewModel : ObservableObject, IIMenuShortCutViewModel
    {

        public MenuShortCutViewModel()
        {
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);
            this.NavOnLoad();
            _bolCanSaveAll = true;
        }

        
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
            get { return "快捷键设置"; }
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


        #region IEventAggregator Subscription

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {
            try
            {
                int id = Convert.ToInt32(args.GetParams()[0]);
                if (id > 0)
                {
                    MenuId = id;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            return args.EventType == PublishEventLocal.Name && args.EventId == PublishEventLocal.ShortCutSelectChange;
        }

        #endregion


        private ObservableCollection<ShortCutTreeItemViewModel> _childTreeItems;

        public ObservableCollection<ShortCutTreeItemViewModel> ChildTreeItems
        {
            get
            {
                if (_childTreeItems == null) _childTreeItems = new ObservableCollection<ShortCutTreeItemViewModel>();
                return _childTreeItems;
            }
        }


        public void NavOnLoad(params object[] parsObjects)
        {
            LoadChildren();
        }

        private void LoadChildren()
        {
            //ChildTreeItems.Clear();
         
            //var t =
            //    ServerInstanceRoot.GetInstanceByKey( 
            //        MainMenuDefine.MainMenuKey);


            //if (t == null) return;
            //string name = t.Name;
            //ChildTreeItems.Add(new ShortCutTreeItemViewModel(null, t.Id , 0)
            //                       {
            //                           Name = name
            //                       });
            //foreach (var f in this.ChildTreeItems) f.LoadChildren();



            ChildTreeItems.Clear();
            var fff = MenuBuilding.BulidCm(MainMenuDefine.MainMenuKey, true, null);
            foreach (var g in fff)
            {

                if (g.Visibility != Visibility.Visible) continue;
                var tmp = new ShortCutTreeItemViewModel(null, g.Id, 0)
                              {
                                  Name = g.Text
                              };
                tmp.LoadChildren(g.CmItems , g.Id);
                if (tmp.ChildTreeItems.Count > 0)
                    ChildTreeItems.Add(tmp);

            }


            //    public  ObservableCollection<IIMenuItem> IimenuItems=new ObservableCollection<IIMenuItem>( );

        //private void ResetM()
        //{
        //    Items.Clear();
        //    IimenuItems.Clear();
        //    var fff = MenuBuilding.BulidCm(MainMenuDefine.MainMenuKey, true, null);
        //    //this.HelpCmm(fff);
        //    this.Helpcmmm(fff);
        //    this.RaisePropertyChanged(() => this.Items);
        //}

        //protected void Helpcmmm(ObservableCollection<IIMenuItem> t)
        //{
        //    var fs = MenuBuilding.HelpCmm(t,true );
        //    Items.Clear();
        //    foreach (var g in fs)
        //    {
        //        Items.Add(g);
        //    }
        //}
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
                    this.UpdateAttri();
                }
            }
        }


        private void UpdateAttri()
        {
            if (MenuId >= MenuIdControlAssign.MenuIdMin && MenuId <= MenuIdControlAssign.MenuIdMax)
            {
                Warning = "请使用键盘设置快捷键......";

                var menu = MenuComponentHolding.GetMenuItemById(MenuId);
                if (menu != null)
                {
                    Name = menu.Text;
                    Tooltips = menu.Tooltips;
                }

               // var eng = DataHolding.EngHolding.GetEngValue(MenuId);
                //var eng = I36N.Services.I36N.ConvertByCoding(MenuId.ToString());//

              //  if (!string.IsNullOrEmpty(eng) && !eng.Equals("Missing"))
              //          Name = eng;
                    //if (!string.IsNullOrEmpty(eng.Item2))
                    //    Tooltips = eng.Item2;
                
                var shortcut =ServicesShortCuts.GetShortCutValue(MenuId);
                if (string.IsNullOrEmpty(shortcut))
                {
                    shortcut = "";
                }
                ShortCuts = shortcut;
                _backUpShortCuts = shortcut;
                return;
            }
            Warning = "菜单夹不允许设置快捷键!!!!!!";
            Tooltips = "";
            Name = "菜单夹不允许设置快捷键!!!!!!";
            ShortCuts = "";
        }

        private string _warning;

        public string Warning
        {
            get { return _warning; }
            set
            {
                if (_warning != value)
                {
                    _warning = value;
                    this.RaisePropertyChanged(() => this.Warning);
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

        private string _tooltips;

        public string Tooltips
        {
            get { return _tooltips; }
            set
            {
                if (_tooltips != value)
                {
                    _tooltips = value;
                    this.RaisePropertyChanged(() => this.Tooltips);
                }
            }
        }


        private string _backUpShortCuts;
        private string _shortCuts;

        public string ShortCuts
        {
            get { return _shortCuts; }
            set
            {
                if (_shortCuts != value)
                {
                    _shortCuts = value;
                    this.RaisePropertyChanged(() => this.ShortCuts);
                }
            }
        }

        private DateTime _dtOk;
        private DateTime _dtSave;
        #region save menuid
        private ICommand _cmdOk;

        public ICommand CmdOk
        {
            get
            {
                if (_cmdOk == null) _cmdOk = new RelayCommand(Ex, CanEx,false );
                return _cmdOk;
            }
        }

        protected void Ex() //保存快捷键
        {
            _dtOk = DateTime.Now;
            //var fff = new DataHoldingExtend.MenuShortCutsHoldingExtend();
            //fff.AddShortCut(MenuId, ShortCuts);
            //fff.WriteUpdateDb(MenuId);

            ServicesShortCuts.UpdateShortCut(MenuId, ShortCuts);
            _backUpShortCuts = ShortCuts;
        }

        protected bool CanEx()
        {
            if (_backUpShortCuts != ShortCuts && MenuId >= MenuIdControlAssign .MenuIdMin &&
                MenuId <= MenuIdControlAssign.MenuIdMax && DateTime.Now.Ticks-_dtOk.Ticks>30000000)
            {
                _bolCanSaveAll = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region save menuid
        private ICommand _cmdclean;

        public ICommand CmdClean
        {
            get
            {
                if (_cmdclean == null) _cmdclean = new RelayCommand(ExCmdClean, CanExCmdClean, false);
                return _cmdclean;
            }
        }

        protected void ExCmdClean() //保存快捷键
        {
            _bolCanSaveAll = true;
            ServicesShortCuts.UpdateShortCut(MenuId, "");
            _backUpShortCuts = "";
            ShortCuts = "";
            this.UpdateAttri();
        }

        protected bool CanExCmdClean()
        {
            if (!string.IsNullOrEmpty(_backUpShortCuts ) && MenuId >= MenuIdControlAssign.MenuIdMin &&
                MenuId <= MenuIdControlAssign.MenuIdMax && DateTime.Now.Ticks - _dtOk.Ticks > 30000000)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion


        #region save all
        private ICommand _cmdSaveAll;

        public ICommand CmdSaveAll
        {
            get
            {
                if (_cmdSaveAll == null) _cmdSaveAll = new RelayCommand(ExSaveAll, CanExSaveAll,false );
                return _cmdSaveAll;
            }
        }

        protected void ExSaveAll() //保存快捷键
        {
            _dtSave = DateTime.Now;
            _bolCanSaveAll = false;
            //var fff = new DataHoldingExtend.MenuShortCutsHoldingExtend();
            //fff.AddShortCut(MenuId, ShortCuts);
            //fff.WriteUpdateDb();

            ServicesShortCuts.UpdateShortCut(MenuId, ShortCuts);
            
            _backUpShortCuts = ShortCuts;

            //EventPublish.PublishEvent(new PublishEventArgs()
            //{
            //    EventId = EventIdAssign.MainMenuInstanceUpdateId,
            //    EventType = PublishEventType.Core,
            //});
        }

        private bool _bolCanSaveAll = true;
        protected bool CanExSaveAll()
        {
            return _bolCanSaveAll && DateTime.Now.Ticks-_dtSave.Ticks>30000000;
        }
        #endregion
    }

}
