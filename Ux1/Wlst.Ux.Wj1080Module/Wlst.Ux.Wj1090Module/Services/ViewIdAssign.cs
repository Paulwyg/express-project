using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj1090Module .Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 27*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 27*100;


        public const int LduInfoSetViewId = ViewIdAssignBaseId + 1;

        public const string LduInfoSetViewAttachRegion =
            RegionNames.DocumentRegion;



        public const int LduTreeInfoViewId = ViewIdAssignBaseId + 2;

        public const string LduTreeInfoViewAttachRegion =
            RegionNames.LeftViewRegion;

        public const int Wj1090LduEventScheduleViewId = ViewIdAssignBaseId + 3;

        public const string Wj1090LduEventScheduleViewAttachRegion =
            RegionNames.DocumentRegion;

        public const int Wj1090LduDataQueryViewModelId = ViewIdAssignBaseId + 4;

        public const string Wj1090LduDataQueryViewModelAttachRegion =
            RegionNames.DocumentRegion;


        public const int LduTreeSettingViewId = ViewIdAssignBaseId + 5;

        public const string LduTreeSettingViewAttachRegion =
            RegionNames.DocumentRegion;

        public const int Wj1090ManageViewId = ViewIdAssignBaseId + 6;
        public const string Wj1090ManageViewAttachRegion =
            RegionNames.LeftViewRegion;

        public const int Wj1090ParaInfoSetViewId = ViewIdAssignBaseId + 7;
        public const string Wj1090ParaInfoSetViewAttachRegion =
            RegionNames.DocumentRegion;
        public const int Wj1090DataSelectionViewId = ViewIdAssignBaseId + 8;
        public const string Wj1090DataSelectionViewAttachRegion =
            RegionNames.DocumentRegion;
    }
}
