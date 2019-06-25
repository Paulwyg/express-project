using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipmentInfo.Services;

namespace Wlst.Ux.EquipmentInfo.EquipmentInfo.MainViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToUxMainView : MenuItemBase
    {
        public NavToUxMainView()
        {
            Id = MenuIdAssign.NavToMainViewId;
            Text = "设备信息";
            Tag = "设备信息";
            Description = "设备信息，ID 为" + MenuIdAssign.NavToMainViewId;
            Tooltips = "设备信息";
            Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex);
            //IsPrivilegLeave = false ;
        }

        public override bool IsCanBeShowRwx()
        {
            return true;

        }
        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdAssign.MainViewId, 1);
        }
    }
}
