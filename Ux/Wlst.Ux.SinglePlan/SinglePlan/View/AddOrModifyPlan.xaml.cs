using System;
using System.Collections.Generic;
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
using Wlst.Ux.SinglePlan.SinglePlan.ViewModel;

namespace Wlst.Ux.SinglePlan.SinglePlan.View
{
    /// <summary>
    /// AddOrModifyPlan.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrModifyPlan : CustomChromeWindow
    {
        public AddOrModifyPlan()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "新增或修改方案";
        }

        public void SetContext(SluOnePlan oit)
        {
            DataContext = oit;
        }
    }
}
