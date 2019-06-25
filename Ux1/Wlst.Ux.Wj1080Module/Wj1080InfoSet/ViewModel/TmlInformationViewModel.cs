using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using SpeechLib;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreMims.TopDataInfo;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.View;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride.WlstMessageBox.ViewModel;
using Wlst.Cr.PPProtocolSvrCnt.Common;

using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Ux.Wj1080Module.Wj1080InfoSet.Services;
using Wlst.Ux.Wj1080Module.Wj1080ManageSettingViewModel.ViewModel;
using Wlst.client;

namespace Wlst.Ux.Wj1080Module.Wj1080InfoSet.ViewModel
{
    [Export(typeof (IITmlInformationViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TmlInformationViewModel : TmlInfomationViewModelBase, IITmlInformationViewModel
    {
        public TmlInformationViewModel()
        {
            this.InitAction();
            //th = new Thread(Run);
            //th.Start();
            
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            int tmlId = 0;
            var ids = parsObjects[0] as List<int>;
            if (ids == null || ids.Count<2)  return;

            tmlId = (int)ids[0];
            SelectIndex = (int)ids[1];



            //var tmlId = (int) parsObjects[0];
            ItemCount = 0;
            PageTotal = "";
            this.ShowInfo = "";
            DtStartTime = DateTime.Now.AddDays(-1);
            DtEndTime = DateTime.Now;
            if (tmlId > 0)
            {
                this.NavOnLoadByBase(tmlId);
                


                this.LuxWorkMode = base.LuxWorkMode;

                this.ReportTime = 10;

            }
            if (SelectIndex ==2)ExCmdOpenEsu();
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
                return "光控参数设置";
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

    //public partial class TmlInformationViewModel
    //{
    //    private void OnTask()
    //    {
    //        this.UpdateLuxData(1400001, (DateTime.Now.Ticks%10000)/10.0);
    //        this.UpdateLuxData(1400002, (DateTime.Now.Ticks%10000)/11.0);
    //        this.UpdateLuxData(1400003, (DateTime.Now.Ticks%10000)/15.0);
    //    }

    //    private Thread th;

    //    private void Run()
    //    {
    //        while (true )
    //        {
    //            OnTask();
    //            Thread.Sleep(10000);
    //        }
    //    }
    //}

    //data
    public partial class TmlInformationViewModel
    {
        private DateTime[] _dateTimes=new DateTime[8];

        #region Remind

        private string _remind;
        public string Remind
        {
            get { return _remind; }
            set
            {
                if (value == _remind) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }
        #endregion

        #region  保存终端信息

        private void SubmitExecute()
        {
            _dateTimes[0] = DateTime.Now;

            var ins = BackViewModelToTerminalInformation();

            if (ins == null) return;
            ins.WjLux.LuxWorkMode = this.LuxWorkMode;

            foreach (
               var tt in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
               )
            {
                if (tt.Value.RtuModel == EnumRtuModel.Wj1080 && tt.Value.RtuFid == ins.RtuFid && tt.Value.RtuFid != 0 && tt.Value.RtuId != ins.RtuId)
                {
                    if (tt.Value.RtuPhyId == ins.RtuPhyId)
                    {
                        WlstMessageBox.Show("无法保存", "该设备物理地址重复！", WlstMessageBoxType.Ok);
                        break;
                    }
                }

            }

            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.UpdateEquipmentInfo(ins);


        }


        private RelayCommand _relayCommand;

        /// <summary>
        /// 提交更新  保存终端信息你
        /// </summary>
        public ICommand SaveAllCommand
        {
            get { return _relayCommand ?? (_relayCommand = new RelayCommand(SubmitExecute, CanExSaveAll, true)); }
        }
        private bool CanExSaveAll()
        {
            return DateTime.Now.Ticks-_dateTimes[0].Ticks>30000000;
        }

        #endregion

        #region QueryLuxData

        private ObservableCollection<LuxRecoredViewModel> _itemsdata;

        public ObservableCollection<LuxRecoredViewModel> Items
        {
            get
            {
                if (_itemsdata == null)
                    _itemsdata = new ObservableCollection<LuxRecoredViewModel>();
                return _itemsdata;
            }
            set
            {
                if (_itemsdata == value) return;
                _itemsdata = value;
                this.RaisePropertyChanged(() => this.Items);
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

        #region QueryLuxData  time

        private void ExCmdOpenEsu()
        {
            _dateTimes[1] = DateTime.Now;
            //this.SndQueryLuxData(this.RtuId, DtStartTime, DtEndTime);
            PageIndex = 0;
            RequestHttpData(this.RtuId, DtStartTime, DtEndTime, PageIndex, 0);
        }

        private bool CanCmdOpenEsu()
        {
            if (RtuId > 1000 && DtStartTime.Ticks < DtEndTime.Ticks)
                return DateTime.Now.Ticks-_dateTimes[1].Ticks>30000000;
            return false;
        }

        private ICommand _CmdOpenEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdQueryLuxData
        {
            get { return _CmdOpenEsu ?? (_CmdOpenEsu = new RelayCommand(ExCmdOpenEsu, CanCmdOpenEsu, true)); }
        }

        #endregion

        #endregion

        #region ZcData

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

        #region TabSelectIndex

        private int _selectIndex;

        public int SelectIndex
        {
            get { return _selectIndex; }
            set
            {
                if (value != _selectIndex)
                {
                    _selectIndex = value;
                    RaisePropertyChanged(() => SelectIndex);
                }
            }
        }

        #endregion

        private string _runmode;

        /// <summary>
        /// 终端运行模式
        /// </summary>

        public string RunMode
        {
            get { return _runmode; }
            set
            {
                if (value != _runmode)
                {
                    _runmode = value;
                    this.RaisePropertyChanged(() => this.RunMode);
                }
            }
        }



        private string _runReportTime;

        /// <summary>
        /// 终端主报时间
        /// </summary>

        public string RunReportTime
        {
            get { return _runReportTime; }
            set
            {
                if (value != _runReportTime)
                {
                    _runReportTime = value;
                    this.RaisePropertyChanged(() => this.RunReportTime);
                }
            }
        }



        private string _runVersion;

        /// <summary>
        /// 终端软件版本
        /// </summary>

        public string RunVersion
        {
            get { return _runVersion; }
            set
            {
                if (value != _runVersion)
                {
                    _runVersion = value;
                    this.RaisePropertyChanged(() => this.RunVersion);
                }
            }
        }

        #endregion

        #region SetMode


        private int _setintmode;

        /// <summary>
        ///  模式 1 选测应答, 2 、 设定主报时间, 4 新-设定主报时间
        /// </summary>

        public   int LuxWorkMode
        {
            get { return _setintmode; }
            set
            {
                if (value != _setintmode)
                {
                    _setintmode = value;
                    this.RaisePropertyChanged(() => this.LuxWorkMode);

                    if (value != 1) IsReportTimeEnable = true;
                    else IsReportTimeEnable = false;
                }

            }
        }

        #region CmdSetMode

        private void ExCmdZcDataTwo()
        {
            //int mode = 0;
            //if (LuxWorkMode  == 1) mode = 0;
            //else if (LuxWorkMode == 2) mode = 1;
            //else
            //{
            //    if (this.IsMainEquipment) mode = 2;
            //    else mode = 3;
            //}
            _dateTimes[2] = DateTime.Now;
            this.SndSetMode(this.RtuId, LuxWorkMode);
        }

        private bool CanCmdZcDataTwo()
        {
            //if (RtuId > 1000 && this.SetIntMode ==3 && this .RtuCommucationType!=2)
            if (RtuId > 1000 &&  this.RtuCommucationType != 2)
                return DateTime.Now.Ticks-_dateTimes[2].Ticks>30000000;
            return false;
        }

        private ICommand _CmdZcDataTwo;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSetMode
        {
            get
            {
                return _CmdZcDataTwo ??
                       (_CmdZcDataTwo = new RelayCommand(ExCmdZcDataTwo, CanCmdZcDataTwo, true));
            }
        }

        #endregion

        #endregion

        #region setReportTime

        private int _ReportTime;

        /// <summary>
        /// 
        /// </summary>

        public int ReportTime
        {
            get { return _ReportTime; }
            set
            {
                if (value != _ReportTime)
                {
                    _ReportTime = value;
                    this.RaisePropertyChanged(() => this.ReportTime);

                }

            }
        }



        private bool _IsReportTimeEnable;

        /// <summary>
        /// 
        /// </summary>

        public bool IsReportTimeEnable
        {
            get { return _IsReportTimeEnable; }
            set
            {
                if (value != _IsReportTimeEnable)
                {
                    _IsReportTimeEnable = value;
                    this.RaisePropertyChanged(() => this.IsReportTimeEnable);

                }

            }
        }

        #region CmdSndReportTime

        private void ExCmdCloseEsu()
        {
            _dateTimes[3] = DateTime.Now;
            this.SndReportTime(this.RtuId, ReportTime);
        }

        private bool CanCmdCloseEsu()
        {
            //if (RtuId > 1000 && SetIntMode == 3 && this.RtuCommucationType != 2)
            if (RtuId > 1000 && this.LuxWorkMode  != 1 && this.RtuCommucationType != 2)
            {
                if (this.IsMainEquipment && ReportTime > 4) return true;
                if (this.IsAttachEquipment && ReportTime > 29)
                    return DateTime.Now.Ticks-_dateTimes[3].Ticks>30000000;
            }
            return false;
        }

        private ICommand _CmdCloseEsu;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdSndReportTime
        {
            get { return _CmdCloseEsu ?? (_CmdCloseEsu = new RelayCommand(ExCmdCloseEsu, CanCmdCloseEsu, true)); }
        }

        #endregion

        #endregion

        #region 分页

        #region PageTotal

        private string _pageTotal;
        public string PageTotal
        {
            get { return _pageTotal; }
            set
            {
                if (value == _pageTotal) return;
                _pageTotal = value;
                RaisePropertyChanged(() => PageTotal);
            }
        }
        #endregion

        #region   PageIndex

        private int _pageIndex;

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (value != _pageIndex)
                {
                    _pageIndex = value;
                    this.RaisePropertyChanged(() => this.PageIndex);
                    RequestHttpData(this.RtuId, DtStartTime, DtEndTime, value, 1);

                }
            }
        }
        #endregion

        #region   ItemCount
        private int _itemCount;

        /// <summary>
        /// 数据总数
        /// </summary>
        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                if (value != _itemCount)
                {
                    _itemCount = value;
                    this.RaisePropertyChanged(() => this.ItemCount);
                }
            }
        }
        #endregion

        #region   PageSize
        private int _pageSize;

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value != _pageSize)
                {
                    _pageSize = value;
                    this.RaisePropertyChanged(() => this.PageSize);
                }
            }
        }
        #endregion

        #region PagerVisi

        private Visibility _pagerVisi = Visibility.Visible;
        public Visibility PagerVisi
        {
            get { return _pagerVisi; }
            set
            {
                if (value == _pagerVisi) return;
                _pagerVisi = value;
                RaisePropertyChanged(() => PagerVisi);
            }
        }
        #endregion

        #endregion

        #region CmdZcMode

        private void ExCmdManuAdjustVol()
        {
            _dateTimes[4] = DateTime.Now;
            this.SndZcMode(this.RtuId);
        }

        private bool CanCmdManuAdjustVol()
        {
            if (RtuId > 1000 && this.RtuCommucationType != 2)
                return DateTime.Now.Ticks-_dateTimes[4].Ticks>30000000;
            return false;
        }

        private ICommand _CmdManuAdjustVol;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcMode
        {
            get
            {
                return _CmdManuAdjustVol ??
                       (_CmdManuAdjustVol = new RelayCommand(ExCmdManuAdjustVol, CanCmdManuAdjustVol, true));
            }
        }

        #endregion

        #region CmdZcReportTime

        private void ExUpdateEsuTime()
        {
            _dateTimes[5] = DateTime.Now;
            this.SndZcReportTime(this.RtuId);
        }

        private bool CanUpdateEsuTime()
        {
            if (RtuId > 1000 && this.RtuCommucationType != 2)
                return DateTime.Now.Ticks-_dateTimes[5].Ticks>30000000;
            return false;
        }

        private ICommand _UpdateEsuTime;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcReportTime
        {
            get { return _UpdateEsuTime ?? (_UpdateEsuTime = new RelayCommand(ExUpdateEsuTime, CanUpdateEsuTime, true)); }
        }

        #endregion

        #region CmdZcVersion

        private void ExCmdZcVersion()
        {
            _dateTimes[6] = DateTime.Now;
            this.SndZcVersion(this.RtuId);
        }

        private bool CanCmdZcVersion()
        {
            if (RtuId > 1000 && this.RtuCommucationType != 2)
                return DateTime.Now.Ticks-_dateTimes[6].Ticks>30000000;
            return false;
        }

        private ICommand _CmdZcVersion;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcVersion
        {
            get { return _CmdZcVersion ?? (_CmdZcVersion = new RelayCommand(ExCmdZcVersion, CanCmdZcVersion, true)); }
        }

        #endregion


        #region CmdZcData

        private void ExCmdZcData()
        {
            _dateTimes[7] = DateTime.Now;
            this.SndZcData(this.RtuId);
        }

        private bool CanCmdZcData()
        {
            if (RtuId > 1000 && this.RtuCommucationType != 2)
                return DateTime.Now.Ticks - _dateTimes[7].Ticks > 30000000;
            return false;
        }

        private ICommand _CmdZcData;

        /// <summary>
        ///   
        /// </summary>
        public ICommand CmdZcData
        {
            get { return _CmdZcData ?? (_CmdZcData = new RelayCommand(ExCmdZcData, CanCmdZcData, true)); }
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
            //    Wlst.Sr.ProtocolPhone .LxAls .wst_request_als_data ,// .wlst_svr_ans_cnt_request_lux_data ,//.ClientPart.wlst_Wj1080_server_ans_clinet_request_LuxData,
            //    LuxDataRequsetOrReply,
            //    typeof(TmlInformationViewModel), this,true);

            ProtocolServer.RegistProtocol(
              Wlst.Sr.ProtocolPhone.LxAls .wst_svr_ans_lux_orders ,//.wlst_svr_ans_cnt_lux_zc_soft_version,//.ClientPart.wlst_Wj1080_server_ans_clinet_order_ZcSoftVersion ,
              OnLuxOrderBack,
              typeof(TmlInformationViewModel), this,true );

        }
        public void OnLuxOrderBack(string session, Wlst.mobile.MsgWithMobile obj)
        {
            var info = obj.WstAlsSvrAnsOrderZcOrSet;
            if (info == null) return;
            int rtuId = info.SuggestedLuxId;

            if (this.ShowInfo.Length > 5000) this.ShowInfo = "";
            if (info.Op == 1)
            {
                UpdateLuxData(rtuId, info.LuxValue);

                //LuxLowTurnOn(rtuId, info.LuxValue);

                if (rtuId != this.RtuId) return;
                string strsucc = "";
               // if (this.ShowInfo.Length > 5000) this.ShowInfo = "";
                this.ShowInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + info.LuxValue + Environment.NewLine + this.ShowInfo;
            }
            if (info.Op == 15)
            {
                if (rtuId != this.RtuId) return;
                string strsucc = "";
               // if (this.ShowInfo.Length > 5000) this.ShowInfo = "";

                strsucc = info.IsSuccesfull ? "成功" : "失败";
                this.ShowInfo = DateTime.Now + " 设置模式" + strsucc + Environment.NewLine + this.ShowInfo;
            }
            if (info.Op == 13)
            {
                if (rtuId != this.RtuId) return;
                string strsucc = "";
               // if (this.ShowInfo.Length > 5000) this.ShowInfo = "";

                strsucc = info.IsSuccesfull ? "成功" : "失败";
                this.ShowInfo = DateTime.Now + " 设置主报时间" + strsucc + Environment.NewLine + this.ShowInfo;
            }
            if (info.Op == 2)
            {
                if (rtuId != this.RtuId) return;
                string strsucc = "";
                if (this.ShowInfo.Length > 5000) this.ShowInfo = "";

                strsucc = info.Mod == 0 ? "每隔5秒主动上报" : info.Mod == 1 ? "选测应答" : "根据设定时间主动上报，默认30秒";
                this.ShowInfo = DateTime.Now + " 召测模式成功:" + strsucc + Environment.NewLine + this.ShowInfo;
                this.RunMode = strsucc;
            }
            if (info.Op == 3)
            {
                if (rtuId != this.RtuId) return;
                string strsucc = "";
               // if (this.ShowInfo.Length > 5000) this.ShowInfo = "";

                this.ShowInfo = DateTime.Now + " 召测主报时间成功，主报时间为[秒]:" + info.Time  + Environment.NewLine +
                                          this.ShowInfo;
                this.RunReportTime = info.Time  + "[秒]";
            } if (info.Op == 4)
            {
                if (rtuId != this.RtuId) return;
                string strsucc = "";
              //  if (this.ShowInfo.Length > 5000) this.ShowInfo = "";


                this.ShowInfo = DateTime.Now + " 召测软件版本成功，软件版本为:" + info.Ver  + Environment.NewLine +
                                           this.ShowInfo;
                this.RunVersion = info.Ver ;
            }

        }

     

        public void LuxDataRequsetOrReply(Wlst.mobile.MsgWithMobile info,int pagingFlag)
        {
            var infos = info.WstAlsDatas;
            //var infos = args.GetParams()[1] as Model.Exchange.ReplyWj1080Data;
            if (infos == null) return;
            if (infos.LuxId != this.RtuId) return;
            if (pagingFlag == 0)
            {
                PageSize = info.Head.PagingNum;
                ItemCount = info.Head.PagingRecordTotal;
                var count = ItemCount / PageSize + (ItemCount % PageSize > 0 ? 1 : 0);
                PagerVisi = count < 2 ? Visibility.Collapsed : Visibility.Visible;
                PageTotal = "页     " + ItemCount + " 条";
            }
            //   this.Items.Clear();
            var tmps = new ObservableCollection<LuxRecoredViewModel>();
            int index = 1 + PageIndex*PageSize;
            var lst = (from t in infos.Info orderby t.DateCreate select t).ToList();
            foreach (var t in lst)//infos.Info)
            {
                tmps.Add(new LuxRecoredViewModel(t){Id =index });
                index = index + 1;
            }
            this.Items = tmps;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" + Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById( RtuId) .RtuPhyId + "--光控数据查询成功，共计" + Items.Count + " 条数据.";
        }
       

        #region 光控数据处理

        internal class LuxDataInfo
        {
            public int LuxId;
            public string LuxName;
            public double LuxData;

            /// <summary>
            /// 0 保留；2  232；3 485 ；6 Socket
            /// </summary>
            public int Commtype;

            public long NameIdGetTime;
            public long DataGetTime;
        }

        private Dictionary<int, LuxDataInfo> info = new Dictionary<int, LuxDataInfo>();


        private int reginCount = 0;
        private void UpdateLuxData(int luxid, double data)
        {
            UpdateLuxDataOne(luxid, data);
            UpdateCoreLuxData();
            if (reginCount > 0 && reginCount < 4)
            {
                Wj1080Module.Services.Common.IsLuxOnTabShowd = true;
                Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                    Wj1080Module.Services.ViewIdAssign.LuxOnTabViewId, true);
                reginCount++;
            }
            if (info.Count > 1 && Wj1080Module.Services.Common.IsLuxOnTabShowd == false)
            {
                Wj1080Module.Services.Common.IsLuxOnTabShowd = true;
                Wlst.Cr.Core.CoreServices.RegionManage.ShowViewByIdAttachRegion(
                    Wj1080Module.Services.ViewIdAssign.LuxOnTabViewId, true);
                reginCount++;
            }
          

            //if (info.ContainsKey(luxid))
            //{
            //    Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
            //        luxid, info[luxid].LuxName, OperatrType.SystemInfo, "光控:" + data);
            //}
            //else
            //{
            //    var luxequip =
            //        Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(luxid);
            //    if (luxequip == null)
            //    {
            //        Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
            //            luxid, "Reserve", OperatrType.SystemInfo, "光控:" + data);
            //    }
            //    else
            //    {
            //        Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
            //            luxid, luxequip.RtuName, OperatrType.SystemInfo, "光控:" + data);
            //    }
            //}
        }

        private void UpdateLuxDataOne(int luxid, double data)
        {
            if (!info.ContainsKey(luxid))
            {
                var luxequip =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(luxid);//.GetEquipmentInfo( luxid);


                if (luxequip == null)
                {
                    info.Add(luxid,
                             new LuxDataInfo()
                                 {
                                     Commtype = 0,
                                     LuxData = data,
                                     LuxId = luxid,
                                     LuxName = "Reserve",
                                     NameIdGetTime = DateTime.Now.Ticks,
                                     DataGetTime = DateTime.Now.Ticks

                                 });
                }
                else
                {
                    var iiinfo = luxequip as Sr.EquipmentInfoHolding.Model.Wj1080Lux;
                    if (iiinfo == null || iiinfo.WjLux==null)
                    {
                        info.Add(luxid,
                                 new LuxDataInfo()
                                     {
                                         Commtype = 0,
                                         LuxData = data,
                                         LuxId = luxid,
                                         LuxName = "Reserve",
                                         NameIdGetTime = DateTime.Now.Ticks,
                                         DataGetTime = DateTime.Now.Ticks
                                     });
                    }
                    else
                    {
                        info.Add(luxid,
                                 new LuxDataInfo()
                                     {
                                         Commtype = iiinfo.WjLux.LuxCommTypeCode,
                                         LuxData = data,
                                         LuxId = luxid,
                                         LuxName = iiinfo.RtuName,
                                         NameIdGetTime = DateTime.Now.Ticks,
                                         DataGetTime = DateTime.Now.Ticks
                                     });
                    }
                }

            }
            else
            {
                if (info[luxid].LuxName.Contains("Reserve"))
                {
                    var luxequip =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(luxid);//.GetEquipmentInfo(luxid);

                    if (luxequip != null)
                    {
                        info[luxid].LuxName = luxequip.RtuName;
                    }
                }
                info[luxid].LuxData = data;
                info[luxid].DataGetTime = DateTime.Now.Ticks;
                if (info[luxid].NameIdGetTime + 600000000 < DateTime.Now.Ticks)
                {
                    var luxequip =
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(luxid );//.GetEquipmentInfo(luxid);

                    if (luxequip == null) return;

                    var iiinfo = luxequip as Sr.EquipmentInfoHolding.Model.Wj1080Lux;
                    if (iiinfo == null) return;
                    info[luxid].Commtype = iiinfo.WjLux.LuxCommTypeCode;
                    info[luxid].LuxName = iiinfo.RtuName;
                    info[luxid].NameIdGetTime = DateTime.Now.Ticks;
                    info[luxid].LuxData = data;
                }

            }

           


        }

        private void UpdateCoreLuxData()
        {
            string mainInfo = null;
            string tooltipsinfo = "" + Environment.NewLine;
            foreach (var t in info)
            {
                if (t.Value.Commtype == 2)
                {
                    mainInfo = t.Value.LuxName + ":" + t.Value.LuxData.ToString("f2");
                    break;
                }
            }
            if (mainInfo == null)
            {
                foreach (var t in info)
                {
                    if (t.Value.Commtype == 6)
                    {
                        mainInfo = t.Value.LuxName + ":" + t.Value.LuxData.ToString("f2");
                        break;
                    }
                }
            }
            if (mainInfo == null)
            {
                foreach (var t in info)
                {
                    mainInfo =t.Value .LuxName  +":" +t.Value.LuxData.ToString("f2");
                    break;
                }
            }

            foreach (var t in info)
            {
                if (t.Value == null) continue;
                var idlux = 0;
                var luxTmlName = "";
                var luxequip =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.Value.LuxId);
                if (luxequip != null)
                {
                    idlux = luxequip.RtuPhyId;

                    if (luxequip.RtuFid != 0)
                    {

                        var fiinfo =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(luxequip.RtuFid);
                        if (fiinfo != null)
                        {
                            idlux = fiinfo.RtuPhyId;
                            luxTmlName = fiinfo.RtuName;
                        }
                    }

                }
                if (luxTmlName == "")
                {
                    tooltipsinfo +=
                        "采集时间: " + (new DateTime(t.Value.DataGetTime)).ToString("yyyy-MM-dd HH:mm:ss") +
                        Environment.NewLine +
                        "物理地址: " + idlux.ToString("d4") + Environment.NewLine +
                        "光控名称: " + t.Value.LuxName + Environment.NewLine +
                        "      光照度: ------ " + t.Value.LuxData.ToString("f2") + " ------" + Environment.NewLine +
                        Environment.NewLine;
                }else
                {
                    tooltipsinfo +=
                        " 采集时间:  " + (new DateTime(t.Value.DataGetTime)).ToString("yyyy-MM-dd HH:mm:ss") +
                        Environment.NewLine +
                        "    主设备:  " + idlux.ToString("d4") + " - " + luxTmlName+ Environment.NewLine +
                        " 光控名称:  " + t.Value.LuxName + Environment.NewLine +
                        "    光照度:  ------ " + t.Value.LuxData.ToString("f2") + " ------" + Environment.NewLine +
                        Environment.NewLine;
                }
            }
            TopDataInfoServers.MySelf.UpdateDataInfo(mainInfo, tooltipsinfo, 1);
        }




