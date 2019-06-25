namespace Wlst.Ux.WJ3005Module.Wj3005TmlInfoSetViewModel.Views
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

        private void textBox5_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            Wlst.Cr.CoreOne.Services.OtherSvr.ChangeDatePickerToToday(sender,e);

        }
    }
}
