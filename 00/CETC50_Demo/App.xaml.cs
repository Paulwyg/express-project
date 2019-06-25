using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;

namespace CETC50_Demo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
           
            if (e.Args.Length > 0)
            {
                var sp = e.Args[0].Split('-');
                foreach (var f in sp)
                {
                    if (string.IsNullOrEmpty(f)) continue;
                    if (string.IsNullOrEmpty(Wlst.Cr.CoreMims.Services.UserInfo.SelfLoginIndefyLoginNamex))
                    {
                        Wlst.Cr.CoreMims.Services.UserInfo.SelfLoginIndefyLoginNamex = f.Trim();
                        continue;
                    }
                    if (string.IsNullOrEmpty(Wlst.Cr.CoreMims.Services.UserInfo.SelfLoginIndefyLoginPsw))
                    {
                        Wlst.Cr.CoreMims.Services.UserInfo.SelfLoginIndefyLoginPsw = f.Trim();
                        break;
                    }
                }
            }

            Check_program();

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

        private void Check_program()
        {
            Process[] process; //创建一个PROCESS类数组 
            process = Process.GetProcesses();//获取当前任务管理器所有运行中程序 
            Process processCurr = Process.GetCurrentProcess();//获取当前运行中程序 
            foreach (Process proces in process)//遍历 
            {
                if (proces.ProcessName == processCurr.ProcessName)
                {
                    if (proces.Id != processCurr.Id &&
                    proces.MainModule.FileName == processCurr.MainModule.FileName)
                    {
                        if (MessageBox.Show("发现相同程序正在运行，是否关闭上一程序? ", "警告", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            proces.Kill();
                        }
                        else
                        {
                            processCurr.Kill();
                        }

                        break;
                    }

                }
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

        //public void Initializzze()
        //{
        //    Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
        //    newWindowThread.SetApartmentState(ApartmentState.MTA);
        //    newWindowThread.IsBackground = true;
        //    newWindowThread.Start();
        //}
        //private void ThreadStartingPoint()
        //{
        //    var bts = new Xboot.Bootstrapper();
        //    bts.Run();
        //    System.Windows.Threading.Dispatcher.Run();
        //}

    }
}
