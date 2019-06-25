using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj2090Module.Services;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp
{


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToOrdersGrpView : MenuItemBase
    {
        public NavToOrdersGrpView()
        {
            Id = Wj2090Module.Services.MenuIdAssign.NavToOrdersGrpViewId;
            Text = "单灯控制中心";
            Tag = "单灯控制中心";
            this.Classic = "主菜单";
            Description = "单灯控制中心 grp，ID 为" + Id;
            Tooltips = "单灯控制中心 grp";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
            //  IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanX();
            return Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX.Count > 0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;
        }

        protected void Ex()
        {

            ExNavWithArgs(
                          ViewIdAssign.OrderLargeGrpViewId,
                          0);
        }

    }
}
