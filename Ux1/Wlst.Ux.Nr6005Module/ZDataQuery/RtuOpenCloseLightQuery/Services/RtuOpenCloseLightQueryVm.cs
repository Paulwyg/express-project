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
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.MessageBoxOverride.MessageBoxOverride;
using Wlst.Cr.PPProtocolSvrCnt.Common;
using Wlst.Sr.TimeTableSystem.Services;
using Wlst.Ux.Nr6005Module.ZDataQuery.RtuOpenCloseLightQuery.Model;
using Wlst.client;

namespace Wlst.Ux.Nr6005Module.ZDataQuery.RtuOpenCloseLightQuery.Services
{
    [Export(typeof (IIRtuOpenCloseLightQuery))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RtuOpenCloseLightQueryVm :
        Wlst.Cr.Core.EventHandlerHelper.EventHandlerHelperExtendNotifyProperyChanged, IIRtuOpenCloseLightQuery
    {
        private bool _thisViewActive = false;

        public RtuOpenCloseLightQueryVm()
        {
            this.InitEvent();
            this.InitAction();
            this.InitActionTime();

        }

        public void NavOnLoad(params object[] parsObjects)
        {
            IsCheckedRtu = true;
            this.DtEndTime = DateTime.Now;
            this.DtStartTime = DateTime.Now.AddDays(-7);
            _thisViewActive = true;
            DtStartTimeTime = DateTime.Now.AddDays(-7);
            DtEndTimeTime = DateTime.Now;
            InitTimeTable();
            ShowOrderInfo = "";
            InitSun();
            IsSelectIndex = 0;
            Remind = "请设置好查询日期后通过选择左侧终端进行查询...";
        }


        public void OnUserHideOrClosing()
        {
            _thisViewActive = false;
            Records = new ObservableCollection<RtuOpenCloseItem>();
            this.RecordTimes = new ObservableCollection<ExecuteItem>();
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
                return "开关灯时间统计"; //I36N .Services.I36N .ConvertByCodingOne("11090001", "Setting");
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

    public partial class RtuOpenCloseLightQueryVm
    {
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

        #region RtuId

        private int _rtuId;

        /// <summary>
        /// 终端地址 或组地址
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
                    PhyId = value;
                    this.RtuName = "Reserve";
                    if (_rtuId > 1000000)
                    {
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
                    //else
                    //{
                    //    var mtp =
                    //        Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGroupInfomation(_rtuId);
                    //    if (mtp == null) this.RtuName = "UnKnown";
                    //    else this.RtuName = mtp.GroupName;
                    //}

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

        #region IsCheckedRtu

        private bool _ischeckedrtu;

        /// <summary>
        /// 是否选中终端
        /// </summary>
        public bool IsCheckedRtu
        {
            get { return _ischeckedrtu; }
            set
            {
                if (value != _ischeckedrtu)
                {
                    _ischeckedrtu = value;
                    this.RaisePropertyChanged(() => this.IsCheckedRtu);
                }
            }
        }


       #endregion

        #region visione

        private Visibility _visione;

        public Visibility Visione
        {
            get { return _visione; }
            set
            {
                if (value == _visione) return;
                _visione = value;
                RaisePropertyChanged(() => Visione);
            }
        }

        #endregion


        #region visione

        private Visibility _visithree;

        public Visibility Visithree
        {
            get { return _visithree; }
            set
            {
                if (value == _visithree) return;
                _visithree = value;
                RaisePropertyChanged(() => Visithree);
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

        #region Records

        private ObservableCollection<RtuOpenCloseItem> _record;

        public ObservableCollection<RtuOpenCloseItem> Records
        {
            get
            {

                if (_record == null)
                    _record = new ObservableCollection<RtuOpenCloseItem>();
                return _record;
            }
            set
            {
                if (_record == value) return;
                _record = value;
                this.RaisePropertyChanged(() => this.Records);
            }
        }

        #endregion

        #region RecordsTwo

        private ObservableCollection<RtuOpenCloseItemTwo> _recordtwo;

        public ObservableCollection<RtuOpenCloseItemTwo> RecordsTwo
        {
            get
            {

                if (_recordtwo == null)
                    _recordtwo = new ObservableCollection<RtuOpenCloseItemTwo>();
                return _recordtwo;
            }
        }

        #endregion


        #region RecordsThree

        private ObservableCollection<RtuOpenCloseItemThree> _recordthree;

        public ObservableCollection<RtuOpenCloseItemThree> RecordsThree
        {
            get
            {

                if (_recordthree == null)
                    _recordthree = new ObservableCollection<RtuOpenCloseItemThree>();
                return _recordthree;
            }
        }

        #endregion

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

        private DateTime _dtQueryAll;
        private ICommand _cmdQueryAll;

        public ICommand CmdQueryAll
        {
            get
            {
                if (_cmdQueryAll == null)
                    _cmdQueryAll = new RelayCommand(ExAll, CanExAll, true);
                return _cmdQueryAll;
            }
        }
        private void ExAll()
        {
            _dtlastsndtime = DateTime.Now;
            Records.Clear();
            RecordsTwo.Clear();
            //if (!GetCheckedOnOffInformation()) return;
            this.RecordsThree.Clear();

                List<int> areaLst = new List<int>();
                List<int> rtusInArea = new List<int>();
                var userProperty = UserInfo.UserLoginInfo;
                bool IsLoadOnlyOneArea = false;
                if (userProperty.D == true)
                {
                    if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 0) return;
                    IsLoadOnlyOneArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count < 2;
                    if (IsLoadOnlyOneArea)
                    {
                        int AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys.ToList()[0];
                        var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                        foreach (var fff in tmlLstOfArea)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                continue;
                            if (fff < 1099999) rtusInArea.Add(fff);
                        }
                    }
                    else
                    {
                        foreach (var f in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Keys)
                        {
                            var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                            foreach (var fff in tmlLstOfArea)
                            {
                                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                    continue;
                                if (fff < 1099999) rtusInArea.Add(fff);
                            }
                        }
                    }
                }
                else
                {
                    areaLst.Clear();
                    foreach (var t in userProperty.AreaX)
                    {
                        if (t >= 0)
                        {
                            areaLst.Add(t);
                        }
                    }
                    //areaLst.AddRange(userProperty.AreaX);
                    foreach (var t in userProperty.AreaW)
                    {
                        if (!areaLst.Contains(t) && t >= 0)
                        {
                            areaLst.Add(t);
                        }
                    }
                    foreach (var f in userProperty.AreaR)
                    {
                        if (!areaLst.Contains(f) && f >= 0)
                        {
                            areaLst.Add(f);
                        }
                    }

                    IsLoadOnlyOneArea = areaLst.Count < 2;
                    if (IsLoadOnlyOneArea)
                    {
                        int AreaId = areaLst[0];
                        var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(AreaId);
                        foreach (var fff in tmlLstOfArea)
                        {
                            if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                continue;
                            if (fff < 1099999) rtusInArea.Add(fff);
                        }
                    }
                    else
                    {

                        foreach (var f in areaLst)
                        {
                            var tmlLstOfArea = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuInArea(f);
                            foreach (var fff in tmlLstOfArea)
                            {
                                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(fff))
                                    continue;
                                if (fff < 1099999) rtusInArea.Add(fff);
                            }

                        }
                    }
                }
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在查询...";
                foreach (var g in rtusInArea)
                {
                    this.Query(this.DtStartTime, this.DtEndTime, g);
                }
            }
        

        private void Ex()
        {
            _dtlastsndtime = DateTime.Now;
            Records.Clear();
            RecordsTwo.Clear();
            //if (!GetCheckedOnOffInformation()) return;
            this.RecordsThree.Clear();
            if (RtuId > 1000000)
            {
                this.Query(this.DtStartTime, this.DtEndTime, this.RtuId);
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在查询...";
            }
           
                //todo
            //else
            //{
            //    var mtp =
            //        Sr.EquipmentGroupInfoHolding.Services.ServicesGrpSingleInfoHold.GetGrpTmlList(_rtuId);
            //    if (mtp.Count > 0)
            //    {
            //        this.Query(this.DtStartTime, this.DtEndTime, mtp);
            //    }
            //}
            // Remind = "查询命令已发送...请等待数据反馈！";

        }

        private DateTime _dtlastsndtime = DateTime.Now;

        private bool CanEx()
        {
            if (DtStartTime > DtEndTime) return false;
            //if (RtuId < 1) return false;
            if (DateTime.Now.Ticks - _dtlastsndtime.Ticks < 50000000) return false;
            return true;
        }

        private bool CanExAll()
        {
            if (DtStartTime > DtEndTime) return false;
            //if (RtuId < 1) return false;
            if (DateTime.Now.Ticks - _dtlastsndtime.Ticks < 50000000) return false;
            return true;
        }

        #endregion

        #region Records

        private ObservableCollection<RecordItemDay> _recorditem;

        public ObservableCollection<RecordItemDay> RecordItem
        {
            get
            {

                if (_recorditem == null)
                {
                    _recorditem = new ObservableCollection<RecordItemDay>();
                }
                return _recorditem;
            }
        }

        private int _IsSelectIndex;

        public int IsSelectIndex
        {
            get { return _IsSelectIndex; }
            set
            {
                if (_IsSelectIndex != value)
                {
                    _IsSelectIndex = value;
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
                    _IsItem.Add(new ShieldLoop() {Name = "日表", Key = 0});
                    _IsItem.Add(new ShieldLoop() {Name = "月表", Key = 1});
                    _IsItem.Add(new ShieldLoop() { Name = "年表", Key = 2 });
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

        #region CmdQueryForDay

        private DateTime _dtQueryForDay;
        private ICommand _cmdQueryForDay;

        public ICommand CmdQueryForDay
        {
            get
            {
                if (_cmdQueryForDay == null)
                    _cmdQueryForDay = new RelayCommand(ExForDay, CanExForDay, true);
                return _cmdQueryForDay;
            }
        }


        private void ExForDay()
        {
            _dtlastsndtimeForDay = DateTime.Now;
            if (DtStartTimeTime.AddDays(3653) < DtEndTimeTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在10年以内", UMessageBoxButton.Ok);
                return;
            }
            
            this.RecordItem.Clear();
            if (RtuId > 1000000 && IsCheckedRtu)
            {
                var tmls = new List<int>();
                tmls.Add(this.RtuId);
                this.Query(this.DtStartTime, this.DtEndTime,tmls, 4);
            }
            else if(IsCheckedRtu != true)
            {
                var tmls = new List<int>();
                foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    tmls.Add(t.Value.RtuId);
                }
                this.Query(this.DtStartTime, this.DtEndTime, tmls, 4);
            }
            else
            {
                UMessageBox.Show("提醒", "请选择终端。", UMessageBoxButton.Ok);
                return;
            }

        }

        private DateTime _dtlastsndtimeForDay = DateTime.Now;

        private bool CanExForDay()
        {
            if (DtStartTime > DtEndTime) return false;
            if (RtuId < 1) return false;
            if (DateTime.Now.Ticks - _dtlastsndtimeForDay.Ticks < 50000000) return false;
            return true;
        }

        #endregion

    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class RtuOpenCloseLightQueryVm
    {

        public void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_open_close_record,
                // .wlst_svr_ans_cnt_wj3090_request_open_close_light_record,//.ClientPart.wlst_OpenCloseLight_server_ans_clinet_request_rtuopencloseLightrecord,
                RecordDataRequest,
                typeof (RtuOpenCloseLightQueryVm), this);
        }

        private string Calculate_XPower(double power, double time)
        {
            if (time == 0)
            {
                return String.Format("{0:F}", 0) + "Kw";
            }
            else
            {

                return String.Format("{0:F}", power / time) + "Kw";
            }
        }

        private double Calculate_DayXPower(double power1, double time1, double power2, double time2, double power3, double time3, double power4, double time4, double power5, double time5, double power6, double time6, double power7, double time7, double power8, double time8)
        {
            double[] kPower = new double[8];

            if (time1 == 0)
            {
                kPower[0] = 0;
            }
            else
            {
                kPower[0] = power1 / time1;
            }

            if (time2 == 0)
            {
                kPower[1] = 0;
            }
            else
            {
                kPower[1] = power2 / time2;
            }

            if (time3 == 0)
            {
                kPower[2] = 0;
            }
            else
            {
                kPower[2] = power3 / time3;
            }

            if (time4 == 0)
            {
                kPower[3] = 0;
            }
            else
            {
                kPower[3] = power4 / time4;
            }

            if (time5 == 0)
            {
                kPower[4] = 0;
            }
            else
            {
                kPower[4] = power5 / time5;
            }

            if (time6 == 0)
            {
                kPower[5] = 0;
            }
            else
            {
                kPower[5] = power6 / time6;
            }

            if (time7 == 0)
            {
                kPower[6] = 0;
            }
            else
            {
                kPower[6] = power7 / time7;
            }

            if (time8 == 0)
            {
                kPower[7] = 0;
            }
            else
            {
                kPower[7] = power8 / time8;
            }

            return kPower[0] + kPower[1] + kPower[2] + kPower[3] + kPower[4] + kPower[5] + kPower[6] + kPower[7];
        }

        public void RecordDataRequest(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var info = infos.WstRtuOpenCloseRecord;
            if (info == null) return;

            if (info.Op == 1)
            {
                this.Records.Clear();
                Visithree = Visibility.Collapsed;
                Visione = Visibility.Visible;
                int index = 1;
                foreach (var t in info.Item1)
                {
                    this.Records.Add(new RtuOpenCloseItem(t) {Index = index});
                    index++;
                }
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  记录查询成功，共计" + info.Item1.Count + " 条数据.";
            }
            else if (info.Op == 2)
            {

                var dir = new Dictionary<Tuple<int, int, int,int>, RtuOpenCloseItemTwo>();
                if (info.Item2.Count == 0)
                {
                    Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  记录查询成功，共计"+RecordsTwo.Count +"条数据.";
                    return;
                }
                int rtuId = info.Item2[0].RtuId;
                var cal = new long[17];

                foreach (var g in info.Item2)
                {
                    if (g.RtuId != rtuId) continue;

                    foreach (var gg in g.Times)
                    {
                        var dt = new DateTime(gg.LastOpenTime);
                        int month = dt.Month;
                        int day = dt.Day;
                        int year = dt.Year;

                        var dt2 = new DateTime(gg.ThisCloseTime);

                        int flg = 0;
                        var tmp = new Tuple<int, int, int, int>(year, month, day, flg);

                        foreach (var l in dir)
                        {
                            if (l.Key.Item1 == tmp.Item1 && l.Key.Item2 == tmp.Item2 && l.Key.Item3 == tmp.Item3 && dir[tmp].Time[g.LoopId - 1] != "--")
                            {
                                flg = flg + 1;
                                tmp = new Tuple<int, int, int, int>(year, month, day, flg);
                            }
                        }

                        if (dir.ContainsKey(tmp))
                        {
                            if (dir[tmp].Time[g.LoopId - 1] != "--")
                            {
                                flg = flg + 1;
                                tmp = new Tuple<int, int, int, int>(year, month, day, flg);
                            }
                        }


                        if (!dir.ContainsKey(tmp))
                        {
                            dir.Add(tmp,
                                    new RtuOpenCloseItemTwo(rtuId)
                                        {
                                            Dt = gg.LastOpenTime,
                                            Date = dt.Year.ToString("D2") + " - " + dt.Month.ToString("D2") + " - " + dt.Day.ToString("D2")
                                        });
                        }
                        var dtend = new DateTime(gg.ThisCloseTime);
                        var content = dt.Hour.ToString("D2") + ":" + dt.Minute.ToString("D2") + " - " +
                                      dtend.Hour.ToString("D2") + ":" + dtend.Minute.ToString("D2");
                        //var close = dtend.Hour*60 + dtend.Minute;
                        //var open = dt.Hour*60 + dt.Minute;

                        if (g.LoopId < 17 && g.LoopId > 0)
                        {
                            long change = 0;
                            if (gg.ThisCloseTime >= gg.LastOpenTime)
                                change = gg.ThisCloseTime - gg.LastOpenTime;
                            else change = gg.LastOpenTime - gg.ThisCloseTime + 864000000000;

                            cal[g.LoopId - 1] = change + cal[g.LoopId - 1];
                            if (change/600000000>60*24)
                            {
                                content = content + "(" + dt2.Month.ToString("D2") + " - " + dt2.Day.ToString("D2") + ")";
                            }

                        }

                        dir[tmp].Time[g.LoopId - 1] = content;
                       
                    }
                }
                var tmps = (from ggg in dir orderby ggg.Key.Item1, ggg.Key.Item2, ggg.Key.Item3, ggg.Key.Item4 select ggg.Value).ToList();

                int index = RecordsTwo.Count + 1;
                int daycol = 0;
                //this.RecordsTwo.Clear();
                Visithree = Visibility.Collapsed;
                Visione = Visibility.Visible;
                string datecol="";

                foreach (var g in tmps)
                {
                    g.Index = index;
                    index++;
                    RecordsTwo.Add(g);
                    if (g.Date != datecol)
                    {
                        datecol = g.Date;
                        daycol++;
                    }

                }
                var tsss = new RtuOpenCloseItemTwo(rtuId) { Date = "统计 " + daycol + " 天", Index = index };
                for (int i = 0; i < 17; i++)
                {
                    long minu = cal[i]/600000000;
                    double hour = minu/60.0;
                    string cont = hour.ToString("f2") + " 小时";
                    tsss.Time[i] = cont;
                }
                RecordsTwo.Add(tsss);
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  记录查询成功，共计" + RecordsTwo.Count + " 条数据.";
            }
            else if (info.Op == 3)
            {

                var dir = new Dictionary<int, RtuOpenCloseItemThree>();

                foreach (var t in info.Item3)
                {
                    if (!dir.ContainsKey(t.RtuId)) dir.Add(t.RtuId, new RtuOpenCloseItemThree(t.RtuId));
                    long minu = t.Time/60;
                    double hour = minu/60.0;
                    dir[t.RtuId].Time[t.LoopId] = hour.ToString("f2");
                }
                int index = 1;
                this.RecordsThree.Clear();
                Visithree = Visibility.Visible;
                Visione = Visibility.Collapsed;

                var tmpssss = (from g in dir orderby g.Key select g.Value).ToList();
                foreach (var g in tmpssss)
                {
                    g.Index = index;
                    index++;
                    RecordsThree.Add(g);
                }
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  记录查询成功，共计" + RecordsThree.Count + " 条数据.";
            }
            else if (info.Op == 4)
            {
                if (IsCheckedRtu)
                {
                    if (IsSelectIndex == 0)
                    {
                        #region
                        var dic = new Dictionary<long, List<Tuple<int, double>>>();

                        foreach (var t in info.Item4)
                        {
                            if (!dic.ContainsKey(t.DateRiqi))
                            {
                                var lst = new List<Tuple<int, double>>();
                                var tu = new Tuple<int, double>(0, 0);

                                for (int i = 0; i < 8; i++)
                                {
                                    lst.Add(tu);
                                }
                                dic.Add(t.DateRiqi, lst);
                            }
                            dic[t.DateRiqi][t.LoopId - 1] = new Tuple<int, double>(t.TimeMinutes, t.Powers);
                        }

                        var dic1 = from objDic in dic orderby objDic.Key select objDic;

                        int id = 1;
                        double K1TimeAdd = 0.0, K2TimeAdd = 0.0, K3TimeAdd = 0.0, K4TimeAdd = 0.0, K5TimeAdd = 0.0, K6TimeAdd = 0.0, K7TimeAdd = 0.0, K8TimeAdd = 0.0;
                        double DayPowerAdd = 0.0, K1PowerAdd = 0.0, K2PowerAdd = 0.0, K3PowerAdd = 0.0, K4PowerAdd = 0.0, K5PowerAdd = 0.0, K6PowerAdd = 0.0, K7PowerAdd = 0.0, K8PowerAdd = 0.0;
                        double DayXPowerAdd = 0;

                        foreach (var t in dic1)
                        {
                            var dt = new DateTime(t.Key);
                            int month = dt.Month;
                            int day = dt.Day;
                            var daypower = t.Value[0].Item2 + t.Value[1].Item2 + t.Value[2].Item2 + t.Value[3].Item2
                                + t.Value[4].Item2 + t.Value[5].Item2 + t.Value[6].Item2 + t.Value[7].Item2;
                            double dayxpower = Calculate_DayXPower(t.Value[0].Item2, (t.Value[0].Item1 / 60.0),
                                t.Value[1].Item2, (t.Value[1].Item1 / 60.0),
                                t.Value[2].Item2, (t.Value[2].Item1 / 60.0),
                                t.Value[3].Item2, (t.Value[3].Item1 / 60.0),
                                t.Value[4].Item2, (t.Value[4].Item1 / 60.0),
                                t.Value[5].Item2, (t.Value[5].Item1 / 60.0),
                                t.Value[6].Item2, (t.Value[6].Item1 / 60.0),
                                t.Value[7].Item2, (t.Value[7].Item1 / 60.0));


                            RecordItem.Add(new RecordItemDay()
                            {
                                Id = id,
                                Day = month + "-" + day,
                                DayPower = String.Format("{0:F}", daypower) + "Kwh",
                                DayXPower = String.Format("{0:F}", dayxpower) + "Kw",
                                K1Time = String.Format("{0:F}", t.Value[0].Item1 / 60.0) + "小时",
                                K1Power = String.Format("{0:F}", t.Value[0].Item2) + "Kwh",
                                K1XPower = Calculate_XPower(t.Value[0].Item2, (t.Value[0].Item1 / 60.0)),
                                K2Time = String.Format("{0:F}", t.Value[1].Item1 / 60.0) + "小时",
                                K2Power = String.Format("{0:F}", t.Value[1].Item2) + "Kwh",
                                K2XPower = Calculate_XPower(t.Value[1].Item2, (t.Value[1].Item1 / 60.0)),
                                K3Time = String.Format("{0:F}", t.Value[2].Item1 / 60.0) + "小时",
                                K3Power = String.Format("{0:F}", t.Value[2].Item2) + "Kwh",
                                K3XPower = Calculate_XPower(t.Value[2].Item2, (t.Value[2].Item1 / 60.0)),
                                K4Time = String.Format("{0:F}", t.Value[3].Item1 / 60.0) + "小时",
                                K4Power = String.Format("{0:F}", t.Value[3].Item2) + "Kwh",
                                K4XPower = Calculate_XPower(t.Value[3].Item2, (t.Value[3].Item1 / 60.0)),
                                K5Time = String.Format("{0:F}", t.Value[4].Item1 / 60.0) + "小时",
                                K5Power = String.Format("{0:F}", t.Value[4].Item2) + "Kwh",
                                K5XPower = Calculate_XPower(t.Value[4].Item2, (t.Value[4].Item1 / 60.0)),
                                K6Time = String.Format("{0:F}", t.Value[5].Item1 / 60.0) + "小时",
                                K6Power = String.Format("{0:F}", t.Value[5].Item2) + "Kwh",
                                K6XPower = Calculate_XPower(t.Value[5].Item2, (t.Value[5].Item1 / 60.0)),
                                K7Time = String.Format("{0:F}", t.Value[6].Item1 / 60.0) + "小时",
                                K7Power = String.Format("{0:F}", t.Value[6].Item2) + "Kwh",
                                K7XPower = Calculate_XPower(t.Value[6].Item2, (t.Value[6].Item1 / 60.0)),
                                K8Time = String.Format("{0:F}", t.Value[7].Item1 / 60.0) + "小时",
                                K8Power = String.Format("{0:F}", t.Value[7].Item2) + "Kwh",
                                K8XPower = Calculate_XPower(t.Value[7].Item2, (t.Value[7].Item1 / 60.0))
                            });
                            id++;

                            DayPowerAdd = DayPowerAdd + daypower;
                            DayXPowerAdd = DayXPowerAdd + dayxpower;
                            K1TimeAdd = K1TimeAdd + t.Value[0].Item1;
                            K1PowerAdd = K1PowerAdd + t.Value[0].Item2;
                            K2TimeAdd = K2TimeAdd + t.Value[1].Item1;
                            K2PowerAdd = K2PowerAdd + t.Value[1].Item2;
                            K3TimeAdd = K3TimeAdd + t.Value[2].Item1;
                            K3PowerAdd = K3PowerAdd + t.Value[2].Item2;
                            K4TimeAdd = K4TimeAdd + t.Value[3].Item1;
                            K4PowerAdd = K4PowerAdd + t.Value[3].Item2;
                            K5TimeAdd = K5TimeAdd + t.Value[4].Item1;
                            K5PowerAdd = K5PowerAdd + t.Value[4].Item2;
                            K6TimeAdd = K6TimeAdd + t.Value[5].Item1;
                            K6PowerAdd = K6PowerAdd + t.Value[5].Item2;
                            K7TimeAdd = K7TimeAdd + t.Value[6].Item1;
                            K7PowerAdd = K7PowerAdd + t.Value[6].Item2;
                            K8TimeAdd = K8TimeAdd + t.Value[7].Item1;
                            K8PowerAdd = K8PowerAdd + t.Value[7].Item2;

                            if (id % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                        }

                        RecordItem.Add(new RecordItemDay()
                        {
                            Id = id,
                            Day = "统计" + (id - 1) + "天",
                            DayPower = String.Format("{0:F}", DayPowerAdd) + "Kwh",
                            DayXPower = String.Format("{0:F}", DayXPowerAdd) + "Kw",
                            K1Time = String.Format("{0:F}", K1TimeAdd / 60.0) + "小时",
                            K1Power = String.Format("{0:F}", K1PowerAdd) + "Kwh",
                            K1XPower = Calculate_XPower(K1PowerAdd, (K1TimeAdd / 60.0)),
                            K2Time = String.Format("{0:F}", K2TimeAdd / 60.0) + "小时",
                            K2Power = String.Format("{0:F}", K2PowerAdd) + "Kwh",
                            K2XPower = Calculate_XPower(K2PowerAdd, (K2TimeAdd / 60.0)),
                            K3Time = String.Format("{0:F}", K3TimeAdd / 60.0) + "小时",
                            K3Power = String.Format("{0:F}", K3PowerAdd) + "Kwh",
                            K3XPower = Calculate_XPower(K3PowerAdd, (K3TimeAdd / 60.0)),
                            K4Time = String.Format("{0:F}", K4TimeAdd / 60.0) + "小时",
                            K4Power = String.Format("{0:F}", K4PowerAdd) + "Kwh",
                            K4XPower = Calculate_XPower(K4PowerAdd, (K4TimeAdd / 60.0)),
                            K5Time = String.Format("{0:F}", K5TimeAdd / 60.0) + "小时",
                            K5Power = String.Format("{0:F}", K5PowerAdd) + "Kwh",
                            K5XPower = Calculate_XPower(K5PowerAdd, (K5TimeAdd / 60.0)),
                            K6Time = String.Format("{0:F}", K6TimeAdd / 60.0) + "小时",
                            K6Power = String.Format("{0:F}", K6PowerAdd) + "Kwh",
                            K6XPower = Calculate_XPower(K6PowerAdd, (K6TimeAdd / 60.0)),
                            K7Time = String.Format("{0:F}", K7TimeAdd / 60.0) + "小时",
                            K7Power = String.Format("{0:F}", K7PowerAdd) + "Kwh",
                            K7XPower = Calculate_XPower(K7PowerAdd, (K7TimeAdd / 60.0)),
                            K8Time = String.Format("{0:F}", K8TimeAdd / 60.0) + "小时",
                            K8Power = String.Format("{0:F}", K8PowerAdd) + "Kwh",
                            K8XPower = Calculate_XPower(K8PowerAdd, (K8TimeAdd / 60.0))
                        });

                        #endregion
                    }
                    else if (IsSelectIndex == 1)
                    {
                        #region
                        var dic = new Dictionary<string, List<Tuple<int, double>>>();

                        foreach (var t in info.Item4)
                        {
                            var dt = new DateTime(t.DateRiqi);
                            if (!dic.ContainsKey(dt.Month.ToString()))
                            {
                                var lst = new List<Tuple<int, double>>();
                                var tu = new Tuple<int, double>(0, 0);

                                for (int i = 0; i < 8; i++)
                                {
                                    lst.Add(tu);
                                }
                                dic.Add(dt.Month.ToString(), lst);
                            }
                            dic[dt.Month.ToString()][t.LoopId - 1] = new Tuple<int, double>(
                                t.TimeMinutes + dic[dt.Month.ToString()][t.LoopId - 1].Item1,
                                t.Powers + dic[dt.Month.ToString()][t.LoopId - 1].Item2);
                        }

                        var dic1 = from objDic in dic orderby objDic.Key select objDic;

                        int id = 1;
                        double K1TimeAdd = 0.0, K2TimeAdd = 0.0, K3TimeAdd = 0.0, K4TimeAdd = 0.0, K5TimeAdd = 0.0, K6TimeAdd = 0.0, K7TimeAdd = 0.0, K8TimeAdd = 0.0;
                        double DayPowerAdd = 0.0, K1PowerAdd = 0.0, K2PowerAdd = 0.0, K3PowerAdd = 0.0, K4PowerAdd = 0.0, K5PowerAdd = 0.0, K6PowerAdd = 0.0, K7PowerAdd = 0.0, K8PowerAdd = 0.0;
                        double DayXPowerAdd = 0;

                        foreach (var t in dic1)
                        {
                            var daypower = t.Value[0].Item2 + t.Value[1].Item2 + t.Value[2].Item2 + t.Value[3].Item2
                                + t.Value[4].Item2 + t.Value[5].Item2 + t.Value[6].Item2 + t.Value[7].Item2;

                            double dayxpower = Calculate_DayXPower(t.Value[0].Item2, (t.Value[0].Item1 / 60.0),
    t.Value[1].Item2, (t.Value[1].Item1 / 60.0),
    t.Value[2].Item2, (t.Value[2].Item1 / 60.0),
    t.Value[3].Item2, (t.Value[3].Item1 / 60.0),
    t.Value[4].Item2, (t.Value[4].Item1 / 60.0),
    t.Value[5].Item2, (t.Value[5].Item1 / 60.0),
    t.Value[6].Item2, (t.Value[6].Item1 / 60.0),
    t.Value[7].Item2, (t.Value[7].Item1 / 60.0));

                            RecordItem.Add(new RecordItemDay()
                            {
                                Id = id,
                                Day = t.Key + "月",
                                DayPower = String.Format("{0:F}", daypower) + "Kwh",
                                DayXPower = String.Format("{0:F}", dayxpower) + "Kw",
                                K1Time = String.Format("{0:F}", t.Value[0].Item1 / 60.0) + "小时",
                                K1Power = String.Format("{0:F}", t.Value[0].Item2) + "Kwh",
                                K1XPower = Calculate_XPower(t.Value[0].Item2, (t.Value[0].Item1 / 60.0)),
                                K2Time = String.Format("{0:F}", t.Value[1].Item1 / 60.0) + "小时",
                                K2Power = String.Format("{0:F}", t.Value[1].Item2) + "Kwh",
                                K2XPower = Calculate_XPower(t.Value[1].Item2, (t.Value[1].Item1 / 60.0)),
                                K3Time = String.Format("{0:F}", t.Value[2].Item1 / 60.0) + "小时",
                                K3Power = String.Format("{0:F}", t.Value[2].Item2) + "Kwh",
                                K3XPower = Calculate_XPower(t.Value[2].Item2, (t.Value[2].Item1 / 60.0)),
                                K4Time = String.Format("{0:F}", t.Value[3].Item1 / 60.0) + "小时",
                                K4Power = String.Format("{0:F}", t.Value[3].Item2) + "Kwh",
                                K4XPower = Calculate_XPower(t.Value[3].Item2, (t.Value[3].Item1 / 60.0)),
                                K5Time = String.Format("{0:F}", t.Value[4].Item1 / 60.0) + "小时",
                                K5Power = String.Format("{0:F}", t.Value[4].Item2) + "Kwh",
                                K5XPower = Calculate_XPower(t.Value[4].Item2, (t.Value[4].Item1 / 60.0)),
                                K6Time = String.Format("{0:F}", t.Value[5].Item1 / 60.0) + "小时",
                                K6Power = String.Format("{0:F}", t.Value[5].Item2) + "Kwh",
                                K6XPower = Calculate_XPower(t.Value[5].Item2, (t.Value[5].Item1 / 60.0)),
                                K7Time = String.Format("{0:F}", t.Value[6].Item1 / 60.0) + "小时",
                                K7Power = String.Format("{0:F}", t.Value[6].Item2) + "Kwh",
                                K7XPower = Calculate_XPower(t.Value[6].Item2, (t.Value[6].Item1 / 60.0)),
                                K8Time = String.Format("{0:F}", t.Value[7].Item1 / 60.0) + "小时",
                                K8Power = String.Format("{0:F}", t.Value[7].Item2) + "Kwh",
                                K8XPower = Calculate_XPower(t.Value[7].Item2, (t.Value[7].Item1 / 60.0))
                            });
                            id++;

                            DayPowerAdd = DayPowerAdd + daypower;
                            DayXPowerAdd = DayXPowerAdd + dayxpower;
                            K1TimeAdd = K1TimeAdd + t.Value[0].Item1;
                            K1PowerAdd = K1PowerAdd + t.Value[0].Item2;
                            K2TimeAdd = K2TimeAdd + t.Value[1].Item1;
                            K2PowerAdd = K2PowerAdd + t.Value[1].Item2;
                            K3TimeAdd = K3TimeAdd + t.Value[2].Item1;
                            K3PowerAdd = K3PowerAdd + t.Value[2].Item2;
                            K4TimeAdd = K4TimeAdd + t.Value[3].Item1;
                            K4PowerAdd = K4PowerAdd + t.Value[3].Item2;
                            K5TimeAdd = K5TimeAdd + t.Value[4].Item1;
                            K5PowerAdd = K5PowerAdd + t.Value[4].Item2;
                            K6TimeAdd = K6TimeAdd + t.Value[5].Item1;
                            K6PowerAdd = K6PowerAdd + t.Value[5].Item2;
                            K7TimeAdd = K7TimeAdd + t.Value[6].Item1;
                            K7PowerAdd = K7PowerAdd + t.Value[6].Item2;
                            K8TimeAdd = K8TimeAdd + t.Value[7].Item1;
                            K8PowerAdd = K8PowerAdd + t.Value[7].Item2;

                            if (id % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                        }

                        RecordItem.Add(new RecordItemDay()
                        {
                            Id = id,
                            Day = "统计" + (id - 1) + "月",
                            DayPower = String.Format("{0:F}", DayPowerAdd) + "Kwh",
                            DayXPower = String.Format("{0:F}", DayXPowerAdd) + "Kw",
                            K1Time = String.Format("{0:F}", K1TimeAdd / 60.0) + "小时",
                            K1Power = String.Format("{0:F}", K1PowerAdd) + "Kwh",
                            K1XPower = Calculate_XPower(K1PowerAdd, (K1TimeAdd / 60.0)),
                            K2Time = String.Format("{0:F}", K2TimeAdd / 60.0) + "小时",
                            K2Power = String.Format("{0:F}", K2PowerAdd) + "Kwh",
                            K2XPower = Calculate_XPower(K2PowerAdd, (K2TimeAdd / 60.0)),
                            K3Time = String.Format("{0:F}", K3TimeAdd / 60.0) + "小时",
                            K3Power = String.Format("{0:F}", K3PowerAdd) + "Kwh",
                            K3XPower = Calculate_XPower(K3PowerAdd, (K3TimeAdd / 60.0)),
                            K4Time = String.Format("{0:F}", K4TimeAdd / 60.0) + "小时",
                            K4Power = String.Format("{0:F}", K4PowerAdd) + "Kwh",
                            K4XPower = Calculate_XPower(K4PowerAdd, (K4TimeAdd / 60.0)),
                            K5Time = String.Format("{0:F}", K5TimeAdd / 60.0) + "小时",
                            K5Power = String.Format("{0:F}", K5PowerAdd) + "Kwh",
                            K5XPower = Calculate_XPower(K5PowerAdd, (K5TimeAdd / 60.0)),
                            K6Time = String.Format("{0:F}", K6TimeAdd / 60.0) + "小时",
                            K6Power = String.Format("{0:F}", K6PowerAdd) + "Kwh",
                            K6XPower = Calculate_XPower(K6PowerAdd, (K6TimeAdd / 60.0)),
                            K7Time = String.Format("{0:F}", K7TimeAdd / 60.0) + "小时",
                            K7Power = String.Format("{0:F}", K7PowerAdd) + "Kwh",
                            K7XPower = Calculate_XPower(K7PowerAdd, (K7TimeAdd / 60.0)),
                            K8Time = String.Format("{0:F}", K8TimeAdd / 60.0) + "小时",
                            K8Power = String.Format("{0:F}", K8PowerAdd) + "Kwh",
                            K8XPower = Calculate_XPower(K8PowerAdd, (K8TimeAdd / 60.0))
                        });

                        #endregion
                    }
                    else if (IsSelectIndex == 2)
                    {
                        #region
                        var dic = new Dictionary<string, List<Tuple<int, double>>>();

                        foreach (var t in info.Item4)
                        {
                            var dt = new DateTime(t.DateRiqi);
                            if (!dic.ContainsKey(dt.Year.ToString("d4")))
                            {
                                var lst = new List<Tuple<int, double>>();
                                var tu = new Tuple<int, double>(0, 0);

                                for (int i = 0; i < 8; i++)
                                {
                                    lst.Add(tu);
                                }
                                dic.Add(dt.Year.ToString("d4"), lst);
                            }
                            dic[dt.Year.ToString("d4")][t.LoopId - 1] = new Tuple<int, double>(
                                t.TimeMinutes + dic[dt.Year.ToString("d4")][t.LoopId - 1].Item1,
                                t.Powers + dic[dt.Year.ToString("d4")][t.LoopId - 1].Item2);
                        }

                        var dic1 = from objDic in dic orderby objDic.Key select objDic;

                        int id = 1;
                        double K1TimeAdd = 0.0, K2TimeAdd = 0.0, K3TimeAdd = 0.0, K4TimeAdd = 0.0, K5TimeAdd = 0.0, K6TimeAdd = 0.0, K7TimeAdd = 0.0, K8TimeAdd = 0.0;
                        double DayPowerAdd = 0.0, K1PowerAdd = 0.0, K2PowerAdd = 0.0, K3PowerAdd = 0.0, K4PowerAdd = 0.0, K5PowerAdd = 0.0, K6PowerAdd = 0.0, K7PowerAdd = 0.0, K8PowerAdd = 0.0;
                        double DayXPowerAdd = 0;

                        foreach (var t in dic1)
                        {
                            var daypower = t.Value[0].Item2 + t.Value[1].Item2 + t.Value[2].Item2 + t.Value[3].Item2
                                + t.Value[4].Item2 + t.Value[5].Item2 + t.Value[6].Item2 + t.Value[7].Item2;
                            var dayxpower = Calculate_DayXPower(t.Value[0].Item2, (t.Value[0].Item1 / 60.0),
t.Value[1].Item2, (t.Value[1].Item1 / 60.0),
t.Value[2].Item2, (t.Value[2].Item1 / 60.0),
t.Value[3].Item2, (t.Value[3].Item1 / 60.0),
t.Value[4].Item2, (t.Value[4].Item1 / 60.0),
t.Value[5].Item2, (t.Value[5].Item1 / 60.0),
t.Value[6].Item2, (t.Value[6].Item1 / 60.0),
t.Value[7].Item2, (t.Value[7].Item1 / 60.0));

                            RecordItem.Add(new RecordItemDay()
                            {
                                Id = id,
                                Day = t.Key + "年",
                                DayPower = String.Format("{0:F}", daypower) + "Kwh",
                                DayXPower = String.Format("{0:F}", dayxpower) + "Kw",
                                K1Time = String.Format("{0:F}", t.Value[0].Item1 / 60.0) + "小时",
                                K1Power = String.Format("{0:F}", t.Value[0].Item2) + "Kwh",
                                K1XPower = Calculate_XPower(t.Value[0].Item2, (t.Value[0].Item1 / 60.0)),
                                K2Time = String.Format("{0:F}", t.Value[1].Item1 / 60.0) + "小时",
                                K2Power = String.Format("{0:F}", t.Value[1].Item2) + "Kwh",
                                K2XPower = Calculate_XPower(t.Value[1].Item2, (t.Value[1].Item1 / 60.0)),
                                K3Time = String.Format("{0:F}", t.Value[2].Item1 / 60.0) + "小时",
                                K3Power = String.Format("{0:F}", t.Value[2].Item2) + "Kwh",
                                K3XPower = Calculate_XPower(t.Value[2].Item2, (t.Value[2].Item1 / 60.0)),
                                K4Time = String.Format("{0:F}", t.Value[3].Item1 / 60.0) + "小时",
                                K4Power = String.Format("{0:F}", t.Value[3].Item2) + "Kwh",
                                K4XPower = Calculate_XPower(t.Value[3].Item2, (t.Value[3].Item1 / 60.0)),
                                K5Time = String.Format("{0:F}", t.Value[4].Item1 / 60.0) + "小时",
                                K5Power = String.Format("{0:F}", t.Value[4].Item2) + "Kwh",
                                K5XPower = Calculate_XPower(t.Value[4].Item2, (t.Value[4].Item1 / 60.0)),
                                K6Time = String.Format("{0:F}", t.Value[5].Item1 / 60.0) + "小时",
                                K6Power = String.Format("{0:F}", t.Value[5].Item2) + "Kwh",
                                K6XPower = Calculate_XPower(t.Value[5].Item2, (t.Value[5].Item1 / 60.0)),
                                K7Time = String.Format("{0:F}", t.Value[6].Item1 / 60.0) + "小时",
                                K7Power = String.Format("{0:F}", t.Value[6].Item2) + "Kwh",
                                K7XPower = Calculate_XPower(t.Value[6].Item2, (t.Value[6].Item1 / 60.0)),
                                K8Time = String.Format("{0:F}", t.Value[7].Item1 / 60.0) + "小时",
                                K8Power = String.Format("{0:F}", t.Value[7].Item2) + "Kwh",
                                K8XPower = Calculate_XPower(t.Value[7].Item2, (t.Value[7].Item1 / 60.0))
                            });
                            id++;

                            DayPowerAdd = DayPowerAdd + daypower;
                            DayXPowerAdd = DayXPowerAdd + dayxpower;
                            K1TimeAdd = K1TimeAdd + t.Value[0].Item1;
                            K1PowerAdd = K1PowerAdd + t.Value[0].Item2;
                            K2TimeAdd = K2TimeAdd + t.Value[1].Item1;
                            K2PowerAdd = K2PowerAdd + t.Value[1].Item2;
                            K3TimeAdd = K3TimeAdd + t.Value[2].Item1;
                            K3PowerAdd = K3PowerAdd + t.Value[2].Item2;
                            K4TimeAdd = K4TimeAdd + t.Value[3].Item1;
                            K4PowerAdd = K4PowerAdd + t.Value[3].Item2;
                            K5TimeAdd = K5TimeAdd + t.Value[4].Item1;
                            K5PowerAdd = K5PowerAdd + t.Value[4].Item2;
                            K6TimeAdd = K6TimeAdd + t.Value[5].Item1;
                            K6PowerAdd = K6PowerAdd + t.Value[5].Item2;
                            K7TimeAdd = K7TimeAdd + t.Value[6].Item1;
                            K7PowerAdd = K7PowerAdd + t.Value[6].Item2;
                            K8TimeAdd = K8TimeAdd + t.Value[7].Item1;
                            K8PowerAdd = K8PowerAdd + t.Value[7].Item2;

                            if (id % 100 == 0) Wlst.Cr.Core.UtilityFunction.UiHelper.UiDoOtherUserEvent();
                        }

                        RecordItem.Add(new RecordItemDay()
                        {
                            Id = id,
                            Day = "统计" + (id - 1) + "年",
                            DayPower = String.Format("{0:F}", DayPowerAdd) + "Kwh",
                            DayXPower = String.Format("{0:F}", DayXPowerAdd) + "Kw",
                            K1Time = String.Format("{0:F}", K1TimeAdd / 60.0) + "小时",
                            K1Power = String.Format("{0:F}", K1PowerAdd) + "Kwh",
                            K1XPower = Calculate_XPower(K1PowerAdd, (K1TimeAdd / 60.0)),
                            K2Time = String.Format("{0:F}", K2TimeAdd / 60.0) + "小时",
                            K2Power = String.Format("{0:F}", K2PowerAdd) + "Kwh",
                            K2XPower = Calculate_XPower(K2PowerAdd, (K2TimeAdd / 60.0)),
                            K3Time = String.Format("{0:F}", K3TimeAdd / 60.0) + "小时",
                            K3Power = String.Format("{0:F}", K3PowerAdd) + "Kwh",
                            K3XPower = Calculate_XPower(K3PowerAdd, (K3TimeAdd / 60.0)),
                            K4Time = String.Format("{0:F}", K4TimeAdd / 60.0) + "小时",
                            K4Power = String.Format("{0:F}", K4PowerAdd) + "Kwh",
                            K4XPower = Calculate_XPower(K4PowerAdd, (K4TimeAdd / 60.0)),
                            K5Time = String.Format("{0:F}", K5TimeAdd / 60.0) + "小时",
                            K5Power = String.Format("{0:F}", K5PowerAdd) + "Kwh",
                            K5XPower = Calculate_XPower(K5PowerAdd, (K5TimeAdd / 60.0)),
                            K6Time = String.Format("{0:F}", K6TimeAdd / 60.0) + "小时",
                            K6Power = String.Format("{0:F}", K6PowerAdd) + "Kwh",
                            K6XPower = Calculate_XPower(K6PowerAdd, (K6TimeAdd / 60.0)),
                            K7Time = String.Format("{0:F}", K7TimeAdd / 60.0) + "小时",
                            K7Power = String.Format("{0:F}", K7PowerAdd) + "Kwh",
                            K7XPower = Calculate_XPower(K7PowerAdd, (K7TimeAdd / 60.0)),
                            K8Time = String.Format("{0:F}", K8TimeAdd / 60.0) + "小时",
                            K8Power = String.Format("{0:F}", K8PowerAdd) + "Kwh",
                            K8XPower = Calculate_XPower(K8PowerAdd, (K8TimeAdd / 60.0))
                        });

                        #endregion
                    }
                }
                else
                {
                    if (IsSelectIndex == 0)
                    {
                    }
                    else if (IsSelectIndex == 1)
                    {
                    }
                    else if (IsSelectIndex == 2)
                    {
                    }
                }
                Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  记录查询成功，共计" + RecordItem.Count + " 条数据.";
            }

            //Remind = "数据已反馈，查询命令已结束，请查看数据！";
        }


        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core);

            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.GroupSelected,
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

                    int id = Convert.ToInt32(args.GetParams()[0]);
                    if (id > 1100000)
                    {
                        var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                        if (tmps == null) return;
                        id = tmps.RtuFid;
                    }
                    if (id < 1000000 || id > 1100000) return;

                    this.RtuId = id;

                }
                else if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.GroupSelected)
                {
                    int id = Convert.ToInt32(args.GetParams()[0]);
                    this.RtuId = id;
                }
            }
            catch (Exception ex)
            {

                ex.ToString();
            }
        }
    }

    /// <summary>
    /// Socket
    /// </summary>
    public partial class RtuOpenCloseLightQueryVm
    {
        private void Query(DateTime dtstarttime, DateTime dtendtime, int tml)
        {
            var tStartTime = new DateTime(dtstarttime.Year, dtstarttime.Month, dtstarttime.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59);


            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_open_close_record;
                // .wlst_cnt_wj3090_request_open_close_light_record ;//.ServerPart.wlst_OpenCloseLight_clinet_request_rtuopencloseLightrecord;
            info.WstRtuOpenCloseRecord.DtEndTime = tEndTime.AddHours(12).Ticks;
            info.WstRtuOpenCloseRecord.DtStartTime = tStartTime.AddHours(-12).Ticks;
            info.WstRtuOpenCloseRecord.Op = 2;
            info.WstRtuOpenCloseRecord.LstRtu.Add(tml);
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在查询...";
        }

        private void Query(DateTime dtstarttime, DateTime dtendtime, List<int> tmls)
        {

            var tStartTime = new DateTime(dtstarttime.Year, dtstarttime.Month, dtstarttime.Day, 0, 0, 1);
            var tEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59);

            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_open_close_record;
                //.ServerPart.wlst_OpenCloseLight_clinet_request_rtuopencloseLightrecord;
            info.WstRtuOpenCloseRecord.DtEndTime = tEndTime.Ticks;
            info.WstRtuOpenCloseRecord.DtStartTime = tStartTime.Ticks;
            info.WstRtuOpenCloseRecord.Op = 3;
            info.WstRtuOpenCloseRecord.LstRtu.AddRange(tmls);
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在查询...";
        }

        private void Query(DateTime dtstarttime, DateTime dtendtime, List<int> tmls, int op)
        {
            var info = Wlst.Sr.ProtocolPhone.LxRtu.wst_rtu_open_close_record;
            var tStartTime = new DateTime();
            var tEndTime = new DateTime();


            if(IsSelectIndex==0)
            {
                tStartTime = new DateTime(dtstarttime.Year, dtstarttime.Month, dtstarttime.Day, 0, 0, 1);
                tEndTime = new DateTime(dtendtime.Year, dtendtime.Month, dtendtime.Day, 23, 59, 59);
            }
            else if (IsSelectIndex == 1)
            {
                tStartTime = new DateTime(dtstarttime.Year, dtstarttime.Month, 1, 0, 0, 1);

                var end = new DateTime(dtendtime.AddMonths(1).Year, dtendtime.AddMonths(1).Month, 1, 0, 0, 1);
                tEndTime = new DateTime(dtendtime.Year, dtendtime.Month, end.AddDays(-1).Day, 23, 59, 59);
            }
            else if (IsSelectIndex == 2)
            {
                tStartTime = new DateTime(dtstarttime.Year, 1, 1, 0, 0, 1);
                tEndTime = new DateTime(dtendtime.Year, 12, 31, 23, 59, 59);
            }


            info.WstRtuOpenCloseRecord.DtEndTime = tEndTime.Ticks;
            info.WstRtuOpenCloseRecord.DtStartTime = tStartTime.Ticks;
            info.WstRtuOpenCloseRecord.Op = 4;//op=4
            info.WstRtuOpenCloseRecord.LstRtu.AddRange(tmls);
            SndOrderServer.OrderSnd(info, 10, 6);
            Remind = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在查询...";
        }
    }



