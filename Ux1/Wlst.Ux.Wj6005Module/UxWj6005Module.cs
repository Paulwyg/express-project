using System.Threading;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj6005Module
{
    [ModuleExport(typeof (UxWj6005Module))]
    public class UxWj6005Module : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
            //new LocalProtocol().RegistProtocol();
            //new GrpMultiInfoHoldingExtend().InitLoad();
            //new GrpSingleInfoHoldingExtend().InitLoad();


            if (Jd601ManageSettingViewModel.ViewModel.Jd601LoadSet.Myself.IsShowTreeOnTab)
                Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddTab,1);
        }


        void AddTab()
        {
            RegionManage.ShowViewByIdAttachRegion(
             Ux.Wj6005Module.Services.ViewIdAssign.Jd601ManageViewId,
             true);
        }


        #endregion
    }
}
