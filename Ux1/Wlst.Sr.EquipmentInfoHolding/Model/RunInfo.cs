using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.Sr.EquipmentInfoHolding.Services;
using Wlst.client;

namespace Wlst.Sr.EquipmentInfoHolding.Model
{
    public class RunInfo
    {
        public RunInfo (int rtuId)
        {
            this.RtuId = rtuId;

        }
        public int RtuId;

        private bool isOnLine;

        /// <summary>
        /// 设备是否在线
        /// </summary>
        public bool IsOnLine
        {
            get { return isOnLine; }
            set
            {
                isOnLine = value;
                //IsNewdata = false;
            }
        }


        private bool _iIsLightHasElectric;
        /// <summary>
        /// 终端是否有电流
        /// </summary>
        public bool IsLightHasElectric  {
            get { return _iIsLightHasElectric; }
            set
            {
                _iIsLightHasElectric = value;
                //IsNewdata = false;
            }
        }

        private int _iErrorCount;
        /// <summary>
        /// 该终端是否包含故障
        /// </summary>
        public int ErrorCount
        {
            get { return _iErrorCount; }
            set
            {
                _iErrorCount = value;
               // IsNewdata = false;
            }
        }


        /// <summary>
        /// 1、全开，2、全关，3、未知
        /// </summary>
        public  int RtuOcStates = 0;

        public  long RtuOcStatesChangedtime = 0; 
        /// <summary>
        /// 终端最新数据
        /// </summary>
        public RtuNewDataInfo RtuNewData = null;

        /// <summary>
        /// 单灯集中器最新数据
        /// </summary>
        public SluMeasureInfo SluNewData = null;

      //  public bool IsNewdata = false;

        /// <summary>
        /// 单灯集中器下的控制器最新数据
        /// </summary>
        public ConcurrentDictionary<int, CtrlMeasureInfo> SluCtrlNewData =
            new ConcurrentDictionary<int, CtrlMeasureInfo>();

        /// <summary>
        /// 单灯集中器下的控制器 图标状态  控制器地址，灯头-通信异常-故障代码-开关灯状态
        /// </summary>
        public ConcurrentDictionary<int, CtrlIconInfo> SluCtrlIconStates =
    new ConcurrentDictionary<int, CtrlIconInfo>();       //todo  tobecontinue


        /// <summary>
        /// 漏电设备下的线路数据
        /// </summary>
        public ConcurrentDictionary<int, LeakNewData.LeakNewDataItem> LeakLineNewData =
            new ConcurrentDictionary<int, LeakNewData.LeakNewDataItem>(); 

        /// <summary>
        /// 线路最新数据
        /// </summary>
        public ConcurrentDictionary<int, LduNewData> LduLinesNewData = new ConcurrentDictionary<int, LduNewData>();


        public void AddCtrlNewState(int ctrlid,CtrlIconInfo ctrlState)
        {
            // IsNewdata = true;
            if (!SluCtrlIconStates.ContainsKey(ctrlid)) SluCtrlIconStates.TryAdd(ctrlid, new CtrlIconInfo());
            if (SluCtrlIconStates[ctrlid] == null) SluCtrlIconStates[ctrlid ] = new CtrlIconInfo();
            SluCtrlIconStates[ctrlid].Errors = ctrlState.Errors;
            SluCtrlIconStates[ctrlid].IsIconUseRtuStateTo = ctrlState.IsIconUseRtuStateTo;
            SluCtrlIconStates[ctrlid].RtuState = ctrlState.RtuState;
            SluCtrlIconStates[ctrlid].UnConnected = ctrlState.UnConnected;
            SluCtrlIconStates[ctrlid].states = ctrlState.states;
        }


