using System.Data;
using System.IO;

namespace Wlst.Ux.MenuNew.MenuSetting.SqlLite
{

    /// <summary>
    ///SQLite数据库操作类
    /// </summary>
    public class SqLite : IISQLite
    {

        private SQLiteInstanceHelper _sqLiteInstanceHelper = null;

        public SqLite(string configFilePath)
        {
            if (!File.Exists(configFilePath)) return;
            _sqLiteInstanceHelper = new SQLiteInstanceHelper(configFilePath);
        }

        /// <summary>
        /// 在后台线程执行耗时的数据写入任务
        /// </summary>
        /// <param name="cmdText"></param>
        public void ExecuteNonQueryInThread(string cmdText)
        {
            if (_sqLiteInstanceHelper == null) return;
            _sqLiteInstanceHelper.ExecuteNonQueryInThread(cmdText);
        }

        /// <summary>
        /// 在后台线程执行耗时的数据写入任务
        /// </summary>
        /// <param name="cmdText"></param>
        public void ExecuteTransactionInThread(string[] cmdText)
        {
            if (_sqLiteInstanceHelper == null) return;
            _sqLiteInstanceHelper.ExecuteTransactionInThread(cmdText);
        }

        /// <summary>
        /// 执行命令，执行更新和删除时返回影响的行的数目
        /// </summary>
        public int ExecuteNonQuery(string cmdText, params object[] p)
        {
            if (_sqLiteInstanceHelper == null) return -1;
            return _sqLiteInstanceHelper.ExecuteNonQuery(cmdText, p);

        }

        /// <summary>
        /// 执行命令，执行更新和删除时返回影响的行的数目
        /// </summary>
        public int ExecuteNonQuery(string cmdText)
        {
            if (_sqLiteInstanceHelper == null) return -1;
            return _sqLiteInstanceHelper.ExecuteNonQuery(cmdText);

        }

        public bool ExecuteTransaction(string[] cmdText)
        {
            if (_sqLiteInstanceHelper == null) return false;
            return _sqLiteInstanceHelper.ExecuteTransaction(cmdText);

        }

        /// <summary>
        /// 执行查询，返回DataSet
        /// </summary>
        public DataSet ExecuteQuery(string cmdText, params object[] p)
        {
            if (_sqLiteInstanceHelper == null) return null;
            return _sqLiteInstanceHelper.ExecuteQuery(cmdText, p);

        }



        /// <summary>
        /// 执行查询，返回查询结果集中的第一行第一列（主要用于返回统计值）
        /// </summary>
        public object ExecuteScalar(string cmdText, params object[] p)
        {
            if (_sqLiteInstanceHelper == null) return 0;
            return _sqLiteInstanceHelper.ExecuteScalar(cmdText, p);

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
        public DataTable ExecutePage(string tableName, string strColumns, string strWhere, string strOrder,
                                     int pageSize, int currentIndex, out int recordOut)
        {
            recordOut = 0;
            if (_sqLiteInstanceHelper == null) return null;

            return _sqLiteInstanceHelper.ExecutePage(tableName, strColumns, strWhere, strOrder, pageSize,
                                                     currentIndex,
                                                     out recordOut);
        }
    }

}
