using System;
using System.Threading;
using System.Windows;
using Login.Login;

using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;

namespace Login
{
    [ModuleExport(typeof (LoginModule))]
    public class LoginModule : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            try
            {
                //LoginView tempWindow = new LoginView();
                //tempWindow.Show();

                Initializzze();
            }
            catch (Exception e)
            {
                if (Application.Current.MainWindow.Visibility != Visibility.Visible) Environment.Exit(0);
            }
        }


        public void Initializzze()
        {
            //LoginView tempWindow = new LoginView();
            //tempWindow.Show();

            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }
        private void ThreadStartingPoint()
        {
            LoginView tempWindow = new LoginView();
            tempWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
        }


        #endregion


        ////定义委托
        //private delegate void DoTaskTwo();

        //public void Initializzze()
        //{
        //    Application.Current.Dispatcher.Invoke(
        //                 System.Windows.Threading.DispatcherPriority.Normal, new DoTaskTwo(Initializzze));
        //}

        //private void InitThread()
        //{
        //          Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
        //    newWindowThread.SetApartmentState(ApartmentState.STA);
        //    newWindowThread.IsBackground = true;
        //    newWindowThread.Start();
        //}

        //private void ThreadStartingPoint()
        //{
        //    LoginView tempWindow = new LoginView();
        //    tempWindow.Show();
        //    System.Windows.Threading.Dispatcher.Run();
        //}
    }
}