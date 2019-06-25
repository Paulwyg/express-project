namespace Wlst.Ux.EquipmentGroupManage.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 42*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 42*100;


        public const int NavToGrpMulitManageViewId = MenuIdAssignBaseId + 1;

        public const int NavToGrpSingleManageViewId = MenuIdAssignBaseId + 2;


        
    }
}
