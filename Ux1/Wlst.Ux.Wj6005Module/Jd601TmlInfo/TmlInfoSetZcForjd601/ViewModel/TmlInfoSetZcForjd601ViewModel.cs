using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.TmlInfoSetZcForjd601.Services;
using Wlst.client;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.TmlInfoSetZcForjd601.ViewModel
{
    [Export(typeof(IITmlInfoSetZcForjd601))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlInfoSetZcForjd601ViewModel : Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, IITmlInfoSetZcForjd601
    {
        public TmlInfoSetZcForjd601ViewModel()
        {
            InitAction();
        }
        public void NavOnLoad(params object[] parsObjects)
        {
     
            var tmlId = (int)parsObjects[0];
            if (tmlId > 0)
            {
                SelectedTmlChange(tmlId);
            }
            ZRtuId = tmlId.ToString(CultureInfo.InvariantCulture);
            OnInitZc();
        }
        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "参数召测"; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        #endregion

        /// <summary>
        /// 提供外界更改终端
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        public void SelectedTmlChange(int rtuId)
        {
            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                     ContainsKey(rtuId)) return;
            var t =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                    rtuId]
                     as Sr.EquipmentInfoHolding.Model.Wj601Esu;

            if (t == null) return;

            var tmp=new EsuParameter()
                        {
                            EsuAlarmDelay = t.WjEsu.EsuAlarmDelay,
                            EsuCloseTime = t.WjEsu.EsuCloseTime,
                            EsuCommTypeCode = t.WjEsu.EsuCommTypeCode,
                            EsuCtRadioA = t.WjEsu.EsuCtRadioA,
                            EsuCtRadioB = t.WjEsu.EsuCtRadioB,
                            EsuCtRadioC = t.WjEsu.EsuCtRadioC,
                            EsuEnerySaveTemp = t.WjEsu.EsuEnerySaveTemp,
                            EsuFanClosedTemp = t.WjEsu.EsuFanClosedTemp,
                            EsuFanSatrtTemp = t.WjEsu.EsuFanSatrtTemp,
                            EsuInputOvervoltageLimit = t.WjEsu.EsuInputOvervoltageLimit,
                            EsuInputUndervoltageLimit = t.WjEsu.EsuInputUndervoltageLimit,
                            EsuMandatoryProtectTemp = t.WjEsu.EsuMandatoryProtectTemp,
                            EsuOutputOverloadLimit = t.WjEsu.EsuOutputOverloadLimit,
                            EsuOutputUndervoltageLimit = t.WjEsu.EsuOutputUndervoltageLimit,
                            EsuPowerSupplyPhases = t.WjEsu.EsuPowerSupplyPhases,
                            EsuPreheatingTime = t.WjEsu.EsuPreheatingTime,
                            EsuRecoverEnergySaveTemp = t.WjEsu.EsuRecoverEnergySaveTemp,
                            EsuRegulatingSpeed = t.WjEsu.EsuRegulatingSpeed,
                            EsuRunMode = t.WjEsu.EsuRunMode,
                            EsuTimeMode = t.WjEsu.EsuTimeMode,
                            EsuWorkMode = t.WjEsu.EsuWorkMode,
                            EsyValidIdentify = t.WjEsu.EsyValidIdentify,
                            IsActiveAlarm = t.WjEsu.IsActiveAlarm,
                            EsuMode = t.WjEsu.EsuMode
                            
             
                        };
            
            _terminalInformation = new Wj601Esu(new EquipmentParameter()
            {
             RtuArgu = t.RtuArgu,
             RtuFid = t.RtuFid,
             RtuGisX = t.RtuGisX,
             RtuGisY = t.RtuGisY,
             RtuId = t.RtuId,
             RtuInstallAddr = t.RtuInstallAddr,
             RtuMapX = t.RtuMapX,
             RtuMapY = t.RtuMapY,
             RtuModel = t.RtuModel,
             RtuName = t.RtuName,
             RtuPhyId = t.RtuPhyId,
             RtuRemark = t.RtuRemark,
             RtuStateCode = t.RtuStateCode
            },
            tmp );



            //var ffff = t.Clone();
            //_terminalInformation = ffff as Jd601TerminalInformation;
            //if (_terminalInformation==null)return;
            ZcJd601Par();
            //if (_terminalInformation != null)
            //{
            //    UpdateVmInfo(_terminalInformation);
            //    AttachRtuId = _terminalInformation.AttachRtuId;
            //}
            //else
            //{
            //    AttachRtuId = 0;
            //    AttachRtuName = "无法解析该设备连接的终端";
            //}
            //OnLoadJd601ParVm();
            //Thread.Sleep(100);
            //RequestJd601Pars(RtuId);

        }
    }
    /// <summary>
    /// 召测参数
    /// </summary>
    public partial class TmlInfoSetZcForjd601ViewModel
    {
        #region private info;

        private string _value1;
        private string _value2;
        private string _value3;
        private string _value4;
        private string _value5;
        private string _value6;
        private string _value7;
        private string _value8;
        private string _value9;
        private string _value10;
        private string _value11;
        private string _value12;
        private string _value13;
        private string _value14;
        private string _value15;
        private string _value16;
        private string _value17;
        private string _value18;
        private string _value19;
        private string _value20;
        private string _value21;
        private string _value22;
        private string _value23;
        private string _value24;
        private string _value25;
        private string _value26;
        private string _value27;

        #endregion

        #region  attri

        public string ZRtuId
        {
            get { return _value1; }
            set
            {
                if (_value1 == value) return;
                _value1 = value;
                RaisePropertyChanged(() => ZRtuId);
            }
        }

        public string ZAlarmDelay
        {
            get { return _value2; }
            set
            {
                if (_value2 == value) return;
                _value2 = value;
                RaisePropertyChanged(() => ZAlarmDelay);
            }
        }

        public string ZCloseTime
        {
            get { return _value3; }
            set
            {
                if (_value3 == value) return;
                _value3 = value;
                RaisePropertyChanged(() => ZCloseTime);
            }
        }

        public string ZOpenTime
        {
            get { return _value4; }
            set
            {
                if (_value4 == value) return;
                _value4 = value;
                RaisePropertyChanged(() => ZOpenTime);
            }
        }

        public string ZCommTypeCode
        {
            get { return _value5; }
            set
            {
                if (_value5 == value) return;
                _value5 = value;
                RaisePropertyChanged(() => ZCommTypeCode);
            }
        }

        public string ZCtRadioA
        {
            get { return _value6; }
            set
            {
                if (_value6 == value) return;
                _value6 = value;
                RaisePropertyChanged(() => ZCtRadioA);
            }
        }

        public string ZCtRadioB
        {
            get { return _value7; }
            set
            {
                if (_value7 == value) return;
                _value7 = value;
                RaisePropertyChanged(() => ZCtRadioB);
            }
        }

        public string ZCtRadioC
        {
            get { return _value8; }
            set
            {
                if (_value8 == value) return;
                _value8 = value;
                RaisePropertyChanged(() => ZCtRadioC);
            }
        }

        public string ZEnerySaveTemp
        {
            get { return _value9; }
            set
            {
                if (_value9 == value) return;
                _value9 = value;
                RaisePropertyChanged(() => ZEnerySaveTemp);
            }
        }

        public string ZFanClosedTemp
        {
            get { return _value10; }
            set
            {
                if (_value10 == value) return;
                _value10 = value;
                RaisePropertyChanged(() => ZFanClosedTemp);
            }
        }

        public string ZFanSatrtTemp
        {
            get { return _value11; }
            set
            {
                if (_value11 == value) return;
                _value11 = value;
                RaisePropertyChanged(() => ZFanSatrtTemp);
            }
        }

        public string ZInputOvervoltageLimit
        {
            get { return _value12; }
            set
            {
                if (_value12 == value) return;
                _value12 = value;
                RaisePropertyChanged(() => ZInputOvervoltageLimit);
            }
        }

        public string ZInputUndervoltageLimit
        {
            get { return _value13; }
            set
            {
                if (_value13 == value) return;
                _value13 = value;
                RaisePropertyChanged(() => ZInputUndervoltageLimit);
            }
        }

        public string ZMandatoryProtectTemp
        {
            get { return _value14; }
            set
            {
                if (_value14 == value) return;
                _value14 = value;
                RaisePropertyChanged(() => ZMandatoryProtectTemp);
            }
        }

        public string ZMode
        {
            get { return _value15; }
            set
            {
                if (_value15 == value) return;
                _value15 = value;
                RaisePropertyChanged(() => ZMode);
            }
        }

        public string ZOutputOverloadLimit
        {
            get { return _value16; }
            set
            {
                if (_value16 == value) return;
                _value16 = value;
                RaisePropertyChanged(() => ZOutputOverloadLimit);
            }
        }

        public string ZOutputUndervoltageLimit
        {
            get { return _value17; }
            set
            {
                if (_value17 == value) return;
                _value17 = value;
                RaisePropertyChanged(() => ZOutputUndervoltageLimit);
            }
        }

        public string ZPowerSupplyPhases
        {
            get { return _value18; }
            set
            {
                if (_value18 == value) return;
                _value18 = value;
                RaisePropertyChanged(() => ZPowerSupplyPhases);
            }
        }

        public string ZPreheatingTime
        {
            get { return _value19; }
            set
            {
                if (_value19 == value) return;
                _value19 = value;
                RaisePropertyChanged(() => ZPreheatingTime);
            }
        }

        public string ZRecoverEnergySaveTemp
        {
            get { return _value20; }
            set
            {
                if (_value20 == value) return;
                _value20 = value;
                RaisePropertyChanged(() => ZRecoverEnergySaveTemp);
            }
        }

        public string ZRegulatingSpeed
        {
            get { return _value21; }
            set
            {
                if (_value21 == value) return;
                _value21 = value;
                RaisePropertyChanged(() => ZRegulatingSpeed);
            }
        }

        public string ZRunMode
        {
            get { return _value22; }
            set
            {
                if (_value22 == value) return;
                _value22 = value;
                RaisePropertyChanged(() => ZRunMode);
            }
        }

        public string ZTimeMode
        {
            get { return _value23; }
            set
            {
                if (_value23 == value) return;
                _value23 = value;
                RaisePropertyChanged(() => ZTimeMode);
            }
        }

        public string ZWorkMode
        {
            get { return _value24; }
            set
            {
                if (_value24 == value) return;
                _value24 = value;
                RaisePropertyChanged(() => ZWorkMode);
            }
        }

        public string ZEsyValidIdentify
        {
            get { return _value25; }
            set
            {
                if (_value25 == value) return;
                _value25 = value;
                RaisePropertyChanged(() => ZEsyValidIdentify);
            }
        }

        public string ZIsActiveAlarm
        {
            get { return _value26; }
            set
            {
                if (_value26 == value) return;
                _value26 = value;
                RaisePropertyChanged(() => ZIsActiveAlarm);
            }
        }

        public string ZVersion
        {
            get { return _value27; }
            set
            {
                if (_value27 == value) return;
                _value27 = value;
                RaisePropertyChanged(() => ZVersion);
            }
        }

        #endregion

        #region Time

        private ObservableCollection<NameValueInt> _zcjd601ParItems;

        public ObservableCollection<NameValueInt> ZcJd601ParItems
        {
            get
            {
                if (_zcjd601ParItems == null)
                {
                    _zcjd601ParItems = new ObservableCollection<NameValueInt>();
                    for (int i = 1; i < 9; i++)
                    {
                        _zcjd601ParItems.Add(new NameValueInt
                                                 {
                            Value = i,
                            Name = "--"
                        });
                    }
                }
                return _zcjd601ParItems;
            }
        }

        public void OnLoadZcJd601ParVm()
        {
            ZcJd601ParItems.Clear();
            for (int i = 1; i < 9; i++)
            {
                ZcJd601ParItems.Add(new NameValueInt
                                        {
                    Value = i,
                    Name = "--"
                });

            }
        }


        #endregion
        private Wlst.Sr.EquipmentInfoHolding.Model.Wj601Esu _terminalInformation;
        public void OnInitZc()
        {
           // zRtuId = _terminalInformation==null ? "--" : _terminalInformation.RtuId.ToString(CultureInfo.InvariantCulture);
            ZAlarmDelay = "--";
            ZCloseTime = "--";
            ZOpenTime = "--";
            ZCommTypeCode = "--";
            ZCtRadioA = "--";
            ZCtRadioB = "--";
            ZCtRadioC = "--";
            ZEnerySaveTemp = "--";
            ZFanClosedTemp = "--";
            ZFanSatrtTemp = "--";
            ZInputOvervoltageLimit = "--";
            ZInputUndervoltageLimit = "--";
            ZMandatoryProtectTemp = "--";
            ZMode = "--";
            ZOutputOverloadLimit = "--";
            ZOutputUndervoltageLimit = "--";
            ZPowerSupplyPhases = "--";
            ZPreheatingTime = "--";
            ZRecoverEnergySaveTemp = "--";
            ZRegulatingSpeed = "--";
            ZRunMode = "--";
            ZTimeMode = "--";
            ZWorkMode = "--";
            ZEsyValidIdentify = "--";
            ZIsActiveAlarm = "--";
            ZVersion = "--";
        }

        //#region visi

        //private Visibility canzcvisi;

        //public Visibility ZcVisi
        //{
        //    get { return canzcvisi; }
        //    set
        //    {
        //        if (canzcvisi == value) return;
        //        canzcvisi = value;
        //        RaisePropertyChanged(() => ZcVisi);
        //    }
        //}

        //#endregion

        //#region CmdZcVisi

        //private void ExCmdZcVisi()
        //{
        //    ZcVisi = ZcVisi != Visibility.Visible ? Visibility.Visible : Visibility.Collapsed;

        //    OnInitZc();
        //    OnLoadZcJd601ParVm();
        //}

        //private bool CanCmdZcVisi()
        //{
        //    if (_terminalInformation != null)
        //        return true;
        //    return false;
        //}

        //private ICommand _relayCmdZcVisi;

        ///// <summary>
        /////   
        ///// </summary>
        //public ICommand CmdZcVisi
        //{
        //    get { return _relayCmdZcVisi ?? (_relayCmdZcVisi = new RelayCommand(ExCmdZcVisi, CanCmdZcVisi, false)); }
        //}

        //#endregion

        private readonly DateTime[] _dateTimes=new DateTime[3];

        #region CmdZcPar

        private void ExCmdZcPar()
        {
            _dateTimes[0] = DateTime.Now;
            ZcJd601Par();
        }

        private bool CanCmdZcPar()
        {
            if (_terminalInformation != null)
                return DateTime.Now.Ticks-_dateTimes[0].Ticks>30000000;
            return false;
        }

        private ICommand _relaCmdZcPar;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcPar
        {
            get { return _relaCmdZcPar ?? (_relaCmdZcPar = new RelayCommand(ExCmdZcPar, CanCmdZcPar, true)); }
        }

        #endregion

        #region CmdZcVersion

        private void ExCmdZcVersion()
        {
            _dateTimes[1] = DateTime.Now;
            ZcVersion();
        }

        private bool CanCmdZcVersion()
        {
            if (_terminalInformation != null)
                return DateTime.Now.Ticks - _dateTimes[1].Ticks > 30000000;
            return false;
        }

        private ICommand _relayCmdZcVersion;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcVersion
        {
            get
            {
                return _relayCmdZcVersion ??
                       (_relayCmdZcVersion = new RelayCommand(ExCmdZcVersion, CanCmdZcVersion, true));
            }
        }

        #endregion

        #region CmdZcTime

        private void ExCmdZcTime()
        {
            _dateTimes[2] = DateTime.Now;
            ZcJd601Time();
        }

        private bool CanCmdZcTime()
        {
            if (_terminalInformation != null)
                return DateTime.Now.Ticks - _dateTimes[2].Ticks > 30000000;
            return false;
        }

        private ICommand _relayCmdZcTime;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcTime
        {
            get { return _relayCmdZcTime ?? (_relayCmdZcTime = new RelayCommand(ExCmdZcTime, CanCmdZcTime, true)); }
        }

        #endregion

        private void UpdateZcVersion(string version)
        {
            ZVersion = version;
        }

        private void UpdateZcPar(ExchangeZcParReply.Jd601ZcParInfo info)
        {

            ZAlarmDelay = info.AlarmDelay + " 秒";
            int hour = info.CloseTime / 60;
            int minute = info.CloseTime % 60;
            ZCloseTime = hour + ":" + minute;
            hour = info.OpenTime / 60;
            minute = info.OpenTime % 60;
            ZOpenTime = hour + ":" + minute;
            ZCommTypeCode = info.CommTypeCode ==0 ?  "独立通信":"终端通信" ;
            ZCtRadioA = info.CtRadioA.ToString(CultureInfo.InvariantCulture);
            ZCtRadioB = info.CtRadioB.ToString(CultureInfo.InvariantCulture);
            ZCtRadioC = info.CtRadioC.ToString(CultureInfo.InvariantCulture);
            ZEnerySaveTemp = info.EnerySaveTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            ZFanClosedTemp = info.FanClosedTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            ZFanSatrtTemp = info.FanSatrtTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            ZInputOvervoltageLimit = info.InputOvervoltageLimit.ToString(CultureInfo.InvariantCulture) + "[V]";
            ZInputUndervoltageLimit = info.InputUndervoltageLimit.ToString(CultureInfo.InvariantCulture) + "[V]";
            ZMandatoryProtectTemp = info.MandatoryProtectTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            ZMode = info.Mode == 1 ? "IGBT模式" : "接触器模式";
            ZOutputOverloadLimit = info.OutputOverloadLimit.ToString(CultureInfo.InvariantCulture) + "[A]";
            ZOutputUndervoltageLimit = info.OutputUndervoltageLimit.ToString(CultureInfo.InvariantCulture) + "[V]";
            ZPowerSupplyPhases = info.PowerSupplyPhases.ToString(CultureInfo.InvariantCulture);
            ZPreheatingTime = info.PreheatingTime.ToString(CultureInfo.InvariantCulture) + "[分]";
            ZRecoverEnergySaveTemp = info.RecoverEnergySaveTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            ZRegulatingSpeed = info.RegulatingSpeed.ToString(CultureInfo.InvariantCulture) + " 秒";
            ZRunMode = info.RunMode == 0? "自动节能" : "手动节能";
            ZTimeMode = info.TimeMode ==1 ? "延时模式" : "定时模式";
            ZWorkMode = info.WorkMode == 0 ? "通用模式" : "特殊模式";
            ZEsyValidIdentify = "是";
            ZIsActiveAlarm = info.IsActiveAlarm ? "是" : "否";
        }

        private void UpdateZcTime(ExchangeZcTimeReply.Jd601ZcTimeInfo info)
        {

            ZcJd601ParItems[0].Name = "" + info.Value1 + "  " + (info.Time1 / 60) + ":" + (info.Time1 % 60);
            ZcJd601ParItems[1].Name = "" + info.Value2 + "  " + (info.Time2 / 60) + ":" + (info.Time2 % 60);
            ZcJd601ParItems[2].Name = "" + info.Value3 + "  " + (info.Time3 / 60) + ":" + (info.Time3 % 60);
            ZcJd601ParItems[3].Name = "" + info.Value4 + "  " + (info.Time4 / 60) + ":" + (info.Time4 % 60);
            ZcJd601ParItems[4].Name = "" + info.Value5 + "  " + (info.Time5 / 60) + ":" + (info.Time5 % 60);
            ZcJd601ParItems[5].Name = "" + info.Value6 + "  " + (info.Time6 / 60) + ":" + (info.Time6 % 60);
            ZcJd601ParItems[6].Name = "" + info.Value7 + "  " + (info.Time7 / 60) + ":" + (info.Time7 % 60);
            ZcJd601ParItems[7].Name = "" + info.Value8 + "  " + (info.Time8 / 60) + ":" + (info.Time8 % 60);

        }

    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class TmlInfoSetZcForjd601ViewModel
    {

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_zc_para ,//.ClientPart.wlst_Jd601_server_ans_clinet_order_ZcJd601Par,
                ZcJd601ParAction,
                typeof(TmlInfoSetZcForjd601ViewModel), this);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_zc_time ,//.ClientPart.wlst_Jd601_server_ans_clinet_order_ZcJd601Time,
                ZcJd601TimeAction,
                typeof(TmlInfoSetZcForjd601ViewModel), this);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_zc_version ,//.ClientPart.wlst_Jd601_server_ans_clinet_order_ZcJd601Version,
                ZcJd601VersionAction,
                typeof(TmlInfoSetZcForjd601ViewModel), this);
        }

        public void ZcJd601ParAction(string session,Wlst .mobile .MsgWithMobile  args)
        {
            var infos = args.WstSvrAnsCntRequestJd601ZcPar ;// GetParams()[1] as Models.Exchange.ExchangeZcParReply;
            if (infos == null) return;
            if (_terminalInformation == null) return;
            if (infos.RtuId != _terminalInformation.RtuId) return;
            UpdateZcPar(infos.Info);
        }
        public void ZcJd601TimeAction(string session, Wlst .mobile .MsgWithMobile args)
        {
            var infos = args.WstSvrAnsCntRequestJd601ZcTime ;//.GetParams()[1] as Models.Exchange.ExchangeZcTimeReply;
            if (infos == null) return;
            if (_terminalInformation == null) return;
            if (infos.RtuId != _terminalInformation.RtuId) return;
            UpdateZcTime(infos.Info);
        }

        public void ZcJd601VersionAction(string session, Wlst.mobile.MsgWithMobile args)
        {
            var infos = args.WstSvrAnsCntRequestJd601ZcVersion ;
            if (infos == null) return;
            if (_terminalInformation == null) return;
            if (infos.RtuId != _terminalInformation.RtuId) return;
            UpdateZcVersion(infos.Info);
        }
    }
    /// <summary>
    /// Socket
    /// </summary>
    public partial class TmlInfoSetZcForjd601ViewModel
    {
        private void ZcJd601Par()
        {
            if (_terminalInformation == null) return;
            var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_zc_para ;//.ServerPart.wlst_Jd601_clinet_order_ZcJd601Par;
            nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion .RtuId = _terminalInformation.RtuId;
            nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion.AttachRtuId = _terminalInformation.RtuFid;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }

        private void ZcVersion()
        {
            if (_terminalInformation == null) return;

            var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_zc_version ;//.ServerPart.wlst_Jd601_clinet_order_ZcJd601Version;
            nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion.RtuId = _terminalInformation.RtuId;
            nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion.AttachRtuId = _terminalInformation.RtuFid;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }

        private void ZcJd601Time()
        {
            if (_terminalInformation == null) return;
            var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_zc_time ;//.ServerPart.wlst_Jd601_clinet_order_ZcJd601Time;
            nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion.RtuId = _terminalInformation.RtuId;
            nt.WstCntRequestJd601ZcDataOrParOrTimeOrVersion.AttachRtuId = _terminalInformation.RtuFid;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }

    }
}
