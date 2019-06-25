

using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.WJ4005Module .Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 69*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 69*100;


        public const int Wj4005TmlInfoSetViewId = ViewIdAssignBaseId + 1;

       

        public const int OpenCloseEventScheduleViewId = ViewIdAssignBaseId + 2;



        public const int ZhaoCeRtuInfoViewId = ViewIdAssignBaseId + 3;

        //public const int ZhaoCeRtuWeekSetViewId = ViewIdAssignBaseId + 4;


        public const int PartolViewId = ViewIdAssignBaseId + 5;

        public const int PartolEventScheduleViewId = ViewIdAssignBaseId + 6;

        public const int EquipmentDailyDataQueryViewId = ViewIdAssignBaseId + 7;

        public const int SndWeekTimeQueryViewId = ViewIdAssignBaseId + 8;

        public const int TmlNewDataViewId = ViewIdAssignBaseId + 9;

        public const string TmlNewDataViewAttachRegion =
            RegionNames.DataRegion;



        public const int ZhaoCeRtuHolidaySetViewId = ViewIdAssignBaseId + 10;


        public const int NewDataSettingViewId = ViewIdAssignBaseId + 11;

        public const int RtuOpenCloseLightQueryViewId = ViewIdAssignBaseId + 12;

        //RtuOpenCloseLightSuccQueryView
        public const int RtuOpenCloseLightSuccQueryViewId = ViewIdAssignBaseId + 13;



        public const int NavToControlCenterManagDemo2Id = ViewIdAssignBaseId + 14;


        public const int NavToLdlViewId = ViewIdAssignBaseId + 15;

        public const int ZhaoCeRtuWeekSetViewIdFor4 = ViewIdAssignBaseId + 16;


        public const int NavToBatchStopView = ViewIdAssignBaseId + 17;


    }
}
