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
    public class OpenOneForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public OpenOneForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopOneForLeakRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForTmlRightMenuId;
            Description = "线路合闸部件，执行线路1合闸，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopOneForLeakRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForTmlRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "合闸_线路1";
            this.Text = "合闸_线路1";
            this.Tooltips = "合闸_线路1";
            OpenClose = 1;
            LeakLineId.Add(1);
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenTwoForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public OpenTwoForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopTwoForLeakRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForTmlRightMenuId;
            Description = "线路合闸部件，执行线路2合闸，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopTwoForLeakRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopTwoForTmlRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "合闸_线路2";
            this.Text = "合闸_线路2";
            this.Tooltips = "合闸_线路2";
            OpenClose = 1;
            LeakLineId.Add(2);
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenThreeForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public OpenThreeForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopThreeForLeakRightMenuId;// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopThreeForTmlRightMenuId;
            Description = "线路合闸部件，执行线路3合闸，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopThreeForLeakRightMenuId +// Infrastructure.IdAssign.MenuIdAssign.OpenLightLoopThreeForTmlRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "合闸_线路3";
            this.Text = "合闸_线路3";
            this.Tooltips = "合闸_线路3";
            OpenClose = 1;
            LeakLineId.Add(3);
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenFourForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public OpenFourForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopFourForLeakRightMenuId;
            Description = "线路合闸部件，执行线路4合闸，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopFourForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "合闸_线路4";
            this.Text = "合闸_线路4";
            this.Tooltips = "合闸_线路4";
            OpenClose = 1;
            LeakLineId.Add(4);
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenFiveForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public OpenFiveForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopFiveForLeakRightMenuId;
            Description = "线路合闸部件，执行线路5合闸，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopFiveForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "合闸_线路5";
            this.Text = "合闸_线路5";
            this.Tooltips = "合闸_线路5";
            OpenClose = 1;
            LeakLineId.Add(5);
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenSixForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public OpenSixForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopSixForLeakRightMenuId;
            Description = "线路合闸部件，执行线路6合闸，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopSixForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "合闸_线路6";
            this.Text = "合闸_线路6";
            this.Tooltips = "合闸_线路6";
            OpenClose = 1;
            LeakLineId.Add(6);
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenSevenForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public OpenSevenForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopSevenForLeakRightMenuId;
            Description = "线路合闸部件，执行线路7合闸，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopSevenForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "合闸_线路7";
            this.Text = "合闸_线路7";
            this.Tooltips = "合闸_线路7";
            OpenClose = 1;
            LeakLineId.Add(7);
        }
    };


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenEightForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public OpenEightForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopEightForLeakRightMenuId;
            Description = "线路合闸部件，执行线路8合闸，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopEightForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "合闸_线路8";
            this.Text = "合闸_线路8";
            this.Tooltips = "合闸_线路8";
            OpenClose = 1;
            LeakLineId.Add(8);
        }
    };

    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class OpenAllForLeakRightMenu : OperateLineControllerForLeakRightMenuBase
    {
        public OpenAllForLeakRightMenu()
        {
            Id = Services.MenuIdAssgin.MenuOpenLightLoopAllForLeakRightMenuId;
            Description = "线路合闸部件，执行所有线路合闸，ID为" + Services.MenuIdAssgin.MenuOpenLightLoopAllForLeakRightMenuId +
                        "，类型为漏电右键菜单。";
            Tag = "合闸_所有线路";
            this.Text = "合闸_所有线路";
            this.Tooltips = "合闸_所有线路";
            OpenClose = 1;
            LeakLineId.Add(10);

        }
    };
}
