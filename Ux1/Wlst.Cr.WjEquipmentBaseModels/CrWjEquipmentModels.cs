using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Cr.WjEquipmentBaseModels
{
    [ModuleExport(typeof(CrWjEquipmentModels))]
    public class CrWjEquipmentModels : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }

        #endregion
    };
}
