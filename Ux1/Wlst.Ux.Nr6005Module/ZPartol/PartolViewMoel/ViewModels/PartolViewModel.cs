using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.AssetManageInfoHold.Model;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.Ux.Nr6005Module.ZPartol.PartolViewMoel.Services;
using Wlst.client;
using Wlst.iif;
using WriteLog = Wlst.Cr.Core.UtilityFunction.WriteLog;

namespace Wlst.Ux.Nr6005Module.ZPartol.PartolViewMoel.ViewModels
{
    /// <summary>
    /// PartolView视图的后台数据
    /// </summary>
    [Export(typeof (IIPartolViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class PartolViewModel : Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged,
                                           IIPartolViewModel
    {

        protected List<int> LstPartolRtu = new List<int>();

        private int _hxxx = 0;
        /// <summary>
        /// 前台界面绑定的图标大小
        /// </summary>
        public int Hightxx
        {
            get
            {
                if (_hxxx < 0.1)
                {
                    _hxxx = (int)Elysium.ThemesSet.FontSet.FontAttriXaml.RowHeadHeightt - 4;
                    if (_hxxx > 25) _hxxx = 25;
                    if (_hxxx < 12) _hxxx = 12;
                }
                return _hxxx;
            }
        }

        public PartolViewModel()
        {
            Shield = Visibility.Hidden;
            Patrol = Visibility.Visible;      
            this.InitEvent();
            this.InitAction();
        }

        protected bool IsViewShowing = false;

        private bool _isShowSumPower = false;
        /// <summary>
        /// 显示功率
        /// </summary>
        public bool IsShowSumPower
        {
            get { return _isShowSumPower; }
            set
            {
                if (_isShowSumPower != value)
                {
                    _isShowSumPower = value;
                    this.RaisePropertyChanged(() => this.IsShowSumPower);

                }
            }
        }


        #region data

        /// <summary>
        /// 
        /// </summary>
        private string _onlineDataUpdateTime;

        public string OnLineDataUpdateTime
        {
            get { return _onlineDataUpdateTime; }
            set
            {
                if (_onlineDataUpdateTime == value) return;
                _onlineDataUpdateTime = value;
                this.RaisePropertyChanged(() => this.OnLineDataUpdateTime);
            }
        }



        private int _SumPartolTmlNumber;

        /// <summary>
        /// 进行召测的实际终端数量
        /// </summary>
        public int SumPartolTmlNumer
        {
            get { return _SumPartolTmlNumber; }
            private set
            {
                if (_SumPartolTmlNumber == value) return;
                _SumPartolTmlNumber = value;
                this.RaisePropertyChanged(() => this.SumPartolTmlNumer);
            }
        }

        private int _sumAnswerPartolTmlNumber;

        /// <summary>
        /// 召测应答终端数量
        /// </summary>
        public int SumAnswerPartolTmlNumber
        {
            get { return _sumAnswerPartolTmlNumber; }
            private set
            {
                if (_sumAnswerPartolTmlNumber == value) return;
                _sumAnswerPartolTmlNumber = value;
                this.RaisePropertyChanged(() => this.SumAnswerPartolTmlNumber);
            }
        }

        private ObservableCollection<PartolItemViewModel> _measurePatrolData;

        public ObservableCollection<PartolItemViewModel> MeasurePatrolData
        {
            get
            {
                if (_measurePatrolData == null)
                    _measurePatrolData = new ObservableCollection<PartolItemViewModel>();

                return _measurePatrolData;
            }
            set
            {
                if (value == _measurePatrolData) return;
                _measurePatrolData = value;
                this.RaisePropertyChanged(() => this.MeasurePatrolData);
            }
        }

        private PartolItemViewModel _currentSelectedItem;

        public PartolItemViewModel CurrentSelectedItem
        {
            get { return _currentSelectedItem; }
            set
            {
                if (value == _currentSelectedItem) return;
                _currentSelectedItem = value;
                this.RaisePropertyChanged(() => this.CurrentSelectedItem);

                if (value != null && value.RtuId > 0)
                {
                    var args = new PublishEventArgs
                                   {
                                       EventType = PublishEventType.Core,
                                       EventId =
                                           Sr.EquipmentInfoHolding.Services.EventIdAssign.
                                           EquipmentSelected,
                                   };
                    args.AddParams(value.RtuId);

                    EventPublish.PublishEvent(args);
                }

            }
        }

        private ShieldItemViewModel _currentShieldPatrolData;

        public ShieldItemViewModel CurrentShieldPatrolData
        {
            get { return _currentShieldPatrolData; }
            set
            {
                if (value == _currentShieldPatrolData) return;
                _currentShieldPatrolData = value;
                this.RaisePropertyChanged(() => this.CurrentShieldPatrolData);

                if (value != null && value.RtuId > 0)
                {
                    var args = new PublishEventArgs
                                   {
                                       EventType = PublishEventType.Core,
                                       EventId =
                                           Sr.EquipmentInfoHolding.Services.EventIdAssign.
                                           EquipmentSelected,
                                   };
                    args.AddParams(value.RtuId);

                    EventPublish.PublishEvent(args);
                }

            }
        }

        

        private ObservableCollection<ShieldItemViewModel> _shieldPatrolData;
        /// <summary>
        /// 屏蔽回路数据
        /// </summary>
        public ObservableCollection<ShieldItemViewModel> ShieldPatrolData
        {
            get
            {
                if (_shieldPatrolData == null)
                    _shieldPatrolData = new ObservableCollection<ShieldItemViewModel>();

                return _shieldPatrolData;
            }
            set
            {
                if (value == _shieldPatrolData) return;
                _shieldPatrolData = value;
                this.RaisePropertyChanged(() => this.ShieldPatrolData);
            }
        }

        private Visibility _all;
        /// <summary>
        /// 显示巡测数据
        /// </summary>
        public Visibility  Patrol
        {
            get { return _all; }
            set
            {
                if (_all == value) return;
                _all = value;
                this.RaisePropertyChanged(() => this.Patrol);
            }
        }

        private Visibility _shield;
        /// <summary>
        /// 显示屏蔽回路数据
        /// </summary>
        public Visibility Shield
        {
            get { return _shield; }
            set
            {
                if (_shield == value) return;
                _shield = value;
                this.RaisePropertyChanged(() => this.Shield);
            }
        }

        #region tabTitle

        public int Index
        {
            get { return 1; }
        }

        public string Title
        {
            get { return "终端巡测"; }
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

        #endregion


        public void NavOnLoad(params object[] parsObjects)
        {
            this.IsShowSumPower = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 8, false);

            IsViewShowing = true;
            LoadItems();
            _dtPartol = DateTime.Now.AddHours(-1);
            _dtCmdTongji = DateTime.Now.AddDays(-1);
            _dicPartolItems.Clear();
            _dicShieldItems.Clear();
            foreach (var f in MeasurePatrolData )
            {
                if (_dicPartolItems.ContainsKey(f.RtuId)) _dicPartolItems[f.RtuId] = f;
                else _dicPartolItems.Add(f.RtuId, f);
            }
            foreach(var f in ShieldPatrolData )
            {
                if (_dicShieldItems.ContainsKey(f.RtuId)) _dicShieldItems[f.RtuId] = f;
                else _dicShieldItems.Add(f.RtuId, f);
            }

            var arg = 1;
            if (parsObjects.Count()>0)
            {
               arg=(int) parsObjects[0];
            }
            if (arg == 0)
            {
                ExCmdOnLine();
            }


            LoadRtuRemarkXml();
            RequestHttpData();
        }

        public void OnUserHideOrClosing()
        {
            IsViewShowing = false;
            MeasurePatrolData.Clear();
            _dicPartolItems.Clear();
            _dicShieldItems.Clear();
        }

        private string Get_Special_GrpName(int rtuId)
        {
            var userProperty = UserInfo.UserLoginInfo;

            if (userProperty.AreaX.Count > 1)
            {
                int _areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(rtuId);

                if (_areaId != -1)
                {
                    var areaInfo = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(_areaId);

                    return "特殊终端" + "-" + areaInfo.AreaName;
                }
            }

            return "----";
        }

        private Dictionary<int, PartolItemViewModel> _dicPartolItems = new Dictionary<int, PartolItemViewModel>();
        private Dictionary<int, ShieldItemViewModel> _dicShieldItems = new Dictionary<int, ShieldItemViewModel>(); 



        private void LoadItems()
        {
            try
            {
                if (MeasurePatrolData.Count > 0)
                {
                    LoadItemsAsyn();
                    return;
                }
                //   MeasurePatrolData.Clear();
                ShieldPatrolData.Clear();
                var userProperty = UserInfo.UserLoginInfo;
                Dictionary<int, string> tmpdir = new Dictionary<int, string>();
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
                {
                    //if (t.Value.LstTml.Count == 0) continue;
                    foreach (var g in t.Value.LstTml)
                    {
                        if (tmpdir.ContainsKey(g)) continue;
                        if(userProperty.AreaX.Count>1)
                        {
                            var areaInfo =
                                Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetAreaInfo(t.Value.AreaId);
                            if (areaInfo != null) tmpdir.Add(g, t.Value.GroupName + "-" +areaInfo.AreaName+ "-" + t.Key.Item2 );
                        }else
                        {
                            tmpdir.Add(g,t.Value.GroupName + "-" + t.Key.Item2 );
                        }
                        
                    }
                }

                // var mtps = new ObservableCollection<PartolItemViewModel>();
                var index = 0;
                foreach (
                    var t in
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                    )
                {
                    if (t.Value.RtuStateCode == 0 || t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;
                    
                    #region 巡测数据加载
                    var ttt = new PartolItemViewModel() {RtuId = t.Key, PhysicalId = t.Value.RtuPhyId};
                    ttt.RtuName = t.Value.RtuName;
                    ttt.RequestNewDataTime = "--";
                    ttt.ReceiveNewDataTime = "--";
                    ttt.TimeSpan = "--";
                    ttt.RtuState = t.Value.RtuStateCode == 2 ? "使用" : t.Value.RtuStateCode == 1 ? "停运" : "不用";
                    ttt.RtuStateInt = t.Value.RtuStateCode;

                    if (tmpdir.ContainsKey(t.Key)) ttt.GrpName = tmpdir[t.Key];
                    //                    else ttt.GrpName = "----";
                    else ttt.GrpName = Get_Special_GrpName(t.Key);

                    var tmp = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.Key);
                    if (tmp != null && tmp.RtuNewData != null)
                    {
                        ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                        ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                        ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                        ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                        ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                        ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                        ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                        ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;
                        ttt.ErrorCount  = tmp.ErrorCount;
                        ttt.OnRtudataArvUpState(tmp.RtuNewData.Alarms.ContainsKey(3) ? 1 : 2);


                    }
                    //Application.Current.Dispatcher.Invoke(
                    //    System.Windows.Threading.DispatcherPriority.Normal, new DelRunRun(DelRun), ttt, 0);
                    // Wlst.Cr.Core.CoreServices.AsynObservableCollectionAdd.Insert(MeasurePatrolData,ttt);

                    index++;
                    MeasurePatrolData.Add(ttt);
                    if (index % 100 == 0)
                    {
                        Wlst.Cr.CoreOne.OtherHelper.Delay.DelayEvent();
                    }
                    #endregion

                    #region 屏蔽回路数据加载

                  
                    var tmp2 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.Value.RtuId );
                    
                    var shield = tmp2 as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (shield != null)
                    {
                        foreach (var x in shield.WjLoops.Values)
                        {
                            if (x.IsShieldLoop == 1)
                            {
                                var shieldLst = new ShieldItemViewModel()
                                                    {RtuId = t.Value.RtuId, RtuName = t.Value.RtuName};
                                shieldLst.LoopId = x.LoopId;
                                shieldLst.LoopName = x.LoopName;
                                if (tmp != null && tmp.RtuNewData != null)
                                {
                                    foreach (var xxx in tmp.RtuNewData.LstNewLoopsData)
                                    {
                                        if (xxx.LoopId == x.LoopId)
                                        {
                                            shieldLst.V = xxx.V.ToString("f2");
                                            shieldLst.A = xxx.A.ToString("f2");
                                            shieldLst.Power = xxx.Power.ToString("f2");
                                        }
                                    }
                                }
                                ShieldPatrolData.Add(shieldLst);
                            }
                        }
                    }


                    #endregion
                }
                //   MeasurePatrolData = mtps;

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error :" + ex);
            }
        }

        private void LoadItemsAsyn()
        {
            try
            {

                var direx = new Dictionary<int, PartolItemViewModel>();
                foreach (var t in MeasurePatrolData) if (!direx.ContainsKey(t.RtuId)) direx.Add(t.RtuId, t);


                Dictionary<int, string> tmpdir = new Dictionary<int, string>();
                foreach (var t in Wlst.Sr.EquipmentInfoHolding .Services.ServicesGrpSingleInfoHold.InfoGroups
                    )
                {
                    //if (t.Value.LstTml.Count == 0) continue;
                    foreach (var g in t.Value.LstTml)
                    {
                        if (tmpdir.ContainsKey(g)) continue;
                        tmpdir.Add(g, t.Value .AreaId .ToString( "d2")+"-"+ t.Key + "-" + t.Value.GroupName);
                    }
                }




                // var mtps = new ObservableCollection<PartolItemViewModel>();
                foreach (
                    var t in
                        Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems 
                    )
                {
                    if (t.Value.RtuStateCode  == 0) continue;
                    //int counterror =
                    //    Wlst.Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.GetFaultLstInfoByRtuId(t.Key).
                    //        Count;


                    if (!direx.ContainsKey(t.Key))
                    {
                        if (t.Value.RtuStateCode  == 0 || t.Value .EquipmentType !=WjParaBase.EquType.Rtu ) continue;

                        var ttt = new PartolItemViewModel() {RtuId = t.Key, PhysicalId = t.Value.RtuPhyId };
                        ttt.RtuName = t.Value.RtuName;
                        ttt.RequestNewDataTime = "--";
                        ttt.ReceiveNewDataTime = "--";
                        ttt.TimeSpan = "--";
                        ttt.DateCreateTime = DateTime.Now.Ticks;
                        //ttt.ErrorCount = counterror;
                        ttt.RtuState = t.Value.RtuStateCode  == 2 ? "使用" : t.Value.RtuStateCode  == 1 ? "停运" : "不用";
                        if (tmpdir.ContainsKey(t.Key)) ttt.GrpName = tmpdir[t.Key];
//                        else ttt.GrpName = "----";
                        else ttt.GrpName = Get_Special_GrpName(t.Key);

                        var tmp = Wlst .Sr .EquipmentInfoHolding .Services .RunningInfoHold .GetRunInfo( t.Key);
                        if (tmp != null && tmp .RtuNewData !=null )
                        {
                            ttt.ErrorCount = tmp.ErrorCount;
                            
                            ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                            ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                            ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                            ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                            ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                            ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                            ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                            ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;
                         
                        }
                        //Application.Current.Dispatcher.Invoke(
                        //    System.Windows.Threading.DispatcherPriority.Normal, new DelRunRun(DelRun), ttt, 0);
                        // Wlst.Cr.Core.CoreServices.AsynObservableCollectionAdd.Insert(MeasurePatrolData,ttt);

                        MeasurePatrolData.Add(ttt);
                    }
                    else
                    {
                        var ttt = direx[t.Key];

                        if (t.Value.RtuStateCode  == 0 || t.Value .EquipmentType !=WjParaBase.EquType.Rtu ) continue;

                        // var ttt = new PartolItemViewModel() { RtuId = t.Key, PhysicalId = t.Value.PhyId };
                        ttt.RtuName = t.Value.RtuName;
                        ttt.RequestNewDataTime = "--";
                        ttt.ReceiveNewDataTime = "--";
                        ttt.TimeSpan = "--";
                        ttt.DateCreateTime = DateTime.Now.Ticks;
                        ttt.RtuState = t.Value.RtuStateCode  == 2 ? "使用" : t.Value.RtuStateCode  == 1 ? "停运" : "不用";
                        if (tmpdir.ContainsKey(t.Key)) ttt.GrpName = tmpdir[t.Key];
//                        else ttt.GrpName = "----";
                        else ttt.GrpName = Get_Special_GrpName(t.Key);
                        //ttt.ErrorCount = counterror;
                        var tmp = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.Key);
                        if (tmp != null && tmp.RtuNewData != null)
                        {
                            ttt.ErrorCount = tmp.ErrorCount;                            
                            ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                            ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                            ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                            ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                            ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                            ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                            ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                            ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;
                        }
                    }

                }
                //   MeasurePatrolData = mtps;

            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error :" + ex);
            }
        }



        #region CmdPartol

        private DateTime _dtPartol;
        private ICommand _cmdPartol;

        public ICommand CmdPartol
        {
            get
            {
                if (_cmdPartol == null) _cmdPartol = new RelayCommand(ExPartol, CanExPartol, true);
                return _cmdPartol;
            }
        }



        private void ExPartol()
        {
            _dtPartol = DateTime.Now;

            LstPartolRtu.Clear();
            foreach (
                var t in
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems 
                )
            {
                if (t.Value.RtuStateCode  == 0 || t.Value .EquipmentType !=WjParaBase.EquType.Rtu ) continue;
 
                LstPartolRtu.Add(t.Key);
            }
            foreach (var t in MeasurePatrolData)
            {
                t.RequestNewDataTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "";
                t.DateCreateTime = DateTime.Now.Ticks;

                t.ReceiveNewDataTime = "--";
                t.TimeSpan = "--";
            }

            SumPartolTmlNumer = LstPartolRtu.Count;
            SumAnswerPartolTmlNumber = 0;

            // LogInfo.Log(I36N.Services.I36N.ConvertByCoding("11030003"));
            SndPartol();

        }



        private bool CanExPartol()
        {
            if (DateTime.Now.Ticks - _dtPartol.Ticks < 60000000) return false;
            return true;
        }

        #endregion

        #region CmdRePartol

        private DateTime _dtRePartol;
        private ICommand _cmdRePartol;

        public ICommand CmdRePartol
        {
            get
            {
                if (_cmdRePartol == null) _cmdRePartol = new RelayCommand(ExRePartol, CanRePartol, true);
                return _cmdRePartol;
            }
        }



        private void ExRePartol()
        {

            foreach (var t in MeasurePatrolData)
            {
                if (LstPartolRtu.Contains(t.RtuId))
                {
                    // t.RequestNewDataTime = DateTime.Now + "";
                    t.ReceiveNewDataTime = "--";
                    t.TimeSpan = "--";
                }

            }
            _dtPartol = DateTime.Now;
            _dtRePartol = DateTime.Now;
            SndPartol();
        }

        private bool CanRePartol()
        {
            if (DateTime.Now.Ticks - _dtPartol.Ticks < 300000000) return false;
            if (DateTime.Now.Ticks - _dtRePartol.Ticks < 150000000) return false;
            return LstPartolRtu.Count > 0;
        }

        #endregion

        #region CmdOnLine

        private ICommand _CmdOnLine;

        public ICommand CmdOnLine
        {
            get
            {
                if (_CmdOnLine == null) _CmdOnLine = new RelayCommand(ExCmdOnLine, CanCmdOnLine, true);
                return _CmdOnLine;
            }
        }


        private void ExCmdOnLine()
        {
           // foreach (var t in MeasurePatrolData) t.SetOnLine(false);
               SndOnLineRequest();
            _lastExCanCmdOnLine = DateTime.Now.Ticks;




            //foreach (var t in MeasurePatrolData)
            //{
            //    if (Sr.EquipmentInfoHolding.Services.RtuOnLineServices.IsRtuOnLine(t.RtuId)) t.SetOnLine(true);
            //    else t.SetOnLine(false);
            //}
        }

        private long _lastExCanCmdOnLine;

        private bool CanCmdOnLine()
        {
            var ts1 = DateTime.Now.Ticks;
            var ts2 = ts1 - _lastExCanCmdOnLine;
            if (ts2 >= 30*PartolItemViewModel.OneSecond)
                return true;
            return false;
        }

        #endregion

        #region CmdTongji

        private DateTime _dtCmdTongji;
        private ICommand _CmdTongji;

        public ICommand CmdTongji
        {
            get
            {
                if (_CmdTongji == null) _CmdTongji = new RelayCommand(ExCmdTongjil, CanTongji, true);
                return _CmdTongji;
            }
        }



        private void ExCmdTongjil()
        {
            _dtCmdTongji = DateTime.Now;
            _dtPartol = DateTime.Now;
            LstPartolRtu.Clear();
            foreach (
                var t in
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .InfoItems 
                )
            {
                if (t.Value.RtuStateCode  == 0 || t.Value .EquipmentType !=WjParaBase.EquType.Rtu ) continue;
 
                LstPartolRtu.Add(t.Key);
            }
            foreach (var t in MeasurePatrolData)
            {
                t.RequestNewDataTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "";
                t.ReceiveNewDataTime = "--";
                t.TimeSpan = "--";
            }

            SumPartolTmlNumer = LstPartolRtu.Count;
            SumAnswerPartolTmlNumber = 0;

        }

        private bool CanTongji()
        {
            if (DateTime.Now.Ticks - _dtCmdTongji.Ticks < 60000000) return false;
            if (DateTime.Now.Ticks - _dtPartol.Ticks < 900000000) return false;
            return true;
        }

        #endregion


        #region CmdReport

        private DateTime _dtReport;
        private ICommand _CmdReport;

        public ICommand CmdReport
        {
            get
            {
                if (_CmdReport == null) _CmdReport = new RelayCommand(ExCmdReport, CanCmdReport, false);
                return _CmdReport;
            }
        }


        private void ExCmdReport()
        {
            _lastExCmdReport = DateTime.Now.Ticks;

            
                WriteData();
        }

        private long _lastExCmdReport = DateTime.Now.AddDays(-1).Ticks;

        private bool CanCmdReport()
        {
            //if (DateTime.Now.Ticks - _lastExCmdReport >= 120*PartolItemViewModel.OneSecond)
               // return true;
            //return false;
            if (DateTime.Now.Ticks - _dtReport.Ticks < 60000000) return false;
            return true;
        }

        #endregion


        #region CmdReportx

        private DateTime _dtReportx;
        private ICommand _CmdReportx;

        public ICommand CmdReportx
        {
            get
            {
                if (_CmdReportx == null) _CmdReportx = new RelayCommand(ExCmdReportX, CanCmdReportX, false);
                return _CmdReportx;
            }
        }


        private void ExCmdReportX()
        {
          
                WriteDatax();
            
        }

        //private long _lastExCmdReport = DateTime.Now.AddDays(-1).Ticks;

        private bool CanCmdReportX()
        {
            if (DateTime.Now.Ticks - _dtReportx.Ticks < 60000000) return false;
            return true;

        }

        #endregion

        #region CmdReturn

        private ICommand _cmdReturn;

        public ICommand CmdReturn
        {
            get { return _cmdReturn ?? (_cmdReturn = new RelayCommand(ExReturn, CanExReturn, true)); }
        }



        private void ExReturn()
        {
            Patrol = Visibility.Visible;
            Shield = Visibility.Collapsed;

        }



        private bool CanExReturn()
        {            
            return true;
        }
        #endregion

        #region CmdShieldData

        private ICommand _cmdShieldData;

        public ICommand CmdShieldData
        {

            get { return _cmdShieldData ?? (_cmdShieldData = new RelayCommand(ExShield, CanExShield, true)); }
            
        }



        private void ExShield()
        {
            Patrol = Visibility.Collapsed;
            Shield = Visibility.Visible ;

        }



        private bool CanExShield()
        {
            return true;
        }
        #endregion

    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class PartolViewModel
    {

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone .LxSys  .wlst_sys_rtu_online  ,//.ClientPart.wlst_Measures_server_ans_clinet_request_RtuOnLine,
                RtuOnLineRequest,
                typeof(PartolViewModel), this,true);

            ProtocolServer.RegistProtocol(
           Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_orders, // .wlst_svr_ans_cnt_request_wj3090_measure ,
           OrderRtuMeasureOrder,
           typeof(PartolViewModel), this);
        }


        public void RtuOnLineRequest(string session, mobile.MsgWithMobile infos)
        {

            var lstrtggu = infos.Args.Addr;
            if (lstrtggu == null) return;
            int online = 0;
            int offline = 0;

            foreach (var t in MeasurePatrolData)
            {
                if (lstrtggu.Contains(t.RtuId))
                {
                    t.SetOnLine(true);
                    online++;

                }
                else
                {
                    t.SetOnLine(false);
                    offline++;
                }
            }

            OnLineDataUpdateTime = DateTime.Now + "  " + "终端在线：" + online + " 离线：" + offline;

        }

        public void UpdateInfo(RtuNewDataInfo tmp)
        {
            if (!IsViewShowing) return;

            if (tmp == null) return;
            if (!_dicPartolItems.ContainsKey(tmp.RtuId)) return;

            try //todo
            {



                var ttt = _dicPartolItems[tmp.RtuId];
                //var newData = RtuNewDataService.GetInfoById(ttt.RtuId);

                //   var tmp = RtuNewDataService.GetInfoById(ttt.RtuId);

                var tmp2 = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(tmp.RtuId );
                if (tmp2 != null)
                {
                    ttt.ErrorCount = tmp2.ErrorCount;
                }
                ttt.ReceiveNewDataTime = tmp.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                
                ttt.SetSwitchOutState(tmp.IsSwitchOutAttraction);
                ttt.RtuCurrentSumA = tmp.RtuCurrentSumA;
                ttt.RtuCurrentSumB = tmp.RtuCurrentSumB;
                ttt.RtuCurrentSumC = tmp.RtuCurrentSumC;
                ttt.DateCreateTime = tmp.DateCreate.Ticks;
                ttt.RtuVoltageA = tmp.RtuVoltageA;
                ttt.RtuVoltageB = tmp.RtuVoltageB;
                ttt.RtuVoltageC = tmp.RtuVoltageC;
                if (LstPartolRtu.Contains(ttt.RtuId))
                {
                    ttt.TimeSpan = string.Format("{0:0.000}",
                                                 (DateTime.Now.Ticks - _dtPartol.Ticks)*1.0/10000000);
                    LstPartolRtu.Remove(ttt.RtuId);
                    SumAnswerPartolTmlNumber++;
                }
                else
                {
                    ttt.TimeSpan = "新数据";
                }




            }
            catch (Exception ex)
            {
                WriteLog.WriteLogError(
                    "Error:" +
                    ex.ToString());
            }



        }

     private void InitEvent()
     {
         //this.AddEventFilterInfo(Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2 ,
         //                        PublishEventType.Core);


     }

        private object obj = 1;
     public void OrderRtuMeasureOrder(string session, Wlst.mobile.MsgWithMobile infos)
     {
         if (!IsViewShowing) return;

         var rtuData = infos.WstRtuOrders;
         if (rtuData == null) return;
         if (rtuData.Op < 31 || rtuData.Op > 32) return;

         //如果型号发生变化 需要进一步修改 
         if (rtuData.Items == null) return;

         foreach (var fff in rtuData.Items)
         {
             try
             {
                 // var fff = t.Clone() as TmlNewData;
                 if (fff == null) continue;
                 if (fff.LstNewLoopsData == null) continue;

                 var tmp = new RtuNewDataInfo(fff);
                 var tmp2 = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(fff.RtuId );
                 if (tmp == null) continue;

                 if (!_dicPartolItems.ContainsKey(tmp.RtuId)) continue;
                 var ttt = _dicPartolItems[tmp.RtuId];

                 //if (tmp.DateCreate.Ticks < ttt.DateCreateTime) continue;
                 ttt.DateCreateTime = tmp.DateCreate.Ticks;
                 if (tmp2 != null)
                 {
                     ttt.ErrorCount = tmp2.ErrorCount;
                 }
                 ttt.ReceiveNewDataTime = tmp.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
           
                 ttt.SetSwitchOutState(tmp.IsSwitchOutAttraction);
                 ttt.RtuCurrentSumA = tmp.RtuCurrentSumA;
                 ttt.RtuCurrentSumB = tmp.RtuCurrentSumB;
                 ttt.RtuCurrentSumC = tmp.RtuCurrentSumC;
                 ttt.RtuVoltageA = tmp.RtuVoltageA;
                 ttt.RtuVoltageB = tmp.RtuVoltageB;
                 ttt.RtuVoltageC = tmp.RtuVoltageC;

                 double PA = 0.00, PB = 0.00, PC = 0.00;
                 if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(tmp.RtuId))
                 {
                     var info = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[tmp.RtuId] as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;


                     foreach (var t in tmp.LstNewLoopsData)
                     {
                         if (t.IsLoop && info.WjLoops.ContainsKey(t.LoopId))
                         {
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
                 ttt.RtuPowerSumA = PA.ToString("f2");
                 ttt.RtuPowerSumB = PB.ToString("f2");
                 ttt.RtuPowerSumC = PC.ToString("f2");
                 ttt.RtuPowerSum = (PA + PB + PC).ToString("f2");



                 lock (obj)
                 {
                     if (LstPartolRtu.Contains(ttt.RtuId))
                     {
                         ttt.TimeSpan = string.Format("{0:0.000}",
                                                      (DateTime.Now.Ticks - _dtPartol.Ticks)*1.0/10000000);
                         LstPartolRtu.Remove(ttt.RtuId);
                         SumAnswerPartolTmlNumber++;
                     }
                     else
                     {
                         ttt.TimeSpan = "新数据";
                     }
                 }

                 #region 屏蔽回路数据加载

                 if (!_dicShieldItems.ContainsKey(tmp .RtuId )) continue;
                 var shield = _dicShieldItems[tmp.RtuId ];

                 foreach (var f in tmp.LstNewLoopsData)
                 {
                     if (f.LoopId == shield.LoopId)
                     {
                         shield.V = f.V.ToString("f2");
                         shield.A = f.A.ToString("f2");
                         shield.Power = f.Power.ToString("f2");
                     }
                 }


                 #endregion

             }
             catch (Exception ex)
             {
           
             }
         }



     }
       
     public override void ExPublishedEvent(PublishEventArgs args)
     {
         if (!IsViewShowing) return;

         //base.ExPublishedEvent(args);
         if (args.EventId == Wlst.Sr.EquipmentInfoHolding.Services.EventIdAssign.RunningInfoUpdate2 )
         {
             try //todo
             {
                 var lstrtggu = args.GetParams()[0] as List<int>;
                 if (lstrtggu == null) return;
                 if (lstrtggu.Count == 0) return;

                 foreach (var gf in lstrtggu)
                 {
                     if (!_dicPartolItems.ContainsKey(gf)) continue;
                     var ttt = _dicPartolItems[gf];

                     var tmp =RunningInfoHold .GetRunInfo( ttt.RtuId);
                     if (tmp == null || tmp.RtuNewData == null) continue;

                     if (tmp.RtuNewData.DateCreate.Ticks < ttt.DateCreateTime) continue;
                     ttt.DateCreateTime = tmp.RtuNewData.DateCreate.Ticks ;

                     ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                     ttt.ErrorCount = tmp.ErrorCount ;
                     ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                     ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                     ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                     ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                     ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                     ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                     ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;

                     ttt.OnRtudataArvUpState(tmp.RtuNewData.Alarms.ContainsKey(3) ? 1 : 2);
                     if (LstPartolRtu.Contains(ttt.RtuId))
                     {
                         ttt.TimeSpan = string.Format("{0:0.000}",
                                                      (DateTime.Now.Ticks - _dtPartol.Ticks)*1.0/10000000);
                         LstPartolRtu.Remove(ttt.RtuId);
                         SumAnswerPartolTmlNumber++;
                     }
                     else
                     {
                         ttt.TimeSpan = "新数据";
                     }

                     #region 屏蔽回路数据加载
    
                     if (!_dicShieldItems.ContainsKey(gf)) continue;
                     var shield = _dicShieldItems[gf];
                    
                     foreach(var f in tmp.RtuNewData.LstNewLoopsData )
                     {
                         if(f.LoopId ==shield.LoopId )
                         {
                             shield.V = f.V.ToString("f2");
                             shield.A = f.A.ToString("f2");
                             shield.Power = f.Power.ToString("f2");
                         }
                     }
                     

                     #endregion
                 }


             }
             catch (Exception ex)
             {
                 WriteLog.WriteLogError(
                     "Error:" +
                     ex.ToString());
             }
         }


     }
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class PartolViewModel
    {
        private void SndPartol()
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu .wst_rtu_orders ;//.wlst_cnt_request_wj3090_measure;
            //info.Args .Addr .AddRange(LstPartolRtu);
            info.WstRtuOrders.Op = 31;
            info.WstRtuOrders.RtuIds.AddRange(LstPartolRtu);
            SndOrderServer.OrderSnd(info);
        }

        private void SndOnLineRequest()
        {
            var info = Wlst.Sr.ProtocolPhone.LxSys .wlst_sys_rtu_online ;// wlst_cnt_wj2090_update_time.ProtocolCnt.ServerPart.wlst_Measures_clinet_request_RtuOnLine;
            SndOrderServer.OrderSnd(info);
        }
    }

    /// <summary>
    /// 导出报表
    /// </summary>
    public partial class PartolViewModel
    {

        private void WriteData()
        {
            try
            {
                if (File.Exists( "Config\\长沙.txt"))  //长沙报表模式
                {
                    StreamReader rd = File.OpenText("Config\\长沙.txt");
                    var pathrd = rd.ReadToEnd();
                    if (pathrd == "") pathrd = "Report\\";
                    if (!Directory.Exists(pathrd.Substring(0,pathrd.Length -2 )))
                    {
                        Directory.CreateDirectory(pathrd.Substring(0, pathrd.Length - 2));
                    }

                    if (!Directory.Exists(pathrd + DateTime.Now.ToString("yyyy年MM月dd日")))
                    {
                        Directory.CreateDirectory(pathrd + DateTime.Now.ToString("yyyy年MM月dd日"));
                    }

                    var data = WriteDataInsNewChangsha();
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(pathrd + DateTime.Now.ToString("yyyy年MM月dd日") + "\\"  + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + " 终端信息表.xls", data.Item1, data.Item2);

                }
                else
                {
                    WriteDataInsNew();
                }
                //WriteDataIns();
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出终端参数报表时报错:" + ex);
            }
        }

        private void WriteDatax()
        {
            try
            {
                //WriteDataRunning();
                if (File.Exists("Config\\长沙.txt")) //长沙报表模式
                {
                    StreamReader rd = File.OpenText("Config\\长沙.txt");
                    var pathrd = rd.ReadToEnd();
                    if (pathrd == "") pathrd = "Report\\";
                    if (!Directory.Exists(pathrd.Substring(0, pathrd.Length - 2)))
                    {
                        Directory.CreateDirectory(pathrd.Substring(0, pathrd.Length - 2));
                    }

                    if (!Directory.Exists(pathrd + DateTime.Now.ToString("yyyy年MM月dd日")))
                    {
                        Directory.CreateDirectory(pathrd + DateTime.Now.ToString("yyyy年MM月dd日"));
                    }

                    var data = WriteDataRunningNewChangsha(true);
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(pathrd + DateTime.Now.ToString("yyyy年MM月dd日") + "\\" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + " 终端状态表.xls", data.Item1, data.Item2);
                    var data1 = WriteDataRunningNewChangsha(false);
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(pathrd + DateTime.Now.ToString("yyyy年MM月dd日") + "\\" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + " 最新数据不是今天的箱变.xls", data1.Item1, data1.Item2);

                    var data2 = WriteDataPowerStaticChangsha(true);
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(pathrd + DateTime.Now.ToString("yyyy年MM月dd日") + "\\" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + " 终端总电流总功率统计.xls", data2.Item1, data2.Item2);
                    var data3 = WriteDataPowerStaticChangsha(false);
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(pathrd + DateTime.Now.ToString("yyyy年MM月dd日") + "\\" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + " 终端总电流总功率统计-仅箱变.xls", data3.Item1, data3.Item2);

                    var data4 = WriteDataVAChangsha(true);
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(pathrd + DateTime.Now.ToString("yyyy年MM月dd日") + "\\" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + " 终端电压电流统计.xls", data4.Item1, data4.Item2);
                    var data5 = WriteDataVAChangsha(false);
                    Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(pathrd + DateTime.Now.ToString("yyyy年MM月dd日") + "\\" + DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + " 终端电压电流统计-仅箱变.xls", data5.Item1, data5.Item2);
                
                
                
                    
                }
                else
                {
                    WriteDataRunningNew();
                }
            }
            catch (Exception ex)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出终端数据报表时报错:" + ex);
            }
        }

        private void WriteDataInsNew()
        {
            //储存附属设备
            var fssbinfo = new Dictionary<int, List<int>>();
            // var lst = new List<>();

            var lstt = new ObservableCollection<PartolItemViewModel>();
            Dictionary<int, string> tmpdir = new Dictionary<int, string>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
            {
                //if (t.Value.LstTml.Count == 0) continue;
                foreach (var g in t.Value.LstTml)
                {
                    if (tmpdir.ContainsKey(g)) continue;
                    tmpdir.Add(g, t.Value.AreaId.ToString("d2") + "-" + t.Key + "-" + t.Value.GroupName);
                }
            }

            foreach (
                  var t in
                      Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                  )
            {
                if (t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;

                #region 巡测数据加载
                var ttt = new PartolItemViewModel() { RtuId = t.Key, PhysicalId = t.Value.RtuPhyId };
                ttt.RtuName = t.Value.RtuName;
                ttt.RequestNewDataTime = "--";
                ttt.ReceiveNewDataTime = "--";
                ttt.TimeSpan = "--";
                ttt.RtuState = t.Value.RtuStateCode == 2 ? "使用" : t.Value.RtuStateCode == 1 ? "停运" : "不用";
                if (tmpdir.ContainsKey(t.Key)) ttt.GrpName = tmpdir[t.Key];
//                else ttt.GrpName = "----";
                else ttt.GrpName = Get_Special_GrpName(t.Key);

                var tmp = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.Key);
                if (tmp != null && tmp.RtuNewData != null)
                {
                    ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                    ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                    ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                    ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                    ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                    ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                    ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                    ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;
                    ttt.ErrorCount = tmp.ErrorCount;
                }

                lstt.Add(ttt);
                #endregion

            }

            foreach (var t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.RtuFid != 0)
                {
                    if (fssbinfo.ContainsKey(t.Value.RtuFid))
                    {
                        fssbinfo[t.Value.RtuFid].Add(t.Key);
                    }
                    else
                    {
                        fssbinfo.Add(t.Value.RtuFid, new List<int>() { t.Key });
                    }
                }
                //else
                //{
                //    if (t.Value.EquipmentType == WjParaBase.EquType.Rtu) 
                //    {
                //        lst.Add(t.Value);
                //    }
                //}
            }


            //建立表头
            var writeinfo = new List<List<object>>();
            var titleinfo = new List<object>();
            titleinfo.Add("序号");

            titleinfo.Add("物理地址");
            titleinfo.Add("逻辑地址");
            titleinfo.Add("终端名称");
            titleinfo.Add("归属分组");

            //最新数据
            titleinfo.Add("是否在线");
            titleinfo.Add("最新数据的使用状态");
            titleinfo.Add("最新数据时间");

            titleinfo.Add("使用状态");
            titleinfo.Add("Ip地址");
            titleinfo.Add("电话号码");
            titleinfo.Add("心跳周期");
            titleinfo.Add("主报周期");

            //二期新增参数
            titleinfo.Add("开通日期");
            titleinfo.Add("最后更新日期");
            titleinfo.Add("安装位置");
            titleinfo.Add("备注信息");
            titleinfo.Add("唯一识别码");
            titleinfo.Add("是否通过电流判断辅助触点");

            //回路信息

            titleinfo.Add("输出矢量序列");
            titleinfo.Add("K1路数");
            titleinfo.Add("K2路数");
            titleinfo.Add("K3路数");
            titleinfo.Add("K4路数");
            titleinfo.Add("K5路数");
            titleinfo.Add("K6路数");
            titleinfo.Add("K7路数");
            titleinfo.Add("K8路数");
            titleinfo.Add("回路总数");
            titleinfo.Add("开关量路数");
            titleinfo.Add("开关量矢量序列");
            titleinfo.Add("电流矢量序列");
            titleinfo.Add("跳变报警序列");
            titleinfo.Add("相位序列");
            titleinfo.Add("互感器比序列");

            //防盗信息
            titleinfo.Add("防盗路数");
            titleinfo.Add("防盗名称");

            //电表信息
            titleinfo.Add("电表变比");


            //终端型号
            titleinfo.Add("设备型号");

            //设备动态备注 配置文件中配置表头  lvf 2019年1月16日10:29:33
            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks.Count != 0)
            {
                foreach (var g in Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks)
                {
                    titleinfo.Add(g.Value);

                }
            }


            //        var lst = (from t in MeasurePatrolData orderby t.PhysicalId select t).ToList();
            int index = 1;
            var lst = (from t in lstt orderby t.PhysicalId select t).ToList();
            foreach (var t in lst)
            {
                var tmp = new List<object>();

                tmp.Add(index);
                index++;

                tmp.Add(t.PhysicalId);
                tmp.Add(t.RtuId);
                tmp.Add(t.RtuName);
                tmp.Add(t.GrpName);
                

                //最新数据
                var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.RtuId);
                if (run != null && run.RtuNewData != null)
                {
                    tmp.Add(run.IsOnLine?"在线":"离线");

                    if (t.RtuState == "使用")
                    {
                        var title = "";
                        if (run.RtuNewData.Alarms.ContainsKey(1) && run.RtuNewData.Alarms[1]) title += "停电";
                        else title += "供电";

                        if (run.RtuNewData.Alarms.ContainsKey(3) && run.RtuNewData.Alarms[3]) title += "停运中";
                        else title += "使用中 ";

                        tmp.Add(title);
                    }
                    else
                    {
                        tmp.Add("--");
                    }
                    tmp.Add(run.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"));

                   
                }
                else
                {
                    tmp.Add("离线");
                    tmp.Add("");
                    tmp.Add("");
                }

                tmp.Add(t.RtuState);


                var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);
                if (tmp1 == null || tmp1.EquipmentType != WjParaBase.EquType.Rtu) continue;
                var tmp2 = tmp1 as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmp2 == null) continue;
                if (tmp2.WjGprs == null || tmp2.WjLoops == null || tmp2.WjSwitchOuts == null || tmp2.WjVoltage == null) continue;

