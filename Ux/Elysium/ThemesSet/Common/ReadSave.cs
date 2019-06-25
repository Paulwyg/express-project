using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Elysium.ThemesSet.Common
{
    public class ReadSave
    {
        public static void Save(Dictionary<string, string> info, string xmlFileName)
        {
            try
            {
                if (!xmlFileName.EndsWith(".xml"))
                {
                    xmlFileName += ".xml";
                }

                string dir = Directory.GetCurrentDirectory() + "\\SystemColorAndFont";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string path = dir + "\\" + xmlFileName;
                if (File.Exists(path)) File.Delete(path);

                try
                {
                    XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                    writer.Formatting = Formatting.Indented; //使用自动缩进便于阅读
                    writer.WriteStartDocument(); //XML声明
                    writer.WriteStartElement("Root"); //书写根元素
                    foreach (var t in info)
                    {
                        if (string.IsNullOrEmpty(t.Key) || string.IsNullOrEmpty(t.Value)) continue;

                        writer.WriteStartElement("elysium"); //开始一个元素
                        writer.WriteAttributeString("key", t.Key); //向先前创建的元素中添加一个属性
                        writer.WriteAttributeString("value", t.Value); //向先前创建的元素中添加一个属性
                        writer.WriteEndElement(); // 关闭元素
                    }
                    //在节点间添加一些空

                    writer.Close();

                }
                catch
                {
                    return;
                }
            }
            catch (Exception ex)
            {
            }

        }

        //lvf 2018年4月12日10:41:21 添加路径参数
        public static void Save(Dictionary<string, string> info, string xmlFileName,string xmlFilePath)
        {
            try
            {
                if (!xmlFileName.EndsWith(".xml"))
                {
                    xmlFileName += ".xml";
                }

                string dir = Directory.GetCurrentDirectory() + "\\" + xmlFilePath;//SystemColorAndFont";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string path = dir + "\\" + xmlFileName;
                if (File.Exists(path)) File.Delete(path);

                try
                {
                    XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                    writer.Formatting = Formatting.Indented; //使用自动缩进便于阅读
                    writer.WriteStartDocument(); //XML声明
                    writer.WriteStartElement("Root"); //书写根元素
                    foreach (var t in info)
                    {
                        if (string.IsNullOrEmpty(t.Key) || string.IsNullOrEmpty(t.Value)) continue;

                        writer.WriteStartElement("elysium"); //开始一个元素
                        writer.WriteAttributeString("key", t.Key); //向先前创建的元素中添加一个属性
                        writer.WriteAttributeString("value", t.Value); //向先前创建的元素中添加一个属性
                        writer.WriteEndElement(); // 关闭元素
                    }
                    //在节点间添加一些空

                    writer.Close();

                }
                catch
                {
                    return;
                }
            }
            catch (Exception ex)
            {
            }

        }


        public static Dictionary<string, string> Read(string xmlFileName)
        {
            var info = new Dictionary<string, string>();

            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName += ".xml";
            }
            string dir = Directory.GetCurrentDirectory() + "\\SystemColorAndFont";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + xmlFileName;
            if (!File.Exists(path)) return info;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

                XmlNode root = xmlDoc.SelectSingleNode("Root");
                if (root != null)
                {
                    var nodelist = root.ChildNodes;

                    foreach (XmlNode nodeType in nodelist)
                    {
                        XmlElement element = (XmlElement) nodeType;
                        if (element != null)
                        {
                            try
                            {
                                string key = element.GetAttribute("key");
                                string value = element.GetAttribute("value");
                                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !info.ContainsKey(key))
                                {
                                    info.Add(key, value);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteLog.WriteLogError("Core Config ReadConfig Error: GetConfigFilePaht path: " + configfilepath +
                //                       ", nodeName :" + nodename + "; Ex:" + ex);
            }
            return info;
        }


        //lvf 2018年4月12日10:41:21 添加路径参数
        public static Dictionary<string, string> Read(string xmlFileName,string xmlFilePath)
        {
            var info = new Dictionary<string, string>();

            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName += ".xml";
            }
            string dir = Directory.GetCurrentDirectory() + "\\"+xmlFilePath;//"\\SystemColorAndFont";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + xmlFileName;
            if (!File.Exists(path)) return info;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);

                XmlNode root = xmlDoc.SelectSingleNode("Root");
                if (root != null)
                {
                    var nodelist = root.ChildNodes;

                    foreach (XmlNode nodeType in nodelist)
                    {
                        XmlElement element = (XmlElement)nodeType;
                        if (element != null)
                        {
                            try
                            {
                                string key = element.GetAttribute("key");
                                string value = element.GetAttribute("value");
                                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !info.ContainsKey(key))
                                {
                                    info.Add(key, value);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteLog.WriteLogError("Core Config ReadConfig Error: GetConfigFilePaht path: " + configfilepath +
                //                       ", nodeName :" + nodename + "; Ex:" + ex);
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return info;
        }
    }
}
