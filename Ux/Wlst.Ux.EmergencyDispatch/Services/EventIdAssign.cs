namespace Wlst.Ux.EmergencyDispatch.Services
{


    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 88*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 88*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 88*100;

        public const int AddOneFaultShield = EventIdAssignBaseId + 1;

        public const int DeleteOneFaultShield = EventIdAssignBaseId + 2;

    }
}
