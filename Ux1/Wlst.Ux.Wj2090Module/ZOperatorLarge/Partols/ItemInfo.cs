using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.ViewModel;

namespace Wlst.Ux.Wj2090Module.ZOperatorLarge.Partols
{
    public class DataSluItemInfo : DataSluItem
    {

        public DataSluItemInfo(int index, int sluId)
            : base(index)
        {
            SluId = sluId;
            SampleTime = DateTime.Now;
            RunState = "--";
            AlarmState = "--";
            PowerOnState = "--";
            CommunicationState = "--";
            ParameterState = "--";
            HardwareState = "--";
            UnkownControlNum = 0;
            ResetNum = "--";
        }
        public DataSluItemInfo(Wlst.client.SluCtrlDataMeasureReply.DataSluCon t, int index)
            : base(t, index)
        {
            
        }

        public void SetData(Wlst.client.SluCtrlDataMeasureReply.DataSluCon t)
        {
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

    }


    public class DataCtrlItemInfo : DataCtrlItem
    {
        public DataCtrlItemInfo(int sluId, int ctrId, int orderId, int index)
            : base(sluId, ctrId, orderId, index)
        {
            SampleTime = "--";
            DateTimeCtrl = "--";
            Temperature = 0;
            Status = "--";
            IsAdjust = "--";
            IsWorkingArgsSet = "--";
            IsNoAlarm = "--";
            IsCtrlStop = "--";
            IsEepromError = "--";
            IsTemperatureSensor = "--";
        }

        public DataCtrlItemInfo(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrl tt, int index)
            : base(tt, index)
        {

        }

        public void SetData(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrl tmp)
        {
            SampleTime = new DateTime(tmp.DateCreate).ToString("yyyy-MM-dd HH:mm:ss");
            OrderId = tmp.OrderId;
            DateTimeCtrl = new DateTime(tmp.DateTimeCtrl).ToString("yyyy-MM-dd HH:mm:ss");
            Temperature = tmp.Temperature;

            Status = tmp.Status == 0
                         ? "正常"
                         : tmp.Status == 1
                               ? "电压越上限"
                               : tmp.Status == 2
                                     ? "电压越下限"
                                     : "通讯故障";

            if (tmp.Status == 3)
            {
                IsAdjust = "--";
                IsWorkingArgsSet = "--";
                IsNoAlarm = "--";
                IsCtrlStop = "--";
                IsEepromError = "--";
                IsTemperatureSensor = "--";
                SampleTime = "--";
            }
            else
            {
                IsAdjust = tmp.IsAdjust ? "已校准" : "未校准";
                IsWorkingArgsSet = tmp.IsWorkingArgsSet ? "已设置" : "未设置";
                IsNoAlarm = tmp.IsNoAlarm ? "禁止" : "允许";
                IsCtrlStop = tmp.IsCtrlStop ? "停运" : "正常";
                IsEepromError = tmp.IsEepromError ? "故障" : "正常";
                IsTemperatureSensor = tmp.IsTemperatureSensor ? "故障" : "正常";
            }

        }

    }


    public class DataLampItemInfo : DataLampItem
    {
        public DataLampItemInfo(int sluId, int ctrId, int lampId, int index)
            : base(sluId, ctrId, lampId, index)
        {
            SampleTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            V = "0.00";
            A = "0.00";
            ActivePower = "0.00";
            Electricity = "0.00";
            IntControlStatus = 0;
            IntLightStatus = 0;
            LeakageStatus = "--";
            IntPowerStatus = 0;
            DateCreate = "--";
            IsCtrlStop = "--";

        }

        //public DataLampItemInfo(DataSluCtrlLampEx tt, int index)
        //    : base(tt, index)
        //{
        //    DateCreate  = new DateTime(tt.DateCreate ).ToString("yyyy-MM-dd HH:mm:ss");
        //    IsCtrlStop = tt.IsCtrlStop ? "停运" : "正常";
        //}

        public DataLampItemInfo(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlLamp    tt, int index)
            : base(tt, index)
        {
            DateCreate = "--";
            IsCtrlStop = "--";
        }

        //public string SlIsCtrlStop;

        ///// <summary>
        ///// 停运 0-正常，1-停运
        ///// </summary>

        //public string IsCtrlStop
        //{
        //    get { return SlIsCtrlStop; }
        //    set
        //    {
        //        if (SlIsCtrlStop == value) return;
        //        SlIsCtrlStop = value;
        //        RaisePropertyChanged(() => IsCtrlStop);
        //    }
        //}

        //private string _saDateCreatempleTime;
        //public string DateCreate
        //{
        //    get { return _saDateCreatempleTime; }
        //    set
        //    {
        //        if (_saDateCreatempleTime == value) return;
        //        _saDateCreatempleTime = value;
        //        RaisePropertyChanged(() => DateCreate);
        //    }
        //}
    }
}
