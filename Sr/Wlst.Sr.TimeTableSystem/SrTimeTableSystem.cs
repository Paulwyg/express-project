using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Wlst.Sr.TimeTableSystem
{
    [ModuleExport(typeof (SrTimeTableSystem))]
    public class SrTimeTableSystem : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            ////throw new NotImplementedException();
            //WeekSetInfoHolding.InitLoad();
           // new PPProtocol.PPProtocol().RegistProtocol();
            //new DataHoldingExtend.SunRiseSetHoldingExtend().InitLoad();
            //new DataHoldingExtend.TmlLoopBelongTimeTableDataHoldingExtend().InitLoad();
            //new DataHoldingExtend.WeekTimeTableInfoHoldingExtend().InitLoad();
            //new DataHoldingExtend.WeekTimeTableInfoHoldingThreeEvent().InitLoad();


            Services.SunRiseSetInfoServices.InitService();
            Services.WeekTimeTableInfoService.InitService();
            //Services.TmlLoopBngTimeTableInfoService.InitService();
            Services.HolidayTimeandBandingServices.Myself.InitService();
            Services.TimeTabletemporaryHold.Myself.Init();
        }

        #endregion
    }
}
