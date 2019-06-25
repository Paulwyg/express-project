using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Sr.EquipemntLightFault.Model;


namespace Wlst.Ux.EquipemntLightFault.Models.Exchange
{
    public class PreErrorRequest
    {
        /// <summary>
        /// 0 为所有终端 其他为终端
        /// </summary>
        public int RtuId;

        /// <summary>
        /// 查询的开始时间 如果与结束时间相同 则不执行查询，必须提供查询时间
        /// </summary>
        public DateTime DtStartTime;

        public DateTime DtEndTime;
    }


    /// <summary>
    /// 与服务器交互现场故障信息
    /// </summary>
    public class EquipmentPreFaultExChange
    {

        public List<PreErrorItem> Info;

        /// <summary>
        /// 
        /// </summary>
        public EquipmentPreFaultExChange()
        {
            Info = new List<PreErrorItem>();
        }
    }

    public class PreErrorItem
    {
        /// <summary>
        /// 序号 自增
        /// </summary>
        public int Id { get; set; }

     
        /// <summary>
        /// 发生时间
        /// </summary>
        public string DateCreate { get; set; }


        /// <summary>
        /// 发生时间
        /// </summary>
        public string DateRemove { get; set; }

        /// <summary>
        /// 终端序号
        /// </summary>

        public int RtuId { get; set; }

        /// <summary>
        /// 回路序号 
        /// </summary>
        public int LoopId { get; set; }

        /// <summary>
        /// 故障序号
        /// </summary>
        public int FaultCodeId { get; set; }


        /// <summary>
        /// 记录编号 数据库存在的记录标号
        /// </summary>
        public long RecordAlarmId { get; set; }

        /// <summary>
        /// 记录编号 数据库存在的记录标号
        /// </summary>
        public long RecordRemoveId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
