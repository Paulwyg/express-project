//using System;
 

//namespace Wlst.Cr.Core.UtilityFunction
//{
//    /// <summary>
//    /// 注册异步处理事件  
//    /// 需要异步等待服务器数据返回
//    /// 数据返回后需要执行函数并更新界面等
//    /// </summary>
//    [Serializable]
//    public class TriggerEvent
//    {

//        /// <summary>
//        /// 注册处理事件 
//        /// </summary>
//        /// <param name="key">监听协议返回关键字  即服务器数据传回来的解析关键字 </param>
//        /// <param name="eventId">等待事件标记id  服务端返回将携带的关键字</param>
//        /// <param name="overtimes">事件处理超时秒数  如果超时程序将该事件退回 并执行超时函数</param>
//        /// <param name="actionEx">事件处理函数</param>
//        /// <param name="actionOverTime">超时处理函数</param>
//        public static void AddTriggerEventTokener(int key, int eventId, int overtimes, Predicate<object> actionEx,
//                                                  Action actionOverTime)
//        {
//            //if (eventId < 5000) return;
//            try
//            {
//                EventTaskHander.AddEventTask(key + "-" + eventId, overtimes, actionEx, actionOverTime);
//            }
//            catch (Exception ex)
//            {
//                WriteLog.WriteLogError("Core TriggerEvent AddTriggerEventTokener error:" + ex);
//            }
//        }

//        /// <summary>
//        /// 删除监听事件
//        /// </summary>
//        /// <param name="key">监听协议返回关键字  即服务器数据传回来的解析关键字 </param>
//        /// <param name="eventId">等待事件标记id  服务端返回将携带的关键字</param>
//        public static void UnTriggerEventTokener(int key, int eventId)
//        {
//            try
//            {
//                EventTaskHander.UnsubscribeEventTask(key + "-" + eventId);
//            }
//            catch (Exception ex)
//            {
//                WriteLog.WriteLogError("Core TriggerEvent UnTriggerEventTokener error:" + ex);
//            }
//        }


//        /// <summary>
//        /// 执行事件处理函数
//        /// </summary>
//        /// <param name="key">监听协议返回关键字  即服务器数据传回来的解析关键字 </param>
//        /// <param name="eventId">等待事件标记id  服务端返回将携带的关键字</param>
//        /// <param name="obj">协议处理主体数据</param>
//        public static void ExEvent(int key, long  eventId, object obj)
//        {
//            try
//            {
//                EventTaskHander.ExEventThenUnsubscribeEventTaskIf(key + "-" + eventId, obj);
//            }
//            catch (Exception ex)
//            {
//                WriteLog.WriteLogError("Core TriggerEvent ExEvent error:" + ex);
//            }
//        }
//    }
//}
