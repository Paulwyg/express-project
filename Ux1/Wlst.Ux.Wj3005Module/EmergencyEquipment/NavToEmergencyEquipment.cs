using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.WJ3005Module.EmergencyEquipment
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEmergencyEquipment : MenuItemBase
    {
        public NavToEmergencyEquipment()
        {
            Id = Wlst.Ux.WJ3005Module.Services.MenuIdAssgin.NavToEmergencyEquipment;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "应急设备设置";
            Tag = "应急设备设置";
            this.Classic = "主菜单";
            Description = "应急设备设置，ID 为" + Wlst.Ux.WJ3005Module.Services.MenuIdAssgin.NavToEmergencyEquipment;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "应急设备设置";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count > 0;
            return false;
        }

        private bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {

            this.ExNavWithArgs(
                WJ3005Module.Services.ViewIdAssign.NavToEmergencyEquipmentViewId,
                0);
        }
    }
}
