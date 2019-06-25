using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Ux.Wj1050Module.Wj1050InfoSetViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.Wj1050Module.Wj1050InfoSetViewModel.ViewModel
{
    [Export(typeof (IITmlInformationViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlInformationViewModel : TmlInfomationViewModelBase, IITmlInformationViewModel
    {
        public TmlInformationViewModel()
        {
            this.InitAction();
        }

        private bool isViewActive = false;
        public void NavOnLoad(params object[] parsObjects)
        {
            isViewActive = true;
            try
            {
                DtStartTime = DateTime.Now.AddMonths(-1);
                DtEndTime = DateTime.Now;
                RadioTimeSelectValue = 1;
                RadioMruTypeSelectValue = 4;
                var tmlId = (int) parsObjects[0];
                if (tmlId > 0)
                {
                    this.NavOnLoadByBase(tmlId);
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        public void OnUserHideOrClosing()
        {
            this.ShowInfo = "";
            this.isViewActive = false;
        }

        #region tab iinterface
        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get
            {
                return "抄表设备参数设置";
                return "Map";
            }
        }


        public bool CanClose
        {
            get { return true; }
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

        #endregion
    };



    /// <summary>
    /// data
    /// </summary>
    public partial class TmlInformationViewModel
    {
        private ObservableCollection<MruDataRecordViewModel> _itemsdata;

        public ObservableCollection<MruDataRecordViewModel> Items
        {
            get
            {
                if (_itemsdata == null)
                    _itemsdata = new ObservableCollection<MruDataRecordViewModel>();
                return _itemsdata;
            }
        }


        #region radiobutton

        /// <summary>
        /// 1 Now,2 LastMonth,3 TwoMonthBefore
        /// </summary>
        private int _radioNow;

        public int RadioTimeSelectValue
        {
            get { return _radioNow; }
            set
            {
                if (value != _radioNow)
                {
                    _radioNow = value;
                    this.RaisePropertyChanged(() => this.RadioTimeSelectValue);
                }
            }
        }



        #endregion


        private string _moniter;

        /// <summary>
        /// 显示 即时信息
        /// </summary>

        public string ShowInfo
        {
            get
            {
                if (string.IsNullOrEmpty(_moniter)) _moniter = "";
                return _moniter;
            }
            set
            {
                if (value != _moniter)
                {
                    _moniter = value;
                    this.RaisePropertyChanged(() => this.ShowInfo);

                }

            }
        }

        #region timeselect

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
        /// 节电设备地址
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

        private DateTime[] _dateTimes=new DateTime[6];

        #region CmdZcMruAddr

        private void ExCmdZcDataTwo()
        {
            _dateTimes[0] = DateTime.Now;
            _zcAddr.Clear();
            this.SndZcMruAddr(this.AttachRtuId, this.RtuId);
        }

        private bool CanCmdZcDataTwo()
        {
            if (RtuId > 1000 && this.AttachRtuId > 1000)
                return DateTime.Now.Ticks-_dateTimes[0].Ticks>30000000;
            return false;
        }

        private ICommand _CmdZcDataTwo;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcMruAddr
        {
            get
            {
                return _CmdZcDataTwo ??
                       (_CmdZcDataTwo = new RelayCommand(ExCmdZcDataTwo, CanCmdZcDataTwo, true));
            }
        }

        #endregion

        #region CmdReadData

        private void ExUpdateEsuTime()
        {
            _dateTimes[1] = DateTime.Now;
            this.SndReadMruData(this.AttachRtuId, this.RtuId);
        }

        private bool CanUpdateEsuTime()
        {
            if (RtuId > 1000 && this.AttachRtuId > 1000)
                return DateTime.Now.Ticks-_dateTimes[1].Ticks>30000000;
            return false;
        }

        private ICommand _UpdateEsuTime;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdReadData
        {
            get { return _UpdateEsuTime ?? (_UpdateEsuTime = new RelayCommand(ExUpdateEsuTime, CanUpdateEsuTime, true)); }
        }

        #endregion

        #region CmdQueryMruData  time

        private void ExCmdOpenEsu()
        {
            this.Items.Clear();
            _dateTimes[2] = DateTime.Now;
          //  this.SndQueryMruData(this.RtuId, DtStartTime, DtEndTime);
        }

        private bool CanCmdOpenEsu()
        {
            if (RtuId > 1000 && DtStartTime.Ticks < DtEndTime.Ticks)
                return DateTime.Now.Ticks-_dateTimes[2].Ticks>30000000;
            return false;
        }

        private ICommand _CmdOpenEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdQueryMruData
        {
            get { return _CmdOpenEsu ?? (_CmdOpenEsu = new RelayCommand(ExCmdOpenEsu, CanCmdOpenEsu, true)); }
        }

        #endregion

        #region CmdQueryMruDataAllNew

        private void ExCmdCloseEsu()
        {
            Items.Clear();
            _dateTimes[3] = DateTime.Now;
            this.SndQueryMruData();
        }

        private bool CanCmdCloseEsu()
        {
            if (RtuId > 1000)
                return DateTime.Now.Ticks-_dateTimes[3].Ticks>30000000;
            return false;
        }

        private ICommand _CmdCloseEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdQueryMruDataAllNew
        {
            get { return _CmdCloseEsu ?? (_CmdCloseEsu = new RelayCommand(ExCmdCloseEsu, CanCmdCloseEsu, true)); }
        }

        #endregion

        #region CmdCleanShowInfo

        private void ExCmdManuAdjustVol()
        {
            _dateTimes[4] = DateTime.Now;
            this.ShowInfo = "";
            _zcAddr.Clear();
        }

        private bool CanCmdManuAdjustVol()
        {
            if (!string.IsNullOrEmpty(ShowInfo))
                return DateTime.Now.Ticks-_dateTimes[4].Ticks>30000000;
            return false;
        }

        private ICommand _CmdManuAdjustVol;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdCleanShowInfo
        {
            get
            {
                return _CmdManuAdjustVol ??
                       (_CmdManuAdjustVol = new RelayCommand(ExCmdManuAdjustVol, CanCmdManuAdjustVol, true));
            }
        }

        #endregion

        #region CmdUpdateAddrByZcAddr

        private List<int> _zcAddr = new List<int>();
        private void ExCmdUpdateAddrByZcAddr()
        {
            _dateTimes[5] = DateTime.Now;
            this.MruAddr1 = _zcAddr[0];
            this.MruAddr2 = _zcAddr[1];
            this.MruAddr3 = _zcAddr[2];
            this.MruAddr4 = _zcAddr[3];
            this.MruAddr5 = _zcAddr[4];
            this.MruAddr6 = _zcAddr[5];
        }

        private bool CanCmdUpdateAddrByZcAddr()
        {
            if (_zcAddr.Count >= 6)
            {
                if (this.MruAddr1 != _zcAddr[0] ||
                    this.MruAddr2 != _zcAddr[1] ||
                    this.MruAddr3 != _zcAddr[2] ||
                    this.MruAddr4 != _zcAddr[3] ||
                    this.MruAddr5 != _zcAddr[4] ||
                    this.MruAddr6 != _zcAddr[5])
                    return DateTime.Now.Ticks-_dateTimes[5].Ticks>30000000;
            }
            return false;
        }

        private ICommand _CmdCmdUpdateAddrByZcAddr;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdUpdateAddrByZcAddr
        {
            get
            {
                return _CmdCmdUpdateAddrByZcAddr ??
                       (_CmdCmdUpdateAddrByZcAddr = new RelayCommand(ExCmdUpdateAddrByZcAddr, CanCmdUpdateAddrByZcAddr, true));
            }
        }

        #endregion

    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class TmlInformationViewModel
    {


        public void InitAction()
        {
            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolCnt.ClientPart.wlst_Mru_server_ans_clinet_request_MruRecordData ,
            //    RequestMruRecordData,
            //    typeof(TmlInformationViewModel), this);
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxMru.wst_svr_ans_cnt_read_mru_addr  ,//.ClientPart.wlst_Mru_server_ans_clinet_order_ReadAddr ,
                MruReadAddr,
                typeof(TmlInformationViewModel), this);
            ProtocolServer.RegistProtocol(
               Wlst.Sr.ProtocolPhone.LxMru.wst_svr_ans_cnt_read_mru_data,//.ClientPart.wlst_Mru_server_ans_client_order_ReadData ,
               MruReadData,
               typeof(TmlInformationViewModel), this);
        }

        //public void RequestMruRecordData(string session, ProtocolEncodingCnt<ReplyMruData> info)
        //{
        //    var infos = info.Data;
        //    if (infos == null) return;
        //    if (!infos.RequestAllNewData && infos.RtuId != this.RtuId) return;
        //    this.Items.Clear();
        //    var xxx = (from t in infos.Info orderby t.DateCreate ascending select t).ToList();
        //    if (xxx.Count == 0) return;

        //    double count = 0;
        //    var tmps = xxx[0];
        //    foreach (var t in xxx)
        //    {
        //        double dirr =Math .Round( t.MruData - tmps.MruData,2);
        //        count += dirr;
        //        var iinfo = new MruDataRecordViewModel(t) {Id = this.Items.Count + 1, Differ = dirr,Count =count };
        //        this.Items.Add(iinfo);
        //        tmps = t;
        //    }
        //}

        public void MruReadAddr(string session, Wlst .mobile .MsgWithMobile  info)
        {
            if (isViewActive == false) return;
            var infos = info.WstMruSvrAnsCntRequestReadMruAddr ;
            //var infos = args.GetParams()[1] as ReplyReadMruAddr;
            if (infos == null) return;
            string str = DateTime.Now + " ";  //vs = System.Convert.ToString(integer, 16).Trim();
            str += "电表所在设备地址为:" + infos.MainRtuId + " ; 电表地址序列为：" + System.Convert.ToString(infos.Addr1, 16).Trim() + "," + System.Convert.ToString(infos.Addr2, 16).Trim() + "," +
                   System.Convert.ToString(infos.Addr3, 16).Trim() + "," + System.Convert.ToString(infos.Addr4, 16).Trim() + "," + System.Convert.ToString(infos.Addr5, 16).Trim() + "," + System.Convert.ToString(infos.Addr6, 16).Trim() + Environment.NewLine;
            this.ShowInfo += str;

            _zcAddr.Clear();
            _zcAddr.Add( infos.Addr1);
            _zcAddr.Add(infos.Addr2);
            _zcAddr.Add(infos.Addr3);
            _zcAddr.Add(infos.Addr4);
            _zcAddr.Add(infos.Addr5);
            _zcAddr.Add(infos.Addr6);
        }

        public void MruReadData(string session,Wlst .mobile .MsgWithMobile  infoss)
        {
            if (isViewActive == false) return;
            var infos = infoss.WstMruSvrAnsCntRequestReadMruData   ;
            if (infos == null) return;
            if (infos.RtuId != this.RtuId) return;

            var info = new MruDataRequest.MruDataItem()
            {
                DateCreate = DateTime.Now.Ticks ,
                DateType = infos.DataTimeType,
                MruData = infos.DataValue,
                MruType = infos.DataMruType,
                RtuId = infos.RtuId
            };
            var vm = new MruDataRecordViewModel(info);
            //this.Items.Insert(0, vm);
            string str = DateTime.Now.ToString("HH:mm:ss");
            str += " 电表地址：" + vm.RtuId + ",即时获取" + vm.MruTypeCode + " 在" + Get_Month_String(vm.DateTypeCode) + "当前时间的采集值：" + vm.MruData + ",计算电量为:" + vm.MruTotal +
                   Environment.NewLine;
            if (ShowInfo.Length > 3000) ShowInfo = "";
            this.ShowInfo += str;
        }


        private string Get_Month_String(string _inputString)
        {
            if (_inputString == "当前")
            {
                return "本月";
            }

            return _inputString;
        }
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class TmlInformationViewModel
    {
        private void SndZcMruAddr(int mainRtuId, int rtuId)
        {
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new ReadMruAddr()
            //               {
            //                   RtuId = RtuId ,
            //                   MainRtuId =AttachRtuId 
            //               };


            var info = Wlst.Sr.ProtocolPhone .LxMru  .wst_cnt_read_mru_addr  ;//.ServerPart.wlst_Mru_clinet_order_ReadAddr
                ;
                info.WstMruCntRequestReadMruAddr.RtuId = rtuId;
                info.WstMruCntRequestReadMruAddr.MainRtuId = AttachRtuId;

            SndOrderServer.OrderSnd(info, 10, 6);


        }


        private void SndReadMruData(int mainRtuId, int rtuId)
        {

            var info = Wlst.Sr.ProtocolPhone .LxMru  .wst_cnt_read_mru_data  ;//.ServerPart.wlst_Mru_client_order_ReadData;
            ;


            var mrutype = RadioMruTypeSelectValue ;

            var timetype = RadioTimeSelectValue -1;

            info.WstMruCntRequestReadMruData.RtuId = rtuId;
            info.WstMruCntRequestReadMruData.Addr1 = this.MruAddr1;// System.Convert.ToInt32(this.MruAddr1 + "", 16);
            info.WstMruCntRequestReadMruData.Addr2 = this.MruAddr2;// System.Convert.ToInt32(this.MruAddr2 + "", 16);
            info.WstMruCntRequestReadMruData.Addr3 = this.MruAddr3;// System.Convert.ToInt32(this.MruAddr3 + "", 16);
            info.WstMruCntRequestReadMruData.Addr4 = this.MruAddr4;// System.Convert.ToInt32(this.MruAddr4 + "", 16);
            info.WstMruCntRequestReadMruData.Addr5 = this.MruAddr5;// System.Convert.ToInt32(this.MruAddr5 + "", 16);
            info.WstMruCntRequestReadMruData.Addr6 = this.MruAddr6;// System.Convert.ToInt32(this.MruAddr6 + "", 16); 
            info.WstMruCntRequestReadMruData.MainRtuId = mainRtuId;
            info.WstMruCntRequestReadMruData.DataMruType = mrutype;
            info.WstMruCntRequestReadMruData.DataTimeType = timetype;
            info.WstMruCntRequestReadMruData.Ver = MruType;

            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SndQueryMruData(int rtuId, DateTime dtstart, DateTime dtend)
        {
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new RequestMruData()
            //               {
            //                   RtuId = rtuId,
            //                   DtStartTime = dtstart,
            //                   DtEndTime = dtend,
            //                   RequestAllNewData = false
            //               };
            //SndOrderServer.OrderSnd(Ux.Wj1050Module.Services.EventIdAssign.RequestMruDataId, null,
            //                        info, waitIdUpdate, 10, 6);

            return;
            var info = Wlst.Sr.ProtocolPhone .LxMru  .wst_request_mru_data  ;//.ServerPart.wlst_Mru_clinet_request_MruRecordData
               ;
            info.WstMruRequestData  .RtuId = rtuId;
            info.WstMruRequestData.DtStartTime = dtstart.Ticks;
            info.WstMruRequestData.DtEndTime = dtend.Ticks;
            //info.WstRequestMruData.RequestNewData = false;

            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SndQueryMruData()
        {
            //int waitIdUpdate = Infrastructure.UtilityFunction.TickCount.EnvironmentTickCount;
            //var info = new RequestMruData()
            //               {
            //                   RtuId = 0,
            //                   DtStartTime = DateTime.Now,
            //                   DtEndTime = DateTime.Now,
            //                   RequestAllNewData = true
            //               };
            //SndOrderServer.OrderSnd(Ux.Wj1050Module.Services.EventIdAssign.RequestMruDataId, null,
            //                        info, waitIdUpdate, 10, 6);

            return;
            //var info = Wlst.Sr.ProtocolCnt.ServerPart.wlst_Mru_clinet_request_MruRecordData
            // ;
            //info.Data.RtuId = 0;
            //info.Data.DtStartTime = DateTime.Now;
            //info.Data.DtEndTime = DateTime.Now;
            //info.Data.RequestNewData = true ;

            //SndOrderServer.OrderSnd(info, 10, 6);
        }


    }
}
