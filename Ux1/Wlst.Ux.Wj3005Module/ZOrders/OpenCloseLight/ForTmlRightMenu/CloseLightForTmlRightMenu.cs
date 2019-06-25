using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Ux.WJ3005Module.ZOrders.OpenCloseLight.ForTmlRightMenu
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopOneForTmlRightMenu : CloseLightLoopControllerForTmlRightMenuBase
    {
        public CloseLightLoopOneForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopOneForTmlRightMenuId;
            Description = "回路关灯部件，执行回路K1关灯，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopOneForTmlRightMenuId
            //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForTmlRightMenuId 
            + "，类型为终端右键菜单。";
            Tag = "关灯_K1";
           
            this.Text = "关K1"; 
            this.Tooltips = "关闭K1回路";
            LoopId = 1;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopTwoForTmlRightMenu : CloseLightLoopControllerForTmlRightMenuBase
    {
        public CloseLightLoopTwoForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopTwoForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForTmlRightMenuId;
            Description = "回路关灯部件，执行回路K2关灯，ID为" +Services .MenuIdAssgin .MenuCloseLightLoopTwoForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "关灯_K2";
            this.Text = "关K2"; 
            this.Tooltips = "关闭K2回路";
            LoopId = 2;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopThreeForTmlRightMenu : CloseLightLoopControllerForTmlRightMenuBase
    {
        public CloseLightLoopThreeForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopThreeForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopThreeForTmlRightMenuId;
            Description = "回路关灯部件，执行回路K3关灯，ID为" +
                        //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopThreeForTmlRightMenuId 
                        Services .MenuIdAssgin .MenuCloseLightLoopThreeForTmlRightMenuId + "，类型为终端右键菜单。";
            Tag = "关灯_K3";
            this.Text = "关K3"; 
            this.Tooltips = "关闭K3回路";
            LoopId = 3;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopFourForTmlRightMenu : CloseLightLoopControllerForTmlRightMenuBase
    {
        public CloseLightLoopFourForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopFourForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFourForTmlRightMenuId;
            Description = "回路关灯部件，执行回路K4关灯，ID为" +Services.MenuIdAssgin .MenuCloseLightLoopFourForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFourForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "关灯_K4";
            this.Text = "关K4"; 
            this.Tooltips = "关闭K4回路";
            LoopId = 4;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopFiveForTmlRightMenu : CloseLightLoopControllerForTmlRightMenuBase
    {
        public CloseLightLoopFiveForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopFiveForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFiveForTmlRightMenuId;
            Description = "回路关灯部件，执行回路K5关灯，ID为" +Services .MenuIdAssgin .MenuCloseLightLoopFiveForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFiveForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "关灯_K5";
            this.Text = "关K5"; 
            this.Tooltips = "关闭K5回路";
            LoopId = 5;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopSixForTmlRightMenu : CloseLightLoopControllerForTmlRightMenuBase
    {
        public CloseLightLoopSixForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopSixForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForTmlRightMenuId;
            Description = "回路关灯部件，执行回路K6关灯，ID为" +Services.MenuIdAssgin .MenuCloseLightLoopSixForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "关灯_K6";
            this.Text = "关K6";
            this.Tooltips = "关闭K6回路";
            LoopId = 6;
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopSevenForTmlRightMenu : CloseLightLoopControllerForTmlRightMenuBase
    {
        public CloseLightLoopSevenForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopSevenForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForTmlRightMenuId;
            Description = "回路关灯部件，执行回路K7关灯，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopSevenForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "关灯_K7";
            this.Text = "关K7";
            this.Tooltips = "关闭K7回路";
            LoopId = 7;
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopEightForTmlRightMenu : CloseLightLoopControllerForTmlRightMenuBase
    {
        public CloseLightLoopEightForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopEightForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForTmlRightMenuId;
            Description = "回路关灯部件，执行回路K8关灯，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopEightForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "关灯_K8";
            this.Text = "关K8";
            this.Tooltips = "关闭K8回路";
            LoopId = 8;
        }
    };


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopAllForTmlRightMenu : CloseLightLoopControllerForTmlRightMenuBase
    {
        public CloseLightLoopAllForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopAllForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopAllForTmlRightMenuId;
            Description = "回路关灯部件，执行所有回路关灯，ID为" +Services .MenuIdAssgin .MenuCloseLightLoopAllForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopAllForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "关灯_所有";
            this.Text = "关所有";
            this.Tooltips = "关闭ALL回路";
            LoopId = 0;
        }
    };
}
