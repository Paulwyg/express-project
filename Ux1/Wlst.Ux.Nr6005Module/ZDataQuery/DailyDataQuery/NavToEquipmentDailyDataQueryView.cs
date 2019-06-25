using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.DailyDataQuery
{


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentDailyDataQueryView : MenuItemBase
    {
        public NavToEquipmentDailyDataQueryView()
        {
            Id = Nr6005Module.Services.MenuIdAssgin.NavToEquipmentDailyDataQueryViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Text = "终端数据";
            Tag = "数据查询";
            this.Classic = "主菜单";
            Description = "数据查询，ID 为" + Nr6005Module.Services.MenuIdAssgin.NavToEquipmentDailyDataQueryViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentDailyDataQueryViewId;
            Tooltips = "数据查询";
            base.IsEnabled = true;
            base.IsCheckable = false;
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
                Nr6005Module.Services.ViewIdAssign.EquipmentDailyDataQueryViewId,
                            //   ViewIdNameAssign.EquipmentDataQueryEquipmentDailyDataQueryViewId,
                               0);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
