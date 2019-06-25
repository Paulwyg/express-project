using System.ComponentModel.Composition;
using System.Windows;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Views;
using Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLevelLowViewModel.Views;

namespace Wlst.Ux.EquipemntLightFault.EquipmentFaultRecordQueryLevelLowViewModel
{

    [Export(typeof (IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToEquipmentFaultRecordQueryViewHigh : MenuItemBase
    {
        public NavToEquipmentFaultRecordQueryViewHigh()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryLevelHighViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Text = "紧急故障查询";
            Tag = "紧急故障查询";
            Classic = "主菜单";
            Description = "终端忽略故障查询，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToEquipmentFaultRecordQueryLevelHighViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Tooltips = "紧急故障查询";
            IsEnabled = true;
            IsCheckable = false;
            base.Command = new RelayCommand(Ex, CanEx, true);
        }
        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
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
            if (FalutQueryWindow == null)
            {
                FalutQueryWindow = new EquipmentFaultRecordQueryView();
                
            }
            FalutQueryWindow.SetLevel(13);
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
        //    ExNavWithArgs(
        //        //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewAttachRegion,
        //        //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewId,

        //        EquipemntLightFault.Services.ViewIdAssign.EquipmentFaultRecordQueryLevelLowViewId,
        //                       0);



        //    // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        //}


    }
}
