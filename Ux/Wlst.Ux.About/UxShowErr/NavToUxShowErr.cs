using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Ux.About.UxShowErr
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToUxStatistics : MenuItemBase
    {
        public NavToUxStatistics()
        {
            Id = Wlst.Ux.About.Services.MenuIdAssgin.NavToShowErrViewModelMainId;
            Text = "故障细节";
            Tag = "故障细节";
            Classic = "主菜单";
            Description = "故障细节，ID 为" + Wlst.Ux.About.Services.MenuIdAssgin.NavToShowErrViewModelMainId;
            Tooltips = "故障细节";
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
            ExNavWithArgs(Wlst.Ux.About.Services.ViewIdAssign.UxShowErrViewId, 0);
        }
    }
}
