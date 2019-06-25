using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;

namespace CETC50_Demo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            //DateTime dt = new DateTime(635488415400000000);
            //dt = DateTime.Now;

            base.OnStartup(e);
            base.DispatcherUnhandledException +=
                new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);

            try
            {
                //  Initializzze();

                var bts = new Xboot.Bootstrapper();
                bts.Run();
            }
            catch (Exception ec)
            {

                //  MessageBox.Show("System Err,Quit Now", "Error", MessageBoxButton.OK);
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Application Run Error In App,Exception is:" + ec);
                Environment.Exit(0);
            }
        }

        private void App_DispatcherUnhandledException(object sender,
                                                      System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Application Run Supper Error In App,Exception is:" +
                                                                e.Exception);
            //throw new NotImplementedException();
        }
    }
}
