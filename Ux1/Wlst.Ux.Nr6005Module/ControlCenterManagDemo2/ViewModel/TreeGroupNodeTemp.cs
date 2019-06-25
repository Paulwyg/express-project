using System.Collections.Generic;
using System.Windows;
using Wlst.Sr.EquipmentInfoHolding.Model;
using Wlst.Ux.Nr6005Module.ControlCenterManagDemo2.Services;

namespace Wlst.Ux.Nr6005Module.ControlCenterManagDemo2.ViewModel
{


    public class TreeGroupNodeTemp : TreeNodeBase
    {
        //public TreeGroupNodeLoacl()
        //{
        //    IsGroup = true;
        //    IsShowSelectedCheckBox = false;
        //}

        public TreeGroupNodeTemp(List<int> rtus)
        {
            AreaId = 0;

            AreaName = "临时操作";

            NodeName = "临时操作";
            

            Visi = Visibility.Visible;
            //_father = mvvmfather;
            IsGroup = true;
            IsShowSelectedCheckBox = false;
            NodeId = 0;
            PhysicalId = 0;
            IsExpanded = true;

            //加载终端节点
            var ntssss = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(rtus);

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
        public TreeGroupNodeTemp(List<int> rtus,string grpName)
        {
            AreaId = 0;

            AreaName = grpName;

            NodeName = grpName;


            Visi = Visibility.Visible;
            //_father = mvvmfather;
            IsGroup = true;
            IsShowSelectedCheckBox = false;
            NodeId =0;
            PhysicalId = 0;
            IsExpanded = true;

            //加载终端节点
            var ntssss = Sr.EquipmentInfoHolding.Services.ServicesGrpSingleInfoHold.GetRtuOrGrpIndex(rtus);

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
