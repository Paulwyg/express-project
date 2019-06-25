using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Wlst.Ux.ViewInstruction.ViewInstruction.Services
{
    public static class ReadViewInstructions
    {
        public static Dictionary<int, string> ReadInstructions(string xmlFileName)
        {
            var info = new Dictionary<int, string>();

            if (!xmlFileName.EndsWith(".xml"))
            {
                xmlFileName += ".xml";
            }
            string dir = Directory.GetCurrentDirectory() + "\\ViewsInstruction";
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
                                int key = Convert.ToInt32(element.GetAttribute("key"));
                                string value = element.GetAttribute("value");
                                if (key > 0 && !string.IsNullOrEmpty(value) && !info.ContainsKey(key))
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
            }
            return info;
        }

        public static bool ReadInstructionConfig()
        {
            bool info = false;
            string dir = Directory.GetCurrentDirectory() + "\\ViewsInstruction";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\ViewInstructionConfig.xml";
            if (!File.Exists(path)) return false;

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
                            if (element.GetAttribute("key") != "ViewInstructionConfig") continue;
                            info = element.GetAttribute("value") == "true";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return info;
        }
    }
}
