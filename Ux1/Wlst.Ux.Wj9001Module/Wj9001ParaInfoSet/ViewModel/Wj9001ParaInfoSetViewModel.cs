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
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Wj9001Module.Wj9001ParaInfoSet.ViewModel;
using Wlst.Ux.Wj9001Module.Wj9001ParaInfoSet.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.Wj9001Module.Wj9001ParaInfoSet.ViewModel
{
    [Export(typeof(IIWj9001ParaInfoSet))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj9001ParaInfoSetViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIWj9001ParaInfoSet
    {
        #region IITab
        public string Title
        {
            get { return "漏电参数设置"; }
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

        public Wj9001ParaInfoSetViewModel()
        {
            InitAction();
            InitEvent();
        }
        public event EventHandler OnNavOnLoadSelectdRtus;

        public void NavOnLoad(params object[] parsObjects)
        {
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

        #region CmdSetPhyId

        private DateTime _dtCmdSetPhyId;
        private ICommand _cmdSetPhyId;
        public ICommand CmdSetPhyId
        {
            get { return _cmdSetPhyId ?? (_cmdSetPhyId = new RelayCommand(ExBtnReSet, CanBtnSetPhyId, true)); }
        }
        private void ExBtnCmdSetPhyId()
        {
            _dtCmdSetPhyId = DateTime.Now;
            var ins = BackViewModelToTerminalInformation();
            if (ins == null) return;
            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(ins);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "保存命令已发送...";
        }
        private bool CanBtnSetPhyId()
        {
            return DateTime.Now.Ticks - _dtCmdSetPhyId.Ticks > 30000000;
        }
        #endregion

        #region CmdReSet

        private DateTime _dtCmdReSet;
        private ICommand _cmdReSet;
        public ICommand CmdReSet
        {
            get { return _cmdReSet ?? (_cmdReSet = new RelayCommand(ExBtnReSet, CanBtnReSet, true)); }
        }
        private void ExBtnReSet()
        {
            _dtCmdReSet = DateTime.Now;
            //var ins = BackViewModelToTerminalInformation();


            var nt = Wlst.Sr.ProtocolPhone.LxLeak.wst_leak_order_zcOrSet;
            var order = new LeakOrders.LeakOrderItem();
            order.Op = 6;
            order.RtuId = RtuId;
            nt.WstLeakOrderZcOrSet.Item.Add(order);
            SndOrderServer.OrderSnd(nt);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "复位命令已发送...";
        }
        private bool CanBtnReSet()
        {
            return DateTime.Now.Ticks - _dtCmdReSet.Ticks > 30000000;
        }
        #endregion

        #region CmdZcOrSnd

        private ICommand _cmCmdZcOrSnd;

        public ICommand CmdZcOrSnd
        {
            get { return _cmCmdZcOrSnd ?? (_cmCmdZcOrSnd = new RelayCommand<string>(ExCmdZcOrSnd, CanCmdZcOrSnd, true)); }
        }


        private long lastexute = 0;
        private int lastexutetpara = 0;

        private void ExCmdZcOrSnd(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            lastexute = DateTime.Now.Ticks;
            lastexutetpara = x;

            SndZcOrSndToSvr(x);

        }

        private bool CanCmdZcOrSnd(string str)
        {
            int x = 0;
            try
            {
                x = Convert.ToInt32(str);
            }
            catch (Exception ex)
            {

            }
            if (x == lastexutetpara)
            {

                return DateTime.Now.Ticks - lastexute > 30000000;
            }
            return true;
            // return x != lastexutetpara && DateTime.Now.Ticks - lastexute > 30000000;
        }
        private void SndZcOrSndToSvr(int x)    //todo
        {
            if (x < 1 || x > 14) return;
            // Msg = "snd  x=" + x;
            if (x == 1)
            {
                var info = WlstMessageBox.Show("操作提示",
                                          "只能接一个硬件设备时才能设置地址", WlstMessageBoxType.YesNo);
                if (info != WlstMessageBoxResults.Yes)
                {
                    return;
                }
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置地址命令已发送...";
            }
            if (x == 2)//todo 
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置参数命令已发送...";
            }
            if (x == 5)
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置时钟命令已发送...";
            }
            if (x == 6)
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "复位命令已发送...";
            }
            if (x == 12)
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "召测时钟命令已发送...";
            }
            if (x == 13)
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "召测命令已发送...";
            }


            var nt = Wlst.Sr.ProtocolPhone.LxLeak.wst_leak_order_zcOrSet;
            var order = new LeakOrders.LeakOrderItem();
            order.Op = x;
            order.RtuId = RtuId;
            nt.WstLeakOrderZcOrSet.Item.Add(order);
            SndOrderServer.OrderSnd(nt);

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

            foreach (
               var tt in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
               )
            {
                if (tt.Value.RtuModel == EnumRtuModel.Wj9001 && tt.Value.RtuFid == ins.RtuFid && tt.Value.RtuFid != 0 && tt.Value.RtuId != ins.RtuId)
                {
                    if (tt.Value.RtuPhyId == ins.RtuPhyId)
                    {
                        WlstMessageBox.Show("无法保存", "该设备物理地址重复！", WlstMessageBoxType.Ok);
                        break;
                    }
                }

            }
            
            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(ins);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "保存命令已发送...";
        }
        private bool CanBtnSaveAndSnd()
        {
            return DateTime.Now.Ticks - _dtSaveAndSnd.Ticks > 30000000;
        }
        #endregion



        #endregion

        #region Items

        private ObservableCollection<Wj9001Model> _items;
        public ObservableCollection<Wj9001Model> Items
        {
            get { return _items ?? (_items = new ObservableCollection<Wj9001Model>()); }
        }
        #endregion

        #region Attris
        #region DataType

        private string _dataType;
        public string DataType
        {
            get { return _dataType; }
            set
            {
                if (_dataType == value) return;
                _dataType = value;
                RaisePropertyChanged(() => DataType);
            }
        }
        #endregion

        #region LeakLineId 线路序号
        private int _leakLineId;
        public int LeakLineId
        {
            get { return _leakLineId; }
            set
            {
                if (_leakLineId == value) return;
                _leakLineId = value;
                RaisePropertyChanged(() => LeakLineId);
            }
        }

        #endregion

        #region LeakId 设备虚拟地址
        private int _leakId;
        public int LeakId
        {
            get { return _leakId; }
            set
            {
                if (_leakId == value) return;
                _leakId = value;
                RaisePropertyChanged(() => LeakId);
            }
        }

        #endregion

        #region AutoBreakOrAutoAlarm 自动分闸 自动报警
        private int _autoBreakOrAutoAlarm;
        public int AutoBreakOrAutoAlarm
        {
            get { return _autoBreakOrAutoAlarm; }
            set
            {
                if (_autoBreakOrAutoAlarm == value) return;
                _autoBreakOrAutoAlarm = value;
                RaisePropertyChanged(() => AutoBreakOrAutoAlarm);
            }
        }

        #endregion

        #region LeakCommTypeCode 通信方式
        private int _leakCommTypeCode;
        public int LeakCommTypeCode
        {
            get { return _leakCommTypeCode; }
            set
            {
                if (_leakCommTypeCode == value) return;
                _leakCommTypeCode = value;
                RaisePropertyChanged(() => LeakCommTypeCode);
            }
        }

        #endregion

        #region LeakMode 漏电模式
        private int _leakMode;
        public int LeakMode
        {
            get { return _leakMode; }
            set
            {
                if (_leakMode == value) return;
                _leakMode = value;
                RaisePropertyChanged(() => LeakMode);
            }
        }

        #endregion

        #region TimeDelayforBreak 延迟时间
        private int _timeDelayforBreak;
        public int TimeDelayforBreak
        {
            get { return _timeDelayforBreak; }
            set
            {
                if (_timeDelayforBreak == value) return;
                _timeDelayforBreak = value;
                RaisePropertyChanged(() => TimeDelayforBreak);
            }
        }

        #endregion

        #region UpperAlarmOrBreakforLeakOrTemperature 报警上下限
        private int _upperAlarmOrBreakforLeakOrTemperature;
        public int UpperAlarmOrBreakforLeakOrTemperature
        {
            get { return _upperAlarmOrBreakforLeakOrTemperature; }
            set
            {
                if (_upperAlarmOrBreakforLeakOrTemperature == value) return;
                _upperAlarmOrBreakforLeakOrTemperature = value;
                RaisePropertyChanged(() => UpperAlarmOrBreakforLeakOrTemperature);
            }
        }

        #endregion

        #region LeakEndLampportSn 末端备用序号
        private string _leakEndLampportSn;
        public string LeakEndLampportSn
        {
            get { return _leakEndLampportSn; }
            set
            {
                if (_leakEndLampportSn != value)
                {
                    _leakEndLampportSn = value;
                    RaisePropertyChanged(() => LeakEndLampportSn);
                }
            }
        }
        #endregion

        #region 备注
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

        #region IsEdit 编辑
        private bool _isEdit;
        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                if (value == _isEdit) return;
                _isEdit = value;
                RaisePropertyChanged(() => IsEdit);
            }
        }
        #endregion

        #region 使用 IsUsed
        private bool _isUsed;
        public bool IsUsed
        {
            get { return _isUsed; }
            set
            {
                if (_isUsed == value) return;
                _isUsed = value;
                RaisePropertyChanged(() => IsUsed);
            }
        }
        #endregion

        #region 使用 IsEnable
        private bool _isEnable;
        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (_isEnable == value) return;
                _isEnable = value;
                RaisePropertyChanged(() => IsEnable);
            }
        }
        #endregion

        #region LineName 线路名称
        private string _lineName;
        public string LineName
        {
            get { return _lineName; }
            set
            {
                if (_lineName != value)
                {
                    _lineName = value;
                    RaisePropertyChanged(() => LineName);
                }
            }
        }
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

        #region 序号

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                if (value != _index)
                {

                    _index = value;
                    RaisePropertyChanged(() => Index);
                }
            }
        }


        #endregion
    

        #region LeakPhyId

        private int _phtId;
        public int LeakPhyId
        {
            get { return _phtId; }
            set
            {
                if(_phtId.Equals(value)) return;
                _phtId = value;
                RaisePropertyChanged(() => LeakPhyId);
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
                //LoopCollectionInfo.Clear();
                //for (var i = 0; i <t.WjLoops.Count; i++)
                //{
                //  if(t.WjLoops[i].SwitchOutputId >0)
                //  {
                //      LoopCollectionInfo.Add(new NameValueInt
                //      {
                //          Name = t.WjLoops[i].LoopName,//.GetAllRtuParaAnalogueAmps()[i].LoopName,
                //          Value = t.WjLoops[i].LoopId//.GetAllRtuParaAnalogueAmps()[i].LoopId
                //      });   
                //  }
                //}
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


        //#region LduCommType
        //private string _lduCommType;

        ///// <summary>
        ///// 通信方式 0 保留，1 电台，2 串口232，3 串口485，4 Zigbee，5 电力载波，6 Socket  一般为3或6
        ///// </summary>
        //public string LduCommType
        //{
        //    get { return _lduCommType; }
        //    set
        //    {
        //        if (_lduCommType != value)
        //        {
        //            _lduCommType = value;
        //            RaisePropertyChanged(() => LduCommType);
        //        }
        //    }
        //}
        //#endregion

        //#region LoopId
        //private int _loopId;
        //public int LoopId
        //{
        //    get { return _loopId; }
        //    set
        //    {
        //        if (_loopId == value) return;
        //        _loopId = value;
        //        RaisePropertyChanged("LoopId");
        //    }
        //}
        //#endregion

        #region 终端的所有回路

        private List<NameValueInt> _loopCollectionInfo;
        public List<NameValueInt> LoopCollectionInfo
        {
            get { return _loopCollectionInfo ?? (_loopCollectionInfo = new List<NameValueInt>()); }
        }

        #endregion

        #endregion
    }

    public partial class Wj9001ParaInfoSetViewModel
    {
        private Sr.EquipmentInfoHolding.Model.Wj9001Leak _terminalInformation;
        //private Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation _terminalInformation;
        private Sr.EquipmentInfoHolding.Model.Wj9001Leak BackViewModelToTerminalInformation()
        //private Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation BackViewModelToTerminalInformation()
        {


            _terminalInformation.RtuName = RtuName;
            _terminalInformation.RtuRemark = Remark;
            _terminalInformation.WjLeakLines.Clear();
            _terminalInformation.RtuPhyId = LeakPhyId;
            foreach (var t in Items)
            {
                var line = t.BackToLeakLineParameter();
                _terminalInformation.WjLeakLines .Add(line.LeakLineId,line );
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
                as Sr.EquipmentInfoHolding.Model.Wj9001Leak;

            if (t == null)
                return;



            var tmp = new List<LeakParameter >();
            var ntg = (from txr in t.WjLeakLines orderby txr.Value.LeakLineId ascending select txr.Value).ToList();
            foreach (var f in ntg  )
            {
                tmp.Add(new LeakParameter()
                            {
                                AutoBreakOrAutoAlarm = f.AutoBreakOrAutoAlarm==2?0:1,//自动分闸
                                IsUsed = f.IsUsed,  //是否使用
                                LeakCommTypeCode = f.LeakCommTypeCode,
                                LeakEndLampportSn = f.LeakEndLampportSn,
                                LeakId = f.LeakId, //1600001+
                                LeakLineId = f.LeakLineId,
                                LeakMode = f.LeakMode,
                                LineName = f.LineName,
                                Remark = f.Remark,
                                TimeDelayforBreak = f.TimeDelayforBreak,
                                UpperAlarmOrBreakforLeakOrTemperature = f.UpperAlarmOrBreakforLeakOrTemperature
                            });
            }
            _terminalInformation = new Wj9001Leak(new EquipmentParameter()
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
            LeakPhyId = _terminalInformation.RtuPhyId;
            RtuName = _terminalInformation.RtuName;
            Remark = _terminalInformation.RtuRemark;
            
            //LduCommType =  _terminalInformation.RtuModel ==EnumRtuModel.Wj9001   ? "无线" : "有线";

            Items.Clear();
            int index = 0;
            foreach (var t in _terminalInformation.WjLeakLines.Values )
            {
                index = index + 1;
                //bool ed = t.IsUsed == true;
                Items.Add( new Wj9001Model(t){DataType = "设置",Index = index,IsEdit =true });
            }
           
            //LoopId = 0;
        }

        private void RequestParsData()
        {
            var info = Sr.ProtocolPhone.LxLeak.wst_leak_order_zcOrSet;//  .wst_ldu_orders  ;//.ServerPart.wlst_Wj1090_clinet_order_ReadPars;
            info.WstLeakOrderZcOrSet.Item[0].RtuId = RtuId;

            info.WstLeakOrderZcOrSet.Item[0].Op = 13;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "召测命令已发送...";
        }
    }
    public partial class Wj9001ParaInfoSetViewModel
    {
        public void InitEvent()
        {
           EventPublish.AddEventTokener( Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandlers, FundOrderFilters);
        }
        public void InitAction()
        {

            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxLeak.wst_leak_order_zcOrSet,// .LxLdu .wst_svr_ans_ldu_orders ,// .wlst_svr_ans_cnt_wj1090_order_read_para ,//.ClientPart.wlst_Wj1090_server_ans_clinet_order_ReadPars,
                ResolveRequestParsData, typeof(Wj9001ParaInfoSetViewModel), this);

        }

        private void ResolveRequestParsData(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstLeakOrderZcOrSet;//.WstSvrAnsLduOrders ;
            if (info == null) return;
            foreach (var g in info.Item)
            {
                if (RtuId == g.RtuId )   //如果是 本客户端召测
                {
                    if (g.Op == 13)
                    {
                        if(g.IsSucc ==1 )
                        {
                           
                            foreach (var item in g.ItemWorkArgsSet)
                            {

                                var leakLine = new LeakParameter
                                {
                                    AutoBreakOrAutoAlarm = item.AlarmWithOption,//自动分闸
                                    IsUsed = item.IsUsed == 1,  //? true : false是否使用
                                    LeakId = item.LeakId,
                                    LeakLineId = item.LeakLineId,
                                    UpperAlarmOrBreakforLeakOrTemperature = item.ValueSet,
                                    TimeDelayforBreak = item.TimeDelayOpe

                                };
                                foreach (var t in Items)
                                {
                                  if(t.LeakLineId==item.LeakLineId)
                                    {
                                        t.ZcItems.Clear();
                                        t.ZcItems.Add( new Wj9001Model(leakLine){DataType ="召测"});
                                        t.ZcItems[0].LineName = "[召测]" + t.LineName;
                                        t.ZcItems[0].Index = t.Index;
                                        t.ZcItems[0].IsEdit = false;
                                        if(item.IsUsed !=1)
                                        {
                                            t.ZcItems[0].AutoBreakOrAutoAlarm = t.AutoBreakOrAutoAlarm;
                                            t.ZcItems[0].TimeDelayforBreak = t.TimeDelayforBreak;
                                        }
                                        
                                    }
                                }



                            }


    //                        var t =
    //Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId]
    //as Sr.EquipmentInfoHolding.Model.Wj9001Leak;
    //                        int index = 0;
    //                        for (var i = 0; i < Items.Count; i++)
    //                        {
    //                            if (Items[i].DataType== "召测")
    //                            {
    //                                Items.Remove(Items[i]);
    //                                i--;
    //                            }
                                
    //                        }
    //                        foreach (var item in g.ItemWorkArgsSet)
    //                        {

    //                            var leakLine = new LeakParameter
    //                            {
    //                                LeakMode = item.AlarmWithOption,//自动分闸
    //                                IsUsed = item.IsUsed==1?true:false,  //是否使用
    //                                LeakId = item.LeakId,                               
    //                                LeakLineId = item.LeakLineId,
    //                                UpperAlarmOrBreakforLeakOrTemperature = item.ValueSet,
    //                                TimeDelayforBreak = item.TimeDelayOpe

    //                            };
    //                            foreach (var fff in t.WjLeakLines.Values)
    //                            {
    //                                if (fff.LeakLineId == leakLine.LeakLineId)  leakLine.LineName = fff.LineName;
    //                            }
    //                            index = index + 1;
    //                            Items.Add(new Wj9001Model(leakLine) { DataType = "召测", Index = index });

                                
                                    
    //                        }
    //                        var ntg = (from txr in Items orderby txr.LeakLineId ascending select txr).ToList();
    //                        Items.Clear();
    //                        foreach (var fa in ntg)
    //                        {
    //                            Items.Add(fa);
    //                        }
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "召测数据已返回！";
                        }
                        else
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "召测参数失败！";
                        }
                       
                    }
                    if(g.Op==1)
                    {
                        if(g.IsSucc==1 )
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置漏电地址成功！";
                        }
                        else
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置漏电地址失败！";
                        }
                       
                    }
                    if (g.Op == 2)
                    {
                        if (g.IsSucc == 1)
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置漏电参数成功！";
                        }
                        else
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置漏电参数失败！";
                        }

                    }
                    if (g.Op == 4)
                    {
                        if (g.IsSucc == 1)
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置检查门限值成功！";
                        }
                        else
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置检查门限值失败！";
                        }
                    }
                    if (g.Op == 5)
                    {
                        if (g.IsSucc == 1)
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置时钟成功！";
                        }
                        else
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "设置时钟失败！";
                        }
                    }
                    if (g.Op == 6)
                    {
                        if (g.IsSucc == 1)
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "复位成功！";
                        }
                        else
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "复位失败！";
                        }
                    }
                    if (g.Op == 12)
                    {
                        if (g.IsSucc == 1)
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "召测时钟： " + new DateTime(g.DtTime).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else
                        {
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "召测时钟失败！";
                        }
                    }

                    break;
                }
            }
                //if (!haveDice ) return;  //如果是本客户端选测则继续运行下面代码
            if (OnNavOnLoadSelectdRtus != null)
            {
                OnNavOnLoadSelectdRtus(null, EventArgs.Empty);
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
                    Remind = id == RtuId ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + "数据保存成功!" : "";

                }
            }
            catch (Exception xe)
            {
                WriteLog.WriteLogError("LduInfoSetViewModel error in FundEventHandlers:ex:" + xe);
            }
        }
    }
}
