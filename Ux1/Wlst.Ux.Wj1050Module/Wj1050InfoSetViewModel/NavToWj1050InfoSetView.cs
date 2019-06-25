using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
 

namespace Wlst.Ux.Wj1050Module.Wj1050InfoSetViewModel
{
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class NavToWj1050InfoSetView : MenuItemBase
   {
       public NavToWj1050InfoSetView()
       {
           Id = Wj1050Module.Services.MenuIdAssgin.NavToWj1050InfoSetViewId;// Infrastructure.IdAssign.MenuIdAssign.NavToWj1050InfoSetViewId;
           Text  = "抄表参数设置";
           Tag = "WJ1050抄表参数设置";
           this.Classic = "右键菜单-WJ1050-专有";
           Description = "WJ1050抄表参数设置，ID 为" + Wj1050Module.Services.MenuIdAssgin.NavToWj1050InfoSetViewId;
           ;// Infrastructure.IdAssign.MenuIdAssign.NavToWj1050InfoSetViewId;
           Tooltips = "WJ1050抄表参数设置";
           base.IsEnabled = true;
           base.IsCheckable = false;
           base.Command = new RelayCommand(Ex );
          // IsPrivilegLeave = true;
       }
       public override bool IsCanBeShowRwx()
       {
           if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
           //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
           var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
           if (equipment == null || equipment.RtuStateCode  == 0) return false;
           var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
           return Wlst.Cr.CoreMims.Services.UserInfo.CanW(areaId);
       }
       protected void Ex()
       {
           
           var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
           if (equipment == null) return;
           int rtuId = equipment.RtuId;
           if (rtuId < 1) return;
           this.ExNavWithArgs(   Wj1050Module .Services .ViewIdAssign .Wj1050InfoSetViewId ,

               //ViewIdNameAssign.Wj1050ModuleWj1050InfoSetViewAttachRegion,
               //               ViewIdNameAssign.Wj1050ModuleWj1050InfoSetViewId,
                              rtuId);
       }
   }
}
