using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.ViewModel;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLevelLowViewModel.ViewModel
{
    class EquipmentFaultRecordQueryEditXml
    {


        //写xml文件
        public static void WriteTime(int time)
        {
            try
            {

                string dir = Directory.GetCurrentDirectory() + "\\config";
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                //string path = dir + "\\" + "\\FaultRules.xml";
                string path = dir + "\\" + "EquipmentFaultRecordQueryColTime.xml";
                if (File.Exists(path)) File.Delete(path);

                try
                {
                    XmlTextWriter writer = new XmlTextWriter(path, System.Text.Encoding.UTF8);
                    writer.Formatting = Formatting.Indented; //使用自动缩进便于阅读
                    writer.WriteStartDocument(); //XML声明
                    writer.WriteStartElement("Time"); //书写根元素
                    writer.WriteAttributeString("No.1", time.ToString("D1"));
                    writer.WriteEndElement(); // 关闭元素
                 
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

     
        //读xml文件
        public static int ReadTime()
        {
            var info = 168;
            string dir = Directory.GetCurrentDirectory() + "\\config";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string path = dir + "\\EquipmentFaultRecordQueryColTime.xml";
            if (!File.Exists(path))
            {
                return 168 ;
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(path);

                XmlNode root = xmlDoc.SelectSingleNode("Time");
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
                                info = Convert.ToInt32(element.GetAttribute("No.1"));
                            }
                            catch (Exception ex)
                            {
                                Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("bug", "配置文件有误，请重新载入。",
                                                                                               UMessageBoxButton.Ok);
                                if (File.Exists(path)) File.Delete(path);
                                break;
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("bug", "配置文件为空。", UMessageBoxButton.Ok);
            }
            return info;
        }


    }
}
