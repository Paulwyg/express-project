using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.Models;
using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreMims.Commands;

namespace Wlst.Ux.MapGis.Services
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MapEditMenu : MenuItemBase
    {
        public MapEditMenu()
        {
            Id = Wlst.Ux.MapGis.Services.MenuIdAssign.MapEditId;// Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Text = "地图编辑";
            Tag = "地图编辑";
            this.Classic = "地图编辑";
            Description = "地图编辑，ID 为" + Id;// Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Tooltips = "地图编辑";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex);
        }

        protected void Ex()
        {
        }
    }
}
