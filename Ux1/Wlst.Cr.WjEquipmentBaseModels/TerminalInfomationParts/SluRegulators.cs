using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts
{
 
    /// <summary>
    /// 开关量输出
    /// </summary>
    [Serializable]
    public class SluRegulators
    {
        [Serializable]
        public class SluRegulator : IISluRegulatorPara
        {
            /// <summary>
            /// 构造函数为空；数据均为赋值；使用默认值
            /// </summary>
            public SluRegulator()
            {
            }
            /// <summary>
            /// 采用输入参数来实现数据赋值
            /// </summary>
            /// <param name="info"></param>
            public SluRegulator(IISluRegulatorPara info)
            {
                this.CtrlId = info.CtrlId;
                this.BarCodeId = info.BarCodeId;
                this.IsAlarmAuto = info.IsAlarmAuto;
                this.LampCode = info.LampCode;
                this.IsAutoOpenLightWhenElec1 = info.IsAutoOpenLightWhenElec1;
                this.IsAutoOpenLightWhenElec2 = info.IsAutoOpenLightWhenElec2;
                this.IsAutoOpenLightWhenElec3 = info.IsAutoOpenLightWhenElec3;
                this.IsAutoOpenLightWhenElec4 = info.IsAutoOpenLightWhenElec4;

                this.CtrlPhyId = info.CtrlPhyId;
                this.IsUsed = info.IsUsed;
                this.SluId  = info.SluId ;
                this.OrderId = info.OrderId;
                this.RtuName = info.RtuName;
                this.VectorLoop1 = info.VectorLoop1;
                this.VectorLoop2 = info.VectorLoop2;
                this.VectorLoop3 = info.VectorLoop3;
                this.VectorLoop4 = info.VectorLoop4;
                this.PowerRate1 = info.PowerRate1;
                this.PowerRate2 = info.PowerRate2;
                this.PowerRate3 = info.PowerRate3;
                this.PowerRate4 = info.PowerRate4;
                this.LightCount = info.LightCount;
                this.UpperPower = info.UpperPower;
                this.LowerPower = info.LowerPower;
                this.RoutePass1 = info.RoutePass1;
                this.RoutePass2 = info.RoutePass2;
                this.RoutePass3 = info.RoutePass3;
                this.RoutePass4 = info.RoutePass4;
                this.XGis = info.XGis;
                this.YGis = info.YGis;
            }

            /// <summary>
            /// 地址  1-256
            /// </summary>

            public int CtrlId { get; set; }

            /// <summary>
            /// 父节点地址  即集中控制器地址
            /// </summary>

            public int SluId { get; set; }

           public int CtrlPhyId { get; set; }
            /// <summary>
            /// 开灯排序地址
            /// </summary>

            public int OrderId { get; set; }


            /// <summary>
            /// 条形码
            /// </summary>

            public long BarCodeId { get; set; }



            /// <summary>
            /// 设备名称  末端灯号等
            /// </summary>

            public string RtuName { get; set; }


            /// <summary>
            /// 是否停运
            /// </summary>

            public bool IsUsed { get; set; }


            /// <summary>
            /// 是否主动报警
            /// </summary>

            public bool IsAlarmAuto { get; set; }


            /// <summary>
            /// 是否上电后自动开灯
            /// </summary>

            public bool IsAutoOpenLightWhenElec1 { get; set; }
            public bool IsAutoOpenLightWhenElec2 { get; set; }
            public bool IsAutoOpenLightWhenElec3 { get; set; }
            public bool IsAutoOpenLightWhenElec4 { get; set; }

            /// <summary>
            /// 回路1矢量
            /// </summary>

            public int VectorLoop1 { get; set; }


            /// <summary>
            /// 回路2矢量
            /// </summary>

            public int VectorLoop2 { get; set; }


            /// <summary>
            /// 回路3矢量
            /// </summary>

            public int VectorLoop3 { get; set; }


            /// <summary>
            /// 回路4矢量
            /// </summary>

            public int VectorLoop4 { get; set; }


            /// <summary>
            /// 额定功率 0-不设置额定功率；1:0-20；2:21-100；
            /// </summary>
            public int PowerRate1 { get; set; }

            /// <summary>
            /// 额定功率 0-不设置额定功率；1:0-20；2:21-100；
            /// </summary>

            public int PowerRate2 { get; set; }
            /// <summary>
            /// 额定功率 0-不设置额定功率；1:0-20；2:21-100；
            /// </summary>

            public int PowerRate3 { get; set; }
            /// <summary>
            /// 额定功率 0-不设置额定功率；1:0-20；2:21-100；
            /// </summary>

            public int PowerRate4 { get; set; }

            /// <summary>
            /// 灯头数  如1控2则为2 
            /// </summary>

            public int LightCount { get; set; }


            /// <summary>
            /// 功率上限  为额定功率百分比 PowerRate* UpperPower/100
            /// </summary>

            public int UpperPower { get; set; }


            /// <summary>
            /// 功率下限  为额定功率百分比 PowerRate* LowerPower/100
            /// </summary>

            public int LowerPower { get; set; }


            /// <summary>
            /// 路由1
            /// </summary>

            public int RoutePass1 { get; set; }


            /// <summary>
            /// 路由2
            /// </summary>

            public int RoutePass2 { get; set; }


            /// <summary>
            /// 路由3
            /// </summary>

            public int RoutePass3 { get; set; }


            /// <summary>
            /// 路由4
            /// </summary>

            public int RoutePass4 { get; set; }

            public string LampCode { get; set; }

            public double XGis { get; set; }
            public double YGis { get; set; }
        };

        public int RtuId;
        /// <summary>
        /// 程序原始数据 程序内不允许任何人使用；使用请使用RtuParaSwitchOut this[int ctrlId]或GetAllRtuParaSwitchOut；
        /// 公用方法仅提供给协议解析使用
        /// </summary>
        public Dictionary<int, SluRegulator> DicRtuParaSluRegulator = new Dictionary<int, SluRegulator>();

        public Dictionary<int, int> DicSluRegulatorPhyToLogic = new Dictionary<int, int>();

        public SluRegulators()
        {
            this.RtuId = 0;
        }

        public SluRegulators(int rtuId)
        {
            this.RtuId = rtuId;
        }

        /// <summary>
        /// 获取指定控制器
        /// </summary>
        /// <param name="rtuid">。集中器 地址</param>
        /// <returns></returns>
        public SluRegulator this[int rtuid]
        {
            get
            {
                if (DicRtuParaSluRegulator.ContainsKey(rtuid))
                {
                    return DicRtuParaSluRegulator[rtuid];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取所有集中器
        /// </summary>
        public List<SluRegulator> GetAllSluRegulators()
        {
            var lstReturn = new List<SluRegulator>();
            var result = from pair in DicRtuParaSluRegulator orderby pair.Key select pair;
            foreach (var p in result)
            {
                lstReturn.Add(p.Value);
            }

            //var list = _dicTmlInfo.OrderBy(d => d.Key);
            //foreach (var p in list) lstReturn.Add(p.Value);

            return lstReturn;

        }

        /// <summary>
        /// 增加开关量 有则更新
        /// </summary>
        /// <param name="ifo">开关量数据</param>
        public void AddSluRegulator(SluRegulator ifo)
        {
            UpdateSluRegulator(ifo);
        }

        /// <summary>
        /// 更新控制器数据   无则增加
        /// </summary>
        /// <param name="ifo">控制器数据</param>
        public void UpdateSluRegulator(SluRegulator ifo)
        {
            if (ifo.SluId  != this.RtuId)
            {
                return;
            }
            var r = new SluRegulator(ifo);

            if (DicRtuParaSluRegulator.ContainsKey(r.CtrlId))
            {
                DicRtuParaSluRegulator[r.CtrlId] = r;
            }
            else
            {
                DicRtuParaSluRegulator.Add(r.CtrlId, r);
            }

            if (DicSluRegulatorPhyToLogic.ContainsKey(ifo.CtrlPhyId))
                DicSluRegulatorPhyToLogic[ifo.CtrlPhyId] = ifo.CtrlId;
            else DicSluRegulatorPhyToLogic.Add(ifo.CtrlPhyId, ifo.CtrlId);
        }

        /// <summary>
        /// 删除控制器数据
        /// </summary>
        /// <param name="rtuid">控制器地址</param>
        public void DeleteSluRegulator(int rtuid)
        {
            if (DicRtuParaSluRegulator.ContainsKey(rtuid))
            {
                DicRtuParaSluRegulator.Remove(rtuid);
            }

            foreach (var g in DicSluRegulatorPhyToLogic )
            {
                if(g.Value ==rtuid )
                {
                    DicSluRegulatorPhyToLogic.Remove(g.Key);
                    break;
                }
            }
        }

        /// <summary>
        /// 删除控制器 all
        /// </summary>
        public void Clear()
        {
            DicRtuParaSluRegulator.Clear();
            DicSluRegulatorPhyToLogic.Clear();
        }


    }
}
