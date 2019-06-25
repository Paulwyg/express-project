using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Wj3005;
using Wlst.Sr.ProtocolCnt.AexchangeModels.ModelParts;
using Wlst.Sr.ProtocolCnt.Wj1090;
using Wlst.Ux.Wj1090Module.Wj1090ParaInfoSet.Services;

namespace Wlst.Ux.Wj1090Module.Wj1090ParaInfoSet.ViewModel
{
    [Export(typeof(IIWj1090ParaInfoSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090ParaInfoSetViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIWj1090ParaInfoSet
    {
        #region IITab
        public string Title
        {
            get { return "参数设置"; }
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

        public Wj1090ParaInfoSetViewModel()
        {
            InitAction();
            InitEvent();
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            IsVisiAlarmAutoReport = true;
            IsVisiAlarmLineShortCircuit = true;
            IsVisiAlarmLineLightOffImpedance = true;
            IsVisiAlarmLineLightOffSingle = true;
            IsVisiAlarmLineLosePower = true;
            IsVisiAlarmLineBrightRate = true;
            IsVisiAlarmLineLightOpenImpedance = true;
            IsVisiAlarmLineLightOpenSingel = true;
            var tmlId = (int)parsObjects[0];
            if (tmlId > 0)
            {
                SelectedTmlChange(tmlId);
            }
        }

        public void OnUserHideOrClosing()
        {
            Items.Clear();
        }

        #region ICommand


        #region CmdBtnZhaoCe

        private DateTime _dtBtnZhaoCe;
        private ICommand _cmdBtnZhaoCe;
        public ICommand CmdBtnZhaoCe
        {
            get { return _cmdBtnZhaoCe ?? (_cmdBtnZhaoCe = new RelayCommand(ExBtnZhaoCe, CanBtnZhaoCe, true)); }
        }
        private void ExBtnZhaoCe()
        {
            _dtBtnZhaoCe = DateTime.Now;
            RequestParsData();
        }
        private bool CanBtnZhaoCe()
        {
            return DateTime.Now.Ticks - _dtBtnZhaoCe.Ticks > 30000000;
        }
        #endregion

        #region CmdSaveAndSnd

        private DateTime _dtSaveAndSnd;
        private ICommand _cmdSaveAndSnd;
        public ICommand CmdSaveAndSnd
        {
            get { return _cmdSaveAndSnd ?? (_cmdSaveAndSnd = new RelayCommand(ExBtnSaveAndSnd, CanBtnSaveAndSnd, true)); }
        }
        private void ExBtnSaveAndSnd()
        {
            _dtSaveAndSnd = DateTime.Now;
            var ins = BackViewModelToTerminalInformation();
            if (ins == null) return;
            Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.UpdateEquipmentInfo(ins);
            Remind = "保存命令已发送...";
        }
        private bool CanBtnSaveAndSnd()
        {
            return DateTime.Now.Ticks - _dtSaveAndSnd.Ticks > 30000000;
        }
        #endregion

        #endregion

        #region Items

        private ObservableCollection<LduLineModel> _items;
        public ObservableCollection<LduLineModel> Items
        {
            get { return _items ?? (_items = new ObservableCollection<LduLineModel>()); }
        }
        #endregion

        #region Attris

        #region 高级显示，控制列表控件显示的列

        #region IsVisiLduLightoffImpedanceLimit 关灯阻抗门限

        private bool _isVisiLduLightoffImpedanceLimit;
        public bool IsVisiLduLightoffImpedanceLimit
        {
            get { return _isVisiLduLightoffImpedanceLimit; }
            set
            {
                if(_isVisiLduLightoffImpedanceLimit==value) return;
                _isVisiLduLightoffImpedanceLimit = value;
                RaisePropertyChanged(()=>IsVisiLduLightoffImpedanceLimit);
            }
        }

        #endregion

        #region IsVisiLduLightoffSingleLimit 关灯信号门限

        private bool _isVisiLduLightoffSingleLimit;
        public bool IsVisiLduLightoffSingleLimit
        {
            get { return _isVisiLduLightoffSingleLimit; }
            set
            {
                if (_isVisiLduLightoffSingleLimit == value) return;
                _isVisiLduLightoffSingleLimit = value;
                RaisePropertyChanged(() => IsVisiLduLightoffSingleLimit);
            }
        }

        #endregion

        #region IsVisiLduLightonImpedanceLimit 开灯阻抗门限

        private bool _isVisiLduLightonImpedanceLimit;
        public bool IsVisiLduLightonImpedanceLimit
        {
            get { return _isVisiLduLightonImpedanceLimit; }
            set
            {
                if (_isVisiLduLightonImpedanceLimit == value) return;
                _isVisiLduLightonImpedanceLimit = value;
                RaisePropertyChanged(() => IsVisiLduLightonImpedanceLimit);
            }
        }

        #endregion

        #region IsVisiLduLightonSingleLimit 开灯信号门限

        private bool _isVisiLduLightonSingleLimit;
        public bool IsVisiLduLightonSingleLimit
        {
            get { return _isVisiLduLightonSingleLimit; }
            set
            {
                if (_isVisiLduLightonSingleLimit == value) return;
                _isVisiLduLightonSingleLimit = value;
                RaisePropertyChanged(() => IsVisiLduLightonSingleLimit);
            }
        }

        #endregion

        #region IsVisiLduBrightRateAlarmLimit 开灯亮灯率门限

        private bool _isVisiLduBrightRateAlarmLimit;
        public bool IsVisiLduBrightRateAlarmLimit
        {
            get { return _isVisiLduBrightRateAlarmLimit; }
            set
            {
                if (_isVisiLduBrightRateAlarmLimit == value) return;
                _isVisiLduBrightRateAlarmLimit = value;
                RaisePropertyChanged(() => IsVisiLduBrightRateAlarmLimit);
            }
        }

        #endregion

        #region IsVisiAlarmAutoReport 主动报警

        private bool _isVisiAlarmAutoReport;
        public bool IsVisiAlarmAutoReport
        {
            get { return _isVisiAlarmAutoReport; }
            set
            {
                if (_isVisiAlarmAutoReport == value) return;
                _isVisiAlarmAutoReport = value;
                RaisePropertyChanged(() => IsVisiAlarmAutoReport);
            }
        }

        #endregion

        #region IsVisiAlarmLineShortCircuit 线路短路

        private bool _isVisiAlarmLineShortCircuit;
        public bool IsVisiAlarmLineShortCircuit
        {
            get { return _isVisiAlarmLineShortCircuit; }
            set
            {
                if (_isVisiAlarmLineShortCircuit == value) return;
                _isVisiAlarmLineShortCircuit = value;
                RaisePropertyChanged(() => IsVisiAlarmLineShortCircuit);
            }
        }

        #endregion

        #region IsVisiAlarmLineLightOffImpedance 关灯阻抗

        private bool _isVisiAlarmLineLightOffImpedance;
        public bool IsVisiAlarmLineLightOffImpedance
        {
            get { return _isVisiAlarmLineLightOffImpedance; }
            set
            {
                if (_isVisiAlarmLineLightOffImpedance == value) return;
                _isVisiAlarmLineLightOffImpedance = value;
                RaisePropertyChanged(() => IsVisiAlarmLineLightOffImpedance);
            }
        }

        #endregion

        #region IsVisiAlarmLineLightOffSingle 关灯脉冲

        private bool _isVisiAlarmLineLightOffSingle;
        public bool IsVisiAlarmLineLightOffSingle
        {
            get { return _isVisiAlarmLineLightOffSingle; }
            set
            {
                if (_isVisiAlarmLineLightOffSingle == value) return;
                _isVisiAlarmLineLightOffSingle = value;
                RaisePropertyChanged(() => IsVisiAlarmLineLightOffSingle);
            }
        }

        #endregion

        #region IsVisiAlarmLineLosePower 供电变化

        private bool _isVisiAlarmLineLosePower;
        public bool IsVisiAlarmLineLosePower
        {
            get { return _isVisiAlarmLineLosePower; }
            set
            {
                if (_isVisiAlarmLineLosePower == value) return;
                _isVisiAlarmLineLosePower = value;
                RaisePropertyChanged(() => IsVisiAlarmLineLosePower);
            }
        }

        #endregion

        #region IsVisiAlarmLineBrightRate 亮灯率变化

        private bool _isVisiAlarmLineBrightRate;
        public bool IsVisiAlarmLineBrightRate
        {
            get { return _isVisiAlarmLineBrightRate; }
            set
            {
                if (_isVisiAlarmLineBrightRate == value) return;
                _isVisiAlarmLineBrightRate = value;
                RaisePropertyChanged(() => IsVisiAlarmLineBrightRate);
            }
        }

        #endregion

        #region IsVisiAlarmLineLightOpenImpedance 开灯阻抗

        private bool _isVisiAlarmLineLightOpenImpedance;
        public bool IsVisiAlarmLineLightOpenImpedance
        {
            get { return _isVisiAlarmLineLightOpenImpedance; }
            set
            {
                if (_isVisiAlarmLineLightOpenImpedance == value) return;
                _isVisiAlarmLineLightOpenImpedance = value;
                RaisePropertyChanged(() => IsVisiAlarmLineLightOpenImpedance);
            }
        }

        #endregion

        #region IsVisiAlarmLineLightOpenSingel 开灯脉冲

        private bool _isVisiAlarmLineLightOpenSingel;
        public bool IsVisiAlarmLineLightOpenSingel
        {
            get { return _isVisiAlarmLineLightOpenSingel; }
            set
            {
                if (_isVisiAlarmLineLightOpenSingel == value) return;
                _isVisiAlarmLineLightOpenSingel = value;
                RaisePropertyChanged(() => IsVisiAlarmLineLightOpenSingel);
            }
        }

        #endregion

        #endregion

        #region Remind

        private string _remind;
        public string Remind
        {
            get { return _remind; }
            set
            {
                if(_remind==value) return;
                _remind = value;
                RaisePropertyChanged(()=>Remind);
            }
        }

        #endregion


        #region AdvanceShow

        private bool _advanceShow;
        public bool AdvanceShow
        {
            get { return _advanceShow; }
            set
            {
                if(_advanceShow==value) return;
                _advanceShow = value;
                RaisePropertyChanged(()=>AdvanceShow);
            }
        }
        #endregion

        #region PhyId

        private int _phtId;
        public int PhyId
        {
            get { return _phtId; }
            set
            {
                if(_phtId.Equals(value)) return;
                _phtId = value;
                RaisePropertyChanged(()=>PhyId);
            }
        }

        #endregion

        #region RtuId

        private int _rtuId;
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId.Equals(value)) return;
                _rtuId = value;
                RaisePropertyChanged(() => RtuId);
            }
        }

        #endregion

        #region RtuName

        private string _rtuName;
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (_rtuName==value) return;
                _rtuName = value;
                RaisePropertyChanged(() => RtuName);
            }
        }

