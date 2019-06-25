using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using Wlst.Cr.Coreb.Servers;

namespace Wlst.Cr.Coreb.EventHelper
{
    /// <summary>
    /// 为外部提供事件的拦截处理以及事件发布功能
    /// </summary>
    public class EventPublisher
    {
      
        private static object obj = 1;
        private static EventPublisher myself = null;

        internal static EventPublisher MySelf
        {
            get
            {
                if (myself == null)
                {
                    lock (obj)
                    {
                        myself = new EventPublisher();
                    }
                }
                return myself;
            }
        }

      
        protected EventPublisher()
        {
            AsyncTask.Qtz.AddQtz("EventPublisRunMain", 8888, DateTime.Now.Ticks, Run1, 100);
            AsyncTask.Qtz.AddQtz("EventPublisRunDispath", 8888, DateTime.Now.Ticks, Run2, 100);
        }

        void Run1(object obj)
        {

            try
            {
                while (publisEvents.Count > 0)
                {
                    PublishEventArgs tmp = null;
                    if (publisEvents.TryDequeue(out tmp))
                    {
                        foreach (var f in Info)
                        {

                            if (f.Value.Item3(tmp))
                            {
                                _exEvent.Enqueue(
                                    new Tuple<Action<PublishEventArgs>, bool, PublishEventArgs>(f.Value.Item2,
                                                                                                f.Value.Item4, tmp));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("EventPublish Run Main Error:" + ex);
            }
        }

        void Run2(object obj)
        {
             
                try
                {
                    while (_exEvent.Count > 0)
                    {
                        Tuple<Action<PublishEventArgs>, bool, PublishEventArgs> tmp = null;
                        if (_exEvent.TryDequeue(out tmp))
                        {
                            if (tmp.Item2)
                            {
                                RunInUiThread(tmp.Item1, tmp.Item3);
                            }
                            else
                            {
                                tmp.Item1(tmp.Item3);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError("EventPublish Run Dispatch Error:" + ex);
                }
            
        }

        //定义委托
        delegate void DoTask(PublishEventArgs data);

        void RunInUiThread(Action<PublishEventArgs> action, PublishEventArgs data)
        {
            Application.Current.Dispatcher.Invoke(
           System.Windows.Threading.DispatcherPriority.Normal, new DoTask(action), data);

        }

        private ConcurrentQueue<Tuple<Action<PublishEventArgs>, bool, PublishEventArgs>> _exEvent =
            new ConcurrentQueue<Tuple<Action<PublishEventArgs>, bool, PublishEventArgs>>();

        private ConcurrentQueue<PublishEventArgs> publisEvents = new ConcurrentQueue<PublishEventArgs>();



        internal void AddEventPublish(PublishEventArgs args)
        {
            //  return EventAggregator.EventPublish(args);
            publisEvents.Enqueue(args);
        }

        ///// <summary>
        ///// 提供外部调用执行事件的拦截处理与事件发布,返回SubscriptionToken 索引，取消注册可使用该参数取消
        ///// </summary>
        ///// <param name="assemblyName">程序集名称,使用事件监听的类所在的程序集名称 </param>
        ///// <param name="fundEventHandler">事件处理函数 参数为PublishEventArgs </param>
        ///// <param name="fundOrderFilter">事件过滤函数 参数为PublishEventArgs  返回过滤结果</param>
        ///// <param name="runInUiThread"> 是否需要在Ui线程运行  否则能提高性能</param>
        //public static long AddEventSubScriptionTokener(string assemblyName,
        //                                               Action<PublishEventArgs> fundEventHandler,
        //                                               Predicate<PublishEventArgs> fundOrderFilter, bool runInUiThread = true)
        //{
        //    if (fundEventHandler == null || fundOrderFilter == null) return -10;
        //    return MySelf.AddEvent(assemblyName, fundEventHandler, fundOrderFilter, runInUiThread);
        //}

        ///// <summary>
        ///// 卸载参数指向的事件；
        ///// </summary>
        ///// <param name="subscriptionToken">需要卸载事件参数索引</param>
        //public static void EventSubScriptionTokenerUnsubscribe(long subscriptionToken)
        //{
        //    MySelf.RemoveEvent(subscriptionToken);
        //}


        ///// <summary>
        ///// 卸载参数指向的程序集名称所具有的所有监听事件；
        ///// </summary>
        ///// <param name="assemblyName">需要卸载的通信事件的程序集名称</param>
        //public static void EventSubScriptionTokenerUnsubscribe(string assemblyName)
        //{
        //    MySelf.RemoveEvent(assemblyName);
        //}




        private long Index = 1;

        private ConcurrentDictionary<long, Tuple<string, Action<PublishEventArgs>, Predicate<PublishEventArgs>, bool>> Info =
            new ConcurrentDictionary<long, Tuple<string, Action<PublishEventArgs>, Predicate<PublishEventArgs>, bool>>();

        internal long AddEvent(string assemblyName, Action<PublishEventArgs> fundEventHandler,
                               Predicate<PublishEventArgs> fundOrderFilter, bool runInUiThread)
        {
            long id = Interlocked.Increment(ref Index);
            while (Info.ContainsKey(id))
            {
                id = Interlocked.Increment(ref Index);
            }
            if (Info.ContainsKey(id) == false)
            {
                Info.TryAdd(id,
                            new Tuple<string, Action<PublishEventArgs>, Predicate<PublishEventArgs>, bool>(assemblyName,
                                                                                                     fundEventHandler,
                                                                                                     fundOrderFilter, runInUiThread));
            }
            return id;
        }

        internal void RemoveEvent(long id)
        {
            if (Info.ContainsKey(id))
            {
                Tuple<string, Action<PublishEventArgs>, Predicate<PublishEventArgs>, bool> tmp = null;
                Info.TryRemove(id, out tmp);
            }

        }


        internal void RemoveEvent(string assemblyName)
        {
            Tuple<string, Action<PublishEventArgs>, Predicate<PublishEventArgs>, bool> tmp = null;

            var ntg =
                (from t in Info where t.Value.Item1 != null && t.Value.Item1.Equals(assemblyName) select t.Key).ToList();
            foreach (var f in ntg)
            {
                if (Info.ContainsKey(f))
                {
                    Info.TryRemove(f, out tmp);
                }
            }
        }


    }
}
