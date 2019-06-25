using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.Setting
{
    [ModuleExport(typeof(UxSetting), DependsOnModuleNames = new string[] { "CrCore", "CrSetting" })]
    public class UxSetting : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //new Protocol.PPProtocol().RegistProtocol();
        }

        #endregion
    }
}
