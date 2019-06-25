using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using Wlst.Cr.CoreOne.CoreInterface;

namespace Wlst.Ux.Wj9001Module.ZOrder
{

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseOneForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public CloseOneForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopOneForLeakRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForTmlRightMenuId;
            Description = "线路分闸部件，执行线路1分闸，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopOneForLeakRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForTmlRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "分闸_线路1";
            this.Text = "分闸_线路1";
            this.Tooltips = "分闸_线路1";
            OpenClose = 2;
            LeakLineId.Add(1);
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseTwoForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public CloseTwoForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopTwoForLeakRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForTmlRightMenuId;
            Description = "线路分闸部件，执行线路2分闸，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopTwoForLeakRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopTwoForTmlRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "分闸_线路2";
            this.Text = "分闸_线路2";
            this.Tooltips = "分闸_线路2";
            OpenClose = 2;
            LeakLineId.Add(2);
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseThreeForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public CloseThreeForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopThreeForLeakRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopThreeForTmlRightMenuId;
            Description = "线路分闸部件，执行线路3分闸，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopThreeForLeakRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.CloseLightLoopThreeForTmlRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "分闸_线路3";
            this.Text = "分闸_线路3";
            this.Tooltips = "分闸_线路3";
            OpenClose = 2;
            LeakLineId.Add(3);
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseFourForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public CloseFourForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopFourForLeakRightMenuId;
            Description = "线路分闸部件，执行线路4分闸，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopFourForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "分闸_线路4";
            this.Text = "分闸_线路4";
            this.Tooltips = "分闸_线路4";
            OpenClose =2;
            LeakLineId.Add(4);
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseFiveForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public CloseFiveForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopFiveForLeakRightMenuId;
            Description = "线路分闸部件，执行线路5分闸，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopFiveForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "分闸_线路5";
            this.Text = "分闸_线路5";
            this.Tooltips = "分闸_线路5";
            OpenClose = 2;
            LeakLineId.Add(5);
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseSixForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public CloseSixForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopSixForLeakRightMenuId;
            Description = "线路分闸部件，执行线路6分闸，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopSixForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "分闸_线路6";
            this.Text = "分闸_线路6";
            this.Tooltips = "分闸_线路6";
            OpenClose = 2;
            LeakLineId.Add(6);
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseSevenForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public CloseSevenForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopSevenForLeakRightMenuId;
            Description = "线路分闸部件，执行线路7分闸，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopSevenForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "分闸_线路7";
            this.Text = "分闸_线路7";
            this.Tooltips = "分闸_线路7";
            OpenClose = 2;
            LeakLineId.Add(7);
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseEightForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public CloseEightForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopEightForLeakRightMenuId;
            Description = "线路分闸部件，执行线路8分闸，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopEightForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "分闸_线路8";
            this.Text = "分闸_线路8";
            this.Tooltips = "分闸_线路8";
            OpenClose = 2;
            LeakLineId.Add(8);
        }

        
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class CloseAllForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public CloseAllForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuCloseLightLoopAllForLeakRightMenuId;
            Description = "线路分闸部件，执行所有线路分闸，ID为" + Services.MenuIdAssgin.MenuCloseLightLoopAllForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "分闸_所有线路";
            this.Text = "分闸_所有线路";
            this.Tooltips = "分闸_所有线路";
            OpenClose = 2;
            LeakLineId.Add(10);
        }


    };

}
