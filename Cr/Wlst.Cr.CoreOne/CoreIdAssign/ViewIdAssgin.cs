using Wlst.Cr.CoreOne.Services;

namespace Wlst.Cr.CoreOne.CoreIdAssign
{
    /// <summary>
    /// 视图地址分配
    /// </summary>
    public class ViewIdAssgin
    {
        /// <summary>
        /// <para> 本模块的视图Id分配;</para> 
        /// <para>每个模块均有自己的独立的视图地址，地址跟随模块的模块地址而分配；</para> 
        /// <para>全局地址使用范围为1 100 000~1 999 999；前十万地址保留；</para> 
        /// <para>每个模块的使用全局Id范围为 1 100 000+ ModuleId*100 ~ 1 100 000+ ModuleId*100 +99, </para> 
        /// <para>每个模块均发放99个视图Id值。</para> 
        /// </summary>
        public const int ViewIdAssignId = 2100000;

        public const int MainViewSettingViewId = ViewIdAssignId + 1;
       
    }
}
