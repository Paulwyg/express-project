namespace Wlst.Ux.EmergencyDispatch.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 88*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 88*100;

        public const int NavToLightEmergencyDispatch = MenuIdAssignBaseId + 1;

        public const int NavToLightFaultShieldViewId = MenuIdAssignBaseId + 2;
        public const int NavToLightEmergencyManag = MenuIdAssignBaseId + 3;

        public const int NavToControlCenterView = MenuIdAssignBaseId + 4;
        public const int NavToControlCenterViewDemo2 = MenuIdAssignBaseId + 5;


        public const int NavToLdlId = MenuIdAssignBaseId + 6;
    }
}
