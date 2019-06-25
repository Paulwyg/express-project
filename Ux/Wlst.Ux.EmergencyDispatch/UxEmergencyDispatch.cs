using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Ux.EmergencyDispatch
{
    [ModuleExport(typeof(UxEmergencyDispatch))]
    public class UxEmergencyDispatch : IModule
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
