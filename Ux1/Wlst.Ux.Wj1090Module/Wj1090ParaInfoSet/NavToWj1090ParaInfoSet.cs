using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.Wj1090Module.Wj1090ParaInfoSet
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj1090ParaInfoSet : MenuItemBase
    {
        public NavToWj1090ParaInfoSet()
        {
            Id = Wj1090Module.Services.MenuIdAssgin.NavToLduInfoSetViewfor1090Id;
                // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Text = "参数设置";
            Tag = "线路检测集中器参数设置";
            Classic = "右键菜单-线路检测集中器-专有";
            Description = "线路检测集中器参数设置，防盗通用，ID 为" + Wj1090Module.Services.MenuIdAssgin.NavToLduInfoSetViewfor1090Id;
                // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Tooltips = "设置当前线路检测参数";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex,CanEx,true);
        //    IsPrivilegLeave = true;

        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
            var equipment = this.Argu as WjParaBase;//Wlst.Cr.WjEquipmentBaseModels.Interface.IIEquipmentInfo;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(areaId);
        }
        private static bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {
            var equipment = Argu as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;// IIEquipmentInfo;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1)
                return;
            ExNavWithArgs(
                Wj1090Module.Services.ViewIdAssign.Wj1090ParaInfoSetViewId,
                rtuId);
        }

    }
}