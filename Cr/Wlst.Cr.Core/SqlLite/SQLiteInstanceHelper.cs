using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading;

namespace Wlst.Cr.Core.SqlLite
{
    /// <summary>
    ///SQLite数据库操作类
    ///确保添加System.Data.SQLite引用 并且版本支持4.0
    /// </summary>
    public class SQLiteInstanceHelper
    {
        //private const string StrConnectString = @"..\..\..\mydatabase.sqlite";
        private readonly SQLiteConnection _conn;
        private static bool _bolWriting = false;

        private Thread th;
        private Queue<string> queue;
        private Queue<List<string>> queueTrans;

        public SQLiteInstanceHelper(string connectString)
        {
            _conn = new SQLiteConnection("Data Source=" + connectString);
            InitSqlite();
        }

        private void InitSqlite()
        {
            queue = new Queue<string>();
            queueTrans = new Queue<List<string>>();
            th = new Thread(runWriteDb);
            th.Start();
        }

        private void runWriteDb()
        {
            while (true)
            {
                try
                {
                    while (queue.Count > 0)
                    {
                        string sql = queue.Dequeue();
                        ExecuteNonQuery(sql);
                        Thread.Sleep(5);
                    }

                    while (queueTrans.Count > 0)
                    {
                        var t = queueTrans.Dequeue();
                        string[] cmdtext = new string[t.Count];
                        for (int i = 0; i < t.Count; i++)
                        {
                            cmdtext[i] = t[i];
                        }
                        ExecuteTransaction(cmdtext);
                        Thread.Sleep(5);
                    }
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 在后台线程执行耗时的数据写入任务
        /// </summary>
        /// <param name="cmdText"></param>
        public void ExecuteNonQueryInThread(string cmdText)
        {
            queue.Enqueue(cmdText);
        }

        /// <summary>
        /// 在后台线程执行耗时的数据写入任务
        /// </summary>
        /// <param name="cmdText"></param>
        public void ExecuteTransactionInThread(string[] cmdText)
        {
            List<string> lst = new List<string>();
            for (int i = 0; i < cmdText.Length; i++)
            {
                lst.Add(cmdText[i]);
            }
            queueTrans.Enqueue(lst);
        }

        /// <summary>
        /// 准备Command
        /// </summary>
        /// <param name="cmd">command</param>
        /// <param name="cmdText">命令参数</param>
        /// <param name="p">参数数组</param>
        private void PrepareCommand(SQLiteCommand cmd, string cmdText, params object[] p)
        {
            if (_conn == null) return;
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }

            cmd.Parameters.Clear();
            cmd.Connection = _conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
            if (p == null) return;
            foreach (object parm in p)
                cmd.Parameters.AddWithValue(string.Empty, parm);
        }

        /// <summary>
        /// 准备Command
        /// </summary>
        /// <param name="cmd">command</param>
        /// <param name="cmdText">命令参数</param>
        /// <param name="p">参数数组</param>
        private void PrepareCommand(SQLiteCommand cmd, string cmdText)
        {
            if (_conn == null) return;
            if (_conn.State != ConnectionState.Open)
            {
                _conn.Open();
            }
            cmd.Parameters.Clear();
            cmd.Connection = _conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
        }


        /// <summary>
        /// 执行命令，执行更新和删除时返回影响的行的数目
        /// </summary>
        public int ExecuteNonQuery(string cmdText, params object[] p)
        {
            using (var command = new SQLiteCommand())
            {

                PrepareCommand(command, cmdText, p);
                int intReturn = 0;
                while (true)
                {
                    if (_bolWriting)
                    {
                        Thread.Sleep(5);
                    }
                    else
                    {
                        _bolWriting = true;
                        try 
                        {
                        intReturn = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            _bolWriting = false;
                            throw;
                        }
                        _bolWriting = false;
                        break;
                    }
                }
                return intReturn;
            }
        }

        /// <summary>
        /// 执行命令，执行更新和删除时返回影响的行的数目
        /// </summary>
        public int ExecuteNonQuery(string cmdText)
        {
            using (var command = new SQLiteCommand())
            {
                PrepareCommand(command, cmdText);
                //return command.ExecuteNonQuery();
                int intReturn = 0;
                while (true)
                {
                    if (_bolWriting)
                    {
                        Thread.Sleep(5);
                    }
                    else
                    {
                        _bolWriting = true;
                        try
                        {
                            intReturn = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            _bolWriting = false;
                            throw;
                        }
                        _bolWriting = false;
                        break;
                    }
                }
                return intReturn;
            }
        }

        public bool ExecuteTransaction(string[] cmdText)
        {

            bool bolRetrun = false;
            while (true)
            {
                if (_bolWriting)
                {
                    Thread.Sleep(5);
                }
                else
                {
                    _bolWriting = true;
                    try
                    {
                        if (_conn == null) return false;

                        var myCommand = new SQLiteCommand();
                        // Open the connection.
                        try
                        {
                            if (_conn.State != ConnectionState.Open)
                                _conn.Open();
                        }
                        catch (Exception ex)
                        {
                            //throw new Exception("Class SQLiteHelp Funciton ExecuteTransaction Open Connection fail :" +
                            //                    ex.ToString());
                            _bolWriting = false;
                            throw new Exception("Class SQLiteHelp Funciton ExecuteTransaction Open Connection fail :" +
                                                ex.ToString());
                            return false;
                        }
                        // Assign the connection property.
                        myCommand.Connection = _conn;
                        // Begin the transaction.
                        SQLiteTransaction myTrans = _conn.BeginTransaction();

                        // Assign transaction object for a pending local transaction
                        myCommand.Transaction = myTrans;

                        try
                        {
                            foreach (var s in cmdText)
                            {
                                myCommand.CommandText = s;
                                myCommand.ExecuteNonQuery();
                            }

                            myTrans.Commit();
                            bolRetrun = true;
                        }
                        catch (Exception e)
                        {
                            myTrans.Rollback();
                            bolRetrun = false;
                            //throw new Exception("Class SQLiteHelp Funciton ExecuteTransaction Commit fail :" +
                            //                    e.ToString());
                        }
                        finally
                        {
                            _conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        _bolWriting = false;
                        throw;
                    }
                    _bolWriting = false;
                    break;
                }
            }
            return bolRetrun;
        }

        /// <summary>
        /// 执行查询，返回DataSet
        /// </summary>
        public DataSet ExecuteQuery(string cmdText, params object[] p)
        {
            if (_conn == null) return null;
            using (var command = new SQLiteCommand())
            {
                var ds = new DataSet();
                PrepareCommand(command, cmdText, p);
                while (true)
                {
                    if (_bolWriting)
                    {
                        Thread.Sleep(5);
                    }
                    else
                    {
                        _bolWriting = true;
                        try
                        {
                            var da = new SQLiteDataAdapter(command);
                            da.Fill(ds);

                        }
                        catch (Exception ex)
                        {
                            _bolWriting = false;
                            throw;

                        }
                        _bolWriting = false;
                        break;
                    }
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行查询，返回DataReader
        /// </summary>
        public SQLiteDataReader ExecuteReader(string cmdText, params object[] p)
        {
            if (_conn == null) return null;
            SQLiteDataReader sr = null;
            using (var command = new SQLiteCommand())
            {

                PrepareCommand(command, cmdText, p);
                while (true)
                {
                    if (_bolWriting)
                    {
                        Thread.Sleep(5);
                    }
                    else
                    {
                        _bolWriting = true;
                        try
                        {
                            sr = command.ExecuteReader(CommandBehavior.CloseConnection);
                        }
                        catch (Exception ex)
                        {
                            _bolWriting = false;
                            throw;
                        }
                        _bolWriting = false;
                        break;
                    }
                }
            }
            return sr;
        }


        /// <summary>
        /// 执行查询，返回查询结果集中的第一行第一列（主要用于返回统计值）
        /// </summary>
        public object ExecuteScalar(string cmdText, params object[] p)
        {
            object obj = null;
            if (_conn == null) return null;
            using (var command = new SQLiteCommand())
            {
                PrepareCommand(command, cmdText, p);

                //return command.ExecuteScalar();

                while (true)
                {
                    if (_bolWriting)
                    {
                        Thread.Sleep(5);
                    }
                    else
                    {
                        _bolWriting = true;
                        try
                        {
                            obj = command.ExecuteScalar();
                        }
                        catch (Exception ex)
                        {
                            _bolWriting = false;
                            throw;
                        }
                        _bolWriting = false;
                        break;
                    }
                }
                return obj;
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
        public DataTable ExecutePage(string tableName, string strColumns, string strWhere, string strOrder, int pageSize,
                                     int currentIndex, out int recordOut)
        {
            recordOut = 0;
            if (_conn == null) return null;
            var dt = new DataTable();
            recordOut = Convert.ToInt32(ExecuteScalar("select count(*) from " + tableName));
            var pagingTemplate = "select {0} from {1} where 1=1 {2} order by {3} limit {4} offset {5} ";
            var offsetCount = (currentIndex - 1)*pageSize;
            var commandText = String.Format(pagingTemplate, strColumns, tableName, strWhere, strOrder,
                                            pageSize.ToString(), offsetCount.ToString());
            //using (var reader = ExecuteReader(commandText))
            //{
            //    if (reader != null)
            //    {
            //        dt.Load(reader);
            //    }
            //}

            while (true)
            {
                if (_bolWriting)
                {
                    Thread.Sleep(5);
                }
                else
                {
                    _bolWriting = true;
                    try
                    {
                        using (var reader = ExecuteReader(commandText))
                        {
                            if (reader != null)
                            {
                                dt.Load(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _bolWriting = false;
                        throw;
                    }

                    _bolWriting = false;
                    break;
                }
            }

            return dt;
        }
    }
}
