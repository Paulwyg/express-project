namespace Wlst.Ux.Wj6005Module.Services
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 30*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 30*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 30 * 100;
        /// <summary>

        /// </summary>
        public const int RequestJd601Pars = EventIdAssignBaseId+1;

        public const int UpdateJd601Pars = EventIdAssignBaseId + 2;

        public const int ZcJd601Par = EventIdAssignBaseId + 3;
        public const int ZcJd601Version = EventIdAssignBaseId + 4;
        public const int ZcJd601Time = EventIdAssignBaseId + 5;

        public const int Jd601EventTaskInstanceInfoRequsetOrReply = EventIdAssignBaseId + 6;
        public const int Jd601EventTaskInstanceInfoUpdate = EventIdAssignBaseId + 7;

        public const int RequestJd601PartlData = EventIdAssignBaseId + 8;
        public const int MeasereJd601One = EventIdAssignBaseId + 9;
        public const int MeasureJd601Two = EventIdAssignBaseId + 10;
        public const int AsynJd601Time = EventIdAssignBaseId + 11;
        public const int OpenCloseJd601 = EventIdAssignBaseId + 12;
        public const int ManuAdjustVolByUser = EventIdAssignBaseId + 13;
    }
}