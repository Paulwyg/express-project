using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Wlst.Cr.Core.CoreInterface;

namespace Wlst.Cr.Core.ComponentHold
{
    /// <summary>
    /// View基础部件持有 
    /// </summary>
    public class ViewComponentHolding //: IPartImportsSatisfiedNotification
    {
        internal  static ConcurrentDictionary<string , object> DictionaryViewItems = new ConcurrentDictionary<string , object>();

        internal  static ConcurrentDictionary<string , IIViewExport> DictionaryViewInterfaceItems =
            new ConcurrentDictionary<string , IIViewExport>();

        /// <summary>
        /// 获取部件数量
        /// </summary>
        public static int Count
        {
            get { return DictionaryViewItems.Count; }
        }

        /// <summary>
        /// 获取是否已经包含该部件
        /// </summary>
        /// <param name="id">View ID</param>
        /// <returns></returns>
        public static bool ContainsComponent(string  id)
        {
            return DictionaryViewItems.ContainsKey(id);
        }


        /// <summary>
        /// 获取所有菜单部件ID
        /// </summary>
        /// <returns></returns>
        public static ICollection<string > GetAllViewsID
        {
            get { return DictionaryViewItems.Keys; }
        }


        /// <summary>
        /// 获取指定id值的部件
        /// </summary>
        /// <param name="id"></param>
        /// <returns>存在则返回部件 不存在则返回null</returns>
        public static object GetViewById(string  id)
        {
            return DictionaryViewItems.ContainsKey(id) ? DictionaryViewItems[id] : null;
        }

        /// <summary>
        /// 获取视图 应该显示的区域 无法获取则显示到默认区域 DocumentRegion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetViewAttachRegionById(string id)
        {
            return DictionaryViewInterfaceItems.ContainsKey(id)
                       ? DictionaryViewInterfaceItems[id].AttachRegion
                       : CoreServices.DocumentRegionName.DocumentRegion;
        }

        /// <summary>
        /// 获取某一父节点view下的所有子view
        /// </summary>
        /// <param name="fatherId">父view  Value</param>
        /// <returns>子view集合  无则 null</returns>
        public static List<object> GetChildViewsByFatherId(int fatherId)
        {
            if (fatherId < 1) return null;
            var lstReturn = new List<object>();
            foreach (var t in DictionaryViewInterfaceItems)
            {
                if (t.Value.fatherId == fatherId)
                {
                    if (DictionaryViewItems.ContainsKey(t.Key))
                    {
                        lstReturn.Add(DictionaryViewItems[t.Key]);
                    }
                }
            }
            return lstReturn;
        }

        /// <summary>
        /// 获取View  Metadata
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static IIViewExport GetViewMetadataById(string  id)
        {
            return DictionaryViewInterfaceItems.ContainsKey(id) ? DictionaryViewInterfaceItems[id] : null;
        }


        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clean()
        {
            try
            {
                DictionaryViewItems.Clear();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 有则更新无则增加
        /// </summary>
        /// <param name="value">View</param>
        /// <param name="publishEvent">是否需要发布事件</param>
        /// <param name="metadata">View metadata</param>
        public void AddViewItem(IIViewExport metadata, object value, bool publishEvent)
        {
            UpdateViewMetadataItem(metadata);
            UpdateViewItem(metadata.Id , value, publishEvent);
        }


        /// <summary>
        /// 更新菜单基础部件 有则更新无则增加
        /// </summary>
        /// <param name="value">View </param>
        /// <param name="publishEvent">是否需要发布事件</param>
        /// <param name="id"> View ID</param>
        public void UpdateViewItem(string  id, object value, bool publishEvent)
        {
            if (DictionaryViewItems.ContainsKey(id))
            {
                DictionaryViewItems[id] = value;
                if (publishEvent)
                {
                    //var args = new PublishEventArgs()
                    //               {
                    //                   EventType = PublishEventTypeCore.ComponentEvent,
                    //                   EventSection = PublishEventSection.Update,
                    //               };
                    //args.AddParams(id);
                    //EventPublisher.EventPublish(args);
                }
            }
            else
            {
                if (!DictionaryViewItems.TryAdd(id, value)) DictionaryViewItems.TryAdd(id, value);

                if (publishEvent)
                {
                    //var args = new PublishEventArgs()
                    //               {
                    //                   EventType = PublishEventTypeCore.ComponentEvent,
                    //                   EventSection = PublishEventSection.Create,
                    //               };
                    //args.AddParams(id);
                    //EventPublisher.EventPublish(args);
                }
            }

        }

        /// <summary>
        /// 更新菜单基础部件 有则更新无则增加
        /// </summary>
        /// <param name="metadata"> View metadata</param>
        public void UpdateViewMetadataItem(IIViewExport metadata)
        {
            if (DictionaryViewInterfaceItems.ContainsKey(metadata.Id ))
            {
                DictionaryViewInterfaceItems[metadata.Id] = metadata;
            }
            else
            {

                if (!DictionaryViewInterfaceItems.TryAdd(metadata.Id, metadata)) DictionaryViewInterfaceItems.TryAdd(metadata.Id, metadata);
            }
        }


        /// <summary>
        /// 删除部件
        /// </summary>
        /// <param name="Id">需要删除的View ID</param>
        /// <param name="publishEvent">是否需要发布删除事件</param>
        public void DeleteViewItem(string  Id, bool publishEvent)
        {
            if (DictionaryViewItems.ContainsKey(Id))
            {
                object obj;
                DictionaryViewItems.TryRemove(Id, out obj);
                if (publishEvent)
                {
                    //var args = new PublishEventArgs()
                    //               {
                    //                   EventType = PublishEventTypeCore.ComponentEvent,
                    //                   EventSection = PublishEventSection.Delete,
                    //               };
                    //args.AddParams(ID);
                    //EventPublisher.EventPublish(args);
                }
            }
            if (DictionaryViewInterfaceItems.ContainsKey(Id))
            {
                IIViewExport obj;
                DictionaryViewInterfaceItems.TryRemove(Id, out obj);
            }
        }
    }

}
