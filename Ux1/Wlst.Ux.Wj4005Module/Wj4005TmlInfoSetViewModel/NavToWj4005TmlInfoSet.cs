using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.Wj4005Module;


namespace Wlst.Ux.WJ4005Module.Wj4005TmlInfoSetViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj4005TmlInfoSet : MenuItemBase
    {
        public NavToWj4005TmlInfoSet()
        {
            Id = WJ4005Module.Services.MenuIdAssgin.NavToWj4005TmlInfoSetId;// Infrastructure.IdAssign.MenuIdAssign.NavToWj4005TmlInfoSetId;
            Text = "4005参数设置";
            Tag = "4005参数设置";
            this.Classic = "右键菜单-4005";
            Description = "照明终端参数设置，ID 为" + WJ4005Module.Services.MenuIdAssgin.NavToWj4005TmlInfoSetId;// Infrastructure.IdAssign.MenuIdAssign.NavToWj4005TmlInfoSetId;
            Tooltips = "设置当前终端终端参数";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex );
           // this.IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            var equipment = this.Argu as Wlst .Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null ) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(areaId);
        }
        protected void Ex()
        {
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1)
                return;
            this.ExNavWithArgs(
                WJ4005Module.Services.ViewIdAssign.Wj4005TmlInfoSetViewId,
                rtuId);
        }
    }
}