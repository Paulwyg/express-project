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

namespace Wlst.Ux.Wj2096Module.TimeInfoQuery
{
    /// <summary>
    /// TimeInfoQueryView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj2096Module.Services.ViewIdAssign.TimeInfoQueryViewId )]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class TimeInfoQueryView : UserControl
    {
        public TimeInfoQueryView()
        {
            InitializeComponent();
        }

        [Import]
        public IITimeInfoQuery Model
        {
            get { return DataContext as IITimeInfoQuery; }
            set
            {
                DataContext = value;
            }
        }
    }
}
