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

namespace Wlst.Ux.EquipmentInfo.EquipmentInfo.SingleLampInfoViewModel.ViewModel
{
    public partial class SingleLampInfoViewModel : EventHandlerHelperExtendNotifyProperyChanged
    {

        private ObservableCollection<SingleLampInfoOneViewModel> _item;

        public ObservableCollection<SingleLampInfoOneViewModel> Item
        {
            get
            {
                if (_item == null)
                    _item = new ObservableCollection<SingleLampInfoOneViewModel>();

                return _item;
            }
            set
            {
                if (value == _item) return;
                _item = value;
                this.RaisePropertyChanged(() => this.Item);
            }
        }

        private int _concentratorNumber;
        /// <summary>
        /// 单灯设备数量
        /// </summary>
        public int ConcentratorNumber
        {
            get { return _concentratorNumber; }
            set
            {
                if (_concentratorNumber == value) return;
                _concentratorNumber = value;
                this.RaisePropertyChanged(() => this.ConcentratorNumber);
            }
        }

        private int _controllerNumber;
        /// <summary>
        /// 控制器数量
        /// </summary>
        public int ControllerNumber
        {
            get { return _controllerNumber; }
            set
            {
                if (_controllerNumber == value) return;
                _controllerNumber = value;
                this.RaisePropertyChanged(() => this.ControllerNumber);
            }
        }


    }

