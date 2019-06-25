using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.StateBarModule.StateBarInBottom
{
    [ModuleExport(typeof (UxStateBarModule))]
    public class UxStateBarModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}