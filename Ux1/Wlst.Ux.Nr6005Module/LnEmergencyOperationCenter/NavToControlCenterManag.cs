using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Nr6005Module.LnEmergencyOperationCenter
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToControlCenterManag : MenuItemBase
    {
        public NavToControlCenterManag()
        {
            Id = Wlst.Ux.Nr6005Module.Services.MenuIdAssgin.NavToLnEmergencyOperationCenter;
            Text = "应急操作中心";
            Tag = "应急操作中心";
            Classic = "主菜单";
            Description = "应急操作中心，ID 为" + Nr6005Module.Services.MenuIdAssgin.NavToLnEmergencyOperationCenter;
            Tooltips = "应急操作中心";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
         //   IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX.Count > 0;
            return false;
        }
        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            ExNavWithArgs(Nr6005Module.Services.ViewIdAssign.NavToLnEmergencyOperationCenterViewId,
                               1);
        }
    }
}
