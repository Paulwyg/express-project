using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Wlst.client;

namespace Wlst.Sr.SlusglInfoHold
{
    [ModuleExport(typeof (SrSlusglInfoHold))]
    public class SrSlusglInfoHold : IModule
    {
        #region IModule 成员

        public void Initialize()
        {
            Services.SluSglInfoHold.MySlef.OnInit();
            //Services.SluSglFieldHold.MySlef.OnInit();
            Services.SluSglFieldGrpHold.MySlef.OnInit();
            Services.TimeInfos.MySelf.OnInit();
            Services.SluSglCtrlDataHold.MySlef.OnInit();
            //Services.EquipmentDataInfoHold.MySlef.InitStart();
            //AreaInfoHold.MySlef.InitStartService();
            //Services.ServicesGrpSingleInfoHold.InitLoad();
            //ServicesGrpMulitInfoHold.MySlef.InitSvr();
            //ServicesGrpMultiInfoHoldNew.InitLoad();
            //Services.RunningInfoHold.InitServices();
            //Services.RtuSxxRuleInstancesHold.Myself.InitLoad();

            return;

            var tmp = new GrpFieldSluSglCtrl.GrpFieldSluSglItem()
                          {CtrlLst = new List<int>(), FieldId = 10, GrpName = "x1", GrpId = 1,Order = 1};
            for (int i = 8000001; i < 8000021; i++) tmp.CtrlLst.Add(i);
            Services.SluSglFieldGrpHold.MySlef.Info.TryAdd(new Tuple<int, int>(10, 1), tmp);

       
            var tmpfiled = new EquSluSgl.ParaFieldSluSgl()
                               {
                                   AreaId = 0,
                                   CtrlLst = new List<EquSluSgl.ParaSluCtrl>(),
                                   DtUpdate = DateTime.Now.Ticks,
                                   FieldId = 10,
                                   FieldName = "xr",
                                   PhyId = 10,
                               };

            foreach (var f in tmp.CtrlLst)
            {
                tmpfiled.CtrlLst.Add(new EquSluSgl.ParaSluCtrl()
                {
                    BarCodeId = f,
                    CtrlGisX = 0,
                    CtrlGisY = 0,
                    CtrlId = f,
                    CtrlName = "N" + f,
                    DtUpdate = DateTime.Now.Ticks,
                    IsAutoOpenLightWhenElec2 = false,
                    IsAutoOpenLightWhenElec3 = false,
                    IsAutoOpenLightWhenElec4 = false,
                    IsUsed = true,
                    IsAlarmAuto = true,
                    IsAutoOpenLightWhenElec1 = false,
                    OrderId = 0,

                });

            }

            Services.SluSglInfoHold.MySlef.Info.TryAdd( 10, tmpfiled);

        }



        #endregion
    }

}
