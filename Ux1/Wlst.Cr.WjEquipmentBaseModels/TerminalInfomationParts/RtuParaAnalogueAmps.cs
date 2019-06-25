using System;
using System.Collections.Generic;
using System.Linq;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts
{
    /// <summary>
    /// 回路数据
    /// </summary>
    [Serializable]
    public class RtuParaAnalogueAmps
    {
        [Serializable]
        public class RtuParaAnalogueAmp:IIRtuParaAnalogueAmpLoop 
        {
            /// <summary>
            /// 构造函数为空；数据均为赋值；使用默认值
            /// </summary>
            public RtuParaAnalogueAmp()
            {
            }


            /// <summary>
            /// 采用输入参数来实现数据赋值
            /// </summary>
            /// <param name="rtuParaAnalogueAmp"></param>
            public RtuParaAnalogueAmp(IIRtuParaAnalogueAmpLoop rtuParaAnalogueAmp)
            {
                this.RtuId = rtuParaAnalogueAmp.RtuId;
                this.LoopId = rtuParaAnalogueAmp.LoopId;
                this.LoopName = rtuParaAnalogueAmp.LoopName;
                this.Range = rtuParaAnalogueAmp.Range;
                this.UpperLimit = rtuParaAnalogueAmp.UpperLimit;
                this.LowerLimit = rtuParaAnalogueAmp.LowerLimit;
                this.VectorMoniliang = rtuParaAnalogueAmp.VectorMoniliang;
                this.Phase = rtuParaAnalogueAmp.Phase;
                this.LightRate = rtuParaAnalogueAmp.LightRate;
                this.LightRateLowerLimit = rtuParaAnalogueAmp.LightRateLowerLimit;
                this.VectorSwitchIn = rtuParaAnalogueAmp.VectorSwitchIn;
                this.SwitchOutId = rtuParaAnalogueAmp.SwitchOutId;
                this.AmRange = rtuParaAnalogueAmp.AmRange;
                this.IsAlarmSwitch = rtuParaAnalogueAmp.IsAlarmSwitch;
                this.IsSwitchStateClose = rtuParaAnalogueAmp.IsSwitchStateClose;
            }

            /// <summary>
            /// 终端序号
            /// </summary>
            public int RtuId { get; set; }

            /// <summary>
            /// 回路序号
            /// </summary>
            public int LoopId { get; set; }

            /// <summary>
            /// 回路名称
            /// </summary>
            public string LoopName { get; set; }

            /// <summary>
            /// 量程/互感器比值
            /// </summary>
            public int Range { get; set; }


            /// <summary>
            /// 电流量程
            /// </summary>
           public  int AmRange { get; set; }

            /// <summary>
            /// 报警上限
            /// </summary>
            public int UpperLimit { get; set; }

            /// <summary>
            /// 报警下限
            /// </summary>
            public int LowerLimit { get; set; }

            /// <summary>
            /// 口矢参数 模拟量口失参数
            /// </summary>
            public int  VectorMoniliang { get; set; }

            /// <summary>
            /// 电压相位
            /// </summary>
            public int Phase { get; set; }

            /// <summary>
            /// 亮灯率计算
            /// </summary>
            public double  LightRate { get; set; }

            /// <summary>
            /// 亮灯率报警下限
            /// </summary>
            public int LightRateLowerLimit { get; set; }

            ///// <summary>
            ///// 关联开关量输入信号
            ///// </summary>
            //public int SwitchInId { get; set; }

            /// <summary>
            /// 关联开关量输出信号
            /// </summary>
            public int SwitchOutId { get; set; }


            /// <summary>
            /// 跳变报警 开关量跳变报警
            /// </summary>
            public bool IsAlarmSwitch { get; set; }

            /// <summary>
            /// 口矢参数  开关量口失参数
            /// </summary>
            public int  VectorSwitchIn { get; set; }

            /// <summary>
            /// 开关量输入 是否为常闭状态
            /// </summary>
            public bool IsSwitchStateClose { get; set; }
        };


        public int RtuId;
        /// <summary>
        /// 程序原始数据 程序内不允许任何人使用；使用请使用RtuParaAnalogueAmp this[int loopId]或GetAllRtuParaAnalogueAmps；
        /// 公用方法仅提供给协议解析使用
        /// </summary>
        public Dictionary<int, RtuParaAnalogueAmps.RtuParaAnalogueAmp> DicRtuParaAnalogueAmp = new Dictionary<int, RtuParaAnalogueAmps.RtuParaAnalogueAmp>();

        /// <summary>
        /// 不允许使用此方法，仅提供给协议使用
        /// </summary>
        public RtuParaAnalogueAmps ()
        {
            this.RtuId = 0;
        }

        public RtuParaAnalogueAmps(int rtuId)
        {
            this.RtuId = rtuId;
        }

        /// <summary>
        /// 获取指定回路数据
        /// </summary>
        /// <param name="loopId"> </param>
        /// <returns></returns>
        public RtuParaAnalogueAmps.RtuParaAnalogueAmp this[int loopId]
        {
            get
            {
                if (DicRtuParaAnalogueAmp.ContainsKey(loopId))
                {
                    return DicRtuParaAnalogueAmp[loopId];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取所有回路数据量 升序排序
        /// </summary>
        public List<RtuParaAnalogueAmps.RtuParaAnalogueAmp> GetAllRtuParaAnalogueAmps()
        {

            var lstReturn = new List<RtuParaAnalogueAmps.RtuParaAnalogueAmp>();
            var result = from pair in DicRtuParaAnalogueAmp orderby pair.Key select pair;
            foreach (var p in result)
            {
                lstReturn.Add(p.Value);
            }

            //var list = _dicTmlInfo.OrderBy(d => d.Key);
            //foreach (var p in list) lstReturn.Add(p.Value);

            return lstReturn;

        }

        /// <summary>
        /// 增加回路数据 有则更新
        /// </summary>
        /// <param name="rtuParaAnalogueAmp">回路数据</param>
        public void AddRtuParaAnalogueAmp(RtuParaAnalogueAmps.RtuParaAnalogueAmp rtuParaAnalogueAmp)
        {
            UpdateRtuParaAnalogueAmp(rtuParaAnalogueAmp);
        }

        /// <summary>
        /// 更新回路数据 无则增加
        /// </summary>
        /// <param name="rtuParaAnalogueAmp">回路数据</param>
        public void UpdateRtuParaAnalogueAmp(RtuParaAnalogueAmps.RtuParaAnalogueAmp rtuParaAnalogueAmp)
        {
            if (rtuParaAnalogueAmp.RtuId != this.RtuId)
            {
                return;
            }
            var r = new RtuParaAnalogueAmps.RtuParaAnalogueAmp(rtuParaAnalogueAmp);

            if (DicRtuParaAnalogueAmp.ContainsKey(r.LoopId))
            {
                DicRtuParaAnalogueAmp[r.LoopId] = r;
            }
            else
            {
                DicRtuParaAnalogueAmp.Add(r.LoopId, r);
            }
        }

        /// <summary>
        /// 删除回路数据
        /// </summary>
        /// <param name="loopId">回路序号</param>
        public void DeleteRtuParaAnalogueAmp(int loopId)
        {
            if (DicRtuParaAnalogueAmp.ContainsKey(loopId))
            {
                DicRtuParaAnalogueAmp.Remove(loopId);
            }
        }

        /// <summary>
        /// 删除回路数据 all
        /// </summary>
        public void Clear()
        {
            DicRtuParaAnalogueAmp.Clear();
        }

    }
}
