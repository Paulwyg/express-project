using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Login.Login
{
    //[Export(typeof (IILoginView))]
    //[PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LoginViewModel : Wlst.Cr.Core.CoreServices.ObservableObject, IILoginView
    {
        private const int LoginWaitTime = 6;
        private static Thread thread;


        public LoginViewModel()
        {

            GetValueFromDb();
            Msg = "欢迎登陆系统..."; // I36N.Services.I36N.ConvertByCodingOne("11000003", "Welcome Login System");

            switch (ServerStyle)
            {
                case 0:
                    ServerMain = false;
                    ServerBak = false;
                    break;
                case 1:
                    ServerMain = true;
                    ServerBak = false;
                    break;
                case 2:
                    ServerMain = false;
                    ServerBak = true;
                    break;
            }


            _bolCanLogin = true;

            InitAction();


            Wlst.Cr.CoreMims.Services.KeepAliveSocket.Myself.SndClientBeatTime = 55;
          //  Wlst.Cr.CoreMims.Services.KeepAliveSocket.Myself.StartHeatBeat();

            SelfLoginx();
        }



        private void SelfLoginx()
        {
            if (string.IsNullOrEmpty(Wlst.Cr.CoreMims.Services.UserInfo.SelfLoginIndefyLoginNamex)) return;
            if (string.IsNullOrEmpty(Wlst.Cr.CoreMims.Services.UserInfo.SelfLoginIndefyLoginPsw)) return;
            this.UserName = Wlst.Cr.CoreMims.Services.UserInfo.SelfLoginIndefyLoginNamex;
            this.UserPsw = Wlst.Cr.CoreMims.Services.UserInfo.SelfLoginIndefyLoginPsw;
            Thread.Sleep(3000);
            ExCmdLogin();
        }





        private bool _loginsuccessful = false;


        #region prop

        private string _msg;

        public string Msg
        {
            get { return _msg; }
            set
            {
                if (_msg != value)
                {
                    _msg = value;
                    this.RaisePropertyChanged(() => this.Msg);
                }
            }
        }


        public string Title
        {
            get { return "城市数字照明综合监控管理系统"; }
        }

        private string _ipAddr;


        public string IpAddr
        {
            get { return _ipAddr; }
            set
            {
                if (value == _ipAddr) return;
                if (!IsValidIP(value)) return;
                _ipAddr = value;
                this.RaisePropertyChanged(() => this.IpAddr);
            }
        }

        private string _ipAddrBak;


        public string IpAddrBak
        {
            get { return _ipAddrBak; }
            set
            {
                if (value == _ipAddrBak) return;
                if (!IsValidIP(value)) return;
                _ipAddrBak = value;
                this.RaisePropertyChanged(() => this.IpAddrBak);
            }
        }

      

        public bool IsValidIP(string ip)
        {
            try
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
                {
                    string[] ips = ip.Split('.');
                    if (ips.Length == 4 || ips.Length == 6)
                    {
                        if (System.Int32.Parse(ips[0]) < 256 &&
                            System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 &
                            System.Int32.Parse(ips[3]) < 256)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private int _ipPort;


        public int IpPort
        {
            get { return _ipPort; }
            set
            {
                if (value == _ipPort) return;
                _ipPort = value;
                this.RaisePropertyChanged(() => this.IpPort);
            }
        }

        private int _ipPortBak;


        public int IpPortBak
        {
            get { return _ipPortBak; }
            set
            {
                if (value == _ipPortBak) return;
                _ipPortBak = value;
                this.RaisePropertyChanged(() => this.IpPortBak);
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value == _userName) return;
                _userName = value;
                this.RaisePropertyChanged(() => this.UserName);
            }
        }

        private string _userPsw;

        public string UserPsw
        {
            get { return _userPsw; }
            set
            {
                if (value == _userPsw) return;
                _userPsw = value;
                this.RaisePropertyChanged(() => this.UserPsw);
            }
        }


        private int _serverStyle;

        public int ServerStyle
        {
            get { return _serverStyle; }
            set
            {
                if (value == _serverStyle) return;
                _serverStyle = value;
                this.RaisePropertyChanged(() => this.ServerStyle);
            }
        }

        private bool _serverMain;

        public bool ServerMain
        {
            get { return _serverMain; }
            set
            {
                if (value == _serverMain) return;
                _serverMain = value;
                this.RaisePropertyChanged(() => this.ServerMain);
                if (ServerMain) ServerBak = false;
            }
        }

        private bool _serverBak;

        public bool ServerBak
        {
            get { return _serverBak; }
            set
            {
                if (value == _serverBak) return;
                _serverBak = value;
                this.RaisePropertyChanged(() => this.ServerBak);
                if (ServerBak) ServerMain = false;
            }
        }


        #endregion

        #region CmdExit

        private ICommand _cmdExit;

        public ICommand CmdExit
        {
            get
            {
                if (_cmdExit == null) _cmdExit = new RelayCommand(ExCmdExit);
                return _cmdExit;
            }
        }

        private void ExCmdExit()
        {
            Environment.Exit(11);
        }

        #endregion

        #region CmdLogin

        private ICommand _cmdLogin;

        public ICommand CmdLogin
        {
            get
            {
                if (_cmdLogin == null) _cmdLogin = new RelayCommand(ExCmdLogin, CanLogin, false);
                return _cmdLogin;
            }
        }

        private bool _bolCanLogin;

        private bool CanLogin()
        {
            return _bolCanLogin;
        }


        private void ExCmdLogin()
        {

            if (string.IsNullOrEmpty(UserName))
            {
                Msg = "用户名为空...";
                return;
            }
            if (string.IsNullOrEmpty(UserPsw))
            {
                Msg = "密码为空..."; 
                return;
            }

            if (string.IsNullOrEmpty(IpAddr))
            {
                Msg = "Ip地址为空..."; 
                return;
            }

            if (ServerMain)
            {
                ServerStyle = 1;
                if (!Wlst.Sr.PPPandSocketSvr.Server.SocketClient.Connect(IpAddr, IpPort, 3))
                {
                    Msg = "连接服务器失败...<未联网或服务器未开启>";
                    return;
                }
                else
                {
                    Msg = "连接服务器成功，正在验证用户名密码...";
                }
                _bolCanLogin = false;
            }
            else if (ServerBak)
            {
                ServerStyle = 2;
                if (!Wlst.Sr.PPPandSocketSvr.Server.SocketClient.Connect(IpAddrBak, IpPortBak, 3))
                {
                    Msg = "连接服务器失败...<未联网或服务器未开启>";
                    return;
                }
                else
                {
                    Msg = "连接服务器成功，正在验证用户名密码...";
                }
                _bolCanLogin = false;
            }
            else
            {
                ServerStyle = 0;
                Msg = "请选择一个服务器连接方式...";
                return;
            }

            



          //  Wlst.Cr.Core.CoreServices.RegionManage.SetLoadInner();
            //更新本地数据库的用户名密码地址
            if (IpAddr == null)IpAddr="0.0.0.0";
            if (IpAddrBak == null) IpAddrBak = "0.0.0.0";

            UpdateKeyVauleDb("ipAddr", IpAddr.Trim());
            UpdateKeyVauleDb("ipPort", IpPort.ToString(CultureInfo.InvariantCulture));
            UpdateKeyVauleDb("userName", UserName);
            UpdateKeyVauleDb("ipAddrBak", IpAddrBak.Trim());
            UpdateKeyVauleDb("ipPortBak", IpPortBak.ToString(CultureInfo.InvariantCulture));
            UpdateKeyVauleDb("serverStyle", ServerStyle.ToString(CultureInfo.InvariantCulture));

            //在程序加载未显示设备列表时  屏蔽来自服务器端的各种显示信息 信息和选测信息
            var lst = new List<string>();
            lst.Add("wlst.msg .server_msg");
            lst.Add("wlst.rtu.2000");
            Wlst.Sr.PPPandSocketSvr.Server.ProtocolServices.ShieldCmd(true, lst);

            Thread.Sleep(900);
            //请求登录
            var xxxinfo = Wlst.Sr.ProtocolPhone .LxLogin  .wst_cnt_login   ;//.ServerPart.wlst_login_clinet_request_login;
            xxxinfo.WstLoginCntLogin  .UserName = UserName;
            xxxinfo.WstLoginCntLogin.UserPassword = UserPsw;

            string sttachinfo = "";

            if (File.Exists("C:\\Windows\\AdminLogin.txt")) //序列号LvfGay
            {
                StreamReader rd = File.OpenText("C:\\Windows\\AdminLogin.txt");
                sttachinfo = rd.ReadToEnd();
                xxxinfo.WstLoginCntLogin.AttachInfo = sttachinfo;
            }


            SndOrderServer.OrderSnd(xxxinfo, 3, 1);

            //线程检测登录
            try
            {
                if (thread.IsAlive) thread.Abort();
            }
            catch (Exception exx)
            {
                
            }
            thread = new Thread(ThreadRunForOutTime);
            thread.Start();
        }

       

        /// <summary>
        /// 点击登录后  更新界面函数
        /// </summary>
        private void ThreadRunForOutTime()
        {
            _bolCanLogin = false;
            int xtimes = 0;

            Msg = "等待服务器应答 ................";



            for (int i = LoginWaitTime; i > -1; i--)
            {
                if (Msg.Length > 1)
                    Msg = Msg.Substring(0, Msg.Length - 1);
                Thread.Sleep(1000);
                xtimes++;
            }
            Msg = "等待超时......"; // I36N.Services.I36N.ConvertByCodingOne("11000010", "等待超时......");

            _bolCanLogin = true;
        }

        //定义委托
        public delegate void DoTask();

        #endregion

        #region db Get && Update

        private void GetValueFromDb()
        {
            try
            {
                var ssss = Convert.ToInt32(SqlLiteHelper.ExecuteQuery(
                    "SELECT COUNT(*) as count FROM sqlite_master WHERE type='table' and name= 'login_setting'")
                                               .Tables[0].Rows[0][0].ToString().Trim());

                if (ssss < 1)
                {
                    SqlLiteHelper.ExecuteQuery(
                        "CREATE TABLE 'login_setting' ('key' text, 'value' text)");
                }

                DataSet ds = SqlLiteHelper.ExecuteQuery("select * from login_setting", null);
                if (ds == null) return;
                int mCount = ds.Tables[0].Rows.Count;
                for (int i = 0; i < mCount; i++)
                {
                    try
                    {
                        string key = ds.Tables[0].Rows[i]["key"].ToString().Trim();
                        string value = ds.Tables[0].Rows[i]["value"].ToString().Trim();
                        UpdateKeyVaule(key, value);
                    }
                    catch (Exception ex)
                    {
                        WriteLog.WriteLogError(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Class login GetValueFromDb has an Error:" + ex.ToString());
            }
        }


        private void UpdateKeyVaule(string key, string value)
        {
            try
            {
                if (key.Equals("ipAddr")) IpAddr = value.Trim();
                if (key.Equals("ipPort")) IpPort = Convert.ToInt32(value.Trim());
                if (key.Equals("userName")) UserName = value.Trim();
                if (key.Equals("ipAddrBak")) IpAddrBak = value.Trim();
                if (key.Equals("ipPortBak")) IpPortBak = Convert.ToInt32(value.Trim());
                if (key.Equals("serverStyle")) ServerStyle = Convert.ToInt32(value.Trim());
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }


        private void UpdateKeyVauleDb(string key, string value)
        {
            var ssss = Convert.ToInt32(SqlLiteHelper.ExecuteQuery(
                "SELECT COUNT(*) as count FROM login_setting WHERE key='" + key + "'").Tables[0].Rows[0][0].ToString().
                                           Trim());

            if (ssss < 1)
            {
                SqlLiteHelper.ExecuteQuery(
                    "insert into login_setting(key,value) values ('" + key + "','" + value + "')");
            }
            else
            {
                SqlLiteHelper.ExecuteQuery(
                    "update login_setting set value='" + value + "' where key='" + key + "'");
            }
        }

        #endregion


       
    }
    /// <summary>
    /// Action
    /// </summary>
    public partial class LoginViewModel
    {
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxLogin  .wst_svr_ans_cnt_login  ,//.ClientPart.wlst_login_server_ans_clinet_request_login,
                OnLogin,
                typeof(LoginViewModel), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxLogin  .wst_svr_force_cnt_login_exit  ,//.ClientPart.wlst_server_ans_clinet_login_out, 
                OnForceExit,
                typeof(LoginViewModel), this);


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxLogin .wst_cnt_relogin  ,//.ClientPart.wlst_login_server_request_clinet_relogin, 
                OnSvrReRelogin,
                typeof(LoginViewModel), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxLogin  .wst_svr_ans_cnt_relogin  ,//.ClientPart.wlst_login_server_clinet_request_relogin, 
                OnRelogin,
                typeof(LoginViewModel), this);
        }

        public void OnSvrReRelogin(string session,Wlst .mobile .MsgWithMobile  infos)
        {
            var info = Wlst.Sr.ProtocolPhone .LxLogin  .wst_cnt_relogin  ;//.ServerPart.wlst_login_clinet_request_relogin;
            info.WstLoginCntLogin.UserPassword  = UserPsw;
            info.WstLoginCntLogin.UserName = UserName;
            SndOrderServer.OrderSnd(info);
        }


        public void OnRelogin(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var t = infos.WstLoginSvrAnsCntLogin  ;
            if (t == null) return;

            if (t.IsLoginSucc )
            {
                Wlst.Cr.CoreMims.Services.KeepAliveSocket.Myself.StartHeatBeat();

                EventPublish.PublishEvent(new PublishEventArgs() {EventId = -1, EventType = PublishEventType.ReCn});

                Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo = t;
            }
        }

        public void OnForceExit(string session, Wlst.mobile.MsgWithMobile infos)
        {

            if (infos.WstLoginSvrTellCntLoginOut  == null) return;
            Wlst.Cr.CoreOne.Services.LogInfo.Log(infos.WstLoginSvrTellCntLoginOut.Why);
            Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(0,
                                                                  " 与服务器断开", OperatrType.SystemInfo,
                                                                  Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.
                                                                      UserName +
                                                                  " 已在其他电脑登陆.");

            WlstMessageBox.Show(Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName + " 异地登陆",
                                "系统已经停止对你的数据服务，" + Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.UserName +
                                " 已在其他电脑登陆.", WlstMessageBoxType.Ok);
            Wlst.Sr.PPPandSocketSvr.Server.SocketClient.Close();
        }

        public void OnLogin(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (_loginsuccessful) return;

            if (infos.WstLoginSvrAnsCntLogin  == null) return;
            try
            {
                _bolCanLogin = true;
                var t = infos.WstLoginSvrAnsCntLogin;

               
                try
                {
                    if (thread.IsAlive) thread.Abort();
                }
                catch (Exception exx)
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + exx);
                }

                #region 若该用户管辖区域已被删除，提示无法登陆

                

                //if (infos.WstSvrAnsCntLogin.SvrInfo == "无区域信息可读取，无法登陆")
                //    {
                //        _bolCanLogin = true;
                //        t.IsLoginSucc = false;
                //        Msg = "无区域信息可读取，无法登陆";                       
                //    }

                #endregion

                if (!t.IsLoginSucc )
                {
                    _bolCanLogin = true;

                    if (infos.WstLoginSvrAnsCntLogin.SvrInfo == "无区域信息可读取，无法登陆")
                    {
                        _bolCanLogin = true;
                        t.IsLoginSucc = false;
                        Msg = "无区域信息可读取，无法登陆!";
                    }
                    else if (t.SvrInfo == "user_name or Password wrong !!!!!!")
                    {
                        Msg = "登陆失败，用户名或密码错误.....";
                    }
                    else if (t.SvrInfo == "该设备为非授权设备，无法登陆")
                    {
                        Msg = "该设备为非授权设备，无法登陆!";
                    }
                    else
                    {
                        Msg = "登陆失败";
                    }
                        
                   
                    
                    return;

                }



                _bolCanLogin = false;
                Msg = "登陆成功!!!"; 
                //UserInfomation UserLoginInfo = new UserInfomation();
                //UserLoginInfo.Department = t.UserInfo .Department;
                //UserLoginInfo.PhoneNumber = t.UserInfo.PhoneNumber;
                //UserLoginInfo.Position = t.UserInfo.Position;
                //UserLoginInfo.UserName = t.UserInfo.UserName;
                //UserLoginInfo.UserPasswrod = t.UserInfo.UserPasswrod;
                //UserLoginInfo.Rwxd  = t.UserInfo.Rwxd ;
                //UserLoginInfo.UserRealName = t.UserInfo.UserRealName;
                //UserLoginInfo.IsInFullSetMod = t.UserInfo.IsInFullSetMod;

                Wlst.Cr.CoreMims.Services.KeepAliveSocket.Myself.StartHeatBeat();

                Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo = t;
                _loginsuccessful = true;

                RunLoad();


            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
            return;

        }

    }

    /// <summary>
    /// Login Succ
    /// </summary>
    public partial class LoginViewModel
    {

        public event EventHandler OnLoginSuccess = null;


        private void RunLoad()
        {

            Application.Current.Dispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Normal, new DoTask(ShowMain));

            Msg = "登陆成功 !!!"; // I36N.Services.I36N.ConvertByCodingOne("11000012", "登陆成功！！！");

        }
        private void ShowMain()
        {

            if (OnLoginSuccess != null)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("Preparing MainWindow....");
            

                //Thread .Sleep(5000);
                OnLoginSuccess(this, null);
              
                Elysium.Manager.Apply(Application.Current, Elysium.Theme.Light);
                System.Windows.Application.Current.MainWindow.WindowState = WindowState.Maximized;
                System.Windows.Application.Current.MainWindow.Visibility = Visibility.Visible;  
              //  Wlst.Cr.Core.CoreServices.RegionManage.RunningLoadViewToWindows();
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogInfo("Prepared MainWindow....");
                Wlst.Cr.Core.ModuleServices.DelayEvent.RaiseEventHappen(DelayEventHappen.EventSvAc);
            }
        }

    }
}