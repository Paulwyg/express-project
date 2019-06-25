﻿using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EmergencyDispatch.ControlCenterManagDemo2
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToControlCenterManag : MenuItemBase
    {
        public NavToControlCenterManag()
        {
            Id = Wlst.Ux.EmergencyDispatch.Services.MenuIdAssgin.NavToControlCenterViewDemo2;
            Text = "控制中心";
            Tag = "控制中心";
            Classic = "主菜单";
            Description = "控制中心，ID 为" + EmergencyDispatch.Services.MenuIdAssgin.NavToControlCenterViewDemo2;
            Tooltips = "控制中心";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
         //   IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX.Count > 0;
        }
        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            ExNavWithArgs( EmergencyDispatch.Services.ViewIdAssign.NavToControlCenterManagDemo2Id,
                               1);
        }
    }
}
