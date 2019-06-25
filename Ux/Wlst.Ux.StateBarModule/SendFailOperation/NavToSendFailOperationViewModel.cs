using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.StateBarModule.SendFailOperation
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSendFailOperationViewModel : MenuItemBase
    {
        public NavToSendFailOperationViewModel()
        {
            Id = Services.MenuIdAssgin.NavToSendFailOperationViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Text = "下发失败记录";
            Tag = "下发失败查询";
            Classic = "主菜单";
            Description = "下发失败记录，ID 为" + Services.MenuIdAssgin.NavToSendFailOperationViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Tooltips = "下发失败记录";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);

        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }
        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            ExNavWithArgs(
                Services.ViewIdAssign.SendFailOperationViewId,
                               1);
        }

    }
}
