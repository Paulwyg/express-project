using System.Threading;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj1090Module
{
    [ModuleExport(typeof(UxWj1090Module))]
    public class UxWj1090Module : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
            //new LocalProtocol().RegistProtocol();
            //new GrpMultiInfoHoldingExtend().InitLoad();
            //new GrpSingleInfoHoldingExtend().InitLoad();


            if (Wj1090Module.LduTreeSettingViewModel.ViewModel.Wj1090TreeSetLoad.Myself.IsShowTreeOnTab)
                Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddTab, 1);
        }


        void AddTab()
        {
            //RegionManage.ShowViewByIdAttachRegion(
            //     Ux.Wj1090Module.Services.ViewIdAssign.LduTreeInfoViewId,
            //     Ux.Wj1090Module.Services.ViewIdAssign.LduTreeInfoViewAttachRegion,
            //     true);
            RegionManage.ShowViewByIdAttachRegion(
            Ux.Wj1090Module.Services.ViewIdAssign.Wj1090ManageViewId,
            Ux.Wj1090Module.Services.ViewIdAssign.Wj1090ManageViewAttachRegion,
            true);
        }

        #endregion
    }
}
