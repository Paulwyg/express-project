using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj9001Module.NewData.ViewModel
{

    public class LeakLineViewModel : Cr.Core.CoreServices.ObservableObject
    {

        #region AttachRtuId 连接的主设备地址

        private int _attachRtuId;

        public int AttachRtuId
        {
            get { return _attachRtuId; }
            set
            {
                if (_attachRtuId== value) return;
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
                if (_rtuId== value) return;
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
                if (_dateTime == value) return;
                _dateTime = value;
                RaisePropertyChanged("DateCreate");
            }
        }

        #endregion

        #region LeakLineId

        /// <summary>
        /// 回路地址 1-6
        /// </summary>
        private int _lineLoopId; //回路标识，二进制转十进制

        public int LeakLineId
        {
            get { return _lineLoopId; }
            set
            {
                if (value == _lineLoopId) return;
                _lineLoopId = value;
                RaisePropertyChanged("LeakLineId");
            }
        }

        #endregion

        #region LeakLineName

        /// <summary>
        /// 回路名称
        /// </summary>
        private string _lineLoopName;

        public string LeakLineName
        {
            get { return _lineLoopName; }
            set
            {
                if (value == _lineLoopName) return;
                _lineLoopName = value;
                RaisePropertyChanged("LeakLineName");
            }
        }

        #endregion


        #region LeakMode

        private string  _v; //回路1电压
        /// <summary>
        /// 1、漏电检测模式  & 2、温度检测模式
        /// </summary>
        public string  LeakMode
        {
            get { return _v; }
            set
            {
                if (_v== value) return;
                _v = value;
                RaisePropertyChanged(() => LeakMode);
            }
        }

        #endregion

        #region AutoBreakOrAutoAlarm

        private string  _a; //回路1电流
        /// <summary>
        /// 1、自动分闸 & 2、自动报警
        /// </summary>
        public string  AutoBreakOrAutoAlarm
        {
            get { return _a; }
            set
            {
                if (_a== value) return;
                _a = value ;
                RaisePropertyChanged("AutoBreakOrAutoAlarm");
            }
        }

        #endregion

        #region StateofOnOff

        /// <summary>
        ///  闸状态：1、分闸状态，2、合闸状态
        /// </summary>
        private string  _powerActive; //回路1有功功率

        public string  StateofOnOff
        {
            get { return _powerActive; }
            set
            {
                if (_powerActive== value) return;
                _powerActive = value;
                RaisePropertyChanged("StateofOnOff");
            }
        }

        #endregion

        #region StateofAlarm

        /// <summary>
        /// 报警状态 1、报警  2、无警
        /// </summary>
        private string  _powerReActive; //回路1无功功率

        public string  StateofAlarm
        {
            get { return _powerReActive; }
            set
            {
                if (_powerReActive== value) return;
                _powerReActive = value;
                RaisePropertyChanged("StateofAlarm");
            }
        }

        #endregion

        #region UpperAlarmOrBreakforLeakOrTemperature

        /// <summary>
        /// //上限值
        /// 如果漏电模式：a 如果为自动分闸 则为分闸漏电上限值，b如果为自动报警则为漏电报警上限值；
        ///如果为温度检测模式：a 如果为自动分闸则为分闸温度上限值，b 如果为自动报警则为温度报警上限值
        /// </summary>
        private int  _powerFactor; //回路1功率因数

        public int UpperAlarmOrBreakforLeakOrTemperature
        {
            get { return _powerFactor; }
            set
            {
                if (value.Equals(_powerFactor)) return;
                _powerFactor = value;
                RaisePropertyChanged("UpperAlarmOrBreakforLeakOrTemperature");
            }
        }

        #endregion

        #region TimeDelayforBreak

        /// <summary>
        /// 如果为自动分闸模式 则为回路自动分闸延迟时间  主动报警模式无意义
        /// </summary>
        private string   _brightRate; //回路1亮灯率

        public string   TimeDelayforBreak
        {
            get { return _brightRate; }
            set
            {
                if (value.Equals(_brightRate)) return;
                _brightRate = value;
                RaisePropertyChanged("TimeDelayforBreak");
            }
        }


        #endregion

        #region AlarmValueLeakOrTemperature

        /// <summary>
        /// 报警值  当设备检测到温度或漏电大于设定的上限值时  设备回记录发生报警时刻的数据
        /// </summary>
        private int _single; //回路1信号强度   

        public int AlarmValueLeakOrTemperature
        {
            get { return _single; }
            set
            {
                if (value.Equals(_single)) return;
                _single = value;
                RaisePropertyChanged("AlarmValueLeakOrTemperature");
            }
        }

        #endregion

        #region CurrentLeakOrTemperature

        /// <summary>
        /// 当前值  系统选测设备数据时的当前漏电值或温度值
        /// </summary>
        private int _impedance; //回路1阻抗

        public int CurrentLeakOrTemperature
        {
            get { return _impedance; }
            set
            {
                if (value.Equals(_impedance)) return;
                _impedance = value;
                RaisePropertyChanged("CurrentLeakOrTemperature");
            }
        }

        #endregion

      
        public LeakLineViewModel(Wlst.client.LeakNewData .LeakNewDataItem  item)
        {
       

      

            RtuId = item .LeakId ;
            DateCreate = new DateTime(item.DateCreate);
            LeakLineId = item.LeakLineId;           
            LeakMode = item.LeakMode == 1 ? "漏电检测" : "温度检测";
            AutoBreakOrAutoAlarm = item.AutoBreakOrAutoAlarm == 1 ? "自动分闸" : "主动报警";
            StateofOnOff = item.StateofOnOff == 1 ? "分闸状态" : "合闸状态";
            StateofAlarm = item.StateofAlarm == 1 ? "报警" : "无警";
            UpperAlarmOrBreakforLeakOrTemperature = item.UpperAlarmOrBreakforLeakOrTemperature;
            TimeDelayforBreak = item.AutoBreakOrAutoAlarm != 1 ? "--" : item.TimeDelayforBreak+"";
            AlarmValueLeakOrTemperature = item.AlarmValueLeakOrTemperature;
            CurrentLeakOrTemperature = item.CurrentLeakOrTemperature;


            var t =
               Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId]
               as Sr.EquipmentInfoHolding.Model.Wj9001Leak;
            if (t == null)
                return;
            if (t.WjLeakLines.ContainsKey(LeakLineId) == false) return;
            LeakLineName = t.WjLeakLines[LeakLineId].LineName;
        }
    }
}
