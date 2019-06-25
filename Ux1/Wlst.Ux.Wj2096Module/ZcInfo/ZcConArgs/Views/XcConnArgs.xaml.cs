﻿using System;
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
using Wlst.Cr.Core.Behavior;
using System.ComponentModel.Composition;
using Wlst.Ux.Wj2096Module.ZcInfo.ZcConArgs.Services;

namespace Wlst.Ux.Wj2096Module.ZcInfo.ZcConArgs.Views
{
    /// <summary>
    /// XcConnArgs.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj2096Module.Services.ViewIdAssign.ZcConnArgsViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class XcConnArgs : UserControl
    {
        public XcConnArgs()
        {
            InitializeComponent();
        }

        [Import]
        public IIXcConnArgs Model
        {
            get { return DataContext as IIXcConnArgs; }
            set { DataContext = value; }
        }
    }
}
