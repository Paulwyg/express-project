namespace Wlst.Ux.Wj3005ExNewDataExcelModule.Services
{
    public class MenuIdAssgin
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 64*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 64*100;

        /// <summary>
        /// 最新数据设置
        /// </summary>
        public const int NavLineNewDataSettingViewId = MenuIdAssignBaseId + 81;
       
    }
}
