using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.client;

namespace Wlst.Sr.EquipmentInfoHolding.Model
{
    [Serializable]
    public class WjParaBase : EquipmentParameter
    {
        public virtual string GetLoopName(int loopid)
        {
            return string.Empty;
        }
      
        /// <summary>
        /// 设备归属类型
        /// </summary>
        public EquType EquipmentType;



        /// <summary>
        /// 连接到本设备的其他设备列表
        /// </summary>
        public List<int> EquipmentsThatAttachToThisRtu;

        public WjParaBase(EquipmentParameter basepara)
        {
            //lvf 2019年4月17日11:18:27  一直为null 报错
            if (basepara == null) return;


            //this.AreaId = basepara.AreaId;
            this.DateCreate = basepara.DateCreate;
            this.DateUpdate = basepara.DateUpdate;
            this.RtuArgu = basepara.RtuArgu;
            this.RtuFid = basepara.RtuFid;
            this.RtuGisX = basepara.RtuGisX;
            this.RtuGisY = basepara.RtuGisY;
            this.RtuId = basepara.RtuId;
            this.RtuInstallAddr = basepara.RtuInstallAddr;
            this.RtuMapX = basepara.RtuMapX;
            this.RtuMapY = basepara.RtuMapY;
            this.RtuModel = basepara.RtuModel;
            this.RtuName = basepara.RtuName;
            this.RtuStateCode = basepara.RtuStateCode;
            this.RtuRemark = basepara.RtuRemark;
            this.RtuPhyId = basepara.RtuPhyId;
            this.Idf = basepara.Idf;
            this.RtuRealState = basepara.RtuRealState;
            //添加地区 lvf 2019年5月6日09:20:23
            this.RtuRegion = basepara.RtuRegion;
            this.Imei = basepara.Imei;


            EquipmentsThatAttachToThisRtu = new List<int>();

            if (this.RtuId < 1000000) this.EquipmentType = EquType.UnKnown;
            else if (this.RtuId < 1099999) this.EquipmentType = EquType.Rtu;
            else if (this.RtuId < 1199999) this.EquipmentType = EquType.Ldu;
            else if (this.RtuId < 1299999) this.EquipmentType = EquType.Esu;
            else if (this.RtuId < 1399999) this.EquipmentType = EquType.Mru;
            else if (this.RtuId < 1499999) this.EquipmentType = EquType.Lux;
            else if (this.RtuId < 1599999) this.EquipmentType = EquType.Slu;
            else if (this.RtuId < 1699999) this.EquipmentType = EquType.Leak;
            else this.EquipmentType = EquType.UnKnown;
        }


        public enum EquType
        {
            UnKnown = 0,

            /// <summary>
            /// 1 000 000 -1 099 999
            /// </summary>
            Rtu=3005,

            /// <summary>
            /// 1 400 000 - 1 499 999
            /// </summary>
            Lux=1080,

            /// <summary>
            /// 1 100 000 - 1 199 999
            /// </summary>
            Ldu=1090,

            /// <summary>
            /// 1 300 000 - 1 399 999
            /// </summary>
            Mru=1050,

            /// <summary>
            /// 1 500 000 - 1 599 999
            /// </summary>
            Slu=2090,

            /// <summary>
            /// 1 200 000 - 1 299 999
            /// </summary>
            Esu=601,

            /// <summary>
            /// 1 600 000-1 699 999 漏电
            /// </summary>
            Leak=9001,
        }




    }

    [Serializable]
    public class Wj3005Rtu : WjParaBase
    {
        /// <summary>
        /// 开关量输出最大路数
        /// </summary>
        public int SwitchOutCount;

        /// <summary>
        /// 电压参数
        /// </summary>
        public RtuVoltageParameter WjVoltage { get; private set; }

        /// <summary>
        /// gprs参数
        /// </summary>
        public RtuGprsParameter WjGprs { get; private set; }

        /// <summary>
        /// 回路参数
        /// </summary>
        public Dictionary<int, RtuAnalogParameter> WjLoops { get; private set; }

        public override string GetLoopName(int loopid)
        {
            if (WjLoops.ContainsKey(loopid)) return WjLoops[loopid].LoopName;
            return base.GetLoopName(loopid);
        }

        ///// <summary>
        ///// 输入参数
        ///// </summary>
        //public Dictionary<int, RtuSwitchInputParameter> WjSwitchIns { get; private set; }

        /// <summary>
        /// 输出参数
        /// </summary>
        public Dictionary<int, RtuSwitchOutputParameter> WjSwitchOuts { get; private set; }

