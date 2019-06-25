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
using Wlst.Ux.ViewInstruction.Services;
using Wlst.Ux.ViewInstruction.ViewInstruction.Services;

namespace Wlst.Ux.ViewInstruction.ViewInstruction.View
{
    /// <summary>
    /// ViewInstructions.xaml 的交互逻辑
    /// </summary>
    [ViewExport( ViewIdAssign.ViewInstructionId,
    AttachRegion =ViewIdAssign.ViewInstructionViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ViewInstructions : UserControl
    {
        public ViewInstructions()
        {
            InitializeComponent();
        }

        [Import]
        public IIViewInstruction Model
        {
            get { return DataContext as IIViewInstruction; }
            set { DataContext = value; }
        }
    }
}
