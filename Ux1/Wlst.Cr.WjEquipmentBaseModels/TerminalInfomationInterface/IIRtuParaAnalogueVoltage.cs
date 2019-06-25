namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    /// <summary>
    /// 终端电压参数
    /// </summary>
    public interface IIRtuParaAnalogueVoltage
    {
        //public RtuParaAnalogueVoltage()
        //{
        //    this.RtuId = 0;
        //}

        //public RtuParaAnalogueVoltage(int rtuId)
        //{
        //    this.RtuId = rtuId;
        //}

        //public RtuParaAnalogueVoltage(RtuParaAnalogueVoltage rtuParaAnalogueVoltage)
        //{
        //    this.LowerLimit = rtuParaAnalogueVoltage.LowerLimit;
        //    this.Range = rtuParaAnalogueVoltage.Range;
        //    this.RtuId = rtuParaAnalogueVoltage.RtuId;
        //    this.RtuVoltageName = rtuParaAnalogueVoltage.RtuVoltageName;
        //    this.UpperLimit = rtuParaAnalogueVoltage.UpperLimit;
        //}

        int RtuId { get; set; }

        /// <summary>
        /// 电压参数 显示名称
        /// </summary>
        string RtuVoltageName { get; set; }

        /// <summary>
        /// 电压参数 量程
        /// </summary>
        int Range { get; set; }

        /// <summary>
        /// 电压参数 报警上限
        /// </summary>
        int UpperLimit { get; set; }

        /// <summary>
        /// 电压参数 报警下限
        /// </summary>
        int LowerLimit { get; set; }

        /// <summary>
        /// 是否开关量输入状态有电流来判断 >0.3  由于其他参悟无法放入故放入电压参数中
        /// </summary>
         bool IsSwitchinputJudgebyA { get; set; }

         /// <summary>
         /// 是否启用屏蔽小电流
         /// </summary>
          bool IsShieldLittleA { get; set; }

         /// <summary>
         /// 屏蔽值
         /// </summary>
          double AShield { get; set; }
    }
}
