using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.Models;

namespace LoginProcess
{
    public class LgpVm
    {
        private ObservableCollection<NameValueStringEx> _items;

        public ObservableCollection<NameValueStringEx> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<NameValueStringEx>();
                    var tmpx = Setting.LoadNewDataLenghtSetConfg();
                    int index = 1;
                    foreach (var f in tmpx)
                    {
                        _items.Add(new NameValueStringEx() {OriName = f.Item1, Value = f.Item2, Name = f.Item1,UserName =f.Item3 ,UserPsw =f.Item4 });
                    }
                   // _items.Add(new NameValueStringEx() { OriName = f.Item1, Value = f.Item2, Name = f.Item1, UserName = f.Item3, UserPsw = f.Item4 });
                    UpdateState();
                }
                return _items;
            }
        }


        private void UpdateState()
        {
            for (int j = Items.Count - 1; j >= 0; j--)
            {
                Items[j].UpdateState();
            }
        }

        
                private ICommand _cmCmdZcOrSnd;

        public ICommand CmdExit
        {
            get { return _cmCmdZcOrSnd ?? (_cmCmdZcOrSnd = new RelayCommand(ExCmdZcOrSnd, CanCmdZcOrSnd, false )); }
        }


        internal void ExCmdZcOrSnd()
        {
            Environment.Exit(0);
        }


      //  private bool lastStatex = false;
        internal bool CanCmdZcOrSnd()
        {
            return true;
        }


    }

    public class NameValueStringEx : Wlst.Cr.CoreOne.Models.NameValueString
    {

        public void UpdateState()
        {
            var brun = GetProcessNamebySetString(Value);
            if (string.IsNullOrEmpty(brun))
            {
                IsProcessIsRunning = true;
                ExtendName = "配置路径错误";
                ResetName();
                return;
            }
            var br = GetProcessIfIsRunning(brun);
            IsProcessIsRunning = br;
            ExtendName = br ? "正在运行" : "";
            ResetName();

        }


        private ICommand _cmCmdZcOrSnd;

        public ICommand CmdZcOrSnd
        {
            get { return _cmCmdZcOrSnd ?? (_cmCmdZcOrSnd = new RelayCommand(ExCmdZcOrSnd, CanCmdZcOrSnd, false )); }
        }


        internal void ExCmdZcOrSnd()
        {
            StartProcessIfNotRunning(Value);
            UpdateState();
        }


      //  private bool lastStatex = false;
        internal bool CanCmdZcOrSnd()
        {
            try
            {
                var x = GetProcessNamebySetString(Value );
                if (string.IsNullOrEmpty(x))
                    return false;
                var g = GetProcessIfIsRunning(x);
                if(g!=IsProcessIsRunning )UpdateState();
                IsProcessIsRunning = g;
                return g == false;
            }
            catch (Exception ex)
            {

            }
            return false;
        }


        internal  string GetProcessNamebySetString(string path)
        {
            if (path.Contains(".exe")) path = path.Replace(".exe", "");
            var sp = path.Split('\\', '/');
            if (sp.Count() == 0) return string.Empty;
            for (int j = sp.Count() - 1; j >= 0; j--) if (!string.IsNullOrEmpty(sp[j])) return sp[j];
            return string.Empty;
        }


        internal bool GetProcessIfIsRunning(string processName)
        {
            Process[] processes = Process.GetProcesses();
            for (int i = 0; i < processes.GetLength(0); i++)
            {
                //我是要找到我需要的YZT.exe的进程,可以根据ProcessName属性判断
                if (processes[i].ProcessName.Equals(processName))
                {
                    //立即停止关联的进程,建议不要用Close()方法
                    // processes[i].Kill();
                    return true;
                }
            }
            return false;
        }


        internal bool StartProcessIfNotRunning(string path)
        {
            var proName = GetProcessNamebySetString(path);
            if (string.IsNullOrEmpty(proName))
                return false;
            if (GetProcessIfIsRunning(proName)) return true;

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(UserPsw))
                Process.Start(path);
            else
                Process.Start(path, UserName + "-" + UserPsw);
            return true;
        }


        public string OriName;
        public string ExtendName;
        public string UserName;
        public string UserPsw;
        public bool IsProcessIsRunning;

        public void ResetName()
        {
            if (!string.IsNullOrEmpty(ExtendName))
                this.Name = OriName + " - " + ExtendName;
            else
                this.Name = OriName;
        }
    }
}
