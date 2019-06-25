using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.About
{
    [ModuleExport(typeof (UxAboutModule))]
    public class UxAboutModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }


        #endregion
    }
}
