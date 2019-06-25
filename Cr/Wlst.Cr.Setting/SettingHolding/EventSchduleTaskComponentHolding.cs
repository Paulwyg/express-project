using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.Setting.Interfaces;

namespace Wlst.Cr.Setting.SettingHolding
{
   public  class EventSchduleTaskComponentHolding
    {

       private static Dictionary<int, IIEventSchduleTask> _dicItems = new Dictionary<int, IIEventSchduleTask>();

       /// <summary>
       /// 获取部件数量
       /// </summary>
       public static int Count
       {
           get { return _dicItems.Count; }
       }

       /// <summary>
       /// 获取是否已经包含该部件
       /// </summary>
       /// <param name="item">任务</param>
       /// <returns></returns>
       public static bool ContainsComponent(IIEventSchduleTask item)
       {
           return _dicItems.ContainsKey(item.EventSchduleClassId);
       }

       /// <summary>
       /// 获取所有任务类 原始数据 勿修改
       /// </summary>
       public static Dictionary<int, IIEventSchduleTask> EventSchduleTaskItems
       {
           get { return _dicItems; }
       }


       /// <summary>
       /// 获取所有部件ID
       /// </summary>
       /// <returns></returns>
       public static ICollection<int> GetAllEventSchduleTaskComponentIDs
       {
           get
           {

               var lst = new List<int>();
               lst.AddRange(from t in _dicItems
                            orderby t.Key
                            select t.Key);
               return lst;

               //var lst = _dictionaryMenuItems.Select(t => t.Key).ToList();
               return _dicItems.Keys;
           }
       }


       /// <summary>
       /// 获取指定id值的部件 不存在则返回null
       /// </summary>
       /// <param name="id"></param>
       /// <returns>存在则返回部件 不存在则返回null</returns>
       public static IIEventSchduleTask GetEventSchduleTaskItemById(int id)
       {
           return _dicItems.ContainsKey(id) ? _dicItems[id] : null;
       }


       /// <summary>
       /// 清除数据
       /// </summary>
       public void Clean()
       {
           try
           {
               _dicItems.Clear();
           }
           catch (Exception)
           {

           }
       }

       /// <summary>
       /// 有则更新无则增加
       /// </summary>
       /// <param name="item">基础部件</param>
       public void AdEventSchduleTaskItem(IIEventSchduleTask item)
       {
           UpdateEventSchduleTaskItem( item);
       }


       /// <summary>
       /// 更新基础部件 有则更新无则增加
       /// </summary>
       /// <param name="item">基础部件</param>
       public void UpdateEventSchduleTaskItem(IIEventSchduleTask item)
       {
           if (_dicItems.ContainsKey(item.EventSchduleClassId))
           {
               _dicItems[item.EventSchduleClassId] = item;
               var args = new PublishEventArgs()
               {
                   EventType = PublishEventType.Core,
                   EventId = Services.EventIdAssign.EventSchduleTaskComponentUpdate,
               };
               args.AddParams(item.EventSchduleClassId);
               EventPublish.PublishEvent(args);
           }
           else
           {
               _dicItems.Add(item.EventSchduleClassId, item);
               var args = new PublishEventArgs()
               {
                   EventType = PublishEventType.Core,
                   EventId = Services.EventIdAssign.EventSchduleTaskComponentAdd,
               };
               args.AddParams(item.EventSchduleClassId);
               EventPublish.PublishEvent(args);
           }

       }

       /// <summary>
       /// 删除部件
       /// </summary>
       /// <param name="item">需要删除的部件</param>
       public void DeleteEventSchduleTaskItem(IIEventSchduleTask item)
       {
           if (_dicItems.ContainsKey(item.EventSchduleClassId))
           {

               _dicItems.Remove(item.EventSchduleClassId);
               var args = new PublishEventArgs()
               {
                   EventType = PublishEventType.Core,
                   EventId = Services.EventIdAssign.EventSchduleTaskComponentDelete,
               };
               args.AddParams(item.EventSchduleClassId);
               EventPublish.PublishEvent(args);
           }
       }

       /// <summary>
       /// 删除未出现在重组后的列表中的部件 删除不存在保留列表中的部件
       /// </summary>
       /// <param name="lstShouldKeepItemsId">需要保留的部件列表清单,函数处理后仅包含此列表数据</param>
       public void UpdateEventSchduleTaskItem(List<IIEventSchduleTask> lstShouldKeepItemsId)
       {
           var lstKeeps = lstShouldKeepItemsId.Select(t => t.EventSchduleClassId).ToList();


           foreach (var t in lstShouldKeepItemsId)
           {
               if (!_dicItems.ContainsKey(t.EventSchduleClassId)) this.AdEventSchduleTaskItem(t);
           }
           var lstNeedDelete = new List<int>();
           foreach (var t in _dicItems.Keys)
           {
               if (lstKeeps.Contains(t)) continue;
               if (!lstNeedDelete.Contains(t)) lstNeedDelete.Add(t);
           }
           if (lstNeedDelete.Count > 0)
           {
               var args = new PublishEventArgs()
               {
                   EventType = PublishEventType.Core,
                   EventId = Services.EventIdAssign.EventSchduleTaskComponentDelete,
               };
               foreach (var t in lstNeedDelete)
               {
                   if (_dicItems.ContainsKey(t))
                   {
                       _dicItems.Remove(t);
                       args.AddParams(t);
                   }
               }
               EventPublish.PublishEvent(args);
           }
       }


       /// <summary>
       /// 删除部件  
       /// </summary>
       /// <param name="id">需要删除的部件id</param>
       public void DeleteEventSchduleTaskItem(int id)
       {
           if (_dicItems.ContainsKey(id))
           {
               _dicItems.Remove(id);
               var args = new PublishEventArgs()
               {
                   EventType = PublishEventType.Core,
                   EventId = Services.EventIdAssign.EventSchduleTaskComponentDelete,
               };
               args.AddParams(id);
               EventPublish.PublishEvent(args);
           }
       }
   }
}
