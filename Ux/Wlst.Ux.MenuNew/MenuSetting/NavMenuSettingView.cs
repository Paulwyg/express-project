using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;
using Wlst.Ux.MenuNew.Services;

namespace Wlst.Ux.MenuNew.MenuSetting
{
    internal class NavMenuSettingView
    {
        [Export(typeof (IISetting))]
        [PartCreationPolicy(CreationPolicy.Shared)]
        public class NavLineNewDataSettingView : SettingBase
        {

            public NavLineNewDataSettingView()
            {
                this.Id = MenuIdAssgin.MenuSettingId;
                this.ViewId = ViewIdAssign.MenuSettingId;
                this.PathSetting = "菜单管理";
                this.Describle = "admin";
            }
        }
    }
}