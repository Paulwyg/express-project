using System;
using System.Collections.Generic;
using Wlst.Sr.Menu.DataHold.MenuInstances;
using Wlst.Sr.Menu.DataHold.MenuInstancesRelation;
using Wlst.Sr.Menu.Models;

namespace Wlst.Sr.Menu.Services
{
    public class ServerInstanceRelation
    {
        private static MenuInstanceRelationHoldExtend instancesRelation =
            new MenuInstanceRelationHoldExtend();

        /// <summary>
        /// 删除实例关系,立即回写数据库并发布事件
        /// </summary>
        /// <param name="instancesId"></param>
        public static  void DeleteMenuInstanceRelation(int instancesId)
        {
            instancesRelation.DeleteMenuInstanceRelation(instancesId);
        }

        /// <summary>
        /// 增加实例关系,仅在程序内增加，未回写数据库，为注册菜单节点提供
        /// </summary>
        /// <param name="sortIndex"> </param>
        /// <param name="name">实例关系名称</param>
        /// <param name="fahterId">父节点为0标示为根节点 </param>
        /// <param name="id"> </param>
        /// <param name="instancesId"> 菜单实例标示值</param>
        public static  void AddMenuInstanceRelation(int fahterId, int id, int sortIndex, string name, int instancesId)
        {
            instancesRelation.AddMenuInstanceRelation(fahterId, id, sortIndex, name, instancesId);
        }

        /// <summary>
        /// 更新菜单实例信息 
        /// </summary>
        /// <param name="instanceId">菜单实例地址 </param>
        /// <param name="instanceKey">该实例菜单关键字 </param>
        /// <param name="lst">所有菜单实例信息的集合，必须包含一个节点信息为 fatherId=0的根节点</param>
        public static  void UpdateMenuInstanceRelation(int instanceId, string instanceKey, IEnumerable<MenuInstancesRelation> lst)
        {
            instancesRelation.UpdateMenuInstanceRelation(instanceId,instanceKey , lst);
        }


        /// <summary>
        /// 获取菜单实例关系映射信息,所有的信息
        /// </summary>
        /// <param name="instanceId">菜单实例关系映射Id值 </param>
        /// <returns>菜单实例关系映射信息 不存在则返回null</returns>
        public static Dictionary<Tuple<int, int>, MenuInstancesRelation> GetInstanceRelation(int instanceId)
        {
            return instancesRelation.GetInstanceRelation(instanceId);
        }

        public static MenuInstancesRelation GetInstanceRelation(int instanceId, int fatherId, int menuId)
        {
            return instancesRelation.GetInstanceRelation(instanceId, fatherId, menuId);
        }

        /// <summary>
        /// 返回的实例菜单该父节点下的所有点均按sortindex排序
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="fatherId"></param>
        /// <returns>返回的实例菜单该父节点下的所有点均按sortindex排序</returns>
        public static List<MenuInstancesRelation> GetInstanceRelationsByfatherId(int instanceId, int fatherId)
        {
            return instancesRelation.GetInstanceRelationsByfatherId(instanceId, fatherId);

        }

        /// <summary>
        /// 获取整个数据
        /// </summary>
        public static Dictionary<int, Dictionary<Tuple<int, int>, MenuInstancesRelation>> GetInstanceRelatioinDic
        {
            get { return instancesRelation.GetInstanceRelatioinDic; }
        }


        /// <summary>
        /// 获取可用的实例菜单值，返回值即可用的
        /// </summary>
        /// <returns></returns>
        public static int GetMaxAviableMenuFileId()
        {
            return instancesRelation.GetMaxAviableMenuFileId();
        }




        /// <summary>
        /// 获取菜单实例关系映射信息,所有的信息,fatherid为0 则表示为根id，返回的字典关键字key为id值，使用时注意
        /// </summary>
        /// <returns>菜单实例关系映射信息 不存在则返回null</returns>
        public static Dictionary<Tuple<int, int>, MenuInstancesRelation> GetInstancesRelationByMenuKey(string key)
        {
            int instanceId = -1;
            foreach (var t in Services .ServerInstanceRoot .GetInstancesDic )
            {
                if (t.Value.Key.Equals(key))
                {
                    instanceId = t.Key;
                    break;
                }
            }
            if (instanceId == -1) return null;
            return instancesRelation.GetInstanceRelation(instanceId);
        }


        /// <summary>
        /// 获取实例信息 通过父id值获取子列表
        /// </summary>
        /// <param name="menuKey"></param>
        /// <param name="fatherId"></param>
        /// <returns>不存在返回空 null</returns>
        public static List<MenuInstancesRelation> GetInstanceRelationsByfatherId(string menuKey, int fatherId)
        {
            int instanceId = -1;
            foreach (var t in Services.ServerInstanceRoot.GetInstancesDic)
            {
                if (t.Value.Key.Equals(menuKey))
                {
                    instanceId = t.Key;
                    break;
                }
            }
            if (instanceId == -1) return null;
            return instancesRelation.GetInstanceRelationsByfatherId(instanceId, fatherId);
        }


    }
}
