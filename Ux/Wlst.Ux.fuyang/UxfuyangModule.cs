using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.fuyang
{
    [ModuleExport(typeof(UxfuyangModule))]
    public class UxfuyangModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        {}

        #endregion
    }
}
