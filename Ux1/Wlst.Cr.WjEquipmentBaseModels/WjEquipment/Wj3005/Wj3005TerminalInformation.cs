using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Wlst.Cr.WjEquipmentBaseModels.Interface;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts;

namespace Wlst.Cr.WjEquipmentBaseModels.WjEquipment.Wj3005
{
    /// <summary>
    /// 为WJ3005终端模型，只能做为主设备
    /// <para> 支持6路输出，16路输入，36路采样，最多36路回路</para>
    /// <para>可扩展附属设备</para>
    /// </summary>
    [Serializable]
    public partial class Wj3005TerminalInformation : TerminalInformationBase
    {
        /// <summary>
        /// 默认状态下 3005 为16路输入  如果为 3090 则为6路
        /// </summary>
        public  int CountSwitchIn = 16;
        /// <summary>
        /// 默认状态下 3005 为6路输出  如果为 3090 则为4路
        /// </summary>
        public  int CountSwitchOut = 6;
        /// <summary>
        /// 默认状态下 3005 为36路回路  如果为 3090 则为16路
        /// </summary>
        public  int CountAmpLoops = 36;
        /// <summary>
        /// 默认状态下 3005 为36路模拟量  如果为 3090 则为16路
        /// </summary>
        public  int CountVectorSample = 36;


        //public override int RtuModel
        //{
        //    get { return 3005; }
        //    set { }
        //}

        public Wj3005TerminalInformation()
            : base()
        {
           // SwitchIn = new SwitchIn(0);
            SwitchOut = new SwitchOut(0);
            RtuParaAnalogueAmps = new RtuParaAnalogueAmps(0);


            Alarm = true;
            Boot = true;
            Call = true;
            Display = true;
            //EnumDeviceType = Wlst.client.EnumDeviceType.Rtu;
            ErrorDelays = 15;
            HeartBeatPeriod = 60;
            Ip = "";
            LowerLimit = 170;
            Md5 = DateTime.Now.Ticks;
            OtherArgu = null;
            PhyId = 0;
            Port = 0;
            Priority = 1;
            Range = 300;
            Report = true;
            ReportDataPeriod = 60;
            Route = true;
            RtuId = 0;
            RtuModel = 3005;
            RtuName = "3005";
            RtuState = 0;
            RtuVoltageName = "Vo";
            Selfcheck = true;
            ServiceProvider = "--";
            SimNumber = "--";
            Sound = true;
            UpperLimit = 300;
            //Xgis = 0;
            Xmap = 0;
           // Ygis = 0;
            Ymap = 0;

        }

        /// <summary>
        /// 终端参数构造函数
        /// </summary>
        /// <param name="rtuId">终端地址</param>
        /// <param name="rtuName">终端名称</param>
        /// <param name="state">终端工作状态</param>
        public Wj3005TerminalInformation(int rtuId, string rtuName, int state)
            : base(rtuId, rtuName, state)
        {
            //SwitchIn = new SwitchIn(rtuId);
            SwitchOut = new SwitchOut(rtuId);
            RtuParaAnalogueAmps = new RtuParaAnalogueAmps(rtuId);


            Alarm = true;
            Boot = true;
            Call = true;
            Display = true;
           // EnumDeviceType =Wlst.client .EnumDeviceType.Rtu;
            ErrorDelays = 15;
            HeartBeatPeriod = 60;
            Ip = "";
            LowerLimit = 170;
            Md5 = DateTime.Now.Ticks;
            OtherArgu = null;
            PhyId = 0;
            Port = 0;
            Priority = 1;
            Range = 300;
            Report = true;
            ReportDataPeriod = 60;
            Route = true;
            RtuId = rtuId;
            RtuModel = 3005;
            RtuName = rtuName;
            RtuState = 0;
            RtuVoltageName = "Vo";
            Selfcheck = true;
            ServiceProvider = "--";
            SimNumber = "--";
            Sound = true;
            UpperLimit = 300;
            //Xgis = 0;
            Xmap = 0;
            //Ygis = 0;
            Ymap = 0;
        }

        public Wj3005TerminalInformation(IIEquipmentInfo baseTerminalInfomation)
            : base(baseTerminalInfomation)
        {
            //SwitchIn = new SwitchIn(baseTerminalInfomation.RtuId);
            SwitchOut = new SwitchOut(baseTerminalInfomation.RtuId);
            RtuParaAnalogueAmps = new RtuParaAnalogueAmps(baseTerminalInfomation.RtuId);
        }


    }

}