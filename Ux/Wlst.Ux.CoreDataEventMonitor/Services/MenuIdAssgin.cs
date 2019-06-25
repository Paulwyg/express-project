namespace Wlst.Ux.CoreDataEventMonitor.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 5*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 5*100;


        public const int NavToEventMonitorViewId = MenuIdAssignBaseId + 1;

        public const int NavToSocketDataMonitorViewId = MenuIdAssignBaseId + 2;
    }
}
