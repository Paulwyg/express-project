namespace Wlst.Sr.EquipmentGroupInfoHolding.Services
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 41*100, 每个模块均发放100个Id值。 todo
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 41*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 41*100;



        /// <summary>
        ///  更新分组 无参数
        /// </summary>
        public const int SingleInfoGroupAllNeedUpdate = EventIdAssignBaseId + 3;


        /// <summary>
        /// 分组中一个终端可属于多个个分组 所有信息更新；无参数 8
        /// <para>界面设置后或第一次从数据库中加载时发布</para>
        /// </summary>
        public const int MulitInfoGroupAllNeedUpdate = EventIdAssignBaseId + 8;


    }
}
