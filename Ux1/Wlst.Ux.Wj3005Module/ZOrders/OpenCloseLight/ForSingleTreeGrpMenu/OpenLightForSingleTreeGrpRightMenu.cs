using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Ux.WJ3005Module.ZOrders.OpenCloseLight.ForSingleTreeGrpMenu
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopOneForSingleTreeGrpRightMenu : OpenLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public OpenLightLoopOneForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopOneForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId;
            Description = "回路开灯部件，执行回路K1开灯，ID为" + Services .MenuIdAssgin .MenuOpenLightLoopOneForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "开灯_K1";
            this.Text = "开K1";
            this.Tooltips = "开启K1回路";
            LoopId = 1;
        }
    };


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopTwoForSingleTreeGrpRightMenu : OpenLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public OpenLightLoopTwoForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopTwoForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForSingleTreeGrpRightMenuId;
            Description = "回路开灯部件，执行回路K2开灯，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopTwoForSingleTreeGrpRightMenuId+// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "开灯_K2";
            this.Text = "开K2";
            this.Tooltips = "开启K2回路";
            LoopId = 2;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopThreeForSingleTreeGrpRightMenu : OpenLightLoopControllerForSingleTreeGrpRightMenuBase
    {

        public OpenLightLoopThreeForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopThreeForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopThreeForSingleTreeGrpRightMenuId;
            Description = "回路开灯部件，执行回路K3开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopThreeForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopThreeForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "开灯_K3";
            this.Text = "开K3";
            this.Tooltips = "开启K3回路";
            LoopId = 3;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopFourForSingleTreeGrpRightMenu : OpenLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public OpenLightLoopFourForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopFourForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFourForSingleTreeGrpRightMenuId;
            Description = "回路开灯部件，执行回路K4开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopFourForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFourForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "开灯_K4";
            this.Text = "开K4";
            this.Tooltips = "开启K4回路";
            LoopId = 4;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopFiveForSingleTreeGrpRightMenu : OpenLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public OpenLightLoopFiveForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopFiveForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFiveForSingleTreeGrpRightMenuId;
            Description = "回路开灯部件，执行回路K5开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopFiveForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFiveForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "开灯_K5";
            this.Text = "开K5";
            this.Tooltips = "开启K5回路";
            LoopId = 5;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopSixForSingleTreeGrpRightMenu : OpenLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public OpenLightLoopSixForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopSixForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForSingleTreeGrpRightMenuId;
            Description = "回路开灯部件，执行回路K6开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopSixForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "开灯_K6";
            this.Text = "开K6";
            this.Tooltips = "开启K6回路";
            LoopId = 6;
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopSevenForSingleTreeGrpRightMenu : OpenLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public OpenLightLoopSevenForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopSevenForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForSingleTreeGrpRightMenuId;
            Description = "回路开灯部件，执行回路K7开灯，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopSevenForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "开灯_K7";
            this.Text = "开K7";
            this.Tooltips = "开启K7回路";
            LoopId = 7;
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopEightForSingleTreeGrpRightMenu : OpenLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public OpenLightLoopEightForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopEightForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForSingleTreeGrpRightMenuId;
            Description = "回路开灯部件，执行回路K8开灯，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopEightForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "开灯_K8";
            this.Text = "开K8";
            this.Tooltips = "开启K8回路";
            LoopId = 8;
        }
    };


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopAllForSingleTreeGrpRightMenu : OpenLightLoopControllerForSingleTreeGrpRightMenuBase
    {
        public OpenLightLoopAllForSingleTreeGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopAllForSingleTreeGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopAllForSingleTreeGrpRightMenuId;
            Description = "回路开灯部件，执行所有回路开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopAllForSingleTreeGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopAllForSingleTreeGrpRightMenuId +
                        "，类型为单分组右键菜单。";
            Tag = "开灯_ALL";
            this.Text = "开所有";
            this.Tooltips = "开启所有回路";
            LoopId = 0;
        }
    };
}
