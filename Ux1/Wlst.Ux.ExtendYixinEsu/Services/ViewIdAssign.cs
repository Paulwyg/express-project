using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.ExtendYixinEsu.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 401*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 401*100;


        public const int JnRtuSetViewId = ViewIdAssignBaseId + 1;

    
        public const int JnQueryViewId = ViewIdAssignBaseId + 2;

       
        public const int TreeTabRtuSetViewId = ViewIdAssignBaseId + 3;

       


        public const int TabTreeViewAttachIdStart = ViewIdAssignBaseId + 4;
        public const int TabTreeViewAttachIdEnd = ViewIdAssignBaseId + 8;

        public const string TabTreeViewAttachRegion =
            RegionNames.LeftViewRegion;
    }
}
