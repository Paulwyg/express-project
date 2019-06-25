using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.AssetManagementModule
{
    [ModuleExport(typeof(UxAssetManagementModule))]
    public class UxAssetManagementModule:IModule
    {
        #region IModule 成员

        public void Initialize()
        {
           
        }

 

        #endregion
    }
}
