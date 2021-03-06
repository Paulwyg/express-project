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
using Wlst.Ux.EquipmentDataQuery.RecordEventQueryViewModel.Services;

namespace Wlst.Ux.EquipmentDataQuery.RecordEventQueryViewModel.View
{
    /// <summary>
    /// RecordEventQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport(AttachNow = false, ID = EquipmentDataQuery.Services.ViewIdAssign.RecordEventQueryViewId,
         AttachRegion = EquipmentDataQuery.Services.ViewIdAssign.RecordEventQueryViewAttachRegion)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class RecordEventQueryView : UserControl
    {
        public RecordEventQueryView()
        {
            InitializeComponent();
        }

        [Import]
        public IIRecordEventQueryViewModel Model
        {
            get { return DataContext as IIRecordEventQueryViewModel; }
            set { DataContext = value; }
        }
    }
}
