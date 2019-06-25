using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.EquipmentInfo.DailyStatistics.SingleLampViewModel.ViewModel
{
    public partial class SingleLampViewModel : VmEventActionProperyChangedBase
    {
        private RadCartesianChart _radchart2 = null;
        public SingleLampViewModel(RadCartesianChart radchart)
        {
            ImageIcon1 = Directory.GetCurrentDirectory() + "\\Image\\Png\\sunrise.png";
            ImageIcon2 = Directory.GetCurrentDirectory() + "\\Image\\Png\\sunset.png";
            BeginDate = DateTime.Now;
            InitEvent();
            InitAction();
            _radchart2 = radchart;
            Step1 = 1;
            Step2 = 1;
		    CommunicateItems.Clear();
            for (int i = 0; i < 7; i++)
            {
                CommunicateItems.Add(new ConcentratorStatistic()
                                         {
                                             DateCreate = BeginDate.AddDays(-i),
                                             Value = 0
                                         });
            }

            LightRateItems.Clear();
            for (int i = 0; i < 7; i++)
            {
                LightRateItems.Add(new ConcentratorStatistic()
                                       {
                                           DateCreate = BeginDate.AddDays(-i),
                                           Value = 0
                                       });  
            }

            FaultItems.Clear();
            for (int i = 0; i < 7; i++)
            {
                FaultItems.Add(new Fault()
                {
                    DtCreateTime = BeginDate.AddDays(-i),
                    Count = 0
                });
            }
            _radchart2.Series.Clear();
            AddLines(FaultItems, 0,Colors.Red);

            ElectricityItems.Clear();
            for (int i = 0; i < 7; i++)
            {
                ElectricityItems.Add(new ConcentratorStatistic()
                {
                    DateCreate = BeginDate.AddDays(-i),
                    Value = 0
                });
            }

            PictureName.Clear();
            PictureName.Add("全部");
            PictureName.Add("集中器时间表");
            PictureName.Add("单灯通信成功率(亮灯率)");
            PictureName.Add("故障统计");
            PictureName.Add("电量统计");
            PictureComboBoxSelected = PictureName[0];
            SluId = 1500001;
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
        #endregion


        #region SluName
        private string _sluName;
        /// <summary>
        /// 集中器名称
        /// </summary>
        public string SluName
        {
            get { return _sluName; }
            set
            {
                if (_sluName == value) return;
                _sluName = value;
                RaisePropertyChanged(() => SluName);
            }
        }

        #endregion

        #region SingleId

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
                this.RelatedRtuName = "";
                foreach (var t in Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    var x = t.Value as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                    if(x==null) continue;
                    if (x.RtuPhyId == _relatedRtuId)
                    {
                        this.RelatedRtuName = x.RtuName;
                        break;
                    }
                }
                IsVisible = _relatedRtuId == 0 ? Visibility.Hidden : Visibility.Visible;

                //if (
                //        !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.
                //             InfoItems.ContainsKey
                //             (_relatedRtuId))
                //    return;
                //var tml =
                //    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                //        [_relatedRtuId];
                //this.RelatedRtuName = tml.RtuName;
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

        public static string PassSluName;
        private int _sluId;
        /// <summary>
        /// 集中器逻辑地址
        /// </summary>
        public int SluId
        {
            get { return _sluId; }
            set
            {
                if (_sluId == value) return;
                _sluId = value;
                RaisePropertyChanged(() => SluId);

                var tmps = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                if (tmps != null)
                {
                    SluName = tmps.RtuName;
                    PhyId = tmps.RtuPhyId;
                    PassSluName = tmps.RtuPhyId + "-" + tmps.RtuName;
                }
                else
                    SluName = "";

                var sss = tmps as Wj2090Slu;
                if(sss==null)
                    return;
                int number = sss.WjSluCtrls.Count;
                ControllerNumber = number;
                RelatedRtuId = sss.WjSlu.RelatedRtuId;

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

        #endregion

        #region TypeItems
        private ObservableCollection<OperatorTypeItem> _typeItems;
        /// <summary>
        /// 
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

        #region CommunicateItems
        private ObservableCollection<ConcentratorStatistic> _communicateItems;
        /// <summary>
        /// 通信成功率
        /// </summary>
        public ObservableCollection<ConcentratorStatistic> CommunicateItems
        {
            get { return _communicateItems ?? (_communicateItems = new ObservableCollection<ConcentratorStatistic>()); }
            set
            {
                if (value == _communicateItems) return;
                _communicateItems = value;
                this.RaisePropertyChanged(() => CommunicateItems);
            }
        }
        #endregion

        #region LightRateItems
        private ObservableCollection<ConcentratorStatistic> _lightRateItems;
        /// <summary>
        /// 亮灯率
        /// </summary>
        public ObservableCollection<ConcentratorStatistic> LightRateItems
        {
            get { return _lightRateItems ?? (_lightRateItems = new ObservableCollection<ConcentratorStatistic>()); }
            set
            {
                if (value == _lightRateItems) return;
                _lightRateItems = value;
                this.RaisePropertyChanged(() => LightRateItems);
            }
        }
        #endregion

        #region ElectricityItems
        private ObservableCollection<ConcentratorStatistic> _electricityItems;
        /// <summary>
        /// 电量统计
        /// </summary>
        public ObservableCollection<ConcentratorStatistic> ElectricityItems
        {
            get { return _electricityItems ?? (_electricityItems = new ObservableCollection<ConcentratorStatistic>()); }
            set
            {
                if (value == _electricityItems) return;
                _electricityItems = value;
                this.RaisePropertyChanged(() => ElectricityItems);
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

        #region OperatorItems
        private ObservableCollection<Operator> _operatorItems;
        /// <summary>
        ///操作信息
        /// </summary>
        public ObservableCollection<Operator> OperatorItems
        {
            get { return _operatorItems ?? (_operatorItems = new ObservableCollection<Operator>()); }
            set
            {
                if (_operatorItems == value) return;
                _operatorItems = value;
                this.RaisePropertyChanged(() => this.OperatorItems);
            }
        }

        #endregion

        #region ShowItems
        private ObservableCollection<Operator> _showItems;
        /// <summary>
        ///
        /// </summary>
        public ObservableCollection<Operator> ShowItems
        {
            get { return _showItems ?? (_showItems = new ObservableCollection<Operator>()); }
            set
            {
                if (_showItems == value) return;
                _showItems = value;
                this.RaisePropertyChanged(() => this.ShowItems);
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

                    _faultCategories.Add(new NameIntBool() { Name = "集中器报警", Value = 1, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "关灯报警", Value = 2, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "开灯报警", Value = 3, IsSelected = true });
                    _faultCategories.Add(new NameIntBool() { Name = "故障总数", Value = 4, IsSelected = true });
                }
                return _faultCategories;
            }
        }
        #endregion
    }

    public partial class SingleLampViewModel
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
            RequestSingleLamp();
        }
        private bool CanExCmdQuery()
        {
            return DateTime.Now.Ticks - _dtCmdQuery.Ticks > 30000000 && SluId != 0;
        }
        #endregion
    }

    public partial class SingleLampViewModel
    {
        private void InitSun()
        {
            var info = Wlst.Sr.TimeTableSystem.Services.SunRiseSetInfoServices.GetSunRiseItemInfo(BeginDate.Month,
                                                                                                  BeginDate.Day);
            SunRise = (info.time_sunrise / 60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ":" +
                      (info.time_sunrise % 60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            SunSet = (info.time_sunset / 60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0') + ":" +
                     (info.time_sunset % 60).ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            XSunRise = 70 + (info.time_sunrise + 720) / 3;
            XSunSet = 70 + (info.time_sunset - 720) / 3;
            X = 70 + (info.time_sunrise + 720) / 3;
            X1 = 70 + (info.time_sunset - 720) / 3;
            Visi = Visibility.Visible;
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
                if (t.Key >= 50 && t.Key < 80)
                    item.Add(new OperatorTypeItem()
                                 {
                                     IsSelected = false,
                                     Name = t.Value.FaultNameByDefine,
                                     Value = t.Key,
                                     Color = color[i]
                                 });
                i++;
            }
            TypeItems = item;
        }

        private void RequestSingleLamp()
        {
            var info = Wlst.Sr.ProtocolPhone.LxStatic.wst_static_oneday_slu;
            info.WstStatisSluOneday.DtTime = BeginDate.Ticks;
            info.WstStatisSluOneday.SluId = SluId;
            SndOrderServer.OrderSnd(info, 10, 6);
        }
    }

    public partial class SingleLampViewModel
    {
        private void InitAction()
        {
            this.RegistProtocol(
               Sr.ProtocolPhone.LxStatic.wst_static_oneday_slu,
               OnRequestSingleLamp, true);
        }

        private void OnRequestSingleLamp(string session, Wlst.mobile.MsgWithMobile infos)
        {
            VisiLine = Visibility.Collapsed;
            CommunicateItems.Clear();
            var info = infos.WstStatisSluOneday;
            if(info==null) return;
            var citem = new ObservableCollection<ConcentratorStatistic>();
            var litem = new ObservableCollection<ConcentratorStatistic>();
            var eitem = new ObservableCollection<ConcentratorStatistic>();
            for (int i = 0; i < 7; i++)
            {
                double rate1 = 0;
                double rate2 = 0;
                double electric = 0;
                foreach (var t in info.ItemsLighton)
                {
                    var x = new DateTime(t.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                    {
                        rate1 = (double)t.SumCommNormal/t.SumTotalCtrl;
                        rate2 = (double)t.SumLightOnCtrl/t.SumTotalCtrl;
                    }
                }
                citem.Add(new ConcentratorStatistic()
                              {
                                  DateCreate = BeginDate.AddDays(-i),
                                  Value = rate1
                              });
                litem.Add(new ConcentratorStatistic()
                              {
                                  DateCreate = BeginDate.AddDays(-i),
                                  Value = rate2
                              });
                foreach (var f in info.ItemsElec)
                {
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                        electric = f.ElecVaule;                   
                }
                eitem.Add(new ConcentratorStatistic()
                {
                    DateCreate = BeginDate.AddDays(-i),
                    Value = Math.Round(electric,2)
                });
            }
            CommunicateItems = citem;
            LightRateItems = litem;
            //foreach (var t in eitem)
            //{
            //    if (t.Value > 1)
            //    {
            //        Step1 = 1;
            //        break;
            //    }
            //    Step1 = 5;
            //}
            var tt = eitem.Select(t => t.Value).Concat(new[] { 1.0 }).Max();
            Step2 = (int) tt/5 > 1 ? (int) tt/5 : 1;
            ElectricityItems = eitem;

            var fitem = new ObservableCollection<Fault>();
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
            //单灯故障
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

                foreach (var f in info.ItemsFault)
                {
                    var x = new DateTime(f.DateCreate);
                    if (BeginDate.AddDays(-i).Year == x.Year && BeginDate.AddDays(-i).Month == x.Month &&
                        BeginDate.AddDays(-i).Day == x.Day)
                    {
                        if (f.FaultId == 50)
                            count1 += f.Count;
                        if (f.FaultId == 61)
                            count2 += f.Count;
                        if (f.FaultId != 50 && f.FaultId != 61)
                            count3 += f.Count;

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
            }
            //foreach (var t in fitem)
            //{
            //    if (t.Count > 1)
            //    {
            //        Step2 = 1;
            //        break;
            //    }
            //    Step2 = 5;
            //}
            var ff = fitem.Select(t => t.Count).Concat(new[] { 1 }).Max();
            Step1 = ff/5 > 1 ? ff/5 : 1;
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

            var oitem = new ObservableCollection<Operator>();
            int j = 0;
            //foreach (var f in info.ItemsPlan )
            //{
            //    j++;
            //    var name = "";
            //    foreach (var t in f.Target)
            //    {
            //        var xxx = t;
            //        if (t.Contains("规则"))
            //        {
            //            var xx = Convert.ToInt32(t.Replace("规则:", ""))/10;
            //            xxx = xx == 1 ? "全部" : xx == 2 ? "隔一亮一" : xx == 3 ? "隔二亮一" : xx == 4 ? "隔三亮一" : "未知";
            //        }
            //        name += " " + xxx;
            //    }
            //    //var name = f.Target.Aggregate("", (current, t) => current + (" " + t));
            //    var t1 = new DateTime(f.DateCreate);
            //    var t2 = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month, BeginDate.AddDays(-1).Day, 12, 0, 0);
            //    var time = (int)(t1-t2).TotalMinutes/3;
            //    oitem.Add(new Operator()
            //                  {
            //                      //DtCreateTime = new DateTime(f.DateCreate).ToString("HH:mm"),
            //                      OperatorName = j + "",
            //                      // f.IsOpen == 0 ? "关灯" : f.IsOpen == 1 ? "开灯" : f.IsOpen == 2 ? "调光" : "未知操作",
            //                      //OperatorObject = name,
            //                      Location = 40 + time,
            //                      Tooltip =
            //                          j + "、 " + new DateTime(f.DateCreate).ToString("HH:mm") + " " + name + " " +
            //                          (f.IsOpen == 0 ? "关灯" : f.IsOpen == 1 ? "开灯" : f.IsOpen == 2 ? "调光" : "未知操作")+"   "
            //                  });
            //}

            var namelist = new List<string>();
            foreach (var f in info.ItemsPlan)
            {
                int line=0;
                j++;
                var name = "";
                foreach (var t in f.Target)
                {
                    var xxx = t;
                    if (t.Contains("规则"))
                    {
                        var xx = Convert.ToInt32(t.Replace("规则:", "")) / 10;
                        xxx = xx == 1 ? "全部" : xx == 2 ? "隔一亮一" : xx == 3 ? "隔二亮一" : xx == 4 ? "隔三亮一" : "未知";
                    }
                    name += " " + xxx;
                }
                if (!namelist.Contains(name))
                {
                    namelist.Add(name);
                    line = j;
                }
                else
                {
                    j--;
                    foreach (var x in oitem)
                    {
                        if (x.OperatorName == name)
                            line = (x.Y1 - 35)/40;
                    }
                }
                //var name = f.Target.Aggregate("", (current, t) => current + (" " + t));
                var t1 = new DateTime(f.DateCreate);
                var t2 = new DateTime(BeginDate.AddDays(-1).Year, BeginDate.AddDays(-1).Month, BeginDate.AddDays(-1).Day, 12, 0, 0);
                var time = (int)(t1 - t2).TotalMinutes / 3;
                oitem.Add(new Operator()
                              {
                                  DtCreateTime = new DateTime(f.DateCreate).ToString("HH:mm"),
                                  OperatorName = name,
                                  Y1 = 35 + 40*line,
                                  Y2 = 45 + 40*line,
                                  Y3 = 15 + 40*line,
                                  Y4 = 25 + 40*line,
                                  Y5 = 33 + 40*line,
                                  OperatorObject =
                                      f.IsOpen == 0 ? "关灯" : f.IsOpen == 1 ? "开灯" : f.IsOpen == 2 ? "调光" : "未知操作",
                                  Location = 78 + time,
                                  Tooltip =
                                      new DateTime(f.DateCreate).ToString("HH:mm") + "  " +
                                      (f.IsOpen == 0 ? "关灯" : f.IsOpen == 1 ? "开灯" : f.IsOpen == 2 ? "调光" : "未知操作"),
                                  Brush =
                                      f.IsOpen == 0
                                          ? new SolidColorBrush(Colors.Gray)
                                          : f.IsOpen == 1
                                                ? new SolidColorBrush(Colors.Orange)
                                                : f.IsOpen == 2
                                                      ? new SolidColorBrush(Colors.Green)
                                                      : new SolidColorBrush(Colors.Blue)
                              });
            }
            OperatorItems = oitem;
            if (oitem.Count > 0)
            {

                VisiLine = Visibility.Visible;
                LineLength = 15 + oitem.Select(t => t.Y1).Concat(new[] {1}).Max();
                LineLength1 = 15 + oitem.Select(t => t.Y1).Concat(new[] {1}).Max();
            }
        }

        public void AddLines(ObservableCollection<Fault> oit, int id,Color color)
        {
            var line = new LineSeries();
            line.Stroke = new SolidColorBrush(color);
            line.StrokeThickness = 2;
            foreach (var f in oit)
            {
                if (f.FaultId == id)
                    line.DataPoints.Add(new CategoricalDataPoint() { Value = f.Count, Category = f.DtCreateTime });
            }
            _radchart2.Series.Add(line);
        }

        private void InitEvent()
        {
            this.AddEventFilterInfo(Sr.EquipmentInfoHolding.Services.EventIdAssign.EquipmentSelected,
                                    PublishEventType.Core,true);
            this.AddEventFilterInfo(8888, "OperatorTypeItemxxxxxx", true);
            this.AddEventFilterInfo(2222, "SingleFaultCategories", true);
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
                        if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(id) == false)
                            return;
                        var tml =Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[id];
                        foreach (var t in tml.EquipmentsThatAttachToThisRtu)
                        {
                            if (t > 1500000 && t < 1600000)
                            {
                                mid = t;
                                break;
                            }
                        }
                        //var tmps = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(id);
                        //if (tmps == null) return;
                        //id = tmps.RtuFid;
                    }
                    if (mid < 1500000 || mid > 1600000) return;

                    SelectRtuIdChange(mid);
                }

                if (args.EventId == 8888 && args.EventType == "OperatorTypeItemxxxxxx")
                {
                    _radchart2.Series.Clear();
                    AddLines(FaultItems, 0,Colors.Red);
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

                if (args.EventId == 2222 && args.EventType == "SingleFaultCategories")
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
            if (rtuId != this.SluId)
            {
                this.SluId = rtuId;

                Step1 = 1;
                Step2 = 1;
                OperatorItems.Clear();
                CommunicateItems.Clear();
                LightRateItems.Clear();
                FaultItems.Clear();
                ElectricityItems.Clear();
                _radchart2.Series.Clear();
                for (int i = 0; i < 7; i++)
                {
                    CommunicateItems.Add(new ConcentratorStatistic()
                                             {
                                                 DateCreate = BeginDate.AddDays(-i),
                                                 Value = 0
                                             });
                }

                for (int i = 0; i < 7; i++)
                {
                    LightRateItems.Add(new ConcentratorStatistic()
                                           {
                                               DateCreate = BeginDate.AddDays(-i),
                                               Value = 0
                                           });
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
                    ElectricityItems.Add(new ConcentratorStatistic()
                                             {
                                                 DateCreate = BeginDate.AddDays(-i),
                                                 Value = 0
                                             });
                }
            }
        }
    }

    /// <summary>
    /// 操作类型模型定义
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
                        EventId = 8888,
                        EventType = "OperatorTypeItemxxxxxx"
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

    public class ConcentratorStatistic: Wlst.Cr.Core.CoreServices.ObservableObject
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

        private double _value;
        public double Value
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
    
    public class Operator:Wlst.Cr.Core.CoreServices.ObservableObject
    {
        private string _operatorName;
        /// <summary>
        /// 操作名称
        /// </summary>
        public string OperatorName
        {
            get { return _operatorName; }
            set
            {
                if (value == _operatorName) return;
                _operatorName = value;
                RaisePropertyChanged(() => OperatorName);
            }
        }

        private string _operatorObject;
        /// <summary>
        /// 操作对象
        /// </summary>
        public string OperatorObject
        {
            get { return _operatorObject; }
            set
            {
                if (value == _operatorObject) return;
                _operatorObject = value;
                RaisePropertyChanged(() => OperatorObject);
            }
        }

        private string _dtCreateTime;
        /// <summary>
        /// 操作时间
        /// </summary>
        public string DtCreateTime
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

        private int _location;
        /// <summary>
        /// 位置
        /// </summary>
        public int Location
        {
            get { return _location; }
            set
            {
                if (value == _location) return;
                _location = value;
                RaisePropertyChanged(() => Location);
            }
        }

        private string _tooltip;
        /// <summary>
        /// 注释
        /// </summary>
        public string Tooltip
        {
            get { return _tooltip; }
            set
            {
                if (value == _tooltip) return;
                _tooltip = value;
                RaisePropertyChanged(() => Tooltip);
            }
        }

        private int _y1 ;
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

        private int _y2;
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

        private int _y3;
        public int Y3
        {
            get { return _y3; }
            set
            {
                if (_y3 != value)
                {
                    _y3 = value;
                    this.RaisePropertyChanged(() => this.Y3);
                }
            }
        }

        private int _y4;
        public int Y4
        {
            get { return _y4; }
            set
            {
                if (_y4 != value)
                {
                    _y4 = value;
                    this.RaisePropertyChanged(() => this.Y4);
                }
            }
        }

        private int _y5;
        public int Y5
        {
            get { return _y5; }
            set
            {
                if (_y5 != value)
                {
                    _y5 = value;
                    this.RaisePropertyChanged(() => this.Y5);
                }
            }
        }

        private Brush _brush;
        public Brush Brush
        {
            get { return _brush; }
            set
            {
                if (_brush == value) return;
                _brush = value;
                RaisePropertyChanged(() => Brush);
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
                        EventId = 2222,
                        EventType = "SingleFaultCategories"
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
