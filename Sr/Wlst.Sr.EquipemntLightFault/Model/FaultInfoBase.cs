using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 


namespace Wlst.Sr.EquipemntLightFault.Model
{
    public class FaultInfoBase
    {
        #region

        /// <summary>
        /// 序号 程序内部识别唯一地址 程序收到一条报警数据后自增
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 报警等级
        /// </summary>
        public int IsShowAtTop { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// 第一次发生时间
        /// </summary>
        public DateTime DateFirst { get; set; }

        /// <summary>
        /// 故障终端序号 第一地址
        /// </summary>
        public int RtuId { get; set; }

        /// <summary>
        /// 故障回路或控制器序号 第二地址
        /// </summary>
        public int LoopId { get; set; }

        /// <summary>
        /// 故障灯头序号 第三地址
        /// </summary>
        public int LampId { get; set; }

        /// <summary>
        /// 终端物理地址
        /// </summary>
        public int RtuPhyId { get; set; }

        public int RtuFhyId { get; set; }
        /// <summary>
        /// 报警终端名称
        /// </summary>
        public string RtuName { get; set; }

        /// <summary>
        /// 报警回路或控制器名称
        /// </summary>
        public string RtuLoopName { get; set; }


        /// <summary>
        /// 报警终端父设备地址 为0则不存在父设备
        /// </summary>
        public int RtuFid { get; set; }

        /// <summary>
        /// 报警终端父设备名称
        /// </summary>
        public string RtuFname { get; set; }


        /// <summary>
        /// 故障序号
        /// </summary>
        public int FaultId { get; set; }

        /// <summary>
        /// 故障名称
        /// </summary>
        public string FaultName { get; set; }

        /// <summary>
        /// 记录编号 数据库存在的记录标号
        /// </summary>
        public long RecordId { get; set; }

        /// <summary>
        /// 在指定时间段内的报警次数统计
        /// </summary>
        public int AlarmCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 当前用户是否具有现实权限
        /// </summary>
        public bool IsThisUserShow { get; set; }

        /// <summary>
        /// 故障报警次数
        /// </summary>
        public int AlarmTimes;

        /// <summary>
        /// 故障报警显示颜色
        /// </summary>
        public string Color;

        /// <summary>
        /// 当前电流
        /// </summary>
        public double A { get; set; }

        /// <summary>
        /// 上限值
        /// </summary>
        public double AUpper { get; set; }

        /// <summary>
        /// 下限值
        /// </summary>
        public double ALower { get; set; }

        /// <summary>
        /// 额定值
        /// </summary>
        public double Aeding { get; set; }

        /// <summary>
        /// 电压
        /// </summary>
        public double V { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int PriorityLevel { get; set; }

        /// <summary>
        /// 派单
        /// </summary>
        public string Paidan { get; set; }

        #endregion
        public FaultInfoBase()
        {
            
        }
        public FaultInfoBase(Wlst .client.EquipmentFaultCurr   .OneFaultItem  fa,int id)
        {
            this.DateCreate = new DateTime(fa.DateCreate);
            this.FaultId = fa.FaultId;
            this.Id = id;
            this.LampId = fa.LampId;
            this.LoopId = fa.LoopId;
            this.RecordId = fa.DateCreate;
            this.Remark = fa.Remark;
            this.RtuId = fa.RtuId;
            this.DateFirst = new DateTime(fa.DtErrFirstAlarm);

            this.PriorityLevel = fa.PriorityLevel;

            this.Paidan = fa.PaiDan;

            this.A = fa.A;
            this.AUpper = fa.AUpper;
            this.ALower = fa.ALower;
            this.Aeding = fa.Aeding;
            this.V = fa.V;

            AlarmTimes = 3;
            DateCreate = new DateTime(fa.DateCreate);
            AlarmCount = fa.AlarmCount + 1;
            //物联网单灯
            if (RtuId > 1700000 && RtuId < 1800000)
            {
                UpdatePriInfoNB();
            }
            else
            {
                UpdatePriInfo();
            }


            if (Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetIsShieldAlarmsThatUserOcLightCause &&
                fa.IsCauseByUserOcLight == 1)
            {
                IsThisUserShow = false;
            }
        }


        internal void UpdatePriInfo()
        {
            var nts = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( RtuId);
            if (nts == null) return;
            RtuName = nts.RtuName;

            if (FaultId == 20 || FaultId == 21 ||(FaultId >=30 && FaultId <= 35))
            {
                var t =
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[RtuId]
                    as Wlst.Sr.EquipmentInfoHolding.Model.Wj3005Rtu;
                if (t == null) return;
                if (t.WjSwitchOuts.ContainsKey(LoopId))
                {
                    RtuLoopName = t.WjSwitchOuts[LoopId].SwitchName;
                }
                else
                {
                    RtuLoopName = "开关量输出 K" + LoopId;
                }

            }
            else if (FaultId > 5 && FaultId<18 || FaultId ==26)
            {
                RtuLoopName = nts.GetLoopName(LoopId);
                if (RtuLoopName.Trim() =="")
                {
                    RtuLoopName = "回路" + LoopId;

                }
                
            }else if (FaultId>=50 && FaultId <80 && LoopId >0 )
            {
                RtuLoopName = nts.GetLoopName(LoopId);
                if (LampId > 0) RtuLoopName = RtuLoopName + "," + LampId;
            }
            else
            {

                RtuLoopName = nts.GetLoopName(LoopId);
               
            }

            

            RtuPhyId = nts.RtuPhyId ;

            RtuFid = nts.RtuFid ;
            if (RtuFid != 0)
            {
                var tmps = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(RtuFid);
                if (tmps != null)
                {
                    RtuFname = tmps.RtuName;
                    RtuPhyId = tmps.RtuPhyId;

                    RtuName = RtuFname + " - " + RtuName;

                    //   RtuPhyId = tmps.PhyId;

                }
            }
            IsThisUserShow = false;
            //故障名称解析 
            var typeInfo = Wlst.Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoById(FaultId);
            if (typeInfo == null)
            {
                this.FaultName = " 未解析";
                Color = "#FF000000";
            }
            else
            {
                this.FaultName = string.IsNullOrEmpty(typeInfo.FaultNameByDefine)
                                     ? typeInfo.FaultName
                                     : typeInfo.FaultNameByDefine;
                Color = typeInfo.Color;
                IsThisUserShow = typeInfo.IsEnable;
                if (typeInfo.PriorityLevel==3) IsShowAtTop = 3;
            }

            if (FaultId == 16)
            {
                if (A == 222)
                {
                    FaultName = "火零均" + FaultName;
                    A = 0;
                }
                else if (A == 333)
                {
                    FaultName = "三相均" + FaultName;
                    A = 0;
                }
            }

            //用户是否显示 以及显示参数
            var userSet = Wlst.Sr.EquipemntLightFault.Services.UserDisplayErrorSelfSetInfoHold.GetInfoById(FaultId);
            if (userSet != null  )
            {
                IsThisUserShow = userSet.IsDisplay   && IsThisUserShow;
                AlarmTimes = userSet.AlarmTimes;
            }
            else
            {
                IsThisUserShow = false;
            }


        }
        internal void UpdatePriInfoNB()
        {
            var nts = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.GetField(RtuId);//Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(RtuId);
            if (nts == null) return;
            RtuName = nts.FieldName;
            RtuPhyId = nts.FieldId;

            IsThisUserShow = false;
            //故障名称解析 
            var typeInfo = Wlst.Sr.EquipemntLightFault.Services.TmlFaultTypeInfoServices.GetInfoById(FaultId);
            if (typeInfo == null)
            {
                this.FaultName = " 未解析";
                Color = "#FF000000";
            }
            else
            {
                this.FaultName = string.IsNullOrEmpty(typeInfo.FaultNameByDefine)
                                     ? typeInfo.FaultName
                                     : typeInfo.FaultNameByDefine;
                Color = typeInfo.Color;
                IsThisUserShow = typeInfo.IsEnable;
                if (typeInfo.PriorityLevel == 3) IsShowAtTop = 3;
            }


            if (FaultId >= 50 && FaultId < 80 && LoopId > 0)
            {
                var ctrlinfo = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(LoopId);
                RtuLoopName = ctrlinfo.CtrlName;
                if (LampId > 0) RtuLoopName = RtuLoopName + "," + LampId;
            }
            else
            {

                var ctrlinfo = Wlst.Sr.SlusglInfoHold.Services.SluSglInfoHold.MySlef.Get(LoopId);
                RtuLoopName = ctrlinfo.CtrlName;

            }
            //用户是否显示 以及显示参数
            var userSet = Wlst.Sr.EquipemntLightFault.Services.UserDisplayErrorSelfSetInfoHold.GetInfoById(FaultId);
            if (userSet != null)
            {
                IsThisUserShow = userSet.IsDisplay && IsThisUserShow;
                AlarmTimes = userSet.AlarmTimes;
            }
            else
            {
                IsThisUserShow = false;
            }


        }
    }
}
