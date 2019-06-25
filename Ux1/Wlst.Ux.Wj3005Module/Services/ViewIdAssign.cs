using Wlst.Cr.CoreOne.Services;

namespace Wlst.Ux.WJ3005Module .Services
{
    public class ViewIdAssign
    {
        /// <summary>
        /// 本模块的视图起始Id，1100000 + 28*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int ViewIdAssignBaseId = 1100000 + 28*100;


        public const int Wj3005TmlInfoSetViewId = ViewIdAssignBaseId + 1;

       

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

        public const int TmlInfoValidQueryView = ViewIdAssignBaseId + 18;

         public const int UserOcInfoViewId = ViewIdAssignBaseId + 19;


         public const int NavToLnEmergencyOperationCenterViewId = ViewIdAssignBaseId + 20;




        public const string NewSvrUserOcInfoViewAttachRegion =
            RegionNames.MsgRegion;


        /// <summary>
        /// 应急设备
        /// </summary>
        public const int NavToEmergencyEquipmentViewId = ViewIdAssignBaseId + 21;

        public const int NavToEmergencyOperationQueryViewId = ViewIdAssignBaseId + 22;

        public const int NavToTmlLoopsQueryViewId = ViewIdAssignBaseId + 23;

        public const int NavToElectricityQueryViewId = ViewIdAssignBaseId + 24;

        public const int NavToElectricityStatisticsViewId = ViewIdAssignBaseId + 25;

        //lvf 2018年9月6日09:09:13  应急终端巡测
        public const int NavToEmergencyOperationNewDataViewId = ViewIdAssignBaseId + 26;

        //lvf 2018年11月8日08:42:59 测试 http 请求数据界面
        public const int EquipmentDailyDataQueryHttpViewId = ViewIdAssignBaseId + 27;

    }
}
