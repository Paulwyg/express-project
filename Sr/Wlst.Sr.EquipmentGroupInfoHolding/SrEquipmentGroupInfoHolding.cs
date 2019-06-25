using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

using Wlst.Sr.EquipmentGroupInfoHolding.Services;

namespace Wlst.Sr.EquipmentGroupInfoHolding
{
    [ModuleExport(typeof (SrEquipmentGroupInfoHolding))]
    public class SrEquipmentGroupInfoHolding : IModule
    {
        #region IModule 成员



        public void Initialize()
        {
           // new PPProtocol().RegistProtocol();
            //new GrpSingleInfoHoldExtend().InitLoad();

           // ServicesGrpSingleInfoHold.InitLoad();
        }

        #endregion
    }
}
