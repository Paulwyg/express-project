using System.Windows;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.Services;

namespace Wlst.Ux.WJ3005Module.ControlCenterManagDemo2.ViewModel
{

    public class TreeGroupNodeLoacl : TreeNodeBase
    {
        //public TreeGroupNodeLoacl()
        //{
        //    IsGroup = true;
        //    IsShowSelectedCheckBox = false;
        //}

        public TreeGroupNodeLoacl(GroupInformation grp)
        {
            AreaId = grp.AreaId;
            if (Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo.ContainsKey(AreaId))
            {
                AreaName = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.AreaInfo[AreaId].AreaName;
            }
            else
            {
                if (AreaId == -1)
                    AreaName = "全部区域";
                else
                    AreaName = grp.AreaId + " ";
            }
          
            NodeName = grp.GroupId  + "-" + grp.GroupName;


            Visi = Visibility.Visible;
            //_father = mvvmfather;
            IsGroup = true;
            IsShowSelectedCheckBox = false;
            NodeId = grp.GroupId;
            PhysicalId = grp.GroupId;


            //加载终端节点
            var ntssss = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(grp.LstTml);

            foreach (
                var t in ntssss)
            {
                if (!Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems.ContainsKey(t))
                    continue;
                var f = Sr.EquipmentInfoHolding.Services.EquipmentDataInfoHold.InfoItems[t];
                if (f == null || f.EquipmentType != WjParaBase.EquType.Rtu) continue;

                 
                    ChildTreeItems.Add(new TreeTmlNode(this, f.RtuId));
            }
        }





    }
}
