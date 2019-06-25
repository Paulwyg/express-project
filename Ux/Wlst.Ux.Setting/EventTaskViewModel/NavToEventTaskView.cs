using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Cr.Setting.Interfaces;
using Wlst.Cr.Setting.Models;

namespace Wlst.Ux.Setting.EventTaskViewModel
{
  
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEventTaskView : MenuItemBase
    {
        public NavToEventTaskView()
        {
            Id = Setting.Services.MenuIdAssgin.NavToEventTaskViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Text = "计划任务";
            Tag = "计划任务设置";
            this.Classic = "主菜单";
            Description = "计划任务设置，ID 为" + Setting.Services.MenuIdAssgin.NavToEventTaskViewId;
            // Infrastructure.IdAssign.MenuIdAssign.NavToSndWeekTimeQueryViewId;
            Tooltips = "计划任务设置";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
           // IsPrivilegLeave = true;
        }
        public override bool IsCanBeShowRwx()
        {
            return  Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.AreaX.Count !=0;
            //return true;
        }
        bool CanEx()
        {
            return true;
        }


        protected void Ex()
        {
            this.ExNavWithArgs(
                //ViewIdNameAssign.EquipmentDataQuerySndWeekTimeQueryViewAttachRegion,
                               Setting.Services.ViewIdAssign.EventTaskViewId,
                //ViewIdNameAssign.EquipmentDataQuerySndWeekTimeQueryViewId,
                               1);
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
