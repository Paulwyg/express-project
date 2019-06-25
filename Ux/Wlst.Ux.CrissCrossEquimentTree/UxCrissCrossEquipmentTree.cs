using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.CoreServices;
using Wlst.Ux.CrissCrossEquipemntTree.SettingViewModel.Services;

namespace Wlst.Ux.CrissCrossEquimentTree
{
    [ModuleExport(typeof(UxCrissCrossEquipmentTree), DependsOnModuleNames = new string[] { "SrEquipmentInfoHolding" })]
    public class UxCrissCrossEquipmentTree : IModule
    {
                #region IModule 成员



        public void Initialize()
        {

            // if (Wlst.Ux.CrissCrossEquipemntTree.SettingViewModel.SettingExtend.Myself.IsShowSingleTreeOnTabOrNot  ) Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddSingleTab, 0);
            if (Wlst.Ux.CrissCrossEquipemntTree.SettingViewModel.Services.SettingExtend.Myself.IsShowSingleTreeOnTabOrNot)
            {
                Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddSingleTab, 0);
            }

        }



        //private void LoadAfterOneSecond()
        //{
        //    Thread.Sleep(1000);
        //    RegionManage.ShowViewByIdAttachRegion(
        //        Services .ViewIdAssign .GrpMulityTabShowViewId ,
        //        Services .ViewIdAssign .GrpMulityTabShowViewAttachRegion ,
        //        //Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleMulTreeViewId,
        //        //Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleMulTreeViewAttachRegion,
        //        Setting .IsShowMulTreeOnTab 
        //        );
        //    RegionManage.ShowViewByIdAttachRegion(
        //        Services .ViewIdAssign .GrpSingleTabShowViewId ,
        //        Services .ViewIdAssign .GrpSingleTabShowViewAttachRegion ,
        //        //Infrastructure.IdAssign.ViewIdNameAssign.GrpShowModuleGrpSingleTreeViewId,
        //        //Infrastructure.IdAssign.ViewIdNameAssign.GrpShowModuleGrpSingleTreeViewAttachReigon,
        //        Setting .IsShowSingleTreeOnTab 
        //        );
        //}

        void AddSingleTab()
        {

            RegionManage.ShowViewByIdAttachRegion(
                   CrissCrossEquipemntTree.Services.ViewIdAssign.GrpSingleTabShowViewId,
                   true);

            RegionManage.ShowViewByIdAttachRegion(
                   CrissCrossEquipemntTree.Services.ViewIdAssign.GrpRegionTabShowViewId,
                   true);
        }


        #endregion



    }
}
