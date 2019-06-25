namespace Wlst.Ux.About.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 67*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 67 * 100;

        public const int UxAboutViewId = ViewIdAssignBaseId + 1;

        public const int UxShowErrViewId = ViewIdAssignBaseId + 2;

        public const int UxThreeLvViewId = ViewIdAssignBaseId + 3;
    }
}
