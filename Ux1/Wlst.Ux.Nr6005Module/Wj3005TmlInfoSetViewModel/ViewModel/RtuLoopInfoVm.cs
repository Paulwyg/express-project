using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;


using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Coreb.EventHelper;
using Wlst.Cr.Coreb.Servers;
using Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.Services;
using Wlst.client;

namespace Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.ViewModel
{



    /// <summary>
    /// 回路数据
    /// </summary>
    public sealed class RtuLoopInfoVm:ObservableObject 
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="rtuParaAnalogueAmp"></param>
        /// <param name="isOneLoop"></param>
        public RtuLoopInfoVm( Wlst .client.RtuAnalogParameter   rtuParaAnalogueAmp,bool isOneLoop)
        {
           
            this.RtuId = rtuParaAnalogueAmp.RtuId;
            this.LoopId = rtuParaAnalogueAmp.LoopId;
            this.LoopName = rtuParaAnalogueAmp.LoopName;
            this.Range = rtuParaAnalogueAmp.MutualInductorRatio  ;

            this.UpperLimit = rtuParaAnalogueAmp.CurrentAlarmUpperlimit ;
            this.LowerLimit = rtuParaAnalogueAmp.CurrentAlarmLowerlimit ;
            this.VectorMoniliang  = rtuParaAnalogueAmp.VectorMoniliang;
            this.Phase = (int )rtuParaAnalogueAmp.VoltagePhaseCode ;
            this.LightRate = rtuParaAnalogueAmp.BrightRate ;
            this.LightRateLowerLimit = rtuParaAnalogueAmp.BrightRateLowerlimit ;
            this.VectorSwitchIn = rtuParaAnalogueAmp.VectorSwitchIn;
            this.SwitchOutId = rtuParaAnalogueAmp.SwitchOutputId ;
            
            this.AmRange = rtuParaAnalogueAmp.CurrentRange ;


            this.IsAlarmSwitch = rtuParaAnalogueAmp.IsAlarmHop ;
            //if (IsAlarmSwitch == false) IsAlarmSwitchSelectItem = IsAlarmSwitchItem[0];
            //else IsAlarmSwitchSelectItem = IsAlarmSwitchItem[1];

            this.IsSwitchStateClose = rtuParaAnalogueAmp.IsSwitchStateClose;
            //if (this.IsSwitchStateClose == false) IsSwitchStateCloseSelectItem = IsSwitchStateCloseItem[0];
            //else IsSwitchStateCloseSelectItem = IsSwitchStateCloseItem[1];

            this.IsShieldLoop = rtuParaAnalogueAmp.IsShieldLoop;
            //if (this.IsShieldLoop == false) IsShieldLoopSelectItem = IsShieldLoopItem[0];
            //else IsShieldLoopSelectItem = IsShieldLoopItem[1];

            this.IsShieldLittleA = rtuParaAnalogueAmp.ShieldLittleA.ToString();
            //if (rtuParaAnalogueAmp.IsShieldLittleA) this.IsShieldLittleA = rtuParaAnalogueAmp.ShieldLittleA.ToString();
            //else this.IsShieldLittleA = "0";
            
            IsThisIsOneRtuLoop = isOneLoop;
            
        }

        //public void SetRtuLoopInfoVm(RtuParaAnalogueAmps.RtuParaAnalogueAmp rtuParaAnalogueAmp, bool isOneLoop)
        //{

        //    this.RtuId = rtuParaAnalogueAmp.RtuId;
        //    this.LoopId = rtuParaAnalogueAmp.LoopId;
        //    this.LoopName = rtuParaAnalogueAmp.LoopName;
        //    this.Range = rtuParaAnalogueAmp.Range;

        //    this.UpperLimit = rtuParaAnalogueAmp.UpperLimit;
        //    this.LowerLimit = rtuParaAnalogueAmp.LowerLimit;
        //    this.VectorMoniliang = rtuParaAnalogueAmp.VectorMoniliang;
        //    this.Phase = rtuParaAnalogueAmp.Phase;
        //    this.LightRate = rtuParaAnalogueAmp.LightRate;
        //    this.LightRateLowerLimit = rtuParaAnalogueAmp.LightRateLowerLimit;
        //    this.VectorSwitchIn = rtuParaAnalogueAmp.VectorSwitchIn;
        //    this.SwitchOutId = rtuParaAnalogueAmp.SwitchOutId;
        //    this.AmRange = rtuParaAnalogueAmp.AmRange;
        //    this.IsAlarmSwitch = rtuParaAnalogueAmp.IsAlarmSwitch;
        //    this.IsSwitchStateClose = rtuParaAnalogueAmp.IsSwitchStateClose;
        //    IsThisIsOneRtuLoop = isOneLoop;

