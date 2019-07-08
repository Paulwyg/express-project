using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Input;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Microsoft.Win32;
using Wlst.client;
using Wlst.Ux.StateBarModule.CommonSet.BaseView;

namespace Wlst.Ux.StateBarModule.CommonSet
{
    [Export(typeof (IICommonSetting))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class CommonSettingViewModel : ObservableObject, IICommonSetting
    {
        public CommonSettingViewModel()
        {
            this.InitAction();
            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(delayEvent, 1);
            var tmp = LoadXmldata();
            Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm = tmp.Item1;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm = tmp.Rest.Item6;

            Wlst.Sr.EquipmentInfoHolding.Services.Others.CopyDataBaseFromSvr = tmp.Item3;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowThisViewOnNewErrArriveInfo = tmp.Item4;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowNewErrArriveOnUi = tmp.Item5;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection = tmp.Item6;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowLotStopRunning = tmp.Item7;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowTimeTableOnTime = tmp.Rest.Item1 ;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsAllowVoiceReport = tmp.Rest.Item2;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowErrsCal = tmp.Rest.Item3;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsControlCenterNoErr = tmp.Rest.Item4;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowArgsInErrInfo = tmp.Rest.Item5;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.RemarkName = tmp.Rest.Item7;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.LoadParaPath = tmp.Rest.Rest.Item1;

            IsD = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;

            this.IsTimeTableSaveShowReport = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3302, 1, false);
            this.IsRefreshAfterReCn = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3302, 2, false);
            var tmpp =Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3302, 3, "0", null);

            this.GlobalAShield =tmpp == null?0.0: Convert.ToDouble(tmpp.Trim());

