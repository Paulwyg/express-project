using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.WeekSetQuery
{


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWeekSetQueryView : MenuItemBase
    {
        public NavToWeekSetQueryView()
        {
            Id = Wj2090Module.Services.MenuIdAssign.NavToWeekSetQueryViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Text = "单灯时间发送查询";
            Tag = "单灯时间发送查询";
            this.Classic = "主菜单";
            Description = "单灯时间发送查询，ID 为" + Wj2090Module.Services.MenuIdAssign.NavToWeekSetQueryViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Tooltips = "单灯时间发送查询";
            base.IsEnabled = true;
            base.IsCheckable = false;
            //IsPrivilegLeave = false;
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
            this.ExNavWithArgs(
                               //ViewIdNameAssign.EquipmentDataQueryEquipmentDailyDataQueryViewAttachRegion,
                               Wj2090Module.Services.ViewIdAssign.WeekSetQueryViewViewId,
                               //   ViewIdNameAssign.EquipmentDataQueryEquipmentDailyDataQueryViewId,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
