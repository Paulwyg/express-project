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
using Wlst.Ux.Wj1090Module.Wj1090DataSelection.Services;

namespace Wlst.Ux.Wj1090Module.Wj1090DataSelection.View
{
    /// <summary>
    /// Wj1090DataSelectionView.xaml 的交互逻辑
    /// </summary>
    [ViewExport( Wj1090Module.Services.ViewIdAssign.Wj1090DataSelectionViewId)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class Wj1090DataSelectionView : UserControl
    {
        public Wj1090DataSelectionView()
        {
            InitializeComponent();
        }

        [Import]
        public IIWj1090DataSelection Model
        {
            get { return DataContext as IIWj1090DataSelection; }
            set { DataContext = value; }
        }

    }
}
