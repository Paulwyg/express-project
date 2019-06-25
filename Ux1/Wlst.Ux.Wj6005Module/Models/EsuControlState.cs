using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj6005Module.Models
{
    public class EsuControlState
    {

        private static Dictionary<int, string> _esucontrolstate;

        /// <summary>
        /// 1-19
        /// </summary>
        public static Dictionary<int, string> EsuControlStates
        {
            get
            {
                if (_esucontrolstate == null)
                {
                    _esucontrolstate = new Dictionary<int, string>();
                    _esucontrolstate.Add(1, "关灯");
                    _esucontrolstate.Add(2, "开灯");
                    _esucontrolstate.Add(3, "开灯，设备预热");
                    _esucontrolstate.Add(4, "正常照明");
                    _esucontrolstate.Add(5, "稳压照明-节能照明");
                    _esucontrolstate.Add(6, "节能照明，调压过程中");
                    _esucontrolstate.Add(7, "过热旁路");
                    _esucontrolstate.Add(8, "设备过热，退出节能[恢复电压]过程中");
                    _esucontrolstate.Add(9, "过热旁边恢复调压控制中");
                    _esucontrolstate.Add(10, "过压旁路");
                    _esucontrolstate.Add(11, "过压旁路，电压正常后，恢复调压控制中");
                    _esucontrolstate.Add(12, "欠压旁路");
                    _esucontrolstate.Add(13, "欠压旁路，电压正常后，恢复调压控制中");
                    _esucontrolstate.Add(14, "过流旁路");
                    _esucontrolstate.Add(15, "过流旁路，电压正常后，恢复调压控制中");
                    _esucontrolstate.Add(16, "瞬间过流旁路");
                    _esucontrolstate.Add(17, "瞬间过流旁路，电压正常后，恢复调压控制中");
                    _esucontrolstate.Add(18, "工作时，输出电压=0，设备旁路");
                    _esucontrolstate.Add(19, "变压器节能控制接触器状态错[接触器状态与预定控制状态不符]");
                }
                return _esucontrolstate;
            }
        }



        private static Dictionary<int, string> _esuerrorstate;

        /// <summary>
        /// 地址节 低-BIT-0 = 0;低-BIT-7 = 7 ;高-BIT-0 = 8;  高-BIT-7 = 15；
        /// </summary>
        public static Dictionary<int, string> EsuErrorStates
        {
            get
            {
                if (_esuerrorstate == null)
                {
                    _esuerrorstate = new Dictionary<int, string>();
                    _esuerrorstate.Add(0, "调压故障[IGBT]");
                    _esuerrorstate.Add(1, "过热故障");
                    _esuerrorstate.Add(2, "过压故障");
                    _esuerrorstate.Add(3, "欠压故障");
                    _esuerrorstate.Add(4, "过载故障");
                    _esuerrorstate.Add(5, "接触器故障");
                    _esuerrorstate.Add(6, "输出欠压故障");
                    _esuerrorstate.Add(7, "保留");


                    _esuerrorstate.Add(8, "读取内部存储器故障");
                    _esuerrorstate.Add(9, "保留");
                    _esuerrorstate.Add(10, "I2C写故障[PCF8574]");
                    _esuerrorstate.Add(11, "I2C读故障[PC8574]");
                    _esuerrorstate.Add(12, "IGBT温度传感器故障[IGBT]");
                    _esuerrorstate.Add(13, "节电控制器温度传感器故障");
                    _esuerrorstate.Add(14, "写外部存储器故障[CAT1161]");
                    _esuerrorstate.Add(15, "读外部存储器故障[CAT1161]");
                }
                return _esucontrolstate;
            }
        }
    }
}