        //}
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="rtuParaAnalogueAmp"></param>
        public RtuLoopInfoVm(RtuLoopInfoVm rtuParaAnalogueAmp)
        {

            this.RtuId = rtuParaAnalogueAmp.RtuId;
            this.LoopId = rtuParaAnalogueAmp.LoopId;
            this.LoopName = "新回路" + rtuParaAnalogueAmp.LoopId;
            this.Range = rtuParaAnalogueAmp.Range;

            this.UpperLimit = rtuParaAnalogueAmp.UpperLimit;
            this.LowerLimit = rtuParaAnalogueAmp.LowerLimit;
            this.VectorMoniliang = 0;
            this.Phase = rtuParaAnalogueAmp.Phase;
            this.LightRate = rtuParaAnalogueAmp.LightRate;
            this.LightRateLowerLimit = rtuParaAnalogueAmp.LightRateLowerLimit;
            this.VectorSwitchIn = rtuParaAnalogueAmp.VectorSwitchIn;
            this.SwitchOutId = rtuParaAnalogueAmp.SwitchOutId;
            this.AmRange = rtuParaAnalogueAmp.AmRange;
            this.IsAlarmSwitch = rtuParaAnalogueAmp.IsAlarmSwitch;
            //if (IsAlarmSwitch == false) IsAlarmSwitchSelectItem = IsAlarmSwitchItem[0];
            //else IsAlarmSwitchSelectItem = IsAlarmSwitchItem[1];

            this.IsSwitchStateClose = rtuParaAnalogueAmp.IsSwitchStateClose;
            //if (this.IsSwitchStateClose == false) IsSwitchStateCloseSelectItem = IsSwitchStateCloseItem[0];
            //else IsSwitchStateCloseSelectItem = IsSwitchStateCloseItem[1];
            
            IsThisIsOneRtuLoop = rtuParaAnalogueAmp.IsThisIsOneRtuLoop;
            
            this.IsShieldLoop = rtuParaAnalogueAmp.IsShieldLoop;

            this.IsShieldLittleA = rtuParaAnalogueAmp.IsShieldLittleA;
        }

        //新建
        public RtuLoopInfoVm(int rtuId,int switchOutId, bool isOneLoop)
        {

            this.RtuId =rtuId ;
            this.LoopId = 0;
            this.LoopName =isOneLoop?"新回路":"新输入";
            this.Range = isOneLoop?100:0;

            this.UpperLimit = isOneLoop?100:0;
            this.LowerLimit = 0;
            this.VectorMoniliang = 1;
            this.Phase = 0;
            this.LightRate =0;
            this.LightRateLowerLimit =0;
            this.VectorSwitchIn = isOneLoop ? switchOutId : 1;
            this.SwitchOutId = switchOutId;
            this.AmRange = isOneLoop?100:0;
            this.IsAlarmSwitch =switchOutId==0?true : false ;
            //if (IsAlarmSwitch == false) IsAlarmSwitchSelectItem = IsAlarmSwitchItem[0];
            //else IsAlarmSwitchSelectItem = IsAlarmSwitchItem[1];

            this.IsSwitchStateClose = false  ;
            //IsSwitchStateCloseSelectItem = IsSwitchStateCloseItem[0];

            IsThisIsOneRtuLoop = isOneLoop;

            this.IsShieldLoop = 0;

            this.IsShieldLittleA = "不屏蔽";

        }

        public static string[] _constColor = GetColor();

        private static bool isLoad = false;
        private static bool mesf;
        public static bool OnMeasurShowDada
        {
            get
            {
                if (isLoad) return mesf;
                else
                {
                    GetColor();
                    isLoad = true;
                    return mesf;
                }
            }
        }
        private static string[] GetColor()
        {

            var info = NewDataSettingViewModel.LoadNewDataLenghtSetConfg();
            string[] myColor = new string[9]
                                   {
                                       info.Item7.Background,
                                       info.Item7.K1Background,
                                       info.Item7.K2Background,
                                       info.Item7.K3Background,
                                       info.Item7.K4Background,
                                       info.Item7.K5Background,
                                       info.Item7.K6Background,
                                       info.Item7.K7Background,
                                       info.Item7.K8Background
                                   };
            mesf = info.Item7.OnMeasureShowData;
            return myColor;
        }

