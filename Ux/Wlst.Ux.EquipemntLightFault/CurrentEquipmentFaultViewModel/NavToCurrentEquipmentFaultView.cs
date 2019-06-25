using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Wlst.Cr.CoreMims.Commands;
using Wlst.Cr.CoreOne.CoreInterface;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Views;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel
{


    [Export(typeof (IIMenuItem))]
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
            //Initializzze();
        }

        public override bool IsCanBeShowRwx()
        {
            //return Wlst.Cr.CoreMims.Services.UserInfo.CanR();
            return true;
        }

        private static bool CanEx()
        {
            return true;
        }

        public static CurrentEquipmentFaultViewForWin FaultWindow = null;


        public static void InitWin()
        {
            if (FaultWindow == null)
            {
                Initializzze();
                //FaultWindow = new CurrentEquipmentFaultViewForWin();
                //FaultWindow.Visibility = Visibility.Collapsed;
                //FaultWindow.Title = "当前故障";
            }


            //FaultWindow.Icon = null;
            //FaultWindow.Show();
            //FaultWindow.Focus(); 
        }


        private delegate void CmbdelegateMbl();
        public static void ShowView()
        {
            //if (_dispatcher == null) return;


            //_dispatcher.Invoke(
            //    System.Windows.Threading.DispatcherPriority.DataBind,
            //    new CmbdelegateMbl(ExShow));
            ExShow();

        }

        protected void Ex()
        {
            //if (_dispatcher == null) return;

            //_dispatcher.Invoke(
            //    System.Windows.Threading.DispatcherPriority.DataBind,
            //    new CmbdelegateMbl(Ex1));

            Ex1();
        }

        static void Ex1( )
        {
            if (FaultWindow == null)
            {
                Initializzze();
            }
            FaultWindow.Visibility = Visibility.Visible;
            FaultWindow.Show();
            FaultWindow.Focus();
            FaultWindow.WindowState = WindowState.Normal;
            FaultWindow.BringIntoView();

        }

        static void ExShow()
        {
            if (FaultWindow == null) return;

            
            
            FaultWindow.Visibility = Visibility.Visible;
            FaultWindow.Show();
            FaultWindow.Focus();

            if ( FaultWindow.WindowState == WindowState.Minimized)
            {
                FaultWindow.WindowState = WindowState.Normal;

                FaultWindow.BringIntoView();
            }
           
        }


        public static  void Initializzze()
        {
            ThreadStartingPoint();
            return;
            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

      //  public static Dispatcher _dispatcher = null;
        private static void ThreadStartingPoint()
        {
             
            FaultWindow = new CurrentEquipmentFaultViewForWin();
         //   _dispatcher = Dispatcher.CurrentDispatcher; ;// FaultWindow.Dispatcher;
            FaultWindow.Visibility = Visibility.Collapsed;
            FaultWindow.Title = "当前故障";

         
            //FaultWindow.Show();
            //FaultWindow.Focus();
        //    System.Windows.Threading.Dispatcher.Run();
            //Elysium.Manager.Apply(Application.Current, Elysium.Theme.Light);
        }

    }
}