                tmp.Add(new System.Net.IPAddress(BitConverter.GetBytes(tmp2.WjGprs.StaticIp)).ToString());
                tmp.Add(tmp2.WjGprs.MobileNo);
                tmp.Add(tmp2.WjGprs.RtuHeartbeatCycle);
                tmp.Add(tmp2.WjGprs.RtuReportCycle);

                //二期新增参数
                tmp.Add(string.Format("{0:G}", new DateTime(tmp2.DateCreate)));
                tmp.Add(string.Format("{0:G}", new DateTime(tmp2.DateUpdate)));
                tmp.Add(tmp2.RtuInstallAddr);
                tmp.Add(tmp2.RtuRemark);
                tmp.Add(tmp2.Idf);
                if (tmp2.WjVoltage != null)
                {
                    string isSwitchinputJudgeByA = (tmp2.WjVoltage.IsSwitchinputJudgebyA == true) ? "是" : "否";
                    tmp.Add(isSwitchinputJudgeByA);
                }
                else
                {
                    tmp.Add("");
                }

                //回路信息

                string str = "";
                var ggg =
                    (from gg in tmp2.WjSwitchOuts.Values orderby gg.SwitchId select gg).
                        ToList();
                foreach (var g in ggg)
                {
                    str += g.SwitchVecotr + "-";
                }
                if (str.Length > 1) str = str.Substring(0, str.Length - 1);
                tmp.Add(str);

