using System;
using Wlst.Cr.Core.Config;
 

namespace Wlst.Cr.Core.UtilityFunction
{

    /// <summary>
    /// 日志写入 
    /// </summary>
    [Serializable]
    public class WriteLog
    {
        //private static readonly IILog4Net Log4NetInstances =
        //    new Log4Net(ConfigFilePath.Log4NetConfigFilePath);


        /// <summary>
        /// 记录程序运行的一般信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLogInfo(string msg)
        {
            Wlst .Cr .Coreb .Servers .WriteLog .WriteInfo(  msg);
        }

        /// <summary>
        /// 记录程序的出错信息
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLogError(string msg)
        {
            Wlst .Cr .Coreb .Servers .WriteLog .WriteLogError(  msg);

        }

        /// <summary>
        /// 记录程序的调试信息 如Exception信息等
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLogDebug(string msg)
        {
            Wlst .Cr .Coreb .Servers .WriteLog .WriteDebug(msg);
        }


    };
}
