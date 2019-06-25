using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Wlst.Cr.Coreb.Servers
{

    public class XmlReadSave
    {
        public static void Save(Dictionary<string, string> info, string path)
        {
            try
            {
                if (!path.EndsWith(".xml"))
                {
                    path += ".xml";
                }

                //string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
                //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                //string path = dir + "\\" + xmlFileName;
                if (File.Exists(path)) File.Delete(path);


                XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                writer.Formatting = Formatting.Indented; //使用自动缩进便于阅读
                writer.WriteStartDocument(); //XML声明
                writer.WriteStartElement("Root"); //书写根元素
                foreach (var t in info)
                {
                    if (string.IsNullOrEmpty(t.Key) || string.IsNullOrEmpty(t.Value)) continue;

                    writer.WriteStartElement("XmlConfig"); //开始一个元素
                    writer.WriteAttributeString("key", t.Key); //向先前创建的元素中添加一个属性
                    writer.WriteAttributeString("value", t.Value); //向先前创建的元素中添加一个属性
                    writer.WriteEndElement(); // 关闭元素
                }
                //在节点间添加一些空

                writer.Close();


            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Save Xml Error: Path :" + path + ",Error:" + ex);
            }

        }


        public static Dictionary<string, string> Read(string path)
        {
            var info = new Dictionary<string, string>();

            if (!path.EndsWith(".xml"))
            {
                path += ".xml";
            }
            //string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
            //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            //string path = dir + "\\" + xmlFileName;
            //if (File.Exists(path))
            //    path = path;

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
                WriteLog.WriteLogError("Read Xml Error: Path :" + path + ",Error:" + ex);
            }
            return info;
        }
    }
}
