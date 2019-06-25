namespace Wlst.Ux.Statistics.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 68*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 68 * 100;

        public const int UxStatisticsViewId = ViewIdAssignBaseId + 1;

        public const int UxRtuElectricityStatisticsViewId = ViewIdAssignBaseId + 2;

        //
        public const int UxDataStatisticsViewId = ViewIdAssignBaseId + 3;
    }
}
