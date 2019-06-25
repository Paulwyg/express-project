﻿using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Wlst.Cr.CoreOne.MessageBoxOverride
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
            get { return this.btnYes.Text; }
            set { this.btnYes.Text = value; }
        }

        public string BtnNo
        {
            get { return this.btnNo.Text; }
            set { this.btnNo.Text = value; }
        }

        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static bool? Show(string title, string msg,UMessageBoxButton uButton)
        {
            var msgBox = new UMessageBox();
            msgBox.Title = title;
            msgBox.Message = msg;
            if(uButton ==UMessageBoxButton.Yes )
            {
                msgBox.border2 .Visibility = Visibility.Collapsed;
                msgBox.btnYes.Text = "Yes";
            }
            else if (uButton == UMessageBoxButton.Ok)
            {
                msgBox.border2.Visibility = Visibility.Collapsed;
                msgBox.btnYes.Text = "Ok";
            }
            else if (uButton == UMessageBoxButton.YesNo )
            {
                msgBox.border2.Visibility = Visibility.Visible;
                msgBox.btnYes.Text = "Yes";
                msgBox.btnNo .Text = "No";
            }
            else if (uButton == UMessageBoxButton.OkCancel )
            {
                msgBox.border2.Visibility = Visibility.Visible;
                msgBox.btnYes.Text = "Ok";
                msgBox.btnNo.Text = "Cancel";
            }
            return msgBox.ShowDialog();
        }

        private void Yes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
            this.Close();  
        }


        private void No_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