        /// <summary>
        /// 还原数据为 RtuParaAnalogueAmp
        /// </summary>
        /// <returns></returns>
        public Wlst .client .RtuAnalogParameter  BackRtuParaAnalogueAmp()
        {

            //var ShieldLittleA = false;
            double isshieldlittlea;

            if (IsShieldLittleA == "不屏蔽")
                isshieldlittlea = 0;
            else 
            {
                isshieldlittlea = double.Parse(IsShieldLittleA);
            }
            
           // if (isshieldlittlea >= 0.001) ShieldLittleA = true;

            return new RtuAnalogParameter()
                       {
                           BrightRate = this.LightRate,
                           BrightRateLowerlimit = this.LightRateLowerLimit,
                           CurrentAlarmLowerlimit = this.LowerLimit,
                           CurrentAlarmUpperlimit = this.UpperLimit,
                           CurrentRange = this.AmRange,
                           IsAlarmHop = this.IsAlarmSwitch,
                           IsSwitchStateClose = this.IsSwitchStateClose,
                           LoopId = this.LoopId,
                           LoopName = this.LoopName,
                           MutualInductorRatio = this.Range,
                           RtuId = this.RtuId,
                           SwitchOutputId = this.SwitchOutId,
                           VectorSwitchIn = this.VectorSwitchIn,
                           VectorMoniliang = this.VectorMoniliang,
                           VoltagePhaseCode = (EnumVoltagePhase) this.Phase,
                           IsShieldLoop = this.IsShieldLoop,
                           ShieldLittleA = isshieldlittlea

                       };
        }

        #region 参数

        protected bool _isThisIsOneRtuLoop;

        /// <summary>
        /// 终端序号
        /// </summary>
        public bool IsThisIsOneRtuLoop
        {
            get { return _isThisIsOneRtuLoop; }
            set
            {
                if (_isThisIsOneRtuLoop == value) return;
                _isThisIsOneRtuLoop = value;

                this.RaisePropertyChanged(() => this.IsThisIsOneRtuLoop);
            }
        }


        protected string  _isTBkColor;

        /// <summary>
        /// 终端序号
        /// </summary>
        public string  BkColor
        {
            get { return _isTBkColor; }
            set
            {
                if (_isTBkColor == value) return;
                _isTBkColor = value;

                this.RaisePropertyChanged(() => this.BkColor);
            }
        }


        protected string _isTBkColors;

        /// <summary>
        /// 终端序号
        /// </summary>
        public string BkColors
        {
            get { return _isTBkColors; }
            set
            {
                if (_isTBkColors == value) return;
                _isTBkColors = value;

                this.RaisePropertyChanged(() => this.BkColors);
            }
        }



        protected string _foregroundColors;

        /// <summary>
        /// 字体颜色
        /// </summary>
        public string ForegroundColors
        {
            get { return _foregroundColors; }
            set
            {
                if (_foregroundColors == value) return;
                _foregroundColors = value;

                this.RaisePropertyChanged(() => this.ForegroundColors);
            }
        }



         private Visibility _txtVisi;

        /// <summary>
        /// 
        /// </summary>
        public Visibility Visi
        {
            get { return _txtVisi; }
            set
            {
                if (value != _txtVisi)
                {
                    _txtVisi = value;
                    this.RaisePropertyChanged(() => this.Visi);
                }
            }
        }

        protected bool  _isIsEnable;

        /// <summary>
        /// 终端序号
        /// </summary>
        public bool  IsEnable
        {
            get { return _isIsEnable; }
            set
            {
                if (_isIsEnable == value) return;
                _isIsEnable = value;

                this.RaisePropertyChanged(() => this.IsEnable);

            }
        }



        protected bool _isIsLineEnable;

        /// <summary>
        /// 海门 才用门控开关量实现日夜开关灯   跟IsEnable参数相反
        /// </summary>
        public bool IsLineEnable
        {
            get { return _isIsLineEnable; }
            set
            {
                if (_isIsLineEnable == value) return;
                _isIsLineEnable = value;

                this.RaisePropertyChanged(() => this.IsLineEnable);
            }
        }

        #region 回路模拟量参数