                var sout = new Dictionary<int, int>();
                var shield = new Dictionary<int, Tuple<int, double>>();
                var alarmHop = new Dictionary<int, bool>();
                var kglsl = "";
                var mnlsl = "";
                var tbbjsl = "";
                var xwsl = "";
                var hgxbsl = "";

                var gggg =
                    (from gg in tmp2.WjLoops.Values orderby gg.LoopId select gg).
                        ToList();

                foreach (var g in gggg)
                {
                    if (!sout.ContainsKey(g.SwitchOutputId)) sout.Add(g.SwitchOutputId, 0);
                    sout[g.SwitchOutputId] = sout[g.SwitchOutputId] + 1;

                    if (!shield.ContainsKey(g.SwitchOutputId))
                    {
                        var a = new Tuple<int, double>(g.IsShieldLoop, g.ShieldLittleA);
                        shield.Add(g.SwitchOutputId, a);
                    }
                    if (!alarmHop.ContainsKey(g.SwitchOutputId)) alarmHop.Add(g.SwitchOutputId, g.IsAlarmHop);

                    kglsl += g.VectorSwitchIn + "-";
                    mnlsl += g.VectorMoniliang + "-";
                    tbbjsl += (g.IsAlarmHop ? 1 : 0) + "-";

                    if (g.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Aphase) xwsl += 0 + "-";
                    else if (g.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Bphase) xwsl += 1 + "-";
                    else if (g.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Cphase) xwsl += 2 + "-";
                    else xwsl += g.VoltagePhaseCode + "-";

                    hgxbsl += g.CurrentRange + "-";
                }
                int hlsum = 0;
                for (int i = 1; i < 9; i++)
                {
                    if (sout.ContainsKey(i))
                    {
                        tmp.Add(sout[i]);
                        hlsum = hlsum + sout[i];
                    }
                    else
                    {
                        tmp.Add(0);
                    }
                }

