using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.SinglePlan.SinglePlan
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSinglePlanView : MenuItemBase
    {
        public NavToSinglePlanView()
        {
            Id = Wlst.Ux.SinglePlan.Services.MenuIdAssgin.NavToSinglePlanViewId;
            Text = "新单灯方案管理";
            Tag = "新单灯方案管理";
            Classic = "主菜单";
            Description = "新单灯方案管理，ID 为"
                          + Wlst.Ux.SinglePlan.Services.MenuIdAssgin.NavToSinglePlanViewId;
            Tooltips = "新单灯方案管理";
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
                Wlst.Ux.SinglePlan.Services.ViewIdAssign.SinglePlanViewId,
                0);
        }
    }
}