        #endregion


        #region 一键开关灯


        private const long OneSecond = 10000000;
        private ConcurrentDictionary<int, long> _openLightTimes = new ConcurrentDictionary<int, long>();
        private ConcurrentDictionary<int, long> _openLightTimesx = new ConcurrentDictionary<int, long>();


        private static bool  isLastMsgboxNotPress = false  ;
        private static DateTime dtlastMsgBoxTime = DateTime.Now;


        //private void LuxLowTurnOn(int luxid, double data)
        //{
        //    return;
        //    if (!wj1080TreeSetLoad.Myself.IsLuxLowTurnOn) return;
        //    if (data > wj1080TreeSetLoad.Myself.AlarmValue)
        //    {
        //        if (_openLightTimesx.ContainsKey(luxid) && data > wj1080TreeSetLoad.Myself.AlarmValue + 10)
        //            _openLightTimesx[luxid] = 0;
        //        return;
        //    }

        //    var rtus = DayLuxLowTime(luxid);
        //    if (rtus.Count == 0) return;

        //    if (_openLightTimes.ContainsKey(luxid)) //光控提示时间 6分钟内不在提示操作 
        //        if (DateTime.Now.Ticks - _openLightTimes[luxid] < 360*OneSecond) return;
        //    if (_openLightTimesx.ContainsKey(luxid)) //光控执行时间 如果4小时内已经执行过了 不再提示
        //        if (DateTime.Now.Ticks - _openLightTimesx[luxid] < 240*60*OneSecond) return;

