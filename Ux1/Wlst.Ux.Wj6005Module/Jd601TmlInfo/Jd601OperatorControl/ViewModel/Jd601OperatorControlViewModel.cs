using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Ux.Wj6005Module.Jd601TmlInfo.Jd601OperatorControl.Service;
using Wlst.client;

namespace Wlst.Ux.Wj6005Module.Jd601TmlInfo.Jd601OperatorControl.ViewModel
{
    [Export(typeof(IIJd601OperatorControl))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601OperatorControlViewModel : Cr.Core.CoreServices .ObservableObject , IIJd601OperatorControl
    {
        public  Jd601OperatorControlViewModel()
        {
            InitAction();
        }
        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "操作控制"; }
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

        public void NavOnLoad(params object[] parsObjects)
        {
            Volvalue = 220;
            Items.Clear();
            OneData = new EsuDataOneItemViewModel();
            //this.TwoData.ResetAllArgs();
            DtStartTime = DateTime.Now.AddDays(-1);
            DtEndTime = DateTime.Now;
            ControlText = "";


            var tmlId = (int)parsObjects[0];

            if (tmlId > 0)
            {
                RtuId = tmlId;
            }
        }
    }

    /// <summary>
    /// Attr,ICommand,Field, DefineFunction
    /// </summary>
    public partial class Jd601OperatorControlViewModel
    {
        #region Field

        private int _attachRtuId;
        #endregion
        #region Attri
        #region  Items
        private ObservableCollection<EsuDataOneItemViewModel> _itemsdata;

        public ObservableCollection<EsuDataOneItemViewModel> Items
        {
            get { return _itemsdata ?? (_itemsdata = new ObservableCollection<EsuDataOneItemViewModel>()); }
        }
        #endregion
        #region OneData
        private EsuDataOneItemViewModel _currentselectDataOne;

        public EsuDataOneItemViewModel OneData
        {
            get { return _currentselectDataOne ?? (_currentselectDataOne = new EsuDataOneItemViewModel()); }
            set
            {
                if (value == _currentselectDataOne) return;
                _currentselectDataOne = value;
                RaisePropertyChanged(() => OneData);
            }
        }
        #endregion
        #region ControlText
        private string _controldata;

        public string ControlText
        {
            get { return _controldata; }
            set
            {
                if (value == _controldata) return;
                _controldata = value;
                RaisePropertyChanged(() => ControlText);
            }
        }
        #endregion
        #region DtStartTime
        private DateTime _dtstart;

        /// <summary>
        /// 查询数据 起始时间
        /// </summary>

        public DateTime DtStartTime
        {
            get { return _dtstart; }
            set
            {
                if (value == _dtstart) return;
                _dtstart = value;
                RaisePropertyChanged(() => DtStartTime);
            }
        }
        #endregion
        #region DtEndTime
        private DateTime _dtEndtime;

        /// <summary>
        /// 节能设备地址
        /// </summary>

        public DateTime DtEndTime
        {
            get { return _dtEndtime; }
            set
            {
                if (value == _dtEndtime) return;
                _dtEndtime = value;
                RaisePropertyChanged(() => DtEndTime);
            }
        }
        #endregion
        #region RtuId
        private int _rtuId;
        /// <summary>
        /// 节能设备地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;
                    RaisePropertyChanged(() => RtuId);

                    //var mmm = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                    //    _rtuId);
                    //if (mmm != null)
                    //{
                    //    _attachRtuId = _rtuId;

                    //    RtuName = mmm.RtuName;

                    //    return;
                    //}
                    var gg =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                            _rtuId);
                    RtuName = "未知";
                    _attachRtuId = 0;
                    if (gg == null) return;

