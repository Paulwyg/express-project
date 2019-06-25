using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.AddMainEquipment
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToAddMainEquipmentView : MenuItemBase
    {
        public NavToAddMainEquipmentView()
        {
            Id = Services.MenuIdAssgin.NavToAddMainEquipmentViewId;
            Text = "增加设备";
            Tag = "增加设备";
            Description = "增加主设备，ID 为" + Services.MenuIdAssgin.NavToAddMainEquipmentViewId;
            Tooltips = "增加主设备";
            Classic = "主菜单";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex,CanEx ,true);
            //IsPrivilegLeave = true;

       
           
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaW.Count > 0;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
        }
        bool CanEx()
        {
            return true;
            return Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D  ;
        }

        protected void Ex()
        {
            
            ExNavWithArgs(Services.ViewIdAssign.AddMainEquipmentViewId,1);
        }

    }
}
