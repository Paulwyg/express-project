using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Setting.Services;

namespace Wlst.Ux.Setting.SystemInfomationViewModel
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSystemInformationView : MenuItemBase
    {
        public NavToSystemInformationView()
        {
            Id = MenuIdAssgin.NavToSystemInformationViewId;
            Text = "系统信息";
            Tag = "系统相关信息";
            Description = "系统相关信息设置以及修改，ID 为" + MenuIdAssgin.NavToSystemInformationViewId;
            Tooltips = "系统相关信息设置以及修改";
            Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
            //IsPrivilegLeave = false ;
        }

        public override bool IsCanBeShowRwx()
        {
            return true;

        }
        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdAssign.EventSystemInformationViewId, 1);
        }

    }
}
