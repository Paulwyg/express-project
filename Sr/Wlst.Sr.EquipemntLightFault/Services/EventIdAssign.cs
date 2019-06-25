namespace Wlst.Sr.EquipemntLightFault.Services
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 +49*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 +49*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 49 * 100;
        /// <summary>
        /// 请求终端故障类型 
        /// </summary>
        public const int FaultTypeRequest = EventIdAssignBaseId+1;

        /// <summary>
        /// 请求终端所有故障 
        /// </summary>
        public const int EquipmentExistFaultRequest = EventIdAssignBaseId+2;
        /// <summary>
        /// 更新终端故障信息 同FaultRequest 相同
        /// </summary>
        public const int EquipmentExistFaultUpdateId = EventIdAssignBaseId + 6;

        /// <summary>
        /// 终端已经存在故障 增加故障，发布为一个参数 list《int》  故障编号
        /// </summary>
        public const int EquipmentExistFaultAddId = EventIdAssignBaseId + 4;

        /// <summary>
        /// 终端已经存在故障 消除故障，发布为一个参数 list《int》  故障编号
        /// </summary>
        public const int EquipementExistFaultDeleteId = EventIdAssignBaseId + 5;



        /// <summary>
        /// 终端是否告警请求  无参数
        /// </summary>
        public const int FaultBandtoEquipmentRequest = EventIdAssignBaseId + 3;

        /// <summary> 
        /// 终端是否告警请求  全部信息更新 无参数，返回数据为Update事件 
        /// </summary>
        public const int FaultBandtoEquipmentUpdate = EventIdAssignBaseId + 7;

        /// <summary>
        /// 无参数
        /// </summary>
        public const int FaultTypeUpdateId = EventIdAssignBaseId + 8;

        /// <summary>
        /// 无参数
        /// </summary>
        public const int UserIndividuationFaultRequestId = EventIdAssignBaseId + 9;
        /// <summary>
        /// 无参数
        /// </summary>
        public const int UserDisplayErrorSelfSetInfoUpdated = EventIdAssignBaseId + 10;

        /// <summary>
        /// Svr事件 ，第二参数为 EquipmentPreFaultExChange 类型
        /// </summary>
        public const int PreExistErrorRequestId = EventIdAssignBaseId + 11;


        /// <summary>
        /// 当终端的故障状态发送变化时候   List [Tuple[int, bool>> 地址-是否有故障
        /// </summary>
        //public const int RtuErrorStateChanged = EventIdAssignBaseId + 12;
    }
}
