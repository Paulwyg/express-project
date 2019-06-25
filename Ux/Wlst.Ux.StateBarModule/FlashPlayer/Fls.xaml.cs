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

namespace Wlst.Ux.StateBarModule.FlashPlayer
{
    /// <summary>
    /// Fls.xaml 的交互逻辑
    /// </summary>
    public partial class Fls : Window
    {
        public Fls()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AxShockwaveFlashObjects.AxShockwaveFlash axShockwaveFlash = new AxShockwaveFlashObjects.AxShockwaveFlash();
            windowsFormsHost1.Child = axShockwaveFlash;
            string swfPath = System.Environment.CurrentDirectory;
            swfPath += @"\FlashPlayer\xxx.swf";
            axShockwaveFlash.Movie = swfPath;

            //axShockwaveFlash.FlashCall += new AxShockwaveFlashObjects._IShockwaveFlashEvents_FlashCallEventHandler(axShockwaveFlash_FlashCall);
        }
    }
}
