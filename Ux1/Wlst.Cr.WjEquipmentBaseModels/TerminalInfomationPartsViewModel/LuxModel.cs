using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationPartsViewModel
{
    public class LuxModel : IILux
    {
        public LuxModel()
        {
            RtuId = -1;
            RtuName = "None";
        }

        public LuxModel(int luxId, string luxName)
        {
            this.RtuId = luxId;
            this.RtuName = luxName;
        }

        /// <summary>
        /// 光控逻辑地址  
        /// </summary>
        public int RtuId { get; set; }

        /// <summary>
        /// 光控名称
        /// </summary>
        public string RtuName { get; set; }

        /// <summary>
        /// 光控安装位置 
        /// </summary>
        public string LuxLocation { get; set; }

        /// <summary>
        /// 通信方式 0 保留，1 电台，2 串口232，3 串口485，4 Zigbee，5 电力载波，6 Socket
        /// </summary>
        public int RtuCommucationType { get; set; }

        /// <summary>
        /// 光控端口号
        /// </summary>
        public int LuxPort { get; set; }

        /// <summary>
        /// 光控工作模式 0 每隔5秒主报，1 选测应答 ，2 根据设定的时间主动山包，默认10秒，GPRS通信，3 根据设定的时间主动上报，默认10秒，485通信
        /// </summary>
        public int LuxWorkMode { get; set; }

        /// <summary>
        /// 如果连接终端 则终端地址
        /// </summary>
        public int AttachRtuId { get; set; }

        /// <summary>
        /// 光控量程
        /// </summary>
        public int LuxRange { get; set; }

        /// <summary>
        /// 光控地址，此地址为光控设备上传数据自带的光控终端地址
        /// </summary>
        public int PhyId { get; set; }
    }
}
