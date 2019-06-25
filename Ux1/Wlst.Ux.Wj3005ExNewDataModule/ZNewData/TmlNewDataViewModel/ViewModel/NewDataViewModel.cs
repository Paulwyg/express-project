using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Prism.MefExtensions.Event;
using Microsoft.Practices.Prism.MefExtensions.Event.EventHelper;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Cr.Core.UtilityFunction;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreMims.ShowMsgInfo;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Sr.Menu.Services;
using Wlst.Ux.Wj3005ExNewDataModule.Services;
using Wlst.Ux.Wj3005ExNewDataModule.ZNewData.TmlNewDataViewModel.Services;

namespace Wlst.Ux.Wj3005ExNewDataModule.ZNewData.TmlNewDataViewModel.ViewModel
{
    [Export(typeof(IINewDataViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewDataViewModel : IINewDataViewModel,Wlst .Cr .CoreMims .CoreInterface .IIShowData 
    {
        public static int RowHeight = 25;
        public static int LoopNameLength = 120;
        public static int TimeNameLength = 120;
        public static int VaNameLength = 80;
        public static bool IsShowLoopId = false;
        public static int RtuNameLength = 375;
        public static string BackgroundColor = "Transparent";
        public static string K1BackgroundColor = "Transparent";
        public static string K2BackgroundColor = "Transparent";
        public static string K3BackgroundColor = "Transparent";
        public static string K4BackgroundColor = "Transparent";
        public static string K5BackgroundColor = "Transparent";
        public static string K6BackgroundColor = "Transparent";
        public static bool IsShowDw = true;
        public static bool OnMeasureShowData = false;

        public static bool ShowDw = true;
        #region tabTitle



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
                        Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.
                            ContainsKey(_rtuId))
                    {

                        this.RtuName = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
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

        #endregion

        private bool isload = false;
        void acload()
        {
            isload = true;
        }
        public NewDataViewModel()
        {
            EventPublisher.AddEventSubScriptionTokener(
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
            var info = ZNewData.NewDataSetting.NewDataSettingViewModel.LoadNewDataLenghtSetConfg();
            RowHeight = info.Item1;
            LoopNameLength = info.Item2;
            TimeNameLength = info.Item3;
            VaNameLength = info.Item4;
            RtuNameLength = info.Item5;
            IsShowLoopId = info.Item6;
            BackgroundColor = info.Item7.Background;
            K1BackgroundColor = info.Item7.K1Background;
            K2BackgroundColor = info.Item7.K2Background;
            K3BackgroundColor = info.Item7.K3Background;
            K4BackgroundColor = info.Item7.K4Background;
            K5BackgroundColor = info.Item7.K5Background;
            K6BackgroundColor = info.Item7.K6Background;
            OnMeasureShowData = info.Item7.OnMeasureShowData;

            InitAction();
        }


        void InitAction()
        {
            ProtocolServer.RegistProtocol(
               Wlst.Sr.ProtocolPhone .ClientListen .wlst_svr_ans_cnt_request_wj3090_near_measure_data ,//.ProtocolCnt.ClientPart.wlst_Measures_server_ans_clinet_request_Near_data,
               OnNearDataArrive,
               typeof(NewDataViewModel), this);
        }


        /// <summary>
        /// 页面加载或是导航显示的时候 需要执行的初始化操作
        /// </summary>
        /// <param name="parsObjects"></param>
        public void NavOnLoad(params object[] parsObjects)
        {

        }

        #region IEventAggregator Subscription

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
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentNewDataArrive:
                        return true;
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:

                        return true;

                    //case Sr .EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:
                    //    return true;
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.RtuDataQueryDataInfoNeedShowInTab:
                        return true;
                        //case Sr.EquipmentNewData .Services .EventIdAssign .RtuTimeArrive :
                        //    return true;
                }
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

        ///// <summary>
        ///// 事件执行服务器数据到达  更新
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //private void ExExecuteEvent(object obj)
        //{
        //    var args = obj as PublishEventArgs;
        //    if (args == null) return;
        //    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal,
        //                                          new Action<PublishEventArgs>(ExExecuteEventIns),
        //                                          args);
        //    return;
        //}

        /// <summary>
        /// 线程执行 具体执行
        /// </summary>
        private void ExExecuteEventIns(PublishEventArgs args)
        {
            try
            {
                switch (args.EventId)
                {
                    case Sr . EquipmentInfoHolding.Services . EventIdAssign.EquipmentNewDataArrive:
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

                        if (lst.Contains(RtuIdNeedUpdate))
                        {
                            OnSelectRtuIdChange(RtuIdNeedUpdate,false );
                        }
                        //else
                        //{
                        //    OnSelectRtuIdChange(lst[0]);
                        //}
                        break;
                    case Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected:

                        var rtuId = Convert.ToInt32(args.GetParams()[0]);
                        if ( rtuId > 1100000)
                    {
                        if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentIdAssignRang.IsSlu(rtuId)) break;

                        var tmps = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(rtuId);
                        if (tmps == null) return;
                        rtuId = tmps.AttachRtuId;
                    }
                        if (rtuId < 1000000 || rtuId > 1100000) return;

                        if (rtuId > 0)
                        {
                            this.OnSelectRtuIdChange(rtuId,true );
                        }
                        break;

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


         void OnSelectRtuIdChange(int rtuId,bool selected)
        {
            this.RtuIdNeedUpdate = rtuId;
            var fff = RtuNewDataService.GetInfoById(rtuId);
            //if (fff == null) return;
            //if (fff.LstNewLoopsData == null) return;

            OnDataChange(rtuId, fff,"",false  );
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

         void OnDataChange(int rtuId, RtuNewDataInfo fff, string attachinfo,bool isHistory)
         {
             var lineItems = new ObservableCollection<LineInfo>();

             var textBlockInfoItems = new ObservableCollection<TextBlockInfo>();

             var textBlock1InfoItems = new ObservableCollection<TextBlock1Info>();

             var arcItems = new ObservableCollection<ArcInfo>();


             var ellItems = new ObservableCollection<EllInfo>();

             try
             {

                 CurrentShowTmlNewData = fff;

                 var rtuState = "";
                 //this.RtuName = this.RtuId+""

                 ; int phyId = 0;
                 var tmpequ = Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(rtuId);
                 if (tmpequ != null)
                 {
                     rtuState = tmpequ.RtuState == 2 ? "使用" : tmpequ.RtuState == 1 ? "停运" : "不用";
                     //this.RtuName = tmpequ.RtuName;
                     phyId = tmpequ.PhyId;
                 }

                 this.RtuIdNeedUpdate = rtuId;
                 this.RtuId = rtuId;
                

                 var rtuName = tmpequ != null ? tmpequ.RtuName : "";
                 string GroupName = "";
                 var groupid =
                     Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(RtuId);
                 if (groupid != -1)
                 {
                     var infosss =
                         Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(groupid);
                     if (infosss != null) GroupName = infosss.GroupName;
                 }


                 //  var fff = Sr.EquipmentNewData.Services.RtuNewDataService.GetInfoById(rtuId);
                 if (fff == null)
                 {

                     this.EllItemss.Clear();
                     this.LineItemss.Clear();
                     this.TextBlockInfoItemss.Clear();
                     this.ArcItemss.Clear();


                     CanWidth = 355 + LoopNameLength + TimeNameLength + 4*VaNameLength;
                     CanHeight = 8*RowHeight + 65;
                     this.RtuId = rtuId;
                     //this.RtuName = this.RtuId + " - " + rtuName ;
                     this.AddBasicRtuInfo(ref lineItems, ref textBlockInfoItems,
                                         phyId .ToString("D4") + " - " + rtuName + "  " + GroupName + "  无数据",
                                          DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + rtuState);

                     //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
                     var swuout = new List<Tuple<int, bool, int, string>>();
                     //添加输出

                     for (int i = 1; i < 7; i++)
                     {
                         swuout.Add(new Tuple<int, bool, int, string>(i, false, 0, ConstColor[i]));
                     }
                     AddSitchOutInfo(ref textBlockInfoItems, ref textBlock1InfoItems, ref lineItems, rtuId, swuout,isHistory );


                     this.EllItemss = ellItems;
                     this.ArcItemss = arcItems;
                     this.LineItemss = lineItems;
                     this.TextBlockInfoItemss = textBlockInfoItems;
                     this.TextBlock1InfoItemss = textBlock1InfoItems;
                     this.BulidMenus(rtuId);
                     return;
                 }

                 //var rtuState = "";
                 //this.RtuName = this.RtuId+""
                 var title = "";
                 if (fff.Alarms.ContainsKey(1) && fff.Alarms[1]) title += "停电";
                 else title += "供电";
                 if (fff.Alarms.ContainsKey(3) && fff.Alarms[3]) title += "停运中";
                 else title += "使用中 ";
                 if (tmpequ != null)
                 {
                     rtuState = tmpequ.RtuState == 0 ? "不用" : title;
                     //this.RtuName = tmpequ.RtuName;
                 }

                 if (fff.LstNewLoopsData == null)
                 {
                     this.EllItemss.Clear();
                     this.LineItemss.Clear();
                     this.TextBlockInfoItemss.Clear();
                     this.ArcItemss.Clear();


                     CanWidth = 365 + LoopNameLength + TimeNameLength + 4*VaNameLength;
                     CanHeight = 8*RowHeight + 65;
                     this.RtuId = fff.RtuId;
                     this.DateTimeGetRtuTime = fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                     //this.RtuName = this.RtuId + " - " + fff.RtuName;
                     this.AddBasicRtuInfo(ref lineItems, ref textBlockInfoItems,
                                          phyId.ToString("D4") + " - " + rtuName + "  " + GroupName,
                                          fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "  " + rtuState);


                     //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
                     var swuout = new List<Tuple<int, bool, int, string>>();
                     //添加输出

                     for (int i = 1; i < 7; i++)
                     {
                         swuout.Add(new Tuple<int, bool, int, string>(i, false, 0, ConstColor[i]));
                     }
                     AddSitchOutInfo(ref textBlockInfoItems, ref textBlock1InfoItems, ref lineItems, rtuId, swuout,isHistory );


                     this.EllItemss = ellItems;
                     this.ArcItemss = arcItems;
                     this.LineItemss = lineItems;
                     this.TextBlockInfoItemss = textBlockInfoItems;
                     this.TextBlock1InfoItemss = textBlock1InfoItems;
                     this.BulidMenus(rtuId);
                     return;
                 }


                 this.DateTimeGetRtuTime = fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");








                 var dic = new Dictionary<int, List<RtuNewOneLoopDataInfo>>();

                 for (int i = 1; i < fff.IsSwitchOutAttraction.Count + 1; i++)
                 {
                     if (dic.ContainsKey(i)) continue;
                     dic.Add(i, new List<RtuNewOneLoopDataInfo>());
                 }
                 foreach (var t in fff.LstNewLoopsData)
                 {
                     if (!dic.ContainsKey(t.SwitchOutId)) dic.Add(t.SwitchOutId, new List<RtuNewOneLoopDataInfo>());
                     dic[t.SwitchOutId].Add(t);
                 }



                 this.AddBasicRtuInfo(ref lineItems, ref textBlockInfoItems,
                                     phyId.ToString("D4") + " - " + rtuName + "  " + GroupName,
                                      fff.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "  " + rtuState);


                 CanWidth = 365 + LoopNameLength + TimeNameLength + 4*VaNameLength;
                 //  CanHeight = fff.LstNewLoopsData.Count * RowHeight  + 30  ; ;
                 CanHeight = fff.LstNewLoopsData.Count*RowHeight + RowHeight +
                             (dic.Keys.Contains(0) ? dic.Count*10 - 10 : dic.Count*10); //CanHeight
                 if (CanHeight < 250) CanHeight = 245;


                 //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
                 List<Tuple<int, bool, int, string>> swout = new List<Tuple<int, bool, int, string>>();


                 //添加输出
                 var fffff = (from t in dic orderby t.Key select t).ToList();
                 foreach (var t in fffff)
                 {
                     if (t.Key < 1) continue;
                     bool isclose = true;
                     if (fff.IsSwitchOutAttraction.Count >= t.Key)
                     {
                         isclose = fff.IsSwitchOutAttraction[t.Key - 1];
                     }
                     swout.Add(new Tuple<int, bool, int, string>(t.Key, isclose, t.Value.Count, ConstColor[t.Key%6]));
                 }
                 AddSitchOutInfo(ref textBlockInfoItems, ref textBlock1InfoItems, ref lineItems, rtuId, swout,isHistory );


                 //添加回路
                 int xIndexCount = 0;
                 int xSoutCount = 0;

                 // var intfo = Wlst.Sr.EquipmentNewData.Services.LduNewDataServices.GetRtuLoopLduInfo(rtuId);
                 foreach (var t in fffff)
                 {
                     if (t.Key < 1) continue;

                     string color = ConstColor[t.Key%6];
                     foreach (var g in t.Value)
                     {

                         double v = 0;
                         double a = 0;
                         double power = 0;
                         double rate = 0;
                         try
                         {
                             v = Convert.ToDouble(g.V);
                             a = Convert.ToDouble(g.A);
                             power = Convert.ToDouble(g.Power);
                             rate = Convert.ToDouble(g.BrightRate);

                         }
                         catch (Exception ex)
                         {
                         }
                         ;
                         var tmp = Wlst.Sr.EquipmentInfoHolding.Services.RtuNewDataService.GetRtuLoopAttachInfo(rtuId,
                                                                                                                g.LoopId);
                         if (tmp == null)
                             AddLoopInfo(ref arcItems, ref ellItems, ref textBlockInfoItems, ref lineItems, xIndexCount,
                                         g.LoopId, g.LoopName, g.SwitchOutId, xSoutCount, g.BolSwitchInState, v, a,
                                         power,
                                         rate, g.Lower, g.Range, color, g.Range > 0,isHistory );
                         else
                         {
                             AddLoopInfo(ref arcItems, ref ellItems, ref textBlockInfoItems, ref lineItems, xIndexCount,
                                         g.LoopId, g.LoopName, g.SwitchOutId, xSoutCount, g.BolSwitchInState, v, a,
                                         power,
                                         rate, g.Lower, g.Range, color, g.Range > 0,isHistory , tmp.Item1,
                                         tmp.Item2 ? "Red" : color);
                         }
                         xIndexCount++;
                     }
                     xSoutCount++;
                 }


                 int subcount = 0;
                 //添加输入 入门
                 foreach (var t in fffff)
                 {
                     if (t.Key != 0) continue;



                     var tmps = (from g in t.Value orderby g.LoopId select g).ToList();
                     for (int i = 0; i < tmps.Count; i++)
                     {
                         //var loopnames = loopname.ContainsKey(tmps[i].LoopId)
                         //                  ? loopname[tmps[i].LoopId]
                         //                  :"D" + tmps[i].LoopId;


                         if (i < tmps.Count - 1 && tmps[i].LoopName.Contains("防盗") &&
                             tmps[i + 1].LoopName.Contains("检测器"))
                         {
                             if (tmps[i].BolSwitchInState != tmps[i + 1].BolSwitchInState)
                             {
                                 AddSwitchInInfo(ref textBlockInfoItems, ref lineItems, xIndexCount, tmps[i].LoopId,
                                                 xSoutCount, true, tmps[i].LoopName + " 正常",
                                                 "Gray");
                                 xIndexCount++;
                             }
                             else
                             {
                                 AddSwitchInInfo(ref textBlockInfoItems, ref lineItems, xIndexCount, tmps[i].LoopId,
                                                 xSoutCount, false, tmps[i].LoopName + " 异常",
                                                 "Red");
                                 xIndexCount++;
                             }
                             i++;
                             subcount++;
                             continue;
                         }
                         if (tmps[i].LoopName.Contains("门"))
                         {
                             var tmpssss = " 正常";
                             if (tmps[i].BolSwitchInState == false) tmpssss = " 打开";
                             AddSwitchInInfo(ref textBlockInfoItems, ref lineItems, xIndexCount, tmps[i].LoopId,
                                             xSoutCount, tmps[i].BolSwitchInState,
                                             tmps[i].LoopName + tmpssss,
                                             tmps[i].BolSwitchInState ? "Gray" : "Red");
                             xIndexCount++;

                             // subcount++;
                         }


                     }
                 }

                 if (subcount > 0)
                 {
                     CanHeight = (fff.LstNewLoopsData.Count - subcount)*RowHeight + RowHeight +
                                 (dic.Keys.Contains(0) ? dic.Count*10 - 10 : dic.Count*10);
                     ;
                     if (CanHeight < 220) CanHeight = 220;
                 }



                 //var infosss = Sr.EquipmentNewData.Services.TmlTimeDataServices.GetInfoById(rtuId);
                 //if (infosss != null)
                 //{
                 //    this.DateTimeGetedRtuTime = infosss.Item1.ToString(CultureInfo.InvariantCulture) + "  获取到终端时钟：" +
                 //                                infosss.Item2;
                 //    // this.DateTimeGetedRtuTime = infosss.Item2;
                 //    this.AddRtuTime(infosss.Item1.ToString(CultureInfo.InvariantCulture) + "  获取到终端时钟：" + infosss.Item2);
                 //}
                 //else
                 //{
                 //    this.DateTimeGetRtuTime = "";
                 //    this.DateTimeGetedRtuTime = "";
                 //}

                 var strinfoxfds =ShowDw? "[A]":"";

                 var suminfo = "A相总电流:" + string.Format("{0:0.00}", fff.RtuCurrentSumA) + strinfoxfds; //string.Format("{0:0.00}", v)
                 suminfo += "      ";
                 suminfo += "B相总电流:" + string.Format("{0:0.00}", fff.RtuCurrentSumB) + strinfoxfds;
                 suminfo += "      ";
                 suminfo += "C相总电流:" + string.Format("{0:0.00}", fff.RtuCurrentSumC) + strinfoxfds;
                 suminfo += "      ";
                 suminfo += attachinfo;
                 this.AddRtuCurrentSumTime(ref textBlockInfoItems, suminfo);

             }
             catch (Exception ex)
             {
                 WriteLog.WriteLogError("On NewDataView change rtu error:" + ex);
             }

             this.EllItemss = ellItems;
             this.ArcItemss = arcItems;
             this.LineItemss = lineItems;
             this.TextBlockInfoItemss = textBlockInfoItems;
             this.TextBlock1InfoItemss = textBlock1InfoItems;
             LineItemsDash.Clear();
             this.BulidMenus(rtuId);
         }


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
            try
            {
                Cm.Items.Clear();
                // ObservableCollection<IIMenuItem> t = null;
                if (Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.ContainsKey(rtuId))
                {
                    var tt = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[rtuId];

                    var tmt = MenuBuilding.BulidCm(tt.RtuModel.ToString(), false, tt);
                    var tmp = MenuBuilding.HelpCmm(tmt);
                    Cm.Items.Add(new MenuItem(){Header = tt.RtuName ,IsEnabled =false  });
                    foreach (var t in tmp) Cm.Items.Add(t);
                    this.RaisePropertyChanged(() => Cm);
                }
            }
            catch (Exception ex)
            {
            }
        }


        public void MeasureRtu()
        {
            try
            {
                if (RtuId < 1000000 || RtuId > 1100000) return;

                var info = Wlst.Sr.ProtocolPhone.ServerListen.wlst_cnt_request_wj3090_measure;
                info.Args .Addr .Add(this.RtuId);
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
             var info = Wlst.Sr.ProtocolPhone.ServerListen.wlst_cnt_request_wj3090_near_measure_data;
             info.WstCntRequestWj3090NearMeasureData .RequestEndTime = CurrentShowTmlNewData.DateCreate.Ticks ;
             info.WstCntRequestWj3090NearMeasureData.RequestStartTime = CurrentShowTmlNewData.DateCreate.Ticks ;
             info.WstCntRequestWj3090NearMeasureData.Tml = CurrentShowTmlNewData.RtuId;
             SndOrderServer.OrderSnd(info, 10, 2);
         }

         void OnNearDataArrive(string session,Wlst .mobile .MsgWithMobile  infos)
         {
             if (infos == null || infos.WstSvrAnsCntRequestWj3090NearMeasureData == null || infos.WstSvrAnsCntRequestWj3090NearMeasureData.LstInfo == null) return;

             if (infos.WstSvrAnsCntRequestWj3090NearMeasureData.LstInfo.Count == 0) return;
             var mtps = new List<RtuNewDataInfo>();
             foreach (var g in infos.WstSvrAnsCntRequestWj3090NearMeasureData.LstInfo)
             {
                 mtps.Add(new RtuNewDataInfo(g));
             }
             OnNearTwoDataArrive(mtps);
         }

         void OnNearTwoDataArrive(List<RtuNewDataInfo> fff)
         {
             if (CurrentShowTmlNewData == null) return;
             if (fff.Count == 0) return;
             if (CurrentShowTmlNewData.RtuId != fff[0].RtuId) return;
             if (CurrentShowTmlNewData.LstNewLoopsData == null || CurrentShowTmlNewData.LstNewLoopsData.Count == 0)
                 return;

           

             var lineItems = new ObservableCollection<LineInfo>();
             var lineItemsdash = new ObservableCollection<LineInfo>();

             var textBlockInfoItems = new ObservableCollection<TextBlockInfo>();

             var textBlock1InfoItems = new ObservableCollection<TextBlock1Info>();

             var arcItems = new ObservableCollection<ArcInfo>();


             var ellItems = new ObservableCollection<EllInfo>();

             try
             {

                 var tmpequ =
                     Wlst.Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(
                         CurrentShowTmlNewData.RtuId);
                 if (tmpequ == null) return;
                 this.AddBasicRtuInfoExtend(ref lineItems, ref textBlockInfoItems,
                                            tmpequ.PhyId.ToString("D4") + " - " + tmpequ.RtuName,CurrentShowTmlNewData .DateCreate .ToString("yyyy-MM-dd HH:mm:ss"));





                 fff.Insert(0,CurrentShowTmlNewData );
                 int maxHiehgt = 0;
                 for (int gggxxx = 1; gggxxx < fff.Count + 1; gggxxx++)
                 {
                     var dic = new Dictionary<int, List<RtuNewOneLoopDataInfo>>();

                     for (int i = 1; i < fff [gggxxx -1].IsSwitchOutAttraction.Count + 1; i++)
                     {
                         if (dic.ContainsKey(i)) continue;
                         dic.Add(i, new List<RtuNewOneLoopDataInfo>());
                     }
                     foreach (var t in fff[gggxxx - 1].LstNewLoopsData)
                     {
                         if (!dic.ContainsKey(t.SwitchOutId)) dic.Add(t.SwitchOutId, new List<RtuNewOneLoopDataInfo>());
                         dic[t.SwitchOutId].Add(t);
                     }


                     //开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色
                     List<Tuple<int, bool, int, string>> swout = new List<Tuple<int, bool, int, string>>();


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
                         swout.Add(new Tuple<int, bool, int, string>(t.Key, isclose, t.Value.Count, ConstColor[t.Key%6]));
                     }

                     AddSitchOutInfoExtend(ref textBlock1InfoItems, ref lineItems, swout, gggxxx );


                     //添加回路
                     int xIndexCount = 0;
                     int xSoutCount = 0;

                     // var intfo = Wlst.Sr.EquipmentNewData.Services.LduNewDataServices.GetRtuLoopLduInfo(rtuId);
                     foreach (var t in fffff)
                     {
                         if (t.Key < 1) continue;

                         string color = ConstColor[t.Key%6];
                         foreach (var g in t.Value)
                         {

                             double v = 0;
                             double a = 0;
                             double power = 0;

                             try
                             {
                                 v = Convert.ToDouble(g.V);
                                 a = Convert.ToDouble(g.A);
                                 power = Convert.ToDouble(g.Power);
                             }
                             catch (Exception ex)
                             {
                             }
                             AddLoopInfoExtend(ref ellItems, ref textBlockInfoItems, ref lineItems,ref lineItemsdash , xIndexCount,
                                               g.LoopId, g.LoopName, g.SwitchOutId, g.BolSwitchInState, v, a,
                                               power, color, g.Range > 0, gggxxx);

                             xIndexCount++;
                         }
                         xSoutCount++;
                     }


                     //int subcount = 0;
                     ////添加输入 入门
                     //foreach (var t in fffff)
                     //{
                     //    if (t.Key != 0) continue;



                     //    var tmps = (from g in t.Value orderby g.LoopId select g).ToList();
                     //    for (int i = 0; i < tmps.Count; i++)
                     //    {
                     //        //var loopnames = loopname.ContainsKey(tmps[i].LoopId)
                     //        //                  ? loopname[tmps[i].LoopId]
                     //        //                  :"D" + tmps[i].LoopId;


                     //        if (i < tmps.Count - 1 && tmps[i].LoopName.Contains("防盗") &&
                     //            tmps[i + 1].LoopName.Contains("检测器"))
                     //        {
                     //            if (tmps[i].BolSwitchInState != tmps[i + 1].BolSwitchInState)
                     //            {
                     //                AddSwitchInInfoExtend(ref textBlockInfoItems, ref lineItems, xIndexCount,
                     //                                      tmps[i].LoopId,
                     //                                      xSoutCount, true, tmps[i].LoopName + " 正常",
                     //                                      "Gray");
                     //                xIndexCount++;
                     //            }
                     //            else
                     //            {
                     //                AddSwitchInInfo(ref textBlockInfoItems, ref lineItems, xIndexCount, tmps[i].LoopId,
                     //                                xSoutCount, false, tmps[i].LoopName + " 异常",
                     //                                "Red");
                     //                xIndexCount++;
                     //            }
                     //            i++;
                     //            subcount++;
                     //            continue;
                     //        }
                     //        if (tmps[i].LoopName.Contains("门"))
                     //        {
                     //            var tmpssss = " 正常";
                     //            if (tmps[i].BolSwitchInState == false) tmpssss = " 打开";
                     //            AddSwitchInInfoExtend(ref textBlockInfoItems, ref lineItems, xIndexCount,
                     //                                  tmps[i].LoopId,
                     //                                  xSoutCount, tmps[i].BolSwitchInState,
                     //                                  tmps[i].LoopName + tmpssss,
                     //                                  tmps[i].BolSwitchInState ? "Gray" : "Red");
                     //            xIndexCount++;

                     //            // subcount++;
                     //        }


                     //    }
                     //}

                     //if (subcount > 0)
                     //{
                     //    var maxHiehgtx = (fff[gggxxx - 1].LstNewLoopsData.Count - subcount) * RowHeight + RowHeight +
                     //                (dic.Keys.Contains(0) ? dic.Count*10 - 10 : dic.Count*10);
                     //    ; if (maxHiehgtx > maxHiehgt) maxHiehgt = maxHiehgtx;
                     //    if (CanHeight < 220) CanHeight = 220;
                     //}
                     //else
                     //{
                         CanWidth = 170 + LoopNameLength + (250 + 110) * gggxxx;
                         //  CanHeight = fff.LstNewLoopsData.Count * RowHeight  + 30  ; ;
                       var   maxHiehgtx = fff[gggxxx - 1].LstNewLoopsData.Count * RowHeight + 2*RowHeight +
                                     (dic.Keys.Contains(0) ? dic.Count*10 - 10 : dic.Count*10); //CanHeight
                       if (maxHiehgtx > maxHiehgt) maxHiehgt = maxHiehgtx;
                       if (maxHiehgt < (4 + dic.Count) * RowHeight) maxHiehgt = (4 + dic.Count)*RowHeight;
                       if (maxHiehgt < 250) maxHiehgt = 245;

                    // }


                    // var suminfo = fff[gggxxx - 1].DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
                   var    suminfo = "A相:" + string.Format("{0:0.00}", fff[gggxxx - 1].RtuCurrentSumA);
                     //string.Format("{0:0.00}", v)
                     suminfo += " B相:" + string.Format("{0:0.00}", fff[gggxxx - 1].RtuCurrentSumB);
                     suminfo += " C相:" + string.Format("{0:0.00}", fff[gggxxx - 1].RtuCurrentSumC);

                  //   this.AddRtuCurrentSumTime(ref textBlockInfoItems, suminfo);

                     var startxgt = 170;
                     if (gggxxx  == 1) startxgt = 170;  //60+3*LoopNameLength
                     if (gggxxx == 2) startxgt = 170 + LoopNameLength + 300 ;
                     if (gggxxx == 3) startxgt = 170 + LoopNameLength + 250 + 110 + 295;
                     lineItems.Add(new LineInfo() //--
                                       {
                                           Color = K1BackgroundColor,
                                           // "AliceBlue",
                                           Index = 0,

                                           X1 = startxgt,
                                           X2 = startxgt+250,
                                           Y1 = RowHeight*2,
                                           Y2 = RowHeight*2
                                       });

                     textBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
                                                {
                                                    BorderThinkness = 0,
                                                    Color = K1BackgroundColor,
                                                    // "Blue",
                                                    CornerRadius = 0,
                                                    Height = RowHeight,
                                                    Index = 0,
                                                    Left = startxgt,
                                                    HorizontalAlign = HorizontalAlignment.Left,


                                                    Text = suminfo,
                                                    Top = RowHeight,
                                                    Width = 245
                                                });

                     if(gggxxx >1)
                     {
                         textBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
                         {
                             BorderThinkness = 0,
                             Color = K1BackgroundColor,
                             // "Blue",
                             CornerRadius = 0,
                             Height = RowHeight,
                             Index = 0,
                             Left = startxgt,
                             HorizontalAlign = HorizontalAlignment.Left,


                             Text =  fff[gggxxx - 1].DateCreate.ToString("yyyy-MM-dd HH:mm:ss"),
                             Top = 0,
                             Width = 245
                         });
                     }

                 }

                 CanHeight = maxHiehgt;
             }
             catch (Exception ex)
             {
                 WriteLog.WriteLogError("On NewDataView change rtu error:" + ex);
             }


             
             this.EllItemss = ellItems;
             this.ArcItemss = arcItems;
             this.LineItemss = lineItems;
             this.LineItemsDash = lineItemsdash;
             this.TextBlockInfoItemss = textBlockInfoItems;
             this.TextBlock1InfoItemss = textBlock1InfoItems;
             this.BulidMenus(CurrentShowTmlNewData.RtuId);
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
             Mit.Text = "终端";
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
         private void AddSitchOutInfoExtend(ref ObservableCollection<TextBlock1Info> TextBlock1InfoItems, ref ObservableCollection<LineInfo> LineItems, List<Tuple<int, bool, int, string>> swout, int indexView)
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


         private void AddLoopInfoExtend( ref ObservableCollection<EllInfo> EllItems,
          ref ObservableCollection<TextBlockInfo> TextBlockInfoItems, ref ObservableCollection<LineInfo> LineItems,ref ObservableCollection<LineInfo> lineItemsdash,
          int index, int loopId, string name, int pastswitchoutcount, bool switchIsClose, double v, double a, double power, string color, bool used,int indexView)
         {

             var startxgt = 170;
             if (indexView == 1) startxgt = 170;  //60+3*LoopNameLength
             if (indexView == 2) startxgt = 170 + LoopNameLength + 250 + 110 ;
             if (indexView == 3) startxgt = 170 + LoopNameLength + 250 + 110 + 250 + 110;


             int startx = startxgt ;
             int starty = RowHeight * 3;

             if (indexView == 1)
             {
                 if (LoopNameLength < 50) LoopNameLength = 50;

                 LineItems.Add(new LineInfo() //--
                                   {
                                       Color = color,
                                       Index = 0,

                                       X1 = startx,
                                       X2 = startx + LoopNameLength,
                                       Y1 = starty + index*RowHeight + pastswitchoutcount*10,
                                       Y2 = starty + index*RowHeight + pastswitchoutcount*10
                                   });

                 string showname = name;
                 if (IsShowLoopId) showname = loopId.ToString("D2") + "-" + name;

                 TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
                                            {
                                                BorderThinkness = 0,
                                                Color = color,
                                                CornerRadius = 0,
                                                Height = RowHeight,
                                                Index = 0,
                                                HorizontalAlign = HorizontalAlignment.Left,
                                                Left = startx + 3,

                                                Text = showname,
                                                Top = starty - RowHeight + index*RowHeight + pastswitchoutcount*10,
                                                Width = LoopNameLength 
                                            });
                 startx += LoopNameLength;
             }
             else
             {
                 lineItemsdash.Add(new LineInfo() // -- KiLj /
                 {
                     Color = color,
                     Index = 0,

                     X1 = startx-200,
                     X2 = startx ,
                     Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                     Y2 = starty + index * RowHeight + pastswitchoutcount * 10,
                 });
             }

             LineItems.Add(new LineInfo() // -- KiLj /
             {
                 Color = color,
                 Index = 0,

                 X1 = startx,
                 X2 = startx + 5,
                 Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                 Y2 = starty + index * RowHeight + pastswitchoutcount * 10
             });
             startx += 5;

             EllItems.Add(new EllInfo()
             {
                 Color = BackgroundColor,// "Transparent",
                 Left = startx,
                 Top = starty - 4 + index * RowHeight + pastswitchoutcount * 10,
                 Wide = 8,
             });

             LineItems.Add(new LineInfo() // -- KiLj /
             {
                 Color = color,
                 Index = 0,

                 X1 = startx,
                 X2 = startx + 40,
                 Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                 Y2 =
                     switchIsClose
                         ? (starty - 3 + index * RowHeight + pastswitchoutcount * 10)
                         : (starty - 15 + index * RowHeight + pastswitchoutcount * 10)
             });
             startx += 30;


             EllItems.Add(new EllInfo()
             {
                 Color = BackgroundColor,
                 Left = startx,
                 Top = starty - 4 + index * RowHeight + pastswitchoutcount * 10,
                 Wide = 8,
             });


             LineItems.Add(new LineInfo() // -- KiLj /--
             {
                 Color = color,
                 Index = 0,

                 X1 = startx,
                 X2 = startx + 215,
                 Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                 Y2 = starty + index * RowHeight + pastswitchoutcount * 10
             });
             //startx += 355;

             var x11 = v.ToString("f2")+" ";
             var x12 = a.ToString("f2")+" ";
             var x13 = power.ToString("f2");
             for (int jj=x11 .Count( );jj<7;jj++)
             {
                 x11 = "  "+x11 ;
             }
             for (int jj = x12.Count(); jj < 7; jj++)
             {
                 x12 = "  " + x12;
             }
             for (int jj = x13.Count(); jj < 7; jj++)
             {
                 x13 = "  " + x13;
             }

             var tmptxt = x11 +
                          (used ? x12: "----- ") +
                          (used ? x13  : "----- ");

             TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj /--
             {
                 BorderThinkness = 0,
                 Color = color,
                 CornerRadius = 0,
                 Height = RowHeight,
                 Index = 0,
                 Left = startx + 10,

                 Text =indexView >1? ((index+1) .ToString("D2")+"-"+ tmptxt):tmptxt ,
                 Top = starty - RowHeight + index * RowHeight + pastswitchoutcount * 10,
                 Width = 210,
                 HorizontalAlign = HorizontalAlignment.Left,
             });
             //startx += 55 + 4 * VaNameLength;
             

         }


         private void AddSwitchInInfoExtend(ref ObservableCollection<TextBlockInfo> TextBlockInfoItems, ref ObservableCollection<LineInfo> LineItems, int index, int loopId, int pastswitchoutcount, bool switchIsClose, string loopName, string color)
         {

             int startx = 200 + TimeNameLength;
             int starty = 15;

             index += 1;

             LineItems.Add(new LineInfo() //--
             {
                 Color = color,
                 Index = 0,

                 X1 = startx,
                 X2 = startx + LoopNameLength,
                 Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                 Y2 = starty + index * RowHeight + pastswitchoutcount * 10
             });


             LineItems.Add(new LineInfo() // -- KiLj /
             {
                 Color = color,
                 Index = 0,

                 X1 = startx + LoopNameLength,
                 X2 = startx + LoopNameLength + 30,
                 Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                 Y2 =
                     switchIsClose
                         ? (starty + index * RowHeight + pastswitchoutcount * 10)
                         : (starty - 10 + index * RowHeight + pastswitchoutcount * 10)
             });

             TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
             {
                 BorderThinkness = 0,
                 Color = color,
                 CornerRadius = 0,
                 Height = RowHeight,
                 Index = 0,
                 Left = startx + 5,

                 Text = " D" + loopId,
                 Top = starty - RowHeight + index * RowHeight + pastswitchoutcount * 10,
                 Width = 45,
                 HorizontalAlign = HorizontalAlignment.Left
             });


             LineItems.Add(new LineInfo() // -- KiLj /
             {
                 Color = color,
                 Index = 0,

                 X1 = startx + LoopNameLength + 30,
                 X2 = startx + LoopNameLength + 430,
                 Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                 Y2 = starty + index * RowHeight + pastswitchoutcount * 10
             });


             TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj /--
             {
                 BorderThinkness = 0,
                 Color = color,
                 CornerRadius = 0,
                 Height = RowHeight,
                 Index = 0,
                 Left = startx + LoopNameLength + 50,

                 Text = loopName,
                 Top = starty - RowHeight + index * RowHeight + pastswitchoutcount * 10,
                 Width = 250
             });

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

            var info = ZNewData.NewDataSetting.NewDataSettingViewModel.LoadNewDataLenghtSetConfg();
            string[] myColor = new string[7]{
                                              info.Item7.Background,
                                              info.Item7.K1Background, 
                                              info.Item7.K2Background, 
                                              info.Item7.K3Background, 
                                              info.Item7.K4Background,
                                              info.Item7.K5Background ,
                                              info.Item7.K6Background
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

        private ObservableCollection<ArcInfo> _arcItemss;
        public ObservableCollection<ArcInfo> ArcItemss
        {
            get
            {
                if (_arcItemss == null) _arcItemss = new ObservableCollection<ArcInfo>();
                return _arcItemss;
            }
            set
            {
                if (_arcItemss != value)
                {
                    _arcItemss = value;
                    this.RaisePropertyChanged(() => this.ArcItemss);
                }
            }
        }

        private ObservableCollection<EllInfo> _ellItemss;
        public ObservableCollection<EllInfo> EllItemss
        {
            get
            {
                if (_ellItemss == null) _ellItemss = new ObservableCollection<EllInfo>();
                return _ellItemss;
            }
            set
            {
                if(_ellItemss!=value )
                {
                    _ellItemss = value;
                    this.RaisePropertyChanged(() => this.EllItemss);
                }
            }
        }
     
        
        
        
        #endregion


        private void AddBasicRtuInfo(ref ObservableCollection<LineInfo> LineItems, ref ObservableCollection<TextBlockInfo> TextBlockInfoItems, string rtuName, string getInfoTime)
        {
            LineItems.Add(new LineInfo() //--
            {
                Color =K1BackgroundColor,// "AliceBlue",
                Index = 0,
                
                X1 = 35,
                X2 = 35,
                Y1 = RowHeight *2,
                Y2 = 150
            });
            LineItems.Add(new LineInfo() //--
            {
                Color = K1BackgroundColor,// "AliceBlue",
                Index = 0,
                
                X1 = 10,
                X2 = 150,
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

            
                Mitx.BorderThinkness = 0;
            Mitx.Color = K1BackgroundColor;// "Blue";
            Mitx.CornerRadius = 0;
            Mitx.Height = RowHeight;
            Mitx.Index = 0;
            Mitx.Left = 10;
            Mitx.Text = getInfoTime;
            Mitx.Top = RowHeight;
            Mitx.Width = 270;
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
            Mit.Text = "终端" ;
            Mit.Top = 150;


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

        private void AddRtuCurrentSumTime( ref ObservableCollection<TextBlockInfo> TextBlockInfoItems,string timeinfo)
        {
            for (int i = TextBlockInfoItems.Count - 1; i >= 0; i--)
            {
                if (TextBlockInfoItems[i].Index == 123)
                {
                    TextBlockInfoItems.RemoveAt(i);
                    break;
                }
            }

            TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
            {
                BorderThinkness = 0,
                Color = K1BackgroundColor,// "Blue",
                CornerRadius = 0,
                Height = RowHeight ,
                Index = 123,
                Left = RtuNameLength+15,
                HorizontalAlign = HorizontalAlignment.Left,
                Tooltips = "终端电流分相总电流。",
                Text = timeinfo,
                Top = 0,
                Width = 600
            });
        }

        /// <summary>
        /// 添加开关量输出信息
        /// </summary>
        /// <param name="swout">开关量输出信息列表 其中：输入回路地址，回路是否处于关闭状态，本输出下的回路路数，本回路的标记颜色</param>
        /// <param name="color">绘图前面部分绘图颜色 默认blue</param>
        private void AddSitchOutInfo(ref ObservableCollection<TextBlockInfo> TextBlockInfoItems, ref ObservableCollection<TextBlock1Info> TextBlock1InfoItems, ref ObservableCollection<LineInfo> LineItems, int rtuId, List<Tuple<int, bool, int, string>> swout,bool isHistory , string color = "Blue")
        {
            int loopsCount = 0;

            //int startx = 70;
            int starty = 3*RowHeight ;
            var isrtuinholidayinfo = Wlst .Sr .TimeTableSystem .Services .HolidayTimeandBandingServices .Myself .IsRtuInHoliday( rtuId );
            var holidayInfo = new List<string>();
            if (isrtuinholidayinfo)
                holidayInfo =
                    Wlst.Sr.TimeTableSystem.Services.HolidayTimeandBandingServices.Myself.
                        GetRtuSwitchOutOpenCloseTimeInholiday(rtuId);




            for (int i = 0; i < swout.Count; i++)
            {
                if(swout [i].Item1 ==1)
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

                int startx = 70;
                //starty += 10;
                //前面一点点
                LineItems.Add(new LineInfo() //--
                {
                    Color = color,
                    Index = 0,
                    
                    X1 = startx,
                    X2 = startx + TimeNameLength+10,
                    Y1 = starty + i * RowHeight ,
                    Y2 = starty + i * RowHeight
                });

                if (isHistory == false)
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

                        var rtuIdOrGrpId =
                            Wlst.Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuBelongGrp(rtuId);


                        if (rtuIdOrGrpId < 1) rtuIdOrGrpId = rtuId;

                        var tmp =
                            Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.
                                GetTmlLoopBandTimeTableTodayOpenCloseTimex(
                                    rtuIdOrGrpId, swout[i].Item1, out yesterday);

                        if (tmp != null)
                        {
                            //var name = "";
                            //if (tmp.TimeTableName.Length >= 3)
                            //    name += tmp.TimeTableName.Substring(0, 3);
                            //else
                            //    name += tmp.TimeTableName;
                            //name += ": ";
                            if (tmp.OpenLightTime == 1500)
                                name += "---";
                            else
                                name += string.Format("{0:D2}", tmp.OpenLightTime/60) + ":" +
                                        string.Format("{0:D2}", tmp.OpenLightTime%60);
                            name += " - ";

                            if (tmp.CloseLightTime == 1500)
                                name += "---";
                            else
                                name += string.Format("{0:D2}", tmp.CloseLightTime/60) + ":" +
                                        string.Format("{0:D2}", tmp.CloseLightTime%60);

                            //////////////提示 信息 
                            var str = new List<string>();
                            if (yesterday != null)
                            {
                                if (yesterday.OpenLightTime > yesterday.CloseLightTime &&
                                    yesterday.OpenLightTime < 1500 &&
                                    yesterday.CloseLightTime > 0)
                                {
                                    str.Add("关灯:" + string.Format("{0:D2}", yesterday.CloseLightTime/60) + ":" +
                                            string.Format("{0:D2}", yesterday.CloseLightTime%60));
                                }
                            }
                            if (tmp.OpenLightTime > 0 && tmp.OpenLightTime < 1500)
                            {
                                str.Add("开灯:" + string.Format("{0:D2}", tmp.OpenLightTime/60) + ":" +
                                        string.Format("{0:D2}", tmp.OpenLightTime%60));
                            }
                            if (tmp.OpenLightTime < 1500 && tmp.CloseLightTime < 1500 &&
                                tmp.CloseLightTime > tmp.OpenLightTime && tmp.CloseLightTime > 0)
                            {
                                str.Add("关灯:" + string.Format("{0:D2}", tmp.CloseLightTime/60) + ":" +
                                        string.Format("{0:D2}", tmp.CloseLightTime%60));
                            }

                            toolinfo = "今日操作:";
                            if (str.Count == 0) toolinfo += "无;";
                            foreach (var t in str)
                            {
                                toolinfo += t + ",";
                            }
                            toolinfo = toolinfo.Substring(0, toolinfo.Length - 1);
                            toolinfo += ";";
                        }
                        else
                        {
                            name = "--- - ---";
                            toolinfo = "今日操作:无;";
                            if (yesterday != null)
                            {
                                if (yesterday.OpenLightTime > yesterday.CloseLightTime &&
                                    yesterday.OpenLightTime < 1500 &&
                                    yesterday.CloseLightTime > 0)
                                {
                                    toolinfo = "今日操作:关灯:" + string.Format("{0:D2}", yesterday.CloseLightTime/60) + ":" +
                                               string.Format("{0:D2}", yesterday.CloseLightTime%60) + ";";
                                }
                            }

                        }
                        timeInfo = name;
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
                                                   Left = startx + addd,
                                                   Tooltips = toolinfo,
                                                   Text = timeInfo,
                                                   Top = starty + i*RowHeight - RowHeight,
                                                   //-20
                                                   HorizontalAlign = HorizontalAlignment.Left,
                                                   Width = TimeNameLength + 10
                                               });
                }
                //}
                //else
                //{
                //    TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
                //    {
                //        BorderThinkness = 0,
                //        Color = color,
                //        CornerRadius = 0,
                //        Height = 20,
                //        Index = 0,
                //        Left = startx + 10,
                //        Tooltips = "回路K" + swout[i].Item1 + " 今天的开灯关灯时间。",
                //        Text = "--- - ---",
                //        Top = starty + i * 30 - 20,
                //        HorizontalAlign =HorizontalAlignment.Left,
                //        Width = 145
                //    });
                //}

                startx += TimeNameLength+10; //220
                //LineItems.Add(new LineInfo() // == /
                //{
                //    Color = color,
                //    Index = 0,
                //    
                //    X1 = startx+20,
                //    X2 = startx+50,
                //    Y1 = starty + i * 30,
                //    Y2 = swout[i].Item2 ? (starty + i * 30) : (starty-10 + i * 30),
                //});

                //LineItems.Add(new LineInfo() //-- / --
                //{
                //    Color = swout[i].Item4 ,
                //    Index = 0,
                //    
                //    X1 = startx+50,
                //    X2 = startx+70,
                //    Y1 = starty + i * 30,
                //    Y2 = starty + i * 30
                //});
                string temp = BackgroundColor;
                //switch (swout[i].Item1)
                //{
                //    case 1:
                //        temp = K1BackgroundColor;
                //        break;
                //    case 2:
                //        temp = K2BackgroundColor;
                //        break;
                //    case 3:
                //        temp = K3BackgroundColor;
                //        break;
                //    case 4:
                //        temp = K4BackgroundColor;
                //        break;
                //    case 5:
                //        temp = K5BackgroundColor;
                //        break;
                //    case 6:
                //        temp = K6BackgroundColor;
                //        break;
                //    default:
                //        temp = BackgroundColor;
                //        break;
                //}
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

                //LineItems.Add(new LineInfo() //-- / -- K1 -- /
                //{
                //    Color = color,
                //    Index = 0,
                    
                //    X1 = startx,
                //    X2 = startx + 30,
                //    Y1 = starty + i * RowHeight,
                //    Y2 = swout[i].Item2 ? (starty + i * RowHeight) : (starty - 10 + i * RowHeight),
                //});
                //startx += 30;//300

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
                    Y1 = starty -RowHeight  + loopsCount * RowHeight + i * 10-RowHeight /2,
                    Y2 = starty  + loopsCount * RowHeight + (swout[i].Item3 - 1) * RowHeight + i * 10 -RowHeight /2
                });

                loopsCount += swout[i].Item3;
            }
        }


