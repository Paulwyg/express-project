namespace Wlst.Ux.Wj1050Module.Services
{


    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 25*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 25*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 25 * 100;

        /// <summary>
        /// 请求的任务信息到达或请求
        /// </summary>
        public const int MruEventTaskInstanceInfoRequsetOrReply = EventIdAssignBaseId + 1;



        /// <summary>
        /// 请求的任务信息到达或请求
        /// </summary>
        public const int MruEventTaskInstanceInfoUpdate = EventIdAssignBaseId + 2;

        public const int MruReadAddrId = EventIdAssignBaseId + 3;

        public const int MruReadDataId = EventIdAssignBaseId + 4;

        public const int RequestMruDataId = EventIdAssignBaseId + 5;


    }
}
