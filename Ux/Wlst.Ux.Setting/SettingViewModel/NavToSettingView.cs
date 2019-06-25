using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Setting.Services;

namespace Wlst.Ux.Setting.SettingViewModel
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSettingView : MenuItemBase
    {
        public NavToSettingView()
        {
            Id = MenuIdAssgin.NavToSettingViewId;
            Text = "选项";
            Tag = "设置程序全局设置";
            Description = "设置程序全局设置，ID 为" + MenuIdAssgin.NavToSettingViewId;
            Tooltips = "设置程序全局设置";
            Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex );
            //IsPrivilegLeave = false ;
        }

        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return true;
        }
        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdAssign.SettingViewId,1 );
        }

    }
}
