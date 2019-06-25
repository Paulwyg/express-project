using System;
using System.Globalization;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.Reflection;
using System.Threading;
using System.Windows;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.TopDataInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.StateBarModule.StateBarInBottom.Services;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Views;
using LogInfo = Wlst.Cr.CoreOne.Services.LogInfo;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.StateBarModule.StateBarInBottom.ViewModel
{
    [Export(typeof (IIStateBarViewModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class StateBarViewModule : Wlst.Cr.Core.CoreServices.ObservableObject, IIStateBarViewModule
    {


       // private Thread timerServer;


        public StateBarViewModule()
        {
            InitAction();
            InitEvent();

            isMidConnormal = true;
            isDbConnnormal = true;

            ConName = "连接正常";
            ConColor = ColorNormal;

            RtusTotals = "---";
            UsedRtus = "---";
            OnLineRuts = "---";
            ErrNums = "0";

            LoadLuxXmldata();
            var tmp = LoadXmldata();
            this.IsShowNewErrArriveOnUi = tmp?Visibility.Visible : Visibility.Collapsed;//lvf
            ShowWarning = IsShowWarning ? Visibility.Visible : Visibility.Collapsed;
            LuxWarning = "";

            IsLuxVisi = Visibility.Collapsed;
            

            Wlst.Cr.Coreb.Servers .QtzLp .AddQtz("null", 8888, DateTime.Now.Ticks , 1, UpdateTime);

            ////timerServer = new Thread(TimerServer);
            ////timerServer.Start();
        }


         void UpdateTime(object obj)
         {
             try
             {


                 if (day != DateTime.Now.Day)
                 {
                     day = DateTime.Now.Day;
                     UpdateTOdaytime();
                 }
                 if (updatesucc == false) UpdateTOdaytime();
                 TimeNowHour = DateTime.Now.ToString("yyyy-MM-dd");
                // TimeNowMinute = DateTime.Now.ToString("HH:mm:ss");


                 if (Wlst.Cr.SuperSocketSvrCnt.Services.SuperSocketClnt.IsConnected == false)
                 {
                     ConName = "连接断开";
                     ChnageColor();

                 }
                 else
                 {

                     if (isMidConnormal == false)
                     {
                         ConName = "通信层断开";
                         ChnageColor();
                     }
                     else if (isDbConnnormal == false)
                     {
                         ConName = "数据库断开";
                         ChnageColor();
                     }
                     else
                     {
                         ConName = "连接正常";
                         ConColor = ColorNormal;
                     }
                 }

                 var infox = TopDataInfoServers.MySelf.GetDataInfo(1);
                 var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month, DateTime.Now.Day);
                 if (infox != null)
                 {
                     try
                     {
                         var sps = infox.Item1.Split(':', '：');
                         if (sps.Length > 1)
                         {
                             double x = 0;
                             if (Double.TryParse(sps[1], out x))
                             {
                                 IsLuxVisi = Visibility.Visible;
                                 LuxValuex = x + "";
                                 LuxTooltips = infox.Item2;


                                 if(x<AlarmValue)
                                 {
                                     if (!IsShowWarning) return;
                                     if (LuxWarning == "光照度值低" && IsShowWarning) return;
                                     int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour * 60;

                                     if (_dtNow <= info.time_sunset -OffSet  && _dtNow >= info.time_sunrise+OffSet)
                                    {
                                        LuxWarning = "光照度值低";   
                                    }
                                 }else
                                 {
                                        LuxWarning = "";
                                 }


                             }

                         }
                     }
                     catch (Exception ex)
                     {

                     }
                 }
             }
             catch (Exception ex)
             {

             }
         }

        private int day = 0;
        private bool updatesucc = false;
        public const string XmlConfigName = "SystemCommonSetConfg";
        public bool IsShowWarning;
        public int StTime;
        public int EndTime;
        public int AlarmValue;
        public int OffSet;

        private bool LoadXmldata()
        {
            int x = 0 ;
            string finename = "";

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
           
            if (info.ContainsKey("IsShowNewErrArriveOnUi"))
            {
                try
                {
                    x = Convert.ToInt32(info["IsShowNewErrArriveOnUi"]);
                }
                catch (Exception ex)
                {
                }
            }
            

            return  x==1;

        }
        private void  LoadLuxXmldata()
        {
            int x = 0;
            string finename = "";
            string name;
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("Wj1080SetConfg");

            if (info.TryGetValue("AlarmValue", out name))
            {
                AlarmValue = int.Parse(name.Trim());
            }
            else AlarmValue = 0;

            if (info.TryGetValue("IsShowWarning", out name))
            {
                IsShowWarning = info["IsShowWarning"].Contains("yes");
            }
            else IsShowWarning = false;

            //if (info.TryGetValue("StTime", out name))
            //{
            //    StTime = int.Parse(name.Trim());
            //}
            //else StTime = 0;

            //if (info.TryGetValue("EndTime", out name))
            //{
            //    EndTime = int.Parse(name.Trim());
            //}
            //else EndTime = 0;


            if (info.TryGetValue("OffSet", out name))
            {
                OffSet  = int.Parse(name.Trim());
            }
            else OffSet  = 0;

        }
 
        private void TimerServer()
        {
            while (true)
            {
                try
                {
                   

                    if (day != DateTime.Now.Day)
                    {
                        day = DateTime.Now.Day;
                        UpdateTOdaytime();
                    }
                    if (updatesucc == false) UpdateTOdaytime();
                    TimeNowHour = DateTime.Now.ToString("yyyy-MM-dd");
                    TimeNowMinute = DateTime.Now.ToString("HH:mm:ss");


                    if (Wlst.Cr.SuperSocketSvrCnt.Services.SuperSocketClnt.IsConnected == false)
                    {
                        ConName = "连接断开";
                        ChnageColor();

                    }
                    else
                    {

                        if (isMidConnormal == false)
                        {
                            ConName = "通信层断开";
                            ChnageColor();
                        }
                        else if (isDbConnnormal == false)
                        {
                            ConName = "数据库断开";
                            ChnageColor();
                        }
                        else
                        {
                            ConName = "连接正常";
                            ConColor = ColorNormal;
                        }
                    }

                    var infox = TopDataInfoServers.MySelf.GetDataInfo(1);
                    if (infox != null)
                    {
                        try
                        {
                            var sps = infox.Item1.Split(':', '：');
                            if (sps.Length > 1)
                            {
                                double x = 0;
                                if (Double.TryParse(sps[1], out x))
                                {
                                    IsLuxVisi = Visibility.Visible;
                                    LuxValuex = x + "";
                                    LuxTooltips = infox.Item2;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                }
                Thread.Sleep(1000);
            }
        }

        private void ChnageColor()
        {
            if (ConColor == ColorNormal || ConColor == ColorUnNormal1)
                ConColor = ColorUnNormal;
            else if (ConColor == ColorNormal || ConColor == ColorUnNormal)
                ConColor = ColorUnNormal1;
        }



        /// <summary>
        /// 更新日出日落显示时间
        /// </summary>
        private void UpdateTOdaytime()
        {

            var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(DateTime.Now.Month,
                                                                                                  DateTime.Now.Day);

            if (info == null)
            {
                updatesucc = false;

                TimeSunRaise = "无";
                TimeSunSet = "无";

                return;
            }
            updatesucc = true;
            TimeSunRaise = (info.time_sunrise / 60).ToString("D2") + ":" + (info.time_sunrise % 60).ToString("D2");
            TimeSunSet = (info.time_sunset / 60).ToString("D2") + ":" + (info.time_sunset % 60).ToString("D2");

        }

        #region 时钟以及日出日落时间更新

        private string _timeNowhour;

        public string TimeNowHour
        {
            get { return _timeNowhour; }
            set
            {
                if (_timeNowhour == value) return;
                _timeNowhour = value;
                this.RaisePropertyChanged(() => this.TimeNowHour);
            }
        }

        private string _timeTimeNowMinuteNowhour;

        public string TimeNowMinute
        {
            get { return _timeTimeNowMinuteNowhour; }
            set
            {
                if (_timeTimeNowMinuteNowhour == value) return;
                _timeTimeNowMinuteNowhour = value;
                this.RaisePropertyChanged(() => this.TimeNowMinute);
            }
        }

        private string _sunraise;

        public string TimeSunRaise
        {
            get { return _sunraise; }
            set
            {
                if (_sunraise == value) return;
                _sunraise = value;
                this.RaisePropertyChanged(() => this.TimeSunRaise);
            }
        }

        private string _sunrset;

        public string TimeSunSet
        {
            get { return _sunrset; }
            set
            {
                if (_sunrset == value) return;
                _sunrset = value;
                this.RaisePropertyChanged(() => this.TimeSunSet);
            }
        }

        #endregion



        private const string ColorNormal = "Green";
        private const string ColorUnNormal = "Red";
        private const string ColorUnNormal1 = "DarkOrange";
        private bool isDbConnnormal = true;
        private bool isMidConnormal = true;

        #region 数据库连接状态以及终端在线信息

        private string _onLineRuts;

        public string OnLineRuts
        {
            get { return _onLineRuts; }
            set
            {
                if (_onLineRuts == value) return;
                _onLineRuts = value;
                this.RaisePropertyChanged(() => this.OnLineRuts);
            }
        }

        private string _tUsedRtus;

        public string UsedRtus
        {
            get { return _tUsedRtus; }
            set
            {
                if (_tUsedRtus == value) return;
                _tUsedRtus = value;
                this.RaisePropertyChanged(() => this.UsedRtus);
            }
        }

        private string _sRtusTotals;

        public string RtusTotals
        {
            get { return _sRtusTotals; }
            set
            {
                if (_sRtusTotals == value) return;
                _sRtusTotals = value;
                this.RaisePropertyChanged(() => this.RtusTotals);
            }
        }


        private string _dbConnColor;

        public string ConColor
        {
            get { return _dbConnColor; }
            set
            {
                if (_dbConnColor == value) return;
                _dbConnColor = value;
                this.RaisePropertyChanged(() => this.ConColor);
            }
        }


        //private string _ddsfbConnColor;

        //public string MidConColor
        //{
        //    get { return _ddsfbConnColor; }
        //    set
        //    {
        //        if (_ddsfbConnColor == value) return;
        //        _ddsfbConnColor = value;
        //        this.RaisePropertyChanged(() => this.MidConColor);
        //    }
        //}


        private string _dbConnDbConName;

        public string ConName
        {
            get { return _dbConnDbConName; }
            set
            {
                if (_dbConnDbConName == value) return;
                _dbConnDbConName = value;
                this.RaisePropertyChanged(() => this.ConName);
            }
        }


        //private string _ddsfbConnName;

        //public string MidConName
        //{
        //    get { return _ddsfbConnName; }
        //    set
        //    {
        //        if (_ddsfbConnName == value) return;
        //        _ddsfbConnName = value;
        //        this.RaisePropertyChanged(() => this.MidConName);
        //    }
        //}

        private Visibility _isShowNewErrArriveOnUi;

        public Visibility IsShowNewErrArriveOnUi
        {
            get { return _isShowNewErrArriveOnUi; }
            set
            {
                if (_isShowNewErrArriveOnUi == value) return;
                _isShowNewErrArriveOnUi = value;
                this.RaisePropertyChanged(() => this.IsShowNewErrArriveOnUi);
            }
        }

        private string _errNums;

        public string ErrNums
        {
            get { return _errNums; }
            set
            {
                if (_errNums == value) return;
                _errNums = value;
                this.RaisePropertyChanged(() => this.ErrNums);
            }
        }
        #endregion



        private string _onLineRxuts;

        public string LuxValuex
        {
            get { return _onLineRxuts; }
            set
            {
                if (_onLineRxuts == value) return;
                _onLineRxuts = value;
                this.RaisePropertyChanged(() => this.LuxValuex);
            }
        }


        private string _luxWarning;

        public string LuxWarning
        {
            get { return _luxWarning; }
            set
            {
                if (_luxWarning == value) return;
                _luxWarning = value;
                this.RaisePropertyChanged(() => this.LuxWarning);

            }
        }


        private string _onLinsdfsdeRxuts;

        public string LuxTooltips
        {
            get { return _onLinsdfsdeRxuts; }
            set
            {
                if (_onLinsdfsdeRxuts == value) return;
                _onLinsdfsdeRxuts = value;
                this.RaisePropertyChanged(() => this.LuxTooltips);
            }
        }

        private Visibility sdfIsLuxVisi;

        public Visibility IsLuxVisi
        {
            get { return sdfIsLuxVisi; }
            set
            {
                if (value == sdfIsLuxVisi) return;
                sdfIsLuxVisi = value;
                this.RaisePropertyChanged(() => this.IsLuxVisi);
            }
        }

        private Visibility _isShowWarning;

        public Visibility ShowWarning
        {
            get { return _isShowWarning; }
            set
            {
                if (value == _isShowWarning) return;
                _isShowWarning = value;
                this.RaisePropertyChanged(() => this.ShowWarning);
            }
        }

        private string _onLtipss;

        public string ConTitps
        {
            get { return _onLtipss; }
            set
            {
                if (_onLtipss == value) return;
                _onLtipss = value;
                this.RaisePropertyChanged(() => this.ConTitps);
            }
        }
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    public partial class StateBarViewModule
    {
        private void InitAction()
        {

            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxSys .wlst_sys_connect_state ,
                //.ClientPart.wlst_Measures_server_ans_clinet_request_SystemConnectStates,
                GetSystemConnectStates,
                typeof (StateBarViewModule), this);

        }


        public static bool IsUserInLimitLogin = false;

        private void GetSystemConnectStates(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstSysConnecteState ;
            if (info == null) return;
            

                OnLineRuts = info.OnLineRtus + "";
                UsedRtus = info.TotalUsedRtus + "";
                RtusTotals = info.TotalRtus + "";

            isMidConnormal = info.IsMiddleConnectToCommucationLayer;
            if (infos.Args.Addr != null && infos.Args.Addr.Count > 1)
            {
                if (info.IsMiddleConnectToCommucationLayer)
                {
                    isDbConnnormal = infos.Args.Addr[0] == 1 && infos.Args.Addr[1] == 1;

                    if (infos.Args.Addr.Count > 2)
                        if (infos.Args.Addr[2] == 0)
                        {
                            IsUserInLimitLogin = true;
                        }
                        else
                        {
                            IsUserInLimitLogin = false;
                        }
                }
            }

            ConTitps = "通信时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " +
                       (IsUserInLimitLogin ? "非完全登陆模式" : "");

        }

        private void InitEvent()
        {
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

        }

        #region EventSubScriptionTokener

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {
            try
            {
                if (args.EventType == PublishEventType.Core)
                {


                    if (args.EventId == 3103605)//EventId =EventIdAssign.PushErrNums
                    {
                        var errnums = args.GetParams()[0].ToString();
                        this.ErrNums = errnums;
                    }
                }
                //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                //{
                //    var infos = args.GetParams()[1] as EquipmentPreFaultExChange;
                //    if (infos == null) return;
                //    OnPreDataBack(infos);
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "EquipmentDataQuery.EquipmentFaultRecordQueryViewModel FundEventHandler occer an error:" +
                    ex);
            }

        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            // if (!IsSingleEquipmentQuery) return false;
            try
            {
                if (args.EventType == PublishEventType.Core)
                {

                    if (args.EventId == 3103605)//EventId =EventIdAssign.PushErrNums
                    {
                        return true;
                    }

                }
                //if (args.EventType == PublishEventType.Sevr &&
                //    args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                //{
                //    return true;
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

  
        #endregion

        public  void ClearErrNum()
        {
            //if (Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowThisViewOnNewErrArriveInfo) //选项中设定
            //{
                //Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegionWithArgu(
                //    1103603, -2); //故障查询界面
            //}
            Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.NavToCurrentEquipmentFaultView.ShowView();


            //this.ErrNums = "0";
        }

    }

}