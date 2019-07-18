using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;


using Microsoft.Win32;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.WJ3005Module.ZDataQuery.DailyDataQuery.Services;
using Wlst.iif;
using Wlst.mobile;

namespace Wlst.Ux.WJ3005Module.ZDataQuery.DailyDataQuery.ViewModel
{
    [Export(typeof (IIEquipmentDailyDataQueryViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class EquipmentDailyDataQueryViewModel :
        Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, IIEquipmentDailyDataQueryViewModel
    {
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

        public EquipmentDailyDataQueryViewModel()
        {
            this.InitEvent();
            this.InitAction();
        }


        public void NavOnLoad(params object[] parsObjects)
        {
            Remind = "请通过点击左侧终端树选择终端进行终端数据查询,所有终端查询指定时刻，单个终端查询最多两个月";
            int rtuId = 0;
           // RtuName = "所有终端";
            this.DtEndTime = DateTime.Now;
            this.DtStartTime = DateTime.Now.AddDays(-1);
            this.StTime = 0;
            this.EndTime = 1439;
            this.SndTimeName = "结束时间";
            this.IsShowSumPower = Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 8, false);

            var areas = new List<int>();
            foreach (var f in Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
            {
                if (areas.Contains(f) == false && f != -1 ) areas.Add(f);
            }
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                {
                    if (areas.Contains(f) == false && f != -1) areas.Add(f);
                }
            }

            if (areas.Count>1) IsAreaOverOne = true;
            else IsAreaOverOne = false;

            _thisViewActive = true;
            IsLocked = false;
            IsAllRtu = false;
            IsCompare = false;
            OneTimeEnable = true;
            CompareEnable = true;
            DeviationValue = 0;
            CompCheck = Visibility.Collapsed;

            IsGjVisi = Wlst.Sr.EquipmentInfoHolding.Services.Others.HsdataQueryShGjOp
                           ? Visibility.Visible
                           : Visibility.Collapsed;

            if (IsGjVisi == Visibility.Visible) this.IsInView = true;
            else this.IsInView = false;

            try
            {
                rtuId = Convert.ToInt32(parsObjects[0]);
            }
            catch (Exception ex)
            {

            }
            if (rtuId > 0)
            {

                this.RtuId = rtuId;
                this.Query();
                QueryLoop();
            }


            //   Remind = "请通过点击左侧终端树选择终端进行终端数据查询...";

            Item.Add(new LoopDataInfo()
                         {
                             DtGetDataTime = DtStartTime,
                             Current = 0
                         });
        }


        //public void RequestHttpData(int index, int rtuid, long dtstart, long dtend)
        //{
        //    //请求数据协议格式，详见Sr\wlst.sr.iif\GoogleBuf\proto
        //    var nt = new Wlst.iif.GetRtusHisdata();
        //    //C#  与  java时间基准不同，需要转换
        //    nt.DateEnd = wlst.sr.iif.UtcTime.GetUtcTime(DateTime.Now);
        //    nt.DateStart = wlst.sr.iif.UtcTime.GetUtcTime(DateTime.Now.AddDays(-60));

        //    nt.ItemsRtu = new List<int>();
        //    nt.ItemsRtu.Add(1000002);
        //    nt.ItemsRtu.Add(1000006);
        //    //请求哪一页  >1
        //    nt.Head = new Head()
        //                  {
        //                      PagingIdx = index,

        //                  };
        //    //序列化，请求数据结构
        //    var base64data = System.Convert.ToBase64String(GetRtusHisdata.SerializeToBytes(nt));

        //    //http get
        //    var data = wlst.sr.iif.HttpGetPost.HttpGet("http://192.168.51.243:18080/mims/get1021", "?pb2=" + base64data);
        //    if (data == null) return;
        //    // 反序列化get到的数据
        //    var databk = Wlst.iif.RtusMeasureInfo.Deserialize(System.Convert.FromBase64String(data));
        //    if (databk == null) return;
        //    //ifst==0 异常，异常原因为 ifmsg
        //    if (databk.Head.IfSt == 0)
        //    {
        //        var msg = databk.Head.IfMsg;
        //    }

        //    ////数据解析
        //    //PageIndex = databk.Head.PagingIdx;
        //    //PageCount = databk.Head.PagingTotal;
        //    //PagingRecordTotal = databk.Head.PagingRecordTotal;
        //    //PagingNum = databk.Head.PagingNum;

        //    foreach (var f in databk.ItemsRtu)
        //    {
        //        //todo  lvf 2018年11月19日16:25:29
        //    }


        //}

   


        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            Records = new ObservableCollection<TmlNewDataViewModelExtend>();
            ExportVisi = Visibility.Collapsed;
            IsCheckedLoop = false;
            ItemCount = 0;
            PageTotal = "";
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
                return "巡测数据查询"; //I36N .Services.I36N .ConvertByCodingOne("11090001", "Setting");
                //return "Setting";
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


    }

    public partial class EquipmentDailyDataQueryViewModel
    {
        #region ICommand

        #region CmdQuery

        private DateTime _dtQuery;
        
        private ICommand _cmdQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdQuery == null)
                    _cmdQuery = new RelayCommand(Ex, CanEx, true);
                return _cmdQuery;
            }
        }


        private void Ex()
        {
            _dtQuery = DateTime.Now;
            Query();
            QueryLoop();
            _isOnExport = false;
            ExportVisi = Visibility.Visible;
            //Records.Clear();
        }

        private DateTime _dtPreQueryStartTime;
        private DateTime _dtPreQueryEndTime;

