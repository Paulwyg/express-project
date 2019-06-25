using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultManageSettingLnViewModel
{
    [Export(typeof(IISetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class EquipmentFaultManageSettingView : SettingBase
    {
        public EquipmentFaultManageSettingView()
        {
            this.Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultManageSettingLnViewId;
            this.ViewId = EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultManageSettingLnViewId;
            this.PathSetting = "火零不平衡选项";
        }
    }
}