        protected int _rtuId;

        /// <summary>
        /// 终端序号
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

        protected int _loopId;

        /// <summary>
        /// 回路序号
        /// </summary>
        public int LoopId
        {
            get { return _loopId; }
            set
            {
                if (_loopId == value) return;
                _loopId = value;

                this.RaisePropertyChanged(() => this.LoopId);
            }
        }

        protected string _loopName;

        /// <summary>
        /// 回路名称
        /// </summary>
        public string LoopName
        {
            get { return _loopName; }
            set
            {
                if (_loopName == value) return;
                _loopName = value;

                this.RaisePropertyChanged(() => this.LoopName);
            }
        }

        protected int _range;
        /// <summary>
        /// 量程/互感器比值
        /// </summary>
        [Range(0, 1275, ErrorMessage = "互感器比值在0到1275之间")]
        public int Range
        {
            get { return _range; }
            set
            {
                if (_range == value) return;
                _range = value;
                InitEvent(7);
                this.RaisePropertyChanged(() => this.Range);
            }
        }

        protected int _AmRange;

        /// <summary>
        /// 电流量程
        /// </summary>
        [Range( 0,1275,ErrorMessage = "电流量程在0到1275之间")]
        public int AmRange
        {
            get { return _AmRange; }
            set
            {
                if (_AmRange == value) return;
                _AmRange = value;
                InitEvent(3);
                this.RaisePropertyChanged(() => this.AmRange);
            }
        }

       
        protected int _upperLimit;

        /// <summary>
        /// 报警上限
        /// </summary>
        public int UpperLimit
        {
            get { return _upperLimit; }
            set
            {
                if (_upperLimit == value) return;
                _upperLimit = value;
                InitEvent(4);
                this.RaisePropertyChanged(() => this.UpperLimit);
            }
        }

        protected int _lowerLimit;

        /// <summary>
        /// 报警下限
        /// </summary>
        public int LowerLimit
        {
            get { return _lowerLimit; }
            set
            {
                if (_lowerLimit == value) return;
                _lowerLimit = value;
                InitEvent(5);
                this.RaisePropertyChanged(() => this.LowerLimit);
            }
        }

        protected int _vector;

        /// <summary>
        /// 口矢参数   采样口矢参数
        /// </summary>
        public int VectorMoniliang
        {
            get { return _vector; }
            set
            {
                if (_vector == value) return;
                _vector = value;
                //InitEvent(2);
                this.RaisePropertyChanged(() => this.VectorMoniliang);
            }
        }

        protected int _phase;

        ///// <summary>
        ///// 电压相位
        ///// </summary>
        //public int Phase
        //{
        //    get { return _phase; }
        //    set
        //    {
        //        if (_phase == value) return;
        //        _phase = value;
        //        this.RaisePropertyChanged(() => this.Phase);
        //    }
        //}

        /// <summary>
        /// 电压相位
        /// </summary>
        public  int Phase
        {
            get
            {
                return _phase;
            }
            set
            {
                //if (_Phase == value) return;
                _phase = value;
                //this.RaisePropertyChanged(() => this.Phase);
                
                for (int t = 0; t < CollectionPhase.Count; t++)
                {
                    if (CollectionPhase[t].Value != _phase)
                        continue;
                    SelectPhaseIndex = t;
                    break;
                }
            }
        }



        protected double _lightRate;

        /// <summary>
        /// 亮灯率计算
        /// </summary>
        public double LightRate
        {
            get { return _lightRate; }
            set
            {
                if (_lightRate == value) return;
                _lightRate = value;

                this.RaisePropertyChanged(() => this.LightRate);
            }
        }

        protected int _lightRateLowerLimit;

        /// <summary>
        /// 亮灯率报警下限
        /// </summary>
        public int LightRateLowerLimit
        {
            get { return _lightRateLowerLimit; }
            set
            {
                if (_lightRateLowerLimit == value) return;
                _lightRateLowerLimit = value;

                this.RaisePropertyChanged(() => this.LightRateLowerLimit);
            }
        }

        #endregion

        #region  开关量参数

        protected int _switchInId;

        /// <summary>
        /// 关联开关量输入信号
        /// </summary>
        public int VectorSwitchIn
        {
            get { return _switchInId; }
            set
            {
                

                if (_switchInId == value) return;
                _switchInId = value;
                this.RaisePropertyChanged(() => this.VectorSwitchIn);
                
            }
        }


