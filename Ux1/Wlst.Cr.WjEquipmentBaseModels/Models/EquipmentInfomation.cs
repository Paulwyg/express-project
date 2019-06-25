using System;
using System.Collections.Generic;
using Wlst.Cr.WjEquipmentBaseModels.Interface;
using Wlst.client;

namespace Wlst.Cr.WjEquipmentBaseModels.Models
{
    [Serializable]
    public class EquipmentInfomation : IIEquipmentInfo
    {
        /// <summary>
        /// 设备参数构造函数
        /// </summary>
        /// <param name="rtuId">设备地址</param>
        /// <param name="rtuName">设备名称</param>
        /// <param name="rtuState">终端工作状态</param>
        public EquipmentInfomation(int rtuId, string rtuName, int rtuState)
        {
            RtuId = rtuId;
            RtuName = rtuName;
            RtuState = rtuState;
           // EnumDeviceType = EnumDeviceType.Reserve;
        }

        public EquipmentInfomation()
        {
            RtuId = 0;
            RtuName = "0";
            RtuState = 0;
           // EnumDeviceType = EnumDeviceType.Reserve;
        }

        public EquipmentInfomation(IIEquipmentInfo info)
        {
            this.AttachRtuId = info.AttachRtuId;
            this.DataCreate = info.DataCreate;
            //this.EnumDeviceType = info.EnumDeviceType;
            this.InstallAddr = info.InstallAddr;
            this.Md5 = info.Md5;
            this.OtherArgu = info.OtherArgu;
            this.PhyId = info.PhyId;
            this.Remark = info.Remark;
            this.RtuId = info.RtuId;
            this.RtuModel = info.RtuModel;
            this.RtuName = info.RtuName;
            this.RtuState = info.RtuState;
            //this.Xgis = info.Xgis;
            //this.Ygis = info.Ygis;
            this.Xmap = info.Xmap;
            this.Ymap = info.Ymap;
            this.Xgis = info.Xgis;
            this.Ygis = info.Ygis;
            this.AreaId = info.AreaId;
        }

        /// <summary>
        /// 设备安装位置
        /// </summary>
        public virtual string InstallAddr { get; set; }

        /// <summary>
        /// 设备备注信息
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 设备开通日期
        /// </summary>
        public virtual long DataCreate { get; set; }

        /// <summary>
        /// 附属设备逻辑ID
        /// </summary>
        public virtual int RtuId { get; set; }

        /// <summary>
        /// 附属设备逻辑地址
        /// </summary>
        public virtual int PhyId { get; set; }

        /// <summary>
        /// 附属设备名称
        /// </summary>
        public virtual string RtuName { get; set; }

        /// <summary>
        /// 连接主设备的逻辑地址
        /// </summary>
        public virtual int AttachRtuId { get; set; }

        public virtual int AreaId { get; set; }

        /// <summary>
        /// 终端工作状态
        ///  0-不用，1-停运，2-使用
        /// </summary>
        public virtual int RtuState { get; set; }

        /// <summary>
        /// 附属设备型号 默认1987
        /// </summary>
        public virtual int RtuModel { get; set; }

        /// <summary>
        /// 地图X坐标 
        /// </summary>
        public virtual double Xmap { get; set; }

        /// <summary>
        /// 地图Y坐标
        /// </summary>
        public virtual double Ymap { get; set; }

        /// <summary>
        ///  GIS以及其他矢量地图
        /// </summary>
        public virtual double Xgis { get; set; }

        /// <summary>
        ///  GIS以及其他矢量地图
        /// </summary>
        public virtual double Ygis { get; set; }

        /// <summary>
        /// 数据最后更新时间
        /// </summary>
        public virtual long Md5 { get; set; }

        /// <summary>
        /// 其他参数
        /// </summary>
        public virtual object OtherArgu { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        //public virtual EnumDeviceType EnumDeviceType { get; set; }


        /// <summary>
        /// 获取设备第一回路 第二回路名称
        /// </summary>
        /// <param name="loopId"></param>
        /// <returns></returns>
        public virtual string  GetRtuLoopName(int loopId)
        {
            return string.Empty;
        }
        public virtual string GetRtuLoopNameandLampCode(int loopId)
        {
            return string.Empty;
        }
    }
}
