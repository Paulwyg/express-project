namespace Wlst.Cr.CoreOne.CoreIdAssign
{

        /// <summary>
    /// 权限验证Button Id规划；菜单中的4110XX全部保留系统使用
        /// </summary>
       public class PrivilegeButtonIdAssign
        {
            /// <summary>
            /// <para> 本模块的权限验证Button Id分配;</para> 
            /// <para>每个模块均有自己的独立的权限验证Button地址，地址跟随模块的模块地址而分配；</para> 
            /// <para>全局地址使用范围为4 100 000~4 999 999</para> 
            /// <para>每个模块的使用全局Id范围为 4 100 000+ ModuleId*100 ~ 4 100 000+ ModuleId*100 +99, </para> 
            /// <para>每个模块均发放99个权限验证Button Id值。</para> 
            /// </summary>
            public const int MenuIdAssignId = 0;

        }
    
}
