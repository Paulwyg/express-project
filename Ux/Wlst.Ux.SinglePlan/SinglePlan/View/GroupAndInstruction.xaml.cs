using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WindowForWlst;
using Wlst.Cr.CoreOne.Models;
using Wlst.Ux.SinglePlan.SinglePlan.ViewModel;

namespace Wlst.Ux.SinglePlan.SinglePlan.View
{
    /// <summary>
    /// GroupAndInstruction.xaml 的交互逻辑
    /// </summary>
    public partial class GroupAndInstruction : CustomChromeWindow
    {
        public GroupAndInstruction()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "绑定分组和指令";
        }

        SluOneGroup   tvx=null ;
        public void SetContext(SluOneGroup oit)
        {
            tvx = oit;
            DataContext = oit;
        }


        public Action<SluOneGroup> OnBtnOk;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(OnBtnOk !=null )
            {
                OnBtnOk(tvx);
            }
            Close();
        }


    }
}