        private bool _alarm;

        /// <summary>
        /// 跳变报警
        /// </summary>
        public bool IsAlarmSwitch
        {
            get { return _alarm; }
            set
            {
                if (_alarm == value) return;
                _alarm = value;
                this.RaisePropertyChanged(() => this.IsAlarmSwitch);
                //if (IsAlarmSwitch == false) IsAlarmSwitchSelectItem = IsAlarmSwitchItem[0];
                //else IsAlarmSwitchSelectItem = IsAlarmSwitchItem[1];
                for (int t = 0; t < IsAlarmSwitchItem.Count; t++)
                {
                    if (IsAlarmSwitchItem[t].Key != IsAlarmSwitch)
                        continue;
                    IsAlarmSwitchSelectIndex = t;
                    break;
                }
            }
        }

        //private AlarmSwitchItem _IsAlarmSwitchSelectItem;

        // //<summary>
        // //跳变报警
        // //</summary>
        //public AlarmSwitchItem IsAlarmSwitchSelectItem
        //{
        //    get { return _IsAlarmSwitchSelectItem; }
        //    set
        //    {
        //        if (_IsAlarmSwitchSelectItem == value) return;
        //        _IsAlarmSwitchSelectItem = value;
        //        InitEvent(1);
        //        this.RaisePropertyChanged(() => this.IsAlarmSwitchSelectItem);
        //        if (IsAlarmSwitchSelectItem == IsAlarmSwitchItem[0]) IsAlarmSwitch = false;
        //        else IsAlarmSwitch = true;
        //    }
        //}

        private int _IsAlarmSwitchSelectIndex;
        public int IsAlarmSwitchSelectIndex
        {
            get
            {
                return _IsAlarmSwitchSelectIndex;
            }
            set
            {
                if (_IsAlarmSwitchSelectIndex != value)
                {
                    _IsAlarmSwitchSelectIndex = value;
                    InitEvent(1);
                    this.RaisePropertyChanged(() => this.IsAlarmSwitchSelectIndex);
                    IsAlarmSwitch = IsAlarmSwitchItem[value].Key;
                }
            }
        }



        private ObservableCollection<AlarmSwitchItem> _IsAlarmSwitchItem;

        /// <summary>
        /// 跳变报警
        /// </summary>
        public ObservableCollection<AlarmSwitchItem> IsAlarmSwitchItem
        {
            get
            {
                if (_IsAlarmSwitchItem == null)
                {
                    _IsAlarmSwitchItem = new ObservableCollection<AlarmSwitchItem>();
                    _IsAlarmSwitchItem.Add(new AlarmSwitchItem() { Name = "不报警", Key = false });
                    _IsAlarmSwitchItem.Add(new AlarmSwitchItem() { Name = "报警", Key = true });
                }

                return _IsAlarmSwitchItem;
            }
        }


        public class AlarmSwitchItem : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private bool _key;

            public bool Key
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


        private bool _IsSwitchStateClose;

        /// <summary>
        /// 开关量输入 是否为常闭状态
        /// </summary>
        public bool IsSwitchStateClose
        {
            get 
            {
                return _IsSwitchStateClose; 
            }
            set
            {
                if (_IsSwitchStateClose == value) return;
                _IsSwitchStateClose = value;
                this.RaisePropertyChanged(() => this.IsSwitchStateClose);
                //if (_IsSwitchStateClose == false) IsSwitchStateCloseSelectItem = IsSwitchStateCloseItem[0];
                //else IsSwitchStateCloseSelectItem = IsSwitchStateCloseItem[1];
                for (int t = 0; t < IsSwitchStateCloseItem.Count; t++)
                {
                    if (IsSwitchStateCloseItem[t].Key != _IsSwitchStateClose)
                        continue;
                    IsSwitchStateCloseSelectIndex = t;
                    break;
                }
            }
        }

        //private SwitchStateCloseItem _IsSwitchStateCloseSelectItem;

        ///// <summary>
        ///// 开关量输入 是否为常闭状态
        ///// </summary>
        //public SwitchStateCloseItem IsSwitchStateCloseSelectItem
        //{
        //    get { return _IsSwitchStateCloseSelectItem; }
        //    set
        //    {
        //        if (_IsSwitchStateCloseSelectItem == value) return;
        //        _IsSwitchStateCloseSelectItem = value;
        //        InitEvent(0);
        //        this.RaisePropertyChanged(() => this.IsSwitchStateCloseSelectItem);
        //        if (IsSwitchStateCloseSelectItem == IsSwitchStateCloseItem[0]) IsSwitchStateClose = false;
        //        else IsSwitchStateClose = true;
        //    }
        //}