        //    if (_openLightTimes.ContainsKey(luxid)) _openLightTimes[luxid] = DateTime.Now.Ticks;
        //    else _openLightTimes.TryAdd(luxid, DateTime.Now.Ticks);

        //    var luxname = info.ContainsKey(luxid) ? info[luxid].LuxName : "光控" + luxid;
        //    VoiceReportFun(luxid);

        //    if (isLastMsgboxNotPress) return;

        //    isLastMsgboxNotPress = true;
        //    dtlastMsgBoxTime = DateTime.Now;
        //    WlstMessageBoxResults nr = WlstMessageBox.Show("仅操作该光控控制的终端，若不操作六分钟内将不再提示开灯",
        //                                                   luxname + " 光照低于" + wj1080TreeSetLoad.Myself.AlarmValue +
        //                                                   "，是否进行一键开灯操作？",
        //                                                   WlstMessageBoxType.YesNo);
        //    isLastMsgboxNotPress = false;
        //    if (nr != WlstMessageBoxResults.Yes)
        //    {
        //        return;
        //    }
        //    while(true )
        //    {
        //        var sss = UMessageBoxWantSomefromUser.Show("输入验证码", "若确认执行一键开灯操作，请输入验证码:1234", "");
        //        if (sss == UMessageBoxWantSomefromUser.CancelReturn)
        //        {
        //            return;
        //        }
        //        if (sss != "1234")
        //        {
        //            UMessageBox.Show("验证失败", "您输入的验证码与默认值不匹配，请检查......", UMessageBoxButton.Yes);
        //            continue ;
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        
                
            
           


