namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    public interface IIRtuParaAnalogueAmpLoop
    {
        /// <summary>
        /// 终端序号
        /// </summary>
        int RtuId { get; set; }

        /// <summary>
        /// 回路序号
        /// </summary>
        int LoopId { get; set; }

        /// <summary>
        /// 电流量程
        /// </summary>
        int AmRange { get; set; }

        /// <summary>
        /// 回路名称
        /// </summary>
        string LoopName { get; set; }

        /// <summary>
        /// 量程/互感器比值
        /// </summary>
        int Range { get; set; }

        /// <summary>
        /// 报警上限
        /// </summary>
        int UpperLimit { get; set; }

        /// <summary>
        /// 报警下限
        /// </summary>
        int LowerLimit { get; set; }

        /// <summary>
        /// 口矢参数
        /// </summary>
        int VectorMoniliang { get; set; }

        /// <summary>
        /// 电压相位
        /// </summary>
        int Phase { get; set; }

        /// <summary>
        /// 亮灯率计算
        /// </summary>
        double  LightRate { get; set; }

        /// <summary>
        /// 亮灯率报警下限
        /// </summary>
        int LightRateLowerLimit { get; set; }

        ///// <summary>
        ///// 关联开关量输入信号
        ///// </summary>
        //int SwitchInId { get; set; }

        /// <summary>
        /// 关联开关量输出信号
        /// </summary>
        int SwitchOutId { get; set; }



        /// <summary>
        /// 跳变报警 开关量跳变报警
        /// </summary>
        bool IsAlarmSwitch { get; set; }

        /// <summary>
        /// 口矢参数  开关量口失参数
        /// </summary>
         int  VectorSwitchIn { get; set; }

         /// <summary>
         /// 开关量输入 是否为常闭状态
         /// </summary>
          bool IsSwitchStateClose { get; set; }
    }
}