        private int _IsSwitchStateCloseSelectIndex;
        public int IsSwitchStateCloseSelectIndex
        {
            get
            {
                return _IsSwitchStateCloseSelectIndex;
            }
            set
            {
                if (_IsSwitchStateCloseSelectIndex != value)
                {
                    _IsSwitchStateCloseSelectIndex = value;
                    InitEvent(0);
                    this.RaisePropertyChanged(() => this.IsSwitchStateCloseSelectIndex);
                    IsSwitchStateClose = IsSwitchStateCloseItem[value].Key;
                }
            }
        }


        private ObservableCollection<SwitchStateCloseItem> _IsSwitchStateCloseItem;

        /// <summary>
        /// 开关量输入
        /// </summary>
        public ObservableCollection<SwitchStateCloseItem> IsSwitchStateCloseItem
        {
            get
            {
                if (_IsSwitchStateCloseItem == null)
                {
                    _IsSwitchStateCloseItem = new ObservableCollection<SwitchStateCloseItem>();
                    _IsSwitchStateCloseItem.Add(new SwitchStateCloseItem() { Name = "常开", Key = false});
                    _IsSwitchStateCloseItem.Add(new SwitchStateCloseItem() { Name = "常闭", Key = true});
                }
                
                return _IsSwitchStateCloseItem;
            }
        }


        public class SwitchStateCloseItem : Wlst.Cr.Core.CoreServices.ObservableObject
        {
            private bool _key;

            public bool Key
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




















        protected int _switchOutId;

        /// <summary>
        /// 关联开关量输出信号
        /// </summary>
        public int SwitchOutId
        {
            get { return _switchOutId; }
            set
            {
                _constColor = GetColor();     //lvf
                if (value >= 0 && value < 9)
                {
                    BkColor = _constColor[value];
                    if (value == 0)
                    {
                        BkColors = _constColor[value];// "black";
                        IsEnable = false;
                        //lvf 2019年6月5日14:53:04 海门门控实现线控日夜报警
                        if (Wlst.Cr.CoreMims.SystemOption.GetOptionIsDefaults(2001, 0) != true)
                        {
                            IsLineEnable = true;
                        }
                        else
                        {
                            IsLineEnable = false;
                        }
                        ForegroundColors = "#FFB8B8B8"; //Color.FromArgb(0xff, 0xb8, 0xb8, 0xb8);
                    }
                    else
                    {
                        BkColors = _constColor[value];
                        IsEnable = true;
                        //lvf 2019年6月5日14:53:04 海门门控实现线控日夜报警
                        if (Wlst.Cr.CoreMims.SystemOption.GetOptionIsDefaults(2001, 0) != true)
                        {
                            IsLineEnable = false;
                        }
                        else
                        {
                            IsLineEnable = true;
                        }
                        ForegroundColors = "Black";
                    }
                }
                if (_switchOutId == value) return;
                _switchOutId = value;
                this.RaisePropertyChanged(() => this.SwitchOutId);

            }
        }

        private string switchoutname;
        /// <summary>
        /// 关联开关量名称
        /// </summary>
        public string SwichtOutName
        {
            get { return switchoutname; }
            set
            {
                if (value != switchoutname)
                {
                    switchoutname = value;
                    this.RaisePropertyChanged(() => this.SwichtOutName);
                }
            }
        }

        #endregion

        #endregion

        #region 屏蔽
        protected string _IsShieldLittleA;
        /// <summary>
        /// 屏蔽小电流
        /// </summary>
        public string IsShieldLittleA
        {
            get { return _IsShieldLittleA; }
            set
            {
                if (_IsShieldLittleA == value) return;
                double isshield;
                if (double.TryParse(value,out isshield))
                {
                    value = isshield.ToString("f1");
                    if (isshield < 0.1 || isshield >1000)
                    {
                        value = "不屏蔽";
                    }
                }
                else
                {
                    value = "不屏蔽";
                }
                _IsShieldLittleA = value;
                InitEvent(8);
                this.RaisePropertyChanged(() => this.IsShieldLittleA);
            }
        }

        private int _IsShieldLoop;

