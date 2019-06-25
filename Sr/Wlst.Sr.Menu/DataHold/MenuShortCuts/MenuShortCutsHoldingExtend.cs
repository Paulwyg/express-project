using System;
using System.Data;
using System.Windows;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.Menu.Services;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Sr.Menu.DataHold.MenuShortCuts
{

    /// <summary>
    /// 实现对菜单资源的汉化功能，汉化数据均保持于此，菜单名字需要汉化需在此实现
    /// 数据库汉化资源交互
    /// 对应数据库表为 menu_shortcuts(id,name,tooltips)
    /// </summary>
    public class MenuShortCutsHoldingExtend : MenuShortCutsHolding
    {
        //menu_eng(id,name,tooltips)

        /// <summary>
        /// 
        /// </summary>
        public MenuShortCutsHoldingExtend()
        {
            InnerLoad();
            this.RegisterShortCuts();
        }

        private void RegisterShortCuts()
        {
            if (RegisteShortCuts.MySelf == null)
            {
                new RegisteShortCuts();
            }
            try
            {
                Application.Current.MainWindow.KeyDown -= new System.Windows.Input.KeyEventHandler(MainWindow_KeyDown);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            Application.Current.MainWindow.KeyDown += new System.Windows.Input.KeyEventHandler(MainWindow_KeyDown);
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (RegisteShortCuts.MySelf != null)
                RegisteShortCuts.MySelf.OnKeyDown(e);
        }


        /// <summary>
        /// 从数据库中读取资源数据 
        /// 在程序初始化的时候执行
        /// </summary>
        private  void InnerLoad()
        {
            var ssss = Convert.ToInt32(SqlLiteHelper.ExecuteQuery(
                "SELECT COUNT(*) as count FROM sqlite_master WHERE type='table' and name= 'menu_shortcuts'").
                                           Tables[0].Rows[0][0].ToString().Trim());


            if (ssss < 1)
            {
                SqlLiteHelper.ExecuteQuery(
                    "CREATE TABLE 'menu_shortcuts' ('id' integer PRIMARY KEY,'shortcut' text)");
            }


            try
            {
                DataSet ds = SqlLiteHelper.ExecuteQuery("select * from menu_shortcuts", null);
                if (ds == null) return;
                int mCount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < mCount; i++)
                {
                    try
                    {
                        // (id integer NOT NULL,tag text,name text NOT NULL,tooltips text)
                        int id = Convert.ToInt32(ds.Tables[0].Rows[i]["id"].ToString().Trim());
                        string shortcut = ds.Tables[0].Rows[i]["shortcut"].ToString().Trim();
                        this.AddShortCut(id, shortcut);
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
                    "Class MenuShortCutsHoldingExtend Function loadItem from SQLlite table menu_shortcuts  Occer an Error:" +
                    ex.ToString());
            }
            var args = new PublishEventArgs()
            {
                EventId =
                    EventIdAssign.MenuShourtCutsLoadUpdate ,
                EventType = PublishEventType.Core
            };

            EventPublish.PublishEvent(args);
        }


        /// <summary>
        /// 增加快捷键信息
        /// </summary>
        /// <param name="menuId">菜单部件唯一地址</param>
        /// <param name="shortcut">名称</param>
        public void AddShortCut(int menuId, string shortcut)
        {
            if (string.IsNullOrEmpty(shortcut)) return;
            if (!DicClassic.ContainsKey(menuId))
            {
                DicClassic.Add(menuId, shortcut);
            }
            else
            {
                DicClassic[menuId] = shortcut;
            }
        }

        /// <summary>
        /// 更新快捷键信息,立即回写数据库并发布事件
        /// </summary>
        /// <param name="menuId">菜单部件唯一标示</param>
        /// <param name="shortcut"></param>
        public void UpdateShortCut(int menuId, string shortcut)
        {
            if (string.IsNullOrEmpty(shortcut))
            {
                if (DicClassic.ContainsKey(menuId))
                {
                    DicClassic.Remove(menuId);
                }
            }
            else
            {
                if (!DicClassic.ContainsKey(menuId))
                {
                    DicClassic.Add(menuId, shortcut);
                }
                else
                {
                    DicClassic[menuId] = shortcut;
                }
            }
            var args = new PublishEventArgs()
                           {
                               EventType = PublishEventType.Core,
                               EventId = EventIdAssign.MenuShourtCutsUpdate,
                           };
            args.AddParams(menuId);
            EventPublish.PublishEvent(args);
            this.WriteUpdateDbByPrivate(menuId);
        }

        /// <summary>
        /// 删除快捷键信息，立即回写数据库并发布事件
        /// </summary>
        /// <param name="menuId"></param>
        public void DeleteShortCut(int menuId)
        {
            if (DicClassic.ContainsKey(menuId))
            {
                DicClassic.Remove(menuId);
                var args = new PublishEventArgs()
                {
                    EventType = PublishEventType.Core,
                    EventId = EventIdAssign.MenuShourtCutsUpdate,
                };
                args.AddParams(menuId);
                EventPublish.PublishEvent(args);
                this.WriteDeleteDbByPrivate(menuId);
            }
        }

        /// <summary>
        /// 所有数据会写数据库
        /// </summary>
        public void WriteUpdateDb()
        {
            WriteUpdateDbByPrivate();
        }



        private void WriteUpdateDbByPrivate()
        {
            SqlLiteHelper.ExecuteNonQuery("delete from menu_shortcuts ");
            foreach (var t in DicClassic)
            {
                if (string.IsNullOrEmpty(t.Value))
                    continue;
                string strUpdateDirectroy = "insert into menu_shortcuts(id,shortcut) values";
                strUpdateDirectroy += "(" + t.Key + ",'" + t.Value + "')";
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

        private void WriteDeleteDbByPrivate(int id)
        {
            SqlLiteHelper.ExecuteNonQuery("delete from menu_shortcuts where id=" + id);
        }

        private void WriteUpdateDbByPrivate(int id)
        {
            SqlLiteHelper.ExecuteNonQuery("delete from menu_shortcuts where id=" + id);
            if (DicClassic.ContainsKey(id))
            {
                var t = DicClassic[id];

                if (!string.IsNullOrEmpty(t))
                {
                    string strUpdateDirectroy = "insert into menu_shortcuts(id,shortcut) values";
                    strUpdateDirectroy += "(" + id + ",'" + t + "')";
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

    }
}
