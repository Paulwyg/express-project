using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.Wj1090Module.Wj1090LduDataQueryViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToWj1090LduDataQueryViewModel : MenuItemBase
    {
        public NavToWj1090LduDataQueryViewModel()
        {
            Id = Wj1090Module.Services.MenuIdAssgin.NavToWj1090LduDataQueryViewModelId;
            Text = "数据查询";
            Tag = "线路检测集中器数据查询";
            Classic = "右键菜单-线路检测集中器-专有";
            Description = "线路检测集中器数据查询，防盗通用，ID 为" 
                + Wj1090Module.Services.MenuIdAssgin.NavToWj1090LduDataQueryViewModelId;
            Tooltips = "线路检测数据查询";
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
             
                Wj1090Module.Services.ViewIdAssign.Wj1090LduDataQueryViewModelId,
                rtuId);
        }
    }
}
