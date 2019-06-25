using System.Windows;
using WindowForWlst;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Views.BaseView
{
    /// <summary>
    /// SetWeekAck.xaml 的交互逻辑
    /// </summary>
    public partial class SetWeekAck : CustomChromeWindow
    {
        public SetWeekAck()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "";

        }

        public void SetContext(WeekReport oit)
        {
            DataContext = oit;;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TimeInfoMnVm.ShowWeekReport(1);

            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TimeInfoMnVm.ShowWeekReport(2);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TimeInfoMnVm.SendWeekSet();
        }

    }
}
