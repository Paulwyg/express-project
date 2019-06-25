using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipmentGroupManage.Services;

namespace Wlst.Ux.EquipmentGroupManage.GrpMulitManageViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToGrpMulitManageView : MenuItemBase  //TreeModuleGrpMulitManageView
    {
        public NavToGrpMulitManageView()
        {
            Id =MenuIdAssgin.NavToGrpMulitManageViewId;
            Text = "本地分组";
            Tag = "设置本地分组，更改本地分组信息";
            this.Classic = "主菜单";
            Description = "设置本地分组，更改显示分组信息，ID 为" + MenuIdAssgin.NavToGrpMulitManageViewId;
            Tooltips = "设置本地分组，更改显示分组信息";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex );
        }

        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count != 0;
            //return base.IsCanBeShowRwx();
        }

        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdAssign .TreeModuleGrpMulitManageViewId,
                               1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }

    }
}
