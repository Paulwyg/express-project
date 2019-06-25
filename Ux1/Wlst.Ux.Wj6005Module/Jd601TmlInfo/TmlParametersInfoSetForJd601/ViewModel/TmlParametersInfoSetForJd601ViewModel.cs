using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.TmlParametersInfoSetForJd601.Services;
using Wlst.Ux.Wj6005Module.Models.BaseViewModel;
using Wlst.client;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.TmlParametersInfoSetForJd601.ViewModel
{
    [Export(typeof(IITmlParametersInfoSetForJd601))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlParametersInfoSetForJd601ViewModel : Jd601AdjustInfoViewModel, IITmlParametersInfoSetForJd601
    {
        #region tab
        public int Index
        {
            get { return 1; }
        }
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
            get { return "节电参数设置"; }
        }

        #endregion

        public TmlParametersInfoSetForJd601ViewModel()
        {
            InitAction();
            InitEvent();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            var tmlId = (int)parsObjects[0];
            if (tmlId > 0)
            {
                SelectedTmlChange(tmlId);
                RequestJd601Pars(tmlId);
            }
            ZcVisi = Visibility.Collapsed;
        }

        private Wlst.Sr.EquipmentInfoHolding.Model.Wj601Esu _terminalInformation;

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
           
            var tmp = new EsuParameter()
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
                RtuStateCode = t.RtuStateCode,         
            },
            tmp);

            //var ffff = t.Clone();
            //_terminalInformation = ffff as Jd601TerminalInformation;
            if (_terminalInformation != null)
            {
                UpdateVmInfo(_terminalInformation);
                AttachRtuId = _terminalInformation.RtuFid;
            }
            else
            {
                AttachRtuId = 0;
                AttachRtuName = "无法解析该设备连接的终端";
            }
            OnLoadJd601ParVm();
            Thread.Sleep(100);
            RequestJd601Pars(RtuId);

        }

        /// <summary>
        /// 将回路信息、输入信、输出信息还原为 终端信息
        /// </summary>
        /// <returns></returns>
        private Wlst.Sr.EquipmentInfoHolding.Model.Wj601Esu BackViewModelToTerminalInformation()
        {
            var info = GetSetIIJd601();
            _terminalInformation.RtuPhyId = this.PhyId;
            _terminalInformation.RtuId = info.RtuId;
            _terminalInformation.RtuName = this.RtuName;//.RtuName;
            _terminalInformation.WjEsu.EsuAlarmDelay = info.EsuAlarmDelay;
            _terminalInformation.WjEsu.EsuCloseTime = info.EsuCloseTime;
            _terminalInformation.WjEsu.EsuCommTypeCode = info.EsuCommTypeCode;
            _terminalInformation.WjEsu.EsuCtRadioA = info.EsuCtRadioA;
            _terminalInformation.WjEsu.EsuCtRadioB = info.EsuCtRadioB;
            _terminalInformation.WjEsu.EsuCtRadioC = info.EsuCtRadioC;
            _terminalInformation.WjEsu.EsuEnerySaveTemp = info.EsuEnerySaveTemp;
            _terminalInformation.WjEsu.EsuFanClosedTemp = info.EsuFanClosedTemp;
            _terminalInformation.WjEsu.EsuFanSatrtTemp = info.EsuFanSatrtTemp;
            _terminalInformation.WjEsu.EsuInputOvervoltageLimit = info.EsuInputOvervoltageLimit;
            _terminalInformation.WjEsu.EsuInputUndervoltageLimit = info.EsuInputUndervoltageLimit;
            _terminalInformation.WjEsu.EsuMandatoryProtectTemp = info.EsuMandatoryProtectTemp;
            //_terminalInformation.RtuModel = this.ZcModel;
            _terminalInformation.WjEsu.EsuOutputOverloadLimit = info.EsuOutputOverloadLimit;
            _terminalInformation.WjEsu.EsuOutputUndervoltageLimit = info.EsuOutputUndervoltageLimit;
            _terminalInformation.WjEsu.EsuPowerSupplyPhases = info.EsuPowerSupplyPhases;
            _terminalInformation.WjEsu.EsuPreheatingTime = info.EsuPreheatingTime;
            _terminalInformation.WjEsu.EsuRecoverEnergySaveTemp = info.EsuRecoverEnergySaveTemp;
            _terminalInformation.WjEsu.EsuRegulatingSpeed = info.EsuRegulatingSpeed;
            _terminalInformation.WjEsu.EsuRunMode = info.EsuRunMode;
            _terminalInformation.WjEsu.EsuTimeMode = info.EsuTimeMode;
            _terminalInformation.WjEsu.EsuWorkMode = info.EsuWorkMode;
            _terminalInformation.WjEsu.EsyValidIdentify = info.EsyValidIdentify;
            _terminalInformation.WjEsu.IsActiveAlarm = info.IsActiveAlarm;


            return _terminalInformation;
        }



      

        #region

        private int _attachRtuId;

        /// <summary>
        /// 如果连接终端 则终端地址  不允许修改
        /// </summary>
        [StringLength(30, ErrorMessage = "名称长度不能大于30")]
        [Required(ErrorMessage = "输入不得为空")]
        public int AttachRtuId
        {
            get { return _attachRtuId; }
            set
            {
                if (value != _attachRtuId)
                {
                    _attachRtuId = value;
                    RaisePropertyChanged(() => AttachRtuId);


                    var t =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);

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
                RaisePropertyChanged(() => AttachRtuName);
            }
        }

        #endregion

        private DateTime [] _dateTimes=new DateTime[2];

        #region CmdSave

        private void ExSave()
        {
            _dateTimes[0] = DateTime.Now;
            UpdateInfo();
        }

        private bool CanSave()
        {
            return _terminalInformation != null && DateTime.Now.Ticks-_dateTimes[0].Ticks>30000000;
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
        private object _zcModel;
        public object ZcModel
        {
            get { return _zcModel; }
            set
            {
                if (value == _zcModel) return;
                _zcModel = value;
                RaisePropertyChanged(() => ZcModel);
            }
        }

        #region CmdZcVisi

        private void ExCmdZcVisi()
        {
            _dateTimes[1] = DateTime.Now;
            if (canzcvisi != Visibility.Visible)
           {
               ZcVisi=Visibility.Visible;
           }
            else
            {
                ZcVisi = Visibility.Collapsed;
                return;
            }

            var getparview =
    Cr.Core.CoreServices.RegionManage.GetViewById(
        Wj6005Module.Services.ViewIdAssign.TmlInfoSetZcForjd601ViewId,this.RtuId);
            if (ZcModel == null) ZcModel = getparview;
        }

        private bool CanCmdZcVisi()
        {
            return _terminalInformation != null && DateTime.Now.Ticks - _dateTimes[1].Ticks > 30000000;
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
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class TmlParametersInfoSetForJd601ViewModel
    {
        public void InitEvent()
        {
            AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId,
                                    PublishEventType.Core);

        }

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_request_para ,//.ClientPart.wlst_Jd601_server_ans_clinet_request_Pars,
                RequestOrUpdateJd601Parsinfo,
                typeof(TmlParametersInfoSetForJd601ViewModel), this);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_update_para ,//.ClientPart.wlst_Jd601_server_ans_clinet_update_Pars,
                RequestOrUpdateJd601Parsinfo,
                typeof(TmlParametersInfoSetForJd601ViewModel), this);
        }

        public void RequestOrUpdateJd601Parsinfo(string session,Wlst .mobile .MsgWithMobile  args)
        {
            var infos = args.WstSvrAnsCntRequestOrUpdateJd601Para ;
            if (infos == null) return;
            if (_terminalInformation == null) return;
            if (infos.RtuId != _terminalInformation.RtuId) return;
            UpdateJd601Vm(infos.Info);
        }

        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            try
            {
                if (_terminalInformation == null) return;
                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                {
                    var attachlst = args.GetParams()[0] as List<Tuple<int, int>>;
                    if (attachlst == null) return;
                    if (attachlst.Any(g => g.Item1 == _terminalInformation.RtuId))
                    {
                        SelectedTmlChange(_terminalInformation.RtuId);
                    }
                }
            }
            catch (Exception ex)
            {
                Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class TmlParametersInfoSetForJd601ViewModel
    {
        private void UpdateInfo()
        {
            if (_terminalInformation == null) return;

            var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_update_para ;//.ServerPart.wlst_Jd601_clinet_update_Pars;
            nt.WstCntUpdateJd601Para .RtuId = _terminalInformation.RtuId;
            foreach (var t in BackJd601Par())
            {
                nt.WstCntUpdateJd601Para.Info.Add(t);
            }
            SndOrderServer.OrderSnd(nt, 10, 3);

            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(
                BackViewModelToTerminalInformation());

        }


        private void RequestJd601Pars(int rtuid)
        {
            if (rtuid < 0) return;

            var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_request_para ;//.ServerPart.wlst_Jd601_clinet_request_Pars;
            nt.WstCntRequestJd601Para .RtuId = rtuid;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }

    }

}
