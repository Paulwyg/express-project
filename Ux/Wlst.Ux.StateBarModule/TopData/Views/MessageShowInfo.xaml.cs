using System.Windows;
using System.Windows.Input;

namespace Wlst.Ux.StateBarModule.TopData.Views
{
    /// <summary>
    /// MessageShowInfo.xaml 的交互逻辑
    /// </summary>

    public partial class MessageShowInfo : Window
    {
        /// <summary>
        /// 禁止在外部实例化
        /// </summary>
        private MessageShowInfo()
        {
            InitializeComponent();
        }

        public  string Title
        {
            get { return this.lblTitle.Text; }
            set { this.lblTitle.Text = value; }
        }

        public   string Message
        {
            get { return this.lblMsg.Text; }
            set {

                this.lblMsg.Text = value;
            }
        }

        public string BtnYes
        {
            get { return this.btnYes.Content.ToString(); }
            set { this.btnYes.Content = value; }
        }


        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static bool? Show(string title, string msg)
        {
            var msgBox = new MessageShowInfo();
            msgBox.Title = title;
            msgBox.Message = msg;
            msgBox.Topmost = true;
            msgBox.btnYes.Content = "确定";

            return msgBox.ShowDialog();
        }

        //private void Yes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    this.DialogResult = true;
        //    this.Close();
        //}
        private void Yes_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    };

}
