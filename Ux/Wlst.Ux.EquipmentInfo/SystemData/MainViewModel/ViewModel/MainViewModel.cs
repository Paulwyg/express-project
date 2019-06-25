using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Charting;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreMims.Services;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.EquipmentInfo.SystemData.MainViewModel.Services;

namespace Wlst.Ux.EquipmentInfo.SystemData.MainViewModel.ViewModel
{
    [Export(typeof (IIMainViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class MainViewModel : EventHandlerHelperExtendNotifyProperyChanged, IIMainViewModel
    {
        private RadCartesianChart _radchart2 = null;
        public MainViewModel(RadCartesianChart radchart)
        {
            InitAction();
            InitEvent();
            _radchart2 = radchart;
        }

        public void NavOnLoad(params object[] parsObjects)
        {
            ImageIcon1 = Directory.GetCurrentDirectory() + "\\Image\\Png\\sunrise.png";
            ImageIcon2 = Directory.GetCurrentDirectory() + "\\Image\\Png\\sunset.png";
            Items.Clear();
            Items1.Clear();
            Items2.Clear();
            LineItemss.Clear();
            SingleLampItems.Clear();
            OnlineWeekSluItems.Clear();
            OnlineWeekRtuItems.Clear();
            FaultItem.Clear();
            DailyFaultItem.Clear();
            HistoryFaultItem.Clear();
            EnergyItems.Clear();
            CurrentItems.Clear();
            OnlineRtuItems.Clear();
            OnlineSluItems.Clear();
            BeginDate = DateTime.Now;
            GetSystemInfo();
            GetAllFaultName();
            InitSun();
            for (
                var i = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month,
                                          BeginDate.AddDays(-1).Day, 12, 0, 0);
                i < new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 14, 0, 0);
                )
            {
                Items.Add(new Energy()
                              {
                                  DateCreate = i,
                                  RtuEnergy = 0
                              });
                Items1.Add(new Online()
                               {
                                   DateCreate = i,
                                   OnlineRate = 0
                               });
                Items2.Add(new Online()
                               {
                                   DateCreate = i,
                                   OnlineRate = 0
                               });
                i = i.AddHours(2);
            }

            for (int i = 0; i < 7; i++)
            {
                SingleLampItems.Add(new SingleLampControl()
                                        {
                                            DateCreate = BeginDate.AddDays(-i),
                                            ControlRate = 0,
                                            LightRate = 0
                                        });
                OnlineWeekSluItems.Add(new Online()
                                           {
                                               DateCreate = BeginDate.AddDays(-i),
                                               OnlineRate = 0
                                           });
                OnlineWeekRtuItems.Add(new Online()
                                           {
                                               DateCreate = BeginDate.AddDays(-i),
                                               OnlineRate = 0
                                           });
                HistoryFaultItem.Add(new HistoryFault()
                                         {
                                             DateCreate = BeginDate.AddDays(-i),
                                             Count = 0
                                         });
                EnergyItems.Add(new Energy()
                                    {
                                        DateCreate = BeginDate.AddDays(-i),
                                        SluEnergy = 0,
                                        RtuEnergy = 0
                                    });
            }

            FaultItem.Add(new Fault()
                              {
                                  Count = 0,
                                  Name = "故障"
                              });

            DailyFaultItem.Add(new Fault()
                                   {
                                       Count = 0,
                                       Name = "故障"
                                   });

            _radchart2.Series.Clear();
            AddLines(HistoryFaultItem, 0,Colors.Red);

            Step1 = 1;
            Step2 = 1;
            Step3 = 1;
            Step4 = 1;
            Step5 = 1;
            Step6 = 1;
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
                return "系统数据";
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

        public void OnUserHideOrClosing()
        {
            TerminalNumber = 0;
            UsedTerminalNumber = 0;
            LuxNumber = 0;
            ElectricMeterNumber = 0;
            LeakageNumber = 0;
            LineNumber = 0;
            UsedLineNumber = 0;
            ConcentratorNumber = 0;
            ControllerNumber = 0;
            LineDetectionNumber = 0;
            LoopNumber = 0;
            ExistFaultNumber = 0;
            DailyFaultNumber = 0;
            Visi = Visibility.Hidden;
            VisiLine = Visibility.Hidden;
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

        private int _step3;

        /// <summary>
        /// 
        /// </summary>
        public int Step3
        {
            get { return _step3; }
            set
            {
                if (_step3 == value) return;
                _step3 = value;
                RaisePropertyChanged(() => Step3);
            }
        }

        private int _step4;

        /// <summary>
        /// 
        /// </summary>
        public int Step4
        {
            get { return _step4; }
            set
            {
                if (_step4 == value) return;
                _step4 = value;
                RaisePropertyChanged(() => Step4);
            }
        }

        private int _step5;

        /// <summary>
        /// 
        /// </summary>
        public int Step5
        {
            get { return _step5; }
            set
            {
                if (_step5 == value) return;
                _step5 = value;
                RaisePropertyChanged(() => Step5);
            }
        }

        private int _step6;

        /// <summary>
        /// 
        /// </summary>
        public int Step6
        {
            get { return _step6; }
            set
            {
                if (_step6 == value) return;
                _step6 = value;
                RaisePropertyChanged(() => Step6);
            }
        }
        #endregion

        #region Items

        private ObservableCollection<Energy> _items;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Energy> Items
        {
            get { return _items ?? (_items = new ObservableCollection<Energy>()); }
            set
            {
                if (_items == value) return;
                _items = value;
                this.RaisePropertyChanged(() => this.Items);
            }
        }

        private ObservableCollection<Online> _items1;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Online> Items1
        {
            get { return _items1 ?? (_items1 = new ObservableCollection<Online>()); }
            set
            {
                if (_items1 == value) return;
                _items1 = value;
                this.RaisePropertyChanged(() => this.Items1);
            }
        }

        private ObservableCollection<Online> _items2;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Online> Items2
        {
            get { return _items2 ?? (_items2 = new ObservableCollection<Online>()); }
            set
            {
                if (_items2 == value) return;
                _items2 = value;
                this.RaisePropertyChanged(() => this.Items2);
            }
        }
        #endregion

        #region 系统消息
        private int _terminalNumber;
        /// <summary>
        /// 终端数量
        /// </summary>
        public int TerminalNumber
        {
            get { return _terminalNumber; }
            set
            {
                if (_terminalNumber == value) return;
                _terminalNumber = value;
                this.RaisePropertyChanged(() => this.TerminalNumber);
            }
        }

        private int _usedTerminalNumber;
        /// <summary>
        /// 使用数量
        /// </summary>
        public int UsedTerminalNumber
        {
            get { return _usedTerminalNumber; }
            set
            {
                if (_usedTerminalNumber == value) return;
                _usedTerminalNumber = value;
                this.RaisePropertyChanged(() => this.UsedTerminalNumber);
            }
        }

        private int _luxNumber;
        /// <summary>
        /// 光控数量
        /// </summary>
        public int LuxNumber
        {
            get { return _luxNumber; }
            set
            {
                if (_luxNumber == value) return;
                _luxNumber = value;
                this.RaisePropertyChanged(() => this.LuxNumber);
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

        #region RecordSun

        private ObservableCollection<SunItem> _recordSun;
        /// <summary>
        /// 日出日落
        /// </summary>
        public ObservableCollection<SunItem> RecordSun
        {
            get { return _recordSun ?? (_recordSun = new ObservableCollection<SunItem>()); }
            set
            {
                if (_recordSun != value)
                {
                    _recordSun = value;
                    this.RaisePropertyChanged(() => this.RecordSun);
                }
            }
        }

        #endregion

        #region FaultItem
        private ObservableCollection<Fault> _faultItem;
        /// <summary>
        /// 现存故障
        /// </summary>
        public ObservableCollection<Fault> FaultItem
        {
            get { return _faultItem ?? (_faultItem = new ObservableCollection<Fault>()); }
            set
            {
                if (_faultItem != value)
                {
                    _faultItem = value;
                    this.RaisePropertyChanged(() => this.FaultItem);
                }
            }
        }
        #endregion

        #region HistoryFaultItem
        private ObservableCollection<HistoryFault> _historyFaultItem;
        /// <summary>
        /// 历史故障
        /// </summary>
        public ObservableCollection<HistoryFault> HistoryFaultItem
        {
            get { return _historyFaultItem ?? (_historyFaultItem = new ObservableCollection<HistoryFault>()); }
            set
            {
                if (_historyFaultItem != value)
                {
                    _historyFaultItem = value;
                    this.RaisePropertyChanged(() => this.HistoryFaultItem);
                }
            }
        }
        #endregion

        public static ObservableCollection<OperatorTypeItem> PassTypeItems;

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
                PassTypeItems = value;
            }
        }
        #endregion

        #region DailyFaultItem
        private ObservableCollection<Fault> _dailyFaultItem;
        /// <summary>
        /// 当天现存故障
        /// </summary>
        public ObservableCollection<Fault> DailyFaultItem
        {
            get { return _dailyFaultItem ?? (_dailyFaultItem = new ObservableCollection<Fault>()); }
            set
            {
                if (_dailyFaultItem != value)
                {
                    _dailyFaultItem = value;
                    this.RaisePropertyChanged(() => this.DailyFaultItem);
                }
            }
        }
        #endregion

        #region ExistFaultNumber
        private int _existFaultNumber;
        /// <summary>
        /// 现存故障数量
        /// </summary>
        public int ExistFaultNumber
        {
            get { return _existFaultNumber; }
            set
            {
                if (_existFaultNumber == value) return;
                _existFaultNumber = value;
                this.RaisePropertyChanged(() => this.ExistFaultNumber);
            }
        }
        #endregion

        #region DailyFaultNumber
        private int _dailyFaultNumber;
        /// <summary>
        /// 今日故障数量
        /// </summary>
        public int DailyFaultNumber
        {
            get { return _dailyFaultNumber; }
            set
            {
                if (_dailyFaultNumber == value) return;
                _dailyFaultNumber = value;
                this.RaisePropertyChanged(() => this.DailyFaultNumber);
            }
        }
        #endregion

        #region SingleLampItems

        private ObservableCollection<SingleLampControl> _singleLampItems;
        /// <summary>
        /// 单灯信息
        /// </summary>
        public ObservableCollection<SingleLampControl> SingleLampItems
        {
            get { return _singleLampItems ?? (_singleLampItems = new ObservableCollection<SingleLampControl>()); }
            set
            {
                if (_singleLampItems == value) return;
                _singleLampItems = value;
                this.RaisePropertyChanged(() => this.SingleLampItems);
            }
        }

        #endregion

        #region EnergyItems

        private ObservableCollection<Energy> _energyItems;
        /// <summary>
        /// 能耗信息
        /// </summary>
        public ObservableCollection<Energy> EnergyItems
        {
            get { return _energyItems ?? (_energyItems = new ObservableCollection<Energy>()); }
            set
            {
                if (_energyItems == value) return;
                _energyItems = value;
                this.RaisePropertyChanged(() => this.EnergyItems);
            }
        }

        #endregion

        #region CurrentItems

        private ObservableCollection<Energy> _currentItems;
        /// <summary>
        /// 电流信息
        /// </summary>
        public ObservableCollection<Energy> CurrentItems
        {
            get { return _currentItems ?? (_currentItems = new ObservableCollection<Energy>()); }
            set
            {
                if (_currentItems == value) return;
                _currentItems = value;
                this.RaisePropertyChanged(() => this.CurrentItems);
            }
        }

        #endregion

        #region OnlineRtuItems

        private ObservableCollection<Online> _onlineRtuItems;
        /// <summary>
        /// 终端在线率信息
        /// </summary>
        public ObservableCollection<Online> OnlineRtuItems
        {
            get { return _onlineRtuItems ?? (_onlineRtuItems = new ObservableCollection<Online>()); }
            set
            {
                if (_onlineRtuItems == value) return;
                _onlineRtuItems = value;
                this.RaisePropertyChanged(() => this.OnlineRtuItems);
            }
        }

        #endregion

        #region OnlineSluItems

        private ObservableCollection<Online> _onlineSluItems;
        /// <summary>
        /// 集中器在线率信息
        /// </summary>
        public ObservableCollection<Online> OnlineSluItems
        {
            get { return _onlineSluItems ?? (_onlineSluItems = new ObservableCollection<Online>()); }
            set
            {
                if (_onlineSluItems == value) return;
                _onlineSluItems = value;
                this.RaisePropertyChanged(() => this.OnlineSluItems);
            }
        }

        #endregion

        #region OnlineWeekSluItems

        private ObservableCollection<Online> _onlineWeekSluItems;
        /// <summary>
        /// 集中器一周在线率信息
        /// </summary>
        public ObservableCollection<Online> OnlineWeekSluItems
        {
            get { return _onlineWeekSluItems ?? (_onlineWeekSluItems = new ObservableCollection<Online>()); }
            set
            {
                if (_onlineWeekSluItems == value) return;
                _onlineWeekSluItems = value;
                this.RaisePropertyChanged(() => this.OnlineWeekSluItems);
            }
        }

        #endregion

        #region OnlineWeekRtuItems

        private ObservableCollection<Online> _onlineWeekRtuItems;
        /// <summary>
        /// 终端一周在线率信息
        /// </summary>
        public ObservableCollection<Online> OnlineWeekRtuItems
        {
            get { return _onlineWeekRtuItems ?? (_onlineWeekRtuItems = new ObservableCollection<Online>()); }
            set
            {
                if (_onlineWeekRtuItems == value) return;
                _onlineWeekRtuItems = value;
                this.RaisePropertyChanged(() => this.OnlineWeekRtuItems);
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

        #region SunRise
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
        #endregion

        #region SunSet
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
        #endregion

        #region XSunRise
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
        #endregion

        #region XSunSet
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
        #endregion

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

        private Visibility _visiLine = Visibility.Hidden;
        /// <summary>
        /// 
        /// </summary>
        public Visibility VisiLine
        {
            get
            {
                return _visiLine;
            }
            set
            {
                if (value == _visiLine) return;
                _visiLine = value;
                RaisePropertyChanged(() => VisiLine);
            }
        }

        private Visibility _visiOne = Visibility.Collapsed;
        /// <summary>
        /// 
        /// </summary>
        public Visibility VisiOne
        {
            get
            {
                return _visiOne;
            }
            set
            {
                if (value == _visiOne) return;
                _visiOne = value;
                RaisePropertyChanged(() => VisiOne);
            }
        }
        #endregion

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

        private int _lineLength;
        /// <summary>
        /// 
        /// </summary>
        public int LineLength
        {
            get { return _lineLength; }
            set
            {
                if (value != _lineLength)
                {
                    _lineLength = value;
                    this.RaisePropertyChanged(() => this.LineLength);
                }
            }
        }

        private int _lineLength1;
        /// <summary>
        /// 
        /// </summary>
        public int LineLength1
        {
            get { return _lineLength1; }
            set
            {
                if (value != _lineLength1)
                {
                    _lineLength1 = value;
                    this.RaisePropertyChanged(() => this.LineLength1);
                }
            }
        }

        private int _lineLength2=100;
        /// <summary>
        /// 
        /// </summary>
        public int LineLength2
        {
            get { return _lineLength2; }
            set
            {
                if (value != _lineLength2)
                {
                    _lineLength2 = value;
                    this.RaisePropertyChanged(() => this.LineLength2);
                }
            }
        }

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

                    _faultCategories.Add(new NameIntBool() { Name = "终端故障", Value = 1, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "开关灯故障", Value = 2, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "终端故障", Value = 3, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "其他故障", Value = 4, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "故障总数", Value = 5, IsSelected = true });
                }
                return _faultCategories;
            }
        }
        #endregion
    }

    public partial class MainViewModel
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
            QueryFault();
            var dtStartTime = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 0, 0, 0);
            var dtEndTime = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 23, 59, 59);
            Sr.EquipemntLightFault.Services.PreErrorServices.ReqeustPreExistError(dtStartTime, dtEndTime);
            QuerySingleLamp();
            QueryHisFault();
            QueryEnergy();
            QueryOnline();
        }
        private bool CanExCmdQuery()
        {
            return DateTime.Now.Ticks - _dtCmdQuery.Ticks > 30000000;
        }
        #endregion

        //#region CmdExistFault
        //private DateTime _dtCmdExistFault;
        //private ICommand _cmdCmdExistFault;
        //public ICommand CmdExistFault
        //{
        //    get
        //    {
        //        if (_cmdCmdExistFault == null)
        //            _cmdCmdExistFault = new RelayCommand(ExCmdExistFault, CanCmdExistFault, false);
        //        return _cmdCmdExistFault;
        //    }
        //}
        //private void ExCmdExistFault()
        //{
        //    _dtCmdExistFault = DateTime.Now;
        //    var faulttype = 0;
            
        //    RegionManage.ShowViewByIdAttachRegionWithArgu(1103603, rtulst);
        //}
        //private bool CanCmdExistFault()
        //{
        //    return DateTime.Now.Ticks - _dtCmdExistFault.Ticks > 30000000;
        //}
        //#endregion
    }

    public partial class MainViewModel
    {
        private void GetSystemInfo()
        {
            foreach (var t in Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
            {
                if (t.Value.EquipmentType == WjParaBase.EquType.Rtu)
                {
                    TerminalNumber++;
                    var x = t.Value as Wj3005Rtu;
                    if (x != null && x.RtuRealState == 0)
                        UsedTerminalNumber++;
                }
                if (t.Value.EquipmentType == WjParaBase.EquType.Lux)
                    LuxNumber++;
                if (t.Value.EquipmentType == WjParaBase.EquType.Mru)
                    ElectricMeterNumber++;
                if (t.Value.EquipmentType == WjParaBase.EquType.Leak)
                {
                    LeakageNumber++;
                    var x = t.Value as Wj9001Leak;
                    if (x != null)
                    {
                        LineNumber += x.WjLeakLines.Count;
                        foreach (var f in x.WjLeakLines)
                        {
                            if (f.Value.IsUsed)
                                UsedLineNumber++;
                        }
                    }
                }
                if (t.Value.EquipmentType == WjParaBase.EquType.Slu)
                {
                    ConcentratorNumber++;
                    var x = t.Value as Wj2090Slu;
                    if (x != null)
                        ControllerNumber += x.WjSluCtrls.Count;                       
                }
                if (t.Value.EquipmentType == WjParaBase.EquType.Ldu)
                {
                    LineDetectionNumber++;
                    var x = t.Value as Wj1090Ldu;
                    if (x != null)
                        LoopNumber += x.WjLduLines.Count;
                }
            }
        }
        private void InitSun()
        {
            if (RecordSun.Count == 31) return;
            RecordSun.Clear();

                var tmp = new ObservableCollection<SunItem>();

                for (int i = 1; i < 13; i++)
                {
                    for (int j = 1; j < 32; j++)
                    {
                        var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(i, j);
                        try
                        {
                            if (info != null)
                            {
                                tmp.Add(new SunItem()
                                            {
                                                Value = info.time_sunrise,
                                                Value2 = info.time_sunset,
                                                Time = new DateTime(DateTime.Now.Year, i, j)
                                            });

                            }
                            else
                            {
                                tmp.Add(new SunItem()
                                            {
                                                Value = 0,
                                                Value2 = 0,
                                                Time = new DateTime(DateTime.Now.Year, i, j)
                                            });
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
            RecordSun = tmp;

            var infos = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(BeginDate.Month, BeginDate.Day);
            SunRise = (infos.time_sunrise/60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ":" +
                      (infos.time_sunrise%60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            SunSet = (infos.time_sunset/60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ":" +
                     (infos.time_sunset%60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            XSunRise = 90 + (infos.time_sunrise + 720) / 3;
            XSunSet = 90 + (infos.time_sunset - 720) / 3;
            X = 80 + (infos.time_sunrise + 720) / 3;
            X1 = 80 + (infos.time_sunset - 720) / 3;
            Visi = Visibility.Visible;
        }
        private void QueryFault()
        {
            FaultItem.Clear();
            var dts = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 0, 0, 1).Ticks;
            var tmox = (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                        orderby t.DateCreate descending
                        select t).ToList();
            ExistFaultNumber = tmox.Count;
            var fitem = new ObservableCollection<Fault>();
            foreach (var t in tmox)
            {
                //if (t.DateCreate.Ticks > dts) continue;
                var j = 0;
                foreach (var f in fitem)
                {
                    if (t.FaultName == f.Name)
                    {
                        f.Count++;
                        break;
                    }
                    j++;
                }
                if (j == fitem.Count)
                    fitem.Add(new Fault()
                                      {
                                          Name = t.FaultName,
                                          Count = 1
                                      });
            }
            if (fitem.Count < 1)
            {
                FaultItem.Add(new Fault()
                                  {
                                      Count = 0,
                                      Name = "故障"
                                  });
            }
            foreach (var t in fitem)
            {
                foreach (var f in TypeItems)
                {
                    if (f.Name.Contains(t.Name))
                    {
                        t.Name = f.Name.Split('-')[0];
                        break;
                    }
                }
            }
            var xx = fitem.Select(t => t.Count).Concat(new[] {1}).Max();
            Step1 = xx/5 > 1 ? xx/5 : 1;
            foreach (var x in fitem.OrderByDescending(x=>x.Count))
            {
                FaultItem.Add(x);
            }
        }
        private void FaultType()
        {
            //var color = new List<Brush>();
            //color.Add(new SolidColorBrush(Colors.Orange));
            //color.Add(new SolidColorBrush(Colors.Blue));
            //color.Add(new SolidColorBrush(Colors.Brown));
            //color.Add(new SolidColorBrush(Colors.Chartreuse));
            //color.Add(new SolidColorBrush(Colors.BlueViolet));
            //color.Add(new SolidColorBrush(Colors.Cyan));
            //color.Add(new SolidColorBrush(Colors.DarkGreen));
            //color.Add(new SolidColorBrush(Colors.DeepPink));
            //color.Add(new SolidColorBrush(Colors.DimGray));
            //color.Add(new SolidColorBrush(Colors.OrangeRed));

            var item = new ObservableCollection<OperatorTypeItem>();
            //int i = 0;
            int j = 0;
            //foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            //{
            //    if (i >= color.Count)
            //        i = 0;
            //    if (!t.Value.IsEnable) continue;
            //    j++;
            //    //if (t.Key >= 1 && t.Key <= 30)
            //        item.Add(new OperatorTypeItem()
            //                     {
            //                         IsSelected = false,
            //                         Name = j + "-" + t.Value.FaultName,
            //                         Value = t.Key,
            //                         Color = color[i]
            //                     });
            //    i++;
            //}

            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                if (!t.Value.IsEnable) continue;

                if ((t.Key >= 1 && t.Key <= 5) || t.Key == 19 || (t.Key >= 22 && t.Key <= 24) || t.Key == 27 || t.Key == 29)
                {
                    j++;
                    item.Add(new OperatorTypeItem()
                                 {
                                     IsSelected = false,
                                     Name = j + "-" + t.Value.FaultNameByDefine ,
                                     Value = t.Key,
                                     Color = new SolidColorBrush(Colors.Gray)
                                 });
                }
            }
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                if (!t.Value.IsEnable) continue;

                if ((t.Key >= 6 && t.Key <= 17) || t.Key == 20 || t.Key == 21 || t.Key == 25 || t.Key == 26 || t.Key == 28)
                {
                    j++;
                    item.Add(new OperatorTypeItem()
                                 {
                                     IsSelected = false,
                                     Name = j + "-" + t.Value.FaultNameByDefine,
                                     Value = t.Key,
                                     Color = new SolidColorBrush(Colors.Green)
                                 });
                }
            }
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                if (!t.Value.IsEnable) continue;

                if (t.Key >= 50 && t.Key <= 65)
                {
                    j++;
                    item.Add(new OperatorTypeItem()
                                 {
                                     IsSelected = false,
                                     Name = j + "-" + t.Value.FaultNameByDefine,
                                     Value = t.Key,
                                     Color = new SolidColorBrush(Colors.Orange)
                                 });
                }
            }
            foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                if (!t.Value.IsEnable) continue;

                if (t.Key == 18 || (t.Key >= 30 && t.Key < 50) || t.Key > 65)
                {
                    j++;
                    item.Add(new OperatorTypeItem()
                                 {
                                     IsSelected = false,
                                     Name = j + "-" + t.Value.FaultNameByDefine,
                                     Value = t.Key,
                                     Color = new SolidColorBrush(Colors.Purple)
                                 });
                }
            }
            //i = 0;
            //foreach (var t in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            //{
            //    if (i >= color.Count)
            //        i = 0;
            //    if (!t.Value.IsEnable) continue;
            //    j++;
            //    if (t.Key >= 50 && t.Key < 80)
            //        item.Add(new OperatorTypeItem()
            //                     {
            //                         IsSelected = false,
            //                         Name = j + "-" + t.Value.FaultName,
            //                         Value = t.Key,
            //                         Color = color[i]
            //                     });
            //    i++;
            //}
            TypeItems = item;
            VisiOne=Visibility.Collapsed;
        }
        private void QuerySingleLamp()
        {
            var info = Wlst.Sr.ProtocolPhone.LxStatic.wst_static_oneday_sys1;
            info.WstStatisSysOneday1.DtTime = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day,12,0,0).Ticks;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void QueryHisFault()
        {
            var info = Wlst.Sr.ProtocolPhone.LxStatic.wst_static_oneday_sys2;
            info.WstStatisSysOneday2.DtTime = BeginDate.Ticks;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void QueryEnergy()
        {
            var info = Wlst.Sr.ProtocolPhone.LxStatic.wst_static_oneday_sys3;
            var date = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 12, 0, 0).Ticks;
            info.WstStatisSysOneday3.DtTime = date;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        private void QueryOnline()
        {
            var info = Wlst.Sr.ProtocolPhone.LxStatic.wst_static_oneday_sys4;
            var date = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 12, 0, 0).Ticks;
            info.WstStatisSysOneday4.DtTime =date;
            SndOrderServer.OrderSnd(info, 10, 6);
        }

        public void OnRequestFaultPre(string session, Wlst.mobile.MsgWithMobile infos)
        {
            DailyFaultItem.Clear();
            var info = infos.WstFaultPre;
            var tmox = (from t in Sr.EquipemntLightFault.Services.TmlExistFaultsInfoServices.InfoDictionary.Values
                        orderby t.DateCreate descending
                        select t).ToList();
            var fitem = new ObservableCollection<Fault>();
            var number = 0;
            var start = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 0, 0, 1).Ticks;
            var end = new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 23, 59, 59).Ticks;
            foreach (var t in tmox)
            {
                if (t.DateCreate.Ticks < start || t.DateCreate.Ticks > end) continue;
                var j = 0;
                foreach (var f in fitem)
                {
                    if (t.FaultName == f.Name)
                    {
                        f.Count++;
                        break;
                    }
                    j++;
                }
                if (j == fitem.Count)
                    fitem.Add(new Fault()
                                  {
                                      Name = t.FaultName,
                                      Count = 1
                                  });
                number++;
            }
            foreach (var f in info.FaultItems)
            {
                var j = 0;
                foreach (var x in fitem)
                {
                    if (GetFaultName(f.FaultId).Item1 == x.Name)
                    {
                        x.Count++;
                        break;
                    }
                    j++;
                }
                if (j == fitem.Count)
                    fitem.Add(new Fault()
                    {
                        Name = GetFaultName(f.FaultId).Item1,
                        Count = 1
                    });
            }
            if (fitem.Count < 1)
            {
                DailyFaultItem.Add(new Fault()
                {
                    Count = 0,
                    Name = "故障"
                });
            }
            foreach (var t in fitem)
            {
                foreach (var f in TypeItems)
                {
                    if (f.Name.Contains(t.Name))
                    {
                        t.Name = f.Name.Split('-')[0];
                        break;
                    }
                }
            }
           
            var xx = fitem.Select(t => t.Count).Concat(new[] { 1 }).Max();
            Step2 = xx/5 > 1 ? xx/5 : 1;
            foreach (var x in fitem.OrderByDescending(x => x.Count))
            {
                DailyFaultItem.Add(x);
            }
            DailyFaultNumber = info.FaultItems.Count + number;
        }

        private readonly List<NameIntBool> _faultName = new List<NameIntBool>();

        private void GetAllFaultName()
        {
            foreach (var item in Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.InfoDictionary)
            {
                _faultName.Add(new NameIntBool { Name = item.Value.FaultNameByDefine, Value = item.Value.FaultId, AreaId = item.Value.PriorityLevel });
            }
        }
        private Tuple<string, int> GetFaultName(int faultid)
        {
            foreach (var item in _faultName.Where(item => faultid == item.Value))
            {
                return new Tuple<string, int>(item.Name, item.AreaId);
            }
            return new Tuple<string, int>("no name", 0);
        }

        private void OnRequestOpenOrClose(string session, Wlst.mobile.MsgWithMobile infos)
        {
            VisiLine=Visibility.Collapsed;
            SingleLampItems.Clear();
            var info = infos.WstStatisSysOneday1;
            if (info == null) return;
            var sitem = new ObservableCollection<SingleLampControl>();
            for (int i = 0; i < 7; i++)
            {
                double rate1 = 0;
                double rate2 = 0;
                foreach (var f in info.ItemsSluCommwithld)
                {
                    if(f.SumTotalCtrl==0) continue;
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                    {
                        rate1 = (double)f.SumCommNormal/f.SumTotalCtrl;
                        rate2 = (double)f.SumLightOnCtrl/f.SumTotalCtrl;
                    }
                }
                sitem.Add(new SingleLampControl()
                {
                    DateCreate = BeginDate.AddDays(-i),
                    ControlRate = rate1,
                    LightRate = rate2
                });
            }
            SingleLampItems = sitem;

            var tmp = new Dictionary<string, List<Tuple<int, long>>>();
            foreach (var f in info.ItemTimetableExec)
            {
                if (f.TaskId == 11)
                {
                    if (tmp.ContainsKey(f.TaskName) == false) tmp.Add(f.TaskName, new List<Tuple<int, long>>());
                    tmp[f.TaskName].Add(new Tuple<int, long>(f.Indentify, f.DateCreate));
                }
            }
            var line = new ObservableCollection<LineInfo>();
            var j = 1;
            foreach (var f in tmp)
            {
                var tnoss = (from t in f.Value orderby t.Item2 ascending select t).ToList();
                bool isallcloase = true;
                foreach (var t in tnoss)
                {
                    if (t.Item1 == 1)
                    {
                        isallcloase = false;
                        break;
                    }
                }
                if(isallcloase) continue;
                long lastOctime = 0;
                bool isClose = false;              
                foreach (var g in tnoss)
                {
                    if (g.Item1 == 1)
                    {
                        if (lastOctime == 0)
                        {
                            lastOctime = g.Item2;
                            continue;
                        }
                    }
                    if (g.Item1 == 11)
                    {
                        if (lastOctime > 0)
                        {
                            var thisclosetime = g.Item2;

                            var d1 = new DateTime(lastOctime);
                            var d12 = new DateTime(d1.Year, d1.Month, d1.Day, 12, 0, 1);
                            if (d12.Ticks > d1.Ticks) d12 = d12.AddDays(-1);

                            var diff1 = (d1.Ticks - d12.Ticks) / 10000000 / 60;
                            var diff2 = (thisclosetime - d12.Ticks) / 10000000 / 60;

                            var x1 = (int)diff1 / 3;
                            var x2 = (int)diff2 / 3;

                            var time = ((diff2 - diff1) / 60).ToString().PadLeft(2, '0') + "小时" + ((diff2 - diff1) % 60).ToString().PadLeft(2, '0') + "分钟";

                            line.Add(new LineInfo()
                            {
                                X1 = 102 + x1,
                                X2 = 98 + x2,
                                Y1 = 60 + j * 30,
                                Y2 = 60 + j * 30,
                                M1 = 80 + x1,
                                M2 = 80 + x2,
                                N1 = 40 + j * 30,
                                N2 = 40 + j * 30,
                                A1 = 50 + j * 30,
                                B1 = 57 + j * 30,
                                A2 = 98 + x1,
                                B2 = 96 + x2,
                                Open = d1.ToString("HH:mm"),
                                Close = new DateTime(thisclosetime).ToString("HH:mm"),
                                Color = new SolidColorBrush(Colors.DarkOrange),
                                SumTime = time,
                                Tooltips1 = d1.ToString("HH:mm"),
                                Tooltips2 = new DateTime(thisclosetime).ToString("HH:mm"),
                                TableName = f.Key,
                                Index = (int)(diff2 - diff1)
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
                    if (d12.Ticks > d1.Ticks) d12 = d12.AddDays(-1);

                    var diff1 = (d1.Ticks - d12.Ticks) / 10000000 / 60;
                    var diff2 = (thisclosetime - d12.Ticks) / 10000000 / 60;

                    var x1 = (int)diff1 / 3;
                    var x2 = (int)diff2 / 3;

                    var time = ((diff2 - diff1) / 60).ToString().PadLeft(2, '0') + "小时" + ((diff2 - diff1) % 60).ToString().PadLeft(2, '0') + "分钟";

                    line.Add(new LineInfo()
                    {
                        X1 = 100 + x1,
                        X2 = 100 + x2,
                        Y1 = 60 + j * 30,
                        Y2 = 60 + j * 30,
                        M1 = 80 + x1,
                        M2 = 80 + x2,
                        N1 = 40 + j * 30,
                        N2 = 40 + j * 30,
                        A1 = 50 + j * 30,
                        B1 = 57 + j * 30,
                        A2 = 96 + x1,
                        B2 = 98 + x2,
                        Open = d1.ToString("HH:mm"),
                        Close = "12:00",
                        Color = new SolidColorBrush(Colors.DarkOrange),
                        SumTime = time,
                        Tooltips1 = d1.ToString("HH:mm"),
                        Tooltips2 = new DateTime(thisclosetime).ToString("HH:mm"),
                        TableName = f.Key,
                        Index = (int)(diff2-diff1)
                    });
                }
                j++;
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
                            x.SumTime = (m / 60).ToString().PadLeft(2, '0') + "小时" + (m % 60).ToString().PadLeft(2, '0') + "分钟";
                    }
                }
            }
            LineItemss = line;
            if(line.Count>0)
            {
                VisiLine=Visibility.Visible;
                LineLength = 80 + line.Count*30;
                LineLength1 = 80 + line.Count*30;
                LineLength2 = 100 + line.Count * 30;
            }

        }


        private void OnRequestFaultHis(string session, Wlst.mobile.MsgWithMobile infos)
        {
            HistoryFaultItem.Clear();
            var info = infos.WstStatisSysOneday2;
            if(info==null) return;
            var hitem = new ObservableCollection<HistoryFault>();
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
                hitem.Add(new HistoryFault()
                {
                    DateCreate = BeginDate.AddDays(-i),
                    Count = count
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

                    hitem.Add(new HistoryFault()
                    {
                        DateCreate = BeginDate.AddDays(-i),
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

                foreach (var f in info.ItemsFault)
                {
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                    {
                        if ((f.FaultId >= 1 && f.FaultId <= 5) || f.FaultId == 19 ||
                            (f.FaultId >= 22 && f.FaultId <= 24) ||
                            f.FaultId == 27 || f.FaultId == 29)
                            count1 += f.Count;
                        if ((f.FaultId >= 6 && f.FaultId <= 17) || f.FaultId == 20 || f.FaultId == 21 || f.FaultId == 25 ||
                            f.FaultId == 26 || f.FaultId == 28)
                            count2 += f.Count;
                        if (f.FaultId >= 50 && f.FaultId <= 65)
                            count3 += f.Count;
                        if (f.FaultId == 18 || (f.FaultId >= 30 && f.FaultId < 50) || f.FaultId > 65)
                            count4 += f.Count;
                    }
                }

                hitem.Add(new HistoryFault()
                              {
                                  DateCreate = BeginDate.AddDays(-i),
                                  Count = count1,
                                  FaultId = 1000
                              });
                hitem.Add(new HistoryFault()
                              {
                                  DateCreate = BeginDate.AddDays(-i),
                                  Count = count2,
                                  FaultId = 1001
                              });
                hitem.Add(new HistoryFault()
                              {
                                  DateCreate = BeginDate.AddDays(-i),
                                  Count = count3,
                                  FaultId = 1002
                              });
                hitem.Add(new HistoryFault()
                              {
                                  DateCreate = BeginDate.AddDays(-i),
                                  Count = count4,
                                  FaultId = 1003
                              });
            }

            var xx = hitem.Select(t => t.Count).Concat(new[] { 1 }).Max();
            Step3 = xx/5 > 1 ? xx/5 : 1;
            HistoryFaultItem = hitem;
            _radchart2.Series.Clear();
            //AddLines(HistoryFaultItem, 0,Colors.Red);
            foreach (var f in FaultCategories)
            {
                if (f.IsSelected)
                {
                    if (f.Value == 1)
                        AddLines(HistoryFaultItem, 1000, Colors.Gray);
                    if (f.Value == 2)
                        AddLines(HistoryFaultItem, 1001, Colors.Green);
                    if (f.Value == 3)
                        AddLines(HistoryFaultItem, 1002, Colors.Orange);
                    if (f.Value == 4)
                        AddLines(HistoryFaultItem, 1003, Colors.Purple);
                    if (f.Value == 5)
                        AddLines(HistoryFaultItem, 0, Colors.Red);
                }
            }
            if(_radchart2.Series.Count<1)
            {
                var itemss = new ObservableCollection<HistoryFault>();
                for (int i = 0; i < 7; i++)
                {
                    itemss.Add(new HistoryFault()
                    {
                        DateCreate = BeginDate.AddDays(-i),
                        Count = 0
                    });
                }
                AddLines(itemss, 0, Colors.Red);
            }
        }

        public void AddLines(ObservableCollection<HistoryFault> oit, int id,Color color)
        {
            var line = new LineSeries();
            line.Stroke = new SolidColorBrush(color);
            line.StrokeThickness = 2;         
            foreach (var f in oit)
            {
                if (f.FaultId == id)
                    line.DataPoints.Add(new CategoricalDataPoint() {Value = f.Count, Category = f.DateCreate});
            }
            _radchart2.Series.Add(line);
        }

        private void OnRequestEnergy(string session, Wlst.mobile.MsgWithMobile infos)
        {
            EnergyItems.Clear();
            CurrentItems.Clear();
            var info = infos.WstStatisSysOneday3;
            if(info==null) return;
            var eitem = new ObservableCollection<Energy>();
            for (int i = 0; i < 7; i++)
            {
                double rtuelec = 0;
                double sluelec = 0;
                foreach (var f in info.ItemsElecRtu)
                {
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                        rtuelec = f.ElecVaule;
                }
                foreach (var f in info.ItemsElecSlu)
                {
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                        sluelec = f.ElecVaule;
                }
                eitem.Add(new Energy()
                {
                    DateCreate = BeginDate.AddDays(-i),
                    RtuEnergy = Math.Round(rtuelec,2),
                    SluEnergy = Math.Round(sluelec,2)
                });
            }

            var xx = eitem.Select(t => t.RtuEnergy).Concat(new[] { 1.0 }).Max();
            Step4 = (int) xx/5 > 1 ? (int) xx/5 : 1;

            var yy = eitem.Select(t => t.SluEnergy).Concat(new[] { 1.0 }).Max();
            Step6 = (int) yy/5 > 1 ? (int) yy/5 : 1;
            EnergyItems = eitem;

            var citem = new ObservableCollection<Energy>();



            var powerdic = new Dictionary<DateTime, double>();
            var time = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month, BeginDate.AddDays(-1).Day, 12, 0, 0);
            var areaid = new List<int>();
            foreach (var f in info.ItemsCurrentPower)
            {
                if (!areaid.Contains(f.AreaId)) areaid.Add(f.AreaId);
            }
            for (int tt = 0; tt < 24; tt++)
            {
                double tol = 0;
                
                foreach (var i in areaid)
                {
                    var total = new List<double>();
                    
                    foreach (var t in info.ItemsCurrentPower)
                    {

                        if (t.DateCreate > time.AddHours(tt).Ticks && t.DateCreate < time.AddHours(tt + 1).Ticks &&
                            t.AreaId == i)
                        {
                            total.Add(t.Power);
                            

                        }
                    }
                    if (total.Count < 1) break;
                    tol += total.Average();
                    
                }
                if (tol > 0)
                    powerdic.Add(time.AddHours(tt), tol);

            }
            foreach (var t in powerdic)
            {
                citem.Add(new Energy()
                {
                    DateCreate = t.Key,
                    RtuEnergy = Math.Round(t.Value, 2)
                });
            }



            //foreach (var t in info.ItemsCurrentPower)
            //{
            //    citem.Add(new Energy()
            //                  {
            //                      DateCreate = new DateTime(t.DateCreate),
            //                      RtuEnergy = Math.Round(t.Power,2)
            //                  });
            //}
            if (citem.Count > 0)
            {
                Items.Clear();

                var tt = citem.Select(t => t.RtuEnergy).Concat(new[] { 1.0 }).Max();
                Step5 = (int) tt/5 > 1 ? (int) tt/5 : 1;
                var itemss = citem.OrderBy(t => t.DateCreate);
                foreach (var i in itemss)
                {
                    CurrentItems.Add(i);
                }
                //CurrentItems = citem;
                var maxtime = citem[0].DateCreate;
                foreach (var t in citem)
                {
                    if (maxtime < t.DateCreate)
                        maxtime = t.DateCreate;
                }
                var mintime = citem[0].DateCreate;
                foreach (var t in citem)
                {
                    if (mintime > t.DateCreate)
                        mintime = t.DateCreate;
                }
               
                for (
                    var i = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month,
                                         BeginDate.AddDays(-1).Day, 12, 0, 0);
                    i < new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 14, 0, 0);
                    )
                {
                    if (i < mintime || i > maxtime)
                        Items.Add(new Energy()
                                      {
                                          DateCreate = i,
                                          RtuEnergy = 0
                                      });
                    i = i.AddHours(2);
                }
            }
            else
                Step5 = 1;

        }

        private void OnRequestOnline(string session, Wlst.mobile.MsgWithMobile infos)
        {
            OnlineRtuItems.Clear();
            OnlineSluItems.Clear();
            OnlineWeekSluItems.Clear();
            OnlineWeekRtuItems.Clear();
            var info = infos.WstStatisSysOneday4;
            if(info==null) return;
            var oritem = new ObservableCollection<Online>();
            var rtudic = new Dictionary<DateTime, double>();
            var time = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month, BeginDate.AddDays(-1).Day, 12, 0, 0);
            var areaid = new List<int>();
            foreach (var f in info.ItemsRtu)
            {
                if (!areaid.Contains(f.AreaId)) areaid.Add(f.AreaId);
            }
            for (int tt = 0; tt < 24; tt = tt + 2)
            {
                double tol = 0;
                double onl = 0;
                foreach (var i in areaid)
                {
                    var total = new List<int>();
                    var online = new List<int>();
                    foreach (var t in info.ItemsRtu)
                    {

                        if (t.DateCreate > time.AddHours(tt).Ticks && t.DateCreate < time.AddHours(tt + 2).Ticks &&
                            t.AreaId == i)
                        {
                            total.Add(t.Totals);
                            online.Add(t.Onlines);

                        }
                    }
                    if (total.Count < 1) break;
                    tol += total.Average();
                    onl += online.Average();
                }
                if (tol > 0 && onl <= tol)
                    rtudic.Add(time.AddHours(tt + 1), onl / tol);

            }
            foreach (var t in rtudic)
            {
                oritem.Add(new Online()
                {
                    DateCreate = t.Key,
                    OnlineRate = t.Value
                });
            }
            //foreach (var t in info.ItemsRtu)
            //{
            //    if (t.Onlines > t.Totals)
            //        t.Onlines = t.Totals;
            //    oritem.Add(new Online()
            //                   {
            //                       DateCreate = new DateTime(t.DateCreate),
            //                       OnlineRate = (double)t.Onlines / t.Totals
            //                   });
            //}
            if(oritem.Count>0)
            {
                Items1.Clear();
                var maxtime = oritem[0].DateCreate;
                foreach (var t in oritem)
                {
                    if (maxtime < t.DateCreate)
                        maxtime = t.DateCreate;
                }
                var mintime = oritem[0].DateCreate;
                foreach (var t in oritem)
                {
                    if (mintime > t.DateCreate)
                        mintime = t.DateCreate;
                }
                for (
                    var i = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month,
                                         BeginDate.AddDays(-1).Day, 12, 0, 0);
                    i < new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 14, 0, 0);
                    )
                {
                    if (i < mintime || i > maxtime)
                        Items1.Add(new Online()
                                       {
                                           DateCreate = i,
                                           OnlineRate = 0
                                       });
                    i = i.AddHours(2);
                }
                OnlineRtuItems = oritem;
            }


            var ositem = new ObservableCollection<Online>();
            var sludic = new Dictionary<DateTime, double>();
            var time1 = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month, BeginDate.AddDays(-1).Day, 12, 0, 0);
            var areaid1 = new List<int>();
            foreach (var f in info.ItemsSlu)
            {
                if (!areaid1.Contains(f.AreaId)) areaid1.Add(f.AreaId);
            }
            for (int tt = 0; tt < 24; tt = tt + 2)
            {
                double tol = 0;
                double onl = 0;
                foreach (var i in areaid1)
                {
                    var total = new List<int>();
                    var online = new List<int>();
                    foreach (var t in info.ItemsSlu)
                    {

                        if (t.DateCreate > time1.AddHours(tt).Ticks && t.DateCreate < time1.AddHours(tt + 2).Ticks &&
                            t.AreaId == i)
                        {
                            total.Add(t.Totals);
                            online.Add(t.Onlines);

                        }
                    }
                    if (total.Count < 1) break;
                    tol += total.Average();
                    onl += online.Average();
                }
                if (tol > 0 && onl <= tol)
                    sludic.Add(time1.AddHours(tt + 1), onl / tol);

            }
            foreach (var t in sludic)
            {
                ositem.Add(new Online()
                {
                    DateCreate = t.Key,
                    OnlineRate = t.Value
                });
            }
            
            //foreach (var f in info.ItemsSlu)
            //{
            //    if (f.Onlines > f.Totals)
            //        f.Onlines = f.Totals;
            //    ositem.Add(new Online()
            //                   {
            //                       DateCreate = new DateTime(f.DateCreate),
            //                       OnlineRate = (double)f.Onlines / f.Totals
            //                   });
            //}
            if(ositem.Count>0)
            {       
                Items2.Clear();
                var maxtime = ositem[0].DateCreate;
                foreach (var t in ositem)
                {
                    if (maxtime < t.DateCreate)
                        maxtime = t.DateCreate;
                }
                var mintime = ositem[0].DateCreate;
                foreach (var t in ositem)
                {
                    if (mintime > t.DateCreate)
                        mintime = t.DateCreate;
                }
                for (
                    var i = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month,
                                         BeginDate.AddDays(-1).Day, 12, 0, 0);
                    i < new DateTime(BeginDate.Year, BeginDate.Month, BeginDate.Day, 14, 0, 0);
                    )
                {
                    if (i < mintime || i > maxtime)
                        Items2.Add(new Online()
                        {
                            DateCreate = i,
                            OnlineRate = 0
                        });
                    i = i.AddHours(2);
                }
                OnlineSluItems = ositem;
            }

            var witems = new ObservableCollection<Online>();
            for (int i = 0; i < 7; i++)
            {
                double rate = 0;
                foreach (var f in info.ItemsRtuSevenDays)
                {
                    if(f.Totals==0) continue;
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                    {
                        rate = (f.Onlines/f.Totals) >= 1 ? 1 : (double) f.Onlines/f.Totals;
                    }
                }
                witems.Add(new Online()
                {
                    DateCreate = BeginDate.AddDays(-i),
                    OnlineRate = rate
                });
            }
            OnlineWeekRtuItems = witems;

            var wsitems = new ObservableCollection<Online>();
            for (int i = 0; i < 7; i++)
            {
                double rate = 0;
                foreach (var f in info.ItemsSluSevenDays)
                {
                    if (f.Totals == 0) continue;
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                    {
                        rate = (f.Onlines / f.Totals) >= 1 ? 1 : (double)f.Onlines / f.Totals;
                    }
                }
                wsitems.Add(new Online()
                {
                    DateCreate = BeginDate.AddDays(-i),
                    OnlineRate = rate
                });
            }
            OnlineWeekSluItems = wsitems;
        }
    }

    public partial class MainViewModel
    {
        private void InitEvent()
        {
            this.AddEventFilterInfo(7777, "OperatorTypeItemtttttt", true);
            this.AddEventFilterInfo(3333, "TabControlChanged", true);
            this.AddEventFilterInfo(8888, "FaultCategories", true);
        }

        public override void ExPublishedEvent(
            PublishEventArgs args)
        {


            try
            {
                if (args.EventId == 7777 && args.EventType == "OperatorTypeItemtttttt")
                {
                    _radchart2.Series.Clear();
                    //AddLines(HistoryFaultItem, 0,Colors.Red);
                    foreach (var f in FaultCategories)
                    {
                        if (f.IsSelected)
                        {
                            if (f.Value == 1)
                                AddLines(HistoryFaultItem, 1000, Colors.Gray);
                            if (f.Value == 2)
                                AddLines(HistoryFaultItem, 1001, Colors.Green);
                            if (f.Value == 3)
                                AddLines(HistoryFaultItem, 1002, Colors.Orange);
                            if (f.Value == 4)
                                AddLines(HistoryFaultItem, 1003, Colors.Purple);
                            if (f.Value == 5)
                                AddLines(HistoryFaultItem, 0, Colors.Red);
                        }
                    }
                    VisiOne=Visibility.Collapsed;
                    foreach (var f in TypeItems)
                    {
                        if (Convert.ToInt16(args.GetParams()[0]) != f.Value)
                            f.IsSelected = false;
                        if (f.IsSelected == false) continue;
                        VisiOne=Visibility.Visible;
                        var line = new LineSeries();
                        line.Stroke = new SolidColorBrush(Colors.Blue);
                        line.StrokeThickness = 2;
                        foreach (var g in HistoryFaultItem)
                        {
                            if (f.Value == g.FaultId)
                                line.DataPoints.Add(new CategoricalDataPoint() { Value = g.Count, Category = g.DateCreate });
                        }
                        _radchart2.Series.Add(line);
                    }
                    if (_radchart2.Series.Count < 1)
                    {
                        var itemss = new ObservableCollection<HistoryFault>();
                        for (int i = 0; i < 7; i++)
                        {
                            itemss.Add(new HistoryFault()
                            {
                                DateCreate = BeginDate.AddDays(-i),
                                Count = 0
                            });
                        }
                        AddLines(itemss, 0, Colors.Red);
                    }
                }

                if (args.EventId == 8888 && args.EventType == "FaultCategories")
                {
                    _radchart2.Series.Clear();
                    //AddLines(HistoryFaultItem, 0, Colors.Red);
                    foreach (var f in FaultCategories)
                    {
                        if (f.IsSelected)
                        {
                            if (f.Value == 1)
                                AddLines(HistoryFaultItem, 1000, Colors.Gray);
                            if (f.Value == 2)
                                AddLines(HistoryFaultItem, 1001, Colors.Green);
                            if (f.Value == 3)
                                AddLines(HistoryFaultItem, 1002, Colors.Orange);
                            if (f.Value == 4)
                                AddLines(HistoryFaultItem, 1003, Colors.Purple);
                            if (f.Value == 5)
                                AddLines(HistoryFaultItem, 0, Colors.Red);
                        }
                    }
                    VisiOne=Visibility.Collapsed;
                    foreach (var f in TypeItems)
                    {
                        if(f.IsSelected==false) continue;
                        VisiOne=Visibility.Visible;
                        var line = new LineSeries();
                        line.Stroke = new SolidColorBrush(Colors.Blue);
                        line.StrokeThickness = 2;
                        foreach (var g in HistoryFaultItem)
                        {
                            if (f.Value == g.FaultId)
                                line.DataPoints.Add(new CategoricalDataPoint() { Value = g.Count, Category = g.DateCreate });
                        }
                        _radchart2.Series.Add(line);
                    }
                    if (_radchart2.Series.Count < 1)
                    {
                        var itemss = new ObservableCollection<HistoryFault>();
                        for (int i = 0; i < 7; i++)
                        {
                            itemss.Add(new HistoryFault()
                            {
                                DateCreate = BeginDate.AddDays(-i),
                                Count = 0
                            });
                        }
                        AddLines(itemss, 0, Colors.Red);
                    }
                }

                if (args.EventId == 3333 && args.EventType == "TabControlChanged")
                {
                    if (args.GetParams().Count < 8) return;
                    PictureName.Clear();
                    var x1 = Convert.ToBoolean(args.GetParams()[0]);
                    var x2 = Convert.ToBoolean(args.GetParams()[2]);
                    var x3 = Convert.ToBoolean(args.GetParams()[4]);
                    var x4 = Convert.ToBoolean(args.GetParams()[6]);
                    if (x1)
                    {
                        PictureName.Add("全部");
                        PictureName.Add("全年日出日落信息");
                        PictureName.Add("时间表操作信息");
                        PictureName.Add("单灯亮灯率曲线图");
                    }
                    if (x2)
                    {
                        PictureName.Add("全部");
                        PictureName.Add("现存故障分布图");
                        PictureName.Add("今日报警故障分布图");
                        PictureName.Add("历史故障曲线图");
                    }
                    if (x3)
                    {
                        PictureName.Add("全部");
                        PictureName.Add("系统能耗统计图");
                        PictureName.Add("系统功率曲线图");
                        PictureName.Add("单灯能耗统计图");
                    }
                    if (x4)
                    {
                        PictureName.Add("全部");
                        PictureName.Add("终端24小时在线率曲线图");
                        PictureName.Add("终端1周在线率曲线图");
                        PictureName.Add("集中器24小时在线率曲线图");
                        PictureName.Add("集中器1周在线率曲线图");
                    }
                    PictureComboBoxSelected = PictureName[0];
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void InitAction()
        {
            ProtocolServer.RegistProtocol(
                Sr.ProtocolPhone.LxFault.wlst_fault_pre,
                OnRequestFaultPre,
                typeof(MainViewModel), this,true);

            ProtocolServer.RegistProtocol(
               Sr.ProtocolPhone.LxStatic.wst_static_oneday_sys1,
               OnRequestOpenOrClose,
               typeof(MainViewModel), this,true);

            ProtocolServer.RegistProtocol(
               Sr.ProtocolPhone.LxStatic.wst_static_oneday_sys2,
               OnRequestFaultHis,
               typeof(MainViewModel), this,true);

            ProtocolServer.RegistProtocol(
               Sr.ProtocolPhone.LxStatic.wst_static_oneday_sys3,
               OnRequestEnergy,
               typeof(MainViewModel), this,true);

            ProtocolServer.RegistProtocol(
               Sr.ProtocolPhone.LxStatic.wst_static_oneday_sys4,
               OnRequestOnline,
               typeof(MainViewModel), this, true);
        }
    }

    public class SunItem : Wlst.Cr.Core.CoreServices.ObservableObject
    {
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

        private int _value2;

        public int Value2
        {
            get { return _value2; }
            set
            {
                if (_value2 != value)
                {
                    _value2 = value;
                    this.RaisePropertyChanged(() => this.Value2);
                }
            }
        }

        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set
            {
                if (_time != value)
                {
                    _time = value;
                    this.RaisePropertyChanged(() => this.Time);
                }
            }
        }
    }

    public class Fault:Wlst.Cr.Core.CoreServices.ObservableObject
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

        private int _count;

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
                    if (value)
                    {
                        var args = new PublishEventArgs()
                                       {
                                           EventId = 7777,
                                           EventType = "OperatorTypeItemtttttt"
                                       };
                        args.AddParams(Value, this);
                        EventPublish.PublishEvent(args);
                    }
                    //EventPublish.PublishEvent(new PublishEventArgs()
                    //{
                    //    EventId = 7777,
                    //    EventType = "OperatorTypeItemtttttt"
                    //});
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

    public class HistoryFault : Wlst.Cr.Core.CoreServices.ObservableObject
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

        private int _count;

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

    public class SingleLampControl:Wlst.Cr.Core.CoreServices.ObservableObject
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

        #region ControlRate

        private double _controlRate;
        public double ControlRate
        {
            get { return _controlRate; }
            set
            {
                if (_controlRate == value) return;
                _controlRate = value;
                RaisePropertyChanged(() => ControlRate);
            }
        }

        #endregion

        #region LightRate

        private double _lightRate;
        public double LightRate
        {
            get { return _lightRate; }
            set
            {
                if (_lightRate == value) return;
                _lightRate = value;
                RaisePropertyChanged(() => LightRate);
            }
        }

        #endregion
    }

    public class Energy:Wlst.Cr.Core.CoreServices.ObservableObject
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

        #region RtuEnergy

        private double _rtuEnergy;
        public double RtuEnergy
        {
            get { return _rtuEnergy; }
            set
            {
                if (_rtuEnergy == value) return;
                _rtuEnergy = value;
                RaisePropertyChanged(() => RtuEnergy);
            }
        }

        #endregion

        #region SluEnergy

        private double _sluEnergy;
        public double SluEnergy
        {
            get { return _sluEnergy; }
            set
            {
                if (_sluEnergy == value) return;
                _sluEnergy = value;
                RaisePropertyChanged(() => SluEnergy);
            }
        }

        #endregion
    }

    public class Online:Wlst.Cr.Core.CoreServices.ObservableObject
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

        #region OnlineRate

        private double _onlineRate;
        public double OnlineRate
        {
            get { return _onlineRate; }
            set
            {
                if (_onlineRate == value) return;
                _onlineRate = value;
                RaisePropertyChanged(() => OnlineRate);
            }
        }

        #endregion
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

        private string _tableName;
        public string TableName
        {
            get { return _tableName; }
            set
            {
                if (_tableName != value)
                {
                    _tableName = value;
                    this.RaisePropertyChanged(() => this.TableName);
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

    public class NameIntBool : ObservableObject
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
                        EventId = 8888,
                        EventType = "FaultCategories"
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
