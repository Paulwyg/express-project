using System;

namespace Wlst.Ux.Wj1090Module.Wj1090DataSelection.ViewModel
{
    public class SelectDataModel: Cr.Core.CoreServices.ObservableObject
    {
            
        #region AttachRtuId 连接的主设备地址
        private int _attachRtuId;
            public int AttachRtuId
            {
                get { return _attachRtuId; }
                set
                {
                    if(_attachRtuId.Equals(value)) return;
                    _attachRtuId = value;
                    RaisePropertyChanged("AttachRtuId");
                }
            }
        #endregion

        #region RtuId 集中控制器地址
            private int _rtuId;
            public int RtuId
            {
                get { return _rtuId; }
                set
                {
                    if (_rtuId.Equals(value)) return;
                    _rtuId = value;
                    RaisePropertyChanged("RtuId");
                }
            }
            #endregion
   
        #region DateCreate 数据接收时间
        private DateTime _dateTime;
        public DateTime DateCreate
        {
            get { return _dateTime; }
            set
            {
                if (_dateTime==value) return;
                _dateTime = value;
                RaisePropertyChanged("DateCreate");
            }
        }
            #endregion

        #region LineLoopId
        /// <summary>
            /// 回路地址 1-6
            /// </summary>
        private int _lineLoopId; //回路标识，二进制转十进制
        public int LineLoopId
        {
            get { return _lineLoopId; }
            set
            {
                if(value==_lineLoopId) return;
                _lineLoopId = value;
                RaisePropertyChanged("LineLoopId");
            }
        }
        #endregion

        #region 电压
        private double _v; //回路1电压
        public double V
        {
            get { return _v; }
            set
            {
                if(_v.Equals(value)) return;
                _v = value;
                RaisePropertyChanged(()=>V);
            }
        }
        #endregion

        #region 电流
        private double _a; //回路1电流
        public double A
        {
            get { return _a; }
            set
            {
                if(_a.Equals(value)) return;
                _a = A;
                RaisePropertyChanged("A");
            }
        }
        #endregion

        #region 有功功率
        /// <summary>
            /// 回路1有功功率
            /// </summary>
        private double _powerActive; //回路1有功功率
        public double PowerActive
        {
            get { return _powerActive; }
            set
            {
                if(_powerActive.Equals(value)) return;
                _powerActive = value;
                RaisePropertyChanged("PowerActive");
            }
        }
        #endregion

        #region 无功功率
        /// <summary>
            /// 回路1无功功率
            /// </summary>
        private double _powerReActive; //回路1无功功率
        public double PowerReActive
        {
            get { return _powerReActive; }
            set
            {
                if(_powerReActive.Equals(value)) return;
                _powerReActive = value;
                RaisePropertyChanged("PowerReActive");
            }
        }
        #endregion

        #region 功率因数
        /// <summary>
            /// 回路1功率因数
            /// </summary>
        private double _powerFactor; //回路1功率因数
        public double PowerFactor
        {
            get { return _powerFactor; }
            set
            {
                if(value.Equals(_powerFactor)) return;
                _powerFactor = value;
                RaisePropertyChanged("PowerFactor");
            }
        }
        #endregion

        #region 亮灯率
        /// <summary>
            /// 回路1亮灯率
            /// </summary>
        private double _brightRate; //回路1亮灯率
        public double BrightRate
        {
            get { return _brightRate; }
            set
            {
                if(value.Equals(_brightRate)) return;
                _brightRate = value;
                RaisePropertyChanged("BrightRate");
            }
        }
        #endregion

        #region 信号强度
        /// <summary>
            /// 回路1信号强度 脉冲
            /// </summary>
        private int _single; //回路1信号强度   
        public int Single
        {
            get { return _single; }
            set
            {
                if (value.Equals(_single)) return;
                _single = value;
                RaisePropertyChanged("Single");
            }
        }
        #endregion

        #region 回路1阻抗
        /// <summary>
        /// 回路1阻抗
        /// </summary>
        private int _impedance; //回路1阻抗
        public int Impedance
        {
            get { return _impedance; }
            set
            {
                if (value.Equals(_impedance)) return;
                _impedance = value;
                RaisePropertyChanged("Impedance");
            }
        }
        #endregion

        #region 12s有用信号数量
        /// <summary>
        /// 回路1 12s有用信号数量  阻抗数
        /// </summary>
        private int _numofUsefullSingleIn12Sec; //回路1 12s有用信号数量
        public int NumofUsefullSingleIn12Sec
        {
            get { return _numofUsefullSingleIn12Sec; }
            set
            {
                if (value.Equals(_numofUsefullSingleIn12Sec)) return;
                _numofUsefullSingleIn12Sec = value;
                RaisePropertyChanged("NumofUsefullSingleIn12Sec");
            }
        }
        #endregion

        #region 12s信号数量
        /// <summary>
        /// 回路1 12s信号数量 跳数
        /// </summary>
        private int _numofSingleIn12Sec; //回路1 12s信号数量
        public int NumofSingleIn12Sec
        {
            get { return _numofSingleIn12Sec; }
            set
            {
                if (value.Equals(_numofSingleIn12Sec)) return;
                _numofSingleIn12Sec = value;
                RaisePropertyChanged("NumofSingleIn12Sec");
            }
        }
        #endregion

        #region 检测标识
        /// <summary>
        /// 回路1检测标识 故障参数
        /// </summary>
        private string _flagDetection; //回路1检测标识
        public string FlagDetection
        {
            get { return _flagDetection; }
            set
            {
                if (value.Equals(_flagDetection)) return;
                _flagDetection = value;
                RaisePropertyChanged("FlagDetection");
            }
        }
        #endregion

        #region 报警标识
        /// <summary>
        /// 回路1报警标识  故障数据
        /// </summary>
        private string _flagAlarm; //回路1报警标识
        public string FlagAlarm
        {
            get { return _flagAlarm; }
            set
            {
                if (value.Equals(_flagAlarm)) return;
                _flagAlarm = value;
                RaisePropertyChanged("FlagAlarm");
            }
        }
        #endregion

        public SelectDataModel(Sr.ProtocolCnt.Wj1090.LduLineData item)
        {
            AttachRtuId = item.AttachRtuId;
            RtuId = item.RtuId;
            DateCreate = item.DateCreate;
            LineLoopId = item.LineLoopId;
            V = item.V;
            A = item.A;
            PowerActive = item.PowerActive;
            PowerReActive = item.PowerReActive;
            PowerFactor = item.PowerFactor;
            BrightRate = item.BrightRate;
            Single = item.Single;
            Impedance = item.Impedance;
            NumofUsefullSingleIn12Sec = item.NumofUsefullSingleIn12Sec;
            NumofSingleIn12Sec = item.NumofSingleIn12Sec;
            //FlagDetection = item.FlagDetection;
            //FlagAlarm = item.FlagAlarm;

            string temp = Convert.ToString(item.FlagAlarm, 2);
            if (temp.Length < 8)
            {
                int i = 0;
                while (8 - temp.Length > 0)
                {
                    temp = "0" + temp;
                    i++;
                }
            }
            FlagAlarm = temp;
            temp = Convert.ToString(item.FlagDetection, 2);
            if (temp.Length < 8)
            {
                int i = 0;
                while (8 - temp.Length > 0)
                {
                    temp = "0" + temp;
                    i++;
                }
            }
            FlagDetection = temp;

        }
    }
}
