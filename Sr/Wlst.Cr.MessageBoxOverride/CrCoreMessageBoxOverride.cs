using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Cr.MessageBoxOverride
{
     [ModuleExport(typeof(CrCoreMessageBoxOverride))]
    public class CrCoreMessageBoxOverride : IModule
    {

        #region IModule 成员

        public void Initialize()
        {

        }

        #endregion
    }
}
