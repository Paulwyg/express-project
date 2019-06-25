using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.MenuNewDraw.Services
{

    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 65*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 65*100;


        public const int MenuViewId = ViewIdAssignBaseId + 1;

        public const string MenuViewAttachRegion =
            RegionNames.MenuViewRegion;

    }
}
