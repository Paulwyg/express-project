using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.ComponentHold;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.CoreOne.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Sr.EquipemntLightFault.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultOnTabViewModel.ViewModel;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.Services;
using Wlst.Ux.EquipemntLightFault.Models.Exchange;
using Wlst.Ux.EquipemntLightFault.Services;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryViewModel.ViewModel
{
    [Export(typeof(IIEquipmentFaultRecordQueryViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentFaultRecordQueryViewModel : ObservableObject, IIEquipmentFaultRecordQueryViewModel
    {
         
        public EquipmentFaultRecordQueryViewModel()
        {
            ClickTime = DateTime.Now;
            InitEvent();
            InitAction();
            DtEndTime = DateTime.Now;
            DtStartTime = DateTime.Now.AddDays(-1);
            var shh = new List<int>() { 19, 20, 21, 46, 53,50 };
            for (int i = 1; i < 80; i++)
            {
                if (shh.Contains(i)) continue;
                Sh.Add(i);
            }
            //LoadIsShowThisViewOnNewErrArrive();
        }

        void ntg_OnIsSelectedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            var ggg = sender as OperatorTypeItem;
            if (ggg == null || ggg.Value.Count == 0) return;
            foreach (var g in TypeItems)
            {
                if(g.IsSelected)
                {
                    g.IsShow = true;
                    for (int i = 0; i < g.Value.Count; i++)
                    {
                        g.Value[i].IsShow = true ;
                    }
                }else
                {
                    g.IsShow = false;
                    g.IsSelectedAll = false;
                    for (int i = 0; i < g.Value.Count; i++)
                    {
                        g.Value[i].IsShow = false;
                        g.Value[i].IsSelected = false;
                    }
                }
                
            }
            //if(ggg.IsSelected)
            //{
            //    ggg.IsShow = true;
            //    foreach (var g in ggg.Value)
            //    {
            //        //g.IsSelected = ggg.IsSelected; 全选功能取消
            //        g.IsShow = true;
            //    }
            //}
            
        }
        private void LoadXml()
        {
            var infos = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("SystemCommonSetConfg");
            if (infos.ContainsKey("IsShowArgsInErrInfo"))
            {
                if (Convert.ToInt32(infos["IsShowArgsInErrInfo"]) == 1)
                {
                    ArgsInfoVisi = true ;
                    ArgsInfoVisiE = false ;   
                }else if(Convert.ToInt32(infos["IsShowArgsInErrInfo"]) == 2)
                {
                    ArgsInfoVisi = true;
                    ArgsInfoVisiE = true  ;   
                }else
                {
                    ArgsInfoVisi = false ;
                    ArgsInfoVisiE = false; 
                }

            }
            else
            {
                ArgsInfoVisi = false ;
                ArgsInfoVisiE = false; 
            }
                



        }
        private bool If_ManageInfo_Exist()
        {
            var lst = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();

            if (lst.Count != 0)
            {
                return true;
            }

            return false;
        }

        private bool isgaojiselected = false;
        private List<int> lastselectedinfo = new List<int>();

        /// <summary>
        /// 记录需要呈现电流电压等数据的的故障id
        /// </summary>
        private static List<int> Sh = new List<int>() ;

        public void NavOnLoad(params object[] parsObjects)
        {

            ManageInfoExist = If_ManageInfo_Exist();
            ManageInfoVisi = false;
            ArgsInfoVisi = false;
            ArgsInfoVisiE = false;
            RtuTypeSelected = RtuType[0];

            //lvf 2019年3月27日10:33:20 
            IsShowChkHideLoopErrs = Sr.EquipmentInfoHolding.Services.Others.CityNum == 7;

            System.TimeSpan ts = DateTime.Now -
                                 Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.ReqExistFaultsTime;
            if (ts.Minutes > 5)
            {
                RequestTmlExistFaultsInfos();
            }

          
            LoadXml();
            //是否呈现删除按钮,管理员权限并且勾选选项 才呈现
            CmdDeleteVisi = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D && Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 1, false) ? Visibility.Visible : Visibility.Collapsed;
            
            var lsthase = new List<int>();
            foreach (var f in EquipmentModelComponentHolding.DicEquipmentModels.Values)
            {
                lsthase.Add(f.ModelKey);
               
            }
            lsthase.Add(0000); //自定义故障 lvf

            CountPreErrs = false  ;

            IsCalcFault = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 2, false);

            IsShowCalcFaultDetail = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(3102, 3, false);


            _isvmsettime = true;
            this.TimeLong = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetTimeAlarmLong;
            _isvmsettime = false;
            this.IsShowErrsCal = Sr.EquipmentInfoHolding.Services.Others.IsShowErrsCal;


            //throw new NotImplementedException();
            isgaojiselected = IsAdvancedQueryChecked;
            // foreach (var t in TypeItems )if(t.IsSelected )lastselectedinfo .Add( t.Value )
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮

            //直接初始化故障类型
            //var count = 0;
            //foreach (var g in TypeItems) count += g.Value.Count;
            //if (count < 5)
            //{
                TypeItems.Clear();

                //var ntg = new NameIntBoolXg {Name = "所有报警", Value = 0, IsSelected = true};
                //ntg.OnIsSelectedChanged += new EventHandler(ntg_OnIsSelectedChanged);
                //ntg.IsMonitor = true;

                //  FaultType.Add(ntg);

                var lst = new Dictionary<Tuple<int, int, int, string>, List<Tuple<int, string>>>();

                foreach (var g in Sr.EquipemntLightFault.Services.FaultClassisDef.FaultClass)
                {
                    var t = g;
                    if (!lsthase.Contains(g.Item1)) continue;
                    if (lst.ContainsKey(g)) continue;
                    if (g.Item4 != "自定义故障")
                    {
                        t =new Tuple<int, int, int, string>(g.Item1,g.Item2,g.Item3,"  "+g.Item4+"  ");
                    }
                    else
                    {
                        t = new Tuple<int, int, int, string>(g.Item1, g.Item2, g.Item3, g.Item4);
                    }
                    lst.Add(t, new List<Tuple<int, string>>());

                }

                foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
                {
                    if (!t.Value.IsEnable) continue;
                    foreach (var f in lst)
                    {
                        if (t.Key >= f.Key.Item2 && t.Key <= f.Key.Item3)
                        {
                            if (!f.Value.Contains(new Tuple<int, string>(t.Key, t.Value.FaultNameByDefine)))
                                f.Value.Add(new Tuple<int, string>(t.Key, t.Value.FaultNameByDefine));
                        }
                    }
                }


                foreach (var g in lst)
                {

                    var gnt = new OperatorTypeItem()
                                  {
                                      IsSelected = false,
                                      Name = g.Key.Item4,
                                      Value = new ObservableCollection<NameIntBool>(),
                                      IsShow =false ,
                                  };
                    foreach (var f in g.Value)
                    {
                        gnt.Value.Add(new NameIntBool() { IsSelected = false, Name = f.Item2, Value = f.Item1 ,IsShow = false });
                    }
                    if (gnt.Value.Count > 0)
                    {
                        gnt.OnIsSelectedChanged += new EventHandler(ntg_OnIsSelectedChanged);
                        TypeItems.Add(gnt);
                    }
                }

            //}
            // if (FaultType.Count > 0) CurrentSelectType = FaultType[0];
            //if (_dtEndTime == new DateTime() || _dtStartTime == new DateTime())
            //{
            //        DtEndTime = DateTime.Now;
            //        DtStartTime = DateTime.Now.AddDays(-1);
            //}

            _dtQuery = DateTime.Now.AddDays(-1);
            IsSingleEquipmentQuery = false  ;
            IsOldFaultQuery = false ;


            GetAllFaultName();



            //添加区域  lvf 2018年6月15日09:40:49  
            AreaName.Clear();
            getAreaRId();
            if (AreaName.Count > 0)
            {
                if (AreaComboBoxSelected == null) AreaComboBoxSelected = AreaName.First();
            }
            AreaVisi = Visibility.Collapsed;
            if (AreaName.Count > 1) AreaVisi = Visibility.Visible;
            QueryMode = 3;



            int rutid = 0;
            SelectedRtus.Clear();


            var ids = parsObjects[0] as List<int>;

            if (ids != null && ids.Count > 1)
            {

                IsAdvancedQueryChecked = true;
                IsSingleEquipmentQuery = true;
                var rtus = (from t in ids
                            where
                                t < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuEnd &&
                                t > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart
                            select t).ToList();
                SelectedRtus.Clear();
                SelectedRtus = rtus;

                QueryMode = 2;
                if (rtus.Count == 0)
                {
                    this.SelectedRtus.Clear();
                    RtuId = 0;
                    RtuName = "通过终端树勾选终端进行故障查询.";
                }
                else
                {
                    RtuId = SelectedRtus[0];
                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (RtuId))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [RtuId];
                    RtuName = tml.RtuName;
                    if (SelectedRtus.Count > 1)
                        RtuName += " [等" + SelectedRtus.Count + "个终端]";
                    Ex();
                }

            }
            else
            {
                SelectedRtus.Clear();
                try
                {
                    rutid = Convert.ToInt32(parsObjects[0]);

                }
                catch (Exception ex)
                {
                }
                IsNewAllQuery = true;
                if (rutid > 0)
                {
                    SelectedRtus.Add(rutid);
                    RtuId = rutid;
                    QueryMode = 2;
                    this.IsAdvancedQueryChecked = true;
                    this.IsSingleEquipmentQuery = true;
                    //IsNewAllQuery = false;

                    this.Ex();
                }


                if (rutid == -1)
                {
                    //  this.IsAdvancedQueryChecked = true;
                    if (_thisViewActive) return;
                    this.Ex();//todo

                }
                if (rutid == -2)
                {
                    ClickTime = DateTime.Now;
                    if (_thisViewActive) return;
                    this.Ex();//todo
                }
            }
            _thisViewActive = true;
            var type = parsObjects[0] as string;
            if (type == null) return;
            var typeid = new List<int>();
            foreach (var t in TypeItems)
            {
                foreach (var f in t.Value)
                {
                    if (type == f.Name.Trim())
                        typeid.Add(f.Value);
                }
            }
            QueryNewErrorAllRtuFault(typeid, false);
        }

        //lvf 2018年8月9日15:49:59  请求最新故障
        private void RequestTmlExistFaultsInfos()
        {
            var info = Wlst.Sr.ProtocolPhone.LxFault.wlst_fault_curr;
            info.WstFaultCurr.Op = 1;
            SndOrderServer.OrderSnd(info, 10, 6);

            //LogInfo.Log("正在请求终端所有故障信息!!!");
            LogInfo.Log("登录信息同步中... 请求故障");

            var info1 = Wlst.Sr.ProtocolPhone.LxSpe.wst_spe_special_rtus;//.wlst_fault_curr;
            info.Args.Cid = 1;
            SndOrderServer.OrderSnd(info1, 10, 6);

            //记录请求最新故障时间 lvf 2018年8月9日15:45:39
            Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.ReqExistFaultsTime = DateTime.Now;

        }

        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            //Records = new ObservableCollection<EquipmentFaultViewModel>();
            //ExportVisi = Visibility.Collapsed;
            //if (Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Count
            //    > 0)
            //    ClickTime =
            //        (from t in
            //             Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.
            //             Values
            //         orderby t.DateCreate descending
            //         select t).ToList()[0].DateCreate;
            //else ClickTime = DateTime.Now;

            //var argss = new PublishEventArgs()
            //                {
            //                    EventType = PublishEventType.Core,
            //                    EventId = EventIdAssign.PushErrNums
            //                };
            //argss.AddParams(0);
            //EventPublish.PublishEvent(argss);
            //_dtEndTime = this.DtEndTime;
            //_dtStartTime = this.DtStartTime;
            this.Records.Clear();
            this.Remind = "";
            ItemCount = 0;
            PageTotal = "";
        }

        #region tab
        public int Index
        {
            get { return 1; }
        }
        public bool CanClose
        {
            get { return true; }
        }

        public bool CanDockInDocumentHost
        {
            get { return true; }
        }

        public bool CanFloat
        {
            get { return true; }
        }

        public bool CanUserPin
        {
            get { return true; }
        }

        public string Title
        {
            get { return "故障查询"; }
        }

        public DateTime ClickTime { get; set; }

        #endregion

    }


    /// <summary>
    /// 打印及打印预览
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {

        //wyg  打印预览
        #region CmdPrintPriview
        private ICommand _cmdPrintPriview;
        public ICommand CmdPrintPriview
        {
            get
            {
                if (_cmdPrintPriview == null)
                    _cmdPrintPriview = new RelayCommand(ExCmdPrintPriview, CanExPrintPriview, false);
                return _cmdPrintPriview;
            }
        }

        private void ExCmdPrintPriview()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("地址");
                tabletitle.Add("名称");
                tabletitle.Add("故障回路");
                tabletitle.Add("故障名称");
                tabletitle.Add("发生时间");
                if (IsOldFaultQuery) tabletitle.Add("消除时间");
                tabletitle.Add("备注");
                var table = new List<List<string>>();
                DateTime createtime;
                DateTime removetime;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                foreach (var g in Records)
                {
                    createtime = Convert.ToDateTime(g.DtCreateTime, dtFormat);
                    var tem = new List<string>();
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.RtuLoopName);
                    tem.Add(g.FaultName);
                    tem.Add(createtime.ToString("MM/dd HH:mm:ss"));
                    if (IsOldFaultQuery)
                    {
                        removetime = Convert.ToDateTime(g.DtRemoceTime, dtFormat);
                        tem.Add(removetime.ToString("MM/dd HH:mm:ss"));
                    }
                    tem.Add(g.Remark);
                    table.Add(tem);
                }
                print.Prints.PrintPriview(tabletitle, table, false, "故障统计表", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrintPriview()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }


        #endregion

        //打印
        #region CmdPrint
        private ICommand _cmdPrint;
        public ICommand CmdPrint
        {
            get
            {
                if (_cmdPrint == null)
                    _cmdPrint = new RelayCommand(ExCmdPrint, CanExPrint, false);
                return _cmdPrint;
            }
        }

        private void ExCmdPrint()
        {
            _dtCmdExport = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("地址");
                tabletitle.Add("名称");
                tabletitle.Add("故障回路");
                tabletitle.Add("故障名称");
                tabletitle.Add("发生时间");
                if (IsOldFaultQuery) tabletitle.Add("消除时间");
                tabletitle.Add("备注");
                var table = new List<List<string>>();
                DateTime createtime;
                DateTime removetime;
                DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
                foreach (var g in Records)
                {
                    createtime = Convert.ToDateTime(g.DtCreateTime, dtFormat);
                    var tem = new List<string>();
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.RtuLoopName);
                    tem.Add(g.FaultName);
                    tem.Add(createtime.ToString("MM/dd HH:mm:ss"));
                    if (IsOldFaultQuery)
                    {
                        removetime = Convert.ToDateTime(g.DtRemoceTime, dtFormat);
                        tem.Add(removetime.ToString("MM/dd HH:mm:ss"));
                    }
                    tem.Add(g.Remark);
                    table.Add(tem);
                }
                print.Prints.Print(tabletitle, table, false, "故障统计表", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrint()
        {
            if (Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
        }
        #endregion
    }








     public partial class EquipmentFaultRecordQueryViewModel
     {
         //多终端选中 协议
         public List<int> SelectedRtus = new List<int>();
     }
    /// <summary>
    /// Field ,Attri, ICommand ,Methods
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {

        #region Field

        private static bool _isOnExport = true;

        #endregion

        #region Attri

        #region ExportVisi

        private Visibility _exportVisi = Visibility.Collapsed;

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

        #region IsAdvancedQueryChecked

        private bool _isAdvancedQueryChecked;

        public bool IsAdvancedQueryChecked
        {
            get { return _isAdvancedQueryChecked; }
            set
            {
                if (_isAdvancedQueryChecked != value)
                {
                    _isAdvancedQueryChecked = value;

                    if (!_isAdvancedQueryChecked)
                    {
                        IsSingleEquipmentQuery = false;
                        //foreach (var nameIntBool in TypeItems )
                        //{
                        //    nameIntBool.IsSelected = nameIntBool.Value == 0;
                        //}

                    }
                    RaisePropertyChanged(() => IsAdvancedQueryChecked);

                    if (lastCount == 0) lastCount = DateTime.Now.Ticks;
                    if (DateTime.Now.Ticks - lastCount < 10000000) counxxxxx++;
                    else counxxxxx = 0;
                    if(counxxxxx >5)
                    {
                        CounterLableDoubleClick = 5;
                    }
                    lastCount = DateTime.Now.Ticks;
                }
            }
        }

        private int counxxxxx = 0;
        private long lastCount = 0;

        #endregion

        #region RtuName

        private string _rtuName;

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

        #region RtuId

        private int _iphyd;

        public int PhyId
        {
            get { return _iphyd; }
            set
            {
                if (_iphyd != value)
                {
                    _iphyd = value;
                    this.RaisePropertyChanged(() => this.PhyId);
                }
            }
        }

        private int _rtuid;

        public int RtuId
        {
            get { return _rtuid; }
            set
            {

                if (value != _rtuid)
                {
                    _rtuid = value;
                    PhyId = value;
                    RaisePropertyChanged(() => RtuId);
                    RtuName = "";
                    if (
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (_rtuid))
                    {
                        var tml =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                                [_rtuid];
                        RtuName = tml.RtuName;

                        if (tml.RtuFid == 0)
                            PhyId = tml.RtuPhyId;
                        else PhyId = value;
                    }
                    if (RtuId>1700000 && RtuId<1800000  )
                    {
                        var nametmp = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(RtuId);
                        if (nametmp == null) return;
                        RtuName = nametmp.FieldName;
                        PhyId = nametmp.PhyId;

                    }

                }
            }
        }

        #endregion

        #region Records

        private ObservableCollection<EquipmentFaultViewModel> _record;

        public ObservableCollection<EquipmentFaultViewModel> Records
        {
            get { return _record ?? (_record = new ObservableCollection<EquipmentFaultViewModel>()); }
            set
            {
                if (_record != value)
                {
                    _record = value;
                    this.RaisePropertyChanged(() => this.Records);
                }
            }
        }

        #endregion

        #region Recordss

        private ObservableCollection<EquipmentFaultViewModel> _records;

        public ObservableCollection<EquipmentFaultViewModel> Recordss
        {
            get { return _records ?? (_records = new ObservableCollection<EquipmentFaultViewModel>()); }
            set
            {
                if (_records != value)
                {
                    _records = value;
                    this.RaisePropertyChanged(() => this.Recordss);
                }
            }
        }

        #endregion

        #region ManageInfoVisi

        private bool _ManageInfoVisi = false;

        public bool ManageInfoVisi
        {
            get { return _ManageInfoVisi; }
            set
            {
                if (value == _ManageInfoVisi) return;
                _ManageInfoVisi = value;
                RaisePropertyChanged(() => ManageInfoVisi);
            }
        }

        #endregion

        #region ArgsInfoVisi

        private bool _argsInfoVisi = false ;

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
        private bool _argsInfoVisiE = false;

        public bool ArgsInfoVisiE
        {
            get { return _argsInfoVisiE; }
            set
            {
                if (value == _argsInfoVisiE) return;
                _argsInfoVisiE = value;
                RaisePropertyChanged(() => ArgsInfoVisiE);
            }
        }

        #endregion

        #region ManageInfoExist

        private bool _ManageInfoExist = false;

        public bool ManageInfoExist
        {
            get { return _ManageInfoExist; }
            set
            {
                if (value == _ManageInfoExist) return;
                _ManageInfoExist = value;
                RaisePropertyChanged(() => ManageInfoExist);
            }
        }

        #endregion

        #region CurrentSelectRecord

        private EquipmentFaultViewModel _currentSelectRecord;

        public EquipmentFaultViewModel CurrentSelectRecord
        {
            get { return _currentSelectRecord ?? (_currentSelectRecord = new EquipmentFaultViewModel()); }
            set
            {
                if (_currentSelectRecord == value) return;
                _currentSelectRecord = value;
                RaisePropertyChanged(() => CurrentSelectRecord);

                if (CurrentSelectRecord == null || CurrentSelectRecord.ChildItems == null)
                { CurrentSelectDataInfo = new ObservableCollection<EquipmentFaultViewModel>(); return; }

                if (IsShowCalcFaultDetail)
                {
                    CurrentSelectDataInfo = new ObservableCollection<EquipmentFaultViewModel>();

                    var tmp = (from t in CurrentSelectRecord.ChildItems orderby t.DtCreateTime select t).ToList();

                    foreach (var g in tmp)
                    {
                        CurrentSelectDataInfo.Add(g);
                    }
                }

            }
        }

        #endregion

        #region CurrentSelectDataInfo

        private ObservableCollection<EquipmentFaultViewModel> _currentSelectDataInfo;

        public ObservableCollection<EquipmentFaultViewModel> CurrentSelectDataInfo
        {
            get { return _currentSelectDataInfo; }
            set
            {
                if (value != _currentSelectDataInfo)
                {
                    _currentSelectDataInfo = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectDataInfo);
                }
            }
        }

        #endregion


        #region DetailViewHeight
        private int _viewheight;

        /// <summary>
        /// 表格高度
        /// </summary>
        public int ViewHeight
        {
            get { return _viewheight; }
            set
            {
                if (_viewheight != value)
                {
                    _viewheight = value;
                    this.RaisePropertyChanged(() => this.ViewHeight);
                }
            }
        }
        #endregion

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _timeItems = null;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> TimeItems
        {
            get
            {
                if (_timeItems == null)
                {
                    _timeItems = new ObservableCollection<NameValueInt>();
                    //for (int i = 1; i < 24; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() { Name = i + "小时", Value = i });
                    //}
                    //for (int i = 1; i < 94; i++)
                    //{
                    //    _timeItems.Add(new NameValueInt() { Name = i + " 天", Value = i * 24 });
                    //}
                    _timeItems.Add(new NameValueInt() { Name = "1小时", Value = 1 });
                    _timeItems.Add(new NameValueInt() { Name = "3小时", Value = 3 });
                    _timeItems.Add(new NameValueInt() { Name = "6小时", Value = 6 });
                    _timeItems.Add(new NameValueInt() { Name = "12小时", Value = 12 });

                    _timeItems.Add(new NameValueInt() { Name = "1天", Value = 1 * 24 });
                    _timeItems.Add(new NameValueInt() { Name = "3天", Value = 3 * 24 });
                    _timeItems.Add(new NameValueInt() { Name = "7天", Value = 7 * 24 });
                    _timeItems.Add(new NameValueInt() { Name = "14天", Value = 14 * 24 });
                    _timeItems.Add(new NameValueInt() { Name = "30天", Value = 30 * 24 });
                    _timeItems.Add(new NameValueInt() { Name = "60天", Value = 60 * 24 });
                    _timeItems.Add(new NameValueInt() { Name = "90天", Value = 90 * 24 });
                }
                return _timeItems;
            }
        }

        private NameValueInt _currenttime;

        /// <summary>
        /// 时间统计有效时间  时间为小时 大于等于1 小于 1440 
        /// </summary>
        public NameValueInt CurrentSelectedTime
        {
            get { return _currenttime; }
            set
            {
                if (_currenttime == value) return;
                _currenttime = value;
                this.RaisePropertyChanged(() => this.CurrentSelectedTime);
                if (value != null)
                {
                    TimeLong = value.Value;
                }

            }
        }
        private bool _isvmsettime;
        private int _time;

        /// <summary>
        /// 时间统计有效时间  时间为小时 大于等于1 小于 1440 
        /// </summary>
        public int TimeLong
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    if (value < 1) value = 1;
                    if (value > 1440) value = 1440;
                    _time = value;
                    this.RaisePropertyChanged(() => this.TimeLong);

                    if (_isvmsettime)
                    {
                        bool find = false;
                        foreach (var t in this.TimeItems)
                        {
                            if (t.Value == value)
                            {
                                CurrentSelectedTime = t;
                                find = true;
                                break;
                            }
                        }
                        if (find == false)
                        {
                            foreach (var t in this.TimeItems)
                            {
                                if (t.Value > value)
                                {
                                    CurrentSelectedTime = t;
                                    find = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        #region FaultType

        //private ObservableCollection<NameIntBoolXg> _fauleType;

        //public ObservableCollection<NameIntBoolXg> FaultType
        //{
        //    get { return _fauleType ?? (_fauleType = new ObservableCollection<NameIntBoolXg>()); }
        //}


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

        #region IsSingleEquipmentQuery

        private bool _isSingleEquipmentQuery;

        /// <summary>
        /// 是否为单个终端故障查询
        /// </summary>
        public bool IsSingleEquipmentQuery
        {
            get { return _isSingleEquipmentQuery; }
            set
            {
                if (_isSingleEquipmentQuery != value)
                {
                    _isSingleEquipmentQuery = value;
                    RaisePropertyChanged(() => IsSingleEquipmentQuery);

                    if (IsSingleEquipmentQuery && Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId != 0)
                    {
                        RtuId = Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId;
                        SelectedRtus.Add(RtuId);  
                    }
                       
                }
            }
        }

        #endregion

        #region IsOldFaultQuery

        private bool _isOldFaultQuery;

        /// <summary>
        /// 是否为新故障查询，是否就为历史故障查询
        /// </summary>
        public bool IsOldFaultQuery
        {
            get { return _isOldFaultQuery; }
            set
            {
                if (_isOldFaultQuery == value) return;
                _isOldFaultQuery = value;
                RaisePropertyChanged(() => IsOldFaultQuery);

                if (IsOldFaultQuery == false)
                {
                    ViewHeight = 0;
                    PagerVisi = Visibility.Collapsed;
                }else if(IsShowCalcFaultDetail)
                {
                    ViewHeight = 200;
                }
                if (IsOldFaultQuery)
                {
                    PagerVisi = middleVisi;
                }
                OnStartTimeEnableChange();
            }
        }

        private bool _isCountPreErrs;
        /// <summary>
        /// 是否统计历史故障 sz
        /// </summary>
        public bool CountPreErrs
        {
            get { return _isCountPreErrs; }
            set
            {
                if (_isCountPreErrs == value) return;
                _isCountPreErrs = value;
                RaisePropertyChanged(() => CountPreErrs);
            }
        }

        private bool _isCalcFault;
        /// <summary>
        /// 是否统计历史故障 通用版 lvf  2018年5月9日09:45:31
        /// </summary>
        public bool IsCalcFault
        {
            get { return _isCalcFault; }
            set
            {
                if (_isCalcFault == value) return;
                _isCalcFault = value;
                RaisePropertyChanged(() => IsCalcFault);
            }
        }


        private bool _isShowChkHideLoopErrs;
        /// <summary>
        /// 苏州新区 需要屏蔽晚上回路报警 谁吃得消  lvf 2019年3月27日10:35:30   一塌糊涂 是否呈现单选框
        /// </summary>
        public bool IsShowChkHideLoopErrs
        {
            get { return _isShowChkHideLoopErrs; }
            set
            {
                if (_isShowChkHideLoopErrs == value) return;
                _isShowChkHideLoopErrs = value;
                RaisePropertyChanged(() => IsShowChkHideLoopErrs);
            }
        }


        private bool _isHideLoopErrInNight;
        /// <summary>
        /// 苏州新区 需要屏蔽晚上回路报警 谁吃得消  lvf 2019年3月27日10:35:30   一塌糊涂
        /// </summary>
        public bool IsHideLoopErrInNight
        {
            get { return _isHideLoopErrInNight; }
            set
            {
                if (_isHideLoopErrInNight == value) return;
                _isHideLoopErrInNight = value;
                RaisePropertyChanged(() => IsHideLoopErrInNight);
            }
        }

        private bool _isShowCalcFaultDetail;

        /// <summary>
        /// 是否显示统计历史故障详细信息 通用版 lvf  2018年5月9日09:45:31
        /// </summary>
        public bool IsShowCalcFaultDetail
        {
            get { return _isShowCalcFaultDetail; }
            set
            {

                if (_isShowCalcFaultDetail == value) return;
                _isShowCalcFaultDetail = value;
                RaisePropertyChanged(() => IsShowCalcFaultDetail);
                if (_isShowCalcFaultDetail)
                    ViewHeight = 200;
                else
                    ViewHeight = 0;
            }
        }

        private bool _isLastPreErrs;   //todo
        /// <summary>
        /// 是否是最后一条
        /// </summary>
        public bool CountLastPreErrs
        {
            get { return _isLastPreErrs; }
            set
            {
                if (_isLastPreErrs == value) return;
                _isLastPreErrs = value;
                RaisePropertyChanged(() => CountLastPreErrs);
            }
        }

        private bool _isCountErrs;
        /// <summary>
        /// 是否统计故障
        /// </summary>
        public bool CountErrs
        {
            get { return _isCountErrs; }
            set
            {
                if (_isCountErrs == value) return;
                _isCountErrs = value;
                RaisePropertyChanged(() => CountErrs);
            }
        }

        private bool _isCountNewErrs;
        /// <summary>
        /// 是否统计现存故障
        /// </summary>
        public bool CountNewErrs
        {
            get { return _isCountNewErrs; }
            set
            {
                if (_isCountNewErrs == value) return;
                _isCountNewErrs = value;
                RaisePropertyChanged(() => CountNewErrs);
            }
        }

        private bool _isOlIsNewAllQuerydFaultQuery;

        /// <summary>
        /// 
        /// </summary>
        public bool IsNewAllQuery
        {
            get { return _isOlIsNewAllQuerydFaultQuery; }
            set
            {
                if (_isOlIsNewAllQuerydFaultQuery == value) return;
                _isOlIsNewAllQuerydFaultQuery = value;
                RaisePropertyChanged(() => IsNewAllQuery);
                OnStartTimeEnableChange();
            }
        }


        private bool _isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFaultQueryTimeStartEnable
        {
            get { return _isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery; }
            set
            {
                if (_isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery == value) return;
                _isOlIsNewIsFaultQueryTimeStartEnableAllQuerydFaultQuery = value;
                RaisePropertyChanged(() => IsFaultQueryTimeStartEnable);
            }
        }



        void OnStartTimeEnableChange()
        {
            if (IsOldFaultQuery) IsFaultQueryTimeStartEnable = true;
            else
            {
                if (IsNewAllQuery) IsFaultQueryTimeStartEnable = false;
                else IsFaultQueryTimeStartEnable = true;
            }
        }

        #endregion

        #region DtEndTime

        private DateTime _dtEndTime;

        /// <summary>
        /// 故障查询结束时间
        /// </summary>
        public DateTime DtEndTime
        {
            get { return _dtEndTime; }
            set
            {
                if (_dtEndTime != value)
                {
                    _dtEndTime = value;
                    
                    RaisePropertyChanged(() => DtEndTime);
                }
            }
        }

        #endregion

        #region DtStartTime

        private DateTime _dtStartTime;

        /// <summary>
        /// 故障查询起始时间
        /// </summary>
        public DateTime DtStartTime
        {
            get { return _dtStartTime; }
            set
            {
                if (_dtStartTime != value)
                {
                    _dtStartTime = value;
                    RaisePropertyChanged(() => DtStartTime);
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
                if (value == _remind) return;
                _remind = value;
                RaisePropertyChanged(() => Remind);
            }
        }

        #endregion

        #region IsShowErrsCal

        private bool _isShowErrsCal;

        /// <summary>
        /// 是否呈现故障统计功能 
        /// </summary>
        public bool IsShowErrsCal
        {
            get { return _isShowErrsCal; }
            set
            {
                if (_isShowErrsCal == value) return;
                _isShowErrsCal = value;
                RaisePropertyChanged(() => IsShowErrsCal);
            }
        }

        #endregion


        #region  QueryMode 
        private int _queryMode;

        /// <summary>
        /// 查询模式   1：全部设备   2：当前设备   3：区域查询  lvf 2018年6月15日09:37:17
        /// </summary>
        public int QueryMode
        {
            get { return _queryMode; }
            set
            {
                if (value != _queryMode)
                {
                    _queryMode = value;
                    this.RaisePropertyChanged(() => this.QueryMode);
                    if (QueryMode == 2)
                    {
                        IsSingleEquipmentQuery = true;
                        //如果是单个终端查询，这无视终端类型的判断。
                        RtuTypeSelected = RtuType[0];
                    }
                    else
                    {
                        IsSingleEquipmentQuery = false;
                    }
                }
            }
        }

        #endregion

        #region AreaId

        public void getAreaRId()
        {
            AreaName.Clear();
            AreaName.Add(new AreaInt() { Value = "全部", Key = -1 });
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                {
                    string area = t.Value.AreaName;
                    AreaName.Add(new AreaInt() { Value = t.Value.AreaId.ToString("d2") + "-" + area, Key = t.Value.AreaId });
                }
            }
            else
            {
                foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                {
                    if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                    {
                        string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                        AreaName.Add(new AreaInt() { Value = t.ToString("d2") + "-" + area, Key = t });
                    }
                }
            }


        }


        private static ObservableCollection<AreaInt> _devices;

        public static ObservableCollection<AreaInt> AreaName
        {
            get
            {
                if (_devices == null)
                {
                    _devices = new ObservableCollection<AreaInt>();
                }
                return _devices;
            }

        }

        public class AreaInt : Wlst.Cr.Core.CoreServices.ObservableObject
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

        private AreaInt _areacomboboxselected;
        private int AreaId;

        public AreaInt AreaComboBoxSelected
        {
            get { return _areacomboboxselected; }
            set
            {
                if (_areacomboboxselected != value)
                {
                    _areacomboboxselected = value;
                    this.RaisePropertyChanged(() => this.AreaComboBoxSelected);
                    if (value == null) return;
                    AreaId = value.Key;

                    GetGrpIdByAreaId();
                }
            }
        }



        private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility AreaVisi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.AreaVisi);
                }
            }
        }




        #endregion


        #region  Group

        private Visibility _txtgrpVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility GrpVisi
        {
            get { return _txtgrpVisi; }
            set
            {
                if (value != _txtgrpVisi)
                {
                    _txtgrpVisi = value;
                    this.RaisePropertyChanged(() => this.GrpVisi);
                }
            }
        }
        private static ObservableCollection<GroupInt> _grpdevices;

        public static ObservableCollection<GroupInt> GroupName
        {
            get
            {
                if (_grpdevices == null)
                {
                    _grpdevices = new ObservableCollection<GroupInt>();
                }
                return _grpdevices;
            }

        }

        public class GroupInt : Wlst.Cr.Core.CoreServices.ObservableObject
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

        private GroupInt _grpcomboboxselected;
        private int GrpId;

        public GroupInt GroupComboBoxSelected
        {
            get { return _grpcomboboxselected; }
            set
            {
                if (_grpcomboboxselected != value)
                {
                    _grpcomboboxselected = value;
                    this.RaisePropertyChanged(() => this.GroupComboBoxSelected);
                    if (value == null) return;
                    GrpId = value.Key;
                }
            }
        }

        public void GetGrpIdByAreaId()
        {
            GroupName.Clear();

            if (AreaId == -1)//全部区域
            {
                GrpVisi = Visibility.Collapsed;

            }
            else
            {
                GrpVisi = Visibility.Visible;
                var area = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(AreaId);
                if (area == null) return;
                var grps =
                    Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GrpInfoList(AreaId);
                GroupName.Add(new GroupInt() { Value = "全部", Key = -1 });
                if (grps.Count > 0)
                {
                    var grpsTmp = (from t in grps orderby t.GroupId select t).ToList();
                    foreach (var f in grpsTmp)
                    {

                        GroupName.Add(new GroupInt() { Value = f.GroupName, Key = f.GroupId });

                    }


                }
                GroupComboBoxSelected = GroupName[0];
            }



        }



        #endregion


        #region RtuType

        private NameValueInt _rtuTypeselected;
        //private int intRtuType;

        public NameValueInt RtuTypeSelected
        {
            get { return _rtuTypeselected; }
            set
            {
                if (_rtuTypeselected != value)
                {
                    _rtuTypeselected = value;
                    this.RaisePropertyChanged(() => this.RtuTypeSelected);
                    //if (value == null) return;
                    //intRtuType = value.Value;


                }
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _rtuType;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> RtuType
        {
            get
            {
                if (_rtuType == null)
                {
                    _rtuType = new ObservableCollection<NameValueInt>();

                    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType.Count != 0)
                    {
                        _rtuType.Add(new NameValueInt() {Name = "所有", Value = -1});
                        foreach (var g in Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuType)
                        {
                            _rtuType.Add(new NameValueInt() {Name = g.Value.Item1, Value = g.Key});
                        }
                        return _rtuType;
                    }

                    _rtuType.Add(new NameValueInt() {Name = "所有", Value = -1});
                    _rtuType.Add(new NameValueInt() {Name = "路灯", Value = 1});
                    _rtuType.Add(new NameValueInt() {Name = "亮化", Value = 2});
                    _rtuType.Add(new NameValueInt() {Name = "广告", Value = 3});
                    _rtuType.Add(new NameValueInt() {Name = "其他", Value = 4});



                }
                return _rtuType;
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
                    Records.Clear();
                    Query(value, 1);
                    //RequestHttpData(BeginDate, EndDate, RtuId, _lstOpetatorType, _selectedsetsndans, _selecteduser,
                    //                PageIndex, 1);

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

        #region   RequestInfo
        private mobile.MsgWithMobile _requestInfo;

        /// <summary>
        /// 请求回来的数据
        /// </summary>
        public mobile.MsgWithMobile RequestInfo
        {
            get { return _requestInfo; }
            set
            {
                if (value != _requestInfo)
                {
                    _requestInfo = value;
                    this.RaisePropertyChanged(() => this.RequestInfo);
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region ICommand

        #region CmdQuery

        private DateTime _dtQuery;

        public ICommand CmdQuery
        {
            get { return new RelayCommand(Ex, CanEx, true); }
        }

        private List<int> GetSelectFaultType()
        {
            var ntg = new List<int>();

            foreach (var g in TypeItems)
            {
                foreach (var f in g.Value)
                {
                    if (f.IsSelected && !ntg.Contains(f.Value)) ntg.Add(f.Value);
                }
            }
            return ntg;
        }

        private List<int> GetAllFaultType()
        {
            var ntg = new List<int>();

            foreach (var g in TypeItems)
            {
                foreach (var f in g.Value)
                {
                   ntg.Add(f.Value);
                }
            }
            return ntg;
        }
        private void Ex()
        {
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;
            Records.Clear();
            if (!GetCheckedInformation()) return;
            this.Records.Clear();
            CountPreErrs = false  ;
            CountNewErrs = false;
            CountErrs = false;
            //ArgsInfoVisi = Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowArgsInErrInfo;
            PageIndex = 0;
            Query(PageIndex, 0);

            

            ManageInfoVisi = ManageInfoExist & (!IsOldFaultQuery) & EquipemntLightFaultSetting.IsShowCQJandDGGH;

            if (ManageInfoVisi == true)
            {
                Write_ManageInfo_To_Records();
            }

            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private void Query(int pageIndex,int pagingFlag)
        {
            var tmptype = GetSelectFaultType();
            if (IsSingleEquipmentQuery) //单个终端查询 IsSingleEquipmentQuery
            {
                if (RtuId == 0)
                {
                    UMessageBox.Show("提醒", "未选择终端！", UMessageBoxButton.Ok);
                    return;
                }
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
                if (!IsOldFaultQuery) //新故障查询
                {
                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryNewErrorSingleFault(RtuId, false);
                    }
                    else //单个故障
                    {
                        // if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryNewErrorSingleFault(RtuId, tmptype, false);
                    }
                }
                else
                {

                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryPreErrorSingleFault(RtuId, pageIndex, pagingFlag);
                    }
                    else //单个故障
                    {
                        //  if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryPreErrorSingleFault(RtuId, tmptype, pageIndex, pagingFlag);
                    }
                }
            }
            else   //全部查询  or 区域查询
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";

                if (!IsOldFaultQuery) //新故障查询
                {
                    //  if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        //QueryNewErrorAllRtuFault();
                        ResolveNewErrorAllRtuFault(false);
                    }
                    else //单个故障
                    {
                        //if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryNewErrorAllRtuFault(tmptype, false);
                    }
                }
                else
                {

                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryPreErrorAllRtuFault(pageIndex, pagingFlag);
                    }
                    else //单个故障
                    {
                        //if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryPreErrorAllRtuFault(tmptype, pageIndex, pagingFlag);
                    }
                }
            }

            

            ManageInfoVisi = ManageInfoExist & (!IsOldFaultQuery) & EquipemntLightFaultSetting.IsShowCQJandDGGH;

            if (ManageInfoVisi == true)
            {
                Write_ManageInfo_To_Records();
            }

            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }


        private DateTime _dtPreQueryStartTime;
        private DateTime _dtPreQueryEndTime;

        private bool CanEx()
        {
            if (IsOldFaultQuery)
            {
                if (DtStartTime > DtEndTime) return false;
            }
            else return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                DateTime.Now.Ticks - _dtQuery.Ticks > 30000000 && DateTime.Now.Ticks - _dtOneAfter.Ticks > 300000 &&
                DateTime.Now.Ticks - _dtOneBefore.Ticks > 300000;

            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                 DateTime.Now.Ticks - _dtQuery.Ticks > 3000000 && DateTime.Now.Ticks - _dtOneAfter.Ticks > 300000 &&
                 DateTime.Now.Ticks - _dtOneBefore.Ticks > 300000;
        }

        #endregion

        #region CmdCountNow

        private DateTime _dtCountNow;

        public ICommand CmdCountNow
        {
            get { return new RelayCommand(ExCountNow, CanExCountNow, true); }
        }

        public List<EquipmentFaultCurr.OneFaultItem> FaultItemsTemp =new List<EquipmentFaultCurr.OneFaultItem>(); 
        private void ExCountNow()
        {
            CountPreErrs = false;
            CountNewErrs = true;
            CountErrs = true;
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            Records.Clear();
            Recordss.Clear();
            var tmptype = GetSelectFaultType();
            if (!GetCheckedInformation()) return;

            this.Recordss.Clear();
            if (this.TimeLong == Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetTimeAlarmLong)
            {
                CountNewErrs = false;
                if (IsSingleEquipmentQuery) //单个终端查询
                {
                    if (tmptype.Count == 0) //所有故障
                    {
                        QueryNewErrorSingleFault(RtuId,true);
                    }
                    else //单个故障
                    {
                        // if (tmptype.Contains(0)) tmptype.Remove(0);

                        QueryNewErrorSingleFault(RtuId, tmptype,true);
                    }
                }
                else
                {
                    if (tmptype.Count == 0) //所有故障
                    {
                        //QueryNewErrorAllRtuFault();
                        ResolveNewErrorAllRtuFault(true);
                    }
                    else //单个故障
                    {
                        //if (tmptype.Contains(0)) tmptype.Remove(0);
                        QueryNewErrorAllRtuFault(tmptype,true);
                    }
                }
                
                return;
            }

            QueryNowErrCount(TimeLong,FaultItemsTemp);
            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private bool CanExCountNow()
        {
            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                DateTime.Now.Ticks - _dtQuery.Ticks > 30000000 && DateTime.Now.Ticks - _dtOneAfter.Ticks > 300000 &&
                DateTime.Now.Ticks - _dtOneBefore.Ticks > 300000;

        }

        #endregion

        #region CmdCountOld

        private DateTime _dtCountOld;

        public ICommand CmdCountOld
        {
            get { return new RelayCommand(ExCountOld, CanExCountOld, true); }
        }


        private void ExCountOld()
        {
            var tmptype = GetSelectFaultType();
            int timeTmp;
            if ((TimeLong / 24) < 1)
            {
                UMessageBox.Show("提醒", "请选择大于1天！", UMessageBoxButton.Ok);
                return;
            }
            else
            {
                timeTmp = TimeLong / 24;
            }

            CountPreErrs = true;
            CountErrs = true;
            Records.Clear();
            Recordss.Clear();
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            if (!GetCheckedInformation()) return;
           


            if (IsSingleEquipmentQuery) //单个终端查询
            {
                if (RtuId == 0) return;
            
                {
                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(SelectedRtus , DtEndTime.AddDays(-timeTmp), DtEndTime);  //默认统计前7天
                    }
                    else //单个故障
                    {
                        Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(SelectedRtus , DtEndTime.AddDays(-timeTmp), DtEndTime,
                                                                                 tmptype);
                        //  if (tmptype.Contains(0)) tmptype.Remove(0);
                        //QueryPreErrorSingleFault(RtuId, tmptype);
                    }
                }
            }
            else
            {
                
                    // if (tmptype.Count == 0) return;
                    if (tmptype.Count == 0) //所有故障
                    {
                        Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtEndTime.AddDays(-timeTmp), DtEndTime);
                        //QueryPreErrorAllRtuFault();
                    }
                    else //单个故障
                    {
                        Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtEndTime.AddDays(-timeTmp), DtEndTime, tmptype);
                        //if (tmptype.Contains(0)) tmptype.Remove(0);
                        //QueryPreErrorAllRtuFault(tmptype);
                    }
                
            }
            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private DateTime _dtCountOldStartTime;
        private DateTime _dtCountOldEndTime;

        private bool CanExCountOld()
        {
            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                DateTime.Now.Ticks - _dtQuery.Ticks > 30000000 && DateTime.Now.Ticks - _dtOneAfter.Ticks > 300000 &&
                DateTime.Now.Ticks - _dtOneBefore.Ticks > 300000;
        }

        #endregion

        #region CmdOneAfter

        private DateTime _dtOneAfter;

        public ICommand CmdOneAfter
        {
            get { return new RelayCommand(ExQueryAfter, CanExQueryAfter, true); }
        }


        private void ExQueryAfter()
        {
            CountPreErrs = false;
            //CountLastPreErrs = false;
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;

            var tmptype = GetSelectFaultType();
            if (tmptype.Count == 0) tmptype = GetAllFaultType();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            if (!GetCheckedInformation()) return;
            //this.Records.Clear();
            //if (CountLastPreErrs )
            //{
            //    UMessageBox.Show("提醒", "无数据,这是最后一条数据！", UMessageBoxButton.Ok);
            //    return;
            //}

            if (IsSingleEquipmentQuery)
            {
                if(Records.Count==0)
                {
                    QueryErrAtSomeTime(RtuId, tmptype, false);
                }
                else
                {
                    if (RtuId==Records[0].RtuId )
                    {
                        DateTime DtTmp = new DateTime();
                        DtTmp = Convert.ToDateTime(Records[0].DtCreateTime).Date.AddDays(1);
                        Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, DtTmp, tmptype, false);
                    }
                    else
                    {
                        QueryErrAtSomeTime(RtuId, tmptype, false);
                    }
                }
                
            }
            else
            {
                RtuId = 0;
                if (Records.Count == 0)
                {
                    QueryErrAtSomeTime(RtuId, tmptype, true);
                }
                else
                {
                    DateTime tmp = new DateTime();
                    tmp = Convert.ToDateTime(Records[0].DtCreateTime).Date.AddDays(1);
                    Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, tmp, tmptype, false);
                }
            }
            //this.Records.Clear();
            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private DateTime _dtPreQueryAfterStartTime;
        private DateTime _dtPreQueryAfterEndTime;

        private bool CanExQueryAfter()
        {
            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                DateTime.Now.Ticks - _dtQuery.Ticks > 30000000 ;
        }

        #endregion

        #region CmdOneBefore/

        private DateTime _dtOneBefore;

        public ICommand CmdOneBefore
        {
            get { return new RelayCommand(ExQueryBefore, CanExQueryBefore, true); }
        }


        private void ExQueryBefore()
        {
            CountPreErrs = false;
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            //Remind = "查询命令已发送...请等待数据反馈！";
            _dtQuery = DateTime.Now;
            var tmptype = GetSelectFaultType();
            if (tmptype.Count == 0) tmptype = GetAllFaultType();
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询 ...";
            //Records.Clear();
            if (!GetCheckedInformation()) return;
            //if (CountLastPreErrs)
            //{
            //    UMessageBox.Show("提醒", "无数据，这是第一条数据~", UMessageBoxButton.Ok);
            //    return;
            //}
            if (IsSingleEquipmentQuery)
            {
                if (Records.Count == 0)
                {
                    QueryErrAtSomeTime(RtuId, tmptype, true );
                }
                else
                {
                    if (RtuId == Records[0].RtuId)
                    {
                        DateTime tmp = new DateTime();
                        tmp = Convert.ToDateTime(Records[0].DtCreateTime).Date;
                        Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, tmp, tmptype, true);
                    }
                    else
                    {
                        QueryErrAtSomeTime(RtuId, tmptype, true);
                    }
                }
            }
            else
            {
                RtuId = 0;
                if (Records.Count == 0)
                {
                    QueryErrAtSomeTime(RtuId, tmptype, true);
                }
                else
                {
                    //if (RtuId == Records[0].RtuId)
                    //{
                        DateTime tmp = new DateTime();
                        tmp = Convert.ToDateTime(Records[0].DtCreateTime).Date.AddDays(-1);
                        Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, tmp, tmptype, true);
                    //}
                    //else
                    //{
                    //    QueryErrAtSomeTime(RtuId, tmptype, true);
                    //}
                }
                
                //DtStartTime = DtStartTime.AddDays(-1);
                //Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(RtuId, DtStartTime, tmptype, true);
            }
            //Records.Clear();
            _isOnExport = false;
            ExportVisi = Visibility.Visible;

        }

        private DateTime _dtPreQueryBeforeStartTime;
        private DateTime _dtPreQueryBeforeEndTime;

        private bool CanExQueryBefore()
        {
            return (_dtPreQueryEndTime.Ticks != DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != DtStartTime.Ticks) &&
                    DateTime.Now.Ticks - _dtQuery.Ticks > 30000000 ;
        }

        #endregion

        #endregion

        #region Methods

        private static string GetRtuName(int rtuId)
        {
            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId))
            {
                return "Unknown";
            }
            var fff =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];

            return fff.RtuName;

        }

        private bool GetCheckedInformation()
        {
            //if (DtStartTime.AddDays(63) < DtEndTime)
            //{
            //    UMessageBox.Show("提醒", "请重新选择时间，时间需选择在62天以内", UMessageBoxButton.Ok);
            //    //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
            //    return false;
            //}
            return true;
        }



        #endregion


        #region DeleteFault

        //// EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultDefineSettingViewId
        //private bool GetIsCurrentHasRightToDelete()
        //{
        //    return
        //        Wlst.Cr.CoreOne.Models.MenuItemBase.IsglobalIdHasOperatorRight(
        //            EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultDefineSettingViewId);
        //}

        private int _counterLableDoubleClick;

        public int CounterLableDoubleClick
        {
            get { return _counterLableDoubleClick; }
            set
            {
                if (_counterLableDoubleClick == value) return;

                _counterLableDoubleClick = value;
                if (_counterLableDoubleClick > 2)
                {
                    _counterLableDoubleClick = 0;
                    //lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
                    //CmdDeleteVisi = Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D ? Visibility.Visible : Visibility.Collapsed;

                }
            }
        }

        private Visibility _cmdDeleteVisi;

        public Visibility CmdDeleteVisi
        {
            get { return _cmdDeleteVisi; }
            set
            {
                if (value == _cmdDeleteVisi) return;
                _cmdDeleteVisi = value;
                this.RaisePropertyChanged(() => this.CmdDeleteVisi);
            }
        }


        #region CmdDelete


        public ICommand CmdDelete
        {
            get { return new RelayCommand(ExDelete, CanExDelete, true); }
        }

        private DateTime _dtDelete;

        private void ExDelete()
        {
            //lvf 2018年8月24日15:14:58 提示删除
            var info = Wlst.Cr.MessageBoxOverride.MessageBoxOverride.UMessageBox.Show("确认", "是否要删除现存故障",
                                                                                          UMessageBoxButton.YesNo);
            if (info != true)
            {
                return ;
            }
            //CmdDeleteVisi = Visibility.Collapsed; lvf 2018年3月28日17:58:05  取消  管理可配置选项呈现删除按钮
            Remind = "删除命令已经发送，1秒后可重新查询...";

            this.DeleteQuery();
            Records.Clear();
        }


        private bool CanExDelete()
        {

            if (this.Records.Count < 1) return false;
            return DateTime.Now.Ticks - _dtDelete.Ticks > 30000000;
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
                lsttitle.Add("故障回路");
                lsttitle.Add("故障名称");
                lsttitle.Add("发生时间");
                if (IsOldFaultQuery) lsttitle.Add("消除时间");
                lsttitle.Add("电压V");
                lsttitle.Add("电流A");
                lsttitle.Add("上限A");
                lsttitle.Add("下限A");
                lsttitle.Add("额定A");
                lsttitle.Add("备注");
                //if (IsOldFaultQuery) 
                lsttitle.Add("电流上下限备注");

                var lstobj = new List<List<object>>();

                foreach (var g in Records)
                {
                    var tmp = new List<object>();
                    tmp.Add(g.Index);
                    tmp.Add(g.PhyId);
                    tmp.Add(g.RtuName);
                    tmp.Add(g.RtuLoopName);
                    tmp.Add(g.FaultName);
                    tmp.Add(g.DtCreateTime);
                    if (IsOldFaultQuery) tmp.Add(g.DtRemoceTime);
                    tmp.Add(g.V);
                    tmp.Add(g.A);
                    tmp.Add(g.AUpper);
                    tmp.Add(g.ALower);
                    tmp.Add(g.Aeding);
                    tmp.Add(g.Remark);
                    //if (IsOldFaultQuery)
                    //{
                        try
                        {
                            if (g.FaultName == "回路电流越上限")
                            {
                                var zhi = g.Remark.Substring(0, g.Remark.IndexOf(","));
                                zhi = zhi.Substring(3, zhi.Length - 3);
                                var xian = g.Remark.Substring(g.Remark.IndexOf(",") + 4, g.Remark.Length - 4 - g.Remark.IndexOf(","));
                                double dzhi, dxian, chazhi;
                                if (double.TryParse(zhi, out dzhi) && double.TryParse(xian, out dxian))
                                {
                                    chazhi = dzhi - dxian;
                                    tmp.Add(chazhi.ToString("F3"));
                                }
                                else
                                {
                                    tmp.Add("Error");
                                }

                            }
                            else if (g.FaultName == "回路电流越下限")
                            {
                                var zhi = g.Remark.Substring(0, g.Remark.IndexOf(","));
                                zhi = zhi.Substring(3, zhi.Length - 3);
                                var xian = g.Remark.Substring(g.Remark.IndexOf(",") + 4, g.Remark.Length - 4 - g.Remark.IndexOf(","));
                                double dzhi, dxian, chazhi;
                                if (double.TryParse(zhi, out dzhi) && double.TryParse(xian, out dxian))
                                {
                                    chazhi = dxian - dzhi;
                                    tmp.Add(chazhi.ToString("F3"));
                                }
                                else
                                {
                                    tmp.Add("Error");
                                }
                            }
                            else
                            {
                                tmp.Add("");
                            }
                        }
                        catch
                        {
                            tmp.Add("Error");
                        }
                    //}

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
            return DateTime.Now.Ticks - _dtCmdExport.Ticks > 30000000;
            return false;
        }

        #endregion



        #endregion
    }


    /// <summary>
    /// Data Query
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {
        private void Write_ManageInfo_To_Records()
        {
            var lst = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();

            for (int i = 0; i < Records.Count; i++)
            {
                foreach (var t in lst)
                {
                    if (t.Value.RtuId == Records[i].RtuId)
                    {
                        Records[i].DYGH = t.Value.Dygh;
                        Records[i].CQJ = t.Value.Cqj;

                        break;
                    }
                }                
            }
        }

        private void QueryNewErrorSingleFault(int rtuId, List<int> faultIds, bool isCol)
        {

            var sss = new List<FaultInfoBase>();
            if (IsNewAllQuery)
            {
                sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                       where SelectedRtus .Contains( gg.RtuId )  && faultIds.Contains(gg.FaultId)
                       select gg).ToList();
            }
            else
            {
                var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
                sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                       where gg.DateCreate.Ticks > dts && SelectedRtus.Contains(gg.RtuId) && faultIds.Contains(gg.FaultId)
                       select gg).ToList();
            }

            Recordss.Clear();
            Records.Clear();
            FaultItemsTemp.Clear();

            bool isloopError = false;
            var obss = new List<EquipmentFaultCurr.OneFaultItem>();

            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            //var ff = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetLstInfoByRtuId(FaultWarmType.Rtu,
            //               rtuId);
            int intx = 0;
            foreach (var t in sss)
            {
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;
                intx++;

                if (!Sh.Contains(t.FaultId))//error.A < 0.0001 && error.ALower < 0.0001 && error.AUpper < 0.0001 && error.Aeding < 0.0001 && error.V < 0.0001
                {
                    isloopError = false;
                }
                else
                {
                    isloopError = true;
                }

                var dtremocetime = "--";
                var dtcreatetime = "--";
                if (isCol)
                {
                    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }


                obs.Add(new EquipmentFaultViewModel
                            {
                                Index = intx,
                                DtCreateTime = dtcreatetime,
                                DtRemoceTime = dtremocetime,
                                FaultId = t.FaultId,
                                RtuId = t.RtuId ,
                                RtuLoopName = error.RtuLoopName,
                                RtuLoops = t.LoopId,
                                FaultName = error.FaultName,
                                PhyId = error.RtuPhyId,
                                Count = t.AlarmCount,
                                RtuName = error.RtuName,
                                Color = error.Color,
                                Remark = error.Remark,
                                DateCreateId = error.RecordId,
                                DateRemoveId = 0,
                                LampId = error.LampId,
                                A = !isloopError ? "---" : error.A.ToString("f2") + "",
                                AUpper = !isloopError ? "---" : error.AUpper.ToString("f2") + "",
                                ALower = !isloopError ? "---" : error.ALower.ToString("f2") + "",
                                Aeding = !isloopError ? "---" : error.Aeding.ToString("f2") + "",
                                V = !isloopError ? "---" : error.V.ToString("f2") + "",
                            });
                obss.Add(new EquipmentFaultCurr.OneFaultItem
                             {
                                 AlarmCount = 0,
                                 FaultId = t.FaultId,
                                 DateCreate = error.DateCreate.Ticks,
                                 LoopId = t.LoopId,
                                 RtuId = t.RtuId,
                                 Remark = error.Remark,
                                 LampId = error.LampId,
                             });

            }
            FaultItemsTemp.AddRange(obss);
            if (!CountNewErrs)
            {
                //添加区域筛选   lvf 2018年6月27日13:57:41
                FilterAreaErrs(obs,isCol);
                //if (isCol) Recordss = obs;
                //else Records = obs;

                //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count + " 条数据.";
            }
        }

        private void QueryNewErrorSingleFault(int rtuId,bool isCol)
        {

            var sss = new List<FaultInfoBase>();
            if (IsNewAllQuery)
            {
                sss = (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                       where SelectedRtus .Contains( gg.RtuId )  
                       select gg).ToList();
            }
            else
            {
                var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
                sss =
                (from gg in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                 where gg.DateCreate.Ticks > dts && SelectedRtus.Contains(gg.RtuId)  
                 select gg).ToList();
            }
            
            Recordss.Clear(); 
            Records.Clear();
            FaultItemsTemp.Clear();
            bool isloopError = false;
            var obss = new List<EquipmentFaultCurr.OneFaultItem>();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            //var ff = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetLstInfoByRtuId(FaultWarmType.Rtu,
            //                                                                                      rtuId);
            int intx = 0;
            foreach (var t in sss)
            {
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;

                intx++;

                if (!Sh.Contains(t.FaultId))
                {
                    isloopError = false;
                }
                else
                {
                    isloopError = true;
                }

                var dtremocetime = "--";
                var dtcreatetime = "--";
                if (isCol)
                {
                    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }


                obs.Add(new EquipmentFaultViewModel
                            {
                                Index = intx,
                                DtCreateTime = dtcreatetime,
                                DtRemoceTime = dtremocetime,
                                FaultId = t.FaultId,
                                PhyId = error.RtuPhyId,
                                RtuId = t.RtuId ,
                                RtuLoopName = error.RtuLoopName,
                                RtuLoops = t.LoopId,
                                FaultName = error.FaultName,
                                Count = t.AlarmCount ,
                                Color = error.Color,
                                RtuName = error.RtuName,
                                Remark = error.Remark,
                                DateCreateId = error.RecordId,
                                DateRemoveId = 0,
                                LampId = error.LampId,

                                A = !isloopError ? "---" : error.A.ToString("f2") + "",
                                AUpper = !isloopError ? "---" : error.AUpper.ToString("f2") + "",
                                ALower = !isloopError ? "---" : error.ALower.ToString("f2") + "",
                                Aeding = !isloopError ? "---" : error.Aeding.ToString("f2") + "",
                                V = !isloopError ? "---" : error.V.ToString("f2") + "",
                            });
                obss.Add(new EquipmentFaultCurr.OneFaultItem
                {
                    AlarmCount = 0,
                    FaultId = t.FaultId,
                    DateCreate = error.DateCreate.Ticks,
                    LoopId = t.LoopId,
                    RtuId = t.RtuId,
                    Remark = error.Remark,
                    LampId = error.LampId,
                    
                });

            }
            FaultItemsTemp.AddRange(obss);
            if (!CountNewErrs)
            {
                //添加区域筛选   lvf 2018年6月27日13:57:41
                FilterAreaErrs(obs,isCol);
                //if (isCol) Recordss = obs;
                //else Records = obs;
                //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";
            }
        }

        private void ResolveNewErrorAllRtuFault(bool isCol)
        {

            bool isloopError = false;
            var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
            FaultItemsTemp.Clear();
            Recordss.Clear();
            Records.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            var obs2 = new ObservableCollection<EquipmentFaultViewModel>();
            var obss = new List<EquipmentFaultCurr.OneFaultItem>();
            int intx = 0;

            var tmox = (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                        orderby  t.DateCreate descending
                        select t).ToList();               //t.IsShowAtTop descending ,
            foreach (var t in tmox)
            {
                if (IsNewAllQuery == false)
                {
                    if (t.DateCreate.Ticks < dts) continue;
                }
                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);

                //if (QueryMode ==3 ) //如果是区域查询 筛选一下咯  lvf 2018年6月19日09:52:54
                //{
                //   var areaid =
                //    Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(t.RtuId);
                //   if (areaid != AreaId) continue;
                //}



                if (error == null) continue;


                //如果 终端类型 需要判断 lvf 2018年12月11日13:48:19
                if (RtuTypeSelected.Value != -1)
                {

                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                             ContainsKey(t.RtuId))
                        return;
                    var tt =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (tt == null || tt.WjVoltage == null) continue;
                    if (tt.WjVoltage.RtuUsedType != RtuTypeSelected.Value) continue;

                }



                intx++;

                var dtcreatetime = "--";
                var dtremocetime = "--";


                if (!Sh.Contains(t.FaultId))
                {
                    isloopError = false ;
                }else
                {
                    isloopError = true   ;
                }
                    
                //if (isCol)
                //{
                //    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                //} 
                //else
                //{
                //    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                //}

                //if (error.DateFirst.Ticks > 100)
                //{
                //    dtcreatetime = error.DateFirst.ToString("yyyy-MM-dd HH:mm:ss");
                //    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                //}
                //else 
                if (!isCol)
                {
                    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }

                if (isCol)
                {
                    obs.Add(new EquipmentFaultViewModel
                    {
                        Index = intx,
                        DtCreateTime = dtcreatetime,
                        DtRemoceTime = dtremocetime,
                        FaultId = t.FaultId,
                        RtuId = t.RtuId,
                        RtuLoopName = error.RtuLoopName,
                        RtuLoops = t.LoopId,
                        PhyId = error.RtuPhyId,
                        FaultName = error.FaultName,
                        Count = t.AlarmCount,
                        Color = error.Color,
                        RtuName = error.RtuName,
                        Remark = error.Remark,
                        DateCreateId = error.RecordId,
                        DateRemoveId = 0,
                        LampId = error.LampId,

                        A = !isloopError ? "---" : error.A.ToString("f2") + "",
                        AUpper = !isloopError ? "---" : error.AUpper.ToString("f2") + "",
                        ALower = !isloopError ? "---" : error.ALower.ToString("f2") + "",
                        Aeding = !isloopError ? "---" : error.Aeding.ToString("f2") + "",
                        V = !isloopError ? "---" : error.V.ToString("f2") + "",
                    });
                }
                else
                {
                    obs.Add(new EquipmentFaultViewModel
                    {
                        Index = intx,
                        DtCreateTime = dtcreatetime,
                        DtRemoceTime = dtremocetime,
                        FaultId = t.FaultId,
                        RtuId = t.RtuId,
                        RtuLoopName = error.RtuLoopName,
                        RtuLoops = t.LoopId,
                        PhyId = error.RtuPhyId,
                        FaultName = error.FaultName,
                        Count = t.AlarmCount,
                        Color = error.Color,
                        RtuName = error.RtuName,
                        Remark = error.Remark,
                        DateCreateId = error.RecordId,
                        DateRemoveId = 0,
                        A = !isloopError ? "---" : error.A.ToString("f2") + "",
                        AUpper = !isloopError ? "---" : error.AUpper.ToString("f2") + "",
                        ALower = !isloopError ? "---" : error.ALower.ToString("f2") + "",
                        Aeding = !isloopError ? "---" : error.Aeding.ToString("f2") + "",
                        V = !isloopError ? "---" : error.V.ToString("f2") + "",
                        LampId = error.LampId,
                    });
                    
                }
                obss.Add(new EquipmentFaultCurr.OneFaultItem
                {
                    AlarmCount = 0,
                    FaultId = t.FaultId,
                    DateCreate =error.DateCreate.Ticks,
                    LoopId = t.LoopId,
                    RtuId = t.RtuId,
                    Remark = error.Remark,
                    LampId = error.LampId,


                    A=error.A,
                    AUpper = error.AUpper,
                    ALower = error.ALower,
                    Aeding = error.Aeding,
                    V = error.V,
                }
                );
            }
            FaultItemsTemp.AddRange(obss);
            if (!CountPreErrs)
            {
                //if (isCol)
                //{
                //    Recordss = obs;
                //    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Recordss.Count + " 条数据.";    

                //} 
                //else
                //{
                //    Records = obs;
                //    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";    

                //}

                //添加区域筛选   lvf 2018年6月27日13:57:41
                FilterAreaErrs(obs, isCol);
            }
            
        }



        private void QueryNewErrorAllRtuFault(List<int> faultIds, bool isCol)
        {
            Recordss.Clear();
            Records.Clear();
            FaultItemsTemp.Clear();

            bool isloopError = false;
            var obss = new List<EquipmentFaultCurr.OneFaultItem>();

            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            int intx = 0;
            var dts = new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 1).Ticks;
            foreach (var t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values)
            {



                if (!faultIds.Contains(t.FaultId)) continue;



                if (IsNewAllQuery == false)
                {
                    if (t.DateCreate.Ticks < dts) continue;
                }
                //if (QueryMode == 3) //如果是区域查询 筛选一下咯  lvf 2018年6月19日09:52:54
                //{
                //    var areaid =
                //     Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(t.RtuId);
                //    if (areaid != AreaId) continue;
                //}


                //this.AddErrorInfo(t);
                var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(t.Id);
                if (error == null) continue;

                //如果 终端类型 需要判断 lvf 2018年12月11日13:48:19
                if (RtuTypeSelected.Value != -1)
                {

                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                             ContainsKey(t.RtuId))
                        return;
                    var tt =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (tt == null || tt.WjVoltage == null) continue;
                    if (tt.WjVoltage.RtuUsedType != RtuTypeSelected.Value) continue;

                }




                int rtuphyid;
                //if (error.FaultId == 48) rtuphyid = error.RtuFhyId;
                //else rtuphyid = error.RtuPhyId;

                if (!Sh.Contains(t.FaultId))
                {
                    isloopError = false;
                }
                else
                {
                    isloopError = true;
                }


                intx++;

                var dtremocetime = "--";
                var dtcreatetime = "--";
                if (isCol)
                {
                    dtremocetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dtcreatetime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                }

                obs.Add(new EquipmentFaultViewModel
                            {
                                Index = intx,
                                DtCreateTime = dtcreatetime,
                                DtRemoceTime = dtremocetime,
                                FaultId = t.FaultId,
                                RtuId = t.RtuId,
                                RtuLoopName = error.RtuLoopName,
                                RtuLoops = t.LoopId,
                                PhyId = error.RtuPhyId,
                                FaultName = error.FaultName,
                                Count = t.AlarmCount,
                                Color = error.Color,
                                RtuName = error.RtuName,
                                Remark = error.Remark,
                                DateCreateId = error.RecordId,
                                DateRemoveId = 0,
                                A = !isloopError ? "---" : error.A.ToString("f2") + "",
                                AUpper = !isloopError ? "---" : error.AUpper.ToString("f2") + "",
                                ALower = !isloopError ? "---" : error.ALower.ToString("f2") + "",
                                Aeding = !isloopError ? "---" : error.Aeding.ToString("f2") + "",
                                V = !isloopError ? "---" : error.V.ToString("f2") + "",
                                LampId = error.LampId,
                            });
                obss.Add(new EquipmentFaultCurr.OneFaultItem
                             {
                                 AlarmCount = 0,
                                 FaultId = t.FaultId,
                                 DateCreate = error.DateCreate.Ticks,
                                 LoopId = t.LoopId,
                                 RtuId = t.RtuId,
                                 Remark = error.Remark,
                                 LampId = error.LampId,

                                 A = error.A,
                                 AUpper = error.AUpper,
                                 ALower = error.ALower,
                                 Aeding = error.Aeding,
                                 V = error.V,

                             });

            }
            FaultItemsTemp.AddRange(obss);
            if (!CountNewErrs)
            {
                //添加区域筛选   lvf 2018年6月27日13:57:41
                FilterAreaErrs(obs, isCol);
                //if (isCol) Recordss = obs;
                //else Records = obs;
                //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + Records.Count + " 条数据.";
            }
        }


        private void QueryPreErrorSingleFault(int rtuId, List<int> faultIds, int pageIndex, int pagingFlag)
        {
            //Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(SelectedRtus, DtStartTime, DtEndTime,
            //                                                                      faultIds);
            RequestInfo = Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistErrorHttp(SelectedRtus, DtStartTime, DtEndTime,
                                                                                  faultIds, pageIndex, pagingFlag);
            OnRequestFaultPre(RequestInfo, pagingFlag);
        }

        private void QueryPreErrorSingleFault(int rtuId, int pageIndex, int pagingFlag)
        {
           //Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(SelectedRtus, DtStartTime, DtEndTime);
           RequestInfo = Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistErrorHttp(SelectedRtus, DtStartTime, DtEndTime, pageIndex, pagingFlag);
           OnRequestFaultPre(RequestInfo, pagingFlag);
        }

        private void QueryPreErrorAllRtuFault(int pageIndex, int pagingFlag)
        {
            //Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtStartTime, DtEndTime);
            RequestInfo = Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistErrorHttp(DtStartTime, DtEndTime, pageIndex, pagingFlag);
            OnRequestFaultPre(RequestInfo, pagingFlag);                                                                                
        }

        private void QueryPreErrorAllRtuFault(List<int> faultIds, int pageIndex, int pagingFlag)
        {
            //Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(DtStartTime, DtEndTime, faultIds);
            RequestInfo = Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistErrorHttp(DtStartTime, DtEndTime, faultIds, pageIndex, pagingFlag);
            OnRequestFaultPre(RequestInfo, pagingFlag);
        }
         private void QueryNowErrCount(int dt,List<EquipmentFaultCurr.OneFaultItem> lstOneFaultItem )
         {
             Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrCountBetweenSomeTime(dt, lstOneFaultItem);
         }
         private void QueryErrAtSomeTime(int rtuId, List<int> faultIds,bool isPre)
         {
             Sr.EquipemntLightFault.Services.PreErrorServices.RequestErrAtSomeTime(rtuId, DtStartTime, faultIds, isPre);
         }
        private void DeleteQuery()
        {
            if (this.Records.Count == 0) return;

            var nt = Wlst.Sr.ProtocolPhone.LxFault.wlst_delete_falut_cur;//.ProtocolCnt.ServerPart.wlst_EquipemntLightFault_clinet_delete_curFault;
            foreach (var t in this.Records)
            {

                
                nt.WstFaultDeleteCurr.DeleteItems.Add(new EquipmentFaultDelete.EquipmentFaultDeleteItem()
                                           {
                                               FaultCode = t.FaultId,
                                               LoopId = t.RtuLoops,
                                               RtuId = t.RtuId,
                                               LampId = t.LampId
                                           });
            }


            Wlst.Sr.PPPandSocketSvr.Server.SocketClient.SndData(nt);
        }

        //private void OnPreDataBack(EquipmentPreFaultExChange info)
        //{
        //    int index = 1;
        //    Records.Clear();
        //    var obs = new ObservableCollection<EquipmentFaultViewModel>();
        //    foreach (var t in info.Info)
        //    {

        //      var mtpsss = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(RtuId);




        //        var itemsss = new EquipmentFaultViewModel
        //                          {
        //                              DtCreateTime = t.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
        //                              DtRemoceTime = t.DateRemove.ToString("yyyy-MM-dd HH:mm:ss"),
        //                              FaultId = t.FaultCodeId,
        //                              FaultName = "",

        //                              Index = index,
        //                              RtuId = t.RtuId,
        //                              PhyId = mtpsss == null ? 0 : mtpsss.PhyId,
        //                              RtuLoopName = mtpsss == null ? t.LoopId + "" : mtpsss.GetRtuLoopName( t.LoopId ),
        //                              RtuLoops = t.LoopId,
        //                              RtuName = mtpsss == null ? "" : mtpsss.RtuName,
        //                              Remark = t.Remark,

        //                              DateCreateId = t.RecordAlarmId,
        //                              DateRemoveId = t.RecordRemoveId,
        //                              LampId = t.LampId
        //                          };
        //        var typ = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoById(t.FaultCodeId);
        //        if (typ != null)
        //        {
        //            itemsss.FaultName = typ.FaultNameByDefine;
        //            itemsss.Color = typ.Color;
        //        }
        //        if (mtpsss == null)
        //        {


        //            itemsss.RtuName = GetRtuName(t.RtuId);
        //            itemsss.PhyId =
        //                Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetPhysicalIdByLogicalId(t.RtuId);
        //        }
        //        obs.Add(itemsss);
        //        index++;
        //    }
        //    Records = obs;
        //    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + Records.Count + " 条数据.";
        //}

        private void FilterAreaErrs(ObservableCollection<EquipmentFaultViewModel> records, bool iscol = false)
        {
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计0条数据.";


            //lvf 苏州新区  过滤  晚上回路报警 2019年3月27日11:01:14
            if (Sr.EquipmentInfoHolding.Services.Others.CityNum==7)
            {
                var recordtmp = new ObservableCollection<EquipmentFaultViewModel>();
                
                if (IsHideLoopErrInNight==false )
                {

                    foreach (var g in records)
                    {
                        recordtmp.Add(g);
                    }
                }
                else
                {
                    //过滤回路报警
                    //var recordtt = (from t in records where (t.FaultId < 6 || t.FaultId > 17) && (t.DtCreateTime >DtEndTime)  select t).ToList();

                    foreach (var g in records)
                    {
                        if ( g.FaultId<6 || g.FaultId>17)
                        {
                            recordtmp.Add(g);
                        }
                        else
                        {

                            var dt = new DateTime(g.DateCreateId);
                            int dtt = dt.Hour*60 + dt.Minute;
                            //如果报警为白天的呈现；如果是晚上的不呈现
                            if (dtt>Wlst.Sr.EquipmentInfoHolding.Services.Others.Sunrise  && dtt <Wlst.Sr.EquipmentInfoHolding.Services.Others.Sunset)
                            {
                                recordtmp.Add(g);
                            
                            }

                        }

                    }

                }

             

                #region  苏州新区 数据处理
                if (QueryMode != 3 || AreaId == -1)
                {
                    if (iscol == false)
                    {
                        Records = recordtmp;
                        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + ItemCount + " 条数据.";
                    }
                    else
                    {
                        Recordss = recordtmp;
                        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + ItemCount + " 条数据.";
                    }

                }
                else
                {
                    if (GrpId == -1) //全部分组
                    {
                        int newindex = 1;
                        foreach (var g in recordtmp)
                        {
                            var areaid =
                                Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(g.RtuId);
                            if (areaid == AreaId)
                            {
                                g.Index = newindex;
                                newindex++;
                                if (iscol == false)
                                {
                                    Records.Add(g);
                                    if (Records.Count % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + ItemCount +
                                             " 条数据.";
                                }
                                else
                                {
                                    Recordss.Add(g);
                                    if (Recordss.Count % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + ItemCount +
                                             " 条数据.";
                                }

                            }
                        }
                    }
                    else //选择了分组
                    {
                        var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(
                            AreaId, GrpId); //GetRtuInArea(AreaId);

                        var pb = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                                  where lstInArea.Contains(t.Value.RtuFid)
                                  select t.Key).ToList();
                        lstInArea.AddRange(pb);

                        int newindex = 1;
                        foreach (var g in recordtmp)
                        {

                            if (lstInArea.Contains(g.RtuId) == false) continue;
                            g.Index = newindex;
                            newindex++;
                            if (iscol == false)
                            {
                                Records.Add(g);
                                if (Records.Count % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + ItemCount +
                                         " 条数据.";
                            }
                            else
                            {
                                Recordss.Add(g);
                                if (Recordss.Count % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + ItemCount +
                                         " 条数据.";
                            }




                        }
                    }

                }


                #endregion

                return;
            }




            if (QueryMode != 3 || AreaId == -1)
            {
                if (iscol == false)
                {
                    Records = records;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + (IsOldFaultQuery?ItemCount: Records.Count) + " 条数据.";
                }
                else
                {
                    Recordss = records;
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + (IsOldFaultQuery ? ItemCount : Recordss.Count) + " 条数据.";
                }

            }
            else
            {
                if (GrpId == -1) //全部分组
                {
                    int newindex = 1;
                    foreach (var g in records)
                    {
                        var areaid =
                            Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(g.RtuId);
                        if (areaid == AreaId)
                        {
                            g.Index = newindex;
                            newindex++;
                            if (iscol == false)
                            {
                                Records.Add(g);
                                if (Records.Count % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + (IsOldFaultQuery ? ItemCount : Records.Count) +
                                         " 条数据.";
                            }
                            else
                            {
                                Recordss.Add(g);
                                if (Recordss.Count % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + (IsOldFaultQuery ? ItemCount : Recordss.Count) +
                                         " 条数据.";
                            }

                        }
                    }
                }
                else //选择了分组
                {
                    var lstInArea = Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(
                        AreaId, GrpId); //GetRtuInArea(AreaId);

                    var pb = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                              where lstInArea.Contains(t.Value.RtuFid)
                              select t.Key).ToList();
                    lstInArea.AddRange(pb);

                    int newindex = 1;
                    foreach (var g in records)
                    {

                        if (lstInArea.Contains(g.RtuId) == false) continue;
                        g.Index = newindex;
                        newindex++;
                        if (iscol == false)
                        {
                            Records.Add(g);
                            if (Records.Count % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + (IsOldFaultQuery ? ItemCount : Records.Count) +
                                     " 条数据.";
                        }
                        else
                        {
                            Recordss.Add(g);
                            if (Recordss.Count % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 故障记录查询成功，共计" + (IsOldFaultQuery ? ItemCount : Recordss.Count) +
                                     " 条数据.";
                        }




                    }
                }

            }

        }

        private void FilterAreaErrs(List<EquipmentFaultViewModel> records, bool iscol = false)
        {
            ObservableCollection<EquipmentFaultViewModel> tmp = new ObservableCollection<EquipmentFaultViewModel>();
            foreach (var g in records)
            {
                tmp.Add(g);
            }
            FilterAreaErrs(tmp, iscol);
        }
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class EquipmentFaultRecordQueryViewModel
    {
        

        public void OnRequestServerData(EquipmentFaultViewModel info)
        {
            if (info == null) return;
            Sr.EquipemntLightFault.Services.PreErrorServices.RequestDataWhenErrorHappen(info.RtuId, info.RtuLoops,
                                                                                        info.DateCreateId);
            
            //发布事件  选中当前节点
            var args = new PublishEventArgs
                           {
                               EventType = PublishEventType.Core,
                               EventId = Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                               EventAttachInfo = "RequestDataWhenErrorHappenEqu",
                           };

            args.AddParams(info.RtuId);
            EventPublish.PublishEvent(args);
        }

        private void InitEvent()
        {
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

        }
        


        #region EventSubScriptionTokener

        private bool _thisViewActive;

        public void FundEventHandler(PublishEventArgs args) // should do somework
        {

            
            try
            {
                if (args.EventType == PublishEventType.Core)
                {


                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        //  
                        if (OptionXmlSvr.GetOptionInt(4001, 2) ==1) return;
                        if (!_thisViewActive) return;
                        if (args.EventAttachInfo == "RequestDataWhenErrorHappenEqu") return;
                        int id = Convert.ToInt32(args.GetParams()[0]);
                        if (id < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart) return;
                        if (RtuId == id) return;

                        SelectedRtus.Clear();
                        SelectedRtus.Add(id);  
                        RtuId = id;
                        
                        if (IsSingleEquipmentQuery)
                        {
                          
                            Records.Clear();
                            CountLastPreErrs = false;

                            Ex();
                        }

                        //if (!IsOldFaultQuery)
                        //{
                        //    Ex();
                        //}else
                        //{
                        //     if (IsSingleEquipmentQuery) Ex();
                        //}
                    }

                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentMulSelected )
                    {
                        if (!_thisViewActive) return;
                        if (OptionXmlSvr.GetOptionInt(4001, 2) != 1) return;
                       // if (args.EventAttachInfo == "RequestDataWhenErrorHappenEqu") return;
                        var  ids = args.GetParams()[0] as List< int >;

                        var rtus = (from t in ids
                                    where
                                        t < Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuEnd &&
                                        t > Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.RtuStart
                                    select t).ToList();
                        SelectedRtus.Clear();
                        SelectedRtus = rtus;
                        if (rtus.Count == 0)
                        {
                            this.SelectedRtus.Clear();
                            RtuId = 0;
                            RtuName = "通过终端树勾选终端进行故障查询.";
                        }
                        else
                        {
                            RtuId = SelectedRtus[0];
                            if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (RtuId))
                                return;
                            var tml =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                                    [RtuId];
                            RtuName = tml.RtuName;
                            if (SelectedRtus.Count > 1)
                                RtuName += " [等" + SelectedRtus.Count + "个终端]";

                        }

                        //if (RtuId == id) return;


                        //if (IsSingleEquipmentQuery)
                        //{
                        //    RtuId = id;
                        //    Records.Clear();
                        //    CountLastPreErrs = false;

                        //    Ex();
                        //}

                        //if (!IsOldFaultQuery)
                        //{
                        //    Ex();
                        //}else
                        //{
                        //     if (IsSingleEquipmentQuery) Ex();
                        //}
                    }
                    //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId)
                    //{
                    //    if (Wlst.Sr.EquipmentInfoHolding.Services.Others.IsShowNewErrArriveOnUi)
                    //    {
                    //        if (args.GetParams().Count > 0)
                    //        {
                    //            var rtx = args.GetParams()[0] as List<int>;
                    //            if (rtx == null) return;

                    //            if (rtx.Count > 1)
                    //            {
                    //                if (Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Count
                    //                    > 0)
                    //                    ClickTime =
                    //                        (from t in
                    //                             Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.
                    //                             InfoDictionary.
                    //                             Values
                    //                         orderby t.DateCreate descending
                    //                         select t).ToList()[0].DateCreate;
                    //            }
                    //            else
                    //            {
                    //                var tmox =
                    //                    (from t in
                    //                         Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.
                    //                         Values
                    //                     where t.DateCreate > ClickTime && t.IsThisUserShow
                    //                     orderby t.DateCreate descending
                    //                     select t).ToList();

                    //                var argss = new PublishEventArgs()
                    //                                {
                    //                                    EventType = PublishEventType.Core,
                    //                                    EventId = EventIdAssign.PushErrNums
                    //                                };
                    //                argss.AddParams(tmox.Count);
                    //                EventPublish.PublishEvent(argss);

                                    
                    //            }
                    //        }
                    //    }
                    //    if (!_thisViewActive) return;
                    //    if (IsLockThisViewOnNewErrArrive) return;
                    //    var info = args.GetParams()[0] as List<int>;
                    //    if (info == null) return;
                    //    foreach (var t in info)
                    //    {
                    //        var ntgs =
                    //            Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.
                    //                GetFaultInfoById
                    //                (t);
                    //        if (ntgs != null)
                    //            AddErrorInfo(ntgs, true);
                    //    }


                    //}
                }
                //if (args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                //{
                //    var infos = args.GetParams()[1] as EquipmentPreFaultExChange;
                //    if (infos == null) return;
                //    OnPreDataBack(infos);
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "EquipmentDataQuery.EquipmentFaultRecordQueryViewModel FundEventHandler occer an error:" +
                    ex);
            }

        }

        public bool FundOrderFilter(PublishEventArgs args) //接收终端选中变更事件
        {
            if (!_thisViewActive) return false;
            // if (!IsSingleEquipmentQuery) return false;
            try
            {
                if (args.EventType == PublishEventType.Core)
                {
                    //if (!IsSingleEquipmentQuery) return false;  lvf
                    //if (args.EventId == Sr.EquipmentGroupInfoHolding.Services.EventIdAssign.MainSingleTreeNodeActive)
                    //{
                    //    if (Convert.ToInt32(args.GetParams()[1]) == 2)
                    //    {
                    //        return true;
                    //    }
                    //}
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                    {
                        if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return false ;
           
                        return true;
                    }
                    if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentMulSelected)
                    {
                        if (OptionXmlSvr.GetOptionInt(4001, 2) == 1) return true;
                        return false;

                    }
                    //if (args.EventId ==Sr.EquipemntLightFault.Services.EventIdAssign.EquipmentExistFaultAddId)
                    //{
                    //    return true;
                    //}

                }
                //if (args.EventType == PublishEventType.Sevr &&
                //    args.EventId == Sr.EquipemntLightFault.Services.EventIdAssign.PreExistErrorRequestId)
                //{
                //    return true;
                //}
            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError("Error:" + ex);
            }
            return false;
        }

        private void AddErrorInfo(FaultInfoBase error, bool dongtaiupdate)
        {
            return;
            // var error = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultInfoById(id);
            if (error == null) return;

            var infovm = new EquipmentFaultViewModel
            {
                Index = 0,
                DtCreateTime = error.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                DtRemoceTime = "--",
                FaultId = error.FaultId,
                RtuId = error.RtuId,
                RtuLoopName = error.RtuLoopName,
                RtuLoops = error.LoopId,
                PhyId = error.RtuPhyId,
                FaultName = error.FaultName,
                Count = error.AlarmCount + 1,
                Color = error.Color,
                RtuName = error.RtuName,
                Remark = error.Remark,
                DateCreateId = error.RecordId,
                DateRemoveId = 0,
            };

            Records.Insert(0, infovm);


            //if (Records.Count > MaxRecordHold)
            //{
            //    Records.RemoveAt(0);
            //}
        }

        #endregion

        private void InitAction()
        {
            //ProtocolServer.RegistProtocol(
            //    Sr.ProtocolPhone.LxFault.wlst_fault_pre,
            //    OnRequestFaultPre,
            //    typeof(EquipmentFaultRecordQueryViewModel), this);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxFault.wlst_fault_curr_time_cal,//现存故障统计
                OnRequestFaultCurrTimeCal,
                typeof (EquipmentFaultRecordQueryViewModel), this);
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxFault.wlst_fault_pre_for_single, //查询最近的前一条 or后一条
                OnRequestFaultPreForSingle,
                typeof (EquipmentFaultRecordQueryViewModel), this);
            //lvf todo
        }


        //private string pathcx = Environment.CurrentDirectory + "\\Config\\IsShowThisViewOnNewErrArrive.txt";
        //private void LoadIsShowThisViewOnNewErrArrive()
        //{
        //    var ft = Wlst.Ux.EquipemntLightFault.Services.fileread.Read(pathcx);
        //    if (string.IsNullOrEmpty(ft)) IsShowThisViewOnNewErrArrive = false;
        //    else
        //    {
        //        int x = 0;
        //        if (Int32.TryParse(ft, out x))
        //        {
        //            IsShowThisViewOnNewErrArrive = (x == 1);
        //        }
        //    }
        //}

        //public static bool IsShowThisViewOnNewErrArriveInfo = false;
        private bool _cheIsLockThisViewOnNewErrArriveck;

        public bool IsLockThisViewOnNewErrArrive
        {
            get { return _cheIsLockThisViewOnNewErrArriveck; }
            set
            {
                if (_cheIsLockThisViewOnNewErrArriveck != value)
                {
                    _cheIsLockThisViewOnNewErrArriveck = value;
                    this.RaisePropertyChanged(() => this.IsLockThisViewOnNewErrArrive);
                    IsLockThisViewOnNewErrArrive = value;
                }
            }
        }

        private Visibility middleVisi;
        public void OnRequestFaultPre(Wlst.mobile.MsgWithMobile infos,int pagingFlag)
        {

            if (!_thisViewActive) return ;
            var list = infos.WstFaultPre;
            if (pagingFlag == 0)
            {
                PageSize = infos.Head.PagingNum;
                ItemCount = infos.Head.PagingRecordTotal;
                var count = ItemCount / PageSize + (ItemCount % PageSize > 0 ? 1 : 0);
                PagerVisi = count < 2 ? Visibility.Collapsed : Visibility.Visible;
                middleVisi = PagerVisi;
                PageTotal = "页     " + ItemCount + " 条";
            }
            ////////if (CountPreErrs)
            ////////{
            ////////    OnRequestFaultPreCount(list);
            ////////    return;
            ////////}
            //var list = obj as List<PreErrorItem>;
            //if (list == null) return;
            //  Records.Clear();
            var tmp = new Dictionary<Tuple<int, int, int>, int>();

            bool isloopError = false;
            Records.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
            int indexx = PageIndex * PageSize;

            //lvf  2018年4月10日15:46:06   按照发生时间排序
            var faultItems = (from t in list.FaultItems orderby t.DateCreate ascending select t).ToList();

            foreach (var item in faultItems) //list.FaultItems
            {
                int count;

                //if (IsUseTimeLongQuery)
                //{

                //if (QueryMode == 3) //如果是区域查询 筛选一下咯  lvf 2018年6月19日09:52:54
                //{
                //    var areaid =
                //     Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(item.RtuId);
                //    if (areaid != AreaId) continue;
                //}

                var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);
                if (tmp.ContainsKey(tu)) count = tmp[tu];
                else
                {
                    count =
                        (from t in list.FaultItems
                         where
                             t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
                             t.FaultId == item.FaultId
                         select t).Count();
                    tmp.Add(tu, count);

                }
                indexx++;









                var mtpsss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(item.RtuId);
                int py = item.RtuId;
                string rtuname = "";
                if (mtpsss != null)
                {
                    rtuname = mtpsss.RtuName;
                    if (mtpsss.RtuFid > 0)
                    {
                        var nfx =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                mtpsss.RtuFid);
                        if (nfx != null)
                        {
                            rtuname = nfx.RtuName + "-" + mtpsss.RtuName;
                            py = nfx.RtuPhyId;
                        }
                    }
                    else py = mtpsss.RtuPhyId;
                }
                else
                {
                    var sluinfo = Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(item.RtuId);
                    if ( sluinfo != null)
                    {
                        rtuname = sluinfo.FieldName;
                        py = sluinfo.PhyId;
                    }
                }
                //var loopName = mtpsss != null
                //                                ? mtpsss.GetLoopName(item.LoopId)
                //                                  : item.LoopId.ToString(CultureInfo.InvariantCulture);
                //if (item.FaultId == 21 || item.FaultId ==20) loopName = "开关量输出K" + item.LoopId;
                string loopName = "";

                if (item.FaultId == 20 || item.FaultId == 21)
                {
                    var t =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[item.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    if (t.WjSwitchOuts.ContainsKey(item.LoopId))
                    {
                        loopName = t.WjSwitchOuts[item.LoopId].SwitchName;
                    }
                    else
                    {
                        loopName = "开关量输出 K" + item.LoopId;
                    }

                }
                else if (item.FaultId > 5 && item.FaultId < 18)
                {
                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (loopName.Trim() == "")
                    {
                        loopName = "回路" + item.LoopId;

                    }

                }
                else if (item.FaultId >= 50 && item.FaultId < 80 && item.LoopId > 0)
                {
                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (item.LampId > 0)
                    {
                        loopName = loopName + "," + item.LampId;
                    }



                    //nb 单灯
                    if (item.RtuId > 1700000 && item.RtuId<1800000)
                    {
                        var ctrlinfo = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(item.LoopId);
                        if (ctrlinfo != null)
                        {
                            loopName = ctrlinfo.OrderId + "-" + ctrlinfo.CtrlName;
                            loopName = loopName + "," + item.LampId;
                        }
                    }
           
                }
                else
                {

                    loopName = mtpsss != null
                                   ? mtpsss.GetLoopName(item.LoopId)
                                   : item.LoopId.ToString(CultureInfo.InvariantCulture);

                }

                //if (item.A < 0.0001 && item.ALower < 0.0001 && item.AUpper < 0.0001 && item.Aeding < 0.0001 && item.V < 0.0001)
                //{
                //    isloopError = false;
                //}
                //else
                //{
                //    isloopError = true;
                //}


                if (!Sh.Contains(item.FaultId))
                {
                    isloopError = false;
                }
                else
                {
                    isloopError = true;
                }


                //筛选

                //如果 终端类型 需要判断 lvf 2018年12月11日13:48:19
                if (RtuTypeSelected.Value != -1)
                {

                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.
                            ContainsKey(item.RtuId))
                        return;
                    var tt =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[item.RtuId]
                            as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (tt == null || tt.WjVoltage == null) continue;
                    if (tt.WjVoltage.RtuUsedType != RtuTypeSelected.Value) continue;

                }


                obs.Add(new EquipmentFaultViewModel
                            {
                                DtCreateTime = new DateTime(item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = new DateTime(item.DateRemove).ToString("yyyy-MM-dd HH:mm:ss"),
                                Index = indexx,
                                FaultId = item.FaultId,
                                RtuId = item.RtuId,
                                PhyId = py,
                                RtuLoopName = loopName,
                                RtuLoops = item.LoopId,
                                RtuName = rtuname,
                                //mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                                FaultName = GetFaultName(item.FaultId).Item1,
                                Color = GetFaultColor(item.FaultId),
                                Count = count,
                                Remark = item.Remark,
                                DateCreateId = item.DateCreate,
                                DateRemoveId = item.DateRemove,
                                LampId = item.LampId,
                                IsShowAtTop = GetFaultName(item.FaultId).Item2,

                                A = !isloopError ? "---" : item.A.ToString("f2") + "",
                                AUpper = !isloopError ? "---" : item.AUpper.ToString("f2") + "",
                                ALower = !isloopError ? "---" : item.ALower.ToString("f2") + "",
                                Aeding = !isloopError ? "---" : item.Aeding.ToString("f2") + "",
                                V = !isloopError ? "---" : item.V.ToString("f2") + "",
                            });
                // if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo
            }

            var mtp = (from t in obs orderby t.DateCreateId ascending select t).ToList(); //t.IsShowAtTop descending,



            //lvf 2018年5月9日09:50:23   历史故障统计 选项中开启
            if (IsCalcFault)
            {
                var errDic = new Dictionary<Tuple<int, int, int, int>, List< EquipmentFaultViewModel>>();
                foreach (var f in mtp )
                {
                    var tu = new Tuple<int, int, int, int>(f.RtuId, f.RtuLoops, f.LampId, f.FaultId);
                    if (errDic.ContainsKey(tu) == false) errDic.Add(tu, new List<EquipmentFaultViewModel>());
                    errDic[tu].Add(f);
                }

                var mtpTmp = new List<EquipmentFaultViewModel>();
                int inddd = 0;
                foreach (var f in errDic )
                {
                    inddd++;
                    var ntg = (from t in f.Value orderby t.DateCreateId ascending select t).ToList();
                    var first = ntg[0];
                    first.Count = ntg.Count;
                    first.Index = inddd;
                    int indeeex = 0;
                    foreach (var fx in ntg)
                    {
                        if(indeeex==0 )
                        {
                            indeeex++;
                          
                            var fxx = new EquipmentFaultViewModel
                                                {
                                                    DtCreateTime = fx.DtCreateTime,
                                                    DtRemoceTime = fx.DtRemoceTime,
                                                    Index = indeeex,
                                                    FaultId = fx.FaultId,
                                                    RtuId = fx.RtuId,
                                                    PhyId = fx.PhyId,
                                                    RtuLoopName = fx.RtuLoopName,
                                                    RtuLoops = fx.RtuLoops,
                                                    RtuName = fx.RtuName,
                                                    //mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                                                    FaultName = fx.FaultName,
                                                    Color = fx.Color,
                                                    Count = fx.Count ,
                                                    Remark = fx.Remark,
                                                    DateCreateId = fx.DateCreateId,
                                                    DateRemoveId = fx.DateRemoveId,
                                                    LampId = fx.LampId,
                                                    IsShowAtTop = fx.IsShowAtTop,

                                                    A = fx.A,
                                                    AUpper = fx.AUpper,
                                                    ALower = fx.ALower,
                                                    Aeding = fx.Aeding,
                                                    V = fx.V,
                                                };
                            first.ChildItems.Add(fxx);
                        }
                        else
                        {
                            indeeex++;
                            fx.Index = indeeex;
                            first.ChildItems.Add(fx);
                        }

                    } // ?
        

                    mtpTmp.Add(first);
                }


                Records.Clear();
                var mtppp = (from t in mtpTmp orderby t.DateCreateId ascending select t).ToList();

                //添加区域筛选   lvf 2018年6月27日13:57:41
                FilterAreaErrs(mtppp);
                
                //foreach (var f in mtppp)
                //{
                //    indexx++;
                //    Records.Add(f);
                //    if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                //}

                ////  Remind = "数据已反馈完毕，请查看数据！";
                //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + errDic.Count + " 条记录. " + mtp.Count + "条数据";
            }
            else
            {
                Records.Clear();
                //添加区域筛选   lvf 2018年6月27日13:57:41
                FilterAreaErrs(mtp);
                //foreach (var f in mtp)
                //{
                //    indexx++;
                //    Records.Add(f);
                //    if (indexx%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                //}

                ////  Remind = "数据已反馈完毕，请查看数据！";
                //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + list.FaultItems.Count + " 条数据.";
            }

        }

        public void OnRequestFaultPreCount(EquipmentFaultPre list)
        {
            var tmp = new Dictionary<Tuple<int, int, int>, Tuple<int, long, long>>();

            Records.Clear();
            Recordss.Clear();
            var obs = new ObservableCollection<EquipmentFaultViewModel>();
           // var lst = new List<Tuple<int, int, int>>();

            var dic = new Dictionary<Tuple<int, int, int>, Tuple<int, long, long>>();


            var llll = (from t in list.FaultItems
                        orderby t.DateCreate 
                        select t);

            foreach (var item in llll)    //todo 不算现存的最新的一条
            {
                var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);

                if (!dic.ContainsKey(tu))
                {
                    var tu2 = new Tuple<int, long, long>(1, item.DateCreate, item.DateCreate);
                    dic.Add(tu, tu2);
                }
                else
                {
                    var tu2 = new Tuple<int, long, long>(dic[tu].Item1 + 1, dic[tu].Item2, item.DateCreate);
                    dic[tu] = tu2;
                }
            }

            tmp = dic;

            //foreach (var item in list.FaultItems)
            //{
            //    int count;
            //    long lastTime;
            //    long firstTime;
            //    //if (IsUseTimeLongQuery)
            //    //{

            //    var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);
            //    var lll = (from t in list.FaultItems
            //               orderby t.DateCreate ascending
            //               where
            //                   t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
            //                   t.FaultId == item.FaultId
            //               select t);

            //    if (tmp.ContainsKey(tu))
            //    {
            //        count = tmp[tu].Item1;
            //        firstTime = tmp[tu].Item2;
            //        lastTime = tmp[tu].Item3;
            //    }
            //    else
            //    {
            //        count = lll.Count();
            //        //(from t in list.FaultItems
            //        // orderby t.DateCreate ascending
            //        // where
            //        //     t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
            //        //     t.FaultId == item.FaultId
            //        // select t).Count();
            //        firstTime = lll.First().DateCreate;
            //        lastTime = lll.Last().DateCreate;
            //        var a = new Tuple<int, long, long>(count, firstTime, lastTime);
            //        tmp.Add(tu, a);

            //    }
            //}
            int indexx = 0;
            foreach (var g in tmp)
            {

                indexx++;

                indexx++;
                int rtuid = g.Key.Item1;
                int py = g.Key.Item1;//.RtuId;
                int loopId = g.Key.Item2;
                int faultId = g.Key.Item3;
                int count = g.Value.Item1;
                long firstTime = g.Value.Item2;
                long lastTime = g.Value.Item3;
                string firstT = "";
                string lastT = "";
                string rtuname = "";


                

                lastT = new DateTime(lastTime).ToString("yyyy-MM-dd HH:mm:ss");
                
                if (count == 1)
                {
                    firstT = "---";
                }else
                {
                    firstT = new DateTime(firstTime).ToString("yyyy-MM-dd HH:mm:ss");
                }
                var mtpsss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuid);
                if (mtpsss != null)
                {
                    rtuname = mtpsss.RtuName;
                    if (mtpsss.RtuFid > 0)
                    {
                        var nfx =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                mtpsss.RtuFid);
                        if (nfx != null)
                        {
                            rtuname = nfx.RtuName + "-" + mtpsss.RtuName;
                            py = nfx.RtuPhyId;
                        }
                    }
                    else py = mtpsss.RtuPhyId;
                }

                //var loopName = mtpsss != null
                //                                ? mtpsss.GetLoopName(loopId) //loopid
                //                                  : loopId.ToString(CultureInfo.InvariantCulture);
                //if (faultId == 21) loopName = "开关量输出K" + loopId; //faultID

                string loopName = "";

                if (faultId == 20 || faultId == 21)
                {
                    var t =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuid]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    if (t.WjSwitchOuts.ContainsKey(loopId))
                    {
                        loopName = t.WjSwitchOuts[loopId].SwitchName;
                    }
                    else
                    {
                        loopName = "开关量输出 K" + loopId;
                    }

                }
                else if (faultId > 5 && faultId < 18)
                {
                    loopName = mtpsss != null ? mtpsss.GetLoopName(loopId) : loopId.ToString(CultureInfo.InvariantCulture);
                    if (loopName.Trim() == "")
                    {
                        loopName = "回路" + loopId;

                    }

                }
                else if (faultId >= 50 && faultId < 80 && loopId > 0)
                {
                    loopName = "控制器" + loopId;
                }
                else
                {

                    loopName = mtpsss != null ? mtpsss.GetLoopName(loopId) : loopId.ToString(CultureInfo.InvariantCulture);

                }

                obs.Add(new EquipmentFaultViewModel
                {
                    DtCreateTime = firstT,//new DateTime(firstTime).ToString("yyyy-MM-dd HH:mm:ss"), //new DateTime(item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                    DtRemoceTime = lastT,//new DateTime(item.DateRemove).ToString("yyyy-MM-dd HH:mm:ss"),
                    Index = indexx,
                    FaultId = faultId,
                    RtuId = py,
                    PhyId = py,
                    RtuLoopName = loopName,
                    RtuLoops = loopId,
                    RtuName = rtuname,//mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                    FaultName = GetFaultName(faultId).Item1,
                    Color = GetFaultColor(faultId),
                    Count = count,
                    //Remark = mtpsss.RtuRemark,//.Remark,
                    DateCreateId = firstTime,//item.DateCreate,
                    DateRemoveId = lastTime,//item.DateRemove,
                    //LampId = item.LampId,
                    IsShowAtTop = GetFaultName(faultId).Item2,



                    
                });
                if (count ==1)
                {
                  
                }
                // if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo
            }
            
            

            var mtp = (from t in obs orderby t.DtRemoceTime descending select t).ToList();//t.IsShowAtTop descending,

            Recordss.Clear();
            Records.Clear();

            //添加区域筛选   lvf 2018年6月27日13:57:41
            FilterAreaErrs(mtp);

            //foreach (var f in mtp)
            //{
            //    indexx++;
            //    Recordss.Add(f);
            //    if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            //}

            ////  Remind = "数据已反馈完毕，请查看数据！";
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + mtp.Count + " 条数据.";//list.FaultItems.Count + " 条数据.";
        }


        public void OnRequestFaultCurrTimeCal(string session, Wlst.mobile.MsgWithMobile infos)
        {
            if (!_thisViewActive) return;
            var list = infos.WstFaultCurrForTimeCal;
            ArgsInfoVisi = false;//统计故障时，隐藏单条具体参数信息

            //var list = obj as List<PreErrorItem>;
            //if (list == null) return;
            //  Records.Clear();
            var tmp = new Dictionary<Tuple<int, int, int>, int>();



           Records.Clear();
           Recordss.Clear();
           var obs = new ObservableCollection<EquipmentFaultViewModel>();

            int indexx = 0;
            foreach (var item in list.FaultItems)
            {
                int count;
                //if (IsUseTimeLongQuery)
                //{

                //var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);
                //if (tmp.ContainsKey(tu)) count = tmp[tu];
                //else
                //{
                //    count =
                //        (from t in list.FaultItems
                //         where
                //             t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
                //             t.FaultId == item.FaultId
                //         select t).Count();
                //    tmp.Add(tu, count);

                //}
                indexx++;
                var mtpsss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(item.RtuId);
                int py = item.RtuId;
                string rtuname = "";
                if (mtpsss != null)
                {
                    rtuname = mtpsss.RtuName;
                    if (mtpsss.RtuFid > 0)
                    {
                        var nfx =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                mtpsss.RtuFid);
                        if (nfx != null)
                        {
                            rtuname = nfx.RtuName + "-" + mtpsss.RtuName;
                            py = nfx.RtuPhyId;
                        }
                    }
                    else py = mtpsss.RtuPhyId;
                }

                //var loopName = mtpsss != null
                //                                ? mtpsss.GetLoopName(item.LoopId)
                //                                  : item.LoopId.ToString(CultureInfo.InvariantCulture);
                //if (item.FaultId == 21 || item.FaultId == 20) loopName = "开关量输出K" + item.LoopId;
                string loopName = "";

                if (item.FaultId == 20 || item.FaultId == 21)
                {
                    var t =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[item.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    if (t.WjSwitchOuts.ContainsKey(item.LoopId))
                    {
                        loopName = t.WjSwitchOuts[item.LoopId].SwitchName;
                    }
                    else
                    {
                        loopName = "开关量输出 K" + item.LoopId;
                    }

                }
                else if (item.FaultId > 5 && item.FaultId < 18)
                {
                    loopName = mtpsss != null ? mtpsss.GetLoopName(item.LoopId) : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (loopName.Trim() == "")
                    {
                        loopName = "回路" + item.LoopId;

                    }

                }
                else if (item.FaultId >= 50 && item.FaultId < 80 && item.LoopId > 0)
                {
                    loopName = mtpsss != null ? mtpsss.GetLoopName(item.LoopId) : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (item.LampId > 0) loopName = loopName + "," + item.LampId;
                }
                else
                {

                    loopName = mtpsss != null ? mtpsss.GetLoopName(item.LoopId) : item.LoopId.ToString(CultureInfo.InvariantCulture);

                }

                obs.Add(new EquipmentFaultViewModel
                            {
                                DtCreateTime = item.DtErrFirstAlarm < 1 ? "--" : new DateTime(item.DtErrFirstAlarm).ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime = new DateTime(item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                Index = indexx,
                                FaultId = item.FaultId,
                                RtuId = item.RtuId,
                                PhyId = py,
                                RtuLoopName = loopName,
                                RtuLoops = item.LoopId,
                                RtuName = rtuname,//mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                                FaultName = GetFaultName(item.FaultId).Item1,
                                Color = GetFaultColor(item.FaultId),
                                Count = item .AlarmCount,
                                Remark = item.Remark,
                                //DateCreateId = item.DateRemove,
                                DateRemoveId = item.DateCreate,
                                LampId = item.LampId,
                                IsShowAtTop = GetFaultName(item.FaultId).Item2
                            });
               // if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo
            }

            var mtp = (from t in obs orderby t.DateCreateId ascending select t).ToList();//t.IsShowAtTop descending,
            
            Recordss.Clear();
            //添加区域筛选   lvf 2018年6月27日13:57:41
            FilterAreaErrs(mtp);
            //foreach (var f in mtp )
            //{
            //    indexx++;
            //    Recordss.Add(f);
            //    if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            //}
            CountNewErrs = false;
            //  Remind = "数据已反馈完毕，请查看数据！";
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + list.FaultItems.Count + " 条数据.";
        }


        public void OnRequestFaultPreForSingle(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var list = infos.WstFaultPreForSingle;
            ArgsInfoVisi = false;//统计故障时，隐藏单条具体参数信息
            if (_thisViewActive == false) return;
            if (list.FaultItems.Count ==0)
            {
                //CountLastPreErrs = true;
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，无数据.";
                UMessageBox.Show("提醒", "无数据,这是最后一条数据！", UMessageBoxButton.Ok);
                return;
            }
            //var list = obj as List<PreErrorItem>;
            //if (list == null) return;
            //  Records.Clear();
            var tmp = new Dictionary<Tuple<int, int, int>, int>();

           Records.Clear();
           Recordss.Clear();
           var obs = new ObservableCollection<EquipmentFaultViewModel>();
            string dateRemove = "";
            int indexx = 0;
            foreach (var item in list.FaultItems)
            {
                int count;
                //if (IsUseTimeLongQuery)
                //{
                if (item.DateRemove>0)
                {
                    dateRemove = new DateTime(item.DateRemove).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    dateRemove = "---";
                }
                var tu = new Tuple<int, int, int>(item.RtuId, item.LoopId, item.FaultId);
                if (tmp.ContainsKey(tu)) count = tmp[tu];
                else
                {
                    count =
                        (from t in list.FaultItems
                         where
                             t.RtuId == item.RtuId && t.LoopId == item.LoopId &&
                             t.FaultId == item.FaultId
                         select t).Count();
                    tmp.Add(tu, count);

                }
                indexx++;
                var mtpsss = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(item.RtuId);
                int py = item.RtuId;
                string rtuname = "";
                if (mtpsss != null)
                {
                    rtuname = mtpsss.RtuName;
                    if (mtpsss.RtuFid > 0)
                    {
                        var nfx =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                                mtpsss.RtuFid);
                        if (nfx != null)
                        {
                            rtuname = nfx.RtuName + "-" + mtpsss.RtuName;
                            py = nfx.RtuPhyId;
                        }
                    }
                    else py = mtpsss.RtuPhyId;
                }

                //var loopName = mtpsss != null
                //                                ? mtpsss.GetLoopName(item.LoopId)
                //                                  : item.LoopId.ToString(CultureInfo.InvariantCulture);
                //if (item.FaultId == 21 || item.FaultId == 20) loopName = "开关量输出K" + item.LoopId;

                string loopName = "";

                if (item.FaultId == 20 || item.FaultId == 21)
                {
                    var t =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[item.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    if (t.WjSwitchOuts.ContainsKey(item.LoopId))
                    {
                        loopName = t.WjSwitchOuts[item.LoopId].SwitchName;
                    }
                    else
                    {
                        loopName = "开关量输出 K" + item.LoopId;
                    }

                }
                else if (item.FaultId > 5 && item.FaultId < 18)
                {
                    loopName = mtpsss != null ? mtpsss.GetLoopName(item.LoopId) : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (loopName.Trim() == "")
                    {
                        loopName = "回路" + item.LoopId;

                    }

                }
                else if (item.FaultId >= 50 && item.FaultId < 80 && item.LoopId > 0)
                {
                    loopName = mtpsss != null ? mtpsss.GetLoopName(item.LoopId) : item.LoopId.ToString(CultureInfo.InvariantCulture);
                    if (item.LampId > 0) loopName = loopName + "," + item.LampId;
                }
                else
                {

                    loopName = mtpsss != null ? mtpsss.GetLoopName(item.LoopId) : item.LoopId.ToString(CultureInfo.InvariantCulture);

                }


                obs.Add(new EquipmentFaultViewModel
                            {
                                DtCreateTime = new DateTime(item.DateCreate).ToString("yyyy-MM-dd HH:mm:ss"),
                                DtRemoceTime =dateRemove,// new DateTime(item.DateRemove).ToString("yyyy-MM-dd HH:mm:ss"),
                                Index = indexx,
                                FaultId = item.FaultId,
                                RtuId = item.RtuId,
                                PhyId = py,
                                RtuLoopName = loopName,
                                RtuLoops = item.LoopId,
                                RtuName = rtuname,//mtpsss != null ? mtpsss.RtuName : GetRtuName(item.RtuId),
                                FaultName = GetFaultName(item.FaultId).Item1,
                                Color = GetFaultColor(item.FaultId),
                                Count = count,
                                Remark = item.Remark,
                                DateCreateId = item.DateCreate,
                                DateRemoveId = item.DateRemove,
                                LampId = item.LampId,
                                IsShowAtTop = GetFaultName(item.FaultId).Item2
                            });
               // if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent(); //todo
            }

            var mtp = (from t in obs orderby t.DateCreateId ascending select t).ToList();//t.IsShowAtTop descending,
            
            Records.Clear();
            Recordss.Clear();
            foreach (var f in mtp )
            {
                indexx++;
                Records.Add(f);
                Recordss.Add(f);
                if (indexx % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            }
            
            //  Remind = "数据已反馈完毕，请查看数据！";
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 查询成功，共" + list.FaultItems.Count + " 条数据.";
        }

        private readonly List<NameIntBool> _faultName = new List<NameIntBool>();

        private void GetAllFaultName()
        {
            foreach (var item in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                _faultName.Add(new NameIntBool { Name = item.Value.FaultNameByDefine, Value = item.Value.FaultId ,AreaId = item.Value.PriorityLevel});
            }
        }

        private Tuple<string,int> GetFaultName(int faultid)
        {
            foreach (var item in _faultName.Where(item => faultid == item.Value))
            {
                return new Tuple<string, int>(item.Name,item.AreaId);
            }
            return new Tuple<string, int>("no name", 0); 
        }


        private string GetFaultColor(int faultid)
        {
            var tmp = Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoById(faultid);
            if (tmp != null) return tmp.Color;
            return null;
        }
    }




    /// <summary>
    /// 操作类型模型定义
    /// </summary>
    public class OperatorTypeItem : ObservableObject
    {

        public event EventHandler OnIsSelectedChanged;


        private bool _check;
        public bool IsSelected
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    if (OnIsSelectedChanged != null)
                    {
                        OnIsSelectedChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        private bool _isShow;

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                if (_isShow != value)
                {
                    _isShow = value;
                    this.RaisePropertyChanged(() => this.IsShow);
                }
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
                foreach (var item in Value)
                {
                    item.IsSelected = _checkall;
                }

                RaisePropertyChanged(() => IsSelectedAll);
            }
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
    }







}