        private bool CanEx()
        {
            if (IsLocked) return false;
            var tStartTime = StTime <= 1440 && StTime >= 0
                                 ? new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, StTime/60,
                                                StTime%60, 0)
                                 : new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 0);
            var tEndTime = EndTime <= 1440 && EndTime >= 0
                               ? new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, EndTime/60, EndTime%60, 0)
                               : new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, 23, 59, 0);

            if (tStartTime > tEndTime) return false;
            //if (_dtPreQueryEndTime.Ticks != this.DtEndTime.Ticks || _dtPreQueryStartTime.Ticks != this.DtStartTime.Ticks)
            //{
            return DateTime.Now.Ticks - _dtQuery.Ticks > 30000000;
            //}
            return false;
        }

        #endregion

        #region  CmdLeft & CmdRight 

        private ICommand _cmdLeft;

        public ICommand CmdLeft
        {
            get
            {
                if (_cmdLeft == null)
                    _cmdLeft = new RelayCommand(ExLeft, CanEx, true);
                return _cmdLeft;
            }
        }

        private ICommand _cmdRight;

        public ICommand CmdRight
        {
            get
            {
                if (_cmdRight == null)
                    _cmdRight = new RelayCommand(ExRight, CanEx, true);
                return _cmdRight;
            }
        }
        private void ExLeft()
        {
            this.DtStartTime = this.DtStartTime.AddDays(-1);
            this.DtEndTime = this.DtEndTime.AddDays(-1);     
            Ex();
        }

        private void ExRight()
        {
            this.DtStartTime = this.DtStartTime.AddDays(1);
            this.DtEndTime = this.DtEndTime.AddDays(1);
            
            Ex();
        }


        #endregion




        #endregion

        #region Field

        private bool _thisViewActive = false;
        private bool _isOnExport = true;

        #endregion

        #region attri

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

        private Visibility _sthIsGjVisims;

        /// <summary>
        /// 查询起始时间 时分
        /// </summary>
        public Visibility IsGjVisi
        {
            get { return _sthIsGjVisims; }
            set
            {
                if (value != _sthIsGjVisims)
                {
                    _sthIsGjVisims = value;
                    this.RaisePropertyChanged(() => this.IsGjVisi);
                }
            }
        }

        #region Start&End Hour Min Second

        private int _sthms;

        /// <summary>
        /// 查询起始时间 时分
        /// </summary>
        public int StTime
        {
            get { return _sthms; }
            set
            {
                if (value != _sthms)
                {
                    _sthms = value;
                    this.RaisePropertyChanged(() => this.StTime);
                }
            }
        }

        private int _endHms;

        /// <summary>
        /// 查询结束时间 时分秒
        /// </summary>
        public int EndTime
        {
            get { return _endHms; }
            set
            {
                if (value != _endHms)
                {
                    _endHms = value;
                    this.RaisePropertyChanged(() => this.EndTime);
                }
            }
        }

        private string _sndtimename;

        /// <summary>
        /// 判断第二段时间名称为“结束时间”还是“比对时间”
        /// </summary>
        public string SndTimeName
        {
            get { return _sndtimename; }
            set
            {
                if (value != _sndtimename)
                {
                    _sndtimename = value;
                    this.RaisePropertyChanged(() => this.SndTimeName);
                }
            }
        }

        #endregion

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




        #region Checkbox

        private bool _lock;

        /// <summary>
        /// 锁定数据
        /// </summary>
        public bool IsLocked
        {
            get { return _lock; }
            set
            {
                if (_lock != value)
                {
                    _lock = value;
                    this.RaisePropertyChanged(() => this.IsLocked);
                }
            }
        }

        private bool _all;

        /// <summary>
        /// 查看所有终端
        /// </summary>
        public bool IsAllRtu
        {
            get { return _all; }
            set
            {
                if (_all != value)
                {
                    _all = value;
                    this.RaisePropertyChanged(() => this.IsAllRtu);
                    if (IsAllRtu == false) IsOneTime = false;
                    if (IsAllRtu && IsCompare == false) IsOneTime = true;
                }
            }
        }

        private bool _one;

        /// <summary>
        /// 查看某一时刻所有终端的值
        /// </summary>
        public bool IsOneTime
        {
            get { return _one; }
            set
            {
                if (_one != value)
                {
                    _one = value;
                    this.RaisePropertyChanged(() => this.IsOneTime);
                    if (IsOneTime == true)
                    {
                        IsOneCheck = Visibility.Collapsed;
                        IsAllRtu = true;
                        CompareEnable = false;
                    }
                    else
                    {
                        IsOneCheck = Visibility.Visible;
                        CompareEnable = true;
                    }
                }
            }
        }

        private bool _oneenable;

        /// <summary>
        /// 查看指定时刻按钮是否为可操作
        /// </summary>
        public bool OneTimeEnable
        {
            get { return _oneenable; }
            set
            {
                if (_oneenable != value)
                {
                    _oneenable = value;
                    this.RaisePropertyChanged(() => this.OneTimeEnable);


                }
            }
        }

        private bool _cmpenable;

        /// <summary>
        /// 查看比对数据按钮是否为可操作
        /// </summary>
        public bool CompareEnable
        {
            get { return _cmpenable; }
            set
            {
                if (_cmpenable != value)
                {
                    _cmpenable = value;
                    this.RaisePropertyChanged(() => this.CompareEnable);


                }
            }
        }

        private Visibility _oneCheck;

        /// <summary>
        /// 查看某一时刻所有终端的值 结束时间隐藏
        /// </summary>
        public Visibility IsOneCheck
        {
            get { return _oneCheck; }
            set
            {
                if (_oneCheck != value)
                {
                    _oneCheck = value;
                    this.RaisePropertyChanged(() => this.IsOneCheck);

                }
            }
        }

        private bool _compare;

        /// <summary>
        /// 查看比对数据
        /// </summary>
        public bool IsCompare
        {
            get { return _compare; }
            set
            {
                if (_compare != value)
                {
                    _compare = value;
                    this.RaisePropertyChanged(() => this.IsCompare);
                    if (IsCompare == true)
                    {
                        CompCheck = Visibility.Visible;
                        OneTimeEnable = false;
                        SndTimeName = " 比对时间";
                    }
                    else
                    {
                        CompCheck = Visibility.Collapsed;
                        OneTimeEnable = true;
                        SndTimeName = " 结束时间";
                    }
                }
            }
        }

        private bool _inview;

        /// <summary>
        /// 表格内显示
        /// </summary>
        public bool IsInView
        {
            get { return _inview; }
            set
            {
                if (_inview != value)
                {
                    _inview = value;
                    this.RaisePropertyChanged(() => this.IsInView);
                    if (_inview)
                        ViewHeight = 200;
                    else
                        ViewHeight = 0;
                }
            }
        }

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

        private bool _deviation;

        /// <summary>
        /// 电流偏差
        /// </summary>
        public bool IsDeviation
        {
            get { return _deviation; }
            set
            {
                if (_deviation != value)
                {
                    _deviation = value;
                    this.RaisePropertyChanged(() => this.IsDeviation);
                    if (IsDeviation == true) CompCheck = Visibility.Visible;
                    else CompCheck = Visibility.Collapsed;
                }
            }
        }

        private Visibility _compCheck;

        /// <summary>
        /// 比对数据是否勾选
        /// </summary>
        public Visibility CompCheck
        {
            get { return _compCheck; }
            set
            {
                if (_compCheck != value)
                {
                    _compCheck = value;
                    this.RaisePropertyChanged(() => this.CompCheck);
                }
            }
        }

        #endregion

        #region RtuId

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
                    //RtuName = "";
                    this.RaisePropertyChanged(() => this.RtuId);
                    if (_rtuId == 0) this.RtuName = "所有终端";

                    PhyId = value;
                    if (
                        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                             InfoItems.ContainsKey
                             (_rtuId))
                        return;
                    var tml =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                            [_rtuId];
                    this.RtuName = tml.RtuName;
                    PhyId = tml.RtuPhyId;

                }
            }
        }


        private int _phyId;

        /// <summary>
        /// 终端地址
        /// </summary>
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (value != _phyId)
                {
                    _phyId = value;
                    //RtuName = "";
                    this.RaisePropertyChanged(() => this.PhyId);

                }
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
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        #endregion

        #region DeviationValue

        private double _devValue;

        /// <summary>
        /// 电流偏差值
        /// </summary>
        [Range(0, 10000, ErrorMessage = "电流偏差不能为负数")]
        public double DeviationValue
        {
            get { return _devValue; }
            set
            {
                if (value != _devValue)
                {
                    _devValue = value;
                    this.RaisePropertyChanged(() => this.DeviationValue);
                }
            }
        }

        #endregion


        #region Records

        private ObservableCollection<TmlNewDataViewModelExtend> _record;

        public ObservableCollection<TmlNewDataViewModelExtend> Records
        {
            get
            {

                if (_record == null)
                    _record = new ObservableCollection<TmlNewDataViewModelExtend>();
                return _record;
            }
            set
            {
                if (_record == value) return;
                _record = value;
                this.RaisePropertyChanged(() => this.Records);
                var loopitem = new ObservableCollection<LoopDataInfo>();
                foreach (var f in value)
                {
                    //var loopInfoItem = new LoopDataInfo();
                    if (f == null || f.DataInfo == null || f.DataInfo.RtuTemperature < -50) continue;

                    var tmps =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            f.DataInfo.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;


                    var fff = new RtuNewDataInfo(f.DataInfo);
                    bool isShieldLoop = false;

                    foreach (var t in fff.LstNewLoopsData)
                    {
                        var loopname = "回路接触器" + t.LoopId;
                        var loop = 0;
                        var loopk = "";
                        if (tmps.WjLoops.ContainsKey(t.LoopId))
                        {
                            loopname = tmps.WjLoops[t.LoopId].LoopName;
                            loop = tmps.WjLoops[t.LoopId].SwitchOutputId;
                            isShieldLoop = tmps.WjLoops[t.LoopId].IsShieldLoop == 1; //0 不屏蔽  1 屏蔽  2 屏蔽并不显示
                            if (tmps.WjLoops[t.LoopId].CurrentRange == 0) isShieldLoop = true;
                            if (tmps.WjSwitchOuts.ContainsKey(loop))
                                loopk = tmps.WjSwitchOuts[loop].SwitchName;
                        }


                        var a = t.A.ToString("f2");
                        if (isShieldLoop && Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 3) == false)
                            a = "----";
                        var brightrate = isShieldLoop ? "----" : (t.BrightRate * 100).ToString("f2") + "%";

                        var v = t.V.ToString("f2");
                        if (isShieldLoop && Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 4) == false)
                            v = "----";
                        var power = isShieldLoop ? "----" : t.Power.ToString("f2");
                        var powerfactor = isShieldLoop ? "----" : t.PowerFactor.ToString("f2");
                        var switchinstate = t.BolSwitchInState ? "吸合" : "断开";
                        if (loopname.Contains("门")) switchinstate = t.BolSwitchInState ? "正常" : "打开";
                        if (loopname.Contains("检测器")) switchinstate = t.BolSwitchInState ? "正常" : "报警";
                        if (loopname.Contains("防盗")) switchinstate = t.BolSwitchInState ? "正常" : "报警";
                        //宁波 lvf 2018年4月13日09:35:16 带K的状态解析
                        if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum == 4 && loopname.Contains("K"))
                        {
                            string tmpssss = "";
                            bool alwayClose = tmps.WjLoops[t.LoopId].IsSwitchStateClose;

                            if (t.BolSwitchInState == false && alwayClose) tmpssss = "断开";
                            if (t.BolSwitchInState && alwayClose) tmpssss = "吸合";
                            if (t.BolSwitchInState == false && alwayClose == false) tmpssss = "吸合";
                            if (t.BolSwitchInState && alwayClose == false) tmpssss = "断开";
                            switchinstate = tmpssss;

                        }


                        if (loopk == "")
                        {
                            a = "";
                            brightrate = "";
                            v = "";
                            power = "";
                            powerfactor = "";
                        }

                        loopitem.Add(new LoopDataInfo()
                        {
                            A = a,
                            BrightRate = brightrate,
                            LoopId = t.LoopId,
                            LoopName = loopname,
                            V = v,
                            Power = power,
                            PowerFactor = powerfactor,
                            SwitchInState = switchinstate,
                            LoopK = loopk,
                            DtGetDataTime = f.DtGetDataTimeFormat
                        });
                        //loopitem.Add(loopInfoItem);
                    }

                }
                var xx = loopitem.OrderBy(x => x.LoopId).ThenBy(x => x.DtGetDataTime).ToList();
                RecordsLoops.Clear();
                foreach (var l in xx)
                {
                    RecordsLoops.Add(l);
                }

                //查询变化图形变化
                if (LoopName != null)
                {
                    var loopInfoItem = new ObservableCollection<LoopDataInfo>();
                    foreach (var f in Records)
                    {

                        if (f == null || f.DataInfo == null || f.DataInfo.RtuTemperature < -50) continue;

                        var tmps =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                                f.DataInfo.RtuId]
                            as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                        var fff = new RtuNewDataInfo(f.DataInfo);
                        double sumcurrent = fff.LstNewLoopsData.Aggregate<RtuNewDataLoopItem, double>(0, (current, t) => current + t.A);
                        foreach (var t in fff.LstNewLoopsData)
                        {
                            if (t.LoopName == LoopName.Name)
                            {
                                loopInfoItem.Add(new LoopDataInfo()
                                {
                                    Current = t.A,
                                    Voltage = t.V,
                                    SumCurrent = sumcurrent,
                                    DtGetDataTime =
                                        f.DtGetDataTimeFormat
                                });
                            }
                        }
                    }
                    RecordsLoop = loopInfoItem;
                    Application.Current.Dispatcher.Invoke(new Action(delegate
                    {
                        Item.Clear();
                    }));
                    if(RecordsLoop.Count<1)
                        Item.Add(new LoopDataInfo()
                                     {
                                         DtGetDataTime = DtStartTime,
                                         Current = 0
                                     });
                }
            }
        }


        //所有终端数据返回  缓存
        private ObservableCollection<TmlNewDataViewModelExtend> _recordtmp;

        public ObservableCollection<TmlNewDataViewModelExtend> RecordsTmp
        {
            get
            {

                if (_recordtmp == null)
                    _recordtmp = new ObservableCollection<TmlNewDataViewModelExtend>();
                return _recordtmp;
            }
            set
            {
                if (_recordtmp == value) return;
                _recordtmp = value;
                this.RaisePropertyChanged(() => this.RecordsTmp);
            }
        }

        private bool _isAreaOverOne;

        /// <summary>
        /// 区域可见
        /// </summary>
        public bool IsAreaOverOne
        {
            get { return _isAreaOverOne; }
            set
            {
                if (value != _isAreaOverOne)
                {
                    _isAreaOverOne = value;
                    this.RaisePropertyChanged(() => this.IsAreaOverOne);
                }
            }
        }


        #endregion

        private ObservableCollection<LoopDataInfo> _recordLoop;
        /// <summary>
        /// 回路信息
        /// </summary>
        public ObservableCollection<LoopDataInfo> RecordsLoop
        {
            get
            {

                if (_recordLoop == null)
                    _recordLoop = new ObservableCollection<LoopDataInfo>();
                return _recordLoop;
            }
            set
            {
                if (_recordLoop == value) return;
                _recordLoop = value;
                this.RaisePropertyChanged(() => this.RecordsLoop);
            }
        }

        private ObservableCollection<LoopDataInfo> _recordLoops;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<LoopDataInfo> RecordsLoops
        {
            get
            {

                if (_recordLoops == null)
                    _recordLoops = new ObservableCollection<LoopDataInfo>();
                return _recordLoops;
            }
            set
            {
                if (_recordLoops == value) return;
                _recordLoops = value;
                this.RaisePropertyChanged(() => this.RecordsLoops);
            }
        }

        private ObservableCollection<LoopDataInfo> _item;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<LoopDataInfo> Item
        {
            get
            {

                if (_item == null)
                    _item = new ObservableCollection<LoopDataInfo>();
                return _item;
            }
            set
            {
                if (_item == value) return;
                _item = value;
                this.RaisePropertyChanged(() => this.Item);
            }
        }

        #region CurrentSelectDataInfo

        private ObservableCollection<LoopDataInfo> _currentSelectDataInfo;

        public ObservableCollection<LoopDataInfo> CurrentSelectDataInfo
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

        #region CurrentSelectRecord

        private TmlNewDataViewModelExtend _currentSelectRecord;

        public TmlNewDataViewModelExtend CurrentSelectRecord
        {
            get { return _currentSelectRecord ?? (_currentSelectRecord = new TmlNewDataViewModelExtend()); }
            set
            {
                if (_currentSelectRecord != value)
                {
                    _currentSelectRecord = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectRecord);

                    if (_currentSelectRecord == null || _currentSelectRecord.DataInfo == null || _currentSelectRecord.DataInfo.RtuTemperature < -50)
                    { CurrentSelectDataInfo = new ObservableCollection<LoopDataInfo>(); return; }

                    var tmps =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            _currentSelectRecord.DataInfo.RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                    if (IsInView)
                    {
                        var fff = new RtuNewDataInfo(_currentSelectRecord.DataInfo);
                        CurrentSelectDataInfo = new ObservableCollection<LoopDataInfo>();
                        bool isShieldLoop = false;
                        foreach (var t in fff.LstNewLoopsData)
                        {
                            var loopname = "回路接触器" + t.LoopId;
                            var loop = 0;
                            var loopk = "";
                            if (tmps.WjLoops.ContainsKey(t.LoopId))
                            {
                                loopname = tmps.WjLoops[t.LoopId].LoopName;
                                loop = tmps.WjLoops[t.LoopId].SwitchOutputId;
                                isShieldLoop = tmps.WjLoops[t.LoopId].IsShieldLoop==1; //0 不屏蔽  1 屏蔽  2 屏蔽并不显示
                                if (tmps.WjLoops[t.LoopId].CurrentRange == 0) isShieldLoop = true;
                                if (tmps.WjSwitchOuts.ContainsKey(loop))
                                    loopk = tmps.WjSwitchOuts[loop].SwitchName;
                            }


                            var a = t.A.ToString("f2");
                            if (isShieldLoop && Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 3) == false) a = "----";
                            var brightrate = isShieldLoop ? "----" : (t.BrightRate * 100).ToString("f2") + "%";

                            var v = t.V.ToString("f2");
                            if (isShieldLoop && Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 4) == false) v = "----";
                            var power = isShieldLoop?"----":t.Power.ToString("f2");
                            var powerfactor = isShieldLoop ? "----" : t.PowerFactor.ToString("f2");
                            var switchinstate  = t.BolSwitchInState ? "吸合" : "断开"; 
                            if (loopname.Contains("门")) switchinstate = t.BolSwitchInState ? "正常" : "打开";
                            if (loopname.Contains("检测器")) switchinstate = t.BolSwitchInState ? "正常" : "报警";
                            if (loopname.Contains("防盗")) switchinstate = t.BolSwitchInState ? "正常" : "报警";
                            //宁波 lvf 2018年4月13日09:35:16 带K的状态解析
                            if (Wlst.Sr.EquipmentInfoHolding.Services.Others.CityNum == 4 && loopname.Contains("K"))
                            {
                                string tmpssss = "";
                                bool alwayClose = tmps.WjLoops[t.LoopId].IsSwitchStateClose;

                                if (t.BolSwitchInState == false && alwayClose) tmpssss = "断开";
                                if (t.BolSwitchInState && alwayClose) tmpssss = "吸合";
                                if (t.BolSwitchInState == false && alwayClose == false) tmpssss = "吸合";
                                if (t.BolSwitchInState && alwayClose == false) tmpssss = "断开";
                                switchinstate = tmpssss;
                                
                            }


                            if (loopk == "")
                            {
                                a = "";
                                brightrate = "";
                                v = "";
                                power = "";
                                powerfactor = "";
                            }

                            CurrentSelectDataInfo.Add(new LoopDataInfo()
                                                          {
                                                              A = a,
                                                              BrightRate = brightrate,
                                                              LoopId = t.LoopId,
                                                              LoopName = loopname,
                                                              V = v,
                                                              Power = power,
                                                              PowerFactor = powerfactor,
                                                              SwitchInState = switchinstate,
                                                              LoopK = loopk
                                                          });
                        }
                    }
                    //else
                    //{
                        var args = new PublishEventArgs
                                       {
                                           EventType = PublishEventType.Core,
                                           EventId =
                                               Sr.EquipmentInfoHolding.Services.EventIdAssign.
                                               RtuDataQueryDataInfoNeedShowInTab,
                                       };
                        args.AddParams(_currentSelectRecord.DataInfo);
                        EventPublish.PublishEvent(args);


                        //  if (WJ3005Module.ZNewData.TmlNewDataViewModel.ViewModel.NewDataViewModel.OnMeasureShowData)
                        EventPublish.PublishEvent(new PublishEventArgs() {EventType = "MainWindow.Measure.show"});
                    //}
                }
            }
        }

        #endregion

        private bool _isCheckedLoop;

        public bool IsCheckedLoop
        {
            get { return _isCheckedLoop; }
            set
            {
                if (value == _isCheckedLoop) return;
                _isCheckedLoop = value;
                this.RaisePropertyChanged(() => this.IsCheckedLoop);
                IsVisiTerminalInfo = value ? Visibility.Collapsed : Visibility.Visible;
                IsVisiLoopInfo = value ? Visibility.Visible : Visibility.Collapsed;
                PagerVisi = !value && count > 1 ? Visibility.Visible : Visibility.Collapsed;
                if (value == false)
                {
                    IsVisiForm = Visibility.Collapsed;
                    IsVisiGraphics = Visibility.Collapsed;
                }

            }
        }

        private Visibility _isVisiTerminalInfo = Visibility.Visible;

        public Visibility IsVisiTerminalInfo
        {
            get { return _isVisiTerminalInfo; }
            set
            {
                if (value == _isVisiTerminalInfo) return;
                _isVisiTerminalInfo = value;
                this.RaisePropertyChanged(() => this.IsVisiTerminalInfo);
            }
        }

        private Visibility _isVisiLoopInfo = Visibility.Collapsed;

        public Visibility IsVisiLoopInfo
        {
            get { return _isVisiLoopInfo; }
            set
            {
                if (value == _isVisiLoopInfo) return;
                _isVisiLoopInfo = value;
                this.RaisePropertyChanged(() => this.IsVisiLoopInfo);
                FormOrGraphics = value == Visibility.Visible ? 1 : 2;
                IsCurrent = value == Visibility.Visible;
                IsVoltage = value != Visibility.Visible;
            }
        }

        private Visibility _isVisiForm = Visibility.Collapsed;

        public Visibility IsVisiForm
        {
            get { return _isVisiForm; }
            set
            {
                if (value == _isVisiForm) return;
                _isVisiForm = value;
                this.RaisePropertyChanged(() => this.IsVisiForm);
            }
        }

        private Visibility _isVisiGraphics = Visibility.Collapsed;

        public Visibility IsVisiGraphics
        {
            get { return _isVisiGraphics; }
            set
            {
                if (value == _isVisiGraphics) return;
                _isVisiGraphics = value;
                this.RaisePropertyChanged(() => this.IsVisiGraphics);
            }
        }

        private int _formOrGraphics;
        /// <summary>
        /// 图形或表格
        /// </summary>
        public int FormOrGraphics
        {
            get { return _formOrGraphics; }
            set
            {
                if (_formOrGraphics != value)
                {
                    _formOrGraphics = value;
                    this.RaisePropertyChanged(() => this.FormOrGraphics);
                    if (FormOrGraphics == 1) FormOrGraphicsStr = "图形";
                    else if (FormOrGraphics == 2) FormOrGraphicsStr = "表格";
                    else FormOrGraphicsStr = "未知";
                    IsVisiGraphics = FormOrGraphics == 1 && IsCheckedLoop ? Visibility.Visible : Visibility.Collapsed;
                    IsVisiForm = FormOrGraphics == 2 && IsCheckedLoop ? Visibility.Visible : Visibility.Collapsed;
                    IsCheckedForm = FormOrGraphics == 1;
                }
            }
        }

        private string _formOrGraphicsStr;
        /// <summary>
        /// 
        /// </summary>
        public string FormOrGraphicsStr
        {
            get { return _formOrGraphicsStr; }
            set
            {
                if (_formOrGraphicsStr != value)
                {
                    _formOrGraphicsStr = value;
                    this.RaisePropertyChanged(() => this.FormOrGraphicsStr);
                }
            }
        }

        private bool _isCheckeForm;

        public bool IsCheckedForm
        {
            get { return _isCheckeForm; }
            set
            {
                if (value == _isCheckeForm) return;
                _isCheckeForm = value;
                this.RaisePropertyChanged(() => this.IsCheckedForm);

            }
        }

        private bool _isCurrent;

        public bool IsCurrent
        {
            get { return _isCurrent; }
            set
            {
                if (value == _isCurrent) return;
                _isCurrent = value;
                this.RaisePropertyChanged(() => this.IsCurrent);
                IsVisiCurrent = IsCurrent ? Visibility.Visible : Visibility.Collapsed;
                if (RecordsLoop.Count > 0)
                {
                    if (IsVoltage == false && IsCurrent == false && IsSumCurrent == false)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(delegate
                        {
                            Item.Clear();
                        }));
                        Item.Add(new LoopDataInfo()
                        {
                            DtGetDataTime = DtStartTime,
                            Current = 0
                        });
                    }
                    else Item.Clear();
                }
            }
        }

        private bool _isSumCurrent;

        public bool IsSumCurrent
        {
            get { return _isSumCurrent; }
            set
            {
                if (value == _isSumCurrent) return;
                _isSumCurrent = value;
                this.RaisePropertyChanged(() => this.IsSumCurrent);
                IsVisiSumCurrent = IsSumCurrent ? Visibility.Visible : Visibility.Collapsed;
                if (RecordsLoop.Count > 0)
                {
                    if (IsVoltage == false && IsCurrent == false && IsSumCurrent == false)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(delegate
                        {
                            Item.Clear();
                        }));
                        Item.Add(new LoopDataInfo()
                        {
                            DtGetDataTime = DtStartTime,
                            Current = 0
                        });
                    }
                    else Item.Clear();
                }
            }
        }

        private bool _isVoltage;

        public bool IsVoltage
        {
            get { return _isVoltage; }
            set
            {
                if (value == _isVoltage) return;
                _isVoltage = value;
                this.RaisePropertyChanged(() => this.IsVoltage);
                IsVisiVoltage = IsVoltage ? Visibility.Visible : Visibility.Collapsed;
                if (RecordsLoop.Count > 0)
                {
                    if (IsVoltage == false && IsCurrent == false && IsSumCurrent == false)
                    {
                        Application.Current.Dispatcher.Invoke(new Action(delegate
                                                                             {
                                                                                 Item.Clear();
                                                                             }));
                        Item.Add(new LoopDataInfo()
                                     {
                                         DtGetDataTime = DtStartTime,
                                         Current = 0
                                     });
                    }
                    else Item.Clear();
                }

            }
        }

        private Visibility _isVisiCurrent;

        public Visibility IsVisiCurrent
        {
            get { return _isVisiCurrent; }
            set
            {
                if (value == _isVisiCurrent) return;
                _isVisiCurrent = value;
                this.RaisePropertyChanged(() => this.IsVisiCurrent);
            }
        }

        private Visibility _isVisiVoltage = Visibility.Collapsed;

        public Visibility IsVisiVoltage
        {
            get { return _isVisiVoltage; }
            set
            {
                if (value == _isVisiVoltage) return;
                _isVisiVoltage = value;
                this.RaisePropertyChanged(() => this.IsVisiVoltage);
            }
        }

        private Visibility _isVisiSumCurrent = Visibility.Collapsed;

        public Visibility IsVisiSumCurrent
        {
            get { return _isVisiSumCurrent; }
            set
            {
                if (value == _isVisiSumCurrent) return;
                _isVisiSumCurrent = value;
                this.RaisePropertyChanged(() => this.IsVisiSumCurrent);
            }
        }

        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _loopItem;
        /// <summary>
        /// 回路列表
        /// </summary>
        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> LoopItem
        {
            get
            {
                if (_loopItem == null)
                {
                    _loopItem = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                }
                return _loopItem;
            }
            set
            {
                if (value == _loopItem) return;
                _loopItem = value;
                this.RaisePropertyChanged(() => LoopItem);
            }
        }

        private Wlst.Cr.CoreOne.Models.NameValueInt _loopName;

        /// <summary>
        /// 回路
        /// </summary>
        public Wlst.Cr.CoreOne.Models.NameValueInt LoopName
        {
            get { return _loopName; }
            set
            {
                if (value != _loopName)
                {
                    _loopName = value;
                    this.RaisePropertyChanged(() => this.LoopName);
                    RecordsLoop.Clear();
                    var loopInfoItem = new ObservableCollection<LoopDataInfo>();
                    foreach (var f in Records)
                    {

                        if (f == null || f.DataInfo == null || f.DataInfo.RtuTemperature < -50) continue;

                        var tmps =
                            Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                                f.DataInfo.RtuId]
                            as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;

                        var fff = new RtuNewDataInfo(f.DataInfo);
                        double sumcurrent = fff.LstNewLoopsData.Aggregate<RtuNewDataLoopItem, double>(0, (current, t) => current + t.A);
                        foreach (var t in fff.LstNewLoopsData)
                        {
                            if (value == null) return;
                            if (t.LoopName == value.Name)
                            {
                                loopInfoItem.Add(new LoopDataInfo()
                                {
                                    Current = t.A,
                                    SumCurrent = sumcurrent,
                                    Voltage = t.V,
                                    DtGetDataTime = f.DtGetDataTimeFormat
                                });
                            }
                        }
                    }
                    RecordsLoop = loopInfoItem;
                }
            }
        }

        private double _zoomNumber;
        /// <summary>
        /// 滚轮
        /// </summary>
        public double ZoomNumber
        {
            get { return _zoomNumber; }
            set
            {
                if (_zoomNumber != value)
                {
                    _zoomNumber = value;
                    this.RaisePropertyChanged(() => this.ZoomNumber);
                    TimeStep = Convert.ToInt32(ZoomNumber*(tEndTime - tStartTime).TotalHours/24) == 0
                                   ? 1
                                   : Convert.ToInt32(ZoomNumber*(tEndTime - tStartTime).TotalHours/24);
                }
            }
        }

        private int _timeStep;
        /// <summary>
        /// 时间间隔
        /// </summary>
        public int TimeStep
        {
            get { return _timeStep; }
            set
            {
                if (_timeStep != value)
                {
                    _timeStep = value;
                    this.RaisePropertyChanged(() => this.TimeStep);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 时间转化
        /// </summary>
        private Tuple<int, int> TimeTransfer(string time)
        {
            if (time != null)
            {
                var timeArray = time.Split(':');
                var hour = Convert.ToInt32(timeArray[0]);
                var minute = Convert.ToInt32(timeArray[1]);
                return new Tuple<int, int>(hour, minute);
            }
            return new Tuple<int, int>(0, 0);
            ;
        }

        /// <summary>
        /// 选中终端变化  提取数据
        /// </summary>
        /// <param name="rtuId"></param>
        private void SelectRtuIdChange(int rtuId)
        {
            if (rtuId < 1) return;
            //todo  request data
            if (rtuId != this.RtuId)
            {
                this.RtuId = rtuId;
                //Query();
            }
        }


        public DateTime tStartTime=new DateTime();
        public DateTime tEndTime = new DateTime();
        private void Query()
        {
            if (!GetCheckedInformation())
            {
                Remind = "请重新设置查询时间。";
                return;
            }
            this.Records.Clear();
            this.RecordsTmp.Clear();
            _dtPreQueryEndTime = this.DtEndTime;
            _dtPreQueryStartTime = this.DtStartTime;

            tStartTime = StTime <= 1440 && StTime >= 0
                                 ? new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, StTime/60,
                                                StTime%60, 0)
                                 : new DateTime(DtStartTime.Year, DtStartTime.Month, DtStartTime.Day, 0, 0, 0);
            tEndTime = EndTime <= 1440 && EndTime >= 0
                               ? new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, EndTime/60, EndTime%60, 0)
                               : new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, 23, 59, 0);

            if (tStartTime < tEndTime)
                ZoomNumber = 24 / (tEndTime - tStartTime).TotalHours;

            //Records.Clear();
            //this.Query(tStartTime, tEndTime, this.RtuId);
            PageIndex = 0;
            RequestHttpData(tStartTime, tEndTime, this.RtuId, 0, 0);
        }

        private void QueryLoop()
        {
            LoopItem.Clear();
            var tmps =
                        Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[
                            RtuId]
                        as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
            if (tmps == null) return;
            var tmp = tmps.WjLoops.OrderBy(x => x.Value.LoopId).ToList();
            var items = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
            foreach (var t in tmp)
            {
                if (t.Value.SwitchOutputId != 0)
                    items.Add(new NameValueInt() { Name = t.Value.LoopName });
            }
            LoopItem = items;
            if (LoopItem != null) LoopName = LoopItem[0];
        }

        #endregion


        #region ICommand

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
                lsttitle.Add("终端id");
                lsttitle.Add("终端名称");
                lsttitle.Add("采集时间");
                lsttitle.Add("A相电压");
                lsttitle.Add("B相电压");
                lsttitle.Add("C相电压");
                lsttitle.Add("A相电流");
                lsttitle.Add("B相电流");
                lsttitle.Add("C相电流");
                lsttitle.Add("K1状态");
                lsttitle.Add("K2状态");
                lsttitle.Add("K3状态");
                lsttitle.Add("K4状态");
                lsttitle.Add("K5状态");
                lsttitle.Add("K6状态");
                lsttitle.Add("K7状态");
                lsttitle.Add("K8状态");
                int loopcount = 0;
                if (Records.Count > 0)
                {
                    loopcount = Records[0].LstNewLoopsData.Count;
                    foreach (var g in Records[0].LstNewLoopsData)
                    {
                        lsttitle.Add("回路" + g.LoopId + "电压");
                        lsttitle.Add("回路" + g.LoopId + "电流");
                        lsttitle.Add("回路" + g.LoopId + "功率");
                    }
                }
                var lstobj = new List<List<object>>();

                foreach (var g in Records)
                {
                    var tmp = new List<object>();
                    
                    tmp.Add(g.Index);
                    tmp.Add(g.RtuId);
                    tmp.Add(g.RtuName);
                    tmp.Add(g.DtGetDataTime);
                    tmp.Add(g.RtuVoltageA);
                    tmp.Add(g.RtuVoltageB);
                    tmp.Add(g.RtuVoltageC);
                    tmp.Add(g.RtuCurrentSumA);
                    tmp.Add(g.RtuCurrentSumB);
                    tmp.Add(g.RtuCurrentSumC);
                    tmp.Add(g.LstIsSwitchOutAttraction.Count > 0
                                ? (g.LstIsSwitchOutAttraction[0].IsSelected ? "开灯" : "关灯")
                                : "--");
                    tmp.Add(g.LstIsSwitchOutAttraction.Count > 1
                                ? (g.LstIsSwitchOutAttraction[1].IsSelected ? "开灯" : "关灯")
                                : "--");
                    tmp.Add(g.LstIsSwitchOutAttraction.Count > 2
                                ? (g.LstIsSwitchOutAttraction[2].IsSelected ? "开灯" : "关灯")
                                : "--");
                    tmp.Add(g.LstIsSwitchOutAttraction.Count > 3
                                ? (g.LstIsSwitchOutAttraction[3].IsSelected ? "开灯" : "关灯")
                                : "--");
                    tmp.Add(g.LstIsSwitchOutAttraction.Count > 4
                                ? (g.LstIsSwitchOutAttraction[4].IsSelected ? "开灯" : "关灯")
                                : "--");
                    tmp.Add(g.LstIsSwitchOutAttraction.Count > 5
                                ? (g.LstIsSwitchOutAttraction[5].IsSelected ? "开灯" : "关灯")
                                : "--");
                    tmp.Add(g.LstIsSwitchOutAttraction.Count > 6
                                ? (g.LstIsSwitchOutAttraction[6].IsSelected ? "开灯" : "关灯")
                                : "--");
                    tmp.Add(g.LstIsSwitchOutAttraction.Count > 7
                                ? (g.LstIsSwitchOutAttraction[7].IsSelected ? "开灯" : "关灯")
                                : "--");

                    int indexx = 0;
                    foreach (var m in g.LstNewLoopsData)
                    {
                        if (loopcount <= indexx) continue;
                        indexx++;

                        tmp.Add(m.V);
                        tmp.Add(m.A);
                        tmp.Add(m.Power);
                    }

                    lstobj.Add(tmp);
                }
                Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRow(lsttitle, lstobj);
                lstobj = null;
                lsttitle = null;
            }
            catch (Exception e)
            {
                Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("导出巡测报表时报错:" + e);
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



        #region   http 分页

      
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
                    RequestHttpData(tStartTime, tEndTime, RtuId, value, 1);

                }
            }
        }

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

        //private void RequestHttpData(DateTime dtstarttime, DateTime dtendtime, int tml,int pageindx)
        //{

        //    //var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(tml );
        //    //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询：" + (tmp == null ? tml  + "" : tmp.RtuName) + " ...";



        //    var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_data; // .wlst_cnt_request_wj3090_measure_data 
        //    info.WstRtuData.DtStartTime = dtstarttime.Ticks;

        //    if (IsAllRtu == true) info.WstRtuData.RtuId = 0;
        //    else info.WstRtuData.RtuId = tml;

        //    _isLastQueryIsAll = IsAllRtu;
        //    _guidsThatLastUsed.Clear();

        //    if (IsOneTime == true) info.WstRtuData.DtEndTime = dtstarttime.AddHours(1).Ticks;
        //    else info.WstRtuData.DtEndTime = dtendtime.Ticks;

        //    if (IsCompare == true) info.WstRtuData.Op = 4;
        //    else if (IsOneTime == true) info.WstRtuData.Op = 3;
        //    else info.WstRtuData.Op = 2;

        //    if (_isLastQueryIsAll)
        //    {
        //        int maxRtuId = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys
        //                        where t < 1100000
        //                        orderby t descending
        //                        select t).ToList()[0];
        //        var tmp = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys
        //                   where t < 1100000
        //                   orderby t descending
        //                   select t).ToList();
        //        int totalCount = tmp.Count;
        //        _queryCount = totalCount / 100;
        //        if (totalCount % 100 > 0) _queryCount += 1;
        //        //_queryCount = (maxRtuId - 1000000) / 100;
        //        //if (maxRtuId % 100 > 0) _queryCount += 1;

        //        for (int i = 1; i <= _queryCount; i++)
        //        {
        //            var ntsnd = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_data; //发送数据太快  如果使用原来的info变量 则发送的时候可能发送的多条相同了 
        //            ntsnd.WstRtuData.Op = info.WstRtuData.Op;
        //            ntsnd.WstRtuData.DtEndTime = info.WstRtuData.DtEndTime;
        //            ntsnd.WstRtuData.DtStartTime = info.WstRtuData.DtStartTime;

        //            ntsnd.Head.Gid += 1;
        //            _guidsThatLastUsed.Add(ntsnd.Head.Gid);
        //            ntsnd.WstRtuData.RtuId = i;

        //            ntsnd.Head.PagingIdx = pageindx;
        //            ntsnd.Head.PagingFlag = 1;

        //           var data= Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(ntsnd);
        //            //请求的包序号  当rtuid为0 时查询所有数据 当rtuid《1000的时候 查询的为 （rtuid-1）*100+1000000 开始的100个终端数据  当rtuid》1000的时候查询该终端的数据
        //           // SndOrderServer.OrderSnd(ntsnd);



        //            Thread.Sleep(100);
        //        }
        //        Remind = "查询命令已发送...请等待数据反馈！";
        //    }
        //    else
        //    {
        //        // info.WstRtuData.RtuId = i; //请求的包序号  当rtuid为0 时查询所有数据 当rtuid《1000的时候 查询的为 （rtuid-1）*100+1000000 开始的100个终端数据  当rtuid》1000的时候查询该终端的数据
        //        SndOrderServer.OrderSnd(info);
        //        Remind = "查询命令已发送...请等待数据反馈！";



        //        //var datax = System.Convert.ToBase64String(Wlst.mobile.MsgWithMobile.SerializeToBytes(info));
        //        //var data = wlst.sr.iif.HttpGetPost.HttpGet("http://10.30.37.184:8080/mims/get122292", "?pb2=" + datax);
        //        //if (data == null) return;
        //        //// 反序列化get到的数据
        //        //var databk = Wlst.mobile.MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
        //        //if (databk == null) return;
        //        ////ifst==0 异常，异常原因为 ifmsg
        //        //RecordDataRequest(null, databk);

        //    }
        //}


        #endregion





    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class EquipmentDailyDataQueryViewModel
    {

        public void InitAction()
        {
            //ProtocolServer.RegistProtocol(
            //    Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_data, // .wlst_svr_ans_cnt_request_wj3090_measure_data ,
            //    RecordDataRequest,
            //    typeof (EquipmentDailyDataQueryViewModel), this);
        }

        #region original

        //public void RecordDataRequest(string session, Wlst.mobile.MsgWithMobile infos)
        //{
        //    if (infos == null || infos.WstRtuData == null) return;
        //    if (infos.WstRtuData.Op != 2 && infos.WstRtuData.Op != 4 && infos.WstRtuData.Op != 3) return;

        //    var info = infos.WstRtuData;

        //    this.RtuId = info.RtuId;

        //    var tEndTime = EndTime <= 1440 && EndTime >= 0 ? new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, EndTime / 60, EndTime % 60, 0) : new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, 23, 59, 0);

        //    var tmpitems = new ObservableCollection<TmlNewDataViewModelExtend>();
        //    var tmplist = new List<TmlNewDataViewModelExtend>();
        //    int index = 1;
        //    foreach (var t in info.Items)
        //    {
        //        // tmpitems.Add(new TmlNewDataViewModelExtend(t) {Index = index});
        //        //index++;
        //        tmplist.Add(new TmlNewDataViewModelExtend(t));
        //    }
        //    var tmplist2 = (from t in tmplist orderby t.PhyIdd, t.DtGetDataTime select t).ToList();
        //    var tmplist3 = new List<TmlNewDataViewModelExtend>();
        //    var tmplist4 = new List<TmlNewDataViewModelExtend>();
        //    if (infos.WstRtuData.Op == 4)
        //    {
        //        if (RtuId == 0)
        //        {
        //            for (int j = 0; j < tmplist2.Count; j++)
        //            {
        //                if (j == 0)
        //                {
        //                    tmplist3.Add(tmplist2[j]);
        //                    if (tmplist2[j + 1].RtuId != tmplist2[j].RtuId)
        //                        tmplist3.Add(new TmlNewDataViewModelExtend() { RtuId = tmplist2[j].RtuId });
        //                    else tmplist3.Add(tmplist2[j + 1]);
        //                }
        //                else if (j >= 0 && j < tmplist2.Count - 1)
        //                {
        //                    if (tmplist2[j].RtuId == tmplist2[j + 1].RtuId)
        //                    {
        //                        tmplist3.Add(tmplist2[j]);
        //                        tmplist3.Add(tmplist2[j + 1]);
        //                    }
        //                    if (tmplist2[j].RtuId != tmplist2[j + 1].RtuId && tmplist2[j].RtuId != tmplist2[j - 1].RtuId)
        //                    {
        //                        tmplist3.Add(tmplist2[j]);
        //                        tmplist3.Add(new TmlNewDataViewModelExtend() { RtuId = tmplist2[j].RtuId });
        //                    }
        //                }
        //                else
        //                {
        //                    if (tmplist2[j].RtuId != tmplist2[j - 1].RtuId)
        //                    {
        //                        tmplist3.Add(tmplist2[j]);
        //                        tmplist3.Add(new TmlNewDataViewModelExtend() { RtuId = tmplist2[j].RtuId });
        //                    }
        //                }
        //            }


        //            for (int k = 0; k < tmplist3.Count; k = k + 2)
        //            {
        //                //var a = tmplist3[k + 1].DtGetDataTime;
        //                //var b = tmplist3[k].DtGetDataTimeFormat < tEndTime.AddMinutes(30);
        //                //var c = tmplist3[k].DtGetDataTimeFormat > tEndTime.AddMinutes(-30);
        //                //var x = new Tuple<string , Tuple<bool,bool>>(a, new Tuple<bool, bool>(b,c));
        //                if (tmplist3[k + 1].DtGetDataTime == null && tmplist3[k].DtGetDataTimeFormat < tEndTime.AddMinutes(60) && tmplist3[k].DtGetDataTimeFormat > tEndTime.AddMinutes(-60))
        //                {

        //                    TmlNewDataViewModelExtend temp = new TmlNewDataViewModelExtend();
        //                    temp = tmplist3[k];
        //                    tmplist3[k] = tmplist3[k + 1];
        //                    tmplist3[k + 1] = temp;
        //                }
        //            }
        //            for (int k = 0; k < tmplist3.Count; k = k + 2)
        //            {
        //                if (
        //                    Math.Abs(Convert.ToDouble(tmplist3[k].RtuCurrentSumA) -
        //                             Convert.ToDouble(tmplist3[k + 1].RtuCurrentSumA)) >= DeviationValue
        //                    || Math.Abs(Convert.ToDouble(tmplist3[k].RtuCurrentSumB) -
        //                                Convert.ToDouble(tmplist3[k + 1].RtuCurrentSumB)) >= DeviationValue
        //                    || Math.Abs(Convert.ToDouble(tmplist3[k].RtuCurrentSumC) -
        //                                Convert.ToDouble(tmplist3[k + 1].RtuCurrentSumC)) >= DeviationValue
        //                    || tmplist3[k + 1].RtuCurrentSumA == "")
        //                {
        //                    tmplist4.Add(tmplist3[k]);
        //                    tmplist4.Add(tmplist3[k + 1]);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (tmplist2.Count < 2)
        //                tmplist2.Add(new TmlNewDataViewModelExtend() { RtuId = RtuId, DtGetDataTime = "--" });
        //            if (
        //                Math.Abs(Convert.ToDouble(tmplist2[0].RtuCurrentSumA) -
        //                         Convert.ToDouble(tmplist2[1].RtuCurrentSumA)) >= DeviationValue
        //                || Math.Abs(Convert.ToDouble(tmplist2[0].RtuCurrentSumB) -
        //                            Convert.ToDouble(tmplist2[1].RtuCurrentSumB)) >= DeviationValue
        //                || Math.Abs(Convert.ToDouble(tmplist2[0].RtuCurrentSumC) -
        //                            Convert.ToDouble(tmplist2[1].RtuCurrentSumC)) >= DeviationValue
        //                || tmplist2[1].RtuCurrentSumA == "")
        //            {
        //                tmplist4.Add(tmplist2[0]);
        //                tmplist4.Add(tmplist2[1]);
        //            }
        //        }
        //        foreach (var t in tmplist4)
        //        {
        //            t.Index = index;
        //            index++;
        //            tmpitems.Add(t);
        //        }
        //    }
        //    else if (infos.WstRtuData.Op == 3)
        //    {
        //        var rtuLst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoList(WjParaBase.EquType.Rtu);
        //        foreach (var t in rtuLst)
        //        {
        //            var lst = (from x in tmplist orderby x.RtuId ascending select x.RtuId).ToList();
        //            if (!lst.Contains(t.RtuId)) tmplist.Add(new TmlNewDataViewModelExtend() { RtuId = t.RtuId, DtGetDataTime = "--" });
        //        }
        //        tmplist3 = (from t in tmplist orderby t.PhyIdd, t.DtGetDataTime select t).ToList();
        //        foreach (var t in tmplist3)
        //        {
        //            t.Index = index;
        //            index++;
        //            tmpitems.Add(t);
        //        }
        //    }
        //    else
        //    {
        //        foreach (var t in tmplist2)
        //        {
        //            t.Index = index;
        //            index++;
        //            tmpitems.Add(t);
        //        }
        //    }



        //    this.Records = tmpitems;
        //    //  Remind = "数据已反馈，查询命令已结束，请查看数据！";
        //    var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(info.RtuId);
        //    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" +
        //             (tmp == null ? info.RtuId + "" : tmp.RtuName) + "--终端数据查询成功，共计" + Records.Count + "条数据.";
        //    //info.Items .Count + " 条数据.";
        //}

        #endregion

        private int count;
        public void RecordDataRequest(Wlst.mobile.MsgWithMobile infos, int pagingFlag,int type)
        {

            if (_thisViewActive == false) return;
            if (infos == null || infos.WstRtuData == null) return;
            //if (infos.WstRtuData.Op != 2 && infos.WstRtuData.Op != 4 && infos.WstRtuData.Op != 3) return;
            if (pagingFlag == 0)
            {
                PageSize = infos.Head.PagingNum;
                ItemCount = infos.Head.PagingRecordTotal;
                count = ItemCount / PageSize + (ItemCount % PageSize > 0 ? 1 : 0);
                PagerVisi = count < 2 || IsCheckedLoop? Visibility.Collapsed : Visibility.Visible;
                PageTotal = "页     " + ItemCount + " 条";
            }
            if (infos.Head.Scc == 0)
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--终端数据查询成功，共计" + ItemCount + "条数据.";
            }
            else
            {
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--终端数据查询失败.";
            }
            var info = infos.WstRtuData;

            this.RtuId = info.RtuId;

            var tEndTime = EndTime <= 1440 && EndTime >= 0
                               ? new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, EndTime/60, EndTime%60, 0)
                               : new DateTime(DtEndTime.Year, DtEndTime.Month, DtEndTime.Day, 23, 59, 0);

            var tmpitems = new ObservableCollection<TmlNewDataViewModelExtend>();
            var tmplist = new List<TmlNewDataViewModelExtend>();
            var tmplistRe = new List<TmlNewDataViewModelExtend>();
            int index = 1+PageSize*PageIndex;
            foreach (var t in info.Items)
            {
                tmplist.Add(new TmlNewDataViewModelExtend(t));
            }
            foreach (var f in tmplist)
            {
                f.DtGetDataTime = f.DtGetDataTime == "0001-01-01 00:00:00" ? "--" : f.DtGetDataTime;
            }
            var tmplist2 = (from t in tmplist orderby t.PhyIdd , t.DtGetDataTime select t).ToList();
            var tmplist3 = new List<TmlNewDataViewModelExtend>();
            var tmplist4 = new List<TmlNewDataViewModelExtend>();
            if (type == 3)
            {

                if (IsAllRtu == false)
                {
                    if (tmplist2.Count < 2 && tmplist2.Count > 0)
                        tmplist2.Add(new TmlNewDataViewModelExtend() {RtuId = RtuId, DtGetDataTime = "--"});
                    if (
                        Math.Abs(Convert.ToDouble(tmplist2[0].RtuCurrentSumA) -
                                 Convert.ToDouble(tmplist2[1].RtuCurrentSumA)) >= DeviationValue
                        || Math.Abs(Convert.ToDouble(tmplist2[0].RtuCurrentSumB) -
                                    Convert.ToDouble(tmplist2[1].RtuCurrentSumB)) >= DeviationValue
                        || Math.Abs(Convert.ToDouble(tmplist2[0].RtuCurrentSumC) -
                                    Convert.ToDouble(tmplist2[1].RtuCurrentSumC)) >= DeviationValue
                        || tmplist2[1].RtuCurrentSumA == "")
                    {
                        tmplist4.Add(tmplist2[0]);
                        tmplist4.Add(tmplist2[1]);
                    }
                }
                else
                {
                    for (int j = 0; j < tmplist2.Count; j++)
                    {
                        if (j == 0)
                        {
                            tmplist3.Add(tmplist2[j]);
                            if (tmplist2[j + 1].RtuId != tmplist2[j].RtuId)
                                tmplist3.Add(new TmlNewDataViewModelExtend() { RtuId = tmplist2[j].RtuId });
                            else tmplist3.Add(tmplist2[j + 1]);
                        }
                        else if (j >= 0 && j < tmplist2.Count - 1)
                        {
                            if (tmplist2[j].RtuId == tmplist2[j + 1].RtuId)
                            {
                                tmplist3.Add(tmplist2[j]);
                                tmplist3.Add(tmplist2[j + 1]);
                            }
                            if (tmplist2[j].RtuId != tmplist2[j + 1].RtuId && tmplist2[j].RtuId != tmplist2[j - 1].RtuId)
                            {
                                tmplist3.Add(tmplist2[j]);
                                tmplist3.Add(new TmlNewDataViewModelExtend() { RtuId = tmplist2[j].RtuId });
                            }
                        }
                        else
                        {
                            if (tmplist2[j].RtuId != tmplist2[j - 1].RtuId)
                            {
                                tmplist3.Add(tmplist2[j]);
                                tmplist3.Add(new TmlNewDataViewModelExtend() { RtuId = tmplist2[j].RtuId });
                            }
                        }
                    }


                    for (int k = 0; k < tmplist3.Count; k = k + 2)
                    {
                        if (tmplist3[k + 1].DtGetDataTime == null &&
                            tmplist3[k].DtGetDataTimeFormat < tEndTime.AddMinutes(60) &&
                            tmplist3[k].DtGetDataTimeFormat > tEndTime.AddMinutes(-60))
                        {

                            TmlNewDataViewModelExtend temp = new TmlNewDataViewModelExtend();
                            temp = tmplist3[k];
                            tmplist3[k] = tmplist3[k + 1];
                            tmplist3[k + 1] = temp;
                        }
                    }
                    for (int k = 0; k < tmplist3.Count; k = k + 2)
                    {
                        if (
                            Math.Abs(Convert.ToDouble(tmplist3[k].RtuCurrentSumA) -
                                     Convert.ToDouble(tmplist3[k + 1].RtuCurrentSumA)) >= DeviationValue
                            || Math.Abs(Convert.ToDouble(tmplist3[k].RtuCurrentSumB) -
                                        Convert.ToDouble(tmplist3[k + 1].RtuCurrentSumB)) >= DeviationValue
                            || Math.Abs(Convert.ToDouble(tmplist3[k].RtuCurrentSumC) -
                                        Convert.ToDouble(tmplist3[k + 1].RtuCurrentSumC)) >= DeviationValue
                            || tmplist3[k + 1].RtuCurrentSumA == "")
                        {
                            tmplist4.Add(tmplist3[k]);
                            tmplist4.Add(tmplist3[k + 1]);
                        }
                    }
                }
                foreach (var t in tmplist4)
                {
                    t.Index = index;
                    index++;
                    tmpitems.Add(t);
                }
            }
            else if (type == 2)
            {
                //var rtuLst = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoList(WjParaBase.EquType.Rtu);
                //foreach (var t in rtuLst)
                //{
                //    var lst = (from x in tmplist orderby x.RtuId ascending select x.RtuId).ToList();
                //    if (!lst.Contains(t.RtuId)) tmplist.Add(new TmlNewDataViewModelExtend() { RtuId = t.RtuId, DtGetDataTime = "--" });
                //}
                //tmplist3 = (from t in tmplist orderby t.PhyIdd, t.DtGetDataTime select t).ToList();
                foreach (var t in tmplist)
                {
                    t.Index = index;
                    index++;
                    tmpitems.Add(t);
                }
            }
            else
            {
                if (Wlst.Cr.CoreOne.Services.OptionXmlSvr.GetOptionBool(2801, 1) == true) //是否倒序呈现
                {
                    var tmp = (from t in tmplist2 orderby t.DtGetDataTime descending select t).ToList();
                    foreach (var t in tmp)
                    {
                        t.Index = index;
                        index++;
                        tmpitems.Add(t);
                    }
                }
                else
                {

                    foreach (var t in tmplist2)
                    {
                        t.Index = index;
                        index++;
                        tmpitems.Add(t);
                    }
                }
            }
            Records = tmpitems;


            //if (_guidsThatLastUsed.Contains(infos.Head.Gid))
            //{
            //    if (infos.WstRtuData.Op == 4)
            //    {
            //        for (int j = 0; j < tmplist2.Count; j++)
            //        {
            //            if (j == 0)
            //            {
            //                tmplist3.Add(tmplist2[j]);
            //                if (tmplist2[j + 1].RtuId != tmplist2[j].RtuId)
            //                    tmplist3.Add(new TmlNewDataViewModelExtend() {RtuId = tmplist2[j].RtuId});
            //                else tmplist3.Add(tmplist2[j + 1]);
            //            }
            //            else if (j >= 0 && j < tmplist2.Count - 1)
            //            {
            //                if (tmplist2[j].RtuId == tmplist2[j + 1].RtuId)
            //                {
            //                    tmplist3.Add(tmplist2[j]);
            //                    tmplist3.Add(tmplist2[j + 1]);
            //                }
            //                if (tmplist2[j].RtuId != tmplist2[j + 1].RtuId && tmplist2[j].RtuId != tmplist2[j - 1].RtuId)
            //                {
            //                    tmplist3.Add(tmplist2[j]);
            //                    tmplist3.Add(new TmlNewDataViewModelExtend() {RtuId = tmplist2[j].RtuId});
            //                }
            //            }
            //            else
            //            {
            //                if (tmplist2[j].RtuId != tmplist2[j - 1].RtuId)
            //                {
            //                    tmplist3.Add(tmplist2[j]);
            //                    tmplist3.Add(new TmlNewDataViewModelExtend() {RtuId = tmplist2[j].RtuId});
            //                }
            //            }
            //        }


            //        for (int k = 0; k < tmplist3.Count; k = k + 2)
            //        {
            //            if (tmplist3[k + 1].DtGetDataTime == null &&
            //                tmplist3[k].DtGetDataTimeFormat < tEndTime.AddMinutes(60) &&
            //                tmplist3[k].DtGetDataTimeFormat > tEndTime.AddMinutes(-60))
            //            {

            //                TmlNewDataViewModelExtend temp = new TmlNewDataViewModelExtend();
            //                temp = tmplist3[k];
            //                tmplist3[k] = tmplist3[k + 1];
            //                tmplist3[k + 1] = temp;
            //            }
            //        }
            //        for (int k = 0; k < tmplist3.Count; k = k + 2)
            //        {
            //            if (
            //                Math.Abs(Convert.ToDouble(tmplist3[k].RtuCurrentSumA) -
            //                         Convert.ToDouble(tmplist3[k + 1].RtuCurrentSumA)) >= DeviationValue
            //                || Math.Abs(Convert.ToDouble(tmplist3[k].RtuCurrentSumB) -
            //                            Convert.ToDouble(tmplist3[k + 1].RtuCurrentSumB)) >= DeviationValue
            //                || Math.Abs(Convert.ToDouble(tmplist3[k].RtuCurrentSumC) -
            //                            Convert.ToDouble(tmplist3[k + 1].RtuCurrentSumC)) >= DeviationValue
            //                || tmplist3[k + 1].RtuCurrentSumA == "")
            //            {
            //                tmplist4.Add(tmplist3[k]);
            //                tmplist4.Add(tmplist3[k + 1]);
            //            }
            //        }
            //        _guidsThatLastUsed.Remove(infos.Head.Gid);
            //        int counxx = Records.Count;
            //        foreach (var f in tmplist4)
            //        {
            //            f.Index = counxx;
            //            this.Records.Add(f);
            //            counxx++;
            //            if (counxx%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();

            //        }
            //        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--" +
            //                 (_queryCount - _guidsThatLastUsed.Count) + "/" + _queryCount + " 终端数据查询成功，共计" +
            //                 Records.Count + "条数据.";


            //        if (_guidsThatLastUsed.Count == 0)
            //        {
            //            var rtuLst =
            //                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoList(
            //                    WjParaBase.EquType.Rtu);

            //            var lst = (from x in Records select x.RtuId).Distinct().ToList();

            //            int count = lst.Count + 1;
            //            foreach (var t in rtuLst)
            //            {
            //                if (!lst.Contains(t.RtuId))
            //                    Records.Add(new TmlNewDataViewModelExtend()
            //                                    {RtuId = t.RtuId, Index = count, DtGetDataTime = "无数据"});

            //                count += 1;
            //            }

            //            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 终端数据查询成功，共计" + Records.Count +
            //                     "条数据.";
            //        }
            //    }
            //    else //所有终端数据返回处理
            //    {
            //        _guidsThatLastUsed.Remove(infos.Head.Gid);
            //        int counxx = 0;

            //        //if (Records.Count == 0)
            //        //{ counxx = 1;}
            //        //else
            //        //{
            //        //    counxx = Records.Count;
            //        //}

            //        foreach (var f in tmplist) //tmplist
            //        {
            //            f.Index = RecordsTmp.Count + 1; //counxx;
            //            this.RecordsTmp.Add(f);
            //            //counxx++;
            //            //if (counxx%100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();

            //        }


            //        Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--" +
            //                 (_queryCount - _guidsThatLastUsed.Count) + "/" + _queryCount + " 终端数据查询成功，共计" +
            //                 Records.Count + "条数据.";

            //        //发送完
            //        if (_guidsThatLastUsed.Count == 0)
            //        {
            //            //延迟
            //            Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
            //            var rtuLst =
            //                Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoList(
            //                    WjParaBase.EquType.Rtu);
            //            //lvf  todo 2018年6月20日16:59:40  test  has question
            //            var lst = new List<int>(); // (from x in Records select x.RtuId).ToList();

            //            var items = (from t in RecordsTmp orderby t.RtuId select t).ToList();
            //            int ccc = 1;
            //            foreach (var g in items)
            //            {
            //                g.Index = ccc;
            //                Records.Add(g);
            //                ccc++;

            //                if (lst.Contains(g.RtuId)) continue;
            //                lst.Add(g.RtuId);
            //            }




            //            //for (int i = this.Records.Count - 1; i >= 0; i--)
            //            //{
            //            //    if (lst.Contains(Records[i].RtuId) ) continue;
            //            //    lst.Add(Records[i].RtuId);

            //            //}
            //            int count = Records.Count;
            //            foreach (var t in rtuLst)
            //            {
            //                if (!lst.Contains(t.RtuId))
            //                    Records.Add(new TmlNewDataViewModelExtend()
            //                                    {RtuId = t.RtuId, Index = count, DtGetDataTime = "无数据"});
            //                count++;
            //            }

            //            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "-- 终端数据查询成功，共计" + Records.Count +
            //                     "条数据.";
            //        }
            //    }

            //}
            //else
            //{
            //    this.Records = tmpitems;


            //    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "--终端数据查询成功，共计" + Records.Count + "条数据.";
            //}

            ////  Remind = "数据已反馈，查询命令已结束，请查看数据！";
            //var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(info.RtuId);
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " --" +
            //         (tmp == null ? info.RtuId + "" : tmp.RtuName) + "--终端数据查询成功，共计" + Records.Count + "条数据.";
            ////info.Items .Count + " 条数据.";
        }


        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);
        }



        public override void ExPublishedEvent(
            PublishEventArgs args)
        {

            if (_thisViewActive == false) return;
            if (IsLocked) return;
            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {

                    int id = Convert.ToInt32(args.GetParams()[0]);
                    if (id > 1100000)
                    {
                        var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                        if (tmps == null) return;
                        id = tmps.RtuFid;
                    }
                    if (id < 1000000 || id > 1100000) return;

                    SelectRtuIdChange(id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class EquipmentDailyDataQueryViewModel
    {
        /// <summary>
        /// 上一次查询 是否是所有设备数据查询  如果是所有数据查询 则需要分包查询 
        /// </summary>
        private bool _isLastQueryIsAll = false;

        private List<long> _guidsThatLastUsed = new List<long>();
        private int _queryCount = 0;

        #region original

        //private void Query(DateTime dtstarttime, DateTime dtendtime, int tml)
        //{

        //    var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(tml);
        //    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询：" + (tmp == null ? tml + "" : tmp.RtuName) + " ...";
        //    var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_data;// .wlst_cnt_request_wj3090_measure_data 
        //    info.WstRtuData.DtStartTime = dtstarttime.Ticks;

        //    if (IsAllRtu == true) info.WstRtuData.RtuId = 0;
        //    else info.WstRtuData.RtuId = tml;

        //    if (IsOneTime == true) info.WstRtuData.DtEndTime = dtstarttime.AddHours(1).Ticks;
        //    else info.WstRtuData.DtEndTime = dtendtime.Ticks;

        //    if (IsCompare == true) info.WstRtuData.Op = 4;
        //    else if (IsOneTime == true) info.WstRtuData.Op = 3;
        //    else info.WstRtuData.Op = 2;

        //    SndOrderServer.OrderSnd(info, 10, 6);
        //    Remind = "查询命令已发送...请等待数据反馈！";
        //}

        #endregion

        private void Query(DateTime dtstarttime, DateTime dtendtime, int tml)
        {

            //var tmp = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(tml );
            //Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询：" + (tmp == null ? tml  + "" : tmp.RtuName) + " ...";



            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_data; // .wlst_cnt_request_wj3090_measure_data 
            info.WstRtuData.DtStartTime = dtstarttime.Ticks;

            if (IsAllRtu == true) info.WstRtuData.RtuId = 0;
            else info.WstRtuData.RtuId = tml;

            _isLastQueryIsAll = IsAllRtu;
            _guidsThatLastUsed.Clear();

            if (IsOneTime == true) info.WstRtuData.DtEndTime = dtstarttime.AddHours(1).Ticks;
            else info.WstRtuData.DtEndTime = dtendtime.Ticks;

            if (IsCompare == true) info.WstRtuData.Op = 4;
            else if (IsOneTime == true) info.WstRtuData.Op = 3;
            else info.WstRtuData.Op = 2;

            if (_isLastQueryIsAll)
            {
                int maxRtuId = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys
                                where t < 1100000
                                orderby t descending
                                select t).ToList()[0];
                var tmp = (from t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.Keys
                           where t < 1100000
                           orderby t descending
                           select t).ToList();
                int totalCount = tmp.Count;
                _queryCount = totalCount/100;
                if (totalCount%100 > 0) _queryCount += 1;
                //_queryCount = (maxRtuId - 1000000) / 100;
                //if (maxRtuId % 100 > 0) _queryCount += 1;

                for (int i = 1; i <= _queryCount; i++)
                {
                    var ntsnd = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_data; //发送数据太快  如果使用原来的info变量 则发送的时候可能发送的多条相同了 
                    ntsnd.WstRtuData.Op = info.WstRtuData.Op;
                    ntsnd.WstRtuData.DtEndTime = info.WstRtuData.DtEndTime;
                    ntsnd.WstRtuData.DtStartTime = info.WstRtuData.DtStartTime;

                    ntsnd.Head.Gid += 1;
                    _guidsThatLastUsed.Add(ntsnd.Head.Gid);
                    ntsnd.WstRtuData.RtuId = i;
                        //请求的包序号  当rtuid为0 时查询所有数据 当rtuid《1000的时候 查询的为 （rtuid-1）*100+1000000 开始的100个终端数据  当rtuid》1000的时候查询该终端的数据
                    SndOrderServer.OrderSnd(ntsnd);


                
                    Thread.Sleep(100);
                }
                Remind = "查询命令已发送...请等待数据反馈！";
            }
            else
            {
                // info.WstRtuData.RtuId = i; //请求的包序号  当rtuid为0 时查询所有数据 当rtuid《1000的时候 查询的为 （rtuid-1）*100+1000000 开始的100个终端数据  当rtuid》1000的时候查询该终端的数据
                SndOrderServer.OrderSnd(info);
                Remind = "查询命令已发送...请等待数据反馈！";



                //var datax = System.Convert.ToBase64String(Wlst.mobile.MsgWithMobile.SerializeToBytes(info));
                //var data = wlst.sr.iif.HttpGetPost.HttpGet("http://10.30.37.184:8080/mims/get122292", "?pb2=" + datax);
                //if (data == null) return;
                //// 反序列化get到的数据
                //var databk = Wlst.mobile.MsgWithMobile.Deserialize(System.Convert.FromBase64String(data));
                //if (databk == null) return;
                ////ifst==0 异常，异常原因为 ifmsg
                //RecordDataRequest(null, databk);

            }
        }


      private bool GetCheckedInformation()
        {
            if ((this.PhyId == 0 || IsAllRtu == true) && IsGjVisi==Visibility.Collapsed)
            {
                UMessageBox.Show("提醒", "请选择一个终端！", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            if ((this.PhyId == 0 || IsAllRtu == true) && IsOneTime == false && IsCompare == false)
            {
                UMessageBox.Show("提醒", "所有终端数据查询请勾选“指定时刻”", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            if (DtStartTime.AddDays(63) < DtEndTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，查询时长在62天以内", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            return true;
        }

        //http请求
      private void RequestHttpData(DateTime dtstarttime, DateTime dtendtime, int tml, int pageIndex, int pagingFlag)
      {
          Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 正在查询...";
          MsgWithMobile info;
          int type;
          if (IsCompare == true)
          {
              info = Wlst.Sr.ProtocolPhone.LxRtuHttp.wst_rtu_data_http3;
              type = 3;
          }
          else if (IsOneTime == true)
          {
              info = Wlst.Sr.ProtocolPhone.LxRtuHttp.wst_rtu_data_http2;
              type = 2;
          }
          else
          {
              info = Wlst.Sr.ProtocolPhone.LxRtuHttp.wst_rtu_data_http1;
              type = 1;
          }
            info.WstRtuData.DtStartTime = dtstarttime.Ticks;
            if (IsAllRtu == true) info.WstRtuData.RtuId = 0;
            else info.WstRtuData.RtuId = tml;

            _isLastQueryIsAll = IsAllRtu;
            _guidsThatLastUsed.Clear();

            if (IsOneTime == true) info.WstRtuData.DtEndTime = dtstarttime.AddHours(1).Ticks;
            else info.WstRtuData.DtEndTime = dtendtime.Ticks;

            
            info.Head.PagingIdx = pageIndex + 1;
            info.Head.PagingFlag = pagingFlag;
            var data = Wlst.Cr.CoreMims.HttpGetPostforMsgWithMobile.OrderSndHttp(info);
            if (data == null) return;
            RecordDataRequest(data, pagingFlag,type);
          
        }

      
        public class LoopDataInfo : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private DateTime _dtGetDataTime;

            /// <summary>
            /// 客户的接收到数据的时间
            /// </summary>
            public DateTime DtGetDataTime
            {
                get { return _dtGetDataTime; }
                set
                {
                    if (value != _dtGetDataTime)
                    {
                        _dtGetDataTime = value;
                        this.RaisePropertyChanged(() => this.DtGetDataTime);
                    }
                }
            }

            private double _current;
            /// <summary>
            /// 电流
            /// </summary>
            public double Current
            {
                get { return _current; }
                set
                {
                    if (value != _current)
                    {
                        _current = value;
                        this.RaisePropertyChanged(() => this.Current);
                    }
                }
            }

            private double _sumCurrent;
            /// <summary>
            /// 总电流
            /// </summary>
            public double SumCurrent
            {
                get { return _sumCurrent; }
                set
                {
                    if (value != _sumCurrent)
                    {
                        _sumCurrent = value;
                        this.RaisePropertyChanged(() => this.SumCurrent);
                    }
                }
            }

            private double _voltage;
            /// <summary>
            /// 电压
            /// </summary>
            public double Voltage
            {
                get { return _voltage; }
                set
                {
                    if (value != _voltage)
                    {
                        _voltage = value;
                        this.RaisePropertyChanged(() => this.Voltage);
                    }
                }
            }

            private int _loopid;
            public int LoopId
            {
                get { return _loopid; }
                set
                {
                    if (_loopid != value)
                    {
                        _loopid = value;
                        this.RaisePropertyChanged(() => this.LoopId);
                    }
                }
            }

            private string _loopname;
            public string LoopName
            {
                get { return _loopname; }
                set
                {
                    if (value != _loopname)
                    {
                        _loopname = value;
                        this.RaisePropertyChanged(() => this.LoopName);
                    }
                }
            }

            private string _loopk;
            public string LoopK
            {
                get { return _loopk; }
                set
                {
                    if (value != _loopk)
                    {
                        _loopk = value;
                        this.RaisePropertyChanged(() => this.LoopK);
                    }
                }
            }

            private string _switchInState;
            public string SwitchInState
            {
                get { return _switchInState; }
                set
                {
                    if (value != _switchInState)
                    {
                        _switchInState = value;
                        this.RaisePropertyChanged(() => this.SwitchInState);
                    }
                }
            }

            private string _a;
            public string A
            {
                get { return _a; }
                set
                {
                    if (value != _a)
                    {
                        _a = value;
                        this.RaisePropertyChanged(() => this.A);
                    }
                }
            }

            private string _v;
            public string V
            {
                get { return _v; }
                set
                {
                    if (value != _v)
                    {
                        _v = value;
                        this.RaisePropertyChanged(() => this.V);
                    }
                }
            }

            private string _power;
            public string Power
            {
                get { return _power; }
                set
                {
                    if (value != _power)
                    {
                        _power = value;
                        this.RaisePropertyChanged(() => this.Power);
                    }
                }
            }

            private string _powerFactor;
            public string PowerFactor
            {
                get { return _powerFactor; }
                set
                {
                    if (value != _powerFactor)
                    {
                        _powerFactor = value;
                        this.RaisePropertyChanged(() => this.PowerFactor);
                    }
                }
            }

            private string _brightRate;
            public string BrightRate
            {
                get { return _brightRate; }
                set
                {
                    if (value != _brightRate)
                    {
                        _brightRate = value;
                        this.RaisePropertyChanged(() => this.BrightRate);
                    }
                }
            }
        }
    }
}
