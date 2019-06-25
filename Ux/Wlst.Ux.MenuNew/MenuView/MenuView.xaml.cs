using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using Microsoft.Windows.Controls.Ribbon;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using Wlst.client;
using EventIdAssign = Wlst.Cr.CoreOne.CoreIdAssign.EventIdAssign;

namespace Wlst.Ux.MenuNew.MenuView
{
    /// <summary>
    /// MenuView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(  Services.ViewIdAssign.MenuViewId,AttachNow = true,
       AttachRegion = Services.ViewIdAssign.MenuViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
            InitEvent();
            InitAction();
        }
    }


    public partial class MenuView
    {
        
        private void ResetM()
        {

            var fff = MenuBuilding.BulidCm(MainMenuDefine.MainMenuKey, true, null);
            //this.HelpCmm(fff);
            //this.Helpcmmm(fff);
            //this.RaisePropertyChanged(() => this.Items);

            Ribbon.Items.Clear();
            foreach (var f in fff)
            {
                if (f.CmItems .Count > 0)
                {
                    RibbonTab rt = new RibbonTab();
                    rt.Header = f.Text;
                    
                    rt.Foreground = new SolidColorBrush(Colors.Black);

                    rt.BorderThickness = new Thickness(0);
                  //  rt.Foreground = new SolidColorBrush(Colors.Black);

                    AddItems(f, ref rt);
                    if (rt.Items.Count > 0) Ribbon.Items.Add(rt);
                }
            }

            //RibbonTabHeaderItemsControl fx;fx.ItemContainerStyle  
        }

        private void AddItems(Cr.CoreOne.CoreInterface.IIMenuItem datax, ref RibbonTab rb)
        {
            RibbonGroup rg = new RibbonGroup();
            //      rg.Header = data.Text;
            var rtn = AddItems(datax);
            foreach (var data in rtn)
            {
                if (data.Visibility != Visibility.Visible) continue ;

                RibbonButton rbf = new RibbonButton();
                rbf.Label = data.Text;
                rbf.Command = data.Command;
                var npt = Wlst.Cr.CoreOne.Services.ImageSourceHelper.MySelf.GetImageSourceById(data.Id);
                if (npt == null) npt = ImageSourceHelper.MySelf.GetImageSourceById(101);
                rbf.LargeImageSource = npt;
                rg.Items.Add(rbf);
            }
            if (rg.Items.Count > 0) rb.Items.Add(rg);
        }

        private List<Cr.CoreOne.CoreInterface.IIMenuItem> AddItems(Cr.CoreOne.CoreInterface.IIMenuItem data)
        {
            var rtn = new List<Cr.CoreOne.CoreInterface.IIMenuItem>();

            if (data.CmItems .Count > 0)
            {
                foreach (var f in data.CmItems)
                {
                    if (f.CmItems.Count > 0) rtn.AddRange(AddItems(f));
                    else rtn.Add(f);
                }

            }

            return rtn;
        }

        //private List<Cr.CoreOne.CoreInterface.IIMenuItem> AddItems(Cr.CoreOne.CoreInterface.IIMenuItem data)
        //{
        //    var rtn = new List<Cr.CoreOne.CoreInterface.IIMenuItem>();

        //    if (data.Items.Count > 0)
        //    {
        //       foreach (var f in data .Items )
        //       {
        //           if(f.Items .Count >0) rtn.AddRange(AddItems(f));
        //           else rtn .Add()
        //       }
        //        rtn .AddRange( AddItems( ))
        //    }
        //    else
        //    {
        //        if ( data.Visibility != Visibility.Visible) return;

        //        RibbonButton rbf = new RibbonButton();
        //        rbf.Label = data.Text;
        //        rbf.Command = data.Command;
        //        if (iisNext)
        //            rbf.SmallImageSource = ImageSourceHelper.MySelf.GetImageSourceById(101);
        //        else
        //            rbf.LargeImageSource = ImageSourceHelper.MySelf.GetImageSourceById(101);
        //        rb.Items.Add(rbf);
        //    }
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

     

        public void  InitEvent()
        {
            
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler, FundOrderFilter);
        }

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(Wlst.Sr.ProtocolPhone.LxLogin.wst_add_or_update_user,
                                          OnAddOrUpdateUser, typeof(MenuView), this);
        }

        public void OnAddOrUpdateUser(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstLoginAddOrUpdateUserInfo;
            if (info == null) return;

            if (info.Op == 2 && info.SvrRtnModified )
            {
                if (info.UserInfo.UserName.Equals(Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName))
                    ResetM();
            }
        }

        #region EventSubScriptionTokener Subscription

        /// <summary>
        /// 不管三七二十一 直接重绘主菜单
        /// </summary>
        /// <param name="args"></param>
        public void FundEventHandler(PublishEventArgs args)
        {
            ResetM();
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
            }
            return false;
        }

        #endregion

        public static void PublishResetMenuEvent()
        {
            EventPublish.PublishEvent(new PublishEventArgs()
            {
                EventType = PublishEventType.Core,
                EventId =
                    Wlst.Sr.Menu.Services.EventIdAssign.MenuInstanceRelationLoadUpdate
            });
        }
    }
}
