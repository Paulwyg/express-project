using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Ux.Nr6005Module.ZOrders.OpenCloseLight.ForMulitTreeGrpMenu
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopOneForMulitGrpRightMenu : OpenLightLoopControllerForMulitGrpRightMenuBase
    {
        public OpenLightLoopOneForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopOneForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForMulitGrpRightMenuId;
            Description = "回路开灯部件，执行回路K1开灯，ID为" + Services .MenuIdAssgin .MenuOpenLightLoopOneForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopOneForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "开灯_K1";
            this.Text = "开K1";
            this.Tooltips = "开启K1回路";
            LoopId = 1;
        }
    };


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopTwoForMulitGrpRightMenu : OpenLightLoopControllerForMulitGrpRightMenuBase
    {
        public OpenLightLoopTwoForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopTwoForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForMulitGrpRightMenuId;
            Description = "回路开灯部件，执行回路K2开灯，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopTwoForMulitGrpRightMenuId+// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "开灯_K2";
            this.Text = "开K2";
            this.Tooltips = "开启K2回路";
            LoopId = 2;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopThreeForMulitGrpRightMenu : OpenLightLoopControllerForMulitGrpRightMenuBase
    {

        public OpenLightLoopThreeForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopThreeForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopThreeForMulitGrpRightMenuId;
            Description = "回路开灯部件，执行回路K3开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopThreeForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopThreeForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "开灯_K3";
            this.Text = "开K3";
            this.Tooltips = "开启K3回路";
            LoopId = 3;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopFourForMulitGrpRightMenu : OpenLightLoopControllerForMulitGrpRightMenuBase
    {
        public OpenLightLoopFourForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopFourForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFourForMulitGrpRightMenuId;
            Description = "回路开灯部件，执行回路K4开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopFourForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFourForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "开灯_K4";
            this.Text = "开K4";
            this.Tooltips = "开启K4回路";
            LoopId = 4;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopFiveForMulitGrpRightMenu : OpenLightLoopControllerForMulitGrpRightMenuBase
    {
        public OpenLightLoopFiveForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopFiveForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFiveForMulitGrpRightMenuId;
            Description = "回路开灯部件，执行回路K5开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopFiveForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopFiveForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "开灯_K5";
            this.Text = "开K5";
            this.Tooltips = "开启K5回路";
            LoopId = 5;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopSixForMulitGrpRightMenu : OpenLightLoopControllerForMulitGrpRightMenuBase
    {
        public OpenLightLoopSixForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopSixForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForMulitGrpRightMenuId;
            Description = "回路开灯部件，执行回路K6开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopSixForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "开灯_K6";
            this.Text = "开K6";
            this.Tooltips = "开启K6回路";
            LoopId = 6;
        }
    };

    //lvf
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopSevenForMulitGrpRightMenu : OpenLightLoopControllerForMulitGrpRightMenuBase
    {
        public OpenLightLoopSevenForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopSevenForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForMulitGrpRightMenuId;
            Description = "回路开灯部件，执行回路K7开灯，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopSevenForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "开灯_K7";
            this.Text = "开K7";
            this.Tooltips = "开启K7回路";
            LoopId = 7;
        }
    };

    //lvf
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopEightForMulitGrpRightMenu : OpenLightLoopControllerForMulitGrpRightMenuBase
    {
        public OpenLightLoopEightForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopEightForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForMulitGrpRightMenuId;
            Description = "回路开灯部件，执行回路K8开灯，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopEightForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopSixForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "开灯_K8";
            this.Text = "开K8";
            this.Tooltips = "开启K8回路";
            LoopId = 8;
        }
    };


    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenLightLoopAllForMulitGrpRightMenu : OpenLightLoopControllerForMulitGrpRightMenuBase
    {
        public OpenLightLoopAllForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopAllForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopAllForMulitGrpRightMenuId;
            Description = "回路开灯部件，执行所有回路开灯，ID为" +Services .MenuIdAssgin .MenuOpenLightLoopAllForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopAllForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "开灯_ALL";
            this.Text = "开所有";
            this.Tooltips = "开启所有回路";
            LoopId = 0;
        }
    };
}