    public partial class RtuOpenCloseLightQueryVm
    {

        private ObservableCollection<ExecuteItem> _records;

        public ObservableCollection<ExecuteItem> RecordTimes
        {
            get
            {
                if (_records == null) _records = new ObservableCollection<ExecuteItem>();
                return _records;
            }
            set
            {
                if (value == _records) return;
                _records = value;
                this.RaisePropertyChanged(() => RecordTimes);
            }
        }
    }

    public partial class RtuOpenCloseLightQueryVm
    {
        private void InitActionTime()
        {
            ProtocolServer.RegistProtocol(
                Wlst.Sr.ProtocolPhone.LxRtuTime.wst_timetable_execute_record,
                // .wlst_svr_ans_cnt_request_timetable_execute_record ,//.ClientPart.wlst_TimeTable_server_ans_clinet_request_timetable_execute_record,
                MyAction, typeof (RtuOpenCloseLightQueryVm), this);
        }



        private void MyAction(string session, Wlst.mobile.MsgWithMobile infos)
        {
            //   Records.Clear();
            RecordTimes.Clear();


            var tmps =
                (from t in infos.WstRtutimeTimetableExecuteRecord.Items orderby t.TimeTableId, t.DateCreate select t).ToList();
            TimeTableExecuteRecord.TimeTableExecuteInfoItem lst = null;
            int index = 0;

            double sum = 0;
            double onesum = 0;
            foreach (var t in tmps)
            {
                index++;

                if (lst == null) lst = t;
                else
                {
                    if (t.TimeTableId == lst.TimeTableId)
                    {
                        if (t.OpenOrClose == 1)
                        {
                            onesum = 0;
                        }
                        else
                        {

                            if (t.OpenOrClose == 2 && lst.OpenOrClose == 1)
                            {
                                onesum = (t.DateCreate - lst.DateCreate)/(60.0*60*10000000);
                                sum += onesum;
                            }
                        }
                    }
                    else
                    {
                        onesum = 0;
                        sum = 0;
                    }
                    lst = t;
                }

                RecordTimes.Add(new ExecuteItem(t)
                                    {Id = index, OneSum = onesum.ToString("f2"), AllSum = sum.ToString("f2")});
            }
            // ShowOrderInfo = "数据已反馈请查看数据！！！";
            ShowOrderInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  记录查询成功，共计" + RecordTimes.Count + " 条数据.";
        }
    }

