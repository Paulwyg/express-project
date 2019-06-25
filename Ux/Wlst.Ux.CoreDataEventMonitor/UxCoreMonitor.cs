using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.CoreDataEventMonitor
{

    [ModuleExport(typeof(UxCoreMonitor),DependsOnModuleNames = new string[] { "CrCore" } )]
    public class UxCoreMonitor : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }

        #endregion
    }
}
