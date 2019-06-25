using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WindowForWlst
{
    public class CustomChromeWindow : Window
    {

        #region DependencyProperty


        public double CaptionHeight
        {
            get { return (double)GetValue(CaptionHeightProperty); }
            set { SetValue(CaptionHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CaptionHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionHeightProperty =
            DependencyProperty.Register("CaptionHeight", typeof(double), typeof(CustomChromeWindow), new UIPropertyMetadata(23d));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CustomChromeWindow), new UIPropertyMetadata(default(CornerRadius)));

        public string TitleCetc
        {
            get { return (string)GetValue(TitleCetcProperty); }
            set { SetValue(TitleCetcProperty, value); }
        }

        public static readonly DependencyProperty TitleCetcProperty =
            DependencyProperty.Register("TitleCetc", typeof (string), typeof (CustomChromeWindow),
                                        new UIPropertyMetadata(string.Empty));

        public Visibility TopmostButtonVisibility
        {
            get { return (Visibility)GetValue(TopmostButtonVisibilityProperty); }
            set { SetValue(TopmostButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty TopmostButtonVisibilityProperty =
            DependencyProperty.Register("TopmostButtonVisibility", typeof(Visibility), typeof(CustomChromeWindow), new UIPropertyMetadata(Visibility.Visible));


        public Visibility MaximizeButtonVisibility
        {
            get { return (Visibility)GetValue(MaximizeButtonVisibilityProperty); }
            set { SetValue(MaximizeButtonVisibilityProperty, value); }
        }
        public static readonly DependencyProperty MaximizeButtonVisibilityProperty =
            DependencyProperty.Register("MaximizeButtonVisibility", typeof(Visibility), typeof(CustomChromeWindow), new UIPropertyMetadata(Visibility.Visible));



        #endregion


        /// <summary>
        /// Initializes the metadata for the window
        /// </summary>
        static CustomChromeWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomChromeWindow),
                new FrameworkPropertyMetadata(typeof(CustomChromeWindow)));
        }

        public CustomChromeWindow()
        {

            // 显示系统菜单项
            var showSystemMenuCmdBinding = new CommandBinding(
                Microsoft.Windows.Shell.SystemCommands.ShowSystemMenuCommand,
                _OnShowSystemMenuCommand
                );
            // 关闭窗体命令
            var closeWindowCmdBinding = new CommandBinding(
               Microsoft.Windows.Shell.SystemCommands.CloseWindowCommand,
               _OnCloseWindowCommand
               );

            // 绑定路由命令
            CommandBindings.Add(showSystemMenuCmdBinding);
            CommandBindings.Add(closeWindowCmdBinding);
        }

        protected override void OnContentRendered(EventArgs e)
        {
            if (
                File.Exists((System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
                             "//SystemXmlConfig//WindowTopmostStatus.txt")))
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

                                if (value[0] == this.TitleCetc)
                                {
                                    this.Topmost = Convert.ToBoolean(Convert.ToInt32(value[1]));
                                }
                            }
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    sr.Close();
                }
            }

            var b = this.GetTemplateChild("TopmostButton") as TopmostButton;

            if (b != null)
            {
                string path = this.Topmost
                                  ? System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Image" +
                                    "//windows//" + "lock.png"
                                  : System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Image" +
                                    "//windows//" + "unlock.png";

                b.Content = new System.Windows.Controls.Image
                                {
                                    Source = GetBitmap(path)
                                };

                b.Visibility = TopmostButtonVisibility;

                //Console.WriteLine(this.TitleCetc);
            }

            var t = this.GetTemplateChild("TitleTextBlock") as TextBlock;

            if(t != null)
            {
                t.Text = TitleCetc;
            }

        }

        private void _OnCloseWindowCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var window = GetWindow(this);

            Microsoft.Windows.Shell.SystemCommands.CloseWindow(window);
        }

        private void _OnShowSystemMenuCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var window = GetWindow(this);
            if (window == null) return;
            var point = new Point(window.Left + 24, window.Top + 24);

            Microsoft.Windows.Shell.SystemCommands.ShowSystemMenu(window, point);
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
