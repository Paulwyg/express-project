namespace Wlst.Sr.PrivilegesCrl.Services
{
    /// <summary>
    /// 本模块的全局事件发布起始Id，3100000 + 56*100, 每个模块均发放100个Id值。
    /// </summary>
    public class EventIdAssign
    {
        /// <summary>
        /// 本模块的全局事件发布起始Id，3100000 + 56*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int EventIdAssignBaseId = 3100000 + 56*100;




        /// <summary>
        /// 修改用户信息，发布事件为第一参数为 ModfliedUserInfo
        /// </summary>
        public const int ModflyUserInfomationId = EventIdAssignBaseId + 3;

        /// <summary>
        /// 请求所有用户信息  一个参数AllUserInfo
        /// </summary>
        public const int RequestAllUserInfomationId = EventIdAssignBaseId + 4;

        /// <summary>
        /// 删除用户信息，携带参数为 用户名
        /// </summary>
        public const int DeleteUserInfoId = EventIdAssignBaseId + 5;

        /// <summary>
        /// 增加新用户 ，携带参数 ModfliedUserInfo
        /// </summary>
        public const int AddNewUserInfoId = EventIdAssignBaseId + 6;


    }
}