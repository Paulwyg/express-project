using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;


namespace Wlst.Ux.fuyang.OnlineStatus
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToOnlineStatusView : MenuItemBase
    {
        public NavToOnlineStatusView()
        {
            Id = fuyang.Services.MenuIdAssgin.NavToOnlineStatusViewId;
            Text = "设备在线状态查询";
            Tag = "设备在线状态查询";
            Classic = "主菜单";
            Description = "设备在线状态查询，ID 为"
                          + fuyang.Services.MenuIdAssgin.NavToOnlineStatusViewId;
            Tooltips = "设备在线状态查询";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex);
            // IsPrivilegLeave = true ;

        }
        public override bool IsCanBeShowRwx()
        {
            return true;
        }

        protected void Ex()
        {

            ExNavWithArgs(
                fuyang.Services.ViewIdAssign.OnlineStatusViewId,
                0);
        }
    }
}
