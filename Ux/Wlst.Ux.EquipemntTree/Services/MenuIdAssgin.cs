namespace Wlst.Ux.EquipemntTree.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 40*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 40 * 100;


        public const int NavLineTreeSettingViewId = MenuIdAssignBaseId + 1;

        //public const int NavToWj1080ManageViewId = MenuIdAssignBaseId + 2;

        //public const int NavToEquipmentFaultWithTmlSettingViewforMainMenuId = MenuIdAssignBaseId + 3;
    }
}