                tmp.Add(hlsum);
                tmp.Add(ggg.Count);

                if (kglsl.Length > 1) kglsl = kglsl.Substring(0, kglsl.Length - 1);
                if (mnlsl.Length > 1) mnlsl = mnlsl.Substring(0, mnlsl.Length - 1);
                if (tbbjsl.Length > 1) tbbjsl = tbbjsl.Substring(0, tbbjsl.Length - 1);
                if (xwsl.Length > 1) xwsl = xwsl.Substring(0, xwsl.Length - 1);
                if (hgxbsl.Length > 1) hgxbsl = hgxbsl.Substring(0, hgxbsl.Length - 1);

                tmp.Add(kglsl);
                tmp.Add(mnlsl);
                tmp.Add(tbbjsl);
                tmp.Add(xwsl);
                tmp.Add(hgxbsl);




                // 防盗信息
                string fdname = "";
                int fdsum = 0;

                if (fssbinfo.ContainsKey(t.RtuId))
                {
                    foreach (var f in fssbinfo[t.RtuId])
                    {
                        if (EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                        {
                            if (EquipmentDataInfoHold.InfoItems[f].RtuModel == EnumRtuModel.Wj1090)
                            {
                                var fdxx = EquipmentDataInfoHold.InfoItems[f] as Wj1090Ldu;
                                var ntg = (from txr in fdxx.WjLduLines orderby txr.Value.LduLineId ascending select txr.Value).ToList();
                                foreach (var tg in ntg)
                                {
                                    fdname = fdname + tg.LduLineName + "/";
                                    fdsum = fdsum + 1;
                                }

                            }
                        }
                    }
                }
                tmp.Add(fdsum);
                tmp.Add(fdname);

                // 电表变比
                string dbbianbi = "";
                if (fssbinfo.ContainsKey(t.RtuId))
                {
                    foreach (var f in fssbinfo[t.RtuId])
                    {
                        if (EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                        {
                            if (EquipmentDataInfoHold.InfoItems[f].RtuModel == EnumRtuModel.Wj1050)
                            {
                                var fdxx = EquipmentDataInfoHold.InfoItems[f] as Wj1050Mru;
                                dbbianbi = dbbianbi + fdxx.WjMru.MruRatio ;
                            }
                        }
                    }
                }
                tmp.Add(dbbianbi);

                // 设备型号
                string sbxh = "";
                sbxh = (int)tmp2.RtuModel+"";
                tmp.Add(sbxh);


                //设备动态配置文件
                if (Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks.Count != 0)
                {
                    if (_dicRemarks.ContainsKey(t.RtuId) )
                    {
                        var lsttt = _dicRemarks[t.RtuId];
                        for (int i = 0; i < Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks.Count; i++)
                        {
                            tmp.Add(lsttt[i]);
                        }
                    }

                }




                #region Old

                //    //titleinfo.Add("输出矢量序列");

                //        string str = "";
                //        var ggg =
                //            (from gg in tmp2.WjSwitchOuts.Values  orderby gg.SwitchId  select gg).
                //                ToList();
                //        foreach (var g in ggg )
                //        {
                //            str += g.SwitchVecotr + "-";
                //        }
                //        if (str.Length > 1) str = str.Substring(0, str.Length - 1);
                //        tmp.Add(str);

                //        var sout = new Dictionary<int, int>();
                //    var shield = new Dictionary<int, Tuple<bool, double>>();
                //    var alarmHop = new Dictionary<int, bool>();
                //        var kglsl = "";
                //        var mnlsl = "";
                //        var tbbjsl = "";
                //        var xwsl = "";
                //        var hgxbsl = "";

                //        var gggg =
                //            (from gg in tmp2.WjLoops .Values   orderby gg.LoopId select gg).
                //                ToList();

                //        foreach (var g in gggg)
                //        {
                //            if (!sout.ContainsKey(g.SwitchOutputId )) sout.Add(g.SwitchOutputId , 0);
                //            sout[g.SwitchOutputId ] = sout[g.SwitchOutputId ] + 1;

                //            if(!shield.ContainsKey(g.SwitchOutputId ))
                //            {
                //                var a = new Tuple<bool, double>(g.IsShieldLoop,g.ShieldLittleA);
                //                shield.Add(g.SwitchOutputId,a);
                //            }
                //            if (!alarmHop.ContainsKey(g.SwitchOutputId)) alarmHop.Add(g.SwitchOutputId, g.IsAlarmHop);                    

                //            kglsl += g.VectorSwitchIn + "-";
                //            mnlsl += g.VectorMoniliang + "-";
                //            tbbjsl += (g.IsAlarmHop  ? 1 : 0) + "-";
                //            xwsl += g.VoltagePhaseCode  + "-";
                //            hgxbsl += g.CurrentRange  + "-";
                //        }
                //        for (int i = 1; i < 9; i++)
                //        {
                //            if (sout.ContainsKey(i))
                //            {
                //                tmp.Add(sout[i]);                           
                //            }
                //            else
                //            {
                //                tmp.Add(0);                         
                //            }

                //        }

                //        tmp.Add(ggg .Count );

                //        if (kglsl.Length > 1) kglsl = kglsl.Substring(0, kglsl.Length - 1);
                //        if (mnlsl.Length > 1) mnlsl = mnlsl.Substring(0, mnlsl.Length - 1);
                //        if (tbbjsl.Length > 1) tbbjsl = tbbjsl.Substring(0, tbbjsl.Length - 1);
                //        if (xwsl.Length > 1) xwsl = xwsl.Substring(0, xwsl.Length - 1);
                //        if (hgxbsl.Length > 1) hgxbsl = hgxbsl.Substring(0, hgxbsl.Length - 1);

                //        tmp.Add(kglsl);
                //        tmp.Add(mnlsl);
                //        tmp.Add(tbbjsl);
                //        tmp.Add(xwsl);
                //        tmp.Add(hgxbsl);


                //    writeinfo.Add(tmp);
                //}


                //var instmp = (from f in lst select f.RtuId).ToList();
                //var ntgs = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                //            where t.Value.EquipmentType == WjParaBase.EquType.Rtu   &&
                //                 !instmp.Contains(t.Key)
                //           select t.Value ).ToList();
                //foreach (var t in ntgs)
                //{
                //    var tmp = new List<object>();
                //    tmp.Add(t.RtuPhyId  );
                //    tmp.Add(t.RtuId);
                //    tmp.Add(t.RtuName);
                //    tmp.Add("");
                //    tmp.Add(t.RtuStateCode ==0?"不用":"未知");
                //    var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById( t.RtuId);
                //    var tmp2 = tmp1 as Wj3005Rtu ;
                //    if (tmp2 == null) return;
                //    {
                //        tmp.Add(tmp2.WjGprs .StaticIp );
                //        tmp.Add(tmp2.WjGprs .MobileNo );
                //        tmp.Add(tmp2.WjGprs .RtuHeartbeatCycle );
                //        tmp.Add(tmp2.WjGprs .RtuReportCycle );
                //    }



                //    //titleinfo.Add("输出矢量序列");


                //    {
                //        string str = "";
                //        var ggg =
                //            (from gg in tmp2.WjSwitchOuts .Values  orderby gg.SwitchId  select gg).
                //                ToList();
                //        foreach (var g in ggg )
                //        {
                //            str += g.SwitchVecotr  + "-";
                //        }
                //        if (str.Length > 1) str = str.Substring(0, str.Length - 1);
                //        tmp.Add(str);
                //    }



                //    //titleinfo.Add("K1路数");
                //    //titleinfo.Add("K2路数");
                //    //titleinfo.Add("K3路数");
                //    //titleinfo.Add("K4路数");
                //    //titleinfo.Add("K5路数");
                //    //titleinfo.Add("K6路数");



                //    //titleinfo.Add("开关量矢量序列");
                //    //titleinfo.Add("模拟量矢量序列");
                //    //titleinfo.Add("跳变报警序列");
                //    //titleinfo.Add("相位序列");
                //    //titleinfo.Add("互感器比序列");


                //    {
                //        var sout = new Dictionary<int, int>();
                //        var kglsl = "";
                //        var mnlsl = "";
                //        var tbbjsl = "";
                //        var xwsl = "";
                //        var hgxbsl = "";

                //        var ggg =
                //            (from gg in tmp2.WjLoops .Values  orderby gg.LoopId select gg).
                //                ToList();

                //        foreach (var g in ggg)
                //        {
                //            if (!sout.ContainsKey(g.SwitchOutputId)) sout.Add(g.SwitchOutputId, 0);
                //            sout[g.SwitchOutputId] = sout[g.SwitchOutputId] + 1;

                //            kglsl += g.VectorSwitchIn + "-";
                //            mnlsl += g.VectorMoniliang + "-";
                //            tbbjsl += (g.IsAlarmHop  ? 1 : 0) + "-";
                //            xwsl += g.VoltagePhaseCode  + "-";
                //            hgxbsl += g.CurrentRange  + "-";
                //        }
                //        for (int i = 1; i < 7; i++)
                //            if (sout.ContainsKey(i)) tmp.Add(sout[i]);
                //            else tmp.Add(0);
                //        tmp.Add(ggg.Count);

                //        if (kglsl.Length > 1) kglsl = kglsl.Substring(0, kglsl.Length - 1);
                //        if (mnlsl.Length > 1) mnlsl = mnlsl.Substring(0, mnlsl.Length - 1);
                //        if (tbbjsl.Length > 1) tbbjsl = tbbjsl.Substring(0, tbbjsl.Length - 1);
                //        if (xwsl.Length > 1) xwsl = xwsl.Substring(0, xwsl.Length - 1);
                //        if (hgxbsl.Length > 1) hgxbsl = hgxbsl.Substring(0, hgxbsl.Length - 1);

                //        tmp.Add(kglsl);
                //        tmp.Add(mnlsl);
                //        tmp.Add(tbbjsl);
                //        tmp.Add(xwsl);
                //        tmp.Add(hgxbsl);
                //    }
                #endregion
                writeinfo.Add(tmp);
            }

            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);
        }

        private Tuple<List<object>, List<List<object>>> WriteDataInsNewChangsha()
        {
            //储存附属设备
            var fssbinfo = new Dictionary<int, List<int>>();
            // var lst = new List<>();

            var lstt = new ObservableCollection<PartolItemViewModel>();
            Dictionary<int, string> tmpdir = new Dictionary<int, string>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
            {
                //if (t.Value.LstTml.Count == 0) continue;
                foreach (var g in t.Value.LstTml)
                {
                    if (tmpdir.ContainsKey(g)) continue;
                    tmpdir.Add(g, t.Value.GroupName + "-" + t.Key);
                }
            }

            foreach (
                  var t in
                      Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                  )
            {
                if (t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;

                #region 巡测数据加载
                var ttt = new PartolItemViewModel() { RtuId = t.Key, PhysicalId = t.Value.RtuPhyId };
                ttt.RtuName = t.Value.RtuName;
                ttt.RequestNewDataTime = "--";
                ttt.ReceiveNewDataTime = "--";
                ttt.TimeSpan = "--";
                ttt.RtuState = t.Value.RtuStateCode == 2 ? "使用" : t.Value.RtuStateCode == 1 ? "停运" : "不用";
                if (tmpdir.ContainsKey(t.Key)) ttt.GrpName = tmpdir[t.Key];
//                else ttt.GrpName = "----";
                else ttt.GrpName = Get_Special_GrpName(t.Key);

                var tmp = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.Key);
                if (tmp != null && tmp.RtuNewData != null)
                {
                    ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                    ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                    ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                    ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                    ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                    ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                    ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                    ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;
                    ttt.ErrorCount = tmp.ErrorCount;
                }

                lstt.Add(ttt);
                #endregion

            }

            foreach (var t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.RtuFid != 0)
                {
                    if (fssbinfo.ContainsKey(t.Value.RtuFid))
                    {
                        fssbinfo[t.Value.RtuFid].Add(t.Key);
                    }
                    else
                    {
                        fssbinfo.Add(t.Value.RtuFid, new List<int>() { t.Key });
                    }
                }
                //else
                //{
                //    if (t.Value.EquipmentType == WjParaBase.EquType.Rtu) 
                //    {
                //        lst.Add(t.Value);
                //    }
                //}
            }

            var ZcInfoR = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();
            Dictionary<int,LampInfo> ZcInfo = new Dictionary<int, LampInfo>();
            foreach (var t in ZcInfoR)
            {
                if (!ZcInfo.ContainsKey(t.Value.RtuId))
                    ZcInfo.Add(t.Value.RtuId, t.Value);
            }


            //建立表头
            var writeinfo = new List<List<object>>();
            var titleinfo = new List<object>();
            titleinfo.Add("序号");

            titleinfo.Add("物理地址");
            //titleinfo.Add("逻辑地址");
            titleinfo.Add("终端名称");

            titleinfo.Add("Ip地址");
            titleinfo.Add("电源杆号");

            titleinfo.Add("K1路数");
            titleinfo.Add("K2路数");
            titleinfo.Add("K3路数");
            titleinfo.Add("K4路数");
            titleinfo.Add("K5路数");
            titleinfo.Add("K6路数");
            titleinfo.Add("K7路数");
            titleinfo.Add("K8路数");
            titleinfo.Add("回路总数");

            titleinfo.Add("相位序列");

            titleinfo.Add("输出矢量序列");

            titleinfo.Add("使用状态");

            titleinfo.Add("心跳周期");
            titleinfo.Add("电表表号");
            titleinfo.Add("电表变比");
            titleinfo.Add("主报周期");

            titleinfo.Add("互感器比序列");
            titleinfo.Add("开通日期");

            titleinfo.Add("归属分组");

            titleinfo.Add("城区局");

            titleinfo.Add("安装位置");
            titleinfo.Add("备注信息");

            titleinfo.Add("最新数据时间");


            int index = 1;
            var lst = (from t in lstt orderby t.PhysicalId select t).ToList();
            foreach (var t in lst)
            {
                var tmp = new List<object>();

                tmp.Add(index);
                index++;

                tmp.Add(t.PhysicalId);
                //tmp.Add(t.RtuId);
                tmp.Add(t.RtuName);

                var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);
                if (tmp1 == null || tmp1.EquipmentType != WjParaBase.EquType.Rtu) continue;
                var tmp2 = tmp1 as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmp2 == null) continue;
                if (tmp2.WjGprs == null || tmp2.WjLoops == null || tmp2.WjSwitchOuts == null || tmp2.WjVoltage == null) continue;

                tmp.Add(new System.Net.IPAddress(BitConverter.GetBytes(tmp2.WjGprs.StaticIp)).ToString());

                if (ZcInfo.ContainsKey(t.RtuId)) tmp.Add(ZcInfo[t.RtuId].Dygh);
                else tmp.Add("");

                //回路
                string str = "";
                var ggg =
                    (from gg in tmp2.WjSwitchOuts.Values orderby gg.SwitchId select gg).
                        ToList();
                foreach (var g in ggg)
                {
                    str += g.SwitchVecotr + "/";
                }
                if (str.Length > 1) str = str.Substring(0, str.Length - 1);
                
                var sout = new Dictionary<int, int>();
                var shield = new Dictionary<int, Tuple<int, double>>();
                var alarmHop = new Dictionary<int, bool>();
                var kglsl = "";
                var mnlsl = "";
                var tbbjsl = "";
                var xwsl = "";
                var hgxbsl = "";

                var gggg =
                    (from gg in tmp2.WjLoops.Values orderby gg.LoopId select gg).
                        ToList();
                foreach (var g in gggg)
                {
                    if (!sout.ContainsKey(g.SwitchOutputId)) sout.Add(g.SwitchOutputId, 0);
                    sout[g.SwitchOutputId] = sout[g.SwitchOutputId] + 1;
                }

                int hlsum = 0;
                for (int i = 1; i < 9; i++)
                {
                    if (sout.ContainsKey(i))
                    {
                        tmp.Add(sout[i]);
                        hlsum = hlsum + sout[i];
                    }
                    else
                    {
                        tmp.Add(0);
                    }
                }

                int meng = 0;
                if (sout.ContainsKey(0))
                {
                    meng = sout[0];
                    hlsum = hlsum + meng;
                }
                tmp.Add(hlsum);

                int loop = 0;
                foreach (var g in gggg)
                {
                    if (!shield.ContainsKey(g.SwitchOutputId))
                    {
                        var a = new Tuple<int, double>(g.IsShieldLoop, g.ShieldLittleA);
                        shield.Add(g.SwitchOutputId, a);
                    }
                    if (!alarmHop.ContainsKey(g.SwitchOutputId)) alarmHop.Add(g.SwitchOutputId, g.IsAlarmHop);

                    kglsl += g.VectorSwitchIn + "/";
                    mnlsl += g.VectorMoniliang + "/";
                    tbbjsl += (g.IsAlarmHop ? 1 : 0) + "/";


                    if (loop < gggg.Count - meng)
                    {
                        if (g.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Aphase) xwsl += 0 + "/";
                        else if (g.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Bphase) xwsl += 1 + "/";
                        else if (g.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Cphase) xwsl += 2 + "/";
                        else xwsl += g.VoltagePhaseCode + "/";

                        hgxbsl += g.CurrentRange + "/";

                    }
                   
                    loop++;
                }

                

                if (kglsl.Length > 1) kglsl = kglsl.Substring(0, kglsl.Length - 1);
                if (mnlsl.Length > 1) mnlsl = mnlsl.Substring(0, mnlsl.Length - 1);
                if (tbbjsl.Length > 1) tbbjsl = tbbjsl.Substring(0, tbbjsl.Length - 1);
                if (xwsl.Length > 1) xwsl = xwsl.Substring(0, xwsl.Length - 1);
                if (hgxbsl.Length > 1) hgxbsl = hgxbsl.Substring(0, hgxbsl.Length - 1);
                
                tmp.Add(xwsl);
                tmp.Add(str);

                tmp.Add(t.RtuState);

                tmp.Add(tmp2.WjGprs.RtuHeartbeatCycle);


                if (ZcInfo.ContainsKey(t.RtuId)) tmp.Add(ZcInfo[t.RtuId].Dbbh);
                else tmp.Add("");


                // 电表变比
                string dbbianbi = "";
                if (fssbinfo.ContainsKey(t.RtuId))
                {
                    foreach (var f in fssbinfo[t.RtuId])
                    {
                        if (EquipmentDataInfoHold.InfoItems.ContainsKey(f))
                        {
                            if (EquipmentDataInfoHold.InfoItems[f].RtuModel == EnumRtuModel.Wj1050)
                            {
                                var fdxx = EquipmentDataInfoHold.InfoItems[f] as Wj1050Mru;
                                dbbianbi = dbbianbi + fdxx.WjMru.MruRatio ;
                            }
                        }
                    }
                }
                tmp.Add(dbbianbi);

                tmp.Add(tmp2.WjGprs.RtuReportCycle);

                tmp.Add(hgxbsl);

                tmp.Add(string.Format("{0:d}", new DateTime(tmp2.DateCreate)));
                
                tmp.Add(t.GrpName);

                if (ZcInfo.ContainsKey(t.RtuId)) tmp.Add(ZcInfo[t.RtuId].Cqj);
                else tmp.Add("");

                tmp.Add(tmp2.RtuInstallAddr);
                tmp.Add(tmp2.RtuRemark);

                var run1 = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.RtuId);
                if (run1 != null && run1.RtuNewData != null)
                {
                    tmp.Add(run1.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    tmp.Add("");
                }

                writeinfo.Add(tmp);
            }
            return new Tuple<List<object>, List<List<object>>>(titleinfo, writeinfo);
           // Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);
        }

        private void WriteDataIns()
        {
            var writeinfo = new List<List<object>>();
            var titleinfo = new List<object>();
            titleinfo.Add("物理地址");
            titleinfo.Add("逻辑地址");
            titleinfo.Add("终端名称");
            titleinfo.Add("归属分组");
            titleinfo.Add("使用状态");
          
            titleinfo.Add("Ip地址");
            
            titleinfo.Add("电话号码");
            titleinfo.Add("心跳周期");
            titleinfo.Add("主报周期");

            //二期新增参数
            titleinfo.Add("开通日期");
            titleinfo.Add("最后更新日期");
            titleinfo.Add("安装位置");
            titleinfo.Add("备注信息");
            titleinfo.Add("唯一识别码");
            titleinfo.Add("是否通过电流判断辅助触点");

            
           
            //titleinfo.Add("输出矢量序列");
            //titleinfo.Add("K1路数");            
            //titleinfo.Add("K2路数");         
            //titleinfo.Add("K3路数");           
            //titleinfo.Add("K4路数");
            //titleinfo.Add("K5路数");                     
            //titleinfo.Add("K6路数");          
            //titleinfo.Add("K7路数");           
            //titleinfo.Add("K8路数");           
            //titleinfo.Add("开关量路数");
            //titleinfo.Add("开关量矢量序列");
            //titleinfo.Add("模拟量矢量序列");
            //titleinfo.Add("跳变报警序列");
            //titleinfo.Add("相位序列");
            //titleinfo.Add("互感器比序列");

            var lst = (from t in MeasurePatrolData orderby t.PhysicalId select t).ToList();
            foreach (var t in lst)
            {
                var tmp = new List<object>();
                tmp.Add(t.PhysicalId);
                tmp.Add(t.RtuId);
                tmp.Add(t.RtuName);
                tmp.Add(t.GrpName);
                tmp.Add(t.RtuState);
                var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById( t.RtuId);
                if (tmp1 == null ||tmp1.EquipmentType !=WjParaBase.EquType.Rtu ) continue;
                var tmp2 = tmp1 as Wlst .Sr .EquipmentInfoHolding .Model .Wj3005Rtu ;
                if(tmp2 ==null ) return;
                
                tmp.Add(new System.Net.IPAddress(BitConverter.GetBytes(tmp2.WjGprs.StaticIp)).ToString());
                    tmp.Add(tmp2.WjGprs  .MobileNo );
                    tmp.Add(tmp2.WjGprs .RtuHeartbeatCycle );
                    tmp.Add(tmp2.WjGprs .RtuReportCycle );
               
                //二期新增参数
                tmp.Add(string.Format("{0:G}", new DateTime(tmp2.DateCreate)));               
                tmp.Add(string.Format("{0:G}", new DateTime(tmp2.DateUpdate)));
                tmp.Add(tmp2.RtuInstallAddr);
                tmp.Add(tmp2.RtuRemark);
                tmp.Add(tmp2.Idf);
                if(tmp2.WjVoltage!=null)
                {
                    string isSwitchinputJudgeByA = (tmp2.WjVoltage.IsSwitchinputJudgebyA == true) ? "是" : "否";
                    tmp.Add(isSwitchinputJudgeByA);
                }
                else
                {
                    tmp.Add("");
                }
                


                #region Old

                //    //titleinfo.Add("输出矢量序列");
 
            //        string str = "";
            //        var ggg =
            //            (from gg in tmp2.WjSwitchOuts.Values  orderby gg.SwitchId  select gg).
            //                ToList();
            //        foreach (var g in ggg )
            //        {
            //            str += g.SwitchVecotr + "-";
            //        }
            //        if (str.Length > 1) str = str.Substring(0, str.Length - 1);
            //        tmp.Add(str);
              
            //        var sout = new Dictionary<int, int>();
            //    var shield = new Dictionary<int, Tuple<bool, double>>();
            //    var alarmHop = new Dictionary<int, bool>();
            //        var kglsl = "";
            //        var mnlsl = "";
            //        var tbbjsl = "";
            //        var xwsl = "";
            //        var hgxbsl = "";

            //        var gggg =
            //            (from gg in tmp2.WjLoops .Values   orderby gg.LoopId select gg).
            //                ToList();

            //        foreach (var g in gggg)
            //        {
            //            if (!sout.ContainsKey(g.SwitchOutputId )) sout.Add(g.SwitchOutputId , 0);
            //            sout[g.SwitchOutputId ] = sout[g.SwitchOutputId ] + 1;
                        
            //            if(!shield.ContainsKey(g.SwitchOutputId ))
            //            {
            //                var a = new Tuple<bool, double>(g.IsShieldLoop,g.ShieldLittleA);
            //                shield.Add(g.SwitchOutputId,a);
            //            }
            //            if (!alarmHop.ContainsKey(g.SwitchOutputId)) alarmHop.Add(g.SwitchOutputId, g.IsAlarmHop);                    

            //            kglsl += g.VectorSwitchIn + "-";
            //            mnlsl += g.VectorMoniliang + "-";
            //            tbbjsl += (g.IsAlarmHop  ? 1 : 0) + "-";
            //            xwsl += g.VoltagePhaseCode  + "-";
            //            hgxbsl += g.CurrentRange  + "-";
            //        }
            //        for (int i = 1; i < 9; i++)
            //        {
            //            if (sout.ContainsKey(i))
            //            {
            //                tmp.Add(sout[i]);                           
            //            }
            //            else
            //            {
            //                tmp.Add(0);                         
            //            }
                        
            //        }
                        
            //        tmp.Add(ggg .Count );

            //        if (kglsl.Length > 1) kglsl = kglsl.Substring(0, kglsl.Length - 1);
            //        if (mnlsl.Length > 1) mnlsl = mnlsl.Substring(0, mnlsl.Length - 1);
            //        if (tbbjsl.Length > 1) tbbjsl = tbbjsl.Substring(0, tbbjsl.Length - 1);
            //        if (xwsl.Length > 1) xwsl = xwsl.Substring(0, xwsl.Length - 1);
            //        if (hgxbsl.Length > 1) hgxbsl = hgxbsl.Substring(0, hgxbsl.Length - 1);

            //        tmp.Add(kglsl);
            //        tmp.Add(mnlsl);
            //        tmp.Add(tbbjsl);
            //        tmp.Add(xwsl);
            //        tmp.Add(hgxbsl);
                   
               
            //    writeinfo.Add(tmp);
            //}


            //var instmp = (from f in lst select f.RtuId).ToList();
            //var ntgs = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
            //            where t.Value.EquipmentType == WjParaBase.EquType.Rtu   &&
            //                 !instmp.Contains(t.Key)
            //           select t.Value ).ToList();
            //foreach (var t in ntgs)
            //{
            //    var tmp = new List<object>();
            //    tmp.Add(t.RtuPhyId  );
            //    tmp.Add(t.RtuId);
            //    tmp.Add(t.RtuName);
            //    tmp.Add("");
            //    tmp.Add(t.RtuStateCode ==0?"不用":"未知");
            //    var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById( t.RtuId);
            //    var tmp2 = tmp1 as Wj3005Rtu ;
            //    if (tmp2 == null) return;
            //    {
            //        tmp.Add(tmp2.WjGprs .StaticIp );
            //        tmp.Add(tmp2.WjGprs .MobileNo );
            //        tmp.Add(tmp2.WjGprs .RtuHeartbeatCycle );
            //        tmp.Add(tmp2.WjGprs .RtuReportCycle );
            //    }
                


            //    //titleinfo.Add("输出矢量序列");

   
            //    {
            //        string str = "";
            //        var ggg =
            //            (from gg in tmp2.WjSwitchOuts .Values  orderby gg.SwitchId  select gg).
            //                ToList();
            //        foreach (var g in ggg )
            //        {
            //            str += g.SwitchVecotr  + "-";
            //        }
            //        if (str.Length > 1) str = str.Substring(0, str.Length - 1);
            //        tmp.Add(str);
            //    }
               


            //    //titleinfo.Add("K1路数");
            //    //titleinfo.Add("K2路数");
            //    //titleinfo.Add("K3路数");
            //    //titleinfo.Add("K4路数");
            //    //titleinfo.Add("K5路数");
            //    //titleinfo.Add("K6路数");



            //    //titleinfo.Add("开关量矢量序列");
            //    //titleinfo.Add("模拟量矢量序列");
            //    //titleinfo.Add("跳变报警序列");
            //    //titleinfo.Add("相位序列");
            //    //titleinfo.Add("互感器比序列");

  
            //    {
            //        var sout = new Dictionary<int, int>();
            //        var kglsl = "";
            //        var mnlsl = "";
            //        var tbbjsl = "";
            //        var xwsl = "";
            //        var hgxbsl = "";

            //        var ggg =
            //            (from gg in tmp2.WjLoops .Values  orderby gg.LoopId select gg).
            //                ToList();

            //        foreach (var g in ggg)
            //        {
            //            if (!sout.ContainsKey(g.SwitchOutputId)) sout.Add(g.SwitchOutputId, 0);
            //            sout[g.SwitchOutputId] = sout[g.SwitchOutputId] + 1;

            //            kglsl += g.VectorSwitchIn + "-";
            //            mnlsl += g.VectorMoniliang + "-";
            //            tbbjsl += (g.IsAlarmHop  ? 1 : 0) + "-";
            //            xwsl += g.VoltagePhaseCode  + "-";
            //            hgxbsl += g.CurrentRange  + "-";
            //        }
            //        for (int i = 1; i < 7; i++)
            //            if (sout.ContainsKey(i)) tmp.Add(sout[i]);
            //            else tmp.Add(0);
            //        tmp.Add(ggg.Count);

            //        if (kglsl.Length > 1) kglsl = kglsl.Substring(0, kglsl.Length - 1);
            //        if (mnlsl.Length > 1) mnlsl = mnlsl.Substring(0, mnlsl.Length - 1);
            //        if (tbbjsl.Length > 1) tbbjsl = tbbjsl.Substring(0, tbbjsl.Length - 1);
            //        if (xwsl.Length > 1) xwsl = xwsl.Substring(0, xwsl.Length - 1);
            //        if (hgxbsl.Length > 1) hgxbsl = hgxbsl.Substring(0, hgxbsl.Length - 1);

            //        tmp.Add(kglsl);
            //        tmp.Add(mnlsl);
            //        tmp.Add(tbbjsl);
            //        tmp.Add(xwsl);
            //        tmp.Add(hgxbsl);
                //    }
                #endregion
                writeinfo.Add(tmp);
            }

            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);
        }

        private void WriteDataRunningNew()
        {
            var writeinfo = new List<List<object>>();
            var titleinfo = new List<object>();
            titleinfo.Add("终端id");
            titleinfo.Add("终端名称");

            titleinfo.Add("使用状态");
            titleinfo.Add("在线");

            titleinfo.Add("回路地址");
            titleinfo.Add("回路名称");
            titleinfo.Add("分组信息");
            titleinfo.Add("是否屏蔽小电流");
            titleinfo.Add("屏蔽小电流值"); 
            titleinfo.Add("是否跳变报警");
            titleinfo.Add("模拟量矢量");
            titleinfo.Add("电流量程");
            titleinfo.Add("报警上限");
            titleinfo.Add("报警下限");
            titleinfo.Add("相位");
            titleinfo.Add("互感器比");   
            titleinfo.Add("电压");
            titleinfo.Add("电流");
            titleinfo.Add("功率");
            titleinfo.Add("亮灯率");
            titleinfo.Add("功率因数");
            titleinfo.Add("辅助触点状态");
            titleinfo.Add("报警数量");
            titleinfo.Add("输出回路状态");
            titleinfo.Add("A相总电流");
            titleinfo.Add("B相总电流");
            titleinfo.Add("C相总电流");
            titleinfo.Add("ABC相总电流");
            titleinfo.Add("总功率");
            titleinfo.Add("A相电压");
            titleinfo.Add("B相电压");
            titleinfo.Add("C相电压");
            titleinfo.Add("最新数据时间");
            
            var tmllst = (from t in MeasurePatrolData orderby t.PhysicalId select t).ToList();
            foreach (var f in tmllst)
            {
                
                var runinfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(f.RtuId);
                var loopinfo2 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(f.RtuId);
                var loopinfo = loopinfo2 as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                double power = 0;
                if (loopinfo == null) return;
                

                foreach (var t in loopinfo.WjLoops.Values)
                {

                    var tmp = new List<object>();
                    tmp.Add(f.RtuId);
                    tmp.Add(f.RtuName);

                    tmp.Add(f.RtuState);
                    tmp.Add(f.OnLine.Contains("-") ? "离线" : "在线");

                    tmp.Add(t.LoopId);
                    tmp.Add(t.LoopName);
                    tmp.Add(f.GrpName);

                    if (t.ShieldLittleA > 0) { tmp.Add("是"); }
                    else { tmp.Add("否"); }

                    tmp.Add(t.ShieldLittleA);

                    if (t.IsAlarmHop == false) {tmp.Add("否");}
                    else {tmp.Add("是");}

                    tmp.Add(t.VectorMoniliang);
                    tmp.Add(t.CurrentRange);
                    tmp.Add(t.CurrentAlarmUpperlimit);
                    tmp.Add(t.CurrentAlarmLowerlimit);
                    if (t.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Aphase) tmp.Add("A");
                    else if (t.VoltagePhaseCode == Wlst.client.EnumVoltagePhase.Bphase) tmp.Add("B");
                    else tmp.Add("C");
                    tmp.Add(t.MutualInductorRatio + "/5");
                    if (runinfo ==null || runinfo.RtuNewData == null)
                    {
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");
                        tmp.Add("");  
                    }
                    else
                    {
                        if (runinfo.RtuNewData.LstNewLoopsData == null)
                        {
                            tmp.Add("");
                            tmp.Add("");
                            tmp.Add("");
                            tmp.Add("");
                            tmp.Add("");
                            tmp.Add("");
                        }
                        else
                        {
                            var flg = false;
                            foreach (var x in runinfo.RtuNewData.LstNewLoopsData)
                            {
                                if (x.LoopId == t.LoopId)
                                {
                                    tmp.Add(x.V);
                                    tmp.Add(x.A);
                                    tmp.Add(x.Power);
                                    tmp.Add(x.BrightRate*100 + "%");
                                    tmp.Add(x.PowerFactor);
                                    if (x.BolSwitchInState == false)
                                    {
                                        tmp.Add("否");
                                    }
                                    else
                                    {
                                        tmp.Add("是");
                                    }
                                    flg = true;
                                }
                                power += x.Power;
                            }
                            if (!flg)
                            {
                                tmp.Add("");
                                tmp.Add("");
                                tmp.Add("");
                                tmp.Add("");
                                tmp.Add("");
                                tmp.Add("");
                            }
                        }


                        tmp.Add(runinfo.RtuNewData.AlarmCount);
                        if (runinfo.RtuNewData.IsSwitchOutAttraction.Count <= t.LoopId - 1)
                        {
                            tmp.Add("--");
                        }
                        else
                        {
                            if (runinfo.RtuNewData.IsSwitchOutAttraction[t.LoopId - 1] == false) { tmp.Add("否"); }
                            else { tmp.Add("是"); }
                        }


                        tmp.Add(runinfo.RtuNewData.RtuCurrentSumA);
                        tmp.Add(runinfo.RtuNewData.RtuCurrentSumB);
                        tmp.Add(runinfo.RtuNewData.RtuCurrentSumC);
                        tmp.Add(
                            (runinfo.RtuNewData.RtuCurrentSumA + runinfo.RtuNewData.RtuCurrentSumB +
                             runinfo.RtuNewData.RtuCurrentSumC).ToString("f2"));
                        tmp.Add(power.ToString("f2"));
                        tmp.Add(runinfo.RtuNewData.RtuVoltageA);
                        tmp.Add(runinfo.RtuNewData.RtuVoltageB);
                        tmp.Add(runinfo.RtuNewData.RtuVoltageC);
                        tmp.Add(runinfo.RtuNewData.DateCreate);
                        
                    }
                    writeinfo.Add(tmp);
                }

                var tmpx = new List<object>();
                tmpx.Add(" ");
                tmpx.Add("开关量输出状态");
                if (runinfo!=null && runinfo.RtuNewData!=null && runinfo.RtuNewData.IsSwitchOutAttraction != null)
                {
                    int intx = 1;
                    foreach (var t in runinfo.RtuNewData.IsSwitchOutAttraction)
                    {
                        tmpx.Add("K"+ intx);
                        if (t == true)tmpx.Add("开");
                        else tmpx.Add("关");

                        intx++;
                    }
                    if (intx <= 8)
                    {
                        for (int i = intx; i <= 8; i++)
                        {
                            tmpx.Add("K" + i);
                            tmpx.Add("--");
                        }
                    }
                }
                else
                {
                    tmpx.Add("K1");
                    tmpx.Add("--");
                    tmpx.Add("K2");
                    tmpx.Add("--");
                    tmpx.Add("K3");
                    tmpx.Add("--"); ;
                    tmpx.Add("K4");
                    tmpx.Add("--");
                    tmpx.Add("K5");
                    tmpx.Add("--");
                    tmpx.Add("K6");
                    tmpx.Add("--");
                    tmpx.Add("K7");
                    tmpx.Add("--");
                    tmpx.Add("K8");
                    tmpx.Add("--");
                }

                writeinfo.Add(tmpx);

                var tmpxx = new List<object>();
                writeinfo.Add(tmpxx);
            }
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);

        }

        private Tuple<List<object>, List<List<object>>> WriteDataRunningNewChangsha(bool state)
        {
            var writeinfo = new List<List<object>>();
            var titleinfo = new List<object>();

            titleinfo.Add("序号");
            titleinfo.Add("城区局");

            if (state)
            {
                //titleinfo.Add("所编号");
            }
            else
            {
                titleinfo.Add("物理地址");
            }
            
            titleinfo.Add("终端名称");
            titleinfo.Add("最新数据时间");

            if (state)
            {
                titleinfo.Add("电源杆号");
                titleinfo.Add("终端状态");
                titleinfo.Add("开关灯状态");
                titleinfo.Add("门控状态");
                titleinfo.Add("电压值(V)");
                titleinfo.Add("电流值(A)");
                titleinfo.Add("开通日期");
            }

            titleinfo.Add("安装位置");
            titleinfo.Add("备注信息");

            if (state)
            {
                titleinfo.Add("所属分组");
            }
            

            var lstt = new ObservableCollection<PartolItemViewModel>();
            Dictionary<int, string> tmpdir = new Dictionary<int, string>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
            {
                //if (t.Value.LstTml.Count == 0) continue;
                foreach (var g in t.Value.LstTml)
                {
                    if (tmpdir.ContainsKey(g)) continue;
                    tmpdir.Add(g, t.Value.GroupName + "-" + t.Key);
                }
            }

            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems )
            {
                if (t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;



                #region 巡测数据加载

                var ttt = new PartolItemViewModel() {RtuId = t.Key, PhysicalId = t.Value.RtuPhyId};
                ttt.RtuName = t.Value.RtuName;
                ttt.RequestNewDataTime = "--";
                ttt.ReceiveNewDataTime = "--";
                ttt.TimeSpan = "--";
                ttt.RtuState = t.Value.RtuStateCode == 2 ? "使用" : t.Value.RtuStateCode == 1 ? "停运" : "不用";
                if (tmpdir.ContainsKey(t.Key)) ttt.GrpName = tmpdir[t.Key];
//                else ttt.GrpName = "----";
                else ttt.GrpName = Get_Special_GrpName(t.Key);

                var tmp = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.Key);
                if (tmp != null && tmp.RtuNewData != null)
                {
                    ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                    ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                    ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                    ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                    ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                    ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                    ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                    ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;
                    ttt.ErrorCount = tmp.ErrorCount;
                }

                lstt.Add(ttt);

                #endregion

            }

            var ZcInfoR = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();
            Dictionary<int, LampInfo> ZcInfo = new Dictionary<int, LampInfo>();
            foreach (var t in ZcInfoR)
            {
                if (!ZcInfo.ContainsKey(t.Value.RtuId))
                    ZcInfo.Add(t.Value.RtuId, t.Value);
            }

            int index = 1;
            var lst = (from t in lstt orderby t.PhysicalId select t).ToList();
            foreach (var t in lst)
            {   
                var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);
                if (tmp1 == null || tmp1.EquipmentType != WjParaBase.EquType.Rtu) continue;
                var tmp2 = tmp1 as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmp2 == null) continue;
                var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.RtuId);


                if (!state)
                {
                    if (run!=null && run.RtuNewData!=null &&run.RtuNewData.DateCreate.Date == DateTime.Now.Date)
                    {
                        continue;
                    }
                }

                var tmp = new List<object>();

                tmp.Add(index);
                index++;

                if (ZcInfo.ContainsKey(t.RtuId)) tmp.Add(ZcInfo[t.RtuId].Cqj);
                else tmp.Add("");

                if (state)
                {
                    //todo 所编号
                }
                else
                {
                    tmp.Add(t.PhysicalId);
                }

                tmp.Add(t.RtuName);


                var run1 = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.RtuId);
                if (run1 != null && run1.RtuNewData != null)
                {
                    tmp.Add(run1.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    tmp.Add("");
                }



                if (state)
                {
                    if (ZcInfo.ContainsKey(t.RtuId)) tmp.Add(ZcInfo[t.RtuId].Dygh);
                    else tmp.Add("");

                    if (run != null && run.RtuNewData != null)
                    {
                        if (t.RtuState == "使用")
                        {
                            var title = "";
                            if (run.RtuNewData.Alarms.ContainsKey(1) && run.RtuNewData.Alarms[1]) title += "停电";
                            else title += "供电";

                            if (run.RtuNewData.Alarms.ContainsKey(3) && run.RtuNewData.Alarms[3]) title += "停运中";
                            else title += "使用中 ";

                            tmp.Add(title);
                        }
                        else
                        {
                            tmp.Add("--");
                        }
                    }
                    else
                    {
                        tmp.Add("");
                    }

                    var runinfo = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.RtuId);
                    var loopinfo2 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);
                    var loopinfo = loopinfo2 as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    double power = 0;
                    if (loopinfo != null)
                    {
                        string tmpx = "";
                        if (runinfo != null && runinfo.RtuNewData != null &&
                            runinfo.RtuNewData.IsSwitchOutAttraction != null)
                        {
                            int intx = 1;
                            foreach (var f in runinfo.RtuNewData.IsSwitchOutAttraction)
                            {
                                if (f == true) tmpx = tmpx + "K" + intx + "开";
                                else tmpx = tmpx + "K" + intx + "关";

                                if (intx == 8) tmpx = tmpx + ",";
                                intx++;
                            }
                            if (intx <= 8)
                            {
                                for (int i = intx; i <= 8; i++)
                                {
                                    tmpx = tmpx + "K" + i + "--";
                                    if (i != 8) tmpx = tmpx + ",";
                                }
                            }
                        }
                        else
                        {
                            tmpx = "K1--,K2--,K3--,K4--,K5--,K6--,K7--,K8--";
                        }
                        tmp.Add(tmpx);
                    }
                    else
                    {
                        tmp.Add("K1--,K2--,K3--,K4--,K5--,K6--,K7--,K8--");
                    }

                    var gggg =
                        (from gg in tmp2.WjLoops.Values orderby gg.LoopId select gg).
                            ToList();
                    var sout = new Dictionary<int, int>();

                    foreach (var g in gggg)
                    {
                        if (!sout.ContainsKey(g.SwitchOutputId)) sout.Add(g.SwitchOutputId, 0);
                        sout[g.SwitchOutputId] = sout[g.SwitchOutputId] + 1;
                    }

                    int meng = 0;
                    if (sout.ContainsKey(0))
                    {
                        meng = sout[0];
                    }

                    if (run != null && run.RtuNewData != null && run.RtuNewData.LstNewLoopsData != null && run.RtuNewData.LstNewLoopsData.Count >= meng)
                    {
                        var lstzs = (from ttf in run.RtuNewData.LstNewLoopsData orderby ttf.LoopId descending select ttf).ToList();
                        string tmpzzz = "";
                        for (int i = 0; i < meng; i++)
                        {
                            if (lstzs[i].LoopName.Contains("门"))
                            {
                                tmpzzz = "关闭";
                                if (!lstzs[i].BolSwitchInState)
                                {
                                    tmpzzz = "打开";
                                    break;
                                }
                            }
                        }

                        tmp.Add(tmpzzz);
                    }
                    else
                    {
                        tmp.Add("");
                    }

                    var U = "A相:" + t.RtuVoltageA.ToString("f2") + ",B相:" + t.RtuVoltageB.ToString("f2") + ",C相:" + t.RtuVoltageC.ToString("f2");
                    tmp.Add(U);
                    var I = "A相:" + t.RtuCurrentSumA.ToString("f2") + ",B相:" + t.RtuCurrentSumB.ToString("f2") + ",C相:" + t.RtuCurrentSumC.ToString("f2");
                    tmp.Add(I);
                    tmp.Add(string.Format("{0:d}", new DateTime(tmp2.DateCreate)));
                }

                tmp.Add(tmp2.RtuInstallAddr);
                tmp.Add(tmp2.RtuRemark);

                if (state)
                {
                    tmp.Add(t.GrpName);
                }
                

                writeinfo.Add(tmp);

               

            }

            var tmpxx = new List<object>();
            if (state)
            {
                tmpxx.Add(DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + "终端状态表");
            }
            else
            {
                tmpxx.Add(DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + "最新数据不是今天的箱变");
            }

            writeinfo.Add(tmpxx);
            
                //Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);

                return new Tuple<List<object>, List<List<object>>>(titleinfo, writeinfo);
        }

        private void WriteDataRunning()
        {
           
            var writeinfo = new List<List<object>>();
            var titleinfo = new List<object>();

            titleinfo.Add("物理地址");
            titleinfo.Add("逻辑地址");
            titleinfo.Add("终端名称");
            titleinfo.Add("归属分组");
            titleinfo.Add("使用状态");
            titleinfo.Add("在线");
            titleinfo.Add("当前故障数");
            titleinfo.Add("最新数据时间");
            titleinfo.Add("报警数量");
            //二期新增参数
            

            titleinfo.Add("K1状态");
            titleinfo.Add("K2状态");
            titleinfo.Add("K3状态");
            titleinfo.Add("K4状态");
            titleinfo.Add("K5状态");
            titleinfo.Add("K6状态");
            titleinfo.Add("K7状态");
            titleinfo.Add("K8状态");
            
            titleinfo.Add("A相电压");
            titleinfo.Add("B相电压");
            titleinfo.Add("C相电压");
            titleinfo.Add("A相电流");
            titleinfo.Add("B相电流");
            titleinfo.Add("C相电流");
            titleinfo.Add("总电流");
            titleinfo.Add("总功率");
            for (int i = 1; i < 49; i++)
            {
                titleinfo.Add("回路" + i);
                titleinfo.Add(i + "电压");
                titleinfo.Add(i + "电流");
                titleinfo.Add(i + "功率");
                titleinfo.Add(i + "功率因数");
                titleinfo.Add(i + "亮灯率");
            }

            var lst = (from t in MeasurePatrolData orderby t.PhysicalId select t).ToList();
            foreach (var t in lst)
            {
                var tmp = new List<object>();

                tmp.Add(t.PhysicalId);
                tmp.Add(t.RtuId);
                tmp.Add(t.RtuName);
                tmp.Add(t.GrpName);
                tmp.Add(t.RtuState);
                tmp.Add(t.OnLine.Contains("-") ? "--" : "√");
                tmp.Add(t.ErrorCount);
                tmp.Add(t.ReceiveNewDataTime);
                tmp.Add(t.AlarmCount);

                tmp.Add(t.SwitchOutState.Count > 0 ? t.SwitchOutState[0].IsSelected ? "开" : "--" : "--");
                
                tmp.Add(t.SwitchOutState.Count > 1 ? t.SwitchOutState[1].IsSelected ? "开" : "--" : "--");
                tmp.Add(t.SwitchOutState.Count > 2 ? t.SwitchOutState[2].IsSelected ? "开" : "--" : "--");
                tmp.Add(t.SwitchOutState.Count > 3 ? t.SwitchOutState[3].IsSelected ? "开" : "--" : "--");
                tmp.Add(t.SwitchOutState.Count > 4 ? t.SwitchOutState[4].IsSelected ? "开" : "--" : "--");
                tmp.Add(t.SwitchOutState.Count > 5 ? t.SwitchOutState[5].IsSelected ? "开" : "--" : "--");
                tmp.Add(t.SwitchOutState.Count > 6 ? t.SwitchOutState[6].IsSelected ? "开" : "--" : "--");
                tmp.Add(t.SwitchOutState.Count > 7 ? t.SwitchOutState[7].IsSelected ? "开" : "--" : "--");


                tmp.Add(t.RtuVoltageA.ToString("f2"));
                tmp.Add(t.RtuVoltageB.ToString("f2"));
                tmp.Add(t.RtuVoltageC.ToString("f2"));
                tmp.Add(t.RtuCurrentSumA.ToString("f2"));
                tmp.Add(t.RtuCurrentSumB.ToString("f2"));
                tmp.Add(t.RtuCurrentSumC.ToString("f2"));
                tmp.Add((t.RtuCurrentSumC + t.RtuCurrentSumA + t.RtuCurrentSumB).ToString("f2"));


                var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold .GetRunInfo( t.RtuId);
                if (tmp1 == null || tmp1 .RtuNewData ==null )
                {
                    tmp.Add("");
                    for (int i = 1; i < 49; i++)
                    {
                        tmp.Add("");
                    }
                }
                else
                {
                    double   dp = 0;
                    var lststr = new Dictionary<int, Tuple< string ,string ,string ,string,string >>();
                    foreach (var g in tmp1.RtuNewData .LstNewLoopsData)
                    {
                        var tus = new Tuple<string, string, string, string, string>(g.BolSwitchInState ? "打开" : "关闭", "--", "--", "--", "--");
                        if (g.IsLoop)
                        {
                            tus = new Tuple<string, string, string, string, string>(g.BolSwitchInState
                                                                                ? "吸合"
                                                                                : "断开", g.V.ToString("f2"), g.A.ToString( "f2"),
                                                                            g.Power.ToString("f2"),g.PowerFactor .ToString( "f2"));
                        }
                        //var strings = g.IsLoop
                        //                  ? (g.BolSwitchInState
                        //                         ? "吸合"
                        //                         : "断开" + "-" + g.V.ToString("f2") + "-" + g.A + "-" + g.Power)
                        //                  : (g.BolSwitchInState ? "开" : "关");
                        if (lststr.ContainsKey(g.LoopId)) continue;
                        lststr.Add(g.LoopId, tus);
                        dp += g.Power;
                    }
                    tmp.Add(dp.ToString("f2"));
                    for (int i = 1; i < 49; i++)
                    {
                        if (lststr.ContainsKey(i))
                        {
                            tmp.Add(lststr[i].Item1 );
                            tmp.Add(lststr[i].Item2);
                            tmp.Add(lststr[i].Item3);
                            tmp.Add(lststr[i].Item4);
                            tmp.Add(lststr[i].Item5);
                        }
                        else
                        {
                            tmp.Add("");
                            tmp.Add("");
                            tmp.Add("");
                            tmp.Add("");
                            tmp.Add("");
                        }
                    }

                }



                writeinfo.Add(tmp);
            }

            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);
        
   
        }

        private Tuple<List<object>, List<List<object>>> WriteDataPowerStaticChangsha(bool state)
        {
            var writeinfo = new List<List<object>>();
            var titleinfo = new List<object>();

            titleinfo.Add("序号");


            titleinfo.Add("地址");
            titleinfo.Add("终端名称");
            titleinfo.Add("城区局");
            titleinfo.Add("电源杆号");
            titleinfo.Add("最新数据时间"); 
            

            titleinfo.Add("A相电流");
            titleinfo.Add("B相电流");
            titleinfo.Add("C相电流");

            titleinfo.Add("A相总功率");
            titleinfo.Add("B相总功率");
            titleinfo.Add("C相总功率");

            titleinfo.Add("A相有功功率");
            titleinfo.Add("B相有功功率");
            titleinfo.Add("C相有功功率");

            titleinfo.Add("总功率");

            titleinfo.Add("所属分组");

            var lstt = new ObservableCollection<PartolItemViewModel>();
            Dictionary<int, string> tmpdir = new Dictionary<int, string>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
            {
                //if (t.Value.LstTml.Count == 0) continue;
                foreach (var g in t.Value.LstTml)
                {
                    if (tmpdir.ContainsKey(g)) continue;
                    tmpdir.Add(g, t.Value.GroupName + "-" + t.Key);
                }
            }

            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;



                #region 巡测数据加载

                var ttt = new PartolItemViewModel() { RtuId = t.Key, PhysicalId = t.Value.RtuPhyId };
                ttt.RtuName = t.Value.RtuName;
                ttt.RequestNewDataTime = "--";
                ttt.ReceiveNewDataTime = "--";
                ttt.TimeSpan = "--";
                ttt.RtuState = t.Value.RtuStateCode == 2 ? "使用" : t.Value.RtuStateCode == 1 ? "停运" : "不用";
                if (tmpdir.ContainsKey(t.Key)) ttt.GrpName = tmpdir[t.Key];
//                else ttt.GrpName = "----";
                else ttt.GrpName = Get_Special_GrpName(t.Key);

                var tmp = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.Key);
                if (tmp != null && tmp.RtuNewData != null)
                {
                    ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                    ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                    ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                    ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                    ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                    ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                    ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                    ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;
                    ttt.ErrorCount = tmp.ErrorCount;
                }

                lstt.Add(ttt);

                #endregion

            }

            var ZcInfoR = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();
            Dictionary<int, LampInfo> ZcInfo = new Dictionary<int, LampInfo>();
            foreach (var t in ZcInfoR)
            {
                if (!ZcInfo.ContainsKey(t.Value.RtuId))
                    ZcInfo.Add(t.Value.RtuId, t.Value);
            }

            bool flag = state;

            int index = 1;
            var lst = (from t in lstt orderby t.PhysicalId select t).ToList();
            foreach (var t in lst)
            {
                var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);
                if (tmp1 == null || tmp1.EquipmentType != WjParaBase.EquType.Rtu) continue;
                var tmp2 = tmp1 as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmp2 == null) continue;
                var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.RtuId);


                //if (!state)
                //{
                //    if (run != null && run.RtuNewData != null && run.RtuNewData.DateCreate.Date == DateTime.Now.Date)
                //    {
                //        continue;
                //    }
                //}

                var tmp = new List<object>();

                flag = state;

                if (state == false)
                {
                    if(t.RtuName.Substring(0, 1) == "*")
                    {
                        flag = true;
                    }
                }

                if (flag == true)
                {
                    tmp.Add(index);
                    index++;



                    tmp.Add(t.PhysicalId);
                    tmp.Add(t.RtuName);

                    if (ZcInfo.ContainsKey(t.RtuId)) tmp.Add(ZcInfo[t.RtuId].Cqj);
                    else tmp.Add("");

                    if (ZcInfo.ContainsKey(t.RtuId)) tmp.Add(ZcInfo[t.RtuId].Dygh);
                    else tmp.Add("");

                    var run1 = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.RtuId);
                    if (run1 != null && run1.RtuNewData != null)
                    {
                        tmp.Add(run1.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        tmp.Add("");
                    }








                    tmp.Add(t.RtuCurrentSumA.ToString("f2"));
                    tmp.Add(t.RtuCurrentSumB.ToString("f2"));
                    tmp.Add(t.RtuCurrentSumC.ToString("f2"));

                    

                    double powerA = (t.RtuCurrentSumA*t.RtuVoltageA)/1000;
                    double powerB = (t.RtuCurrentSumB*t.RtuVoltageB)/1000;
                    double powerC = (t.RtuCurrentSumC*t.RtuVoltageC)/1000;

                    tmp.Add(powerA.ToString("f2"));
                    tmp.Add(powerB.ToString("f2"));
                    tmp.Add(powerC.ToString("f2"));

                    double youPowerA = 0;
                    double youPowerB = 0;
                    double youPowerC = 0;

                    if (run != null)
                    {
                        if(run.RtuNewData != null)
                        {
                            if (run.RtuNewData.LstNewLoopsData != null)
                            {
                                if(tmp2 != null)
                                {
                                    if (tmp2.WjLoops != null)
                                    {
                                        int x = 1;


                                        foreach (var loop in run.RtuNewData.LstNewLoopsData)
                                        {
                                            if (tmp2.WjLoops[x].VoltagePhaseCode == EnumVoltagePhase.Aphase)
                                            {
                                                youPowerA += (loop.V*loop.A)/1000;
                                            }
                                            else if (tmp2.WjLoops[x].VoltagePhaseCode == EnumVoltagePhase.Bphase)
                                            {
                                                youPowerB += (loop.V*loop.A)/1000;
                                            }
                                            else if (tmp2.WjLoops[x].VoltagePhaseCode == EnumVoltagePhase.Cphase)
                                            {
                                                youPowerC += (loop.V*loop.A)/1000;
                                            }

                                            x++;
                                        }
                                    }
                                }
                            }
                        }
                    }


                    tmp.Add(youPowerA.ToString("f2"));
                    tmp.Add(youPowerB.ToString("f2"));
                    tmp.Add(youPowerC.ToString("f2"));

                    tmp.Add((powerA + powerB + powerC).ToString("f2"));

                    tmp.Add(t.GrpName);

                    writeinfo.Add(tmp);
                }


            }

            var tmpxx = new List<object>();
            if (state)
            {
                tmpxx.Add(DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + "终端总电流总功率统计表");
            }
            else
            {
                tmpxx.Add(DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + "终端总电流总功率统计表-仅箱变");
            }

            writeinfo.Add(tmpxx);

            //Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);

            return new Tuple<List<object>, List<List<object>>>(titleinfo, writeinfo);
        }

        private Tuple<List<object>, List<List<object>>> WriteDataVAChangsha(bool state)
        {
            var writeinfo = new List<List<object>>();
            var titleinfo = new List<object>();

            titleinfo.Add("序号");
            titleinfo.Add("地址");

            titleinfo.Add("终端名称");

            for(int i = 1 ; i <= 56; i++)
            {
                titleinfo.Add(i);
            }

            var lstt = new ObservableCollection<PartolItemViewModel>();
            Dictionary<int, string> tmpdir = new Dictionary<int, string>();
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.InfoGroups)
            {
                //if (t.Value.LstTml.Count == 0) continue;
                foreach (var g in t.Value.LstTml)
                {
                    if (tmpdir.ContainsKey(g)) continue;
                    tmpdir.Add(g, t.Value.GroupName + "-" + t.Key);
                }
            }

            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.EquipmentType != WjParaBase.EquType.Rtu) continue;



                #region 巡测数据加载

                var ttt = new PartolItemViewModel() { RtuId = t.Key, PhysicalId = t.Value.RtuPhyId };
                ttt.RtuName = t.Value.RtuName;
                ttt.RequestNewDataTime = "--";
                ttt.ReceiveNewDataTime = "--";
                ttt.TimeSpan = "--";
                ttt.RtuState = t.Value.RtuStateCode == 2 ? "使用" : t.Value.RtuStateCode == 1 ? "停运" : "不用";
                if (tmpdir.ContainsKey(t.Key)) ttt.GrpName = tmpdir[t.Key];
