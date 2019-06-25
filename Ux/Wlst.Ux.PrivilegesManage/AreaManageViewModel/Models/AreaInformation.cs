using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Ux.PrivilegesManage.AreaManageViewModel.Models
{
    

    public class DrapTmlInformation: EquipmentParameter
    {
        public DrapTmlInformation(EquipmentParameter item)
        {
            this.RtuPhyId = item.RtuPhyId;
            this.RtuName = item.RtuName;
            
        }
    }
}
