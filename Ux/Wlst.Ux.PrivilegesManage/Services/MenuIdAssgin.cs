namespace Wlst.Ux.PrivilegesManage.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 57*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 57*100;


        public const int NavToSelfInfoChangeViewId = MenuIdAssignBaseId + 1;

        public const int NavToModflyOtherUserInfoViewId = MenuIdAssignBaseId + 2;

        public const int NavToUserManageViewId = MenuIdAssignBaseId + 3;

        public const int NavToUserAndPrivilegeManageViewId = MenuIdAssignBaseId + 4;

        public const int NavToAreaManageViewId = MenuIdAssignBaseId + 5;

    }
}
