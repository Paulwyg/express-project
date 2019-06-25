using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Ux.Nr6005Module.ZOrders.OpenCloseLight;

namespace Wlst.Ux.Nr6005Module
{
    [ModuleExport(typeof (UxNr6005Module))]
    public class UxNr6005Module : IModule
    {
        //lan 1110 0000

        #region IModule 成员

        private OpenCloseLightDataDispatch _openCloseLightDataDispatch;

        public void Initialize()
        {

            //throw new NotImplementedException();

            _openCloseLightDataDispatch = new OpenCloseLightDataDispatch();
        }

        #endregion

    }
}