        /// <summary>
        /// 屏蔽回路 0不屏蔽 ,1屏蔽,2屏蔽并隐藏
        /// </summary>
        public int IsShieldLoop
        {
            get { return _IsShieldLoop; }
            set
            {
                if (_IsShieldLoop == value) return;
                _IsShieldLoop = value;
                this.RaisePropertyChanged(() => this.IsShieldLoop);
                for (int t = 0; t < IsShieldLoopItem.Count; t++)
                {
                    if (IsShieldLoopItem[t].Key != _IsShieldLoop)
                        continue;
                    IsShieldLoopSelectIndex = t;
                    break;
                }
            }
        }

        //private ShieldLoop _IsShieldLoopSelectItem;

        ///// <summary>
        ///// 屏蔽回路
        ///// </summary>
        //public ShieldLoop IsShieldLoopSelectItem
        //{
        //    get { return _IsShieldLoopSelectItem; }
        //    set
        //    {
        //        if (_IsShieldLoopSelectItem == value) return;
        //        _IsShieldLoopSelectItem = value;
        //        InitEvent(9);
        //        this.RaisePropertyChanged(() => this.IsShieldLoopSelectItem);
        //        if (IsShieldLoopSelectItem == _IsShieldLoopItem[0]) IsShieldLoop = false;
        //        else IsShieldLoop = true;
        //    }
        //}


        private int _IsShieldLoopSelectIndex;
        public int IsShieldLoopSelectIndex
        {
            get
            {
                return _IsShieldLoopSelectIndex;
            }
            set
            {
                if (_IsShieldLoopSelectIndex != value)
                {
                    _IsShieldLoopSelectIndex = value;
                    InitEvent(9);
                    this.RaisePropertyChanged(() => this.IsShieldLoopSelectIndex);
                    IsShieldLoop = IsShieldLoopItem[value].Key;
                }
            }
        }



        private ObservableCollection<ShieldLoop> _IsShieldLoopItem;

