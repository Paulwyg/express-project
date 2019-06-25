using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.AddMainEquipment
{
    [ModuleExport(typeof(UxAddMainEquipment), DependsOnModuleNames = new[] { "CrCore", "SrEquipmentInfoHolding" })]
    public class UxAddMainEquipment : IModule
    {
        #region IModule 成员

        public void Initialize()
        {

        }

        #endregion
    }
}
