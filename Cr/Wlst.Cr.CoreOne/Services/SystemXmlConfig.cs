using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Wlst.Cr.CoreOne.Services
{
    public class SystemXmlConfig
    {
        public static void Save(Dictionary<string, string> info, string xmlFileName,string filePath =null)
        {
            try
            {
                if (!xmlFileName.EndsWith(".xml"))
                {
                    xmlFileName += ".xml";
                }
                //lvf 2018年3月21日16:45:24  添加可填写路径的配置文件保存
                var fp = "\\SystemXmlConfig";
                if(!string.IsNullOrEmpty(filePath)) fp =filePath.Trim();
                string dir = Directory.GetCurrentDirectory() + fp;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                string path = dir + "\\" + xmlFileName;
                if (File.Exists(path)) File.Delete(path);

                Wlst.Cr.Coreb.Servers.XmlReadSave.Save(info, path);



            }
            catch (Exception ex)
            {
            }

        }



        public static Dictionary<string, string> Read(string xmlFileName, string filePath = null)
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
            return Wlst.Cr.Coreb.Servers.XmlReadSave.Read(path);

        }

    }
}
