using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;

using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Wj1050Module.Wj1050DataInqueryModel.Services;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Ux.Wj1050Module.Wj1050ManageSettingViewModel.ViewModel;
using Wlst.Ux.Wj1050Module.Wj1050ManageViewModel.ViewModel;
using Wlst.client;

namespace Wlst.Ux.Wj1050Module.Wj1050DataInqueryModel.Models
{
    [Export(typeof(IIWj1050DataInquery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1050DataInqueryViewModel : Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, IIWj1050DataInquery
    {
        public Wj1050DataInqueryViewModel()
        {
            InitialAction();
            InitEvent();

        }


        private bool _isViewShow = false;

        public void NavOnLoad(params object[] parsObjects)
        {
            BegTime = DateTime.Now.AddDays(-30);
            EndTime = DateTime.Now;
            ArgsInfoVisi = false;
            ArgsInfoVisi1 = true;
            try
            {
                RtuId = (int)parsObjects[0];
                if (RtuId == 0)
                    IsRtuSelected = false;
                else IsRtuSelected = true;
            }
            catch (Exception ex)
            {

            }
            _isViewShow = true;
            Remind = "请设置好查询时间后进行查询...";
            IsSelectIndex = 0;
        }

        public void OnUserHideOrClosing()
        {
            _isViewShow = false;
        }
        #region IITab
        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return string.Format("抄表数据查询"); }
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

        private ObservableCollection<MruDataRecordViewModel> _itemsdata;

        public ObservableCollection<MruDataRecordViewModel> Items
        {
            get { return _itemsdata ?? (_itemsdata = new ObservableCollection<MruDataRecordViewModel>()); }
        }

        #region ArgsInfoVisi

        private bool _argsInfoVisi = false;

        public bool ArgsInfoVisi
        {
            get { return _argsInfoVisi; }
            set
            {
                if (value == _argsInfoVisi) return;
                _argsInfoVisi = value;
                RaisePropertyChanged(() => ArgsInfoVisi);
            }
        }
        private bool _argsInfoVisi1 = false;

        public bool ArgsInfoVisi1
        {
            get { return _argsInfoVisi1; }
            set
            {
                if (value == _argsInfoVisi1) return;
                _argsInfoVisi1 = value;
                RaisePropertyChanged(() => ArgsInfoVisi1);
            }
        }
        private bool _argsInfoVisi2 = false;

        public bool ArgsInfoVisi2
        {
            get { return _argsInfoVisi2; }
            set
            {
                if (value == _argsInfoVisi2) return;
                _argsInfoVisi2 = value;
                RaisePropertyChanged(() => ArgsInfoVisi2);
            }
        }
        #endregion

        #region Isitem
        private int _isSelectIndex;

        public int IsSelectIndex
        {
            get { return _isSelectIndex; }
            set
            {
                if (_isSelectIndex != value)
                {
                    _isSelectIndex = value;
                    this.RaisePropertyChanged(() => this.IsSelectIndex);
                }
            }
        }


        private ObservableCollection<ShieldLoop> _IsItem;

        public ObservableCollection<ShieldLoop> IsItem
        {
            get
            {
                if (_IsItem == null)
                {
                    _IsItem = new ObservableCollection<ShieldLoop>();
                    _IsItem.Add(new ShieldLoop() { Name = "统计", Key = 0 });
                    _IsItem.Add(new ShieldLoop() { Name = "日表", Key = 1 });
                    _IsItem.Add(new ShieldLoop() { Name = "月表", Key = 2 });
                    _IsItem.Add(new ShieldLoop() { Name = "年表", Key = 3 });
                    _IsItem.Add(new ShieldLoop() { Name = "全部", Key = 4 });

                }

                return _IsItem;
            }
        }


        public class ShieldLoop : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private int _key;

            public int Key
            {
                get { return _key; }
                set
                {
                    if (_key != value)
                    {
                        _key = value;
                        this.RaisePropertyChanged(() => this.Key);
                    }
                }
            }

            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    if (value != _name)
                    {
                        _name = value;
                        this.RaisePropertyChanged(() => this.Name);
                    }
                }
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

        #region AttachRtuId

        private int _attachRtuId;
        public int AttachRtuId
        {
            get { return _attachRtuId; }
            set
            {
                if (_attachRtuId.Equals(value)) return;
                _attachRtuId = value;
                RaisePropertyChanged(() => AttachRtuId);
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
                if (_rtuName == value) return;
                _rtuName = value;

                RaisePropertyChanged(() => RtuName);
            }
        }



        private bool _rtIsRtuSelected;
        public bool IsRtuSelected
        {
            get { return _rtIsRtuSelected; }
            set
            {
                if (_rtIsRtuSelected == value) return;
                _rtIsRtuSelected = value;

                RaisePropertyChanged(() => IsRtuSelected);

                if (DateTime.Now.Ticks - lasttime < 10000000)
                {
                    xcount++;

                    if (xcount > 5)
                    {
                        xcount = 0;
                        DaoChuPara();
                    }
                }
                else
                {
                    xcount = 0;
                }
                lasttime = DateTime.Now.Ticks;
            }
        }

        private long lasttime = 0;
        private int xcount;
        void DaoChuPara()
        {
            var writeinfo = new List<List<object>>();
            var titleinfo = new List<object>();
            titleinfo.Add("序号");
            titleinfo.Add("终端地址");
            titleinfo.Add("终端名称");
            titleinfo.Add("电表名称");
            titleinfo.Add("电表标识");
            titleinfo.Add("抄表时间");
            titleinfo.Add("抄表类型");
            titleinfo.Add("抄表值");
            titleinfo.Add("电量");
            titleinfo.Add("差值");
            titleinfo.Add("累计差值");
            titleinfo.Add("备注");
            titleinfo.Add("地址1");
            titleinfo.Add("地址2");
            titleinfo.Add("地址3");
            titleinfo.Add("地址4");
            titleinfo.Add("地址5");
            titleinfo.Add("地址6");

            var listhasdata = new List<int>();
            var tmllst = (from t in Items orderby t.Id select t).ToList();
            foreach (var f in tmllst)
            {


                listhasdata.Add(f.RtuId);

                var tmp = new List<object>();
                tmp.Add(f.Id);
                tmp.Add(f.AttachRtuId);
                tmp.Add(f.AttachRtuName);
                tmp.Add(f.RtuId);
                tmp.Add(f.RtuName);
                tmp.Add(f.DateCreate);
                var type = f.DateTypeCode + "||" + f.MruTypeCode;
                tmp.Add(type);
                tmp.Add(f.MruData);
                tmp.Add(f.MruTotal);
                tmp.Add(f.Differ);
                tmp.Add(f.Count);
                tmp.Add(f.Remarks);

                bool set = false;
                var fidequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.RtuId);
                if (fidequ != null)
                {
                    var ntsdf =
                        fidequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj1050Mru;
                    if (ntsdf != null && ntsdf.WjMru != null)
                    {
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr1, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr2, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr3, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr4, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr5, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr6, 16).Trim());
                        set = true;
                    }
                }
                if (set == false)
                {
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(0);
                }

                writeinfo.Add(tmp);

            }

