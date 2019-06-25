using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Ux.Nr6005Module.ZOrders.OpenCloseLight.ForTmlRightMenu
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopOneForTmlRightMenu : OpenLightLoopControllerForTmlRightMenuBase
    {
        public OpenLightLoopOneForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopOneForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForTmlRightMenuId;
            Description = "回路开灯部件，执行回路K1开灯，ID为" + Services .MenuIdAssgin .MenuOpenLightLoopOneForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "开灯_K1";
            this.Text = "开K1";
            this.Tooltips = "开启K1回路";
            LoopId = 1;
        }
    };


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopTwoForTmlRightMenu : OpenLightLoopControllerForTmlRightMenuBase
    {
        public OpenLightLoopTwoForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopTwoForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForTmlRightMenuId;
            Description = "回路开灯部件，执行回路K2开灯，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopTwoForTmlRightMenuId+// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "开灯_K2";
            this.Text = "开K2";
            this.Tooltips = "开启K2回路";
            LoopId = 2;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopThreeForTmlRightMenu : OpenLightLoopControllerForTmlRightMenuBase
    {

        public OpenLightLoopThreeForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopThreeForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopThreeForTmlRightMenuId;
            Description = "回路开灯部件，执行回路K3开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopThreeForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopThreeForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "开灯_K3";
            this.Text = "开K3";
            this.Tooltips = "开启K3回路";
            LoopId = 3;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopFourForTmlRightMenu : OpenLightLoopControllerForTmlRightMenuBase
    {
        public OpenLightLoopFourForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopFourForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFourForTmlRightMenuId;
            Description = "回路开灯部件，执行回路K4开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopFourForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFourForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "开灯_K4";
            this.Text = "开K4";
            this.Tooltips = "开启K4回路";
            LoopId = 4;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopFiveForTmlRightMenu : OpenLightLoopControllerForTmlRightMenuBase
    {
        public OpenLightLoopFiveForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopFiveForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFiveForTmlRightMenuId;
            Description = "回路开灯部件，执行回路K5开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopFiveForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFiveForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "开灯_K5";
            this.Text = "开K5";
            this.Tooltips = "开启K5回路";
            LoopId = 5;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopSixForTmlRightMenu : OpenLightLoopControllerForTmlRightMenuBase
    {
        public OpenLightLoopSixForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopSixForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForTmlRightMenuId;
            Description = "回路开灯部件，执行回路K6开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopSixForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "开灯_K6";
            this.Text = "开K6";
            this.Tooltips = "开启K6回路";
            LoopId = 6;
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopSevenForTmlRightMenu : OpenLightLoopControllerForTmlRightMenuBase
    {
        public OpenLightLoopSevenForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopSevenForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForTmlRightMenuId;
            Description = "回路开灯部件，执行回路K7开灯，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopSevenForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "开灯_K7";
            this.Text = "开K7";
            this.Tooltips = "开启K7回路";
            LoopId = 7;
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopEightForTmlRightMenu : OpenLightLoopControllerForTmlRightMenuBase
    {
        public OpenLightLoopEightForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopEightForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForTmlRightMenuId;
            Description = "回路开灯部件，执行回路K8开灯，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopEightForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "开灯_K8";
            this.Text = "开K8";
            this.Tooltips = "开启K8回路";
            LoopId = 8;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopAllForTmlRightMenu : OpenLightLoopControllerForTmlRightMenuBase
    {
        public OpenLightLoopAllForTmlRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopAllForTmlRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopAllForTmlRightMenuId;
            Description = "回路开灯部件，执行所有回路开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopAllForTmlRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopAllForTmlRightMenuId +
                        "，类型为终端右键菜单。";
            Tag = "开灯_ALL";
            this.Text = "开所有";
            this.Tooltips = "开启所有回路";
            LoopId = 0;
        }
    };
}
