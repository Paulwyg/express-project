using System.Threading;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj1050Module
{
    /// <summary>
    /// 抄表模块
    /// </summary>
    [ModuleExport(typeof(UxWj1050Module))]
    public class UxWj1050Module : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
            //new LocalProtocol().RegistProtocol();
            //new GrpMultiInfoHoldingExtend().InitLoad();
            //new GrpSingleInfoHoldingExtend().InitLoad();



            if (Wj1050Module.Wj1050ManageSettingViewModel.ViewModel.Wj1050TreeSetLoad.Myself.IsShowTreeOnTab)
                Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddTab, 0);
        }


        void AddTab()
        {
            RegionManage.ShowViewByIdAttachRegion(
                Ux.Wj1050Module.Services.ViewIdAssign.Wj1050ManageViewId,
           
                true);
        }

        #endregion
    }
}
