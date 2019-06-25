namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    /// <summary>
    /// 本模块的设置信息
    /// </summary>
    public class UxLdTreeSetting
    {
        /// <summary>
        /// 是否在主界面显示 单终端树
        /// </summary>
        public static bool IsShowSingleTreeOnTab;

        /// <summary>
        /// 终端树 快速操作中不显示无效开关量 0 不显示 1 屏蔽 2 全部
        /// </summary>
        public static int IsRutsNotShowNullK;
        /// <summary>
        /// 终端树 显示快速操作  0 普通模式 1 快速模式
        /// </summary>
        public static int IsShowRapidOp;
        /// <summary>
        /// 快速查询 显示数量上限
        /// </summary>
        public static int SearchLimit;
    }
}
