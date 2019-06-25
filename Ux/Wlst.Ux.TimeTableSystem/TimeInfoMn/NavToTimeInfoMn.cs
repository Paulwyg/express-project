using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeInfoMn : MenuItemBase //TreeModuleGrpMulitManageView
    {
        public NavToTimeInfoMn()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToNavToTimeInfoMnViewId ;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Text = "周设置";
            this.Classic = "主菜单";
            Tag = "时间表与绑定，设置时间表信息与终端、组的绑定信息";
            Description = "时间表与绑定，设置时间表信息与终端、组的绑定信息，ID 为" +
                        TimeTableSystem.Services.MenuIdAssgin.NavToNavToTimeInfoMnViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Tooltips = "时间表与绑定，设置时间表信息与终端、组的绑定信息";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
           // this.IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count > 0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D);
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            //return true;
        }
        bool CanEx()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count>0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) ;
            //return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.IsInFullSetMod ;
            //return true;
        }

        protected void Ex()
        {
            this.ExNavWithArgs(
                //ViewIdNameAssign.TimeTableSystemModuleTimeTableBandingViewAttachRegion,
                //               ViewIdNameAssign.TimeTableSystemModuleTimeTableBandingViewId,
              
                TimeTableSystem.Services.ViewIdAssign.TimeInfoMnViewId ,
                               -1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }

    }
}
