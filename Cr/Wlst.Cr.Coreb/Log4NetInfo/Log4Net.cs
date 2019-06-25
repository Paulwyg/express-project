using System;
using System.IO;

namespace Wlst.Cr.Coreb.Log4NetInfo
{
    /// <summary>
    /// 实现Log4Net的一些基本日志函数
    /// </summary>
    internal class Log4Net 
    {

        private log4net.ILog _log = null;
       // private string path;
        public Log4Net()
        {
            var configFilePath = Config.ConfigFilePath .Log4NetSystemtConfigFilePath;
            try
            {
                //path = configFilePath;
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

        ///// <summary>
        ///// 记录程序运行的一般信息
        ///// </summary>
        ///// <param name="msg"></param>
        //public void WriteLogOther(string msg)
        //{
        //    try
        //    {
        //        if (_log.IsFatalEnabled) _log.Fatal(msg);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }

        //}

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
        ///// <summary>
        ///// 记录程序的调试信息 如Exception信息等
        ///// </summary>
        ///// <param name="msg"></param>
        //public void WriteLogWarn(string msg)
        //{
        //    try
        //    {
        //        if (_log.IsWarnEnabled) _log.Warn(msg);
        //    }
        //    catch (Exception ex)
        //    {

        //        ex.ToString();
        //    }
        //}

    };
}
