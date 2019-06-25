using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj2096Module
{

    [ModuleExport(typeof (UxWj2096Module))]
    public class UxWj2096Module : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //TimeInfos.MySelf.Init();
            if (Ux.Wj2096Module.TreeTab.Set.Wj2096TreeSetLoad.Myself.IsShowTreeOnTab)
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddTab, 0);


 
        }


        private void AddTab()
        {
            RegionManage.ShowViewByIdAttachRegion(
             Ux.Wj2096Module.Services.ViewIdAssign.TreeTabVeiwId, true);
        }

        #endregion
    }
}
