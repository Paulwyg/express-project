
using System.Windows.Controls;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.WJ4005Module.Wj4005TmlInfoSetViewModel.ViewModel;

namespace Wlst.Ux.WJ4005Module.Wj4005TmlInfoSetViewModel.Views
{
    /// <summary>
    /// TerminalInfoBaseView.xaml 的交互逻辑
    /// </summary>
    public partial class TerminalInfoBaseViewMsl
    {
        public TerminalInfoBaseViewMsl()
        {
            InitializeComponent();
        }

        private void comboBox7_LostFocus_1(object sender, System.Windows.RoutedEventArgs e)
        {
            TmlInformationViewModel.ComboBoxText = ((ComboBox)sender).Text;
        }
    }
}
