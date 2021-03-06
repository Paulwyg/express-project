﻿using System.ComponentModel.Composition;
using System.Windows.Controls;
using Wlst.Cr.Core.Behavior;
using Wlst.Ux.WJ3005Module.RtuLdlSet.Services;

namespace Wlst.Ux.WJ3005Module.RtuLdlSet.Views
{
    /// <summary>
    /// LdlSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(WJ3005Module.Services.ViewIdAssign.NavToLdlViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class LdlSetView : UserControl
    {
        public LdlSetView()
        {
            InitializeComponent();
        }


        [Import]
        private IILdlSetView Model
        {
            get { return DataContext as IILdlSetView; }
            set { DataContext = value; }
        }
    }
}
