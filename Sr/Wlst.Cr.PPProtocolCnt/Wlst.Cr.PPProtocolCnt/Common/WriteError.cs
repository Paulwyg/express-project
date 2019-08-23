using System;
using Wlst.Cr.PPProtocolSvrCnt.Log4NetInfo;

namespace Wlst.Cr.PPProtocolSvrCnt.Common
{
    /// <summary>
    /// 记录出错信息 需要外部提供记录出错信息的写入函数
    /// </summary>
    public class WriteError
    {


        private static readonly IILog4Net Log4NetInstances =
            new Log4Net(ConfigFilePath.Log4NetConfigFilePath);

        //public static Action<string> WriteLogError = null;

        /// <summary>
        /// 记录程序运行的一般信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLogInfo(string msg)
        {
            Log4NetInstances.WriteLogInfo(msg);

        }

        /// <summary>
        /// 记录程序的出错信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLogError(string msg)
        {
            //if (WriteLogError != null)
            //    WriteLogError(msg);

          //  if (msg.Contains("NoErr") == false) return;
         
            Log4NetInstances.WriteLogError(msg);
        }

        /// <summary>
        /// 记录程序的调试信息 如Exception信息等
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLogDebug(string msg)
        {
            Log4NetInstances.WriteLogDebug(msg);
        }


    }
}
