//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Wlst.Ux.EquipmentDataQuery.RecordEventQueryViewModel.Model
//{
//    public class ExchangeReplyRecordEvent
//    {
//        public List<RecordEvent> Info;

//        public ExchangeReplyRecordEvent()
//        {
//            Info = new List<RecordEvent>();
//        }
//    };
//    public class RecordEvent
//    {
//        /// <summary>
//        /// autoIncrement
//        /// </summary>

//        public int EventId { get; set; }

//        /// <summary>
//        /// 发生时间
//        /// </summary>

//        public long DateCreate { get; set; }

//        /// <summary>
//        /// 操作的类型 详见操作类型 OperatorTypeEnum 终端设备的
//        /// </summary>

//        public int OperatorType { get; set; }

//        /// <summary>
//        /// 操作的用户
//        /// </summary>

//        public string UserName { get; set; }

//        /// <summary>
//        /// 通信类型: 0保留， 1电台， 2串口232， 3串口485， 4Zigbee， 5电力载波， 6 Socket
//        /// </summary>
    
//        public int CommType { get; set; }

//        /// <summary>
//        /// 操作设备类型  0保留，1终端，2光控，3节能，4线路检查，5单灯，6抄表
//        /// </summary>

//        public int DeviceType { get; set; }

//        /// <summary>
//        /// 多个地址；分开
//        /// </summary>

//        public string DeviceIds { get; set; }


//        public int Grpid { get; set; }


//        public int LoopId { get; set; }



//        public List<int> GetDeviceIds()
//        {

//            var lst = new List<int>();
//            if (string.IsNullOrEmpty(DeviceIds)) return lst;
//            var sp = DeviceIds.Split(';');
//            foreach (var t in sp)
//            {
//                try
//                {
//                    int prii = Convert.ToInt32(t);
//                    if (prii > 0) lst.Add(prii);
//                }
//                catch (Exception ex)
//                {
//                }
//            }
//            return lst;
//        }

//        public void UpdateDeviceIds(List<int> right)
//        {
//            this.DeviceIds = "";
//            foreach (var t in right)
//            {
//                DeviceIds += t + ";";
//            }
//        }
//    }
//}
