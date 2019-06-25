using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Services;

namespace Wlst.Ux.EquipemntLightFault.CurrentEquipmentFaultViewModel.Views
{
    /// <summary>
    /// CurrentEquipmentFaultViewForWin.xaml 的交互逻辑
    /// </summary>
    public partial class CurrentEquipmentFaultViewForWin : Window
    {
       // private IICurrentEquipmentFaultView Model;
        public CurrentEquipmentFaultViewForWin()
        {
            InitializeComponent();

            this .DataContext  = new CurrentEquipmentFaultViewModel.ViewModel .CurrentEquipmentFaultViewModel();
            this .WindowStartupLocation=WindowStartupLocation.CenterScreen;
        }
        //[Import]
        //public IICurrentEquipmentFaultView Model
        //{
        //    get { return DataContext as IICurrentEquipmentFaultView; }
        //    set { DataContext = value; }
        //}
        protected override void OnClosed(EventArgs e)
        {
            this.Hide();
           
            //base.OnClosed(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
            //base.OnClosing(e);
        }

       
    }
}