        public  void AddRtuNewData(Wlst.client.TmlNewData tmlNewData)
        {
            // IsNewdata = true;
            this.RtuNewData = new RtuNewDataInfo(tmlNewData);
            RtuOcStates = tmlNewData.IsAllSwitchOpen;

            //if(tmlNewData.IsAllSwitchOpen==RtuOcStates)
            //{
            //    RtuOcStatesChangedtime = RtuNewData.DateCreate.Ticks;
            //}
            //else if(RtuNewData.IsAllSwitchOpen != 3)
            //{
               
            //    RtuOcStatesChangedtime = RtuNewData.DateCreate.Ticks;



            //    var args = new PublishEventArgs()
            //    {
            //        EventType = PublishEventType.Core,
            //        EventId =  EventIdAssign.MapNeedChangeIcon ,
            //    };
            //    var rtus = RtuNewData.RtuId;
            //    args.AddParams(rtus);
            //    EventPublish.PublishEvent(args);
            //}
            ////RtuOcStates = 3;
            ////bool allOpen = true;
            ////var currentSum = RtuNewData.RtuCurrentSumA + RtuNewData.RtuCurrentSumB + RtuNewData.RtuCurrentSumC;
            ////if (currentSum == 0)
            ////{
            ////    RtuOcStates = 1;
            ////    RtuOcStatesChangedtime = RtuNewData.DateCreate.Ticks;
            ////}
            ////var switchOutOnOff = RtuNewData.IsSwitchOutAttraction;
            ////var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(RtuId);
            ////for (int i = 1; i < 9; i++)
            ////{
            ////    var timeTable =
            ////                    Wlst.Sr.TimeTableSystem.Services.WeekTimeTableInfoService.
            ////                        GetTmlLoopBandTimeTableTodayOpenCloseTimex(areaId,
            ////                                                                   RtuId, i);
            ////    if (timeTable != null)
            ////    {
            ////        if (switchOutOnOff.Count > i)
            ////        {
            ////            if (!switchOutOnOff[i - 1]) allOpen = false;
            ////        }
            ////    }

            ////}
            ////if (allOpen == true)//判断 绑定时间表的输出开启 并有电流
            ////{
            ////    RtuOcStates = 2;
            ////    RtuOcStatesChangedtime = RtuNewData.DateCreate.Ticks;
            ////}
            

        }

        internal void AddSluNewData(Wlst.client.SluCtrlDataMeasureReply.DataSluCon info)
        {
          //  IsNewdata = true;
            if (SluNewData == null) SluNewData = new SluMeasureInfo(RtuId);
            SluNewData.SluData = info;
            SluNewData.LastUpdate = 1; // = info;
            SluNewData.LastUpdateTime = DateTime.Now.Ticks;
        }

        internal void AddSluNewData(List<Wlst.client.SluCtrlDataMeasureReply.UnknowCtrl> info)
        {
            //IsNewdata = true;
            if (SluNewData == null) SluNewData = new SluMeasureInfo(RtuId);
            SluNewData.DataUnknown = info;
            SluNewData.LastUpdate = 2; // = info;InfoCtrl[tukey].LastUpdateTime  = DateTime .Now .Ticks ;
            SluNewData.LastUpdateTime = DateTime.Now.Ticks;
        }

        internal void AddLeakNewData(Wlst.client.LeakNewData data)
        {
            foreach (var f in data.Items)
            {
                if (!LeakLineNewData.ContainsKey(f.LeakLineId))
                    LeakLineNewData.TryAdd(f.LeakLineId, f);
                else LeakLineNewData[f.LeakLineId] = f;
            }
        }

        internal  void AddSluCtrlNewData(int ctrlId, Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData info)
        {
            //IsNewdata = true;
            if (!SluCtrlNewData.ContainsKey(ctrlId)) SluCtrlNewData.TryAdd(ctrlId, new CtrlMeasureInfo(RtuId, ctrlId));
            SluCtrlNewData[ctrlId].Data5 = info;
            SluCtrlNewData[ctrlId].LastUpdate = 5;
            SluCtrlNewData[ctrlId].LastUpdateTime = info.Info.DateTimeCtrl;//DateTime.Now.Ticks;
        }

        internal void AddLduNewData(LduLineData info)
        {
            //IsNewdata = true;
            var tmp = new LduNewData(info);
            if (LduLinesNewData.ContainsKey(tmp.LineLoopId)) LduLinesNewData[tmp.LineLoopId] = tmp;
            else LduLinesNewData.TryAdd(tmp.LineLoopId, tmp);
        }
    }

    #region rtu new data

        /// <summary>
    /// 终端设备最新数据；与服务器交互数据
    /// </summary>
    public class RtuNewDataInfo
    {
        /// <summary>
        /// 终端地址
        /// </summary>
        public int RtuId;

