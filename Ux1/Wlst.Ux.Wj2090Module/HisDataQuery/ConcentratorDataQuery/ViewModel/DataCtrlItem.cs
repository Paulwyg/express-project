using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2090Module.HisDataQuery.ConcentratorDataQuery.ViewModel
{
   public  class DataCtrlItem :Wlst .Cr .Core .CoreServices .ObservableObject 
    {
       public DataCtrlItem(int sluId, int ctrId, int orderId, int index)
       {
           Index = index;
           if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(sluId ))
           SluId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( sluId).RtuPhyId  .ToString("D4");
           ControlId = Wj2090Module.Services.CommonSlu.GetPhyIdByCtrl(sluId,ctrId ); ; OrderId = orderId;
           ControlName = Wj2090Module.Services.CommonSlu.GetNameByCtrl(sluId, ctrId);
       }
       public DataCtrlItem(Wlst.client.SluCtrlDataMeasureReply.DataSluCtrl tmp, int index)
       {
           Index = index;
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
           IsAdjust = tmp.IsAdjust ? "已校准" : "未校准";
           IsWorkingArgsSet = tmp.IsWorkingArgsSet ? "已设置" : "未设置";
           IsNoAlarm = tmp.IsNoAlarm ? "禁止" : "允许";
           IsCtrlStop = tmp.IsCtrlStop ? "停运" : "正常";
           IsEepromError = tmp.IsEepromError ? "故障" : "正常";
           IsTemperatureSensor = tmp.IsTemperatureSensor ? "故障" : "正常";
           if (Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(tmp .SluId ))
           SluId = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( tmp.SluId).RtuPhyId  .ToString("D4");
           ControlId =Wj2090Module .Services .CommonSlu .GetPhyIdByCtrl( tmp .SluId ,tmp.CtrlId);
           ControlName = Wj2090Module.Services.CommonSlu.GetNameByCtrl(tmp.SluId, tmp.CtrlId);
       }

       #region Index

       private int _index;
       public int Index
       {
           get { return _index; }
           set
           {
               if (_index == value) return;
               _index = value;
               RaisePropertyChanged(() => Index);
           }
       }


   
       private string  _indeSluIdx;
       public string  SluId
       {
           get { return _indeSluIdx; }
           set
           {
               if (_indeSluIdx==value) return;
               _indeSluIdx = value;
               RaisePropertyChanged(() => SluId);

               //var infos = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.GetEquipmentInfo(value);
               //if (infos != null)
               //{
               //    if (infos.AttachRtuId == 0)
               //    {
               //        SluShowId = string.Format("{0:D7}", infos.PhyId);
               //    }
               //    else SluShowId = infos.RtuId + "";
               //}
           }
       }

       private string _ssdfSluId;

       public string SluShowId
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

       #region ControlId
       private int _controlId;
       public int ControlId
       {
           get { return _controlId; }
           set
           {
               if (_controlId == value) return;
               _controlId = value;
               RaisePropertyChanged(() => ControlId);
           }
       }
       #endregion

       //lvf 2018年4月23日16:16:40 列改成控制器名称
       #region ControlName
       private string _controlName;
       public string ControlName
       {
           get { return _controlName; }
           set
           {
               if (_controlName == value) return;
               _controlName = value;
               RaisePropertyChanged(() => ControlName);
           }
       }
       #endregion


       #region SampleTime
       private string  _sampleTime;
       public string  SampleTime
       {
           get { return _sampleTime; }
           set
           {
               if (_sampleTime == value) return;
               _sampleTime = value;
               RaisePropertyChanged(() => SampleTime);
           }
       }
       #endregion
        public int SluIOrderIdd;

        /// <summary>
        /// 控制器地址 排序地址
        /// </summary>

        public int OrderId
        {
            get { return SluIOrderIdd; }
            set
            {
                if (SluIOrderIdd == value) return;
                SluIOrderIdd = value;
                RaisePropertyChanged(() => OrderId);
            }
        }

        public string SluDateTimeCtrl;

        /// <summary>
        /// 日 时:分  数据在控制器中生成的时间
        /// </summary>

        public string DateTimeCtrl
        {
            get { return SluDateTimeCtrl; }
            set
            {
                if (SluDateTimeCtrl == value) return;
                SluDateTimeCtrl = value;
                RaisePropertyChanged(() => DateTimeCtrl);
            }
        }

        public int SluIsdTemperature;

        /// <summary>
        /// 温度
        /// </summary>

        public int Temperature
        {
            get { return SluIsdTemperature; }
            set
            {
                if (SluIsdTemperature == value) return;
                SluIsdTemperature = value;
                RaisePropertyChanged(() => Temperature);
            }
        }

        public string SluStatus;

        /// <summary>
        /// 状态 0-正常，1-电压越上限，2-电压越下限，3-通讯故障
        /// </summary>

        public string Status
        {
            get { return SluStatus; }
            set
            {
                if (SluStatus == value) return;
                SluStatus = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public string SluIsAdjust;

        /// <summary>
        /// 已校准 0-未校准，1-已校准
        /// </summary>

        public string IsAdjust
        {
            get { return SluIsAdjust; }
            set
            {
                if (SluIsAdjust == value) return;
                SluIsAdjust = value;
                RaisePropertyChanged(() => IsAdjust);
            }
        }

        public string SlIsWorkingArgsSet;

        /// <summary>
        /// 工作参数设置 0-未设置，1-已设置
        /// </summary>

        public string IsWorkingArgsSet
        {
            get { return SlIsWorkingArgsSet; }
            set
            {
                if (SlIsWorkingArgsSet == value) return;
                SlIsWorkingArgsSet = value;
                RaisePropertyChanged(() => IsWorkingArgsSet);
            }
        }

        public string SlIsNoAlarm;

        /// <summary>
        /// 禁止主动报警 0-允许，1-禁止
        /// </summary>

        public string IsNoAlarm
        {
            get { return SlIsNoAlarm; }
            set
            {
                if (SlIsNoAlarm == value) return;
                SlIsNoAlarm = value;
                RaisePropertyChanged(() => IsNoAlarm);
            }
        }

        public string SlIsCtrlStop;

        /// <summary>
        /// 停运 0-正常，1-停运
        /// </summary>

        public string IsCtrlStop
        {
            get { return SlIsCtrlStop; }
            set
            {
                if (SlIsCtrlStop == value) return;
                SlIsCtrlStop = value;
                RaisePropertyChanged(() => IsCtrlStop);
            }
        }

        public string SIsEepromError;

        /// <summary>
        /// EEPROM故障 0-正常，1-故障
        /// </summary>

        public string IsEepromError
        {
            get { return SIsEepromError; }
            set
            {
                if (SIsEepromError == value) return;
                SIsEepromError = value;
                RaisePropertyChanged(() => IsEepromError);
            }
        }


        public string SIsTemperatureSensor;

        /// <summary>
        /// 温度传感器故障 0-正常，1-故障
        /// </summary>

        public string IsTemperatureSensor
        {
            get { return SIsTemperatureSensor; }
            set
            {
                if (SIsTemperatureSensor == value) return;
                SIsTemperatureSensor = value;
                RaisePropertyChanged(() => IsTemperatureSensor);
            }
        }

    }
}
