using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.ExtendYixinEsu.TreeTabRtuSet
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToJnQueryView : MenuItemBase
    {
        public NavToJnQueryView()
        {
            Id = ExtendYixinEsu.Services.MenuIdAssgin.NavToTreeTabRtuSetId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToWj1050InfoSetViewId;
            Text = "终端分区管理";
            Tag = "终端分区管理";
            Classic = "主菜单";
            Description = "终端分区管理，ID 为" + ExtendYixinEsu.Services.MenuIdAssgin.NavToTreeTabRtuSetId;
            Tooltips = "终端分区管理";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex);
            //IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return true;
        }
        protected void Ex()
        {
            // JnRtuSet.NavTo.NavToLdl();
            ExNavWithArgs( ExtendYixinEsu.Services.ViewIdAssign.TreeTabRtuSetViewId,
                          0);
        }
    }
}
