namespace Wlst.Sr.EquipmentInfoHolding.Services
{
    /// <summary>
    /// 本模块的设置信息
    /// </summary>
    public class UxTreeSetting
    {
        /// <summary>
        /// 在分组显示时  是否显示id值
        /// </summary>
        public static bool IsShowGrpInTreeModelShowId;



        /// <summary>
        /// 在分组显示终端级别时  是否显示终端下的设备以及回路
        /// </summary>
        public static bool IsShowGrpInTreeModelShowTmlChildNode;

        /// <summary>
        /// 是否在主界面显示 单终端树
        /// </summary>
        public static bool IsShowSingleTreeOnTab;


        /// <summary>
        /// 是否在主界面显示多终端树
        /// </summary>
        public static bool IsShowMulTreeOnTab;

        /// <summary>
        /// 是否在选中分组的时候地图只显示选中分组下的终端
        /// </summary>
        public static bool IsSelectGrpMapOnlyShow;


        /// <summary>
        /// 终端图标不显示故障图标
        /// </summary>
        public static bool IsRutsNotShowError;
        /// <summary>
        /// 终端树 快速操作中不显示无效开关量 0 不显示 1 屏蔽 2 全部
        /// </summary>
        public static int IsRutsNotShowNullK;
        /// <summary>
        /// 终端树 显示快速操作  0 普通模式 1 快速模式
        /// </summary>
        public static int IsShowRapidOp;
        /// <summary>
        /// 终端树排序  1 按照终端物理地址排序，2 按照拼音排序，3 按照分组内的地址排序，4 按照逻辑地址排序
        /// </summary>
        public static int TreeSortBy;

        /// <summary>
        /// 是否显示分组
        /// </summary>
        public static bool IsShowGrp;
        /// <summary>
        /// 是否显示id值
        /// </summary>
        public static bool IsShowArea;
        /// <summary>
        /// 在分组显示时  主设备编号，名称
        /// </summary>
        public static bool IsShowFid;
        /// <summary>
        /// 快速查询 显示数量上限
        /// </summary>
        public static int SearchLimit;
    }
}
