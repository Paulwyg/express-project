using System.Windows.Input;
using Wlst.Cr.CoreOne.MessageBoxOverride.WlstMessageBox.ViewModel;

namespace Wlst.Cr.CoreOne.MessageBoxOverride.WlstMessageBox.View
{
    /// <summary>
    /// MessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class WlstMessageBox
    {
        private static WlstMessageBox _messageBox;
        public WlstMessageBox()
        {
            InitializeComponent();
        }
        private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        public WlstMessageBoxResults Result { get; set; }

        public static WlstMessageBoxResults Show(string title, string message, string details, WlstMessageBoxType style)
        {
            _messageBox = new WlstMessageBox();
            var viewModel = new WlstMessageBoxViewModel(_messageBox, title, message, details, style);
            _messageBox.DataContext = viewModel;
            _messageBox.ShowDialog();
            return _messageBox.Result;
        }

        public static WlstMessageBoxResults Show(string message)
        {
            return Show("上海五零盛同信息科技有限公司", message, string.Empty, WlstMessageBoxType.Ok);
        }

        public static WlstMessageBoxResults Show(string title, string message)
        {
            return Show(title, message, string.Empty, WlstMessageBoxType.Ok);
        }
        public static WlstMessageBoxResults Show(string message, WlstMessageBoxType style)
        {
            return Show("上海五零盛同信息科技有限公司", message, string.Empty, style);
        }
        public static WlstMessageBoxResults Show(string title, string message, string details)
        {
            return Show(title, message, details, WlstMessageBoxType.Ok);
        }

        public static WlstMessageBoxResults Show(string title, string message, WlstMessageBoxType style)
        {
            return Show(title, message, string.Empty, style);
        }

    }
}
