namespace Wlst.Ux.Wj1090Module.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 27*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 27*100;


        public const int NavToLduInfoSetViewfor1090Id = MenuIdAssignBaseId + 1;
        public const int NavToLduInfoSetViewfor30910Id = MenuIdAssignBaseId + 2;
        public const int NavToLduInfoSetViewfor30920Id = MenuIdAssignBaseId + 3;

        //public const int NavToWj1080ManageViewId = MenuIdAssignBaseId + 2;

        //public const int NavToEquipmentFaultWithTmlSettingViewforMainMenuId = MenuIdAssignBaseId + 3;

        /// <summary>
        /// 此值 永远不许变动  2102704
        /// </summary>
        public const int EventSchduleNavTaskWj1090LduEventScheduleViewId = MenuIdAssignBaseId + 4;

        public const int NavToSetBrightLightBaseId = MenuIdAssignBaseId + 5;

        public const int NavToClearBrightLightBaseId = MenuIdAssignBaseId + 6;

        public const int NavToWj1090LduDataQueryViewModelId = MenuIdAssignBaseId + 7;

        public const int NavLineLduTreeSettingViewId = MenuIdAssignBaseId + 8;

        /// <summary>
        /// 选测集中控制器
        /// </summary>
        public const int MeasureControllerForMenuId = MenuIdAssignBaseId + 9;

        public const int NavWj1090ParaInfoSetMenuId = MenuIdAssignBaseId + 10;
        public const int NavWj1090DataSelectionMenuId = MenuIdAssignBaseId + 11;
    }
}
