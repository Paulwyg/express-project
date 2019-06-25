using System.Collections.Generic;
using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;


namespace Wlst.Ux.Wj1080Module.Wj1080InfoSet
{
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class NavToWj1080TmlInfoSet : MenuItemBase
   {
       public NavToWj1080TmlInfoSet()
       {
           Id = Wj1080Module.Services.MenuIdAssgin.NavToWj1080TmlInfoSetId;
           // Infrastructure.IdAssign.MenuIdAssign.NavToWj1080TmlInfoSetId;
           Text  = "参数设置";
           Tag = "WJ1080终端参数设置";
           this.Classic = "右键菜单-WJ1080-专有";
           Description = "WJ1080终端参数设置，ID 为" + Wj1080Module.Services.MenuIdAssgin.NavToWj1080TmlInfoSetId;
           //Infrastructure.IdAssign.MenuIdAssign.NavToWj1080TmlInfoSetId;
           Tooltips = "WJ1080终端参数设置";
           base.IsEnabled = true;
           base.IsCheckable = false;
           base.Command = new RelayCommand(Ex );
          // IsPrivilegLeave = true;
       }
       public override bool IsCanBeShowRwx()
       {
           if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
           //return Wlst.Cr.CoreMims.Services.UserInfo.CanW();
           var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding .Model .WjParaBase ;//.Services.EquipmentDataInfoHold.Info;
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
           int SelectIndex = 0;
           List<int> list = new List<int>() { rtuId, SelectIndex };
           this.ExNavWithArgs(
               //ViewIdNameAssign.Wj1080ModuleWj1080TmlInfoSetViewAttachRegion,
               //               ViewIdNameAssign.Wj1080ModuleWj1080TmlInfoSetViewId,
              
               Wj1080Module .Services .ViewIdAssign .Wj1080TmlInfoSetViewId ,
                              list);
       }
   }
}
