using System.Data;

namespace Wlst.Cr.Core.SqlLite
{
    public interface IISQLite
    {

        /// <summary>
        /// 在后台线程执行耗时的数据写入任务
        /// </summary>
        /// <param name="cmdText"></param>
         void ExecuteNonQueryInThread(string cmdText);

        /// <summary>
        /// 在后台线程执行耗时的数据写入任务
        /// </summary>
        /// <param name="cmdText"></param>
         void ExecuteTransactionInThread(string[] cmdText);

        /// <summary>
        /// 执行命令，执行更新和删除时返回影响的行的数目
        /// </summary>
        int ExecuteNonQuery(string cmdText, params object[] p);


        /// <summary>
        /// 执行命令，执行更新和删除时返回影响的行的数目
        /// </summary>
        int ExecuteNonQuery(string cmdText);


        bool ExecuteTransaction(string[] cmdText);


        /// <summary>
        /// 执行查询，返回DataSet
        /// </summary>
        DataSet ExecuteQuery(string cmdText, params object[] p);




        /// <summary>
        /// 执行查询，返回查询结果集中的第一行第一列（主要用于返回统计值）
        /// </summary>
        object ExecuteScalar(string cmdText, params object[] p);


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
        DataTable ExecutePage(string tableName, string strColumns, string strWhere, string strOrder,
                              int pageSize, int currentIndex, out int recordOut);

    }
}
