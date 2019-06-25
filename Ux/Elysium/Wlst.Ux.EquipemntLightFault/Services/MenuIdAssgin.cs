namespace Wlst.Ux.EquipemntLightFault.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 36*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 36*100;


        public const int NavToEquipmentFaultDefineSettingViewId = MenuIdAssignBaseId + 1;

        public const int NavToEquipmentFaultRecordQueryViewId = MenuIdAssignBaseId + 2;

        public const int NavToEquipmentFaultWithTmlSettingViewforMainMenuId = MenuIdAssignBaseId + 3;

        public const int NavToUserIndividuationFaultSettingViewId = MenuIdAssignBaseId + 4;

        public const int NavToFaultDefineSettingManagViewId = MenuIdAssignBaseId + 5;

        public const int NavToEquipmentFaultWithTmlAlarmMsgViewId = MenuIdAssignBaseId + 6;

        public const int NavToRecordMsgViewId = MenuIdAssignBaseId + 7;

        public const int NavToEquipmentTmlMobileViewId = MenuIdAssignBaseId + 8;

        public const int NavToEquipmentFaultRecordQueryViewTmlRightMenuId = MenuIdAssignBaseId + 9;

        public const int NavToRtuAmpSxxViewId = MenuIdAssignBaseId + 10;

        public const int NavToCurrentEquipmentFaultViewId = MenuIdAssignBaseId + 11;
    }
}
