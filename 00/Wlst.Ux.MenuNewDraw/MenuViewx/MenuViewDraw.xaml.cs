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
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.Behavior;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.Services;
using Wlst.Sr.Menu.Services;

namespace Wlst.Ux.MenuNewDraw.MenuViewx
{
    /// <summary>
    /// MenuViewDraw.xaml 的交互逻辑
    /// </summary>
 
    [ViewExport(AttachNow = true, ID = Services.ViewIdAssign.MenuViewId,
   AttachRegion = Services.ViewIdAssign.MenuViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MenuViewDraw : UserControl
    {
        public MenuViewDraw()
        {
            InitializeComponent();
            InitEvent();
        }
    }


    public partial class MenuViewDraw
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
                if (f.Items.Count > 0)
                {
                    MenuItem rt = new MenuItem();
                    rt.Header = f.Text;

                    var npt = ImageSourceHelper.MySelf.GetBitmapImageById(f.Id);
                    if (npt == null) npt = ImageSourceHelper.MySelf.GetBitmapImageById(101);
                    if (npt != null)
                    {
                        rt.Icon = new Image {Source = npt};
                    }

                    AddItems(f, ref rt);
                    if (rt.Items.Count > 0) Ribbon.Items.Add(rt);
                }
            }
        }

        private void AddItems(Cr.CoreOne.CoreInterface.IIMenuItem datax, ref MenuItem rb)
        {

            var rtn = AddItems(datax);
            foreach (var data in rtn)
            {
                if (data.Visibility != Visibility.Visible) continue;

                MenuItem rbf = new MenuItem();
                rbf.Header = data.Text;
                rbf.Command = data.Command;


                var npt = ImageSourceHelper.MySelf.GetBitmapImageById( data.Id);
                if (npt == null) npt = ImageSourceHelper.MySelf.GetBitmapImageById(101);
                if (npt != null)
                {
                    rbf.Icon = new Image { Source = npt  };
                }
                rb.Items.Add(rbf);
            }

        }

        private List<Cr.CoreOne.CoreInterface.IIMenuItem> AddItems(Cr.CoreOne.CoreInterface.IIMenuItem data)
        {
            var rtn = new List<Cr.CoreOne.CoreInterface.IIMenuItem>();

            if (data.Items.Count > 0)
            {
                foreach (var f in data.Items)
                {
                    if (f.Items.Count > 0) rtn.AddRange(AddItems(f));
                    else rtn.Add(f);
                }

            }

            return rtn;
        }

     


        public void InitEvent()
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
            if (args.EventType == PublishEventType.SvAv)
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
                if (args.EventId ==Wlst .Cr .CoreOne .CoreIdAssign  . EventIdAssign.MenuComponentAdd ||
                    args.EventId == Wlst.Cr.CoreOne.CoreIdAssign.EventIdAssign.MenuComponentDelete)
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