        /// <summary>
        /// 屏蔽回路
        /// </summary>
        public ObservableCollection<ShieldLoop> IsShieldLoopItem
        {
            get
            {
                if (_IsShieldLoopItem == null)
                {
                    _IsShieldLoopItem = new ObservableCollection<ShieldLoop>();
                    _IsShieldLoopItem.Add(new ShieldLoop() { Name = "不屏蔽", Key = 0 });
                    _IsShieldLoopItem.Add(new ShieldLoop() { Name = "屏蔽", Key = 1 });
                    _IsShieldLoopItem.Add(new ShieldLoop() { Name = "屏蔽并隐藏", Key = 2 });
                }

                return _IsShieldLoopItem;
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

        #region assist Phase show

        //辅助显示接触器状态所有选择
        public ObservableCollection<StateBase> CollectionPhase
        {
            get
            {
                return Comm.CollectionPhase;
            }
        }

        private int _SelectPhaseIndex;

        //辅助显示接触器状态当前选择
        public int SelectPhaseIndex
        {
            get
            {
                return _SelectPhaseIndex;
            }
            set
            {
                if (_SelectPhaseIndex != value)
                {
                    _SelectPhaseIndex = value;
                    InitEvent(6);
                    this.RaisePropertyChanged(() => this.SelectPhaseIndex);
                    Phase = CollectionPhase[value].Value;
                }
            }
        }



        #endregion


        #region 批量操作
        private void InitEvent(int i)
        {
            var args = new PublishEventArgs()
            {
                EventType = "Wlst.Ux.Nr6005Module.Wj3005TmlInfoSetViewModel.ViewModel.RtuLoopInfoVm",
                EventId = 20161012
            };
            args.AddParams(i, this);
            EventPublish.PublishEvent(args);
        }
        #endregion
    }

    public partial class NewDataSettingViewModel
    {

        public const string XmlConfigName = "NewDataLenghtSetConfg";

        /// <summary>
        /// RowHeight LoopNameLength TimeNameLength VaNameLength
        /// </summary>
        /// <returns></returns>
        public static Tuple<int, int, int, int, int, bool, BackgroundSet> LoadNewDataLenghtSetConfg()
        {
            //public static int RowHeight = 25;
            //public static int LoopNameLength = 120;
            //public static int TimeNameLength = 120;
            //public static int VaNameLength = 80;

            int x1 = 0, x2 = 0, x3 = 0, x4 = 0, x5 = 0;
            int x6 = 0;
            int x7 = 0;
            string background = "",
                   k1background = "",
                   k2background = "",
                   k3background = "",
                   k4background = "",
                   k5background = "",
                   k6background = "",
                   k7background = "",
                   k8background = "";
            bool isshow = false;
            var info = Wlst.Cr.CoreOne.Services.SystemXmlConfig.Read(XmlConfigName);
            if (info.ContainsKey("NewDataRowHeight"))
            {
                try
                {
                    x1 = Convert.ToInt32(info["NewDataRowHeight"]);
                }
                catch (Exception ex)
                {
                }
            }


            if (info.ContainsKey("NewDataLoopNameLength"))
            {
                try
                {
                    x2 = Convert.ToInt32(info["NewDataLoopNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }


            if (info.ContainsKey("NewDataTimeNameLength"))
            {
                try
                {
                    x3 = Convert.ToInt32(info["NewDataTimeNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }



            if (info.ContainsKey("NewDataVaNameLength"))
            {
                try
                {
                    x4 = Convert.ToInt32(info["NewDataVaNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("IsShowLoopId"))
            {
                try
                {
                    isshow = Convert.ToInt32(info["IsShowLoopId"]) == 1 ? true : false;
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("RtuNameLength"))
            {
                try
                {
                    x5 = Convert.ToInt32(info["RtuNameLength"]);
                }
                catch (Exception ex)
                {
                }
            }

            if (info.ContainsKey("OnMeasureShowData"))
            {
                try
                {
                    x6 = Convert.ToInt32(info["OnMeasureShowData"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("ShowDw"))
            {
                try
                {
                    x7 = Convert.ToInt32(info["ShowDw"]);
                }
                catch (Exception ex)
                {
                }
            }
            if (info.ContainsKey("BackgroundColor"))
            {
                try
                {
                    background = info["BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                background = "Black";
            }

            if (info.ContainsKey("K1BackgroundColor"))
            {
                try
                {
                    k1background = info["K1BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k1background = "Black";
            }
            if (info.ContainsKey("K2BackgroundColor"))
            {
                try
                {
                    k2background = info["K2BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k2background = "Black";
            }
            if (info.ContainsKey("K3BackgroundColor"))
            {
                try
                {
                    k3background = info["K3BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k3background = "Black";
            }
            if (info.ContainsKey("K4BackgroundColor"))
            {
                try
                {
                    k4background = info["K4BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k4background = "Black";
            }
            if (info.ContainsKey("K5BackgroundColor"))
            {
                try
                {
                    k5background = info["K5BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k5background = "Black";
            }
            if (info.ContainsKey("K6BackgroundColor"))
            {
                try
                {
                    k6background = info["K6BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k6background = "Black";
            }
            if (info.ContainsKey("K7BackgroundColor"))
            {
                try
                {
                    k7background = info["K7BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k7background = "Black";
            }
            if (info.ContainsKey("K8BackgroundColor"))
            {
                try
                {
                    k8background = info["K8BackgroundColor"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                k8background = "Black";
            }
            if (x1 < 15) x1 = 15;
            if (x2 < 60) x2 = 60;
            if (x3 < 60) x3 = 60;
            if (x4 < 60) x4 = 60;
            if (x5 < 250) x5 = 250;
            return new Tuple<int, int, int, int, int, bool, BackgroundSet>(x1, x2, x3, x4, x5, isshow,
                                                                                 new BackgroundSet()
                                                                                 {
                                                                                     Background = background,
                                                                                     K1Background = k1background,
                                                                                     K2Background = k2background,
                                                                                     K3Background = k3background,
                                                                                     K4Background = k4background,
                                                                                     K5Background = k5background,
                                                                                     K6Background = k6background,
                                                                                     K7Background = k7background,
                                                                                     K8Background = k8background,
                                                                                     OnMeasureShowData = x6 == 1,
                                                                                     ShowDw = x7 == 0
                                                                                 });

        }


    }

    public class BackgroundSet
    {
        public string Background { get; set; }
        public string K1Background { get; set; }
        public string K2Background { get; set; }
        public string K3Background { get; set; }
        public string K4Background { get; set; }
        public string K5Background { get; set; }
        public string K6Background { get; set; }
        public string K7Background { get; set; }
        public string K8Background { get; set; }
        public bool OnMeasureShowData { get; set; }
        public bool ShowDw { get; set; }
    }
}
