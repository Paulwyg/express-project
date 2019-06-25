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
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuHolidaySetViewModel.Services;

namespace Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuHolidaySetViewModel.Views
{
    /// <summary>
    /// ZhaoCeRtuHolidaySetView.xaml 的交互逻辑
    /// </summary>

    [ViewExport(Nr6005Module.Services.ViewIdAssign.ZhaoCeRtuHolidaySetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZhaoCeRtuHolidaySetView : UserControl
    {
        public ZhaoCeRtuHolidaySetView()
        {
            InitializeComponent();
        }

        [Import]
        public IIZhaoCeRtuHolidaySetViewModel Model
        {
            get { return DataContext as IIZhaoCeRtuHolidaySetViewModel; }
            set { DataContext = value; }
        }
    }
}
