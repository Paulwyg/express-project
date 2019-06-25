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
using Wlst.Ux.TimeTableSystem.HolidayTimeSet.Services;

namespace Wlst.Ux.TimeTableSystem.HolidayTimeSet.Views
{
    /// <summary>
    /// HolidayTimeSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( TimeTableSystem.Services.ViewIdAssign.HolidayTimeSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class HolidayTimeSetView : UserControl
    {
        public HolidayTimeSetView()
        {
            InitializeComponent();
        }


        [Import]
        public IIHolidayTimeSet Model
        {
            get { return DataContext as IIHolidayTimeSet; }
            set { DataContext = value; }
        }

    }
}
