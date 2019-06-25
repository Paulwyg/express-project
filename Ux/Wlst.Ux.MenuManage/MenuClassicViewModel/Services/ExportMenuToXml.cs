using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Wlst.Ux.MenuManage.MenuClassicViewModel.Services
{
    internal class ExportMenuToXml
    {
        public static void WriteMenuToXml()
        {
            try
            {
                var lst = new List<Tuple<int, string, string>>();
                var tmp =
                    (from t in Wlst.Cr.CoreOne.ComponentHold.MenuComponentHolding.GetAllMenuItem orderby t.Key select t)
                        .
                        ToList();
                lst.Add(new Tuple<int, string, string>(1001, "开关量输出回路开灯状态图标", "巡测或其他扼要显示"));
                lst.Add(new Tuple<int, string, string>(1002, "开关量输出回路关灯灯状态图标", "巡测或其他扼要显示"));
                foreach (var t in tmp)
                {
                    lst.Add(new Tuple<int, string, string>(t.Key, t.Value.Text, t.Value.Description));
                }
                ReadSave.Save(lst, "ImageSourcefromCode");
                tmp.Clear() ;// = null;
            }
            catch (Exception ex)
            {

            }
        }
    }


    internal class ReadSave
   {
       public static void Save(IEnumerable<Tuple<int, string, string>> info, string xmlFileName)
       {
           try
           {
               if (!xmlFileName.EndsWith(".xml"))
               {
                   xmlFileName += ".xml";
               }

               string dir = Directory.GetCurrentDirectory() + "\\Image";
               if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
               string path = dir + "\\" + xmlFileName;
               if (File.Exists(path)) File.Delete(path);

               try
               {
                   XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                   writer.Formatting = Formatting.Indented; //使用自动缩进便于阅读
                   writer.WriteStartDocument(); //XML声明
                   writer.WriteStartElement("ImageSource"); //书写根元素
                   foreach (var t in info)
                   {
                       if (t.Item1 < 10) continue;

                       writer.WriteStartElement("img"); //开始一个元素
                       writer.WriteAttributeString("name", t.Item1 + ""); //向先前创建的元素中添加一个属性
                       writer.WriteAttributeString("value", @"Image//Icon//"); //向先前创建的元素中添加一个属性
                       writer.WriteAttributeString("text", t.Item2); //向先前创建的元素中添加一个属性
                       writer.WriteAttributeString("desc", t.Item3); //向先前创建的元素中添加一个属性
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


       //public static Dictionary<string, string> Read(string xmlFileName)
       //{
       //    var info = new Dictionary<string, string>();

       //    if (!xmlFileName.EndsWith(".xml"))
       //    {
       //        xmlFileName += ".xml";
       //    }
       //    string dir = Directory.GetCurrentDirectory() + "\\SystemColorAndFont";
       //    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
       //    string path = dir + "\\" + xmlFileName;
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
       //    }
       //    return info;
       //}
   }
}
