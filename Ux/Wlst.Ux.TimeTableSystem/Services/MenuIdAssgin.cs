namespace Wlst.Ux.TimeTableSystem.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 32*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 32*100;


        public const int NavToTimeTableBandingViewId = MenuIdAssignBaseId + 1;

        public const int NavToTimeTableSetId = MenuIdAssignBaseId + 2;

        //public const int NavToEquipmentFaultWithTmlSettingViewforMainMenuId = MenuIdAssignBaseId + 3;

        public const int NavToTimeTableManageViewId = MenuIdAssignBaseId + 3;

        public const int NavToOpenCloseReportQueryViewId = MenuIdAssignBaseId + 4;

        public const int NavToHolidayTimeSetViewId = MenuIdAssignBaseId + 5;

        public const int NavToTimeTabletemporaryViewId = MenuIdAssignBaseId + 6;

        public const int NavToNavToTimeInfoMnViewId = MenuIdAssignBaseId + 7;

        public const int NavToNavToTimeInfoMnGrpViewId = MenuIdAssignBaseId + 8;

        public const int NavToTunnelInfoSetViewId = MenuIdAssignBaseId + 9;

        public const int NavToNavToTimeInfoNewGrpViewId = MenuIdAssignBaseId + 10;

    }
}
