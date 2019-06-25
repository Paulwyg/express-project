using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.Coreb.EventHelper;

namespace Wlst.Cr.Coreb.Servers
{
   public class EventPublish
    {
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="args">事件参数</param>
        /// <returns>事件是否成功发布</returns>
       public static void PublishEvent(PublishEventArgs args)
        {
            //  return EventAggregator.EventPublish(args);
            EventHelper.EventPublisher.MySelf.AddEventPublish(args);
        }

        /// <summary>
        /// 提供外部调用执行事件的拦截处理与事件发布,返回SubscriptionToken 索引，取消注册可使用该参数取消
        /// </summary>
        /// <param name="assemblyName">程序集名称,使用事件监听的类所在的程序集名称 </param>
        /// <param name="fundEventHandler">事件处理函数 参数为PublishEventArgs </param>
        /// <param name="fundOrderFilter">事件过滤函数 参数为PublishEventArgs  返回过滤结果</param>
        /// <param name="runInUiThread"> 是否需要在Ui线程运行  否则能提高性能</param>
        public static long AddEventTokener(string assemblyName,
                                                       Action<PublishEventArgs> fundEventHandler,
                                                       Predicate<PublishEventArgs> fundOrderFilter, bool runInUiThread = true)
        {
            if (fundEventHandler == null || fundOrderFilter == null) return -10;
            return EventHelper.EventPublisher .MySelf.AddEvent(assemblyName, fundEventHandler, fundOrderFilter, runInUiThread);
        }

        /// <summary>
        /// 卸载参数指向的事件；
        /// </summary>
        /// <param name="subscriptionToken">需要卸载事件参数索引</param>
        public static void EventUnsubscribe(long subscriptionToken)
        {
            EventHelper.EventPublisher.MySelf.RemoveEvent(subscriptionToken);
        }


        /// <summary>
        /// 卸载参数指向的程序集名称所具有的所有监听事件；
        /// </summary>
        /// <param name="assemblyName">需要卸载的通信事件的程序集名称</param>
        public static void EventUnsubscribe(string assemblyName)
        {
            EventHelper.EventPublisher.MySelf.RemoveEvent(assemblyName);
        }
    }
}
