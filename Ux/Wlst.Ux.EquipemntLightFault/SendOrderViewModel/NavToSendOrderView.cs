using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;


namespace Wlst.Ux.EquipemntLightFault.SendOrderViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSendOrderView : MenuItemBase
    {
        public NavToSendOrderView()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToSendOrderViewforMainMenuId;
            Text = "派单";
            Tag = "派单";
            Classic = "主菜单";
            Description = "派单，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToSendOrderViewforMainMenuId;
            Tooltips = "派单";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX.Count > 0;
        }

        private bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            ExNavWithArgs(EquipemntLightFault.Services.ViewIdAssign.SendOrderViewId,
                          0);
        }
    }

}