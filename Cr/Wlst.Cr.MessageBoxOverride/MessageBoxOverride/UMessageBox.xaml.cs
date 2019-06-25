using System.Windows;
using System.Windows.Input;

namespace Wlst.Cr.MessageBoxOverride.MessageBoxOverride
{
    /// <summary>
    /// UMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class UMessageBox : Window
    {
        /// <summary>
        /// 禁止在外部实例化
        /// </summary>
        private UMessageBox()
        {
            InitializeComponent();
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

        public string BtnYes
        {
            get { return this.btnYes.Content.ToString(); }
            set { this.btnYes.Content = value; }
        }

        public string BtnNo
        {
            get { return this.btnNo.Content.ToString(); }
            set { this.btnNo.Content = value; }
        }

        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static bool? Show(string title, string msg, UMessageBoxButton uButton)
        {
            var msgBox = new UMessageBox();
            msgBox.Title = title;
            msgBox.Message = msg;
            msgBox.Topmost = true;
            if (uButton == UMessageBoxButton.Yes)
            {
              msgBox.btnNo.Visibility=Visibility.Collapsed;
                msgBox.btnYes.Content = "是";
            }
            else if (uButton == UMessageBoxButton.Ok)
            {
                msgBox.btnNo.Visibility = Visibility.Collapsed;
                msgBox.btnYes.Content = "确定";
            }
            else if (uButton == UMessageBoxButton.YesNo)
            {
                msgBox.btnNo.Visibility = Visibility.Visible;
                msgBox.btnYes.Content = "是";
                msgBox.btnNo.Content = "否";
            }
            else if (uButton == UMessageBoxButton.OkCancel)
            {
                msgBox.btnNo.Visibility = Visibility.Visible;
                msgBox.btnYes.Content = "确定";
                msgBox.btnNo.Content = "取消";
            }
            return msgBox.ShowDialog();
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
    };

    public enum UMessageBoxButton
    {
        Ok = 1,
        OkCancel,
        Yes,
        YesNo
    }

}

