namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    public interface IILux
    {
        /// <summary>
        /// 光控逻辑地址
        /// </summary>
         int RtuId { get; set; }

        /// <summary>
        /// 光控名称
        /// </summary>
         string RtuName { get; set; }

        /// <summary>
        /// 光控安装位置 
        /// </summary>
         string LuxLocation { get; set; }

        /// <summary>
        /// 通信方式 0 保留，1 电台，2 串口232，3 串口485，4 Zigbee，5 电力载波，6 Socket
        /// </summary>
         int RtuCommucationType { get; set; }

        /// <summary>
        /// 光控端口号
        /// </summary>
         int LuxPort { get; set; }

        /// <summary>
        /// 光控工作模式 0 每隔5秒主报，1 选测应答 ，2 根据设定的时间主动山包 ，4 新- 根据设定的时间主动上报
        /// </summary>
         int LuxWorkMode { get; set; }

        /// <summary>
        /// 如果连接终端 则终端地址
        /// </summary>
         int AttachRtuId { get; set; }

        /// <summary>
        /// 光控量程
        /// </summary>
         int LuxRange { get; set; }

        /// <summary>
        /// 光控地址，此地址为光控设备上传数据自带的光控终端地址
        /// </summary>
         int PhyId { get; set; }

    }
}
