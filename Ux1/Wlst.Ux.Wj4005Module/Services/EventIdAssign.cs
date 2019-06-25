namespace Wlst.Ux.WJ4005Module.Services
{


    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 69*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 69*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 69*100;

        public const int OpenLight = EventIdAssignBaseId + 1;
        public const int CloseLight = EventIdAssignBaseId + 2;
        public const int MesasureId = EventIdAssignBaseId + 3;

        public const int OpenCloseEventTaskInstanceInfoRequsetOrReply = EventIdAssignBaseId +4;

        public const int OpenCloseEventTaskInstanceInfoUpdate = EventIdAssignBaseId + 5;

        public const int RtuAsynTimeId = EventIdAssignBaseId + 6;

        /// <summary>
        /// 召测终端参数；
        /// <para>终端逻辑地址列表 </para>
        /// </summary>
        public const int ZhaoceRtuInfo = EventIdAssignBaseId + 7;

        /// <summary>
        /// 召测终端周设置；
        /// <para>终端逻辑地址列表 </para>
        /// </summary>
        public const int ZhaoceRtuWeekSetK1K3 = EventIdAssignBaseId + 8;


        /// <summary>
        /// 召测终端周设置；
        /// <para>终端逻辑地址列表 </para>
        /// </summary>
        public const int ZhaoceRtuWeekSetK4K6 = EventIdAssignBaseId + 9;


        /// <summary>
        /// 请求的任务信息到达或请求
        /// </summary>
        public const int PartolEventTaskInstanceInfoRequsetOrReply = EventIdAssignBaseId + 10;



        /// <summary>
        /// 请求的任务信息到达或请求
        /// </summary>
        public const int PartolEventTaskInstanceInfoUpdate = EventIdAssignBaseId + 11;


        public const int RecordWeekTimeRequest = EventIdAssignBaseId + 12;

        public const int RecordDataRequest = EventIdAssignBaseId + 13;
    }
}
