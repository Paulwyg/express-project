using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using Wlst.Cr.Core.UtilityFunction;

namespace Wlst.Cr.CoreOne.Services
{
    [Serializable]
    public class ImageSourceHelper
    {
        private static Dictionary<int, string> _dictionary = new Dictionary<int, string>();

        private static Dictionary<int, string> _dicName = new Dictionary<int, string>();
        private static List<int> nonexist = new List<int>(); 
        private static ImageSourceHelper _myself;

        public static ImageSourceHelper MySelf
        {
            get
            {
                if (_myself == null) _myself = new ImageSourceHelper();
                return _myself;
            }
        }

        private ImageSourceHelper()
        {
            LoadImageSource();
        }

        /// <summary>
        /// 使用
        /// </summary>
        private void LoadImageSource()
        {
            _dictionary.Clear();

            try
            {
                string fileName = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Image" + "//" +
                                  "ImageSource.Xaml";
                if (!File.Exists(fileName)) return;
                LoadInternationalizationGlobalizationData(fileName);
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("ImageSourceHelper Load Error:" + ex);
                //todo  witre error log
            }
        }


        private void LoadInternationalizationGlobalizationData(string path)
        {

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                var fatherNode = xmlDoc.SelectSingleNode("ImageSource");
                if (fatherNode == null) return;

                XmlNodeList nodeList = fatherNode.ChildNodes;
                foreach (XmlNode nodeType in nodeList)
                {
                    try
                    {
                        XmlElement element = (XmlElement) nodeType;
                        int elementName = Convert.ToInt32(element.GetAttribute("name").Trim());
                        string elementValue = element.GetAttribute("value").Trim();
                        string elementText = element.GetAttribute("text").Trim();

                        if (!string.IsNullOrEmpty(elementText) && !_dicName.ContainsKey(elementName))
                        {
                            _dicName.Add(elementName, elementText);
                        }

                        if (string.IsNullOrEmpty(elementValue)) continue;
                        if (_dictionary.ContainsKey(elementName)) continue;
                        _dictionary.Add(elementName, elementValue);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>存在则返回  不存在则返回 ""</returns>
        public string GetImageRelativeSourceById(int id)
        {
            if (_dictionary.ContainsKey(id)) return _dictionary[id];
            else return "";
        }

        /// <summary>
        /// 通过地址获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回 null</returns>
        public BitmapSource GetBitmapSourceById(int id)
        {
            if (nonexist.Contains(id)) return null;
            var path = GetImageRelativeSourceById(id);
            if (string.IsNullOrEmpty(path))
            {
                path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Image" + "//Icon//" + id +
                       ".png";
            }
            string rPath = path;
            if (!File.Exists(path))
            {
                rPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path;
            }
            if (!File.Exists(rPath))
            {
                if (!nonexist.Contains(id)) nonexist.Add(id);
                return null;
            }
            return GetBitmap(rPath);
        }

        /// <summary>
        /// 通过地址获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回 null</returns>
        public BitmapImage GetBitmapImageById(int id)
        {
            if (nonexist.Contains(id)) return null;
            var path = GetImageRelativeSourceById(id);
            if (string.IsNullOrEmpty(path))
            {
                path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Image" + "//Icon//" + id +
                       ".png";
            }
            string rPath = path;
            if (!File.Exists(path))
            {
                rPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path;
            }
            if (!File.Exists(rPath))
            {
                if (!nonexist.Contains(id)) nonexist.Add(id);
                return null;
            }


            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = System.IO.File.OpenRead(rPath);
            bitmapImage.EndInit();

            return bitmapImage;
        }

        /// <summary>
        /// 通过地址获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回 null</returns>
        public string GetAbsolutePathById(int id)
        {
            try
            {
                var path = GetImageRelativeSourceById(id);

                if (string.IsNullOrEmpty(path)) return null;
                string rPath = path;
                if (!File.Exists(path))
                {
                    rPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path;
                }
                if (!File.Exists(rPath)) return null;
                return rPath;
            }
            catch (Exception ex)
            {
            }
            return null;

        }

        /// <summary>
        /// 通过地址获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回 null</returns>
        public Image GetImageById(int id)
        {
            try
            {
                var path = GetImageRelativeSourceById(id);

                if (string.IsNullOrEmpty(path)) return null;
                string rPath = path;
                if (!File.Exists(path))
                {
                    rPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path;
                }
                if (!File.Exists(rPath)) return null;
                return Image.FromFile(rPath);
            }
            catch (Exception ex)
            {
            }
            return null;

        }


        /// <summary>
        /// 通过地址获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回 null</returns>
        public ImageSource GetImageSourceById(int id)
        {
            return this.GetBitmapSourceById(id);
        }

        public string GetTextById(int id)
        {
            if (_dicName.ContainsKey(id)) return _dicName[id];
            return string.Empty;
        }

        /// <summary>
        /// 通过地址获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns>不存在返回 null</returns>
        public Icon GetIconById(int id)
        {
            try
            {
                var path = GetImageRelativeSourceById(id);

                if (string.IsNullOrEmpty(path)) return null;
                string rPath = path;
                if (!File.Exists(path))
                {
                    rPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + path;
                }
                if (!File.Exists(rPath)) return null;


                System.Drawing.Size size = new System.Drawing.Size(16, 16);

                Bitmap srcBitmap = new Bitmap(rPath);

                Bitmap icoBitmap = new Bitmap(srcBitmap, size);
                //获得原位图的图标句柄
                var a = icoBitmap.GetHicon();
                //从图标的指定WINDOWS句柄创建Icon
                return System.Drawing.Icon.FromHandle(a);


                //Icon.ExtractAssociatedIcon(rPath);
                return Icon.ExtractAssociatedIcon(rPath);
            }
            catch (Exception ex)
            {
            }
            return null;

        }




        /// <summary>
        /// 通过路径获取bitmap图片格式
        /// </summary>
        /// <param name="imageSource"></param>
        /// <returns>不存在返回 null</returns>
        public static BitmapSource GetBitmap(string imageSource)
        {
            try
            {
                if (!File.Exists(imageSource)) return null;
                var img = System.Drawing.Image.FromFile(imageSource);
                var bmp = new System.Drawing.Bitmap(img);
                IntPtr hBitmap = bmp.GetHbitmap();
                var wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap
                    (hBitmap,
                     IntPtr.Zero,
                     Int32Rect.Empty,
                     BitmapSizeOptions.
                         FromEmptyOptions
                         ());

                return wpfBitmap;
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
