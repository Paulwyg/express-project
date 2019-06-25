using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 64*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 64*100;





        public const int TmlNewDataViewId = ViewIdAssignBaseId + 9;
        public const int TmlNewDataViewLeftId = ViewIdAssignBaseId + 10;
        public const string TmlNewDataViewAttachRegion =
            RegionNames.DataRegion;




        public const int NewDataSettingViewId = ViewIdAssignBaseId + 11;

    }
}
