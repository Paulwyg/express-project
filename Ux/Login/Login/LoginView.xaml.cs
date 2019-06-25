using System;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Login.Login
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        private bool isInSigninArea = true;
        private bool isInConfigArea = false;

        public LoginView()
        {
            this.InitializeComponent();
            VisualStateManager.GoToElementState(this.Base, "StateSigninIn", true);
            LoginViewModel lvm = new LoginViewModel();
            lvm.OnLoginSuccess += OnShowMain;
            DataContext = lvm;
        }



        void OnShowMain(object sender, EventArgs args)
        {
            this.Dispatcher.BeginInvoke((Action) this.Hide);

        }

        private void evt_btn_exit_app(object sender, System.Windows.RoutedEventArgs e)
        {
            base.OnClosed(e);
            Environment.Exit(11);
        }

        private void evt_btn_signin(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!isInSigninArea)
            {
                isInSigninArea = true;
                isInConfigArea = false;
                VisualStateManager.GoToElementState(this.Base, "StateSigninIn", true);
            }
        }

        private void evt_btn_config(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!isInConfigArea)
            {
                isInConfigArea = true;
                isInSigninArea = false;
                VisualStateManager.GoToElementState(this.Base, "StateConfigIn", true);
            }
        }

        private void EvtClickBtnSignin(object sender, System.Windows.RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
        }
    }

}
