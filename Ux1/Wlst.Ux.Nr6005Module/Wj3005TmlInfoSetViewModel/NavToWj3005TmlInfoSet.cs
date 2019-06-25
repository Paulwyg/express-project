using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
 

namespace Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj3005TmlInfoSet : MenuItemBase
    {
        public NavToWj3005TmlInfoSet()
        {
            Id = Nr6005Module.Services.MenuIdAssgin.NavToWj3005TmlInfoSetId;// Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Text = "终端参数设置";
            Tag = "终端参数设置";
            this.Classic = "右键菜单-3005-3009";
            Description = "照明终端参数设置，ID 为" + Nr6005Module.Services.MenuIdAssgin.NavToWj3005TmlInfoSetId;// Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
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
                Nr6005Module .Services .ViewIdAssign .Wj3005TmlInfoSetViewId ,
                rtuId);
        }
    }
}