            int index = 1;
            foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (f.Value.RtuModel == EnumRtuModel.Wj1050)
                {
                    var tmp = new List<object>();
                    var ntsdf =
                        f.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj1050Mru;
                    if (ntsdf != null && ntsdf.WjMru != null)
                    {
                        tmp.Add(index);
                        index++;

                        var nps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.Value.RtuFid);
                        if (nps != null)
                        {

                            tmp.Add(nps.RtuPhyId);
                            tmp.Add(nps.RtuName);
                        }
                        else
                        {
                            tmp.Add("终端地址");
                            tmp.Add("终端名称");
                        }

                        tmp.Add(f.Key);
                        tmp.Add(f.Value.RtuName);
                        tmp.Add(0);
                        tmp.Add(0);
                        tmp.Add(0);
                        tmp.Add(0);
                        tmp.Add(0);
                        tmp.Add(0);
                        tmp.Add("无数据");


                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr1, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr2, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr3, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr4, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr5, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr6, 16).Trim());
                        writeinfo.Add(tmp);
                    }
                }
            }

            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);

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
                int fid = 0;
                var ddd =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                if (ddd != null)
                {
                    RtuName = ddd.RtuId + " - " + ddd.RtuName;
                    fid = ddd.RtuFid;
                }
                var ggg =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(fid);
                if (ggg != null)
                {
                    this.AttachRtuId = ggg.RtuId;
                    this.AttachRtuName = ggg.RtuPhyId + " - " + ggg.RtuName;
                }
            }
        }

        #endregion

        #region BegTime

        private DateTime _begTime;
        public DateTime BegTime
        {
            get { return _begTime; }
            set
            {
                if (_begTime == value) return;
                _begTime = value;
                RaisePropertyChanged(() => BegTime);
            }
        }

        #endregion

        #region EndTime
        private DateTime _endTime;
        public DateTime EndTime
        {
            get { return _endTime; }
            set
            {
                if (_endTime == value) return;
                _endTime = value;
                RaisePropertyChanged(() => EndTime);
            }
        }

        #endregion

        #region BegTime1

        private DateTime _begTime1;
        public DateTime BegTime1
        {
            get { return _begTime1; }
            set
            {
                if (_begTime1 == value) return;
                _begTime1 = value;
                RaisePropertyChanged(() => BegTime1);
            }
        }

        #endregion

        #region EndTime1
        private DateTime _endTime1;
        public DateTime EndTime1
        {
            get { return _endTime1; }
            set
            {
                if (_endTime1 == value) return;
                _endTime1 = value;
                RaisePropertyChanged(() => EndTime1);
            }
        }

        #endregion

        #region  CmdQueryHistoryData

        private DateTime _dtQueryHistoryData;
        private ICommand _cmdQueryHistoryData;
        public ICommand CmdQueryHistoryData
        {
            get
            {
                return _cmdQueryHistoryData ??
                       (_cmdQueryHistoryData = new RelayCommand(ExQueryHistoryData, CanQueryHistoryData, true
                                                   ));
            }
        }
        private void ExQueryHistoryData()
        {
            _isChaoBiao = false;
            _dtQueryHistoryData = DateTime.Now;
            if (BegTime.AddDays(3653) < EndTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在10年以内", UMessageBoxButton.Ok);
                return;
            }

            var info = Wlst.Sr.ProtocolPhone.LxMru.wst_request_mru_data;// .wlst_cnt_request_mru_data ;//.ServerPart.wlst_Mru_clinet_request_MruRecordData;
            info.WstMruRequestData.RtuId = RtuId;

            if (IsSelectIndex == 0)
            {
                ArgsInfoVisi = false;
                ArgsInfoVisi1 = true;
                info.WstMruRequestData.DtStartTime = BegTime.Ticks;
                info.WstMruRequestData.DtEndTime = EndTime.Ticks;
            }
            else if (IsSelectIndex == 1)
            {
                //info.WstMruRequestData.DtStartTime = new DateTime(BegTime.Year, BegTime.Month, 1).Ticks;

                //var end = new DateTime(EndTime.AddMonths(1).Year, EndTime.AddMonths(1).Month, 1);
                //info.WstMruRequestData.DtEndTime = new DateTime(EndTime.Year, EndTime.Month, end.AddDays(-1).Day).Ticks;
                ArgsInfoVisi = true;
                ArgsInfoVisi1 = false;
                info.WstMruRequestData.DtStartTime = BegTime.Ticks;
                info.WstMruRequestData.DtEndTime = EndTime.Ticks;

            }
            else if (IsSelectIndex == 2)
            {
                //info.WstMruRequestData.DtStartTime = new DateTime(BegTime.Year, 1, 1).Ticks;
                //info.WstMruRequestData.DtEndTime = new DateTime(EndTime.Year, 12, 31).Ticks;
                ArgsInfoVisi = true;
                ArgsInfoVisi1 = false;
                info.WstMruRequestData.DtStartTime = BegTime.Ticks;
                info.WstMruRequestData.DtEndTime = EndTime.Ticks;
            }
            else if (IsSelectIndex == 3)
            {
                ArgsInfoVisi = true;
                ArgsInfoVisi1 = false;
                info.WstMruRequestData.DtStartTime = BegTime.Ticks;
                info.WstMruRequestData.DtEndTime = EndTime.Ticks;
            }
            else if (IsSelectIndex == 4)
            {
                ArgsInfoVisi = true;
                ArgsInfoVisi1 = false;
                info.WstMruRequestData.DtStartTime = BegTime.Ticks;
                info.WstMruRequestData.DtEndTime = EndTime.Ticks;
            }


            if (IsRtuSelected)
                info.WstMruRequestData.Op = 3;
            else
                info.WstMruRequestData.Op = 4;
            OpOperator = 0;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
        }
        private bool CanQueryHistoryData()
        {
            return DateTime.Now.Ticks - _dtQueryHistoryData.Ticks > 30000000;
        }
        #endregion

        #region  CmdQueryNewData

        private DateTime _dtQueryNewData;
        private ICommand _cmdQueryNewData;
        public ICommand CmdQueryNewData
        {
            get { return _cmdQueryNewData ?? (_cmdQueryNewData = new RelayCommand(ExQueryNewData, CanQueryNewData, true)); }
        }
        private void ExQueryNewData()
        {
            _dtQueryNewData = DateTime.Now;
            _isChaoBiao = false;
            ArgsInfoVisi = true;
            ArgsInfoVisi1 = false;
            var info = Wlst.Sr.ProtocolPhone.LxMru.wst_request_mru_data;// ;//.ServerPart.wlst_Mru_clinet_request_MruRecordData;
            info.WstMruRequestData.RtuId = RtuId;
            info.WstMruRequestData.DtStartTime = DateTime.Now.Ticks;
            info.WstMruRequestData.DtEndTime = DateTime.Now.Ticks;
            if (IsRtuSelected)
                info.WstMruRequestData.Op = 1;
            else
                info.WstMruRequestData.Op = 2;
            OpOperator = 0;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
        }
        private bool CanQueryNewData()
        {
            return DateTime.Now.Ticks - _dtQueryNewData.Ticks > 30000000;
        }

        #endregion

        #region CmdQueryFailureData
        private DateTime _dtQueryFailureData;
        private ICommand _cmdQueryFailureData;
        public ICommand CmdQueryFailureData
        {
            get { return _cmdQueryFailureData ?? (_cmdQueryFailureData = new RelayCommand(ExQueryFailureData, CanQueryFailureData, true)); }
        }
        private void ExQueryFailureData()
        {
            _isChaoBiao = false;
            ArgsInfoVisi = false;
            ArgsInfoVisi1 = false;
            ArgsInfoVisi2 = false;
            _dtQueryFailureData = DateTime.Now;
            var info = Wlst.Sr.ProtocolPhone.LxMru.wst_request_mru_data;// ;//.ServerPart.wlst_Mru_clinet_request_MruRecordData;
            info.WstMruRequestData.RtuId = RtuId;
            info.WstMruRequestData.DtStartTime = BegTime.Ticks;
            info.WstMruRequestData.DtEndTime = EndTime.Ticks;
                info.WstMruRequestData.Op = 4;
                OpOperator = 1;
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
        }
        private bool CanQueryFailureData()
        {
            return DateTime.Now.Ticks - _dtQueryFailureData.Ticks > 30000000;
        }
        #endregion

        #region  Remind

        private string _remind;
        public string Remind
        {
            get { return _remind; }
            set
            {
                if (_remind == value) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }

        #endregion


        #region  CmdCb

        private DateTime _dtQuQuCbataa;
        private ICommand _dtQuCbata;
        public ICommand CmdCb
        {
            get { return _dtQuCbata ?? (_dtQuCbata = new RelayCommand(ExCb, CanCb, true)); }
        }

        private bool _isChaoBiao = false;
        private void ExCb()
        {
            _dtQuQuCbataa = DateTime.Now;
            _isChaoBiao = true;
            ArgsInfoVisi = true;
            ArgsInfoVisi1 = false;

            if (IsRtuSelected)
            {

                if (Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsMru(RtuId))
                {
                    SndReadMruData(RtuId);

                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在抄表 ... " + RtuId;
                }
                else
                {
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 该设备非电表设备 ... " + RtuId;
                }
            }
            else
            {
                foreach (
                    var g in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys)
                {
                    if (Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsMru(g))
                    {
                        SndReadMruData(g);
                    }
                }
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在对所有设备抄表 ... ";
            }
            this.Items.Clear();

        }

        private bool CanCb()
        {
            return DateTime.Now.Ticks - _dtQuQuCbataa.Ticks > 30000000;
        }


        private long guidx = 0;
        private void SndReadMruData(int rtuId)
        {

            var tmps = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuId);
            if (tmps == null) return;
            var gggg = tmps as Wlst.Sr.EquipmentInfoHolding.Model.Wj1050Mru;
            if (gggg == null) return;


            var info = Wlst.Sr.ProtocolPhone.LxMru.wst_cnt_read_mru_data;//.ServerPart.wlst_Mru_client_order_ReadData;
            ;

            info.WstMruCntRequestReadMruData.RtuId = rtuId;
            info.WstMruCntRequestReadMruData.Addr1 = gggg.WjMru.MruAddr1; // System.Convert.ToInt32(this.MruAddr1 + "", 16);
            info.WstMruCntRequestReadMruData.Addr2 = gggg.WjMru.MruAddr2; // System.Convert.ToInt32(this.MruAddr2 + "", 16);
            info.WstMruCntRequestReadMruData.Addr3 = gggg.WjMru.MruAddr3; // System.Convert.ToInt32(this.MruAddr3 + "", 16);
            info.WstMruCntRequestReadMruData.Addr4 = gggg.WjMru.MruAddr4; // System.Convert.ToInt32(this.MruAddr4 + "", 16);
            info.WstMruCntRequestReadMruData.Addr5 = gggg.WjMru.MruAddr5; // System.Convert.ToInt32(this.MruAddr5 + "", 16);
            info.WstMruCntRequestReadMruData.Addr6 = gggg.WjMru.MruAddr6; // System.Convert.ToInt32(this.MruAddr6 + "", 16); 
            info.WstMruCntRequestReadMruData.MainRtuId = tmps.RtuFid;

            info.WstMruCntRequestReadMruData.DataMruType = 4;
            info.WstMruCntRequestReadMruData.DataTimeType = 0;
            info.WstMruCntRequestReadMruData.Ver = gggg.WjMru.MruType;

            if (info.Head.Gid <= guidx) info.Head.Gid = guidx + 1;
            guidx = info.Head.Gid;
            OpOperator = 0;
            //SndOrderServer.OrderSnd(info, 10, 6);
            SndOrderServer.OrderSnd(info);
        }

        #endregion

    }

    public partial class Wj1050DataInqueryViewModel
    {
        private void InitialAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxMru.wst_request_mru_data,//.ClientPart.wlst_Mru_server_ans_clinet_request_MruRecordData,
                RequestMruRecordData,
                typeof(Wj1050DataInqueryViewModel), this,true);


            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxMru.wst_svr_ans_cnt_read_mru_data,//.ClientPart.wlst_Mru_server_ans_client_order_ReadData,
                MruReadData,
                typeof(Wj1050DataInqueryViewModel), this,true);
        }

        //0、其他 1、抄表查询失败
        int OpOperator = 0;

        private void RequestMruRecordData(string session, Wlst.mobile.MsgWithMobile info)
        {
            var infos = info.WstMruRequestData;
            if (infos == null) return;
            //   if (!infos.RequestAllNewData && infos.RtuId != this.RtuId) return;
            this.Items.Clear();
            var xxx = (from t in infos.Items orderby t.RtuId, t.DateCreate ascending select t).ToList();
            if (OpOperator == 1)
            {

                    var lst = (from t in xxx select t.RtuId).Distinct().ToList();
                    var rtu = (from t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems where t.Value.RtuModel == EnumRtuModel.Wj1050 && lst.Contains(t.Key) == false select t.Key).ToList();
                    if (rtu == null)
                    {
                        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  在" + BegTime.ToString("yyyy-MM-dd") + "至" + EndTime.ToString("yyyy-MM-dd") + "时间段内，" + "无抄表数据终端共计0 个.";
                    }
                    else
                    {
                        Cxsb(rtu);
                    }
                    return;
            }

            if (xxx.Count == 0)
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  抄表数据查询成功，共计0 条数据.";
                return;
            }

            int rtuid = 0;

            double count = 0;
            var tmps = xxx[0];
            var xxx1 = new List<MruDataRequest.MruDataItem>();
            List<double> bmrudata = new List<double>();
            List<double> emrudata = new List<double>();
            List<long> begtime = new List<long>();
            List<long> endtime = new List<long>();
            bmrudata.Add(xxx[0].MruData);
            begtime.Add(xxx[0].DateCreate);
            if (infos.Op == 3 || infos.Op == 4)
            {
                if (IsSelectIndex == 0)
                {    
                    xxx1.Add(xxx[0]);
                    var rtuidmore = 0;
                    foreach(var t in xxx)
                    {
                        if (t.RtuId != rtuidmore && t != xxx[0])
                        {
                            bmrudata.Add(t.MruData);
                            emrudata.Add(tmps.MruData);
                            begtime.Add(t.DateCreate);
                            endtime.Add(tmps.DateCreate);
                            xxx1.Add(t);
                        }
                        rtuidmore = t.RtuId;
                          tmps = t;
                    }
                    
                }
                else if (IsSelectIndex == 1)
                {
                    var dayismore = new DateTime();
                    var rtuidmore = 0;
                    foreach (var t in xxx)
                    {
                        var day = new DateTime(t.DateCreate);
                        if (t.RtuId != rtuidmore)
                        {
                            xxx1.Add(t);
                            rtuidmore = t.RtuId;
                        }
                        else if (day.Date != dayismore)
                        {
                            xxx1.Add(t);
                        }
                        dayismore = day.Date;
                    }
                }
                else if (IsSelectIndex == 2)
                {
                    var dayismore = new DateTime();
                    var rtuidmore = 0;
                    foreach (var t in xxx)
                    {
                        var day = new DateTime(t.DateCreate);
                        if (t.RtuId != rtuidmore)
                        {
                            xxx1.Add(t);
                            rtuidmore = t.RtuId;
                        }
                        else if (day.Day == EndTime.Day && t != xxx[0] && day.Date != dayismore)
                        {
                            xxx1.Add(t);
                        }
                        dayismore = day.Date;
                    }
                }
                else if (IsSelectIndex == 3)
                {
                    var rtuidmore = 0;
                    var dayismore = new DateTime();
                    foreach (var t in xxx)
                    {
                        var day = new DateTime(t.DateCreate);
                        if (t.RtuId != rtuidmore)
                        {
                            xxx1.Add(t);
                            rtuidmore = t.RtuId;
                        }
                        else if (day.Day == EndTime.Day && day.Month == EndTime.Month && t != xxx[0] && day.Date != dayismore)
                        {
                            xxx1.Add(t);
                        }
                        dayismore = day.Date;
                    }
                }
                else if (IsSelectIndex == 4)
                {
                    xxx1 = xxx;
                }
            } 
            else if (infos.Op == 1 || infos.Op == 2)
            {
                    xxx1 = xxx;            
            }
  
            emrudata.Add(xxx[xxx.Count - 1].MruData);
            endtime.Add(xxx[xxx.Count - 1].DateCreate);
            int i = 0;
            foreach (var t in xxx1)
            {
                var fidequ =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);
                int fid = 0;
                double scall = 0;
                string rtufidName = "未知";
                string remarks = "";
                string rtuname = "";
                int mruaddr1 = 0;
                int mruaddr2 = 0;
                int mruaddr3 = 0;
                int mruaddr4 = 0;
                int mruaddr5 = 0;
                int mruaddr6 = 0;
                if (fidequ != null)
                {
                    var ntsdf =
                        fidequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj1050Mru;
                    if (ntsdf != null)
                    {
                        scall = ntsdf.WjMru.MruRatio / 5.0;
                        remarks = fidequ.RtuRemark;
                        rtuname = fidequ.RtuName;
                        mruaddr1 = ntsdf.WjMru.MruAddr1;
                        mruaddr2 = ntsdf.WjMru.MruAddr2;
                        mruaddr3 = ntsdf.WjMru.MruAddr3;
                        mruaddr4 = ntsdf.WjMru.MruAddr4;
                        mruaddr5 = ntsdf.WjMru.MruAddr5;
                        mruaddr6 = ntsdf.WjMru.MruAddr6;
                    }
                    fid = fidequ.RtuFid;
                    if (fid > 1000000 && fid < 1100000)
                    {
                        fidequ =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(fid);
                        if (fidequ != null)
                        {
                            fid = fidequ.RtuPhyId;
                            rtufidName = fidequ.RtuName;
                        }
                    }
                }
                if (t.RtuId != rtuid)
                {
                    rtuid = t.RtuId;
                    count = 0;
                   

                    //double dirr = Math.Round(t.MruData - tmps.MruData, 2);
                    //count += dirr;
                    string datetype = "";
                    switch (t.DateType)
                    {
                        case 0:
                            datetype = "当前时间";
                            break;
                        case 1:
                            datetype = "上月时间";
                            break;
                        case 2:
                            datetype = "上上月时间";
                            break;
                    }

                    var dt = new DateTime(t.DateCreate);            
                    int year = dt.Year;
                    int month = dt.Month;
                    int day = dt.Day;
                    int year1 = 0, month1 = 0, day1 = 0, year2 = 0, month2 = 0, day2 = 0;
                    if (infos.Op == 3 || infos.Op == 4)
                    {
                        if (IsSelectIndex == 0)
                        {
                            var dx = new DateTime(begtime[i]);
                            year1 = dx.Year;
                            month1 = dx.Month;
                            day1 = dx.Day;
                            var dy = new DateTime(endtime[i]);
                            year2 = dy.Year;
                            month2 = dy.Month;
                            day2 = dy.Day;
                        }
                    }
                    var iinfo = new MruDataRecordViewModel(t)
                    {
                        Id = this.Items.Count + 1,
                        Differ = "--",
                        Count = count,
                        AttachRtuId = fid,
                        AttachRtuName = rtufidName,
                        RtuName = rtuname,
                        Remarks = remarks,
                        sacal = scall,
                        MruTotal = t.MruData * scall,
                        DateCreate = year + "-" + month + "-" + day,
                        MruData = t.MruData,
                        DateTypeCode = datetype,
                        BegTime1 = year1 + "-" + month1 + "-" + day1,
                        EndTime1 = year2 + "-" + month2 + "-" + day2,
                        BMruData = IsSelectIndex == 0 && (infos.Op == 3 || infos.Op == 4) ? bmrudata[i] : 0,
                        EMruData = IsSelectIndex == 0 && (infos.Op == 3 || infos.Op == 4) ? emrudata[i] : 0,
                        BMruTotal = IsSelectIndex == 0 && (infos.Op == 3 || infos.Op == 4) ? bmrudata[i] * scall : 0,
                        EMruTotal = IsSelectIndex == 0 && (infos.Op == 3 || infos.Op == 4) ? emrudata[i] * scall : 0,
                        DifferData = IsSelectIndex == 0 && (infos.Op == 3 || infos.Op == 4) ? (emrudata[i] - bmrudata[i]) : 0,
                        DifferTotal = IsSelectIndex == 0 && (infos.Op == 3 || infos.Op == 4) ? (emrudata[i] - bmrudata[i]) * scall : 0,
                        MruAddr1 = System.Convert.ToString(mruaddr1, 16).Trim().PadLeft(2, '0'),
                        MruAddr2 = System.Convert.ToString(mruaddr2, 16).Trim().PadLeft(2, '0'),
                        MruAddr3 = System.Convert.ToString(mruaddr3, 16).Trim().PadLeft(2, '0'),
                        MruAddr4 = System.Convert.ToString(mruaddr4, 16).Trim().PadLeft(2, '0'),
                        MruAddr5 = System.Convert.ToString(mruaddr5, 16).Trim().PadLeft(2, '0'),
                        MruAddr6 = System.Convert.ToString(mruaddr6, 16).Trim().PadLeft(2, '0')
                    };
                    this.Items.Add(iinfo);
                    tmps = t;
                    i++;
                }
                else
                {
                    string datetype = "";
                    switch (t.DateType)
                    {
                        case 0:
                            datetype = "当前时间";
                            break;
                        case 1:
                            datetype = "上月时间";
                            break;
                        case 2:
                            datetype = "上上月时间";
                            break;
                    }
                    rtuid = t.RtuId;
                    double dirr = Math.Round(t.MruData - tmps.MruData, 2) * scall;
                    count += dirr;

                    var dt = new DateTime(t.DateCreate);
                    int year = dt.Year;
                    int month = dt.Month;
                    int day = dt.Day;

                    var iinfo = new MruDataRecordViewModel(t)
                    {
                        Id = this.Items.Count + 1,
                        Differ = dirr + "",
                        Count = count,
                        AttachRtuId = fid,
                        AttachRtuName = rtufidName,
                        RtuName = rtuname,
                        Remarks = remarks,
                        sacal = scall,
                        MruTotal = t.MruData * scall,
                        DateCreate = year + "-" + month + "-" + day,
                        DateTypeCode = datetype,
                        MruAddr1 = System.Convert.ToString(mruaddr1, 16).Trim().PadLeft(2, '0'),
                        MruAddr2 = System.Convert.ToString(mruaddr2, 16).Trim().PadLeft(2, '0'),
                        MruAddr3 = System.Convert.ToString(mruaddr3, 16).Trim().PadLeft(2, '0'),
                        MruAddr4 = System.Convert.ToString(mruaddr4, 16).Trim().PadLeft(2, '0'),
                        MruAddr5 = System.Convert.ToString(mruaddr5, 16).Trim().PadLeft(2, '0'),
                        MruAddr6 = System.Convert.ToString(mruaddr6, 16).Trim().PadLeft(2, '0')
                    };
                    this.Items.Add(iinfo);
                    tmps = t;
                }

            }


                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  抄表数据查询成功，共计" + Items.Count + " 条数据.";

        }

        void Cxsb(List<int> rtu)
        {
          foreach (var f in rtu)
          {
              var fidequ =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f);
              int fid = 0;
              double scall = 0;
              string rtufidName = "未知";
              string remarks = "";
              string rtuname = "";
              int mruaddr1 = 0;
              int mruaddr2 = 0;
              int mruaddr3 = 0;
              int mruaddr4 = 0;
              int mruaddr5 = 0;
              int mruaddr6 = 0;
              if (fidequ != null)
              {
                  var ntsdf =
                      fidequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj1050Mru;
                  if (ntsdf != null)
                  {
                      scall = ntsdf.WjMru.MruRatio / 5.0;
                      remarks = fidequ.RtuRemark;
                      rtuname = fidequ.RtuName;
                      mruaddr1 = ntsdf.WjMru.MruAddr1;
                      mruaddr2 = ntsdf.WjMru.MruAddr2;
                      mruaddr3 = ntsdf.WjMru.MruAddr3;
                      mruaddr4 = ntsdf.WjMru.MruAddr4;
                      mruaddr5 = ntsdf.WjMru.MruAddr5;
                      mruaddr6 = ntsdf.WjMru.MruAddr6;
                  }
                  fid = fidequ.RtuFid;
                  if (fid > 1000000 && fid < 1100000)
                  {
                      fidequ =
                          Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(fid);
                      if (fidequ != null)
                      {
                          fid = fidequ.RtuPhyId;
                          rtufidName = fidequ.RtuName;
                      }
                  }
              }
              var iinfo = new MruDataRecordViewModel()
              {
                  Id = this.Items.Count + 1,
                  AttachRtuId = fid,
                  AttachRtuName = rtufidName,
                  RtuName = rtuname,
                  Remarks = remarks,
                  RtuId = f,
                  MruAddr1 = System.Convert.ToString(mruaddr1, 16).Trim().PadLeft(2, '0'),
                  MruAddr2 = System.Convert.ToString(mruaddr2, 16).Trim().PadLeft(2, '0'),
                  MruAddr3 = System.Convert.ToString(mruaddr3, 16).Trim().PadLeft(2, '0'),
                  MruAddr4 = System.Convert.ToString(mruaddr4, 16).Trim().PadLeft(2, '0'),
                  MruAddr5 = System.Convert.ToString(mruaddr5, 16).Trim().PadLeft(2, '0'),
                  MruAddr6 = System.Convert.ToString(mruaddr6, 16).Trim().PadLeft(2, '0')
              };
              this.Items.Add(iinfo);
          }
          Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  在" + BegTime.ToString("yyyy-MM-dd") + "至" + EndTime.ToString("yyyy-MM-dd") + "时间段内，" + "无抄表数据终端共计" + Items.Count + " 个.";
        }

        private void InitEvent()
        {
            AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected, PublishEventType.Core);
        }

        public override void ExPublishedEvent(PublishEventArgs args)
        {
            if (_isViewShow == false) return;
            if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
            {
                try
                {
                    int fftuId = Convert.ToInt32(args.GetParams()[0]);
                    if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsMru(fftuId))
                    {
                        this.RtuId = fftuId;
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }



        public void MruReadData(string session, Wlst.mobile.MsgWithMobile infoss)
        {
            if (_isViewShow == false) return;

            if (_isChaoBiao == false) return;

            var infos = infoss.WstMruSvrAnsCntRequestReadMruData;
            if (infos == null) return;
            var info = new MruDataRequest.MruDataItem()
            {
                DateCreate = DateTime.Now.Ticks,
                DateType = infos.DataTimeType,
                MruData = infos.DataValue,
                MruType = infos.DataMruType,
                RtuId = infos.RtuId
            };



            var fidequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(info.RtuId);
            int fid = 0;
            double scall = 0;
            string rtufidName = "未知";
            string remarks = "";
            string rtuname = "";
            int mruaddr1 = 0;
            int mruaddr2 = 0;
            int mruaddr3 = 0;
            int mruaddr4 = 0;
            int mruaddr5 = 0;
            int mruaddr6 = 0;
            if (fidequ != null)
            {
                var ntsdf =
                    fidequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj1050Mru;
                if (ntsdf != null)
                {
                    scall = ntsdf.WjMru.MruRatio / 5.0;
                    remarks = fidequ.RtuRemark;
                    rtuname = fidequ.RtuName;
                    mruaddr1 = ntsdf.WjMru.MruAddr1;
                    mruaddr2 = ntsdf.WjMru.MruAddr2;
                    mruaddr3 = ntsdf.WjMru.MruAddr3;
                    mruaddr4 = ntsdf.WjMru.MruAddr4;
                    mruaddr5 = ntsdf.WjMru.MruAddr5;
                    mruaddr6 = ntsdf.WjMru.MruAddr6;
                }
                fid = fidequ.RtuFid;
                if (fid > 1000000 && fid < 1100000)
                {
                    fidequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(fid);
                    if (fidequ != null)
                    {
                        fid = fidequ.RtuPhyId;
                        rtufidName = fidequ.RtuName;
                    }
                }
            }
            var vm = new MruDataRecordViewModel(info)
            {
                Id = this.Items.Count + 1,
                Differ = "--",
                Count = 0,
                AttachRtuId = fid,
                AttachRtuName = rtufidName,
                sacal = scall,
                MruTotal = info.MruData * scall,
                RtuName = rtuname,
                Remarks = remarks,
                MruAddr1 = System.Convert.ToString(mruaddr1, 16).Trim().PadLeft(2, '0'),
                MruAddr2 = System.Convert.ToString(mruaddr2, 16).Trim().PadLeft(2, '0'),
                MruAddr3 = System.Convert.ToString(mruaddr3, 16).Trim().PadLeft(2, '0'),
                MruAddr4 = System.Convert.ToString(mruaddr4, 16).Trim().PadLeft(2, '0'),
                MruAddr5 = System.Convert.ToString(mruaddr5, 16).Trim().PadLeft(2, '0'),
                MruAddr6 = System.Convert.ToString(mruaddr6, 16).Trim().PadLeft(2, '0')
            };

            this.Items.Add(vm);

            //this.Items.Insert(0, vm);
            string str = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ";
            str += "设备：" + vm.RtuId + "," + vm.DateTypeCode + "时间" + vm.MruTypeCode + " 抄表值为：" + vm.MruData +
                   ",电量为:" + vm.MruTotal;

            this.Remind = str;
        }

        private ICommand _dtDaoChu;
        public ICommand CmdDaoChu
        {
            get { return _dtDaoChu ?? (_dtDaoChu = new RelayCommand(ExCmdDaoChu, CanCmdDaoChu, true)); }
        }

        private void ExCmdDaoChu()
        {
            
            haveDataRtuList.Clear();
            var writeinfo = new List<List<object>>();
            var writeAllinfo = new Dictionary<int, List<List<object>>>();
            var titleinfo = new List<object>();

            titleinfo.Add("序号");
            titleinfo.Add("终端地址");
            titleinfo.Add("终端名称");
            titleinfo.Add("电表名称");
            titleinfo.Add("电表标识");
            titleinfo.Add("抄表类型");
            if (IsSelectIndex == 0)
            {
                titleinfo.Add("起始时间");
                titleinfo.Add("起始抄表值");
                titleinfo.Add("起始电量");
                titleinfo.Add("截止时间");
                titleinfo.Add("截止抄表值");
                titleinfo.Add("截止电量");
                titleinfo.Add("总抄表差额");
                titleinfo.Add("总电量差额");
            }
            else
            {
                titleinfo.Add("抄表时间");
                titleinfo.Add("抄表值");
                titleinfo.Add("电量");
                titleinfo.Add("差值");
                titleinfo.Add("累计差值");
            }
            titleinfo.Add("备注");
            titleinfo.Add("地址1");
            titleinfo.Add("地址2");
            titleinfo.Add("地址3");
            titleinfo.Add("地址4");
            titleinfo.Add("地址5");
            titleinfo.Add("地址6");




            //    var tmp = new List<object>();
            //    tmp.Add(this.Items.Count + 1);
            //    tmp.Add(para.RtuFid);
            //    tmp.Add(para.RtuName);
            //    tmp.Add(ntsdf.RtuId);
            //    tmp.Add(ntsdf.RtuName);

            //    Id = ,
            //            Differ = "--",
            //            Count = count,
            //            AttachRtuId = fid,
            //            AttachRtuName = rtufidName,
            //            RtuName = rtuname,
            //            Remarks = remarks,
            //            sacal = scall,

            //}


            var tmllst = (from t in Items orderby t.Id select t).ToList();

            foreach (var f in tmllst)
            {

                if (!haveDataRtuList.Contains(f.RtuId )) haveDataRtuList.Add(f.RtuId);

                var tmp = new List<object>();
               

                tmp.Add(f.Id);
                tmp.Add(f.AttachRtuId);
                tmp.Add(f.AttachRtuName);
                tmp.Add(f.RtuId);
                tmp.Add(f.RtuName);
                var type = f.DateTypeCode + "||" + f.MruTypeCode;
                tmp.Add(type);
                if (IsSelectIndex == 0)
                {
                    tmp.Add(f.BegTime1);
                    tmp.Add(f.BMruData.ToString("f2"));
                    tmp.Add(f.BMruTotal.ToString("f2"));
                    tmp.Add(f.EndTime1);
                    tmp.Add(f.EMruData.ToString("f2"));
                    tmp.Add(f.EMruTotal.ToString("f2"));
                    tmp.Add(f.DifferData.ToString("f2"));
                    tmp.Add(f.DifferTotal.ToString("f2"));
                }
                else
                {
                    tmp.Add(f.DateCreate);
                    tmp.Add(f.MruData.ToString("f2"));
                    tmp.Add(f.MruTotal.ToString("f2"));
                    tmp.Add(f.Differ);
                    tmp.Add(f.Count.ToString("f2"));
                }
                tmp.Add(f.Remarks);

                bool set = false;
                var fidequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.RtuId);
                if (fidequ != null)
                {
                    var ntsdf =
                        fidequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj1050Mru;
                    if (ntsdf != null && ntsdf.WjMru != null)
                    {
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr1, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr2, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr3, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr4, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr5, 16).Trim());
                        tmp.Add(System.Convert.ToString(ntsdf.WjMru.MruAddr6, 16).Trim());
                        set = true;
                    }
                }
                if (set == false)
                {
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(0);
                    tmp.Add(0);
                }
                writeinfo.Add(tmp);
                if (writeAllinfo.ContainsKey(f.AttachRtuId) == false)
                    writeAllinfo.Add(f.AttachRtuId, new List<List<object>>());
                writeAllinfo[f.AttachRtuId].Add(tmp);

            }
            //writeAllinfo.AddRange(writeinfo);
            var allMru = LoadAllMru();
            foreach (var g in allMru)
            {
                var tmpsss = new List<object>();
                var para = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                int rtuid = 0;
                if (para != null)
                {
                    string attachRtuName = "";

                    if (haveDataRtuList.Contains(para.RtuId)) continue;
                    if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(para.RtuFid))
                    {
                        attachRtuName =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[para.RtuFid].RtuName;
                        rtuid = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[para.RtuFid].RtuPhyId;
                    }
                    tmpsss.Add(0);
                    tmpsss.Add(rtuid);
                    tmpsss.Add(attachRtuName);
                    tmpsss.Add(para.RtuId);
                    tmpsss.Add(para.RtuName);
                }


                if (writeAllinfo.ContainsKey(rtuid) == false)
                    writeAllinfo.Add(rtuid, new List<List<object>>());
                writeAllinfo[rtuid].Add(tmpsss);
                //writeAllinfo.Add(tmpsss);
            }


            var tmpss = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                         where t.Value.RtuId < 1100000
                         select t.Value).ToList();
            foreach (var f in tmpss)
            {
                if (writeAllinfo.ContainsKey(f.RtuPhyId)) continue;
                var tmpsss = new List<object>();
                tmpsss.Add(0);
                tmpsss.Add(f.RtuPhyId);
                tmpsss.Add(f.RtuName);
                tmpsss.Add("无电表");
                if (writeAllinfo.ContainsKey(f.RtuPhyId) == false)
                    writeAllinfo.Add(f.RtuPhyId, new List<List<object>>());
                writeAllinfo[f.RtuPhyId].Add(tmpsss);

            }

            //var tmps = (from t in writeAllinfo orderby   select t).ToList();

            var ord = (from t in writeAllinfo orderby t.Key ascending select t.Value).ToList();
            var writeAllinfos = new List<List<object>>();
            int index = 1;
            foreach (var f in ord)
            {
                foreach (var g in f)
                {
                    g[0] = index;
                    index++;
                    writeAllinfos.Add(g);
                }
            }

            var filep=Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);
            var filepp = filep.Substring(0, filep.Length - 4) + "-1";
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(filepp, titleinfo, writeAllinfos);
        }

        private bool CanCmdDaoChu()
        {
            return DateTime.Now.Ticks - _dtQuQuCbataa.Ticks > 30000000;
        }


        protected bool IsLoadOnlyOneArea = false;
        List<int> rtuLst = new List<int>();
        List<int> haveDataRtuList = new List<int>(); 
        public List<int> LoadAllMru()
        {
            rtuLst .Clear();
            if (ServicesGrpSingleInfoHold.InfoGroups.Count == 0 &&
                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0)
                return new List<int>();
            var userProperty = UserInfo.UserLoginInfo;
            List<int> areaLst = new List<int>();
            areaLst.AddRange(userProperty.AreaX);
            foreach (var t in userProperty.AreaW)
            {
                if (!areaLst.Contains(t))
                {
                    areaLst.Add(t);
                }
            }
            foreach (var f in userProperty.AreaR)
            {
                if (!areaLst.Contains(f))
                {
                    areaLst.Add(f);
                }
            }
            IsLoadOnlyOneArea = areaLst.Count < 2;

            if (userProperty.D == true)
            {
               
                    foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                    {
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                        
                        foreach (var a in lstInArea)
                        {
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                            if (pb == null) continue;
                            if (pb.EquipmentType == WjParaBase.EquType.Mru && pb.RtuFid == 0) //线路为主设备
                            {
                                rtuLst.Add(pb.RtuId);
                            
                            }
                            else if (pb.EquipmentType == WjParaBase.EquType.Rtu &&
                                     pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有线路
                            {
                                foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                                {
                                    var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                    if (pa == null) continue;
                                    if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                                    {
                                        rtuLst.Add(g);
                                      

                                    }
                                }
                            }
                        }
                    }
               

            }
            else
            {           
                    foreach (var f in areaLst)
                    {
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);

                        foreach (var a in lstInArea)
                        {
                            var pb = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(a);
                            if (pb == null) continue;
                            if (pb.EquipmentType == WjParaBase.EquType.Mru && pb.RtuFid == 0) //线路为主设备
                            {
                                rtuLst.Add(pb.RtuId);
                                //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                //break; ;
                            }
                            else if (pb.EquipmentType == WjParaBase.EquType.Rtu &&
                                     pb.EquipmentsThatAttachToThisRtu.Count > 0) //haha 特殊终端下有线路
                            {
                                foreach (var g in pb.EquipmentsThatAttachToThisRtu)
                                {
                                    var pa = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                                    if (pa == null) continue;
                                    if (pa.EquipmentType == WjParaBase.EquType.Mru && pa.RtuFid > 0)
                                    {
                                        rtuLst.Add(g);
                                        //this.ChildTreeItems.Add(new TreeNodeAreaViewModel(null, f, 0, TypeOfTabTreeNode.IsArea));
                                        //break;

                                    }
                                }
                            }
                        }
                     
                    }

            } return rtuLst;

        }









    }



}
