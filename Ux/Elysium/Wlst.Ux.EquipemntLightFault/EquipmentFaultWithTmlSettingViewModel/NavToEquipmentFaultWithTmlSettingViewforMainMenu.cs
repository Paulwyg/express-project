using System.ComponentModel.Composition;

using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultWithTmlSettingViewModel
{
    //导航到终端故障设置 主菜单
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentFaultWithTmlSettingViewforMainMenu : MenuItemBase
    {
        public NavToEquipmentFaultWithTmlSettingViewforMainMenu()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultWithTmlSettingViewforMainMenuId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultWithTmlSettingViewforMainMenuId;
            Text = "终端故障设置";
            Tag = "终端故障设置";
            this.Classic = "主菜单";
            Description = "终端故障设置，ID 为" +
                        EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultWithTmlSettingViewforMainMenuId;
                        //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultWithTmlSettingViewforMainMenuId;
            Tooltips = "终端故障设置";
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
                //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultWithTmlSettingViewAttachRegion,
                //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultWithTmlSettingViewId,
            
                EquipemntLightFault .Services .ViewIdAssign .EquipmentFaultWithTmlSettingViewId ,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
