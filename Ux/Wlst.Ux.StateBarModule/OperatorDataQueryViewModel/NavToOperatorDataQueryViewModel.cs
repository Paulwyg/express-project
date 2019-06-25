using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.StateBarModule.OperatorDataQueryViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToOperatorDataQueryViewModel : MenuItemBase
    {
        public NavToOperatorDataQueryViewModel()
        {
            Id = Services.MenuIdAssgin.NavToOperatorDataQueryViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Text = "操作记录";
            Tag = "操作记录查询";
            Classic = "主菜单";
            Description = "操作记录查询，ID 为" + Services.MenuIdAssgin.NavToOperatorDataQueryViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Tooltips = "操作记录查询";
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
                Services.ViewIdAssign.OperatorDataQueryViewId,
                               1);
        }

    }
}
