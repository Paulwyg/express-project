using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface
{
    public interface  IISlu
    {
         int RtuId { get; set; }

        /// <summary>
        /// 光控名称
        /// </summary>
        string RtuName { get; set; }

        /// <summary>
        /// 是否启用集中器巡测
        /// </summary>

         bool IsPartrolMeasured { get; set; }


        /// <summary>
        /// 是否投运
        /// </summary>
         bool IsUsed { get; set; }

         /// <summary>
         ///  1 Zigbee, 0 线路载波
         /// </summary>
         int IsZigbee { get; set; }

        /// <summary>
        /// 是否主动报警
        /// </summary>
         bool IsAlarmAuto { get; set; }


        /// <summary>
        /// 是否自动补发指令
        /// </summary>
         bool IsSndOrderAuto { get; set; }


        /// <summary>
        /// zigbee 地址
        /// </summary>
         long  ZigbeeAddress { get; set; }



        /// <summary>
        /// 控制器数目 最多256
        /// </summary>
         int SumOfControls { get; set; }


        /// <summary>
        /// 域名  1-65383
        /// </summary>
         int DomainName { get; set; }


        //[AttrisColumn("rtu_id")]
        // int domain_nameLastUsed { get; set; }

        ////域名 上一次使用的域名
        //[AttrisColumn("rtu_id")]
        // int DomainNamePrepare { get; set; }

        //未生效的新域名 1默认等待设置

        /// <summary>
        /// 电压上限
        /// </summary>
         int UpperVoltage { get; set; }

        //电压上限
        /// <summary>
        /// 电压下限
        /// </summary>
         int LowerVoltage { get; set; }


          double Longitude { get; set; }

         //纬度

          double Latitude { get; set; }



        /// <summary>
        /// 蓝牙Pin码
        /// </summary>
         int BluetoothPin { get; set; }


        /// <summary>
        /// 安全模式 : 0-无安全模式-配对成功即可查询但不可设置；1-安全模式1-配对成功即可查询设置；2-安全模式2-配对成功并经主台确认可查询设置；默认0
        /// </summary>
         int SecurityPattern { get; set; }


        /// <summary>
        /// 理由运行模式：1-标准模式；2-扩展模式；3-III代模式；4-IV代模式；5自适应模式；默认1
        /// </summary>
         int RouteRunPattern { get; set; }


        /// <summary>
        /// 启用的信道  1-16，存在于该列表中则启用，不存在则不启用 
        /// </summary>
         List<int> ChannelUsed { get; set; }


        /// <summary>
        /// 连续通信失败多少次后报警  1-50；
        /// </summary>
         int AlarmCountCommucationFail { get; set; }


        /// <summary>
        /// 功率因数低于多少后报警； 40-100
        /// </summary>
         int AlarmPowerfactorLower { get; set; }


         /// <summary>
         /// 电流量程；0.1-20
         /// </summary>
          double CurrentUpper { get; set; }

         /// <summary>
         /// 功率量程  ；10-2000
         /// </summary>
          int PowerUpper { get; set; }




         /// <summary>
         /// 1 -PWM ;2 -485 
         /// </summary>
          int PowerAdjustType { get; set; }

         /// <summary>
         /// 功率调节频率或波特率
         /// </summary>
          int PowerAdjustBound { get; set; }


          /// <summary>
          /// Ip地址 
          /// </summary>

           string  StaticIp { get; set; }

          /// <summary>
          /// 手机号码
          /// </summary>

           string MobileNo { get; set; }
    }
}
