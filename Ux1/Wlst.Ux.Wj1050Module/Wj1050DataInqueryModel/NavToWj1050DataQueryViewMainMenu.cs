using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;



namespace Wlst.Ux.Wj1050Module.Wj1050DataInqueryModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj1050DataQueryViewMainMenu : MenuItemBase
    {
        public NavToWj1050DataQueryViewMainMenu()
        {
            Id = Wj1050Module.Services.MenuIdAssgin.NavToWj1050DataQueryViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToWj1050InfoSetViewId;
            Text = "抄表数据查询";
            Tag = "WJ1050抄表数据查询";
            Classic = "右键菜单-抄表设备";
            Description = "WJ1050抄表数据查询，ID 为" + Wj1050Module.Services.MenuIdAssgin.NavToWj1050DataQueryViewId;
            Tooltips = "WJ1050抄表参数设置";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex);
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }
        protected void Ex()
        {

            var equipment = Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null) return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1) return;
            ExNavWithArgs(
                Wj1050Module.Services.ViewIdAssign.Wj1050DataInqueryViewId,
                               rtuId);
        }
    }
}
