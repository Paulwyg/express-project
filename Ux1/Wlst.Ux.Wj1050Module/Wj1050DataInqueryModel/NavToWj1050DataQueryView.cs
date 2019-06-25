using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;



namespace Wlst.Ux.Wj1050Module.Wj1050DataInqueryModel
{
   [Export(typeof(IIMenuItem))]
   [PartCreationPolicy(CreationPolicy.Shared)]
   public class NavToWj1050DataQueryView : MenuItemBase
   {
       public NavToWj1050DataQueryView()
       {
           Id = Wj1050Module.Services.MenuIdAssgin.NavToWj1050DataQueryViewMainId;// Infrastructure.IdAssign.MenuIdAssign.NavToWj1050InfoSetViewId;
           Text  = "抄表数据查询";
           Tag = "WJ1050抄表数据查询";
           Classic = "主菜单";
           Description = "WJ1050抄表数据查询，ID 为" + Wj1050Module.Services.MenuIdAssgin.NavToWj1050DataQueryViewMainId;
           Tooltips = "WJ1050抄表参数设置";
           IsEnabled = true;
           IsCheckable = false;
           base.Command = new RelayCommand(Ex );
       }
       public override bool IsCanBeShowRwx()
       {
           return true;
       }
       protected void Ex()
       {
           
           //var equipment = Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
           //if (equipment == null) return;
           //int rtuId = equipment.RtuId;
           //if (rtuId < 1) return;
           ExNavWithArgs(
               Wj1050Module .Services .ViewIdAssign.Wj1050DataInqueryViewId ,
                              0);
       }
   }
}
