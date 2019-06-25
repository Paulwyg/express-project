using System.Threading;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj1080Module
{
    /// <summary>
    /// 光控模块
    /// </summary>
    [ModuleExport(typeof(UxWj1080Module))]
    public class UxWj1080Module : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
            //new LocalProtocol().RegistProtocol();
            //new GrpMultiInfoHoldingExtend().InitLoad();
            //new GrpSingleInfoHoldingExtend().InitLoad();


            if (Wj1080Module.Wj1080ManageSettingViewModel.ViewModel.wj1080TreeSetLoad .Myself.IsShowTreeOnTab)
                Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddTab, 0);
        }


        void AddTab()
        {
            RegionManage.ShowViewByIdAttachRegion(
               Ux.Wj1080Module.Services.ViewIdAssign.Wj1080ManageViewId,
            
               true);
        }



        #endregion
    }
}
