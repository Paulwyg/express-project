using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Wlst.Cr.MessageBoxOverride.MessageBoxOverride
{
    /// <summary>
    /// UMessageBoxWantSomefromUser.xaml 的交互逻辑
    /// </summary>
    public partial class UMessageBoxWantSomefromUser : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));


        }
        #region  ICommand

        private ICommand _enter;
        public ICommand Enter
        {
            get { return _enter ?? ((_enter = new DelegateCommand(ExEnter))); }
        }
        private void ExEnter()
        {
            this.DialogResult = true;
            this.Close();
        }
        #endregion
        /// <summary>
        /// 禁止在外部实例化
        /// </summary>
        private UMessageBoxWantSomefromUser()
        {
            InitializeComponent();
            DataContext = this;
        }

        public new string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        public string Message
        {
            get { return this.lblMsg.Text; }
            set { this.lblMsg.Text = value; }
        }

        private string _txtText;
        public string TxtText
        {
            get { return _txtText; }
            set
            {
                if (_txtText == value) return;
                _txtText = value;
                NotifyPropertyChange("TxtText");
            }
        }

        /// <summary>
        /// 如果返回为本数据则说明用户点击了 取消
        /// </summary>
        public const string CancelReturn = "falsefalsefalsefalsefalse123321";

        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        /// <param name="txt">默认输入框中显示的数据 </param>
        /// <returns>如果用户输入则返回输入内容，如果用户取消或是退出则返回 参数 CancelReturn= "falsefalsefalsefalsefalse123321" 固定值</returns>
        public static string Show(string title, string msg, string txt)
        {
            var msgBox = new UMessageBoxWantSomefromUser();
            msgBox.Title = title;
            msgBox.Message = msg;
            msgBox.TxtText = txt;
            var rs = msgBox.ShowDialog();
            if (rs == false) return CancelReturn;
            if (rs == true) return msgBox.TxtText;
            return CancelReturn;
        }

        //private void Yes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.DialogResult = true;
        //    this.Close();

        //}


        //private void No_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.DialogResult = false;
        //    this.Close();
        //}

        private void Yes_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void No_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