    public partial class SingleLampInfoViewModel
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
            var number = 0;
            var number1 = 0;
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.EquipmentType != WjParaBase.EquType.Slu)
                    continue;
                var x = t.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
                if (x == null)
                    continue;
                RequestSingleLampInformation(x, i);
                if (x.WjSluCtrls.Count != 0)
                {
                    i = i + x.WjSluCtrls.Count;
                    number1 = number1 + x.WjSluCtrls.Count;
                }
                else
                    i++;
                number++;
            }

            ConcentratorNumber = number;
            ControllerNumber = number1;

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
                tabletitle.Add("单灯地址");
                tabletitle.Add("名称");
                tabletitle.Add("序号");
                tabletitle.Add("条形码");
                tabletitle.Add("灯杆编码");
                tabletitle.Add("主动告警");
                tabletitle.Add("投运");
                tabletitle.Add("灯头数");
                tabletitle.Add("上电开灯");
                tabletitle.Add("回路矢量");
                tabletitle.Add("回路额定功率");
                tabletitle.Add("功率上限");
                tabletitle.Add("功率下限");
                var table = new List<List<string>>();
                foreach (var g in Item)
                {
                    var tem = new List<string>();
                    tem.Add(g.Index.ToString());
                    tem.Add(g.PhyId.ToString());
                    tem.Add(g.SingleName);
                    tem.Add(g.ControllerIndex.ToString());
                    tem.Add(g.BarCodeId);
                    tem.Add(g.LampCode);
                    tem.Add(g.IsActiveAlarm);
                    tem.Add(g.IsRun);
                    tem.Add(g.HolderNumber.ToString());
                    tem.Add(g.IsPowerOnLight);
                    tem.Add(g.LoopVector);
                    tem.Add(g.LoopPowerRating);
                    tem.Add(g.PowerUpper.ToString());
                    tem.Add(g.PowerLower.ToString());
                    table.Add(tem);
                }
                print.Prints.Print(tabletitle, table, true, "单灯设备信息", Wlst.Sr.EquipmentInfoHolding.Services.Others.SystemName, "", "");
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

    public partial class SingleLampInfoViewModel
    {
        private void RequestSingleLampInformation(Wj2090Slu infos,int i)
        {
            var info = infos.WjSluCtrls.OrderBy(t => t.Value.CtrlId).ToList();
            var j = 0;
            if(infos.WjSluCtrls.Count==0)
            {
                var tt = new SingleLampInfoOneViewModel();
                tt.Index = ++i;
                tt.PhyId = infos.RtuPhyId;
                tt.SingleName = infos.RtuName;
                Item.Add(tt);
            }
            else
                foreach (var f in info)
                {
                    j++;
                    var tt = new SingleLampInfoOneViewModel();
                    tt.Index = ++i;
                    tt.PhyId = infos.RtuPhyId;
                    tt.SingleName = infos.RtuName;
                    tt.BarCodeId = string.Format("{0:D13}", f.Value.BarCodeId);
                    tt.LampCode = f.Value.LampCode;
                    tt.IsActiveAlarm = f.Value.IsAlarmAuto ? "是" : "否";
                    tt.IsRun = f.Value.IsUsed ? "是" : "否";
                    tt.HolderNumber = f.Value.LightCount;
                    tt.IsPowerOnLight = (f.Value.IsAutoOpenLightWhenElec1 ? "是" : "否") + " " +
                                        (f.Value.IsAutoOpenLightWhenElec2 ? "是" : "否") + " " +
                                        (f.Value.IsAutoOpenLightWhenElec3 ? "是" : "否") + " " +
                                        (f.Value.IsAutoOpenLightWhenElec4 ? "是" : "否");
                    tt.LoopVector = f.Value.VectorLoop1.ToString() + " " + f.Value.VectorLoop2.ToString() + " " +
                                    f.Value.VectorLoop3.ToString() + " " + f.Value.VectorLoop4.ToString();
                    tt.LoopPowerRating = ConverPower(f.Value.PowerRate1) + " " + ConverPower(f.Value.PowerRate2) + " " +
                                         ConverPower(f.Value.PowerRate3) + " " + ConverPower(f.Value.PowerRate4) + " ";
                    tt.PowerUpper = f.Value.UpperPower.ToString()+"%";
                    tt.PowerLower = f.Value.LowerPower.ToString()+"%";
                    tt.ControllerIndex = j;
                    Item.Add(tt);
                }


        }

        private string ConverPower(int power)
        {
            string newpower="";
            switch (power)
            {
                case 0:
                    newpower= "未设置";
                    break;
                case 1:
                    newpower = "20";
                    break;
                case 14:
                    newpower = "50";
                    break;
                case 15:
                    newpower = "75";
                    break;
                case 2:
                    newpower = "100";
                    break;
                case 3:
                    newpower = "120";
                    break;
                case 4:
                    newpower = "150";
                    break;
                case 5:
                    newpower = "200";
                    break;
                case 6:
                    newpower = "250";
                    break;
                case 7:
                    newpower = "300";
                    break;
                case 8:
                    newpower = "400";
                    break;
                case 9:
                    newpower = "600";
                    break;
                case 10:
                    newpower = "800";
                    break;
                case 11:
                    newpower = "1000";
                    break;
                case 12:
                    newpower = "1500";
                    break;
                case 13:
                    newpower = "2000";
                    break;

            }
            return newpower;
        }
    }

    public class SingleLampInfoOneViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
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
        /// 集中器物理地址
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

        private string _singleName;
        /// <summary>
        /// 集中器名称
        /// </summary>
        public string SingleName
        {
            get { return _singleName; }
            set
            {
                if (_singleName == value) return;
                _singleName = value;
                RaisePropertyChanged(() => SingleName);
            }
        }

        private int _controllerIndex;
        /// <summary>
        /// 序号
        /// </summary>
        public int ControllerIndex
        {
            get { return _controllerIndex; }
            set
            {
                if (_controllerIndex == value) return;
                _controllerIndex = value;
                this.RaisePropertyChanged(() => this.ControllerIndex);
            }
        }

        private string _barCodeId;
        /// <summary>
        /// 条形码
        /// </summary>
        public string BarCodeId
        {
            get { return _barCodeId; }
            set
            {
                if (_barCodeId == value) return;
                _barCodeId = value;
                RaisePropertyChanged(() => BarCodeId);
            }
        }

        private string _lampCode;
        /// <summary>
        /// 灯杆编码
        /// </summary>
        public string LampCode
        {
            get { return _lampCode; }
            set
            {
                if (_lampCode == value) return;
                _lampCode = value;
                RaisePropertyChanged(() => LampCode);
            }
        }

        private string _isActiveAlarm;
        /// <summary>
        /// 主动报警
        /// </summary>
        public string IsActiveAlarm
        {
            get { return _isActiveAlarm; }
            set
            {
                if (_isActiveAlarm == value) return;
                _isActiveAlarm = value;
                RaisePropertyChanged(() => IsActiveAlarm);
            }
        }

        private string _isRun;
        /// <summary>
        /// 投运
        /// </summary>
        public string IsRun
        {
            get { return _isRun; }
            set
            {
                if (_isRun == value) return;
                _isRun = value;
                RaisePropertyChanged(() => IsRun);
            }
        }

        private int _holderNumber;
        /// <summary>
        /// 灯头数
        /// </summary>
        public int HolderNumber
        {
            get { return _holderNumber; }
            set
            {
                if (_holderNumber == value) return;
                _holderNumber = value;
                this.RaisePropertyChanged(() => this.HolderNumber);
            }
        }

        private string _isPowerOnLight;
        /// <summary>
        /// 上电开灯
        /// </summary>
        public string IsPowerOnLight
        {
            get { return _isPowerOnLight; }
            set
            {
                if (_isPowerOnLight == value) return;
                _isPowerOnLight = value;
                RaisePropertyChanged(() => IsPowerOnLight);
            }
        }

        private string _loopVector;
        /// <summary>
        /// 回路矢量
        /// </summary>
        public string LoopVector
        {
            get { return _loopVector; }
            set
            {
                if (_loopVector == value) return;
                _loopVector = value;
                RaisePropertyChanged(() => LoopVector);
            }
        }

        private string _loopPowerRating;
        /// <summary>
        /// 回路额定功率
        /// </summary>
        public string LoopPowerRating
        {
            get { return _loopPowerRating; }
            set
            {
                if (_loopPowerRating == value) return;
                _loopPowerRating = value;
                RaisePropertyChanged(() => LoopPowerRating);
            }
        }

        private string _powerUpper;
        /// <summary>
        /// 功率上限
        /// </summary>
        public string PowerUpper
        {
            get { return _powerUpper; }
            set
            {
                if (_powerUpper == value) return;
                _powerUpper = value;
                RaisePropertyChanged(() => PowerUpper);
            }
        }

        private string _powerLower;
        /// <summary>
        /// 功率下限
        /// </summary>
        public string PowerLower
        {
            get { return _powerLower; }
            set
            {
                if (_powerLower == value) return;
                _powerLower = value;
                RaisePropertyChanged(() => PowerLower);
            }
        }
    }
}
