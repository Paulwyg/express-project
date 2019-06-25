using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLevelLowViewModel.Views;


namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLevelLowViewModel
{
   
    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentFaultRecordQueryViewTmlRightMenuHigh : MenuItemBase
    {
        public NavToEquipmentFaultRecordQueryViewTmlRightMenuHigh()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryViewTmlRightLevelHighMenuId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Text = "紧急故障查询";
            Tag = "终端紧急故障查询";
            Classic = "终端右键菜单";
            Description = "终端紧急故障查询，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryViewTmlRightLevelHighMenuId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Tooltips = "终端紧急故障查询";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            if (Wlst.Cr.CoreMims.Services.UserInfo.UserLoginInfo.D) return true;
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
            if (equipment == null || equipment.RtuStateCode  == 0) return false;
            var areaId = Sr.EquipmentInfoHolding.Services.AreaInfoHold.MySlef.GetRtuBelongArea(equipment.RtuId);
            return Wlst.Cr.CoreMims.Services.UserInfo.CanR(areaId );
        }
        static bool CanEx()
        {
            return true;
        }



        public static EquipmentFaultRecordQueryView FalutQueryWindow = null;
        public static void InitWin()
        {
            if (FalutQueryWindow == null)
            {
                FalutQueryWindow = new EquipmentFaultRecordQueryView();
                FalutQueryWindow.Visibility = Visibility.Collapsed;
                FalutQueryWindow.Title = "紧急故障查询";
                FalutQueryWindow.TitleCetc = "紧急故障查询";
            }


            //FaultWindow.Icon = null;
            //FaultWindow.Show();
            //FaultWindow.Focus(); 
        }
        public static void ShowView()
        {
            if (FalutQueryWindow == null) return;
            FalutQueryWindow.Visibility = Visibility.Visible;
            FalutQueryWindow.Show();
            FalutQueryWindow.Focus();
            FalutQueryWindow.WindowState = WindowState.Normal; ;
            FalutQueryWindow.BringIntoView();
            FalutQueryWindow.Title = "紧急故障查询";
            FalutQueryWindow.TitleCetc = "紧急故障查询";
        }
        protected void Ex()
        {

            var equipment = this.Argu as Wlst.Sr.EquipmentInfoHolding.Model.WjParaBase;
            if (equipment == null)
                return;
            int rtuId = equipment.RtuId;
            if (rtuId < 1) return;

            if (FalutQueryWindow == null)
            {
                FalutQueryWindow = new EquipmentFaultRecordQueryView();

            }
            FalutQueryWindow.SetLevel(13);
            FalutQueryWindow.SetSelectRtu(rtuId);
            FalutQueryWindow.Visibility = Visibility.Visible;
            FalutQueryWindow.Title = "紧急故障查询";
            FalutQueryWindow.TitleCetc = "紧急故障查询";
            FalutQueryWindow.NavOnlOAD();
            //FaultWindow.Icon = null;
            FalutQueryWindow.Show();
            FalutQueryWindow.Focus();
            //ExNavWithArgs(
            //    //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewAttachRegion,
            //    //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewId,

            //    EquipemntLightFault.Services.ViewIdAssign.CurrentEquipmentFaultViewId,
            //                   "");
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }







        //protected void Ex()
        //{

        //    var equipment = this.Argu as Wlst.Sr .EquipmentInfoHolding .Model .WjParaBase ;
        //    if (equipment == null)
        //        return;
        //    int rtuId = equipment.RtuId;
        //    if (rtuId < 1) return;
        //    var tmplst = Wlst.Sr.EquipmentInfoHolding.Services.Others.TmlTreeChked;
        //    bool multQuery = false;
        //    if ( tmplst.Count>1)
        //    {
        //        if (tmplst.Contains(rtuId))
        //        {
        //            multQuery = true;
        //            //    ExNavWithArgs(
        //            //EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryViewId,
        //            //               tmplst);
        //        }
        //    }

        //    if (multQuery)
        //    {
        //         ExNavWithArgs(
        //            EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryLevelLowViewId,
        //                           tmplst);
        //    }
        //    else
        //    {
        //          ExNavWithArgs(
        //        //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewAttachRegion,
        //        //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewId,
                
        //        EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryLevelLowViewId,
        //                       rtuId);
        //    // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        //    }

          
        //}





    }
}
