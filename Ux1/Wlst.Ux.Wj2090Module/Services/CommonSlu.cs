using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace Wlst.Ux.Wj2090Module.Services
{
    public class CommonSlu
    {
        internal static List<int> GetPhyIdsByCtrls(int sluId, List<int> ctrlIds)
        {
            var rtn = new List<int>();
            var sluinfo =
               Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( 
                   sluId);
            var cons = sluinfo as Wlst .Sr .EquipmentInfoHolding .Model .Wj2090Slu ;
            if (cons == null) return rtn;
            foreach (var g in ctrlIds)
            {
                if (cons.WjSluCtrls .ContainsKey(g))
                    rtn.Add(cons.WjSluCtrls [g].CtrlPhyId);
            }
            return rtn;
        }


        internal static int GetPhyIdByCtrl(int sluId,int ctrlId)
        {
            var sluinfo =
               Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold .GetInfoById( 
                   sluId);
            var cons = sluinfo as Wlst.Sr .EquipmentInfoHolding .Model .Wj2090Slu ;
            if (cons == null) return 0;

            if (cons.WjSluCtrls .ContainsKey(ctrlId))
               return  cons.WjSluCtrls [ctrlId].CtrlPhyId;
            
            return 0;
        }

        //lvf 2018年4月23日16:18:38 添加  读取控制器名称
        internal static string  GetNameByCtrl(int sluId, int ctrlId)
        {
            var sluinfo =
               Wlst.Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(
                   sluId);
            var cons = sluinfo as Wlst.Sr.EquipmentInfoHolding.Model.Wj2090Slu;
            if (cons == null) return "未知";

            if (cons.WjSluCtrls.ContainsKey(ctrlId))
                return cons.WjSluCtrls[ctrlId].LampCode;

            return "未知";
        }

    }
}
