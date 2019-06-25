using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.EquipmentDataQuery
{

    [ModuleExport(typeof (UxEquipmentDataQuery))]
    public class UxEquipmentDataQuery : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //new PPProtocol().RegistProtocol();
            //new MainEquipemntInfoHoldingExtend().InitLoad();
            //new AttachEquipemntInfoHoldingExtend().InitLoad();
            //new EquipmentRunningInfoHoldingExtend().InitLoad();
          //  new PPProtocol.PPProtocol().RegistProtocol();
        }

        //View i36N id 11040000~11049999

        #endregion
    }
}
