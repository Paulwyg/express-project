

namespace Wlst.Sr.EquipemntLightFault.Interfaces
{
    /// <summary>
    /// 现场设备故障接口
    /// </summary>
    public interface IIEquipmentFault
    {

        /// <summary>
        /// 序号 自增
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        string DateCreate { get; set; }

        /// <summary>
        /// 终端序号
        /// </summary>

        int RtuId { get; set; }

        /// <summary>
        /// 回路序号 
        /// </summary>
        int LoopId { get; set; }

        /// <summary>
        /// 故障序号
        /// </summary>
        int FaultCodeId { get; set; }


        /// <summary>
        /// 记录编号
        /// </summary>
        long  RecordId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        string Remark { get; set; }
    }
}
