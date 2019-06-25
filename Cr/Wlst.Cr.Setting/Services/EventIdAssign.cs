namespace Wlst.Cr.Setting.Services
{
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 4*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 4*100;

        /// <summary>
        /// 设置模块中的部件更新，携带参数：该设置中的部件Id值，可能是列表 args[0] args[1]  均为int
        /// </summary>
        public const int SettingModuleComponentUpdate = EventIdAssignBaseId + 1;

        /// <summary>
        /// 设置模块中的部件删除，携带参数：该设置中的部件Id值，可能是列表 args[0] args[1]  均为int
        /// </summary>
        public const int SettingModuleComponentDelete = EventIdAssignBaseId + 2;

        /// <summary>
        /// 设置模块中的部件增加，携带参数：该设置中的部件Id值，可能是列表 args[0] args[1]  均为int
        /// </summary>
        public const int SettingModuleComponentAdd = EventIdAssignBaseId + 3;


        /// <summary>
        /// 任务模块中的部件更新，携带参数：该设置中的部件Id值，可能是列表 args[0] args[1]  均为int
        /// </summary>
        public const int EventSchduleTaskComponentUpdate = EventIdAssignBaseId + 4;

        /// <summary>
        /// 任务模块中的部件删除，携带参数：该设置中的部件Id值，可能是列表 列表为 args[0] args[1]  均为int
        /// </summary>
        public const int EventSchduleTaskComponentDelete = EventIdAssignBaseId + 5;

        /// <summary>
        /// 任务模块中的部件增加，携带参数：该设置中的部件Id值，可能是列表 args[0] args[1]  均为int
        /// </summary>
        public const int EventSchduleTaskComponentAdd = EventIdAssignBaseId + 6;

    }
}
