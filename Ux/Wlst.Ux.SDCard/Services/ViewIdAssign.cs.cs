namespace Wlst.Ux.SDCard.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 78*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 78 * 100;

        public const int UxSDCardQueryViewId = ViewIdAssignBaseId + 1;
    }
}
