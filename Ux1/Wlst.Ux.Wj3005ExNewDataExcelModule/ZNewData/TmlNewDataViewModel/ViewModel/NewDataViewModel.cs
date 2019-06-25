using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataViewModel.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.Services;
using Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel;
using Wlst.client;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataViewModel.ViewModel
{
    [Export(typeof(IINewDataViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataViewModel : IINewDataViewModel,Wlst .Cr .CoreMims .CoreInterface .IIShowData
    {
        private static NewDataViewModel _myself;
        public static NewDataViewModel Myself
        {
            get { return _myself; }
        }
        public static  int RowHeight = 25;
        public static int LoopNameLength = 120;
        public static int TimeNameLength = 120;
        public static int VaNameLength = 80;
        public static int KxNameLength = 125;

        // 选测终端id  lvf 2018年3月27日10:31:37
        public static int IntMeasureRtuid = 0;
        //选测时间  lvf 2018年3月27日10:31:20
        public static long LngMeasureTime = 0;
        //选测,判定失败的时间间隔 单位为秒  lvf 2018年3月27日10:31:03
        public static int IntMeasureOverTime = 0;

        //public static bool IsShowLoopId = false;
        //public static bool IsCompare = false;
        //public static bool IsDetailed = false;
        //public static bool IsOnlineRate = false;

        public static List<bool> IsShowPropoery = new List<bool>();

    
        /// <summary>
        /// 0 、序号
        /// 1 、回路名称
        /// 2 、参考电流
        /// 3、 亮灯率
        /// 4、 功率因数


        /// 5、 互感器比
        /// 6、 回路上限
        /// 7 、回路下限
        /// 8、 线路状态


        /// 9 、昨日数据
        /// 10、状态
        /// 11、电压
        /// 12、电流
        /// 13、功率

        /// 14、手动选测自动显示数据
        /// 15、显示回路数据电压电流等单位
        /// 16、历史数据查询显示高级选项
        /// 18 、功率
        /// 17、最新数据 屏蔽回路显示电流
        /// 19. 最新数据 屏蔽回路显示电压
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool GetShowIndex(int index)
        {
            if (IsShowPropoery.Count > index) return IsShowPropoery[index];
            return false;
        }


        public static int RtuNameLength = 500;
        public static string BackgroundColor = "Transparent";
        public static string K1BackgroundColor = "Transparent";
        public static string K2BackgroundColor = "Transparent";
        public static string K3BackgroundColor = "Transparent";
        public static string K4BackgroundColor = "Transparent";
        public static string K5BackgroundColor = "Transparent";
        public static string K6BackgroundColor = "Transparent";
        public static string K7BackgroundColor = "Transparent";
        public static string K8BackgroundColor = "Transparent";
        //public static bool IsShowDw = true;
        //public static bool OnMeasureShowData = false;
        //public static bool HsdataQueryShGjOp = false ;
        //public static bool ShowDw = true;
        #region tabTitle

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get
            {
                return  "最新数据";

            }
        }


        public bool CanClose
        {
            get { return false; }
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

        public int RtuIdNeedUpdate;
        public int HistoryDataResponse;
        public Dictionary<int, Tuple<int, bool, double>> AnaPara;

        ///// <summary>
        ///// 最新数据屏蔽回路显示电流电压
        ///// </summary>
        //public static bool ShieldLoopShA = true ;
        //public static bool ShieldLoopShV = true;

        #region OnlyTreeNodeChangeCanActiveNewData

        private bool _onlyTreeNodeChangeCanActiveNewData;

        /// <summary>
        /// 更新最新数据时 使用书节点焦点转移
        /// </summary>
        public bool OnlyTreeNodeChangeCanActiveNewData
        {
            get { return _onlyTreeNodeChangeCanActiveNewData; }
            set
            {
                if (value != _onlyTreeNodeChangeCanActiveNewData)
                {
                    _onlyTreeNodeChangeCanActiveNewData = value;
                    this.RaisePropertyChanged(() => this.OnlyTreeNodeChangeCanActiveNewData);
                }
            }
        }

        #endregion

        private Dictionary<int, Tuple<int, TmlNewData.TmlNewDataforOneLoop>> _historydata;
        public Dictionary<int, Tuple<int,TmlNewData.TmlNewDataforOneLoop>> HistoryData
        {
            get
            {
                if (_historydata == null)
                    _historydata = new Dictionary<int, Tuple<int,TmlNewData.TmlNewDataforOneLoop>>();
                return _historydata;
            }
        }

        

        private List<TmlNewData> _onlineRate;
        public List<TmlNewData> OnlineRate
        {
            get
            {
                if (_onlineRate == null)
                    _onlineRate = new List<TmlNewData>();
                return _onlineRate;
            }
        }


        private int rouheix;
        public int RowHightx
        {
            get { return rouheix; }
            set
            {
                if (value == rouheix) return;
                rouheix = value;
                this.RaisePropertyChanged(() => this. RowHightx);
            }
        }

        private int dataHEIGHT;
        public int DataHeiht
        {
            get
            {
                if (dataHEIGHT < 60) dataHEIGHT = 160;
                return dataHEIGHT;
            }
            set
            {
                if (value == dataHEIGHT+5) return;
                dataHEIGHT = value-5;
                this.RaisePropertyChanged(() => this.DataHeiht);

                DataHeihtx = dataHEIGHT - 90;// 45;
            }
        }


        private int dataHEIGHTx;
        public int DataHeihtx
        {
            get
            {
                if (dataHEIGHTx < 50) dataHEIGHTx = 50;
                return dataHEIGHTx;
            }
            set
            {
                if (value == dataHEIGHTx) return;
                dataHEIGHTx = value;
                this.RaisePropertyChanged(() => this.DataHeihtx);
            }
        }

        private Visibility _AssetManageInfoVisibility;

        /// <summary>
        /// 
        /// </summary>
        public Visibility AssetManageInfoVisibility
        {
            get { return _AssetManageInfoVisibility; }
            set
            {
                if (value != _AssetManageInfoVisibility)
                {
                    _AssetManageInfoVisibility = value;
                    this.RaisePropertyChanged(() => this.AssetManageInfoVisibility);
                }
            }
        }

        #region


        private string _DateTimeGetRtuTime;

        /// <summary>
        /// 获取到终端时间的本地时间
        /// </summary>
        public string DateTimeGetRtuTime
        {
            get { return _DateTimeGetRtuTime; }
            set
            {
                if (value != _DateTimeGetRtuTime)
                {
                    _DateTimeGetRtuTime = value;
                    this.RaisePropertyChanged(() => this.DateTimeGetRtuTime);
                }
            }
        }


        //private string  _DateTimeGetedRtuTime;

        ///// <summary>
        ///// 终端时间
        ///// </summary>
        //public string  DateTimeGetedRtuTime
        //{
        //    get { return _DateTimeGetedRtuTime; }
        //    set
        //    {
        //        if (value != _DateTimeGetedRtuTime)
        //        {
        //            _DateTimeGetedRtuTime = value;
        //            this.RaisePropertyChanged(() => this.DateTimeGetedRtuTime);
        //        }
        //    }
        //}

        private int _rtuId;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (value != _rtuId)
                {
                    _rtuId = value;

                    this.RaisePropertyChanged(() => this.RtuId);
                    if (
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .
                            ContainsKey(_rtuId))
                    {

                        this.RtuName = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [
                            _rtuId].RtuName;

                    }
                }
            }
        }

        private string _rtuName;

        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private string _sumInfo; // todo
        public string SumInfo
        {
            get { return _sumInfo; }
            set
            {
                if (value != _sumInfo)
                {
                    _sumInfo = value;
                    this.RaisePropertyChanged(() => this.SumInfo);
                }
            }
        }


        private string _timeInfo; // todo
        public string TimeInfo
        {
            get { return _timeInfo; }
            set
            {
                if (value != _timeInfo)
                {
                    _timeInfo = value;
                    this.RaisePropertyChanged(() => this.TimeInfo);
                }
            }
        }

        private Visibility _isShowFail;
        //xiansh
        public Visibility IsShowFailVis
        {
            get
            {
                return _isShowFail;
            }
            set
            {
                if (_isShowFail != value)
                {
                    _isShowFail = value;
                    this.RaisePropertyChanged(() => this.IsShowFailVis);
                }
            }
        }

        #endregion

        private bool isload = false;
        void acload()
        {
            isload = true;
        }
        public NewDataViewModel()
        {
            LoadFromHeightInfo();
            _myself = this;
           EventPublish.AddEventTokener( 
                Assembly.GetExecutingAssembly().GetName().ToString(), FundEventHandler,
                FundOrderFilter);

            Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(acload, 2, DelayEventHappen.EventOne);
            //var ii =
            //    new List<Tuple<int, bool, int>>();
            //ii.Add(new Tuple<int, bool, int>(1, true, 2));
            //ii.Add(new Tuple<int, bool, int>(2, false, 3));
            //ii.Add(new Tuple<int, bool, int>(3, true, 1));
            //var jj = new List<Tuple<int, int, bool, double, string, string, string>>();
            //jj.Add(new Tuple<int, int, bool, double, string, string, string>(1, 1, true, 220.1, "15", "12.3", "12"));
            //jj.Add(new Tuple<int, int, bool, double, string, string, string>(2, 1, true, 220.3, "18", "12.3", "12"));
            //jj.Add(new Tuple<int, int, bool, double, string, string, string>(3, 2, false , 220.8, "0", "0", "0"));
            //jj.Add(new Tuple<int, int, bool, double, string, string, string>(4, 2, true, 210.1, "115", "12.3", "12"));
            //jj.Add(new Tuple<int, int, bool, double, string, string, string>(5, 2, false , 223.1, "0", "0", "0"));
            //jj.Add(new Tuple<int, int, bool, double, string, string, string>(6, 3, true, 226.1, "44", "12.3", "12"));

            //this.DateTimeGetRtuTime = "2013-7-22 16:39:24";
            //this.RtuName = "10001 - 武宁路123号地址";
            //this.DateTimeGetedRtuTime = "2013-7-22 16:39:24 获取到终端时钟 2013-7-22 16:39:13";


            //var kk = new List<Tuple<int, bool, string>>();
            //kk.Add(new Tuple<int, bool, string>(8, true, "外箱门"));
            //kk.Add(new Tuple<int, bool, string>(9, false , "外箱门2"));

            //this.UpdateInfo(ii, jj, kk);
            var info = ZNewData.NewDataSetting.NewDataSettingViewModel.LoadNewDataLenghtSetConfgX();
            RowHeight = info.Item1;
            this.RowHightx = RowHeight;
            LoopNameLength = info.Item2;
            TimeNameLength = info.Item3;
            VaNameLength = info.Item4;
            RtuNameLength = info.Item5;
            KxNameLength = info.Item6.Item1;
         
            BackgroundColor = info.Item7.Background;
            K1BackgroundColor = info.Item7.K1Background;
            K2BackgroundColor = info.Item7.K2Background;
            K3BackgroundColor = info.Item7.K3Background;
            K4BackgroundColor = info.Item7.K4Background;
            K5BackgroundColor = info.Item7.K5Background;
            K6BackgroundColor = info.Item7.K6Background;
            K7BackgroundColor = info.Item7.K7Background;
            K8BackgroundColor = info.Item7.K8Background;
            
      
            InitIsShow(info.Item6);
            
            InitEventH();
            InitAction();
            LoadNewDataLenghtSetConfg();

           // VisiAdvancedData();
        }
        public event EventHandler<EventArsgLoopCount> LoopCountChanged;
        void InitIsShow(Tuple<int, int, int, int, int> data)
        {
            // 2-5
            ////item.Add(new NameIntBool() { IsSelected = true, AreaId = 0, Value = 1 }); //序号
            ////item.Add(new NameIntBool() { IsSelected = true, AreaId = 0, Value = 10 }); //回路名称
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 0, Value = 100 }); //参考电流
            ////item.Add(new NameIntBool() { IsSelected = true, AreaId = 0, Value = 1000 }); //亮灯率
            ////item.Add(new NameIntBool() { IsSelected = true, AreaId = 0, Value = 10000 }); //功率因数


            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 1, Value = 1 }); //互感器比
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 1, Value = 10 }); //回路上限
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 1, Value = 100 }); //回路下限
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 1, Value = 1000 }); //线路状态


            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 1 }); //昨日数据
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 10 }); //状态
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 100 }); //电压
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 1000 }); //电流
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 2, Value = 10000 }); //功率

            ////item.Add(new NameIntBool() { IsSelected = true, AreaId = 3, Value = 1 }); //手动选测自动显示数据
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 3, Value = 10 }); //显示回路数据电压电流等单位
            ////item.Add(new NameIntBool() { IsSelected = false, AreaId = 3, Value = 100 }); //历史数据查询显示高级选项


            //for (int i = 0; i < 4; i++)
            //{
            //    int xcount = 0;
            //    foreach (var f in Item)
            //    {
            //        if (f.AreaId == i)
            //        {
            //            xcount += f.Value;
            //        }
            //    }
            //    info.Add("IsShow" + i, xcount + "");
            //}

            IsShowPropoery.Clear();
            IsShowPropoery.AddRange(Getrrrrrrr(data.Item2, 5));
            IsShowPropoery.AddRange(Getrrrrrrr(data.Item3, 4));
            IsShowPropoery.AddRange(Getrrrrrrr(data.Item4, 5));
            IsShowPropoery.AddRange(Getrrrrrrr(data.Item5, 4));

            var tmprsfd = Getrrrrrrr(data.Item2, 6);
            IsShowPropoery.Add(tmprsfd[tmprsfd.Count - 1]);
            var tmprsffd = Getrrrrrrr(data.Item5, 5);
            IsShowPropoery.Add(tmprsffd[tmprsffd.Count - 1]);
            var tmprsffda = Getrrrrrrr(data.Item3, 5);
            IsShowPropoery.Add(tmprsffda[tmprsffda.Count - 1]);

            Wlst.Sr.EquipmentInfoHolding.Services.Others.HsdataQueryShGjOp = GetShowIndex(16);
            Wlst.Sr.EquipmentInfoHolding.Services.Others.NewdataShieldLoopShA = GetShowIndex(17);
            Wlst.Sr.EquipmentInfoHolding.Services.Others.NewdataShieldLoopShV = GetShowIndex(19);

            //lvf 2018年3月29日08:52:26 配置选项中可以设置,间隔时间为多久判定失败
            IntMeasureOverTime = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 21, 5);


            ////lvf  2018年4月11日15:41:46 判断是否为City特殊版本   4宁波 loopname 有更改
            //string path = Directory.GetCurrentDirectory() + "\\Config" + "\\" + "City.txt";
            
            //if (File.Exists(path))
            //{
            //    var sr = new StreamReader(path, Encoding.Default);
            //    CityNum = sr.ReadLine();

            //    sr.Close();
            //}
        }

        List<bool> Getrrrrrrr(int x, int number)
        {
            var rtn = new List<bool>();
            int data = x;
            for (int i = 0; i < number; i++)
            {
                rtn.Add(data%10 == 1);
                data = data/10;
            }
            return rtn;
        }


        public const string XmlConfigName = "CETC50_DemoAreaSet.xml";


         public  void   LoadNewDataLenghtSetConfg()
        {
            try
            {
                var info =
                    Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(Directory.GetCurrentDirectory() +
                                                                  "\\SystemColorAndFont\\" + XmlConfigName);
                if (info.ContainsKey("Area45Height")) DataHeiht  = Int32.Parse(info["Area45Height"]);
                else dataHEIGHT = 250;
            }
             catch (Exception ex)
             {
                 dataHEIGHT = 250;
             }
        }


        void InitAction()
        {
            ProtocolServer.RegistProtocol(
               Wlst.Sr.ProtocolPhone .LxRtu .wst_rtu_near_data ,// .wlst_svr_ans_cnt_request_wj3090_near_measure_data ,//.ProtocolCnt.ClientPart.wlst_Measures_server_ans_clinet_request_Near_data,
               OnNearDataArrive,
               typeof(NewDataViewModel), this);


            ProtocolServer.RegistProtocol(Sr.ProtocolPhone.LxRtu.wst_rtu_orders,//.wlst_svr_ans_cnt_request_snd_rtu_time,
                //.ClientPart.wlst_asyntime_server_ans_clinet_order_sendweeksetk1k3,
                                          OnRtuTimeArrive, typeof(NewDataViewModel), this);
            
            
        }

        void InitEventH()
        {
            ProtocolServer.RegistProtocol(
               Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_ana_data,// 请求给定时间段的指定时间的终端最新数据  智能分析  WstRtuAnaData
               HistoryDataArrive,
               typeof(NewDataViewModel), this);
            
        }
        


        /// <summary>
        /// 页面加载或是导航显示的时候 需要执行的初始化操作
        /// </summary>
        /// <param name="parsObjects"></param>
        public void NavOnLoad(params object[] parsObjects)
        {


            //ShieldLoopShA = ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(17);
            //ShieldLoopShV = ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(19);
        }

        /// <summary>
        /// 判断是否勾选“查看比对数据”和“查看详细数据”
        /// </summary>
        private void VisiAdvancedData(bool isHistory)
        {
            if (GetShowIndex(9 ) == true && isHistory ==false)
            {
                IsCompareCheck = Visibility.Visible;
            }
            else IsCompareCheck = Visibility.Collapsed;
            if (GetShowIndex( 5) == true) IsDetailCheck = Visibility.Visible;
            else IsDetailCheck = Visibility.Collapsed;
            if (GetShowIndex( 7) == true) IsOnlineRateCheck  = Visibility.Visible;
            else IsOnlineRateCheck = Visibility.Collapsed;

        }

        #region IEventAggregator Subscription
       // public static bool CurrentUserThisView = true;
        private void LoadFromHeightInfo()
        {
            try
            {
                var info =
                    Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(Directory.GetCurrentDirectory() +
                                                                  "\\SystemColorAndFont\\" + XmlConfigName);
                //if (info.ContainsKey("DataArea"))
                //{
                //    var tr = Int32.Parse(info["DataArea"]);
                //    if (tr == 5)
                //    {
                //        CurrentUserThisView = false;
                //    }
                //}

            }
            catch (Exception ex)
            {
               
            }
        }

        /// <summary>
        /// 事件过滤
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private bool FundOrderFilter(PublishEventArgs args)
        {
            if (isload == false) return false;

            if (args.EventType == PublishEventType.Core)
            {
                switch (args.EventId)
                {
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2 :

                        if (Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel.NewDataVmLeft .CurrentUserThisView  ) return false;

                         var lst = args.GetParams()[0] as List<int>;
                        if (lst == null || lst.Count == 0) return false ;
                        //lvf 2018年4月16日13:23:13    判断是否是用户最后一次点击的设备
                        //if (!lst.Contains(Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId)) return false;
                        if (lst.Contains(RtuIdNeedUpdate))
                        {
                            return true;
                        }
                        return false ;
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:
                        if (Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel.NewDataVmLeft.CurrentUserThisView) return false;
                        return true;
 

                    //case Sr .EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:
                    //    return true;
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab:
                        if (Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel.NewDataVmLeft.CurrentUserThisView) return false;
                        return true;
                        //case Sr.EquipmentNewData .Services .EventIdAssign .RtuTimeArrive :
                        //    return true;
                    
                }
            }
            if (args.EventType == "MainWindow.update.windowsset")
            {
                return true;
            }
            if (args.EventType == "MainWindow.Measure.show")
            {
                return true;
            } if (args.EventType == "MainWindow.Measure.close")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private void FundEventHandler(PublishEventArgs args)
        {

            try
            {
                ExExecuteEventIns(args);
                //return;
                //Async.Run(new Action<object>(ExExecuteEvent), args);
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("FundEventHandler No Dispatcher Error:" + ex);
            }
        }



        /// <summary>
        /// 线程执行 具体执行
        /// </summary>
        private void ExExecuteEventIns(PublishEventArgs args)
        {

            if (args.EventType == "MainWindow.Measure.show")
            {

                if (TmlNewDataVmLeft.ViewModel.NewDataVmLeft.CurrentUserThisView)
                {
                    ShowNewDataServices.ShowNewDataView(ViewIdAssign.TmlNewDataViewLeftId); 
                }
                else
                {
                    ShowNewDataServices.ShowNewDataView(ViewIdAssign.TmlNewDataViewId); 
                }
                return;
            }

            if (args.EventType == "MainWindow.Measure.close")  // lvf  2018年3月29日08:45:03  如果 接受到关闭最新数据框事件
            {
                

                if (TmlNewDataVmLeft.ViewModel.NewDataVmLeft.CurrentUserThisView)  // 关闭相应的最新数据框界面
                {
                    ShowNewDataServices.CloseNewDataView(ViewIdAssign.TmlNewDataViewLeftId);
                }
                else
                {
                    ShowNewDataServices.CloseNewDataView(ViewIdAssign.TmlNewDataViewId);
                }
                if (args.EventAttachInfo == null) return; // EventAttachInfo记录点击终端的ID
                IntMeasureRtuid = Convert.ToInt32(args.EventAttachInfo.ToString());
                LngMeasureTime = DateTime.Now.Ticks;   //记录选测时间
                //开启线程
                Wlst.Cr.Coreb.Servers.QtzLp.AddQtz("null", 8888, DateTime.Now.Ticks + IntMeasureOverTime * 10000000,50, ShowMeasureFail, LngMeasureTime,0);
                return;
            }
            try
            {
                switch (args.EventId)
                {
                    case 0:
                        if (args.EventType == "MainWindow.update.windowsset")
                        { 
                            //int heix = 0;
                            //if(Int32 .TryParse( args .EventAttachInfo .ToString() ,out heix ))
                            //{
                            //    DataHeiht = heix;
                            //}
                            DataHeiht = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(3301, 12, 250, "\\SystemColorAndFont");
                            ;
                        }

                        break;

              

                    case Sr . EquipmentInfoHolding.Services . EventIdAssign.RunningInfoUpdate2 :
                        var lst = args.GetParams()[0] as List<int>;
                        if (lst == null || lst.Count == 0) return;
                        //if (Infrastructure.DataHolding.Setting.IsOnlyTreeEventCanUpdateNewDataTab)
                        //    //OnlyTreeNodeChangeCanActiveNewData) //仅树选中能激活
                        //{
                        //    if (lst.Contains(RtuIdNeedUpdate))
                        //    {
                        //        OnSelectRtuIdChange(RtuIdNeedUpdate);
                        //    }
                        //    return;
                        //}

                        if (lst.Contains(IntMeasureRtuid)) IntMeasureRtuid =0;
                        if (lst.Contains(RtuIdNeedUpdate))
                        {
                            var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuIdNeedUpdate);
                            if (run == null || run.RtuNewData == null) return;
                            OnSelectRtuIdChange(RtuIdNeedUpdate,true  );   //弹框
                        }
                        //else
                        //{
                        //    OnSelectRtuIdChange(lst[0]);
                        //}
                        break;
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:

                        var rtuId = Convert.ToInt32(args.GetParams()[0]);
                        if (rtuId > 1600000 && rtuId < 1700000) return;
                        //Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId = rtuId;
                        if ( rtuId > 1100000)
                        {
                            
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(rtuId)) break;
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLine(rtuId)) break;
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsRtuIsLeak(rtuId)) break;
                        var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( rtuId);
                        if (tmps == null) return;
                        rtuId = tmps.RtuFid ;
                    }
                        if (rtuId > 1000000 && rtuId < 1100000)                         
                        {
                            if (Wlst.Ux.Wj3005ExNewDataExcelModule.ZNewData.TmlNewDataVmLeft.ViewModel.NewDataVmLeft.CurrentUserThisView)  return;
                            this.OnSelectRtuIdChange(rtuId,true );
                        }
                        break;

                    //case Sr.EquipmentInfoHolding.Services.EventIdAssign.HistoryDataUpdate:

                    //    var RtuId = Convert.ToInt32(args.GetParams()[0]);
                    //    var Run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuId);
                    //    if(Run!=null && Run.RtuNewData !=null && RtuId == RtuIdNeedUpdate )
                    //        OnDataChange(RtuId, Run.RtuNewData, "", false);
                    //    else OnDataChange(RtuId,null,"",false);
                        
                    //    HistoryDataResponse = 0;

                    //    break;

                    //case Sr.EquipmentInfoHolding.Services.EventIdAssign.MainEquipmentSelectedByOtherWay:

                    //    var rtuIds = Convert.ToInt32(args.GetParams()[0]);
                    //    if (rtuIds > 0)
                    //    {
                    //        this.OnSelectRtuIdChange(rtuIds);
                    //    }
                    //    break;

                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab:
                        try
                        {
                            var info = args.GetParams()[0] as Wlst.client.TmlNewData;
                            if (info == null) return;
                            OnOtherViewShowData(info);
                        }
                        catch (Exception ex)
                        {
                            
                        }

                        break;




                        //case Sr.EquipmentNewData.Services.EventIdAssign.RtuTimeArrive:
                        //    try
                        //    {
                        //        var rtuffd = Convert.ToInt32(args.GetParams()[0]);
                        //        if (rtuffd > 0 && rtuffd == this.RtuId)
                        //        {
                        //            //this.DateTimeGetRtuTime = DateTime.Now.ToString() + "  获取到终端时钟：";
                        //            //this.DateTimeGetedRtuTime = args.GetParams()[1].ToString();
                        //            this.AddRtuTime( DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  获取到终端时钟：" + args.GetParams()[1]);
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {

                        //    }
                        //    break;
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
            }
        }
        #endregion

        /// <summary>
        /// 等待一段时间后,判断是否选测是否应答;如果未应答
        /// </summary>
        private  void ShowMeasureFail(object obj)
        {
            var lastMeasureTime = (long) obj;
            if (lastMeasureTime != LngMeasureTime) return;
            if (IntMeasureRtuid == 0) return;

            if (TmlNewDataVmLeft.ViewModel.NewDataVmLeft.CurrentUserThisView)
            {
                ShowNewDataServices.ShowNewDataView(ViewIdAssign.TmlNewDataViewLeftId);
                
                //IntMeasureRtuid = 0;
            }
            else
            {
                ShowNewDataServices.ShowNewDataView(ViewIdAssign.TmlNewDataViewId);
                OnSelectRtuIdChange(IntMeasureRtuid, true, true);
                IntMeasureRtuid = 0;
            }
            
            
        }




        /// <summary>
        /// 终端点击时间  
        /// </summary>
        /// <param name="rtuId"></param>
        /// <param name="selected"></param>
        /// <param name="showFail">是否需要红色标识出选测失败  吕峰 2018年3月27日09:28:21</param>
        void OnSelectRtuIdChange(int rtuId, bool selected,bool showFail = false )
        {
            IsShowFailVis = showFail?Visibility.Visible : Visibility.Collapsed;
            this.RtuIdNeedUpdate = rtuId;
            var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuIdNeedUpdate);
            if (run == null || run.RtuNewData == null)
            {
                //if(showFail)
                //{
                    OnDataChange(rtuId, null, "", false);
                    //IsShowFailVis = Visibility.Visible;
                    return;
                //}
                OnDataChange(rtuId, null, "", false);
                return;
            }
            //if( showFail)
            //{
            //    OnDataChange(rtuId, run.RtuNewData, "", false);
            //    IsShowFailVis = Visibility.Visible;
            //}
            //else
            //{
                OnDataChange(rtuId, run.RtuNewData, "", false);
            //}

            if (GetShowIndex( 9)) RequestHistoryData(rtuId, run.RtuNewData.DateCreate.AddDays(-1));
            if (selected)
                Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
                    ViewIdAssign.TmlNewDataViewId);
        }


        private RtuNewDataInfo CurrentShowTmlNewData = null;
         void OnOtherViewShowData(Wlst.client.TmlNewData data)
         {
             try
             {
                 if (data.LstNewLoopsData.Count == 0) return;

                 //CurrentShowTmlNewData = data;

                 this.RtuIdNeedUpdate = data.RtuId;
                 var fff = new RtuNewDataInfo(data);
                 //if (fff.LstNewLoopsData == null) return;

                 OnDataChange(data.RtuId, fff, "历史数据",true );
                 Wlst.Cr.CoreMims.Services.ShowNewDataServices.ShowNewDataView(
                ViewIdAssign.TmlNewDataViewId);
             }
             catch (Exception ex)
             {

             }
         }

         private int Get_TransferState(int rtuId)
         {
             var lst = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();

             if (lst.Count != 0)
             {
                 foreach (var t in lst)
                 {
                     if (t.Value.RtuId == rtuId)
                     {
                         return t.Value.IsYj;
                     }
                 }
             }

             return 0;
         }

         private Tuple<int, long, long> _lastOnDataChangeRtuwithtime = new Tuple<int, long, long>(0, 0, 0);
         void OnDataChange(int rtuId, RtuNewDataInfo fff, string attachinfo, bool isHistory)
         {
             //ShieldLoopShA = ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(17);
             //ShieldLoopShV = ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.GetShowIndex(19);
             VisiAdvancedData(isHistory);
             //优化性能  当当前选中的点和上一次选中的点一致  同时上一次刷新在0.5秒内不再执行
             this.TimeInfo = "";
             if (rtuId == _lastOnDataChangeRtuwithtime.Item1)
             {
                 if (fff != null && fff.DateCreate.Ticks == _lastOnDataChangeRtuwithtime.Item3)
                     return;
                 //if (DateTime.Now.Ticks - _lastOnDataChangeRtuwithtime.Item2 < 15000000)
                 //    return;
             }
             ////lvf 2018年4月16日13:23:13    判断是否是用户最后一次点击的设备
             //if (rtuId != Wlst.Sr.EquipmentInfoHolding.Services.Others.CurrentSelectRtuId) return;
             _lastOnDataChangeRtuwithtime = new Tuple<int, long, long>(rtuId, DateTime.Now.Ticks,
                                                                       fff == null ? 0 : fff.DateCreate.Ticks);

             var lineItems = new ObservableCollection<LineInfo>();

             var textBlockInfoItems = new ObservableCollection<TextBlockInfo>();

             var textBlock1InfoItems = new ObservableCollection<TextBlock1Info>();
             var anaPara = new Dictionary<int, Tuple<int, int, double>>();
             var looxInfo = new ObservableCollection<LoopInfox>();
             Visifd = Visibility.Collapsed;
             IsDataVisi = Visibility.Collapsed;


             try
             {

                 CurrentShowTmlNewData = fff;

                 var rtuState = "";
                 //this.RtuName = this.RtuId+""

                 ;
                 int phyId = 0;
                 var tmpequ = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(rtuId);
                 var tmpequ2 = tmpequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                 if (tmpequ != null)
                 {
                     rtuState = tmpequ.RtuStateCode == 2 ? "使用" : tmpequ.RtuStateCode == 1 ? "停运" : "不用";
                     //this.RtuName = tmpequ.RtuName;
                     phyId = tmpequ.RtuPhyId;
                 }
                 if (tmpequ2 != null && tmpequ2.WjLoops != null)
                 {
                     foreach (var t in tmpequ2.WjLoops)
                     {
                         anaPara.Add(t.Value.LoopId,
                                     new Tuple<int, int, double>(t.Value.MutualInductorRatio, t.Value.IsShieldLoop,
                                                                 t.Value.ShieldLittleA));
                         GetAnaPara(anaPara);
                     }
                 }

                 NameValueInt TransferState = new NameValueInt();

                 if (isHistory == false)
                 {
                     //if (Wlst.Sr.EquipmentInfoHolding.Services.Others.IsAllowAssetManageInfo == true)
                     {
                         int _index = Get_TransferState(rtuId);
                         string _name = string.Empty;

                         //if (_index == 1)
                         //{
                         //    //_name = "已移交";
                         //}
                         if (_index == 2)
                         {
                             _name = "未移交";

                             AssetManageInfoVisibility = Visibility.Visible;
                             TransferState = new NameValueInt() {Name = _name, Value = _index};
                         }
                         else
                         {
                             AssetManageInfoVisibility = Visibility.Hidden;
                         }




                     }
                     //else
                     //{

                     //}
                 }
                 else
                 {
                     TransferState = new NameValueInt() {Name = "历史", Value = 2};

                     AssetManageInfoVisibility = Visibility.Visible;
                 }

                 this.RtuIdNeedUpdate = rtuId;
                 this.RtuId = rtuId;

                 var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuIdNeedUpdate);
                 int rtuTemp = 0;
                 Tuple<int, int> onlineRate = new Tuple<int, int>(-1, 1);


                 if (run != null && run.RtuNewData != null)
                 {
                     rtuTemp = run.RtuNewData.RtuTemperature;
                     onlineRate = new Tuple<int, int>(run.RtuNewData.TimesBackPartolIn24Hour,
                                                      run.RtuNewData.TimesPartolIn24Hour);


                     foreach (var t in run.RtuNewData.LstNewLoopsData)
                     {
                         foreach (var f in fff.LstNewLoopsData)
                         {
                             if (t.LoopId == f.LoopId)
                             {
                                 f.AvgOf7daysA = t.AvgOf7daysA;
                                 break;
                             }
                         }
                     }
                 }

                 var rtuName = tmpequ != null ? tmpequ.RtuName : "";
                 string GroupName = "";
                 string AreaName = "";

                 string color2 = "";
                 var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuId);
                 AreaName = (UserInfo.UserLoginInfo.D == false && UserInfo.UserLoginInfo.AreaR.Count < 2)
                                ? ""
                                : Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[areaId].AreaName;
                 if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 1) AreaName = "";

                 string Error = "";
                 var lst = Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values;
                 foreach (var x in lst)
                 {
                     if (x.RtuId == rtuId)
                     {
                         Error = x.FaultName;

                     }

                 }

                 if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 15, 1) == 2)
                 {
                     var groupidx =
                         Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(RtuId);
                     //.Item2;
                     //Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(RtuId);
                     if (groupidx != null)
                     {
                         var infosss =
                             Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(
                                 groupidx.Item1, groupidx.Item2);
                         if (infosss != null) GroupName = infosss.GroupName + "  " + Error;
                     }
                 }
                 else if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionInt(2801, 15, 1) == 3)
                 {
                     GroupName = GroupName + "  " + tmpequ.RtuRemark;
                 }


                 //  var fff = Sr.EquipmentNewData.Services.RtuNewDataService.GetInfoById(rtuId);
                 if (fff == null)
                 {

                     IsDataVisi = Visibility.Collapsed;
                     this.LineItemss.Clear();
                     this.TextBlockInfoItemss.Clear();
                     this.LoopxInfo.Clear();
                     this.SumInfo = "";
                     this.TimeInfo = "";
                     CanWidth = 355 + LoopNameLength + TimeNameLength + 4*VaNameLength;
                     CanHeight = 8*RowHeight + 65;
                     this.RtuId = rtuId;
                     //this.RtuName = this.RtuId + " - " + rtuName ;
                     this.AddBasicRtuInfo(ref lineItems, ref textBlockInfoItems,
                                          phyId.ToString("D4") + " - " + rtuName + "  " + AreaName + "  " + GroupName,
                                          " " + "  " + rtuState, rtuTemp, TransferState);

                     //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
                     var swuout = new List<Tuple<int, bool, int, string, string>>();
                     //添加输出
                     //lvf
                     ConstColor = GetColor();
                     if (tmpequ2.RtuModel == EnumRtuModel.Wj3006)
                     {
                         for (int i = 1; i < 9; i++)
                         {
                             swuout.Add(new Tuple<int, bool, int, string, string>(i, false, 0, ConstColor[i], ""));
                         }
                     }
                     else
                     {
                         for (int i = 1; i < 7; i++)
                         {
                             swuout.Add(new Tuple<int, bool, int, string, string>(i, false, 0, ConstColor[i], ""));
                         }
                     }

                     AddSitchOutInfo(ref textBlockInfoItems, ref textBlock1InfoItems, ref lineItems, rtuId, swuout,
                                     isHistory);



                     this.LineItemss = lineItems;
                     this.TextBlockInfoItemss = textBlockInfoItems;
                     this.TextBlock1InfoItemss = textBlock1InfoItems;
                     this.BulidMenus(rtuId);
                     //RunDispatch(new Tuple<int, Action<Tuple<ObservableCollection<MenuItem>, string>>>(rtuId, UpdateMenu));
                     return;
                 }

                 //var rtuState = "";
                 //this.RtuName = this.RtuId+""
                 var title = "";
                 bool powerOff = true;
                 if (fff.LstNewLoopsData == null) title += "正常";
                 else
                 {
                     int xcoutn = 0;
                     foreach (var g in fff.LstNewLoopsData)
                     {
                         xcoutn++;
                         if (g.V > 1)
                         {
                             powerOff = false;
                             break;
                         }
                     }
                     if (xcoutn == 0)
                     {
                         if (fff.Alarms.ContainsKey(1) && fff.Alarms[1]) title += "断电";
                         else title += "供电";
                     }
                     else
                     {
                         //if (fff.Alarms.ContainsKey(1) && fff.Alarms[1]) title += "停电";
                         if (powerOff) title += "停电";

                         else
                         {
                             if (fff.Alarms.ContainsKey(1) && fff.Alarms[1]) title += "断电";
                             else title += "供电";
                         }
                     }
                 }

                 //if (fff.Alarms.ContainsKey(1) && fff.Alarms[1]) title += "停电";
                 //else title += "供电";
                 if (fff.Alarms.ContainsKey(3) && fff.Alarms[3]) title += "停运中";
                 else title += "使用中 ";
                 if (tmpequ != null)
                 {
                     rtuState = tmpequ.RtuStateCode == 0 ? "不用" : title;
                     //this.RtuName = tmpequ.RtuName;
                 }

                 if (fff.LstNewLoopsData == null)
                 {

                     this.LineItemss.Clear();
                     this.TextBlockInfoItemss.Clear();
                     this.LoopxInfo.Clear();
                     this.TimeInfo = "";

                     CanWidth = 365 + LoopNameLength + TimeNameLength + 4*VaNameLength;
                     CanHeight = 8*RowHeight + 65;
                     this.RtuId = fff.RtuId;
                     this.DateTimeGetRtuTime = fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                     //this.RtuName = this.RtuId + " - " + fff.RtuName;
                     this.AddBasicRtuInfo(ref lineItems, ref textBlockInfoItems,
                                          phyId.ToString("D4") + " - " + rtuName + "  " + AreaName + "  " + GroupName,
                                          fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "  " + rtuState, rtuTemp,
                                          TransferState);


                     //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
                     var swuout = new List<Tuple<int, bool, int, string, string>>();
                     //添加输出
                     ConstColor = GetColor(); //lvf
                     for (int i = 1; i < 9; i++)
                     {
                         swuout.Add(new Tuple<int, bool, int, string, string>(i, false, 0, ConstColor[i], ""));
                     }
                     AddSitchOutInfo(ref textBlockInfoItems, ref textBlock1InfoItems, ref lineItems, rtuId, swuout,
                                     isHistory);

                     IsDataVisi = Visibility.Collapsed;
                     this.LineItemss = lineItems;
                     this.TextBlockInfoItemss = textBlockInfoItems;
                     this.TextBlock1InfoItemss = textBlock1InfoItems;
                     //RunDispatch(new Tuple<int, Action<Tuple<ObservableCollection<MenuItem>, string>>>(rtuId, UpdateMenu));
                     this.BulidMenus(rtuId);
                     return;
                 }

                 //RequestHistoryData(rtuId, fff.DateCreate.AddDays(-1));
                 //InitEventH();



                 this.DateTimeGetRtuTime = fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");

                 var dic = new Dictionary<int, Tuple<string, List<RtuNewDataLoopItem>>>();

                 for (int i = 1; i < fff.IsSwitchOutAttraction.Count + 1; i++)
                 {
                     if (dic.ContainsKey(i)) continue;
                     dic.Add(i, new Tuple<string, List<RtuNewDataLoopItem>>("", new List<RtuNewDataLoopItem>()));

                 }
                 foreach (var t in fff.LstNewLoopsData)
                 {
                     if (!dic.ContainsKey(t.SwitchOutId))
                         dic.Add(t.SwitchOutId,
                                 new Tuple<string, List<RtuNewDataLoopItem>>("", new List<RtuNewDataLoopItem>()));
                     dic[t.SwitchOutId].Item2.Add(t);
                 }




                 this.AddBasicRtuInfo(ref lineItems, ref textBlockInfoItems,
                                      phyId.ToString("D4") + " - " + rtuName + "  " + AreaName + "  " + GroupName,
                                      fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "  " + rtuState, rtuTemp,
                                      TransferState);


                 CanWidth = 365 + LoopNameLength + TimeNameLength + 4*VaNameLength;
                 //  CanHeight = fff.LstNewLoopsData.Count * RowHeight  + 30  ; ;
                 CanHeight = fff.LstNewLoopsData.Count*RowHeight + RowHeight +
                             (dic.Keys.Contains(0) ? dic.Count*10 - 10 : dic.Count*10); //CanHeight
                 if (CanHeight < 230) CanHeight = 230;


                 //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
                 List<Tuple<int, bool, int, string, string>> swout = new List<Tuple<int, bool, int, string, string>>();


                 //添加输出
                 var fffff = (from t in dic orderby t.Key select t).ToList();
                 ConstColor = GetColor();
                 foreach (var t in fffff)
                 {
                     if (t.Key < 1) continue;
                     bool isclose = true;
                     if (fff.IsSwitchOutAttraction.Count >= t.Key)
                     {
                         isclose = fff.IsSwitchOutAttraction[t.Key - 1];
                     }
                     //lvf
                     string a = "";
                     foreach (var x in tmpequ2.WjSwitchOuts)
                     {
                         if (x.Key == t.Key)
                         {
                             if (x.Value.SwitchName.Contains("开关量输出"))
                             {
                                 var tmp = x.Value.SwitchName;
                                 int ab = tmp.IndexOf("K");
                                 x.Value.SwitchName = tmp.Substring(ab, 2);
                             }

                             a = x.Value.SwitchName;
                         }


                     }
                     swout.Add(new Tuple<int, bool, int, string, string>(t.Key, isclose, t.Value.Item2.Count,
                                                                         ConstColor[t.Key], a));

                 }
                 AddSitchOutInfo(ref textBlockInfoItems, ref textBlock1InfoItems, ref lineItems, rtuId, swout, isHistory);


                 //添加回路


                 var dicattach = new Dictionary<int, bool>();
                 var dicattachName = new Dictionary<int, string>();

                 var strinfoxfds = GetShowIndex(15) ? "[A]" : "";
                 var isABC = true;
                 foreach (var f in fff.LstNewLoopsData)
                 {
                     if (f.LoopName.Contains("火线"))
                     {
                         isABC = false;
                         break;
                     }

                 }

                 //lvf 2018年8月17日08:45:30  完善
                 if (isABC) //三相
                 {

                     double PA = 0.00, PB = 0.00, PC = 0.00;
                     if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(RtuId))
                     {
                         var info =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId] as
                            Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                         foreach (var t in fff.LstNewLoopsData)
                         {
                             if (info != null && (t.IsLoop && info.WjLoops.ContainsKey(t.LoopId)))
                             {
                                 //屏蔽回路不参与累计   lvf 2018年9月11日08:51:25 老孟提议
                                 if (info.WjLoops[t.LoopId].IsShieldLoop == 1 || info.WjLoops[t.LoopId].IsShieldLoop == 2) continue;

                                 switch (info.WjLoops[t.LoopId].VoltagePhaseCode)
                                 {
                                     case Wlst.client.EnumVoltagePhase.Aphase:
                                         PA += t.Power;
                                         break;
                                     case Wlst.client.EnumVoltagePhase.Bphase:
                                         PB += t.Power;
                                         break;
                                     case Wlst.client.EnumVoltagePhase.Cphase:
                                         PC += t.Power;
                                         break;
                                 }
                             }
                         }
                     }
                     double totalPower = (fff.RtuVoltageA * fff.RtuCurrentSumA + fff.RtuVoltageB * fff.RtuCurrentSumB +
                                          fff.RtuVoltageC * fff.RtuCurrentSumC) / 1000;

                     var pn = (PA + PB + PC)/totalPower;
                     if(pn>1) pn = 1;

                     this.SumInfo = "";
                     this.SumInfo = "A相:" + string.Format("{0:0.00}", fff.RtuCurrentSumA) + strinfoxfds;
                     this.SumInfo += "   ";
                     this.SumInfo += "B相:" + string.Format("{0:0.00}", fff.RtuCurrentSumB) + strinfoxfds;
                     this.SumInfo += "   ";
                     this.SumInfo += "C相:" + string.Format("{0:0.00}", fff.RtuCurrentSumC) + strinfoxfds;
                     this.SumInfo += "   ";
                     this.SumInfo += "总功" + string.Format("{0:0.00}", PA + PB + PC);
                     this.SumInfo += "   ";
                     this.SumInfo += "总因" + (totalPower != 0 ? string.Format("{0:0.00}",pn ) : "--");
                     this.SumInfo += "  ";

                 }
                 else //火零
                 {
                     double FireCur = 0.00, ZeroCur = 0.00, AllPower = 0.00, AllUI = 0.00;
                     if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(RtuId))
                     {
                         var info =
                            Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId] as
                            Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                         foreach (var t in fff.LstNewLoopsData)
                         {
                             if (info != null && (t.IsLoop && info.WjLoops.ContainsKey(t.LoopId)))
                             {

                                 //屏蔽回路不参与累计   lvf 2018年9月11日08:51:25 老孟提议
                                 if (info.WjLoops[t.LoopId].IsShieldLoop == 1 || info.WjLoops[t.LoopId].IsShieldLoop == 2) continue;
                                 if (t.LoopName.Contains("零线") == false)
                                 {
                                     FireCur += t.A;
                                     AllPower += t.Power;
                                     AllUI += t.A * t.V;

                                 }
                                 else
                                 {
                                     ZeroCur += t.A;
                                 }

                             }
                         }
                     }
                     var powerNum = AllPower*1000/AllUI;
                     if (powerNum > 1) powerNum = 1;

                     this.SumInfo = "";
                     this.SumInfo = "火总" + string.Format("{0:0.00}", FireCur) + strinfoxfds;
                     this.SumInfo += "   ";
                     this.SumInfo += "零总" + string.Format("{0:0.00}", ZeroCur) + strinfoxfds;
                     this.SumInfo += "   ";
                     this.SumInfo += "总功" + string.Format("{0:0.00}", AllPower);
                     this.SumInfo += "   ";
                     this.SumInfo += " 总因" + (AllUI != 0 ? string.Format("{0:0.00}", powerNum) : "--");//AllPower * 1000 / AllUI
                     this.SumInfo += "   ";



                 }
                 //double PA = 0.00, PB = 0.00, PC = 0.00;
                 //if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(RtuId))
                 //{
                 //    var info =
                 //        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId] as
                 //        Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                 //    foreach (var t in fff.LstNewLoopsData)
                 //    {
                 //        if (info != null && (t.IsLoop && info.WjLoops.ContainsKey(t.LoopId)))
                 //        {
                 //            switch (info.WjLoops[t.LoopId].VoltagePhaseCode)
                 //            {
                 //                case Wlst.client.EnumVoltagePhase.Aphase:
                 //                    PA += t.Power;
                 //                    break;
                 //                case Wlst.client.EnumVoltagePhase.Bphase:
                 //                    PB += t.Power;
                 //                    break;
                 //                case Wlst.client.EnumVoltagePhase.Cphase:
                 //                    PC += t.Power;
                 //                    break;
                 //            }
                 //        }
                 //    }
                 //}
                 //double totalPower = (fff.RtuVoltageA*fff.RtuCurrentSumA + fff.RtuVoltageB*fff.RtuCurrentSumB +
                 //                     fff.RtuVoltageC*fff.RtuCurrentSumC)/1000;
                 //this.SumInfo = "";
                 //this.SumInfo = (isABC ? "A相:" : "火总") + string.Format("{0:0.00}", fff.RtuCurrentSumA) + strinfoxfds;
                 ////string.Format("{0:0.00}", v)
                 //this.SumInfo += "   ";
                 //this.SumInfo += "总功" + string.Format("{0:0.00}", PA + PB + PC);
                 //this.SumInfo += "   ";
                 //this.SumInfo += (isABC ? "B相:" : "零总") + string.Format("{0:0.00}", fff.RtuCurrentSumB) + strinfoxfds;
                 //this.SumInfo += "   ";
                 //this.SumInfo += "总因" + (totalPower != 0 ? string.Format("{0:0.00}", (PA + PB + PC)/totalPower) : "--");
                 //this.SumInfo += "   ";
                 //this.SumInfo += isABC ? ("C相:" + string.Format("{0:0.00}", fff.RtuCurrentSumC) + strinfoxfds) : "";
                 //this.SumInfo += "  ";

                 //this.SumInfo = "";
                 if (tmpequ != null)
                 {
                     foreach (var g in tmpequ.EquipmentsThatAttachToThisRtu)
                     {
                         var attrtuInfo = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(g);
                         if (attrtuInfo == null || attrtuInfo.EquipmentType != WjParaBase.EquType.Ldu) continue;
                         var wjldu = attrtuInfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj1090Ldu;
                         if (wjldu == null) continue;
                         foreach (var f in wjldu.WjLduLines)
                         {
                             if (f.Value.IsUsed == false) continue;
                             var errors =
                                 (from t in
                                      Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary
                                  where
                                      t.Value.RtuId == g && t.Value.LoopId == f.Value.LduLineId &&
                                      t.Value.IsThisUserShow
                                  select t).ToList().Count;


                             if (dicattach.ContainsKey(f.Value.LduLoopId) == false)
                             {
                                 dicattach.Add(f.Value.LduLoopId, errors > 0);
                                 dicattachName.Add(f.Value.LduLoopId, f.Value.LduLineName);
                             }
                         }
                     }
                 }
                 ConstColor = GetColor();

                 this.IsLdlAs100Per = LoadXmldata(); //crc


                 foreach (var t in fffff)
                 {
                     if (t.Key < 1) continue;
                     string color = ConstColor[t.Key];
                     var ntgs = (from f in t.Value.Item2 orderby f.LoopId ascending select f).ToList();
                     foreach (var g in ntgs)
                     {

                         double v = 0;
                         double a = 0;
                         double power = 0;
                         double rate = 0;
                         string upper = "";
                         string lower = "";
                         double refA = 0;
                         if (IsLdlAs100Per && g.A > 0) //crc
                             rate = 1.00;
                         else
                             rate = g.BrightRate;
                         try
                         {
                             v = Convert.ToDouble(g.V);
                             a = Convert.ToDouble(g.A);
                             power = Convert.ToDouble(g.Power);
                             refA = Convert.ToDouble(g.AvgOf7daysA);
                             upper = g.Upper.ToString("f2");
                             lower = g.Lower.ToString("f2");


                         }
                         catch (Exception ex)
                         {
                         }
                         ;

                         if (dicattach.ContainsKey(g.LoopId))
                         {

                             AddLoopInfox(ref looxInfo,
                                          g.LoopId, g.LoopName, g.BolSwitchInState, v, a,
                                          power,
                                          rate, upper, lower, anaPara, refA, onlineRate, color, g.Range > 0,
                                          isHistory, dicattach[g.LoopId] ? "被盗" : "正常", dicattachName[g.LoopId],
                                          dicattach[g.LoopId] ? "Red" : color);


                         }
                         if (!dicattach.ContainsKey(g.LoopId))
                         {
                             AddLoopInfox(ref looxInfo,
                                          g.LoopId, g.LoopName, g.BolSwitchInState, v, a,
                                          power,
                                          rate, upper, lower, anaPara, refA, onlineRate, color, g.Range > 0, isHistory,
                                          " ", " ", color);

                         }

                     }

                 }



                 //添加输入 入门
                 foreach (var t in fffff)
                 {
                     if (t.Key != 0) continue;
                     var loopDetect = new List<Tuple<string, string>>();
                     var loopDetect2 = new List<string>();
                     foreach (var g in t.Value.Item2)
                     {
                         string tmpName = "";
                         if (g.LoopName.Contains("门")) continue;
                         if (g.LoopName.Contains("K")) continue;
                         if (g.LoopName.Contains("A"))
                         {
                             loopDetect.Add(new Tuple<string, string>(g.LoopName.Split('A')[0], g.LoopName.Split('A')[1]));
                             tmpName = g.LoopName.Split('A')[1];
                         }
                         if (g.LoopName.Contains("防盗"))
                         {
                             loopDetect2.Add(g.LoopName.Substring(0, g.LoopName.IndexOf("防盗")));
                             tmpName = g.LoopName.Substring(0, g.LoopName.IndexOf("防盗"));
                         }
                         if (g.LoopName.Contains("B") &&
                             loopDetect.Contains(new Tuple<string, string>(g.LoopName.Split('B')[0],
                                                                           g.LoopName.Split('B')[1]))) continue;
                         if (g.LoopName.Contains("检测器") &&
                             loopDetect2.Contains(g.LoopName.Substring(0, g.LoopName.IndexOf("检测器")))) continue;
                         double v = 0;
                         double a = 0;
                         double power = 0;
                         double rate = 0;
                         string upper = "";
                         string lower = "";
                         double refA = 0;
                         string color3 = "Gray";
                         string status = "正常";
                         foreach (var x in lst)
                         {
                             if (x.RtuId == rtuId && x.RtuLoopName.Contains(tmpName))
                             {
                                 color3 = "Red";
                                 status = x.FaultName.Length < 3 ? x.FaultName : "报警";
                                 break;
                             }
                         }
                         try
                         {
                             v = Convert.ToDouble(g.V);
                             a = Convert.ToDouble(g.A);
                             power = Convert.ToDouble(g.Power);
                             refA = Convert.ToDouble(g.AvgOf7daysA);
                             upper = g.Upper.ToString("f2");
                             lower = g.Lower.ToString("f2");
                             rate = g.BrightRate;
                         }
                         catch (Exception ex)
                         {
                         }

                         AddSwitchInInfo(ref looxInfo, g.LoopId, g.LoopName, status,
                                         color3);



                     }

                     var tmps = (from g in t.Value.Item2 orderby g.LoopId select g).ToList();
                     for (int i = 0; i < tmps.Count; i++)
                     {
                         //string color = ConstColor[t.Key];
                         //if (i < tmps.Count - 1 && tmps[i].LoopName.Contains("防盗") &&
                         //    tmps[i + 1].LoopName.Contains("检测器"))
                         //{
                         //    if (tmps[i].BolSwitchInState != tmps[i + 1].BolSwitchInState)
                         //    {
                         //        AddSwitchInInfo(ref looxInfo, tmps[i].LoopId, tmps[i].LoopName, "正常",
                         //                        "Gray");

                         //    }
                         //    else
                         //    {
                         //        AddSwitchInInfo(ref looxInfo, tmps[i].LoopId,
                         //                        tmps[i].LoopName, "异常",
                         //                        "Red");

                         //    }


                         //    continue;
                         //}
                         if (tmps[i].LoopName.Contains("门"))
                         {
                             var tmpssss = "正常";
                             if (tmps[i].BolSwitchInState == false) tmpssss = "打开";
                             AddSwitchInInfo(ref looxInfo, tmps[i].LoopId,

                                             tmps[i].LoopName, tmpssss,
                                             tmps[i].BolSwitchInState ? "Gray" : "Red");
                             continue;
                         }

                         // todo
                         //int x = Wlst.Cr.Core.CoreServices.SystemOption.GetOption(4);

                         if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum == 4) //4为宁波
                         {
                             if (tmps[i].LoopName.Contains("K"))
                             {
                                 string tmpssss = "";
                                 var rtuInfo =
                                     Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                                         rtuId]
                                     as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                                 bool alwayClose = rtuInfo.WjLoops[tmps[i].LoopId].IsSwitchStateClose;


                                 if (tmps[i].BolSwitchInState == false && alwayClose) tmpssss = "断开";
                                 if (tmps[i].BolSwitchInState && alwayClose) tmpssss = "吸合";
                                 if (tmps[i].BolSwitchInState == false && alwayClose == false) tmpssss = "吸合";
                                 if (tmps[i].BolSwitchInState && alwayClose == false) tmpssss = "断开";

                                 AddSwitchInInfo(ref looxInfo, tmps[i].LoopId,

                                                 tmps[i].LoopName, tmpssss,
                                                 tmps[i].BolSwitchInState ? "Gray" : "Red");
                             }

                         }
                         else
                         {
                             if (tmps[i].LoopName.Contains("K"))
                             {
                                 var tmpssss = "正常";
                                 if (tmps[i].BolSwitchInState == false) tmpssss = "报警";
                                 AddSwitchInInfo(ref looxInfo, tmps[i].LoopId,

                                                 tmps[i].LoopName, tmpssss,
                                                 tmps[i].BolSwitchInState ? "Gray" : "Red");
                             }
                         }

                     }
                 }

             }
             catch (Exception ex)
             {
                 WriteLog.WriteLogError("On NewDataView change rtu error:" + ex);
             }

             //this.LoopxInfo = looxInfo;
             updateloopinfo(looxInfo);
             this.LineItemss = lineItems;
             this.TextBlockInfoItemss = textBlockInfoItems;
             this.TextBlock1InfoItemss = textBlock1InfoItems;
             LineItemsDash.Clear();
             this.BulidMenus(rtuId);
             if (LoopCountChanged != null)
                 LoopCountChanged(this,
                                  new EventArsgLoopCount() {IsShowPro = IsShowPropoery, LoopCount = looxInfo.Count});

             //    RunDispatch(new Tuple<int, Action<Tuple<ObservableCollection<MenuItem>, string>>>(rtuId, UpdateMenu));
         }

        void updateloopinfo(ObservableCollection<LoopInfox>data )
        {
            LoopxInfo.Clear();
            if (data.Count <= 7) LoopxInfo = data;
            else
            {
                foreach (var f in data)
                {
                    LoopxInfo.Add(f);
                   // Cr.CoreOne.OtherHelper.Delay.DelayEvent();
                }
            }
        }

         private bool LoadXmldata() //crc
         {
             int x = 0;
             var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read("SystemCommonSetConfg");
             if (info.ContainsKey("IsLdlAs100Per"))
             {
                 try
                 {
                     x = Convert.ToInt32(info["IsLdlAs100Per"]);
                 }
                 catch (Exception ex)
                 {
                     WriteLog.WriteLogError("LoadXmldata error:" + ex);
                 }
             }
             return x == 1;
         }

        // #region Thread Pool

        // //定义委托
        // public   delegate void DoTask(Tuple<ObservableCollection<MenuItem>, string> data);

        // private static WaitCallback _callBack = new WaitCallback(PooledFunc);
        // private static void PooledFunc(object state)
        // {
        //     var rf = state as Tuple<int, Action<Tuple<ObservableCollection<MenuItem>, string>>>;
        //     if (rf == null) return;
        //     // if (state == null) return;
        //     try
        //     {

        //         if (
        //             Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(
        //                 rf.Item1))
        //         {
        //             var tt =
        //                 Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rf.Item1];

        //             var tmt = MenuBuilding.BulidCm(tt.RtuModel.ToString(), false, tt);
        //             var tmp = MenuBuilding.HelpCmm(tmt);
        //             if (rf.Item2 != null)
        //             {
        //                 Application.Current.Dispatcher.Invoke(
        //                     System.Windows.Threading.DispatcherPriority.Normal, new DoTask(rf.Item2),
        //                     new Tuple<ObservableCollection<MenuItem>, string>(tmp, tt.RtuName));
        //             }
        //             //Cm.Items.Add(new MenuItem() {Header = tt.RtuName, IsEnabled = false});
        //             //foreach (var t in tmp) Cm.Items.Add(t);
        //             //this.RaisePropertyChanged(() => Cm);
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //     }

        // }

        // private void UpdateMenu(Tuple<ObservableCollection<MenuItem>, string> data)
        // {
        //     Cm.Items.Clear();
        //     Cm.Items.Add(new MenuItem() {Header = data.Item2, IsEnabled = false});
        //     foreach (var t in data.Item1) Cm.Items.Add(t);
        //     this.RaisePropertyChanged(() => Cm);
        // }


        //public static void RunDispatch(Tuple<int, Action<Tuple<ObservableCollection<MenuItem>, string>>> t)
        // {
        //     ThreadPool.QueueUserWorkItem(_callBack, t);
        // }


      
        //#endregion


         #region menu

         private ContextMenu _cm;

        public ContextMenu Cm
        {
            get
            {
                if (_cm == null)
                {
                    _cm = new ContextMenu();
                    _cm.BorderThickness=new Thickness(0);
                }
                return _cm;
            }
        }



        private void BulidMenus(int rtuId)
        {
            ResetContextMenu(rtuId);
            //return;
            //try
            //{
            //    Cm.Items.Clear();
            //    // ObservableCollection<IIMenuItem> t = null;
            //    if (Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(rtuId))
            //    {
            //        var tt = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId];

            //        var tmt = MenuBuilding.BulidCm(tt.RtuModel.ToString(), false, tt);
            //        var tmp = MenuBuilding.HelpCmm(tmt);
            //        Cm.Items.Add(new MenuItem() { Header = tt.RtuName, IsEnabled = false });
            //        foreach (var t in tmp) Cm.Items.Add(t);
            //        this.RaisePropertyChanged(() => Cm);
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }


        #region  Reset ContextMenu

        public void ResetContextMenu(int NodeId)
        {
            UpdateCm(NodeId);
        }


        private ObservableCollection<IIMenuItem> items;
        public ObservableCollection<IIMenuItem> CmItems
        {
            get { return items; }
            set
            {
                if (value == items) return;
                items = value;
                this.RaisePropertyChanged(() => this.CmItems);
            }
        }


        public void UpdateCm(int rtuId)
        {
            if (
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems .ContainsKey(
                   rtuId))
            {
                var tt =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems [rtuId];

                var tmt = MenuBuilding.BulidCm( ((int )tt.RtuModel).ToString(), false, tt);
                if (tmt != null)
                {
                    tmt.Insert(0, new MenuItemBase()
                    {
                        Text = tt.RtuPhyId  .ToString("d3") + "-" + tt.RtuName,
                        IsEnabled = false,
                        TextTmp = tt.RtuPhyId.ToString("d3") + "-" + tt.RtuName,
                    });
                }
                CmItems = tmt;
            }
        }

        #endregion


        public void MeasureRtu()
        {
            try
            {
                if (RtuId < 1000000 || RtuId > 1100000) return;

                var info = Wlst.Sr.ProtocolPhone.LxRtu .wst_rtu_orders ;
                info.Args .Addr .Add(this.RtuId);
                info.WstRtuOrders.Op = 31;
                info.WstRtuOrders.RtuIds.Add(this.RtuId);
                SndOrderServer.OrderSnd(info);

                Wlst.Cr.CoreMims.ShowMsgInfo.ShowNewMsg.AddNewShowMsg(
                    this.RtuId, RtuName, OperatrType.UserOperator, "选测终端");
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

    }
    
    //扩展显示
     public partial class NewDataViewModel
     {

         public void RequestNearData()
         {
             if (CurrentShowTmlNewData == null) return;
             var info = Wlst.Sr.ProtocolPhone.LxRtu .wst_rtu_near_data ;
             info.WstRtuData  .DtEndTime  = CurrentShowTmlNewData.DateCreate.Ticks ;
             info.WstRtuData.DtStartTime  = CurrentShowTmlNewData.DateCreate.Ticks;
             info.WstRtuData.RtuId  = CurrentShowTmlNewData.RtuId;
             info.WstRtuData.Op = 2;
             SndOrderServer.OrderSnd(info, 10, 2);
         }

         void OnRtuTimeArrive(string session, Wlst.mobile.MsgWithMobile infos)
         {
             var datax = infos.WstRtuOrders;
             if (datax == null) return;
             if (datax.Op != 22) return;
             if (datax.RtuIds[0] == this.RtuId)
             {
                 var tmp = datax.Date.Substring(0,datax.Date.Length-1);
                this.TimeInfo = "时钟:" + tmp;//RuntimeFieldHandle 

                 //Mitx.Text = _getInfoTimex + "时钟:" + tmp;
             }
         }

         void OnNearDataArrive(string session,Wlst .mobile .MsgWithMobile  infos)
         {
             if (infos == null || infos.WstRtuData == null || infos.WstRtuData.Items  == null) return;

             if (infos.WstRtuData.Items .Count == 0) return;
             var mtps = new List<RtuNewDataInfo>();
             foreach (var g in infos.WstRtuData.Items )
             {
                 mtps.Add(new RtuNewDataInfo(g));
             }
             OnNearTwoDataArrive(mtps);
         }

         private void RequestHistoryData(int rtuId,DateTime  datetime)
         {
             var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_ana_data ;
             info.WstRtuAnaData.RtuId = rtuId;
             info.WstRtuAnaData.LoopId = 0;
             info.WstRtuAnaData.DtMiddleTime = datetime.Ticks  ;
             info.WstRtuAnaData.PreMinutes =30;
             info.WstRtuAnaData.AfeMinutes  = 30;
             info.WstRtuAnaData.Days = 0;
             SndOrderServer.OrderSnd(info, 10, 6);
         }

         private Dictionary<int, Tuple<int,int,double>> GetAnaPara(Dictionary<int, Tuple<int,int,double>> a)
         {
             return a;
         }

         void HistoryDataArrive(string session, Wlst.mobile.MsgWithMobile infos)
         {
             if (infos == null || infos.WstRtuAnaData == null) return;

             HistoryData.Clear();
             HistoryDataResponse = infos.WstRtuAnaData.RtuId;
             if (RtuId != HistoryDataResponse) return;

             foreach (var t in infos.WstRtuAnaData.Info)
             {

                 foreach (var ttt in t.LstNewLoopsData)
                 {
                     if (!HistoryData.ContainsKey(ttt.LoopId))
                         HistoryData.Add(ttt.LoopId, new Tuple<int, TmlNewData.TmlNewDataforOneLoop>(t.RtuId, ttt));
                 }

             }

             if (LoopxInfo != null)
                 foreach (var f in LoopxInfo)
                 {

                     //lvf 2018年8月24日17:08:11   bug
                     if (f.LoopName.Contains("门")) continue;
                             if (HistoryData.ContainsKey(f.LoopId))
                             {
                                 f.YesterdayA = f.isShieldLoop ==0
                                                    ? HistoryData[f.LoopId].Item2.A.ToString("f2")
                                                    : "----";
                                 f.YesterdayP = f.isShieldLoop == 0
                                                    ? HistoryData[f.LoopId].Item2.Power.ToString("f2")
                                                    : "----";
                             }
                         
                     




                 }
             //var args = new PublishEventArgs()
             //{
             //    EventType = PublishEventType.Core,
             //    EventId = Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.HistoryDataUpdate,
             //};
             //args.AddParams(HistoryDataResponse );
             //EventPublish.PublishEvent(args);



             //var RtuId = Convert.ToInt32(args.GetParams()[0]);
             //var Run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(RtuId);
             //if (Run != null && Run.RtuNewData != null)
             //    OnDataChange(RtuId, Run.RtuNewData, "", false);
             //else OnDataChange(RtuId, null, "", false);

             // HistoryDataResponse = 0;

         }



         void OnNearTwoDataArrive(List<RtuNewDataInfo> fff)
         {
             Visifd = Visibility.Collapsed;
             if (CurrentShowTmlNewData == null) return;
             if (fff.Count == 0) return;
             if (CurrentShowTmlNewData.RtuId != fff[0].RtuId) return;
             if (CurrentShowTmlNewData.LstNewLoopsData == null || CurrentShowTmlNewData.LstNewLoopsData.Count == 0)
                 return;


             var lineItems = new ObservableCollection<LineInfo>();
             var lineItemsdash = new ObservableCollection<LineInfo>();

             var textBlockInfoItems = new ObservableCollection<TextBlockInfo>();

             var textBlock1InfoItems = new ObservableCollection<TextBlock1Info>();
             var looxInfo = new ObservableCollection<LoopInfox>();
             IsDataVisi = Visibility.Collapsed;


             try
             {

                 var tmpequ =
                     Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( 
                         CurrentShowTmlNewData.RtuId);
                 var tmpequ2 = tmpequ as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                 var anaPara = new Dictionary<int, Tuple<int,int,double>>();
                 
                 if (tmpequ2.WjLoops != null)
                 {
                     foreach (var t in tmpequ2.WjLoops)
                     {
                         anaPara.Add(t.Value.LoopId, new Tuple<int, int,double>( t.Value.MutualInductorRatio,t.Value.IsShieldLoop,t.Value.ShieldLittleA  ));
                     }
                 }
                 if (tmpequ == null) return;
                 this.AddBasicRtuInfoExtend(ref lineItems, ref textBlockInfoItems,
                                            tmpequ.RtuPhyId .ToString("D4") + " - " + tmpequ.RtuName,CurrentShowTmlNewData .DateCreate .ToString("yyyy-MM-dd HH:mm:ss"));



                 Tuple<int, int> onlineRate = new Tuple<int, int>(CurrentShowTmlNewData.TimesBackPartolIn24Hour,
                                                                  CurrentShowTmlNewData.TimesPartolIn24Hour);

                 fff.Insert(0,CurrentShowTmlNewData );
                 int maxHiehgt = 0;
                 for (int gggxxx = 1; gggxxx < fff.Count + 1; gggxxx++)
                 {
                    // RtuNewOneLoopDataInfo 
                     var dic = new Dictionary<int, Tuple<string,List<RtuNewDataLoopItem>>>();

                     for (int i = 1; i < fff [gggxxx -1].IsSwitchOutAttraction.Count + 1; i++)
                     {
                         if (dic.ContainsKey(i)) continue;
                         dic.Add(i, new Tuple<string, List<RtuNewDataLoopItem>>("",new List<RtuNewDataLoopItem>( )));
                     }
                     foreach (var t in fff[gggxxx - 1].LstNewLoopsData)
                     {
                         if (!dic.ContainsKey(t.SwitchOutId)) dic.Add(t.SwitchOutId, new Tuple<string, List<RtuNewDataLoopItem>>("", new List<RtuNewDataLoopItem>()));
                         dic[t.SwitchOutId].Item2.Add( t);
                     }
                     


                     //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
                     List<Tuple<int, bool, int, string,string>> swout = new List<Tuple<int, bool, int, string,string>>();

                     
                     //添加输出
                     var fffff = (from t in dic orderby t.Key select t).ToList();
                     foreach (var t in fffff)
                     {
                         if (t.Key < 1) continue;
                         bool isclose = true;
                         if (fff[gggxxx - 1].IsSwitchOutAttraction.Count >= t.Key)
                         {
                             isclose = fff[gggxxx - 1].IsSwitchOutAttraction[t.Key - 1];
                         }

                         string a = "";
                         foreach (var x in tmpequ2.WjSwitchOuts)
                         {
                             if (x.Key == t.Key)
                             {
                                 a = x.Value.SwitchName;
                             }
                         }
                         
                         swout.Add(new Tuple<int, bool, int, string,string>(t.Key, isclose, t.Value.Item2 .Count, ConstColor[t.Key%6],a ));
                     }

                     AddSitchOutInfoExtend(ref textBlock1InfoItems, ref lineItems, swout, gggxxx );


                     // var intfo = Wlst.Sr.EquipmentNewData.Services.LduNewDataServices.GetRtuLoopLduInfo(rtuId);
                     foreach (var t in fffff)
                     {
                         if (t.Key < 1) continue;

                         string color = ConstColor[t.Key%6];
                         foreach (var g in t.Value.Item2)
                         {

                             double v = 0;
                             double a = 0;
                             double power = 0;
                             string upper = "";
                             string lower = "";
                             double refA = 0;
                             try
                             {
                                 v = Convert.ToDouble(g.V);
                                 a = Convert.ToDouble(g.A);
                                 power = Convert.ToDouble(g.Power);
                                 upper = Convert.ToString(g.Upper);
                                 lower = Convert.ToString(g.Lower);
                                 refA = Convert.ToDouble(g.AvgOf7daysA);
                             }
                             catch (Exception ex)
                             {
                             }
                             AddLoopInfox(ref looxInfo, 
                                               g.LoopId, g.LoopName, g.BolSwitchInState, v, a,
                                               power,0, upper,lower,anaPara ,refA,onlineRate,color, g.Range > 0,true ,"");

                         }
                     }


                  
                         CanWidth = 170 + LoopNameLength + (250 + 110) * gggxxx;
                         //  CanHeight = fff.LstNewLoopsData.Count * RowHeight  + 30  ; ;
                       var   maxHiehgtx = fff[gggxxx - 1].LstNewLoopsData.Count * RowHeight + 2*RowHeight +
                                     (dic.Keys.Contains(0) ? dic.Count*10 - 10 : dic.Count*10); //CanHeight
                       if (maxHiehgtx > maxHiehgt) maxHiehgt = maxHiehgtx;
                       if (maxHiehgt < (4 + dic.Count) * RowHeight) maxHiehgt = (4 + dic.Count)*RowHeight;
                       if (maxHiehgt < 230) maxHiehgt = 230;

                 }

                 CanHeight = maxHiehgt;
             }
             catch (Exception ex)
             {
                 WriteLog.WriteLogError("On NewDataView change rtu error:" + ex);
             }



             //this.LoopxInfo = looxInfo;
             updateloopinfo(looxInfo);
             this.LineItemss = lineItems;
             this.LineItemsDash = lineItemsdash;
             this.TextBlockInfoItemss = textBlockInfoItems;
             this.TextBlock1InfoItemss = textBlock1InfoItems;
             this.BulidMenus(CurrentShowTmlNewData.RtuId);
             //RunDispatch(new Tuple<int, Action<Tuple<ObservableCollection<MenuItem>, string>>>(CurrentShowTmlNewData .RtuId , UpdateMenu));
         }


         private void AddBasicRtuInfoExtend(ref ObservableCollection<LineInfo> LineItems, ref ObservableCollection<TextBlockInfo> TextBlockInfoItems, string rtuName,string timeinfo)
         {
             LineItems.Add(new LineInfo() //--
             {
                 Color = K1BackgroundColor,// "AliceBlue",
                 Index = 0,

                 X1 = 35,
                 X2 = 35,
                 Y1 = RowHeight * 2,
                 Y2 = 150
             });
             LineItems.Add(new LineInfo() //--
             {
                 Color = K1BackgroundColor,// "AliceBlue",
                 Index = 0,

                 X1 = 10,
                 X2 = 150,
                 Y1 = RowHeight * 2,
                 Y2 = RowHeight * 2
             });

             TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
             {
                 BorderThinkness = 0,
                 Color = K1BackgroundColor,// "Blue",
                 CornerRadius = 0,
                 Height = RowHeight,
                 Index = 0,
                 Left = 10,
                 HorizontalAlign = HorizontalAlignment.Left,


                 Text = rtuName,
                 Top = 0,
                 Width = RtuNameLength
             });

             Mit.BorderThinkness = 1;
             Mit.Color = K1BackgroundColor;// "Blue";
             Mit.CornerRadius = 5;
             Mit.Height = 50;
             Mit.Width = 50;
             Mit.Index = 0;
             Mit.Left = 10;
             Mit.Tooltips = "";
             Mit.Text = "ceshi";
             Mit.Top = 150;

             Mitx.Text = timeinfo;

             LineItems.Add(new LineInfo() //--
             {
                 Color = K1BackgroundColor,// "Blue",
                 Index = 0,

                 X1 = 60,
                 X2 = 70,
                 Y1 = 175,
                 Y2 = 175
             });
             LineItems.Add(new LineInfo() //--
             {
                 Color = K1BackgroundColor,// "AliceBlue",
                 Index = 0,

                 X1 = 70,
                 X2 = 70,
                 Y1 = RowHeight * 2 + 10,
                 Y2 = 250
             });

         }


         /// <summary>
         /// 添加开关量输出信息
         /// </summary>
         /// <param name="swout">开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色</param>
         /// <param name="color">绘图前面部分绘图颜色 默认blue</param>
         private void AddSitchOutInfoExtend(ref ObservableCollection<TextBlock1Info> TextBlock1InfoItems, ref ObservableCollection<LineInfo> LineItems, List<Tuple<int, bool, int, string,string>> swout, int indexView)
         {
             int loopsCount = 0;

         
             int starty = 4 * RowHeight;
             int startxgt = 70;

             if (indexView == 1) startxgt = 70;  //60+3*LoopNameLength
             if (indexView == 2) startxgt = 70 + LoopNameLength + 250 + 110;
             if (indexView == 3) startxgt = 70 + LoopNameLength + 250 + 110 + 250 + 110;


             for (int i = 0; i < swout.Count; i++)
             {


                 int startx = startxgt;
                 
                 LineItems.Add(new LineInfo() //-- / -- K1 --
                 {
                     Color = swout[i].Item4,
                     Index = 0,

                     X1 = startx+1,
                     X2 = startx + 9,
                     Y1 = starty + i * RowHeight,
                     Y2 = starty + i * RowHeight
                 });
                 startx +=  10; //220

               
                 string temp = BackgroundColor;
             
                 TextBlock1InfoItems.Add(new TextBlock1Info() //-- / -- K1
                 {
                     BorderThinkness = 1,
                     Color = swout[i].Item4,
                     CornerRadius = 0,
                     Height = 20,
                     Index = 0,
                     Left = startx,
                     BackgroundColor = swout[i].Item2 ? temp : "Transparent",
                     Text = "K" + swout[i].Item1,
                     Top = starty - 10 + i * RowHeight,
                     Width = 30
                 });
                 startx += 30; //250

                 LineItems.Add(new LineInfo() //-- / -- K1 --
                 {
                     Color = swout[i].Item4,
                     Index = 0,

                     X1 = startx,
                     X2 = startx + 10,
                     Y1 = starty + i * RowHeight,
                     Y2 = starty + i * RowHeight
                 });
                 startx += 10; //270

         

                 LineItems.Add(new LineInfo() //-- / -- K1 --
                 {
                     Color = swout[i].Item4,
                     Index = 0,

                     X1 = startx,
                     X2 = startx + 40 - i * 7,
                     Y1 = starty + i * RowHeight,
                     Y2 = starty + i * RowHeight
                 });
                 startx += 40 - i * 7; //350-i*5

                 if (swout[i].Item3 == 0)
                 {
                     continue;
                 }

                 LineItems.Add(new LineInfo() //-- / -- K1 转 
                 {
                     Color = swout[i].Item4,
                     Index = 0,

                     X1 = startx,
                     X2 = startx,
                     Y1 = starty + i * RowHeight,
                     Y2 = starty - RowHeight + loopsCount * RowHeight + (swout[i].Item3 - 1) * RowHeight / 2 + i * 10
                 });

                 LineItems.Add(new LineInfo() //-- / -- K1 转 --
                 {
                     Color = swout[i].Item4,
                     Index = 0,

                     X1 = startx,
                     X2 = startx + 10 + i * 7,
                     Y1 = starty - RowHeight + loopsCount * RowHeight + (swout[i].Item3 - 1) * RowHeight / 2 + i * 10,
                     Y2 = starty - RowHeight + loopsCount * RowHeight + (swout[i].Item3 - 1) * RowHeight / 2 + i * 10
                 });
                 startx += 10 + i * 7; //380

                 LineItems.Add(new LineInfo() //-- / -- K1 转 --竖
                 {
                     Color = swout[i].Item4,
                     Index = 0,

                     X1 = startx,
                     X2 = startx,
                     Y1 = starty - RowHeight + loopsCount * RowHeight + i * 10 - RowHeight / 2,
                     Y2 = starty + loopsCount * RowHeight + (swout[i].Item3 - 1) * RowHeight + i * 10 - RowHeight / 2
                 }); //x2=170

                 loopsCount += swout[i].Item3;
             }
         }




   


     }

    public partial class NewDataViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        //public static string[] ConstColor = new string[]
        //                                  {
        //                                      "DarkCyan", "DarkViolet", "DarkGoldenrod", "DarkRed", "DarkGreen", "DarkGray" ,"DarkSalmon"
        //                                  };
        public static string[] ConstColor = GetColor();
        private static string[] GetColor()
        {

            var info = ZNewData.NewDataSetting.NewDataSettingViewModel.LoadNewDataLenghtSetConfgX();
            string[] myColor = new string[9]{
                                              info.Item7.Background,
                                              info.Item7.K1Background, 
                                              info.Item7.K2Background, 
                                              info.Item7.K3Background, 
                                              info.Item7.K4Background,
                                              info.Item7.K5Background ,
                                              info.Item7.K6Background,
                                              info.Item7.K7Background,
                                              info.Item7.K8Background,
                                          };
            return myColor;
        }
        #region def


        //private ObservableCollection<LineInfo> _lineItems;
        //private ObservableCollection<TextBlockInfo> _textBlockInfoItems;
        //public ObservableCollection<LineInfo> LineItems
        //{
        //    get
        //    {
        //        if (_lineItems == null) _lineItems = new ObservableCollection<LineInfo>();
        //        return _lineItems;
        //    }
        //}
        //public ObservableCollection<TextBlockInfo> TextBlockInfoItems
        //{
        //    get
        //    {
        //        if (_textBlockInfoItems == null) _textBlockInfoItems = new ObservableCollection<TextBlockInfo>();
        //        return _textBlockInfoItems;
        //    }
        //}


        //private ObservableCollection<ArcInfo> _arcItems;
        //public ObservableCollection<ArcInfo> ArcItems
        //{
        //    get
        //    {
        //        if (_arcItems == null) _arcItems = new ObservableCollection<ArcInfo>();
        //        return _arcItems;
        //    }
        //}

        //private ObservableCollection<EllInfo> _ellItems;
        //public ObservableCollection<EllInfo> EllItems
        //{
        //    get
        //    {
        //        if (_ellItems == null) _ellItems = new ObservableCollection<EllInfo>();
        //        return _ellItems;
        //    }
        //}
        private bool _isLdlAs100Per;

        public bool IsLdlAs100Per
        {
            get
            {
                return _isLdlAs100Per;
            }
            set
            {
                if (value != _isLdlAs100Per)
                {
                    _isLdlAs100Per = value;
                    this.RaisePropertyChanged(() => this.IsLdlAs100Per);
                }
            }
        }


              private ObservableCollection<LineInfo> _lLineItemsDash;
        public ObservableCollection<LineInfo> LineItemsDash
        {
            get
            {
                if (_lLineItemsDash == null) _lLineItemsDash = new ObservableCollection<LineInfo>();
                return _lLineItemsDash;
            }
            set
            {
                if (_lLineItemsDash != value)
                {
                    _lLineItemsDash = value;
                    this.RaisePropertyChanged(() => this.LineItemsDash);
                }
            }
        }


        private ObservableCollection<LineInfo> _lineItemss;
        private ObservableCollection<TextBlockInfo> _textBlockInfoItemss;
        public ObservableCollection<LineInfo> LineItemss
        {
            get
            {
                if (_lineItemss == null) _lineItemss = new ObservableCollection<LineInfo>();
                return _lineItemss;
            }
            set
            {
                if (_lineItemss != value)
                {
                    _lineItemss = value;
                    this.RaisePropertyChanged(() => this.LineItemss);
                }
            }
        }
        public ObservableCollection<TextBlockInfo> TextBlockInfoItemss
        {
            get
            {
                if (_textBlockInfoItemss == null) _textBlockInfoItemss = new ObservableCollection<TextBlockInfo>();
                return _textBlockInfoItemss;
            }
            set
            {
                if (_textBlockInfoItemss != value)
                {
                    _textBlockInfoItemss = value;
                    this.RaisePropertyChanged(() => this.TextBlockInfoItemss);
                }
            }
        }

        private ObservableCollection<LoopInfox  > _lineInfox;

        public ObservableCollection<LoopInfox> LoopxInfo
        {
            get
            {
                if (_lineInfox == null) _lineInfox = new ObservableCollection<LoopInfox>();
                return _lineInfox;
            }
            set
            {
                if (_lineInfox != value)
                {
                    _lineInfox = value;

                    this.RaisePropertyChanged(() => this.LoopxInfo);
                
                    if (value == null) return;

                    IsDataVisi = value.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                    foreach (var f in value)
                    {
                        if (string.IsNullOrEmpty(f.AttachInfo)) continue;
                        Visifd = Visibility.Visible;
                        return;
                    }
                }
            }
        }

        private Visibility _linIsDataVisieInfox;

         public Visibility IsDataVisi
        {
            get
            {
                return _linIsDataVisieInfox;
            }
            set
            {
                if (_linIsDataVisieInfox != value)
                {
                    _linIsDataVisieInfox = value;
                    this.RaisePropertyChanged(() => this.IsDataVisi);
                }
            }
        }
        
         private Visibility _linIsDataVVisifdisieInfox;

        public Visibility Visifd
        {
            get { return _linIsDataVVisifdisieInfox; }
            set
            {
                if (_linIsDataVVisifdisieInfox != value)
                {
                    _linIsDataVVisifdisieInfox = value;
                    this.RaisePropertyChanged(() => this.Visifd);
                    if (VisiChanged != null) VisiChanged(value, new EventArgs());
                }
            }
        }

        private Visibility _isCompare;

        public Visibility IsCompareCheck
        {
            get { return _isCompare; }
            set
            {
                if (_isCompare != value)
                {
                    _isCompare = value;
                    this.RaisePropertyChanged(() => this.IsCompareCheck);
                    if (CompareVisiChanged != null) CompareVisiChanged(value, new EventArgs());
                }
            }
        }


        private Visibility _isDetail;

        public Visibility IsDetailCheck
        {
            get { return _isDetail; }
            set
            {
                if (_isDetail != value)
                {
                    _isDetail = value;
                    this.RaisePropertyChanged(() => this.IsDetailCheck);
                    if (DetailVisiChanged != null) DetailVisiChanged(value, new EventArgs());

                }
            }
        }

        private Visibility _isOnlineRate;

        public Visibility IsOnlineRateCheck
        {
            get { return _isOnlineRate; }
            set
            {
                if (_isOnlineRate != value)
                {
                    _isOnlineRate = value;
                    this.RaisePropertyChanged(() => this.IsOnlineRateCheck);
                    if (OnlineVisiChanged != null) OnlineVisiChanged(value, new EventArgs());

                }
            }
        }

        public event EventHandler VisiChanged;
        public event EventHandler CompareVisiChanged;
        public event EventHandler ShieldLoopVisiChanged;

        public event EventHandler DetailVisiChanged;
        public event EventHandler OnlineVisiChanged;
        public event EventHandler MarginChanged;

        private double _left;

        public double Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    this.RaisePropertyChanged(() => this.Left);
                    if (MarginChanged != null) MarginChanged(value, new EventArgs());

                }
            }
        }

        private ObservableCollection<TextBlock1Info> _textBlock1InfoItemss;
        public ObservableCollection<TextBlock1Info> TextBlock1InfoItemss
        {
            get
            {
                if (_textBlock1InfoItemss == null) _textBlock1InfoItemss = new ObservableCollection<TextBlock1Info>();
                return _textBlock1InfoItemss;
            }
            set
            {
                if (_textBlock1InfoItemss != value)
                {
                    _textBlock1InfoItemss = value;
                    this.RaisePropertyChanged(() => this.TextBlock1InfoItemss);
                }
            }
        }


        
        
        
        #endregion



        private string _getInfoTimex = string.Empty;
        private void AddBasicRtuInfo(ref ObservableCollection<LineInfo> LineItems, ref ObservableCollection<TextBlockInfo> TextBlockInfoItems,
            string rtuName, string getInfoTime, int rtutemperature, NameValueInt _transferState)
        {
            LineItems.Add(new LineInfo() //--
            {
                Color =K1BackgroundColor,// "AliceBlue",
                Index = 0,
                
                X1 = 35,
                X2 = 35,
                Y1 = RowHeight *2,
                Y2 = 80
            });
            LineItems.Add(new LineInfo() //--
            {
                Color = K1BackgroundColor,// "AliceBlue",
                Index = 0,
                
                X1 = 10,
                X2 = 80,
                Y1 = RowHeight *2,
                Y2 = RowHeight * 2
            });

            TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
            {
                BorderThinkness = 0,
                Color = K1BackgroundColor,// "Blue",
                CornerRadius = 0,
                Height = RowHeight ,
                Index = 0,
                Left = 10,
                HorizontalAlign = HorizontalAlignment.Left,

                
                Text = rtuName,
                Top = 0,
                Width = RtuNameLength
            });

            //TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
            //{
            //    BorderThinkness = 0,
            //    Color = K1BackgroundColor,// "Blue",
            //    CornerRadius = 0,
            //    Height = RowHeight ,
            //    Index = 0,
            //    Left = 10,
                
            //    Text = getInfoTime,
            //    HorizontalAlign = HorizontalAlignment.Left,
            //    Top = RowHeight ,
            //    Width = 270
            //});

            _getInfoTimex = getInfoTime;
                Mitx.BorderThinkness = 0;
            Mitx.Color = K1BackgroundColor;// "Blue";
            Mitx.CornerRadius = 0;
            Mitx.Height = RowHeight;
            Mitx.Index = 0;
            Mitx.Left = 10;
            Mitx.Text = getInfoTime;
            Mitx.Top = RowHeight;
            Mitx.Width = 370;
            Mitx.HorizontalAlign = HorizontalAlignment.Left;


            //TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
            //{
            //    BorderThinkness = 1,
            //    Color = "Blue",
            //    CornerRadius = 5,
            //    Height = 50,
            //    Width = 50,
            //    Index = 0,
            //    Left = 10,
            //    
            //    Text = "  终端",
            //    Top = 150
            //});
            Mit.BorderThinkness = 1;
            Mit.Color = K1BackgroundColor;// "Blue";
            Mit.CornerRadius = 5;
            Mit.Height = 50;
            Mit.Width = 50;
            Mit.Index = 0;
            Mit.Left = 10;
            Mit.Tooltips = "";
            Mit.Text = rtutemperature != null && rtutemperature > 0 && rtutemperature < 71 ? rtutemperature.ToString() + "度" : "终端";
            Mit.Top = 80;

            if (_transferState.Value == 2)
            {
                Mit1.Color = "Red";
            }
            else
            {
                Mit1.Color = "Blue";
            }

            Mit1.Index = 0;
            Mit1.Left = 13;
            Mit1.Text = _transferState.Name;
            Mit1.Top = 140;


            LineItems.Add(new LineInfo() //--
            {
                Color = K1BackgroundColor,// "Blue",
                Index = 0,
                
                X1 = 60,
                X2 = 70,
                Y1 = 105,
                Y2 = 105
            });
            LineItems.Add(new LineInfo() //--
            {
                Color = K1BackgroundColor,// "AliceBlue",
                Index = 0,
                
                X1 = 70,
                X2 = 70,
                Y1 = RowHeight * 2 + 10,
                Y2 = RowHeight * 8 + 10
            });

        }


        /// <summary>
        /// 添加开关量输出信息
        /// </summary>
        /// <param name="swout">开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色</param>
        /// <param name="color">绘图前面部分绘图颜色 默认blue</param>
        private void AddSitchOutInfo(ref ObservableCollection<TextBlockInfo> TextBlockInfoItems, ref ObservableCollection<TextBlock1Info> TextBlock1InfoItems, ref ObservableCollection<LineInfo> LineItems, int rtuId, List<Tuple<int, bool, int, string,string>> swout, bool isHistory, string color = "Blue")
        {
            int loopsCount = 0;
            var areaid =
                   Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuId);
            //int startx = 70;
            int starty = 3*RowHeight;
            var isrtuinholidayinfo =
                Wlst.Sr.TimeTableSystem.Services.HolidayTimeandBandingServices.Myself.IsRtuInHoliday(areaid,rtuId);
            var holidayInfo = new List<string>();
            if (isrtuinholidayinfo)
                holidayInfo =
                    Wlst.Sr.TimeTableSystem.Services.HolidayTimeandBandingServices.Myself.
                        GetRtuSwitchOutOpenCloseTimeInholiday(areaid,rtuId);




            for (int i = 0; i < swout.Count; i++)
            {
                if (swout[i].Item1 == 1)
                {
                    color = K1BackgroundColor;
                }
                if (swout[i].Item1 == 2)
                {
                    color = K2BackgroundColor;
                }
                if (swout[i].Item1 == 3)
                {
                    color = K3BackgroundColor;
                }
                if (swout[i].Item1 == 4)
                {
                    color = K4BackgroundColor;
                }
                if (swout[i].Item1 == 5)
                {
                    color = K5BackgroundColor;
                }
                if (swout[i].Item1 == 6)
                {
                    color = K6BackgroundColor;
                }
                if (swout[i].Item1 == 7)
                {
                    color = K7BackgroundColor;
                }
                if (swout[i].Item1 == 8)
                {
                    color = K8BackgroundColor;
                }

                int startx = 70;
                //starty += 10;
                //前面一点点
                LineItems.Add(new LineInfo() //--
                                  {
                                      Color = color,
                                      Index = 0,

                                      X1 = startx,
                                      X2 = startx + TimeNameLength + 10 + KxNameLength -30,
                                      Y1 = starty + i*RowHeight,
                                      Y2 = starty + i*RowHeight
                                  });

                string temp = BackgroundColor;
                TextBlock1InfoItems.Add(new TextBlock1Info() //-- / -- K1
                                            {
                                                BorderThinkness = 1,
                                                Color = swout[i].Item4,
                                                CornerRadius = 0,
                                                Height = 20,
                                                Index = 0,
                                                Left = startx,
                                                BackgroundColor = swout[i].Item2 ? temp : "Transparent",
                                                Text = swout[i].Item5,
                                                Top = starty - 19 + i*RowHeight,
                                                Width = KxNameLength 
                                            });
                startx += 30; //250
                Left = KxNameLength -40;
                //if (isHistory == false)
                {
                    var timeInfo = "";
                    var toolinfo = "";
                    if (isrtuinholidayinfo)
                    {
                        if (holidayInfo != null && holidayInfo.Count > i) timeInfo = "假 " + holidayInfo[i];
                        else timeInfo = "假日时间未知";
                        toolinfo = "节假日开关灯.";
                    }
                    else
                    {
                        var name = "";
                        Wlst.Sr.TimeTableSystem.Models.TodayOpenCloseTime yesterday;

                        var rtuIdOrGrpId = rtuId;
                        
                        //if (xtmp != null) rtuIdOrGrpId = xtmp.Item2;
                       
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(rtuId ))
                        {
                            //var areaId = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId].AreaId;
                            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuId);
                            var tmp =
                                Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.
                                    GetTmlLoopBandTimeTableTodayOpenCloseTimex(areaId,
                                                                               rtuIdOrGrpId, swout[i].Item1);

                            if (tmp != null)
                            {

                                //var name = "";
                               
                                if (tmp.TimeOnOff.Count > 0)
                                {

                                    if (tmp.TimeOnOff.Count == 1)
                                    {
                                        if (tmp.TimeOnOff[0].Item1 == 1500)
                                            name += "25:00";//"---";
                                        else
                                            name += string.Format("{0:D2}", tmp.TimeOnOff[0].Item1 / 60) + ":" +
                                                    string.Format("{0:D2}", tmp.TimeOnOff[0].Item1 % 60);
                                        name += " - ";

                                        if (tmp.TimeOnOff[0].Item2 == 1500)
                                            name += "25:00";//"---";
                                        else
                                            name += string.Format("{0:D2}", tmp.TimeOnOff[0].Item2/60) + ":" +
                                                    string.Format("{0:D2}", tmp.TimeOnOff[0].Item2%60);
                                    }
                                    else
                                    {
                                        //if (tmp.TimeTableName.Length >= 3)
                                        //    name += tmp.TimeTableName.Substring(0, 3);
                                        //else
                                        //    name += tmp.TimeTableName;
                                        //name += ": ";
                                        name += "多段开关灯";
                                    }
                                    var str = "";
                                    foreach (var OnOffTime in tmp.TimeOnOff)
                                    {
                                        if (OnOffTime.Item1 == 1500)
                                            str += "25:00";// "---";
                                        else
                                            str += string.Format("{0:D2}", OnOffTime.Item1/60) + ":" +
                                                   string.Format("{0:D2}", OnOffTime.Item1%60);
                                        str += " - ";

                                        if (OnOffTime.Item2 == 1500)
                                            str += "25:00";// "---";
                                        else
                                            str += string.Format("{0:D2}", OnOffTime.Item2/60) + ":" +
                                                   string.Format("{0:D2}", OnOffTime.Item2%60);

                                        str += " | ";
                                    }

                                    toolinfo = "今日操作:";
                                    if (str.Length == 0) toolinfo += "无;";

                                    toolinfo += str;
                                }
                                else
                                {
                                    name ="25:00 - 25:00";
                                    toolinfo = "今日操作:无;";

                                }
                              
                                timeInfo = name;
                            }
                            else
                            {
                                timeInfo = "25:00 - 25:00";
                                toolinfo = "今日操作:无;";
                            }

                        }
                    }

                    int addd = 10;
                    if (isrtuinholidayinfo) addd = 3;
                    TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
                    {
                        BorderThinkness = 0,
                        Color = color,
                        CornerRadius = 0,
                        Height = RowHeight,
                        Index = 0,
                        Left = startx + addd + KxNameLength -30,
                        Tooltips = toolinfo,
                        Text = timeInfo,
                        Top = starty + i * RowHeight - RowHeight,
                        //-20
                        HorizontalAlign = HorizontalAlignment.Left,
                        Width = TimeNameLength + 10 + 30 
                    });

                }
                //else
                //{
                //    //int addd = 10;
                //    //if (isrtuinholidayinfo) addd = 3;
                //    TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
                //                               {
                //                                   BorderThinkness = 0,
                //                                   Color = color,
                //                                   CornerRadius = 0,
                //                                   Height = RowHeight,
                //                                   Index = 0,
                //                                   Left = startx + KxNameLength - 30,
                //                                   Tooltips = "",
                //                                   Text = "历史数据",
                //                                   Top = starty + i * RowHeight - RowHeight,
                //                                   //-20
                //                                   HorizontalAlign = HorizontalAlignment.Left,
                //                                   Width = TimeNameLength + 10
                //                               });


                //}

                startx += TimeNameLength + 10; //220
                startx += 40 - i * 7; //350-i*5
                if (swout[i].Item3 == 0)
                {
                    continue;
                }
                loopsCount += swout[i].Item3;
            }
        }



        /// <summary>
        /// 添加回路
        /// </summary>
        /// <param name="loopId">回路地址</param>
        /// <param name="name">回路名称 </param>
        /// <param name="switchIsClose">本回路开关量输入是否闭合</param>
        /// <param name="v">电压</param>
        /// <param name="a">电流</param>
        /// <param name="power">功率</param>
        /// <param name="rate">亮灯率</param>
        /// <param name="color">本回路颜色</param>
        /// <param name="used">回路是否使用中 量程是否为不为0 </param>
        /// <param name="attachInfo">附加显示信息 </param>
        private void AddLoopInfox(ref ObservableCollection<LoopInfox> loopInfox, int loopId, string name, bool switchIsClose, double v, double a, double power, double rate, string upper, string lower, Dictionary<int, Tuple<int, int, double>> anapara, double referencedata, Tuple<int, int> calonline, string color, bool used, bool isHistory, string attachInfo = null, string attachInfoName = null, string attachcolor = "000000")
        {
            
            
            
            
            var pws = "0.0";
            if(used && a >0 && v >0 && power >0)
            {
                var exr = power*1000/(v*a);
                if (exr > 1 && exr < 1.2) exr = 1;
                pws = string.Format("{0:0.00}", exr );
            }

            string yesterdayax= "";
            string yesterdaypx = "";
            string referenceAx = "";
            string ratiox = "";
            string onlineRate = "";
            int isShieldLoop = 0;
            double ShieldLittleA = 0.0;
            
           

            foreach(var t in anapara)
            {
                if(t.Key ==loopId )
                {
                    ratiox = t.Value.Item1 .ToString("f0")+"/5";
                    isShieldLoop = t.Value.Item2;
                    ShieldLittleA = t.Value.Item3;
                }
                
            }
            if (isShieldLoop == 2) return;

            referenceAx =referencedata.ToString("f2") ;
            if (isShieldLoop == 1 && Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 3) == false) referenceAx = "----"; 

            // foreach(var t in historydata )
            //{
            //    if(t.Key ==loopId )
            //    {
            //        yesterdayax = isShieldLoop == false ? t.Value.Item2.A.ToString("f2") : "----" ;
            //        yesterdaypx = isShieldLoop == false ? t.Value.Item2.Power.ToString("f2") : "----" ;                    
            //    }
            //}

            
                if(calonline .Item2  !=0)
                {
                    onlineRate = isShieldLoop == 0 ? (calonline.Item1 * 100 / calonline.Item2).ToString("f2") + "%" : "----"; 
                }
                else
                {
                    onlineRate = "----";
                }
                
            string upperx = isShieldLoop == 0 ? upper: "----";
            string lowerx = isShieldLoop == 0 ? lower : "----";





            string vx = GetShowIndex(15) ? string.Format("{0:0.00}", v) + "" : string.Format("{0:0.00}", v);
            if(isShieldLoop == 1  )
            {
                if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 4) == false) vx = "----";  //屏蔽回路是否显示电压
                //vx= ShieldLoopShAV? "----" :GetShowIndex( 15) ? string.Format("{0:0.00}", v) + "" : string.Format("{0:0.00}", v) ;
            }

            string ax = "";
            if (used)
            {
                if ((ShieldLittleA == 0.0 || a > ShieldLittleA) )
                {
                    ax = GetShowIndex(15) ? string.Format("{0:0.00}", a) + "A" : string.Format("{0:0.00}", a);
                }
                else
                {

                    ax =a == 0?"0.00": "<" + ShieldLittleA;
                }
            }
            else
            {
                ax = "----";
            }
            
            //used ? (ShieldLittleA == 0.0 || a > ShieldLittleA) ? GetShowIndex(15) ? string.Format("{0:0.00}", a) + "" : string.Format("{0:0.00}", a) : "<" + ShieldLittleA : "----";
           if(isShieldLoop ==1 )
           {
               if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 3) == false) ax = "----";   //屏蔽回路是否显示电流
           }
            //ShieldLoopShAV?isShieldLoop == false ? used ? (ShieldLittleA == 0.0 || a > ShieldLittleA) ? GetShowIndex(15) ? string.Format("{0:0.00}", a) + "" : string.Format("{0:0.00}", a) : "<" + ShieldLittleA : "----" : "----":"----";
           
            
            string powerx = isShieldLoop == 0 ?used
                                ? GetShowIndex(15) ? string.Format("{0:0.00}", power) + "" : string.Format("{0:0.00}", power)
                                : "----" : "----";



            loopInfox.Add(new LoopInfox(loopId, name, vx, ax, powerx,
                                        used ? isShieldLoop == 0 ? string.Format("{0:0.00}", rate * 100) + " %" : "----" : "----", used ? isShieldLoop == 0 ? pws : "----" : "----",
                                        switchIsClose ? "吸合" : "断开", isHistory ? "" : attachInfo, attachInfoName, color, attachcolor, referenceAx, upperx, lowerx, ratiox, onlineRate, switchIsClose ? "#CD0000" : "#000000", isShieldLoop));

        }


        private void AddSwitchInInfo(ref ObservableCollection<LoopInfox > loopInfox,  int loopId,  string loopName,string  switchShowInfo, string color)
        {
            loopInfox.Add(new LoopInfox(loopId, loopName, "", "", "", "", "", switchShowInfo, "","", color, color,  "", "", "", "","","",0));
        }



        private int _height;
        private int _width;
        public int CanHeight
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    this.RaisePropertyChanged(() => this.CanHeight);
                }
            }
        }
        public int CanWidth
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    this.RaisePropertyChanged(() => this.CanWidth);
                }
            }
        }



        private TextBlockInfo _menuItem;
        public TextBlockInfo Mit
        {
            get
            {
                if (_menuItem == null) _menuItem = new TextBlockInfo();
                return _menuItem;
            }
        }

        private TextBlockInfo _menuItem1;
        public TextBlockInfo Mit1
        {
            get
            {
                if (_menuItem1 == null) _menuItem1 = new TextBlockInfo();
                return _menuItem1;
            }
        }

        private TextBlockInfo _menuItemxx;
        public TextBlockInfo Mitx
        {
            get
            {
                if (_menuItemxx == null) _menuItemxx = new TextBlockInfo();
                return _menuItemxx;
            }
            set
            {
                if (value == _menuItemxx) return;
                _menuItemxx = value;
                this.RaisePropertyChanged(() => this.Mitx);
            }
        }
    }


    public class LineInfo : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public LineInfo()
        {
            StrokeThickness = 1;
        }
        private int _index;

        private int _x1;
        private int _y1;
        private int _y2;
        private int _x2;
        private string _tooltips;
        private string _color;

        public int Index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }

        public int X1
        {
            get { return _x1; }
            set
            {
                if (_x1 != value)
                {
                    _x1 = value;
                    this.RaisePropertyChanged(() => this.X1);
                }
            }
        }

        public int Y1
        {
            get { return _y1; }
            set
            {
                if (_y1 != value)
                {
                    _y1 = value;
                    this.RaisePropertyChanged(() => this.Y1);
                }
            }
        }

        public int Y2
        {
            get { return _y2; }
            set
            {
                if (_y2 != value)
                {
                    _y2 = value;
                    this.RaisePropertyChanged(() => this.Y2);
                }
            }
        }

        public int X2
        {
            get { return _x2; }
            set
            {
                if (_x2 != value)
                {
                    _x2 = value;
                    this.RaisePropertyChanged(() => this.X2);
                }
            }
        }

        public string Tooltips
        {
            get { return _tooltips; }
            set
            {
                if (_tooltips != value)
                {
                    _tooltips = value;
                    this.RaisePropertyChanged(() => this.Tooltips);
                }
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    this.RaisePropertyChanged(() => this.Color);
                }
            }
        }

        private int _borderThinkness;
        public int StrokeThickness
        {
            get { return _borderThinkness; }
            set
            {
                if (_borderThinkness != value)
                {
                    _borderThinkness = value;
                    this.RaisePropertyChanged(() => this.StrokeThickness);
                }
            }
        }
    }

    public class TextBlockInfo : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int _index;

        private int _left;
        private int _top;
        private int _height;
        private int _width;
        private int _borderThinkness;
        private int _cornerRadius;
        private string _tooltips;
        private string _color;
        private string _text;

        public TextBlockInfo ()
        {
            HorizontalAlign = HorizontalAlignment.Center;
            VerticalAlign = VerticalAlignment.Center;
        }
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    this.RaisePropertyChanged(() => this.Index);
                }
            }
        }

        public int BorderThinkness
        {
            get { return _borderThinkness; }
            set
            {
                if (_borderThinkness != value)
                {
                    _borderThinkness = value;
                    this.RaisePropertyChanged(() => this.BorderThinkness);
                }
            }
        }

        public int Left
        {
            get { return _left; }
            set
            {
                if (_left != value)
                {
                    _left = value;
                    this.RaisePropertyChanged(() => this.Left);
                }
            }
        }

        public int Top
        {
            get { return _top; }
            set
            {
                if (_top != value)
                {
                    _top = value;
                    this.RaisePropertyChanged(() => this.Top);
                }
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                if (_height != value)
                {
                    _height = value;
                    this.RaisePropertyChanged(() => this.Height);
                }
            }
        }

        public int Width
        {
            get { return _width; }
            set
            {
                if (_width != value)
                {
                    _width = value;
                    this.RaisePropertyChanged(() => this.Width);
                }
            }
        }

        public string Tooltips
        {
            get { return _tooltips; }
            set
            {
                if (_tooltips != value)
                {
                    _tooltips = value;
                    this.RaisePropertyChanged(() => this.Tooltips);
                }
            }
        }

        public string Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    this.RaisePropertyChanged(() => this.Color);
                }
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    this.RaisePropertyChanged(() => this.Text);
                }
            }
        }

        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                if (_cornerRadius != value)
                {
                    _cornerRadius = value;
                    this.RaisePropertyChanged(() => this.CornerRadius);
                }
            }
        }

        private HorizontalAlignment _horizontalAlign;
        public HorizontalAlignment HorizontalAlign
        {
            get { return _horizontalAlign; }
            set
            {
                if (_horizontalAlign != value)
                {
                    _horizontalAlign = value;
                    this.RaisePropertyChanged(() => this.HorizontalAlign);
                }
            }
        }

        private VerticalAlignment _verticalAlign;
        public VerticalAlignment VerticalAlign
        {
            get { return _verticalAlign; }
            set
            {
                if (_verticalAlign != value)
                {
                    _verticalAlign = value;
                    this.RaisePropertyChanged(() => this.VerticalAlign);
                }
            }
        }
    }

    public class MitTextBlock:TextBlockInfo
    {
        private string _text;
        public new  string Text
        {
            get { return _text; }
            set
            {
               // if (_text != value)
                {
                    _text = value;
                    this.RaisePropertyChanged(() => this.Text);
                }
            }
        }
    }

    public class TextBlock1Info : TextBlockInfo
    {
        public TextBlock1Info()
        {
            HorizontalAlign = HorizontalAlignment.Center;
            VerticalAlign = VerticalAlignment.Center;
        }
        private string _backgroundcolor;


        public string BackgroundColor
        {
            get { return _backgroundcolor; }
            set
            {
                if (_backgroundcolor != value)
                {
                    _backgroundcolor = value;
                    this.RaisePropertyChanged(() => this.BackgroundColor);
                }
            }
        }

    }

   
}
