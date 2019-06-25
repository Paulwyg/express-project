﻿using System;
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
using Wlst.Cr.CoreOne.Services;
using Wlst.Ux.Wj1090Module.LduTreeSettingViewModel.Services;
using Wlst.Ux.Wj1090Module.NewData.Services;

namespace Wlst.Ux.Wj1090Module.NewData.View
{
    /// <summary>
    /// NewData.xaml 的交互逻辑
    /// </summary>
    [ViewExport(
    Wj1090Module.Services.ViewIdAssign.NewDataViewId,
        AttachRegion =RegionNames.DataRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class NewData : UserControl
    {
        public NewData()
        {
            InitializeComponent();
        }

        [Import]
        public IINewData Model
        {
            get { return DataContext as IINewData; }
            set { DataContext = value; }
        }
    }
}
