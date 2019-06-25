using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule
{

    [ModuleExport(typeof(Wj3005ExNewDataExcelModule))]
    public class Wj3005ExNewDataExcelModule : IModule
    {

        #region IModule 成员

        public void Initialize()
        {

        }

        #endregion

    }
}
