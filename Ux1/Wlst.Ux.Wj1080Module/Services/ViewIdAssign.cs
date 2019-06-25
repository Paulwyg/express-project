using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj1080Module.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 26*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 26*100;


        public const int Wj1080TmlInfoSetViewId = ViewIdAssignBaseId + 1;

  

        public const int Wj1080ManageViewId = ViewIdAssignBaseId + 2;

        public const string Wj1080ManageViewAttachRegion =
            RegionNames.LeftViewRegion;

        public const int Wj1080ManageSettingViewId = ViewIdAssignBaseId + 3;

        public const string Wj1080ManageSettingViewAttachRegion =
            RegionNames.LeftViewRegion;

        public const int PartolEventScheduleViewId = ViewIdAssignBaseId + 4;



        public const int LuxOnTabViewId = ViewIdAssignBaseId + 5;
        public const string LuxOnTabViewAttachRegion =
            RegionNames.MsgRegion;
    }
}
