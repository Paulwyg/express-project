using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
 

namespace Wlst.Ux.Nr6005Module.ZDataQuery.SndWeekTimeQuery
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToSndWeekTimeQueryViewTmlRightMenu : MenuItemBase
    {
        public NavToSndWeekTimeQueryViewTmlRightMenu()
        {
            Id = Nr6005Module.Services.MenuIdAssgin.NavToSndWeekTimeQueryViewTmlRightMenuId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "周设置记录";
            Tag = "周设置记录";
            this.Classic = "终端右键菜单";
            Description = "周设置记录，ID 为" +
                          Nr6005Module.Services.MenuIdAssgin.NavToSndWeekTimeQueryViewTmlRightMenuId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "周设置记录";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex,CanEx ,true );
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId);
        }
        bool CanEx()
        {
            return true;
        }



        protected void Ex()
        {
            var terminalInfo = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            int rtuId = 0;
            if (terminalInfo != null) rtuId = terminalInfo.RtuId;
            this.ExNavWithArgs(
                               Nr6005Module.Services.ViewIdAssign.SndWeekTimeQueryViewId,
                               rtuId);
        }


    }
}
