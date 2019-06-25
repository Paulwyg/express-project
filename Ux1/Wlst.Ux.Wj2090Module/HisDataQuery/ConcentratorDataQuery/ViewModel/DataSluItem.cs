using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.ViewModel
{
    public class DataSluItem:Wlst.Cr.Core.CoreServices.ObservableObject
    {
        public DataSluItem(int index)
        {
            Index = index;
        }
        public DataSluItem(Wlst.client.SluCtrlDataMeasureReply.DataSluCon t, int index)
        {
            Index = index;
            SluId = t.RtuId ;

            

            SampleTime = new DateTime(t.DateCreate);
            RunState = t.IsSluStop == false ? "正常" : "停运";
            AlarmState = t.IsEnableAlarm ? "允许主报" : "禁止主报";
            PowerOnState = t.IsPowerOn ? "开机申请" : "非开机申请";
            CommunicationState = t.IsGprs ? "GPRS通信" : "485通信";
            ParameterState = (!t.IsConcentratorArgsError && !t.IsCtrlArgsError)
                                 ? "正常"
                                 : (t.IsConcentratorArgsError ? "集中器参数错误;" : "")
                                   + (t.IsCtrlArgsError ? "控制器参数错误" : "");
            HardwareState = (!t.IsZigbeeError && !t.IsCarrierError && !t.IsFramError && !t.IsBluetoothError &&
                             !t.IsTimerError)
                                ? "正常"
                                : (t.IsZigbeeError ? "Zigbee模块出错;" : "")
                                  + (t.IsCarrierError ? "电力载波模块出错;" : "")
                                  + (t.IsFramError ? "FRAM出错;" : "")
                                  + (t.IsBluetoothError ? "蓝牙模块出错;" : "")
                                  + (t.IsTimerError ? "硬件时钟出错;" : "");
            UnkownControlNum = t.UnknowCtrlCount;
            ResetNum = "今天:" + t.Rest0 + ";昨天:" + t.Rest1 + ";前天:" + t.Rest2 + ";大前天:" + t.Rest3;
            ZgbCommunication = t.CommunicationChannel;
        }
        #region SluId

        private int   _indeSluIdx;
        public int SluId
        {
            get { return _indeSluIdx; }
            set
            {
                if (_indeSluIdx.Equals(value)) return;
                _indeSluIdx = value;
                RaisePropertyChanged(() => SluId);
                var mtpr = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(value);
                if(mtpr !=null )
                SluShowId = mtpr .RtuPhyId  .ToString("D4");
               
            }
        }

        private string  _ssdfSluId;

        public string  SluShowId
        {
            get { return _ssdfSluId; }
            set
            {
                if (value != _ssdfSluId)
                {
                    _ssdfSluId = value;
                    this.RaisePropertyChanged(() => this.SluShowId);
                }
            }
        }

        #endregion
        #region index

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                if(_index.Equals(value)) return;
                _index = value;
                RaisePropertyChanged(()=>Index);
            }
        }

        #endregion

        #region SampleTime

        private DateTime _sampleTime;
        public DateTime SampleTime
        {
            get { return _sampleTime; }
            set
            {
                if(_sampleTime==value) return;
                _sampleTime = value;
                RaisePropertyChanged(()=>SampleTime);
            }
        }

        #endregion

        #region RunState

        private string _runState;
        public string RunState
        {
            get { return _runState; }
            set
            {
                if (_runState == value) return;
                _runState = value;
                RaisePropertyChanged(() => RunState);
            }
        }

        #endregion

        #region AlarmState

        private string _alarmState;
        public string AlarmState
        {
            get { return _alarmState; }
            set
            {
                if (_alarmState == value) return;
                _alarmState = value;
                RaisePropertyChanged(() => AlarmState);
            }
        }

        #endregion

        #region PowerOnState

        private string _powerOnState;
        public string PowerOnState
        {
            get { return _powerOnState; }
            set
            {
                if (_powerOnState == value) return;
                _powerOnState = value;
                RaisePropertyChanged(() => PowerOnState);
            }
        }

        #endregion

        #region CommunicationState

        private string _communicationState;
        public string CommunicationState
        {
            get { return _communicationState; }
            set
            {
                if (_communicationState == value) return;
                _communicationState = value;
                RaisePropertyChanged(() => CommunicationState);
            }
        }

        #endregion

        #region ParameterState

        private string _parameterState;
        public string ParameterState
        {
            get { return _parameterState; }
            set
            {
                if (_parameterState == value) return;
                _parameterState = value;
                RaisePropertyChanged(() => ParameterState);
            }
        }

        #endregion

        #region HardwareState

        private string _hardwareState;
        public string HardwareState
        {
            get { return _hardwareState; }
            set
            {
                if (_hardwareState == value) return;
                _hardwareState = value;
                RaisePropertyChanged(() => HardwareState);
            }
        }

        #endregion

        #region UnkownControlNum

        private int _unkownControlNum;
        public int UnkownControlNum
        {
            get { return _unkownControlNum; }
            set
            {
                if (_unkownControlNum == value) return;
                _unkownControlNum = value;
                RaisePropertyChanged(() => UnkownControlNum);
            }
        }

        #endregion

        #region ResetNum

        private string _resetNum;
        public string ResetNum
        {
            get { return _resetNum; }
            set
            {
                if (_resetNum == value) return;
                _resetNum = value;
                RaisePropertyChanged(() => ResetNum);
            }
        }

        #endregion

        #region ZgbCommunication

        private int _zgbCommunication;
        public int ZgbCommunication
        {
            get { return _zgbCommunication; }
            set
            {
                if (_zgbCommunication == value) return;
                _zgbCommunication = value;
                RaisePropertyChanged(() => ZgbCommunication);
            }
        }

        #endregion
    }
}
