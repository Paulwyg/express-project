using System;
using Wlst.Cr.Core.CoreServices;
using Wlst.client;


namespace Wlst.Ux.Wj1090Module.Wj1090LduDataQueryViewModel.ViewModel
{
    public class LduDataQueryoneItemViewModel : ObservableObject
    {
        #region 序号 自增

        private int _recordIndex;
        public int RecordIndex
        {
            get { return _recordIndex; }
            set
            {
                if (value != _recordIndex)
                {

                    _recordIndex = value;
                    RaisePropertyChanged(() => RecordIndex);
                }
            }
        }


        #endregion

        #region 终端名称
        private string _attachRtuName;
        public string AttachRtuName
        {
            get { return _attachRtuName; }
            set
            {
                if (value != _attachRtuName)
                {

                    _attachRtuName = value;
                    RaisePropertyChanged(() => AttachRtuName);
                }
            }
        }
        #endregion

        #region 集中器名称
        private string _rtuName;
        public string RtuName
        {
            get { return _rtuName; }
            set
            {
                if (value != _rtuName)
                {

                    _rtuName = value;
                    RaisePropertyChanged(() => RtuName);
                }
            }
        }
        #endregion

        #region 采样时间
        private DateTime _dateCreate;
        public DateTime DateCreate
        {
            get { return _dateCreate; }
            set
            {
                if (value != _dateCreate)
                {

                    _dateCreate = value;
                    RaisePropertyChanged(() => DateCreate);
                }
            }
        }
        #endregion

        #region 线路名称

        private string _lineLoopName;
        public string LineLoopName
        {
            get { return _lineLoopName; }
            set
            {
                if (value != _lineLoopName)
                {

                    _lineLoopName = value;
                    RaisePropertyChanged(() => LineLoopName);
                }
            }
        }
        #endregion

        #region 电压
        private double _v;
        public double V
        {
            get { return _v; }
            set
            {
                if (!_v.Equals(value))
                {

                    _v = value;
                    RaisePropertyChanged(() => V);
                }
            }
        }
        #endregion
        #region 电流
        private double _a;
        public double A
        {
            get { return _a; }
            set
            {
                if (!value.Equals(_a))
                {

                    _a = value;
                    RaisePropertyChanged(() => A);
                }
            }
        }
        #endregion

        #region 有功功率
        private double _powerActive;
        public double PowerActive
        {
            get { return _powerActive; }
            set
            {
                if (!value.Equals(_powerActive))
                {

                    _powerActive = value;
                    RaisePropertyChanged(() => PowerActive);
                }
            }
        }

        #endregion

        #region 无功功率
        private double _powerReActive;
        public double PowerReActive
        {
            get { return _powerReActive; }
            set
            {
                if (!value.Equals(_powerReActive))
                {

                    _powerReActive = value;
                    RaisePropertyChanged(() => PowerReActive);
                }
            }
        }
        #endregion

        #region 功率因数
        private double _powerFactor;
        public double PowerFactor
        {
            get { return _powerFactor; }
            set
            {
                if (value > 1 && value < 1.2) value = 1;
                if (!value.Equals(_powerFactor))
                {

                    _powerFactor = value;
                    RaisePropertyChanged(() => PowerFactor);
                }
            }
        }
        #endregion

        #region 亮灯率
        private double _brightRate;
        public double BrightRate
        {
            get { return _brightRate; }
            set
            {
                if (!value.Equals(_brightRate))
                {

                    _brightRate = value;
                    RaisePropertyChanged(() => BrightRate);
                }
            }
        }
        #endregion

        #region 信号强度

        private int _single;
        public int Single
        {
            get { return _single; }
            set
            {
                if (!value.Equals(_single))
                {

                    _single = value;
                    RaisePropertyChanged(() => Single);
                }
            }
        }
        #endregion

        #region 线路阻抗
        private int _impedance;
        public int Impedance
        {
            get { return _impedance; }
            set
            {
                if (!value.Equals(_impedance))
                {

                    _impedance = value;
                    RaisePropertyChanged(() => Impedance);
                }
            }
        }
        #endregion

        #region 12秒信号数
        private int _numOfUseFullSingleIn12Sec;
        public int NumOfUseFullSingleIn12Sec
        {
            get { return _numOfUseFullSingleIn12Sec; }
            set
            {
                if (!value.Equals(_numOfUseFullSingleIn12Sec))
                {

                    _numOfUseFullSingleIn12Sec = value;
                    RaisePropertyChanged(() => NumOfUseFullSingleIn12Sec);
                }
            }
        }
        #endregion

