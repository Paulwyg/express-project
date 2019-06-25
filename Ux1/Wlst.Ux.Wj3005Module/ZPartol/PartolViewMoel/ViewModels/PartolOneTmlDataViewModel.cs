using System;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.Services;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.WJ3005Module.ZPartol.PartolViewMoel.ViewModels
{
    /// <summary>
    /// 巡测时 一个终端的数据vm
    /// </summary>
    public class PartolOneTmlDataViewModel : ObservableObject
    {
        public const int OnBitMapImageId = 1001;
        public const int OffBitMapImageId = 1002;
        private int _rtuId;
        private string _rtuName;
        private DateTime _sndRequestNewDataTime;
        private DateTime _rcvNewDataTime;

        private string  _timeSpan;

        private readonly BitmapSource onBitmap;
        private readonly BitmapSource offBitmap;
        public BitmapSource[] _SwitchOutImg;
        private Color[] _SwitchOutColor;
        public PartolOneTmlDataViewModel()
        {
            _sndRequestNewDataTime = new DateTime(1987, 10, 6);
            _rcvNewDataTime = new DateTime(1987, 10, 6);

            _SwitchOutColor = new Color[16];
            SwitchOutLoopSum = 6;
            RequestNewDataTime = "--";
            ReceiveNewDataTimeTime = "--";
            _RcvNewDataCount = 10;//默认接收最新数据设置为10  

            _SwitchOutImg = new BitmapSource[16];
            onBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(PartolOneTmlDataViewModel .OnBitMapImageId  );// Resources.Onimageimageimage;
            offBitmap = ImageSourceHelper.MySelf.GetBitmapSourceById(OffBitMapImageId); 
            for (int i = 0; i < 16; i++)
            {
                _SwitchOutColor[i] = Colors.Red;
                _SwitchOutImg[i] = offBitmap;
            }
          
        }


        /// <summary>
        /// 开关量输出路数 最多支持16路 默认6路
        /// </summary>
        public int SwitchOutLoopSum;

        /// <summary>
        /// 如果先安装设备 设备数据上来时软件并未增加此终端，则巡测数据显示 “未知终端” 
        /// 当软件增加此终端后 名称应该立即更新，则在改设置数据下一次到达时更新名称
        /// </summary>
        private bool RtuHasNoName;
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

                RtuName = "未知终端";
                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems  .ContainsKey(_rtuId))
                {
                    var t = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems  [_rtuId];
                  
                        RtuName = t.RtuName;
                    
                    RtuHasNoName = false;
                }
                else
                {
                    RtuHasNoName = true;
                }
            }
        }

        /// <summary>
        /// 终端名称 设置好地址后会自动查找
        /// </summary>
        public string RtuName
        {
            get { return _rtuName; }
            private set
            {
                if (_rtuName == value) return;
                _rtuName = value;
                this.RaisePropertyChanged(() => this.RtuName);
            }
        }



        /// <summary>
        /// 设置发送选测时间
        /// </summary>
        public void SetSndRequestNewDataTime()
        {
            //发送最新数据请求时  设置发送时间 但清除接收时间 接收时间与时间差均为 --
            _sndRequestNewDataTime = DateTime.Now;
            RcvNewDataTime = new DateTime(1987, 10, 6);
            RequestNewDataTime = _sndRequestNewDataTime.ToString(CultureInfo.InvariantCulture);
            RcvNewDataCount = 0;
        }



        private string _RequestNewDataTime;
        /// <summary>
        /// 请求终端最新数据时间
        /// </summary>
        public string RequestNewDataTime
        {
            get { return _RequestNewDataTime; }
            set
            {
                if (_RequestNewDataTime == value) return;
                _RequestNewDataTime = value;
                this.RaisePropertyChanged(() => this.RequestNewDataTime);
            }
        }
        private int  _Reqerror;
        /// <summary>
        /// 请求终端最新数据时间
        /// </summary>
        public int ErrorCount
        {
            get { return _Reqerror; }
            set
            {
                if (_Reqerror == value) return;
                _Reqerror = value;
                this.RaisePropertyChanged(() => this.ErrorCount);
            }
        }


        /// <summary>
        /// 接收时间
        /// </summary>
        private DateTime RcvNewDataTime
        {
            get { return _rcvNewDataTime; }
            set
            {
                if (_rcvNewDataTime == value) return;
                _rcvNewDataTime = value;
                //如果接收时间为 初始化的时间则 接收时间与时间差 均为--
                if (_rcvNewDataTime < new DateTime(2012, 1, 1))
                {
                    ReceiveNewDataTimeTime = "--";
                    TimeSpan = "--";
                    return;
                }

                ReceiveNewDataTimeTime = _rcvNewDataTime.ToString(CultureInfo.InvariantCulture);
                //如果发送时间为初始值的话 不计算时间差
                if (_sndRequestNewDataTime > new DateTime(2012, 1, 1))
                {
                    var ts2 = new TimeSpan(_sndRequestNewDataTime.Ticks);
                    var ts1 = new TimeSpan(_rcvNewDataTime.Ticks);
                    var ts = ts1.Subtract(ts2).Duration();  
                   // long lng = ts.Days*24*60*60 + ts.Hours*60*60 + ts.Minutes*60 + ts.Seconds;
                    TimeSpan = ts.TotalSeconds.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    TimeSpan = "--";
                }
            }
        }

        private string _ReceiveNewDataTimeTime;
        //需要显示的接收最新数据时间
        public string ReceiveNewDataTimeTime
        {
            get { return _ReceiveNewDataTimeTime; }
            set
            {
                if (_ReceiveNewDataTimeTime == value) return;
                _ReceiveNewDataTimeTime = value;
                this.RaisePropertyChanged(() => this.ReceiveNewDataTimeTime);
            }
        }


        /// <summary>
        /// 发送与接收时间长
        /// </summary>
        public string  TimeSpan
        {
            get { return _timeSpan; }
            private set
            {
                if (_timeSpan == value) return;
                _timeSpan = value;
                this.RaisePropertyChanged(() => this.TimeSpan);
            }
        }
         

        public Color[] SwitchOutColor
        {
            get { return _SwitchOutColor; }
        }

        public BitmapSource[] SwitchOutImg
        {
            get { return _SwitchOutImg; }
        }

        /// <summary>
        /// 设置开关量输出吸合状态
        /// </summary>
        /// <param name="LoopK">开关量输出0~15</param>
        /// <param name="IsThisLoopOpen">这个开关量输出是否已经打开 true则为开</param>
        private void SetSwitchOutLoopKShowColor(int LoopK, bool IsThisLoopOpen)
        {
            if (LoopK >= 0 && LoopK < 16)
            {
                if (IsThisLoopOpen)
                {
                    _SwitchOutColor[LoopK] = Colors.Red;
                    _SwitchOutImg[LoopK] = onBitmap;
                }
                else
                {
                    _SwitchOutColor[LoopK] = Colors.White;
                    _SwitchOutImg[LoopK] = offBitmap;
                }
            }
        }

        //E:\CETC50\Programs\CETC50_Demo\BasicFunctionModule\Resource\on.bmp

        public void SetArgs(object obj)
        {
            var newdata = obj as RtuNewDataInfo;// TmlNewData;
            if (newdata == null) return;
            if (newdata.RtuId != RtuId) return;
            RcvNewDataTime = newdata.DateCreate ;

            for (int i = 0; i < newdata.IsSwitchOutAttraction .Count;i++ )
            {
                SetSwitchOutLoopKShowColor(i, newdata.IsSwitchOutAttraction[i]);
            }
            this.RaisePropertyChanged(() => this.SwitchOutColor);
            this.RaisePropertyChanged(() => this.SwitchOutImg);
            RcvNewDataCount++;

            if (RtuHasNoName)
            {

                if (Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems  .ContainsKey(_rtuId))
                {
                    var t = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems  [_rtuId];
                    
                        RtuName = t.RtuName;
                        RtuHasNoName = false;
                   
                }
                else
                {
                    RtuHasNoName = true;
                }
            }
        }


        /// <summary>
        /// 检测是否需要从新召测
        /// </summary>
        /// <param name="MaxMinutes">时效时间 分钟</param>
        /// <returns></returns>
        public bool IsDataOldEnoughForNewData(int MaxMinutes)
        {
            if (MaxMinutes < 1) MaxMinutes = 1;
            if (RcvNewDataTime > DateTime.Now) return true;
            var ts1 = new TimeSpan(DateTime.Now.Ticks);
            var ts2 = new TimeSpan(RcvNewDataTime.Ticks);
            var ts = ts1.Subtract(ts2).Duration();
            long minutes = ts.Days*24*60 + ts.Hours*60 + ts.Minutes;
            if (minutes >= MaxMinutes)
            {
                return true;
            }
            return false;
        }

        private int _RcvNewDataCount;
        /// <summary>
        /// 统计接在上次发送召测命令后收到最新数据的次数
        /// </summary>
        public int RcvNewDataCount
        {
            get { return _RcvNewDataCount; }
            private set { _RcvNewDataCount = value; }
        }
    }
}