        public string RtuName;

        /// <summary>
        /// 电压
        /// </summary>
        public double RtuVoltageA;

        /// <summary>
        /// 电压
        /// </summary>
        public double RtuVoltageB;

        /// <summary>
        /// 电压
        /// </summary>
        public double RtuVoltageC;

        /// <summary>
        /// 电流
        /// </summary>
        public double RtuCurrentSumA;

        /// <summary>
        /// 电流
        /// </summary>
        public double RtuCurrentSumB;

        /// <summary>
        /// 电流
        /// </summary>
        public double RtuCurrentSumC;

        /// <summary>
        /// 数据发生时间
        /// </summary>
        public DateTime DateCreate;

        /// <summary>
        /// 3006设备温度
        /// </summary>
        public int RtuTemperature;

        /// <summary>
        /// 24小时内巡测返回次数   历史数据此项无效
        /// </summary>
        public int TimesBackPartolIn24Hour;

        /// <summary>
        /// 24小时内巡测次数   历史数据此项无效
        /// </summary>
        public int TimesPartolIn24Hour;

        /// <summary>
        /// 1 全开  2全关  3 未知
        /// </summary>
        public int IsAllSwitchOpen;
        /// <summary>
        /// 回路最新数据
        /// 其回路信息存放结构为 NewDataforOneLoop
        /// </summary>
        public List<RtuNewDataLoopItem> LstNewLoopsData;

        /// <summary>
        /// 开关量输出状态数据 是否每个开关量输出吸合连接 0~
        /// </summary>
        public List<bool> IsSwitchOutAttraction;
        /// <summary>
        /// 终端数据携带的报警，1 供电报警，2 开机申请，3 停运，4 报警位报警，5 电压超限制，6 电流超限，7 无电流报警
        /// </summary>
        public Dictionary<int, bool> Alarms;

            public int AlarmCount;
        ///// <summary>
        ///// 构造函数；需要知道终端地址
        ///// </summary>
        //public RtuNewDataInfo()
        //{
        //    RtuId = 0;
        //    LstNewLoopsData = new List<RtuNewDataLoopItem>();
        //    IsSwitchOutAttraction = new List<bool>();
        //    Alarms = new Dictionary<int, bool>();
        //}

            public RtuNewDataInfo(Wlst.client.TmlNewData tmlNewData)
            {
                this.DateCreate = new DateTime(tmlNewData.DateCreate);
                this.RtuCurrentSumA = tmlNewData.RtuCurrentSumA;
                this.RtuCurrentSumB = tmlNewData.RtuCurrentSumB;
                this.RtuCurrentSumC = tmlNewData.RtuCurrentSumC;
                this.RtuId = tmlNewData.RtuId;
                this.RtuVoltageA = tmlNewData.RtuVoltageA;
                this.RtuVoltageB = tmlNewData.RtuVoltageB;
                this.RtuVoltageC = tmlNewData.RtuVoltageC;
                this.RtuTemperature = tmlNewData.RtuTemperature;
                this.TimesBackPartolIn24Hour = tmlNewData.TimesBackPartolIn24Hour;
                this.TimesPartolIn24Hour = tmlNewData.TimesPartolIn24Hour;
                this.IsAllSwitchOpen = tmlNewData.IsAllSwitchOpen;
                

                LstNewLoopsData = new List<RtuNewDataLoopItem>();
                IsSwitchOutAttraction = new List<bool>();
                this.Alarms = new Dictionary<int, bool>();
                
                if (tmlNewData.IsSwitchOutAttraction != null)
                    foreach (var t in tmlNewData.IsSwitchOutAttraction) this.IsSwitchOutAttraction.Add(t);
                if (tmlNewData.Alarms != null)
                {
                    foreach (var t in tmlNewData.Alarms) if (!this.Alarms.ContainsKey(t)) this.Alarms.Add(t, true);
                    this.AlarmCount = tmlNewData.Alarms.Count;
                }



                if (tmlNewData.LstNewLoopsData.Count == 0) return;

                var data = Services.EquipmentDataInfoHold.GetInfoById(RtuId);
                if (data == null) return;
                var amps = data as Wj3005Rtu;
                if (amps == null) return;

                UpdateLoopsInfo(amps, tmlNewData.LstNewLoopsData);



            }


