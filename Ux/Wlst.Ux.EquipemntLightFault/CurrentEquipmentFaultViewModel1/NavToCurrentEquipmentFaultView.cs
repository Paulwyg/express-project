using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Views;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel
{


    [Export(typeof(IIMenuItem))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NavToCurrentEquipmentFaultView : MenuItemBase
    {
        public NavToCurrentEquipmentFaultView()
        {
            Id = EquipemntLightFault.Services.MenuIdAssgin.NavToCurrentEquipmentFaultViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Text = "当前故障";
            Tag = "当前故障";
            Classic = "主菜单";
            Description = "当前故障查看，ID 为" + EquipemntLightFault.Services.MenuIdAssgin.NavToCurrentEquipmentFaultViewId;
            //Infrastructure.IdAssign.MenuIdAssign.NavToEquipmentFaultRecordQueryViewId;
            Tooltips = "当前故障";
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

        public static CurrentEquipmentFaultViewForWin FaultWindow = null;

        protected void Ex()
        {
            if (FaultWindow==null)
            {
                FaultWindow=new CurrentEquipmentFaultViewForWin();
            }

            FaultWindow.Visibility = Visibility.Visible;
            FaultWindow.Title = "当前故障";
            //FaultWindow.Icon = null;
            FaultWindow.Show();

            //ExNavWithArgs(
            //    //ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewAttachRegion,
            //    //               ViewIdNameAssign.EquipemntLightFaultEquipmentFaultRecordQueryViewId,

            //    EquipemntLightFault.Services.ViewIdAssign.CurrentEquipmentFaultViewId,
            //                   "");
            // MessageBoxResult result = MessageBox.Show("Sorry for have't finish this function ......", "Sorry", MessageBoxButton.OK);
        }


    }
}
