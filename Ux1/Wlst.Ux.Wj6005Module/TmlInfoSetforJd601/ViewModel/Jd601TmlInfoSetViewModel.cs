using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;
using Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Jd601;
using Wlst.Sr.ProtocolCnt.AexchangeModels.ModelEnum;
using Wlst.Sr.ProtocolCnt.Jd601;
using Wlst.Ux.Wj6005Module.Models.BaseViewModel;
using Wlst.Ux.Wj6005Module.TmlInfoSetforJd601.Services;

namespace Wlst.Ux.Wj6005Module.TmlInfoSetforJd601.ViewModel
{
    [Export(typeof (IIJd601TmlInfoSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601TmlInfoSetViewModel : Jd601AdjustInfoViewModel, IIJd601TmlInfoSet
    {
        public Jd601TmlInfoSetViewModel()
        {
            this.InitEvent();
            this.InitAction();
        }

        #region tab

        /// <summary>
        /// <c>True</c> if this instance can pin; otherwise, <c>False</c>.
        /// 是否可锁定
        /// </summary>
        public bool CanUserPin
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this pane can float; otherwise, <c>false</c>.
        /// 是否可悬浮
        /// </summary>
        public bool CanFloat
        {
            get { return true; }
        }

        /// <summary>
        /// <c>True</c> if this instance can dock in the document host; otherwise, <c>false</c>.
        /// 是否可移动至document host
        /// </summary>
        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanClose
        {
            get { return true; }
        }

        public string Title
        {
            get { return "参数设置"; }
        }

        #endregion

        #region NavOnLoad

        public void NavOnLoad(params object[] parsObjects)
        {
            var tmlId = (int) parsObjects[0];
            if (tmlId > 0)
            {
                SelectedTmlChange(tmlId);
                RequestJd601Pars(tmlId);
            }
            ZcVisi = Visibility.Collapsed;
        }


        private Jd601TerminalInformation _terminalInformation = null;

        /// <summary>
        /// 提供外界更改终端
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        public void SelectedTmlChange(int rtuId)
        {
            if (
                !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary .
                     ContainsKey(rtuId)) return;
            var t =
                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary [
                    rtuId]
                     as Jd601TerminalInformation;

            if (t == null) return;

            var ffff = t.Clone();
            _terminalInformation = ffff as Jd601TerminalInformation;
            if (_terminalInformation != null)
            {
                this.UpdateVmInfo(_terminalInformation);
                this.AttachRtuId = _terminalInformation.AttachRtuId;
            }
            else
            {
                this.AttachRtuId = 0;
                this.AttachRtuName = "无法解析该设备连接的终端";
            }
            this.OnLoadJd601ParVm();
            Thread.Sleep(100);
            this.RequestJd601Pars(this.RtuId);

        }



        /// <summary>
        /// 将回路信息、输入信、输出信息还原为 终端信息
        /// </summary>
        /// <returns></returns>
        private Jd601TerminalInformation BackViewModelToTerminalInformation()
        {
            var info = this.GetSetIIJd601();
            _terminalInformation.PhyId = info.PhyId;
            _terminalInformation.RtuId = info.RtuId;
            _terminalInformation.RtuName = info.RtuName;
            _terminalInformation.AlarmDelay = info.AlarmDelay;
            _terminalInformation.CloseTime = info.CloseTime;
            _terminalInformation.CommTypeCode = info.CommTypeCode;
            _terminalInformation.CtRadioA = info.CtRadioA;
            _terminalInformation.CtRadioB = info.CtRadioB;
            _terminalInformation.CtRadioC = info.CtRadioC;
            _terminalInformation.EnerySaveTemp = info.EnerySaveTemp;
            _terminalInformation.FanClosedTemp = info.FanClosedTemp;
            _terminalInformation.FanSatrtTemp = info.FanSatrtTemp;
            _terminalInformation.InputOvervoltageLimit = info.InputOvervoltageLimit;
            _terminalInformation.InputUndervoltageLimit = info.InputUndervoltageLimit;
            _terminalInformation.MandatoryProtectTemp = info.MandatoryProtectTemp;
            _terminalInformation.Mode = info.Mode;
            _terminalInformation.OutputOverloadLimit = info.OutputOverloadLimit;
            _terminalInformation.OutputUndervoltageLimit = info.OutputUndervoltageLimit;
            _terminalInformation.PowerSupplyPhases = info.PowerSupplyPhases;
            _terminalInformation.PreheatingTime = info.PreheatingTime;
            _terminalInformation.RecoverEnergySaveTemp = info.RecoverEnergySaveTemp;
            _terminalInformation.RegulatingSpeed = info.RegulatingSpeed;
            _terminalInformation.RunMode = info.RunMode;
            _terminalInformation.TimeMode = info.TimeMode;
            _terminalInformation.WorkMode = info.WorkMode;
            _terminalInformation.EsyValidIdentify = info.EsyValidIdentify;
            _terminalInformation.IsActiveAlarm = info.IsActiveAlarm;
            return _terminalInformation;
        }

        #endregion

        #region

        private int _attachRtuId;

        /// <summary>
        /// 如果连接终端 则终端地址  不允许修改
        /// </summary>
        public int AttachRtuId
        {
            get { return _attachRtuId; }
            set
            {
                if (value != _attachRtuId)
                {
                    _attachRtuId = value;
                    this.RaisePropertyChanged(() => this.AttachRtuId);


                    var t =
                        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentInfo(value);

                    if (t == null)
                    {

                        AttachRtuName = "设备" + AttachRtuName;
                    }
                    else
                    {
                        AttachRtuName = t.RtuName;
                    }
                }
            }
        }


        private string _attachRtuName;

        /// <summary>
        /// 如果连接终端 则终端地址  不允许修改
        /// </summary>
        public string AttachRtuName
        {
            get { return _attachRtuName; }
            set
            {
                if (_attachRtuName == value) return;
                _attachRtuName = value;
                this.RaisePropertyChanged(() => this.AttachRtuName);
            }
        }

        #endregion

        #region CmdSave

        private void ExSave()
        {
            this.UpdateInfo();
        }

        private bool CanSave()
        {
            if (_terminalInformation != null)
                return true;
            return false;
        }

        private ICommand _relayCommandUp;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSave
        {
            get { return _relayCommandUp ?? (_relayCommandUp = new RelayCommand(ExSave, CanSave, true)); }
        }

        #endregion
    }


    /// <summary>
    /// 召测参数
    /// </summary>
    public partial class Jd601TmlInfoSetViewModel
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

        public string zRtuId
        {
            get { return _value1; }
            set
            {
                if (_value1 == value) return;
                _value1 = value;
                this.RaisePropertyChanged(() => this.zRtuId);
            }
        }

        public string zAlarmDelay
        {
            get { return _value2; }
            set
            {
                if (_value2 == value) return;
                _value2 = value;
                this.RaisePropertyChanged(() => this.zAlarmDelay);
            }
        }

        public string zCloseTime
        {
            get { return _value3; }
            set
            {
                if (_value3 == value) return;
                _value3 = value;
                this.RaisePropertyChanged(() => this.zCloseTime);
            }
        }

        public string zOpenTime
        {
            get { return _value4; }
            set
            {
                if (_value4 == value) return;
                _value4 = value;
                this.RaisePropertyChanged(() => this.zOpenTime);
            }
        }

        public string zCommTypeCode
        {
            get { return _value5; }
            set
            {
                if (_value5 == value) return;
                _value5 = value;
                this.RaisePropertyChanged(() => this.zCommTypeCode);
            }
        }

        public string zCtRadioA
        {
            get { return _value6; }
            set
            {
                if (_value6 == value) return;
                _value6 = value;
                this.RaisePropertyChanged(() => this.zCtRadioA);
            }
        }

        public string zCtRadioB
        {
            get { return _value7; }
            set
            {
                if (_value7 == value) return;
                _value7 = value;
                this.RaisePropertyChanged(() => this.zCtRadioB);
            }
        }

        public string zCtRadioC
        {
            get { return _value8; }
            set
            {
                if (_value8 == value) return;
                _value8 = value;
                this.RaisePropertyChanged(() => this.zCtRadioC);
            }
        }

        public string zEnerySaveTemp
        {
            get { return _value9; }
            set
            {
                if (_value9 == value) return;
                _value9 = value;
                this.RaisePropertyChanged(() => this.zEnerySaveTemp);
            }
        }

        public string zFanClosedTemp
        {
            get { return _value10; }
            set
            {
                if (_value10 == value) return;
                _value10 = value;
                this.RaisePropertyChanged(() => this.zFanClosedTemp);
            }
        }

        public string zFanSatrtTemp
        {
            get { return _value11; }
            set
            {
                if (_value11 == value) return;
                _value11 = value;
                this.RaisePropertyChanged(() => this.zFanSatrtTemp);
            }
        }

        public string zInputOvervoltageLimit
        {
            get { return _value12; }
            set
            {
                if (_value12 == value) return;
                _value12 = value;
                this.RaisePropertyChanged(() => this.zInputOvervoltageLimit);
            }
        }

        public string zInputUndervoltageLimit
        {
            get { return _value13; }
            set
            {
                if (_value13 == value) return;
                _value13 = value;
                this.RaisePropertyChanged(() => this.zInputUndervoltageLimit);
            }
        }

        public string zMandatoryProtectTemp
        {
            get { return _value14; }
            set
            {
                if (_value14 == value) return;
                _value14 = value;
                this.RaisePropertyChanged(() => this.zMandatoryProtectTemp);
            }
        }

        public string zMode
        {
            get { return _value15; }
            set
            {
                if (_value15 == value) return;
                _value15 = value;
                this.RaisePropertyChanged(() => this.zMode);
            }
        }

        public string zOutputOverloadLimit
        {
            get { return _value16; }
            set
            {
                if (_value16 == value) return;
                _value16 = value;
                this.RaisePropertyChanged(() => this.zOutputOverloadLimit);
            }
        }

        public string zOutputUndervoltageLimit
        {
            get { return _value17; }
            set
            {
                if (_value17 == value) return;
                _value17 = value;
                this.RaisePropertyChanged(() => this.zOutputUndervoltageLimit);
            }
        }

        public string zPowerSupplyPhases
        {
            get { return _value18; }
            set
            {
                if (_value18 == value) return;
                _value18 = value;
                this.RaisePropertyChanged(() => this.zPowerSupplyPhases);
            }
        }

        public string zPreheatingTime
        {
            get { return _value19; }
            set
            {
                if (_value19 == value) return;
                _value19 = value;
                this.RaisePropertyChanged(() => this.zPreheatingTime);
            }
        }

        public string zRecoverEnergySaveTemp
        {
            get { return _value20; }
            set
            {
                if (_value20 == value) return;
                _value20 = value;
                this.RaisePropertyChanged(() => this.zRecoverEnergySaveTemp);
            }
        }

        public string zRegulatingSpeed
        {
            get { return _value21; }
            set
            {
                if (_value21 == value) return;
                _value21 = value;
                this.RaisePropertyChanged(() => this.zRegulatingSpeed);
            }
        }

        public string zRunMode
        {
            get { return _value22; }
            set
            {
                if (_value22 == value) return;
                _value22 = value;
                this.RaisePropertyChanged(() => this.zRunMode);
            }
        }

        public string zTimeMode
        {
            get { return _value23; }
            set
            {
                if (_value23 == value) return;
                _value23 = value;
                this.RaisePropertyChanged(() => this.zTimeMode);
            }
        }

        public string zWorkMode
        {
            get { return _value24; }
            set
            {
                if (_value24 == value) return;
                _value24 = value;
                this.RaisePropertyChanged(() => this.zWorkMode);
            }
        }

        public string zEsyValidIdentify
        {
            get { return _value25; }
            set
            {
                if (_value25 == value) return;
                _value25 = value;
                this.RaisePropertyChanged(() => this.zEsyValidIdentify);
            }
        }

        public string zIsActiveAlarm
        {
            get { return _value26; }
            set
            {
                if (_value26 == value) return;
                _value26 = value;
                this.RaisePropertyChanged(() => this.zIsActiveAlarm);
            }
        }

        public string zVersion
        {
            get { return _value27; }
            set
            {
                if (_value27 == value) return;
                _value27 = value;
                this.RaisePropertyChanged(() => this.zVersion);
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
                        _zcjd601ParItems.Add(new NameValueInt()
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
                ZcJd601ParItems.Add(new NameValueInt()
                                        {
                                            Value = i,
                                            Name = "--"
                                        });

            }
        }


        #endregion

        public void OnInitZc()
        {
            zRtuId = this.RtuId.ToString(CultureInfo.InvariantCulture);
            zAlarmDelay = "--";
            zCloseTime = "--";
            zOpenTime = "--";
            zCommTypeCode = "--";
            zCtRadioA = "--";
            zCtRadioB = "--";
            zCtRadioC = "--";
            zEnerySaveTemp = "--";
            zFanClosedTemp = "--";
            zFanSatrtTemp = "--";
            zInputOvervoltageLimit = "--";
            zInputUndervoltageLimit = "--";
            zMandatoryProtectTemp = "--";
            zMode = "--";
            zOutputOverloadLimit = "--";
            zOutputUndervoltageLimit = "--";
            zPowerSupplyPhases = "--";
            zPreheatingTime = "--";
            zRecoverEnergySaveTemp = "--";
            zRegulatingSpeed = "--";
            zRunMode = "--";
            zTimeMode = "--";
            zWorkMode = "--";
            zEsyValidIdentify = "--";
            zIsActiveAlarm = "--";
            zVersion = "--";
        }

        #region visi

        private Visibility canzcvisi;

        public Visibility ZcVisi
        {
            get { return canzcvisi; }
            set
            {
                if (canzcvisi == value) return;
                canzcvisi = value;
                this.RaisePropertyChanged(() => this.ZcVisi);
            }
        }

        #endregion

        #region CmdZcVisi

        private void ExCmdZcVisi()
        {
            if (ZcVisi != Visibility.Visible) ZcVisi = Visibility.Visible;
            else ZcVisi = Visibility.Collapsed;

            OnInitZc();
            OnLoadZcJd601ParVm();
        }

        private bool CanCmdZcVisi()
        {
            if (_terminalInformation != null)
                return true;
            return false;
        }

        private ICommand _relayCmdZcVisi;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcVisi
        {
            get { return _relayCmdZcVisi ?? (_relayCmdZcVisi = new RelayCommand(ExCmdZcVisi, CanCmdZcVisi, false)); }
        }

        #endregion

        #region CmdZcPar

        private void ExCmdZcPar()
        {
            this.ZcJd601Par(_terminalInformation.RtuId);
        }

        private bool CanCmdZcPar()
        {
            if (_terminalInformation != null)
                return true;
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
            this.ZcVersion(_terminalInformation.RtuId);
        }

        private bool CanCmdZcVersion()
        {
            if (_terminalInformation != null)
                return true;
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
            this.ZcJd601Time(_terminalInformation.RtuId);
        }

        private bool CanCmdZcTime()
        {
            if (_terminalInformation != null)
                return true;
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
            this.zVersion = version;
        }

        private void UpdateZcPar(Jd601ZcParInfo info)
        {

            zAlarmDelay = info.AlarmDelay + " 秒";
            int hour = info.CloseTime/60;
            int minute = info.CloseTime%60;
            zCloseTime = hour + ":" + minute;
            hour = info.OpenTime/60;
            minute = info.OpenTime%60;
            zOpenTime = hour + ":" + minute;
            zCommTypeCode = info.CommTypeCode == EsyCommTypeCode.ThrouthRtu ? "终端通信" : "独立通信";
            zCtRadioA = info.CtRadioA.ToString(CultureInfo.InvariantCulture);
            zCtRadioB = info.CtRadioB.ToString(CultureInfo.InvariantCulture);
            zCtRadioC = info.CtRadioC.ToString(CultureInfo.InvariantCulture);
            zEnerySaveTemp = info.EnerySaveTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            zFanClosedTemp = info.FanClosedTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            zFanSatrtTemp = info.FanSatrtTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            zInputOvervoltageLimit = info.InputOvervoltageLimit.ToString(CultureInfo.InvariantCulture) + "[V]";
            zInputUndervoltageLimit = info.InputUndervoltageLimit.ToString(CultureInfo.InvariantCulture) + "[V]";
            zMandatoryProtectTemp = info.MandatoryProtectTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            zMode = info.Mode == EsyMode.IGBT ? "IGBT模式" : "接触器模式";
            zOutputOverloadLimit = info.OutputOverloadLimit.ToString(CultureInfo.InvariantCulture) + "[A]";
            zOutputUndervoltageLimit = info.OutputUndervoltageLimit.ToString(CultureInfo.InvariantCulture) + "[V]";
            zPowerSupplyPhases = info.PowerSupplyPhases.ToString(CultureInfo.InvariantCulture);
            zPreheatingTime = info.PreheatingTime.ToString(CultureInfo.InvariantCulture) + "[分]";
            zRecoverEnergySaveTemp = info.RecoverEnergySaveTemp.ToString(CultureInfo.InvariantCulture) + "[C]";
            zRegulatingSpeed = info.RegulatingSpeed.ToString(CultureInfo.InvariantCulture) + " 秒";
            zRunMode = info.RunMode == EsyRunMode.Auto ? "自动节能" : "手动节能";
            zTimeMode = info.TimeMode == EsyTimeMode.Delaying ? "延时模式" : "时间模式";
            zWorkMode = info.WorkMode == EsyWorkMode.CommonMode ? "通用模式" : "特殊模式";
            zEsyValidIdentify = "是";
            zIsActiveAlarm = info.IsActiveAlarm ? "是" : "否";
        }

        private void UpdateZcTime(Jd601ZcTimeInfo info)
        {

            ZcJd601ParItems[0].Name = "" + info.Value1 + "  " + (info.Time1/60) + ":" + (info.Time1%60);
            ZcJd601ParItems[1].Name = "" + info.Value2 + "  " + (info.Time2/60) + ":" + (info.Time2%60);
            ZcJd601ParItems[2].Name = "" + info.Value3 + "  " + (info.Time3/60) + ":" + (info.Time3%60);
            ZcJd601ParItems[3].Name = "" + info.Value4 + "  " + (info.Time4/60) + ":" + (info.Time4%60);
            ZcJd601ParItems[4].Name = "" + info.Value5 + "  " + (info.Time5/60) + ":" + (info.Time5%60);
            ZcJd601ParItems[5].Name = "" + info.Value6 + "  " + (info.Time6/60) + ":" + (info.Time6%60);
            ZcJd601ParItems[6].Name = "" + info.Value7 + "  " + (info.Time7/60) + ":" + (info.Time7%60);
            ZcJd601ParItems[7].Name = "" + info.Value8 + "  " + (info.Time8/60) + ":" + (info.Time8%60);

        }

    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class Jd601TmlInfoSetViewModel
    {
        public void InitEvent()
        {
            this.AddEventFilterInfo(Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId ,
                                    PublishEventType.Core);

        }

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_request_Pars,
                RequestOrUpdateJd601Parsinfo,
                typeof(Jd601TmlInfoSetViewModel), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_update_Pars,
                RequestOrUpdateJd601Parsinfo,
                typeof(Jd601TmlInfoSetViewModel), this);

            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_order_ZcJd601Par,
                ZcJd601Par,
                typeof(Jd601TmlInfoSetViewModel), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_order_ZcJd601Time,
                ZcJd601Time,
                typeof(Jd601TmlInfoSetViewModel), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_order_ZcJd601Version,
                ZcJd601Version,
                typeof(Jd601TmlInfoSetViewModel), this);
        }

        public void RequestOrUpdateJd601Parsinfo(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ExchangeReplyEsyPar> args)
        {
            var infos = args.Data;
            if (infos == null) return;
            if (_terminalInformation == null) return;
            if (infos.RtuId != _terminalInformation.RtuId) return;
            this.UpdateJd601Vm(infos.Info);
        }

        public void ZcJd601Par(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ExchangeZcParReply> args)
        {
            var infos = args.Data;// GetParams()[1] as Models.Exchange.ExchangeZcParReply;
            if (infos == null) return;
            if (_terminalInformation == null) return;
            if (infos.RtuId != _terminalInformation.RtuId) return;
            this.UpdateZcPar(infos.Info); ;
        }
        public void ZcJd601Time(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ExchangeZcTimeReply> args)
        {
            var infos = args.Data ;//.GetParams()[1] as Models.Exchange.ExchangeZcTimeReply;
            if (infos == null) return;
            if (_terminalInformation == null) return;
            if (infos.RtuId != _terminalInformation.RtuId) return;
            this.UpdateZcTime(infos.Info);
        }
        public void ZcJd601Version(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ExchangeZcVersionReply> args)
        {
            var infos = args.Data;
            if (infos == null) return;
            if (_terminalInformation == null) return;
            if (infos.RtuId != _terminalInformation.RtuId) return;
            this.UpdateZcVersion(infos.Info);
        }


        public override void ExPublishedEvent(
            Microsoft.Practices.Prism.MefExtensions.Event.EventHelper.PublishEventArgs args)
        {
            try
            {
                if (_terminalInformation == null) return;
                if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                {
                    var attachlst = args.GetParams()[0] as List<Tuple<int, int>>;
                    if (attachlst == null) return;
                    if (attachlst.Any(g => g.Item1 == _terminalInformation.RtuId))
                    {
                        this.SelectedTmlChange(_terminalInformation.RtuId);
                    }
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class Jd601TmlInfoSetViewModel
    {
        private void UpdateInfo()
        {
            if (_terminalInformation == null) return;
            ////EqipentInfoExchange info = new EqipentInfoExchange();
            ////var ggg =
            ////    Wlst.Sr.EquipmentInfoHolding.ProtcolModels.ModelsConvert.EquipmentInfoSvrConvert.ConvetWjInfoToSvrInfo(
            ////        _terminalInformation);
            ////if(ggg ==null ) return;
            ////info.LstInfo.Add(ggg );
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;

            //Models.Exchange.ExchangeReplyEsyPar infos = new ExchangeReplyEsyPar() {RtuId = _terminalInformation.RtuId};
            //foreach (var t in this.BackJd601Par())
            //{
            //    infos.Info.Add(t);
            //}
            ////先发送时间信息  再更新终端参数  否则在服务端执行参数下发时可能下发时间未0的信息
            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.UpdateJd601Pars, null,
            //                        infos, waitIdUpdate, 10, 6);

            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_update_Pars;
            nt.Data.RtuId = _terminalInformation.RtuId;
            foreach (var t in this.BackJd601Par())
            {
                nt.Data.Info.Add(t);
            }
            SndOrderServer.OrderSnd(nt, 10, 3);

            Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.UpdateEquipmentInfo(
                BackViewModelToTerminalInformation());

            //Thread.Sleep(100);
            // waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            // SndOrderServer.OrderSnd(EventIdAssign.AttachEquipmentUpdateEventId, null,
            //                         info, waitIdUpdate, 10, 6);


        }


        private void RequestJd601Pars(int rtuid)
        {
            if (rtuid < 0) return;
            //Models.Exchange.ExchangeRequestEsyPar info = new ExchangeRequestEsyPar()
            //                                                 {
            //                                                     RtuId = rtuid
            //                                                 };
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;

            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.RequestJd601Pars, null,
            //                        info, waitIdUpdate, 10, 6);

            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_request_Pars;
            nt.Data.RtuId = rtuid;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }


        private void ZcJd601Par(int rtuId)
        {
            if (_terminalInformation == null) return;
            //ExchangeZc info = new ExchangeZc();
            //info.AttachRtuId = _terminalInformation.AttachRtuId;
            //info.RtuId = _terminalInformation.RtuId;
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;


            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.ZcJd601Par, null,
            //                        info, waitIdUpdate, 30, 6);


            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_order_ZcJd601Par;
            nt.Data.RtuId = _terminalInformation.RtuId;
            nt.Data.AttachRtuId = _terminalInformation.AttachRtuId;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }

        private void ZcVersion(int rtuId)
        {
            if (_terminalInformation == null) return;
            //ExchangeZc info = new ExchangeZc();
            //info.AttachRtuId = _terminalInformation.AttachRtuId;
            //info.RtuId = _terminalInformation.RtuId;
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;


            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.ZcJd601Version, null,
            //                        info, waitIdUpdate, 30, 6);

            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_order_ZcJd601Version;
            nt.Data.RtuId = _terminalInformation.RtuId;
            nt.Data.AttachRtuId = _terminalInformation.AttachRtuId;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }

        private void ZcJd601Time(int rtuId)
        {
            if (_terminalInformation == null) return;
            //ExchangeZc info = new ExchangeZc();
            //info.AttachRtuId = _terminalInformation.AttachRtuId;
            //info.RtuId = _terminalInformation.RtuId;
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.ZcJd601Time, null,
            //                        info, waitIdUpdate, 30, 6);


            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_order_ZcJd601Time;
            nt.Data.RtuId = _terminalInformation.RtuId;
            nt.Data.AttachRtuId = _terminalInformation.AttachRtuId;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }
    }



}
