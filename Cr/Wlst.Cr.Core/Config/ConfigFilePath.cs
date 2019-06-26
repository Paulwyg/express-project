using System;
using System.Collections.Generic;
using System.Xml;

namespace Wlst.Cr.Core.Config
{
    /// <summary>
    /// 配置文件放置位置 
    /// Config\log4net.xml
    /// Config\mydatabase.sqlite
    /// </summary>
    public class ConfigFilePath
    {
        public const string SqLiteLocationFilePathPrivate = "SQLiteLocationFilePath";

        public static string SQLiteLocationFilePath //= @"..\..\..\mydatabase.sqlite"; //@"\Config\mydatabase.sqlite";
        {
            get
            {
                string strPath = ReadConfig.GetConfigFilePaht(ConfigFile, SqLiteLocationFilePathPrivate);
                if (string.IsNullOrEmpty(strPath)) strPath = @"Config\mydatabase.sqlite";



                if (!System.IO.File.Exists(strPath))
                {
                    try
                    {
                        if (!System.IO.Directory.Exists("Config"))
                        {
                            System.IO.Directory.CreateDirectory("Config");
                        }
                        System.Data.SQLite.SQLiteConnection.CreateFile(strPath);
                    }

                    catch (Exception ex)
                    {
                        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                    }
                }
                return strPath;
            }
        }

        private const string Log4NetConfigFilePathPrivate = "Log4NetConfigFilePath";

        public static string Log4NetConfigFilePath //= @"..\..\..\CETC50_Demo\bin\Debug\Config\log4net.xml";
        {
            get
            {
                string strPath = ReadConfig.GetConfigFilePaht(ConfigFile, Log4NetConfigFilePathPrivate);
                if (string.IsNullOrEmpty(strPath)) strPath = @"Config\log4net.xml";
                // string strPath = ReadConfig.GetConfigFilePaht(ConfigFile, SqLiteLocationFilePathPrivate);
                if (!System.IO.File.Exists(strPath))
                {
                    try
                    {
                        //System.Data.SQLite.SQLiteConnection.CreateFile(strPath);
                        //System.IO.File.Create(strPath);
                        Log4NetXml log4NetXml = new Log4NetXml();
                        //string[] spSp = new string[2] {"//", "\\"};
                        //string[] sp = strPath.Split(spSp, StringSplitOptions.RemoveEmptyEntries);
                        //if (sp.Length > 1)
                        //{
                        //    string path = "";
                        //    for (int i = 0; i < sp.Length - 1; i++)
                        //    {
                        //        path = path + sp[i] + "\\";
                        //    }
                        //    if (path.Length > 1) path = path.Substring(0, path.Length - 1);

                        //    if (!System.IO.Directory.Exists(path))
                        //    {
                        //        System.IO.Directory.CreateDirectory(path);
                        //    }
                        //}

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

        public const string ConfigFile = @"Config\ConfigFile.xml";

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
                    XmlElement element = (XmlElement) nodeType;
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


            LstXml.Add(
                "<appender name=\"LogServerInfo\" type=\"log4net.Appender.RollingFileAppender\" Encoding=\"utf-8\" LockingModel=\"MinimalLock\">");

            LstXml.Add("<file value=\"..\\log\\\" />");
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

            LstXml.Add("<file value=\"..\\log\\\" />");
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

            LstXml.Add("<file value=\"..\\log\\\" />");
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
