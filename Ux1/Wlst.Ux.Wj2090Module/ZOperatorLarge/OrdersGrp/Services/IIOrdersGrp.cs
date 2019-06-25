using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.OrdersGrp.Services
{
   public  interface IIOrdersGrp : IITab, IINavOnLoad, IIOnHideOrClose
   {
       void Query();
       void SelectAllSwitchOut(int kx);
   }
}
