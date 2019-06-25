using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Models;
using Wlst.Sr.EquipmentInfoHolding.Model;

namespace Wlst.Ux.TimeTableSystem.TimeInfoMn.Services
{
    public class LuxGetServer
    {
        public static IEnumerable<NameValueInt> GetAllLuxEquipment
        {
            get
            {
                var lst = new List<NameValueInt>();
                foreach (var t in
                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems)
                {
                    try
                    {
                        var fffff = t.Value;
                        if (fffff.EquipmentType == WjParaBase.EquType.Lux)
                        {
                            if (fffff.RtuFid == 0)
                            {
                                lst.Add(new NameValueInt()
                                            {Name = fffff.RtuName, Value = fffff.RtuId, Value2 = fffff.RtuPhyId}); //主设备
                            }
                            else
                            {
                                var info =
                                    Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.GetInfoById(fffff.RtuFid);
                                if (info == null)
                                {
                                    lst.Add(new NameValueInt()
                                                {Name = fffff.RtuName, Value = fffff.RtuId, Value2 = fffff.RtuPhyId});
                                }
                                else
                                {
                                    lst.Add(new NameValueInt()
                                                {Name = fffff.RtuName, Value = fffff.RtuId, Value2 = info.RtuPhyId});
                                        //附属设备
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                    }
                }
                var lstt = (from t in lst orderby t.Value select t).ToList();
                return lstt;
            }
        }
    }
}
