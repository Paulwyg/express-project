using System;
using Wlst.Cr.Core.CoreServices;
using Wlst.client;
using System.Windows.Media;

namespace Wlst.Ux.Wj9001Module.Wj9001DataQueryViewModel.ViewModel
{
    public class Wj9001DataQueryoneItemViewModel : ObservableObject
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

        #region 漏电保护器编号

        private int _leakId;
        public int LeakId
        {
            get { return _leakId; }
            set
            {
                if (value != _leakId)
                {

                    _leakId = value;
                    RaisePropertyChanged(() => LeakId);
                }
            }
        }
        #endregion

        #region 漏电 线路编号

        private int _leakLineId;
        public int LeakLineId
        {
            get { return _leakLineId; }
            set
            {
                if (value != _leakLineId)
                {

                    _leakLineId = value;
                    RaisePropertyChanged(() => LeakLineId);
                }
            }
        }
        #endregion

        #region 漏电检测模式  1漏电  2温度

        private int _leakMode;
        public int LeakMode
        {
            get { return _leakMode; }
            set
            {
                if (value != _leakMode)
                {

                    _leakMode = value;
                    RaisePropertyChanged(() => LeakMode);
                }
            }
        }
        #endregion

        #region 自动分闸 自动报警

        private int _autoBreakOrAutoAlarm;
        public int AutoBreakOrAutoAlarm
        {
            get { return _autoBreakOrAutoAlarm; }
            set
            {
                if (value != _autoBreakOrAutoAlarm)
                {

                    _autoBreakOrAutoAlarm = value;
                    RaisePropertyChanged(() => AutoBreakOrAutoAlarm);
                }
            }
        }
        #endregion

        #region 自动分闸 自动报警

        private string _autoBreakOrAutoAlarmTemp;
        public string AutoBreakOrAutoAlarmTemp
        {
            get { return _autoBreakOrAutoAlarmTemp; }
            set
            {
                if (value != _autoBreakOrAutoAlarmTemp)
                {

                    _autoBreakOrAutoAlarmTemp = value;
                    RaisePropertyChanged(() => AutoBreakOrAutoAlarmTemp);
                }
            }
        }
        #endregion

        #region 分合闸状态

        private int _stateofOnOff;
        public int StateofOnOff
        {
            get { return _stateofOnOff; }
            set
            {
                if (value != _stateofOnOff)
                {

                    _stateofOnOff = value;
                    RaisePropertyChanged(() => StateofOnOff);
                }
            }
        }
        #endregion

        #region 分合闸状态

        private string _stateofOnOffTemp;
        public string StateofOnOffTemp
        {
            get { return _stateofOnOffTemp; }
            set
            {
                if (value != _stateofOnOffTemp)
                {

                    _stateofOnOffTemp = value;
                    RaisePropertyChanged(() => StateofOnOffTemp);
                }
            }
        }
        #endregion

        #region 报警状态

        private int _stateofAlarm;
        public int StateofAlarm
        {
            get { return _stateofAlarm; }
            set
            {
                if (value != _stateofAlarm)
                {

                    _stateofAlarm = value;
                    RaisePropertyChanged(() => StateofAlarm);
                }
            }
        }
        #endregion

        #region 报警状态

        private string _stateofAlarmTemp;
        public string StateofAlarmTemp
        {
            get { return _stateofAlarmTemp; }
            set
            {
                if (value != _stateofAlarmTemp)
                {

                    _stateofAlarmTemp = value;
                    RaisePropertyChanged(() => StateofAlarmTemp);
                }
            }
        }
        #endregion

        #region 报警上下限

        private int _upperAlarmOrBreakforLeakOrTemperature;
        public int UpperAlarmOrBreakforLeakOrTemperature
        {
            get { return _upperAlarmOrBreakforLeakOrTemperature; }
            set
            {
                if (value != _upperAlarmOrBreakforLeakOrTemperature)
                {

                    _upperAlarmOrBreakforLeakOrTemperature = value;
                    RaisePropertyChanged(() => UpperAlarmOrBreakforLeakOrTemperature);
                }
            }
        }
        #endregion

        #region 延迟时间

        private int _timeDelayforBreak;
        public int TimeDelayforBreak
        {
            get { return _timeDelayforBreak; }
            set
            {
                if (value != _timeDelayforBreak)
                {

                    _timeDelayforBreak = value;
                    RaisePropertyChanged(() => TimeDelayforBreak);
                }
            }
        }
        #endregion


        #region 报警值

        private int _alarmValueLeakOrTemperature;
        public int AlarmValueLeakOrTemperature
        {
            get { return _alarmValueLeakOrTemperature; }
            set
            {
                if (value != _alarmValueLeakOrTemperature)
                {

                    _alarmValueLeakOrTemperature = value;
                    RaisePropertyChanged(() => AlarmValueLeakOrTemperature);
                }
            }
        }
        #endregion

        #region 当前值

