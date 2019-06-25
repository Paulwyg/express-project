using System.ComponentModel.Composition;
using Wlst.Cr.Core.CoreInterface;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.MenuShortCut.Services;

namespace Wlst.Ux.MenuShortCut.MenuShortCutViewModel
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToMenuShortCutView : MenuItemBase
    {
        public NavToMenuShortCutView()
        {
            Id =MenuIdAssgin.NavToMenuShortCutViewId;
            Text = "快捷键";
            Tag = "快捷键设置";
            this.Classic = "主菜单或全局设置";
            Description = "快捷键设置，ID 为" + MenuIdAssgin.NavToMenuShortCutViewId;
            Tooltips = "快捷键设置";
            base.IsEnabled = true;
            base.IsCheckable = false;
            base.Command = new RelayCommand(Ex );

            var ff = System.Diagnostics.Process.GetCurrentProcess().MaxWorkingSet;
            var gg = System.Diagnostics.Process.GetCurrentProcess().WorkingSet64;
            var tt = System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet;
            var kk = ff;
            var ggg = gg;
            var kkk = tt;
            int x = 100;
        }


        protected void Ex()
        {
            this.ExNavWithArgs(ViewIdAssign.MenuShortCutViewId ,
                               1);
        }

    }
}
