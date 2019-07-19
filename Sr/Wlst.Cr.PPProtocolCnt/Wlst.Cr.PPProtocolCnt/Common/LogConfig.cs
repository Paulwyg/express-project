using System;
using System.Collections.Generic;
using System.Xml;

namespace Wlst.Cr.PPProtocolSvrCnt.Common
{

    public class ConfigFilePath
    {

        private const string Log4NetConfigFilePathPrivate = "Log4NetConfigFilePath";

        public static string Log4NetConfigFilePath 
        {
            get
            {
                string strPath = ReadConfig.GetConfigFilePaht(ConfigFile, Log4NetConfigFilePathPrivate);
                if (string.IsNullOrEmpty(strPath)) strPath = @"Config\ProtocolLog4net.xml";

                if (!System.IO.File.Exists(strPath))
                {
                    try
                    {
                        Log4NetXml log4NetXml = new Log4NetXml();

                        if (!System.IO.Directory.Exists("Config"))
                        {
                            System.IO.Directory.CreateDirectory("Config");
                        }
                        using (System.IO.StreamWriter sw = System.IO.File.CreateText(strPath))
                        {
                            foreach (var t in log4NetXml.LstXml)
                                sw.WriteLine(t);
                        }

                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                        str = "";
                    }
                }
                return strPath;
            }
        }

        public const string ConfigFile = @"Config\ProtocolLog4net.xml";

    }

    public class ReadConfig
    {
        public static string GetConfigFilePaht(string configfilepath, string nodename)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configfilepath);

                XmlNodeList nodeListMenu = xmlDoc.SelectSingleNode("configuration").ChildNodes;
                foreach (XmlNode nodeType in nodeListMenu)
                {
                    XmlElement element = (XmlElement)nodeType;
                    if (element.Name.Equals(nodename))
                    {
                        return element.GetAttribute("value");
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteLog.WriteLogError("Core Config ReadConfig Error: GetConfigFilePaht path: " + configfilepath +
                //                       ", nodeName :" + nodename + "; Ex:" + ex);
            }
            return "";
        }
    }

    public class Log4NetXml
    {
        public List<string> LstXml;

        public Log4NetXml()
        {
            LstXml = new List<string>();
            Load();
        }

        private void Load()
        {
            LstXml.Add("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            LstXml.Add("<!--请勿擅自更改此文件内容。-->");
            LstXml.Add("<configuration>");
            LstXml.Add("<log4net>");
            LstXml.Add("<root>");
            LstXml.Add("<level value=\"ALL\" />");
            LstXml.Add("<appender-ref ref=\"LogServerInfo\" />");
            LstXml.Add("<appender-ref ref=\"LogServerDebug\" />");
            LstXml.Add("<appender-ref ref=\"LogServerError\" />");

            LstXml.Add("</root>");
            LstXml.Add("<logger name=\"LogServer\" additivity=\"true\">");
            LstXml.Add("<level value=\"ALL\" />");
            LstXml.Add("<appender-ref ref=\"LogServerInfo\" />");
            LstXml.Add("<appender-ref ref=\"LogServerDebug\" />");
            LstXml.Add("<appender-ref ref=\"LogServerError\" />");
            LstXml.Add("</logger>");

            LstXml.Add(
                "<appender name=\"LogServerInfo\" type=\"log4net.Appender.RollingFileAppender\" Encoding=\"utf-8\" LockingModel=\"MinimalLock\">");

            LstXml.Add("<file value=\"..\\protocollog\\\" />");
            LstXml.Add("<appendToFile value=\"true\" />");
            LstXml.Add("<rollingStyle value=\"Composite\" />");
            LstXml.Add("<maximumFileSize value=\"30MB\" />");
            LstXml.Add("<maxSizeRollBackups value=\"-1\" />");

            LstXml.Add("<datePattern value=\"yyyyMMdd\".Info.log\"\" />");
            LstXml.Add("<staticLogFileName value=\"false\" />");
            LstXml.Add("<layout type=\"log4net.Layout.PatternLayout\">");
            LstXml.Add("<conversionPattern value=\"%d{yyyy-MM-dd HH:mm:ss} [%-5p] - %m%n\" />");
            LstXml.Add("</layout>");
            LstXml.Add("<filter type=\"log4net.Filter.LevelRangeFilter\">");
            LstXml.Add("<levelMin value=\"INFO\" />");
            LstXml.Add("<levelMax value=\"INFO\" />");
            LstXml.Add("</filter>");
            LstXml.Add("</appender>");
            LstXml.Add(
                "<appender name=\"LogServerDebug\" type=\"log4net.Appender.RollingFileAppender\" Encoding=\"utf-8\" LockingModel=\"MinimalLock\">");

            LstXml.Add("<file value=\"..\\protocollog\\\" />");
            LstXml.Add("<appendToFile value=\"true\" />");
            LstXml.Add("<rollingStyle value=\"Composite\" />");
            LstXml.Add("<maximumFileSize value=\"50MB\" />");
            LstXml.Add("<maxSizeRollBackups value=\"-1\" />");

            LstXml.Add("<datePattern value=\"yyyyMMdd\".Debug.log\"\" />");
            LstXml.Add("<staticLogFileName value=\"false\" />");
            LstXml.Add("<layout type=\"log4net.Layout.PatternLayout\">");
            LstXml.Add("<conversionPattern value=\"%d{yyyy-MM-dd HH:mm:ss} [%-5p] - %m%n\" />");
            LstXml.Add("</layout>");
            LstXml.Add("<filter type=\"log4net.Filter.LevelRangeFilter\">");
            LstXml.Add("<levelMin value=\"DEBUG\" />");
            LstXml.Add("<levelMax value=\"DEBUG\" />");
            LstXml.Add("</filter>");
            LstXml.Add("</appender>");
            LstXml.Add(
                "<appender name=\"LogServerError\" type=\"log4net.Appender.RollingFileAppender\" Encoding=\"utf-8\" LockingModel=\"MinimalLock\">");

            LstXml.Add("<file value=\"..\\protocollog\\\" />");
            LstXml.Add("<appendToFile value=\"true\" />");
            LstXml.Add("<rollingStyle value=\"Composite\" />");
            LstXml.Add("<maximumFileSize value=\"20MB\" />");
            LstXml.Add("<maxSizeRollBackups value=\"-1\" />");

            LstXml.Add("<datePattern value=\"yyyyMMdd\".Error.log\"\" />");
            LstXml.Add("<staticLogFileName value=\"false\" />");
            LstXml.Add("<layout type=\"log4net.Layout.PatternLayout\">");
            LstXml.Add("<conversionPattern value=\"%d{yyyy-MM-dd HH:mm:ss} [%-5p] - %m%n\" />");
            LstXml.Add("</layout>");
            LstXml.Add("<filter type=\"log4net.Filter.LevelRangeFilter\">");
            LstXml.Add("<levelMin value=\"ERROR\" />");
            LstXml.Add("<levelMax value=\"ERROR\" />");
            LstXml.Add("</filter>");
            LstXml.Add("</appender>");
            LstXml.Add("</log4net>");
            LstXml.Add("</configuration>");
        }

    }
}