        #endregion

        #region AttachRtuId

        private int _attachRtuId;
        public int AttachRtuId
        {
            get { return _attachRtuId; }
            set
            {
                if (_attachRtuId == value) return;
                _attachRtuId = value;
                RaisePropertyChanged(() => AttachRtuId);

                AttachRtuName = "Reserve";
                if (!Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(_attachRtuId))
                    return;

                var t =Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[_attachRtuId] as Wj3005TerminalInformation;
                if(t==null) return;
                AttachRtuName = t.RtuName;
                AttachPhyId = t.PhyId;
                LoopCollectionInfo.Clear();
                for (var i = 0; i <t.RtuParaAnalogueAmps.GetAllRtuParaAnalogueAmps().Count; i++)
                {
                  if(t.RtuParaAnalogueAmps.GetAllRtuParaAnalogueAmps()[i].SwitchOutId>0)
                  {
                      LoopCollectionInfo.Add(new NameValueInt
                      {
                          Name = t.RtuParaAnalogueAmps.GetAllRtuParaAnalogueAmps()[i].LoopName,
                          Value = t.RtuParaAnalogueAmps.GetAllRtuParaAnalogueAmps()[i].LoopId
                      });   
                  }
                }
            }
        }

        private int _attachPhyId;

        public int AttachPhyId
        {
            get { return _attachPhyId; }
            set
            {
                if (_attachPhyId == value) return;
                _attachPhyId = value;
                RaisePropertyChanged(() => AttachPhyId);
            }
        }
        #endregion

