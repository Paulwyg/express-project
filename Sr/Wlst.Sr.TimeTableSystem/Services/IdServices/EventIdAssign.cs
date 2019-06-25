namespace Wlst.Sr.TimeTableSystem.Services.IdServices
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 51*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 51*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 51 * 100;
        /// <summary>
        /// 时间表更新 
        /// <para> 无参数</para>
        /// </summary>
        public const int TimeTimeUpdate =EventIdAssignBaseId+ 1;

        /// <summary>
        /// 时间表请求 
        /// </summary>
        public const int TimeTimeRequest = EventIdAssignBaseId + 2;


        /// <summary>
        /// 日出日过事件发布 无参数
        /// </summary>
        public const int SunSetRiseRequest = EventIdAssignBaseId + 9;
        /// <summary>
        /// 日出日落时间增加 10
        /// <para>第一参数 月份</para>
        /// <para>第二参数 日期</para>
        /// </summary>
        public const int SunSetRiseAdd = EventIdAssignBaseId + 10;

        /// <summary>
        /// 日出日落删除 11
        /// <para>第一参数 月份</para>
        /// <para>第二参数 日期</para>
        /// </summary>
        public const int SunSetRiseDelete = EventIdAssignBaseId + 11;
        /// <summary>
        /// 日出日落更新 12
        /// <para>第一参数 月份</para>
        /// <para>第二参数 日期</para>
        /// </summary>
        public const int SunSetRiseUpdate = EventIdAssignBaseId + 12;

        /// <summary>
        /// 服务器反馈下一次执行时间表操作的信息
        /// </summary>
        public const int TimeTableEventRequest = EventIdAssignBaseId + 13;


        /// <summary>
        /// 当节假日信息或绑定信息发送变化的时候 发布时间  即客户端收到服务器的更新信息时发布 无参数
        /// </summary>
        public const int TimeHolidayTimeSchduleAndRtuBandingChanged = EventIdAssignBaseId + 14;

        /// <summary>
        /// 临时方案更新
        /// </summary>
        public const int TimeTemporaryInfoUpdateId = EventIdAssignBaseId + 15;

        /// <summary>
        /// 临时方案请求
        /// </summary>
        public const int TimeTemporaryInfoRequestId = EventIdAssignBaseId + 16;

        public const int TimeTemporaryInfoDeleteId = EventIdAssignBaseId + 17;

        /// <summary>
        /// 隧道方案更新
        /// </summary>
        public const int TunnelInfoSetUpdateId = EventIdAssignBaseId + 18;
        /// <summary>
        /// 隧道方案请求
        /// </summary>
        public const int TunnelInfoSetRequestId = EventIdAssignBaseId + 19;
    }
}