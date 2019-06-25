namespace Wlst.Ux.ExtendYixinEsu.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 401*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 401 * 100;


        public const int NavToJnQueryViewId = MenuIdAssignBaseId + 1;

        public const int NavToTreeTabRtuSetId = MenuIdAssignBaseId + 2;
    }
}