        #region AttachRtuName

        private string _attachRtuName;
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

        #region Remark
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set
            {
                if (_remark != value)
                {
                    _remark = value;
                    RaisePropertyChanged(() => Remark);
                }
            }
        }
        #endregion

        #region LduCommType
        private string _lduCommType;

        /// <summary>
        /// 通信方式 0 保留，1 电台，2 串口232，3 串口485，4 Zigbee，5 电力载波，6 Socket  一般为3或6
        /// </summary>
        public string LduCommType
        {
            get { return _lduCommType; }
            set
            {
                if (_lduCommType != value)
                {
                    _lduCommType = value;
                    RaisePropertyChanged(() => LduCommType);
                }
            }
        }
        #endregion

        #region LoopId
        private int _loopId;
        public int LoopId
        {
            get { return _loopId; }
            set
            {
                if (_loopId == value) return;
                _loopId = value;
                RaisePropertyChanged("LoopId");
            }
        }
        #endregion

        #region 终端的所有回路

        private List<NameValueInt> _loopCollectionInfo;
        public List<NameValueInt> LoopCollectionInfo
        {
            get { return _loopCollectionInfo ?? (_loopCollectionInfo = new List<NameValueInt>()); }
        }

        #endregion

        #endregion
    }

    public partial class Wj1090ParaInfoSetViewModel
    {
        private Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation _terminalInformation;
        private Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation BackViewModelToTerminalInformation()
        {
            _terminalInformation.RtuName = RtuName;
            _terminalInformation.Remark = Remark;
            _terminalInformation.LduLines.Clear();
            foreach (var t in Items)
            {
                _terminalInformation.LduLines.Add(t.BackToLduLineParameter());
            }
            return _terminalInformation;
        }

        public void SelectedTmlChange(int rtuId)
        {
            if (
                !Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.
                     ContainsKey(rtuId))
                return;
            var t =
                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId]
                as Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;

            if (t == null)
                return;
            var ffff = t.Clone();
            var tmp = ffff as Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;
            //属性自动生成
            if (tmp == null)
                return;
            _terminalInformation = tmp;

            InitViewModel();
        }

        private void InitViewModel()
        {
            if (_terminalInformation == null)
                return;


            AttachRtuId = _terminalInformation.AttachRtuId;
            RtuId = _terminalInformation.RtuId;
            PhyId = _terminalInformation.PhyId;
            RtuName = _terminalInformation.RtuName;
            Remark = _terminalInformation.Remark;

            LduCommType = _terminalInformation.RtuModel == 30910 ? "无线" : "有线";

            Items.Clear();
            foreach (var t in _terminalInformation.LduLines)
            {
                Items.Add( new LduLineModel(t, LoopCollectionInfo){DataType = "初始数据"});
            }
            LoopId = 0;
        }

        private void RequestParsData()
        {
            var info = Sr.ProtocolCnt.ServerPart.wlst_Wj1090_clinet_order_ReadPars;
            info.Data.RtuId = RtuId;
            info.Data.ControlId = LoopId;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = "招测命令已发送...";
        }

        private void ResolveRequestParsData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<LduParsData> infos)
        {
            var info = infos.Data;
            if (info == null) return;
            if (RtuId != info.RtuId) return;  //如果是本客户端选测则继续运行下面代码

            foreach (LduLinePars item in info.Items)
            {
                var lduLine = new LduLineParameter
                {
                    AlarmAutoReport = item.AlarmAutoReport,
                    AlarmLineBrightRate = item.AlarmLineBrightRate,
                    AlarmLineLightOffImpedance = item.AlarmLineLightOffImpedance,
                    AlarmLineLightOffSingle = item.AlarmLineLightOffSingle,
                    AlarmLineLightOpenImpedance = item.AlarmLineLightOpenImpedance,
                    AlarmLineLightOpenSingel = item.AlarmLineLightOpenSingel,
                    AlarmLineLosePower = item.AlarmLineLosePower,
                    AlarmLineShortCircuit = item.AlarmLineShortCircuit,
                    LduBrightRateAlarmLimit = item.LduBrightRateAlarmLimit,
                    LduEndLampportSn = string.Format("{0}",item.LduEndLampportSn),
                    LduLightoffImpedanceLimit = item.LduLightoffImpedanceLimit,
                    LduLightoffSingleLimit = item.LduLightoffSingleLimit,
                    LduLightonImpedanceLimit = item.LduLightonImpedanceLimit,
                    LduLightonSingleLimit = item.LduLightonSingleLimit,
                    LduLoopID = item.LineLoopId,
                    LduPhase = item.LduPhase,

                    MutualInductorRadio = item.MutualInductorRadio
                };
                foreach (var t in Items)
                {
                    if (t.LduLineID == item.LineLoopId)
                    {
                        t.ZcItems.Clear();
                        t.ZcItems.Add(new LduLineModel(lduLine, LoopCollectionInfo){DataType = "招测数据"});
                        t.ZcItems[0].LduLineID = t.LduLineID;
                        t.ZcItems[0].LduLineName = "[招测] " + t.LduLineName;
                        t.ZcItems[0].IsUsed = t.IsUsed;
                        t.ZcItems[0].Remark = t.Remark;
                        t.ZcItems[0].LduControlTypeCode = t.LduControlTypeCode;
                    }
                }
            }
            Remind = "招测数据已返回！";
        }

    
    }
    public partial class Wj1090ParaInfoSetViewModel
    {
        public void InitEvent()
        {
            EventPublisher.AddEventSubScriptionTokener(Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }
        public void InitAction()
        {

            ProtocolServer.RegistProtocol( Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_ReadPars,
                ResolveRequestParsData, typeof(Wj1090ParaInfoSetViewModel), this);

            ProtocolServer.RegistProtocol(Sr.ProtocolCnt.ClientPart.wlst_Wj1090_server_ans_clinet_order_SetPars,
                              ResolveAnsSndParsData,typeof(Wj1090ParaInfoSetViewModel), this);



        }

        private void ResolveAnsSndParsData(string session, Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<List<int>> infos)
        {
            var info = infos.AddrLst;

            if (info == null) return;
            if (info.Count > 0)
                Remind = "数据下发成功!!" + string.Format("{0:F}", DateTime.Now);

        }

        public bool FundOrderFilters(PublishEventArgs args) //接收终端选中变更事件
        {
            try
            {
                if (args.EventType == PublishEventType.Core && args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        public void FundEventHandlers(PublishEventArgs args)
        {
            try
            {
                if (args.EventType == PublishEventType.Core && args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentUpdateEventId)
                {
                    var temptuple = args.GetParams()[0];
                    if (temptuple == null) return;
                    var temp = temptuple as List<Tuple<int, int>>;
                    if (temp == null) return;
                    var id = temp[0].Item1;
                    Remind = id == RtuId ? "数据保存成功!":"";

                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("LduInfoSetViewModel error in FundEventHandlers:ex:" + xe);
            }
        }
    }
}
