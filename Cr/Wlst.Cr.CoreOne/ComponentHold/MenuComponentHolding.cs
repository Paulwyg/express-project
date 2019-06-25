using System;
using System.Collections.Generic;
using System.Linq;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreOne.CoreIdAssign;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;

namespace Wlst.Cr.CoreOne.ComponentHold
{
    /// <summary>
    /// 菜单基础部件holding 
    /// </summary>
    public class MenuComponentHolding //: IPartImportsSatisfiedNotification
    {
        private static Dictionary<int, IIMenuItem> _dictionaryMenuItems = new Dictionary<int, IIMenuItem>();

        /// <summary>
        /// <param>获取菜单基础部件</param>
        /// <param>允许调用部件内部对输出内容进行设置不允许进行任何的修改</param>
        /// </summary>
        /// <param name="startId">起始ID号 大于</param>
        /// <param name="endId">结束ID号 小于等于</param>
        /// <returns></returns>
        public static List<IIMenuItem> GetLstMenu(int startId, int endId)
        {
            var lstReturn = new List<IIMenuItem>();
            foreach (int intKey in _dictionaryMenuItems.Keys)
                if (intKey > startId && intKey <= endId)
                    lstReturn.Add(_dictionaryMenuItems[intKey]);

            return lstReturn;
        }

        /// <summary>
        /// 获取部件数量
        /// </summary>
        public static int Count
        {
            get { return _dictionaryMenuItems.Count; }
        }

        /// <summary>
        /// 获取是否已经包含该部件
        /// </summary>
        /// <param name="menuItem">菜单</param>
        /// <returns></returns>
        public static bool ContainsComponent(IIMenuItem menuItem)
        {
            return _dictionaryMenuItems.ContainsKey(menuItem.Id);
        }

        public static Dictionary<int, IIMenuItem> GetAllMenuItem
        {
            get { return _dictionaryMenuItems; }
        }

        /// <summary>
        /// 获取所有菜单部件ID
        /// </summary>
        /// <returns></returns>
        public static ICollection<int> GetAllComponentIDs
        {
            get
            {

                var lst = new List<int>();
                lst.AddRange(from t in _dictionaryMenuItems
                             orderby t.Key
                             select t.Key);
                return lst;

                //var lst = _dictionaryMenuItems.Select(t => t.Key).ToList();
                return _dictionaryMenuItems.Keys;
            }
        }


        /// <summary>
        /// 获取指定id值的部件 不存在则返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>存在则返回部件 不存在则返回null</returns>
        public static IIMenuItem GetMenuItemById(int id)
        {
            return _dictionaryMenuItems.ContainsKey(id) ? _dictionaryMenuItems[id] : null;
        }


        /// <summary>
        /// 清除数据
        /// </summary>
        public void Clean()
        {
            try
            {
                _dictionaryMenuItems.Clear();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 有则更新无则增加
        /// </summary>
        /// <param name="menuItem">基础部件</param>
        public void AddMenuItem(IIMenuItem menuItem)
        {
            UpdateMenuItem(menuItem);
        }


        /// <summary>
        /// 更新菜单基础部件 有则更新无则增加
        /// </summary>
        /// <param name="menuItem">基础部件</param>
        public void UpdateMenuItem(IIMenuItem menuItem)
        {
            if (_dictionaryMenuItems.ContainsKey(menuItem.Id))
            {
                _dictionaryMenuItems[menuItem.Id] = menuItem;

                    var args = new PublishEventArgs()
                    {
                        EventType = PublishEventType.Core,
                        EventId = EventIdAssign.MenuComponentUpdate,
                    };
                    args.AddParams(menuItem.Id);
                    EventPublish.PublishEvent(args);
            }
            else
            {
                _dictionaryMenuItems.Add(menuItem.Id, menuItem);
                    var args = new PublishEventArgs()
                    {
                        EventType = PublishEventType.Core,
                        EventId = EventIdAssign.MenuComponentAdd,
                    };
                    args.AddParams(menuItem.Id);
                    EventPublish.PublishEvent(args);
                

            }

        }

        /// <summary>
        /// 删除部件
        /// </summary>
        /// <param name="menuItem">需要删除的部件</param>
        public void DeleteMenuItem(IIMenuItem menuItem)
        {
            if (_dictionaryMenuItems.ContainsKey(menuItem.Id))
            {

                _dictionaryMenuItems.Remove(menuItem.Id);
                    var args = new PublishEventArgs()
                    {
                        EventType = PublishEventType.Core,
                        EventId = EventIdAssign.MenuComponentDelete,
                    };
                    args.AddParams(menuItem.Id);
                    EventPublish.PublishEvent(args);
                
            }
        }

        /// <summary>
        /// 更新菜单列表 ，仅保留参数中的列表
        /// </summary>
        /// <param name="lstShouldKeepItemsId">更新菜单列表仅保留参数中的列表</param>
        public void UpdateMenuItem(List<IIMenuItem> lstShouldKeepItemsId)
        {
            var lstKeep = new List<int>();
            foreach (var t in lstShouldKeepItemsId)
            {
                if (!lstKeep.Contains(t.Id)) lstKeep.Add(t.Id);
            }

            var lstNeedDelete = new List<int>();
            foreach (var t in _dictionaryMenuItems.Keys)
            {
                if (lstKeep.Contains(t)) continue;
                if (!lstNeedDelete.Contains(t)) lstNeedDelete.Add(t);
            }

            if (lstNeedDelete.Count > 0)
            {
                var args = new PublishEventArgs()
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = EventIdAssign.MenuComponentDelete,
                               };

                foreach (var t in lstNeedDelete)
                {
                    if (_dictionaryMenuItems.ContainsKey(t)) _dictionaryMenuItems.Remove(t);
                    args.AddParams(t);
                }
                EventPublish.PublishEvent(args);
            }


            foreach (var t in lstShouldKeepItemsId)
            {
                var args = new PublishEventArgs()
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = EventIdAssign.MenuComponentAdd,
                               };
                if (!_dictionaryMenuItems.ContainsKey(t.Id))
                {
                    _dictionaryMenuItems.Add(t.Id, t);
                    args.AddParams(t.Id);
                }
                if (args.GetParams().Count > 0) EventPublish.PublishEvent(args);
            }
        }


        /// <summary>
        /// 删除部件  
        /// </summary>
        /// <param name="id">需要删除的部件id</param>
        public void DeleteMenuItem(int id)
        {
            if (_dictionaryMenuItems.ContainsKey(id))
            {
                _dictionaryMenuItems.Remove(id);
                    var args = new PublishEventArgs()
                    {
                        EventType = PublishEventType.Core,
                        EventId = EventIdAssign.MenuComponentDelete  ,
                    };
                    args.AddParams(id);
                    EventPublish.PublishEvent(args);
                
            }
        }
    }
}
