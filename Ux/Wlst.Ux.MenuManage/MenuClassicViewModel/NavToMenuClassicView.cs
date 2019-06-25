using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.MenuManage.Services;

namespace Wlst.Ux.MenuManage.MenuClassicViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToMenuClassicView : MenuItemBase
    {
        public NavToMenuClassicView()
        {
            Id = MenuIdAssgin.NavToMenuClassicViewId;
            Text = "设定模板菜单";
            Tag = "设定模板菜单";
            Description = "设定模板菜单，ID 为" + MenuIdAssgin.NavToMenuClassicViewId;
            Tooltips = "设定模板菜单";
            this.Classic = "主菜单";
            base.Command = new RelayCommand(Ex );
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return true;
        }

        protected void Ex()
        {
            this.ExNavWithArgs( ViewIdAssign.MenuClassicViewId,
                               1);
        }

    }
}
