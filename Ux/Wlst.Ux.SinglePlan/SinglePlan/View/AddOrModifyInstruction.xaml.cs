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
    /// AddOrModifyInstruction.xaml 的交互逻辑
    /// </summary>
    public partial class AddOrModifyInstruction : CustomChromeWindow
    {
        public AddOrModifyInstruction()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Title = "新增或修改指令";
        }

        private SluOneInstruction tvx;
        public void SetContext(SluOneInstruction oit)
        {
            tvx = oit;
            DataContext = oit;
        }

    }

}
