namespace Wlst.Ux.TimeTableSystem.Services
{


    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 32*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 32*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 32 * 100;

        public const int EventAddOrUpdateTimeTableAnimationId = EventIdAssignBaseId + 1;
        public const int EventSaveOrCancelTimeTableAnimationId = EventIdAssignBaseId + 2;

        public const int EventSaveTimeTableData = EventIdAssignBaseId + 3;
        public const int EventCancelTimeTableData = EventIdAssignBaseId + 4;

    }
}
