using Wlst.client;

namespace Wlst.Sr.EquipmentInfoHolding.Model
{

    public class GroupInformation : GroupItemsInfo.GroupItem
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index;

        public GroupInformation(GroupItemsInfo.GroupItem item)
        {
            //如果为本地分组  则区域地址为  -1
            this.AreaId = item.AreaId;
            this.GroupId = item.GroupId;
            this.GroupName = item.GroupName;
            this.LstTml = item.LstTml;

        }
    }


}
