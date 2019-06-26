using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Wlst.Cr.Core.UtilityFunction
{
    internal class SystemXmlConfig
    {
        public static void Save(Dictionary<string, string> info, string xmlFileName)
        {
            try
            {
                if (!xmlFileName.EndsWith(".xml"))
                {
                    xmlFileName += ".xml";
                }

                string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string path = dir + "\\" + xmlFileName;
                if (File.Exists(path)) File.Delete(path);

                Wlst.Cr.Coreb.Servers.XmlReadSave.Save(info, path);



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
            string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\" + xmlFileName;
            if (File.Exists(xmlFileName))
                path = xmlFileName;
            if (!File.Exists(path)) return info;
            return Wlst.Cr.Coreb.Servers.XmlReadSave.Read(path);

        }
    }
}
