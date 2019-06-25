using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.RadMapJpeg.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 35*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 35*100;


        public const int MapJpegViewId = ViewIdAssignBaseId + 1;

        public const string MapJpegViewAttachRegion =
            RegionNames.MapRegion  ;


        public const int SettingViewId = ViewIdAssignBaseId + 2;

        public const int EquViewId = ViewIdAssignBaseId + 3;

        public const string EquViewAttachRegion =
            RegionNames.RightViewRegion;

    }
}
