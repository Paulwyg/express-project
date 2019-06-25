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

namespace Wlst.Ux.Wj2090Module.TreeTab.Set
{
    /// <summary>
    /// Wj2090TreeSetView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj2090Module.Services.ViewIdAssign.Wj2090TreeSetViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj2090TreeSetView : UserControl
    {
        public Wj2090TreeSetView()
        {
            InitializeComponent();
        }

        [Import]
        public IIWj2090TreeSet Model
        {

            get { return DataContext as IIWj2090TreeSet; }
            set { DataContext = value; }
        }
    }
}
