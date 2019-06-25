using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using System.Globalization;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.EquipmentInfo.DailyStatistics.TerminalViewModel.Views;
using Wlst.mobile;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.TerminalViewModel.ViewModel
{
    public partial class TerminalViewModel : VmEventActionProperyChangedBase
    {
        private RadCartesianChart _radchart2 = null;
        public TerminalViewModel(RadCartesianChart radchart)
        {
            ImageIcon1 = Directory.GetCurrentDirectory() + "\\Image\\Png\\sunrise.png";
            ImageIcon2 = Directory.GetCurrentDirectory() + "\\Image\\Png\\sunset.png";
            Items.Clear();
            LineItemss.Clear();
            EnergyItems.Clear();
            FaultItems.Clear();
            BeginDate = DateTime.Now;
            InitEvent();
            InitAction();
            _radchart2 = radchart;

            for (
                var i = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month,
                                          BeginDate.AddDays(-1).Day, 12, 0, 0);
                i < new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 14, 0, 0);
                )
            {
                Items.Add(new OperatorOneRecordViewModel()
                              {
                                  Time = i,
                                  OperatorType1 = 0,
                                  Brush = new SolidColorBrush(Colors.White)
                              });

                i = i.AddHours(2);
            }

            for (int i = 0; i < 7; i++)
            {
                FaultItems.Add(new Fault()
                {
                    DtCreateTime = BeginDate.AddDays(-i),
                    Count = 0
                });
            }
            _radchart2.Series.Clear();
            AddLines(FaultItems, 0, Colors.Red);

            for (int i = 0; i < 7; i++)
            {
                EnergyItems.Add(new EnergyItem()
                                    {
                                        DateCreate = BeginDate.AddDays(-i),
                                        ElecVaule = 0
                                    });
            }

            Step1 = 1;
            Step2 = 1;

            PictureName.Clear();
            PictureName.Add("全部");
            PictureName.Add("开关灯信息");
            PictureName.Add("操作信息");
            PictureName.Add("故障统计");
            PictureName.Add("能耗统计");
            PictureComboBoxSelected = PictureName[0];
            RtuId = 1000001;
        }

        #region Step
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

        private int _step2;

        /// <summary>
        /// 
        /// </summary>
        public int Step2
        {
            get { return _step2; }
            set
            {
                if (_step2 == value) return;
                _step2 = value;
                RaisePropertyChanged(() => Step2);
            }
        }
        #endregion

        private string _imageIcon1;
        public string ImageIcon1
        {
            get { return _imageIcon1; }
            set
            {
                if (value != _imageIcon1)
                {
                    _imageIcon1 = value;
                    this.RaisePropertyChanged(() => this.ImageIcon1);
                }
            }
        }

        private string _imageIcon2;
        public string ImageIcon2
        {
            get { return _imageIcon2; }
            set
            {
                if (value != _imageIcon2)
                {
                    _imageIcon2 = value;
                    this.RaisePropertyChanged(() => this.ImageIcon2);
                }
            }
        }

        public static string PassRtuName;
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
                    PassRtuName = tml.RtuPhyId + "-" + tml.RtuName;
                    MruId = 0;
                    foreach (var t in tml.EquipmentsThatAttachToThisRtu)
                    {
                        if (t > 1300000 && t < 1400000)
                            MruId = t;
                    }
                    var i = 0;
                    var j = 0;
                    var sss = tml as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if (sss != null)
                        foreach (var t in sss.WjLoops)
                        {
                            if (t.Value.SwitchOutputId != 0)
                                i++;
                            j++;
                        }
                    AnalogNumber = i;
                    ThresholdNumber = j - i;
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
                RaisePropertyChanged(() => MruId);
                var ddd =
                    Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                if (ddd != null)
                    MruName = ddd.RtuName;
                else
                    MruName = "";
                if (MruId == 0)
                    IsVisible = Visibility.Hidden;
                else
                    IsVisible = Visibility.Visible;
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

        private int _analogNumber;
        /// <summary>
        /// 模拟量
        /// </summary>
        public int AnalogNumber
        {
            get { return _analogNumber; }
            set
            {
                if (value != _analogNumber)
                {
                    _analogNumber = value;
                    this.RaisePropertyChanged(() => this.AnalogNumber);

                }
            }
        }

        private int _thresholdNumber;
        /// <summary>
        /// 门限
        /// </summary>
        public int ThresholdNumber
        {
            get { return _thresholdNumber; }
            set
            {
                if (value != _thresholdNumber)
                {
                    _thresholdNumber = value;
                    this.RaisePropertyChanged(() => this.ThresholdNumber);

                }
            }
        }

        private string _sunRise;
        /// <summary>
        /// 日出时间
        /// </summary>
        public string SunRise
        {
            get { return _sunRise; }
            set
            {
                if (value != _sunRise)
                {
                    _sunRise = value;
                    this.RaisePropertyChanged(() => this.SunRise);
                }
            }
        }

        private string _sunSet;
        /// <summary>
        /// 日落时间
        /// </summary>
        public string SunSet
        {
            get { return _sunSet; }
            set
            {
                if (value != _sunSet)
                {
                    _sunSet = value;
                    this.RaisePropertyChanged(() => this.SunSet);
                }
            }
        }

        private int _xSunRise;
        /// <summary>
        /// 日出
        /// </summary>
        public int XSunRise
        {
            get { return _xSunRise; }
            set
            {
                if (value != _xSunRise)
                {
                    _xSunRise = value;
                    this.RaisePropertyChanged(() => this.XSunRise);
                }
            }
        }

        private int _xSunSet;
        /// <summary>
        /// 日落
        /// </summary>
        public int XSunSet
        {
            get { return _xSunSet; }
            set
            {
                if (value != _xSunSet)
                {
                    _xSunSet = value;
                    this.RaisePropertyChanged(() => this.XSunSet);
                }
            }
        }

        private int _x;
        /// <summary>
        /// 
        /// </summary>
        public int X
        {
            get { return _x; }
            set
            {
                if (value != _x)
                {
                    _x = value;
                    this.RaisePropertyChanged(() => this.X);
                }
            }
        }

        private int _x1;
        /// <summary>
        /// 
        /// </summary>
        public int X1
        {
            get { return _x1; }
            set
            {
                if (value != _x1)
                {
                    _x1 = value;
                    this.RaisePropertyChanged(() => this.X1);
                }
            }
        }

        private string _energyName="能耗统计";
        public string EnergyName
        {
            get { return _energyName; }
            set
            {
                if (value != _energyName)
                {
                    _energyName = value;
                    this.RaisePropertyChanged(() => this.EnergyName);
                }
            }
        }

        #region Visi
        private Visibility _visi = Visibility.Hidden;
        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get
            {
                return _visi;
            }
            set
            {
                if (value == _visi) return;
                _visi = value;
                RaisePropertyChanged(() => Visi);
            }
        }
        #endregion

        #region IsVisible
        private Visibility _isVisible = Visibility.Hidden;
        /// <summary>
        /// 
        /// </summary>
        public Visibility IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                if (value == _isVisible) return;
                _isVisible = value;
                RaisePropertyChanged(() => IsVisible);
            }
        }
        #endregion

        #region Items

        private ObservableCollection<OperatorOneRecordViewModel> _items;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<OperatorOneRecordViewModel> Items
        {
            get { return _items ?? (_items = new ObservableCollection<OperatorOneRecordViewModel>()); }
            set
            {
                if (_items == value) return;
                _items = value;
                this.RaisePropertyChanged(() => this.Items);
            }
        }

        #endregion

        #region OperationItems


        private ObservableCollection<OperatorOneRecordViewModel> _operationItems;
        /// <summary>
        /// 操作信息
        /// </summary>
        public ObservableCollection<OperatorOneRecordViewModel> OperationItems
        {
            get { return _operationItems ?? (_operationItems = new ObservableCollection<OperatorOneRecordViewModel>()); }
            set
            {
                if (_operationItems == value) return;
                _operationItems = value;
                this.RaisePropertyChanged(() => this.OperationItems);
            }
        }

        #endregion

        #region OpenOrCloseOperationItems

        private ObservableCollection<OperatorOneRecordViewModel> _openOrCloseOperationItems;
        /// <summary>
        /// 开关灯操作信息
        /// </summary>
        public ObservableCollection<OperatorOneRecordViewModel> OpenOrCloseOperationItems
        {
            get { return _openOrCloseOperationItems ?? (_openOrCloseOperationItems = new ObservableCollection<OperatorOneRecordViewModel>()); }
            set
            {
                if (_openOrCloseOperationItems == value) return;
                _openOrCloseOperationItems = value;
                this.RaisePropertyChanged(() => this.OpenOrCloseOperationItems);
            }
        }


        #endregion

        #region PairItems

        private ObservableCollection<OperatorOneRecordViewModel> _pairItems;
        /// <summary>
        /// 对时信息
        /// </summary>
        public ObservableCollection<OperatorOneRecordViewModel> PairItems
        {
            get { return _pairItems ?? (_pairItems = new ObservableCollection<OperatorOneRecordViewModel>()); }
            set
            {
                if (_pairItems == value) return;
                _pairItems = value;
                this.RaisePropertyChanged(() => this.PairItems);
            }
        }


        #endregion

        #region SendWeekSettingOperationItems

        private ObservableCollection<OperatorOneRecordViewModel> _sendWeekSettingOperationItems;
        /// <summary>
        /// 发送周设置操作信息
        /// </summary>
        public ObservableCollection<OperatorOneRecordViewModel> SendWeekSettingOperationItems
        {
            get { return _sendWeekSettingOperationItems ?? (_sendWeekSettingOperationItems = new ObservableCollection<OperatorOneRecordViewModel>()); }
            set
            {
                if (_sendWeekSettingOperationItems == value) return;
                _sendWeekSettingOperationItems = value;
                this.RaisePropertyChanged(() => this.SendWeekSettingOperationItems);
            }
        }
        #endregion

        #region SendParametersOperationItems

        private ObservableCollection<OperatorOneRecordViewModel> _sendParametersOperationItems;
        /// <summary>
        /// 发送参数操作信息
        /// </summary>
        public ObservableCollection<OperatorOneRecordViewModel> SendParametersOperationItems
        {
            get { return _sendParametersOperationItems ?? (_sendParametersOperationItems = new ObservableCollection<OperatorOneRecordViewModel>()); }
            set
            {
                if (_sendParametersOperationItems == value) return;
                _sendParametersOperationItems = value;
                this.RaisePropertyChanged(() => this.SendParametersOperationItems);
            }
        }
        #endregion

        #region ResetTerminalItems

        private ObservableCollection<OperatorOneRecordViewModel> _resetTerminalItems;
        /// <summary>
        /// 终端复位
        /// </summary>
        public ObservableCollection<OperatorOneRecordViewModel> ResetTerminalItems
        {
            get { return _resetTerminalItems ?? (_resetTerminalItems = new ObservableCollection<OperatorOneRecordViewModel>()); }
            set
            {
                if (_resetTerminalItems == value) return;
                _resetTerminalItems = value;
                this.RaisePropertyChanged(() => this.ResetTerminalItems);
            }
        }
        #endregion

        #region LineItemss
        private ObservableCollection<LineInfo> _lineItemss;
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
        #endregion

        #region OpenOrCloseItems

        private ObservableCollection<OpenOrClose> _openOrCloseItems;
        /// <summary>
        /// 开关灯信息
        /// </summary>
        public ObservableCollection<OpenOrClose> OpenOrCloseItems
        {
            get { return _openOrCloseItems ?? (_openOrCloseItems = new ObservableCollection<OpenOrClose>()); }
            set
            {
                if (_openOrCloseItems == value) return;
                _openOrCloseItems = value;
                this.RaisePropertyChanged(() => this.OpenOrCloseItems);
            }
        }

        #endregion

        #region FaultItems
        public static ObservableCollection<Fault> opitem;
        private ObservableCollection<Fault> _faultItems;
        /// <summary>
        /// 故障信息
        /// </summary>
        public ObservableCollection<Fault> FaultItems
        {
            get { return _faultItems ?? (_faultItems = new ObservableCollection<Fault>()); }
            set
            {
                if (_faultItems == value) return;
                _faultItems = value;
                this.RaisePropertyChanged(() => this.FaultItems);
                opitem = FaultItems;
            }
        }

        #endregion

        #region OneFaultItems

        private ObservableCollection<Fault> _oneFaultItems;
        /// <summary>
        /// 一类故障信息
        /// </summary>
        public ObservableCollection<Fault> OneFaultItems
        {
            get { return _oneFaultItems ?? (_oneFaultItems = new ObservableCollection<Fault>()); }
            set
            {
                if (_oneFaultItems == value) return;
                _oneFaultItems = value;
                this.RaisePropertyChanged(() => this.OneFaultItems);
            }
        }

        #endregion

        #region TypeItems
        private ObservableCollection<OperatorTypeItem> _typeItems;
        /// <summary>
        /// 故障类型
        /// </summary>
        public ObservableCollection<OperatorTypeItem> TypeItems
        {
            get { return _typeItems ?? (_typeItems = new ObservableCollection<OperatorTypeItem>()); }
            set
            {
                if (value == _typeItems) return;
                _typeItems = value;
                this.RaisePropertyChanged(() => TypeItems);
            }
        }
        #endregion

        #region EnergyItems
        private ObservableCollection<EnergyItem> _energyItems;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<EnergyItem> EnergyItems
        {
            get { return _energyItems ?? (_energyItems = new ObservableCollection<EnergyItem>()); }
            set
            {
                if (value == _energyItems) return;
                _energyItems = value;
                this.RaisePropertyChanged(() => EnergyItems);
            }
        }
        #endregion

        public static DateTime PassDate;

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
                PassDate = value;
            }
        }

        private string _sTime;
        /// <summary>
        /// 
        /// </summary>
        public string STime
        {
            get
            {
                return _sTime;
            }
            set
            {
                if (value == _sTime) return;
                _sTime = value;
                RaisePropertyChanged(() => STime);
            }
        }

        private string _eTime;
        /// <summary>
        /// 
        /// </summary>
        public string ETime
        {
            get
            {
                return _eTime;
            }
            set
            {
                if (value == _eTime) return;
                _eTime = value;
                RaisePropertyChanged(() => ETime);
            }
        }
        #endregion

        #region IsOpenOrClose

        private bool _isOpenOrClose = true;
        /// <summary>
        /// 开关灯
        /// </summary>
        public bool IsOpenOrClose
        {
            get
            {
                return _isOpenOrClose;
            }
            set
            {
                if (value == _isOpenOrClose) return;
                _isOpenOrClose = value;
                RaisePropertyChanged(() => IsOpenOrClose);

                var item = new ObservableCollection<OperatorOneRecordViewModel>();
                foreach (var f in OperationItems)
                    item.Add(f);
                if (IsOpenOrClose)
                    foreach (var t in OpenOrCloseOperationItems)
                        item.Add(t);
                else
                    foreach (var t in OpenOrCloseOperationItems)
                        item.Remove(t);
                OperationItems = item;
            }
        }
        #endregion

        #region IsPair
        private bool _isPair;
        /// <summary>
        /// 对时
        /// </summary>
        public bool IsPair
        {
            get
            {
                return _isPair;
            }
            set
            {
                if (value == _isPair) return;
                _isPair = value;
                RaisePropertyChanged(() => IsPair);
                var item = new ObservableCollection<OperatorOneRecordViewModel>();
                foreach (var f in OperationItems)
                    item.Add(f);
                if (IsPair)
                    foreach (var t in PairItems)
                        item.Add(t);
                else
                    foreach (var t in PairItems)
                        item.Remove(t);
                OperationItems = item;
            }
        }
        #endregion

        #region IsSendWeekSetting
        private bool _isSendWeekSetting;
        /// <summary>
        /// 发送周设置
        /// </summary>
        public bool IsSendWeekSetting
        {
            get
            {
                return _isSendWeekSetting;
            }
            set
            {
                if (value == _isSendWeekSetting) return;
                _isSendWeekSetting = value;
                RaisePropertyChanged(() => IsSendWeekSetting);

                var item = new ObservableCollection<OperatorOneRecordViewModel>();
                foreach (var f in OperationItems)
                    item.Add(f);
                if (IsSendWeekSetting)
                    foreach (var t in SendWeekSettingOperationItems)
                        item.Add(t);
                else
                    foreach (var t in SendWeekSettingOperationItems)
                        item.Remove(t);
                OperationItems = item;
            }
        }
        #endregion

        #region IsSendParameters
        private bool _isSendParameters;
        /// <summary>
        /// 发送参数
        /// </summary>
        public bool IsSendParameters
        {
            get
            {
                return _isSendParameters;
            }
            set
            {
                if (value == _isSendParameters) return;
                _isSendParameters = value;
                RaisePropertyChanged(() => IsSendParameters);

                var item = new ObservableCollection<OperatorOneRecordViewModel>();
                foreach (var f in OperationItems)
                    item.Add(f);
                if (IsSendParameters)
                    foreach (var t in SendParametersOperationItems)
                        item.Add(t);
                else
                    foreach (var t in SendParametersOperationItems)
                        item.Remove(t);
                OperationItems = item;
            }
        }
        #endregion

        #region IsTerminalReset
        private bool _isTerminalReset;
        /// <summary>
        /// 发送参数
        /// </summary>
        public bool IsTerminalReset
        {
            get
            {
                return _isTerminalReset;
            }
            set
            {
                if (value == _isTerminalReset) return;
                _isTerminalReset = value;
                RaisePropertyChanged(() => IsTerminalReset);

                var item = new ObservableCollection<OperatorOneRecordViewModel>();
                foreach (var f in OperationItems)
                    item.Add(f);
                if (IsSendParameters)
                    foreach (var t in ResetTerminalItems)
                        item.Add(t);
                else
                    foreach (var t in ResetTerminalItems)
                        item.Remove(t);
                OperationItems = item;
            }
        }
        #endregion

        #region PictureName
        private ObservableCollection<string> _pictureName;

        public ObservableCollection<string> PictureName
        {
            get { return _pictureName ?? (_pictureName = new ObservableCollection<string>()); }
            set
            {
                if (_pictureName == value) return;
                _pictureName = value;
                this.RaisePropertyChanged(() => this.PictureName);
            }
        }
        #endregion

        public static string PassName;
        #region PictureComboBoxSelected
        private string _picturecomboboxselected;

        public string PictureComboBoxSelected
        {
            get { return _picturecomboboxselected; }
            set
            {
                if (_picturecomboboxselected != value)
                {
                    _picturecomboboxselected = value;
                    this.RaisePropertyChanged(() => this.PictureComboBoxSelected);
                    PassName = value;
                }
            }
        }
        #endregion

        public static Dictionary<string, string> OperatorTypes = new Dictionary<string, string>();

        #region FaultCategories
        private ObservableCollection<NameIntBool> _faultCategories;
        /// <summary>
        /// 故障大类
        /// </summary>
        public ObservableCollection<NameIntBool> FaultCategories
        {
            get
            {
                if (_faultCategories == null)
                {
                    _faultCategories = new ObservableCollection<NameIntBool>();

                    _faultCategories.Add(new NameIntBool() { Name = "终端供电", Value = 1, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "回路开灯", Value = 2, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "回路关灯", Value = 3, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "回路其他", Value = 4, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "终端其他", Value = 5, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "故障总数", Value = 6, IsSelected = true });
                }
                return _faultCategories;
            }
        }
        #endregion
    }
    public partial class TerminalViewModel
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
            STime = BeginDate.AddDays(-1).ToString("MM-dd");
            ETime = BeginDate.ToString("MM-dd");
            InitSun();
            FaultType();
            //QueryOperation();
            QueryTerminal();
        }
        private bool CanExCmdQuery()
        {
            return DateTime.Now.Ticks - _dtCmdQuery.Ticks > 30000000 && RtuId != 0;
        }
        #endregion
    }

    public partial class TerminalViewModel
    {
        private void QueryOperation()
        {
            OperationItems.Clear();
            var info = Wlst.Sr.ProtocolPhone.LxSys.wst_sys_operator_record;
            info.WstSysOperatorRecord.Op = 2;
            info.WstSysOperatorRecord.DtStartTime = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 0, 0, 1).Ticks;
            info.WstSysOperatorRecord.DtEndTime = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 23, 59, 59).Ticks;
            info.WstSysOperatorRecord.RtuId = RtuId;
            //info.WstSysOperatorRecord.OperatorIds = lstOpetatorType;
            info.WstSysOperatorRecord.IsClientSnd = 0;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void InitSun()
        {
            var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(BeginDate.Month,
                                                                                                  BeginDate.Day);
            if(info==null) return;
            SunRise = (info.time_sunrise/60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ":" +
                      (info.time_sunrise%60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            SunSet = (info.time_sunset/60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ":" +
                     (info.time_sunset%60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            XSunRise = 30 + (info.time_sunrise + 720)/3;
            XSunSet = 30 + (info.time_sunset - 720)/3;
            X = 15 + (info.time_sunrise + 720)/3;
            X1 = 15 + (info.time_sunset - 720)/3;
            Visi=Visibility.Visible;
        }

        private void QueryTerminal()
        {
            var info = Wlst.Sr.ProtocolPhone.LxStatic.wst_static_oneday_rtu;
            info.WstStatisRtuOneday.DtTime = BeginDate.Ticks;
            info.WstStatisRtuOneday.RtuId = RtuId;
            info.WstStatisRtuOneday.MruId = MruId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void FaultType()
        {
            var color = new List<Brush>();
            color.Add(new SolidColorBrush(Colors.Orange));
            color.Add(new SolidColorBrush(Colors.Blue));
            color.Add(new SolidColorBrush(Colors.Brown));
            color.Add(new SolidColorBrush(Colors.Chartreuse));
            color.Add(new SolidColorBrush(Colors.BlueViolet));
            color.Add(new SolidColorBrush(Colors.Cyan));
            color.Add(new SolidColorBrush(Colors.DarkGreen));
            color.Add(new SolidColorBrush(Colors.DeepPink));
            color.Add(new SolidColorBrush(Colors.DimGray));
            color.Add(new SolidColorBrush(Colors.OrangeRed));

            var item = new ObservableCollection<OperatorTypeItem>();
            int i = 0;
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                if (i >= color.Count)
                    i = 0;
                if (!t.Value.IsEnable) continue;
                if (t.Key >= 1 && t.Key <= 30)
                    item.Add(new OperatorTypeItem()
                                 {
                                     IsSelected = false,
                                     Name = t.Value.FaultNameByDefine,
                                     Value = t.Key,
                                     Color =color[i]
                                 });
                i++;
            }
            TypeItems = item;
        }
    }

    public partial class TerminalViewModel
    {
        private void InitAction()
        {
            this.RegistProtocol(
                Sr.ProtocolPhone.LxSys.wst_sys_operator_record,//.wlst_svr_ans_cnt_request_operator_type,
                GetAllOperatorTypes, true);

            //this.RegistProtocol(
            //   Sr.ProtocolPhone.LxFault.wlst_fault_pre,
            //   OnRequestFaultPre,true);

            this.RegistProtocol(
               Sr.ProtocolPhone.LxStatic.wst_static_oneday_rtu,
               OnRequestTerminal, true);
        }

        private void GetAllOperatorTypes(string session, Wlst.mobile.MsgWithMobile infos)
        {
            //if (IsViewShowd == false) return;
            Wlst.Cr.Core.CoreServices.RegionManage.DispatcherInvoke(GetAllOperatorTypes1, infos);
        }

        private void GetAllOperatorTypes1(object infosss)
        {
            var infos = infosss as MsgWithMobile;
            if(infos==null) return;
            var info = infos.WstSysOperatorRecord;
            // var info = args.GetParams()[1] as AllUserInfo;
            if (info == null) return;
            if (info.Op == 1)
            {
                OperatorTypes.Clear();
                foreach (var item in info.TypeItems)
                {
                    var opid = item.Id + "";
                    if (OperatorTypes.ContainsKey(opid) == true)
                    {
                        OperatorTypes[opid] = item.Name;
                    }
                    else
                    {
                        OperatorTypes.Add(opid, item.Name);
                    }

                }
            }


        }

        private void OnRequestTerminal(string session, Wlst.mobile.MsgWithMobile infos)
        {
            EnergyItems.Clear();
            FaultItems.Clear();
            OperationItems.Clear();
            var eitem = new ObservableCollection<EnergyItem>();
            var info = infos.WstStatisRtuOneday;
            if (info.ItemsElec.Count > 0)
            {
                bool isMru = true;
                foreach (var f in info.ItemsElec)
                {
                    if (f.MruId != 0)
                    {
                        isMru = true;
                        break;
                    }
                    isMru = false;
                }
                EnergyName = isMru ? "能耗统计(电表值)" : "能耗统计(估算值)";
            }
            for (int i = 0; i < 7; i++)
            {
                double elecVaule = 0;
                foreach (var f in info.ItemsElec)
                {
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                        elecVaule = f.ElecVaule;
                }
                eitem.Add(new EnergyItem()
                              {
                                  DateCreate = BeginDate.AddDays(-i),
                                  ElecVaule = Math.Round(elecVaule, 2)
                              });
            }

            var xx = eitem.Select(t => t.ElecVaule).Concat(new[] {1.0}).Max();
            Step2 = (int) xx/5 > 1 ? (int) xx/5 : 1;
            EnergyItems = eitem;

            var fitem = new ObservableCollection<Fault>();
            //总故障
            for (int i = 0; i < 7; i++)
            {
                var count = 0;
                foreach (var f in info.ItemsFault)
                {
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                        count += f.Count;
                }

                fitem.Add(new Fault()
                              {
                                  DtCreateTime = BeginDate.AddDays(-i),
                                  Count = count,
                                  FaultId = 0
                              });
            }
            //所有故障
            foreach (var t in TypeItems)
            {
                for (int i = 0; i < 7; i++)
                {

                    var count1 = 0;
                    foreach (var f in info.ItemsFault)
                    {
                        var x = new DateTime(f.DateCreate);

                        if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                            BeginDate.AddDays(-i).Day == x.Day && f.FaultId == t.Value)
                            count1 += f.Count;
                    }

                    fitem.Add(new Fault()
                                  {
                                      DtCreateTime = BeginDate.AddDays(-i),
                                      Count = count1,
                                      FaultId = t.Value
                                  });
                }
            }

            //分类故障
            for (int i = 0; i < 7; i++)
            {
                var count1 = 0;
                var count2 = 0;
                var count3 = 0;
                var count4 = 0;
                var count5 = 0;

                foreach (var f in info.ItemsFault)
                {
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                    {
                        if (f.FaultId >= 1 && f.FaultId <= 5)
                            count1 += f.Count;
                        if ((f.FaultId >= 9 && f.FaultId <= 11) || (f.FaultId >= 15 && f.FaultId <= 17) ||
                            f.FaultId == 20)
                            count2 += f.Count;
                        if ((f.FaultId >= 12 && f.FaultId <= 14) || f.FaultId == 21)
                            count3 += f.Count;
                        if (f.FaultId == 25 || (f.FaultId >= 6 && f.FaultId <= 8) || f.FaultId == 26 || f.FaultId == 28)
                            count4 += f.Count;
                        if ((f.FaultId >= 22 && f.FaultId <= 24) || f.FaultId == 19 || f.FaultId == 27 ||
                            f.FaultId == 29)
                            count5 += f.Count;
                    }
                }

                fitem.Add(new Fault()
                              {
                                  DtCreateTime = BeginDate.AddDays(-i),
                                  Count = count1,
                                  FaultId = 1000
                              });
                fitem.Add(new Fault()
                              {
                                  DtCreateTime = BeginDate.AddDays(-i),
                                  Count = count2,
                                  FaultId = 1001
                              });
                fitem.Add(new Fault()
                              {
                                  DtCreateTime = BeginDate.AddDays(-i),
                                  Count = count3,
                                  FaultId = 1002
                              });
                fitem.Add(new Fault()
                              {
                                  DtCreateTime = BeginDate.AddDays(-i),
                                  Count = count4,
                                  FaultId = 1003
                              });
                fitem.Add(new Fault()
                              {
                                  DtCreateTime = BeginDate.AddDays(-i),
                                  Count = count5,
                                  FaultId = 1004
                              });
            }

            var tt = fitem.Select(t => t.Count).Concat(new[] {1}).Max();
            Step1 = tt/5 > 1 ? tt/5 : 1;
            FaultItems = fitem;
            _radchart2.Series.Clear();
            foreach (var f in FaultCategories)
            {
                if (f.IsSelected)
                {
                    if (f.Value == 1)
                        AddLines(FaultItems, 1000, Colors.Gray);
                    if (f.Value == 2)
                        AddLines(FaultItems, 1001, Colors.Green);
                    if (f.Value == 3)
                        AddLines(FaultItems, 1002, Colors.Orange);
                    if (f.Value == 4)
                        AddLines(FaultItems, 1003, Colors.Purple);
                    if (f.Value == 5)
                        AddLines(FaultItems, 1004, Colors.Blue);
                    if (f.Value == 6)
                        AddLines(FaultItems, 0, Colors.Red);
                }
            }
            if (_radchart2.Series.Count < 1)
            {
                var itemss = new ObservableCollection<Fault>();
                for (int i = 0; i < 7; i++)
                {
                    itemss.Add(new Fault()
                    {
                        DtCreateTime = BeginDate.AddDays(-i),
                        Count = 0
                    });
                }
                AddLines(itemss, 0, Colors.Red);
            }
            //AddLines(FaultItems, 0);

            //var ocitem = new ObservableCollection<OpenOrClose>();            
            //var loops = new List<string>();
            var tmp = new Dictionary<int, List<Tuple<int, long>>>();
            foreach (var f in info.ItemsOc)
            {
                int loopid = 0;
                if (Int32.TryParse(f.Content.Replace("回路:", "").Replace(",",""), out loopid) && (f.OperatorType == 19 || f.OperatorType == 20))
                {
                    if(loopid==0)
                    {
                        for(int i=1;i<9;i++)
                        {
                            if (tmp.ContainsKey(i) == false) tmp.Add(i, new List<Tuple<int, long>>());
                            tmp[i].Add(new Tuple<int, long>(f.OperatorType, f.DateCreate));
                        }
                    }
                    if (loopid > 0)
                    {
                        if (tmp.ContainsKey(loopid) == false) tmp.Add(loopid, new List<Tuple<int, long>>());
                        tmp[loopid].Add(new Tuple<int, long>(f.OperatorType, f.DateCreate));
                    }
                }
            }

            var line = new ObservableCollection<LineInfo>();
            foreach (var f in tmp)
            {
                var tnoss = (from t in f.Value orderby t.Item2 ascending select t).ToList();
                var lasttnoss = tnoss[tnoss.Count-1];
                long lastOctime = 0;
                bool isClose = false;
                foreach (var g in tnoss)
                {
                    //测试一下
                    if (g.Item2 == lasttnoss.Item2 && g.Item1 == 19)
                    {
                        var thisclosetime =
                            new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 12, 0, 1).Ticks >
                            DateTime.Now.Ticks
                                ? DateTime.Now.Ticks
                                : new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 12, 0, 1).Ticks;

                        var d1 = new DateTime(g.Item2);
                        var d12 = new DateTime(d1.Year, d1.Month, d1.Day, 12, 0, 1);
                        if (d12.Ticks > d1.Ticks) d12 = d12.AddDays(-1);

                        var diff1 = (d1.Ticks - d12.Ticks) / 10000000 / 60;
                        var diff2 = (thisclosetime - d12.Ticks) / 10000000 / 60;

                        var x1 = (int)diff1 / 3;
                        var x2 = (int)diff2 / 3;

                        var time = ((diff2 - diff1) / 60).ToString().PadLeft(2, '0') + "小时" + ((diff2 - diff1) % 60).ToString().PadLeft(2, '0') + "分钟";

                        line.Add(new LineInfo()
                        {
                            X1 = 44 + x1,
                            X2 = 38 + x2,
                            Y1 = 60 + f.Key * 30,
                            Y2 = 60 + f.Key * 30,
                            M1 = 20 + x1,
                            M2 = 20 + x2,
                            N1 = 40 + f.Key * 30,
                            N2 = 40 + f.Key * 30,
                            A1 = 50 + f.Key * 30,
                            B1 = 57 + f.Key * 30,
                            A2 = 40 + x1,
                            B2 = 36 + x2,
                            Open = d1.ToString("HH:mm"),
                            Close = new DateTime(thisclosetime).ToString("HH:mm"),
                            Color = new SolidColorBrush(Colors.DarkOrange),
                            SumTime = time,
                            Tooltips1 = d1.ToString("HH:mm"),
                            Tooltips2 = new DateTime(thisclosetime).ToString("HH:mm"),
                            Index = (int)(diff2 - diff1)
                        });

                        lastOctime = 0;
                        isClose = true;
                    }


                    if (g.Item1 == 19)
                    {
                        if (lastOctime == 0)
                        {
                            lastOctime = g.Item2;
                            continue;
                        }
                    }
                    if (g.Item1 == 20)
                    {
                        if (lastOctime > 0)
                        {
                            var thisclosetime = g.Item2;

                            var d1 = new DateTime(lastOctime);
                            var d12 = new DateTime(d1.Year, d1.Month, d1.Day, 12, 0, 1);
                            if (d12.Ticks > d1.Ticks) d12 = d12.AddDays(-1);

                            var diff1 = (d1.Ticks - d12.Ticks)/10000000/60;
                            var diff2 = (thisclosetime - d12.Ticks)/10000000/60;

                            var x1 = (int) diff1/3;
                            var x2 = (int) diff2/3;

                            var time = ((diff2 - diff1) / 60).ToString().PadLeft(2, '0') + "小时" + ((diff2 - diff1) % 60).ToString().PadLeft(2, '0') + "分钟";

                            line.Add(new LineInfo()
                                         {
                                             X1 = 44 + x1,
                                             X2 = 38 + x2,
                                             Y1 = 60 + f.Key*30,
                                             Y2 = 60 + f.Key*30,
                                             M1 = 20 + x1,
                                             M2 = 20 + x2,
                                             N1 = 40 + f.Key*30,
                                             N2 = 40 + f.Key*30,
                                             A1 = 50 + f.Key*30,
                                             B1 = 57 + f.Key*30,
                                             A2 = 40 + x1,
                                             B2 = 36 + x2,
                                             Open = d1.ToString("HH:mm"),
                                             Close = new DateTime(thisclosetime).ToString("HH:mm"),
                                             Color = new SolidColorBrush(Colors.DarkOrange),
                                             SumTime = time,
                                             Tooltips1 = d1.ToString("HH:mm"),
                                             Tooltips2 = new DateTime(thisclosetime).ToString("HH:mm"),
                                             Index = (int) (diff2 - diff1)
                                         });

                            lastOctime = 0;
                            isClose = true;
                        }

                        //测试一下
                        if(g.Item2==tnoss[0].Item2)
                        {
                            var thisclosetime = g.Item2;

                            var d1 = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month, BeginDate.AddDays(-1).Day, 12, 0, 1);
                            //var d12 = new DateTime(d1.Year, d1.Month, d1.Day, 12, 0, 1);
                            //if (d12.Ticks > d1.Ticks) d12 = d12.AddDays(-1);

                            //var diff1 = (d1.Ticks - d12.Ticks) / 10000000 / 60;
                            var diff2 = (thisclosetime - d1.Ticks) / 10000000 / 60;

                            var x1 = 0;
                            var x2 = (int)diff2 / 3;

                            var time = ((diff2 - 0) / 60).ToString().PadLeft(2, '0') + "小时" + ((diff2 - 0) % 60).ToString().PadLeft(2, '0') + "分钟";

                            line.Add(new LineInfo()
                            {
                                X1 = 44 + x1,
                                X2 = 38 + x2,
                                Y1 = 60 + f.Key * 30,
                                Y2 = 60 + f.Key * 30,
                                M1 = 20 + x1,
                                M2 = 20 + x2,
                                N1 = 40 + f.Key * 30,
                                N2 = 40 + f.Key * 30,
                                A1 = 50 + f.Key * 30,
                                B1 = 57 + f.Key * 30,
                                A2 = 40 + x1,
                                B2 = 36 + x2,
                                Open = d1.ToString("HH:mm"),
                                Close = new DateTime(thisclosetime).ToString("HH:mm"),
                                Color = new SolidColorBrush(Colors.DarkOrange),
                                SumTime = time,
                                Tooltips1 = d1.ToString("HH:mm"),
                                Tooltips2 = new DateTime(thisclosetime).ToString("HH:mm"),
                                Index = (int)(diff2 - 0)
                            });

                            lastOctime = 0;
                            isClose = true;
                        }

                    }
                }

                if (lastOctime > 0 && isClose == false)
                {


                    var d1 = new DateTime(lastOctime);
                    var thisclosetime = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 12, 0, 1).Ticks;
                    var d12 = new DateTime(d1.Year, d1.Month, d1.Day, 12, 0, 1);
                    if (d12.Ticks < d1.Ticks) d12 = d12.AddDays(-1);

                    var diff1 = (d1.Ticks - d12.Ticks)/10000000/60;
                    var diff2 = (thisclosetime - d12.Ticks)/10000000/60;

                    var x1 = (int) diff1/3;
                    var x2 = (int) diff2/3;

                    var time = ((diff2 - diff1) / 60).ToString().PadLeft(2, '0') + "小时" + ((diff2 - diff1) % 60).ToString().PadLeft(2, '0') + "分钟";

                    line.Add(new LineInfo()
                                 {
                                     X1 = 40 + x1,
                                     X2 = 40 + x2,
                                     Y1 = 60 + f.Key*30,
                                     Y2 = 60 + f.Key*30,
                                     M1 = 20 + x1,
                                     M2 = 20 + x2,
                                     N1 = 40 + f.Key*30,
                                     N2 = 40 + f.Key*30,
                                     A1 = 50 + f.Key*30,
                                     B1 = 57 + f.Key*30,
                                     A2 = 36 + x1,
                                     B2 = 38 + x2,
                                     Open = d1.ToString("HH:mm"),
                                     Close = "12:00",
                                     Color = new SolidColorBrush(Colors.DarkOrange),
                                     SumTime = time,
                                     Tooltips1 = d1.ToString("HH:mm"),
                                     Tooltips2 = "12:00",
                                     Index = (int) (diff2 - diff1)
                                 });
                }
            }
            var dic = new Dictionary<int, List<LineInfo>>();
            foreach (var l in line)
            {
                if (dic.ContainsKey(l.Y1) == false) dic.Add(l.Y1, new List<LineInfo>());
                dic[l.Y1].Add(l);
            }
            foreach (var t in dic)
            {
                var m = 0;
                if (t.Value.Count > 1)
                {
                    m += t.Value.Sum(f => f.Index);
                    foreach (var x in line)
                    {
                        if (x.Y1 == t.Key)
                            x.SumTime = (m/60).ToString().PadLeft(2, '0') + "小时" + (m%60).ToString().PadLeft(2, '0') + "分钟";
                    }
                }
            }
            LineItemss = line;

            var opitem = new ObservableCollection<OperatorOneRecordViewModel>();
            foreach (var i in info.ItemsOc)
            {
                opitem.Add(new OperatorOneRecordViewModel()
                               {
                                   Time = new DateTime(i.DateCreate),
                                   OperatorType1 = i.IsClientSnd == 1 && i.OperatorType == 19
                                                       ? 1
                                                       : i.IsClientSnd == 1 && i.OperatorType == 20
                                                             ? 2
                                                             : i.IsClientSnd == 3 ? 3 : 0,
                                   Brush = new SolidColorBrush(Colors.Red)
                               });
            }
            OpenOrCloseOperationItems = opitem;
            var spitem = new ObservableCollection<OperatorOneRecordViewModel>();
            foreach (var i in info.ItemsSndArgu)
            {
                if (i.OperatorType != 11)
                    spitem.Add(new OperatorOneRecordViewModel()
                                   {
                                       Time = new DateTime(i.DateCreate),
                                       OperatorType1 = i.IsClientSnd == 1 ? 8 : i.IsClientSnd == 3 ? 9 : 0,
                                       Brush = new SolidColorBrush(Colors.Orange)
                                   });
            }
            SendParametersOperationItems = spitem;
            var wpitem = new ObservableCollection<OperatorOneRecordViewModel>();
            foreach (var i in info.ItemsWeekset)
            {
                wpitem.Add(new OperatorOneRecordViewModel()
                               {
                                   Time = new DateTime(i.DateCreate),
                                   OperatorType1 = 6,
                                   Brush = new SolidColorBrush(Colors.Green)
                               });
                if (i.DateReply != 0)
                    wpitem.Add(new OperatorOneRecordViewModel()
                                   {
                                       Time = new DateTime(i.DateReply),
                                       OperatorType1 = 7,
                                       Brush = new SolidColorBrush(Colors.Green)
                                   });
            }
            SendWeekSettingOperationItems = wpitem;
            var pitem = new ObservableCollection<OperatorOneRecordViewModel>();
            foreach (var i in info.ItemsSndArgu)
            {
                if (i.OperatorType == 11)
                    pitem.Add(new OperatorOneRecordViewModel()
                                  {
                                      Time = new DateTime(i.DateCreate),
                                      OperatorType1 = i.IsClientSnd == 1 ? 4 : i.IsClientSnd == 3 ? 5 : 0,
                                      Brush = new SolidColorBrush(Colors.Blue)
                                  });
            }
            PairItems = pitem;

            var ritem = new ObservableCollection<OperatorOneRecordViewModel>();
            foreach (var i in info.ItemsSndArgu)
            {
                if (i.OperatorType == 26)
                    ritem.Add(new OperatorOneRecordViewModel()
                    {
                        Time = new DateTime(i.DateCreate),
                        OperatorType1 = i.IsClientSnd == 1 ? 10 : i.IsClientSnd == 3 ? 10 : 0,
                        Brush = new SolidColorBrush(Colors.DimGray)
                    });
            }
            ResetTerminalItems = ritem;
            //空白图表
            if (opitem.Count > 0 || spitem.Count > 0 || wpitem.Count > 0 || pitem.Count > 0 || ritem.Count > 0)
            {
                Items.Clear();
                for (var i = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month,
                                          BeginDate.AddDays(-1).Day, 12, 0, 0);
                     i < new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 14, 0, 0);)
                {
                    Items.Add(new OperatorOneRecordViewModel()
                                  {
                                      Time = i,
                                      OperatorType1 = 0,
                                      Brush = new SolidColorBrush(Colors.White)
                                  });

                    i = i.AddHours(2);
                }
            }
            var operatoritem = new ObservableCollection<OperatorOneRecordViewModel>();
            if (IsOpenOrClose)
                foreach (var t in opitem)
                    operatoritem.Add(t);
            if (IsPair)
                foreach (var t in pitem)
                    operatoritem.Add(t);
            if (IsSendParameters)
                foreach (var t in spitem)
                    operatoritem.Add(t);
            if (IsSendWeekSetting)
                foreach (var t in wpitem)
                    operatoritem.Add(t);
            OperationItems = operatoritem;

        }

        public void AddLines(ObservableCollection<Fault> oit, int id, Color color)
        {
            var line = new LineSeries();
            line.Stroke = new SolidColorBrush(color);
            line.StrokeThickness = 2;
            foreach (var f in oit)
            {
                if (f.FaultId == id)
                    line.DataPoints.Add(new CategoricalDataPoint() {Value = f.Count, Category = f.DtCreateTime});
            }
            _radchart2.Series.Add(line);
        }



        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core,true);

            this.AddEventFilterInfo(9999,"OperatorTypeItemfffffffffffff",true );
            this.AddEventFilterInfo(1111, "TerminalFaultCategories", true);
        }



        public override void ExPublishedEvent(
            PublishEventArgs args)
        {


            try
            {

                if (args.EventId == Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected)
                {
                    
                    int id = Convert.ToInt32(args.GetParams()[0]);
                    var ids = id;
                    if (id > 1100000)
                    {
                        //var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                        //if (tmps == null) return;
                        //id = tmps.RtuFid;
                        var tmps = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                        if (tmps == null) return;

                        if (id > 1500000 && id < 1600000)
                        {
                            var sss = tmps as Wj2090Slu;
                            if (sss == null) return;

                            var relatedRtuId = sss.WjSlu.RelatedRtuId;
                            if (relatedRtuId == 0) return;
                            ids =
                                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetLidByPhyId(
                                    relatedRtuId, WjParaBase.EquType.Rtu);

                        }

                        if(id > 1600000 && id < 1700000)
                        {
                            var sss = tmps as Wj9001Leak;
                            if (sss == null) return;

                            var relatedRtuId = sss.RtuFid;
                            if (relatedRtuId == 0) return;
                            ids = relatedRtuId;
                        }

                    }
                    if (id < 1000000 || (id > 1100000 && id < 1500000) || id > 1700000) return;

                    SelectRtuIdChange(ids);
                }

                if (args.EventId == 9999 && args.EventType == "OperatorTypeItemfffffffffffff")
                {
                    _radchart2.Series.Clear();
                    AddLines(FaultItems, 0, Colors.Red);
                    foreach (var f in TypeItems)
                    {
                        if (f.IsSelected == false) continue;
                        var line = new LineSeries();
                        line.Stroke = f.Color;
                        line.StrokeThickness = 2;
                        foreach (var g in FaultItems)
                        {
                            if (f.Value == g.FaultId)
                                line.DataPoints.Add(new CategoricalDataPoint()
                                                        {Value = g.Count, Category = g.DtCreateTime});
                        }
                        _radchart2.Series.Add(line);
                    }
                    if (_radchart2.Series.Count < 1)
                    {
                        var itemss = new ObservableCollection<Fault>();
                        for (int i = 0; i < 7; i++)
                        {
                            itemss.Add(new Fault()
                            {
                                DtCreateTime = BeginDate.AddDays(-i),
                                Count = 0
                            });
                        }
                        AddLines(itemss, 0, Colors.Red);
                    }
                }

                if (args.EventId == 1111 && args.EventType == "TerminalFaultCategories")
                {
                    _radchart2.Series.Clear();
                    //AddLines(HistoryFaultItem, 0, Colors.Red);
                    foreach (var f in FaultCategories)
                    {
                        if (f.IsSelected)
                        {
                            if (f.Value == 1)
                                AddLines(FaultItems, 1000, Colors.Gray);
                            if (f.Value == 2)
                                AddLines(FaultItems, 1001, Colors.Green);
                            if (f.Value == 3)
                                AddLines(FaultItems, 1002, Colors.Orange);
                            if (f.Value == 4)
                                AddLines(FaultItems, 1003, Colors.Purple);
                            if (f.Value == 5)
                                AddLines(FaultItems, 1004, Colors.Blue);
                            if (f.Value == 6)
                                AddLines(FaultItems, 0, Colors.Red);
                        }
                    }
                    if (_radchart2.Series.Count < 1)
                    {
                        var itemss = new ObservableCollection<Fault>();
                        for (int i = 0; i < 7; i++)
                        {
                            itemss.Add(new Fault()
                            {
                                DtCreateTime = BeginDate.AddDays(-i),
                                Count = 0
                            });
                        }
                        AddLines(itemss, 0, Colors.Red);
                    }
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
            if (rtuId != this.RtuId)
            {
                this.RtuId = rtuId;

                LineItemss.Clear();
                EnergyItems.Clear();
                FaultItems.Clear();
                OperationItems.Clear();
                _radchart2.Series.Clear();
                for (
                var i = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month,
                                          BeginDate.AddDays(-1).Day, 12, 0, 0);
                i < new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 14, 0, 0);
                )
                {
                    Items.Add(new OperatorOneRecordViewModel()
                    {
                        Time = i,
                        OperatorType1 = 0,
                        Brush = new SolidColorBrush(Colors.White)
                    });

                    i = i.AddHours(2);
                }

                for (int i = 0; i < 7; i++)
                {
                    FaultItems.Add(new Fault()
                    {
                        DtCreateTime = BeginDate.AddDays(-i),
                        Count = 0
                    });
                }
                AddLines(FaultItems, 0, Colors.Red);

                for (int i = 0; i < 7; i++)
                {
                    EnergyItems.Add(new EnergyItem()
                    {
                        DateCreate = BeginDate.AddDays(-i),
                        ElecVaule = 0
                    });
                }
                Step1 = 1;
                Step2 = 1;
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
        private string _tooltips1;
        private string _tooltips2;
        private Brush _color;

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

        private int _m1;
        public int M1
        {
            get { return _m1; }
            set
            {
                if (_m1 != value)
                {
                    _m1 = value;
                    this.RaisePropertyChanged(() => this.M1);
                }
            }
        }

        private int _m2;
        public int M2
        {
            get { return _m2; }
            set
            {
                if (_m2 != value)
                {
                    _m2 = value;
                    this.RaisePropertyChanged(() => this.M2);
                }
            }
        }

        private int _n1;
        public int N1
        {
            get { return _n1; }
            set
            {
                if (_n1 != value)
                {
                    _n1 = value;
                    this.RaisePropertyChanged(() => this.N1);
                }
            }
        }

        private int _n2;
        public int N2
        {
            get { return _n2; }
            set
            {
                if (_n2 != value)
                {
                    _n2 = value;
                    this.RaisePropertyChanged(() => this.N2);
                }
            }
        }

        private int _a1;
        public int A1
        {
            get { return _a1; }
            set
            {
                if (_a1 != value)
                {
                    _a1 = value;
                    this.RaisePropertyChanged(() => this.A1);
                }
            }
        }

        private int _a2;
        public int A2
        {
            get { return _a2; }
            set
            {
                if (_a2 != value)
                {
                    _a2 = value;
                    this.RaisePropertyChanged(() => this.A2);
                }
            }
        }

        private int _b1;
        public int B1
        {
            get { return _b1; }
            set
            {
                if (_b1 != value)
                {
                    _b1 = value;
                    this.RaisePropertyChanged(() => this.B1);
                }
            }
        }

        private int _b2;
        public int B2
        {
            get { return _b2; }
            set
            {
                if (_b2 != value)
                {
                    _b2 = value;
                    this.RaisePropertyChanged(() => this.B2);
                }
            }
        }

        public string Tooltips1
        {
            get { return _tooltips1; }
            set
            {
                if (_tooltips1 != value)
                {
                    _tooltips1 = value;
                    this.RaisePropertyChanged(() => this.Tooltips1);
                }
            }
        }

        public string Tooltips2
        {
            get { return _tooltips2; }
            set
            {
                if (_tooltips2 != value)
                {
                    _tooltips2 = value;
                    this.RaisePropertyChanged(() => this.Tooltips2);
                }
            }
        }

        private string _sumTime;
        public string SumTime
        {
            get { return _sumTime; }
            set
            {
                if (_sumTime != value)
                {
                    _sumTime = value;
                    this.RaisePropertyChanged(() => this.SumTime);
                }
            }
        }

        private string _open;
        public string Open
        {
            get { return _open; }
            set
            {
                if (_open != value)
                {
                    _open = value;
                    this.RaisePropertyChanged(() => this.Open);
                }
            }
        }

        private string _close;
        public string Close
        {
            get { return _close; }
            set
            {
                if (_close != value)
                {
                    _close = value;
                    this.RaisePropertyChanged(() => this.Close);
                }
            }
        }

        public Brush Color
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

    public class OperatorOneRecordViewModel : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        #region Time

        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set
            {
                if (_time == value) return;
                _time = value;
                RaisePropertyChanged(() => Time);
            }
        }

        #endregion

        #region OperatorType1
        private int _operatorType1;
        /// <summary>
        /// 操作类型
        /// </summary>
        public int OperatorType1
        {
            get { return _operatorType1; }
            set
            {
                if (_operatorType1 == value) return;
                _operatorType1 = value;
                //if (value == 19 || value == 20 || value == 21)
                //    _operatorType1 = 1;
                //else if (value == 11)
                //    _operatorType1 = 2;
                //else if (value >= 12 && value <= 18)
                //    _operatorType1 = 4;
                //else
                //    _operatorType1 = 3;
                RaisePropertyChanged(() => OperatorType1);

            }
        }
        #endregion

        private Brush _brush;
        public Brush Brush
        {
            get { return _brush; }
            set
            {
                if(_brush==value) return;
                _brush = value;
                RaisePropertyChanged(() => Brush);
            }
        }
    }

    public class Fault : Wlst.Cr.Core.CoreServices.ObservableObject
    {

        private int _faultId;
        /// <summary>
        /// 故障类型
        /// </summary>
        public int FaultId
        {
            get { return _faultId; }
            set
            {
                if (value == _faultId) return;
                _faultId = value;
                RaisePropertyChanged(() => FaultId);
            }
        }

        private DateTime _dtCreateTime;

        public DateTime DtCreateTime
        {
            get { return _dtCreateTime; }
            set
            {
                if (_dtCreateTime != value)
                {
                    _dtCreateTime = value;
                    this.RaisePropertyChanged(() => this.DtCreateTime);
                }
            }
        }

        private int _count;
        /// <summary>
        /// 故障总数
        /// </summary>
        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    this.RaisePropertyChanged(() => this.Count);
                }
            }
        }

    }

    /// <summary>
    /// 故障类型模型定义
    /// </summary>
    public class OperatorTypeItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private bool _check;
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
                                                      EventId = 9999,
                                                      EventType = "OperatorTypeItemfffffffffffff"
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
            get { return _value ; }
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

    public class EnergyItem:Wlst.Cr.Core.CoreServices.ObservableObject
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

        #region ElecVaule

        private double _elecVaule;
        public double ElecVaule
        {
            get { return _elecVaule; }
            set
            {
                if (_elecVaule == value) return;
                _elecVaule = value;
                RaisePropertyChanged(() => ElecVaule);
            }
        }

        #endregion
    }

    public class OpenOrClose:Wlst.Cr.Core.CoreServices.ObservableObject
    {

        private string _loop;
        /// <summary>
        /// 回路
        /// </summary>
        public string Loop
        {
            get { return _loop; }
            set
            {
                if (value == _loop) return;
                _loop = value;
                RaisePropertyChanged(() => Loop);
            }
        }

        private ObservableCollection<DateTime> _open;
        /// <summary>
        /// 开灯时间
        /// </summary>
        public ObservableCollection<DateTime> Open
        {
            get { return _open; }
            set
            {
                if (value == _open) return;
                _open = value;
                RaisePropertyChanged(() => Open);
            }
        }

        private ObservableCollection<DateTime> _close;
        /// <summary>
        /// 关灯时间
        /// </summary>
        public ObservableCollection<DateTime> Close
        {
            get { return _close; }
            set
            {
                if (value == _close) return;
                _close = value;
                RaisePropertyChanged(() => Close);
            }
        }

        private double _length;
        /// <summary>
        /// 开灯时长
        /// </summary>
        public double Length
        {
            get { return _length; }
            set
            {
                if (value == _length) return;
                _length = value;
                RaisePropertyChanged(() => Length);
            }
        }

        private double _location;
        /// <summary>
        /// 位置
        /// </summary>
        public double Location
        {
            get { return _location; }
            set
            {
                if (value == _location) return;
                _location = value;
                RaisePropertyChanged(() => Location);
            }
        }
    }

    public class NameIntBool : Wlst.Cr.Core.CoreServices.ObservableObject
    {


        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => this.Name);
                }
            }
        }

        private int _areaid;

        public int AreaId
        {
            get { return _areaid; }
            set
            {
                if (_areaid != value)
                {
                    _areaid = value;
                    this.RaisePropertyChanged(() => this.AreaId);
                }
            }
        }

        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    this.RaisePropertyChanged(() => this.Value);
                }
            }
        }

        private bool _check;

        public bool IsSelected
        {
            get { return _check; }
            set
            {
                if (_check != value)
                {
                    _check = value;
                    this.RaisePropertyChanged(() => this.IsSelected);
                    IsVisi = value ? Visibility.Visible : Visibility.Collapsed;

                    var args = new PublishEventArgs()
                    {
                        EventId = 1111,
                        EventType = "TerminalFaultCategories"
                    };
                    EventPublish.PublishEvent(args);
                }
            }
        }

        private bool _isEnable;

        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (_isEnable != value)
                {
                    _isEnable = value;
                    this.RaisePropertyChanged(() => this.IsEnable);
                }
            }
        }

        private bool _isShow;

        public bool IsShow
        {
            get { return _isShow; }
            set
            {
                if (_isShow != value)
                {
                    _isShow = value;
                    this.RaisePropertyChanged(() => this.IsShow);
                }
            }
        }

        private Visibility _isVisi;

        public Visibility IsVisi
        {
            get { return _isVisi; }
            set
            {
                if (_isVisi != value)
                {
                    _isVisi = value;
                    this.RaisePropertyChanged(() => this.IsVisi);
                }
            }
        }


    }
}
