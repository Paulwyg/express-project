using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.EquipmentInfo.EquipmentInfo.ElectricMeterInfoViewModel.ViewModel
{
    public partial class ElectricMeterInfoViewModel : EventHandlerHelperExtendNotifyProperyChanged
    {

        private ObservableCollection<ElectricMeterInfoOneViewModel> _item;

        public ObservableCollection<ElectricMeterInfoOneViewModel> Item
        {
            get
            {
                if (_item == null)
                    _item = new ObservableCollection<ElectricMeterInfoOneViewModel>();

                return _item;
            }
            set
            {
                if (value == _item) return;
                _item = value;
                this.RaisePropertyChanged(() => this.Item);
            }
        }

        private int _electricMeterNumber;
        /// <summary>
        /// 电表数量
        /// </summary>
        public int ElectricMeterNumber
        {
            get { return _electricMeterNumber; }
            set
            {
                if (_electricMeterNumber == value) return;
                _electricMeterNumber = value;
                this.RaisePropertyChanged(() => this.ElectricMeterNumber);
            }
        }

       
    }

    public partial class ElectricMeterInfoViewModel
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
                if (t.Value.EquipmentType != WjParaBase.EquType.Mru)
                    continue;
                var x = t.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj1050Mru;
                if (x == null)
                    continue;
                i++;
                RequestElectricMeterInformation(x, i);
            }
            ElectricMeterNumber = i;

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
                tabletitle.Add("电表地址");
                tabletitle.Add("电表名称");
                tabletitle.Add("备注");
                tabletitle.Add("电表协议");
                tabletitle.Add("电表变比");
                tabletitle.Add("波特率");
                tabletitle.Add("电表地址");
                var table = new List<List<string>>();
                foreach (var g in Item)
                {
                    var tem = new List<string>();
                    tem.Add(g.Index.ToString());
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.RtuName);
                    tem.Add(g.MruId.ToString());
                    tem.Add(g.MruName);
                    tem.Add(g.MruRemark);
                    tem.Add(g.Protocol);
                    tem.Add(g.MruRatio.ToString());
                    tem.Add(g.MruBandRate.ToString());
                    tem.Add(g.MruAddr);
                    table.Add(tem);
                }
                print.Prints.Print(tabletitle, table, true, "电表设备信息", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
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

    public partial class ElectricMeterInfoViewModel
    {
        private void RequestElectricMeterInformation(Wj1050Mru info,int i)
        {
            var tt = new ElectricMeterInfoOneViewModel();
            tt.Index = i;
            tt.MruId = info.RtuId;
            tt.MruName = info.RtuName;
            tt.MruRemark = info.RtuRemark;
            tt.Protocol = info.WjMru.MruType == 1 ? "97" : "07";
            tt.MruRatio = info.WjMru.MruRatio;
            tt.MruBandRate = info.WjMru.MruBaudrate;
            tt.MruAddr = System.Convert.ToString(info.WjMru.MruAddr1, 16).Trim().PadLeft(2, '0') + " " +
                         System.Convert.ToString(info.WjMru.MruAddr2, 16).Trim().PadLeft(2, '0') + " " +
                         System.Convert.ToString(info.WjMru.MruAddr3, 16).Trim().PadLeft(2, '0') + " " +
                         System.Convert.ToString(info.WjMru.MruAddr4, 16).Trim().PadLeft(2, '0') + " " +
                         System.Convert.ToString(info.WjMru.MruAddr5, 16).Trim().PadLeft(2, '0') + " " +
                         System.Convert.ToString(info.WjMru.MruAddr6, 16).Trim().PadLeft(2, '0');
            Item.Add(tt);
        }
    }

    public class ElectricMeterInfoOneViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
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
                if (value != _rtuName)
                {
                    _rtuName = value;
                    this.RaisePropertyChanged(() => this.RtuName);
                }
            }
        }

        private int _mruId;

        /// <summary>
        /// 电表地址  
        /// </summary>
        public int MruId
        {
            get { return _mruId; }
            set
            {
                if (_mruId.Equals(value)) return;
                _mruId = value;

                RaisePropertyChanged(() => RtuId);
                int fid = 0;
                var ddd =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                if (ddd != null)
                {
                    RtuName =  ddd.RtuName;
                    fid = ddd.RtuFid;
                }
                var ggg =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(fid);
                if (ggg != null)
                {
                    this.RtuId = ggg.RtuId;
                    this.PhyId = ggg.RtuPhyId;
                    this.RtuName = ggg.RtuName;
                }
            }
        }

        private string _mruName;

        /// <summary>
        /// 电表名称
        /// </summary>
        public string MruName
        {
            get { return _mruName; }
            set
            {
                if (value != _mruName)
                {
                    _mruName = value;
                    this.RaisePropertyChanged(() => this.MruName);
                }
            }
        }

        private string _mruRemark;

        /// <summary>
        /// 备注
        /// </summary>
        public string MruRemark
        {
            get { return _mruRemark; }
            set
            {
                if (value != _mruRemark)
                {
                    _mruRemark = value;
                    this.RaisePropertyChanged(() => this.MruRemark);
                }
            }
        }

        private string _protocol;

        /// <summary>
        /// 备注
        /// </summary>
        public string Protocol
        {
            get { return _protocol; }
            set
            {
                if (value != _protocol)
                {
                    _protocol = value;
                    this.RaisePropertyChanged(() => this.Protocol);
                }
            }
        }

        private int _mruRatio;

        /// <summary>
        /// 电表变比
        /// </summary>
        public int MruRatio
        {
            get { return _mruRatio; }
            set
            {
                if (value != _mruRatio)
                {
                    _mruRatio = value;
                    this.RaisePropertyChanged(() => this.MruRatio);
                }
            }
        }

        private int _mruBandRate;

        /// <summary>
        /// 电表波特率
        /// </summary>
        public int MruBandRate
        {
            get { return _mruBandRate; }
            set
            {
                if (value != _mruBandRate)
                {
                    _mruBandRate = value;
                    this.RaisePropertyChanged(() => this.MruBandRate);
                }
            }
        }

        private string _mruAddr;

        /// <summary>
        /// 电表地址
        /// </summary>
        public string MruAddr
        {
            get { return _mruAddr; }
            set
            {
                if (value != _mruAddr)
                {
                    _mruAddr = value;
                    this.RaisePropertyChanged(() => this.MruAddr);
                }
            }
        }
    }
}
