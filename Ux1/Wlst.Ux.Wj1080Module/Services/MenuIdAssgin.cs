namespace Wlst.Ux.Wj1080Module.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 26*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 26*100;


        public const int NavToWj1080TmlInfoSetId = MenuIdAssignBaseId + 1;

        public const int NavLineJd601ManageSettingView = MenuIdAssignBaseId + 2;

        //public const int NavToEquipmentFaultWithTmlSettingViewforMainMenuId = MenuIdAssignBaseId + 3;

        public const int NavTaskPartolEventScheduleView = MenuIdAssignBaseId + 3;

        public const int NavMsgRegionLuxOnTabView = MenuIdAssignBaseId + 4;

        public const int NavToWj1080DataStatisticsId = MenuIdAssignBaseId + 5;
    }
}
