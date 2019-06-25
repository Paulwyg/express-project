using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;


namespace Wlst.Ux.TimeTableSystem
{
    [ModuleExport(typeof (UxTimeTableSystemModule))]
    public class UxTimeTableSystemModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            //////throw new NotImplementedException();
            ////WeekSetInfoHolding.InitLoad();
            //new Wlst.Sr.TimeTableSystem.PPProtocol.PPProtocol().RegistProtocol();
            //new SunRiseSetHoldingExtend().InitLoad();
            //new TmlLoopBelongTimeTableDataHoldingExtend().InitLoad();
            //new WeekTimeTableInfoHoldingExtend().InitLoad();
            ////new DataHoldingExtend.WeekTimeTableInfoHoldingThreeEvent().InitLoad();
        }

        #endregion
    }
}