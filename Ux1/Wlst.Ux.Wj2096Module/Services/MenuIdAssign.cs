using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Ux.Wj2096Module.Services
{
    public class MenuIdAssign
    {
        /// <summary>
        /// 本模块的菜单起始Id，2100000 + 75*100, 每个模块均发放100个Id值。
        /// </summary>
        public const int MenuIdAssignBaseId = 2100000 + 75 * 100;

        public const int NavToWj2096TreeSetMenuId = MenuIdAssignBaseId + 1;

        public const int NavToWj2096SluInfoSetMenuId = MenuIdAssignBaseId + 2;

        public const int NavToWeekSetQueryMenuId = MenuIdAssignBaseId + 3;

        public const int NavToFieldGroupSetMenuId = MenuIdAssignBaseId + 4;

        public const int NavToFieldCtrlGroupSetMenuId = MenuIdAssignBaseId + 5;



        public const int MeasureControllerForMenuId = MenuIdAssignBaseId + 6;

        public const int MeasureControllerCtrlForMenuId = MenuIdAssignBaseId + 7;



        public const int OpenCloseChangeLightCtrlMenuId = MenuIdAssignBaseId + 30;

        public const int OpenCloseChangeLightMenuId = MenuIdAssignBaseId + 50;

        public const int NavToTimeInfoSetMenuId = MenuIdAssignBaseId + 8;

        public const int TimeInfoQueryMenuId = MenuIdAssignBaseId + 9;

        public const int ZcConnLocalArgsMenuId = MenuIdAssignBaseId + 10;


        public const int MeasureControllerCtrlRunArgsForMenuId = MenuIdAssignBaseId + 11;

        public const int NavToCtrlDataQueryId = MenuIdAssignBaseId + 12;
        
    }
}
