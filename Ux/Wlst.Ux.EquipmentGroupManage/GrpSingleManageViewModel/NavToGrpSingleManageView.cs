using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipmentGroupManage.Services;

namespace Wlst.Ux.EquipmentGroupManage.GrpSingleManageViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToGrpSingleManageView : MenuItemBase
    {
        public NavToGrpSingleManageView()
        {
            Id = MenuIdAssgin .NavToGrpSingleManageViewId;
            Text = "全局分组";
            Tag = "终端分组设置，一个终端只能属于一个组";
            this.Classic = "主菜单";
            Description = "终端分组设置，一个终端只能属于一个组，ID 为" + MenuIdAssgin .NavToGrpSingleManageViewId;
            Tooltips = "终端分组设置，一个终端只能属于一个组";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex,CanEx ,true  );
         //   IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            return true;
        }
        bool CanEx()
        {
            return true;
        }

        protected void Ex()
        {
            this.ExNavWithArgs( ViewIdAssign .GrpShowModuleGrpSingleManageViewId,
                                1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
