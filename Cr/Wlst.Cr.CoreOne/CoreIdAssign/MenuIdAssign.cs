namespace Wlst.Cr.CoreOne.CoreIdAssign
{
    /// <summary>
    /// 菜单Id规划；菜单中的2110XX全部保留系统使用
    /// </summary>
    public class MenuIdAssign
    {
        /// <summary>
        /// <para> 本模块的菜单Id分配;</para> 
        /// <para>每个模块均有自己的独立的菜单地址，地址跟随模块的模块地址而分配；</para> 
        /// <para>全局地址使用范围为2 100 000~2 899 999；前十万地址保留；后十万地址保留给菜单夹以及菜单实例；</para> 
        /// <para>每个模块的使用全局Id范围为 2 100 000+ ModuleId*100 ~ 2 100 000+ ModuleId*100 +99, </para> 
        /// <para>每个模块均发放99个菜单Id值。</para> 
        /// </summary>
        public const int MenuIdAssignId = 0;

        public const int NavMainViewSettingViewId = 2101110;

    }
}
