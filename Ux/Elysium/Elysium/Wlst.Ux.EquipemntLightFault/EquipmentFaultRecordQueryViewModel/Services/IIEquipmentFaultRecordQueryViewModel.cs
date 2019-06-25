using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.Services
{
  public   interface IIEquipmentFaultRecordQueryViewModel:IINavOnLoad ,IITab ,IIOnHideOrClose
    {

      int CounterLableDoubleClick { get; set; }
      void OnRequestServerData(EquipmentFaultViewModel info);
    }
}
