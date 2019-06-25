using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.About.UxAbout
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToUxAbout : MenuItemBase
    {
        public NavToUxAbout()
        {
            Id = Wlst.Ux.About.Services.MenuIdAssgin.NavToUxAboutViewModelMainId;
            Text = "关于";
            Tag = "关于";
            Classic = "主菜单";
            Description = "关于，ID 为" + Wlst.Ux.About.Services.MenuIdAssgin.NavToUxAboutViewModelMainId;
            Tooltips = "关于";
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
            ExNavWithArgs(About.Services.ViewIdAssign.UxAboutViewId, 0);
        }
    }
}
