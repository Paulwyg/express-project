using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WindowForWlst
{
    internal class TopmostButton : CaptionButton
    {
        static TopmostButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TopmostButton), new FrameworkPropertyMetadata(typeof(TopmostButton)));

        }

        protected override void OnClick()
        {
            base.OnClick();

            var w = Window.GetWindow(this) as CustomChromeWindow;

            if (w != null)
            {
                w.Topmost = !w.Topmost;

                string path = w.Topmost ? System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Image" + "//windows//" + "lock.png"
                    : System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Image" + "//windows//" + "unlock.png";

                this.Content = new System.Windows.Controls.Image
                                   {
                                       Source = GetBitmap(path)
                                   };


                var info = new Dictionary<string, string>();

                if (File.Exists((System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "//SystemXmlConfig//WindowTopmostStatus.txt")))
                {
                    using (
                        var sr =
                            new StreamReader(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
                                             "//SystemXmlConfig//WindowTopmostStatus.txt"))
                    {
                        try
                        {
                            while (sr.EndOfStream == false)
                            {
                                var line = sr.ReadLine();

                                if (line != null)
                                {
                                    string[] value = line.Split(',');

                                    info.Add(value[0], value[1]);
                                }
                            }
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                        sr.Close();
                    }

                    string removeKey = string.Empty;
                    foreach (var t in info)
                    {
                        if (t.Key == w.TitleCetc)
                        {
                            removeKey = t.Key;
                            break;
                        }
                    }

                    if (removeKey != string.Empty)
                    {
                        info.Remove(removeKey);
                    }
                }

                info.Add(w.TitleCetc, (w.Topmost ? 1 : 0) + "");


                using (var sw = new StreamWriter(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "//SystemXmlConfig//WindowTopmostStatus.txt", false))
                {
                    foreach (var t in info)
                    {
                        sw.WriteLine(t.Key + "," + t.Value);
                    }

                    sw.Close();
                }
            }
        }

        private static BitmapSource GetBitmap(string imageSource)
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