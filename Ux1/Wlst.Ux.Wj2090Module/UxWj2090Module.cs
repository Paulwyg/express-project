using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.Wj2090Module.SrInfo;

namespace Wlst.Ux.Wj2090Module
{

    [ModuleExport(typeof (UxWj2090Module))]
    public class UxWj2090Module : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            TimeInfos.MySelf.Init();
         //   CtrGrpInfo.MySelf.Init();
           // NewDataInfo.MySelf.Init();
            //SluGrpInfoHold.MySelf.Init();
            if (Wj2090Module.TreeTab.Set.Wj2090TreeSetLoad.Myself.IsShowTreeOnTab)
                Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddTab, 0);

            new Wlst.Cr.CoreMims.FileSyncWithSvr.FileSyncWithSvr().InitAciotn();
        }


        private void AddTab()
        {
            RegionManage.ShowViewByIdAttachRegion(
                Ux.Wj2090Module.Services.ViewIdAssign.Wj2090TreeViewId, true);
        }

        #endregion
    }
}
