using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipmentInfo.Services;

namespace Wlst.Ux.EquipmentInfo.SystemData.MainViewModel
{
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToMainView : MenuItemBase
    {
        public NavToMainView()
        {
            Id = MenuIdAssign.NavToSystemDataViewId;
            Text = "系统数据";
            Tag = "系统数据";
            Description = "系统数据，ID 为" + MenuIdAssign.NavToSystemDataViewId;
            Tooltips = "系统数据";
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
            this.ExNavWithArgs(ViewIdAssign.SystemDataViewId, 1);
        }
    }
}
