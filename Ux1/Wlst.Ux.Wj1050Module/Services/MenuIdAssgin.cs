namespace Wlst.Ux.Wj1050Module.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 25*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 25*100;


        public const int NavToWj1050InfoSetViewId = MenuIdAssignBaseId + 1;

        //public const int NavToWj1050ManageViewId = MenuIdAssignBaseId + 2;

        public const int EventSchduleNavTaskWj1050MruEventScheduleViewId = MenuIdAssignBaseId + 3;

        public const int NavLineWj1050ManageSettingViewId = MenuIdAssignBaseId + 4;

        public const int NavToWj1050DataQueryViewId = MenuIdAssignBaseId + 5;
        public const int NavToWj1050DataQueryViewMainId = MenuIdAssignBaseId + 6;
    }
}
