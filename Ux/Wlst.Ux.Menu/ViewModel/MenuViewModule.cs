using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Reflection;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Menu.Services;
using EventIdAssign = Wlst.Cr.CoreOne.CoreIdAssign.EventIdAssign;

namespace Wlst.Ux.Menu.ViewModel
{
    [Export(typeof (IIMenuViewModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MenuViewModule :Wlst .Cr.Core .CoreServices .ObservableObject , IIMenuViewModule
    {
        //private EventSubScriptionTokener _eventSubScriptionTokener;

        private ObservableCollection<MenuItem> _m;

        public ObservableCollection<MenuItem> Items
        {
            get
            {
                if (_m == null) _m = new ObservableCollection<MenuItem>();
                return _m;
            }
        }

        
        public  ObservableCollection<IIMenuItem> IimenuItems=new ObservableCollection<IIMenuItem>( );

        private void ResetM()
        {
            Items.Clear();
            IimenuItems.Clear();
            var fff = MenuBuilding.BulidCm(MainMenuDefine.MainMenuKey, true, null);
            //this.HelpCmm(fff);
            this.Helpcmmm(fff);
            this.RaisePropertyChanged(() => this.Items);
        }

        protected void Helpcmmm(ObservableCollection<IIMenuItem> t)
        {
            var fs = MenuBuilding.HelpCmm(t,true );
            Items.Clear();
            foreach (var g in fs)
            {
                Items.Add(g);
            }
        }

     

        public MenuViewModule()
        {
            
            EventPublisher.AddEventSubScriptionTokener(
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);
        }

       

        #region EventSubScriptionTokener Subscription

        /// <summary>
        /// 不管三七二十一 直接重绘主菜单
        /// </summary>
        /// <param name="args"></param>
        public void FundEventHandler(PublishEventArgs args)
        {
            if(args .EventType ==PublishEventType .SvAv )
            {
                ResetM();
            }
            else
            {
                ResetM();
            }
            
            //if (PropertyChanged != null)
            //{
            //    this.PropertyChanged(this, new PropertyChangedEventArgs("Items"));
            //}
        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {

            if (args.EventType == PublishEventType.SvAv)
            {
                return true;
            }
            if (args.EventType == PublishEventType.Core)
            {
                if (args.EventId ==EventIdAssign  .MenuComponentAdd ||
                    args.EventId == EventIdAssign.MenuComponentDelete)
                {
                    return true;
                }
                if (args.EventId == Wlst.Sr.Menu.Services.EventIdAssign.MenuInstanceRelationLoadUpdate
                    )
                {
                    return true;
                }
                if (args.EventId == Wlst.Sr.Menu.Services.EventIdAssign.MenuShourtCutsLoadUpdate
                    || args.EventId == Wlst.Sr.Menu.Services.EventIdAssign.MenuShourtCutsUpdate
                    )
                {
                    return true;
                }
                if (args.EventId == Wlst.Sr.Menu.Services.EventIdAssign.MenuInstanceRelationUpdate)
                {
                    if (args.GetParams().Count == 2)
                    {
                        if (args.GetParams()[1].ToString().Contains(MainMenuDefine.MainMenuKey))
                        {
                            return true;
                        }
                    }
                }
                if (args.EventId == Wlst.Sr.PrivilegesCrl.Services.EventIdAssign.RequestOrUpdateUserPrivilegInfoId)
                {
                    return true;
                }
                if (args.EventId == Wlst.Sr.PrivilegesCrl.Services.EventIdAssign.ModflyUserInfomationId)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        public static void PublishResetMenuEvent()
        {
            EventPublisher.EventPublish(new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.Menu.Services.EventIdAssign.MenuInstanceRelationLoadUpdate
            });
        }
    }
}