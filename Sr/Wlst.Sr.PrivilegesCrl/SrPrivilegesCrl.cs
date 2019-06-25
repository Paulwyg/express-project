using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Sr.PrivilegesCrl.Services;

namespace Wlst.Sr.PrivilegesCrl
{
    [ModuleExport(typeof (SrPrivilegesCrl), DependsOnModuleNames = new string[] {"CrCore"})]
    public class SrPrivilegesCrl : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //new Protocols.PPProtocol().RegistProtocol();
            new PrivilegeInfoServer();
        }

        #endregion
    }
}
