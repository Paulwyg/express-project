using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.TunnelInfo.TunnelInfoSet
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToTunnelInfoSetView : MenuItemBase
    {
        public NavToTunnelInfoSetView()
        {
            Id = TimeTableSystem.Services.MenuIdAssgin.NavToTunnelInfoSetViewId;
            Text = "隧道方案";
            Tag = "隧道方案";
            Classic = "主菜单";
            Description = "隧道方案设置，线路检测通用，ID 为"
                          + TimeTableSystem.Services.MenuIdAssgin.NavToTunnelInfoSetViewId;
            Tooltips = "隧道方案";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            // IsPrivilegLeave = true ;

        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0; ;
        }
        private bool CanEx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0;
        }

        protected void Ex()
        {

            ExNavWithArgs(
                TimeTableSystem.Services.ViewIdAssign.TunnelInfoSetViewId,
                0);
        }
    }
}
