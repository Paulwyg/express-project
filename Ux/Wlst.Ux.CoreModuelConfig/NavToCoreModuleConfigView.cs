using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreOne.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Wlst.Ux.CoreModuelConfig
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToCoreModuleConfigView : MenuItemBase
    {
        public NavToCoreModuleConfigView()
        {
            Id = Services.MenuIdAssgin.NavToCoreModuleConfigViewId;
            Text = "模块控制";
            Tag = "控制程序加载模块";
            Description = "控制程序加载模块，ID 为" + Services.MenuIdAssgin.NavToCoreModuleConfigViewId;
            Tooltips = "控制程序加载模块";
            Classic = "主菜单";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex );
            
        }


        protected void Ex()
        {
            this.ExNavNoArgs(DocumentRegionName.DocumentRegion,
                             Services.ViewIdAssign.CoreModuleConfigViewId);
        }

    }
}
