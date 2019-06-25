namespace Wlst.Ux.MenuManage.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 11*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 11*100;


        public const int NavToMenuClassicViewId = MenuIdAssignBaseId + 1;

        public const int NavToMenuInstanceRelationViewId = MenuIdAssignBaseId + 2;

        //2101110 该ID值已经被CETC50_Demo NvaToAreaSets Menu占用，请勿使用该ID；
    }
}