        /// <summary>
        /// 添加回路
        /// </summary>
        /// <param name="index">回路所在绘图的第几个 0开始</param>
        /// <param name="loopId">回路地址</param>
        /// <param name="name">回路名称 </param>
        /// <param name="switchoutid">回路所在的开关量输出地址 1-6</param>
        /// <param name="pastswitchoutcount">在绘本回路之前已经绘了几个开关量输出</param>
        /// <param name="switchIsClose">本回路开关量输入是否闭合</param>
        /// <param name="v">电压</param>
        /// <param name="a">电流</param>
        /// <param name="power">功率</param>
        /// <param name="rate">亮灯率</param>
        /// <param name="range">上限 </param>
        /// <param name="color">本回路颜色</param>
        /// <param name="lower"> 下限</param>
        /// <param name="used">回路是否使用中 量程是否为不为0 </param>
        /// <param name="attachInfo">附加显示信息 </param>
        private void AddLoopInfo(ref ObservableCollection<ArcInfo> ArcItems, ref ObservableCollection<EllInfo> EllItems,
            ref ObservableCollection<TextBlockInfo> TextBlockInfoItems, ref ObservableCollection<LineInfo> LineItems,
            int index, int loopId, string name, int switchoutid, int pastswitchoutcount, bool switchIsClose, double v, double a, double power, double rate, int lower, int range, string color, bool used, bool isHistory , string attachInfo = null, string attachcolor = "Red")
        {

            int startx = 170 + TimeNameLength;
            int starty = RowHeight *2;

if (LoopNameLength < 50) LoopNameLength = 50;

            LineItems.Add(new LineInfo() //--
            {
                Color = color,
                Index = 0,
                
                X1 = startx,
                X2 = startx + LoopNameLength,
                Y1 = starty + index * RowHeight  + pastswitchoutcount * 10,
                Y2 = starty + index * RowHeight + pastswitchoutcount * 10
            });

            string showname = name;
            if (IsShowLoopId) showname = loopId.ToString("D2") + "-" + name;

            TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
            {
                BorderThinkness = 0,
                Color = color,
                CornerRadius = 0,
                Height = RowHeight,
                Index = 0,
                HorizontalAlign = HorizontalAlignment.Left,
                Left = startx + 3,

                Text = showname,
                Top = starty - RowHeight + index * RowHeight + pastswitchoutcount * 10,
                Width = LoopNameLength-3
            }); 
            startx += LoopNameLength;