//                else ttt.GrpName = "----";
                else ttt.GrpName = Get_Special_GrpName(t.Key);

                var tmp = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.Key);
                if (tmp != null && tmp.RtuNewData != null)
                {
                    ttt.ReceiveNewDataTime = tmp.RtuNewData.DateCreate.ToString("yyyy-MM-dd HH:mm:ss") + "";
                    ttt.SetSwitchOutState(tmp.RtuNewData.IsSwitchOutAttraction);
                    ttt.RtuCurrentSumA = tmp.RtuNewData.RtuCurrentSumA;
                    ttt.RtuCurrentSumB = tmp.RtuNewData.RtuCurrentSumB;
                    ttt.RtuCurrentSumC = tmp.RtuNewData.RtuCurrentSumC;
                    ttt.RtuVoltageA = tmp.RtuNewData.RtuVoltageA;
                    ttt.RtuVoltageB = tmp.RtuNewData.RtuVoltageB;
                    ttt.RtuVoltageC = tmp.RtuNewData.RtuVoltageC;
                    ttt.ErrorCount = tmp.ErrorCount;
                }

                lstt.Add(ttt);

                #endregion

            }

            var ZcInfoR = Wlst.Sr.AssetManageInfoHold.Services.LampInfoHold.GetData();
            Dictionary<int, LampInfo> ZcInfo = new Dictionary<int, LampInfo>();
            foreach (var t in ZcInfoR)
            {
                if (!ZcInfo.ContainsKey(t.Value.RtuId))
                    ZcInfo.Add(t.Value.RtuId, t.Value);
            }

            bool flag = state;

            int index = 1;
            var lst = (from t in lstt orderby t.PhysicalId select t).ToList();
            foreach (var t in lst)
            {
                var tmp1 = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(t.RtuId);
                if (tmp1 == null || tmp1.EquipmentType != WjParaBase.EquType.Rtu) continue;
                var tmp2 = tmp1 as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (tmp2 == null) continue;
                var run = Wlst.Sr.EquipmentInfoHolding.Services.RunningInfoHold.GetRunInfo(t.RtuId);



                //if (!state)
                //{
                //    if (run != null && run.RtuNewData != null && run.RtuNewData.DateCreate.Date == DateTime.Now.Date)
                //    {
                //        continue;
                //    }
                //}

                var tmp = new List<object>();

                flag = state;

                if (state == false)
                {
                    if (t.RtuName.Substring(0, 1) == "*")
                    {
                        flag = true;
                    }
                }

                if (flag == true)
                {
                    tmp.Add(index);
                    index++;

                    tmp.Add(t.PhysicalId);

                    tmp.Add(t.RtuName);

                    bool wFlag = false;

                    if (run != null)
                    {
                        if (run.RtuNewData != null)
                        {
                            if (run.RtuNewData.LstNewLoopsData != null)
                            {
                                wFlag = true;

                                foreach (var loop in run.RtuNewData.LstNewLoopsData)
                                {
                                    if (loop.SwitchOutId != 0)
                                    {
                                        tmp.Add((loop.V).ToString("f2"));
                                    }
                                }
                            }
                        }
                    }

                    if (wFlag == false)
                    {
                        if (tmp2 != null)
                        {
                            if (tmp2.WjLoops != null)
                            {
                                foreach (var loop in tmp2.WjLoops)
                                {
                                    if (loop.Value.SwitchOutputId != 0)
                                    {
                                        tmp.Add(0);
                                    }
                                }
                            }
                        }
                    }




                    writeinfo.Add(tmp);

                    tmp = new List<object>();

                    tmp.Add(string.Empty);
                    tmp.Add(string.Empty);
                    tmp.Add(string.Empty);

                    wFlag = false;

                    if (run != null)
                    {
                        if (run.RtuNewData != null)
                        {
                            if (run.RtuNewData.LstNewLoopsData != null)
                            {
                                wFlag = true;

                                foreach (var loop in run.RtuNewData.LstNewLoopsData)
                                {
                                    if (loop.SwitchOutId != 0)
                                    {
                                        tmp.Add((loop.A).ToString("f2"));
                                    }
                                }
                            }
                        }


                    }

                    if (wFlag == false)
                    {
                        if (tmp2 != null)
                        {
                            if (tmp2.WjLoops != null)
                            {
                                foreach (var loop in tmp2.WjLoops)
                                {
                                    if (loop.Value.SwitchOutputId != 0)
                                    {
                                        tmp.Add(0);
                                    }
                                }
                            }
                        }
                    }

                    writeinfo.Add(tmp);
                }


            }

            var tmpxx = new List<object>();
            if (state)
            {
                tmpxx.Add(DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + "终端电压电流表");
            }
            else
            {
                tmpxx.Add(DateTime.Now.ToString("yyyy年MM月dd日HH时mm分") + "终端电压电流表-仅箱变");
            }

            writeinfo.Add(tmpxx);

            //Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(titleinfo, writeinfo);

            return new Tuple<List<object>, List<List<object>>>(titleinfo, writeinfo);
        }

        //lvf 2019年1月16日10:14:57  载入 动态备注 才采取httpget方式
        private void LoadRtuRemarkXml()
        {


            var info = Elysium.ThemesSet.Common.ReadSave.Read("LocalRtuRemarks", "SystemXmlConfig");
            if (info.Count == 0) return;
            Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks.Clear();
            for (int i = 0; i < 8; i++)
            {
                if (info.ContainsKey(i + ""))
                {
                    var remarkName = "";

                    remarkName = info[i + ""];
                    if (string.IsNullOrEmpty(remarkName.Trim())) continue;

                    Wlst.Sr.EquipmentInfoHolding.Services.Others.LocalRtuRemarks.Add(i, remarkName);


                }
            }

        }
        private Dictionary<int, List<string>> _dicRemarks = new Dictionary<int, List<string>>(); 

        public void RequestHttpData()
        {

            _dicRemarks.Clear();
            //请求数据协议格式，详见Sr\wlst.sr.iif\GoogleBuf\proto
            var nt = new Wlst.iif.EquimentRemarkRead();
            nt.RtuId = new List<int>();

            //序列化，请求数据结构
            var base64data = System.Convert.ToBase64String(EquimentRemarkRead.SerializeToBytes(nt));

            //http get
            var url = "http://" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverIpAddr + ":" + Wlst.Sr.EquipmentInfoHolding.Services.Others.SeverHttpPort + "/mims/get10010";//"http://10.3.9.8:18080/mims/get10010"
            var data = wlst.sr.iif.HttpGetPost.HttpGet(url, "?pb2=" + base64data);
            if (data == null) return;
            // 反序列化get到的数据
            var databk = Wlst.iif.EquimentRemarkReturn.Deserialize(System.Convert.FromBase64String(data));
            if (databk == null || databk.EquRemark == null) return;

            foreach (var g in databk.EquRemark)
            {
                var lst = new List<string>();
                lst.Add(g.Remark1);
                lst.Add(g.Remark2);
                lst.Add(g.Remark3);
                lst.Add(g.Remark4);
                lst.Add(g.Remark5);
                lst.Add(g.Remark6);
                lst.Add(g.Remark7);
                _dicRemarks.Add(g.RtuId,lst);
            }

        }

        
    }
}
