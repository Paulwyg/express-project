using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.client;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.LeakageViewModel.ViewModel
{
    public partial class LeakageViewModel : EventHandlerHelperExtendNotifyProperyChanged
    {
        private RadCartesianChart _radchart1 = null;
        public LeakageViewModel(RadCartesianChart radchart)
        {
            BeginDate = DateTime.Now.AddDays(-7);
            EndDate = DateTime.Now;
            InitAction();
            InitEvent();
            _radchart1 = radchart;
            _radchart1.Series.Clear();
            var itemss = new ObservableCollection<LeakQueryoneItemViewModel>();
            //var line = new LineSeries();
            //for (var i = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month,
            //                              BeginDate.AddDays(-1).Day, 12, 0, 0);
            //    i < new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 12, 0, 1); )
            for (var i = new DateTime(BeginDate.Year, BeginDate.Month,
                    BeginDate.Day, 12, 0, 0);
                i < new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 12, 0, 1); )
            {
                itemss.Add(new LeakQueryoneItemViewModel()
                               {
                                   DateCreate = i,
                                   Value = 0
                               });
                //line.DataPoints.Add(new CategoricalDataPoint() { Value = 0, Category = i });
                //_radchart1.Series.Add(line);
                i = i.AddHours(2);
            }
            AddLines(itemss, new SolidColorBrush(Colors.White));
            Step1 = 1;
            LeakId = 1600001;
        }

        #region LeakName
        private string _leakName;
        /// <summary>
        /// 漏电设备名称
        /// </summary>
        public string LeakName
        {
            get { return _leakName; }
            set
            {
                if (_leakName == value) return;
                _leakName = value;
                RaisePropertyChanged(() => LeakName);
            }
        }

        #endregion

        #region LeakId

        private int _phyId;

        /// <summary>
        /// 漏电设备物理地址
        /// </summary>
        public int PhyId
        {
            get { return _phyId; }
            set
            {
                if (_phyId == value) return;
                _phyId = value;
                RaisePropertyChanged(() => PhyId);
            }
        }


        private int _relatedRtuId;

        /// <summary>
        /// 关联主设备地址
        /// </summary>
        public int RelatedRtuId
        {
            get { return _relatedRtuId; }
            set
            {
                if (_relatedRtuId == value) return;
                _relatedRtuId = value;
                RaisePropertyChanged(() => RelatedRtuId);
                foreach (var t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    var x = t.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (x == null) continue;
                    if (x.RtuPhyId == _relatedRtuId)
                    {
                        this.RelatedRtuName = x.RtuName;
                        break;
                    }
                }
            }
        }

        private string _relatedRtuName;
        /// <summary>
        /// 关联终端名称
        /// </summary>
        public string RelatedRtuName
        {
            get { return _relatedRtuName; }
            set
            {
                if (_relatedRtuName == value) return;
                _relatedRtuName = value;
                RaisePropertyChanged(() => RelatedRtuName);
            }
        }

        private int _leakId;

        /// <summary>
        /// 漏电设备逻辑地址
        /// </summary>
        public int LeakId
        {
            get { return _leakId; }
            set
            {
                if (_leakId == value) return;
                _leakId = value;
                RaisePropertyChanged(() => LeakId);

                var tmps = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                if (tmps != null)
                    LeakName = tmps.RtuName;
                else
                    LeakName = "";

                var sss = tmps as Wj9001Leak;
                if (sss == null)
                    return;
                //int number = sss.WjLeakLines.Count(f => f.Value.IsUsed);
                int number = sss.WjLeakLines.Count;
                UsedLineNumber = number;
                RelatedRtuId = sss.RtuFid;
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(sss.RtuFid))
                    return;

                var tx = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[sss.RtuFid];
                var t = tx as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (t == null) return;
                RelatedRtuName = t.RtuName;
                RelatedRtuId = t.RtuPhyId;
            }
        }

        private int _usedLineNumber;
        /// <summary>
        /// 使用线路数量
        /// </summary>
        public int UsedLineNumber
        {
            get { return _usedLineNumber; }
            set
            {
                if (_usedLineNumber == value) return;
                _usedLineNumber = value;
                this.RaisePropertyChanged(() => this.UsedLineNumber);
            }
        }
        #endregion

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

        #region EndDate
        private DateTime _endDate;
        /// <summary>
        /// 查询结束时间
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (value == _endDate) return;
                _endDate = value;
                RaisePropertyChanged(() => EndDate);
            }
        }
        #endregion


        #region Items

        private ObservableCollection<LeakQueryoneItemViewModel> _items;

        public ObservableCollection<LeakQueryoneItemViewModel> Items
        {
            get { return _items ?? (_items = new ObservableCollection<LeakQueryoneItemViewModel>()); }
            set
            {
                if (value == _items) return;
                _items = value;
                this.RaisePropertyChanged(() => Items);
            }
        }

        #endregion

        #region LoopItems
        private ObservableCollection<LoopOneItems> _loopItems;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<LoopOneItems> LoopItems
        {
            get { return _loopItems ?? (_loopItems = new ObservableCollection<LoopOneItems>()); }
            set
            {
                if (value == _loopItems) return;
                _loopItems = value;
                this.RaisePropertyChanged(() => LoopItems);
            }
        }
        #endregion

        #region Step1
        private int _step1;

        /// <summary>
        /// 
        /// </summary>
        public int Step1
        {
            get { return _step1; }
            set
            {
                if (_step1 == value) return;
                _step1 = value;
                RaisePropertyChanged(() => Step1);
            }
        }
        #endregion
    }

    public partial class LeakageViewModel
    {
        //查询
        #region CmdQuery
        private DateTime _dtCmdQuery;
        private ICommand _cmdCmdQuery;

        public ICommand CmdQuery
        {
            get
            {
                if (_cmdCmdQuery == null)
                    _cmdCmdQuery = new RelayCommand(ExCmdQuery, CanExCmdQuery, false);
                return _cmdCmdQuery;
            }
        }

        private void ExCmdQuery()
        {
            _dtCmdQuery = DateTime.Now;
            AddLinkName();
            RequestLeak();
        }
        private bool CanExCmdQuery()
        {
            return DateTime.Now.Ticks - _dtCmdQuery.Ticks > 30000000 && LeakId != 0;
        }
        #endregion

        private void AddLinkName()
        {
            var tmps = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(LeakId);
            var sss = tmps as Wj9001Leak;
            if (sss == null)
                return;
            var color = new List<Brush>
                            {
                                new SolidColorBrush(Colors.Orange),
                                new SolidColorBrush(Colors.Blue),
                                new SolidColorBrush(Colors.Brown),
                                new SolidColorBrush(Colors.BlueViolet),
                                new SolidColorBrush(Colors.DarkGreen),
                                new SolidColorBrush(Colors.DeepPink),
                                new SolidColorBrush(Colors.DimGray),
                                new SolidColorBrush(Colors.OrangeRed)
                            };
            int i = 0;
            var loopitems = new ObservableCollection<LoopOneItems>();
            foreach (var f in sss.WjLeakLines)
            {
                if(f.Value.LineName.Contains("温度检测")) continue;
                if(f.Value.IsUsed ==false)continue;
                loopitems.Add(new LoopOneItems()
                {
                    Color = color[i],
                    Name = f.Value.LineName,
                    Value = f.Value.LeakLineId
                });
                i++;
            }
            LoopItems = loopitems;
        }

        private void RequestLeak()
        {
            var tStartTime = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 12, 0, 1);
            var tEndTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 12, 0, 0);

            var info = Sr.ProtocolPhone.LxLeak.wst_leak_datas;
            info.WstLeakDatas.RtuId = LeakId;
            info.WstLeakDatas.DtStartTime = tStartTime.Ticks;
            info.WstLeakDatas.DtEndTime = tEndTime.Ticks;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
    }

    public partial class LeakageViewModel
    {
        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
               Sr.ProtocolPhone.LxLeak.wst_leak_datas,
               OnRequestLeak,
               typeof(LeakageViewModel), this, true);
        }

        private RequestLeakData infoss = new RequestLeakData();
        private void OnRequestLeak(string session, Wlst.mobile.MsgWithMobile infos)
        {
            var st = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 12, 0, 0);
            var et = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 12, 0, 1);
            var line1 = new LineSeries();
            _radchart1.Series.Clear();
            for (var j = st; j < et; )
            {
                line1.DataPoints.Add(new CategoricalDataPoint() {Value = 0, Category = j});
                j = j.AddHours(2);
            }
            _radchart1.Series.Add(line1);

            var info = infos.WstLeakDatas; //WstLduHisData  ;
            infoss = info;
            if (info.Items.Count == 0) return;
            _radchart1.Series.Clear();
            int xx = 5;
            var maxtime = info.Items[0].Items[0].DateCreate;
            var mintime = info.Items[0].Items[0].DateCreate;
            foreach (var x in LoopItems)
            {
                var items = new ObservableCollection<LeakQueryoneItemViewModel>();
                foreach (LeakNewData item in info.Items)
                {
                    foreach (var f in item.Items)
                    {
                        if (x.Value == f.LeakLineId)
                        {
                            items.Add(new LeakQueryoneItemViewModel()
                                          {
                                              DateCreate = new DateTime(f.DateCreate),
                                              Value = f.CurrentLeakOrTemperature
                                          });
                            if (maxtime < f.DateCreate) maxtime = f.DateCreate;
                            if (mintime > f.DateCreate) mintime = f.DateCreate;
                        }
                    }

                }

                AddLines(items, x.Color);
                if (items.Select(t => t.Value).Concat(new[] { 1 }).Max() > xx)
                    xx = items.Select(t => t.Value).Concat(new[] { 1 }).Max();

            }

            var line = new LineSeries();
            for (var j = st; j < et; )
            {
                if (j.Ticks < mintime || j.Ticks > maxtime)
                    line.DataPoints.Add(new CategoricalDataPoint() {Value = 0, Category = j});
                j = j.AddHours(2);
            }
            _radchart1.Series.Add(line);
            Step1 = xx / 5 > 1 ? xx / 5 : 1;
        }

        public void AddLines(ObservableCollection<LeakQueryoneItemViewModel> oit, Brush color)
        {
            var line = new LineSeries();
            line.Stroke = color;
            line.StrokeThickness = 2;
            foreach (var f in oit)
                line.DataPoints.Add(new CategoricalDataPoint() { Value = f.Value, Category = f.DateCreate });
            _radchart1.Series.Add(line);
        }

        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core,true);
            this.AddEventFilterInfo(6666, "LoopOneItems", true);
        }
        public override void ExPublishedEvent(
           PublishEventArgs args)
        {

            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {

                    int id = Convert.ToInt32(args.GetParams()[0]);
                    var mid = id;
                    if (id < 1100000)
                    {
                        var tml = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[id];
                        foreach (var t in tml.EquipmentsThatAttachToThisRtu)
                        {
                            if (t > 1600000 && t < 1700000)
                            {
                                mid = t;
                                break;
                            }
                        }
                    //    var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                    //    if (tmps == null) return;
                    //    id = tmps.RtuFid;
                    }
                    if (mid < 1600000 || mid > 1700000) return;

                    SelectRtuIdChange(mid);
                }

                if (args.EventId == 6666 && args.EventType == "LoopOneItems")
                {
                    if (infoss.Items.Count == 0) return;
                    _radchart1.Series.Clear();
                    
                    int xx = 5;
                    var maxtime = infoss.Items[0].Items[0].DateCreate;
                    var mintime = infoss.Items[0].Items[0].DateCreate;
                    foreach (var f in LoopItems)
                    {
                        var items = new ObservableCollection<LeakQueryoneItemViewModel>();
                        if (f.IsSelected == false) continue;
                        foreach (var t in infoss.Items)
                        {
                            foreach (var tt in t.Items)
                            {
                                if (tt.LeakLineId == f.Value)
                                {
                                    items.Add(new LeakQueryoneItemViewModel()
                                                  {
                                                      DateCreate = new DateTime(tt.DateCreate),
                                                      Value = tt.CurrentLeakOrTemperature
                                                  });
                                    if (maxtime < tt.DateCreate) maxtime = tt.DateCreate;
                                    if (mintime > tt.DateCreate) mintime = tt.DateCreate;
                                }
                            }
                        }
                        AddLines(items, f.Color);
                        if (items.Select(t => t.Value).Concat(new[] { 1 }).Max() > xx)
                            xx = items.Select(t => t.Value).Concat(new[] { 1 }).Max();
                    }

                    for (var j = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month,
                                              BeginDate.AddDays(-1).Day, 12, 0, 0);
                         j < new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 12, 0, 1); )
                    {
                        if (j.Ticks < mintime || j.Ticks > maxtime)
                        {
                            var line = new LineSeries();
                            line.DataPoints.Add(new CategoricalDataPoint() {Value = 0, Category = j});
                            _radchart1.Series.Add(line);
                        }
                        j = j.AddHours(2);
                    }
                    Step1 = xx / 5 > 1 ? xx / 5 : 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 选中终端变化  提取数据
        /// </summary>
        /// <param name="rtuId"></param>
        private void SelectRtuIdChange(int rtuId)
        {
            if (rtuId < 1) return;
            if (rtuId != this.LeakId)
            {
                this.LeakId = rtuId;

                _radchart1.Series.Clear();
                var itemss = new ObservableCollection<LeakQueryoneItemViewModel>();
                for (var i = new DateTime(BeginDate.Year, BeginDate.Month,
                        BeginDate.Day, 12, 0, 0);
                    i < new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 12, 0, 1); )
                {
                    itemss.Add(new LeakQueryoneItemViewModel()
                    {
                        DateCreate = i,
                        Value = 0
                    });
                    i = i.AddHours(2);
                }
                AddLines(itemss, new SolidColorBrush(Colors.White));
                Step1 = 1;
            }
        }
    }

    public class LeakQueryoneItemViewModel:EventHandlerHelperExtendNotifyProperyChanged
    {
        #region DateCreate

        private DateTime _dateCreate;
        public DateTime DateCreate
        {
            get { return _dateCreate; }
            set
            {
                if (_dateCreate == value) return;
                _dateCreate = value;
                RaisePropertyChanged(() => DateCreate);
            }
        }

        #endregion

        #region Value

        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        #endregion
    }

    public class LoopOneItems:EventHandlerHelperExtendNotifyProperyChanged
    {
        private bool _check = true;
        public bool IsSelected
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    EventPublish.PublishEvent(new PublishEventArgs()
                    {
                        EventId = 6666,
                        EventType = "LoopOneItems"
                    });

                }
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

        private int _value;
        /// <summary>
        /// 
        /// </summary>
        public int Value
        {
            get { return _value; }
            set
            {
                if (value == _value) return;
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        private Brush _color;
        /// <summary>
        /// 背景色
        /// </summary>
        public Brush Color
        {
            get { return _color; }
            set
            {
                if (value != _color)
                {
                    _color = value;
                    RaisePropertyChanged(() => Color);
                }
            }
        }
    }
}
