using System;
using System.Data;

using Wlst.Cr.Core.Config;
using IISQLite = Wlst.Cr.Core.SqlLite.IISQLite;
using SqLite = Wlst.Cr.Core.SqlLite.SqLite;

namespace Wlst.Cr.Core.UtilityFunction
{

    /// <summary>
    ///SQLite数据库操作类
    ///确保添加System.Data.SQLite引用 并且版本支持4.0
    /// </summary>
    [Serializable]
    public class SqlLiteHelper
    {
        //private const string StrConnectString = @"..\..\..\mydatabase.sqlite";

        private static IISQLite _sqlLiteInstancesr =
            new SqLite(ConfigFilePath.SQLiteLocationFilePath);

        /// <summary>
        /// 在后台线程执行耗时的数据写入任务
        /// </summary>
        /// <param name="cmdText"></param>
        public static void ExecuteNonQueryInThread(string cmdText)
        {
            _sqlLiteInstancesr.ExecuteNonQueryInThread(cmdText);
        }

        /// <summary>
        /// 在后台线程执行耗时的数据写入任务
        /// </summary>
        /// <param name="cmdText"></param>
        public static void ExecuteTransactionInThread(string[] cmdText)
        {
            _sqlLiteInstancesr.ExecuteTransactionInThread(cmdText);
        }

        /// <summary>
        /// 执行命令，执行更新和删除时返回影响的行的数目
        /// </summary>
        public static int ExecuteNonQuery(string cmdText, params object[] p)
        {
            try
            {
                return _sqlLiteInstancesr.ExecuteNonQuery(cmdText, p);
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// 执行命令，执行更新和删除时返回影响的行的数目
        /// </summary>
        public static int ExecuteNonQuery(string cmdText)
        {
            try
            {
                return _sqlLiteInstancesr.ExecuteNonQuery(cmdText);
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(ex.ToString());
                return 0;
            }
        }

        public static bool ExecuteTransaction(string[] cmdText)
        {
            try
            {
                return _sqlLiteInstancesr.ExecuteTransaction(cmdText);
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 执行查询，返回DataSet
        /// </summary>
        public static DataSet ExecuteQuery(string cmdText, params object[] p)
        {
            try
            {
                return _sqlLiteInstancesr.ExecuteQuery(cmdText, p);
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(ex.ToString());
                return null;
            }
        }



        /// <summary>
        /// 执行查询，返回查询结果集中的第一行第一列（主要用于返回统计值）
        /// </summary>
        public static object ExecuteScalar(string cmdText, params object[] p)
        {

            try
            {
                return _sqlLiteInstancesr.ExecuteScalar(cmdText, p);
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="tableName">表明</param>
        /// <param name="strColumns">列名</param>
        /// <param name="strWhere">条件</param>
        /// <param name="strOrder">排序</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="currentIndex">当前页</param>
        /// <param name="recordOut">总数</param>
        /// <returns>表</returns>
        public static DataTable ExecutePage(string tableName, string strColumns, string strWhere, string strOrder,
                                            int pageSize, int currentIndex, out int recordOut)
        {

            try
            {
                return _sqlLiteInstancesr.ExecutePage(tableName, strColumns, strWhere, strOrder, pageSize, currentIndex,
                                                      out recordOut);
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(ex.ToString());
                recordOut = 0;
                return null;
            }
        }
    }
}

