using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.Services
{
         public  interface IIEquipmentAllFaultOnTabViewModel:IITab
         {
             void ClearVoiceReportItems();
             void OnRequestServerData(OneTmlExistFaultViewModel info);
         }
}
