using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.HolidayTimeSet
{


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToHolidayTimeSetView : MenuItemBase //TreeModuleGrpMulitManageView
    {
        public NavToHolidayTimeSetView()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToHolidayTimeSetViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Text = "节假日设置";
            this.Classic = "主菜单";
            Tag = "节假日设置，设置终端节假日时间。";
            Description = "节假日设置，设置终端节假日时间，ID 为" +
                          TimeTableSystem.Services.MenuIdAssgin.NavToHolidayTimeSetViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToTimeTableBandingViewId;
            Tooltips = "节假日设置，设置终端节假日时间。";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex,CanEx ,true );
           // this.IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count > 0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D);
            //var equipment = this.Argu as Wlst.Cr.WjEquipmentBaseModels.Interface.IIEquipmentInfo;
            //if (equipment == null || equipment.RtuState == 0) return false;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW(equipment.AreaId);
            //return true;
        }
        bool CanEx()
        {
            return (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count > 0 || Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D);
            //return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.IsInFullSetMod;
            //return true;
        }
        protected void Ex()
        {
            this.ExNavWithArgs(
                //ViewIdNameAssign.TimeTableSystemModuleTimeTableBandingViewAttachRegion,
                //               ViewIdNameAssign.TimeTableSystemModuleTimeTableBandingViewId,
       
                TimeTableSystem.Services.ViewIdAssign.HolidayTimeSetViewId,
                1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }

    }
}
