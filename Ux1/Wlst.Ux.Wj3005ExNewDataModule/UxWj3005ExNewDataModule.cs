using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.Wj3005ExNewDataModule
{
    
    [ModuleExport(typeof(UxWj3005ExNewDataModule))]
    public class UxWj3005ExNewDataModule : IModule
    {

        #region IModule 成员

        public void Initialize()
        {

        }

        #endregion

    }
}