            EllItems.Add(new EllInfo()
            {
                Color = BackgroundColor,// "Transparent",
                Left = startx,
                Top = starty -4 + index * RowHeight + pastswitchoutcount * 10,
                Wide = 8,
            });

            LineItems.Add(new LineInfo() // -- KiLj /
            {
                Color = color,
                Index = 0,
                
                X1 = startx ,
                X2 = startx + 40,
                Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                Y2 =
                    switchIsClose
                        ? (starty -3+ index * RowHeight + pastswitchoutcount * 10)
                        : (starty - 15 + index * RowHeight + pastswitchoutcount * 10)
            });
            startx += 30;


            EllItems.Add(new EllInfo()
            {
                Color = BackgroundColor,
                Left = startx,
                Top = starty - 4 + index * RowHeight + pastswitchoutcount * 10,
                Wide = 8,
            });
           

            LineItems.Add(new LineInfo() // -- KiLj /--
            {
                Color = color,
                Index = 0,
                
                X1 = startx ,
                X2 = startx + 35 + 5 * VaNameLength,
                Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                Y2 = starty + index * RowHeight + pastswitchoutcount * 10
            });
            //startx += 355;

            #region  计算电压电流功率显示数据格式
           
            var pws = "0.0";
            if(used && a >0 && v >0 && power >0)
            {
                var exr = power * 1000 / (v * a);
                if (exr > 1 && exr < 1.2) exr = 1;
                pws = string.Format("{0:0.00}", exr );
            }

