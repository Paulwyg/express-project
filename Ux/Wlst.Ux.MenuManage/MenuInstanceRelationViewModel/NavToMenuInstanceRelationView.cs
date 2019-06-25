using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.MenuManage.Services;

namespace Wlst.Ux.MenuManage.MenuInstanceRelationViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToMenuInstanceRelationView : MenuItemBase
    {
        public NavToMenuInstanceRelationView()
        {
            Id = MenuIdAssgin.NavToMenuInstanceRelationViewId;
            Text = "设置菜单实例";
            Tag = "设置菜单实例";
            this.Classic = "主菜单";
            Description = "设置菜单实例，ID 为" + MenuIdAssgin.NavToMenuInstanceRelationViewId;
            Tooltips = "设置菜单实例";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex );
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return true;
        }

        protected void Ex()
        {
            this.ExNavWithArgs( ViewIdAssign.MenuInstanceRelationViewId ,
                               1);
        }

    }
}
