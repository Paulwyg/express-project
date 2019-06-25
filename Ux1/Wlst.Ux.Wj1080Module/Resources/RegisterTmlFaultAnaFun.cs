using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj1080Module.Resources
{
    ///// <summary>
    ///// 注册 设备类型故障数据解析函数
    ///// </summary>
    //public class RegisterTmlFaultAnaFun
    //{
    //    private static EquipmentFaultConverInfo GetInfoByFault(EquipmentFault fault)
    //    {
    //        var info = new EquipmentFaultConverInfo();
    //        info.EquipmentRtuId = fault.RtuId;
    //        info.EquipmentPhyId = fault.RtuId;
    //        info.CreatTime = fault.DateCreate.ToString("yyyy-MM-dd HH:mm:ss");
    //        info.Id = fault.Id;
    //        info.EquipmentNameOne = fault.RtuId + "";
    //        info.EquipmentNameTwo = "--";

    //        if (
    //            Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary.
    //                ContainsKey(fault.RtuId))
    //        {
    //            var ffff =
    //                Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
    //                    fault.RtuId];

    //            info.EquipmentNameOne = ffff.RtuName;
    //            info.EquipmentPhyId = ffff.PhyId;
    //            if (fault.LoopId > 0)
    //            {
    //                var amp = Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary[
    //                    fault.RtuId] as Wlst.Cr.WjEquipmentBaseModels.TerminalInfomationInterface.IIRtuParaAnalogueAmps;
    //                if (amp == null)
    //                {
    //                    info.EquipmentNameTwo = "回路:" + fault.LoopId + "";
    //                }
    //                else
    //                {
    //                    foreach (var ggg in amp.RtuParaAnalogueAmps.GetAllRtuParaAnalogueAmps()) if (ggg.LoopId == fault.LoopId)
    //                        {
    //                            info.EquipmentNameTwo = ggg.LoopName;
    //                        }
    //                }
    //            }
    //        }


    //        info.EquipmentNameTree = "";
    //        return info;
    //    }

    //    /// <summary>
    //    /// 注册故障解析函数
    //    /// </summary>
    //    public static void RegisterFun()
    //    {
    //        Sr.EquipemntLightFault.Services.EquipmentFaultInfoConverServices.RegisterEquipmentExistErrorConverFun(
    //            3005,
    //            GetInfoByFault);
    //    }


    //}
}