        private int _currentLeakOrTemperature;
        public int CurrentLeakOrTemperature
        {
            get { return _currentLeakOrTemperature; }
            set
            {
                if (value != _currentLeakOrTemperature)
                {

                    _currentLeakOrTemperature = value;
                    RaisePropertyChanged(() => CurrentLeakOrTemperature);
                }
            }
        }
        #endregion

        #region 行色
        private string _rowBackgroudColor;

        /// <summary>
        /// 设置行背景颜色
        /// </summary>
        public string rowBackgroudColor
        {
            get { return _rowBackgroudColor; }
            set
            {
                if (value == _rowBackgroudColor) return;
                _rowBackgroudColor = value;
                RaisePropertyChanged(() => rowBackgroudColor);
            }
        }

        private string _rowForeColor;

        /// <summary>
        /// 设置行前景颜色
        /// </summary>
        public string rowForeColor
        {
            get { return _rowForeColor; }
            set
            {
                if (value == _rowForeColor) return;
                _rowForeColor = value;
                RaisePropertyChanged(() => rowForeColor);
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">LduLineData 数据</param>
        /// <param name="index">序号-自增从0开始</param>
        /// 
        public Wj9001DataQueryoneItemViewModel(LeakNewData.LeakNewDataItem item, int index )
        {
            var dh = Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(item.LeakId);//.GetEquipmentInfo(item.LduId);
            if (dh != null && dh.RtuFid != 0)  ResolveAttachName(dh .RtuFid ); 


            RecordIndex = index + 1;
          
            ResolveRtuName(item.LeakId );
            DateCreate =new DateTime(  item.DateCreate);
            ResolveLeakLineName(item.LeakLineId ,item.LeakId );
            LeakId = item.LeakId;
            LeakLineId = item.LeakLineId;
            LeakMode = item.LeakMode;
            AutoBreakOrAutoAlarm = item.AutoBreakOrAutoAlarm;   //1自动分闸  2自动报警
            if(AutoBreakOrAutoAlarm==1)
            {
                AutoBreakOrAutoAlarmTemp = "自动分闸";
            }
            else if (AutoBreakOrAutoAlarm==2)
            {
                AutoBreakOrAutoAlarmTemp = "自动报警";
            }
            StateofOnOff = item.StateofOnOff;   //分合闸状态   1 分  2合
            if (StateofOnOff == 1)
            {
                StateofOnOffTemp = "分闸";
            }
            else if (StateofOnOff == 2)
            {
                StateofOnOffTemp = "合闸";
            }
            else
            {
                StateofOnOffTemp = "正常";
            }
            StateofAlarm = item.StateofAlarm;  //报警状态   1报警  2无警
            if (StateofAlarm == 1)
            {
                StateofAlarmTemp = "有警";
                rowBackgroudColor = "#FFC20303";
                rowForeColor = "White";
            }
            else if (StateofAlarm == 2)
            {
                StateofAlarmTemp = "无警";
                rowBackgroudColor = "White";
                rowForeColor = "Black";
            }
            else
            {
                StateofAlarmTemp = "正常";
                rowBackgroudColor = "White";
                rowForeColor = "Black";
            }

            UpperAlarmOrBreakforLeakOrTemperature = item.UpperAlarmOrBreakforLeakOrTemperature;
            TimeDelayforBreak = item.TimeDelayforBreak;
            AlarmValueLeakOrTemperature = item.AlarmValueLeakOrTemperature;
            CurrentLeakOrTemperature = item.CurrentLeakOrTemperature;

            //string temp = Convert.ToString(item.FlagAlarm, 2);
            //if(temp.Length<8)
            //{
            //    int i = 0;
            //    while (8-temp.Length>0)
            //    {
            //        temp = "0" + temp;
            //        i++;
            //    }
            //}
            //FlagAlarm =temp;
            //temp = Convert.ToString(item.FlagDetection, 2);
            //if(temp.Length<8)
            //{
            //    int i = 0;
            //    while (8-temp.Length>0)
            //    {
            //        temp = "0" + temp;
            //        i++;
            //    }
            //}
            //FlagDetection = temp;

            ////////var tmps = Ux.Wj9001Module.Services.Alarm.GetInfo(item.FlagAlarm, item.FlagDetection); todo
            ////////if (tmps.Item1 && tmps.Item2) FlagIsDao = "被盗-短路";
            ////////else if (tmps.Item1) FlagIsDao = "被盗";
            ////////else if (tmps.Item2) FlagIsDao = "短路";
            ////////else FlagIsDao = "正常";
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
        private void ResolveLeakLineName(int lduLineId,int rtuId)
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
                        [rtuId] as Sr.EquipmentInfoHolding.Model.Wj9001Leak;// Cr.WjEquipmentBaseModels.WjEquipment.Wj1090.Wj1090TerminalInformation;
                if (tml == null) return;
                if (!tml.WjLeakLines.ContainsKey(lduLineId)) return;
                LineLoopName = tml.WjLeakLines[lduLineId].LineName;
            }
        }
        #endregion
    }
}
