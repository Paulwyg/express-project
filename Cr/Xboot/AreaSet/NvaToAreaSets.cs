using System.ComponentModel.Composition;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;

namespace Xboot.AreaSet
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NvaToAreaSets : MenuItemBase
    {
        public NvaToAreaSets()
        {
            Id = 2101110;// Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Text = "界面布局";
            Tag = "前台界面布局控制";
            this.Classic = "主菜单";
            Description = "界面布局";// Infrastructure.IdAssign.MenuIdAssign.NavToWj3005TmlInfoSetId;
            Tooltips = "前台界面布局控制";
            base.IsCheckable = false;
            base.IsEnabled = true;
            base.Command = new RelayCommand(Ex );
            //this.IsPrivilegLeave = false;
        }
        public override bool IsCanBeShowRwx()
        {
            return true;
        }
        protected void Ex()
        {
            AreaSet.AreaSeta info = new AreaSeta();
            info.ShowDialog();
        }
    }
}
