using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.StateBarModule
{

    [ModuleExport(typeof(UxTopDataModule))]
    public class UxTopDataModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
