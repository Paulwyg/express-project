using System;
using System.Windows.Input;

namespace Login.Login
{
    public interface IILoginView
    {
        //string msg { get; }

        string IpAddr { get; set; }

        int IpPort { get; set; }

        string IpAddrBak { get; set; }

        int IpPortBak { get; set; }

        int ServerStyle { get; set; }

        string UserName { get; set; }

        string UserPsw { get; set; }

        ICommand CmdExit { get; }

        ICommand CmdLogin { get; }

        event EventHandler OnLoginSuccess;


    }
}