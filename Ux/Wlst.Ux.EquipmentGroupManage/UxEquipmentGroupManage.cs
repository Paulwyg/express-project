using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.EquipmentGroupManage
{
    [ModuleExport(typeof(UxEquipmentGroupManage))]
    public class UxEquipmentGroupManage : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }

        //View i36N id 11040000~11049999

        #endregion
    }
}
