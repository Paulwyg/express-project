using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Wlst.Sr.EquipmentInfoHolding.Config
{


    internal class Config1
    {

        private const string LoadDataConfig = "PackageRequestRtus";

        public static int ConfigPackageRequestRtus
        {
            get
            {
                try
                {
                    string strPath = @"SystemXmlConfig\LoadDataConfig.xml";

                    if (!System.IO.File.Exists(strPath))
                    {
                        try
                        {
                            PackageRequestRtusConfigXml log4NetXml = new PackageRequestRtusConfigXml();

                            if (!System.IO.Directory.Exists("SystemXmlConfig"))
                            {
                                System.IO.Directory.CreateDirectory("SystemXmlConfig");
                            }
                            using (System.IO.StreamWriter sw = System.IO.File.CreateText(strPath))
                            {
                                foreach (var t in log4NetXml.LstXml)
                                    sw.WriteLine(t);
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }
                    var get = ReadPackageRequestRtusConfigXmlConfig.GetConfigFilePaht(strPath, LoadDataConfig);
                    int xxx = Convert.ToInt32(get);
                    if (xxx < 5) xxx = 5;
                    if (xxx > 99) xxx = 99;
                    return xxx;
                }
                catch (Exception ex)
                {
                    return 99;
                }
            }
        }



    }

    internal class ReadPackageRequestRtusConfigXmlConfig
    {
        public static string GetConfigFilePaht(string configfilepath, string nodename)
        {
            string rtn = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configfilepath);

                var selectSingleNode = xmlDoc.SelectSingleNode("configuration");
                if (selectSingleNode != null)
                {
                    XmlNodeList nodeListMenu = selectSingleNode.ChildNodes;
                    foreach (XmlNode nodeType in nodeListMenu)
                    {
                        XmlElement element = (XmlElement) nodeType;
                        if (element.Name.Equals(nodename))
                        {
                            rtn = element.GetAttribute("value");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SystemXmlConfig LoadDataConfig 配置文件读取出错...,Error:" + ex);
            }
            return rtn;
        }
    }



    internal class PackageRequestRtusConfigXml
    {
        public List<string> LstXml;

        public PackageRequestRtusConfigXml()
        {
            LstXml = new List<string>();
            Load();
        }

        private void Load()
        {
            LstXml.Add("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            LstXml.Add("<!--请勿擅自更改此文件内容。-->");
            LstXml.Add("<configuration>");
            LstXml.Add(
                "<PackageRequestRtus value=\"99\">");
            LstXml.Add("</PackageRequestRtus>");
            LstXml.Add("</configuration>");
        }

    }


}
