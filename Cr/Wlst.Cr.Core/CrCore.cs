using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions.Modularity;

namespace Wlst.Cr.Core
{

    [ModuleExport(typeof (CrCore))]
    public class CrCore : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
           // ModuleLoadManager.SelfLoadConfigViewByAuto();
        }

        #endregion
    }
}
