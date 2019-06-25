namespace Wlst.Ux.EquipmentGroupManage.Services
{
    /// <summary>
    /// 模块内的事件发布请使用此处定义事件类型数据
    /// </summary>
    public class PublishEventTypeLocal
    {
        public const string Name = "GrpShowModule";

        /// <summary>
        /// 显示分组规划时  在终端列表中终端归属组删除时发布此事件
        /// 参数携带2个：1、终端地址，2、需要删除的终端路径
        /// </summary>
        public const int DeleteGrpPathByCommandBasicShowGroupManage = 100001;

        //public const string DeleteGrpPathByCommandBasicShowGroupManage = "DeleteGrpPathByCommand_BasicShowGroupManage";
        public const int RcvGrpInfofromServerAndNeedUpdateGroupInfo = 100002;



        public const int GrpSingleReloadTmlGroup = 100003;

        public const int GrpMultiReloadTmlGroup = 100004;
    }
}
