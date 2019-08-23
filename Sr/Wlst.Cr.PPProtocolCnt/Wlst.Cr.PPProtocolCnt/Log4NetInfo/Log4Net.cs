using System;
using System.IO;

namespace Wlst.Cr.PPProtocolSvrCnt.Log4NetInfo
{
    /// <summary>
    /// 实现Log4Net的一些基本日志函数
    /// </summary>
    public class Log4Net : IILog4Net
    {

        private log4net.ILog _log = null;

        public Log4Net(string configFilePath)
        {
            try
            {
                if (!File.Exists(configFilePath)) return;
                var configFile = new FileInfo(configFilePath);
                log4net.Config.XmlConfigurator.Configure(configFile);
                _log = log4net.LogManager.GetLogger("LogServer");
            }
            catch (Exception ex)
            {
                //_log = null;
            }
        }

        /// <summary>
        /// 记录程序运行的一般信息
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLogInfo(string msg)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info(msg);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        /// <summary>
        /// 记录程序的出错信息
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLogError(string msg)
        {
            try
            {
                if (_log.IsErrorEnabled) _log.Error(msg);
            }
            catch (Exception ex)
            {

                ex.ToString();
            }

        }

        /// <summary>
        /// 记录程序的调试信息 如Exception信息等
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLogDebug(string msg)
        {
            try
            {
                if (_log.IsDebugEnabled) _log.Debug(msg);
            }
            catch (Exception ex)
            {

                ex.ToString();
            }
        }


    };
}
