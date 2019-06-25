using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.fuyang.BroadcastStrategy
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToBroadcastStrategyView : MenuItemBase
    {
        public NavToBroadcastStrategyView()
        {
            Id = fuyang.Services.MenuIdAssgin.NavToBroadcastStrategyViewId;
            Text = "播放策略管理";
            Tag = "播放策略管理";
            Classic = "主菜单";
            Description = "播放策略管理，ID 为"
                          + fuyang.Services.MenuIdAssgin.NavToBroadcastStrategyViewId;
            Tooltips = "播放策略管理";
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
                fuyang.Services.ViewIdAssign.BroadcastStrategyViewId,
                0);
        }
    }
}