            this.stylename = "";
        }


        private void InitAction()
        {

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSys  .wst_sys_title  ,//.ClientPart.wlst_server_ans_clinet_request_sys_title,
                ActionRcvSysTitle,
                typeof (CommonSettingViewModel), this,true);
        }

        private void delayEvent()
        {
            var xxxinfo = Wlst.Sr.ProtocolPhone .LxSys  .wst_sys_title  ;//.ServerPart.wlst_clinet_request_sys_title;
            xxxinfo.WstSysTitle  .Op  = 1;
            SndOrderServer.OrderSnd(xxxinfo, 1, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        public void ActionRcvSysTitle(string session,Wlst .mobile .MsgWithMobile  infos)
        {

            if (infos.WstSysTitle   == null) return;
            try
            {
                if (infos.WstSysTitle.Op == 1)
                {
                    if (string.IsNullOrEmpty(infos.WstSysTitle.SystemName)) return;
                    var dir = new Dictionary<string, string>();
                    dir.Add("SystemTitle", infos.WstSysTitle.SystemName);
                    this.SystemName = infos.WstSysTitle.SystemName;
                    Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName = infos.WstSysTitle.SystemName;

                    //this.HLbphLower = infos.WstSysTitle.HlbphLower;
                    //this.HLbphUpper = infos.WstSysTitle.HlbphUpper;
                    //this.HlbphUpdateAlarm = infos.WstSysTitle.HlbphUpdateAlarm;
                    //this.HLbphTimer = infos.WstSysTitle.HlbphTimer;

                    //lvf 2018年5月25日16:27:04  地图经纬度
                    this.SystemX = infos.WstSysTitle.Lng;
                    this.SystemY = infos.WstSysTitle.Lat;

                    //lvf 2018年9月4日10:10:34  全局屏蔽小电流
                    this.GlobalAShield = infos.WstSysTitle.GlobalAShield;


                    sysx = this.SystemX;
                    sysy = this.SystemY;

                    //lvf 2019年4月28日14:12:12 设置区域读取
                    this.RegionItems.Clear();
                    Wlst.Sr.EquipmentInfoHolding.Services.Others.RegionItems.Clear();
                    foreach (var g in infos.WstSysTitle.ReginItems)
                    {
                        RegionItems.Add(new RegionItem()
                        {
                            RegionId = g.Id,
                            RegionName = g.RegionName
                        });

                        var tu = new Tuple<int,string>(g.Id,g.RegionName);
                        Sr.EquipmentInfoHolding.Services.Others.RegionItems.Add(tu);
                    }
                    RegionRemarks = "共有" + this.RegionItems.Count + "个地区";
                    //todo test
                    //for (int i = 0; i < 3; i++)
                    //{
                    //    RegionItems.Add(new RegionItem()
                    //    {
                    //        RegionId = i,
                    //        RegionName = "新地区"+i
                    //    });

                    //    //Wlst.Sr.EquipmentInfoHolding.Services.Others.RegionItems.Clear();
                    //    //var tu = new Tuple<int, string>(i, );
                    //    //Sr.EquipmentInfoHolding.Services.Others.RegionItems.Add(tu);
                    //}



                    Elysium.ThemesSet.Common.ReadSave.Save(dir, TitleSetPath,TitleFilePath);


                    //MainWindow.title.change
                    var arg = new PublishEventArgs() {EventType = "MainWindow.title.change"};
                    arg.AddParams(SystemName);
                    EventPublish.PublishEvent(arg);
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }


        #region  define

        #region SystemName

        private string _backgroundColor;

        public string SystemName
        {
            get { return _backgroundColor; }
            set
            {
                if (value != _backgroundColor)
                {
                    _backgroundColor = value;
                    RaisePropertyChanged(() => this.SystemName);
                }
            }
        }

        #endregion



        #region RemarkName

        private string _RemarkName;

        public string RemarkName
        {
            get { return _RemarkName; }
            set
            {
                if (value != _RemarkName)
                {
                    _RemarkName = value;
                    RaisePropertyChanged(() => this.RemarkName);
                }
            }
        }



        #endregion


        #region RegionRemarks

        private string _regionRemarks;

        public string RegionRemarks
        {
            get { return _regionRemarks; }
            set
            {
                if (value != _regionRemarks)
                {
                    _regionRemarks = value;
                    RaisePropertyChanged(() => this.RegionRemarks);
                }
            }
        }



        #endregion
        

        #region IsOpenCloseLigthSecondConfirm


        private int _isOpenCloseLigthSecondConfirm;

        public int IsOpenCloseLigthSecondConfirm
        {
            get { return _isOpenCloseLigthSecondConfirm; }
            set
            {
                if (value != _isOpenCloseLigthSecondConfirm)
                {
                    _isOpenCloseLigthSecondConfirm = value;
                    RaisePropertyChanged(() => IsOpenCloseLigthSecondConfirm);
                }
            }
        }

        private int _isCloseLigthSecondConfirm;

        public int IsCloseLigthSecondConfirm
        {
            get { return _isCloseLigthSecondConfirm; }
            set
            {
                if (value != _isCloseLigthSecondConfirm)
                {
                    _isCloseLigthSecondConfirm = value;
                    RaisePropertyChanged(() => IsCloseLigthSecondConfirm);
                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _openCloseLigthSecondConfirm;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> OpenCloseLigthSecondConfirm
        {
            get
            {
                if (_openCloseLigthSecondConfirm == null)
                {
                    _openCloseLigthSecondConfirm = new ObservableCollection<NameValueInt>();
                    //for (int i = 1; i < 24; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + "小时", Value = i});
                    //}
                    //for (int i = 1; i < 94; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + " 天", Value = i*24});
                    //}

                    _openCloseLigthSecondConfirm.Add(new NameValueInt() { Name = "不验证", Value = 0 });
                    _openCloseLigthSecondConfirm.Add(new NameValueInt() { Name = "二次确认", Value = 1 });
                    _openCloseLigthSecondConfirm.Add(new NameValueInt() { Name = "密码确认", Value = 2 });


                }
                return _openCloseLigthSecondConfirm;
            }
        }
     

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _showArgsInErrInfo;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> ShowArgsInErrInfo
        {
            get
            {
                if (_showArgsInErrInfo == null)
                {
                    _showArgsInErrInfo = new ObservableCollection<NameValueInt>();
                    //for (int i = 1; i < 24; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + "小时", Value = i});
                    //}
                    //for (int i = 1; i < 94; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() {Name = i + " 天", Value = i*24});
                    //}

                    _showArgsInErrInfo.Add(new NameValueInt() { Name = "不显示参数", Value = 0 });
                    _showArgsInErrInfo.Add(new NameValueInt() { Name = "除了额定电流，都显示", Value = 1 });
                    _showArgsInErrInfo.Add(new NameValueInt() { Name = "全部显示", Value = 2 });


                }
                return _showArgsInErrInfo;
            }
        }

        #endregion

        #region IsShowThisViewOnNewErrArriveInfo


        private bool _cheIsShowThisViewOnNewErrArriveck;

        public bool IsShowThisViewOnNewErrArriveInfo
        {
            get { return _cheIsShowThisViewOnNewErrArriveck; }
            set
            {
                if (value != _cheIsShowThisViewOnNewErrArriveck)
                {
                    _cheIsShowThisViewOnNewErrArriveck = value;
                    RaisePropertyChanged(() => IsShowThisViewOnNewErrArriveInfo);
                }
            }
        }



        #endregion

        #region IsShowNewErrArriveOnUi


        private bool _cheIsShowNewErrOnUi;

        public bool IsShowNewErrArriveOnUi
        {
            get { return _cheIsShowNewErrOnUi; }
            set
            {
                if (value != _cheIsShowNewErrOnUi)
                {
                    _cheIsShowNewErrOnUi = value;
                    RaisePropertyChanged(() => IsShowNewErrArriveOnUi);
                }
            }
        }



        #endregion

        #region IsShowTimeTableOnTime


        private bool _cheIsShowTimeTableOnTime;

        public bool IsShowTimeTableOnTime
        {
            get { return _cheIsShowTimeTableOnTime; }
            set
            {
                if (value != _cheIsShowTimeTableOnTime)
                {
                    _cheIsShowTimeTableOnTime = value;
                    RaisePropertyChanged(() => IsShowTimeTableOnTime);
                }
            }
        }



        #endregion

        #region IsShowLotStopRunning


        private bool _cheIsShowLotStopRunning;

        public bool IsShowLotStopRunning
        {
            get { return _cheIsShowLotStopRunning; }
            set
            {
                if (value != _cheIsShowLotStopRunning)
                {
                    _cheIsShowLotStopRunning = value;
                    RaisePropertyChanged(() => IsShowLotStopRunning);
                }
            }
        }



        #endregion

        #region IsOldUseTwoOpenLightSection


        private bool _isOldUseTwoOpenLightSection;

        public bool IsOldUseTwoOpenLightSection
        {
            get { return _isOldUseTwoOpenLightSection; }
            set
            {
                if (value != _isOldUseTwoOpenLightSection)
                {
                    _isOldUseTwoOpenLightSection = value;
                    RaisePropertyChanged(() => IsOldUseTwoOpenLightSection);
                }
            }
        }



        #endregion

        #region IsAllowVoiceReport


        private bool _cheIsAllowVoiceReport;

        public bool IsAllowVoiceReport
        {
            get { return _cheIsAllowVoiceReport; }
            set
            {
                if (value != _cheIsAllowVoiceReport)
                {
                    _cheIsAllowVoiceReport = value;
                    RaisePropertyChanged(() => IsAllowVoiceReport);
                }
            }
        }



        #endregion

        #region IsD


        private bool _cheIsD;

        public bool IsD
        {
            get { return _cheIsD; }
            set
            {
                if (value != _cheIsD)
                {
                    _cheIsD = value;
                    RaisePropertyChanged(() => IsD);
                }
            }
        }



        #endregion

        #region IsShowErrsCal


        private bool _cheIsShowErrsCal;

        public bool IsShowErrsCal
        {
            get { return _cheIsShowErrsCal; }
            set
            {
                if (value != _cheIsShowErrsCal)
                {
                    _cheIsShowErrsCal = value;
                    RaisePropertyChanged(() => IsShowErrsCal);
                }
            }
        }



        #endregion

        #region IsControlCenterNoErr

        private bool _isControlCenterNoErr;

        /// <summary>
        /// 控制中心屏蔽报警
        /// </summary>
        public bool IsControlCenterNoErr
        {
            get { return _isControlCenterNoErr; }
            set
            {
                if (_isControlCenterNoErr == value) return;
                _isControlCenterNoErr = value;
                RaisePropertyChanged(() => IsControlCenterNoErr);
            }
        }

        #endregion

        #region IsTimeTableSaveShowReport

        private bool _isTimeTableSaveShowReport;

        /// <summary>
        /// 时间表设置保存弹出周设置发送情况
        /// </summary>
        public bool IsTimeTableSaveShowReport
        {
            get { return _isTimeTableSaveShowReport; }
            set
            {
                if (_isTimeTableSaveShowReport == value) return;
                _isTimeTableSaveShowReport = value;
                RaisePropertyChanged(() => IsTimeTableSaveShowReport);
            }
        }

        #endregion

        #region IsRefreshAfterReCn

        private bool _isRefreshAfterReCn;

        /// <summary>
        /// 系统重连,是否刷新界面  lvf  2018年3月29日08:42:29
        /// </summary>
        public bool IsRefreshAfterReCn
        {
            get { return _isRefreshAfterReCn; }
            set
            {
                if (_isRefreshAfterReCn == value) return;
                _isRefreshAfterReCn = value;
                RaisePropertyChanged(() => IsRefreshAfterReCn);
            }
        }

        #endregion



        #region IsShowArgsInErrInfo

        private int _isShowArgsInErrInfo;

        /// <summary>
        /// 故障查询界面，显示参数信息
        /// </summary>
        public int IsShowArgsInErrInfo
        {
            get { return _isShowArgsInErrInfo; }
            set
            {
                if (_isShowArgsInErrInfo == value) return;
                _isShowArgsInErrInfo = value;
                RaisePropertyChanged(() => IsShowArgsInErrInfo);
            }
        }

        #endregion

        #region LoadParaPath

        private string _loadParaPath;

        public string LoadParaPath
        {
            get { return _loadParaPath; }
            set
            {
                if (value != _loadParaPath)
                {
                    _loadParaPath = value;
                    RaisePropertyChanged(() => this.LoadParaPath);
                }
            }
        }
        #endregion

        #region LoadParaFileName

        private string _loadParaFileName;

        public string LoadParaFileName
        {
            get { return _loadParaFileName; }
            set
            {
                if (value != _loadParaFileName)
                {
                    _loadParaFileName = value;
                    RaisePropertyChanged(() => this.LoadParaFileName);
                }
            }
        }

        #endregion


        #region SystemX
        private double _value7;
        /// <summary>
        /// 地图X坐标 仅JPG
        /// </summary>
        public double SystemX
        {
            get { return _value7; }
            set
            {
                if (value != _value7)
                {
                    _value7 = value;
                    this.RaisePropertyChanged(() => this.SystemX);
                }
            }
        }
        #endregion


        #region SystemY

        private double _value8;
        /// <summary>
        /// 地图Y坐标仅JPG
        /// </summary>
        public double SystemY
        {
            get { return _value8; }
            set
            {
                if (value != _value8)
                {
                    _value8 = value;
                    this.RaisePropertyChanged(() => this.SystemY);
                }
            }
        }
        #endregion


        #region GlobalAShield

        private double _globalAShield;
        /// <summary>
        /// 全局屏蔽小电流
        /// </summary>
        public double GlobalAShield
        {
            get { return _globalAShield; }
            set
            {
                if (value != _globalAShield)
                {
                    _globalAShield = value;
                    this.RaisePropertyChanged(() => this.GlobalAShield);
                }
            }
        }
        #endregion




        //lvf 2019年4月28日14:07:55 设置地区
        #region RegionItems

        private ObservableCollection<RegionItem> _regionItems;
        public ObservableCollection<RegionItem> RegionItems
        {
            get
            {
                if (_regionItems == null) _regionItems = new ObservableCollection<RegionItem>();
                return _regionItems;
            }
            set
            {
                if (_regionItems == value) return;
                _regionItems = value;
                RaisePropertyChanged(() => RegionItems);
            }
        }

        #endregion




        //#region IsOnlyShowTmlOnline


        //private bool _cheIsOnlyShowTmlOnline;

        //public bool IsOnlyShowTmlOnline
        //{
        //    get { return _cheIsOnlyShowTmlOnline; }
        //    set
        //    {
        //        if (value != _cheIsOnlyShowTmlOnline)
        //        {
        //            _cheIsOnlyShowTmlOnline = value;
        //            RaisePropertyChanged(() => IsOnlyShowTmlOnline);
        //        }
        //    }
        //}



        //#endregion
        //#region IsLdlAs100Per


        //private bool _isLdlAs100Per;

        //public bool IsLdlAs100Per
        //{
        //    get { return _isLdlAs100Per; }
        //    set
        //    {
        //        if (value != _isLdlAs100Per)
        //        {
        //            _isLdlAs100Per = value;
        //            RaisePropertyChanged(() => IsLdlAs100Per);
        //        }
        //    }
        //}
        
        //#endregion

        //#region IsShowFastControl


        //private bool _isShowFastControl;

        //public bool IsShowFastControl
        //{
        //    get { return _isShowFastControl; }
        //    set
        //    {
        //        if (value != _isShowFastControl)
        //        {
        //            _isShowFastControl = value;
        //            RaisePropertyChanged(() => IsShowFastControl);
        //        }
        //    }
        //}



        //#endregion

        #region IsCopySvrDataBase


        private bool _OnIsCopySvrDataBase;

        public bool IsCopySvrDataBase
        {
            get { return _OnIsCopySvrDataBase; }
            set
            {
                if (value != _OnIsCopySvrDataBase)
                {
                    _OnIsCopySvrDataBase = value;
                    RaisePropertyChanged(() => IsCopySvrDataBase);
                }
            }
        }



        #endregion
        #endregion

        #region Open Para File
        private ICommand _cmdOpenParaFile;

        public ICommand CmdOpenParaFile
        {
            get
            {

                if (_cmdOpenParaFile == null) _cmdOpenParaFile = new RelayCommand(ExCmdOpenParaFile, CanExCmdOpenParaFile, false);
                return _cmdOpenParaFile;
            }
        }

        private void ExCmdOpenParaFile()
        {
            string dir = Directory.GetCurrentDirectory() + "\\SystemXmlConfig\\WJ3005Para";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            string extension = "xml";
            var dialog = new OpenFileDialog()
            {
                InitialDirectory = dir,
                DefaultExt = extension,
                Filter =
                    String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*",
                                  extension,
                                  "Xml File"),
                FilterIndex = 1
            };
            if (dialog.ShowDialog() == true)
            {
                LoadParaPath = dialog.FileName;
                this.LoadParaFileName = this.LoadParaPath.Substring(this.LoadParaPath.LastIndexOf("\\", System.StringComparison.Ordinal) + 1);
            }
        }


        private bool CanExCmdOpenParaFile()
        {
            return true;
        }
        #endregion

        #region Clear Para File
        private ICommand _cmdClearParaFile;

        public ICommand CmdClearParaFile
        {
            get
            {

                if (_cmdClearParaFile == null) _cmdClearParaFile = new RelayCommand(ExCmdClearParaFile, CanExCmdClearParaFile, false);
                return _cmdClearParaFile;
            }
        }

        private void ExCmdClearParaFile()
        {
            LoadParaPath = string.Empty;
            this.LoadParaFileName = LoadParaPath;
        }


        private bool CanExCmdClearParaFile()
        {
            return true;
        }
        #endregion




        #region CmdSetRegion

        private BaseView.SetSystemRegion _setSystemRegion = null;

        private ICommand _cmdSetRegion;

        public ICommand CmdSetRegion
        {
            get
            {

                if (_cmdSetRegion == null) _cmdSetRegion = new RelayCommand(ExCmdSetRegion, CanExCmdSetRegion, false);
                return _cmdSetRegion;
            }
        }

        private void ExCmdSetRegion()
        {
            if (_setSystemRegion != null)
            {
                try
                {
                    _setSystemRegion.OnFormBtnOkClick -=
                        new EventHandler<BaseView.EventArgsSetRegions>(_setSystemRegions_OnFormBtnOkClick);
                }
                catch (Exception ex)
                {

                }
                _setSystemRegion = null;
            }

            _setSystemRegion = new SetSystemRegion();
            _setSystemRegion.OnFormBtnOkClick +=
                new EventHandler<BaseView.EventArgsSetRegions>(_setSystemRegions_OnFormBtnOkClick);
            _setSystemRegion.SetContext(RegionItems);
            _setSystemRegion.ShowDialog();
        }


        private bool CanExCmdSetRegion()
        {
            return true;
        }


        private void _setSystemRegions_OnFormBtnOkClick(object sender, BaseView.EventArgsSetRegions e) //todo 暂存
        {
          

            try
            {
                _setSystemRegion.OnFormBtnOkClick -=
                    new EventHandler<BaseView.EventArgsSetRegions>(_setSystemRegions_OnFormBtnOkClick);
            }
            catch (Exception ex)
            {

            }
            _setSystemRegion = null;

            this.RegionRemarks = "共有" + this.RegionItems.Count + "个地区";
        }
        #endregion



        


        private DateTime _dtApply;
        private ICommand _cmdApply;

        public ICommand CmdApply
        {
            get
            {

                if (_cmdApply == null) _cmdApply = new RelayCommand(Ex, CanEx, false);
                return _cmdApply;
            }
        }

        //todo 目前未作对终端过滤  如停运不发送选测等
        private void Ex()
        {
            _dtApply = DateTime.Now;

            Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm =
                this.IsOpenCloseLigthSecondConfirm;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm =
    this.IsCloseLigthSecondConfirm;

            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowThisViewOnNewErrArriveInfo =
                this.IsShowThisViewOnNewErrArriveInfo;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowNewErrArriveOnUi =
                this.IsShowNewErrArriveOnUi;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection =
                this.IsOldUseTwoOpenLightSection;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowLotStopRunning =
                this.IsShowLotStopRunning;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowTimeTableOnTime =
                this.IsShowTimeTableOnTime;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsAllowVoiceReport =
                this.IsAllowVoiceReport;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowErrsCal =
                this.IsShowErrsCal;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsControlCenterNoErr =
                this.IsControlCenterNoErr;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowArgsInErrInfo =
                this.IsShowArgsInErrInfo;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.RemarkName = this.RemarkName;
    //        Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowHLbph =
    //            this._IsShowHLbph;
    //        Wlst.Sr.EquipmentInfoHolding.Services.Others.HLbphUpper =
    //            this.HLbphUpper;
    //        Wlst.Sr.EquipmentInfoHolding.Services.Others.HLbphLower =
    //            this.HLbphLower;
    //        Wlst.Sr.EquipmentInfoHolding.Services.Others.HlbphUpdateAlarm =
    //            this.HlbphUpdateAlarm;
    //        Wlst.Sr.EquipmentInfoHolding.Services.Others.HLbphTimer =
    //            this.HLbphTimer;
    //        Wlst.Sr.EquipmentInfoHolding.Services.Others.HLbphShieldTimer =
    //this.HLbphShieldTimer;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.LoadParaPath = this.LoadParaPath;
            //Wlst.Sr.EquipmentInfoHolding.Services.Others.IsOnlyShowTmlOnline =
            //    this.IsOnlyShowTmlOnline;
            //Wlst.Sr.EquipmentInfoHolding.Services.Others.IsLdlAs100Per =
            //    this.IsLdlAs100Per;
            //Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowFastControl =
            //    this.IsShowFastControl;
            this.SavConfig();

            var dir = new Dictionary<string, string>();
            dir.Add("SystemTitle", SystemName);
            Elysium.ThemesSet.Common.ReadSave.Save(dir, TitleSetPath,TitleFilePath);


            var xxxinfo = Wlst.Sr.ProtocolPhone .LxSys  .wst_sys_title  ;//.ServerPart.wlst_clinet_request_sys_title;
            xxxinfo.WstSysTitle.Op  = 2;
            xxxinfo.WstSysTitle.SystemName = this.SystemName;
            //xxxinfo.WstSysTitle.HlbphLower = this.HLbphLower;
            //xxxinfo.WstSysTitle.HlbphUpper = this.HLbphUpper;
            //xxxinfo.WstSysTitle.HlbphUpdateAlarm = this.HlbphUpdateAlarm;
            //xxxinfo.WstSysTitle.HlbphTimer = this.HLbphTimer;

            
            //lvf 2018年5月25日16:23:57 系统经纬度
            xxxinfo.WstSysTitle.Lng = this.SystemX;
            xxxinfo.WstSysTitle.Lat = this.SystemY;


            //lvf 2018年9月4日10:01:49  系统屏蔽小电流
            xxxinfo.WstSysTitle.GlobalAShield = this.GlobalAShield; //todo



            //lvf 2019年4月28日14:45:57  设置区域
            var lstRegion = new List<SystemTitle.ReginInfo>();
            foreach (var g in RegionItems)
            {
                lstRegion.Add(new SystemTitle.ReginInfo()
                {
                    Id = g.RegionId,
                    RegionName = g.RegionName
                });
            }
            xxxinfo.WstSysTitle.ReginItems = lstRegion;

            SndOrderServer.OrderSnd(xxxinfo, 1, 1);

            
        }

        private bool CanEx()
        {
            if (
                this.SystemName == sysname &&
                (this.CurrentSelectItem != null && CurrentSelectItem.Value == stylename) &&
                this.IsOpenCloseLigthSecondConfirm ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm &&
                this.IsCloseLigthSecondConfirm ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm &&
                 this.IsCopySvrDataBase  ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.CopyDataBaseFromSvr  &&
                !string.IsNullOrEmpty(SystemName) &&
                this.IsShowThisViewOnNewErrArriveInfo ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowThisViewOnNewErrArriveInfo &&
                this.IsShowNewErrArriveOnUi==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowNewErrArriveOnUi &&
                this.IsOldUseTwoOpenLightSection ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection &&
                this.IsShowLotStopRunning==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowLotStopRunning &&
                this.IsShowTimeTableOnTime ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others .IsShowTimeTableOnTime &&
                this.IsAllowVoiceReport ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsAllowVoiceReport &&
                this.IsShowErrsCal ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowErrsCal &&
                this.IsControlCenterNoErr ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsControlCenterNoErr &&
                this.IsShowArgsInErrInfo ==
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowArgsInErrInfo &&
                this.RemarkName == Wlst.Sr.EquipmentInfoHolding.Services.Others.RemarkName &&
                 Wlst.Sr.EquipmentInfoHolding.Services.Others.LoadParaPath == this.LoadParaPath
                //&&
                //this.IsOnlyShowTmlOnline ==
                //Wlst.Sr.EquipmentInfoHolding.Services.Others.IsOnlyShowTmlOnline 
                //&& this.IsLdlAs100Per ==
                //Wlst.Sr.EquipmentInfoHolding.Services.Others.IsLdlAs100Per &&
                //this.IsShowFastControl ==
                //Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowFastControl
                && this.SystemX == sysx && this.SystemY == sysy
                )
                return false;
            return DateTime.Now.Ticks - _dtApply.Ticks > 10000000;
        }

        private string sysname;

        private double sysx;
        private double sysy;


        public void NavOnLoad(params object[] parsObjects)
        {
            IsD = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D;
            this.SystemName = GetSystemTitle();
            this.IsOpenCloseLigthSecondConfirm =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.OpenCloseLightSecondConfirm;
            this.IsCloseLigthSecondConfirm =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.CloseLightSecondConfirm;
            this.IsCopySvrDataBase = Wlst.Sr.EquipmentInfoHolding.Services.Others.CopyDataBaseFromSvr;
            this.IsShowThisViewOnNewErrArriveInfo =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowThisViewOnNewErrArriveInfo;
            this.IsShowNewErrArriveOnUi =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowNewErrArriveOnUi;
            this.IsOldUseTwoOpenLightSection =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsOldUseTwoOpenLightSection;
            this.IsShowLotStopRunning =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowLotStopRunning;
            this.IsShowTimeTableOnTime =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowTimeTableOnTime;
            this.IsAllowVoiceReport =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsAllowVoiceReport;
            this.IsShowErrsCal =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowErrsCal;
            this.IsControlCenterNoErr =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsControlCenterNoErr;
            this.IsShowArgsInErrInfo =
                Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowArgsInErrInfo;
            this.RemarkName = Wlst.Sr.EquipmentInfoHolding.Services.Others.RemarkName;
            this.LoadParaPath = Wlst.Sr.EquipmentInfoHolding.Services.Others.LoadParaPath;
            

            if (!(string.IsNullOrEmpty(this.LoadParaPath)))
            {
                this.LoadParaFileName =
                    this.LoadParaPath.Substring(this.LoadParaPath.LastIndexOf("\\", System.StringComparison.Ordinal) + 1);
            }
            else
            {
                this.LoadParaFileName = string.Empty;
            }
            //this.IsOnlyShowTmlOnline =
            //     Wlst.Sr.EquipmentInfoHolding.Services.Others.IsOnlyShowTmlOnline;
            //this.IsShowFastControl =
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowFastControl;
            //this.IsLdlAs100Per =
            //    Wlst.Sr.EquipmentInfoHolding.Services.Others.IsLdlAs100Per;
            sysname = this.SystemName;
            _isVmChangeCurrentselect = true;
            stylename = LoadXmldata().Item2;
            //     stylename = this.StyleName;

            foreach (var t in this.StyleItems)
            {
                if (t.Value.Equals(stylename))
                {
                    CurrentSelectItem = t;
                    break;
                }
            }


            this.IsRefreshAfterReCn = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3302, 2, false);

            this.GlobalAShield = Convert.ToDouble(Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOption(3302, 3,"0",null).Trim());
            


            _isVmChangeCurrentselect = false;

            delayEvent();
        }



    }

    public partial class CommonSettingViewModel
    {

        private string stylename;

        private bool _isshowreversetmp;
        //#region StyleName

        //private string _bStyleName;

        //public string StyleName
        //{
        //    get { return _bStyleName; }
        //    set
        //    {
        //        if (value != _bStyleName)
        //        {
        //            _bStyleName = value;
        //            RaisePropertyChanged(() => this.StyleName);
        //            if (_isVmChangeCurrentselect)
        //            {
        //                foreach (var t in this.StyleItems)
        //                {
        //                    if (t.Value.Equals(value))
        //                    {
        //                        CurrentSelectItem = t;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //#endregion

        private bool _isVmChangeCurrentselect;
        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueString> _styleItems = null;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueString> StyleItems
        {
            get
            {
                if (_styleItems == null)
                {
                    _styleItems = new ObservableCollection<NameValueString>();
                    try
                    {
                       // _styleItems.Add(new NameValueString() { Name = "当前样式", Value = "当前样式" });

                        string dir = Directory.GetCurrentDirectory() + "\\SystemColorAndFont";
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        DirectoryInfo mydir = new DirectoryInfo(dir);

                        foreach (FileSystemInfo fsi in mydir.GetDirectories())
                        {
                            var fulename = fsi.FullName;
                            var filss = new DirectoryInfo(fulename);
                            bool isxml = false;
                            foreach (FileSystemInfo g in filss.GetFileSystemInfos())
                            {
                                if (g.FullName.ToUpper().EndsWith(".xml".ToUpper()))
                                {
                                    isxml = true;
                                    break;
                                }
                            }
                            if (isxml)
                            {
                                _styleItems.Add(new NameValueString() {Name = fsi.Name, Value = fsi.Name});
                            }
                        }
                        //if (_styleItems.Count > 0) CurrentSelectItem = _styleItems[0];


                    }
                    catch (Exception ex)
                    {
                    }
                }
                return _styleItems;
            }
        }


        private Wlst.Cr.CoreOne.Models.NameValueString _currentSelectItem;

        public Wlst.Cr.CoreOne.Models.NameValueString CurrentSelectItem
        {
            get { return _currentSelectItem; }
            set
            {
                if (value == _currentSelectItem) return;
                _currentSelectItem = value;
                this.RaisePropertyChanged(() => this.CurrentSelectItem);
            }
        }

    }


    public partial class CommonSettingViewModel
    {

  
        private const string TitleSetPath = "Cetc50_System_Title";
        private const string TitleFilePath = "SystemXmlConfig";
        private string GetSystemTitle()
        {
            var info = Elysium.ThemesSet.Common.ReadSave.Read(TitleSetPath,TitleFilePath);
            string res = "";
            if (info.ContainsKey("SystemTitle"))
            {
                res = info["SystemTitle"];
                return res;
            }
            else
            {
                res = "城市数字照明综合监控管理系统";
                try
                {
                    var temp = new Dictionary<string, string>();
                    temp.Add("SystemTitle", res);
                    Elysium.ThemesSet.Common.ReadSave.Save(temp, TitleSetPath);
                }
                catch (Exception)
                {

                }
                return res;

            }

        }


        public const string XmlConfigName = "SystemCommonSetConfg";

        /// <summary>
        /// RowHeight LoopNameLength TimeNameLength VaNameLength
        /// </summary>
        /// <returns></returns>
        private static Tuple<int, string, bool,bool,bool,bool,bool,Tuple<bool,bool,bool,bool,int,int,string,Tuple<string>>> LoadXmldata()
        {


            int x1 = 1;
            int x12 = 1;
            int x2 = 1;
            int x3 = 0;
            int x4 = 0;
            int x5 = 0;
            int x6 = 0;
            int x7 = 0;
            int x8 = 0;
            int x9 = 0;
            int x10 = 0;
            int x11 = 0;
            string finename = "";
            string paraPath = "";
            string remarkname = "终端备注";

            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("IsOpenCloseLigthSecondConfirm"))
            {
                try
                {
                    x1 = Convert.ToInt32(info["IsOpenCloseLigthSecondConfirm"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsCloseLigthSecondConfirm"))
            {
                try
                {
                    x12 = Convert.ToInt32(info["IsCloseLigthSecondConfirm"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsShowThisViewOnNewErrArriveInfo"))
            {
                try
                {
                    x3 = Convert.ToInt32(info["IsShowThisViewOnNewErrArriveInfo"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsShowNewErrArriveOnUi"))
            {
                try
                {
                    x4 = Convert.ToInt32(info["IsShowNewErrArriveOnUi"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsOldUseTwoOpenLightSection"))
            {
                try
                {
                    x5 = Convert.ToInt32(info["IsOldUseTwoOpenLightSection"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("CurrentStyleName"))
            {
                try
                {
                    finename = info["CurrentStyleName"].Trim();
                }
                catch (Exception ex)
                {
                }
            }

            if (info.ContainsKey("RemarkName"))
            {
                try
                {
                    remarkname = info["RemarkName"].Trim();
                }
                catch (Exception ex)
                {
                }
            }

            if (info.ContainsKey("IsCopySvrDataBase"))
            {
                try
                {
                    x2 = Convert.ToInt32(info["IsCopySvrDataBase"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsShowLotStopRunning"))
            {
                try
                {
                    x6 = Convert.ToInt32(info["IsShowLotStopRunning"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsShowTimeTableOnTime"))
            {
                try
                {
                    x7 = Convert.ToInt32(info["IsShowTimeTableOnTime"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsAllowVoiceReport"))
            {
                try
                {
                    x8 = Convert.ToInt32(info["IsAllowVoiceReport"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsShowErrsCal"))
            {
                try
                {
                    x9 = Convert.ToInt32(info["IsShowErrsCal"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsControlCenterNoErr"))
            {
                try
                {
                    x10 = Convert.ToInt32(info["IsControlCenterNoErr"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsShowArgsInErrInfo"))
            {
                try
                {
                    x11 = Convert.ToInt32(info["IsShowArgsInErrInfo"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("LoadParaPath"))
            {
                try
                {
                    paraPath = info["LoadParaPath"].Trim();
                }
                catch (Exception ex)
                {
                }
            }
            //if (info.ContainsKey("IsShowFastControl"))
            //{
            //    try
            //    {
            //        x6 = Convert.ToInt32(info["IsShowFastControl"]);
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
            //if (info.ContainsKey("IsLdlAs100Per"))
            //{
            //    try
            //    {
            //        x7 = Convert.ToInt32(info["IsLdlAs100Per"]);
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}

            return new Tuple<int, string, bool, bool, bool, bool, bool, Tuple<bool, bool, bool, bool, int, int, string, Tuple<string>>>(x1, finename, x2 == 1, x3 == 1, x4 == 1, x5 == 1, x6 == 1, new Tuple<bool, bool, bool, bool, int, int, string, Tuple<string>>(x7 == 1, x8 == 1, x9 == 1, x10 == 1, x11, x12, remarkname, new Tuple<string>(paraPath)));

        }

        /// <summary>
        /// 
        /// </summary>
        public void SavConfig()
        {
            if (CurrentSelectItem != null) stylename = CurrentSelectItem.Value;
            var info = new Dictionary<string, string>();
            info.Add("IsOpenCloseLigthSecondConfirm", (this.IsOpenCloseLigthSecondConfirm) + "");
            info.Add("IsCloseLigthSecondConfirm", (this.IsCloseLigthSecondConfirm) + "");
            info.Add("IsCopySvrDataBase", (this.IsCopySvrDataBase ? 1 : 0) + "");
            info.Add("CurrentStyleName", stylename);
            info.Add("IsShowThisViewOnNewErrArriveInfo", (this.IsShowThisViewOnNewErrArriveInfo ? 1 : 0) + "");
            info.Add("IsShowNewErrArriveOnUi", (this.IsShowNewErrArriveOnUi ? 1 : 0) + "");
            info.Add("IsOldUseTwoOpenLightSection", (this.IsOldUseTwoOpenLightSection ? 1 : 0) + "");
            info.Add("IsShowLotStopRunning", (this.IsShowLotStopRunning ? 1 : 0) + "");
            info.Add("IsShowTimeTableOnTime", (this.IsShowTimeTableOnTime ? 1 : 0) + "");
            info.Add("IsAllowVoiceReport", (this.IsAllowVoiceReport ? 1 : 0) + "");
            info.Add("IsShowErrsCal", (this.IsShowErrsCal ? 1 : 0) + "");
            info.Add("IsControlCenterNoErr", (this.IsControlCenterNoErr ? 1 : 0) + "");
            info.Add("IsShowArgsInErrInfo", (this.IsShowArgsInErrInfo) + "");
            info.Add("RemarkName", RemarkName);
            info.Add("LoadParaPath", (this.LoadParaPath) + "");


            //info.Add("IsOnlyShowTmlOnline", (this.IsOnlyShowTmlOnline ? 1 : 0) + "");
            //info.Add("IsLdlAs100Per", (this.IsLdlAs100Per ? 1 : 0) + "");
            //info.Add("IsShowFastControl", (this.IsShowFastControl ? 1 : 0) + "");
            Wlst.Cr.CoreOne.Services.SystemXmlConfig.Save(info, XmlConfigName);
            ChangeXmlConfig(stylename);


            var dicOp = new Dictionary<int, string>();
            var dicDesc = new Dictionary<int, string>();
            dicOp.Add(1, IsTimeTableSaveShowReport ? "1" : "0");
            dicDesc.Add(1, "数据查询倒序显示");

            dicOp.Add(2, IsRefreshAfterReCn ? "1" : "0");
            dicDesc.Add(2, "系统重连后,是否刷新界面");

            dicOp.Add(3, GlobalAShield+"");
            dicDesc.Add(3, "全局屏蔽小电流");

            dicOp.Add(4, SystemX + "");
            dicDesc.Add(4, "系统经纬度");

            dicOp.Add(5, SystemY + "");
            dicDesc.Add(5, "系统经纬度");

            //dicOp.Add(3, HLbphShieldTimer+"" );
            //dicDesc.Add(3, "火零不平衡应急关灯屏蔽时间表时间");
          
            Wlst.Cr.CoreOne.Services.OptionXmlSvr.SaveXml(3302, dicOp, dicDesc); //3005模块，数据查询倒序
            

           
        }


        private void ChangeXmlConfig(string filename)
        {
            try
            {

                if (string.IsNullOrEmpty(filename)) return;
                string dir = Directory.GetCurrentDirectory() + "\\SystemColorAndFont";
                if (!Directory.Exists(dir)) return;
                string dirnew = Directory.GetCurrentDirectory() + "\\SystemColorAndFont\\" + filename;
                if (!Directory.Exists(dirnew)) return;
                DirectoryInfo mydir = new DirectoryInfo(dir);
                DirectoryInfo mydirnew = new DirectoryInfo(dirnew);


                int xcount = 0;
                foreach (FileSystemInfo g in mydirnew.GetFileSystemInfos())
                {
                    if (g.FullName.ToUpper().EndsWith(".xml".ToUpper()))
                    {
                        xcount++;
                    }
                }
                if (xcount < 15) return;

                foreach (FileSystemInfo g in mydir.GetFileSystemInfos())
                {
                    if (g.FullName.ToUpper().EndsWith(".xml".ToUpper()))
                    {
                        File.Delete(g.FullName);
                    }
                }
                foreach (FileSystemInfo g in mydirnew.GetFileSystemInfos())
                {
                    if (g.FullName.ToUpper().EndsWith(".xml".ToUpper()))
                    {
                        //File.Delete(g.FullName);
                        File.Copy(g.FullName, dir + "\\" + g.Name, true);
                    }
                }


            }
            catch (Exception ex)
            {
            }
        }

    }


    public class RegionItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {


        private int _regionId;
        /// <summary>
        /// 地区编号
        /// </summary>
        public int RegionId
        {
            get { return _regionId; }
            set
            {
                if (value == _regionId) return;
                _regionId = value;
                this.RaisePropertyChanged(() => this.RegionId);
            }
        }

        private string _regionName;
        /// <summary>
        /// 地区名称
        /// </summary>
        public string RegionName
        {
            get { return _regionName; }
            set
            {
                if (value == _regionName) return;
                _regionName = value;
                this.RaisePropertyChanged(() => this.RegionName);
            }
        }





    }

}
