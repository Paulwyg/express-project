using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.EquipemntLightFault.UserFaultSettingByAdmin.ViewModel
{
    public class UserInfoItem : NameValueInt
    {
        public List<int> Areas;
        public UserInfoItem ()
        {
            Areas = new List<int>();
        }
        public long Value;
    }
}
