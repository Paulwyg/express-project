using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.PrivilegesManage.UserInfoManageViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToUserAndPrivilegeInfoManagementViewModel : MenuItemBase
    {
        public NavToUserAndPrivilegeInfoManagementViewModel()
        {
            Id = PrivilegesManage.Services.MenuIdAssgin.NavToUserAndPrivilegeManageViewId; // Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Text = "用户管理";
            Tag = "用户管理";
            Description = "用户管理，ID 为" + PrivilegesManage.Services.MenuIdAssgin.NavToUserAndPrivilegeManageViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Tooltips = "用户管理";
            Classic = "主菜单";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex,CanEx,true  );
        }

         

        bool CanEx()
        {
            return true;// Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.IsInFullSetMod;
        }

        public override bool IsCanBeShowRwx()
        {
            return Wlst.Cr.CoreMims.Services.UserInfo.Cand();
            //return true;
        }
        private void Ex()
        {

            var sss = UMessageBoxWantPassWord.Show("密码验证", "请输入您的用户密码", "");
            if (sss == UMessageBoxWantPassWord.CancelReturn)
            {
                return;
            }
            if (sss != UserInfo.UserLoginInfo.UserPassword)
            {
                UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                                                   UMessageBoxButton.Yes);
                return;
            }

            ExNavWithArgs(PrivilegesManage.Services.ViewIdAssign.UserInfoManageViewId, 1);
        }
    }
}
