namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    public interface IIWj1050Equipment
    {
        /// <summary>
        /// 附属设备逻辑ID
        /// </summary>
        int RtuId { get; set; }

        /// <summary>
        /// 附属设备逻辑地址
        /// </summary>
        int PhyId { get; set; }

        /// <summary>
        /// 附属设备名称
        /// </summary>
        string RtuName { get; set; }

        /// <summary>
        /// 电表波特率
        /// </summary>
        int MruBandRate { get; set; }

        /// <summary>
        /// 电表变比
        /// </summary>
        int MruRatio { get; set; }

        /// <summary>
        /// 电表类型 1 ：1997协议；2 ：2007 协议
        /// </summary>
        int MruType { get; set; }
        ///// <summary>
        ///// 备注
        ///// </summary>
        //string MruRemark { get; set; }

        /// <summary>
        /// 电表地址
        /// </summary>
        int MruAddr1 { get; set; }
        /// <summary>
        /// 电表地址
        /// </summary>
        int MruAddr2 { get; set; }
        /// <summary>
        /// 电表地址
        /// </summary>
        int MruAddr3 { get; set; }
        /// <summary>
        /// 电表地址
        /// </summary>
        int MruAddr4 { get; set; }
        /// <summary>
        /// 电表地址
        /// </summary>
        int MruAddr5 { get; set; }
        /// <summary>
        /// 电表地址
        /// </summary>
        int MruAddr6 { get; set; }

    }
}
