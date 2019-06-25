using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.EquipmentInfo.EquipmentInfo.LineDetectionInfoViewModel.ViewModel
{
    public partial class LineDetectionInfoViewModel : EventHandlerHelperExtendNotifyProperyChanged
    {

        private ObservableCollection<LineDetectionInfoOneViewModel> _item;

        public ObservableCollection<LineDetectionInfoOneViewModel> Item
        {
            get
            {
                if (_item == null)
                    _item = new ObservableCollection<LineDetectionInfoOneViewModel>();

                return _item;
            }
            set
            {
                if (value == _item) return;
                _item = value;
                this.RaisePropertyChanged(() => this.Item);
            }
        }

        private int _lineDetectionNumber;
        /// <summary>
        /// 线路检测数量
        /// </summary>
        public int LineDetectionNumber
        {
            get { return _lineDetectionNumber; }
            set
            {
                if (_lineDetectionNumber == value) return;
                _lineDetectionNumber = value;
                this.RaisePropertyChanged(() => this.LineDetectionNumber);
            }
        }

        private int _loopNumber;
        /// <summary>
        /// 回路数量
        /// </summary>
        public int LoopNumber
        {
            get { return _loopNumber; }
            set
            {
                if (_loopNumber == value) return;
                _loopNumber = value;
                this.RaisePropertyChanged(() => this.LoopNumber);
            }
        }
    }

    public partial class LineDetectionInfoViewModel
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
            var j = 0;
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.EquipmentType != WjParaBase.EquType.Ldu)
                    continue;
                var x = t.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj1090Ldu;
                if (x == null)
                    continue;
                RequestLineDetectionInformation(x, i);
                if (x.WjLduLines.Count != 0)
                    i = i + x.WjLduLines.Count;
                else
                    i = i + 1;
                j++;
            }
            LoopNumber = i;
            LineDetectionNumber = j;
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
                tabletitle.Add("线路检测地址");
                tabletitle.Add("线路检测名称");
                tabletitle.Add("线路序号");
                tabletitle.Add("线路名称");
                tabletitle.Add("是否使用");
                tabletitle.Add("线路检测回路");
                tabletitle.Add("互感器量程");
                tabletitle.Add("相位");
                tabletitle.Add("主动报警");
                tabletitle.Add("线路短路");
                tabletitle.Add("关灯阻抗");
                tabletitle.Add("关灯脉冲");
                tabletitle.Add("供电变化");
                tabletitle.Add("亮灯率变化");
                tabletitle.Add("开灯阻抗");
                tabletitle.Add("开灯脉冲");
                var table = new List<List<string>>();
                foreach (var g in Item)
                {
                    var tem = new List<string>();
                    tem.Add(g.Index.ToString());
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.LinePhyId.ToString());
                    tem.Add(g.LineDetectionName);
                    tem.Add(g.LineId.ToString());
                    tem.Add(g.LineName);
                    tem.Add(g.IsUsed);
                    tem.Add(g.LoopName);
                    tem.Add(g.MutualInductorRadio.ToString());
                    tem.Add(g.HolderNumber);
                    tem.Add(g.AlarmAutoReport);
                    tem.Add(g.AlarmLineShortCircuit);
                    tem.Add(g.AlarmLineLightOffImpedance);
                    tem.Add(g.AlarmLineLightOffSingle);
                    tem.Add(g.AlarmLineLosePower);
                    tem.Add(g.AlarmLineBrightRate);
                    tem.Add(g.AlarmLineLightOpenImpedance);
                    tem.Add(g.AlarmLineLightOpenSingel);
                    table.Add(tem);
                }
                print.Prints.Print(tabletitle, table, true, "线路检测信息", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
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

    public partial class LineDetectionInfoViewModel
    {
        private void RequestLineDetectionInformation(Wj1090Ldu infos,int i)
        {
            var info = infos.WjLduLines.OrderBy(t => t.Value.LduLineId).ToList();
            if (infos.WjLduLines.Count == 0)
            {
                var tt = new LineDetectionInfoOneViewModel();
                tt.RtuId = infos.RtuFid;
                tt.LinePhyId = infos.RtuId;
                tt.LineDetectionName = infos.RtuName;
                tt.Index = ++i;
                Item.Add(tt);
            }
            else
                foreach (var f in info)
                {
                    var tt = new LineDetectionInfoOneViewModel();
                    tt.RtuId = infos.RtuFid;
                    tt.LinePhyId = infos.RtuId;
                    tt.LineDetectionName = infos.RtuName;
                    tt.Index = ++i;
                    tt.LineId = f.Value.LduLineId;
                    tt.LineName = f.Value.LduLineName;
                    tt.IsUsed = f.Value.IsUsed ? "是" : "否";
                    //if (infos.WjLoops != null)
                    //    foreach (var l in infos.WjLoops.Where(l => f.Value.LduLoopId == l.Value.LoopId))
                    //    {
                    //        tt.LoopName = l.Value.LoopName;
                    //    }
                    tt.MutualInductorRadio = f.Value.MutualInductorRadio;
                    tt.HolderNumber = f.Value.LduPhase==0?"A相":f.Value.LduPhase==1?"B相":"C相";
                    tt.AlarmAutoReport = f.Value.AlarmAutoReport ? "报警" : "不报警";
                    tt.AlarmLineShortCircuit = f.Value.AlarmLineShortCircuit ? "报警" : "不报警";
                    tt.AlarmLineLightOffImpedance = f.Value.AlarmLineLightOffImpedance ? "报警" : "不报警";
                    tt.AlarmLineLightOffSingle = f.Value.AlarmLineLightOffSingle ? "报警" : "不报警";
                    tt.AlarmLineLosePower = f.Value.AlarmLineLosePower ? "报警" : "不报警";
                    tt.AlarmLineBrightRate = f.Value.AlarmLineBrightRate ? "报警" : "不报警";
                    tt.AlarmLineLightOpenImpedance = f.Value.AlarmLineLightOpenImpedance ? "报警" : "不报警";
                    tt.AlarmLineLightOpenSingel = f.Value.AlarmLineLightOpenSingel ? "报警" : "不报警";
                    Item.Add(tt);
                }

        }
    }

    public class LineDetectionInfoOneViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
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

                    foreach (var l in t.WjLoops.Where(l => LineId == l.Value.LoopId))
                    {
                        LoopName = l.Value.LoopName;
                    }
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

        private int _linePhyId;
        /// <summary>
        /// 线路检测地址
        /// </summary>
        public int LinePhyId
        {
            get { return _linePhyId; }
            set
            {
                if (_linePhyId == value) return;
                _linePhyId = value;
                RaisePropertyChanged(() => LinePhyId);
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
                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(RtuId))
                {
                    var tx = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId];
                    var t = tx as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (t == null) return;
                    foreach (var l in t.WjLoops.Where(l => _lineId == l.Value.LoopId))
                    {
                        LoopName = l.Value.LoopName;
                    }
                }
            }
        }

        private string _lineDetectionName;
        /// <summary>
        /// 线路检测名称
        /// </summary>
        public string LineDetectionName
        {
            get { return _lineDetectionName; }
            set
            {
                if (_lineDetectionName == value) return;
                _lineDetectionName = value;
                RaisePropertyChanged(() => LineDetectionName);
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

        private string _loopName;
        /// <summary>
        /// 检测终端回路
        /// </summary>
        public string LoopName
        {
            get { return _loopName; }
            set
            {
                if (_loopName == value) return;
                _loopName = value;
                RaisePropertyChanged(() => LoopName);
            }
        }

        private int _mutualInductorRadio;
        /// <summary>
        /// 互感器量程
        /// </summary>
        public int MutualInductorRadio
        {
            get { return _mutualInductorRadio; }
            set
            {
                if (_mutualInductorRadio == value) return;
                _mutualInductorRadio = value;
                RaisePropertyChanged(() => MutualInductorRadio);
            }
        }

        private string _holderNumber;
        /// <summary>
        /// 相位
        /// </summary>
        public string HolderNumber
        {
            get { return _holderNumber; }
            set
            {
                if (_holderNumber == value) return;
                _holderNumber = value;
                RaisePropertyChanged(() => HolderNumber);
            }
        }

        private string _alarmAutoReport;
        /// <summary>
        /// 主动报警
        /// </summary>
        public string AlarmAutoReport
        {
            get { return _alarmAutoReport; }
            set
            {
                if (_alarmAutoReport == value) return;
                _alarmAutoReport = value;
                RaisePropertyChanged(() => AlarmAutoReport);
            }
        }

        private string _alarmLineShortCircuit;
        /// <summary>
        /// 线路短路
        /// </summary>
        public string AlarmLineShortCircuit
        {
            get { return _alarmLineShortCircuit; }
            set
            {
                if (_alarmLineShortCircuit == value) return;
                _alarmLineShortCircuit = value;
                RaisePropertyChanged(() => AlarmLineShortCircuit);
            }
        }

        private string _alarmLineLightOffImpedance;
        /// <summary>
        /// 关灯阻抗
        /// </summary>
        public string AlarmLineLightOffImpedance
        {
            get { return _alarmLineLightOffImpedance; }
            set
            {
                if (_alarmLineLightOffImpedance == value) return;
                _alarmLineLightOffImpedance = value;
                RaisePropertyChanged(() => AlarmLineLightOffImpedance);
            }
        }

        private string _alarmLineLightOffSingle;
        /// <summary>
        /// 关灯脉冲
        /// </summary>
        public string AlarmLineLightOffSingle
        {
            get { return _alarmLineLightOffSingle; }
            set
            {
                if (_alarmLineLightOffSingle == value) return;
                _alarmLineLightOffSingle = value;
                RaisePropertyChanged(() => AlarmLineLightOffSingle);
            }
        }

        private string _alarmLineLosePower;
        /// <summary>
        /// 供电变化
        /// </summary>
        public string AlarmLineLosePower
        {
            get { return _alarmLineLosePower; }
            set
            {
                if (_alarmLineLosePower == value) return;
                _alarmLineLosePower = value;
                RaisePropertyChanged(() => AlarmLineLosePower);
            }
        }

        private string _alarmLineBrightRate;
        /// <summary>
        /// 亮灯率变化
        /// </summary>
        public string AlarmLineBrightRate
        {
            get { return _alarmLineBrightRate; }
            set
            {
                if (_alarmLineBrightRate == value) return;
                _alarmLineBrightRate = value;
                RaisePropertyChanged(() => AlarmLineBrightRate);
            }
        }

        private string _alarmLineLightOpenImpedance;
        /// <summary>
        /// 开灯阻抗
        /// </summary>
        public string AlarmLineLightOpenImpedance
        {
            get { return _alarmLineLightOpenImpedance; }
            set
            {
                if (_alarmLineLightOpenImpedance == value) return;
                _alarmLineLightOpenImpedance = value;
                RaisePropertyChanged(() => AlarmLineLightOpenImpedance);
            }
        }

        private string _alarmLineLightOpenSingel;
        /// <summary>
        /// 开灯脉冲
        /// </summary>
        public string AlarmLineLightOpenSingel
        {
            get { return _alarmLineLightOpenSingel; }
            set
            {
                if (_alarmLineLightOpenSingel == value) return;
                _alarmLineLightOpenSingel = value;
                RaisePropertyChanged(() => AlarmLineLightOpenSingel);
            }
        }
    }
}
