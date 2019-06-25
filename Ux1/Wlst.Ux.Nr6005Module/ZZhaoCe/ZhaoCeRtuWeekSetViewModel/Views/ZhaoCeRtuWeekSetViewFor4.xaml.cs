using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.Services;


namespace Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.Views
{
    /// <summary>
    /// ZhaoCeRtuWeekSetViewFor4.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Nr6005Module.Services.ViewIdAssign.ZhaoCeRtuWeekSetViewIdFor4)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZhaoCeRtuWeekSetViewFor4 : UserControl
    {
        public ZhaoCeRtuWeekSetViewFor4()
        {
            InitializeComponent();
        }

        [Import]
        public IIZhaoCeRtuWeekSetViewModel Model
        {
            get { return DataContext as IIZhaoCeRtuWeekSetViewModel; }
            set { DataContext = value; }
        }
    }
}
