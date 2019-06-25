using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQuerySZViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQuerySZViewModel.Services
{
  public   interface IIEquipmentFaultRecordQueryViewModel:IINavOnLoad ,IITab ,IIOnHideOrClose
    {

      int CounterLableDoubleClick { get; set; }
      void OnRequestServerData(EquipmentFaultViewModel info);

    }
}
