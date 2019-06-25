//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Wlst.Ux.EquipmentDataQuery.SndWeekTimeQueryViewModel.Model
//{
//    internal class ExchangeReplyRecordWeekTime
//    {
//        public DateTime RequestStartTime;
//        public DateTime RequestEndTime;

//        /// <summary>
//        /// 为0 则不考虑
//        /// </summary>
//        public int Tml;

//        public List<RecordWeekTime> Info;

//        public ExchangeReplyRecordWeekTime()
//        {
//            Info = new List<RecordWeekTime>();
//            RequestStartTime = DateTime.Now;
//            RequestEndTime = DateTime.Now;
//            Tml = 0;
//        }
//    };


//    public class RecordWeekTime
//    {
//        /// <summary>
//        /// autoIncrement
//        /// </summary>

//        public int RecordId { get; set; }


//        /// <summary>
//        /// RtuId
//        /// </summary>

//        public int RtuId { get; set; }

//        /// <summary>
//        /// 下发的周设置的类型 K1K3：13 ;K4K6：46 ;K7K8 78
//        /// </summary>

//        public int WeekTimeType { get; set; }

//        /// <summary>
//        /// 发生时间 下发时间
//        /// </summary>

//        public long DateCreate { get; set; }

//        /// <summary>
//        /// 应答时间  下发时应答时间为 0  
//        /// </summary>

//        public long DateReply { get; set; }

//        /// <summary>
//        /// 操作的用户
//        /// </summary>
//        public string UserName { get; set; }

//    }
//}
