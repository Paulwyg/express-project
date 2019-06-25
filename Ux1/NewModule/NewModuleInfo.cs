using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace NewModule
{
  

    [ModuleExport(typeof(NewModuleInfo))]
    public class NewModuleInfo : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
         
        }
        #endregion
    }
}
