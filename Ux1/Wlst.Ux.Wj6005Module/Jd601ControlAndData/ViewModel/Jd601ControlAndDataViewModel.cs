using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Sr.ProtocolCnt.Jd601;
using Wlst.Ux.Wj6005Module.Jd601ControlAndData.Services;


namespace Wlst.Ux.Wj6005Module.Jd601ControlAndData.ViewModel
{


    [Export(typeof (IIJd601ControlAndData))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Jd601ControlAndDataViewModel :
        Wlst.Cr.Core.CoreServices .ObservableObject , IIJd601ControlAndData
    {
        public Jd601ControlAndDataViewModel()
        {
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
            get { return "控制与数据"; }
        }

        #endregion

        #region NavOnLoad

        public void NavOnLoad(params object[] parsObjects)
        {
            try
            {
                Volvalue = 220;
                this.Items.Clear();
                this.OneData = new EsuDataOneItemViewModel();
                this.TwoData.ResetAllArgs();
                DtStartTime = DateTime.Now.AddDays(-1);
                DtEndTime = DateTime.Now;
                ControlText = "";


                var tmlId = (int) parsObjects[0];

                if (tmlId > 0)
                {
                    this.RtuId = tmlId;
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

    }


    /// <summary>
    /// data
    /// </summary>
    public partial class Jd601ControlAndDataViewModel
    {
        #region attri

        private ObservableCollection<EsuDataOneItemViewModel> _itemsdata;

        public ObservableCollection<EsuDataOneItemViewModel> Items
        {
            get { return _itemsdata ?? (_itemsdata = new ObservableCollection<EsuDataOneItemViewModel>()); }
        }

        private EsuDataOneItemViewModel _currentselectDataOne;

        public EsuDataOneItemViewModel OneData
        {
            get { return _currentselectDataOne ?? (_currentselectDataOne = new EsuDataOneItemViewModel()); }
            set
            {
                if (value != _currentselectDataOne)
                {
                    _currentselectDataOne = value;
                    this.RaisePropertyChanged(() => this.OneData);
                }
            }
        }


        private EsuDataTwoItemViewModel _twoData;

        public EsuDataTwoItemViewModel TwoData
        {
            get { return _twoData ?? (_twoData = new EsuDataTwoItemViewModel()); }
            set
            {
                if (value == _twoData) return;
                _twoData = value;
                this.RaisePropertyChanged(() => this.TwoData);
            }
        }

        private string _controldata;

        public string ControlText
        {
            get { return _controldata; }
            set
            {
                if (value != _controldata)
                {
                    _controldata = value;
                    this.RaisePropertyChanged(() => this.ControlText);
                }

            }
        }

        private void AddControlText(string text)
        {
            this.ControlText += DateTime.Now + " " + text + Environment.NewLine;
        }

        private int rtuId;

        /// <summary>
        /// 节能设备地址
        /// </summary>

        public int RtuId
        {
            get { return rtuId; }
            set
            {
                if (value != rtuId)
                {
                    rtuId = value;
                    this.RaisePropertyChanged(() => this.RtuId);

                    var mmm = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetMainEquipmentInfo(
                        rtuId);
                    if (mmm != null)
                    {
                        AttachRtuId = rtuId;

                        RtuName = mmm.RtuName;

                        return;
                    }
                    var gg =
                        Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetAttachEquipmentInfo(
                            rtuId);
                    RtuName = "未知";
                    AttachRtuId = 0;
                    if (gg == null) return;

                    RtuName = gg.RtuName;
                    AttachRtuId = gg.AttachRtuId;
                }

            }
        }

        private string rtuName;

        /// <summary>
        /// 节能设备
        /// </summary>

        public string RtuName
        {
            get { return rtuName; }
            set
            {
                if (value != rtuName)
                {
                    rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }

            }
        }

        #endregion

        #region arrti control


        private int volvalue;

        /// <summary>
        /// 手动调压值
        /// </summary>

        public int Volvalue
        {
            get { return volvalue; }
            set
            {
                if (value != volvalue)
                {
                    volvalue = value;
                    this.RaisePropertyChanged(() => this.Volvalue);

                }

            }
        }


        private DateTime dtstart;

        /// <summary>
        /// 查询数据 起始时间
        /// </summary>

        public DateTime DtStartTime
        {
            get { return dtstart; }
            set
            {
                if (value != dtstart)
                {
                    dtstart = value;
                    this.RaisePropertyChanged(() => this.DtStartTime);

                }

            }
        }


        private DateTime dtEndtime;

        /// <summary>
        /// 节能设备地址
        /// </summary>

        public DateTime DtEndTime
        {
            get { return dtEndtime; }
            set
            {
                if (value != dtEndtime)
                {
                    dtEndtime = value;
                    this.RaisePropertyChanged(() => this.DtEndTime);

                }

            }
        }

        #endregion

        #region CmdRequestOneData

        private void ExCmdQueryOneData()
        {
            //todo

            this.RequestOneData(DtStartTime, DtEndTime, RtuId);
        }

        private bool CanCmdQueryOneData()
        {
            if (RtuId > 1000 && DtEndTime.Ticks > DtStartTime.Ticks)
                return true;
            return false;
        }

        private ICommand _CmdQueryOneData;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdRequestOneData
        {
            get
            {
                return _CmdQueryOneData ??
                       (_CmdQueryOneData = new RelayCommand(ExCmdQueryOneData, CanCmdQueryOneData, true));
            }
        }

        #endregion

        #region CmdZcDataOne

        private void ExCmdZcDataOne()
        {
            this.ZcJd601DataOne(this.AttachRtuId, this.RtuId);
        }

        private bool CanCmdZcDataOne()
        {
            if (RtuId > 1000)
                return true;
            return false;
        }

        private ICommand _CmdZcDataOne;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcDataOne
        {
            get { return _CmdZcDataOne ?? (_CmdZcDataOne = new RelayCommand(ExCmdZcDataOne, CanCmdZcDataOne, true)); }
        }

        #endregion

        #region CmdZcDataTwo

        private void ExCmdZcDataTwo()
        {
            this.ZcJd601DataTwo(this.AttachRtuId, this.RtuId);
        }

        private bool CanCmdZcDataTwo()
        {
            if (RtuId > 1000)
                return true;
            return false;
        }

        private ICommand _CmdZcDataTwo;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdCmdZcDataTwo
        {
            get
            {
                return _CmdZcDataTwo ??
                       (_CmdZcDataTwo = new RelayCommand(ExCmdZcDataTwo, CanCmdZcDataTwo, true));
            }
        }

        #endregion

        #region CmdUpdateEsuTime

        private void ExUpdateEsuTime()
        {
            this.UpdateEsuTime(this.AttachRtuId, this.RtuId);
        }

        private bool CanUpdateEsuTime()
        {
            if (RtuId > 1000)
                return true;
            return false;
        }

        private ICommand _UpdateEsuTime;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdUpdateEsuTime
        {
            get { return _UpdateEsuTime ?? (_UpdateEsuTime = new RelayCommand(ExUpdateEsuTime, CanUpdateEsuTime, true)); }
        }

        #endregion

        #region CmdOpenEsu

        private void ExCmdOpenEsu()
        {
            this.OpenCloseEsu(this.AttachRtuId, this.RtuId, true);
        }

        private bool CanCmdOpenEsu()
        {
            if (RtuId > 1000)
                return true;
            return false;
        }

        private ICommand _CmdOpenEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdOpenEsu
        {
            get { return _CmdOpenEsu ?? (_CmdOpenEsu = new RelayCommand(ExCmdOpenEsu, CanCmdOpenEsu, true)); }
        }

        #endregion

        #region CmdCloseEsu

        private void ExCmdCloseEsu()
        {
            this.OpenCloseEsu(this.AttachRtuId, this.RtuId, false);
        }

        private bool CanCmdCloseEsu()
        {
            if (RtuId > 1000)
                return true;
            return false;
        }

        private ICommand _CmdCloseEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdCloseEsu
        {
            get { return _CmdCloseEsu ?? (_CmdCloseEsu = new RelayCommand(ExCmdCloseEsu, CanCmdCloseEsu, true)); }
        }

        #endregion

        #region CmdManuAdjustVol

        private void ExCmdManuAdjustVol()
        {
            this.ManuAdjustVol(this.AttachRtuId, this.RtuId, Volvalue);
        }

        private bool CanCmdManuAdjustVol()
        {
            if (RtuId > 1000 && Volvalue > 119 && Volvalue < 299)
                return true;
            return false;
        }

        private ICommand _CmdManuAdjustVol;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdManuAdjustVol
        {
            get
            {
                return _CmdManuAdjustVol ??
                       (_CmdManuAdjustVol = new RelayCommand(ExCmdManuAdjustVol, CanCmdManuAdjustVol, true));
            }
        }

        #endregion
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class Jd601ControlAndDataViewModel
    {



        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_order_AsynTime ,
                AsynJd601Time,
                typeof(Jd601ControlAndDataViewModel), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_order_OpenClostEsu ,
                OpenCloseJd601,
                typeof(Jd601ControlAndDataViewModel), this);

            ProtocolServer.RegistProtocol(
              Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_order_MeasureOne ,
              MeasereJd601One,
              typeof(Jd601ControlAndDataViewModel), this);
            ProtocolServer.RegistProtocol(
              Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_order_MeasureTwo ,
              MeasureJd601Two,
              typeof(Jd601ControlAndDataViewModel), this);

            ProtocolServer.RegistProtocol(
              Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_order_AdjustVol ,
              ManuAdjustVolByUser,
              typeof(Jd601ControlAndDataViewModel), this);
            ProtocolServer.RegistProtocol(
              Wlst.Sr.ProtocolCnt.ClientPart.wlst_Jd601_server_ans_clinet_request_MeasureData,
              RequestJd601PartlData,
              typeof(Jd601ControlAndDataViewModel), this);
        }

        public void AsynJd601Time(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ReplyEsuControl> args)
        {
            var infos = args.Data;
            if (infos == null) return;
            if (infos.RtuId == RtuId)
            {
                this.AddControlText(infos.IsSuccesfull ? "时间同步成功!!!" : "时间同步失败...");
            }
        }
        public void OpenCloseJd601(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ReplyEsuControl> args)
        {
            var infos = args.Data;
            if (infos == null) return;
            if (infos.RtuId == RtuId)
            {
                this.AddControlText(infos.IsSuccesfull ? "手动操作成功!!!" : "手动操作失败...");
            }
        }

        public void MeasereJd601One(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ReplyEsuData> args)
        {
            var infos = args.Data;
            if (infos == null) return;
            if (RtuId != infos.RtuId) return;
            if (!infos.LastOneRecord || infos.Info.Count <= 0) return;
            var infosss = new EsuDataOneItemViewModel(infos.Info[0]);
            this.Items.Add(infosss);
            this.OneData = infosss;
        }
        public void MeasureJd601Two(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ReplyEsuDataTwo> args)
        {
            var infos = args.Data;
            if (infos == null) return;
            if (RtuId != infos.RtuId) return;
            var infosss = new EsuDataTwoItemViewModel(infos.Info);
            this.TwoData = infosss;
        }

        public void ManuAdjustVolByUser(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ReplyEsuControl> args)
        {
            var infos = args.Data;
            if (infos == null) return;
            if (infos.RtuId == RtuId)
            {
                this.AddControlText(infos.IsSuccesfull ? "手动调压成功!!!" : "手动调压失败...");
            }
        }
        public void RequestJd601PartlData(string session, Wlst.Cr.PPProtocolSvrCnt.Common.ProtocolEncodingCnt<ReplyEsuData> args)
        {
            var infos = args.Data;
            if (infos == null) return;
            if (infos.Info.Count == 0) return;
            this.Items.Clear();
            foreach (var t in infos.Info)
            {
                this.Items.Add(new EsuDataOneItemViewModel(t));
            }
            if (this.Items.Count > 0) this.OneData = this.Items[0];
        }

    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class Jd601ControlAndDataViewModel
    {
        protected int AttachRtuId;

        private void RequestOneData(DateTime dtstart, DateTime dtendtime, int rtuid)
        {


            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new RequestEsuData()
            //               {
            //                   DtEndTime = dtendtime,
            //                   DtStartTime = dtstart,
            //                   LastOneRecord = false,
            //                   RtuId = RtuId
            //               };
            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.RequestJd601PartlData, null,
            //                        info, waitIdUpdate, 10, 6);

            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_request_MeasureData;
            nt.Data.DtEndTime = dtendtime;
            nt.Data.DtStartTime = dtstart;
            nt.Data.LastOneRecord = false;
            nt.Data.RtuId = RtuId;
            SndOrderServer.OrderSnd(nt, 10, 3);

        }


        private void ZcJd601DataOne(int mainRtuId, int rtuId)
        {
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new ExchangeZc()
            //               {
            //                   AttachRtuId = mainRtuId,
            //                   RtuId = rtuId,
            //               };
            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.MeasereJd601One, null,
            //                        info, waitIdUpdate, 10, 6);

            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_order_MeasureOne;
            nt.Data.AttachRtuId = mainRtuId;
            nt.Data.RtuId = rtuId;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }

        private void ZcJd601DataTwo(int mainRtuId, int rtuId)
        {
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new ExchangeZc()
            //               {
            //                   AttachRtuId = mainRtuId,
            //                   RtuId = rtuId,
            //               };
            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.MeasureJd601Two, null,
            //                        info, waitIdUpdate, 10, 6);

            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_order_MeasureTwo;
            nt.Data.AttachRtuId = mainRtuId;
            nt.Data.RtuId = rtuId;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }


        private void UpdateEsuTime(int mainRtuId, int rtuId)
        {
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new EsuControl()
            //               {
            //                   RtuId = rtuId,
            //                   AttachRtuId = mainRtuId,
            //                   DtTimeNow = DateTime.Now,
            //               };
            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.AsynJd601Time, null,
            //                        info, waitIdUpdate, 10, 6);

            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_order_AsynTime;
            nt.Data.AttachRtuId = mainRtuId;
            nt.Data.RtuId = rtuId;
            nt.Data.DtTimeNow = DateTime.Now;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }

        private void OpenCloseEsu(int mainRtuId, int rtuId, bool isopen)
        {
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new EsuControl()
            //               {
            //                   RtuId = rtuId,
            //                   AttachRtuId = mainRtuId,
            //                   DtTimeNow = DateTime.Now,
            //                   IsOpenEsu = isopen
            //               };
            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.OpenCloseJd601, null,
            //                        info, waitIdUpdate, 10, 6);

            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_order_OpenClostEsu;
            nt.Data.AttachRtuId = mainRtuId;
            nt.Data.RtuId = rtuId;
            nt.Data.DtTimeNow = DateTime.Now;
            nt.Data.IsOpenEsu = isopen;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }

        private void ManuAdjustVol(int mainRtuId, int rtuId, int volValue)
        {
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new EsuControl()
            //               {
            //                   RtuId = rtuId,
            //                   AttachRtuId = mainRtuId,
            //                   DtTimeNow = DateTime.Now,
            //                   IsOpenEsu = false,
            //                   ManuVolVaule = volValue
            //               };
            //SndOrderServer.OrderSnd(Ux.Wj6005Module.Services.EventIdAssign.ManuAdjustVolByUser, null,
            //                        info, waitIdUpdate, 10, 6);

            var nt = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Jd601_clinet_order_AdjustVol;
            nt.Data.AttachRtuId = mainRtuId;
            nt.Data.RtuId = rtuId;
            nt.Data.DtTimeNow = DateTime.Now;
            nt.Data.IsOpenEsu = false;
            nt.Data.ManuVolVaule = volValue;
            SndOrderServer.OrderSnd(nt, 10, 3);
        }
    }
}
