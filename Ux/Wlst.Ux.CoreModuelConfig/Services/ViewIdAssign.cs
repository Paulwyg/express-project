namespace Wlst.Ux.CoreModuelConfig.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 7*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 7*100;


        public const int CoreModuleConfigViewId = ViewIdAssignBaseId + 1;
    }
}
