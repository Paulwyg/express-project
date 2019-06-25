using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Ux.StateBarModule.SendFailOperation.Service;
using Wlst.mobile;

namespace Wlst.Ux.StateBarModule.SendFailOperation.ViewModel
{

    [Export(typeof(IISendFailOperationViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class SendFailOperationViewModel: VmEventActionProperyChangedBase, IISendFailOperationViewModel
    {
        public SendFailOperationViewModel()
        {
            Title = "下发失败查询";
            InitAction();
            InitEvent();
        }

        private bool _isViewVisual;
        public override void NavOnLoadr(params object[] parsObjects)
        {
            if (_isViewVisual) return;
            _isViewVisual = true;
            GetAllOperatorTypes();

            //BeginDate = DateTime.Now.AddDays(-1);
            //EndDate = DateTime.Now;
            //RequestAllUserInfomation();

            //Remind = "请设置好查询日期[最多一个月]以及高级查询条件进行查询...";
        }

        public override void OnUserHideOrClosingr()
        {
            _isViewVisual = false;
            //UserItems = new ObservableCollection<NameIntBool>();
            //TypeItems = new ObservableCollection<OperatorTypeItem>();
            //Items = new ObservableCollection<OperatorOneRecordViewModel>();
            //ExportVisi = Visibility.Collapsed;
        }

    }

    /// <summary>
    /// 绑定属性字段及其命令
    /// </summary>
    public partial class SendFailOperationViewModel
    {
        #region Field


        #region

        private static ObservableCollection<TypeInt> _typedevices;

        public static ObservableCollection<TypeInt> TypeName
        {
            get
            {
                if (_typedevices == null)
                {
                    _typedevices = new ObservableCollection<TypeInt>();
                }
                return _typedevices;
            }

        }

        public class TypeInt : Wlst.Cr.Core.CoreServices.ObservableObject
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

            private string _value;

            public string Value
            {
                get { return _value; }
                set
                {
                    if (value != _value)
                    {
                        _value = value;
                        this.RaisePropertyChanged(() => this.Value);
                    }
                }
            }
        }

        private TypeInt _typeComboboxselected;
        private int TypeId;

        public TypeInt TypeComboBoxSelected
        {
            get { return _typeComboboxselected; }
            set
            {
                if (_typeComboboxselected != value)
                {
                    _typeComboboxselected = value;
                    this.RaisePropertyChanged(() => this.TypeComboBoxSelected);
                    if (value == null) return;
                    TypeId = value.Key;

                    this.Records.Clear();
                    switch (TypeId)
                    {
                        case 1:
                        case 2: //普通终端参数（包括集中器）
                        case 3:
                        case 4:
                        case 6:
                            TypeOneVisi = Visibility.Visible;
                            TypeTwoVisi = Visibility.Collapsed;
                            TypeThreeVisi = Visibility.Collapsed;
                            break;                       
                        case 5: //控制器
                        case 7:
                            TypeOneVisi = Visibility.Collapsed;
                            TypeTwoVisi = Visibility.Visible;
                            TypeThreeVisi = Visibility.Collapsed;
                            break;
                        default:
                            TypeOneVisi = Visibility.Visible;
                            TypeTwoVisi = Visibility.Collapsed;
                            TypeThreeVisi = Visibility.Collapsed;
                            break;
                    }



                }
            }
        }

        #endregion


        #endregion

        #region property
        #region BeginDate
        private DateTime _beginDate;
        /// <summary>
        /// 查询开始时间
        /// </summary>
        public DateTime BeginDate
        {
            get
            {
                return _beginDate;
            }
            set
            {
                if (value == _beginDate) return;
                _beginDate = value;
                RaisePropertyChanged(() => BeginDate);
            }
        }
        #endregion

        #region  EndDate

        private DateTime _endDate;
        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (value == _endDate) return;
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
            }
        }

        #endregion

        #region IsTmlChecked

        private bool _isTmlChecked;
        /// <summary>
        /// 
        /// </summary>
        public bool IsTmlChecked
        {
            get { return _isTmlChecked; }
            set
            {
                if (value == _isTmlChecked) return;
                _isTmlChecked = value;
                RaisePropertyChanged(() => IsTmlChecked);
            }
        }

        #endregion

        #region DetailInfo
        private string _detailInfo;
        /// <summary>
        /// 
        /// </summary>
        public string DetailInfo
        {
            get { return _detailInfo; }
            set
            {
                if (_detailInfo == value) return;
                _detailInfo = value;
                RaisePropertyChanged(() => DetailInfo);
            }
        }
        #endregion


        #region DetailVisible
        private string _detailVisible;
        /// <summary>
        /// 
        /// </summary>
        public string DetailVisible
        {
            get { return _detailVisible; }
            set
            {
                if (_detailVisible == value) return;
                _detailVisible = value;
                RaisePropertyChanged(() => DetailVisible);
            }
        }
        #endregion

        #region Items

        private ObservableCollection<SendFailOneRecordViewModel> _items;
        /// <summary>
        /// 查询结果信息
        /// </summary>
        public ObservableCollection<SendFailOneRecordViewModel> Records
        {
            get { return _items ?? (_items = new ObservableCollection<SendFailOneRecordViewModel>()); }
            set
            {
                if (_items == value) return;
                _items = value;
                this.RaisePropertyChanged(() => this.Records);
            }
        }

        #endregion

        #region RtuName
        private string _rtuName;

        /// <summary>
        /// 
        /// </summary>
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    RaisePropertyChanged(() => RtuName);
                }
            }
        }
        #endregion

        #region Remind

        private string _remind;
        /// <summary>
        /// 提示信息
        /// </summary>
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

        #region ExportVisi

        private Visibility _exportVisi = Visibility.Collapsed;
        /// <summary>
        /// 
        /// </summary>
        public Visibility ExportVisi
        {
            get { return _exportVisi; }
            set
            {
                if (value == _exportVisi) return;
                _exportVisi = value;
                RaisePropertyChanged(() => ExportVisi);
            }
        }
        #endregion

        private int _rtuId;

        /// <summary>
        /// 
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value == _rtuId) return;
                _rtuId = value;
                //PhyId = value;
                RaisePropertyChanged(() => RtuId);
                RtuName = "未知设备";
                if (
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                        InfoItems.ContainsKey
                        (_rtuId))
                {
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [_rtuId];
                    RtuName = tml.RtuPhyId+ " - " + tml.RtuName  ;

                }
                else if (Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(_rtuId) != null)
                {

                    var tml =
                        Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(_rtuId);
                    RtuName = tml.PhyId.ToString() + " - " + tml.FieldName;

                    //if (tml.PhyId == 0)
                    //    SluPhyId = tml.PhyId.ToString();
                    //else SluPhyId = value.ToString();
                }
            }




            //PhyId = tml.PhyId;
        }



        //private int _iphyd;

        //public int PhyId
        //{
        //    get { return _iphyd; }
        //    set
        //    {
        //        if (_iphyd != value)
        //        {
        //            _iphyd = value;
        //            this.RaisePropertyChanged(() => this.PhyId);
        //        }
        //    }
        //}



        #region TypeVisi

        private Visibility _typeOneVisi = Visibility.Collapsed;
        /// <summary>
        /// 
        /// </summary>
        public Visibility TypeOneVisi
        {
            get { return _typeOneVisi; }
            set
            {
                if (value == _typeOneVisi) return;
                _typeOneVisi = value;
                RaisePropertyChanged(() => TypeOneVisi);
            }
        }



        private Visibility _typeTwoVisi = Visibility.Collapsed;
        /// <summary>
        /// 
        /// </summary>
        public Visibility TypeTwoVisi
        {
            get { return _typeTwoVisi; }
            set
            {
                if (value == _typeTwoVisi) return;
                _typeTwoVisi = value;
                RaisePropertyChanged(() => TypeTwoVisi);
            }
        }


        private Visibility _typeThreeVisi = Visibility.Collapsed;
        /// <summary>
        /// 
        /// </summary>
        public Visibility TypeThreeVisi
        {
            get { return _typeThreeVisi; }
            set
            {
                if (value == _typeThreeVisi) return;
                _typeThreeVisi = value;
                RaisePropertyChanged(() => TypeThreeVisi);
            }
        }
        #endregion



        #endregion

        #region ICommand

        #region CmdQuery

        private DateTime _dtQuery;
        private ICommand _cmdQuery;
        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand CmdQuery
        {
            get { return _cmdQuery ?? (_cmdQuery = new RelayCommand(ExCmdQuery, CanCmdQuery, true)); }
        }
        private void ExCmdQuery()
        {
            _dtQuery = DateTime.Now;
            Query();
            //Remind = "查询命令已发送...，请等待数据反馈！";
            ExportVisi = Visibility.Visible;
        }
        private bool CanCmdQuery()
        {
            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }
        #endregion


        #region CmdSend

        private DateTime _dtSend;
        private ICommand _cmdSend;
        /// <summary>
        /// 查询命令
        /// </summary>
        public ICommand CmdSend
        {
            get { return _cmdSend ?? (_cmdSend = new RelayCommand(ExCmdSend, CanCmdSend, true)); }
        }
        private void ExCmdSend()
        {
            _dtSend = DateTime.Now;
            Send();
            //Remind = "下发命令已发送...，请稍后查询！";
        }
        private bool CanCmdSend()
        {
            return DateTime.Now.Ticks - _dtSend.Ticks > 30000000;
        }
        #endregion


        #region CmdExport
        private DateTime _dtCmdExport;
        private ICommand _cmdCmdExport;

        public ICommand CmdExport
        {
            get
            {
                if (_cmdCmdExport == null)
                    _cmdCmdExport = new RelayCommand(ExCmdExport, CanExCmdExport, false);
                return _cmdCmdExport;
            }
        }

        private void ExCmdExport()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var lsttitle = new List<Object>();
                lsttitle.Add("序号");
                lsttitle.Add("终端地址");
                lsttitle.Add("终端名称");
                //控制器
                if (TypeId == 5 || TypeId == 7)
                {
                    lsttitle.Add("控制器地址");
                    lsttitle.Add("控制器名称");
                }

                lsttitle.Add("内容");



                var lstobj = new List<List<object>>();

                foreach (var g in Records)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.PhyId);
                    tmp.Add(g.RtuName);
                    //控制器
                    if (TypeId == 5 || TypeId == 7)
                    {
                        tmp.Add(g.PhyId2);
                        tmp.Add(g.RtuName2);
                    }

                    tmp.Add(g.ParaInfo);

                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出报表时报错:" + ex);
            }

        }

        private bool CanExCmdExport()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 10000000;
            return false;
        }

        #endregion



        #endregion

    }


    /// <summary>
    /// 方法
    /// </summary>
    public partial class SendFailOperationViewModel
    {
        /// <summary>
        /// 选中终端变化  提取数据
        /// </summary>
        /// <param name="rtuId"></param>
        private void SelectRtuIdChange(int rtuId)
        {
            if (rtuId < 1) return;
            RtuId = rtuId;

        }

        private void Query()
        {
            this.Records.Clear();
            Remind = "正在查询......";
            if (_isViewVisual == false) return;
            //Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(GetAllOperatorTypes1, infos);

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_get_or_set_paras_need_snd_info;
            //查询未下发成功的设备列表
            info.WstGetOrSetParasNeedSndInfo.Op = 2;
            //参数类别
            info.WstGetOrSetParasNeedSndInfo.TypeId = TypeId;
            //需要查询的终端列表
            var rtulst = new List<int>();
            if (IsTmlChecked) rtulst.Add(this.RtuId); 
            info.WstGetOrSetParasNeedSndInfo.RtuIds = rtulst;
            //序列化，请求数据结构
            var base64data = System.Convert.ToBase64String(MsgWithMobile.SerializeToBytes(info));

            //http get
            var url = "http://" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + ":" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort + "/mims/getparasnd";
            var data = wlst.sr.iif.HttpGetPost.HttpGet(url, "?pb2=" + base64data);
            //var data1 = wlst.sr.iif.HttpGetPost.HttpPost(url , "pb2=" + base64data);

            if (data == null) return;
            // 反序列化get到的数据
            var databk = MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
            if (databk == null || databk.WstGetOrSetParasNeedSndInfo == null) return;

            var rtuInfo = databk.WstGetOrSetParasNeedSndInfo.Items;
            if (rtuInfo.Count > 0) this.Records.Clear();
            foreach (var g in rtuInfo)
            {
                var tmp = new SendFailOneRecordViewModel()
                {
                    RtuId = g.RtuId,
                    RtuId2 = g.RtuId2,
                    RtuId3 = g.RtuId3,
                    RtuName = g.RtuName,
                    RtuName2 = g.RtuName2,
                    RtuName3 = g.RtuName3,
                    PhyId = g.RtuPhyId,
                    PhyId2 = g.RtuPhyId2,
                    PhyId3 = g.RtuPhyId3,
                    ParaInfo = g.ParaInfo,
                };
                tmp.Index = this.Records.Count + 1;
                this.Records.Add(tmp);

            }
            Remind = "查询成功，共有 "+ Records.Count + " 条记录";

        }

        private void Send()
        {
            if (_isViewVisual == false) return;
            if (this.Records == null || this.Records.Count == 0)
            {
                UMessageBox.Show("提醒", "未找到需要下发相关指令的记录", UMessageBoxButton.Ok);
                return;
            }
            //Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(GetAllOperatorTypes1, infos);

            Remind = "下发命令已发送...";
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_get_or_set_paras_need_snd_info;
            //查询未下发成功的设备列表
            info.WstGetOrSetParasNeedSndInfo.Op = 3;
            //参数类别
            info.WstGetOrSetParasNeedSndInfo.TypeId = TypeId;
            //需要下发的终端列表
            var rtuInfo = new List<client.GetOrSetParasNeedSndInfo.RtuInfo>();

            foreach (var g in this.Records)
            {
                rtuInfo.Add(new client.GetOrSetParasNeedSndInfo.RtuInfo() {
                    RtuId = g.RtuId,
                    RtuId2 = g.RtuId2,
                    RtuId3 = g.RtuId3,
                    RtuName= g.RtuName,
                    RtuName2= g.RtuName2,
                    RtuName3 = g.RtuName3,
                    RtuPhyId= g.PhyId,
                    RtuPhyId2= g.PhyId2,
                    RtuPhyId3 = g.PhyId3,
                    ParaInfo = g.ParaInfo
                });
            }


            info.WstGetOrSetParasNeedSndInfo.Items = rtuInfo;
            //序列化，请求数据结构
            var base64data = System.Convert.ToBase64String(MsgWithMobile.SerializeToBytes(info));

            //http get
            var url = "http://" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + ":" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort + "/mims/getparasnd";
            var data = wlst.sr.iif.HttpGetPost.HttpGet(url, "?pb2=" + base64data);
            //var data1 = wlst.sr.iif.HttpGetPost.HttpPost(url , "pb2=" + base64data);

            if (data == null) return;
            // 反序列化get到的数据
            var databk = MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
            if (databk == null || databk.WstGetOrSetParasNeedSndInfo == null) return;
            if (databk.Head.Scc == 1) Remind = "已下发至中间层，请稍后查询.....";


        }

    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class SendFailOperationViewModel
    {
        public void InitAction()
        {

            //this.RegistProtocol(
            //    Sr.ProtocolPhone.LxSys.wst_sys_operator_record,//.wlst_svr_ans_cnt_request_operator_type,
            //    GetAllOperatorTypes, true);

        }

        /// <summary>
        /// 获取操作类型
        /// </summary>
        /// <param name="session"></param>
        /// <param name="infos"></param>
        private void GetAllOperatorTypes() //string session, Wlst.mobile.MsgWithMobile infos
        {
            if (_isViewVisual == false) return;
            //Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(GetAllOperatorTypes1, infos);

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_get_or_set_paras_need_snd_info;
            //1. 请求
            info.WstGetOrSetParasNeedSndInfo.Op = 1;


            //序列化，请求数据结构
            var base64data = System.Convert.ToBase64String(MsgWithMobile.SerializeToBytes(info));

            //http get
            var url = "http://" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + ":" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort + "/mims/getparasnd";
            var data = wlst.sr.iif.HttpGetPost.HttpGet(url, "?pb2=" + base64data);
            //var data1 = wlst.sr.iif.HttpGetPost.HttpPost(url , "pb2=" + base64data);

            if (data == null) return;
            // 反序列化get到的数据
            var databk = MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
            if (databk == null || databk.WstGetOrSetParasNeedSndInfo == null) return;

            var typelst = databk.WstGetOrSetParasNeedSndInfo.TypeItems;
            if (typelst.Count > 0) TypeName.Clear();
            foreach (var g in typelst)
            {
                TypeName.Add(new TypeInt() { Value = g.TypeName, Key = g.TypeId });
            }


            //默认第一个
            TypeComboBoxSelected = TypeName[0];



        }



        private void InitEvent()
        {
            AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                               PublishEventType.Core);
        }

        public override void ExPublishedEvent(
            PublishEventArgs args)
        {
            if (!_isViewVisual) return;

            switch (args.EventId)
            {

                case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:
                    {


                        int id = Convert.ToInt32(args.GetParams()[0]);

                        //if (id > 1100000)
                        //{
                        //    var tmps = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(id);
                        //    if (tmps == null) return;
                        //    if(tmps .AttachRtuId !=0)
                        //    {
                        //        id = tmps.AttachRtuId;
                        //    }

                        //}
                        //  if (id < 1000000 || id > 1100000) return;

                        if (IsTmlChecked)
                        {
                            SelectRtuIdChange(id);

                        }
                    }
                    break;

            }
        }

    }

   
}
