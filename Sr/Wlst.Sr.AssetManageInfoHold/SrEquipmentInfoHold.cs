using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Sr.AssetManageInfoHold.Services;

namespace Wlst.Sr.AssetManageInfoHold
{
    [ModuleExport(typeof(SrEquipmentInfoHold))]
    public class SrEquipmentInfoHold : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            LampInfoHold.InitLoad();
            SimInfoHold.InitLoad();
        }


        #endregion
    }
}