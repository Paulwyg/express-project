using System;
using System.Collections.Generic;
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

namespace Wlst.Ux.About.UxAbout.Views
{
    /// <summary>
    /// WebPager.xaml 的交互逻辑
    /// </summary>
    public partial class WebPager : Window
    {
        public WebPager()
        {
            InitializeComponent();

            Window12.SetUrl("http://180.169.111.33:64813/gis_main_zhuhai_ie/gis_demo.html");
        }
    }
}
