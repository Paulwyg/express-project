using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows.Input;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj1090Module.Wj1090ParaInfoSet.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.Wj1090Module.Wj1090ParaInfoSet.ViewModel
{
    [Export(typeof(IIWj1090ParaInfoSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090ParaInfoSetViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIWj1090ParaInfoSet
    {
        #region IITab
        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "线路检测参数设置"; }
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
            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(ins);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  保存命令已发送...";
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
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(_attachRtuId))//.EquipmentInfoDictionary.ContainsKey(_attachRtuId))
                    return;

                var tx = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[_attachRtuId];//.EquipmentInfoDictionary[_attachRtuId] as Wj3005TerminalInformation;
                var t = tx as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if(t==null) return;
                AttachRtuName = t.RtuName;
                AttachPhyId = t.RtuPhyId;
                LoopCollectionInfo.Clear();

                var ntg =
                    (from txr in t.WjLoops where txr.Value.SwitchOutputId > 0 orderby txr.Value.LoopId select txr.Value)
                        .ToList();
                foreach (var f in ntg)
                {
                      LoopCollectionInfo.Add(new NameValueInt
                      {
                          Name = f.LoopName,//.GetAllRtuParaAnalogueAmps()[i].LoopName,
                          Value = f.LoopId//.GetAllRtuParaAnalogueAmps()[i].LoopId
                      }); 
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
        private Sr.EquipmentInfoHolding.Model.Wj1090Ldu _terminalInformation;
        //private Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation _terminalInformation;
        private Sr.EquipmentInfoHolding.Model.Wj1090Ldu BackViewModelToTerminalInformation()
        //private Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation BackViewModelToTerminalInformation()
        {
            _terminalInformation.RtuName = RtuName;
            _terminalInformation.RtuRemark = Remark;
            _terminalInformation.WjLduLines.Clear();
            foreach (var t in Items)
            {
                var line = t.BackToLduLineParameter();
                _terminalInformation.WjLduLines .Add( line .LduLineId ,line );
            }
            return _terminalInformation;
        }

        public void SelectedTmlChange(int rtuId)
        {
            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                     ContainsKey(rtuId))
                return;
            var t =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId]
                as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;

            if (t == null)
                return;


            var tmp = new List<LduLineParameter >();
            var ntg = (from txr in t.WjLduLines orderby txr.Value.LduLineId ascending select txr.Value).ToList();
            foreach (var f in ntg  )
            {
                tmp.Add(new LduLineParameter()
                            {
                                AlarmAutoReport = f.AlarmAutoReport,
                                AlarmLineBrightRate = f.AlarmLineBrightRate,
                                AlarmLineLightOffImpedance =f.AlarmLineLightOffImpedance,
                                AlarmLineLightOffSingle = f.AlarmLineLightOffSingle,
                                AlarmLineLightOpenImpedance =f.AlarmLineLightOpenImpedance,
                                AlarmLineLightOpenSingel = f.AlarmLineLightOpenSingel,
                                AlarmLineLosePower = f.AlarmLineLosePower,
                                AlarmLineShortCircuit = f.AlarmLineShortCircuit,
                                LduLineId = f.LduLineId,
                                LduLoopId = f.LduLoopId,
                                LduEndLampportSn = f.LduEndLampportSn,
                                LduBrightRateAlarmLimit = f.LduBrightRateAlarmLimit,
                                LduCommTypeCode = f.LduCommTypeCode,
                                LduConcentratorId = f.LduConcentratorId,
                                LduControlTypeCode = f.LduControlTypeCode,
                                LduLightoffImpedanceLimit = f.LduLightoffImpedanceLimit,
                                LduLightoffSingleLimit = f.LduLightoffSingleLimit,
                                LduLightonImpedanceLimit = f.LduLightonImpedanceLimit,
                                LduLightonSingleLimit = f.LduLightonSingleLimit,
                                LduLineName = f.LduLineName,
                                LduPhase = f.LduPhase,
                                Remark = f.Remark,
                                MutualInductorRadio = f.MutualInductorRadio,
                                IsUsed = f.IsUsed
                            });
            }
            _terminalInformation = new Wj1090Ldu(new EquipmentParameter()
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
            //var tmp = ffff as Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;
            ////属性自动生成
            //if (tmp == null)
            //    return;
            //_terminalInformation = tmp;

            InitViewModel();
        }

        private void InitViewModel()
        {
            if (_terminalInformation == null)
                return;


            AttachRtuId = _terminalInformation.RtuFid;
            RtuId = _terminalInformation.RtuId;
            PhyId = _terminalInformation.RtuPhyId;
            RtuName = _terminalInformation.RtuName;
            Remark = _terminalInformation.RtuRemark;

            LduCommType =  _terminalInformation.RtuModel ==EnumRtuModel .Wj30910   ? "无线" : "有线";

            Items.Clear();
            foreach (var t in _terminalInformation.WjLduLines.Values )
            {
                Items.Add( new LduLineModel(t, LoopCollectionInfo){DataType = "设置"});
            }
            LoopId = 0;
        }

        private void RequestParsData()
        {
            var info = Sr.ProtocolPhone .LxLdu  .wst_ldu_orders  ;//.ServerPart.wlst_Wj1090_clinet_order_ReadPars;
            info.WstLduOrders  .LduId  = RtuId;
            info.WstLduOrders.LineIds .Add(  LoopId);
            info.WstLduOrders.Op = 2;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  召测命令已发送...";
        }

    
    }
    public partial class Wj1090ParaInfoSetViewModel
    {
        public void InitEvent()
        {
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }
        public void InitAction()
        {

            ProtocolServer.RegistProtocol( Sr.ProtocolPhone .LxLdu .wst_svr_ans_ldu_orders ,// .wlst_svr_ans_cnt_wj1090_order_read_para ,//.ClientPart.wlst_Wj1090_server_ans_clinet_order_ReadPars,
                ResolveRequestParsData, typeof(Wj1090ParaInfoSetViewModel), this);

        }

        private void ResolveRequestParsData(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstLduSvrAnsOrders ;
            if (info == null) return;
            if (RtuId != info.LduId ) return;  //如果是本客户端选测则继续运行下面代码
            if (info.Op == 2)
            {
                foreach (var  item in info.ItemsPara )
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
                                          LduEndLampportSn = string.Format("{0}", item.LduEndLampportSn),
                                          LduLightoffImpedanceLimit = item.LduLightoffImpedanceLimit,
                                          LduLightoffSingleLimit = item.LduLightoffSingleLimit,
                                          LduLightonImpedanceLimit = item.LduLightonImpedanceLimit,
                                          LduLightonSingleLimit = item.LduLightonSingleLimit,
                                          LduLoopId = item.LineId ,
                                          LduPhase = item.LduPhase,

                                          MutualInductorRadio = item.MutualInductorRadio
                                      };
                    foreach (var t in Items)
                    {
                        if (t.LduLineID == item.LineId )
                        {
                            t.ZcItems.Clear();
                            t.ZcItems.Add(new LduLineModel(lduLine, LoopCollectionInfo) {DataType = "召测"});
                            t.ZcItems[0].LduLineID = t.LduLineID;
                            t.ZcItems[0].LduLineName = "[召测] " + t.LduLineName;
                            t.ZcItems[0].IsUsed = t.IsUsed;
                            t.ZcItems[0].Remark = t.Remark;
                            t.ZcItems[0].LduControlTypeCode = t.LduControlTypeCode;
                        }
                    }
                }
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  召测数据已返回！";
            }
            if(info .Op ==11)
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  数据下发成功!!" + string.Format("{0:F}", DateTime.Now);
            }

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
                    Remind = id == RtuId ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  数据保存成功!" : "";

                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("LduInfoSetViewModel error in FundEventHandlers:ex:" + xe);
            }
        }
    }
}