        public Wj3005Rtu(EquipmentParameter basepara, RtuVoltageParameter voltage, RtuGprsParameter gprs,
                         List<RtuAnalogParameter> loops, 
                         List<RtuSwitchOutputParameter> switchout)
            : base(basepara)
        {
            WjVoltage = voltage;
            WjLoops = new Dictionary<int, RtuAnalogParameter>();
            //WjSwitchIns = new Dictionary<int, RtuSwitchInputParameter>();
            WjSwitchOuts = new Dictionary<int, RtuSwitchOutputParameter>();
            WjGprs = gprs;

            foreach (var f in loops)
                if (WjLoops.ContainsKey(f.LoopId) == false) WjLoops.Add(f.LoopId, f);

            //foreach (var f in switchin)
            //    if (WjSwitchIns.ContainsKey(f.SwitchId) == false)
            //        WjSwitchIns.Add(f.SwitchId, f);
            foreach (var f in switchout)
                //if (WjSwitchOuts.ContainsKey(f.SwitchId) == false)                //青岛开发区需求 可以重复switchID
                    WjSwitchOuts.Add(f.SwitchId, f);

            if (this.RtuModel == EnumRtuModel.Wj3005) SwitchOutCount = 6;
            else if (this.RtuModel == EnumRtuModel.Wj3090) SwitchOutCount = 4;
            else if (this.RtuModel == EnumRtuModel.Wj3006) SwitchOutCount = 8;
            else SwitchOutCount = 6;
        }
    }

    [Serializable]
    public class Wj1080Lux : WjParaBase
    {
        public LuxParameter WjLux { get; private set; }

        public Wj1080Lux(EquipmentParameter basepara, LuxParameter lux)
            : base(basepara)
        {
            WjLux = lux;
        }
    }

    [Serializable]
    public class Wj1050Mru : WjParaBase
    {
        public MruParameter WjMru { get; private set; }

        public Wj1050Mru(EquipmentParameter basepara, MruParameter mru)
            : base(basepara)
        {
            WjMru = mru;
        }
    }

    [Serializable]
    public class Wj601Esu : WjParaBase
    {
        public EsuParameter WjEsu { get; private set; }

        public Wj601Esu(EquipmentParameter basepara, EsuParameter esu)
            : base(basepara)
        {
            WjEsu = esu;
        }
    }

    [Serializable]
    public class Wj2090Slu : WjParaBase
    {

        public override string GetLoopName(int loopid)
        {
            if (WjSluCtrls.ContainsKey(loopid)) return WjSluCtrls[loopid].LampCode;
            return base.GetLoopName(loopid);
        }



        public SluParameter WjSlu { get; private set; }

        /// <summary>
        /// 控制器参数
        /// </summary>
        public Dictionary<int, SluRegulatorParameter> WjSluCtrls { get; private set; }

        public Dictionary<int, Wlst.client.SluRegulatorGroupParameter> WjSluCtrlGrps { get; private set; }

        public Wj2090Slu(EquipmentParameter basepara, SluParameter slu, List<SluRegulatorParameter> ctrls,
                         List<SluRegulatorGroupParameter> ctrlGrps)
            : base(basepara)
        {
            WjSlu = slu;
            WjSluCtrls = new Dictionary<int, SluRegulatorParameter>();
            foreach (var f in ctrls)
            {
                if (!WjSluCtrls.ContainsKey(f.CtrlId)) WjSluCtrls.Add(f.CtrlId, f);
            }
            WjSluCtrlGrps = new Dictionary<int, SluRegulatorGroupParameter>();
            foreach (var f in ctrlGrps)
            {
                if (!WjSluCtrlGrps.ContainsKey(f.GrpId)) WjSluCtrlGrps.Add(f.GrpId, f);
            }
        }
    }

    [Serializable]
    public class Wj1090Ldu : WjParaBase
    {
        public override string GetLoopName(int loopid)
        {
            if (WjLduLines.ContainsKey(loopid)) return WjLduLines[loopid].LduLineName ;
            return base.GetLoopName(loopid);
        }
        /// <summary>
        /// 防盗回路采纳数
        /// </summary>
        public Dictionary<int, LduLineParameter> WjLduLines { get; private set; }

        public Wj1090Ldu(EquipmentParameter basepara, List<LduLineParameter> ctrls)
            : base(basepara)
        {
            WjLduLines = new Dictionary<int, LduLineParameter>();
            foreach (var f in ctrls)
            {
                if (!WjLduLines.ContainsKey(f.LduLineId)) WjLduLines.Add(f.LduLineId, f);
            }
        }
    }


    [Serializable]
    public class Wj9001Leak : WjParaBase
    {
        public override string GetLoopName(int loopid)
        {
            if (WjLeakLines.ContainsKey(loopid)) return WjLeakLines[loopid].LineName;
            return base.GetLoopName(loopid);
        }
        /// <summary>
        /// 防盗回路采纳数
        /// </summary>
        public Dictionary<int, LeakParameter> WjLeakLines { get; private set; }

        public Wj9001Leak(EquipmentParameter basepara, List<LeakParameter> leakLines)
            : base(basepara)
        {
            WjLeakLines = new Dictionary<int, LeakParameter>();
            foreach (var f in leakLines)
            {
                if (!WjLeakLines.ContainsKey(f.LeakLineId)) WjLeakLines.Add(f.LeakLineId, f);
            }
        }
    }
}
