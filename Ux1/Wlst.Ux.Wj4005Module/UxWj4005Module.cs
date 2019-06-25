using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
//using Wlst.Ux.WJ3005Module.ZOrders.OpenCloseLight;

namespace Wlst.Ux.Wj4005Module
{
    [ModuleExport(typeof(UxWj4005Module))]
    public class UxWj4005Module : IModule
    {
        #region IModule 成员

        //private OpenCloseLightDataDispatch _openCloseLightDataDispatch;

        public void Initialize()
        {

            //throw new NotImplementedException();

            //_openCloseLightDataDispatch = new OpenCloseLightDataDispatch();
        }
        #endregion
    }
}
