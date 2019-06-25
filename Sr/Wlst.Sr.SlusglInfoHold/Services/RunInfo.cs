using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Wlst.Cr.Core.EventHandlerHelper;
using Wlst.client;

namespace Wlst.Sr.SlusglInfoHold.Services
{
    public class RunInfo
    {
        public RunInfo(int rtuId)
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
        public bool IsLightHasElectric
        {
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
        /// 1、全关，2、全开，3、未知
        /// </summary>
        public int RtuOcStates = 0;

        public long RtuOcStatesChangedtime = 0;


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


       


        public void AddCtrlNewState(int ctrlid, CtrlIconInfo ctrlState)
        {
            // IsNewdata = true;
            if (!SluCtrlIconStates.ContainsKey(ctrlid)) SluCtrlIconStates.TryAdd(ctrlid, new CtrlIconInfo());
            if (SluCtrlIconStates[ctrlid] == null) SluCtrlIconStates[ctrlid] = new CtrlIconInfo();
            SluCtrlIconStates[ctrlid].Errors = ctrlState.Errors;
            SluCtrlIconStates[ctrlid].IsIconUseRtuStateTo = ctrlState.IsIconUseRtuStateTo;
            SluCtrlIconStates[ctrlid].RtuState = ctrlState.RtuState;
            SluCtrlIconStates[ctrlid].UnConnected = ctrlState.UnConnected;
            SluCtrlIconStates[ctrlid].states = ctrlState.states;
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

   
        internal void AddSluCtrlNewData(int ctrlId, Wlst.client.SluCtrlDataMeasureReply.DataSluCtrlData info)
        {
            //IsNewdata = true;
            if (!SluCtrlNewData.ContainsKey(ctrlId)) SluCtrlNewData.TryAdd(ctrlId, new CtrlMeasureInfo(RtuId, ctrlId));
            SluCtrlNewData[ctrlId].Data5 = info;
            SluCtrlNewData[ctrlId].LastUpdate = 5;
            SluCtrlNewData[ctrlId].LastUpdateTime = DateTime.Now.Ticks;
        }

       
    }



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
        public bool IsIconUseRtuStateTo = false;
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


}