        #region 12秒跳变数
        private int _numOfSingleIn12Sec;
        public int NumOfSingleIn12Sec
        {
            get { return _numOfSingleIn12Sec; }
            set
            {
                if (!value.Equals(_numOfSingleIn12Sec))
                {

                    _numOfSingleIn12Sec = value;
                    RaisePropertyChanged(() => NumOfSingleIn12Sec);
                }
            }
        }
        #endregion
        #region 检测标识
        private string _flagDetection;
        public string FlagDetection
        {
            get { return _flagDetection; }
            set
            {
                if (!value.Equals(_flagDetection))
                {

                    _flagDetection = value;
                    RaisePropertyChanged(() => FlagDetection);
                }
            }
        }
        #endregion

        #region 报警标识/报警状态
        private string _flagAlarm;
        public string FlagAlarm
        {
            get { return _flagAlarm; }
            set
            {
                if (!value.Equals(_flagAlarm))
                {

                    _flagAlarm = value;
                    RaisePropertyChanged(() => FlagAlarm);
                }
            }
        }
        #endregion
        #region 是否被盗

        /// <summary>
        /// 回路1报警标识  故障数据
        /// </summary>
        private string _flsdfsd; //回路1报警标识

        public string FlagIsDao
        {
            get { return _flsdfsd; }
            set
            {
                if (value.Equals(_flsdfsd)) return;
                _flsdfsd = value;
                RaisePropertyChanged("FlagIsDao");
            }
        }

        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">LduLineData 数据</param>
        /// <param name="index">序号-自增从0开始</param>
        /// 
        public LduDataQueryoneItemViewModel(LduLineData item, int index )
        {
            var dh = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(item.LduId);//.GetEquipmentInfo(item.LduId);
            if (dh != null && dh.RtuFid != 0)  ResolveAttachName(dh .RtuFid ); 


            RecordIndex = index + 1;
          
            ResolveRtuName(item.LduId );
            DateCreate =new DateTime(  item.DateCreate);
            ResolveLduLineName(item.LineId ,item.LduId );
            V = item.V;
            A = item.A;
            PowerActive = item.PowerActive;
            PowerReActive = item.PowerReActive;
            PowerFactor = item.PowerFactor;
            BrightRate = item.BrightRate;
            Single = item.Single;
            Impedance = item.Impedance;
            NumOfUseFullSingleIn12Sec = item.NumofUsefullSingleIn12Sec;
            NumOfSingleIn12Sec = item.NumofSingleIn12Sec;
            string temp = Convert.ToString(item.FlagAlarm, 2);
            if(temp.Length<8)
            {
                int i = 0;
                while (8-temp.Length>0)
                {
                    temp = "0" + temp;
                    i++;
                }
            }
            FlagAlarm =temp;
            temp = Convert.ToString(item.FlagDetection, 2);
            if(temp.Length<8)
            {
                int i = 0;
                while (8-temp.Length>0)
                {
                    temp = "0" + temp;
                    i++;
                }
            }
            FlagDetection = temp;

            var tmps = Ux.Wj1090Module.Services.Alarm.GetInfo(item.FlagAlarm, item.FlagDetection);
            if (tmps.Item1 && tmps.Item2) FlagIsDao = "被盗-短路";
            else if (tmps.Item1) FlagIsDao = "被盗";
            else if (tmps.Item2) FlagIsDao = "短路";
            else FlagIsDao = "正常";
        }

        #region 解析名称

        //解析终端名称
        private void ResolveAttachName(int attachId)
        {
            AttachRtuName = "Reserve";
            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey( attachId))
                     //EquipmentInfoDictionary.ContainsKey
                     //(attachId))
                return;
            var tml =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[attachId];//.EquipmentInfoDictionary
                    //[attachId];
            AttachRtuName = tml.RtuName;
        }
        //解析集中器名称
        private void ResolveRtuName(int rtuId)
        {
            RtuName = "Reserve";
            if (
                !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey
                     (rtuId))
                return;
            var tml =
                Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[rtuId];//.EquipmentInfoDictionary
                    //[rtuId];
            RtuName = tml.RtuName;
        }
        //解析线路名称
        private void ResolveLduLineName(int lduLineId,int rtuId)
        {
            if (lduLineId > 0)
            {
                LineLoopName = "Reserve";
                if (
                    !Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey
                         (rtuId))
                    return;
                var tml =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems
                        [rtuId] as Sr.EquipmentInfoHolding.Model.Wj1090Ldu;// Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;
                if (tml == null) return;
                if (!tml.WjLduLines.ContainsKey(lduLineId)) return;
                LineLoopName = tml.WjLduLines[lduLineId].LduLineName;
            }
        }
        #endregion
    }
}
