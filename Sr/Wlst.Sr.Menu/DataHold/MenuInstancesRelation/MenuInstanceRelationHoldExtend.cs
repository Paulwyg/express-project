using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Sr.Menu.DataHold.MenuInstancesRelation
{

    /// <summary>
    /// 实现对菜单实例关系的管理功能
    /// 对应数据库表为 menu_instances_relation  
    /// </summary>
    public class MenuInstanceRelationHoldExtend : MenuInstanceRelationHold
    {
   
        /// <summary>
        /// 
        /// </summary>
        public MenuInstanceRelationHoldExtend()
        {
            InnerLoad();
        }


        /// <summary>
        /// 从数据库中读取资源数据 
        /// 在程序初始化的时候执行
        /// </summary>
        private   void InnerLoad()
        {
            DicInstanceRelation.Clear();
            var ssss = Convert.ToInt32(SqlLiteHelper.ExecuteQuery(
                "SELECT COUNT(*) as count FROM sqlite_master WHERE type='table' and name= 'menu_instances_relation'").
                                           Tables[0].Rows[0][0].ToString().Trim());


            if (ssss < 1)
            {
                SqlLiteHelper.ExecuteQuery(
                    "CREATE TABLE 'menu_instances_relation' ('father_id' integer,'id' integer,'sort_index' integer,'name' text,'" +
                    "instances_id' integer)");
            }


            try
            {
                DataSet ds = SqlLiteHelper.ExecuteQuery("select * from menu_instances_relation", null);
                if (ds == null) return;
                int mCount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < mCount; i++)
                {
                    try
                    {
                        // (id integer NOT NULL,tag text,name text NOT NULL,tooltips text)
                        int fatherId = Convert.ToInt32(ds.Tables[0].Rows[i]["father_id"].ToString().Trim());
                        int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString().Trim());

                        if ((id >= MenuIdControlAssign.MenuFileGroupIdMin &&
                             id <= MenuIdControlAssign.MenuFileGroupIdMax) ||
                            (id >= MenuIdControlAssign.MenuIdMin &&
                             id <= MenuIdControlAssign.MenuIdMax))
                        {
                            int sortIndex = Convert.ToInt32(ds.Tables[0].Rows[i]["sort_index"].ToString().Trim());
                            string name = ds.Tables[0].Rows[i]["name"].ToString().Trim();
                            int instancesId = Convert.ToInt32(ds.Tables[0].Rows[i]["instances_id"].ToString().Trim());
                            this.UpdateMenuInstanceRelation(fatherId, id, sortIndex, name, instancesId);
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "Class MenuInstanceRelationHoldingExtend Function loadItem from SQLlite table menu_instances_relation  Occer an Error:" +
                    ex.ToString());
            }
            var args = new PublishEventArgs()
            {
                EventId =
                    EventIdAssign.MenuInstanceRelationLoadUpdate ,
                EventType = PublishEventType.Core
            };

            EventPublish.PublishEvent(args);
        }





        /// <summary>
        /// 更新实例菜单 无则增加
        /// </summary>
        /// <param name="sortIndex"> </param>
        /// <param name="name">实例菜单名称</param>
        /// <param name="fahterId">父节点为0标示为根节点 </param>
        /// <param name="id"> </param>
        /// <param name="instancesId">菜单实例标示值 </param>
        private  void UpdateMenuInstanceRelation(int fahterId, int id, int sortIndex, string name, int instancesId)
        {
           
            if (!DicInstanceRelation.ContainsKey(instancesId))
                DicInstanceRelation.Add(instancesId, new Dictionary<Tuple<int, int>, Wlst.Sr.Menu.Models.MenuInstancesRelation>());
            var keyKey = new Tuple<int, int>(fahterId, id);
            if (!DicInstanceRelation[instancesId].ContainsKey(keyKey))
            {
                DicInstanceRelation[instancesId].Add(keyKey,
                                            new Wlst.Sr.Menu.Models.MenuInstancesRelation()
                                                {
                                                    FatherId = fahterId,
                                                    Id = id,
                                                    SortIndex = sortIndex,
                                                    Name = name,
                                                    InstancesId = instancesId
                                                }
                    );
            }
            else
            {
                DicInstanceRelation[instancesId][keyKey].FatherId = fahterId;
                DicInstanceRelation[instancesId][keyKey].Id = id;
                DicInstanceRelation[instancesId][keyKey].SortIndex = sortIndex;
                DicInstanceRelation[instancesId][keyKey].Name = name;
                DicInstanceRelation[instancesId][keyKey].InstancesId = instancesId;
            }
        }

        /// <summary>
        /// 增加实例关系
        /// </summary>
        /// <param name="sortIndex"> </param>
        /// <param name="name">实例关系名称</param>
        /// <param name="fahterId">父节点为0标示为根节点 </param>
        /// <param name="id"> </param>
        /// <param name="instancesId"> 菜单实例标示值</param>
        public void AddMenuInstanceRelation(int fahterId, int id, int sortIndex, string name, int instancesId)
        {
            this.UpdateMenuInstanceRelation(fahterId, id, sortIndex, name, instancesId);
        }


        /// <summary>
        /// 删除实例关系 
        /// </summary>
        /// <param name="instancesId">菜单实例地址</param>
        /// <param name="fatherId"> </param>
        /// <param name="id">需要删除的菜单地址id</param>
        public void DeleteMenuInstanceRelation(int instancesId,int fatherId, int id)
        {
            if (!DicInstanceRelation.ContainsKey(instancesId)) return;
            if (!DicInstanceRelation[instancesId].ContainsKey(new Tuple<int, int>( fatherId ,id ))) return;
            DicInstanceRelation[instancesId].Remove(new Tuple<int, int>( fatherId ,id ));
        }

        /// <summary>
        /// 删除实例关系,立即回写数据库并发布事件
        /// </summary>
        /// <param name="instancesId"></param>
        public void DeleteMenuInstanceRelation(int instancesId)
        {
            if (DicInstanceRelation.ContainsKey(instancesId))
            {
                DicInstanceRelation.Remove(instancesId);
                var args = new PublishEventArgs()
                               {
                                   EventType = PublishEventType.Core,
                                   EventId = EventIdAssign.MenuInstanceRelationUpdate
                               };
                args.AddParams(instancesId);
                EventPublish.PublishEvent(args);
            }
            WriteDeleteDbByPrivate(instancesId);
        }

        /// <summary>
        /// 更新菜单实例信息 
        /// </summary>
        /// <param name="instanceId">菜单实例地址 </param>
        /// <param name="instanceKey">该实例菜单关键字 </param>
        /// <param name="lst">所有菜单实例信息的集合，必须包含一个节点信息为 fatherId=0的根节点</param>
        public void UpdateMenuInstanceRelation(int instanceId,string instanceKey, IEnumerable<Wlst.Sr.Menu.Models.MenuInstancesRelation> lst)
        {
            if (DicInstanceRelation.ContainsKey(instanceId)) DicInstanceRelation[instanceId].Clear();
            else DicInstanceRelation.Add(instanceId, new Dictionary<Tuple<int, int>, Wlst.Sr.Menu.Models.MenuInstancesRelation>());
            foreach (var t in lst)
            {
                this.AddMenuInstanceRelation(t.FatherId, t.Id, t.SortIndex, t.Name, t.InstancesId);
            }
            var args = new PublishEventArgs()
                           {
                               EventType = PublishEventType.Core,
                               EventId = EventIdAssign.MenuInstanceRelationUpdate
                           };
            args.AddParams(instanceId);
            args.AddParams(instanceKey);
            EventPublish.PublishEvent(args);

            WriteUpdateDbByPrivate(instanceId);
        }

   
        ///// <summary>
        ///// 所有数据会写数据库
        ///// </summary>
        //public void WriteUpdateDb(int instancesId)
        //{
        //    WriteUpdateDbByPrivate(instancesId);
        //}
        private  void WriteDeleteDbByPrivate(int instancesId)
        {
            SqlLiteHelper.ExecuteNonQuery("delete from menu_instances_relation where instances_id=" + instancesId);
        }

        private void WriteUpdateDbByPrivate(int instancesId)
        {
            //menu_instances_relation id(father_id,id,sort_index,name,instances_id)
            SqlLiteHelper.ExecuteNonQuery("delete from menu_instances_relation where instances_id=" + instancesId);
            if (!DicInstanceRelation.ContainsKey(instancesId)) return;
            foreach (var t in DicInstanceRelation[instancesId])
            {
                try
                {
                    string strUpdateDirectroy =
                        "insert into menu_instances_relation(father_id,id,sort_index,name,instances_id) values";
                    strUpdateDirectroy += "(" + t.Value.FatherId + "," + t.Value.Id + "," + t.Value.SortIndex + ",'" +
                                          t.Value.Name + "'," + t.Value.InstancesId + ")";
                    SqlLiteHelper.ExecuteNonQueryInThread(strUpdateDirectroy);
                }
                catch (Exception ex)
                {
                    WriteLog.WriteLogError(
                        "Class MenuInstanceRelationHoldingExtend Function WriteUpdateDbByPrivate from SQLlite table menu_instances_relation  Occer an Error:" +
                        ex.ToString());
                }
            }
        }

    }
}
