﻿namespace Wlst.Ux.CrissCrossEquipemntTree.Models
{
    public  enum TypeOfTabTreeNode
    {
        /// <summary>
        /// Is  group or  not
        /// </summary>
        IsGrp,
        /// <summary>
        /// Is  terminal or not
        /// </summary>
        IsTml,
        /// <summary>
        /// Is the part of  one  terminal
        /// </summary>
        IsTmlParts,
        /// <summary>
        /// Is SPECIAL  group
        /// </summary>
        IsGrpSpecial,
        /// <summary>
        /// 所有终端
        /// </summary>
        IsAll,
        /// <summary>
        /// area
        /// </summary>
        IsArea,
        /// <summary>
        /// 地区分组
        /// </summary>
        IsRegion,
        /// <summary>
        /// 地区特殊分组
        /// </summary>
        IsRegionSpecial,
    }
}
