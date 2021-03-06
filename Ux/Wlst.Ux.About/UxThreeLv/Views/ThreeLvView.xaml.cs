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
using Wlst.Ux.About.UxThreeLv.Services;

namespace Wlst.Ux.About.UxThreeLv.Views
{
    /// <summary>
    /// ThreeLvView.xaml 的交互逻辑
    /// </summary>
    /// 
    [ViewExport(Wlst.Ux.About.Services.ViewIdAssign.UxThreeLvViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ThreeLvView : UserControl
    {
        public ThreeLvView()
        {
            InitializeComponent();
       }

        [Import]
        private IIUxThreeLvModule Model
        {
            get { return DataContext as IIUxThreeLvModule; }
            set
            {
                DataContext = value;
            }
        }
    }
}
