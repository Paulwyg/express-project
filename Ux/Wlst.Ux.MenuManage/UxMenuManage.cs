using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.MenuManage
{
    [ModuleExport(typeof(UxMenuManage), DependsOnModuleNames = new string[] { "CrCore", "SrMenu" })]
    public class UxMenuManage : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }



        #endregion
    }
}
