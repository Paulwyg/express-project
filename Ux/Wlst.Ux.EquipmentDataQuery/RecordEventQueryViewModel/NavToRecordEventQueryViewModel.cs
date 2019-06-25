using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipmentDataQuery.RecordEventQueryViewModel
{


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToRecordEventQueryViewModel : MenuItemBase
    {
        public NavToRecordEventQueryViewModel()
        {
            Id = EquipmentDataQuery.Services.MenuIdAssgin.NavToRecordEventQueryViewId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "事件记录查询";
            Tag = "事件记录查询";
            this.Classic = "主菜单";
            Description = "事件记录查询，ID 为" + EquipmentDataQuery.Services.MenuIdAssgin.NavToRecordEventQueryViewId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "事件记录查询";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }

        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            this.ExNavWithArgs(EquipmentDataQuery.Services.ViewIdAssign.RecordEventQueryViewAttachRegion,
                               //ViewIdNameAssign.EquipmentDataQuerySndWeekTimeQueryViewAttachRegion,
                               EquipmentDataQuery.Services.ViewIdAssign.RecordEventQueryViewId,
                               //ViewIdNameAssign.EquipmentDataQuerySndWeekTimeQueryViewId,
                               1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
