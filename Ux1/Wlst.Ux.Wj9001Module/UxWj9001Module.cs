using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.CoreServices;

namespace Wlst.Ux.Wj9001Module
{
   
    [ModuleExport(typeof(UxWj9001Module))]
    public class UxWj9001Module : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //throw new NotImplementedException();
            //new LocalProtocol().RegistProtocol();
            //new GrpMultiInfoHoldingExtend().InitLoad();
            //new GrpSingleInfoHoldingExtend().InitLoad();


            if (Wj9001ManageSetting.ViewModel.Wj9001LoadSet  .Myself.IsShowTreeOnTab)
                Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddTab, 1);
        }


        void AddTab()
        {
            RegionManage.ShowViewByIdAttachRegion(
             Ux.Wj9001Module.Services.ViewIdAssign.Wj9001TreeViewId,
             true);
        }


        #endregion
    }
}
