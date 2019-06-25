using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConsoleApp1
{
    public class Program
    {
        private const string XmlConfigName = "MapGis";
        static void Main(string[] args)
        {
            var info = Readxml(XmlConfigName);

            foreach (var f in info)
            {
              //  Console.WriteLine(f.Key + "   -  " + f.Value);
            }
            Console.ReadLine();
        }



        public static Dictionary<string, string> Readxml(string xmlFileName, string filePath = null)
        {
            var info = new Dictionary<string, string>();

            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName += ".xml";
            }
            //lvf 2018年3月21日16:45:24  添加可填写路径的配置文件读取
            var fp = "\\SystemXmlConfig";
            if (!string.IsNullOrEmpty(filePath)) fp = filePath.Trim();
            string dir = Directory.GetCurrentDirectory() + fp;
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + xmlFileName;
            if (File.Exists(xmlFileName))
                path = xmlFileName;
            if (!File.Exists(path)) return info;
            Console.WriteLine("test gis path:" + path);
            return Read(path);

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
                       
                        XmlElement element = (XmlElement)nodeType;
                        if (element != null)
                        {
                            foreach (XmlAttribute  f in element .Attributes )
                            {
 Console.WriteLine("node   - " + f.Value  );
                            }
                            try
                            {
                                string key = element.GetAttribute("key");
                                string value = element.GetAttribute("value");
                                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !info.ContainsKey(key))
                                {
                                    info.Add(key, value);
                                }


                               // Console.Write("Read Xml key:  :" + key + ",value:" + value);
                            }
                            catch (Exception ex)
                            {
                                Console.Write("Read Xml Error: element :" + element  + ",Error:" + ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteLog.WriteLogError("Core Config ReadConfig Error: GetConfigFilePaht path: " + configfilepath +
                //                       ", nodeName :" + nodename + "; Ex:" + ex);
                Console.Write("Read Xml Error: Path :" + path + ",Error:" + ex);
            }
            return info;
        }

    }
}