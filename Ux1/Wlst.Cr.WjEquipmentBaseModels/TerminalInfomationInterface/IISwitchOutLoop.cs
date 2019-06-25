namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    public interface IISwitchOutLoop
    {
        /// <summary>
        /// 终端序号
        /// </summary>
         int RtuId { get; set; }

        /// <summary>
        /// 开关量输出序号
        /// </summary>
         int SwitchOutId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
         string SwichtOutName { get; set; }

        /// <summary>
        /// 归属于哪个分组控制
        /// </summary>
         int ControlGroup { get; set; }

        /// <summary>
        /// 口矢参数
        /// </summary>
         int Vector { get; set; }

        /// <summary>
        /// 回路路数
        /// </summary>
         int LoopSum { get; set; }
    }
}
