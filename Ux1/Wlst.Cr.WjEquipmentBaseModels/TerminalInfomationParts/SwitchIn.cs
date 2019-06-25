//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface;

//namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationParts
//{

//    /// <summary>
//    /// 开关量输入
//    /// </summary>
//    [Serializable]
//    public class SwitchIn
//    {
//        [Serializable]
//        public class RtuParaSwitchIn:IISwitchInLoop 
//        {
//            /// <summary>
//            /// 构造函数为空；数据均为赋值；使用默认值
//            /// </summary>
//            public RtuParaSwitchIn()
//            {
//            }


//            /// <summary>
//            /// 采用输入参数来实现数据赋值
//            /// </summary>
//            /// <param name="rtuParaSwitchIn"></param>
//            public RtuParaSwitchIn(IISwitchInLoop rtuParaSwitchIn)
//            {
//                this.Alarm = rtuParaSwitchIn.Alarm;
//                this.ContactorState = rtuParaSwitchIn.ContactorState;
//                this.UnNormalState = rtuParaSwitchIn.UnNormalState;
//                this.NoramlState = rtuParaSwitchIn.NoramlState;
//                this.RtuId = rtuParaSwitchIn.RtuId;
//                this.SwitchInId = rtuParaSwitchIn.SwitchInId;
//                this.SwichtInName = rtuParaSwitchIn.SwichtInName;
//                this.Vector = rtuParaSwitchIn.Vector;
//            }

//            /// <summary>
//            /// 终端序号
//            /// </summary>
//            public int RtuId { get; set; }

//            /// <summary>
//            /// 开关量输入序号
//            /// </summary>
//            public int SwitchInId { get; set; }

//            /// <summary>
//            /// 名称
//            /// </summary>
//            public string SwichtInName { get; set; }

//            /// <summary>
//            /// 接触器状态:常开/常闭/无
//            /// </summary>
//            public byte ContactorState { get; set; }

//            /// <summary>
//            /// 跳变报警
//            /// </summary>
//            public byte Alarm { get; set; }

//            /// <summary>
//            /// 口矢参数
//            /// </summary>
//            public byte Vector { get; set; }

//            /// <summary>
//            /// 状态 正常的显示名称
//            /// </summary>
//            public int NoramlState { get; set; }

//            /// <summary>
//            /// 状态 异常的显示名称
//            /// </summary>
//            public int UnNormalState { get; set; }
//        };

//        public int RtuId;
//        /// <summary>
//        /// 程序原始数据 程序内不允许任何人使用；使用请使用RtuParaSwitchIn this[int switchInId]或GetAllRtuParaSwitchIn；
//        /// 公用方法仅提供给协议解析使用
//        /// </summary>
//        public Dictionary<int, SwitchIn.RtuParaSwitchIn> DicRtuParaSwitchIn = new Dictionary<int, SwitchIn.RtuParaSwitchIn>();

//        /// <summary>
//        /// 不允许使用此方法，仅提供给协议使用
//        /// </summary>
//        public SwitchIn ()
//        {
//            this.RtuId = 0;
//        }

//        public SwitchIn(int rtuId)
//        {
//            this.RtuId = rtuId;
//        }

//        /// <summary>
//        /// 获取指定开关量
//        /// </summary>
//        /// <param name="switchInId">开关量输入序号</param>
//        /// <returns></returns>
//        public SwitchIn.RtuParaSwitchIn this[int switchInId]
//        {
//            get
//            {
//                if (DicRtuParaSwitchIn.ContainsKey(switchInId))
//                {
//                    return DicRtuParaSwitchIn[switchInId];
//                }
//                else
//                {
//                    return null;
//                }
//            }
//        }

//        /// <summary>
//        /// 获取所有开关量 升序排序
//        /// </summary>
//        public List<SwitchIn.RtuParaSwitchIn> GetAllRtuParaSwitchIn()
//        {
//                var lstReturn = new List<SwitchIn.RtuParaSwitchIn>();
//                var result = from pair in DicRtuParaSwitchIn orderby pair.Key select pair;
//                foreach (var p in result)
//                {
//                    lstReturn.Add(p.Value);
//                }

//                //var list = _dicTmlInfo.OrderBy(d => d.Key);
//                //foreach (var p in list) lstReturn.Add(p.Value);

//                return lstReturn;
            
//        }

//        /// <summary>
//        /// 增加开关量 有则更新
//        /// </summary>
//        /// <param name="rtuParaSwitchIn">开关量数据</param>
//        public void AddRtuParaSwitchIn(SwitchIn.RtuParaSwitchIn rtuParaSwitchIn)
//        {
//            UpdateRtuParaSwitchIn(rtuParaSwitchIn);
//        }

//        /// <summary>
//        /// 更新开关量 无则增加
//        /// </summary>
//        /// <param name="rtuParaSwitchIn">开关量数据</param>
//        public void UpdateRtuParaSwitchIn(SwitchIn.RtuParaSwitchIn rtuParaSwitchIn)
//        {
//            if (rtuParaSwitchIn.RtuId != this.RtuId)
//            {
//                return;
//            }
//            var r = new SwitchIn.RtuParaSwitchIn(rtuParaSwitchIn);

//            if (DicRtuParaSwitchIn.ContainsKey(r.SwitchInId))
//            {
//                DicRtuParaSwitchIn[r.SwitchInId] = r;
//            }
//            else
//            {
//                DicRtuParaSwitchIn.Add(r.SwitchInId, r);
//            }
//        }

//        /// <summary>
//        /// 删除开关量
//        /// </summary>
//        /// <param name="switchInId">开关量序号</param>
//        public void DeleteRtuParaSwitchIn(int switchInId)
//        {
//            if (DicRtuParaSwitchIn.ContainsKey(switchInId))
//            {
//                DicRtuParaSwitchIn.Remove(switchInId);
//            }
//        }

//        /// <summary>
//        /// 删除开关量 all
//        /// </summary>
//        public void Clear()
//        {
//            DicRtuParaSwitchIn.Clear();
//        }


//    }
//}
