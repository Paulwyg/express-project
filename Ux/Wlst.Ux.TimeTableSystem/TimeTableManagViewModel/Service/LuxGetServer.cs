using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.TimeTableSystem.TimeTableManagViewModel.Service
{
    public class LuxGetServer
    {
        public static IEnumerable<NameValueInt> GetAllLuxEquipment
        {
            get
            {
                var lst = new List<NameValueInt>();
                foreach (var t in
                    Sr.EquipmentInfoHolding.Services.ServicesEquipemntInfoHold.EquipmentInfoDictionary)
                {
                    try
                    {
                        var fffff = t.Value;
                        if (fffff.RtuModel == 1080)
                        {
                            lst.Add(new NameValueInt() { Name = fffff.RtuName, Value = fffff.RtuId });
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
