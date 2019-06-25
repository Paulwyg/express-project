namespace Wlst.Ux.Wj1090Module.Services
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 27*100, 每个模块均发放100个Id值。 todo
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 27*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 27 * 100;

        /// <summary>
        /// 当设置界面防盗设置更改启用时，发布事件通知树更新数据。
        /// <para></para>
        /// </summary>
        public const int EventUpdatePartsNodes = EventIdAssignBaseId + 1;

        public const int EventTreeNodeLineSelected = EventIdAssignBaseId + 2;

        public const int EventTreeNodeConcentratorSelected = EventIdAssignBaseId + 3;

        public const int EventTreeNodeTmlSelected = EventIdAssignBaseId + 4;

    }
}