            #endregion

            TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj /--
            {
                BorderThinkness = 0,
                Color = color,
                CornerRadius = 0,
                Height = RowHeight,
                Index = 0,
                Left = startx + 10,

                Text = ShowDw ? string.Format("{0:0.00}", v) + " [V]" : string.Format("{0:0.00}", v),
                Top = starty - RowHeight + index * RowHeight + pastswitchoutcount * 10,
                Width = VaNameLength,
                HorizontalAlign = HorizontalAlignment.Right,
            });


            TextBlockInfoItems.Add(new TextBlockInfo()
            {
                BorderThinkness = 0,
                Color = color,
                CornerRadius = 0,
                Height = RowHeight,
                Index = 0,
                Left = startx + 15 + VaNameLength,

                Text = used ? ShowDw ? string.Format("{0:0.00}", a) + " [A]" : string.Format("{0:0.00}", a) : "----   ",
                Top = starty - RowHeight + index * RowHeight + pastswitchoutcount * 10,
                HorizontalAlign = HorizontalAlignment.Right,
                Width = VaNameLength
            });

            TextBlockInfoItems.Add(new TextBlockInfo()
            {
                BorderThinkness = 0,
                Color = color,
                CornerRadius = 0,
                Height = RowHeight,
                Index = 0,
                Left = startx + 20 + 2 * VaNameLength,

                Text = used ? ShowDw ? string.Format("{0:0.00}", power) + " [Kw]" : string.Format("{0:0.00}", power) : "----    ",
                Top = starty - RowHeight + index * RowHeight + pastswitchoutcount * 10,
                HorizontalAlign = HorizontalAlignment.Right,
                Width = VaNameLength + 10
            });


            TextBlockInfoItems.Add(new TextBlockInfo()
            {
                BorderThinkness = 0,
                Color = color,
                CornerRadius = 0,
                Height = RowHeight,
                Index = 0,
                Left = startx + 30 + 3 * VaNameLength,

                Text = used ? pws : "----  ",
                Top = starty - RowHeight + index * RowHeight + pastswitchoutcount * 10,
                HorizontalAlign = HorizontalAlignment.Right,
                Width = VaNameLength-20>0?VaNameLength-20:VaNameLength
            });

            TextBlockInfoItems.Add(new TextBlockInfo()
            {
                BorderThinkness = 0,
                Color = color,
                CornerRadius = 0,
                Height = RowHeight,
                Index = 0,
                Left = startx + 10 + 4 * VaNameLength,

                Text = used ? string.Format("{0:0.00}", rate*100) + " %" : "----  ",
                Top = starty - RowHeight + index * RowHeight + pastswitchoutcount * 10,
                HorizontalAlign = HorizontalAlignment.Right,
                Width = VaNameLength
            });

       


            startx += 35+5*VaNameLength ;

            ArcItems.Add(new ArcInfo()
            {
                StartPoint = startx  + "," + (starty+ index * RowHeight  + pastswitchoutcount * 10),
                Point = startx  + "," + ( starty+ index * RowHeight -15 + pastswitchoutcount * 10),
                Color = color

            });


            EllItems.Add(new EllInfo()
            {
                Color = a < 0.1 ? "Gray" : "Gold",
                Left = (startx  - 12),
                Top = starty - 23 + index * RowHeight + pastswitchoutcount * 10,
                Wide = 16,
            });

            if (isHistory == false)
            {
                if (!string.IsNullOrEmpty(attachInfo))
                {

                    LineItems.Add(new LineInfo() // -- KiLj /--
                                      {
                                          Color = attachcolor,
                                          Index = 0,

                                          X1 = startx,
                                          X2 = startx + 90,
                                          Y1 = starty + index*RowHeight + pastswitchoutcount*10,
                                          Y2 = starty + index*RowHeight + pastswitchoutcount*10
                                      });


                    TextBlockInfoItems.Add(new TextBlockInfo()
                                               {
                                                   BorderThinkness = 0,
                                                   Color = color,
                                                   CornerRadius = 0,
                                                   Height = RowHeight,
                                                   Index = 0,
                                                   Left = startx + 10,

                                                   Text = attachInfo,
                                                   //"tt.Item2 ==1?"正常":tt.Item2 ==2?"被盗":"未知",// string.Format("{0:0.00}", rate) + " %",
                                                   Top = starty - RowHeight + index*RowHeight + pastswitchoutcount*10,
                                                   Width = 60,
                                                   HorizontalAlign = HorizontalAlignment.Center
                                               });


                }
            }




            //if (range < 1) return;
            //if (range > lower) return;


            //int x = 400 / range;
            //if (lower > 0)
            //{

            //    LineItems.Add(new LineInfo()
            //    {
            //        Color = "Blue",
            //        Index = 0,
            //        Tooltips = "下限" + lower,
            //        X1 = startx + 80 + 400 * lower / range - 2,
            //        X2 = startx + 80 + 400 * lower / range - 2,
            //        Y1 = starty - 3 + index * RowHeight + pastswitchoutcount * 10,
            //        Y2 = starty + 3 + index * RowHeight + pastswitchoutcount * 10,
            //        StrokeThickness = 4,
            //    });
            //}

            //LineItems.Add(new LineInfo()
            //{
            //    Color = "Red",
            //    Index = 0,
            //    Tooltips = "上限" + range,
            //    X1 = 726,
            //    X2 = 726,
            //    Y1 = starty - 3 + index * RowHeight + pastswitchoutcount * 10,
            //    Y2 = starty + 3 + index * RowHeight + pastswitchoutcount * 10,
            //    StrokeThickness = 4,
            //});

            //if (a > 1)
            //{
            //    LineItems.Add(new LineInfo()
            //    {
            //        Color = "Red",
            //        Index = 0,
            //        Tooltips = "当前电流60A",
            //        X1 = (int)(startx + 80 + 400 * a / range - 2),
            //        X2 = (int)(startx + 80 + 400 * a / range - 2),
            //        Y1 = starty - 3 + index * RowHeight + pastswitchoutcount * 10,
            //        Y2 = starty + 3 + index * RowHeight + pastswitchoutcount * 10,
            //        StrokeThickness = 4,
            //    });
            //}

        }

        /// <summary>
        /// 添加开关量回路数据  如门
        /// </summary>
        /// <param name="index">序号 第几个</param>
        /// <param name="loopId">回路地址</param>
    
        /// <param name="pastswitchoutcount">已经添加过的开关量输出的数</param>
        /// <param name="switchIsClose">开关量输入是否关闭</param>
        /// <param name="loopName">回路名称</param>
        /// <param name="color">颜色</param>
        private void AddSwitchInInfo(ref ObservableCollection<TextBlockInfo> TextBlockInfoItems, ref ObservableCollection<LineInfo> LineItems,int index, int loopId, int pastswitchoutcount, bool switchIsClose, string loopName, string color)
        {

            int startx = 200+TimeNameLength;
            int starty = 15;

            index += 1;

            LineItems.Add(new LineInfo() //--
            {
                Color = color,
                Index = 0,
                
                X1 = startx,
                X2 = startx + LoopNameLength ,
                Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                Y2 = starty + index * RowHeight + pastswitchoutcount * 10
            });


            LineItems.Add(new LineInfo() // -- KiLj /
            {
                Color = color,
                Index = 0,
                
                X1 = startx + LoopNameLength,
                X2 = startx + LoopNameLength+30,
                Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                Y2 =
                    switchIsClose
                        ? (starty + index * RowHeight + pastswitchoutcount * 10)
                        : (starty - 10 + index * RowHeight + pastswitchoutcount * 10)
            });

            TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj
            {
                BorderThinkness = 0,
                Color = color,
                CornerRadius = 0,
                Height = RowHeight ,
                Index = 0,
                Left = startx + 5,
                
                Text = " D" + loopId,
                Top = starty - RowHeight  + index * RowHeight + pastswitchoutcount * 10,
                Width = 45,
                HorizontalAlign =HorizontalAlignment.Left
            });


            LineItems.Add(new LineInfo() // -- KiLj /
            {
                Color = color,
                Index = 0,
                
                X1 = startx +LoopNameLength + 30,
                X2 = startx + LoopNameLength + 430,
                Y1 = starty + index * RowHeight + pastswitchoutcount * 10,
                Y2 = starty + index * RowHeight + pastswitchoutcount * 10
            });


            TextBlockInfoItems.Add(new TextBlockInfo() // -- KiLj /--
            {
                BorderThinkness = 0,
                Color = color,
                CornerRadius = 0,
                Height = RowHeight ,
                Index = 0,
                Left = startx + LoopNameLength + 50,
                
                Text = loopName,
                Top = starty - RowHeight  + index * RowHeight + pastswitchoutcount * 10,
                Width = 250
            });

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

    public class ArcInfo : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public ArcInfo()
        {
            StrokeThickness = 1;
        }
        private string _color;
        private string _startPoint;
        private string _point;
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

        public string StartPoint
        {
            get { return _startPoint; }
            set
            {
                if (_startPoint != value)
                {
                    _startPoint = value;
                    this.RaisePropertyChanged(() => this.StartPoint);
                }
            }
        }

        public string Point
        {
            get { return _point; }
            set
            {
                if (_point != value)
                {
                    _point = value;
                    this.RaisePropertyChanged(() => this.Point);
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

    public class EllInfo : Wlst.Cr.Core.CoreServices.ObservableObject
    {


        private int _left;
        private int _top;
        private string _color;
        private int _wide;
        public int Wide
        {
            get { return _wide; }
            set
            {
                if (_wide != value)
                {
                    _wide = value;
                    this.RaisePropertyChanged(() => this.Wide);
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


    }
}
