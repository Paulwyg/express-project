using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel;
using Wlst.Ux.EquipemntLightFault.Services;

namespace Wlst.Ux.EquipemntLightFault
{

    [ModuleExport(typeof (UxEquipemntLightFault))]
    public class UxEquipemntLightFault : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            NavToCurrentEquipmentFaultView.InitWin();
            //new PPProtocol.PPProtocol().RegistProtocol();
        }

        #endregion
    }
}