        #region Help Mothed

        private void UpdateLoopsInfo(Wj3005Rtu amp,
 List<Wlst.client.TmlNewData.TmlNewDataforOneLoop> loopsData)
        {
            this.LstNewLoopsData.Clear();
            if (amp == null) return;
            if (amp.WjLoops  == null) return;


            foreach (var f in loopsData)
            {
                foreach (var t in amp.WjLoops .Values )
                {
                    if (t.LoopId   == f.LoopId)
                    {

                        var loopsInfo = new RtuNewDataLoopItem();
                        loopsInfo.LoopId = t.LoopId;
                        loopsInfo.LoopName = t.LoopName;

                        loopsInfo.V = f.V;
                        loopsInfo.BrightRate = f.BrightRate;
                        loopsInfo.Power = Math.Round(f.Power, 2);

                        loopsInfo.PowerFactor =  Math.Round(f.PowerFactor, 5);
                        loopsInfo.A = f.A;
                        loopsInfo.AoverRange = f.AoverRange;
                        loopsInfo.VoverRange = f.VoverRange;
                        loopsInfo.ShieldLittleA = t.ShieldLittleA;

                        loopsInfo.Range = t.CurrentRange;
                        loopsInfo.Lower = t.CurrentAlarmLowerlimit ;
                        loopsInfo.Upper = t.CurrentAlarmUpperlimit ;
                        loopsInfo.IsLoop = t.SwitchOutputId  > 0;

                        loopsInfo.SwitchOutId = t.SwitchOutputId ;
                        loopsInfo.BolSwitchInState = f.SwitchInState;
                        loopsInfo.AvgOf7daysA = f.AvgOf7daysA;

                        this.LstNewLoopsData.Add(loopsInfo);

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="switchInStateNormal"></param>
        /// <returns></returns>
        private string GetSwitchInState(int switchInStateNormal)
        {
            switch (switchInStateNormal)
            {
                case 0:
                    return "断";

                case 1:
                    return "通";

                case 2:
                    return "正常";

                case 3:
                    return "被盗";

                case 4:
                    return "打开";

                case 5:
                    return "关闭";

            }
            return "Reserve";
        }

        #endregion
    };


    /// <summary>
    /// 回路最新数据
    /// </summary>
    public class RtuNewDataLoopItem
    {
        public int LoopId;

        public string LoopName;

        /// <summary>
        /// 电压
        /// </summary>
        public double  V;

        /// <summary>
        /// 电流
        /// </summary>
        public double A;

        /// <summary>
        /// 功率
        /// </summary>
        public double  Power;

        /// <summary>
        /// 功率因数
        /// </summary>
        public double PowerFactor;

        /// <summary>
        /// 亮灯率
        /// </summary>
        public double BrightRate;

        ///// <summary>
        ///// 开关量输入状态  true 吸合
        ///// </summary>
        //public string SwitchInState;

        /// <summary>
        /// 输入状态 true 吸合
        /// </summary>
        public bool  BolSwitchInState;
        /// <summary>
        /// 下限
        /// </summary>
        public int Lower;
        /// <summary>
        /// 上限
        /// </summary>
        public int Upper;
        /// <summary>
        /// 量程
        /// </summary>
        public int Range;
        /// <summary>
        /// 是否为回路
        /// </summary>
        public bool IsLoop;

        /// <summary>
        /// 输出编号
        /// </summary>
        public int SwitchOutId;

        /// <summary>
        /// //0 正常，1 下限 2 上限 3 量程
        /// </summary>
        public int AoverRange;

        /// <summary>
        /// //0 正常，1 下限 2 上限 3 量程
        /// </summary>
        public int VoverRange;

        /// <summary>
        /// 参考电流   历史数据此项无效
        /// </summary>
        public double AvgOf7daysA;

        /// <summary>
        /// 屏蔽小电流值
        /// </summary>
        public double ShieldLittleA;
    }

    #endregion

    #region slu new data


    public class CtrlMeasureInfo
    {
        public Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData Data5;
        public Wlst.client.SluCtrlDataMeasureReply.CtrlPhyinfo DataPhy4;
        public Wlst.client.SluCtrlDataMeasureReply.AssistCtrlData DataAss6;
        public int SluId;
        public int CtrlId;
        public long LastUpdateTime;

        /// <summary>
        /// 最后更新的数据是 4 物理信息，5 控制器数据，6 控制器辅助数据
        /// </summary>
        public int LastUpdate;

        public CtrlMeasureInfo(int sluId, int ctrlId)
        {
            SluId = sluId;
            CtrlId = ctrlId;
            Data5 = null;
            DataPhy4 = null;
            DataAss6 = null;
        }
    }

    public class SluMeasureInfo
    {
        public int SluId;
        public List<Wlst.client.SluCtrlDataMeasureReply.UnknowCtrl> DataUnknown;
        public Wlst.client.SluCtrlDataMeasureReply.DataSluCon SluData;
        public long LastUpdateTime;

        /// <summary>
        /// 最后更新的数据是 2 未知控制器，1 集中器数据
        /// </summary>
        public int LastUpdate;

        public SluMeasureInfo(int sluId)
        {
            SluId = sluId;
            DataUnknown = new List<Wlst.client.SluCtrlDataMeasureReply.UnknowCtrl>();
            SluData = null;
        }
    }

    /// <summary>
    ///   lampInfo   key:lampId   value:LampError - lampOnOff
    /// </summary>

    public class CtrlIconInfo
    {
        /// <summary>
        /// 是否使用终端 状态作为判断标准
        /// </summary>
        public bool IsIconUseRtuStateTo=false ;
        /// <summary>
        /// 1、开灯，2、关灯  查阅该控制器故障  结合故障
        /// </summary>
        public int RtuState = 0;

        public bool UnConnected;

        /// <summary>
        /// 0-正常亮灯，1-一档节能，2-二档节能，3-关灯
        /// </summary>
        public int states;

        public List<int> Errors = new List<int>(); 


        ////1
        //public int AllOpen;
        ////1
        //public int AllClose;

        //public Dictionary<int, int> States = new Dictionary<int, int>();
        //public Dictionary<int, List<int>> Errors = new Dictionary<int, List<int>>(); 

        //public ConcurrentDictionary<int, Tuple<int, int>> LampInfo;
        //public CtrlIconInfo(bool unConnected, ConcurrentDictionary<int, Tuple<int, int>> lampInfo)
        //{
        //    UnConnected = unConnected;
        //    LampInfo = lampInfo;
        //}
    }

    #endregion

    #region ldu line new data

     /// <summary>
    /// 线路检测最新数据  集中器 回路
    /// </summary>
    public class LduNewData
    {
        /// <summary>
        /// 连接的主设备地址
        /// </summary>
        public int AttachRtuId;

        /// <summary>
        /// 集中控制器地址
        /// </summary>
        public int RtuId;
        /// <summary>
        /// 
        /// </summary>
        public string RtuName;
        /// <summary>
        /// 数据接收时间
        /// </summary>
        public DateTime DateCreate;

        /// <summary>
        /// 回路地址 1-6
        /// </summary>
        public int LineLoopId; //回路标识，二进制转十进制

        public double V; //回路1电压
        public double A; //回路1电流

        /// <summary>
        /// 回路1有功功率
        /// </summary>
        public double PowerActive; //回路1有功功率

        /// <summary>
        /// 回路1无功功率
        /// </summary>
        public double PowerReActive; //回路1无功功率

        /// <summary>
        /// 回路1功率因数
        /// </summary>
        public double PowerFactor; //回路1功率因数

        /// <summary>
        /// 回路1亮灯率
        /// </summary>
        public double BrightRate; //回路1亮灯率

        /// <summary>
        /// 回路1信号强度 脉冲
        /// </summary>
        public int Single; //回路1信号强度

        /// <summary>
        /// 回路1阻抗
        /// </summary>
        public int Impedance; //回路1阻抗

        /// <summary>
        /// 回路1 12s有用信号数量  阻抗数
        /// </summary>
        public int NumofUsefullSingleIn12Sec; //回路1 12s有用信号数量

        /// <summary>
        /// 回路1 12s信号数量 跳数
        /// </summary>
        public int NumofSingleIn12Sec; //回路1 12s信号数量


        /// <summary>
        /// 回路1检测标识 故障参数
        /// </summary>
        public int FlagDetection; //回路1检测标识
        /// <summary>
        /// 回路1报警标识  故障数据
        /// </summary>
        public int FlagAlarm; //回路1报警标识


        public LduNewData( LduLineData info)
        {
            this.A = info.A;
            this.V = info.V;
            this.RtuId = info.LduId ;
            this.DateCreate = new DateTime( info.DateCreate);

            var dh =Services .EquipmentDataInfoHold .GetInfoById( info.LduId);
            if (dh != null && dh.RtuFid  != 0)
            {
                RtuName = dh.RtuName;
                AttachRtuId = dh.RtuFid ;
            }
           // this.AttachRtuId = info.AttachRtuId;
            this.BrightRate = info.BrightRate;

            Impedance = info.Impedance;
            LineLoopId = info.LineId ;
            NumofSingleIn12Sec = info.NumofSingleIn12Sec;
            NumofUsefullSingleIn12Sec = info.NumofUsefullSingleIn12Sec;
            PowerFactor = info.PowerFactor;
            PowerActive = info.PowerActive;
            PowerReActive = info.PowerReActive;
            Single = info.Single;
            this.FlagAlarm = info.FlagAlarm;
            this.FlagDetection = info.FlagDetection;
            this.SetAlarmLine(info.FlagAlarm, info.FlagDetection);


            //if (Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
            //    EquipmentInfoDictionary.ContainsKey(RtuId))
            //{

            //    this.RtuName = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.
            //        EquipmentInfoDictionary[RtuId].RtuName;
            //}

        }

        ///// <summary>
        ///// 回路1检测标识 故障参数
        ///// </summary>
        //public int FlagDetection; //回路1检测标识

        ///// <summary>
        ///// 回路1报警标识  故障数据
        ///// </summary>
        //public int FlagAlarm; //回路1报警标识


        //faultsss.Add(41, new Tuple<string, string>("开灯脉冲告警", "开灯信号强度告警"));
        //faultsss.Add(42, new Tuple<string, string>("开灯阻抗主动告警", "开灯阻抗主动告警"));
        //faultsss.Add(43, new Tuple<string, string>("亮灯率变化告警", "亮灯率变化主动告警"));
        //faultsss.Add(44, new Tuple<string, string>("供电变化告警", "线路失电主动告警"));
        //faultsss.Add(45, new Tuple<string, string>("关灯脉冲告警", "关灯信号主动告警"));
        //faultsss.Add(46, new Tuple<string, string>("关灯阻抗告警", "关灯阻抗主动告警"));
        //faultsss.Add(47, new Tuple<string, string>("线路短路告警", "线路短路主动告警"));
        //faultsss.Add(48, new Tuple<string, string>("主动告警", "线路主动告警"));

        /// <summary>
        /// 线路被盗
        /// </summary>
        public bool IsStolen { get; set; }

        /// <summary>
        /// 线路短路
        /// </summary>
        public bool IsShortCircuit { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="flagAlarm"></param>
        /// <param name="flagDetection"></param>
        private void SetAlarmLine(int flagAlarm, int flagDetection)
        {


            //var newErrors = new List<int>();

            if ((flagDetection >> 3 & 1) == 1) //1 关灯 0 开灯
            {
                if ((flagAlarm >> 6 & 1) == 1 && (flagDetection >> 6 & 1) == 1)
                    IsShortCircuit = true;

                if (((flagAlarm >> 4 & 1) == 1 && (flagDetection >> 4 & 1) == 1) ||
                    ((flagAlarm >> 5 & 1) == 1 && (flagDetection >> 5 & 1) == 1))
                    IsStolen = true;
            }
            else
            {
                if (((flagAlarm & 1) == 1 && (flagDetection & 1) == 1) ||
                    ((flagAlarm >> 1 & 1) == 1 && (flagDetection >> 1 & 1) == 1) ||
                    ((flagAlarm >> 2 & 1) == 1 && (flagDetection >> 2 & 1) == 1))
                    IsStolen = true;
            }

        }
    }

#endregion
}
