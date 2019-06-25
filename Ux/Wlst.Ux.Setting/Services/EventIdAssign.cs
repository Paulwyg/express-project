namespace Wlst.Ux.Setting.Services
{


    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 53*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 53*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 53*100;

        /// <summary>
        /// 请求的任务信息到达或请求 EvenTasktInstanceforExchange
        /// </summary>
        public const int EventTaskInstanceInfoRequsetOrReply = EventIdAssignBaseId + 1;


        /// <summary>
        /// 删除任务计划
        /// <para> 事件携带一个参数  DeleteEventTaskInstanceforExchange</para>
        /// </summary>
        public const int EventTaskInstanceInfoDelete = EventIdAssignBaseId + 2;

        /// <summary>
        /// 中间层返回数据更新 EvenTasktInstanceforExchange 类
        /// </summary>
        public const int EventTaskInstanceInfoUpdate = EventIdAssignBaseId + 3;


        /// <summary>
        /// 中间层返回数据更 EventTaskNextExcuteInfo
        /// </summary>
        public const int EventTaskNextExcuteInfo = EventIdAssignBaseId + 4;



        /// <summary>
        /// </summary>
        public const int RecordEventRequest = EventIdAssignBaseId + 5;


        public const int RecordTaskRequest = EventIdAssignBaseId + 6;


        public const int AnimationOperatorDataQueryViewModelEnterId = EventIdAssignBaseId + 7;

    }
}
