﻿using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.Services;

namespace Wlst.Ux.Nr6005Module.ZZhaoCe.ZhaoCeRtuWeekSetViewModel.Views
{
    /// <summary>
    /// ZhaoCeRtuWeekSetView.xaml 的交互逻辑
    /// </summary>
    //[ViewExport( Nr6005Module.Services.ViewIdAssign.ZhaoCeRtuWeekSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ZhaoCeRtuWeekSetView : UserControl
    {
        public ZhaoCeRtuWeekSetView()
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
