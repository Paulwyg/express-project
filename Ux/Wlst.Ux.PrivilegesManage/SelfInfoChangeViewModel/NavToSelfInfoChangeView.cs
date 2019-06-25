using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.PrivilegesManage.Services;

namespace Wlst.Ux.PrivilegesManage.SelfInfoChangeViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSelfInfoChangeView : MenuItemBase
    {
        public NavToSelfInfoChangeView()
        {
            Id = MenuIdAssgin.NavToSelfInfoChangeViewId; // Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Text = "用户信息";
            Tag = "用户信息查看与修改";
            Description = "用户信息查看与修改，ID 为" + MenuIdAssgin.NavToSelfInfoChangeViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Tooltips = "用户信息查看与修改";
            Classic = "主菜单";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex);
            //IsPrivilegLeave = true;
        }

        //public override bool IsCanBeShowRwx()
        //{
        //    return true;
        //}

        public override bool IsCanBeShowRwx()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.Cand()==false;
            //return true;
        }
        private void Ex()
        {
            ExNavWithArgs( ViewIdAssign.SelfInfoChangeViewId, 1);

        }

    }
}
