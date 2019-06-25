using System.ComponentModel.Composition;
using Infrastructure.MessageBoxOverride;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.UtilityFunction;
using Wlst.Sr.ProtocolCnt.PrivilegeInfo.ToClient;
using Wlst.Ux.PrivilegesManage.Services;

namespace Wlst.Ux.PrivilegesManage.ModflyOtherUserInfoViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToModflyOtherUserInfoView : MenuItemBase
    {
        public NavToModflyOtherUserInfoView()
        {
            Id = MenuIdAssgin.NavToModflyOtherUserInfoViewId; // Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Text = "用户管理";
            Tag = "管理其他用户信息及密码";
            Description = "管理其他用户信息及密码，ID 为" + MenuIdAssgin.NavToModflyOtherUserInfoViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToPatrolViewId;
            Tooltips = "管理其他用户信息及密码";
            this.Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
            this.IsPrivilegLeave = true;
        }


        private void Ex()
        {
            var sss = Infrastructure.MessageBoxOverride.UMessageBoxWantSomefromUser.Show("密码验证", "请输入您的用户密码", "");
            if (sss == Infrastructure.MessageBoxOverride.UMessageBoxWantSomefromUser.CancelReturn)
            {
                return;
            }
            if (sss != UserInfo.UserLoginInfo.UserPasswrod)
            {
                Infrastructure.MessageBoxOverride.UMessageBox.Show("验证失败", "您输入的密码与本用户密码不匹配，请检查......",
                                                                   UMessageBoxButton.Yes);
                return;
            }

            this.ExNavWithArgs(ViewIdAssign.ModflyOtherUserInfoViewAttachRegion,
                              ViewIdAssign.ModflyOtherUserInfoViewId, 1);
            return;
            //todo 

            LogInfo.Log("正在请求服务器验证用户权限...");
            //验证用户权限
            Sr.PrivilegesCrl.Services.PrivilegeInfoServer.ExIfHasRightToModflyOtherUserInfo(RightInfoCheck,
                                                                                            RightInfoCheckRequestOutTime);
        }

        private bool RightInfoCheck(object obj)
        {
            var t = obj as UserRightOfPrivilege;
            if (t == null)
            {
                Infrastructure.MessageBoxOverride.UMessageBox.Show("验证用户权限失败", "验证失败：请求服务错误...",UMessageBoxButton.Ok );
                return true;
            }
            if (!t.Right)
            {
                Infrastructure.MessageBoxOverride.UMessageBox.Show("验证用户权限失败", "验证失败：您不具有管理其他用户权限的权力!!!", UMessageBoxButton.Ok);
                return true;
            }

            this.ExNavWithArgs(ViewIdAssign.ModflyOtherUserInfoViewAttachRegion,
                               ViewIdAssign.ModflyOtherUserInfoViewId, 1);
            return true;
        }

        private void RightInfoCheckRequestOutTime()
        {
            Infrastructure.MessageBoxOverride.UMessageBox.Show("验证用户权限失败", "验证失败：请求验证服务超时...", UMessageBoxButton.Ok);
        }

    }
}
