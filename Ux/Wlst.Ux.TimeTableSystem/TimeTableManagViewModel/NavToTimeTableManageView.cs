using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeTableManageView : MenuItemBase //TreeModuleGrpMulitManageView
    {
        public NavToTimeTableManageView()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToTimeTableManageViewId;
            Text = "时间表管理";
            Classic = "主菜单";
            Tag = "时间表管理，时间表增加及其修改";
            Description = "时间表管理，ID 为" +
                        TimeTableSystem.Services.MenuIdAssgin.NavToTimeTableManageViewId;
            Tooltips = "时间表管理";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex ,CanEx ,true );
            IsPrivilegLeave = true;
        }

        bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {
            ExNavWithArgs(
                TimeTableSystem.Services.ViewIdAssign.TimeTableManageViewAttachRegion,
                TimeTableSystem.Services.ViewIdAssign.TimeTableManageViewId,
                               1);
        }

    }
}
