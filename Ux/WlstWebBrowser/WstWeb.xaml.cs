using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace WlstWebBrowser
{
    /// <summary>
    /// WstWeb.xaml 的交互逻辑
    /// </summary>
    public partial class WstWeb : UserControl
    {
        public WstWeb()
        {
            InitializeComponent();
        }

        private bool _firstload = true;

        /// <summary>
        /// 调用端 设置URL 只能设置一次
        /// </summary>
        /// <param name="url"></param>
        public void SetUrl(string url)
        {
            if (_firstload == false) return;
            _firstload = false;
            //WebBrowser隐藏网页的JavaScript错误
            this.WbrExam.SuppressScriptErrors(true);
            WbrExam.Navigating += WebBrowserMain_Navigating;

            ////WebBrowser与JavaScript交互
            //this.wbrExam.ObjectForScripting = new OprateBasic(this);
            WbrExam.Navigate(url);
        }
               
        void WebBrowserMain_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SetWebBrowserSilents(sender as WebBrowser, true);
        }

        /// <summary>
        /// 设置浏览器静默，不弹错误提示框
        /// </summary>
        /// <param name="webBrowser">要设置的WebBrowser控件浏览器</param>
        /// <param name="silent">是否静默</param>
        private void SetWebBrowserSilents(WebBrowser webBrowser, bool silent)
        {
            FieldInfo fi = typeof (WebBrowser).GetField("_axIWebBrowser2",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                object browser = fi.GetValue(webBrowser);
                if (browser != null)
                    browser.GetType()
                        .InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] {silent});
            }
        }


        //public void Message(string str)
        //{
        //    MessageBox.Show(str);
        //}

        ///// <summary>
        ///// WebBrowser与JavaScript交互
        ///// </summary>
        //[System.Runtime.InteropServices.ComVisible(true)]
        //public class OprateBasic
        //{
        //    private MainWindow instance;

        //    public OprateBasic(MainWindow instance)
        //    {
        //        this.instance = instance;
        //    }

        //    //提供给JS调用
        //    public void HandleMessage(string p)
        //    {
        //        instance.Message(p);
        //    }
        //}

        ////CS调用JS
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    this.wbrExam.InvokeScript("invokeScript", new object[] { "CS调用JS" });
        //}
    }

    /// <summary>
    /// WebBrowser隐藏网页的JavaScript错误
    /// </summary>
    public static class WebBrowserExtensions
    {
        public static void SuppressScriptErrors(this WebBrowser webBrowser, bool hide)
        {
            FieldInfo fiComWebBrowser = typeof (WebBrowser).GetField("_axIWebBrowser2",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;

            object objComWebBrowser = fiComWebBrowser.GetValue(webBrowser);
            if (objComWebBrowser == null) return;

            objComWebBrowser.GetType()
                .InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] {hide});
        }
    }
}

