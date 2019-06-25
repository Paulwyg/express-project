using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLevelLowViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLevelLowViewModel.Services
{
    public interface IIEquipmentFaultRecordQueryViewModel : IITab, IIOnHideOrClose  //IINavOnLoad ,
    {

      int CounterLableDoubleClick { get; set; }
      void OnRequestServerData(EquipmentFaultViewModel info);

    }
}
