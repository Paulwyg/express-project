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

namespace Wlst.Ux.EquipmentInfo.EquipmentInfo.ElectricMeterInfoViewModel.Views
{
    /// <summary>
    /// ElectricMeterInfoView.xaml 的交互逻辑
    /// </summary>
    public partial class ElectricMeterInfoView : UserControl
    {
        public ElectricMeterInfoView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Wlst.Cr.CoreMims.ReportExcel.ExcelExport.ExcelExportWriteByRadGridView(rgv);
        }
    }
}
