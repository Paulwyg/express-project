using System;
using System.Diagnostics;
using System.IO;

namespace Wlst.Cr.Coreb.Servers
{
   public class EmptyInapp
    {
        /// <summary>
        /// 在当前运行目录执行empty
        /// </summary>
       public static  void StartEmptyInBinpath()
        {
            if (File.Exists(Environment.CurrentDirectory + "\\empty.exe") == false) return;
            StartCmd(Environment.CurrentDirectory, "empty " + System.Diagnostics.Process.GetCurrentProcess().ProcessName + "*");
        }

        /// <summary>
        /// 给定empty目录 执行empty
        /// </summary>
        /// <param name="emptyPath"></param>
       public static  void StartEmpty(string emptyPath)
        {
            if (File.Exists(emptyPath + "\\empty.exe") == false) return;
            StartCmd(emptyPath, "empty " + System.Diagnostics.Process.GetCurrentProcess().ProcessName + "*");
        }

        /// <summary>
        /// 执行Cmd命令
        /// </summary>
        /// <param name="workingDirectory">要启动的进程的目录</param>
        /// <param name="command">要执行的命令</param>

        public static  void StartCmd(String workingDirectory, string command)
        {
            try
            {
                var p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.WorkingDirectory = workingDirectory;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine(command);
                p.StandardInput.WriteLine("exit");

            }
            catch (Exception ex)
            {
                //  WriteSystemLog.WriteSystemLogError("执行Cmd命令备份数据库时出错：" + ex.ToString());
            }
        }


    }
}
