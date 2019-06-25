using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wlst.Sr.EquipmentInfoHolding.Model
{
    public class SelectedInfo
    {
        public int AreaId;
        public int RtuOrGrpId;
        public int LoopOrCtrlId;
        public SelectType SelectedType;
        /// <summary>
        ///  事件来源  1、终端数，2、地图，3、其他查询界面
        /// </summary>
        public int Source;

        public SelectedInfo (int areaId, int rtuOrGrpId,SelectType type)
        {
            AreaId = areaId;
            this.RtuOrGrpId = rtuOrGrpId;
            SelectedType = type;
        }

        public enum SelectType
        {
            /// <summary>
            /// 选择的为设备 如终端、集中器等一级设备
            /// </summary>
            Rtu = 1,

            /// <summary>
            /// 选中的为二级设备
            /// </summary>
            Loop,

            /// <summary>
            /// 选中的为全局分组
            /// </summary>
            SingleGrp,

            /// <summary>
            /// 选中的为本地分组
            /// </summary>
            LocalGrp,
        }

    }
}
