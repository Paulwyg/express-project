using System;
using System.Collections.Generic;
using System.Data;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Models;
using Wlst.Sr.Menu.Services;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Sr.Menu.DataHold.Classic
{
    /// <summary>
    /// 实现对菜单资源的管理功能
    /// 对应数据库表为 menu_classic  
    /// </summary>
    public class ClassicDataHoldExtend : ClassicDataHold
    {
        /// <summary>
        /// 
        /// </summary>
        public ClassicDataHoldExtend()
        {
            InnerLoad();
        }

        /// <summary>
        /// 从数据库中读取资源数据 ,在程序初始化的时候执行,模块内部会执行，其他地方不允许执行
        /// </summary>
        private void InnerLoad()
        {
            DicClassic.Clear();
            var ssss = Convert.ToInt32(SqlLiteHelper.ExecuteQuery(
                "SELECT COUNT(*) as count FROM sqlite_master WHERE type='table' and name= 'menu_classic'").
                                           Tables[0].Rows[0][0].ToString().Trim());


            if (ssss < 1)
            {
                SqlLiteHelper.ExecuteQuery(
                    "CREATE TABLE 'menu_classic' ('id' integer PRIMARY KEY,'name' text,'" +
                    "content' text)");
            }


            try
            {
                DataSet ds = SqlLiteHelper.ExecuteQuery("select * from menu_classic", null);
                if (ds == null) return;
                int mCount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < mCount; i++)
                {
                    try
                    {
                        // (id integer NOT NULL,tag text,name text NOT NULL,tooltips text)
                        int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString().Trim());
                        if (id >= MenuIdControlAssign.MenuClassicIdMin &&
                            id <= MenuIdControlAssign.MenuClassicIdMax)
                        {
                            string name = ds.Tables[0].Rows[i]["name"].ToString().Trim();
                            string content = ds.Tables[0].Rows[i]["content"].ToString().Trim();
                            this.UpdateMneu(id, name, content);
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
                    "Class SupperClassicDataHoldingExtend Function loadItem from SQLlite table menu_classic  Occer an Error:" +
                    ex.ToString());
            }

            var args = new PublishEventArgs()
            {
                EventId =
                    EventIdAssign.ClassicMenuLoadUpdate ,
                EventType = PublishEventType.Core
            };
   
            EventPublish.PublishEvent(args);
        }

        /// <summary>
        /// 更新模板菜单 无则增加,并回写数据库
        /// </summary>
        /// <param name="keyId">唯一标示</param>
        /// <param name="name">模板菜单名称</param>
        /// <param name="content">模板菜单包含的菜单集合  以#分割开</param>
        private  void UpdateMneu(int keyId, string name, string content)
        {
            if (MenuIdControlAssign.MenuClassicIdMin <= keyId && keyId <= MenuIdControlAssign.MenuClassicIdMax)
            {

            }
            else
            {
                return;
            }
            if (!DicClassic.ContainsKey(keyId))
            {
                DicClassic.Add(keyId, new MenuClassic() {Id = keyId, Name = name});
            }
            DicClassic[keyId].Name = name;

            string[] sp = content.Split('#');
            foreach (var t in sp)
            {
                if (string.IsNullOrEmpty(t)) continue;
                try
                {
                    int x = Convert.ToInt32(t);
                    if (x > 0)
                    {
                        if (!DicClassic[keyId].Items.Contains(x))
                            DicClassic[keyId].Items.Add(x);
                    }
                }
                catch (Exception ex)
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                }
            }
        }

        /// <summary>
        /// 更新模板菜单 无则增加,并回写数据库
        /// </summary>
        /// <param name="keyId">唯一标示</param>
        /// <param name="name">模板菜单名称</param>
        /// <param name="content">模板菜单包含的菜单集合</param>
        public void UpdateMneu(int keyId, string name, List<int> content)
        {
            if (MenuIdControlAssign.MenuClassicIdMin <= keyId && keyId <= MenuIdControlAssign.MenuClassicIdMax)
            {

            }
            else
            {
                return;
            }
            if (!DicClassic.ContainsKey(keyId))
            {
                DicClassic.Add(keyId, new MenuClassic() {Id = keyId, Name = name});
            }
            DicClassic[keyId].Name = name;

            DicClassic[keyId].Items.Clear();
            foreach (var t in content)
            {
                if (!DicClassic[keyId].Items.Contains(t))
                    DicClassic[keyId].Items.Add(t);
            }
            this.WriteUpdateDbByPrivate(keyId);
            var args = new PublishEventArgs()
            {
                EventId =
                    EventIdAssign.ClassicMenuUpdate,
                EventType = PublishEventType.Core
            };
            args.AddParams(keyId);
           EventPublish.PublishEvent(args);
        }


        /// <summary>
        /// 删除模板菜单,并删除数据库中保留的模板菜单
        /// </summary>
        /// <param name="keyId"></param>
        public void DeleteMneu(int keyId)
        {
            if (DicClassic.ContainsKey(keyId))
            {
                DicClassic.Remove(keyId);
                WriteDeleteDbByPrivate(keyId);

                var args = new PublishEventArgs()
                {
                    EventId =
                        EventIdAssign.ClassicMenuUpdate,
                    EventType = PublishEventType.Core
                };
                args.AddParams(keyId);
                EventPublish.PublishEvent(args);
            }
        }


        private void WriteDeleteDbByPrivate(int keyId)
        {
            SqlLiteHelper.ExecuteNonQuery("delete from menu_classic where id=" + keyId);
        }

        private void WriteUpdateDbByPrivate(int keyId)
        {
            SqlLiteHelper.ExecuteNonQuery("delete from menu_classic where id=" + keyId);
            if (!DicClassic.ContainsKey(keyId)) return;
            string content = "";
            foreach (var t in DicClassic[keyId].Items)
            {
                content += t;
                content += "#";
            }

            string strUpdateDirectroy = "insert into menu_classic(id,name,content) values";
            strUpdateDirectroy += "(" + keyId + ",'" + DicClassic[keyId].Name + "','" + content + "')";
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
