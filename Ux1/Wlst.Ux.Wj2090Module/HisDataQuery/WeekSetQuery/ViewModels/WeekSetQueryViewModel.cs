using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.Wj2090Module.HisDataQuery.WeekSetQuery.Services;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.WeekSetQuery.ViewModels
{


    [Export(typeof (IIWeekSetQuery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class WeekSetQueryViewModel :
        Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, IIWeekSetQuery
    {

        public WeekSetQueryViewModel()
        {
            this.InitAciton();
            this.InitEvent();
        }

        private bool _thisViewActive = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            DtEndTime = DateTime.Now;
            DtStartTime = DateTime.Now.AddDays(-1);
            _thisViewActive = true;
            _dtCmdExport = DateTime.Now.AddDays(-1);
        }

        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            this.Items.Clear();
            ItemCount = 0;
            PageTotal = "";
            Remind = "";
        }

        #region IITab
        public int Index
        {
            get { return 1; }
        }
        public string Title
        {
            get { return "单灯时间发送记录查询"; }
            //get { return "记录查询"; }
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
    }

    /// <summary>
    /// 属性
    /// </summary>
    public partial class WeekSetQueryViewModel
    {
        private ObservableCollection<WeekSetItemVm> _isPowerOnLight;

        public ObservableCollection<WeekSetItemVm> Items
        {
            get
            {
                if (_isPowerOnLight == null)
                {
                    _isPowerOnLight = new ObservableCollection<WeekSetItemVm>();

                }
                return _isPowerOnLight;
            }
            set
            {
                if (_isPowerOnLight == value) return;
                _isPowerOnLight = value;
                RaisePropertyChanged(() => Items);
            }
        }

        private string _indsdfsdfdf;

        public string SluName
        {
            get { return _indsdfsdfdf; }
            set
            {
                if (_indsdfsdfdf == value) return;
                _indsdfsdfdf = value;
                RaisePropertyChanged(() => SluName);
            }
        }

        private int _indexsdf;

        public int SluId
        {
            get { return _indexsdf; }
            set
            {
                if (_indexsdf == value) return;
                _indexsdf = value;
                RaisePropertyChanged(() => SluId);



                var infos = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                if (infos != null)
                {
                    SluName = infos.RtuName;
                    if (infos.RtuFid == 0)
                    {
                        SluShowId = infos.RtuPhyId.ToString("D4");
                    }
                    else
                    {
                        var ntg =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                infos.RtuFid);
                        if (ntg != null)
                        {
                            SluShowId = ntg.RtuPhyId.ToString("D4");
                        }
                        else SluShowId = value.ToString("D7");
                    }
                }
                else if (
                    Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(SluId) != null)
                {
                    var tml =
                        Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(SluId);
                    SluName = tml.FieldName;
                    SluShowId = tml.PhyId.ToString("D4");
                    //if (tml.PhyId == 0)
                    //    SluPhyId = tml.PhyId.ToString();
                    //else SluPhyId = value.ToString();
                }
                else
                {
                    SluShowId = value.ToString("D7");
                    SluName = "--";
                }
            }
        }
        private string _ssdfSluId;

        public string SluShowId
        {
            get { return _ssdfSluId; }
            set
            {
                if (value != _ssdfSluId)
                {
                    _ssdfSluId = value;
                    this.RaisePropertyChanged(() => this.SluShowId);
                }
            }
        }




        private bool _isRtuSelected;

        public bool IsRtuSelected
        {
            get { return _isRtuSelected; }
            set
            {
                if (value != _isRtuSelected)
                {
                    _isRtuSelected = value;
                    this.RaisePropertyChanged(() => this.IsRtuSelected);
                }
            }
        }

        private int _indexsdsdfsdsdf;

        public int CtrlId
        {
            get { return _indexsdsdfsdsdf; }
            set
            {
                if (_indexsdsdfsdsdf == value) return;
                _indexsdsdfsdsdf = value;
                RaisePropertyChanged(() => CtrlId);

                CtrlIdShow = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(SluId, value);
            }
        }


        private int _indexsdsdf;

        public int  CtrlIdShow
        {
            get { return _indexsdsdf; }
            set
            {
                if (_indexsdsdf == value) return;
                _indexsdsdf = value;
                RaisePropertyChanged(() => CtrlIdShow);

              
            }
        }

        

        #region DtEndTime

        private DateTime _dtEndTime;

        public DateTime DtEndTime
        {
            get { return _dtEndTime; }
            set
            {
                if (_dtEndTime != value)
                {
                    _dtEndTime = value;
                    this.RaisePropertyChanged(() => this.DtEndTime);
                }
            }
        }

        #endregion

        #region DtStartTime

        private DateTime _dtStartTime;

        public DateTime DtStartTime
        {
            get { return _dtStartTime; }
            set
            {
                if (_dtStartTime != value)
                {
                    _dtStartTime = value;
                    this.RaisePropertyChanged(() => this.DtStartTime);
                }
            }
        }

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
                    RequestHttpData(value, 1);

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


        #region CmdQuery

        #region CmdExport

        private DateTime _dtCmdExport;
        private ICommand _cmdCmdExport;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, true);
                return _cmdCmdExport;
            }
        }

        private void ExCmdExport()
        {
            _dtCmdExport = DateTime.Now;
            //Query();
            PageIndex = 0;
            RequestHttpData(PageIndex, 0);
        }


        private bool CanExCmdExport()
        {
           // if (SluId < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }

        #endregion

        private bool GetCheckedInformation()
        {
            if (DtStartTime.AddDays(63) < DtEndTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在62天以内", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            return true;
        }

        #endregion
    }


    /// <summary>
    /// Event
    /// </summary>
    public partial class WeekSetQueryViewModel
    {

        #region Aciton

        private void InitAciton()
        {
            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_weekset_record ,// .wlst_svr_ans_cnt_wj2090_request_weekset_record ,//.ClientPart.wlst_Wj2090_svr_ans_clinet_request_slu_weekset, 
            //    OnZcOrSetBack,
            //    typeof (WeekSetQueryViewModel), this);
        }


        private void OnZcOrSetBack(Wlst.mobile.MsgWithMobile info, int pagingFlag)
        {
            if (info == null || info.WstSluWeeksetRecord == null) return;
            if (pagingFlag == 0)
            {
                PageSize = info.Head.PagingNum;
                ItemCount = info.Head.PagingRecordTotal;
                var count = ItemCount / PageSize + (ItemCount % PageSize > 0 ? 1 : 0);
                PagerVisi = count < 2 ? Visibility.Collapsed : Visibility.Visible;
                PageTotal = "页     " + ItemCount + " 条";
            }
            var lst = new ObservableCollection<WeekSetItemVm>();
            int i = 1 + PageIndex*PageSize;
            foreach (var t in info.WstSluWeeksetRecord.Items)
            {
                lst.Add(new WeekSetItemVm(t) {Index = i++});
            }
            this.Items.Clear();
            this.Items = lst;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 共计查阅 " + lst.Count + " 条数据.";
        }

        #endregion


        #region Event

        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
        }



        public override void ExPublishedEvent(
            PublishEventArgs args)
        {

            if (_thisViewActive == false) return;
            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    if (args.GetParams().Count == 1)
                    {
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        if (id > Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart
                            && id < Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd)
                        {
                            SluId = id;
                            CtrlId = 0;
                            Remind = "请点击查询来查阅" + SluShowId + " 时间发送记录...";
                        }
                        else if (
                            Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(id) != null)
                        {
                            SluId = id;
                            CtrlId = 0;
                            Remind = "请点击查询来查阅" + SluShowId + " 时间发送记录...";

                            
                        }
                    }
                    else
                    {
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        int ctrlid = Convert.ToInt32(args.GetParams()[1]);
                        if (id > Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluStart
                            && id < Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.SluEnd)
                        {
                            SluId = id;
                            CtrlId = ctrlid;
                            Remind = "请点击查询来查阅" + SluShowId + "-" + ctrlid + " 时间发送记录...";
                        }
                    }

                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        #endregion

        #region Socket

        private void Query()
        {
            if (!GetCheckedInformation()) return;
            this.Items.Clear();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询：" + SluShowId + " ...";
            var info = Wlst.Sr.ProtocolPhone .LxSlu .wst_slu_weekset_record ;// .wlst_cnt_wj2090_request_weekset_record;//.ServerPart.wlst_Wj2090_clinet_request_slu_weekset;
            info.WstSluWeeksetRecord .CtrlId = 0;
            info.WstSluWeeksetRecord.DtEndTime = new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, 23, 59, 0).Ticks;// DtEndTime.Ticks;
            info.WstSluWeeksetRecord.DtStartTime = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 0).Ticks;// DtStartTime.Ticks;
            info.WstSluWeeksetRecord.SluId = IsRtuSelected? SluId:0;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        //http请求
        private void RequestHttpData(int pageIndex, int pagingFlag)
        {
            if (!GetCheckedInformation()) return;
            this.Items.Clear();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询：" + SluShowId + " ...";
            var info = Wlst.Sr.ProtocolPhone.LxSluHttp.wst_slu_weekset_record_http;// .wlst_cnt_wj2090_request_weekset_record;//.ServerPart.wlst_Wj2090_clinet_request_slu_weekset;
            info.WstSluWeeksetRecord.CtrlId = 0;
            info.WstSluWeeksetRecord.DtEndTime = new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, 23, 59, 0).Ticks;// DtEndTime.Ticks;
            info.WstSluWeeksetRecord.DtStartTime = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 0).Ticks;// DtStartTime.Ticks;
            info.WstSluWeeksetRecord.SluId = IsRtuSelected ? SluId : 0;
            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (data == null) return;
            OnZcOrSetBack(data, pagingFlag);
        }
        #endregion
    }
}
