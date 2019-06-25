using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.TimeTableBandingViewModel
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeTableBandingView : MenuItemBase //TreeModuleGrpMulitManageView
    {
        public NavToTimeTableBandingView()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToTimeTableBandingViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Text = "时间表与绑定";
            this.Classic = "主菜单";
            Tag = "时间表与绑定，设置时间表与终端、组的绑定信息";
            Description = "时间表与绑定，设置时间表与终端、组的绑定信息，ID 为" +
                          TimeTableSystem.Services.MenuIdAssgin.NavToTimeTableBandingViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Tooltips = "时间表与绑定，设置时间表与终端、组的绑定信息";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            this.IsPrivilegLeave = true;
        }

        bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            this.ExNavWithArgs(
                //ViewIdNameAssign.TimeTableSystemModuleTimeTableBandingViewAttachRegion,
                //               ViewIdNameAssign.TimeTableSystemModuleTimeTableBandingViewId,
                TimeTableSystem .Services .ViewIdAssign .TimeTableBandingViewAttachRegion  ,
                TimeTableSystem.Services.ViewIdAssign.TimeInfoMnViewId,
                               1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }

    }
}
