using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.TimeTabletemporaryView
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTimeTabletemporaryView : MenuItemBase //TreeModuleGrpMulitManageView
    {
        public NavToTimeTabletemporaryView()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToTimeTabletemporaryViewId;
            Text = "临时周设置";
            Classic = "主菜单";
            Tag = "临时周设置，辅助调整周设置";
            Description = "临时周设置，ID 为" +
                          TimeTableSystem.Services.MenuIdAssgin.NavToTimeTabletemporaryViewId;
            Tooltips = "临时周设置";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return true;
        }
        private bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            ExNavWithArgs(
               
                TimeTableSystem.Services.ViewIdAssign.TimeTabletemporaryViewId,
                1);
        }

    }
}
