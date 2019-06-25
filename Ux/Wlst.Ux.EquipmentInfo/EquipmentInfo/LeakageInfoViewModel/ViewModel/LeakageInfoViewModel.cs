using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.EquipmentInfo.EquipmentInfo.LeakageInfoViewModel.ViewModel
{
    public partial class LeakageInfoViewModel : EventHandlerHelperExtendNotifyProperyChanged
    {
        private ObservableCollection<LeakageInfoOneViewModel> _item;

        public ObservableCollection<LeakageInfoOneViewModel> Item
        {
            get
            {
                if (_item == null)
                    _item = new ObservableCollection<LeakageInfoOneViewModel>();

                return _item;
            }
            set
            {
                if (value == _item) return;
                _item = value;
                this.RaisePropertyChanged(() => this.Item);
            }
        }

        private int _leakageNumber;
        /// <summary>
        /// 漏电数量
        /// </summary>
        public int LeakageNumber
        {
            get { return _leakageNumber; }
            set
            {
                if (_leakageNumber == value) return;
                _leakageNumber = value;
                this.RaisePropertyChanged(() => this.LeakageNumber);
            }
        }

        private int _lineNumber;
        /// <summary>
        /// 总线路数量
        /// </summary>
        public int LineNumber
        {
            get { return _lineNumber; }
            set
            {
                if (_lineNumber == value) return;
                _lineNumber = value;
                this.RaisePropertyChanged(() => this.LineNumber);
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
    }

    public partial class LeakageInfoViewModel
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
            Item.Clear();
            var i = 0;
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.EquipmentType != WjParaBase.EquType.Leak)
                    continue;
                var x = t.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj9001Leak;
                if (x == null)
                    continue;
                RequestLeakageInformation(x, i);
                if (x.WjLeakLines.Count != 0)
                    i = i + x.WjLeakLines.Count;
                else
                    i = i + 1;
                foreach (var f in x.WjLeakLines)
                {
                    if (f.Value.IsUsed)
                        UsedLineNumber++;                  
                }
                LeakageNumber++;
            }
            LineNumber = i;
        }
        private bool CanExCmdQuery()
        {
            return DateTime.Now.Ticks - _dtCmdQuery.Ticks > 30000000;
        }
        #endregion

        //打印
        #region CmdPrint
        private DateTime _dtCmdPrint;
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
            _dtCmdPrint = DateTime.Now;
            try
            {
                var tabletitle = new List<string>();
                tabletitle.Add("序号");
                tabletitle.Add("终端地址");
                tabletitle.Add("终端名称");
                tabletitle.Add("漏电地址");
                tabletitle.Add("漏电名称");
                tabletitle.Add("线路序号");
                tabletitle.Add("线路名称");
                tabletitle.Add("是否使用");
                tabletitle.Add("是否分闸");
                tabletitle.Add("上限设置(mA/℃)");
                tabletitle.Add("动作延迟(ms)");

                var table = new List<List<string>>();
                foreach (var g in Item)
                {
                    var tem = new List<string>();
                    tem.Add(g.Index.ToString());
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.LeakPhyId.ToString());
                    tem.Add(g.LeakName);
                    tem.Add(g.LineId.ToString());
                    tem.Add(g.LineName);
                    tem.Add(g.IsUsed);
                    tem.Add(g.AutoBreakOrAutoAlarm);
                    tem.Add(g.UpperAlarmOrBreakforLeakOrTemperature.ToString());
                    tem.Add(g.TimeDelayforBreak.ToString());
                    table.Add(tem);
                }
                print.Prints.Print(tabletitle, table, true, "漏电设备信息", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
            }
            catch (Exception)
            {

                throw;
            }
        }

        private bool CanExPrint()
        {
            if (Item.Count < 1) return false;
            return DateTime.Now.Ticks - _dtCmdPrint.Ticks > 30000000;
        }
        #endregion
    }

    public partial class LeakageInfoViewModel
    {
        private void RequestLeakageInformation(Wj9001Leak infos,int i)
        {
            var info = infos.WjLeakLines.OrderBy(t => t.Value.LeakLineId).ToList();
            if(infos.WjLeakLines.Count==0)
            {
                var tt = new LeakageInfoOneViewModel();
                tt.RtuId = infos.RtuFid;
                tt.LeakPhyId = infos.RtuId;
                tt.LeakName = infos.RtuName;
                tt.Index = ++i;
                Item.Add(tt);
            }
            else
                foreach (var f in info)
                {
                    var tt = new LeakageInfoOneViewModel();
                    tt.Index = ++i;
                    tt.RtuId = infos.RtuFid;
                    tt.LeakPhyId = infos.RtuId;
                    tt.LeakName = infos.RtuName;
                    tt.LineId = f.Value.LeakLineId;
                    tt.LineName = f.Value.LineName;
                    tt.IsUsed = f.Value.IsUsed ? "是" : "否";
                    tt.AutoBreakOrAutoAlarm = f.Value.AutoBreakOrAutoAlarm==1 ? "是" : "否";
                    tt.UpperAlarmOrBreakforLeakOrTemperature = f.Value.UpperAlarmOrBreakforLeakOrTemperature;
                    tt.TimeDelayforBreak = f.Value.TimeDelayforBreak;
                    Item.Add(tt);
                }
        }
    }

    public class LeakageInfoOneViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private int _index;
        /// <summary>
        /// 序号
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                if (_index == value) return;
                _index = value;
                this.RaisePropertyChanged(() => this.Index);
            }
        }

        private int _phyId;
        /// <summary>
        /// 终端物理地址
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

        private int _rtuId;
        /// <summary>
        /// 终端逻辑地址
        /// </summary>
        public int RtuId
        {
            get { return _rtuId; }
            set
            {
                if (_rtuId == value) return;
                _rtuId = value;
                this.RaisePropertyChanged(() => this.RtuId);
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(_rtuId))
                    return;

                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(_rtuId))
                {
                    var tx = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[_rtuId];
                    var t = tx as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    RtuName = t.RtuName;
                    PhyId = t.RtuPhyId;
                }
            }
        }

        private string _rtuName;
        /// <summary>
        /// 终端名称
        /// </summary>
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

        private int _leakPhyId;
        /// <summary>
        /// 漏电地址
        /// </summary>
        public int LeakPhyId
        {
            get { return _leakPhyId; }
            set
            {
                if (_leakPhyId == value) return;
                _leakPhyId = value;
                RaisePropertyChanged(() => LeakPhyId);
            }
        }

        private string _leakName;
        /// <summary>
        /// 漏电名称
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

        private int _lineId;
        /// <summary>
        /// 线路序号
        /// </summary>
        public int LineId
        {
            get { return _lineId; }
            set
            {
                if (_lineId == value) return;
                _lineId = value;
                RaisePropertyChanged(() => LineId);
            }
        }

        private string _lineName;
        /// <summary>
        /// 线路名称
        /// </summary>
        public string LineName
        {
            get { return _lineName; }
            set
            {
                if (_lineName == value) return;
                _lineName = value;
                RaisePropertyChanged(() => LineName);
            }
        }

        private string _isUsed;
        /// <summary>
        /// 是否使用
        /// </summary>
        public string IsUsed
        {
            get { return _isUsed; }
            set
            {
                if (_isUsed == value) return;
                _isUsed = value;
                RaisePropertyChanged(() => IsUsed);
            }
        }

        private string _autoBreakOrAutoAlarm;
        /// <summary>
        /// 是否分闸
        /// </summary>
        public string AutoBreakOrAutoAlarm
        {
            get { return _autoBreakOrAutoAlarm; }
            set
            {
                if (_autoBreakOrAutoAlarm == value) return;
                _autoBreakOrAutoAlarm = value;
                RaisePropertyChanged(() => AutoBreakOrAutoAlarm);
            }
        }

        private int _upperAlarmOrBreakforLeakOrTemperature;
        /// <summary>
        /// 上限设置
        /// </summary>
        public int UpperAlarmOrBreakforLeakOrTemperature
        {
            get { return _upperAlarmOrBreakforLeakOrTemperature; }
            set
            {
                if (_upperAlarmOrBreakforLeakOrTemperature == value) return;
                _upperAlarmOrBreakforLeakOrTemperature = value;
                RaisePropertyChanged(() => UpperAlarmOrBreakforLeakOrTemperature);
            }
        }

        private int _timeDelayforBreak;
        /// <summary>
        /// 动作延迟
        /// </summary>
        public int TimeDelayforBreak
        {
            get { return _timeDelayforBreak; }
            set
            {
                if (_timeDelayforBreak == value) return;
                _timeDelayforBreak = value;
                RaisePropertyChanged(() => TimeDelayforBreak);
            }
        }
    }
}
