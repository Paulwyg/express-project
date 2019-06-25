using System;
using System.Collections.Generic;
using System.Linq;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.Setting.Interfaces;

namespace Wlst.Cr.Setting.SettingHolding
{
    /// <summary>
    /// 菜单基础部件holding 
    /// </summary>
    public class SettingComponentHolding //: IPartImportsSatisfiedNotification
    {
        private static Dictionary<int, IISetting> _dictionaryMenuItems = new Dictionary<int, IISetting>();

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
        public static bool ContainsComponent(IISetting menuItem)
        {
            return _dictionaryMenuItems.ContainsKey(menuItem.Id);
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
        public static IISetting GetMenuItemById(int id)
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
        public void AddMenuItem(IISetting menuItem)
        {
            UpdateMenuItem(menuItem);
        }


        /// <summary>
        /// 更新菜单基础部件 有则更新无则增加
        /// </summary>
        /// <param name="menuItem">基础部件</param>
        public void UpdateMenuItem(IISetting menuItem)
        {
            if (_dictionaryMenuItems.ContainsKey(menuItem.Id))
            {
                _dictionaryMenuItems[menuItem.Id] = menuItem;
                var args = new PublishEventArgs()
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = Services.EventIdAssign.SettingModuleComponentUpdate,
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
                                   EventId = Services.EventIdAssign.SettingModuleComponentAdd,
                               };
                args.AddParams(menuItem.Id);
                EventPublish.PublishEvent(args);
            }

        }

        /// <summary>
        /// 删除部件
        /// </summary>
        /// <param name="menuItem">需要删除的部件</param>
        public void DeleteMenuItem(IISetting menuItem)
        {
            if (_dictionaryMenuItems.ContainsKey(menuItem.Id))
            {

                _dictionaryMenuItems.Remove(menuItem.Id);
                var args = new PublishEventArgs()
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = Services.EventIdAssign.SettingModuleComponentDelete,
                               };
                args.AddParams(menuItem.Id);
                EventPublish.PublishEvent(args);
            }
        }

        /// <summary>
        /// 删除未出现在重组后的列表中的部件
        /// </summary>
        /// <param name="lstShouldKeepItemsId">需要保留的部件列表清单,函数处理后仅包含此列表数据</param>
        public void UpdateMenuItem(List<IISetting> lstShouldKeepItemsId)
        {
            var lstKeeps = lstShouldKeepItemsId.Select(t => t.Id).ToList();


            foreach (var t in lstShouldKeepItemsId)
            {
                if (!_dictionaryMenuItems.ContainsKey(t.Id)) this.AddMenuItem(t);
            }
            var lstNeedDelete = new List<int>();
            foreach (var t in _dictionaryMenuItems.Keys)
            {
                if (lstKeeps.Contains(t)) continue;
                if (!lstNeedDelete.Contains(t)) lstNeedDelete.Add(t);
            }
            if (lstNeedDelete.Count > 0)
            {
                var args = new PublishEventArgs()
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = Services.EventIdAssign.SettingModuleComponentDelete,
                               };
                foreach (var t in lstNeedDelete)
                {
                    if (_dictionaryMenuItems.ContainsKey(t))
                    {
                        _dictionaryMenuItems.Remove(t);
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
        public void DeleteMenuItem(int id)
        {
            if (_dictionaryMenuItems.ContainsKey(id))
            {
                _dictionaryMenuItems.Remove(id);
                var args = new PublishEventArgs()
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = Services.EventIdAssign.SettingModuleComponentUpdate,
                               };
                args.AddParams(id);
                EventPublish.PublishEvent(args);
            }
        }
    }
}
