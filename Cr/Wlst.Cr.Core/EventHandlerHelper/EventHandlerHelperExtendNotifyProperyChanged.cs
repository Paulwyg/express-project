using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
 
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;


namespace Wlst.Cr.Core.EventHandlerHelper
{

   /// <summary>
   ///  继承此类来实现事件数据处理；需要完成加入监听的事件地址与类型，并重写ExPublishedEvent函数
    /// 如果监听的事件过滤复杂可使用：
    /// EventPublisher.AddEventSubScriptionTokener(Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,FundOrderFilter);
    /// 来处理
   /// </summary>
   [Serializable]
   public class EventHandlerHelperExtendNotifyProperyChanged:Wlst .Cr .Core .CoreServices .ObservableObject 
   {
       /// <summary>
       /// 继承此类来实现事件数据处理；需要完成加入监听的事件地址与类型，并重写ExPublishedEvent函数
       /// </summary>
       public EventHandlerHelperExtendNotifyProperyChanged( )
       {
           this.InitEventPri( );
       }


       /// <summary>
       /// 执行数据初始化并注册事件
       /// </summary>
       private void InitEventPri( )
       {
           Wlst.Cr.Coreb.Servers.EventPublish.AddEventTokener(this.GetType().GUID + "", FundEventHandler, FundOrderFilter, false );
       }
 

       /// <summary>
       /// 取消 注册事件处理函数
       /// </summary>
       public void UnsubscribeEvent()
       {
          Wlst .Cr .Coreb .Servers .EventPublish.EventUnsubscribe(  this.GetType().GUID + "");
       }



       /// <summary>
       /// 事件监听的类型与地址
       /// </summary>
       protected ConcurrentDictionary<int, Tuple<string, bool>> EventFilterTipeAndId = new ConcurrentDictionary<int, Tuple<string, bool>>();

       /// <summary>
       /// 为过滤事件提供添加过滤属性
       /// </summary>
       /// <param name="eventId"></param>
       /// <param name="eventType">默认DocumentRegion</param>
       /// <param name="runInUi">是否在主UI线程运行 </param>
       public void AddEventFilterInfo(int eventId, string eventType = Core .EventHandlerHelper .PublishEventType .Core, bool runInUi = false)
       {
           if (EventFilterTipeAndId.ContainsKey(eventId)) EventFilterTipeAndId[eventId] = new Tuple<string, bool>(eventType, runInUi);
           else EventFilterTipeAndId.TryAdd(eventId, new Tuple<string, bool>(eventType, runInUi));
       }


       /// <summary>
       /// 事件过滤
       /// </summary>
       /// <param name="args"></param>
       /// <returns></returns>
       private bool FundOrderFilter(PublishEventArgs args)
       {
           //if (EventFilterTipeAndId.ContainsKey(-1)) return true;
           if (EventFilterTipeAndId.ContainsKey(0) && args.EventType == EventFilterTipeAndId[0].Item1)
               return true;

           if (EventFilterTipeAndId.ContainsKey(args.EventId) && args.EventType == EventFilterTipeAndId[args.EventId].Item1)
           {
               return FundOrderFilterForExtendCheck(args);
           }
           return false;
       }


       /// <summary>
       /// 若存在特殊的需要添加判断的  若返回false  所有检查都将不通过  否则继续监测
       /// </summary>
       /// <param name="args"></param>
       /// <returns></returns>
       public virtual bool FundOrderFilterForExtendCheck(PublishEventArgs args)
       {
           return true ;
       }


       delegate void DoTask(PublishEventArgs data);
       private void FundEventHandler(PublishEventArgs args)
       {
           try
           {
               bool runInui = true;
               if (EventFilterTipeAndId.ContainsKey(0) && args.EventType == EventFilterTipeAndId[0].Item1)
               {
                   runInui = EventFilterTipeAndId[0].Item2;
               }
               else if (EventFilterTipeAndId.ContainsKey(args.EventId) &&
                        args.EventType == EventFilterTipeAndId[args.EventId].Item1)
               {
                   runInui = EventFilterTipeAndId[args.EventId].Item2;
               }
               if (runInui)
               {
                   Application.Current.Dispatcher.Invoke(
                       System.Windows.Threading.DispatcherPriority.Send, new DoTask(ExPublishedEvent), args);
               }
               else
               {
                   RuninThreadJt(args);
               }
           }
           catch (Exception ex)
           {
               Cr .Core .UtilityFunction .WriteLog .WriteLogError("Publish Evenet Error without Invock:"+ex );
           }
       }


       ConcurrentDictionary<string, int> _tmp3 = new ConcurrentDictionary<string, int>();

       void RuninThreadJt(PublishEventArgs args)
       {
           string title = this.GetType().ToString() + "-" + args.EventId;
           if (_tmp3.ContainsKey(title))
           {
               Application.Current.Dispatcher.Invoke(
                   System.Windows.Threading.DispatcherPriority.Send, new DoTask(ExPublishedEvent), args);
               return;
           }

           try
           {
               ExPublishedEvent(args);
               Wrlog();
               return;
           }
           catch (Exception ex)
           {
               Cr.Core.UtilityFunction.WriteLog.WriteLogError("Publish Evenet Need Runin Ui  " + ex);
           }

           if (_tmp3.ContainsKey(title) == false) _tmp3.TryAdd(title, 0);
           Application.Current.Dispatcher.Invoke(
               System.Windows.Threading.DispatcherPriority.Send, new DoTask(ExPublishedEvent), args);

       }

       private long _dtls = 0;
       void Wrlog()
       {
           if (DateTime.Now.Ticks - _dtls > 60 * 50000000L)
           {
               foreach (var f in _tmp3)
               {
                   WriteLog.WriteLogError( "Event Need Runin Ui :" + f.Key);
               }
           }
       }

       ///// <summary>
       ///// 事件执行服务器数据到达  更新
       ///// </summary>
       ///// <param name="obj"></param>
       ///// <returns></returns>
       //private void ExExecuteEvent(object obj)
       //{
       //    var args = obj as PublishEventArgs;
       //    if (args == null) return;
       //    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
       //                                          new Action<PublishEventArgs>(ExExecuteEventIns),
       //                                          args);
       //    return;
       //}

       ///// <summary>
       ///// 线程执行 具体执行
       ///// </summary>
       //private void ExExecuteEventIns(PublishEventArgs args)
       //{
       //    ExPublishedEvent(args);
       //}

       /// <summary>
       /// 如果使用本类作为继承，需要处理事件函数，则需要实现本类并处理注册了的事件
       /// </summary>
       /// <param name="args"></param>
       public virtual void ExPublishedEvent(PublishEventArgs args)
       {

       }

   }
}
