using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.SinglePlan
{
    [ModuleExport(typeof(UxSinglePlanModule))]
    public class UxSinglePlanModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        { }

        #endregion
    }
}
