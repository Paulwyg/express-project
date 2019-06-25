using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Ux.Nr6005Module.ZOrders.OpenCloseLight.ForSingleTreeGrpMenu
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopOneForSingleTreeGrpRightMenu : CloseLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public CloseLightLoopOneForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "回路关灯部件，执行回路K1关灯，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopOneForSingleTreeGrpRightMenuId
            //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForSingleTreeGrpRightMenuId 
            + "，类型为单分组右键菜单。";
            Tag = "关灯_K1";
           
            this.Text = "关K1"; 
            this.Tooltips = "关闭K1回路";
            LoopId = 1;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopTwoForSingleTreeGrpRightMenu : CloseLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public CloseLightLoopTwoForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopTwoForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForSingleTreeGrpRightMenuId;
            Description = "回路关灯部件，执行回路K2关灯，ID为" +Services .MenuIdAssgin .MenuCloseLightLoopTwoForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "关灯_K2";
            this.Text = "关K2";
            this.Tooltips = "关闭K2回路";
            LoopId = 2;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopThreeForSingleTreeGrpRightMenu : CloseLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public CloseLightLoopThreeForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopThreeForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopThreeForSingleTreeGrpRightMenuId;
            Description = "回路关灯部件，执行回路K3关灯，ID为" +
                        //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopThreeForSingleTreeGrpRightMenuId 
                        Services.MenuIdAssgin.MenuCloseLightLoopThreeForSingleTreeGrpRightMenuId + "，类型为单分组右键菜单。";
            Tag = "关灯_K3";
            this.Text = "关K3";
            this.Tooltips = "关闭K3回路";
            LoopId = 3;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopFourForSingleTreeGrpRightMenu : CloseLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public CloseLightLoopFourForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopFourForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFourForSingleTreeGrpRightMenuId;
            Description = "回路关灯部件，执行回路K4关灯，ID为" +Services.MenuIdAssgin .MenuCloseLightLoopFourForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFourForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "关灯_K4";
            this.Text = "关K4"; 
            this.Tooltips = "关闭K4回路";
            LoopId = 4;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopFiveForSingleTreeGrpRightMenu : CloseLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public CloseLightLoopFiveForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopFiveForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFiveForSingleTreeGrpRightMenuId;
            Description = "回路关灯部件，执行回路K5关灯，ID为" +Services .MenuIdAssgin .MenuCloseLightLoopFiveForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFiveForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "关灯_K5";
            this.Text = "关K5";
            this.Tooltips = "关闭K5回路";
            LoopId = 5;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopSixForSingleTreeGrpRightMenu : CloseLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public CloseLightLoopSixForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopSixForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForSingleTreeGrpRightMenuId;
            Description = "回路关灯部件，执行回路K6关灯，ID为" +Services.MenuIdAssgin .MenuCloseLightLoopSixForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "关灯_K6";
            this.Text = "关K6"; 
            this.Tooltips = "关闭K6回路";
            LoopId = 6;
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopSevenForSingleTreeGrpRightMenu : CloseLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public CloseLightLoopSevenForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopSevenForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForSingleTreeGrpRightMenuId;
            Description = "回路关灯部件，执行回路K7关灯，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopSevenForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "关灯_K7";
            this.Text = "关K7";
            this.Tooltips = "关闭K7回路";
            LoopId = 7;
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopEightForSingleTreeGrpRightMenu : CloseLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public CloseLightLoopEightForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopEightForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForSingleTreeGrpRightMenuId;
            Description = "回路关灯部件，执行回路K8关灯，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopEightForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "关灯_K8";
            this.Text = "关K8";
            this.Tooltips = "关闭K8回路";
            LoopId = 8;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopAllForSingleTreeGrpRightMenu : CloseLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public CloseLightLoopAllForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopAllForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopAllForSingleTreeGrpRightMenuId;
            Description = "回路关灯部件，执行所有回路关灯，ID为" +Services .MenuIdAssgin .MenuCloseLightLoopAllForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopAllForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "关灯_所有";
            this.Text = "关所有";
            this.Tooltips = "关闭ALL回路";
            LoopId = 0;
        }
    };
}
