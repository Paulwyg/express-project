using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.AssetManagementModule.Services;

namespace Wlst.Ux.AssetManagementModule.SimManage
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSimManage : MenuItemBase
    {
        public NavToSimManage()
        {
            Id = Wlst.Ux.AssetManagementModule.Services.MenuIdAssign.NavToSimManageViewId;
            Text = "SIM卡资产管理";
            Tag = "SIM卡资产管理，武汉资产管理";
            this.Classic = "主菜单";
            Description = "SIM卡资产管理，武汉资产管理，ID 为" + Wlst.Ux.AssetManagementModule.Services.MenuIdAssign.NavToSimManageViewId;
            Tooltips = "SIM卡资产管理，武汉资产管理";
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
            this.ExNavWithArgs(ViewIdAssign.SimManageViewId,
                                1);
        }
    }
}