                    RtuName = gg.RtuName;
                    _attachRtuId = gg.RtuFid;
                }

            }
        }
        #endregion
        #region RtuName
        private string _rtuName;

        /// <summary>
        /// 节能设备
        /// </summary>

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value == _rtuName) return;
                _rtuName = value;
                RaisePropertyChanged(() => RtuName);
            }
        }

        private string _rtuNsdfme;

        /// <summary>
        /// 节能设备
        /// </summary>

        public string Msg
        {
            get { return _rtuNsdfme; }
            set
            {
                if (value == _rtuNsdfme) return;
                _rtuNsdfme = value;
                RaisePropertyChanged(() => Msg);
            }
        }
      

        #endregion
        #region Volvalue
        private int _volvalue;

        /// <summary>
        /// 手动调压值
        /// </summary>

        public int Volvalue
        {
            get { return _volvalue; }
            set
            {
                if (value == _volvalue) return;
                _volvalue = value;
                RaisePropertyChanged(() => Volvalue);
            }
        }
        #endregion
        #region IsCheckedRtu

        private bool _ischeckedrtu;

        /// <summary>
        /// 是否查询所有节电设备 lvf 2018年7月12日09:54:28  sb苏州
        /// </summary>
        public bool IsCheckedAllRtu
        {
            get { return _ischeckedrtu; }
            set
            {
                if (value != _ischeckedrtu)
                {
                    _ischeckedrtu = value;
                    this.RaisePropertyChanged(() => this.IsCheckedAllRtu);

                }
            }
        }
        #endregion


        #endregion
        #region ICommand
        private DateTime [] _dateTimes=new DateTime[5];

        #region CmdUpdateEsuTime

        private void ExUpdateEsuTime()
        {
            _dateTimes[0] = DateTime.Now;
            UpdateEsuTime(_attachRtuId, RtuId);
        }

        private bool CanUpdateEsuTime()
        {
            if (RtuId > 1000)
                return DateTime.Now.Ticks-_dateTimes[0].Ticks>30000000;
            return false;
        }

        private ICommand _updateEsuTime;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdUpdateEsuTime
        {
            get { return _updateEsuTime ?? (_updateEsuTime = new RelayCommand(ExUpdateEsuTime, CanUpdateEsuTime, true)); }
        }

        #endregion

        #region CmdOpenEsu

        private void ExCmdOpenEsu()
        {
            _dateTimes[1] = DateTime.Now;
            OpenCloseEsu(_attachRtuId, RtuId, true);
        }

        private bool CanCmdOpenEsu()
        {
            if (RtuId > 1000)
                return DateTime.Now.Ticks - _dateTimes[1].Ticks > 30000000;
            return false;
        }

        private ICommand _cmdOpenEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdOpenEsu
        {
            get { return _cmdOpenEsu ?? (_cmdOpenEsu = new RelayCommand(ExCmdOpenEsu, CanCmdOpenEsu, true)); }
        }

        #endregion

        #region CmdCloseEsu

        private void ExCmdCloseEsu()
        {
            _dateTimes[2] = DateTime.Now;
            OpenCloseEsu(_attachRtuId, RtuId, false);
        }

        private bool CanCmdCloseEsu()
        {
            if (RtuId > 1000)
                return DateTime.Now.Ticks - _dateTimes[2].Ticks > 30000000;
            return false;
        }

        private ICommand _cmdCloseEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdCloseEsu
        {
            get { return _cmdCloseEsu ?? (_cmdCloseEsu = new RelayCommand(ExCmdCloseEsu, CanCmdCloseEsu, true)); }
        }

        #endregion

        #region CmdManuAdjustVol

        private void ExCmdManuAdjustVol()
        {
            _dateTimes[3] = DateTime.Now;
            ManuAdjustVol(_attachRtuId, RtuId, Volvalue);
        }

        private bool CanCmdManuAdjustVol()
        {
            return RtuId > 1000 && Volvalue > 119 && Volvalue < 299  && DateTime.Now.Ticks-_dateTimes[3].Ticks>30000000;;
        }

        private ICommand _cmdManuAdjustVol;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdManuAdjustVol
        {
            get
            {
                return _cmdManuAdjustVol ??
                       (_cmdManuAdjustVol = new RelayCommand(ExCmdManuAdjustVol, CanCmdManuAdjustVol, true));
            }
        }

        #endregion

        #region CmdRequestOneData

        private void ExCmdQueryOneData()
        {
            _dateTimes[4] = DateTime.Now;
            IsCheckedAllRtu = false;
            RequestOneData(DtStartTime, DtEndTime);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
        }

        private bool CanCmdQueryOneData()
        {
            return RtuId > 1000 && DtEndTime.Ticks > DtStartTime.Ticks  && DateTime.Now.Ticks-_dateTimes[4].Ticks>30000000;;
        }

        private ICommand _cmdQueryOneData;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdRequestOneData
        {
            get
            {
                return _cmdQueryOneData ??
                       (_cmdQueryOneData = new RelayCommand(ExCmdQueryOneData, CanCmdQueryOneData, true));
            }
        }

        #endregion

        #region CmdRequestAllData

        //记录终端与节电器的对应关系
        public ConcurrentDictionary<int, int> rtuAttachInfo = new ConcurrentDictionary<int, int>(); 
        public  List<int> Rtulst = new List<int>(); 
        private void ExCmdQueryAllData()
        {
            _dateTimes[4] = DateTime.Now;
            IsCheckedAllRtu = true;
            Rtulst.Clear();
            rtuAttachInfo.Clear();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.RtuId > 1200000 && t.Value.RtuId < 1300000)
                {
                    if (Rtulst.Contains(t.Value.RtuId) == false)
                    {
                        Rtulst.Add(t.Value.RtuId);

                    }
                    if (rtuAttachInfo.ContainsKey(t.Value.RtuId) == false)
                        rtuAttachInfo.TryAdd(t.Value.RtuId, t.Value.RtuFid);
                }

            }
            RequestAllData(DtStartTime, DtEndTime);

            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
        }

        private bool CanCmdQueryAllData()
        {
            return  DtEndTime.Ticks > DtStartTime.Ticks && DateTime.Now.Ticks - _dateTimes[4].Ticks > 30000000; ;
        }

        private ICommand _cmdQueryAllData;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdRequestAllData
        {
            get
            {
                return _cmdQueryAllData ??
                       (_cmdQueryAllData = new RelayCommand(ExCmdQueryAllData, CanCmdQueryAllData, true));
            }
        }

        #endregion
        #endregion

        #region DefineFunction
        private void AddControlText(string text)
        {
            ControlText += DateTime.Now + " " + text + Environment.NewLine;
        }
        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class Jd601OperatorControlViewModel
    {
        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_request_measure_data ,//.ClientPart.wlst_Jd601_server_ans_clinet_request_MeasureData,
                RequestJd601PartlData,
                typeof(Jd601OperatorControlViewModel), this);

            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_order_asyn_time ,//.ClientPart.wlst_Jd601_server_ans_clinet_order_AsynTime,
                AsynJd601Time,
                typeof(Jd601OperatorControlViewModel), this);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_order_open_close ,//.ClientPart.wlst_Jd601_server_ans_clinet_order_OpenClostEsu,
                OpenCloseJd601,
                typeof(Jd601OperatorControlViewModel), this);


            ProtocolServer.RegistProtocol(
              Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_jd601_order_adjust_vol ,//.ClientPart.wlst_Jd601_server_ans_clinet_order_AdjustVol,
              ManuAdjustVolByUser,
              typeof(Jd601OperatorControlViewModel), this);

        }

        public void AsynJd601Time(string session,Wlst .mobile .MsgWithMobile  args)
        {
            var infos = args.WstSvrAnsCntOrderJd601Control ;
            if (infos == null) return;
            if (infos.RtuId == RtuId)
            {
                AddControlText(infos.IsSuccesfull ? "时间同步成功!!!" : "时间同步失败...");
            }
        }
        public void OpenCloseJd601(string session, Wlst .mobile .MsgWithMobile  args)
        {
            var infos = args.WstSvrAnsCntOrderJd601Control;
            if (infos == null) return;
            if (infos.RtuId == RtuId)
            {
                AddControlText(infos.IsSuccesfull ? "手动操作成功!!!" : "手动操作失败...");
            }
        }
        public void ManuAdjustVolByUser(string session, Wlst .mobile .MsgWithMobile  args)
        {
            var infos = args.WstSvrAnsCntOrderJd601Control ;
            if (infos == null) return;
            if (infos.RtuId == RtuId)
            {
                AddControlText(infos.IsSuccesfull ? "手动调压成功!!!" : "手动调压失败...");
            }
        }

        public void RequestJd601PartlData(string session, Wlst .mobile .MsgWithMobile  args)
        {
            Msg =DateTime .Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询信息已经反馈，请查看!!!";
            var infos = args.WstSvrAnsCntRequestOrMeasureJd601Data;
            if (infos == null) return;
            if (infos.Info.Count == 0) return;
            Items.Clear();
           
            var index = 1;
            foreach (var t in infos.Info)
            {
                var rtuid = 0;
                if (IsCheckedAllRtu)
                {
                    if (rtuAttachInfo.ContainsKey(t.RtuId) == false) continue;
                    rtuid = rtuAttachInfo[t.RtuId];
                }

                Items.Add(new EsuDataOneItemViewModel(t, rtuid, index));
                if (Rtulst.Contains(t.RtuId)) Rtulst.Remove(t.RtuId);
                index++;
            }
            if (IsCheckedAllRtu)
            {
                //没有数据的节电器补全
                if (Rtulst.Count > 0)
                {
                    foreach (var g in Rtulst)
                    {
                        if (rtuAttachInfo.ContainsKey(g) == false) continue;
                        var rtuid = rtuAttachInfo[g];
                        Items.Add(new EsuDataOneItemViewModel()
                                      {
                                          Index = index,
                                          AttachId = g,
                                          RtuId = rtuid,
                                          Id = "---",
                                          EsuTemperature = "---",
                                          EsuTargetValue = "---",
                                          EsuRunTime = "---",
                                          EsuOutputCurrentpA = "---",
                                          EsuOutputCurrentpB = "---",
                                          EsuOutputCurrentpC = "---",
                                      });
                        index++;
                    }
                }
            }

            if (Items.Count > 0) OneData = Items[0];
            //  Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询："
                    Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  记录查询成功，共计" + Items .Count+ " 条数据.";
        }    
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class Jd601OperatorControlViewModel
    {
        private void RequestOneData(DateTime dtstart, DateTime dtendtime)
        {
            var tStartTime = new DateTime(dtstart.Year, dtstart.Month, dtstart.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59);

            var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_request_measure_data;//.ServerPart.wlst_Jd601_clinet_request_MeasureData;
            nt.WstCntRequestJd601Data .DtEndTime = tEndTime.Ticks ;
            nt.WstCntRequestJd601Data.DtStartTime = tStartTime.Ticks ;
            nt.WstCntRequestJd601Data.LastOneRecord = false;
            nt.WstCntRequestJd601Data.OP = 1;
            var rtulst = new List<int>();
            rtulst.Add(RtuId);
            nt.WstCntRequestJd601Data.RtuId = rtulst;
            SndOrderServer.OrderSnd(nt, 10, 3);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";

        }

        private void RequestAllData(DateTime dtstart, DateTime dtendtime)
        {
            var tStartTime = new DateTime(dtstart.Year, dtstart.Month, dtstart.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59);

            var nt = Sr.ProtocolPhone.ServerListen.wlst_cnt_jd601_request_measure_data;//.ServerPart.wlst_Jd601_clinet_request_MeasureData;
            nt.WstCntRequestJd601Data.DtEndTime = tEndTime.Ticks;
            nt.WstCntRequestJd601Data.DtStartTime = tStartTime.Ticks;
            nt.WstCntRequestJd601Data.LastOneRecord = false;
            nt.WstCntRequestJd601Data.OP = 2;
            nt.WstCntRequestJd601Data.RtuId = Rtulst;
            SndOrderServer.OrderSnd(nt, 10, 3);
            Msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";

        }
        private void UpdateEsuTime(int mainRtuId, int rtuId)
        {

            var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_order_asyn_time ;//.ServerPart.wlst_Jd601_clinet_order_AsynTime;
            nt.WstCntOrderJd601Control .AttachRtuId = mainRtuId;
            nt.WstCntOrderJd601Control.RtuId = rtuId;
            nt.WstCntOrderJd601Control.DtTimeNow = DateTime.Now.Ticks ;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }
        private void OpenCloseEsu(int mainRtuId, int rtuId, bool isopen)
        {

            var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_order_open_close ;//.ServerPart.wlst_Jd601_clinet_order_OpenClostEsu;
            nt.WstCntOrderJd601Control.AttachRtuId = mainRtuId;
            nt.WstCntOrderJd601Control.RtuId = rtuId;
            nt.WstCntOrderJd601Control.DtTimeNow = DateTime.Now.Ticks ;
            nt.WstCntOrderJd601Control.IsOpenEsu = isopen;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }
        private void ManuAdjustVol(int mainRtuId, int rtuId, int volValue)
        {

            var nt = Sr.ProtocolPhone .ServerListen .wlst_cnt_jd601_order_adjust_vol ;//.ServerPart.wlst_Jd601_clinet_order_AdjustVol;
            nt.WstCntOrderJd601Control.AttachRtuId = mainRtuId;
            nt.WstCntOrderJd601Control.RtuId = rtuId;
            nt.WstCntOrderJd601Control.DtTimeNow = DateTime.Now.Ticks ;
            nt.WstCntOrderJd601Control.IsOpenEsu = false;
            nt.WstCntOrderJd601Control.ManuVolVaule = volValue;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }
    }
}
