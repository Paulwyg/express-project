using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Xml;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Ux.About.UxAbout.Services;

namespace Wlst.Ux.About.UxAbout.ViewModel
{
    [Export(typeof(IIUxAboutModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AboutViewModel :EventHandlerHelperExtendNotifyProperyChanged, Cr.Core.CoreInterface.IITab, IIUxAboutModule
    {
        public void NavOnLoad(params object[] parsObjects)
        {
            try
            {
                softwareName = "数字化城市照明监控管理平台软件";

                softwareVersion = "软件版本 \"v5.0.3-20180928\"";

               // protocolVersion = "协议版本 v20170501";

                companyNameA = "中国电子科技集团公司第五十研究所";

                companyNameB = "上海五零盛同信息科技有限公司";

                companyPhone = "021-62547600-6304";

                LoadAboutSourceData(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "about" + "//" +
                                    "AboutSource.xaml");

            }
            catch (Exception ec)
            {

            }
        }

        public void OnUserHideOrClosing()
        {

        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "关于"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }
        #endregion

        #region Attris
        #region softwareName
        private string _softwareName;
        public string softwareName
        {
            get { return _softwareName; }
            set
            {
                if (_softwareName == value) return;
                _softwareName = value;
                RaisePropertyChanged(() => softwareName);
            }
        }
        #endregion

        #region softwareVersion
        private string _softwareVersion;
        public string softwareVersion
        {
            get { return _softwareVersion; }
            set
            {
                if (_softwareVersion == value) return;
                _softwareVersion = value;
                RaisePropertyChanged(() => softwareVersion);
            }
        }
        #endregion

        #region protocolVersion
        private string _protocolVersion;
        public string protocolVersion
        {
            get { return _protocolVersion; }
            set
            {
                if (_protocolVersion == value) return;
                _protocolVersion = value;
                RaisePropertyChanged(() => protocolVersion);
            }
        }
        #endregion

        #region companyNameA
        private string _companyNameA;
        public string companyNameA
        {
            get { return _companyNameA; }
            set
            {
                if (_companyNameA == value) return;
                _companyNameA = value;
                RaisePropertyChanged(() => companyNameA);
            }
        }
        #endregion

        #region companyNameB
        private string _companyNameB;
        public string companyNameB
        {
            get { return _companyNameB; }
            set
            {
                if (_companyNameB == value) return;
                _companyNameB = value;
                RaisePropertyChanged(() => companyNameB);
            }
        }
        #endregion

        #region companyPhone
        private string _companyPhone;
        public string companyPhone
        {
            get { return _companyPhone; }
            set
            {
                if (_companyPhone == value) return;
                _companyPhone = value;
                RaisePropertyChanged(() => companyPhone);
            }
        }
        #endregion
        #endregion

        public AboutViewModel()
        {
            //            InitAction();
            //            InitEvent();

        }

        private void LoadAboutSourceData(string path)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                var fatherNode = xmlDoc.SelectSingleNode("AboutSource");
                if (fatherNode == null) return;

                XmlNodeList nodeList = fatherNode.ChildNodes;
                foreach (XmlNode nodeType in nodeList)
                {
                    try
                    {
                        XmlElement element = (XmlElement)nodeType;
                        int elementName = Convert.ToInt32(element.GetAttribute("name").Trim());
                        string elementText = element.GetAttribute("text").Trim();

                        if (!string.IsNullOrEmpty(elementText))
                        {
                            if (elementName == 0)
                            {
                                softwareName = elementText;
                            }
                            else if (elementName == 1)
                            {
                                softwareVersion = elementText;
                            }
                            else if (elementName == 2)
                            {
                                protocolVersion = elementText;
                            }
                            else if (elementName == 3)
                            {
                                companyNameA = elementText;
                            }
                            else if (elementName == 4)
                            {
                                companyNameB = elementText;
                            }
                            else if (elementName == 5)
                            {
                                companyPhone = elementText;
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Culter data Load Error: path:" + path + ";Exception" + ex);
            }
        }
    }
}
