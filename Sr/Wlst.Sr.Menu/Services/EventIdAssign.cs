namespace Wlst.Sr.Menu.Services
{
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 10*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 10*100;

        /// <summary>
        /// 菜单类别控制的菜单实例列表发生变化，可能删除，携带参数：菜单类别地址
        /// </summary>
        public const int ClassicMenuUpdate = EventIdAssignBaseId + 1;

        /// <summary>
        /// 菜单实例信息发生变化，包含删除，携带参数：菜单实例地址
        /// </summary>
        public const int MenuInstanceUpdate = EventIdAssignBaseId + 2;

        /// <summary>
        /// 菜单实例信息发生变化，包含删除节点等，携带参数2个：菜单实例地址，菜单实例关键字
        /// 此为管理菜单实例的下层关系，无法删除节点信息的
        /// </summary>
        public const int MenuInstanceRelationUpdate = EventIdAssignBaseId + 3;

        /// <summary>
        /// 菜单快捷键修改，包含删除该快捷键，携带修改的快捷键地址列表,为多个参数地址
        /// </summary>
        public const int MenuShourtCutsUpdate = EventIdAssignBaseId + 4;


        /// <summary>
        /// 菜单类别控制加载完成，无参数
        /// </summary>
        public const int ClassicMenuLoadUpdate = EventIdAssignBaseId + 5;

        /// <summary>
        /// 菜单实例信息加载完成，无参数
        /// </summary>
        public const int MenuInstanceLoadUpdate = EventIdAssignBaseId + 6;

        /// <summary>
        /// 菜单实例信息加载完成，无参数
        /// </summary>
        public const int MenuInstanceRelationLoadUpdate = EventIdAssignBaseId + 7;

        /// <summary>
        /// 菜单快捷键加载完成，无参数
        /// </summary>
        public const int MenuShourtCutsLoadUpdate = EventIdAssignBaseId + 8;

    }
}
