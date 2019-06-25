using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Ux.WJ3005Module.ZOrders.OpenCloseLight;

namespace Wlst.Ux.WJ3005Module
{
    [ModuleExport(typeof (UxWj3005Module))]
    public class UxWj3005Module : IModule
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