namespace Wlst.Ux.SinglePlan.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 180*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 180 * 100;


        public const int NavToSinglePlanViewId = MenuIdAssignBaseId + 1;


        public const int NavToSingleGrpViewId = MenuIdAssignBaseId + 2;
    }
}
