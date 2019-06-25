using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.Wj9001Module.Wj9001DataQueryViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj9001LeakDataQueryViewModel : MenuItemBase
    {
        public NavToWj9001LeakDataQueryViewModel()
        {
            Id = Wj9001Module.Services.MenuIdAssgin.NavToWj9001LeakDataQueryViewModelId;
            Text = "漏电数据查询";
            Tag = "漏电数据查询";
            Classic = "右键菜单-漏电设备-专有";
            Description = "漏电数据查询，漏电通用，ID 为"
                + Wj9001Module.Services.MenuIdAssgin.NavToWj9001LeakDataQueryViewModelId;
            Tooltips = "漏电数据查询";
            IsCheckable = false;
            IsEnabled = true;
            base.Command = new RelayCommand(Ex, CanEx, true);
            //IsPrivilegLeave = true;

        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Sr.EquipmentInfoHolding.Model.WjParaBase;//Wlst.Cr.WjEquipmentBaseModels.Interface.IIEquipmentInfo;
            if (equipment == null || equipment.RtuStateCode == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }
        bool CanEx()
        {
            return true;
        }
        protected void Ex()
        {

            var equipment = Argu as WjParaBase;// IIEquipmentInfo;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1)
                return;

            ExNavWithArgs(
               
                //Wj1090Module.Services.ViewIdAssign.Wj1090LduDataQueryViewModelId,
                //rtuId);
                Wj9001Module.Services.ViewIdAssign.Wj9001DataQueryViewId,rtuId);
        }
    }
}
