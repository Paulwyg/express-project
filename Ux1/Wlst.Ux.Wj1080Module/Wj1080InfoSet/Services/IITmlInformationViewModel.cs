using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Ux.Wj1080Module.Wj1080InfoSet.Services
{
    public interface IITmlInformationViewModel:IINavOnLoad ,IITab 
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
        /// 光控类型  1 直连COM；2 GPRS方式；3 终端485方式
        /// </summary>
        int RtuCommucationType { get; set; }

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

        string ShowInfo { get; set; }
    }
}
