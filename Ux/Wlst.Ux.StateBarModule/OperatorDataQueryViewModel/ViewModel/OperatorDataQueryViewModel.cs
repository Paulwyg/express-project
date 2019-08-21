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
using Wlst.Ux.StateBarModule.OperatorDataQueryViewModel.Service;
using Wlst.mobile;

namespace Wlst.Ux.StateBarModule.OperatorDataQueryViewModel.ViewModel
{
    [Export(typeof(IIOperatorDataQueryViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class OperatorDataQueryViewModel : VmEventActionProperyChangedBase, IIOperatorDataQueryViewModel
    {

        public OperatorDataQueryViewModel()
        {
            Title = "操作数据查询";
            InitAction();
            InitEvent();
        }

        private bool _isViewVisual;
        public override void NavOnLoadr(params object[] parsObjects)
        {
            if (_isViewVisual) return;
            _isViewVisual = true;
            UserItems.Clear();
            TypeItems.Clear();
            Items.Clear();
            IsAdvanceQueryChecked = false;
            BeginDate = DateTime.Now.AddDays(-1);
            EndDate = DateTime.Now;
            RequestAllUserInfomation();
            RequestAllOperatorTypeHttp();
            Remind = "";
        }

        public override void OnUserHideOrClosingr()
        {
            _isViewVisual = false;
            UserItems = new ObservableCollection<NameIntBool>();
            TypeItems = new ObservableCollection<OperatorTypeItem>();
            Items = new ObservableCollection<OperatorOneRecordViewModel>();
            ExportVisi = Visibility.Collapsed;
            ItemCount = 0;
            PageTotal = "";
        }

    }

    /// <summary>
    /// 绑定属性字段及其命令
    /// </summary>
    public partial class OperatorDataQueryViewModel
    {
        #region Field

        private readonly List<int> _lstOpetatorType = new List<int>();
        private string _selecteduser;
        private int _selectedsetsndans;

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

        #region IsAdvanceQueryChecked

        private bool _isAdvanceQueryChecked;
        public bool IsAdvanceQueryChecked
        {
            get { return _isAdvanceQueryChecked; }
            set
            {
                if (value == _isAdvanceQueryChecked) return;
                _isAdvanceQueryChecked = value;
                if (_isAdvanceQueryChecked)
                {
                    IsTmlChecked = false;
                    foreach (var item in OrderTransmissionTypeItems)
                    {
                        item.IsSelected = item.Value == 0;
                    }
                    foreach (var typeItem in TypeItems)
                    {
                        foreach (var item in typeItem.Value)
                        {
                            item.IsSelected = false;
                        }
                        typeItem.Key.IsSelected = false;
                        
                    }
                    if (TypeItems.Count > 0)
                    {
                        TypeItems[0].Key.IsSelected = true;
                    }

                    if (CurrentSelectUser == null && UserItems.Count > 0) CurrentSelectUser = UserItems[0];
                    //foreach (var item in UserItems)
                    //{
                    //    item.IsSelected = item.Name == "所有";
                    //}
                }
                RaisePropertyChanged(() => IsAdvanceQueryChecked);
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

        private ObservableCollection<OperatorOneRecordViewModel> _items;
        /// <summary>
        /// 查询结果信息
        /// </summary>
        public ObservableCollection<OperatorOneRecordViewModel> Items
        {
            get { return _items ?? (_items = new ObservableCollection<OperatorOneRecordViewModel>()); }
            set
            {
                if (_items == value) return;
                _items = value;
                this.RaisePropertyChanged(() => this.Items);
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
                    Items.Clear();
                    RequestHttpData(BeginDate, EndDate, RtuId, _lstOpetatorType, _selectedsetsndans, _selecteduser,
                                    value, 1);

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

        private Visibility _pagerVisi = Visibility.Collapsed;
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
                    RtuName = value + " - " + tml.RtuName + "    物理地址:" + tml.RtuPhyId;

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

        private ObservableCollection<NameIntBool> _userItems;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NameIntBool> UserItems
        {
            get { return _userItems ?? (_userItems = new ObservableCollection<NameIntBool>()); }
            set
            {
                if (value == _userItems) return;
                _userItems = value;
                this.RaisePropertyChanged(() => UserItems);
            }
        }

        private NameIntBool _currnetselectuser;

        public NameIntBool CurrentSelectUser
        {
            get { return _currnetselectuser; }
            set
            {
                if (value == _currnetselectuser) return;
                _currnetselectuser = value;
                this.RaisePropertyChanged(() => this.CurrentSelectUser);
                if (value != null)
                {
                    _selecteduser = value.Name;
                    if (_selecteduser == "所有")
                    {
                        _selecteduser = string.Empty;
                    }
                }
                else
                {
                    _selecteduser = string.Empty;
                }
            }
        }

        private ObservableCollection<NameIntBool> _orderTransmissionTypeItems;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NameIntBool> OrderTransmissionTypeItems
        {
            get
            {
                return _orderTransmissionTypeItems ??
                       (_orderTransmissionTypeItems = new ObservableCollection<NameIntBool>
                                                          {
                                                              new NameIntBool {Name = "所有", IsSelected = true, Value = 0},
                                                              new NameIntBool {Name = "下发指令", IsSelected = false, Value = 1},
                                                              new NameIntBool {Name = "设置指令", IsSelected = false, Value = 2},
                                                              new NameIntBool {Name = "设备上传", IsSelected = false, Value = 3}
                                                          });
            }
        }

        private ObservableCollection<OperatorTypeItem> _typeItems;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<OperatorTypeItem> TypeItems
        {
            get { return _typeItems ?? (_typeItems = new ObservableCollection<OperatorTypeItem>()); }
            set
            {
                if (value == _typeItems) return;
                _typeItems = value;
                this.RaisePropertyChanged(() => TypeItems);
            }
        }

       



  



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
            Remind = "正在查询...";
            Query();
            ExportVisi = Visibility.Visible;
        }
        private bool CanCmdQuery()
        {
            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
        }
        #endregion


        //#region CmdSelectAllorNot

        //private ICommand _cmdSelectAllorNot;
        ///// <summary>
        ///// 全选/全不选命令
        ///// </summary>
        //public ICommand CmdSelectAllorNot
        //{
        //    get { return _cmdSelectAllorNot ?? (_cmdSelectAllorNot = new RelayCommand(ExCmdSelectAllorNot, CanCmdSelectAllorNot, true)); }
        //}
        //private void ExCmdSelectAllorNot()
        //{
        //    foreach (var typeitem in TypeItems)
        //    {
        //        foreach (var item in typeitem.Value)
        //        {
        //            item.IsSelected = typeitem.IsSelectedAll;
        //        }
        //    }

        //}
        //private bool CanCmdSelectAllorNot()
        //{
        //    return true;
        //}

        

        //#endregion
    }

    /// <summary>
    /// 方法
    /// </summary>
    public partial class OperatorDataQueryViewModel
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

        /// <summary>
        /// 获取用户选中的信息
        /// </summary>
        private bool GetCheckedInformation()
        {
            if (BeginDate.AddDays(31) < EndDate)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在30天以内", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            #region
            //foreach (var nameIntBool in UserItems)
            //{
            //    if (!nameIntBool.IsSelected) continue;
            //    _selecteduser = nameIntBool.Name;
            //    if(_selecteduser=="所有")
            //    {
            //        _selecteduser = string.Empty;
            //    }
            //    break;
            //}
            //if(CurrentSelectUser !=null )
            //{
            //    _selecteduser = CurrentSelectUser.Name;
            //    if (_selecteduser == "所有")
            //    {
            //        _selecteduser = string.Empty;
            //    }
            //}
            #endregion
            _lstOpetatorType.Clear();
            foreach (var typeitem in TypeItems)
            {
                foreach (var item in typeitem.Value)
                {
                    if (item.IsSelected)
                    {
                        _lstOpetatorType.Add(item.Value);
                    }
                }
            }

            foreach (NameIntBool orderTransmissionTypeItem in OrderTransmissionTypeItems)
            {
                if (!orderTransmissionTypeItem.IsSelected) continue;
                _selectedsetsndans = orderTransmissionTypeItem.Value;
                break;
            }
            return true;
        }

        private void Query()
        {
            if (!GetCheckedInformation()) return;
            this.Items.Clear();
            //RequestTaskQueryData(BeginDate, EndDate, RtuId, _lstOpetatorType, _selectedsetsndans, _selecteduser);
            PageIndex = 0;
            RequestHttpData(BeginDate, EndDate, RtuId, _lstOpetatorType, _selectedsetsndans, _selecteduser, PageIndex, 0);
        }
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class OperatorDataQueryViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public void InitAction()
        {

            //this.RegistProtocol(
            //    Sr.ProtocolPhone.LxSys.wst_sys_operator_record ,//.wlst_svr_ans_cnt_request_operator_type,
            //    GetAllOperatorTypes,true );

            //this.RegistProtocol(Sr.ProtocolPhone.ClientListen.wlst_svr_ans_cnt_record_request_operator,
            //                    GetListOperators);
            this.RegistProtocol(
                Sr.ProtocolPhone.LxLogin.wst_request_user_info,
                OnRequestAllUserInfo1,true );
        }

        //private void GetAllOperatorTypes(string session, Wlst.mobile.MsgWithMobile infos)
        //{
        //    if (IsViewShowd == false) return;
        //    Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(GetAllOperatorTypes1, infos);
        //}

        private void GetAllOperatorTypes1(object infosss, int pagingFlag, int type)
        {
            var infos = infosss as MsgWithMobile;
            var info = infos.WstSysOperatorRecord;
            // var info = args.GetParams()[1] as AllUserInfo;
            if (info == null) return;
            if (pagingFlag == 0)
            {
                PageSize = infos.Head.PagingNum;
                ItemCount = infos.Head.PagingRecordTotal;
                var count = ItemCount/PageSize + (ItemCount%PageSize > 0 ? 1 : 0);
                PagerVisi = count < 2 ? Visibility.Collapsed : Visibility.Visible;
                PageTotal = "页     " + ItemCount + " 条";
            }
            if (infos.Head.Scc == 0)
            {
                Remind =   "查询成功，共" + ItemCount + "条数据.";
            }
            else
            {
                Remind =  "查询失败.";
            }
            if (type == 1)
            {
                foreach (var item in info.TypeItems)
                {
                    var isContainThisType = false;
                    foreach (var t in TypeItems)
                    {
                        if (t.Key.Value != item.CollectionId) continue;
                        isContainThisType = true;
                        var temp = new NameIntBool {Name = item.Name, Value = item.Id};
                        if (!t.Value.Contains(temp))
                        {
                            t.Value.Add(temp);
                        }
                    }
                    if (isContainThisType) continue;
                    var tempvalue = new ObservableCollection<NameIntBool>
                                        {
                                            new NameIntBool
                                                {
                                                    IsSelected = false,
                                                    Name = item.Name,
                                                    Value = item.Id
                                                }
                                        };
                    TypeItems.Add(new OperatorTypeItem
                                      {
                                          Key =
                                              new NameIntBool
                                                  {
                                                      IsSelected = false,
                                                      Name = item.CollectionName,
                                                      Value = item.CollectionId
                                                  },
                                          Name = item.CollectionName.Substring(0, 2),
                                          Value = tempvalue,


                                      });
                }
            }
            else
            {

                //  var tmpitems = new ObservableCollection<OperatorOneRecordViewModel>();
                //Items.Clear();

                for (int i = 0; i < info.RecordItems.Count; i++)
                {
                    var item = info.RecordItems[i];
                    Items.Add(new OperatorOneRecordViewModel(item, TypeItems, i + 1+PageIndex*PageSize));
                    if (i%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo

                }
                //Items = tmpitems;


                //Remind = "数据装载完毕...请查看...";
                //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 操作记录查询成功，共计" + info.RecordItems.Count +
                //         " 条数据.";
            }
        }




        private void OnCmdWatchDetailInfo(object sender, EventArgs e)
        {
            var oneRecord = sender as OperatorOneRecordViewModel;
            if (oneRecord == null) return;
            DetailInfo = oneRecord.Addresses;
            var ar = new PublishEventArgs
                         {
                             EventId = Services.EventIdAssign.AnimationOperatorDataQueryViewModelEnterId,
                             EventType = PublishEventType.None
                         };
            EventPublish.PublishEvent(ar);

        }

        //private void LoadOperators(object list)
        //{
        //    List<RecordOperator> record = list as List<RecordOperator>;
        //    if(record==null) return;
        //    int i = 0;
        //    List<OperatorOneRecordViewModel> lst = new List<OperatorOneRecordViewModel>();
        //    foreach (var item in record)
        //    {
        //        //for (int k = 0; k < 20; k++)
        //        //{

        //        //    if (i == 100)
        //        //    {
        //        //        Wlst.Cr.CoreOne.Services.AsynObservableCollectionAdd.Inserts(Items, lst);
        //        //        i = 0;
        //        //    }
        //        //    else
        //        //    {
        //        //        lst.Add(new OperatorOneRecordViewModel(item, TypeItems));
        //        //        i++;
        //        //    }
        //        //}
        //         Wlst.Cr.Core.CoreServices.AsynObservableCollectionAdd.Insert(Items,new OperatorOneRecordViewModel(item,TypeItems));
        //    }
        //    foreach (OperatorOneRecordViewModel item in Items)
        //    {
        //        item.OnCmdWatchDetailInfo += OnCmdWatchDetailInfo;
        //    }
        //    Remind = "数据反馈完毕，请查看数据！";
        //}


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

        //private void OnRequestAllUserInfo(string session, Wlst.mobile.MsgWithMobile infos)
        //{
        //    if (IsViewShowd == false) return;
        //    Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(OnRequestAllUserInfo1, infos);
        //}

        private void OnRequestAllUserInfo1(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (IsViewShowd == false) return;
            //var infos = infosss as MsgWithMobile;
            if (infos == null) return;
            var info = infos.WstLoginRequestUserInfo;
            UserItems.Clear();
            UserItems.Add(new NameIntBool {Name = "所有", IsSelected = true});
            if (info.UserInfo.Count > 0)
            {
                foreach (var t in info.UserInfo)
                {
                    UserItems.Add(new NameIntBool {Name = t.UserName, IsSelected = false});
                }
            }
            else
            {
                UserItems.Add(new NameIntBool { Name = UserInfo.UserLoginInfo.UserName, IsSelected = true });
            }
            CurrentSelectUser = UserItems[0]; 
        }


    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class OperatorDataQueryViewModel
    {
        private void RequestAllUserInfomation()
        {
            // LogInfo.Log("正在请求所有用户信息!!!");
            var info = Sr.ProtocolPhone.LxLogin .wst_request_user_info ;//.ServerPart.wlst_PrivilegeInfo_clinet_request_UserInfomation;
            SndOrderServer.OrderSnd(info, 10, 6);

        }

        private void RequestAllOperatorType()
        {
            
            //  LogInfo.Log("正在请求所有操作类型信息!!!");
            var info = Sr.ProtocolPhone.LxSys .wst_sys_operator_record ;
            info.WstSysOperatorRecord.Op = 1;
            SndOrderServer.OrderSnd(info); //发送6次,现在取消,lvf 2018年8月30日17:13:35
        }

        private void RequestAllOperatorTypeHttp()
        {

            //  LogInfo.Log("正在请求所有操作类型信息!!!");
            var info = Sr.ProtocolPhone.LxSysHttp.wst_sys_operator_record_type_http;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (data == null) return;
            GetAllOperatorTypes1(data, 1,1);
        }

        private void RequestTaskQueryData(DateTime dtstarttime, DateTime dtendtime, int tml, List<int> lstOpetatorType, int setsndans, string user)
        {
            // LogInfo.Log("正在请求所有类型信息!!!");

            var info = Wlst.Sr.ProtocolPhone.LxSys .wst_sys_operator_record ;
            info.WstSysOperatorRecord.Op = 2;


            info.WstSysOperatorRecord.DtStartTime = new DateTime(dtstarttime.Year, dtstarttime.Month, dtstarttime.Day, 0, 0, 1).Ticks;
            info.WstSysOperatorRecord.DtEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59).Ticks;
            info.WstSysOperatorRecord.RtuId = IsTmlChecked ? tml : 0;

            info.WstSysOperatorRecord.OperatorIds = lstOpetatorType;
            info.WstSysOperatorRecord.UserName = user;
            info.WstSysOperatorRecord.IsClientSnd = setsndans;
            SndOrderServer.OrderSnd(info, 10, 6);

            Remind =  "正在查询...";
        }

        //http请求
        private void RequestHttpData(DateTime dtstarttime, DateTime dtendtime, int tml, List<int> lstOpetatorType, int setsndans, string user, int pageIndex, int pagingFlag)
        {
            if (!GetCheckedInformation()) return;
            //this.Record.Clear();
            //Remind = "查询命令已发送...请等待数据反馈！";
            var info = Wlst.Sr.ProtocolPhone.LxSysHttp.wst_sys_operator_record_http;//.wlst_cnt_request_weekset_record;
            info.WstSysOperatorRecord.DtStartTime = new DateTime(dtstarttime.Year, dtstarttime.Month, dtstarttime.Day, 0, 0, 1).Ticks;
            info.WstSysOperatorRecord.DtEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59).Ticks;
            info.WstSysOperatorRecord.RtuId = IsTmlChecked ? tml : 0;

            info.WstSysOperatorRecord.OperatorIds = lstOpetatorType;
            info.WstSysOperatorRecord.UserName = user;
            info.WstSysOperatorRecord.IsClientSnd = setsndans;

            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (data == null) return;
            //SndOrderServer.OrderSnd(info, 10, 6);
            GetAllOperatorTypes1(data, pagingFlag,2);
        }
    }

    /// <summary>
    /// 操作类型模型定义
    /// </summary>
    public class OperatorTypeItem : ObservableObject
    {
        private NameIntBool _key;

        /// <summary>
        /// 
        /// </summary>
        public NameIntBool Key
        {
            #region
            get
            {
                if (_key == null)
                {
                    _key = new NameIntBool();
                    _key.OnIsSelectedChanged += new EventHandler(_key_OnIsSelectedChanged);
                }
                return _key;
            }
            set
            {
                if (_key == value) return;
                try
                {
                    if (_key != null)
                    {
                        _key.OnIsSelectedChanged -= new EventHandler(_key_OnIsSelectedChanged);
                    }
                }
                catch (Exception ex)
                {
                }
                _key = value;
                if (_key != null)
                {
                    _key.OnIsSelectedChanged += new EventHandler(_key_OnIsSelectedChanged);
                }
                RaisePropertyChanged(() => Key);
            }
            #endregion
            //get { return _key; }
            //set
            //{
            //    if (value != _key)
            //    {
            //        _key = value;
            //        RaisePropertyChanged(() => Key);
            //    }
            //}
        }

        void _key_OnIsSelectedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (_key == null) return;
            foreach (var nameIntBool in Value)
            {
                nameIntBool.IsSelected = _key.IsSelected;
            }

            if (Key.IsSelected)
            {
                MaxHeight = 1000;
            }
            else MaxHeight = 0;
        }

        private string _name;
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }


        private int _namsdfsdfe;
        /// <summary>
        /// 
        /// </summary>
        public int  MaxHeight
        {
            get { return _namsdfsdfe; }
            set
            {
                if (value != _namsdfsdfe)
                {
                    _namsdfsdfe = value;
                    RaisePropertyChanged(() => MaxHeight);
                }
            }
        }

        private ObservableCollection<NameIntBool> _value;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<NameIntBool> Value
        {
            get { return _value ?? (_value = new ObservableCollection<NameIntBool>()); }
            set
            {
                if (value == _value) return;
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        private bool _checkall;

        public bool IsSelectedAll
        {
            get { return _checkall; }
            set
            {
                if (value == _checkall) return;
                _checkall = value;
                   foreach(var item in Value )
                    {
                        item.IsSelected = _checkall;
                    }
                
                RaisePropertyChanged(() => IsSelectedAll);
            }
        }

       
    }
}
        #endregion
