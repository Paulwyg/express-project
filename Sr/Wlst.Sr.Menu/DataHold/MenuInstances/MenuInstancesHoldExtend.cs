using System;
using System.Data;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Models;
using Wlst.Sr.Menu.Services;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Sr.Menu.DataHold.MenuInstances
{

    /// <summary>
    /// 实现对菜单实例的管理
    /// 数据库汉化资源交互
    /// 对应数据库表为 menu_instances(id,name,key,id_classic)
    /// </summary>
    public class MenuInstancesHoldExtend : MenuInstancesHold
    {
        //menu_instances(id,name,key,id_classic)

        /// <summary>
        /// 
        /// </summary>
        public MenuInstancesHoldExtend()
        {
            InnerLoad();
        }


        /// <summary>
        /// 从数据库中读取资源数据 
        /// 在程序初始化的时候执行
        /// </summary>
        private   void InnerLoad()
        {
            var ssss = Convert.ToInt32(SqlLiteHelper.ExecuteQuery(
                "SELECT COUNT(*) as count FROM sqlite_master WHERE type='table' and name= 'menu_instances'").
                                           Tables[0].Rows[0][0].ToString().Trim());


            if (ssss < 1)
            {
                SqlLiteHelper.ExecuteQuery(
                    "CREATE TABLE 'menu_instances' ('id' integer PRIMARY KEY,'name' text,'key' text,'" +
                    "id_classic' integer)");
            }


            try
            {
                DataSet ds = SqlLiteHelper.ExecuteQuery("select * from menu_instances", null);
                if (ds == null) return;
                int mCount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < mCount; i++)
                {
                    try
                    {
                        // (id integer NOT NULL,tag text,name text NOT NULL,tooltips text)
                        int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString().Trim());
                        if (id >= MenuIdControlAssign.MenuInstanceKeyIdMin &&
                            id <= MenuIdControlAssign.MenuInstancesKeyIdMax)
                        {
                            string name = ds.Tables[0].Rows[i]["name"].ToString().Trim();
                            string key = ds.Tables[0].Rows[i]["key"].ToString().Trim();
                            int idClassic = Convert.ToInt32(ds.Tables[0].Rows[i]["id_classic"].ToString().Trim());
                            this.AddMenuInstances(id, name, key, idClassic);
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
                    "Class MenuInstancesHoldingExtend Function loadItem from SQLlite table menu_instances  Occer an Error:" +
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
        /// 增加菜单实例
        /// </summary>
        /// <param name="keyId">唯一地址</param>
        /// <param name="name">名称</param>
        /// <param name="keyMenu"></param>
        /// <param name="idClassic"></param>
        private  void AddMenuInstances(int keyId, string name, string keyMenu, int idClassic)
        {
            if (!DicInstances.ContainsKey(keyId))
            {
                DicInstances.Add(keyId,
                               new MenuInstance()
                               {
                                   Id = keyId,
                                   IdClassic = idClassic,
                                   Key = keyMenu,
                                   Name = name
                               });
            }
            else
            {
                DicInstances[keyId].Id = keyId;
                DicInstances[keyId].IdClassic = idClassic;
                DicInstances[keyId].Key = keyMenu;
                DicInstances[keyId].Name = name;
            }
        }

        /// <summary>
        /// 更新菜单实例 无则增加，回写数据库
        /// </summary>
        /// <param name="keyId">唯一标示</param>
        /// <param name="name"></param>
        /// <param name="keyMenu"></param>
        /// <param name="idClassic"></param>
        public void UpdateMenuInstances(int keyId, string name, string keyMenu, int idClassic)
        {
            if (keyId >= MenuIdControlAssign.MenuInstanceKeyIdMin &&
                keyId <= MenuIdControlAssign.MenuInstancesKeyIdMax)
            {
            }
            else
            {
                return;
            }
            if (!DicInstances.ContainsKey(keyId))
            {
                DicInstances.Add(keyId,
                                 new MenuInstance()
                                     {
                                         Id = keyId,
                                         IdClassic = idClassic,
                                         Key = keyMenu,
                                         Name = name
                                     });
            }
            else
            {
                DicInstances[keyId].Id = keyId;
                DicInstances[keyId].IdClassic = idClassic;
                DicInstances[keyId].Key = keyMenu;
                DicInstances[keyId].Name = name;
            }
            WriteUpdateDbByPrivate(keyId);

            var args = new PublishEventArgs()
                           {
                               EventId =
                                   EventIdAssign.
                                   MenuInstanceUpdate,
                               EventType = PublishEventType.Core
                           };
            args.AddParams(keyId);
            EventPublish.PublishEvent(args);
        }

        /// <summary>
        /// 删除菜单实例，回写数据库
        /// </summary>
        /// <param name="keyId"></param>
        public void DeleteMenuInstances(int keyId)
        {
            if (DicInstances.ContainsKey(keyId))
            {
                DicInstances.Remove(keyId);

                var args = new PublishEventArgs()
                {
                    EventId =
                        EventIdAssign.
                        MenuInstanceUpdate,
                    EventType = PublishEventType.Core
                };
                args.AddParams(keyId);
                EventPublish.PublishEvent(args);
                WriteDeleteDbByPrivate(keyId);
            }
        }

        ///// <summary>
        ///// 所有数据会写数据库
        ///// </summary>
        //public void WriteUpdateDb()
        //{
        //    WriteUpdateDbByPrivate();
        //}


        ///// <summary>
        ///// 会写数据库
        ///// </summary>
        //public void WriteUpdateDb(int id)
        //{
        //    WriteUpdateDbByPrivate(id);
        //}

        //"CREATE TABLE 'menu_instances' ('id' integer PRIMARY KEY,'name' text,'key' text,'" +
        //           "id_classic' integer)");

        private void WriteDeleteDbByPrivate(int id)
        {
            SqlLiteHelper.ExecuteNonQuery("delete from menu_instances where id=" + id);
        }

        private void WriteUpdateDbByPrivate()
        {
            SqlLiteHelper.ExecuteNonQuery("delete from menu_instances ");
            foreach (var t in DicInstances)
            {
                string strUpdateDirectroy = "insert into menu_instances(id,name,key,id_classic) values";
                strUpdateDirectroy += "(" + t.Key + ",'" + t.Value.Name + "','" + t.Value.Key + "'," + t.Value.IdClassic +
                                      ")";
                try
                {
                    SqlLiteHelper.ExecuteNonQueryInThread(strUpdateDirectroy);
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
        }
        private void WriteUpdateDbByPrivate(int id)
        {
            SqlLiteHelper.ExecuteNonQuery("delete from menu_instances where id=" + id);
            if (!DicInstances.ContainsKey(id)) return;
            var t = DicInstances[id];
            string strUpdateDirectroy = "insert into menu_instances(id,name,key,id_classic) values";
            strUpdateDirectroy += "(" + t.Id + ",'" + t.Name + "','" + t.Key + "'," + t.IdClassic +
                                  ")";
            try
            {
                SqlLiteHelper.ExecuteNonQueryInThread(strUpdateDirectroy);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
    }
}
