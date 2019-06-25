namespace Wlst.Ux.PrivilegesManage.Services
{


    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 57*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 57*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 57*100;

        public const int ResetAnimationEventId = EventIdAssignBaseId + 1;
        public const int AddAnimationEventId = EventIdAssignBaseId + 2;
        public const int FleshAnimationEventId = EventIdAssignBaseId + 3;
        public const int CancelFleshAnimationEventId = EventIdAssignBaseId + 4;
        //更新区域分组 无参数
        public const int SingleInfoAreaAllNeedUpdate = EventIdAssignBaseId + 5;


    }
}
