using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wlst.Cr.CoreMims.Services
{
    public class ImageIcon
    {
        private static Dictionary<string, BitmapImage> Images = new Dictionary<string, BitmapImage>();
        private static Dictionary<string, BitmapFrame> ImagesBitmapFrame = new Dictionary<string, BitmapFrame>();

        private static List<Tuple<string, string>> GetAllFiles()
        {
            string path =System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"Image/EquipmentImage";

            var rtn = new List<Tuple<string, string>>();
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                if (fi.FullName.Contains(".png") == false) continue;
                var name = fi.Name;
                name = name.Replace(".png", "");
                rtn.Add(new Tuple<string, string>(name, fi.FullName));
            }

            return rtn;
        }

        private static BitmapImage GetBitmapImageByPath(string path)
        {

            if (!File.Exists(path))
            {
                return null;
            }
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = System.IO.File.OpenRead(path);
                bitmapImage.EndInit();
               return  bitmapImage.Clone();
                return bitmapImage;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        private static BitmapFrame GetBitmapFrameImageByPath(string path)
        {

            if (!File.Exists(path))
            {
                return null;
            }
            try
            {
                var imageSourceConverter = new ImageSourceConverter();
                var byList = new List<byte[]>();

                using (BinaryReader binReader = new BinaryReader(File.Open(path, FileMode.Open)))
                {
                    FileInfo fileInfo = new FileInfo(path);

                    if ((long)int.MaxValue > fileInfo.Length)
                    {
                        byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                        byList.Add(bytes);
                    }
                    else
                    {
                        int leng = 1024;
                        byte[] bytes = new byte[fileInfo.Length];
                        for (long j = 0; j < (fileInfo.Length / (long)leng + (long)1); j++)
                        {
                            byte[] b = binReader.ReadBytes(leng);
                            if (b == null || b.Length < 1)
                            {
                                break;
                            }
                            for (long jj = j * leng; jj < (j + 1) * leng; jj++)
                            {
                                bytes[jj] = b[jj % leng];
                            }
                        }
                        byList.Add(bytes);
                    }
                }

                var stream = new MemoryStream(byList[0]);
                return imageSourceConverter.ConvertFrom(stream) as BitmapFrame;

            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private static object obj = 1;

        static void LoadImage()
        {
            if (Images.Count == 0)
            {
                lock (obj)
                {
                    if (Images.Count == 0)
                    {
                        var lst = GetAllFiles();
                        foreach (var f in lst)
                        {
                          
                            var imgFrm = GetBitmapFrameImageByPath(f.Item2);
                            if (imgFrm != null && ImagesBitmapFrame.ContainsKey(f.Item1) == false)
                                ImagesBitmapFrame.Add(f.Item1, imgFrm);
                            
                            var img = GetBitmapImageByPath(f.Item2);
                            if (img != null && Images.ContainsKey(f.Item1) == false) Images.Add(f.Item1, img);


                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取EquipmentImage目录下的png图片 通过名字获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static BitmapImage GetBitmapImageByName(string name)
        {
            if (Images.Count == 0) LoadImage();

            if (Images.ContainsKey(name)) return Images[name];
            if (Images.ContainsKey("unknown")) return Images["unknown"];

            return null;
        }

        public static BitmapImage GetBitmapImage(string name)
        {
            return GetBitmapImageByName(name);
        }

        public static BitmapImage GetBitmapImage(int name)
        {
            return GetBitmapImageByName(name + "");
        }



        /// <summary>
        /// 获取EquipmentImage目录下的png图片 通过名字获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static BitmapFrame GetBitmapFrameByName(string name)
        {
            if (ImagesBitmapFrame.Count == 0) LoadImage();

            if (ImagesBitmapFrame.ContainsKey(name)) return ImagesBitmapFrame[name];
            if (ImagesBitmapFrame.ContainsKey("unknown")) return ImagesBitmapFrame["unknown"];

            return null;
        }

        public static BitmapFrame GetBitmapFrame(string name)
        {
            return GetBitmapFrameByName(name);
        }

        public static BitmapFrame GetBitmapFrame(int name)
        {
            return GetBitmapFrameByName(name + "");
        }

    }
}
