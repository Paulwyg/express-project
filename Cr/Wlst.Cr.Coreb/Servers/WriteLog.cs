using Wlst.Cr.Coreb.Log4NetInfo;

namespace Wlst.Cr.Coreb.Servers
{

    /// <summary>
    /// 日志写入
    /// </summary>
    public class WriteLog
    {
        //public static bool ShowConsolLog = false;
        //public static bool IsWriteDebugSysteRunningInfo = true;
        //public static bool IsWriteDebugSerdataInfo = true;
        //public static bool IsWriteDebugClidataInfo = true;

        private static object obj = 1;
        private static Log4Net _log4NetInstancesr = null;

        private static Log4Net Log4NetInstances
        {
            get
            {
                if (_log4NetInstancesr == null)
                {
                    lock (obj)
                    {
                        _log4NetInstancesr = new Log4Net();
                    }

                }
                return _log4NetInstancesr;
            }
        }


        /// <summary>
        /// 记录程序运行的一般信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteInfo(string msg)
        {
            Log4NetInstances.WriteLogInfo(msg);
        }

        /// <summary>
        /// 记录程序的出错信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLogError(string msg)
        {

            Log4NetInstances.WriteLogError(msg);
        }

        /// <summary>
        /// 记录程序的调试信息 如Exception信息等
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteDebug(string msg)
        {

            Log4NetInstances.WriteLogDebug(msg);

        }



    };
}
