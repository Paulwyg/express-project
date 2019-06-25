using System.Windows;
using System.Windows.Controls;

namespace Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.Views
{
    /// <summary>
    /// SndArgsView.xaml 的交互逻辑
    /// </summary>
    public partial class SndArgsView : UserControl
    {
        public SndArgsView()
        {
            InitializeComponent();
            //button14.Visibility = Visibility.Hidden;
            //button11.Visibility = Visibility.Hidden;
            //datepicker1.Visibility = Visibility.Hidden;
            //datepicker2.Visibility = Visibility.Hidden;
        }

        private int txt = 0;
        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            txt = txt + 1;
            if (txt >= 2)
            {
                ZcItems.Visibility = Visibility.Visible;
                //button14.Visibility = Visibility.Visible; 
                //button11.Visibility = Visibility.Visible;
                //datepicker1.Visibility = Visibility.Visible;
                //datepicker2.Visibility = Visibility.Visible;
                txt = 0;
            }
        }

        private void button14_Click(object sender, RoutedEventArgs e)
        {
            ZcItems.Visibility = Visibility.Hidden;
            //button14.Visibility = Visibility.Hidden;
            //button11.Visibility = Visibility.Hidden;
            //datepicker1.Visibility = Visibility.Hidden;
            //datepicker2.Visibility = Visibility.Hidden;
        }
    }
}
