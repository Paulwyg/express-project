using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.WJ3005Module.EmergencyOperationQuery
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToOperationQuery : MenuItemBase
    {
        public NavToOperationQuery()
        {
            Id = Wlst.Ux.WJ3005Module.Services.MenuIdAssgin.NavToEmergencyOperationQuery;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "应急操作查询";
            Tag = "应急操作查询";
            this.Classic = "主菜单";
            Description = "应急操作查询，ID 为" + Wlst.Ux.WJ3005Module.Services.MenuIdAssgin.NavToEmergencyOperationQuery;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "应急操作查询";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0;
            return false;
        }

        private bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {

            this.ExNavWithArgs(
                WJ3005Module.Services.ViewIdAssign.NavToEmergencyOperationQueryViewId,
                0);
        }
    }
}
