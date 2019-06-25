using System.ComponentModel.Composition;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Ux.WJ3005Module.ZOrders.OpenCloseLight.ForMulitTreeGrpMenu
{
    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopOneForMulitGrpRightMenu : CloseLightLoopControllerForMulitGrpRightMenuBase
    {
        public CloseLightLoopOneForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopOneForMulitGrpRightMenuId;
            Description = "回路关灯部件，执行回路K1关灯，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopOneForMulitGrpRightMenuId
            //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopOneForMulitGrpRightMenuId 
            + "，类型为多终端分组右键菜单。";
            Tag = "关灯_K1";
           
            this.Text = "关K1";
            this.Tooltips = "关闭K1回路";
            LoopId = 1;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopTwoForMulitGrpRightMenu : CloseLightLoopControllerForMulitGrpRightMenuBase
    {
        public CloseLightLoopTwoForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopTwoForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForMulitGrpRightMenuId;
            Description = "回路关灯部件，执行回路K2关灯，ID为" +Services .MenuIdAssgin .MenuCloseLightLoopTwoForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "关灯_K2";
            this.Text = "关K2"; 
            this.Tooltips = "关闭K2回路";
            LoopId = 2;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopThreeForMulitGrpRightMenu : CloseLightLoopControllerForMulitGrpRightMenuBase
    {
        public CloseLightLoopThreeForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopThreeForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopThreeForMulitGrpRightMenuId;
            Description = "回路关灯部件，执行回路K3关灯，ID为" +
                        //Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopThreeForMulitGrpRightMenuId 
                        Services.MenuIdAssgin.MenuCloseLightLoopThreeForMulitGrpRightMenuId + "，类型为多终端分组右键菜单。";
            Tag = "关灯_K3";
            this.Text = "关K3"; 
            this.Tooltips = "关闭K3回路";
            LoopId = 3;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopFourForMulitGrpRightMenu : CloseLightLoopControllerForMulitGrpRightMenuBase
    {
        public CloseLightLoopFourForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopFourForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFourForMulitGrpRightMenuId;
            Description = "回路关灯部件，执行回路K4关灯，ID为" +Services.MenuIdAssgin .MenuCloseLightLoopFourForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFourForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "关灯_K4";
            this.Text = "关K4";
            this.Tooltips = "关闭K4回路";
            LoopId = 4;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopFiveForMulitGrpRightMenu : CloseLightLoopControllerForMulitGrpRightMenuBase
    {
        public CloseLightLoopFiveForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopFiveForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFiveForMulitGrpRightMenuId;
            Description = "回路关灯部件，执行回路K5关灯，ID为" +Services .MenuIdAssgin .MenuCloseLightLoopFiveForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopFiveForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "关灯_K5";
            this.Text = "关K5"; 
            this.Tooltips = "关闭K5回路";
            LoopId = 5;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopSixForMulitGrpRightMenu : CloseLightLoopControllerForMulitGrpRightMenuBase
    {
        public CloseLightLoopSixForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopSixForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForMulitGrpRightMenuId;
            Description = "回路关灯部件，执行回路K6关灯，ID为" +Services.MenuIdAssgin .MenuCloseLightLoopSixForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "关灯_K6";
            this.Text = "关K6"; 
            this.Tooltips = "关闭K6回路";
            LoopId = 6;
        }
    };

    //lvf
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopSevenForMulitGrpRightMenu : CloseLightLoopControllerForMulitGrpRightMenuBase
    {
        public CloseLightLoopSevenForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopSevenForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForMulitGrpRightMenuId;
            Description = "回路关灯部件，执行回路K7关灯，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopSevenForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "关灯_K7";
            this.Text = "关K7";
            this.Tooltips = "关闭K7回路";
            LoopId = 7;
        }
    };

    //lvf
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopEightForMulitGrpRightMenu : CloseLightLoopControllerForMulitGrpRightMenuBase
    {
        public CloseLightLoopEightForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopEightForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForMulitGrpRightMenuId;
            Description = "回路关灯部件，执行回路K8关灯，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopEightForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopSixForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "关灯_K8";
            this.Text = "关K8";
            this.Tooltips = "关闭K8回路";
            LoopId = 8;
        }
    };

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseLightLoopAllForMulitGrpRightMenu : CloseLightLoopControllerForMulitGrpRightMenuBase
    {
        public CloseLightLoopAllForMulitGrpRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopAllForMulitGrpRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopAllForMulitGrpRightMenuId;
            Description = "回路关灯部件，执行所有回路关灯，ID为" +Services .MenuIdAssgin .MenuCloseLightLoopAllForMulitGrpRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopAllForMulitGrpRightMenuId +
                        "，类型为多终端分组右键菜单。";
            Tag = "关灯_所有";
            this.Text = "关所有";
            this.Tooltips = "关闭ALL回路";
            LoopId = 0;
        }
    };
}
