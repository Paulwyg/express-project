using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wlst.Cr.CoreMims.Services
{
    public partial class SystemOptionSvr
    {

        /// <summary>
        /// 将数据写入硬盘
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="xmldata"></param>
        /// <param name="isDesc"></param>
        public static void SaveModuleXml(int moduleid, Dictionary<int, string> xmldata, bool isDesc)
        {
            SaveModuleXmlPri(moduleid, xmldata, isDesc);
        }


        /// <summary>
        /// mid: 文件id，模块id +1 
        /// key：
        /// Int ：  1-99  ，
        /// string  100-199 ，
        /// boolean 200-299 （1、false，2、true ，其他 false），
        /// double  300-399 
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="key"></param>
        /// <param name="defaultRtn"></param>
        /// <returns></returns>
        public static int GetInt(int mid, int key, int defaultInt)
        {
            var rtn = GetValue(mid, key);
            if (string.IsNullOrEmpty(rtn)) return defaultInt;

            int ix = 0;
            if (Int32.TryParse(rtn, out ix))
            {
                return ix;
            }
            return defaultInt;
        }

        /// <summary>
        /// mid: 文件id，模块id +1 
        /// key：
        /// Int ：  1-99  ，
        /// string  100-199 ，
        /// boolean 200-299 （1、false，2、true ，其他 false），
        /// double  300-399 
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="key"></param>
        /// <param name="defaultStr"></param>
        /// <returns></returns>
        public static string GetString(int mid, int key, string defaultStr)
        {
            var rtn = GetValue(mid, key);
            if (string.IsNullOrEmpty(rtn)) return defaultStr;
            return rtn;

        }

        /// <summary>
        /// 获取模块下的所有key
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public static List <int > GetMoudleKeys(int mid)
        {
            if (idc.ContainsKey(mid) == false) return new List<int>();
            return idc[mid].Keys.ToList();
        }


        /// <summary>
        /// 1、false，2、true ，其他 false
        /// mid: 文件id，模块id +1 
        /// key：
        /// Int ：  1-9999  ，
        /// string  10000-19999 ，
        /// boolean 20000-29999 （1、false，2、true ，其他 false），
        /// double  30000-39999 
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="key"></param>
        /// <param name="defaultRtn"></param>
        /// <returns></returns>
        public static bool GetBoolean(int mid, int key, bool defaultBool)
        {
            var rtn = GetValue(mid, key);
            if (string.IsNullOrEmpty(rtn)) return defaultBool;

            int ix = 0;
            if (Int32.TryParse(rtn, out ix))
            {
                return ix == 2;
            }
            return defaultBool;
        }

        /// <summary>
        /// mid: 文件id，模块id +1 
        /// key：
        /// Int ：  1-9999  ，
        /// string  10000-19999 ，
        /// boolean 20000-29999 （1、false，2、true ，其他 false），
        /// double  30000-39999 
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="key"></param>
        /// <param name="defaultRtn"></param>
        /// <returns></returns>
        public static double GetDouble(int mid, int key, double defaultDouble)
        {
            var rtn = GetValue(mid, key);
            if (string.IsNullOrEmpty(rtn)) return defaultDouble;

            double ix = 0;
            if (double.TryParse(rtn, out ix))
            {
                return ix ;
            }
            return defaultDouble;
        }
    }
    public partial class SystemOptionSvr
    {
        /// <summary>
        /// mouduleid  -   key  - value
        /// </summary>
        internal static ConcurrentDictionary<int, ConcurrentDictionary<int, string>> idc = new ConcurrentDictionary<int, ConcurrentDictionary<int, string>>();


        private static bool IsLoad = false;
        /// <summary>
        /// 读取硬盘文件的数据到内存  仅读取一次
        /// </summary>
        private static void InitLoad()
        {
            if (IsLoad) return;
            IsLoad = true;
            var modules = XmlSystemOptionSvr.GetFiles();
            foreach (var l in modules)
            {
                if (idc.ContainsKey(l) == false) idc.TryAdd(l, new ConcurrentDictionary<int, string>());
                var datas = XmlSystemOptionSvr.ReadFromXmlfile(l);
                foreach (var g in datas)
                {
                    int idx = 0;
                    if (Int32.TryParse(g.Key, out idx))
                    {
                        if (idx > 0 && idc[l].ContainsKey(idx) == false)
                        {
                            idc[l].TryAdd(idx, g.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 如果不存在 返回  string.Empty
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetValue(int moduleid, int key)
        {
            InitLoad();
            if (idc.ContainsKey(moduleid) && idc[moduleid].ContainsKey(key)) return idc[moduleid][key];
            return string.Empty;
        }

        /// <summary>
        /// 将数据写入硬盘
        /// </summary>
        /// <param name="moduleid"></param>
        /// <param name="xmldata"></param>
        /// <param name="isDesc"></param>
        private static void SaveModuleXmlPri(int moduleid, Dictionary<int, string> xmldata, bool isDesc)
        {
            if (isDesc == false)
            {
                if (idc.ContainsKey(moduleid)) idc[moduleid].Clear();
                else idc.TryAdd(moduleid, new ConcurrentDictionary<int, string>());
            }
            var savedic = new Dictionary<string, string>();
            foreach (var l in xmldata)
            {
                if (isDesc == false) if (idc[moduleid].ContainsKey(l.Key) == false) idc[moduleid].TryAdd(l.Key, l.Value);
                if (savedic.ContainsKey(l.Key + " ") == false) savedic.Add(l.Key + "", l.Value);
            }

            XmlSystemOptionSvr.SaveToXmlFile(savedic, moduleid, isDesc);
        }

    }

    internal class XmlSystemOptionSvr
    {
        /// <summary>
        /// 读取 100 xml文件的路径
        /// </summary>
        public static string DirectoryPach = Directory.GetCurrentDirectory() + "\\SystemOption";

        /// <summary>
        /// 获取运行目录下  SystemOption文件夹下 类似  数字.XML的
        /// </summary>
        /// <returns></returns>
        public static List<int> GetFiles()
        {
            var rtn = new List<int>();
            try
            {

                var path = Directory.GetCurrentDirectory() + "\\SystemOption";
                if (!Directory.Exists(DirectoryPach)) return rtn;


                DirectoryInfo TheFolder = new DirectoryInfo(path);
                //遍历文件
                foreach (FileInfo NextFile in TheFolder.GetFiles())
                {
                    var filename = NextFile.Name;

                    filename = filename.Replace(".xml", "");

                    int moduleid = 0;
                    if (Int32.TryParse(filename, out moduleid))
                    {
                        if (moduleid > 0)
                        {
                            rtn.Add(moduleid);
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Coreb.Servers.WriteLog.WriteLogError("XmlSystemOptionSvr 读取SystemOption目录下的文件时出错.");
            }
            return rtn;

        }


        /// <summary>
        /// 将数据写入 硬盘
        /// </summary>
        /// <param name="info"></param>
        /// <param name="moduleId"></param>
        /// <param name="isDesc"></param>
        public static void SaveToXmlFile(Dictionary<string, string> info, int moduleId, bool isDesc)
        {
            try
            {
                var xmlFileName = moduleId + ".xml";
                if (isDesc)
                {
                    xmlFileName = moduleId + "_desc.xml";
                }
                if (!Directory.Exists(DirectoryPach)) Directory.CreateDirectory(DirectoryPach);
                string path = DirectoryPach + "\\" + xmlFileName;
                if (File.Exists(path)) File.Delete(path);

                XmlReadSave.Save(info, path);
            }
            catch (Exception ex)
            {
            }

        }

        /// <summary>
        /// 读取硬盘的数据
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ReadFromXmlfile(int moduleId)
        {
            var info = new Dictionary<string, string>();

            var xmlFileName = moduleId + ".xml";


            if (!Directory.Exists(DirectoryPach)) Directory.CreateDirectory(DirectoryPach);
            string path = DirectoryPach + "\\" + xmlFileName;
            if (File.Exists(path) ==
                false) return info;
            return XmlReadSave.Read(path);

        }




        //public static void Save(Dictionary<string, string> info, string path)
        //{
        //    try
        //    {
        //        if (!path.EndsWith(".xml"))
        //        {
        //            path += ".xml";
        //        }

        //        //string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
        //        //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        //        //string path = dir + "\\" + xmlFileName;
        //        if (File.Exists(path)) File.Delete(path);


        //        XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
        //        writer.Formatting = Formatting.Indented; //使用自动缩进便于阅读
        //        writer.WriteStartDocument(); //XML声明
        //        writer.WriteStartElement("Root"); //书写根元素
        //        foreach (var t in info)
        //        {
        //            if (string.IsNullOrEmpty(t.Key) || string.IsNullOrEmpty(t.Value)) continue;

        //            writer.WriteStartElement("XmlConfig"); //开始一个元素
        //            writer.WriteAttributeString("key", t.Key); //向先前创建的元素中添加一个属性
        //            writer.WriteAttributeString("value", t.Value); //向先前创建的元素中添加一个属性
        //            writer.WriteEndElement(); // 关闭元素
        //        }
        //        //在节点间添加一些空

        //        writer.Close();


        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteLogError("Save Xml Error: Path :" + path + ",Error:" + ex);
        //    }

        //}


        //public static Dictionary<string, string> Read(string path)
        //{
        //    var info = new Dictionary<string, string>();

        //    if (!path.EndsWith(".xml"))
        //    {
        //        path += ".xml";
        //    }
        //    //string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
        //    //if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        //    //string path = dir + "\\" + xmlFileName;
        //    //if (File.Exists(path))
        //    //    path = path;

        //    if (!File.Exists(path)) return info;

        //    try
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.Load(path);

        //        XmlNode root = xmlDoc.SelectSingleNode("Root");
        //        if (root != null)
        //        {
        //            var nodelist = root.ChildNodes;

        //            foreach (XmlNode nodeType in nodelist)
        //            {
        //                XmlElement element = (XmlElement)nodeType;
        //                if (element != null)
        //                {
        //                    try
        //                    {
        //                        string key = element.GetAttribute("key");
        //                        string value = element.GetAttribute("value");
        //                        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !info.ContainsKey(key))
        //                        {
        //                            info.Add(key, value);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteLog.WriteLogError("Core Config ReadConfig Error: GetConfigFilePaht path: " + configfilepath +
        //        //                       ", nodeName :" + nodename + "; Ex:" + ex);
        //        // WriteLog.WriteLogError("Read Xml Error: Path :" + path + ",Error:" + ex);
        //    }
        //    return info;
        //}
    }
}
 