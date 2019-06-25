using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Wlst.Cr.Core.UtilityFunction;

namespace I36N.DataHolding
{
    public class I36NdataHolding
    {
        private static CultureInfo _cultureInfo = null;
        private static Dictionary<string, string> _dictionary = new Dictionary<string, string>();


        public I36NdataHolding()
        {

        }

        /// <summary>
        /// 使用前 请对 System.Threading.Thread.CurrentThread.CurrentUICulture设置
        /// </summary>
        private void ReloadCultureInfo()
        {
            _dictionary.Clear();
            if (_cultureInfo == null) return;
            string fileName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "language" + "//" +
                              _cultureInfo.Name;
            try
            {
                // if (!File.Exists(fileName)) return;
                var theFolder = new DirectoryInfo(fileName);
                FileInfo[] fileInfo = theFolder.GetFiles();
                //遍历文件夹
                foreach (FileInfo nextFile in fileInfo) //遍历文件
                {
                    if (nextFile.Extension == ".lang")
                    {
                        LoadInternationalizationGlobalizationData(nextFile.FullName);
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Culter data Load Error:" + ex);
                //todo  witre error log
            }
        }


        private void LoadInternationalizationGlobalizationData(string path)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                var fatherNode = xmlDoc.SelectSingleNode("I36N");
                if (fatherNode == null) return;

                XmlNodeList nodeList = fatherNode.ChildNodes;
                foreach (XmlNode nodeType in nodeList)
                {
                    XmlElement element = (XmlElement)nodeType;
                    string elementName = element.GetAttribute("name").Trim();
                    string elementValue = element.GetAttribute("value").Trim();
                    if (string.IsNullOrEmpty(elementName) || string.IsNullOrEmpty(elementValue)) continue;
                    if (_dictionary.ContainsKey(elementName)) continue;
                    _dictionary.Add(elementName, elementValue);
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Culter data Load Error: path:" + path + ";Exception" + ex);
            }
        }

        /// <summary>
        /// 更改CultureInfo
        /// </summary>
        public CultureInfo CurrentCultureInfo
        {
            get { return _cultureInfo; }
            set
            {
                if (_cultureInfo != null)
                {
                    if (_cultureInfo.Name.Equals(value.Name)) return;
                }
                _cultureInfo = value;
                ReloadCultureInfo();
            }
        }



        /// <summary>
        /// 获取名称，如果存在则返回，不存在则返回null
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>如果存在则返回，不存在则返回null</returns>
        public string GetName(string id)
        {
            if (_dictionary.ContainsKey(id))
            {
                return _dictionary[id];
            }
            return null;
        }


    }
}
