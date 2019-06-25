using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.Statistics
{
    [ModuleExport(typeof(UxStatisticsModule))]
    public class UxStatisticsModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }


        #endregion
    }
}
