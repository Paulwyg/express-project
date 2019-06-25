using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.PrivilegesManage
{

    [ModuleExport(typeof (UxPrivilegesManage), DependsOnModuleNames = new[] {"CrCore"})]
    public class UxPrivilegesManage : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
            // new Protocol.PPProtocol().RegistProtocol();
        }

        #endregion
    }
}
