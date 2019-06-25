using System.Windows;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.About.UxAbout.Views;
using Application = System.Windows.Forms.Application;

namespace Wlst.Ux.About.UxAbout
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWeb : MenuItemBase
    {
        public NavToWeb()
        {
            Id = Wlst.Ux.About.Services.MenuIdAssgin.NavToWebPageId;
            Text = "资产管理";
            Tag = "资产管理";
            Classic = "主菜单";
            Description = "资产管理，ID 为" + Wlst.Ux.About.Services.MenuIdAssgin.NavToWebPageId;
            Tooltips = "资产管理";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex,CanEx,true);

        }
        public override bool IsCanBeShowRwx()
        {
            return true;
        }

        private static bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            WebPager wb = new WebPager();
          wb.Show();
        }
    }
}
