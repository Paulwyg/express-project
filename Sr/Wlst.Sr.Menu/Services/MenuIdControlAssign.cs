namespace Wlst.Sr.Menu.Services
{
        /// <summary>
        /// <para> 本模块的菜单Id分配;2 xxx xxx均为菜单地址；部分主菜单、右键菜单、单灯菜单。</para> 
        /// <para>每个模块均有自己的独立的菜单地址，地址跟随模块的模块地址而分配；</para> 
        /// <para>全局地址使用范围为2 100 000~2 899 999；前十万地址保留；后十万地址保留给菜单夹以及菜单实例；</para> 
        /// <para>每个模块的使用全局Id范围为 2 100 000+ ModuleId*100 ~ 2 100 000+ ModuleId*100 +99, </para> 
        /// <para>每个模块均发放99个菜单Id值。</para> 
        /// <para>模块地址请在Assembly中AssemblyFileVersion中查阅，第三地址即为模块地址；如【"1.0.1.0"】模块地址为1。</para> 
        /// </summary>
    public class MenuIdControlAssign
    {
        /// <summary>
        /// 部件菜单的最小ID值 2100000;
        /// </summary>
        public const int MenuIdMin = 2100000;

        /// <summary>
        /// 部件菜单的最大ID值 2899999
        /// </summary>
        public const int MenuIdMax = 2899999;

        /// <summary>
        /// 模板菜单最小ID值  2900001
        /// </summary>
        public const int MenuClassicIdMin = 2900001;

        /// <summary>
        /// 模板菜单最多ID值   2919999
        /// </summary>
        public const int MenuClassicIdMax = 2919999;

        /// <summary>
        /// 目标设备最小ID值  2920001
        /// </summary>
        public const int MenuInstanceKeyIdMin = 2920001;

        /// <summary>
        /// 目标设置ID最大值   2939999
        /// </summary>
        public const int MenuInstancesKeyIdMax = 2939999;

        /// <summary>
        /// 菜单夹最小ID值 2940001
        /// </summary>
        public const int MenuFileGroupIdMin = 2940001;

        /// <summary>
        /// 菜单夹最大ID值 2959999
        /// </summary>
        public const int MenuFileGroupIdMax = 2959999;
    }
}
