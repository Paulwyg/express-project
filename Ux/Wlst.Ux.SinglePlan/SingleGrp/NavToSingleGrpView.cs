using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.SinglePlan.SingleGrp
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSingleGrpView : MenuItemBase
    {
        public NavToSingleGrpView()
        {
            Id = Wlst.Ux.SinglePlan.Services.MenuIdAssgin.NavToSingleGrpViewId;
            Text = "新单灯分组";
            Tag = "新单灯分组管理";
            Classic = "主菜单";
            Description = "新单灯分组，ID 为"
                          + Wlst.Ux.SinglePlan.Services.MenuIdAssgin.NavToSingleGrpViewId;
            Tooltips = "新单灯分组管理";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex);
            // IsPrivilegLeave = true ;

        }
        public override bool IsCanBeShowRwx()
        {
            return true;
        }

        protected void Ex()
        {

            ExNavWithArgs(
                Wlst.Ux.SinglePlan.Services.ViewIdAssign.SingleGrpViewId,
                0);
        }
    }
}
