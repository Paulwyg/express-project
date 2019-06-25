using Wlst.Cr.WjEquipmentBaseModels.Models;
using Wlst.client;


namespace Wlst.Cr.WjEquipmentBaseModels.Interface
{
    public interface IIEquipmentInfo //WjEquipmentModels.Interface.IIEquipmentInfo
    {
        /// <summary>
        /// 附属设备逻辑ID
        /// </summary>
        int RtuId { get; set; }

        /// <summary>
        /// 本设备附属到的主设备的地址 如果为0则本设备为主设备  
        /// </summary>
        int AttachRtuId { get; set; }

        /// <summary>
        /// 附属设备逻辑地址
        /// </summary>
        int PhyId { get; set; }

        /// <summary>
        /// 附属设备名称
        /// </summary>
        string RtuName { get; set; }

        /// <summary>
        /// 终端工作状态
        ///  0-不用，1-停运，2-使用
        /// </summary>
        int RtuState { get; set; }

        /// <summary>
        /// 附属设备型号 默认1987
        /// </summary>
        int RtuModel { get; set; }

        /// <summary>
        /// 地图X坐标 仅JPG
        /// </summary>
        double  Xmap { get; set; }

        /// <summary>
        /// 地图Y坐标仅JPG
        /// </summary>
        double  Ymap { get; set; }

        /// <summary>
        ///  GIS以及其他矢量地图
        /// </summary>
        double Xgis { get; set; }

        /// <summary>
        /// GIS以及其他矢量地图
        /// </summary>
        double Ygis { get; set; }

        /// <summary>
        /// 数据最后更新时间
        /// </summary>
        long   Md5 { get; set; }

        /// <summary>
        /// 其他参数
        /// </summary>
        object OtherArgu { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        //EnumDeviceType EnumDeviceType { get; set; }

        /// <summary>
        /// 设备安装位置
        /// </summary>
        string InstallAddr { get; set; }

        /// <summary>
        /// 设备备注信息
        /// </summary>
        string Remark { get; set; }

        /// <summary>
        /// 设备开通日期
        /// </summary>
        long  DataCreate { get; set; }


       string  GetRtuLoopName(int loopId);
       
        string GetRtuLoopNameandLampCode(int loopId);

        int AreaId { get; set; }
    }
}
