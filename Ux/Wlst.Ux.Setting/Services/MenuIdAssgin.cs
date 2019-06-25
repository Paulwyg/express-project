namespace Wlst.Ux.Setting.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 53*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 53*100;


        public const int NavToSettingViewId = MenuIdAssignBaseId + 1;

        public const int NavToEventTaskViewId = MenuIdAssignBaseId + 2;


        public const int NavToNavToRecordTaskQueryViewId = MenuIdAssignBaseId + 3;

        public const int NavToSystemInformationViewId = MenuIdAssignBaseId + 6;
    }
}
