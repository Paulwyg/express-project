using System.Threading;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.Cr.Core.CoreServices;
using Wlst.Cr.Core.ModuleServices;
using Wlst.Ux.EquipemntTree.SettingViewModel.Services;
using Setting = Wlst.Sr.EquipmentInfoHolding.Services.UxTreeSetting;

namespace Wlst.Ux.EquipemntTree
{
    [ModuleExport(typeof(UxEquipemntTree), DependsOnModuleNames = new string[] { "SrEquipmentInfoHolding" })]
    public class UxEquipemntTree : IModule
    {
        #region IModule 成员



        public void Initialize()
        {
            //throw new NotImplementedException();

             
            //new LocalProtocol().RegistProtocol();
            //new GrpMultiInfoHoldingExtend().InitLoad();
            //new GrpSingleInfoHoldingExtend().InitLoad();


            //Thread thread;
            //thread = new Thread(LoadAfterOneSecond);
            //thread.Start();
            //View i36N id 11020000~11029999

            if (SettingExtend.Myself .IsShowMulTreeOnTabOrNot ) Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddMulityTab, 0);
            if (SettingExtend.Myself.IsShowSingleTreeOnTabOrNot  ) Wlst.Cr.Core.ModuleServices.DelayEvent.RegisterDelayEvent(AddSingleTab, 0);
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

        void AddMulityTab()
        {
            RegionManage.ShowViewByIdAttachRegion(
              Services.ViewIdAssign.GrpMulityTabShowViewId,
          
                //Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleMulTreeViewId,
                //Infrastructure.IdAssign.ViewIdNameAssign.TreeModuleMulTreeViewAttachRegion,
            true
              );
        }
        void AddSingleTab()
        {
            RegionManage.ShowViewByIdAttachRegion(
               Services.ViewIdAssign.GrpSingleTabShowViewId,
              
                //Infrastructure.IdAssign.ViewIdNameAssign.GrpShowModuleGrpSingleTreeViewId,
                //Infrastructure.IdAssign.ViewIdNameAssign.GrpShowModuleGrpSingleTreeViewAttachReigon,
               true
               );
        }


        #endregion

    }
}
