using System.ComponentModel.Composition;

using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlPriorityLevelSettingViewModel
{
    //导航到终端优先级设置 主菜单
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentFaultWithTmlPriorityLevelSettingViewforMainMenu : MenuItemBase
    {
        public NavToEquipmentFaultWithTmlPriorityLevelSettingViewforMainMenu()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultWithTmlPriorityLevelSettingViewforMainMenuId;
            Text = "终端优先级设置";
            Tag = "终端优先级设置";
            this.Classic = "主菜单";
            Description = "终端优先级设置，ID 为" +
                        EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultWithTmlPriorityLevelSettingViewforMainMenuId;
            Tooltips = "终端优先级设置";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex,CanEx ,true  );
           // this.IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {//return Wlst.Cr.CoreMims.Services.UserInfo.CanW();

            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;
        }

        bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0;
        }
        protected void Ex()
        {
            this.ExNavWithArgs(
                EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultWithTmlPriorityLevelSettingViewId,
                               0);
        }


    }
}
