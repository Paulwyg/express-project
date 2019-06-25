using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentFaultDefineSettingView : MenuItemBase
    {
        public NavToEquipmentFaultDefineSettingView()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultDefineSettingViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultDefineSettingViewId;
            Text = "故障设置";
            Tag = "故障设置";
            this.Classic = "主菜单";
            Description = "系统故障设置，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultDefineSettingViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultDefineSettingViewId;
            Tooltips = "系统故障设置";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex,CanEx ,true  );
            //this.IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW .Count >0 ;;
        }
        bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {
            this.ExNavWithArgs(
                //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultDefineSettingViewAttachRegion,
                //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultDefineSettingViewId,

                EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultDefineSettingViewId,
                               1);


            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
