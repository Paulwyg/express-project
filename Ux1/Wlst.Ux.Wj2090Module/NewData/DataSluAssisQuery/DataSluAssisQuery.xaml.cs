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
using Wlst.Ux.Wj2090Module.Wj2090InfoSet.Services;
using System.ComponentModel.Composition;

namespace Wlst.Ux.Wj2090Module.NewData.DataSluAssisQuery
{
    /// <summary>
    /// DataSluAssisQuery.xaml 的交互逻辑
    /// </summary>
    [ViewExport(Wj2090Module.Services.ViewIdAssign.DataSluAssisQuery)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class DataSluAssisQuery : UserControl
    {
        public DataSluAssisQuery()
        {
            InitializeComponent();
        }

        [Import]
        public IIDataSluAssisQuery Model
        {
            get { return DataContext as IIDataSluAssisQuery; }
            set { DataContext = value; }
        }
    }



}