        //    if (_openLightTimesx.ContainsKey(luxid)) _openLightTimesx[luxid] = DateTime.Now.Ticks;
        //    else _openLightTimesx.TryAdd(luxid, DateTime.Now.Ticks);

        //    OpenLight(DayLuxLowTime(luxid));
           
        //    if (info.ContainsKey(luxid))
        //    {
        //        Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
        //            luxid, info[luxid].LuxName, OperatrType.SystemInfo, "白天光照度低，光照度值:" + data);
        //    }


        //}

        /// <summary>
        /// 获取需要开关灯的 终端地址-回路
        /// </summary>
        /// <param name="luxid"></param>
        /// <returns></returns>
        private List<Tuple<int, int>> DayLuxLowTime(int luxid)
        {
            var lst = new List<Tuple<int, int>>();
            var allTimeTable = WeekTimeTableInfoService.WeekTimeTableInfoDictionary.Values;
            foreach (var t in allTimeTable)
            {
                if (t.LuxId != luxid) continue;

                foreach (var g in t.RuleItems )
                {
                    if (g.DayOfWeekUsed .Contains( (int )DateTime .Now .DayOfWeek ) )
                    {
                        var timeNow = DateTime.Now.Hour*60 + DateTime.Now.Minute;

                        if (g.TypeOn >2) break;// return new List<Tuple<int, int>>();
                        var sunset =
                            Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(
                                DateTime.Now.Month, DateTime.Now.Day);
                        if (sunset == null) break;
                        var timeon = sunset.time_sunset + t.LightOnOffset;
                        var timeoff = g.TypeOff < 3 ? sunset.time_sunrise + t.LightOffOffset : g.TimeOff;

                        if (timeoff  <= timeon )
                        {
                            if (timeoff  >= timeNow) break; //return new List<Tuple<int, int>>();
                            if (timeNow > timeon - t.LuxEffective) break; //return new List<Tuple<int, int>>();
                        }
                        else
                        {
                            if (720 >= timeNow) break; //return new List<Tuple<int, int>>();
                            if (timeNow > timeon  - t.LuxEffective) break;
                            // return new List<Tuple<int, int>>();
                        }

                        var areaid =
                            Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(luxid);//20160823 增加areaid 待测 todo
                        foreach (
                            var f in
                                Wlst.Sr.TimeTableSystem.Services.RtuOrGprBandingTimeTableInfoService.
                                    GetBangdingToThisTimeTablesTmls(areaid,t.TimeId))
                        {
                            var rtuOrGrpId = f.Item2 ;
                            if (rtuOrGrpId > 1000000 && rtuOrGrpId < 1100000)
                            {
                                lst.Add(f);
                            }
                            else
                            {
                                var flg =
                                    Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
                                        f.Item1, rtuOrGrpId);
                                if (flg != null)
                                {
                                    foreach (var ss in flg.LstTml) //.r..g..GetGrpTmlList(rtuOrGrpId))
                                    {
                                        lst.Add(new Tuple<int, int>(ss, f.Item2));
                                    }
                                }
                                
                            }

                        }

                    }
                }
            }
            return lst;
        }

        private void OpenLight(List<Tuple<int, int>> rtuList)
        {


            var infox = Wlst.Sr.ProtocolPhone.LxRtu .wst_cnt_order_rtu_open_close_center ;//.wlst_cnt_wj3090_order_open_close_light_center;
            infox.WstRtuCntOrderOpenCloseCenter  = new OpenCloseOperatorCenter()
                                                        {
                                                            //K1Rtus =
                                                            //    (from t in rtuList where t.Item2 == 1 select t.Item1).
                                                            //    ToList(),
                                                            //K2Rtus =
                                                            //    (from t in rtuList where t.Item2 == 2 select t.Item1).
                                                            //    ToList(),
                                                            //K3Rtus =
                                                            //    (from t in rtuList where t.Item2 == 3 select t.Item1).
                                                            //    ToList(),
                                                            //K4Rtus =
                                                            //    (from t in rtuList where t.Item2 == 4 select t.Item1).
                                                            //    ToList(),
                                                            //K5Rtus =
                                                            //    (from t in rtuList where t.Item2 == 5 select t.Item1).
                                                            //    ToList(),
                                                            //K6Rtus =
                                                            //    (from t in rtuList where t.Item2 == 6 select t.Item1).
                                                            //    ToList(),
                                                            Open = 1
                                                        };
          var   K1Rtus =
                (from t in rtuList where t.Item2 == 1 select t.Item1).
                    ToList();
          var   K2Rtus =
                (from t in rtuList where t.Item2 == 2 select t.Item1).
                    ToList();
          var   K3Rtus =
                (from t in rtuList where t.Item2 == 3 select t.Item1).
                    ToList();
          var   K4Rtus =
                (from t in rtuList where t.Item2 == 4 select t.Item1).
                    ToList();
          var   K5Rtus =
                (from t in rtuList where t.Item2 == 5 select t.Item1).
                    ToList();
          var   K6Rtus =
                (from t in rtuList where t.Item2 == 6 select t.Item1).
                    ToList();
          if (K1Rtus.Count > 0)
              infox.WstRtuCntOrderOpenCloseCenter.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem()
                                                                {LoopId = 1, Rtus = K1Rtus});

          if (K2Rtus.Count > 0)
              infox.WstRtuCntOrderOpenCloseCenter.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 2, Rtus = K2Rtus });

          if (K3Rtus.Count > 0)
              infox.WstRtuCntOrderOpenCloseCenter.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 3, Rtus = K3Rtus });

          if (K4Rtus.Count > 0)
              infox.WstRtuCntOrderOpenCloseCenter.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 4, Rtus = K4Rtus });

          if (K5Rtus.Count > 0)
              infox.WstRtuCntOrderOpenCloseCenter.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 5, Rtus = K5Rtus });

          if (K6Rtus.Count > 0)
              infox.WstRtuCntOrderOpenCloseCenter.Items.Add(new OpenCloseOperatorCenter.OpenCloseOperatorCenterItem() { LoopId = 6, Rtus = K6Rtus });
            SndOrderServer.OrderSnd(infox, 10, 2);
        }

        private void VoiceReportFun(int luxid)
        {
            try
            {
                SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFDefault; //.SVSFlagsAsync;

                var voice = new SpVoice();

                string speaktext;

                try
                {
                    if (info.ContainsKey(luxid))
                    {
                        speaktext = info[luxid].LuxName + "白天光照度低";
                        voice.Speak(speaktext, SpFlags);
                    }
                    else
                    {
                        speaktext ="白天光照度低";
                        voice.Speak(speaktext, SpFlags);
                    }
                }
                catch (Exception ex)
                {
                    Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("语音报警出错:" + ex);
                }
                Thread.Sleep(1000);
            }
            catch (Exception)
            {

            }
        }

        #endregion
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class TmlInformationViewModel
    {
        private void SndQueryLuxData(int rtuId, DateTime dtstart, DateTime dtend)
        {
            var tStartTime = new DateTime(dtstart.Year, dtstart.Month, dtstart.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtend.Year, dtend.Month, dtend.Day, 23, 59, 59);
            var info = Wlst.Sr.ProtocolPhone.LxAls.wst_request_als_data;
                // .wlst_cnt_request_lux_data ;//.ServerPart.wlst_Wj1080_clinet_request_LuxData  ;
            info.WstAlsDatas.LuxId = RtuId;
            info.WstAlsDatas.DtStartTime = tStartTime.Ticks;
            info.WstAlsDatas.DtEndTime = tEndTime.Ticks;

            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
        }

        private void RequestHttpData(int rtuId, DateTime dtstart, DateTime dtend, int pageIndex, int pagingFlag)
        {
            var tStartTime = new DateTime(dtstart.Year, dtstart.Month, dtstart.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtend.Year, dtend.Month, dtend.Day, 23, 59, 59);
            var info = Wlst.Sr.ProtocolPhone.LxAlsHttp.wst_als_data_record_http;
            // .wlst_cnt_request_lux_data ;//.ServerPart.wlst_Wj1080_clinet_request_LuxData  ;
            info.WstAlsDatas.LuxId = RtuId;
            info.WstAlsDatas.DtStartTime = tStartTime.Ticks;
            info.WstAlsDatas.DtEndTime = tEndTime.Ticks;

            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (data == null) return;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
        }

        private void SndSetMode(int rtuId, int mode)
        {

            var info = Wlst.Sr.ProtocolPhone .LxAls  .wst_lux_orders  ;//.ServerPart.wlst_Wj1080_clinet_order_SetMulitLuxMode;
            info.WstAlsOrderZcOrSet  .LuxId  =rtuId ;
            info.WstAlsOrderZcOrSet.Op  = 15;
            info.WstAlsOrderZcOrSet.Args  = mode;
            info.WstAlsOrderZcOrSet.LuxAddr = PhyId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SndReportTime(int rtuid, int time)
        {
            var info = Wlst.Sr.ProtocolPhone.LxAls.wst_lux_orders;//.ServerPart.wlst_Wj1080_clinet_order_SetMulitLuxMode;
            info.WstAlsOrderZcOrSet.LuxId = rtuid ;
            info.WstAlsOrderZcOrSet.Op = 13;
            info.WstAlsOrderZcOrSet.Args = time ;
            info.WstAlsOrderZcOrSet.LuxAddr = PhyId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SndZcMode(int rtuid)
        {
            var info = Wlst.Sr.ProtocolPhone.LxAls.wst_lux_orders;//.ServerPart.wlst_Wj1080_clinet_order_SetMulitLuxMode;
            info.WstAlsOrderZcOrSet.LuxId = rtuid;
            info.WstAlsOrderZcOrSet.Op = 2;
            info.WstAlsOrderZcOrSet.Args = 0;
            info.WstAlsOrderZcOrSet.LuxAddr = PhyId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SndZcReportTime(int rtuid)
        {
            var info = Wlst.Sr.ProtocolPhone.LxAls.wst_lux_orders;//.ServerPart.wlst_Wj1080_clinet_order_SetMulitLuxMode;
            info.WstAlsOrderZcOrSet.LuxId = rtuid;
            info.WstAlsOrderZcOrSet.Op = 3;
            info.WstAlsOrderZcOrSet.Args = 0;
            info.WstAlsOrderZcOrSet.LuxAddr = PhyId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SndZcVersion(int rtuid)
        {
            var info = Wlst.Sr.ProtocolPhone.LxAls.wst_lux_orders;//.ServerPart.wlst_Wj1080_clinet_order_SetMulitLuxMode;
            info.WstAlsOrderZcOrSet.LuxId = rtuid;
            info.WstAlsOrderZcOrSet.Op = 4;
            info.WstAlsOrderZcOrSet.Args = 0;
            info.WstAlsOrderZcOrSet.LuxAddr = PhyId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void SndZcData(int rtuid)
        {
            var info = Wlst.Sr.ProtocolPhone.LxAls.wst_lux_orders;//.ServerPart.wlst_Wj1080_clinet_order_SetMulitLuxMode;
            info.WstAlsOrderZcOrSet.LuxId = rtuid;
            info.WstAlsOrderZcOrSet.Op = 1;
            info.WstAlsOrderZcOrSet.Args = 0;
            info.WstAlsOrderZcOrSet.LuxAddr = PhyId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
    }
}
