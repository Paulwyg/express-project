using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.PrivilegesManage.UserInfoManageViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToUserInfoManageView:MenuItemBase
    {
        public NavToUserInfoManageView()
        {
            Id = PrivilegesManage.Services.MenuIdAssgin.NavToUserManageViewId; // Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Text = "用户信息管理";
            Tag = "用户信息管理";
            Description = "用户信息管理，ID 为" + PrivilegesManage.Services.MenuIdAssgin.NavToUserManageViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Tooltips = "用户信息管理";
            this.Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
            
        }


        private void Ex()
        {
            this.ExNavWithArgs(PrivilegesManage.Services.ViewIdAssign.UserInfoManageViewAttachRegion,
                               PrivilegesManage.Services.ViewIdAssign.UserInfoManageViewId, 1);

        }
    }
}
