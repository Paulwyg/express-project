using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.AssetManagementModule.Services;

namespace Wlst.Ux.AssetManagementModule.LampManage
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToLampManage : MenuItemBase
    {
        public NavToLampManage()
        {
            Id =  Wlst.Ux.AssetManagementModule.Services.MenuIdAssign.NavToLampManageViewId ;
            Text = "电源资产管理";
            Tag = "电源资产管理，长沙资产管理 电源杆号信息";
            this.Classic = "主菜单";
            Description = "电源资产管理，长沙资产管理 电源杆号信息，ID 为" + Wlst.Ux.AssetManagementModule.Services.MenuIdAssign.NavToLampManageViewId;
            Tooltips = "电源资产管理，长沙资产管理 电源杆号信息";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //   IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            //if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0;
            ////return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return true;
        }
        bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdAssign.LampManageViewId,
                                1);
        }
    }
}
