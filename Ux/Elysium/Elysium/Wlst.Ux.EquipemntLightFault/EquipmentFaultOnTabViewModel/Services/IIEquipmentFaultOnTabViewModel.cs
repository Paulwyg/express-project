using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.Services
{
   public  interface IIEquipmentFaultOnTabViewModel:IITab
   {
       void OnRequestServerData(OneTmlExistFaultViewModel info);
    }
}
