namespace Wlst.Cr.CoreOne.CoreIdAssign
{
    /// <summary>
    /// 全局Id说明
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// <para> 本模块的全局事件发布Id 3100000 + 1*100;</para> 
        /// <para>每个模块均有自己的独立的全局事件发布Id地址，地址跟随模块的模块地址而分配；</para> 
        /// <para>全局地址使用范围为3 100 000~3 999 999；前十万地址保留；</para> 
        /// <para>每个模块的使用全局Id范围为 3 100 000+ ModuleId*100 ~ 3 100 000+ ModuleId*100 +99, </para> 
        /// <para>每个模块均发放99个全局事件Id值。</para> 
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 1*100;


        /// <summary>
        /// 程序菜单部件更新，携带参数：更新的菜单部件ID值列表
        /// </summary>
        public const int MenuComponentUpdate = EventIdAssignBaseId + 1;

        /// <summary>
        /// 程序菜单部件删除，携带参数：菜单部件ID值列表
        /// </summary>
        public const int MenuComponentDelete = EventIdAssignBaseId + 2;

        /// <summary>
        /// 程序菜单部件增加，携带参数：菜单部件ID值列表
        /// </summary>
        public const int MenuComponentAdd = EventIdAssignBaseId + 3;

        /// <summary>
        /// 程序界面说明界面
        /// </summary>
        public const int ShowViewInstructionEventId = EventIdAssignBaseId + 7;

        public const int OpenOrCloseLightReceiveEventId = EventIdAssignBaseId + 8;

        public const int AsyncTimeEventId = EventIdAssignBaseId + 9;

    }
}
