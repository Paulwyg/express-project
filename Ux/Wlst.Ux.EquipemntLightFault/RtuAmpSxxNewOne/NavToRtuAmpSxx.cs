using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultDefineSettingViewModel.RtuAmpSxxNewOne
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToRtuAmpSxx : MenuItemBase
    {
        //public static void NavTo()
        //{
        //    RegionManage.ShowViewByIdAttachRegionWithArgu(EquipemntLightFault.Services.ViewIdAssign.RtuAmpSxxViewId,
        //                                                  "");
        //}
        
            public NavToRtuAmpSxx()
            {
                Id = EquipemntLightFault.Services.MenuIdAssgin.NavToRtuAmpSxxViewId;
                //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
                Text = "设置电流上下限";
                this.Classic = "主菜单";
                Tag = "设置电流上下限，设置电流上下限报警的对比值";
                Description = "设置电流上下限，设置电流上下限报警的对比值，ID 为" +
                            EquipemntLightFault.Services.MenuIdAssgin.NavToRtuAmpSxxViewId;
                //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
                Tooltips = "设置电流上下限，设置电流上下限报警的对比值";
                base.IsCheckable = false;
                base.IsEnabled = true;
                base.Command = new RelayCommand(Ex, CanEx, true);
                // this.IsPrivilegLeave = true;
            }
            public override bool IsCanBeShowRwx()
            {
                return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D);
                //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
                //return true;
            }
            bool CanEx()
            {
                return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D);
                //return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.IsInFullSetMod ;
                //return true;
            }

            protected void Ex()
            {
                this.ExNavWithArgs(
                    //ViewIdNameAssign.TimeTableSystemModuleTimeTableBandingViewAttachRegion,
                    //               ViewIdNameAssign.TimeTableSystemModuleTimeTableBandingViewId,

                    EquipemntLightFault.Services.ViewIdAssign.RtuAmpSxxViewId,
                                   "");
                // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
            }

        }
    
}
