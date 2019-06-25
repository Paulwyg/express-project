using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLnViewModel.Services
{
    public interface IIEquipmentFaultRecordQueryLnViewModel : IINavOnLoad, IITab, IIOnHideOrClose
    {
        int CounterLableDoubleClick { get; set; }
        void OnRequestServerData(EquipmentFaultViewModel info);
    
    }
}
