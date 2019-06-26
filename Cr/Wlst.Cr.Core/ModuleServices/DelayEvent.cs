using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
 
using Wlst.Cr.Core.EventHandlerHelper;

namespace Wlst.Cr.Core.ModuleServices
{
    /// <summary>
    /// 延迟事件  待系统启动完毕后需要立即处理的事件   或待系统系统完毕后 一段时间需要处理的事件
    /// </summary>
    public class DelayEvent
    {
        protected static List<Tuple<Action, int, int>> Eve = new List<Tuple<Action, int, int>>();
        protected static List<int> HappenedEvent = new List<int>() {0};

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ac">延迟任务 程序启动后的1200秒 所有任务将结束</param>
        /// <param name="delaySeconds">延迟时间 如果为0 则为加载完成后立即执行</param>
        /// <param name="delayEvent">需要等待事件的发生 默认为等待主界面呈现</param>
        public static void RegisterDelayEvent(Action ac, int delaySeconds,
                                              DelayEventHappen delayEvent = DelayEventHappen.EventSvAc )
        {

            Eve.Add(new Tuple<Action, int, int>(ac, delaySeconds, (int) delayEvent));
        }



        /// <summary>
        /// 宣布事件发生 等待该事件发生的任务则按照给定的等待时间就要执行
        /// </summary>
        /// <param name="delayEvent"></param>
        public static void RaiseEventHappen(DelayEventHappen delayEvent)
        {
            if (delayEvent == DelayEventHappen.EventSvAc) if (!HappenedEvent.Contains(6)) HappenedEvent.Add(6);
            if (delayEvent == DelayEventHappen.EventFive) if (!HappenedEvent.Contains(5)) HappenedEvent.Add(5);
            if (delayEvent == DelayEventHappen.EventFour) if (!HappenedEvent.Contains(4)) HappenedEvent.Add(4);
            if (delayEvent == DelayEventHappen.EventThree) if (!HappenedEvent.Contains(3)) HappenedEvent.Add(3);
            if (delayEvent == DelayEventHappen.EventTwo) if (!HappenedEvent.Contains(2)) HappenedEvent.Add(2);
            if (delayEvent == DelayEventHappen.EventOne) if (!HappenedEvent.Contains(1)) HappenedEvent.Add(1);

            if (delayEvent == DelayEventHappen.EventSvAc)
            {
                Core.CoreServices.RegionManage.IsSystemMainViewActive = true;
                ModuleServices.DelayEvent.ExAction();
            }

        }



        /// <summary>
        /// 执行函数  外部不得调用
        /// </summary>
        internal static void ExAction()
        {
            Thread th = new Thread(run);
            th.Start();
        }

        private static int Count = 1;
        private static int[] EventCount = new int[9];

        private static void run()
        {
            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("DelayEvent Running...");
            Thread.Sleep(500);
            while (Eve.Count > 0)
            {
                try
                {
                    foreach (var t in HappenedEvent)
                    {
                        for (int i = Eve.Count - 1; i >= 0; i--)
                        {
                            if (Eve[i].Item3 != t) continue;
                            if (Eve[i].Item2 <= EventCount[t])
                            {
                                try
                                {
                                    Application.Current.Dispatcher.Invoke(
                                        System.Windows.Threading.DispatcherPriority.Normal,
                                        new DoTask(Eve[i].Item1));

                                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("DelayEvent Do Task " +
                                                                                       Eve[i].Item1.ToString());
                                }
                                catch (Exception ex)
                                {

                                }
                                try
                                {
                                    Eve.RemoveAt(i);
                                }
                                catch (Exception ex)
                                {
                                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("DelayEvent In Core Err:" + ex);
                                }
                            }
                        }
                        EventCount[t]++;
                    }
                }
                catch (Exception ex)
                {

                }
                Count++;
                Thread.Sleep(1000);
                if (Count > 1200) break;
            }
        }

        //定义委托
        private delegate void DoTask();
    }

    /// <summary>
    /// 预定义可能发生的事件
    /// </summary>
    public enum DelayEventHappen
    {
        /// <summary>
        /// 不等待任何事件 直接执行
        /// </summary>
        NoneEvent = 0,

        /// <summary>
        /// 等待事件1的发送 然后执行
        /// </summary>
        EventOne = 1,

        /// <summary>
        /// 等待事件2的发送 然后执行
        /// </summary>
        EventTwo = 2,

        /// <summary>
        /// 等待事件3的发送 然后执行
        /// </summary>
        EventThree = 3,

        /// <summary>
        /// 等待事件4的发送 然后执行
        /// </summary>
        EventFour = 4,

        /// <summary>
        /// 等待事件5的发送 然后执行
        /// </summary>
        EventFive = 5,

        /// <summary>
        /// 系统激活  
        /// </summary>
        EventSvAc=6,
    }
}
