namespace Wlst.Ux.EquipemntLightFault.Services
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 +36*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 +36*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 36 * 100;


        public const int FaultErrorsRequest = EventIdAssignBaseId+1;

        public const int EventAddFaultDefineSetttingAnimationId = EventIdAssignBaseId + 2;

        public const int EventCancelDefineSettingAnimationId = EventIdAssignBaseId + 3;

        public const int EventSaveAddFaultSettingRecordId = EventIdAssignBaseId + 4;

        /// <summary>
        /// 推送故障数量
        /// </summary>
        public const int PushErrNums = EventIdAssignBaseId + 5;

        /// <summary>
        /// 故障勾选
        /// </summary>
        public const int EquipmentFaultIsChecked = EventIdAssignBaseId + 6;

        /// <summary>
        /// 故障勾选数量
        /// </summary>
        public const int EquipmentFaultIsCheckedCount = EventIdAssignBaseId + 7;

        /// <summary>
        /// 关闭语音报警三分钟
        /// </summary>
        public const int VoiceAlarmClosed = EventIdAssignBaseId + 8;



    }
}
