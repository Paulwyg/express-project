using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.EquipmentInfo
{
    [ModuleExport(typeof(UxEquipmentInfo))]
    public class UxEquipmentInfo : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
