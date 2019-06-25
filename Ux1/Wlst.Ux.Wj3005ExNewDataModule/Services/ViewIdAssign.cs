using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.Wj3005ExNewDataModule.Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 63*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 28*100;





        public const int TmlNewDataViewId = ViewIdAssignBaseId + 9;

        public const string TmlNewDataViewAttachRegion =
            RegionNames.DataRegion;




        public const int NewDataSettingViewId = ViewIdAssignBaseId + 11;

        public const string NewDataSettingViewAttachRegion =
            RegionNames.DocumentRegion;


    }
}
