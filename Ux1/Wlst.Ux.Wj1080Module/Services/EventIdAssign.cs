namespace Wlst.Ux.Wj1080Module.Services
{


    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 26*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 26*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 26*100;




        public const int LuxDataRequsetOrReply = EventIdAssignBaseId + 1;

        public const int LuxDataMeasureEvent = EventIdAssignBaseId + 2;

        public const int LuxSetModeEvent = EventIdAssignBaseId + 3;

        public const int LuxSetReportTimeEvent = EventIdAssignBaseId + 4;

        public const int LuxZcModeEvent = EventIdAssignBaseId + 5;

        public const int LuxZcReportTimeEvent = EventIdAssignBaseId + 6;

        public const int LuxZcVersionEvent = EventIdAssignBaseId + 7;
    }
}
