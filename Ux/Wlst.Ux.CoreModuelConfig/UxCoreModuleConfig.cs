using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.CoreModuelConfig
{
    [ModuleExport(typeof (UxCoreModuleConfig) ,DependsOnModuleNames = new string[] { "CrCore" })]
    public class UxCoreModuleConfig : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }

        #endregion
    }
}
