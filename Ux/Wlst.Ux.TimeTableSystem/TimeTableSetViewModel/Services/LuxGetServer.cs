using System;
using System.Collections.Generic;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.WjEquipmentBaseModels.Interface;


namespace Wlst.Ux.TimeTableSystem.TimeTableSetViewModel.Services
{
    public class LuxGetServer
    {
        public static IEnumerable<NameValueInt > GetAllLuxEquipment
        {
            get
            {
                var lst = new List<NameValueInt>();
                foreach (var t in
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary )
                {
                    try
                    {
                        var fffff = t.Value;
                        if (fffff.RtuModel == 1080)
                        {
                            lst.Add(new NameValueInt() {Name = fffff .RtuName ,Value =fffff .RtuId });
                        }
                    }
                    catch (Exception ex)
                    {
                        Wlst.Cr.Core.UtilityFunction.WriteLog.WriteLogError("Error:" + ex);
                    }
                }

                return lst;
            }
        }
    }
}