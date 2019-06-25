using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManag
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToControlCenterManag : MenuItemBase
    {
        public NavToControlCenterManag()
        {
            Id = Wlst.Ux.EmergencyDispatch.Services.MenuIdAssgin.NavToControlCenterView;
            Text = "控制中心";
            Tag = "控制中心";
            Classic = "主菜单";
            Description = "控制中心，ID 为" + EmergencyDispatch.Services.MenuIdAssgin.NavToControlCenterView;
            Tooltips = "控制中心";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);

        }

        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            ExNavWithArgs(EmergencyDispatch.Services.ViewIdAssign.NatToControlCenterManagAttachRegion,
                EmergencyDispatch.Services.ViewIdAssign.NavToControlCenterManagId,
                               1);
        }
    }
}
