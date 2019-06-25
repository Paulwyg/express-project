using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.Wj9001Module.Wj9001ParaInfoSet
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj9001ParaInfoSet : MenuItemBase
    {
        public NavToWj9001ParaInfoSet()
        {
            Id = Wj9001Module.Services.MenuIdAssgin.NavToLeakInfoSetViewfor9001Id;
                // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Text = "参数设置";
            Tag = "漏电参数设置";
            Classic = "右键菜单-漏电保护器-专有";
            Description = "漏电参数设置，线路检测通用，ID 为" + Wj9001Module.Services.MenuIdAssgin.NavToLeakInfoSetViewfor9001Id;
                // Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Tooltips = "设置当前终端终端参数";
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
            if (equipment == null) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanW(areaId);
        }
        private static bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {
            var equipment = Argu as Sr.EquipmentInfoHolding.Model.Wj9001Leak;// IIEquipmentInfo;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1)
                return;
            ExNavWithArgs(
                Wj9001Module.Services.ViewIdAssign.Wj9001ParaSetViewId,
                rtuId);
        }

    }
}