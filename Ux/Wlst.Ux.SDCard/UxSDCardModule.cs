using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.SDCard
{
    [ModuleExport(typeof (UxSDCardModule))]
    public class UxSDCardModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }


        #endregion
    }
}
