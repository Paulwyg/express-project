using System;
using System.Collections.Generic;
using System.Linq;
using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts
{
    /// <summary>
    /// 开关量输出
    /// </summary>
    [Serializable]
    public class SwitchOut
    {
        [Serializable]
        public class RtuParaSwitchOut:IISwitchOutLoop 
        {
            /// <summary>
            /// 构造函数为空；数据均为赋值；使用默认值
            /// </summary>
            public RtuParaSwitchOut()
            {
            }
            /// <summary>
            /// 采用输入参数来实现数据赋值
            /// </summary>
            /// <param name="rtuParaSwitchOut"></param>
            public RtuParaSwitchOut(IISwitchOutLoop rtuParaSwitchOut)
            {
                this.RtuId = rtuParaSwitchOut.RtuId;
                this.SwitchOutId = rtuParaSwitchOut.SwitchOutId;
                this.SwichtOutName = rtuParaSwitchOut.SwichtOutName;
                this.ControlGroup = rtuParaSwitchOut.ControlGroup;
                this.Vector = rtuParaSwitchOut.Vector;
                this.LoopSum = rtuParaSwitchOut.LoopSum;
            }

            /// <summary>
            /// 终端序号
            /// </summary>
            public int RtuId { get; set; }

            /// <summary>
            /// 开关量输出序号
            /// </summary>
            public int SwitchOutId { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string SwichtOutName { get; set; }

            /// <summary>
            /// 归属于哪个分组控制
            /// </summary>
            public int ControlGroup { get; set; }

            /// <summary>
            /// 口矢参数
            /// </summary>
            public int Vector { get; set; }

            /// <summary>
            /// 回路路数
            /// </summary>
            public int LoopSum { get; set; }
        };

        public int RtuId;
        /// <summary>
        /// 程序原始数据 程序内不允许任何人使用；使用请使用RtuParaSwitchOut this[int switchOutId]或GetAllRtuParaSwitchOut；
        /// 公用方法仅提供给协议解析使用
        /// </summary>
        public Dictionary<int, SwitchOut.RtuParaSwitchOut> DicRtuParaSwitchOut = new Dictionary<int, SwitchOut.RtuParaSwitchOut>();

        public SwitchOut()
        {
            this.RtuId = 0;
        }

        public SwitchOut(int rtuId)
        {
            this.RtuId = rtuId;
        }

        /// <summary>
        /// 获取指定开关量
        /// </summary>
        /// <param name="switchOutId">开关量输入序号</param>
        /// <returns></returns>
        public SwitchOut.RtuParaSwitchOut this[int switchOutId]
        {
            get
            {
                if (DicRtuParaSwitchOut.ContainsKey(switchOutId))
                {
                    return DicRtuParaSwitchOut[switchOutId];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取所有开关量 升序排序
        /// </summary>
        public List<SwitchOut.RtuParaSwitchOut> GetAllRtuParaSwitchOut()
        {
            var lstReturn = new List<SwitchOut.RtuParaSwitchOut>();
            var result = from pair in DicRtuParaSwitchOut orderby pair.Key select pair;
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
        /// <param name="rtuParaSwitchOut">开关量数据</param>
        public void AddRtuParaSwitchOut(SwitchOut.RtuParaSwitchOut rtuParaSwitchOut)
        {
            UpdateRtuParaSwitchOut(rtuParaSwitchOut);
        }

        /// <summary>
        /// 更新开关量 无则增加
        /// </summary>
        /// <param name="rtuParaSwitchOut">开关量数据</param>
        public void UpdateRtuParaSwitchOut(SwitchOut.RtuParaSwitchOut rtuParaSwitchOut)
        {
            if (rtuParaSwitchOut.RtuId != this.RtuId)
            {
                return;
            }
            var r = new SwitchOut.RtuParaSwitchOut(rtuParaSwitchOut);

            if (DicRtuParaSwitchOut.ContainsKey(r.SwitchOutId))
            {
                DicRtuParaSwitchOut[r.SwitchOutId] = r;
            }
            else
            {
                DicRtuParaSwitchOut.Add(r.SwitchOutId, r);
            }
        }

        /// <summary>
        /// 删除开关量
        /// </summary>
        /// <param name="switchInId">开关量序号</param>
        public void DeleteRtuParaSwitchOut(int switchInId)
        {
            if (DicRtuParaSwitchOut.ContainsKey(switchInId))
            {
                DicRtuParaSwitchOut.Remove(switchInId);
            }
        }

        /// <summary>
        /// 删除开关量 all
        /// </summary>
        public void Clear()
        {
            DicRtuParaSwitchOut.Clear();
        }


    }
}
