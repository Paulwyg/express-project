using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    /// <summary>
    /// 单灯控制器设备
    /// </summary>
   public interface IISluRegulatorPara
   {  /// <summary>
       /// 地址  1-999
       /// </summary>

       int CtrlId { get; set; }


       int CtrlPhyId { get; set; }
       /// <summary>
       /// 父节点地址  即集中控制器地址
       /// </summary>

        int SluId { get; set; }
        string LampCode { get; set; }

       /// <summary>
       /// 开灯排序地址
       /// </summary>

        int OrderId { get; set; }


       /// <summary>
       /// 条形码
       /// </summary>

        long BarCodeId { get; set; }



       /// <summary>
       /// 设备名称  末端灯号等
       /// </summary>

        string RtuName { get; set; }


       /// <summary>
       /// 是否停运
       /// </summary>

        bool IsUsed { get; set; }


       /// <summary>
       /// 是否主动报警
       /// </summary>

        bool IsAlarmAuto { get; set; }


bool IsAutoOpenLightWhenElec1 { get; set; }
         bool IsAutoOpenLightWhenElec2 { get; set; }
         bool IsAutoOpenLightWhenElec3 { get; set; }
         bool IsAutoOpenLightWhenElec4 { get; set; }


       /// <summary>
       /// 回路1矢量
       /// </summary>

        int VectorLoop1 { get; set; }


       /// <summary>
       /// 回路2矢量
       /// </summary>

        int VectorLoop2 { get; set; }


       /// <summary>
       /// 回路3矢量
       /// </summary>

        int VectorLoop3 { get; set; }


       /// <summary>
       /// 回路4矢量
       /// </summary>

        int VectorLoop4 { get; set; }


        /// <summary>
        /// 额定功率 0-不设置额定功率；1:0-20；2:21-100；
        /// </summary>
         int PowerRate1 { get; set; }

        /// <summary>
        /// 额定功率 0-不设置额定功率；1:0-20；2:21-100；
        /// </summary>

         int PowerRate2 { get; set; }
        /// <summary>
        /// 额定功率 0-不设置额定功率；1:0-20；2:21-100；
        /// </summary>

         int PowerRate3 { get; set; }
        /// <summary>
        /// 额定功率 0-不设置额定功率；1:0-20；2:21-100；
        /// </summary>

         int PowerRate4 { get; set; }

        /// <summary>
        /// 灯头数  如1控2则为2 
        /// </summary>

         int LightCount { get; set; }


       /// <summary>
       /// 功率上限  为额定功率百分比 PowerRate* UpperPower/100
       /// </summary>

        int UpperPower { get; set; }


       /// <summary>
       /// 功率下限  为额定功率百分比 PowerRate* LowerPower/100
       /// </summary>

        int LowerPower { get; set; }


       /// <summary>
       /// 路由1
       /// </summary>

        int RoutePass1 { get; set; }


       /// <summary>
       /// 路由2
       /// </summary>

        int RoutePass2 { get; set; }


       /// <summary>
       /// 路由3
       /// </summary>

        int RoutePass3 { get; set; }


       /// <summary>
       /// 路由4
       /// </summary>

        int RoutePass4 { get; set; }

        double XGis { get; set; }
        double YGis { get; set; }
    }
}
