using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.Setting.RecordTaskQueryViewModel
{


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToRecordTaskQueryViewModel : MenuItemBase
    {
        public NavToRecordTaskQueryViewModel()
        {
            Id = Setting.Services.MenuIdAssgin.NavToNavToRecordTaskQueryViewId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "任务记录";
            Tag = "任务执行记录查询";
            this.Classic = "主菜单";
            Description = "任务执行记录查询，ID 为" + Setting.Services.MenuIdAssgin.NavToNavToRecordTaskQueryViewId;
                // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "任务执行记录查询";
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
                               //ViewIdNameAssign.EquipmentDataQuerySndWeekTimeQueryViewAttachRegion,
                               Setting.Services.ViewIdAssign.RecordTaskQueryViewId,
                               //ViewIdNameAssign.EquipmentDataQuerySndWeekTimeQueryViewId,
                               1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
