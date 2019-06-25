using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.TopDataInfo;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Ux.StateBarModule.StateBarInBottom.Services;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Views;
using Wlst.Ux.TimeTableSystem.TimeInfoMn.ViewModel;
using LogInfo = Wlst.Cr.CoreOne.Services.LogInfo;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Ux.StateBarModule.StateBarInBottom.ViewModel
{
    [Export(typeof (IIStateBarViewModule))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class StateBarViewModule : Wlst.Cr.Core.CoreServices.ObservableObject, IIStateBarViewModule
    {


        //// private Thread timerServer;
        //public const int SND_ALIAS = 0x10000;
        //public const int SND_ALIAS_ID = 0x110000;
        //public const int SND_ALIAS_START = 0;
        //public const int SND_APPLICATION = 0x80;
        //public const int SND_ASYNC = 0x1;
        //public const int SND_FILENAME = 0x20000;
        //public const int SND_LOOP = 0x8;
        //public const int SND_MEMORY = 0x4;
        //public const int SND_NODEFAULT = 0x2;
        //public const int SND_NOSTOP = 0x10;
        //public const int SND_NOWAIT = 0x2000;
        //public const int SND_PURGE = 0x40;
        //public const int SND_RESERVED = unchecked((int)0xFF000000);
        //public const int SND_RESOURCE = 0x40004;
        //public const int SND_SYNC = 0x0;
        //public const int SND_TYPE_MASK = 0x170007;
        //public const int SND_VALID = 0x1F;
        //public const int SND_VALIDFLAGS = 0x17201F;

        //[DllImport("winmm")]
        //public static extern int PlaySound(string lpszName, IntPtr hModule, int dwFlags);

        //public static int PlaySoundFile(string _SoundFilePath, bool _bSynch, bool _bLoop)
        //{
        //    if (_SoundFilePath != null)
        //    {
        //        if (!System.IO.File.Exists(_SoundFilePath))
        //        {
        //            return 0;
        //        }
        //    }

        //    int Flags;

        //    if (_bSynch)
        //    {
        //        Flags = SND_FILENAME | SND_SYNC;
        //    }
        //    else
        //    {
        //        Flags = SND_FILENAME | SND_ASYNC;
        //    }

        //    if (_bLoop)
        //    {
        //        Flags |= SND_LOOP;
        //    }

        //    Flags |= SND_NOSTOP;

        //    return PlaySound(_SoundFilePath, IntPtr.Zero, Flags);
        //}

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
            EmergencyNums = "0";

           // LoadLuxXmldata();
            var tmp = LoadXmldata();
            this.IsShowNewErrArriveOnUi = tmp?Visibility.Visible : Visibility.Collapsed;//lvf


            //ctiynum todo
            this.IsShowEmergencyNum= Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum == 1 ? Visibility.Visible : Visibility.Collapsed;


            //ShowWarning = IsShowWarning ? Visibility.Visible : Visibility.Collapsed;
            LuxWarning = "";

            //_lngNextPartolTime = 0;
            _intPartolTime = 0;
            IsLuxVisi = Visibility.Collapsed;

           //LoadLuxXmldata();

            GetTimeTableParameter();
            if (LuxEffective == int.MinValue)
            {
                LuxEffective = 60;
                LightOnOffSet = 0;
                LightOffOffSet = 0;
            }

            th = new Thread(UpdateTime);
            th.Start();

            //Wlst.Cr.Coreb.Servers .QtzLp .AddQtz("null", 8888, DateTime.Now.Ticks , 1, UpdateTime);

            ////timerServer = new Thread(TimerServer);
            ////timerServer.Start();
            
        }

        private Thread th;

        private ObservableCollection<TimeTableInfomationItem> _items;

        public ObservableCollection<TimeTableInfomationItem> Items
        {
            get { return _items ?? (_items = new ObservableCollection<TimeTableInfomationItem>()); }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => Items);
            }
        }

        private void GetTimeTableParameter()
        {
                Items.Clear();

                foreach (var itemTable in WeekTimeTableInfoService.GeteekTimeTableInfoList(0))
                {
                    Items.Add(new TimeTableInfomationItem(itemTable, 0));
                }

                LuxEffective = int.MinValue;
                foreach (var tt in Items)
                {
                    if (tt.LuxId == CurrentSelectLux)
                    {
                        bool flg = false;

                        foreach (var t in tt.RuleItems)
                        {
                            if (t.IsUsedLuxOn && t.IsUsedLuxOff)
                            {
                                flg = true;
                            }
                            else
                            {
                                flg = false; 
                            }
                        }

                        if (!flg) continue;

                        LuxEffective = tt.LuxEffective;
                        LightOnOffSet = tt.LightOnOffset;
                        LightOffOffSet = tt.LightOffOffset;
                        LuxOnValue = tt.LuxOnValue;
                        LuxOffValue = tt.LuxOffValue;
                        UpdateTOdaytime();
                        break;

                        //int _index = 0;

                        //for (int j = 0; j < Items[i].RuleItems.Count; j++)
                        //{
                        //    if ((Items[i].RuleItems[j].DateDay == DateTime.Now.Day) &&
                        //        (Items[i].RuleItems[j].DateMonth == DateTime.Now.Month))
                        //    {
                        //        _index = j;
                        //    }
                        //}

                        //if ((Items[i].MainRuleItems[_index].MainTimeOnOne.Contains("光") == true) &&
                        //    (Items[i].MainRuleItems[_index].MainTimeOffOne.Contains("光") == true))
                        //{
                        //    LuxEffective = Items[i].LuxEffective;
                        //    LightOnOffSet = Items[i].LightOnOffset;
                        //    LightOffOffSet = Items[i].LightOffOffset;
                        //    LuxOnValue = Items[i].LuxOnValue;
                        //    LuxOffValue = Items[i].LuxOffValue;
                        //    break;
                        //}
                    }
                }
            
        }

        private bool Is1080ShowTopRight = false;

        private bool blTurnOnFirst = false;
        private bool blTurnOffFirst = false;
        private bool blLuxLowHighFirst = false;
        private bool blNeedGetPartolTime = true;

         void UpdateTime1()
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
                    ConName = "通信断开";
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
             
         }

        //private MediaPlayer player = null;
        private int tst = 0;
        //光控启用语音 提示
        void UpdateTime2()
        {

//            GetTimeTableParameter();
           //LoadLuxXmldata();

            Is1080ShowTopRight = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 1, false);
            CurrentSelectLux = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 2, 0);

            IsShowWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 3, false);
            ShowWarning = IsShowWarning ? Visibility.Visible : Visibility.Collapsed;

            //IsSunRiseSpeechWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 4, false);
            IsSunSetSpeechWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 5, false);

            //SunRiseAlarmValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 6, 0);
            //SunRiseOffSet = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 7, 0);

            SunSetAlarmValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 8, 0);
            //SunSetOffSet = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 9, 0);

            IsTrunOnWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 10, false);
            IsTrunOffWarning = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 11, false);

            IsShowOpenClose = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2101, 12, false);

            //SunOpenValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 13, 0);
            //SunCloseValue = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 14, 0);

            SunBefore = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 15, 0);
            TimeBefore = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2101, 16, 0);

            //if (DateTime.Now.Hour == 14 && DateTime.Now.Minute == 0 && DateTime.Now.Second == 0)
            //{
            //    GetTimeTableParameter();
            //    if (LuxEffective == int.MinValue)
            //    {
            //        LuxEffective = 60;
            //        LightOnOffSet = 0;
            //        LightOffOffSet = 0;
            //    }
            //}


            var infox = TopDataInfoServers.MySelf.GetDataInfo(1);

            if (infox != null)
            {
                try
                {

                    var sps = infox.Item1.Split(':', '：');
                    if (sps.Length > 1)
                    {

                       


                        string LuxInfo = infox.Item2;

                        string[] sp = new string[] { "\r\n" };

                        string[] spstring = LuxInfo.Split(sp, StringSplitOptions.None);
                        if (CurrentSelectLux == 0)
                        {

                            try
                            {

                                if (sps.Length > 1)
                                {
                                    //    double x = 0;
                                    if (Double.TryParse(sps[1], out LuxValue))
                                    {
                                        IsLuxVisi = Visibility.Visible;
                                        LuxValuex = LuxValue + "";
                                        LuxTooltips = infox.Item2;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        else
                        {
                            if (
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(
                                    CurrentSelectLux))
                            {
                                var tm =
                                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                                        CurrentSelectLux] as Wlst.Sr.EquipmentInfoHolding.Model.Wj1080Lux;

                                for (int i = 0; i < spstring.Length; i++)
                                {
                                    if (spstring[i].Contains("物理地址") == true)
                                    {
                                        var sps1 = spstring[i].Split(':', '：');

                                        if (Convert.ToInt32(sps1[1]) == tm.RtuPhyId)
                                        {
                                            var sps2 = spstring[i + 2].Split(':', '：', ' ', '-');

                                            for (int j = 0; j < sps2.Length; j++)
                                            {
                                                if (sps2[j] != "")
                                                {
                                                    if (sps2[j].Contains("光照度") == false)
                                                    {
                                                        LuxValue = Convert.ToDouble(sps2[j]);
                                                        break;
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        if (Is1080ShowTopRight)
                            IsLuxVisi = Visibility.Collapsed;
                        else
                            IsLuxVisi = Visibility.Visible;
                        LuxValuex = LuxValue + "";
                        LuxTooltips = infox.Item2;

                        if (IsShowWarning == true)
                        {
                            ////tst = tst + 1;
                            ////if (tst >= 10)
                            ////{
                            ////    if (player == null) player = new MediaPlayer();
                            ////    player.Stop();
                            ////    player.Open(new Uri(Directory.GetCurrentDirectory() + "\\AlarmSound\\LuxLowAlarm.WAV", UriKind.Relative));
                            ////    player.Play();
                            ////    tst = 0;
                            ////}

                            //GetTimeTableParameter();
                            //if (LuxEffective == int.MinValue)
                            //{
                            //    LuxEffective = 60;
                            //    LightOnOffSet = 0;
                            //    LightOffOffSet = 0;
                            //}

                            var info =
                                Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                DateTime.Now.Month, DateTime.Now.Day);
                            //if (LuxEffective != int.MinValue)
                            //{
                            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour * 60;

                            if ((_dtNow <= info.time_sunset + LightOnOffSet - LuxEffective) &&
                                (_dtNow >= info.time_sunrise + LightOffOffSet + LuxEffective))
                            {
                                if (LuxValue <= SunSetAlarmValue)
                                {
                                    if (Is1080ShowTopRight)
                                        LuxWarning = "";
                                    else
                                        LuxWarning = "光照度低";

                                    if (IsSunSetSpeechWarning)
                                    {
                                        if (blLuxLowHighFirst == false)
                                        {
                                            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\AlarmSound"))
                                            {
                                                if (File.Exists(Directory.GetCurrentDirectory() + "\\AlarmSound\\LuxLowAlarm.WAV"))
                                                {
                                                    try
                                                    {
                                                        //PlaySoundFile(
                                                        //    Directory.GetCurrentDirectory() + "\\AlarmSound\\LuxLowAlarm.WAV",
                                                        //    false, false);

                                                        //MediaPlayer player = new MediaPlayer();
                                                        MediaPlayer player = new MediaPlayer();
                                                        player.Stop();
                                                        player.Open(new Uri(Directory.GetCurrentDirectory() + "\\AlarmSound\\LuxLowAlarm.WAV", UriKind.Relative));
                                                        player.Play();

                                                        blLuxLowHighFirst = true;

                                                    }
                                                    catch (Exception)
                                                    {
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    blLuxLowHighFirst = false;
                                    LuxWarning = "";
                                }
                            }
                            else
                            {
                                LuxWarning = "";
                            }
                        }
                        else
                        {
                            LuxWarning = "";
                        }



                        if (IsShowOpenClose)
                        {

                            //GetTimeTableParameter();
                            //if (LuxEffective == int.MinValue)
                            //{
                            //    LuxEffective = 60;
                            //    LightOnOffSet = 0;
                            //    LightOffOffSet = 0;
                            //}
                            var info =
                                 Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                     DateTime.Now.Month, DateTime.Now.Day);
                            int _dtNow = DateTime.Now.Minute + DateTime.Now.Hour * 60;


                            //if (LuxEffective != int.MinValue)
                            //{
                            if ((_dtNow >= info.time_sunset + LightOnOffSet - LuxEffective) &&
                                (_dtNow <= info.time_sunset + LightOnOffSet))
                            {
                                if (LuxValue <= LuxOnValue + SunBefore || _dtNow == info.time_sunset + LightOnOffSet - TimeBefore)
                                {

                                    if (IsTrunOnWarning)
                                    {

                                        if (blTurnOnFirst == false)
                                        {
                                            WriteLog.WriteLogInfo("开始读取wav文件");
                                            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\AlarmSound"))
                                            {
                                                WriteLog.WriteLogInfo("找到AlarmSound文件夹");
                                                var path = Directory.GetCurrentDirectory() + "\\AlarmSound\\TurnOn.WAV";
                                                if (File.Exists(path))
                                                {
                                                    WriteLog.WriteLogInfo("找到TurnOn.WAV文件");
                                                    try
                                                    {
                                                        //PlaySoundFile(
                                                        //    Directory.GetCurrentDirectory() + "\\AlarmSound\\TurnOn.WAV",
                                                        //    false, false);

                                                        //MediaPlayer player = new MediaPlayer();
                                                        MediaPlayer player = new MediaPlayer();
                                                        player.Stop();
                                                        player.Open(new Uri(Directory.GetCurrentDirectory() + "\\AlarmSound\\TurnOn.WAV", UriKind.Relative));
                                                        player.Play();

                                                        blTurnOnFirst = true;
                                                        WriteLog.WriteLogInfo("成功播报");
                                                    }
                                                    catch (Exception)
                                                    {
                                                    }
                                                }
                                                else
                                                {
                                                    WriteLog.WriteLogInfo("未找到文件");
                                                }

                                            }
                                            else
                                            {
                                                WriteLog.WriteLogInfo("未找到文件夹路径");
                                            }

                                        }
                                    }
                                }
                                
                                //}
                            }
                            else
                            {
                                blTurnOnFirst = false;
                                WriteLog.WriteLogInfo("超过了提醒时间段，重置提醒标识符，待第二天报警");
                            }


                            if ((_dtNow >= info.time_sunrise + LightOffOffSet - LuxEffective) &&
                                    (_dtNow <= info.time_sunrise + LightOffOffSet))
                            {
                                if (LuxValue >= LuxOffValue - SunBefore || _dtNow == info.time_sunrise + LightOffOffSet - TimeBefore)
                                {
                                    if (IsTrunOffWarning == true)
                                    {
                                        if (blTurnOffFirst == false)
                                        {
                                            //blTurnOffFirst = true;
                                            WriteLog.WriteLogInfo("开始读取wav文件");

                                            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\AlarmSound"))
                                            {
                                                WriteLog.WriteLogInfo("找到AlarmSound文件夹");
                                                var path = Directory.GetCurrentDirectory() + "\\AlarmSound\\TurnOff.WAV";
                                                if (File.Exists(path))
                                                {
                                                    WriteLog.WriteLogInfo("找到TurnOff.WAV文件");
                                                    try
                                                    {
                                                        //PlaySoundFile(
                                                        //    Directory.GetCurrentDirectory() + "\\AlarmSound\\TurnOff.WAV",
                                                        //    false, false);

                                                        //MediaPlayer player = new MediaPlayer();
                                                        MediaPlayer player = new MediaPlayer();
                                                        player.Stop();
                                                        player.Open(new Uri(Directory.GetCurrentDirectory() + "\\AlarmSound\\TurnOff.WAV", UriKind.Relative));
                                                        player.Play();

                                                        blTurnOffFirst = true;
                                                        WriteLog.WriteLogInfo("成功播报");
                                                    }
                                                    catch (Exception)
                                                    {
                                                    }
                                                }
                                                else
                                                {
                                                    WriteLog.WriteLogInfo("未找到wav文件");
                                                }
                                            }
                                            else
                                            {
                                                WriteLog.WriteLogInfo("未找到文件夹路径");
                                            }
                                        }
                                    }
                                }
                                
                            }
                            else
                            {
                                blTurnOffFirst = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex = ex ;
                }
            }

        }

        void UpdateTime3()
        {

            TimeSpan ts = _dtNextPartolTime - DateTime.Now;
            
            if(ts.Ticks<0)//需要读取服务器下次巡测时间
            {
                _intPartolTime++;
                TimeSpan tss = DateTime.Now - _dtNextPartolTime;  //时差
                if (tss.TotalSeconds > 60 && _intPartolTime>5)
                {
                    ReadNextPartolTime();
                    _intPartolTime = 0;
                }


                return;
            }
            if (ts.Ticks == 0) NextPartolTime = "正在巡测";


            if (ts.Ticks > 0)
            {
                NextPartolTime = new DateTime(ts.Ticks).ToString("HH:mm:ss");
            }
            //else
            //{
            //    NextPartolTime = "正在巡测";
            //    TimeSpan tss = DateTime.Now - _dtNextPartolTime;

            //    if (tss.TotalSeconds > 30)
            //    {
            //        _intPartolTime++;
            //        if (_intPartolTime % 3 == 0) ReadNextPartolTime();

            //    }
            //}
        }

        void UpdateTime(object obj)
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(1000);
                    UpdateTime1(); //更新界面时钟

                    UpdateTime2(); //光控 启用语音提示 要开关灯了
                    UpdateTime3(); //提示 距离下次巡测时间
                }
                catch (Exception ex)
                {

                }
            }
        }






        private int day = 0;
        private bool updatesucc = false;
        public const string XmlConfigName = "SystemCommonSetConfg";

        public bool IsShowWarning;
        //public int SunRiseAlarmValue;
        public int SunSetAlarmValue;
        //public int SunRiseOffSet;
        //public int SunSetOffSet;
        //public bool IsSunRiseSpeechWarning;
        public bool IsSunSetSpeechWarning;

        public bool IsTrunOnWarning;
        public bool IsTrunOffWarning;

        public int CurrentSelectLux;

        public bool IsShowOpenClose;
        public int SunOpenValue;
        public int SunCloseValue;

        public int SunBefore;
        public int TimeBefore;

        public int LuxEffective;
        public int LightOnOffSet;
        public int LightOffOffSet;

        public int LuxOnValue;
        public int LuxOffValue;

        public double LuxValue;

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


            //西安 特殊功能   城市代号为5 lvf 2018年4月12日13:07:15
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum == 5)
            {
                int timeTurnOff = info.time_sunrise + LightOffOffSet;
                int timeTurnOn = info.time_sunset + LightOnOffSet;
                TimeSunRaise = (timeTurnOff / 60).ToString("D2") + ":" + (timeTurnOff % 60).ToString("D2");
                TimeSunSet = (timeTurnOn / 60).ToString("D2") + ":" + (timeTurnOn % 60).ToString("D2");
            }
            else
            {
                TimeSunRaise = (info.time_sunrise/60).ToString("D2") + ":" + (info.time_sunrise%60).ToString("D2");
                TimeSunSet = (info.time_sunset/60).ToString("D2") + ":" + (info.time_sunset%60).ToString("D2");
            }
            Wlst.Sr.EquipmentInfoHolding.Services.Others.Sunrise = info.time_sunrise;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.Sunset = info.time_sunset;

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

        private string _nextPartolTime;

        public string NextPartolTime
        {
            get { return _nextPartolTime; }
            set
            {
                if (_nextPartolTime == value) return;
                _nextPartolTime = value;
                this.RaisePropertyChanged(() => this.NextPartolTime);
            }
        }

        //private long _lngNextPartolTime = 0;
        private int _intPartolTime =0;
        private DateTime _dtNextPartolTime;

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




        private Visibility _isShowEmergencyNum;

        public Visibility IsShowEmergencyNum
        {
            get { return _isShowEmergencyNum; }
            set
            {
                if (_isShowEmergencyNum == value) return;
                _isShowEmergencyNum = value;
                this.RaisePropertyChanged(() => this.IsShowEmergencyNum);
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


        private string _emergencyNums;

        public string EmergencyNums
        {
            get { return _emergencyNums; }
            set
            {
                if (_emergencyNums == value) return;
                _emergencyNums = value;
                this.RaisePropertyChanged(() => this.EmergencyNums);
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
                Sr.ProtocolPhone.LxSys.wlst_sys_connect_state,
                //.ClientPart.wlst_Measures_server_ans_clinet_request_SystemConnectStates,
                GetSystemConnectStates,
                typeof (StateBarViewModule), this, true);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxSys.wst_sys_nextpartoltime,
                //.ClientPart.wlst_Measures_server_ans_clinet_request_SystemConnectStates,
                GetNextPartolTime,
                typeof (StateBarViewModule), this, true);
            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtuTime.wst_timetable_set,
                                          ExSaveWeek, typeof (StateBarViewModule), this, true);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxSys.wlst_eme_op_static,
                //.ClientPart.wlst_Measures_server_ans_clinet_request_SystemConnectStates,
                GetEmeNums,
                typeof(StateBarViewModule), this, true);

        }

        private void ExSaveWeek(string session, Wlst.mobile.MsgWithMobile infos)
        {
            GetTimeTableParameter();
            if (LuxEffective == int.MinValue)
            {
                LuxEffective = 60;
                LightOnOffSet = 0;
                LightOffOffSet = 0;
            }
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

            //ConTitps = "通信时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " +
            //           (IsUserInLimitLogin ? "非完全登陆模式" : "");

        }


        /// <summary>
        /// 获取 应急关灯异常的设备数量
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        private void GetEmeNums(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstEmeOpStatic ;
            if (info == null) return;
            this.EmergencyNums = info.FaultSwitch+"";



        }



        System.Windows.Threading.DispatcherTimer timer;
        private void GetNextPartolTime(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.Args;
            if (info == null) return;
            _intPartolTime = 0; 
            _dtNextPartolTime = new DateTime(info.Scid);
            //TimeSpan ts = _dtNextPartolTime - DateTime.Now;
            //if (ts.Ticks  < 0) return;
            ////_lngNextPartolTime = ts.Ticks;
            //NextPartolTime = new DateTime(ts.Ticks).ToString("HH:mm:ss");

            //timer = new System.Windows.Threading.DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, 1);   //间隔1秒
            //timer.Tick += new EventHandler(timer_Tick);
            //timer.Start();
        }

        //void timer_Tick(object sender, EventArgs e)
        //{
        //    if (_lngNextPartolTime == 0)
        //    {
        //        NextPartolTime = "正在巡测";
        //        timer.Stop();
        //        return;
        //    }
        //    _lngNextPartolTime = _lngNextPartolTime - 1;
        //    NextPartolTime = new DateTime(_lngNextPartolTime).ToString("HH:mm:ss");

        //}

        private void ReadNextPartolTime()
        {
            TimeSpan ts = _dtNextPartolTime - DateTime.Now;
            if (ts.Ticks > 0) return;
            var infoxx = Sr.ProtocolPhone.LxSys.wst_sys_nextpartoltime;
            // .wlst_cnt_wj3090_order_open_close_light_center ;//.ServerPart.wlst_OpenCloseLight_clinet_order_opencloseLightCenter;
            // info.WstCntOrderWj3090OpenClsoeCenter  = data;
            infoxx.Args.Addr.Add(0);
            infoxx.Args.Addr.Add(1);
            SndOrderServer.OrderSnd(infoxx, 10, 6);
        }

        private void InitEvent()
        {
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter,false );
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
                    //todo 监听 应急关灯异常数量


                    if(args.EventId ==Wlst.Sr.TimeTableSystem.Services.IdServices.EventIdAssign.TimeTimeUpdate)
                    {
                        GetTimeTableParameter();
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
                    if (args.EventId == Wlst.Sr.TimeTableSystem.Services.IdServices.EventIdAssign.TimeTimeUpdate)
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


        public void ClearEmergencyNum()
        {
            //显示应急中心
            RegionManage.ShowViewByIdAttachRegion(1102820,true);
        }

    }

}