using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.CrissCrossEquipemntTree.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 192*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 192 * 100;


        public const int GrpMulityTabShowViewId = ViewIdAssignBaseId + 1;

        public const string GrpMulityTabShowViewAttachRegion =
            RegionNames.LeftViewRegion ;


        public const int GrpSingleTabShowViewId = ViewIdAssignBaseId + 2;

        public const string GrpSingleTabShowViewAttachRegion =
            RegionNames.LeftViewRegion ;

        public const int SettingViewId = ViewIdAssignBaseId + 3;

        public const int GrpRegionTabShowViewId = ViewIdAssignBaseId + 4;
        
    }
}
