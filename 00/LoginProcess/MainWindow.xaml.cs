using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowForWlst;

namespace LoginProcess
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow :CustomChromeWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new LgpVm();

            Elysium.Manager.Apply(Application.Current, Elysium.Theme.Light);
            LoadBkgd();
            this.ResizeMode = ResizeMode.NoResize;

            this.Title = "快捷登陆";
        }

        private void LoadBkgd()
        {
            string dir = Directory.GetCurrentDirectory() + "\\Image";
            if (!Directory.Exists(dir)) return;
            string path = dir + "\\yxjsj.jpeg";

            if (File.Exists(path))
            {
                ImageBrush b3 = new ImageBrush();
                b3.ImageSource = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                this.xgd .Background = b3;
                return;
            }
            path = dir + "\\yxjsj.jpg";

            if (File.Exists(path))
            {
                ImageBrush b3 = new ImageBrush();
                b3.ImageSource = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                this.xgd.Background = b3;
                return;
            }
        }
    }
}