    /// <summary>
    /// Event
    /// </summary>
    public partial class RtuOpenCloseLightQueryVm
    {
        #region attri

        private DateTime _dtStartTimeTime;

        public DateTime DtStartTimeTime
        {
            get { return _dtStartTimeTime; }
            set
            {
                if (value != _dtStartTimeTime)
                {
                    _dtStartTimeTime = value;
                    this.RaisePropertyChanged(() => this.DtStartTimeTime);

                }
            }
        }

        private DateTime _dtEndTimeTime;

        public DateTime DtEndTimeTime
        {
            get { return _dtEndTimeTime; }
            set
            {
                if (value != _dtEndTimeTime)
                {
                    _dtEndTimeTime = value;
                    this.RaisePropertyChanged(() => this.DtEndTimeTime);
                }
            }
        }


        private void InitTimeTable()
        {
            TimeTableItems.Clear();
            if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.D)
            {
                if (Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.Count == 1)
                {
                    TimeTableItems.Add(new NameIntBool()
                    {
                        IsSelected = false,
                        Name = "全部",
                        Value = 0,
                        AreaId = Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.First().Value.AreaId
                    });
                }
                else
                {
                    foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo)
                    {
                        string area = t.Value.AreaName;
                        TimeTableItems.Add(new NameIntBool() { IsSelected = false, Name = area + "全部", Value = 0, AreaId = t.Value.AreaId });
                    }
                }
            }
            else
            {
                if (Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Count == 1)
                {
                    TimeTableItems.Add(new NameIntBool()
                    {
                        IsSelected = false,
                        Name = "全部",
                        Value = 0,
                        AreaId = Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.First()
                    });
                }
                else
                {
                    foreach (var t in Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR)
                    {
                        if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(t))
                        {
                            string area = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[t].AreaName;
                            TimeTableItems.Add(new NameIntBool() { IsSelected = false, Name = area + "全部", Value = 0, AreaId = t});
                        }
                    }
                }
            }
            

            var tmp =
                (from t in WeekTimeTableInfoService.WeekTimeTableInfoDictionary
                 orderby t.Key.Item1 , t.Key.Item2
                 select t);
            foreach (var t in tmp)
            {
                if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaR.Contains(t.Key.Item1) ||
                    UserInfo.UserLoginInfo.D)
                {
                    TimeTableItems.Add(new NameIntBool()
                                           {
                                               IsSelected = false,
                                               Name = t.Value.TimeName,
                                               Value = t.Value.TimeId,
                                               AreaId = t.Key.Item1
                                           });
                }

            }
            if (TimeTableItems.Count > 0) CurrentSelectTimeTableItem = TimeTableItems[0];
        }

        private ObservableCollection<NameIntBool> _timetableitems;

        public ObservableCollection<NameIntBool> TimeTableItems
        {
            get
            {
                if (_timetableitems == null)
                {
                    _timetableitems = new ObservableCollection<NameIntBool>();
                }
                return _timetableitems;
            }
        }

        private NameIntBool _currentselectopentimetableitem;

        public NameIntBool CurrentSelectTimeTableItem
        {
            get { return _currentselectopentimetableitem; }
            set
            {
                if (value != _currentselectopentimetableitem)
                {
                    _currentselectopentimetableitem = value;
                    this.RaisePropertyChanged(() => this.CurrentSelectTimeTableItem);
                }
            }
        }


        private string _showOrderInfo;

        public string ShowOrderInfo
        {
            get { return _showOrderInfo; }
            set
            {
                if (_showOrderInfo == value) return;
                _showOrderInfo = value;
                RaisePropertyChanged(() => ShowOrderInfo);
            }
        }

        #endregion

        #region ICommand CmdQuery



        private ICommand _cmdAddTimeTable;

        public ICommand CmdQueryTime
        {
            get
            {
                return _cmdAddTimeTable ??
                       (_cmdAddTimeTable = new RelayCommand(ExCmdAddTimeTable, CanCmdAddTimeTable, true));
            }

        }

        private DateTime _dtLastQuery;

        private bool CanCmdAddTimeTable()
        {
            if (DtStartTime.Ticks > DtEndTime.Ticks) return false;


            return DateTime.Now.Ticks - _dtLastQuery.Ticks > 30000000;
        }

        private void ExCmdAddTimeTable()
        {

            if (!GetCheckedTimeInformation()) return;
            this.RecordTimes.Clear();
            var timetableid = 0;
            var areaid = 0;
            if (CurrentSelectTimeTableItem != null)
            {
                timetableid = CurrentSelectTimeTableItem.Value;
                areaid = CurrentSelectTimeTableItem.AreaId;
            }

            var tStartTime = new DateTime(DtStartTimeTime.Year, DtStartTimeTime.Month, DtStartTimeTime.Day, 0, 0, 1);
            var tEndTime = new DateTime(DtEndTimeTime.Year, DtEndTimeTime.Month, DtEndTimeTime.Day, 23, 59, 59);

            var info = Wlst.Sr.ProtocolPhone.LxRtuTime.wst_timetable_execute_record;
                // .wlst_cnt_request_timetable_execute_record ;//.ServerPart.wlst_TimeTable_clinet_request_timetable_execute_record;
            info.WstRtutimeTimetableExecuteRecord.DtEndTime = tEndTime.AddHours(12).Ticks; ;//DtEndTimeTime.Ticks;
            info.WstRtutimeTimetableExecuteRecord.DtStartTime = tStartTime.AddHours(-12).Ticks; ;// DtStartTimeTime.Ticks;
            info.WstRtutimeTimetableExecuteRecord.TimeTableId = timetableid;
            info.WstRtutimeTimetableExecuteRecord.AreaId = areaid;
            // info.WstTimetableExecuteRecord.TimeTableId = timetableid;

            SndOrderServer.OrderSnd(info, 10, 6);
            // ShowOrderInfo = "查询命令已发送...请等待数据反馈！！！";
            ShowOrderInfo = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  正在查询 ...";
        }



        #endregion
    }


    /// <summary>
    /// 日出日落时间 
    /// </summary>
    public partial class RtuOpenCloseLightQueryVm
    {
        #region Records

        private ObservableCollection<SunItem> _recordSun;

        public ObservableCollection<SunItem> RecordSun
        {
            get
            {

                if (_recordSun == null)
                {
                    _recordSun = new ObservableCollection<SunItem>();
                }
                return _recordSun;
            }
        }

        #endregion

        private void InitSun()
        {
            if (RecordSun.Count == 31) return;
            RecordSun.Clear();


            for (int j = 1; j < 32; j++)
            {
                SunItem tmp = new SunItem();
                tmp.Records.Add(new NameValueInt() {Name = j + " "});
                for (int i = 1; i < 13; i++)
                {

                    var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(i, j);
                    if (info != null)
                    {
                        tmp.Records.Add(new NameValueInt()
                                            {
                                                Name =
                                                    (info.time_sunrise/60).ToString("D2") + ":" +
                                                    (info.time_sunrise%60).ToString("D2") + " - " +
                                                    (info.time_sunset/60).ToString("D2") + ":" +
                                                    (info.time_sunset%60).ToString("D2")
                                            });
                    }
                    else
                    {
                        tmp.Records.Add(new NameValueInt()
                                            {
                                                Name = "--:-- - --:--"
                                            });
                    }
                }
                RecordSun.Add(tmp);

            }

        }

        private bool GetCheckedTimeInformation()
        {
            if (DtStartTimeTime.AddDays(720) < DtEndTimeTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在720天以内", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            return true;
        }

        private bool GetCheckedOnOffInformation()
        {
            if (DtStartTime.AddDays(63) < DtEndTime)
            {
                UMessageBox.Show("提醒", "请重新选择时间，时间需选择在62天以内", UMessageBoxButton.Ok);
                //WLSTMessageBox.WpfMessageBox.Show("请重新选择时间，时间需选择在30天以内");
                return false;
            }
            return true;
        }
    }

    public class SunItem
    {
        private ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> _records;

        public ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt> Records
        {
            get
            {
                if (_records == null)
                    _records = new ObservableCollection<Wlst.Cr.CoreOne.Models.NameValueInt>();
                return _records;
            }
        }
    }

    public class RecordItemDay : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    this.RaisePropertyChanged(() => this.Id);
                }
            }
        }


        private string _day;

        public string Day
        {
            get { return _day; }
            set
            {
                if (_day != value)
                {
                    _day = value;
                    this.RaisePropertyChanged(() => this.Day);
                }
            }
        }

        private string _daypower;

        public string DayPower
        {
            get { return _daypower; }
            set
            {
                if (_daypower != value)
                {
                    _daypower = value;
                    this.RaisePropertyChanged(() => this.DayPower);
                }
            }
        }

        private string _dayxpower;

        public string DayXPower
        {
            get { return _dayxpower; }
            set
            {
                if (_dayxpower != value)
                {
                    _dayxpower = value;
                    this.RaisePropertyChanged(() => this.DayXPower);
                }
            }
        }

        private string _k1power;

        public string K1Power
        {
            get { return _k1power; }
            set
            {
                if (value != _k1power)
                {
                    _k1power = value;
                    this.RaisePropertyChanged(() => this.K1Power);
                }
            }
        }

        private string _k1time;

        public string K1Time
        {
            get { return _k1time; }
            set
            {
                if (value != _k1time)
                {
                    _k1time = value;
                    this.RaisePropertyChanged(() => this.K1Time);
                }
            }
        }

        private string _k3power;

        public string K3Power
        {
            get { return _k3power; }
            set
            {
                if (value != _k3power)
                {
                    _k3power = value;
                    this.RaisePropertyChanged(() => this.K3Power);
                }
            }
        }

        private string _k2time;

        public string K2Time
        {
            get { return _k2time; }
            set
            {
                if (value != _k2time)
                {
                    _k2time = value;
                    this.RaisePropertyChanged(() => this.K2Time);
                }
            }
        }

        private string _k2power;

        public string K2Power
        {
            get { return _k2power; }
            set
            {
                if (value != _k2power)
                {
                    _k2power = value;
                    this.RaisePropertyChanged(() => this.K2Power);
                }
            }
        }

        private string _k3time;

        public string K3Time
        {
            get { return _k3time; }
            set
            {
                if (value != _k3time)
                {
                    _k3time = value;
                    this.RaisePropertyChanged(() => this.K3Time);
                }
            }
        }

        private string _k4power;

        public string K4Power
        {
            get { return _k4power; }
            set
            {
                if (value != _k4power)
                {
                    _k4power = value;
                    this.RaisePropertyChanged(() => this.K4Power);
                }
            }
        }

        private string _k4time;

        public string K4Time
        {
            get { return _k4time; }
            set
            {
                if (value != _k4time)
                {
                    _k4time = value;
                    this.RaisePropertyChanged(() => this.K4Time);
                }
            }
        }

        private string _k5power;

        public string K5Power
        {
            get { return _k5power; }
            set
            {
                if (value != _k5power)
                {
                    _k5power = value;
                    this.RaisePropertyChanged(() => this.K5Power);
                }
            }
        }

        private string _k5time;

        public string K5Time
        {
            get { return _k5time; }
            set
            {
                if (value != _k5time)
                {
                    _k5time = value;
                    this.RaisePropertyChanged(() => this.K5Time);
                }
            }
        }

        private string _k6power;

        public string K6Power
        {
            get { return _k6power; }
            set
            {
                if (value != _k6power)
                {
                    _k6power = value;
                    this.RaisePropertyChanged(() => this.K6Power);
                }
            }
        }

        private string _k6time;

        public string K6Time
        {
            get { return _k6time; }
            set
            {
                if (value != _k6time)
                {
                    _k6time = value;
                    this.RaisePropertyChanged(() => this.K6Time);
                }
            }
        }

        private string _k7power;

        public string K7Power
        {
            get { return _k7power; }
            set
            {
                if (value != _k7power)
                {
                    _k7power = value;
                    this.RaisePropertyChanged(() => this.K7Power);
                }
            }
        }

        private string _k7time;

        public string K7Time
        {
            get { return _k7time; }
            set
            {
                if (value != _k7time)
                {
                    _k7time = value;
                    this.RaisePropertyChanged(() => this.K7Time);
                }
            }
        }

        private string _k8power;

        public string K8Power
        {
            get { return _k8power; }
            set
            {
                if (value != _k8power)
                {
                    _k8power = value;
                    this.RaisePropertyChanged(() => this.K8Power);
                }
            }
        }

        private string _k8time;

        public string K8Time
        {
            get { return _k8time; }
            set
            {
                if (value != _k8time)
                {
                    _k8time = value;
                    this.RaisePropertyChanged(() => this.K8Time);
                }
            }
        }

        
        private string _k1xpower;

        public string K1XPower
        {
            get { return _k1xpower; }
            set
            {
                if (value != _k1xpower)
                {
                    _k1xpower = value;
                    this.RaisePropertyChanged(() => this.K1XPower);
                }
            }
        }

        private string _k2xpower;

        public string K2XPower
        {
            get { return _k2xpower; }
            set
            {
                if (value != _k2xpower)
                {
                    _k2xpower = value;
                    this.RaisePropertyChanged(() => this.K2XPower);
                }
            }
        }

        private string _k3xpower;

        public string K3XPower
        {
            get { return _k3xpower; }
            set
            {
                if (value != _k3xpower)
                {
                    _k3xpower = value;
                    this.RaisePropertyChanged(() => this.K3XPower);
                }
            }
        }

        private string _k4xpower;

        public string K4XPower
        {
            get { return _k4xpower; }
            set
            {
                if (value != _k4xpower)
                {
                    _k4xpower = value;
                    this.RaisePropertyChanged(() => this.K4XPower);
                }
            }
        }

        private string _k5xpower;

        public string K5XPower
        {
            get { return _k5xpower; }
            set
            {
                if (value != _k5xpower)
                {
                    _k5xpower = value;
                    this.RaisePropertyChanged(() => this.K5XPower);
                }
            }
        }

        private string _k6xpower;

        public string K6XPower
        {
            get { return _k6xpower; }
            set
            {
                if (value != _k6xpower)
                {
                    _k6xpower = value;
                    this.RaisePropertyChanged(() => this.K6XPower);
                }
            }
        }

        private string _k7xpower;

        public string K7XPower
        {
            get { return _k7xpower; }
            set
            {
                if (value != _k7xpower)
                {
                    _k7xpower = value;
                    this.RaisePropertyChanged(() => this.K7XPower);
                }
            }
        }

        private string _k8xpower;

        public string K8XPower
        {
            get { return _k8xpower; }
            set
            {
                if (value != _k8xpower)
                {
                    _k8xpower = value;
                    this.RaisePropertyChanged(() => this.K8XPower);
                }
            }
        }
    
    }
}

